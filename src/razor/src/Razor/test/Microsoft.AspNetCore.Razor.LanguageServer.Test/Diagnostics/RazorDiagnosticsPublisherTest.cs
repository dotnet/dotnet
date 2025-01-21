// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Diagnostics;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Diagnostic = Microsoft.VisualStudio.LanguageServer.Protocol.Diagnostic;
using DiagnosticSeverity = Microsoft.VisualStudio.LanguageServer.Protocol.DiagnosticSeverity;
using RazorDiagnosticFactory = Microsoft.AspNetCore.Razor.Language.RazorDiagnosticFactory;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Diagnostics;

public class RazorDiagnosticsPublisherTest : LanguageServerTestBase
{
    private static readonly HostProject s_hostProject = new("C:/project/project.csproj", "C:/project/obj", RazorConfiguration.Default, "TestRootNamespace");
    private static readonly HostDocument s_openHostDocument = new("C:/project/open_document.cshtml", "C:/project/open_document.cshtml");
    private static readonly HostDocument s_closedHostDocument = new("C:/project/closed_document.cshtml", "C:/project/closed_document.cshtml");
    private static readonly Uri s_openedDocumentUri = new(s_openHostDocument.FilePath);

    private static readonly SourceText s_razorText = SourceText.From("""
        @using Microsoft.AspNetCore.Components.Forms;
        
        @code {
            private string _id { get; set; }
        }
        
        <div>
            <div>
                <InputSelect @bind-Value="_id">
                    @if (true)
                    {
                        <option>goo</option>
                    }
                </InputSelect>
            </div>
        </div>
        """);

    private static readonly TestCode s_razorTestCodeWithError = """
        @using Microsoft.AspNetCore.Components.Forms;
        
        @code {
            private string _id { get; set; }
        }
        
        <div>
            <div>
                <InputSelect @bind-Value="_id">
                    @if (true)
                    {
                        <[|option|]>goo</opti>
                    }
                </InputSelect>
            </div>
        </div>
        """;

    private static readonly SourceText s_razorTextWithError = SourceText.From(s_razorTestCodeWithError.Text);

    private static readonly RazorDiagnostic[] s_singleRazorDiagnostic =
    [
        RazorDiagnosticFactory.CreateParsing_MissingEndTag(GetErrorSpan(s_openHostDocument.FilePath, s_razorTextWithError, s_razorTestCodeWithError), "option")
    ];

    private static readonly Diagnostic[] s_singleCSharpDiagnostic =
    [
        new Diagnostic()
        {
            Code = "TestCode",
            Severity = DiagnosticSeverity.Error,
            Message = "TestMessage",
            Range = VsLspFactory.CreateSingleLineRange(line: 0, character: 0, length: 1)
        }
    ];

    private readonly TestProjectSnapshotManager _projectManager;
    private readonly DocumentContextFactory _documentContextFactory;

    public RazorDiagnosticsPublisherTest(ITestOutputHelper testOutput) : base(testOutput)
    {
        _projectManager = CreateProjectSnapshotManager();
        _documentContextFactory = new DocumentContextFactory(_projectManager, LoggerFactory);
    }

    protected override async Task InitializeAsync()
    {
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
            updater.AddDocument(s_hostProject.Key, s_openHostDocument, EmptyTextLoader.Instance);
            updater.OpenDocument(s_hostProject.Key, s_openHostDocument.FilePath, s_razorText);
            updater.AddDocument(s_hostProject.Key, s_closedHostDocument, EmptyTextLoader.Instance);
        });
    }

    private static SourceSpan GetErrorSpan(string filePath, SourceText text, TestCode testCode)
    {
        var span = testCode.Span;
        var location = new SourceLocation(filePath, span.Start, text.GetLinePosition(span.Start));

        return new SourceSpan(location, span.Length);
    }

    private Task UpdateWithErrorTextAsync()
    {
        return _projectManager.UpdateAsync(updater =>
        {
            updater.UpdateDocumentText(s_hostProject.Key, s_openHostDocument.FilePath, s_razorTextWithError);
        });
    }

    [Fact]
    public async Task DocumentProcessed_NewWorkQueued_RestartsTimer()
    {
        // Arrange
        await UpdateWithErrorTextAsync();

        var clientConnectionMock = new StrictMock<IClientConnection>();
        clientConnectionMock
            .Setup(d => d.SendNotificationAsync(
                "textDocument/publishDiagnostics",
                It.IsAny<PublishDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();
        clientConnectionMock
            .Setup(d => d.SendRequestAsync<DocumentDiagnosticParams, SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?>(
                CustomMessageNames.RazorCSharpPullDiagnosticsEndpointName,
                It.IsAny<DocumentDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?(new FullDocumentDiagnosticReport()))
            .Verifiable();

        using var publisher = CreatePublisher(clientConnectionMock.Object);
        var publisherAccessor = publisher.GetTestAccessor();

        // Act 1
        var openDocument = _projectManager.GetRequiredDocument(s_hostProject.Key, s_openHostDocument.FilePath);
        var codeDocument = await openDocument.GetGeneratedOutputAsync(DisposalToken);

        publisher.DocumentProcessed(codeDocument, openDocument);
        await publisherAccessor.WaitForDiagnosticsToPublishAsync();

        // Assert 1
        clientConnectionMock
            .Verify(d => d.SendRequestAsync<DocumentDiagnosticParams, SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?>(
                CustomMessageNames.RazorCSharpPullDiagnosticsEndpointName,
                It.IsAny<DocumentDiagnosticParams>(),
                It.IsAny<CancellationToken>()),
                Times.Once);

        // Act 2
        publisher.DocumentProcessed(codeDocument, openDocument);
        await publisherAccessor.WaitForDiagnosticsToPublishAsync();

        // Assert 2
        clientConnectionMock
            .Verify(d => d.SendRequestAsync<DocumentDiagnosticParams, SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?>(
                CustomMessageNames.RazorCSharpPullDiagnosticsEndpointName,
                It.IsAny<DocumentDiagnosticParams>(),
                It.IsAny<CancellationToken>()),
                Times.Exactly(2));
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    public async Task PublishDiagnosticsAsync_NewDocumentDiagnosticsGetPublished(bool shouldContainCSharpDiagnostic, bool shouldContainRazorDiagnostic)
    {
        // Arrange
        if (shouldContainRazorDiagnostic)
        {
            await UpdateWithErrorTextAsync();
        }

        var singleCSharpDiagnostic = new[]
        {
            new Diagnostic()
            {
                Code = "TestCode",
                Severity = DiagnosticSeverity.Error,
                Message = "TestMessage",
                Range = VsLspFactory.CreateSingleLineRange(line: 0, character: 0, length: 1)
            }
        };

        var openDocument = _projectManager.GetRequiredDocument(s_hostProject.Key, s_openHostDocument.FilePath);

        var clientConnectionMock = new StrictMock<IClientConnection>();
        var requestResult = new FullDocumentDiagnosticReport();
        if (shouldContainCSharpDiagnostic)
        {
            requestResult.Items = singleCSharpDiagnostic;
        }

        clientConnectionMock
            .Setup(server => server.SendRequestAsync<DocumentDiagnosticParams, SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?>(
                CustomMessageNames.RazorCSharpPullDiagnosticsEndpointName,
                It.IsAny<DocumentDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Callback((string method, DocumentDiagnosticParams @params, CancellationToken cancellationToken) =>
            {
                Assert.Equal(s_openedDocumentUri, @params.TextDocument.Uri);
            })
            .ReturnsAsync(new SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?(requestResult));

        clientConnectionMock
            .Setup(server => server.SendNotificationAsync(
                "textDocument/publishDiagnostics",
                It.IsAny<PublishDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Callback((string method, PublishDiagnosticParams @params, CancellationToken cancellationToken) =>
            {
                Assert.Equal(s_openHostDocument.FilePath.TrimStart('/'), @params.Uri.AbsolutePath);
                Assert.Equal(shouldContainCSharpDiagnostic && shouldContainRazorDiagnostic ? 2 : 1, @params.Diagnostics.Length);
                if (shouldContainCSharpDiagnostic)
                {
                    Assert.Equal(singleCSharpDiagnostic[0], shouldContainRazorDiagnostic ? @params.Diagnostics[1] : @params.Diagnostics[0]);
                }

                if (shouldContainRazorDiagnostic)
                {
                    var resultRazorDiagnostic = @params.Diagnostics[0];
                    var razorDiagnostic = s_singleRazorDiagnostic[0];
                    Assert.True(openDocument.TryGetText(out var sourceText));
                    var expectedRazorDiagnostic = RazorDiagnosticConverter.Convert(razorDiagnostic, sourceText, openDocument);
                    Assert.Equal(expectedRazorDiagnostic.Message, resultRazorDiagnostic.Message);
                    Assert.Equal(expectedRazorDiagnostic.Severity, resultRazorDiagnostic.Severity);
                    Assert.Equal(expectedRazorDiagnostic.Range, resultRazorDiagnostic.Range);
                    Assert.NotNull(expectedRazorDiagnostic.Projects);

                    var project = Assert.Single(expectedRazorDiagnostic.Projects);
                    Assert.Equal(openDocument.Project.DisplayName, project.ProjectName);
                    Assert.Equal(openDocument.Project.Key.Id, project.ProjectIdentifier);

                }
            })
            .Returns(Task.CompletedTask);

        using var publisher = CreatePublisher(clientConnectionMock.Object);
        var publisherAccessor = publisher.GetTestAccessor();

        // Act
        await publisherAccessor.PublishDiagnosticsAsync(openDocument, DisposalToken);

        // Assert
        clientConnectionMock.VerifyAll();
    }

    [Fact]
    public async Task PublishDiagnosticsAsync_NewRazorDiagnosticsGetPublished()
    {
        // Arrange
        await UpdateWithErrorTextAsync();

        var openDocument = _projectManager.GetRequiredDocument(s_hostProject.Key, s_openHostDocument.FilePath);

        var clientConnectionMock = new StrictMock<IClientConnection>();
        clientConnectionMock
            .Setup(server => server.SendRequestAsync<DocumentDiagnosticParams, SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?>(
                CustomMessageNames.RazorCSharpPullDiagnosticsEndpointName,
                It.IsAny<DocumentDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Callback((string method, DocumentDiagnosticParams @params, CancellationToken cancellationToken) =>
            {
                Assert.Equal(s_openedDocumentUri, @params.TextDocument.Uri);
            })
            .ReturnsAsync(new SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?(new FullDocumentDiagnosticReport()));

        clientConnectionMock
            .Setup(server => server.SendNotificationAsync(
                "textDocument/publishDiagnostics",
                It.IsAny<PublishDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Callback((string method, PublishDiagnosticParams @params, CancellationToken cancellationToken) =>
            {
                Assert.Equal(openDocument.FilePath.TrimStart('/'), @params.Uri.AbsolutePath);
                var diagnostic = Assert.Single(@params.Diagnostics);
                var razorDiagnostic = s_singleRazorDiagnostic[0];
                Assert.True(openDocument.TryGetText(out var sourceText));
                var expectedDiagnostic = RazorDiagnosticConverter.Convert(razorDiagnostic, sourceText, openDocument);
                Assert.Equal(expectedDiagnostic.Message, diagnostic.Message);
                Assert.Equal(expectedDiagnostic.Severity, diagnostic.Severity);
                Assert.Equal(expectedDiagnostic.Range, diagnostic.Range);

                Assert.NotNull(expectedDiagnostic.Projects);
                var project = Assert.Single(expectedDiagnostic.Projects);
                Assert.Equal(openDocument.Project.DisplayName, project.ProjectName);
                Assert.Equal(openDocument.Project.Key.Id, project.ProjectIdentifier);
            })
            .Returns(Task.CompletedTask);

        using var publisher = CreatePublisher(clientConnectionMock.Object);
        var publisherAccessor = publisher.GetTestAccessor();
        publisherAccessor.SetPublishedDiagnostics(openDocument.FilePath, razorDiagnostics: [], csharpDiagnostics: null);

        // Act
        await publisherAccessor.PublishDiagnosticsAsync(openDocument, DisposalToken);

        // Assert
        clientConnectionMock.VerifyAll();
    }

    [Fact]
    public async Task PublishDiagnosticsAsync_NewCSharpDiagnosticsGetPublished()
    {
        // Arrange
        var openDocument = _projectManager.GetRequiredDocument(s_hostProject.Key, s_openHostDocument.FilePath);

        var arranging = true;
        var clientConnectionMock = new StrictMock<IClientConnection>();
        clientConnectionMock
            .Setup(server => server.SendRequestAsync<DocumentDiagnosticParams, SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?>(
                CustomMessageNames.RazorCSharpPullDiagnosticsEndpointName,
                It.IsAny<DocumentDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Callback((string method, DocumentDiagnosticParams @params, CancellationToken cancellationToken) =>
            {
                Assert.Equal(s_openedDocumentUri, @params.TextDocument.Uri);
            })
            .ReturnsAsync(
                new SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?(
                    arranging ? new FullDocumentDiagnosticReport() : new FullDocumentDiagnosticReport { Items = s_singleCSharpDiagnostic.ToArray() }));

        clientConnectionMock.Setup(
            server => server.SendNotificationAsync(
                "textDocument/publishDiagnostics",
                It.IsAny<PublishDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Callback((string method, PublishDiagnosticParams @params, CancellationToken cancellationToken) =>
            {
                Assert.Equal(openDocument.FilePath.TrimStart('/'), @params.Uri.AbsolutePath);
                Assert.True(openDocument.TryGetText(out var sourceText));
                if (arranging)
                {
                    Assert.Empty(@params.Diagnostics);
                }
                else
                {
                    var diagnostic = Assert.Single(@params.Diagnostics);
                    Assert.Equal(s_singleCSharpDiagnostic[0], diagnostic);
                }
            })
            .Returns(Task.CompletedTask);

        using var publisher = CreatePublisher(clientConnectionMock.Object);
        var publisherAccessor = publisher.GetTestAccessor();

        await publisherAccessor.PublishDiagnosticsAsync(openDocument, DisposalToken);
        arranging = false;

        // Act
        await publisherAccessor.PublishDiagnosticsAsync(openDocument, DisposalToken);

        // Assert
        clientConnectionMock.VerifyAll();
    }

    [Fact]
    public async Task PublishDiagnosticsAsync_NoopsIfRazorDiagnosticsAreSameAsPreviousPublish()
    {
        // Arrange
        await UpdateWithErrorTextAsync();

        var openDocument = _projectManager.GetRequiredDocument(s_hostProject.Key, s_openHostDocument.FilePath);

        var clientConnectionMock = new StrictMock<IClientConnection>();
        clientConnectionMock
            .Setup(server => server.SendRequestAsync<DocumentDiagnosticParams, SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?>(
                CustomMessageNames.RazorCSharpPullDiagnosticsEndpointName,
                It.IsAny<DocumentDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Callback((string method, DocumentDiagnosticParams @params, CancellationToken cancellationToken) =>
            {
                Assert.Equal(s_openedDocumentUri, @params.TextDocument.Uri);
            })
            .ReturnsAsync(new SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?(new FullDocumentDiagnosticReport()));

        using var publisher = CreatePublisher(clientConnectionMock.Object);
        var publisherAccessor = publisher.GetTestAccessor();
        publisherAccessor.SetPublishedDiagnostics(openDocument.FilePath, s_singleRazorDiagnostic, csharpDiagnostics: null);

        // Act & Assert
        await publisherAccessor.PublishDiagnosticsAsync(openDocument, DisposalToken);
    }

    [Fact]
    public async Task PublishDiagnosticsAsync_NoopsIfCSharpDiagnosticsAreSameAsPreviousPublish()
    {
        // Arrange
        var openDocument = _projectManager.GetRequiredDocument(s_hostProject.Key, s_openHostDocument.FilePath);

        var clientConnectionMock = new StrictMock<IClientConnection>();
        var arranging = true;

        clientConnectionMock
            .Setup(server => server.SendRequestAsync<DocumentDiagnosticParams, SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?>(
                CustomMessageNames.RazorCSharpPullDiagnosticsEndpointName,
                It.IsAny<DocumentDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Callback((string method, DocumentDiagnosticParams @params, CancellationToken cancellationToken) =>
            {
                Assert.Equal(s_openedDocumentUri, @params.TextDocument.Uri);
            })
            .ReturnsAsync(new SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?(new FullDocumentDiagnosticReport()));

        clientConnectionMock
            .Setup(server => server.SendNotificationAsync(
                "textDocument/publishDiagnostics",
                It.IsAny<PublishDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Callback((string method, PublishDiagnosticParams @params, CancellationToken cancellationToken) =>
            {
                if (!arranging)
                {
                    Assert.Fail("This callback should not have been received since diagnostics are the same as previous published");
                }

                Assert.Equal(openDocument.FilePath.TrimStart('/'), @params.Uri.AbsolutePath);
                Assert.True(openDocument.TryGetText(out var sourceText));
                Assert.Empty(@params.Diagnostics);
            })
            .Returns(Task.CompletedTask);

        using var publisher = CreatePublisher(clientConnectionMock.Object);
        var publisherAccessor = publisher.GetTestAccessor();

        await publisherAccessor.PublishDiagnosticsAsync(openDocument, DisposalToken);
        arranging = false;

        // Act & Assert
        await publisherAccessor.PublishDiagnosticsAsync(openDocument, DisposalToken);
    }

    [Fact]
    public void ClearClosedDocuments_ClearsDiagnosticsForClosedDocument()
    {
        // Arrange
        var clientConnectionMock = new StrictMock<IClientConnection>();
        clientConnectionMock
            .Setup(server => server.SendNotificationAsync(
                "textDocument/publishDiagnostics",
                It.IsAny<PublishDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Callback((string method, PublishDiagnosticParams @params, CancellationToken cancellationToken) =>
            {
                Assert.Equal(s_closedHostDocument.FilePath.TrimStart('/'), @params.Uri.AbsolutePath);
                Assert.Empty(@params.Diagnostics);
            })
            .Returns(Task.CompletedTask);

        using var publisher = CreatePublisher(clientConnectionMock.Object);
        var publisherAccessor = publisher.GetTestAccessor();
        publisherAccessor.SetPublishedDiagnostics(s_closedHostDocument.FilePath, s_singleRazorDiagnostic, s_singleCSharpDiagnostic);

        // Act
        publisherAccessor.ClearClosedDocuments();

        // Assert
        clientConnectionMock.VerifyAll();
    }

    [Fact]
    public void ClearClosedDocuments_NoopsIfDocumentIsStillOpen()
    {
        // Arrange
        using var publisher = CreatePublisher();
        var publisherAccessor = publisher.GetTestAccessor();
        publisherAccessor.SetPublishedDiagnostics(s_openHostDocument.FilePath, s_singleRazorDiagnostic, s_singleCSharpDiagnostic);

        // Act & Assert
        publisherAccessor.ClearClosedDocuments();
    }

    [Fact]
    public void ClearClosedDocuments_NoopsIfDocumentIsClosedButNoDiagnostics()
    {
        // Arrange
        using var publisher = CreatePublisher();
        var publisherAccessor = publisher.GetTestAccessor();
        publisherAccessor.SetPublishedDiagnostics(s_closedHostDocument.FilePath, razorDiagnostics: [], csharpDiagnostics: []);

        // Act & Assert
        publisherAccessor.ClearClosedDocuments();
    }

    [Fact]
    public void ClearClosedDocuments_RestartsTimerIfDocumentsStillOpen()
    {
        // Arrange
        using var publisher = CreatePublisher();
        var publisherAccessor = publisher.GetTestAccessor();
        publisherAccessor.SetPublishedDiagnostics(s_closedHostDocument.FilePath, razorDiagnostics: [], csharpDiagnostics: []);
        publisherAccessor.SetPublishedDiagnostics(s_openHostDocument.FilePath, razorDiagnostics: [], csharpDiagnostics: []);

        // Act
        publisherAccessor.ClearClosedDocuments();

        // Assert
        Assert.True(publisherAccessor.IsWaitingToClearClosedDocuments);
    }

    private TestRazorDiagnosticsPublisher CreatePublisher(IClientConnection? clientConnection = null)
    {
        clientConnection ??= StrictMock.Of<IClientConnection>();

        var documentContextFactory = new DocumentContextFactory(_projectManager, LoggerFactory);
        var filePathService = new LSPFilePathService(TestLanguageServerFeatureOptions.Instance);
        var documentMappingService = new LspDocumentMappingService(filePathService, documentContextFactory, LoggerFactory);
        var translateDiagnosticsService = new RazorTranslateDiagnosticsService(documentMappingService, LoggerFactory);

        return new TestRazorDiagnosticsPublisher(_projectManager, clientConnection, TestLanguageServerFeatureOptions.Instance, translateDiagnosticsService, _documentContextFactory, LoggerFactory);
    }

    private sealed class TestRazorDiagnosticsPublisher(
        ProjectSnapshotManager projectManager,
        IClientConnection clientConnection,
        LanguageServerFeatureOptions options,
        RazorTranslateDiagnosticsService translateDiagnosticsService,
        IDocumentContextFactory documentContextFactory,
        ILoggerFactory loggerFactory) : RazorDiagnosticsPublisher(
            projectManager,
            clientConnection,
            options,
            translateDiagnosticsService: new Lazy<RazorTranslateDiagnosticsService>(() => translateDiagnosticsService),
            documentContextFactory: new Lazy<IDocumentContextFactory>(() => documentContextFactory),
            loggerFactory,
            publishDelay: TimeSpan.FromMilliseconds(1));
}
