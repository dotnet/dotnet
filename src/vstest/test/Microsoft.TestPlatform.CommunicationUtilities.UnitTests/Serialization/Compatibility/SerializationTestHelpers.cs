// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

/// <summary>
/// Shared helpers for serialization tests. Uses Newtonsoft JToken for JSON
/// normalization on all TFMs (Newtonsoft.Json is referenced on every TFM).
/// </summary>
internal static class SerializationTestHelpers
{
    /// <summary>
    /// Assert two JSON strings are semantically equal (ignoring whitespace).
    /// </summary>
    public static void AssertJsonEqual(string expected, string actual, string? message = null)
    {
        Assert.AreEqual(Normalize(expected), Normalize(actual),
            message ?? $"JSON mismatch.\nExpected:\n{expected}\n\nActual:\n{actual}");
    }

    /// <summary>
    /// Minify a JSON string to remove whitespace.
    /// </summary>
    public static string Minify(string json)
    {
        return Normalize(json);
    }

    private static string Normalize(string json)
    {
        return Newtonsoft.Json.Linq.JToken.Parse(json).ToString(Newtonsoft.Json.Formatting.None);
    }
}
