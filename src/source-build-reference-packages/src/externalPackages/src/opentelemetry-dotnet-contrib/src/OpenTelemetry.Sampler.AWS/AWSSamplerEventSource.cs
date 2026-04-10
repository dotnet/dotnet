// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.Tracing;

namespace OpenTelemetry.Sampler.AWS;

[EventSource(Name = "OpenTelemetry-Sampler-AWS")]
internal class AWSSamplerEventSource : EventSource
{
    public static AWSSamplerEventSource Log = new AWSSamplerEventSource();

    [Event(1, Message = "Error response from {0} with status code {1}", Level = EventLevel.Warning)]
    public void FailedToGetSuccessResponse(string endpoint, string statusCode)
    {
        this.WriteEvent(1, endpoint, statusCode);
    }

    [Event(2, Message = "Exception from AWSXRayRemoteSampler while executing request {0})}", Level = EventLevel.Warning)]
    public void ExceptionFromSampler(string message)
    {
        this.WriteEvent(2, message);
    }

    [Event(3, Message = "Failed to deserialize to object in format: '{0}', error: '{1}'.", Level = EventLevel.Warning)]
    public void FailedToDeserializeResponse(string format, string error)
    {
        this.WriteEvent(3, format, error);
    }

    [Event(4, Message = "Using fallback sampler. Either rules cache has expired or no rules matched the request.", Level = EventLevel.Informational)]
    public void InfoUsingFallbackSampler()
    {
        this.WriteEvent(4);
    }
}
