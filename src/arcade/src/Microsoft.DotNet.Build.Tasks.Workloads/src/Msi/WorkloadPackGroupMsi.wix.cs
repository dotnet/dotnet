// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System.Xml.Linq;
using Microsoft.Build.Framework;
using Microsoft.DotNet.Build.Tasks.Workloads.Wix;
using Microsoft.NET.Sdk.WorkloadManifestReader;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Msi
{
    internal class WorkloadPackGroupMsi : MsiBase
    {
        WorkloadPackGroupPackage _package;

        /// <inheritdoc />
        protected override string BaseOutputName => Metadata.Id;

        protected override string? MsiPackageType => DefaultValues.WorkloadPackGroupMsi;

        public WorkloadPackGroupMsi(WorkloadPackGroupPackage package, string platform, IBuildEngine buildEngine,
            WixToolsetConfiguration wixToolsetConfig,
            string baseIntermediatOutputPath,
            bool createWixPack = true)
             : base(package, buildEngine, wixToolsetConfig, platform, baseIntermediatOutputPath, createWixPack)
        {
            _package = package;

            UpgradeCode = Utils.CreateUuid(UpgradeCodeNamespaceUuid, $"{Metadata.Id};{Platform}");
            ProviderKeyName = $"{_package.Id},{Metadata.PackageVersion},{Platform}";
            InstallationRecordKey = $@"{InstallRecordBaseKey}\InstalledPackGroups\{Platform}\{package.Id}\{Metadata.PackageVersion}";

            ReplacementTokens[MsiTokens.__PROVIDER_KEY_NAME__] = ProviderKeyName;
            ReplacementTokens[MsiTokens.__UPGRADECODE__] = UpgradeCode.ToString("B");
        }

        public override string Create()
        {
            WixDocument productDoc = CreateProduct();

            XElement installRecordKey = base.CreateInstallationRecord();

            int packCount = 0;

            foreach (var pack in _package.Packs)
            {
                // Calculate the installation directory name and ID for the pack.
                string packDirName = WorkloadPackMsi.GetInstallDir(pack.Kind);
                string packDirReference = WorkloadPackMsi.GetDirectoryReference(pack.Kind);

                // Get the directory element associated with the pack or if it doesn't exist, add the directory and return it.
                var packDirectory = productDoc.GetDirectory(packDirReference) ?? productDoc.GetDirectory("DOTNETHOME")
                        .AddDirectory(packDirReference, packDirName);

                if (pack.Kind != WorkloadPackKind.Library && pack.Kind != WorkloadPackKind.Template)
                {
                    // Create directories for the package ID and version (which should be under the "packs" directory).
                    // <Directory Name="packs" ... >
                    //   <Directory Name="x.y.z." Id="dir21902ab298...">
                    //     <Directory Name="2.0.3" Id="dir2020af0..." />
                    // Generate a new reference for the version directory that can be passed to Heat.
                    packDirReference = WixDocument.GetDirectoryReference();
                    packDirectory.AddDirectory(WixDocument.CreateDirectory(pack.Id))
                        .AddDirectory(WixDocument.CreateDirectory(pack.PackageVersion.ToString(), packDirReference));
                }

                productDoc.GetFeature("F_PackageContents")
                    .AddComponentGroupRef(HarvestDirectory(pack.DestinationDirectory, packDirReference, $"SourceDir{packCount:D4}"));

                // Add an install record for each pack in the pack group. Setting the root to null allows
                // nesting RegistryKey elements.
                installRecordKey.AddRegistryKey(pack.Id, null)
                    .AddRegistryKey(pack.PackageVersion.ToString(), null)
                    .AddRegistryValue(null, "");

                packCount++;
            }

            productDoc.AddRegistryKey("C_InstallationRecord", installRecordKey);
            productDoc.Save();

            return "";
        }
    }
}
