// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.RoslynWorkspace.Test;

public class RazorWorkspaceListenerTest
{
    [Fact]
    public async Task ProjectAdded_SchedulesTask()
    {
        using var workspace = new AdhocWorkspace(CodeAnalysis.Host.Mef.MefHostServices.DefaultHost);

        using var listener = new TestRazorWorkspaceListener();
        listener.EnsureInitialized(workspace, "temp.json");

        var project = workspace.AddProject("TestProject", LanguageNames.CSharp);
        listener.NotifyDynamicFile(project.Id);

        await listener.WaitForDebounceAsync();

        Assert.Equal(1, listener.SerializeCalls[project.Id]);
    }

    [Fact]
    public async Task TwoProjectsAdded_OneWithDynamicFiles_SchedulesOneTask()
    {
        using var workspace = new AdhocWorkspace(CodeAnalysis.Host.Mef.MefHostServices.DefaultHost);

        using var listener = new TestRazorWorkspaceListener();
        listener.EnsureInitialized(workspace, "temp.json");

        var project1 = workspace.AddProject("TestProject1", LanguageNames.CSharp);

        var project2 = workspace.AddProject("TestProject2", LanguageNames.CSharp);
        listener.NotifyDynamicFile(project2.Id);

        await listener.WaitForDebounceAsync();

        // These are different projects, so should cause two calls
        Assert.False(listener.SerializeCalls.ContainsKey(project1.Id));
        Assert.Equal(1, listener.SerializeCalls[project2.Id]);
    }

    [Fact]
    public async Task TwoProjectsAdded_SchedulesTwoTasks()
    {
        using var workspace = new AdhocWorkspace(CodeAnalysis.Host.Mef.MefHostServices.DefaultHost);

        using var listener = new TestRazorWorkspaceListener();
        listener.EnsureInitialized(workspace, "temp.json");

        var project1 = workspace.AddProject("TestProject1", LanguageNames.CSharp);
        listener.NotifyDynamicFile(project1.Id);

        await listener.WaitForDebounceAsync();

        var project2 = workspace.AddProject("TestProject2", LanguageNames.CSharp);
        listener.NotifyDynamicFile(project2.Id);

        await listener.WaitForDebounceAsync();

        // These are different projects, so should cause two calls
        Assert.Equal(1, listener.SerializeCalls[project1.Id]);
        Assert.Equal(1, listener.SerializeCalls[project2.Id]);
    }

    [Fact]
    public async Task ProjectAddedAndRemoved_NoTasks()
    {
        using var workspace = new AdhocWorkspace(CodeAnalysis.Host.Mef.MefHostServices.DefaultHost);

        using var listener = new TestRazorWorkspaceListener();
        listener.EnsureInitialized(workspace, "temp.json");

        var project = workspace.AddProject("TestProject", LanguageNames.CSharp);
        listener.NotifyDynamicFile(project.Id);

        var newSolution = project.Solution.RemoveProject(project.Id);
        Assert.True(workspace.TryApplyChanges(newSolution));

        // We can't wait for debounce here, because it won't happen, but if we don't wait for _something_ we won't know
        // if the test fails, so a delay is annoyingly necessary.
        await Task.Delay(500);

        Assert.Empty(listener.SerializeCalls);
    }

    [Fact]
    public async Task TwoProjectChanges_SchedulesOneTask()
    {
        using var workspace = new AdhocWorkspace(CodeAnalysis.Host.Mef.MefHostServices.DefaultHost);

        using var listener = new TestRazorWorkspaceListener();
        listener.EnsureInitialized(workspace, "temp.json");

        var project = workspace.AddProject("TestProject", LanguageNames.CSharp);
        listener.NotifyDynamicFile(project.Id);

        var newSolution = project.Solution.WithProjectDefaultNamespace(project.Id, "NewDefaultNamespace");
        Assert.True(workspace.TryApplyChanges(newSolution));

        await listener.WaitForDebounceAsync();

        Assert.Equal(1, listener.SerializeCalls[project.Id]);
    }

    [Fact]
    public async Task DocumentAdded_NoDynamicFiles_NoTasks()
    {
        using var workspace = new AdhocWorkspace(CodeAnalysis.Host.Mef.MefHostServices.DefaultHost);

        using var listener = new TestRazorWorkspaceListener();
        listener.EnsureInitialized(workspace, "temp.json");

        var project = workspace.AddProject("TestProject", LanguageNames.CSharp);

        workspace.AddDocument(project.Id, "Document", SourceText.From("Hi there"));

        // We can't wait for debounce here, because it won't happen, but if we don't wait for _something_ we won't know
        // if the test fails, so a delay is annoyingly necessary.
        await Task.Delay(500);

        Assert.Empty(listener.SerializeCalls);
    }

    [Fact]
    public async Task DocumentAdded_SchedulesTask()
    {
        using var workspace = new AdhocWorkspace(CodeAnalysis.Host.Mef.MefHostServices.DefaultHost);

        using var listener = new TestRazorWorkspaceListener();
        listener.EnsureInitialized(workspace, "temp.json");

        var project = workspace.AddProject("TestProject", LanguageNames.CSharp);
        listener.NotifyDynamicFile(project.Id);

        workspace.AddDocument(project.Id, "Document", SourceText.From("Hi there"));

        await listener.WaitForDebounceAsync();

        Assert.Equal(1, listener.SerializeCalls[project.Id]);
    }

    [Fact]
    public async Task DocumentAdded_WithDelay_SchedulesTwoTasks()
    {
        using var workspace = new AdhocWorkspace(CodeAnalysis.Host.Mef.MefHostServices.DefaultHost);

        using var listener = new TestRazorWorkspaceListener();
        listener.EnsureInitialized(workspace, "temp.json");

        var project = workspace.AddProject("TestProject", LanguageNames.CSharp);
        listener.NotifyDynamicFile(project.Id);

        await listener.WaitForDebounceAsync();

        workspace.AddDocument(project.Id, "Document", SourceText.From("Hi there"));

        await listener.WaitForDebounceAsync();

        Assert.Equal(2, listener.SerializeCalls[project.Id]);
    }

    [Fact]
    public async Task ProjectAddedAndRemoved_DeferredInitialization_NoTasks()
    {
        using var workspace = new AdhocWorkspace(CodeAnalysis.Host.Mef.MefHostServices.DefaultHost);

        using var listener = new TestRazorWorkspaceListener();

        var project = workspace.AddProject("TestProject", LanguageNames.CSharp);

        var newSolution = project.Solution.RemoveProject(project.Id);
        Assert.True(workspace.TryApplyChanges(newSolution));

        // Initialize everything now, in a deferred manner
        listener.EnsureInitialized(workspace, "temp.json");
        listener.NotifyDynamicFile(project.Id);

        // We can't wait for debounce here, because it won't happen, but if we don't wait for _something_ we won't know
        // if the test fails, so a delay is annoyingly necessary.
        await Task.Delay(500);

        Assert.Empty(listener.SerializeCalls);
    }

    private class TestRazorWorkspaceListener : RazorWorkspaceListener
    {
        private ConcurrentDictionary<ProjectId, int> _serializeCalls = new();
        private TaskCompletionSource _completionSource = new();

        public ConcurrentDictionary<ProjectId, int> SerializeCalls => _serializeCalls;

        public TestRazorWorkspaceListener()
            : base(NullLoggerFactory.Instance)
        {
        }

        protected override Task SerializeProjectAsync(ProjectId projectId, CancellationToken ct)
        {
            _serializeCalls.AddOrUpdate(projectId, 1, (id, curr) => curr + 1);

            _completionSource.TrySetResult();
            _completionSource = new();

            return Task.CompletedTask;
        }

        internal async Task WaitForDebounceAsync()
        {
            await _completionSource.Task;
            _completionSource = new();
        }
    }
}
