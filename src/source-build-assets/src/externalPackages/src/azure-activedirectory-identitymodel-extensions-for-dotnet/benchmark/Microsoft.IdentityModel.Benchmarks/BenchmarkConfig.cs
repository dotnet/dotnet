// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using Perfolizer.Horology;

namespace Microsoft.IdentityModel.Benchmarks
{
    public class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
            AddJob(Job.MediumRun
                .WithLaunchCount(4)
                .WithMaxAbsoluteError(TimeInterval.FromMilliseconds(10))
                .AsDefault())
                    // uncomment to disable validation to enable debugging through benchmarks
                    //.WithOption(ConfigOptions.DisableOptimizationsValidator, true)
                    .AddColumn(StatisticColumn.P90, StatisticColumn.P95, StatisticColumn.P100)
                    .WithOrderer(new DefaultOrderer(SummaryOrderPolicy.Method))
                    .HideColumns(Column.WarmupCount, Column.Type, Column.Job)
                    .AddDiagnoser(MemoryDiagnoser.Default); // https://benchmarkdotnet.org/articles/configs/diagnosers.html
                    //.AddDiagnoser(new EtwProfiler()) // Uncomment to generate traces / flame graphs. Doc: https://adamsitnik.com/ETW-Profiler/
        }
    }
}
