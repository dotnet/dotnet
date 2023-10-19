// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Test.Apex.VisualStudio.Solution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet.Configuration;
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
            // Arrange
            EnsureVisualStudioHost();

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
            // Arrange
            EnsureVisualStudioHost();

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
            // Arrange
            EnsureVisualStudioHost();

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
            // Arrange
            EnsureVisualStudioHost();

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
            // Arrange
            EnsureVisualStudioHost();

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
            // Arrange
            EnsureVisualStudioHost();
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
            // Arrange
            EnsureVisualStudioHost();
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
            // Arrange
            EnsureVisualStudioHost();

            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, addNetStandardFeeds: true))
            {
                var project2 = testContext.SolutionService.AddProject(ProjectLanguage.CSharp, projectTemplate, ProjectTargetFramework.V46, "TestProject2");
                project2.Build();
                var project3 = testContext.SolutionService.AddProject(ProjectLanguage.CSharp, projectTemplate, ProjectTargetFramework.V46, "TestProject3");
                project3.Build();
                var projectX = testContext.SolutionService.AddProject(ProjectLanguage.CSharp, projectTemplate, ProjectTargetFramework.V46, "TestProjectX");
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
            EnsureVisualStudioHost();
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
            EnsureVisualStudioHost();

            using var simpleTestPathContext = new SimpleTestPathContext();
            string solutionDirectory = simpleTestPathContext.SolutionRoot;
            var privateRepositoryPath = Path.Combine(solutionDirectory, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            var packageName = "Contoso.A";
            var packageVersion = "1.0.0";

            await CommonUtility.CreatePackageInSourceAsync(privateRepositoryPath, packageName, packageVersion);

            //Create nuget.config with Package source mapping filtering rules.
            CommonUtility.CreateConfigurationFile(Path.Combine(solutionDirectory, "NuGet.config"), $@"<?xml version=""1.0"" encoding=""utf-8""?>
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

            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, noAutoRestore: false, addNetStandardFeeds: false, simpleTestPathContext: simpleTestPathContext))
            {
                var nugetConsole = GetConsole(testContext.Project);

                // Act
                nugetConsole.InstallPackageFromPMC(packageName, packageVersion);

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
            EnsureVisualStudioHost();

            using var simpleTestPathContext = new SimpleTestPathContext();
            string solutionDirectory = simpleTestPathContext.SolutionRoot;
            var privateRepositoryPath = Path.Combine(solutionDirectory, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            var packageName = "Contoso.A";
            var packageVersion1 = "1.0.0";
            var packageVersion2 = "2.0.0";

            await CommonUtility.CreatePackageInSourceAsync(privateRepositoryPath, packageName, packageVersion1);
            await CommonUtility.CreatePackageInSourceAsync(privateRepositoryPath, packageName, packageVersion2);

            //Create nuget.config with Package source mapping filtering rules.
            CommonUtility.CreateConfigurationFile(Path.Combine(solutionDirectory, "NuGet.config"), $@"<?xml version=""1.0"" encoding=""utf-8""?>
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

            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, noAutoRestore: false, addNetStandardFeeds: false, simpleTestPathContext: simpleTestPathContext))
            {
                var nugetConsole = GetConsole(testContext.Project);

                // Act
                nugetConsole.InstallPackageFromPMC(packageName, packageVersion1);
                nugetConsole.UpdatePackageFromPMC(packageName, packageVersion2);

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
            EnsureVisualStudioHost();

            using var simpleTestPathContext = new SimpleTestPathContext();
            string solutionDirectory = simpleTestPathContext.SolutionRoot;
            var packageName = "Contoso.A";
            var packageVersion1 = "1.0.0";
            var packageVersion2 = "2.0.0";

            var opensourceRepositoryPath = Path.Combine(solutionDirectory, "OpensourceRepository");
            Directory.CreateDirectory(opensourceRepositoryPath);

            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(opensourceRepositoryPath, packageName, packageVersion1, "Thisisfromopensourcerepo1.txt");
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(opensourceRepositoryPath, packageName, packageVersion2, "Thisisfromopensourcerepo2.txt");

            var privateRepositoryPath = Path.Combine(solutionDirectory, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(privateRepositoryPath, packageName, packageVersion1, "Thisisfromprivaterepo1.txt");
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(privateRepositoryPath, packageName, packageVersion2, "Thisisfromprivaterepo2.txt");

            //Create nuget.config with Package source mapping filtering rules.
            CommonUtility.CreateConfigurationFile(Path.Combine(solutionDirectory, "NuGet.config"), $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""ExternalRepository"" value=""{opensourceRepositoryPath}"" />
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

            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, noAutoRestore: false, addNetStandardFeeds: false, simpleTestPathContext: simpleTestPathContext))
            {
                var nugetConsole = GetConsole(testContext.Project);

                // Act
                nugetConsole.InstallPackageFromPMC(packageName, packageVersion1);

                // Assert
                CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion1, Logger);

                var packagesDirectory = Path.Combine(solutionDirectory, "packages");
                var uniqueContentFile = Path.Combine(packagesDirectory, packageName + '.' + packageVersion1, "lib", "net45", "Thisisfromprivaterepo1.txt");
                // Make sure name squatting package not restored from  opensource repository.
                Assert.IsTrue(File.Exists(uniqueContentFile));
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackagesConfigTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task UpdatePackageForPC_PackageSourceMapping_WithMultipleFeedsWithIdenticalPackages_UpdatesCorrectPackage(ProjectTemplate projectTemplate)
        {
            // Arrange
            EnsureVisualStudioHost();

            using var simpleTestPathContext = new SimpleTestPathContext();
            string solutionDirectory = simpleTestPathContext.SolutionRoot;
            var packageName = "Contoso.A";
            var packageVersion1 = "1.0.0";
            var packageVersion2 = "2.0.0";

            var opensourceRepositoryPath = Path.Combine(solutionDirectory, "OpensourceRepository");
            Directory.CreateDirectory(opensourceRepositoryPath);

            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(opensourceRepositoryPath, packageName, packageVersion1, "Thisisfromopensourcerepo1.txt");
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(opensourceRepositoryPath, packageName, packageVersion2, "Thisisfromopensourcerepo2.txt");

            var privateRepositoryPath = Path.Combine(solutionDirectory, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(privateRepositoryPath, packageName, packageVersion1, "Thisisfromprivaterepo1.txt");
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(privateRepositoryPath, packageName, packageVersion2, "Thisisfromprivaterepo2.txt");

            //Create nuget.config with Package source mapping filtering rules.
            CommonUtility.CreateConfigurationFile(Path.Combine(solutionDirectory, "NuGet.config"), $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""ExternalRepository"" value=""{opensourceRepositoryPath}"" />
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

            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, noAutoRestore: false, addNetStandardFeeds: false, simpleTestPathContext: simpleTestPathContext))
            {
                var nugetConsole = GetConsole(testContext.Project);

                // Act
                nugetConsole.InstallPackageFromPMC(packageName, packageVersion1);
                nugetConsole.UpdatePackageFromPMC(packageName, packageVersion2);

                // Assert
                CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion2, Logger);

                var packagesDirectory = Path.Combine(solutionDirectory, "packages");
                var uniqueContentFile = Path.Combine(packagesDirectory, packageName + '.' + packageVersion2, "lib", "net45", "Thisisfromprivaterepo2.txt");
                // Make sure name squatting package not restored from  opensource repository.
                Assert.IsTrue(File.Exists(uniqueContentFile));
            }
        }

        [Ignore("https://github.com/NuGet/Home/issues/12899")]
        [DataTestMethod]
        [DataRow(ProjectTemplate.ClassLibrary, false)]
        [DataRow(ProjectTemplate.NetStandardClassLib, true)]
        [Timeout(DefaultTimeout)]
        public async Task UpdateAllReinstall_WithPackageReferenceProject_WarnsAsync(ProjectTemplate projectTemplate, bool warns)
        {
            EnsureVisualStudioHost();
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
            var expectedMessage = $"The `-Reinstall` parameter does not apply to PackageReference based projects `{Path.GetFileNameWithoutExtension(testContext.Project.UniqueName)}`.";
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
            EnsureVisualStudioHost();

            using var simpleTestPathContext = new SimpleTestPathContext();
            string solutionDirectory = simpleTestPathContext.SolutionRoot;
            var packageName = "Contoso.A";
            var packageVersion1 = "1.0.0";

            var opensourceRepositoryPath = Path.Combine(solutionDirectory, "OpensourceRepository");
            Directory.CreateDirectory(opensourceRepositoryPath);

            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(opensourceRepositoryPath, packageName, packageVersion1);

            var privateRepositoryPath = Path.Combine(solutionDirectory, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(privateRepositoryPath, packageName, packageVersion1);

            //Create nuget.config with Package namespace filtering rules.
            CommonUtility.CreateConfigurationFile(Path.Combine(solutionDirectory, "NuGet.Config"), $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""ExternalRepository"" value=""{opensourceRepositoryPath}"" />
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
        <packageSource key=""nuget"">
            <package pattern=""Microsoft.*"" />
            <package pattern=""NetStandard*"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");

            using var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, noAutoRestore: false, addNetStandardFeeds: true, simpleTestPathContext: simpleTestPathContext);
            var nugetConsole = GetConsole(testContext.Project);

            // Act
            nugetConsole.InstallPackageFromPMC(packageName, packageVersion1);

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
            EnsureVisualStudioHost();

            using var simpleTestPathContext = new SimpleTestPathContext();
            string solutionDirectory = simpleTestPathContext.SolutionRoot;
            var packageName = "Contoso.A";
            var packageVersion1 = "1.0.0";
            var packageVersion2 = "2.0.0";

            var opensourceRepositoryPath = Path.Combine(solutionDirectory, "OpensourceRepository");
            Directory.CreateDirectory(opensourceRepositoryPath);

            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(opensourceRepositoryPath, packageName, packageVersion1);
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(opensourceRepositoryPath, packageName, packageVersion2);

            var privateRepositoryPath = Path.Combine(solutionDirectory, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(privateRepositoryPath, packageName, packageVersion1);
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(privateRepositoryPath, packageName, packageVersion2);

            //Create nuget.config with Package namespace filtering rules.
            CommonUtility.CreateConfigurationFile(Path.Combine(solutionDirectory, "NuGet.config"), $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""ExternalRepository"" value=""{opensourceRepositoryPath}"" />
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
        <packageSource key=""nuget"">
            <package pattern=""Microsoft.*"" />
            <package pattern=""NetStandard*"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");

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

        [Ignore("https://github.com/NuGet/Home/issues/12931")]
        [DataTestMethod]
        [DynamicData(nameof(GetNetCoreTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task VerifyCacheFileInsideObjFolder(ProjectTemplate projectTemplate)
        {
            // Arrange
            EnsureVisualStudioHost();

            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, addNetStandardFeeds: true))
            {
                var packageName = "VerifyCacheFilePackage";
                var packageVersion = "1.0.0";
                await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion);
                var nugetConsole = GetConsole(testContext.Project);

                //Act
                nugetConsole.InstallPackageFromPMC(packageName, packageVersion);
                FileInfo CacheFilePath = CommonUtility.GetCacheFilePath(testContext.Project.FullPath);

                // Assert
                testContext.SolutionService.Build();
                testContext.NuGetApexTestService.WaitForAutoRestore();
                CommonUtility.WaitForFileExists(CacheFilePath);

                testContext.Project.Rebuild();
                CommonUtility.WaitForFileExists(CacheFilePath);

                testContext.Project.Clean();
                CommonUtility.WaitForFileNotExists(CacheFilePath);
            }
        }

        [DataTestMethod]
        //[DataRow(ProjectTemplate.ClassLibrary, "PackageA", "1.0.0", "2.0.0", "PackageB", "1.0.1", "2.0.1")] [Ignore("https://github.com/NuGet/Home/issues/12932")]
        [DataRow(ProjectTemplate.NetStandardClassLib, "PackageC", "1.0.0", "2.0.0", "PackageD", "1.1.0", "2.2.0")]
        [Timeout(DefaultTimeout)]
        public async Task UpdateAllPackagesInPMC(ProjectTemplate projectTemplate, string packageName1, string packageVersion1, string packageVersion2, string packageName2, string packageVersion3, string packageVersion4)
        {
            EnsureVisualStudioHost();
            using (var simpleTestPathContext = new SimpleTestPathContext())
            {
                // Arrange
                await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName1, packageVersion1);
                await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName1, packageVersion2);
                await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName2, packageVersion3);
                await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName2, packageVersion4);

                using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, addNetStandardFeeds: true, simpleTestPathContext: simpleTestPathContext))
                {
                    var solutionService = VisualStudio.Get<SolutionService>();
                    var nugetConsole = GetConsole(testContext.Project);

                    // Act
                    nugetConsole.InstallPackageFromPMC(packageName1, packageVersion1);
                    nugetConsole.InstallPackageFromPMC(packageName2, packageVersion3);
                    testContext.SolutionService.Build();
                    testContext.NuGetApexTestService.WaitForAutoRestore();

                    nugetConsole.Execute("update-package");
                    testContext.SolutionService.Build();
                    testContext.NuGetApexTestService.WaitForAutoRestore();

                    // Assert
                    if (projectTemplate.ToString().Equals("ClassLibrary"))
                    {
                        CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName1, packageVersion2, Logger);
                        CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName2, packageVersion4, Logger);
                    }
                    else
                    {
                        CommonUtility.AssertPackageReferenceExists(VisualStudio, testContext.Project, packageName1, packageVersion2, Logger);
                        CommonUtility.AssertPackageReferenceExists(VisualStudio, testContext.Project, packageName2, packageVersion4, Logger);
                    }
                    VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
                    Assert.IsTrue(VisualStudio.HasNoErrorsInOutputWindows());
                }
            }
        }

        [Ignore("https://github.com/NuGet/Home/issues/12930")]
        [DataTestMethod]
        [DynamicData(nameof(GetIOSTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task InstallPackageForIOSProjectInPMC(ProjectTemplate projectTemplate)
        {
            EnsureVisualStudioHost();
            using (var simpleTestPathContext = new SimpleTestPathContext())
            {
                // Arrange
                var packageName = "IOSTestPackage";
                var v100 = "1.0.0";
                await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, v100);

                using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, simpleTestPathContext: simpleTestPathContext))
                {
                    VisualStudio.AssertNoErrors();
                    var solutionService = VisualStudio.Get<SolutionService>();
                    testContext.SolutionService.Build();

                    // Act
                    var nugetConsole = GetConsole(testContext.Project);

                    nugetConsole.InstallPackageFromPMC(packageName, v100);
                    testContext.SolutionService.Build();
                    testContext.NuGetApexTestService.WaitForAutoRestore();

                    // Assert
                    VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
                    CommonUtility.AssertPackageInAssetsFile(VisualStudio, testContext.Project, packageName, v100, Logger);
                    Assert.IsTrue(VisualStudio.HasNoErrorsInOutputWindows());
                }
            }
        }

        [Ignore("https://github.com/NuGet/Home/issues/12930")]
        [DataTestMethod]
        [DynamicData(nameof(GetIOSTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task UpdatePackageForIOSProjectInPMC(ProjectTemplate projectTemplate)
        {
            EnsureVisualStudioHost();
            using (var simpleTestPathContext = new SimpleTestPathContext())
            {
                // Arrange
                var packageName = "IOSTestPackage";
                var v100 = "1.0.0";
                var v200 = "2.0.0";

                await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, v100);
                await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, v200);

                using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, simpleTestPathContext: simpleTestPathContext))
                {
                    VisualStudio.AssertNoErrors();
                    var solutionService = VisualStudio.Get<SolutionService>();
                    testContext.SolutionService.Build();

                    // Act
                    var nugetConsole = GetConsole(testContext.Project);

                    nugetConsole.InstallPackageFromPMC(packageName, v100);
                    testContext.SolutionService.Build();
                    testContext.NuGetApexTestService.WaitForAutoRestore();

                    nugetConsole.UpdatePackageFromPMC(packageName, v200);
                    testContext.SolutionService.Build();
                    testContext.NuGetApexTestService.WaitForAutoRestore();

                    // Assert
                    VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
                    CommonUtility.AssertPackageInAssetsFile(VisualStudio, testContext.Project, packageName, v200, Logger);
                    Assert.IsTrue(VisualStudio.HasNoErrorsInOutputWindows());
                }
            }
        }

        [Ignore("https://github.com/NuGet/Home/issues/12930")]
        [DataTestMethod]
        [DynamicData(nameof(GetIOSTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task UninstallPackageForIOSProjectInPMC(ProjectTemplate projectTemplate)
        {
            EnsureVisualStudioHost();
            using (var simpleTestPathContext = new SimpleTestPathContext())
            {
                //Arrange
                var PackageName = "IOSTestPackage";
                var v100 = "1.0.0";

                await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, PackageName, v100);

                using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, simpleTestPathContext: simpleTestPathContext))
                {
                    VisualStudio.AssertNoErrors();
                    var solutionService = VisualStudio.Get<SolutionService>();
                    testContext.SolutionService.Build();
                    testContext.NuGetApexTestService.WaitForAutoRestore();

                    // Act
                    var nugetConsole = GetConsole(testContext.Project);

                    nugetConsole.InstallPackageFromPMC(PackageName, v100);
                    testContext.SolutionService.Build();
                    testContext.NuGetApexTestService.WaitForAutoRestore();

                    nugetConsole.UninstallPackageFromPMC(PackageName);
                    testContext.SolutionService.Build();
                    testContext.NuGetApexTestService.WaitForAutoRestore();

                    //Asset
                    VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
                    CommonUtility.AssertPackageNotInAssetsFile(VisualStudio, testContext.Project, PackageName, v100, Logger);
                    Assert.IsTrue(VisualStudio.HasNoErrorsInOutputWindows());
                }
            }
        }

        [DataTestMethod]
        [DataRow(ProjectTemplate.WCFServiceApplication)]
        [DataRow(ProjectTemplate.NetStandardClassLib)]
        [Timeout(DefaultTimeout)]

        public async Task InstallLatestPackageInPMC(ProjectTemplate projectTemplate)
        {
            EnsureVisualStudioHost();
            using (var simpleTestPathContext = new SimpleTestPathContext())
            {
                // Arrange
                var packageName = "InstallLatestInPMC";
                var v100 = "1.0.0";
                var v200 = "2.0.0";
                await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, v100);
                await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, v200);

                using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, addNetStandardFeeds: true, simpleTestPathContext: simpleTestPathContext))
                {
                    var solutionService = VisualStudio.Get<SolutionService>();
                    var nugetConsole = GetConsole(testContext.Project);

                    // Act
                    nugetConsole.Execute("install-package InstallLatestInPMC");
                    testContext.SolutionService.Build();
                    testContext.NuGetApexTestService.WaitForAutoRestore();

                    // Assert
                    if (projectTemplate.ToString().Equals("WCFServiceApplication"))
                    {
                        CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, v200, Logger);
                    }
                    else
                    {
                        CommonUtility.AssertPackageReferenceExists(VisualStudio, testContext.Project, packageName, v200, Logger);
                    }
                    VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
                    Assert.IsTrue(VisualStudio.HasNoErrorsInOutputWindows());
                }
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackagesConfigTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public void VerifyInitScriptsExecution(ProjectTemplate projectTemplate)
        {
            EnsureVisualStudioHost();
            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger))
            {
                // Arrange
                SolutionService solutionService = VisualStudio.Get<SolutionService>();
                var nugetConsole = GetConsole(testContext.Project);
                var source = NuGetConstants.V3FeedUrl;

                // Act
                nugetConsole.Execute($"install-package EntityFramework -source {source} -Verbose");

                // Assert
                Assert.IsTrue(nugetConsole.IsMessageFoundInPMC("init.ps1"), "The init.ps1 script in TestProject was not executed when the EntityFramework package was installed");

                // Act
                nugetConsole.Clear();
                nugetConsole.Execute($"install-package jquery -source {source} -Verbose");

                // Assert
                Assert.IsTrue(nugetConsole.IsMessageFoundInPMC("install.ps1"), "The install.ps1 script in TestProject was not executed when the jquery package was installed.");

                // Act
                nugetConsole.Clear();
                nugetConsole.Execute($"install-package entityframework.sqlservercompact -source {source} -Verbose");

                // Assert
                // nugetConsole.IsMessageFoundInPMC is case sensitive.
                Assert.IsTrue(nugetConsole.IsMessageFoundInPMC("Install.ps1"), "The Install.ps1 script in TestProject was not executed when the Entityframework.sqlservercompact package was installed.");
            }
        }

        // There  is a bug with VS or Apex where NetCoreConsoleApp creates a netcore 2.1 project that is not supported by the sdk
        // Commenting out any NetCoreConsoleApp template and swapping it for NetStandardClassLib as both are package ref.
        public static IEnumerable<object[]> GetNetCoreTemplates()
        {
            yield return new object[] { ProjectTemplate.NetStandardClassLib };
        }

        public static IEnumerable<object[]> GetPackageReferenceTemplates()
        {
            yield return new object[] { ProjectTemplate.NetStandardClassLib };
        }

        public static IEnumerable<object[]> GetPackagesConfigTemplates()
        {
            yield return new object[] { ProjectTemplate.ClassLibrary };
        }

        public static IEnumerable<object[]> GetIOSTemplates()
        {
            yield return new object[] { ProjectTemplate.IOSLibraryApp };
        }
    }
}
