// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class OpenDocumentGeneratorTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    private readonly HostDocument[] _documents =
    [
        new HostDocument("c:/Test1/Index.cshtml", "Index.cshtml"),
        new HostDocument("c:/Test1/Components/Counter.cshtml", "Components/Counter.cshtml"),
    ];

    private readonly HostProject _hostProject1 = new("c:/Test1/Test1.csproj", "c:/Test1/obj", RazorConfiguration.Default, "TestRootNamespace");
    private readonly HostProject _hostProject2 = new("c:/Test2/Test2.csproj", "c:/Test2/obj", RazorConfiguration.Default, "TestRootNamespace");

    [Fact]
    public async Task AddDocument_ProcessesOpenDocument()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager(new LspProjectEngineFactoryProvider(TestRazorLSPOptionsMonitor.Create()));
        var listener = new TestDocumentProcessedListener();
        using var generator = CreateOpenDocumentGenerator(projectManager, listener);

        await projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(_hostProject1);
            updater.AddProject(_hostProject2);
            updater.AddDocument(_hostProject1.Key, _documents[0], EmptyTextLoader.Instance);
            updater.OpenDocument(_hostProject1.Key, _documents[0].FilePath, SourceText.From(string.Empty));
        });

        await listener.GetProcessedDocumentAsync(cancelAfter: TimeSpan.FromSeconds(10));
        listener.Reset();

        await projectManager.UpdateAsync(updater =>
        {
            updater.RemoveDocument(_hostProject1.Key, _documents[0].FilePath);
            updater.AddDocument(_hostProject2.Key, _documents[0], EmptyTextLoader.Instance);
        });

        // Assert
        var document = await listener.GetProcessedDocumentAsync(cancelAfter: TimeSpan.FromSeconds(10));
        Assert.Equal(_hostProject2.Key, document.Project.Key);
        Assert.Equal(_documents[0].FilePath, document.FilePath);
    }

    [Fact]
    public async Task AddDocument_IgnoresClosedDocument()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var listener = new TestDocumentProcessedListener();
        using var generator = CreateOpenDocumentGenerator(projectManager, listener);

        await projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(_hostProject1);
            updater.AddProject(_hostProject2);

            // Act
            updater.AddDocument(_hostProject1.Key, _documents[0], EmptyTextLoader.Instance);
        });

        // Assert
        await Assert.ThrowsAsync<TaskCanceledException>(() => listener.GetProcessedDocumentAsync(cancelAfter: TimeSpan.FromMilliseconds(50)));
    }

    [Fact]
    public async Task UpdateDocumentText_IgnoresClosedDocument()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var listener = new TestDocumentProcessedListener();
        using var generator = CreateOpenDocumentGenerator(projectManager, listener);

        await projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(_hostProject1);
            updater.AddProject(_hostProject2);
            updater.AddDocument(_hostProject1.Key, _documents[0], EmptyTextLoader.Instance);

            // Act
            updater.UpdateDocumentText(_hostProject1.Key, _documents[0].FilePath, SourceText.From("new"));
        });

        // Assert
        await Assert.ThrowsAsync<TaskCanceledException>(() => listener.GetProcessedDocumentAsync(cancelAfter: TimeSpan.FromMilliseconds(50)));
    }

    [Fact]
    public async Task UpdateDocumentText_ProcessesOpenDocument()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var listener = new TestDocumentProcessedListener();
        using var generator = CreateOpenDocumentGenerator(projectManager, listener);

        await projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(_hostProject1);
            updater.AddProject(_hostProject2);
            updater.AddDocument(_hostProject1.Key, _documents[0], EmptyTextLoader.Instance);
            updater.OpenDocument(_hostProject1.Key, _documents[0].FilePath, SourceText.From(string.Empty));

            // Act
            updater.UpdateDocumentText(_hostProject1.Key, _documents[0].FilePath, SourceText.From("new"));
        });

        // Assert

        var document = await listener.GetProcessedDocumentAsync(cancelAfter: TimeSpan.FromSeconds(10));
        Assert.Equal(document.FilePath, _documents[0].FilePath);
    }

    [Fact]
    public async Task UpdateProjectWorkspaceState_IgnoresClosedDocument()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var listener = new TestDocumentProcessedListener();
        using var generator = CreateOpenDocumentGenerator(projectManager, listener);

        await projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(_hostProject1);
            updater.AddProject(_hostProject2);
            updater.AddDocument(_hostProject1.Key, _documents[0], EmptyTextLoader.Instance);

            // Act
            updater.UpdateProjectWorkspaceState(_hostProject1.Key, ProjectWorkspaceState.Default);
        });

        // Assert
        await Assert.ThrowsAsync<TaskCanceledException>(() => listener.GetProcessedDocumentAsync(cancelAfter: TimeSpan.FromMilliseconds(50)));
    }

    [Fact]
    public async Task UpdateProjectWorkspaceState_ProcessesOpenDocument()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var listener = new TestDocumentProcessedListener();
        using var generator = CreateOpenDocumentGenerator(projectManager, listener);

        await projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(_hostProject1);
            updater.AddProject(_hostProject2);
            updater.AddDocument(_hostProject1.Key, _documents[0], EmptyTextLoader.Instance);
            updater.OpenDocument(_hostProject1.Key, _documents[0].FilePath, SourceText.From(string.Empty));

            // Act
            updater.UpdateProjectWorkspaceState(_hostProject1.Key, ProjectWorkspaceState.Default);
        });

        // Assert
        var document = await listener.GetProcessedDocumentAsync(cancelAfter: TimeSpan.FromSeconds(10));
        Assert.Equal(document.FilePath, _documents[0].FilePath);
    }

    private OpenDocumentGenerator CreateOpenDocumentGenerator(
        ProjectSnapshotManager projectManager,
        params IDocumentProcessedListener[] listeners)
    {
        return new OpenDocumentGenerator(listeners, projectManager, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
    }

    private class TestDocumentProcessedListener : IDocumentProcessedListener
    {
        private TaskCompletionSource<IDocumentSnapshot> _tcs;

        public TestDocumentProcessedListener()
        {
            _tcs = new TaskCompletionSource<IDocumentSnapshot>();
        }

        public Task<IDocumentSnapshot> GetProcessedDocumentAsync(TimeSpan cancelAfter)
        {
            var cts = new CancellationTokenSource(cancelAfter);
            var registration = cts.Token.Register(() => _tcs.SetCanceled());
            _ = _tcs.Task.ContinueWith(
                (t) =>
                {
                    registration.Dispose();
                    cts.Dispose();
                },
                TaskScheduler.Current);

            return _tcs.Task;
        }

        public void DocumentProcessed(RazorCodeDocument codeDocument, DocumentSnapshot document)
        {
            _tcs.SetResult(document);
        }

        internal void Reset()
        {
            _tcs = new TaskCompletionSource<IDocumentSnapshot>();
        }
    }
}
