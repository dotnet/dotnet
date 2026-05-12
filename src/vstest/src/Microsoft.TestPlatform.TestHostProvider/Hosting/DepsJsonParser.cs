// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if !NETCOREAPP
using System;
using System.Collections.Generic;
using System.IO;

using Jsonite;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Hosting;

/// <summary>
/// Minimal deps.json parser using Jsonite. Replaces DependencyContextJsonReader
/// to avoid System.Text.Json transitive dependency on .NET Framework.
/// </summary>
internal static class DepsJsonParser
{
    public static DepsJsonLibrary? FindRuntimeLibrary(Stream stream, string libraryName)
    {
        using var reader = new StreamReader(stream);
        if (Json.Deserialize(reader) is not IDictionary<string, object> root)
        {
            return null;
        }

        // Get the runtime target name: runtimeTarget.name
        string? targetName = root.TryGetValue("runtimeTarget", out var rt)
            && rt is IDictionary<string, object> runtimeTarget
            && runtimeTarget.TryGetValue("name", out var tn)
            ? tn?.ToString()
            : null;
        if (targetName == null)
        {
            return null;
        }

        // Look up the target entries for the runtime target
        if (!root.TryGetValue("targets", out var targetsObj)
            || targetsObj is not IDictionary<string, object> targets
            || !targets.TryGetValue(targetName, out var teObj)
            || teObj is not IDictionary<string, object> targetEntries)
        {
            return null;
        }

        // Find the entry matching "libraryName/version" (case-insensitive)
        string prefix = libraryName + "/";
        string? matchingKey = null;
        IDictionary<string, object>? matchingEntry = null;
        foreach (var kvp in targetEntries)
        {
            if (kvp.Key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
                && kvp.Value is IDictionary<string, object> entry)
            {
                matchingKey = kvp.Key;
                matchingEntry = entry;
                break;
            }
        }

        if (matchingKey == null || matchingEntry == null)
        {
            return null;
        }

        // Extract name and version from "Name/Version" key
        int slashIndex = matchingKey.IndexOf('/');
        string name = matchingKey.Substring(0, slashIndex);
        string version = matchingKey.Substring(slashIndex + 1);

        // Collect runtime assembly paths (keys of the "runtime" dictionary)
        var runtimeAssemblyPaths = new List<string>();
        if (matchingEntry.TryGetValue("runtime", out var runtimeObj)
            && runtimeObj is IDictionary<string, object> runtime)
        {
            foreach (var key in runtime.Keys)
            {
                runtimeAssemblyPaths.Add(key);
            }
        }

        // Get the path from the "libraries" section
        string? path = null;
        if (root.TryGetValue("libraries", out var libsObj)
            && libsObj is IDictionary<string, object> libraries)
        {
            foreach (var kvp in libraries)
            {
                if (kvp.Key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
                    && kvp.Value is IDictionary<string, object> libEntry
                    && libEntry.TryGetValue("path", out var p))
                {
                    path = p?.ToString();
                    break;
                }
            }
        }

        return new DepsJsonLibrary
        {
            Name = name,
            Version = version,
            Path = path,
            RuntimeAssemblyPaths = runtimeAssemblyPaths,
        };
    }
}

internal class DepsJsonLibrary
{
    public string Name { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string? Path { get; set; }
    public List<string> RuntimeAssemblyPaths { get; set; } = new();
}
#endif
