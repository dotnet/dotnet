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

internal class OpenDocumentGenerator : IProjectSnapshotChangeTrigger, IDisposable
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
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;
    private readonly IReadOnlyList<DocumentProcessedListener> _documentProcessedListeners;
    private readonly BatchingWorkQueue _workQueue;
    private ProjectSnapshotManagerBase? _projectManager;

    public OpenDocumentGenerator(
        IEnumerable<DocumentProcessedListener> documentProcessedListeners,
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        LanguageServerFeatureOptions languageServerFeatureOptions,
        IErrorReporter errorReporter)
    {
        if (documentProcessedListeners is null)
        {
            throw new ArgumentNullException(nameof(documentProcessedListeners));
        }

        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        if (languageServerFeatureOptions is null)
        {
            throw new ArgumentNullException(nameof(languageServerFeatureOptions));
        }

        _documentProcessedListeners = documentProcessedListeners.ToArray();
        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _languageServerFeatureOptions = languageServerFeatureOptions;
        _workQueue = new BatchingWorkQueue(s_batchingTimeSpan, FilePathComparer.Instance, errorReporter);
    }

    private ProjectSnapshotManagerBase ProjectManager => _projectManager ?? throw new InvalidOperationException($"{nameof(ProjectManager)} was unexpectedly 'null'. Has {nameof(Initialize)} been called?");

    public void Initialize(ProjectSnapshotManagerBase projectManager)
    {
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
            if (!ProjectManager.IsDocumentOpen(document.FilePath.AssumeNotNull()) && !_languageServerFeatureOptions.UpdateBuffersForClosedDocuments)
            {
                return;
            }

            var key = $"{document.Project.Key.Id}:{document.FilePath.AssumeNotNull()}";
            var workItem = new ProcessWorkItem(document, _documentProcessedListeners, _projectSnapshotManagerDispatcher);
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
