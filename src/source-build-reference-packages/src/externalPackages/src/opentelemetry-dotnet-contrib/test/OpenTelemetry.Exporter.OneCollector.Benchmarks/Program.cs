// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using BenchmarkDotNet.Running;

namespace OpenTelemetry.Exporter.OneCollector.Benchmarks;

internal static class Program
{
    public static void Main(string[] args)
    {
        if (Debugger.IsAttached)
        {
            // Note: This block is so you can start the project with debugger
            // attached and step through the code. It is helpful when working on
            // it.
            var benchmarks = new LogRecordCommonSchemaJsonHttpPostBenchmarks
            {
                NumberOfBatches = 10_000,
                NumberOfLogRecordsPerBatch = 1000,
                EnableCompression = true,
            };

            benchmarks.GlobalSetup();

            benchmarks.Export();

            benchmarks.GlobalCleanup();
        }
        else
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }
}
