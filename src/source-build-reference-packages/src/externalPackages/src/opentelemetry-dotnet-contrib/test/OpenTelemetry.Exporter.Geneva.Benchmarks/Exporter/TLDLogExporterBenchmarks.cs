// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter.Geneva.MsgPack;
using OpenTelemetry.Exporter.Geneva.Tld;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

/*
BenchmarkDotNet v0.13.10, Windows 11 (10.0.23424.1000)
Intel Core i7-9700 CPU 3.00GHz, 1 CPU, 8 logical and 8 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


| Method                     | Mean       | Error    | StdDev   | Allocated |
|--------------------------- |-----------:|---------:|---------:|----------:|
| MsgPack_SerializeLogRecord |   441.4 ns |  3.19 ns |  2.99 ns |         - |
| TLD_SerializeLogRecord     |   263.5 ns |  2.93 ns |  2.75 ns |         - |
| MsgPack_ExportLogRecord    | 1,039.3 ns | 20.55 ns | 46.81 ns |         - |
| TLD_ExportLogRecord        |   890.5 ns | 17.48 ns | 25.07 ns |         - |
*/

namespace OpenTelemetry.Exporter.Geneva.Benchmarks;

[MemoryDiagnoser]
public class TLDLogExporterBenchmarks
{
    private readonly LogRecord logRecord;
    private readonly Batch<LogRecord> batch;
    private readonly MsgPackLogExporter msgPackExporter;
    private readonly TldLogExporter tldExporter;
    private readonly ILoggerFactory loggerFactoryForMsgPack;

    public TLDLogExporterBenchmarks()
    {
        this.msgPackExporter = new MsgPackLogExporter(
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

        this.tldExporter = new TldLogExporter(new GenevaExporterOptions()
        {
            ConnectionString = "EtwSession=OpenTelemetry;PrivatePreviewEnableTraceLoggingDynamic=true",
            PrepopulatedFields = new Dictionary<string, object>
            {
                ["cloud.role"] = "BusyWorker",
                ["cloud.roleInstance"] = "CY1SCH030021417",
                ["cloud.roleVer"] = "9.0.15289.2",
            },
        });

        this.logRecord = GenerateTestLogRecord();
        this.batch = GenerateTestLogRecordBatch();

        this.loggerFactoryForMsgPack = LoggerFactory.Create(builder =>
            builder.AddOpenTelemetry(loggerOptions =>
            {
                loggerOptions.AddGenevaLogExporter(exporterOptions =>
                {
                    exporterOptions.ConnectionString = "EtwSession=OpenTelemetry";
                    exporterOptions.PrepopulatedFields = new Dictionary<string, object>
                    {
                        ["cloud.role"] = "BusyWorker",
                        ["cloud.roleInstance"] = "CY1SCH030021417",
                        ["cloud.roleVer"] = "9.0.15289.2",
                    };
                });
            }));
    }

    [Benchmark]
    public void MsgPack_SerializeLogRecord()
    {
        this.msgPackExporter.SerializeLogRecord(this.logRecord);
    }

    [Benchmark]
    public void TLD_SerializeLogRecord()
    {
        this.tldExporter.SerializeLogRecord(this.logRecord);
    }

    [Benchmark]
    public void MsgPack_ExportLogRecord()
    {
        this.msgPackExporter.Export(this.batch);
    }

    [Benchmark]
    public void TLD_ExportLogRecord()
    {
        this.tldExporter.Export(this.batch);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        this.batch.Dispose();
        this.loggerFactoryForMsgPack.Dispose();
        this.tldExporter.Dispose();
        this.msgPackExporter.Dispose();
    }

    private static LogRecord GenerateTestLogRecord()
    {
        var exportedItems = new List<LogRecord>(1);
        using var factory = LoggerFactory.Create(builder => builder
            .AddOpenTelemetry(loggerOptions =>
            {
                loggerOptions.AddInMemoryExporter(exportedItems);
            }));

        var logger = factory.CreateLogger("TestLogger");
        logger.LogInformation("Hello from {Food} {Price}.", "artichoke", 3.99);
        return exportedItems[0];
    }

    private static Batch<LogRecord> GenerateTestLogRecordBatch()
    {
        var items = new List<LogRecord>(1);
        using var batchGeneratorExporter = new BatchGeneratorExporter();
        using var factory = LoggerFactory.Create(builder => builder
            .AddOpenTelemetry(loggerOptions =>
            {
                loggerOptions.AddProcessor(new SimpleLogRecordExportProcessor(batchGeneratorExporter));
            }));

        var logger = factory.CreateLogger("TestLogger");
        logger.LogInformation("Hello from {Food} {Price}.", "artichoke", 3.99);
        return batchGeneratorExporter.Batch;
    }

    private class BatchGeneratorExporter : BaseExporter<LogRecord>
    {
        public Batch<LogRecord> Batch { get; set; }

        public override ExportResult Export(in Batch<LogRecord> batch)
        {
            this.Batch = batch;
            return ExportResult.Success;
        }
    }
}
