// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.CodeAnalysis.Text;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

public class DefaultProjectSnapshotManagerTest : VisualStudioWorkspaceTestBase
{
    private static readonly HostDocument[] s_documents =
    [
        TestProjectData.SomeProjectFile1,
        TestProjectData.SomeProjectFile2,

        // linked file
        TestProjectData.AnotherProjectNestedFile3,

        TestProjectData.SomeProjectComponentFile1,
        TestProjectData.SomeProjectComponentFile2,
    ];

    private static readonly HostProject s_hostProject = new(
        TestProjectData.SomeProject.FilePath,
        TestProjectData.SomeProject.IntermediateOutputPath,
        FallbackRazorConfiguration.MVC_2_0,
        TestProjectData.SomeProject.RootNamespace);

    private static readonly HostProject s_hostProjectWithConfigurationChange = new(
        TestProjectData.SomeProject.FilePath,
        TestProjectData.SomeProject.IntermediateOutputPath,
        FallbackRazorConfiguration.MVC_1_0,
        TestProjectData.SomeProject.RootNamespace);

    private readonly ProjectWorkspaceState _projectWorkspaceStateWithTagHelpers;
    private readonly TestProjectSnapshotManager _projectManager;
    private readonly SourceText _sourceText;

    public DefaultProjectSnapshotManagerTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        var someTagHelpers = ImmutableArray.Create(
            TagHelperDescriptorBuilder.Create("Test1", "TestAssembly").Build());

        _projectManager = new TestProjectSnapshotManager(triggers: [], ProjectEngineFactoryProvider, Dispatcher);

        _projectWorkspaceStateWithTagHelpers = ProjectWorkspaceState.Create(someTagHelpers);

        _sourceText = SourceText.From("Hello world");
    }

    [UIFact]
    public void Initialize_DoneInCorrectOrderBasedOnInitializePriorityPriority()
    {
        // Arrange
        var initializedOrder = new List<string>();
        var highPriorityTrigger = new PriorityInitializeInspectionTrigger(() => initializedOrder.Add("highPriority"));
        var defaultPriorityTrigger = new InitializeInspectionTrigger(() => initializedOrder.Add("lowPriority"));

        // Building this list in the wrong order so we can verify priority matters
        var triggers = new[] { defaultPriorityTrigger, highPriorityTrigger };

        // Act
        var projectManager = new TestProjectSnapshotManager(triggers, ProjectEngineFactoryProvider, Dispatcher);

        // Assert
        Assert.Equal(["highPriority", "lowPriority"], initializedOrder);
    }

    [UIFact]
    public async Task DocumentAdded_AddsDocument()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
        });

        _projectManager.Reset();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[0], null!);
        });

        // Assert
        var snapshot = _projectManager.GetSnapshot(s_hostProject);
        Assert.Single(snapshot.DocumentFilePaths,
            filePath => filePath == s_documents[0].FilePath);

        Assert.Equal(ProjectChangeKind.DocumentAdded, _projectManager.ListenersNotifiedOf);
    }

    [UIFact]
    public async Task DocumentAdded_AddsDocument_Legacy()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
        });

        _projectManager.Reset();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[0], null!);
        });

        // Assert
        var snapshot = _projectManager.GetSnapshot(s_hostProject);
        Assert.Single(
            snapshot.DocumentFilePaths,
            filePath => filePath == s_documents[0].FilePath &&
                        snapshot.GetDocument(filePath).AssumeNotNull().FileKind == FileKinds.Legacy);

        Assert.Equal(ProjectChangeKind.DocumentAdded, _projectManager.ListenersNotifiedOf);
    }

    [UIFact]
    public async Task DocumentAdded_AddsDocument_Component()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
        });

        _projectManager.Reset();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[3], null!);
        });

        // Assert
        var snapshot = _projectManager.GetSnapshot(s_hostProject);
        Assert.Single(
            snapshot.DocumentFilePaths,
            filePath => filePath == s_documents[3].FilePath &&
                        snapshot.GetDocument(filePath).AssumeNotNull().FileKind == FileKinds.Component);

        Assert.Equal(ProjectChangeKind.DocumentAdded, _projectManager.ListenersNotifiedOf);
    }

    [UIFact]
    public async Task DocumentAdded_IgnoresDuplicate()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[0], null!);
        });

        _projectManager.Reset();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[0], null!);
        });

        // Assert
        var snapshot = _projectManager.GetSnapshot(s_hostProject);
        Assert.Single(snapshot.DocumentFilePaths,
            filePath => filePath == s_documents[0].FilePath);

        Assert.Null(_projectManager.ListenersNotifiedOf);
    }

    [UIFact]
    public void DocumentAdded_IgnoresUnknownProject()
    {
        // Arrange

        // Act
        _projectManager.DocumentAdded(s_hostProject.Key, s_documents[0], null!);

        // Assert
        var snapshot = _projectManager.GetSnapshot(s_hostProject);
        Assert.Null(snapshot);
    }

    [UIFact]
    public async Task DocumentAdded_NullLoader_HasEmptyText()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
        });

        _projectManager.Reset();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[0], null!);
        });

        // Assert
        var snapshot = _projectManager.GetSnapshot(s_hostProject);
        var document = snapshot.GetDocument(snapshot.DocumentFilePaths.Single());
        Assert.NotNull(document);

        var text = await document.GetTextAsync();
        Assert.Equal(0, text.Length);
    }

    [UIFact]
    public async Task DocumentAdded_WithLoader_LoadesText()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
        });

        _projectManager.Reset();

        var expected = SourceText.From("Hello");

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[0], TextLoader.From(TextAndVersion.Create(expected, VersionStamp.Default)));
        });

        // Assert
        var snapshot = _projectManager.GetSnapshot(s_hostProject);
        var filePath = Assert.Single(snapshot.DocumentFilePaths);
        var document = snapshot.GetDocument(filePath);
        Assert.NotNull(document);

        var actual = await document.GetTextAsync();
        Assert.Same(expected, actual);
    }

    [UIFact]
    public async Task DocumentAdded_CachesTagHelpers()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
            _projectManager.ProjectWorkspaceStateChanged(s_hostProject.Key, _projectWorkspaceStateWithTagHelpers);
        });

        _projectManager.Reset();

        var originalTagHelpers = await _projectManager.GetSnapshot(s_hostProject).GetTagHelpersAsync(DisposalToken);

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[0], null!);
        });

        // Assert
        var newTagHelpers = await _projectManager.GetSnapshot(s_hostProject).GetTagHelpersAsync(DisposalToken);

        Assert.Equal(originalTagHelpers.Length, newTagHelpers.Length);
        for (var i = 0; i < originalTagHelpers.Length; i++)
        {
            Assert.Same(originalTagHelpers[i], newTagHelpers[i]);
        }
    }

    [UIFact]
    public async Task DocumentAdded_CachesProjectEngine()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
        });

        _projectManager.Reset();

        var snapshot = _projectManager.GetSnapshot(s_hostProject);
        var projectEngine = snapshot.GetProjectEngine();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[0], null!);
        });

        // Assert
        snapshot = _projectManager.GetSnapshot(s_hostProject);
        Assert.Same(projectEngine, snapshot.GetProjectEngine());
    }

    [UIFact]
    public async Task DocumentRemoved_RemovesDocument()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[0], null!);
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[1], null!);
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[2], null!);
        });

        _projectManager.Reset();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentRemoved(s_hostProject.Key, s_documents[1]);
        });

        // Assert
        var snapshot = _projectManager.GetSnapshot(s_hostProject);
        Assert.Collection(
            snapshot.DocumentFilePaths.OrderBy(f => f),
            f => Assert.Equal(s_documents[2].FilePath, f),
            f => Assert.Equal(s_documents[0].FilePath, f));

        Assert.Equal(ProjectChangeKind.DocumentRemoved, _projectManager.ListenersNotifiedOf);
    }

    [UIFact]
    public async Task DocumentRemoved_IgnoresNotFoundDocument()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
        });

        _projectManager.Reset();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentRemoved(s_hostProject.Key, s_documents[0]);
        });

        // Assert
        var snapshot = _projectManager.GetSnapshot(s_hostProject);
        Assert.Empty(snapshot.DocumentFilePaths);

        Assert.Null(_projectManager.ListenersNotifiedOf);
    }

    [UIFact]
    public async Task DocumentRemoved_IgnoresUnknownProject()
    {
        // Arrange

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentRemoved(s_hostProject.Key, s_documents[0]);
        });

        // Assert
        var snapshot = _projectManager.GetSnapshot(s_hostProject);
        Assert.Null(snapshot);
    }

    [UIFact]
    public async Task DocumentRemoved_CachesTagHelpers()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
            _projectManager.ProjectWorkspaceStateChanged(s_hostProject.Key, _projectWorkspaceStateWithTagHelpers);
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[0], null!);
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[1], null!);
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[2], null!);
        });

        _projectManager.Reset();

        var originalTagHelpers = await _projectManager.GetSnapshot(s_hostProject).GetTagHelpersAsync(CancellationToken.None);

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentRemoved(s_hostProject.Key, s_documents[1]);
        });

        // Assert
        var newTagHelpers = await _projectManager.GetSnapshot(s_hostProject).GetTagHelpersAsync(CancellationToken.None);

        Assert.Equal(originalTagHelpers.Length, newTagHelpers.Length);
        for (var i = 0; i < originalTagHelpers.Length; i++)
        {
            Assert.Same(originalTagHelpers[i], newTagHelpers[i]);
        }
    }

    [UIFact]
    public async Task DocumentRemoved_CachesProjectEngine()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[0], null!);
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[1], null!);
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[2], null!);
        });

        _projectManager.Reset();

        var snapshot = _projectManager.GetSnapshot(s_hostProject);
        var projectEngine = snapshot.GetProjectEngine();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentRemoved(s_hostProject.Key, s_documents[1]);
        });

        // Assert
        snapshot = _projectManager.GetSnapshot(s_hostProject);
        Assert.Same(projectEngine, snapshot.GetProjectEngine());
    }
    [UIFact]
    public async Task DocumentOpened_UpdatesDocument()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[0], null!);
        });

        _projectManager.Reset();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentOpened(s_hostProject.Key, s_documents[0].FilePath, _sourceText);
        });

        // Assert
        Assert.Equal(ProjectChangeKind.DocumentChanged, _projectManager.ListenersNotifiedOf);

        var snapshot = _projectManager.GetSnapshot(s_hostProject);
        var text = await snapshot.GetDocument(s_documents[0].FilePath)!.GetTextAsync();
        Assert.Same(_sourceText, text);

        Assert.True(_projectManager.IsDocumentOpen(s_documents[0].FilePath));
    }

    [UIFact]
    public async Task DocumentClosed_UpdatesDocument()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[0], null!);
            _projectManager.DocumentOpened(s_hostProject.Key, s_documents[0].FilePath, _sourceText);
        });

        _projectManager.Reset();

        var expected = SourceText.From("Hi");
        var textAndVersion = TextAndVersion.Create(expected, VersionStamp.Create());

        Assert.True(_projectManager.IsDocumentOpen(s_documents[0].FilePath));

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentClosed(s_hostProject.Key, s_documents[0].FilePath, TextLoader.From(textAndVersion));
        });

        // Assert
        Assert.Equal(ProjectChangeKind.DocumentChanged, _projectManager.ListenersNotifiedOf);

        var snapshot = _projectManager.GetSnapshot(s_hostProject);
        var text = await snapshot.GetDocument(s_documents[0].FilePath)!.GetTextAsync();
        Assert.Same(expected, text);
        Assert.False(_projectManager.IsDocumentOpen(s_documents[0].FilePath));
    }

    [UIFact]
    public async Task DocumentClosed_AcceptsChange()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[0], null!);
        });

        _projectManager.Reset();

        var expected = SourceText.From("Hi");
        var textAndVersion = TextAndVersion.Create(expected, VersionStamp.Create());

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentClosed(s_hostProject.Key, s_documents[0].FilePath, TextLoader.From(textAndVersion));
        });

        // Assert
        Assert.Equal(ProjectChangeKind.DocumentChanged, _projectManager.ListenersNotifiedOf);

        var snapshot = _projectManager.GetSnapshot(s_hostProject);
        var text = await snapshot.GetDocument(s_documents[0].FilePath)!.GetTextAsync();
        Assert.Same(expected, text);
    }

    [UIFact]
    public async Task DocumentChanged_Snapshot_UpdatesDocument()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[0], null!);
            _projectManager.DocumentOpened(s_hostProject.Key, s_documents[0].FilePath, _sourceText);
        });

        _projectManager.Reset();

        var expected = SourceText.From("Hi");

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentChanged(s_hostProject.Key, s_documents[0].FilePath, expected);
        });

        // Assert
        Assert.Equal(ProjectChangeKind.DocumentChanged, _projectManager.ListenersNotifiedOf);

        var snapshot = _projectManager.GetSnapshot(s_hostProject);
        var text = await snapshot.GetDocument(s_documents[0].FilePath)!.GetTextAsync();
        Assert.Same(expected, text);
    }

    [UIFact]
    public async Task DocumentChanged_Loader_UpdatesDocument()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[0], null!);
            _projectManager.DocumentOpened(s_hostProject.Key, s_documents[0].FilePath, _sourceText);
        });

        _projectManager.Reset();

        var expected = SourceText.From("Hi");
        var textAndVersion = TextAndVersion.Create(expected, VersionStamp.Create());

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentChanged(s_hostProject.Key, s_documents[0].FilePath, TextLoader.From(textAndVersion));
        });

        // Assert
        Assert.Equal(ProjectChangeKind.DocumentChanged, _projectManager.ListenersNotifiedOf);

        var snapshot = _projectManager.GetSnapshot(s_hostProject);
        var text = await snapshot.GetDocument(s_documents[0].FilePath)!.GetTextAsync();
        Assert.Same(expected, text);
    }

    [UIFact]
    public async Task ProjectAdded_WithoutWorkspaceProject_NotifiesListeners()
    {
        // Arrange

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
        });

        // Assert
        Assert.Equal(ProjectChangeKind.ProjectAdded, _projectManager.ListenersNotifiedOf);
    }

    [UIFact]
    public async Task ProjectConfigurationChanged_ConfigurationChange_ProjectWorkspaceState_NotifiesListeners()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
        });

        _projectManager.Reset();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectConfigurationChanged(s_hostProjectWithConfigurationChange);
        });

        // Assert
        Assert.Equal(ProjectChangeKind.ProjectChanged, _projectManager.ListenersNotifiedOf);
    }

    [UIFact]
    public async Task ProjectConfigurationChanged_ConfigurationChange_WithProjectWorkspaceState_NotifiesListeners()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
            _projectManager.ProjectWorkspaceStateChanged(s_hostProject.Key, _projectWorkspaceStateWithTagHelpers);
        });

        _projectManager.Reset();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectConfigurationChanged(s_hostProjectWithConfigurationChange);
        });

        // Assert
        Assert.Equal(ProjectChangeKind.ProjectChanged, _projectManager.ListenersNotifiedOf);
    }

    [UIFact]
    public async Task ProjectConfigurationChanged_ConfigurationChange_DoesNotCacheProjectEngine()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
        });

        _projectManager.Reset();

        var snapshot = _projectManager.GetSnapshot(s_hostProject);
        var projectEngine = snapshot.GetProjectEngine();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectConfigurationChanged(s_hostProjectWithConfigurationChange);
        });

        // Assert
        snapshot = _projectManager.GetSnapshot(s_hostProjectWithConfigurationChange);
        Assert.NotSame(projectEngine, snapshot.GetProjectEngine());
    }

    [UIFact]
    public async Task ProjectConfigurationChanged_IgnoresUnknownProject()
    {
        // Arrange

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectConfigurationChanged(s_hostProject);
        });

        // Assert
        Assert.Empty(_projectManager.GetProjects());

        Assert.Null(_projectManager.ListenersNotifiedOf);
    }

    [UIFact]
    public async Task ProjectRemoved_RemovesProject_NotifiesListeners()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
        });

        _projectManager.Reset();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectRemoved(s_hostProject.Key);
        });

        // Assert
        Assert.Empty(_projectManager.GetProjects());

        Assert.Equal(ProjectChangeKind.ProjectRemoved, _projectManager.ListenersNotifiedOf);
    }

    [UIFact]
    public void ProjectWorkspaceStateChanged_WithoutHostProject_IgnoresWorkspaceState()
    {
        // Arrange

        // Act
        _projectManager.ProjectWorkspaceStateChanged(s_hostProject.Key, _projectWorkspaceStateWithTagHelpers);

        // Assert
        Assert.Empty(_projectManager.GetProjects());

        Assert.Null(_projectManager.ListenersNotifiedOf);
    }

    [UIFact]
    public async Task ProjectWorkspaceStateChanged_WithHostProject_FirstTime_NotifiesListeners()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
        });

        _projectManager.Reset();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectWorkspaceStateChanged(s_hostProject.Key, _projectWorkspaceStateWithTagHelpers);
        });

        // Assert
        Assert.Equal(ProjectChangeKind.ProjectChanged, _projectManager.ListenersNotifiedOf);
    }

    [UIFact]
    public async Task WorkspaceProjectChanged_WithHostProject_NotifiesListeners()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
            _projectManager.ProjectWorkspaceStateChanged(s_hostProject.Key, ProjectWorkspaceState.Default);
        });

        _projectManager.Reset();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectWorkspaceStateChanged(s_hostProject.Key, _projectWorkspaceStateWithTagHelpers);
        });

        // Assert
        Assert.Equal(ProjectChangeKind.ProjectChanged, _projectManager.ListenersNotifiedOf);
    }

    [UIFact]
    public async Task NestedNotifications_NotifiesListenersInCorrectOrder()
    {
        // Arrange
        var listenerNotifications = new List<ProjectChangeKind>();

        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
        });

        _projectManager.Reset();
        _projectManager.Changed += (sender, args) =>
        {
            // These conditions will result in a triply nested change notification of Add -> Change -> Remove all within the .Change chain.

            if (args.Kind == ProjectChangeKind.DocumentAdded)
            {
                _projectManager.DocumentOpened(s_hostProject.Key, s_documents[0].FilePath, _sourceText);
            }
            else if (args.Kind == ProjectChangeKind.DocumentChanged)
            {
                _projectManager.DocumentRemoved(s_hostProject.Key, s_documents[0]);
            }
        };

        _projectManager.Changed += (sender, args) => listenerNotifications.Add(args.Kind);
        _projectManager.NotifyChangedEvents = true;

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[0], null!);
        });

        // Assert
        Assert.Equal(new[] { ProjectChangeKind.DocumentAdded, ProjectChangeKind.DocumentChanged, ProjectChangeKind.DocumentRemoved }, listenerNotifications);
    }

    [UIFact]
    public async Task SolutionClosing_ProjectChangedEventsCorrect()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
        });

        _projectManager.Reset();

        _projectManager.Changed += (sender, args) => Assert.True(args.SolutionIsClosing);
        _projectManager.NotifyChangedEvents = true;

        var textLoader = new Mock<TextLoader>(MockBehavior.Strict);

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.SolutionClosed();
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[0], textLoader.Object);
        });

        // Assert
        Assert.Equal(ProjectChangeKind.DocumentAdded, _projectManager.ListenersNotifiedOf);
        textLoader.Verify(d => d.LoadTextAndVersionAsync(It.IsAny<LoadTextOptions>(), It.IsAny<CancellationToken>()), Times.Never());
    }

    private class TestProjectSnapshotManager(
        IEnumerable<IProjectSnapshotChangeTrigger> triggers,
        IProjectEngineFactoryProvider projectEngineFactoryProvider,
        ProjectSnapshotManagerDispatcher dispatcher)
        : DefaultProjectSnapshotManager(triggers, projectEngineFactoryProvider, dispatcher, Mock.Of<IErrorReporter>(MockBehavior.Strict))
    {
        public ProjectChangeKind? ListenersNotifiedOf { get; private set; }

        public bool NotifyChangedEvents { get; set; }

        public ProjectSnapshot GetSnapshot(HostProject hostProject)
        {
            return GetProjects().Cast<ProjectSnapshot>().FirstOrDefault(s => s.FilePath == hostProject.FilePath);
        }

        public ProjectSnapshot GetSnapshot(Project workspaceProject)
        {
            return GetProjects().Cast<ProjectSnapshot>().FirstOrDefault(s => s.FilePath == workspaceProject.FilePath);
        }

        public void Reset()
        {
            ListenersNotifiedOf = null;
        }

        protected override void NotifyListeners(ProjectChangeEventArgs e)
        {
            ListenersNotifiedOf = e.Kind;

            if (NotifyChangedEvents)
            {
                base.NotifyListeners(e);
            }
        }
    }

    private class InitializeInspectionTrigger(Action initializeNotification) : IProjectSnapshotChangeTrigger
    {
        private readonly Action _initializeNotification = initializeNotification;

        public void Initialize(ProjectSnapshotManagerBase projectManager)
        {
            _initializeNotification();
        }
    }

    private class PriorityInitializeInspectionTrigger(Action initializeNotification)
        : InitializeInspectionTrigger(initializeNotification), IPriorityProjectSnapshotChangeTrigger;
}
