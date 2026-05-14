// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet.Versioning;

namespace Microsoft.DotNet.Arcade.Sdk
{
    public class CheckRequiredDotNetVersion : Microsoft.Build.Utilities.Task
    {
        private static readonly string s_cacheKey = "CheckRequiredDotNetVersion-6ED0A075-A4B3-46B1-97D4-448558D515D3";

        private sealed class CacheEntry
        {
            public readonly DateTime LastWrite;
            public readonly bool Success;

            public CacheEntry(DateTime lastWrite, bool success)
            {
                LastWrite = lastWrite;
                Success = success;
            }
        }

        [Required]
        public string RepositoryRoot { get; set; }

        [Required]
        public string SdkVersion { get; set; }

        public override bool Execute()
        {
            if (!SemanticVersion.TryParse(SdkVersion, out var currentSdkVersion))
            {
                Log.LogError($"Invalid version: {SdkVersion}");
                return false;
            }

            var globalJsonPath = Path.Combine(RepositoryRoot, "global.json");
            DateTime lastWrite;
            try
            {
                lastWrite = File.GetLastWriteTimeUtc(globalJsonPath);
            }
            catch (Exception e)
            {
                Log.LogError($"Error accessing file '{globalJsonPath}': {e.Message}");
                return false;
            }

            var cachedResult = (CacheEntry)BuildEngine4.GetRegisteredTaskObject(s_cacheKey, RegisteredTaskObjectLifetime.Build);
            if (cachedResult != null && lastWrite == cachedResult.LastWrite)
            {
                // Error has already been reported if the current SDK version is not sufficient.
                if (!cachedResult.Success)
                {
                    Log.LogMessage(MessageImportance.Low, $"Previous .NET Core SDK version check failed.");
                }

                return cachedResult.Success;
            }

            bool execute()
            {
                string globalJson;
                try
                {
                    globalJson = File.ReadAllText(globalJsonPath);
                }
                catch (Exception e)
                {
                    Log.LogError($"Error reading file '{globalJsonPath}': {e.Message}");
                    return false;
                }

                // avoid Newtonsoft.Json dependency
                var match = Regex.Match(globalJson, $@"""dotnet""\s*:\s*""([^""]+)""");
                if (!match.Success)
                {
                    Log.LogError($"Unable to determine dotnet version from file '{globalJsonPath}'.");
                    return false;
                }

                var minSdkVersionStr = match.Groups[1].Value;
                if (!SemanticVersion.TryParse(minSdkVersionStr, out var minSdkVersion))
                {
                    Log.LogError($"DotNet version specified in '{globalJsonPath}' is invalid: {minSdkVersionStr}.");
                    return false;
                }

                if (currentSdkVersion < minSdkVersion)
                {
                    Log.LogError($"The .NET Core SDK version {currentSdkVersion} is below the minimum required version {minSdkVersion}. You can install newer .NET Core SDK from https://www.microsoft.com/net/download.");
                    return false;
                }

                return true;
            }

            bool success = execute();
            BuildEngine4.RegisterTaskObject(s_cacheKey, new CacheEntry(lastWrite, success), RegisteredTaskObjectLifetime.Build, allowEarlyCollection: true);
            return success;
        }
    }
}
