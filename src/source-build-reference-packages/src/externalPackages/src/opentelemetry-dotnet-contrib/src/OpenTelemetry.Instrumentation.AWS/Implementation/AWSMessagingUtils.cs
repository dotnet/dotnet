// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Context.Propagation;

namespace OpenTelemetry.Instrumentation.AWS.Implementation;

internal static class AWSMessagingUtils
{
    internal static IReadOnlyDictionary<string, string> InjectIntoDictionary(PropagationContext propagationContext)
    {
        var carrier = new Dictionary<string, string>();
        Propagators.DefaultTextMapPropagator.Inject(propagationContext, carrier, (c, k, v) => c[k] = v);
        return carrier;
    }
}
