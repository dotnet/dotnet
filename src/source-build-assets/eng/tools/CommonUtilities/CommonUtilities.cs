// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Build.Evaluation;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace SbrpUtilities;

public record PackageVersionMetadata(
    int? Revision,
    string? FileVersion,
    string? AssemblyVersion,
    string? InformationalVersion);

/// <summary>
/// How a single aspect (FileVersionRevision / AssemblyVersionOverride /
/// InformationalVersionOverride) is bound to an MSBuild property in the .proj.
/// </summary>
/// <param name="PropertyName">Name of the MSBuild property in the .proj holding the value.</param>
/// <param name="IsExplicit">True when the binding came from item-level metadata on the
/// individual <c>FileVersionValidationPackage</c> item; false when it came from an
/// <c>ItemDefinitionGroup</c> default. Explicit bindings are an opt-in: if the named
/// property does not exist, that is an error. Defaulted bindings are "validate if present":
/// if the named property does not exist, the aspect is silently skipped for this item.</param>
public record AspectBinding(string PropertyName, bool IsExplicit);

/// <summary>
/// Describes one NuGet package referenced by a <c>FileVersionValidationPackage</c> item
/// in an external package <c>.proj</c> file. Encapsulates which MSBuild property names hold
/// the corresponding version override values, and which property in <c>eng/Versions.props</c>
/// holds the package's release version.
///
/// Each aspect binding is nullable. Null means no binding at all (the item neither
/// inherited a default nor declared item-level metadata for that aspect).
/// </summary>
public record ValidationPackageItem(
    string PackageId,
    AspectBinding? FileVersionRevision,
    AspectBinding? AssemblyVersionOverride,
    AspectBinding? InformationalVersionOverride,
    string? ReleaseVersionPropertyName);

public static class CommonUtilities
{
    private const string ItemName = "FileVersionValidationPackage";
    private const string FileVersionRevisionMetadataName = "FileVersionRevisionProperty";
    private const string AssemblyVersionOverrideMetadataName = "AssemblyVersionOverrideProperty";
    private const string InformationalVersionOverrideMetadataName = "InformationalVersionOverrideProperty";
    private const string ReleaseVersionMetadataName = "ReleaseVersionProperty";

    /// <summary>
    /// Parses the <c>FileVersionValidationPackage</c> items from an evaluated MSBuild project.
    /// Item-level metadata wins; <c>ItemDefinitionGroup</c> defaults from imported files apply
    /// where the item itself does not specify the metadata.
    /// </summary>
    public static IReadOnlyList<ValidationPackageItem> ParseValidationPackageItems(Project project)
    {
        string projFile = project.FullPath;

        List<ValidationPackageItem> results = new();
        foreach (ProjectItem item in project.GetItems(ItemName))
        {
            string packageId = item.EvaluatedInclude;
            if (string.IsNullOrWhiteSpace(packageId))
            {
                throw new InvalidOperationException(
                    $"{Path.GetFileName(projFile)}: <{ItemName}> item is missing a non-empty Include attribute.");
            }

            results.Add(new ValidationPackageItem(
                packageId,
                ReadAspectBinding(item, FileVersionRevisionMetadataName, projFile),
                ReadAspectBinding(item, AssemblyVersionOverrideMetadataName, projFile),
                ReadAspectBinding(item, InformationalVersionOverrideMetadataName, projFile),
                ReadLiteralMetadataValue(item, ReleaseVersionMetadataName, projFile)));
        }

        DetectPropertyNameCollisions(results, project);

        return results;
    }

    /// <summary>
    /// Reads an aspect-property metadata value from a project item, detecting whether the
    /// binding is explicit (item-level) or defaulted (inherited from an
    /// <c>ItemDefinitionGroup</c>). Returns <c>null</c> when no binding exists for the aspect.
    /// </summary>
    private static AspectBinding? ReadAspectBinding(ProjectItem item, string metadataName, string projFile)
    {
        string? value = ReadLiteralMetadataValue(item, metadataName, projFile);
        if (value is null)
        {
            return null;
        }

        // DirectMetadata only includes metadata declared on the item itself, not metadata
        // inherited from ItemDefinitionGroup. This is exactly the explicit-vs-default
        // distinction we need.
        bool isExplicit = item.DirectMetadata.Any(m =>
            string.Equals(m.Name, metadataName, StringComparison.Ordinal));

        return new AspectBinding(value, isExplicit);
    }

    /// <summary>
    /// Reads a metadata value and validates that it is a literal property name (no MSBuild
    /// expression characters). Returns <c>null</c> when the metadata is absent or empty.
    /// </summary>
    private static string? ReadLiteralMetadataValue(ProjectItem item, string metadataName, string projFile)
    {
        if (!item.HasMetadata(metadataName))
        {
            return null;
        }

        // Check the UN-evaluated value for MSBuild expressions before falling back to the
        // evaluated value. MSBuild would expand $(Foo) to empty (if Foo is undefined) before
        // we ever see it, hiding the bug. Authors who write $(Foo), @(Bar), or %(Baz) here
        // are almost certainly confused — the metadata value is a literal property NAME, not
        // a property reference. Reject up front.
        ProjectMetadata? meta = item.GetMetadata(metadataName);
        string? unevaluated = meta?.UnevaluatedValue;
        if (unevaluated is not null
            && (unevaluated.Contains("$(", StringComparison.Ordinal)
                || unevaluated.Contains("@(", StringComparison.Ordinal)
                || unevaluated.Contains("%(", StringComparison.Ordinal)))
        {
            throw new InvalidOperationException(
                $"{Path.GetFileName(projFile)}: <{ItemName} Include=\"{item.EvaluatedInclude}\"> " +
                $"{metadataName} metadata must be a literal property name, not an MSBuild expression. " +
                $"Got: '{unevaluated}'.");
        }

        string value = item.GetMetadataValue(metadataName);
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return value;
    }

    /// <summary>
    /// Detects when two distinct <c>FileVersionValidationPackage</c> items in the same project
    /// resolve to the same target property for a writeable aspect (FileVersionRevision,
    /// AssemblyVersionOverride, InformationalVersionOverride). This is dangerous because the
    /// auto-update tool would write the same property twice with different per-package values
    /// — last write wins, and at least one package's metadata would silently be wrong.
    /// </summary>
    /// <remarks>
    /// An item is only counted for collision purposes if it actually opts into the aspect —
    /// either via an explicit (item-level) binding, or via a defaulted binding whose target
    /// property is defined in the project. Items that inherit a default but where the target
    /// property does not exist are silently skipped at read/write time, so they cannot
    /// collide.
    /// </remarks>
    /// <example>
    /// Given a multi-item .proj like:
    /// <code>
    /// <![CDATA[
    /// <!-- BAD: both items inherit the default FileVersionRevisionProperty=FileVersionRevision
    ///      and both write to a FileVersionRevision property that exists in the .proj -->
    /// <PropertyGroup>
    ///   <FileVersionRevision>0</FileVersionRevision>
    /// </PropertyGroup>
    /// <ItemGroup>
    ///   <FileVersionValidationPackage Include="PackageA" />
    ///   <FileVersionValidationPackage Include="PackageB" />
    /// </ItemGroup>
    /// ]]>
    /// </code>
    /// Both items would target the same FileVersionRevision property, but each package
    /// publishes its own revision. The fix is per-item explicit metadata:
    /// <code>
    /// <![CDATA[
    /// <ItemGroup>
    ///   <FileVersionValidationPackage Include="PackageA">
    ///     <FileVersionRevisionProperty>PackageAFileVersionRevision</FileVersionRevisionProperty>
    ///   </FileVersionValidationPackage>
    ///   <FileVersionValidationPackage Include="PackageB">
    ///     <FileVersionRevisionProperty>PackageBFileVersionRevision</FileVersionRevisionProperty>
    ///   </FileVersionValidationPackage>
    /// </ItemGroup>
    /// ]]>
    /// </code>
    /// </example>
    private static void DetectPropertyNameCollisions(IReadOnlyList<ValidationPackageItem> items, Project project)
    {
        if (items.Count < 2)
        {
            return;
        }

        CheckAspectCollisions(items, FileVersionRevisionMetadataName, i => i.FileVersionRevision, project);
        CheckAspectCollisions(items, AssemblyVersionOverrideMetadataName, i => i.AssemblyVersionOverride, project);
        CheckAspectCollisions(items, InformationalVersionOverrideMetadataName, i => i.InformationalVersionOverride, project);
    }

    private static void CheckAspectCollisions(
        IReadOnlyList<ValidationPackageItem> items,
        string metadataName,
        Func<ValidationPackageItem, AspectBinding?> selector,
        Project project)
    {
        // An item participates in this aspect only if its binding is explicit OR the
        // target property is actually defined in the project. A defaulted binding whose
        // target property is missing is a silent skip at read/write time, so it can't
        // collide with anything.
        var collisions = items
            .Select(item => new { item, binding = selector(item) })
            .Where(x => x.binding is not null
                && (x.binding!.IsExplicit || project.GetProperty(x.binding.PropertyName) is not null))
            .GroupBy(x => x.binding!.PropertyName, StringComparer.Ordinal)
            .Where(g => g.Select(x => x.item.PackageId).Distinct(StringComparer.Ordinal).Count() > 1);

        foreach (var collision in collisions)
        {
            string collidingPackages = string.Join(", ",
                collision.Select(x => x.item.PackageId).Distinct(StringComparer.Ordinal));
            throw new InvalidOperationException(
                $"{Path.GetFileName(project.FullPath)}: multiple <{ItemName}> items resolve {metadataName} " +
                $"to the same property '{collision.Key}': {collidingPackages}. Each package needs " +
                $"its own property — add explicit <{metadataName}> metadata to each item to override " +
                $"the shared default.");
        }
    }

    /// <summary>
    /// Reads the evaluated value of a property from a project, or <c>null</c> if the property
    /// is not defined.
    /// </summary>
    public static string? ReadPropertyValue(Project project, string propertyName)
    {
        ProjectProperty? property = project.GetProperty(propertyName);
        return property?.EvaluatedValue;
    }

    /// <summary>
    /// Finds the release version for a component in eng/Versions.props by matching the
    /// normalized (hyphen-stripped, case-insensitive) component name against properties
    /// ending in "ReleaseVersion".
    /// </summary>
    public static string? FindReleaseVersion(Project versionsProject, string componentName)
    {
        string normalizedName = componentName.Replace("-", "", StringComparison.Ordinal).ToLowerInvariant();

        foreach (ProjectProperty property in versionsProject.AllEvaluatedProperties)
        {
            string name = property.Name;
            if (!name.EndsWith("ReleaseVersion", StringComparison.Ordinal))
            {
                continue;
            }

            string namePrefix = name.Substring(0, name.Length - "ReleaseVersion".Length);
            if (namePrefix.Equals(normalizedName, StringComparison.OrdinalIgnoreCase))
            {
                return property.EvaluatedValue;
            }
        }

        return null;
    }

    /// <summary>
    /// Finds a release version in eng/Versions.props by exact property name match.
    /// Used when a <c>FileVersionValidationPackage</c> item explicitly names a
    /// <c>ReleaseVersionProperty</c>.
    /// </summary>
    public static string? FindReleaseVersionByPropertyName(Project versionsProject, string propertyName)
    {
        return ReadPropertyValue(versionsProject, propertyName);
    }

    /// <summary>
    /// Downloads a NuGet package from the sources configured in the NuGet.config
    /// found at <paramref name="settingsRoot"/>. Returns the package stream, or null
    /// if the package was not found in any source.
    /// </summary>
    public static async Task<MemoryStream?> DownloadPackageAsync(
        string settingsRoot, string packageId, string version, CancellationToken cancellationToken = default)
    {
        ISettings settings = Settings.LoadDefaultSettings(settingsRoot);
        PackageSourceProvider sourceProvider = new(settings);
        IEnumerable<PackageSource> sources = sourceProvider.LoadPackageSources().Where(s => s.IsEnabled);
        var providers = Repository.Provider.GetCoreV3();
        NuGetVersion nugetVersion = new(version);

        using SourceCacheContext cacheContext = new();

        foreach (PackageSource source in sources)
        {
            SourceRepository repository = new(source, providers);
            MemoryStream stream = new();
            try
            {
                FindPackageByIdResource resource = await repository.GetResourceAsync<FindPackageByIdResource>(cancellationToken);
                if (await resource.CopyNupkgToStreamAsync(
                    packageId, nugetVersion, stream, cacheContext, NullLogger.Instance, cancellationToken))
                {
                    stream.Position = 0;
                    return stream;
                }
            }
            catch
            {
            }

            stream.Dispose();
        }

        return null;
    }

    /// <summary>
    /// Downloads a NuGet package and returns version metadata from its first DLL.
    /// Combines <see cref="DownloadPackageAsync"/> with DLL extraction and version reading.
    /// Returns null values if the package could not be downloaded or contains no DLL.
    /// </summary>
    public static async Task<PackageVersionMetadata> GetPackageVersionMetadataAsync(
        string settingsRoot, string packageId, string version, CancellationToken cancellationToken = default)
    {
        using MemoryStream? packageStream = await DownloadPackageAsync(settingsRoot, packageId, version, cancellationToken);
        if (packageStream is null)
        {
            return new(null, null, null, null);
        }

        using PackageArchiveReader packageReader = new(packageStream);
        string? dllItem = packageReader.GetLibItems()
            .SelectMany(group => group.Items)
            .FirstOrDefault(item => item.EndsWith(".dll", StringComparison.OrdinalIgnoreCase));

        if (dllItem is null)
        {
            return new(null, null, null, null);
        }

        string tempDll = Path.Combine(Path.GetTempPath(), $"sbrp-{Guid.NewGuid():N}.dll");
        try
        {
            using (Stream dllStream = packageReader.GetStream(dllItem))
            using (FileStream fs = File.Create(tempDll))
            {
                dllStream.CopyTo(fs);
            }

            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(tempDll);
            string? assemblyVersion = AssemblyName.GetAssemblyName(tempDll).Version?.ToString();
            return new(versionInfo.FilePrivatePart, versionInfo.FileVersion, assemblyVersion, versionInfo.ProductVersion);
        }
        finally
        {
            try
            {
                File.Delete(tempDll);
            }
            catch
            {
            }
        }
    }
}
