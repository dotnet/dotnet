// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
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

    private readonly IProjectSnapshotManager _projectManager;
    private readonly IRazorDynamicFileInfoProviderInternal _infoProvider;
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger _logger;

    private readonly CancellationTokenSource _disposeTokenSource;
    private readonly AsyncBatchingWorkQueue<(IProjectSnapshot, IDocumentSnapshot)> _workQueue;
    private ImmutableHashSet<string> _suppressedDocuments;
    private bool _solutionIsClosing;

    [ImportingConstructor]
    public BackgroundDocumentGenerator(
        IProjectSnapshotManager projectManager,
        IRazorDynamicFileInfoProviderInternal infoProvider,
        ILoggerFactory loggerFactory)
        : this(projectManager, infoProvider, loggerFactory, s_delay)
    {
    }

    // Provided for tests to be able to modify the timer delay
    protected BackgroundDocumentGenerator(
        IProjectSnapshotManager projectManager,
        IRazorDynamicFileInfoProviderInternal infoProvider,
        ILoggerFactory loggerFactory,
        TimeSpan delay)
    {
        _projectManager = projectManager;
        _infoProvider = infoProvider;
        _loggerFactory = loggerFactory;
        _logger = loggerFactory.GetOrCreateLogger<BackgroundDocumentGenerator>();

        _disposeTokenSource = new();
        _workQueue = new AsyncBatchingWorkQueue<(IProjectSnapshot, IDocumentSnapshot)>(
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

    protected virtual async Task ProcessDocumentAsync(IProjectSnapshot project, IDocumentSnapshot document, CancellationToken cancellationToken)
    {
        await document.GetGeneratedOutputAsync(cancellationToken).ConfigureAwait(false);

        UpdateFileInfo(project, document);
    }

    public virtual void Enqueue(IProjectSnapshot project, IDocumentSnapshot document)
    {
        if (_disposeTokenSource.IsCancellationRequested)
        {
            return;
        }

        if (project is ProjectSnapshot { HostProject: FallbackHostProject })
        {
            // We don't support closed file code generation for fallback projects
            return;
        }

        if (Suppressed(project, document))
        {
            return;
        }

        _workQueue.AddWork((project, document));
    }

    protected virtual async ValueTask ProcessBatchAsync(ImmutableArray<(IProjectSnapshot, IDocumentSnapshot)> items, CancellationToken token)
    {
        foreach (var (project, document) in items.GetMostRecentUniqueItems(Comparer.Instance))
        {
            if (token.IsCancellationRequested)
            {
                return;
            }

            // If the solution is closing, suspect any in-progress work
            if (_solutionIsClosing)
            {
                break;
            }

            try
            {
                await ProcessDocumentAsync(project, document, token).ConfigureAwait(false);
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
                _logger.LogError(ex, $"Error encountered from project '{project.FilePath}':{Environment.NewLine}{ex}");
            }
        }
    }

    private bool Suppressed(IProjectSnapshot project, IDocumentSnapshot document)
    {
        var filePath = document.FilePath;

        if (_projectManager.IsDocumentOpen(filePath))
        {
            ImmutableInterlocked.Update(ref _suppressedDocuments, static (set, filePath) => set.Add(filePath), filePath);
            _infoProvider.SuppressDocument(project.Key, filePath);
            return true;
        }

        ImmutableInterlocked.Update(ref _suppressedDocuments, static (set, filePath) => set.Remove(filePath), filePath);
        return false;
    }

    private void UpdateFileInfo(IProjectSnapshot project, IDocumentSnapshot document)
    {
        var filePath = document.FilePath;

        if (!_suppressedDocuments.Contains(filePath))
        {
            var container = new DefaultDynamicDocumentContainer(document, _loggerFactory);
            _infoProvider.UpdateFileInfo(project.Key, container);
        }
    }

    private void ProjectManager_Changed(object sender, ProjectChangeEventArgs args)
    {
        // We don't want to do any work on solution close
        if (args.IsSolutionClosing)
        {
            _solutionIsClosing = true;
            return;
        }

        _solutionIsClosing = false;

        switch (args.Kind)
        {
            case ProjectChangeKind.ProjectAdded:
                {
                    var newProject = args.Newer.AssumeNotNull();

                    foreach (var documentFilePath in newProject.DocumentFilePaths)
                    {
                        if (newProject.TryGetDocument(documentFilePath, out var document))
                        {
                            Enqueue(newProject, document);
                        }
                    }

                    break;
                }

            case ProjectChangeKind.ProjectChanged:
                {
                    var newProject = args.Newer.AssumeNotNull();

                    foreach (var documentFilePath in newProject.DocumentFilePaths)
                    {
                        if (newProject.TryGetDocument(documentFilePath, out var document))
                        {
                            Enqueue(newProject, document);
                        }
                    }

                    break;
                }

            case ProjectChangeKind.DocumentAdded:
            case ProjectChangeKind.DocumentChanged:
                {
                    var newProject = args.Newer.AssumeNotNull();
                    var documentFilePath = args.DocumentFilePath.AssumeNotNull();

                    if (newProject.TryGetDocument(documentFilePath, out var document))
                    {
                        Enqueue(newProject, document);

                        foreach (var relatedDocument in newProject.GetRelatedDocuments(document))
                        {
                            Enqueue(newProject, relatedDocument);
                        }
                    }

                    break;
                }

            case ProjectChangeKind.DocumentRemoved:
                {
                    // For removals use the old snapshot to find the removed document, so we can figure out
                    // what the imports were in the new snapshot.
                    var newProject = args.Newer.AssumeNotNull();
                    var oldProject = args.Older.AssumeNotNull();
                    var documentFilePath = args.DocumentFilePath.AssumeNotNull();

                    if (oldProject.TryGetDocument(documentFilePath, out var document))
                    {
                        foreach (var relatedDocument in newProject.GetRelatedDocuments(document))
                        {
                            Enqueue(newProject, relatedDocument);
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
                throw new InvalidOperationException($"Unknown ProjectChangeKind {args.Kind}");
        }
    }
}
