using System;
using System.IO;
using System.Threading;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;

namespace NestedReentrancyProbe;

// Finalizes an independent, real-event-only binlog before bounded hang termination.
public sealed class SnapshotLogger : ILogger
{
    private readonly object _sync = new();
    private readonly ForwardingSource _source = new();
    private BinaryLogger? _logger;
    private Timer? _timer;
    private IEventSource? _actualSource;
    public LoggerVerbosity Verbosity { get; set; } = LoggerVerbosity.Diagnostic;
    public string? Parameters { get; set; }

    public void Initialize(IEventSource eventSource)
    {
        if (Parameters is null || !Path.IsPathFullyQualified(Parameters))
            throw new ArgumentException("Snapshot logger requires an absolute binlog filename.");
        _logger = new BinaryLogger
        {
            Parameters = $"LogFile={Parameters};ProjectImports=None",
            Verbosity = LoggerVerbosity.Diagnostic
        };
        _logger.Initialize(_source);
        _actualSource = eventSource;
        eventSource.AnyEventRaised += Forward;
        _timer = new Timer(_ => Freeze(), null, TimeSpan.FromSeconds(15), Timeout.InfiniteTimeSpan);
    }

    private void Forward(object sender, BuildEventArgs args)
    {
        lock (_sync)
        {
            if (_logger is not null)
                _source.Forward(args);
        }
    }

    private void Freeze()
    {
        lock (_sync)
        {
            if (_logger is null)
                return;
            _logger.Shutdown();
            _logger = null;
            File.WriteAllText(Parameters + ".status.txt",
                $"Snapshot finalized at {DateTime.UtcNow:O}. Actual event prefix only; no synthetic task or build completion events. Freeze timer=15 seconds; early Shutdown also finalizes.\n");
        }
    }

    public void Shutdown()
    {
        _timer?.Dispose();
        if (_actualSource is not null)
            _actualSource.AnyEventRaised -= Forward;
        Freeze();
    }

    private sealed class ForwardingSource : IEventSource
    {
        public event AnyEventHandler? AnyEventRaised;
        public void Forward(BuildEventArgs args) => AnyEventRaised?.Invoke(this, args);

        // BinaryLogger consumes AnyEventRaised. The remaining interface events are unused.
        public event BuildMessageEventHandler MessageRaised { add { } remove { } }
        public event BuildErrorEventHandler ErrorRaised { add { } remove { } }
        public event BuildWarningEventHandler WarningRaised { add { } remove { } }
        public event BuildStartedEventHandler BuildStarted { add { } remove { } }
        public event BuildFinishedEventHandler BuildFinished { add { } remove { } }
        public event ProjectStartedEventHandler ProjectStarted { add { } remove { } }
        public event ProjectFinishedEventHandler ProjectFinished { add { } remove { } }
        public event TargetStartedEventHandler TargetStarted { add { } remove { } }
        public event TargetFinishedEventHandler TargetFinished { add { } remove { } }
        public event TaskStartedEventHandler TaskStarted { add { } remove { } }
        public event TaskFinishedEventHandler TaskFinished { add { } remove { } }
        public event CustomBuildEventHandler CustomEventRaised { add { } remove { } }
        public event BuildStatusEventHandler StatusEventRaised { add { } remove { } }
    }
}
