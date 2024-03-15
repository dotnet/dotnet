// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
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

        _projectManager = CreateProjectSnapshotManager();

        _projectWorkspaceStateWithTagHelpers = ProjectWorkspaceState.Create(someTagHelpers);

        _sourceText = SourceText.From("Hello world");
    }

    [UIFact]
    public async Task Initialize_DoneInCorrectOrderBasedOnInitializePriorityPriority()
    {
        // Arrange
        var initializedOrder = new List<string>();
        var projectManager = CreateProjectSnapshotManager();
        projectManager.Changed += delegate { initializedOrder.Add("lowPriority"); };
        projectManager.PriorityChanged += delegate { initializedOrder.Add("highPriority"); };

        // Act
        await RunOnDispatcherAsync(() =>
        {
            projectManager.ProjectAdded(
                new("C:/path/to/project.csproj", "C:/path/to/obj", RazorConfiguration.Default, rootNamespace: null));
        });

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

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[0], null!);
        });

        // Assert
        var project = _projectManager.GetLoadedProject(s_hostProject.Key);
        Assert.Single(project.DocumentFilePaths,
            filePath => filePath == s_documents[0].FilePath);

        listener.AssertNotifications(
            x => x.DocumentAdded());
    }

    [UIFact]
    public async Task DocumentAdded_AddsDocument_Legacy()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
        });

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[0], null!);
        });

        // Assert
        var project = _projectManager.GetLoadedProject(s_hostProject.Key);
        Assert.Single(
            project.DocumentFilePaths,
            filePath => filePath == s_documents[0].FilePath &&
                        project.GetDocument(filePath).AssumeNotNull().FileKind == FileKinds.Legacy);

        listener.AssertNotifications(
            x => x.DocumentAdded());
    }

    [UIFact]
    public async Task DocumentAdded_AddsDocument_Component()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
        });

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[3], null!);
        });

        // Assert
        var project = _projectManager.GetLoadedProject(s_hostProject.Key);
        Assert.Single(
            project.DocumentFilePaths,
            filePath => filePath == s_documents[3].FilePath &&
                        project.GetDocument(filePath).AssumeNotNull().FileKind == FileKinds.Component);

        listener.AssertNotifications(
            x => x.DocumentAdded());
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

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[0], null!);
        });

        // Assert
        var project = _projectManager.GetLoadedProject(s_hostProject.Key);
        Assert.Single(project.DocumentFilePaths,
            filePath => filePath == s_documents[0].FilePath);

        listener.AssertNoNotifications();
    }

    [UIFact]
    public async Task DocumentAdded_IgnoresUnknownProject()
    {
        // Arrange

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[0], null!);
        });

        // Assert
        var projectKeys = _projectManager.GetAllProjectKeys(s_hostProject.FilePath);
        Assert.Empty(projectKeys);
    }

    [UIFact]
    public async Task DocumentAdded_NullLoader_HasEmptyText()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
        });

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[0], null!);
        });

        // Assert
        var project = _projectManager.GetLoadedProject(s_hostProject.Key);
        var documentFilePath = Assert.Single(project.DocumentFilePaths);
        var document = project.GetDocument(documentFilePath);
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

        var expected = SourceText.From("Hello");

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[0], TextLoader.From(TextAndVersion.Create(expected, VersionStamp.Default)));
        });

        // Assert
        var project = _projectManager.GetLoadedProject(s_hostProject.Key);
        var documentFilePath = Assert.Single(project.DocumentFilePaths);
        var document = project.GetDocument(documentFilePath);
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

        var originalTagHelpers = await _projectManager.GetLoadedProject(s_hostProject.Key).GetTagHelpersAsync(DisposalToken);

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[0], null!);
        });

        // Assert
        var newTagHelpers = await _projectManager.GetLoadedProject(s_hostProject.Key).GetTagHelpersAsync(DisposalToken);

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

        var project = _projectManager.GetLoadedProject(s_hostProject.Key);
        var projectEngine = project.GetProjectEngine();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[0], null!);
        });

        // Assert
        project = _projectManager.GetLoadedProject(s_hostProject.Key);
        Assert.Same(projectEngine, project.GetProjectEngine());
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

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentRemoved(s_hostProject.Key, s_documents[1]);
        });

        // Assert
        var project = _projectManager.GetLoadedProject(s_hostProject.Key);
        Assert.Collection(
            project.DocumentFilePaths.OrderBy(f => f),
            f => Assert.Equal(s_documents[2].FilePath, f),
            f => Assert.Equal(s_documents[0].FilePath, f));

        listener.AssertNotifications(
            x => x.DocumentRemoved());
    }

    [UIFact]
    public async Task DocumentRemoved_IgnoresNotFoundDocument()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
        });

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentRemoved(s_hostProject.Key, s_documents[0]);
        });

        // Assert
        var project = _projectManager.GetLoadedProject(s_hostProject.Key);
        Assert.Empty(project.DocumentFilePaths);

        listener.AssertNoNotifications();
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
        var projectKeys = _projectManager.GetAllProjectKeys(s_hostProject.FilePath);
        Assert.Empty(projectKeys);
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

        var originalTagHelpers = await _projectManager.GetLoadedProject(s_hostProject.Key).GetTagHelpersAsync(DisposalToken);

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentRemoved(s_hostProject.Key, s_documents[1]);
        });

        // Assert
        var newTagHelpers = await _projectManager.GetLoadedProject(s_hostProject.Key).GetTagHelpersAsync(DisposalToken);

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

        var project = _projectManager.GetLoadedProject(s_hostProject.Key);
        var projectEngine = project.GetProjectEngine();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentRemoved(s_hostProject.Key, s_documents[1]);
        });

        // Assert
        project = _projectManager.GetLoadedProject(s_hostProject.Key);
        Assert.Same(projectEngine, project.GetProjectEngine());
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

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentOpened(s_hostProject.Key, s_documents[0].FilePath, _sourceText);
        });

        // Assert
        listener.AssertNotifications(
            x => x.DocumentChanged());

        var project = _projectManager.GetLoadedProject(s_hostProject.Key);
        var document = project.GetDocument(s_documents[0].FilePath);
        Assert.NotNull(document);
        var text = await document.GetTextAsync();
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

        using var listener = _projectManager.ListenToNotifications();

        var expected = SourceText.From("Hi");
        var textAndVersion = TextAndVersion.Create(expected, VersionStamp.Create());

        Assert.True(_projectManager.IsDocumentOpen(s_documents[0].FilePath));

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentClosed(s_hostProject.Key, s_documents[0].FilePath, TextLoader.From(textAndVersion));
        });

        // Assert
        listener.AssertNotifications(
            x => x.DocumentChanged());

        var project = _projectManager.GetLoadedProject(s_hostProject.Key);
        var document = project.GetDocument(s_documents[0].FilePath);
        Assert.NotNull(document);
        var text = await document.GetTextAsync();
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

        using var listener = _projectManager.ListenToNotifications();

        var expected = SourceText.From("Hi");
        var textAndVersion = TextAndVersion.Create(expected, VersionStamp.Create());

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentClosed(s_hostProject.Key, s_documents[0].FilePath, TextLoader.From(textAndVersion));
        });

        // Assert
        listener.AssertNotifications(
            x => x.DocumentChanged());

        var project = _projectManager.GetLoadedProject(s_hostProject.Key);
        var document = project.GetDocument(s_documents[0].FilePath);
        Assert.NotNull(document);
        var text = await document.GetTextAsync();
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

        using var listener = _projectManager.ListenToNotifications();

        var expected = SourceText.From("Hi");

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentChanged(s_hostProject.Key, s_documents[0].FilePath, expected);
        });

        // Assert
        listener.AssertNotifications(
            x => x.DocumentChanged());

        var project = _projectManager.GetLoadedProject(s_hostProject.Key);
        var document = project.GetDocument(s_documents[0].FilePath);
        Assert.NotNull(document);
        var text = await document.GetTextAsync();
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

        using var listener = _projectManager.ListenToNotifications();

        var expected = SourceText.From("Hi");
        var textAndVersion = TextAndVersion.Create(expected, VersionStamp.Create());

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentChanged(s_hostProject.Key, s_documents[0].FilePath, TextLoader.From(textAndVersion));
        });

        // Assert
        listener.AssertNotifications(
            x => x.DocumentChanged());

        var project = _projectManager.GetLoadedProject(s_hostProject.Key);
        var document = project.GetDocument(s_documents[0].FilePath);
        Assert.NotNull(document);
        var text = await document.GetTextAsync();
        Assert.Same(expected, text);
    }

    [UIFact]
    public async Task ProjectAdded_WithoutWorkspaceProject_NotifiesListeners()
    {
        // Arrange
        using var listener = _projectManager.ListenToNotifications();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
        });

        // Assert
        listener.AssertNotifications(
            x => x.ProjectAdded());
    }

    [UIFact]
    public async Task ProjectConfigurationChanged_ConfigurationChange_ProjectWorkspaceState_NotifiesListeners()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
        });

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectConfigurationChanged(s_hostProjectWithConfigurationChange);
        });

        // Assert
        listener.AssertNotifications(
            x => x.ProjectChanged());
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

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectConfigurationChanged(s_hostProjectWithConfigurationChange);
        });

        // Assert
        listener.AssertNotifications(
            x => x.ProjectChanged());
    }

    [UIFact]
    public async Task ProjectConfigurationChanged_ConfigurationChange_DoesNotCacheProjectEngine()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
        });

        var project = _projectManager.GetLoadedProject(s_hostProject.Key);
        var projectEngine = project.GetProjectEngine();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectConfigurationChanged(s_hostProjectWithConfigurationChange);
        });

        // Assert
        project = _projectManager.GetLoadedProject(s_hostProjectWithConfigurationChange.Key);
        Assert.NotSame(projectEngine, project.GetProjectEngine());
    }

    [UIFact]
    public async Task ProjectConfigurationChanged_IgnoresUnknownProject()
    {
        // Arrange
        using var listener = _projectManager.ListenToNotifications();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectConfigurationChanged(s_hostProject);
        });

        // Assert
        Assert.Empty(_projectManager.GetProjects());

        listener.AssertNoNotifications();
    }

    [UIFact]
    public async Task ProjectRemoved_RemovesProject_NotifiesListeners()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
        });

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectRemoved(s_hostProject.Key);
        });

        // Assert
        Assert.Empty(_projectManager.GetProjects());

        listener.AssertNotifications(
            x => x.ProjectRemoved());
    }

    [UIFact]
    public void ProjectWorkspaceStateChanged_WithoutHostProject_IgnoresWorkspaceState()
    {
        // Arrange
        using var listener = _projectManager.ListenToNotifications();

        // Act
        _projectManager.ProjectWorkspaceStateChanged(s_hostProject.Key, _projectWorkspaceStateWithTagHelpers);

        // Assert
        Assert.Empty(_projectManager.GetProjects());

        listener.AssertNoNotifications();
    }

    [UIFact]
    public async Task ProjectWorkspaceStateChanged_WithHostProject_FirstTime_NotifiesListeners()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
        });

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectWorkspaceStateChanged(s_hostProject.Key, _projectWorkspaceStateWithTagHelpers);
        });

        // Assert
        listener.AssertNotifications(
            x => x.ProjectChanged());
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

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectWorkspaceStateChanged(s_hostProject.Key, _projectWorkspaceStateWithTagHelpers);
        });

        // Assert
        listener.AssertNotifications(
            x => x.ProjectChanged());
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

        using var listener = _projectManager.ListenToNotifications();

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

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[0], null!);
        });

        // Assert
        listener.AssertNotifications(
            x => x.DocumentAdded(),
            x => x.DocumentChanged(),
            x => x.DocumentRemoved());
    }

    [UIFact]
    public async Task SolutionClosing_ProjectChangedEventsCorrect()
    {
        // Arrange
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.ProjectAdded(s_hostProject);
        });

        using var listener = _projectManager.ListenToNotifications();

        var textLoader = new Mock<TextLoader>(MockBehavior.Strict);

        // Act
        await RunOnDispatcherAsync(() =>
        {
            _projectManager.SolutionClosed();
            _projectManager.DocumentAdded(s_hostProject.Key, s_documents[0], textLoader.Object);
        });

        // Assert
        listener.AssertNotifications(
            x => x.DocumentAdded(solutionIsClosing: true));

        textLoader.Verify(d => d.LoadTextAndVersionAsync(It.IsAny<LoadTextOptions>(), It.IsAny<CancellationToken>()), Times.Never());
    }
}
