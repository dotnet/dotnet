// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Text.Json;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Protocol;

internal static class JsonHelpers
{
    private static readonly Lazy<JsonSerializerOptions> s_roslynLspJsonSerializerOptions = new(CreateRoslynLspJsonSerializerOptions);
    private static readonly Lazy<JsonSerializerOptions> s_vsLspJsonSerializerOptions = new(CreateVsLspJsonSerializerOptions);

    /// <summary>
    /// Serializer options to use when serializing or deserializing a Roslyn LSP type
    /// </summary>
    internal static JsonSerializerOptions RoslynLspJsonSerializerOptions => s_roslynLspJsonSerializerOptions.Value;

    /// <summary>
    /// Serializer options to use when serializing or deserializing a VS Platform LSP type
    /// </summary>
    internal static JsonSerializerOptions VsLspJsonSerializerOptions => s_vsLspJsonSerializerOptions.Value;

    /// <summary>
    /// Converts a Roslyn LSP object to a VS Platform LSP object via serializing to text and deserializing to VS LSP type
    /// </summary>
    internal static TVsLspResult? ToVsLSP<TVsLspResult, TRoslynLspSource>(TRoslynLspSource source)
    {
        return JsonSerializer.Deserialize<TVsLspResult>(JsonSerializer.SerializeToDocument(source, RoslynLspJsonSerializerOptions), VsLspJsonSerializerOptions);
    }

    /// <summary>
    /// Converts a VS Platform LSP object to a Roslyn LSP object via serializing to text and deserializing to Roslyn LSP type
    /// </summary>
    internal static TRoslynLspResult? ToRoslynLSP<TRoslynLspResult, TVsLspSource>(TVsLspSource? source)
    {
        return JsonSerializer.Deserialize<TRoslynLspResult>(JsonSerializer.SerializeToDocument(source, VsLspJsonSerializerOptions), RoslynLspJsonSerializerOptions);
    }

    /// <summary>
    /// Adds VS Platform LSP converters for VSInternal variation of types (e.g. VSInternalClientCapability from ClientCapability)
    /// </summary>
    internal static void AddVSInternalExtensionConverters(JsonSerializerOptions serializerOptions)
    {
        // In its infinite wisdom, the LSP client has a public method that takes Newtonsoft.Json types, but an internal method that takes System.Text.Json types.
        typeof(VSInternalExtensionUtilities).GetMethod("AddVSInternalExtensionConverters", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)!.Invoke(null, [serializerOptions]);
    }

    private static JsonSerializerOptions CreateRoslynLspJsonSerializerOptions()
    {
        var serializerOptions = new JsonSerializerOptions();

        foreach (var converter in RazorServiceDescriptorsWrapper.GetLspConverters())
        {
            serializerOptions.Converters.Add(converter);
        }

        return serializerOptions;
    }

    private static JsonSerializerOptions CreateVsLspJsonSerializerOptions()
    {
        var serializerOptions = new JsonSerializerOptions();

        AddVSInternalExtensionConverters(serializerOptions);
        return serializerOptions;
    }
}
