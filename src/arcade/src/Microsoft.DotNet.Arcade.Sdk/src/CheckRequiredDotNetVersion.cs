// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet.Versioning;

namespace Microsoft.DotNet.Arcade.Sdk;

public class CheckRequiredDotNetVersion : Microsoft.Build.Utilities.Task
{
    private readonly record struct CacheKey(string GlobalJsonPath, string SdkVersion, DateTime LastWrite);

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

        var cacheKey = new CacheKey(globalJsonPath, SdkVersion, lastWrite);
        if (BuildEngine4.GetRegisteredTaskObject(cacheKey, RegisteredTaskObjectLifetime.Build) is bool cachedSuccess)
        {
            // Error has already been reported if the current SDK version is not sufficient.
            if (!cachedSuccess)
            {
                Log.LogMessage(MessageImportance.Low, $"Previous .NET Core SDK version check failed.");
            }

            return cachedSuccess;
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
        BuildEngine4.RegisterTaskObject(cacheKey, success, RegisteredTaskObjectLifetime.Build, allowEarlyCollection: true);
        return success;
    }
}
