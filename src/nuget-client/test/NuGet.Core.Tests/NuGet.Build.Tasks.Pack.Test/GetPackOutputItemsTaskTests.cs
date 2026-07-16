// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Build.Tasks.Pack.Test
{
    public class GetPackOutputItemsTaskTests
    {
        public static IEnumerable<object[]> PackageFileNameTestCases => PackageFileNameTestCase.TestCases;

        // This unit test verifies that GetPackOutputItemsTask outputs the expected file name.
        [Theory]
        [MemberData(nameof(PackageFileNameTestCases))]
        public void GetPackOutputItemsTaskTests_Execute_CheckPackageFileName(PackageFileNameTestCase testCase)
        {
            var outputItemTask = new GetPackOutputItemsTask
            {
                PackageId = PackageFileNameTestCase.IdProjProp,
                PackageVersion = testCase.VersionProjProp,
                IncludeSymbols = testCase.IncludeSymbols,
                SymbolPackageFormat = PackageFileNameTestsCommon.GetSymbolPackageFormatText(testCase.SymbolPackageFormat),
                OutputFileNamesWithoutVersion = testCase.OutputFileNamesWithoutVersion
            };

            var nuspecProps = new List<string>();
            if (!string.IsNullOrWhiteSpace(testCase.VersionNuspecProperties))
            {
                nuspecProps.Add($"version={testCase.VersionNuspecProperties}");
            }
            if (!string.IsNullOrWhiteSpace(testCase.IdNuspecProperties))
            {
                nuspecProps.Add($"id={testCase.IdNuspecProperties}");
            }
            if (nuspecProps.Count > 0)
            {
                outputItemTask.NuspecProperties = nuspecProps.ToArray();
            }

            using var testDirectory = TestDirectory.Create();
            outputItemTask.PackageOutputPath = testDirectory.Path;
            outputItemTask.NuspecOutputPath = testDirectory.Path;
            if (testCase.UseNuspecFile)
            {
                outputItemTask.NuspecFile = Path.Combine(testDirectory.Path, PackageFileNameTestsCommon.FILENAME_NUSPEC_FILE);
            }

            PackageFileNameTestsCommon.CreateNuspecFile(testCase, testDirectory);

            Assert.True(outputItemTask.Execute());

            // GetPackOutputItemsTask always emits the primary .nupkg (and its .nuspec) plus, when symbols
            // are requested, the symbols package (and its .nuspec). Assert on the exact set of package files
            // so the test also fails if the task emits an unexpected extra package, not only a missing one.
            string[] actualPackageFiles = outputItemTask.OutputPackItems
                .Select(item => Path.GetFileName(item.ItemSpec))
                .Where(name => name.EndsWith(".nupkg", StringComparison.OrdinalIgnoreCase)
                    || name.EndsWith(".snupkg", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            actualPackageFiles.Should().BeEquivalentTo(testCase.OutputNupkgNames);
        }

        [Fact]
        public void GetPackOutputItemsTaskTests_Execute_NuspecFileDoesNotExist_FallsBackToProjectProperties()
        {
            using var testDirectory = TestDirectory.Create();

            var outputItemTask = new GetPackOutputItemsTask
            {
                PackageId = "MyPackage",
                PackageVersion = "1.2.3",
                PackageOutputPath = testDirectory.Path,
                NuspecOutputPath = testDirectory.Path,
                SymbolPackageFormat = "snupkg",
                // Point at a nuspec path that does not exist on disk (mirrors the source-build scenario).
                NuspecFile = Path.Combine(testDirectory.Path, "does-not-exist.nuspec")
            };

            Assert.True(outputItemTask.Execute());

            string[] actualPackageFiles = outputItemTask.OutputPackItems
                .Select(item => Path.GetFileName(item.ItemSpec))
                .Where(name => name.EndsWith(".nupkg", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            actualPackageFiles.Should().BeEquivalentTo(new[] { "MyPackage.1.2.3.nupkg" });
        }
    }
}
