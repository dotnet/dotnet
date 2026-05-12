// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using NuGet.Packaging;
using SbrpUtilities;
using Xunit;
using Xunit.Abstractions;

namespace SbrpTests;

public class ExternalPackageTests
{
    public ITestOutputHelper Output { get; set; }

    public ExternalPackageTests(ITestOutputHelper output)
    {
        Output = output;
    }

    /// <summary>
    /// Validates that the SourceRevisionId property in each external package .proj file
    /// matches the commit hash of the corresponding git submodule. This ensures the correct
    /// repo commit hash is embedded in the built packages rather than the VMR's commit hash.
    /// See https://github.com/dotnet/source-build/issues/5506
    /// </summary>
    [Fact]
    public void SourceRevisionIdMatchesSubmoduleCommit()
    {
        string repoRoot = PathUtilities.GetRepoRoot().TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        string projectsDir = Path.Combine(repoRoot, "src", "externalPackages", "projects");
        string submodulesDir = Path.Combine(repoRoot, "src", "externalPackages", "src");

        string[] projFiles = Directory.GetFiles(projectsDir, "*.proj");
        Assert.True(projFiles.Length > 0, $"No .proj files found in {projectsDir}");

        List<string> errors = new();
        int checkedCount = 0;

        foreach (string projFile in projFiles)
        {
            string packageName = Path.GetFileNameWithoutExtension(projFile);
            string submoduleDir = Path.Combine(submodulesDir, packageName);

            // Skip packages that don't have a corresponding submodule
            if (!Directory.Exists(submoduleDir) || !Directory.EnumerateFileSystemEntries(submoduleDir).Any())
            {
                Output.WriteLine($"Skipping {packageName}: submodule directory not found or empty.");
                continue;
            }

            // Get the submodule's commit hash
            var result = ExecuteHelper.ExecuteProcess("git", $"-C {submoduleDir} rev-parse HEAD", Output);
            if (result.Process.ExitCode != 0)
            {
                Output.WriteLine($"Skipping {packageName}: unable to resolve submodule commit hash.");
                continue;
            }

            string submoduleHash = result.StdOut.Trim();

            // Parse SourceRevisionId from the .proj file
            XDocument doc = XDocument.Load(projFile);
            string? sourceRevisionId = doc.Descendants("SourceRevisionId").FirstOrDefault()?.Value;

            if (string.IsNullOrEmpty(sourceRevisionId))
            {
                errors.Add($"{packageName}.proj: Missing SourceRevisionId property. Expected '{submoduleHash}'.");
            }
            else if (!string.Equals(submoduleHash, sourceRevisionId, StringComparison.OrdinalIgnoreCase))
            {
                errors.Add($"{packageName}.proj: SourceRevisionId '{sourceRevisionId}' does not match submodule commit '{submoduleHash}'.");
            }

            checkedCount++;
        }

        Assert.True(checkedCount > 0, "No external packages with initialized submodules were found to validate.");
        Assert.True(errors.Count == 0,
            $"SourceRevisionId validation failed:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}");
    }

    /// <summary>
    /// Validates that the FileVersionRevision property in external package .proj files
    /// matches the actual FileVersion revision from the Microsoft-shipped NuGet package.
    /// The test discovers the package ID from the component's build output directory
    /// (artifacts/obj/{component}/), downloads the same package from NuGet, extracts a DLL,
    /// reads its FileVersion, and compares the revision component.
    /// See https://github.com/dotnet/source-build/issues/5509
    /// </summary>
    [SkippableFact]
    public async Task FileVersionRevisionMatchesPublishedPackage()
    {
        string repoRoot = PathUtilities.GetRepoRoot().TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        string projectsDir = Path.Combine(repoRoot, "src", "externalPackages", "projects");
        string versionsPropsPath = Path.Combine(repoRoot, "eng", "Versions.props");
        string artifactsObjDir = Path.Combine(repoRoot, "artifacts", "obj");

        string[] projFiles = Directory.GetFiles(projectsDir, "*.proj");
        Assert.True(projFiles.Length > 0, $"No .proj files found in {projectsDir}");

        List<string> errors = new();
        int checkedCount = 0;

        foreach (string projFile in projFiles)
        {
            string componentName = Path.GetFileNameWithoutExtension(projFile);
            XDocument doc = XDocument.Load(projFile);
            string? fileVersionRevision = doc.Descendants("FileVersionRevision").FirstOrDefault()?.Value;

            if (fileVersionRevision is null)
            {
                continue;
            }

            // Find matching release version in eng/Versions.props
            string? releaseVersion = CommonUtilities.FindReleaseVersion(versionsPropsPath, componentName);

            if (string.IsNullOrEmpty(releaseVersion))
            {
                errors.Add($"{componentName}.proj: No matching release version property found in eng/Versions.props.");
                checkedCount++;
                continue;
            }

            // Find a .nupkg produced by this specific component
            string componentObjDir = Path.Combine(artifactsObjDir, componentName);
            if (!Directory.Exists(componentObjDir))
            {
                Output.WriteLine($"Skipping {componentName}: build output directory not found ({componentObjDir}).");
                continue;
            }

            string[] componentNupkgs = Directory.GetFiles(componentObjDir, "*.nupkg", SearchOption.AllDirectories);

            // Find the first package that contains a DLL so we can read its FileVersion
            string? packageId = null;
            foreach (string nupkg in componentNupkgs)
            {
                using PackageArchiveReader reader = new(nupkg);
                bool hasDll = reader.GetLibItems()
                    .SelectMany(group => group.Items)
                    .Any(item => item.EndsWith(".dll", StringComparison.OrdinalIgnoreCase));
                if (hasDll)
                {
                    packageId = reader.NuspecReader.GetId();
                    break;
                }
            }

            if (packageId is null)
            {
                Output.WriteLine($"Skipping {componentName}: no .nupkg with a DLL found in build output.");
                continue;
            }

            // Download the published package and read the FileVersion revision
            var (revision, fileVersion) = await CommonUtilities.GetFileVersionRevisionAsync(
                repoRoot, packageId, releaseVersion);

            if (revision is null)
            {
                errors.Add($"{componentName}.proj: Unable to download {packageId} {releaseVersion} to validate FileVersionRevision.");
                checkedCount++;
                continue;
            }

            if (!int.TryParse(fileVersionRevision, out int expectedRevision) || expectedRevision != revision.Value)
            {
                errors.Add($"{componentName}.proj: FileVersionRevision '{fileVersionRevision}' does not match " +
                    $"actual revision '{revision}' from {packageId} {releaseVersion} " +
                    $"(FileVersion: {fileVersion}).");
            }

            checkedCount++;
        }

        Skip.If(checkedCount == 0, "No components with FileVersionRevision had build output to validate.");
        Assert.True(errors.Count == 0,
            $"FileVersionRevision validation failed:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}");
    }

    /// <summary>
    /// Validates that the release version configured in eng/Versions.props for each external
    /// component matches the version of at least one NuGet package produced by that component.
    /// The test scans each component's build output directory (artifacts/obj/{component}/) for
    /// .nupkg files and verifies at least one has the expected version.
    /// This catches cases where a submodule is updated but the release version is not.
    /// </summary>
    [SkippableFact]
    public void ReleaseVersionMatchesPackageOutput()
    {
        string repoRoot = PathUtilities.GetRepoRoot().TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        string projectsDir = Path.Combine(repoRoot, "src", "externalPackages", "projects");
        string versionsPropsPath = Path.Combine(repoRoot, "eng", "Versions.props");
        string artifactsObjDir = Path.Combine(repoRoot, "artifacts", "obj");

        string[] projFiles = Directory.GetFiles(projectsDir, "*.proj");
        Assert.True(projFiles.Length > 0, $"No .proj files found in {projectsDir}");

        List<string> errors = new();
        int checkedCount = 0;

        foreach (string projFile in projFiles)
        {
            string componentName = Path.GetFileNameWithoutExtension(projFile);
            string? releaseVersion = CommonUtilities.FindReleaseVersion(versionsPropsPath, componentName);

            if (string.IsNullOrEmpty(releaseVersion))
            {
                continue;
            }

            // Scan the component's own build output directory for .nupkg files
            string componentObjDir = Path.Combine(artifactsObjDir, componentName);
            if (!Directory.Exists(componentObjDir))
            {
                Output.WriteLine($"Skipping {componentName}: build output directory not found ({componentObjDir}).");
                continue;
            }

            string[] componentNupkgs = Directory.GetFiles(componentObjDir, "*.nupkg", SearchOption.AllDirectories);
            if (componentNupkgs.Length == 0)
            {
                Output.WriteLine($"Skipping {componentName}: no .nupkg files found in build output.");
                continue;
            }

            string versionSuffix = $".{releaseVersion}.nupkg";
            bool foundMatch = componentNupkgs.Any(f =>
                Path.GetFileName(f).EndsWith(versionSuffix, StringComparison.OrdinalIgnoreCase));

            if (!foundMatch)
            {
                string foundPackages = string.Join(", ", componentNupkgs.Select(Path.GetFileName));
                errors.Add($"{componentName}: Expected package version {releaseVersion} but found: {foundPackages}");
            }

            checkedCount++;
        }

        Skip.If(checkedCount == 0, "No components with release versions had build output to validate.");
        Assert.True(errors.Count == 0,
            $"Release version validation failed:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}");
    }
}
