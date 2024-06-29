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


namespace NuGet.Tests.Apex.Daily
{
    [TestClass]
    public class NuGetConsoleTestCase : SharedVisualStudioHostTestClass
    {
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
        [DataRow(ProjectTemplate.ClassLibrary, "PackageA", "1.0.0", "2.0.0", "PackageB", "1.0.1", "2.0.1")]
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


        [DataTestMethod]
        [Timeout(DefaultTimeout)]
        public async Task VerifyCmdFindPackageExactMatchInPMC()
        {
            EnsureVisualStudioHost();
            using (var simpleTestPathContext = new SimpleTestPathContext())
            {
                // Arrange
                var PackageName = "TestPackage";
                var v100 = "1.0.0";
                await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, PackageName, v100);

                using (var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.NetCoreConsoleApp, Logger, addNetStandardFeeds: true, simpleTestPathContext: simpleTestPathContext))
                {
                    SolutionService solutionService = VisualStudio.Get<SolutionService>();
                    var nugetConsole = GetConsole(testContext.Project);

                    // Act
                    nugetConsole.Execute($"find-package {PackageName} -ExactMatch");

                    // Assert
                    string PMCText = nugetConsole.GetText();
                    PMCText.Should().Contain(PackageName);
                    PMCText.Should().Contain(v100);
                }
            }
        }

        [DataTestMethod]
        [Timeout(DefaultTimeout)]
        public async Task VerifyCmdGetPackageUpdateInPMC()
        {
            EnsureVisualStudioHost();
            using (var simpleTestPathContext = new SimpleTestPathContext())
            {
                // Arrange
                var packageName = "TestPackage";
                var v100 = "1.0.0";
                var v200 = "2.0.0";
                await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, v100);
                await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, v200);

                using (var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.NetCoreConsoleApp, Logger, addNetStandardFeeds: true, simpleTestPathContext: simpleTestPathContext))
                {
                    // Arrange
                    SolutionService solutionService = VisualStudio.Get<SolutionService>();
                    var nugetConsole = GetConsole(testContext.Project);

                    nugetConsole.InstallPackageFromPMC(packageName, v100);
                    testContext.SolutionService.Build();
                    testContext.NuGetApexTestService.WaitForAutoRestore();
                    nugetConsole.Clear();

                    // Act
                    nugetConsole.Execute("get-package -update");

                    // Assert
                    string PMCText = nugetConsole.GetText();
                    PMCText.Should().Contain(v200);
                }
            }
        }

        [DataTestMethod]
        [Timeout(DefaultTimeout)]
        public void VerifyCmdGetProjectInPMC()
        {
            EnsureVisualStudioHost();
            using (var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ClassLibrary, Logger))
            {
                // Arrange
                SolutionService solutionService = VisualStudio.Get<SolutionService>();
                var nugetConsole = GetConsole(testContext.Project);

                //Act
                nugetConsole.Execute("Get-Project");

                // Assert
                string PMCText = nugetConsole.GetText();
                PMCText.Should().Contain(testContext.Project.Name);
                PMCText.Should().Contain("C#");
                PMCText.Should().Contain(testContext.Project.FullPath);
            }
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

        public static IEnumerable<object[]> GetIOSTemplates()
        {
            yield return new object[] { ProjectTemplate.IOSLibraryApp };
        }
    }
}
