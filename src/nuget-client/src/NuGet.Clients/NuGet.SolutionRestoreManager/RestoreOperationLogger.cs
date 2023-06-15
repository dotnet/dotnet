// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using NuGet.Common;
using NuGet.PackageManagement.VisualStudio;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Common;
using IAsyncServiceProvider = Microsoft.VisualStudio.Shell.IAsyncServiceProvider;
using MSBuildVerbosityLevel = NuGet.SolutionRestoreManager.VerbosityLevel;
using Task = System.Threading.Tasks.Task;

namespace NuGet.SolutionRestoreManager
{
    /// <summary>
    /// Aggregates logging and UI services consumed by the <see cref="SolutionRestoreJob"/>.
    /// </summary>
    internal sealed class RestoreOperationLogger : LoggerBase, ILogger, IDisposable
    {
        private readonly IAsyncServiceProvider _asyncServiceProvider;
        private readonly Lazy<IOutputConsoleProvider> _outputConsoleProvider;

        // Queue of (bool reportProgress, bool showAsOutputMessage, ILogMessage logMessage)
        private readonly ConcurrentQueue<(bool reportProgress, bool showAsOutputMessage, ILogMessage logMessage)> _loggedMessages = new();
        private readonly object _lock = new();
        private bool _currentlyWritingMessages = false;

        private Lazy<INuGetErrorList> _errorList;
        private RestoreOperationSource _operationSource;
        private JoinableTaskFactory _taskFactory;
        private JoinableTaskCollection _jtc;

        private CancellationTokenSource _externalCts;
        private Func<CancellationToken, Task<RestoreOperationProgressUI>> _progressFactory;
        private IOutputConsole _outputConsole;

        private bool _cancelled;
        private bool _hasHeaderBeenShown;
        private bool _showErrorList;

        // The value of the "MSBuild project build output verbosity" setting
        // of VS. From 0 (quiet) to 4 (Diagnostic).
        public int OutputVerbosity { get; private set; }

        public RestoreOperationLogger(
            Lazy<IOutputConsoleProvider> outputConsoleProvider)
            : this(AsyncServiceProvider.GlobalProvider, outputConsoleProvider)
        { }

        // Set the base logger to debug level, all filter will be done here.

        public RestoreOperationLogger(
            IAsyncServiceProvider asyncServiceProvider,
            Lazy<IOutputConsoleProvider> outputConsoleProvider)
            : base(LogLevel.Debug)
        {
            Assumes.Present(asyncServiceProvider);
            Assumes.Present(outputConsoleProvider);

            _asyncServiceProvider = asyncServiceProvider;
            _outputConsoleProvider = outputConsoleProvider;
        }

        public async Task StartAsync(
            RestoreOperationSource operationSource,
            Lazy<INuGetErrorList> errorList,
            JoinableTaskFactory jtf,
            CancellationTokenSource cts)
        {
            Assumes.Present(errorList);
            Assumes.Present(jtf);
            Assumes.Present(cts);

            _operationSource = operationSource;
            _errorList = errorList;

            _jtc = jtf.Context.CreateCollection();
            _taskFactory = jtf.Context.CreateFactory(_jtc);

            _externalCts = cts;
            _externalCts.Token.Register(() => _cancelled = true);

            _progressFactory = t => StatusBarProgress.StartAsync(_asyncServiceProvider, _taskFactory, t);

            await _taskFactory.RunAsync(async () =>
            {
                OutputVerbosity = await GetMSBuildOutputVerbositySettingAsync();

                switch (_operationSource)
                {
                    case RestoreOperationSource.Implicit: // background auto-restore
                        _outputConsole = await _outputConsoleProvider.Value.CreatePackageManagerConsoleAsync();
                        break;
                    case RestoreOperationSource.OnBuild:
                        _outputConsole = await _outputConsoleProvider.Value.CreateBuildOutputConsoleAsync();
                        await _outputConsole.ActivateAsync();
                        break;
                    case RestoreOperationSource.Explicit:
                        _outputConsole = await _outputConsoleProvider.Value.CreatePackageManagerConsoleAsync();
                        await _outputConsole.ActivateAsync();
                        await _outputConsole.ClearAsync();
                        break;
                }
            });

            if (_errorList.IsValueCreated)
            {
                // Clear old entries
                _errorList.Value.ClearNuGetEntries();
            }
        }

        public async Task StopAsync()
        {
            await _jtc.JoinTillEmptyAsync();

            if (_showErrorList)
            {
                // Give the error list focus
                await _errorList.Value.BringToFrontIfSettingsPermitAsync();
            }
        }

        public override void LogInformationSummary(string data)
        {
            // Treat Summary as Debug
            Log(LogLevel.Debug, data);
        }

        public sealed override void Log(ILogMessage logMessage)
        {
            HandleErrorsAndWarnings(logMessage);

            if (DisplayMessage(logMessage.Level))
            {
                var verbosityLevel = GetMSBuildLevel(logMessage.Level);
                var reportProgress = ShouldReportProgress(logMessage);

                // Write to the output window if the verbosity level is high enough.
                var showAsOutputMessage = ShouldShowMessageAsOutput(verbosityLevel);

                // Avoid moving to the UI thread unless there is work to do
                if (reportProgress || showAsOutputMessage)
                {
                    // Take a lock here so that we can accurately determine if we need to spawn a new task after enqueuing a message.
                    // The task will continue to run until _loggedMessages.Count == 0 inside the lock.
                    lock (_lock)
                    {
                        // Make sure the message is queued in order of calls to LogAsync, but don't wait for the UI thread
                        // to actually show it.
                        _loggedMessages.Enqueue((reportProgress, showAsOutputMessage, logMessage));

                        // avoid creating a duplicate log task while one is currently running
                        if (!_currentlyWritingMessages)
                        {
                            _currentlyWritingMessages = true;
                            _ = _taskFactory.RunAsync(ProcessMessageQueue);
                        }
                    }

                    // we received a message and the logging task isn't currently running. Start a new task to process the queue.
                    async Task ProcessMessageQueue()
                    {
                        // capture current progress from the current execution context
                        var progress = RestoreOperationProgressUI.Current;

                        // This might be a different message than the one enqueued above, but overall the printing order
                        // will match the order of calls to LogAsync.
                        while (true)
                        {
                            ILogMessage logMessage = null;
                            while (_loggedMessages.TryDequeue(out var message))
                            {
                                var verbosityLevel = GetMSBuildLevel(message.logMessage.Level);

                                // capture most recent progress message
                                if (message.reportProgress)
                                {
                                    logMessage = message.logMessage;
                                }

                                // Output console
                                if (message.showAsOutputMessage)
                                {
                                    await WriteLineAsync(verbosityLevel, message.logMessage.FormatWithCode());
                                }
                            }

                            // only show the most recent message on the status bar
                            if (logMessage is not null && progress is not null)
                            {
                                await progress.ReportProgressAsync(logMessage.Message);
                            }

                            lock (_lock)
                            {
                                // Messages could be added after we exit the while loop that's calling TryDequeue.
                                // If we get here and still have messages in the queue, we should continue processing.
                                // Since messages are only Enqueued inside the lock above, we have either handled all the messages or
                                // the next message will be Enqueued and immediately spawn another processing task.
                                // If we're at zero messages, we can stop this task.
                                if (_loggedMessages.Count == 0)
                                {
                                    _currentlyWritingMessages = false;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Log messages to VS. This is optimized to determine
        /// if the message needs to be logged before moving to the
        /// UI thread.
        /// </summary>
        public sealed override Task LogAsync(ILogMessage logMessage)
        {
            Log(logMessage);
            return Task.CompletedTask;
        }

        private void HandleErrorsAndWarnings(ILogMessage logMessage)
        {
            // Display only errors/warnings
            if (logMessage.Level >= LogLevel.Warning)
            {
                var errorListEntry = new ErrorListTableEntry(logMessage);

                // Add the entry to the list
                _errorList.Value.AddNuGetEntries(errorListEntry);

                // Display the error list after restore completes
                _showErrorList = true;
            }
        }

        private static bool ShouldReportProgress(ILogMessage logMessage)
        {
            // Only show messages with VerbosityLevel.Minimal.
            // Do not show errors, warnings, verbose or debug messages on the progress dialog
            // Avoid showing indented messages, these are typically not useful for the progress dialog since
            // they are missing the context of the parent text above it
            return RestoreOperationProgressUI.Current != null
                && GetMSBuildLevel(logMessage.Level) == MSBuildVerbosityLevel.Minimal
                && !IsStringIndented(logMessage);

            static bool IsStringIndented(ILogMessage logMessage)
            {
                return logMessage.Message.Length > 0 && char.IsWhiteSpace(logMessage.Message[0]);
            }
        }

        /// <summary>
        /// Outputs a message to the debug output pane, if the VS MSBuildOutputVerbosity
        /// setting value is greater than or equal to the given verbosity. So if verbosity is 0,
        /// it means the message is always written to the output pane.
        /// </summary>
        /// <param name="verbosity">The verbosity level.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">An array of objects to write using format. </param>
        public Task WriteLineAsync(MSBuildVerbosityLevel verbosity, string format, params object[] args)
        {
            if (ShouldShowMessageAsOutput(verbosity))
            {
                return _outputConsole.WriteLineAsync(format, args);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// True if the message has a high enough verbosity level.
        /// </summary>
        protected override bool DisplayMessage(LogLevel messageLevel)
        {
            var verbosityLevel = GetMSBuildLevel(messageLevel);

            if (_cancelled)
            {
                // If an operation is canceled, don't log anything, simply return
                // And, show a single message gets shown in the summary that package restore has been canceled
                // Do not report it as separate errors
                return false;
            }

            // If the verbosity level of message is worse than VerbosityLevel.Normal, that is,
            // VerbosityLevel.Detailed or VerbosityLevel.Diagnostic, AND,
            // _msBuildOutputVerbosity is lesser than verbosityLevel; do nothing
            if (verbosityLevel > MSBuildVerbosityLevel.Normal && OutputVerbosity < (int)verbosityLevel)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// True if this message will be written out.
        /// </summary>
        public bool ShouldShowMessageAsOutput(MSBuildVerbosityLevel verbosity)
        {
            return _outputConsole != null && OutputVerbosity >= (int)verbosity;
        }

        public async Task LogExceptionAsync(Exception ex)
        {
            string message;
            if (OutputVerbosity < 3)
            {
                message = string.Format(CultureInfo.CurrentCulture,
                    Resources.ErrorOccurredRestoringPackages,
                    ex.Message);
            }
            else
            {
                // output exception detail when _msBuildOutputVerbosity is >= Detailed.
                message = string.Format(CultureInfo.CurrentCulture, Resources.ErrorOccurredRestoringPackages, ex);
            }

            if (_operationSource == RestoreOperationSource.Explicit)
            {
                // Write to the error window and console
                Log(LogMessage.Create(LogLevel.Error, message));
            }
            else
            {
                // Write to console
                await WriteLineAsync(MSBuildVerbosityLevel.Quiet, message);
            }

            ExceptionHelper.WriteErrorToActivityLog(ex);
        }

        public async Task ShowErrorAsync(string errorText)
        {
            var entry = new ErrorListTableEntry(errorText, LogLevel.Error);

            _errorList.Value.AddNuGetEntries(entry);
            await _errorList.Value.BringToFrontIfSettingsPermitAsync();
        }

        public Task WriteHeaderAsync()
        {
            if (!_hasHeaderBeenShown)
            {
                _hasHeaderBeenShown = true;

                switch (_operationSource)
                {
                    case RestoreOperationSource.Implicit:
                        return WriteLineAsync(MSBuildVerbosityLevel.Normal, Resources.RestoringPackages);
                    case RestoreOperationSource.OnBuild:
                        return WriteLineAsync(MSBuildVerbosityLevel.Normal, Resources.PackageRestoreOptOutMessage);
                    case RestoreOperationSource.Explicit:
                        return WriteLineAsync(MSBuildVerbosityLevel.Normal, Resources.RestoringPackages);
                }
            }
            return Task.CompletedTask;
        }

        public async Task WriteSummaryAsync(NuGetOperationStatus operationStatus, TimeSpan duration)
        {
            var forceStatusWrite = _operationSource == RestoreOperationSource.Explicit;
            var quietOrMinimal = forceStatusWrite ? MSBuildVerbosityLevel.Quiet : MSBuildVerbosityLevel.Minimal;
            var quietOrNormal = forceStatusWrite ? MSBuildVerbosityLevel.Quiet : MSBuildVerbosityLevel.Normal;
            var quietOrDetailed = forceStatusWrite ? MSBuildVerbosityLevel.Quiet : MSBuildVerbosityLevel.Detailed;

            switch (operationStatus)
            {
                case NuGetOperationStatus.Cancelled:
                    await WriteLineAsync(
                        quietOrMinimal,
                        Resources.PackageRestoreCanceled);
                    break;
                case NuGetOperationStatus.NoOp:
                    if (forceStatusWrite)
                    {
                        await WriteLineAsync(
                                quietOrDetailed,
                                Resources.NothingToRestore);
                    }
                    break;
                case NuGetOperationStatus.Failed:
                    await WriteLineAsync(
                            quietOrMinimal,
                            Resources.PackageRestoreFinishedWithError);
                    break;
                case NuGetOperationStatus.Succeeded:
                    await WriteLineAsync(
                            quietOrNormal,
                            Resources.PackageRestoreFinished);
                    break;
            }

            if (_operationSource != RestoreOperationSource.OnBuild && (_hasHeaderBeenShown || forceStatusWrite))
            {
                // Submit all messages at once. Avoid needless thread switching
                var fullMessage =
                    string.Format(CultureInfo.CurrentCulture, Resources.Operation_TotalTime, duration) +
                    Environment.NewLine +
                    Resources.Operation_Finished +
                    Environment.NewLine +
                    string.Empty +
                    Environment.NewLine;

                await WriteLineAsync(quietOrMinimal, fullMessage);
            }
        }

        public async Task RunWithProgressAsync(
            Func<RestoreOperationLogger, RestoreOperationProgressUI, CancellationToken, Task> asyncRunMethod,
            CancellationToken token)
        {
            await using (var progress = await _progressFactory(token))
            using (var ctr = progress.RegisterUserCancellationAction(() => _externalCts.Cancel()))
            {
                // Save the progress instance in the current execution context.
                // The value won't be available outside of this async method.
                RestoreOperationProgressUI.Current = progress;

                await asyncRunMethod(this, progress, token);
            }
        }

        public Task RunWithProgressAsync(
            Action<RestoreOperationLogger, RestoreOperationProgressUI, CancellationToken> runAction,
            CancellationToken token)
        {
            return RunWithProgressAsync(
                (l, p, t) => { runAction(l, p, t); return Task.CompletedTask; },
                token);
        }

        /// <summary>
        /// Returns the value of the VisualStudio MSBuildOutputVerbosity setting.
        /// </summary>
        /// <remarks>
        /// 0 is Quiet, while 4 is diagnostic.
        /// </remarks>
        private async Task<int> GetMSBuildOutputVerbositySettingAsync()
        {
            await _taskFactory.SwitchToMainThreadAsync();

            var dte = await _asyncServiceProvider.GetDTEAsync();

            var properties = dte.get_Properties("Environment", "ProjectsAndSolution");
            var value = properties.Item("MSBuildOutputVerbosity").Value;
            if (value is int)
            {
                return (int)value;
            }
            return 0;
        }

        public void Dispose()
        {
            _externalCts?.Dispose();
        }

        /// <summary>
        /// NuGet LogLevel -> MSBuild verbosity
        /// </summary>
        private static MSBuildVerbosityLevel GetMSBuildLevel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Error:
                case LogLevel.Warning:
                    return MSBuildVerbosityLevel.Quiet;
                case LogLevel.Minimal:
                    return MSBuildVerbosityLevel.Minimal;
                case LogLevel.Information:
                    return MSBuildVerbosityLevel.Normal;
                case LogLevel.Verbose:
                    return MSBuildVerbosityLevel.Detailed;
                default:
                    return MSBuildVerbosityLevel.Diagnostic;
            }
        }

        /// <summary>
        /// MSBuild verbosity -> NuGet LogLevel
        /// </summary>
        private static LogLevel GetLogLevel(MSBuildVerbosityLevel level)
        {
            switch (level)
            {
                case MSBuildVerbosityLevel.Quiet:
                    return LogLevel.Warning;
                case MSBuildVerbosityLevel.Minimal:
                case MSBuildVerbosityLevel.Normal:
                    return LogLevel.Information;
                case MSBuildVerbosityLevel.Detailed:
                    return LogLevel.Verbose;
                default:
                    return LogLevel.Debug;
            }
        }

        internal class StatusBarProgress : RestoreOperationProgressUI
        {
            private static object Icon = (short)Constants.SBAI_General;
            private readonly JoinableTaskFactory _taskFactory;
            private readonly IVsStatusbar _statusBar;
            private uint _cookie = 0;

            private StatusBarProgress(
                IVsStatusbar statusBar,
                JoinableTaskFactory taskFactory)
            {
                _statusBar = statusBar;
                _taskFactory = taskFactory;
            }

            public static async Task<RestoreOperationProgressUI> StartAsync(
                IAsyncServiceProvider asyncServiceProvider,
                JoinableTaskFactory jtf,
                CancellationToken token)
            {
                token.ThrowIfCancellationRequested();

                await jtf.SwitchToMainThreadAsync(token);

                var statusBar = await asyncServiceProvider.GetServiceAsync<SVsStatusbar, IVsStatusbar>();

                // Make sure the status bar is not frozen
                int frozen;
                statusBar.IsFrozen(out frozen);

                if (frozen != 0)
                {
                    statusBar.FreezeOutput(0);
                }

                statusBar.Animation(1, ref Icon);

                RestoreOperationProgressUI progress = new StatusBarProgress(statusBar, jtf);
                await progress.ReportProgressAsync(Resources.RestoringPackages);

                return progress;
            }

            public override async ValueTask DisposeAsync()
            {
                await _taskFactory.SwitchToMainThreadAsync();

                _statusBar.Animation(0, ref Icon);
                _statusBar.Progress(ref _cookie, 0, "", 0, 0);
                _statusBar.FreezeOutput(0);
                _statusBar.Clear();
            }

            public override async Task ReportProgressAsync(
                string progressMessage,
                uint currentStep,
                uint totalSteps)
            {
                await _taskFactory.SwitchToMainThreadAsync();

                // Make sure the status bar is not frozen
                int frozen;
                _statusBar.IsFrozen(out frozen);

                if (frozen != 0)
                {
                    _statusBar.FreezeOutput(0);
                }

                _statusBar.SetText(progressMessage);

                if (totalSteps != 0)
                {
                    _statusBar.Progress(ref _cookie, 1, "", currentStep, totalSteps);
                }
            }
        }
    }
}
