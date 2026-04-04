// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter.Geneva.MsgPack;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

/*
BenchmarkDotNet v0.13.10, Windows 11 (10.0.23424.1000)
Intel Core i7-9700 CPU 3.00GHz, 1 CPU, 8 logical and 8 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


| Method                    | IncludeFormattedMessage | Mean       | Error    | StdDev   | Median     | Gen0   | Allocated |
|-------------------------- |------------------------ |-----------:|---------:|---------:|-----------:|-------:|----------:|
| LoggerWithMessageTemplate | False                   | 1,119.7 ns | 22.42 ns | 29.15 ns | 1,110.3 ns | 0.0153 |     104 B |
| LoggerWithDirectLoggerAPI | False                   | 1,358.0 ns | 26.31 ns | 33.28 ns | 1,346.6 ns | 0.0496 |     320 B |
| LoggerWithSourceGenerator | False                   | 1,132.1 ns | 22.30 ns | 18.62 ns | 1,133.8 ns | 0.0095 |      64 B |
| SerializeLogRecord        | False                   |   455.0 ns |  6.39 ns |  5.97 ns |   454.3 ns |      - |         - |
| Export                    | False                   | 1,005.3 ns | 19.99 ns | 36.55 ns |   998.9 ns |      - |         - |
| LoggerWithMessageTemplate | True                    | 1,153.7 ns | 16.13 ns | 14.30 ns | 1,153.6 ns | 0.0153 |     104 B |
| LoggerWithDirectLoggerAPI | True                    | 1,342.0 ns | 26.39 ns | 29.33 ns | 1,335.7 ns | 0.0496 |     320 B |
| LoggerWithSourceGenerator | True                    | 1,182.3 ns | 22.21 ns | 19.69 ns | 1,181.2 ns | 0.0095 |      64 B |
| SerializeLogRecord        | True                    |   455.6 ns |  8.28 ns | 15.56 ns |   450.5 ns |      - |         - |
| Export                    | True                    |   971.7 ns | 19.47 ns | 44.75 ns |   947.5 ns |      - |         - |
*/

namespace OpenTelemetry.Exporter.Geneva.Benchmarks;

[MemoryDiagnoser]
public class LogExporterBenchmarks
{
    private readonly ILogger logger;
    private readonly ILoggerFactory loggerFactory;
    private readonly MsgPackLogExporter exporter;
    private readonly LogRecord logRecord;
    private readonly Batch<LogRecord> batch;

    [Params(true, false)]
    public bool IncludeFormattedMessage { get; set; }

    public LogExporterBenchmarks()
    {
        // for total cost of logging + msgpack serialization + export
        this.loggerFactory = LoggerFactory.Create(builder => builder
            .AddOpenTelemetry(loggerOptions =>
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
                    exporterOptions.TableNameMappings = new Dictionary<string, string>
                    {
                        ["*"] = "*",
                        ["TestCompany"] = "*",
                        ["TestCompany.TestNamespace"] = "*",
                        ["TestCompany.TestNamespace.TestLogger"] = "TestLoggerTable",
                    };
                });

                loggerOptions.IncludeFormattedMessage = this.IncludeFormattedMessage;
            }));

        this.logger = this.loggerFactory.CreateLogger("TestCompany.TestNamespace.TestLogger");

        // For msgpack serialization + export
        this.logRecord = GenerateTestLogRecord();
        this.batch = GenerateTestLogRecordBatch();
        this.exporter = new MsgPackLogExporter(
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
    }

    [Benchmark]
    public void LoggerWithMessageTemplate()
    {
        this.logger.LogInformation("Hello from {Food} {Price}.", "artichoke", 3.99);
    }

    [Benchmark]
    public void LoggerWithDirectLoggerAPI()
    {
        var food = "artichoke";
        var price = 3.99;
        this.logger.Log(
            logLevel: LogLevel.Information,
            eventId: default,
            state: new List<KeyValuePair<string, object>>()
            {
                new("food", food),
                new("price", price),
            },
            exception: null,
            formatter: (state, ex) => $"Hello from {food} {price}.");
    }

    [Benchmark]
    public void LoggerWithSourceGenerator()
    {
        Food.SayHello(this.logger, "artichoke", 3.99);
    }

    [Benchmark]
    public void SerializeLogRecord()
    {
        this.exporter.SerializeLogRecord(this.logRecord);
    }

    [Benchmark]
    public void Export()
    {
        this.exporter.Export(this.batch);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        this.loggerFactory.Dispose();
        this.batch.Dispose();
        this.exporter.Dispose();
    }

    private static LogRecord GenerateTestLogRecord()
    {
        var items = new List<LogRecord>(1);
        using var factory = LoggerFactory.Create(builder => builder
            .AddOpenTelemetry(loggerOptions =>
            {
                loggerOptions.AddInMemoryExporter(items);
            }));

        var logger = factory.CreateLogger("TestLogger");
        logger.LogInformation("Hello from {Food} {Price}.", "artichoke", 3.99);
        return items[0];
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
