// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

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
            ProjectRestoreMetadataFrameworkInfo frameworkInfo = null;

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
        public static TargetFrameworkInformation GetTargetFramework(this PackageSpec project, string targetAlias)
        {
            foreach (var framework in project.TargetFrameworks)
            {
                if (framework.TargetAlias.Equals(targetAlias))
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
        public static ProjectRestoreMetadataFrameworkInfo GetRestoreMetadataFramework(this PackageSpec project, string targetAlias)
        {
            foreach (var framework in project.RestoreMetadata?.TargetFrameworks)
            {
                if (framework.TargetAlias.Equals(targetAlias))
                {
                    return framework;
                }
            }
            return null;
        }
    }
}
