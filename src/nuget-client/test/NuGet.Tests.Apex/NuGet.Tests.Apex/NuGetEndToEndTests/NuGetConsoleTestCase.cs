// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Test.Apex.VisualStudio.Solution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet.Test.Utility;

namespace NuGet.Tests.Apex
{
    [TestClass]
    public class NuGetConsoleTestCase : SharedVisualStudioHostTestClass
    {
        [DataTestMethod]
        [DynamicData(nameof(GetPackageReferenceTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task InstallPackageFromPMCWithNoAutoRestoreVerifyAssetsFileAsync(ProjectTemplate projectTemplate)
        {
            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, noAutoRestore: true, addNetStandardFeeds: true))
            {
                var packageName = "TestPackage";
                var packageVersion = "1.0.0";
                await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion);

                var nugetConsole = GetConsole(testContext.Project);

                nugetConsole.InstallPackageFromPMC(packageName, packageVersion);

                CommonUtility.AssertPackageInAssetsFile(VisualStudio, testContext.Project, packageName, packageVersion, Logger);
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackagesConfigTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task InstallPackageFromPMCVerifyInstallForPCAsync(ProjectTemplate projectTemplate)
        {
            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger))
            {
                var packageName = "TestPackage";
                var packageVersion = "1.0.0";
                await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion);

                var nugetConsole = GetConsole(testContext.Project);

                nugetConsole.InstallPackageFromPMC(packageName, packageVersion);

                CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion, Logger);
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackagesConfigTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task UninstallPackageFromPMCForPCAsync(ProjectTemplate projectTemplate)
        {
            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger))
            {
                var packageName = "TestPackage";
                var packageVersion = "1.0.0";
                await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion);

                var nugetConsole = GetConsole(testContext.Project);

                nugetConsole.InstallPackageFromPMC(packageName, packageVersion);
                nugetConsole.UninstallPackageFromPMC(packageName);

                CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion, Logger);
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackagesConfigTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task UpdatePackageFromPMCForPCAsync(ProjectTemplate projectTemplate)
        {
            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger))
            {
                var packageName = "TestPackage";
                var packageVersion1 = "1.0.0";
                var packageVersion2 = "2.0.0";
                await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion1);
                await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion2);

                var nugetConsole = GetConsole(testContext.Project);

                nugetConsole.InstallPackageFromPMC(packageName, packageVersion1);
                nugetConsole.UpdatePackageFromPMC(packageName, packageVersion2);

                CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion2, Logger);
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackagesConfigTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task InstallMultiplePackagesFromPMCForPCAsync(ProjectTemplate projectTemplate)
        {
            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger))
            {
                var packageName1 = "TestPackage1";
                var packageVersion1 = "1.0.0";
                await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName1, packageVersion1);

                var packageName2 = "TestPackage2";
                var packageVersion2 = "1.2.3";
                await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName2, packageVersion2);

                var nugetConsole = GetConsole(testContext.Project);

                nugetConsole.InstallPackageFromPMC(packageName1, packageVersion1);
                nugetConsole.InstallPackageFromPMC(packageName2, packageVersion2);

                CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName1, packageVersion1, Logger);
                CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName2, packageVersion2, Logger);
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackagesConfigTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task UninstallMultiplePackagesFromPMCForPCAsync(ProjectTemplate projectTemplate)
        {
            var packageName1 = "TestPackage1";
            var packageVersion1 = "1.0.0";
            var packageName2 = "TestPackage2";
            var packageVersion2 = "1.2.3";

            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger))
            {
                await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName1, packageVersion1);
                await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName2, packageVersion2);

                var nugetConsole = GetConsole(testContext.Project);

                nugetConsole.InstallPackageFromPMC(packageName1, packageVersion1);
                testContext.SolutionService.Build();
                testContext.NuGetApexTestService.WaitForAutoRestore();

                nugetConsole.InstallPackageFromPMC(packageName2, packageVersion2);
                testContext.SolutionService.Build();
                testContext.NuGetApexTestService.WaitForAutoRestore();

                nugetConsole.UninstallPackageFromPMC(packageName1);
                testContext.SolutionService.Build();
                testContext.NuGetApexTestService.WaitForAutoRestore();

                nugetConsole.UninstallPackageFromPMC(packageName2);
                testContext.SolutionService.Build();
                testContext.NuGetApexTestService.WaitForAutoRestore();

                CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, testContext.Project, packageName1, packageVersion1, Logger);
                CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, testContext.Project, packageName2, packageVersion2, Logger);
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackagesConfigTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task DowngradePackageFromPMCForPCAsync(ProjectTemplate projectTemplate)
        {
            var packageName = "TestPackage";
            var packageVersion1 = "1.0.0";
            var packageVersion2 = "2.0.0";

            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger))
            {
                await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion1);
                await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion2);

                var nugetConsole = GetConsole(testContext.Project);

                nugetConsole.InstallPackageFromPMC(packageName, packageVersion2);
                nugetConsole.UpdatePackageFromPMC(packageName, packageVersion1);

                CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion1, Logger);
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetNetCoreTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task NetCoreTransitivePackageReferenceLimitAsync(ProjectTemplate projectTemplate)
        {
            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, addNetStandardFeeds: true))
            {
                var project2 = testContext.SolutionService.AddProject(ProjectLanguage.CSharp, projectTemplate, CommonUtility.DefaultTargetFramework, "TestProject2");
                project2.Build();
                var project3 = testContext.SolutionService.AddProject(ProjectLanguage.CSharp, projectTemplate, CommonUtility.DefaultTargetFramework, "TestProject3");
                project3.Build();
                var projectX = testContext.SolutionService.AddProject(ProjectLanguage.CSharp, projectTemplate, CommonUtility.DefaultTargetFramework, "TestProjectX");
                projectX.Build();
                testContext.SolutionService.Build();

                testContext.Project.References.Dte.AddProjectReference(project2);
                testContext.Project.References.Dte.AddProjectReference(projectX);
                project2.References.Dte.AddProjectReference(project3);
                testContext.SolutionService.SaveAll();
                testContext.SolutionService.Build();

                var nugetConsole = GetConsole(project3);

                var packageName = "newtonsoft.json";
                var packageVersion = "9.0.1";
                await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion);

                nugetConsole.InstallPackageFromPMC(packageName, packageVersion);
                testContext.SolutionService.Build();
                project2.Build();
                project3.Build();
                projectX.Build();
                testContext.SolutionService.Build();

                CommonUtility.AssertPackageInAssetsFile(VisualStudio, project3, packageName, packageVersion, Logger);
                CommonUtility.AssertPackageInAssetsFile(VisualStudio, testContext.Project, packageName, packageVersion, Logger);
                CommonUtility.AssertPackageInAssetsFile(VisualStudio, project2, packageName, packageVersion, Logger);
                CommonUtility.AssertPackageNotInAssetsFile(VisualStudio, projectX, packageName, packageVersion, Logger);
            }
        }

        [DataTestMethod]
        [DataRow(ProjectTemplate.ClassLibrary, false)]
        [DataRow(ProjectTemplate.NetCoreConsoleApp, true)]
        [DataRow(ProjectTemplate.NetStandardClassLib, true)]
        [Timeout(DefaultTimeout)]
        public async Task InstallAndUpdatePackageWithSourceParameterWarnsAsync(ProjectTemplate projectTemplate, bool warns)
        {
            var packageName = "TestPackage";
            var packageVersion1 = "1.0.0";
            var packageVersion2 = "2.0.0";
            var source = "https://api.nuget.org/v3/index.json";

            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, addNetStandardFeeds: true))
            {
                // Arrange
                var solutionService = VisualStudio.Get<SolutionService>();
                testContext.SolutionService.Build();

                await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion1);
                await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion2);

                var nugetTestService = GetNuGetTestService();
                var nugetConsole = GetConsole(testContext.Project);

                // Act
                nugetConsole.InstallPackageFromPMC(packageName, packageVersion1, source);
                testContext.SolutionService.Build();

                // Assert
                var expectedMessage = $"The 'Source' parameter is not respected for the transitive package management based project(s) {Path.GetFileNameWithoutExtension(testContext.Project.UniqueName)}. The enabled sources in your NuGet configuration will be used";
                Assert.IsTrue(warns == nugetConsole.IsMessageFoundInPMC(expectedMessage), expectedMessage);
                VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
                Assert.IsTrue(VisualStudio.HasNoErrorsInOutputWindows());

                // setup again
                nugetConsole.Clear();

                // Act
                nugetConsole.UpdatePackageFromPMC(packageName, packageVersion2, source);
                testContext.SolutionService.Build();

                // Assert
                Assert.IsTrue(warns == nugetConsole.IsMessageFoundInPMC(expectedMessage), expectedMessage);
                VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
                Assert.IsTrue(VisualStudio.HasNoErrorsInOutputWindows());

                nugetConsole.Clear();
                solutionService.Save();
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackagesConfigTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task InstallPackageForPC_PackageSourceMapping_WithSingleFeed(ProjectTemplate projectTemplate)
        {
            // Arrange
            using var simpleTestPathContext = new SimpleTestPathContext();

            var packageName = "Contoso.A";
            var packageVersion = "1.0.0";

            await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, packageVersion);
            simpleTestPathContext.Settings.AddPackageSourceMapping("source", "Contoso.*", "Test.*");

            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, noAutoRestore: false, addNetStandardFeeds: false, simpleTestPathContext: simpleTestPathContext))
            {
                var nugetConsole = GetConsole(testContext.Project);

                // Act
                nugetConsole.InstallPackageFromPMC(packageName, packageVersion);
                testContext.SolutionService.SaveAll();

                // Assert
                CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion, Logger);
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackagesConfigTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task UpdatePackageForPC_PackageSourceMapping_WithSingleFeed(ProjectTemplate projectTemplate)
        {
            // Arrange
            using var simpleTestPathContext = new SimpleTestPathContext();

            var packageName = "Contoso.A";
            var packageVersion1 = "1.0.0";
            var packageVersion2 = "2.0.0";

            await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, packageVersion1);
            await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, packageVersion2);
            simpleTestPathContext.Settings.AddPackageSourceMapping("source", "Contoso.*", "Test.*");

            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, noAutoRestore: false, addNetStandardFeeds: false, simpleTestPathContext: simpleTestPathContext))
            {
                var nugetConsole = GetConsole(testContext.Project);

                // Act
                nugetConsole.InstallPackageFromPMC(packageName, packageVersion1);
                nugetConsole.UpdatePackageFromPMC(packageName, packageVersion2);
                testContext.SolutionService.SaveAll();

                // Assert
                CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion2, Logger);
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackagesConfigTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task InstallPackageForPC_PackageSourceMapping_WithMultipleFeedsWithIdenticalPackages_InstallsCorrectPackage(ProjectTemplate projectTemplate)
        {
            // Arrange
            using var simpleTestPathContext = new SimpleTestPathContext();
            string solutionDirectory = simpleTestPathContext.SolutionRoot;
            var packageName = "Contoso.A";
            var packageVersion1 = "1.0.0";
            var packageVersion2 = "2.0.0";

            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, packageVersion1, "Thisisfromopensourcerepo1.txt");
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, packageVersion2, "Thisisfromopensourcerepo2.txt");

            var privateRepositoryPath = Path.Combine(solutionDirectory, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(privateRepositoryPath, packageName, packageVersion1, "Thisisfromprivaterepo1.txt");
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(privateRepositoryPath, packageName, packageVersion2, "Thisisfromprivaterepo2.txt");

            simpleTestPathContext.Settings.AddSource("PrivateRepository", privateRepositoryPath);
            simpleTestPathContext.Settings.AddPackageSourceMapping("source", "External.*", "Others.*");
            simpleTestPathContext.Settings.AddPackageSourceMapping("PrivateRepository", "Contoso.*", "Test.*");

            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, noAutoRestore: false, addNetStandardFeeds: false, simpleTestPathContext: simpleTestPathContext))
            {
                var nugetConsole = GetConsole(testContext.Project);

                // Act
                nugetConsole.InstallPackageFromPMC(packageName, packageVersion1);
                testContext.SolutionService.SaveAll();

                // Assert
                CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion1, Logger);

                var packagesDirectory = Path.Combine(solutionDirectory, "packages");
                var uniqueContentFile = Path.Combine(packagesDirectory, packageName + '.' + packageVersion1, "lib", "net45", "Thisisfromprivaterepo1.txt");
                // Make sure name squatting package not restored from opensource repository.
                Assert.IsTrue(File.Exists(uniqueContentFile));
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackagesConfigTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task UpdatePackageForPC_PackageSourceMapping_WithMultipleFeedsWithIdenticalPackages_UpdatesCorrectPackage(ProjectTemplate projectTemplate)
        {
            // Arrange
            using var simpleTestPathContext = new SimpleTestPathContext();
            string solutionDirectory = simpleTestPathContext.SolutionRoot;
            var packageName = "Contoso.A";
            var packageVersion1 = "1.0.0";
            var packageVersion2 = "2.0.0";

            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, packageVersion1, "Thisisfromopensourcerepo1.txt");
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, packageVersion2, "Thisisfromopensourcerepo2.txt");

            var privateRepositoryPath = Path.Combine(solutionDirectory, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(privateRepositoryPath, packageName, packageVersion1, "Thisisfromprivaterepo1.txt");
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(privateRepositoryPath, packageName, packageVersion2, "Thisisfromprivaterepo2.txt");

            simpleTestPathContext.Settings.AddSource("PrivateRepository", privateRepositoryPath);
            simpleTestPathContext.Settings.AddPackageSourceMapping("source", "External.*", "Others.*");
            simpleTestPathContext.Settings.AddPackageSourceMapping("PrivateRepository", "Contoso.*", "Test.*");

            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, noAutoRestore: false, addNetStandardFeeds: false, simpleTestPathContext: simpleTestPathContext))
            {
                var nugetConsole = GetConsole(testContext.Project);

                // Act
                nugetConsole.InstallPackageFromPMC(packageName, packageVersion1);
                nugetConsole.UpdatePackageFromPMC(packageName, packageVersion2);
                testContext.SolutionService.SaveAll();

                // Assert
                CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion2, Logger);

                var packagesDirectory = Path.Combine(solutionDirectory, "packages");
                var uniqueContentFile = Path.Combine(packagesDirectory, packageName + '.' + packageVersion2, "lib", "net45", "Thisisfromprivaterepo2.txt");
                // Make sure name squatting package not restored from opensource repository.
                Assert.IsTrue(File.Exists(uniqueContentFile));
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackagesConfigTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task RestorePackageForPC_PackageSourceMapping_WithSingleFeed(ProjectTemplate projectTemplate)
        {
            // Arrange
            using var simpleTestPathContext = new SimpleTestPathContext();

            var packageName = "SolutionLevelPkg";
            var packageVersion = "1.0.0";

            await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, packageVersion);
            simpleTestPathContext.Settings.AddPackageSourceMapping("source", "Soluti*");

            using var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, noAutoRestore: false, addNetStandardFeeds: false, simpleTestPathContext: simpleTestPathContext);
            var nugetConsole = GetConsole(testContext.Project);

            // Install the package so the project system is aware of packages.config
            nugetConsole.InstallPackageFromPMC(packageName, packageVersion);
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion, Logger);

            // Delete both the packages folder and global packages folder to force restore to re-download
            Directory.Delete(simpleTestPathContext.PackagesV2, recursive: true);
            Directory.Delete(simpleTestPathContext.UserPackagesFolder, recursive: true);

            // Act
            testContext.SolutionService.Build();
            testContext.SolutionService.SaveAll();

            // Assert
            var nupkgPath = Path.Combine(simpleTestPathContext.PackagesV2, $"{packageName}.{packageVersion}", $"{packageName}.{packageVersion}.nupkg");
            Assert.IsTrue(File.Exists(nupkgPath), $"'{nupkgPath}' should exist");
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackagesConfigTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task RestorePackageForPC_PackageSourceMapping_WithMultipleFeedsWithIdenticalPackages_RestoresCorrectPackage(ProjectTemplate projectTemplate)
        {
            // Arrange
            using var simpleTestPathContext = new SimpleTestPathContext();
            string solutionDirectory = simpleTestPathContext.SolutionRoot;

            var packageName = "Contoso.MVC.ASP";
            var packageVersion = "1.0.0";

            var privateRepositoryPath = Path.Combine(solutionDirectory, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, packageVersion, "Thisisfromopensourcerepo.txt");
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(privateRepositoryPath, packageName, packageVersion, "Thisisfromprivaterepo.txt");

            simpleTestPathContext.Settings.AddSource("PrivateRepository", privateRepositoryPath);
            simpleTestPathContext.Settings.AddPackageSourceMapping("source", "Others.*");
            simpleTestPathContext.Settings.AddPackageSourceMapping("PrivateRepository", "Contoso.MVC.*");

            using var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, noAutoRestore: false, addNetStandardFeeds: false, simpleTestPathContext: simpleTestPathContext);
            var nugetConsole = GetConsole(testContext.Project);

            // Install the package so the project system is aware of packages.config
            nugetConsole.InstallPackageFromPMC(packageName, packageVersion);
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion, Logger);

            // Delete both the packages folder and global packages folder to force restore to re-download
            Directory.Delete(simpleTestPathContext.PackagesV2, recursive: true);
            Directory.Delete(simpleTestPathContext.UserPackagesFolder, recursive: true);

            // Act
            testContext.SolutionService.Build();
            testContext.SolutionService.SaveAll();

            // Assert
            var packageFolder = Path.Combine(simpleTestPathContext.PackagesV2, $"{packageName}.{packageVersion}");
            Assert.IsTrue(File.Exists(Path.Combine(packageFolder, $"{packageName}.{packageVersion}.nupkg")), "Package nupkg should exist");
            // Make sure name squatting package not restored from opensource repository.
            var uniqueContentFile = Path.Combine(packageFolder, "lib", "net45", "Thisisfromprivaterepo.txt");
            Assert.IsTrue(File.Exists(uniqueContentFile), $"'{uniqueContentFile}' should exist");
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackagesConfigTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task InstallPackageForPC_PackageSourceMapping_WithWrongMappedFeed_Fails(ProjectTemplate projectTemplate)
        {
            // Arrange
            using var simpleTestPathContext = new SimpleTestPathContext();
            var privateRepositoryPath = Path.Combine(simpleTestPathContext.SolutionRoot, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            var packageName = "Pkg";
            var packageVersion = "1.0.0";

            await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, packageVersion);
            Assert.IsTrue(File.Exists(Path.Combine(simpleTestPathContext.PackageSource, $"{packageName}.{packageVersion}.nupkg")));
            simpleTestPathContext.Settings.AddSource("PrivateRepository", privateRepositoryPath);
            simpleTestPathContext.Settings.AddPackageSourceMapping("PrivateRepository", "Solution*");

            using var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, noAutoRestore: false, addNetStandardFeeds: false, simpleTestPathContext: simpleTestPathContext);
            var nugetConsole = GetConsole(testContext.Project);

            // Act
            nugetConsole.InstallPackageFromPMC(packageName, packageVersion);
            testContext.SolutionService.SaveAll();

            // Assert
            CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, testContext.Project, packageName, Logger);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackagesConfigTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task InstallPackageForPC_PackageSourceMapping_WithCorrectSourceOption_InstallsCorrectPackage(ProjectTemplate projectTemplate)
        {
            // Arrange
            using var simpleTestPathContext = new SimpleTestPathContext();
            var solutionDirectory = simpleTestPathContext.SolutionRoot;

            var privateRepositoryPath = Path.Combine(solutionDirectory, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            var packageName = "Contoso.MVC.ASP";
            var packageVersion = "1.0.0";

            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, packageVersion, "Thisisfromopensourcerepo1.txt");
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(privateRepositoryPath, packageName, packageVersion, "Thisisfromprivaterepo1.txt");
            simpleTestPathContext.Settings.AddSource("PrivateRepository", privateRepositoryPath);
            simpleTestPathContext.Settings.AddPackageSourceMapping("PrivateRepository", "Contoso.MVC.*");

            using var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, noAutoRestore: false, addNetStandardFeeds: false, simpleTestPathContext: simpleTestPathContext);
            var nugetConsole = GetConsole(testContext.Project);

            // Act
            nugetConsole.InstallPackageFromPMC(packageName, packageVersion, privateRepositoryPath);
            testContext.SolutionService.SaveAll();

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion, Logger);

            var packagesDirectory = Path.Combine(solutionDirectory, "packages");
            var uniqueContentFile = Path.Combine(packagesDirectory, packageName + '.' + packageVersion, "lib", "net45", "Thisisfromprivaterepo1.txt");
            Assert.IsTrue(File.Exists(uniqueContentFile), $"'{uniqueContentFile}' should exist");
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackagesConfigTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public void InstallPackageForPC_PackageSourceMapping_WithWrongSourceOption_Fails(ProjectTemplate projectTemplate)
        {
            // Arrange
            using var simpleTestPathContext = new SimpleTestPathContext();
            var solutionDirectory = simpleTestPathContext.SolutionRoot;

            var opensourceRepositoryPath = Path.Combine(solutionDirectory, "OpensourceRepository");
            Directory.CreateDirectory(opensourceRepositoryPath);

            var privateRepositoryPath = Path.Combine(solutionDirectory, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            var packageName = "Contoso.MVC.ASP";
            var packageVersion = "1.0.0";

            simpleTestPathContext.Settings.AddSource("OpensourceRepository", opensourceRepositoryPath);
            simpleTestPathContext.Settings.AddSource("PrivateRepository", privateRepositoryPath);
            simpleTestPathContext.Settings.AddPackageSourceMapping("PrivateRepository", "Contoso.MVC.*");

            using var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, noAutoRestore: false, addNetStandardFeeds: false, simpleTestPathContext: simpleTestPathContext);
            var nugetConsole = GetConsole(testContext.Project);

            // Act — Install from the opensourceRepo where the package doesn't match the source mapping
            nugetConsole.InstallPackageFromPMC(packageName, packageVersion, opensourceRepositoryPath);
            testContext.SolutionService.SaveAll();

            // Assert
            CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, testContext.Project, packageName, Logger);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackagesConfigTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task UpdatePackageForPC_PackageSourceMapping_WithCorrectSourceOption_UpdatesCorrectPackage(ProjectTemplate projectTemplate)
        {
            // Arrange
            using var simpleTestPathContext = new SimpleTestPathContext();
            var solutionDirectory = simpleTestPathContext.SolutionRoot;

            var opensourceRepositoryPath = Path.Combine(solutionDirectory, "OpensourceRepository");
            Directory.CreateDirectory(opensourceRepositoryPath);

            var privateRepositoryPath = Path.Combine(solutionDirectory, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            var packageName = "Contoso.MVC.ASP";
            var packageVersion1 = "1.0.0";
            var packageVersion2 = "2.0.0";

            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(privateRepositoryPath, packageName, packageVersion1, "Thisisfromprivaterepo1.txt");
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(privateRepositoryPath, packageName, packageVersion2, "Thisisfromprivaterepo2.txt");
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(opensourceRepositoryPath, packageName, packageVersion1, "Thisisfromopensourcerepo1.txt");
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(opensourceRepositoryPath, packageName, packageVersion2, "Thisisfromopensourcerepo2.txt");

            simpleTestPathContext.Settings.AddSource("OpensourceRepository", opensourceRepositoryPath);
            simpleTestPathContext.Settings.AddSource("PrivateRepository", privateRepositoryPath);
            simpleTestPathContext.Settings.AddPackageSourceMapping("PrivateRepository", "Contoso.MVC.*");

            using var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, noAutoRestore: false, addNetStandardFeeds: false, simpleTestPathContext: simpleTestPathContext);
            var nugetConsole = GetConsole(testContext.Project);

            // Act
            nugetConsole.InstallPackageFromPMC(packageName, packageVersion1, privateRepositoryPath);
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion1, Logger);

            nugetConsole.UpdatePackageFromPMC(packageName, packageVersion2, privateRepositoryPath);
            testContext.SolutionService.SaveAll();
            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion2, Logger);

            var packagesDirectory = Path.Combine(solutionDirectory, "packages");
            var uniqueContentFile = Path.Combine(packagesDirectory, packageName + '.' + packageVersion2, "lib", "net45", "Thisisfromprivaterepo2.txt");
            Assert.IsTrue(File.Exists(uniqueContentFile), $"'{uniqueContentFile}' should exist");
        }

        [DataTestMethod]
        [DataRow(ProjectTemplate.ClassLibrary, false)]
        [DataRow(ProjectTemplate.NetStandardClassLib, true)]
        [Timeout(DefaultTimeout)]
        public async Task UpdateAllReinstall_WithPackageReferenceProject_WarnsAsync(ProjectTemplate projectTemplate, bool warns)
        {
            var packageName = "TestPackage";
            var packageVersion1 = "1.0.0";

            using var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, addNetStandardFeeds: true);
            // Arrange
            var solutionService = VisualStudio.Get<SolutionService>();
            testContext.SolutionService.Build();

            await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion1);

            var nugetTestService = GetNuGetTestService();
            var nugetConsole = GetConsole(testContext.Project);

            // Pre-conditions
            nugetConsole.InstallPackageFromPMC(packageName, packageVersion1);
            testContext.SolutionService.Build();
            VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
            VisualStudio.HasNoErrorsInOutputWindows().Should().BeTrue();
            nugetConsole.Clear();

            // Act
            nugetConsole.Execute("Update-Package -Reinstall");

            // Assert
            var expectedMessage = $"The `-Reinstall` parameter does not apply to PackageReference based projects `{Path.GetFileNameWithoutExtension(testContext.Project.UniqueName)}'.";
            nugetConsole.IsMessageFoundInPMC(expectedMessage).Should().Be(warns, because: nugetConsole.GetText());
            VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
            VisualStudio.HasNoErrorsInOutputWindows().Should().BeTrue();

            nugetConsole.Clear();
            solutionService.Save();
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackageReferenceTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task InstallPackageForPR_PackageNamespace_WithMultipleFeedsWithIdenticalPackages_InstallsCorrectPackage(ProjectTemplate projectTemplate)
        {
            // Arrange
            using var simpleTestPathContext = new SimpleTestPathContext();
            var packageName = "Contoso.A";
            var packageVersion1 = "1.0.0";

            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, packageVersion1);

            var privateRepositoryPath = Path.Combine(simpleTestPathContext.SolutionRoot, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(privateRepositoryPath, packageName, packageVersion1);

            simpleTestPathContext.Settings.AddSource("PrivateRepository", privateRepositoryPath);
            simpleTestPathContext.Settings.AddPackageSourceMapping("source", "External.*", "Others.*");
            simpleTestPathContext.Settings.AddPackageSourceMapping("PrivateRepository", "Contoso.*", "Test.*");
            simpleTestPathContext.Settings.AddPackageSourceMapping("nuget", "Microsoft.*", "NetStandard*");

            using var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, noAutoRestore: false, addNetStandardFeeds: true, simpleTestPathContext: simpleTestPathContext);
            var nugetConsole = GetConsole(testContext.Project);

            // Act
            nugetConsole.InstallPackageFromPMC(packageName, packageVersion1);
            testContext.SolutionService.SaveAll();

            // Assert
            var expectedMessage = $"Installed {packageName} {packageVersion1} from {privateRepositoryPath}";
            Assert.IsTrue(nugetConsole.IsMessageFoundInPMC(expectedMessage), expectedMessage);
            VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
            Assert.IsTrue(VisualStudio.HasNoErrorsInOutputWindows());
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackageReferenceTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task UpdatePackageForPR_PackageNamespace_WithMultipleFeedsWithIdenticalPackages_InstallsCorrectPackage(ProjectTemplate projectTemplate)
        {
            // Arrange
            using var simpleTestPathContext = new SimpleTestPathContext();
            var packageName = "Contoso.A";
            var packageVersion1 = "1.0.0";
            var packageVersion2 = "2.0.0";

            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, packageVersion1);
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, packageVersion2);

            var privateRepositoryPath = Path.Combine(simpleTestPathContext.SolutionRoot, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(privateRepositoryPath, packageName, packageVersion1);
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(privateRepositoryPath, packageName, packageVersion2);

            simpleTestPathContext.Settings.AddSource("PrivateRepository", privateRepositoryPath);
            simpleTestPathContext.Settings.AddPackageSourceMapping("source", "External.*", "Others.*");
            simpleTestPathContext.Settings.AddPackageSourceMapping("PrivateRepository", "Contoso.*", "Test.*");
            simpleTestPathContext.Settings.AddPackageSourceMapping("nuget", "Microsoft.*", "NetStandard*");

            using var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, noAutoRestore: false, addNetStandardFeeds: true, simpleTestPathContext: simpleTestPathContext);
            var solutionService = VisualStudio.Get<SolutionService>();
            var nugetConsole = GetConsole(testContext.Project);

            //Pre-conditions
            nugetConsole.InstallPackageFromPMC(packageName, packageVersion1);
            testContext.SolutionService.Build();
            VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
            VisualStudio.HasNoErrorsInOutputWindows().Should().BeTrue();
            nugetConsole.Clear();

            // Act
            nugetConsole.UpdatePackageFromPMC(packageName, packageVersion2);

            // Assert
            var expectedMessage = $"Installed {packageName} {packageVersion2} from {privateRepositoryPath}";
            nugetConsole.IsMessageFoundInPMC(expectedMessage).Should().BeTrue(because: nugetConsole.GetText());
            VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
            VisualStudio.HasNoErrorsInOutputWindows().Should().BeTrue();

            nugetConsole.Clear();
            solutionService.Save();
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task UpdatePackageForPR_PackageIdWithDifferentCase_UpdatesSuccessfully()
        {
            // Arrange
            using var simpleTestPathContext = new SimpleTestPathContext();
            string solutionDirectory = simpleTestPathContext.SolutionRoot;
            var packageName = "Contoso.A";
            var packageVersion1 = "1.0.0";
            var packageVersion2 = "2.0.0";

            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, packageVersion1);
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, packageVersion2);

            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.NetCoreClassLib, Logger, noAutoRestore: false, addNetStandardFeeds: true, simpleTestPathContext: simpleTestPathContext);
            var solutionService = VisualStudio.Get<SolutionService>();
            var nugetConsole = GetConsole(testContext.Project);

            //Pre-conditions
            nugetConsole.InstallPackageFromPMC(packageName.ToLowerInvariant(), packageVersion1);
            var expectedMessage = $"Successfully installed '{packageName} {packageVersion1}' to ";
            nugetConsole.IsMessageFoundInPMC(expectedMessage).Should().BeTrue(because: nugetConsole.GetText());
            testContext.SolutionService.Build();
            VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
            VisualStudio.HasNoErrorsInOutputWindows().Should().BeTrue();
            nugetConsole.Clear();

            // Act
            nugetConsole.UpdatePackageFromPMC(packageName.ToUpperInvariant(), packageVersion2);

            // Assert
            expectedMessage = $"Successfully installed '{packageName} {packageVersion2}' to ";
            nugetConsole.IsMessageFoundInPMC(expectedMessage).Should().BeTrue(because: nugetConsole.GetText());
            VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
            VisualStudio.HasNoErrorsInOutputWindows().Should().BeTrue();

            nugetConsole.Clear();
            solutionService.Save();
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public void InstallPackageFromPMCWithInvalidAbsoluteLocalSource_Fails()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ConsoleApplication, Logger);

            var packageName = "Rules";
            var source = @"c:\temp\data";
            var expectedMessage = $"Unable to find package '{packageName}' at source '{source}'. Source not found.";

            var nugetConsole = GetConsole(testContext.Project);

            nugetConsole.Execute($"Install-Package {packageName} -ProjectName {testContext.Project.Name} -Source {source}");

            Assert.IsTrue(
                nugetConsole.IsMessageFoundInPMC(expectedMessage),
                $"Expected error message was not found in PMC output. Actual output: {nugetConsole.GetText()}");
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public void InstallPackageFromPMCWithValidAbsoluteLocalSource_PackageNotFound_Fails()
        {
            // Uses the solution root as a valid existing directory that contains no packages.
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ConsoleApplication, Logger);

            var packageName = "Rules";
            var source = testContext.SolutionRoot;
            var expectedMessage = $"Unable to find package '{packageName}'";

            var nugetConsole = GetConsole(testContext.Project);

            nugetConsole.Execute($"Install-Package {packageName} -ProjectName {testContext.Project.Name} -Source '{source}'");

            Assert.IsTrue(
                nugetConsole.IsMessageFoundInPMC(expectedMessage),
                $"Expected error message was not found in PMC output. Actual output: {nugetConsole.GetText()}");
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public void InstallPackageFromPMCWithInvalidRelativeLocalSource_Fails()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ConsoleApplication, Logger);

            var packageName = "Rules";
            var source = @"..\invalid_folder";
            var expectedMessage = $"Unable to find package '{packageName}' at source '{source}'. Source not found.";

            var nugetConsole = GetConsole(testContext.Project);

            nugetConsole.Execute($"Install-Package {packageName} -ProjectName {testContext.Project.Name} -Source {source}");

            Assert.IsTrue(
                nugetConsole.IsMessageFoundInPMC(expectedMessage),
                $"Expected error message was not found in PMC output. Actual output: {nugetConsole.GetText()}");
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public void InstallPackageFromPMCWithValidRelativeLocalSource_PackageNotFound_Fails()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ConsoleApplication, Logger);

            var packageName = "Rules";
            var source = @"..\";
            var expectedMessage = $"Unable to find package '{packageName}'";

            var nugetConsole = GetConsole(testContext.Project);

            nugetConsole.Execute($"Install-Package {packageName} -ProjectName {testContext.Project.Name} -Source {source}");

            Assert.IsTrue(
                nugetConsole.IsMessageFoundInPMC(expectedMessage),
                $"Expected error message was not found in PMC output. Actual output: {nugetConsole.GetText()}");
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public void InstallPackageFromPMCWithInvalidHttpSource_Fails()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ConsoleApplication, Logger);

            var packageName = "Rules";
            var source = "http://example.com";
            var expectedMessage = $"Unable to find package '{packageName}' at source '{source}'.";

            var nugetConsole = GetConsole(testContext.Project);

            nugetConsole.Execute($"Install-Package {packageName} -ProjectName {testContext.Project.Name} -Source {source}");

            Assert.IsTrue(
                nugetConsole.IsMessageFoundInPMC(expectedMessage),
                $"Expected error message was not found in PMC output. Actual output: {nugetConsole.GetText()}");
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public void InstallPackageFromPMCWithIncompleteHttpSource_Fails()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ConsoleApplication, Logger);

            var packageName = "Rules";
            var source = "http://";
            var expectedMessage = $"Unable to find package '{packageName}' at source '{source}'. Source not found.";

            var nugetConsole = GetConsole(testContext.Project);

            nugetConsole.Execute($"Install-Package {packageName} -ProjectName {testContext.Project.Name} -Source {source}");

            Assert.IsTrue(
                nugetConsole.IsMessageFoundInPMC(expectedMessage),
                $"Expected error message was not found in PMC output. Actual output: {nugetConsole.GetText()}");
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public void InstallPackageFromPMCWithInvalidKnownSource_Fails()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ConsoleApplication, Logger);

            var packageName = "Rules";
            var source = "nuget.random";
            var expectedMessage = $"Unable to find package '{packageName}' at source '{source}'. Source not found.";

            var nugetConsole = GetConsole(testContext.Project);

            nugetConsole.Execute($"Install-Package {packageName} -ProjectName {testContext.Project.Name} -Source {source}");

            Assert.IsTrue(
                nugetConsole.IsMessageFoundInPMC(expectedMessage),
                $"Expected error message was not found in PMC output. Actual output: {nugetConsole.GetText()}");
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public void InstallPackageFromPMCWithInvalidSourceFormat_Fails()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ConsoleApplication, Logger);

            var source = "d:package";
            var expectedMessage = $"Unsupported type of source '{source}'. Please provide an HTTP or local source.";

            var nugetConsole = GetConsole(testContext.Project);

            nugetConsole.Execute($"Install-Package jQuery -Source {source}");

            Assert.IsTrue(
                nugetConsole.IsMessageFoundInPMC(expectedMessage),
                $"Expected error message was not found in PMC output. Actual output: {nugetConsole.GetText()}");
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task InstallPackageFromPMCWithWhatIf_DoesNotInstallPackageAsync()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ConsoleApplication, Logger);

            var packageName = "TestPackage";
            var packageVersion = "1.0.0";
            await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion);

            var nugetConsole = GetConsole(testContext.Project);

            nugetConsole.Execute($"Install-Package {packageName} -ProjectName {testContext.Project.Name} -Version {packageVersion} -WhatIf");

            CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, testContext.Project, packageName, Logger);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public void InstallPackageFromPMCWithFtpSource_Fails()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ConsoleApplication, Logger);

            var packageName = "Rules";
            var source = "ftp://Rules";
            var expectedMessage = $"Unsupported type of source '{source}'. Please provide an HTTP or local source.";

            var nugetConsole = GetConsole(testContext.Project);

            nugetConsole.Execute($"Install-Package {packageName} -ProjectName {testContext.Project.Name} -Source {source}");

            Assert.IsTrue(
                nugetConsole.IsMessageFoundInPMC(expectedMessage),
                $"Expected error message was not found in PMC output. Actual output: {nugetConsole.GetText()}");
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public void UninstallPackageFromPMCNotInstalled_Fails()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ClassLibrary, Logger);

            var packageName = "TestPackage";
            var expectedMessage = $"Package '{packageName}' to be uninstalled could not be found in project '{testContext.Project.Name}'";

            var nugetConsole = GetConsole(testContext.Project);

            nugetConsole.Execute($"Uninstall-Package {packageName}");

            Assert.IsTrue(
                nugetConsole.IsMessageFoundInPMC(expectedMessage),
                $"Expected error message was not found in PMC output. Actual output: {nugetConsole.GetText()}");
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public void UpdatePackageFromPMCNotInstalled_Fails()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ClassLibrary, Logger);

            var packageName = "TestPackage";
            var expectedMessage = $"'{packageName}' was not installed in any project. Update failed.";

            var nugetConsole = GetConsole(testContext.Project);

            nugetConsole.Execute($"Update-Package {packageName}");

            Assert.IsTrue(
                nugetConsole.IsMessageFoundInPMC(expectedMessage),
                $"Expected error message was not found in PMC output. Actual output: {nugetConsole.GetText()}");
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public void GetPackageFromPMCWithInvalidSource_Fails()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ConsoleApplication, Logger);

            var source = "d:package";
            var expectedMessage = $"Unsupported type of source '{source}'. Please provide an HTTP or local source.";

            var nugetConsole = GetConsole(testContext.Project);

            nugetConsole.Execute($"Get-Package -ListAvailable -Source {source}");

            Assert.IsTrue(
                nugetConsole.IsMessageFoundInPMC(expectedMessage),
                $"Expected error message was not found in PMC output. Actual output: {nugetConsole.GetText()}");
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public void GetPackageFromPMCWithInvalidProjectName_Fails()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ConsoleApplication, Logger);

            var invalidProjectName = "invalidname";
            var expectedMessage = $"Project '{invalidProjectName}' is not found.";

            var nugetConsole = GetConsole(testContext.Project);

            nugetConsole.Execute($"Get-Package -ProjectName {invalidProjectName}");

            Assert.IsTrue(
                nugetConsole.IsMessageFoundInPMC(expectedMessage),
                $"Expected error message was not found in PMC output. Actual output: {nugetConsole.GetText()}");
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task InstallPackageFromPMCWithWhatIfDowngrade_DoesNotDowngradeAsync()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ConsoleApplication, Logger);

            var packageName = "TestPackage";
            var packageVersion1 = "1.0.0";
            var packageVersion2 = "2.0.0";
            await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion1);
            await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion2);

            var nugetConsole = GetConsole(testContext.Project);

            nugetConsole.InstallPackageFromPMC(packageName, packageVersion2);
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion2, Logger);

            nugetConsole.Execute($"Install-Package {packageName} -ProjectName {testContext.Project.Name} -Version {packageVersion1} -Source {testContext.PackageSource} -WhatIf");

            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion2, Logger);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task UninstallPackageFromPMCWithWhatIf_DoesNotUninstallAsync()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ClassLibrary, Logger);

            var packageName = "TestPackage";
            var packageVersion = "1.0.0";
            await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion);

            var nugetConsole = GetConsole(testContext.Project);

            nugetConsole.InstallPackageFromPMC(packageName, packageVersion);
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion, Logger);

            nugetConsole.Execute($"Uninstall-Package {packageName} -ProjectName {testContext.Project.Name} -WhatIf");

            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion, Logger);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task UpdatePackageFromPMCWithInvalidSource_FailsAsync()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ConsoleApplication, Logger);

            var packageName = "TestPackage";
            var packageVersion = "1.0.0";
            await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion);

            var nugetConsole = GetConsole(testContext.Project);

            nugetConsole.InstallPackageFromPMC(packageName, packageVersion);

            var source = "d:package";
            var expectedMessage = $"Unsupported type of source '{source}'. Please provide an HTTP or local source.";

            nugetConsole.Execute($"Update-Package {packageName} -Source {source}");

            Assert.IsTrue(
                nugetConsole.IsMessageFoundInPMC(expectedMessage),
                $"Expected error message was not found in PMC output. Actual output: {nugetConsole.GetText()}");
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task InstallPackageFromPMCWithoutVersion_InstallsLatestStableAsync()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ConsoleApplication, Logger);

            var packageName = "TestPackage";
            var olderVersion = "0.4.0";
            var latestStableVersion = "0.5.0";
            var prereleaseVersion = "0.6.0-beta";
            await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, olderVersion);
            await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, latestStableVersion);
            await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, prereleaseVersion);

            var nugetConsole = GetConsole(testContext.Project);

            nugetConsole.Execute($"Install-Package {packageName} -ProjectName {testContext.Project.Name} -Source {testContext.PackageSource}");

            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, latestStableVersion, Logger);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task InstallPackageFromPMCWithPrerelease_InstallsLatestPrereleaseAsync()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ConsoleApplication, Logger);

            var packageName = "TestPackage";
            var stableVersion = "0.5.0";
            var latestPrereleaseVersion = "0.6.0-beta";
            await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, stableVersion);
            await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, latestPrereleaseVersion);

            var nugetConsole = GetConsole(testContext.Project);

            nugetConsole.Execute($"Install-Package {packageName} -ProjectName {testContext.Project.Name} -Source {testContext.PackageSource} -IncludePrerelease");

            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, latestPrereleaseVersion, Logger);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task InstallPackageFromPMCWithFileUriSource_SucceedsAsync()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ConsoleApplication, Logger);

            var packageName = "TestPackage";
            var packageVersion = "2.0.0";
            await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion);

            var fileUri = "file:///" + testContext.PackageSource.Replace("\\", "/");

            var nugetConsole = GetConsole(testContext.Project);

            nugetConsole.Execute($"Install-Package {packageName} -Version {packageVersion} -Source {fileUri}");

            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion, Logger);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task GetPackageFromPMC_ListsInstalledPackagesAsync()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ConsoleApplication, Logger);

            var packageName1 = "TestPackageA";
            var packageVersion1 = "1.0.0";
            var packageName2 = "TestPackageB";
            var packageVersion2 = "2.0.0";
            await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName1, packageVersion1);
            await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName2, packageVersion2);

            var nugetConsole = GetConsole(testContext.Project);

            // Assert no packages are installed initially
            nugetConsole.Clear();
            nugetConsole.Execute("Get-Package");
            string emptyText = nugetConsole.GetText();
            ParseGetPackageTableOutput(emptyText).Should().BeEmpty(because: emptyText);

            nugetConsole.InstallPackageFromPMC(packageName1, packageVersion1);
            nugetConsole.InstallPackageFromPMC(packageName2, packageVersion2);

            nugetConsole.Clear();
            nugetConsole.Execute("Get-Package");

            string pmcText = nugetConsole.GetText();
            var packages = ParseGetPackageTableOutput(pmcText);
            packages.Select(p => p.Id).Should().Contain(packageName1, because: pmcText);
            packages.Select(p => p.Id).Should().Contain(packageName2, because: pmcText);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task GetPackageFromPMCForProject_ReturnsCorrectPackagesAsync()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ConsoleApplication, Logger);

            var packageName = "TestPackage";
            var packageVersion = "1.5.0";
            await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion);

            var nugetConsole = GetConsole(testContext.Project);

            nugetConsole.InstallPackageFromPMC(packageName, packageVersion);

            nugetConsole.Clear();
            nugetConsole.Execute($"Get-Package -ProjectName {testContext.Project.Name}");

            string pmcText = nugetConsole.GetText();
            var packages = ParseGetPackageTableOutput(pmcText);
            packages.Select(p => p.Id).Should().Contain(packageName, because: pmcText);
            packages.Select(p => p.Versions).Should().Contain(v => v.Contains(packageVersion), because: pmcText);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task GetPackageFromPMCForProject_ReturnsEmptyForOtherProjectAsync()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ConsoleApplication, Logger);

            var project2 = testContext.SolutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, CommonUtility.DefaultTargetFramework, "TestProject2");
            testContext.SolutionService.SaveAll();

            var packageName = "TestPackage";
            var packageVersion = "1.0.0";
            await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion);

            var nugetConsole = GetConsole(testContext.Project);

            nugetConsole.InstallPackageFromPMC(packageName, packageVersion);

            // Get-Package for the other project which has no packages should return empty
            nugetConsole.Clear();
            nugetConsole.Execute($"Get-Package -ProjectName {project2.Name}");

            string pmcText = nugetConsole.GetText();
            var packages = ParseGetPackageTableOutput(pmcText);
            packages.Should().BeEmpty(because: pmcText);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task GetPackageFromPMCWithFilter_ReturnsMatchingPackageAsync()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ClassLibrary, Logger);

            var packageName = "TestFilterPackage";
            var packageVersion = "1.0.0";
            await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion);

            var nugetConsole = GetConsole(testContext.Project);

            nugetConsole.InstallPackageFromPMC(packageName, packageVersion);

            nugetConsole.Clear();
            nugetConsole.Execute("Get-Package 'TestFilter'");

            string pmcText = nugetConsole.GetText();
            var packages = ParseGetPackageTableOutput(pmcText);
            packages.Should().ContainSingle(because: pmcText)
                .Which.Id.Should().Be(packageName, because: pmcText);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task UpdatePackageFromPMCWithWhatIf_DoesNotUpdateAsync()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ClassLibrary, Logger);

            var packageName = "TestPackage";
            var packageVersion1 = "1.0.0";
            var packageVersion2 = "2.0.0";
            await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion1);
            await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion2);

            var nugetConsole = GetConsole(testContext.Project);

            nugetConsole.InstallPackageFromPMC(packageName, packageVersion1);
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion1, Logger);

            nugetConsole.Execute($"Update-Package {packageName} -Source {testContext.PackageSource} -WhatIf");

            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion1, Logger);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task UpdatePackageFromPMCToSpecificVersion_UpdatesCorrectlyAsync()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ClassLibrary, Logger);

            var packageName = "TestPackage";
            var packageVersion1 = "1.0.0";
            var packageVersion2 = "2.0.0";
            await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion1);
            await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion2);

            var nugetConsole = GetConsole(testContext.Project);

            nugetConsole.InstallPackageFromPMC(packageName, packageVersion1);
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion1, Logger);

            nugetConsole.Execute($"Update-Package {packageName} -Version {packageVersion2} -Source {testContext.PackageSource}");

            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion2, Logger);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task InstallPackageFromPMCToMultipleProjects_SucceedsAsync()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ConsoleApplication, Logger);

            var project2 = testContext.SolutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, CommonUtility.DefaultTargetFramework, "TestProject2");
            testContext.SolutionService.SaveAll();

            var packageName = "TestPackage";
            var packageVersion = "1.0.0";
            await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion);

            var nugetConsole = GetConsole(testContext.Project);

            // Pipe all projects to Install-Package to replicate the PS1 piping behavior
            nugetConsole.Execute($"Get-Project -All | Install-Package {packageName} -Version {packageVersion} -Source {testContext.PackageSource}");

            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion, Logger);
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project2, packageName, packageVersion, Logger);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task GetPackageFromPMCWithListAvailable_ReturnsAvailablePackagesAsync()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ConsoleApplication, Logger);

            var packages = new[]
            {
                ("TestPackageA", "1.0.0"),
                ("TestPackageB", "1.0.0"),
                ("TestPackageC", "1.0.0"),
                ("TestPackageD", "1.0.0"),
                ("TestPackageE", "1.0.0"),
            };

            foreach (var (name, version) in packages)
            {
                await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, name, version);
            }

            var nugetConsole = GetConsole(testContext.Project);
            nugetConsole.Clear();

            nugetConsole.Execute($"Get-Package -ListAvailable -Source {testContext.PackageSource}");

            string pmcText = nugetConsole.GetText();
            var availablePackages = ParseGetPackageTableOutput(pmcText);
            availablePackages.Should().HaveCount(packages.Length, because: pmcText);
            foreach (var (name, _) in packages)
            {
                availablePackages.Select(p => p.Id).Should().Contain(name, because: pmcText);
            }
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public void GetProject_CanAccessProjectName()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.NetCoreConsoleApp, Logger);

            var nugetConsole = GetConsole(testContext.Project);
            nugetConsole.Clear();

            nugetConsole.Execute("$proj = Get-Project; $proj.Name");

            string pmcText = nugetConsole.GetText();
            pmcText.Should().Contain(testContext.Project.Name, because: pmcText);
            pmcText.Should().NotContain("FullyQualifiedErrorId", because: pmcText);
        }

        public static IEnumerable<object[]> GetNetCoreTemplates()
        {
            yield return new object[] { ProjectTemplate.NetCoreConsoleApp };
        }

        public static IEnumerable<object[]> GetPackageReferenceTemplates()
        {
            yield return new object[] { ProjectTemplate.NetStandardClassLib };
        }

        public static IEnumerable<object[]> GetPackagesConfigTemplates()
        {
            yield return new object[] { ProjectTemplate.ClassLibrary };
        }

        public static IEnumerable<object[]> GetMauiTemplates()
        {
            yield return new object[] { ProjectTemplate.MauiClassLibrary };
        }

        /// <summary>
        /// Holds a single row from the Get-Package PMC table output.
        /// </summary>
        private sealed class PmcPackageEntry
        {
            public string Id { get; }
            public string Versions { get; }
            public string ProjectName { get; }

            public PmcPackageEntry(string id, string versions, string projectName)
            {
                Id = id;
                Versions = versions;
                ProjectName = projectName;
            }
        }

        /// <summary>
        /// Parses the tabular output of a Get-Package PMC command into a list of package entries.
        /// The expected output format is a fixed-width table with a separator row of dashes:
        /// <code>
        /// Id                 Versions    ProjectName
        /// --                 --------    -----------
        /// TestPackageA       {1.0.0}     MyProject
        /// TestPackageB       {2.0.0}     MyProject
        /// </code>
        /// Parsing begins after the separator row and stops at the first empty line.
        /// Column boundaries are inferred from the positions of the dash groups in the separator row.
        /// </summary>
        private static List<PmcPackageEntry> ParseGetPackageTableOutput(string pmcText)
        {
            var entries = new List<PmcPackageEntry>();
            bool pastSeparator = false;
            int[]? columnStarts = null;

            foreach (string rawLine in pmcText.Split('\n'))
            {
                string line = rawLine.TrimEnd('\r');

                if (!pastSeparator)
                {
                    // The separator row contains only dashes and spaces (e.g. "--  --------  -----------")
                    string stripped = line.Replace("-", "").Replace(" ", "");
                    if (stripped.Length == 0 && line.Contains("--"))
                    {
                        columnStarts = FindColumnStarts(line);
                        pastSeparator = true;
                    }
                    continue;
                }

                if (line.Trim().Length == 0)
                {
                    break; // End of table
                }

                if (columnStarts != null)
                {
                    string id = ExtractColumn(line, columnStarts, 0).Trim();
                    string versions = columnStarts.Length >= 2 ? ExtractColumn(line, columnStarts, 1).Trim() : string.Empty;
                    string projectName = columnStarts.Length >= 3 ? ExtractColumn(line, columnStarts, 2).Trim() : string.Empty;

                    if (id.Length > 0)
                    {
                        entries.Add(new PmcPackageEntry(id, versions, projectName));
                    }
                }
            }

            return entries;
        }

        /// <summary>
        /// Returns the start index of each group of dashes in the separator row,
        /// giving the column start positions for the fixed-width table.
        /// </summary>
        private static int[] FindColumnStarts(string separatorLine)
        {
            var starts = new List<int>();
            bool inDashes = false;

            for (int i = 0; i < separatorLine.Length; i++)
            {
                if (separatorLine[i] == '-' && !inDashes)
                {
                    starts.Add(i);
                    inDashes = true;
                }
                else if (separatorLine[i] != '-')
                {
                    inDashes = false;
                }
            }

            return starts.ToArray();
        }

        /// <summary>
        /// Extracts the text for a specific column from a data row, using the column-start
        /// positions obtained from the separator row.
        /// </summary>
        private static string ExtractColumn(string line, int[] columnStarts, int columnIndex)
        {
            int start = columnStarts[columnIndex];
            if (start >= line.Length)
            {
                return string.Empty;
            }

            if (columnIndex + 1 < columnStarts.Length)
            {
                int end = columnStarts[columnIndex + 1];
                if (end > line.Length)
                {
                    end = line.Length;
                }
                return line.Substring(start, end - start);
            }

            return line.Substring(start);
        }
    }
}
