// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using Microsoft.Internal.NuGet.Testing.SignedPackages.ChildProcess;
using NuGet.Common;
using NuGet.Test.Utility;
using Test.Utility.Signing;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.CommandLine.FuncTest.Commands
{
    /// <summary>
    /// Tests Sign command
    /// These tests require admin privilege as the certs need to be added to the root store location
    /// </summary>
    [Collection(SignCommandTestCollection.Name)]
    public class VerifyCommandTests
    {
        private readonly string _noTimestamperWarningCode = NuGetLogCode.NU3027.ToString();
        private readonly string _primarySignatureInvalidErrorCode = NuGetLogCode.NU3018.ToString();
        private readonly string _signingDefaultErrorCode = NuGetLogCode.NU3000.ToString();
        private readonly string _noMatchingCertErrorCode = NuGetLogCode.NU3034.ToString();

        private SignCommandTestFixture _testFixture;
        private readonly ITestOutputHelper _testOutputHelper;
        private TrustedTestCert<TestCertificate> _trustedTestCert;
        private string _nugetExePath;

        public VerifyCommandTests(SignCommandTestFixture fixture, ITestOutputHelper testOutputHelper)
        {
            _testFixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _testOutputHelper = testOutputHelper;
            _trustedTestCert = _testFixture.TrustedTestCertificate;
            _nugetExePath = _testFixture.NuGetExePath;
        }

        [CIOnlyFact]
        public async Task VerifyCommand_VerifySignedPackageSucceedsAsync()
        {
            // Arrange
            var package = new SimpleTestPackageContext();
            using (var dir = TestDirectory.Create())
            using (var zipStream = await package.CreateAsStreamAsync())
            {
                var packagePath = Path.Combine(dir, Guid.NewGuid().ToString());

                zipStream.Seek(offset: 0, loc: SeekOrigin.Begin);

                using (var fileStream = File.OpenWrite(packagePath))
                {
                    zipStream.CopyTo(fileStream);
                }

                var signResult = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"sign {packagePath} -CertificateFingerprint {_trustedTestCert.Source.Cert.Thumbprint} -CertificateStoreName {_trustedTestCert.StoreName} -CertificateStoreLocation {_trustedTestCert.StoreLocation}",
                    testOutputHelper: _testOutputHelper);

                signResult.Success.Should().BeTrue();

                // Act
                var verifyResult = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"verify {packagePath} -Signatures",
                    testOutputHelper: _testOutputHelper);

                // Assert
                verifyResult.Success.Should().BeTrue();
                verifyResult.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [CIOnlyFact]
        public async Task VerifyCommand_VerifySignedAndTimestampedPackageSucceedsAsync()
        {
            // Arrange
            var timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();
            var package = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var zipStream = await package.CreateAsStreamAsync())
            {
                var packagePath = Path.Combine(dir, Guid.NewGuid().ToString());

                zipStream.Seek(offset: 0, loc: SeekOrigin.Begin);

                using (var fileStream = File.OpenWrite(packagePath))
                {
                    zipStream.CopyTo(fileStream);
                }

                var signResult = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"sign {packagePath} -Timestamper {timestampService.Url.OriginalString} -CertificateFingerprint {_trustedTestCert.Source.Cert.Thumbprint} -CertificateStoreName {_trustedTestCert.StoreName} -CertificateStoreLocation {_trustedTestCert.StoreLocation}",
                    testOutputHelper: _testOutputHelper);

                signResult.Success.Should().BeTrue();

                // Act
                var verifyResult = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"verify {packagePath} -Signatures",
                    testOutputHelper: _testOutputHelper);

                // Assert
                verifyResult.Success.Should().BeTrue();
                verifyResult.AllOutput.Should().NotContain(_noTimestamperWarningCode);
            }
        }

        [CIOnlyFact]
        public async Task VerifyCommand_VerifyResignedPackageSucceedsAsync()
        {
            // Arrange
            var package = new SimpleTestPackageContext();
            using (var dir = TestDirectory.Create())
            using (var zipStream = await package.CreateAsStreamAsync())
            {
                var packagePath = Path.Combine(dir, Guid.NewGuid().ToString());

                zipStream.Seek(offset: 0, loc: SeekOrigin.Begin);

                using (var fileStream = File.OpenWrite(packagePath))
                {
                    zipStream.CopyTo(fileStream);
                }

                var firstResult = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"sign {packagePath} -CertificateFingerprint {_trustedTestCert.Source.Cert.Thumbprint} -CertificateStoreName {_trustedTestCert.StoreName} -CertificateStoreLocation {_trustedTestCert.StoreLocation}",
                    testOutputHelper: _testOutputHelper);

                var secondResult = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"sign {packagePath} -CertificateFingerprint {_trustedTestCert.Source.Cert.Thumbprint} -CertificateStoreName {_trustedTestCert.StoreName} -CertificateStoreLocation {_trustedTestCert.StoreLocation} -Overwrite",
                    testOutputHelper: _testOutputHelper);

                firstResult.Success.Should().BeTrue();
                secondResult.Success.Should().BeTrue();

                // Act
                var verifyResult = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"verify {packagePath} -Signatures",
                    testOutputHelper: _testOutputHelper);

                // Assert
                verifyResult.Success.Should().BeTrue();
                verifyResult.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [CIOnlyFact]
        public async Task VerifyCommand_VerifyOnPackageSignedWithValidCertificateChainSucceedsAsync()
        {
            // Arrange
            var cert = _testFixture.TrustedTestCertificateChain.Leaf;
            var package = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var zipStream = await package.CreateAsStreamAsync())
            {
                var packagePath = Path.Combine(dir, Guid.NewGuid().ToString());

                zipStream.Seek(offset: 0, loc: SeekOrigin.Begin);

                using (var fileStream = File.OpenWrite(packagePath))
                {
                    zipStream.CopyTo(fileStream);
                }

                var signResult = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"sign {packagePath} -CertificateFingerprint {cert.Source.Cert.Thumbprint} -CertificateStoreName {cert.StoreName} -CertificateStoreLocation {cert.StoreLocation}",
                    testOutputHelper: _testOutputHelper);

                signResult.Success.Should().BeTrue();

                // Act
                var verifyResult = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"verify {packagePath} -Signatures",
                    testOutputHelper: _testOutputHelper);

                // Assert
                verifyResult.Success.Should().BeTrue();
                verifyResult.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [CIOnlyFact]
        public async Task VerifyCommand_VerifyOnPackageSignedWithAllowedCertificateSucceedsAsync()
        {
            // Arrange
            var cert = _testFixture.TrustedTestCertificateChain.Leaf;
            var package = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var zipStream = await package.CreateAsStreamAsync())
            {
                var packagePath = Path.Combine(dir, Guid.NewGuid().ToString());

                zipStream.Seek(offset: 0, loc: SeekOrigin.Begin);

                using (var fileStream = File.OpenWrite(packagePath))
                {
                    zipStream.CopyTo(fileStream);
                }

                var signResult = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"sign {packagePath} -CertificateFingerprint {cert.Source.Cert.Thumbprint} -CertificateStoreName {cert.StoreName} -CertificateStoreLocation {cert.StoreLocation}",
                    testOutputHelper: _testOutputHelper);

                signResult.Success.Should().BeTrue();

                var certificateFingerprintString = SignatureTestUtility.GetFingerprint(cert.Source.Cert, HashAlgorithmName.SHA256);

                // Act
                var verifyResult = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"verify {packagePath} -Signatures -CertificateFingerprint {certificateFingerprintString};abc;def",
                    testOutputHelper: _testOutputHelper);

                // Assert
                verifyResult.Success.Should().BeTrue();
                verifyResult.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [CIOnlyFact]
        public async Task VerifyCommand_VerifyOnPackageSignedWithoutAllowedCertificateFailsAsync()
        {
            // Arrange
            var cert = _testFixture.TrustedTestCertificateChain.Leaf;
            var package = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var zipStream = await package.CreateAsStreamAsync())
            {
                var packagePath = Path.Combine(dir, Guid.NewGuid().ToString());

                zipStream.Seek(offset: 0, loc: SeekOrigin.Begin);

                using (var fileStream = File.OpenWrite(packagePath))
                {
                    zipStream.CopyTo(fileStream);
                }

                var signResult = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"sign {packagePath} -CertificateFingerprint {cert.Source.Cert.Thumbprint} -CertificateStoreName {cert.StoreName} -CertificateStoreLocation {cert.StoreLocation}",
                    testOutputHelper: _testOutputHelper);

                signResult.Success.Should().BeTrue();

                // Act
                var verifyResult = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"verify {packagePath} -Signatures -CertificateFingerprint abc;def",
                    testOutputHelper: _testOutputHelper);

                // Assert
                verifyResult.Success.Should().BeFalse();
                verifyResult.AllOutput.Should().Contain(_noTimestamperWarningCode);
                verifyResult.AllOutput.Should().Contain(_noMatchingCertErrorCode);
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
            TrustedTestCert<TestCertificate> cert = _testFixture.TrustedTestCertificateChain.Leaf;

            using (var pathContext = new SimpleTestPathContext())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");
                string testDirectory = pathContext.WorkingDirectory;
                await SimpleTestPackageUtility.CreatePackagesAsync(testDirectory, nupkg);

                //Act
                string certificateFingerprintString = SignatureTestUtility.GetFingerprint(cert.Source.Cert, HashAlgorithmName.SHA256);
                string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(cert.Source.Cert, nupkg, testDirectory);

                // Arrange
                string trustedSignersSectionContent = $@"
    <trustedSigners>
        <author name=""signed"">
            <certificate fingerprint=""{certificateFingerprintString}"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""{allowUntrustedRoot}"" />
        </author>
    </trustedSigners>
";
                SimpleTestSettingsContext.AddSectionIntoNuGetConfig(testDirectory, trustedSignersSectionContent, "configuration");
                string fingerprint = verifyCertificateFingerprint ? $"-CertificateFingerprint {certificateFingerprintString};def" : string.Empty;

                // Act
                CommandRunnerResult verifyResult = CommandRunner.Run(
                    _nugetExePath,
                    testDirectory,
                    $"verify {signedPackagePath} -Signatures {fingerprint}",
                    testOutputHelper: _testOutputHelper);

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
            TrustedTestCert<TestCertificate> cert = _testFixture.TrustedTestCertificateChain.Leaf;

            using (var pathContext = new SimpleTestPathContext())
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");
                string testDirectory = pathContext.WorkingDirectory;
                await SimpleTestPackageUtility.CreatePackagesAsync(testDirectory, nupkg);

                //Act
                string certificateFingerprintString = SignatureTestUtility.GetFingerprint(cert.Source.Cert, HashAlgorithmName.SHA256);
                string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(cert.Source.Cert, nupkg, testDirectory);

                // Arrange
                string trustedSignersSectionContent = $@"
    <trustedSigners>
        <repository name=""MyCert"" serviceIndex=""{pathContext.PackageSource}"">
            <certificate fingerprint=""{certificateFingerprintString}"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""{allowUntrustedRoot}"" />
        </repository>
    </trustedSigners>
";
                SimpleTestSettingsContext.AddSectionIntoNuGetConfig(pathContext.WorkingDirectory, trustedSignersSectionContent, "configuration");
                string fingerprint = verifyCertificateFingerprint ? $"-CertificateFingerprint {certificateFingerprintString};def" : string.Empty;

                // Act
                CommandRunnerResult verifyResult = CommandRunner.Run(
                    _nugetExePath,
                    pathContext.PackageSource,
                    $"verify {signedPackagePath} -Signatures {fingerprint}",
                    testOutputHelper: _testOutputHelper);

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
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            using (var testCertificate = new X509Certificate2(_testFixture.UntrustedSelfIssuedCertificateInCertificateStore))
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");
                string testDirectory = pathContext.WorkingDirectory;
                await SimpleTestPackageUtility.CreatePackagesAsync(testDirectory, nupkg);
                string packagePath = Path.Combine(testDirectory, nupkg.PackageName);

                //Act
                string certificateFingerprintString = SignatureTestUtility.GetFingerprint(testCertificate, HashAlgorithmName.SHA256);
                string repoServiceIndex = "https://serviceindex.test/v3/index.json";
                string signedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(testCertificate, packagePath, pathContext.PackageSource, new Uri(repoServiceIndex));

                // Arrange
                string trustedSignersSectionContent = $@"
    <trustedSigners>
        <author name=""MyCert"">
            <certificate fingerprint=""{certificateFingerprintString}"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""{allowUntrustedRoot}"" />
        </author>
    </trustedSigners>
";
                SimpleTestSettingsContext.AddSectionIntoNuGetConfig(pathContext.WorkingDirectory, trustedSignersSectionContent, "configuration");
                string fingerprint = verifyCertificateFingerprint ? $"-CertificateFingerprint {certificateFingerprintString};def" : string.Empty;

                // Act
                CommandRunnerResult verifyResult = CommandRunner.Run(
                    _nugetExePath,
                    pathContext.PackageSource,
                    $"verify {signedPackagePath} -Signatures {fingerprint}",
                    testOutputHelper: _testOutputHelper);

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
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            using (var testCertificate = new X509Certificate2(_testFixture.UntrustedSelfIssuedCertificateInCertificateStore))
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");
                string testDirectory = pathContext.WorkingDirectory;
                await SimpleTestPackageUtility.CreatePackagesAsync(testDirectory, nupkg);
                string packagePath = Path.Combine(testDirectory, nupkg.PackageName);

                //Act
                string certificateFingerprintString = SignatureTestUtility.GetFingerprint(testCertificate, HashAlgorithmName.SHA256);
                string repoServiceIndex = "https://serviceindex.test/v3/index.json";
                string signedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(testCertificate, packagePath, pathContext.PackageSource, new Uri(repoServiceIndex));

                // Arrange
                string trustedSignersSectionContent = $@"
    <trustedSigners>
        <repository name=""MyCert"" serviceIndex = ""{repoServiceIndex}"">
            <certificate fingerprint=""{certificateFingerprintString}"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""{allowUntrustedRoot}"" />
        </repository>
    </trustedSigners>
";
                SimpleTestSettingsContext.AddSectionIntoNuGetConfig(pathContext.WorkingDirectory, trustedSignersSectionContent, "configuration");
                string fingerprint = verifyCertificateFingerprint ? $"-CertificateFingerprint {certificateFingerprintString};def" : string.Empty;

                // Act
                CommandRunnerResult verifyResult = CommandRunner.Run(
                    _nugetExePath,
                    pathContext.PackageSource,
                    $"verify {signedPackagePath} -Signatures {fingerprint}",
                    testOutputHelper: _testOutputHelper);

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
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            using (var testCertificate = new X509Certificate2(_testFixture.UntrustedSelfIssuedCertificateInCertificateStore))
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");
                string testDirectory = pathContext.WorkingDirectory;
                await SimpleTestPackageUtility.CreatePackagesAsync(testDirectory, nupkg);
                string packagePath = Path.Combine(testDirectory, nupkg.PackageName);

                //Act
                string certificateFingerprintString = SignatureTestUtility.GetFingerprint(testCertificate, HashAlgorithmName.SHA256);
                string repoServiceIndex = "https://serviceindex.test/v3/index.json";
                string signedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(testCertificate, packagePath, pathContext.PackageSource, new Uri(repoServiceIndex));

                // Arrange
                string trustedSignersSectionContent = $@"
    <trustedSigners>
        <repository name=""MyCert"" serviceIndex = ""{repoServiceIndex}"">
            <certificate fingerprint=""{certificateFingerprintString}"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""{allowUntrustedRoot}"" />
        </repository>
    </trustedSigners>
";
                SimpleTestSettingsContext.AddSectionIntoNuGetConfig(pathContext.WorkingDirectory, trustedSignersSectionContent, "configuration");
                string fingerprint = verifyCertificateFingerprint ? $"-CertificateFingerprint {certificateFingerprintString};def" : string.Empty;

                // Act
                CommandRunnerResult verifyResult = CommandRunner.Run(
                    _nugetExePath,
                    pathContext.PackageSource,
                    $"verify {signedPackagePath} -Signatures {fingerprint}",
                    testOutputHelper: _testOutputHelper);

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
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            using (var testCertificate = new X509Certificate2(_testFixture.UntrustedSelfIssuedCertificateInCertificateStore))
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");
                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.WorkingDirectory, nupkg);
                string packagePath = Path.Combine(pathContext.WorkingDirectory, nupkg.PackageName);

                //Act
                var certificateFingerprintString = SignatureTestUtility.GetFingerprint(testCertificate, HashAlgorithmName.SHA256);
                var packageOwners = new List<string>()
                {
                    "nuget",
                    "contoso"
                };
                string repoServiceIndex = "https://serviceindex.test/v3/index.json";
                string signedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(testCertificate, packagePath, pathContext.PackageSource, new Uri(repoServiceIndex), null, packageOwners);

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
                string fingerprint = verifyCertificateFingerprint ? $"-CertificateFingerprint {certificateFingerprintString};def" : string.Empty;

                // Act
                CommandRunnerResult verifyResult = CommandRunner.Run(
                    _nugetExePath,
                    pathContext.PackageSource,
                    $"verify {signedPackagePath} -Signatures {fingerprint}",
                    testOutputHelper: _testOutputHelper);

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
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            using (var testCertificate = new X509Certificate2(_testFixture.UntrustedSelfIssuedCertificateInCertificateStore))
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");
                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.WorkingDirectory, nupkg);
                string packagePath = Path.Combine(pathContext.WorkingDirectory, nupkg.PackageName);

                //Act
                string certificateFingerprintString = SignatureTestUtility.GetFingerprint(testCertificate, HashAlgorithmName.SHA256);
                var packageOwners = new List<string>()
                {
                    "nuget",
                    "contoso"
                };
                string repoServiceIndex = "https://serviceindex.test/v3/index.json";
                string signedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(testCertificate, packagePath, pathContext.PackageSource, new Uri(repoServiceIndex), null, packageOwners);

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
                string fingerprint = verifyCertificateFingerprint ? $"-CertificateFingerprint {certificateFingerprintString};def" : string.Empty;

                // Act
                CommandRunnerResult verifyResult = CommandRunner.Run(
                    _nugetExePath,
                    pathContext.PackageSource,
                    $"verify {signedPackagePath} -Signatures {fingerprint}",
                    testOutputHelper: _testOutputHelper);

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
            TrustedTestCert<TestCertificate> cert = _testFixture.TrustedTestCertificateChain.Leaf;

            using (var pathContext = new SimpleTestPathContext())
            {
                var package = new SimpleTestPackageContext();
                string certFingerprint = SignatureTestUtility.GetFingerprint(cert.Source.Cert, HashAlgorithmName.SHA256);
                var packageOwners = new List<string>()
                {
                    "nuget",
                    "contoso"
                };
                string repoServiceIndex = "https://serviceindex.test/v3/index.json";
                string signedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(cert.Source.Cert, package, pathContext.PackageSource, new Uri(repoServiceIndex), null, packageOwners);

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
                string fingerprint = verifyCertificateFingerprint ? $"-CertificateFingerprint {certFingerprint};def" : string.Empty;

                //Act
                CommandRunnerResult verifyResult = CommandRunner.Run(
                    _nugetExePath,
                    pathContext.PackageSource,
                    $"verify {signedPackagePath} -Signatures {fingerprint}",
                    testOutputHelper: _testOutputHelper);

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
            TrustedTestCert<TestCertificate> cert = _testFixture.TrustedTestCertificateChain.Leaf;

            using (var pathContext = new SimpleTestPathContext())
            {
                var package = new SimpleTestPackageContext();
                string certFingerprint = SignatureTestUtility.GetFingerprint(cert.Source.Cert, HashAlgorithmName.SHA256);
                var packageOwners = new List<string>()
                {
                    "nuget",
                    "contoso"
                };
                string repoServiceIndex = "https://serviceindex.test/v3/index.json";
                string signedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(cert.Source.Cert, package, pathContext.PackageSource, new Uri(repoServiceIndex), null, packageOwners);

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
                string fingerprint = verifyCertificateFingerprint ? $"-CertificateFingerprint {certFingerprint};def" : string.Empty;

                //Act
                CommandRunnerResult verifyResult = CommandRunner.Run(
                    _nugetExePath,
                    pathContext.PackageSource,
                    $"verify {signedPackagePath} -Signatures {fingerprint}",
                    testOutputHelper: _testOutputHelper);

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
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            using (var testCertificate = new X509Certificate2(_testFixture.UntrustedSelfIssuedCertificateInCertificateStore))
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");

                //Act
                string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, pathContext.WorkingDirectory);
                string certificateFingerprintString = SignatureTestUtility.GetFingerprint(testCertificate, HashAlgorithmName.SHA256);

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

                // Act
                // pass custom nuget2.config file, but doesn't have trustedSigners section
                CommandRunnerResult verifyResult = CommandRunner.Run(
                    _nugetExePath,
                    pathContext.PackageSource,
                    $"verify {signedPackagePath} -All -CertificateFingerprint {certificateFingerprintString};def -ConfigFile {nugetConfigPath2}",
                    testOutputHelper: _testOutputHelper);

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
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            using (var testCertificate = new X509Certificate2(_testFixture.UntrustedSelfIssuedCertificateInCertificateStore))
            {
                var nupkg = new SimpleTestPackageContext("A", "1.0.0");

                //Act
                string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, pathContext.WorkingDirectory);
                string certificateFingerprintString = SignatureTestUtility.GetFingerprint(testCertificate, HashAlgorithmName.SHA256);

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

                // Act
                // pass custom nuget2.config file, it has trustedSigners section
                CommandRunnerResult verifyResult = CommandRunner.Run(
                    _nugetExePath,
                    pathContext.PackageSource,
                    $"verify {signedPackagePath} -All -CertificateFingerprint {certificateFingerprintString};def -ConfigFile {nugetConfigPath2}",
                    testOutputHelper: _testOutputHelper);

                // Assert
                // allowUntrustedRoot is set true in nuget2.config, so verify succeeds.
                verifyResult.Success.Should().BeTrue();
                verifyResult.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }
    }
}
