// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Logging;

[Shared]
[Export(typeof(IRazorLoggerProvider))]
[method: ImportingConstructor]
internal class OutputWindowLoggerProvider(
    // Anything this class imports would have a circular dependency if they tried to log anything,
    // or used anything that does logging, so make sure everything of ours is imported lazily
    Lazy<IClientSettingsManager> clientSettingsManager,
    JoinableTaskContext joinableTaskContext)
    : IRazorLoggerProvider
{
    private readonly Lazy<IClientSettingsManager> _clientSettingsManager = clientSettingsManager;
    private readonly OutputPane _outputPane = new OutputPane(joinableTaskContext);

    public ILogger CreateLogger(string categoryName)
    {
        return new OutputPaneLogger(categoryName, _outputPane, _clientSettingsManager.Value);
    }

    public void Dispose()
    {
        _outputPane.Dispose();
    }

    private class OutputPaneLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly OutputPane _outputPane;
        private readonly IClientSettingsManager _clientSettingsManager;

        public OutputPaneLogger(string categoryName, OutputPane outputPane, IClientSettingsManager clientSettingsManager)
        {
            _categoryName = categoryName;
            _outputPane = outputPane;
            _clientSettingsManager = clientSettingsManager;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return Scope.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _clientSettingsManager.GetClientSettings().AdvancedSettings.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                _outputPane.WriteLine($"{DateTime.Now:h:mm:ss.fff} [{_categoryName}] {formatter(state, exception)}");
                if (exception is not null)
                {
                    _outputPane.WriteLine(exception.ToString());
                }
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
