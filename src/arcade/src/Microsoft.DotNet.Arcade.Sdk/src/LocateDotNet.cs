// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Arcade.Sdk;

[MSBuildMultiThreadableTask]
public class LocateDotNet : Task, IMultiThreadableTask
{
    private readonly record struct CacheKey(AbsolutePath GlobalJsonPath, DateTime LastWrite, string Paths);

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

        // GetEnvironmentVariable is nullable, and under multithreaded execution it is a lookup in a
        // per-project environment rather than the process block, so a missing PATH is more reachable
        // than it used to be. Fail with a clear message instead of a NullReferenceException below.
        if (string.IsNullOrEmpty(paths))
        {
            Log.LogError("Unable to locate dotnet because the PATH environment variable is not set.");
            return;
        }

        // The read/write pair below is not atomic, so under multithreaded execution two threads
        // can both miss and both compute the value. That is benign here: the computation is pure
        // and deterministic for a given (global.json, timestamp, PATH), so the loser of the race
        // has its registration dropped and the winner's identical entry stands. The cache is an
        // optimization, not a lock.
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
        // Split with RemoveEmptyEntries, so no entry is empty and GetAbsolutePath cannot reject one.
        var dotNetDir = paths.Split(new[] { Path.PathSeparator }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(p => File.Exists(TaskEnvironment.GetAbsolutePath(Path.Combine(p, fileName))));

        if (dotNetDir == null || !Directory.Exists(TaskEnvironment.GetAbsolutePath(Path.Combine(dotNetDir, "sdk", sdkVersion))))
        {
            Log.LogError($"Unable to find dotnet with SDK version '{sdkVersion}'");
            return;
        }

        // GetAbsolutePath absolutizes but does not canonicalize, so canonicalize explicitly to keep
        // this [Output] in the same form the previous Path.GetFullPath produced. PATH entries
        // routinely carry '..' segments and trailing separators.
        DotNetPath = TaskEnvironment.GetAbsolutePath(Path.Combine(dotNetDir, fileName)).GetCanonicalForm();
        BuildEngine4.RegisterTaskObject(cacheKey, DotNetPath, RegisteredTaskObjectLifetime.Build, allowEarlyCollection: true);
    }
}
