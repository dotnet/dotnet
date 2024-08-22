// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
    }
}
