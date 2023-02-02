// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;
using RazorDiagnosticFactory = Microsoft.AspNetCore.Razor.Language.RazorDiagnosticFactory;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class RazorDiagnosticsPublisherTest : LanguageServerTestBase
{
    private static readonly RazorDiagnostic[] s_emptyDiagnostics = Array.Empty<RazorDiagnostic>();

    private static readonly RazorDiagnostic[] s_singleDiagnostic = new RazorDiagnostic[]
    {
        RazorDiagnosticFactory.CreateDirective_BlockDirectiveCannotBeImported("test")
    };

    private readonly ProjectSnapshotManager _projectManager;
    private readonly DocumentSnapshot _closedDocument;
    private readonly DocumentSnapshot _openedDocument;
    private readonly RazorCodeDocument _testCodeDocument;

    public RazorDiagnosticsPublisherTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        var testProjectManager = TestProjectSnapshotManager.Create(LegacyDispatcher);
        var hostProject = new HostProject("C:/project/project.csproj", RazorConfiguration.Default, "TestRootNamespace");
        testProjectManager.ProjectAdded(hostProject);
        var sourceText = SourceText.From(string.Empty);
        var textAndVersion = TextAndVersion.Create(sourceText, VersionStamp.Default);
        var openedHostDocument = new HostDocument("C:/project/open_document.cshtml", "C:/project/open_document.cshtml");
        testProjectManager.DocumentAdded(hostProject, openedHostDocument, TextLoader.From(textAndVersion));
        testProjectManager.DocumentOpened(hostProject.FilePath, openedHostDocument.FilePath, sourceText);
        var closedHostDocument = new HostDocument("C:/project/closed_document.cshtml", "C:/project/closed_document.cshtml");
        testProjectManager.DocumentAdded(hostProject, closedHostDocument, TextLoader.From(textAndVersion));

        _openedDocument = testProjectManager.Projects[0].GetDocument(openedHostDocument.FilePath);
        _closedDocument = testProjectManager.Projects[0].GetDocument(closedHostDocument.FilePath);
        _projectManager = testProjectManager;
        _testCodeDocument = TestRazorCodeDocument.CreateEmpty();
    }

    [Fact]
    public void DocumentProcessed_NewWorkQueued_RestartsTimer()
    {
        // Arrange
        var processedOpenDocument = TestDocumentSnapshot.Create(_openedDocument.FilePath);
        var codeDocument = CreateCodeDocument(s_singleDiagnostic);
        processedOpenDocument.With(codeDocument);
        // ILanguageServerDocument
        var languageServerDocument = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict).Object;
        Mock.Get(languageServerDocument)
            .Setup(d => d.SendNotificationAsync(
                "textDocument/publishDiagnostics",
                It.IsAny<PublishDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();
        using (var publisher = new TestRazorDiagnosticsPublisher(LegacyDispatcher, languageServerDocument, LoggerFactory)
        {
            BlockBackgroundWorkCompleting = new ManualResetEventSlim(initialState: true),
            NotifyBackgroundWorkCompleting = new ManualResetEventSlim(initialState: false),
        })
        {
            publisher.Initialize(_projectManager);
            publisher.DocumentProcessed(_testCodeDocument, processedOpenDocument);
            Assert.True(publisher.NotifyBackgroundWorkCompleting.Wait(TimeSpan.FromSeconds(2)));
            publisher.NotifyBackgroundWorkCompleting.Reset();

            // Act
            publisher.DocumentProcessed(_testCodeDocument, processedOpenDocument);
            publisher.BlockBackgroundWorkCompleting.Set();

            // Assert
            // Verify that background work starts completing "again"
            Assert.True(publisher.NotifyBackgroundWorkCompleting.Wait(TimeSpan.FromSeconds(2)));
        }
    }

    [Fact]
    public async Task PublishDiagnosticsAsync_NewDocumentDiagnosticsGetPublished()
    {
        // Arrange
        var processedOpenDocument = TestDocumentSnapshot.Create(_openedDocument.FilePath);
        var codeDocument = CreateCodeDocument(s_singleDiagnostic);
        processedOpenDocument.With(codeDocument);
        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        languageServer
            .Setup(server => server.SendNotificationAsync(
                "textDocument/publishDiagnostics",
                It.IsAny<PublishDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, PublishDiagnosticParams, CancellationToken>((method, @params, cancellationToken) =>
            {
                Assert.Equal(processedOpenDocument.FilePath.TrimStart('/'), @params.Uri.AbsolutePath);
                var diagnostic = Assert.Single(@params.Diagnostics);
                var razorDiagnostic = s_singleDiagnostic[0];
                processedOpenDocument.TryGetText(out var sourceText);
                var expectedDiagnostic = RazorDiagnosticConverter.Convert(razorDiagnostic, sourceText);
                Assert.Equal(expectedDiagnostic.Message, diagnostic.Message);
                Assert.Equal(expectedDiagnostic.Severity, diagnostic.Severity);
                Assert.Equal(expectedDiagnostic.Range, diagnostic.Range);
            })
            .Returns(Task.CompletedTask);
        using (var publisher = new TestRazorDiagnosticsPublisher(LegacyDispatcher, languageServer.Object, LoggerFactory))
        {
            publisher.Initialize(_projectManager);

            // Act
            await publisher.PublishDiagnosticsAsync(processedOpenDocument);

            // Assert
            languageServer.VerifyAll();
        }
    }

    [Fact]
    public async Task PublishDiagnosticsAsync_NewDiagnosticsGetPublished()
    {
        // Arrange
        var processedOpenDocument = TestDocumentSnapshot.Create(_openedDocument.FilePath);
        var codeDocument = CreateCodeDocument(s_singleDiagnostic);
        processedOpenDocument.With(codeDocument);
        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        languageServer.Setup(
            server => server.SendNotificationAsync(
                "textDocument/publishDiagnostics",
                It.IsAny<PublishDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, PublishDiagnosticParams, CancellationToken>((method, @params, cancellationTokne) =>
            {
                Assert.Equal(processedOpenDocument.FilePath.TrimStart('/'), @params.Uri.AbsolutePath);
                var diagnostic = Assert.Single(@params.Diagnostics);
                var razorDiagnostic = s_singleDiagnostic[0];
                processedOpenDocument.TryGetText(out var sourceText);
                var expectedDiagnostic = RazorDiagnosticConverter.Convert(razorDiagnostic, sourceText);
                Assert.Equal(expectedDiagnostic.Message, diagnostic.Message);
                Assert.Equal(expectedDiagnostic.Severity, diagnostic.Severity);
                Assert.Equal(expectedDiagnostic.Range, diagnostic.Range);
            })
            .Returns(Task.CompletedTask);

        using (var publisher = new TestRazorDiagnosticsPublisher(LegacyDispatcher, languageServer.Object, LoggerFactory))
        {
            publisher.PublishedDiagnostics[processedOpenDocument.FilePath] = s_emptyDiagnostics;
            publisher.Initialize(_projectManager);

            // Act
            await publisher.PublishDiagnosticsAsync(processedOpenDocument);

            // Assert
            languageServer.VerifyAll();
        }
    }

    [Fact]
    public async Task PublishDiagnosticsAsync_NoopsIfDiagnosticsAreSameAsPreviousPublish()
    {
        // Arrange
        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        var processedOpenDocument = TestDocumentSnapshot.Create(_openedDocument.FilePath);
        var codeDocument = CreateCodeDocument(s_singleDiagnostic);
        processedOpenDocument.With(codeDocument);

        using var publisher = new TestRazorDiagnosticsPublisher(LegacyDispatcher, languageServer.Object, LoggerFactory);
        publisher.PublishedDiagnostics[processedOpenDocument.FilePath] = s_singleDiagnostic;
        publisher.Initialize(_projectManager);

        // Act & Assert
        await publisher.PublishDiagnosticsAsync(processedOpenDocument);
    }

    [Fact]
    public void ClearClosedDocuments_ClearsDiagnosticsForClosedDocument()
    {
        // Arrange
        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        languageServer
            .Setup(server => server.SendNotificationAsync(
                "textDocument/publishDiagnostics",
                It.IsAny<PublishDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, PublishDiagnosticParams, CancellationToken>((method, @params, cancellationToken) =>
            {
                Assert.Equal(_closedDocument.FilePath.TrimStart('/'), @params.Uri.AbsolutePath);
                Assert.Empty(@params.Diagnostics);
            })
            .Returns(Task.CompletedTask);

        using var publisher = new TestRazorDiagnosticsPublisher(LegacyDispatcher, languageServer.Object, LoggerFactory);
        publisher.PublishedDiagnostics[_closedDocument.FilePath] = s_singleDiagnostic;
        publisher.Initialize(_projectManager);

        // Act
        publisher.ClearClosedDocuments();

        // Assert
        languageServer.VerifyAll();
    }

    [Fact]
    public void ClearClosedDocuments_NoopsIfDocumentIsStillOpen()
    {
        // Arrange
        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        using (var publisher = new TestRazorDiagnosticsPublisher(LegacyDispatcher, languageServer.Object, LoggerFactory))
        {
            publisher.PublishedDiagnostics[_openedDocument.FilePath] = s_singleDiagnostic;
            publisher.Initialize(_projectManager);

            // Act & Assert
            publisher.ClearClosedDocuments();
        }
    }

    [Fact]
    public void ClearClosedDocuments_NoopsIfDocumentIsClosedButNoDiagnostics()
    {
        // Arrange
        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        using (var publisher = new TestRazorDiagnosticsPublisher(LegacyDispatcher, languageServer.Object, LoggerFactory))
        {
            publisher.PublishedDiagnostics[_closedDocument.FilePath] = s_emptyDiagnostics;
            publisher.Initialize(_projectManager);

            // Act & Assert
            publisher.ClearClosedDocuments();
        }
    }

    [Fact]
    public void ClearClosedDocuments_RestartsTimerIfDocumentsStillOpen()
    {
        // Arrange
        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        using (var publisher = new TestRazorDiagnosticsPublisher(LegacyDispatcher, languageServer.Object, LoggerFactory))
        {
            publisher.PublishedDiagnostics[_closedDocument.FilePath] = s_emptyDiagnostics;
            publisher.PublishedDiagnostics[_openedDocument.FilePath] = s_emptyDiagnostics;
            publisher.Initialize(_projectManager);

            // Act
            publisher.ClearClosedDocuments();

            // Assert
            Assert.NotNull(publisher._documentClosedTimer);
        }
    }

    private static RazorCodeDocument CreateCodeDocument(params RazorDiagnostic[] diagnostics)
    {
        var codeDocument = TestRazorCodeDocument.CreateEmpty();
        var razorCSharpDocument = RazorCSharpDocument.Create(codeDocument, string.Empty, RazorCodeGenerationOptions.CreateDefault(), diagnostics);
        codeDocument.SetCSharpDocument(razorCSharpDocument);

        return codeDocument;
    }

    private class TestRazorDiagnosticsPublisher : RazorDiagnosticsPublisher, IDisposable
    {
        public TestRazorDiagnosticsPublisher(
            ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
            ClientNotifierServiceBase languageServer,
            ILoggerFactory loggerFactory)
            : base(projectSnapshotManagerDispatcher, languageServer, loggerFactory)
        {
            // The diagnostics publisher by default will wait 2 seconds until publishing diagnostics. For testing purposes we redcuce
            // the amount of time we wait for diagnostic publishing because we have more concrete control of the timer and its lifecycle.
            _publishDelay = TimeSpan.FromMilliseconds(1);
        }

        public void Dispose()
        {
            _workTimer?.Dispose();
            _documentClosedTimer?.Dispose();
        }
    }
}
