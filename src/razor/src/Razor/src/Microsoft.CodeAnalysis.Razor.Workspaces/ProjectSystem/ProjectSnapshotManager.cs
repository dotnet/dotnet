// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

// The implementation of project snapshot manager abstracts the host's underlying project system (HostProject),
// to provide a immutable view of the underlying project systems.
//
// The HostProject support all of the configuration that the Razor SDK exposes via the project system
// (language version, extensions, named configuration).
//
// The implementation will create a ProjectSnapshot for each HostProject.
internal partial class ProjectSnapshotManager(
    IProjectEngineFactoryProvider projectEngineFactoryProvider,
    ProjectSnapshotManagerDispatcher dispatcher)
    : IProjectSnapshotManager
{
    public event EventHandler<ProjectChangeEventArgs>? PriorityChanged;
    public event EventHandler<ProjectChangeEventArgs>? Changed;

    // Each entry holds a ProjectState and an optional ProjectSnapshot. ProjectSnapshots are
    // created lazily.
    private readonly ReadWriterLocker _rwLocker = new();
    private readonly Dictionary<ProjectKey, Entry> _projects_needsLock = [];
    private readonly HashSet<string> _openDocuments_needsLock = new(FilePathComparer.Instance);
    private static readonly LoadTextOptions s_loadTextOptions = new(SourceHashAlgorithm.Sha256);

    // We have a queue for changes because if one change results in another change aka, add -> open
    // we want to make sure the "add" finishes running first before "open" is notified.
    private readonly Queue<ProjectChangeEventArgs> _notificationWork = new();
    private readonly IProjectEngineFactoryProvider _projectEngineFactoryProvider = projectEngineFactoryProvider;
    private readonly ProjectSnapshotManagerDispatcher _dispatcher = dispatcher;

    // internal for testing
    internal bool IsSolutionClosing { get; private set; }

    public ImmutableArray<IProjectSnapshot> GetProjects()
    {
        using var _ = _rwLocker.EnterReadLock();
        using var builder = new PooledArrayBuilder<IProjectSnapshot>(_projects_needsLock.Count);

        foreach (var (_, entry) in _projects_needsLock)
        {
            builder.Add(entry.GetSnapshot());
        }

        return builder.DrainToImmutable();
    }

    public ImmutableArray<string> GetOpenDocuments()
    {
        using var _ = _rwLocker.EnterReadLock();
        return _openDocuments_needsLock.ToImmutableArray();
    }

    public IProjectSnapshot GetLoadedProject(ProjectKey projectKey)
    {
        using (_rwLocker.EnterReadLock())
        {
            if (_projects_needsLock.TryGetValue(projectKey, out var entry))
            {
                return entry.GetSnapshot();
            }
        }

        throw new InvalidOperationException($"No project snapshot exists with the key, '{projectKey}'");
    }

    public bool TryGetLoadedProject(ProjectKey projectKey, [NotNullWhen(true)] out IProjectSnapshot? project)
    {
        using (_rwLocker.EnterReadLock())
        {
            if (_projects_needsLock.TryGetValue(projectKey, out var entry))
            {
                project = entry.GetSnapshot();
                return true;
            }
        }

        project = null;
        return false;
    }

    public ImmutableArray<ProjectKey> GetAllProjectKeys(string projectFileName)
    {
        if (projectFileName is null)
        {
            throw new ArgumentNullException(nameof(projectFileName));
        }

        using var _ = _rwLocker.EnterReadLock();
        using var projects = new PooledArrayBuilder<ProjectKey>(capacity: _projects_needsLock.Count);

        foreach (var (key, entry) in _projects_needsLock)
        {
            if (FilePathComparer.Instance.Equals(entry.State.HostProject.FilePath, projectFileName))
            {
                projects.Add(key);
            }
        }

        return projects.DrainToImmutable();
    }

    public bool IsDocumentOpen(string documentFilePath)
    {
        if (documentFilePath is null)
        {
            throw new ArgumentNullException(nameof(documentFilePath));
        }

        using var _ = _rwLocker.EnterReadLock();
        return _openDocuments_needsLock.Contains(documentFilePath);
    }

    private void DocumentAdded(ProjectKey projectKey, HostDocument document, TextLoader textLoader)
    {
        if (document is null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        if (TryChangeEntry_UsesLock(
            projectKey,
            document.FilePath,
            new AddDocumentAction(document, textLoader),
            out var oldSnapshot,
            out var newSnapshot))
        {
            NotifyListeners(oldSnapshot, newSnapshot, document.FilePath, ProjectChangeKind.DocumentAdded);
        }
    }

    private void DocumentRemoved(ProjectKey projectKey, HostDocument document)
    {
        if (document is null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        if (TryChangeEntry_UsesLock(
            projectKey,
            document.FilePath,
            new RemoveDocumentAction(document),
            out var oldSnapshot,
            out var newSnapshot))
        {
            NotifyListeners(oldSnapshot, newSnapshot, document.FilePath, ProjectChangeKind.DocumentRemoved);
        }
    }

    private void DocumentOpened(ProjectKey projectKey, string documentFilePath, SourceText sourceText)
    {
        if (documentFilePath is null)
        {
            throw new ArgumentNullException(nameof(documentFilePath));
        }

        if (sourceText is null)
        {
            throw new ArgumentNullException(nameof(sourceText));
        }

        if (TryChangeEntry_UsesLock(
            projectKey,
            documentFilePath,
            new OpenDocumentAction(sourceText),
            out var oldSnapshot,
            out var newSnapshot))
        {
            NotifyListeners(oldSnapshot, newSnapshot, documentFilePath, ProjectChangeKind.DocumentChanged);
        }
    }

    private void DocumentClosed(ProjectKey projectKey, string documentFilePath, TextLoader textLoader)
    {
        if (documentFilePath is null)
        {
            throw new ArgumentNullException(nameof(documentFilePath));
        }

        if (textLoader is null)
        {
            throw new ArgumentNullException(nameof(textLoader));
        }

        if (TryChangeEntry_UsesLock(
            projectKey,
            documentFilePath,
            new CloseDocumentAction(textLoader),
            out var oldSnapshot,
            out var newSnapshot))
        {
            NotifyListeners(oldSnapshot, newSnapshot, documentFilePath, ProjectChangeKind.DocumentChanged);
        }
    }

    private void DocumentChanged(ProjectKey projectKey, string documentFilePath, SourceText sourceText)
    {
        if (documentFilePath is null)
        {
            throw new ArgumentNullException(nameof(documentFilePath));
        }

        if (sourceText is null)
        {
            throw new ArgumentNullException(nameof(sourceText));
        }

        if (TryChangeEntry_UsesLock(
            projectKey,
            documentFilePath,
            new DocumentTextChangedAction(sourceText),
            out var oldSnapshot,
            out var newSnapshot))
        {
            NotifyListeners(oldSnapshot, newSnapshot, documentFilePath, ProjectChangeKind.DocumentChanged);
        }
    }

    private void DocumentChanged(ProjectKey projectKey, string documentFilePath, TextLoader textLoader)
    {
        if (documentFilePath is null)
        {
            throw new ArgumentNullException(nameof(documentFilePath));
        }

        if (textLoader is null)
        {
            throw new ArgumentNullException(nameof(textLoader));
        }

        if (TryChangeEntry_UsesLock(
            projectKey,
            documentFilePath,
            new DocumentTextLoaderChangedAction(textLoader),
            out var oldSnapshot,
            out var newSnapshot))
        {
            NotifyListeners(oldSnapshot, newSnapshot, documentFilePath, ProjectChangeKind.DocumentChanged);
        }
    }

    private void ProjectAdded(HostProject hostProject)
    {
        if (hostProject is null)
        {
            throw new ArgumentNullException(nameof(hostProject));
        }

        if (TryChangeEntry_UsesLock(
            hostProject.Key,
            documentFilePath: null,
            new ProjectAddedAction(hostProject),
            out var oldSnapshot,
            out var newSnapshot))
        {
            NotifyListeners(older: null, newSnapshot, documentFilePath: null, ProjectChangeKind.ProjectAdded);
        }
    }

    private void ProjectConfigurationChanged(HostProject hostProject)
    {
        if (hostProject is null)
        {
            throw new ArgumentNullException(nameof(hostProject));
        }

        if (TryChangeEntry_UsesLock(
            hostProject.Key,
            documentFilePath: null,
            new HostProjectUpdatedAction(hostProject),
            out var oldSnapshot,
            out var newSnapshot))
        {
            NotifyListeners(oldSnapshot, newSnapshot, documentFilePath: null, ProjectChangeKind.ProjectChanged);
        }
    }

    private void ProjectWorkspaceStateChanged(ProjectKey projectKey, ProjectWorkspaceState? projectWorkspaceState)
    {
        if (projectWorkspaceState is null)
        {
            throw new ArgumentNullException(nameof(projectWorkspaceState));
        }

        if (TryChangeEntry_UsesLock(
            projectKey,
            documentFilePath: null,
            new ProjectWorkspaceStateChangedAction(projectWorkspaceState),
            out var oldSnapshot,
            out var newSnapshot))
        {
            NotifyListeners(oldSnapshot, newSnapshot, documentFilePath: null, ProjectChangeKind.ProjectChanged);
        }
    }

    private void ProjectRemoved(ProjectKey projectKey)
    {
        if (TryChangeEntry_UsesLock(
            projectKey,
            documentFilePath: null,
            new ProjectRemovedAction(projectKey),
            out var oldSnapshot,
            out var newSnapshot))
        {
            NotifyListeners(oldSnapshot, newSnapshot, documentFilePath: null, ProjectChangeKind.ProjectRemoved);
        }
    }

    private void SolutionOpened()
    {
        IsSolutionClosing = false;
    }

    private void SolutionClosed()
    {
        IsSolutionClosing = true;
    }

    private void NotifyListeners(IProjectSnapshot? older, IProjectSnapshot? newer, string? documentFilePath, ProjectChangeKind kind)
    {
        // Change notifications should always be sent on the dispatcher.
        _dispatcher.AssertRunningOnDispatcher();

        NotifyListenersCore(new ProjectChangeEventArgs(older, newer, documentFilePath, kind, IsSolutionClosing));
    }

    private void NotifyListenersCore(ProjectChangeEventArgs e)
    {
        _notificationWork.Enqueue(e);

        if (_notificationWork.Count == 1)
        {
            // Only one notification, go ahead and start notifying. In the situation where Count > 1
            // it means an event was triggered as a response to another event. To ensure order we won't
            // immediately re-invoke Changed here, we'll wait for the stack to unwind to notify others.
            // This process still happens synchronously it just ensures that events happen in the correct
            // order. For instance lets take the situation where a document is added to a project.
            // That document will be added and then opened. However, if the result of "adding" causes an
            // "open" to trigger we want to ensure that "add" finishes prior to "open" being notified.

            // Start unwinding the notification queue
            do
            {
                // Don't dequeue yet, we want the notification to sit in the queue until we've finished
                // notifying to ensure other calls to NotifyListeners know there's a currently running event loop.
                var args = _notificationWork.Peek();
                PriorityChanged?.Invoke(this, args);
                Changed?.Invoke(this, args);

                _notificationWork.Dequeue();
            }
            while (_notificationWork.Count > 0);
        }
    }

    private static Func<Task<TextAndVersion>> CreateTextAndVersionFunc(TextLoader textLoader)
        => textLoader is null
            ? DocumentState.EmptyLoader
            : (() => textLoader.LoadTextAndVersionAsync(s_loadTextOptions, CancellationToken.None));

    private bool TryChangeEntry_UsesLock(
        ProjectKey projectKey,
        string? documentFilePath,
        IUpdateProjectAction action,
        [NotNullWhen(true)] out IProjectSnapshot? oldSnapshot,
        [NotNullWhen(true)] out IProjectSnapshot? newSnapshot)
    {
        using var upgradeableLock = _rwLocker.EnterUpgradeableReadLock();

        if (action is ProjectAddedAction projectAddedAction)
        {
            if (_projects_needsLock.ContainsKey(projectAddedAction.HostProject.Key))
            {
                oldSnapshot = newSnapshot = null;
                return false;
            }

            // We're about to mutate, so assert that we're on the dispatcher thread.
            _dispatcher.AssertRunningOnDispatcher();

            var state = ProjectState.Create(
                _projectEngineFactoryProvider,
                projectAddedAction.HostProject,
                ProjectWorkspaceState.Default);
            var newEntry = new Entry(state);

            oldSnapshot = newSnapshot = newEntry.GetSnapshot();
            using (upgradeableLock.EnterWriteLock())
            {
                _projects_needsLock[projectAddedAction.HostProject.Key] = newEntry;
            }

            return true;
        }

        if (_projects_needsLock.TryGetValue(projectKey, out var entry))
        {
            // if the solution is closing we don't need to bother computing new state
            if (IsSolutionClosing)
            {
                oldSnapshot = newSnapshot = entry.GetSnapshot();
                return true;
            }
            else
            {
                DocumentState? documentState = null;
                if (documentFilePath is not null)
                {
                    _ = entry.State.Documents.TryGetValue(documentFilePath, out documentState);
                }

                var newEntry = Change(entry, action, documentState);
                if (!ReferenceEquals(newEntry?.State, entry.State))
                {
                    oldSnapshot = entry.GetSnapshot();
                    newSnapshot = newEntry?.GetSnapshot() ?? oldSnapshot;

                    // We're about to mutate, so assert that we're on the dispatcher thread.
                    _dispatcher.AssertRunningOnDispatcher();

                    using (upgradeableLock.EnterWriteLock())
                    {
                        if (newEntry is null)
                        {
                            _projects_needsLock.Remove(projectKey);
                        }
                        else
                        {
                            _projects_needsLock[projectKey] = newEntry;
                        }

                        switch (action)
                        {
                            case OpenDocumentAction:
                                _openDocuments_needsLock.Add(documentFilePath.AssumeNotNull());
                                break;
                            case CloseDocumentAction:
                                _openDocuments_needsLock.Remove(documentFilePath.AssumeNotNull());
                                break;
                        }
                    }

                    return true;
                }
            }
        }

        oldSnapshot = newSnapshot = null;
        return false;
    }

    private static Entry? Change(Entry originalEntry, IUpdateProjectAction action, DocumentState? documentState)
    {
        switch (action)
        {
            case AddDocumentAction(var newDocument, var textLoader):
                return new Entry(originalEntry.State.WithAddedHostDocument(newDocument, CreateTextAndVersionFunc(textLoader)));

            case RemoveDocumentAction(var originalDocument):
                return new Entry(originalEntry.State.WithRemovedHostDocument(originalDocument));

            case CloseDocumentAction(var textLoader):
                {
                    if (documentState is null)
                    {
                        throw new ArgumentNullException(nameof(documentState));
                    }

                    var state = originalEntry.State.WithChangedHostDocument(
                        documentState.HostDocument,
                        () => textLoader.LoadTextAndVersionAsync(s_loadTextOptions, cancellationToken: default));
                    return new Entry(state);
                }

            case OpenDocumentAction(var sourceText):
                {
                    if (documentState is null)
                    {
                        throw new ArgumentNullException(nameof(documentState));
                    }

                    if (documentState.TryGetText(out var olderText) &&
                        documentState.TryGetTextVersion(out var olderVersion))
                    {
                        var version = sourceText.ContentEquals(olderText) ? olderVersion : olderVersion.GetNewerVersion();
                        var newState = originalEntry.State.WithChangedHostDocument(documentState.HostDocument, sourceText, version);
                        return new Entry(newState);
                    }
                    else
                    {
                        var newState = originalEntry.State.WithChangedHostDocument(documentState.HostDocument, async () =>
                        {
                            olderText = await documentState.GetTextAsync().ConfigureAwait(false);
                            olderVersion = await documentState.GetTextVersionAsync().ConfigureAwait(false);

                            var version = sourceText.ContentEquals(olderText) ? olderVersion : olderVersion.GetNewerVersion();
                            return TextAndVersion.Create(sourceText, version, documentState.HostDocument.FilePath);
                        });

                        return new Entry(newState);
                    }
                }

            case DocumentTextLoaderChangedAction(var textLoader):
                {
                    var newState = originalEntry.State.WithChangedHostDocument(
                        documentState.AssumeNotNull().HostDocument,
                        CreateTextAndVersionFunc(textLoader));

                    return new Entry(newState);
                }

            case DocumentTextChangedAction(var sourceText):
                {
                    documentState.AssumeNotNull();
                    if (documentState.TryGetText(out var olderText) &&
                        documentState.TryGetTextVersion(out var olderVersion))
                    {
                        var version = sourceText.ContentEquals(olderText) ? olderVersion : olderVersion.GetNewerVersion();
                        var state = originalEntry.State.WithChangedHostDocument(documentState.HostDocument, sourceText, version);

                        return new Entry(state);
                    }
                    else
                    {
                        var state = originalEntry.State.WithChangedHostDocument(documentState.HostDocument, async () =>
                        {
                            olderText = await documentState.GetTextAsync().ConfigureAwait(false);
                            olderVersion = await documentState.GetTextVersionAsync().ConfigureAwait(false);

                            var version = sourceText.ContentEquals(olderText) ? olderVersion : olderVersion.GetNewerVersion();
                            return TextAndVersion.Create(sourceText, version, documentState.HostDocument.FilePath);
                        });

                        return new Entry(state);
                    }
                }

            case ProjectRemovedAction:
                return null;

            case ProjectWorkspaceStateChangedAction(var workspaceState):
                return new Entry(originalEntry.State.WithProjectWorkspaceState(workspaceState));

            case HostProjectUpdatedAction(var hostProject):
                return new Entry(originalEntry.State.WithHostProject(hostProject));

            default:
                throw new InvalidOperationException($"Unexpected action type {action.GetType()}");
        }
    }

    public void Update(Action<Updater> updater)
    {
        _dispatcher.AssertRunningOnDispatcher();
        updater(new(this));
    }

    public void Update<TState>(Action<Updater, TState> updater, TState state)
    {
        _dispatcher.AssertRunningOnDispatcher();
        updater(new(this), state);
    }

    public TResult Update<TResult>(Func<Updater, TResult> updater)
    {
        _dispatcher.AssertRunningOnDispatcher();
        return updater(new(this));
    }

    public TResult Update<TState, TResult>(Func<Updater, TState, TResult> updater, TState state)
    {
        _dispatcher.AssertRunningOnDispatcher();
        return updater(new(this), state);
    }

    public Task UpdateAsync(Action<Updater> updater, CancellationToken cancellationToken)
    {
        return _dispatcher.RunAsync(
            static x => x.updater(new(x.instance)),
            (updater, instance: this),
            cancellationToken);
    }

    public Task UpdateAsync<TState>(Action<Updater, TState> updater, TState state, CancellationToken cancellationToken)
    {
        return _dispatcher.RunAsync(
            static x => x.updater(new(x.instance), x.state),
            (updater, state, instance: this),
            cancellationToken);
    }

    public Task<TResult> UpdateAsync<TResult>(Func<Updater, TResult> updater, CancellationToken cancellationToken)
    {
        return _dispatcher.RunAsync(
            static x => x.updater(new(x.instance)),
            (updater, instance: this),
            cancellationToken);
    }

    public Task<TResult> UpdateAsync<TState, TResult>(Func<Updater, TState, TResult> updater, TState state, CancellationToken cancellationToken)
    {
        return _dispatcher.RunAsync(
            static x => x.updater(new(x.instance), x.state),
            (updater, state, instance: this),
            cancellationToken);
    }

    public Task UpdateAsync(Func<Updater, Task> updater, CancellationToken cancellationToken)
    {
        return _dispatcher.RunAsync(
            static x => x.updater(new(x.instance)),
            (updater, instance: this),
            cancellationToken).Unwrap();
    }

    public Task UpdateAsync<TState>(Func<Updater, TState, Task> updater, TState state, CancellationToken cancellationToken)
    {
        return _dispatcher.RunAsync(
            static x => x.updater(new(x.instance), x.state),
            (updater, state, instance: this),
            cancellationToken).Unwrap();
    }

    public Task<TResult> UpdateAsync<TResult>(Func<Updater, Task<TResult>> updater, CancellationToken cancellationToken)
    {
        return _dispatcher.RunAsync(
            static x => x.updater(new(x.instance)),
            (updater, instance: this),
            cancellationToken).Unwrap();
    }

    public Task<TResult> UpdateAsync<TState, TResult>(Func<Updater, TState, Task<TResult>> updater, TState state, CancellationToken cancellationToken)
    {
        return _dispatcher.RunAsync(
            static x => x.updater(new(x.instance), x.state),
            (updater, state, instance: this),
            cancellationToken).Unwrap();
    }

    private class Entry(ProjectState state)
    {
        public readonly ProjectState State = state;
        private IProjectSnapshot? _snapshotUnsafe;

        public IProjectSnapshot GetSnapshot()
        {
            return _snapshotUnsafe ??= new ProjectSnapshot(State);
        }
    }
}
