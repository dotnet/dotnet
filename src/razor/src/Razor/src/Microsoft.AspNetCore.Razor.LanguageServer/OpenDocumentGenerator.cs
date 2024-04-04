// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal partial class OpenDocumentGenerator : IRazorStartupService, IDisposable
{
    // Using 10 milliseconds for the delay here because we want document synchronization to be very fast,
    // so that features like completion are not delayed, but at the same time we don't want to do more work
    // than necessary when both C# and HTML documents change at the same time, firing our event handler
    // twice. Through testing 10ms was a good balance towards providing some de-bouncing but having minimal
    // to no impact on results.
    //
    // It's worth noting that the queue implementation means that this delay is not restarted with each new
    // work item, so even in very high speed typing, with changes coming in at sub-10-millisecond speed,
    // the queue will still process documents even if the user doesn't pause at all, but also will not process
    // a document for each keystroke.
    private static readonly TimeSpan s_delay = TimeSpan.FromMilliseconds(10);

    private readonly ImmutableArray<DocumentProcessedListener> _listeners;
    private readonly IProjectSnapshotManager _projectManager;
    private readonly ProjectSnapshotManagerDispatcher _dispatcher;
    private readonly LanguageServerFeatureOptions _options;

    private readonly AsyncBatchingWorkQueue<IDocumentSnapshot> _workQueue;
    private readonly CancellationTokenSource _disposeTokenSource;

    public OpenDocumentGenerator(
        IEnumerable<DocumentProcessedListener> listeners,
        IProjectSnapshotManager projectManager,
        ProjectSnapshotManagerDispatcher dispatcher,
        LanguageServerFeatureOptions options)
    {
        _listeners = listeners.ToImmutableArray();
        _projectManager = projectManager;
        _dispatcher = dispatcher;
        _options = options;

        _disposeTokenSource = new();
        _workQueue = new AsyncBatchingWorkQueue<IDocumentSnapshot>(
            s_delay,
            ProcessBatchAsync,
            _disposeTokenSource.Token);

        _projectManager.Changed += ProjectManager_Changed;

        foreach (var listener in _listeners)
        {
            listener.Initialize(_projectManager);
        }
    }

    public void Dispose()
    {
        _disposeTokenSource.Cancel();
        _disposeTokenSource.Dispose();
    }

    private async ValueTask ProcessBatchAsync(ImmutableArray<IDocumentSnapshot> items, CancellationToken token)
    {
        foreach (var document in items.GetMostRecentUniqueItems(Comparer.Instance))
        {
            if (token.IsCancellationRequested)
            {
                return;
            }

            var codeDocument = await document.GetGeneratedOutputAsync().ConfigureAwait(false);

            await _dispatcher
                .RunAsync(
                    static state =>
                    {
                        var (codeDocument, document, listeners, token) = state;

                        foreach (var listener in listeners)
                        {
                            if (token.IsCancellationRequested)
                            {
                                return;
                            }

                            listener.DocumentProcessed(codeDocument, document);
                        }
                    },
                    state: (codeDocument, document, _listeners, token),
                    token)
                .ConfigureAwait(false);
        }
    }

    private void ProjectManager_Changed(object? sender, ProjectChangeEventArgs args)
    {
        // Don't do any work if the solution is closing
        if (args.SolutionIsClosing)
        {
            return;
        }

        switch (args.Kind)
        {
            case ProjectChangeKind.ProjectChanged:
                {
                    var newProject = args.Newer.AssumeNotNull();

                    foreach (var documentFilePath in newProject.DocumentFilePaths)
                    {
                        if (newProject.TryGetDocument(documentFilePath, out var document))
                        {
                            EnqueueIfNecessary(document);
                        }
                    }

                    break;
                }

            case ProjectChangeKind.DocumentAdded:
                {
                    var newProject = args.Newer.AssumeNotNull();
                    var documentFilePath = args.DocumentFilePath.AssumeNotNull();

                    if (newProject.TryGetDocument(documentFilePath, out var document))
                    {
                        // We don't enqueue the current document because added documents are initially closed.

                        foreach (var relatedDocument in newProject.GetRelatedDocuments(document))
                        {
                            EnqueueIfNecessary(relatedDocument);
                        }
                    }

                    break;
                }

            case ProjectChangeKind.DocumentChanged:
                {
                    var newProject = args.Newer.AssumeNotNull();
                    var documentFilePath = args.DocumentFilePath.AssumeNotNull();

                    if (newProject.TryGetDocument(documentFilePath, out var document))
                    {
                        EnqueueIfNecessary(document);

                        foreach (var relatedDocument in newProject.GetRelatedDocuments(document))
                        {
                            EnqueueIfNecessary(relatedDocument);
                        }
                    }

                    break;
                }

            case ProjectChangeKind.DocumentRemoved:
                {
                    var newProject = args.Newer.AssumeNotNull();
                    var oldProject = args.Older.AssumeNotNull();
                    var documentFilePath = args.DocumentFilePath.AssumeNotNull();

                    if (oldProject.TryGetDocument(documentFilePath, out var document))
                    {
                        foreach (var relatedDocument in oldProject.GetRelatedDocuments(document))
                        {
                            var relatedDocumentFilePath = relatedDocument.FilePath.AssumeNotNull();

                            if (newProject.TryGetDocument(relatedDocumentFilePath, out var newRelatedDocument))
                            {
                                EnqueueIfNecessary(newRelatedDocument);
                            }
                        }
                    }

                    break;
                }

            case ProjectChangeKind.ProjectRemoved:
                {
                    // No-op. We don't need to enqueue recompilations if the project is being removed
                    break;
                }
        }

        void EnqueueIfNecessary(IDocumentSnapshot document)
        {
            if (!_projectManager.IsDocumentOpen(document.FilePath.AssumeNotNull()) &&
                !_options.UpdateBuffersForClosedDocuments)
            {
                return;
            }

            _workQueue.AddWork(document);
        }
    }
}
