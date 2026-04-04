// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Data;
using System.Diagnostics;
using System.Text;
using Microsoft.Data.SqlClient;
using OpenTelemetry.Instrumentation.SqlClient.Implementation;
using OpenTelemetry.Metrics;
using OpenTelemetry.Tests;
using OpenTelemetry.Trace;
using Xunit;

namespace OpenTelemetry.Instrumentation.SqlClient.Tests;

[Trait("CategoryName", "SqlIntegrationTests")]
public sealed class SqlClientIntegrationTests : IClassFixture<SqlClientIntegrationTestsFixture>
{
    private const string GetContextInfoQuery = "SELECT CONTEXT_INFO()";

    private readonly SqlClientIntegrationTestsFixture fixture;

    public SqlClientIntegrationTests(SqlClientIntegrationTestsFixture fixture)
    {
        this.fixture = fixture;
    }

    [EnabledOnDockerPlatformTheory(DockerPlatform.Linux)]
    [InlineData(CommandType.Text, "select 1/1", "select ?/?")]
    [InlineData(CommandType.Text, "select 1/0", "select ?/?", true)]
    [InlineData(CommandType.Text, "select 1/0", "select ?/?", true, true)]
#if NET
    [InlineData(CommandType.Text, GetContextInfoQuery, GetContextInfoQuery, false, false, false)]
    [InlineData(CommandType.Text, GetContextInfoQuery, GetContextInfoQuery, false, false, true)]
    [InlineData(CommandType.StoredProcedure, "sp_who", "sp_who")]
#endif
    [InlineData(CommandType.Text, "exec sp_who", "exec sp_who")]
    public void SuccessfulCommandTest(
        CommandType commandType,
        string commandText,
        string? sanitizedCommandText,
        bool isFailure = false,
        bool recordException = false,
        bool enableTransaction = false)
    {
        using var scope = EnvironmentVariableScope.Create(
            SqlClientTraceInstrumentationOptions.ContextPropagationLevelEnvVar,
            commandText == GetContextInfoQuery ? "true" : null);

#if NETFRAMEWORK
        // Disable things not available on netfx
        recordException = false;
#endif

        var sampler = new TestSampler();
        var activities = new List<Activity>();
        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .SetSampler(sampler)
            .AddInMemoryExporter(activities)
            .AddSqlClientInstrumentation(options =>
            {
#if NET
                options.RecordException = recordException;
#endif
            })
            .Build();

        using var sqlConnection = new SqlConnection(this.GetConnectionString());

        sqlConnection.Open();

        sqlConnection.ChangeDatabase("master");
        SqlTransaction? transaction = null;
#pragma warning disable CA2100
        using var sqlCommand = new SqlCommand(commandText, sqlConnection)
#pragma warning restore CA2100
        {
            CommandType = commandType,
        };

        if (enableTransaction)
        {
            transaction = sqlConnection.BeginTransaction();
            sqlCommand.Transaction = transaction;
        }

        object commandResult = DBNull.Value;
        try
        {
            commandResult = sqlCommand.ExecuteScalar();
        }
        catch (Exception)
        {
        }

        transaction?.Commit();

        var activity = Assert.Single(activities);

        VerifyContextInfo(commandText, commandResult, activity);
        VerifyActivityData(commandType, sanitizedCommandText, isFailure, recordException, activity);
        VerifySamplingParameters(sampler.LatestSamplingParameters);

        if (isFailure)
        {
#if NET
            Assert.Equal(ActivityStatusCode.Error, activity.Status);
            Assert.Equal("Divide by zero error encountered.", activity.StatusDescription);
            Assert.EndsWith("SqlException", activity.GetTagValue(SemanticConventions.AttributeErrorType) as string);
            Assert.Equal("8134", activity.GetTagValue(SemanticConventions.AttributeDbResponseStatusCode));
#else
            Assert.Equal(ActivityStatusCode.Error, activity.Status);
            Assert.Equal("8134", activity.StatusDescription);
            Assert.EndsWith("SqlException", activity.GetTagValue(SemanticConventions.AttributeErrorType) as string);
            Assert.Equal("8134", activity.GetTagValue(SemanticConventions.AttributeDbResponseStatusCode));
#endif
        }
    }

#if NET
    [EnabledOnDockerPlatformFact(DockerPlatform.Linux)]
    public async Task SuccessfulParameterizedQueryTest()
    {
        // Arrange
        var sampler = new TestSampler();
        var activities = new List<Activity>();

        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .SetSampler(sampler)
            .AddInMemoryExporter(activities)
            .AddSqlClientInstrumentation(options => options.SetDbQueryParameters = true)
            .Build();

        using var sqlConnection = new SqlConnection(this.GetConnectionString());

        await sqlConnection.OpenAsync();

        sqlConnection.ChangeDatabase("master");

        using var sqlCommand = new SqlCommand("SELECT @x + @y", sqlConnection);

        sqlCommand.Parameters.AddWithValue("@x", 42);
        sqlCommand.Parameters.AddWithValue("@y", 37);

        // Act
        var result = await sqlCommand.ExecuteScalarAsync();

        // Assert
        Assert.Equal(79, result);

        var activity = Assert.Single(activities);

        Assert.Equal(42, activity.GetTagValue("db.query.parameter.@x"));
        Assert.Equal(37, activity.GetTagValue("db.query.parameter.@y"));
    }
#endif

    [EnabledOnDockerPlatformFact(DockerPlatform.Linux)]
    public async Task ActivityIsStoppedWhenOnlyUsingMetrics()
    {
        // Arrange
        var activities = new List<Activity>();
        var metrics = new List<MetricSnapshot>();

        using var listener = new ActivityListener();
        listener.ActivityStarted = activities.Add;
        listener.Sample = (ref _) => ActivitySamplingResult.AllData;
        listener.ShouldListenTo = _ => true;

        ActivitySource.AddActivityListener(listener);

        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddInMemoryExporter(metrics)
            .AddSqlClientInstrumentation()
            .Build();

        using var sqlConnection = new SqlConnection(this.GetConnectionString());

        await sqlConnection.OpenAsync();

        sqlConnection.ChangeDatabase("master");

        using var sqlCommand = new SqlCommand("select 1/1", sqlConnection);

        // Act
        var result = await sqlCommand.ExecuteScalarAsync();

        // Assert
        Assert.Equal(1, result);

        var activity = Assert.Single(activities);

        Assert.True(activity.IsStopped);
    }

    private static void VerifyContextInfo(
        string? commandText,
        object commandResult,
        Activity activity)
    {
        if (commandText == GetContextInfoQuery)
        {
            Assert.NotEqual(commandResult, DBNull.Value);
            Assert.True(commandResult is byte[]);
            var contextInfo = Encoding.ASCII.GetString((byte[])commandResult).TrimEnd('\0');
            Assert.Equal(contextInfo, activity.Id);
        }
    }

    private static void VerifyActivityData(
        CommandType commandType,
        string? commandText,
        bool isFailure,
        bool recordException,
        Activity activity)
    {
        var dbQuerySummary = activity.GetTagValue(SemanticConventions.AttributeDbQuerySummary);
        Assert.Equal(dbQuerySummary, activity.DisplayName);

        Assert.Equal(ActivityKind.Client, activity.Kind);

        if (!isFailure)
        {
            Assert.Equal(ActivityStatusCode.Unset, activity.Status);
        }
        else
        {
            Assert.Equal(ActivityStatusCode.Error, activity.Status);
            Assert.NotNull(activity.StatusDescription);

            if (recordException)
            {
                var events = activity.Events.ToList();
                Assert.Single(events);

                Assert.Equal(SemanticConventions.AttributeExceptionEventName, events[0].Name);
            }
            else
            {
                Assert.Empty(activity.Events);
            }
        }

        Assert.Equal(SqlTelemetryHelper.MicrosoftSqlServerDbSystemName, activity.GetTagValue(SemanticConventions.AttributeDbSystemName));
        Assert.Equal("master", activity.GetTagValue(SemanticConventions.AttributeDbNamespace));

        Assert.DoesNotContain(activity.TagObjects, tag => tag.Key.StartsWith("db.query.parameter.", StringComparison.Ordinal));
        Assert.DoesNotContain(activity.Tags, tag => tag.Key.StartsWith("db.query.parameter.", StringComparison.Ordinal));

        switch (commandType)
        {
            case CommandType.StoredProcedure:
                Assert.Equal(commandText, activity.GetTagValue(SemanticConventions.AttributeDbStoredProcedureName));
                break;
            case CommandType.Text:
                Assert.Equal(commandText, activity.GetTagValue(SemanticConventions.AttributeDbQueryText));
                break;
            case CommandType.TableDirect:
                Assert.Fail("Not supported command type: CommandType.TableDirect");
                break;
            default:
                Assert.Fail($"Not supported command type: {commandType}");
                break;
        }
    }

    private static void VerifySamplingParameters(SamplingParameters samplingParameters)
    {
        Assert.NotNull(samplingParameters.Tags);
        Assert.Contains(
            samplingParameters.Tags,
            kvp => kvp.Key == SemanticConventions.AttributeDbSystemName
                   && kvp.Value != null
                   && (string)kvp.Value == SqlTelemetryHelper.MicrosoftSqlServerDbSystemName);
    }

    private string GetConnectionString()
        => this.fixture.DatabaseContainer.GetConnectionString();
}
