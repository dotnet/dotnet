// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.IO;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace Microsoft.AspNetCore.BenchmarkDotNet.Runner;

partial class Program
{
    private static int Main(string[] args)
    {
        var summaries = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly)
            .Run(args, ManualConfig.CreateEmpty());

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
