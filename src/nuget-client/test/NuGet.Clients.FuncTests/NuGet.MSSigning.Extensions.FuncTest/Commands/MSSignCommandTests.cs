// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using Microsoft.Internal.NuGet.Testing.SignedPackages.ChildProcess;
using Moq;
using NuGet.CommandLine;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.MSSigning.Extensions.FuncTest.Commands
{
    /// <summary>
    /// Tests Sign command
    /// These tests require admin privilege as the certs need to be added to the root store location
    /// </summary>
    [Collection(MSSignCommandTestCollection.Name)]
    public class MSSignCommandTests
    {
        private readonly string _noTimestamperWarningCode = NuGetLogCode.NU3002.ToString();
        private readonly string _invalidCertificateFingerprintCode = NuGetLogCode.NU3043.ToString();
        private const string Sha256Hash = "a591a6d40bf420404a011733cfb7b190d62c65bf0bcda32b55b046cbb7f506fb";

        private readonly TrustedTestCert<TestCertificate> _trustedTestCertWithPrivateKey;
        private readonly TrustedTestCert<TestCertificate> _trustedTestCertWithoutPrivateKey;

        private MSSignCommandTestFixture _testFixture;
        private readonly string _nugetExePath;

        public MSSignCommandTests(MSSignCommandTestFixture fixture)
        {
            _testFixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _trustedTestCertWithPrivateKey = _testFixture.TrustedTestCertificateWithPrivateKey;
            _trustedTestCertWithoutPrivateKey = _testFixture.TrustedTestCertificateWithoutPrivateKey;
            _nugetExePath = _testFixture.NuGetExePath;
        }

        [CIOnlyFact]
        public void GetAuthorSignRequest_InvalidCertificateFile()
        {
            var mockConsole = new Mock<IConsole>();
            var timestampUri = "http://timestamp.test/url";

            // Arrange
            using (var dir = TestDirectory.Create())
            using (var test = new MSSignCommandTestContext(_trustedTestCertWithPrivateKey.TrustedCert))
            {
                var signCommand = new MSSignCommand
                {
                    Console = mockConsole.Object,
                    Timestamper = timestampUri,
                    CertificateFile = Path.Combine(dir, "non-existant-cert.pfx"),
                    CSPName = test.CertificateCSPName,
                    KeyContainer = test.CertificateKeyContainer,
                    CertificateFingerprint = SignatureTestUtility.GetFingerprint(test.Cert, Common.HashAlgorithmName.SHA256),
                };
                signCommand.Arguments.Add(Path.Combine(dir, "package.nupkg"));

                // Act & Assert
                var ex = Assert.Throws<CryptographicException>(() => signCommand.GetAuthorSignRequest());
                Assert.Contains("The system cannot find the file specified.", ex.Message);
            }
        }

        [CIOnlyFact]
        public void GetAuthorSignRequest_InvalidCSPName()
        {
            var mockConsole = new Mock<IConsole>();
            var timestampUri = "http://timestamp.test/url";

            // Arrange
            using (var dir = TestDirectory.Create())
            using (var test = new MSSignCommandTestContext(_trustedTestCertWithoutPrivateKey.TrustedCert, exportPfx: false))
            {
                var signCommand = new MSSignCommand
                {
                    Console = mockConsole.Object,
                    Timestamper = timestampUri,
                    CertificateFile = test.CertificatePath,
                    CSPName = "random nonexistant csp name",
                    KeyContainer = test.CertificateKeyContainer,
                    CertificateFingerprint = SignatureTestUtility.GetFingerprint(test.Cert, Common.HashAlgorithmName.SHA256),
                };
                signCommand.Arguments.Add(Path.Combine(dir, "package.nupkg"));

                // Act & Assert
                var ex = Assert.Throws<InvalidOperationException>(() => signCommand.GetAuthorSignRequest());
                Assert.Equal("Can't find cng key.", ex.Message);
            }
        }

        [CIOnlyFact]
        public void GetAuthorSignRequest_InvalidKeyContainer()
        {
            var mockConsole = new Mock<IConsole>();
            var timestampUri = "http://timestamp.test/url";

            // Arrange
            using (var dir = TestDirectory.Create())
            using (var test = new MSSignCommandTestContext(_trustedTestCertWithoutPrivateKey.TrustedCert, exportPfx: false))
            {
                var signCommand = new MSSignCommand
                {
                    Console = mockConsole.Object,
                    Timestamper = timestampUri,
                    CertificateFile = test.CertificatePath,
                    CSPName = test.CertificateCSPName,
                    KeyContainer = "invalid-key-container",
                    CertificateFingerprint = SignatureTestUtility.GetFingerprint(test.Cert, Common.HashAlgorithmName.SHA256),
                };
                signCommand.Arguments.Add(Path.Combine(dir, "package.nupkg"));

                // Act & Assert
                var ex = Assert.Throws<InvalidOperationException>(() => signCommand.GetAuthorSignRequest());
                Assert.Equal("Can't find cng key.", ex.Message);
            }
        }

        [CIOnlyFact]
        public void GetAuthorSignRequest_InvalidFingerprint()
        {
            var mockConsole = new Mock<IConsole>();
            var timestampUri = "http://timestamp.test/url";

            // Arrange
            using (var dir = TestDirectory.Create())
            using (var test = new MSSignCommandTestContext(_trustedTestCertWithPrivateKey.TrustedCert))
            {
                var signCommand = new MSSignCommand
                {
                    Console = mockConsole.Object,
                    Timestamper = timestampUri,
                    CertificateFile = test.CertificatePath,
                    CSPName = test.CertificateCSPName,
                    KeyContainer = test.CertificateKeyContainer,
                    CertificateFingerprint = Sha256Hash,
                };
                signCommand.Arguments.Add(Path.Combine(dir, "package.nupkg"));

                // Act & Assert
                var ex = Assert.Throws<InvalidOperationException>(() => signCommand.GetAuthorSignRequest());
                Assert.Equal("Can't find specified certificate.", ex.Message);
            }
        }

        [CIOnlyFact]
        public void GetAuthorSignRequest_Success()
        {
            var mockConsole = new Mock<IConsole>();
            var timestampUri = "http://timestamp.test/url";

            // Arrange
            using (var dir = TestDirectory.Create())
            using (var test = new MSSignCommandTestContext(_trustedTestCertWithPrivateKey.TrustedCert))
            {
                var actualCertFingerprint = SignatureTestUtility.GetFingerprint(test.Cert, Common.HashAlgorithmName.SHA256);

                var signCommand = new MSSignCommand
                {
                    Console = mockConsole.Object,
                    Timestamper = timestampUri,
                    CertificateFile = test.CertificatePath,
                    CSPName = test.CertificateCSPName,
                    KeyContainer = test.CertificateKeyContainer,
                    CertificateFingerprint = actualCertFingerprint,
                };
                signCommand.Arguments.Add(Path.Combine(dir, "package.nupkg"));

                // Act
                var signRequest = signCommand.GetAuthorSignRequest();

                // Assert
                Assert.Equal(SignatureType.Author, signRequest.SignatureType);
                Assert.NotNull(signRequest.Certificate);
                string expectedCertFingerprint = SignatureTestUtility.GetFingerprint(signRequest.Certificate, Common.HashAlgorithmName.SHA256);
                Assert.Equal(expectedCertFingerprint, actualCertFingerprint, StringComparer.Ordinal);
                Assert.NotNull(signRequest.PrivateKey);
            }
        }

        [CIOnlyFact]
        public async Task MSSignCommand_PrimarySignPackage_WithNoTimestampAsync()
        {
            var package = new SimpleTestPackageContext();

            // Arrange
            using (var test = new MSSignCommandTestContext(_trustedTestCertWithPrivateKey.TrustedCert))
            {
                string certSha256Hash = SignatureTestUtility.GetFingerprint(test.Cert, Common.HashAlgorithmName.SHA256);
                var unsignedPackageFile = await package.CreateAsFileAsync(test.Directory, Guid.NewGuid().ToString());
                var command = $"mssign {unsignedPackageFile} -CertificateFile {test.CertificatePath} -CSPName \"{test.CertificateCSPName}\" -KeyContainer \"{test.CertificateKeyContainer}\" -CertificateFingerprint {certSha256Hash}";

                var result = CommandRunner.Run(
                    _nugetExePath,
                    test.Directory,
                    command);

                result.Success.Should().BeTrue(because: result.AllOutput);
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [CIOnlyFact]
        public async Task MSSignCommand_PrimarySignPackage_WithTimestampAsync()
        {
            var timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();
            var package = new SimpleTestPackageContext();

            // Arrange
            using (var test = new MSSignCommandTestContext(_trustedTestCertWithPrivateKey.TrustedCert))
            {
                string certSha256Hash = SignatureTestUtility.GetFingerprint(test.Cert, Common.HashAlgorithmName.SHA256);
                var unsignedPackageFile = await package.CreateAsFileAsync(test.Directory, Guid.NewGuid().ToString());
                var command = $"mssign {unsignedPackageFile} -Timestamper {timestampService.Url} -CertificateFile {test.CertificatePath} -CSPName \"{test.CertificateCSPName}\" -KeyContainer \"{test.CertificateKeyContainer}\" -CertificateFingerprint {certSha256Hash}";

                var result = CommandRunner.Run(
                    _nugetExePath,
                    test.Directory,
                    command);

                result.Success.Should().BeTrue(because: result.AllOutput);
                result.AllOutput.Should().NotContain(_noTimestamperWarningCode);
            }
        }

        [CIOnlyFact]
        public async Task MSSignCommand_ResignPackageWithoutOverwriteFailsAsync()
        {
            var package = new SimpleTestPackageContext();

            // Arrange
            using (var test = new MSSignCommandTestContext(_trustedTestCertWithPrivateKey.TrustedCert))
            {
                string certSha256Hash = SignatureTestUtility.GetFingerprint(test.Cert, Common.HashAlgorithmName.SHA256);
                var unsignedPackageFile = await package.CreateAsFileAsync(test.Directory, Guid.NewGuid().ToString());
                var command = $"mssign {unsignedPackageFile} -CertificateFile {test.CertificatePath} -CSPName \"{test.CertificateCSPName}\" -KeyContainer \"{test.CertificateKeyContainer}\" -CertificateFingerprint {certSha256Hash}";

                var result = CommandRunner.Run(
                    _nugetExePath,
                    test.Directory,
                    command);

                result.Success.Should().BeTrue(because: result.AllOutput);
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);

                result = CommandRunner.Run(
                    _nugetExePath,
                    test.Directory,
                    command);

                result.Success.Should().BeFalse(because: result.AllOutput);
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
                result.Errors.Should().Contain("NU3001: The package already contains a signature. Please remove the existing signature before adding a new signature.");
            }
        }

        [CIOnlyFact]
        public async Task MSSignCommand_ResignPackageWithOverwriteSuccessAsync()
        {
            var package = new SimpleTestPackageContext();

            // Arrange
            using (var test = new MSSignCommandTestContext(_trustedTestCertWithPrivateKey.TrustedCert))
            {
                string certSha256Hash = SignatureTestUtility.GetFingerprint(test.Cert, Common.HashAlgorithmName.SHA256);
                var unsignedPackageFile = await package.CreateAsFileAsync(test.Directory, Guid.NewGuid().ToString());
                var command = $"mssign {unsignedPackageFile} -CertificateFile {test.CertificatePath} -CSPName \"{test.CertificateCSPName}\" -KeyContainer \"{test.CertificateKeyContainer}\" -CertificateFingerprint {certSha256Hash}";
                var commandWithOverwrite = $"mssign {unsignedPackageFile} -CertificateFile {test.CertificatePath} -CSPName \"{test.CertificateCSPName}\" -KeyContainer \"{test.CertificateKeyContainer}\" -CertificateFingerprint {certSha256Hash} -Overwrite";

                var result = CommandRunner.Run(
                    _nugetExePath,
                    test.Directory,
                    command);

                result.Success.Should().BeTrue(because: result.AllOutput);
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);

                result = CommandRunner.Run(
                    _nugetExePath,
                    test.Directory,
                    commandWithOverwrite);

                result.Success.Should().BeTrue(because: result.AllOutput);
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [CIOnlyFact]
        public async Task MSSignCommand_SignPackageWithSHA1CertificateFingerprint_RaisesExceptionAsync()
        {
            var result = await ExecuteMSSignCommandAsync(Common.HashAlgorithmName.SHA1);

            result.Success.Should().BeFalse(because: result.AllOutput);
            result.Errors.Should().Contain(_invalidCertificateFingerprintCode);
        }

        [CIOnlyTheory]
        [InlineData(Common.HashAlgorithmName.SHA256)]
        [InlineData(Common.HashAlgorithmName.SHA384)]
        [InlineData(Common.HashAlgorithmName.SHA512)]
        public async Task MSSignCommand_SignPackageWithSecureCertificateFingerprint_SucceedsAsync(Common.HashAlgorithmName hashAlgorithmName)
        {
            var result = await ExecuteMSSignCommandAsync(hashAlgorithmName);

            result.Success.Should().BeTrue(because: result.AllOutput);
            result.AllOutput.Should().NotContain(_invalidCertificateFingerprintCode);
        }

        private async Task<CommandRunnerResult> ExecuteMSSignCommandAsync(Common.HashAlgorithmName hashAlgorithmName)
        {
            var timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();
            var package = new SimpleTestPackageContext();

            // Arrange
            using var test = new MSSignCommandTestContext(_trustedTestCertWithPrivateKey.TrustedCert);
            var unsignedPackageFile = await package.CreateAsFileAsync(test.Directory, Guid.NewGuid().ToString());
            string certificateFingerprint = hashAlgorithmName == Common.HashAlgorithmName.SHA1
                ? test.Cert.Thumbprint
                : SignatureTestUtility.GetFingerprint(test.Cert, hashAlgorithmName);
            var command = $"mssign {unsignedPackageFile} -Timestamper {timestampService.Url} -CertificateFile {test.CertificatePath} -CSPName \"{test.CertificateCSPName}\" -KeyContainer \"{test.CertificateKeyContainer}\" -CertificateFingerprint {certificateFingerprint}";

            var result = CommandRunner.Run(
                _nugetExePath,
                test.Directory,
                command);

            return result;
        }

    }
}
