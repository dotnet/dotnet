// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Jsonite;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.MTP;

/// <summary>
/// Small accessors over the Jsonite JSON object model used by the MTP JSON-RPC client.
///
/// The MTP wire is serialized with Jsonite (not System.Text.Json) so that the client works on every
/// framework we ship — including the .NET Framework runner, where taking a dependency on
/// System.Text.Json would introduce binding-redirect fallout in hosts that run without them. Parsed
/// JSON is a plain object graph: objects are <see cref="JsonObject"/> (a
/// <c>Dictionary&lt;string, object&gt;</c>), arrays are <see cref="JsonArray"/> (a
/// <c>List&lt;object&gt;</c>), numbers are <c>int</c>/<c>long</c>/<c>double</c>, and everything else
/// is <c>string</c>/<c>bool</c>/<c>null</c>.
/// </summary>
internal static class MtpJson
{
    public static JsonObject? AsObject(object? node) => node as JsonObject;

    public static JsonArray? AsArray(object? node) => node as JsonArray;

    public static object? GetValue(JsonObject? node, string key)
        => node is not null && node.TryGetValue(key, out object? value) ? value : null;

    public static string? GetString(JsonObject? node, string key)
        => GetValue(node, key) as string;

    public static bool TryGetInt(JsonObject? node, string key, out int result)
        => TryToInt(GetValue(node, key), out result);

    public static bool TryGetDouble(JsonObject? node, string key, out double result)
    {
        switch (GetValue(node, key))
        {
            case double d: result = d; return true;
            case int i: result = i; return true;
            case long l: result = l; return true;
            case decimal m: result = (double)m; return true;
            default: result = 0; return false;
        }
    }

    public static bool TryToInt(object? value, out int result)
    {
        switch (value)
        {
            case int i: result = i; return true;
            case long l: result = unchecked((int)l); return true;
            case double d: result = (int)d; return true;
            case decimal m: result = (int)m; return true;
            default: result = 0; return false;
        }
    }
}
