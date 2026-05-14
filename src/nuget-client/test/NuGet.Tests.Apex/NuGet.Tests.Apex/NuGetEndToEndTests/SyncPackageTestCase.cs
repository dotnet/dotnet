// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.Test.Apex.VisualStudio.Solution;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NuGet.Tests.Apex
{
    [TestClass]
    public class SyncPackageTestCase : SharedVisualStudioHostTestClass
    {
        // Migrated from Test-SyncPackagesInSolutionUp in SyncPackageTest.ps1
        // Syncs package A from the project with the higher version to all other projects.
        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task SyncPackageFromPMC_UpVersion_SyncsAllProjectsAsync()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ConsoleApplication, Logger);

            var packageName = "A";
            var packageVersion1 = "1.0.0";
            var packageVersion2 = "2.0.0";
            var dependencyName = "B";
            var dependencyVersion1 = "1.0.0";
            var dependencyVersion2 = "2.0.0";

            await CommonUtility.CreateDependenciesPackageInSourceAsync(testContext.PackageSource, packageName, packageVersion1, dependencyName, dependencyVersion1);
            await CommonUtility.CreateDependenciesPackageInSourceAsync(testContext.PackageSource, packageName, packageVersion2, dependencyName, dependencyVersion2);

            var project2 = testContext.SolutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, CommonUtility.DefaultTargetFramework, "TestProject2");
            project2.Build();
            testContext.SolutionService.Save();

            var nugetConsole = GetConsole(testContext.Project);

            // Install A 1.0.0 into project1 and A 2.0.0 into project2
            nugetConsole.Execute($"Install-Package {packageName} -ProjectName {testContext.Project.Name} -Version {packageVersion1} -Source {testContext.PackageSource}");
            nugetConsole.Execute($"Install-Package {packageName} -ProjectName {project2.Name} -Version {packageVersion2} -Source {testContext.PackageSource}");

            // Pre-conditions
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion1, Logger);
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, dependencyName, dependencyVersion1, Logger);
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project2, packageName, packageVersion2, Logger);
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project2, dependencyName, dependencyVersion2, Logger);

            // Sync from project2 (which has A 2.0.0)
            nugetConsole.Execute($"Sync-Package -ProjectName {project2.Name} -Id {packageName} -Source {testContext.PackageSource}");

            // Assert: project1 should now have A 2.0.0 and B 2.0.0
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion2, Logger);
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, dependencyName, dependencyVersion2, Logger);
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project2, packageName, packageVersion2, Logger);
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project2, dependencyName, dependencyVersion2, Logger);

            // Old versions should no longer be installed in project1
            CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion1, Logger);
            CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, testContext.Project, dependencyName, dependencyVersion1, Logger);
        }

        // Migrated from Test-SyncPackagesInSolutionDown in SyncPackageTest.ps1
        // Syncs package A from the project with the lower version to all other projects.
        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task SyncPackageFromPMC_DownVersion_SyncsAllProjectsAsync()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ConsoleApplication, Logger);

            var packageName = "A";
            var packageVersion1 = "1.0.0";
            var packageVersion2 = "2.0.0";
            var dependencyName = "B";
            var dependencyVersion1 = "1.0.0";
            var dependencyVersion2 = "2.0.0";

            await CommonUtility.CreateDependenciesPackageInSourceAsync(testContext.PackageSource, packageName, packageVersion1, dependencyName, dependencyVersion1);
            await CommonUtility.CreateDependenciesPackageInSourceAsync(testContext.PackageSource, packageName, packageVersion2, dependencyName, dependencyVersion2);

            var project2 = testContext.SolutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, CommonUtility.DefaultTargetFramework, "TestProject2");
            project2.Build();
            testContext.SolutionService.Save();

            var nugetConsole = GetConsole(testContext.Project);

            // Install A 1.0.0 into project1 and A 2.0.0 into project2
            nugetConsole.Execute($"Install-Package {packageName} -ProjectName {testContext.Project.Name} -Version {packageVersion1} -Source {testContext.PackageSource}");
            nugetConsole.Execute($"Install-Package {packageName} -ProjectName {project2.Name} -Version {packageVersion2} -Source {testContext.PackageSource}");

            // Pre-conditions
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion1, Logger);
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, dependencyName, dependencyVersion1, Logger);
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project2, packageName, packageVersion2, Logger);
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project2, dependencyName, dependencyVersion2, Logger);

            // Sync from project1 (which has A 1.0.0)
            nugetConsole.Execute($"Sync-Package -ProjectName {testContext.Project.Name} -Id {packageName} -Source {testContext.PackageSource}");

            // Assert: project1 stays at A 1.0.0 / B 1.0.0
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, packageName, packageVersion1, Logger);
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, dependencyName, dependencyVersion1, Logger);
            // project2 should now have A 1.0.0, but B stays at 2.0.0 (dependency is not downgraded)
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project2, packageName, packageVersion1, Logger);
            CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project2, dependencyName, dependencyVersion2, Logger);

            // Old version of A should no longer be installed in project2
            CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, project2, packageName, packageVersion2, Logger);
        }

        // Migrated from Test-SyncPackagesInSolutionPlural in SyncPackageTest.ps1
        // Syncs package A from a middle version across five projects.
        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task SyncPackageFromPMC_MultipleProjects_SyncsAllProjectsAsync()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ConsoleApplication, Logger);

            var packageName = "A";
            var dependencyName = "B";

            // Create packages A v1-5, each depending on B v1-5
            for (int i = 1; i <= 5; i++)
            {
                await CommonUtility.CreateDependenciesPackageInSourceAsync(
                    testContext.PackageSource, packageName, $"{i}.0.0", dependencyName, $"{i}.0.0");
            }

            // Create 4 additional projects (project1 is testContext.Project)
            var project2 = testContext.SolutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, CommonUtility.DefaultTargetFramework, "TestProject2");
            project2.Build();
            var project3 = testContext.SolutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, CommonUtility.DefaultTargetFramework, "TestProject3");
            project3.Build();
            var project4 = testContext.SolutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, CommonUtility.DefaultTargetFramework, "TestProject4");
            project4.Build();
            var project5 = testContext.SolutionService.AddProject(ProjectLanguage.CSharp, ProjectTemplate.ClassLibrary, CommonUtility.DefaultTargetFramework, "TestProject5");
            project5.Build();
            testContext.SolutionService.Save();

            var nugetConsole = GetConsole(testContext.Project);

            // Install different versions of A into each project
            var projects = new[] { testContext.Project, project2, project3, project4, project5 };
            for (int i = 0; i < projects.Length; i++)
            {
                var version = $"{i + 1}.0.0";
                nugetConsole.Execute($"Install-Package {packageName} -ProjectName {projects[i].Name} -Version {version} -Source {testContext.PackageSource}");
            }

            // Pre-conditions: each project has the expected version of A
            for (int i = 0; i < projects.Length; i++)
            {
                CommonUtility.AssertPackageInPackagesConfig(VisualStudio, projects[i], packageName, $"{i + 1}.0.0", Logger);
            }

            // Sync from project3 (which has A 3.0.0)
            nugetConsole.Execute($"Sync-Package -ProjectName {project3.Name} -Id {packageName} -Source {testContext.PackageSource}");

            // Assert: all projects should now have A 3.0.0
            foreach (var project in projects)
            {
                CommonUtility.AssertPackageInPackagesConfig(VisualStudio, project, packageName, "3.0.0", Logger);
            }

            // Old versions of A should no longer be installed
            CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, testContext.Project, packageName, "1.0.0", Logger);
            CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, project2, packageName, "2.0.0", Logger);
            CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, project4, packageName, "4.0.0", Logger);
            CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, project5, packageName, "5.0.0", Logger);
        }
    }
}
