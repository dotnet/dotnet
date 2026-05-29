// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using SbrpUtilities;
using Xunit;
using Xunit.Abstractions;

namespace SbrpTests;

public class ExternalPackageTests
{
    private static readonly string RepoRoot =
        PathUtilities.GetRepoRoot().TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

    private static readonly string ProjectsDir =
        Path.Combine(RepoRoot, "src", "externalPackages", "projects");

    private static readonly string VersionsPropsPath =
        Path.Combine(RepoRoot, "eng", "Versions.props");

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
        string submodulesDir = Path.Combine(RepoRoot, "src", "externalPackages", "src");

        List<string> errors = new();
        int checkedCount = 0;

        foreach ((string projFile, string packageName, XDocument doc) in LoadProjectFiles())
        {
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
        AssertNoErrors(errors, "SourceRevisionId");
    }

    /// <summary>
    /// Validates that the FileVersionRevision property in external package .proj files
    /// matches the actual FileVersion revision from the Microsoft-shipped NuGet package.
    /// The test uses the FileVersionValidationPackage property to identify the package,
    /// downloads it from NuGet, extracts a DLL, reads its FileVersion, and compares the
    /// revision component.
    /// See https://github.com/dotnet/source-build/issues/5509
    /// </summary>
    [SkippableFact]
    public async Task FileVersionRevisionMatchesPublishedPackage()
    {
        await ValidateVersionOverrideAsync(
            "FileVersionRevision",
            metadata => metadata.Revision?.ToString());
    }

    /// <summary>
    /// Validates that the AssemblyVersionOverride property in external package .proj files matches
    /// the actual AssemblyVersion from the Microsoft-shipped NuGet package. The test discovers
    /// components that declare this override along with a FileVersionValidationPackage, downloads
    /// the published package from NuGet, and compares the embedded AssemblyVersion.
    /// </summary>
    [SkippableFact]
    public async Task AssemblyVersionOverrideMatchesPublishedPackage()
    {
        await ValidateVersionOverrideAsync(
            "AssemblyVersionOverride",
            metadata => metadata.AssemblyVersion);
    }

    /// <summary>
    /// Validates that the InformationalVersionOverride property in external package .proj files matches
    /// the actual InformationalVersion (ProductVersion) from the Microsoft-shipped NuGet package. The
    /// test discovers components that declare this override along with a FileVersionValidationPackage,
    /// downloads the published package from NuGet, and compares the embedded InformationalVersion.
    /// </summary>
    [SkippableFact]
    public async Task InformationalVersionOverrideMatchesPublishedPackage()
    {
        await ValidateVersionOverrideAsync(
            "InformationalVersionOverride",
            metadata => metadata.InformationalVersion);
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
        string artifactsObjDir = Path.Combine(RepoRoot, "artifacts", "obj");

        List<string> errors = new();
        int checkedCount = 0;

        foreach ((string projFile, string packageName, XDocument doc) in LoadProjectFiles())
        {
            string? releaseVersion = CommonUtilities.FindReleaseVersion(VersionsPropsPath, packageName);

            if (string.IsNullOrEmpty(releaseVersion))
            {
                continue;
            }

            // Scan the component's own build output directory for .nupkg files
            string componentObjDir = Path.Combine(artifactsObjDir, packageName);
            if (!Directory.Exists(componentObjDir))
            {
                Output.WriteLine($"Skipping {packageName}: build output directory not found ({componentObjDir}).");
                continue;
            }

            string[] componentNupkgs = Directory.GetFiles(componentObjDir, "*.nupkg", SearchOption.AllDirectories);
            if (componentNupkgs.Length == 0)
            {
                Output.WriteLine($"Skipping {packageName}: no .nupkg files found in build output.");
                continue;
            }

            string versionSuffix = $".{releaseVersion}.nupkg";
            bool foundMatch = componentNupkgs.Any(f =>
                Path.GetFileName(f).EndsWith(versionSuffix, StringComparison.OrdinalIgnoreCase));

            if (!foundMatch)
            {
                string foundPackages = string.Join(", ", componentNupkgs.Select(Path.GetFileName));
                errors.Add($"{packageName}: Expected package version {releaseVersion} but found: {foundPackages}");
            }

            checkedCount++;
        }

        Skip.If(checkedCount == 0, "No components with release versions had build output to validate.");
        AssertNoErrors(errors, "Release version");
    }

    /// <summary>
    /// Loads all .proj files from the external packages projects directory,
    /// returning the file path, component name, and parsed XML document.
    /// </summary>
    private static IEnumerable<(string ProjFile, string PackageName, XDocument Doc)> LoadProjectFiles()
    {
        string[] projFiles = Directory.GetFiles(ProjectsDir, "*.proj");
        Assert.True(projFiles.Length > 0, $"No .proj files found in {ProjectsDir}");

        foreach (string projFile in projFiles)
        {
            string packageName = Path.GetFileNameWithoutExtension(projFile);
            XDocument doc = XDocument.Load(projFile);
            yield return (projFile, packageName, doc);
        }
    }

    /// <summary>
    /// Looks up the release version for a component. If not found, adds an error and increments
    /// the checked count. Returns null when the caller should skip this component.
    /// </summary>
    private static string? FindReleaseVersionOrError(string packageName, List<string> errors)
    {
        string? releaseVersion = CommonUtilities.FindReleaseVersion(VersionsPropsPath, packageName);
        if (string.IsNullOrEmpty(releaseVersion))
        {
            errors.Add($"{packageName}.proj: No matching release version property found in eng/Versions.props.");
            return null;
        }

        return releaseVersion;
    }

    /// <summary>
    /// Validates that a version override MSBuild property in external package .proj files matches
    /// the corresponding value from the Microsoft-shipped NuGet package.
    /// </summary>
    private async Task ValidateVersionOverrideAsync(
        string overridePropertyName,
        Func<PackageVersionMetadata, string?> actualValueSelector)
    {
        List<string> errors = new();
        int checkedCount = 0;

        foreach ((string projFile, string packageName, XDocument doc) in LoadProjectFiles())
        {
            string? expectedValue = doc.Descendants(overridePropertyName).FirstOrDefault()?.Value;
            if (expectedValue is null)
            {
                continue;
            }

            string? packageId = doc.Descendants("FileVersionValidationPackage").FirstOrDefault()?.Value;
            if (string.IsNullOrEmpty(packageId))
            {
                errors.Add($"{packageName}.proj: Has {overridePropertyName} but missing FileVersionValidationPackage.");
                checkedCount++;
                continue;
            }

            string? releaseVersion = FindReleaseVersionOrError(packageName, errors);
            if (releaseVersion is null)
            {
                checkedCount++;
                continue;
            }

            var versionMetadata = await CommonUtilities.GetPackageVersionMetadataAsync(
                RepoRoot, packageId, releaseVersion);

            string? actualValue = actualValueSelector(versionMetadata);
            if (string.IsNullOrEmpty(actualValue))
            {
                Output.WriteLine($"Skipping {packageName}: unable to read {overridePropertyName} from {packageId} {releaseVersion}.");
                continue;
            }

            if (expectedValue != actualValue)
            {
                errors.Add($"{packageName}.proj: {overridePropertyName} '{expectedValue}' does not match " +
                    $"actual '{actualValue}' from {packageId} {releaseVersion}.");
            }

            checkedCount++;
        }

        Skip.If(checkedCount == 0, $"No components with {overridePropertyName} were found to validate.");
        AssertNoErrors(errors, overridePropertyName);
    }

    private static void AssertNoErrors(List<string> errors, string validationName)
    {
        Assert.True(errors.Count == 0,
            $"{validationName} validation failed:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}");
    }
}
