// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System;
using System.IO;
using System.IO.Packaging;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks.Workloads.Wix;
using Microsoft.NET.Sdk.WorkloadManifestReader;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Msi
{
    internal class WorkloadPackMsi : MsiBase
    {
        private WorkloadPackPackage _package;

        protected override string BaseOutputName => _package.ShortName;

        public WorkloadPackMsi(WorkloadPackPackage package, string platform, IBuildEngine buildEngine, 
            WixToolsetConfiguration wixToolsetConfig,
            string baseIntermediatOutputPath) :
            base(MsiMetadata.Create(package), buildEngine, wixToolsetConfig, platform, baseIntermediatOutputPath)
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
            using WixDocument productDoc = CreateProduct();            

            // Add the default installation directory based on the workload pack kind.
            string directoryReference = "InstallDir";
            var directory = productDoc.GetDirectory("DOTNETHOME")
                .AddDirectory("InstallDir", GetInstallDir(_package.Kind));

            if (_package.Kind != WorkloadPackKind.Library && _package.Kind != WorkloadPackKind.Template)
            {
                directory.AddDirectory("PackageDir", Metadata.Id)
                    .AddDirectory("VersionDir", Metadata.PackageVersion.ToString());
                // Override the directory reference for harvesting.
                directoryReference = "VersionDir";
            }

            productDoc.AddRegistryKey("C_InstallationRecord", CreateInstallationRecord());

            // Harvest the template.
            productDoc.GetFeature("F_PackageContents")
                .Add(HarvestDirectory(_package.DestinationDirectory, directoryReference));

            return "";
        }

        public  ITaskItem Build2(string outputPath, ITaskItem[]? iceSuppressions = null)
        {
            //Create();
            //Directory.CreateDirectory(SourcePath);
            //// Harvest the package contents before adding it to the source files we need to compile.
            //string packageContentWxs = Path.Combine(SourcePath, "PackageContent.wxs");
            //string directoryReference = _package.Kind == WorkloadPackKind.Library || _package.Kind == WorkloadPackKind.Template ?
            //    "InstallDir" : "VersionDir";

            //HarvesterToolTask heat = new(BuildEngine, WixToolsetPath)
            //{
            //    DirectoryReference = directoryReference,
            //    OutputFile = packageContentWxs,
            //    Platform = this.Platform,
            //    SourceDirectory = _package.DestinationDirectory
            //};

            //if (!heat.Execute())
            //{
            //    throw new Exception(Strings.HeatFailedToHarvest);
            //}

            //CompilerToolTask candle = CreateDefaultCompiler();

            //candle.AddSourceFiles(packageContentWxs,
            //    AddFile("DependencyProvider.wxs"),
            //    AddFile("Directories.wxs"),
            //    AddFile("dotnethome_x64.wxs"),
            //    AddFile("Product.wxs"),
            //    AddFile("Registry.wxs"));

            //// Only extract the include file as it's not compilable, but imported by various source files.
            //AddFile("Variables.wxi");

            //// Workload packs are not upgradable so the upgrade code is generated using the package identity as that
            //// includes the package version.
            //Guid upgradeCode = Utils.CreateUuid(UpgradeCodeNamespaceUuid, $"{_package.Identity};{Platform}");
            //string providerKeyName = $"{_package.Id},{_package.PackageVersion},{Platform}";

            //candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.InstallDir, $"{GetInstallDir(_package.Kind)}");
            //candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.UpgradeCode, $"{upgradeCode:B}");
            //candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.DependencyProviderKeyName, $"{providerKeyName}");
            //candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.PackKind, $"{_package.Kind}");
            //candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.SourceDir, $"{_package.DestinationDirectory}");
            //candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.InstallationRecordKey, $"InstalledPacks");

            //if (!candle.Execute())
            //{
            //    throw new Exception(Strings.FailedToCompileMsi);
            //}

            //ITaskItem msi = Link(candle.OutputPath, Path.Combine(outputPath, OutputName), iceSuppressions);

            //AddDefaultPackageFiles(msi);

            //return msi;

            return new TaskItem();
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
