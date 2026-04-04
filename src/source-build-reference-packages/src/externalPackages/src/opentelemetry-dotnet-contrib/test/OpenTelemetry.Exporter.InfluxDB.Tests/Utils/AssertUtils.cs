// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Xunit;

namespace OpenTelemetry.Exporter.InfluxDB.Tests.Utils;

internal static class AssertUtils
{
    public static void HasField<TKey, TValue>(TKey expectedKey, TValue expectedValue, IReadOnlyDictionary<TKey, object> collection)
        where TKey : notnull
    {
        Assert.Contains(expectedKey, collection);
        Assert.Equal(expectedValue, collection[expectedKey]);
    }

    public static void HasTag(string expectedKey, string expectedValue, int index, IReadOnlyList<KeyValuePair<string, string>> collection)
    {
        Assert.Equal(expectedKey, collection[index].Key);
        Assert.Equal(expectedValue, collection[index].Value);
    }
}
