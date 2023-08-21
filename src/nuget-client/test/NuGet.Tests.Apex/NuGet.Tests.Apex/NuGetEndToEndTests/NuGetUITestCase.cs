// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Threading.Tasks;
using Microsoft.Test.Apex.VisualStudio.Solution;
using NuGet.StaFact;
using NuGet.Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.Tests.Apex
{
    public class NuGetUITestCase : SharedVisualStudioHostTestClass, IClassFixture<VisualStudioHostFixtureFactory>
    {
        private const string TestPackageName = "Contoso.A";
        private const string TestPackageVersionV1 = "1.0.0";
        private const string TestPackageVersionV2 = "2.0.0";
        private const string PrimarySourceName = "source";
        private const string SecondarySourceName = "SecondarySource";

        private readonly SimpleTestPathContext _pathContext = new SimpleTestPathContext();

        public NuGetUITestCase(VisualStudioHostFixtureFactory visualStudioHostFixtureFactory, ITestOutputHelper output)
            : base(visualStudioHostFixtureFactory, output)
        {
        }

        [StaFact]
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
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);

            NuGetUIProjectTestExtension uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.SwitchTabToBrowse();
            uiwindow.SearchPackageFromUI(TestPackageName);

            // Assert
            VisualStudio.AssertNoErrors();
        }

        [StaFact]
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
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);

            NuGetUIProjectTestExtension uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project, TestPackageName, TestPackageVersionV1, XunitLogger);
        }

        [StaFact]
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
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);

            NuGetUIProjectTestExtension uiwindow = nugetTestService.GetUIWindowfromProject(nuProject);
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);

            VisualStudio.SelectProjectInSolutionExplorer(project.Name);
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);

            VisualStudio.ClearOutputWindow();
            NuGetUIProjectTestExtension uiwindow2 = nugetTestService.GetUIWindowfromProject(project);
            uiwindow2.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project, TestPackageName, TestPackageVersionV1, XunitLogger);
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, nuProject, TestPackageName, TestPackageVersionV1, XunitLogger);
        }

        [StaFact]
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
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);

            NuGetUIProjectTestExtension uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);

            CommonUtility.WaitForFileExists(packagesConfigFile);

            VisualStudio.ClearWindows();

            // Act
            uiwindow.UninstallPackageFromUI(TestPackageName);

            CommonUtility.WaitForFileNotExists(packagesConfigFile);

            // Assert
            CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, project, TestPackageName, XunitLogger);
        }

        [StaFact]
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
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);

            NuGetUIProjectTestExtension uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);

            // Act
            VisualStudio.ClearWindows();
            uiwindow.UpdatePackageFromUI(TestPackageName, TestPackageVersionV2);

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project, TestPackageName, TestPackageVersionV2, XunitLogger);
        }

        [StaFact]
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
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);

            NuGetUIProjectTestExtension uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project, TestPackageName, TestPackageVersionV1, XunitLogger);
        }

        [StaFact]
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
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);
            NuGetUIProjectTestExtension uiwindow = nugetTestService.GetUIWindowfromProject(nuProject);
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);
            VisualStudio.SelectProjectInSolutionExplorer(project.Name);
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);

            VisualStudio.ClearOutputWindow();
            NuGetUIProjectTestExtension uiwindow2 = nugetTestService.GetUIWindowfromProject(project);
            uiwindow2.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project, TestPackageName, TestPackageVersionV1, XunitLogger);
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, nuProject, TestPackageName, TestPackageVersionV1, XunitLogger);
        }

        [StaFact]
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
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);

            NuGetUIProjectTestExtension uiwindow = nugetTestService.GetUIWindowfromProject(project);

            // Set option to package source option to All
            uiwindow.SetPackageSourceOptionToAll();
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project, "contoso.a", XunitLogger);
        }

        [StaFact]
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
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);

            NuGetUIProjectTestExtension uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);

            // Assert
            // Even though Contoso.a exist in ExternalRepository, but packageSourceMapping filter doesn't let restore from it.
            CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, project, "contoso.a", XunitLogger);
        }

        [StaFact]
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
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);
            NuGetUIProjectTestExtension uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);

            // Act
            VisualStudio.ClearWindows();
            uiwindow.UpdatePackageFromUI(TestPackageName, TestPackageVersionV2);

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project, TestPackageName, TestPackageVersionV2, XunitLogger);
        }


        [StaFact]
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
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);
            NuGetUIProjectTestExtension uiwindow = nugetTestService.GetUIWindowfromProject(project);

            // Set option to package source option to All
            uiwindow.SetPackageSourceOptionToAll();
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);

            // Act
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV2);

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project, TestPackageName, TestPackageVersionV2, XunitLogger);
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


        [StaFact]
        public void InstallPackageToWebSiteProjectFromUI()
        {
            // Arrange
            EnsureVisualStudioHost();
            var dte = VisualStudio.Dte;
            var solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution();
            var project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.WebSiteEmpty, ProjectTargetFramework.V48, "WebSiteEmpty");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);
            var nugetTestService = GetNuGetTestService();
            var uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI("log4net", "2.0.12");

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project, "log4net", "2.0.12", XunitLogger);
        }

        [StaFact]
        public void UpdateWebSitePackageFromUI()
        {
            // Arrange
            EnsureVisualStudioHost();
            var dte = VisualStudio.Dte;
            var solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution();
            var project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.WebSiteEmpty, ProjectTargetFramework.V48, "WebSiteEmpty");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);
            var nugetTestService = GetNuGetTestService();
            var uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI("log4net", "2.0.13");
            VisualStudio.ClearWindows();
            uiwindow.UpdatePackageFromUI("log4net", "2.0.15");

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project, "log4net", "2.0.15", XunitLogger);
        }

        [StaFact]
        public void UninstallWebSitePackageFromUI()
        {
            // Arrange
            EnsureVisualStudioHost();
            var dte = VisualStudio.Dte;
            var solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution();
            var project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.WebSiteEmpty, ProjectTargetFramework.V48, "WebSiteEmpty");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);
            var nugetTestService = GetNuGetTestService();
            var uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI("log4net", "2.0.15");
            VisualStudio.ClearWindows();
            uiwindow.UninstallPackageFromUI("log4net");

            // Assert
            CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, project, "log4net", XunitLogger);
        }

        [StaFact]
        public async Task SearchPackageInBrowseTabFromUI()
        {
            // Arrange
            var tabName = "Browse";
            await CommonUtility.CreatePackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV1);

            NuGetApexTestService nugetTestService = GetNuGetTestService();

            SolutionService solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution("TestSolution", _pathContext.SolutionRoot);
            ProjectTestExtension project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V48, "TestProject");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);

            NuGetUIProjectTestExtension uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.SwitchTabToBrowse();
            uiwindow.SearchPackageFromUI(TestPackageName);

            // Assert
            VisualStudio.AssertNoErrors();
            uiwindow.AssertSearchedPackageItem(tabName, TestPackageName);
        }

        [StaFact]
        public async Task SearchPackageInInstalledTabFromUI()
        {
            // Arrange
            var TestPackageName2 = "Contoso.B";
            var tabName = "Installed";
            await CommonUtility.CreatePackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV1);
            await CommonUtility.CreatePackageInSourceAsync(_pathContext.PackageSource, TestPackageName2, TestPackageVersionV1);
            CommonUtility.CreatePackage(TestPackageName2, TestPackageVersionV1);

            NuGetApexTestService nugetTestService = GetNuGetTestService();

            SolutionService solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution("TestSolution", _pathContext.SolutionRoot);
            ProjectTestExtension project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V48, "TestProject");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);

            NuGetUIProjectTestExtension uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);
            uiwindow.InstallPackageFromUI(TestPackageName2, TestPackageVersionV1);
            uiwindow.SearchPackageFromUI(TestPackageName);

            // Assert
            VisualStudio.AssertNoErrors();
            uiwindow.AssertSearchedPackageItem(tabName, TestPackageName, TestPackageVersionV1);
        }

        [StaFact]
        public async Task SearchPackageInUpdatesTabFromUI()
        {
            //Arrange
            var tabName = "Updates";
            await CommonUtility.CreatePackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV1);
            await CommonUtility.CreatePackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV2);

            NuGetApexTestService nugetTestService = GetNuGetTestService();

            SolutionService solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution("TestSolution", _pathContext.SolutionRoot);
            ProjectTestExtension project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V48, "TestProject");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);

            NuGetUIProjectTestExtension uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);
            uiwindow.SwitchTabToUpdate();
            uiwindow.SearchPackageFromUI(TestPackageName);

            // Assert
            VisualStudio.AssertNoErrors();
            uiwindow.AssertSearchedPackageItem(tabName, TestPackageName, TestPackageVersionV2);
        }

        [NuGetWpfTheory]
        [InlineData(ProjectTemplate.ClassLibrary, "Newtonsoft.Json", "12.0.2")]
        [InlineData(ProjectTemplate.NetCoreClassLib, "Newtonsoft.Json", "12.0.2")]
        public void InstallVulnerablePackageFromUI(ProjectTemplate projectTemplate, string packageName, string packageVersion)
        {
            // Arrange
            EnsureVisualStudioHost();
            var solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution();
            var project = solutionService.AddProject(ProjectLanguage.CSharp, projectTemplate, "TestProject");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);
            var nugetTestService = GetNuGetTestService();
            var uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(packageName, packageVersion);
            solutionService.Build();

            // Assert
            CommonUtility.AssertInstalledPackageByProjectType(VisualStudio, projectTemplate, project, packageName, packageVersion, XunitLogger);
            uiwindow.AssertInstalledPackageVulnerable();
        }

        [NuGetWpfTheory]
        [InlineData(ProjectTemplate.ClassLibrary, "Newtonsoft.Json", "12.0.2", "13.0.1")]
        [InlineData(ProjectTemplate.NetCoreClassLib, "Newtonsoft.Json", "12.0.2", "13.0.2")]
        public void UpdateVulnerablePackageFromUI(ProjectTemplate projectTemplate, string packageName, string packageVersion1, string packageVersion2)
        {
            // Arrange
            EnsureVisualStudioHost();
            var solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution();
            var project = solutionService.AddProject(ProjectLanguage.CSharp, projectTemplate, "TestProject");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);
            var nugetTestService = GetNuGetTestService();
            var uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(packageName, packageVersion1);
            solutionService.Build();

            // Assert
            CommonUtility.AssertInstalledPackageByProjectType(VisualStudio, projectTemplate, project, packageName, packageVersion1, XunitLogger);
            uiwindow.AssertInstalledPackageVulnerable();

            // Act
            uiwindow.UpdatePackageFromUI(packageName, packageVersion2);

            // Assert
            CommonUtility.AssertInstalledPackageByProjectType(VisualStudio, projectTemplate, project, packageName, packageVersion2, XunitLogger);
            uiwindow.AssertInstalledPackageNotVulnerable();
        }

        [NuGetWpfTheory]
        [InlineData(ProjectTemplate.ClassLibrary, "Newtonsoft.Json", "12.0.3")]
        [InlineData(ProjectTemplate.NetCoreClassLib, "Newtonsoft.Json", "12.0.3")]
        public void UninstallVulnerablePackageFromUI(ProjectTemplate projectTemplate, string packageName, string packageVersion)
        {
            // Arrange
            EnsureVisualStudioHost();
            var solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution();
            var project = solutionService.AddProject(ProjectLanguage.CSharp, projectTemplate, "Testproject");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);
            var nugetTestService = GetNuGetTestService();
            var uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(packageName, packageVersion);
            solutionService.Build();

            // Assert
            CommonUtility.AssertInstalledPackageByProjectType(VisualStudio, projectTemplate, project, packageName, packageVersion, XunitLogger);
            uiwindow.AssertInstalledPackageVulnerable();

            // Act
            VisualStudio.ClearWindows();
            uiwindow.UninstallPackageFromUI(packageName);

            // Assert
            if (projectTemplate.Equals(ProjectTemplate.ClassLibrary))
            {
                CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, project, packageName, XunitLogger);
            }
            else
            {
                CommonUtility.AssertPackageReferenceDoesNotExist(VisualStudio, project, packageName, XunitLogger);
            }
        }
    }
}
