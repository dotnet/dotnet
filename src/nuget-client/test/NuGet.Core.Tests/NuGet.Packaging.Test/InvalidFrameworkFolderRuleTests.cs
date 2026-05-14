// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Packaging.Rules;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class InvalidFrameworkFolderRuleTests
    {
        [Fact]
        public void Validate_AssemblyWithInvalidTFMInPath_GeneratesWarning()
        {
            // Arrange
            var nuspecContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
"<package xmlns=\"http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd\">" +
"   <metadata>" +
"        <id>test</id>" +
"        <version>1.0.0</version>" +
"        <authors>Unit Test</authors>" +
"        <description>Sample Description</description>" +
"        <language>en-US</language>" +
"    <dependencies>" +
"      <dependency id=\"System.Collections.Immutable\" version=\"4.3.0\" />" +
"    </dependencies>" +
"    </metadata>" +
"</package>";

            using (var testDirectory = TestDirectory.Create())
            {
                var nuspecPath = Path.Combine(testDirectory, "test.nuspec");
                File.AppendAllText(nuspecPath, nuspecContent);

                // create a random_tfm directory in a lib directory
                Directory.CreateDirectory(Path.Combine(testDirectory, "lib", "random_tfm"));

                // place a dll inside the folder
                var stream = File.Create(Path.Combine(testDirectory, "lib", "random_tfm", "test.dll"));
                stream.Dispose();

                var builder = new PackageBuilder();
                var runner = new PackCommandRunner(
                    new PackArgs
                    {
                        CurrentDirectory = testDirectory,
                        OutputDirectory = testDirectory,
                        Path = nuspecPath,
                        Exclude = Array.Empty<string>(),
                        Symbols = true,
                        Logger = NullLogger.Instance
                    },
                    MSBuildProjectFactory.ProjectCreator,
                    builder);

                Assert.True(runner.RunPackageBuild());

                var ruleSet = RuleSet.PackageCreationRuleSet;
                var nupkgPath = Path.Combine(testDirectory, "test.1.0.0.nupkg");

                using (var reader = new PackageArchiveReader(nupkgPath))
                {
                    var issues = new List<PackagingLogMessage>();
                    foreach (var rule in ruleSet)
                    {
                        issues.AddRange(rule.Validate(reader).OrderBy(p => p.Code.ToString(), StringComparer.CurrentCulture));
                    }

                    Assert.Contains(issues, p => p.Code == NuGetLogCode.NU5103 && p.Message.Contains("'lib' is not recognized as a valid framework name"));
                }
            }
        }

        [Fact]
        public void Validate_AssemblyWithValidTFMInPath_NoWarning()
        {
            // Arrange
            var nuspecContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
"<package xmlns=\"http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd\">" +
"   <metadata>" +
"        <id>test</id>" +
"        <version>1.0.0</version>" +
"        <authors>Unit Test</authors>" +
"        <description>Sample Description</description>" +
"        <language>en-US</language>" +
"    <dependencies>" +
"      <dependency id=\"System.Collections.Immutable\" version=\"4.3.0\" />" +
"    </dependencies>" +
"    </metadata>" +
"</package>";

            using (var testDirectory = TestDirectory.Create())
            {
                var nuspecPath = Path.Combine(testDirectory, "test.nuspec");
                File.AppendAllText(nuspecPath, nuspecContent);

                // create a random_tfm directory in a lib directory
                Directory.CreateDirectory(Path.Combine(testDirectory, "lib", "net46"));

                // place a dll inside the folder
                var stream = File.Create(Path.Combine(testDirectory, "lib", "net46", "test.dll"));
                stream.Dispose();

                var builder = new PackageBuilder();
                var runner = new PackCommandRunner(
                    new PackArgs
                    {
                        CurrentDirectory = testDirectory,
                        OutputDirectory = testDirectory,
                        Path = nuspecPath,
                        Exclude = Array.Empty<string>(),
                        Symbols = true,
                        Logger = NullLogger.Instance
                    },
                    MSBuildProjectFactory.ProjectCreator,
                    builder);

                Assert.True(runner.RunPackageBuild());

                var ruleSet = RuleSet.PackageCreationRuleSet;
                var nupkgPath = Path.Combine(testDirectory, "test.1.0.0.nupkg");

                using (var reader = new PackageArchiveReader(nupkgPath))
                {
                    var issues = new List<PackagingLogMessage>();
                    foreach (var rule in ruleSet)
                    {
                        issues.AddRange(rule.Validate(reader).OrderBy(p => p.Code.ToString(), StringComparer.CurrentCulture));
                    }

                    Assert.DoesNotContain(issues, p => p.Code == NuGetLogCode.NU5103 && p.Message.Contains("'lib' is not recognized as a valid framework name"));
                }
            }
        }
    }
}
