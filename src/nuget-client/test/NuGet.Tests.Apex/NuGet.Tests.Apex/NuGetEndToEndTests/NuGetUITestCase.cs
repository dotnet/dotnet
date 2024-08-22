// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Test.Apex.VisualStudio.Solution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet.Test.Utility;

namespace NuGet.Tests.Apex
{
    [TestClass]
    public class NuGetUITestCase : SharedVisualStudioHostTestClass
    {
        private const string TestPackageName = "Contoso.A";
        private const string TestPackageVersionV1 = "1.0.0";
        private const string TestPackageVersionV2 = "2.0.0";
        private const string PrimarySourceName = "source";
        private const string SecondarySourceName = "SecondarySource";

        private readonly SimpleTestPathContext _pathContext = new SimpleTestPathContext();

        public NuGetUITestCase()
            : base()
        {
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task SearchPackageFromUI()
        {
            // Arrange
            await CommonUtility.CreatePackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV1);

            NuGetApexTestService nugetTestService = GetNuGetTestService();

            SolutionService solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution("TestSolution", _pathContext.SolutionRoot);
            ProjectTestExtension project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);

            NuGetUIProjectTestExtension uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.SwitchTabToBrowse();
            uiwindow.SearchPackageFromUI(TestPackageName);

            // Assert
            VisualStudio.AssertNoErrors();
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task InstallPackageFromUI()
        {
            // Arrange
            await CommonUtility.CreatePackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV1);

            NuGetApexTestService nugetTestService = GetNuGetTestService();

            SolutionService solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution("TestSolution", _pathContext.SolutionRoot);
            ProjectTestExtension project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);

            NuGetUIProjectTestExtension uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project, TestPackageName, TestPackageVersionV1, Logger);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task InstallPackageToProjectsFromUI()
        {
            // Arrange
            await CommonUtility.CreatePackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV1);

            NuGetApexTestService nugetTestService = GetNuGetTestService();

            SolutionService solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution("TestSolution", _pathContext.SolutionRoot);
            ProjectTestExtension project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            ProjectTestExtension nuProject = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "NuProject");
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);

            NuGetUIProjectTestExtension uiwindow = nugetTestService.GetUIWindowfromProject(nuProject);
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);

            VisualStudio.SelectProjectInSolutionExplorer(project.Name);
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);

            VisualStudio.ClearOutputWindow();
            NuGetUIProjectTestExtension uiwindow2 = nugetTestService.GetUIWindowfromProject(project);
            uiwindow2.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project, TestPackageName, TestPackageVersionV1, Logger);
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, nuProject, TestPackageName, TestPackageVersionV1, Logger);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task UninstallPackageFromUI()
        {
            // Arrange
            await CommonUtility.CreatePackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV1);

            NuGetApexTestService nugetTestService = GetNuGetTestService();

            SolutionService solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution("TestSolution", _pathContext.SolutionRoot);
            ProjectTestExtension project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            solutionService.SaveAll();

            FileInfo packagesConfigFile = GetPackagesConfigFile(project);
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);

            NuGetUIProjectTestExtension uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);

            CommonUtility.WaitForFileExists(packagesConfigFile);

            VisualStudio.ClearWindows();

            // Act
            uiwindow.UninstallPackageFromUI(TestPackageName);

            CommonUtility.WaitForFileNotExists(packagesConfigFile);

            // Assert
            CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, project, TestPackageName, Logger);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task UpdatePackageFromUI()
        {
            // Arrange
            await CommonUtility.CreatePackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV1);
            await CommonUtility.CreatePackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV2);

            NuGetApexTestService nugetTestService = GetNuGetTestService();

            SolutionService solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution("TestSolution", _pathContext.SolutionRoot);
            ProjectTestExtension project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            VisualStudio.ClearWindows();
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);

            NuGetUIProjectTestExtension uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);

            // Act
            VisualStudio.ClearWindows();
            uiwindow.UpdatePackageFromUI(TestPackageName, TestPackageVersionV2);

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project, TestPackageName, TestPackageVersionV2, Logger);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task InstallPackageFromUI_PC_PackageSourceMapping_WithSingleFeed_Match_Succeeds()
        {
            // Arrange
            await CommonUtility.CreatePackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV1);

            // Create nuget.config with Package source mapping filtering rules before project is created.
            _pathContext.Settings.AddPackageSourceMapping(PrimarySourceName, "Contoso.*", "Test.*");

            NuGetApexTestService nugetTestService = GetNuGetTestService();

            SolutionService solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution("TestSolution", _pathContext.SolutionRoot);
            ProjectTestExtension project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);

            NuGetUIProjectTestExtension uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project, TestPackageName, TestPackageVersionV1, Logger);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task InstallPackageToProjectsFromUI_PC_PackageSourceMapping_WithSingleFeed_Match_Succeeds()
        {
            // Arrange
            await CommonUtility.CreatePackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV1);

            // Create nuget.config with Package source mapping filtering rules before project is created.
            _pathContext.Settings.AddPackageSourceMapping(PrimarySourceName, "Contoso.*", "Test.*");

            NuGetApexTestService nugetTestService = GetNuGetTestService();

            SolutionService solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution("TestSolution", _pathContext.SolutionRoot);
            ProjectTestExtension project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            ProjectTestExtension nuProject = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "NuProject");
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);
            NuGetUIProjectTestExtension uiwindow = nugetTestService.GetUIWindowfromProject(nuProject);
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);
            VisualStudio.SelectProjectInSolutionExplorer(project.Name);
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);

            VisualStudio.ClearOutputWindow();
            NuGetUIProjectTestExtension uiwindow2 = nugetTestService.GetUIWindowfromProject(project);
            uiwindow2.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project, TestPackageName, TestPackageVersionV1, Logger);
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, nuProject, TestPackageName, TestPackageVersionV1, Logger);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task InstallPackageFromUI_PC_PackageSourceMapping_WithMultiFeed_Succeed()
        {
            // Arrange
            string secondarySourcePath = Directory.CreateDirectory(Path.Combine(_pathContext.SolutionRoot, SecondarySourceName)).FullName;

            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(secondarySourcePath, TestPackageName, TestPackageVersionV1);
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV1);

            // Create nuget.config with Package source mapping filtering rules before project is created.
            _pathContext.Settings.AddSource(SecondarySourceName, secondarySourcePath);
            _pathContext.Settings.AddPackageSourceMapping(SecondarySourceName, "External.*", "Others.*");
            _pathContext.Settings.AddPackageSourceMapping(PrimarySourceName, "Contoso.*", "Test.*");

            NuGetApexTestService nugetTestService = GetNuGetTestService();

            SolutionService solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution("TestSolution", _pathContext.SolutionRoot);
            ProjectTestExtension project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);

            NuGetUIProjectTestExtension uiwindow = nugetTestService.GetUIWindowfromProject(project);

            // Set option to package source option to All
            uiwindow.SetPackageSourceOptionToAll();
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project, "contoso.a", Logger);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task InstallPackageFromUI_PC_PackageSourceMapping_WithMultiFeed_Fails()
        {
            // Arrange
            string secondarySourcePath = Directory.CreateDirectory(Path.Combine(_pathContext.SolutionRoot, SecondarySourceName)).FullName;

            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(secondarySourcePath, TestPackageName, TestPackageVersionV1);

            // Create nuget.config with Package source mapping filtering rules before project is created.
            _pathContext.Settings.AddSource(SecondarySourceName, secondarySourcePath);
            _pathContext.Settings.AddPackageSourceMapping(SecondarySourceName, "External.*", "Others.*");
            _pathContext.Settings.AddPackageSourceMapping(PrimarySourceName, "Contoso.*", "Test.*");

            NuGetApexTestService nugetTestService = GetNuGetTestService();

            SolutionService solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution("TestSolution", _pathContext.SolutionRoot);
            ProjectTestExtension project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);

            NuGetUIProjectTestExtension uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);

            // Assert
            // Even though Contoso.a exist in ExternalRepository, but PackageNamespaces filter doesn't let restore from it.
            CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, project, "contoso.a", Logger);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task UpdatePackageFromUI_PC_PackageSourceMapping_WithSingleFeed_Succeeds()
        {
            // Arrange
            await CommonUtility.CreatePackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV1);
            await CommonUtility.CreatePackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV2);

            // Create nuget.config with Package source mapping filtering rules before project is created.
            _pathContext.Settings.AddPackageSourceMapping(PrimarySourceName, "Contoso.*", "Test.*");

            NuGetApexTestService nugetTestService = GetNuGetTestService();

            SolutionService solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution("TestSolution", _pathContext.SolutionRoot);
            ProjectTestExtension project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);
            NuGetUIProjectTestExtension uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);

            // Act
            VisualStudio.ClearWindows();
            uiwindow.UpdatePackageFromUI(TestPackageName, TestPackageVersionV2);

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project, TestPackageName, TestPackageVersionV2, Logger);
        }


        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task UpdatePackageFromUI_PC_PackageSourceMapping_WithMultiFeed_Succeed()
        {
            // Arrange
            string secondarySourcePath = Directory.CreateDirectory(Path.Combine(_pathContext.SolutionRoot, SecondarySourceName)).FullName;
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(secondarySourcePath, TestPackageName, TestPackageVersionV1);
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(secondarySourcePath, TestPackageName, TestPackageVersionV2);
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV1);
            await CommonUtility.CreateNetFrameworkPackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV2);

            // Create nuget.config with Package source mapping filtering rules before project is created.
            _pathContext.Settings.AddSource(SecondarySourceName, secondarySourcePath);
            _pathContext.Settings.AddPackageSourceMapping(SecondarySourceName, "External.*", "Others.*");
            _pathContext.Settings.AddPackageSourceMapping(PrimarySourceName, "Contoso.*", "Test.*");

            NuGetApexTestService nugetTestService = GetNuGetTestService();

            SolutionService solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution("TestSolution", _pathContext.SolutionRoot);
            ProjectTestExtension project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);
            NuGetUIProjectTestExtension uiwindow = nugetTestService.GetUIWindowfromProject(project);

            // Set option to package source option to All
            uiwindow.SetPackageSourceOptionToAll();
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);

            // Act
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV2);

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project, TestPackageName, TestPackageVersionV2, Logger);
        }

        public override void Dispose()
        {
            _pathContext.Dispose();

            base.Dispose();
        }

        private static FileInfo GetPackagesConfigFile(ProjectTestExtension project)
        {
            var projectFile = new FileInfo(project.FullPath);

            return new FileInfo(Path.Combine(projectFile.DirectoryName, "packages.config"));
        }
    }
}
