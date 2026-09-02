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
        private readonly record struct CacheKey(string GlobalJsonPath, DateTime LastWrite, string Paths);

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

            var cacheKey = new CacheKey(globalJsonPath, lastWrite, paths);
            if (BuildEngine4.GetRegisteredTaskObject(cacheKey, RegisteredTaskObjectLifetime.Build) is string cachedPath)
            {
                Log.LogMessage(MessageImportance.Low, $"Reused cached value.");
                DotNetPath = cachedPath;
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
            var dotNetDir = paths.Split(new[] { Path.PathSeparator }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(p => File.Exists(Path.Combine(p, fileName)));

            if (dotNetDir == null || !Directory.Exists(Path.Combine(dotNetDir, "sdk", sdkVersion)))
            {
                Log.LogError($"Unable to find dotnet with SDK version '{sdkVersion}'");
                return;
            }

            DotNetPath = Path.GetFullPath(Path.Combine(dotNetDir, fileName));
            BuildEngine4.RegisterTaskObject(cacheKey, DotNetPath, RegisteredTaskObjectLifetime.Build, allowEarlyCollection: true);
        }
    }
}
