// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using Xunit;

namespace OpenTelemetry.Exporter.OneCollector.Tests;

public class LogRecordCommonSchemaJsonSerializerTests
{
    [Fact]
    public void EmptyLogRecordJsonTest()
    {
        var json = GetLogRecordJson(1, (index, logRecord) => { });

        Assert.Equal(
            """{"ver":"4.0","name":"Namespace.Name","time":"2032-01-18T10:11:12Z","iKey":"o:tenant-token","data":{"severityText":"Trace","severityNumber":1}}""" + "\n",
            json);
    }

    [Fact]
    public void MultipleEmptyLogRecordJsonTest()
    {
        var json = GetLogRecordJson(2, (index, logRecord) => { });

        Assert.Equal(
            """{"ver":"4.0","name":"Namespace.Name","time":"2032-01-18T10:11:12Z","iKey":"o:tenant-token","data":{"severityText":"Trace","severityNumber":1}}""" + "\n"
            + """{"ver":"4.0","name":"Namespace.Name","time":"2032-01-18T10:11:12Z","iKey":"o:tenant-token","data":{"severityText":"Trace","severityNumber":1}}""" + "\n",
            json);
    }

    [Theory]
    [InlineData(LogLevel.Trace, "Trace", 1)]
    [InlineData(LogLevel.Debug, "Debug", 5)]
    [InlineData(LogLevel.Information, "Information", 9)]
    [InlineData(LogLevel.Warning, "Warning", 13)]
    [InlineData(LogLevel.Error, "Error", 17)]
    [InlineData(LogLevel.Critical, "Critical", 21)]
    [InlineData(LogLevel.None, "Trace", 1)]
    public void LogRecordLogLevelJsonTest(LogLevel logLevel, string severityText, int severityNumber)
    {
        var json = GetLogRecordJson(1, (index, logRecord) =>
        {
#pragma warning disable CS0618 // Type or member is obsolete
            // TODO: Update to use LogRecord.Severity
            logRecord.LogLevel = logLevel;
#pragma warning restore CS0618 // Type or member is obsolete
        });

        Assert.Equal(
            $$$"""{"ver":"4.0","name":"Namespace.Name","time":"2032-01-18T10:11:12Z","iKey":"o:tenant-token","data":{"severityText":"{{{severityText}}}","severityNumber":{{{severityNumber}}}}}""" + "\n",
            json);
    }

    [Theory]
    [InlineData("MyClass.Company", null)]
    [InlineData("MyClass.Company", "MyEvent")]
    public void LogRecordCategoryNameAndEventNameJsonTest(string categoryName, string? eventName)
    {
        var json = GetLogRecordJson(1, (index, logRecord) =>
        {
            logRecord.CategoryName = categoryName;
            logRecord.EventId = new(0, eventName);
        });

        Assert.Equal(
            $$$"""{"ver":"4.0","name":"{{{categoryName}}}.{{{eventName ?? "Name"}}}","time":"2032-01-18T10:11:12Z","iKey":"o:tenant-token","data":{"severityText":"Trace","severityNumber":1}}""" + "\n",
            json);
    }

    [Theory]
    [InlineData("MyClass.Company", null, "MyNewEvent")]
    [InlineData("MyClass.Company", "MyEvent", "MyNewEvent")]
    [InlineData("MyClass.OtherCompany", "MyEvent", "MyDefaultEvent")]
    [InlineData("NotMapped", null, "Namespace.Name")]
    public void EventFullNameMappedJsonTest(string categoryName, string? eventName, string expectedEventFullName)
    {
        var eventFullNameMappings = new Dictionary<string, EventFullName>
        {
            { "MyClass.Company", EventFullName.Create("MyNewEvent") },
            { "MyClass", EventFullName.Create("MyDefaultEvent") },
        };

        var json = GetLogRecordJson(
            1,
            (index, logRecord) =>
            {
                logRecord.CategoryName = categoryName;
                logRecord.EventId = new(0, eventName);
            },
            eventFullNameMappings: eventFullNameMappings);

        var expectedName = eventName == null
            ? string.Empty
            : $"\"name\":\"{eventName}\",";

        Assert.Equal(
            $$$"""{"ver":"4.0","name":"{{{expectedEventFullName}}}","time":"2032-01-18T10:11:12Z","iKey":"o:tenant-token","data":{"namespace":"{{{categoryName}}}",{{{expectedName}}}"severityText":"Trace","severityNumber":1}}""" + "\n",
            json);
    }

    [Fact]
    public void LogRecordEventIdJsonTest()
    {
        var json = GetLogRecordJson(1, (index, logRecord) =>
        {
            logRecord.EventId = new(18);
        });

        Assert.Equal(
            """{"ver":"4.0","name":"Namespace.Name","time":"2032-01-18T10:11:12Z","iKey":"o:tenant-token","data":{"eventId":18,"severityText":"Trace","severityNumber":1}}""" + "\n",
            json);
    }

    [Fact]
    public void LogRecordTimestampJsonTest()
    {
        var json = GetLogRecordJson(1, (index, logRecord) =>
        {
            logRecord.Timestamp = DateTime.SpecifyKind(new DateTime(2023, 1, 18, 10, 18, 0), DateTimeKind.Utc);
        });

        Assert.Equal(
            """{"ver":"4.0","name":"Namespace.Name","time":"2023-01-18T10:18:00Z","iKey":"o:tenant-token","data":{"severityText":"Trace","severityNumber":1}}""" + "\n",
            json);
    }

    [Fact]
    public void LogRecordOriginalFormatBodyJsonTest()
    {
        var json = GetLogRecordJson(1, (index, logRecord) =>
        {
            logRecord.Attributes = [new KeyValuePair<string, object?>("{OriginalFormat}", "hello world")];
            logRecord.FormattedMessage = "goodbye world";
        });

        Assert.Equal(
            """{"ver":"4.0","name":"Namespace.Name","time":"2032-01-18T10:11:12Z","iKey":"o:tenant-token","data":{"severityText":"Trace","severityNumber":1,"body":"hello world","formattedMessage":"goodbye world"}}""" + "\n",
            json);
    }

    [Fact]
    public void LogRecordBodyJsonTest()
    {
        var json = GetLogRecordJson(1, (index, logRecord) =>
        {
            logRecord.Body = "hello world";
            logRecord.FormattedMessage = "goodbye world";
        });

        Assert.Equal(
            """{"ver":"4.0","name":"Namespace.Name","time":"2032-01-18T10:11:12Z","iKey":"o:tenant-token","data":{"severityText":"Trace","severityNumber":1,"body":"hello world","formattedMessage":"goodbye world"}}""" + "\n",
            json);
    }

    [Fact]
    public void LogRecordFormattedMessageBodyJsonTest()
    {
        var json = GetLogRecordJson(1, (index, logRecord) =>
        {
            logRecord.FormattedMessage = "goodbye world";
        });

        Assert.Equal(
            """{"ver":"4.0","name":"Namespace.Name","time":"2032-01-18T10:11:12Z","iKey":"o:tenant-token","data":{"severityText":"Trace","severityNumber":1,"body":"goodbye world","formattedMessage":"goodbye world"}}""" + "\n",
            json);
    }

    [Fact]
    public void LogRecordResourceJsonTest()
    {
        var resource = ResourceBuilder.CreateEmpty()
            .AddAttributes(new Dictionary<string, object>
            {
                ["resourceKey1"] = "resourceValue1",
                ["resourceKey2"] = "resourceValue2",
            })
            .Build();

        var json = GetLogRecordJson(1, (index, logRecord) => { }, resource);

        Assert.Equal(
            """{"ver":"4.0","name":"Namespace.Name","time":"2032-01-18T10:11:12Z","iKey":"o:tenant-token","data":{"severityText":"Trace","severityNumber":1,"resourceKey1":"resourceValue1","resourceKey2":"resourceValue2"}}""" + "\n",
            json);
    }

    [Fact]
    public void LogRecordScopesJsonTest()
    {
        var scopeProvider = new ScopeProvider(
            [
                new KeyValuePair<string, object?>("scope1Key1", "scope1Value1"),
                new KeyValuePair<string, object?>("scope1Key2", "scope1Value2")
            ],
            [new KeyValuePair<string, object?>("scope2Key1", "scope2Value1")]);

        var json = GetLogRecordJson(1, (index, logRecord) => { }, scopeProvider: scopeProvider);

        Assert.Equal(
            """{"ver":"4.0","name":"Namespace.Name","time":"2032-01-18T10:11:12Z","iKey":"o:tenant-token","data":{"severityText":"Trace","severityNumber":1,"scope1Key1":"scope1Value1","scope1Key2":"scope1Value2","scope2Key1":"scope2Value1"}}""" + "\n",
            json);
    }

    [Fact]
    public void LogRecordAttributesJsonTest()
    {
        var json = GetLogRecordJson(1, (index, logRecord) =>
        {
            logRecord.Attributes =
            [
                new KeyValuePair<string, object?>("stateKey1", "stateValue1"),
                new KeyValuePair<string, object?>("stateKey2", "stateValue2")
            ];
        });

        Assert.Equal(
            """{"ver":"4.0","name":"Namespace.Name","time":"2032-01-18T10:11:12Z","iKey":"o:tenant-token","data":{"severityText":"Trace","severityNumber":1,"stateKey1":"stateValue1","stateKey2":"stateValue2"}}""" + "\n",
            json);
    }

    [Fact]
    public void LogRecordAttributesWithEventFullNameJsonTest()
    {
        string json = GetLogRecordJson(1, (index, logRecord) =>
        {
            logRecord.Attributes = new List<KeyValuePair<string, object?>>
            {
                new KeyValuePair<string, object?>("{EventFullName}", "company_Product_EventName"),
                new KeyValuePair<string, object?>("stateKey1", "stateValue1"),
                new KeyValuePair<string, object?>("stateKey2", "stateValue2"),
            };
        });

        Assert.Equal(
            """{"ver":"4.0","name":"Company_Product_EventName","time":"2032-01-18T10:11:12Z","iKey":"o:tenant-token","data":{"severityText":"Trace","severityNumber":1,"stateKey1":"stateValue1","stateKey2":"stateValue2"}}""" + "\n",
            json);
    }

    [Fact]
    public void LogRecordTraceContextJsonTest()
    {
        var traceId = ActivityTraceId.CreateRandom();
        var spanId = ActivitySpanId.CreateRandom();

        var json = GetLogRecordJson(1, (index, logRecord) =>
        {
            logRecord.TraceId = traceId;
            logRecord.SpanId = spanId;
            logRecord.TraceFlags = ActivityTraceFlags.Recorded;
        });

        Assert.Equal(
            $$$$"""
            {"ver":"4.0","name":"Namespace.Name","time":"2032-01-18T10:11:12Z","iKey":"o:tenant-token","data":{"severityText":"Trace","severityNumber":1},"ext":{"dt":{"traceId":"{{{{traceId}}}}","spanId":"{{{{spanId}}}}"}}}
            """ + "\n",
            json);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void LogRecordExceptionJsonTest(bool includeStackTraceAsString)
    {
        var json = GetLogRecordJson(
            1,
            (index, logRecord) =>
            {
                logRecord.Exception = new InvalidOperationException();
            },
            includeStackTraceAsString: includeStackTraceAsString);

        var stackJson = includeStackTraceAsString
            ? ",\"stack\":\"System.InvalidOperationException: Operation is not valid due to the current state of the object.\""
            : string.Empty;

        Assert.Equal(
            $$$$"""
            {"ver":"4.0","name":"Namespace.Name","time":"2032-01-18T10:11:12Z","iKey":"o:tenant-token","data":{"severityText":"Trace","severityNumber":1},"ext":{"ex":{"type":"System.InvalidOperationException","msg":"Operation is not valid due to the current state of the object."{{{{stackJson}}}}}}}
            """ + "\n",
            json);
    }

    [Fact]
    public void LogRecordExtensionsJsonTest()
    {
        var scopeProvider = new ScopeProvider(
            [new KeyValuePair<string, object?>("ext.scope.field", "scopeValue1")]);

        var resource = ResourceBuilder.CreateEmpty()
            .AddAttributes(new Dictionary<string, object>
            {
                ["ext.resource.field"] = "resourceValue1",
            })
            .Build();

        var json = GetLogRecordJson(
            2,
            (index, logRecord) =>
            {
                logRecord.Attributes = [new KeyValuePair<string, object?>("ext.state.field", "stateValue1")];
            },
            resource,
            scopeProvider);

        Assert.Equal(
            """{"ver":"4.0","name":"Namespace.Name","time":"2032-01-18T10:11:12Z","iKey":"o:tenant-token","data":{"severityText":"Trace","severityNumber":1},"ext":{"state":{"field":"stateValue1"},"resource":{"field":"resourceValue1"},"scope":{"field":"scopeValue1"}}}""" + "\n"
            + """{"ver":"4.0","name":"Namespace.Name","time":"2032-01-18T10:11:12Z","iKey":"o:tenant-token","data":{"severityText":"Trace","severityNumber":1},"ext":{"state":{"field":"stateValue1"},"resource":{"field":"resourceValue1"},"scope":{"field":"scopeValue1"}}}""" + "\n",
            json);
    }

    private static string GetLogRecordJson(
        int numberOfLogRecords,
        Action<int, LogRecord> writeLogRecordCallback,
        Resource? resource = null,
        ScopeProvider? scopeProvider = null,
        bool includeStackTraceAsString = false,
        IReadOnlyDictionary<string, EventFullName>? eventFullNameMappings = null)
    {
        var serializer = new LogRecordCommonSchemaJsonSerializer(
            new EventNameManager("Namespace", "Name", eventFullNameMappings),
            "tenant-token",
            exceptionStackTraceHandling: includeStackTraceAsString ? OneCollectorExporterSerializationExceptionStackTraceHandlingType.IncludeAsString : OneCollectorExporterSerializationExceptionStackTraceHandlingType.Ignore);

        using var stream = new MemoryStream();

        var logRecords = new LogRecord[numberOfLogRecords];

        for (var i = 0; i < numberOfLogRecords; i++)
        {
            var logRecord = (LogRecord)Activator.CreateInstance(typeof(LogRecord), nonPublic: true)!;

            logRecord.Timestamp = DateTime.SpecifyKind(new DateTime(2032, 1, 18, 10, 11, 12), DateTimeKind.Utc);

            if (scopeProvider != null)
            {
                var logRecordILoggerDataType = typeof(LogRecord).Assembly.GetType("OpenTelemetry.Logs.LogRecord+LogRecordILoggerData")
                    ?? throw new InvalidOperationException("OpenTelemetry.Logs.LogRecord+LogRecordILoggerData could not be found reflectively.");

                var iLoggerDataField = typeof(LogRecord).GetField("ILoggerData", BindingFlags.Instance | BindingFlags.NonPublic)
                    ?? throw new InvalidOperationException("LogRecord.ILoggerData could not be found reflectively.");

                var scopeProviderField = logRecordILoggerDataType.GetField("ScopeProvider", BindingFlags.Instance | BindingFlags.Public)
                    ?? throw new InvalidOperationException("LogRecordILoggerData.ScopeProvider could not be found reflectively.");

                var iLoggerData = iLoggerDataField.GetValue(logRecord);

                scopeProviderField.SetValue(iLoggerData, scopeProvider);

                iLoggerDataField.SetValue(logRecord, iLoggerData);
            }

            writeLogRecordCallback(i, logRecord);
            logRecords[i] = logRecord;
        }

        var batch = new Batch<LogRecord>(logRecords, numberOfLogRecords);

        var state = new BatchSerializationState<LogRecord>(in batch);

        serializer.SerializeBatchOfItemsToStream(
            resource ?? Resource.Empty,
            ref state,
            stream,
            initialSizeOfPayloadInBytes: 0,
            out var result);

        return Encoding.UTF8.GetString(stream.ToArray());
    }

    private sealed class ScopeProvider : IExternalScopeProvider
    {
        private readonly List<KeyValuePair<string, object?>>[] scopes;

        public ScopeProvider(params List<KeyValuePair<string, object?>>[] scopes)
        {
            this.scopes = scopes;
        }

        public void ForEachScope<TState>(Action<object, TState> callback, TState state)
        {
            foreach (var scope in this.scopes)
            {
                callback(scope, state);
            }
        }

        public IDisposable Push(object? state)
        {
            throw new NotImplementedException();
        }
    }
}
