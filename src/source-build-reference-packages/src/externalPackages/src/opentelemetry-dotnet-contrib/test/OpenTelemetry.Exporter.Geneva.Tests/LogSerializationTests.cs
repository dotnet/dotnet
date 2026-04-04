// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#nullable disable

using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter.Geneva.MsgPack;
using OpenTelemetry.Logs;
using Xunit;

namespace OpenTelemetry.Exporter.Geneva.Tests;

public class LogSerializationTests
{
    /*
    Run from the current directory:
    dotnet test -f net8.0 --filter FullyQualifiedName~LogSerializationTests -l "console;verbosity=detailed"
    */
    [Fact]
    public void SerializationTestForException()
    {
        var exceptionMessage = "Exception Message";
        var exStack = "Exception StackTrace";
        var ex = new MyException(exceptionMessage, exStack);
        var exportedFields = GetExportedFieldsAfterLogging(
            logger =>
            {
                logger.Log<object>(
                            logLevel: LogLevel.Information,
                            eventId: default,
                            state: null,
                            exception: ex,
                            formatter: null);
            },
            (genevaOptions) =>
            genevaOptions.ExceptionStackExportMode = ExceptionStackExportMode.ExportAsString);

        var actualExceptionMessage = exportedFields["env_ex_msg"];
        Assert.Equal(exceptionMessage, actualExceptionMessage);

        var actualExceptionType = exportedFields["env_ex_type"];
        Assert.Equal(typeof(MyException).FullName, actualExceptionType);

        var actualExceptionStack = exportedFields["env_ex_stack"];
        Assert.Equal(exStack, actualExceptionStack);
    }

    [Fact]
    public void SerializationTestForExceptionTrim()
    {
        var exceptionMessage = "Exception Message";
        var exStack = new string('e', 16383 + 1);
        var ex = new MyException(exceptionMessage, exStack);
        var exportedFields = GetExportedFieldsAfterLogging(
            logger => logger.LogError(ex, "Error occurred. {Field1} {Field2}", "value1", "value2"),
            (genevaOptions) => genevaOptions.ExceptionStackExportMode = ExceptionStackExportMode.ExportAsString);

        var actualExceptionMessage = exportedFields["env_ex_msg"];
        Assert.Equal(exceptionMessage, actualExceptionMessage);

        var actualExceptionType = exportedFields["env_ex_type"];
        Assert.Equal(typeof(MyException).FullName, actualExceptionType);

        var actualExceptionStack = exportedFields["env_ex_stack"];
        Assert.EndsWith("...", (string)actualExceptionStack);

        var actualValue = exportedFields["Field1"];
        Assert.Equal("value1", actualValue);

        actualValue = exportedFields["Field2"];
        Assert.Equal("value2", actualValue);

        // PrintFields(this.output, exportedFields);
    }

    private static Dictionary<object, object> GetExportedFieldsAfterLogging(Action<ILogger> doLog, Action<GenevaExporterOptions> configureGeneva = null)
    {
        var path = string.Empty;
        try
        {
            var exporterOptions = new GenevaExporterOptions();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                exporterOptions.ConnectionString = "EtwSession=OpenTelemetry";
            }
            else
            {
                path = GenerateTempFilePath();
                exporterOptions.ConnectionString = "Endpoint=unix:" + path;
            }

            configureGeneva?.Invoke(exporterOptions);

            using var exporter = new GenevaLogExporter(exporterOptions);

            using var loggerFactory = LoggerFactory.Create(builder => builder
                    .AddOpenTelemetry(options =>
                    {
                        options.AddProcessor(new ReentrantExportProcessor<LogRecord>(exporter));
                    })
                    .AddFilter(typeof(LogSerializationTests).FullName, LogLevel.Trace)); // Enable all LogLevels

            List<ArraySegment<byte>> exportedData = [];
            (exporter.Exporter as MsgPackLogExporter).DataTransportListener = (data) => exportedData.Add(data);

            var logger = loggerFactory.CreateLogger<LogSerializationTests>();
            doLog(logger);

            Assert.Single(exportedData);
            var fluentdData = MessagePack.MessagePackSerializer.Deserialize<object>(exportedData[0], MessagePack.Resolvers.ContractlessStandardResolver.Options);

            return GetFields(fluentdData);
        }
        finally
        {
            try
            {
                File.Delete(path);
            }
            catch
            {
            }
        }
    }

    private static string GenerateTempFilePath()
    {
        while (true)
        {
            var path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            if (!File.Exists(path))
            {
                return path;
            }
        }
    }

    private static Dictionary<object, object> GetFields(object fluentdData)
    {
        /* Fluentd Forward Mode:
        [
            "Log",
            [
                [ <timestamp>, { "env_ver": "4.0", ... } ]
            ],
            { "TimeFormat": "DateTime" }
        ]
        */

        var TimeStampAndMappings = ((fluentdData as object[])[1] as object[])[0];
        var mapping = (TimeStampAndMappings as object[])[1] as Dictionary<object, object>;
        return mapping;
    }

    private class MyException : Exception
    {
        private readonly string stackTrace;

        public MyException(string message, string stackTrace)
        : base(message)
        {
            this.stackTrace = stackTrace;
        }

        public override string ToString()
        {
            return this.stackTrace;
        }
    }
}
