// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.Diagnostics;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.LanguageServices.Razor;

[Shared]
[Export(typeof(RazorLogger))]
internal class ActivityLogRazorLogger : RazorLogger
{
    private readonly IServiceProvider _serviceProvider;
    private readonly JoinableTaskFactory _joinableTaskFactory;

    [ImportingConstructor]
    public ActivityLogRazorLogger(SVsServiceProvider serviceProvider, JoinableTaskContext joinableTaskContext)
    {
        if (serviceProvider is null)
        {
            throw new ArgumentNullException(nameof(serviceProvider));
        }

        if (joinableTaskContext is null)
        {
            throw new ArgumentNullException(nameof(joinableTaskContext));
        }

        _serviceProvider = serviceProvider;
        _joinableTaskFactory = joinableTaskContext.Factory;
    }

    public override void LogError(string message)
    {
        Log(__ACTIVITYLOG_ENTRYTYPE.ALE_ERROR, message);
    }

    public override void LogWarning(string message)
    {
        Log(__ACTIVITYLOG_ENTRYTYPE.ALE_WARNING, message);
    }

    public override void LogVerbose(string message)
    {
        Log(__ACTIVITYLOG_ENTRYTYPE.ALE_INFORMATION, message);
    }

    private void Log(__ACTIVITYLOG_ENTRYTYPE logType, string message)
    {
        // This is an async void method. Catch all exceptions so it doesn't crash the process.
        try
        {
            _joinableTaskFactory.Run(async () => {
                await _joinableTaskFactory.SwitchToMainThreadAsync();

                var activityLog = GetActivityLog();
                if (activityLog != null)
                {
                    var hr = activityLog.LogEntry(
                        (uint)logType,
                        "Razor LSP Client",
                        $"Info:{Environment.NewLine}{message}");
                    ErrorHandler.ThrowOnFailure(hr);
                }
            });
        }
        catch (Exception ex)
        {
            Debug.Fail($"Razor LSP client logging failed. Error: {ex.Message}");
        }
    }

    private IVsActivityLog GetActivityLog()
    {
        ThreadHelper.ThrowIfNotOnUIThread();
        return (IVsActivityLog)_serviceProvider.GetService(typeof(SVsActivityLog));
    }
}
