// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Logging;
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
    private static readonly RazorDiagnostic[] s_emptyRazorDiagnostics = Array.Empty<RazorDiagnostic>();

    private static readonly RazorDiagnostic[] s_singleRazorDiagnostic = new RazorDiagnostic[]
    {
        RazorDiagnosticFactory.CreateDirective_BlockDirectiveCannotBeImported("test")
    };

    private static readonly Diagnostic[] s_emptyCSharpDiagnostics = Array.Empty<Diagnostic>();
    private static readonly Diagnostic[] s_singleCSharpDiagnostic = new Diagnostic[]
    {
        new Diagnostic()
        {
            Code = "TestCode",
            Severity = DiagnosticSeverity.Error,
            Message = "TestMessage",
            Range = new Range()
            {
                Start = new Position(0,0),
                End = new Position(0, 1)
            }
        }
    };

    private readonly ProjectSnapshotManager _projectManager;
    private readonly IDocumentSnapshot _closedDocument;
    private readonly IDocumentSnapshot _openedDocument;
    private readonly RazorCodeDocument _testCodeDocument;
    private readonly Uri _openedDocumentUri;

    public RazorDiagnosticsPublisherTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        var testProjectManager = TestProjectSnapshotManager.Create(ErrorReporter, Dispatcher);
        var hostProject = new HostProject("C:/project/project.csproj", "C:/project/obj", RazorConfiguration.Default, "TestRootNamespace");
        testProjectManager.ProjectAdded(hostProject);
        var sourceText = SourceText.From(string.Empty);
        var textAndVersion = TextAndVersion.Create(sourceText, VersionStamp.Default);
        var openedHostDocument = new HostDocument("C:/project/open_document.cshtml", "C:/project/open_document.cshtml");
        testProjectManager.DocumentAdded(hostProject.Key, openedHostDocument, TextLoader.From(textAndVersion));
        testProjectManager.DocumentOpened(hostProject.Key, openedHostDocument.FilePath, sourceText);
        var closedHostDocument = new HostDocument("C:/project/closed_document.cshtml", "C:/project/closed_document.cshtml");
        testProjectManager.DocumentAdded(hostProject.Key, closedHostDocument, TextLoader.From(textAndVersion));

        var openedDocument = testProjectManager.GetProjects()[0].GetDocument(openedHostDocument.FilePath);
        Assert.NotNull(openedDocument);
        _openedDocument = openedDocument;
        _openedDocumentUri = new Uri("C:/project/open_document.cshtml");

        var closedDocument = testProjectManager.GetProjects()[0].GetDocument(closedHostDocument.FilePath);
        Assert.NotNull(closedDocument);
        _closedDocument = closedDocument;

        _projectManager = testProjectManager;
        _testCodeDocument = TestRazorCodeDocument.CreateEmpty();
    }

    [Fact]
    public void DocumentProcessed_NewWorkQueued_RestartsTimer()
    {
        // Arrange
        Assert.NotNull(_openedDocument.FilePath);
        var processedOpenDocument = TestDocumentSnapshot.Create(_openedDocument.FilePath);
        var codeDocument = CreateCodeDocument(s_singleRazorDiagnostic);
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
        Mock.Get(languageServerDocument)
            .Setup(d => d.SendRequestAsync<DocumentDiagnosticParams, SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?>(
                CustomMessageNames.RazorCSharpPullDiagnosticsEndpointName,
                It.IsAny<DocumentDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?(new FullDocumentDiagnosticReport())))
            .Verifiable();

        var documentContextFactory = new TestDocumentContextFactory(_openedDocument.FilePath, codeDocument);
        var translateDiagnosticsService = new RazorTranslateDiagnosticsService(Mock.Of<IRazorDocumentMappingService>(MockBehavior.Strict), LoggerFactory);

        using var publisher = new TestRazorDiagnosticsPublisher(LegacyDispatcher, languageServerDocument, TestLanguageServerFeatureOptions.Instance, translateDiagnosticsService, documentContextFactory, LoggerFactory)
        {
            BlockBackgroundWorkCompleting = new ManualResetEventSlim(initialState: true),
            NotifyBackgroundWorkCompleting = new ManualResetEventSlim(initialState: false),
        };

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

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    public async Task PublishDiagnosticsAsync_NewDocumentDiagnosticsGetPublished(bool shouldContainCSharpDiagnostic, bool shouldContainRazorDiagnostic)
    {
        // Arrange
        var singleCSharpDiagnostic = new Diagnostic[]
        {
            new Diagnostic()
            {
                Code = "TestCode",
                Severity = DiagnosticSeverity.Error,
                Message = "TestMessage",
                Range = new Range()
                {
                    Start = new Position(0,0),
                    End = new Position(0, 1)
                }
            }
        };

        Assert.NotNull(_openedDocument.FilePath);
        var processedOpenDocument = TestDocumentSnapshot.Create(_openedDocument.FilePath);
        var codeDocument = CreateCodeDocument(shouldContainRazorDiagnostic ? s_singleRazorDiagnostic : s_emptyRazorDiagnostics);
        processedOpenDocument.With(codeDocument);

        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        var requestResult = new FullDocumentDiagnosticReport();
        if (shouldContainCSharpDiagnostic)
        {
            requestResult.Items = singleCSharpDiagnostic;
        }

        languageServer
            .Setup(server => server.SendRequestAsync<DocumentDiagnosticParams, SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?>(
                CustomMessageNames.RazorCSharpPullDiagnosticsEndpointName,
                It.IsAny<DocumentDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, DocumentDiagnosticParams, CancellationToken>((method, @params, cancellationToken) =>
            {
                Assert.Equal(_openedDocumentUri, @params.TextDocument.Uri);
            })
            .Returns(Task.FromResult(new SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?(requestResult)));
        languageServer
            .Setup(server => server.SendNotificationAsync(
                "textDocument/publishDiagnostics",
                It.IsAny<PublishDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, PublishDiagnosticParams, CancellationToken>((method, @params, cancellationToken) =>
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
        var filePathService = new FilePathService(TestLanguageServerFeatureOptions.Instance);
        var documentMappingService = new RazorDocumentMappingService(filePathService, documentContextFactory, LoggerFactory);
        var translateDiagnosticsService = new RazorTranslateDiagnosticsService(documentMappingService, LoggerFactory);

        using var publisher = new TestRazorDiagnosticsPublisher(LegacyDispatcher, languageServer.Object, TestLanguageServerFeatureOptions.Instance, translateDiagnosticsService, documentContextFactory, LoggerFactory);
        publisher.Initialize(_projectManager);

        // Act
        await publisher.PublishDiagnosticsAsync(processedOpenDocument);

        // Assert
        languageServer.VerifyAll();
    }

    [Fact]
    public async Task PublishDiagnosticsAsync_NewRazorDiagnosticsGetPublished()
    {
        // Arrange
        Assert.NotNull(_openedDocument.FilePath);
        var processedOpenDocument = TestDocumentSnapshot.Create(_openedDocument.FilePath);
        var codeDocument = CreateCodeDocument(s_singleRazorDiagnostic);
        processedOpenDocument.With(codeDocument);
        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        languageServer
            .Setup(server => server.SendRequestAsync<DocumentDiagnosticParams, SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?>(
                CustomMessageNames.RazorCSharpPullDiagnosticsEndpointName,
                It.IsAny<DocumentDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, DocumentDiagnosticParams, CancellationToken>((method, @params, cancellationToken) =>
            {
                Assert.Equal(_openedDocumentUri, @params.TextDocument.Uri);
            })
            .Returns(Task.FromResult(new SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?(new FullDocumentDiagnosticReport())));

        languageServer
            .Setup(server => server.SendNotificationAsync(
                "textDocument/publishDiagnostics",
                It.IsAny<PublishDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, PublishDiagnosticParams, CancellationToken>((method, @params, cancellationToken) =>
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
        var translateDiagnosticsService = new RazorTranslateDiagnosticsService(Mock.Of<IRazorDocumentMappingService>(MockBehavior.Strict), LoggerFactory);

        using var publisher = new TestRazorDiagnosticsPublisher(LegacyDispatcher, languageServer.Object, TestLanguageServerFeatureOptions.Instance, translateDiagnosticsService, documentContextFactory, LoggerFactory);
        publisher.PublishedRazorDiagnostics[processedOpenDocument.FilePath] = s_emptyRazorDiagnostics;
        publisher.Initialize(_projectManager);

        // Act
        await publisher.PublishDiagnosticsAsync(processedOpenDocument);

        // Assert
        languageServer.VerifyAll();
    }

    [Fact]
    public async Task PublishDiagnosticsAsync_NewCSharpDiagnosticsGetPublished()
    {
        // Arrange
        Assert.NotNull(_openedDocument.FilePath);
        var processedOpenDocument = TestDocumentSnapshot.Create(_openedDocument.FilePath);
        var codeDocument = CreateCodeDocument(s_emptyRazorDiagnostics);
        processedOpenDocument.With(codeDocument);
        var arranging = true;
        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        languageServer
            .Setup(server => server.SendRequestAsync<DocumentDiagnosticParams, SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?>(
                CustomMessageNames.RazorCSharpPullDiagnosticsEndpointName,
                It.IsAny<DocumentDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, DocumentDiagnosticParams, CancellationToken>((method, @params, cancellationToken) =>
            {
                Assert.Equal(_openedDocumentUri, @params.TextDocument.Uri);
            })
            .Returns(Task.FromResult(
                new SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?(
                    arranging ? new FullDocumentDiagnosticReport() : new FullDocumentDiagnosticReport { Items = s_singleCSharpDiagnostic })));

        languageServer.Setup(
            server => server.SendNotificationAsync(
                "textDocument/publishDiagnostics",
                It.IsAny<PublishDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, PublishDiagnosticParams, CancellationToken>((method, @params, cancellationToken) =>
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
        var translateDiagnosticsService = new RazorTranslateDiagnosticsService(Mock.Of<IRazorDocumentMappingService>(MockBehavior.Strict), LoggerFactory);

        using var publisher = new TestRazorDiagnosticsPublisher(LegacyDispatcher, languageServer.Object, TestLanguageServerFeatureOptions.Instance, translateDiagnosticsService, documentContextFactory, LoggerFactory);
        publisher.Initialize(_projectManager);
        await publisher.PublishDiagnosticsAsync(processedOpenDocument);
        arranging = false;

        // Act
        await publisher.PublishDiagnosticsAsync(processedOpenDocument);

        // Assert
        languageServer.VerifyAll();
    }

    [Fact]
    public async Task PublishDiagnosticsAsync_NoopsIfRazorDiagnosticsAreSameAsPreviousPublish()
    {
        // Arrange
        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        languageServer
            .Setup(server => server.SendRequestAsync<DocumentDiagnosticParams, SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?>(
                CustomMessageNames.RazorCSharpPullDiagnosticsEndpointName,
                It.IsAny<DocumentDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, DocumentDiagnosticParams, CancellationToken>((method, @params, cancellationToken) =>
            {
                Assert.Equal(_openedDocumentUri, @params.TextDocument.Uri);
            })
            .Returns(Task.FromResult(new SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?(new FullDocumentDiagnosticReport())));
        Assert.NotNull(_openedDocument.FilePath);
        var processedOpenDocument = TestDocumentSnapshot.Create(_openedDocument.FilePath);
        var codeDocument = CreateCodeDocument(s_singleRazorDiagnostic);
        processedOpenDocument.With(codeDocument);

        var documentContextFactory = new TestDocumentContextFactory(_openedDocument.FilePath, codeDocument);
        var filePathService = new FilePathService(TestLanguageServerFeatureOptions.Instance);
        var documentMappingService = new RazorDocumentMappingService(filePathService, documentContextFactory, LoggerFactory);
        var translateDiagnosticsService = new RazorTranslateDiagnosticsService(documentMappingService, LoggerFactory);

        using var publisher = new TestRazorDiagnosticsPublisher(LegacyDispatcher, languageServer.Object, TestLanguageServerFeatureOptions.Instance, translateDiagnosticsService, documentContextFactory, LoggerFactory);
        publisher.PublishedRazorDiagnostics[processedOpenDocument.FilePath] = s_singleRazorDiagnostic;
        publisher.Initialize(_projectManager);

        // Act & Assert
        await publisher.PublishDiagnosticsAsync(processedOpenDocument);
    }

    [Fact]
    public async Task PublishDiagnosticsAsync_NoopsIfCSharpDiagnosticsAreSameAsPreviousPublish()
    {
        // Arrange
        Assert.NotNull(_openedDocument.FilePath);
        var processedOpenDocument = TestDocumentSnapshot.Create(_openedDocument.FilePath);
        var codeDocument = CreateCodeDocument(s_emptyRazorDiagnostics);
        processedOpenDocument.With(codeDocument);
        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        var arranging = true;

        languageServer
            .Setup(server => server.SendRequestAsync<DocumentDiagnosticParams, SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?>(
                CustomMessageNames.RazorCSharpPullDiagnosticsEndpointName,
                It.IsAny<DocumentDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, DocumentDiagnosticParams, CancellationToken>((method, @params, cancellationToken) =>
            {
                Assert.Equal(_openedDocumentUri, @params.TextDocument.Uri);
            })
            .Returns(Task.FromResult(new SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?(new FullDocumentDiagnosticReport())));

        languageServer
            .Setup(server => server.SendNotificationAsync(
                "textDocument/publishDiagnostics",
                It.IsAny<PublishDiagnosticParams>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, PublishDiagnosticParams, CancellationToken>((method, @params, cancellationToken) =>
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
        var translateDiagnosticsService = new RazorTranslateDiagnosticsService(Mock.Of<IRazorDocumentMappingService>(MockBehavior.Strict), LoggerFactory);

        using var publisher = new TestRazorDiagnosticsPublisher(LegacyDispatcher, languageServer.Object, TestLanguageServerFeatureOptions.Instance, translateDiagnosticsService, documentContextFactory, LoggerFactory);
        publisher.Initialize(_projectManager);
        await publisher.PublishDiagnosticsAsync(processedOpenDocument);
        arranging = false;

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
                Assert.NotNull(_closedDocument.FilePath);
                Assert.Equal(_closedDocument.FilePath.TrimStart('/'), @params.Uri.AbsolutePath);
                Assert.Empty(@params.Diagnostics);
            })
            .Returns(Task.CompletedTask);

        var documentContextFactory = new TestDocumentContextFactory();
        var translateDiagnosticsService = new RazorTranslateDiagnosticsService(Mock.Of<IRazorDocumentMappingService>(MockBehavior.Strict), LoggerFactory);

        using var publisher = new TestRazorDiagnosticsPublisher(LegacyDispatcher, languageServer.Object, TestLanguageServerFeatureOptions.Instance, translateDiagnosticsService, documentContextFactory, LoggerFactory);
        Assert.NotNull(_closedDocument.FilePath);
        publisher.PublishedRazorDiagnostics[_closedDocument.FilePath] = s_singleRazorDiagnostic;
        publisher.PublishedCSharpDiagnostics[_closedDocument.FilePath] = s_singleCSharpDiagnostic;
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
        var documentContextFactory = new TestDocumentContextFactory();
        var translateDiagnosticsService = new RazorTranslateDiagnosticsService(Mock.Of<IRazorDocumentMappingService>(MockBehavior.Strict), LoggerFactory);

        using var publisher = new TestRazorDiagnosticsPublisher(LegacyDispatcher, languageServer.Object, TestLanguageServerFeatureOptions.Instance, translateDiagnosticsService, documentContextFactory, LoggerFactory);
        Assert.NotNull(_openedDocument.FilePath);
        publisher.PublishedRazorDiagnostics[_openedDocument.FilePath] = s_singleRazorDiagnostic;
        publisher.PublishedCSharpDiagnostics[_openedDocument.FilePath] = s_singleCSharpDiagnostic;
        publisher.Initialize(_projectManager);

        // Act & Assert
        publisher.ClearClosedDocuments();
    }

    [Fact]
    public void ClearClosedDocuments_NoopsIfDocumentIsClosedButNoDiagnostics()
    {
        // Arrange
        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        var documentContextFactory = new TestDocumentContextFactory();
        var translateDiagnosticsService = new RazorTranslateDiagnosticsService(Mock.Of<IRazorDocumentMappingService>(MockBehavior.Strict), LoggerFactory);

        using var publisher = new TestRazorDiagnosticsPublisher(LegacyDispatcher, languageServer.Object, TestLanguageServerFeatureOptions.Instance, translateDiagnosticsService, documentContextFactory, LoggerFactory);
        Assert.NotNull(_closedDocument.FilePath);
        publisher.PublishedRazorDiagnostics[_closedDocument.FilePath] = s_emptyRazorDiagnostics;
        publisher.PublishedCSharpDiagnostics[_closedDocument.FilePath] = s_emptyCSharpDiagnostics;
        publisher.Initialize(_projectManager);

        // Act & Assert
        publisher.ClearClosedDocuments();
    }

    [Fact]
    public void ClearClosedDocuments_RestartsTimerIfDocumentsStillOpen()
    {
        // Arrange
        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);
        var documentContextFactory = new TestDocumentContextFactory();
        var translateDiagnosticsService = new RazorTranslateDiagnosticsService(Mock.Of<IRazorDocumentMappingService>(MockBehavior.Strict), LoggerFactory);

        using var publisher = new TestRazorDiagnosticsPublisher(LegacyDispatcher, languageServer.Object, TestLanguageServerFeatureOptions.Instance, translateDiagnosticsService, documentContextFactory, LoggerFactory);
        Assert.NotNull(_closedDocument.FilePath);
        Assert.NotNull(_openedDocument.FilePath);
        publisher.PublishedRazorDiagnostics[_closedDocument.FilePath] = s_emptyRazorDiagnostics;
        publisher.PublishedCSharpDiagnostics[_closedDocument.FilePath] = s_emptyCSharpDiagnostics;
        publisher.PublishedRazorDiagnostics[_openedDocument.FilePath] = s_emptyRazorDiagnostics;
        publisher.PublishedCSharpDiagnostics[_openedDocument.FilePath] = s_emptyCSharpDiagnostics;
        publisher.Initialize(_projectManager);

        // Act
        publisher.ClearClosedDocuments();

        // Assert
        Assert.NotNull(publisher._documentClosedTimer);
    }

    private static RazorCodeDocument CreateCodeDocument(params RazorDiagnostic[] diagnostics)
    {
        var codeDocument = TestRazorCodeDocument.Create("hello");
        var razorCSharpDocument = RazorCSharpDocument.Create(codeDocument, "hello", RazorCodeGenerationOptions.CreateDefault(), diagnostics);
        codeDocument.SetCSharpDocument(razorCSharpDocument);

        return codeDocument;
    }

    private class TestRazorDiagnosticsPublisher : RazorDiagnosticsPublisher, IDisposable
    {
        public TestRazorDiagnosticsPublisher(
            ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
            ClientNotifierServiceBase languageServer,
            LanguageServerFeatureOptions languageServerFeatureOptions,
            RazorTranslateDiagnosticsService razorTranslateDiagnosticsService,
            DocumentContextFactory documentContextFactory,
            ILoggerFactory loggerFactory)
            : base(projectSnapshotManagerDispatcher, languageServer, languageServerFeatureOptions, new Lazy<RazorTranslateDiagnosticsService>(razorTranslateDiagnosticsService), new Lazy<DocumentContextFactory>(documentContextFactory), loggerFactory)
        {
            // The diagnostics publisher by default will wait 2 seconds until publishing diagnostics. For testing purposes we reduce
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
