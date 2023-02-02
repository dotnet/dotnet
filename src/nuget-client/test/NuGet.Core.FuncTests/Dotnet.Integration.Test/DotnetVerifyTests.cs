// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Common;
using NuGet.Test.Utility;
using Test.Utility.Signing;
using Xunit;

namespace Dotnet.Integration.Test
{
    [Collection(DotnetIntegrationCollection.Name)]
    public class DotnetVerifyTests
    {
        private readonly string _noTimestamperWarningCode = NuGetLogCode.NU3027.ToString();
        private readonly string _primarySignatureInvalidErrorCode = NuGetLogCode.NU3018.ToString();
        private readonly string _noMatchingCertErrorCode = NuGetLogCode.NU3034.ToString();
        private readonly string _notSignedErrorCode = NuGetLogCode.NU3004.ToString();

        private MsbuildIntegrationTestFixture _msbuildFixture;
        private readonly SignCommandTestFixture _signFixture;

        public DotnetVerifyTests(MsbuildIntegrationTestFixture msbuildFixture, SignCommandTestFixture signFixture)
        {
            _msbuildFixture = msbuildFixture;
            _signFixture = signFixture;
        }

        [CIOnlyFact]
        public async Task Verify_UnSignedPackage_Fails()
        {
            using (var packageDir = TestDirectory.Create())
            {
                var packageId = "Unsigned.PackageX";
                var packageVersion = "1.0.0";
                var packageFile = await TestPackagesCore.GetRuntimePackageAsync(packageDir, packageId, packageVersion);

                //Act
                var result = _msbuildFixture.RunDotnet(
                    packageDir,
                    $"nuget verify {packageFile.FullName}",
                    ignoreExitCode: true);

                result.Success.Should().BeFalse(because: result.AllOutput);
                result.Output.Should().Contain(_notSignedErrorCode);
            }
        }

        // https://github.com/NuGet/Home/issues/11178
        [PlatformFact(Platform.Windows, Platform.Linux)]
        public void Verify_AuthorSignedAndTimestampedPackageWithOptionAll_Succeeds()
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var packageFile = new FileInfo(Path.Combine(testDirectory, "TestPackage.AuthorSigned.1.0.0.nupkg"));
                var package = SigningTestUtility.GetResourceBytes(packageFile.Name);
                File.WriteAllBytes(packageFile.FullName, package);

                //Act
                var result = _msbuildFixture.RunDotnet(
                    testDirectory,
                    $"nuget verify {packageFile.FullName} --all",
                    ignoreExitCode: true);

                result.Success.Should().BeTrue(because: result.AllOutput);
                result.Output.Should().NotContain(_noTimestamperWarningCode);
                result.Output.Should().NotContain(_primarySignatureInvalidErrorCode);
            }
        }

        [CIOnlyFact]
        public void Verify_SignedPackageWithoutAllowedCertificate_Fails()
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var packageFile = new FileInfo(Path.Combine(testDirectory, "TestPackage.AuthorSigned.1.0.0.nupkg"));
                var package = SigningTestUtility.GetResourceBytes(packageFile.Name);
                File.WriteAllBytes(packageFile.FullName, package);

                //Act
                var result = _msbuildFixture.RunDotnet(
                    testDirectory,
                    $"nuget verify {packageFile.FullName} " +
                    $"--certificate-fingerprint 775AAB607AA76028A7CC7A873A9513FF0C3B40DF09B7B83D21689A3675B34D9A --certificate-fingerprint DEF",
                    ignoreExitCode: true);

                result.Success.Should().BeFalse(because: result.AllOutput);
                result.AllOutput.Should().Contain(_noMatchingCertErrorCode);
            }
        }

        // https://github.com/NuGet/Home/issues/11178
        [PlatformFact(Platform.Windows, Platform.Linux)]
        public void Verify_SignedPackageWithAllowedCertificate_Succeeds()
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var packageFile = new FileInfo(Path.Combine(testDirectory, "TestPackage.AuthorSigned.1.0.0.nupkg"));
                var package = SigningTestUtility.GetResourceBytes(packageFile.Name);
                File.WriteAllBytes(packageFile.FullName, package);

                //Act
                var result = _msbuildFixture.RunDotnet(
                    testDirectory,
                    $"nuget verify {packageFile.FullName} " +
                    $"--certificate-fingerprint 3F9001EA83C560D712C24CF213C3D312CB3BFF51EE89435D3430BD06B5D0EECE --certificate-fingerprint def",
                    ignoreExitCode: true);

                result.Success.Should().BeTrue(because: result.AllOutput);
            }
        }

        // https://github.com/NuGet/Home/issues/11178
        [PlatformFact(Platform.Windows, Platform.Linux)]
        public void Verify_MultipleSignedPackagesWithWildCardAndDetailedVerbosity_MixedResults()
        {
            // Arrange
            using (var testDirectory1 = TestDirectory.Create())
            {
                using (var testDirectory2 = TestDirectory.Create())
                {
                    var packagX = new FileInfo(Path.Combine(testDirectory1, "TestPackage.AuthorSigned.1.0.0.nupkg"));
                    var bPackageX = SigningTestUtility.GetResourceBytes(packagX.Name);
                    File.WriteAllBytes(packagX.FullName, bPackageX);

                    var packageY = new FileInfo(Path.Combine(testDirectory2, "Test.Reposigned.1.0.0.nupkg"));
                    var bpackageY = SigningTestUtility.GetResourceBytes(packageY.Name);
                    File.WriteAllBytes(packageY.FullName, bpackageY);

                    //Act
                    var result = _msbuildFixture.RunDotnet(
                        testDirectory1,
                        $"nuget verify {packagX.FullName} {Path.Combine(testDirectory2, "*.nupkg")} -v d",
                        ignoreExitCode: true);

                    result.Success.Should().BeFalse(because: result.AllOutput);
                    result.AllOutput.Should().Contain("Successfully verified package 'TestPackage.AuthorSigned.1.0.0'.");
                    result.AllOutput.Should().Contain($"Verifying Test.Reposigned.1.0.0");
                    result.AllOutput.Should().Contain(_primarySignatureInvalidErrorCode);
                    result.AllOutput.Should().Contain(_noTimestamperWarningCode);
                }
            }
        }

        [CIOnlyTheory]
        [InlineData("true", true)]
        [InlineData("true", false)]
        [InlineData("false", true)]
        [InlineData("false", false)]
        public async Task Verify_AuthorSignedPackage_WithAuthorItemTrustedCertificate_Succeeds(string allowUntrustedRoot, bool verifyCertificateFingerprint)
        {
            // Arrange
            IX509StoreCertificate storeCertificate = _signFixture.DefaultCertificate;

            using (var pathContext = new SimpleTestPathContext())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");
                string testDirectory = pathContext.WorkingDirectory;
                await SimpleTestPackageUtility.CreatePackagesAsync(testDirectory, nupkg);

                // Act
                string certificateFingerprintString = SignatureTestUtility.GetFingerprint(storeCertificate.Certificate, HashAlgorithmName.SHA256);
                string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(storeCertificate.Certificate, nupkg, testDirectory);

                // Arrange
                string trustedSignersSectionContent = $@"
    <trustedSigners>
        <author name=""signed"">
            <certificate fingerprint=""{certificateFingerprintString}"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""{allowUntrustedRoot}"" />
        </author>
    </trustedSigners>
";
                SimpleTestSettingsContext.AddSectionIntoNuGetConfig(testDirectory, trustedSignersSectionContent, "configuration");
                string fingerprint = verifyCertificateFingerprint ? $"--certificate-fingerprint {certificateFingerprintString} --certificate-fingerprint def" : string.Empty;

                // Act
                CommandRunnerResult verifyResult = _msbuildFixture.RunDotnet(
                    pathContext.WorkingDirectory,
                    $"nuget verify {signedPackagePath} {fingerprint}",
                    ignoreExitCode: true);

                // Assert
                // For certificate with trusted root setting allowUntrustedRoot to true/false doesn't matter
                verifyResult.Success.Should().BeTrue(because: verifyResult.AllOutput);
                verifyResult.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [CIOnlyTheory]
        [InlineData("true", true)]
        [InlineData("true", false)]
        [InlineData("false", true)]
        [InlineData("false", false)]
        public async Task Verify_AuthorSignedPackage_WithRepositoryItemTrustedCertificate_Fails(string allowUntrustedRoot, bool verifyCertificateFingerprint)
        {
            // Arrange
            IX509StoreCertificate storeCertificate = _signFixture.DefaultCertificate;

            using (var pathContext = new SimpleTestPathContext())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");
                string testDirectory = pathContext.WorkingDirectory;
                await SimpleTestPackageUtility.CreatePackagesAsync(testDirectory, nupkg);

                // Act
                string certificateFingerprintString = SignatureTestUtility.GetFingerprint(storeCertificate.Certificate, HashAlgorithmName.SHA256);
                string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(storeCertificate.Certificate, nupkg, testDirectory);

                // Arrange
                string trustedSignersSectionContent = $@"
    <trustedSigners>
        <repository name=""MyCert"" serviceIndex=""{pathContext.PackageSource}"">
            <certificate fingerprint=""{certificateFingerprintString}"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""{allowUntrustedRoot}"" />
        </repository>
    </trustedSigners>
";
                SimpleTestSettingsContext.AddSectionIntoNuGetConfig(pathContext.WorkingDirectory, trustedSignersSectionContent, "configuration");
                string fingerprint = verifyCertificateFingerprint ? $"--certificate-fingerprint {certificateFingerprintString} --certificate-fingerprint def" : string.Empty;

                // Act
                CommandRunnerResult verifyResult = _msbuildFixture.RunDotnet(
                    testDirectory,
                    $"nuget verify {signedPackagePath} {fingerprint}",
                    ignoreExitCode: true);

                // Assert
                verifyResult.Success.Should().BeFalse(because: verifyResult.AllOutput);
                verifyResult.AllOutput.Should().Contain(_noMatchingCertErrorCode);
                verifyResult.AllOutput.Should().Contain(_noTimestamperWarningCode);
                verifyResult.AllOutput.Should().Contain("This package is signed but not by a trusted signer.");
            }
        }

        [CIOnlyTheory]
        [InlineData("true", true)]
        [InlineData("true", false)]
        [InlineData("false", true)]
        [InlineData("false", false)]
        public async Task Verify_RepositorySignedPackage_WithAuthorItemUntrustedCertificate_Fails(string allowUntrustedRoot, bool verifyCertificateFingerprint)
        {
            IX509StoreCertificate storeCertificate = _signFixture.UntrustedSelfIssuedCertificateInCertificateStore;

            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");
                string testDirectory = pathContext.WorkingDirectory;
                await SimpleTestPackageUtility.CreatePackagesAsync(testDirectory, nupkg);
                string packagePath = Path.Combine(testDirectory, nupkg.PackageName);

                //Act
                string certificateFingerprintString = SignatureTestUtility.GetFingerprint(storeCertificate.Certificate, HashAlgorithmName.SHA256);
                string repoServiceIndex = "https://serviceindex.test/v3/index.json";
                string signedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(storeCertificate.Certificate, packagePath, pathContext.PackageSource, new Uri(repoServiceIndex));

                // Arrange
                string trustedSignersSectionContent = $@"
    <trustedSigners>
        <author name=""MyCert"">
            <certificate fingerprint=""{certificateFingerprintString}"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""{allowUntrustedRoot}"" />
        </author>
    </trustedSigners>
";
                SimpleTestSettingsContext.AddSectionIntoNuGetConfig(pathContext.WorkingDirectory, trustedSignersSectionContent, "configuration");
                string fingerprint = verifyCertificateFingerprint ? $"--certificate-fingerprint {certificateFingerprintString} --certificate-fingerprint def" : string.Empty;

                //Act
                CommandRunnerResult verifyResult = _msbuildFixture.RunDotnet(
                    testDirectory,
                    $"nuget verify {signedPackagePath} {fingerprint}",
                    ignoreExitCode: true);

                // Assert
                verifyResult.Success.Should().BeFalse(because: verifyResult.AllOutput);
                verifyResult.AllOutput.Should().Contain(_noTimestamperWarningCode);
                verifyResult.AllOutput.Should().Contain(_noMatchingCertErrorCode);
                verifyResult.AllOutput.Should().Contain("This package is signed but not by a trusted signer.");

                if (bool.TryParse(allowUntrustedRoot, out bool parsed) && !parsed)
                {
                    verifyResult.AllOutput.Should().Contain(_primarySignatureInvalidErrorCode);
                }
                else
                {
                    verifyResult.AllOutput.Should().NotContain(_primarySignatureInvalidErrorCode);
                }
            }
        }

        [CIOnlyTheory]
        [InlineData("false", true)]
        [InlineData("false", false)]
        public async Task Verify_RepositorySignedPackage_WithRepositoryItemUntrustedCertificate_AllowUntrustedRootSetFalse_Fails(string allowUntrustedRoot, bool verifyCertificateFingerprint)
        {
            IX509StoreCertificate storeCertificate = _signFixture.UntrustedSelfIssuedCertificateInCertificateStore;

            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");
                string testDirectory = pathContext.WorkingDirectory;
                await SimpleTestPackageUtility.CreatePackagesAsync(testDirectory, nupkg);
                string packagePath = Path.Combine(testDirectory, nupkg.PackageName);

                //Act
                string certificateFingerprintString = SignatureTestUtility.GetFingerprint(storeCertificate.Certificate, HashAlgorithmName.SHA256);
                string repoServiceIndex = "https://serviceindex.test/v3/index.json";
                string signedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(storeCertificate.Certificate, packagePath, pathContext.PackageSource, new Uri(repoServiceIndex));

                // Arrange
                string trustedSignersSectionContent = $@"
    <trustedSigners>
        <repository name=""MyCert"" serviceIndex = ""{repoServiceIndex}"">
            <certificate fingerprint=""{certificateFingerprintString}"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""{allowUntrustedRoot}"" />
        </repository>
    </trustedSigners>
";
                SimpleTestSettingsContext.AddSectionIntoNuGetConfig(pathContext.WorkingDirectory, trustedSignersSectionContent, "configuration");
                string fingerprint = verifyCertificateFingerprint ? $"--certificate-fingerprint {certificateFingerprintString} --certificate-fingerprint def" : string.Empty;

                //Act
                CommandRunnerResult verifyResult = _msbuildFixture.RunDotnet(
                    testDirectory,
                    $"nuget verify {signedPackagePath} {fingerprint}",
                    ignoreExitCode: true);

                // Assert
                // Unless allowUntrustedRoot is set true in nuget.config verify always fails for cert without trusted root.
                verifyResult.Success.Should().BeFalse(because: verifyResult.AllOutput);
                verifyResult.AllOutput.Should().Contain(_noTimestamperWarningCode);
                verifyResult.AllOutput.Should().Contain(_primarySignatureInvalidErrorCode);
                verifyResult.AllOutput.Should().Contain("The repository primary signature's signing certificate is not trusted by the trust provider.");
            }
        }

        [CIOnlyTheory]
        [InlineData("true", true)]
        [InlineData("true", false)]
        public async Task Verify_RepositorySignedPackage_WithRepositoryItemUntrustedCertificate_AllowUntrustedRootSetTrue_Succeeds(string allowUntrustedRoot, bool verifyCertificateFingerprint)
        {
            IX509StoreCertificate storeCertificate = _signFixture.UntrustedSelfIssuedCertificateInCertificateStore;

            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");
                string testDirectory = pathContext.WorkingDirectory;
                await SimpleTestPackageUtility.CreatePackagesAsync(testDirectory, nupkg);
                string packagePath = Path.Combine(testDirectory, nupkg.PackageName);

                //Act
                string certificateFingerprintString = SignatureTestUtility.GetFingerprint(storeCertificate.Certificate, HashAlgorithmName.SHA256);
                string repoServiceIndex = "https://serviceindex.test/v3/index.json";
                string signedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(storeCertificate.Certificate, packagePath, pathContext.PackageSource, new Uri(repoServiceIndex));

                // Arrange
                string trustedSignersSectionContent = $@"
    <trustedSigners>
        <repository name=""MyCert"" serviceIndex = ""{repoServiceIndex}"">
            <certificate fingerprint=""{certificateFingerprintString}"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""{allowUntrustedRoot}"" />
        </repository>
    </trustedSigners>
";
                SimpleTestSettingsContext.AddSectionIntoNuGetConfig(pathContext.WorkingDirectory, trustedSignersSectionContent, "configuration");
                string fingerprint = verifyCertificateFingerprint ? $"--certificate-fingerprint {certificateFingerprintString} --certificate-fingerprint def" : string.Empty;

                //Act
                CommandRunnerResult verifyResult = _msbuildFixture.RunDotnet(
                    testDirectory,
                    $"nuget verify {signedPackagePath} {fingerprint}",
                    ignoreExitCode: true);

                // Assert
                // If allowUntrustedRoot is set true in nuget.config then verify succeeds for cert with untrusted root.
                verifyResult.Success.Should().BeTrue();
                verifyResult.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [CIOnlyTheory]
        [InlineData("true", true)]
        [InlineData("false", true)]
        [InlineData("true", false)]
        [InlineData("false", false)]
        public async Task Verify_RepositorySignedPackage_WithRepositoryItemUntrustedCertificate_AllowUntrustedRootSetTrue_WrongOwners_Fails(string allowUntrustedRoot, bool verifyCertificateFingerprint)
        {
            IX509StoreCertificate storeCertificate = _signFixture.UntrustedSelfIssuedCertificateInCertificateStore;

            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");
                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.WorkingDirectory, nupkg);
                string packagePath = Path.Combine(pathContext.WorkingDirectory, nupkg.PackageName);

                //Act
                var certificateFingerprintString = SignatureTestUtility.GetFingerprint(storeCertificate.Certificate, HashAlgorithmName.SHA256);
                var packageOwners = new List<string>()
                {
                    "nuget",
                    "contoso"
                };
                string repoServiceIndex = "https://serviceindex.test/v3/index.json";
                string signedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(storeCertificate.Certificate, packagePath, pathContext.PackageSource, new Uri(repoServiceIndex), null, packageOwners);

                // Arrange
                string trustedSignersSectionContent = $@"
    <trustedSigners>
        <repository name=""MyCert"" serviceIndex = ""{repoServiceIndex}"">
            <certificate fingerprint=""{certificateFingerprintString}"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""{allowUntrustedRoot}"" />
            <owners>Nuget;Contoso</owners>
        </repository>
    </trustedSigners>
";
                SimpleTestSettingsContext.AddSectionIntoNuGetConfig(pathContext.WorkingDirectory, trustedSignersSectionContent, "configuration");
                string fingerprint = verifyCertificateFingerprint ? $"--certificate-fingerprint {certificateFingerprintString} --certificate-fingerprint DEF" : string.Empty;

                //Act
                CommandRunnerResult verifyResult = _msbuildFixture.RunDotnet(
                    pathContext.WorkingDirectory,
                    $"nuget verify {signedPackagePath} {fingerprint}",
                    ignoreExitCode: true);

                // Assert
                // Owners is casesensitive, owner info should be "nuget;contoso" not "Nuget;Contoso"
                verifyResult.Success.Should().BeFalse(because: verifyResult.AllOutput);
                verifyResult.AllOutput.Should().Contain(_noMatchingCertErrorCode);
                verifyResult.AllOutput.Should().Contain("This package is signed but not by a trusted signer.");
            }
        }

        [CIOnlyTheory]
        [InlineData("true", true)]
        [InlineData("true", false)]
        public async Task Verify_RepositorySignedPackage_WithRepositoryItemUntrustedCertificate_AllowUntrustedRootSetTrue_CorrectOwners_Succeeds(string allowUntrustedRoot, bool verifyCertificateFingerprint)
        {
            IX509StoreCertificate storeCertificate = _signFixture.UntrustedSelfIssuedCertificateInCertificateStore;

            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");
                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.WorkingDirectory, nupkg);
                string packagePath = Path.Combine(pathContext.WorkingDirectory, nupkg.PackageName);

                //Act
                string certificateFingerprintString = SignatureTestUtility.GetFingerprint(storeCertificate.Certificate, HashAlgorithmName.SHA256);
                var packageOwners = new List<string>()
                {
                    "nuget",
                    "contoso"
                };
                string repoServiceIndex = "https://serviceindex.test/v3/index.json";
                string signedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(storeCertificate.Certificate, packagePath, pathContext.PackageSource, new Uri(repoServiceIndex), null, packageOwners);

                // Arrange
                string trustedSignersSectionContent = $@"
    <trustedSigners>
        <repository name=""MyCert"" serviceIndex = ""{repoServiceIndex}"">
            <certificate fingerprint=""{certificateFingerprintString}"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""{allowUntrustedRoot}"" />
            <owners>nuget;Contoso</owners>
        </repository>
    </trustedSigners>
";
                SimpleTestSettingsContext.AddSectionIntoNuGetConfig(pathContext.WorkingDirectory, trustedSignersSectionContent, "configuration");
                string fingerprint = verifyCertificateFingerprint ? $"--certificate-fingerprint {certificateFingerprintString} --certificate-fingerprint DEF" : string.Empty;

                //Act
                CommandRunnerResult verifyResult = _msbuildFixture.RunDotnet(
                    pathContext.WorkingDirectory,
                    $"nuget verify {signedPackagePath} {fingerprint}",
                    ignoreExitCode: true);

                // Assert
                // Owners is casesensitive, here owner "nuget" matches
                verifyResult.Success.Should().BeTrue();
                verifyResult.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [CIOnlyTheory]
        [InlineData("true", true)]
        [InlineData("false", true)]
        [InlineData("false", false)]
        [InlineData("true", false)]
        public async Task Verify_RepositorySignedPackage_WithRepositoryItemTrustedCertificate_AllowUntrustedRootSet_WrongOwners_Fails(string allowUntrustedRoot, bool verifyCertificateFingerprint)
        {
            // Arrange
            IX509StoreCertificate storeCertificate = _signFixture.DefaultCertificate;

            using (var pathContext = new SimpleTestPathContext())
            {
                var package = new SimpleTestPackageContext();
                string certFingerprint = SignatureTestUtility.GetFingerprint(storeCertificate.Certificate, HashAlgorithmName.SHA256);
                var packageOwners = new List<string>()
                {
                    "nuget",
                    "contoso"
                };
                string repoServiceIndex = "https://serviceindex.test/v3/index.json";
                string signedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                    storeCertificate.Certificate,
                    package,
                    pathContext.PackageSource,
                    new Uri(repoServiceIndex),
                    timestampService: null,
                    packageOwners);

                string testDirectory = pathContext.WorkingDirectory;

                // Arrange
                string trustedSignersSectionContent = $@"
    <trustedSigners>
    <repository name=""NuGetTrust"" serviceIndex=""{repoServiceIndex}"">
      <certificate fingerprint=""{certFingerprint}"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""{allowUntrustedRoot}"" />
      <owners>Nuget;Contoso</owners>
    </repository>
    </trustedSigners>
";
                SimpleTestSettingsContext.AddSectionIntoNuGetConfig(testDirectory, trustedSignersSectionContent, "configuration");
                string fingerprint = verifyCertificateFingerprint ? $"--certificate-fingerprint {certFingerprint} --certificate-fingerprint DEF" : string.Empty;

                //Act
                CommandRunnerResult verifyResult = _msbuildFixture.RunDotnet(
                    testDirectory,
                    $"nuget verify {signedPackagePath} {fingerprint}",
                    ignoreExitCode: true);

                // Assert
                // Owners is casesensitive, owner info should be "nuget;contoso" not "Nuget;Contoso"
                verifyResult.Success.Should().BeFalse();
                verifyResult.AllOutput.Should().Contain(_noMatchingCertErrorCode);
            }
        }

        [CIOnlyTheory]
        [InlineData("true", true)]
        [InlineData("false", true)]
        [InlineData("false", false)]
        [InlineData("true", false)]
        public async Task Verify_RepositorySignedPackage_WithRepositoryItemTrustedCertificate_AllowUntrustedRootSet_CorrectOwners_Succeeds(string allowUntrustedRoot, bool verifyCertificateFingerprint)
        {
            // Arrange
            IX509StoreCertificate storeCertificate = _signFixture.DefaultCertificate;

            using (var pathContext = new SimpleTestPathContext())
            {
                var package = new SimpleTestPackageContext();
                string certFingerprint = SignatureTestUtility.GetFingerprint(storeCertificate.Certificate, HashAlgorithmName.SHA256);
                var packageOwners = new List<string>()
                {
                    "nuget",
                    "contoso"
                };
                string repoServiceIndex = "https://serviceindex.test/v3/index.json";
                string signedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                    storeCertificate.Certificate,
                    package,
                    pathContext.PackageSource,
                    new Uri(repoServiceIndex),
                    timestampService: null,
                    packageOwners);
                string testDirectory = pathContext.WorkingDirectory;

                string trustedSignersSectionContent = $@"
    <trustedSigners>
    <repository name=""NuGetTrust"" serviceIndex=""{repoServiceIndex}"">
      <certificate fingerprint=""{certFingerprint}"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""{allowUntrustedRoot}"" />
      <owners>nuget;Contoso</owners>
    </repository>
    </trustedSigners>
";
                SimpleTestSettingsContext.AddSectionIntoNuGetConfig(testDirectory, trustedSignersSectionContent, "configuration");
                string fingerprint = verifyCertificateFingerprint ? $"--certificate-fingerprint {certFingerprint} --certificate-fingerprint DEF" : string.Empty;

                //Act
                CommandRunnerResult verifyResult = _msbuildFixture.RunDotnet(
                    testDirectory,
                    $"nuget verify {signedPackagePath} {fingerprint}",
                    ignoreExitCode: true);

                // Assert
                // For certificate with trusted root setting allowUntrustedRoot value true/false doesn't matter.
                // Owners is casesensitive, here owner "nuget" matches
                verifyResult.Success.Should().BeTrue();
                verifyResult.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [CIOnlyFact]
        public async Task VerifyCommand_AuthorSignedPackage_WithUntrustedCertificate_AllowUntrustedRootIsSetTrue_WrongNugetConfig_Fails()
        {
            IX509StoreCertificate storeCertificate = _signFixture.UntrustedSelfIssuedCertificateInCertificateStore;

            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");

                //Act
                string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(storeCertificate.Certificate, nupkg, pathContext.WorkingDirectory);
                string certificateFingerprintString = SignatureTestUtility.GetFingerprint(storeCertificate.Certificate, HashAlgorithmName.SHA256);

                // Arrange
                string nugetConfigPath = Path.Combine(pathContext.WorkingDirectory, NuGet.Configuration.Settings.DefaultSettingsFileName);
                string nugetConfigPath2 = Path.Combine(pathContext.WorkingDirectory, "nuget2.config");
                // nuget2.config doesn't have change for trustedSigners
                File.Copy(nugetConfigPath, nugetConfigPath2);

                string trustedSignersSectionContent = $@"
    <trustedSigners>
        <author name=""MyCert"">
            <certificate fingerprint=""{certificateFingerprintString}"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true"" />
        </author>
    </trustedSigners>
";
                SimpleTestSettingsContext.AddSectionIntoNuGetConfig(pathContext.WorkingDirectory, trustedSignersSectionContent, "configuration");

                //Act
                // pass custom nuget2.config file, but doesn't have trustedSigners section
                CommandRunnerResult verifyResult = _msbuildFixture.RunDotnet(
                    pathContext.WorkingDirectory,
                    $"nuget verify {signedPackagePath} --all --certificate-fingerprint {certificateFingerprintString} --certificate-fingerprint def --configfile {nugetConfigPath2}",
                    ignoreExitCode: true);

                // Assert
                // allowUntrustedRoot is not set true in nuget2.config, but in nuget.config, so verify fails.
                verifyResult.Success.Should().BeFalse();
                verifyResult.AllOutput.Should().Contain(_noTimestamperWarningCode);
                verifyResult.AllOutput.Should().Contain(_primarySignatureInvalidErrorCode);
            }
        }

        [CIOnlyFact]
        public async Task VerifyCommand_AuthorSignedPackage_WithUntrustedCertificate_AllowUntrustedRootIsSetTrue_CorrectNugetConfig_Succeed()
        {
            IX509StoreCertificate storeCertificate = _signFixture.UntrustedSelfIssuedCertificateInCertificateStore;

            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");

                //Act
                string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(storeCertificate.Certificate, nupkg, pathContext.WorkingDirectory);
                string certificateFingerprintString = SignatureTestUtility.GetFingerprint(storeCertificate.Certificate, HashAlgorithmName.SHA256);

                // Arrange
                string nugetConfigPath = Path.Combine(pathContext.WorkingDirectory, NuGet.Configuration.Settings.DefaultSettingsFileName);
                string nugetConfigPath2 = Path.Combine(pathContext.WorkingDirectory, "nuget2.config");
                File.Copy(nugetConfigPath, nugetConfigPath2);

                string trustedSignersSectionContent = $@"
    <trustedSigners>
        <author name=""MyCert"">
            <certificate fingerprint=""{certificateFingerprintString}"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true"" />
        </author>
    </trustedSigners>
";
                SimpleTestSettingsContext.AddSectionIntoNuGetConfig(nugetConfigPath2, trustedSignersSectionContent, "configuration");

                //Act
                // pass custom nuget2.config file, it has trustedSigners section
                CommandRunnerResult verifyResult = _msbuildFixture.RunDotnet(
                    pathContext.WorkingDirectory,
                    $"nuget verify {signedPackagePath} --all --certificate-fingerprint {certificateFingerprintString} --certificate-fingerprint def --configfile {nugetConfigPath2}",
                    ignoreExitCode: true);

                // Assert
                // allowUntrustedRoot is set true in nuget2.config, so verify succeeds.
                verifyResult.Success.Should().BeTrue();
                verifyResult.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }
    }
}
