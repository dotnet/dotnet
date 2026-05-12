// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Data;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using static OpenTelemetry.Internal.DatabaseSemanticConventionHelper;

namespace OpenTelemetry.Instrumentation.EntityFrameworkCore;

/// <summary>
/// Options for <see cref="EntityFrameworkInstrumentation"/>.
/// </summary>
public class EntityFrameworkInstrumentationOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityFrameworkInstrumentationOptions"/> class.
    /// </summary>
    public EntityFrameworkInstrumentationOptions()
        : this(new ConfigurationBuilder().AddEnvironmentVariables().Build())
    {
    }

    internal EntityFrameworkInstrumentationOptions(IConfiguration configuration)
    {
        var databaseSemanticConvention = GetSemanticConventionOptIn(configuration);
        this.EmitOldAttributes = databaseSemanticConvention.HasFlag(DatabaseSemanticConvention.Old);
        this.EmitNewAttributes = databaseSemanticConvention.HasFlag(DatabaseSemanticConvention.New);

        if (configuration["OTEL_DOTNET_EXPERIMENTAL_EFCORE_ENABLE_TRACE_DB_QUERY_PARAMETERS"] is { Length: > 0 } value &&
            bool.TryParse(value, out var setDbQueryParameters))
        {
            this.SetDbQueryParameters = setDbQueryParameters;
        }
    }

    /// <summary>
    /// Gets or sets an action to enrich an Activity from the db command.
    /// </summary>
    /// <remarks>
    /// <para><see cref="Activity"/>: the activity being enriched.</para>
    /// <para><see cref="IDbCommand"/>: db command to allow access to command.</para>
    /// </remarks>
    public Action<Activity, IDbCommand>? EnrichWithIDbCommand { get; set; }

    /// <summary>
    /// Gets or sets a filter function that determines whether or not to
    /// collect telemetry about a command from a particular provider.
    /// </summary>
    /// <remarks>
    /// <b>Notes:</b>
    /// <list type="bullet">
    /// <item>The first parameter passed to the filter function is the provider name.</item>
    /// <item>The second parameter passed to the filter function is <see cref="IDbCommand"/> from which additional
    /// information can be extracted.</item>
    /// <item>The return value for the filter:
    /// <list type="number">
    /// <item>If filter returns <see langword="true" />, the command is
    /// collected.</item>
    /// <item>If filter returns <see langword="false" /> or throws an
    /// exception, the command is <b>NOT</b> collected.</item>
    /// </list></item>
    /// </list>
    /// </remarks>
    public Func<string?, IDbCommand, bool>? Filter { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not the <see cref="EntityFrameworkInstrumentation"/>
    /// should add the names and values of query parameters as the <c>db.query.parameter.{key}</c> tag.
    /// Default value: <see langword="false"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>WARNING: SetDbQueryParameters will capture the raw
    /// <c>Value</c>. Make sure your query parameters never
    /// contain any sensitive data.</b>
    /// </para>
    /// </remarks>
    internal bool SetDbQueryParameters { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the old database attributes should be emitted.
    /// </summary>
    internal bool EmitOldAttributes { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the new database attributes should be emitted.
    /// </summary>
    internal bool EmitNewAttributes { get; set; }
}
