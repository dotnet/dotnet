// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;

using Microsoft.VisualStudio.LegacyEditor.Razor.Settings;
using Microsoft.VisualStudio.Razor.Settings;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LegacyEditor.Razor;

public class RazorDocumentManagerTest : VisualStudioTestBase
{
    private const string FilePath = "C:/Some/Path/TestDocumentTracker.cshtml";
    private const string ProjectPath = "C:/Some/Path/TestProject.csproj";

    private readonly TestProjectSnapshotManager _projectManager;
    private readonly IWorkspaceEditorSettings _workspaceEditorSettings;
    private readonly IImportDocumentManager _importDocumentManager;

    public RazorDocumentManagerTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _projectManager = CreateProjectSnapshotManager();

        _workspaceEditorSettings = new WorkspaceEditorSettings(
            StrictMock.Of<IClientSettingsManager>());

        var importDocumentManagerMock = new StrictMock<IImportDocumentManager>();
        importDocumentManagerMock
            .Setup(m => m.OnSubscribed(It.IsAny<IVisualStudioDocumentTracker>()))
            .Verifiable();
        importDocumentManagerMock
            .Setup(m => m.OnUnsubscribed(It.IsAny<IVisualStudioDocumentTracker>()))
            .Verifiable();
        _importDocumentManager = importDocumentManagerMock.Object;
    }

    [UIFact]
    public async Task OnTextViewOpened_ForNonRazorTextBuffer_DoesNothing()
    {
        // Arrange
        var editorFactoryService = StrictMock.Of<IRazorEditorFactoryService>();
        var documentManager = new RazorDocumentManager(editorFactoryService, JoinableTaskContext);
        var textView = StrictMock.Of<ITextView>();
        var nonCoreTextBuffer = VsMocks.CreateTextBuffer(core: false);

        // Act & Assert
        await documentManager.OnTextViewOpenedAsync(textView, [nonCoreTextBuffer]);
    }

    [UIFact]
    public async Task OnTextViewOpened_ForRazorTextBuffer_AddsTextViewToTracker()
    {
        // Arrange
        var textView = StrictMock.Of<ITextView>();
        var coreTextBuffer = VsMocks.CreateTextBuffer(core: true);

        IVisualStudioDocumentTracker? documentTracker = new VisualStudioDocumentTracker(
            JoinableTaskContext,
            FilePath,
            ProjectPath,
            _projectManager,
            _workspaceEditorSettings,
            ProjectEngineFactories.DefaultProvider,
            coreTextBuffer,
            _importDocumentManager);
        var editorFactoryService = StrictMock.Of<IRazorEditorFactoryService>(f =>
            f.TryGetDocumentTracker(coreTextBuffer, out documentTracker) == true);
        var documentManager = new RazorDocumentManager(editorFactoryService, JoinableTaskContext);

        // Act
        await documentManager.OnTextViewOpenedAsync(textView, [coreTextBuffer]);

        // Assert
        Assert.Same(Assert.Single(documentTracker.TextViews), textView);
    }

    [UIFact]
    public async Task OnTextViewOpened_SubscribesAfterFirstTextViewOpened()
    {
        // Arrange
        var textView = StrictMock.Of<ITextView>();
        var coreTextBuffer = VsMocks.CreateTextBuffer(core: true);
        var nonCoreTextBuffer = VsMocks.CreateTextBuffer(core: false);

        IVisualStudioDocumentTracker? documentTracker = new VisualStudioDocumentTracker(
            JoinableTaskContext,
            FilePath,
            ProjectPath,
            _projectManager,
            _workspaceEditorSettings,
            ProjectEngineFactories.DefaultProvider,
            coreTextBuffer,
            _importDocumentManager);
        var editorFactoryService = StrictMock.Of<IRazorEditorFactoryService>(f =>
            f.TryGetDocumentTracker(It.IsAny<ITextBuffer>(), out documentTracker) == true);
        var documentManager = new RazorDocumentManager(editorFactoryService, JoinableTaskContext);

        // Assert 1
        Assert.False(documentTracker.IsSupportedProject);

        // Act
        await documentManager.OnTextViewOpenedAsync(textView, [coreTextBuffer, nonCoreTextBuffer]);

        // Assert 2
        Assert.True(documentTracker.IsSupportedProject);
    }

    [UIFact]
    public async Task OnTextViewClosed_TextViewWithoutDocumentTracker_DoesNothing()
    {
        // Arrange
        var documentManager = new RazorDocumentManager(StrictMock.Of<IRazorEditorFactoryService>(), JoinableTaskContext);
        var textView = StrictMock.Of<ITextView>();
        var coreTextBuffer = VsMocks.CreateTextBuffer(core: true);

        // Act
        await documentManager.OnTextViewClosedAsync(textView, [coreTextBuffer]);

        // Assert
        Assert.False(coreTextBuffer.Properties.ContainsProperty(typeof(IVisualStudioDocumentTracker)));
    }

    [UIFact]
    public async Task OnTextViewClosed_ForAnyTextBufferWithTracker_RemovesTextView()
    {
        // Arrange
        var textView1 = StrictMock.Of<ITextView>();
        var textView2 = StrictMock.Of<ITextView>();
        var coreTextBuffer = VsMocks.CreateTextBuffer(core: true);
        var nonCoreTextBuffer = VsMocks.CreateTextBuffer(core: false);

        // Preload the buffer's properties with a tracker, so it's like we've already tracked this one.
        var documentTracker = new VisualStudioDocumentTracker(
            JoinableTaskContext,
            FilePath,
            ProjectPath,
            _projectManager,
            _workspaceEditorSettings,
            ProjectEngineFactories.DefaultProvider,
            coreTextBuffer,
            _importDocumentManager);
        documentTracker.AddTextView(textView1);
        documentTracker.AddTextView(textView2);
        coreTextBuffer.Properties.AddProperty(typeof(IVisualStudioDocumentTracker), documentTracker);

        documentTracker = new VisualStudioDocumentTracker(
            JoinableTaskContext, FilePath, ProjectPath, _projectManager, _workspaceEditorSettings,
            ProjectEngineFactories.DefaultProvider, nonCoreTextBuffer, _importDocumentManager);
        documentTracker.AddTextView(textView1);
        documentTracker.AddTextView(textView2);
        nonCoreTextBuffer.Properties.AddProperty(typeof(IVisualStudioDocumentTracker), documentTracker);

        var editorFactoryService = StrictMock.Of<IRazorEditorFactoryService>();
        var documentManager = new RazorDocumentManager(editorFactoryService, JoinableTaskContext);

        // Act
        await documentManager.OnTextViewClosedAsync(textView2, [coreTextBuffer, nonCoreTextBuffer]);

        // Assert
        documentTracker = coreTextBuffer.Properties.GetProperty<VisualStudioDocumentTracker>(typeof(IVisualStudioDocumentTracker));
        Assert.Same(Assert.Single(documentTracker.TextViews), textView1);

        documentTracker = nonCoreTextBuffer.Properties.GetProperty<VisualStudioDocumentTracker>(typeof(IVisualStudioDocumentTracker));
        Assert.Same(Assert.Single(documentTracker.TextViews), textView1);
    }

    [UIFact]
    public async Task OnTextViewClosed_UnsubscribesAfterLastTextViewClosed()
    {
        // Arrange
        var textView1 = StrictMock.Of<ITextView>();
        var textView2 = StrictMock.Of<ITextView>();
        var coreTextBuffer = VsMocks.CreateTextBuffer(core: true);
        var nonCoreTextBuffer = VsMocks.CreateTextBuffer(core: false);

        var documentTracker = new VisualStudioDocumentTracker(
            JoinableTaskContext,
            FilePath,
            ProjectPath,
            _projectManager,
            _workspaceEditorSettings,
            ProjectEngineFactories.DefaultProvider,
            coreTextBuffer,
            _importDocumentManager);

        coreTextBuffer.Properties.AddProperty(typeof(IVisualStudioDocumentTracker), documentTracker);
        var editorFactoryService = StrictMock.Of<IRazorEditorFactoryService>();
        var documentManager = new RazorDocumentManager(editorFactoryService, JoinableTaskContext);

        // Populate the text views
        documentTracker.Subscribe();

        documentTracker.AddTextView(textView1);
        documentTracker.AddTextView(textView2);

        // Act 1
        await documentManager.OnTextViewClosedAsync(textView2, [coreTextBuffer, nonCoreTextBuffer]);

        // Assert 1
        Assert.True(documentTracker.IsSupportedProject);

        // Act
        await documentManager.OnTextViewClosedAsync(textView1, [coreTextBuffer, nonCoreTextBuffer]);

        // Assert 2
        Assert.False(documentTracker.IsSupportedProject);
    }
}
