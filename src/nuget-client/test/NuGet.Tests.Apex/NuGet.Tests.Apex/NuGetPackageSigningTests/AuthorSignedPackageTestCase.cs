// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Test.Apex.VisualStudio.Solution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet.Test.Utility;
using Test.Utility.Signing;

namespace NuGet.Tests.Apex
{
    [TestClass]
    public class AuthorSignedPackageTestCase : SharedVisualStudioHostTestClass
    {
        private static SignedPackagesTestsApexFixture Fixture;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            Fixture = new SignedPackagesTestsApexFixture();
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackagesConfigTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task InstallFromPMCForPC_SucceedAsync(ProjectTemplate projectTemplate)
        {
            var signedPackage = Fixture.AuthorSignedTestPackage;

            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger))
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(testContext.PackageSource, signedPackage);

                var nugetConsole = GetConsole(testContext.Project);

                nugetConsole.InstallPackageFromPMC(signedPackage.Id, signedPackage.Version);

                CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, signedPackage.Id, signedPackage.Version, Logger);
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackagesConfigTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task UninstallFromPMCForPC_SucceedAsync(ProjectTemplate projectTemplate)
        {
            var signedPackage = Fixture.AuthorSignedTestPackage;

            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger))
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(testContext.PackageSource, signedPackage);

                var nugetConsole = GetConsole(testContext.Project);

                nugetConsole.InstallPackageFromPMC(signedPackage.Id, signedPackage.Version);
                nugetConsole.UninstallPackageFromPMC(signedPackage.Id);

                CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, testContext.Project, signedPackage.Id, signedPackage.Version, Logger);
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackagesConfigTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task UpdateUnsignedToSignedVersionFromPMCForPC_SucceedAsync(ProjectTemplate projectTemplate)
        {
            var packageVersion09 = "0.9.0";
            var signedPackage = Fixture.AuthorSignedTestPackage;

            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger))
            {
                await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, signedPackage.Id, packageVersion09);
                await SimpleTestPackageUtility.CreatePackagesAsync(testContext.PackageSource, signedPackage);

                var nugetConsole = GetConsole(testContext.Project);

                nugetConsole.InstallPackageFromPMC(signedPackage.Id, packageVersion09);
                nugetConsole.UpdatePackageFromPMC(signedPackage.Id, signedPackage.Version);

                CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, signedPackage.Id, signedPackage.Version, Logger);
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackagesConfigTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task DowngradeSignedToUnsignedVersionFromPMCForPC_SucceedAsync(ProjectTemplate projectTemplate)
        {
            // This test is not considered an ideal behavior of the product but states the current behavior.
            // A package that is already installed as signed should be specailly treated and a user should not be
            // able to downgrade to an unsigned version. This test needs to be updated once this behavior gets
            // corrected in the product.
            var packageVersion09 = "0.9.0";
            var signedPackage = Fixture.AuthorSignedTestPackage;

            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger))
            {
                await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, signedPackage.Id, packageVersion09);
                await SimpleTestPackageUtility.CreatePackagesAsync(testContext.PackageSource, signedPackage);

                var nugetConsole = GetConsole(testContext.Project);

                nugetConsole.InstallPackageFromPMC(signedPackage.Id, signedPackage.Version);
                nugetConsole.UpdatePackageFromPMC(signedPackage.Id, packageVersion09);

                CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, signedPackage.Id, packageVersion09, Logger);
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackagesConfigTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task WithExpiredCertificate_InstallFromPMCForPC_WarnAsync(ProjectTemplate projectTemplate)
        {
            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger))
            using (var trustedExpiringTestCert = SigningUtility.GenerateTrustedTestCertificateThatWillExpireSoon())
            {
                Logger.WriteMessage("Creating package");

                var package = CommonUtility.CreatePackage("ExpiredTestPackage", "1.0.0");

                Logger.WriteMessage("Signing package");
                var expiredTestPackage = CommonUtility.AuthorSignPackage(package, trustedExpiringTestCert.Source.Cert);
                await SimpleTestPackageUtility.CreatePackagesAsync(testContext.PackageSource, expiredTestPackage);

                Logger.WriteMessage("Waiting for package to expire");
                SigningUtility.WaitForCertificateToExpire(trustedExpiringTestCert.Source.Cert);

                var nugetConsole = GetConsole(testContext.Project);

                nugetConsole.InstallPackageFromPMC(expiredTestPackage.Id, expiredTestPackage.Version);

                // TODO: Fix bug where no warnings are shown when package is untrusted but still installed
                //nugetConsole.IsMessageFoundInPMC("expired certificate").Should().BeTrue("expired certificate warning");
                CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, expiredTestPackage.Id, expiredTestPackage.Version, Logger);
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPackagesConfigTemplates), DynamicDataSourceType.Method)]
        [Timeout(DefaultTimeout)]
        public async Task Tampered_InstallFromPMCForPC_FailAsync(ProjectTemplate projectTemplate)
        {
            var signedPackage = Fixture.AuthorSignedTestPackage;

            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, Logger))
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(testContext.PackageSource, signedPackage);
                SignedArchiveTestUtility.TamperWithPackage(Path.Combine(testContext.PackageSource, signedPackage.PackageName));

                var nugetConsole = GetConsole(testContext.Project);

                nugetConsole.InstallPackageFromPMC(signedPackage.Id, signedPackage.Version);
                nugetConsole.IsMessageFoundInPMC("package integrity check failed").Should().BeTrue("Integrity failed message shown.");

                CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, testContext.Project, signedPackage.Id, signedPackage.Version, Logger);
            }
        }

        public static IEnumerable<object[]> GetPackagesConfigTemplates()
        {
            yield return new object[] { ProjectTemplate.ClassLibrary };
        }

        public static IEnumerable<object[]> GetPackageReferenceTemplates()
        {
            yield return new object[] { ProjectTemplate.NetStandardClassLib };
        }
    }
}
