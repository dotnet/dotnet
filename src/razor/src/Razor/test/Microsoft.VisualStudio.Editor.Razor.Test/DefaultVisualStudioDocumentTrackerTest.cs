// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Editor;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Editor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.Editor.Razor.Documents;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Editor.Razor;

public class DefaultVisualStudioDocumentTrackerTest : ProjectSnapshotManagerDispatcherWorkspaceTestBase
{
    private readonly IContentType _razorCoreContentType;
    private readonly ITextBuffer _textBuffer;
    private readonly string _filePath;
    private readonly string _projectPath;
    private readonly string _rootNamespace;
    private readonly HostProject _hostProject;
    private readonly HostProject _updatedHostProject;
    private readonly HostProject _otherHostProject;
    private Project _workspaceProject;
    private readonly ImportDocumentManager _importDocumentManager;
    private readonly WorkspaceEditorSettings _workspaceEditorSettings;
    private readonly List<TagHelperDescriptor> _someTagHelpers;
    private TestTagHelperResolver _tagHelperResolver;
    private readonly ProjectSnapshotManagerBase _projectManager;
    private readonly DefaultVisualStudioDocumentTracker _documentTracker;

    public DefaultVisualStudioDocumentTrackerTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _razorCoreContentType = Mock.Of<IContentType>(c => c.IsOfType(RazorLanguage.ContentType) && c.IsOfType(RazorConstants.LegacyContentType), MockBehavior.Strict);
        _textBuffer = Mock.Of<ITextBuffer>(b => b.ContentType == _razorCoreContentType, MockBehavior.Strict);

        _filePath = TestProjectData.SomeProjectFile1.FilePath;
        _projectPath = TestProjectData.SomeProject.FilePath;
        _rootNamespace = TestProjectData.SomeProject.RootNamespace;

        _importDocumentManager = new Mock<ImportDocumentManager>(MockBehavior.Strict).Object;
        Mock.Get(_importDocumentManager).Setup(m => m.OnSubscribed(It.IsAny<VisualStudioDocumentTracker>())).Verifiable();
        Mock.Get(_importDocumentManager).Setup(m => m.OnUnsubscribed(It.IsAny<VisualStudioDocumentTracker>())).Verifiable();

        _workspaceEditorSettings = new DefaultWorkspaceEditorSettings(Mock.Of<IClientSettingsManager>(MockBehavior.Strict));

        _someTagHelpers = new List<TagHelperDescriptor>()
        {
            TagHelperDescriptorBuilder.Create("test", "test").Build(),
        };

        _projectManager = new TestProjectSnapshotManager(Workspace, ProjectEngineFactoryProvider, Dispatcher) { AllowNotifyListeners = true };

        _hostProject = new HostProject(_projectPath, TestProjectData.SomeProject.IntermediateOutputPath, FallbackRazorConfiguration.MVC_2_1, _rootNamespace);
        _updatedHostProject = new HostProject(_projectPath, TestProjectData.SomeProject.IntermediateOutputPath, FallbackRazorConfiguration.MVC_2_0, _rootNamespace);
        _otherHostProject = new HostProject(TestProjectData.AnotherProject.FilePath, TestProjectData.AnotherProject.IntermediateOutputPath, FallbackRazorConfiguration.MVC_2_0, TestProjectData.AnotherProject.RootNamespace);

        _documentTracker = new DefaultVisualStudioDocumentTracker(
            Dispatcher,
            JoinableTaskFactory.Context,
            _filePath,
            _projectPath,
            _projectManager,
            _workspaceEditorSettings,
            ProjectEngineFactoryProvider,
            _textBuffer,
            _importDocumentManager);
    }

    protected override void ConfigureWorkspaceServices(List<IWorkspaceService> services)
    {
        _tagHelperResolver = new TestTagHelperResolver();
        services.Add(_tagHelperResolver);
    }

    protected override void ConfigureWorkspace(AdhocWorkspace workspace)
    {
        _workspaceProject = workspace.AddProject(ProjectInfo.Create(
            ProjectId.CreateNewId(),
            new VersionStamp(),
            "Test1",
            "TestAssembly",
            LanguageNames.CSharp,
            filePath: TestProjectData.SomeProject.FilePath));
    }

    [UIFact]
    public void Subscribe_NoopsIfAlreadySubscribed()
    {
        // Arrange
        var callCount = 0;
        _documentTracker.ContextChanged += (sender, args) => callCount++;
        _documentTracker.Subscribe();

        // Act
        _documentTracker.Subscribe();

        // Assert
        Assert.Equal(1, callCount);
    }

    [UIFact]
    public void Unsubscribe_NoopsIfAlreadyUnsubscribed()
    {
        // Arrange
        var callCount = 0;
        _documentTracker.Subscribe();
        _documentTracker.ContextChanged += (sender, args) => callCount++;
        _documentTracker.Unsubscribe();

        // Act
        _documentTracker.Unsubscribe();

        // Assert
        Assert.Equal(1, callCount);
    }

    [UIFact]
    public void Unsubscribe_NoopsIfSubscribeHasBeenCalledMultipleTimes()
    {
        // Arrange
        var callCount = 0;
        _documentTracker.Subscribe();
        _documentTracker.Subscribe();
        _documentTracker.ContextChanged += (sender, args) => callCount++;

        // Act - 1
        _documentTracker.Unsubscribe();

        // Assert - 1
        Assert.Equal(0, callCount);

        // Act - 2
        _documentTracker.Unsubscribe();

        // Assert - 2
        Assert.Equal(1, callCount);
    }

    [UIFact]
    public void EditorSettingsManager_Changed_TriggersContextChanged()
    {
        // Arrange
        var called = false;
        _documentTracker.ContextChanged += (sender, args) =>
        {
            Assert.Equal(ContextChangeKind.EditorSettingsChanged, args.Kind);
            called = true;
            Assert.Equal(ContextChangeKind.EditorSettingsChanged, args.Kind);
        };

        // Act
        _documentTracker.EditorSettingsManager_Changed(null, null);

        // Assert
        Assert.True(called);
    }

    [UIFact]
    public void ProjectManager_Changed_ProjectAdded_TriggersContextChanged()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject);

        var e = new ProjectChangeEventArgs(null, _projectManager.GetLoadedProject(_hostProject.Key), ProjectChangeKind.ProjectAdded);

        var called = false;
        _documentTracker.ContextChanged += (sender, args) =>
        {
            called = true;

            Assert.Same(_projectManager.GetLoadedProject(_hostProject.Key), _documentTracker.ProjectSnapshot);
        };

        // Act
        _documentTracker.ProjectManager_Changed(_projectManager, e);

        // Assert
        Assert.True(called);
    }

    [UIFact]
    public void ProjectManager_Changed_ProjectChanged_TriggersContextChanged()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject);

        var e = new ProjectChangeEventArgs(null, _projectManager.GetLoadedProject(_hostProject.Key), ProjectChangeKind.ProjectChanged);

        var called = false;
        _documentTracker.ContextChanged += (sender, args) =>
        {
            called = true;

            Assert.Same(_projectManager.GetLoadedProject(_hostProject.Key), _documentTracker.ProjectSnapshot);
        };

        // Act
        _documentTracker.ProjectManager_Changed(_projectManager, e);

        // Assert
        Assert.True(called);
    }

    [UIFact]
    public void ProjectManager_Changed_ProjectRemoved_TriggersContextChanged_WithEphemeralProject()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject);

        var project = _projectManager.GetLoadedProject(_hostProject.Key);
        _projectManager.ProjectRemoved(_hostProject.Key);

        var e = new ProjectChangeEventArgs(project, null, ProjectChangeKind.ProjectRemoved);

        var called = false;
        _documentTracker.ContextChanged += (sender, args) =>
        {
            // This can be called both with tag helper and project changes.
            called = true;

            Assert.IsType<EphemeralProjectSnapshot>(_documentTracker.ProjectSnapshot);
        };

        // Act
        _documentTracker.ProjectManager_Changed(_projectManager, e);

        // Assert
        Assert.True(called);
    }

    [UIFact]
    public void ProjectManager_Changed_IgnoresUnknownProject()
    {
        // Arrange
        _projectManager.ProjectAdded(_otherHostProject);

        var e = new ProjectChangeEventArgs(null, _projectManager.GetLoadedProject(_otherHostProject.Key), ProjectChangeKind.ProjectChanged);

        var called = false;
        _documentTracker.ContextChanged += (sender, args) => called = true;

        // Act
        _documentTracker.ProjectManager_Changed(_projectManager, e);

        // Assert
        Assert.False(called);
    }

    [UIFact]
    public void Import_Changed_ImportAssociatedWithDocument_TriggersContextChanged()
    {
        // Arrange
        var called = false;
        _documentTracker.ContextChanged += (sender, args) =>
        {
            Assert.Equal(ContextChangeKind.ImportsChanged, args.Kind);
            called = true;
        };

        var importChangedArgs = new ImportChangedEventArgs("path/to/import", FileChangeKind.Changed, new[] { _filePath });

        // Act
        _documentTracker.Import_Changed(null, importChangedArgs);

        // Assert
        Assert.True(called);
    }

    [UIFact]
    public void Import_Changed_UnrelatedImport_DoesNothing()
    {
        // Arrange
        _documentTracker.ContextChanged += (sender, args) => throw new InvalidOperationException();

        var importChangedArgs = new ImportChangedEventArgs("path/to/import", FileChangeKind.Changed, new[] { "path/to/differentfile" });

        // Act & Assert (Does not throw)
        _documentTracker.Import_Changed(null, importChangedArgs);
    }

    [UIFact]
    public void Subscribe_SetsSupportedProjectAndTriggersContextChanged()
    {
        // Arrange
        var called = false;
        _documentTracker.ContextChanged += (sender, args) => called = true;

        // Act
        _documentTracker.Subscribe();

        // Assert
        Assert.True(called);
        Assert.True(_documentTracker.IsSupportedProject);
    }

    [UIFact]
    public void Unsubscribe_ResetsSupportedProjectAndTriggersContextChanged()
    {
        // Arrange

        // Subscribe once to set supported project
        _documentTracker.Subscribe();

        var called = false;
        _documentTracker.ContextChanged += (sender, args) =>
        {
            called = true;
            Assert.Equal(ContextChangeKind.ProjectChanged, args.Kind);
        };

        // Act
        _documentTracker.Unsubscribe();

        // Assert
        Assert.False(_documentTracker.IsSupportedProject);
        Assert.True(called);
    }

    [UIFact]
    public void AddTextView_AddsToTextViewCollection()
    {
        // Arrange
        var textView = Mock.Of<ITextView>(MockBehavior.Strict);

        // Act
        _documentTracker.AddTextView(textView);

        // Assert
        Assert.Collection(_documentTracker.TextViews, v => Assert.Same(v, textView));
    }

    [UIFact]
    public void AddTextView_DoesNotAddDuplicateTextViews()
    {
        // Arrange
        var textView = Mock.Of<ITextView>(MockBehavior.Strict);

        // Act
        _documentTracker.AddTextView(textView);
        _documentTracker.AddTextView(textView);

        // Assert
        Assert.Collection(_documentTracker.TextViews, v => Assert.Same(v, textView));
    }

    [UIFact]
    public void AddTextView_AddsMultipleTextViewsToCollection()
    {
        // Arrange
        var textView1 = Mock.Of<ITextView>(MockBehavior.Strict);
        var textView2 = Mock.Of<ITextView>(MockBehavior.Strict);

        // Act
        _documentTracker.AddTextView(textView1);
        _documentTracker.AddTextView(textView2);

        // Assert
        Assert.Collection(
            _documentTracker.TextViews,
            v => Assert.Same(v, textView1),
            v => Assert.Same(v, textView2));
    }

    [UIFact]
    public void RemoveTextView_RemovesTextViewFromCollection_SingleItem()
    {
        // Arrange
        var textView = Mock.Of<ITextView>(MockBehavior.Strict);
        _documentTracker.AddTextView(textView);

        // Act
        _documentTracker.RemoveTextView(textView);

        // Assert
        Assert.Empty(_documentTracker.TextViews);
    }

    [UIFact]
    public void RemoveTextView_RemovesTextViewFromCollection_MultipleItems()
    {
        // Arrange
        var textView1 = Mock.Of<ITextView>(MockBehavior.Strict);
        var textView2 = Mock.Of<ITextView>(MockBehavior.Strict);
        var textView3 = Mock.Of<ITextView>(MockBehavior.Strict);
        _documentTracker.AddTextView(textView1);
        _documentTracker.AddTextView(textView2);
        _documentTracker.AddTextView(textView3);

        // Act
        _documentTracker.RemoveTextView(textView2);

        // Assert
        Assert.Collection(
            _documentTracker.TextViews,
            v => Assert.Same(v, textView1),
            v => Assert.Same(v, textView3));
    }

    [UIFact]
    public void RemoveTextView_NoopsWhenRemovingTextViewNotInCollection()
    {
        // Arrange
        var textView1 = Mock.Of<ITextView>(MockBehavior.Strict);
        _documentTracker.AddTextView(textView1);
        var textView2 = Mock.Of<ITextView>(MockBehavior.Strict);

        // Act
        _documentTracker.RemoveTextView(textView2);

        // Assert
        Assert.Collection(_documentTracker.TextViews, v => Assert.Same(v, textView1));
    }

    [UIFact]
    public void Subscribed_InitializesEphemeralProjectSnapshot()
    {
        // Arrange

        // Act
        _documentTracker.Subscribe();

        // Assert
        Assert.IsType<EphemeralProjectSnapshot>(_documentTracker.ProjectSnapshot);
    }

    [UIFact]
    public void Subscribed_InitializesRealProjectSnapshot()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject);

        // Act
        _documentTracker.Subscribe();

        // Assert
        Assert.IsType<ProjectSnapshot>(_documentTracker.ProjectSnapshot);
    }

    [UIFact]
    public void Subscribed_ListensToProjectChanges()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject);

        _documentTracker.Subscribe();

        var args = new List<ContextChangeEventArgs>();
        _documentTracker.ContextChanged += (sender, e) => args.Add(e);

        // Act
        _projectManager.ProjectConfigurationChanged(_updatedHostProject);

        // Assert
        var snapshot = Assert.IsType<ProjectSnapshot>(_documentTracker.ProjectSnapshot);

        Assert.Same(_updatedHostProject, snapshot.HostProject);

        Assert.Collection(
            args,
            e => Assert.Equal(ContextChangeKind.ProjectChanged, e.Kind));
    }

    [UIFact]
    public void Subscribed_ListensToProjectRemoval()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject);

        _documentTracker.Subscribe();

        var args = new List<ContextChangeEventArgs>();
        _documentTracker.ContextChanged += (sender, e) => args.Add(e);

        // Act
        _projectManager.ProjectRemoved(_hostProject.Key);

        // Assert
        Assert.IsType<EphemeralProjectSnapshot>(_documentTracker.ProjectSnapshot);

        Assert.Collection(
            args,
            e => Assert.Equal(ContextChangeKind.ProjectChanged, e.Kind));
    }
}
