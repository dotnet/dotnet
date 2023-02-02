// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Test.Apex.VisualStudio.Solution;
using NuGet.Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.Tests.Apex
{
    public class IVsServicesTestCase : SharedVisualStudioHostTestClass, IClassFixture<VisualStudioHostFixtureFactory>
    {
        private const string TestPackageName = "Contoso.A";
        private const string TestPackageVersionV1 = "1.0.0";
        private const string TestPackageVersionV2 = "2.0.0";
        private const string PrimarySourceName = "source";
        private const string SecondarySourceName = "SecondarySource";

        private readonly SimpleTestPathContext _pathContext = new SimpleTestPathContext();

        public IVsServicesTestCase(VisualStudioHostFixtureFactory visualStudioHostFixtureFactory, ITestOutputHelper output)
            : base(visualStudioHostFixtureFactory, output)
        {
        }

        [StaFact]
        public void SimpleInstallFromIVsInstaller()
        {
            // Arrange
            NuGetApexTestService nugetTestService = GetNuGetTestService();

            SolutionService solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution();
            ProjectTestExtension projExt = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            EnvDTE.Project project = VisualStudio.Dte.Solution.Projects.Item(1);

            // Act
            nugetTestService.InstallPackage(project.UniqueName, "newtonsoft.json");

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, projExt, "newtonsoft.json", XunitLogger);
        }

        [StaFact(Skip = "https://github.com/NuGet/Home/issues/11308")]
        public void SimpleUninstallFromIVsInstaller()
        {
            // Arrange
            NuGetApexTestService nugetTestService = GetNuGetTestService();

            SolutionService solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution();
            ProjectTestExtension projExt = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            string projectUniqueName = VisualStudio.Dte.Solution.Projects.Item(1).UniqueName;

            // Act
            nugetTestService.InstallPackage(projectUniqueName, "newtonsoft.json");
            nugetTestService.UninstallPackage(projectUniqueName, "newtonsoft.json");

            // Assert
            CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, projExt, "newtonsoft.json", XunitLogger);
        }


        [StaFact]
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

        [StaFact(Skip = "https://github.com/NuGet/Home/issues/11308")]
        public async Task SimpleInstallFromIVsInstaller_PackageSourceMapping_WithSingleFeed()
        {
            // Arrange
            await CommonUtility.CreatePackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV1);
            _pathContext.Settings.AddPackageSourceMapping(PrimarySourceName, "Contoso.*", "Test.*");
            
            NuGetApexTestService nugetTestService = GetNuGetTestService();

            SolutionService solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution("TestSolution", _pathContext.SolutionRoot);
            ProjectTestExtension projExt = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            solutionService.SaveAll();
            EnvDTE.Project project = VisualStudio.Dte.Solution.Projects.Item(1);

            // Act
            nugetTestService.InstallPackage(project.UniqueName, TestPackageName);

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, projExt, TestPackageName, XunitLogger);
        }

        [StaFact]
        public async Task SimpleUpdateFromIVsInstaller_PackageSourceMapping_WithSingleFeed()
        {
            // Arrange
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV1, "Thisisfromprivaterepo1.txt");
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV2, "Thisisfromprivaterepo2.txt");
            _pathContext.Settings.AddPackageSourceMapping(PrimarySourceName, "Contoso.*", "Test.*");
            
            NuGetApexTestService nugetTestService = GetNuGetTestService();

            SolutionService solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution("TestSolution", _pathContext.SolutionRoot);
            ProjectTestExtension projExt = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            solutionService.SaveAll();
            EnvDTE.Project project = VisualStudio.Dte.Solution.Projects.Item(1);

            // Act
            nugetTestService.InstallPackage(project.UniqueName, TestPackageName, TestPackageVersionV1);
            nugetTestService.InstallPackage(project.UniqueName, TestPackageName, TestPackageVersionV2);

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, projExt, TestPackageName, XunitLogger);

            string uniqueContentFile = Path.Combine(_pathContext.PackagesV2, TestPackageName + '.' + TestPackageVersionV2, "lib", "net45", "Thisisfromprivaterepo2.txt");

            // Make sure version 2 is restored.
            File.Exists(uniqueContentFile).Should().BeTrue($"'{uniqueContentFile}' should exist");
        }

        [StaFact(Skip = "https://github.com/NuGet/Home/issues/11308")]
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
            ProjectTestExtension projExt = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            solutionService.SaveAll();
            EnvDTE.Project project = VisualStudio.Dte.Solution.Projects.Item(1);

            // Act
            nugetTestService.InstallPackage(project.UniqueName, TestPackageName, TestPackageVersionV1);

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, projExt, TestPackageName, XunitLogger);

            string uniqueContentFile = Path.Combine(_pathContext.PackagesV2, TestPackageName + '.' + TestPackageVersionV1, "lib", "net45", "Thisisfromprivaterepo1.txt");

            // Make sure name squatting package not restored from secondary repository.
            File.Exists(uniqueContentFile).Should().BeTrue($"'{uniqueContentFile}' should exist");
        }

        [StaFact]
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
            ProjectTestExtension projExt = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            solutionService.SaveAll();
            EnvDTE.Project project = VisualStudio.Dte.Solution.Projects.Item(1);

            // Act
            nugetTestService.InstallPackage(project.UniqueName, TestPackageName, TestPackageVersionV1);
            nugetTestService.InstallPackage(project.UniqueName, TestPackageName, TestPackageVersionV2);

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, projExt, TestPackageName, XunitLogger);

            string uniqueContentFile = Path.Combine(_pathContext.PackagesV2, TestPackageName + '.' + TestPackageVersionV2, "lib", "net45", "Thisisfromprivaterepo2.txt");

            // Make sure name squatting package not restored from secondary repository.
            File.Exists(uniqueContentFile).Should().BeTrue($"'{uniqueContentFile}' should exist");
        }

        public override void Dispose()
        {
            _pathContext.Dispose();

            base.Dispose();
        }
    }
}
