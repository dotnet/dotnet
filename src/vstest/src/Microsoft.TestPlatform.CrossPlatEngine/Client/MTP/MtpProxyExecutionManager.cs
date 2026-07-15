// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;

using Jsonite;

using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.MTP;

/// <summary>
/// An <see cref="IProxyExecutionManager"/> that runs tests by driving a Microsoft.Testing.Platform
/// (MTP) application over the MTP JSON-RPC protocol instead of the vstest testhost protocol.
/// </summary>
internal sealed class MtpProxyExecutionManager : IProxyExecutionManager, IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    /// <summary>
    /// Optional data collection manager (e.g. code coverage). When present, it is started before the
    /// run to obtain profiler environment variables that are injected into the MTP application, is
    /// notified of the MTP application's process id, and is asked for its attachments (such as the
    /// .coverage file) once the run completes.
    /// </summary>
    private readonly IProxyDataCollectionManager? _dataCollectionManager;

    private readonly DataCollectionRunEventsHandler? _dataCollectionEventsHandler;

    /// <summary>
    /// Forwards per-test-case started/ended notifications (observed from the MTP application) to the
    /// out-of-process datacollector. Created only when a data collector asks for test-case-level
    /// events (e.g. Blame); left null for code coverage or when data collection is off.
    /// </summary>
    private MtpDataCollectionForwarder? _testCaseEventForwarder;

    private bool _isInitialized;

    public MtpProxyExecutionManager()
    {
    }

    public MtpProxyExecutionManager(IProxyDataCollectionManager dataCollectionManager)
    {
        _dataCollectionManager = dataCollectionManager;
        _dataCollectionEventsHandler = new DataCollectionRunEventsHandler();
    }

    public bool IsInitialized => _isInitialized;

    /// <summary>
    /// Environment variables to inject into the MTP application process. Used to pass code coverage
    /// profiler settings supplied by the data collector.
    /// </summary>
    public IDictionary<string, string?>? EnvironmentVariables { get; set; }

    public void Initialize(bool skipDefaultAdapters) => _isInitialized = true;

    public void InitializeTestRun(TestRunCriteria testRunCriteria, IInternalTestRunEventsHandler eventHandler)
        => Initialize(skipDefaultAdapters: true);

    public int StartTestRun(TestRunCriteria testRunCriteria, IInternalTestRunEventsHandler eventHandler)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var aggregate = new RunAggregate();
        var attachments = new List<AttachmentSet>();
        var executorUris = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var invokedDataCollectors = new List<InvokedDataCollector>();
        int processId = 0;
        bool aborted = false;

        // Inject environment variables declared in the runsettings RunConfiguration/EnvironmentVariables
        // into the MTP application launch. On the classic path ProxyOperationManager reads these from the
        // runsettings and passes them to the testhost process; the MTP application is its own host, so we
        // apply them here. Done before BeforeTestRun so datacollector-provided profiler variables merge on
        // top and win on collision (matching the classic ordering).
        ApplyRunSettingsEnvironmentVariables(testRunCriteria.TestRunSettings);

        BeforeTestRun(eventHandler);

        foreach (var (source, tests) in BuildWork(testRunCriteria))
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                break;
            }

            try
            {
                processId = RunSource(source, tests, eventHandler, aggregate, attachments, executorUris);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                EqtTrace.Error("MtpProxyExecutionManager.StartTestRun: run failed for '{0}': {1}", source, ex);
                eventHandler.HandleLogMessage(ObjectModel.Logging.TestMessageLevel.Error, $"Microsoft.Testing.Platform run failed for '{source}': {ex.Message}");
                aborted = true;
            }
        }

        AfterTestRun(attachments, invokedDataCollectors);

        TestRunStatistics finalStats = aggregate.Snapshot();
        var completeArgs = new TestRunCompleteEventArgs(
            finalStats,
            _cancellationTokenSource.IsCancellationRequested,
            aborted,
            null,
            new Collection<AttachmentSet>(attachments),
            stopwatch.Elapsed);

        foreach (InvokedDataCollector collector in invokedDataCollectors)
        {
            completeArgs.InvokedDataCollectors.Add(collector);
        }

        eventHandler.HandleTestRunComplete(completeArgs, null, attachments, executorUris.ToList());
        return processId;
    }

    /// <summary>
    /// Starts the data collector (if any) before the run and injects the profiler environment
    /// variables it produces into the MTP application launch. Any messages the data collector logged
    /// during startup are forwarded to the run events handler.
    /// </summary>
    private void BeforeTestRun(IInternalTestRunEventsHandler eventHandler)
    {
        if (_dataCollectionManager is null)
        {
            return;
        }

        _dataCollectionManager.Initialize();

        DataCollectionParameters parameters;
        try
        {
            parameters = _dataCollectionManager.BeforeTestRunStart(
                resetDataCollectors: true,
                isRunStartingNow: true,
                runEventsHandler: _dataCollectionEventsHandler!);
        }
        catch (Exception)
        {
            _dataCollectionManager.AfterTestRunEnd(isCanceled: true, runEventsHandler: _dataCollectionEventsHandler!);
            throw;
        }

        if (parameters?.EnvironmentVariables is { } dataCollectionEnvironmentVariables)
        {
            EnvironmentVariables ??= new Dictionary<string, string?>();
            foreach (KeyValuePair<string, string?> variable in dataCollectionEnvironmentVariables)
            {
                EnvironmentVariables[variable.Key] = variable.Value;
            }
        }

        // If a data collector needs per-test-case events (e.g. Blame tracks the currently running
        // test to attribute crashes), it opens a socket and returns its port. In the classic path
        // testhost connects to it; under MTP there is no testhost, so we connect from here and
        // forward the started/ended notifications we observe from the MTP application. A port of 0
        // means no collector needs these events (e.g. code coverage) and we do nothing.
        if (parameters?.DataCollectionEventsPort > 0)
        {
            _testCaseEventForwarder = new MtpDataCollectionForwarder();
            if (!_testCaseEventForwarder.Connect(parameters.DataCollectionEventsPort))
            {
                eventHandler.HandleLogMessage(
                    ObjectModel.Logging.TestMessageLevel.Warning,
                    "Could not connect to the data collector for per-test-case events; collectors that rely on them (such as Blame) may not function for this Microsoft.Testing.Platform run.");
                _testCaseEventForwarder.Dispose();
                _testCaseEventForwarder = null;
            }
        }

        // Surface any messages the data collector produced while starting up.
        foreach (Tuple<ObjectModel.Logging.TestMessageLevel, string?> message in _dataCollectionEventsHandler!.Messages)
        {
            eventHandler.HandleLogMessage(message.Item1, message.Item2);
        }

        _dataCollectionEventsHandler.Messages.Clear();
    }

    /// <summary>
    /// Ends the data collector (if any) after the run and collects its attachments (such as the
    /// .coverage file) and the list of invoked data collectors.
    /// </summary>
    private void AfterTestRun(List<AttachmentSet> attachments, List<InvokedDataCollector> invokedDataCollectors)
    {
        if (_dataCollectionManager is null)
        {
            return;
        }

        // Signal end-of-stream on the test-case event channel so the datacollector's wait on it
        // completes promptly instead of blocking until the connection timeout (~90s).
        _testCaseEventForwarder?.NotifySessionEnd();

        DataCollectionResult result = _dataCollectionManager.AfterTestRunEnd(
            _cancellationTokenSource.IsCancellationRequested,
            _dataCollectionEventsHandler!);

        if (result.Attachments is { Count: > 0 })
        {
            lock (attachments)
            {
                attachments.AddRange(result.Attachments);
            }
        }

        if (result.InvokedDataCollectors is { Count: > 0 })
        {
            invokedDataCollectors.AddRange(result.InvokedDataCollectors);
        }
    }

    public void Cancel(IInternalTestRunEventsHandler eventHandler) => _cancellationTokenSource.Cancel();

    public void Abort(IInternalTestRunEventsHandler eventHandler) => _cancellationTokenSource.Cancel();

    public void Close() => _cancellationTokenSource.Cancel();

    public void Dispose()
    {
        try
        {
            _testCaseEventForwarder?.Dispose();
        }
        catch
        {
            // ignore
        }

        try
        {
            _dataCollectionManager?.Dispose();
        }
        catch
        {
            // ignore
        }

        try
        {
            _cancellationTokenSource.Dispose();
        }
        catch
        {
            // ignore
        }
    }

    private int RunSource(
        string source,
        List<TestCase>? tests,
        IInternalTestRunEventsHandler eventHandler,
        RunAggregate aggregate,
        List<AttachmentSet> attachments,
        HashSet<string> executorUris)
    {
        var completed = new ManualResetEventSlim(false);

        using var connection = new MtpServerConnection();
        connection.LogReceived += (level, message) => eventHandler.HandleLogMessage(MtpClientHelpers.MapLevel(level), message);
        connection.TestNodesUpdated += parameters =>
        {
            if (MtpClientHelpers.IsCompletionSentinel(parameters))
            {
                completed.Set();
                return;
            }

            var results = new List<TestResult>();
            foreach (JsonObject node in MtpClientHelpers.EnumerateNodes(parameters))
            {
                if (!MtpTestNodeConverter.IsActionNode(node))
                {
                    continue;
                }

                string? state = MtpTestNodeConverter.GetExecutionState(node);

                if (EqtTrace.IsVerboseEnabled)
                {
                    EqtTrace.Verbose("MtpProxyExecutionManager: node update uid={0} state={1}", MtpJson.GetString(node, MtpConstants.Uid), state ?? "(none)");
                }

                // A test entering the in-progress state is our "test started" signal. Forwarding it
                // lets per-test-case collectors (e.g. Blame) know which test is in flight, which is
                // what makes crash attribution work when the test never reaches a terminal state.
                if (_testCaseEventForwarder is { } forwarder && state == MtpConstants.StateInProgress)
                {
                    forwarder.NotifyTestCaseStart(MtpTestNodeConverter.ToTestCase(node, source));
                    continue;
                }

                if (!MtpTestNodeConverter.IsTerminalState(state))
                {
                    continue;
                }

                TestResult result = MtpTestNodeConverter.ToTestResult(node, source);
                _testCaseEventForwarder?.NotifyTestCaseEnd(result);
                results.Add(result);
            }

            if (results.Count == 0)
            {
                return;
            }

            TestRunStatistics snapshot;
            lock (aggregate.Lock)
            {
                foreach (TestResult result in results)
                {
                    aggregate.Add(result);
                    if (result.TestCase.ExecutorUri is { } uri)
                    {
                        executorUris.Add(uri.ToString());
                    }
                }

                snapshot = aggregate.Snapshot();
            }

            eventHandler.HandleTestRunStatsChange(new TestRunChangedEventArgs(snapshot, results, null));
        };

        connection.Start(source, EnvironmentVariables, MtpClientHelpers.GetConnectionTimeout());

        // Let the data collector (e.g. code coverage) know the process it should track. The profiler
        // env vars were already injected via EnvironmentVariables above.
        _dataCollectionManager?.TestHostLaunched(connection.ProcessId);

        connection.InvokeAsync(MtpConstants.InitializeMethod, MtpClientHelpers.InitializeParameters(), _cancellationTokenSource.Token).GetAwaiter().GetResult();

        var runId = Guid.NewGuid();
        var runParameters = new Dictionary<string, object?> { [MtpConstants.RunIdParameter] = runId.ToString() };
        if (tests is { Count: > 0 })
        {
            runParameters[MtpConstants.TestsParameter] = BuildTestsFilter(tests);
        }

        var runTask = connection.InvokeAsync(MtpConstants.RunTestsMethod, runParameters, _cancellationTokenSource.Token);
        object? response = runTask.GetAwaiter().GetResult();
        completed.Wait(TimeSpan.FromSeconds(3));

        CollectAttachments(response, attachments);
        connection.SendNotification(MtpConstants.ExitMethod, null);
        return connection.ProcessId;
    }

    private static IEnumerable<(string Source, List<TestCase>? Tests)> BuildWork(TestRunCriteria criteria)
    {
        if (criteria.HasSpecificTests && criteria.Tests is not null)
        {
            return criteria.Tests
                .GroupBy(test => test.Source)
                .Select(group => (group.Key, (List<TestCase>?)group.ToList()));
        }

        return (criteria.Sources ?? Enumerable.Empty<string>())
            .Select(source => (source, (List<TestCase>?)null));
    }

    /// <summary>
    /// Reads the environment variables declared in the runsettings
    /// <c>RunConfiguration/EnvironmentVariables</c> and merges them into <see cref="EnvironmentVariables"/>
    /// so they are applied to the MTP application launch.
    /// </summary>
    private void ApplyRunSettingsEnvironmentVariables(string? runSettings)
    {
        Dictionary<string, string?>? runSettingsEnvironmentVariables = InferRunSettingsHelper.GetEnvironmentVariables(runSettings);
        if (runSettingsEnvironmentVariables is null || runSettingsEnvironmentVariables.Count == 0)
        {
            return;
        }

        EnvironmentVariables ??= new Dictionary<string, string?>(
            Environment.OSVersion.Platform == PlatformID.Win32NT ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
        foreach (KeyValuePair<string, string?> variable in runSettingsEnvironmentVariables)
        {
            EnvironmentVariables[variable.Key] = variable.Value;
        }
    }

    private static List<Dictionary<string, object?>> BuildTestsFilter(List<TestCase> tests)
        => tests
            .Select(test => new Dictionary<string, object?>
            {
                [MtpConstants.Uid] = test.GetPropertyValue(MtpTestNodeConverter.MtpUidProperty, test.FullyQualifiedName),
                [MtpConstants.DisplayName] = test.DisplayName,
            })
            .ToList();

    private static void CollectAttachments(object? response, List<AttachmentSet> attachments)
    {
        if (MtpJson.AsObject(response) is not JsonObject responseObject
            || !responseObject.TryGetValue(MtpConstants.AttachmentsProperty, out object? attachmentsValue)
            || attachmentsValue is not JsonArray attachmentArray)
        {
            return;
        }

        var set = new AttachmentSet(new Uri(MtpConstants.DefaultExecutorUri), "Microsoft.Testing.Platform");
        foreach (object? attachmentObject in attachmentArray)
        {
            if (attachmentObject is not JsonObject attachment)
            {
                continue;
            }

            string? path = GetStringProperty(attachment, MtpConstants.AttachmentUriProperty)
                ?? GetStringProperty(attachment, MtpConstants.AttachmentPathProperty);
            if (string.IsNullOrEmpty(path))
            {
                continue;
            }

            if (!TryCreateFileUri(path!, out Uri? fileUri))
            {
                continue;
            }

            string display = GetStringProperty(attachment, MtpConstants.DisplayName) ?? Path.GetFileName(path!);
            set.Attachments.Add(new UriDataAttachment(fileUri!, display));
        }

        if (set.Attachments.Count > 0)
        {
            lock (attachments)
            {
                attachments.Add(set);
            }
        }
    }

    private static string? GetStringProperty(JsonObject element, string name)
        => element.TryGetValue(name, out object? value) && value is string text ? text : null;

    private static bool TryCreateFileUri(string path, out Uri? uri)
    {
        try
        {
            uri = Uri.TryCreate(path, UriKind.Absolute, out Uri? absolute) && absolute.IsFile
                ? absolute
                : new Uri(Path.GetFullPath(path));
            return true;
        }
        catch (Exception ex) when (ex is ArgumentException or UriFormatException or NotSupportedException or PathTooLongException)
        {
            uri = null;
            return false;
        }
    }

    private sealed class RunAggregate
    {
        public object Lock { get; } = new();

        private readonly Dictionary<TestOutcome, long> _byOutcome = new();
        private long _executed;

        public void Add(TestResult result)
        {
            _byOutcome.TryGetValue(result.Outcome, out long count);
            _byOutcome[result.Outcome] = count + 1;
            _executed++;
        }

        public TestRunStatistics Snapshot()
            => new(_executed, new Dictionary<TestOutcome, long>(_byOutcome));
    }
}
