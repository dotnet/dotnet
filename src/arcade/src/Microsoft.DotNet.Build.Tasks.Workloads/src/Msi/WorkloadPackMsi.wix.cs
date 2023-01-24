// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.DotNet.Build.Tasks.Workloads.Wix;
using Microsoft.NET.Sdk.WorkloadManifestReader;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Msi
{
    internal class WorkloadPackMsi : MsiBase
    {
        private WorkloadPackPackage _package;

        /// <inheritdoc />
        protected override string BaseOutputName => _package.ShortName;

        public WorkloadPackMsi(WorkloadPackPackage package, string platform, IBuildEngine buildEngine, string wixToolsetPath,
            string baseIntermediatOutputPath) :
            base(MsiMetadata.Create(package), buildEngine, wixToolsetPath, platform, baseIntermediatOutputPath)
        {
            _package = package;
        }

        public override ITaskItem Build(string outputPath, ITaskItem[]? iceSuppressions = null)
        {
            // Harvest the package contents before adding it to the source files we need to compile.
            string packageContentWxs = Path.Combine(WixSourceDirectory, "PackageContent.wxs");
            string directoryReference = _package.Kind == WorkloadPackKind.Library || _package.Kind == WorkloadPackKind.Template ?
                "InstallDir" : "VersionDir";

            HarvesterToolTask heat = new(BuildEngine, WixToolsetPath)
            {
                DirectoryReference = directoryReference,
                OutputFile = packageContentWxs,
                Platform = this.Platform,
                SourceDirectory = _package.DestinationDirectory
            };

            if (!heat.Execute())
            {
                throw new Exception(Strings.HeatFailedToHarvest);
            }

            CompilerToolTask candle = CreateDefaultCompiler();

            candle.AddSourceFiles(packageContentWxs,
                EmbeddedTemplates.Extract("DependencyProvider.wxs", WixSourceDirectory),
                EmbeddedTemplates.Extract("Directories.wxs", WixSourceDirectory),
                EmbeddedTemplates.Extract("dotnethome_x64.wxs", WixSourceDirectory),
                EmbeddedTemplates.Extract("Product.wxs", WixSourceDirectory),
                EmbeddedTemplates.Extract("Registry.wxs", WixSourceDirectory));

            // Only extract the include file as it's not compilable, but imported by various source files.
            EmbeddedTemplates.Extract("Variables.wxi", WixSourceDirectory);

            // Workload packs are not upgradable so the upgrade code is generated using the package identity as that
            // includes the package version.
            Guid upgradeCode = Utils.CreateUuid(UpgradeCodeNamespaceUuid, $"{_package.Identity};{Platform}");
            string providerKeyName = $"{_package.Id},{_package.PackageVersion},{Platform}";

            candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.InstallDir, $"{GetInstallDir(_package.Kind)}");
            candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.UpgradeCode, $"{upgradeCode:B}");
            candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.DependencyProviderKeyName, $"{providerKeyName}");
            candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.PackKind, $"{_package.Kind}");
            candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.SourceDir, $"{_package.DestinationDirectory}");
            candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.InstallationRecordKey, $"InstalledPacks");

            if (!candle.Execute())
            {
                throw new Exception(Strings.FailedToCompileMsi);
            }

            ITaskItem msi = Link(candle.OutputPath, Path.Combine(outputPath, OutputName), iceSuppressions);

            AddDefaultPackageFiles(msi);

            return msi;
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
    }
}

#nullable disable
