// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Test.Apex.VisualStudio.Solution;
using NuGet.StaFact;
using NuGet.Test.Utility;
using Test.Utility.Signing;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.Tests.Apex
{
    public class RepositoryCountersignedPackageTestCase : SharedVisualStudioHostTestClass, IClassFixture<SignedPackagesTestsApexFixture>
    {
        private SignedPackagesTestsApexFixture _fixture;

        public RepositoryCountersignedPackageTestCase(SignedPackagesTestsApexFixture apexSigningFixture, ITestOutputHelper output)
            : base(apexSigningFixture, output)
        {
            _fixture = apexSigningFixture;
        }

        [CIOnlyNuGetWpfTheory]
        [MemberData(nameof(GetPackagesConfigTemplates))]
        public async Task InstallFromPMCForPC_SucceedAsync(ProjectTemplate projectTemplate)
        {
            // Arrange
            EnsureVisualStudioHost();

            var signedPackage = _fixture.RepositoryCountersignedTestPackage;

            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, XunitLogger))
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(testContext.PackageSource, signedPackage);

                var nugetConsole = GetConsole(testContext.Project);

                nugetConsole.InstallPackageFromPMC(signedPackage.Id, signedPackage.Version);

                CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, signedPackage.Id, signedPackage.Version, XunitLogger);
            }
        }

        [CIOnlyNuGetWpfTheory]
        [MemberData(nameof(GetPackagesConfigTemplates))]
        public async Task UninstallFromPMCForPC_SucceedAsync(ProjectTemplate projectTemplate)
        {
            // Arrange
            EnsureVisualStudioHost();

            var signedPackage = _fixture.RepositoryCountersignedTestPackage;

            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, XunitLogger))
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(testContext.PackageSource, signedPackage);

                var nugetConsole = GetConsole(testContext.Project);

                nugetConsole.InstallPackageFromPMC(signedPackage.Id, signedPackage.Version);
                nugetConsole.UninstallPackageFromPMC(signedPackage.Id);

                CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, testContext.Project, signedPackage.Id, signedPackage.Version, XunitLogger);
            }
        }

        [CIOnlyNuGetWpfTheory]
        [MemberData(nameof(GetPackagesConfigTemplates))]
        public async Task UpdateUnsignedToSignedVersionFromPMCForPC_SucceedAsync(ProjectTemplate projectTemplate)
        {
            // Arrange
            EnsureVisualStudioHost();

            var packageVersion09 = "0.9.0";
            var signedPackage = _fixture.RepositoryCountersignedTestPackage;

            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, XunitLogger))
            {
                await CommonUtility.CreatePackageInSourceAsync(testContext.PackageSource, signedPackage.Id, packageVersion09);
                await SimpleTestPackageUtility.CreatePackagesAsync(testContext.PackageSource, signedPackage);

                var nugetConsole = GetConsole(testContext.Project);

                nugetConsole.InstallPackageFromPMC(signedPackage.Id, packageVersion09);
                nugetConsole.UpdatePackageFromPMC(signedPackage.Id, signedPackage.Version);

                CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, signedPackage.Id, signedPackage.Version, XunitLogger);
            }
        }

        [CIOnlyNuGetWpfTheory]
        [MemberData(nameof(GetPackagesConfigTemplates))]
        public async Task WithExpiredAuthorCertificateAtCountersigning_InstallFromPMCForPC_WarnAsync(ProjectTemplate projectTemplate)
        {
            // Arrange
            EnsureVisualStudioHost();

            var timestampService = await _fixture.GetDefaultTrustedTimestampServiceAsync();

            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, XunitLogger))
            using (var trustedCert = _fixture.TrustedRepositoryTestCertificate)
            using (var trustedExpiringTestCert = SigningUtility.GenerateTrustedTestCertificateThatWillExpireSoon())
            {
                XunitLogger.LogInformation("Creating package");
                var package = CommonUtility.CreatePackage("ExpiredTestPackage", "1.0.0");

                XunitLogger.LogInformation("Signing package");
                var expiredTestPackage = CommonUtility.AuthorSignPackage(package, trustedExpiringTestCert.Source.Cert);
                await SimpleTestPackageUtility.CreatePackagesAsync(testContext.PackageSource, expiredTestPackage);
                var packageFullName = Path.Combine(testContext.PackageSource, expiredTestPackage.PackageName);

                XunitLogger.LogInformation("Waiting for package to expire");
                SigningUtility.WaitForCertificateToExpire(trustedExpiringTestCert.Source.Cert);

                XunitLogger.LogInformation("Countersigning package");
                var countersignedPackage = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                    new X509Certificate2(trustedCert.Source.Cert),
                    packageFullName,
                    testContext.PackageSource,
                    new Uri("https://v3serviceIndexUrl.test/api/index.json"),
                    timestampService.Url);
                File.Copy(countersignedPackage, packageFullName, overwrite: true);
                File.Delete(countersignedPackage);

                var nugetConsole = GetConsole(testContext.Project);

                nugetConsole.InstallPackageFromPMC(expiredTestPackage.Id, expiredTestPackage.Version);

                // TODO: Fix bug where no warnings are shown when package is untrusted but still installed
                //nugetConsole.IsMessageFoundInPMC("expired certificate").Should().BeTrue("expired certificate warning");
                CommonUtility.AssertPackageInPackagesConfig(VisualStudio, testContext.Project, expiredTestPackage.Id, expiredTestPackage.Version, XunitLogger);
            }
        }

        [CIOnlyNuGetWpfTheory]
        [MemberData(nameof(GetPackagesConfigTemplates))]
        public async Task Tampered_InstallFromPMCForPC_FailAsync(ProjectTemplate projectTemplate)
        {
            // Arrange
            EnsureVisualStudioHost();

            var signedPackage = _fixture.RepositoryCountersignedTestPackage;

            using (var testContext = new ApexTestContext(VisualStudio, projectTemplate, XunitLogger))
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(testContext.PackageSource, signedPackage);
                SignedArchiveTestUtility.TamperWithPackage(Path.Combine(testContext.PackageSource, signedPackage.PackageName));

                var nugetConsole = GetConsole(testContext.Project);

                nugetConsole.InstallPackageFromPMC(signedPackage.Id, signedPackage.Version);
                nugetConsole.IsMessageFoundInPMC("package integrity check failed").Should().BeTrue("Integrity failed message shown.");

                CommonUtility.AssertPackageNotInPackagesConfig(VisualStudio, testContext.Project, signedPackage.Id, signedPackage.Version, XunitLogger);
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
