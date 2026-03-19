// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using TestUtilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Tests;

/// <summary>
/// Validates that NuGet Central Package Management (CPM) configuration across
/// all repos in the VMR is consistent. In particular, checks that implicitly
/// defined PackageReferences in MSBuild .targets and .props files do not conflict
/// with PackageVersion entries in Directory.Packages.props files.
/// See: https://learn.microsoft.com/nuget/consume-packages/Central-Package-Management
/// </summary>
[Trait("Category", "CentralPackageManagement")]
public class CentralPackageManagementTests
{
    private ITestOutputHelper OutputHelper { get; }

    public static bool IncludeCpmTests => !string.IsNullOrWhiteSpace(Config.RepoRoot);

    public CentralPackageManagementTests(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
    }

    /// <summary>
    /// NuGet CPM rule: implicitly defined PackageReferences (IsImplicitlyDefined="true")
    /// cannot have a corresponding PackageVersion entry in Directory.Packages.props.
    /// Violation produces NU1009 at restore time.
    ///
    /// This test scans all repos in the VMR for this conflict pattern.
    /// </summary>
    [ConditionalFact(typeof(CentralPackageManagementTests), nameof(IncludeCpmTests))]
    public void ImplicitPackageReferences_ShouldNotConflictWithPackageVersionEntries()
    {
        string repoRoot = Config.RepoRoot!;
        string srcDir = Path.Combine(repoRoot, "src");

        var allConflicts = new List<string>();

        // Scan each repo under src/
        foreach (string repoDir in Directory.GetDirectories(srcDir))
        {
            string repoName = Path.GetFileName(repoDir);
            string directoryPackagesProps = Path.Combine(repoDir, "Directory.Packages.props");

            if (!File.Exists(directoryPackagesProps))
            {
                continue;
            }

            // Collect PackageVersion entries, following simple Import directives
            HashSet<string> packageVersionIds = GetPackageVersionIds(directoryPackagesProps, OutputHelper);

            if (packageVersionIds.Count == 0)
            {
                continue;
            }

            // Find all IsImplicitlyDefined="true" PackageReferences in .targets and .props files
            var implicitRefs = new List<(string file, string packageId)>();

            foreach (string file in EnumerateMSBuildFiles(repoDir))
            {
                // Cheap pre-filter: skip files that don't contain the marker string
                string content;
                try
                {
                    content = File.ReadAllText(file);
                }
                catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
                {
                    OutputHelper.WriteLine($"WARNING: Could not read {file}: {ex.Message}");
                    continue;
                }

                if (!content.Contains("IsImplicitlyDefined", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                foreach (string id in GetImplicitPackageReferenceIds(content, file, OutputHelper))
                {
                    implicitRefs.Add((file, id));
                }
            }

            // Check for conflicts
            foreach (var (file, packageId) in implicitRefs)
            {
                if (packageVersionIds.Contains(packageId))
                {
                    string relativePath = Path.GetRelativePath(repoRoot, file);
                    string message = $"[{repoName}] '{packageId}' is implicitly defined in {relativePath} " +
                                     $"but also has a PackageVersion entry in {Path.GetRelativePath(repoRoot, directoryPackagesProps)}";
                    allConflicts.Add(message);
                    OutputHelper.WriteLine($"CONFLICT: {message}");
                }
            }
        }

        Assert.True(allConflicts.Count == 0,
            $"Found {allConflicts.Count} CPM conflict(s) that will cause NU1009 errors. " +
            $"Remove the PackageVersion entries from Directory.Packages.props for implicitly defined packages, " +
            $"or remove IsImplicitlyDefined from the PackageReference.\n" +
            string.Join("\n", allConflicts));
    }

    /// <summary>
    /// Collects PackageVersion Include/Update values from an XML file,
    /// following simple Import directives (relative paths without MSBuild properties).
    /// </summary>
    private static HashSet<string> GetPackageVersionIds(string xmlFile, ITestOutputHelper output)
    {
        var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        CollectPackageVersionIds(xmlFile, result, output);
        return result;
    }

    private static void CollectPackageVersionIds(string xmlFile, HashSet<string> result, ITestOutputHelper output)
    {
        XDocument doc;
        try
        {
            doc = XDocument.Load(xmlFile);
        }
        catch
        {
            return;
        }

        string? directory = Path.GetDirectoryName(xmlFile);

        foreach (var element in doc.Descendants())
        {
            if (element.Name.LocalName.Equals("PackageVersion", StringComparison.OrdinalIgnoreCase))
            {
                string? id = element.Attribute("Include")?.Value
                    ?? element.Attribute("Update")?.Value;
                if (id != null)
                {
                    result.Add(id);
                }
            }
            else if (element.Name.LocalName.Equals("Import", StringComparison.OrdinalIgnoreCase))
            {
                string? project = element.Attribute("Project")?.Value;
                if (project == null || directory == null)
                {
                    continue;
                }

                if (project.Contains("$("))
                {
                    output.WriteLine($"INFO: Skipping Import with MSBuild property in {xmlFile}: {project}");
                    continue;
                }

                string importPath = Path.GetFullPath(Path.Combine(directory, project));
                if (File.Exists(importPath))
                {
                    CollectPackageVersionIds(importPath, result, output);
                }
            }
        }
    }

    private static IEnumerable<string> GetImplicitPackageReferenceIds(string content, string filePath, ITestOutputHelper output)
    {
        XDocument doc;
        try
        {
            doc = XDocument.Parse(content);
        }
        catch (Exception ex)
        {
            output.WriteLine($"WARNING: Failed to parse {filePath}: {ex.Message}");
            yield break;
        }

        foreach (var element in doc.Descendants())
        {
            if (!element.Name.LocalName.Equals("PackageReference", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            string? isImplicit = element.Attribute("IsImplicitlyDefined")?.Value;
            if (string.Equals(isImplicit, "true", StringComparison.OrdinalIgnoreCase))
            {
                string? id = element.Attribute("Include")?.Value;
                if (id != null)
                {
                    yield return id;
                }
            }
        }
    }

    private static IEnumerable<string> EnumerateMSBuildFiles(string directory)
    {
        var options = new EnumerationOptions
        {
            RecurseSubdirectories = true,
            IgnoreInaccessible = true,
        };

        foreach (string file in Directory.EnumerateFiles(directory, "*.targets", options))
        {
            // Skip test assets and node_modules
            if (file.Contains("testassets", StringComparison.OrdinalIgnoreCase) ||
                file.Contains("node_modules", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
            yield return file;
        }

        foreach (string file in Directory.EnumerateFiles(directory, "*.props", options))
        {
            if (file.Contains("testassets", StringComparison.OrdinalIgnoreCase) ||
                file.Contains("node_modules", StringComparison.OrdinalIgnoreCase) ||
                file.EndsWith("Directory.Packages.props", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
            yield return file;
        }
    }
}
