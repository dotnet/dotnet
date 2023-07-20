// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Test.Utility;
using Test.Utility.Signing;
using Xunit;

namespace NuGet.Signing.CrossFramework.Test
{
    [Collection(CrossVerifyTestCollection.Name)]
    public class CrossFrameworkVerificationTest
    {
        private const int SHA1HashLength = 20;

        private readonly string _successfullyVerified = "Successfully verified package 'packageA.1.0.0'";
        private readonly string _noTimestamperWarning = "NU3027: The signature should be timestamped to enable long-term signature validity after the certificate has expired";
        private static readonly Uri ServiceIndexUrl = new Uri("https://v3serviceIndex.test/api/index.json");

        private readonly CrossVerifyTestFixture _testFixture;

        public CrossFrameworkVerificationTest(CrossVerifyTestFixture fixture)
        {
            _testFixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async Task Verify_AuthorSignedPackage_SuccessAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();
            X509Certificate2 defaultAuthorCertificate = await _testFixture.GetDefaultAuthorSigningCertificateAsync();

            using (TestDirectory dir = TestDirectory.Create())
            using (var authorCertificate = new X509Certificate2(defaultAuthorCertificate))
            {
                string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                    authorCertificate,
                    nupkg,
                    dir);

                // Act
                CommandRunnerResult result = RunVerifyCommand(signedPackagePath);

                // Assert
                result.Success.Should().BeTrue(because: result.AllOutput);
                result.AllOutput.Should().Contain(_successfullyVerified);
                Regex.Matches(result.AllOutput, _noTimestamperWarning).Count.Should().Be(1);
            }
        }

        [Fact]
        public async Task Verify_AuthorSignedTimestampedPackage_SuccessAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();
            X509Certificate2 defaultAuthorCertificate = await _testFixture.GetDefaultAuthorSigningCertificateAsync();
            TimestampService timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();

            using (TestDirectory dir = TestDirectory.Create())
            using (var authorCertificate = new X509Certificate2(defaultAuthorCertificate))
            {
                string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                    authorCertificate,
                    nupkg,
                    dir,
                    timestampService.Url);

                // Act
                CommandRunnerResult result = RunVerifyCommand(signedPackagePath);

                // Assert
                result.Success.Should().BeTrue(because: result.AllOutput);
                result.AllOutput.Should().Contain(_successfullyVerified);
                Regex.Matches(result.AllOutput, _noTimestamperWarning).Count.Should().Be(0);
            }
        }

        [Fact]
        public async Task Verify_RepositorySignedPackage_SuccessAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();
            X509Certificate2 defaultRepositoryCertificate = await _testFixture.GetDefaultRepositorySigningCertificateAsync();

            using (TestDirectory dir = TestDirectory.Create())
            using (var repositoryCertificate = new X509Certificate2(defaultRepositoryCertificate))
            {
                string signedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                    repositoryCertificate,
                    nupkg,
                    dir,
                    ServiceIndexUrl);

                // Act
                CommandRunnerResult result = RunVerifyCommand(signedPackagePath);

                // Assert
                result.Success.Should().BeTrue(because: result.AllOutput);
                result.AllOutput.Should().Contain(_successfullyVerified);
                Regex.Matches(result.AllOutput, _noTimestamperWarning).Count.Should().Be(1);
            }
        }

        [Fact]
        public async Task Verify_RepositorySignedTimestampedPackage_SuccessAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();
            X509Certificate2 defaultRepositoryCertificate = await _testFixture.GetDefaultRepositorySigningCertificateAsync();
            TimestampService timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();

            using (TestDirectory dir = TestDirectory.Create())
            using (var repositoryCertificate = new X509Certificate2(defaultRepositoryCertificate))
            {
                string signedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                    repositoryCertificate,
                    nupkg,
                    dir,
                    ServiceIndexUrl,
                    timestampService.Url);

                // Act
                CommandRunnerResult result = RunVerifyCommand(signedPackagePath);

                // Assert
                result.Success.Should().BeTrue(because: result.AllOutput);
                result.AllOutput.Should().Contain(_successfullyVerified);
                Regex.Matches(result.AllOutput, _noTimestamperWarning).Count.Should().Be(0);
            }
        }

        [Fact]
        public async Task Verify_AuthorSignedRepositoryCounterSignedPackage_SuccessAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();
            X509Certificate2 defaultAuthorCertificate = await _testFixture.GetDefaultAuthorSigningCertificateAsync();
            X509Certificate2 defaultRepositoryCertificate = await _testFixture.GetDefaultRepositorySigningCertificateAsync();

            using (TestDirectory dir = TestDirectory.Create())
            using (var authorCertificate = new X509Certificate2(defaultAuthorCertificate))
            using (var repositoryCertificate = new X509Certificate2(defaultRepositoryCertificate))
            {
                string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                   authorCertificate,
                   nupkg,
                   dir);

                string countersignedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                    repositoryCertificate,
                    signedPackagePath,
                    dir,
                    ServiceIndexUrl);

                // Act
                CommandRunnerResult result = RunVerifyCommand(countersignedPackagePath);

                // Assert
                result.Success.Should().BeTrue(because: result.AllOutput);
                result.AllOutput.Should().Contain(_successfullyVerified);
                Regex.Matches(result.AllOutput, _noTimestamperWarning).Count.Should().Be(2);
            }
        }

        [Fact]
        public async Task Verify_AuthorSignedTimestampedRepositoryCounterSignedPackage_SuccessAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();
            X509Certificate2 defaultAuthorCertificate = await _testFixture.GetDefaultAuthorSigningCertificateAsync();
            X509Certificate2 defaultRepositoryCertificate = await _testFixture.GetDefaultRepositorySigningCertificateAsync();
            TimestampService timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();

            using (TestDirectory dir = TestDirectory.Create())
            using (var authorCertificate = new X509Certificate2(defaultAuthorCertificate))
            using (var repositoryCertificate = new X509Certificate2(defaultRepositoryCertificate))
            {
                string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                   authorCertificate,
                   nupkg,
                   dir,
                   timestampService.Url);

                string countersignedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                    repositoryCertificate,
                    signedPackagePath,
                    dir,
                    ServiceIndexUrl);

                // Act
                CommandRunnerResult result = RunVerifyCommand(countersignedPackagePath);

                // Assert
                result.Success.Should().BeTrue(because: result.AllOutput);
                result.AllOutput.Should().Contain(_successfullyVerified);
                Regex.Matches(result.AllOutput, _noTimestamperWarning).Count.Should().Be(1);
            }
        }

        [Fact]
        public async Task Verify_AuthorSignedRepositoryCounterSignedTimestampedPackage_SuccessAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();
            X509Certificate2 defaultAuthorCertificate = await _testFixture.GetDefaultAuthorSigningCertificateAsync();
            X509Certificate2 defaultRepositoryCertificate = await _testFixture.GetDefaultRepositorySigningCertificateAsync();
            TimestampService timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();

            using (TestDirectory dir = TestDirectory.Create())
            using (var authorCertificate = new X509Certificate2(defaultAuthorCertificate))
            using (var repositoryCertificate = new X509Certificate2(defaultRepositoryCertificate))
            {
                string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                   authorCertificate,
                   nupkg,
                   dir);

                string countersignedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                    repositoryCertificate,
                    signedPackagePath,
                    dir,
                    ServiceIndexUrl,
                    timestampService.Url);

                // Act
                CommandRunnerResult result = RunVerifyCommand(countersignedPackagePath);

                // Assert
                result.Success.Should().BeTrue(because: result.AllOutput);
                result.AllOutput.Should().Contain(_successfullyVerified);
                Regex.Matches(result.AllOutput, _noTimestamperWarning).Count.Should().Be(1);
            }
        }

        [Fact]
        public async Task Verify_AuthorSignedTimestampedRepositoryCounterSignedTimestampedPackage_SuccessAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();
            X509Certificate2 defaultAuthorCertificate = await _testFixture.GetDefaultAuthorSigningCertificateAsync();
            X509Certificate2 defaultRepositoryCertificate = await _testFixture.GetDefaultRepositorySigningCertificateAsync();
            TimestampService timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();

            using (TestDirectory dir = TestDirectory.Create())
            using (var authorCertificate = new X509Certificate2(defaultAuthorCertificate))
            using (var repositoryCertificate = new X509Certificate2(defaultRepositoryCertificate))
            {
                string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                   authorCertificate,
                   nupkg,
                   dir,
                   timestampService.Url);

                string countersignedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                    repositoryCertificate,
                    signedPackagePath,
                    dir,
                    ServiceIndexUrl,
                    timestampService.Url);

                // Act
                CommandRunnerResult result = RunVerifyCommand(countersignedPackagePath);

                // Assert
                result.Success.Should().BeTrue(because: result.AllOutput);
                result.AllOutput.Should().Contain(_successfullyVerified);
                Regex.Matches(result.AllOutput, _noTimestamperWarning).Count.Should().Be(0);
            }
        }

#if IS_DESKTOP
        //The following tests are from NuGet.Core.FuncTests\NuGet.Packaging.FuncTest\SigningTests\SignatureUtilityTests.cs.
        //As timestamping in net5.0 is stricter, they could not be enabled in net5.0 code path. That's why we cross verify them.
        [Fact]
        public async Task GetTimestampCertificateChain_WithNoSigningCertificateUsage_Throws()
        {
            ISigningTestServer testServer = await _testFixture.GetSigningTestServerAsync();
            CertificateAuthority rootCa = await _testFixture.GetDefaultTrustedCertificateAuthorityAsync();
            var options = new TimestampServiceOptions()
            {
                SigningCertificateUsage = SigningCertificateUsage.None
            };
            TimestampService timestampService = TimestampService.Create(rootCa, options);

            using (testServer.RegisterResponder(timestampService))
            {
                var nupkg = new SimpleTestPackageContext();

                using (var certificate = new X509Certificate2(_testFixture.TrustedTestCertificate.Source.Cert))
                using (TestDirectory directory = TestDirectory.Create())
                {
                    string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                        certificate,
                        nupkg,
                        directory,
                        timestampService.Url);

                    // Act
                    CommandRunnerResult result = RunVerifyCommand(signedPackagePath);

                    // Assert
                    result.Success.Should().BeFalse(because: result.AllOutput);
                    result.AllOutput.Should().Contain("Either the signing-certificate or signing-certificate-v2 attribute must be present.");
                    result.AllOutput.Should().NotContain(_successfullyVerified);
                }
            }
        }

        [Theory]
        [InlineData(SigningCertificateUsage.V1)]
        public async Task GetTimestampCertificateChain_WithShortEssCertIdCertificateHash_Throws(
            SigningCertificateUsage signingCertificateUsage)
        {
            ISigningTestServer testServer = await _testFixture.GetSigningTestServerAsync();
            CertificateAuthority rootCa = await _testFixture.GetDefaultTrustedCertificateAuthorityAsync();
            var options = new TimestampServiceOptions()
            {
                SigningCertificateUsage = signingCertificateUsage,
                SigningCertificateV1Hash = new byte[SHA1HashLength - 1]
            };
            TimestampService timestampService = TimestampService.Create(rootCa, options);

            using (testServer.RegisterResponder(timestampService))
            {
                var nupkg = new SimpleTestPackageContext();

                using (var certificate = new X509Certificate2(_testFixture.TrustedTestCertificate.Source.Cert))
                using (TestDirectory directory = TestDirectory.Create())
                {
                    string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                        certificate,
                        nupkg,
                        directory,
                        timestampService.Url);

                    // Act
                    CommandRunnerResult result = RunVerifyCommand(signedPackagePath);

                    // Assert
                    result.Success.Should().BeFalse(because: result.AllOutput);
                    result.AllOutput.Should().Contain("A certificate referenced by the signing-certificate attribute could not be found.");
                    result.AllOutput.Should().NotContain(_successfullyVerified);
                }
            }
        }

        [Theory]
        [InlineData(SigningCertificateUsage.V1)]
        public async Task GetTimestampCertificateChain_WithMismatchedEssCertIdCertificateHash_ReturnsChain(
            SigningCertificateUsage signingCertificateUsage)
        {
            ISigningTestServer testServer = await _testFixture.GetSigningTestServerAsync();
            CertificateAuthority rootCa = await _testFixture.GetDefaultTrustedCertificateAuthorityAsync();
            var options = new TimestampServiceOptions()
            {
                SigningCertificateUsage = signingCertificateUsage,
                SigningCertificateV1Hash = new byte[SHA1HashLength]
            };
            TimestampService timestampService = TimestampService.Create(rootCa, options);

            using (testServer.RegisterResponder(timestampService))
            {
                var nupkg = new SimpleTestPackageContext();

                using (var certificate = new X509Certificate2(_testFixture.TrustedTestCertificate.Source.Cert))
                using (TestDirectory directory = TestDirectory.Create())
                {
                    string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                        certificate,
                        nupkg,
                        directory,
                        timestampService.Url);

                    // Act
                    CommandRunnerResult result = RunVerifyCommand(signedPackagePath);

                    // Assert
                    result.Success.Should().BeTrue(because: result.AllOutput);
                    result.AllOutput.Should().Contain(_successfullyVerified);
                    Regex.Matches(result.AllOutput, _noTimestamperWarning).Count.Should().Be(0);
                }
            }
        }
#endif
        private CommandRunnerResult RunVerifyCommand(string packagePath)
        {
#if IS_DESKTOP
            //command and arguments for dotnet.exe nuget verify command
            string command = _testFixture._dotnetExePath;
            string arguments = $"nuget verify {packagePath} -v n";
#else
            //command and arguments for nuget.exe verify command
            string command = _testFixture._nugetExePath;
            string arguments = $"verify {packagePath} -Signatures";
#endif
            CommandRunnerResult verifyResult = CommandRunner.Run(
                command,
                workingDirectory: ".",
                arguments,
                waitForExit: true);

            return verifyResult;
        }
    }
}
