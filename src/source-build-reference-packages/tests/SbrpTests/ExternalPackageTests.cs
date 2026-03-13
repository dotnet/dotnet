// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
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
}
