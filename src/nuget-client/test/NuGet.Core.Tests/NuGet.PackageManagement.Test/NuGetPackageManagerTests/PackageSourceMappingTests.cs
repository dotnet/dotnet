// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.PackageManagement.Test.NuGetPackageManagerTests
{
    public class PackageSourceMappingTests
    {
        // Following are the various sets of packages that are small in size. To be used by the functional tests
        private readonly List<PackageIdentity> _packageWithDependents = new List<PackageIdentity>
            {
                new PackageIdentity("jQuery", new NuGetVersion("1.4.4")),
                new PackageIdentity("jQuery", new NuGetVersion("1.6.4")),
                new PackageIdentity("jQuery.Validation", new NuGetVersion("1.13.1")),
                new PackageIdentity("jQuery.UI.Combined", new NuGetVersion("1.11.2"))
            };

        private readonly XunitLogger _logger;

        public PackageSourceMappingTests(ITestOutputHelper output)
        {
            _logger = new XunitLogger(output);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public async Task PreviewInstallPackage_NewSourceMapping_AffectsRestoreSummaryRequest(bool isValidNewMappingSource, bool isValidNewMappingID)
        {
            // Arrange
            var package = _packageWithDependents[0];

            var packages = new List<SourcePackageDependencyInfo>
            {
                new SourcePackageDependencyInfo(package.Id, package.Version, new PackageDependency[] { }, listed: true, source: null),
            };
            SourceRepositoryProvider sourceRepositoryProvider = CreateSource(packages);

            // Set up NuGetProject
            var nugetProjectContext = new TestNuGetProjectContext();

            // Create Package Manager
            using var solutionManager = new TestSolutionManager();
            using var settingsDir = TestDirectory.Create();
            var settings = new Settings(settingsDir);

            var nuGetPackageManager = new NuGetPackageManager(
            sourceRepositoryProvider,
            settings,
            solutionManager,
            new TestDeleteOnRestartManager());

            var buildIntegratedProjectA = solutionManager.AddBuildIntegratedProject("projectA") as BuildIntegratedNuGetProject;

            // Act
            var primarySources = sourceRepositoryProvider.GetRepositories() as IReadOnlyCollection<SourceRepository>;
            PackageIdentity target = _packageWithDependents[0];
            IReadOnlyList<BuildIntegratedNuGetProject> projects = new List<BuildIntegratedNuGetProject>()
            {
                buildIntegratedProjectA
            };

            SourceRepository primarySource = primarySources.First();
            var newMappingSource = isValidNewMappingSource ? primarySource.PackageSource.Name : "invalidSource";
            var newMappingID = isValidNewMappingID ? target.Id : "invalidPackage";

            var nugetAction = NuGetProjectAction.CreateInstallProjectAction(target, primarySource, buildIntegratedProjectA);
            var actions = new NuGetProjectAction[] { nugetAction };

            var nugetProjectActionsLookup =
                new Dictionary<string, NuGetProjectAction[]>(PathUtility.GetStringComparerBasedOnOS())
            {
                { primarySource.PackageSource.Name, actions }
            };

            IEnumerable<ResolvedAction> resolvedActions = await nuGetPackageManager.PreviewBuildIntegratedProjectsActionsAsync(
                projects,
                nugetProjectActionsLookup: nugetProjectActionsLookup,
                packageIdentity: target,
                primarySources,
                nugetProjectContext,
                versionRange: null,
                newMappingID,
                newMappingSource,
                CancellationToken.None);

            // Assert

            Assert.Single(resolvedActions);
            ResolvedAction resolvedAction = resolvedActions.Single();
            Assert.IsType<BuildIntegratedProjectAction>(resolvedAction.Action);

            var buildIntegratedProjectAction = resolvedAction.Action as BuildIntegratedProjectAction;
            RestoreSummaryRequest summaryRequest = buildIntegratedProjectAction.RestoreResultPair.SummaryRequest;

            // Request should have "*" Pattern Mapping for the requested new mapping source.
            PackageSourceMapping requestedSourceMapping = summaryRequest.Request.PackageSourceMapping;
            Assert.True(requestedSourceMapping.IsEnabled);
            IReadOnlyList<string> mappedSources = requestedSourceMapping.GetConfiguredPackageSources(newMappingID);
            Assert.Contains(newMappingSource, mappedSources);
        }

        [Fact]
        public async Task Preview_InstallForPC_PackageSourceMapping_WithSingleFeed_Succeeds()
        {
            // Arrange
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Set up Package Source
                var sources = new List<PackageSource>();

                var privateRepositoryPath = Path.Combine(testSolutionManager.SolutionDirectory, "PrivateRepository");
                Directory.CreateDirectory(privateRepositoryPath);

                // Replace the default nuget.config with custom one.
                File.WriteAllText(testSolutionManager.NuGetConfigPath, $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""PrivateRepository"" value=""{privateRepositoryPath}"" />
    </packageSources>
    <packageSourceMapping>
        <packageSource key=""PrivateRepository"">
            <package pattern=""Contoso.*"" />
            <package pattern=""Test.*"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");
                var contosoPackageIdentity = new PackageIdentity("Contoso.A", new NuGetVersion("1.0.0"));

                var ContosoReal = new SimpleTestPackageContext()
                {
                    Id = contosoPackageIdentity.Id,
                    Version = "1.0.0"
                };
                ContosoReal.AddFile("lib/net461/contosoA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    privateRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ContosoReal);

                sources.Add(new PackageSource(privateRepositoryPath, "PrivateRepository"));

                SourceRepositoryProvider sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(sources);

                ISettings testSettings = Settings.LoadSpecificSettings(testSolutionManager.SolutionDirectory, "NuGet.Config");
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                var packageActions = (await nuGetPackageManager.PreviewInstallPackageAsync(msBuildNuGetProject, contosoPackageIdentity,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories(), null, token)).ToList();

                // Assert
                Assert.Equal(1, packageActions.Count());
                Assert.True(contosoPackageIdentity.Equals(packageActions[0].PackageIdentity));
                Assert.Equal(NuGetProjectActionType.Install, packageActions[0].NuGetProjectActionType);
                Assert.Equal(privateRepositoryPath,
                    packageActions[0].SourceRepository.PackageSource.Source);
            }
        }

        [Fact]
        public async Task Preview_InstallForPC_PackageSourceMapping_WithMultipleFeedsWithIdenticalPackages_RestoresCorrectPackage()
        {
            // This test same as having multiple source repositories and `All` option is selected in PMUI.
            // Arrange
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Set up Package Source
                var sources = new List<PackageSource>();
                var externalRepositoryPath = Path.Combine(testSolutionManager.SolutionDirectory, "ExternalRepository");
                Directory.CreateDirectory(externalRepositoryPath);

                var privateRepositoryPath = Path.Combine(testSolutionManager.SolutionDirectory, "PrivateRepository");
                Directory.CreateDirectory(privateRepositoryPath);

                // Replace the default nuget.config with custom one.
                File.WriteAllText(testSolutionManager.NuGetConfigPath, $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""ExternalRepository"" value=""{externalRepositoryPath}"" />
    <add key=""PrivateRepository"" value=""{privateRepositoryPath}"" />
    </packageSources>
    <packageSourceMapping>
        <packageSource key=""externalRepository"">
            <package pattern=""External.*"" />
            <package pattern=""Others.*"" />
        </packageSource>
        <packageSource key=""PrivateRepository"">
            <package pattern=""Contoso.*"" />
            <package pattern=""Test.*"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");
                var contosoPackageIdentity = new PackageIdentity("Contoso.A", new NuGetVersion("1.0.0"));

                var ExternalA = new SimpleTestPackageContext()
                {
                    Id = contosoPackageIdentity.Id,  // Package id conflict with PrivateRepository
                    Version = "1.0.0"
                };
                ExternalA.AddFile("lib/net461/externalA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    externalRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ExternalA);

                sources.Add(new PackageSource(externalRepositoryPath, "ExternalRepository"));

                var ContosoReal = new SimpleTestPackageContext()
                {
                    Id = contosoPackageIdentity.Id,
                    Version = "1.0.0"
                };
                ContosoReal.AddFile("lib/net461/contosoA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    privateRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ContosoReal);

                sources.Add(new PackageSource(privateRepositoryPath, "PrivateRepository"));

                SourceRepositoryProvider sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(sources);

                ISettings testSettings = Settings.LoadSpecificSettings(testSolutionManager.SolutionDirectory, "NuGet.Config");
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                var packageActions = (await nuGetPackageManager.PreviewInstallPackageAsync(msBuildNuGetProject, contosoPackageIdentity,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories(), null, token)).ToList();

                // Assert
                Assert.Equal(1, packageActions.Count());
                Assert.True(contosoPackageIdentity.Equals(packageActions[0].PackageIdentity));
                Assert.Equal(NuGetProjectActionType.Install, packageActions[0].NuGetProjectActionType);
                Assert.Equal(privateRepositoryPath,
                    packageActions[0].SourceRepository.PackageSource.Source);
            }
        }

        [Fact]
        public async Task Preview_InstallForPC_PackageSourceMapping_WithMultipleFeeds_SecondarySourcesNotConsidered()
        {
            // This test same as having multiple source repositories but only 1 sourcerepository is selected in PMUI.
            // Direct package dependencies doesn't consider secondary sources(not selected sources on UI).
            // Arrange
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Set up Package Source
                var primarySources = new List<PackageSource>();
                var secondarySources = new List<PackageSource>();
                var externalRepositoryPath = Path.Combine(testSolutionManager.SolutionDirectory, "ExternalRepository");
                Directory.CreateDirectory(externalRepositoryPath);

                var privateRepositoryPath = Path.Combine(testSolutionManager.SolutionDirectory, "PrivateRepository");
                Directory.CreateDirectory(privateRepositoryPath);

                // Replace the default nuget.config with custom one.
                File.WriteAllText(testSolutionManager.NuGetConfigPath, $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""ExternalRepository"" value=""{externalRepositoryPath}"" />
    <add key=""PrivateRepository"" value=""{privateRepositoryPath}"" />
    </packageSources>
    <packageSourceMapping>
        <packageSource key=""externalRepository"">
            <package pattern=""External.*"" />
            <package pattern=""Others.*"" />
        </packageSource>
        <packageSource key=""PrivateRepository"">
            <package pattern=""Contoso.*"" />
            <package pattern=""Test.*"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");
                var contosoPackageIdentity = new PackageIdentity("Contoso.A", new NuGetVersion("1.0.0"));

                var ExternalA = new SimpleTestPackageContext()
                {
                    Id = contosoPackageIdentity.Id,  // Package id conflict with PrivateRepository
                    Version = "1.0.0"
                };
                ExternalA.AddFile("lib/net461/externalA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    externalRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ExternalA);

                var primarySource = new PackageSource(externalRepositoryPath, "ExternalRepository");

                var ContosoReal = new SimpleTestPackageContext()
                {
                    Id = contosoPackageIdentity.Id,
                    Version = "1.0.0"
                };
                ContosoReal.AddFile("lib/net461/contosoA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    privateRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ContosoReal);

                primarySources.Add(primarySource);
                secondarySources.AddRange(primarySources);
                secondarySources.Add(new PackageSource(privateRepositoryPath, "PrivateRepository"));

                SourceRepositoryProvider primarySourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(primarySources);
                SourceRepositoryProvider secondarySourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(secondarySources);

                ISettings testSettings = Settings.LoadSpecificSettings(testSolutionManager.SolutionDirectory, "NuGet.Config");
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    secondarySourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act and Assert
                var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                {
                    await nuGetPackageManager.PreviewInstallPackageAsync(msBuildNuGetProject, contosoPackageIdentity.Id,
                    new ResolutionContext(), new TestNuGetProjectContext(), primarySourceRepositoryProvider.GetRepositories(), secondarySourceRepositoryProvider.GetRepositories(), token);
                });

                // Even though Contoso.A 1.0.0 exist in ExternalRepository it wouldn't be picked for install.
                // PrivateRepository is passed in secondary sources but for packages.config project not considered.
                Assert.True(ex.Message.StartsWith("Package 'Contoso.A 1.0.0' is not found in the following primary source(s): "));
            }
        }

        [Fact]
        public async Task Preview_InstallForPC_PackageSourceMapping_WithMultipleFeeds_ForTransitiveDepency_SecondarySourcesConsidered()
        {
            // This test same as having multiple source repositories but only 1 sourcerepository is selected in PMUI.
            // Even though direct package dependencies doesn't consider secondary sources(not selected sources on UI), but transitive dependencies do consider it.
            // Arrange
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Set up Package Source
                var primarySources = new List<PackageSource>();
                var secondarySources = new List<PackageSource>();
                var externalRepositoryPath = Path.Combine(testSolutionManager.SolutionDirectory, "ExternalRepository");
                Directory.CreateDirectory(externalRepositoryPath);

                var privateRepositoryPath = Path.Combine(testSolutionManager.SolutionDirectory, "PrivateRepository");
                Directory.CreateDirectory(privateRepositoryPath);

                // Replace the default nuget.config with custom one.
                File.WriteAllText(testSolutionManager.NuGetConfigPath, $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""ExternalRepository"" value=""{externalRepositoryPath}"" />
    <add key=""PrivateRepository"" value=""{privateRepositoryPath}"" />
    </packageSources>
    <packageSourceMapping>
        <packageSource key=""externalRepository"">
            <package pattern=""Direct.*"" />
            <package pattern=""External.*"" />
            <package pattern=""Others.*"" />
        </packageSource>
        <packageSource key=""PrivateRepository"">
            <package pattern=""Contoso.*"" />
            <package pattern=""Test.*"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");
                var contosoPackageIdentity = new PackageIdentity("Contoso.A", new NuGetVersion("1.0.0"));
                var directPackageIdentity = new PackageIdentity("Direct.A", new NuGetVersion("1.0.0"));

                var ExternalContosoA = new SimpleTestPackageContext()
                {
                    Id = contosoPackageIdentity.Id,  // Package id conflict with PrivateRepository
                    Version = "1.0.0"
                };
                ExternalContosoA.AddFile("lib/net461/externalA.dll");

                var ExternalDirectA = new SimpleTestPackageContext()
                {
                    Id = directPackageIdentity.Id,
                    Version = "1.0.0",
                    Dependencies = new List<SimpleTestPackageContext>() { ExternalContosoA } // We set Contoso.A as dependent package.
                };
                ExternalDirectA.AddFile("lib/net461/directA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    externalRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ExternalContosoA, ExternalDirectA);

                var primarySource = new PackageSource(externalRepositoryPath, "ExternalRepository");

                var ContosoReal = new SimpleTestPackageContext()
                {
                    Id = contosoPackageIdentity.Id,
                    Version = "1.0.0"
                };
                ContosoReal.AddFile("lib/net461/contosoA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    privateRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ContosoReal);

                primarySources.Add(primarySource);
                secondarySources.AddRange(primarySources);
                secondarySources.Add(new PackageSource(privateRepositoryPath, "PrivateRepository"));

                SourceRepositoryProvider primarySourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(primarySources);
                SourceRepositoryProvider secondarySourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(secondarySources);

                ISettings testSettings = Settings.LoadSpecificSettings(testSolutionManager.SolutionDirectory, "NuGet.Config");
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    secondarySourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                var packageActions = (await nuGetPackageManager.PreviewInstallPackageAsync(msBuildNuGetProject, directPackageIdentity.Id,
                    new ResolutionContext(), new TestNuGetProjectContext(), primarySourceRepositoryProvider.GetRepositories(), secondarySourceRepositoryProvider.GetRepositories(), token)).ToList();

                // Assert
                Assert.Equal(2, packageActions.Count());
                Assert.True(contosoPackageIdentity.Equals(packageActions[0].PackageIdentity));
                Assert.Equal(NuGetProjectActionType.Install, packageActions[0].NuGetProjectActionType);
                // Contoso.A comes from PrivateRepository due to package source mapping filtering even though same package Id exist in Externalrepository.
                Assert.Equal(privateRepositoryPath,
                    packageActions[0].SourceRepository.PackageSource.Source);

                Assert.True(directPackageIdentity.Equals(packageActions[1].PackageIdentity));
                Assert.Equal(NuGetProjectActionType.Install, packageActions[1].NuGetProjectActionType);
                // Direct.A comes from Externalrepository.
                Assert.Equal(externalRepositoryPath,
                    packageActions[1].SourceRepository.PackageSource.Source);
            }
        }

        [Fact]
        public async Task Preview_UpdateForPC_PackageSourceMapping_WithMultipleFeeds_SecondarySourcesNotConsidered()
        {
            // This test same as having multiple source repositories but only 1 sourcerepository is selected in PMUI.
            // Arrange
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Set up Package Source
                var primarySources = new List<PackageSource>();
                var secondarySources = new List<PackageSource>();
                var externalRepositoryPath = Path.Combine(testSolutionManager.SolutionDirectory, "ExternalRepository");
                Directory.CreateDirectory(externalRepositoryPath);

                var privateRepositoryPath = Path.Combine(testSolutionManager.SolutionDirectory, "PrivateRepository");
                Directory.CreateDirectory(privateRepositoryPath);

                // Replace the default nuget.config with custom one.
                File.WriteAllText(testSolutionManager.NuGetConfigPath, $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""ExternalRepository"" value=""{externalRepositoryPath}"" />
    <add key=""PrivateRepository"" value=""{privateRepositoryPath}"" />
    </packageSources>
    <packageSourceMapping>
        <packageSource key=""externalRepository"">
            <package pattern=""External.*"" />
            <package pattern=""Others.*"" />
        </packageSource>
        <packageSource key=""PrivateRepository"">
            <package pattern=""Contoso.*"" />
            <package pattern=""Test.*"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");
                var contosoPackageIdentity = new PackageIdentity("Contoso.A", new NuGetVersion("1.0.0"));

                var ExternalA_v1 = new SimpleTestPackageContext()
                {
                    Id = contosoPackageIdentity.Id,  // Package id conflict with PrivateRepository
                    Version = "1.0.0"
                };
                ExternalA_v1.AddFile("lib/net461/externalA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    externalRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ExternalA_v1);

                var ExternalA_v2 = new SimpleTestPackageContext()
                {
                    Id = contosoPackageIdentity.Id,  // Package id conflict with PrivateRepository
                    Version = "2.0.0"
                };
                ExternalA_v2.AddFile("lib/net461/externalA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    externalRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ExternalA_v2);

                var primarySource = new PackageSource(externalRepositoryPath, "ExternalRepository");

                var ContosoReal_V1 = new SimpleTestPackageContext()
                {
                    Id = contosoPackageIdentity.Id,
                    Version = "1.0.0"
                };
                ContosoReal_V1.AddFile("lib/net461/contosoA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    privateRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ContosoReal_V1);

                var ContosoReal_V2 = new SimpleTestPackageContext()
                {
                    Id = contosoPackageIdentity.Id,
                    Version = "2.0.0"
                };
                ContosoReal_V2.AddFile("lib/net461/contosoA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    privateRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ContosoReal_V2);

                primarySources.Add(primarySource);
                secondarySources.AddRange(primarySources);
                secondarySources.Add(new PackageSource(privateRepositoryPath, "PrivateRepository"));

                SourceRepositoryProvider primarySourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(primarySources);
                SourceRepositoryProvider secondarySourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(secondarySources);

                ISettings testSettings = Settings.LoadSpecificSettings(testSolutionManager.SolutionDirectory, "NuGet.Config");
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    secondarySourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(testSolutionManager, testSettings);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;

                var resolutionContext = new ResolutionContext();
                var resolvedPackage = await NuGetPackageManager.GetLatestVersionAsync(
                    contosoPackageIdentity.Id,
                    msBuildNuGetProject,
                    new ResolutionContext(),
                    secondarySourceRepositoryProvider.GetRepositories().First(),
                    NullLogger.Instance,
                    token);

                var packageLatest = new PackageIdentity(contosoPackageIdentity.Id, resolvedPackage.LatestVersion);
                var nugetProjectContext = new TestNuGetProjectContext();

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act
                // Passing both ExternalRepository and PrivateRepository via secondarySourceRepositoryProvider as primary sources, so it can install.
                await nuGetPackageManager.InstallPackageAsync(msBuildNuGetProject, contosoPackageIdentity,
                    resolutionContext, nugetProjectContext, secondarySourceRepositoryProvider.GetRepositories(), null, token);

                // Assert
                // Check that the packages.config file exists after the installation
                Assert.True(File.Exists(packagesConfigPath));
                // Check the number of packages and packages returned by PackagesConfigProject after the installation
                packagesInPackagesConfig = (await msBuildNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(1, packagesInPackagesConfig.Count);
                Assert.Equal(contosoPackageIdentity, packagesInPackagesConfig[0].PackageIdentity);
                Assert.Equal(msBuildNuGetProject.ProjectSystem.TargetFramework, packagesInPackagesConfig[0].TargetFramework);

                // Main Act and Assert
                var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                {
                    await nuGetPackageManager.PreviewUpdatePackagesAsync(
                        new List<NuGetProject> { msBuildNuGetProject },
                        new ResolutionContext(DependencyBehavior.Highest, false, true, VersionConstraints.None),
                        nugetProjectContext,
                        primarySourceRepositoryProvider.GetRepositories(),
                        secondarySourceRepositoryProvider.GetRepositories(),
                        token);
                });

                // Even though Contoso.A 2.0.0 exist in ExternalRepository it wouldn't be picked for install.
                // PrivateRepository is passed in secondary sources but for packages.config project not considered.
                // Since we requested bulk upgrade the error message would be generic.
                Assert.True(ex.Message.StartsWith("Unable to gather dependency information for multiple packages"));
            }
        }

        [Fact]
        public async Task Preview_UpdateForPC_PackageSourceMapping_WithMultipleFeeds_Fails()
        {
            // Arrange
            using (var testSolutionManager = new TestSolutionManager())
            {
                // Set up Package Source
                var sources = new List<PackageSource>();
                var externalRepositoryPath = Path.Combine(testSolutionManager.SolutionDirectory, "ExternalRepository");
                Directory.CreateDirectory(externalRepositoryPath);

                var privateRepositoryPath = Path.Combine(testSolutionManager.SolutionDirectory, "PrivateRepository");
                Directory.CreateDirectory(privateRepositoryPath);

                // Replace the default nuget.config with custom one.
                File.WriteAllText(testSolutionManager.NuGetConfigPath, $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""ExternalRepository"" value=""{externalRepositoryPath}"" />
    <add key=""PrivateRepository"" value=""{privateRepositoryPath}"" />
    </packageSources>
    <packageSourceMapping>
        <packageSource key=""externalRepository"">
            <package pattern=""External.*"" />
            <package pattern=""Others.*"" />
        </packageSource>
        <packageSource key=""PrivateRepository"">
            <package pattern=""Contoso.*"" />
            <package pattern=""Test.*"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");
                var contosoPackageIdentity = new PackageIdentity("Contoso.A", new NuGetVersion("1.0.0"));

                var ExternalA = new SimpleTestPackageContext()
                {
                    Id = contosoPackageIdentity.Id,  // Initial version had package id conflict with Contoso repository
                    Version = "1.0.0"
                };
                ExternalA.AddFile("lib/net461/externalA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    externalRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ExternalA);

                var ExternalB = new SimpleTestPackageContext()
                {
                    Id = "External.B",  // name conflict resolved.
                    Version = "2.0.0"
                };
                ExternalB.AddFile("lib/net461/externalB.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    externalRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ExternalB);

                sources.Add(new PackageSource(externalRepositoryPath, "ExternalRepository"));

                var ContosoReal = new SimpleTestPackageContext()
                {
                    Id = contosoPackageIdentity.Id,
                    Version = "2.0.0"
                };
                ContosoReal.AddFile("lib/net461/contosoA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    privateRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ContosoReal);

                sources.Add(new PackageSource(privateRepositoryPath, "PrivateRepository"));

                SourceRepositoryProvider sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(sources);

                ISettings testSettings = Settings.LoadSpecificSettings(testSolutionManager.SolutionDirectory, "NuGet.Config");
                var token = CancellationToken.None;
                var deleteOnRestartManager = new TestDeleteOnRestartManager();
                var nuGetPackageManager = new NuGetPackageManager(
                    sourceRepositoryProvider,
                    testSettings,
                    testSolutionManager,
                    deleteOnRestartManager);

                var msBuildNuGetProject = testSolutionManager.AddNewMSBuildProject();
                var msBuildNuGetProjectSystem = msBuildNuGetProject.ProjectSystem as TestMSBuildNuGetProjectSystem;
                var packagesConfigPath = msBuildNuGetProject.PackagesConfigNuGetProject.FullPath;

                // Pre-Assert
                // Check that the packages.config file does not exist
                Assert.False(File.Exists(packagesConfigPath));
                // Check that there are no packages returned by PackagesConfigProject
                var packagesInPackagesConfig = (await msBuildNuGetProject.PackagesConfigNuGetProject.GetInstalledPackagesAsync(token)).ToList();
                Assert.Equal(0, packagesInPackagesConfig.Count);
                Assert.Equal(0, msBuildNuGetProjectSystem.References.Count);

                // Act and Assert
                var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                {
                    await nuGetPackageManager.PreviewInstallPackageAsync(msBuildNuGetProject, contosoPackageIdentity,
                    new ResolutionContext(), new TestNuGetProjectContext(), sourceRepositoryProvider.GetRepositories(), null, token);
                });

                // Even though Contoso.A 1.0.0 exist in ExternalRepository it wouldn't be picked for install.
                Assert.True(ex.Message.StartsWith("Package 'Contoso.A 1.0.0' is not found in the following primary source(s): "));
            }
        }

        private SourceRepositoryProvider CreateSource(List<SourcePackageDependencyInfo> packages)
        {
            var resourceProviders = new List<Lazy<INuGetResourceProvider>>();
            resourceProviders.Add(new Lazy<INuGetResourceProvider>(() => new TestDependencyInfoProvider(packages)));
            resourceProviders.Add(new Lazy<INuGetResourceProvider>(() => new TestMetadataProvider(packages)));

            var packageSource = new PackageSource("http://temp");
            var packageSourceProvider = new TestPackageSourceProvider(new[] { packageSource });

            return new SourceRepositoryProvider(packageSourceProvider, resourceProviders);
        }
    }
}
