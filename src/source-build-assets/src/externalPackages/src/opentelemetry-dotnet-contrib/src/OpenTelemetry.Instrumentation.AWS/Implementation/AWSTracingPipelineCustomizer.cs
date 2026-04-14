// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Amazon.Runtime;
using Amazon.Runtime.Internal;

namespace OpenTelemetry.Instrumentation.AWS.Implementation;

/// <summary>
/// Wires <see cref="AWSTracingPipelineHandler"/> and <see cref="AWSPropagatorPipelineHandler"/> into the AWS
/// <see cref="RuntimePipeline"/> so they can inject trace headers and add request information to the tags.
/// </summary>
internal class AWSTracingPipelineCustomizer : IRuntimePipelineCustomizer
{
    public const string UniqueName = "AWS Tracing Registration Customization";

    private readonly AWSClientInstrumentationOptions options;

    public AWSTracingPipelineCustomizer(AWSClientInstrumentationOptions options)
    {
        this.options = options;
    }

    string IRuntimePipelineCustomizer.UniqueName => UniqueName;

    public void Customize(Type serviceClientType, RuntimePipeline pipeline)
    {
        if (!typeof(AmazonServiceClient).IsAssignableFrom(serviceClientType))
        {
            return;
        }

        var tracingPipelineHandler = new AWSTracingPipelineHandler(this.options);
        var propagatingPipelineHandler = new AWSPropagatorPipelineHandler();

        // AWSTracingPipelineHandler must execute early in the AWS SDK pipeline
        // in order to manipulate outgoing requests objects before they are marshalled (ie serialized).
        pipeline.AddHandlerBefore<Marshaller>(tracingPipelineHandler);

        // AWSPropagatorPipelineHandler executes after the AWS SDK has marshalled (ie serialized)
        // the outgoing request object so that it can work with the request's Headers
        pipeline.AddHandlerBefore<RetryHandler>(propagatingPipelineHandler);
    }
}
