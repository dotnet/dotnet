// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using OpenTelemetry.Exporter.Geneva.MsgPack;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

/*
BenchmarkDotNet v0.13.10, Windows 11 (10.0.23424.1000)
Intel Core i7-9700 CPU 3.00GHz, 1 CPU, 8 logical and 8 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


| Method                           | Mean       | Error    | StdDev   | Gen0   | Allocated |
|--------------------------------- |-----------:|---------:|---------:|-------:|----------:|
| ExportActivity                   |   847.1 ns | 16.34 ns | 22.36 ns |      - |         - |
| SerializeActivity                |   261.5 ns |  2.91 ns |  2.58 ns |      - |         - |
| CreateActivityWithGenevaExporter | 1,066.0 ns | 20.98 ns | 56.35 ns | 0.0648 |     416 B |
*/

namespace OpenTelemetry.Exporter.Geneva.Benchmarks;

[MemoryDiagnoser]
public class TraceExporterBenchmarks
{
    private readonly Activity activity;
    private readonly Batch<Activity> batch;
    private readonly MsgPackTraceExporter exporter;
    private readonly TracerProvider tracerProvider;
    private readonly ActivitySource activitySource = new("OpenTelemetry.Exporter.Geneva.Benchmark");

    public TraceExporterBenchmarks()
    {
        Activity.DefaultIdFormat = ActivityIdFormat.W3C;

        this.batch = this.CreateBatch();

        #region Create activity to be used for Serialize and Export benchmark methods
        var activityListener = new ActivityListener
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

        activityListener.Dispose();
        #endregion

        this.exporter = new MsgPackTraceExporter(
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

        this.tracerProvider = Sdk.CreateTracerProviderBuilder()
            .SetSampler(new AlwaysOnSampler())
            .AddSource(this.activitySource.Name)
            .AddGenevaTraceExporter(options =>
            {
                options.ConnectionString = "EtwSession=OpenTelemetry";
                options.PrepopulatedFields = new Dictionary<string, object>
                {
                    ["cloud.role"] = "BusyWorker",
                    ["cloud.roleInstance"] = "CY1SCH030021417",
                    ["cloud.roleVer"] = "9.0.15289.2",
                };
            })
            .Build();
    }

    [Benchmark]
    public void ExportActivity()
    {
        this.exporter.Export(this.batch);
    }

    [Benchmark]
    public void SerializeActivity()
    {
        this.exporter.SerializeActivity(this.activity);
    }

    [Benchmark]
    public void CreateActivityWithGenevaExporter()
    {
        // this activity will be created and sent to Geneva exporter
        using var activity = this.activitySource.StartActivity("Benchmark");
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        this.activity.Dispose();
        this.activitySource.Dispose();
        this.batch.Dispose();
        this.exporter.Dispose();
        this.tracerProvider.Dispose();
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
