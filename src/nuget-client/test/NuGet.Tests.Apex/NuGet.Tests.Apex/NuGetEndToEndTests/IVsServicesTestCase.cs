// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Test.Apex.VisualStudio.Solution;
using Microsoft.VisualStudio.OLE.Interop;
using NuGet.Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.Tests.Apex
{
    public class IVsServicesTestCase : SharedVisualStudioHostTestClass, IClassFixture<VisualStudioHostFixtureFactory>
    {
        public IVsServicesTestCase(VisualStudioHostFixtureFactory visualStudioHostFixtureFactory, ITestOutputHelper output)
            : base(visualStudioHostFixtureFactory, output)
        {
        }

        [StaFact]
        public void SimpleInstallFromIVsInstaller()
        {
            // Arrange
            EnsureVisualStudioHost();
            var dte = VisualStudio.Dte;
            var solutionService = VisualStudio.Get<SolutionService>();
            var nugetTestService = GetNuGetTestService();

            solutionService.CreateEmptySolution();
            var projExt = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");

            var project = dte.Solution.Projects.Item(1);

            // Act
            nugetTestService.InstallPackage(project.UniqueName, "newtonsoft.json");

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, projExt, "newtonsoft.json", XunitLogger);
        }

        [StaFact]
        public void SimpleUninstallFromIVsInstaller()
        {
            // Arrange
            EnsureVisualStudioHost();
            var solutionService = VisualStudio.Get<SolutionService>();
            var nugetTestService = GetNuGetTestService();
            solutionService.CreateEmptySolution();
            var projExt = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            var dte = VisualStudio.Dte;
            var project = dte.Solution.Projects.Item(1);
            nugetTestService.InstallPackage(project.UniqueName, "newtonsoft.json");

            // Act
            nugetTestService.UninstallPackage(project.UniqueName, "newtonsoft.json");

            // Assert
            CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, projExt, "newtonsoft.json", XunitLogger);
        }


        [StaFact]
        public void IVSPathContextProvider2_WithEmptySolution_WhenTryCreateUserWideContextIsCalled_SolutionWideConfigurationIsNotIncluded()
        {
            // Arrange
            using (var testContext = new SimpleTestPathContext())
            {
                EnsureVisualStudioHost();
                var dte = VisualStudio.Dte;
                var solutionService = VisualStudio.Get<SolutionService>();
                var nugetTestService = GetNuGetTestService();

                solutionService.CreateEmptySolution("project", testContext.SolutionRoot);

                // Act
                var userPackagesFolder = nugetTestService.GetUserPackagesFolderFromUserWideContext();

                // Assert
                // The global packages folder should not be the one configured by the test context!
                userPackagesFolder.Should().NotBe(testContext.UserPackagesFolder);
            }
        }

        [StaFact]
        public async Task SimpleInstallFromIVsInstaller_PackageSourceMapping_WithSingleFeed()
        {
            // Arrange
            EnsureVisualStudioHost();
            var dte = VisualStudio.Dte;
            string mainDirectory = CommonUtility.CreateSolutionDirectory(Directory.GetCurrentDirectory());
            string solutionDirectory = Path.Combine(mainDirectory, "Solution");
            var solutionService = VisualStudio.Get<SolutionService>();
            var nugetTestService = GetNuGetTestService();

            var privateRepositoryPath = Path.Combine(mainDirectory, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            var packageName = "Contoso.A";
            var packageVersion = "1.0.0";

            await CommonUtility.CreatePackageInSourceAsync(privateRepositoryPath, packageName, packageVersion);

            // Create nuget.config with Package source mapping filtering rules before project is created.
            CommonUtility.CreateConfigurationFile(Path.Combine(mainDirectory, "NuGet.config"), $@"<?xml version=""1.0"" encoding=""utf-8""?>
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

            solutionService.CreateEmptySolution("TestSolution", solutionDirectory);
            var projExt = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            solutionService.SaveAll();
            var project = dte.Solution.Projects.Item(1);

            // Act
            nugetTestService.InstallPackage(project.UniqueName, "contoso.a");

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, projExt, packageName, XunitLogger);
        }

        [StaFact]
        public async Task SimpleUpdateFromIVsInstaller_PackageSourceMapping_WithSingleFeed()
        {
            // Arrange
            EnsureVisualStudioHost();
            var dte = VisualStudio.Dte;
            string mainDirectory = CommonUtility.CreateSolutionDirectory(Directory.GetCurrentDirectory());
            string solutionDirectory = Path.Combine(mainDirectory, "Solution");
            var solutionService = VisualStudio.Get<SolutionService>();
            var nugetTestService = GetNuGetTestService();

            var privateRepositoryPath = Path.Combine(mainDirectory, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            var packageName = "Contoso.A";
            var packageVersionV1 = "1.0.0";
            var packageVersionV2 = "2.0.0";

            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(privateRepositoryPath, packageName, packageVersionV1, "Thisisfromprivaterepo1.txt");
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(privateRepositoryPath, packageName, packageVersionV2, "Thisisfromprivaterepo2.txt");

            // Create nuget.config with Package source mapping filtering rules before project is created.
            CommonUtility.CreateConfigurationFile(Path.Combine(mainDirectory, "NuGet.config"), $@"<?xml version=""1.0"" encoding=""utf-8""?>
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

            solutionService.CreateEmptySolution("TestSolution", solutionDirectory);
            var projExt = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            solutionService.SaveAll();
            var project = dte.Solution.Projects.Item(1);

            // Act
            nugetTestService.InstallPackage(project.UniqueName, "contoso.a", "1.0.0");
            nugetTestService.InstallPackage(project.UniqueName, "contoso.a", "2.0.0");

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, projExt, packageName, XunitLogger);

            var packagesDirectory = Path.Combine(solutionDirectory, "packages");
            var uniqueContentFile = Path.Combine(packagesDirectory, packageName + '.' + packageVersionV2, "lib", "net45", "Thisisfromprivaterepo2.txt");
            // Make sure version 2 is restored.
            Assert.True(File.Exists(uniqueContentFile));
        }

        [StaFact]
        public async Task SimpleInstallFromIVsInstaller_PackageSourceMapping_WithMultipleFeedsWithIdenticalPackages_InstallsCorrectPackage()
        {
            // Arrange
            EnsureVisualStudioHost();
            var dte = VisualStudio.Dte;
            string mainDirectory = CommonUtility.CreateSolutionDirectory(Directory.GetCurrentDirectory());
            string solutionDirectory = Path.Combine(mainDirectory, "Solution");
            var solutionService = VisualStudio.Get<SolutionService>();
            var nugetTestService = GetNuGetTestService();

            var packageName = "Contoso.A";
            var packageVersionV1 = "1.0.0";
            var packageVersionV2 = "2.0.0";

            var opensourceRepositoryPath = Path.Combine(mainDirectory, "OpensourceRepository");
            Directory.CreateDirectory(opensourceRepositoryPath);

            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(opensourceRepositoryPath, packageName, packageVersionV1, "Thisisfromopensourcerepo1.txt");
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(opensourceRepositoryPath, packageName, packageVersionV2, "Thisisfromopensourcerepo2.txt");

            var privateRepositoryPath = Path.Combine(mainDirectory, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(privateRepositoryPath, packageName, packageVersionV1, "Thisisfromprivaterepo1.txt");
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(privateRepositoryPath, packageName, packageVersionV2, "Thisisfromprivaterepo2.txt");

            // Create nuget.config with Package source mapping filtering rules before project is created.
            CommonUtility.CreateConfigurationFile(Path.Combine(mainDirectory, "NuGet.config"), $@"<?xml version=""1.0"" encoding=""utf-8""?>
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

            solutionService.CreateEmptySolution("TestSolution", solutionDirectory);
            var projExt = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            solutionService.SaveAll();
            var project = dte.Solution.Projects.Item(1);

            // Act
            nugetTestService.InstallPackage(project.UniqueName, "contoso.a", "1.0.0");

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, projExt, packageName, XunitLogger);

            var packagesDirectory = Path.Combine(solutionDirectory, "packages");
            var uniqueContentFile = Path.Combine(packagesDirectory, packageName + '.' + packageVersionV1, "lib", "net45", "Thisisfromprivaterepo1.txt");
            // Make sure name squatting package not restored from  opensource repository.
            Assert.True(File.Exists(uniqueContentFile));
        }

        [StaFact]
        public async Task SimpleUpdateFromIVsInstaller_PackageSourceMapping_WithMultipleFeedsWithIdenticalPackages_UpdatesCorrectPackage()
        {
            // Arrange
            EnsureVisualStudioHost();
            var dte = VisualStudio.Dte;
            string mainDirectory = CommonUtility.CreateSolutionDirectory(Directory.GetCurrentDirectory());
            string solutionDirectory = Path.Combine(mainDirectory, "Solution");
            var solutionService = VisualStudio.Get<SolutionService>();
            var nugetTestService = GetNuGetTestService();

            var packageName = "Contoso.A";
            var packageVersionV1 = "1.0.0";
            var packageVersionV2 = "2.0.0";

            var opensourceRepositoryPath = Path.Combine(mainDirectory, "OpensourceRepository");
            Directory.CreateDirectory(opensourceRepositoryPath);

            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(opensourceRepositoryPath, packageName, packageVersionV1, "Thisisfromopensourcerepo1.txt");
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(opensourceRepositoryPath, packageName, packageVersionV2, "Thisisfromopensourcerepo2.txt");

            var privateRepositoryPath = Path.Combine(mainDirectory, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(privateRepositoryPath, packageName, packageVersionV1, "Thisisfromprivaterepo1.txt");
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(privateRepositoryPath, packageName, packageVersionV2, "Thisisfromprivaterepo2.txt");

            // Create nuget.config with Package source mapping filtering rules before project is created.
            CommonUtility.CreateConfigurationFile(Path.Combine(mainDirectory, "NuGet.config"), $@"<?xml version=""1.0"" encoding=""utf-8""?>
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

            solutionService.CreateEmptySolution("TestSolution", solutionDirectory);
            var projExt = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            solutionService.SaveAll();
            var project = dte.Solution.Projects.Item(1);

            // Act
            nugetTestService.InstallPackage(project.UniqueName, "contoso.a", "1.0.0");
            nugetTestService.InstallPackage(project.UniqueName, "contoso.a", "2.0.0");
            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, projExt, packageName, XunitLogger);

            var packagesDirectory = Path.Combine(solutionDirectory, "packages");
            var uniqueContentFile = Path.Combine(packagesDirectory, packageName + '.' + packageVersionV2, "lib", "net45", "Thisisfromprivaterepo2.txt");
            // Make sure name squatting package not restored from  opensource repository.
            Assert.True(File.Exists(uniqueContentFile));
        }
    }
}
