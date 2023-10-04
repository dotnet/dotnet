// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Editor.Razor.Logging;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Logging;

[Shared]
[Export(typeof(IOutputWindowLogger))]
internal class OutputWindowLogger : IOutputWindowLogger, IDisposable
{
#if DEBUG
    private const LogLevel MinimumLogLevel = LogLevel.Debug;
#else
    private const LogLevel MinimumLogLevel = LogLevel.Warning;
#endif

    private readonly OutputPane _outputPane;

    private ILogger? _testLogger;

    [ImportingConstructor]
    public OutputWindowLogger(JoinableTaskContext joinableTaskContext)
    {
        _outputPane = new OutputPane(joinableTaskContext);
    }

    public IDisposable BeginScope<TState>(TState state) => Scope.Instance;

    public void SetTestLogger(ILogger? testLogger)
    {
        _testLogger = testLogger;
    }

    public void Dispose()
    {
        _outputPane.Dispose();
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= MinimumLogLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        _testLogger?.Log(logLevel, eventId, state, exception, formatter);

        if (IsEnabled(logLevel))
        {
            _outputPane.WriteLine(DateTime.Now.ToString("h:mm:ss.fff ") + formatter(state, exception));
            if (exception is not null)
            {
                _outputPane.WriteLine(exception.ToString());
            }
        }
    }

    private class OutputPane : IDisposable
    {
        private static readonly Guid s_outputPaneGuid = new("BBAFF416-4AF5-41F2-9F93-91F283E43C3B");

        private readonly JoinableTaskContext _threadingContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly AsyncQueue<string> _outputQueue;
        private readonly CancellationTokenSource _disposalTokenSource;
        private IVsOutputWindowPane? _doNotAccessDirectlyOutputPane;

        public OutputPane(JoinableTaskContext threadingContext)
        {
            _threadingContext = threadingContext;
            _serviceProvider = ServiceProvider.GlobalProvider;

            _outputQueue = new AsyncQueue<string>();
            _disposalTokenSource = new CancellationTokenSource();

            _ = StartListeningAsync();
        }

        private async Task StartListeningAsync()
        {
            // Ensure that we're never on the UI thread before we start listening, in case the async queue doesn't yield
            // I suspect this is overkill :D
            await TaskScheduler.Default.SwitchTo(alwaysYield: true);

            while (!_disposalTokenSource.IsCancellationRequested)
            {
                await DequeueAsync(_disposalTokenSource.Token).ConfigureAwait(false);
            }
        }

        public void WriteLine(string value)
        {
            _outputQueue.TryEnqueue(value);
        }

        private async Task DequeueAsync(CancellationToken cancellationToken)
        {
            var value = await _outputQueue.DequeueAsync(cancellationToken).ConfigureAwait(false);
            if (value is null || cancellationToken.IsCancellationRequested)
            {
                return;
            }

            await _threadingContext.Factory.SwitchToMainThreadAsync(cancellationToken);

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
            _threadingContext.AssertUIThread();

            if (_doNotAccessDirectlyOutputPane is null)
            {

                var outputWindow = (IVsOutputWindow)_serviceProvider.GetService(typeof(SVsOutputWindow));

                // this should bring output window to the front
                _doNotAccessDirectlyOutputPane = CreateOutputPane(outputWindow);
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

        public void Dispose()
        {
            _outputQueue.Complete();
            _disposalTokenSource.Cancel();
        }
    }

    private class Scope : IDisposable
    {
        public static readonly Scope Instance = new();

        public void Dispose()
        {
        }
    }
}
