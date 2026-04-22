// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

using Jsonite;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.JsoniteReference;

/// <summary>
/// A thin wrapper around the Jsonite library for test-level JSON operations.
///
/// Jsonite deserializes JSON into JsonObject (Dictionary&lt;string, object&gt;)
/// and JsonArray (List&lt;object&gt;). It does NOT support typed deserialization
/// to arbitrary CLR types the way System.Text.Json or Newtonsoft do.
///
/// This wrapper is used for:
///   1. Verifying Jsonite can successfully parse JSON produced by System.Text.Json.
///   2. Round-trip fidelity: STJ JSON → Jsonite parse → Jsonite serialize → compare.
///   3. Performance comparisons for raw JSON parsing and serialization.
/// </summary>
internal sealed class JsoniteSerializer
{
    private static JsoniteSerializer? s_instance;

    /// <summary>
    /// Gets the singleton instance.
    /// </summary>
    public static JsoniteSerializer Instance => s_instance ??= new JsoniteSerializer();

    private JsoniteSerializer() { }

    /// <summary>
    /// Parse a JSON string into Jsonite's object graph (JsonObject/JsonArray/primitives).
    /// </summary>
    public object? Parse(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return null;
        }

        return Json.Deserialize(json);
    }

    /// <summary>
    /// Serialize an object graph (JsonObject/JsonArray/primitives) back to a JSON string.
    /// </summary>
    public string Serialize(object? value)
    {
        if (value is null)
        {
            return "null";
        }

        return Json.Serialize(value);
    }

    /// <summary>
    /// Round-trip: parse JSON with Jsonite, then re-serialize to a JSON string.
    /// Useful for normalization and comparison testing.
    /// </summary>
    public string RoundTrip(string json)
    {
        var parsed = Parse(json);
        return Serialize(parsed);
    }

    /// <summary>
    /// Validate that the given JSON string can be parsed by Jsonite without errors.
    /// </summary>
    public bool TryParse(string json, out object? result, out string? error)
    {
        try
        {
            result = Parse(json);
            error = null;
            return true;
        }
        catch (JsonException ex)
        {
            result = null;
            error = ex.ToString();
            return false;
        }
    }

    /// <summary>
    /// Extract the "Payload" member from a parsed message object.
    /// Returns null if the message doesn't have a Payload member.
    /// </summary>
    public object? ExtractPayload(object? parsedMessage)
    {
        if (parsedMessage is JsonObject obj && obj.TryGetValue("Payload", out var payload))
        {
            return payload;
        }

        return null;
    }

    /// <summary>
    /// Extract the "MessageType" member from a parsed message object.
    /// </summary>
    public string? ExtractMessageType(object? parsedMessage)
    {
        if (parsedMessage is JsonObject obj && obj.TryGetValue("MessageType", out var messageType))
        {
            return messageType as string;
        }

        return null;
    }

    /// <summary>
    /// Extract the "Version" member from a parsed versioned message object.
    /// </summary>
    public int? ExtractVersion(object? parsedMessage)
    {
        if (parsedMessage is JsonObject obj && obj.TryGetValue("Version", out var version))
        {
            return version switch
            {
                int i => i,
                long l => (int)l,
                double d => (int)d,
                string s when int.TryParse(s, out int parsed) => parsed,
                _ => null,
            };
        }

        return null;
    }

    /// <summary>
    /// Deep-compare two Jsonite object graphs for structural equality.
    /// Returns true if they are structurally equal.
    /// </summary>
    public static bool DeepEquals(object? a, object? b)
    {
        if (a is null && b is null) return true;
        if (a is null || b is null) return false;

        if (a is JsonObject objA && b is JsonObject objB)
        {
            if (objA.Count != objB.Count) return false;
            foreach (var kvp in objA)
            {
                if (!objB.TryGetValue(kvp.Key, out var otherValue)) return false;
                if (!DeepEquals(kvp.Value, otherValue)) return false;
            }

            return true;
        }

        if (a is JsonArray arrA && b is JsonArray arrB)
        {
            if (arrA.Count != arrB.Count) return false;
            for (int i = 0; i < arrA.Count; i++)
            {
                if (!DeepEquals(arrA[i], arrB[i])) return false;
            }

            return true;
        }

        // For primitives, compare values. Handle numeric type differences.
        if (a is IConvertible && b is IConvertible)
        {
            try
            {
                // Compare as strings for cross-type numeric equality
                string sa = Convert.ToString(a, CultureInfo.InvariantCulture)!;
                string sb = Convert.ToString(b, CultureInfo.InvariantCulture)!;
                return string.Equals(sa, sb, StringComparison.Ordinal);
            }
            catch
            {
                // Fall through to object.Equals
            }
        }

        return object.Equals(a, b);
    }
}
