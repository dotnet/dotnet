// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;

namespace NuGet.SolutionRestoreManager.Test
{
    /// <summary>
    /// Helper class providing a method of building <see cref="IVsProjectRestoreInfo"/>
    /// out of <see cref="PackageSpec"/>.
    /// </summary>
    internal class ProjectRestoreInfoBuilder
    {
        private readonly VsProjectRestoreInfo _projectRestoreInfo;
        private readonly VsProjectRestoreInfo2 _projectRestoreInfo2;

        private ProjectRestoreInfoBuilder(VsProjectRestoreInfo pri, VsProjectRestoreInfo2 pri2)
        {
            _projectRestoreInfo = pri;
            _projectRestoreInfo2 = pri2;
        }

        /// <summary>
        /// Creates project restore info object to be consumed by <see cref="IVsSolutionRestoreService"/>.
        /// </summary>
        /// <param name="packageSpec">Source project restore object</param>
        /// <returns>Desired project restore object</returns>
        public static ProjectRestoreInfoBuilder FromPackageSpec(
            PackageSpec packageSpec,
            string baseIntermediatePath,
            bool crossTargeting)
        {
            if (packageSpec == null)
            {
                throw new ArgumentNullException(nameof(packageSpec));
            }

            if (packageSpec.TargetFrameworks == null)
            {
                return null;
            }

            var projectProperties = new VsProjectProperties { };

            if (packageSpec.Version != null)
            {
                projectProperties = new VsProjectProperties
                {
                    { "PackageVersion", packageSpec.Version.ToString() }
                };
            }

            var targetFrameworks = new VsTargetFrameworks(
                packageSpec
                    .TargetFrameworks
                    .Select(tfm => ToTargetFrameworkInfo(tfm, projectProperties)));

            var targetFrameworks2 = new VsTargetFrameworks2(
                packageSpec
                    .TargetFrameworks
                    .Select(tfm => ToTargetFrameworkInfo2(tfm, projectProperties)));

            var pri = new VsProjectRestoreInfo(
                baseIntermediatePath,
                targetFrameworks);

            var pri2 = new VsProjectRestoreInfo2(
                baseIntermediatePath,
                targetFrameworks2);

            if (crossTargeting)
            {
                pri.OriginalTargetFrameworks = string.Join(";",
                    packageSpec
                        .TargetFrameworks
                        .Select(tfm => tfm.FrameworkName.GetShortFolderName()));
            }

            return new ProjectRestoreInfoBuilder(pri, pri2);
        }

        public ProjectRestoreInfoBuilder WithTool(string name, string version)
        {
            var properties = new VsReferenceProperties
            {
                { "Version", version }
            };

            _projectRestoreInfo.ToolReferences = new VsReferenceItems
            {
                new VsReferenceItem(name, properties)
            };

            _projectRestoreInfo2.ToolReferences = new VsReferenceItems
            {
                new VsReferenceItem(name, properties)
            };

            return this;
        }

        public ProjectRestoreInfoBuilder WithTargetFrameworkInfo(
            IVsTargetFrameworkInfo tfi)
        {
            if (_projectRestoreInfo.TargetFrameworks is VsTargetFrameworks vsTargetFrameworks && tfi is VsTargetFrameworkInfo)
            {
                vsTargetFrameworks.Add(tfi);
            }

            if (_projectRestoreInfo2.TargetFrameworks is VsTargetFrameworks2 vsTargetFrameworks2 && tfi is VsTargetFrameworkInfo2)
            {
                vsTargetFrameworks2.Add((IVsTargetFrameworkInfo2)tfi);
            }

            return this;
        }

        public VsProjectRestoreInfo ProjectRestoreInfo => _projectRestoreInfo;

        public VsProjectRestoreInfo2 ProjectRestoreInfo2 => _projectRestoreInfo2;

        private static VsTargetFrameworkInfo ToTargetFrameworkInfo(
            TargetFrameworkInformation tfm,
            IEnumerable<IVsProjectProperty> globalProperties)
        {
            var packageReferences = tfm
                .Dependencies
                .Where(d => d.LibraryRange.TypeConstraint == LibraryDependencyTarget.Package)
                .Select(ToPackageReference);

            var projectReferences = tfm
                .Dependencies
                .Where(d => d.LibraryRange.TypeConstraint == LibraryDependencyTarget.ExternalProject)
                .Select(ToProjectReference);

            var projectProperties = new VsProjectProperties
            {
                {
                    "PackageTargetFallback",
                    string.Join(";", tfm.Imports.Select(x => x.GetShortFolderName()))
                },
            };

            return new VsTargetFrameworkInfo(
                tfm.FrameworkName.ToString(),
                packageReferences,
                projectReferences,
                projectProperties.Concat(globalProperties),
                originalTargetFramework: tfm.TargetAlias);
        }

        private static VsTargetFrameworkInfo2 ToTargetFrameworkInfo2(
            TargetFrameworkInformation tfm,
            IEnumerable<IVsProjectProperty> globalProperties)
        {
            var packageReferences = tfm
                .Dependencies
                .Where(d => d.LibraryRange.TypeConstraint == LibraryDependencyTarget.Package)
                .Select(ToPackageReference);

            var projectReferences = tfm
                .Dependencies
                .Where(d => d.LibraryRange.TypeConstraint == LibraryDependencyTarget.ExternalProject)
                .Select(ToProjectReference);

            var packageDownloads = tfm
                .DownloadDependencies
                .GroupBy(e => e.Name)
                .Select(ToPackageDownload);

            var frameworkReferences = tfm.FrameworkReferences.Select(ToFrameworkReference);

            var projectProperties = new VsProjectProperties
            {
                {
                    "PackageTargetFallback",
                    string.Join(";", tfm.Imports.Select(x => x.GetShortFolderName()))
                }
            };

            return new VsTargetFrameworkInfo2(
                tfm.FrameworkName.ToString(),
                packageReferences,
                projectReferences,
                packageDownloads,
                frameworkReferences,
                projectProperties.Concat(globalProperties),
                originalTargetFramework: tfm.TargetAlias);
        }

        public static IEnumerable<IVsProjectProperty> GetTargetFrameworkProperties(NuGetFramework framework, string originalString = null, string clrSupport = null)
        {
            string platformVersion = framework.PlatformVersion.ToString();
            string platformMoniker = GetTargetPlatformMoniker(framework);
            string windowsTargetPlatformMinVersion = string.Empty;
            if (!string.IsNullOrEmpty(clrSupport))
            {
                windowsTargetPlatformMinVersion = framework.PlatformVersion.ToString();
                var lowerPlatformVersionFramework = new NuGetFramework(
                    framework.Framework,
                    framework.Version,
                    framework.Platform,
                    new Version(framework.PlatformVersion.Major - 1, 0, 0));
                platformMoniker = lowerPlatformVersionFramework.DotNetPlatformName;
                platformVersion = lowerPlatformVersionFramework.PlatformVersion.ToString();
            }

            return new IVsProjectProperty[]
            {
                new VsProjectProperty(ProjectBuildProperties.TargetFrameworkMoniker, GetTargetFrameworkMoniker(framework)),
                new VsProjectProperty(ProjectBuildProperties.TargetPlatformMoniker, platformMoniker),
                new VsProjectProperty(ProjectBuildProperties.TargetFrameworkIdentifier, framework.Framework),
                new VsProjectProperty(ProjectBuildProperties.TargetFrameworkVersion, "v" + framework.Version),
                new VsProjectProperty(ProjectBuildProperties.TargetFrameworkProfile, framework.Profile),
                new VsProjectProperty(ProjectBuildProperties.TargetPlatformIdentifier, framework.Platform),
                new VsProjectProperty(ProjectBuildProperties.TargetPlatformVersion, platformVersion),
                new VsProjectProperty(ProjectBuildProperties.TargetFramework, originalString ?? framework.GetShortFolderName()),
                new VsProjectProperty(ProjectBuildProperties.CLRSupport, clrSupport ?? string.Empty),
                new VsProjectProperty(ProjectBuildProperties.WindowsTargetPlatformMinVersion, windowsTargetPlatformMinVersion)
            };
        }

        private static string GetTargetPlatformMoniker(NuGetFramework framework)
        {
            if (framework.HasPlatform)
            {
                return framework.DotNetPlatformName;
            }
            return null;
        }

        private static string GetTargetFrameworkMoniker(NuGetFramework framework)
        {
            var parts = new List<string>(3) { framework.Framework };

            parts.Add(string.Format(CultureInfo.InvariantCulture, "Version=v{0}", GetDisplayVersion(framework.Version)));

            if (!string.IsNullOrEmpty(framework.Profile))
            {
                parts.Add(string.Format(CultureInfo.InvariantCulture, "Profile={0}", framework.Profile));
            }

            return string.Join(",", parts);
        }

        private static string GetDisplayVersion(Version version)
        {
            var sb = new StringBuilder(string.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor));

            if (version.Build > 0
                || version.Revision > 0)
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, ".{0}", version.Build);

                if (version.Revision > 0)
                {
                    sb.AppendFormat(CultureInfo.InvariantCulture, ".{0}", version.Revision);
                }
            }

            return sb.ToString();
        }

        public static IEnumerable<IVsProjectProperty> GetTargetFrameworkProperties(string targetFrameworkMoniker, string originalFrameworkName = null)
        {
            var framework = NuGetFramework.Parse(targetFrameworkMoniker);
            var originalTFM = !string.IsNullOrEmpty(originalFrameworkName) ?
                            originalFrameworkName :
                            GetTargetFramework(framework, targetFrameworkMoniker);

            return GetTargetFrameworkProperties(framework, originalTFM);
        }

        private static string GetTargetFramework(NuGetFramework framework, string targetFrameworkMoniker)
        {
            try
            {
                return framework.GetShortFolderName();
            }
            catch
            {
                return targetFrameworkMoniker;
            }
        }

        private static IVsReferenceItem ToFrameworkReference(FrameworkDependency frameworkDependency)
        {
            var properties = new VsReferenceProperties(
                new[] { new VsReferenceProperty("PrivateAssets", FrameworkDependencyFlagsUtils.GetFlagString(frameworkDependency.PrivateAssets)) }
            );
            return new VsReferenceItem(frameworkDependency.Name, properties);
        }

        private static IVsReferenceItem ToPackageReference(LibraryDependency library)
        {
            var properties = new VsReferenceProperties(
                new[] { new VsReferenceProperty("Version", library.LibraryRange.VersionRange.OriginalString) }
            );
            return new VsReferenceItem(library.Name, properties);
        }

        private static IVsReferenceItem ToProjectReference(LibraryDependency library)
        {
            var properties = new VsReferenceProperties(
                new[] { new VsReferenceProperty("ProjectFileFullPath", library.LibraryRange.Name) }
            );
            return new VsReferenceItem(library.Name, properties);
        }

        private static IVsReferenceItem ToToolReference(LibraryRange libraryRange)
        {
            var properties = new VsReferenceProperties(
                new[] { new VsReferenceProperty("Version", libraryRange.VersionRange.OriginalString) }
            );
            return new VsReferenceItem(libraryRange.Name, properties);
        }

        private static IVsReferenceItem ToPackageDownload(IGrouping<string, DownloadDependency> library)
        {
            string versionProperty = string.Join(";", library.Select(e => e.VersionRange.OriginalString));

            var properties = new VsReferenceProperties(
                    new[] { new VsReferenceProperty("Version", versionProperty) }
                );
            return new VsReferenceItem(library.Key, properties);
        }
    }
}
