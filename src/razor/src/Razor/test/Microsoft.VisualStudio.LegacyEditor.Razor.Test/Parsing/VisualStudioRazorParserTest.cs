// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Editor;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.Razor.ProjectSystem.Legacy;

using Microsoft.VisualStudio.Language.Intellisense;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Parsing;

public class VisualStudioRazorParserTest : VisualStudioTestBase
{
    private readonly ILegacyProjectSnapshot _projectSnapshot;
    private readonly IProjectEngineFactoryProvider _projectEngineFactoryProvider;
    private readonly CodeAnalysis.Workspace _workspace;

    public VisualStudioRazorParserTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _workspace = TestWorkspace.Create();
        AddDisposable(_workspace);

        var engine = RazorProjectEngine.Create(RazorConfiguration.Default, RazorProjectFileSystem.Empty);

        var factoryMock = new StrictMock<IProjectEngineFactory>();
        factoryMock
            .Setup(x => x.Create(It.IsAny<RazorConfiguration>(), It.IsAny<RazorProjectFileSystem>(), It.IsAny<Action<RazorProjectEngineBuilder>>()))
            .Returns(engine);

        var factoryProviderMock = new StrictMock<IProjectEngineFactoryProvider>();
        factoryProviderMock
            .Setup(x => x.GetFactory(It.IsAny<RazorConfiguration>()))
            .Returns(factoryMock.Object);

        _projectEngineFactoryProvider = factoryProviderMock.Object;

        _projectSnapshot = new EphemeralProjectSnapshot(_projectEngineFactoryProvider, "c:\\SomeProject.csproj");
    }

    private IVisualStudioDocumentTracker CreateDocumentTracker(bool isSupportedProject = true, int versionNumber = 0)
        => StrictMock.Of<IVisualStudioDocumentTracker>(tracker =>
            tracker.TextBuffer == new TestTextBuffer(new StringTextSnapshot(string.Empty, versionNumber), /* contentType */ null) &&
            tracker.ProjectPath == "c:\\SomeProject.csproj" &&
            tracker.ProjectSnapshot == _projectSnapshot &&
            tracker.FilePath == "c:\\SomeFilePath.cshtml" &&
            tracker.IsSupportedProject == isSupportedProject);

    private VisualStudioRazorParser CreateParser(IVisualStudioDocumentTracker documentTracker)
        => new(
            documentTracker,
            _projectEngineFactoryProvider,
            StrictMock.Of<ICompletionBroker>(),
            LoggerFactory,
            JoinableTaskContext);

    [UIFact]
    public async Task GetLatestCodeDocumentAsync_WaitsForParse()
    {
        // Arrange
        var documentTracker = CreateDocumentTracker();
        using var parser = CreateParser(documentTracker);
        var latestChange = new SourceChange(0, 0, string.Empty);
        var latestSnapshot = documentTracker.TextBuffer.CurrentSnapshot;
        parser._latestChangeReference = new BackgroundParser.ChangeReference(latestChange, latestSnapshot);
        var codeDocument = TestRazorCodeDocument.CreateEmpty();
        var syntaxTree = RazorSyntaxTree.Parse(TestRazorSourceDocument.Create());
        codeDocument.SetSyntaxTree(syntaxTree);
        var args = new BackgroundParserResultsReadyEventArgs(
            parser._latestChangeReference,
            codeDocument);

        // Act - 1
        var getLatestCodeDocumentTask = parser.GetLatestCodeDocumentAsync(StringTextSnapshot.Empty);

        // Assert - 1
        Assert.False(getLatestCodeDocumentTask.IsCompleted);

        // Act - 2
        await Task.Run(() => parser.OnResultsReady(sender: null!, args));

        // Assert - 2
        Assert.True(getLatestCodeDocumentTask.IsCompleted);

        // Act - 3
        var latestCodeDocument = await getLatestCodeDocumentTask;

        // Assert - 3
        Assert.Same(latestCodeDocument, codeDocument);
    }

    [UIFact]
    public async Task GetLatestCodeDocumentAsync_NoPendingChangesReturnsImmediately()
    {
        // Arrange
        var documentTracker = CreateDocumentTracker();
        using var parser = CreateParser(documentTracker);
        var latestChange = new SourceChange(0, 0, string.Empty);
        var latestSnapshot = documentTracker.TextBuffer.CurrentSnapshot;
        parser._latestChangeReference = new BackgroundParser.ChangeReference(latestChange, latestSnapshot);
        var codeDocument = TestRazorCodeDocument.CreateEmpty();
        var syntaxTree = RazorSyntaxTree.Parse(TestRazorSourceDocument.Create());
        codeDocument.SetSyntaxTree(syntaxTree);
        var args = new BackgroundParserResultsReadyEventArgs(
            parser._latestChangeReference,
            codeDocument);

        // Initialize the document with some content so we have a syntax tree to return.
        await Task.Run(() => parser.OnResultsReady(sender: null!, args));

        // Act - 1
        var getLatestCodeDocumentTask = parser.GetLatestCodeDocumentAsync(StringTextSnapshot.Empty);

        // Assert - 1
        Assert.True(getLatestCodeDocumentTask.IsCompleted);

        // Act - 2
        var latestCodeDocument = await getLatestCodeDocumentTask;

        // Assert - 2
        Assert.Same(latestCodeDocument, codeDocument);
    }

    [UIFact]
    public void GetLatestCodeDocumentAsync_MultipleCallsSameSnapshotMemoizesReturnedTasks()
    {
        // Arrange
        var documentTracker = CreateDocumentTracker();
        using var parser = CreateParser(documentTracker);
        var latestChange = new SourceChange(0, 0, string.Empty);
        var latestSnapshot = documentTracker.TextBuffer.CurrentSnapshot;
        parser._latestChangeReference = new BackgroundParser.ChangeReference(latestChange, latestSnapshot);
        var sameSnapshot = StringTextSnapshot.Empty;

        // Act
        var getLatestCodeDocumentTask1 = parser.GetLatestCodeDocumentAsync(sameSnapshot);
        var getLatestCodeDocumentTask2 = parser.GetLatestCodeDocumentAsync(sameSnapshot);

        // Assert
        Assert.Same(getLatestCodeDocumentTask1, getLatestCodeDocumentTask2);
    }

    [UIFact]
    public void GetLatestCodeDocumentAsync_MultipleCallsDifferentSnapshotsReturnDifferentTasks()
    {
        // Arrange
        var documentTracker = CreateDocumentTracker();
        using var parser = CreateParser(documentTracker);
        var latestChange = new SourceChange(0, 0, string.Empty);
        var latestSnapshot = documentTracker.TextBuffer.CurrentSnapshot;
        parser._latestChangeReference = new BackgroundParser.ChangeReference(latestChange, latestSnapshot);
        var snapshot1 = new StringTextSnapshot("Snapshot 1");
        var snapshot2 = new StringTextSnapshot("Snapshot 2");

        // Act
        var getLatestCodeDocumentTask1 = parser.GetLatestCodeDocumentAsync(snapshot1);
        var getLatestCodeDocumentTask2 = parser.GetLatestCodeDocumentAsync(snapshot2);

        // Assert
        Assert.NotSame(getLatestCodeDocumentTask1, getLatestCodeDocumentTask2);
    }

    [UIFact]
    public async Task GetLatestCodeDocumentAsync_LatestChangeIsNewerThenRequested_ReturnsImmediately()
    {
        // Arrange
        var documentTracker = CreateDocumentTracker(versionNumber: 1337);
        var olderSnapshot = new StringTextSnapshot("Older", versionNumber: 910);
        using var parser = CreateParser(documentTracker);
        var latestChange = new SourceChange(0, 0, string.Empty);
        var latestSnapshot = documentTracker.TextBuffer.CurrentSnapshot;
        parser._latestChangeReference = new BackgroundParser.ChangeReference(latestChange, latestSnapshot);
        var codeDocument = TestRazorCodeDocument.CreateEmpty();
        var syntaxTree = RazorSyntaxTree.Parse(TestRazorSourceDocument.Create());
        codeDocument.SetSyntaxTree(syntaxTree);
        var args = new BackgroundParserResultsReadyEventArgs(
            parser._latestChangeReference,
            codeDocument);

        // Initialize the document with some content so we have a syntax tree to return.
        await Task.Run(() => parser.OnResultsReady(sender: null!, args));

        // Act - 1
        var getLatestCodeDocumentTask = parser.GetLatestCodeDocumentAsync(olderSnapshot);

        // Assert - 1
        Assert.True(getLatestCodeDocumentTask.IsCompleted);

        // Act - 2
        var latestCodeDocument = await getLatestCodeDocumentTask;

        // Assert - 2
        Assert.Same(latestCodeDocument, codeDocument);
    }

    [UIFact]
    public async Task GetLatestCodeDocumentAsync_ParserDisposed_ReturnsImmediately()
    {
        // Arrange
        var documentTracker = CreateDocumentTracker();
        var codeDocument = TestRazorCodeDocument.CreateEmpty();
        var syntaxTree = RazorSyntaxTree.Parse(TestRazorSourceDocument.Create());
        VisualStudioRazorParser parser;
        codeDocument.SetSyntaxTree(syntaxTree);
        using (parser = CreateParser(documentTracker))
        {
            var latestChange = new SourceChange(0, 0, string.Empty);
            var latestSnapshot = documentTracker.TextBuffer.CurrentSnapshot;
            parser._latestChangeReference = new BackgroundParser.ChangeReference(latestChange, latestSnapshot);
            var args = new BackgroundParserResultsReadyEventArgs(
                parser._latestChangeReference,
                codeDocument);

            // Initialize the document with some content so we have a syntax tree to return.
            await Task.Run(() => parser.OnResultsReady(sender: null!, args));
        }

        var newerSnapshot = new StringTextSnapshot("Newer", versionNumber: 1337);

        // Act - 1
        var getLatestCodeDocumentTask = parser.GetLatestCodeDocumentAsync(newerSnapshot);

        // Assert - 1
        Assert.True(getLatestCodeDocumentTask.IsCompleted);

        // Act - 2
        var latestCodeDocument = await getLatestCodeDocumentTask;

        // Assert - 2
        Assert.Same(latestCodeDocument, codeDocument);
    }

    [UIFact]
    public void CodeDocumentRequest_Complete_CanBeCalledMultipleTimes()
    {
        // Arrange
        var source = TestRazorSourceDocument.Create();
        var codeDocument = RazorCodeDocument.Create(source);
        var request = new VisualStudioRazorParser.CodeDocumentRequest(StringTextSnapshot.Empty, DisposalToken);

        // Act & Assert
        request.Complete(codeDocument);
        request.Complete(codeDocument);
        request.Complete(codeDocument);
    }

    [UIFact]
    public async Task CodeDocumentRequest_Complete_FinishesTask()
    {
        // Arrange
        var source = TestRazorSourceDocument.Create();
        var codeDocument = RazorCodeDocument.Create(source);
        var request = new VisualStudioRazorParser.CodeDocumentRequest(StringTextSnapshot.Empty, DisposalToken);

        // Act
        request.Complete(codeDocument);

        // Assert
        Assert.True(request.Task.IsCompleted);
        var resolvedSyntaxTree = await request.Task;
        Assert.Same(codeDocument, resolvedSyntaxTree);
    }

    [UIFact]
    public void CodeDocumentRequest_Cancel_CanBeCalledMultipleTimes()
    {
        // Arrange
        var request = new VisualStudioRazorParser.CodeDocumentRequest(StringTextSnapshot.Empty, DisposalToken);

        // Act & Assert
        request.Cancel();
        request.Cancel();
        request.Cancel();
    }

    [UIFact]
    public void CodeDocumentRequest_Cancel_CancelsTask()
    {
        // Arrange
        var request = new VisualStudioRazorParser.CodeDocumentRequest(StringTextSnapshot.Empty, DisposalToken);

        // Act
        request.Cancel();

        // Assert
        Assert.True(request.Task.IsCanceled);
    }

    [UIFact]
    public void CodeDocumentRequest_LinkedTokenCancel_CancelsTask()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var request = new VisualStudioRazorParser.CodeDocumentRequest(StringTextSnapshot.Empty, cts.Token);

        // Act
        cts.Cancel();

        // Assert
        Assert.True(request.Task.IsCanceled);
    }

    [UIFact]
    public void CodeDocumentRequest_CompleteToCancelNoops()
    {
        // Arrange
        var source = TestRazorSourceDocument.Create();
        var codeDocument = RazorCodeDocument.Create(source);
        var request = new VisualStudioRazorParser.CodeDocumentRequest(StringTextSnapshot.Empty, DisposalToken);

        // Act - 1
        request.Complete(codeDocument);

        // Assert - 1
        Assert.True(request.Task.IsCompleted);

        // Act - 2
        request.Cancel();

        // Assert - 2
        Assert.False(request.Task.IsCanceled);
    }

    [UIFact]
    public void CodeDocumentRequest_CancelToCompleteNoops()
    {
        // Arrange
        var source = TestRazorSourceDocument.Create();
        var codeDocument = RazorCodeDocument.Create(source);
        var request = new VisualStudioRazorParser.CodeDocumentRequest(StringTextSnapshot.Empty, DisposalToken);

        // Act - 1
        request.Cancel();

        // Assert - 1
        Assert.True(request.Task.IsCanceled);

        // Act & Assert - 2
        request.Complete(codeDocument);
    }

    [UIFact]
    public void ReparseOnUIThread_NoopsIfDisposed()
    {
        // Arrange
        var parser = CreateParser(CreateDocumentTracker());
        parser.Dispose();

        // Act & Assert
        parser.ReparseOnUIThread();
    }

    [UIFact]
    public void OnIdle_NoopsIfDisposed()
    {
        // Arrange
        var parser = CreateParser(CreateDocumentTracker());
        parser.Dispose();

        // Act & Assert
        parser.OnIdle();
    }

    [UIFact]
    public void OnDocumentStructureChanged_NoopsIfDisposed()
    {
        // Arrange
        var parser = CreateParser(CreateDocumentTracker());
        parser.Dispose();

        // Act & Assert
        parser.OnDocumentStructureChanged(new object());
    }

    [UIFact]
    public void OnDocumentStructureChanged_IgnoresEditsThatAreOld()
    {
        // Arrange
        using var parser = CreateParser(CreateDocumentTracker());
        var called = false;
        parser.DocumentStructureChanged += (sender, e) => called = true;
        parser._latestChangeReference = new BackgroundParser.ChangeReference(null, new StringTextSnapshot(string.Empty));
        var args = new BackgroundParserResultsReadyEventArgs(
            new BackgroundParser.ChangeReference(new SourceChange(0, 0, string.Empty), new StringTextSnapshot(string.Empty)),
            TestRazorCodeDocument.CreateEmpty());

        // Act
        parser.OnDocumentStructureChanged(args);

        // Assert
        Assert.False(called);
    }

    [UIFact]
    public void OnDocumentStructureChanged_FiresForLatestTextBufferEdit()
    {
        // Arrange
        var documentTracker = CreateDocumentTracker();
        using var parser = CreateParser(documentTracker);
        var called = false;
        parser.DocumentStructureChanged += (sender, e) => called = true;
        var latestChange = new SourceChange(0, 0, string.Empty);
        var latestSnapshot = documentTracker.TextBuffer.CurrentSnapshot;
        parser._latestChangeReference = new BackgroundParser.ChangeReference(latestChange, latestSnapshot);
        var codeDocument = TestRazorCodeDocument.CreateEmpty();
        codeDocument.SetSyntaxTree(RazorSyntaxTree.Parse(TestRazorSourceDocument.Create()));
        var args = new BackgroundParserResultsReadyEventArgs(
            parser._latestChangeReference,
            codeDocument);

        // Act
        parser.OnDocumentStructureChanged(args);

        // Assert
        Assert.True(called);
    }

    [UIFact]
    public void OnDocumentStructureChanged_FiresForOnlyLatestTextBufferReparseEdit()
    {
        // Arrange
        var documentTracker = CreateDocumentTracker();
        using var parser = CreateParser(documentTracker);
        var called = false;
        parser.DocumentStructureChanged += (sender, e) => called = true;
        var latestSnapshot = documentTracker.TextBuffer.CurrentSnapshot;
        parser._latestChangeReference = new BackgroundParser.ChangeReference(null, latestSnapshot);
        var codeDocument = TestRazorCodeDocument.CreateEmpty();
        codeDocument.SetSyntaxTree(RazorSyntaxTree.Parse(TestRazorSourceDocument.Create()));
        var badArgs = new BackgroundParserResultsReadyEventArgs(
            // This is a different reparse edit, shouldn't be fired for this call
            new BackgroundParser.ChangeReference(null, latestSnapshot),
            codeDocument);
        var goodArgs = new BackgroundParserResultsReadyEventArgs(
            parser._latestChangeReference,
            codeDocument);

        // Act - 1
        parser.OnDocumentStructureChanged(badArgs);

        // Assert - 1
        Assert.False(called);

        // Act - 2
        parser.OnDocumentStructureChanged(goodArgs);

        // Assert - 2
        Assert.True(called);
    }

    [UIFact]
    public void StartIdleTimer_DoesNotRestartTimerWhenAlreadyRunning()
    {
        // Arrange
        using var parser = CreateParser(CreateDocumentTracker());
        parser.BlockBackgroundIdleWork = new ManualResetEventSlim();
        parser._idleDelay = TimeSpan.FromSeconds(5);
        parser.StartIdleTimer();
        using var currentTimer = parser._idleTimer;

        // Act
        parser.StartIdleTimer();
        var afterTimer = parser._idleTimer;

        // Assert
        Assert.NotNull(currentTimer);
        Assert.Same(currentTimer, afterTimer);
    }

    [UIFact]
    public void StopIdleTimer_StopsTimer()
    {
        // Arrange
        using var parser = CreateParser(CreateDocumentTracker());
        parser.BlockBackgroundIdleWork = new ManualResetEventSlim();
        parser._idleDelay = TimeSpan.FromSeconds(5);
        parser.StartIdleTimer();
        var currentTimer = parser._idleTimer;

        // Act
        parser.StopIdleTimer();

        // Assert
        Assert.NotNull(currentTimer);
        Assert.Null(parser._idleTimer);
    }

    [UIFact]
    public void StopParser_DetachesFromTextBufferChangeLoop()
    {
        // Arrange
        var documentTracker = CreateDocumentTracker();
        var textBuffer = (TestTextBuffer)documentTracker.TextBuffer;
        using var parser = CreateParser(documentTracker);
        parser.StartParser();

        // Act
        parser.StopParser();

        // Assert
        Assert.Empty(textBuffer.AttachedChangedEvents);
        Assert.Null(parser._parser);
    }

    [UIFact]
    public void StartParser_AttachesToTextBufferChangeLoop()
    {
        // Arrange
        var documentTracker = CreateDocumentTracker();
        var textBuffer = (TestTextBuffer)documentTracker.TextBuffer;
        using var parser = CreateParser(documentTracker);

        // Act
        parser.StartParser();

        // Assert
        Assert.Single(textBuffer.AttachedChangedEvents);
        Assert.NotNull(parser._parser);
    }

    [UIFact]
    public void TryReinitializeParser_ReturnsTrue_IfProjectIsSupported()
    {
        // Arrange
        using var parser = CreateParser(CreateDocumentTracker(isSupportedProject: true));

        // Act
        var result = parser.TryReinitializeParser();

        // Assert
        Assert.True(result);
    }

    [UIFact]
    public void TryReinitializeParser_ReturnsFalse_IfProjectIsNotSupported()
    {
        // Arrange
        using var parser = CreateParser(CreateDocumentTracker(isSupportedProject: false));

        // Act
        var result = parser.TryReinitializeParser();

        // Assert
        Assert.False(result);
    }
}
