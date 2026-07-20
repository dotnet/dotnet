// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Commands.Test.RestoreCommandTests
{
    public class MonoAndroidDeprecationTests
    {
        #region ShouldCheck tests

        [Theory]
        [InlineData("net11.0-android35.0", "11.0.100", true, true, "net11.0-android with SdkLevel 11")]
        [InlineData("net12.0-android35.0", "11.0.100", true, true, "net12.0-android with SdkLevel 11")]
        [InlineData("net10.0-android35.0", "11.0.100", true, false, "net10.0-android below version threshold")]
        [InlineData("net6.0-android31.0", "11.0.100", true, false, "net6.0-android below version threshold")]
        [InlineData("net11.0-android35.0", "10.0.100", true, false, "SdkAnalysisLevel too old")]
        [InlineData("net11.0-ios18.0", "11.0.100", true, false, "iOS platform, not android")]
        [InlineData("net11.0", "11.0.100", true, false, "no platform")]
        public void ShouldCheck_WithSdkAnalysisLevel_ReturnsExpected(
            string frameworkString,
            string sdkAnalysisLevel,
            bool usingMicrosoftNETSdk,
            bool expected,
            string because)
        {
            var spec = CreatePackageSpec(frameworkString);
            spec.RestoreMetadata.SdkAnalysisLevel = NuGetVersion.Parse(sdkAnalysisLevel);
            spec.RestoreMetadata.UsingMicrosoftNETSdk = usingMicrosoftNETSdk;
            var framework = NuGetFramework.Parse(frameworkString);

            MonoAndroidDeprecation.ShouldCheck(spec, framework).Should().Be(expected, because);
        }

        [Fact]
        public void ShouldCheck_NullRestoreMetadata_ReturnsFalse()
        {
            var spec = new PackageSpec();
            spec.RestoreMetadata = null;
            var framework = NuGetFramework.Parse("net11.0-android35.0");

            MonoAndroidDeprecation.ShouldCheck(spec, framework).Should().BeFalse();
        }

        [Fact]
        public void ShouldCheck_NullSdkAnalysisLevel_UsingMicrosoftNETSdk_ReturnsFalse()
        {
            var spec = CreatePackageSpec("net11.0-android35.0");
            spec.RestoreMetadata.SdkAnalysisLevel = null;
            spec.RestoreMetadata.UsingMicrosoftNETSdk = true;
            var framework = NuGetFramework.Parse("net11.0-android35.0");

            MonoAndroidDeprecation.ShouldCheck(spec, framework).Should().BeFalse();
        }

        [Fact]
        public void ShouldCheck_NullSdkAnalysisLevel_NotUsingMicrosoftNETSdk_ReturnsTrue()
        {
            var spec = CreatePackageSpec("net11.0-android35.0");
            spec.RestoreMetadata.SdkAnalysisLevel = null;
            spec.RestoreMetadata.UsingMicrosoftNETSdk = false;
            var framework = NuGetFramework.Parse("net11.0-android35.0");

            MonoAndroidDeprecation.ShouldCheck(spec, framework).Should().BeTrue();
        }

        #endregion

        #region IsMonoAndroidFramework tests

        [Fact]
        public void IsMonoAndroidFramework_MonoAndroid_ReturnsTrue()
        {
            var framework = NuGetFramework.Parse("monoandroid10.0");

            MonoAndroidDeprecation.IsMonoAndroidFramework(framework).Should().BeTrue();
        }

        [Fact]
        public void IsMonoAndroidFramework_MonoAndroidNoVersion_ReturnsTrue()
        {
            var framework = new NuGetFramework(FrameworkConstants.FrameworkIdentifiers.MonoAndroid);

            MonoAndroidDeprecation.IsMonoAndroidFramework(framework).Should().BeTrue();
        }

        [Fact]
        public void IsMonoAndroidFramework_NetCoreApp_ReturnsFalse()
        {
            var framework = NuGetFramework.Parse("net6.0-android31.0");

            MonoAndroidDeprecation.IsMonoAndroidFramework(framework).Should().BeFalse();
        }

        [Fact]
        public void IsMonoAndroidFramework_NetStandard_ReturnsFalse()
        {
            var framework = NuGetFramework.Parse("netstandard2.0");

            MonoAndroidDeprecation.IsMonoAndroidFramework(framework).Should().BeFalse();
        }

        [Fact]
        public void IsMonoAndroidFramework_Null_ReturnsFalse()
        {
            MonoAndroidDeprecation.IsMonoAndroidFramework(null).Should().BeFalse();
        }

        #endregion

        #region ShouldWarn tests

        [Fact]
        public void ShouldWarn_MonoAndroidWithOnlyEmptyFolderPlaceholders_ReturnsFalse()
        {
            var monoAndroid = NuGetFramework.Parse("monoandroid10.0");
            var compileItems = new[]
            {
                new LockFileItem("ref/MonoAndroid10/_._")
            };
            var runtimeItems = new[]
            {
                new LockFileItem("lib/MonoAndroid10/_._")
            };

            MonoAndroidDeprecation.ShouldWarn(
                monoAndroid,
                compileItems,
                monoAndroid,
                runtimeItems).Should().BeFalse();
        }

        #endregion

        #region Integration tests

        [Fact]
        public async Task Restore_Net11Android_MonoAndroidPackage_SdkLevel11_EmitsNU1703()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            var packageA = new SimpleTestPackageContext("a", "1.0.0");
            packageA.AddFile("lib/monoandroid10.0/a.dll");

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageA);

            var spec = ProjectTestHelpers.GetPackageSpec("Project1",
                pathContext.SolutionRoot,
                framework: "net11.0-android35.0",
                dependencyName: "a");
            spec.RestoreMetadata.SdkAnalysisLevel = NuGetVersion.Parse("11.0.100");
            spec.RestoreMetadata.UsingMicrosoftNETSdk = true;

            var logger = new TestLogger();
            var command = new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, spec));

            // Act
            var result = await command.ExecuteAsync();

            // Assert
            result.Success.Should().BeTrue(because: logger.ShowMessages());
            result.LockFile.LogMessages.Should().HaveCount(1);
            result.LockFile.LogMessages[0].Code.Should().Be(NuGetLogCode.NU1703);
            result.LockFile.LogMessages[0].Level.Should().Be(LogLevel.Warning);
            result.LockFile.LogMessages[0].LibraryId.Should().Be("a");
            result.LockFile.LogMessages[0].Message.Should().Contain("MonoAndroid");
            result.LockFile.LogMessages[0].TargetGraphs.Should().HaveCount(1);
            result.LockFile.LogMessages[0].TargetGraphs[0].Should().Be("net11.0-android35.0");
            logger.Errors.Should().Be(0);
            logger.Warnings.Should().Be(1);
        }

        [Fact]
        public async Task Restore_Net11Android_MultiplePackages_OnlyMonoAndroidPackageGetsNU1703()
        {
            // One package with monoandroid, one with netstandard - only monoandroid package should warn
            using var pathContext = new SimpleTestPathContext();

            var packageA = new SimpleTestPackageContext("a", "1.0.0");
            packageA.AddFile("lib/monoandroid10.0/a.dll");

            var packageB = new SimpleTestPackageContext("b", "2.0.0");
            packageB.AddFile("lib/netstandard2.0/b.dll");

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageA,
                packageB);

            var spec = ProjectTestHelpers.GetPackageSpec("Project1",
                pathContext.SolutionRoot,
                framework: "net11.0-android35.0",
                dependencyName: "a");
            // Add second dependency
            var tfi = spec.TargetFrameworks[0];
            tfi.Dependencies.Add(new LibraryModel.LibraryDependency
            {
                LibraryRange = new LibraryModel.LibraryRange("b", VersionRange.Parse("2.0.0"), LibraryModel.LibraryDependencyTarget.Package)
            });

            spec.RestoreMetadata.SdkAnalysisLevel = NuGetVersion.Parse("11.0.100");
            spec.RestoreMetadata.UsingMicrosoftNETSdk = true;

            var logger = new TestLogger();
            var command = new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, spec));

            // Act
            var result = await command.ExecuteAsync();

            // Assert
            result.Success.Should().BeTrue(because: logger.ShowMessages());
            result.LockFile.LogMessages.Where(m => m.Code == NuGetLogCode.NU1703).Should().HaveCount(1);
            result.LockFile.LogMessages.Single(m => m.Code == NuGetLogCode.NU1703).LibraryId.Should().Be("a");
            logger.Warnings.Should().Be(1);
        }

        [Fact]
        public async Task Restore_Net11Android_MonoAndroidPackage_SdkLevel11_NU1703_PackageLevelSuppression()
        {
            // NoWarn on the package dependency itself should suppress the warning
            using var pathContext = new SimpleTestPathContext();

            var packageA = new SimpleTestPackageContext("a", "1.0.0");
            packageA.AddFile("lib/monoandroid10.0/a.dll");

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageA);

            var spec = ProjectTestHelpers.GetPackageSpec("Project1",
                pathContext.SolutionRoot,
                framework: "net11.0-android35.0",
                dependencyName: "a");
            spec.RestoreMetadata.SdkAnalysisLevel = NuGetVersion.Parse("11.0.100");
            spec.RestoreMetadata.UsingMicrosoftNETSdk = true;

            // Reconstruct the dependency with package-level NoWarn for NU1703
            var oldTfi = spec.TargetFrameworks[0];
            var oldDep = oldTfi.Dependencies[0];
            var newDep = new LibraryModel.LibraryDependency(oldDep)
            {
                NoWarn = System.Collections.Immutable.ImmutableArray.Create(NuGetLogCode.NU1703)
            };
            spec.TargetFrameworks[0] = new TargetFrameworkInformation(oldTfi)
            {
                Dependencies = System.Collections.Immutable.ImmutableArray.Create(newDep)
            };

            var logger = new TestLogger();
            var command = new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, spec));

            // Act
            var result = await command.ExecuteAsync();

            // Assert
            result.Success.Should().BeTrue(because: logger.ShowMessages());
            result.LockFile.LogMessages.Should().NotContain(m => m.Code == NuGetLogCode.NU1703);
            logger.Warnings.Should().Be(0);
        }

        #endregion

        #region Helpers

        private static PackageSpec CreatePackageSpec(string framework)
        {
            var spec = ProjectTestHelpers.GetPackageSpec("TestProject", @"C:\", framework);
            return spec;
        }

        #endregion
    }
}
