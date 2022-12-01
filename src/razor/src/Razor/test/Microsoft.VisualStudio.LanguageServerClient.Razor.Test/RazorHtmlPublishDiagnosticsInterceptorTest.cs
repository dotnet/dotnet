// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Logging;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Test;
using Microsoft.VisualStudio.Text;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

public class RazorHtmlPublishDiagnosticsInterceptorTest : TestBase
{
    private static readonly Uri s_razorUri = new("C:/path/to/file.razor");
    private static readonly Uri s_cshtmlUri = new("C:/path/to/file.cshtml");
    private static readonly Uri s_razorVirtualHtmlUri = new("C:/path/to/file.razor__virtual.html");
    private static readonly Uri s_razorVirtualCssUri = new("C:/path/to/file.razor__virtual.css");

    private static readonly Diagnostic s_validDiagnostic_HTML = new()
    {
        Range = new Range()
        {
            Start = new Position(149, 19),
            End = new Position(149, 23)
        },
        Code = null
    };

    private static readonly Diagnostic s_validDiagnostic_CSS = new()
    {
        Range = new Range()
        {
            Start = new Position(150, 19),
            End = new Position(150, 23)
        },
        Code = "expectedSemicolon",
    };

    private static readonly Diagnostic[] s_diagnostics = new Diagnostic[]
    {
        s_validDiagnostic_HTML,
        s_validDiagnostic_CSS
    };

    private readonly RazorLSPConventions _razorLSPConventions;
    private readonly HTMLCSharpLanguageServerLogHubLoggerProvider _loggerProvider;

    public RazorHtmlPublishDiagnosticsInterceptorTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        var logger = new Mock<ILogger>(MockBehavior.Strict).Object;
        Mock.Get(logger).Setup(l => l.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>())).Verifiable();
        _loggerProvider = Mock.Of<HTMLCSharpLanguageServerLogHubLoggerProvider>(l =>
            l.CreateLogger(It.IsAny<string>()) == logger &&
            l.InitializeLoggerAsync(It.IsAny<CancellationToken>()) == Task.CompletedTask,
            MockBehavior.Strict);
        _razorLSPConventions = new RazorLSPConventions(TestLanguageServerFeatureOptions.Instance);
    }

    [Fact]
    public async Task ApplyChangesAsync_InvalidParams_ThrowsException()
    {
        // Arrange
        var documentManager = new TestDocumentManager();
        var diagnosticsProvider = Mock.Of<LSPDiagnosticsTranslator>(MockBehavior.Strict);

        var htmlDiagnosticsInterceptor = new RazorHtmlPublishDiagnosticsInterceptor(documentManager, diagnosticsProvider, _razorLSPConventions, _loggerProvider);
        var diagnosticRequest = new CodeActionParams()
        {
            TextDocument = new TextDocumentIdentifier()
            {
                Uri = s_razorUri
            }
        };

        // Act
        await Assert.ThrowsAnyAsync<ArgumentException>(async () =>
                await htmlDiagnosticsInterceptor.ApplyChangesAsync(
                    JToken.FromObject(diagnosticRequest),
                    containedLanguageName: string.Empty,
                    cancellationToken: default).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [Fact]
    public async Task ApplyChangesAsync_RazorUriNotSupported_ReturnsDefaultResponse()
    {
        // Arrange
        var documentManager = new TestDocumentManager();
        var diagnosticsProvider = Mock.Of<LSPDiagnosticsTranslator>(MockBehavior.Strict);

        var htmlDiagnosticsInterceptor = new RazorHtmlPublishDiagnosticsInterceptor(documentManager, diagnosticsProvider, _razorLSPConventions, _loggerProvider);
        var diagnosticRequest = new PublishDiagnosticParams()
        {
            Diagnostics = s_diagnostics,
            Uri = s_razorUri
        };
        var token = JToken.FromObject(diagnosticRequest);

        // Act
        var result = await htmlDiagnosticsInterceptor.ApplyChangesAsync(token, containedLanguageName: string.Empty, cancellationToken: default).ConfigureAwait(false);

        // Assert
        Assert.Same(token, result.UpdatedToken);
        Assert.False(result.ChangedDocumentUri);
    }

    [Fact]
    public async Task ApplyChangesAsync_CshtmlUriNotSupported_ReturnsDefaultResponse()
    {
        // Arrange
        var documentManager = new TestDocumentManager();
        var diagnosticsProvider = Mock.Of<LSPDiagnosticsTranslator>(MockBehavior.Strict);

        var htmlDiagnosticsInterceptor = new RazorHtmlPublishDiagnosticsInterceptor(documentManager, diagnosticsProvider, _razorLSPConventions, _loggerProvider);
        var diagnosticRequest = new PublishDiagnosticParams()
        {
            Diagnostics = s_diagnostics,
            Uri = s_cshtmlUri
        };
        var token = JToken.FromObject(diagnosticRequest);

        // Act
        var result = await htmlDiagnosticsInterceptor.ApplyChangesAsync(token, containedLanguageName: string.Empty, cancellationToken: default).ConfigureAwait(false);

        // Assert
        Assert.Same(token, result.UpdatedToken);
        Assert.False(result.ChangedDocumentUri);
    }

    [Fact]
    public async Task ApplyChangesAsync_CssUriNotSupported_ReturnsDefaultResponse()
    {
        // Arrange
        var documentManager = new TestDocumentManager();
        var diagnosticsProvider = Mock.Of<LSPDiagnosticsTranslator>(MockBehavior.Strict);

        var htmlDiagnosticsInterceptor = new RazorHtmlPublishDiagnosticsInterceptor(documentManager, diagnosticsProvider, _razorLSPConventions, _loggerProvider);
        var diagnosticRequest = new PublishDiagnosticParams()
        {
            Diagnostics = s_diagnostics,
            Uri = s_razorVirtualCssUri
        };
        var token = JToken.FromObject(diagnosticRequest);

        // Act
        var result = await htmlDiagnosticsInterceptor.ApplyChangesAsync(token, containedLanguageName: string.Empty, cancellationToken: default).ConfigureAwait(false);

        // Assert
        Assert.Same(token, result.UpdatedToken);
        Assert.False(result.ChangedDocumentUri);
    }

    [Fact]
    public async Task ApplyChangesAsync_RazorDocumentNotFound_ReturnsEmptyDiagnosticResponse()
    {
        // Arrange
        var documentManager = new TestDocumentManager();
        var diagnosticsProvider = Mock.Of<LSPDiagnosticsTranslator>(MockBehavior.Strict);

        var htmlDiagnosticsInterceptor = new RazorHtmlPublishDiagnosticsInterceptor(documentManager, diagnosticsProvider, _razorLSPConventions, _loggerProvider);
        var diagnosticRequest = new PublishDiagnosticParams()
        {
            Diagnostics = s_diagnostics,
            Uri = s_razorVirtualHtmlUri
        };

        // Act
        var result = await htmlDiagnosticsInterceptor.ApplyChangesAsync(JToken.FromObject(diagnosticRequest), string.Empty, cancellationToken: default).ConfigureAwait(false);

        // Assert
        var updatedParams = result.UpdatedToken.ToObject<PublishDiagnosticParams>();
        Assert.Empty(updatedParams.Diagnostics);
        Assert.Equal(s_razorUri, updatedParams.Uri);
        Assert.True(result.ChangedDocumentUri);
    }

    [Fact]
    public async Task ApplyChangesAsync_VirtualHtmlDocumentNotFound_ReturnsEmptyDiagnosticResponse()
    {
        // Arrange
        var diagnosticsProvider = Mock.Of<LSPDiagnosticsTranslator>(MockBehavior.Strict);

        var testVirtualDocument = new TestVirtualDocumentSnapshot(s_razorUri, hostDocumentVersion: 0);
        LSPDocumentSnapshot testDocument = new TestLSPDocumentSnapshot(s_razorUri, version: 0, testVirtualDocument);
        var documentManager = new Mock<TrackingLSPDocumentManager>(MockBehavior.Strict);
        documentManager.Setup(manager => manager.TryGetDocument(It.IsAny<Uri>(), out testDocument))
            .Returns(true);

        var htmlDiagnosticsInterceptor = new RazorHtmlPublishDiagnosticsInterceptor(documentManager.Object, diagnosticsProvider, _razorLSPConventions, _loggerProvider);
        var diagnosticRequest = new PublishDiagnosticParams()
        {
            Diagnostics = s_diagnostics,
            Uri = s_razorVirtualHtmlUri
        };

        // Act
        var result = await htmlDiagnosticsInterceptor.ApplyChangesAsync(JToken.FromObject(diagnosticRequest), string.Empty, cancellationToken: default).ConfigureAwait(false);

        // Assert
        var updatedParams = result.UpdatedToken.ToObject<PublishDiagnosticParams>();
        Assert.Empty(updatedParams.Diagnostics);
        Assert.Equal(s_razorUri, updatedParams.Uri);
        Assert.True(result.ChangedDocumentUri);
    }

    [Fact]
    public async Task ApplyChangesAsync_EmptyDiagnostics_ReturnsEmptyDiagnosticResponse()
    {
        // Arrange
        var documentManager = CreateDocumentManager();
        var diagnosticsProvider = Mock.Of<LSPDiagnosticsTranslator>(MockBehavior.Strict);

        var htmlDiagnosticsInterceptor = new RazorHtmlPublishDiagnosticsInterceptor(documentManager, diagnosticsProvider, _razorLSPConventions, _loggerProvider);
        var diagnosticRequest = new PublishDiagnosticParams()
        {
            Diagnostics = Array.Empty<Diagnostic>(),
            Uri = s_razorVirtualHtmlUri
        };

        // Act
        var result = await htmlDiagnosticsInterceptor.ApplyChangesAsync(JToken.FromObject(diagnosticRequest), string.Empty, cancellationToken: default).ConfigureAwait(false);

        // Assert
        var updatedParams = result.UpdatedToken.ToObject<PublishDiagnosticParams>();
        Assert.Empty(updatedParams.Diagnostics);
        Assert.Equal(s_razorUri, updatedParams.Uri);
        Assert.True(result.ChangedDocumentUri);
    }

    [Fact]
    public async Task ApplyChangesAsync_ProcessesDiagnostics_ReturnsDiagnosticResponse()
    {
        // Arrange
        var documentManager = CreateDocumentManager();
        var diagnosticsProvider = GetDiagnosticsProvider();

        var htmlDiagnosticsInterceptor = new RazorHtmlPublishDiagnosticsInterceptor(documentManager, diagnosticsProvider, _razorLSPConventions, _loggerProvider);
        var diagnosticRequest = new PublishDiagnosticParams()
        {
            Diagnostics = s_diagnostics,
            Uri = s_razorVirtualHtmlUri
        };

        // Act
        var result = await htmlDiagnosticsInterceptor.ApplyChangesAsync(JToken.FromObject(diagnosticRequest), string.Empty, cancellationToken: default).ConfigureAwait(false);

        // Assert
        var updatedParams = result.UpdatedToken.ToObject<PublishDiagnosticParams>();
        Assert.Equal(s_diagnostics, updatedParams.Diagnostics);
        Assert.Equal(s_razorUri, updatedParams.Uri);
        Assert.True(result.ChangedDocumentUri);
    }

    private static TrackingLSPDocumentManager CreateDocumentManager(int hostDocumentVersion = 0)
    {
        var testVirtualDocUri = s_razorVirtualHtmlUri;
        var testVirtualDocument = new TestVirtualDocumentSnapshot(s_razorUri, hostDocumentVersion);
        var htmlVirtualDocument = new HtmlVirtualDocumentSnapshot(testVirtualDocUri, Mock.Of<ITextSnapshot>(MockBehavior.Strict), hostDocumentVersion);
        LSPDocumentSnapshot testDocument = new TestLSPDocumentSnapshot(s_razorUri, hostDocumentVersion, testVirtualDocument, htmlVirtualDocument);
        var documentManager = new Mock<TrackingLSPDocumentManager>(MockBehavior.Strict);
        documentManager.Setup(manager => manager.TryGetDocument(It.IsAny<Uri>(), out testDocument))
            .Returns(true);
        return documentManager.Object;
    }

    private static LSPDiagnosticsTranslator GetDiagnosticsProvider()
    {
        var diagnosticsToIgnore = new HashSet<string>()
        {
            // N/A For HTML Diagnostics for now
            // https://dev.azure.com/devdiv/DevDiv/_workitems/edit/1257401
        };

        var diagnosticsProvider = new Mock<LSPDiagnosticsTranslator>(MockBehavior.Strict);
        diagnosticsProvider.Setup(d =>
            d.TranslateAsync(
                RazorLanguageKind.Html,
                s_razorUri,
                It.IsAny<Diagnostic[]>(),
                It.IsAny<CancellationToken>()))
            .Returns((RazorLanguageKind lang, Uri uri, Diagnostic[] diagnostics, CancellationToken ct) =>
            {
                var filteredDiagnostics = diagnostics.Where(d => !CanDiagnosticBeFiltered(d));
                if (!filteredDiagnostics.Any())
                {
                    return Task.FromResult(new RazorDiagnosticsResponse()
                    {
                        Diagnostics = Array.Empty<Diagnostic>(),
                        HostDocumentVersion = 0
                    });
                }

                return Task.FromResult(new RazorDiagnosticsResponse()
                {
                    Diagnostics = filteredDiagnostics.ToArray(),
                    HostDocumentVersion = 0
                });

                bool CanDiagnosticBeFiltered(Diagnostic d)
                {
                    return d.Code.HasValue &&
                        diagnosticsToIgnore.Contains(d.Code.Value.Second) &&
                        d.Severity != DiagnosticSeverity.Error;
                }
            });

        return diagnosticsProvider.Object;
    }
}
