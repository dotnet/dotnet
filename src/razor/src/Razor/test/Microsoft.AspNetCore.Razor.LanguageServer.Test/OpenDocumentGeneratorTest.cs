// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class OpenDocumentGeneratorTest : LanguageServerTestBase
{
    private readonly HostDocument[] _documents;
    private readonly HostProject _hostProject1;
    private readonly HostProject _hostProject2;

    public OpenDocumentGeneratorTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _documents =
        [
            new HostDocument("c:/Test1/Index.cshtml", "Index.cshtml"),
            new HostDocument("c:/Test1/Components/Counter.cshtml", "Components/Counter.cshtml"),
        ];

        _hostProject1 = new HostProject("c:/Test1/Test1.csproj", "c:/Test1/obj", RazorConfiguration.Default, "TestRootNamespace");
        _hostProject2 = new HostProject("c:/Test2/Test2.csproj", "c:/Test2/obj", RazorConfiguration.Default, "TestRootNamespace");
    }

    [Fact]
    public async Task DocumentAdded_IgnoresClosedDocument()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var listener = new TestDocumentProcessedListener();
        var queue = new TestOpenDocumentGenerator(projectManager, Dispatcher, ErrorReporter, listener);

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(_hostProject1);
            updater.ProjectAdded(_hostProject2);

            // Act
            updater.DocumentAdded(_hostProject1.Key, _documents[0], null);
        });

        // Assert
        await Assert.ThrowsAsync<TaskCanceledException>(() => listener.GetProcessedDocumentAsync(cancelAfter: TimeSpan.FromMilliseconds(50)));
    }

    [Fact]
    public async Task DocumentChanged_IgnoresClosedDocument()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var listener = new TestDocumentProcessedListener();
        var queue = new TestOpenDocumentGenerator(projectManager, Dispatcher, ErrorReporter, listener);

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(_hostProject1);
            updater.ProjectAdded(_hostProject2);
            updater.DocumentAdded(_hostProject1.Key, _documents[0], null);

            // Act
            updater.DocumentChanged(_hostProject1.Key, _documents[0].FilePath, SourceText.From("new"));
        });

        // Assert
        await Assert.ThrowsAsync<TaskCanceledException>(() => listener.GetProcessedDocumentAsync(cancelAfter: TimeSpan.FromMilliseconds(50)));
    }

    [Fact]
    public async Task DocumentChanged_ProcessesOpenDocument()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var listener = new TestDocumentProcessedListener();
        var queue = new TestOpenDocumentGenerator(projectManager, Dispatcher, ErrorReporter, listener);

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(_hostProject1);
            updater.ProjectAdded(_hostProject2);
            updater.DocumentAdded(_hostProject1.Key, _documents[0], null);
            updater.DocumentOpened(_hostProject1.Key, _documents[0].FilePath, SourceText.From(string.Empty));

            // Act
            updater.DocumentChanged(_hostProject1.Key, _documents[0].FilePath, SourceText.From("new"));
        });

        // Assert

        var document = await listener.GetProcessedDocumentAsync(cancelAfter: TimeSpan.FromSeconds(10));
        Assert.Equal(document.FilePath, _documents[0].FilePath);
    }

    [Fact]
    public async Task ProjectChanged_IgnoresClosedDocument()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var listener = new TestDocumentProcessedListener();
        var queue = new TestOpenDocumentGenerator(projectManager, Dispatcher, ErrorReporter, listener);

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(_hostProject1);
            updater.ProjectAdded(_hostProject2);
            updater.DocumentAdded(_hostProject1.Key, _documents[0], null);

            // Act
            updater.ProjectWorkspaceStateChanged(_hostProject1.Key,
                ProjectWorkspaceState.Create(LanguageVersion.CSharp8));
        });

        // Assert
        await Assert.ThrowsAsync<TaskCanceledException>(() => listener.GetProcessedDocumentAsync(cancelAfter: TimeSpan.FromMilliseconds(50)));
    }

    [Fact]
    public async Task ProjectChanged_ProcessesOpenDocument()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var listener = new TestDocumentProcessedListener();
        var queue = new TestOpenDocumentGenerator(projectManager, Dispatcher, ErrorReporter, listener);

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(_hostProject1);
            updater.ProjectAdded(_hostProject2);
            updater.DocumentAdded(_hostProject1.Key, _documents[0], null);
            updater.DocumentOpened(_hostProject1.Key, _documents[0].FilePath, SourceText.From(string.Empty));

            // Act
            updater.ProjectWorkspaceStateChanged(_hostProject1.Key,
                ProjectWorkspaceState.Create(LanguageVersion.CSharp8));
        });

        // Assert

        var document = await listener.GetProcessedDocumentAsync(cancelAfter: TimeSpan.FromSeconds(10));
        Assert.Equal(document.FilePath, _documents[0].FilePath);
    }

    private class TestOpenDocumentGenerator(
        IProjectSnapshotManager projectManager,
        ProjectSnapshotManagerDispatcher dispatcher,
        IErrorReporter errorReporter,
        params DocumentProcessedListener[] listeners)
        : OpenDocumentGenerator(listeners, projectManager, dispatcher, TestLanguageServerFeatureOptions.Instance, errorReporter);

    private class TestDocumentProcessedListener : DocumentProcessedListener
    {
        private readonly TaskCompletionSource<IDocumentSnapshot> _tcs;

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

        public override void DocumentProcessed(RazorCodeDocument codeDocument, IDocumentSnapshot document)
        {
            _tcs.SetResult(document);
        }

        public override void Initialize(IProjectSnapshotManager projectManager)
        {
        }
    }
}
