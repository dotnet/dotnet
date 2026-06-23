// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks.Workloads.Wix;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Msi
{
    internal class WorkloadSetMsi : MsiBase
    {
        private WorkloadSetPackage _package;

        protected override string BaseOutputName => Path.GetFileNameWithoutExtension(_package.PackagePath);

        protected override string? MsiPackageType => DefaultValues.WorkloadSetMsi;

        public WorkloadSetMsi(WorkloadSetPackage package, string platform, IBuildEngine buildEngine,
            WixToolsetConfiguration wixToolsetConfig,
            string baseIntermediatOutputPath,
            bool createWixPack = true) :
            base(package, buildEngine, wixToolsetConfig, platform, baseIntermediatOutputPath, createWixPack)
        {
            _package = package;
            InstallationRecordKey = $@"{InstallRecordBaseKey}\InstalledWorkloadSets\{Platform}\{_package.SdkFeatureBand}\{_package.PackageVersion}";
            UpgradeCode = Utils.CreateUuid(UpgradeCodeNamespaceUuid, $"{_package.Identity};{Platform}");
            ProviderKeyName = $"Microsoft.NET.Workload.Set,{_package.SdkFeatureBand},{_package.PackageVersion},{Platform}";
            ReplacementTokens[MsiTokens.__PROVIDER_KEY_NAME__] = ProviderKeyName;
            ReplacementTokens[MsiTokens.__UPGRADECODE__] = UpgradeCode.ToString("B");
        }

        public override string Create()
        {
            WixDocument productDoc = CreateProduct();

            productDoc.AddRegistryKey("C_InstallationRecord", CreateInstallationRecord());

            var directory = productDoc.GetDirectory(MsiDirectories.DOTNETHOME)
                .AddDirectory(MsiDirectories.SdkManifestDir, "sdk-manifests")
                .AddDirectory(MsiDirectories.SdkFeatureBandVersionDir, $"{_package.SdkFeatureBand}")
                .AddDirectory(MsiDirectories.WorkloadSetsDir, $"workloadsets")
                .AddDirectory(MsiDirectories.WorkloadSetVersionDir, $"{_package.WorkloadSetVersion}");

            string packageDataDirectory = Path.Combine(_package.DestinationDirectory, "data");
            productDoc.GetFeature("F_PackageContents")
                .AddComponentGroupRef(HarvestDirectory(packageDataDirectory, MsiDirectories.WorkloadSetVersionDir));
            productDoc.Save();

            return "";
        }
    }
}

#nullable disable
