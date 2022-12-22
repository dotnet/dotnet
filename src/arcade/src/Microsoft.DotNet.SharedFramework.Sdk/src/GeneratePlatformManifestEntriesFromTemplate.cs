// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.DotNet.SharedFramework.Sdk
{
    public class GeneratePlatformManifestEntriesFromTemplate : BuildTask
    {
        [Required]
        public ITaskItem[] PlatformManifestEntryTemplates { get; set; }

        [Required]
        public ITaskItem[] Files { get; set; }

        public string FallbackAssemblyVersion { get; set; }

        public string FallbackFileVersion { get; set; }

        [Output]
        public ITaskItem[] PlatformManifestEntries { get; set; }

        public override bool Execute()
        {
            List<PlatformManifestEntry> entries = new List<PlatformManifestEntry>();
            var files = Files.ToLookup(file => Path.GetFileName(file.ItemSpec)).ToDictionary(l => l.Key, l=> l.First());
            foreach (var entryTemplate in PlatformManifestEntryTemplates)
            {
                if (files.TryGetValue(entryTemplate.ItemSpec, out ITaskItem existingFile))
                {
                    // This file in the platform manifest template exists on this platform.
                    // Use the information from the file itself in its entry.
                    entries.Add(new PlatformManifestEntry
                    {
                        Name = entryTemplate.ItemSpec,
                        AssemblyVersion = FileUtilities.GetAssemblyName(existingFile.ItemSpec)?.Version.ToString() ?? string.Empty,
                        FileVersion = FileUtilities.GetFileVersion(existingFile.ItemSpec)?.ToString() ?? string.Empty
                    });
                }
                else
                {
                    // This file does not exist on this platform. It only exists on another platform.
                    var isNativeEntry = entryTemplate.GetMetadata("IsNative") == "true";
                    var entryTemplateExtension = Path.GetExtension(entryTemplate.ItemSpec);
                    // Use file version 0.0.0.0 for non-Windows executable and shared library files
                    bool useFileVersionZero = isNativeEntry && entryTemplateExtension != ".dll" && entryTemplateExtension != ".exe";
                    string assemblyVersion = string.Empty;
                    string fileVersion = string.Empty;
                    if (isNativeEntry)
                    {
                        assemblyVersion = "";
                    }
                    else
                    {
                        string localAssemblyVersionFallback = entryTemplate.GetMetadata("FallbackAssemblyVersion");
                        assemblyVersion = !string.IsNullOrEmpty(localAssemblyVersionFallback) ? localAssemblyVersionFallback : FallbackAssemblyVersion;
                    }
                    if (useFileVersionZero)
                    {
                        fileVersion = "0.0.0.0";
                    }
                    else
                    {
                        string localFileVersionFallback = entryTemplate.GetMetadata("FallbackFileVersion");
                        fileVersion = !string.IsNullOrEmpty(localFileVersionFallback) ? localFileVersionFallback : FallbackFileVersion;
                    }
                    entries.Add(new PlatformManifestEntry
                    {
                        Name = entryTemplate.ItemSpec,
                        AssemblyVersion = assemblyVersion,
                        FileVersion = fileVersion,
                        IsNative = isNativeEntry
                    });
                }
            }

            PlatformManifestEntries = entries.Select(entry =>
                {
                    var item = new TaskItem(entry.Name);
                    if (string.IsNullOrEmpty(entry.AssemblyVersion) && !entry.IsNative)
                    {
                        Log.LogError($"The platform manifest entry for '{entry.Name}' does not have a fallback assembly version specified and is not built on this platform.");
                    }
                    item.SetMetadata("AssemblyVersion", entry.AssemblyVersion);
                    if (string.IsNullOrEmpty(entry.FileVersion))
                    {
                        Log.LogError($"The platform manifest entry for '{entry.Name}' does not have a fallback file version specified and is not built on this platform.");
                    }
                    item.SetMetadata("FileVersion", entry.FileVersion);
                    return item;
                }).ToArray();
            return !Log.HasLoggedErrors;
        }
    }
}
