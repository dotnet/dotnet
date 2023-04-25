using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
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

        // Wait for debounce
        await Task.Delay(500);

        Assert.Equal(1, listener.SerializeCalls[project.Id]);
    }

    [Fact]
    public async Task TwoProjectsAdded_SchedulesTwoTasks()
    {
        using var workspace = new AdhocWorkspace(CodeAnalysis.Host.Mef.MefHostServices.DefaultHost);

        using var listener = new TestRazorWorkspaceListener();
        listener.EnsureInitialized(workspace, "temp.json");

        var project1 = workspace.AddProject("TestProject1", LanguageNames.CSharp);
        var project2 = workspace.AddProject("TestProject2", LanguageNames.CSharp);

        // Wait for debounce
        await Task.Delay(500);

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
        var newSolution = project.Solution.RemoveProject(project.Id);
        Assert.True(workspace.TryApplyChanges(newSolution));

        // Wait for debounce
        await Task.Delay(500);

        Assert.Empty(listener.SerializeCalls);
    }

    [Fact]
    public async Task TwoProjectChanges_SchedulesOneTasks()
    {
        using var workspace = new AdhocWorkspace(CodeAnalysis.Host.Mef.MefHostServices.DefaultHost);

        using var listener = new TestRazorWorkspaceListener();
        listener.EnsureInitialized(workspace, "temp.json");

        var project = workspace.AddProject("TestProject", LanguageNames.CSharp);
        var newSolution = project.Solution.WithProjectDefaultNamespace(project.Id, "NewDefaultNamespace");
        Assert.True(workspace.TryApplyChanges(newSolution));

        // Wait for debounce
        await Task.Delay(500);

        Assert.Equal(1, listener.SerializeCalls[project.Id]);
    }

    [Fact]
    public async Task DocumentAdded_SchedulesTask()
    {
        using var workspace = new AdhocWorkspace(CodeAnalysis.Host.Mef.MefHostServices.DefaultHost);

        using var listener = new TestRazorWorkspaceListener();
        listener.EnsureInitialized(workspace, "temp.json");

        var project = workspace.AddProject("TestProject", LanguageNames.CSharp);

        workspace.AddDocument(project.Id, "Document", SourceText.From("Hi there"));

        // Wait for debounce
        await Task.Delay(500);

        Assert.Equal(1, listener.SerializeCalls[project.Id]);
    }

    [Fact]
    public async Task DocumentAdded_WithDelay_SchedulesTwoTasks()
    {
        using var workspace = new AdhocWorkspace(CodeAnalysis.Host.Mef.MefHostServices.DefaultHost);

        using var listener = new TestRazorWorkspaceListener();
        listener.EnsureInitialized(workspace, "temp.json");

        var project = workspace.AddProject("TestProject", LanguageNames.CSharp);

        // Wait for debounce
        await Task.Delay(500);

        workspace.AddDocument(project.Id, "Document", SourceText.From("Hi there"));

        // Wait for debounce
        await Task.Delay(500);

        Assert.Equal(2, listener.SerializeCalls[project.Id]);
    }

    private class TestRazorWorkspaceListener : RazorWorkspaceListener
    {
        private ConcurrentDictionary<ProjectId, int> _serializeCalls = new();

        public ConcurrentDictionary<ProjectId, int> SerializeCalls => _serializeCalls;

        protected override Task SerializeProjectAsync(ProjectId projectId, CancellationToken ct)
        {
            _serializeCalls.AddOrUpdate(projectId, 1, (id, curr) => curr + 1);

            return Task.CompletedTask;
        }
    }
}
