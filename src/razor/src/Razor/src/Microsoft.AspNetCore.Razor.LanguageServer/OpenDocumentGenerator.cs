// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class OpenDocumentGenerator : IRazorStartupService, IDisposable
{
    // Using 10 milliseconds for the delay here because we want document synchronization to be very fast,
    // so that features like completion are not delayed, but at the same time we don't want to do more work
    // than necessary when both C# and HTML documents change at the same time, firing our event handler
    // twice. Through testing 10ms was a good balance towards providing some de-bouncing but having minimal
    // to no impact on results.
    // It's worth noting that the queue implementation means that this delay is not restarted with each new
    // work item, so even in very high speed typing, with changings coming in at sub-10-millisecond speed,
    // the queue will still process documents even if the user doesn't pause at all, but also will not process
    // a document for each keystroke.
    private static readonly TimeSpan s_batchingTimeSpan = TimeSpan.FromMilliseconds(10);

    private readonly IProjectSnapshotManager _projectManager;
    private readonly ProjectSnapshotManagerDispatcher _dispatcher;
    private readonly LanguageServerFeatureOptions _options;
    private readonly IReadOnlyList<DocumentProcessedListener> _listeners;
    private readonly BatchingWorkQueue _workQueue;

    public OpenDocumentGenerator(
        IEnumerable<DocumentProcessedListener> listeners,
        IProjectSnapshotManager projectManager,
        ProjectSnapshotManagerDispatcher dispatcher,
        LanguageServerFeatureOptions options,
        IErrorReporter errorReporter)
    {
        _listeners = listeners.ToArray();
        _projectManager = projectManager;
        _dispatcher = dispatcher;
        _options = options;
        _workQueue = new BatchingWorkQueue(s_batchingTimeSpan, FilePathComparer.Instance, errorReporter);

        _projectManager.Changed += ProjectManager_Changed;

        foreach (var listener in _listeners)
        {
            listener.Initialize(_projectManager);
        }
    }

    public void Dispose()
    {
        _workQueue.Dispose();
    }

    private void ProjectManager_Changed(object? sender, ProjectChangeEventArgs args)
    {
        // Don't do any work if the solution is closing
        if (args.SolutionIsClosing)
        {
            return;
        }

        _dispatcher.AssertRunningOnDispatcher();

        switch (args.Kind)
        {
            case ProjectChangeKind.ProjectChanged:
                {
                    var newProject = args.Newer.AssumeNotNull();

                    foreach (var documentFilePath in newProject.DocumentFilePaths)
                    {
                        if (newProject.GetDocument(documentFilePath) is { } document)
                        {
                            TryEnqueue(document);
                        }
                    }

                    break;
                }

            case ProjectChangeKind.DocumentAdded:
                {
                    var newProject = args.Newer.AssumeNotNull();
                    var documentFilePath = args.DocumentFilePath.AssumeNotNull();

                    if (newProject.GetDocument(documentFilePath) is { } document)
                    {
                        // We don't enqueue the current document because added documents are by default closed.

                        foreach (var relatedDocument in newProject.GetRelatedDocuments(document))
                        {
                            TryEnqueue(relatedDocument);
                        }
                    }

                    break;
                }

            case ProjectChangeKind.DocumentChanged:
                {
                    var newProject = args.Newer.AssumeNotNull();
                    var documentFilePath = args.DocumentFilePath.AssumeNotNull();

                    if (newProject.GetDocument(documentFilePath) is { } document)
                    {
                        TryEnqueue(document);

                        foreach (var relatedDocument in newProject.GetRelatedDocuments(document))
                        {
                            TryEnqueue(relatedDocument);
                        }
                    }

                    break;
                }

            case ProjectChangeKind.DocumentRemoved:
                {
                    var newProject = args.Newer.AssumeNotNull();
                    var oldProject = args.Older.AssumeNotNull();
                    var documentFilePath = args.DocumentFilePath.AssumeNotNull();

                    if (oldProject.GetDocument(documentFilePath) is { } document)
                    {
                        foreach (var relatedDocument in oldProject.GetRelatedDocuments(document))
                        {
                            var relatedDocumentFilePath = relatedDocument.FilePath.AssumeNotNull();

                            if (newProject.GetDocument(relatedDocumentFilePath) is { } newRelatedDocument)
                            {
                                TryEnqueue(newRelatedDocument);
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

        void TryEnqueue(IDocumentSnapshot document)
        {
            var documentFilePath = document.FilePath.AssumeNotNull();

            if (!_projectManager.IsDocumentOpen(documentFilePath) &&
                !_options.UpdateBuffersForClosedDocuments)
            {
                return;
            }

            var key = $"{document.Project.Key.Id}:{documentFilePath}";
            var workItem = new ProcessWorkItem(document, _listeners, _dispatcher);
            _workQueue.Enqueue(key, workItem);
        }
    }

    private class ProcessWorkItem : BatchableWorkItem
    {
        private readonly IDocumentSnapshot _latestDocument;
        private readonly IEnumerable<DocumentProcessedListener> _documentProcessedListeners;
        private readonly ProjectSnapshotManagerDispatcher _dispatcher;

        public ProcessWorkItem(
            IDocumentSnapshot latestDocument,
            IReadOnlyList<DocumentProcessedListener> documentProcessedListeners,
            ProjectSnapshotManagerDispatcher dispatcher)
        {
            _latestDocument = latestDocument;
            _documentProcessedListeners = documentProcessedListeners;
            _dispatcher = dispatcher;
        }

        public override async ValueTask ProcessAsync(CancellationToken cancellationToken)
        {
            var codeDocument = await _latestDocument.GetGeneratedOutputAsync().ConfigureAwait(false);

            await _dispatcher.RunAsync(() =>
            {
                foreach (var listener in _documentProcessedListeners)
                {
                    listener.DocumentProcessed(codeDocument, _latestDocument);
                }
            }, cancellationToken).ConfigureAwait(false);
        }
    }
}
