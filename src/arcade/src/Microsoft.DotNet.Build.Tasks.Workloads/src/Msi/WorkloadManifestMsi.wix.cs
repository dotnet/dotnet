// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public WorkloadManifestMsi(WorkloadManifestPackage package, string platform, IBuildEngine buildEngine, string wixToolsetPath,
            string baseIntermediateOutputPath, bool isSxS = false) :
            base(MsiMetadata.Create(package), buildEngine, wixToolsetPath, platform, baseIntermediateOutputPath)
        {
            Package = package;
            IsSxS = isSxS;
        }

        /// <inheritdoc />
        /// <exception cref="Exception" />
        public override ITaskItem Build(string outputPath, ITaskItem[]? iceSuppressions = null)
        {
            // Harvest the package contents before adding it to the source files we need to compile.
            string packageContentWxs = Path.Combine(WixSourceDirectory, "PackageContent.wxs");
            string packageDataDirectory = Path.Combine(Package.DestinationDirectory, "data");

            HarvesterToolTask heat = new(BuildEngine, WixToolsetPath)
            {
                DirectoryReference = IsSxS ? MsiDirectories.ManifestVersionDirectory : MsiDirectories.ManifestIdDirectory,
                OutputFile = packageContentWxs,
                Platform = this.Platform,
                SourceDirectory = packageDataDirectory
            };

            if (!heat.Execute())
            {
                throw new Exception(Strings.HeatFailedToHarvest);
            }

            foreach (var file in Directory.GetFiles(packageDataDirectory).Select(f => Path.GetFullPath(f)))
            {
                NuGetPackageFiles[file] = @"\data\extractedManifest\" + Path.GetFileName(file);
            }

            //  Add WorkloadPackGroups.json to add to workload manifest MSI
            string? jsonContentWxs = null;
            string? jsonDirectory = null;

            if (WorkloadPackGroups.Any())
            {
                jsonContentWxs = Path.Combine(WixSourceDirectory, "JsonContent.wxs");

                string jsonAsString = JsonSerializer.Serialize(WorkloadPackGroups, typeof(IList<WorkloadPackGroupJson>), new JsonSerializerOptions() { WriteIndented = true });
                jsonDirectory = Path.Combine(WixSourceDirectory, "json");
                Directory.CreateDirectory(jsonDirectory);

                string jsonFullPath = Path.GetFullPath(Path.Combine(jsonDirectory, "WorkloadPackGroups.json"));
                File.WriteAllText(jsonFullPath, jsonAsString);

                HarvesterToolTask jsonHeat = new(BuildEngine, WixToolsetPath)
                {
                    DirectoryReference = IsSxS ? MsiDirectories.ManifestVersionDirectory : MsiDirectories.ManifestIdDirectory,
                    OutputFile = jsonContentWxs,
                    Platform = this.Platform,
                    SourceDirectory = jsonDirectory,
                    SourceVariableName = "JsonSourceDir",
                    ComponentGroupName = "CG_PackGroupJson"
                };

                if (!jsonHeat.Execute())
                {
                    throw new Exception(Strings.HeatFailedToHarvest);
                }

                NuGetPackageFiles[jsonFullPath] = @"\data\extractedManifest\" + Path.GetFileName(jsonFullPath);
            }

            CompilerToolTask candle = CreateDefaultCompiler();
            candle.AddSourceFiles(packageContentWxs,
                EmbeddedTemplates.Extract("DependencyProvider.wxs", WixSourceDirectory),
                EmbeddedTemplates.Extract("dotnethome_x64.wxs", WixSourceDirectory),
                EmbeddedTemplates.Extract("ManifestProduct.wxs", WixSourceDirectory),
                EmbeddedTemplates.Extract("Registry.wxs", WixSourceDirectory));

            if (IsSxS)
            {
                candle.AddPreprocessorDefinition("ManifestVersion", Package.GetManifest().Version);
            }

            if (jsonContentWxs != null)
            {
                candle.AddSourceFiles(jsonContentWxs);
                candle.AddPreprocessorDefinition("IncludePackGroupJson", "true");
                candle.AddPreprocessorDefinition("JsonSourceDir", jsonDirectory);
            }
            else
            {
                candle.AddPreprocessorDefinition("IncludePackGroupJson", "false");
            }

            // Only extract the include file as it's not compilable, but imported by various source files.
            EmbeddedTemplates.Extract("Variables.wxi", WixSourceDirectory);

            // To support upgrades, the UpgradeCode must be stable within an SDK feature band.
            // For example, 6.0.101 and 6.0.108 will generate the same GUID for the same platform and manifest ID. 
            // The workload author will need to guarantee that the version for the MSI is higher than previous shipped versions
            // to ensure upgrades correctly trigger. For SxS installs we use the package identity that would include that includes
            // the package version.
            Guid upgradeCode = IsSxS ? Utils.CreateUuid(UpgradeCodeNamespaceUuid, $"{Package.Identity};{Platform}") :
                Utils.CreateUuid(UpgradeCodeNamespaceUuid, $"{Package.ManifestId};{Package.SdkFeatureBand};{Platform}");
            string providerKeyName = IsSxS ?
                $"{Package.ManifestId},{Package.SdkFeatureBand},{Package.PackageVersion},{Platform}" :
                $"{Package.ManifestId},{Package.SdkFeatureBand},{Platform}";

            // Set up additional preprocessor definitions.
            candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.UpgradeCode, $"{upgradeCode:B}");
            candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.DependencyProviderKeyName, $"{providerKeyName}");
            candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.SourceDir, $"{packageDataDirectory}");
            candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.SdkFeatureBandVersion, $"{Package.SdkFeatureBand}");
            candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.InstallationRecordKey, $"InstalledManifests");

            // The temporary installer in the SDK (6.0) used lower invariants of the manifest ID.
            // We have to do the same to ensure the keypath generation produces stable GUIDs.
            candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.ManifestId, $"{Package.ManifestId.ToLowerInvariant()}");

            if (!candle.Execute())
            {
                throw new Exception(Strings.FailedToCompileMsi);
            }

            ITaskItem msi = Link(candle.OutputPath, Path.Combine(outputPath, OutputName), iceSuppressions);

            AddDefaultPackageFiles(msi);

            return msi;
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
