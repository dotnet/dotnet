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
    [MSBuildMultiThreadableTask]
    public class LocateDotNet : Microsoft.Build.Utilities.Task, IMultiThreadableTask
    {
        private static readonly string s_cacheKey = "LocateDotNet-FCDFF825-F35B-4601-9CB5-74DCA498B589";

        private sealed class CacheEntry
        {
            public readonly string GlobalJsonPath;
            public readonly DateTime LastWrite;
            public readonly string Paths;
            public readonly string Value;

            public CacheEntry(string globalJsonPath, DateTime lastWrite, string paths, string value)
            {
                GlobalJsonPath = globalJsonPath;
                LastWrite = lastWrite;
                Paths = paths;
                Value = value;
            }
        }

        /// <summary>Injected by MSBuild so paths resolve against the project directory in multithreaded builds.</summary>
        public TaskEnvironment TaskEnvironment { get; set; } = TaskEnvironment.Fallback;

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
            var globalJsonPath = TaskEnvironment.GetAbsolutePath(Path.Combine(RepositoryRoot, "global.json"));

            var lastWrite = File.GetLastWriteTimeUtc(globalJsonPath);
            var paths = TaskEnvironment.GetEnvironmentVariable("PATH");

            // The cache is registered per build, not per project, so the repository identity has to
            // be part of the entry. Otherwise a second repository with a coincidentally matching
            // global.json timestamp and PATH would reuse the first repository's dotnet.
            var cachedResult = (CacheEntry)BuildEngine4.GetRegisteredTaskObject(s_cacheKey, RegisteredTaskObjectLifetime.Build);
            if (cachedResult != null &&
                string.Equals(globalJsonPath.Value, cachedResult.GlobalJsonPath, StringComparison.OrdinalIgnoreCase) &&
                lastWrite == cachedResult.LastWrite &&
                paths == cachedResult.Paths)
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
            var dotNetDir = paths.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(p => File.Exists(TaskEnvironment.GetAbsolutePath(Path.Combine(p, fileName))));

            if (dotNetDir == null || !Directory.Exists(TaskEnvironment.GetAbsolutePath(Path.Combine(dotNetDir, "sdk", sdkVersion))))
            {
                Log.LogError($"Unable to find dotnet with SDK version '{sdkVersion}'");
                return;
            }

            DotNetPath = TaskEnvironment.GetAbsolutePath(Path.Combine(dotNetDir, fileName));
            BuildEngine4.RegisterTaskObject(s_cacheKey, new CacheEntry(globalJsonPath.Value, lastWrite, paths, DotNetPath), RegisteredTaskObjectLifetime.Build, allowEarlyCollection: true);
        }
    }
}
