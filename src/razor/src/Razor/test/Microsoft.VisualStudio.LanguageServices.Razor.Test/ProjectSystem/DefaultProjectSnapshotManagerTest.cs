// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Text;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

public class DefaultProjectSnapshotManagerTest : ProjectSnapshotManagerDispatcherWorkspaceTestBase
{
    private readonly HostDocument[] _documents;
    private readonly HostProject _hostProject;
    private readonly HostProject _hostProject2;
    private readonly HostProject _hostProjectWithConfigurationChange;
    private readonly ProjectWorkspaceState _projectWorkspaceStateWithTagHelpers;
    private readonly TestTagHelperResolver _tagHelperResolver;
    private readonly TestProjectSnapshotManager _projectManager;
    private readonly SourceText _sourceText;

    public DefaultProjectSnapshotManagerTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        var someTagHelpers = ImmutableArray.Create(
            TagHelperDescriptorBuilder.Create("Test1", "TestAssembly").Build());

        _tagHelperResolver = new TestTagHelperResolver()
        {
            TagHelpers = someTagHelpers,
        };

        _documents = new HostDocument[]
        {
            TestProjectData.SomeProjectFile1,
            TestProjectData.SomeProjectFile2,

            // linked file
            TestProjectData.AnotherProjectNestedFile3,

            TestProjectData.SomeProjectComponentFile1,
            TestProjectData.SomeProjectComponentFile2,
        };

        _hostProject = new HostProject(TestProjectData.SomeProject.FilePath, TestProjectData.SomeProject.IntermediateOutputPath, FallbackRazorConfiguration.MVC_2_0, TestProjectData.SomeProject.RootNamespace);
        _hostProject2 = new HostProject(TestProjectData.AnotherProject.FilePath, TestProjectData.AnotherProject.IntermediateOutputPath, FallbackRazorConfiguration.MVC_2_1, TestProjectData.AnotherProject.RootNamespace);
        _hostProjectWithConfigurationChange = new HostProject(TestProjectData.SomeProject.FilePath, TestProjectData.SomeProject.IntermediateOutputPath, FallbackRazorConfiguration.MVC_1_0, TestProjectData.SomeProject.RootNamespace);

        _projectManager = new TestProjectSnapshotManager(Enumerable.Empty<IProjectSnapshotChangeTrigger>(), Workspace, Dispatcher);

        _projectWorkspaceStateWithTagHelpers = new ProjectWorkspaceState(_tagHelperResolver.TagHelpers, default);

        _sourceText = SourceText.From("Hello world");
    }

    protected override void ConfigureWorkspaceServices(List<IWorkspaceService> services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.Add(_tagHelperResolver);
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
        var projectManager = new TestProjectSnapshotManager(triggers, Workspace, Dispatcher);

        // Assert
        Assert.Equal(new[] { "highPriority", "lowPriority" }, initializedOrder);
    }

    [UIFact]
    public void DocumentAdded_AddsDocument()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject);
        _projectManager.Reset();

        // Act
        _projectManager.DocumentAdded(_hostProject.Key, _documents[0], null);

        // Assert
        var snapshot = _projectManager.GetSnapshot(_hostProject);
        Assert.Collection(snapshot.DocumentFilePaths.OrderBy(f => f), d => Assert.Equal(_documents[0].FilePath, d));

        Assert.Equal(ProjectChangeKind.DocumentAdded, _projectManager.ListenersNotifiedOf);
    }

    [UIFact]
    public void DocumentAdded_AddsDocument_Legacy()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject);
        _projectManager.Reset();

        // Act
        _projectManager.DocumentAdded(_hostProject.Key, _documents[0], null);

        // Assert
        var snapshot = _projectManager.GetSnapshot(_hostProject);
        Assert.Collection(
            snapshot.DocumentFilePaths.OrderBy(f => f),
            d =>
            {
                Assert.Equal(_documents[0].FilePath, d);
                Assert.Equal(FileKinds.Legacy, snapshot.GetDocument(d).FileKind);
            });

        Assert.Equal(ProjectChangeKind.DocumentAdded, _projectManager.ListenersNotifiedOf);
    }

    [UIFact]
    public void DocumentAdded_AddsDocument_Component()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject);
        _projectManager.Reset();

        // Act
        _projectManager.DocumentAdded(_hostProject.Key, _documents[3], null);

        // Assert
        var snapshot = _projectManager.GetSnapshot(_hostProject);
        Assert.Collection(
            snapshot.DocumentFilePaths.OrderBy(f => f),
            d =>
            {
                Assert.Equal(_documents[3].FilePath, d);
                Assert.Equal(FileKinds.Component, snapshot.GetDocument(d).FileKind);
            });

        Assert.Equal(ProjectChangeKind.DocumentAdded, _projectManager.ListenersNotifiedOf);
    }

    [UIFact]
    public void DocumentAdded_IgnoresDuplicate()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject);
        _projectManager.DocumentAdded(_hostProject.Key, _documents[0], null);
        _projectManager.Reset();

        // Act
        _projectManager.DocumentAdded(_hostProject.Key, _documents[0], null);

        // Assert
        var snapshot = _projectManager.GetSnapshot(_hostProject);
        Assert.Collection(snapshot.DocumentFilePaths.OrderBy(f => f), d => Assert.Equal(_documents[0].FilePath, d));

        Assert.Null(_projectManager.ListenersNotifiedOf);
    }

    [UIFact]
    public void DocumentAdded_IgnoresUnknownProject()
    {
        // Arrange

        // Act
        _projectManager.DocumentAdded(_hostProject.Key, _documents[0], null);

        // Assert
        var snapshot = _projectManager.GetSnapshot(_hostProject);
        Assert.Null(snapshot);
    }

    [UIFact]
    public async Task DocumentAdded_NullLoader_HasEmptyText()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject);
        _projectManager.Reset();

        // Act
        _projectManager.DocumentAdded(_hostProject.Key, _documents[0], null);

        // Assert
        var snapshot = _projectManager.GetSnapshot(_hostProject);
        var document = snapshot.GetDocument(snapshot.DocumentFilePaths.Single());

        var text = await document.GetTextAsync();
        Assert.Equal(0, text.Length);
    }

    [UIFact]
    public async Task DocumentAdded_WithLoader_LoadesText()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject);
        _projectManager.Reset();

        var expected = SourceText.From("Hello");

        // Act
        _projectManager.DocumentAdded(_hostProject.Key, _documents[0], TextLoader.From(TextAndVersion.Create(expected, VersionStamp.Default)));

        // Assert
        var snapshot = _projectManager.GetSnapshot(_hostProject);
        var document = snapshot.GetDocument(snapshot.DocumentFilePaths.Single());

        var actual = await document.GetTextAsync();
        Assert.Same(expected, actual);
    }

    [UIFact]
    public void DocumentAdded_CachesTagHelpers()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject);
        _projectManager.ProjectWorkspaceStateChanged(_hostProject.Key, _projectWorkspaceStateWithTagHelpers);
        _projectManager.Reset();

        var originalTagHelpers = _projectManager.GetSnapshot(_hostProject).TagHelpers;

        // Act
        _projectManager.DocumentAdded(_hostProject.Key, _documents[0], null);

        // Assert
        var newTagHelpers = _projectManager.GetSnapshot(_hostProject).TagHelpers;

        Assert.Equal(originalTagHelpers.Length, newTagHelpers.Length);
        for (var i = 0; i < originalTagHelpers.Length; i++)
        {
            Assert.Same(originalTagHelpers[i], newTagHelpers[i]);
        }
    }

    [UIFact]
    public void DocumentAdded_CachesProjectEngine()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject);
        _projectManager.Reset();

        var snapshot = _projectManager.GetSnapshot(_hostProject);
        var projectEngine = snapshot.GetProjectEngine();

        // Act
        _projectManager.DocumentAdded(_hostProject.Key, _documents[0], null);

        // Assert
        snapshot = _projectManager.GetSnapshot(_hostProject);
        Assert.Same(projectEngine, snapshot.GetProjectEngine());
    }

    [UIFact]
    public void DocumentRemoved_RemovesDocument()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject);
        _projectManager.DocumentAdded(_hostProject.Key, _documents[0], null);
        _projectManager.DocumentAdded(_hostProject.Key, _documents[1], null);
        _projectManager.DocumentAdded(_hostProject.Key, _documents[2], null);
        _projectManager.Reset();

        // Act
        _projectManager.DocumentRemoved(_hostProject.Key, _documents[1]);

        // Assert
        var snapshot = _projectManager.GetSnapshot(_hostProject);
        Assert.Collection(
            snapshot.DocumentFilePaths.OrderBy(f => f),
            d => Assert.Equal(_documents[2].FilePath, d),
            d => Assert.Equal(_documents[0].FilePath, d));

        Assert.Equal(ProjectChangeKind.DocumentRemoved, _projectManager.ListenersNotifiedOf);
    }

    [UIFact]
    public void DocumentRemoved_IgnoresNotFoundDocument()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject);
        _projectManager.Reset();

        // Act
        _projectManager.DocumentRemoved(_hostProject.Key, _documents[0]);

        // Assert
        var snapshot = _projectManager.GetSnapshot(_hostProject);
        Assert.Empty(snapshot.DocumentFilePaths);

        Assert.Null(_projectManager.ListenersNotifiedOf);
    }

    [UIFact]
    public void DocumentRemoved_IgnoresUnknownProject()
    {
        // Arrange

        // Act
        _projectManager.DocumentRemoved(_hostProject.Key, _documents[0]);

        // Assert
        var snapshot = _projectManager.GetSnapshot(_hostProject);
        Assert.Null(snapshot);
    }

    [UIFact]
    public void DocumentRemoved_CachesTagHelpers()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject);
        _projectManager.ProjectWorkspaceStateChanged(_hostProject.Key, _projectWorkspaceStateWithTagHelpers);
        _projectManager.DocumentAdded(_hostProject.Key, _documents[0], null);
        _projectManager.DocumentAdded(_hostProject.Key, _documents[1], null);
        _projectManager.DocumentAdded(_hostProject.Key, _documents[2], null);
        _projectManager.Reset();

        var originalTagHelpers = _projectManager.GetSnapshot(_hostProject).TagHelpers;

        // Act
        _projectManager.DocumentRemoved(_hostProject.Key, _documents[1]);

        // Assert
        var newTagHelpers = _projectManager.GetSnapshot(_hostProject).TagHelpers;

        Assert.Equal(originalTagHelpers.Length, newTagHelpers.Length);
        for (var i = 0; i < originalTagHelpers.Length; i++)
        {
            Assert.Same(originalTagHelpers[i], newTagHelpers[i]);
        }
    }

    [UIFact]
    public void DocumentRemoved_CachesProjectEngine()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject);
        _projectManager.DocumentAdded(_hostProject.Key, _documents[0], null);
        _projectManager.DocumentAdded(_hostProject.Key, _documents[1], null);
        _projectManager.DocumentAdded(_hostProject.Key, _documents[2], null);
        _projectManager.Reset();

        var snapshot = _projectManager.GetSnapshot(_hostProject);
        var projectEngine = snapshot.GetProjectEngine();

        // Act
        _projectManager.DocumentRemoved(_hostProject.Key, _documents[1]);

        // Assert
        snapshot = _projectManager.GetSnapshot(_hostProject);
        Assert.Same(projectEngine, snapshot.GetProjectEngine());
    }
    [UIFact]
    public async Task DocumentOpened_UpdatesDocument()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject);
        _projectManager.DocumentAdded(_hostProject.Key, _documents[0], null);
        _projectManager.Reset();

        // Act
        _projectManager.DocumentOpened(_hostProject.Key, _documents[0].FilePath, _sourceText);

        // Assert
        Assert.Equal(ProjectChangeKind.DocumentChanged, _projectManager.ListenersNotifiedOf);

        var snapshot = _projectManager.GetSnapshot(_hostProject);
        var text = await snapshot.GetDocument(_documents[0].FilePath).GetTextAsync();
        Assert.Same(_sourceText, text);

        Assert.True(_projectManager.IsDocumentOpen(_documents[0].FilePath));
    }

    [UIFact]
    public async Task DocumentClosed_UpdatesDocument()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject);
        _projectManager.DocumentAdded(_hostProject.Key, _documents[0], null);
        _projectManager.DocumentOpened(_hostProject.Key, _documents[0].FilePath, _sourceText);
        _projectManager.Reset();

        var expected = SourceText.From("Hi");
        var textAndVersion = TextAndVersion.Create(expected, VersionStamp.Create());

        Assert.True(_projectManager.IsDocumentOpen(_documents[0].FilePath));

        // Act
        _projectManager.DocumentClosed(_hostProject.Key, _documents[0].FilePath, TextLoader.From(textAndVersion));

        // Assert
        Assert.Equal(ProjectChangeKind.DocumentChanged, _projectManager.ListenersNotifiedOf);

        var snapshot = _projectManager.GetSnapshot(_hostProject);
        var text = await snapshot.GetDocument(_documents[0].FilePath).GetTextAsync();
        Assert.Same(expected, text);
        Assert.False(_projectManager.IsDocumentOpen(_documents[0].FilePath));
    }

    [UIFact]
    public async Task DocumentClosed_AcceptsChange()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject);
        _projectManager.DocumentAdded(_hostProject.Key, _documents[0], null);
        _projectManager.Reset();

        var expected = SourceText.From("Hi");
        var textAndVersion = TextAndVersion.Create(expected, VersionStamp.Create());

        // Act
        _projectManager.DocumentClosed(_hostProject.Key, _documents[0].FilePath, TextLoader.From(textAndVersion));

        // Assert
        Assert.Equal(ProjectChangeKind.DocumentChanged, _projectManager.ListenersNotifiedOf);

        var snapshot = _projectManager.GetSnapshot(_hostProject);
        var text = await snapshot.GetDocument(_documents[0].FilePath).GetTextAsync();
        Assert.Same(expected, text);
    }

    [UIFact]
    public async Task DocumentChanged_Snapshot_UpdatesDocument()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject);
        _projectManager.DocumentAdded(_hostProject.Key, _documents[0], null);
        _projectManager.DocumentOpened(_hostProject.Key, _documents[0].FilePath, _sourceText);
        _projectManager.Reset();

        var expected = SourceText.From("Hi");

        // Act
        _projectManager.DocumentChanged(_hostProject.Key, _documents[0].FilePath, expected);

        // Assert
        Assert.Equal(ProjectChangeKind.DocumentChanged, _projectManager.ListenersNotifiedOf);

        var snapshot = _projectManager.GetSnapshot(_hostProject);
        var text = await snapshot.GetDocument(_documents[0].FilePath).GetTextAsync();
        Assert.Same(expected, text);
    }

    [UIFact]
    public async Task DocumentChanged_Loader_UpdatesDocument()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject);
        _projectManager.DocumentAdded(_hostProject.Key, _documents[0], null);
        _projectManager.DocumentOpened(_hostProject.Key, _documents[0].FilePath, _sourceText);
        _projectManager.Reset();

        var expected = SourceText.From("Hi");
        var textAndVersion = TextAndVersion.Create(expected, VersionStamp.Create());

        // Act
        _projectManager.DocumentChanged(_hostProject.Key, _documents[0].FilePath, TextLoader.From(textAndVersion));

        // Assert
        Assert.Equal(ProjectChangeKind.DocumentChanged, _projectManager.ListenersNotifiedOf);

        var snapshot = _projectManager.GetSnapshot(_hostProject);
        var text = await snapshot.GetDocument(_documents[0].FilePath).GetTextAsync();
        Assert.Same(expected, text);
    }

    [UIFact]
    public void ProjectAdded_WithoutWorkspaceProject_NotifiesListeners()
    {
        // Arrange

        // Act
        _projectManager.ProjectAdded(_hostProject);

        // Assert
        Assert.Equal(ProjectChangeKind.ProjectAdded, _projectManager.ListenersNotifiedOf);
    }

    [UIFact]
    public void ProjectConfigurationChanged_ConfigurationChange_ProjectWorkspaceState_NotifiesListeners()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject);
        _projectManager.Reset();

        // Act
        _projectManager.ProjectConfigurationChanged(_hostProjectWithConfigurationChange);

        // Assert
        Assert.Equal(ProjectChangeKind.ProjectChanged, _projectManager.ListenersNotifiedOf);
    }

    [UIFact]
    public void ProjectConfigurationChanged_ConfigurationChange_WithProjectWorkspaceState_NotifiesListeners()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject);
        _projectManager.ProjectWorkspaceStateChanged(_hostProject.Key, _projectWorkspaceStateWithTagHelpers);
        _projectManager.Reset();

        // Act
        _projectManager.ProjectConfigurationChanged(_hostProjectWithConfigurationChange);

        // Assert
        Assert.Equal(ProjectChangeKind.ProjectChanged, _projectManager.ListenersNotifiedOf);
    }

    [UIFact]
    public void ProjectConfigurationChanged_ConfigurationChange_DoesNotCacheProjectEngine()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject);
        _projectManager.Reset();

        var snapshot = _projectManager.GetSnapshot(_hostProject);
        var projectEngine = snapshot.GetProjectEngine();

        // Act
        _projectManager.ProjectConfigurationChanged(_hostProjectWithConfigurationChange);

        // Assert
        snapshot = _projectManager.GetSnapshot(_hostProjectWithConfigurationChange);
        Assert.NotSame(projectEngine, snapshot.GetProjectEngine());
    }

    [UIFact]
    public void ProjectConfigurationChanged_IgnoresUnknownProject()
    {
        // Arrange

        // Act
        _projectManager.ProjectConfigurationChanged(_hostProject);

        // Assert
        Assert.Empty(_projectManager.GetProjects());

        Assert.Null(_projectManager.ListenersNotifiedOf);
    }

    [UIFact]
    public void ProjectRemoved_RemovesProject_NotifiesListeners()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject);
        _projectManager.Reset();

        // Act
        _projectManager.ProjectRemoved(_hostProject.Key);

        // Assert
        Assert.Empty(_projectManager.GetProjects());

        Assert.Equal(ProjectChangeKind.ProjectRemoved, _projectManager.ListenersNotifiedOf);
    }

    [UIFact]
    public void ProjectWorkspaceStateChanged_WithoutHostProject_IgnoresWorkspaceState()
    {
        // Arrange

        // Act
        _projectManager.ProjectWorkspaceStateChanged(_hostProject.Key, _projectWorkspaceStateWithTagHelpers);

        // Assert
        Assert.Empty(_projectManager.GetProjects());

        Assert.Null(_projectManager.ListenersNotifiedOf);
    }

    [UIFact]
    public void ProjectWorkspaceStateChanged_WithHostProject_FirstTime_NotifiesListenters()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject);
        _projectManager.Reset();

        // Act
        _projectManager.ProjectWorkspaceStateChanged(_hostProject.Key, _projectWorkspaceStateWithTagHelpers);

        // Assert
        Assert.Equal(ProjectChangeKind.ProjectChanged, _projectManager.ListenersNotifiedOf);
    }

    [UIFact]
    public void WorkspaceProjectChanged_WithHostProject_NotifiesListenters()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject);
        _projectManager.ProjectWorkspaceStateChanged(_hostProject.Key, ProjectWorkspaceState.Default);
        _projectManager.Reset();

        // Act
        _projectManager.ProjectWorkspaceStateChanged(_hostProject.Key, _projectWorkspaceStateWithTagHelpers);

        // Assert
        Assert.Equal(ProjectChangeKind.ProjectChanged, _projectManager.ListenersNotifiedOf);
    }

    [UIFact]
    public void NestedNotifications_NotifiesListenersInCorrectOrder()
    {
        // Arrange
        var listenerNotifications = new List<ProjectChangeKind>();
        _projectManager.ProjectAdded(_hostProject);
        _projectManager.Reset();
        _projectManager.Changed += (sender, args) =>
        {
            // These conditions will result in a triply nested change notification of Add -> Change -> Remove all within the .Change chain.

            if (args.Kind == ProjectChangeKind.DocumentAdded)
            {
                _projectManager.DocumentOpened(_hostProject.Key, _documents[0].FilePath, _sourceText);
            }
            else if (args.Kind == ProjectChangeKind.DocumentChanged)
            {
                _projectManager.DocumentRemoved(_hostProject.Key, _documents[0]);
            }
        };
        _projectManager.Changed += (sender, args) => listenerNotifications.Add(args.Kind);
        _projectManager.NotifyChangedEvents = true;

        // Act
        _projectManager.DocumentAdded(_hostProject.Key, _documents[0], null);

        // Assert
        Assert.Equal(new[] { ProjectChangeKind.DocumentAdded, ProjectChangeKind.DocumentChanged, ProjectChangeKind.DocumentRemoved }, listenerNotifications);
    }

    [UIFact]
    public void SolutionClosing_ProjectChangedEventsCorrect()
    {
        // Arrange
        _projectManager.ProjectAdded(_hostProject);
        _projectManager.Reset();

        _projectManager.Changed += (sender, args) => Assert.True(args.SolutionIsClosing);
        _projectManager.NotifyChangedEvents = true;

        var textLoader = new Mock<TextLoader>(MockBehavior.Strict);

        // Act
        _projectManager.SolutionClosed();
        _projectManager.DocumentAdded(_hostProject.Key, _documents[0], textLoader.Object);

        // Assert
        Assert.Equal(ProjectChangeKind.DocumentAdded, _projectManager.ListenersNotifiedOf);
        textLoader.Verify(d => d.LoadTextAndVersionAsync(It.IsAny<LoadTextOptions>(), It.IsAny<CancellationToken>()), Times.Never());
    }

    private class TestProjectSnapshotManager : DefaultProjectSnapshotManager
    {
        public TestProjectSnapshotManager(IEnumerable<IProjectSnapshotChangeTrigger> triggers, Workspace workspace, ProjectSnapshotManagerDispatcher dispatcher)
            : base(Mock.Of<IErrorReporter>(MockBehavior.Strict), triggers, workspace, dispatcher)
        {
        }

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
