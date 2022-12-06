// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Common;
using NuGet.Frameworks;

namespace NuGet.Commands
{
    public static class MSBuildProjectFrameworkUtility
    {
        /// <summary>
        /// Determine the target framework of an msbuild project.
        /// Returns the <see cref="FrameworkName"/> equivalent representation.
        /// </summary>
        /// <remarks>
        /// Do not use this when expecting a project to target a .NET 5 era framework.
        /// </remarks>
        public static IEnumerable<string> GetProjectFrameworkStrings(
            string projectFilePath,
            string targetFrameworks,
            string targetFramework,
            string targetFrameworkMoniker,
            string targetPlatformIdentifier,
            string targetPlatformVersion,
            string targetPlatformMinVersion)
        {
            return GetProjectFrameworkStrings(
                projectFilePath,
                targetFrameworks,
                targetFramework,
                targetFrameworkMoniker,
                targetPlatformIdentifier,
                targetPlatformVersion,
                targetPlatformMinVersion,
                isManagementPackProject: false,
                isXnaWindowsPhoneProject: false);
        }

        /// <summary>
        /// Given the properties from an msbuild project file and a the file path, infer the target framework.
        /// This method prioritizes projects without a framework, such as vcxproj and accounts for the mismatching arguments in UAP projects, where the TFI and TFV are set but should be ignored.
        /// Likewise, this method will *ignore* unnecessary properties, such as TPI and TPV when profiles are used, and frameworks that do not support platforms have some default values.
        /// </summary>
        /// <returns>The inferred framework. Unsupported otherwise.</returns>
        public static NuGetFramework GetProjectFramework(
            string projectFilePath,
            string targetFrameworkMoniker,
            string targetPlatformMoniker,
            string targetPlatformMinVersion)
        {
            return GetProjectFramework(
                projectFilePath,
                targetFrameworkMoniker,
                targetPlatformMoniker,
                targetPlatformIdentifier: null,
                targetPlatformVersion: null,
                targetPlatformMinVersion,
                clrSupport: null,
                isXnaWindowsPhoneProject: false,
                isManagementPackProject: false);
        }

        public static NuGetFramework GetProjectFramework(
            string projectFilePath,
            string targetFrameworkMoniker,
            string targetPlatformMoniker,
            string targetPlatformMinVersion,
            string clrSupport)
        {
            return GetProjectFramework(
                projectFilePath,
                targetFrameworkMoniker,
                targetPlatformMoniker,
                targetPlatformIdentifier: null,
                targetPlatformVersion: null,
                targetPlatformMinVersion,
                clrSupport,
                isXnaWindowsPhoneProject: false,
                isManagementPackProject: false);
        }

        /// <summary>
        /// Determine the target framework of an msbuild project.
        /// Returns the <see cref="FrameworkName"/> equivalent representation.
        /// </summary>
        /// <remarks>
        /// Do not use this when expecting a project to target a .NET 5 era framework.
        /// </remarks>
        public static IEnumerable<string> GetProjectFrameworkStrings(
            string projectFilePath,
            string targetFrameworks,
            string targetFramework,
            string targetFrameworkMoniker,
            string targetPlatformIdentifier,
            string targetPlatformVersion,
            string targetPlatformMinVersion,
            bool isXnaWindowsPhoneProject,
            bool isManagementPackProject)
        {
            return GetProjectFrameworks(
                projectFilePath,
                targetFrameworks,
                targetFramework,
                targetFrameworkMoniker,
                targetPlatformMoniker: null,
                targetPlatformIdentifier,
                targetPlatformVersion,
                targetPlatformMinVersion,
                clrSupport: null,
                isXnaWindowsPhoneProject,
                isManagementPackProject);
        }

        internal static IEnumerable<string> GetProjectFrameworks(
            string projectFilePath,
            string targetFrameworks,
            string targetFramework,
            string targetFrameworkMoniker,
            string targetPlatformMoniker,
            string targetPlatformIdentifier,
            string targetPlatformVersion,
            string targetPlatformMinVersion,
            string clrSupport,
            bool isXnaWindowsPhoneProject,
            bool isManagementPackProject)
        {
            var frameworks = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);

            // TargetFrameworks property
            frameworks.UnionWith(MSBuildStringUtility.Split(targetFrameworks));

            if (frameworks.Count > 0)
            {
                return frameworks;
            }

            // TargetFramework property
            var currentFrameworkString = MSBuildStringUtility.TrimAndGetNullForEmpty(targetFramework);

            if (!string.IsNullOrEmpty(currentFrameworkString))
            {
                frameworks.Add(currentFrameworkString);

                return frameworks;
            }

            return new string[] { GetProjectFramework(
                projectFilePath,
                targetFrameworkMoniker,
                targetPlatformMoniker,
                targetPlatformIdentifier,
                targetPlatformVersion,
                targetPlatformMinVersion,
                clrSupport,
                isXnaWindowsPhoneProject,
                isManagementPackProject).DotNetFrameworkName };
        }

        internal static NuGetFramework GetProjectFramework(
            string projectFilePath,
            string targetFrameworkMoniker,
            string targetPlatformMoniker,
            string targetPlatformIdentifier,
            string targetPlatformVersion,
            string targetPlatformMinVersion,
            string clrSupport,
            bool isXnaWindowsPhoneProject,
            bool isManagementPackProject)
        {
            bool isCppCliSet = clrSupport?.Equals("NetCore", StringComparison.OrdinalIgnoreCase) == true;
            bool isCppCli = false;
            // C++ check
            if (projectFilePath?.EndsWith(".vcxproj", StringComparison.OrdinalIgnoreCase) == true)
            {
                if (!isCppCliSet)
                {
                    // The C++ project does not have a TargetFrameworkMoniker property set. 
                    // We hard-code the return value to Native.
                    return FrameworkConstants.CommonFrameworks.Native;
                }
                isCppCli = true;
            }

            // The MP project does not have a TargetFrameworkMoniker property set. 
            // We hard-code the return value to SCMPInfra.
            if (isManagementPackProject)
            {
                return NuGetFramework.Parse("SCMPInfra, Version=0.0");
            }

            // UAP/Windows store projects
            var platformMoniker = MSBuildStringUtility.TrimAndGetNullForEmpty(targetPlatformMoniker);
            var platformMonikerIdentifier = GetParts(platformMoniker).FirstOrDefault();

            var platformIdentifier = MSBuildStringUtility.TrimAndGetNullForEmpty(targetPlatformIdentifier);
            var platformMinVersion = MSBuildStringUtility.TrimAndGetNullForEmpty(targetPlatformMinVersion);
            var platformVersion = MSBuildStringUtility.TrimAndGetNullForEmpty(targetPlatformVersion);

            var effectivePlatformVersion = platformMinVersion ?? platformVersion;
            var effectivePlatformIdentifier = platformMonikerIdentifier ?? platformIdentifier;

            // Check for JS project
            if (projectFilePath?.EndsWith(".jsproj", StringComparison.OrdinalIgnoreCase) == true)
            {
                // JavaScript apps do not have a TargetFrameworkMoniker property set.
                // We read the TargetPlatformIdentifier and targetPlatformMinVersion instead
                // use the default values for JS if they were not given

                // Prefer moniker over individual properties
                if (!string.IsNullOrEmpty(platformMoniker))
                {
                    return GetFrameworkFromMoniker(platformMonikerIdentifier, platformMoniker, platformMinVersion);
                }

                if (string.IsNullOrEmpty(effectivePlatformVersion))
                {
                    effectivePlatformVersion = "0.0";
                }

                if (string.IsNullOrEmpty(effectivePlatformIdentifier))
                {
                    effectivePlatformIdentifier = FrameworkConstants.FrameworkIdentifiers.Windows;
                }

                return NuGetFramework.Parse($"{effectivePlatformIdentifier}, Version={effectivePlatformVersion}");
            }

            if (StringComparer.OrdinalIgnoreCase.Equals(effectivePlatformIdentifier, "UAP"))
            {
                // Use the platform id and versions, this is done for UAP projects
                // Prefer moniker over individual properties
                if (!string.IsNullOrEmpty(platformMoniker))
                {
                    return GetFrameworkFromMoniker(effectivePlatformIdentifier, platformMoniker, platformMinVersion);
                }
                if (!string.IsNullOrEmpty(effectivePlatformVersion))
                {
                    return NuGetFramework.Parse($"{effectivePlatformIdentifier}, Version={effectivePlatformVersion}");
                }
            }

            // TargetFrameworkMoniker
            var currentFrameworkString = MSBuildStringUtility.TrimAndGetNullForEmpty(targetFrameworkMoniker);

            if (!string.IsNullOrEmpty(currentFrameworkString))
            {
                // XNA project lies about its true identity, reporting itself as a normal .NET 4.0 project.
                // We detect it and changes its target framework to Silverlight4-WindowsPhone71
                if (isXnaWindowsPhoneProject
                    && ".NETFramework,Version=v4.0".Equals(currentFrameworkString, StringComparison.OrdinalIgnoreCase))
                {
                    currentFrameworkString = "Silverlight,Version=v4.0,Profile=WindowsPhone71";
                    return NuGetFramework.Parse(currentFrameworkString);
                }
                if (isCppCli) // Don't use the platform moniker to CPP/CLI.
                {
                    platformMoniker = null;
                }
                NuGetFramework framework = NuGetFramework.ParseComponents(currentFrameworkString, platformMoniker);

                if (isCppCli)
                {
                    return new DualCompatibilityFramework(framework, FrameworkConstants.CommonFrameworks.Native);
                }

                return framework;
            }

            // Default to unsupported it no framework was found.
            return NuGetFramework.UnsupportedFramework;
        }

        private static NuGetFramework GetFrameworkFromMoniker(string platformIdentifier, string platformMoniker, string platformMinVersion)
        {
            if (!string.IsNullOrEmpty(platformMinVersion))
            {
                return NuGetFramework.Parse($"{platformIdentifier}, Version={platformMinVersion}");
            }
            else
            {
                return NuGetFramework.Parse(platformMoniker);
            }
        }

        private static string[] GetParts(string targetPlatformMoniker)
        {
            return targetPlatformMoniker != null ?
                targetPlatformMoniker.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray() :
                new string[] { };
        }


        /// <summary>
        /// Parse project framework strings into NuGetFrameworks.
        /// </summary>
        public static IEnumerable<NuGetFramework> GetProjectFrameworks(IEnumerable<string> frameworkStrings)
        {
            if (frameworkStrings == null)
            {
                throw new ArgumentNullException(nameof(frameworkStrings));
            }

            var frameworks = new List<NuGetFramework>();

            foreach (var frameworkString in frameworkStrings)
            {
                var parsed = NuGetFramework.Parse(frameworkString);

                // Replace if needed
                parsed = GetProjectFrameworkReplacement(parsed);

                // Add only unique frameworks
                if (!frameworks.Contains(parsed))
                {
                    frameworks.Add(parsed);
                }
            }

            return frameworks;
        }

        /// <summary>
        /// Parse existing nuget framework for .net core 4.5.1 or 4.5 and return compatible framework instance
        /// </summary>
        public static NuGetFramework GetProjectFrameworkReplacement(NuGetFramework framework)
        {
            if (framework == null)
            {
                throw new ArgumentNullException(nameof(framework));
            }

            // if the framework is .net core 4.5.1 return windows 8.1
            if (framework.Framework.Equals(FrameworkConstants.FrameworkIdentifiers.NetCore, StringComparison.Ordinal)
                && framework.Version.Equals(Version.Parse("4.5.1.0")))
            {
                return new NuGetFramework(FrameworkConstants.FrameworkIdentifiers.Windows,
                       new Version("8.1"), framework.Profile);
            }
            // if the framework is .net core 4.5 return 8.0
            if (framework.Framework.Equals(FrameworkConstants.FrameworkIdentifiers.NetCore, StringComparison.Ordinal)
                && framework.Version.Equals(Version.Parse("4.5.0.0")))
            {
                return new NuGetFramework(FrameworkConstants.FrameworkIdentifiers.Windows,
                       new Version("8.0"), framework.Profile);
            }

            return framework;
        }
    }
}
