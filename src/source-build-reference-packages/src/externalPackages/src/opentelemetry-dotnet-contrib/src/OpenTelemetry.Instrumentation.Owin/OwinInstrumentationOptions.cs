// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Owin;

namespace OpenTelemetry.Instrumentation.Owin;

/// <summary>
/// Options for requests instrumentation.
/// </summary>
public class OwinInstrumentationOptions
{
    private const string DisableQueryRedactionEnvVar = "OTEL_DOTNET_EXPERIMENTAL_OWIN_DISABLE_URL_QUERY_REDACTION";

    /// <summary>
    /// Initializes a new instance of the <see cref="OwinInstrumentationOptions"/> class.
    /// </summary>
    public OwinInstrumentationOptions()
        : this(new ConfigurationBuilder().AddEnvironmentVariables().Build())
    {
    }

    internal OwinInstrumentationOptions(IConfiguration configuration)
    {
        Debug.Assert(configuration != null, "configuration was null");

        if (configuration!.TryGetBoolValue(
            OwinInstrumentationEventSource.Log,
            DisableQueryRedactionEnvVar,
            out var disableUrlQueryRedaction))
        {
            this.DisableUrlQueryRedaction = disableUrlQueryRedaction;
        }
    }

    /// <summary>
    /// Gets or sets a Filter function that determines whether or not to collect telemetry about requests on a per request basis.
    /// The Filter gets the <see cref="IOwinContext"/>, and should return a boolean.
    /// If Filter returns true, the request is collected.
    /// If Filter returns false or throw exception, the request is filtered out.
    /// </summary>
    public Func<IOwinContext, bool>? Filter { get; set; }

    /// <summary>
    /// Gets or sets an action to enrich the <see cref="Activity"/> created by the instrumentation.
    /// </summary>
    public Action<Activity, OwinEnrichEventType, IOwinContext, Exception?>? Enrich { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the exception will be recorded as <see cref="ActivityEvent"/> or not.
    /// </summary>
    /// <remarks>
    /// https://github.com/open-telemetry/semantic-conventions/blob/main/docs/exceptions/exceptions-spans.md.
    /// </remarks>
    public bool RecordException { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the url query value should be redacted or not.
    /// </summary>
    /// <remarks>
    /// The query parameter values are redacted with value set as Redacted.
    /// e.g. `?key1=value1` is set as `?key1=Redacted`.
    /// The redaction can be disabled by setting this property to <see langword="true" />.
    /// </remarks>
    internal bool DisableUrlQueryRedaction { get; set; }
}
