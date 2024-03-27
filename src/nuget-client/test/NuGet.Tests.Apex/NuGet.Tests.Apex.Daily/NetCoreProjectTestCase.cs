// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Test.Apex.VisualStudio.Solution;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NuGet.Tests.Apex.Daily
{
    [TestClass]
    public class NetCoreProjectTestCase : SharedVisualStudioHostTestClass
    {
        [DataTestMethod]
        [DynamicData(nameof(GetNetCoreTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task InstallPackageToNetCoreProjectFromUI(ProjectTemplate projectTemplate)
        {
            EnsureVisualStudioHost();

            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, addNetStandardFeeds: true))
            {
                // Arrange
                var packageName = "NetCoreInstallTestPackage";
                var packageVersion = "1.0.0";
                await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion);

                VisualStudio.AssertNoErrors();

                // Act
                CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);
                var nugetTestService = GetNuGetTestService();
                var uiwindow = nugetTestService.GetUIWindowfromProject(testContext.Project);
                uiwindow.InstallPackageFromUI(packageName, packageVersion);

                // Assert
                VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
                CommonUtility.AssertPackageReferenceExists(VisualStudio, testContext.Project, packageName, packageVersion, Logger);
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetNetCoreTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task UpdatePackageToNetCoreProjectFromUI(ProjectTemplate projectTemplate)
        {
            EnsureVisualStudioHost();

            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, addNetStandardFeeds: true))
            {
                // Arrange
                var packageName = "NetCoreUpdateTestPackage";
                var packageVersion1 = "1.0.0";
                var packageVersion2 = "2.0.0";

                await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion1);
                await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion2);

                VisualStudio.AssertNoErrors();

                // Act
                CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);
                var nugetTestService = GetNuGetTestService();
                var uiwindow = nugetTestService.GetUIWindowfromProject(testContext.Project);
                uiwindow.InstallPackageFromUI(packageName, packageVersion1);
                testContext.SolutionService.Build();
                testContext.NuGetApexTestService.WaitForAutoRestore();

                uiwindow.UpdatePackageFromUI(packageName, packageVersion2);
                testContext.SolutionService.Build();
                testContext.NuGetApexTestService.WaitForAutoRestore();

                // Assert
                VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
                CommonUtility.AssertPackageReferenceExists(VisualStudio, testContext.Project, packageName, packageVersion2, Logger);
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetNetCoreTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task UninstallPackageFromNetCoreProjectFromUI(ProjectTemplate projectTemplate)
        {
            EnsureVisualStudioHost();

            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger, addNetStandardFeeds: true))
            {
                // Arrange
                var packageName = "NetCoreUninstallTestPackage";
                var packageVersion = "1.0.0";

                await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, packageVersion);

                VisualStudio.AssertNoErrors();

                // Act
                CommonUtility.OpenNuGetPackageManagerWithDte(VisualStudio, Logger);
                var nugetTestService = GetNuGetTestService();
                var uiwindow = nugetTestService.GetUIWindowfromProject(testContext.Project);
                uiwindow.InstallPackageFromUI(packageName, packageVersion);
                testContext.SolutionService.Build();
                testContext.NuGetApexTestService.WaitForAutoRestore();

                uiwindow.UninstallPackageFromUI(packageName);
                testContext.SolutionService.Build();
                testContext.NuGetApexTestService.WaitForAutoRestore();

                // Assert
                VisualStudio.AssertNuGetOutputDoesNotHaveErrors();
                CommonUtility.AssertPackageReferenceDoesNotExist(VisualStudio, testContext.Project, packageName, Logger);
            }
        }

        public static IEnumerable<object[]> GetNetCoreTemplates()
        {
            yield return new object[] { ProjectTemplate.NetStandardClassLib };
        }
    }
}
