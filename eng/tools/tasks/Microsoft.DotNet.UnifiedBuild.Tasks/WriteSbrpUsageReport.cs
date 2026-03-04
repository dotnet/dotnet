// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet.LibraryModel;
using NuGet.ProjectModel;

namespace Microsoft.DotNet.UnifiedBuild.Tasks;

/// <summary>
/// Reports the usage of the source-build-reference-packages:
/// 1. SBRP references
/// 2. Unreferenced packages
/// </summary>
public partial class WriteSbrpUsageReport : Task
{
    private const string SbrpRepoName = "source-build-reference-packages";
    private const string ErrorCode = "SBRP001";

    private readonly Dictionary<string, PackageInfo> _sbrpPackages = [];

    /// <summary>
    /// Path to the SBRP src directory.
    /// </summary>
    [Required]
    public required string SbrpRepoSrcPath { get; set; }

    /// <summary>
    /// Directory path containing the built SBRP packages.
    /// </summary>
    [Required]
    public required string SbrpPackagesPath { get; set; }

    /// <summary>
    /// Paths to the project.assets.json files produced by the build.
    ///
    /// %(Identity): project.assets.json file path.
    /// </summary>
    [Required]
    public required ITaskItem[] ProjectAssetsJsons { get; set; }

    /// <summary>
    /// Path to the usage report to.
    /// </summary>
    [Required]
    public required string OutputPath { get; set; }

    public override bool Execute()
    {
        try
        {
            Log.LogMessage($"Scanning for SBRP Package Usage...");

            ReadSbrpPackages(Path.Combine("referencePackages", "src"), trackTfms: true);
            ReadSbrpPackages(Path.Combine("targetPacks", "ILsrc"), trackTfms: false);
            ReadSbrpPackages(Path.Combine("textOnlyPackages", "src"), trackTfms: false);
            ReadExternalPackages(Path.Combine("externalPackages", "src"));

            ScanProjectReferences();

            GenerateUsageReport();
        }
        catch (Exception ex)
        {
            LogError($"Failed to generate SBRP usage report: {ex}");
        }

        return !Log.HasLoggedErrors;
    }

    private void GenerateUsageReport()
    {
        PackageInfo[] existingSbrps = [.. _sbrpPackages.Values.OrderBy(pkg => pkg.Id)];
        PurgeNonReferencedReferences();
        IEnumerable<string> unreferencedSbrps = GetUnreferencedSbrps().Select(pkg => pkg.Path).OrderBy(id => id);

        if (unreferencedSbrps.Count() == existingSbrps.Length)
        {
            LogError("No SBRP packages are detected as being referenced.");
        }

        Report report = new(existingSbrps, unreferencedSbrps);

        string reportFilePath = Path.Combine(OutputPath, "sbrpPackageUsage.json");
#pragma warning disable CA1869 // Cache and reuse 'JsonSerializerOptions' instances
        string jsonContent = JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true });
#pragma warning restore CA1869 // Cache and reuse 'JsonSerializerOptions' instances
        File.WriteAllText(reportFilePath, jsonContent);
    }

    /// <summary>
    /// Removes all references from unreferenced SBRP packages. This is necessary to determine the
    /// complete set of unreferenced SBRP packages.
    /// </summary>
    private void PurgeNonReferencedReferences()
    {
        bool hasPurged;
        do
        {
            hasPurged = false;
            PackageInfo[] unrefPkgs = GetUnreferencedSbrps().ToArray();

            foreach (PackageInfo sbrpPkg in _sbrpPackages.Values)
            {
                foreach (PackageInfo unrefPkg in unrefPkgs)
                {
                    var unref = sbrpPkg.References.Keys
                        .SingleOrDefault(path => path.Contains(SbrpRepoName) && path.Contains($"{unrefPkg.Name}.{unrefPkg.Version}"));
                    if (unref != null)
                    {
                        Log.LogMessage($"Removing {unrefPkg.Id} from {sbrpPkg.Id}'s references.");
                        sbrpPkg.References.Remove(unref);
                        hasPurged = true;
                    }
                }
            }
        } while (hasPurged);
    }

    private IEnumerable<PackageInfo> GetUnreferencedSbrps() =>
        _sbrpPackages.Values.Where(pkg => pkg.References.Count == 0);

    /// <summary>
    /// Extracts package name and version from a .nupkg file by reading its .nuspec metadata.
    /// </summary>
    /// <param name="nupkgFilePath">Path to the .nupkg file</param>
    /// <returns>A tuple containing the package name and version, or (null, null) if parsing failed</returns>
    private (string? Name, string? Version) GetPackageInfoFromNupkg(string nupkgFilePath)
    {
        try
        {
            using var fileStream = File.OpenRead(nupkgFilePath);
            using var archive = new ZipArchive(fileStream, ZipArchiveMode.Read);
            var nuspecEntry = archive.Entries.FirstOrDefault(e => e.Name.EndsWith(".nuspec"));
            
            if (nuspecEntry == null)
            {
                LogError($"Could not find .nuspec in {nupkgFilePath}");
                return (null, null);
            }

            using var stream = nuspecEntry.Open();
            var nuspecDoc = XDocument.Load(stream);
            var ns = nuspecDoc.Root?.GetDefaultNamespace() ?? XNamespace.None;
            
            string? packageName = nuspecDoc.Root?.Element(ns + "metadata")?.Element(ns + "id")?.Value;
            string? version = nuspecDoc.Root?.Element(ns + "metadata")?.Element(ns + "version")?.Value;

            if (string.IsNullOrEmpty(packageName) || string.IsNullOrEmpty(version))
            {
                LogError($"Could not parse package name and version from nuspec in {nupkgFilePath}");
                return (null, null);
            }

            return (packageName, version);
        }
        catch (Exception ex)
        {
            LogError($"Error processing {nupkgFilePath}: {ex.Message}");
            return (null, null);
        }
    }

    /// <summary>
    /// External packages cannot be discovered in the same way as the other packages, scanning the src for csprojs.
    /// Externals don't follow any convention and not all of the external src is even built.
    /// To discover external packages, the package output is scanned and every package that isn't another package
    /// type is assumed to be an external package.
    /// </summary>
    private void ReadExternalPackages(string packageSrcRelativePath)
    {
        string packageSrcPath = Path.Combine(SbrpRepoSrcPath, packageSrcRelativePath);

        foreach (string nupkgFile in Directory.GetFiles(SbrpPackagesPath, "*.nupkg", SearchOption.TopDirectoryOnly))
        {
            var (packageName, version) = GetPackageInfoFromNupkg(nupkgFile);
            
            if (packageName == null || version == null)
            {
                continue;
            }

            if (!_sbrpPackages.TryGetValue(PackageInfo.GetId(packageName, version), out PackageInfo? info))
            {
                // The exact source file path is not readily available so '**' is used to convey the approximate location.
                info = new(packageName, version, Path.Combine(packageSrcPath, "**", packageName, version));
                TrackSbrpPackage(info);
            }
        }
    }

    private void ReadSbrpPackages(string packageSrcRelativePath, bool trackTfms)
    {
        string packageSrcPath = Path.Combine(SbrpRepoSrcPath, packageSrcRelativePath);
        foreach (string projectPath in Directory.GetFiles(packageSrcPath, "*.csproj", SearchOption.AllDirectories))
        {
            DirectoryInfo? directory = Directory.GetParent(projectPath);
            string version = directory!.Name;
            string projectName = Path.GetFileNameWithoutExtension(projectPath);
            HashSet<string>? tfms = null;

            if (trackTfms)
            {
                XDocument xmlDoc = XDocument.Load(projectPath);
                // Reference packages are generated using the TargetFrameworks property
                // so there is no need to handle the TargetFramework property.
                tfms = xmlDoc.Element("Project")?
                    .Elements("PropertyGroup")
                    .Elements("TargetFrameworks")
                    .FirstOrDefault()?.Value?
                    .Split(';')
                    .ToHashSet();

                if (tfms == null || tfms.Count == 0)
                {
                    LogError($"No TargetFrameworks were detected in {projectPath}.");
                    continue;
                }
            }

            PackageInfo info = new(projectName[..(projectName.Length - 1 - version.Length)],
                version,
                directory.FullName,
                tfms);
            TrackSbrpPackage(info);
        }
    }

    private void ScanProjectReferences()
    {
        if (ProjectAssetsJsons.Length == 0)
        {
            LogError($"No project.assets.json files were specified.");
            return;
        }

        foreach (string projectJsonFile in ProjectAssetsJsons.Select(item => item.GetMetadata("Identity")))
        {
            try
            {
                ScanProjectAssetsFile(projectJsonFile);
            }
            catch (Exception ex)
            {
                LogError($"Error processing {projectJsonFile}: {ex.Message}");
            }
        }
    }

    private void ScanProjectAssetsFile(string projectJsonFile)
    {
        LockFile lockFile = new LockFileFormat().Read(projectJsonFile);
        foreach (LockFileTargetLibrary lib in lockFile.Targets.SelectMany(t => t.Libraries))
        {
            IEnumerable<string> tfms = lib.CompileTimeAssemblies
                .Where(asm => asm.Path.StartsWith("lib") || asm.Path.StartsWith("ref"))
                .Select(asm => asm.Path.Split('/')[1]);

            TrackPackageReference(lockFile.Path, lib.Name, lib.Version?.ToString(), tfms);
        }

        foreach (DownloadDependency downloadDep in lockFile.PackageSpec.TargetFrameworks.SelectMany(fx => fx.DownloadDependencies))
        {
            TrackPackageReference(lockFile.Path, downloadDep.Name, downloadDep.VersionRange.MinVersion?.ToString(), Enumerable.Empty<string>());
        }

        // Track framework references (e.g., Microsoft.NETCore.App, Microsoft.AspNetCore.App)
        // These correspond to targeting packs like Microsoft.NETCore.App.Ref.6.0.0
        foreach (var targetFramework in lockFile.PackageSpec.TargetFrameworks)
        {
            foreach (var frameworkRef in targetFramework.FrameworkReferences)
            {
                string? targetingPackVersion = InferTargetingPackVersion(targetFramework.FrameworkName.GetShortFolderName());
                if (targetingPackVersion != null)
                {
                    string targetingPackName = $"{frameworkRef.Name}.Ref";
                    TrackPackageReference(
                        lockFile.Path,
                        targetingPackName,
                        targetingPackVersion,
                        [ targetFramework.FrameworkName.GetShortFolderName() ]);
                }
            }
        }

        if (lockFile.PackageSpec.RestoreMetadata.ProjectPath.Contains(SbrpRepoName))
        {
            // For SBRP projects, we need to track the project references as well. While project references are included in the targets
            // which were processed above, only the resolved version is included in the cases when the dependency graph contains multiple
            // versions. All project references must be tracked as dependencies because they are required to build SBRP.
            foreach (ProjectRestoreMetadataFrameworkInfo targetFx in lockFile.PackageSpec.RestoreMetadata.TargetFrameworks)
            {
                foreach (ProjectRestoreReference projectRef in targetFx.ProjectReferences)
                {
                    if (projectRef.ProjectPath.Contains(SbrpRepoName))
                    {
                        string[] pathSegments = projectRef.ProjectPath.Split('/');
                        string projName = pathSegments[pathSegments.Length - 3];
                        string projVersion = pathSegments[pathSegments.Length - 2];
                        TrackPackageReference(lockFile.Path, projName, projVersion, new[] { targetFx.TargetAlias });
                    }
                    else
                    {
                        Log.LogMessage($"Unexpected non-SBRP project reference detected: {projectRef.ProjectPath}");
                    }
                }
            }
        }
    }

    // Match patterns like net6.0, net7.0, net8.0, net9.0, net10.0, etc.
    [GeneratedRegex(@"^net(\d+\.\d+)$")]
    private static partial Regex GetTargetingPackVersionRegex();

    /// <summary>
    /// Infers the targeting pack version from a target framework moniker.
    /// For source-build, we map net6.0 -> 6.0.0, net7.0 -> 7.0.0, etc.
    /// </summary>
    private static string? InferTargetingPackVersion(string tfm)
    {
        var match = GetTargetingPackVersionRegex().Match(tfm);
        if (match.Success)
        {
            return $"{match.Groups[1].Value}.0";
        }

        return null;
    }

    private void TrackPackageReference(string lockFilePath, string? name, string? version, IEnumerable<string> tfms)
    {
        string id = PackageInfo.GetId(name, version);
        if (!_sbrpPackages.TryGetValue(id, out PackageInfo? info))
        {
            return;
        }

        if (!info.References.TryGetValue(lockFilePath, out HashSet<string>? referencedTfms))
        {
            referencedTfms = [];
            info.References.Add(lockFilePath, referencedTfms);
        }

        foreach (string tfm in tfms)
        {
            referencedTfms!.Add(tfm);
        }
    }

    private void TrackSbrpPackage(PackageInfo info)
    {
        _sbrpPackages.Add(info.Id, info);
        Log.LogMessage($"Detected package: {info.Id}");
    }

    private void LogError(string message) =>
        Log.LogError(subcategory: null, errorCode: ErrorCode, helpKeyword: null,
            file: null, lineNumber: 0, columnNumber: 0, endLineNumber: 0, endColumnNumber: 0,
            message: message);

    private record PackageInfo(string Name, string Version, string Path, HashSet<string>? Tfms = default)
    {
        public string Id => GetId(Name, Version);

        // Dictionary of projects referencing the SBRP and the TFMs referenced by each project
        public Dictionary<string, HashSet<string>> References { get; } = [];

        public static string GetId(string? Name, string? Version) => $"{Name?.ToLowerInvariant()}/{Version}";
    }

    private record Report(IEnumerable<PackageInfo> Sbrps, IEnumerable<string> UnreferencedSbrps);
}
