// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NET

using System.Text.RegularExpressions;
using Microsoft.LinuxTracepoints.Provider;

namespace OpenTelemetry.Tests;

// Warning: Do NOT use this class/design to listen/read user_events in prod.
// It is a hack to workaround lack of decent bits for listening. Hopefully
// this can be removed if/when
// https://github.com/microsoft/LinuxTracepoints-Net/ has listening bits or
// dotnet/runtime supports user_events (both reading & writing) directly.
internal sealed class PerfTracepointListener : IDisposable
{
    private readonly string name;
    private readonly PerfTracepoint tracepoint;
    private readonly ConsoleCommand catCommand;
    private readonly Regex eventRegex = new("(\\w+?)=([\\w\\(\\) .,-]*)( |$)", RegexOptions.Compiled);

    public PerfTracepointListener(string name, string nameArgs)
    {
        this.name = name;

        this.tracepoint = new PerfTracepoint(nameArgs);

        // EACCES (13): Permission denied
        if (this.tracepoint.RegisterResult == 13)
        {
            throw new UnauthorizedAccessException($"Tracepoint could not be registered: '{this.tracepoint.RegisterResult}'. Permission denied.");
        }

        if (this.tracepoint.RegisterResult != 0)
        {
            throw new NotSupportedException($"Tracepoint could not be registered: '{this.tracepoint.RegisterResult}'");
        }

        this.catCommand = ConsoleCommand.Start("cat", "/sys/kernel/debug/tracing/trace_pipe", onOutputReceived: this.OnCatOutputReceived);
        if (this.catCommand.Errors.Any())
        {
            throw new InvalidOperationException($"Could not read '{name}' tracepoints");
        }
    }

    public List<Dictionary<string, string>> Events { get; } = [];

    public bool IsEnabled()
    {
        ConsoleCommand.Run(
            "cat",
            $"/sys/kernel/tracing/events/user_events/{this.name}/enable",
            out var output,
            out var errors);

        return errors.Any() || output.Count() != 1
            ? throw new InvalidOperationException($"Could not determine if '{this.name}' tracepoint is enabled")
            : output.First() != "0";
    }

    public void Enable()
    {
        var errors = ConsoleCommand.Run("sh", @$"-c ""echo '1' > /sys/kernel/tracing/events/user_events/{this.name}/enable""");

        if (errors.Any())
        {
            throw new InvalidOperationException($"Could not enable '{this.name}' tracepoint");
        }
    }

    public void Disable()
    {
        var errors = ConsoleCommand.Run("sh", @$"-c ""echo '0' > /sys/kernel/tracing/events/user_events/{this.name}/enable""");

        if (errors.Any())
        {
            throw new InvalidOperationException($"Could not disable '{this.name}' tracepoint");
        }
    }

    public void Dispose()
    {
        try
        {
            if (this.catCommand != null)
            {
                if (this.catCommand.Errors.Any())
                {
                    throw new InvalidOperationException($"Could not read '{this.name}' tracepoints");
                }

                this.catCommand.Kill();
                this.catCommand.Dispose();
            }
        }
        finally
        {
            this.tracepoint.Dispose();
        }
    }

    private void OnCatOutputReceived(string output)
    {
        var name = $": {this.name}:";

        var startingPosition = output.IndexOf(name, StringComparison.Ordinal);
        if (startingPosition < 0)
        {
            return;
        }

        startingPosition += name.Length;

        var matches = this.eventRegex.Matches(output, startingPosition);

        if (matches.Count > 0)
        {
            Dictionary<string, string> eventData = new(matches.Count);

            foreach (Match match in matches)
            {
                eventData[match.Groups[1].Value] = match.Groups[2].Value;
            }

            this.Events.Add(eventData);
        }
    }
}

#endif
