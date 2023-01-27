// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

// The implementation of project snapshot manager abstracts the host's underlying project system (HostProject),
// to provide a immutable view of the underlying project systems.
//
// The HostProject support all of the configuration that the Razor SDK exposes via the project system
// (language version, extensions, named configuration).
//
// The implementation will create a ProjectSnapshot for each HostProject.
internal class DefaultProjectSnapshotManager : ProjectSnapshotManagerBase
{
    public override event EventHandler<ProjectChangeEventArgs> Changed;

    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private readonly ProjectSnapshotChangeTrigger[] _triggers;

    // Each entry holds a ProjectState and an optional ProjectSnapshot. ProjectSnapshots are
    // created lazily.
    private readonly Dictionary<string, Entry> _projects;
    private readonly HashSet<string> _openDocuments;
    private readonly LoadTextOptions LoadTextOptions = new LoadTextOptions(SourceHashAlgorithm.Sha256);

    // We have a queue for changes because if one change results in another change aka, add -> open we want to make sure the "add" finishes running first before "open" is notified.
    private readonly Queue<ProjectChangeEventArgs> _notificationWork;

    public DefaultProjectSnapshotManager(
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        ErrorReporter errorReporter,
        IEnumerable<ProjectSnapshotChangeTrigger> triggers,
        Workspace workspace)
    {
        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        if (errorReporter is null)
        {
            throw new ArgumentNullException(nameof(errorReporter));
        }

        if (triggers is null)
        {
            throw new ArgumentNullException(nameof(triggers));
        }

        if (workspace is null)
        {
            throw new ArgumentNullException(nameof(workspace));
        }

        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _triggers = triggers.OrderByDescending(trigger => trigger.InitializePriority).ToArray();
        Workspace = workspace;
        ErrorReporter = errorReporter;

        _projects = new Dictionary<string, Entry>(FilePathComparer.Instance);
        _openDocuments = new HashSet<string>(FilePathComparer.Instance);
        _notificationWork = new Queue<ProjectChangeEventArgs>();

        // All methods involving the project snapshot manager need to be run on the
        // project snapshot manager's specialized thread. The LSP editor should already
        // be on the specialized thread, however the old editor may be calling this
        // constructor on the UI thread.
        if (_projectSnapshotManagerDispatcher.IsDispatcherThread)
        {
            InitializeTriggers(this, _triggers);
        }
        else
        {
            _ = _projectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync(
                () => InitializeTriggers(this, _triggers), CancellationToken.None);
        }

        static void InitializeTriggers(
            DefaultProjectSnapshotManager snapshotManager,
            ProjectSnapshotChangeTrigger[] triggers)
        {
            for (var i = 0; i < triggers.Length; i++)
            {
                triggers[i].Initialize(snapshotManager);
            }
        }
    }

    // internal for testing
    internal bool IsSolutionClosing { get; private set; }

    public override IReadOnlyList<ProjectSnapshot> Projects
    {
        get
        {
            _projectSnapshotManagerDispatcher.AssertDispatcherThread();

            var i = 0;
            var projects = new ProjectSnapshot[_projects.Count];
            foreach (var entry in _projects)
            {
                projects[i++] = entry.Value.GetSnapshot();
            }

            return projects;
        }
    }

    public override IReadOnlyCollection<string> OpenDocuments
    {
        get
        {
            _projectSnapshotManagerDispatcher.AssertDispatcherThread();

            return _openDocuments;
        }
    }

    public override Workspace Workspace { get; }

    public override ErrorReporter ErrorReporter { get; }

    public override ProjectSnapshot GetLoadedProject(string filePath)
    {
        if (filePath is null)
        {
            throw new ArgumentNullException(nameof(filePath));
        }

        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        if (_projects.TryGetValue(filePath, out var entry))
        {
            return entry.GetSnapshot();
        }

        return null;
    }

    public override ProjectSnapshot GetOrCreateProject(string filePath)
    {
        if (filePath is null)
        {
            throw new ArgumentNullException(nameof(filePath));
        }

        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        return GetLoadedProject(filePath) ?? new EphemeralProjectSnapshot(Workspace.Services, filePath);
    }

    public override bool IsDocumentOpen(string documentFilePath)
    {
        if (documentFilePath is null)
        {
            throw new ArgumentNullException(nameof(documentFilePath));
        }

        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        return _openDocuments.Contains(documentFilePath);
    }

    public override void DocumentAdded(HostProject hostProject, HostDocument document, TextLoader textLoader)
    {
        if (hostProject is null)
        {
            throw new ArgumentNullException(nameof(hostProject));
        }

        if (document is null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        if (_projects.TryGetValue(hostProject.FilePath, out var entry))
        {
            // if the solution is closing we don't need to bother computing new state
            if (IsSolutionClosing)
            {
                var oldSnapshot = entry.GetSnapshot();
                NotifyListeners(oldSnapshot, oldSnapshot, document.FilePath, ProjectChangeKind.DocumentAdded);
            }
            else
            {
                var loader = textLoader is null
                    ? DocumentState.EmptyLoader
                    : (() => textLoader.LoadTextAndVersionAsync(LoadTextOptions, CancellationToken.None));
                var state = entry.State.WithAddedHostDocument(document, loader);

                // Document updates can no-op.
                if (!object.ReferenceEquals(state, entry.State))
                {
                    var oldSnapshot = entry.GetSnapshot();
                    entry = new Entry(state);
                    _projects[hostProject.FilePath] = entry;
                    NotifyListeners(oldSnapshot, entry.GetSnapshot(), document.FilePath, ProjectChangeKind.DocumentAdded);
                }
            }
        }
    }

    public override void DocumentRemoved(HostProject hostProject, HostDocument document)
    {
        if (hostProject is null)
        {
            throw new ArgumentNullException(nameof(hostProject));
        }

        if (document is null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        if (_projects.TryGetValue(hostProject.FilePath, out var entry))
        {
            // if the solution is closing we don't need to bother computing new state
            if (IsSolutionClosing)
            {
                var snapshot = entry.GetSnapshot();
                NotifyListeners(snapshot, snapshot, document.FilePath, ProjectChangeKind.DocumentRemoved);
            }
            else
            {
                var state = entry.State.WithRemovedHostDocument(document);

                // Document updates can no-op.
                if (!object.ReferenceEquals(state, entry.State))
                {
                    var oldSnapshot = entry.GetSnapshot();
                    entry = new Entry(state);
                    _projects[hostProject.FilePath] = entry;
                    NotifyListeners(oldSnapshot, entry.GetSnapshot(), document.FilePath, ProjectChangeKind.DocumentRemoved);
                }
            }
        }
    }

    public override void DocumentOpened(string projectFilePath, string documentFilePath, SourceText sourceText)
    {
        if (projectFilePath is null)
        {
            throw new ArgumentNullException(nameof(projectFilePath));
        }

        if (documentFilePath is null)
        {
            throw new ArgumentNullException(nameof(documentFilePath));
        }

        if (sourceText is null)
        {
            throw new ArgumentNullException(nameof(sourceText));
        }

        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        if (_projects.TryGetValue(projectFilePath, out var entry) &&
            entry.State.Documents.TryGetValue(documentFilePath, out var older))
        {
            // if the solution is closing we don't need to bother computing new state
            if (IsSolutionClosing)
            {
                var oldSnapshot = entry.GetSnapshot();
                NotifyListeners(oldSnapshot, oldSnapshot, documentFilePath, ProjectChangeKind.DocumentChanged);
            }
            else
            {
                ProjectState state;

                var currentText = sourceText;
                if (older.TryGetText(out var olderText) &&
                    older.TryGetTextVersion(out var olderVersion))
                {
                    var version = currentText.ContentEquals(olderText) ? olderVersion : olderVersion.GetNewerVersion();
                    state = entry.State.WithChangedHostDocument(older.HostDocument, currentText, version);
                }
                else
                {
                    state = entry.State.WithChangedHostDocument(older.HostDocument, async () =>
                    {
                        olderText = await older.GetTextAsync().ConfigureAwait(false);
                        olderVersion = await older.GetTextVersionAsync().ConfigureAwait(false);

                        var version = currentText.ContentEquals(olderText) ? olderVersion : olderVersion.GetNewerVersion();
                        return TextAndVersion.Create(currentText, version, documentFilePath);
                    });
                }

                _openDocuments.Add(documentFilePath);

                // Document updates can no-op.
                if (!object.ReferenceEquals(state, entry.State))
                {
                    var oldSnapshot = entry.GetSnapshot();
                    entry = new Entry(state);
                    _projects[projectFilePath] = entry;
                    NotifyListeners(oldSnapshot, entry.GetSnapshot(), documentFilePath, ProjectChangeKind.DocumentChanged);
                }
            }
        }
    }

    public override void DocumentClosed(string projectFilePath, string documentFilePath, TextLoader textLoader)
    {
        if (projectFilePath is null)
        {
            throw new ArgumentNullException(nameof(projectFilePath));
        }

        if (documentFilePath is null)
        {
            throw new ArgumentNullException(nameof(documentFilePath));
        }

        if (textLoader is null)
        {
            throw new ArgumentNullException(nameof(textLoader));
        }

        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        if (_projects.TryGetValue(projectFilePath, out var entry) &&
            entry.State.Documents.TryGetValue(documentFilePath, out var older))
        {
            // if the solution is closing we don't need to bother computing new state
            if (IsSolutionClosing)
            {
                var oldSnapshot = entry.GetSnapshot();
                NotifyListeners(oldSnapshot, oldSnapshot, documentFilePath, ProjectChangeKind.DocumentChanged);
            }
            else
            {
                var state = entry.State.WithChangedHostDocument(
                    older.HostDocument,
                    async () => await textLoader.LoadTextAndVersionAsync(LoadTextOptions, cancellationToken: default));

                _openDocuments.Remove(documentFilePath);

                // Document updates can no-op.
                if (!object.ReferenceEquals(state, entry.State))
                {
                    var oldSnapshot = entry.GetSnapshot();
                    entry = new Entry(state);
                    _projects[projectFilePath] = entry;
                    NotifyListeners(oldSnapshot, entry.GetSnapshot(), documentFilePath, ProjectChangeKind.DocumentChanged);
                }
            }
        }
    }

    public override void DocumentChanged(string projectFilePath, string documentFilePath, SourceText sourceText)
    {
        if (projectFilePath is null)
        {
            throw new ArgumentNullException(nameof(projectFilePath));
        }

        if (documentFilePath is null)
        {
            throw new ArgumentNullException(nameof(documentFilePath));
        }

        if (sourceText is null)
        {
            throw new ArgumentNullException(nameof(sourceText));
        }

        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        if (_projects.TryGetValue(projectFilePath, out var entry) &&
            entry.State.Documents.TryGetValue(documentFilePath, out var older))
        {
            ProjectState state;

            var currentText = sourceText;

            // if the solution is closing we don't need to bother computing new state
            if (IsSolutionClosing)
            {
                var oldSnapshot = entry.GetSnapshot();
                NotifyListeners(oldSnapshot, oldSnapshot, documentFilePath, ProjectChangeKind.DocumentChanged);
            }
            else
            {
                if (older.TryGetText(out var olderText) &&
                older.TryGetTextVersion(out var olderVersion))
                {
                    var version = currentText.ContentEquals(olderText) ? olderVersion : olderVersion.GetNewerVersion();
                    state = entry.State.WithChangedHostDocument(older.HostDocument, currentText, version);
                }
                else
                {
                    state = entry.State.WithChangedHostDocument(older.HostDocument, async () =>
                    {
                        olderText = await older.GetTextAsync().ConfigureAwait(false);
                        olderVersion = await older.GetTextVersionAsync().ConfigureAwait(false);

                        var version = currentText.ContentEquals(olderText) ? olderVersion : olderVersion.GetNewerVersion();
                        return TextAndVersion.Create(currentText, version, documentFilePath);
                    });
                }

                // Document updates can no-op.
                if (!object.ReferenceEquals(state, entry.State))
                {
                    var oldSnapshot = entry.GetSnapshot();
                    entry = new Entry(state);
                    _projects[projectFilePath] = entry;
                    NotifyListeners(oldSnapshot, entry.GetSnapshot(), documentFilePath, ProjectChangeKind.DocumentChanged);
                }
            }
        }
    }

    public override void DocumentChanged(string projectFilePath, string documentFilePath, TextLoader textLoader)
    {
        if (projectFilePath is null)
        {
            throw new ArgumentNullException(nameof(projectFilePath));
        }

        if (documentFilePath is null)
        {
            throw new ArgumentNullException(nameof(documentFilePath));
        }

        if (textLoader is null)
        {
            throw new ArgumentNullException(nameof(textLoader));
        }

        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        if (_projects.TryGetValue(projectFilePath, out var entry) &&
            entry.State.Documents.TryGetValue(documentFilePath, out var older))
        {
            // if the solution is closing we don't need to bother computing new state
            if (IsSolutionClosing)
            {
                var oldSnapshot = entry.GetSnapshot();
                NotifyListeners(oldSnapshot, oldSnapshot, documentFilePath, ProjectChangeKind.DocumentChanged);
            }
            else
            {
                var state = entry.State.WithChangedHostDocument(
                    older.HostDocument,
                    async () => await textLoader.LoadTextAndVersionAsync(LoadTextOptions, cancellationToken: default));

                // Document updates can no-op.
                if (!ReferenceEquals(state, entry.State))
                {
                    var oldSnapshot = entry.GetSnapshot();
                    entry = new Entry(state);
                    _projects[projectFilePath] = entry;
                    NotifyListeners(oldSnapshot, entry.GetSnapshot(), documentFilePath, ProjectChangeKind.DocumentChanged);
                }
            }
        }
    }

    public override void ProjectAdded(HostProject hostProject)
    {
        if (hostProject is null)
        {
            throw new ArgumentNullException(nameof(hostProject));
        }

        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        // We don't expect to see a HostProject initialized multiple times for the same path. Just ignore it.
        if (_projects.ContainsKey(hostProject.FilePath))
        {
            return;
        }

        var state = ProjectState.Create(Workspace.Services, hostProject);
        var entry = new Entry(state);
        _projects[hostProject.FilePath] = entry;

        // We need to notify listeners about every project add.
        NotifyListeners(older: null, entry.GetSnapshot(), documentFilePath: null, ProjectChangeKind.ProjectAdded);
    }

    public override void ProjectConfigurationChanged(HostProject hostProject)
    {
        if (hostProject is null)
        {
            throw new ArgumentNullException(nameof(hostProject));
        }

        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        if (_projects.TryGetValue(hostProject.FilePath, out var entry))
        {
            // if the solution is closing we don't need to bother computing new state
            if (IsSolutionClosing)
            {
                var oldSnapshot = entry.GetSnapshot();
                NotifyListeners(oldSnapshot, oldSnapshot, documentFilePath: null, ProjectChangeKind.ProjectChanged);
            }
            else
            {
                var state = entry.State.WithHostProject(hostProject);

                // HostProject updates can no-op.
                if (!object.ReferenceEquals(state, entry.State))
                {
                    var oldSnapshot = entry.GetSnapshot();
                    entry = new Entry(state);
                    _projects[hostProject.FilePath] = entry;
                    NotifyListeners(oldSnapshot, entry.GetSnapshot(), documentFilePath: null, ProjectChangeKind.ProjectChanged);
                }
            }
        }
    }

    public override void ProjectWorkspaceStateChanged(string projectFilePath, ProjectWorkspaceState projectWorkspaceState)
    {
        if (projectFilePath is null)
        {
            throw new ArgumentNullException(nameof(projectFilePath));
        }

        if (projectWorkspaceState is null)
        {
            throw new ArgumentNullException(nameof(projectWorkspaceState));
        }

        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        if (_projects.TryGetValue(projectFilePath, out var entry))
        {
            // if the solution is closing we don't need to bother computing new state
            if (IsSolutionClosing)
            {
                var oldSnapshot = entry.GetSnapshot();
                NotifyListeners(oldSnapshot, oldSnapshot, documentFilePath: null, ProjectChangeKind.ProjectChanged);
            }
            else
            {
                var state = entry.State.WithProjectWorkspaceState(projectWorkspaceState);

                // HostProject updates can no-op.
                if (!object.ReferenceEquals(state, entry.State))
                {
                    var oldSnapshot = entry.GetSnapshot();
                    entry = new Entry(state);
                    _projects[projectFilePath] = entry;
                    NotifyListeners(oldSnapshot, entry.GetSnapshot(), documentFilePath: null, ProjectChangeKind.ProjectChanged);
                }
            }
        }
    }

    public override void ProjectRemoved(HostProject hostProject)
    {
        if (hostProject is null)
        {
            throw new ArgumentNullException(nameof(hostProject));
        }

        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        if (_projects.TryGetValue(hostProject.FilePath, out var entry))
        {
            // We need to notify listeners about every project removal.
            var oldSnapshot = entry.GetSnapshot();
            _projects.Remove(hostProject.FilePath);
            NotifyListeners(oldSnapshot, newer: null, documentFilePath: null, ProjectChangeKind.ProjectRemoved);
        }
    }

    public override void SolutionOpened()
    {
        IsSolutionClosing = false;
    }

    public override void SolutionClosed()
    {
        IsSolutionClosing = true;
    }

    public override void ReportError(Exception exception)
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        ErrorReporter.ReportError(exception);
    }

    public override void ReportError(Exception exception, ProjectSnapshot project)
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        ErrorReporter.ReportError(exception, project);
    }

    public override void ReportError(Exception exception, HostProject hostProject)
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        var snapshot = hostProject?.FilePath is null ? null : GetLoadedProject(hostProject.FilePath);
        ErrorReporter.ReportError(exception, snapshot);
    }

    private void NotifyListeners(ProjectSnapshot older, ProjectSnapshot newer, string documentFilePath, ProjectChangeKind kind)
    {
        NotifyListeners(new ProjectChangeEventArgs(older, newer, documentFilePath, kind, IsSolutionClosing));
    }

    // virtual so it can be overridden in tests
    protected virtual void NotifyListeners(ProjectChangeEventArgs e)
    {
        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        _notificationWork.Enqueue(e);

        if (_notificationWork.Count == 1)
        {
            // Only one notification, go ahead and start notifying. In the situation where Count > 1 it means an event was triggered as a response to another event.
            // To ensure order we wont immediately re-invoke Changed here, we'll wait for the stack to unwind to notify others. This process still happens synchronously
            // it just ensures that events happen in the correct order. For instance lets take the situation where a document is added to a project. That document will be
            // added and then opened. However, if the result of "adding" causes an "open" to triger we want to ensure that "add" finishes prior to "open" being notified.

            // Start unwinding the notification queue
            do
            {
                // Don't dequeue yet, we want the notification to sit in the queue until we've finished notifying to ensure other calls to NotifyListeners know there's a currently running event loop.
                var args = _notificationWork.Peek();
                Changed?.Invoke(this, args);

                _notificationWork.Dequeue();
            }
            while (_notificationWork.Count > 0);
        }
    }

    private class Entry
    {
        private ProjectSnapshot _snapshotUnsafe;
        public readonly ProjectState State;

        public Entry(ProjectState state)
        {
            State = state;
        }

        public ProjectSnapshot GetSnapshot()
        {
            return _snapshotUnsafe ??= new DefaultProjectSnapshot(State);
        }
    }
}
