// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json.Linq;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.NewtonsoftReference;

/// <summary>
/// Helper for comparing the current serializer output against the
/// original Newtonsoft.Json implementation.
/// On .NET Core the current serializer is STJ; on net48 it is Jsonite.
/// </summary>
internal static class NewtonsoftComparisonHelper
{
    private static readonly NewtonsoftJsonDataSerializer Instance = NewtonsoftJsonDataSerializer.Instance;

    /// <summary>
    /// Serialize with the original Newtonsoft implementation for comparison.
    /// </summary>
    public static string SerializePayload(string messageType, object payload, int version)
        => Instance.SerializePayload(messageType, payload, version);

    /// <summary>
    /// Assert that the current serializer and Newtonsoft produce identical JSON for the same payload.
    /// </summary>
    public static void AssertMatchesNewtonsoft(string messageType, object payload, int version)
    {
        var currentJson = JsonDataSerializer.Instance.SerializePayload(messageType, payload, version);
        var newtonsoftJson = Instance.SerializePayload(messageType, payload, version);

        // Normalize both for comparison: collapse whitespace AND sort object properties
        // so the comparison is order-independent (Newtonsoft and STJ/Jsonite may emit
        // properties in different order depending on the target framework).
        var normalizedCurrent = NormalizeJson(currentJson);
        var normalizedNewtonsoft = NormalizeJson(newtonsoftJson);

        Assert.AreEqual(normalizedNewtonsoft, normalizedCurrent,
            $"Serializer output differs from Newtonsoft for {messageType} v{version}.\n" +
            $"Newtonsoft:\n{newtonsoftJson}\n\nCurrent:\n{currentJson}");
    }

    /// <summary>
    /// Normalize JSON by sorting object properties alphabetically (recursively)
    /// and collapsing whitespace, so comparisons are order- and format-independent.
    /// Uses Newtonsoft JToken which is available on all TFMs.
    /// </summary>
    private static string NormalizeJson(string json)
    {
        if (string.IsNullOrEmpty(json)) return json;
        var token = JToken.Parse(json);
        SortProperties(token);
        return token.ToString(Newtonsoft.Json.Formatting.None);
    }

    private static void SortProperties(JToken token)
    {
        if (token is JObject obj)
        {
            var properties = obj.Properties().OrderBy(p => p.Name, System.StringComparer.Ordinal).ToList();
            obj.RemoveAll();
            foreach (var prop in properties)
            {
                SortProperties(prop.Value);
                obj.Add(prop);
            }
        }
        else if (token is JArray arr)
        {
            foreach (var item in arr)
            {
                SortProperties(item);
            }
        }
    }
}