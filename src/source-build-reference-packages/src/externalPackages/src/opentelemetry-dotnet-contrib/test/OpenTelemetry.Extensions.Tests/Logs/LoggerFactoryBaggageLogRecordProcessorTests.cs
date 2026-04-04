// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Trace;
using Xunit;

namespace OpenTelemetry.Extensions.Tests.Logs;

public class LoggerFactoryBaggageLogRecordProcessorTests
{
    [Fact]
    public void BaggageLogRecordProcessor_CanAddAllowAllBaggageKeysPredicate()
    {
        var logRecordList = new List<LogRecord>();
        using var loggerFactory = LoggerFactory.Create(builder => builder
        .AddOpenTelemetry(options =>
        {
            options.AddBaggageProcessor();
            options.AddInMemoryExporter(logRecordList);
        })
        .AddFilter("*", LogLevel.Trace)); // Enable all LogLevels
        Baggage.SetBaggage("allow", "value");

        var logger = loggerFactory.CreateLogger(GetTestMethodName());
        logger.LogError("this does not matter");
        var logRecord = Assert.Single(logRecordList);
        Assert.NotNull(logRecord);
        Assert.NotNull(logRecord.Attributes);
        Assert.Contains(logRecord.Attributes, kv => kv.Key == "allow");
    }

    [Fact]
    public void BaggageLogRecordProcessor_CanUseCustomPredicate()
    {
        var logRecordList = new List<LogRecord>();
        using var loggerFactory = LoggerFactory.Create(builder => builder
        .AddOpenTelemetry(options =>
        {
            options.AddBaggageProcessor((baggageKey) => baggageKey.StartsWith("allow", StringComparison.Ordinal));
            options.AddInMemoryExporter(logRecordList);
        })
        .AddFilter("*", LogLevel.Trace)); // Enable all LogLevels
        Baggage.SetBaggage("allow", "value");
        Baggage.SetBaggage("deny", "other_value");

        var logger = loggerFactory.CreateLogger(GetTestMethodName());
        logger.LogError("this does not matter");
        var logRecord = Assert.Single(logRecordList);
        Assert.NotNull(logRecord);
        Assert.NotNull(logRecord.Attributes);
        Assert.Contains(logRecord.Attributes, kv => kv.Key == "allow");
        Assert.DoesNotContain(logRecord.Attributes, kv => kv.Key == "deny");
    }

    [Fact]
    public void BaggageLogRecordProcessor_CanUseRegex()
    {
        var regex = new Regex("^allow", RegexOptions.Compiled);
        var logRecordList = new List<LogRecord>();
        using var loggerFactory = LoggerFactory.Create(builder => builder
        .AddOpenTelemetry(options =>
        {
            options.AddBaggageProcessor(regex.IsMatch);
            options.AddInMemoryExporter(logRecordList);
        })
        .AddFilter("*", LogLevel.Trace)); // Enable all LogLevels
        Baggage.SetBaggage("allow", "value");
        Baggage.SetBaggage("deny", "other_value");

        var logger = loggerFactory.CreateLogger(GetTestMethodName());
        logger.LogError("this does not matter");
        var logRecord = Assert.Single(logRecordList);
        Assert.NotNull(logRecord);
        Assert.NotNull(logRecord.Attributes);
        Assert.Contains(logRecord.Attributes, kv => kv.Key == "allow");
        Assert.DoesNotContain(logRecord.Attributes, kv => kv.Key == "deny");
    }

    [Fact]
    public void BaggageLogRecordProcessor_PredicateThrows_DoesNothing()
    {
        var logRecordList = new List<LogRecord>();
        using var loggerFactory = LoggerFactory.Create(builder => builder
        .AddOpenTelemetry(options =>
        {
            options.AddBaggageProcessor(_ => throw new Exception("Predicate throws an exception."));
            options.AddInMemoryExporter(logRecordList);
        })
        .AddFilter("*", LogLevel.Trace)); // Enable all LogLevels
        Baggage.SetBaggage("deny", "value");

        var logger = loggerFactory.CreateLogger(GetTestMethodName());
        logger.LogError("this does not matter");
        var logRecord = Assert.Single(logRecordList);
        Assert.NotNull(logRecord);
        Assert.DoesNotContain(logRecord?.Attributes ?? [], kv => kv.Key == "deny");
    }

    [Fact]
    public void BaggageLogRecordProcessor_PredicateThrows_OnlyDropsEntriesThatThrow()
    {
        var logRecordList = new List<LogRecord>();
        using var loggerFactory = LoggerFactory.Create(builder => builder
        .AddOpenTelemetry(options =>
        {
            options.AddBaggageProcessor(key =>
            {
                return key != "allow" ? throw new Exception("Predicate throws an exception.") : true;
            });
            options.AddInMemoryExporter(logRecordList);
        })
        .AddFilter("*", LogLevel.Trace)); // Enable all LogLevels
        Baggage.SetBaggage("allow", "value");
        Baggage.SetBaggage("deny", "value");
        Baggage.SetBaggage("deny_2", "value");

        var logger = loggerFactory.CreateLogger(GetTestMethodName());
        logger.LogError("this does not matter");
        var logRecord = Assert.Single(logRecordList);
        Assert.NotNull(logRecord);
        Assert.NotNull(logRecord.Attributes);
        Assert.Contains(logRecord.Attributes, kv => kv.Key == "allow");
        Assert.DoesNotContain(logRecord?.Attributes ?? [], kv => kv.Key == "deny");
    }

    private static string GetTestMethodName([CallerMemberName] string callingMethodName = "")
    {
        return callingMethodName;
    }
}
