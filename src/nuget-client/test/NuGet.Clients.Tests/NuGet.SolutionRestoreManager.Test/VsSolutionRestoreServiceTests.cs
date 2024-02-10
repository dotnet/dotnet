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
using NuGet.Test.Utility;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Telemetry;
using Test.Utility.ProjectManagement;
using Xunit;
using static NuGet.Frameworks.FrameworkConstants;

namespace NuGet.SolutionRestoreManager.Test
{
    [Collection(DispatcherThreadCollection.CollectionName)]
    public class VsSolutionRestoreServiceTests : IDisposable
    {
        private readonly TestDirectory _testDirectory;

        public VsSolutionRestoreServiceTests()
        {
            _testDirectory = TestDirectory.Create();

#pragma warning disable VSSDK005 // Avoid instantiating JoinableTaskContext
            var joinableTaskContext = new JoinableTaskContext(Thread.CurrentThread, SynchronizationContext.Current);
#pragma warning restore VSSDK005 // Avoid instantiating JoinableTaskContext

            NuGetUIThreadHelper.SetCustomJoinableTaskFactory(joinableTaskContext.Factory);
        }

        public void Dispose()
        {
            _testDirectory.Dispose();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void NominateProjectAsync_Always_SchedulesAutoRestore(bool isV2Nomination)
        {
            var cps = NewCpsProject();

            var restoreWorker = CreateDefaultISolutionRestoreWorkerMock();

            // Act
            var actualRestoreTask = isV2Nomination ? NominateProjectAsync(cps.ProjectFullPath, cps.ProjectRestoreInfo2, CancellationToken.None, restoreWorker: restoreWorker)
                : NominateProjectAsync(cps.ProjectFullPath, cps.ProjectRestoreInfo, CancellationToken.None, restoreWorker: restoreWorker);

            restoreWorker
                .Verify(
                    x => x.ScheduleRestoreAsync(It.IsAny<SolutionRestoreRequest>(), CancellationToken.None),
                    Times.Once(),
                    "Service should schedule auto-restore operation.");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task NominateProjectAsync_ConsoleAppTemplate(bool isV2Nomination)
        {
            var consoleAppProjectJson = @"{
    ""frameworks"": {
        ""netcoreapp1.0"": {
            ""dependencies"": {
                ""Microsoft.NET.Sdk"": {
                    ""target"": ""Package"",
                    ""version"": ""1.0.0-alpha-20161019-1""
                },
                ""Microsoft.NETCore.App"": {
                    ""target"": ""Package"",
                    ""version"": ""1.0.1""
                }
            }
        }
    }
}";
            var projectName = "ConsoleApp1";
            var cps = NewCpsProject(consoleAppProjectJson, projectName);
            var projectFullPath = cps.ProjectFullPath;
            var expectedBaseIntermediate = cps.ProjectRestoreInfo.BaseIntermediatePath == cps.ProjectRestoreInfo2.BaseIntermediatePath ? cps.ProjectRestoreInfo.BaseIntermediatePath : "The test builder is broken!";

            // Act
            var actualRestoreSpec = isV2Nomination ?
                await CaptureNominateResultAsync(projectFullPath, cps.ProjectRestoreInfo2) :
                await CaptureNominateResultAsync(projectFullPath, cps.ProjectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.GetProjectSpec(projectFullPath);
            Assert.NotNull(actualProjectSpec);
            Assert.Equal("1.0.0", actualProjectSpec.Version.ToString());

            var actualMetadata = actualProjectSpec.RestoreMetadata;
            Assert.NotNull(actualMetadata);
            Assert.Equal(projectFullPath, actualMetadata.ProjectPath);
            Assert.Equal(projectName, actualMetadata.ProjectName);
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task NominateProjectAsync_ConsoleAppTemplateWithPlatform(bool isV2Nomination)
        {
            var consoleAppProjectJson = @"{
    ""frameworks"": {
        ""net5.0-windows10.0"": {
            ""dependencies"": {
                ""Microsoft.NET.Sdk"": {
                    ""target"": ""Package"",
                    ""version"": ""1.0.0-alpha-20161019-1""
                },
                ""Microsoft.NETCore.App"": {
                    ""target"": ""Package"",
                    ""version"": ""1.0.1""
                }
            }
        }
    }
}";
            var projectName = "ConsoleApp1";
            var cps = NewCpsProject(consoleAppProjectJson, projectName);
            var projectFullPath = cps.ProjectFullPath;
            var expectedBaseIntermediate = cps.ProjectRestoreInfo.BaseIntermediatePath == cps.ProjectRestoreInfo2.BaseIntermediatePath ? cps.ProjectRestoreInfo.BaseIntermediatePath : "The test builder is broken!";

            // Act
            var actualRestoreSpec = isV2Nomination ?
                await CaptureNominateResultAsync(projectFullPath, cps.ProjectRestoreInfo2) :
                await CaptureNominateResultAsync(projectFullPath, cps.ProjectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.GetProjectSpec(projectFullPath);
            Assert.NotNull(actualProjectSpec);
            Assert.Equal("1.0.0", actualProjectSpec.Version.ToString());

            var actualMetadata = actualProjectSpec.RestoreMetadata;
            Assert.NotNull(actualMetadata);
            Assert.Equal(projectFullPath, actualMetadata.ProjectPath);
            Assert.Equal(projectName, actualMetadata.ProjectName);
            Assert.Equal(ProjectStyle.PackageReference, actualMetadata.ProjectStyle);
            Assert.Equal(expectedBaseIntermediate, actualMetadata.OutputPath);

            Assert.Single(actualProjectSpec.TargetFrameworks);
            var actualTfi = actualProjectSpec.TargetFrameworks.Single();

            var expectedFramework = NuGetFramework.Parse("net5.0-windows10.0");
            Assert.Equal(expectedFramework, actualTfi.FrameworkName);

            AssertPackages(actualTfi,
                "Microsoft.NET.Sdk:1.0.0-alpha-20161019-1",
                "Microsoft.NETCore.App:1.0.1");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task NominateProjectAsync_WithCliTool(bool isV2Nomination)
        {
            const string toolProjectJson = @"{
    ""frameworks"": {
        ""netcoreapp1.0"": { }
    }
}";
            var cps = NewCpsProject(toolProjectJson);
            var builder = cps.Builder.WithTool("Foo.Test.Tools", "2.0.0");
            var projectFullPath = cps.ProjectFullPath;

            // Act
            var actualRestoreSpec = isV2Nomination ?
                await CaptureNominateResultAsync(projectFullPath, builder.ProjectRestoreInfo2) :
                await CaptureNominateResultAsync(projectFullPath, builder.ProjectRestoreInfo);


            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.GetProjectSpec(projectFullPath);
            Assert.NotNull(actualProjectSpec);

            var actualToolSpec = actualRestoreSpec
                .Projects
                .Where(p => !object.ReferenceEquals(p, actualProjectSpec))
                .Single();
            var actualMetadata = actualToolSpec.RestoreMetadata;
            Assert.NotNull(actualMetadata);
            Assert.Equal(projectFullPath, actualMetadata.ProjectPath);
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
            Assert.Equal("2.0.0", actualToolLibrary.LibraryRange.VersionRange.OriginalString);
        }

        [Theory]
        [InlineData("netcoreapp2.0", @"..\packages", @"..\source1;..\source2", @"..\fallback1;..\fallback2", false)]
        [InlineData("netcoreapp2.0", @"C:\packagesPath", @"..\source1;..\source2", @"C:\fallback1;C:\fallback2", false)]
        [InlineData("netcoreapp2.0", null, null, null, false)]
        [InlineData("netcoreapp1.0", null, null, null, false)]
        [InlineData("netcoreapp2.0", @"..\packages", @"..\source1;..\source2", @"..\fallback1;..\fallback2")]
        [InlineData("netcoreapp2.0", @"C:\packagesPath", @"..\source1;..\source2", @"C:\fallback1;C:\fallback2")]
        [InlineData("netcoreapp2.0", null, null, null)]
        [InlineData("netcoreapp1.0", null, null, null)]
        public async Task NominateProjectAsync_WithCliTool_RestoreSettings(
            string toolFramework, string restorePackagesPath, string restoreSources, string fallbackFolders, bool isV2Nominate = true)
        {
            var expectedToolFramework = NuGetFramework.Parse(toolFramework);

            var tfms = isV2Nominate ?
                (IVsTargetFrameworkInfo)
                new VsTargetFrameworkInfo2(
                        "netcoreapp2.0",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] { new VsProjectProperty("RestorePackagesPath", restorePackagesPath),
                                new VsProjectProperty("RestoreSources", restoreSources),
                                new VsProjectProperty("RestoreFallbackFolders", fallbackFolders),
                                new VsProjectProperty("DotnetCliToolTargetFramework", toolFramework) }) :
                new VsTargetFrameworkInfo(
                        "netcoreapp2.0",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] { new VsProjectProperty("RestorePackagesPath", restorePackagesPath),
                                new VsProjectProperty("RestoreSources", restoreSources),
                                new VsProjectProperty("RestoreFallbackFolders", fallbackFolders),
                                new VsProjectProperty("DotnetCliToolTargetFramework", toolFramework) });
            var cps = NewCpsProject(@"{ }");
            var builder = cps.Builder
                .WithTool("Foo.Test.Tools", "2.0.0")
                .WithTargetFrameworkInfo(tfms);

            var projectFullPath = cps.ProjectFullPath;

            // Act
            var actualRestoreSpec = isV2Nominate ?
                await CaptureNominateResultAsync(projectFullPath, builder.ProjectRestoreInfo2) :
                await CaptureNominateResultAsync(projectFullPath, builder.ProjectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.GetProjectSpec(projectFullPath);
            Assert.NotNull(actualProjectSpec);

            var actualToolSpec = actualRestoreSpec
                .Projects
                .Where(p => !object.ReferenceEquals(p, actualProjectSpec))
                .Single();
            var actualMetadata = actualToolSpec.RestoreMetadata;
            Assert.NotNull(actualMetadata);
            Assert.Equal(projectFullPath, actualMetadata.ProjectPath);
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
            Assert.Equal("2.0.0", actualToolLibrary.LibraryRange.VersionRange.OriginalString);

            Assert.Equal(restorePackagesPath, actualToolSpec.RestoreMetadata.PackagesPath);

            var specSources = actualToolSpec.RestoreMetadata.Sources?.Select(e => e.Source);
            var expectedSources = MSBuildStringUtility.Split(restoreSources);
            Assert.True(Enumerable.SequenceEqual(expectedSources.OrderBy(t => t), specSources.OrderBy(t => t)));

            var specFallback = actualToolSpec.RestoreMetadata.FallbackFolders;
            var expectedFallback = MSBuildStringUtility.Split(fallbackFolders);
            Assert.True(Enumerable.SequenceEqual(expectedFallback.OrderBy(t => t), specFallback.OrderBy(t => t)));
        }

        [Theory]
        [InlineData(
@"{
    ""frameworks"": {
        ""netstandard1.4"": { }
    }
}", "netstandard1.4", "netstandard1.4")]
        [InlineData(
@"{
    ""frameworks"": {
        ""netstandard1.4"": {
            ""targetAlias"": ""netstandard1.4""
        },
        ""net46"": {
            ""targetAlias"": ""net46""
        }
    }
}", "netstandard1.4;net46", "netstandard1.4;net46")]
        [InlineData(
@"{
    ""frameworks"": {
        ""netstandard1.4"": { },
        ""net46"": { }
    }
}", "\r\n    netstandard1.4;\r\n    net46\r\n    ", "netstandard1.4;net46")]
        [InlineData(
@"{
    ""frameworks"": {
        ""netstandard1.4"": { }
    }
}", "netstandard1.4", "netstandard1.4", false)]
        [InlineData(
@"{
    ""frameworks"": {
        ""netstandard1.4"": {
            ""targetAlias"": ""netstandard1.4""
        },
        ""net46"": {
            ""targetAlias"": ""net46""
        }
    }
}", "netstandard1.4;net46", "netstandard1.4;net46", false)]
        [InlineData(
@"{
    ""frameworks"": {
        ""netstandard1.4"": { },
        ""net46"": { }
    }
}", "\r\n    netstandard1.4;\r\n    net46\r\n    ", "netstandard1.4;net46", false)]
        public async Task NominateProjectAsync_CrossTargeting(
            string projectJson, string rawOriginalTargetFrameworks, string expectedOriginalTargetFrameworks, bool isV2Nominate = true)
        {
            var cps = NewCpsProject(
                projectJson: projectJson,
                crossTargeting: true);
            var projectFullPath = cps.ProjectFullPath;
            var pri = cps.ProjectRestoreInfo;
            var pri2 = cps.ProjectRestoreInfo2;

            pri.OriginalTargetFrameworks = rawOriginalTargetFrameworks;
            pri2.OriginalTargetFrameworks = rawOriginalTargetFrameworks;

            // Act
            var actualRestoreSpec = isV2Nominate ?
                await CaptureNominateResultAsync(projectFullPath, pri) :
                await CaptureNominateResultAsync(projectFullPath, pri2);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.GetProjectSpec(projectFullPath);
            Assert.NotNull(actualProjectSpec);

            var actualMetadata = actualProjectSpec.RestoreMetadata;
            Assert.NotNull(actualMetadata);
            Assert.True(actualMetadata.CrossTargeting);

            var actualOriginalTargetFrameworks = string.Join(";", actualMetadata.OriginalTargetFrameworks);
            Assert.Equal(
                expectedOriginalTargetFrameworks,
                actualOriginalTargetFrameworks);
        }

        [Theory]
        [InlineData(
@"{
    ""frameworks"": {
        ""net5.0-windows7.0"": {
            ""targetAlias"": ""net5.0-windows""
        }
    }
}", "", "net5.0-windows")]
        public async Task NominateProjectAsync_WithoutOriginalTargetFrameworks_SetOriginalTargetFrameworksToAlias(
            string projectJson, string rawOriginalTargetFrameworks, string expectedOriginalTargetFrameworks)
        {
            var cps = NewCpsProject(
                projectJson: projectJson,
                crossTargeting: true);
            var projectFullPath = cps.ProjectFullPath;
            var pri2 = cps.ProjectRestoreInfo2;

            pri2.OriginalTargetFrameworks = rawOriginalTargetFrameworks;

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectFullPath, pri2);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.GetProjectSpec(projectFullPath);
            Assert.NotNull(actualProjectSpec);

            var actualMetadata = actualProjectSpec.RestoreMetadata;
            Assert.NotNull(actualMetadata);
            Assert.False(actualMetadata.CrossTargeting);

            var actualOriginalTargetFrameworks = string.Join(";", actualMetadata.OriginalTargetFrameworks);
            Assert.Equal(
                expectedOriginalTargetFrameworks,
                actualOriginalTargetFrameworks);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task NominateProjectAsync_Imports(bool isV2Nomination)
        {
            const string projectJson = @"{
    ""frameworks"": {
        ""netstandard1.4"": {
            ""imports"": [""dotnet5.3"",""portable-net452+win81""]
        }
    },
}";
            var cps = NewCpsProject(projectJson);
            var projectFullPath = cps.ProjectFullPath;

            // Act
            var actualRestoreSpec = isV2Nomination ?
                await CaptureNominateResultAsync(projectFullPath, cps.ProjectRestoreInfo2) :
                await CaptureNominateResultAsync(projectFullPath, cps.ProjectRestoreInfo);
            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.GetProjectSpec(projectFullPath);
            Assert.NotNull(actualProjectSpec);

            var actualTfi = actualProjectSpec.TargetFrameworks.Single();
            var actualImports = string.Join(";", actualTfi.Imports.Select(x => x.GetShortFolderName()));
            Assert.Equal("dotnet53;portable-net452+win81", actualImports);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task NominateProjectAsync_WithValidPackageVersion(bool isV2Nomination)
        {
            const string projectJson = @"{
    ""version"": ""1.2.0-beta1"",
    ""frameworks"": {
        ""netcoreapp1.0"": { }
    }
}";
            var cps = NewCpsProject(projectJson);
            var projectFullPath = cps.ProjectFullPath;

            // Act
            var actualRestoreSpec = isV2Nomination ?
                await CaptureNominateResultAsync(projectFullPath, cps.ProjectRestoreInfo2) :
                await CaptureNominateResultAsync(projectFullPath, cps.ProjectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.GetProjectSpec(projectFullPath);
            Assert.NotNull(actualProjectSpec);
            Assert.Equal("1.2.0-beta1", actualProjectSpec.Version.ToString());
        }

        [Theory]
        [InlineData("1.2.3", "1.2.3")]
        [InlineData("1.2.0-beta1", "1.2.0-beta1")]
        [InlineData("1.0.0", "1.0.0.0")]
        public async Task NominateProjectAsync_WithIdenticalPackageVersions(string version1, string version2)
        {
            var cps = NewCpsProject("{ }");
            var projectFullPath = cps.ProjectFullPath;
            var pri = cps.Builder
                .WithTargetFrameworkInfo(
                    new VsTargetFrameworkInfo(
                        "netcoreapp1.0",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] { new VsProjectProperty("PackageVersion", version1) }))
                .WithTargetFrameworkInfo(
                    new VsTargetFrameworkInfo(
                        "net46",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] { new VsProjectProperty("PackageVersion", version2) }))
                .ProjectRestoreInfo;

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectFullPath, cps.ProjectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.GetProjectSpec(projectFullPath);
            Assert.NotNull(actualProjectSpec);
            Assert.Equal(version1, actualProjectSpec.Version.ToString());
        }

        [Theory]
        [InlineData("1.2.3", "1.2.3")]
        [InlineData("1.2.0-beta1", "1.2.0-beta1")]
        [InlineData("1.0.0", "1.0.0.0")]
        public async Task NominateProjectAsync_PRI2_WithIdenticalPackageVersions(string version1, string version2)
        {
            var cps = NewCpsProject("{ }");
            var projectFullPath = cps.ProjectFullPath;
            var pri = cps.Builder
                .WithTargetFrameworkInfo(
                    new VsTargetFrameworkInfo2(
                        "netcoreapp1.0",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] { new VsProjectProperty("PackageVersion", version1) }))
                .WithTargetFrameworkInfo(
                    new VsTargetFrameworkInfo2(
                        "net46",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] { new VsProjectProperty("PackageVersion", version2) }))
                .ProjectRestoreInfo2;

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectFullPath, pri);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.GetProjectSpec(projectFullPath);
            Assert.NotNull(actualProjectSpec);
            Assert.Equal(version1, actualProjectSpec.Version.ToString());
        }

        // The data in the restore settings should be unprocessed meaning the paths should stay relative.
        // The processing of the paths will be done later in the netcorepackagereferenceproject
        [Theory]
        [InlineData(@"..\packages", @"..\source1;..\source2", @"..\fallback1;..\fallback2", false)]
        [InlineData(@"C:\packagesPath", @"..\source1;..\source2", @"C:\fallback1;C:\fallback2", false)]
        [InlineData(null, null, null, false)]
        [InlineData(@"..\packages", @"..\source1;..\source2", @"..\fallback1;..\fallback2", true)]
        [InlineData(@"C:\packagesPath", @"..\source1;..\source2", @"C:\fallback1;C:\fallback2", true)]
        [InlineData(null, null, null, true)]
        public async Task NominateProjectAsync_RestoreSettings(string restorePackagesPath, string restoreSources, string fallbackFolders, bool isV2Nominate)
        {
            var cps = NewCpsProject("{ }");
            var projectFullPath = cps.ProjectFullPath;

            var vstfm = isV2Nominate ?
                (IVsTargetFrameworkInfo)new VsTargetFrameworkInfo2(
                        "netcoreapp1.0",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] {new VsProjectProperty("RestorePackagesPath", restorePackagesPath),
                               new VsProjectProperty("RestoreSources", restoreSources),
                               new VsProjectProperty("RestoreFallbackFolders", fallbackFolders)}) :

                new VsTargetFrameworkInfo(
                        "netcoreapp1.0",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] {new VsProjectProperty("RestorePackagesPath", restorePackagesPath),
                               new VsProjectProperty("RestoreSources", restoreSources),
                               new VsProjectProperty("RestoreFallbackFolders", fallbackFolders)});

            var builder = cps.Builder
                .WithTargetFrameworkInfo(vstfm);

            // Act
            var actualRestoreSpec = isV2Nominate ?
                await CaptureNominateResultAsync(projectFullPath, builder.ProjectRestoreInfo2) :
                await CaptureNominateResultAsync(projectFullPath, builder.ProjectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.GetProjectSpec(projectFullPath);
            Assert.NotNull(actualProjectSpec);
            Assert.Equal(restorePackagesPath, actualProjectSpec.RestoreMetadata.PackagesPath);

            var specSources = actualProjectSpec.RestoreMetadata.Sources?.Select(e => e.Source);
            var expectedSources = MSBuildStringUtility.Split(restoreSources);
            Assert.True(Enumerable.SequenceEqual(expectedSources.OrderBy(t => t), specSources.OrderBy(t => t)));

            var specFallback = actualProjectSpec.RestoreMetadata.FallbackFolders;
            var expectedFallback = MSBuildStringUtility.Split(fallbackFolders);
            Assert.True(Enumerable.SequenceEqual(expectedFallback.OrderBy(t => t), specFallback.OrderBy(t => t)));
        }

        [Theory]
        [InlineData(@"..\packages", @"..\source1;..\source2", @"..\fallback1;Clear;..\fallback2", false)]
        [InlineData(@"C:\packagesPath", @"Clear;..\source1;..\source2", @"C:\fallback1;C:\fallback2", false)]
        [InlineData(@"..\packages", @"..\source1;..\source2", @"..\fallback1;Clear;..\fallback2", true)]
        [InlineData(@"C:\packagesPath", @"Clear;..\source1;..\source2", @"C:\fallback1;C:\fallback2", true)]
        public async Task NominateProjectAsync_RestoreSettingsClear(string restorePackagesPath, string restoreSources, string fallbackFolders, bool isV2Nominate)
        {
            var vstfm = isV2Nominate ?
                (IVsTargetFrameworkInfo)
                new VsTargetFrameworkInfo2(
                    "netcoreapp1.0",
                    Enumerable.Empty<IVsReferenceItem>(),
                    Enumerable.Empty<IVsReferenceItem>(),
                    Enumerable.Empty<IVsReferenceItem>(),
                    Enumerable.Empty<IVsReferenceItem>(),
                    new[] {new VsProjectProperty("RestorePackagesPath", restorePackagesPath),
                            new VsProjectProperty("RestoreSources", restoreSources),
                            new VsProjectProperty("RestoreFallbackFolders", fallbackFolders)}) :
                new VsTargetFrameworkInfo(
                    "netcoreapp1.0",
                    Enumerable.Empty<IVsReferenceItem>(),
                    Enumerable.Empty<IVsReferenceItem>(),
                    new[] {new VsProjectProperty("RestorePackagesPath", restorePackagesPath),
                            new VsProjectProperty("RestoreSources", restoreSources),
                            new VsProjectProperty("RestoreFallbackFolders", fallbackFolders)});

            var cps = NewCpsProject("{ }");
            var projectFullPath = cps.ProjectFullPath;
            var builder = cps.Builder
                .WithTargetFrameworkInfo(
                    vstfm);

            // Act
            var actualRestoreSpec = isV2Nominate ?
                await CaptureNominateResultAsync(projectFullPath, builder.ProjectRestoreInfo2) :
                await CaptureNominateResultAsync(projectFullPath, builder.ProjectRestoreInfo);
            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.GetProjectSpec(projectFullPath);
            Assert.NotNull(actualProjectSpec);
            Assert.Equal(restorePackagesPath, actualProjectSpec.RestoreMetadata.PackagesPath);

            var specSources = actualProjectSpec.RestoreMetadata.Sources?.Select(e => e.Source);
            var expectedSources = MSBuildStringUtility.Split(restoreSources).Any(e => StringComparer.OrdinalIgnoreCase.Equals("clear", e)) ? new string[] { "Clear" } : MSBuildStringUtility.Split(restoreSources);
            Assert.True(Enumerable.SequenceEqual(expectedSources.OrderBy(t => t), specSources.OrderBy(t => t)));

            var specFallback = actualProjectSpec.RestoreMetadata.FallbackFolders;
            var expectedFallback = MSBuildStringUtility.Split(fallbackFolders).Any(e => StringComparer.OrdinalIgnoreCase.Equals("clear", e)) ? new string[] { "Clear" } : MSBuildStringUtility.Split(fallbackFolders);
            Assert.True(Enumerable.SequenceEqual(expectedFallback.OrderBy(t => t), specFallback.OrderBy(t => t)));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task NominateProjectAsync_CacheFilePathInPackageSpec_Succeeds(bool isV2Nominate)
        {

            var vstfm = isV2Nominate ?
                        (IVsTargetFrameworkInfo)
                        new VsTargetFrameworkInfo2(
                            "netcoreapp1.0",
                            Enumerable.Empty<IVsReferenceItem>(),
                            Enumerable.Empty<IVsReferenceItem>(),
                            Enumerable.Empty<IVsReferenceItem>(),
                            Enumerable.Empty<IVsReferenceItem>(),
                            new IVsProjectProperty[] { }) :
                        new VsTargetFrameworkInfo(
                            "netcoreapp1.0",
                            Enumerable.Empty<IVsReferenceItem>(),
                            Enumerable.Empty<IVsReferenceItem>(),
                            new IVsProjectProperty[] { });

            var cps = NewCpsProject("{ }");
            var projectFullPath = cps.ProjectFullPath;
            var builder = cps.Builder
                .WithTargetFrameworkInfo(vstfm);

            // Act
            var actualRestoreSpec = isV2Nominate ?
                         await CaptureNominateResultAsync(projectFullPath, builder.ProjectRestoreInfo2) :
                         await CaptureNominateResultAsync(projectFullPath, builder.ProjectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.GetProjectSpec(projectFullPath);
            Assert.NotNull(actualProjectSpec);
            Assert.Equal(Path.Combine(actualProjectSpec.RestoreMetadata.OutputPath, NoOpRestoreUtilities.NoOpCacheFileName), actualProjectSpec.RestoreMetadata.CacheFilePath);
        }

        [Theory]
        [InlineData("1.0.0", "1.2.3")]
        [InlineData("1.0.0", "")]
        [InlineData("1.0.0", "   ")]
        [InlineData("1.0.0", null)]
        public async Task NominateProjectAsync_WithDifferentPackageVersions_Fails(string version1, string version2)
        {
            var cps = NewCpsProject("{ }");
            var projectFullPath = cps.ProjectFullPath;
            var pri = cps.Builder
                .WithTargetFrameworkInfo(
                    new VsTargetFrameworkInfo(
                        "netcoreapp1.0",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] { new VsProjectProperty("PackageVersion", version1) }))
                .WithTargetFrameworkInfo(
                    new VsTargetFrameworkInfo(
                        "net46",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] { new VsProjectProperty("PackageVersion", version2) }))
                .ProjectRestoreInfo;

            var cache = Mock.Of<IProjectSystemCache>();
            var restoreWorker = Mock.Of<ISolutionRestoreWorker>();
            var vsSolution2 = Mock.Of<IVsSolution2>();
            var asyncLazySolution2 = new Microsoft.VisualStudio.Threading.AsyncLazy<IVsSolution2>(() => Task.FromResult(vsSolution2));
            var telemetryProvider = Mock.Of<INuGetTelemetryProvider>();

            var service = new VsSolutionRestoreService(
                cache, restoreWorker, NuGet.Common.NullLogger.Instance, asyncLazySolution2, telemetryProvider);

            // Act
            var result = await service.NominateProjectAsync(projectFullPath, pri, CancellationToken.None);

            Assert.False(result, "Project restore nomination must fail.");
        }

        [Theory]
        [InlineData("1.0.0", "1.2.3")]
        [InlineData("1.0.0", "")]
        [InlineData("1.0.0", "   ")]
        [InlineData("1.0.0", null)]
        public async Task NominateProjectAsync_PRI2_WithDifferentPackageVersions_Fails(string version1, string version2)
        {
            var cps = NewCpsProject("{ }");
            var projectFullPath = cps.ProjectFullPath;
            var pri = cps.Builder
                .WithTargetFrameworkInfo(
                    new VsTargetFrameworkInfo2(
                        "netcoreapp1.0",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] { new VsProjectProperty("PackageVersion", version1) }))
                .WithTargetFrameworkInfo(
                    new VsTargetFrameworkInfo2(
                        "net46",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] { new VsProjectProperty("PackageVersion", version2) }))
                .ProjectRestoreInfo2;

            var cache = Mock.Of<IProjectSystemCache>();
            var restoreWorker = Mock.Of<ISolutionRestoreWorker>();
            var vsSolution2 = Mock.Of<IVsSolution2>();
            var asyncLazySolution2 = new Microsoft.VisualStudio.Threading.AsyncLazy<IVsSolution2>(() => Task.FromResult(vsSolution2));
            var telemetryProvider = Mock.Of<INuGetTelemetryProvider>();

            var service = new VsSolutionRestoreService(
                cache, restoreWorker, NuGet.Common.NullLogger.Instance, asyncLazySolution2, telemetryProvider);

            // Act
            var result = await service.NominateProjectAsync(projectFullPath, pri, CancellationToken.None);

            Assert.False(result, "Project restore nomination must fail.");
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task NominateProjectAsync_PackageId(bool isV2Nominate)
        {
            var vstfm = isV2Nominate ?
                (IVsTargetFrameworkInfo)new VsTargetFrameworkInfo2(
                        "netcoreapp1.0",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] { new VsProjectProperty("PackageId", "TestPackage") }) :
                new VsTargetFrameworkInfo(
                        "netcoreapp1.0",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] { new VsProjectProperty("PackageId", "TestPackage") });

            var cps = NewCpsProject("{ }");
            var projectFullPath = cps.ProjectFullPath;
            var builder = cps.Builder
                .WithTargetFrameworkInfo(vstfm);

            // Act
            var actualRestoreSpec = isV2Nominate ?
                         await CaptureNominateResultAsync(projectFullPath, builder.ProjectRestoreInfo2) :
                         await CaptureNominateResultAsync(projectFullPath, builder.ProjectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.GetProjectSpec(projectFullPath);
            Assert.NotNull(actualProjectSpec);
            Assert.Equal("TestPackage", actualProjectSpec.Name);
        }

        [Fact]
        public async Task DifferingPackageIdsFailsWithUsableErrorMessage()
        {
            var cps = NewCpsProject("{ }");
            var projectFullPath = cps.ProjectFullPath;
            var pri = cps.Builder
                .WithTargetFrameworkInfo(
                    new VsTargetFrameworkInfo2(
                        "netcoreapp1.0",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] { new VsProjectProperty("PackageId", "PackageId.netcoreapp1.0") }))
                .WithTargetFrameworkInfo(
                    new VsTargetFrameworkInfo2(
                        "net46",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] { new VsProjectProperty("PackageId", "PackageId.net46") }))
                .ProjectRestoreInfo2;

            var cache = Mock.Of<IProjectSystemCache>();
            var restoreWorker = Mock.Of<ISolutionRestoreWorker>();
            var vsSolution2 = Mock.Of<IVsSolution2>();
            var asyncLazySolution2 = new Microsoft.VisualStudio.Threading.AsyncLazy<IVsSolution2>(() => Task.FromResult(vsSolution2));
            var telemetryProvider = Mock.Of<INuGetTelemetryProvider>();

            var service = new VsSolutionRestoreService(
                cache, restoreWorker, NuGet.Common.NullLogger.Instance, asyncLazySolution2, telemetryProvider);

            // Act
            var additionalMessages = await CaptureAdditionalMessagesAsync(projectFullPath, pri);

            // Assert
            var additionalMessage = Assert.Single(additionalMessages);
            Assert.Equal(NuGetLogCode.NU1105, additionalMessage.Code);
            Assert.Equal(projectFullPath, additionalMessage.ProjectPath);
            Assert.Equal(projectFullPath, additionalMessage.FilePath);
            Assert.Contains(string.Format(CultureInfo.CurrentCulture, Resources.PropertyDoesNotHaveSingleValue, "PackageId", "PackageId.net46, PackageId.netcoreapp1.0"), additionalMessage.Message);
        }

        [Fact]
        public async Task NominateProjectAsync_VerifySourcesAreCombinedAcrossFrameworks()
        {
            var cps = NewCpsProject(@"{ }");
            var pri = cps.Builder
                .WithTargetFrameworkInfo(
                    new VsTargetFrameworkInfo(
                        "netcoreapp1.0",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] { new VsProjectProperty("RestoreSources", "base"),
                                new VsProjectProperty("RestoreFallbackFolders", "base"),
                                new VsProjectProperty("RestoreAdditionalProjectSources", "a;d"),
                                new VsProjectProperty("RestoreAdditionalProjectFallbackFolders", "x;z")}))
                .WithTargetFrameworkInfo(
                    new VsTargetFrameworkInfo(
                        "netcoreapp2.0",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] { new VsProjectProperty("RestoreSources", "base"),
                                new VsProjectProperty("RestoreFallbackFolders", "base"),
                                new VsProjectProperty("RestoreAdditionalProjectSources", "b"),
                                new VsProjectProperty("RestoreAdditionalProjectFallbackFolders", "y")}))
                .WithTargetFrameworkInfo(
                    new VsTargetFrameworkInfo(
                        "netcoreapp2.1",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] { new VsProjectProperty("RestoreSources", "base"),
                                new VsProjectProperty("RestoreFallbackFolders", "base"),
                                new VsProjectProperty("RestoreAdditionalProjectSources", "b"),
                                new VsProjectProperty("RestoreAdditionalProjectFallbackFolders", "y")}))
                .ProjectRestoreInfo;
            var projectFullPath = cps.ProjectFullPath;

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectFullPath, pri);
            var metadata = actualRestoreSpec.Projects.Single().RestoreMetadata;

            // Assert
            metadata.Sources.Select(e => e.Source).Should().BeEquivalentTo(new[] { "base", VSRestoreSettingsUtilities.AdditionalValue, "a", "b", "d" });
            metadata.FallbackFolders.Should().BeEquivalentTo(new[] { "base", VSRestoreSettingsUtilities.AdditionalValue, "x", "y", "z" });
        }

        [Fact]
        public async Task NominateProjectAsync_PRI2_VerifySourcesAreCombinedAcrossFrameworks()
        {
            var cps = NewCpsProject(@"{ }");
            var pri = cps.Builder
                .WithTargetFrameworkInfo(
                    new VsTargetFrameworkInfo2(
                        "netcoreapp1.0",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] { new VsProjectProperty("RestoreSources", "base"),
                                new VsProjectProperty("RestoreFallbackFolders", "base"),
                                new VsProjectProperty("RestoreAdditionalProjectSources", "a;d"),
                                new VsProjectProperty("RestoreAdditionalProjectFallbackFolders", "x;z")}))
                .WithTargetFrameworkInfo(
                    new VsTargetFrameworkInfo2(
                        "netcoreapp2.0",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] { new VsProjectProperty("RestoreSources", "base"),
                                new VsProjectProperty("RestoreFallbackFolders", "base"),
                                new VsProjectProperty("RestoreAdditionalProjectSources", "b"),
                                new VsProjectProperty("RestoreAdditionalProjectFallbackFolders", "y")}))
                .WithTargetFrameworkInfo(
                    new VsTargetFrameworkInfo2(
                        "netcoreapp2.1",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] { new VsProjectProperty("RestoreSources", "base"),
                                new VsProjectProperty("RestoreFallbackFolders", "base"),
                                new VsProjectProperty("RestoreAdditionalProjectSources", "b"),
                                new VsProjectProperty("RestoreAdditionalProjectFallbackFolders", "y")}))
                .ProjectRestoreInfo2;
            var projectFullPath = cps.ProjectFullPath;

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectFullPath, pri);
            var metadata = actualRestoreSpec.Projects.Single().RestoreMetadata;

            // Assert
            metadata.Sources.Select(e => e.Source).Should().BeEquivalentTo(new[] { "base", VSRestoreSettingsUtilities.AdditionalValue, "a", "b", "d" });
            metadata.FallbackFolders.Should().BeEquivalentTo(new[] { "base", VSRestoreSettingsUtilities.AdditionalValue, "x", "y", "z" });
        }

        [Fact]
        public async Task NominateProjectAsync_VerifyExcludedFallbackFoldersAreRemoved()
        {
            var cps = NewCpsProject(@"{ }");
            var pri = cps.Builder
                .WithTargetFrameworkInfo(
                    new VsTargetFrameworkInfo(
                        "netcoreapp1.0",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] { new VsProjectProperty("RestoreSources", "base"),
                                new VsProjectProperty("RestoreFallbackFolders", "base"),
                                new VsProjectProperty("RestoreAdditionalProjectFallbackFolders", "x")})) // Add FF
                .WithTargetFrameworkInfo(
                    new VsTargetFrameworkInfo(
                        "netcoreapp2.0",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] { new VsProjectProperty("RestoreSources", "base"),
                                new VsProjectProperty("RestoreFallbackFolders", "base"),
                                new VsProjectProperty("RestoreAdditionalProjectSources", "a"),
                                new VsProjectProperty("RestoreAdditionalProjectFallbackFoldersExcludes", "x")})) // Remove FF
                .ProjectRestoreInfo;
            var projectFullPath = cps.ProjectFullPath;

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectFullPath, pri);
            var metadata = actualRestoreSpec.Projects.Single().RestoreMetadata;

            // Assert
            metadata.Sources.Select(e => e.Source).Should().BeEquivalentTo(new[] { "base", VSRestoreSettingsUtilities.AdditionalValue, "a" });
            metadata.FallbackFolders.Should().BeEquivalentTo(new[] { "base" });
        }

        [Fact]
        public async Task NominateProjectAsync_PRI2_VerifyExcludedFallbackFoldersAreRemoved()
        {
            var cps = NewCpsProject(@"{ }");
            var pri = cps.Builder
                .WithTargetFrameworkInfo(
                    new VsTargetFrameworkInfo2(
                        "netcoreapp1.0",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] { new VsProjectProperty("RestoreSources", "base"),
                                new VsProjectProperty("RestoreFallbackFolders", "base"),
                                new VsProjectProperty("RestoreAdditionalProjectFallbackFolders", "x")})) // Add FF
                .WithTargetFrameworkInfo(
                    new VsTargetFrameworkInfo2(
                        "netcoreapp2.0",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] { new VsProjectProperty("RestoreSources", "base"),
                                new VsProjectProperty("RestoreFallbackFolders", "base"),
                                new VsProjectProperty("RestoreAdditionalProjectSources", "a"),
                                new VsProjectProperty("RestoreAdditionalProjectFallbackFoldersExcludes", "x")})) // Remove FF
                .ProjectRestoreInfo2;
            var projectFullPath = cps.ProjectFullPath;

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectFullPath, pri);
            var metadata = actualRestoreSpec.Projects.Single().RestoreMetadata;

            // Assert
            metadata.Sources.Select(e => e.Source).Should().BeEquivalentTo(new[] { "base", VSRestoreSettingsUtilities.AdditionalValue, "a" });
            metadata.FallbackFolders.Should().BeEquivalentTo(new[] { "base" });
        }

        [Fact]
        public async Task NominateProjectAsync_VerifyToolRestoresUseAdditionalSourcesAndFallbackFolders()
        {
            var cps = NewCpsProject(@"{ }");
            var pri = cps.Builder
                .WithTool("ToolTest", "2.0.0")
                .WithTargetFrameworkInfo(
                    new VsTargetFrameworkInfo(
                        "netcoreapp1.0",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] { new VsProjectProperty("RestoreSources", "base"),
                                new VsProjectProperty("RestoreFallbackFolders", "base"),
                                new VsProjectProperty("RestoreAdditionalProjectFallbackFolders", "x;y")}))
                .WithTargetFrameworkInfo(
                    new VsTargetFrameworkInfo(
                        "netcoreapp2.0",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] { new VsProjectProperty("RestoreSources", "base"),
                                new VsProjectProperty("RestoreFallbackFolders", "base"),
                                new VsProjectProperty("RestoreAdditionalProjectSources", "a"),
                                new VsProjectProperty("RestoreAdditionalProjectFallbackFoldersExcludes", "y")}))
                .ProjectRestoreInfo;
            var projectFullPath = cps.ProjectFullPath;

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectFullPath, pri);

            // Find the tool spec
            var metadata = actualRestoreSpec.Projects.Single(e => e.RestoreMetadata.ProjectStyle == ProjectStyle.DotnetCliTool).RestoreMetadata;

            // Assert
            metadata.Sources.Select(e => e.Source).Should().BeEquivalentTo(new[] { "base", VSRestoreSettingsUtilities.AdditionalValue, "a" });
            metadata.FallbackFolders.Should().BeEquivalentTo(new[] { "base", VSRestoreSettingsUtilities.AdditionalValue, "x" });
        }

        [Fact]
        public async Task NominateProjectAsync_PRI2_VerifyToolRestoresUseAdditionalSourcesAndFallbackFolders()
        {
            var cps = NewCpsProject(@"{ }");
            var pri = cps.Builder
                .WithTool("ToolTest", "2.0.0")
                .WithTargetFrameworkInfo(
                    new VsTargetFrameworkInfo2(
                        "netcoreapp1.0",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] { new VsProjectProperty("RestoreSources", "base"),
                                new VsProjectProperty("RestoreFallbackFolders", "base"),
                                new VsProjectProperty("RestoreAdditionalProjectFallbackFolders", "x;y")}))
                .WithTargetFrameworkInfo(
                    new VsTargetFrameworkInfo2(
                        "netcoreapp2.0",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] { new VsProjectProperty("RestoreSources", "base"),
                                new VsProjectProperty("RestoreFallbackFolders", "base"),
                                new VsProjectProperty("RestoreAdditionalProjectSources", "a"),
                                new VsProjectProperty("RestoreAdditionalProjectFallbackFoldersExcludes", "y")}))
                .ProjectRestoreInfo2;
            var projectFullPath = cps.ProjectFullPath;

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectFullPath, pri);

            // Find the tool spec
            var metadata = actualRestoreSpec.Projects.Single(e => e.RestoreMetadata.ProjectStyle == ProjectStyle.DotnetCliTool).RestoreMetadata;

            // Assert
            metadata.Sources.Select(e => e.Source).Should().BeEquivalentTo(new[] { "base", VSRestoreSettingsUtilities.AdditionalValue, "a" });
            metadata.FallbackFolders.Should().BeEquivalentTo(new[] { "base", VSRestoreSettingsUtilities.AdditionalValue, "x" });
        }

        [Theory]
        [InlineData(@"..\source1", @"..\additionalsource", @"..\fallback1", @"..\additionalFallback1", false)]
        [InlineData(@"..\source1", null, @"..\fallback1", null, false)]
        [InlineData(null, @"..\additionalsource", null, @"..\additionalFallback1", false)]
        [InlineData(@"Clear;C:\source1", @"C:\additionalsource", @"C:\fallback1;Clear", @"C:\additionalFallback1", false)]
        [InlineData(@"C:\source1;Clear", @"C:\additionalsource", @"Clear;C:\fallback1", @"C:\additionalFallback1", false)]
        [InlineData(@"..\source1", @"..\additionalsource", @"..\fallback1", @"..\additionalFallback1", true)]
        [InlineData(@"..\source1", null, @"..\fallback1", null, true)]
        [InlineData(null, @"..\additionalsource", null, @"..\additionalFallback1", true)]
        [InlineData(@"Clear;C:\source1", @"C:\additionalsource", @"C:\fallback1;Clear", @"C:\additionalFallback1", true)]
        [InlineData(@"C:\source1;Clear", @"C:\additionalsource", @"Clear;C:\fallback1", @"C:\additionalFallback1", true)]
        public async Task NominateProjectAsync_WithRestoreAdditionalSourcesAndFallbackFolders(string restoreSources, string restoreAdditionalProjectSources, string restoreFallbackFolders, string restoreAdditionalFallbackFolders, bool isV2Nominate)
        {
            var vstfms = isV2Nominate ?
                (IVsTargetFrameworkInfo)
                new VsTargetFrameworkInfo2(
                        "netcoreapp2.0",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] { new VsProjectProperty("RestoreSources", restoreSources),
                                new VsProjectProperty("RestoreFallbackFolders", restoreFallbackFolders),
                                new VsProjectProperty("RestoreAdditionalProjectSources", restoreAdditionalProjectSources),
                                new VsProjectProperty("RestoreAdditionalProjectFallbackFolders", restoreAdditionalFallbackFolders)}) :
                new VsTargetFrameworkInfo(
                        "netcoreapp2.0",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] { new VsProjectProperty("RestoreSources", restoreSources),
                                new VsProjectProperty("RestoreFallbackFolders", restoreFallbackFolders),
                                new VsProjectProperty("RestoreAdditionalProjectSources", restoreAdditionalProjectSources),
                                new VsProjectProperty("RestoreAdditionalProjectFallbackFolders", restoreAdditionalFallbackFolders)});

            var cps = NewCpsProject(@"{ }");
            var builder = cps.Builder
                .WithTool("Foo.Test", "2.0.0")
                .WithTargetFrameworkInfo(vstfms);

            var projectFullPath = cps.ProjectFullPath;

            // Act
            var actualRestoreSpec = isV2Nominate ?
                await CaptureNominateResultAsync(projectFullPath, builder.ProjectRestoreInfo2) :
                await CaptureNominateResultAsync(projectFullPath, builder.ProjectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.GetProjectSpec(projectFullPath);
            Assert.NotNull(actualProjectSpec);

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
        [InlineData(@"C:\source1", @"C:\additionalsource", @"C:\source1;C:\additionalsource", @"C:\fallback1", @"C:\additionalFallback1", @"C:\fallback1;C:\additionalFallback1", false)]
        [InlineData(@"C:\source1", null, @"C:\source1", @"C:\fallback1", null, @"C:\fallback1", false)]
        [InlineData(null, @"C:\additionalsource", @"C:\additionalsource", null, @"C:\additionalFallback1", @"C:\additionalFallback1", false)]
        [InlineData(@"Clear;C:\source1", @"C:\additionalsource", @"C:\additionalsource", @"C:\fallback1;Clear", @"C:\additionalFallback1", @"C:\additionalFallback1", false)]
        [InlineData(@"C:\source1;Clear", @"C:\additionalsource", @"C:\additionalsource", @"Clear;C:\fallback1", @"C:\additionalFallback1", @"C:\additionalFallback1", false)]
        [InlineData(@"C:\source1", @"C:\additionalsource", @"C:\source1;C:\additionalsource", @"C:\fallback1", @"C:\additionalFallback1", @"C:\fallback1;C:\additionalFallback1", true)]
        [InlineData(@"C:\source1", null, @"C:\source1", @"C:\fallback1", null, @"C:\fallback1", true)]
        [InlineData(null, @"C:\additionalsource", @"C:\additionalsource", null, @"C:\additionalFallback1", @"C:\additionalFallback1", true)]
        [InlineData(@"Clear;C:\source1", @"C:\additionalsource", @"C:\additionalsource", @"C:\fallback1;Clear", @"C:\additionalFallback1", @"C:\additionalFallback1", true)]
        [InlineData(@"C:\source1;Clear", @"C:\additionalsource", @"C:\additionalsource", @"Clear;C:\fallback1", @"C:\additionalFallback1", @"C:\additionalFallback1", true)]
        public async Task VSSolutionRestoreService_VSRestoreSettingsUtilities_Integration(string restoreSources, string restoreAdditionalProjectSources, string expectedRestoreSources, string restoreFallbackFolders, string restoreAdditionalFallbackFolders, string expectedFallbackFolders, bool isV2Nominate)
        {

            var vstfms = isV2Nominate ?
                        (IVsTargetFrameworkInfo)
                        new VsTargetFrameworkInfo2(
                            "netcoreapp2.0",
                            Enumerable.Empty<IVsReferenceItem>(),
                            Enumerable.Empty<IVsReferenceItem>(),
                            Enumerable.Empty<IVsReferenceItem>(),
                            Enumerable.Empty<IVsReferenceItem>(),
                            new[] { new VsProjectProperty("RestoreSources", restoreSources),
                                    new VsProjectProperty("RestoreFallbackFolders", restoreFallbackFolders),
                                    new VsProjectProperty("RestoreAdditionalProjectSources", restoreAdditionalProjectSources),
                                    new VsProjectProperty("RestoreAdditionalProjectFallbackFolders", restoreAdditionalFallbackFolders)}) :
                        new VsTargetFrameworkInfo(
                            "netcoreapp2.0",
                            Enumerable.Empty<IVsReferenceItem>(),
                            Enumerable.Empty<IVsReferenceItem>(),
                            new[] { new VsProjectProperty("RestoreSources", restoreSources),
                                    new VsProjectProperty("RestoreFallbackFolders", restoreFallbackFolders),
                                    new VsProjectProperty("RestoreAdditionalProjectSources", restoreAdditionalProjectSources),
                                    new VsProjectProperty("RestoreAdditionalProjectFallbackFolders", restoreAdditionalFallbackFolders)});

            var cps = NewCpsProject(@"{ }");
            var builder = cps.Builder
                .WithTool("Foo.Test", "2.0.0")
                .WithTargetFrameworkInfo(vstfms);
            var projectFullPath = cps.ProjectFullPath;

            // Act
            var actualRestoreSpec = isV2Nominate ?
                await CaptureNominateResultAsync(projectFullPath, builder.ProjectRestoreInfo2) :
                await CaptureNominateResultAsync(projectFullPath, builder.ProjectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.GetProjectSpec(projectFullPath);
            Assert.NotNull(actualProjectSpec);

            var specSources = VSRestoreSettingsUtilities.GetSources(NullSettings.Instance, actualProjectSpec).Select(e => e.Source);

            var expectedSources = MSBuildStringUtility.Split(expectedRestoreSources);

            Assert.True(Enumerable.SequenceEqual(expectedSources.OrderBy(t => t), specSources.OrderBy(t => t)));

            var specFallback = VSRestoreSettingsUtilities.GetFallbackFolders(NullSettings.Instance, actualProjectSpec);

            var expectedFallback = MSBuildStringUtility.Split(expectedFallbackFolders);

            Assert.True(
                Enumerable.SequenceEqual(expectedFallback.OrderBy(t => t), specFallback.OrderBy(t => t)),
                "expected: " + string.Join(",", expectedFallback.ToArray()) + "\nactual: " + string.Join(",", specFallback.ToArray()));
        }

        [Theory]
        [InlineData("true", null, "false", false)]
        [InlineData(null, "packages.A.lock.json", null, false)]
        [InlineData("true", null, "true", false)]
        [InlineData("false", null, "false", false)]
        [InlineData("true", null, "false", true)]
        [InlineData(null, "packages.A.lock.json", null, true)]
        [InlineData("true", null, "true", true)]
        [InlineData("false", null, "false", true)]
        public async Task NominateProjectAsync_LockFileSettings(
            string restorePackagesWithLockFile,
            string lockFilePath,
            string restoreLockedMode,
            bool isV2Nominate)
        {
            var vstfms = isV2Nominate ?
                (IVsTargetFrameworkInfo)
                new VsTargetFrameworkInfo2(
                        "netcoreapp1.0",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] {
                            new VsProjectProperty("RestorePackagesWithLockFile", restorePackagesWithLockFile),
                            new VsProjectProperty("NuGetLockFilePath", lockFilePath),
                            new VsProjectProperty("RestoreLockedMode", restoreLockedMode)}) :
                new VsTargetFrameworkInfo(
                        "netcoreapp1.0",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] {
                            new VsProjectProperty("RestorePackagesWithLockFile", restorePackagesWithLockFile),
                            new VsProjectProperty("NuGetLockFilePath", lockFilePath),
                            new VsProjectProperty("RestoreLockedMode", restoreLockedMode)});
            var cps = NewCpsProject("{ }");
            var projectFullPath = cps.ProjectFullPath;
            var builder = cps.Builder
                .WithTargetFrameworkInfo(vstfms);

            // Act
            var actualRestoreSpec = isV2Nominate ?
                await CaptureNominateResultAsync(projectFullPath, builder.ProjectRestoreInfo2) :
                await CaptureNominateResultAsync(projectFullPath, builder.ProjectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.GetProjectSpec(projectFullPath);
            Assert.NotNull(actualProjectSpec);
            Assert.Equal(restorePackagesWithLockFile, actualProjectSpec.RestoreMetadata.RestoreLockProperties.RestorePackagesWithLockFile);
            Assert.Equal(lockFilePath, actualProjectSpec.RestoreMetadata.RestoreLockProperties.NuGetLockFilePath);
            Assert.Equal(MSBuildStringUtility.IsTrue(restoreLockedMode), actualProjectSpec.RestoreMetadata.RestoreLockProperties.RestoreLockedMode);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task NominateProjectAsync_RuntimeGraph(bool isV2Nominate)
        {
            var vstfms = isV2Nominate ?
                (IVsTargetFrameworkInfo)
                new VsTargetFrameworkInfo2(
                        "netcoreapp1.0",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] {
                            new VsProjectProperty("RuntimeIdentifier", "win10-x64")
                        }) :
                new VsTargetFrameworkInfo(
                        "netcoreapp1.0",
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] {
                            new VsProjectProperty("RuntimeIdentifier", "win10-x64")
                        });

            var cps = NewCpsProject("{ }");
            var projectFullPath = cps.ProjectFullPath;
            var builder = cps.Builder
                .WithTargetFrameworkInfo(vstfms);

            // Act
            var actualRestoreSpec = isV2Nominate ?
                await CaptureNominateResultAsync(projectFullPath, builder.ProjectRestoreInfo2) :
                await CaptureNominateResultAsync(projectFullPath, builder.ProjectRestoreInfo);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.GetProjectSpec(projectFullPath);
            Assert.NotNull(actualProjectSpec);
            Assert.Equal("win10-x64", actualProjectSpec.RuntimeGraph.Runtimes.Single().Key);
        }

        [Fact]
        public async Task NominateProjectAsync_BasicPackageDownload()
        {
            var consoleAppProjectJson = @"{
    ""frameworks"": {
        ""netcoreapp3.0"": {
            ""dependencies"": {
                ""NuGet.Protocol"": {
                    ""target"": ""Package"",
                    ""version"": ""5.1.0""
                },
            },
            ""downloadDependencies"": [
                {""name"" : ""NetCoreTargetingPack"", ""version"" : ""[1.0.0]""},
                {""name"" : ""NetCoreRuntimePack"", ""version"" : ""[1.0.0]""},
                {""name"" : ""NetCoreApphostPack"", ""version"" : ""[1.0.0]""},
            ]
            }
        }
    }";
            var projectName = "ConsoleApp1";
            var cps = NewCpsProject(consoleAppProjectJson, projectName);
            var projectFullPath = cps.ProjectFullPath;
            var expectedBaseIntermediate = cps.ProjectRestoreInfo2.BaseIntermediatePath;

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectFullPath, cps.ProjectRestoreInfo2);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.GetProjectSpec(projectFullPath);
            Assert.NotNull(actualProjectSpec);
            Assert.Equal("1.0.0", actualProjectSpec.Version.ToString());

            var actualMetadata = actualProjectSpec.RestoreMetadata;
            Assert.NotNull(actualMetadata);
            Assert.Equal(projectFullPath, actualMetadata.ProjectPath);
            Assert.Equal(projectName, actualMetadata.ProjectName);
            Assert.Equal(ProjectStyle.PackageReference, actualMetadata.ProjectStyle);
            Assert.Equal(expectedBaseIntermediate, actualMetadata.OutputPath);

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

        [Fact]
        public async Task NominateProjectAsync_PackageDownload_IgnoreDuplicateIds()
        {
            var consoleAppProjectJson = @"{
    ""frameworks"": {
        ""netcoreapp3.0"": {
            ""dependencies"": {
                ""NuGet.Protocol"": {
                    ""target"": ""Package"",
                    ""version"": ""5.1.0""
                },
            },
            ""downloadDependencies"": [
                {""name"" : ""NetCoreTargetingPack"", ""version"" : ""[1.0.0]""},
                {""name"" : ""NetCoreTargetingPack"", ""version"" : ""[2.0.0]""},
            ]
            }
        }
    }";
            var projectName = "ConsoleApp1";
            var cps = NewCpsProject(consoleAppProjectJson, projectName);
            var projectFullPath = cps.ProjectFullPath;
            var expectedBaseIntermediate = cps.ProjectRestoreInfo2.BaseIntermediatePath;

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectFullPath, cps.ProjectRestoreInfo2);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.GetProjectSpec(projectFullPath);
            Assert.NotNull(actualProjectSpec);
            Assert.Equal("1.0.0", actualProjectSpec.Version.ToString());

            var actualMetadata = actualProjectSpec.RestoreMetadata;
            Assert.NotNull(actualMetadata);
            Assert.Equal(projectFullPath, actualMetadata.ProjectPath);
            Assert.Equal(projectName, actualMetadata.ProjectName);
            Assert.Equal(ProjectStyle.PackageReference, actualMetadata.ProjectStyle);
            Assert.Equal(expectedBaseIntermediate, actualMetadata.OutputPath);

            Assert.Single(actualProjectSpec.TargetFrameworks);
            var actualTfi = actualProjectSpec.TargetFrameworks.Single();

            var expectedFramework = NuGetFramework.Parse("netcoreapp3.0");
            Assert.Equal(expectedFramework, actualTfi.FrameworkName);

            AssertPackages(actualTfi,
                "NuGet.Protocol:5.1.0");
            AssertDownloadPackages(actualTfi,
                "NetCoreTargetingPack:[1.0.0]");
        }

        [Theory]
        [InlineData("[1.0.0];[2.0.0]")]
        [InlineData(";[1.0.0];;[2.0.0];")]
        public async Task NominateProjectAsync_PackageDownload_AllowsVersionList(string versionString)
        {
            var consoleAppProjectJson = @"{
    ""frameworks"": {
        ""netcoreapp3.0"": {
            ""dependencies"": {
                ""NuGet.Protocol"": {
                    ""target"": ""Package"",
                    ""version"": ""5.1.0""
                },
            },
            ""downloadDependencies"": [
                {""name"" : ""NetCoreTargetingPack"", ""version"" : """ + versionString + @"""}
            ]
            }
        }
    }";
            var projectName = "ConsoleApp1";
            var cps = NewCpsProject(consoleAppProjectJson, projectName);
            var projectFullPath = cps.ProjectFullPath;
            var expectedBaseIntermediate = cps.ProjectRestoreInfo2.BaseIntermediatePath;

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectFullPath, cps.ProjectRestoreInfo2);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.GetProjectSpec(projectFullPath);
            Assert.NotNull(actualProjectSpec);
            Assert.Equal("1.0.0", actualProjectSpec.Version.ToString());

            var actualMetadata = actualProjectSpec.RestoreMetadata;
            Assert.NotNull(actualMetadata);
            Assert.Equal(projectFullPath, actualMetadata.ProjectPath);
            Assert.Equal(projectName, actualMetadata.ProjectName);
            Assert.Equal(ProjectStyle.PackageReference, actualMetadata.ProjectStyle);
            Assert.Equal(expectedBaseIntermediate, actualMetadata.OutputPath);

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
            var consoleAppProjectJson = @"{
    ""frameworks"": {
        ""netcoreapp3.0"": {
            ""dependencies"": {
                ""NuGet.Protocol"": {
                    ""target"": ""Package"",
                    ""version"": ""5.1.0""
                },
            },
            ""downloadDependencies"": [
                {""name"" : ""NetCoreTargetingPack"", ""version"" : ""[1.0.0, )""},
            ]
            }
        }
    }";
            var projectName = "ConsoleApp1";
            var cps = NewCpsProject(consoleAppProjectJson, projectName);
            var projectFullPath = cps.ProjectFullPath;
            var cache = Mock.Of<IProjectSystemCache>();
            var restoreWorker = Mock.Of<ISolutionRestoreWorker>();
            var vsSolution2 = Mock.Of<IVsSolution2>();
            var asyncLazySolution2 = new Microsoft.VisualStudio.Threading.AsyncLazy<IVsSolution2>(() => Task.FromResult(vsSolution2));
            var telemetryProvider = Mock.Of<INuGetTelemetryProvider>();

            var service = new VsSolutionRestoreService(
                cache, restoreWorker, NullLogger.Instance, asyncLazySolution2, telemetryProvider);

            var result = await service.NominateProjectAsync(projectFullPath, cps.ProjectRestoreInfo2, CancellationToken.None);

            Assert.False(result, "Project restore nomination must fail.");
        }

        [Fact]
        public async Task NominateProjectAsync_BasicFrameworkReferences()
        {
            var consoleAppProjectJson = @"{
    ""frameworks"": {
        ""netcoreapp3.0"": {
            ""dependencies"": {
                ""NuGet.Protocol"": {
                    ""target"": ""Package"",
                    ""version"": ""5.1.0""
                },
            },
            ""frameworkReferences"": {
                ""Microsoft.WindowsDesktop.App|WPF"" : {
                    ""privateAssets"" : ""none""
                },
                ""Microsoft.WindowsDesktop.App|WinForms"" : {
                    ""privateAssets"" : ""all""
                }
            }
            }
        }
    }";
            var projectName = "ConsoleApp1";
            var cps = NewCpsProject(consoleAppProjectJson, projectName);
            var projectFullPath = cps.ProjectFullPath;
            var expectedBaseIntermediate = cps.ProjectRestoreInfo2.BaseIntermediatePath;

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectFullPath, cps.ProjectRestoreInfo2);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.GetProjectSpec(projectFullPath);
            Assert.NotNull(actualProjectSpec);
            Assert.Equal("1.0.0", actualProjectSpec.Version.ToString());

            var actualMetadata = actualProjectSpec.RestoreMetadata;
            Assert.NotNull(actualMetadata);
            Assert.Equal(projectFullPath, actualMetadata.ProjectPath);
            Assert.Equal(projectName, actualMetadata.ProjectName);
            Assert.Equal(ProjectStyle.PackageReference, actualMetadata.ProjectStyle);
            Assert.Equal(expectedBaseIntermediate, actualMetadata.OutputPath);

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
            var consoleAppProjectJson = @"{
    ""frameworks"": {
        ""netcoreapp3.0"": {
            ""dependencies"": {
                ""NuGet.Protocol"": {
                    ""target"": ""Package"",
                    ""version"": ""5.1.0""
                },
            },
            ""frameworkReferences"": {
                ""Microsoft.WindowsDesktop.App|WinForms"" : {
                    ""privateAssets"" : ""none""
                },
                ""Microsoft.WindowsDesktop.App|WINFORMS"" : {
                    ""privateAssets"" : ""none""
                }
            }
            }
        }
    }";
            var projectName = "ConsoleApp1";
            var cps = NewCpsProject(consoleAppProjectJson, projectName);
            var projectFullPath = cps.ProjectFullPath;
            var expectedBaseIntermediate = cps.ProjectRestoreInfo2.BaseIntermediatePath;

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectFullPath, cps.ProjectRestoreInfo2);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.GetProjectSpec(projectFullPath);
            Assert.NotNull(actualProjectSpec);
            Assert.Equal("1.0.0", actualProjectSpec.Version.ToString());

            var actualMetadata = actualProjectSpec.RestoreMetadata;
            Assert.NotNull(actualMetadata);
            Assert.Equal(projectFullPath, actualMetadata.ProjectPath);
            Assert.Equal(projectName, actualMetadata.ProjectName);
            Assert.Equal(ProjectStyle.PackageReference, actualMetadata.ProjectStyle);
            Assert.Equal(expectedBaseIntermediate, actualMetadata.OutputPath);

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
            var consoleAppProjectJson = @"{
    ""frameworks"": {
        ""netcoreapp3.0"": {
            ""frameworkReferences"": {
                ""Microsoft.WindowsDesktop.App|WPF"" : {
                    ""privateAssets"" : ""none""
                },
                ""Microsoft.WindowsDesktop.App|WinForms"" : {
                    ""privateAssets"" : ""none""
                }
            }
        },
         ""netcoreapp3.1"": {
            ""frameworkReferences"": {
                ""Microsoft.ASPNetCore.App"" : {
                    ""privateAssets"" : ""none""
                }
            }
            }
        }
        },
    }";
            var projectName = "ConsoleApp1";
            var cps = NewCpsProject(consoleAppProjectJson, projectName);
            var projectFullPath = cps.ProjectFullPath;
            var expectedBaseIntermediate = cps.ProjectRestoreInfo2.BaseIntermediatePath;

            // Act
            var actualRestoreSpec = await CaptureNominateResultAsync(projectFullPath, cps.ProjectRestoreInfo2);

            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.GetProjectSpec(projectFullPath);
            Assert.NotNull(actualProjectSpec);
            Assert.Equal("1.0.0", actualProjectSpec.Version.ToString());

            var actualMetadata = actualProjectSpec.RestoreMetadata;
            Assert.NotNull(actualMetadata);
            Assert.Equal(projectFullPath, actualMetadata.ProjectPath);
            Assert.Equal(projectName, actualMetadata.ProjectName);
            Assert.Equal(ProjectStyle.PackageReference, actualMetadata.ProjectStyle);
            Assert.Equal(expectedBaseIntermediate, actualMetadata.OutputPath);

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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task NominateProjectAsync_WarningProperties(bool isV2Nominate)
        {
            var packageReference = new VsReferenceItem("NuGet.Protocol", new VsReferenceProperties() { new VsReferenceProperty("NoWarn", "NU1605") });

            var vstfms = isV2Nominate ?
                (IVsTargetFrameworkInfo)
                new VsTargetFrameworkInfo2(
                        "netcoreapp1.0",
                        new IVsReferenceItem[] { packageReference },
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] {
                            new VsProjectProperty("TreatWarningsAsErrors", "true"),
                            new VsProjectProperty("WarningsAsErrors", "NU1603;NU1604"),
                            new VsProjectProperty("WarningsNotAsErrors", "NU1801;NU1802")
                        }) :
                new VsTargetFrameworkInfo(
                        "netcoreapp1.0",
                        new IVsReferenceItem[] { packageReference },
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] {
                            new VsProjectProperty("TreatWarningsAsErrors", "true"),
                            new VsProjectProperty("WarningsAsErrors", "NU1603;NU1604"),
                            new VsProjectProperty("WarningsNotAsErrors", "NU1801;NU1802")
                        });

            var cps = NewCpsProject("{ }");
            var projectFullPath = cps.ProjectFullPath;
            var builder = cps.Builder
                .WithTargetFrameworkInfo(vstfms);

            // Act
            var actualRestoreSpec = isV2Nominate ?
                    await CaptureNominateResultAsync(projectFullPath, builder.ProjectRestoreInfo2) :
                    await CaptureNominateResultAsync(projectFullPath, builder.ProjectRestoreInfo);
            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.GetProjectSpec(projectFullPath);
            Assert.NotNull(actualProjectSpec);
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task NominateProjectAsync_RuntimeIdentifierGraphPath(bool isV2Nominate)
        {
            var runtimeGraphPath = @"C:\Program Files\dotnet\sdk\3.0.100\runtime.json";

            var packageReference = new VsReferenceItem("NuGet.Protocol", new VsReferenceProperties() { });

            var vstfms = isV2Nominate ?
                (IVsTargetFrameworkInfo)
                new VsTargetFrameworkInfo2(
                        "netcoreapp1.0",
                        new IVsReferenceItem[] { packageReference },
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] {
                            new VsProjectProperty(ProjectBuildProperties.RuntimeIdentifierGraphPath, runtimeGraphPath)
                        }) :
                new VsTargetFrameworkInfo(
                        "netcoreapp1.0",
                        new IVsReferenceItem[] { packageReference },
                        Enumerable.Empty<IVsReferenceItem>(),
                        new[] {
                            new VsProjectProperty(ProjectBuildProperties.RuntimeIdentifierGraphPath, runtimeGraphPath)
                        });

            var cps = NewCpsProject("{ }");
            var projectFullPath = cps.ProjectFullPath;
            var builder = cps.Builder
                .WithTargetFrameworkInfo(vstfms);

            // Act
            var actualRestoreSpec = isV2Nominate ?
                    await CaptureNominateResultAsync(projectFullPath, builder.ProjectRestoreInfo2) :
                    await CaptureNominateResultAsync(projectFullPath, builder.ProjectRestoreInfo);
            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.GetProjectSpec(projectFullPath);
            Assert.NotNull(actualProjectSpec);
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
            _ = await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.NominateProjectAsync(@"F:\project\project.csproj", (IVsProjectRestoreInfo)null, CancellationToken.None));

            _ = await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.NominateProjectAsync(@"F:\project\project.csproj", (IVsProjectRestoreInfo2)null, CancellationToken.None));
        }

        [Fact]
        public async Task NominateProjectAsync_InvalidTargetFrameworkMoniker_Succeeds()
        {
            // Arrange
            const string projectFullPath = @"f:\project\project.csproj";
            var restoreWorker = CreateDefaultISolutionRestoreWorkerMock();

            var emptyReferenceItems = Array.Empty<VsReferenceItem>();
            var projectRestoreInfo = new VsProjectRestoreInfo2(@"f:\project\",
                new VsTargetFrameworks2(new[]
                {
                    new VsTargetFrameworkInfo2(
                        targetFrameworkMoniker: "_,Version=2.0",
                        packageReferences: emptyReferenceItems,
                        projectReferences: emptyReferenceItems,
                        packageDownloads: emptyReferenceItems,
                        frameworkReferences: emptyReferenceItems,
                        projectProperties: Array.Empty<IVsProjectProperty>())
                }));

            // Act
            var additionalMessages = await CaptureAdditionalMessagesAsync(projectFullPath, projectRestoreInfo, restoreWorker: restoreWorker);

            // Assert
            var additionalMessage = Assert.Single(additionalMessages);
            Assert.Equal(NuGetLogCode.NU1105, additionalMessage.Code);
            restoreWorker.Verify(rw => rw.ScheduleRestoreAsync(It.IsAny<SolutionRestoreRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task NominateProjectAsync_InvalidDependencyVersion_Succeeds()
        {
            // Arrange
            const string projectFullPath = @"f:\project\project.csproj";

            var restoreWorker = CreateDefaultISolutionRestoreWorkerMock();

            var emptyReferenceItems = Array.Empty<VsReferenceItem>();
            var projectRestoreInfo = new VsProjectRestoreInfo2(@"f:\project\",
                new VsTargetFrameworks2(new[]
                {
                    new VsTargetFrameworkInfo2(
                        targetFrameworkMoniker: FrameworkConstants.CommonFrameworks.NetStandard20.ToString(),
                        packageReferences: new[]
                        {
                            new VsReferenceItem("packageId", new VsReferenceProperties(new []
                            {
                                new VsReferenceProperty("Version", "foo")
                            }))
                        },
                        projectReferences: emptyReferenceItems,
                        packageDownloads: emptyReferenceItems,
                        frameworkReferences: emptyReferenceItems,
                        projectProperties: Array.Empty<IVsProjectProperty>())
                }));

            // Act
            var additionalMessages = await CaptureAdditionalMessagesAsync(projectFullPath, projectRestoreInfo, restoreWorker);

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

            var emptyReferenceItems = Array.Empty<VsReferenceItem>();
            var projectRestoreInfo = new VsProjectRestoreInfo2(@"f:\project\",
                new VsTargetFrameworks2(new[]
                {
                    new VsTargetFrameworkInfo2(
                        targetFrameworkMoniker: FrameworkConstants.CommonFrameworks.NetStandard20.ToString(),
                        packageReferences: emptyReferenceItems,
                        projectReferences: emptyReferenceItems,
                        packageDownloads: emptyReferenceItems,
                        frameworkReferences: emptyReferenceItems,
                        projectProperties: Array.Empty<IVsProjectProperty>())
                }));

            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await NominateProjectAsync(@"f:\project\project.csproj", projectRestoreInfo, CancellationToken.None, restoreWorker: restoreWorker));
        }

        [Fact]
        public void ToPackageSpec_CentralVersions_AreAddedToThePackageSpecIfCPVMIsEnabled()
        {
            // Arrange
            ProjectNames projectName = new ProjectNames(@"f:\project\project.csproj", "project", "project.csproj", "projectC", Guid.NewGuid().ToString());
            var emptyReferenceItems = Array.Empty<VsReferenceItem>();

            var targetFrameworks = new VsTargetFrameworkInfo3[] { new VsTargetFrameworkInfo3(
                targetFrameworkMoniker: CommonFrameworks.NetStandard20.ToString(),
                packageReferences: new[] { new VsReferenceItem("foo", new VsReferenceProperties()) },
                projectReferences: emptyReferenceItems,
                packageDownloads: emptyReferenceItems,
                frameworkReferences: emptyReferenceItems,
                projectProperties: ProjectRestoreInfoBuilder.GetTargetFrameworkProperties(CommonFrameworks.NetStandard20).Concat(new VsProjectProperty[] { new VsProjectProperty("ManagePackageVersionsCentrally", "true") }),
                centralPackageVersions: new[]
                        {
                            new VsReferenceItem("foo", new VsReferenceProperties(new []
                            {
                                new VsReferenceProperty("Version", "2.0.0")
                            })),
                            // the second centralPackageVersion with the same version name will be ignored
                            new VsReferenceItem("foo", new VsReferenceProperties(new []
                            {
                                new VsReferenceProperty("Version", "3.0.0")
                            }))
                        })
            };

            // Act
            var result = VsSolutionRestoreService.ToPackageSpec(projectName, targetFrameworks, CommonFrameworks.NetStandard20.ToString(), string.Empty);

            // Assert
            var tfm = result.TargetFrameworks.First();

            var packageVersion = Assert.Single(tfm.CentralPackageVersions);
            Assert.Equal("foo", packageVersion.Key);
            Assert.Equal("[2.0.0, )", packageVersion.Value.VersionRange.ToNormalizedString());
            var packageReference = Assert.Single(tfm.Dependencies);
            Assert.Equal("[2.0.0, )", packageReference.LibraryRange.VersionRange.ToNormalizedString());
            Assert.True(result.RestoreMetadata.CentralPackageVersionsEnabled);
        }

        [Fact]
        public void ToPackageSpec_CentralVersions_CPVMIsEnabled_NoPackageVersions()
        {
            // Arrange
            ProjectNames projectName = new ProjectNames(@"f:\project\project.csproj", "project", "project.csproj", "projectC", Guid.NewGuid().ToString());
            var emptyReferenceItems = Array.Empty<VsReferenceItem>();

            var targetFrameworks = new VsTargetFrameworkInfo3[] { new VsTargetFrameworkInfo3(
                targetFrameworkMoniker: CommonFrameworks.NetStandard20.ToString(),
                packageReferences: new[] { new VsReferenceItem("foo", new VsReferenceProperties()) },
                projectReferences: emptyReferenceItems,
                packageDownloads: emptyReferenceItems,
                frameworkReferences: emptyReferenceItems,
                projectProperties: ProjectRestoreInfoBuilder.GetTargetFrameworkProperties(CommonFrameworks.NetStandard20).Concat(new VsProjectProperty[] { new VsProjectProperty("ManagePackageVersionsCentrally", "true") }),
                centralPackageVersions: Enumerable.Empty<IVsReferenceItem>())
            };

            // Act
            var result = VsSolutionRestoreService.ToPackageSpec(projectName, targetFrameworks, CommonFrameworks.NetStandard20.ToString(), string.Empty);

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
            var emptyReferenceItems = Array.Empty<VsReferenceItem>();
            var packageReferenceProperties = packRefVersion == null ?
                new VsReferenceProperties() :
                new VsReferenceProperties(new[] { new VsReferenceProperty("Version", packRefVersion) });
            var projectProperties = managePackageVersionsCentrally == null ?
                new VsProjectProperty[0] :
                new VsProjectProperty[] { new VsProjectProperty("ManagePackageVersionsCentrally", managePackageVersionsCentrally) };

            var targetFrameworks = new VsTargetFrameworkInfo3[] { new VsTargetFrameworkInfo3(
                targetFrameworkMoniker: CommonFrameworks.NetStandard20.ToString(),
                packageReferences: new[] { new VsReferenceItem("foo", packageReferenceProperties) },
                projectReferences: emptyReferenceItems,
                packageDownloads: emptyReferenceItems,
                frameworkReferences: emptyReferenceItems,
                projectProperties: ProjectRestoreInfoBuilder.GetTargetFrameworkProperties(CommonFrameworks.NetStandard20).Concat(projectProperties),
                centralPackageVersions: new[]
                        {
                            new VsReferenceItem("foo", new VsReferenceProperties(new []
                            {
                                new VsReferenceProperty("Version", "2.0.0")
                            }))
                        })
            };

            // Act
            var result = VsSolutionRestoreService.ToPackageSpec(projectName, targetFrameworks, CommonFrameworks.NetStandard20.ToString(), string.Empty);

            // Assert
            var tfm = result.TargetFrameworks.First();
            var expectedPackageReferenceVersion = packRefVersion == null ? "(, )" : "[1.0.0, )";
            Assert.Equal(0, tfm.CentralPackageVersions.Count);
            Assert.Equal(1, tfm.Dependencies.Count);
            Assert.Equal(expectedPackageReferenceVersion, tfm.Dependencies.First().LibraryRange.VersionRange.ToNormalizedString());
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
            var emptyReferenceItems = Array.Empty<VsReferenceItem>();

            var targetFrameworks = new VsTargetFrameworkInfo3[] { new VsTargetFrameworkInfo3(
                targetFrameworkMoniker: CommonFrameworks.NetStandard20.ToString(),
                packageReferences: new[] { new VsReferenceItem("foo", new VsReferenceProperties()) },
                projectReferences: emptyReferenceItems,
                packageDownloads: emptyReferenceItems,
                frameworkReferences: emptyReferenceItems,
                projectProperties: ProjectRestoreInfoBuilder.GetTargetFrameworkProperties(CommonFrameworks.NetStandard20).Concat(new VsProjectProperty[]
                {
                    new VsProjectProperty(ProjectBuildProperties.ManagePackageVersionsCentrally, "true"),
                    new VsProjectProperty(ProjectBuildProperties.CentralPackageVersionOverrideEnabled, isCentralPackageVersionOverrideEnabled)
                }),
                centralPackageVersions: new[]
                        {
                            new VsReferenceItem("foo", new VsReferenceProperties(new []
                            {
                                new VsReferenceProperty("Version", "2.0.0")
                            })),
                            // the second centralPackageVersion with the same version name will be ignored
                            new VsReferenceItem("foo", new VsReferenceProperties(new []
                            {
                                new VsReferenceProperty("Version", "3.0.0")
                            }))
                        })
            };

            // Act
            PackageSpec result = VsSolutionRestoreService.ToPackageSpec(projectName, targetFrameworks, CommonFrameworks.NetStandard20.ToString(), string.Empty);

            // Assert
            TargetFrameworkInformation tfm = result.TargetFrameworks.First();

            KeyValuePair<string, CentralPackageVersion> packageVersion = Assert.Single(tfm.CentralPackageVersions);
            Assert.Equal("foo", packageVersion.Key);
            Assert.Equal("[2.0.0, )", packageVersion.Value.VersionRange.ToNormalizedString());
            LibraryDependency packageReference = Assert.Single(tfm.Dependencies);
            Assert.Equal("[2.0.0, )", packageReference.LibraryRange.VersionRange.ToNormalizedString());

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
            var emptyReferenceItems = Array.Empty<VsReferenceItem>();

            var targetFrameworks = new VsTargetFrameworkInfo3[] { new VsTargetFrameworkInfo3(
                targetFrameworkMoniker: CommonFrameworks.NetStandard20.ToString(),
                packageReferences: new[] { new VsReferenceItem("foo", new VsReferenceProperties()) },
                projectReferences: emptyReferenceItems,
                packageDownloads: emptyReferenceItems,
                frameworkReferences: emptyReferenceItems,
                projectProperties: ProjectRestoreInfoBuilder.GetTargetFrameworkProperties(CommonFrameworks.NetStandard20).Concat(new VsProjectProperty[]
                {
                    new VsProjectProperty(ProjectBuildProperties.ManagePackageVersionsCentrally, "true"),
                    new VsProjectProperty(ProjectBuildProperties.CentralPackageTransitivePinningEnabled, CentralPackageTransitivePinningEnabled),
                }),
                centralPackageVersions: Array.Empty<IVsReferenceItem>())
            };

            // Act
            PackageSpec result = VsSolutionRestoreService.ToPackageSpec(projectName, targetFrameworks, CommonFrameworks.NetStandard20.ToString(), string.Empty);

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
            var emptyReferenceItems = Array.Empty<VsReferenceItem>();

            var targetFrameworks = new VsTargetFrameworkInfo3[] { new VsTargetFrameworkInfo3(
                targetFrameworkMoniker: CommonFrameworks.NetStandard20.ToString(),
                packageReferences: new[] { new VsReferenceItem("foo", new VsReferenceProperties()) },
                projectReferences: emptyReferenceItems,
                packageDownloads: emptyReferenceItems,
                frameworkReferences: emptyReferenceItems,
                projectProperties: ProjectRestoreInfoBuilder.GetTargetFrameworkProperties(CommonFrameworks.NetStandard20).Concat(new VsProjectProperty[]
                {
                    new VsProjectProperty(ProjectBuildProperties.ManagePackageVersionsCentrally, "true"),
                    new VsProjectProperty(ProjectBuildProperties.CentralPackageFloatingVersionsEnabled, centralPackageFloatingVersionsEnabled),
                }),
                centralPackageVersions: Array.Empty<IVsReferenceItem>())
            };

            // Act
            PackageSpec result = VsSolutionRestoreService.ToPackageSpec(projectName, targetFrameworks, CommonFrameworks.NetStandard20.ToString(), string.Empty);

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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task NominateProjectAsync_PackageReferenceWithAliases(bool isV2Nominate)
        {
            var properties = new VsReferenceProperties();
            properties.Add("Aliases", "Core");
            var packageReference = new VsReferenceItem("NuGet.Protocol", properties);

            var vstfms = isV2Nominate ?
                (IVsTargetFrameworkInfo)
                new VsTargetFrameworkInfo2(
                        "netcoreapp1.0",
                        new IVsReferenceItem[] { packageReference },
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsProjectProperty>()
                        ) :
                new VsTargetFrameworkInfo(
                        "netcoreapp1.0",
                        new IVsReferenceItem[] { packageReference },
                        Enumerable.Empty<IVsReferenceItem>(),
                        Enumerable.Empty<IVsProjectProperty>()
                        );

            var cps = NewCpsProject("{ }");
            var projectFullPath = cps.ProjectFullPath;
            var builder = cps.Builder
                .WithTargetFrameworkInfo(vstfms);

            // Act
            var actualRestoreSpec = isV2Nominate ?
                    await CaptureNominateResultAsync(projectFullPath, builder.ProjectRestoreInfo2) :
                    await CaptureNominateResultAsync(projectFullPath, builder.ProjectRestoreInfo);
            // Assert
            SpecValidationUtility.ValidateDependencySpec(actualRestoreSpec);

            var actualProjectSpec = actualRestoreSpec.GetProjectSpec(projectFullPath);
            Assert.NotNull(actualProjectSpec);
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
            var emptyReferenceItems = Array.Empty<VsReferenceItem>();
            var packageReferenceProperties = new VsReferenceProperties();

            var targetFrameworks = new VsTargetFrameworkInfo2[]
            {
                new VsTargetFrameworkInfo2(
                    targetFrameworkMoniker: "tfm1",
                    packageReferences: emptyReferenceItems,
                    projectReferences: emptyReferenceItems,
                    packageDownloads: emptyReferenceItems,
                    frameworkReferences: emptyReferenceItems,
                    projectProperties: ProjectRestoreInfoBuilder.GetTargetFrameworkProperties(CommonFrameworks.NetStandard20, "tfm1"),
                    addTargetFrameworkProperties: false),
                new VsTargetFrameworkInfo2(
                    targetFrameworkMoniker: "tfm1",
                    packageReferences: emptyReferenceItems,
                    projectReferences: emptyReferenceItems,
                    packageDownloads: emptyReferenceItems,
                    frameworkReferences: emptyReferenceItems,
                    projectProperties: ProjectRestoreInfoBuilder.GetTargetFrameworkProperties(CommonFrameworks.NetStandard21, "tfm2"),
                    addTargetFrameworkProperties: false)
            };
            var originalTargetFrameworksString = string.Join(";", targetFrameworks.Select(tf => tf.TargetFrameworkMoniker));

            // Act
            var result = VsSolutionRestoreService.ToPackageSpec(projectName, targetFrameworks, originalTargetFrameworksString, string.Empty);

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
            var emptyReferenceItems = Array.Empty<VsReferenceItem>();
            var packageReferenceProperties = new VsReferenceProperties();
            var managedFramework = NuGetFramework.Parse("net5.0-windows10.0");
            var nativeFramework = CommonFrameworks.Native;
            var targetFrameworks = new VsTargetFrameworkInfo2[]
            {
                new VsTargetFrameworkInfo2(
                    targetFrameworkMoniker: "tfm1",
                    packageReferences: emptyReferenceItems,
                    projectReferences: emptyReferenceItems,
                    packageDownloads: emptyReferenceItems,
                    frameworkReferences: emptyReferenceItems,
                    projectProperties: ProjectRestoreInfoBuilder.GetTargetFrameworkProperties(managedFramework, "tfm1", clrSupport),
                    addTargetFrameworkProperties: false),
            };
            var originalTargetFrameworksString = string.Join(";", targetFrameworks.Select(tf => tf.TargetFrameworkMoniker));

            // Act
            var result = VsSolutionRestoreService.ToPackageSpec(projectName, targetFrameworks, originalTargetFrameworksString, string.Empty);

            // Assert
            Assert.Equal(1, result.TargetFrameworks.Count);
            TargetFrameworkInformation targetFrameworkInfo = Assert.Single(result.TargetFrameworks, tf => tf.TargetAlias == "tfm1");

            if (isDualCompatibilityFramework)
            {
                var comparer = NuGetFrameworkFullComparer.Instance;
                comparer.Equals(targetFrameworkInfo.FrameworkName, managedFramework).Should().BeTrue();
                targetFrameworkInfo.FrameworkName.Should().BeOfType<DualCompatibilityFramework>();
                var dualCompatibilityFramework = targetFrameworkInfo.FrameworkName as DualCompatibilityFramework;
                dualCompatibilityFramework.RootFramework.Should().Be(managedFramework);
                dualCompatibilityFramework.SecondaryFramework.Should().Be(CommonFrameworks.Native);
            }
            else
            {
                targetFrameworkInfo.FrameworkName.Should().NotBeOfType<DualCompatibilityFramework>();
                targetFrameworkInfo.FrameworkName.Should().Be(nativeFramework);
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
            var emptyReferenceItems = Array.Empty<VsReferenceItem>();
            var projectReferenceProperties = new VsReferenceProperties();
            VsReferenceItem[] projectReferences = new[]
            {
                new VsReferenceItem(referencedProjectPath, projectReferenceProperties),
                new VsReferenceItem(relativePath, projectReferenceProperties)
            };
            var targetFrameworks = new VsTargetFrameworkInfo2[]
            {
                new VsTargetFrameworkInfo2("net5.0",
                packageReferences: emptyReferenceItems,
                projectReferences: projectReferences,
                packageDownloads: emptyReferenceItems,
                frameworkReferences: emptyReferenceItems,
                projectProperties: Array.Empty<IVsProjectProperty>())
            };

            // Act
            var actual = VsSolutionRestoreService.ToPackageSpec(projectName, targetFrameworks, originalTargetFrameworkstr: string.Empty, msbuildProjectExtensionsPath: string.Empty);

            // Assert
            ProjectRestoreMetadataFrameworkInfo targetFramework = Assert.Single(actual.RestoreMetadata.TargetFrameworks);
            var projectReference = Assert.Single(targetFramework.ProjectReferences);
            Assert.Equal(referencedProjectPath, projectReference.ProjectUniqueName);
        }

        private delegate void TryGetProjectNamesCallback(string projectPath, out ProjectNames projectNames);
        private delegate bool TryGetProjectNamesReturns(string projectPath, out ProjectNames projectNames);

        [Fact]
        public void ToPackageSpec_PackageDownloadWithNoVersion_ThrowsException()
        {
            // Arrange
            string packageName = "package";
            ProjectNames projectName = new ProjectNames(@"n:\path\to\current\project.csproj", "project", "project.csproj", "project", Guid.NewGuid().ToString());
            var emptyReferenceItems = Array.Empty<VsReferenceItem>();
            var targetFrameworks = new VsTargetFrameworkInfo2[]
            {
                new VsTargetFrameworkInfo2(
                    targetFrameworkMoniker: "net5.0",
                    packageReferences: emptyReferenceItems,
                    projectReferences: emptyReferenceItems,
                    packageDownloads: new List<IVsReferenceItem>
                    {
                        new VsReferenceItem(
                            packageName,
                            new VsReferenceProperties(new [] { new VsReferenceProperty("Version", null) }))
                    },
                    frameworkReferences: emptyReferenceItems,
                    projectProperties: Array.Empty<IVsProjectProperty>())
            };

            string expected = string.Format(CultureInfo.CurrentCulture, Resources.Error_PackageDownload_NoVersion, packageName);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(() => VsSolutionRestoreService.ToPackageSpec(projectName, targetFrameworks, originalTargetFrameworkstr: string.Empty, msbuildProjectExtensionsPath: string.Empty));
            Assert.Equal(expected, exception.Message);
        }

        private async Task<DependencyGraphSpec> CaptureNominateResultAsync(
            string projectFullPath,
            IVsProjectRestoreInfo pri,
            Mock<IProjectSystemCache> cache = null)
        {
            DependencyGraphSpec capturedRestoreSpec = null;

            if (cache == null)
            {
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
            var result = await NominateProjectAsync(projectFullPath, pri, CancellationToken.None, cache: cache);

            Assert.True(result, "Project restore nomination should succeed.");

            return capturedRestoreSpec;
        }

        private async Task<IReadOnlyList<IAssetsLogMessage>> CaptureAdditionalMessagesAsync(
            string projectFullPath,
            IVsProjectRestoreInfo2 pri,
            Mock<ISolutionRestoreWorker> restoreWorker = null)
        {
            IReadOnlyList<IAssetsLogMessage> additionalMessages = null;

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
            var result = await NominateProjectAsync(projectFullPath, pri, CancellationToken.None, cache: cache, restoreWorker, logger);

            Assert.True(result, "Project restore nomination should succeed.");
            logger.Verify(l => l.LogError(It.IsAny<string>()), Times.Never);

            return additionalMessages;
        }

        private async Task<DependencyGraphSpec> CaptureNominateResultAsync(
            string projectFullPath,
            IVsProjectRestoreInfo2 pri,
            Mock<IProjectSystemCache> cache = null)
        {
            DependencyGraphSpec capturedRestoreSpec = null;

            if (cache == null)
            {
                cache = CreateDefaultIProjectSystemCacheMock(projectFullPath);
            }

            cache
                .Setup(x => x.AddProjectRestoreInfo(
                    It.IsAny<ProjectNames>(),
                    It.IsAny<DependencyGraphSpec>(),
                    It.IsAny<IReadOnlyList<IAssetsLogMessage>>()))
                .Callback<ProjectNames, DependencyGraphSpec, IReadOnlyList<IAssetsLogMessage>>(
                    (_, dg, __) => { capturedRestoreSpec = dg; });

            // Act
            var result = await NominateProjectAsync(projectFullPath, pri, CancellationToken.None, cache: cache);

            Assert.True(result, "Project restore nomination should succeed.");

            return capturedRestoreSpec;
        }

        private Task<bool> NominateProjectAsync(
            string projectFullPath,
            IVsProjectRestoreInfo pri,
            CancellationToken cancellationToken,
            Mock<IProjectSystemCache> cache = null,
            Mock<ISolutionRestoreWorker> restoreWorker = null,
            Mock<ILogger> logger = null)
        {
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

            return service.NominateProjectAsync(projectFullPath, pri, cancellationToken);
        }

        private Task<bool> NominateProjectAsync(
            string projectFullPath,
            IVsProjectRestoreInfo2 pri,
            CancellationToken cancellationToken,
            Mock<IProjectSystemCache> cache = null,
            Mock<ISolutionRestoreWorker> restoreWorker = null,
            Mock<ILogger> logger = null)
        {
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

            return service.NominateProjectAsync(projectFullPath, pri, cancellationToken);
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

        private TestContext NewCpsProject(
            string projectJson = null,
            string projectName = null,
            bool crossTargeting = false)
        {
            const string DefaultProjectJson = @"{
    ""frameworks"": {
        ""netcoreapp1.0"": {
            ""dependencies"": { }
        }
    }
}";
            if (projectName == null)
            {
                projectName = $"{Guid.NewGuid()}";
            }

            var projectLocation = _testDirectory.Path;
            var projectFullPath = Path.Combine(projectLocation, $"{projectName}.csproj");
            var baseIntermediatePath = Path.Combine(projectLocation, "obj");
            Directory.CreateDirectory(baseIntermediatePath);

            var spec = JsonPackageSpecReader.GetPackageSpec(projectJson ?? DefaultProjectJson, projectName, projectFullPath);
            var builder = ProjectRestoreInfoBuilder.FromPackageSpec(
                spec,
                baseIntermediatePath,
                crossTargeting);

            return new TestContext
            {
                ProjectFullPath = projectFullPath,
                Builder = builder
            };
        }

        private static void AssertPackages(TargetFrameworkInformation actualTfi, params string[] expectedPackages)
        {
            var actualPackages = actualTfi
                .Dependencies
                .Where(ld => ld.LibraryRange.TypeConstraint == LibraryDependencyTarget.Package)
                .Select(ld => $"{ld.Name}:{ld.LibraryRange.VersionRange.OriginalString}");

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

        private class TestContext
        {
            public string ProjectFullPath { get; set; }
            public ProjectRestoreInfoBuilder Builder { get; set; }

            public VsProjectRestoreInfo ProjectRestoreInfo => Builder.ProjectRestoreInfo;

            public VsProjectRestoreInfo2 ProjectRestoreInfo2 => Builder.ProjectRestoreInfo2;

        }
    }
}
