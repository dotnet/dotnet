// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Telemetry;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Extensions.AWS.Trace;

namespace OpenTelemetry.Instrumentation.AWS.Implementation;

/// <summary>
/// Uses <see cref="AWSXRayPropagator"/> to inject the current Activity Context and
/// Baggage into the outgoing AWS SDK Request.
/// <para />
/// Must execute after the AWS SDK has marshalled (ie serialized)
/// the outgoing request object so that it can work with the <see cref="IRequest"/>'s
/// <see cref="IRequest.Headers"/>.
/// </summary>
internal class AWSPropagatorPipelineHandler : PipelineHandler
{
    private static readonly AWSXRayPropagator AwsPropagator = new();

    private static readonly Action<IDictionary<string, string>, string, string> Setter = (carrier, name, value) =>
    {
        carrier[name] = value;
    };

    public override void InvokeSync(IExecutionContext executionContext)
    {
        ProcessBeginRequest(executionContext);

        base.InvokeSync(executionContext);
    }

    public override async Task<T> InvokeAsync<T>(IExecutionContext executionContext)
    {
        ProcessBeginRequest(executionContext);

        return await base.InvokeAsync<T>(executionContext).ConfigureAwait(false);
    }

    private static void ProcessBeginRequest(IExecutionContext executionContext)
    {
        var currentActivity = Activity.Current;

        // Propagate the current activity if it was created by the AWS SDK
        if (currentActivity == null || !currentActivity.Source.Name.StartsWith(TelemetryConstants.TelemetryScopePrefix, StringComparison.Ordinal))
        {
            return;
        }

        AwsPropagator.Inject(
            new PropagationContext(currentActivity.Context, Baggage.Current),
            executionContext.RequestContext.Request.Headers,
            Setter);
    }
}
