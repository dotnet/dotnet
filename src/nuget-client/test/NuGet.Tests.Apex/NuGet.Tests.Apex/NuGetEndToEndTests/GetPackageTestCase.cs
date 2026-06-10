// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Test.Apex.VisualStudio.Solution;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NuGet.Tests.Apex
{
    [TestClass]
    public class GetPackageTestCase : SharedVisualStudioHostTestClass
    {
        /// <summary>
        /// Single integration test that exercises the full Get-Package lifecycle in one VS session:
        /// Install → Get-Package (installed) → Get-Package -ListAvailable → Get-Package -Updates -Prerelease.
        /// Detailed filtering/prerelease logic is covered by cmdlet-level unit tests in
        /// NuGetConsole.Host.PowerShell.Test/Cmdlets/GetPackageCommandTests.cs.
        /// </summary>
        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task GetPackage_InstallListAndUpdateLifecycleAsync()
        {
            using var testContext = new ApexTestContext(VisualStudio, ProjectTemplate.ConsoleApplication, Logger);

            var packageName = "LifecycleTestPackage";

            // Create a stable version and a prerelease update
            await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, "1.0.0");
            await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, packageName, "2.0.0-beta");

            var nugetConsole = GetConsole(testContext.Project);
            string escapedSource = testContext.PackageSource.Replace("'", "''");

            // 1. Verify no packages installed initially
            nugetConsole.Clear();
            nugetConsole.Execute("Get-Package");
            nugetConsole.GetText().Should().NotContain(packageName);

            // 2. Install stable version
            nugetConsole.InstallPackageFromPMC(packageName, "1.0.0");

            // 3. Get-Package (installed) — should list the installed package
            nugetConsole.Clear();
            nugetConsole.Execute("Get-Package");
            string installedText = nugetConsole.GetText();
            installedText.Should().Contain(packageName, because: "package was just installed");
            installedText.Should().Contain("1.0.0");

            // 4. Get-Package -ListAvailable — should show the package from source
            nugetConsole.Clear();
            nugetConsole.Execute($"Get-Package -ListAvailable -Source '{escapedSource}' -Filter '{packageName}'");
            string listAvailableText = nugetConsole.GetText();
            listAvailableText.Should().Contain(packageName, because: "package exists in source");

            // 5. Get-Package -Updates — without prerelease, no update (2.0.0-beta is prerelease)
            nugetConsole.Clear();
            nugetConsole.Execute($"Get-Package -Updates -Source '{escapedSource}'");
            string updatesText = nugetConsole.GetText();
            updatesText.Should().NotContain("2.0.0-beta", because: "prerelease updates hidden without -Prerelease switch");

            // 6. Get-Package -Updates -Prerelease — should find prerelease update
            nugetConsole.Clear();
            nugetConsole.Execute($"Get-Package -Updates -Prerelease -Source '{escapedSource}'");
            string prereleaseUpdatesText = nugetConsole.GetText();
            prereleaseUpdatesText.Should().Contain(packageName, because: "prerelease update exists");
            prereleaseUpdatesText.Should().Contain("2.0.0-beta", because: "-Prerelease switch enables prerelease updates");
        }
    }
}
