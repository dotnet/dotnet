// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Test.Apex.VisualStudio.Solution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet.Test.Utility;

namespace NuGet.Tests.Apex
{
    [TestClass]
    public class IVsServicesTestCase : SharedVisualStudioHostTestClass
    {
        private const string TestPackageName = "Contoso.A";
        private const string TestPackageVersionV1 = "1.0.0";
        private const string TestPackageVersionV2 = "2.0.0";
        private const string PrimarySourceName = "source";
        private const string SecondarySourceName = "SecondarySource";

        internal const int LongerTimeout = 10 * 60 * 1000; // 10 minutes

        private readonly SimpleTestPathContext _pathContext = new SimpleTestPathContext();

        [TestMethod]
        [Timeout(LongerTimeout)]
        public void SimpleInstallFromIVsInstaller()
        {
            // Arrange
            NuGetApexTestService nugetTestService = GetNuGetTestService();

            SolutionService solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution();
            ProjectTestExtension projExt = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, CommonUtility.DefaultTargetFramework, "TestProject");
            EnvDTE.Project project = VisualStudio.Dte.Solution.Projects.Item(1);

            // Act
            nugetTestService.InstallPackage(project.UniqueName, "newtonsoft.json");

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, projExt, "newtonsoft.json", Logger);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public void SimpleUninstallFromIVsInstaller()
        {
            // Arrange
            NuGetApexTestService nugetTestService = GetNuGetTestService();

            SolutionService solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution();
            ProjectTestExtension projExt = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, CommonUtility.DefaultTargetFramework, "TestProject");
            string projectUniqueName = VisualStudio.Dte.Solution.Projects.Item(1).UniqueName;

            // Act
            nugetTestService.InstallPackage(projectUniqueName, "newtonsoft.json");
            nugetTestService.UninstallPackage(projectUniqueName, "newtonsoft.json");

            // Assert
            CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, projExt, "newtonsoft.json", Logger);
        }


        [TestMethod]
        [Timeout(DefaultTimeout)]
        public void IVSPathContextProvider2_WithEmptySolution_WhenTryCreateUserWideContextIsCalled_SolutionWideConfigurationIsNotIncluded()
        {
            // Arrange
            NuGetApexTestService nugetTestService = GetNuGetTestService();

            SolutionService solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution("project", _pathContext.SolutionRoot);

            // Act
            string userPackagesFolder = nugetTestService.GetUserPackagesFolderFromUserWideContext();

            // Assert
            // The global packages folder should not be the one configured by the test context!
            userPackagesFolder.Should().NotBe(_pathContext.UserPackagesFolder);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task SimpleInstallFromIVsInstaller_PackageSourceMapping_WithSingleFeed()
        {
            // Arrange
            await CommonUtility.CreatePackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV1);
            _pathContext.Settings.AddPackageSourceMapping(PrimarySourceName, "Contoso.*", "Test.*");

            NuGetApexTestService nugetTestService = GetNuGetTestService();

            SolutionService solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution("TestSolution", _pathContext.SolutionRoot);
            ProjectTestExtension projExt = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, CommonUtility.DefaultTargetFramework, "TestProject");
            solutionService.SaveAll();
            EnvDTE.Project project = VisualStudio.Dte.Solution.Projects.Item(1);

            // Act
            nugetTestService.InstallPackage(project.UniqueName, TestPackageName);
            solutionService.SaveAll();

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, projExt, TestPackageName, Logger);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task SimpleUpdateFromIVsInstaller_PackageSourceMapping_WithSingleFeed()
        {
            // Arrange
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV1, "Thisisfromprivaterepo1.txt");
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV2, "Thisisfromprivaterepo2.txt");
            _pathContext.Settings.AddPackageSourceMapping(PrimarySourceName, "Contoso.*", "Test.*");

            NuGetApexTestService nugetTestService = GetNuGetTestService();

            SolutionService solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution("TestSolution", _pathContext.SolutionRoot);
            ProjectTestExtension projExt = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, CommonUtility.DefaultTargetFramework, "TestProject");
            solutionService.SaveAll();
            EnvDTE.Project project = VisualStudio.Dte.Solution.Projects.Item(1);

            // Act
            nugetTestService.InstallPackage(project.UniqueName, TestPackageName, TestPackageVersionV1);
            nugetTestService.InstallPackage(project.UniqueName, TestPackageName, TestPackageVersionV2);
            solutionService.SaveAll();

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, projExt, TestPackageName, Logger);

            string uniqueContentFile = Path.Combine(_pathContext.PackagesV2, TestPackageName + '.' + TestPackageVersionV2, "lib", "net45", "Thisisfromprivaterepo2.txt");

            // Make sure version 2 is restored.
            Assert.IsTrue(File.Exists(uniqueContentFile), $"'{uniqueContentFile}' should exist");
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task SimpleInstallFromIVsInstaller_PackageSourceMapping_WithMultipleFeedsWithIdenticalPackages_InstallsCorrectPackage()
        {
            // Arrange
            string secondarySourcePath = Directory.CreateDirectory(Path.Combine(_pathContext.SolutionRoot, SecondarySourceName)).FullName;
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(secondarySourcePath, TestPackageName, TestPackageVersionV1, "Thisisfromsecondaryrepo1.txt");
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(secondarySourcePath, TestPackageName, TestPackageVersionV2, "Thisisfromsecondaryrepo2.txt");
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV1, "Thisisfromprivaterepo1.txt");
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV2, "Thisisfromprivaterepo2.txt");
            _pathContext.Settings.AddSource(SecondarySourceName, secondarySourcePath);
            _pathContext.Settings.AddPackageSourceMapping(SecondarySourceName, "External.*", "Others.*");
            _pathContext.Settings.AddPackageSourceMapping(PrimarySourceName, "Contoso.*", "Test.*");

            NuGetApexTestService nugetTestService = GetNuGetTestService();

            SolutionService solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution("TestSolution", _pathContext.SolutionRoot);
            ProjectTestExtension projExt = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, CommonUtility.DefaultTargetFramework, "TestProject");
            solutionService.SaveAll();
            EnvDTE.Project project = VisualStudio.Dte.Solution.Projects.Item(1);

            // Act
            nugetTestService.InstallPackage(project.UniqueName, TestPackageName, TestPackageVersionV1);
            solutionService.SaveAll();

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, projExt, TestPackageName, Logger);

            string uniqueContentFile = Path.Combine(_pathContext.PackagesV2, TestPackageName + '.' + TestPackageVersionV1, "lib", "net45", "Thisisfromprivaterepo1.txt");

            // Make sure name squatting package not restored from secondary repository.
            Assert.IsTrue(File.Exists(uniqueContentFile), $"'{uniqueContentFile}' should exist");
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task SimpleUpdateFromIVsInstaller_PackageSourceMapping_WithMultipleFeedsWithIdenticalPackages_UpdatesCorrectPackage()
        {
            // Arrange
            string secondarySourcePath = Directory.CreateDirectory(Path.Combine(_pathContext.SolutionRoot, SecondarySourceName)).FullName;
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(secondarySourcePath, TestPackageName, TestPackageVersionV1, "Thisisfromsecondaryrepo1.txt");
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(secondarySourcePath, TestPackageName, TestPackageVersionV2, "Thisisfromsecondaryrepo2.txt");
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV1, "Thisisfromprivaterepo1.txt");
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV2, "Thisisfromprivaterepo2.txt");
            _pathContext.Settings.AddSource(SecondarySourceName, secondarySourcePath);
            _pathContext.Settings.AddPackageSourceMapping(SecondarySourceName, "External.*", "Others.*");
            _pathContext.Settings.AddPackageSourceMapping(PrimarySourceName, "Contoso.*", "Test.*");

            NuGetApexTestService nugetTestService = GetNuGetTestService();

            SolutionService solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution("TestSolution", _pathContext.SolutionRoot);
            ProjectTestExtension projExt = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, CommonUtility.DefaultTargetFramework, "TestProject");
            solutionService.SaveAll();
            EnvDTE.Project project = VisualStudio.Dte.Solution.Projects.Item(1);

            // Act
            nugetTestService.InstallPackage(project.UniqueName, TestPackageName, TestPackageVersionV1);
            nugetTestService.InstallPackage(project.UniqueName, TestPackageName, TestPackageVersionV2);
            solutionService.SaveAll();
            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, projExt, TestPackageName, Logger);
            string uniqueContentFile = Path.Combine(_pathContext.PackagesV2, TestPackageName + '.' + TestPackageVersionV2, "lib", "net45", "Thisisfromprivaterepo2.txt");

            // Make sure name squatting package not restored from secondary repository.
            Assert.IsTrue(File.Exists(uniqueContentFile), $"'{uniqueContentFile}' should exist");
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task SimpleInstallFromIVsInstaller_PackageSourceMapping_WithMissingMappedSource_Fails()
        {
            // Arrange
            await CommonUtility.CreatePackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV1);
            // Map the package pattern to a non-existent source "SecretPackages" — package exists in primary source but mapping won't route there.
            _pathContext.Settings.AddPackageSourceMapping("SecretPackages", "Contoso.*", "Test.*");

            NuGetApexTestService nugetTestService = GetNuGetTestService();

            SolutionService solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution("TestSolution", _pathContext.SolutionRoot);
            ProjectTestExtension projExt = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, CommonUtility.DefaultTargetFramework, "TestProject");
            solutionService.SaveAll();
            EnvDTE.Project project = VisualStudio.Dte.Solution.Projects.Item(1);

            // Act — InstallPackage swallows InvalidOperationException, so the call returns without throwing.
            nugetTestService.InstallPackage(project.UniqueName, TestPackageName);
            solutionService.SaveAll();

            // Assert
            CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, projExt, TestPackageName, Logger);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task SimpleInstallLatestFromIVsInstaller_PackageSourceMapping_WithMultipleFeedsWithIdenticalPackages_InstallsCorrectPackage()
        {
            // Arrange
            string secondarySourcePath = Directory.CreateDirectory(Path.Combine(_pathContext.SolutionRoot, SecondarySourceName)).FullName;
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(secondarySourcePath, TestPackageName, TestPackageVersionV1, "Thisisfromsecondaryrepo1.txt");
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(secondarySourcePath, TestPackageName, TestPackageVersionV2, "Thisisfromsecondaryrepo2.txt");
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV1, "Thisisfromprivaterepo1.txt");
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV2, "Thisisfromprivaterepo2.txt");
            _pathContext.Settings.AddSource(SecondarySourceName, secondarySourcePath);
            _pathContext.Settings.AddPackageSourceMapping(SecondarySourceName, "External.*", "Others.*");
            _pathContext.Settings.AddPackageSourceMapping(PrimarySourceName, "Contoso.*", "Test.*");

            NuGetApexTestService nugetTestService = GetNuGetTestService();

            SolutionService solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution("TestSolution", _pathContext.SolutionRoot);
            ProjectTestExtension projExt = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, CommonUtility.DefaultTargetFramework, "TestProject");
            solutionService.SaveAll();
            EnvDTE.Project project = VisualStudio.Dte.Solution.Projects.Item(1);

            // Act — install latest (no version specified)
            nugetTestService.InstallPackage(project.UniqueName, TestPackageName);
            solutionService.SaveAll();

            // Assert — latest version (2.0.0) should be installed from the mapped primary source
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, projExt, TestPackageName, Logger);

            string uniqueContentFile = Path.Combine(_pathContext.PackagesV2, TestPackageName + '.' + TestPackageVersionV2, "lib", "net45", "Thisisfromprivaterepo2.txt");
            Assert.IsTrue(File.Exists(uniqueContentFile), $"'{uniqueContentFile}' should exist");
        }

        public override void Dispose()
        {
            _pathContext.Dispose();

            base.Dispose();
        }
    }
}
