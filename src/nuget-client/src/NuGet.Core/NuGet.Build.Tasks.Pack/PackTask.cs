// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.Build.Framework;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Packaging;
using ILogger = NuGet.Common.ILogger;

namespace NuGet.Build.Tasks.Pack
{
    public class PackTask : Microsoft.Build.Utilities.Task, IPackTaskRequest<ITaskItem>
    {
        [Required]
        public ITaskItem PackItem { get; set; }

        public ITaskItem[] PackageFiles { get; set; }
        public ITaskItem[] PackageFilesToExclude { get; set; }
        public string[] TargetFrameworks { get; set; }
        public string[] PackageTypes { get; set; }
        public ITaskItem[] BuildOutputInPackage { get; set; }
        public ITaskItem[] ProjectReferencesWithVersions { get; set; }
        public string PackageId { get; set; }
        public string PackageVersion { get; set; }
        public string Title { get; set; }
        public string[] Authors { get; set; }
        public string Description { get; set; }
        public bool DevelopmentDependency { get; set; }
        public string Copyright { get; set; }
        public bool RequireLicenseAcceptance { get; set; }
        public string RestoreOutputPath { get; set; }
        public string LicenseUrl { get; set; }
        public string ProjectUrl { get; set; }
        public string IconUrl { get; set; }
        public string[] Tags { get; set; }
        public string ReleaseNotes { get; set; }
        public ITaskItem[] TargetPathsToSymbols { get; set; }
        public ITaskItem[] FrameworksWithSuppressedDependencies { get; set; }
        public string AssemblyName { get; set; }
        public string PackageOutputPath { get; set; }
        public bool IsTool { get; set; }
        public bool IncludeSymbols { get; set; }
        public bool IncludeSource { get; set; }
        public bool InstallPackageToOutputPath { get; set; }
        public bool OutputFileNamesWithoutVersion { get; set; }
        public string RepositoryUrl { get; set; }
        public string RepositoryType { get; set; }
        public string RepositoryBranch { get; set; }
        public string RepositoryCommit { get; set; }
        public ITaskItem[] SourceFiles { get; set; }
        public bool NoPackageAnalysis { get; set; }
        public string NuspecFile { get; set; }
        public string SymbolPackageFormat { get; set; }
        public string MinClientVersion { get; set; }
        public bool Serviceable { get; set; }
        public ITaskItem[] FrameworkAssemblyReferences { get; set; }
        public bool ContinuePackingAfterGeneratingNuspec { get; set; }
        public bool NoDefaultExcludes { get; set; }
        public string NuspecOutputPath { get; set; }
        public bool IncludeBuildOutput { get; set; }
        public string[] BuildOutputFolders { get; set; }
        public string[] ContentTargetFolders { get; set; }
        public string[] NuspecProperties { get; set; }
        public string NuspecBasePath { get; set; }
        public string[] AllowedOutputExtensionsInPackageBuildOutputFolder { get; set; }
        public string[] AllowedOutputExtensionsInSymbolsPackageBuildOutputFolder { get; set; }
        public string NoWarn { get; set; }
        public string TreatWarningsAsErrors { get; set; }
        public string WarningsAsErrors { get; set; }
        public string WarningsNotAsErrors { get; set; }
        public string PackageLicenseExpression { get; set; }
        public string PackageLicenseFile { get; set; }
        public string PackageLicenseExpressionVersion { get; set; }
        public string Readme { get; set; }
        public bool Deterministic { get; set; }
        public string PackageIcon { get; set; }
        public ILogger Logger => new MSBuildLogger(Log);

        private IPackTaskLogic _packTaskLogic;

        /// <summary>
        /// This property is only used for testing.
        /// </summary>
        public IPackTaskLogic PackTaskLogic
        {
            get
            {
                if (_packTaskLogic == null)
                {
                    _packTaskLogic = new PackTaskLogic();
                }

                return _packTaskLogic;
            }

            set
            {
                _packTaskLogic = value;
            }
        }

        public override bool Execute()
        {
            try
            {
#if DEBUG
                var debugPackTask = Environment.GetEnvironmentVariable("DEBUG_PACK_TASK");
                if (!string.IsNullOrEmpty(debugPackTask) && debugPackTask.Equals(bool.TrueString, StringComparison.OrdinalIgnoreCase))
                {
                    Debugger.Launch();
                }
#endif

                var request = GetRequest();
                var logic = PackTaskLogic;
                PackageBuilder packageBuilder = null;

                // If packing using a Nuspec file, we don't need to build a PackageBuilder here
                // as the package builder is built by reading the manifest file later in the code path.
                // Passing a null package builder for nuspec file code path is perfectly valid.
                if (string.IsNullOrEmpty(request.NuspecFile))
                {
                    packageBuilder = logic.GetPackageBuilder(request);
                }

                PackArgs packArgs = logic.GetPackArgs(request);
                var packRunner = logic.GetPackCommandRunner(request, packArgs, packageBuilder);

                return logic.BuildPackage(packRunner);
            }
            catch (Exception ex)
            {
                ExceptionUtilities.LogException(ex, Logger);
                return false;
            }

        }

        /// <summary>
        /// This method does two important things:
        /// 1. Normalizes string parameters, trimming whitespace and coalescing empty strings to null.
        /// 2. Wrap <see cref="ITaskItem"/> instances to facility unit testing.
        /// </summary>
        private IPackTaskRequest<IMSBuildItem> GetRequest()
        {
            return new PackTaskRequest
            {
                AllowedOutputExtensionsInPackageBuildOutputFolder = MSBuildStringUtility.TrimAndExcludeNullOrEmpty(AllowedOutputExtensionsInPackageBuildOutputFolder),
                AllowedOutputExtensionsInSymbolsPackageBuildOutputFolder = MSBuildStringUtility.TrimAndExcludeNullOrEmpty(AllowedOutputExtensionsInSymbolsPackageBuildOutputFolder),
                AssemblyName = MSBuildStringUtility.TrimAndGetNullForEmpty(AssemblyName),
                Authors = MSBuildStringUtility.TrimAndExcludeNullOrEmpty(Authors),
                BuildOutputInPackage = MSBuildUtility.WrapMSBuildItem(BuildOutputInPackage),
                BuildOutputFolders = MSBuildStringUtility.TrimAndExcludeNullOrEmpty(BuildOutputFolders),
                ContinuePackingAfterGeneratingNuspec = ContinuePackingAfterGeneratingNuspec,
                ContentTargetFolders = MSBuildStringUtility.TrimAndExcludeNullOrEmpty(ContentTargetFolders),
                Copyright = MSBuildStringUtility.TrimAndGetNullForEmpty(Copyright),
                Description = MSBuildStringUtility.TrimAndGetNullForEmpty(Description),
                DevelopmentDependency = DevelopmentDependency,
                FrameworkAssemblyReferences = MSBuildUtility.WrapMSBuildItem(FrameworkAssemblyReferences),
                FrameworksWithSuppressedDependencies = MSBuildUtility.WrapMSBuildItem(FrameworksWithSuppressedDependencies),
                IconUrl = MSBuildStringUtility.TrimAndGetNullForEmpty(IconUrl),
                IncludeBuildOutput = IncludeBuildOutput,
                IncludeSource = IncludeSource,
                IncludeSymbols = IncludeSymbols,
                InstallPackageToOutputPath = InstallPackageToOutputPath,
                IsTool = IsTool,
                LicenseUrl = MSBuildStringUtility.TrimAndGetNullForEmpty(LicenseUrl),
                Logger = Logger,
                MinClientVersion = MSBuildStringUtility.TrimAndGetNullForEmpty(MinClientVersion),
                NoDefaultExcludes = NoDefaultExcludes,
                NoPackageAnalysis = NoPackageAnalysis,
                NuspecBasePath = MSBuildStringUtility.TrimAndGetNullForEmpty(NuspecBasePath),
                NuspecFile = MSBuildStringUtility.TrimAndGetNullForEmpty(NuspecFile),
                NuspecOutputPath = MSBuildStringUtility.TrimAndGetNullForEmpty(NuspecOutputPath),
                NuspecProperties = MSBuildStringUtility.TrimAndExcludeNullOrEmpty(NuspecProperties),
                OutputFileNamesWithoutVersion = OutputFileNamesWithoutVersion,
                PackageFiles = MSBuildUtility.WrapMSBuildItem(PackageFiles),
                PackageFilesToExclude = MSBuildUtility.WrapMSBuildItem(PackageFilesToExclude),
                PackageId = MSBuildStringUtility.TrimAndGetNullForEmpty(PackageId),
                PackageOutputPath = MSBuildStringUtility.TrimAndGetNullForEmpty(PackageOutputPath),
                PackageTypes = MSBuildStringUtility.TrimAndExcludeNullOrEmpty(PackageTypes),
                PackageVersion = MSBuildStringUtility.TrimAndGetNullForEmpty(PackageVersion),
                PackItem = MSBuildUtility.WrapMSBuildItem(PackItem),
                ProjectReferencesWithVersions = MSBuildUtility.WrapMSBuildItem(ProjectReferencesWithVersions),
                ProjectUrl = MSBuildStringUtility.TrimAndGetNullForEmpty(ProjectUrl),
                ReleaseNotes = MSBuildStringUtility.TrimAndGetNullForEmpty(ReleaseNotes),
                RepositoryType = MSBuildStringUtility.TrimAndGetNullForEmpty(RepositoryType),
                RepositoryUrl = MSBuildStringUtility.TrimAndGetNullForEmpty(RepositoryUrl),
                RepositoryBranch = MSBuildStringUtility.TrimAndGetNullForEmpty(RepositoryBranch),
                RepositoryCommit = MSBuildStringUtility.TrimAndGetNullForEmpty(RepositoryCommit),
                RequireLicenseAcceptance = RequireLicenseAcceptance,
                RestoreOutputPath = MSBuildStringUtility.TrimAndGetNullForEmpty(RestoreOutputPath),
                Serviceable = Serviceable,
                SourceFiles = MSBuildUtility.WrapMSBuildItem(SourceFiles),
                SymbolPackageFormat = MSBuildStringUtility.TrimAndGetNullForEmpty(SymbolPackageFormat),
                Tags = MSBuildStringUtility.TrimAndExcludeNullOrEmpty(Tags),
                TargetFrameworks = MSBuildStringUtility.TrimAndExcludeNullOrEmpty(TargetFrameworks),
                TargetPathsToSymbols = MSBuildUtility.WrapMSBuildItem(TargetPathsToSymbols),
                Title = MSBuildStringUtility.TrimAndGetNullForEmpty(Title),
                TreatWarningsAsErrors = MSBuildStringUtility.TrimAndGetNullForEmpty(TreatWarningsAsErrors),
                NoWarn = MSBuildStringUtility.TrimAndGetNullForEmpty(NoWarn),
                WarningsAsErrors = MSBuildStringUtility.TrimAndGetNullForEmpty(WarningsAsErrors),
                WarningsNotAsErrors = MSBuildStringUtility.TrimAndGetNullForEmpty(WarningsNotAsErrors),
                PackageLicenseExpression = MSBuildStringUtility.TrimAndGetNullForEmpty(PackageLicenseExpression),
                PackageLicenseFile = MSBuildStringUtility.TrimAndGetNullForEmpty(PackageLicenseFile),
                PackageLicenseExpressionVersion = MSBuildStringUtility.TrimAndGetNullForEmpty(PackageLicenseExpressionVersion),
                Readme = MSBuildStringUtility.TrimAndGetNullForEmpty(Readme),
                Deterministic = Deterministic,
                PackageIcon = MSBuildStringUtility.TrimAndGetNullForEmpty(PackageIcon),
            };
        }
    }
}
