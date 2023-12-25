// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Editor.Razor.Logging;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Logging;

[Shared]
[Export(typeof(IRazorLoggerProvider))]
internal sealed class RazorLogHubLoggerProvider : IRazorLoggerProvider
{
    // Unique, monotonically increasing ID to identify LogHub session to persist
    // across server restarts.
    private static int s_logHubSessionId;
    private const string LogFileIdentifier = "Razor";

    private readonly RazorLogHubTraceProvider _traceProvider;
    private readonly RazorLogger _razorLogger;
    private readonly AsyncQueue<(TraceEventType Level, string Message, object[] Args)> _outputQueue;
    private readonly CancellationTokenSource _disposalTokenSource;

    [ImportingConstructor]
    public RazorLogHubLoggerProvider(RazorLogHubTraceProvider traceProvider, RazorLogger razorLogger)
    {
        _traceProvider = traceProvider ?? throw new ArgumentNullException(nameof(traceProvider));
        _razorLogger = razorLogger ?? throw new ArgumentNullException(nameof(razorLogger));

        _outputQueue = new();
        _disposalTokenSource = new CancellationTokenSource();

        _ = StartListeningAsync(_disposalTokenSource.Token);
    }

    private async Task StartListeningAsync(CancellationToken cancellationToken)
    {
        // Ensure that we're never on the UI thread before we start listening, in case the async queue doesn't yield
        // I suspect this is overkill :D
        await TaskScheduler.Default.SwitchTo(alwaysYield: true);

        var logInstanceNumber = Interlocked.Increment(ref s_logHubSessionId);
        var traceSource = await _traceProvider.InitializeTraceAsync(LogFileIdentifier, logInstanceNumber, cancellationToken).ConfigureAwait(false);
        if (traceSource is null)
        {
            _razorLogger.LogError("Could not initialize trace source for Razor LogHub logger. No LogHub log will be created.");
            return;
        }

        while (!cancellationToken.IsCancellationRequested)
        {
            var value = await _outputQueue.DequeueAsync(cancellationToken).ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            traceSource.TraceEvent(value.Level, id: 0, value.Message, value.Args);
        }
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new RazorLogHubLogger(categoryName, this);
    }

    public void Dispose()
    {
        _outputQueue.Complete();
        _disposalTokenSource.Cancel();
    }

    internal void Queue(TraceEventType level, string message, params object[] args)
    {
        _outputQueue.TryEnqueue((level, message, args));
    }
}
