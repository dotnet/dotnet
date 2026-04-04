// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Sampler.AWS.Tests;

internal static class Utils
{
    internal static SamplingParameters CreateSamplingParameters()
    {
        return CreateSamplingParametersWithTags([]);
    }

    internal static SamplingParameters CreateSamplingParametersWithTags(Dictionary<string, string> tags)
    {
        var traceId = ActivityTraceId.CreateRandom();
        var parentSpanId = ActivitySpanId.CreateRandom();
        var traceFlags = ActivityTraceFlags.None;

        var parentContext = new ActivityContext(traceId, parentSpanId, traceFlags);

        var tagList = new List<KeyValuePair<string, object?>>();

        foreach (var tag in tags)
        {
            tagList.Add(new KeyValuePair<string, object?>(tag.Key, tag.Value));
        }

        return new SamplingParameters(
            parentContext,
            traceId,
            "myActivityName",
            ActivityKind.Server,
            tagList,
            null);
    }

    internal static Resource CreateResource(string serviceName, string cloudPlatform)
    {
        var resourceAttributes = new List<KeyValuePair<string, object>>
        {
            new("service.name", serviceName),
            new("cloud.platform", cloudPlatform),
        };

        return ResourceBuilder.CreateEmpty().AddAttributes(resourceAttributes).Build();
    }
}
