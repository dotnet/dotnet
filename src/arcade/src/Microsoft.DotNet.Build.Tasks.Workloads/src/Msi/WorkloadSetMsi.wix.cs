// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks.Workloads.Wix;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Msi
{
    internal class WorkloadSetMsi : MsiBase
    {
        private WorkloadSetPackage _package;

        protected override string BaseOutputName => Path.GetFileNameWithoutExtension(_package.PackagePath);

        public WorkloadSetMsi(WorkloadSetPackage package, string platform, IBuildEngine buildEngine, 
            WixToolsetConfiguration wixToolsetConfig,
            string baseIntermediatOutputPath) :
            base(MsiMetadata.Create(package), buildEngine, wixToolsetConfig, platform, baseIntermediatOutputPath)
        {
            _package = package;
            InstallationRecordKey = $@"{InstallRecordBaseKey}\InstalledWorkloadSets\{Platform}\{_package.SdkFeatureBand}\{_package.PackageVersion}";
            UpgradeCode = Utils.CreateUuid(UpgradeCodeNamespaceUuid, $"{_package.Identity};{Platform}");
            ProviderKeyName = $"Microsoft.NET.Workload.Set,{_package.SdkFeatureBand},{_package.PackageVersion},{Platform}";
            ReplacementTokens[MsiTokens.__PROVIDER_KEY_NAME__] = ProviderKeyName;
            ReplacementTokens[MsiTokens.__UPGRADECODE__] = UpgradeCode.ToString("B");
        }

        public override string Create() => "";

        public  ITaskItem Build2(string outputPath, ITaskItem[]? iceSuppressions)
        {
            //// Harvest the package contents before adding it to the source files we need to compile.
            //string packageContentWxs = Path.Combine(SourcePath, "PackageContent.wxs");
            //string packageDataDirectory = Path.Combine(_package.DestinationDirectory, "data");

            //HarvesterToolTask heat = new(BuildEngine, WixToolsetPath)
            //{
            //    DirectoryReference = MsiDirectories.WorkloadSetVersionDirectory,
            //    OutputFile = packageContentWxs,
            //    Platform = this.Platform,
            //    SourceDirectory = packageDataDirectory
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
            //    AddFile("WorkloadSetProduct.wxs"));

            //// Extract the include file as it's not compilable, but imported by various source files.
            //AddFile("Variables.wxi");

            //Guid upgradeCode = Utils.CreateUuid(UpgradeCodeNamespaceUuid, $"{_package.Identity};{Platform}");
            //string providerKeyName = $"Microsoft.NET.Workload.Set,{_package.SdkFeatureBand},{_package.PackageVersion},{Platform}";

            //// Set up additional preprocessor definitions.
            //candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.UpgradeCode, $"{upgradeCode:B}");
            //candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.DependencyProviderKeyName, $"{providerKeyName}");
            //candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.SourceDir, $"{packageDataDirectory}");
            //candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.SdkFeatureBandVersion, $"{_package.SdkFeatureBand}");
            //candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.WorkloadSetVersion, $"{_package.WorkloadSetVersion}");
            //candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.InstallationRecordKey, $"InstalledWorkloadSets");

            //if (!candle.Execute())
            //{
            //    throw new Exception(Strings.FailedToCompileMsi);
            //}

            //ITaskItem msi = Link(candle.OutputPath, Path.Combine(outputPath, OutputName), iceSuppressions);

            //AddDefaultPackageFiles(msi);

            //return msi;

            return new TaskItem();
        }
    }
}

#nullable disable
