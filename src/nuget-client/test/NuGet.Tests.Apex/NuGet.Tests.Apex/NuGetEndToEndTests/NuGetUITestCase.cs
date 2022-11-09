// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.Test.Apex.VisualStudio.Solution;
using Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.Tests.Apex
{
    public class NuGetUITestCase : SharedVisualStudioHostTestClass, IClassFixture<VisualStudioHostFixtureFactory>
    {
        private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);
        private static readonly TimeSpan Interval = TimeSpan.FromSeconds(2);

        public NuGetUITestCase(VisualStudioHostFixtureFactory visualStudioHostFixtureFactory, ITestOutputHelper output)
            : base(visualStudioHostFixtureFactory, output)
        {
        }

        [StaFact]
        public void SearchPackageFromUI()
        {
            // Arrange
            EnsureVisualStudioHost();
            var dte = VisualStudio.Dte;
            var solutionService = VisualStudio.Get<SolutionService>();

            solutionService.CreateEmptySolution();
            var project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);
            var nugetTestService = GetNuGetTestService();
            var uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.SwitchTabToBrowse();
            uiwindow.SeachPackgeFromUI("newtonsoft.json");

            // Assert
            VisualStudio.AssertNoErrors();
        }

        [StaFact]
        public void InstallPackageFromUI()
        {
            // Arrange
            EnsureVisualStudioHost();
            var dte = VisualStudio.Dte;
            var solutionService = VisualStudio.Get<SolutionService>();

            solutionService.CreateEmptySolution();
            var project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);
            var nugetTestService = GetNuGetTestService();
            var uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI("newtonsoft.json", "9.0.1");

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project, "newtonsoft.json", "9.0.1", XunitLogger);
        }

        [StaFact]
        public void InstallPackageToProjectsFromUI()
        {
            // Arrange
            EnsureVisualStudioHost();
            var dte = VisualStudio.Dte;
            var solutionService = VisualStudio.Get<SolutionService>();

            solutionService.CreateEmptySolution();
            var project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            var nuProject = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "NuProject");
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);
            var nugetTestService = GetNuGetTestService();
            var uiwindow = nugetTestService.GetUIWindowfromProject(nuProject);
            uiwindow.InstallPackageFromUI("newtonsoft.json", "9.0.1");
            VisualStudio.SelectProjectInSolutionExplorer(project.Name);
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);

            VisualStudio.ClearOutputWindow();
            var uiwindow2 = nugetTestService.GetUIWindowfromProject(project);
            uiwindow2.InstallPackageFromUI("newtonsoft.json", "9.0.1");

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project, "newtonsoft.json", "9.0.1", XunitLogger);
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, nuProject, "newtonsoft.json", "9.0.1", XunitLogger);
        }

        [StaFact]
        public void UninstallPackageFromUI()
        {
            // Arrange
            EnsureVisualStudioHost();
            var dte = VisualStudio.Dte;
            var solutionService = VisualStudio.Get<SolutionService>();

            solutionService.CreateEmptySolution();
            var project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            solutionService.SaveAll();

            FileInfo packagesConfigFile = GetPackagesConfigFile(project);
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);
            var nugetTestService = GetNuGetTestService();
            var uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI("newtonsoft.json", "9.0.1");

            WaitForFileExists(packagesConfigFile);

            VisualStudio.ClearWindows();

            // Act
            uiwindow.UninstallPackageFromUI("newtonsoft.json");

            WaitForFileNotExists(packagesConfigFile);

            // Assert
            CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, project, "newtonsoft.json", XunitLogger);
        }

        [StaFact]
        public void UpdatePackageFromUI()
        {
            // Arrange
            EnsureVisualStudioHost();
            var dte = VisualStudio.Dte;
            var solutionService = VisualStudio.Get<SolutionService>();

            solutionService.CreateEmptySolution();
            var project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            VisualStudio.ClearWindows();
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);
            var nugetTestService = GetNuGetTestService();
            var uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI("newtonsoft.json", "9.0.1");

            // Act
            VisualStudio.ClearWindows();
            uiwindow.UpdatePackageFromUI("newtonsoft.json", "10.0.3");

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project, "newtonsoft.json", "10.0.3", XunitLogger);
        }

        [StaFact]
        public async Task InstallPackageFromUI_PC_PackageSourceMapping_WithSingleFeed_Match_Succeeds()
        {
            // Arrange
            EnsureVisualStudioHost();
            var solutionService = VisualStudio.Get<SolutionService>();
            string solutionDirectory = CommonUtility.CreateSolutionDirectory(Directory.GetCurrentDirectory());
            var privateRepositoryPath = Path.Combine(solutionDirectory, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            var packageName = "Contoso.A";
            var packageVersion = "1.0.0";

            await CommonUtility.CreatePackageInSourceAsync(privateRepositoryPath, packageName, packageVersion);

            // Create nuget.config with Package source mapping filtering rules before project is created.
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

            solutionService.CreateEmptySolution("TestSolution", solutionDirectory);

            var project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);
            var nugetTestService = GetNuGetTestService();
            var uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI("contoso.a", "1.0.0");

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project, "Contoso.A", "1.0.0", XunitLogger);
        }

        [StaFact]
        public async Task InstallPackageToProjectsFromUI_PC_PackageSourceMapping_WithSingleFeed_Match_Succeeds()
        {
            // Arrange
            EnsureVisualStudioHost();
            var solutionService = VisualStudio.Get<SolutionService>();
            string solutionDirectory = CommonUtility.CreateSolutionDirectory(Directory.GetCurrentDirectory());
            var privateRepositoryPath = Path.Combine(solutionDirectory, "PrivateRepository");

            Directory.CreateDirectory(privateRepositoryPath);

            var packageName = "Contoso.A";
            var packageVersion = "1.0.0";

            await CommonUtility.CreatePackageInSourceAsync(privateRepositoryPath, packageName, packageVersion);

            // Create nuget.config with Package source mapping filtering rules before project is created.
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

            solutionService.CreateEmptySolution("TestSolution", solutionDirectory);

            var project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            var nuProject = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "NuProject");
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);
            var nugetTestService = GetNuGetTestService();
            var uiwindow = nugetTestService.GetUIWindowfromProject(nuProject);
            uiwindow.InstallPackageFromUI("contoso.a", "1.0.0");
            VisualStudio.SelectProjectInSolutionExplorer(project.Name);
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);

            VisualStudio.ClearOutputWindow();
            var uiwindow2 = nugetTestService.GetUIWindowfromProject(project);
            uiwindow2.InstallPackageFromUI("contoso.a", "1.0.0");

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project, "contoso.a", "1.0.0", XunitLogger);
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, nuProject, "contoso.a", "1.0.0", XunitLogger);
        }

        [StaFact]
        public async Task InstallPackageFromUI_PC_PackageSourceMapping_WithMultiFeed_Succeed()
        {
            // Arrange
            EnsureVisualStudioHost();
            var solutionService = VisualStudio.Get<SolutionService>();
            string solutionDirectory = CommonUtility.CreateSolutionDirectory(Directory.GetCurrentDirectory());
            var externalRepositoryPath = Path.Combine(solutionDirectory, "ExternalRepository");
            Directory.CreateDirectory(externalRepositoryPath);
            var privateRepositoryPath = Path.Combine(solutionDirectory, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            // Create nuget.config with Package source mapping filtering rules before project is created.
            CommonUtility.CreateConfigurationFile(Path.Combine(solutionDirectory, "NuGet.config"), $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""encyclopaedia"" value=""{externalRepositoryPath}"" />
    <add key=""encyclopædia"" value=""{privateRepositoryPath}"" />
    </packageSources>
    <packageSourceMapping>
        <packageSource key=""encyclopaedia"">
            <package pattern=""External.*"" />
            <package pattern=""Others.*"" />
        </packageSource>
        <packageSource key=""encyclopædia"">
            <package pattern=""Contoso.*"" />
            <package pattern=""Test.*"" />
        </packageSource>
    </packageSourceMapping>
//</configuration>");

            solutionService.CreateEmptySolution("TestSolution", solutionDirectory);
            var packageName = "Contoso.A";
            var packageVersion1 = "1.0.0";
            await CommonUtility.CreatePackageInSourceAsync(externalRepositoryPath, packageName, packageVersion1);
            await CommonUtility.CreatePackageInSourceAsync(privateRepositoryPath, packageName, packageVersion1);
            var project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);
            var nugetTestService = GetNuGetTestService();
            var uiwindow = nugetTestService.GetUIWindowfromProject(project);

            // Set option to package source option to All
            uiwindow.SetPackageSourceOptionToAll();
            uiwindow.InstallPackageFromUI("contoso.a", "1.0.0");

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project, "contoso.a", XunitLogger);
        }

        [StaFact]
        public async Task InstallPackageFromUI_PC_PackageSourceMapping_WithMultiFeed_Fails()
        {
            // Arrange
            EnsureVisualStudioHost();
            var solutionService = VisualStudio.Get<SolutionService>();
            string solutionDirectory = CommonUtility.CreateSolutionDirectory(Directory.GetCurrentDirectory());
            var externalRepositoryPath = Path.Combine(solutionDirectory, "ExternalRepository");
            Directory.CreateDirectory(externalRepositoryPath);
            var privateRepositoryPath = Path.Combine(solutionDirectory, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            var packageName = "Contoso.A";
            var packageVersion = "1.0.0";
            await CommonUtility.CreatePackageInSourceAsync(externalRepositoryPath, packageName, packageVersion);

            // Create nuget.config with Package source mapping filtering rules before project is created.
            CommonUtility.CreateConfigurationFile(Path.Combine(solutionDirectory, "NuGet.config"), $@"<?xml version=""1.0"" encoding=""utf-8""?>
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

            solutionService.CreateEmptySolution("TestSolution", solutionDirectory);

            var project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);
            var nugetTestService = GetNuGetTestService();
            var uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI("contoso.a", "1.0.0");

            // Assert
            // Even though Contoso.a exist in ExternalRepository, but packageSourceMapping filter doesn't let restore from it.
            CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, project, "contoso.a", XunitLogger);
        }

        [StaFact]
        public async Task UpdatePackageFromUI_PC_PackageSourceMapping_WithSingleFeed_Succeeds()
        {
            // Arrange
            EnsureVisualStudioHost();
            var solutionService = VisualStudio.Get<SolutionService>();
            string solutionDirectory = CommonUtility.CreateSolutionDirectory(Directory.GetCurrentDirectory());
            var privateRepositoryPath = Path.Combine(solutionDirectory, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            var packageName = "Contoso.A";
            var packageVersionV1 = "1.0.0";
            var packageVersionV2 = "2.0.0";

            await CommonUtility.CreatePackageInSourceAsync(privateRepositoryPath, packageName, packageVersionV1);
            await CommonUtility.CreatePackageInSourceAsync(privateRepositoryPath, packageName, packageVersionV2);

            // Create nuget.config with Package source mapping filtering rules before project is created.
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

            solutionService.CreateEmptySolution("TestSolution", solutionDirectory);

            var project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);
            var nugetTestService = GetNuGetTestService();
            var uiwindow = nugetTestService.GetUIWindowfromProject(project);
            uiwindow.InstallPackageFromUI("contoso.a", "1.0.0");

            // Act
            VisualStudio.ClearWindows();
            uiwindow.UpdatePackageFromUI("contoso.a", "2.0.0");

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project, "contoso.a", "2.0.0", XunitLogger);
        }


        [StaFact]
        public async Task UpdatePackageFromUI_PC_PackageSourceMapping_WithMultiFeed_Succeed()
        {
            // Arrange
            EnsureVisualStudioHost();
            var solutionService = VisualStudio.Get<SolutionService>();
            string solutionDirectory = CommonUtility.CreateSolutionDirectory(Directory.GetCurrentDirectory());
            var externalRepositoryPath = Path.Combine(solutionDirectory, "ExternalRepository");
            Directory.CreateDirectory(externalRepositoryPath);
            var privateRepositoryPath = Path.Combine(solutionDirectory, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            // Create nuget.config with Package source mapping filtering rules before project is created.
            CommonUtility.CreateConfigurationFile(Path.Combine(solutionDirectory, "NuGet.config"), $@"<?xml version=""1.0"" encoding=""utf-8""?>
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
//</configuration>");

            solutionService.CreateEmptySolution("TestSolution", solutionDirectory);

            var packageName = "Contoso.A";
            var packageVersion1 = "1.0.0";
            var packageVersion2 = "2.0.0";
            await CommonUtility.CreatePackageInSourceAsync(externalRepositoryPath, packageName, packageVersion1);
            await CommonUtility.CreatePackageInSourceAsync(externalRepositoryPath, packageName, packageVersion2);
            await CommonUtility.CreatePackageInSourceAsync(privateRepositoryPath, packageName, packageVersion1);
            await CommonUtility.CreatePackageInSourceAsync(privateRepositoryPath, packageName, packageVersion2);
            var project = solutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, ProjectTargetFramework.V46, "TestProject");
            VisualStudio.ClearOutputWindow();
            solutionService.SaveAll();

            // Act
            CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, XunitLogger);
            var nugetTestService = GetNuGetTestService();
            var uiwindow = nugetTestService.GetUIWindowfromProject(project);

            // Set option to package source option to All
            uiwindow.SetPackageSourceOptionToAll();
            uiwindow.InstallPackageFromUI("contoso.a", "1.0.0");

            // Act
            uiwindow.InstallPackageFromUI("contoso.a", "2.0.0");

            // Assert
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project, "contoso.a", "2.0.0", XunitLogger);
        }

        private static FileInfo GetPackagesConfigFile(ProjectTestExtension project)
        {
            var projectFile = new FileInfo(project.FullPath);

            return new FileInfo(Path.Combine(projectFile.DirectoryName, "packages.config"));
        }

        private static void WaitForFileExists(FileInfo file)
        {
            Omni.Common.WaitFor.IsTrue(
                () => File.Exists(file.FullName),
                Timeout,
                Interval,
                $"{file.FullName} did not exist within {Timeout}.");
        }

        private static void WaitForFileNotExists(FileInfo file)
        {
            Omni.Common.WaitFor.IsTrue(
                () => !File.Exists(file.FullName),
                Timeout,
                Interval,
                $"{file.FullName} still existed after {Timeout}.");
        }
    }
}
