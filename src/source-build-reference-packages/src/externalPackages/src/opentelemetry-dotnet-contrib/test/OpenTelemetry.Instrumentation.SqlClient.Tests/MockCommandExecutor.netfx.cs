// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using Microsoft.Data.SqlClient;
using OpenTelemetry.Instrumentation.SqlClient.Implementation;
using OpenTelemetry.Trace;
using Xunit;

namespace OpenTelemetry.Instrumentation.SqlClient.Tests;

[Collection("SqlClient")]
public class MockCommandExecutor
{
    private interface IFakeBehavingSqlEventSource : IDisposable
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

#pragma warning disable xUnit1013 // Public method should be marked as test
    public static void ExecuteCommand(string connectionString, CommandType commandType, string commandText, bool error, SqlClientLibrary library)
#pragma warning restore xUnit1013 // Public method should be marked as test
    {
        var eventSourceType = library == SqlClientLibrary.SystemDataSqlClient
            ? typeof(FakeBehavingAdoNetSqlEventSource)
            : typeof(FakeBehavingMdsSqlEventSource);

        using var fakeSqlEventSource = (IFakeBehavingSqlEventSource)Activator.CreateInstance(eventSourceType);

        using var sqlConnection = new SqlConnection(connectionString);
        using var sqlCommand = sqlConnection.CreateCommand();

        var objectId = Guid.NewGuid().GetHashCode();

        fakeSqlEventSource.WriteBeginExecuteEvent(objectId, sqlConnection.DataSource, "master", commandText);

        // success is stored in the first bit in compositeState 0b001
        var successFlag = !error ? 1 : 0;

        // isSqlException is stored in the second bit in compositeState 0b010
        var isSqlExceptionFlag = error ? 2 : 0;

        // synchronous state is stored in the third bit in compositeState 0b100
        var synchronousFlag = false ? 4 : 0;

        var compositeState = successFlag | isSqlExceptionFlag | synchronousFlag;

        fakeSqlEventSource.WriteEndExecuteEvent(objectId, compositeState, sqlExceptionNumber: 0);
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
