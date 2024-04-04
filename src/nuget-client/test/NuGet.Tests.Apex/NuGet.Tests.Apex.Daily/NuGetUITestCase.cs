// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Test.Apex.VisualStudio.Solution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet.Test.Utility;

namespace NuGet.Tests.Apex.Daily
{
    [TestClass]
    public class NuGetUITestCase : SharedVisualStudioHostTestClass
    {
        private const string TestPackageName = "Contoso.A";
        private const string TestPackageVersionV1 = "1.0.0";
        private const string TestPackageVersionV2 = "2.0.0";

        private readonly SimpleTestPathContext _pathContext = new SimpleTestPathContext();

        public NuGetUITestCase()
            : base()
        {
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
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
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);
            var nugetTestService = GetNuGetTestService();
            var uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI("log4net", "2.0.12");

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project, "log4net", "2.0.12", Logger);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
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
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);
            var nugetTestService = GetNuGetTestService();
            var uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI("log4net", "2.0.13");
            VisualStudio.ClearWindows();
            uiwindow.UpdatePackageFromUI("log4net", "2.0.15");

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project, "log4net", "2.0.15", Logger);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
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
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);
            var nugetTestService = GetNuGetTestService();
            var uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI("log4net", "2.0.15");
            VisualStudio.ClearWindows();
            uiwindow.UninstallPackageFromUI("log4net");

            // Assert
            CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, project, "log4net", Logger);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
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
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);

            NuGetUIProjectTestExtension uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.SwitchTabToBrowse();
            uiwindow.SearchPackageFromUI(TestPackageName);

            // Assert
            VisualStudio.AssertNoErrors();
            uiwindow.AssertSearchedPackageItem(tabName, TestPackageName);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
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
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);

            NuGetUIProjectTestExtension uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);
            uiwindow.InstallPackageFromUI(TestPackageName2, TestPackageVersionV1);
            uiwindow.SearchPackageFromUI(TestPackageName);

            // Assert
            VisualStudio.AssertNoErrors();
            uiwindow.AssertSearchedPackageItem(tabName, TestPackageName, TestPackageVersionV1);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
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
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);

            NuGetUIProjectTestExtension uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);
            uiwindow.SwitchTabToUpdate();
            uiwindow.SearchPackageFromUI(TestPackageName);

            // Assert
            VisualStudio.AssertNoErrors();
            uiwindow.AssertSearchedPackageItem(tabName, TestPackageName, TestPackageVersionV2);
        }

        [DataTestMethod]
        [DataRow(ProjectTemplate.ClassLibrary, "Newtonsoft.Json", "12.0.2")]
        [DataRow(ProjectTemplate.NetCoreClassLib, "Newtonsoft.Json", "12.0.2")]
        [Timeout(DefaultTimeout)]
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
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);
            var nugetTestService = GetNuGetTestService();
            var uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(packageName, packageVersion);
            solutionService.Build();

            // Assert
            CommonUtility.AssertInstalledPackageByProjectType(VisualStudio, projectTemplate, project, packageName, packageVersion, Logger);
            uiwindow.AssertInstalledPackageVulnerable();
        }

        [DataTestMethod]
        [DataRow(ProjectTemplate.ClassLibrary, "Newtonsoft.Json", "12.0.2", "13.0.1")]
        [DataRow(ProjectTemplate.NetCoreClassLib, "Newtonsoft.Json", "12.0.2", "13.0.2")]
        [Timeout(DefaultTimeout)]
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
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);
            var nugetTestService = GetNuGetTestService();
            var uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(packageName, packageVersion1);
            solutionService.Build();

            // Assert
            CommonUtility.AssertInstalledPackageByProjectType(VisualStudio, projectTemplate, project, packageName, packageVersion1, Logger);
            uiwindow.AssertInstalledPackageVulnerable();

            // Act
            uiwindow.UpdatePackageFromUI(packageName, packageVersion2);

            // Assert
            CommonUtility.AssertInstalledPackageByProjectType(VisualStudio, projectTemplate, project, packageName, packageVersion2, Logger);
            uiwindow.AssertInstalledPackageNotVulnerable();
        }

        [DataTestMethod]
        [DataRow(ProjectTemplate.ClassLibrary, "Newtonsoft.Json", "12.0.3")]
        [DataRow(ProjectTemplate.NetCoreClassLib, "Newtonsoft.Json", "12.0.3")]
        [Timeout(DefaultTimeout)]
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
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);
            var nugetTestService = GetNuGetTestService();
            var uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(packageName, packageVersion);
            solutionService.Build();

            // Assert
            CommonUtility.AssertInstalledPackageByProjectType(VisualStudio, projectTemplate, project, packageName, packageVersion, Logger);
            uiwindow.AssertInstalledPackageVulnerable();

            // Act
            VisualStudio.ClearWindows();
            uiwindow.UninstallPackageFromUI(packageName);

            // Assert
            CommonUtility.AssertUninstalledPackageByProjectType(VisualStudio, projectTemplate, project, packageName, Logger);
        }

        [DataTestMethod]
        [DataRow(ProjectTemplate.ClassLibrary, "jquery", "3.5.0")]
        [DataRow(ProjectTemplate.NetCoreClassLib, "jquery", "3.5.0")]
        [Timeout(DefaultTimeout)]
        public void InstallDeprecatedPackageFromUI(ProjectTemplate projectTemplate, string packageName, string packageVersion)
        {
            // Arrange
            EnsureVisualStudioHost();
            var solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution();
            var project = solutionService.AddProject(ProjectLanguage.CSharp, projectTemplate, "TestProject");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);
            var nugetTestService = GetNuGetTestService();
            var uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(packageName, packageVersion);
            solutionService.Build();

            // Assert
            CommonUtility.AssertInstalledPackageByProjectType(VisualStudio, projectTemplate, project, packageName, packageVersion, Logger);
            uiwindow.AssertInstalledPackageDeprecated();
        }

        [DataTestMethod]
        [DataRow(ProjectTemplate.ClassLibrary, "jQuery", "3.5.0", "3.6.0")]
        [DataRow(ProjectTemplate.NetCoreClassLib, "jQuery", "3.5.0", "3.6.3")]
        [Timeout(DefaultTimeout)]
        public void UpdateDeprecatedPackageFromUI(ProjectTemplate projectTemplate, string packageName, string packageVersion1, string packageVersion2)
        {
            // Arrange
            EnsureVisualStudioHost();
            var solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution();
            var project = solutionService.AddProject(ProjectLanguage.CSharp, projectTemplate, "TestProject");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);
            var nugetTestService = GetNuGetTestService();
            var uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(packageName, packageVersion1);
            solutionService.Build();

            // Assert
            CommonUtility.AssertInstalledPackageByProjectType(VisualStudio, projectTemplate, project, packageName, packageVersion1, Logger);
            uiwindow.AssertInstalledPackageDeprecated();

            // Act
            uiwindow.UpdatePackageFromUI(packageName, packageVersion2);

            // Assert
            CommonUtility.AssertInstalledPackageByProjectType(VisualStudio, projectTemplate, project, packageName, packageVersion2, Logger);
            uiwindow.AssertInstalledPackageNotDeprecated();
        }

        [DataTestMethod]
        [DataRow(ProjectTemplate.ClassLibrary, "jQuery", "3.5.0")]
        [DataRow(ProjectTemplate.NetCoreClassLib, "jQuery", "3.5.0")]
        [Timeout(DefaultTimeout)]
        public void UninstallDeprecatedPackageFromUI(ProjectTemplate projectTemplate, string packageName, string packageVersion)
        {
            // Arrange
            EnsureVisualStudioHost();
            var solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution();
            var project = solutionService.AddProject(ProjectLanguage.CSharp, projectTemplate, "Testproject");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);
            var nugetTestService = GetNuGetTestService();
            var uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(packageName, packageVersion);
            solutionService.Build();

            // Assert
            CommonUtility.AssertInstalledPackageByProjectType(VisualStudio, projectTemplate, project, packageName, packageVersion, Logger);
            uiwindow.AssertInstalledPackageDeprecated();

            // Act
            VisualStudio.ClearWindows();
            uiwindow.UninstallPackageFromUI(packageName);

            // Assert
            CommonUtility.AssertUninstalledPackageByProjectType(VisualStudio, projectTemplate, project, packageName, Logger);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task SearchTransitivePackageInInstalledTabFromUI()
        {
            // Arrange
            var transitivePackageName = "Contoso.B";
            await CommonUtility.CreateDependenciesPackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV1, transitivePackageName, TestPackageVersionV1);

            NuGetApexTestService nugetTestService = GetNuGetTestService();

            var solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution("TestSolution", _pathContext.SolutionRoot);
            var project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.NetCoreClassLib, "TestProject");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);

            var uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);
            solutionService.Build();

            CommonUtility.AssertPackageReferenceExists(VisualStudio, project, TestPackageName, TestPackageVersionV1, Logger);
            uiwindow.AssertPackageNameAndType(TestPackageName, NuGet.VisualStudio.PackageLevel.TopLevel);
            uiwindow.AssertPackageNameAndType(transitivePackageName, NuGet.VisualStudio.PackageLevel.Transitive);

            // Act
            uiwindow.SearchPackageFromUI(transitivePackageName);

            // Assert
            VisualStudio.AssertNoErrors();
            uiwindow.AssertPackageNameAndType(transitivePackageName, NuGet.VisualStudio.PackageLevel.Transitive);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task InstallTransitivePackageFromUI()
        {
            // Arrange
            var transitivePackageName = "Contoso.B";
            await CommonUtility.CreateDependenciesPackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV1, transitivePackageName, TestPackageVersionV1);

            NuGetApexTestService nugetTestService = GetNuGetTestService();

            var solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution("TestSolution", _pathContext.SolutionRoot);
            var project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.NetCoreConsoleApp, "TestProject");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);

            var uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);
            solutionService.Build();

            uiwindow.AssertPackageNameAndType(TestPackageName, NuGet.VisualStudio.PackageLevel.TopLevel);
            uiwindow.AssertPackageNameAndType(transitivePackageName, NuGet.VisualStudio.PackageLevel.Transitive);

            // Act
            uiwindow.InstallPackageFromUI(transitivePackageName, TestPackageVersionV1);
            solutionService.Build();

            // Assert
            VisualStudio.AssertNoErrors();
            CommonUtility.AssertPackageReferenceExists(VisualStudio, project, transitivePackageName, TestPackageVersionV1, Logger);
            uiwindow.AssertPackageNameAndType(transitivePackageName, NuGet.VisualStudio.PackageLevel.TopLevel);
        }

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task Uninstall_WithMultiplePackagesThatDependOnEachOther_PackageGoesFromDirectToTransitive()
        {
            // Arrange
            var transitivePackageName = "Contoso.B";
            await CommonUtility.CreateDependenciesPackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV1, transitivePackageName, TestPackageVersionV1);

            NuGetApexTestService nugetTestService = GetNuGetTestService();

            var solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution("TestSolution", _pathContext.SolutionRoot);
            var project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.NetCoreClassLib, "Testproject");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);

            var uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);
            solutionService.Build();

            uiwindow.AssertPackageNameAndType(TestPackageName, NuGet.VisualStudio.PackageLevel.TopLevel);
            uiwindow.AssertPackageNameAndType(transitivePackageName, NuGet.VisualStudio.PackageLevel.Transitive);

            uiwindow.InstallPackageFromUI(transitivePackageName, TestPackageVersionV1);
            solutionService.Build();

            CommonUtility.AssertPackageReferenceExists(VisualStudio, project, transitivePackageName, TestPackageVersionV1, Logger);
            uiwindow.AssertPackageNameAndType(transitivePackageName, NuGet.VisualStudio.PackageLevel.TopLevel);

            // Act
            uiwindow.UninstallPackageFromUI(transitivePackageName);
            solutionService.Build();

            // Assert
            VisualStudio.AssertNoErrors();
            CommonUtility.AssertPackageReferenceDoesNotExist(VisualStudio, project, transitivePackageName, Logger);
            uiwindow.AssertPackageNameAndType(TestPackageName, NuGet.VisualStudio.PackageLevel.TopLevel);
            uiwindow.AssertPackageNameAndType(transitivePackageName, NuGet.VisualStudio.PackageLevel.Transitive);
        }

        [TestMethod]
        [DataRow(ProjectTemplate.NetCoreConsoleApp)]
        [DataRow(ProjectTemplate.ConsoleApplication)]
        [Timeout(DefaultTimeout)]
        public async Task VerifyRestorePackageByRestoreNuGetPackagesContextMenu(ProjectTemplate projectTemplate)
        {
            // Arrange
            string packageFolderPath;
            string installedPackageFolderPath;

            await CommonUtility.CreatePackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV1);

            NuGetApexTestService nugetTestService = GetNuGetTestService();

            var solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution("TestSolution", _pathContext.SolutionRoot);
            var project = solutionService.AddProject(ProjectLanguage.CSharp, projectTemplate, "TestProject");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);

            var uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);
            solutionService.Build();

            if (projectTemplate.Equals(ProjectTemplate.NetCoreConsoleApp))
            {
                packageFolderPath = _pathContext.UserPackagesFolder;
                installedPackageFolderPath = Path.Combine(packageFolderPath, TestPackageName);
            }
            else
            {
                packageFolderPath = _pathContext.PackagesV2;
                installedPackageFolderPath = Path.Combine(packageFolderPath, "Contoso.A.1.0.0");
            }

            Directory.Exists(installedPackageFolderPath).Should().BeTrue();

            // Act
            Directory.Delete(packageFolderPath, true);
            Directory.Exists(packageFolderPath).Should().BeFalse();
            CommonUtility.RestoreNuGetPackages(VisualStudio, Logger);

            // Assert
            CommonUtility.WaitForDirectoryExists(installedPackageFolderPath);
        }

        [TestMethod]
        [DataRow(ProjectTemplate.NetCoreConsoleApp)]
        [DataRow(ProjectTemplate.ConsoleApplication)]
        [Timeout(DefaultTimeout)]
        public async Task VerifyPackageRestoredByBuilding(ProjectTemplate projectTemplate)
        {
            // Arrange
            string packageFolderPath;
            string installedPackageFolderPath;
            await CommonUtility.CreatePackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV1);

            NuGetApexTestService nugetTestService = GetNuGetTestService();

            SolutionService solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution("TestSolution", _pathContext.SolutionRoot);
            ProjectTestExtension project = solutionService.AddProject(ProjectLanguage.CSharp, projectTemplate, "TestProject");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);
            NuGetUIProjectTestExtension uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);

            if (projectTemplate.Equals(ProjectTemplate.NetCoreConsoleApp))
            {
                packageFolderPath = _pathContext.UserPackagesFolder;
                installedPackageFolderPath = Path.Combine(packageFolderPath, TestPackageName);
            }
            else
            {
                packageFolderPath = _pathContext.PackagesV2;
                installedPackageFolderPath = Path.Combine(packageFolderPath, "Contoso.A.1.0.0");
            }

            Directory.Exists(installedPackageFolderPath).Should().BeTrue();

            // Act
            Directory.Delete(packageFolderPath, true);
            Directory.Exists(packageFolderPath).Should().BeFalse();
            solutionService.Build();

            // Assert
            Directory.Exists(installedPackageFolderPath).Should().BeTrue();
        }


        [DataTestMethod]
        [Timeout(DefaultTimeout)]
        public void VerifyPackageNotRestoredAfterDisablingPackageRestore()
        {
            // Arrange
            EnsureVisualStudioHost();

            SolutionService solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution("TestSolution", _pathContext.SolutionRoot);
            solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.IOSLibraryApp, "TestProject");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            // Act
            _pathContext.Settings.DisableAutoRestore();
            CommonUtility.RestoreNuGetPackages(VisualStudio, Logger);
            VisualStudio.ObjectModel.Shell.ToolWindows.ErrorHub.ShowErrors();

            // Assert
            VisualStudio.AssertErrorListContainsSpecificError("NuGet restore is currently disabled.");
        }

        [DataTestMethod]
        [Timeout(DefaultTimeout)]
        public async Task VerifyPackageNotRestoredAfterDisablingAutomaticInPackageRestoreSection()
        {
            // Arrange
            NuGetApexTestService nugetTestService = GetNuGetTestService();

            await CommonUtility.CreatePackageInSourceAsync(_pathContext.PackageSource, TestPackageName, TestPackageVersionV1);

            SolutionService solutionService = VisualStudio.Get<SolutionService>();
            solutionService.CreateEmptySolution("TestSolution", _pathContext.SolutionRoot);
            ProjectTestExtension project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ConsoleApplication, "TestProject");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);
            NuGetUIProjectTestExtension uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI(TestPackageName, TestPackageVersionV1);

            _pathContext.Settings.DisableAutomaticInPackageRestoreSection();

            var installedPackageFolderPath = Path.Combine(_pathContext.PackagesV2, "Contoso.A.1.0.0");
            Directory.Exists(installedPackageFolderPath).Should().BeTrue();
            Directory.Delete(installedPackageFolderPath, true);

            // Act
            solutionService.Build();

            // Assert
            VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
            CommonUtility.WaitForDirectoryNotExists(installedPackageFolderPath);
        }

        public override void Dispose()
        {
            _pathContext.Dispose();

            base.Dispose();
        }
    }
}
