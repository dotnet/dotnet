// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Versioning;

namespace NuGet.Build.Tasks.Pack
{
    public class GetPackOutputItemsTask : Task
    {
        [Required]
        public string PackageId { get; set; }

        [Required]
        public string PackageVersion { get; set; }

        [Required]
        public string PackageOutputPath { get; set; }

        [Required]
        public string NuspecOutputPath { get; set; }

        public string NuspecFile { get; set; }

        public string[] NuspecProperties { get; set; }

        public bool IncludeSymbols { get; set; }

        public bool IncludeSource { get; set; }

        public string SymbolPackageFormat { get; set; }

        public bool OutputFileNamesWithoutVersion { get; set; }

        /// <summary>
        /// Output items
        /// </summary>
        [Output]
        public ITaskItem[] OutputPackItems { get; set; }

        public override bool Execute()
        {
            (string packageId, NuGetVersion version) = GetPackageIdAndVersion();

            var symbolPackageFormat = PackArgs.GetSymbolPackageFormat(MSBuildStringUtility.TrimAndGetNullForEmpty(SymbolPackageFormat));
            var nupkgFileName = PackCommandRunner.GetOutputFileName(packageId, version, isNupkg: true, symbols: false, symbolPackageFormat: symbolPackageFormat, excludeVersion: OutputFileNamesWithoutVersion);
            var nuspecFileName = PackCommandRunner.GetOutputFileName(packageId, version, isNupkg: false, symbols: false, symbolPackageFormat: symbolPackageFormat, excludeVersion: OutputFileNamesWithoutVersion);

            var outputs = new List<ITaskItem>();
            outputs.Add(new TaskItem(Path.Combine(PackageOutputPath, nupkgFileName)));
            outputs.Add(new TaskItem(Path.Combine(NuspecOutputPath, nuspecFileName)));

            if (IncludeSource || IncludeSymbols)
            {
                var nupkgSymbolsFileName = PackCommandRunner.GetOutputFileName(packageId, version, isNupkg: true, symbols: true, symbolPackageFormat: symbolPackageFormat, excludeVersion: OutputFileNamesWithoutVersion);
                var nuspecSymbolsFileName = PackCommandRunner.GetOutputFileName(packageId, version, isNupkg: false, symbols: true, symbolPackageFormat: symbolPackageFormat, excludeVersion: OutputFileNamesWithoutVersion);
                outputs.Add(new TaskItem(Path.Combine(PackageOutputPath, nupkgSymbolsFileName)));
                outputs.Add(new TaskItem(Path.Combine(NuspecOutputPath, nuspecSymbolsFileName)));
            }

            OutputPackItems = outputs.ToArray();
            return true;
        }

        private (string packageId, NuGetVersion version) GetPackageIdAndVersion()
        {
            string packageId = PackageId;
            var packageVersion = PackageVersion;
            NuGetVersion version = null;

            // Extract the version from the nuspec file if it exists and is valid, otherwise use the version from the project.
            if (!string.IsNullOrWhiteSpace(NuspecFile))
            {
                bool hasVersionInNuspecProperties = false;
                if (NuspecProperties != null && NuspecProperties.Length > 0)
                {
                    PackArgs packArgs = new PackArgs() { Version = packageVersion };
                    PackTaskLogic.SetPackArgsPropertiesFromNuspecProperties(packArgs, MSBuildStringUtility.TrimAndExcludeNullOrEmpty(NuspecProperties));
                    // If the logic depends only on checking for a non-null value, it may incorrectly  detect cases where the parsing logic changes the version based on a key other than the "version" key.
                    // Currently, supported only version property in NuspecProperties.
                    if (packArgs.Properties.ContainsKey("version"))
                    {
                        packageVersion = packArgs.Version;
                        hasVersionInNuspecProperties = true;
                    }
                }

                var nuspecReader = new NuspecReader(NuspecFile);
                packageId = nuspecReader.GetId();
                if (!hasVersionInNuspecProperties)
                {
                    version = nuspecReader.GetVersion();
                }
            }

            if (version == null && !NuGetVersion.TryParse(packageVersion, out version))
            {
                throw new ArgumentException(string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.InvalidPackageVersion,
                    packageVersion));
            }

            return (packageId, version);
        }
    }
}
