// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using NuGet.Frameworks;
using NuGet.ProjectModel;

namespace Test.Utility.Commands
{
    public class PackageSpecOperationsUtility
    {
        public static void AddTargetFramework(PackageSpec packageSpec, string frameworkName)
        {
            var nugetFramework = NuGetFramework.Parse(frameworkName);
            if (!packageSpec.TargetFrameworks.Any(e => e.FrameworkName.Equals(nugetFramework)))
            {
                packageSpec.TargetFrameworks.Add(new TargetFrameworkInformation() { FrameworkName = nugetFramework });
                if (packageSpec.RestoreMetadata != null)
                {
                    packageSpec.RestoreMetadata.TargetFrameworks.Add(new ProjectRestoreMetadataFrameworkInfo() { FrameworkName = nugetFramework });
                    packageSpec.RestoreMetadata.OriginalTargetFrameworks.Add(frameworkName);
                    packageSpec.RestoreMetadata.CrossTargeting = packageSpec.RestoreMetadata.OriginalTargetFrameworks.Count > 1;
                }
            }
            else
            {
                throw new InvalidOperationException($"The target framework {frameworkName} already exists");
            }
        }

        public static void RemoveTargetFramework(PackageSpec packageSpec, string frameworkName)
        {
            var nugetFramework = NuGetFramework.Parse(frameworkName);
            var targetFrameworkToRemove = packageSpec.TargetFrameworks.FirstOrDefault(e => e.FrameworkName.Equals(nugetFramework));
            if (targetFrameworkToRemove != null)
            {
                packageSpec.TargetFrameworks.Remove(targetFrameworkToRemove);

                if (packageSpec.RestoreMetadata != null)
                {
                    // Assume that it has to be here.
                    var projectRestoreMetadataFrameworkInfoToRemove = packageSpec.RestoreMetadata.TargetFrameworks.First(e => e.FrameworkName.Equals(nugetFramework));
                    packageSpec.RestoreMetadata.TargetFrameworks.Remove(projectRestoreMetadataFrameworkInfoToRemove);
                    packageSpec.RestoreMetadata.OriginalTargetFrameworks.Remove(frameworkName);
                    packageSpec.RestoreMetadata.CrossTargeting = packageSpec.RestoreMetadata.OriginalTargetFrameworks.Count > 1;
                }
            }
            else
            {
                throw new InvalidOperationException($"The target framework {frameworkName} does not exist");
            }
        }

        public static void AddProjectReference(PackageSpec spec, PackageSpec child, params NuGetFramework[] frameworks)
        {
            if (frameworks.Length == 0)
            {
                // Use all frameworks if none were given
                frameworks = spec.TargetFrameworks.Select(e => e.FrameworkName).ToArray();
            }

            foreach (var framework in spec
                .RestoreMetadata
                .TargetFrameworks
                .Where(e => frameworks.Contains(e.FrameworkName)))
            {
                framework.ProjectReferences.Add(new ProjectRestoreReference()
                {
                    ProjectUniqueName = child.RestoreMetadata.ProjectUniqueName,
                    ProjectPath = child.RestoreMetadata.ProjectPath
                });
            }
        }
    }
}
