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
    public class ReposignCommandTests
    {
        private readonly string _noTimestamperWarningCode = NuGetLogCode.NU3002.ToString();
        private readonly string _invalidCertificateFingerprintCode = NuGetLogCode.NU3043.ToString();
        private const string Sha256Hash = "a591a6d40bf420404a011733cfb7b190d62c65bf0bcda32b55b046cbb7f506fb";

        private readonly TrustedTestCert<TestCertificate> _trustedTestCertWithPrivateKey;
        private readonly TrustedTestCert<TestCertificate> _trustedTestCertWithoutPrivateKey;

        private MSSignCommandTestFixture _testFixture;
        private readonly string _nugetExePath;

        public ReposignCommandTests(MSSignCommandTestFixture fixture)
        {
            _testFixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _trustedTestCertWithPrivateKey = _testFixture.TrustedTestCertificateWithPrivateKey;
            _trustedTestCertWithoutPrivateKey = _testFixture.TrustedTestCertificateWithoutPrivateKey;
            _nugetExePath = _testFixture.NuGetExePath;
        }

        [CIOnlyFact]
        public void GetRepositorySignRequest_InvalidCertificateFile()
        {
            var mockConsole = new Mock<IConsole>();
            var v3serviceIndex = "https://v3serviceindex.test/api/index.json";
            var timestampUri = "http://timestamp.test/url";

            // Arrange
            using (var dir = TestDirectory.Create())
            using (var test = new MSSignCommandTestContext(_trustedTestCertWithPrivateKey.TrustedCert))
            {
                var reposignCommand = new RepoSignCommand
                {
                    Console = mockConsole.Object,
                    Timestamper = timestampUri,
                    CertificateFile = Path.Combine(dir, "non-existant-cert.pfx"),
                    CSPName = test.CertificateCSPName,
                    KeyContainer = test.CertificateKeyContainer,
                    CertificateFingerprint = SignatureTestUtility.GetFingerprint(test.Cert, Common.HashAlgorithmName.SHA256),
                    V3ServiceIndexUrl = v3serviceIndex,
                };
                reposignCommand.Arguments.Add(Path.Combine(dir, "package.nupkg"));

                // Act & Assert
                var ex = Assert.Throws<CryptographicException>(() => reposignCommand.GetRepositorySignRequest());
                Assert.Contains("The system cannot find the file specified.", ex.Message);
            }
        }

        [CIOnlyFact]
        public void GetRepositorySignRequest_InvalidCSPName()
        {
            var mockConsole = new Mock<IConsole>();
            var v3serviceIndex = "https://v3serviceindex.test/api/index.json";
            var timestampUri = "http://timestamp.test/url";

            // Arrange
            using (var dir = TestDirectory.Create())
            using (var test = new MSSignCommandTestContext(_trustedTestCertWithoutPrivateKey.TrustedCert, exportPfx: false))
            {
                var reposignCommand = new RepoSignCommand
                {
                    Console = mockConsole.Object,
                    Timestamper = timestampUri,
                    CertificateFile = test.CertificatePath,
                    CSPName = "random nonexistant csp name",
                    KeyContainer = test.CertificateKeyContainer,
                    CertificateFingerprint = SignatureTestUtility.GetFingerprint(test.Cert, Common.HashAlgorithmName.SHA256),
                    V3ServiceIndexUrl = v3serviceIndex,
                };
                reposignCommand.Arguments.Add(Path.Combine(dir, "package.nupkg"));

                // Act & Assert
                var ex = Assert.Throws<InvalidOperationException>(() => reposignCommand.GetRepositorySignRequest());
                Assert.Equal("Can't find cng key.", ex.Message);
            }
        }

        [CIOnlyFact]
        public void GetRepositorySignRequest_InvalidKeyContainer()
        {
            var mockConsole = new Mock<IConsole>();
            var v3serviceIndex = "https://v3serviceindex.test/api/index.json";
            var timestampUri = "http://timestamp.test/url";

            // Arrange
            using (var dir = TestDirectory.Create())
            using (var test = new MSSignCommandTestContext(_trustedTestCertWithoutPrivateKey.TrustedCert, exportPfx: false))
            {
                var reposignCommand = new RepoSignCommand
                {
                    Console = mockConsole.Object,
                    Timestamper = timestampUri,
                    CertificateFile = test.CertificatePath,
                    CSPName = test.CertificateCSPName,
                    KeyContainer = "invalid-key-container",
                    CertificateFingerprint = SignatureTestUtility.GetFingerprint(test.Cert, Common.HashAlgorithmName.SHA256),
                    V3ServiceIndexUrl = v3serviceIndex,
                };
                reposignCommand.Arguments.Add(Path.Combine(dir, "package.nupkg"));

                // Act & Assert
                var ex = Assert.Throws<InvalidOperationException>(() => reposignCommand.GetRepositorySignRequest());
                Assert.Equal("Can't find cng key.", ex.Message);
            }
        }

        [CIOnlyTheory]
        [InlineData(Sha256Hash)]
        public void GetRepositorySignRequest_InvalidFingerprint(string certificateFingerPrint)
        {
            var mockConsole = new Mock<IConsole>();
            var v3serviceIndex = "https://v3serviceindex.test/api/index.json";
            var timestampUri = "http://timestamp.test/url";

            // Arrange
            using (var dir = TestDirectory.Create())
            using (var test = new MSSignCommandTestContext(_trustedTestCertWithPrivateKey.TrustedCert))
            {
                var reposignCommand = new RepoSignCommand
                {
                    Console = mockConsole.Object,
                    Timestamper = timestampUri,
                    CertificateFile = test.CertificatePath,
                    CSPName = test.CertificateCSPName,
                    KeyContainer = test.CertificateKeyContainer,
                    CertificateFingerprint = certificateFingerPrint,
                    V3ServiceIndexUrl = v3serviceIndex,
                };
                reposignCommand.Arguments.Add(Path.Combine(dir, "package.nupkg"));

                // Act & Assert
                var ex = Assert.Throws<InvalidOperationException>(() => reposignCommand.GetRepositorySignRequest());
                Assert.Equal("Can't find specified certificate.", ex.Message);
            }
        }

        [CIOnlyFact]
        public void GetRepositorySignRequest_Success()
        {
            var mockConsole = new Mock<IConsole>();
            var v3serviceIndex = "https://v3serviceindex.test/api/index.json";
            var timestampUri = "http://timestamp.test/url";

            // Arrange
            using (var dir = TestDirectory.Create())
            using (var test = new MSSignCommandTestContext(_trustedTestCertWithPrivateKey.TrustedCert))
            {
                var reposignCommand = new RepoSignCommand
                {
                    Console = mockConsole.Object,
                    Timestamper = timestampUri,
                    CertificateFile = test.CertificatePath,
                    CSPName = test.CertificateCSPName,
                    KeyContainer = test.CertificateKeyContainer,
                    CertificateFingerprint = SignatureTestUtility.GetFingerprint(test.Cert, Common.HashAlgorithmName.SHA256),
                    V3ServiceIndexUrl = v3serviceIndex,
                };
                reposignCommand.Arguments.Add(Path.Combine(dir, "package.nupkg"));

                // Act
                var signRequest = reposignCommand.GetRepositorySignRequest();

                // Assert
                Assert.Equal(v3serviceIndex, signRequest.V3ServiceIndexUrl.AbsoluteUri, StringComparer.Ordinal);
                Assert.Equal(SignatureType.Repository, signRequest.SignatureType);
                Assert.NotNull(signRequest.Certificate);
                Assert.Equal(signRequest.Certificate.Thumbprint, test.Cert.Thumbprint, StringComparer.Ordinal);
                Assert.NotNull(signRequest.PrivateKey);
            }
        }

        [CIOnlyFact]
        public async Task ReposignCommand_PrimarySignPackage_WithNoTimestampAsync()
        {
            var package = new SimpleTestPackageContext();

            // Arrange
            using (var test = new MSSignCommandTestContext(_trustedTestCertWithPrivateKey.TrustedCert))
            {
                var unsignedPackageFile = await package.CreateAsFileAsync(test.Directory, Guid.NewGuid().ToString());
                string certSha256Hash = SignatureTestUtility.GetFingerprint(test.Cert, Common.HashAlgorithmName.SHA256);
                var command = $"reposign {unsignedPackageFile} -CertificateFile {test.CertificatePath} -CSPName \"{test.CertificateCSPName}\" -KeyContainer \"{test.CertificateKeyContainer}\" -CertificateFingerprint {certSha256Hash} -V3ServiceIndexUrl https://v3serviceIndex.test/api/index.json";

                var result = CommandRunner.Run(
                    _nugetExePath,
                    test.Directory,
                    command);

                result.Success.Should().BeTrue(because: result.AllOutput);
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [CIOnlyFact]
        public async Task ReposignCommand_PrimarySignPackage_WithTimestampAsync()
        {
            var timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();
            var package = new SimpleTestPackageContext();

            // Arrange
            using (var test = new MSSignCommandTestContext(_trustedTestCertWithPrivateKey.TrustedCert))
            {
                var unsignedPackageFile = await package.CreateAsFileAsync(test.Directory, Guid.NewGuid().ToString());
                string certSha256Hash = SignatureTestUtility.GetFingerprint(test.Cert, Common.HashAlgorithmName.SHA256);
                var command = $"reposign {unsignedPackageFile} -Timestamper {timestampService.Url} -CertificateFile {test.CertificatePath} -CSPName \"{test.CertificateCSPName}\" -KeyContainer \"{test.CertificateKeyContainer}\" -CertificateFingerprint {certSha256Hash} -V3ServiceIndexUrl https://v3serviceIndex.test/api/index.json";

                var result = CommandRunner.Run(
                    _nugetExePath,
                    test.Directory,
                    command);

                result.Success.Should().BeTrue(because: result.AllOutput);
                result.AllOutput.Should().NotContain(_noTimestamperWarningCode);
            }
        }

        [CIOnlyFact]
        public async Task ReposignCommand_Countersign_RepositorySignedPackage_FailAsync()
        {
            var package = new SimpleTestPackageContext();

            // Arrange
            using (var test = new MSSignCommandTestContext(_trustedTestCertWithPrivateKey.TrustedCert))
            {
                var unsignedPackageFile = await package.CreateAsFileAsync(test.Directory, Guid.NewGuid().ToString());
                string certSha256Hash = SignatureTestUtility.GetFingerprint(test.Cert, Common.HashAlgorithmName.SHA256);
                var command = $"reposign {unsignedPackageFile} -CertificateFile {test.CertificatePath} -CSPName \"{test.CertificateCSPName}\" -KeyContainer \"{test.CertificateKeyContainer}\" -CertificateFingerprint {certSha256Hash} -V3ServiceIndexUrl https://v3serviceIndex.test/api/index.json";

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
                result.Errors.Should().Contain("NU3033: A repository primary signature must not have a repository countersignature.");
            }
        }

        [CIOnlyFact]
        public async Task ReposignCommand_Countersign_AuthorSignedPackage_WithNoTimestampAsync()
        {
            var package = new SimpleTestPackageContext();

            // Arrange
            using (var test = new MSSignCommandTestContext(_trustedTestCertWithPrivateKey.TrustedCert))
            {
                var unsignedPackageFile = await package.CreateAsFileAsync(test.Directory, Guid.NewGuid().ToString());
                string certSha256Hash = SignatureTestUtility.GetFingerprint(test.Cert, Common.HashAlgorithmName.SHA256);
                var authorSignCommand = $"mssign {unsignedPackageFile} -CertificateFile {test.CertificatePath} -CSPName \"{test.CertificateCSPName}\" -KeyContainer \"{test.CertificateKeyContainer}\" -CertificateFingerprint {certSha256Hash}";
                var repoSignCommand = $"reposign {unsignedPackageFile} -CertificateFile {test.CertificatePath} -CSPName \"{test.CertificateCSPName}\" -KeyContainer \"{test.CertificateKeyContainer}\" -CertificateFingerprint {certSha256Hash} -V3ServiceIndexUrl https://v3serviceIndex.test/api/index.json";

                var result = CommandRunner.Run(
                    _nugetExePath,
                    test.Directory,
                    authorSignCommand);

                result.Success.Should().BeTrue(because: result.AllOutput);
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);

                result = CommandRunner.Run(
                    _nugetExePath,
                    test.Directory,
                    repoSignCommand);

                result.Success.Should().BeTrue(because: result.AllOutput);
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [CIOnlyFact]
        public async Task ReposignCommand_Countersign_AuthorSignedPackage_WithTimestampAsync()
        {
            var package = new SimpleTestPackageContext();
            var timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();

            // Arrange
            using (var test = new MSSignCommandTestContext(_trustedTestCertWithPrivateKey.TrustedCert))
            {
                var unsignedPackageFile = await package.CreateAsFileAsync(test.Directory, Guid.NewGuid().ToString());
                string certSha256Hash = SignatureTestUtility.GetFingerprint(test.Cert, Common.HashAlgorithmName.SHA256);
                var authorSignCommand = $"mssign {unsignedPackageFile} -Timestamper {timestampService.Url} -CertificateFile {test.CertificatePath} -CSPName \"{test.CertificateCSPName}\" -KeyContainer \"{test.CertificateKeyContainer}\" -CertificateFingerprint {certSha256Hash}";
                var repoSignCommand = $"reposign {unsignedPackageFile} -Timestamper {timestampService.Url} -CertificateFile {test.CertificatePath} -CSPName \"{test.CertificateCSPName}\" -KeyContainer \"{test.CertificateKeyContainer}\" -CertificateFingerprint {certSha256Hash} -V3ServiceIndexUrl https://v3serviceIndex.test/api/index.json";

                var result = CommandRunner.Run(
                    _nugetExePath,
                    test.Directory,
                    authorSignCommand);

                result.Success.Should().BeTrue(because: result.AllOutput);
                result.AllOutput.Should().NotContain(_noTimestamperWarningCode);

                result = CommandRunner.Run(
                    _nugetExePath,
                    test.Directory,
                    repoSignCommand);

                result.Success.Should().BeTrue(because: result.AllOutput);
                result.AllOutput.Should().NotContain(_noTimestamperWarningCode);
            }
        }

        [CIOnlyFact]
        public async Task ReposignCommand_Countersign_RepositoryCountersignedPackage_FailAsync()
        {
            var package = new SimpleTestPackageContext();

            // Arrange
            using (var test = new MSSignCommandTestContext(_trustedTestCertWithPrivateKey.TrustedCert))
            {
                var unsignedPackageFile = await package.CreateAsFileAsync(test.Directory, Guid.NewGuid().ToString());
                string certSha256Hash = SignatureTestUtility.GetFingerprint(test.Cert, Common.HashAlgorithmName.SHA256);
                var authorSignCommand = $"mssign {unsignedPackageFile} -CertificateFile {test.CertificatePath} -CSPName \"{test.CertificateCSPName}\" -KeyContainer \"{test.CertificateKeyContainer}\" -CertificateFingerprint {certSha256Hash}";
                var repoSignCommand = $"reposign {unsignedPackageFile} -CertificateFile {test.CertificatePath} -CSPName \"{test.CertificateCSPName}\" -KeyContainer \"{test.CertificateKeyContainer}\" -CertificateFingerprint {certSha256Hash} -V3ServiceIndexUrl https://v3serviceIndex.test/api/index.json";

                var result = CommandRunner.Run(
                    _nugetExePath,
                    test.Directory,
                    authorSignCommand);

                result.Success.Should().BeTrue(because: result.AllOutput);
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);

                result = CommandRunner.Run(
                    _nugetExePath,
                    test.Directory,
                    repoSignCommand);

                result.Success.Should().BeTrue(because: result.AllOutput);
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);

                result = CommandRunner.Run(
                    _nugetExePath,
                    test.Directory,
                    repoSignCommand);

                result.Success.Should().BeFalse(because: result.AllOutput);
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
                result.Errors.Should().Contain("NU3032: The package already contains a repository countersignature. Please remove the existing signature before adding a new repository countersignature.");
            }
        }

        [CIOnlyFact]
        public async Task RepoSignCommand_SignPackageWithSHA1CertificateFingerprint_RaisesExceptionAsync()
        {
            var result = await ExecuteRepoSignCommandAsync(Common.HashAlgorithmName.SHA1);

            result.Success.Should().BeFalse(because: result.AllOutput);
            result.Errors.Should().Contain(_invalidCertificateFingerprintCode);
        }

        [CIOnlyTheory]
        [InlineData(Common.HashAlgorithmName.SHA256)]
        [InlineData(Common.HashAlgorithmName.SHA384)]
        [InlineData(Common.HashAlgorithmName.SHA512)]
        public async Task RepoSignCommand_SignPackageWithSecureCertificateFingerprint_SucceedsAsync(Common.HashAlgorithmName hashAlgorithmName)
        {
            var result = await ExecuteRepoSignCommandAsync(hashAlgorithmName);

            result.Success.Should().BeTrue(because: result.AllOutput);
            result.AllOutput.Should().NotContain(_invalidCertificateFingerprintCode);
        }

        private async Task<CommandRunnerResult> ExecuteRepoSignCommandAsync(Common.HashAlgorithmName hashAlgorithmName)
        {
            var timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();
            var package = new SimpleTestPackageContext();

            // Arrange
            using var test = new MSSignCommandTestContext(_trustedTestCertWithPrivateKey.TrustedCert);
            var unsignedPackageFile = await package.CreateAsFileAsync(test.Directory, Guid.NewGuid().ToString());
            string certificateFingerprint = hashAlgorithmName == Common.HashAlgorithmName.SHA1
                ? test.Cert.Thumbprint
                : SignatureTestUtility.GetFingerprint(test.Cert, hashAlgorithmName);
            var command = $"reposign {unsignedPackageFile} -Timestamper {timestampService.Url} -CertificateFile {test.CertificatePath} -CSPName \"{test.CertificateCSPName}\" -KeyContainer \"{test.CertificateKeyContainer}\" -CertificateFingerprint {certificateFingerprint} -V3ServiceIndexUrl https://v3serviceIndex.test/api/index.json";

            var result = CommandRunner.Run(
                _nugetExePath,
                test.Directory,
                command);

            return result;
        }
    }
}
