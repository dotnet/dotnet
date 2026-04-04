// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using OpenTelemetry.Instrumentation.SqlClient.Implementation;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Xunit;

namespace OpenTelemetry.Instrumentation.SqlClient.Tests;

[Collection("SqlClient")]
public class SqlEventSourceTests
{
    public static TheoryData<Type, CommandType, string, bool, int, bool, bool> EventSourceFakeTestCases()
    {
        /* netfx driver can't capture queries, only stored procedure names */
        /* always emit some attribute */
        var bools = new[] { true, false };
        var query =
            from eventSourceType in new[] { typeof(FakeBehavingAdoNetSqlEventSource), typeof(FakeBehavingMdsSqlEventSource) }
            from commandType in new[] { CommandType.StoredProcedure, CommandType.Text }
            from isFailure in bools
            from tracingEnabled in bools
            from metricsEnabled in bools
            where !(commandType == CommandType.Text)
            let commandText = commandType == CommandType.Text
                ? (!isFailure ? "select 1/1" : "select 1/0")
                : "sp_who"
            let sqlExceptionNumber = 0
            select new
            {
                eventSourceType,
                commandType,
                commandText,
                isFailure,
                sqlExceptionNumber,
                tracingEnabled,
                metricsEnabled,
            };

        var testCases = new TheoryData<Type, CommandType, string, bool, int, bool, bool>();

        foreach (var item in query)
        {
            testCases.Add(
                item.eventSourceType,
                item.commandType,
                item.commandText,
                item.isFailure,
                item.sqlExceptionNumber,
                item.tracingEnabled,
                item.metricsEnabled);
        }

        return testCases;
    }

    [Theory]
    [MemberData(nameof(EventSourceFakeTestCases))]
    public void EventSourceFakeTests(
        Type eventSourceType,
        CommandType commandType,
        string commandText,
        bool isFailure,
        int sqlExceptionNumber,
        bool tracingEnabled,
        bool metricsEnabled)
    {
        using var fakeSqlEventSource = (IFakeBehavingSqlEventSource)Activator.CreateInstance(eventSourceType);

        var activities = new List<Activity>();
        var metrics = new List<Metric>();

        var traceProviderBuilder = Sdk.CreateTracerProviderBuilder();

        if (tracingEnabled)
        {
            traceProviderBuilder.AddInMemoryExporter(activities)
                .AddSqlClientInstrumentation();
        }

        var meterProviderBuilder = Sdk.CreateMeterProviderBuilder();

        if (metricsEnabled)
        {
            meterProviderBuilder.AddInMemoryExporter(metrics)
                .AddSqlClientInstrumentation();
        }

        using var traceProvider = traceProviderBuilder.Build();
        using var meterProvider = meterProviderBuilder.Build();

        var objectId = Guid.NewGuid().GetHashCode();
        var dataSource = "127.0.0.1\\instanceName,port";

        try
        {
            fakeSqlEventSource.WriteBeginExecuteEvent(objectId, dataSource, "master", commandType == CommandType.StoredProcedure ? commandText : string.Empty);

            // success is stored in the first bit in compositeState 0b001
            var successFlag = !isFailure ? 1 : 0;

            // isSqlException is stored in the second bit in compositeState 0b010
            var isSqlExceptionFlag = sqlExceptionNumber > 0 ? 2 : 0;

            // synchronous state is stored in the third bit in compositeState 0b100
            var synchronousFlag = false ? 4 : 0;

            var compositeState = successFlag | isSqlExceptionFlag | synchronousFlag;

            fakeSqlEventSource.WriteEndExecuteEvent(objectId, compositeState, sqlExceptionNumber);
        }
        finally
        {
            traceProvider.Dispose();
            Assert.True(meterProvider.ForceFlush());
        }

        Activity? activity = null;

        if (tracingEnabled)
        {
            activity = Assert.Single(activities);
            VerifyActivityData(commandText, isFailure, dataSource, activity);
        }

        var dbClientOperationDurationMetrics = metrics
            .Where(metric => metric.Name == "db.client.operation.duration")
            .ToArray();

        if (metricsEnabled)
        {
            var metric = Assert.Single(dbClientOperationDurationMetrics);
            Assert.NotNull(metric);
            Assert.Equal("s", metric.Unit);
            Assert.Equal(MetricType.Histogram, metric.MetricType);

            var metricPoints = new List<MetricPoint>();
            foreach (var p in metric.GetMetricPoints())
            {
                metricPoints.Add(p);
            }

            var metricPoint = Assert.Single(metricPoints);

            if (activity != null)
            {
                var count = metricPoint.GetHistogramCount();
                var sum = metricPoint.GetHistogramSum();
                Assert.Equal(activity.Duration.TotalSeconds, sum);
            }
        }
        else
        {
            Assert.Empty(dbClientOperationDurationMetrics);
        }
    }

    [Theory]
    [InlineData(typeof(FakeMisbehavingAdoNetSqlEventSource))]
    [InlineData(typeof(FakeMisbehavingMdsSqlEventSource))]
    public void EventSourceFakeUnknownEventWithNullPayloadTest(Type eventSourceType)
    {
        using var fakeSqlEventSource = (IFakeMisbehavingSqlEventSource)Activator.CreateInstance(eventSourceType);

        var exportedItems = new List<Activity>();
        using var shutdownSignal = Sdk.CreateTracerProviderBuilder()
            .AddInMemoryExporter(exportedItems)
            .AddSqlClientInstrumentation()
            .Build();

        fakeSqlEventSource.WriteUnknownEventWithNullPayload();

        shutdownSignal.Dispose();

        Assert.Empty(exportedItems);
    }

    [Theory]
    [InlineData(typeof(FakeMisbehavingAdoNetSqlEventSource))]
    [InlineData(typeof(FakeMisbehavingMdsSqlEventSource))]
    public void EventSourceFakeInvalidPayloadTest(Type eventSourceType)
    {
        using var fakeSqlEventSource = (IFakeMisbehavingSqlEventSource)Activator.CreateInstance(eventSourceType);

        var exportedItems = new List<Activity>();
        using var shutdownSignal = Sdk.CreateTracerProviderBuilder()
            .AddInMemoryExporter(exportedItems)
            .AddSqlClientInstrumentation()
            .Build();

        fakeSqlEventSource.WriteBeginExecuteEvent("arg1");

        fakeSqlEventSource.WriteEndExecuteEvent("arg1", "arg2", "arg3", "arg4");
        shutdownSignal.Dispose();

        Assert.Empty(exportedItems);
    }

    private static void VerifyActivityData(
        string commandText,
        bool isFailure,
        string dataSource,
        Activity activity)
    {
        Assert.Equal("instanceName.master", activity.DisplayName);

        Assert.Equal(ActivityKind.Client, activity.Kind);

        var connectionDetails = SqlConnectionDetails.ParseFromDataSource(dataSource);

        if (!string.IsNullOrEmpty(connectionDetails.ServerHostName))
        {
            Assert.Equal(connectionDetails.ServerHostName, activity.GetTagValue(SemanticConventions.AttributeServerAddress));
        }
        else
        {
            Assert.Equal(connectionDetails.ServerIpAddress, activity.GetTagValue(SemanticConventions.AttributeServerAddress));
        }

        if (connectionDetails.Port.HasValue)
        {
            Assert.Equal(connectionDetails.Port, activity.GetTagValue(SemanticConventions.AttributeServerPort));
        }

        Assert.Equal(SqlTelemetryHelper.MicrosoftSqlServerDbSystemName, activity.GetTagValue(SemanticConventions.AttributeDbSystemName));
        Assert.Equal("instanceName.master", activity.GetTagValue(SemanticConventions.AttributeDbNamespace));
        Assert.Equal(commandText, activity.GetTagValue(SemanticConventions.AttributeDbQueryText));

        if (!isFailure)
        {
            Assert.Equal(ActivityStatusCode.Unset, activity.Status);
        }
        else
        {
            Assert.Equal(ActivityStatusCode.Error, activity.Status);
            Assert.NotNull(activity.StatusDescription);
        }
    }

#pragma warning disable SA1201 // Elements should appear in the correct order
    // Helper interface to be able to have single test method for multiple EventSources, want to keep it close to the event sources themselves.
    private interface IFakeBehavingSqlEventSource : IDisposable
#pragma warning restore SA1201 // Elements should appear in the correct order
    {
        void WriteBeginExecuteEvent(int objectId, string dataSource, string databaseName, string commandText);

        void WriteEndExecuteEvent(int objectId, int compositeState, int sqlExceptionNumber);
    }

    private interface IFakeMisbehavingSqlEventSource : IDisposable
    {
        void WriteBeginExecuteEvent(string arg1);

        void WriteEndExecuteEvent(string arg1, string arg2, string arg3, string arg4);

        void WriteUnknownEventWithNullPayload();
    }

    [EventSource(Name = SqlEventSourceListener.AdoNetEventSourceName + "-FakeFriendly")]
    private class FakeBehavingAdoNetSqlEventSource : EventSource, IFakeBehavingSqlEventSource
    {
        [Event(SqlEventSourceListener.BeginExecuteEventId)]
        public void WriteBeginExecuteEvent(int objectId, string dataSource, string databaseName, string commandText)
        {
            this.WriteEvent(SqlEventSourceListener.BeginExecuteEventId, objectId, dataSource, databaseName, commandText);
        }

        [Event(SqlEventSourceListener.EndExecuteEventId)]
        public void WriteEndExecuteEvent(int objectId, int compositeState, int sqlExceptionNumber)
        {
            this.WriteEvent(SqlEventSourceListener.EndExecuteEventId, objectId, compositeState, sqlExceptionNumber);
        }
    }

    [EventSource(Name = SqlEventSourceListener.MdsEventSourceName + "-FakeFriendly")]
    private class FakeBehavingMdsSqlEventSource : EventSource, IFakeBehavingSqlEventSource
    {
        [Event(SqlEventSourceListener.BeginExecuteEventId)]
        public void WriteBeginExecuteEvent(int objectId, string dataSource, string databaseName, string commandText)
        {
            this.WriteEvent(SqlEventSourceListener.BeginExecuteEventId, objectId, dataSource, databaseName, commandText);
        }

        [Event(SqlEventSourceListener.EndExecuteEventId)]
        public void WriteEndExecuteEvent(int objectId, int compositeState, int sqlExceptionNumber)
        {
            this.WriteEvent(SqlEventSourceListener.EndExecuteEventId, objectId, compositeState, sqlExceptionNumber);
        }
    }

    [EventSource(Name = SqlEventSourceListener.AdoNetEventSourceName + "-FakeEvil")]
    private class FakeMisbehavingAdoNetSqlEventSource : EventSource, IFakeMisbehavingSqlEventSource
    {
        [Event(SqlEventSourceListener.BeginExecuteEventId)]
        public void WriteBeginExecuteEvent(string arg1)
        {
            this.WriteEvent(SqlEventSourceListener.BeginExecuteEventId, arg1);
        }

        [Event(SqlEventSourceListener.EndExecuteEventId)]
        public void WriteEndExecuteEvent(string arg1, string arg2, string arg3, string arg4)
        {
            this.WriteEvent(SqlEventSourceListener.EndExecuteEventId, arg1, arg2, arg3, arg4);
        }

        [Event(3)]
        public void WriteUnknownEventWithNullPayload()
        {
            object[]? args = null;

            this.WriteEvent(3, args);
        }
    }

    [EventSource(Name = SqlEventSourceListener.MdsEventSourceName + "-FakeEvil")]
    private class FakeMisbehavingMdsSqlEventSource : EventSource, IFakeMisbehavingSqlEventSource
    {
        [Event(SqlEventSourceListener.BeginExecuteEventId)]
        public void WriteBeginExecuteEvent(string arg1)
        {
            this.WriteEvent(SqlEventSourceListener.BeginExecuteEventId, arg1);
        }

        [Event(SqlEventSourceListener.EndExecuteEventId)]
        public void WriteEndExecuteEvent(string arg1, string arg2, string arg3, string arg4)
        {
            this.WriteEvent(SqlEventSourceListener.EndExecuteEventId, arg1, arg2, arg3, arg4);
        }

        [Event(3)]
        public void WriteUnknownEventWithNullPayload()
        {
            object[]? args = null;

            this.WriteEvent(3, args);
        }
    }
}
#endif
