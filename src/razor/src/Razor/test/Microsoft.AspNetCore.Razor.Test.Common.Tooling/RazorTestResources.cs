// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Serialization.Json;

namespace Microsoft.AspNetCore.Razor.Test.Common;

internal static class RazorTestResources
{
    public const string BlazorServerAppTagHelpersJson = "BlazorServerApp.TagHelpers.json";

    private static ImmutableArray<TagHelperDescriptor>? s_blazorServerAppTagHelpers;

    private readonly static Dictionary<(string Name, string? Folder), string> s_textMap = new();
    private readonly static Dictionary<(string Name, string? Folder), byte[]> s_bytesMap = new();

    private static string GetResourceName(string name, string? folder)
        => folder is not null
            ? $"{typeof(RazorTestResources).Namespace}.Resources.{folder}.{name}"
            : $"{typeof(RazorTestResources).Namespace}.Resources.{name}";

    private static Stream GetResourceStream(string name, string? folder = null)
    {
        var resourceName = GetResourceName(name, folder);

        return Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Could not find resource: {resourceName}");
    }

    public static string GetResourceText(string name, string? folder = null)
    {
        lock (s_textMap)
        {
            var key = (name, folder);

            if (s_textMap.TryGetValue(key, out var value))
            {
                return value;
            }

            using var stream = GetResourceStream(name, folder);
            using var reader = new StreamReader(stream);

            value = reader.ReadToEnd();

            s_textMap.Add(key, value);

            return value;
        }
    }

    public static byte[] GetResourceBytes(string name, string? folder = null)
    {
        lock (s_bytesMap)
        {
            var key = (name, folder);

            if (s_bytesMap.TryGetValue(key, out var value))
            {
                return value;
            }

            using var stream = GetResourceStream(name, folder);

            value = new byte[stream.Length];
#if NET
            stream.ReadExactly(value);
#else
            stream.Read(value, 0, value.Length);
#endif

            s_bytesMap.Add(key, value);

            return value;
        }
    }

    public static ImmutableArray<TagHelperDescriptor> BlazorServerAppTagHelpers
    {
        get
        {
            return s_blazorServerAppTagHelpers ??= ReadBlazorServerAppTagHelpers();

            static ImmutableArray<TagHelperDescriptor> ReadBlazorServerAppTagHelpers()
            {
                var bytes = GetResourceBytes(BlazorServerAppTagHelpersJson);

                return JsonDataConvert.DeserializeTagHelperArray(bytes);
            }
        }
    }
}
