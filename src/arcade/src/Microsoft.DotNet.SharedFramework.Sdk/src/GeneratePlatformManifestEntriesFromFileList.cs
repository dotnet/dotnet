// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.DotNet.SharedFramework.Sdk
{
    public class GeneratePlatformManifestEntriesFromFileList : BuildTask
    {
        [Required]
        public ITaskItem[] Files { get; set; }

        [Output]
        public ITaskItem[] PlatformManifestEntries { get; set; }

        public override bool Execute()
        {
            var entries = new List<PlatformManifestEntry>();
            foreach (var file in Files)
            {
                entries.Add(new PlatformManifestEntry
                {
                    Name = file.ItemSpec,
                    AssemblyVersion = FileUtilities.GetAssemblyName(file.GetMetadata("OriginalFilePath"))?.Version.ToString() ?? string.Empty,
                    FileVersion = FileUtilities.GetFileVersion(file.GetMetadata("OriginalFilePath"))?.ToString() ?? string.Empty
                });
            }

            PlatformManifestEntries = entries.Select(entry =>
            {
                var item = new TaskItem(entry.Name);
                item.SetMetadata("AssemblyVersion", entry.AssemblyVersion);
                item.SetMetadata("FileVersion", entry.FileVersion);
                return item;
            }).ToArray();
            return true;
        }
    }
}
