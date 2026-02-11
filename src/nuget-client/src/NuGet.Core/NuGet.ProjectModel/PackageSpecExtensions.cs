// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Frameworks;

namespace NuGet.ProjectModel
{
    public static class PackageSpecExtensions
    {
        /// <summary>
        /// Get the nearest framework available in the project.
        /// </summary>
        public static TargetFrameworkInformation GetTargetFramework(this PackageSpec project, NuGetFramework targetFramework)
        {
            var frameworkInfo = project.TargetFrameworks.FirstOrDefault(f => NuGetFramework.Comparer.Equals(targetFramework, f.FrameworkName));
            if (frameworkInfo == null)
            {
                frameworkInfo = NuGetFrameworkUtility.GetNearest(project.TargetFrameworks,
                    targetFramework,
                    item => item.FrameworkName);
            }

            return frameworkInfo ?? new TargetFrameworkInformation();
        }

        /// <summary>
        /// Get restore metadata framework. This is based on the project's target frameworks, then an 
        /// exact match is found under restore metadata.
        /// </summary>
        public static ProjectRestoreMetadataFrameworkInfo GetRestoreMetadataFramework(this PackageSpec project, NuGetFramework targetFramework)
        {
            ProjectRestoreMetadataFrameworkInfo? frameworkInfo = null;

            var projectFrameworkInfo = GetTargetFramework(project, targetFramework);

            if (projectFrameworkInfo.FrameworkName != null)
            {
                frameworkInfo = project.RestoreMetadata?.TargetFrameworks
                    .FirstOrDefault(f => f.FrameworkName.Equals(projectFrameworkInfo.FrameworkName));
            }

            return frameworkInfo ?? new ProjectRestoreMetadataFrameworkInfo();
        }

        /// <summary>
        /// Get the <see cref="TargetFrameworkInformation"/> for the given target alias.
        /// If there is not a match, null is returned.
        /// </summary>
        /// <param name="project">The project</param>
        /// <param name="targetAlias">The target alias to search for</param>
        /// <returns>The <see cref="TargetFrameworkInformation"/> if it exists, <see langword="null"/> otherwise. </returns>
        public static TargetFrameworkInformation? GetTargetFramework(this PackageSpec project, string? targetAlias)
        {
            foreach (var framework in project.TargetFrameworks.NoAllocEnumerate())
            {
                if (string.Equals(framework.TargetAlias, targetAlias, StringComparison.OrdinalIgnoreCase))
                {
                    return framework;
                }
            }
            return null;
        }

        /// <summary>
        /// Get the <see cref="ProjectRestoreMetadataFrameworkInfo"/> for the given target alias.
        /// If there is not a match, null is returned.
        /// </summary>
        /// <param name="project">The project</param>
        /// <param name="targetAlias">The target alias to search for</param>
        /// <returns>The <see cref="ProjectRestoreMetadataFrameworkInfo"/> if it exists, <see langword="null"/> otherwise. </returns>
        public static ProjectRestoreMetadataFrameworkInfo? GetRestoreMetadataFramework(this PackageSpec project, string? targetAlias)
        {
            if (project.RestoreMetadata != null)
            {
                foreach (var framework in project.RestoreMetadata.TargetFrameworks.NoAllocEnumerate())
                {
                    if (string.Equals(framework.TargetAlias, targetAlias, StringComparison.OrdinalIgnoreCase))
                    {
                        return framework;
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// Gets the nearest target framework within the project based on the given target framework and target alias.
        /// If multiple frameworks match, the target alias is used to disambiguate.
        /// If there are duplicates and no matching alias, then an empty TargetFrameworkInformation is returned.
        /// </summary>
        /// <param name="project">The project spec in question.</param>
        /// <param name="targetFramework">The pivot target framework.</param>
        /// <param name="targetAlias">The alias for disambiguation</param>
        public static TargetFrameworkInformation GetNearestTargetFramework(this PackageSpec project, NuGetFramework targetFramework, string? targetAlias)
        {
            TargetFrameworkInformation? result = null;
            List<TargetFrameworkInformation>? frameworks = null;
            FindMatchingFrameworks(project, targetFramework, ref result, ref frameworks);

            // We matched a single framework.
            if (result != null && frameworks == null)
            {
                return result;
            }

            // No exact match found, find the nearest
            if (result == null || frameworks == null)
            {
                var reducer = new FrameworkReducer(DefaultFrameworkNameProvider.Instance, DefaultCompatibilityProvider.Instance);
                NuGetFramework? mostCompatibleFramework = reducer.GetNearest(targetFramework, project.TargetFrameworks.Select(e => e.FrameworkName));
                if (mostCompatibleFramework != null)
                {
                    FindMatchingFrameworks(project, mostCompatibleFramework, ref result, ref frameworks);
                }
            }

            // We matched a single framework.
            if (result != null && frameworks == null)
            {
                return result;
            }

            // Multiple matchingFrameworks matched exactly, use target alias to disambiguate
            if (result != null && frameworks != null)
            {
                foreach (var framework in frameworks)
                {
                    if (string.Equals(framework.TargetAlias, targetAlias, StringComparison.OrdinalIgnoreCase))
                    {
                        return framework;
                    }
                }
            }

            return new TargetFrameworkInformation();

            static void FindMatchingFrameworks(PackageSpec project, NuGetFramework targetFramework, ref TargetFrameworkInformation? matchedFramework, ref List<TargetFrameworkInformation>? matchingFrameworks)
            {
                foreach (TargetFrameworkInformation framework in project.TargetFrameworks.NoAllocEnumerate())
                {
                    if (NuGetFramework.Comparer.Equals(targetFramework, framework.FrameworkName))
                    {
                        // Result not being null means we have multiple matches.
                        if (matchedFramework != null)
                        {
                            if (matchingFrameworks == null)
                            {
                                matchingFrameworks = [matchedFramework, framework];
                            }
                            else
                            {
                                matchingFrameworks.Add(framework);
                            }
                        }
                        matchedFramework = framework;
                    }
                }
            }
        }
    }
}
