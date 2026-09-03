// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Test.Apex.VisualStudio.Solution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet.Test.Utility;

namespace NuGet.Tests.Apex
{
    [TestClass]
    public class PackageReferenceTestCase : SharedVisualStudioHostTestClass
    {
        [DataTestMethod]
        [DynamicData(nameof(GetPackageReferenceTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task InstallPackage_PMC(ProjectTemplate projectTemplate)
        {
            using (var simpleTestPathContext = new SimpleTestPathContext())
            {
                // Arrange
                var packageName = "TestPackage";
                var packageVersion = "1.0.0";
                await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, packageVersion);
                EnsurePackageReferenceFormat(simpleTestPathContext, projectTemplate);

                using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, simpleTestPathContext: simpleTestPathContext))
                {
                    VisualStudio.AssertNoErrors();
                    testContext.SolutionService.Build();

                    // Act
                    var nugetConsole = GetConsole(testContext.Project);

                    nugetConsole.InstallPackageFromPMC(packageName, packageVersion);
                    testContext.SolutionService.Build();

                    // Assert
                    VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
                    CommonUtility.AssertPackageInAssetsFile(VisualStudio, testContext.Project, packageName, packageVersion, Logger);
                    Assert.IsTrue(VisualStudio.HasNoErrorsInOutputWindows());
                }
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackageReferenceTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task UpdatePackage_PMC(ProjectTemplate projectTemplate)
        {
            using (var simpleTestPathContext = new SimpleTestPathContext())
            {
                // Arrange
                var packageName = "TestPackage";
                var packageVersion1 = "1.0.0";
                var packageVersion2 = "2.0.0";

                await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, packageVersion1);
                await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, packageVersion2);
                EnsurePackageReferenceFormat(simpleTestPathContext, projectTemplate);

                using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, simpleTestPathContext: simpleTestPathContext))
                {
                    VisualStudio.AssertNoErrors();
                    testContext.SolutionService.Build();

                    // Act
                    var nugetConsole = GetConsole(testContext.Project);

                    nugetConsole.InstallPackageFromPMC(packageName, packageVersion1);
                    testContext.SolutionService.Build();

                    nugetConsole.UpdatePackageFromPMC(packageName, packageVersion2);
                    testContext.SolutionService.Build();

                    // Assert
                    VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
                    CommonUtility.AssertPackageInAssetsFile(VisualStudio, testContext.Project, packageName, packageVersion2, Logger);
                    Assert.IsTrue(VisualStudio.HasNoErrorsInOutputWindows());
                }
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackageReferenceTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task UninstallPackage_PMC(ProjectTemplate projectTemplate)
        {
            using (var simpleTestPathContext = new SimpleTestPathContext())
            {
                // Arrange
                var packageName = "TestPackage";
                var packageVersion = "1.0.0";

                await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, packageVersion);
                EnsurePackageReferenceFormat(simpleTestPathContext, projectTemplate);

                using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, simpleTestPathContext: simpleTestPathContext))
                {
                    VisualStudio.AssertNoErrors();
                    testContext.SolutionService.Build();

                    // Act
                    var nugetConsole = GetConsole(testContext.Project);

                    nugetConsole.InstallPackageFromPMC(packageName, packageVersion);
                    testContext.SolutionService.Build();

                    nugetConsole.UninstallPackageFromPMC(packageName);
                    testContext.SolutionService.Build();

                    // Assert
                    VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
                    CommonUtility.AssertPackageNotInAssetsFile(VisualStudio, testContext.Project, packageName, packageVersion, Logger);
                    Assert.IsTrue(VisualStudio.HasNoErrorsInOutputWindows());
                }
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackageReferenceTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task InstallMultiplePackages_PMC(ProjectTemplate projectTemplate)
        {
            using (var simpleTestPathContext = new SimpleTestPathContext())
            {
                // Arrange
                var packageName1 = "TestPackage1";
                var packageVersion1 = "1.0.0";
                var packageName2 = "TestPackage2";
                var packageVersion2 = "1.2.3";

                await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName1, packageVersion1);
                await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName2, packageVersion2);
                EnsurePackageReferenceFormat(simpleTestPathContext, projectTemplate);

                using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, simpleTestPathContext: simpleTestPathContext))
                {
                    VisualStudio.AssertNoErrors();
                    testContext.SolutionService.Build();

                    // Act
                    var nugetConsole = GetConsole(testContext.Project);

                    nugetConsole.InstallPackageFromPMC(packageName1, packageVersion1);
                    testContext.SolutionService.Build();

                    nugetConsole.InstallPackageFromPMC(packageName2, packageVersion2);
                    testContext.SolutionService.Build();

                    // Assert
                    VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
                    CommonUtility.AssertPackageInAssetsFile(VisualStudio, testContext.Project, packageName1, packageVersion1, Logger);
                    CommonUtility.AssertPackageInAssetsFile(VisualStudio, testContext.Project, packageName2, packageVersion2, Logger);
                    Assert.IsTrue(VisualStudio.HasNoErrorsInOutputWindows());
                }
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackageReferenceTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task InstallAndUninstallAllPackages_PMC(ProjectTemplate projectTemplate)
        {
            using (var simpleTestPathContext = new SimpleTestPathContext())
            {
                // Arrange
                var packageName1 = "TestPackage1";
                var packageVersion1 = "1.0.0";
                var packageName2 = "TestPackage2";
                var packageVersion2 = "1.2.3";

                await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName1, packageVersion1);
                await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName2, packageVersion2);
                EnsurePackageReferenceFormat(simpleTestPathContext, projectTemplate);

                using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, simpleTestPathContext: simpleTestPathContext))
                {
                    VisualStudio.AssertNoErrors();
                    testContext.SolutionService.Build();

                    var nugetConsole = GetConsole(testContext.Project);

                    // Act - Install both
                    nugetConsole.InstallPackageFromPMC(packageName1, packageVersion1);
                    testContext.SolutionService.Build();

                    nugetConsole.InstallPackageFromPMC(packageName2, packageVersion2);
                    testContext.SolutionService.Build();

                    // Act - Uninstall both
                    nugetConsole.UninstallPackageFromPMC(packageName1);
                    testContext.SolutionService.Build();

                    nugetConsole.UninstallPackageFromPMC(packageName2);
                    testContext.SolutionService.Build();

                    // Assert
                    VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
                    CommonUtility.AssertPackageNotInAssetsFile(VisualStudio, testContext.Project, packageName1, packageVersion1, Logger);
                    CommonUtility.AssertPackageNotInAssetsFile(VisualStudio, testContext.Project, packageName2, packageVersion2, Logger);
                    Assert.IsTrue(VisualStudio.HasNoErrorsInOutputWindows());
                }
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackageReferenceTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task CleanDeletesCacheFile(ProjectTemplate projectTemplate)
        {
            using (var simpleTestPathContext = new SimpleTestPathContext())
            {
                // Arrange
                var packageName = "TestPackage";
                var packageVersion = "1.0.0";
                await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, packageVersion);
                EnsurePackageReferenceFormat(simpleTestPathContext, projectTemplate);

                using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, simpleTestPathContext: simpleTestPathContext))
                {
                    var nugetConsole = GetConsole(testContext.Project);

                    nugetConsole.InstallPackageFromPMC(packageName, packageVersion);
                    testContext.SolutionService.Build();

                    FileInfo cacheFilePath = CommonUtility.GetCacheFilePath(testContext.Project.FullPath);
                    CommonUtility.WaitForFileExists(cacheFilePath);

                    // Act
                    testContext.Project.Clean();

                    // Assert
                    CommonUtility.WaitForFileNotExists(cacheFilePath);
                }
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackageReferenceTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task RebuildDoesNotDeleteCacheFile(ProjectTemplate projectTemplate)
        {
            using (var simpleTestPathContext = new SimpleTestPathContext())
            {
                // Arrange
                var packageName = "TestPackage";
                var packageVersion = "1.0.0";
                await CommonUtility.CreatePackageInSourceAsync(simpleTestPathContext.PackageSource, packageName, packageVersion);
                EnsurePackageReferenceFormat(simpleTestPathContext, projectTemplate);

                using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, simpleTestPathContext: simpleTestPathContext))
                {
                    var nugetConsole = GetConsole(testContext.Project);

                    nugetConsole.InstallPackageFromPMC(packageName, packageVersion);
                    testContext.SolutionService.Build();

                    FileInfo cacheFilePath = CommonUtility.GetCacheFilePath(testContext.Project.FullPath);
                    CommonUtility.WaitForFileExists(cacheFilePath);

                    // Act
                    testContext.Project.Rebuild();

                    // Assert
                    CommonUtility.WaitForFileExists(cacheFilePath);
                }
            }
        }

        /// <summary>
        /// For legacy (non-SDK) projects like ConsoleApplication, sets the default package format to PackageReference.
        /// SDK-style projects (e.g. NetCoreConsoleApp) are always PackageReference, so this is a no-op for them.
        /// </summary>
        private static void EnsurePackageReferenceFormat(SimpleTestPathContext simpleTestPathContext, ProjectTemplate projectTemplate)
        {
            if (projectTemplate == ProjectTemplate.ConsoleApplication)
            {
                simpleTestPathContext.Settings.SetPackageFormatToPackageReference();
            }
        }

        public static IEnumerable<object[]> GetPackageReferenceTemplates()
        {
            // Legacy PackageReference: classic (non-SDK) csproj, requires SetPackageFormatToPackageReference() before project creation.
            yield return new object[] { ProjectTemplate.ConsoleApplication };
            // SDK-style: inherently PackageReference.
            yield return new object[] { ProjectTemplate.NetCoreConsoleApp };
        }
    }
}
