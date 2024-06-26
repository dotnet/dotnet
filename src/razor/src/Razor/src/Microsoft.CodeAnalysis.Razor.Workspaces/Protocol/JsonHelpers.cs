// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.CodeAnalysis.Razor.Protocol;

internal static class JsonHelpers
{
    private const string s_convertedFlag = "__convertedFromJsonElement";

    /// <summary>
    /// Normalizes data from JsonElement to JObject as thats what we expect to process
    /// </summary>
    internal static object? TryConvertFromJsonElement(object? data)
    {
        if (data is JsonElement element)
        {
            var jObject = JObject.Parse(element.GetRawText());
            jObject[s_convertedFlag] = true;
            return jObject;
        }

        return data;
    }

    /// <summary>
    /// Converts from JObject back to JsonElement, but only if the original conversion was done with <see cref="TryConvertFromJsonElement(object?)"/>
    /// </summary>
    internal static object? TryConvertBackToJsonElement(object? data)
    {
        if (data is JObject jObject &&
           jObject.ContainsKey(s_convertedFlag))
        {
            return JsonDocument.Parse(jObject.ToString()).RootElement;
        }

        return data;
    }
}
