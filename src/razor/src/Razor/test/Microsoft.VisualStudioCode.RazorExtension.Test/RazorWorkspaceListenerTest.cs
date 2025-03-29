// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.IO;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudioCode.RazorExtension.Test;

public class RazorWorkspaceListenerTest(ITestOutputHelper testOutputHelper) : ToolingTestBase(testOutputHelper)
{
    [Fact]
    public async Task ProjectAdded_SchedulesTask()
    {
        using var workspace = new AdhocWorkspace(CodeAnalysis.Host.Mef.MefHostServices.DefaultHost);

        using var listener = new TestRazorWorkspaceListener();
        listener.EnsureInitialized(workspace);

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
        listener.EnsureInitialized(workspace);

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
        listener.EnsureInitialized(workspace);

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
        listener.EnsureInitialized(workspace);

        // IntermediateOutput information is needed for project removal to be communicated properly
        var projectInfo = ProjectInfo.Create(
            ProjectId.CreateNewId(),
            VersionStamp.Create(),
            "TestProject",
            "TestProject",
            LanguageNames.CSharp);

        projectInfo = projectInfo.WithCompilationOutputInfo(projectInfo.CompilationOutputInfo.WithAssemblyPath(@"C:\test\out\test.dll"));
        projectInfo = projectInfo.WithFilePath(@"C:\test\test.csproj");

        var project = workspace.AddProject(projectInfo);
        listener.NotifyDynamicFile(project.Id);
        await listener.WaitForDebounceAsync();
        Assert.Single(listener.SerializeCalls);

        var newSolution = project.Solution.RemoveProject(project.Id);
        Assert.True(workspace.TryApplyChanges(newSolution));

        await listener.WaitForDebounceAsync();
        Assert.Single(listener.RemoveCalls);
    }

    [Fact]
    public async Task TwoProjectChanges_SchedulesOneTask()
    {
        using var workspace = new AdhocWorkspace(CodeAnalysis.Host.Mef.MefHostServices.DefaultHost);

        using var listener = new TestRazorWorkspaceListener();
        listener.EnsureInitialized(workspace);

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
        listener.EnsureInitialized(workspace);

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
        listener.EnsureInitialized(workspace);

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
        listener.EnsureInitialized(workspace);

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
        listener.EnsureInitialized(workspace);
        listener.NotifyDynamicFile(project.Id);

        // We can't wait for debounce here, because it won't happen, but if we don't wait for _something_ we won't know
        // if the test fails, so a delay is annoyingly necessary.
        await Task.Delay(500);

        Assert.Empty(listener.SerializeCalls);
    }

    [Fact]
    public async Task TestSerialization()
    {
        using var workspace = new AdhocWorkspace(CodeAnalysis.Host.Mef.MefHostServices.DefaultHost);

        var pipe = new Pipe();
        using var readerStream = pipe.Reader.AsStream();
        using var listener = new StreamBasedListener(workspace, pipe.Writer.AsStream());

        var projectInfo = ProjectInfo.Create(
            ProjectId.CreateNewId(),
            VersionStamp.Create(),
            "TestProject",
            "TestProject",
            LanguageNames.CSharp);

        projectInfo = projectInfo.WithCompilationOutputInfo(projectInfo.CompilationOutputInfo.WithAssemblyPath(@"C:\test\out\test.dll"));
        projectInfo = projectInfo.WithFilePath(@"C:\test\test.csproj");
        projectInfo = projectInfo.WithAdditionalDocuments([DocumentInfo.Create(DocumentId.CreateNewId(projectInfo.Id), @"Page.razor", filePath: @"C:\test\Page.razor")]);

        var intermediateDirectory = Path.GetDirectoryName(projectInfo.CompilationOutputInfo.AssemblyPath);

        // Test update action
        var project = workspace.AddProject(projectInfo);
        listener.NotifyDynamicFile(project.Id);

        var action = readerStream.ReadProjectInfoAction();
        Assert.Equal(ProjectInfoAction.Update, action);

        var deserializedProjectInfo = await readerStream.ReadProjectInfoAsync(DisposalToken);
        Assert.NotNull(deserializedProjectInfo);
        Assert.Single(deserializedProjectInfo.Documents);
        Assert.Equal("TestProject", deserializedProjectInfo.DisplayName);
        Assert.Equal("ASP", deserializedProjectInfo.RootNamespace);
        Assert.Equal(@"C:/test/out/", deserializedProjectInfo.ProjectKey.Id);
        Assert.Equal(@"C:\test\test.csproj", deserializedProjectInfo.FilePath);

        // Test remove action
        var newSolution = project.Solution.RemoveProject(project.Id);
        Assert.True(workspace.TryApplyChanges(newSolution));

        action = readerStream.ReadProjectInfoAction();
        Assert.Equal(ProjectInfoAction.Remove, action);

        Assert.Equal(intermediateDirectory, await readerStream.ReadProjectInfoRemovalAsync(DisposalToken));
    }

    private class TestRazorWorkspaceListener : RazorWorkspaceListenerBase
    {
        private ConcurrentDictionary<ProjectId, int> _serializeCalls = new();
        private ConcurrentDictionary<ProjectId, int> _removeCalls = new();

        private TaskCompletionSource _completionSource = new();

        public ConcurrentDictionary<ProjectId, int> SerializeCalls => _serializeCalls;
        public ConcurrentDictionary<ProjectId, int> RemoveCalls => _removeCalls;

        public TestRazorWorkspaceListener()
            : base(NullLoggerFactory.Instance.CreateLogger(""))
        {
        }

        public void EnsureInitialized(Workspace workspace)
        {
            EnsureInitialized(workspace, static () => Stream.Null);
        }

        private protected override ValueTask ProcessWorkAsync(ImmutableArray<Work> work, CancellationToken cancellationToken)
        {
            foreach (var unit in work)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (unit is UpdateWork updateWork)
                {
                    UpdateProject(updateWork.ProjectId);
                }

                if (unit is RemovalWork removeWork)
                {
                    RemoveProject(removeWork.ProjectId);
                }
            }

            return ValueTask.CompletedTask;
        }

        private void RemoveProject(ProjectId projectId)
        {
            _removeCalls.AddOrUpdate(projectId, 1, (id, curr) => curr + 1);

            _completionSource.TrySetResult();
            _completionSource = new();
        }

        private void UpdateProject(ProjectId projectId)
        {
            _serializeCalls.AddOrUpdate(projectId, 1, (id, curr) => curr + 1);

            _completionSource.TrySetResult();
            _completionSource = new();
        }

        internal async Task WaitForDebounceAsync()
        {
            await _completionSource.Task.WaitAsync(TimeSpan.FromSeconds(20));
            _completionSource = new();
        }

        private protected override Task CheckConnectionAsync(Stream stream, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class StreamBasedListener : RazorWorkspaceListenerBase
    {
        public StreamBasedListener(Workspace workspace, Stream stream)
            : base(NullLoggerFactory.Instance.CreateLogger(""))
        {
            EnsureInitialized(workspace, () => stream);
        }

        private protected override Task CheckConnectionAsync(Stream stream, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
