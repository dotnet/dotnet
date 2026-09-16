// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Tools;

[MSBuildMultiThreadableTask]
public class UpdatePackageVersionTask : Task, IMultiThreadableTask
{
    /// <summary>Injected by MSBuild so paths resolve against the project directory in multithreaded builds.</summary>
    public TaskEnvironment TaskEnvironment { get; set; } = TaskEnvironment.Fallback;

    public string VersionKind { get; set; }

    [Required]
    public string[] Packages { get; set; }

    [Required]
    public string OutputDirectory { get; set; }

    public bool ExactVersions { get; set; }

    public bool AllowPreReleaseDependencies { get; set; }

    public override bool Execute()
    {
        try
        {
            ExecuteImpl();
            return !Log.HasLoggedErrors;
        }
        finally
        {
        }
    }

    private void ExecuteImpl()
    {
        VersionTranslation translation;
        if (string.IsNullOrEmpty(VersionKind))
        {
            translation = VersionTranslation.None;
        }
        else if (StringComparer.OrdinalIgnoreCase.Equals(VersionKind, "release"))
        {
            translation = VersionTranslation.Release;
        }
        else if (StringComparer.OrdinalIgnoreCase.Equals(VersionKind, "prerelease"))
        {
            translation = VersionTranslation.PreRelease;
        }
        else
        {
            Log.LogError($"Invalid value for task argument {nameof(VersionKind)}: '{VersionKind}'. Specify 'release' or 'prerelease' or leave empty.");
            return;
        }

        var preReleaseDependencies = new List<string>();

        try
        {
            // OutputDirectory is [Required], so MSBuild fails the task with MSB4044 before Execute runs
            // if it is unset or expands to an empty string. Always pass a non-null directory here:
            // NuGetVersionUpdater treats a null directory as read-only mode and writes no packages.
            var outputDirectory = new DirectoryInfo(TaskEnvironment.GetAbsolutePath(OutputDirectory));

            NuGetVersionUpdater.Run(Packages.Select(p => new FileInfo(TaskEnvironment.GetAbsolutePath(p))), outputDirectory, translation, ExactVersions, allowPreReleaseDependency: (packageId, dependencyId, dependencyVersion) =>
            {
                if (AllowPreReleaseDependencies)
                {
                    Log.LogMessage(MessageImportance.High, $"Package '{packageId}' depends on a pre-release package '{dependencyId}, {dependencyVersion}'");
                    preReleaseDependencies.Add($"{dependencyId}, {dependencyVersion}");
                    return true;
                }

                return false;
            });

            if (translation == VersionTranslation.Release)
            {
                File.WriteAllLines(Path.Combine(outputDirectory.FullName, "PreReleaseDependencies.txt"), preReleaseDependencies.Distinct());
            }
        }
        catch (AggregateException e)
        {
            foreach (var inner in e.InnerExceptions)
            {
                Log.LogErrorFromException(inner);
            }
        }
        catch (Exception e)
        {
            Log.LogErrorFromException(e);
        }

    }
}
