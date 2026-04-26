// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if !NETFRAMEWORK
using System.Diagnostics;
#endif
using Microsoft.Extensions.Configuration;
#if NET
using OpenTelemetry.Instrumentation.SqlClient.Implementation;
#endif

namespace OpenTelemetry.Instrumentation.SqlClient;

/// <summary>
/// Options for <see cref="SqlClientInstrumentation"/>.
/// </summary>
/// <remarks>
/// For help and examples see: <a href="https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/src/OpenTelemetry.Instrumentation.SqlClient/README.md#advanced-configuration" />.
/// </remarks>
public class SqlClientTraceInstrumentationOptions
{
    internal const string ContextPropagationLevelEnvVar = "OTEL_DOTNET_EXPERIMENTAL_SQLCLIENT_ENABLE_TRACE_CONTEXT_PROPAGATION";
    internal const string SetDbQueryParametersEnvVar = "OTEL_DOTNET_EXPERIMENTAL_SQLCLIENT_ENABLE_TRACE_DB_QUERY_PARAMETERS";

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlClientTraceInstrumentationOptions"/> class.
    /// </summary>
    public SqlClientTraceInstrumentationOptions()
        : this(new ConfigurationBuilder().AddEnvironmentVariables().Build())
    {
    }

    internal SqlClientTraceInstrumentationOptions(IConfiguration configuration)
    {
#if NET
        Debug.Assert(configuration != null, "configuration was null");

        if (configuration!.TryGetBoolValue(
                SqlClientInstrumentationEventSource.Log,
                ContextPropagationLevelEnvVar,
                out var enableTraceContextPropagation))
        {
            this.EnableTraceContextPropagation = enableTraceContextPropagation;
        }

        if (configuration!.TryGetBoolValue(
                SqlClientInstrumentationEventSource.Log,
                SetDbQueryParametersEnvVar,
                out var setDbQueryParameters))
        {
            this.SetDbQueryParameters = setDbQueryParameters;
        }
#endif
    }

#if !NETFRAMEWORK
    /// <summary>
    /// Gets or sets an action to enrich an <see cref="Activity"/> with the
    /// raw <c>SqlCommand</c> object.
    /// </summary>
    /// <remarks>
    /// <para><b>Enrich is only executed on .NET runtimes.</b></para>
    /// The parameters passed to the enrich action are:
    /// <list type="number">
    /// <item>The <see cref="Activity"/> being enriched.</item>
    /// <item>The raw <c>SqlCommand</c> object from which additional
    /// information can be extracted to enrich the <see cref="Activity"/>.</item>
    /// </list>
    /// </remarks>
    public Action<Activity, object>? EnrichWithSqlCommand { get; set; }

    /// <summary>
    /// Gets or sets a filter function that determines whether or not to
    /// collect telemetry about a command.
    /// </summary>
    /// <remarks>
    /// <para><b>Filter is only executed on .NET runtimes.</b></para>
    /// Notes:
    /// <list type="bullet">
    /// <item>The first parameter passed to the filter function is the raw
    /// <c>SqlCommand</c> object for the command being executed.</item>
    /// <item>The return value for the filter function is interpreted as:
    /// <list type="bullet">
    /// <item>If filter returns <see langword="true" />, the command is
    /// collected.</item>
    /// <item>If filter returns <see langword="false" /> or throws an
    /// exception the command is NOT collected.</item>
    /// </list></item>
    /// </list>
    /// </remarks>
    public Func<object, bool>? Filter { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the exception will be
    /// recorded as <see cref="ActivityEvent"/> or not. Default value: <see
    /// langword="false"/>.
    /// </summary>
    /// <remarks>
    /// <para><b>RecordException is only supported on .NET runtimes.</b></para>
    /// <para>For specification details see: <see
    /// href="https://github.com/open-telemetry/semantic-conventions/blob/main/docs/exceptions/exceptions-spans.md"/>.</para>
    /// </remarks>
    public bool RecordException { get; set; }
#endif

#if !NETFRAMEWORK
    /// <summary>
    /// Gets or sets a value indicating whether or not the <see cref="SqlClientInstrumentation"/>
    /// should add the names and values of query parameters as the <c>db.query.parameter.{key}</c> tag.
    /// Default value: <see langword="false"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>WARNING: SetDbQueryParameters will capture the raw
    /// <c>Value</c>. Make sure your query parameters never
    /// contain any sensitive data.</b>
    /// </para>
    /// <para>
    /// <b>SetDbQueryParameters is only supported on .NET runtimes.</b>
    /// </para>
    /// </remarks>
    internal bool SetDbQueryParameters { get; set; }
#endif

#if NET
    /// <summary>
    /// Gets or sets a value indicating whether to send traceparent information to SQL Server database.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>Only `CommandType.Text` commands are supported for trace context propagation.</b>
    /// Note: This uses the SET CONTEXT_INFO command to set traceparent information
    /// for the current connection, which results in an additional round-trip to the database.
    /// </para>
    /// </remarks>
    internal bool EnableTraceContextPropagation { get; set; }
#endif
}
