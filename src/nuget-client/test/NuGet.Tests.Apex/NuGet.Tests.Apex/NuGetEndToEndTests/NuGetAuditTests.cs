// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Test.Apex.VisualStudio.Shell;
using Microsoft.Test.Apex.VisualStudio.Solution;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet.Protocol;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;

namespace NuGet.Tests.Apex.NuGetEndToEndTests
{
    [TestClass]
    public class NuGetAuditTests : SharedVisualStudioHostTestClass
    {
        private const string TestPackageName = "Contoso.A";
        private const string TestPackageVersionV1 = "1.0.0";
        private const string TestPackageVersionV2 = "2.0.0";

        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task PackagesConfig_SuppressAdvisory()
        {
            // 1. Create Directory.Build.props with suppression for package.A cve1
            // 2. Create mock server with package.A with cve1 and cve2
            // 3. Add mock server to nuget.config
            // 3. Create packages.config project
            // 4. Install package.A
            // 5. check error list to see if only cve2 is listed

            // Arrange
            SimpleTestPathContext testPathContext = new();
            var dbpContents = @"<Project>
    <ItemGroup>
        <NuGetAuditSuppress Include=""https://cve.test/1"" />
    </ItemGroup>
</Project>";
            File.WriteAllText(Path.Combine(testPathContext.SolutionRoot, "Directory.Build.props"), dbpContents);

            using var mockServer = new FileSystemBackedV3MockServer(testPathContext.PackageSource, sourceReportsVulnerabilities: true);
            mockServer.Vulnerabilities.Add("contoso.a", new System.Collections.Generic.List<(Uri, PackageVulnerabilitySeverity, VersionRange)>
            {
                (new Uri("https://cve.test/1"), PackageVulnerabilitySeverity.High, VersionRange.Parse("(, 2.0.0)")),
                (new Uri("https://cve.test/2"), PackageVulnerabilitySeverity.High, VersionRange.Parse("(, 2.0.0)")),
            });

            await CommonUtility.CreatePackageInSourceAsync(testPathContext.PackageSource, TestPackageName, TestPackageVersionV1);

            mockServer.Start();

            testPathContext.Settings.AddSource("auditSource", mockServer.ServiceIndexUri, allowInsecureConnectionsValue: "true");

            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ConsoleApplication, Logger, addNetStandardFeeds: true, simpleTestPathContext: testPathContext);

            var errorListService = VisualStudio.Get<ErrorListService>();
            errorListService.ShowWarnings();

            // Act
            testContext.NuGetApexTestService.InstallPackage(testContext.Project.UniqueName, TestPackageName);
            testContext.SolutionService.SaveAll();
            testContext.SolutionService.Build();

            // Assert
            VisualStudio.AssertNoErrors();

            var errors = VisualStudio.ObjectModel.Shell.ToolWindows.ErrorList.AllItems.Select(i => i.Description).ToList();
            errors.Where(msg => msg.Contains(TestPackageName)).Should().ContainSingle();
            errors.Single(msg => msg.Contains(TestPackageName)).Should().Contain("https://cve.test/2");
        }
    }
}
