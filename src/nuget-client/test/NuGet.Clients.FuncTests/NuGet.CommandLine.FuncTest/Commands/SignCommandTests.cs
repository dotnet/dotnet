// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Test.Utility.Signing;
using Xunit;

namespace NuGet.CommandLine.FuncTest.Commands
{
    /// <summary>
    /// Tests Sign command
    /// These tests require admin privilege as the certs need to be added to the root store location
    /// </summary>
    [Collection(SignCommandTestCollection.Name)]
    public class SignCommandTests
    {
        private const string _packageAlreadySignedError = "NU3001: The package already contains a signature. Please remove the existing signature before adding a new signature.";
        private readonly string _invalidPasswordErrorCode = NuGetLogCode.NU3001.ToString();
        private readonly string _chainBuildFailureErrorCode = NuGetLogCode.NU3018.ToString();
        private readonly string _noCertFoundErrorCode = NuGetLogCode.NU3001.ToString();
        private readonly string _noTimestamperWarningCode = NuGetLogCode.NU3002.ToString();
        private readonly string _timestampUnsupportedDigestAlgorithmCode = NuGetLogCode.NU3024.ToString();

        private SignCommandTestFixture _testFixture;
        private TrustedTestCert<TestCertificate> _trustedTestCert;
        private string _nugetExePath;

        public SignCommandTests(SignCommandTestFixture fixture)
        {
            _testFixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _trustedTestCert = _testFixture.TrustedTestCertificate;
            _nugetExePath = _testFixture.NuGetExePath;
        }

        [CIOnlyFact]
        public async Task SignCommand_SignPackageAsync()
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

                // Act
                var result = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"sign {packagePath} -CertificateFingerprint {_trustedTestCert.Source.Cert.Thumbprint} -CertificateStoreName {_trustedTestCert.StoreName} -CertificateStoreLocation {_trustedTestCert.StoreLocation}");

                // Assert
                result.Success.Should().BeTrue();
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [CIOnlyFact]
        public async Task SignCommand_SignPackageWithInvalidEkuFailsAsync()
        {
            // Arrange
            var invalidEkuCert = _testFixture.TrustedTestCertificateWithInvalidEku;
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

                // Act
                var result = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"sign {packagePath} -CertificateFingerprint {invalidEkuCert.Source.Cert.Thumbprint} -CertificateStoreName {invalidEkuCert.StoreName} -CertificateStoreLocation {invalidEkuCert.StoreLocation}");

                // Assert
                result.Success.Should().BeFalse();
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [CIOnlyFact]
        public async Task SignCommand_SignPackageWithExpiredCertificateFailsAsync()
        {
            // Arrange
            var expiredCert = _testFixture.TrustedTestCertificateExpired;
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

                // Act
                var result = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"sign {packagePath} -CertificateFingerprint {expiredCert.Source.Cert.Thumbprint} -CertificateStoreName {expiredCert.StoreName} -CertificateStoreLocation {expiredCert.StoreLocation}");

                // Assert
                result.Success.Should().BeFalse();
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
                result.AllOutput.Should().Contain(_noCertFoundErrorCode);
            }
        }

        [CIOnlyFact]
        public async Task SignCommand_SignPackageWithNotYetValidCertificateFailsAsync()
        {
            // Arrange
            var cert = _testFixture.TrustedTestCertificateNotYetValid;
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

                // Act
                var result = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"sign {packagePath} -CertificateFingerprint {cert.Source.Cert.Thumbprint} -CertificateStoreName {cert.StoreName} -CertificateStoreLocation {cert.StoreLocation}");

                // Assert
                result.Success.Should().BeFalse();
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
                result.AllOutput.Should().Contain(_noCertFoundErrorCode);
            }
        }

        [CIOnlyFact]
        public async Task SignCommand_SignPackageWithTimestampingAsync()
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

                // Act
                var result = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"sign {packagePath} -CertificateFingerprint {_trustedTestCert.Source.Cert.Thumbprint}  -CertificateStoreName {_trustedTestCert.StoreName} -CertificateStoreLocation {_trustedTestCert.StoreLocation} -Timestamper {timestampService.Url.OriginalString}");

                // Assert
                result.Success.Should().BeTrue();
                result.AllOutput.Should().NotContain(_noTimestamperWarningCode);
            }
        }

        [CIOnlyFact]
        public async Task SignCommand_SignPackageWithValidCertChainAsync()
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

                // Act
                var result = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"sign {packagePath} -CertificateFingerprint {cert.Source.Cert.Thumbprint}  -CertificateStoreName {cert.StoreName} -CertificateStoreLocation {cert.StoreLocation}");

                // Assert
                result.Success.Should().BeTrue();
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [CIOnlyFact]
        public async Task SignCommand_SignPackageWithRevokedLeafCertChainAsync()
        {
            // Arrange
            var cert = _testFixture.RevokedTestCertificateWithChain;
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

                // Act
                var result = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"sign {packagePath} -CertificateFingerprint {cert.Source.Cert.Thumbprint}  -CertificateStoreName {cert.StoreName} -CertificateStoreLocation {cert.StoreLocation}");

                // Assert
                result.Success.Should().BeFalse();
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [CIOnlyFact]
        public async Task SignCommand_SignPackageWithUnknownRevocationCertChainAsync()
        {
            // Arrange
            var cert = _testFixture.RevocationUnknownTestCertificateWithChain;
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

                // Act
                var result = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"sign {packagePath} -CertificateFingerprint {cert.Source.Cert.Thumbprint} -CertificateStoreName {cert.StoreName} -CertificateStoreLocation {cert.StoreLocation}");

                // Assert
                result.Success.Should().BeTrue();
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
                result.AllOutput.Should().Contain(_chainBuildFailureErrorCode);
                result.AllOutput.Should().Contain("The revocation function was unable to check revocation for the certificate");
            }
        }

        [CIOnlyFact]
        public async Task SignCommand_SignPackageWithOutputDirectoryAsync()
        {
            // Arrange
            var package = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var outputDir = TestDirectory.Create())
            using (var zipStream = await package.CreateAsStreamAsync())
            {
                var packageFileName = Guid.NewGuid().ToString();
                var packagePath = Path.Combine(dir, packageFileName);
                var signedPackagePath = Path.Combine(outputDir, packageFileName);

                zipStream.Seek(offset: 0, loc: SeekOrigin.Begin);

                using (var fileStream = File.OpenWrite(packagePath))
                {
                    zipStream.CopyTo(fileStream);
                }

                // Act
                var result = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"sign {packagePath} -CertificateFingerprint {_trustedTestCert.Source.Cert.Thumbprint}  -CertificateStoreName {_trustedTestCert.StoreName} -CertificateStoreLocation {_trustedTestCert.StoreLocation} -OutputDirectory {outputDir}");

                // Assert
                result.Success.Should().BeTrue();
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
                File.Exists(signedPackagePath).Should().BeTrue();
            }
        }

        [CIOnlyFact]
        public async Task SignCommand_ResignPackageWithoutOverwriteFailsAsync()
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
                    $"sign {packagePath} -CertificateFingerprint {_trustedTestCert.Source.Cert.Thumbprint}  -CertificateStoreName {_trustedTestCert.StoreName} -CertificateStoreLocation {_trustedTestCert.StoreLocation}");

                // Act
                var secondResult = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"sign {packagePath} -CertificateFingerprint {_trustedTestCert.Source.Cert.Thumbprint}  -CertificateStoreName {_trustedTestCert.StoreName} -CertificateStoreLocation {_trustedTestCert.StoreLocation}");

                // Assert
                firstResult.Success.Should().BeTrue();
                firstResult.AllOutput.Should().Contain(_noTimestamperWarningCode);
                secondResult.Success.Should().BeFalse();
                secondResult.Errors.Should().Contain(_packageAlreadySignedError);
            }
        }

        [CIOnlyFact]
        public async Task SignCommand_ResignPackageWithOverwriteSuccessAsync()
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

                // Act
                var firstResult = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"sign {packagePath} -CertificateFingerprint {_trustedTestCert.Source.Cert.Thumbprint} -CertificateStoreName {_trustedTestCert.StoreName} -CertificateStoreLocation {_trustedTestCert.StoreLocation}");

                var secondResult = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"sign {packagePath} -CertificateFingerprint {_trustedTestCert.Source.Cert.Thumbprint} -CertificateStoreName {_trustedTestCert.StoreName} -CertificateStoreLocation {_trustedTestCert.StoreLocation} -Overwrite");

                // Assert
                firstResult.Success.Should().BeTrue();
                firstResult.AllOutput.Should().Contain(_noTimestamperWarningCode);
                secondResult.Success.Should().BeTrue();
                secondResult.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [CIOnlyFact]
        public async Task SignCommand_SignPackageWithOverwriteSuccessAsync()
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

                // Act
                var firstResult = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"sign {packagePath} -CertificateFingerprint {_trustedTestCert.Source.Cert.Thumbprint} -CertificateStoreName {_trustedTestCert.StoreName} -CertificateStoreLocation {_trustedTestCert.StoreLocation} -Overwrite");

                // Assert
                firstResult.Success.Should().BeTrue();
                firstResult.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [CIOnlyFact]
        public async Task SignCommand_SignPackageWithPfxFileSuccessAsync()
        {
            // Arrange
            var package = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var zipStream = await package.CreateAsStreamAsync())
            {
                var packagePath = Path.Combine(dir, Guid.NewGuid().ToString());
                var pfxPath = Path.Combine(dir, Guid.NewGuid().ToString());

                var password = Guid.NewGuid().ToString();
                var pfxBytes = _trustedTestCert.Source.Cert.Export(X509ContentType.Pfx, password);

                using (var fileStream = File.OpenWrite(pfxPath))
                using (var pfxStream = new MemoryStream(pfxBytes))
                {
                    pfxStream.CopyTo(fileStream);
                }

                zipStream.Seek(offset: 0, loc: SeekOrigin.Begin);

                using (var fileStream = File.OpenWrite(packagePath))
                {
                    zipStream.CopyTo(fileStream);
                }

                // Act
                var firstResult = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"sign {packagePath} -CertificatePath {pfxPath} -CertificatePassword {password}");

                // Assert
                firstResult.Success.Should().BeTrue();
                firstResult.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [CIOnlyFact]
        public async Task SignCommand_SignPackageWithPfxFileInteractiveSuccessAsync()
        {
            // Arrange
            var package = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var zipStream = await package.CreateAsStreamAsync())
            {
                var packagePath = Path.Combine(dir, Guid.NewGuid().ToString());
                var pfxPath = Path.Combine(dir, Guid.NewGuid().ToString());

                var password = Guid.NewGuid().ToString();
                var pfxBytes = _trustedTestCert.Source.Cert.Export(X509ContentType.Pfx, password);

                using (var fileStream = File.OpenWrite(pfxPath))
                using (var pfxStream = new MemoryStream(pfxBytes))
                {
                    pfxStream.CopyTo(fileStream);
                }

                zipStream.Seek(offset: 0, loc: SeekOrigin.Begin);

                using (var fileStream = File.OpenWrite(packagePath))
                {
                    zipStream.CopyTo(fileStream);
                }

                // Act
                var firstResult = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"sign {packagePath} -CertificatePath {pfxPath}",
                    inputAction: (w) =>
                    {
                        w.WriteLine(password);
                    });

                // Assert
                firstResult.Success.Should().BeTrue();
                firstResult.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [CIOnlyFact]
        public async Task SignCommand_SignPackageWithPfxFileInteractiveInvalidPasswordFailsAsync()
        {
            // Arrange
            var package = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var zipStream = await package.CreateAsStreamAsync())
            {
                var packagePath = Path.Combine(dir, Guid.NewGuid().ToString());
                var pfxPath = Path.Combine(dir, Guid.NewGuid().ToString());

                var password = Guid.NewGuid().ToString();
                var pfxBytes = _trustedTestCert.Source.Cert.Export(X509ContentType.Pfx, password);

                using (var fileStream = File.OpenWrite(pfxPath))
                using (var pfxStream = new MemoryStream(pfxBytes))
                {
                    pfxStream.CopyTo(fileStream);
                }

                zipStream.Seek(offset: 0, loc: SeekOrigin.Begin);

                using (var fileStream = File.OpenWrite(packagePath))
                {
                    zipStream.CopyTo(fileStream);
                }

                // Act
                var firstResult = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"sign {packagePath} -CertificatePath {pfxPath}",
                    inputAction: (w) =>
                    {
                        w.WriteLine(Guid.NewGuid().ToString());
                    });

                // Assert
                firstResult.Success.Should().BeFalse();
                firstResult.AllOutput.Should().Contain(string.Format(_invalidPasswordErrorCode, pfxPath));
            }
        }

        [CIOnlyFact]
        public async Task SignCommand_SignPackageWithPfxFileWithoutPasswordAndWithNonInteractiveFailsAsync()
        {
            // Arrange
            var package = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var zipStream = await package.CreateAsStreamAsync())
            {
                var packagePath = Path.Combine(dir, Guid.NewGuid().ToString());
                var pfxPath = Path.Combine(dir, Guid.NewGuid().ToString());

                var password = Guid.NewGuid().ToString();
                var pfxBytes = _trustedTestCert.Source.Cert.Export(X509ContentType.Pfx, password);

                using (var fileStream = File.OpenWrite(pfxPath))
                using (var pfxStream = new MemoryStream(pfxBytes))
                {
                    pfxStream.CopyTo(fileStream);
                }

                zipStream.Seek(offset: 0, loc: SeekOrigin.Begin);

                using (var fileStream = File.OpenWrite(packagePath))
                {
                    zipStream.CopyTo(fileStream);
                }

                // Act
                var firstResult = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"sign {packagePath} -CertificatePath {pfxPath} -NonInteractive");

                // Assert
                firstResult.Success.Should().BeFalse();
                firstResult.AllOutput.Should().Contain(string.Format(_invalidPasswordErrorCode, pfxPath));
            }
        }

        [CIOnlyFact]
        public async Task SignCommand_SignPackageWithPfxFileWithNonInteractiveAndStdInPasswordFailsAsync()
        {
            // Arrange
            var package = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var zipStream = await package.CreateAsStreamAsync())
            {
                var packagePath = Path.Combine(dir, Guid.NewGuid().ToString());
                var pfxPath = Path.Combine(dir, Guid.NewGuid().ToString());

                var password = Guid.NewGuid().ToString();
                var pfxBytes = _trustedTestCert.Source.Cert.Export(X509ContentType.Pfx, password);

                using (var fileStream = File.OpenWrite(pfxPath))
                using (var pfxStream = new MemoryStream(pfxBytes))
                {
                    pfxStream.CopyTo(fileStream);
                }

                zipStream.Seek(offset: 0, loc: SeekOrigin.Begin);

                using (var fileStream = File.OpenWrite(packagePath))
                {
                    zipStream.CopyTo(fileStream);
                }

                // Act
                var firstResult = CommandRunner.Run(
                    _nugetExePath,
                    dir,
                    $"sign {packagePath} -CertificatePath {pfxPath} -NonInteractive",
                    inputAction: (w) =>
                    {
                        w.WriteLine(Guid.NewGuid().ToString());
                    });

                // Assert
                firstResult.Success.Should().BeFalse();
                firstResult.AllOutput.Should().Contain(string.Format(_invalidPasswordErrorCode, pfxPath));
            }
        }

        [CIOnlyFact]
        public async Task SignCommand_SignPackageWithUntrustedSelfIssuedCertificateInCertificateStoreAsync()
        {
            using (var directory = TestDirectory.Create())
            {
                var packageContext = new SimpleTestPackageContext();
                var packageFile = await packageContext.CreateAsFileAsync(directory, fileName: Guid.NewGuid().ToString());

                using (var certificate = _testFixture.UntrustedSelfIssuedCertificateInCertificateStore)
                {
                    var result = CommandRunner.Run(
                        _nugetExePath,
                        directory,
                        $"sign {packageFile.FullName} -CertificateFingerprint {certificate.Thumbprint}");

                    Assert.True(result.Success);
                    Assert.Contains(_noTimestamperWarningCode, result.AllOutput);
                }
            }
        }

        [CIOnlyFact]
        public async Task SignCommand_SignPackageWithUnsuportedTimestampHashAlgorithm_ShouldNotModifyPackageAsync()
        {
            var testServer = await _testFixture.GetSigningTestServerAsync();
            var certificateAuthority = await _testFixture.GetDefaultTrustedCertificateAuthorityAsync();
            var options = new TimestampServiceOptions() { SignatureHashAlgorithm = new Oid(Oids.Sha1) };
            var timestampService = TimestampService.Create(certificateAuthority, options);

            using (testServer.RegisterResponder(timestampService))
            using (var directory = TestDirectory.Create())
            {
                var packageContext = new SimpleTestPackageContext();
                var packageFile = await packageContext.CreateAsFileAsync(directory, fileName: Guid.NewGuid().ToString());
                var originalFile = File.ReadAllBytes(packageFile.FullName);

                using (var certificate = _testFixture.UntrustedSelfIssuedCertificateInCertificateStore)
                {
                    var result = CommandRunner.Run(
                        _nugetExePath,
                        directory,
                        $"sign {packageFile.FullName} -CertificateFingerprint {certificate.Thumbprint} -Timestamper {timestampService.Url}");

                    Assert.False(result.Success);
                    Assert.Contains(_timestampUnsupportedDigestAlgorithmCode, result.AllOutput);
                    Assert.Contains("The timestamp signature has an unsupported digest algorithm (SHA1). The following algorithms are supported: SHA256, SHA384, SHA512.", result.AllOutput);

                    var resultingFile = File.ReadAllBytes(packageFile.FullName);
                    Assert.Equal(resultingFile, originalFile);
                }
            }
        }
    }
}
