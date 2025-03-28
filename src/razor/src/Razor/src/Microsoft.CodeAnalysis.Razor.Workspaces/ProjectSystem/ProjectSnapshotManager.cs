// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.PooledObjects;
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
internal partial class ProjectSnapshotManager : IDisposable
{
    private readonly IProjectEngineFactoryProvider _projectEngineFactoryProvider;
    private readonly RazorCompilerOptions _compilerOptions;
    private readonly Dispatcher _dispatcher;
    private readonly ILogger _logger;
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

    #region protected by dispatcher

    /// <summary>
    ///  A queue of ordered notifications to process.
    /// </summary>
    /// <remarks>
    ///  ⚠️ This field must only be accessed when running on the dispatcher.
    /// </remarks>
    private readonly Queue<ProjectChangeEventArgs> _notificationQueue = new();

    /// <summary>
    ///  <see langword="true"/> while <see cref="_notificationQueue"/> is being processed.
    /// </summary>
    /// <remarks>
    ///  ⚠️ This field must only be accessed when running on the dispatcher.
    /// </remarks>
    private bool _processingNotifications;

    #endregion

    /// <summary>
    /// Constructs an instance of <see cref="ProjectSnapshotManager"/>.
    /// </summary>
    /// <param name="projectEngineFactoryProvider">The <see cref="IProjectEngineFactoryProvider"/> to
    /// use when creating <see cref="ProjectState"/>.</param>
    /// <param name="compilerOptions">Options used to control Razor compilation.</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> to use.</param>
    /// <param name="initializer">An optional callback to set up the initial set of projects and documents.
    /// Note that this is called during construction, so it does not run on the dispatcher and notifications
    /// will not be sent.</param>
    public ProjectSnapshotManager(
        IProjectEngineFactoryProvider projectEngineFactoryProvider,
        RazorCompilerOptions compilerOptions,
        ILoggerFactory loggerFactory,
        Action<Updater>? initializer = null)
    {
        _projectEngineFactoryProvider = projectEngineFactoryProvider;
        _compilerOptions = compilerOptions;
        _dispatcher = new(loggerFactory);
        _logger = loggerFactory.GetOrCreateLogger(GetType());

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

    public ImmutableArray<ProjectSnapshot> GetProjects()
    {
        using (_readerWriterLock.DisposableRead())
        {
            using var builder = new PooledArrayBuilder<ProjectSnapshot>(_projectMap.Count);

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

    public bool ContainsProject(ProjectKey projectKey)
    {
        using (_readerWriterLock.DisposableRead())
        {
            return _projectMap.ContainsKey(projectKey);
        }
    }

    public bool TryGetProject(ProjectKey projectKey, [NotNullWhen(true)] out ProjectSnapshot? project)
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

    public ImmutableArray<ProjectKey> GetProjectKeysWithFilePath(string filePath)
    {
        using (_readerWriterLock.DisposableRead())
        {
            using var projects = new PooledArrayBuilder<ProjectKey>(capacity: _projectMap.Count);

            foreach (var (key, entry) in _projectMap)
            {
                if (FilePathComparer.Instance.Equals(entry.State.HostProject.FilePath, filePath))
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

    private void AddProject(HostProject hostProject)
    {
        if (TryAddProject(hostProject, out var newSnapshot, out var isSolutionClosing))
        {
            NotifyListeners(ProjectChangeEventArgs.ProjectAdded(newSnapshot, isSolutionClosing));
        }
    }

    private void RemoveProject(ProjectKey projectKey)
    {
        if (TryRemoveProject(projectKey, out var oldProject, out var isSolutionClosing))
        {
            NotifyListeners(ProjectChangeEventArgs.ProjectRemoved(oldProject, isSolutionClosing));
        }
    }

    private void UpdateProjectConfiguration(HostProject hostProject)
    {
        if (TryUpdateProject(
            hostProject.Key,
            transformer: state => state.WithHostProject(hostProject),
            out var oldProject,
            out var newProject,
            out var isSolutionClosing))
        {
            NotifyListeners(ProjectChangeEventArgs.ProjectChanged(oldProject, newProject, isSolutionClosing));
        }
    }

    private void UpdateProjectWorkspaceState(ProjectKey projectKey, ProjectWorkspaceState projectWorkspaceState)
    {
        if (TryUpdateProject(
            projectKey,
            transformer: state => state.WithProjectWorkspaceState(projectWorkspaceState),
            out var oldProject,
            out var newProject,
            out var isSolutionClosing))
        {
            NotifyListeners(ProjectChangeEventArgs.ProjectChanged(oldProject, newProject, isSolutionClosing));
        }
    }

    private void AddDocument(ProjectKey projectKey, HostDocument hostDocument, SourceText text)
    {
        if (TryUpdateProject(
            projectKey,
            transformer: state => state.AddDocument(hostDocument, text),
            out var oldProject,
            out var newSnapshot,
            out var isSolutionClosing))
        {
            NotifyListeners(ProjectChangeEventArgs.DocumentAdded(oldProject, newSnapshot, hostDocument.FilePath, isSolutionClosing));
        }
    }

    private void AddDocument(ProjectKey projectKey, HostDocument hostDocument, TextLoader textLoader)
    {
        if (TryUpdateProject(
            projectKey,
            transformer: state => state.AddDocument(hostDocument, textLoader),
            out var oldProject,
            out var newProject,
            out var isSolutionClosing))
        {
            NotifyListeners(ProjectChangeEventArgs.DocumentAdded(oldProject, newProject, hostDocument.FilePath, isSolutionClosing));
        }
    }

    private void RemoveDocument(ProjectKey projectKey, string documentFilePath)
    {
        if (TryUpdateProject(
            projectKey,
            transformer: state => state.RemoveDocument(documentFilePath),
            out var oldProject,
            out var newProject,
            out var isSolutionClosing))
        {
            NotifyListeners(ProjectChangeEventArgs.DocumentRemoved(oldProject, newProject, documentFilePath, isSolutionClosing));
        }
    }

    private void OpenDocument(ProjectKey projectKey, string documentFilePath, SourceText text)
    {
        using (_readerWriterLock.DisposableWrite())
        {
            _openDocumentSet.Add(documentFilePath);
        }

        UpdateDocumentText(projectKey, documentFilePath, text);
    }

    private void CloseDocument(ProjectKey projectKey, string documentFilePath, TextLoader textLoader)
    {
        using (_readerWriterLock.DisposableWrite())
        {
            _openDocumentSet.Remove(documentFilePath);
        }

        UpdateDocumentText(projectKey, documentFilePath, textLoader);
    }

    private void UpdateDocumentText(ProjectKey projectKey, string documentFilePath, SourceText text)
    {
        if (TryUpdateProject(
            projectKey,
            transformer: state => state.WithDocumentText(documentFilePath, text),
            out var oldProject,
            out var newProject,
            out var isSolutionClosing))
        {
            NotifyListeners(ProjectChangeEventArgs.DocumentChanged(oldProject, newProject, documentFilePath, isSolutionClosing));
        }
    }

    private void UpdateDocumentText(ProjectKey projectKey, string documentFilePath, TextLoader textLoader)
    {
        if (TryUpdateProject(
            projectKey,
            transformer: state => state.WithDocumentText(documentFilePath, textLoader),
            out var oldProject,
            out var newProject,
            out var isSolutionClosing))
        {
            NotifyListeners(ProjectChangeEventArgs.DocumentChanged(oldProject, newProject, documentFilePath, isSolutionClosing));
        }
    }

    private bool TryAddProject(HostProject hostProject, [NotNullWhen(true)] out ProjectSnapshot? newProject, out bool isSolutionClosing)
    {
        if (_initialized)
        {
            _dispatcher.AssertRunningOnDispatcher();
        }

        using var upgradeableLock = _readerWriterLock.DisposableUpgradeableRead();

        isSolutionClosing = _isSolutionClosing;

        // If the solution is closing or the project already exists, don't add a new project.
        if (isSolutionClosing || _projectMap.ContainsKey(hostProject.Key))
        {
            newProject = null;
            return false;
        }

        var state = ProjectState.Create(hostProject, _compilerOptions, _projectEngineFactoryProvider);

        var newEntry = new Entry(state);

        upgradeableLock.EnterWrite();
        _projectMap.Add(hostProject.Key, newEntry);

        newProject = newEntry.GetSnapshot();
        return true;
    }

    private bool TryRemoveProject(ProjectKey projectKey, [NotNullWhen(true)] out ProjectSnapshot? oldProject, out bool isSolutionClosing)
    {
        if (_initialized)
        {
            _dispatcher.AssertRunningOnDispatcher();
        }

        using var upgradeableLock = _readerWriterLock.DisposableUpgradeableRead();

        isSolutionClosing = _isSolutionClosing;

        if (!_projectMap.TryGetValue(projectKey, out var entry))
        {
            oldProject = null;
            return false;
        }

        oldProject = entry.GetSnapshot();

        upgradeableLock.EnterWrite();
        _projectMap.Remove(projectKey);

        return true;
    }

    private bool TryUpdateProject(
        ProjectKey projectKey,
        Func<ProjectState, ProjectState> transformer,
        [NotNullWhen(true)] out ProjectSnapshot? oldProject,
        [NotNullWhen(true)] out ProjectSnapshot? newProject,
        out bool isSolutionClosing)
    {
        if (_initialized)
        {
            _dispatcher.AssertRunningOnDispatcher();
        }

        using var upgradeableLock = _readerWriterLock.DisposableUpgradeableRead();

        isSolutionClosing = _isSolutionClosing;

        if (!_projectMap.TryGetValue(projectKey, out var oldEntry))
        {
            oldProject = newProject = null;
            return false;
        }

        // If the solution is closing, we don't need to bother computing new state.
        if (isSolutionClosing)
        {
            oldProject = newProject = oldEntry.GetSnapshot();
            return true;
        }

        var oldState = oldEntry.State;
        var newState = transformer(oldState);

        if (ReferenceEquals(oldState, newState))
        {
            oldProject = newProject = null;
            return false;
        }

        upgradeableLock.EnterWrite();

        var newEntry = new Entry(newState);
        _projectMap[projectKey] = newEntry;

        oldProject = oldEntry.GetSnapshot();
        newProject = newEntry.GetSnapshot();

        return true;
    }

    private void NotifyListeners(ProjectChangeEventArgs notification)
    {
        if (!_initialized)
        {
            return;
        }

        // Notifications are *always* sent using the dispatcher.
        // This ensures that _notificationQueue and _processingNotifications are synchronized.
        _dispatcher.AssertRunningOnDispatcher();

        // Enqueue the latest notification.
        _notificationQueue.Enqueue(notification);

        // We're already processing the notification queue, so we're done.
        if (_processingNotifications)
        {
            return;
        }

        Debug.Assert(_notificationQueue.Count == 1, "There should only be a single queued notification when processing begins.");

        // The notification queue is processed when it contains *exactly* one notification.
        // Note that a notification subscriber may mutate the current solution and cause additional
        // notifications to be be enqueued. However, because we are already running on the dispatcher,
        // those updates will occur synchronously.

        _processingNotifications = true;
        try
        {
            while (_notificationQueue.Count > 0)
            {
                var current = _notificationQueue.Dequeue();

                PriorityChanged?.Invoke(this, current);
                Changed?.Invoke(this, current);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while sending notifications.");
        }
        finally
        {
            _processingNotifications = false;
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
