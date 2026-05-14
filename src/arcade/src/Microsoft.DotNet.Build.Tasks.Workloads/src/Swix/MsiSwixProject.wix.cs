// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Deployment.DotNet.Releases;
using Microsoft.DotNet.Build.Tasks.Workloads.Msi;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Swix
{
    /// <summary>
    /// Creates a SWIX project for an MSI package.
    /// </summary>
    public class MsiSwixProject : SwixProjectBase
    {
        ITaskItem _msi;

        /// <summary>
        /// The target platform of the package.
        /// </summary>
        protected string Chip
        {
            get;
        }

        /// <summary>
        /// The machine architecture of the package.
        /// </summary>
        protected string MachineArch
        {
            get;
        }

        /// <summary>
        /// The platform associated with the MSI.
        /// </summary>
        protected string Platform
        {
            get;
        }

        /// <summary>
        /// The product architecture of Visual Studio.
        /// </summary>
        protected string ProductArch
        {
            get;
        }

        /// <inheritdoc />
        protected override string ProjectFile
        {
            get;
        }

        /// <inheritdoc />
        protected override string ProjectSourceDirectory
        {
            get;
        }

        public MsiSwixProject(ITaskItem msi, string baseIntermediateOutputPath, string baseOutputPath,
            ReleaseVersion sdkFeatureBand,
            string chip = null, string machineArch = null, string productArch = null, bool outOfSupport = false) : base(msi.GetMetadata(Metadata.SwixPackageId), new Version(msi.GetMetadata(Metadata.Version)), baseIntermediateOutputPath, baseOutputPath, outOfSupport)
        {
            _msi = msi;
            Chip = chip;
            MachineArch = machineArch;
            ProductArch = productArch;
            Platform = msi.GetMetadata(Metadata.Platform);

            // At least one of Chip or MachineArch should have a value, otherwise we cannot generate valid SWIX.
            if (string.IsNullOrWhiteSpace(Chip) && string.IsNullOrWhiteSpace(MachineArch))
            {
                throw new ArgumentOutOfRangeException(Strings.ChipOrMachineArchRequired);
            }

            // We need to always use the platform as an output folder because the chip value will be x64 for both arm64/x64 MSIs
            // and machineArch is not guaranteed to be applicable. 
            ProjectSourceDirectory = Path.Combine(SwixDirectory, $"{sdkFeatureBand}", Id, Platform);
            ValidateRelativePackagePath(GetRelativePackagePath());

            // The name of the .swixproj file is used to create the JSON manifest that will be merged into the .vsman file later.
            // For drop publishing all the JSON manifests and payloads must reside in the same folder so we shorten the project names
            // and use a hashed filename to avoid path too long errors.
            string hashInputs = $"{Id},{Version.ToString(3)},{sdkFeatureBand},{Platform},{Chip},{machineArch}";
            ProjectFile = $"{Utils.GetTruncatedHash(hashInputs, HashAlgorithmName.SHA256)}.swixproj";

            ReplacementTokens[SwixTokens.__VS_PAYLOAD_SOURCE__] = msi.GetMetadata(Metadata.FullPath);
        }

        /// <inheritdoc />
        protected override string GetRelativePackagePath()
        {
            string relativePath = base.GetRelativePackagePath();

            relativePath += !string.IsNullOrEmpty(Chip) ? $",chip={Chip}" : string.Empty;
            relativePath += !string.IsNullOrEmpty(ProductArch) ? $",productarch={ProductArch}" : string.Empty;
            relativePath += !string.IsNullOrEmpty(Chip) ? $",machinearch={MachineArch}" : string.Empty;

            return Path.Combine(relativePath, Path.GetFileName(_msi.ItemSpec));
        }

        /// <inheritdoc />
        public override string Create()
        {
            string swixProj = EmbeddedTemplates.Extract("msi.swixproj", ProjectSourceDirectory, ProjectFile);
            Utils.StringReplace(swixProj, ReplacementTokens, Encoding.UTF8);
            FileInfo fileInfo = new(_msi.ItemSpec);

            // Since we can't use preprocessor directives in the source, we'll do the conditional authoring inline instead.
            using StreamWriter msiWriter = File.CreateText(Path.Combine(ProjectSourceDirectory, "msi.swr"));

            msiWriter.WriteLine($"use vs");
            msiWriter.WriteLine();
            msiWriter.WriteLine($"package name={Id}");
            msiWriter.WriteLine($"        version={Version}");

            // VS does support setting chip, productArch, and machineArch on a single package. 
            if (!string.IsNullOrWhiteSpace(Chip))
            {
                msiWriter.WriteLine($"        vs.package.chip={Chip}");
            }

            if (!string.IsNullOrWhiteSpace(ProductArch))
            {
                msiWriter.WriteLine($"        vs.package.productArch={ProductArch}");
            }

            if (!string.IsNullOrEmpty(MachineArch))
            {
                msiWriter.WriteLine($"        vs.package.machineArch={MachineArch}");
            }

            if (OutOfSupport)
            {
                msiWriter.WriteLine($"        vs.package.outOfSupport=yes");
            }

            msiWriter.WriteLine($"        vs.package.type=msi");
            msiWriter.WriteLine();
            msiWriter.WriteLine($"vs.installSize");
            msiWriter.WriteLine($"  SystemDrive={MsiUtils.GetInstallSize(_msi.ItemSpec)}");
            msiWriter.WriteLine($"  TargetDrive=0");
            msiWriter.WriteLine($"  SharedDrive=0");
            msiWriter.WriteLine();
            msiWriter.WriteLine($"vs.logFiles");
            msiWriter.WriteLine($"  vs.logFile pattern=\"dd_setup*{Id}*.log\"");
            msiWriter.WriteLine();
            msiWriter.WriteLine($"vs.msiProperties");
            msiWriter.WriteLine($"  vs.msiProperty name=\"MSIFASTINSTALL\" value=\"7\"");
            msiWriter.WriteLine($"  vs.msiProperty name=\"VSEXTUI\" value=\"1\"");
            msiWriter.WriteLine();
            msiWriter.WriteLine($"vs.payloads");
            msiWriter.WriteLine($"  vs.payload source=$(PayloadSource)");
            msiWriter.WriteLine($"             size={fileInfo.Length}");
            msiWriter.WriteLine();

            return swixProj;
        }
    }
}
