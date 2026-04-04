// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using OpenTelemetry.Exporter.Geneva.MsgPack;
using OpenTelemetry.Exporter.Geneva.Tld;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

/*
BenchmarkDotNet v0.13.10, Windows 11 (10.0.23424.1000)
Intel Core i7-9700 CPU 3.00GHz, 1 CPU, 8 logical and 8 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


| Method                    | Mean     | Error    | StdDev   | Allocated |
|-------------------------- |---------:|---------:|---------:|----------:|
| MsgPack_SerializeActivity | 266.4 ns |  3.84 ns |  3.59 ns |         - |
| TLD_SerializeActivity     | 298.9 ns |  1.99 ns |  1.66 ns |         - |
| MsgPack_ExportActivity    | 787.3 ns | 15.70 ns | 31.71 ns |         - |
| TLD_ExportActivity        | 878.6 ns |  9.84 ns |  9.20 ns |         - |
*/

namespace OpenTelemetry.Exporter.Geneva.Benchmarks;

[MemoryDiagnoser]
public class TLDTraceExporterBenchmarks
{
    private readonly Activity? activity;
    private readonly Batch<Activity> batch;
    private readonly MsgPackTraceExporter msgPackExporter;
    private readonly TldTraceExporter tldExporter;
    private readonly ActivitySource activitySource = new("OpenTelemetry.Exporter.Geneva.Benchmark");

    public TLDTraceExporterBenchmarks()
    {
        Activity.DefaultIdFormat = ActivityIdFormat.W3C;

        this.batch = this.CreateBatch();

        using var activityListener = new ActivityListener
        {
            ActivityStarted = null,
            ActivityStopped = null,
            ShouldListenTo = (activitySource) => activitySource.Name == this.activitySource.Name,
            Sample = (ref ActivityCreationOptions<ActivityContext> options) => ActivitySamplingResult.AllDataAndRecorded,
        };

        ActivitySource.AddActivityListener(activityListener);

        using (var testActivity = this.activitySource.StartActivity("Benchmark"))
        {
            this.activity = testActivity!;
            this.activity.SetTag("tagString", "value");
            this.activity.SetTag("tagInt", 100);
            this.activity.SetStatus(ActivityStatusCode.Error);
        }

        this.msgPackExporter = new MsgPackTraceExporter(
            new GenevaExporterOptions
            {
                ConnectionString = "EtwSession=OpenTelemetry",
                PrepopulatedFields = new Dictionary<string, object>
                {
                    ["cloud.role"] = "BusyWorker",
                    ["cloud.roleInstance"] = "CY1SCH030021417",
                    ["cloud.roleVer"] = "9.0.15289.2",
                },
            },
            () => Resource.Empty);

        this.tldExporter = new TldTraceExporter(new GenevaExporterOptions()
        {
            ConnectionString = "EtwSession=OpenTelemetry;PrivatePreviewEnableTraceLoggingDynamic=true",
            PrepopulatedFields = new Dictionary<string, object>
            {
                ["cloud.role"] = "BusyWorker",
                ["cloud.roleInstance"] = "CY1SCH030021417",
                ["cloud.roleVer"] = "9.0.15289.2",
            },
        });
    }

    [Benchmark]
    public void MsgPack_SerializeActivity()
    {
        this.msgPackExporter.SerializeActivity(this.activity!);
    }

    [Benchmark]
    public void TLD_SerializeActivity()
    {
        this.tldExporter.SerializeActivity(this.activity!);
    }

    [Benchmark]
    public void MsgPack_ExportActivity()
    {
        this.msgPackExporter.Export(this.batch);
    }

    [Benchmark]
    public void TLD_ExportActivity()
    {
        this.tldExporter.Export(this.batch);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        this.activity?.Dispose();
        this.batch.Dispose();
        this.activitySource.Dispose();
        this.msgPackExporter.Dispose();
        this.tldExporter.Dispose();
    }

    private Batch<Activity> CreateBatch()
    {
        using var batchGeneratorExporter = new BatchGeneratorExporter();
        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .SetSampler(new AlwaysOnSampler())
            .AddSource(this.activitySource.Name)
            .AddProcessor(new SimpleActivityExportProcessor(batchGeneratorExporter))
            .Build();

        using (var activity = this.activitySource.StartActivity("Benchmark")!)
        {
            activity.SetTag("tagString", "value");
            activity.SetTag("tagInt", 100);
            activity.SetStatus(ActivityStatusCode.Error);
        }

        return batchGeneratorExporter.Batch;
    }

    private class BatchGeneratorExporter : BaseExporter<Activity>
    {
        public Batch<Activity> Batch { get; set; }

        public override ExportResult Export(in Batch<Activity> batch)
        {
            this.Batch = batch;
            return ExportResult.Success;
        }
    }
}
