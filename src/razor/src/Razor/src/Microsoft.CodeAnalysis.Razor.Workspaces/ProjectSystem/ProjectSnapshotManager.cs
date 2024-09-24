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
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

// The implementation of project snapshot manager abstracts the host's underlying project system (HostProject),
// to provide a immutable view of the underlying project systems.
//
// The HostProject support all of the configuration that the Razor SDK exposes via the project system
// (language version, extensions, named configuration).
//
// The implementation will create a ProjectSnapshot for each HostProject.
internal partial class ProjectSnapshotManager : IProjectSnapshotManager, IDisposable
{
    private static readonly LoadTextOptions s_loadTextOptions = new(SourceHashAlgorithm.Sha256);

    private readonly IProjectEngineFactoryProvider _projectEngineFactoryProvider;
    private readonly Dispatcher _dispatcher;
    private readonly bool _initialized;

    public event EventHandler<ProjectChangeEventArgs>? PriorityChanged;
    public event EventHandler<ProjectChangeEventArgs>? Changed;

    private readonly ReaderWriterLockSlim _readerWriterLock = new(LockRecursionPolicy.NoRecursion);

    #region protected by lock

    /// <summary>
    /// A map of <see cref="ProjectKey"/> to <see cref="Entry"/>, which wraps a <see cref="ProjectState"/>
    /// and lazily creates a <see cref="ProjectSnapshot"/>.
    /// </summary>
    private readonly Dictionary<ProjectKey, Entry> _projectMap = [];

    /// <summary>
    /// The set of open documents.
    /// </summary>
    private readonly HashSet<string> _openDocumentSet = new(FilePathComparer.Instance);

    /// <summary>
    /// Determines whether or not the solution is closing.
    /// </summary>
    private bool _isSolutionClosing;

    #endregion

    // We have a queue for changes because if one change results in another change aka, add -> open
    // we want to make sure the "add" finishes running first before "open" is notified.
    private readonly Queue<ProjectChangeEventArgs> _notificationQueue = new();

    /// <summary>
    /// Constructs an instance of <see cref="ProjectSnapshotManager"/>.
    /// </summary>
    /// <param name="projectEngineFactoryProvider">The <see cref="IProjectEngineFactoryProvider"/> to
    /// use when creating <see cref="ProjectState"/>.</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> to use.</param>
    /// <param name="initializer">An optional callback to set up the initial set of projects and documents.
    /// Note that this is called during construction, so it does not run on the dispatcher and notifications
    /// will not be sent.</param>
    public ProjectSnapshotManager(
        IProjectEngineFactoryProvider projectEngineFactoryProvider,
        ILoggerFactory loggerFactory,
        Action<Updater>? initializer = null)
    {
        _projectEngineFactoryProvider = projectEngineFactoryProvider;
        _dispatcher = new(loggerFactory);

        initializer?.Invoke(new(this));

        _initialized = true;
    }

    public void Dispose()
    {
        _dispatcher.Dispose();
        _readerWriterLock.Dispose();
    }

    public bool IsSolutionClosing
    {
        get
        {
            using (_readerWriterLock.DisposableRead())
            {
                return _isSolutionClosing;
            }
        }
    }

    public ImmutableArray<IProjectSnapshot> GetProjects()
    {
        using (_readerWriterLock.DisposableRead())
        {
            using var builder = new PooledArrayBuilder<IProjectSnapshot>(_projectMap.Count);

            foreach (var (_, entry) in _projectMap)
            {
                builder.Add(entry.GetSnapshot());
            }

            return builder.DrainToImmutable();
        }
    }

    public ImmutableArray<string> GetOpenDocuments()
    {
        using (_readerWriterLock.DisposableRead())
        {
            return [.. _openDocumentSet];
        }
    }

    public IProjectSnapshot GetLoadedProject(ProjectKey projectKey)
    {
        using (_readerWriterLock.DisposableRead())
        {
            if (_projectMap.TryGetValue(projectKey, out var entry))
            {
                return entry.GetSnapshot();
            }
        }

        throw new InvalidOperationException($"No project snapshot exists with the key, '{projectKey}'");
    }

    public bool TryGetLoadedProject(ProjectKey projectKey, [NotNullWhen(true)] out IProjectSnapshot? project)
    {
        using (_readerWriterLock.DisposableRead())
        {
            if (_projectMap.TryGetValue(projectKey, out var entry))
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
        using (_readerWriterLock.DisposableRead())
        {
            using var projects = new PooledArrayBuilder<ProjectKey>(capacity: _projectMap.Count);

            foreach (var (key, entry) in _projectMap)
            {
                if (FilePathComparer.Instance.Equals(entry.State.HostProject.FilePath, projectFileName))
                {
                    projects.Add(key);
                }
            }

            return projects.DrainToImmutable();
        }
    }

    public bool IsDocumentOpen(string documentFilePath)
    {
        using (_readerWriterLock.DisposableRead())
        {
            return _openDocumentSet.Contains(documentFilePath);
        }
    }

    private void DocumentAdded(ProjectKey projectKey, HostDocument document, TextLoader textLoader)
    {
        if (_initialized)
        {
            _dispatcher.AssertRunningOnDispatcher();
        }

        if (TryUpdate(
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
        if (_initialized)
        {
            _dispatcher.AssertRunningOnDispatcher();
        }

        if (TryUpdate(
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
        if (_initialized)
        {
            _dispatcher.AssertRunningOnDispatcher();
        }

        if (TryUpdate(
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
        if (_initialized)
        {
            _dispatcher.AssertRunningOnDispatcher();
        }

        if (TryUpdate(
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
        if (_initialized)
        {
            _dispatcher.AssertRunningOnDispatcher();
        }

        if (TryUpdate(
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
        if (_initialized)
        {
            _dispatcher.AssertRunningOnDispatcher();
        }

        if (TryUpdate(
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
        if (_initialized)
        {
            _dispatcher.AssertRunningOnDispatcher();
        }

        if (TryUpdate(
            hostProject.Key,
            documentFilePath: null,
            new ProjectAddedAction(hostProject),
            out _,
            out var newSnapshot))
        {
            NotifyListeners(older: null, newSnapshot, documentFilePath: null, ProjectChangeKind.ProjectAdded);
        }
    }

    private void ProjectConfigurationChanged(HostProject hostProject)
    {
        if (_initialized)
        {
            _dispatcher.AssertRunningOnDispatcher();
        }

        if (TryUpdate(
            hostProject.Key,
            documentFilePath: null,
            new HostProjectUpdatedAction(hostProject),
            out var oldSnapshot,
            out var newSnapshot))
        {
            NotifyListeners(oldSnapshot, newSnapshot, documentFilePath: null, ProjectChangeKind.ProjectChanged);
        }
    }

    private void ProjectWorkspaceStateChanged(ProjectKey projectKey, ProjectWorkspaceState projectWorkspaceState)
    {
        if (_initialized)
        {
            _dispatcher.AssertRunningOnDispatcher();
        }

        if (TryUpdate(
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
        if (_initialized)
        {
            _dispatcher.AssertRunningOnDispatcher();
        }

        if (TryUpdate(
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
        if (_initialized)
        {
            _dispatcher.AssertRunningOnDispatcher();
        }

        using (_readerWriterLock.DisposableWrite())
        {
            _isSolutionClosing = false;
        }
    }

    private void SolutionClosed()
    {
        if (_initialized)
        {
            _dispatcher.AssertRunningOnDispatcher();
        }

        using (_readerWriterLock.DisposableWrite())
        {
            _isSolutionClosing = true;
        }
    }

    private void NotifyListeners(IProjectSnapshot? older, IProjectSnapshot? newer, string? documentFilePath, ProjectChangeKind kind)
    {
        if (!_initialized)
        {
            return;
        }

        _notificationQueue.Enqueue(new ProjectChangeEventArgs(older, newer, documentFilePath, kind, IsSolutionClosing));

        if (_notificationQueue.Count == 1)
        {
            // Only one notification, go ahead and start notifying. In the situation where Count > 1
            // it means an event was triggered as a response to another event. To ensure order we won't
            // immediately re-invoke Changed here, we'll wait for the stack to unwind to notify others.
            // This process still happens synchronously it just ensures that events happen in the correct
            // order. For instance, let's take the situation where a document is added to a project.
            // That document will be added and then opened. However, if the result of "adding" causes an
            // "open" to trigger we want to ensure that "add" finishes prior to "open" being notified.

            // Start unwinding the notification queue
            do
            {
                // Don't dequeue yet, we want the notification to sit in the queue until we've finished
                // notifying to ensure other calls to NotifyListeners know there's a currently running event loop.
                var args = _notificationQueue.Peek();
                PriorityChanged?.Invoke(this, args);
                Changed?.Invoke(this, args);

                _notificationQueue.Dequeue();
            }
            while (_notificationQueue.Count > 0);
        }
    }

    private bool TryUpdate(
        ProjectKey projectKey,
        string? documentFilePath,
        IUpdateProjectAction action,
        [NotNullWhen(true)] out IProjectSnapshot? oldSnapshot,
        [NotNullWhen(true)] out IProjectSnapshot? newSnapshot)
    {
        using var upgradeableLock = _readerWriterLock.DisposableUpgradeableRead();

        if (action is ProjectAddedAction(var hostProject))
        {
            // If the project already exists, we can't add it again, so return false.
            if (_projectMap.ContainsKey(hostProject.Key))
            {
                oldSnapshot = newSnapshot = null;
                return false;
            }

            // ... otherwise, add the project and return true.

            var state = ProjectState.Create(
                _projectEngineFactoryProvider,
                hostProject,
                ProjectWorkspaceState.Default);

            var newEntry = new Entry(state);

            upgradeableLock.EnterWrite();
            _projectMap[hostProject.Key] = newEntry;

            oldSnapshot = newSnapshot = newEntry.GetSnapshot();
            return true;
        }

        if (_projectMap.TryGetValue(projectKey, out var entry))
        {
            // if the solution is closing we don't need to bother computing new state
            if (_isSolutionClosing)
            {
                oldSnapshot = newSnapshot = entry.GetSnapshot();
                return true;
            }

            // If we're removing a project, we don't need to try and compute new state for it.
            // We can just remove it.
            if (action is ProjectRemovedAction)
            {
                upgradeableLock.EnterWrite();

                _projectMap.Remove(projectKey);

                oldSnapshot = newSnapshot = entry.GetSnapshot();
                return true;
            }

            // ... otherwise, compute a new entry and update if it's changed from the old state.
            var documentState = documentFilePath is not null
                ? entry.State.Documents.GetValueOrDefault(documentFilePath)
                : null;

            var newEntry = ComputeNewEntry(entry, action, documentState);

            if (!ReferenceEquals(newEntry.State, entry.State))
            {
                upgradeableLock.EnterWrite();

                _projectMap[projectKey] = newEntry;

                switch (action)
                {
                    case OpenDocumentAction:
                        _openDocumentSet.Add(documentFilePath.AssumeNotNull());
                        break;
                    case CloseDocumentAction:
                        _openDocumentSet.Remove(documentFilePath.AssumeNotNull());
                        break;
                }

                oldSnapshot = entry.GetSnapshot();
                newSnapshot = newEntry.GetSnapshot();

                return true;
            }
        }

        oldSnapshot = newSnapshot = null;
        return false;
    }

    private static Entry ComputeNewEntry(Entry originalEntry, IUpdateProjectAction action, DocumentState? documentState)
    {
        switch (action)
        {
            case AddDocumentAction(var newDocument, var textLoader):
                return new Entry(originalEntry.State.WithAddedHostDocument(newDocument, CreateTextAndVersionFunc(textLoader)));

            case RemoveDocumentAction(var originalDocument):
                return new Entry(originalEntry.State.WithRemovedHostDocument(originalDocument));

            case CloseDocumentAction(var textLoader):
                {
                    // If the document being closed has already been removed from the project then we no-op
                    if (documentState is null)
                    {
                        return originalEntry;
                    }

                    var state = originalEntry.State.WithChangedHostDocument(
                        documentState.HostDocument,
                        () => textLoader.LoadTextAndVersionAsync(s_loadTextOptions, cancellationToken: default));
                    return new Entry(state);
                }

            case OpenDocumentAction(var sourceText):
                {
                    documentState.AssumeNotNull();

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
                    // If the document being changed has already been removed from the project then we no-op
                    if (documentState is null)
                    {
                        return originalEntry;
                    }

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

            case ProjectWorkspaceStateChangedAction(var workspaceState):
                return new Entry(originalEntry.State.WithProjectWorkspaceState(workspaceState));

            case HostProjectUpdatedAction(var hostProject):
                return new Entry(originalEntry.State.WithHostProject(hostProject));

            default:
                throw new InvalidOperationException($"Unexpected action type {action.GetType()}");
        }

        static Func<Task<TextAndVersion>> CreateTextAndVersionFunc(TextLoader textLoader)
        {
            return textLoader is null
                ? DocumentState.EmptyLoader
                : (() => textLoader.LoadTextAndVersionAsync(s_loadTextOptions, CancellationToken.None));
        }
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
}
