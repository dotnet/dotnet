// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.ServiceModel.Channels;
using Microsoft.Extensions.Configuration;
using static OpenTelemetry.Internal.RpcSemanticConventionHelper;

namespace OpenTelemetry.Instrumentation.Wcf;

/// <summary>
/// Options for WCF instrumentation.
/// </summary>
public class WcfInstrumentationOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WcfInstrumentationOptions"/> class.
    /// </summary>
    public WcfInstrumentationOptions()
        : this(new ConfigurationBuilder().AddEnvironmentVariables().Build())
    {
    }

    internal WcfInstrumentationOptions(IConfiguration configuration)
    {
        var rpcSemanticConvention = GetSemanticConventionOptIn(configuration);
        this.EmitOldRpcAttributes = rpcSemanticConvention.HasFlag(RpcSemanticConvention.Old);
        this.EmitNewRpcAttributes = rpcSemanticConvention.HasFlag(RpcSemanticConvention.New);
    }

    /// <summary>
    /// Gets or sets an action to enrich an Activity.
    /// </summary>
    /// <remarks>
    /// <para><see cref="Activity"/>: the activity being enriched.</para>
    /// <para>string: the name of the event. Will be one of the constants in <see cref="WcfEnrichEventNames"/>.
    /// </para>
    /// <para>object: the raw <see cref="Message"/> from which additional information can be extracted to enrich the activity.
    /// </para>
    /// </remarks>
    public Action<Activity, string, object>? Enrich { get; set; }

    /// <summary>
    /// Gets or sets a Filter function to filter instrumentation for requests on a per request basis.
    /// The Filter gets the Message, and should return a boolean.
    /// If Filter returns true, the request is collected.
    /// If Filter returns false or throw exception, the request is filtered out.
    /// </summary>
    public Func<Message, bool>? IncomingRequestFilter { get; set; }

    /// <summary>
    /// Gets or sets a Filter function to filter instrumentation for requests on a per request basis.
    /// The Filter gets the Message, and should return a boolean.
    /// If Filter returns true, the request is collected.
    /// If Filter returns false or throw exception, the request is filtered out.
    /// </summary>
    public Func<Message, bool>? OutgoingRequestFilter { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether down stream instrumentation (HttpClient) is suppressed (disabled). Default value: True.
    /// </summary>
    public bool SuppressDownstreamInstrumentation { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether or not the SOAP message version should be added as the <see cref="WcfInstrumentationConstants.AttributeSoapMessageVersion"/> tag. Default value: False.
    /// </summary>
    public bool SetSoapMessageVersion { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether exception will be recorded
    /// as an <see cref="ActivityEvent"/> or not. Default value: <see
    /// langword="false"/>.
    /// </summary>
    /// <remarks>
    /// <para>For specification details see: <see
    /// href="https://github.com/open-telemetry/semantic-conventions/blob/v1.42.0/docs/exceptions/exceptions-spans.md"
    /// />.</para>
    /// </remarks>
    public bool RecordException { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the old RPC attributes should be emitted.
    /// </summary>
    internal bool EmitOldRpcAttributes { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the new RPC attributes should be emitted.
    /// </summary>
    internal bool EmitNewRpcAttributes { get; set; }
}
