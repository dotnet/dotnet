// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Build.Framework;
using NuGet.Commands;
using ILogger = NuGet.Common.ILogger;

namespace NuGet.Build.Tasks.Pack
{
    /// <summary>
    /// All of the properties provided by MSBuild to execute pack.
    /// </summary>
    /// <typeparam name="TItem">
    /// The item type. This will either be <see cref="ITaskItem"/> or a <see cref="IMSBuildItem"/>.
    /// </typeparam>
    public interface IPackTaskRequest<TItem>
    {
        string[] AllowedOutputExtensionsInPackageBuildOutputFolder { get; }
        string[] AllowedOutputExtensionsInSymbolsPackageBuildOutputFolder { get; }
        string AssemblyName { get; }
        string[] Authors { get; }
        TItem[] BuildOutputInPackage { get; }
        string[] BuildOutputFolders { get; }
        string[] ContentTargetFolders { get; }
        bool ContinuePackingAfterGeneratingNuspec { get; }
        string Copyright { get; }
        string Description { get; }
        bool DevelopmentDependency { get; }
        TItem[] FrameworkAssemblyReferences { get; }
        TItem[] FrameworksWithSuppressedDependencies { get; }
        string IconUrl { get; }
        bool IncludeBuildOutput { get; }
        bool IncludeSource { get; }
        bool IncludeSymbols { get; }
        bool InstallPackageToOutputPath { get; }
        bool IsTool { get; }
        string LicenseUrl { get; }
        ILogger Logger { get; }
        string MinClientVersion { get; }
        bool NoDefaultExcludes { get; }
        bool NoPackageAnalysis { get; }
        string NoWarn { get; }
        string NuspecBasePath { get; }
        string NuspecFile { get; }
        string[] NuspecProperties { get; }
        string NuspecOutputPath { get; }
        bool OutputFileNamesWithoutVersion { get; }
        TItem[] PackageFiles { get; }
        TItem[] PackageFilesToExclude { get; }
        string PackageId { get; }
        string PackageOutputPath { get; }
        string[] PackageTypes { get; }
        string PackageVersion { get; }
        TItem PackItem { get; }
        TItem[] ProjectReferencesWithVersions { get; }
        string ProjectUrl { get; }
        string ReleaseNotes { get; }
        string RepositoryType { get; }
        string RepositoryUrl { get; }
        string RepositoryBranch { get; }
        string RepositoryCommit { get; }
        bool RequireLicenseAcceptance { get; }
        string RestoreOutputPath { get; }
        bool Serviceable { get; }
        TItem[] SourceFiles { get; }
        string SymbolPackageFormat { get; }
        string[] Tags { get; }
        string[] TargetFrameworks { get; }
        TItem[] TargetPathsToSymbols { get; }
        string Title { get; }
        string TreatWarningsAsErrors { get; }
        string WarningsAsErrors { get; }
        string WarningsNotAsErrors { get; }
        string PackageLicenseExpression { get; }
        string PackageLicenseExpressionVersion { get; }
        string PackageLicenseFile { get; }
        string Readme { get; }
        bool Deterministic { get; }
        string PackageIcon { get; }
    }
}
