// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Logging;

[Shared]
[Export(typeof(OutputWindowLogger))]
internal class OutputWindowLogger : ILogger
{
    private const LogLevel MinimumLogLevel = LogLevel.Warning;
    private readonly OutputPane _outputPane;

    [ImportingConstructor]
    public OutputWindowLogger(JoinableTaskContext joinableTaskContext)
    {
        _outputPane = new OutputPane(joinableTaskContext);
    }

    [Obsolete("Exists only for Mock.")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    internal OutputWindowLogger()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        throw new NotImplementedException();
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= MinimumLogLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (IsEnabled(logLevel))
        {
            _outputPane.WriteLine(formatter(state, exception));
            if (exception is not null)
            {
                _outputPane.WriteLine(exception.ToString());
            }
        }
    }

    private class OutputPane
    {
        private static readonly Guid s_outputPaneGuid = new("BBAFF416-4AF5-41F2-9F93-91F283E43C3B");

        private readonly JoinableTaskContext _threadingContext;
        private readonly IServiceProvider _serviceProvider;
        private IVsOutputWindowPane? _doNotAccessDirectlyOutputPane;

        public OutputPane(JoinableTaskContext threadingContext)
        {
            _threadingContext = threadingContext;
            _serviceProvider = ServiceProvider.GlobalProvider;
        }

        public void WriteLine(string value)
        {
            WriteLineInternal(value);
        }

        private void WriteLineInternal(string value)
        {
            var pane = GetPane();
            if (pane is null)
            {
                return;
            }

            // https://learn.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.interop.ivsoutputwindowpane.outputstringthreadsafe?view=visualstudiosdk-2022#remarks
            if (pane is IVsOutputWindowPaneNoPump noPumpPane)
            {
                noPumpPane.OutputStringNoPump(value + Environment.NewLine);
            }
            else
            {
                pane.OutputStringThreadSafe(value + Environment.NewLine);
            }
        }

        private IVsOutputWindowPane GetPane()
        {
            if (_doNotAccessDirectlyOutputPane is null)
            {
                _threadingContext.Factory.Run(async () =>
                {
                    await _threadingContext.Factory.SwitchToMainThreadAsync();

                    if (_doNotAccessDirectlyOutputPane != null)
                    {
                        // check whether other one already initialized output window.
                        // the output API already handle double initialization, so this is just quick bail
                        // rather than any functional issue
                        return;
                    }

                    var outputWindow = (IVsOutputWindow)_serviceProvider.GetService(typeof(SVsOutputWindow));

                    // this should bring outout window to the front
                    _doNotAccessDirectlyOutputPane = CreateOutputPane(outputWindow);
                });
            }

            return _doNotAccessDirectlyOutputPane!;
        }

        private IVsOutputWindowPane? CreateOutputPane(IVsOutputWindow outputWindow)
        {
            _threadingContext.AssertUIThread();

            // Try to get the workspace pane if it has already been registered
            var workspacePaneGuid = s_outputPaneGuid;

            // If the pane has already been created, CreatePane returns it
            if (ErrorHandler.Succeeded(outputWindow.CreatePane(ref workspacePaneGuid, "Razor Logger Output", fInitVisible: 1, fClearWithSolution: 1)) &&
                ErrorHandler.Succeeded(outputWindow.GetPane(ref workspacePaneGuid, out var pane)))
            {
                return pane;
            }

            return null;
        }
    }
}
