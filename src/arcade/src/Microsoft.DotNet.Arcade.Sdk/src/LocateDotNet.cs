// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Arcade.Sdk
{
    public class LocateDotNet : Microsoft.Build.Utilities.Task
    {
        private static readonly string s_cacheKey = "LocateDotNet-FCDFF825-F35B-4601-9CB5-74DCA498B589";

        private sealed class CacheEntry
        {
            public readonly DateTime LastWrite;
            public readonly string Paths;
            public readonly string Value;

            public CacheEntry(DateTime lastWrite, string paths, string value)
            {
                LastWrite = lastWrite;
                Paths = paths;
                Value = value;
            }
        }

        [Required]
        public string RepositoryRoot { get; set; }

        [Output]
        public string DotNetPath { get; set; }

        public override bool Execute()
        {
            ExecuteImpl();
            return !Log.HasLoggedErrors;
        }

        private void ExecuteImpl()
        {
            var globalJsonPath = Path.Combine(RepositoryRoot, "global.json");

            var lastWrite = File.GetLastWriteTimeUtc(globalJsonPath);
            var paths = Environment.GetEnvironmentVariable("PATH");

            var cachedResult = (CacheEntry)BuildEngine4.GetRegisteredTaskObject(s_cacheKey, RegisteredTaskObjectLifetime.Build);
            if (cachedResult != null && lastWrite == cachedResult.LastWrite && paths == cachedResult.Paths)
            {
                Log.LogMessage(MessageImportance.Low, $"Reused cached value.");
                DotNetPath = cachedResult.Value;
                return;
            }

            var globalJson = File.ReadAllText(globalJsonPath);

            // avoid Newtonsoft.Json dependency
            var match = Regex.Match(globalJson, @"""dotnet""\s*:\s*""([^""]+)""");
            if (!match.Success)
            {
                Log.LogError($"Unable to determine dotnet version from file '{globalJsonPath}'.");
                return;
            }

            var sdkVersion = match.Groups[1].Value;

            var fileName = (Path.DirectorySeparatorChar == '\\') ? "dotnet.exe" : "dotnet";
            var dotNetDir = paths.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(p => File.Exists(Path.Combine(p, fileName)));

            if (dotNetDir == null || !Directory.Exists(Path.Combine(dotNetDir, "sdk", sdkVersion)))
            {
                Log.LogError($"Unable to find dotnet with SDK version '{sdkVersion}'");
                return;
            }

            DotNetPath = Path.GetFullPath(Path.Combine(dotNetDir, fileName));
            BuildEngine4.RegisterTaskObject(s_cacheKey, new CacheEntry(lastWrite, paths, DotNetPath), RegisteredTaskObjectLifetime.Build, allowEarlyCollection: true);
        }
    }
}
