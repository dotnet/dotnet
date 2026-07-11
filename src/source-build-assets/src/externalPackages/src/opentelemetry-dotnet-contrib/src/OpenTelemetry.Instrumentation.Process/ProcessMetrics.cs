// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.Metrics;
using Diagnostics = System.Diagnostics;

namespace OpenTelemetry.Instrumentation.Process;

internal sealed class ProcessMetrics
{
    internal static readonly Version SemanticConventionsVersion = new(1, 25, 0);
    internal static readonly Meter MeterInstance = Metrics.MeterFactory.Create<ProcessMetrics>(SemanticConventionsVersion);

    static ProcessMetrics()
    {
        MeterInstance.CreateObservableUpDownCounter(
            "process.memory.usage",
            () =>
            {
                using var process = Diagnostics.Process.GetCurrentProcess();
                return process.WorkingSet64;
            },
            unit: "By",
            description: "The amount of physical memory in use.");

        MeterInstance.CreateObservableUpDownCounter(
            "process.memory.virtual",
            () =>
            {
                using var process = Diagnostics.Process.GetCurrentProcess();
                return process.VirtualMemorySize64;
            },
            unit: "By",
            description: "The amount of committed virtual memory.");

        MeterInstance.CreateObservableCounter(
            "process.cpu.time",
            () =>
            {
                using var process = Diagnostics.Process.GetCurrentProcess();
                return new[]
                {
                    new Measurement<double>(process.UserProcessorTime.TotalSeconds, new KeyValuePair<string, object?>("process.cpu.state", "user")),
                    new Measurement<double>(process.PrivilegedProcessorTime.TotalSeconds, new KeyValuePair<string, object?>("process.cpu.state", "system")),
                };
            },
            unit: "s",
            description: "Total CPU seconds broken down by different states.");

        MeterInstance.CreateObservableUpDownCounter(
            "process.thread.count",
            () =>
            {
                using var process = Diagnostics.Process.GetCurrentProcess();
                return process.Threads.Count;
            },
            unit: "{thread}",
            description: "Process threads count.");
    }
}
