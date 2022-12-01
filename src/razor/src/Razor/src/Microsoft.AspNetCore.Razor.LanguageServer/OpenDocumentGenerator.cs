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

internal class OpenDocumentGenerator : ProjectSnapshotChangeTrigger, IDisposable
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

    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private readonly IReadOnlyList<DocumentProcessedListener> _documentProcessedListeners;
    private readonly BatchingWorkQueue _workQueue;
    private ProjectSnapshotManagerBase? _projectManager;

    public OpenDocumentGenerator(
        IEnumerable<DocumentProcessedListener> documentProcessedListeners,
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        ErrorReporter errorReporter)
    {
        if (documentProcessedListeners is null)
        {
            throw new ArgumentNullException(nameof(documentProcessedListeners));
        }

        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        _documentProcessedListeners = documentProcessedListeners.ToArray();
        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _workQueue = new BatchingWorkQueue(s_batchingTimeSpan, FilePathComparer.Instance, errorReporter);
    }

    private ProjectSnapshotManagerBase ProjectManager => _projectManager ?? throw new InvalidOperationException($"{nameof(ProjectManager)} was unexpectedly 'null'. Has {nameof(Initialize)} been called?");

    public override void Initialize(ProjectSnapshotManagerBase projectManager)
    {
        if (projectManager is null)
        {
            throw new ArgumentNullException(nameof(projectManager));
        }

        _projectManager = projectManager;

        ProjectManager.Changed += ProjectSnapshotManager_Changed;

        foreach (var documentProcessedListener in _documentProcessedListeners)
        {
            documentProcessedListener.Initialize(ProjectManager);
        }
    }

    public void Dispose()
    {
        _workQueue.Dispose();
    }

    private void ProjectSnapshotManager_Changed(object? sender, ProjectChangeEventArgs args)
    {
        // Don't do any work if the solution is closing
        if (args.SolutionIsClosing)
        {
            return;
        }

        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        switch (args.Kind)
        {
            case ProjectChangeKind.ProjectChanged:
                {
                    var projectSnapshot = args.Newer!;
                    foreach (var documentFilePath in projectSnapshot.DocumentFilePaths)
                    {
                        var document = projectSnapshot.GetDocument(documentFilePath);
                        TryEnqueue(document);
                    }

                    break;
                }

            case ProjectChangeKind.DocumentAdded:
                {
                    var projectSnapshot = args.Newer!;
                    var document = projectSnapshot.GetDocument(args.DocumentFilePath);

                    // We don't enqueue the current document because added documents are by default closed.

                    foreach (var relatedDocument in projectSnapshot.GetRelatedDocuments(document))
                    {
                        TryEnqueue(relatedDocument);
                    }

                    break;
                }

            case ProjectChangeKind.DocumentChanged:
                {
                    var projectSnapshot = args.Newer!;
                    var document = projectSnapshot.GetDocument(args.DocumentFilePath);

                    TryEnqueue(document);

                    foreach (var relatedDocument in projectSnapshot.GetRelatedDocuments(document))
                    {
                        TryEnqueue(relatedDocument);
                    }

                    break;
                }

            case ProjectChangeKind.DocumentRemoved:
                {
                    var olderProject = args.Older!;
                    var document = olderProject.GetDocument(args.DocumentFilePath);

                    foreach (var relatedDocument in olderProject.GetRelatedDocuments(document))
                    {
                        var newerRelatedDocument = args.Newer!.GetDocument(relatedDocument.FilePath);
                        TryEnqueue(newerRelatedDocument);
                    }

                    break;
                }

                void TryEnqueue(DocumentSnapshot document)
                {
                    if (!ProjectManager.IsDocumentOpen(document.FilePath))
                    {
                        return;
                    }

                    var workItem = new ProcessWorkItem(document, _documentProcessedListeners, _projectSnapshotManagerDispatcher);
                    _workQueue.Enqueue(document.FilePath, workItem);
                }
        }
    }

    private class ProcessWorkItem : BatchableWorkItem
    {
        private readonly DocumentSnapshot _latestDocument;
        private readonly IEnumerable<DocumentProcessedListener> _documentProcessedListeners;
        private readonly ProjectSnapshotManagerDispatcher _dispatcher;

        public ProcessWorkItem(
            DocumentSnapshot latestDocument,
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

            await _dispatcher.RunOnDispatcherThreadAsync(() =>
            {
                foreach (var listener in _documentProcessedListeners)
                {
                    listener.DocumentProcessed(codeDocument, _latestDocument);
                }
            }, cancellationToken).ConfigureAwait(false);
        }
    }
}
