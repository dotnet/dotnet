// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace Microsoft.DotNet.Arcade.Sdk.Tests
{
    /// <summary>
    /// Validates that implicitly defined PackageReferences in .targets and .props files
    /// do not conflict with PackageVersion entries in Directory.Packages.props.
    /// NuGet CPM rule: implicit PackageReferences cannot have a corresponding
    /// PackageVersion entry — they must set Version directly on the PackageReference.
    /// Violation produces NU1009 at restore time.
    /// </summary>
    public class CentralPackageManagementTests
    {
        private static readonly string? s_repoRoot = TryGetRepoRoot();

        [Fact]
        public void ImplicitPackageReferences_ShouldNotConflictWithPackageVersionEntries()
        {
            if (s_repoRoot == null)
            {
                // Running on Helix or outside the repo — skip
                return;
            }

            var directoryPackagesPropsPath = Path.Combine(s_repoRoot, "Directory.Packages.props");
            Assert.True(File.Exists(directoryPackagesPropsPath), $"Directory.Packages.props not found at {directoryPackagesPropsPath}");

            // Collect all PackageVersion entries from Directory.Packages.props
            var packageVersionIds = GetPackageIds(directoryPackagesPropsPath, "PackageVersion");

            // Find all IsImplicitlyDefined="true" PackageReferences in .targets and .props files
            var implicitReferences = new List<(string file, string packageId)>();
            var sdkToolsDir = Path.Combine(s_repoRoot, "src", "Microsoft.DotNet.Arcade.Sdk", "tools");

            foreach (var file in Directory.EnumerateFiles(sdkToolsDir, "*.targets", SearchOption.AllDirectories)
                .Concat(Directory.EnumerateFiles(sdkToolsDir, "*.props", SearchOption.AllDirectories)))
            {
                foreach (var id in GetImplicitPackageReferenceIds(file))
                {
                    implicitReferences.Add((file, id));
                }
            }

            // Assert: no implicit PackageReference should have a matching PackageVersion
            var conflicts = implicitReferences
                .Where(r => packageVersionIds.Contains(r.packageId))
                .ToList();

            Assert.True(conflicts.Count == 0,
                $"NU1009 conflict: the following implicitly defined PackageReferences have " +
                $"corresponding PackageVersion entries in Directory.Packages.props. " +
                $"Remove the PackageVersion entries or remove IsImplicitlyDefined from the PackageReference.\n" +
                string.Join("\n", conflicts.Select(c =>
                    $"  - '{c.packageId}' (implicit in {Path.GetRelativePath(s_repoRoot, c.file)})")));
        }

        /// <summary>
        /// Collects PackageVersion Include values from an XML file,
        /// following simple Import directives (relative paths without MSBuild properties).
        /// </summary>
        private static HashSet<string> GetPackageIds(string propsFile, string elementName)
        {
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            CollectElementIds(propsFile, elementName, result);
            return result;
        }

        private static void CollectElementIds(string xmlFile, string elementName, HashSet<string> result)
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
                if (element.Name.LocalName == elementName)
                {
                    string? id = element.Attribute("Include")?.Value;
                    if (id != null)
                    {
                        result.Add(id);
                    }
                }
                else if (element.Name.LocalName == "Import")
                {
                    string? project = element.Attribute("Project")?.Value;
                    if (project != null && !project.Contains("$(") && directory != null)
                    {
                        string importPath = Path.GetFullPath(Path.Combine(directory, project));
                        if (File.Exists(importPath))
                        {
                            CollectElementIds(importPath, elementName, result);
                        }
                    }
                }
            }
        }

        private static IEnumerable<string> GetImplicitPackageReferenceIds(string targetsFile)
        {
            XDocument doc;
            try
            {
                doc = XDocument.Load(targetsFile);
            }
            catch (System.Xml.XmlException)
            {
                // Non-XML files with .targets/.props extension — skip
                yield break;
            }

            foreach (var element in doc.Descendants().Where(e => e.Name.LocalName == "PackageReference"))
            {
                var isImplicit = element.Attribute("IsImplicitlyDefined")?.Value;
                if (string.Equals(isImplicit, "true", StringComparison.OrdinalIgnoreCase))
                {
                    var id = element.Attribute("Include")?.Value;
                    if (id != null)
                    {
                        yield return id;
                    }
                }
            }
        }

        private static string? TryGetRepoRoot()
        {
            var dir = AppContext.BaseDirectory;
            while (dir != null)
            {
                if (File.Exists(Path.Combine(dir, "Directory.Packages.props")) &&
                    Directory.Exists(Path.Combine(dir, "src", "Microsoft.DotNet.Arcade.Sdk")))
                {
                    return dir;
                }
                dir = Path.GetDirectoryName(dir);
            }

            dir = Directory.GetCurrentDirectory();
            while (dir != null)
            {
                if (File.Exists(Path.Combine(dir, "Directory.Packages.props")) &&
                    Directory.Exists(Path.Combine(dir, "src", "Microsoft.DotNet.Arcade.Sdk")))
                {
                    return dir;
                }
                dir = Path.GetDirectoryName(dir);
            }

            return null;
        }
    }
}
