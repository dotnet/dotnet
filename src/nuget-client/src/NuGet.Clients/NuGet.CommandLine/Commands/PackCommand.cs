// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Packaging.Core;

namespace NuGet.CommandLine
{
    using System.Globalization;
    using NuGet.Packaging;
    using NuGet.Versioning;

    [Command(typeof(NuGetCommand), "pack", "PackageCommandDescription", MaxArgs = 1, UsageSummaryResourceName = "PackageCommandUsageSummary",
            UsageDescriptionResourceName = "PackageCommandUsageDescription", UsageExampleResourceName = "PackCommandUsageExamples")]
    public class PackCommand : Command
    {
        internal static readonly string SymbolsExtension = ".symbols" + PackagingCoreConstants.NupkgExtension;

        private readonly HashSet<string> _excludes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, string> _properties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private Version _minClientVersionValue;

        [Option(typeof(NuGetCommand), "PackageCommandOutputDirDescription")]
        public string OutputDirectory { get; set; }

        [Option(typeof(NuGetCommand), "PackageCommandBasePathDescription")]
        public string BasePath { get; set; }

        [Option(typeof(NuGetCommand), "PackageCommandVersionDescription")]
        public string Version { get; set; }

        [Option(typeof(NuGetCommand), "PackageCommandSuffixDescription")]
        public string Suffix { get; set; }

        [Option(typeof(NuGetCommand), "PackageCommandExcludeDescription")]
        public ICollection<string> Exclude
        {
            get { return _excludes; }
        }

        [Option(typeof(NuGetCommand), "PackageCommandSymbolsDescription")]
        public bool Symbols { get; set; }

        [Option(typeof(NuGetCommand), "PackageCommandToolDescription")]
        public bool Tool { get; set; }

        [Option(typeof(NuGetCommand), "PackageCommandBuildDescription")]
        public bool Build { get; set; }

        [Option(typeof(NuGetCommand), "PackageCommandNoDefaultExcludes")]
        public bool NoDefaultExcludes { get; set; }

        [Option(typeof(NuGetCommand), "PackageCommandNoRunAnalysis")]
        public bool NoPackageAnalysis { get; set; }

        [Option(typeof(NuGetCommand), "PackageCommandExcludeEmptyDirectories")]
        public bool ExcludeEmptyDirectories { get; set; }

        [Option(typeof(NuGetCommand), "PackageCommandIncludeReferencedProjects")]
        public bool IncludeReferencedProjects { get; set; }

        [Option(typeof(NuGetCommand), "PackageCommandPropertiesDescription", AltName = "p")]
        public Dictionary<string, string> Properties
        {
            get
            {
                return _properties;
            }
        }

        [Option(typeof(NuGetCommand), "PackageCommandMinClientVersion")]
        public string MinClientVersion { get; set; }

        [Option(typeof(NuGetCommand), "PackageCommandSymbolPackageFormat")]
        public string SymbolPackageFormat { get; set; }

        [Option(typeof(NuGetCommand), "PackageCommandPackagesDirectory")]
        public string PackagesDirectory { get; set; }

        [Option(typeof(NuGetCommand), "PackageCommandSolutionDirectory")]
        public string SolutionDirectory { get; set; }

        [Option(typeof(NuGetCommand), "CommandMSBuildVersion")]
        public string MSBuildVersion { get; set; }

        [Option(typeof(NuGetCommand), "CommandMSBuildPath")]
        public string MSBuildPath { get; set; }

        [Option(typeof(NuGetCommand), "PackageCommandInstallPackageToOutputPath")]
        public bool InstallPackageToOutputPath { get; set; }

        [Option(typeof(NuGetCommand), "PackageCommandOutputFileNamesWithoutVersion")]
        public bool OutputFileNamesWithoutVersion { get; set; }

        [Option(typeof(NuGetCommand), "PackageCommandConfigFile")]
        public new string ConfigFile { get; set; }

        public override void ExecuteCommand()
        {
            var packArgs = new PackArgs();
            packArgs.Logger = Console;
            packArgs.Arguments = Arguments;
            packArgs.OutputDirectory = OutputDirectory;
            packArgs.BasePath = BasePath;
            packArgs.MsBuildDirectory = new Lazy<string>(() => MsBuildUtility.GetMsBuildDirectoryFromMsBuildPath(MSBuildPath, MSBuildVersion, Console).Value.Path);

            if (!string.IsNullOrEmpty(PackagesDirectory))
            {
                packArgs.PackagesDirectory = Path.GetFullPath(PackagesDirectory);
            }

            if (!string.IsNullOrEmpty(SolutionDirectory))
            {
                packArgs.SolutionDirectory = Path.GetFullPath(SolutionDirectory);
            }

            // Get the input file
            packArgs.Path = PackCommandRunner.GetInputFile(packArgs);

            // Set the current directory if the files being packed are in a different directory
            PackCommandRunner.SetupCurrentDirectory(packArgs);

            Console.WriteLine(LocalizedResourceManager.GetString("PackageCommandAttemptingToBuildPackage"), Path.GetFileName(packArgs.Path));

            if (!string.IsNullOrEmpty(MinClientVersion))
            {
                if (!System.Version.TryParse(MinClientVersion, out _minClientVersionValue))
                {
                    throw new CommandException(LocalizedResourceManager.GetString("PackageCommandInvalidMinClientVersion"));
                }
            }

            if (!string.IsNullOrEmpty(SymbolPackageFormat))
            {
                packArgs.SymbolPackageFormat = PackArgs.GetSymbolPackageFormat(SymbolPackageFormat);
            }
            packArgs.Build = Build;
            packArgs.Exclude = Exclude;
            packArgs.ExcludeEmptyDirectories = ExcludeEmptyDirectories;
            packArgs.IncludeReferencedProjects = IncludeReferencedProjects;
            switch (Verbosity)
            {
                case Verbosity.Detailed:
                    {
                        packArgs.LogLevel = LogLevel.Verbose;
                        break;
                    }
                case Verbosity.Normal:
                    {
                        packArgs.LogLevel = LogLevel.Information;
                        break;
                    }
                case Verbosity.Quiet:
                    {
                        packArgs.LogLevel = LogLevel.Minimal;
                        break;
                    }
            }
            packArgs.MinClientVersion = _minClientVersionValue;
            packArgs.NoDefaultExcludes = NoDefaultExcludes;
            packArgs.NoPackageAnalysis = NoPackageAnalysis;
            if (Properties.Any())
            {
                packArgs.Properties.AddRange(Properties);
            }
            packArgs.Suffix = Suffix;
            packArgs.Symbols = Symbols;
            packArgs.Tool = Tool;
            packArgs.InstallPackageToOutputPath = InstallPackageToOutputPath;
            packArgs.OutputFileNamesWithoutVersion = OutputFileNamesWithoutVersion;

            if (!string.IsNullOrEmpty(Version))
            {
                NuGetVersion version;
                if (!NuGetVersion.TryParse(Version, out version))
                {
                    throw new PackagingException(NuGetLogCode.NU5010, string.Format(CultureInfo.CurrentCulture, NuGetResources.InstallCommandPackageReferenceInvalidVersion, Version));
                }
                packArgs.Version = version.ToFullString();
            }

            var packCommandRunner = new PackCommandRunner(packArgs, ProjectFactory.ProjectCreator);
            if (!packCommandRunner.RunPackageBuild())
            {
                throw new ExitCodeException(1);
            }
        }
    }
}
