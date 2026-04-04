// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Data;
using System.Diagnostics;
#if NET
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
#endif
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Trace;
using Xunit;

namespace OpenTelemetry.Instrumentation.SqlClient.Tests;

[Collection("SqlClient")]
public class SqlClientTraceInstrumentationOptionsTests
{
    private const string TestConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Database=master;Encrypt=True;TrustServerCertificate=True";

    [Fact]
    public void SqlClient_NamedOptions()
    {
        var defaultExporterOptionsConfigureOptionsInvocations = 0;
        var namedExporterOptionsConfigureOptionsInvocations = 0;

        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .ConfigureServices(services =>
            {
                services.Configure<SqlClientTraceInstrumentationOptions>(o => defaultExporterOptionsConfigureOptionsInvocations++);

                services.Configure<SqlClientTraceInstrumentationOptions>("Instrumentation2", o => namedExporterOptionsConfigureOptionsInvocations++);
            })
            .AddSqlClientInstrumentation()
            .AddSqlClientInstrumentation("Instrumentation2", configureSqlClientTraceInstrumentationOptions: null)
            .Build();

        Assert.Equal(1, defaultExporterOptionsConfigureOptionsInvocations);
        Assert.Equal(1, namedExporterOptionsConfigureOptionsInvocations);
    }

    [Fact]
    public void DbQueryTextCollected()
    {
        var activities = new List<Activity>();

        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddSqlClientInstrumentation()
            .AddInMemoryExporter(activities)
            .Build();

        var commandText = "select * from sys.databases";
        MockCommandExecutor.ExecuteCommand(TestConnectionString, CommandType.Text, commandText, false, SqlClientLibrary.SystemDataSqlClient);
        MockCommandExecutor.ExecuteCommand(TestConnectionString, CommandType.Text, commandText, false, SqlClientLibrary.MicrosoftDataSqlClient);

        tracerProvider.ForceFlush();
        Assert.Equal(2, activities.Count);

        Assert.Equal(commandText, activities[0].GetTagValue(SemanticConventions.AttributeDbQueryText));
        Assert.Equal(commandText, activities[1].GetTagValue(SemanticConventions.AttributeDbQueryText));
    }

#if !NETFRAMEWORK
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ExceptionCapturedWhenRecordExceptionEnabled(bool recordException)
    {
        var activities = new List<Activity>();

        using var traceProvider = Sdk.CreateTracerProviderBuilder()
            .AddSqlClientInstrumentation(options =>
            {
                options.RecordException = recordException;
            })
            .AddInMemoryExporter(activities)
            .Build();

        MockCommandExecutor.ExecuteCommand(TestConnectionString, CommandType.StoredProcedure, "SP_GetOrders", true, SqlClientLibrary.SystemDataSqlClient);
        MockCommandExecutor.ExecuteCommand(TestConnectionString, CommandType.StoredProcedure, "SP_GetOrders", true, SqlClientLibrary.MicrosoftDataSqlClient);

        traceProvider.ForceFlush();

        Assert.Equal(2, activities.Count);

        Assert.Equal(ActivityStatusCode.Error, activities[0].Status);
        Assert.Equal(ActivityStatusCode.Error, activities[1].Status);
        Assert.NotNull(activities[0].StatusDescription);
        Assert.NotNull(activities[1].StatusDescription);

        if (recordException)
        {
            var events0 = activities[0].Events.ToList();
            var events1 = activities[1].Events.ToList();
            Assert.Single(events0);
            Assert.Single(events1);
            Assert.Equal(SemanticConventions.AttributeExceptionEventName, events0[0].Name);
            Assert.Equal(SemanticConventions.AttributeExceptionEventName, events1[0].Name);
        }
        else
        {
            Assert.Empty(activities[0].Events);
            Assert.Empty(activities[1].Events);
        }
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, false)]
    [InlineData(true, true)]
    [InlineData(false, true)]
    public void ShouldEnrichWhenEnabled(bool shouldEnrich, bool error)
    {
        var activities = new List<Activity>();

        using var traceProvider = Sdk.CreateTracerProviderBuilder()
            .AddSqlClientInstrumentation(
            (opt) =>
            {
                if (shouldEnrich)
                {
                    opt.EnrichWithSqlCommand = ActivityEnrichment;
                }
            })
            .AddInMemoryExporter(activities)
            .Build();

        MockCommandExecutor.ExecuteCommand(TestConnectionString, CommandType.Text, "SELECT * FROM Foo", error, SqlClientLibrary.SystemDataSqlClient);
        MockCommandExecutor.ExecuteCommand(TestConnectionString, CommandType.Text, "SELECT * FROM Foo", error, SqlClientLibrary.MicrosoftDataSqlClient);

        Assert.Equal(2, activities.Count);
        if (shouldEnrich)
        {
            Assert.Contains("enriched", activities[0].Tags.Select(x => x.Key));
            Assert.Contains("enriched", activities[1].Tags.Select(x => x.Key));
            Assert.Equal("yes", activities[0].Tags.FirstOrDefault(tag => tag.Key == "enriched").Value);
            Assert.Equal("yes", activities[1].Tags.FirstOrDefault(tag => tag.Key == "enriched").Value);
        }
        else
        {
            Assert.DoesNotContain(activities[0].Tags, tag => tag.Key == "enriched");
            Assert.DoesNotContain(activities[1].Tags, tag => tag.Key == "enriched");
        }
    }

    [Fact]
    public void ShouldCollectTelemetryWhenFilterEvaluatesToTrue()
    {
        var activities = this.RunCommandWithFilter(
            "select 2",
            cmd =>
            {
                return cmd is not SqlCommand command || command.CommandText == "select 2";
            });

        Assert.Single(activities);
        Assert.True(activities[0].IsAllDataRequested);
        Assert.True(activities[0].ActivityTraceFlags.HasFlag(ActivityTraceFlags.Recorded));
    }

    [Fact]
    public void ShouldNotCollectTelemetryWhenFilterEvaluatesToFalse()
    {
        var activities = this.RunCommandWithFilter(
            "select 1",
            cmd =>
            {
                return cmd is not SqlCommand command || command.CommandText == "select 2";
            });

        Assert.Empty(activities);
    }

    [Fact]
    public void ShouldNotCollectTelemetryAndShouldNotPropagateExceptionWhenFilterThrowsException()
    {
        var activities = this.RunCommandWithFilter(
            "select 1",
            cmd => throw new InvalidOperationException("foobar"));

        Assert.Empty(activities);
    }

    [Fact]
    public void ShouldNotEmitDatabaseQueryParametersByDefault()
    {
        var configuration = new ConfigurationBuilder().Build();
        var options = new SqlClientTraceInstrumentationOptions(configuration);
        Assert.False(options.SetDbQueryParameters);
    }

    [Theory]
    [InlineData("", false)]
    [InlineData("invalid", false)]
    [InlineData("false", false)]
    [InlineData("true", true)]
    public void ShouldAssignSetDatabaseQueryParametersFromEnvironmentVariable(string value, bool expected)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["OTEL_DOTNET_EXPERIMENTAL_SQLCLIENT_ENABLE_TRACE_DB_QUERY_PARAMETERS"] = value })
            .Build();
        var options = new SqlClientTraceInstrumentationOptions(configuration);
        Assert.Equal(expected, options.SetDbQueryParameters);
    }

    private static void ActivityEnrichment(Activity activity, object obj)
    {
        activity.SetTag("enriched", "yes");
        Assert.IsType<SqlCommand>(obj);
    }

    private Activity[] RunCommandWithFilter(
        string commandText,
        Func<object, bool> filter)
    {
        var activities = new List<Activity>();
        using (Sdk.CreateTracerProviderBuilder()
           .AddSqlClientInstrumentation(
               options =>
               {
                   options.Filter = filter;
               })
           .AddInMemoryExporter(activities)
           .Build())
        {
            MockCommandExecutor.ExecuteCommand(TestConnectionString, CommandType.Text, commandText, false, SqlClientLibrary.MicrosoftDataSqlClient);
        }

        return [.. activities];
    }
#endif
}
