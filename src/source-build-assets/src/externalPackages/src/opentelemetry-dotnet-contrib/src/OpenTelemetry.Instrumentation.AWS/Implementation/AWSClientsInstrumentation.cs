// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Amazon.Runtime.Internal;

namespace OpenTelemetry.Instrumentation.AWS.Implementation;

internal class AWSClientsInstrumentation
{
    public AWSClientsInstrumentation(AWSClientInstrumentationOptions options)
    {
        RuntimePipelineCustomizerRegistry.Instance.Register(new AWSTracingPipelineCustomizer(options));
    }
}
