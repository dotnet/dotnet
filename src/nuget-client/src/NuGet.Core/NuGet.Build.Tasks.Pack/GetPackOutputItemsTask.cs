// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
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
            // Extract the version from the nuspec file if it exists and is valid, otherwise use the version from the project.
            if (string.IsNullOrWhiteSpace(NuspecFile) || !File.Exists(NuspecFile))
            {
                return (PackageId, ParseVersion(PackageVersion));
            }

            // Build property bag from NuspecProperties, exactly as PackTaskLogic/PackCommandRunner does.
            PackArgs packArgs = new PackArgs();
            if (NuspecProperties != null && NuspecProperties.Length > 0)
            {
                PackTaskLogic.SetPackArgsPropertiesFromNuspecProperties(packArgs, MSBuildStringUtility.TrimAndExcludeNullOrEmpty(NuspecProperties));
            }

            // Preprocess the raw nuspec stream first — matching how Pack calls Preprocessor.Process in
            // Manifest.ReadFrom before parsing XML — then create NuspecReader from the result.
            using Stream fileStream = File.OpenRead(NuspecFile);
            string preprocessed = Preprocessor.Process(fileStream, packArgs.GetPropertyValue);
            var nuspecReader = new NuspecReader(new MemoryStream(Encoding.UTF8.GetBytes(preprocessed)));
            string packageId = nuspecReader.GetId();
            if (packArgs.Version is not null)
            {
                return (packageId, ParseVersion(packArgs.Version));
            }

            return (packageId, nuspecReader.GetVersion() ?? ParseVersion(PackageVersion));

            static NuGetVersion ParseVersion(string packageVersion)
            {
                if (!NuGetVersion.TryParse(packageVersion, out var version))
                {
                    throw new ArgumentException(string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.InvalidPackageVersion,
                        packageVersion));
                }

                return version;
            }
        }
    }
}
