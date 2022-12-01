// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Validators;

namespace BenchmarkDotNet.Attributes;

internal class DefaultCoreProfileConfig : ManualConfig
{
    public DefaultCoreProfileConfig()
    {
        AddLogger(ConsoleLogger.Default);
        AddExporter(MarkdownExporter.GitHub);

        AddDiagnoser(MemoryDiagnoser.Default);
        AddColumn(StatisticColumn.OperationsPerSecond);
        AddColumnProvider(DefaultColumnProviders.Instance);

        AddValidator(JitOptimizationsValidator.FailOnError);

        AddJob(Job.InProcess
            .WithStrategy(RunStrategy.Throughput));
    }
}
