// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if !NETFRAMEWORK
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;

#if NET
using System.Text;
#endif
using OpenTelemetry.Internal;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Instrumentation.SqlClient.Implementation;

internal sealed class SqlClientDiagnosticListener : ListenerHandler
{
    public const string SqlDataBeforeExecuteCommand = "System.Data.SqlClient.WriteCommandBefore";
    public const string SqlMicrosoftBeforeExecuteCommand = "Microsoft.Data.SqlClient.WriteCommandBefore";

    public const string SqlDataAfterExecuteCommand = "System.Data.SqlClient.WriteCommandAfter";
    public const string SqlMicrosoftAfterExecuteCommand = "Microsoft.Data.SqlClient.WriteCommandAfter";

    public const string SqlDataWriteCommandError = "System.Data.SqlClient.WriteCommandError";
    public const string SqlMicrosoftWriteCommandError = "Microsoft.Data.SqlClient.WriteCommandError";

#if NET
    private const string ContextInfoParameterName = "@opentelemetry_traceparent";
    private const string SetContextSql = $"set context_info {ContextInfoParameterName}";

    private const string IL2026Justification = "Client application usage will ensure that core types from usage are preserved.";
#endif

    private readonly PropertyFetcher<IDbCommand> commandFetcher = new("Command");
    private readonly PropertyFetcher<Exception> exceptionFetcher = new("Exception");
    private readonly PropertyFetcher<int> exceptionNumberFetcher = new("Number");
    private readonly AsyncLocal<long> beginTimestamp = new();

    public SqlClientDiagnosticListener(string sourceName)
        : base(sourceName)
    {
    }

    public override bool SupportsNullActivity => true;

    public override void OnEventWritten(string name, object? payload)
    {
        if (SqlClientInstrumentation.Instance.HandleManager.TracingHandles == 0
            && SqlClientInstrumentation.Instance.HandleManager.MetricHandles == 0)
        {
            return;
        }

        var options = SqlClientInstrumentation.Instance.GetTracingOptions();
        var activity = Activity.Current;
        switch (name)
        {
            case SqlDataBeforeExecuteCommand:
            case SqlMicrosoftBeforeExecuteCommand:
                {
                    if (!TryFetchCommand(this.commandFetcher, payload, out var command))
                    {
                        SqlClientInstrumentationEventSource.Log.NullPayload(nameof(SqlClientDiagnosticListener), name);
                        return;
                    }

#if NET
                    // skip if this is an injected query
                    if (options.EnableTraceContextPropagation &&
                        command.CommandType is CommandType.Text && command.CommandText == SetContextSql)
                    {
                        return;
                    }
#endif

                    // Metrics-only fast path: if the ActivitySource has no listeners then StartActivity
                    // will always return null and no trace will be produced. Skip the (relatively
                    // expensive) connection tag derivation, query sanitization, filtering and enrichment
                    // entirely and only capture the start timestamp needed to compute the metric duration
                    // in the matching WriteCommandAfter/WriteCommandError event.
                    if (!SqlTelemetryHelper.ActivitySource.HasListeners())
                    {
                        this.beginTimestamp.Value = Stopwatch.GetTimestamp();
                        return;
                    }

                    var connection = command.Connection;
                    var databaseName = connection?.Database;
                    var dataSource = (connection as DbConnection)?.DataSource;

                    var startTags = SqlTelemetryHelper.GetTagListFromConnectionInfo(dataSource, databaseName, out var activityName);

                    var commandType = command.CommandType;
                    var commandText = command.CommandText;

                    switch (commandType)
                    {
                        case CommandType.StoredProcedure:
                            DatabaseSemanticConventionHelper.AddTagsForSamplingAndUpdateActivityNameForStoredProcedure(
                                ref startTags,
                                commandText,
                                ref activityName);
                            break;

                        case CommandType.Text:
                            DatabaseSemanticConventionHelper.AddTagsForSamplingAndUpdateActivityNameForQueryText(
                                ref startTags,
                                commandText,
                                ref activityName);
                            break;

                        case CommandType.TableDirect:
                        default:
                            break;
                    }

                    activity = SqlTelemetryHelper.ActivitySource.StartActivity(
                        activityName,
                        ActivityKind.Client,
                        default(ActivityContext),
                        startTags);

                    if (activity == null)
                    {
                        // There is no listener or it decided not to sample the current request.
                        this.beginTimestamp.Value = Stopwatch.GetTimestamp();
                        return;
                    }

#if NET
                    if (options.EnableTraceContextPropagation &&
                        command.CommandType is CommandType.Text && connection is { State: ConnectionState.Open })
                    {
                        using var setContextCommand = connection.CreateCommand();
                        setContextCommand.Transaction = command.Transaction;
                        setContextCommand.CommandText = SetContextSql;
                        setContextCommand.CommandType = CommandType.Text;
                        var parameter = setContextCommand.CreateParameter();
                        parameter.ParameterName = ContextInfoParameterName;

                        var tracedflags = FormatActivityTraceFlags(activity.ActivityTraceFlags);
                        var traceparent = $"00-{activity.TraceId.ToHexString()}-{activity.SpanId.ToHexString()}-{tracedflags}";

                        parameter.DbType = DbType.Binary;
                        parameter.Value = Encoding.UTF8.GetBytes(traceparent);
                        setContextCommand.Parameters.Add(parameter);

                        setContextCommand.ExecuteNonQuery();
                    }
#endif

                    if (activity.IsAllDataRequested)
                    {
                        try
                        {
                            if (options.Filter?.Invoke(command) == false)
                            {
                                SqlClientInstrumentationEventSource.Log.CommandIsFilteredOut(activity.OperationName);
                                activity.IsAllDataRequested = false;
                                activity.ActivityTraceFlags &= ~ActivityTraceFlags.Recorded;
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            SqlClientInstrumentationEventSource.Log.CommandFilterException(ex);
                            activity.IsAllDataRequested = false;
                            activity.ActivityTraceFlags &= ~ActivityTraceFlags.Recorded;
                            return;
                        }

                        if (options.SetDbQueryParameters)
                        {
                            SqlParameterProcessor.AddQueryParameters(activity, command);
                        }

                        try
                        {
                            options.EnrichWithSqlCommand?.Invoke(activity, command);
                        }
                        catch (Exception ex)
                        {
                            SqlClientInstrumentationEventSource.Log.EnrichmentException(ex);
                        }
                    }
                }

                break;
            case SqlDataAfterExecuteCommand:
            case SqlMicrosoftAfterExecuteCommand:
                {
                    _ = TryFetchCommand(this.commandFetcher, payload, out var command);

#if NET
                    // skip if this is an injected query
                    if (options.EnableTraceContextPropagation && command != null &&
                        command.CommandType is CommandType.Text && command.CommandText == SetContextSql)
                    {
                        return;
                    }
#endif

                    if (activity == null)
                    {
                        SqlClientInstrumentationEventSource.Log.NullActivity(name);
                        this.RecordDuration(null, payload);
                        return;
                    }

                    if (activity.Source != SqlTelemetryHelper.ActivitySource)
                    {
                        this.RecordDuration(null, payload);
                        return;
                    }

                    activity.Stop();
                    this.RecordDuration(activity, payload);
                }

                break;
            case SqlDataWriteCommandError:
            case SqlMicrosoftWriteCommandError:
                {
                    _ = TryFetchCommand(this.commandFetcher, payload, out var command);

#if NET
                    // skip if this is an injected query
                    if (options.EnableTraceContextPropagation && command != null &&
                        command.CommandType is CommandType.Text && command.CommandText == SetContextSql)
                    {
                        return;
                    }
#endif

                    if (activity == null)
                    {
                        SqlClientInstrumentationEventSource.Log.NullActivity(name);
                        this.RecordDuration(null, payload);
                        return;
                    }

                    if (activity.Source != SqlTelemetryHelper.ActivitySource)
                    {
                        this.RecordDuration(null, payload);
                        return;
                    }

                    try
                    {
                        if (activity.IsAllDataRequested)
                        {
                            if (TryFetchException(this.exceptionFetcher, payload, out var exception))
                            {
                                activity.AddTag(SemanticConventions.AttributeErrorType, exception.GetType().FullName);

                                if (TryFetchExceptionNumber(this.exceptionNumberFetcher, exception, out var exceptionNumber))
                                {
                                    activity.AddTag(SemanticConventions.AttributeDbResponseStatusCode, exceptionNumber.ToString(CultureInfo.InvariantCulture));
                                }

                                activity.SetStatus(ActivityStatusCode.Error, exception.Message);

                                if (options.RecordException)
                                {
                                    activity.AddException(exception);
                                }
                            }
                            else
                            {
                                SqlClientInstrumentationEventSource.Log.NullPayload(nameof(SqlClientDiagnosticListener), name);
                            }
                        }
                    }
                    finally
                    {
                        activity.Stop();
                        this.RecordDuration(activity, payload, hasError: true);
                    }
                }

                break;
            default:
                break;
        }
    }

    private static string FormatActivityTraceFlags(ActivityTraceFlags flags)
    {
        // https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/3867
        // will change this code to use ActivityTraceFlags.RandomTraceId instead of 2.
        // If new enum values are added in the future the Fallback path will ensure
        // that the handling is functionally correct, but the switch should be updated
        // to include the new value(s) for better readability and performance where possible.
        return flags switch
        {
            ActivityTraceFlags.None => "00",
            ActivityTraceFlags.Recorded => "01",
            (ActivityTraceFlags)2 => "02",
            ActivityTraceFlags.Recorded | (ActivityTraceFlags)2 => "03",
            _ => Fallback((byte)flags),
        };

        static string Fallback(byte flags)
        {
            // IDE0302 suppressed as benchmarking showed that the explicitly stackalloc'd variant was more performant
#pragma warning disable IDE0302 // Simplify collection initialization
            Span<char> buffer = stackalloc char[2];
#pragma warning restore IDE0302 // Simplify collection initialization

            buffer[0] = GetHexChar(flags >> 4);
            buffer[1] = GetHexChar(flags & 0xF);

            return buffer.ToString();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static char GetHexChar(int value)
            {
                return (char)(value + (value < 10 ? '0' : 'a' - 10));
            }
        }
    }

#if NET
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = IL2026Justification)]
#endif
    private static bool TryFetchCommand(
        PropertyFetcher<IDbCommand> fetcher,
        object? payload,
        [NotNullWhen(true)] out IDbCommand? command)
        => fetcher.TryFetch(payload, out command);

#if NET
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = IL2026Justification)]
#endif
    private static bool TryFetchException(
        PropertyFetcher<Exception> fetcher,
        object? payload,
        [NotNullWhen(true)] out Exception? exception)
        => fetcher.TryFetch(payload, out exception);

#if NET
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = IL2026Justification)]
#endif
    private static bool TryFetchExceptionNumber(
        PropertyFetcher<int> fetcher,
        Exception exception,
        out int number)
        => fetcher.TryFetch(exception, out number);

    private void RecordDuration(Activity? activity, object? payload, bool hasError = false)
    {
        if (SqlClientInstrumentation.Instance.HandleManager.MetricHandles == 0)
        {
            return;
        }

        var tags = default(TagList);

        if (activity != null && activity.IsAllDataRequested)
        {
            foreach (var name in SqlTelemetryHelper.SharedTagNames)
            {
                var value = activity.GetTagItem(name);
                if (value != null)
                {
                    tags.Add(name, value);
                }
            }
        }
        else if (payload != null)
        {
            if (TryFetchCommand(this.commandFetcher, payload, out var command))
            {
                var connection = command.Connection;
                var databaseName = connection?.Database;
                var dataSource = (connection as DbConnection)?.DataSource;

                var connectionTags = SqlTelemetryHelper.GetTagListFromConnectionInfo(
                    dataSource,
                    databaseName,
                    out _);

                foreach (var tag in connectionTags)
                {
                    tags.Add(tag.Key, tag.Value);
                }

                if (command.CommandType is CommandType.StoredProcedure)
                {
                    tags.Add(SemanticConventions.AttributeDbStoredProcedureName, command.CommandText);
                }
            }

            if (hasError)
            {
                if (TryFetchException(this.exceptionFetcher, payload, out var exception))
                {
                    tags.Add(SemanticConventions.AttributeErrorType, exception.GetType().FullName);

                    if (TryFetchExceptionNumber(this.exceptionNumberFetcher, exception, out var exceptionNumber))
                    {
                        tags.Add(SemanticConventions.AttributeDbResponseStatusCode, exceptionNumber.ToString(CultureInfo.InvariantCulture));
                    }
                }
            }
        }

        var duration = activity?.Duration.TotalSeconds
            ?? SqlTelemetryHelper.CalculateDurationFromTimestamp(this.beginTimestamp.Value);
        SqlTelemetryHelper.DbClientOperationDuration.Record(duration, tags);
    }
}
#endif
