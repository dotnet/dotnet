// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Editor.Razor.Documents;
using Microsoft.VisualStudio.Editor.Razor.Settings;
using Microsoft.VisualStudio.LegacyEditor.Razor.Settings;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LegacyEditor.Razor;

public class VisualStudioDocumentTrackerTest : VisualStudioWorkspaceTestBase
{
    private readonly ITextBuffer _textBuffer;
    private readonly string _filePath;
    private readonly HostProject _hostProject;
    private readonly HostProject _updatedHostProject;
    private readonly HostProject _otherHostProject;
    private readonly TestProjectSnapshotManager _projectManager;
    private readonly VisualStudioDocumentTracker _documentTracker;

    public VisualStudioDocumentTrackerTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _textBuffer = VsMocks.CreateTextBuffer(
            VsMocks.ContentTypes.Create(RazorConstants.LegacyCoreContentType, RazorLanguage.CoreContentType));

        _filePath = TestProjectData.SomeProjectFile1.FilePath;
        var projectPath = TestProjectData.SomeProject.FilePath;
        var rootNamespace = TestProjectData.SomeProject.RootNamespace;

        var importDocumentManagerMock = StrictMock.Of<IImportDocumentManager>();
        Mock.Get(importDocumentManagerMock)
            .Setup(m => m.OnSubscribed(It.IsAny<IVisualStudioDocumentTracker>()))
            .Verifiable();
        Mock.Get(importDocumentManagerMock)
            .Setup(m => m.OnUnsubscribed(It.IsAny<IVisualStudioDocumentTracker>()))
            .Verifiable();

        var workspaceEditorSettings = new WorkspaceEditorSettings(StrictMock.Of<IClientSettingsManager>());

        _projectManager = CreateProjectSnapshotManager();

        _hostProject = new HostProject(projectPath, TestProjectData.SomeProject.IntermediateOutputPath, FallbackRazorConfiguration.MVC_2_1, rootNamespace);
        _updatedHostProject = new HostProject(projectPath, TestProjectData.SomeProject.IntermediateOutputPath, FallbackRazorConfiguration.MVC_2_0, rootNamespace);
        _otherHostProject = new HostProject(TestProjectData.AnotherProject.FilePath, TestProjectData.AnotherProject.IntermediateOutputPath, FallbackRazorConfiguration.MVC_2_0, TestProjectData.AnotherProject.RootNamespace);

        _documentTracker = new VisualStudioDocumentTracker(
            Dispatcher,
            JoinableTaskFactory.Context,
            _filePath,
            projectPath,
            _projectManager,
            workspaceEditorSettings,
            ProjectEngineFactoryProvider,
            _textBuffer,
            importDocumentManagerMock);
    }

    protected override void ConfigureWorkspace(AdhocWorkspace workspace)
    {
        workspace.AddProject(ProjectInfo.Create(
            ProjectId.CreateNewId(),
            new VersionStamp(),
            "Test1",
            "TestAssembly",
            LanguageNames.CSharp,
            filePath: TestProjectData.SomeProject.FilePath));
    }

    [UIFact]
    public async Task Subscribe_NoopsIfAlreadySubscribed()
    {
        // Arrange
        var callCount = 0;
        _documentTracker.ContextChanged += (sender, args) => callCount++;
        await RunOnDispatcherAsync(_documentTracker.Subscribe);

        // Act
        await RunOnDispatcherAsync(_documentTracker.Subscribe);

        // Assert
        Assert.Equal(1, callCount);
    }

    [UIFact]
    public async Task Unsubscribe_NoopsIfAlreadyUnsubscribed()
    {
        // Arrange
        var callCount = 0;
        await RunOnDispatcherAsync(_documentTracker.Subscribe);
        _documentTracker.ContextChanged += (sender, args) => callCount++;
        await RunOnDispatcherAsync(_documentTracker.Unsubscribe);

        // Act
        await RunOnDispatcherAsync(_documentTracker.Unsubscribe);

        // Assert
        Assert.Equal(1, callCount);
    }

    [UIFact]
    public async Task Unsubscribe_NoopsIfSubscribeHasBeenCalledMultipleTimes()
    {
        // Arrange
        var callCount = 0;
        await RunOnDispatcherAsync(_documentTracker.Subscribe);
        await RunOnDispatcherAsync(_documentTracker.Subscribe);
        _documentTracker.ContextChanged += (sender, args) => callCount++;

        // Act - 1
        await RunOnDispatcherAsync(_documentTracker.Unsubscribe);

        // Assert - 1
        Assert.Equal(0, callCount);

        // Act - 2
        await RunOnDispatcherAsync(_documentTracker.Unsubscribe);

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
        _documentTracker.EditorSettingsManager_Changed(null!, null!);

        // Assert
        Assert.True(called);
    }

    [UIFact]
    public async Task ProjectManager_Changed_ProjectAdded_TriggersContextChanged()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(_hostProject);
        });

        var e = new ProjectChangeEventArgs(null!, _projectManager.GetLoadedProject(_hostProject.Key), ProjectChangeKind.ProjectAdded);

        var called = false;
        _documentTracker.ContextChanged += (sender, args) =>
        {
            called = true;

            Assert.Same(_projectManager.GetLoadedProject(_hostProject.Key), _documentTracker.ProjectSnapshot);
        };

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _documentTracker.ProjectManager_Changed(_projectManager, e);
        });

        // Assert
        Assert.True(called);
    }

    [UIFact]
    public async Task ProjectManager_Changed_ProjectChanged_TriggersContextChanged()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(_hostProject);
        });

        var e = new ProjectChangeEventArgs(null!, _projectManager.GetLoadedProject(_hostProject.Key), ProjectChangeKind.ProjectChanged);

        var called = false;
        _documentTracker.ContextChanged += (sender, args) =>
        {
            called = true;

            Assert.Same(_projectManager.GetLoadedProject(_hostProject.Key), _documentTracker.ProjectSnapshot);
        };

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _documentTracker.ProjectManager_Changed(_projectManager, e);
        });

        // Assert
        Assert.True(called);
    }

    [UIFact]
    public async Task ProjectManager_Changed_ProjectRemoved_TriggersContextChanged_WithEphemeralProject()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(_hostProject);
        });

        var project = _projectManager.GetLoadedProject(_hostProject.Key);

        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectRemoved(_hostProject.Key);
        });

        var e = new ProjectChangeEventArgs(project!, null!, ProjectChangeKind.ProjectRemoved);

        var called = false;
        _documentTracker.ContextChanged += (sender, args) =>
        {
            // This can be called both with tag helper and project changes.
            called = true;

            Assert.IsType<EphemeralProjectSnapshot>(_documentTracker.ProjectSnapshot);
        };

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _documentTracker.ProjectManager_Changed(_projectManager, e);
        });

        // Assert
        Assert.True(called);
    }

    [UIFact]
    public async Task ProjectManager_Changed_IgnoresUnknownProject()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(_otherHostProject);
        });

        var e = new ProjectChangeEventArgs(null!, _projectManager.GetLoadedProject(_otherHostProject.Key), ProjectChangeKind.ProjectChanged);

        var called = false;
        _documentTracker.ContextChanged += (sender, args) => called = true;

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _documentTracker.ProjectManager_Changed(_projectManager, e);
        });

        // Assert
        Assert.False(called);
    }

    [UIFact]
    public async Task Import_Changed_ImportAssociatedWithDocument_TriggersContextChanged()
    {
        // Arrange
        var called = false;
        _documentTracker.ContextChanged += (sender, args) =>
        {
            Assert.Equal(ContextChangeKind.ImportsChanged, args.Kind);
            called = true;
        };

        var importChangedArgs = new ImportChangedEventArgs("path/to/import", FileChangeKind.Changed, [_filePath]);

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _documentTracker.Import_Changed(null!, importChangedArgs);
        });

        // Assert
        Assert.True(called);
    }

    [UIFact]
    public async Task Import_Changed_UnrelatedImport_DoesNothing()
    {
        // Arrange
        _documentTracker.ContextChanged += (sender, args) => throw new InvalidOperationException();

        var importChangedArgs = new ImportChangedEventArgs("path/to/import", FileChangeKind.Changed, ["path/to/differentfile"]);

        // Act & Assert (Does not throw)
        await RunOnDispatcherAsync(() =>
        {
            _documentTracker.Import_Changed(null!, importChangedArgs);
        });
    }

    [UIFact]
    public async Task Subscribe_SetsSupportedProjectAndTriggersContextChanged()
    {
        // Arrange
        var called = false;
        _documentTracker.ContextChanged += (sender, args) => called = true;

        // Act
        await RunOnDispatcherAsync(_documentTracker.Subscribe);

        // Assert
        Assert.True(called);
        Assert.True(_documentTracker.IsSupportedProject);
    }

    [UIFact]
    public async Task Unsubscribe_ResetsSupportedProjectAndTriggersContextChanged()
    {
        // Arrange

        // Subscribe once to set supported project
        await RunOnDispatcherAsync(_documentTracker.Subscribe);

        var called = false;
        _documentTracker.ContextChanged += (sender, args) =>
        {
            called = true;
            Assert.Equal(ContextChangeKind.ProjectChanged, args.Kind);
        };

        // Act
        await RunOnDispatcherAsync(_documentTracker.Unsubscribe);

        // Assert
        Assert.False(_documentTracker.IsSupportedProject);
        Assert.True(called);
    }

    [UIFact]
    public void AddTextView_AddsToTextViewCollection()
    {
        // Arrange
        var textView = StrictMock.Of<ITextView>();

        // Act
        _documentTracker.AddTextView(textView);

        // Assert
        var currentView = Assert.Single(_documentTracker.TextViews);
        Assert.Same(currentView, textView);
    }

    [UIFact]
    public void AddTextView_DoesNotAddDuplicateTextViews()
    {
        // Arrange
        var textView = StrictMock.Of<ITextView>();

        // Act
        _documentTracker.AddTextView(textView);
        _documentTracker.AddTextView(textView);

        // Assert
        var currentView = Assert.Single(_documentTracker.TextViews);
        Assert.Same(currentView, textView);
    }

    [UIFact]
    public void AddTextView_AddsMultipleTextViewsToCollection()
    {
        // Arrange
        var textView1 = StrictMock.Of<ITextView>();
        var textView2 = StrictMock.Of<ITextView>();

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
        var textView = StrictMock.Of<ITextView>();
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
        var textView1 = StrictMock.Of<ITextView>();
        var textView2 = StrictMock.Of<ITextView>();
        var textView3 = StrictMock.Of<ITextView>();
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
        var textView1 = StrictMock.Of<ITextView>();
        _documentTracker.AddTextView(textView1);
        var textView2 = StrictMock.Of<ITextView>();

        // Act
        _documentTracker.RemoveTextView(textView2);

        // Assert
        var currentView = Assert.Single(_documentTracker.TextViews);
        Assert.Same(currentView, textView1);
    }

    [UIFact]
    public async Task Subscribed_InitializesEphemeralProjectSnapshot()
    {
        // Arrange

        // Act
        await RunOnDispatcherAsync(_documentTracker.Subscribe);

        // Assert
        Assert.IsType<EphemeralProjectSnapshot>(_documentTracker.ProjectSnapshot);
    }

    [UIFact]
    public async Task Subscribed_InitializesRealProjectSnapshot()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(_hostProject);
        });

        // Act
        await RunOnDispatcherAsync(_documentTracker.Subscribe);

        // Assert
        Assert.IsType<ProjectSnapshot>(_documentTracker.ProjectSnapshot);
    }

    [UIFact]
    public async Task Subscribed_ListensToProjectChanges()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(_hostProject);

            _documentTracker.Subscribe();
        });

        var args = new List<ContextChangeEventArgs>();
        _documentTracker.ContextChanged += (sender, e) => args.Add(e);

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectConfigurationChanged(_updatedHostProject);
        });

        // Assert
        var snapshot = Assert.IsType<ProjectSnapshot>(_documentTracker.ProjectSnapshot);

        Assert.Same(_updatedHostProject, snapshot.HostProject);

        var currentArgs = Assert.Single(args);
        Assert.Equal(ContextChangeKind.ProjectChanged, currentArgs.Kind);
    }

    [UIFact]
    public async Task Subscribed_ListensToProjectRemoval()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(_hostProject);

            _documentTracker.Subscribe();
        });

        var args = new List<ContextChangeEventArgs>();
        _documentTracker.ContextChanged += (sender, e) => args.Add(e);

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectRemoved(_hostProject.Key);
        });

        // Assert
        Assert.IsType<EphemeralProjectSnapshot>(_documentTracker.ProjectSnapshot);

        var currentArgs = Assert.Single(args);
        Assert.Equal(ContextChangeKind.ProjectChanged, currentArgs.Kind);
    }
}
