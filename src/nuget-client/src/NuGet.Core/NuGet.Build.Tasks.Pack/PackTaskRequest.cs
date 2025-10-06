// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Commands;
using NuGet.Common;

namespace NuGet.Build.Tasks.Pack
{
    public class PackTaskRequest : IPackTaskRequest<IMSBuildItem>
    {
        public string[] AllowedOutputExtensionsInPackageBuildOutputFolder { get; set; }
        public string[] AllowedOutputExtensionsInSymbolsPackageBuildOutputFolder { get; set; }
        public string AssemblyName { get; set; }
        public string[] Authors { get; set; }
        public IMSBuildItem[] BuildOutputInPackage { get; set; }
        public string[] BuildOutputFolders { get; set; }
        public string[] ContentTargetFolders { get; set; }
        public bool ContinuePackingAfterGeneratingNuspec { get; set; }
        public string Copyright { get; set; }
        public string Description { get; set; }
        public bool DevelopmentDependency { get; set; }
        public IMSBuildItem[] FrameworkAssemblyReferences { get; set; }
        public string IconUrl { get; set; }
        public bool IncludeBuildOutput { get; set; }
        public bool IncludeSource { get; set; }
        public bool IncludeSymbols { get; set; }
        public bool InstallPackageToOutputPath { get; set; }
        public bool IsTool { get; set; }
        public string LicenseUrl { get; set; }
        public ILogger Logger { get; set; }
        public string MinClientVersion { get; set; }
        public bool NoDefaultExcludes { get; set; }
        public bool NoPackageAnalysis { get; set; }
        public string NuspecFile { get; set; }
        public string NuspecOutputPath { get; set; }
        public IMSBuildItem[] PackageFiles { get; set; }
        public IMSBuildItem[] PackageFilesToExclude { get; set; }
        public string PackageId { get; set; }
        public string PackageOutputPath { get; set; }
        public string[] PackageTypes { get; set; }
        public string PackageVersion { get; set; }
        public IMSBuildItem PackItem { get; set; }
        public IMSBuildItem[] ProjectReferencesWithVersions { get; set; }
        public string ProjectUrl { get; set; }
        public string NuspecBasePath { get; set; }
        public string[] NuspecProperties { get; set; }
        public bool OutputFileNamesWithoutVersion { get; set; }
        public string Readme { get; set; }
        public string ReleaseNotes { get; set; }
        public string RepositoryType { get; set; }
        public string RepositoryUrl { get; set; }
        public string RepositoryBranch { get; set; }
        public string RepositoryCommit { get; set; }
        public bool RequireLicenseAcceptance { get; set; }
        public string RestoreOutputPath { get; set; }
        public bool Serviceable { get; set; }
        public IMSBuildItem[] SourceFiles { get; set; }
        public string[] Tags { get; set; }
        public string[] TargetFrameworks { get; set; }
        public IMSBuildItem[] TargetPathsToSymbols { get; set; }
        public string Title { get; set; }
        public string NoWarn { get; set; }
        public string TreatWarningsAsErrors { get; set; }
        public string WarningsAsErrors { get; set; }
        public string WarningsNotAsErrors { get; set; }
        public IMSBuildItem[] FrameworksWithSuppressedDependencies { get; set; }
        public string SymbolPackageFormat { get; set; }
        public string PackageLicenseFile { get; set; }
        public string PackageLicenseExpression { get; set; }
        public string PackageLicenseExpressionVersion { get; set; }
        public bool Deterministic { get; set; }
        public string PackageIcon { get; set; }
    }
}
