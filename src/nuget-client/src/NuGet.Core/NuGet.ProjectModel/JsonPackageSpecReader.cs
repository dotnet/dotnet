// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.LibraryModel;

namespace NuGet.ProjectModel
{
    public static partial class JsonPackageSpecReader
    {
        private const char VersionSeparator = ';';
        public static readonly string RestoreOptions = "restore";
        public static readonly string RestoreSettings = "restoreSettings";
        public static readonly string HideWarningsAndErrors = "hideWarningsAndErrors";
        public static readonly string PackOptions = "packOptions";
        public static readonly string PackageType = "packageType";
        public static readonly string Files = "files";

        /// <summary>
        /// Load and parse a project.json file
        /// </summary>
        /// <param name="name">project name</param>
        /// <param name="packageSpecPath">file path</param>
        public static PackageSpec GetPackageSpec(string name, string packageSpecPath)
        {
            return FileUtility.SafeRead(filePath: packageSpecPath, read: (stream, filePath) => GetPackageSpec(stream, name, filePath, null));
        }

        public static PackageSpec GetPackageSpec(string json, string name, string packageSpecPath)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return GetPackageSpec(ms, name, packageSpecPath, null);
            }
        }

        public static PackageSpec GetPackageSpec(Stream stream, string name, string packageSpecPath, string snapshotValue)
        {
            return GetPackageSpec(stream, name, packageSpecPath, snapshotValue, EnvironmentVariableWrapper.Instance);
        }

        internal static PackageSpec GetPackageSpec(Stream stream, string name, string packageSpecPath, string snapshotValue, IEnvironmentVariableReader environmentVariableReader)
        {
            return GetPackageSpecUtf8JsonStreamReader(stream, name, packageSpecPath, environmentVariableReader, snapshotValue);
        }

        private static string ExtractMacro(string value, string userSettingsDirectory, bool useMacros)
        {
            if (useMacros)
            {
                return MacroStringsUtility.ExtractMacro(value, userSettingsDirectory, MacroStringsUtility.UserMacro);
            }
            return value;
        }

        private static void ExtractMacros(List<string> paths, string userSettingsDirectory, bool useMacros)
        {
            if (useMacros)
            {
                MacroStringsUtility.ExtractMacros(paths, userSettingsDirectory, MacroStringsUtility.UserMacro);
            }
        }

        private static void AddTargetFramework(PackageSpec packageSpec, NuGetFramework frameworkName, NuGetFramework secondaryFramework, TargetFrameworkInformation targetFrameworkInformation)
        {
            NuGetFramework updatedFramework = frameworkName;

            if (targetFrameworkInformation.Imports.Length > 0)
            {
                NuGetFramework[] imports = targetFrameworkInformation.Imports.ToArray();

                if (targetFrameworkInformation.AssetTargetFallback)
                {
                    updatedFramework = new AssetTargetFallbackFramework(GetDualCompatibilityFrameworkIfNeeded(frameworkName, secondaryFramework), imports);
                }
                else
                {
                    updatedFramework = new FallbackFramework(GetDualCompatibilityFrameworkIfNeeded(frameworkName, secondaryFramework), imports);
                }
            }
            else
            {
                updatedFramework = GetDualCompatibilityFrameworkIfNeeded(frameworkName, secondaryFramework);
            }

            targetFrameworkInformation = new TargetFrameworkInformation(targetFrameworkInformation) { FrameworkName = updatedFramework };

            packageSpec.TargetFrameworks.Add(targetFrameworkInformation);
        }
        private static NuGetFramework GetDualCompatibilityFrameworkIfNeeded(NuGetFramework frameworkName, NuGetFramework secondaryFramework)
        {
            if (secondaryFramework != default)
            {
                return new DualCompatibilityFramework(frameworkName, secondaryFramework);
            }

            return frameworkName;
        }

        private static bool ValidateDependencyTarget(LibraryDependencyTarget targetValue)
        {
            var isValid = false;

            switch (targetValue)
            {
                case LibraryDependencyTarget.Package:
                case LibraryDependencyTarget.Project:
                case LibraryDependencyTarget.ExternalProject:
                    isValid = true;
                    break;
            }

            return isValid;
        }
    }
}
