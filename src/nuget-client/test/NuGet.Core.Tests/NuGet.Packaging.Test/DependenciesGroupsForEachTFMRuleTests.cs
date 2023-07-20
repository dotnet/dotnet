// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging.Rules;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class DependenciesGroupsForEachTFMRuleTests
    {
        [Fact]
        public void GenerateWarnings_PackageWithDependenciesForEachTFMInLib_ShouldNotWarn()
        {
            //Arrange
            var frameworks = new NuGetFramework[]
            {
                FrameworkConstants.CommonFrameworks.Net2,
                FrameworkConstants.CommonFrameworks.Net35,
                FrameworkConstants.CommonFrameworks.Net4,
                FrameworkConstants.CommonFrameworks.Net45,
                FrameworkConstants.CommonFrameworks.NetStandard10,
                FrameworkConstants.CommonFrameworks.NetStandard13,
                FrameworkConstants.CommonFrameworks.NetStandard20,
            };

            var files = new string[]
            {
                "lib/net20/test.dll",
                "lib/net35/test.dll",
                "lib/net40/test.dll",
                "lib/net45/test.dll",
                "lib/netstandard1.0/test.dll",
                "lib/netstandard1.3/test.dll",
                "lib/netstandard2.0/test.dll",
            };

            // Act
            var rule = new DependenciesGroupsForEachTFMRule();
            var (compat, file, nuspec) = rule.Categorize(files, frameworks);
            var issues = rule.GenerateWarnings(compat, file, nuspec).ToList();

            // Assert
            Assert.Equal(0, issues.Count);
        }

        [Fact]
        public void GenerateWarnings_PackageWithDependenciesInRefHasExactMatch_ShouldNotWarn()
        {
            var frameworks = new NuGetFramework[]
            {
                FrameworkConstants.CommonFrameworks.Net2,
                FrameworkConstants.CommonFrameworks.Net35,
                FrameworkConstants.CommonFrameworks.Net4,
                FrameworkConstants.CommonFrameworks.Net45,
                FrameworkConstants.CommonFrameworks.NetStandard10,
                FrameworkConstants.CommonFrameworks.NetStandard13,
                FrameworkConstants.CommonFrameworks.NetStandard20,
            };

            //Arrange
            var files = new string[]
            {
                "ref/net20/test.dll",
                "ref/net35/test.dll",
                "ref/net40/test.dll",
                "ref/net45/test.dll",
                "ref/netstandard1.0/test.dll",
                "ref/netstandard1.3/test.dll",
                "ref/netstandard2.0/test.dll",
            };

            // Act
            var rule = new DependenciesGroupsForEachTFMRule();
            var (compat, file, nuspec) = rule.Categorize(files, frameworks);
            var issues = rule.GenerateWarnings(compat, file, nuspec).ToList();

            // Assert
            Assert.Equal(0, issues.Count);
        }

        [Fact]
        public void GenerateWarnings_PackageWithoutDependenciesForEachTFMInLib_ShouldThrowOneWarningCode()
        {
            //Arrange
            var frameworks = new NuGetFramework[]
            {
                FrameworkConstants.CommonFrameworks.Net2,
                FrameworkConstants.CommonFrameworks.Net35,
                FrameworkConstants.CommonFrameworks.Net4,
                FrameworkConstants.CommonFrameworks.Net45,
                FrameworkConstants.CommonFrameworks.NetStandard10,
                FrameworkConstants.CommonFrameworks.NetStandard13,
                FrameworkConstants.CommonFrameworks.NetStandard20,
            };

            var files = new string[]
            {
                "lib/net20/test.dll",
                "lib/net35/test.dll",
                "lib/net40/test.dll"
            };

            // Act
            var rule = new DependenciesGroupsForEachTFMRule();
            var (compat, file, nuspec) = rule.Categorize(files, frameworks);
            var issues = rule.GenerateWarnings(compat, file, nuspec).ToList();

            // Assert
            var expectedWarningMessageExact = AnalysisResources.DependenciesGroupsForEachTFMHasNoExactMatch + Environment.NewLine +
                "- Add lib or ref assemblies for the net45 target framework" + Environment.NewLine +
                "- Add lib or ref assemblies for the netstandard1.0 target framework" + Environment.NewLine +
                "- Add lib or ref assemblies for the netstandard1.3 target framework" + Environment.NewLine +
                "- Add lib or ref assemblies for the netstandard2.0 target framework";

            var warning = issues.Single(p => p.Code == NuGetLogCode.NU5128);
            Assert.Equal(expectedWarningMessageExact, warning.Message);
            Assert.Equal(1, issues.Count);
        }

        [Fact]
        public void GenerateWarnings_PackageWithDependenciesHasCompatMatchNotExactMatch_ShouldThrowTwoWarningCodes()
        {
            var frameworks = new NuGetFramework[]
            {
                FrameworkConstants.CommonFrameworks.Net45,
            };

            var files = new string[]
            {
                "lib/net472/test.dll"
            };

            // Act
            var rule = new DependenciesGroupsForEachTFMRule();
            var (compat, file, nuspec) = rule.Categorize(files, frameworks);
            var issues = rule.GenerateWarnings(compat, file, nuspec).ToList();

            // Assert
            var expectedWarningMessageExact = AnalysisResources.DependenciesGroupsForEachTFMHasNoExactMatch + Environment.NewLine +
                "- Add lib or ref assemblies for the net45 target framework";
            var expectedWarningMessageCompat = AnalysisResources.DependenciesGroupsForEachTFMHasCompatMatch + Environment.NewLine +
                "- Add a dependency group for .NETFramework4.7.2 to the nuspec";

            var firstWarning = issues.Single(p => p.Code == NuGetLogCode.NU5128);
            Assert.Equal(expectedWarningMessageExact, firstWarning.Message);
            var secondWarning = issues.Single(p => p.Code == NuGetLogCode.NU5130);
            Assert.Equal(expectedWarningMessageCompat, secondWarning.Message);
            Assert.Equal(2, issues.Count);
        }

        [Fact]
        public void GenerateWarnings_PackageHasNoDependencyGroupWithFilesInTheLib_ShouldThrowOneWarningCode()
        {
            var frameworks = Array.Empty<NuGetFramework>();
            var files = new string[]
            {
                "lib/net20/test.dll",
                "lib/net35/test.dll",
                "lib/net472/test.dll"
            };

            // Act
            var rule = new DependenciesGroupsForEachTFMRule();
            var (compat, file, nuspec) = rule.Categorize(files, frameworks);
            var issues = rule.GenerateWarnings(compat, file, nuspec).ToList();

            // Assert
            var expectedWarningMessageExact = AnalysisResources.DependenciesGroupsForEachTFMHasNoExactMatch + Environment.NewLine +
                "- Add a dependency group for .NETFramework2.0 to the nuspec" + Environment.NewLine +
                "- Add a dependency group for .NETFramework3.5 to the nuspec" + Environment.NewLine +
                "- Add a dependency group for .NETFramework4.7.2 to the nuspec";

            var firstWarning = issues.Single(p => p.Code == NuGetLogCode.NU5128);
            Assert.Equal(expectedWarningMessageExact, firstWarning.Message);
            Assert.Equal(1, issues.Count);
        }

        [Fact]
        public void GenerateWarnings_PackageHasCompatMatchFromNuspecToPackage_ShouldThrowOneWarningCode()
        {
            var frameworks = new string[]
            {
                ".NETFramework4.7.2"
            }.Select(f => NuGetFramework.Parse(f)); //.NETFrameowrk4.7.2 doesn't have a constant

            var files = new string[]
            {
                "lib/net45/test.dll"
            };

            // Act
            var rule = new DependenciesGroupsForEachTFMRule();
            var (compat, file, nuspec) = rule.Categorize(files, frameworks);
            var issues = rule.GenerateWarnings(compat, file, nuspec).ToList();

            // Assert
            var expectedWarningMessageExact = AnalysisResources.DependenciesGroupsForEachTFMHasNoExactMatch + Environment.NewLine +
                "- Add a dependency group for .NETFramework4.5 to the nuspec" + Environment.NewLine +
                "- Add lib or ref assemblies for the net472 target framework";
            var firstWarning = issues.Single(p => p.Code == NuGetLogCode.NU5128);
            Assert.Equal(expectedWarningMessageExact, firstWarning.Message);
            Assert.Equal(1, issues.Count);
        }

        [Fact]
        public void GenerateWarnings_PackageHasNoDepencyNodeAndHasFilesInLib_ShouldNotWarn()
        {
            var frameworks = Array.Empty<NuGetFramework>();
            var files = new string[]
            {
                "lib/test.dll"
            };

            // Act
            var rule = new DependenciesGroupsForEachTFMRule();
            var (compat, file, nuspec) = rule.Categorize(files, frameworks);
            var issues = rule.GenerateWarnings(compat, file, nuspec).ToList();

            // Assert
            Assert.Equal(0, issues.Count);
        }

        [Fact]
        public void Categorize_PackageWithPerfectlyMatchedTFMsInLibAndNuspec_ShouldAllBeEmpty()
        {
            //Arrange
            var frameworks = new NuGetFramework[]
            {
                FrameworkConstants.CommonFrameworks.Net2,
                FrameworkConstants.CommonFrameworks.Net35,
                FrameworkConstants.CommonFrameworks.Net4,
                FrameworkConstants.CommonFrameworks.Net45,
                FrameworkConstants.CommonFrameworks.NetStandard10,
                FrameworkConstants.CommonFrameworks.NetStandard13,
                FrameworkConstants.CommonFrameworks.NetStandard20,
            };

            var files = new string[]
            {
                "lib/net20/test.dll",
                "lib/net35/test.dll",
                "lib/net40/test.dll",
                "lib/net45/test.dll",
                "lib/netstandard1.0/test.dll",
                "lib/netstandard1.3/test.dll",
                "lib/netstandard2.0/test.dll",
            };

            // Act
            var rule = new DependenciesGroupsForEachTFMRule();
            var (compat, file, nuspec) = rule.Categorize(files, frameworks);

            Assert.Empty(compat);
            Assert.Empty(file);
            Assert.Empty(nuspec);
        }

        [Fact]
        public void Categorize_PackageWithUnmatchedTFMsInNuspec_ShouldHaveOneNotEmpty()
        {
            //Arrange
            var frameworks = new NuGetFramework[]
            {
                FrameworkConstants.CommonFrameworks.Net2,
                FrameworkConstants.CommonFrameworks.Net35,
                FrameworkConstants.CommonFrameworks.Net4,
                FrameworkConstants.CommonFrameworks.Net45,
                FrameworkConstants.CommonFrameworks.NetStandard10,
                FrameworkConstants.CommonFrameworks.NetStandard13,
                FrameworkConstants.CommonFrameworks.NetStandard20,
            };

            var files = new string[]
            {
                "lib/net20/test.dll",
                "lib/net35/test.dll",
                "lib/net40/test.dll",
                "lib/netstandard1.0/test.dll",
                "lib/netstandard1.3/test.dll",
                "lib/netstandard2.0/test.dll",
            };

            // Act
            var rule = new DependenciesGroupsForEachTFMRule();
            var (compat, file, nuspec) = rule.Categorize(files, frameworks);

            Assert.Empty(compat);
            Assert.Empty(file);
            Assert.True(nuspec.Contains(FrameworkConstants.CommonFrameworks.Net45));
            Assert.Equal(1, nuspec.Count);
        }

        [Fact]
        public void Categorize_PackageWithUnmatchedTFMsInFile_ShouldHaveOneNotEmpty()
        {
            //Arrange
            var frameworks = new NuGetFramework[]
            {
                FrameworkConstants.CommonFrameworks.Net2,
                FrameworkConstants.CommonFrameworks.Net35,
                FrameworkConstants.CommonFrameworks.Net4,
                FrameworkConstants.CommonFrameworks.NetStandard10,
                FrameworkConstants.CommonFrameworks.NetStandard13,
                FrameworkConstants.CommonFrameworks.NetStandard20,
            };

            var files = new string[]
            {
                "lib/net20/test.dll",
                "lib/net35/test.dll",
                "lib/net40/test.dll",
                "lib/net45/test.dll",
                "lib/netstandard1.0/test.dll",
                "lib/netstandard1.3/test.dll",
                "lib/netstandard2.0/test.dll",
            };

            // Act
            var rule = new DependenciesGroupsForEachTFMRule();
            var (compat, file, nuspec) = rule.Categorize(files, frameworks);

            Assert.Empty(compat);
            Assert.True(file.Contains(FrameworkConstants.CommonFrameworks.Net45));
            Assert.Equal(file.Count, 1);
            Assert.Empty(nuspec);
        }

        [Fact]
        public void Categorize_PackageWithUnmatchedTFMsInNuspecAndFile_ShouldHaveTwoNotEmpty()
        {
            //Arrange
            var frameworks = new NuGetFramework[]
            {
                FrameworkConstants.CommonFrameworks.Net35,
                FrameworkConstants.CommonFrameworks.Net4,
                FrameworkConstants.CommonFrameworks.Net45,
                FrameworkConstants.CommonFrameworks.NetStandard10,
                FrameworkConstants.CommonFrameworks.NetStandard13,
                FrameworkConstants.CommonFrameworks.NetStandard20,
            };

            var files = new string[]
            {
                "lib/net20/test.dll",
                "lib/net35/test.dll",
                "lib/net40/test.dll",
                "lib/netstandard1.0/test.dll",
                "lib/netstandard1.3/test.dll",
                "lib/netstandard2.0/test.dll",
            };

            // Act
            var rule = new DependenciesGroupsForEachTFMRule();
            var (compat, file, nuspec) = rule.Categorize(files, frameworks);

            Assert.Empty(compat);
            Assert.True(file.Contains(FrameworkConstants.CommonFrameworks.Net2));
            Assert.True(nuspec.Contains(FrameworkConstants.CommonFrameworks.Net45));
            Assert.Equal(file.Count, 1);
            Assert.Equal(nuspec.Count, 1);
        }

        [Fact]
        public void Categorize_PackageWithUnmatchedTFMsInNuspecAndFile_ShouldHaveAllNotEmpty()
        {
            //Arrange
            var frameworks = new NuGetFramework[]
            {
                FrameworkConstants.CommonFrameworks.Net35,
                FrameworkConstants.CommonFrameworks.Net45,
            };

            var files = new string[]
            {
                "lib/net20/test.dll",
                "lib/net35/test.dll",
                "lib/net472/test.dll",
            };

            // Act
            var rule = new DependenciesGroupsForEachTFMRule();
            var (compat, file, nuspec) = rule.Categorize(files, frameworks);

            Assert.True(compat.Contains(NuGetFramework.Parse("net472")));
            Assert.True(file.Contains(FrameworkConstants.CommonFrameworks.Net2));
            Assert.True(nuspec.Contains(FrameworkConstants.CommonFrameworks.Net45));
            Assert.Equal(file.Count, 1);
            Assert.Equal(nuspec.Count, 1);
            Assert.Equal(compat.Count, 1);
        }

        [Fact]
        public void Categorize_PackageWithNoFilesOnlyDependencies_ShouldHaveNuspecNotEmpty()
        {
            //Arrange
            var frameworks = new NuGetFramework[]
            {
                FrameworkConstants.CommonFrameworks.Net35,
                FrameworkConstants.CommonFrameworks.Net4,
                FrameworkConstants.CommonFrameworks.Net45,
                FrameworkConstants.CommonFrameworks.NetStandard10,
                FrameworkConstants.CommonFrameworks.NetStandard13,
                FrameworkConstants.CommonFrameworks.NetStandard20,
            };

            var files = Array.Empty<string>();

            // Act
            var rule = new DependenciesGroupsForEachTFMRule();
            var (compat, file, nuspec) = rule.Categorize(files, frameworks);

            Assert.Empty(compat);
            Assert.Empty(file);
            Assert.True(nuspec.Contains(FrameworkConstants.CommonFrameworks.Net35));
            Assert.True(nuspec.Contains(FrameworkConstants.CommonFrameworks.Net4));
            Assert.True(nuspec.Contains(FrameworkConstants.CommonFrameworks.Net45));
            Assert.True(nuspec.Contains(FrameworkConstants.CommonFrameworks.NetStandard10));
            Assert.True(nuspec.Contains(FrameworkConstants.CommonFrameworks.NetStandard13));
            Assert.True(nuspec.Contains(FrameworkConstants.CommonFrameworks.NetStandard20));
            Assert.Equal(nuspec.Count, 6);
        }

        [Fact]
        public void Categorize_PackageWithNoDependenciesOnlyFiles_ShouldHaveFileNotEmpty()
        {
            //Arrange
            var frameworks = Array.Empty<NuGetFramework>();

            var files = new string[]
            {
                "lib/net35/test.dll",
                "lib/net40/test.dll",
                "lib/net45/test.dll",
                "lib/netstandard1.0/test.dll",
                "lib/netstandard1.3/test.dll",
                "lib/netstandard2.0/test.dll",
            };


            // Act
            var rule = new DependenciesGroupsForEachTFMRule();
            var (compat, file, nuspec) = rule.Categorize(files, frameworks);

            Assert.Empty(compat);
            Assert.True(file.Contains(FrameworkConstants.CommonFrameworks.Net35));
            Assert.True(file.Contains(FrameworkConstants.CommonFrameworks.Net4));
            Assert.True(file.Contains(FrameworkConstants.CommonFrameworks.Net45));
            Assert.True(file.Contains(FrameworkConstants.CommonFrameworks.NetStandard10));
            Assert.True(file.Contains(FrameworkConstants.CommonFrameworks.NetStandard13));
            Assert.True(file.Contains(FrameworkConstants.CommonFrameworks.NetStandard20));
            Assert.Equal(file.Count, 6);
            Assert.Empty(nuspec);
        }
    }
}
