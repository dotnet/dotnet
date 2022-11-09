// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Tools
{
#if NET472
    [LoadInSeparateAppDomain]
    public sealed class UpdatePackageVersionTask : AppDomainIsolatedTask
    {
        static UpdatePackageVersionTask() => AssemblyResolution.Initialize();
#else
    public class UpdatePackageVersionTask : Microsoft.Build.Utilities.Task
    {
#endif
        public string VersionKind { get; set; }

        [Required]
        public string[] Packages { get; set; }

        [Required]
        public string OutputDirectory { get; set; }

        public bool ExactVersions { get; set; }

        public bool AllowPreReleaseDependencies { get; set; }

        public override bool Execute()
        {
#if NET472
            AssemblyResolution.Log = Log;
#endif
            try
            {
                ExecuteImpl();
                return !Log.HasLoggedErrors;
            }
            finally
            {
#if NET472
                AssemblyResolution.Log = null;
#endif
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
                NuGetVersionUpdater.Run(Packages, OutputDirectory, translation, ExactVersions, allowPreReleaseDependency: (packageId, dependencyId, dependencyVersion) =>
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
                    File.WriteAllLines(Path.Combine(OutputDirectory, "PreReleaseDependencies.txt"), preReleaseDependencies.Distinct());
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
}
