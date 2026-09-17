using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Microsoft.Build.Framework;
using Task = Microsoft.Build.Utilities.Task;

namespace NestedReentrancyProbe;

public abstract class TracedTask : Task
{
    [Required] public string RunDir { get; set; } = "";
    [Required] public string Role { get; set; } = "";
    private string? _trace;

    protected void Begin()
    {
        if (!Path.IsPathFullyQualified(RunDir))
            throw new ArgumentException("RunDir must be absolute.");
        Directory.CreateDirectory(RunDir);
        _trace = Path.Combine(RunDir, $"{Role}-{Environment.ProcessId}-{Guid.NewGuid():N}.trace");
        Record("START");
    }

    protected void Record(string stage)
    {
        File.AppendAllText(_trace!, $"{DateTime.UtcNow:O} role={Role} pid={Environment.ProcessId} tid={Environment.CurrentManagedThreadId} stage={stage}{Environment.NewLine}");
    }

    protected void Message(string stage)
    {
        Record(stage);
        Log.LogMessage(MessageImportance.High, $"PROBE role={Role} pid={Environment.ProcessId} tid={Environment.CurrentManagedThreadId} stage={stage}");
    }
}

// Intentionally unmarked, so the exact -mt engine automatically ejects it.
public sealed class NestedBuildTask : TracedTask
{
    [Required] public string ChildProject { get; set; } = "";
    public string ChildTarget { get; set; } = "Build";

    public override bool Execute()
    {
        Begin();
        if (!Path.IsPathFullyQualified(ChildProject))
            throw new ArgumentException("ChildProject must be absolute.");
        Message($"CALL_CHILD project={ChildProject} target={ChildTarget}");
        bool result = BuildEngine.BuildProjectFile(ChildProject, new[] { ChildTarget }, new Hashtable(), new Hashtable());
        Message($"CHILD_RETURN success={result}");
        Record($"EXECUTE_RETURN success={result}");
        return result;
    }
}

[MSBuildMultiThreadableTask]
public sealed class FileGateTask : TracedTask
{
    public string Signal { get; set; } = "";
    public string WaitFor { get; set; } = "";
    public int TimeoutSeconds { get; set; } = 75;

    public override bool Execute()
    {
        Begin();
        if ((Signal.Length != 0 && !Path.IsPathFullyQualified(Signal)) ||
            (WaitFor.Length != 0 && !Path.IsPathFullyQualified(WaitFor)))
            throw new ArgumentException("Gate paths must be absolute.");
        Message("YIELD_BEGIN");
        var engine = (IBuildEngine3)BuildEngine;
        engine.Yield();
        bool opened = false;
        try
        {
            Record("YIELDED");
            if (Signal.Length != 0)
            {
                File.WriteAllText(Signal, $"{Role} {Environment.ProcessId}");
                Record($"SIGNAL path={Signal}");
            }
            if (WaitFor.Length == 0)
            {
                opened = true;
            }
            else
            {
                var clock = Stopwatch.StartNew();
                // Poll only the explicit file gate; elapsed time never releases a successful gate.
                while (!(opened = File.Exists(WaitFor)) && clock.Elapsed < TimeSpan.FromSeconds(TimeoutSeconds))
                    Thread.Sleep(10);
            }
            Record(opened ? "GATE_OPEN" : "GATE_TIMEOUT");
        }
        finally
        {
            Record("REACQUIRE_BEGIN");
            engine.Reacquire();
            Record("REACQUIRED");
        }
        if (!opened)
            Log.LogError($"Gate {Role} timed out waiting for {WaitFor}");
        Message($"EXECUTE_RETURN success={opened}");
        return opened;
    }
}
