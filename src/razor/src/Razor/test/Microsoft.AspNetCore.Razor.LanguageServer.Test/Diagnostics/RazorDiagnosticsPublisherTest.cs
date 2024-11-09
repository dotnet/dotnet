// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
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

public class RazorDiagnosticsPublisherTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    private static readonly RazorDiagnostic[] s_singleRazorDiagnostic =
    [
        RazorDiagnosticFactory.CreateDirective_BlockDirectiveCannotBeImported("test")
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

    // These fields are initialized by InitializeAsync()
#nullable disable
    private IProjectSnapshotManager _projectManager;
    private IDocumentSnapshot _closedDocument;
    private IDocumentSnapshot _openedDocument;
    private RazorCodeDocument _testCodeDocument;
    private Uri _openedDocumentUri;
#nullable enable

    protected override async Task InitializeAsync()
    {
        var testProjectManager = CreateProjectSnapshotManager();
        var hostProject = new HostProject("C:/project/project.csproj", "C:/project/obj", RazorConfiguration.Default, "TestRootNamespace");
        var sourceText = SourceText.From(string.Empty);
        var textAndVersion = TextAndVersion.Create(sourceText, VersionStamp.Default);
        var openedHostDocument = new HostDocument("C:/project/open_document.cshtml", "C:/project/open_document.cshtml");
        var closedHostDocument = new HostDocument("C:/project/closed_document.cshtml", "C:/project/closed_document.cshtml");

        await testProjectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(hostProject);
            updater.DocumentAdded(hostProject.Key, openedHostDocument, TextLoader.From(textAndVersion));
            updater.DocumentOpened(hostProject.Key, openedHostDocument.FilePath, sourceText);
            updater.DocumentAdded(hostProject.Key, closedHostDocument, TextLoader.From(textAndVersion));
        });

        var project = testProjectManager.GetLoadedProject(hostProject.Key);

        var openedDocument = project.GetDocument(openedHostDocument.FilePath).AssumeNotNull();
        _openedDocument = openedDocument;
        _openedDocumentUri = new Uri("C:/project/open_document.cshtml");

        var closedDocument = project.GetDocument(closedHostDocument.FilePath).AssumeNotNull();
        _closedDocument = closedDocument;

        _projectManager = testProjectManager;
        _testCodeDocument = TestRazorCodeDocument.CreateEmpty();
    }

    [Fact]
    public async Task DocumentProcessed_NewWorkQueued_RestartsTimer()
    {
        // Arrange
        var codeDocument = CreateCodeDocument(s_singleRazorDiagnostic);
        var processedOpenDocument = TestDocumentSnapshot.Create(_openedDocument.FilePath, codeDocument);

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
            .Returns(Task.FromResult(new SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?(new FullDocumentDiagnosticReport())))
            .Verifiable();

        var documentContextFactory = new TestDocumentContextFactory(_openedDocument.FilePath, codeDocument);
        var translateDiagnosticsService = new RazorTranslateDiagnosticsService(StrictMock.Of<IDocumentMappingService>(), LoggerFactory);

        using var publisher = new TestRazorDiagnosticsPublisher(_projectManager, clientConnectionMock.Object, TestLanguageServerFeatureOptions.Instance, translateDiagnosticsService, documentContextFactory, LoggerFactory);
        var publisherAccessor = publisher.GetTestAccessor();

        // Act 1
        publisher.DocumentProcessed(_testCodeDocument, processedOpenDocument);
        await publisherAccessor.WaitForDiagnosticsToPublishAsync();

        // Assert 1
        clientConnectionMock
            .Verify(d => d.SendRequestAsync<DocumentDiagnosticParams, SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?>(
                CustomMessageNames.RazorCSharpPullDiagnosticsEndpointName,
                It.IsAny<DocumentDiagnosticParams>(),
                It.IsAny<CancellationToken>()),
                Times.Once);

        // Act 2
        publisher.DocumentProcessed(_testCodeDocument, processedOpenDocument);
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

        var codeDocument = CreateCodeDocument(shouldContainRazorDiagnostic ? s_singleRazorDiagnostic : []);
        var processedOpenDocument = TestDocumentSnapshot.Create(_openedDocument.FilePath, codeDocument);

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
                Assert.Equal(_openedDocumentUri, @params.TextDocument.Uri);
            })
            .Returns(Task.FromResult(new SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?(requestResult)));

        clientConnectionMock
            .Setup(server => server.SendNotificationAsync(
                "textDocument/publishDiagnostics",
                It.IsAny<PublishDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Callback((string method, PublishDiagnosticParams @params, CancellationToken cancellationToken) =>
            {
                Assert.Equal(processedOpenDocument.FilePath.TrimStart('/'), @params.Uri.AbsolutePath);
                Assert.Equal(shouldContainCSharpDiagnostic && shouldContainRazorDiagnostic ? 2 : 1, @params.Diagnostics.Length);
                if (shouldContainCSharpDiagnostic)
                {
                    Assert.Equal(singleCSharpDiagnostic[0], shouldContainRazorDiagnostic ? @params.Diagnostics[1] : @params.Diagnostics[0]);
                }

                if (shouldContainRazorDiagnostic)
                {
                    var resultRazorDiagnostic = @params.Diagnostics[0];
                    var razorDiagnostic = s_singleRazorDiagnostic[0];
                    Assert.True(processedOpenDocument.TryGetText(out var sourceText));
                    var expectedRazorDiagnostic = RazorDiagnosticConverter.Convert(razorDiagnostic, sourceText, _openedDocument);
                    Assert.Equal(expectedRazorDiagnostic.Message, resultRazorDiagnostic.Message);
                    Assert.Equal(expectedRazorDiagnostic.Severity, resultRazorDiagnostic.Severity);
                    Assert.Equal(expectedRazorDiagnostic.Range, resultRazorDiagnostic.Range);
                    Assert.NotNull(expectedRazorDiagnostic.Projects);
                    Assert.Single(expectedRazorDiagnostic.Projects);

                    var project = expectedRazorDiagnostic.Projects.Single();
                    Assert.Equal(_openedDocument.Project.DisplayName, project.ProjectName);
                    Assert.Equal(_openedDocument.Project.Key.Id, project.ProjectIdentifier);

                }
            })
            .Returns(Task.CompletedTask);

        var documentContextFactory = new TestDocumentContextFactory(_openedDocument.FilePath, codeDocument);
        var filePathService = new LSPFilePathService(TestLanguageServerFeatureOptions.Instance);
        var documentMappingService = new LspDocumentMappingService(filePathService, documentContextFactory, LoggerFactory);
        var translateDiagnosticsService = new RazorTranslateDiagnosticsService(documentMappingService, LoggerFactory);

        using var publisher = new TestRazorDiagnosticsPublisher(_projectManager, clientConnectionMock.Object, TestLanguageServerFeatureOptions.Instance, translateDiagnosticsService, documentContextFactory, LoggerFactory);
        var publisherAccessor = publisher.GetTestAccessor();

        // Act
        await publisherAccessor.PublishDiagnosticsAsync(processedOpenDocument, DisposalToken);

        // Assert
        clientConnectionMock.VerifyAll();
    }

    [Fact]
    public async Task PublishDiagnosticsAsync_NewRazorDiagnosticsGetPublished()
    {
        // Arrange
        var codeDocument = CreateCodeDocument(s_singleRazorDiagnostic);
        var processedOpenDocument = TestDocumentSnapshot.Create(_openedDocument.FilePath, codeDocument);

        var clientConnectionMock = new StrictMock<IClientConnection>();
        clientConnectionMock
            .Setup(server => server.SendRequestAsync<DocumentDiagnosticParams, SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?>(
                CustomMessageNames.RazorCSharpPullDiagnosticsEndpointName,
                It.IsAny<DocumentDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Callback((string method, DocumentDiagnosticParams @params, CancellationToken cancellationToken) =>
            {
                Assert.Equal(_openedDocumentUri, @params.TextDocument.Uri);
            })
            .Returns(Task.FromResult(new SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?(new FullDocumentDiagnosticReport())));

        clientConnectionMock
            .Setup(server => server.SendNotificationAsync(
                "textDocument/publishDiagnostics",
                It.IsAny<PublishDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Callback((string method, PublishDiagnosticParams @params, CancellationToken cancellationToken) =>
            {
                Assert.Equal(processedOpenDocument.FilePath.TrimStart('/'), @params.Uri.AbsolutePath);
                var diagnostic = Assert.Single(@params.Diagnostics);
                var razorDiagnostic = s_singleRazorDiagnostic[0];
                Assert.True(processedOpenDocument.TryGetText(out var sourceText));
                var expectedDiagnostic = RazorDiagnosticConverter.Convert(razorDiagnostic, sourceText, _openedDocument);
                Assert.Equal(expectedDiagnostic.Message, diagnostic.Message);
                Assert.Equal(expectedDiagnostic.Severity, diagnostic.Severity);
                Assert.Equal(expectedDiagnostic.Range, diagnostic.Range);

                Assert.NotNull(expectedDiagnostic.Projects);
                var project = expectedDiagnostic.Projects.Single();
                Assert.Equal(_openedDocument.Project.DisplayName, project.ProjectName);
                Assert.Equal(_openedDocument.Project.Key.Id, project.ProjectIdentifier);
            })
            .Returns(Task.CompletedTask);

        var documentContextFactory = new TestDocumentContextFactory(_openedDocument.FilePath, codeDocument);
        var translateDiagnosticsService = new RazorTranslateDiagnosticsService(StrictMock.Of<IDocumentMappingService>(), LoggerFactory);

        using var publisher = new TestRazorDiagnosticsPublisher(_projectManager, clientConnectionMock.Object, TestLanguageServerFeatureOptions.Instance, translateDiagnosticsService, documentContextFactory, LoggerFactory);
        var publisherAccessor = publisher.GetTestAccessor();
        publisherAccessor.SetPublishedDiagnostics(processedOpenDocument.FilePath, razorDiagnostics: [], csharpDiagnostics: null);

        // Act
        await publisherAccessor.PublishDiagnosticsAsync(processedOpenDocument, DisposalToken);

        // Assert
        clientConnectionMock.VerifyAll();
    }

    [Fact]
    public async Task PublishDiagnosticsAsync_NewCSharpDiagnosticsGetPublished()
    {
        // Arrange
        var codeDocument = CreateCodeDocument([]);
        var processedOpenDocument = TestDocumentSnapshot.Create(_openedDocument.FilePath, codeDocument);

        var arranging = true;
        var clientConnectionMock = new StrictMock<IClientConnection>();
        clientConnectionMock
            .Setup(server => server.SendRequestAsync<DocumentDiagnosticParams, SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?>(
                CustomMessageNames.RazorCSharpPullDiagnosticsEndpointName,
                It.IsAny<DocumentDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Callback((string method, DocumentDiagnosticParams @params, CancellationToken cancellationToken) =>
            {
                Assert.Equal(_openedDocumentUri, @params.TextDocument.Uri);
            })
            .Returns(Task.FromResult(
                new SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?(
                    arranging ? new FullDocumentDiagnosticReport() : new FullDocumentDiagnosticReport { Items = s_singleCSharpDiagnostic.ToArray() })));

        clientConnectionMock.Setup(
            server => server.SendNotificationAsync(
                "textDocument/publishDiagnostics",
                It.IsAny<PublishDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Callback((string method, PublishDiagnosticParams @params, CancellationToken cancellationToken) =>
            {
                Assert.Equal(processedOpenDocument.FilePath.TrimStart('/'), @params.Uri.AbsolutePath);
                Assert.True(processedOpenDocument.TryGetText(out var sourceText));
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

        var documentContextFactory = new TestDocumentContextFactory(_openedDocument.FilePath, codeDocument);
        var translateDiagnosticsService = new RazorTranslateDiagnosticsService(StrictMock.Of<IDocumentMappingService>(), LoggerFactory);

        using var publisher = new TestRazorDiagnosticsPublisher(_projectManager, clientConnectionMock.Object, TestLanguageServerFeatureOptions.Instance, translateDiagnosticsService, documentContextFactory, LoggerFactory);
        var publisherAccessor = publisher.GetTestAccessor();

        await publisherAccessor.PublishDiagnosticsAsync(processedOpenDocument, DisposalToken);
        arranging = false;

        // Act
        await publisherAccessor.PublishDiagnosticsAsync(processedOpenDocument, DisposalToken);

        // Assert
        clientConnectionMock.VerifyAll();
    }

    [Fact]
    public async Task PublishDiagnosticsAsync_NoopsIfRazorDiagnosticsAreSameAsPreviousPublish()
    {
        // Arrange
        var clientConnectionMock = new StrictMock<IClientConnection>();
        clientConnectionMock
            .Setup(server => server.SendRequestAsync<DocumentDiagnosticParams, SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?>(
                CustomMessageNames.RazorCSharpPullDiagnosticsEndpointName,
                It.IsAny<DocumentDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Callback((string method, DocumentDiagnosticParams @params, CancellationToken cancellationToken) =>
            {
                Assert.Equal(_openedDocumentUri, @params.TextDocument.Uri);
            })
            .Returns(Task.FromResult(new SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?(new FullDocumentDiagnosticReport())));

        var codeDocument = CreateCodeDocument(s_singleRazorDiagnostic);
        var processedOpenDocument = TestDocumentSnapshot.Create(_openedDocument.FilePath, codeDocument);

        var documentContextFactory = new TestDocumentContextFactory(_openedDocument.FilePath, codeDocument);
        var filePathService = new LSPFilePathService(TestLanguageServerFeatureOptions.Instance);
        var documentMappingService = new LspDocumentMappingService(filePathService, documentContextFactory, LoggerFactory);
        var translateDiagnosticsService = new RazorTranslateDiagnosticsService(documentMappingService, LoggerFactory);

        using var publisher = new TestRazorDiagnosticsPublisher(_projectManager, clientConnectionMock.Object, TestLanguageServerFeatureOptions.Instance, translateDiagnosticsService, documentContextFactory, LoggerFactory);
        var publisherAccessor = publisher.GetTestAccessor();
        publisherAccessor.SetPublishedDiagnostics(processedOpenDocument.FilePath, s_singleRazorDiagnostic, csharpDiagnostics: null);

        // Act & Assert
        await publisherAccessor.PublishDiagnosticsAsync(processedOpenDocument, DisposalToken);
    }

    [Fact]
    public async Task PublishDiagnosticsAsync_NoopsIfCSharpDiagnosticsAreSameAsPreviousPublish()
    {
        // Arrange
        var codeDocument = CreateCodeDocument([]);
        var processedOpenDocument = TestDocumentSnapshot.Create(_openedDocument.FilePath, codeDocument);

        var clientConnectionMock = new StrictMock<IClientConnection>();
        var arranging = true;

        clientConnectionMock
            .Setup(server => server.SendRequestAsync<DocumentDiagnosticParams, SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?>(
                CustomMessageNames.RazorCSharpPullDiagnosticsEndpointName,
                It.IsAny<DocumentDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Callback((string method, DocumentDiagnosticParams @params, CancellationToken cancellationToken) =>
            {
                Assert.Equal(_openedDocumentUri, @params.TextDocument.Uri);
            })
            .Returns(Task.FromResult(new SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?(new FullDocumentDiagnosticReport())));

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

                Assert.Equal(processedOpenDocument.FilePath.TrimStart('/'), @params.Uri.AbsolutePath);
                Assert.True(processedOpenDocument.TryGetText(out var sourceText));
                Assert.Empty(@params.Diagnostics);
            })
            .Returns(Task.CompletedTask);

        var documentContextFactory = new TestDocumentContextFactory(_openedDocument.FilePath, codeDocument);
        var translateDiagnosticsService = new RazorTranslateDiagnosticsService(StrictMock.Of<IDocumentMappingService>(), LoggerFactory);

        using var publisher = new TestRazorDiagnosticsPublisher(_projectManager, clientConnectionMock.Object, TestLanguageServerFeatureOptions.Instance, translateDiagnosticsService, documentContextFactory, LoggerFactory);
        var publisherAccessor = publisher.GetTestAccessor();

        await publisherAccessor.PublishDiagnosticsAsync(processedOpenDocument, DisposalToken);
        arranging = false;

        // Act & Assert
        await publisherAccessor.PublishDiagnosticsAsync(processedOpenDocument, DisposalToken);
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
                Assert.Equal(_closedDocument.FilePath.TrimStart('/'), @params.Uri.AbsolutePath);
                Assert.Empty(@params.Diagnostics);
            })
            .Returns(Task.CompletedTask);

        var documentContextFactory = new TestDocumentContextFactory();
        var translateDiagnosticsService = new RazorTranslateDiagnosticsService(StrictMock.Of<IDocumentMappingService>(), LoggerFactory);

        using var publisher = new TestRazorDiagnosticsPublisher(_projectManager, clientConnectionMock.Object, TestLanguageServerFeatureOptions.Instance, translateDiagnosticsService, documentContextFactory, LoggerFactory);
        var publisherAccessor = publisher.GetTestAccessor();
        publisherAccessor.SetPublishedDiagnostics(_closedDocument.FilePath, s_singleRazorDiagnostic, s_singleCSharpDiagnostic);

        // Act
        publisherAccessor.ClearClosedDocuments();

        // Assert
        clientConnectionMock.VerifyAll();
    }

    [Fact]
    public void ClearClosedDocuments_NoopsIfDocumentIsStillOpen()
    {
        // Arrange
        var clientConnectionMock = new StrictMock<IClientConnection>();
        var documentContextFactory = new TestDocumentContextFactory();
        var translateDiagnosticsService = new RazorTranslateDiagnosticsService(StrictMock.Of<IDocumentMappingService>(), LoggerFactory);

        using var publisher = new TestRazorDiagnosticsPublisher(_projectManager, clientConnectionMock.Object, TestLanguageServerFeatureOptions.Instance, translateDiagnosticsService, documentContextFactory, LoggerFactory);
        var publisherAccessor = publisher.GetTestAccessor();
        publisherAccessor.SetPublishedDiagnostics(_openedDocument.FilePath, s_singleRazorDiagnostic, s_singleCSharpDiagnostic);

        // Act & Assert
        publisherAccessor.ClearClosedDocuments();
    }

    [Fact]
    public void ClearClosedDocuments_NoopsIfDocumentIsClosedButNoDiagnostics()
    {
        // Arrange
        var clientConnectionMock = new StrictMock<IClientConnection>();
        var documentContextFactory = new TestDocumentContextFactory();
        var translateDiagnosticsService = new RazorTranslateDiagnosticsService(StrictMock.Of<IDocumentMappingService>(), LoggerFactory);

        using var publisher = new TestRazorDiagnosticsPublisher(_projectManager, clientConnectionMock.Object, TestLanguageServerFeatureOptions.Instance, translateDiagnosticsService, documentContextFactory, LoggerFactory);
        var publisherAccessor = publisher.GetTestAccessor();
        publisherAccessor.SetPublishedDiagnostics(_closedDocument.FilePath, razorDiagnostics: [], csharpDiagnostics: []);

        // Act & Assert
        publisherAccessor.ClearClosedDocuments();
    }

    [Fact]
    public void ClearClosedDocuments_RestartsTimerIfDocumentsStillOpen()
    {
        // Arrange
        var clientConnectionMock = new StrictMock<IClientConnection>();
        var documentContextFactory = new TestDocumentContextFactory();
        var translateDiagnosticsService = new RazorTranslateDiagnosticsService(StrictMock.Of<IDocumentMappingService>(), LoggerFactory);

        using var publisher = new TestRazorDiagnosticsPublisher(_projectManager, clientConnectionMock.Object, TestLanguageServerFeatureOptions.Instance, translateDiagnosticsService, documentContextFactory, LoggerFactory);
        var publisherAccessor = publisher.GetTestAccessor();
        publisherAccessor.SetPublishedDiagnostics(_closedDocument.FilePath, razorDiagnostics: [], csharpDiagnostics: []);
        publisherAccessor.SetPublishedDiagnostics(_openedDocument.FilePath, razorDiagnostics: [], csharpDiagnostics: []);

        // Act
        publisherAccessor.ClearClosedDocuments();

        // Assert
        Assert.True(publisherAccessor.IsWaitingToClearClosedDocuments);
    }

    private static RazorCodeDocument CreateCodeDocument(IEnumerable<RazorDiagnostic> diagnostics)
    {
        var codeDocument = TestRazorCodeDocument.Create("hello");
        var razorCSharpDocument = new RazorCSharpDocument(codeDocument, "hello", RazorCodeGenerationOptions.Default, diagnostics.ToImmutableArray());
        codeDocument.SetCSharpDocument(razorCSharpDocument);

        return codeDocument;
    }

    private class TestRazorDiagnosticsPublisher(
        IProjectSnapshotManager projectManager,
        IClientConnection clientConnection,
        LanguageServerFeatureOptions options,
        RazorTranslateDiagnosticsService translateDiagnosticsService,
        IDocumentContextFactory documentContextFactory,
        ILoggerFactory loggerFactory) : RazorDiagnosticsPublisher(projectManager, clientConnection, options,
              new Lazy<RazorTranslateDiagnosticsService>(() => translateDiagnosticsService),
              new Lazy<IDocumentContextFactory>(() => documentContextFactory),
              loggerFactory,
              publishDelay: TimeSpan.FromMilliseconds(1));
}
