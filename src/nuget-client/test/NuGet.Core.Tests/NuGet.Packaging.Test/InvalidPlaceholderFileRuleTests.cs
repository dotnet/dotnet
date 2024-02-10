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
    public class InvalidPlaceholderFileRuleTests
    {
        [Fact]
        public void Validate_FileWithEmptyDirectorySymbolWithOtherFiles_GeneratesWarning()
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
"    </metadata>" +
"</package>";

            using (var testDirectory = TestDirectory.Create())
            {
                var nuspecPath = Path.Combine(testDirectory, "test.nuspec");
                File.AppendAllText(nuspecPath, nuspecContent);

                // create a directory that contains a _._ file and another file
                var toolsPath = Path.Combine(testDirectory, "tools");
                Directory.CreateDirectory(toolsPath);

                var emptyDirFilePath = Path.Combine(toolsPath, "_._");
                var otherFilePath = Path.Combine(toolsPath, "sample.txt");
                var stream = File.CreateText(emptyDirFilePath);
                stream.Dispose();
                stream = File.CreateText(otherFilePath);
                stream.Dispose();

                var builder = new PackageBuilder();
                var runner = new PackCommandRunner(
                    new PackArgs
                    {
                        CurrentDirectory = testDirectory,
                        OutputDirectory = testDirectory,
                        Path = nuspecPath,
                        Exclude = Array.Empty<string>(),
                        Symbols = false,
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

                    Assert.Contains(issues, p => p.Code == NuGetLogCode.NU5109 && p.Message.Contains(@"The file at 'tools/_._' uses the symbol for empty directory '_._'"));
                }
            }
        }

        [Fact]
        public void Validate_FileWithEmptyDirectorySymbolWithNoOtherFiles_NoWarning()
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
"    </metadata>" +
"</package>";

            using (var testDirectory = TestDirectory.Create())
            {
                var nuspecPath = Path.Combine(testDirectory, "test.nuspec");
                File.AppendAllText(nuspecPath, nuspecContent);

                // create a directory that contains a _._ file but no other file
                var toolsPath = Path.Combine(testDirectory, "tools");
                Directory.CreateDirectory(toolsPath);

                var emptyDirFilePath = Path.Combine(toolsPath, "_._");
                var stream = File.CreateText(emptyDirFilePath);
                stream.Dispose();

                var builder = new PackageBuilder();
                var runner = new PackCommandRunner(
                    new PackArgs
                    {
                        CurrentDirectory = testDirectory,
                        OutputDirectory = testDirectory,
                        Path = nuspecPath,
                        Exclude = Array.Empty<string>(),
                        Symbols = false,
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

                    Assert.DoesNotContain(issues, p => p.Code == NuGetLogCode.NU5109 && p.Message.Contains(@"The file at 'tools/_._' uses the symbol for empty directory '_._'"));
                }
            }
        }
    }
}
