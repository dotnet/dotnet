// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BenchmarkDotNet.Filters;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks;

internal static class Resources
{
    private readonly static Dictionary<(string Name, string? Folder), string> s_textMap = new();
    private readonly static object s_gate = new();

    private static string GetResourceName(string name, string? folder)
        => folder is not null
            ? $"{typeof(Resources).Namespace}.Resources.{folder}.{name}"
            : $"{typeof(Resources).Namespace}.Resources.{name}";

    private static Stream GetResourceStream(string name, string? folder = null)
    {
        var resourceName = GetResourceName(name, folder);

        return Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Could not find resource: {resourceName}");
    }

    public static string GetResourceText(string name, string? folder = null)
    {
        lock (s_gate)
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
}
