// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using NuGet.PackageManagement.VisualStudio;
using NuGet.ProjectManagement.Projects;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Common;
using NuGet.VisualStudio.Telemetry;
using IAsyncServiceProvider = Microsoft.VisualStudio.Shell.IAsyncServiceProvider;
using Task = System.Threading.Tasks.Task;

namespace NuGet.SolutionRestoreManager
{
    /// <summary>
    /// Solution restore job scheduler.
    /// </summary>
    [Export(typeof(ISolutionRestoreWorker))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal sealed class SolutionRestoreWorker : SolutionEventsListener, ISolutionRestoreWorker, IDisposable
    {
        private const int IdleTimeoutMs = 400;
        private const int RequestQueueLimit = 150;
        private const int PromoteAttemptsLimit = 150;
        private const int DelaySolutionLoadRetry = 100;
        private const int MaxIdleWaitTimeMs = 30000;
        private static TimeSpan BulkRestoreCoordinationTimeout = new(hours: 0, minutes: 5, seconds: 0);

        private readonly object _lockPendingRequestsObj = new object();

        private readonly IAsyncServiceProvider _asyncServiceProvider;
        private readonly Lazy<IVsSolutionManager> _solutionManager;
        private readonly Lazy<INuGetLockService> _lockService;
        private readonly Lazy<Common.ILogger> _logger;
        private readonly Lazy<IVulnerabilitiesNotificationService> _vulnerabilitiesFoundService;
        private readonly AsyncLazy<IComponentModel> _componentModel;

        private EnvDTE.SolutionEvents _solutionEvents;
        private CancellationTokenSource _workerCts;
        private AsyncLazy<bool> _backgroundJobRunner;
        private Lazy<BlockingCollection<SolutionRestoreRequest>> _pendingRequests;
        private BackgroundRestoreOperation _pendingRestore;
        private Task<bool> _activeRestoreTask;
        private int _initialized;
        private bool _isFirstRestore = true;
        private DateTimeOffset _lastRestoreCompletedTime;
        private RestoreOperationSource _lastRestoreOperationSource;

        private IVsSolution _vsSolution;

        private SolutionRestoreJobContext _restoreJobContext;

        private readonly JoinableTaskCollection _joinableCollection;
        private readonly AsyncManualResetEvent _solutionLoadedEvent;
        private readonly AsyncManualResetEvent _isCompleteEvent;

        private IVsSolutionManager SolutionManager => _solutionManager.Value;

        private Common.ILogger Logger => _logger.Value;

        private Lazy<INuGetErrorList> _errorList;
        private readonly Lazy<IOutputConsoleProvider> _outputConsoleProvider;

        private readonly Lazy<INuGetFeatureFlagService> _nugetFeatureFlagService;

        public Task<bool> CurrentRestoreOperation => _activeRestoreTask;

        /// <summary>
        /// True if a restore is currently running.
        /// </summary>
        public bool IsBusy => !_activeRestoreTask.IsCompleted;

        /// <summary>
        /// True if any operation is running, pending, or waiting.
        /// </summary>
        public bool IsRunning => !_isCompleteEvent.IsSet;

        public JoinableTaskFactory JoinableTaskFactory { get; }

        [ImportingConstructor]
        public SolutionRestoreWorker(
            Lazy<IVsSolutionManager> solutionManager,
            Lazy<INuGetLockService> lockService,
            [Import("VisualStudioActivityLogger")]
            Lazy<Common.ILogger> logger,
            Lazy<INuGetErrorList> errorList,
            Lazy<IOutputConsoleProvider> outputConsoleProvider,
            Lazy<INuGetFeatureFlagService> nugetFeatureFlagService,
            Lazy<IVulnerabilitiesNotificationService> vulnerabilitiesFoundService)
            : this(AsyncServiceProvider.GlobalProvider,
                  solutionManager,
                  lockService,
                  logger,
                  errorList,
                  outputConsoleProvider,
                  nugetFeatureFlagService,
                  vulnerabilitiesFoundService)
        { }

        public SolutionRestoreWorker(
            IAsyncServiceProvider asyncServiceProvider,
            Lazy<IVsSolutionManager> solutionManager,
            Lazy<INuGetLockService> lockService,
            Lazy<Common.ILogger> logger,
            Lazy<INuGetErrorList> errorList,
            Lazy<IOutputConsoleProvider> outputConsoleProvider,
            Lazy<INuGetFeatureFlagService> nugetFeatureFlagService,
            Lazy<IVulnerabilitiesNotificationService> vulnerabilitiesFoundService)
        {
            if (asyncServiceProvider == null)
            {
                throw new ArgumentNullException(nameof(asyncServiceProvider));
            }

            if (solutionManager == null)
            {
                throw new ArgumentNullException(nameof(solutionManager));
            }

            if (lockService == null)
            {
                throw new ArgumentNullException(nameof(lockService));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (errorList == null)
            {
                throw new ArgumentNullException(nameof(errorList));
            }

            if (outputConsoleProvider == null)
            {
                throw new ArgumentNullException(nameof(outputConsoleProvider));
            }

            if (nugetFeatureFlagService == null)
            {
                throw new ArgumentNullException(nameof(nugetFeatureFlagService));
            }

            if (vulnerabilitiesFoundService == null)
            {
                throw new ArgumentNullException(nameof(vulnerabilitiesFoundService));
            }

            _asyncServiceProvider = asyncServiceProvider;
            _solutionManager = solutionManager;
            _lockService = lockService;
            _logger = logger;
            _errorList = errorList;
            _outputConsoleProvider = outputConsoleProvider;
            _nugetFeatureFlagService = nugetFeatureFlagService;
            _vulnerabilitiesFoundService = vulnerabilitiesFoundService;

            var joinableTaskContextNode = new JoinableTaskContextNode(ThreadHelper.JoinableTaskContext);
            _joinableCollection = joinableTaskContextNode.CreateCollection();
            JoinableTaskFactory = joinableTaskContextNode.CreateFactory(_joinableCollection);

            _componentModel = new AsyncLazy<IComponentModel>(async () =>
                {
                    return await asyncServiceProvider.GetServiceAsync<SComponentModel, IComponentModel>();
                },
                JoinableTaskFactory);
            _solutionLoadedEvent = new AsyncManualResetEvent();
            _isCompleteEvent = new AsyncManualResetEvent();

            Reset();
        }

        private async Task StartInitializationAsync()
        {
            if (Interlocked.CompareExchange(ref _initialized, 1, 0) == 0)
            {
                await JoinableTaskFactory.RunAsync(async () =>
                {
                    await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                    var dte = await _asyncServiceProvider.GetDTEAsync();
                    _solutionEvents = dte.Events.SolutionEvents;
                    _solutionEvents.BeforeClosing += SolutionEvents_BeforeClosing;
                    _solutionEvents.AfterClosing += SolutionEvents_AfterClosing;

                    _vsSolution = await _asyncServiceProvider.GetServiceAsync<SVsSolution, IVsSolution>();
                    Assumes.Present(_vsSolution);
                    Advise(_vsSolution);
                });

                // Signal the background job runner solution is loaded
                // Needed when OnAfterBackgroundSolutionLoadComplete fires before
                // Advise has been called.
                if (!_solutionLoadedEvent.IsSet && await IsSolutionFullyLoadedAsync())
                {
                    _solutionLoadedEvent.Set();
                }
            }
        }

        private async Task<bool> IsSolutionFullyLoadedAsync()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            IVsSolution vsSolution = _vsSolution;

            if (vsSolution == null)
            {
                // Initialization may not have completed yet.
                return false;
            }

            object value;
            var hr = vsSolution.GetProperty((int)(__VSPROPID4.VSPROPID_IsSolutionFullyLoaded), out value);
            ErrorHandler.ThrowOnFailure(hr);

            return (bool)value;
        }

        public void Dispose()
        {
            Reset(isDisposing: true);

            if (_initialized != 0)
            {
                JoinableTaskFactory.Run(async () =>
                {
                    await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                    _solutionEvents.AfterClosing -= SolutionEvents_AfterClosing;
                    Unadvise();
                });
            }
        }

        private void Reset(bool isDisposing = false)
        {
            // Make sure worker restore operation is cancelled
            _workerCts?.Cancel();

            if (_backgroundJobRunner?.IsValueCreated == true)
            {
                // Await completion of the background work
                JoinableTaskFactory.Run(
                    async () =>
                    {
                        // Do not block VS forever
                        // After the specified delay the task will disjoin.
                        await _backgroundJobRunner.GetValueAsync().WithTimeout(TimeSpan.FromSeconds(5));
                    });
            }

            _pendingRestore?.Dispose();
            _workerCts?.Dispose();

            if (_pendingRequests?.IsValueCreated == true)
            {
                _pendingRequests.Value.Dispose();
            }

            if (!isDisposing)
            {
                _solutionLoadedEvent.Reset();

                _workerCts = new CancellationTokenSource();

                _pendingRequests = new Lazy<BlockingCollection<SolutionRestoreRequest>>(
                    () => new BlockingCollection<SolutionRestoreRequest>(RequestQueueLimit));

                _pendingRestore = new BackgroundRestoreOperation();
                _activeRestoreTask = TaskResult.True;
                _restoreJobContext = new SolutionRestoreJobContext();

                // Set to signaled, restore is no longer busy
                _isCompleteEvent.Set();
            }
        }

        private void SolutionEvents_BeforeClosing()
        {
            // Signal background runner to terminate execution
            _workerCts?.Cancel();
        }

        private void SolutionEvents_AfterClosing()
        {
            Reset();

            // Clear warnings/errors from nuget
            if (_errorList.IsValueCreated)
            {
                _errorList.Value.ClearNuGetEntries();
            }
        }

        public async Task<bool> ScheduleRestoreAsync(
            SolutionRestoreRequest request, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return false;
            }

            // Reset to signal that a restore is in progress.
            // This sets IsRunning to true
            _isCompleteEvent.Reset();

            try
            {
                await StartInitializationAsync();

                BackgroundRestoreOperation pendingRestore = _pendingRestore;

                // lock _pendingRequests to figure out if we need to start a new background job for restore
                // or if there is already one running which will also take care of current request.
                lock (_lockPendingRequestsObj)
                {
                    var shouldStartNewBGJobRunner = true;

                    // check if there are already pending restore request or active restore task
                    // then don't initiate a new background job runner.
                    if (_pendingRequests.Value.Count > 0 || IsBusy)
                    {
                        shouldStartNewBGJobRunner = false;
                    }
                    else if (_lockService.Value.IsLockHeld && _lockService.Value.LockCount > 0)
                    {
                        // when restore is not running but NuGet lock is still held for the current async operation,
                        // then it means other NuGet operation like Install or Update are in progress which will
                        // take care of running restore for appropriate projects so skipping auto restore in that case.
                        return true;
                    }

                    AsyncLazy<bool> backgroundJobRunner = _backgroundJobRunner;

                    if (backgroundJobRunner == null)
                    {
                        shouldStartNewBGJobRunner = true;
                    }
                    else if (backgroundJobRunner.IsValueCreated && backgroundJobRunner.IsValueFactoryCompleted)
                    {
                        Task<bool> valueTask = backgroundJobRunner.GetValueAsync();

                        if (valueTask.IsFaulted || valueTask.IsCanceled)
                        {
                            shouldStartNewBGJobRunner = true;
                        }
                    }

                    // on-board request onto pending restore operation
                    _pendingRequests.Value.TryAdd(request);

                    // When there is no current background restore job running, then start a new one.
                    // Otherwise, the current request will await the existing job to be completed.
                    if (shouldStartNewBGJobRunner)
                    {
                        _backgroundJobRunner = new AsyncLazy<bool>(
                           () => StartBackgroundJobRunnerAsync(_workerCts.Token),
                           JoinableTaskFactory);
                    }
                }

                try
                {
                    using (_joinableCollection.Join())
                    {
                        // Await completion of the requested restore operation or
                        // completion of the current job runner.
                        // The caller will be unblocked immediately upon
                        // cancellation request via provided token.
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
                        return await await Task
                            .WhenAny(
                                pendingRestore.Task,
                                _backgroundJobRunner.GetValueAsync())
                            .WithCancellation(token);
#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks
                    }
                }
                catch (OperationCanceledException) when (token.IsCancellationRequested)
                {
                    return false;
                }
                catch (Exception e)
                {
                    Logger.LogError(e.ToString());
                    return false;
                }
            }
            finally
            {
                // Signal that all pending operations are complete.
                _isCompleteEvent.Set();
            }
        }

        public async Task<bool> RestoreAsync(SolutionRestoreRequest request, CancellationToken token)
        {
            // Signal that restore is running
            _isCompleteEvent.Reset();

            try
            {
                using (_joinableCollection.Join())
                {
                    await StartInitializationAsync();

                    using (var restoreOperation = new BackgroundRestoreOperation())
                    {
                        await PromoteTaskToActiveAsync(restoreOperation, token);
                        var isBulkRestoreCoordinationEnabled = await IsBulkRestoreCoordinationEnabledAsync();
                        var restoreTrackingData = GetRestoreTrackingData(
                            restoreReason: ImplicitRestoreReason.None,
                            requestCount: 1,
                            isBulkRestoreCoordinationEnabled: isBulkRestoreCoordinationEnabled,
                            projectRestoreInfoSourcesCount: -1,
                            bulkRestoreCoordinationCheckStartTime: default,
                            projectsReadyCheckCount: 0,
                            projectReadyTimings: new List<TimeSpan>(),
                            request.ExplicitRestoreReason);
                        var result = await ProcessRestoreRequestAsync(restoreOperation, request, restoreTrackingData, token);

                        return result;
                    }
                }
            }
            finally
            {
                // Signal that restore has been completed.
                _isCompleteEvent.Set();
            }
        }

        private async Task<bool> IsBulkRestoreCoordinationEnabledAsync()
        {
            return await _nugetFeatureFlagService.Value.IsFeatureEnabledAsync(NuGetFeatureFlagConstants.BulkRestoreCoordination);
        }

        public async Task CleanCacheAsync()
        {
            // get all build integrated based nuget projects and delete the cache file.
            await Task.WhenAll(
                (await SolutionManager.GetNuGetProjectsAsync()).OfType<BuildIntegratedNuGetProject>().Select(async e =>
                    Common.FileUtility.Delete(await e.GetCacheFilePathAsync())));

            Interlocked.Exchange(ref _restoreJobContext, new SolutionRestoreJobContext());
        }

        private async Task<bool> StartBackgroundJobRunnerAsync(CancellationToken token)
        {
            // Hops onto a background pool thread
            await TaskScheduler.Default;

            var status = false;
            // Check if the solution is fully loaded
            while (!_solutionLoadedEvent.IsSet)
            {
                // Needed when OnAfterBackgroundSolutionLoadComplete fires before
                // Advise has been called.
                if (await IsSolutionFullyLoadedAsync())
                {
                    _solutionLoadedEvent.Set();
                    break;
                }
                else
                {
                    // Waits for 100ms to let solution fully load or canceled
                    await _solutionLoadedEvent.WaitAsync()
                        .WithTimeout(TimeSpan.FromMilliseconds(DelaySolutionLoadRetry))
                        .WithCancellation(token);
                }
            }

            ImplicitRestoreReason restoreReason = ImplicitRestoreReason.None;
            var isBulkRestoreCoordinationEnabled = await IsBulkRestoreCoordinationEnabledAsync();
            DateTime? bulkRestoreCoordinationCheckStartTime = default;
            // Loops until there are pending restore requests or it's get cancelled
            while (!token.IsCancellationRequested)
            {
                lock (_lockPendingRequestsObj)
                {
                    // if no pending restore requests then shut down the restore job runner.
                    if (_pendingRequests.Value.Count == 0)
                    {
                        break;
                    }
                }

                // Grabs a local copy of pending restore operation
                using (var restoreOperation = _pendingRestore)
                {
                    try
                    {
                        // Blocks the execution until first request is scheduled
                        // Monitors the cancelllation token as well.
                        var request = _pendingRequests.Value.Take(token);

                        token.ThrowIfCancellationRequested();

                        // Claims the ownership over the active task
                        // Awaits for currently running restore to complete
                        await PromoteTaskToActiveAsync(restoreOperation, token);

                        token.ThrowIfCancellationRequested();
                        DateTime lastNominationReceived = DateTime.UtcNow;
                        int requestCount = 1;
                        int projectsReadyCheckCount = 0;
                        int projectRestoreInfoSourcesCount = -1;
                        ExplicitRestoreReason explicitRestoreReason = request.ExplicitRestoreReason;
                        List<TimeSpan> projectReadyTimings = null;
                        // Drains the queue
                        while (!_pendingRequests.Value.IsCompleted
                            && !token.IsCancellationRequested)
                        {
                            SolutionRestoreRequest next;

                            // check if there are pending nominations
                            var isAllProjectsNominated = await _solutionManager.Value.IsAllProjectsNominatedAsync();

                            // Try to get a request without a timeout. We don't want to *block* the threadpool thread.
                            if (!_pendingRequests.Value.TryTake(out next, millisecondsTimeout: 0, token))
                            {
                                if (isAllProjectsNominated)
                                {
                                    if (isBulkRestoreCoordinationEnabled)
                                    {
                                        var projectReadyCheckMeasurement = Stopwatch.StartNew();
                                        if (bulkRestoreCoordinationCheckStartTime == default)
                                        {
                                            bulkRestoreCoordinationCheckStartTime = DateTime.UtcNow;
                                        }
                                        projectsReadyCheckCount++;
                                        // If we are about to start restore, we should run through all the projects to ensure there isn't a pending nomination.
                                        IReadOnlyList<object> restoreProjectInfoSources = _solutionManager.Value.GetAllProjectRestoreInfoSources();
                                        projectRestoreInfoSourcesCount = restoreProjectInfoSources.Count;
                                        var allProjectsReady = true;
                                        var bulkCheckTimeout = false;
                                        for (int i = 0; i < restoreProjectInfoSources.Count && !bulkCheckTimeout; i++)
                                        {
                                            var restoreInfoSource = (IVsProjectRestoreInfoSource)restoreProjectInfoSources[i];
                                            if (restoreInfoSource.HasPendingNomination)
                                            {
                                                allProjectsReady = false;
                                                TimeSpan timeoutTime = CalculateTimeoutTime(bulkRestoreCoordinationCheckStartTime.Value, DateTime.UtcNow, BulkRestoreCoordinationTimeout);
                                                var timeoutTask = Task.Delay(timeoutTime, token);
                                                var whenNominatedTask = restoreInfoSource.WhenNominated(token);

                                                var result = await Task.WhenAny(whenNominatedTask, timeoutTask);
                                                if (result == timeoutTask)
                                                {
                                                    bulkCheckTimeout = true;
                                                }
                                            }
                                        }

                                        projectReadyCheckMeasurement.Stop();
                                        if (projectReadyTimings == null)
                                        {
                                            projectReadyTimings = new();
                                        }
                                        projectReadyTimings.Add(projectReadyCheckMeasurement.Elapsed);

                                        if (allProjectsReady)
                                        {
                                            restoreReason = ImplicitRestoreReason.ProjectsReady;
                                            break;
                                        }
                                        if (bulkCheckTimeout)
                                        {
                                            restoreReason = ImplicitRestoreReason.ProjectsReadyCheckTimeout;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        restoreReason = ImplicitRestoreReason.AllProjectsNominated;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (!isBulkRestoreCoordinationEnabled)
                                    {
                                        // Break if we've waited for more than 30s without an actual nomination.
                                        if (lastNominationReceived.AddMilliseconds(MaxIdleWaitTimeMs) < DateTime.UtcNow)
                                        {
                                            restoreReason = ImplicitRestoreReason.NominationsIdleTimeout;
                                            break;
                                        }
                                    }
                                    await Task.Delay(IdleTimeoutMs, token);
                                }
                            }
                            else
                            {
                                requestCount++;
                                lastNominationReceived = DateTime.UtcNow;
                                // Upgrade request if necessary
                                if (next != null && next.RestoreSource != request.RestoreSource)
                                {
                                    // there could be requests of two types: Auto-Restore or Explicit
                                    // Explicit is always preferred.
                                    request = new SolutionRestoreRequest(
                                        next.ForceRestore || request.ForceRestore,
                                        RestoreOperationSource.Explicit,
                                        next.ExplicitRestoreReason);

                                    // we don't want to delay explicit solution restore request so just break at this time.
                                    restoreReason = ImplicitRestoreReason.None;
                                    break;
                                }
                            }
                        }

                        token.ThrowIfCancellationRequested();

                        // Replaces pending restore operation with a new one.
                        // Older value is ignored.
                        var ignore = Interlocked.CompareExchange(
                            ref _pendingRestore, new BackgroundRestoreOperation(), restoreOperation);

                        token.ThrowIfCancellationRequested();

                        Dictionary<string, object> restoreStartTrackingData = GetRestoreTrackingData(
                            restoreReason,
                            requestCount,
                            isBulkRestoreCoordinationEnabled,
                            projectRestoreInfoSourcesCount,
                            bulkRestoreCoordinationCheckStartTime,
                            projectsReadyCheckCount,
                            projectReadyTimings,
                            explicitRestoreReason);

                        // Runs restore job with scheduled request params
                        status = await ProcessRestoreRequestAsync(restoreOperation, request, restoreStartTrackingData, token);

                        // Repeats...
                    }
                    catch (OperationCanceledException) when (token.IsCancellationRequested)
                    {
                        // Ignores
                    }
                    catch (Exception e)
                    {
                        // Writes stack to activity log
                        Logger.LogError(e.ToString());
                        // Do not die just yet
                    }
                }
            }

            return status;
        }

        /// <summary>
        /// Calculates the timeout time.
        /// </summary>
        /// <param name="startTime">The start time from which to calculate</param>
        /// <param name="currentTime">The current time</param>
        /// <param name="timeoutTime">The timeout time</param>
        /// <returns>The leftover timeout time, or 0.</returns>
        internal static TimeSpan CalculateTimeoutTime(DateTime startTime, DateTime currentTime, TimeSpan timeoutTime)
        {
            TimeSpan leftoverTime = (startTime - currentTime) + timeoutTime;
            if (leftoverTime.Ticks > 0)
            {
                return leftoverTime;
            }
            return new TimeSpan(ticks: 0);
        }

        private static Dictionary<string, object> GetRestoreTrackingData(ImplicitRestoreReason restoreReason, int requestCount, bool isBulkRestoreCoordinationEnabled, int projectRestoreInfoSourcesCount, DateTime? bulkRestoreCoordinationCheckStartTime, int projectsReadyCheckCount, List<TimeSpan> projectReadyTimings, ExplicitRestoreReason explicitRestoreReason)
        {
            double bulkRestoreCoordinationTotalTime = bulkRestoreCoordinationCheckStartTime == default ?
                0.0 :
                (DateTime.UtcNow - bulkRestoreCoordinationCheckStartTime.Value).TotalSeconds;

            return new()
            {
                { RestoreTelemetryEvent.ImplicitRestoreReason, restoreReason },
                { RestoreTelemetryEvent.RequestCount, requestCount },
                { RestoreTelemetryEvent.IsBulkFileRestoreCoordinationEnabled, isBulkRestoreCoordinationEnabled },
                { RestoreTelemetryEvent.ProjectRestoreInfoSourcesCount, projectRestoreInfoSourcesCount },
                { RestoreTelemetryEvent.ProjectsReadyCheckTotalTime, bulkRestoreCoordinationTotalTime },
                { RestoreTelemetryEvent.ProjectsReadyCheckCount, projectsReadyCheckCount },
                { RestoreTelemetryEvent.ProjectReadyCheckTimings, TelemetryUtility.ToJsonArrayOfTimingsInSeconds(projectReadyTimings) },
                { RestoreTelemetryEvent.ExplicitRestoreReason, explicitRestoreReason },
            };
        }

        private async Task<bool> ProcessRestoreRequestAsync(
            BackgroundRestoreOperation restoreOperation,
            SolutionRestoreRequest request,
            Dictionary<string, object> restoreStartTrackingData,
            CancellationToken token)
        {
            // if the request is implicit & this is the first restore, assume we are restoring due to a solution load.
            var isSolutionLoadRestore = _isFirstRestore &&
                request.RestoreSource == RestoreOperationSource.Implicit;
            double timeSinceLastRestoreCompletedTime = _isFirstRestore ?
                0.0 :
                (DateTimeOffset.UtcNow - _lastRestoreCompletedTime).TotalSeconds;
            string lastRestoreOperationSourcee = _isFirstRestore ?
                "None" :
                _lastRestoreOperationSource.ToString();

            _isFirstRestore = false;
            _lastRestoreOperationSource = request.RestoreSource;
            restoreStartTrackingData.Add(nameof(RestoreTelemetryEvent.IsSolutionLoadRestore), isSolutionLoadRestore);
            restoreStartTrackingData.Add(nameof(RestoreTelemetryEvent.TimeSinceLastRestoreCompleted), timeSinceLastRestoreCompletedTime);
            restoreStartTrackingData.Add(nameof(RestoreTelemetryEvent.LastRestoreOperationSource), lastRestoreOperationSourcee);

            // Start the restore job in a separate task on a background thread
            // it will switch into main thread when necessary.
            var joinableTask = JoinableTaskFactory.RunAsync(
            () => StartRestoreJobAsync(request, restoreStartTrackingData, token));

            var continuation = joinableTask
                .Task
                .ContinueWith(t => restoreOperation.ContinuationAction(t));

            bool restoreTask = await joinableTask;
            _lastRestoreCompletedTime = DateTimeOffset.UtcNow;

            return restoreTask;
        }

        private async Task PromoteTaskToActiveAsync(BackgroundRestoreOperation restoreOperation, CancellationToken token)
        {
            var pendingTask = restoreOperation.Task;

            var attempt = 0;
            for (var retry = true;
                retry && !token.IsCancellationRequested && attempt != PromoteAttemptsLimit;
                attempt++)
            {
                // Grab local copy of active task
                var activeTask = _activeRestoreTask;

                // Await for the completion of the active *unbound* task
                var cancelTcs = new TaskCompletionSource<bool>();
                using (var ctr = token.Register(() => cancelTcs.TrySetCanceled()))
                {
                    await Task.WhenAny(activeTask, cancelTcs.Task);
                }

                // Try replacing active task with the new one.
                // Retry from the beginning if the active task has changed.
                retry = Interlocked.CompareExchange(
                    ref _activeRestoreTask, pendingTask, activeTask) != activeTask;
            }

            if (attempt == PromoteAttemptsLimit)
            {
                throw new InvalidOperationException("Failed promoting pending task.");
            }
        }

        private async Task<bool> StartRestoreJobAsync(
            SolutionRestoreRequest request, Dictionary<string, object> restoreStartTrackingData, CancellationToken token)
        {
            await TaskScheduler.Default;

            using (var jobCts = CancellationTokenSource.CreateLinkedTokenSource(token))
            {
                return await _lockService.Value.ExecuteNuGetOperationAsync(async () =>
                {
                    var componentModel = await _componentModel.GetValueAsync(jobCts.Token);
                    using (var logger = new RestoreOperationLogger(_outputConsoleProvider))
                    {
                        try
                        {
                            // Start logging
                            await logger.StartAsync(
                            request.RestoreSource,
                            _errorList,
                            JoinableTaskFactory,
                            jobCts);

                            // Run restore
                            var job = componentModel.GetService<ISolutionRestoreJob>();
                            return await job.ExecuteAsync(request, _restoreJobContext, logger, restoreStartTrackingData, _vulnerabilitiesFoundService, jobCts.Token);
                        }
                        finally
                        {
                            // Complete all logging
                            await logger.StopAsync();
                        }
                    }
                }, jobCts.Token);
            }
        }

        public override int OnAfterBackgroundSolutionLoadComplete()
        {
            _solutionLoadedEvent.Set();

            return VSConstants.S_OK;
        }

        private class BackgroundRestoreOperation
            : IEquatable<BackgroundRestoreOperation>, IDisposable
        {
            private readonly Guid _id = Guid.NewGuid();

            private TaskCompletionSource<bool> JobTcs { get; } = new TaskCompletionSource<bool>();

            public Task<bool> Task => JobTcs.Task;

            public System.Runtime.CompilerServices.TaskAwaiter<bool> GetAwaiter() => Task.GetAwaiter();

            public void ContinuationAction(Task<bool> targetTask)
            {
                Assumes.True(targetTask.IsCompleted);

                // propagate the restore target task status to the *unbound* active task.
                if (targetTask.IsFaulted || targetTask.IsCanceled)
                {
                    // fail the restore result if the target task has failed or cancelled.
                    JobTcs.TrySetResult(result: false);
                }
                else
                {
                    // completed successfully
#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
                    JobTcs.TrySetResult(targetTask.Result);
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits
                }
            }

            public bool Equals(BackgroundRestoreOperation other)
            {
                if (ReferenceEquals(null, other))
                {
                    return false;
                }

                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                return _id == other._id;
            }

            public override bool Equals(object obj) => Equals(obj as BackgroundRestoreOperation);

            public override int GetHashCode() => _id.GetHashCode();

            public static bool operator ==(BackgroundRestoreOperation left, BackgroundRestoreOperation right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(BackgroundRestoreOperation left, BackgroundRestoreOperation right)
            {
                return !Equals(left, right);
            }

            public override string ToString() => _id.ToString();

            public void Dispose()
            {
                // Inner code block of using clause may throw an unhandled exception.
                // This'd result in leaving the active task in incomplete state.
                // Hence the next restore operation would stop responding forever.
                // To resolve potential deadlock issue the unbound task is to be completed here.
                if (!Task.IsCompleted && !Task.IsCanceled && !Task.IsFaulted)
                {
                    JobTcs.TrySetResult(result: false);
                }
            }
        }
    }
}
