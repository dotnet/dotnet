// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System;
using Microsoft.Build.Framework;
using Microsoft.DotNet.Build.Tasks.Workloads.Wix;
using Microsoft.NET.Sdk.WorkloadManifestReader;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Msi
{
    internal class WorkloadPackMsi : MsiBase
    {
        private WorkloadPackPackage _package;

        protected override string BaseOutputName => _package.ShortName;

        protected override string? MsiPackageType => DefaultValues.WorkloadPackMsi;

        public WorkloadPackMsi(WorkloadPackPackage package, string platform, IBuildEngine buildEngine,
            WixToolsetConfiguration wixToolsetConfig,
            string baseIntermediatOutputPath,
            bool createWixPack = true) :
            base(package, buildEngine, wixToolsetConfig, platform, baseIntermediatOutputPath, createWixPack)
        {
            _package = package;

            // Workload packs are not upgradable so the upgrade code is generated using the package identity as that
            // includes the package version.
            UpgradeCode = Utils.CreateUuid(UpgradeCodeNamespaceUuid, $"{_package.Identity};{Platform}");
            ProviderKeyName = $"{_package.Id},{_package.PackageVersion},{Platform}";
            InstallationRecordKey = $@"{InstallRecordBaseKey}\InstalledPacks\{Platform}\{package.Id}\{package.PackageVersion}";

            ReplacementTokens[MsiTokens.__PROVIDER_KEY_NAME__] = ProviderKeyName;
            ReplacementTokens[MsiTokens.__UPGRADECODE__] = UpgradeCode.ToString("B");
        }

        public override string Create()
        {
            WixDocument productDoc = CreateProduct();

            // Add the default installation directory based on the workload pack kind.
            string directoryReference = MsiDirectories.InstallDir;
            var directory = productDoc.GetDirectory(MsiDirectories.DOTNETHOME)
                .AddDirectory(MsiDirectories.InstallDir, GetInstallDir(_package.Kind));

            if (_package.Kind != WorkloadPackKind.Library && _package.Kind != WorkloadPackKind.Template)
            {
                directory.AddDirectory(MsiDirectories.PackageDir, Metadata.Id)
                    .AddDirectory(MsiDirectories.VersionDir, Metadata.PackageVersion.ToString());
                // Override the directory reference for harvesting.
                directoryReference = MsiDirectories.VersionDir;
            }

            productDoc.AddRegistryKey("C_InstallationRecord", CreateInstallationRecord());

            // Harvest the template.
            productDoc.GetFeature("F_PackageContents")
                .AddComponentGroupRef(HarvestDirectory(_package.DestinationDirectory, directoryReference));
            productDoc.Save();

            return "";
        }

        /// <summary>
        /// Get the installation directory based on the kind of workload pack.
        /// </summary>
        /// <param name="kind">The workload pack kind.</param>
        /// <returns>The name of the root installation directory.</returns>
        internal static string GetInstallDir(WorkloadPackKind kind) =>
            kind switch
            {
                WorkloadPackKind.Framework or WorkloadPackKind.Sdk => "packs",
                WorkloadPackKind.Library => "library-packs",
                WorkloadPackKind.Template => "template-packs",
                WorkloadPackKind.Tool => "tool-packs",
                _ => throw new ArgumentException(string.Format(Strings.UnknownWorkloadKind, kind)),
            };

        /// <summary>
        /// Gets the directory reference ID associated with the workload pack kind.
        /// </summary>
        /// <param name="kind">The workload pack kind.</param>
        /// <returns>The directory reference (ID) of the installation directory.</returns>
        internal static string GetDirectoryReference(WorkloadPackKind kind) =>
            kind switch
            {
                WorkloadPackKind.Framework or WorkloadPackKind.Sdk => "PacksDir",
                WorkloadPackKind.Library => "LibraryPacksDir",
                WorkloadPackKind.Template => "TemplatePacksDir",
                WorkloadPackKind.Tool => "ToolPacksDir",
                _ => throw new ArgumentException(string.Format(Strings.UnknownWorkloadKind, kind)),
            };
    }
}

#nullable disable
