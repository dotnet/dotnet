// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks.Workloads.Wix;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Msi
{
    /// <summary>
    /// Base class for workload MSIs templates.
    /// </summary>
    internal abstract class MsiBase : WorkloadTemplateBase
    {
        /// <summary>
        /// MSBuild tool task for running the WiX CLI.
        /// </summary>
        private WixToolTask _wixToolTask;

        /// <summary>
        /// The base registry key where all workload records are written.
        /// </summary>
        protected const string InstallRecordBaseKey = @"SOFTWARE\Microsoft\dotnet";

        /// <summary>
        /// Replacement token for license URLs in the generated EULA.
        /// </summary>
        private static readonly string __LICENSE_URL__ = nameof(__LICENSE_URL__);

        /// <summary>
        /// Static RTF text for inserting a EULA into the MSI. The license URL of the NuGet package will be embedded 
        /// as plain text since the text control used to render the MSI UI does not render hyperlinks even though RTF supports
        /// it.
        /// </summary>
        internal static readonly string s_eula = @"{\rtf1\ansi\ansicpg1252\deff0\nouicompat\deflang1033{\fonttbl{\f0\fnil\fcharset0 Calibri;}}
{\colortbl ;\red0\green0\blue255;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sa200\sl276\slmult1\f0\fs22\lang9 This software is licensed separately as set out in its accompanying license. By continuing, you also agree to that license (__LICENSE_URL__).\par
\par
}";
        /// <summary>
        /// The UUID namespace to use for generating an upgrade code.
        /// </summary>
        internal static readonly Guid UpgradeCodeNamespaceUuid = Guid.Parse("C743F81B-B3B5-4E77-9F6D-474EFF3A722C");

        /// <summary>
        /// Metadata for the MSI such as package ID, version, author information, etc.
        /// </summary>
        public IWorkloadPackageMetadata Metadata
        {
            get;
        }

        /// <summary>
        /// The filename of the MSI. The name excludes the platform identifier and extension.
        /// </summary>
        protected abstract string BaseOutputName
        {
            get;
        }

        protected IBuildEngine BuildEngine
        {
            get;
        }

        /// <summary>
        /// When <see langword="true"/>, a wixpack archive will be generated that can be used to sign the MSI.
        /// </summary>
        protected bool CreateWixPack
        {
            get;
        }

        /// <summary>
        /// The root intermediate output directory. 
        /// </summary>
        protected string BaseIntermediateOutputPath
        {
            get;
        }

        /// <summary>
        /// The registry key for tracking installation records used by the CLI and
        /// and finalizer. 
        /// </summary>
        protected string InstallationRecordKey
        {
            get;
            init;
        }

        /// <summary>
        /// Gets the value to use for the manufacturer. 
        /// </summary>
        protected string Manufacturer =>
            (!string.IsNullOrWhiteSpace(Metadata.Authors) && (Metadata.Authors.IndexOf("Microsoft", StringComparison.OrdinalIgnoreCase) < 0)) ?
            Metadata.Authors :
            DefaultValues.Manufacturer;

        /// <summary>
        /// The package type represented by the MSI.
        /// </summary>
        protected abstract string? MsiPackageType
        {
            get;
        }

        /// <summary>
        /// The provider key name used to manage MSI dependents.
        /// </summary>
        protected string ProviderKeyName
        {
            get;
            init;
        }

        /// <summary>
        /// The platform of the MSI.
        /// </summary>
        protected string Platform
        {
            get;
        }

        /// <summary>
        /// The ProductCode of the MSI.
        /// </summary>
        protected Guid ProductCode
        {
            get;
            init;
        }

        /// <summary>
        /// The filename of the MSI file to generate.
        /// </summary>
        protected string OutputName =>
            $"{Utils.GetTruncatedHash(BaseOutputName, HashAlgorithmName.SHA256)}-{Platform}.msi";

        /// <summary>
        /// Toolset configuration to use to invoke the WiX CLI and related tools.
        /// </summary>
        protected WixToolsetConfiguration WixToolsetConfig
        {
            get;
        }

        /// <summary>
        /// The UpgradeCode of the MSI.
        /// </summary>
        protected Guid UpgradeCode
        {
            get;
            init;
        }

        /// <summary>
        /// Set of files to include in the NuGet package that will wrap the MSI. Keys represent the source files and the
        /// value contains the relative path inside the generated NuGet package.
        /// </summary>
        public Dictionary<string, string> NuGetPackageFiles { get; set; } = new();

        public MsiBase(IWorkloadPackageMetadata metadata, IBuildEngine buildEngine, WixToolsetConfiguration wixToolsetConfig,
            string platform, string baseIntermediateOutputPath, bool createWixPack = true) : base(baseIntermediateOutputPath)
        {
            BuildEngine = buildEngine;
            Metadata = metadata;
            WixToolsetConfig = wixToolsetConfig;
            CreateWixPack = createWixPack;

            InstallationRecordKey = InstallRecordBaseKey;
            Platform = platform;
            BaseIntermediateOutputPath = baseIntermediateOutputPath;
            ProviderKeyName = "";

            ProductCode = Guid.NewGuid();
            ReplacementTokens[MsiTokens.__MANUFACTURER__] = Manufacturer;
            ReplacementTokens[MsiTokens.__NAME__] = GetProductName(Platform);
            ReplacementTokens[MsiTokens.__PACKAGE_ID__] = metadata.Id;
            ReplacementTokens[MsiTokens.__PACKAGE_VERSION__] = metadata.PackageVersion.ToString();
            ReplacementTokens[MsiTokens.__PRODUCTCODE__] = ProductCode.ToString("B");
            ReplacementTokens[MsiTokens.__VERSION__] = metadata.MsiVersion.ToString();

            SourcePath = Path.Combine(SourcePath, "wix", metadata.Id, $"{metadata.PackageVersion}", platform);

            _wixToolTask = new WixToolTask(buildEngine, wixToolsetConfig);
        }

        /// <summary>
        /// Create the initial set of source files required to build the MSI. 
        /// </summary>
        /// <remarks>
        /// Derived classes should call this method inside their own Create method to ensure the base product source file is generated. 
        /// </remarks>
        /// <returns>A WixDocument representing the Product.wxs source file.</returns>
        protected WixDocument CreateProduct()
        {
            // Generate the EULA on disk and include its path as a replacement token before
            // the primary .wxs template is created since AddFile always applies the replacement tokens.
            ReplacementTokens[MsiTokens.__EULA_RTF__] = GenerateEula();
            var productDoc = new WixDocument(AddFile("Product.wxs"));

            // Add additional authoring to set DOTNETHOME property based on the native machine
            // type. This is needed to support non-native installs like x64 on arm64.
            if (Platform == "x64")
            {
                AddFile("dotnethome_x64.wxs");
                productDoc.AddCustomActionRef("Set_DOTNETHOME_NON_NATIVE_ARCHITECTURE");
            }

            return productDoc;
        }

        /// <summary>
        /// Creates a RegistryKey for the workload installation record.
        /// </summary>
        /// <returns>The RegistryKey element for the workload installation record.</returns>
        protected virtual XElement CreateInstallationRecord() =>
            WixDocument.CreateRegistryKey(InstallationRecordKey, "HKLM")
                .AddRegistryValue("DependencyProviderKey", ProviderKeyName, keyPath: true)
                .AddRegistryValue("ProductCode", $"{ProductCode:B}")
                .AddRegistryValue("UpgradeCode", $"{UpgradeCode:B}")
                .AddRegistryValue("ProductVersion", Metadata.MsiVersion.ToString())
                .AddRegistryValue("ProductLanguage", DefaultValues.Wix.Language, type: "integer");

        /// <summary>
        /// Produces an MSI and returns a task item with metadata about the MSI.
        /// </summary>
        /// <param name="outputPath">The directory where the MSI will be generated.</param>
        /// <returns>An item representing the built MSI.</returns>
        public ITaskItem Build(string outputPath)
        {
            // Ensure that all the sources are generated before building the MSI.
            Create();

            _wixToolTask.Architecture = Platform;
            _wixToolTask.OutputPath = Path.Combine(outputPath, OutputName);
            _wixToolTask.AddPreprocessorDefinition(PreprocessorDefinitionNames.InstallerPlatform, Platform);
            _wixToolTask.AddSourceFiles(Files);

            if (!_wixToolTask.Execute())
            {
                throw new Exception(string.Format(Strings.FailedToCompileMsi, _wixToolTask.GetWixCommandLine()));
            }

            TaskItem msiItem = new TaskItem(_wixToolTask.OutputPath);
            msiItem.SetMetadata(Workloads.Metadata.Platform, Platform);
            msiItem.SetMetadata(Workloads.Metadata.Version, $"{Metadata.MsiVersion}");
            msiItem.SetMetadata(Workloads.Metadata.SwixPackageId, Metadata.SwixPackageId);
            msiItem.SetMetadata(Workloads.Metadata.PackageType, MsiPackageType);
            msiItem.SetMetadata(Workloads.Metadata.SourcePath, SourcePath);

            // NuGet limits package sizes to 250MB and Visual Studio has seen degraded performance for online installs
            // when files exceed this. The actual limit is capped below 250MB to account for additional metadata and files
            // we may include in the NuGet package that will wrap the workload MSI.
            var fi = new FileInfo(msiItem.ItemSpec);
            if (fi.Length > DefaultValues.MaxMsiSize)
            {
                throw new Exception($"The generated MSI, {msiItem.ItemSpec}, exceeded the maximum allowed size ({DefaultValues.MaxMsiSize} bytes).");
            }

            // Create the JSON manifest for CLI based installations.
            string msiJsonPath = MsiProperties.Create(msiItem.ItemSpec);
            NuGetPackageFiles[Path.GetFullPath(msiJsonPath)] = @"\data\msi.json";
            NuGetPackageFiles[msiItem.GetMetadata(Workloads.Metadata.FullPath)] = @"\data";
            NuGetPackageFiles["LICENSE.TXT"] = @"\";

            if (CreateWixPack)
            {
                var createWixPackTask = _wixToolTask.GetCreateWixBuildWixpackTask(BuildEngine, IntermediateOutputPath,
                    OutputPath, _wixToolTask.OutputPath, Path.Combine(SourcePath, "wixpack"));

                if (!createWixPackTask.Execute())
                {
                    throw new Exception("Failed to generate wixpack.");
                }

                msiItem.SetMetadata(Workloads.Metadata.WixPack, createWixPackTask.OutputFile);
            }

            return msiItem;
        }

        /// <summary>
        /// Gets the platform specific ProductName MSI property.  
        /// </summary>
        /// <param name="platform">The platform targeted by the MSI.</param>
        /// <returns>A string containing the product name of the MSI.</returns>
        protected string GetProductName(string platform) =>
            (string.IsNullOrWhiteSpace(Metadata.Title) ? Metadata.Id : Metadata.Title) + $" ({platform})";

        /// <summary>
        /// Generates a EULA (RTF file) that contains the license URL of the underlying NuGet package.
        /// </summary>
        protected string GenerateEula()
        {
            string eulaRtf = Path.Combine(SourcePath, "eula.rtf");
            Directory.CreateDirectory(SourcePath);
            File.WriteAllText(eulaRtf, s_eula.Replace(__LICENSE_URL__, Metadata.LicenseUrl));

            return eulaRtf;
        }

        /// <summary>
        /// Invokes Heat to harvest a directory and creates a ComponentGroupRef element that can be inserted into
        /// the generated source.
        /// </summary>
        /// <param name="sourcePath">The file system of the directory to harvest.</param>
        /// <param name="directoryReference">The directory reference to use for root directories.</param>
        /// <param name="sourceVariableName">The preprocessor variable to use for substituting File@Source.</param>
        /// <returns>A string containing the component group ID associated with the harvested files.</returns>
        /// <exception cref="Exception">If Heat failed to execute.</exception>
        /// <remarks>Starting with v5, WiX supports a Files element that can be used to perform simple harvesting using
        /// globs. The authoring is generated in memory instead of a separate source file, making it incompatible with 
        /// wixpacks Arcade uses for signing.
        /// </remarks>
        protected string HarvestDirectory(string sourcePath, string directoryReference,
            string sourceVariableName = DefaultValues.Wix.SourceVariableName)
        {
            // Generate a random component group ID. The generated ComponentGroupRef XML element will have the same ID
            // and can then be inserted into any element that supports havinge a ComponentGroupRef as a child, for example,
            // a Feature element.
            string componentGroupId = $"cg{Guid.NewGuid():N}";
            string outputFile = Path.Combine(SourcePath, componentGroupId + ".wxs");

            HarvesterToolTask heat = new(BuildEngine, WixToolsetConfig)
            {
                ComponentGroupName = componentGroupId,
                DirectoryReference = directoryReference,
                OutputFile = outputFile,
                Platform = this.Platform,
                SourceDirectory = sourcePath,
                SourceVariableName = sourceVariableName
            };

            if (!heat.Execute())
            {
                throw new Exception(Strings.HeatFailedToHarvest);
            }

            AddSourceFile(outputFile);
            _wixToolTask.AddPreprocessorDefinition(sourceVariableName, sourcePath);

            return componentGroupId;
        }
    }
}

#nullable disable
