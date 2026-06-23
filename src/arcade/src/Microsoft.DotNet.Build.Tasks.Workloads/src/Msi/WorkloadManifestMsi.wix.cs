// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Build.Framework;
using Microsoft.DotNet.Build.Tasks.Workloads.Wix;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Msi
{
    /// <summary>
    /// Represents a workload manifest MSI.
    /// </summary>
    internal class WorkloadManifestMsi : MsiBase
    {
        public WorkloadManifestPackage Package { get; }

        public List<WorkloadPackGroupJson> WorkloadPackGroups { get; } = new();

        /// <inheritdoc />
        protected override string BaseOutputName => Path.GetFileNameWithoutExtension(Package.PackagePath);

        /// <summary>
        /// <see langword="true">True</see> if the manifest installer supports side-by-side installs, otherwise the installer
        /// supports major upgrades.
        /// </summary>
        protected bool IsSxS
        {
            get;
        }

        protected override string? MsiPackageType => DefaultValues.ManifestMsi;

        public WorkloadManifestMsi(WorkloadManifestPackage package, string platform, IBuildEngine buildEngine,
            WixToolsetConfiguration wixToolsetConfig,
            string baseIntermediateOutputPath, bool isSxS = false, bool createWixPack = true) :
            base(package, buildEngine, wixToolsetConfig, platform, baseIntermediateOutputPath, createWixPack)
        {
            Package = package;
            IsSxS = isSxS;

            ProviderKeyName = IsSxS ? $"{Package.ManifestId},{Package.SdkFeatureBand},{Package.PackageVersion},{Platform}" :
               $"{Package.ManifestId},{Package.SdkFeatureBand},{Platform}";

            InstallationRecordKey = $@"{InstallRecordBaseKey}\InstalledManifests\{Platform}\{package.Id}\{package.PackageVersion}";

            // To support upgrades, the UpgradeCode must be stable within an SDK feature band.
            // For example, 6.0.101 and 6.0.108 must generate the same GUID for the same platform and
            // manifest ID. The workload author must ensure the ProductVersion is higher than previously
            // shipped versions. For SxS installs the UpgradeCode must be a random GUID.
            UpgradeCode = IsSxS ? Guid.NewGuid() :
                    Utils.CreateUuid(UpgradeCodeNamespaceUuid, $"{Package.ManifestId};{Package.SdkFeatureBand};{Platform}");

            ReplacementTokens[MsiTokens.__PROVIDER_KEY_NAME__] = ProviderKeyName;
            ReplacementTokens[MsiTokens.__UPGRADECODE__] = UpgradeCode.ToString("B");
        }

        public override string Create()
        {
            WixDocument productDoc = CreateProduct();

            // Add the manifest directories. The temporary installer in the SDK (6.0) used lower invariants
            // of the manifest ID. We have to do the same to ensure stable GUIDs are generated for components.
            // For example, we'll end up with authoring similar to
            // <Directory Id="DOTNETHOME" Name="dotnet">
            //   <Directory Id="SdkManifestDir" Name="sdk-manifests">
            //     <Directory Id="SdkFeatureBandVersionDir" Name="6.0.200">
            //       <Directory Id="ManifestIdDir" Name="microsoft.net.workload.mono.toolchain" />
            //     </Directory>
            //   </Directory>
            // </Directory>
            var directory = productDoc.GetDirectory(MsiDirectories.DOTNETHOME)
                .AddDirectory(MsiDirectories.SdkManifestDir, "sdk-manifests")
                .AddDirectory(MsiDirectories.SdkFeatureBandVersionDir, $"{Package.SdkFeatureBand}")
                .AddDirectory(MsiDirectories.ManifestIdDir, $"{Package.ManifestId.ToLowerInvariant()}");

            // For SxS installs, different manifests for the same feature band need to be installed
            // in versioned directories.
            if (IsSxS)
            {
                directory.AddDirectory(MsiDirectories.ManifestVersionDir, Package.GetManifest().Version);
            }

            productDoc.AddRegistryKey("C_InstallationRecord", CreateInstallationRecord());

            // Harvest the package content and add the generated component group reference to an
            // existing feature.
            string packageDataDirectory = Path.Combine(Package.DestinationDirectory, "data");
            string filesDirectoryId = IsSxS ?
                MsiDirectories.ManifestVersionDir :
                MsiDirectories.ManifestIdDir;
            productDoc.GetFeature("F_PackageContents")
                .AddComponentGroupRef(HarvestDirectory(packageDataDirectory, filesDirectoryId));

            foreach (var file in Directory.GetFiles(packageDataDirectory))
            {
                NuGetPackageFiles[file] = @"\data\extractedManifest\" + Path.GetFileName(file);
            }

            if (WorkloadPackGroups.Count > 0)
            {
                string jsonAsString = JsonSerializer.Serialize(WorkloadPackGroups, typeof(IList<WorkloadPackGroupJson>), new JsonSerializerOptions() { WriteIndented = true });
                string jsonDirectory = Path.Combine(SourcePath, "json");
                Directory.CreateDirectory(jsonDirectory);

                string jsonFullPath = Path.GetFullPath(Path.Combine(jsonDirectory, "WorkloadPackGroups.json"));
                File.WriteAllText(jsonFullPath, jsonAsString);

                productDoc.GetFeature("F_PackageContents")
                    .AddComponentGroupRef(HarvestDirectory(jsonDirectory, filesDirectoryId, "JsonSourceDir"));

                NuGetPackageFiles[jsonFullPath] = @"\data\extractedManifest\" + Path.GetFileName(jsonFullPath);
            }

            productDoc.Save();

            return "";
        }

        public class WorkloadPackGroupJson
        {
            public string? GroupPackageId { get; set; }
            public string? GroupPackageVersion { get; set; }

            public List<WorkloadPackJson> Packs { get; set; } = new List<WorkloadPackJson>();
        }

        public class WorkloadPackJson
        {
            public string? PackId { get; set; }

            public string? PackVersion { get; set; }
        }
    }
}

#nullable disable
