// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Instrumentation.AWSLambda.Implementation;

internal static class CommonExtensions
{
    internal static T? GetValueByKeyIgnoringCase<T>(this IDictionary<string, T> dict, string key)
    {
        // TODO: there may be opportunities for performance improvements of this method.

        // We had to introduce case-insensitive headers search as can't fully rely on
        // AWS documentation stating that expected headers are lower-case. AWS test
        // console offers JSON example with camel case header names.
        // See X-Forwarded-Proto or X-Forwarded-Port for example.

        if (dict == null)
        {
            return default;
        }

        foreach (var kvp in dict)
        {
            if (string.Equals(kvp.Key, key, StringComparison.OrdinalIgnoreCase))
            {
                return kvp.Value;
            }
        }

        return default;
    }
}
