// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Common;

internal class BackgroundDocumentGenerator : IProjectSnapshotChangeTrigger
{
    private record struct WorkResult(RazorCodeDocument Output, IDocumentSnapshot Document);

    private readonly ProjectSnapshotManagerDispatcher _dispatcher;
    private readonly IEnumerable<DocumentProcessedListener> _listeners;
    private readonly Dictionary<DocumentKey, IDocumentSnapshot> _work;
    private ProjectSnapshotManagerBase? _projectManager;
    private Timer? _timer;
    private bool _solutionIsClosing;

    public BackgroundDocumentGenerator(
        ProjectSnapshotManagerDispatcher dispatcher,
        IEnumerable<DocumentProcessedListener> listeners)
    {
        _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        _listeners = listeners ?? throw new ArgumentNullException(nameof(listeners));
        _work = new Dictionary<DocumentKey, IDocumentSnapshot>();
    }

    // For testing only
    protected BackgroundDocumentGenerator(
        ProjectSnapshotManagerDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
        _work = new Dictionary<DocumentKey, IDocumentSnapshot>();
        _listeners = Enumerable.Empty<DocumentProcessedListener>();
    }

    public bool HasPendingNotifications
    {
        get
        {
            lock (_work)
            {
                return _work.Count > 0;
            }
        }
    }

    public TimeSpan Delay { get; set; } = TimeSpan.Zero;

    public bool IsScheduledOrRunning => _timer != null;

    // Used in tests to ensure we can control when background work starts.
    public ManualResetEventSlim? BlockBackgroundWorkStart { get; set; }

    // Used in tests to ensure we can know when background work finishes.
    public ManualResetEventSlim? NotifyBackgroundWorkStarting { get; set; }

    // Used in unit tests to ensure we can know when background has captured its current workload.
    public ManualResetEventSlim? NotifyBackgroundCapturedWorkload { get; set; }

    // Used in tests to ensure we can control when background work completes.
    public ManualResetEventSlim? BlockBackgroundWorkCompleting { get; set; }

    // Used in tests to ensure we can know when background work finishes.
    public ManualResetEventSlim? NotifyBackgroundWorkCompleted { get; set; }

    [MemberNotNull(nameof(_projectManager))]
    public void Initialize(ProjectSnapshotManagerBase projectManager)
    {
        _projectManager = projectManager;

        _projectManager.Changed += ProjectSnapshotManager_Changed;

        foreach (var documentProcessedListener in _listeners)
        {
            documentProcessedListener.Initialize(_projectManager);
        }
    }

    private void OnStartingBackgroundWork()
    {
        if (BlockBackgroundWorkStart != null)
        {
            BlockBackgroundWorkStart.Wait();
            BlockBackgroundWorkStart.Reset();
        }

        if (NotifyBackgroundWorkStarting != null)
        {
            NotifyBackgroundWorkStarting.Set();
        }
    }

    private void OnCompletingBackgroundWork()
    {
        if (BlockBackgroundWorkCompleting != null)
        {
            BlockBackgroundWorkCompleting.Wait();
            BlockBackgroundWorkCompleting.Reset();
        }
    }

    private void OnCompletedBackgroundWork()
    {
        if (NotifyBackgroundWorkCompleted != null)
        {
            NotifyBackgroundWorkCompleted.Set();
        }
    }

    private void OnBackgroundCapturedWorkload()
    {
        if (NotifyBackgroundCapturedWorkload != null)
        {
            NotifyBackgroundCapturedWorkload.Set();
        }
    }

    // Internal for testing
    internal void Enqueue(IDocumentSnapshot document)
    {
        _dispatcher.AssertDispatcherThread();

        lock (_work)
        {
            // We only want to store the last 'seen' version of any given document. That way when we pick one to process
            // it's always the best version to use.
            var key = new DocumentKey(document.Project.Key, document.FilePath.AssumeNotNull());
            _work[key] = document;

            StartWorker();
        }
    }

    private void StartWorker()
    {
        // Access to the timer is protected by the lock in Synchronize and in Timer_Tick
        // Timer will fire after a fixed delay, but only once.
        _timer ??= new Timer(Timer_Tick, null, Delay, Timeout.InfiniteTimeSpan);
    }

    private void Timer_Tick(object? state)
    {
        Timer_TickAsync(CancellationToken.None).Forget();
    }

    private async Task Timer_TickAsync(CancellationToken cancellationToken)
    {
        try
        {
            OnStartingBackgroundWork();

            KeyValuePair<DocumentKey, IDocumentSnapshot>[] work;
            List<WorkResult> results = new();
            lock (_work)
            {
                work = _work.ToArray();
                _work.Clear();
            }

            OnBackgroundCapturedWorkload();

            for (var i = 0; i < work.Length; i++)
            {
                if (_solutionIsClosing)
                {
                    break;
                }

                var document = work[i].Value;
                try
                {
                    var codeDocument = await document.GetGeneratedOutputAsync().ConfigureAwait(false);
                    results.Add(new WorkResult(codeDocument, document));
                }
                catch (Exception ex)
                {
                    ReportError(ex);
                }
            }

            OnCompletingBackgroundWork();

            if (!_solutionIsClosing)
            {
                await _dispatcher.RunOnDispatcherThreadAsync(
                    () => NotifyDocumentsProcessed(results),
                    cancellationToken).ConfigureAwait(false);
            }

            lock (_work)
            {
                // Suppress analyzer that suggests using DisposeAsync().
#pragma warning disable VSTHRD103 // Call async methods when in an async method

                // Resetting the timer allows another batch of work to start.
                _timer?.Dispose();
                _timer = null;

#pragma warning restore VSTHRD103

                // If more work came in while we were running start the worker again.
                if (_work.Count > 0 && !_solutionIsClosing)
                {
                    StartWorker();
                }
            }

            OnCompletedBackgroundWork();
        }
        catch (Exception ex)
        {
            // This is something totally unexpected, let's just send it over to the workspace.
            ReportError(ex);

            // Suppress analyzer that suggests using DisposeAsync().
#pragma warning disable VSTHRD103 // Call async methods when in an async method

            _timer?.Dispose();
            _timer = null;

#pragma warning restore VSTHRD103
        }
    }

    private void NotifyDocumentsProcessed(List<WorkResult> results)
    {
        for (var i = 0; i < results.Count; i++)
        {
            foreach (var documentProcessedTrigger in _listeners)
            {
                var document = results[i].Document;
                var codeDocument = results[i].Output;

                documentProcessedTrigger.DocumentProcessed(codeDocument, document);
            }
        }
    }

    private void ProjectSnapshotManager_Changed(object? sender, ProjectChangeEventArgs args)
    {
        // Don't do any work if the solution is closing
        if (args.SolutionIsClosing)
        {
            _solutionIsClosing = true;
            return;
        }

        _solutionIsClosing = false;

        _dispatcher.AssertDispatcherThread();

        switch (args.Kind)
        {
            case ProjectChangeKind.ProjectAdded:
                {
                    var newProject = args.Newer.AssumeNotNull();

                    foreach (var documentFilePath in newProject.DocumentFilePaths)
                    {
                        if (newProject.GetDocument(documentFilePath) is { } document)
                        {
                            Enqueue(document);
                        }
                    }

                    break;
                }
            case ProjectChangeKind.ProjectChanged:
                {
                    var newProject = args.Newer.AssumeNotNull();

                    foreach (var documentFilePath in newProject.DocumentFilePaths)
                    {
                        if (newProject.GetDocument(documentFilePath) is { } document)
                        {
                            Enqueue(document);
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
                        Enqueue(document);

                        foreach (var relatedDocument in newProject.GetRelatedDocuments(document))
                        {
                            Enqueue(relatedDocument);
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
                        Enqueue(document);

                        foreach (var relatedDocument in newProject.GetRelatedDocuments(document))
                        {
                            Enqueue(relatedDocument);
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
                                Enqueue(newRelatedDocument);
                            }
                        }
                    }

                    break;
                }
            case ProjectChangeKind.ProjectRemoved:
                {
                    // ignore
                    break;
                }

            default:
                throw new InvalidOperationException(SR.FormatUnknown_ProjectChangeKind(args.Kind));
        }
    }

    private void ReportError(Exception ex)
    {
        if (_projectManager is null)
        {
            return;
        }

        _dispatcher
            .RunOnDispatcherThreadAsync(
                () => _projectManager.ReportError(ex),
                CancellationToken.None)
            .Forget();
    }
}
