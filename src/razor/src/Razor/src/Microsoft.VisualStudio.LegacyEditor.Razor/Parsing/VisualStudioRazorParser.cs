// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Logging;

using Microsoft.Extensions.Internal;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Razor.Extensions;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Threading;
using static Microsoft.VisualStudio.LegacyEditor.Razor.Parsing.BackgroundParser;
using ITextBuffer = Microsoft.VisualStudio.Text.ITextBuffer;
using Timer = System.Threading.Timer;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Parsing;

internal class VisualStudioRazorParser : IVisualStudioRazorParser, IDisposable
{
    // Internal for testing.
    internal TimeSpan _idleDelay = TimeSpan.FromSeconds(3);
    internal Timer? _idleTimer;
    internal BackgroundParser? _parser;
    internal ChangeReference? _latestChangeReference;
    internal RazorSyntaxTreePartialParser? _partialParser;

    private readonly object _idleLock = new();
    private readonly object _updateStateLock = new();
    private readonly ICompletionBroker _completionBroker;
    private readonly IVisualStudioDocumentTracker _documentTracker;
    private readonly JoinableTaskContext _joinableTaskContext;
    private readonly IProjectEngineFactoryProvider _projectEngineFactoryProvider;
    private readonly ILogger _logger;
    private readonly List<CodeDocumentRequest> _codeDocumentRequests;
    private readonly TaskScheduler _uiThreadScheduler;
    private RazorProjectEngine? _projectEngine;
    private RazorCodeDocument? _codeDocument;
    private ITextSnapshot? _snapshot;
    private bool _disposed;
    private ITextSnapshot? _latestParsedSnapshot;

    public event EventHandler<DocumentStructureChangedEventArgs>? DocumentStructureChanged;

    public VisualStudioRazorParser(
        IVisualStudioDocumentTracker documentTracker,
        IProjectEngineFactoryProvider projectEngineFactoryProvider,
        ICompletionBroker completionBroker,
        ILoggerFactory loggerFactory,
        JoinableTaskContext joinableTaskContext)
    {
        _joinableTaskContext = joinableTaskContext;
        _projectEngineFactoryProvider = projectEngineFactoryProvider;
        _logger = loggerFactory.GetOrCreateLogger<VisualStudioRazorParser>();
        _completionBroker = completionBroker;
        _documentTracker = documentTracker;
        _codeDocumentRequests = new List<CodeDocumentRequest>();

        _documentTracker.ContextChanged += DocumentTracker_ContextChanged;

        _joinableTaskContext.AssertUIThread();
        _uiThreadScheduler = TaskScheduler.FromCurrentSynchronizationContext();
    }

    public string FilePath => _documentTracker.FilePath;

    public RazorCodeDocument? CodeDocument => _codeDocument;

    public ITextSnapshot? Snapshot => _snapshot;

    public ITextBuffer TextBuffer => _documentTracker.TextBuffer;

    public bool HasPendingChanges => _latestChangeReference is not null;

    // Used in unit tests to ensure we can be notified when idle starts.
    internal ManualResetEventSlim? NotifyUIIdleStart { get; set; }

    // Used in unit tests to ensure we can block background idle work.
    internal ManualResetEventSlim? BlockBackgroundIdleWork { get; set; }

    public Task<RazorCodeDocument?> GetLatestCodeDocumentAsync(ITextSnapshot atOrNewerSnapshot, CancellationToken cancellationToken = default)
    {
        if (atOrNewerSnapshot is null)
        {
            throw new ArgumentNullException(nameof(atOrNewerSnapshot));
        }

        lock (_updateStateLock)
        {
            if (_disposed ||
                _latestParsedSnapshot is not null && atOrNewerSnapshot.Version.VersionNumber <= _latestParsedSnapshot.Version.VersionNumber)
            {
                return Task.FromResult(CodeDocument);
            }

            CodeDocumentRequest? request = null;
            for (var i = _codeDocumentRequests.Count - 1; i >= 0; i--)
            {
                if (_codeDocumentRequests[i].Snapshot == atOrNewerSnapshot)
                {
                    request = _codeDocumentRequests[i];
                    break;
                }
            }

            if (request is null)
            {
                request = new CodeDocumentRequest(atOrNewerSnapshot, cancellationToken);
                _codeDocumentRequests.Add(request);
            }

            // Null suppression is required here to convert from Task<RazorCodeDocument> to Task<RazorCodeDocument?>
            // The task itself can never be null, so this is safe
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
            return request.Task!;
#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks
        }
    }

    // WebTools depends on this method. Do not remove until old editor is phased out
    public void QueueReparse()
    {
        // Can be called from any thread

        try
        {
            if (_joinableTaskContext.IsOnMainThread)
            {
                ReparseOnUIThread();
            }
            else
            {
                _ = Task.Factory.StartNew(
                    () => ReparseOnUIThread(), CancellationToken.None, TaskCreationOptions.None, _uiThreadScheduler);
            }
        }
        catch (Exception ex)
        {
            Debug.Fail($"""
                VisualStudioRazorParser.QueueReparse threw exception:
                {ex.Message}
                Stack trace:
                {ex.StackTrace}
                """);
        }
    }

    public void Dispose()
    {
        _joinableTaskContext.AssertUIThread();

        StopParser();

        _documentTracker.ContextChanged -= DocumentTracker_ContextChanged;

        StopIdleTimer();

        lock (_updateStateLock)
        {
            _disposed = true;
            foreach (var request in _codeDocumentRequests)
            {
                request.Cancel();
            }
        }
    }

    // Internal for testing
    internal void DocumentTracker_ContextChanged(object sender, ContextChangeEventArgs args)
    {
        _joinableTaskContext.AssertUIThread();

        if (!TryReinitializeParser())
        {
            return;
        }

        // We have a new parser, force a reparse to generate new document information. Note that this
        // only blocks until the reparse change has been queued.
        ReparseOnUIThread();
    }

    // Internal for testing
    internal bool TryReinitializeParser()
    {
        _joinableTaskContext.AssertUIThread();

        StopParser();

        if (!_documentTracker.IsSupportedProject)
        {
            // Tracker is either starting up, tearing down or wrongfully instantiated.
            // Either way, the tracker can't act on its associated project, neither can we.
            return false;
        }

        StartParser();

        return true;
    }

    // Internal for testing
    [MemberNotNull(nameof(_parser))]
    internal void StartParser()
    {
        _joinableTaskContext.AssertUIThread();

        // Make sure any tests use the real thing or a good mock. These tests can cause failures
        // that are hard to understand when this throws.
        Debug.Assert(_documentTracker.IsSupportedProject);

        var projectSnapshot = _documentTracker.ProjectSnapshot.AssumeNotNull();

        _projectEngine = _projectEngineFactoryProvider.Create(
            projectSnapshot.Configuration,
            rootDirectoryPath: Path.GetDirectoryName(projectSnapshot.FilePath).AssumeNotNull(),
            ConfigureProjectEngine);

        Debug.Assert(_projectEngine.Engine is not null);
        Debug.Assert(_projectEngine.FileSystem is not null);

        // We might not have a document snapshot in the case of an ephemeral project.
        // If we don't have a document then infer the FileKind from the FilePath.
        var fileKind = projectSnapshot.GetDocument(_documentTracker.FilePath)?.FileKind ?? FileKinds.GetFileKindFromFilePath(_documentTracker.FilePath);

        var projectDirectory = Path.GetDirectoryName(_documentTracker.ProjectPath);
        _parser = new BackgroundParser(_projectEngine, FilePath, projectDirectory, fileKind);
        _parser.ResultsReady += OnResultsReady;
        _parser.Start();

        TextBuffer.Changed += TextBuffer_OnChanged;
    }

    // Internal for testing
    internal void StopParser()
    {
        _joinableTaskContext.AssertUIThread();

        if (_parser is not null)
        {
            // Detach from the text buffer until we have a new parser to handle changes.
            TextBuffer.Changed -= TextBuffer_OnChanged;

            _parser.ResultsReady -= OnResultsReady;
            _parser.Dispose();
            _parser = null;
        }
    }

    // Internal for testing
    internal void StartIdleTimer()
    {
        _joinableTaskContext.AssertUIThread();

        lock (_idleLock)
        {
            // Timer will fire after a fixed delay, but only once.
            _idleTimer ??= NonCapturingTimer.Create(state => ((VisualStudioRazorParser)state).Timer_Tick(), this, _idleDelay, Timeout.InfiniteTimeSpan);
        }
    }

    // Internal for testing
    internal void StopIdleTimer()
    {
        // Can be called from any thread.

        lock (_idleLock)
        {
            if (_idleTimer is not null)
            {
                _idleTimer.Dispose();
                _idleTimer = null;
            }
        }
    }

    private void TextBuffer_OnChanged(object sender, TextContentChangedEventArgs args)
    {
        _joinableTaskContext.AssertUIThread();

        if (args.Changes.Count > 0)
        {
            // Idle timers are used to track provisional changes. Provisional changes only last for a single text change. After that normal
            // partial parsing rules apply (stop the timer).
            StopIdleTimer();
        }

        var snapshot = args.After;
        if (!args.TextChangeOccurred(out var changeInformation))
        {
            // Ensure we get a parse for latest snapshot.
            QueueChange(change: null, snapshot);
            return;
        }

        var change = new SourceChange(changeInformation.firstChange.OldPosition, changeInformation.oldText.Length, changeInformation.newText);
        var result = PartialParseResultInternal.Rejected;
        RazorSyntaxTree? partialParseSyntaxTree = null;
        using (_parser.AssumeNotNull().SynchronizeMainThreadState())
        {
            // Check if we can partial-parse
            if (_partialParser is not null && _parser.IsIdle)
            {
                (result, partialParseSyntaxTree) = _partialParser.Parse(change);
            }
        }

        // If partial parsing failed or there were outstanding parser tasks, start a full reparse
        if ((result & PartialParseResultInternal.Rejected) == PartialParseResultInternal.Rejected)
        {
            QueueChange(change, snapshot);
        }
        else
        {
            var currentCodeDocument = CodeDocument;
            if (currentCodeDocument is null)
            {
                // CodeDocument should have been initialized but was not.
                Debug.Fail($"{nameof(CodeDocument)} should have been initialized but was not.");
                return;
            }

            var codeDocument = RazorCodeDocument.Create(
                currentCodeDocument.Source,
                currentCodeDocument.Imports,
                currentCodeDocument.ParserOptions,
                currentCodeDocument.CodeGenerationOptions);

            foreach (var item in currentCodeDocument.Items)
            {
                codeDocument.Items[item.Key] = item.Value;
            }

            codeDocument.SetSyntaxTree(partialParseSyntaxTree);
            TryUpdateLatestParsedSyntaxTreeSnapshot(codeDocument, snapshot);
        }

        if ((result & PartialParseResultInternal.Provisional) == PartialParseResultInternal.Provisional)
        {
            StartIdleTimer();
        }
    }

    // Internal for testing
    internal void OnIdle()
    {
        _joinableTaskContext.AssertUIThread();

        if (_disposed)
        {
            return;
        }

        OnNotifyUIIdle();

        foreach (var textView in _documentTracker.TextViews)
        {
            if (_completionBroker.IsCompletionActive(textView))
            {
                // Completion list is still active, need to re-start timer.
                StartIdleTimer();
                return;
            }
        }

        ReparseOnUIThread();
    }

    // Internal for testing
    internal void ReparseOnUIThread()
    {
        _joinableTaskContext.AssertUIThread();

        if (_disposed)
        {
            return;
        }

        var snapshot = TextBuffer.CurrentSnapshot;
        QueueChange(change: null, snapshot);
    }

    private void QueueChange(SourceChange? change, ITextSnapshot snapshot)
    {
        _joinableTaskContext.AssertUIThread();

        // _parser can be null if we're in the midst of rebuilding the internal parser (TagHelper refresh/solution teardown)
        _latestChangeReference = _parser?.QueueChange(change, snapshot);
    }

    private void OnNotifyUIIdle()
    {
        NotifyUIIdleStart?.Set();
    }

    private void OnStartingBackgroundIdleWork()
    {
        BlockBackgroundIdleWork?.Wait();
    }

    private void Timer_Tick()
    {
        try
        {
            OnStartingBackgroundIdleWork();
            StopIdleTimer();

            // We need to get back to the UI thread to properly check if a completion is active.
            _ = Task.Factory.StartNew(OnIdle_QueueOnUIThreadAsync, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
        }
        catch (Exception ex)
        {
            // This is something totally unexpected, let's just send it over to the workspace.
            _logger.LogError(ex);
        }

        async Task OnIdle_QueueOnUIThreadAsync()
        {
            await _joinableTaskContext.Factory.SwitchToMainThreadAsync();
            OnIdle();
        }
    }

    // Internal for testing
    internal void OnResultsReady(object sender, BackgroundParserResultsReadyEventArgs args)
    {
        _ = OnResultsReadyAsync(args, CancellationToken.None);
    }

    private async Task OnResultsReadyAsync(BackgroundParserResultsReadyEventArgs args, CancellationToken cancellationToken)
    {
        try
        {
            UpdateParserState(args.CodeDocument, args.ChangeReference.Snapshot);

            // Jump back to UI thread to notify structure changes.
            await _joinableTaskContext.Factory.SwitchToMainThreadAsync(cancellationToken);
            OnDocumentStructureChanged(args);
        }
        catch (Exception ex)
        {
            Debug.Fail($"""
                VisualStudioRazorParser.OnResultsReady threw exception:
                {ex.Message}
                Stack trace:
                {ex.StackTrace}
                """);
        }
    }

    // Internal for testing
    internal void OnDocumentStructureChanged(object state)
    {
        _joinableTaskContext.AssertUIThread();

        if (_disposed)
        {
            return;
        }

        var backgroundParserArgs = (BackgroundParserResultsReadyEventArgs)state;
        if (_latestChangeReference is null || // extra hardening
            _latestChangeReference != backgroundParserArgs.ChangeReference)
        {
            // In the middle of parsing a newer change or about to parse a newer change.
            return;
        }

        if (backgroundParserArgs.ChangeReference.Snapshot != TextBuffer.CurrentSnapshot)
        {
            // Changes have impacted the snapshot after our we recorded our last change reference.
            // This can happen for a multitude of reasons, usually because of a user auto-completing
            // C# statements (causes multiple edits in quick succession). This ensures that our latest
            // parse corresponds to the current snapshot.
            ReparseOnUIThread();
            return;
        }

        _latestChangeReference = null;

        var documentStructureChangedArgs = new DocumentStructureChangedEventArgs(
            backgroundParserArgs.ChangeReference.Change,
            backgroundParserArgs.ChangeReference.Snapshot,
            backgroundParserArgs.CodeDocument);
        DocumentStructureChanged?.Invoke(this, documentStructureChangedArgs);
    }

    private void ConfigureProjectEngine(RazorProjectEngineBuilder builder)
    {
        var projectSnapshot = _documentTracker.ProjectSnapshot;
        if (projectSnapshot != null)
        {
            builder.SetCSharpLanguageVersion(projectSnapshot.CSharpLanguageVersion);
        }

        builder.SetRootNamespace(projectSnapshot?.RootNamespace);

        var settings = _documentTracker.EditorSettings;

        builder.ConfigureCodeGenerationOptions(builder =>
        {
            builder.IndentSize = settings.IndentSize;
            builder.IndentWithTabs = settings.IndentWithTabs;
            builder.RemapLinePragmaPathsOnWindows = true;
        });

        builder.Features.Add(new VisualStudioTagHelperFeature(_documentTracker.TagHelpers));

        builder.ConfigureParserOptions(ConfigureParserOptions);
    }

    private void UpdateParserState(RazorCodeDocument codeDocument, ITextSnapshot snapshot)
    {
        lock (_updateStateLock)
        {
            if (_snapshot is not null && snapshot.Version.VersionNumber < _snapshot.Version.VersionNumber)
            {
                // Changes flowed out of order due to the slight race condition at the beginning of this method. Our current
                // CodeDocument and Snapshot are newer then the ones that made it into the lock.
                return;
            }

            _codeDocument = codeDocument;
            _snapshot = snapshot;
            _partialParser = new RazorSyntaxTreePartialParser(_codeDocument.GetSyntaxTree());
            TryUpdateLatestParsedSyntaxTreeSnapshot(_codeDocument, _snapshot);
        }
    }

    private void TryUpdateLatestParsedSyntaxTreeSnapshot(RazorCodeDocument codeDocument, ITextSnapshot snapshot)
    {
        if (snapshot is null)
        {
            throw new ArgumentNullException(nameof(snapshot));
        }

        lock (_updateStateLock)
        {
            if (_latestParsedSnapshot is null ||
                _latestParsedSnapshot.Version.VersionNumber < snapshot.Version.VersionNumber)
            {
                _latestParsedSnapshot = snapshot;

                CompleteCodeDocumentRequestsForSnapshot(codeDocument, snapshot);
            }
        }
    }

    private void CompleteCodeDocumentRequestsForSnapshot(RazorCodeDocument codeDocument, ITextSnapshot snapshot)
    {
        lock (_updateStateLock)
        {
            if (_codeDocumentRequests.Count == 0)
            {
                return;
            }

            using var matchingRequests = new PooledArrayBuilder<CodeDocumentRequest>();
            for (var i = _codeDocumentRequests.Count - 1; i >= 0; i--)
            {
                var request = _codeDocumentRequests[i];
                if (request.Snapshot.Version.VersionNumber <= snapshot.Version.VersionNumber)
                {
                    // This change was for a newer snapshot, we can complete the TCS.
                    matchingRequests.Add(request);
                    _codeDocumentRequests.RemoveAt(i);
                }
            }

            // The matching requests are in reverse order so we need to invoke them from the back to front.
            for (var i = matchingRequests.Count - 1; i >= 0; i--)
            {
                // At this point it's possible these requests have been cancelled, if that's the case Complete noops.
                matchingRequests[i].Complete(codeDocument);
            }
        }
    }

    private class VisualStudioTagHelperFeature : RazorEngineFeatureBase, ITagHelperFeature
    {
        private readonly IReadOnlyList<TagHelperDescriptor>? _tagHelpers;

        public VisualStudioTagHelperFeature(IReadOnlyList<TagHelperDescriptor>? tagHelpers)
        {
            _tagHelpers = tagHelpers;
        }

        public IReadOnlyList<TagHelperDescriptor>? GetDescriptors()
        {
            return _tagHelpers;
        }
    }

    // Internal for testing
    internal static void ConfigureParserOptions(RazorParserOptions.Builder builder)
    {
        builder.EnableSpanEditHandlers = true;
        builder.UseRoslynTokenizer = false;
    }

    // Internal for testing
    internal class CodeDocumentRequest
    {
        private readonly object _completionLock = new();
        private readonly TaskCompletionSource<RazorCodeDocument> _taskCompletionSource;
        private readonly CancellationTokenRegistration _cancellationTokenRegistration;
        private bool _done;

        public CodeDocumentRequest(ITextSnapshot snapshot, CancellationToken cancellationToken)
        {
            if (snapshot is null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            Snapshot = snapshot;
            _taskCompletionSource = new TaskCompletionSource<RazorCodeDocument>(TaskCreationOptions.RunContinuationsAsynchronously);
            _cancellationTokenRegistration = cancellationToken.Register(Cancel);
            Task = _taskCompletionSource.Task;

            if (cancellationToken.IsCancellationRequested)
            {
                // If the token was already cancelled we need to bail.
                Cancel();
            }
        }

        public ITextSnapshot Snapshot { get; }

        public Task<RazorCodeDocument> Task { get; }

        public void Complete(RazorCodeDocument codeDocument)
        {
            if (codeDocument is null)
            {
                throw new ArgumentNullException(nameof(codeDocument));
            }

            lock (_completionLock)
            {
                if (_done)
                {
                    // Request was already cancelled.
                    return;
                }

                _done = true;
            }

            _cancellationTokenRegistration.Dispose();
            _taskCompletionSource.SetResult(codeDocument);
        }

        public void Cancel()
        {
            lock (_completionLock)
            {
                if (_done)
                {
                    return;
                }

                _done = true;
            }

            _taskCompletionSource.TrySetCanceled();
            _cancellationTokenRegistration.Dispose();
        }
    }
}
