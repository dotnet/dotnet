// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.Razor.ProjectSystem;

namespace Microsoft.VisualStudio.Razor.DynamicFiles;

[Export(typeof(IRazorStartupService))]
internal partial class BackgroundDocumentGenerator : IRazorStartupService, IDisposable
{
    private static readonly TimeSpan s_delay = TimeSpan.FromSeconds(2);

    private readonly ProjectSnapshotManager _projectManager;
    private readonly IFallbackProjectManager _fallbackProjectManager;
    private readonly IRazorDynamicFileInfoProviderInternal _infoProvider;
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger _logger;

    private readonly CancellationTokenSource _disposeTokenSource;
    private readonly AsyncBatchingWorkQueue<DocumentKey> _workQueue;
    private readonly HashSet<DocumentKey> _workerSet;
    private ImmutableHashSet<string> _suppressedDocuments;
    private bool _solutionIsClosing;

    [ImportingConstructor]
    public BackgroundDocumentGenerator(
        ProjectSnapshotManager projectManager,
        IFallbackProjectManager fallbackProjectManager,
        IRazorDynamicFileInfoProviderInternal infoProvider,
        ILoggerFactory loggerFactory)
        : this(projectManager, fallbackProjectManager, infoProvider, loggerFactory, s_delay)
    {
    }

    // Provided for tests to be able to modify the timer delay
    protected BackgroundDocumentGenerator(
        ProjectSnapshotManager projectManager,
        IFallbackProjectManager fallbackProjectManager,
        IRazorDynamicFileInfoProviderInternal infoProvider,
        ILoggerFactory loggerFactory,
        TimeSpan delay)
    {
        _projectManager = projectManager;
        _fallbackProjectManager = fallbackProjectManager;
        _infoProvider = infoProvider;
        _loggerFactory = loggerFactory;
        _logger = loggerFactory.GetOrCreateLogger<BackgroundDocumentGenerator>();

        _workerSet = [];
        _disposeTokenSource = new();
        _workQueue = new AsyncBatchingWorkQueue<DocumentKey>(
            delay,
            processBatchAsync: ProcessBatchAsync,
            equalityComparer: null,
            idleAction: RazorEventSource.Instance.BackgroundDocumentGeneratorIdle,
            _disposeTokenSource.Token);
        _suppressedDocuments = ImmutableHashSet<string>.Empty.WithComparer(FilePathComparer.Instance);
        _projectManager.Changed += ProjectManager_Changed;
    }

    public void Dispose()
    {
        if (_disposeTokenSource.IsCancellationRequested)
        {
            return;
        }

        _disposeTokenSource.Cancel();
        _disposeTokenSource.Dispose();
    }

    protected Task WaitUntilCurrentBatchCompletesAsync()
        => _workQueue.WaitUntilCurrentBatchCompletesAsync();

    protected virtual Task ProcessDocumentAsync(DocumentSnapshot document, CancellationToken cancellationToken)
    {
        UpdateFileInfo(document);

        return Task.CompletedTask;
    }

    public virtual void EnqueueIfNecessary(DocumentKey documentKey)
    {
        if (_disposeTokenSource.IsCancellationRequested)
        {
            return;
        }

        if (_fallbackProjectManager.IsFallbackProject(documentKey.ProjectKey))
        {
            // We don't support closed file code generation for fallback projects
            return;
        }

        if (Suppressed(documentKey))
        {
            return;
        }

        _workQueue.AddWork(documentKey);
    }

    protected virtual async ValueTask ProcessBatchAsync(ImmutableArray<DocumentKey> items, CancellationToken token)
    {
        _workerSet.Clear();

        foreach (var key in items.GetMostRecentUniqueItems(_workerSet))
        {
            if (token.IsCancellationRequested)
            {
                return;
            }

            // If the solution is closing, avoid any in-progress work.
            if (_solutionIsClosing)
            {
                return;
            }

            if (!_projectManager.TryGetDocument(key, out var document))
            {
                continue;
            }

            try
            {
                await ProcessDocumentAsync(document, token).ConfigureAwait(false);
            }
            catch (UnauthorizedAccessException)
            {
                // Ignore UnauthorizedAccessException. These can occur when a file gets its permissions changed as we're processing it.
            }
            catch (IOException)
            {
                // Ignore IOException. These can occur when a file was in the middle of being renamed and it disappears as we're processing it.
                // This is a common case and does not warrant an activity log entry.
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error encountered from project '{document.Project.FilePath}':{Environment.NewLine}{ex}");
            }
        }
    }

    private bool Suppressed(DocumentKey documentKey)
    {
        var filePath = documentKey.FilePath;

        if (_projectManager.IsDocumentOpen(filePath))
        {
            ImmutableInterlocked.Update(ref _suppressedDocuments, static (set, filePath) => set.Add(filePath), filePath);
            _infoProvider.SuppressDocument(documentKey);
            return true;
        }

        ImmutableInterlocked.Update(ref _suppressedDocuments, static (set, filePath) => set.Remove(filePath), filePath);
        return false;
    }

    private void UpdateFileInfo(DocumentSnapshot document)
    {
        var filePath = document.FilePath;

        if (!_suppressedDocuments.Contains(filePath))
        {
            var container = new DefaultDynamicDocumentContainer(document, _loggerFactory);
            _infoProvider.UpdateFileInfo(document.Project.Key, container);
        }
    }

    private void ProjectManager_Changed(object sender, ProjectChangeEventArgs args)
    {
        // We don't want to do any work on solution close.
        if (args.IsSolutionClosing)
        {
            _solutionIsClosing = true;
            return;
        }

        _solutionIsClosing = false;

        switch (args.Kind)
        {
            case ProjectChangeKind.ProjectAdded:
            case ProjectChangeKind.ProjectChanged:
                {
                    var newProject = args.Newer.AssumeNotNull();

                    foreach (var documentFilePath in newProject.DocumentFilePaths)
                    {
                        EnqueueIfNecessary(new(newProject.Key, documentFilePath));
                    }

                    break;
                }

            case ProjectChangeKind.DocumentAdded:
            case ProjectChangeKind.DocumentChanged:
                {
                    var newProject = args.Newer.AssumeNotNull();
                    var documentFilePath = args.DocumentFilePath.AssumeNotNull();

                    EnqueueIfNecessary(new(newProject.Key, documentFilePath));

                    foreach (var relatedDocumentFilePath in newProject.GetRelatedDocumentFilePaths(documentFilePath))
                    {
                        EnqueueIfNecessary(new(newProject.Key, relatedDocumentFilePath));
                    }

                    break;
                }

            case ProjectChangeKind.DocumentRemoved:
                {
                    var newProject = args.Newer.AssumeNotNull();
                    var oldProject = args.Older.AssumeNotNull();
                    var documentFilePath = args.DocumentFilePath.AssumeNotNull();

                    // For removals use the old snapshot to find related documents to update if they exist
                    // in the new snapshot.

                    foreach (var relatedDocumentFilePath in oldProject.GetRelatedDocumentFilePaths(documentFilePath))
                    {
                        if (newProject.ContainsDocument(relatedDocumentFilePath))
                        {
                            EnqueueIfNecessary(new(newProject.Key, relatedDocumentFilePath));
                        }
                    }

                    break;
                }

            case ProjectChangeKind.ProjectRemoved:
                {
                    // No-op. We don't need to compile anything if the project is being removed
                    break;
                }

            default:
                Assumed.Unreachable($"Unknown {nameof(ProjectChangeKind)}: {args.Kind}");
                break;
        }
    }
}
