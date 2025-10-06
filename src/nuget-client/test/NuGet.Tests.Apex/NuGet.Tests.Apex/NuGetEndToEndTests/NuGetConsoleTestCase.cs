// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
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
    }
}
