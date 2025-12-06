// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Threading;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.ProjectSystem;

public class ProjectSnapshotManagerTest : VisualStudioWorkspaceTestBase
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

    private static readonly HostProject s_hostProject = TestProjectData.SomeProject with
    {
        Configuration = FallbackRazorConfiguration.MVC_2_0
    };

    private static readonly HostProject s_hostProjectWithConfigurationChange = TestProjectData.SomeProject with
    {
        Configuration = FallbackRazorConfiguration.MVC_1_0
    };

    private readonly ProjectWorkspaceState _projectWorkspaceStateWithTagHelpers;
    private readonly TestProjectSnapshotManager _projectManager;
    private readonly SourceText _sourceText;

    public ProjectSnapshotManagerTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _projectManager = CreateProjectSnapshotManager();

        _projectWorkspaceStateWithTagHelpers = ProjectWorkspaceState.Create([
            TagHelperDescriptorBuilder.CreateTagHelper("Test1", "TestAssembly").Build()]);

        _sourceText = SourceText.From("Hello world");
    }

    [UIFact]
    public async Task Initialize_DoneInCorrectOrderBasedOnInitializePriority()
    {
        // Arrange
        var initializedOrder = new List<string>();
        var projectManager = CreateProjectSnapshotManager();
        projectManager.Changed += delegate { initializedOrder.Add("lowPriority"); };
        projectManager.PriorityChanged += delegate { initializedOrder.Add("highPriority"); };

        // Act
        await projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(
                new("C:/path/to/project.csproj", "C:/path/to/obj", RazorConfiguration.Default, rootNamespace: null));
        });

        // Assert
        Assert.Equal(["highPriority", "lowPriority"], initializedOrder);
    }

    [UIFact]
    public async Task AddDocument_AddsDocument()
    {
        // Arrange
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
        });

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddDocument(s_hostProject.Key, s_documents[0], EmptyTextLoader.Instance);
        });

        // Assert
        var project = _projectManager.GetRequiredProject(s_hostProject.Key);
        Assert.Single(project.DocumentFilePaths,
            filePath => filePath == s_documents[0].FilePath);

        listener.AssertNotifications(
            x => x.DocumentAdded());
    }

    [UIFact]
    public async Task AddDocument_AddsDocument_Legacy()
    {
        // Arrange
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
        });

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddDocument(s_hostProject.Key, s_documents[0], EmptyTextLoader.Instance);
        });

        // Assert
        var project = _projectManager.GetRequiredProject(s_hostProject.Key);
        Assert.Single(
            project.DocumentFilePaths,
            filePath => filePath == s_documents[0].FilePath &&
                        project.GetRequiredDocument(filePath).FileKind == RazorFileKind.Legacy);

        listener.AssertNotifications(
            x => x.DocumentAdded());
    }

    [UIFact]
    public async Task AddDocument_AddsDocument_Component()
    {
        // Arrange
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
        });

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddDocument(s_hostProject.Key, s_documents[3], EmptyTextLoader.Instance);
        });

        // Assert
        var project = _projectManager.GetRequiredProject(s_hostProject.Key);
        Assert.Single(
            project.DocumentFilePaths,
            filePath => filePath == s_documents[3].FilePath &&
                        project.GetRequiredDocument(filePath).FileKind == RazorFileKind.Component);

        listener.AssertNotifications(
            x => x.DocumentAdded());
    }

    [UIFact]
    public async Task AddDocument_IgnoresDuplicate()
    {
        // Arrange
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
            updater.AddDocument(s_hostProject.Key, s_documents[0], EmptyTextLoader.Instance);
        });

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddDocument(s_hostProject.Key, s_documents[0], EmptyTextLoader.Instance);
        });

        // Assert
        var project = _projectManager.GetRequiredProject(s_hostProject.Key);
        Assert.Single(project.DocumentFilePaths,
            filePath => filePath == s_documents[0].FilePath);

        listener.AssertNoNotifications();

        Assert.Equal(1, project.GetRequiredDocument(s_documents[0].FilePath).Version);
    }

    [UIFact]
    public async Task AddDocument_IgnoresUnknownProject()
    {
        // Arrange

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddDocument(s_hostProject.Key, s_documents[0], StrictMock.Of<TextLoader>());
        });

        // Assert
        var projectKeys = _projectManager.GetProjectKeysWithFilePath(s_hostProject.FilePath);
        Assert.Empty(projectKeys);
    }

    [UIFact]
    public async Task AddDocument_EmptyLoader_HasEmptyText()
    {
        // Arrange
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
        });

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddDocument(s_hostProject.Key, s_documents[0], EmptyTextLoader.Instance);
        });

        // Assert
        var project = _projectManager.GetRequiredProject(s_hostProject.Key);
        var documentFilePath = Assert.Single(project.DocumentFilePaths);
        var document = project.GetRequiredDocument(documentFilePath);

        var text = await document.GetTextAsync(DisposalToken);
        Assert.Equal(0, text.Length);
    }

    [UIFact]
    public async Task AddDocument_WithLoader_LoadsText()
    {
        // Arrange
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
        });

        var expected = SourceText.From("Hello");

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddDocument(s_hostProject.Key, s_documents[0], TextLoader.From(TextAndVersion.Create(expected, VersionStamp.Default)));
        });

        // Assert
        var project = _projectManager.GetRequiredProject(s_hostProject.Key);
        var documentFilePath = Assert.Single(project.DocumentFilePaths);
        var document = project.GetRequiredDocument(documentFilePath);

        var actual = await document.GetTextAsync(DisposalToken);
        Assert.Same(expected, actual);
    }

    [UIFact]
    public async Task AddDocument_CachesTagHelpers()
    {
        // Arrange
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
            updater.UpdateProjectWorkspaceState(s_hostProject.Key, _projectWorkspaceStateWithTagHelpers);
        });

        var originalTagHelpers = await _projectManager
            .GetRequiredProject(s_hostProject.Key)
            .GetTagHelpersAsync(DisposalToken);

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddDocument(s_hostProject.Key, s_documents[0], EmptyTextLoader.Instance);
        });

        // Assert
        var newTagHelpers = await _projectManager
            .GetRequiredProject(s_hostProject.Key)
            .GetTagHelpersAsync(DisposalToken);

        Assert.SameItems(originalTagHelpers, newTagHelpers);
    }

    [UIFact]
    public async Task AddDocument_CachesProjectEngine()
    {
        // Arrange
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
        });

        var project = _projectManager.GetRequiredProject(s_hostProject.Key);
        var projectEngine = project.ProjectEngine;

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddDocument(s_hostProject.Key, s_documents[0], EmptyTextLoader.Instance);
        });

        // Assert
        var newProjectEngine = _projectManager.GetRequiredProject(s_hostProject.Key).ProjectEngine;

        Assert.Same(projectEngine, newProjectEngine);
    }

    [UIFact]
    public async Task RemoveDocument_RemovesDocument()
    {
        // Arrange
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
            updater.AddDocument(s_hostProject.Key, s_documents[0], EmptyTextLoader.Instance);
            updater.AddDocument(s_hostProject.Key, s_documents[1], EmptyTextLoader.Instance);
            updater.AddDocument(s_hostProject.Key, s_documents[2], EmptyTextLoader.Instance);
        });

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.RemoveDocument(s_hostProject.Key, s_documents[1].FilePath);
        });

        // Assert
        var project = _projectManager.GetRequiredProject(s_hostProject.Key);
        Assert.Collection(
            project.DocumentFilePaths.OrderBy(f => f),
            f => Assert.Equal(s_documents[2].FilePath, f),
            f => Assert.Equal(s_documents[0].FilePath, f));

        listener.AssertNotifications(
            x => x.DocumentRemoved());
    }

    [UIFact]
    public async Task RemoveDocument_IgnoresNotFoundDocument()
    {
        // Arrange
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
        });

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.RemoveDocument(s_hostProject.Key, s_documents[0].FilePath);
        });

        // Assert
        var project = _projectManager.GetRequiredProject(s_hostProject.Key);
        Assert.Empty(project.DocumentFilePaths);

        listener.AssertNoNotifications();
    }

    [UIFact]
    public async Task RemoveDocument_IgnoresUnknownProject()
    {
        // Arrange

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.RemoveDocument(s_hostProject.Key, s_documents[0].FilePath);
        });

        // Assert
        var projectKeys = _projectManager.GetProjectKeysWithFilePath(s_hostProject.FilePath);
        Assert.Empty(projectKeys);
    }

    [UIFact]
    public async Task RemoveDocument_CachesTagHelpers()
    {
        // Arrange
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
            updater.UpdateProjectWorkspaceState(s_hostProject.Key, _projectWorkspaceStateWithTagHelpers);
            updater.AddDocument(s_hostProject.Key, s_documents[0], EmptyTextLoader.Instance);
            updater.AddDocument(s_hostProject.Key, s_documents[1], EmptyTextLoader.Instance);
            updater.AddDocument(s_hostProject.Key, s_documents[2], EmptyTextLoader.Instance);
        });

        var originalTagHelpers = await _projectManager
            .GetRequiredProject(s_hostProject.Key)
            .GetTagHelpersAsync(DisposalToken);

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.RemoveDocument(s_hostProject.Key, s_documents[1].FilePath);
        });

        // Assert
        var newTagHelpers = await _projectManager
            .GetRequiredProject(s_hostProject.Key)
            .GetTagHelpersAsync(DisposalToken);

        Assert.SameItems(originalTagHelpers, newTagHelpers);
    }

    [UIFact]
    public async Task RemoveDocument_CachesProjectEngine()
    {
        // Arrange
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
            updater.AddDocument(s_hostProject.Key, s_documents[0], EmptyTextLoader.Instance);
            updater.AddDocument(s_hostProject.Key, s_documents[1], EmptyTextLoader.Instance);
            updater.AddDocument(s_hostProject.Key, s_documents[2], EmptyTextLoader.Instance);
        });

        var project = _projectManager.GetRequiredProject(s_hostProject.Key);
        var projectEngine = project.ProjectEngine;

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.RemoveDocument(s_hostProject.Key, s_documents[1].FilePath);
        });

        // Assert
        var newProjectEngine = _projectManager.GetRequiredProject(s_hostProject.Key).ProjectEngine;

        Assert.Same(projectEngine, newProjectEngine);
    }

    [UIFact]
    public async Task OpenDocument_UpdatesDocument()
    {
        // Arrange
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
            updater.AddDocument(s_hostProject.Key, s_documents[0], EmptyTextLoader.Instance);
        });

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.OpenDocument(s_hostProject.Key, s_documents[0].FilePath, _sourceText);
        });

        // Assert
        listener.AssertNotifications(
            x => x.DocumentChanged());

        var document = _projectManager.GetRequiredDocument(s_hostProject.Key, s_documents[0].FilePath);

        var text = await document.GetTextAsync(DisposalToken);
        Assert.Same(_sourceText, text);

        Assert.True(_projectManager.IsDocumentOpen(s_documents[0].FilePath));

        Assert.Equal(2, document.Version);
    }

    [UIFact]
    public async Task CloseDocument_UpdatesDocument()
    {
        // Arrange
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
            updater.AddDocument(s_hostProject.Key, s_documents[0], EmptyTextLoader.Instance);
            updater.OpenDocument(s_hostProject.Key, s_documents[0].FilePath, _sourceText);
        });

        using var listener = _projectManager.ListenToNotifications();

        var expected = SourceText.From("Hi");
        var textAndVersion = TextAndVersion.Create(expected, VersionStamp.Create());

        Assert.True(_projectManager.IsDocumentOpen(s_documents[0].FilePath));

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.CloseDocument(s_hostProject.Key, s_documents[0].FilePath, TextLoader.From(textAndVersion));
        });

        // Assert
        listener.AssertNotifications(
            x => x.DocumentChanged());

        var document = _projectManager.GetRequiredDocument(s_hostProject.Key, s_documents[0].FilePath);

        var text = await document.GetTextAsync(DisposalToken);
        Assert.Same(expected, text);
        Assert.False(_projectManager.IsDocumentOpen(s_documents[0].FilePath));
        Assert.Equal(3, document.Version);
    }

    [UIFact]
    public async Task CloseDocument_AcceptsChange()
    {
        // Arrange
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
            updater.AddDocument(s_hostProject.Key, s_documents[0], EmptyTextLoader.Instance);
        });

        using var listener = _projectManager.ListenToNotifications();

        var expected = SourceText.From("Hi");
        var textAndVersion = TextAndVersion.Create(expected, VersionStamp.Create());

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.CloseDocument(s_hostProject.Key, s_documents[0].FilePath, TextLoader.From(textAndVersion));
        });

        // Assert
        listener.AssertNotifications(
            x => x.DocumentChanged());

        var document = _projectManager.GetRequiredDocument(s_hostProject.Key, s_documents[0].FilePath);

        var text = await document.GetTextAsync(DisposalToken);
        Assert.Same(expected, text);
    }

    [UIFact]
    public async Task UpdateDocumentText_Snapshot_UpdatesDocument()
    {
        // Arrange
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
            updater.AddDocument(s_hostProject.Key, s_documents[0], EmptyTextLoader.Instance);
            updater.OpenDocument(s_hostProject.Key, s_documents[0].FilePath, _sourceText);
        });

        using var listener = _projectManager.ListenToNotifications();

        var expected = SourceText.From("Hi");

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.UpdateDocumentText(s_hostProject.Key, s_documents[0].FilePath, expected);
        });

        // Assert
        listener.AssertNotifications(
            x => x.DocumentChanged());

        var document = _projectManager.GetRequiredDocument(s_hostProject.Key, s_documents[0].FilePath);

        var text = await document.GetTextAsync(DisposalToken);
        Assert.Same(expected, text);
        Assert.Equal(3, document.Version);
    }

    [UIFact]
    public async Task UpdateDocumentText_Loader_UpdatesDocument()
    {
        // Arrange
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
            updater.AddDocument(s_hostProject.Key, s_documents[0], EmptyTextLoader.Instance);
            updater.OpenDocument(s_hostProject.Key, s_documents[0].FilePath, _sourceText);
        });

        using var listener = _projectManager.ListenToNotifications();

        var expected = SourceText.From("Hi");
        var textAndVersion = TextAndVersion.Create(expected, VersionStamp.Create());

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.UpdateDocumentText(s_hostProject.Key, s_documents[0].FilePath, TextLoader.From(textAndVersion));
        });

        // Assert
        listener.AssertNotifications(
            x => x.DocumentChanged());

        var document = _projectManager.GetRequiredDocument(s_hostProject.Key, s_documents[0].FilePath);

        var text = await document.GetTextAsync(DisposalToken);
        Assert.Same(expected, text);
        Assert.Equal(3, document.Version);
    }

    [UIFact]
    public async Task AddProject_WithoutWorkspaceProject_NotifiesListeners()
    {
        // Arrange
        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
        });

        // Assert
        listener.AssertNotifications(
            x => x.ProjectAdded());
    }

    [UIFact]
    public async Task UpdateProjectConfiguration_ConfigurationChange_NotifiesListeners()
    {
        // Arrange
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
        });

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.UpdateProjectConfiguration(s_hostProjectWithConfigurationChange);
        });

        // Assert
        listener.AssertNotifications(
            x => x.ProjectChanged());
    }

    [UIFact]
    public async Task UpdateProjectWorkspaceState_ConfigurationChange_WithProjectWorkspaceState_NotifiesListeners()
    {
        // Arrange
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
            updater.UpdateProjectWorkspaceState(s_hostProject.Key, _projectWorkspaceStateWithTagHelpers);
        });

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.UpdateProjectConfiguration(s_hostProjectWithConfigurationChange);
        });

        // Assert
        listener.AssertNotifications(
            x => x.ProjectChanged());
    }

    [UIFact]
    public async Task UpdateProjectWorkspaceState_ConfigurationChange_DoesNotCacheProjectEngine()
    {
        // Arrange
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
        });

        var project = _projectManager.GetRequiredProject(s_hostProject.Key);
        var projectEngine = project.ProjectEngine;

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.UpdateProjectConfiguration(s_hostProjectWithConfigurationChange);
        });

        // Assert
        var newProjectEngine = _projectManager.GetRequiredProject(s_hostProjectWithConfigurationChange.Key).ProjectEngine;

        Assert.NotSame(projectEngine, newProjectEngine);
    }

    [UIFact]
    public async Task UpdateProjectWorkspaceState_IgnoresUnknownProject()
    {
        // Arrange
        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.UpdateProjectConfiguration(s_hostProject);
        });

        // Assert
        Assert.Empty(_projectManager.GetProjects());

        listener.AssertNoNotifications();
    }

    [UIFact]
    public async Task RemoveProject_RemovesProject_NotifiesListeners()
    {
        // Arrange
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
        });

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.RemoveProject(s_hostProject.Key);
        });

        // Assert
        Assert.Empty(_projectManager.GetProjects());

        listener.AssertNotifications(
            x => x.ProjectRemoved());
    }

    [UIFact]
    public async Task UpdateProjectWorkspaceState_UnknownProject()
    {
        // Arrange
        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.UpdateProjectWorkspaceState(s_hostProject.Key, _projectWorkspaceStateWithTagHelpers);
        });

        // Assert
        Assert.Empty(_projectManager.GetProjects());

        listener.AssertNoNotifications();
    }

    [UIFact]
    public async Task UpdateProjectWorkspaceState_UpdateDocuments()
    {
        // Arrange
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
            updater.AddDocument(s_hostProject.Key, s_documents[0], EmptyTextLoader.Instance);
        });

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.UpdateProjectWorkspaceState(s_hostProject.Key, _projectWorkspaceStateWithTagHelpers);
        });

        // Assert
        var document = _projectManager.GetRequiredDocument(s_hostProject.Key, s_documents[0].FilePath);

        Assert.Equal(2, document.Version);
    }

    [UIFact]
    public async Task UpdateProjectWorkspaceState_KnownProject_FirstTime_NotifiesListeners()
    {
        // Arrange
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
        });

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.UpdateProjectWorkspaceState(s_hostProject.Key, _projectWorkspaceStateWithTagHelpers);
        });

        // Assert
        listener.AssertNotifications(
            x => x.ProjectChanged());
    }

    [UIFact]
    public async Task UpdateProjectWorkspaceState_KnownProject_NotifiesListeners()
    {
        // Arrange
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
            updater.UpdateProjectWorkspaceState(s_hostProject.Key, ProjectWorkspaceState.Default);
        });

        using var listener = _projectManager.ListenToNotifications();

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.UpdateProjectWorkspaceState(s_hostProject.Key, _projectWorkspaceStateWithTagHelpers);
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

        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
        });

        using var listener = _projectManager.ListenToNotifications();

        _projectManager.Changed += (sender, args) =>
        {
            // These conditions will result in a triply nested change notification of Add -> Change -> Remove all within the .Change chain.

            if (args.Kind == ProjectChangeKind.DocumentAdded)
            {
                _projectManager.UpdateAsync(updater =>
                    updater.OpenDocument(s_hostProject.Key, s_documents[0].FilePath, _sourceText)).Forget();
            }
            else if (args.Kind == ProjectChangeKind.DocumentChanged)
            {
                _projectManager.UpdateAsync(updater =>
                    updater.RemoveDocument(s_hostProject.Key, s_documents[0].FilePath)).Forget();
            }
        };

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddDocument(s_hostProject.Key, s_documents[0], EmptyTextLoader.Instance);
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
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
        });

        using var listener = _projectManager.ListenToNotifications();

        var textLoader = new Mock<TextLoader>(MockBehavior.Strict);

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.SolutionClosed();
            updater.AddDocument(s_hostProject.Key, s_documents[0], textLoader.Object);
        });

        // Assert
        listener.AssertNotifications(
            x => x.DocumentAdded(solutionIsClosing: true));

        textLoader.Verify(d => d.LoadTextAndVersionAsync(It.IsAny<LoadTextOptions>(), It.IsAny<CancellationToken>()), Times.Never());
    }

    [Fact]
    public async Task SolutionClosing_RemovesProjectAndClosesDocument()
    {
        // Arrange

        // Add project and open document.
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
            updater.AddDocument(s_hostProject.Key, s_documents[0], EmptyTextLoader.Instance);
            updater.OpenDocument(s_hostProject.Key, s_documents[0].FilePath, _sourceText);
        });

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.SolutionClosed();
            updater.RemoveProject(s_hostProject.Key);
            updater.CloseDocument(s_hostProject.Key, s_documents[0].FilePath, EmptyTextLoader.Instance);
        });

        // Assert
        Assert.False(_projectManager.ContainsDocument(s_hostProject.Key, s_documents[0].FilePath));
        Assert.False(_projectManager.ContainsProject(s_hostProject.Key));
        Assert.False(_projectManager.IsDocumentOpen(s_documents[0].FilePath));
    }
}
