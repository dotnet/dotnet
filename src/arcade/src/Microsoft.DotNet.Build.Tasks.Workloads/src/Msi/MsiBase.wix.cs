// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks.Workloads.Wix;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Msi
{
    internal abstract class MsiBase
    {
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
        public MsiMetadata Metadata
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
        /// The directory where the compiler output (.wixobj files) will be generated.
        /// </summary>
        protected string CompilerOutputPath
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
        /// Gets the value to use for the manufacturer. 
        /// </summary>
        protected string Manufacturer =>
            (!string.IsNullOrWhiteSpace(Metadata.Authors) && (Metadata.Authors.IndexOf("Microsoft", StringComparison.OrdinalIgnoreCase) < 0)) ?
            Metadata.Authors :
            DefaultValues.Manufacturer;

        /// <summary>
        /// The platform of the MSI.
        /// </summary>
        protected string Platform
        {
            get;
        }

        /// <summary>
        /// The filename of the MSI file to generate.
        /// </summary>
        protected string OutputName => $"{Utils.GetTruncatedHash(BaseOutputName, HashAlgorithmName.SHA256)}-{Platform}.msi";

        /// <summary>
        /// The directory where the WiX source code will be generated.
        /// </summary>
        protected string WixSourceDirectory
        {
            get;
        }

        /// <summary>
        /// The directory containing the WiX toolset binaries.
        /// </summary>
        protected string WixToolsetPath
        {
            get;
        }

        /// <summary>
        /// Set of files to include in the NuGet package that will wrap the MSI. Keys represent the source files and the
        /// value contains the relative path inside the generated NuGet package.
        /// </summary>
        public Dictionary<string, string> NuGetPackageFiles { get; set; } = new();

        public MsiBase(MsiMetadata metadata, IBuildEngine buildEngine, string wixToolsetPath,
            string platform, string baseIntermediateOutputPath)
        {
            BuildEngine = buildEngine;
            WixToolsetPath = wixToolsetPath;
            Platform = platform;
            BaseIntermediateOutputPath = baseIntermediateOutputPath;

            // Candle expects the output path to be terminated with a single '\'.
            CompilerOutputPath = Utils.EnsureTrailingSlash(Path.Combine(baseIntermediateOutputPath, "wixobj", metadata.Id, $"{metadata.PackageVersion}", platform));
            WixSourceDirectory = Path.Combine(baseIntermediateOutputPath, "src", "wix", metadata.Id, $"{metadata.PackageVersion}", platform);
            Metadata = metadata;
        }

        /// <summary>
        /// Produces an MSI and returns a task item with metadata about the MSI.
        /// </summary>
        /// <param name="outputPath">The directory where the MSI will be generated.</param>
        /// <param name="iceSuppressions">A set of internal consistency evaluators to suppress or <see langword="null"/>.</param>
        /// <returns>An item representing the built MSI.</returns>
        public abstract ITaskItem Build(string outputPath, ITaskItem[]? iceSuppressions);

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
            string eulaRtf = Path.Combine(WixSourceDirectory, "eula.rtf");
            File.WriteAllText(eulaRtf, s_eula.Replace(__LICENSE_URL__, Metadata.LicenseUrl));

            return eulaRtf;
        }

        /// <summary>
        /// Creates a new compiler tool task and configures some common extensions and preprocessor
        /// variables.
        /// </summary>
        /// <returns></returns>
        protected CompilerToolTask CreateDefaultCompiler()
        {
            CompilerToolTask candle = new(BuildEngine, WixToolsetPath, CompilerOutputPath, Platform);

            // Required extension to parse the dependency provider authoring.
            candle.AddExtension(WixExtensions.WixDependencyExtension);

            candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.EulaRtf, GenerateEula());
            candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.Manufacturer, Manufacturer);
            candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.PackageId, Metadata.Id);
            candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.PackageVersion, $"{Metadata.PackageVersion}");
            candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.Platform, Platform);
            candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.ProductCode, $"{Guid.NewGuid():B}");
            candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.ProductName, GetProductName(Platform));
            candle.AddPreprocessorDefinition(PreprocessorDefinitionNames.ProductVersion, $"{Metadata.MsiVersion}");

            return candle;
        }

        /// <summary>
        /// Links the MSI using the output from the WiX compiler using a default set of WiX extensions.
        /// </summary>
        /// <param name="compilerOutputPath">The path where the output of the compiler (.wixobj files) will be generated.</param>
        /// <param name="outputFile">The full path of the MSI to create during linking.</param>
        /// <param name="iceSuppressions">A set of internal consistency evaluators to suppress. May be <see langword="null"/>.</param>
        /// <returns>An <see cref="ITaskItem"/> for the MSI that was created.</returns>
        /// <exception cref="Exception"></exception>
        protected ITaskItem Link(string compilerOutputPath, string outputFile, ITaskItem[]? iceSuppressions = null)
        {
            return Link(compilerOutputPath, outputFile, iceSuppressions, WixExtensions.WixDependencyExtension,
                WixExtensions.WixUIExtension, WixExtensions.WixUtilExtension);
        }

        /// <summary>
        /// Links the MSI using the output from the WiX compiler and a set of WiX extensions.
        /// </summary>
        /// <param name="compilerOutputPath">The path where the output of the compiler (.wixobj files) can be found.</param>
        /// <param name="outputFile">The full path of the MSI to create during linking.</param>
        /// <param name="iceSuppressions">A set of internal consistency evaluators to suppress. May be <see langword="null"/>.</param>
        /// <param name="wixExtensions">A list of WiX extensions to include when linking the MSI.</param>
        /// <returns>An <see cref="ITaskItem"/> for the MSI that was created.</returns>
        /// <exception cref="Exception"></exception>
        protected ITaskItem Link(string compilerOutputPath, string outputFile, ITaskItem[]? iceSuppressions, params string[] wixExtensions)
        {
            // Link the MSI. The generated filename contains the semantic version (excluding build metadata) and platform. 
            // If the source package already contains a platform, e.g. an aliased package that has a RID, then we don't add
            // the platform again.
            LinkerToolTask light = new(BuildEngine, WixToolsetPath)
            {
                OutputFile = outputFile,
                SourceFiles = Directory.EnumerateFiles(compilerOutputPath, "*.wixobj"),
                SuppressIces = iceSuppressions == null ? null : string.Join(";", iceSuppressions.Select(i => i.ItemSpec))
            };

            foreach (string wixExtension in wixExtensions)
            {
                light.AddExtension(wixExtension);
            }

            if (!light.Execute())
            {
                throw new Exception(Strings.FailedToLinkMsi);
            }

            TaskItem msiItem = new TaskItem(light.OutputFile);

            // Return a task item that contains all the information about the generated MSI.            
            msiItem.SetMetadata(Workloads.Metadata.Platform, Platform);
            msiItem.SetMetadata(Workloads.Metadata.WixObj, compilerOutputPath);
            msiItem.SetMetadata(Workloads.Metadata.Version, $"{Metadata.MsiVersion}");
            msiItem.SetMetadata(Workloads.Metadata.SwixPackageId, Metadata.SwixPackageId);

            return msiItem;
        }

        protected void AddDefaultPackageFiles(ITaskItem msi)
        {
            NuGetPackageFiles[msi.GetMetadata(Workloads.Metadata.FullPath)] = @"\data";

            // Create the JSON manifest for CLI based installations.
            string msiJsonPath = MsiProperties.Create(msi.ItemSpec);
            NuGetPackageFiles[Path.GetFullPath(msiJsonPath)] = "\\data\\msi.json";

            NuGetPackageFiles["LICENSE.TXT"] = @"\";
        }
    }
}

#nullable disable
