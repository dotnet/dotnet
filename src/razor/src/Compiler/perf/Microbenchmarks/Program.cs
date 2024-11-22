// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Linq;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace Microsoft.AspNetCore.BenchmarkDotNet.Runner;

partial class Program
{
    private static int Main(string[] args)
    {
        IConfig config = Debugger.IsAttached
            ? new DebugInProcessConfig()
            : ManualConfig.CreateEmpty();

        var summaries = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly)
            .Run(args, config);

        foreach (var summary in summaries)
        {
            if (summary.HasCriticalValidationErrors)
            {
                return Fail(summary, nameof(summary.HasCriticalValidationErrors));
            }

            foreach (var report in summary.Reports)
            {
                if (!report.BuildResult.IsGenerateSuccess)
                {
                    return Fail(report, nameof(report.BuildResult.IsGenerateSuccess));
                }

                if (!report.BuildResult.IsBuildSuccess)
                {
                    return Fail(report, nameof(report.BuildResult.IsBuildSuccess));
                }

                if (!report.AllMeasurements.Any())
                {
                    return Fail(report, nameof(report.AllMeasurements));
                }
            }
        }

        return 0;
    }

    private static int Fail(object o, string message)
    {
        Console.Error.WriteLine("'{0}' failed, reason: '{1}'", o, message);
        return 1;
    }
}
