// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using NuGet.Common;
using NuGet.PackageManagement.VisualStudio;
using NuGet.ProjectManagement;
using NuGet.VisualStudio.Telemetry;
using AsyncLazyInt = Microsoft.VisualStudio.Threading.AsyncLazy<int>;
using Task = System.Threading.Tasks.Task;

namespace NuGet.VisualStudio.Common
{
    [Export(typeof(INuGetUILogger))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class OutputConsoleLogger : INuGetUILogger, IDisposable
    {
        private const string DTEProjectPage = "ProjectsAndSolution";
        private const string DTEEnvironmentCategory = "Environment";
        private const string MSBuildVerbosityKey = "MSBuildOutputVerbosity";

        private const int DefaultVerbosityLevel = 2;

        private readonly IVisualStudioShell _visualStudioShell;
        private readonly Lazy<INuGetErrorList> _errorList;

        /// <summary>
        /// Keeps track of whether or not Dispose() has been called.  If the value is zero, Dispose() has not been called.  If the value is not zero, Dispose() has been called.
        /// </summary>
        private int _isDisposed = 0;

        [SuppressMessage("Build", "CA2213:'OutputConsoleLogger' contains field '_semaphore' that is of IDisposable type 'ReentrantSemaphore', but it is never disposed. Change the Dispose method on 'OutputConsoleLogger' to call Close or Dispose on this field.", Justification = "Field is disposed from async task invoked from Dispose.")]
        internal readonly ReentrantSemaphore _semaphore = ReentrantSemaphore.Create(1, NuGetUIThreadHelper.JoinableTaskFactory.Context, ReentrantSemaphore.ReentrancyMode.NotAllowed);

        private IOutputConsole _outputConsole;
        private AsyncLazyInt _verbosityLevel;

        [ImportingConstructor]
        public OutputConsoleLogger(
            IOutputConsoleProvider consoleProvider,
            Lazy<INuGetErrorList> errorList)
            : this(
                  new VisualStudioShell(AsyncServiceProvider.GlobalProvider),
                  consoleProvider,
                  errorList)
        {
        }

        internal OutputConsoleLogger(
            IVisualStudioShell visualStudioShell,
            IOutputConsoleProvider consoleProvider,
            Lazy<INuGetErrorList> errorList)
        {
            Verify.ArgumentIsNotNull(visualStudioShell, nameof(visualStudioShell));
            Verify.ArgumentIsNotNull(consoleProvider, nameof(consoleProvider));
            Verify.ArgumentIsNotNull(errorList, nameof(errorList));

            _visualStudioShell = visualStudioShell;
            _errorList = errorList;
            _verbosityLevel = new AsyncLazyInt(() => GetMSBuildVerbosityLevelAsync(), NuGetUIThreadHelper.JoinableTaskFactory);

            Run(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                await _visualStudioShell.SubscribeToBuildBeginAsync(() => _errorList.Value.ClearNuGetEntries());
                await _visualStudioShell.SubscribeToAfterClosingAsync(() => _errorList.Value.ClearNuGetEntries());
                _outputConsole = await consoleProvider.CreatePackageManagerConsoleAsync();
            });
        }

        public void Dispose()
        {
            // Only allow one thread to dispose this object
            if (Interlocked.CompareExchange(ref _isDisposed, 1, 0) != 0)
            {
                return;
            }

            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await _semaphore.ExecuteAsync(async () =>
                {
                    await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                    _errorList.Value.Dispose();
                });

                _semaphore.Dispose();
            }
            ).PostOnFailure(nameof(OutputConsoleLogger), nameof(Dispose));
        }

        public void End()
        {
            Run(async () =>
            {
                await _outputConsole.WriteLineAsync(Resources.Finished);
                await _outputConsole.WriteLineAsync(string.Empty);

                // Give the error list focus
                await _errorList.Value.BringToFrontIfSettingsPermitAsync();
            });
        }

        public void Log(MessageLevel level, string message, params object[] args)
        {
            Run(async () =>
            {
                int verbosityLevel = await _verbosityLevel.GetValueAsync();

                if (level == MessageLevel.Info
                    || level == MessageLevel.Error
                    || level == MessageLevel.Warning
                    || verbosityLevel > DefaultVerbosityLevel)
                {
                    if (args.Length > 0)
                    {
                        message = string.Format(CultureInfo.CurrentCulture, message, args);
                    }

                    await _outputConsole.WriteLineAsync(message);
                }
            });
        }

        public void Log(ILogMessage message)
        {
            Run(async () =>
            {
                int verbosityLevel = await _verbosityLevel.GetValueAsync();

                if (message.Level == LogLevel.Information
                    || message.Level == LogLevel.Error
                    || message.Level == LogLevel.Warning
                    || verbosityLevel > DefaultVerbosityLevel)
                {
                    await _outputConsole.WriteLineAsync(message.FormatWithCode());

                    if (message.Level == LogLevel.Error ||
                        message.Level == LogLevel.Warning)
                    {
                        ReportError(message);
                    }
                }
            });
        }

        public void Start()
        {
            Run(async () =>
            {
                await _outputConsole.ActivateAsync();
                await _outputConsole.ClearAsync();
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                _errorList.Value.ClearNuGetEntries();
            });
        }

        private async Task<int> GetMSBuildVerbosityLevelAsync()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var value = await _visualStudioShell.GetPropertyValueAsync(DTEEnvironmentCategory, DTEProjectPage, MSBuildVerbosityKey);
            if (value is int)
            {
                return (int)value;
            }

            return DefaultVerbosityLevel;
        }

        public void ReportError(ILogMessage message)
        {
            var errorListEntry = new ErrorListTableEntry(message);
            _errorList.Value.AddNuGetEntries(errorListEntry);
        }

        private void Run(Func<Task> action, [CallerMemberName] string methodName = null)
        {
            // If Dispose() has been called, do not execute the action because the semaphore was disposed of
            if (_isDisposed != 0)
            {
                return;
            }

            NuGetUIThreadHelper.JoinableTaskFactory
                               .RunAsync(() => _semaphore.ExecuteAsync(action))
                               .PostOnFailure(nameof(OutputConsoleLogger), methodName);
        }
    }
}
