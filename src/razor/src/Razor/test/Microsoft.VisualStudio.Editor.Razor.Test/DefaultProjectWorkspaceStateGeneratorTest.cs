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
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

public class DefaultProjectWorkspaceStateGeneratorTest : ProjectSnapshotManagerDispatcherTestBase
{
    private readonly ImmutableArray<TagHelperDescriptor> _resolvableTagHelpers;
    private readonly Workspace _workspace;
    private readonly Project _workspaceProject;
    private readonly ProjectSnapshot _projectSnapshot;
    private readonly ProjectWorkspaceState _projectWorkspaceStateWithTagHelpers;

    public DefaultProjectWorkspaceStateGeneratorTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        var tagHelperResolver = new TestTagHelperResolver()
        {
            TagHelpers = ImmutableArray.Create(TagHelperDescriptorBuilder.Create("ResolvableTagHelper", "TestAssembly").Build())
        };

        _resolvableTagHelpers = tagHelperResolver.TagHelpers;
        var workspaceServices = new List<IWorkspaceService>() { tagHelperResolver };
        var testServices = TestServices.Create(workspaceServices, Enumerable.Empty<ILanguageService>());
        _workspace = TestWorkspace.Create(testServices);
        AddDisposable(_workspace);
        var projectId = ProjectId.CreateNewId("Test");
        var solution = _workspace.CurrentSolution.AddProject(ProjectInfo.Create(
            projectId,
            VersionStamp.Default,
            "Test",
            "Test",
            LanguageNames.CSharp,
            TestProjectData.SomeProject.FilePath));
        _workspaceProject = solution.GetProject(projectId);
        _projectSnapshot = new ProjectSnapshot(ProjectState.Create(_workspace.Services, TestProjectData.SomeProject));
        _projectWorkspaceStateWithTagHelpers = new ProjectWorkspaceState(ImmutableArray.Create(
            TagHelperDescriptorBuilder.Create("TestTagHelper", "TestAssembly").Build()),
            csharpLanguageVersion: default);
    }

    [UIFact]
    public void Dispose_MakesUpdateNoop()
    {
        // Arrange
        using (var stateGenerator = new DefaultProjectWorkspaceStateGenerator(Dispatcher))
        {
            stateGenerator.BlockBackgroundWorkStart = new ManualResetEventSlim(initialState: false);

            // Act
            stateGenerator.Dispose();
            stateGenerator.Update(_workspaceProject, _projectSnapshot, DisposalToken);

            // Assert
            Assert.Empty(stateGenerator.Updates);
        }
    }

    [UIFact]
    public void Update_StartsUpdateTask()
    {
        // Arrange
        using (var stateGenerator = new DefaultProjectWorkspaceStateGenerator(Dispatcher))
        {
            stateGenerator.BlockBackgroundWorkStart = new ManualResetEventSlim(initialState: false);

            // Act
            stateGenerator.Update(_workspaceProject, _projectSnapshot, DisposalToken);

            // Assert
            var update = Assert.Single(stateGenerator.Updates);
            Assert.False(update.Value.Task.IsCompleted);
        }
    }

    [UIFact]
    public void Update_SoftCancelsIncompleteTaskForSameProject()
    {
        // Arrange
        using (var stateGenerator = new DefaultProjectWorkspaceStateGenerator(Dispatcher))
        {
            stateGenerator.BlockBackgroundWorkStart = new ManualResetEventSlim(initialState: false);
            stateGenerator.Update(_workspaceProject, _projectSnapshot, DisposalToken);
            var initialUpdate = stateGenerator.Updates.Single().Value;

            // Act
            stateGenerator.Update(_workspaceProject, _projectSnapshot, DisposalToken);

            // Assert
            Assert.True(initialUpdate.Cts.IsCancellationRequested);
        }
    }

    [UIFact]
    public async Task Update_NullWorkspaceProject_ClearsProjectWorkspaceState()
    {
        // Arrange
        using (var stateGenerator = new DefaultProjectWorkspaceStateGenerator(Dispatcher))
        {
            stateGenerator.NotifyBackgroundWorkCompleted = new ManualResetEventSlim(initialState: false);
            var projectManager = new TestProjectSnapshotManager(_workspace, Dispatcher);
            stateGenerator.Initialize(projectManager);
            projectManager.ProjectAdded(_projectSnapshot.HostProject);
            projectManager.ProjectWorkspaceStateChanged(_projectSnapshot.Key, _projectWorkspaceStateWithTagHelpers);

            // Act
            stateGenerator.Update(workspaceProject: null, _projectSnapshot, DisposalToken);

            // Jump off the UI thread so the background work can complete.
            await Task.Run(() => stateGenerator.NotifyBackgroundWorkCompleted.Wait(TimeSpan.FromSeconds(3)));

            // Assert
            var newProjectSnapshot = projectManager.GetLoadedProject(_projectSnapshot.Key);
            Assert.Empty(newProjectSnapshot.TagHelpers);
        }
    }

    [UIFact]
    public async Task Update_ResolvesTagHelpersAndUpdatesWorkspaceState()
    {
        // Arrange
        using (var stateGenerator = new DefaultProjectWorkspaceStateGenerator(Dispatcher))
        {
            stateGenerator.NotifyBackgroundWorkCompleted = new ManualResetEventSlim(initialState: false);
            var projectManager = new TestProjectSnapshotManager(_workspace, Dispatcher);
            stateGenerator.Initialize(projectManager);
            projectManager.ProjectAdded(_projectSnapshot.HostProject);

            // Act
            stateGenerator.Update(_workspaceProject, _projectSnapshot, DisposalToken);

            // Jump off the UI thread so the background work can complete.
            await Task.Run(() => stateGenerator.NotifyBackgroundWorkCompleted.Wait(TimeSpan.FromSeconds(3)));

            // Assert
            var newProjectSnapshot = projectManager.GetLoadedProject(_projectSnapshot.Key);
            Assert.Equal(_resolvableTagHelpers, newProjectSnapshot.TagHelpers, TagHelperDescriptorComparer.Default);
        }
    }
}
