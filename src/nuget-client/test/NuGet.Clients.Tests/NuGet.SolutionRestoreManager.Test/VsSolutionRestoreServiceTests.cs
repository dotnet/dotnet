// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using Moq;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.PackageManagement.VisualStudio;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
using NuGet.Versioning;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Telemetry;
using Test.Utility.ProjectManagement;
using Xunit;
using static NuGet.Frameworks.FrameworkConstants;

namespace NuGet.SolutionRestoreManager.Test
{
    [Collection(DispatcherThreadCollection.CollectionName)]
    public class VsSolutionRestoreServiceTests
    {
        public VsSolutionRestoreServiceTests()
        {
#pragma warning disable VSSDK005 // Avoid instantiating JoinableTaskContext
            var joinableTaskContext = new JoinableTaskContext(Thread.CurrentThread, SynchronizationContext.Current);
#pragma warning restore VSSDK005 // Avoid instantiating JoinableTaskContext

            NuGetUIThreadHelper.SetCustomJoinableTaskFactory(joinableTaskContext.Factory);
        }

        [Fact]
        public void NominateProjectAsync_Always_SchedulesAutoRestore()
        {
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netcoreapp1.0")
                .Build();

            var restoreWorker = CreateDefaultISolutionRestoreWorkerMock();

            // Act
            var actualRestoreTask = NominateProjectAsync(projectRestoreInfo, CancellationToken.None, restoreWorker: restoreWorker);

            restoreWorker
                .Verify(
                    x => x.ScheduleRestoreAsync(It.IsAny<SolutionRestoreRequest>(), CancellationToken.None),
                    Times.Once(),
                    "Service should schedule auto-restore operation.");
        }

        [Fact]
        public async Task NominateProjectAsync_ConsoleAppTemplate()
        {
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netcoreapp1.0", builder =>
                {
                    builder
                    .WithProperty("OutputType", "exe")
                    .WithItem(ProjectItems.PackageReference, "Microsoft.NET.Sdk", [new(ProjectBuildProperties.Version, "1.0.0-alpha-20161019-1")])
                    .WithItem(ProjectItems.PackageReference, "Microsoft.NETCore.App", [new(ProjectBuildProperties.Version, "1.0.1")]);
                })
                .Build();
            var expectedBaseIntermediate = projectRestoreInfo.MSBuildProjectExtensionsPath;

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.Projects.Single();
            Assert.Equal("1.0.0", actualProjectSpec.Version.ToString());

            var actualMetadata = actualProjectSpec.RestoreMetadata;
            Assert.NotNull(actualMetadata);
            Assert.Equal(ProjectStyle.PackageReference, actualMetadata.ProjectStyle);
            Assert.Equal(expectedBaseIntermediate, actualMetadata.OutputPath);

            Assert.Single(actualProjectSpec.TargetFrameworks);
            var actualTfi = actualProjectSpec.TargetFrameworks.Single();

            var expectedFramework = NuGetFramework.Parse("netcoreapp1.0");
            Assert.Equal(expectedFramework, actualTfi.FrameworkName);

            AssertPackages(actualTfi,
                "Microsoft.NET.Sdk:1.0.0-alpha-20161019-1",
                "Microsoft.NETCore.App:1.0.1");
        }

        [Fact]
        public async Task NominateProjectAsync_ConsoleAppTemplateWithPlatform()
        {
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("net5.0-windows10.0", builder: builder =>
                {
                    builder
                    .WithProperty("OutputType", "exe")
                    .WithProperty(ProjectBuildProperties.TargetFramework, "net5.0-windows")
                    .WithItem(ProjectItems.PackageReference, "Microsoft.NET.Sdk", [new(ProjectBuildProperties.Version, "1.0.0-alpha-20161019-1")])
                    .WithItem(ProjectItems.PackageReference, "Microsoft.NETCore.App", [new(ProjectBuildProperties.Version, "1.0.1")]);
                })
                .Build();
            var expectedBaseIntermediate = projectRestoreInfo.MSBuildProjectExtensionsPath;

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.Projects.Single();
            Assert.Equal("1.0.0", actualProjectSpec.Version.ToString());

            var actualMetadata = actualProjectSpec.RestoreMetadata;
            Assert.NotNull(actualMetadata);
            Assert.Equal(ProjectStyle.PackageReference, actualMetadata.ProjectStyle);
            Assert.Equal(expectedBaseIntermediate, actualMetadata.OutputPath);

            Assert.Single(actualProjectSpec.TargetFrameworks);
            var actualTfi = actualProjectSpec.TargetFrameworks.Single();

            var expectedFramework = NuGetFramework.Parse("net5.0-windows10.0");
            Assert.Equal(expectedFramework, actualTfi.FrameworkName);
            Assert.Equal("net5.0-windows", actualTfi.TargetAlias);

            AssertPackages(actualTfi,
                "Microsoft.NET.Sdk:1.0.0-alpha-20161019-1",
                "Microsoft.NETCore.App:1.0.1");
        }

        [Fact]
        public async Task NominateProjectAsync_WithCliTool()
        {
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netcoreapp1.0")
                .WithTool("Foo.Test.Tools", "2.0.0")
                .Build();

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var projectFullPath = projectRestoreInfo.TargetFrameworks[0].Properties["MSBuildProjectFullPath"];
            var actualProjectSpec = actualRestoreSpec.GetProjectSpec(projectFullPath);

            var actualToolSpec = actualRestoreSpec
                .Projects
                .Single(p => !object.ReferenceEquals(p, actualProjectSpec));
            var actualMetadata = actualToolSpec.RestoreMetadata;
            Assert.NotNull(actualMetadata);
            Assert.Equal(ProjectStyle.DotnetCliTool, actualMetadata.ProjectStyle);
            Assert.Null(actualMetadata.OutputPath);

            var actualToolFramework = actualToolSpec
                .TargetFrameworks
                .Single()
                .FrameworkName;
            Assert.Equal(CommonFrameworks.NetCoreApp10, actualToolFramework);

            var actualToolLibrary = actualToolSpec
                .TargetFrameworks
                .Single()
                .Dependencies
                .Single();
            Assert.Equal("Foo.Test.Tools", actualToolLibrary.Name);
            Assert.Equal("2.0.0", actualToolLibrary.LibraryRange.VersionRange?.OriginalString);
        }

        [Theory]
        [InlineData("netcoreapp2.0", @"..\packages", @"..\source1;..\source2", @"..\fallback1;..\fallback2")]
        [InlineData("netcoreapp2.0", @"C:\packagesPath", @"..\source1;..\source2", @"C:\fallback1;C:\fallback2")]
        [InlineData("netcoreapp2.0", null, null, null)]
        [InlineData("netcoreapp1.0", null, null, null)]
        public async Task NominateProjectAsync_WithCliTool_RestoreSettings(
            string toolFramework, string restorePackagesPath, string restoreSources, string fallbackFolders)
        {
            var expectedToolFramework = NuGetFramework.Parse(toolFramework);

            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo(toolFramework, builder =>
                {
                    builder
                    .WithProperty("RestorePackagesPath", restorePackagesPath)
                    .WithProperty("RestoreSources", restoreSources)
                    .WithProperty("RestoreFallbackFolders", fallbackFolders)
                    .WithProperty("DotnetCliToolTargetFramework", toolFramework);
                })
                .WithTool("Foo.Test.Tools", "2.0.0")
                .Build();

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var projectFullPath = projectRestoreInfo.TargetFrameworks[0].Properties["MSBuildProjectFullPath"];
            var actualProjectSpec = actualRestoreSpec.GetProjectSpec(projectFullPath);

            var actualToolSpec = actualRestoreSpec
                .Projects
                .Single(p => !object.ReferenceEquals(p, actualProjectSpec));
            var actualMetadata = actualToolSpec.RestoreMetadata;
            Assert.NotNull(actualMetadata);
            Assert.Equal(ProjectStyle.DotnetCliTool, actualMetadata.ProjectStyle);
            Assert.Null(actualMetadata.OutputPath);

            var actualToolFramework = actualToolSpec
                .TargetFrameworks
                .Single()
                .FrameworkName;
            Assert.Equal(expectedToolFramework, actualToolFramework);

            var actualToolLibrary = actualToolSpec
                .TargetFrameworks
                .Single()
                .Dependencies
                .Single();
            Assert.Equal("Foo.Test.Tools", actualToolLibrary.Name);
            Assert.Equal("2.0.0", actualToolLibrary.LibraryRange.VersionRange?.OriginalString);

            Assert.Equal(restorePackagesPath, actualToolSpec.RestoreMetadata.PackagesPath);

            var specSources = actualToolSpec.RestoreMetadata.Sources?.Select(e => e.Source);
            var expectedSources = MSBuildStringUtility.Split(restoreSources);
            Assert.True(Enumerable.SequenceEqual(expectedSources.OrderBy(t => t), specSources.OrderBy(t => t)));

            var specFallback = actualToolSpec.RestoreMetadata.FallbackFolders;
            var expectedFallback = MSBuildStringUtility.Split(fallbackFolders);
            Assert.True(Enumerable.SequenceEqual(expectedFallback.OrderBy(t => t), specFallback.OrderBy(t => t)));
        }

        [Theory]
        [InlineData("netstandard1.4")]
        [InlineData("netstandard1.4;net46")]
        public async Task NominateProjectAsync_CrossTargeting(string targetFrameworks)
        {
            var builder = new TestProjectRestoreInfoBuilder();
            var splitTargetFrameworks = targetFrameworks.Split([';'], StringSplitOptions.RemoveEmptyEntries);
            foreach (var targetFramework in splitTargetFrameworks)
            {
                builder.WithTargetFrameworkInfo(targetFramework);
            }

            var projectRestoreInfo = builder.Build();

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.Projects.Single();

            var actualMetadata = actualProjectSpec.RestoreMetadata;
            Assert.NotNull(actualMetadata);
            Assert.True(actualMetadata.CrossTargeting);

            Assert.Equal(targetFrameworks, projectRestoreInfo.OriginalTargetFrameworks);
        }

        [Theory]
        [InlineData(ProjectBuildProperties.PackageTargetFallback)]
        [InlineData(ProjectBuildProperties.AssetTargetFallback)]
        public async Task NominateProjectAsync_Imports(string propertyName)
        {
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netstandard1.4", builder =>
                {
                    builder.WithProperty(propertyName, "dotnet5.3;portable-net452+win81");
                })
                .Build();

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.Projects.Single();

            var actualTfi = actualProjectSpec.TargetFrameworks.Single();
            var actualImports = string.Join(";", actualTfi.Imports.Select(x => x.GetShortFolderName()));
            Assert.Equal("dotnet53;portable-net452+win81", actualImports);
        }

        [Fact]
        public async Task NominateProjectAsync_WithValidPackageVersion()
        {
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netcoreapp1.0", builder =>
                {
                    builder.WithProperty(ProjectBuildProperties.PackageVersion, "1.2.0-beta1");
                })
                .Build();

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.Projects.Single();
            Assert.Equal("1.2.0-beta1", actualProjectSpec.Version.ToString());
        }

        [Theory]
        [InlineData("1.2.3", "1.2.3")]
        [InlineData("1.2.0-beta1", "1.2.0-beta1")]
        [InlineData("1.0.0", "1.0.0.0")]
        public async Task NominateProjectAsync_WithIdenticalPackageVersions(string version1, string version2)
        {
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netcoreapp1.0", builder =>
                {
                    builder.WithProperty(ProjectBuildProperties.PackageVersion, version1);
                })
                .WithTargetFrameworkInfo("net46", builder =>
                {
                    builder.WithProperty(ProjectBuildProperties.PackageVersion, version2);
                })
                .Build();

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.Projects.Single();
            Assert.Equal(version1, actualProjectSpec.Version.ToString());
        }

        // The data in the restore settings should be unprocessed meaning the paths should stay relative.
        // The processing of the paths will be done later in the netcorepackagereferenceproject
        [Theory]
        [InlineData(@"..\packages", @"..\source1;..\source2", @"..\fallback1;..\fallback2")]
        [InlineData(@"C:\packagesPath", @"..\source1;..\source2", @"C:\fallback1;C:\fallback2")]
        [InlineData(null, null, null)]
        public async Task NominateProjectAsync_RestoreSettings(string restorePackagesPath, string restoreSources, string fallbackFolders)
        {
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netcoreapp1.0", builder =>
                {
                    builder
                    .WithProperty("RestorePackagesPath", restorePackagesPath)
                    .WithProperty("RestoreSources", restoreSources)
                    .WithProperty("RestoreFallbackFolders", fallbackFolders);
                })
                .Build();

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.Projects.Single();
            Assert.Equal(restorePackagesPath, actualProjectSpec.RestoreMetadata.PackagesPath);

            var specSources = actualProjectSpec.RestoreMetadata.Sources?.Select(e => e.Source);
            var expectedSources = MSBuildStringUtility.Split(restoreSources);
            Assert.True(Enumerable.SequenceEqual(expectedSources.OrderBy(t => t), specSources.OrderBy(t => t)));

            var specFallback = actualProjectSpec.RestoreMetadata.FallbackFolders;
            var expectedFallback = MSBuildStringUtility.Split(fallbackFolders);
            Assert.True(Enumerable.SequenceEqual(expectedFallback.OrderBy(t => t), specFallback.OrderBy(t => t)));
        }

        [Theory]
        [InlineData(@"..\packages", @"..\source1;..\source2", @"..\fallback1;Clear;..\fallback2")]
        [InlineData(@"C:\packagesPath", @"Clear;..\source1;..\source2", @"C:\fallback1;C:\fallback2")]
        public async Task NominateProjectAsync_RestoreSettingsClear(string restorePackagesPath, string restoreSources, string fallbackFolders)
        {
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netcoreapp1.0", builder =>
                {
                    builder
                    .WithProperty("RestorePackagesPath", restorePackagesPath)
                    .WithProperty("RestoreSources", restoreSources)
                    .WithProperty("RestoreFallbackFolders", fallbackFolders);
                })
                .Build();

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectRestoreInfo);
            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.Projects.Single();
            Assert.Equal(restorePackagesPath, actualProjectSpec.RestoreMetadata.PackagesPath);

            var specSources = actualProjectSpec.RestoreMetadata.Sources?.Select(e => e.Source);
            var expectedSources = MSBuildStringUtility.Split(restoreSources).Any(e => StringComparer.OrdinalIgnoreCase.Equals("clear", e)) ? new string[] { "Clear" } : MSBuildStringUtility.Split(restoreSources);
            Assert.True(Enumerable.SequenceEqual(expectedSources.OrderBy(t => t), specSources.OrderBy(t => t)));

            var specFallback = actualProjectSpec.RestoreMetadata.FallbackFolders;
            var expectedFallback = MSBuildStringUtility.Split(fallbackFolders).Any(e => StringComparer.OrdinalIgnoreCase.Equals("clear", e)) ? new string[] { "Clear" } : MSBuildStringUtility.Split(fallbackFolders);
            Assert.True(Enumerable.SequenceEqual(expectedFallback.OrderBy(t => t), specFallback.OrderBy(t => t)));
        }

        [Fact]
        public async Task NominateProjectAsync_CacheFilePathInPackageSpec_Succeeds()
        {
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netcoreapp1.0")
                .Build();

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.Projects.Single();
            Assert.Equal(Path.Combine(actualProjectSpec.RestoreMetadata.OutputPath, NoOpRestoreUtilities.NoOpCacheFileName), actualProjectSpec.RestoreMetadata.CacheFilePath);
        }

        [Theory]
        [InlineData("1.0.0", "1.2.3")]
        [InlineData("1.0.0", "")]
        [InlineData("1.0.0", "   ")]
        [InlineData("1.0.0", null)]
        public async Task NominateProjectAsync_WithDifferentPackageVersions_Fails(string version1, string version2)
        {
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netcoreapp1.0", builder =>
                {
                    builder.WithProperty(ProjectBuildProperties.PackageVersion, version1);
                })
                .WithTargetFrameworkInfo("net45", builder =>
                {
                    builder.WithProperty(ProjectBuildProperties.PackageVersion, version2);
                })
                .Build();

            var projectFullPath = projectRestoreInfo.TargetFrameworks[0].Properties["MSBuildProjectFullPath"];

            var cache = Mock.Of<IProjectSystemCache>();
            var restoreWorker = Mock.Of<ISolutionRestoreWorker>();
            var vsSolution2 = Mock.Of<IVsSolution2>();
            var asyncLazySolution2 = new Microsoft.VisualStudio.Threading.AsyncLazy<IVsSolution2>(() => Task.FromResult(vsSolution2));
            var telemetryProvider = Mock.Of<INuGetTelemetryProvider>();

            var service = new VsSolutionRestoreService(
                cache, restoreWorker, NullLogger.Instance, asyncLazySolution2, telemetryProvider);

            // Act
            var result = await ((IVsSolutionRestoreService5)service).NominateProjectAsync(projectFullPath, projectRestoreInfo, CancellationToken.None);

            Assert.False(result, "Project restore nomination must fail.");
        }

        [Fact]
        public async Task NominateProjectAsync_PackageId()
        {
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netcoreapp1.0", builder =>
                {
                    builder.WithProperty(ProjectBuildProperties.PackageId, "TestPackage");
                })
                .Build();

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.Projects.Single();
            Assert.Equal("TestPackage", actualProjectSpec.Name);
        }

        [Fact]
        public async Task DifferingPackageIdsFailsWithUsableErrorMessage()
        {
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netcoreapp1.0", builder =>
                {
                    builder.WithProperty(ProjectBuildProperties.PackageId, "PackageId.netcoreapp1.0");
                })
                .WithTargetFrameworkInfo("net46", builder =>
                {
                    builder.WithProperty(ProjectBuildProperties.PackageId, "PackageId.net46");
                })
                .Build();

            var cache = Mock.Of<IProjectSystemCache>();
            var restoreWorker = Mock.Of<ISolutionRestoreWorker>();
            var vsSolution2 = Mock.Of<IVsSolution2>();
            var asyncLazySolution2 = new Microsoft.VisualStudio.Threading.AsyncLazy<IVsSolution2>(() => Task.FromResult(vsSolution2));
            var telemetryProvider = Mock.Of<INuGetTelemetryProvider>();

            var service = new VsSolutionRestoreService(
                cache, restoreWorker, NuGet.Common.NullLogger.Instance, asyncLazySolution2, telemetryProvider);

            // Act
            var additionalMessages = await CaptureAdditionalMessagesAsync(projectRestoreInfo);

            // Assert
            var additionalMessage = Assert.Single(additionalMessages);
            Assert.Equal(NuGetLogCode.NU1105, additionalMessage.Code);
            additionalMessage.Message.Should().Contain(string.Format(CultureInfo.CurrentCulture, Resources.PropertyDoesNotHaveSingleValue, "PackageId", "PackageId.net46, PackageId.netcoreapp1.0"));
        }

        [Fact]
        public async Task NominateProjectAsync_VerifySourcesAreCombinedAcrossFrameworks()
        {
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netcoreapp1.0", builder =>
                {
                    builder
                    .WithProperty("RestoreSources", "base")
                    .WithProperty("RestoreFallbackFolders", "base")
                    .WithProperty("RestoreAdditionalProjectSources", "a;d")
                    .WithProperty("RestoreAdditionalProjectFallbackFolders", "x;z");
                })
                .WithTargetFrameworkInfo("netcoreapp2.0", builder =>
                {
                    builder
                    .WithProperty("RestoreSources", "base")
                    .WithProperty("RestoreFallbackFolders", "base")
                    .WithProperty("RestoreAdditionalProjectSources", "b")
                    .WithProperty("RestoreAdditionalProjectFallbackFolders", "y");
                })
                .WithTargetFrameworkInfo("netcoreapp2.1", builder =>
                {
                    builder
                    .WithProperty("RestoreSources", "base")
                    .WithProperty("RestoreFallbackFolders", "base")
                    .WithProperty("RestoreAdditionalProjectSources", "b")
                    .WithProperty("RestoreAdditionalProjectFallbackFolders", "y");
                })
                .Build();

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectRestoreInfo);
            var metadata = actualRestoreSpec.Projects.Single().RestoreMetadata;

            // Assert
            metadata.Sources.Select(e => e.Source).Should().BeEquivalentTo(new[] { "base", VSRestoreSettingsUtilities.AdditionalValue, "a", "b", "d" });
            metadata.FallbackFolders.Should().BeEquivalentTo(new[] { "base", VSRestoreSettingsUtilities.AdditionalValue, "x", "y", "z" });
        }

        [Fact]
        public async Task NominateProjectAsync_VerifyExcludedFallbackFoldersAreRemoved()
        {
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netcoreapp1.0", builder =>
                {
                    builder
                    .WithProperty("RestoreSources", "base")
                    .WithProperty("RestoreFallbackFolders", "base")
                    .WithProperty("RestoreAdditionalProjectFallbackFolders", "x"); // Add FF
                })
                .WithTargetFrameworkInfo("netcoreapp2.0", builder =>
                {
                    builder
                    .WithProperty("RestoreSources", "base")
                    .WithProperty("RestoreFallbackFolders", "base")
                    .WithProperty("RestoreAdditionalProjectSources", "a")
                    .WithProperty("RestoreAdditionalProjectFallbackFoldersExcludes", "x"); // Remove FF
                })
                .Build();

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectRestoreInfo);
            var metadata = actualRestoreSpec.Projects.Single().RestoreMetadata;

            // Assert
            metadata.Sources.Select(e => e.Source).Should().BeEquivalentTo(new[] { "base", VSRestoreSettingsUtilities.AdditionalValue, "a" });
            metadata.FallbackFolders.Should().BeEquivalentTo(new[] { "base" });
        }

        [Fact]
        public async Task NominateProjectAsync_VerifyToolRestoresUseAdditionalSourcesAndFallbackFolders()
        {
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTool("ToolTest", "2.0.0")
                .WithTargetFrameworkInfo("netcoreapp1.0", builder =>
                {
                    builder
                    .WithProperty("RestoreSources", "base")
                    .WithProperty("RestoreFallbackFolders", "base")
                    .WithProperty("RestoreAdditionalProjectFallbackFolders", "x;y");
                })
                .WithTargetFrameworkInfo("netcoreapp2.0", builder =>
                {
                    builder
                    .WithProperty("RestoreSources", "base")
                    .WithProperty("RestoreFallbackFolders", "base")
                    .WithProperty("RestoreAdditionalProjectSources", "a")
                    .WithProperty("RestoreAdditionalProjectFallbackFoldersExcludes", "y");
                })
                .Build();

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectRestoreInfo);

            // Find the tool spec
            var metadata = actualRestoreSpec.Projects.Single(e => e.RestoreMetadata.ProjectStyle == ProjectStyle.DotnetCliTool).RestoreMetadata;

            // Assert
            metadata.Sources.Select(e => e.Source).Should().BeEquivalentTo(new[] { "base", VSRestoreSettingsUtilities.AdditionalValue, "a" });
            metadata.FallbackFolders.Should().BeEquivalentTo(new[] { "base", VSRestoreSettingsUtilities.AdditionalValue, "x" });
        }

        [Theory]
        [InlineData(@"..\source1", @"..\additionalsource", @"..\fallback1", @"..\additionalFallback1")]
        [InlineData(@"..\source1", null, @"..\fallback1", null)]
        [InlineData(null, @"..\additionalsource", null, @"..\additionalFallback1")]
        [InlineData(@"Clear;C:\source1", @"C:\additionalsource", @"C:\fallback1;Clear", @"C:\additionalFallback1")]
        [InlineData(@"C:\source1;Clear", @"C:\additionalsource", @"Clear;C:\fallback1", @"C:\additionalFallback1")]
        public async Task NominateProjectAsync_WithRestoreAdditionalSourcesAndFallbackFolders(string restoreSources, string restoreAdditionalProjectSources, string restoreFallbackFolders, string restoreAdditionalFallbackFolders)
        {
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netcoreapp2.0", builder =>
                {
                    builder
                    .WithProperty("RestoreSources", restoreSources)
                    .WithProperty("RestoreFallbackFolders", restoreFallbackFolders)
                    .WithProperty("RestoreAdditionalProjectSources", restoreAdditionalProjectSources)
                    .WithProperty("RestoreAdditionalProjectFallbackFolders", restoreAdditionalFallbackFolders);
                })
                .Build();

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.Projects.Single();

            var specSources = actualProjectSpec.RestoreMetadata.Sources?.Select(e => e.Source);

            var expectedSources =
                (MSBuildStringUtility.Split(restoreSources).Any(e => StringComparer.OrdinalIgnoreCase.Equals("clear", e)) ?
                    new string[] { "Clear" } :
                    MSBuildStringUtility.Split(restoreSources)).
                Concat(
                restoreAdditionalProjectSources != null ?
                    new List<string>() { VSRestoreSettingsUtilities.AdditionalValue }.Concat(MSBuildStringUtility.Split(restoreAdditionalProjectSources)) :
                    new string[] { }
                );

            Assert.True(Enumerable.SequenceEqual(expectedSources.OrderBy(t => t), specSources.OrderBy(t => t)));

            var specFallback = actualProjectSpec.RestoreMetadata.FallbackFolders;

            var expectedFallback =
                (MSBuildStringUtility.Split(restoreFallbackFolders).Any(e => StringComparer.OrdinalIgnoreCase.Equals("clear", e)) ?
                    new string[] { "Clear" } :
                    MSBuildStringUtility.Split(restoreFallbackFolders)).
                Concat(
                restoreAdditionalFallbackFolders != null ?
                    new List<string>() { VSRestoreSettingsUtilities.AdditionalValue }.Concat(MSBuildStringUtility.Split(restoreAdditionalFallbackFolders)) :
                    new string[] { }
                );

            Assert.True(
                Enumerable.SequenceEqual(expectedFallback.OrderBy(t => t), specFallback.OrderBy(t => t)),
                "expected: " + string.Join(",", expectedFallback.ToArray()) + "\nactual: " + string.Join(",", specFallback.ToArray()));
        }


        [Theory]
        [InlineData(@"C:\source1", @"C:\additionalsource", @"C:\source1;C:\additionalsource", @"C:\fallback1", @"C:\additionalFallback1", @"C:\fallback1;C:\additionalFallback1")]
        [InlineData(@"C:\source1", null, @"C:\source1", @"C:\fallback1", null, @"C:\fallback1")]
        [InlineData(null, @"C:\additionalsource", @"C:\additionalsource", null, @"C:\additionalFallback1", @"C:\additionalFallback1")]
        [InlineData(@"Clear;C:\source1", @"C:\additionalsource", @"C:\additionalsource", @"C:\fallback1;Clear", @"C:\additionalFallback1", @"C:\additionalFallback1")]
        [InlineData(@"C:\source1;Clear", @"C:\additionalsource", @"C:\additionalsource", @"Clear;C:\fallback1", @"C:\additionalFallback1", @"C:\additionalFallback1")]
        public async Task VSSolutionRestoreService_VSRestoreSettingsUtilities_Integration(string restoreSources, string restoreAdditionalProjectSources, string expectedRestoreSources, string restoreFallbackFolders, string restoreAdditionalFallbackFolders, string expectedFallbackFolders)
        {
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netcoreapp2.0", builder =>
                {
                    builder
                    .WithProperty("RestoreSources", restoreSources)
                    .WithProperty("RestoreFallbackFolders", restoreFallbackFolders)
                    .WithProperty("RestoreAdditionalProjectSources", restoreAdditionalProjectSources)
                    .WithProperty("RestoreAdditionalProjectFallbackFolders", restoreAdditionalFallbackFolders);
                })
                .Build();

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.Projects.Single();

            var specSources = VSRestoreSettingsUtilities.GetSources(NullSettings.Instance, actualProjectSpec).Select(e => e.Source);

            var expectedSources = MSBuildStringUtility.Split(expectedRestoreSources);

            specSources.OrderBy(t => t).Should().BeEquivalentTo(expectedSources.OrderBy(t => t));

            var specFallback = VSRestoreSettingsUtilities.GetFallbackFolders(NullSettings.Instance, actualProjectSpec);

            var expectedFallback = MSBuildStringUtility.Split(expectedFallbackFolders);

            Assert.True(
                Enumerable.SequenceEqual(expectedFallback.OrderBy(t => t), specFallback.OrderBy(t => t)),
                "expected: " + string.Join(",", expectedFallback.ToArray()) + "\nactual: " + string.Join(",", specFallback.ToArray()));
        }

        [Theory]
        [InlineData("true", null, "false")]
        [InlineData(null, "packages.A.lock.json", null)]
        [InlineData("true", null, "true")]
        [InlineData("false", null, "false")]
        public async Task NominateProjectAsync_LockFileSettings(
            string restorePackagesWithLockFile,
            string lockFilePath,
            string restoreLockedMode)
        {
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netcoreapp1.0", builder =>
                {
                    builder
                    .WithProperty("RestorePackagesWithLockFile", restorePackagesWithLockFile)
                    .WithProperty("NuGetLockFilePath", lockFilePath)
                    .WithProperty("RestoreLockedMode", restoreLockedMode);
                })
                .Build();

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.Projects.Single();
            Assert.Equal(restorePackagesWithLockFile, actualProjectSpec.RestoreMetadata.RestoreLockProperties.RestorePackagesWithLockFile);
            Assert.Equal(lockFilePath, actualProjectSpec.RestoreMetadata.RestoreLockProperties.NuGetLockFilePath);
            Assert.Equal(MSBuildStringUtility.IsTrue(restoreLockedMode), actualProjectSpec.RestoreMetadata.RestoreLockProperties.RestoreLockedMode);
        }

        [Fact]
        public async Task NominateProjectAsync_RuntimeGraph()
        {
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netcoreapp1.0", builder =>
                {
                    builder.WithProperty("RuntimeIdentifier", "win10-x64");
                })
                .Build();

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.Projects.Single();
            Assert.Equal("win10-x64", actualProjectSpec.RuntimeGraph.Runtimes.Single().Key);
        }

        [Fact]
        public async Task NominateProjectAsync_BasicPackageDownload()
        {
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netcoreapp3.0", builder =>
                {
                    builder
                    .WithItem(ProjectItems.PackageReference, "NuGet.Protocol", [new("version", "5.1.0")])
                    .WithItem(ProjectItems.PackageDownload, "NetCoreTargetingPack", [new("version", "[1.0.0]")])
                    .WithItem(ProjectItems.PackageDownload, "NetCoreRuntimePack", [new("version", "[1.0.0]")])
                    .WithItem(ProjectItems.PackageDownload, "NetCoreApphostPack", [new("version", "[1.0.0]")]);
                })
                .Build();

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.Projects.Single();
            Assert.Equal("1.0.0", actualProjectSpec.Version.ToString());

            var actualMetadata = actualProjectSpec.RestoreMetadata;
            Assert.NotNull(actualMetadata);
            Assert.Equal(ProjectStyle.PackageReference, actualMetadata.ProjectStyle);

            Assert.Single(actualProjectSpec.TargetFrameworks);
            var actualTfi = actualProjectSpec.TargetFrameworks.Single();

            var expectedFramework = NuGetFramework.Parse("netcoreapp3.0");
            Assert.Equal(expectedFramework, actualTfi.FrameworkName);

            AssertPackages(actualTfi,
                "NuGet.Protocol:5.1.0");
            AssertDownloadPackages(actualTfi,
                "NetCoreTargetingPack:[1.0.0]",
                "NetCoreRuntimePack:[1.0.0]",
                "NetCoreApphostPack:[1.0.0]"
                );
        }

        [Theory]
        [InlineData("[1.0.0];[2.0.0]")]
        [InlineData(";[1.0.0];;[2.0.0];")]
        public async Task NominateProjectAsync_PackageDownload_AllowsVersionList(string versionString)
        {
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netcoreapp3.0", builder =>
                {
                    builder
                    .WithItem(ProjectItems.PackageReference, "NuGet.Protocol", [new("version", "5.1.0")])
                    .WithItem(ProjectItems.PackageDownload, "NetCoreTargetingPack", [new("version", versionString)]);
                })
                .Build();

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.Projects.Single();
            Assert.Equal("1.0.0", actualProjectSpec.Version.ToString());

            var actualMetadata = actualProjectSpec.RestoreMetadata;
            Assert.NotNull(actualMetadata);
            Assert.Equal(ProjectStyle.PackageReference, actualMetadata.ProjectStyle);

            Assert.Single(actualProjectSpec.TargetFrameworks);
            var actualTfi = actualProjectSpec.TargetFrameworks.Single();

            var expectedFramework = NuGetFramework.Parse("netcoreapp3.0");
            Assert.Equal(expectedFramework, actualTfi.FrameworkName);

            AssertPackages(actualTfi,
                "NuGet.Protocol:5.1.0");
            AssertDownloadPackages(actualTfi,
                "NetCoreTargetingPack:[1.0.0]",
                "NetCoreTargetingPack:[2.0.0]");
        }

        [Fact]
        public async Task NominateProjectAsync_PackageDownload_InexactVersionsNotAllowed()
        {
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netcoreapp3.0", builder =>
                {
                    builder
                    .WithItem(ProjectItems.PackageReference, "NuGet.Protocol", [new("version", "5.1.0")])
                    .WithItem(ProjectItems.PackageDownload, "NetCoreTargetingPack", [new("version", "[1.0.0, )")]);
                })
                .Build();
            var projectFullPath = projectRestoreInfo.TargetFrameworks[0].Properties["MSBuildProjectFullPath"];
            var cache = Mock.Of<IProjectSystemCache>();
            var restoreWorker = Mock.Of<ISolutionRestoreWorker>();
            var vsSolution2 = Mock.Of<IVsSolution2>();
            var asyncLazySolution2 = new Microsoft.VisualStudio.Threading.AsyncLazy<IVsSolution2>(() => Task.FromResult(vsSolution2));
            var telemetryProvider = Mock.Of<INuGetTelemetryProvider>();

            var service = new VsSolutionRestoreService(
                cache, restoreWorker, NullLogger.Instance, asyncLazySolution2, telemetryProvider);

            var result = await ((IVsSolutionRestoreService5)service).NominateProjectAsync(projectFullPath, projectRestoreInfo, CancellationToken.None);

            Assert.False(result, "Project restore nomination must fail.");
        }

        [Fact]
        public async Task NominateProjectAsync_BasicFrameworkReferences()
        {
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netcoreapp3.0", builder =>
                {
                    builder
                    .WithItem(ProjectItems.PackageReference, "NuGet.Protocol", [new("version", "5.1.0")])
                    .WithItem(ProjectItems.FrameworkReference, "Microsoft.WindowsDesktop.App|WPF", [new("privateAssets", "none")])
                    .WithItem(ProjectItems.FrameworkReference, "Microsoft.WindowsDesktop.App|WinForms", [new("privateAssets", "all")]);
                })
                .Build();

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.Projects.Single();
            Assert.Equal("1.0.0", actualProjectSpec.Version.ToString());

            var actualMetadata = actualProjectSpec.RestoreMetadata;
            Assert.NotNull(actualMetadata);
            Assert.Equal(ProjectStyle.PackageReference, actualMetadata.ProjectStyle);

            Assert.Single(actualProjectSpec.TargetFrameworks);
            var actualTfi = actualProjectSpec.TargetFrameworks.Single();

            var expectedFramework = NuGetFramework.Parse("netcoreapp3.0");
            Assert.Equal(expectedFramework, actualTfi.FrameworkName);

            AssertFrameworkReferences(actualTfi,
                "Microsoft.WindowsDesktop.App|WinForms:all",
                "Microsoft.WindowsDesktop.App|WPF:none");
        }

        [Fact]
        public async Task NominateProjectAsync_FrameworkReferencesAreCaseInsensitive()
        {
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netcoreapp3.0", builder =>
                {
                    builder
                    .WithItem(ProjectItems.PackageReference, "NuGet.Protocol", [new("version", "5.1.0")])
                    .WithItem(ProjectItems.FrameworkReference, "Microsoft.WindowsDesktop.App|WinForms", [new("privateAssets", "none")])
                    .WithItem(ProjectItems.FrameworkReference, "Microsoft.WindowsDesktop.App|WINFORMS", [new("privateAssets", "none")]);
                })
                .Build();

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.Projects.Single();
            Assert.Equal("1.0.0", actualProjectSpec.Version.ToString());

            var actualMetadata = actualProjectSpec.RestoreMetadata;
            Assert.NotNull(actualMetadata);
            Assert.Equal(ProjectStyle.PackageReference, actualMetadata.ProjectStyle);

            Assert.Single(actualProjectSpec.TargetFrameworks);
            var actualTfi = actualProjectSpec.TargetFrameworks.Single();

            var expectedFramework = NuGetFramework.Parse("netcoreapp3.0");
            Assert.Equal(expectedFramework, actualTfi.FrameworkName);

            AssertFrameworkReferences(actualTfi,
                "Microsoft.WindowsDesktop.App|WinForms:none");
        }

        [Fact]
        public async Task NominateProjectAsync_FrameworkReferencesMultiTargeting()
        {
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netcoreapp3.0", builder =>
                {
                    builder
                    .WithItem(ProjectItems.FrameworkReference, "Microsoft.WindowsDesktop.App|WPF", [new("privateAssets", "none")])
                    .WithItem(ProjectItems.FrameworkReference, "Microsoft.WindowsDesktop.App|WinForms", [new("privateAssets", "none")]);
                })
                .WithTargetFrameworkInfo("netcoreapp3.1", builder =>
                {
                    builder
                    .WithItem(ProjectItems.FrameworkReference, "Microsoft.ASPNetCore.App", [new("privateAssets", "none")]);
                })
                .Build();

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.Projects.Single();
            Assert.Equal("1.0.0", actualProjectSpec.Version.ToString());

            var actualMetadata = actualProjectSpec.RestoreMetadata;
            Assert.NotNull(actualMetadata);
            Assert.Equal(ProjectStyle.PackageReference, actualMetadata.ProjectStyle);

            Assert.Equal(2, actualProjectSpec.TargetFrameworks.Count);

            var tfi = actualProjectSpec.TargetFrameworks.First();

            var expectedFramework = NuGetFramework.Parse("netcoreapp3.0");
            Assert.Equal(expectedFramework, tfi.FrameworkName);

            AssertFrameworkReferences(tfi,
                "Microsoft.WindowsDesktop.App|WinForms:none",
                "Microsoft.WindowsDesktop.App|WPF:none");

            tfi = actualProjectSpec.TargetFrameworks.Last();
            expectedFramework = NuGetFramework.Parse("netcoreapp3.1");
            Assert.Equal(expectedFramework, tfi.FrameworkName);

            AssertFrameworkReferences(tfi,
                "Microsoft.ASPNetCore.App:none");
        }

        [Fact]
        public async Task NominateProjectAsync_WarningProperties()
        {
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netcoreapp1.0", builder =>
                {
                    builder
                    .WithProperty("TreatWarningsAsErrors", "true")
                    .WithProperty("WarningsAsErrors", "NU1603;NU1604")
                    .WithProperty("WarningsNotAsErrors", "NU1801;NU1802")
                    .WithItem(ProjectItems.PackageReference, "NuGet.Protocol", [new("NoWarn", "NU1605")]);
                })
                .Build();

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.Projects.Single();
            Assert.True(actualProjectSpec.RestoreMetadata.ProjectWideWarningProperties.AllWarningsAsErrors);
            Assert.True(actualProjectSpec.TargetFrameworks.First().Dependencies.First().NoWarn.First().Equals(NuGetLogCode.NU1605));
            Assert.Null(actualProjectSpec.TargetFrameworks.First().RuntimeIdentifierGraphPath);
            Assert.True(actualProjectSpec.RestoreMetadata.ProjectWideWarningProperties.WarningsAsErrors.Count.Equals(2));
            Assert.True(actualProjectSpec.RestoreMetadata.ProjectWideWarningProperties.WarningsAsErrors.Contains(NuGetLogCode.NU1603));
            Assert.True(actualProjectSpec.RestoreMetadata.ProjectWideWarningProperties.WarningsAsErrors.Contains(NuGetLogCode.NU1604));
            Assert.True(actualProjectSpec.RestoreMetadata.ProjectWideWarningProperties.WarningsNotAsErrors.Count.Equals(2));
            Assert.True(actualProjectSpec.RestoreMetadata.ProjectWideWarningProperties.WarningsNotAsErrors.Contains(NuGetLogCode.NU1801));
            Assert.True(actualProjectSpec.RestoreMetadata.ProjectWideWarningProperties.WarningsNotAsErrors.Contains(NuGetLogCode.NU1802));
        }

        [Fact]
        public async Task NominateProjectAsync_RuntimeIdentifierGraphPath()
        {
            var runtimeGraphPath = @"C:\Program Files\dotnet\sdk\3.0.100\runtime.json";

            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netcoreapp1.0", builder =>
                {
                    builder.WithProperty(ProjectBuildProperties.RuntimeIdentifierGraphPath, runtimeGraphPath);
                })
                .Build();

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.Projects.Single();
            Assert.Equal(runtimeGraphPath, actualProjectSpec.TargetFrameworks.First().RuntimeIdentifierGraphPath);
        }

        [Fact]
        public async Task NominateProjectAsync_ThrowsNullReferenceException()
        {
            var cache = Mock.Of<IProjectSystemCache>();

            Mock.Get(cache)
                .Setup(x => x.AddProjectRestoreInfo(
                    It.IsAny<ProjectNames>(),
                    It.IsAny<DependencyGraphSpec>(),
                    It.IsAny<IReadOnlyList<IAssetsLogMessage>>()))
                .Returns(true);

            var completedRestoreTask = Task.FromResult(true);

            var restoreWorker = Mock.Of<ISolutionRestoreWorker>();
            var vsSolution2 = Mock.Of<IVsSolution2>();
            var asyncLazySolution2 = new Microsoft.VisualStudio.Threading.AsyncLazy<IVsSolution2>(() => Task.FromResult(vsSolution2));
            var telemetryProvider = Mock.Of<INuGetTelemetryProvider>();

            var service = new VsSolutionRestoreService(
                cache, restoreWorker, NullLogger.Instance, asyncLazySolution2, telemetryProvider);

            // Act
            _ = await Assert.ThrowsAsync<ArgumentNullException>(async () => await ((IVsSolutionRestoreService5)service).NominateProjectAsync(@"F:\project\project.csproj", null!, CancellationToken.None));
        }

        [Fact]
        public async Task NominateProjectAsync_InvalidTargetFrameworkMoniker_Succeeds()
        {
            // Arrange
            var restoreWorker = CreateDefaultISolutionRestoreWorkerMock();

            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netcoreapp1.0", builder =>
                {
                    builder
                    .WithProperty("TargetFrameworkMoniker", "_,Version=2.0");
                })
                .Build();

            // Act
            var additionalMessages = await CaptureAdditionalMessagesAsync(projectRestoreInfo, restoreWorker: restoreWorker);

            // Assert
            var additionalMessage = Assert.Single(additionalMessages);
            Assert.Equal(NuGetLogCode.NU1105, additionalMessage.Code);
            restoreWorker.Verify(rw => rw.ScheduleRestoreAsync(It.IsAny<SolutionRestoreRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task NominateProjectAsync_InvalidDependencyVersion_Succeeds()
        {
            // Arrange
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netstandard2.0", builder =>
                {
                    builder
                    .WithItem(ProjectItems.PackageReference, "packageId", [new("Version", "foo")]);
                })
                .Build();

            var restoreWorker = CreateDefaultISolutionRestoreWorkerMock();

            // Act
            var additionalMessages = await CaptureAdditionalMessagesAsync(projectRestoreInfo, restoreWorker);

            // Assert
            var additionalMessage = Assert.Single(additionalMessages);
            Assert.Equal(NuGetLogCode.NU1105, additionalMessage.Code);
            restoreWorker.Verify(rw => rw.ScheduleRestoreAsync(It.IsAny<SolutionRestoreRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task NominateProjectAsync_CancelledToken_ThrowsOperationCanceledException()
        {
            // Arrange
            var restoreWorker = new Mock<ISolutionRestoreWorker>();
            restoreWorker.Setup(x => x.ScheduleRestoreAsync(
                    It.IsAny<SolutionRestoreRequest>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromException<bool>(new OperationCanceledException()));

            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netstandard2.0")
                .Build();

            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await NominateProjectAsync(projectRestoreInfo, CancellationToken.None, restoreWorker: restoreWorker));
        }

        [Fact]
        public void ToPackageSpec_CentralVersions_AreAddedToThePackageSpecIfCPVMIsEnabled()
        {
            // Arrange
            ProjectNames projectName = new ProjectNames(@"f:\project\project.csproj", "project", "project.csproj", "projectC", Guid.NewGuid().ToString());

            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netstandard2.0", builder =>
                {
                    builder
                    .WithProperty("ManagePackageVersionsCentrally", "true")
                    .WithItem(ProjectItems.PackageReference, "foo", [])
                    .WithItem(ProjectItems.PackageVersion, "foo", [new("Version", "2.0.0")])
                    // the second PackageVersion with the same version name will be ignored
                    .WithItem(ProjectItems.PackageVersion, "foo", [new("Version", "3.0.0")]);
                })
                .Build();

            // Act
            var result = VsSolutionRestoreService.ToPackageSpec(projectName, projectRestoreInfo);

            // Assert
            var tfm = result.TargetFrameworks.First();

            var packageVersion = Assert.Single(tfm.CentralPackageVersions);
            Assert.Equal("foo", packageVersion.Key);
            Assert.Equal("[2.0.0, )", packageVersion.Value.VersionRange.ToNormalizedString());
            var packageReference = Assert.Single(tfm.Dependencies);
            Assert.Equal("[2.0.0, )", packageReference.LibraryRange.VersionRange?.ToNormalizedString());
            Assert.True(result.RestoreMetadata.CentralPackageVersionsEnabled);
        }

        [Fact]
        public void ToPackageSpec_CentralVersions_CPVMIsEnabled_NoPackageVersions()
        {
            // Arrange
            ProjectNames projectName = new ProjectNames(@"f:\project\project.csproj", "project", "project.csproj", "projectC", Guid.NewGuid().ToString());

            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netstandard2.0", builder =>
                {
                    builder
                    .WithProperty("ManagePackageVersionsCentrally", "true")
                    .WithItem(ProjectItems.PackageReference, "foo", []);
                })
                .Build();

            // Act
            var result = VsSolutionRestoreService.ToPackageSpec(projectName, projectRestoreInfo);

            // Assert
            var tfm = result.TargetFrameworks.First();

            Assert.Equal(0, tfm.CentralPackageVersions.Count);
            Assert.True(result.RestoreMetadata.CentralPackageVersionsEnabled);
        }

        /// <summary>
        /// The default for DisableCentralPackageVersions should be disabled.
        /// </summary>
        [Theory]
        [InlineData("1.0.0", "false")]
        [InlineData(null, null)]
        [InlineData("1.0.0", "")]
        public void ToPackageSpec_CentralVersions_AreNotAddedToThePackageSpecIfCPVMIsNotEnabled(string packRefVersion, string managePackageVersionsCentrally)
        {
            // Arrange
            ProjectNames projectName = new ProjectNames(@"f:\project\project.csproj", "project", "project.csproj", "projectC", Guid.NewGuid().ToString());

            KeyValuePair<string, string>[] packageReferenceProperties = packRefVersion == null ?
                [] :
                new KeyValuePair<string, string>[] { new("Version", packRefVersion) };

            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netstandard2.0", builder =>
                {
                    builder
                    .WithItem(ProjectItems.PackageReference, "foo", packageReferenceProperties)
                    .WithItem(ProjectItems.PackageVersion, "foo", [new("Version", "2.0.0")]);
                    if (managePackageVersionsCentrally is not null)
                    {
                        builder.WithProperty("ManagePackageVersionsCentrally", managePackageVersionsCentrally);
                    }
                })
                .Build();

            // Act
            var result = VsSolutionRestoreService.ToPackageSpec(projectName, projectRestoreInfo);

            // Assert
            var tfm = result.TargetFrameworks.First();
            var expectedPackageReferenceVersion = packRefVersion == null ? "(, )" : "[1.0.0, )";
            Assert.Equal(0, tfm.CentralPackageVersions.Count);
            Assert.Equal(1, tfm.Dependencies.Length);
            Assert.Equal(expectedPackageReferenceVersion, tfm.Dependencies.First().LibraryRange.VersionRange?.ToNormalizedString());
            Assert.False(result.RestoreMetadata.CentralPackageVersionsEnabled);
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData("", true)]
        [InlineData("                     ", true)]
        [InlineData("true", true)]
        [InlineData("invalid", true)]
        [InlineData("false", false)]
        [InlineData("           false    ", false)]
        public void ToPackageSpec_CentralVersionOverride_CanBeDisabled(string isCentralPackageVersionOverrideEnabled, bool expected)
        {
            // Arrange
            ProjectNames projectName = new ProjectNames(@"f:\project\project.csproj", "project", "project.csproj", "projectC", Guid.NewGuid().ToString());

            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netstandard2.0", builder =>
                {
                    builder
                    .WithProperty(ProjectBuildProperties.ManagePackageVersionsCentrally, "true")
                    .WithProperty(ProjectBuildProperties.CentralPackageVersionOverrideEnabled, isCentralPackageVersionOverrideEnabled)
                    .WithItem(ProjectItems.PackageReference, "foo", [])
                    .WithItem(ProjectItems.PackageVersion, "foo", [new("Version", "2.0.0")])
                    // the second PackageVersion with the same version name will be ignored
                    .WithItem(ProjectItems.PackageVersion, "foo", [new("Version", "3.0.0")]);
                })
                .Build();

            // Act
            PackageSpec result = VsSolutionRestoreService.ToPackageSpec(projectName, projectRestoreInfo);

            // Assert
            TargetFrameworkInformation tfm = result.TargetFrameworks.First();

            KeyValuePair<string, CentralPackageVersion> packageVersion = Assert.Single(tfm.CentralPackageVersions);
            Assert.Equal("foo", packageVersion.Key);
            Assert.Equal("[2.0.0, )", packageVersion.Value.VersionRange.ToNormalizedString());
            LibraryDependency packageReference = Assert.Single(tfm.Dependencies);
            Assert.Equal("[2.0.0, )", packageReference.LibraryRange.VersionRange?.ToNormalizedString());

            if (expected)
            {
                Assert.False(result.RestoreMetadata.CentralPackageVersionOverrideDisabled);
            }
            else
            {
                Assert.True(result.RestoreMetadata.CentralPackageVersionOverrideDisabled);
            }
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData(" ", false)]
        [InlineData("invalid", false)]
        [InlineData("false", false)]
        [InlineData("true", true)]
        [InlineData("           true    ", true)]
        public void ToPackageSpec_TransitiveDependencyPinning_CanBeEnabled(string CentralPackageTransitivePinningEnabled, bool expected)
        {
            // Arrange
            ProjectNames projectName = new ProjectNames(@"f:\project\project.csproj", "project", "project.csproj", "projectC", Guid.NewGuid().ToString());

            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netstandard2.0", builder =>
                {
                    builder
                    .WithProperty(ProjectBuildProperties.ManagePackageVersionsCentrally, "true")
                    .WithProperty(ProjectBuildProperties.CentralPackageTransitivePinningEnabled, CentralPackageTransitivePinningEnabled)
                    .WithItem(ProjectItems.PackageReference, "foo", [new("Version", "1.0.0")]);
                })
                .Build();

            // Act
            PackageSpec result = VsSolutionRestoreService.ToPackageSpec(projectName, projectRestoreInfo);

            // Assert

            if (expected)
            {
                Assert.True(result.RestoreMetadata.CentralPackageTransitivePinningEnabled);
            }
            else
            {
                Assert.False(result.RestoreMetadata.CentralPackageTransitivePinningEnabled);
            }
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData(" ", false)]
        [InlineData("invalid", false)]
        [InlineData("false", false)]
        [InlineData("true", true)]
        [InlineData("           true    ", true)]
        public void ToPackageSpec_CentralPackageFloatingVersions_CanBeEnabled(string centralPackageFloatingVersionsEnabled, bool expected)
        {
            // Arrange
            ProjectNames projectName = new ProjectNames(@"f:\project\project.csproj", "project", "project.csproj", "projectC", Guid.NewGuid().ToString());

            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netstandard2.0", builder =>
                {
                    builder
                    .WithProperty(ProjectBuildProperties.ManagePackageVersionsCentrally, "true")
                    .WithProperty(ProjectBuildProperties.CentralPackageFloatingVersionsEnabled, centralPackageFloatingVersionsEnabled)
                    .WithItem(ProjectItems.PackageReference, "foo", [new("Version", "1.0.0")]);
                })
                .Build();

            // Act
            PackageSpec result = VsSolutionRestoreService.ToPackageSpec(projectName, projectRestoreInfo);

            // Assert

            if (expected)
            {
                Assert.True(result.RestoreMetadata.CentralPackageFloatingVersionsEnabled);
            }
            else
            {
                Assert.False(result.RestoreMetadata.CentralPackageFloatingVersionsEnabled);
            }
        }

        [Fact]
        public async Task NominateProjectAsync_PackageReferenceWithAliases()
        {
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netcoreapp1.0", builder =>
                {
                    builder
                    .WithItem(ProjectItems.PackageReference, "NuGet.Protocol", [new("Aliases", "Core")]);
                })
                .Build();


            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.Projects.Single();
            Assert.Equal("Core", actualProjectSpec.TargetFrameworks.First().Dependencies.First().Aliases);
        }

        /// <summary>
        /// The default for DisableCentralPackageVersions should be disabled.
        /// </summary>
        [Fact]
        public void ToPackageSpec_TargetFrameworkWithAlias_DefinesAliasCorrectly()
        {
            // Arrange
            ProjectNames projectName = new ProjectNames(@"f:\project\project.csproj", "project", "project.csproj", "projectC", Guid.NewGuid().ToString());

            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netstandard2.0", builder =>
                {
                    builder
                    .WithProperty(ProjectBuildProperties.TargetFramework, "tfm1");
                })
                .WithTargetFrameworkInfo("netstandard2.1", builder =>
                {
                    builder
                    .WithProperty(ProjectBuildProperties.TargetFramework, "tfm2");
                })
                .Build();

            // Act
            var result = VsSolutionRestoreService.ToPackageSpec(projectName, projectRestoreInfo);

            // Assert
            Assert.Equal(2, result.TargetFrameworks.Count);
            TargetFrameworkInformation targetFrameworkInfo = Assert.Single(result.TargetFrameworks, tf => tf.FrameworkName == CommonFrameworks.NetStandard20);
            Assert.Equal("tfm1", targetFrameworkInfo.TargetAlias);
            targetFrameworkInfo = Assert.Single(result.TargetFrameworks, tf => tf.FrameworkName == CommonFrameworks.NetStandard21);
            Assert.Equal("tfm2", targetFrameworkInfo.TargetAlias);
        }

        // In VS, when a PackageReference CPS (SDK style) project gets loaded, the project system needs to send us a nomination.
        // We then parse/transform that nomination input into a DependencyGraphSpec. If there's any problem, we need to display a message.
        // However, the "contract" is that NuGet only ever writes errors for CPS projects to the assets file, and CPS will read the assets
        // file and replay the errors in Visual Studio's Error List. Therefore, in VS, we must generate an assets file, even when there's
        // a nomination error. We do this by creating a minimal DependencyGraphSpec, and do a normal restore with it. This is the only
        // production code that needs to create the minimal DGSpec, hence the method to create the minimal DGSpec is in our VS assembly.
        //
        // For testing, we want to be able to test the minimal DGSpec in NuGet.Commands.Test and NuGet.PackageManagement.Test. There are a
        // few problems here. First, NuGet.SolutionRestoreManager.dll is only net472, but NuGet.Commands.Test and NuGet.PackageManagement.Test
        // also target netstandard and net5. Additionally, NuGet.Commands is part of the NuGet SDK, meaning it only uses other NuGet SDK projects
        // and I don't want to add CreateMinimalDependencyGraphSpec to the public API.
        //
        // Therefore, what I've done is duplicated the method, once in NuGet.SolutionRestoreManagement, once in Test.Utilities. This test
        // makes sure the implementations are the same, so if someone modifies some restore code and the minimum DGSpec grows, they will
        // naturally change the Test.Utilities copy of the method as tests that use that will fail. This test will make sure they update the
        // NuGet.SolutionRestoreManager copy as well.
        [Fact]
        public void CreateMinimalDependencyGraphSpec_ComparedToTestUtility_AreEqual()
        {
            // Arrange
            const string projectPath = @"c:\src\project\project.csproj";
            const string outputPath = @"c:\src\project\obj";

            DependencyGraphSpec productionMinimalDGSpec = VsSolutionRestoreService.CreateMinimalDependencyGraphSpec(projectPath, outputPath);
            DependencyGraphSpec testMinimalDGSpec = DependencyGraphSpecTestUtilities.CreateMinimalDependencyGraphSpec(projectPath, outputPath);

            string prodJson = Newtonsoft.Json.JsonConvert.SerializeObject(productionMinimalDGSpec);
            string testJson = Newtonsoft.Json.JsonConvert.SerializeObject(testMinimalDGSpec);

            // Act/Assert
            Assert.Equal(prodJson, testJson);
        }

        [Theory]
        [InlineData("NetCore", true)]
        [InlineData("NetFx", false)]
        [InlineData("false", false)]
        public void ToPackageSpec_TargetFrameworkWithCLRSupport_InterpretsFrameworkCorrect(string clrSupport, bool isDualCompatibilityFramework)
        {
            // Arrange
            ProjectNames projectName = new ProjectNames(@"f:\project\project.vcxproj", "project", "project.csproj", "project", Guid.NewGuid().ToString());

            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("net5.0-windows10.0", builder =>
                {
                    builder
                    .WithProperty(ProjectBuildProperties.TargetFramework, "tfm1")
                    .WithProperty(ProjectBuildProperties.CLRSupport, clrSupport)
                    .WithProperty(ProjectBuildProperties.WindowsTargetPlatformMinVersion, "10.0");
                })
                .Build();

            // Act
            var result = VsSolutionRestoreService.ToPackageSpec(projectName, projectRestoreInfo);

            // Assert
            Assert.Equal(1, result.TargetFrameworks.Count);
            TargetFrameworkInformation targetFrameworkInfo = Assert.Single(result.TargetFrameworks, tf => tf.TargetAlias == "tfm1");

            if (isDualCompatibilityFramework)
            {
                var managedFramework = NuGetFramework.Parse("net5.0-windows10.0");
                var comparer = NuGetFrameworkFullComparer.Instance;
                comparer.Equals(targetFrameworkInfo.FrameworkName, managedFramework).Should().BeTrue();
                targetFrameworkInfo.FrameworkName.Should().BeOfType<DualCompatibilityFramework>();
                var dualCompatibilityFramework = (DualCompatibilityFramework)targetFrameworkInfo.FrameworkName;
                dualCompatibilityFramework.RootFramework.Should().Be(managedFramework);
                dualCompatibilityFramework.SecondaryFramework.Should().Be(CommonFrameworks.Native);
            }
            else
            {
                targetFrameworkInfo.FrameworkName.Should().NotBeOfType<DualCompatibilityFramework>();
                targetFrameworkInfo.FrameworkName.Should().Be(CommonFrameworks.Native);
            }
        }

        [Fact]
        public void ToPackageSpec_TwoProjectReferencesToSameProject_DeduplicatesProjectReference()
        {
            // Arrange
            string currentProjectPath = @"n:\path\to\current\project.csproj";
            string referencedProjectPath = @"n:\path\to\some\reference.csproj";
            string relativePath = new Uri(currentProjectPath).MakeRelativeUri(new Uri(referencedProjectPath)).OriginalString;

            ProjectNames projectName = new(currentProjectPath, "project", "project.csproj", "project", Guid.NewGuid().ToString());

            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netstandard2.0", builder =>
                {
                    builder.WithItem(ProjectItems.ProjectReference, relativePath, []);
                })
                .Build();

            // Act
            var actual = VsSolutionRestoreService.ToPackageSpec(projectName, projectRestoreInfo);

            // Assert
            ProjectRestoreMetadataFrameworkInfo targetFramework = Assert.Single(actual.RestoreMetadata.TargetFrameworks);
            var projectReference = Assert.Single(targetFramework.ProjectReferences);
            Assert.Equal(referencedProjectPath, projectReference.ProjectUniqueName);
        }

        private delegate bool TryGetProjectNamesReturns(string projectPath, out ProjectNames projectNames);

        [Fact]
        public void ToPackageSpec_PackageDownloadWithNoVersion_ThrowsException()
        {
            // Arrange
            string packageName = "package";
            ProjectNames projectName = new ProjectNames(@"n:\path\to\current\project.csproj", "project", "project.csproj", "project", Guid.NewGuid().ToString());

            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netstandard2.0", builder =>
                {
                    builder.WithItem(ProjectItems.PackageDownload, packageName, []);
                })
                .Build();

            string expected = string.Format(CultureInfo.CurrentCulture, Resources.Error_PackageDownload_NoVersion, packageName);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(() => VsSolutionRestoreService.ToPackageSpec(projectName, projectRestoreInfo));
            Assert.Equal(expected, exception.Message);
        }

        [Fact]
        public void ToPackageSpec_WithNuGetAuditSupressItem_PackageSpecHasSupressUrl()
        {
            // Arrange
            ProjectNames projectName = new(@"n:\path\to\current\project.csproj", "project", "project.csproj", "project", Guid.NewGuid().ToString());
            string cveUrl = "https://cve.test/1";

            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netstandard2.0", builder =>
                {
                    builder.WithItem(ProjectItems.NuGetAuditSuppress, cveUrl, []);
                })
                .Build();

            // Act
            var actual = VsSolutionRestoreService.ToPackageSpec(projectName, projectRestoreInfo);

            // Assert
            RestoreAuditProperties auditProperties = actual.RestoreMetadata.RestoreAuditProperties;
            string actualUrl = Assert.Single(auditProperties.SuppressedAdvisories ?? []);
            actualUrl.Should().Be(cveUrl);
        }

        [Fact]
        public void ToPackageSpec_TwoTFMsWithDifferentNuGetAuditSupressItems_Throws()
        {
            // Arrange
            ProjectNames projectName = new(@"n:\path\to\current\project.csproj", "project", "project.csproj", "project", Guid.NewGuid().ToString());
            string cve1Url = "https://cve.test/1";
            string cve2Url = "https://cve.test/2";

            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("net8.0", builder =>
                {
                    builder.WithItem(ProjectItems.NuGetAuditSuppress, cve1Url, []);
                })
                .WithTargetFrameworkInfo("net48", builder =>
                {
                    builder.WithItem(ProjectItems.NuGetAuditSuppress, cve2Url, []);
                })
                .Build();

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => VsSolutionRestoreService.ToPackageSpec(projectName, projectRestoreInfo));
            exception.Message.Should().Contain(ProjectItems.NuGetAuditSuppress);
        }

        [Fact]
        public async Task NominateProjectAsync_CanSetUseLegacyDependencyResolver()
        {
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netstandard2.0", builder =>
                {
                    builder
                    .WithProperty(ProjectBuildProperties.RestoreUseLegacyDependencyResolver, "true");
                })
                .Build();

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.Projects.Single();
            actualProjectSpec.RestoreMetadata.UseLegacyDependencyResolver.Should().BeTrue();
        }

        [Fact]
        public void ToPackageSpec_WithCPMAndAutoReferencedPackages_SkipsAddingVersion()
        {
            // Arrange
            ProjectNames projectName = new ProjectNames(@"f:\project\project.csproj", "project", "project.csproj", "projectC", Guid.NewGuid().ToString());

            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netstandard2.0", builder =>
                {
                    builder
                    .WithProperty(ProjectBuildProperties.ManagePackageVersionsCentrally, "true")
                    .WithItem(ProjectItems.PackageReference, "foo", [])
                    .WithItem(ProjectItems.PackageReference, "bar", [new("IsImplicitlyDefined", "true"), new("Version", "10.0.1")])
                    .WithItem(ProjectItems.PackageVersion, "foo", [new("Version", "2.0.0")]);
                })
                .Build();

            // Act
            var result = VsSolutionRestoreService.ToPackageSpec(projectName, projectRestoreInfo);

            // Assert
            var tfm = result.TargetFrameworks.First();

            tfm.CentralPackageVersions.Should().HaveCount(1);
            tfm.CentralPackageVersions.Single().Value.Name.Should().Be("foo");
            result.RestoreMetadata.CentralPackageVersionsEnabled.Should().BeTrue();
            tfm.Dependencies.Should().HaveCount(2);
            tfm.Dependencies[0].Name.Should().Be("foo");
            tfm.Dependencies[0].VersionCentrallyManaged.Should().BeTrue();
            tfm.Dependencies[0].LibraryRange.VersionRange.Should().Be(VersionRange.Parse("2.0.0"));
            tfm.Dependencies[0].AutoReferenced.Should().BeFalse();

            tfm.Dependencies[1].Name.Should().Be("bar");
            tfm.Dependencies[1].VersionCentrallyManaged.Should().BeFalse();
            tfm.Dependencies[1].LibraryRange.VersionRange.Should().Be(VersionRange.Parse("10.0.1"));
            tfm.Dependencies[1].AutoReferenced.Should().BeTrue();
        }

        [Fact]
        public void ToPackageSpec_WithCPMAndVersionOverride_SetsVersionToVersionOverride()
        {
            // Arrange
            ProjectNames projectName = new ProjectNames(@"f:\project\project.csproj", "project", "project.csproj", "projectC", Guid.NewGuid().ToString());

            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netstandard2.0", builder =>
                {
                    builder
                    .WithProperty(ProjectBuildProperties.ManagePackageVersionsCentrally, "true")
                    .WithProperty(ProjectBuildProperties.CentralPackageVersionOverrideEnabled, "true")
                    .WithItem(ProjectItems.PackageReference, "foo", [])
                    .WithItem(ProjectItems.PackageReference, "bar", [new("VersionOverride", "10.0.1")])
                    .WithItem(ProjectItems.PackageVersion, "foo", [new("Version", "2.0.0")])
                    .WithItem(ProjectItems.PackageVersion, "bar", [new("Version", "3.0.0")]);
                })
                .Build();

            // Act
            var result = VsSolutionRestoreService.ToPackageSpec(projectName, projectRestoreInfo);

            // Assert
            var tfm = result.TargetFrameworks.First();

            tfm.CentralPackageVersions.Should().HaveCount(2);
            result.RestoreMetadata.CentralPackageVersionsEnabled.Should().BeTrue();
            tfm.Dependencies.Should().HaveCount(2);
            tfm.Dependencies[0].Name.Should().Be("foo");
            tfm.Dependencies[0].VersionCentrallyManaged.Should().BeTrue();
            tfm.Dependencies[0].LibraryRange.VersionRange.Should().Be(VersionRange.Parse("2.0.0"));
            tfm.Dependencies[0].AutoReferenced.Should().BeFalse();

            tfm.Dependencies[1].Name.Should().Be("bar");
            tfm.Dependencies[1].VersionCentrallyManaged.Should().BeFalse();
            tfm.Dependencies[1].LibraryRange.VersionRange.Should().Be(VersionRange.Parse("10.0.1"));
            tfm.Dependencies[1].VersionOverride.Should().Be(VersionRange.Parse("10.0.1"));
            tfm.Dependencies[1].AutoReferenced.Should().BeFalse();
        }

        [Fact]
        public void ToPackageSpec_WithCPMAndPackageReferenceWithDifferentCasing_CombinesCorrectly()
        {
            // Arrange
            ProjectNames projectName = new ProjectNames(@"f:\project\project.csproj", "project", "project.csproj", "projectC", Guid.NewGuid().ToString());
            string packageId = "foo";

            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netstandard2.0", builder =>
                {
                    builder
                    .WithProperty(ProjectBuildProperties.ManagePackageVersionsCentrally, "true")
                    .WithProperty(ProjectBuildProperties.CentralPackageVersionOverrideEnabled, "true")
                    .WithItem(ProjectItems.PackageReference, packageId, [])
                    .WithItem(ProjectItems.PackageVersion, packageId.ToUpperInvariant(), [new("Version", "2.0.0")]);
                })
                .Build();

            // Act
            var result = VsSolutionRestoreService.ToPackageSpec(projectName, projectRestoreInfo);

            // Assert
            var tfm = result.TargetFrameworks.First();

            tfm.CentralPackageVersions.Should().HaveCount(1);
            result.RestoreMetadata.CentralPackageVersionsEnabled.Should().BeTrue();
            tfm.Dependencies.Should().HaveCount(1);
            tfm.Dependencies[0].Name.Should().Be(packageId);
            tfm.Dependencies[0].VersionCentrallyManaged.Should().BeTrue();
            tfm.Dependencies[0].LibraryRange.VersionRange.Should().Be(VersionRange.Parse("2.0.0"));
            tfm.Dependencies[0].AutoReferenced.Should().BeFalse();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ToPackageSpec_WithPrunePackageReference_CapturesAsMaxVersions(bool restoreEnablePackagePruning)
        {
            // Arrange
            ProjectNames projectName = new(@"n:\path\to\current\project.csproj", "project", "project.csproj", "project", Guid.NewGuid().ToString());

            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netstandard2.0", builder =>
                {
                    builder
                    .WithProperty(ProjectBuildProperties.RestoreEnablePackagePruning, restoreEnablePackagePruning.ToString())
                    .WithItem(ProjectItems.PrunePackageReference, "PackageA", [new("Version", "1.0.0")]);
                })
                .Build();

            // Act
            var actualRestoreSpec = VsSolutionRestoreService.ToPackageSpec(projectName, projectRestoreInfo);

            // Assert
            if (restoreEnablePackagePruning)
            {
                actualRestoreSpec.TargetFrameworks[0].PackagesToPrune.Should().HaveCount(1);
                KeyValuePair<string, PrunePackageReference> result = actualRestoreSpec.TargetFrameworks[0].PackagesToPrune.Single();
                result.Key.Should().Be("PackageA");
                result.Value.Name.Should().Be("PackageA");
                result.Value.VersionRange.Should().Be(VersionRange.Parse("(, 1.0.0]"));
            }
            else
            {
                actualRestoreSpec.TargetFrameworks[0].PackagesToPrune.Should().BeEmpty();

            }
        }

        [Fact]
        public void ToPackageSpec_WithPrunePackageReference_WithMissingVersion_Throws()
        {
            // Arrange
            ProjectNames projectName = new(@"n:\path\to\current\project.csproj", "project", "project.csproj", "project", Guid.NewGuid().ToString());

            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netstandard2.0", builder =>
                {
                    builder
                    .WithProperty(ProjectBuildProperties.RestoreEnablePackagePruning, "true")
                    .WithItem(ProjectItems.PrunePackageReference, "PackageA", []);
                })
                .Build();

            // Act & Assert
            var result = Assert.Throws<ArgumentException>(() => VsSolutionRestoreService.ToPackageSpec(projectName, projectRestoreInfo));
            result.Message.Should().Contain("PrunePackageReference");
        }

        [Fact]
        public void ToPackageSpec_WithPrunePackageReference_AccountsForDefaults()
        {
            // Arrange
            ProjectNames projectName = new(@"n:\path\to\current\project.csproj", "project", "project.csproj", "project", Guid.NewGuid().ToString());

            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("netstandard2.0", builder =>
                {
                    builder
                    .WithProperty(ProjectBuildProperties.RestorePackagePruningDefault, "false")
                    .WithItem(ProjectItems.PrunePackageReference, "PackageA", [new("Version", "1.0.0")]);
                })
                .WithTargetFrameworkInfo("net10.0", builder =>
                {
                    builder
                    .WithProperty(ProjectBuildProperties.RestorePackagePruningDefault, "true")
                    .WithItem(ProjectItems.PrunePackageReference, "PackageB", [new("Version", "1.0.0")]);
                })
                .Build();

            // Act
            var actualRestoreSpec = VsSolutionRestoreService.ToPackageSpec(projectName, projectRestoreInfo);

            // Assert
            actualRestoreSpec.TargetFrameworks[0].PackagesToPrune.Should().HaveCount(1);
            KeyValuePair<string, PrunePackageReference> result = actualRestoreSpec.TargetFrameworks[0].PackagesToPrune.Single();
            result.Key.Should().Be("PackageA");
            result.Value.Name.Should().Be("PackageA");
            result.Value.VersionRange.Should().Be(VersionRange.Parse("(, 1.0.0]"));

            actualRestoreSpec.TargetFrameworks[1].PackagesToPrune.Should().HaveCount(1);
            KeyValuePair<string, PrunePackageReference> resultB = actualRestoreSpec.TargetFrameworks[1].PackagesToPrune.Single();
            resultB.Key.Should().Be("PackageB");
            resultB.Value.Name.Should().Be("PackageB");
            resultB.Value.VersionRange.Should().Be(VersionRange.Parse("(, 1.0.0]"));
        }

        private async Task<DependencyGraphSpec> CaptureNominateResultAsync(
            IVsProjectRestoreInfo3 pri,
            Mock<IProjectSystemCache>? cache = null)
        {
            DependencyGraphSpec? capturedRestoreSpec = null;

            if (cache == null)
            {
                var projectFullPath =
                    pri.TargetFrameworks[0].Properties["MSBuildProjectFullPath"]
                    ?? throw new ArgumentException(paramName: nameof(pri), message: "Property MSBuildProjectFullPath not set");

                cache = CreateDefaultIProjectSystemCacheMock(projectFullPath);
            }

            cache
                .Setup(x => x.AddProjectRestoreInfo(
                    It.IsAny<ProjectNames>(),
                    It.IsAny<DependencyGraphSpec>(),
                    It.IsAny<IReadOnlyList<IAssetsLogMessage>>()))
                .Callback<ProjectNames, DependencyGraphSpec, IReadOnlyList<IAssetsLogMessage>>(
                    (_, dg, __) => { capturedRestoreSpec = dg; })
                .Returns(true);

            // Act
            var result = await NominateProjectAsync(pri, CancellationToken.None, cache: cache);

            Assert.True(result, "Project restore nomination should succeed.");
            capturedRestoreSpec.Should().NotBeNull("Project restore nomination should produce a DependencyGraphSpec.");

            return capturedRestoreSpec;
        }

        private async Task<IReadOnlyList<IAssetsLogMessage>> CaptureAdditionalMessagesAsync(
            IVsProjectRestoreInfo3 pri,
            Mock<ISolutionRestoreWorker>? restoreWorker = null)
        {
            IReadOnlyList<IAssetsLogMessage>? additionalMessages = null;

            string projectFullPath = pri.TargetFrameworks[0].Properties["MSBuildProjectFullPath"]
                ?? throw new ArgumentException(paramName: nameof(pri), message: "Property MSBuildProjectFullPath not set");

            var cache = CreateDefaultIProjectSystemCacheMock(projectFullPath);

            cache
                .Setup(x => x.AddProjectRestoreInfo(
                    It.IsAny<ProjectNames>(),
                    It.IsAny<DependencyGraphSpec>(),
                    It.IsAny<IReadOnlyList<IAssetsLogMessage>>()))
                .Callback<ProjectNames, DependencyGraphSpec, IReadOnlyList<IAssetsLogMessage>>(
                    (_, _, am) => { additionalMessages = am; })
                .Returns(true);

            var logger = new Mock<ILogger>();

            // Act
            var result = await NominateProjectAsync(pri, CancellationToken.None, cache: cache, restoreWorker, logger);

            Assert.True(result, "Project restore nomination should succeed.");
            logger.Verify(l => l.LogError(It.IsAny<string>()), Times.Never);

            return additionalMessages ?? [];
        }

        private Task<bool> NominateProjectAsync(
            IVsProjectRestoreInfo3 pri,
            CancellationToken cancellationToken,
            Mock<IProjectSystemCache>? cache = null,
            Mock<ISolutionRestoreWorker>? restoreWorker = null,
            Mock<ILogger>? logger = null)
        {
            var projectFullPath =
                pri.TargetFrameworks[0].Properties["MSBuildProjectFullPath"]
                ?? throw new ArgumentException(paramName: nameof(pri), message: "Property MSBuildProjectFullPath not set");

            if (cache == null)
            {
                cache = CreateDefaultIProjectSystemCacheMock(projectFullPath);
            }

            if (restoreWorker == null)
            {
                restoreWorker = CreateDefaultISolutionRestoreWorkerMock();
            }

            if (logger == null)
            {
                logger = new Mock<ILogger>();
            }
            var vsSolution2 = Mock.Of<IVsSolution2>();
            var asyncLazySolution2 = new Microsoft.VisualStudio.Threading.AsyncLazy<IVsSolution2>(() => Task.FromResult(vsSolution2));
            var telemetryProvider = Mock.Of<INuGetTelemetryProvider>();

            var service = new VsSolutionRestoreService(cache.Object, restoreWorker.Object, logger.Object, asyncLazySolution2, telemetryProvider);

            return ((IVsSolutionRestoreService5)service).NominateProjectAsync(projectFullPath, pri, cancellationToken);
        }

        private Mock<IProjectSystemCache> CreateDefaultIProjectSystemCacheMock(string projectFullPath)
        {
            var projectNames = new ProjectNames(
                fullName: projectFullPath,
                uniqueName: Path.GetFileName(projectFullPath),
                shortName: Path.GetFileNameWithoutExtension(projectFullPath),
                customUniqueName: Path.GetFileName(projectFullPath),
                projectId: Guid.NewGuid().ToString());

            var cache = new Mock<IProjectSystemCache>();
            cache
                .Setup(x => x.TryGetProjectNames(projectFullPath, out It.Ref<ProjectNames>.IsAny))
                .Returns(new TryGetProjectNamesReturns((string projectPath, out ProjectNames pn) =>
                {
                    pn = projectNames;
                    return true;
                }));

            cache
                .Setup(x => x.AddProjectRestoreInfo(
                    It.IsAny<ProjectNames>(),
                    It.IsAny<DependencyGraphSpec>(),
                    It.IsAny<IReadOnlyList<IAssetsLogMessage>>()))
                .Returns(true);

            return cache;
        }

        private Mock<ISolutionRestoreWorker> CreateDefaultISolutionRestoreWorkerMock()
        {
            var restoreWorker = new Mock<ISolutionRestoreWorker>();

            restoreWorker
                .Setup(x => x.ScheduleRestoreAsync(It.IsAny<SolutionRestoreRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            return restoreWorker;
        }

        private static void AssertPackages(TargetFrameworkInformation actualTfi, params string[] expectedPackages)
        {
            var actualPackages = actualTfi
                .Dependencies
                .Where(ld => ld.LibraryRange.TypeConstraint == LibraryDependencyTarget.Package)
                .Select(ld => $"{ld.Name}:{ld.LibraryRange.VersionRange?.OriginalString}");

            Assert.Equal(expectedPackages, actualPackages);
        }

        private static void AssertDownloadPackages(TargetFrameworkInformation actualTfi, params string[] expectedPackages)
        {
            var actualPackages = actualTfi
                .DownloadDependencies
                .Select(ld => $"{ld.Name}:{ld.VersionRange.OriginalString}");

            Assert.Equal(expectedPackages, actualPackages);
        }

        private static void AssertFrameworkReferences(TargetFrameworkInformation actualTfi, params string[] expectedPackages)
        {
            Assert.Equal(expectedPackages,
                actualTfi.FrameworkReferences
                .OrderBy(e => e)
                .Select(e => e.Name + ":" + FrameworkDependencyFlagsUtils.GetFlagString(e.PrivateAssets))
                .ToArray());
        }
    }
}
