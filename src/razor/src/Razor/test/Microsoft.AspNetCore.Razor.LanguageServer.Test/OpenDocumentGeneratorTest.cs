// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
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
        _documents = new HostDocument[]
        {
            new HostDocument("c:/Test1/Index.cshtml", "Index.cshtml"),
            new HostDocument("c:/Test1/Components/Counter.cshtml", "Components/Counter.cshtml"),
        };

        _hostProject1 = new HostProject("c:/Test1/Test1.csproj", RazorConfiguration.Default, "TestRootNamespace");
        _hostProject2 = new HostProject("c:/Test2/Test2.csproj", RazorConfiguration.Default, "TestRootNamespace");
    }

    [Fact]
    public async Task DocumentAdded_IgnoresClosedDocument()
    {
        // Arrange
        var projectManager = TestProjectSnapshotManager.Create(Dispatcher);
        var listener = new TestDocumentProcessedListener();
        var queue = new TestOpenDocumentGenerator(Dispatcher, listener);

        await Dispatcher.RunOnDispatcherThreadAsync(() =>
        {
            projectManager.ProjectAdded(_hostProject1);
            projectManager.ProjectAdded(_hostProject2);
            projectManager.AllowNotifyListeners = true;

            queue.Initialize(projectManager);

            // Act
            projectManager.DocumentAdded(_hostProject1, _documents[0], null);
        }, DisposalToken);

        // Assert
        await Assert.ThrowsAsync<TaskCanceledException>(() => listener.GetProcessedDocumentAsync(cancelAfter: TimeSpan.FromMilliseconds(50)));
    }

    [Fact]
    public async Task DocumentChanged_IgnoresClosedDocument()
    {
        // Arrange
        var projectManager = TestProjectSnapshotManager.Create(Dispatcher);
        var listener = new TestDocumentProcessedListener();
        var queue = new TestOpenDocumentGenerator(Dispatcher, listener);

        await Dispatcher.RunOnDispatcherThreadAsync(() =>
        {
            projectManager.ProjectAdded(_hostProject1);
            projectManager.ProjectAdded(_hostProject2);
            projectManager.AllowNotifyListeners = true;
            projectManager.DocumentAdded(_hostProject1, _documents[0], null);

            queue.Initialize(projectManager);

            // Act
            projectManager.DocumentChanged(_hostProject1.FilePath, _documents[0].FilePath, SourceText.From("new"));
        }, DisposalToken);

        // Assert
        await Assert.ThrowsAsync<TaskCanceledException>(() => listener.GetProcessedDocumentAsync(cancelAfter: TimeSpan.FromMilliseconds(50)));
    }

    [Fact]
    public async Task DocumentChanged_ProcessesOpenDocument()
    {
        // Arrange
        var projectManager = TestProjectSnapshotManager.Create(Dispatcher);
        var listener = new TestDocumentProcessedListener();
        var queue = new TestOpenDocumentGenerator(Dispatcher, listener);

        await Dispatcher.RunOnDispatcherThreadAsync(() =>
        {
            projectManager.ProjectAdded(_hostProject1);
            projectManager.ProjectAdded(_hostProject2);
            projectManager.AllowNotifyListeners = true;
            projectManager.DocumentAdded(_hostProject1, _documents[0], null);
            projectManager.DocumentOpened(_hostProject1.FilePath, _documents[0].FilePath, SourceText.From(string.Empty));

            queue.Initialize(projectManager);

            // Act
            projectManager.DocumentChanged(_hostProject1.FilePath, _documents[0].FilePath, SourceText.From("new"));
        }, DisposalToken);

        // Assert

        var document = await listener.GetProcessedDocumentAsync(cancelAfter: TimeSpan.FromSeconds(10));
        Assert.Equal(document.FilePath, _documents[0].FilePath);
    }

    [Fact]
    public async Task ProjectChanged_IgnoresClosedDocument()
    {
        // Arrange
        var projectManager = TestProjectSnapshotManager.Create(Dispatcher);
        var listener = new TestDocumentProcessedListener();
        var queue = new TestOpenDocumentGenerator(Dispatcher, listener);

        await Dispatcher.RunOnDispatcherThreadAsync(() =>
        {
            projectManager.ProjectAdded(_hostProject1);
            projectManager.ProjectAdded(_hostProject2);
            projectManager.AllowNotifyListeners = true;
            projectManager.DocumentAdded(_hostProject1, _documents[0], null);

            queue.Initialize(projectManager);

            // Act
            projectManager.ProjectWorkspaceStateChanged(_hostProject1.FilePath, new ProjectWorkspaceState(Array.Empty<TagHelperDescriptor>(), LanguageVersion.CSharp8));
        }, DisposalToken);

        // Assert
        await Assert.ThrowsAsync<TaskCanceledException>(() => listener.GetProcessedDocumentAsync(cancelAfter: TimeSpan.FromMilliseconds(50)));
    }

    [Fact]
    public async Task ProjectChanged_ProcessesOpenDocument()
    {
        // Arrange
        var projectManager = TestProjectSnapshotManager.Create(Dispatcher);
        var listener = new TestDocumentProcessedListener();
        var queue = new TestOpenDocumentGenerator(Dispatcher, listener);

        await Dispatcher.RunOnDispatcherThreadAsync(() =>
        {
            projectManager.ProjectAdded(_hostProject1);
            projectManager.ProjectAdded(_hostProject2);
            projectManager.AllowNotifyListeners = true;
            projectManager.DocumentAdded(_hostProject1, _documents[0], null);
            projectManager.DocumentOpened(_hostProject1.FilePath, _documents[0].FilePath, SourceText.From(string.Empty));

            queue.Initialize(projectManager);

            // Act
            projectManager.ProjectWorkspaceStateChanged(_hostProject1.FilePath, new ProjectWorkspaceState(Array.Empty<TagHelperDescriptor>(), LanguageVersion.CSharp8));
        }, DisposalToken);

        // Assert

        var document = await listener.GetProcessedDocumentAsync(cancelAfter: TimeSpan.FromSeconds(10));
        Assert.Equal(document.FilePath, _documents[0].FilePath);
    }

    private class TestOpenDocumentGenerator : OpenDocumentGenerator
    {
        public TestOpenDocumentGenerator(
            ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
            params DocumentProcessedListener[] listeners)
            : base(listeners, projectSnapshotManagerDispatcher, new DefaultErrorReporter())
        {
        }
    }

    private class TestDocumentProcessedListener : DocumentProcessedListener
    {
        private readonly TaskCompletionSource<DocumentSnapshot> _tcs;

        public TestDocumentProcessedListener()
        {
            _tcs = new TaskCompletionSource<DocumentSnapshot>();
        }

        public Task<DocumentSnapshot> GetProcessedDocumentAsync(TimeSpan cancelAfter)
        {
            var cts = new CancellationTokenSource(cancelAfter);
            var registration = cts.Token.Register(() => _tcs.SetCanceled(cts.Token));
            _ = _tcs.Task.ContinueWith(
                (t) =>
                {
                    registration.Dispose();
                    cts.Dispose();
                },
                TaskScheduler.Current);

            return _tcs.Task;
        }

        public override void DocumentProcessed(RazorCodeDocument codeDocument, DocumentSnapshot document)
        {
            _tcs.SetResult(document);
        }

        public override void Initialize(ProjectSnapshotManager projectManager)
        {
        }
    }
}
