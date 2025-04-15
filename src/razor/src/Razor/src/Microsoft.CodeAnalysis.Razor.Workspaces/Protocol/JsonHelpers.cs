// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;

namespace Microsoft.CodeAnalysis.Razor.Protocol;

internal static class JsonHelpers
{
    private static readonly Lazy<JsonSerializerOptions> s_jsonSerializerOptions = new(CreateJsonSerializerOptions);

    /// <summary>
    /// Serializer options to use when serializing or deserializing a Roslyn LSP type
    /// </summary>
    internal static JsonSerializerOptions JsonSerializerOptions => s_jsonSerializerOptions.Value;

    /// <summary>
    /// Converts an LSP object to a different LSP object, either by casting or serializing and deserializing
    /// </summary>
    internal static TResult? Convert<TSource, TResult>(TSource? source)
    {
        if (source is TResult result)
        {
            return result;
        }

        return JsonSerializer.Deserialize<TResult>(JsonSerializer.SerializeToDocument(source, JsonSerializerOptions), JsonSerializerOptions);
    }

    /// <summary>
    /// Converts an array of LSP objects to a different LSP object, either by casting or serializing and deserializing
    /// </summary>
    internal static TResult[] ConvertAll<TSource, TResult>(TSource[] source)
    {
        using var results = new PooledArrayBuilder<TResult>(source.Length);
        foreach (var item in source)
        {
            if (Convert<TSource, TResult>(item) is { } converted)
            {
                results.Add(converted);
            }
            else
            {
                Debug.Fail("Could not convert item to expected type.");
            }
        }

        return results.ToArray();
    }

    private static JsonSerializerOptions CreateJsonSerializerOptions()
    {
        var serializerOptions = new JsonSerializerOptions();

        foreach (var converter in RazorServiceDescriptorsWrapper.GetLspConverters())
        {
            serializerOptions.Converters.Add(converter);
        }

        return serializerOptions;
    }
}
