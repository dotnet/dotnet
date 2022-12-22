// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Commands.Test
{
    public class PackCommandRunnerTests
    {
        [Fact]
        public void RunPackageBuild_WithDefaultExcludes_ExcludesDefaultExcludes()
        {
            using (var test = DefaultExclusionsTest.Create())
            {
                var args = new PackArgs()
                {
                    CurrentDirectory = test.CurrentDirectory.FullName,
                    Exclude = Enumerable.Empty<string>(),
                    Logger = NullLogger.Instance,
                    Path = test.NuspecFile.FullName
                };
                var runner = new PackCommandRunner(args, createProjectFactory: null);

                Assert.True(runner.RunPackageBuild());

                using (FileStream stream = test.NupkgFile.OpenRead())
                using (var package = new ZipArchive(stream, ZipArchiveMode.Read))
                {
                    foreach (string unexpectedEntryName in test.UnexpectedEntryNames)
                    {
                        Assert.Equal(0, package.Entries.Count(entry => entry.Name == unexpectedEntryName));
                    }

                    foreach (string expectedEntryName in test.ExpectedEntryNames)
                    {
                        Assert.Equal(1, package.Entries.Count(entry => entry.Name == expectedEntryName));
                    }
                }
            }
        }

        [Fact]
        public void RunPackageBuild_WithGenerateNugetPackageFalse_ReturnsTrue()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var args = new PackArgs()
                {
                    CurrentDirectory = testDirectory.Path,
                    Logger = NullLogger.Instance,
                    PackTargetArgs = new MSBuildPackTargetArgs()
                    {
                        NuspecOutputPath = Path.Combine(testDirectory.Path, "obj", "Debug"),
                        ContentFiles = new Dictionary<string, IEnumerable<ContentMetadata>>()
                    },
                    Path = string.Empty,
                    Exclude = Array.Empty<string>()
                };
                var packageBuilder = new PackageBuilder()
                {
                    Id = "test",
                    Version = new NuGetVersion(1, 0, 0),
                    Description = "Testing PackCommandRunner.GenerateNugetPackage = false"
                };
                packageBuilder.Authors.Add("tester");

                var runner = new PackCommandRunner(args, MSBuildProjectFactory.ProjectCreator, packageBuilder);
                runner.GenerateNugetPackage = false;

                // Act
                var actual = runner.RunPackageBuild();

                // Assert
                Assert.True(actual, "PackCommandRunner.RunPackageBuild was not successful");
                var expectedNuspecPath = Path.Combine(args.PackTargetArgs.NuspecOutputPath, "test.1.0.0.nuspec");
                Assert.True(File.Exists(expectedNuspecPath), "nuspec file does not exist");
            }
        }

        private sealed class DefaultExclusionsTest : IDisposable
        {
            private readonly TestDirectory _testDirectory;

            internal DirectoryInfo CurrentDirectory { get; }
            internal FileInfo NupkgFile { get; }
            internal FileInfo NuspecFile { get; }
            internal IEnumerable<string> ExpectedEntryNames { get; }
            internal IEnumerable<string> UnexpectedEntryNames { get; }

            private DefaultExclusionsTest(
                TestDirectory testDirectory,
                DirectoryInfo currentDirectory,
                FileInfo nuspecFile,
                FileInfo nupkgFile,
                IEnumerable<string> expectedEntryNames,
                IEnumerable<string> unexpectedEntryNames)
            {
                _testDirectory = testDirectory;
                CurrentDirectory = currentDirectory;
                NuspecFile = nuspecFile;
                NupkgFile = nupkgFile;
                ExpectedEntryNames = expectedEntryNames;
                UnexpectedEntryNames = unexpectedEntryNames;
            }

            internal static DefaultExclusionsTest Create()
            {
                TestDirectory testDirectory = TestDirectory.Create();

                var rootDirectory = new DirectoryInfo(testDirectory.Path);

                DirectoryInfo packRootDirectory = rootDirectory.CreateSubdirectory("a");

                string[] expectedEntryNames = new[] { ".d.e", "f.g" };
                string[] unexpectedEntryNames = new[] { "b.nupkg", ".c" };

                foreach (string entryName in expectedEntryNames.Concat(unexpectedEntryNames))
                {
                    File.WriteAllText(Path.Combine(packRootDirectory.FullName, entryName), string.Empty);
                }

                DirectoryInfo currentDirectory = rootDirectory.CreateSubdirectory("h");

                var nuspecFile = new FileInfo(Path.Combine(currentDirectory.FullName, "i.nuspec"));

                string pattern = $"..{Path.DirectorySeparatorChar}{packRootDirectory.Name}{Path.DirectorySeparatorChar}**{Path.DirectorySeparatorChar}*.*";

                const string packageId = "DefaultExclusions";
                const string packageVersion = "1.0.0";

                File.WriteAllText(nuspecFile.FullName, $@"<?xml version=""1.0""?>
<package>
    <metadata>
        <id>{packageId}</id>
        <version>{packageVersion}</version>
        <title>title</title>
        <description>description</description>
        <authors>author</authors>
        <requireLicenseAcceptance>false</requireLicenseAcceptance>
        <dependencies />
    </metadata>
    <files>
        <file src=""{pattern}"" target="""" />
    </files>   
</package>");

                var nupkgFile = new FileInfo(Path.Combine(currentDirectory.FullName, $"{packageId}.{packageVersion}.nupkg"));

                return new DefaultExclusionsTest(
                    testDirectory,
                    currentDirectory,
                    nuspecFile,
                    nupkgFile,
                    expectedEntryNames.Prepend($"{packageId}.nuspec"),
                    unexpectedEntryNames);
            }

            public void Dispose()
            {
                _testDirectory.Dispose();
            }
        }
    }
}
