// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;

namespace OpenTelemetry.Instrumentation.AWS.Implementation;

internal class Utils
{
    internal static object? GetTagValue(Activity activity, string tagName)
    {
        foreach (var tag in activity.TagObjects)
        {
            if (tag.Key.Equals(tagName, StringComparison.Ordinal))
            {
                return tag.Value;
            }
        }

        return null;
    }

    internal static string RemoveSuffix(string originalString, string suffix)
    {
        return string.IsNullOrEmpty(originalString)
            ? string.Empty
            : originalString.EndsWith(suffix, StringComparison.Ordinal)
                ?
                originalString.Substring(0, originalString.Length - suffix.Length)
                : originalString;
    }

    /// <summary>
    /// Removes amazon prefix from service name. There are two type of service name.
    ///     Amazon.DynamoDbV2
    ///     AmazonS3
    ///     .
    /// </summary>
    /// <param name="serviceName">Name of the service.</param>
    /// <returns>String after removing Amazon prefix.</returns>
    internal static string RemoveAmazonPrefixFromServiceName(string serviceName)
    {
        return RemovePrefix(RemovePrefix(serviceName, "Amazon"), ".");
    }

    private static string RemovePrefix(string originalString, string prefix)
    {
        return string.IsNullOrEmpty(originalString) ? string.Empty :
            originalString.StartsWith(prefix, StringComparison.Ordinal) ? originalString.Substring(prefix.Length) :
            originalString;
    }
}
