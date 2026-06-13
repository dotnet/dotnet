// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Build.Evaluation;
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

    private static readonly string DefaultsPropsPath =
        Path.Combine(ProjectsDir, "validation.props");

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

        foreach (string projFile in Directory.GetFiles(ProjectsDir, "*.proj"))
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

            // SourceRevisionId is a literal value declared in each .proj's <PropertyGroup>;
            // read it directly via XDocument so this test doesn't depend on MSBuild evaluation
            // (which would need SDK resolution / MSBuildLocator and can't run inside dotnet test).
            string? sourceRevisionId = ReadSourceRevisionIdFromProj(projFile);

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

    private static string? ReadSourceRevisionIdFromProj(string projFile)
    {
        XDocument doc = XDocument.Load(projFile);
        return doc.Root?
            .Elements("PropertyGroup")
            .Elements("SourceRevisionId")
            .LastOrDefault()
            ?.Value;
    }

    /// <summary>
    /// Validates that the property named by each &lt;FileVersionValidationPackage&gt; item's
    /// <c>FileVersionRevisionProperty</c> metadata matches the actual FileVersion revision
    /// from the Microsoft-shipped NuGet package named by the item's Include attribute.
    /// Items that have no binding for this aspect do not participate in this check.
    /// See https://github.com/dotnet/source-build/issues/5509
    /// </summary>
    [SkippableFact]
    public async Task FileVersionRevisionMatchesPublishedPackage()
    {
        await ValidateVersionOverrideAsync(
            "FileVersionRevision",
            item => item.FileVersionRevision,
            metadata => metadata.Revision?.ToString());
    }

    /// <summary>
    /// Validates that the property named by each &lt;FileVersionValidationPackage&gt; item's
    /// <c>AssemblyVersionOverrideProperty</c> metadata matches the actual AssemblyVersion
    /// from the Microsoft-shipped NuGet package. Items that have no binding for this aspect
    /// do not participate in this check.
    /// </summary>
    [SkippableFact]
    public async Task AssemblyVersionOverrideMatchesPublishedPackage()
    {
        await ValidateVersionOverrideAsync(
            "AssemblyVersionOverride",
            item => item.AssemblyVersionOverride,
            metadata => metadata.AssemblyVersion);
    }

    /// <summary>
    /// Validates that the property named by each &lt;FileVersionValidationPackage&gt; item's
    /// <c>InformationalVersionOverrideProperty</c> metadata matches the actual InformationalVersion
    /// (ProductVersion) from the Microsoft-shipped NuGet package. Items that have no binding
    /// for this aspect do not participate in this check.
    /// </summary>
    [SkippableFact]
    public async Task InformationalVersionOverrideMatchesPublishedPackage()
    {
        await ValidateVersionOverrideAsync(
            "InformationalVersionOverride",
            item => item.InformationalVersionOverride,
            metadata => metadata.InformationalVersion);
    }

    /// <summary>
    /// Validates that each release version configured in eng/Versions.props produces a matching
    /// .nupkg in the component's build output directory. For projects with one or more
    /// &lt;FileVersionValidationPackage&gt; items, expects a .nupkg whose file name is
    /// <c>{PackageId}.{releaseVersion}.nupkg</c>. For projects without validation items, falls
    /// back to scanning for any .nupkg ending in <c>.{releaseVersion}.nupkg</c> using the
    /// auto-derived release version.
    /// This catches cases where a submodule is updated but the release version is not.
    /// </summary>
    [SkippableFact]
    public void ReleaseVersionMatchesPackageOutput()
    {
        string artifactsObjDir = Path.Combine(RepoRoot, "artifacts", "obj");
        using ProjectCollection versionsCollection = new();
        Project versionsProject = versionsCollection.LoadProject(VersionsPropsPath);

        List<string> errors = new();
        int checkedCount = 0;

        foreach ((_, string packageName, Project project) in LoadProjectFiles())
        {
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

            IReadOnlyList<ValidationPackageItem> items = CommonUtilities.ParseValidationPackageItems(project);

            if (items.Count == 0)
            {
                string? releaseVersion = CommonUtilities.FindReleaseVersion(versionsProject, packageName);
                if (string.IsNullOrEmpty(releaseVersion))
                {
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
                continue;
            }

            foreach (ValidationPackageItem item in items)
            {
                string? releaseVersion = ResolveReleaseVersion(versionsProject, item, packageName);
                if (string.IsNullOrEmpty(releaseVersion))
                {
                    errors.Add($"{packageName}.props: Could not resolve release version for validation package '{item.PackageId}' " +
                        $"({(item.ReleaseVersionPropertyName is null ? "auto-derived" : $"explicit ReleaseVersionProperty='{item.ReleaseVersionPropertyName}'")}).");
                    checkedCount++;
                    continue;
                }

                string expectedFileName = $"{item.PackageId}.{releaseVersion}.nupkg";
                bool found = componentNupkgs.Any(f =>
                    Path.GetFileName(f).Equals(expectedFileName, StringComparison.OrdinalIgnoreCase));

                if (!found)
                {
                    string foundPackages = string.Join(", ", componentNupkgs.Select(Path.GetFileName));
                    errors.Add($"{packageName}: Expected built package '{expectedFileName}' but found: {foundPackages}");
                }

                checkedCount++;
            }
        }

        Skip.If(checkedCount == 0, "No components with release versions had build output to validate.");
        AssertNoErrors(errors, "Release version");
    }

    /// <summary>
    /// Loads validation metadata for each external package component. Discovers components by
    /// listing the sibling .proj files (one per component) and then loading that component's
    /// <c>&lt;component&gt;.props</c> file when present. Components without a sibling .props
    /// have no validation overrides; the shared defaults file is loaded as a stand-in so the
    /// returned <see cref="Project"/> is non-null and yields zero
    /// <c>FileVersionValidationPackage</c> items via <see cref="CommonUtilities.ParseValidationPackageItems"/>.
    /// </summary>
    private static IEnumerable<(string ProjFile, string PackageName, Project Project)> LoadProjectFiles()
    {
        string[] projFiles = Directory.GetFiles(ProjectsDir, "*.proj");
        Assert.True(projFiles.Length > 0, $"No .proj files found in {ProjectsDir}");

        foreach (string projFile in projFiles)
        {
            string packageName = Path.GetFileNameWithoutExtension(projFile);
            string componentPropsFile = Path.Combine(ProjectsDir, $"{packageName}.props");
            string toLoad = File.Exists(componentPropsFile) ? componentPropsFile : DefaultsPropsPath;
            // 'using' inside the iterator: the ProjectCollection is disposed when the consumer
            // advances past this yield (or abandons the enumerator). Callers must not retain
            // the yielded Project beyond their iteration body.
            using ProjectCollection collection = new();
            Project project = collection.LoadProject(toLoad);
            yield return (projFile, packageName, project);
        }
    }

    /// <summary>
    /// Resolves the release version for a validation item: by explicit
    /// <c>ReleaseVersionProperty</c> metadata when set, or by auto-derive from the .proj
    /// file name otherwise.
    /// </summary>
    private static string? ResolveReleaseVersion(Project versionsProject, ValidationPackageItem item, string componentName)
    {
        if (item.ReleaseVersionPropertyName is not null)
        {
            return CommonUtilities.FindReleaseVersionByPropertyName(versionsProject, item.ReleaseVersionPropertyName);
        }

        return CommonUtilities.FindReleaseVersion(versionsProject, componentName);
    }

    /// <summary>
    /// Validates one aspect of version overrides across every <c>&lt;FileVersionValidationPackage&gt;</c>
    /// item in every external package .proj. The selectors pick which aspect binding to check and
    /// which downloaded-package field to compare against.
    /// </summary>
    /// <param name="overrideAspectName">Friendly name for error/skip messages
    /// (e.g. <c>FileVersionRevision</c>).</param>
    /// <param name="bindingSelector">Picks the AspectBinding for the aspect being validated;
    /// null means the item has no binding for this aspect and is silently skipped.</param>
    /// <param name="actualValueSelector">Picks the corresponding field from the downloaded
    /// package's metadata.</param>
    private async Task ValidateVersionOverrideAsync(
        string overrideAspectName,
        Func<ValidationPackageItem, AspectBinding?> bindingSelector,
        Func<PackageVersionMetadata, string?> actualValueSelector)
    {
        using ProjectCollection versionsCollection = new();
        Project versionsProject = versionsCollection.LoadProject(VersionsPropsPath);
        List<string> errors = new();
        int checkedCount = 0;

        // Cache per (packageId, version) so multiple aspects/items don't re-download.
        Dictionary<string, PackageVersionMetadata> metadataCache = new();

        foreach ((_, string packageName, Project project) in LoadProjectFiles())
        {
            IReadOnlyList<ValidationPackageItem> items = CommonUtilities.ParseValidationPackageItems(project);

            foreach (ValidationPackageItem item in items)
            {
                AspectBinding? binding = bindingSelector(item);
                if (binding is null)
                {
                    continue;
                }

                string? expectedValue = CommonUtilities.ReadPropertyValue(project, binding.PropertyName);
                if (expectedValue is null)
                {
                    // Explicit binding to a non-existent property is always an error: the author
                    // expressed intent to validate this aspect, so a missing target property is a
                    // configuration bug. Defaulted bindings are "validate if present" — silently
                    // skip when the target property is not defined, since the item didn't ask
                    // for this aspect specifically.
                    if (binding.IsExplicit)
                    {
                        errors.Add($"{packageName}.props: {overrideAspectName} item for package '{item.PackageId}' " +
                            $"names property '{binding.PropertyName}' but no such property is defined.");
                        checkedCount++;
                    }
                    continue;
                }

                string? releaseVersion = ResolveReleaseVersion(versionsProject, item, packageName);
                if (string.IsNullOrEmpty(releaseVersion))
                {
                    errors.Add($"{packageName}.props: {overrideAspectName} item for package '{item.PackageId}': " +
                        $"could not resolve release version " +
                        $"({(item.ReleaseVersionPropertyName is null ? "auto-derive failed" : $"property '{item.ReleaseVersionPropertyName}' not in eng/Versions.props")}).");
                    checkedCount++;
                    continue;
                }

                string cacheKey = $"{item.PackageId}|{releaseVersion}";
                if (!metadataCache.TryGetValue(cacheKey, out PackageVersionMetadata? versionMetadata))
                {
                    versionMetadata = await CommonUtilities.GetPackageVersionMetadataAsync(
                        RepoRoot, item.PackageId, releaseVersion);
                    metadataCache[cacheKey] = versionMetadata;
                }

                string? actualValue = actualValueSelector(versionMetadata);
                if (string.IsNullOrEmpty(actualValue))
                {
                    errors.Add($"{packageName}.props: {overrideAspectName} item for package '{item.PackageId}': " +
                        $"package {item.PackageId} {releaseVersion} was downloaded but did not expose a " +
                        $"{overrideAspectName} field. Either the package no longer ships this metadata, or the " +
                        $"download/extraction is broken — verify the published package contents.");
                    checkedCount++;
                    continue;
                }

                if (expectedValue != actualValue)
                {
                    errors.Add($"{packageName}.props: {binding.PropertyName} '{expectedValue}' does not match " +
                        $"actual '{actualValue}' from {item.PackageId} {releaseVersion}.");
                }

                checkedCount++;
            }
        }

        Skip.If(checkedCount == 0, $"No components with {overrideAspectName} were found to validate.");
        AssertNoErrors(errors, overrideAspectName);
    }

    private static void AssertNoErrors(List<string> errors, string validationName)
    {
        Assert.True(errors.Count == 0,
            $"{validationName} validation failed:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}");
    }
}
