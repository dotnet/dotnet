// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Test.Utility;
using Test.Utility.Signing;
using Xunit;
using BcAccuracy = Org.BouncyCastle.Asn1.Tsp.Accuracy;
using BcX509Certificate = Org.BouncyCastle.X509.X509Certificate;
using HashAlgorithmName = NuGet.Common.HashAlgorithmName;

namespace NuGet.Packaging.FuncTest
{
    [Collection(SigningTestCollection.Name)]
    public class PackageSignatureVerifierTests
    {
        private readonly SigningTestFixture _testFixture;

        public PackageSignatureVerifierTests(SigningTestFixture fixture)
        {
            _testFixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [CIOnlyFact]
        public void Constructor_WhenArgumentIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => new PackageSignatureVerifier(verificationProviders: null));

            Assert.Equal("verificationProviders", exception.ParamName);
        }

        [Collection(SigningTestCollection.Name)]
        public class SignatureTrustAndValidityVerificationProviderTests
        {
            private static readonly Uri TestServiceIndexUrl = new Uri("https://v3serviceIndex.test/api/index.json");
            private readonly SignedPackageVerifierSettings _verifyCommandSettings = SignedPackageVerifierSettings.GetVerifyCommandDefaultPolicy(TestEnvironmentVariableReader.EmptyInstance);
            private readonly SigningTestFixture _testFixture;
            private readonly TrustedTestCert<TestCertificate> _trustedTestCert;
            private readonly TestCertificate _untrustedTestCertificate;
            private readonly IList<ISignatureVerificationProvider> _trustProviders;

            public SignatureTrustAndValidityVerificationProviderTests(SigningTestFixture fixture)
            {
                _testFixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
                _trustedTestCert = _testFixture.TrustedTestCertificate;
                _untrustedTestCertificate = _testFixture.UntrustedTestCertificate;
                _trustProviders = new List<ISignatureVerificationProvider>()
                {
                    new SignatureTrustAndValidityVerificationProvider()
                };
            }

            [CIOnlyFact]
            public async Task VerifySignaturesAsync_ValidCertificate_SuccessAsync()
            {
                // Arrange
                var nupkg = new SimpleTestPackageContext();

                using (TestDirectory dir = TestDirectory.Create())
                using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
                {
                    string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);
                    var verifier = new PackageSignatureVerifier(_trustProviders);

                    using (var packageReader = new PackageArchiveReader(signedPackagePath))
                    {
                        // Act
                        VerifySignaturesResult result = await verifier.VerifySignaturesAsync(packageReader, _verifyCommandSettings, CancellationToken.None);
                        IEnumerable<PackageVerificationResult> resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());

                        // Assert
                        result.IsValid.Should().BeTrue();
                        resultsWithErrors.Count().Should().Be(0);
                    }
                }
            }

            [CIOnlyFact]
            public async Task VerifySignaturesAsync_ValidCertificateAndTimestamp_SuccessAsync()
            {
                // Arrange
                var nupkg = new SimpleTestPackageContext();
                TimestampService timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();

                using (TestDirectory dir = TestDirectory.Create())
                using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
                {
                    string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                        testCertificate,
                        nupkg,
                        dir,
                        timestampService.Url);
                    var verifier = new PackageSignatureVerifier(_trustProviders);

                    using (var packageReader = new PackageArchiveReader(signedPackagePath))
                    {
                        // Act
                        VerifySignaturesResult result = await verifier.VerifySignaturesAsync(packageReader, _verifyCommandSettings, CancellationToken.None);
                        IEnumerable<PackageVerificationResult> resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());

                        // Assert
                        result.IsValid.Should().BeTrue();
                        resultsWithErrors.Count().Should().Be(0);
                    }
                }
            }

            [CIOnlyFact]
            public async Task VerifySignaturesAsync_ValidCertificateAndTimestampWithDifferentHashAlgorithms_SuccessAsync()
            {
                var packageContext = new SimpleTestPackageContext();
                TimestampService timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();

                using (TestDirectory directory = TestDirectory.Create())
                using (var certificate = new X509Certificate2(_trustedTestCert.Source.Cert))
                {
                    string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                        certificate,
                        packageContext,
                        directory,
                        timestampService.Url,
                        signatureHashAlgorithm: HashAlgorithmName.SHA512);
                    var verifier = new PackageSignatureVerifier(_trustProviders);

                    using (var packageReader = new PackageArchiveReader(signedPackagePath))
                    {
                        VerifySignaturesResult result = await verifier.VerifySignaturesAsync(packageReader, _verifyCommandSettings, CancellationToken.None);
                        IEnumerable<PackageVerificationResult> resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());

                        result.IsValid.Should().BeTrue();
                        resultsWithErrors.Count().Should().Be(0);
                    }
                }
            }

            // https://github.com/NuGet/Home/issues/11459
            [PlatformFact(Platform.Windows, Platform.Linux, CIOnly = true)]
            public async Task VerifySignaturesAsync_ExpiredCertificateAndTimestamp_SuccessAsync()
            {
                CertificateAuthority ca = await _testFixture.GetDefaultTrustedCertificateAuthorityAsync();
                TimestampService timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();
                AsymmetricCipherKeyPair keyPair = SigningTestUtility.GenerateKeyPair(publicKeyLength: 2048);
                DateTimeOffset now = DateTimeOffset.UtcNow;
                var issueOptions = new IssueCertificateOptions()
                {
                    KeyPair = keyPair,
                    NotAfter = now.AddSeconds(10),
                    NotBefore = now.AddSeconds(-2),
                    SubjectName = new X509Name("CN=NuGet Test Expired Certificate")
                };
                BcX509Certificate bcCertificate = ca.IssueCertificate(issueOptions);

                using (TestDirectory directory = TestDirectory.Create())
                using (X509Certificate2 certificate = CertificateUtilities.GetCertificateWithPrivateKey(bcCertificate, keyPair))
                {
                    var packageContext = new SimpleTestPackageContext();
                    string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                        certificate,
                        packageContext,
                        directory,
                        timestampService.Url);

                    await SignatureTestUtility.WaitForCertificateExpirationAsync(certificate);

                    var verifier = new PackageSignatureVerifier(_trustProviders);

                    using (var packageReader = new PackageArchiveReader(signedPackagePath))
                    {
                        VerifySignaturesResult result = await verifier.VerifySignaturesAsync(packageReader, _verifyCommandSettings, CancellationToken.None);

                        PackageVerificationResult trustProvider = result.Results.Single();

                        Assert.True(result.IsValid);
                        Assert.Equal(SignatureVerificationStatus.Valid, trustProvider.Trust);
                        Assert.Equal(0, trustProvider.Issues.Count(issue => issue.Level == LogLevel.Error));
                        Assert.Equal(0, trustProvider.Issues.Count(issue => issue.Level == LogLevel.Warning));
                    }
                }
            }

            // https://github.com/NuGet/Home/issues/11459
            [PlatformFact(Platform.Windows, Platform.Linux, CIOnly = true)]
            public async Task VerifySignaturesAsync_ExpiredCertificateAndTimestampWithTooLargeRange_FailsAsync()
            {
                ISigningTestServer testServer = await _testFixture.GetSigningTestServerAsync();
                CertificateAuthority ca = await _testFixture.GetDefaultTrustedCertificateAuthorityAsync();
                var accuracy = new BcAccuracy(seconds: new DerInteger(30), millis: null, micros: null);
                var serviceOptions = new TimestampServiceOptions() { Accuracy = accuracy };
                TimestampService timestampService = TimestampService.Create(ca, serviceOptions);
                AsymmetricCipherKeyPair keyPair = SigningTestUtility.GenerateKeyPair(publicKeyLength: 2048);
                DateTimeOffset now = DateTimeOffset.UtcNow;
                var issueOptions = new IssueCertificateOptions()
                {
                    KeyPair = keyPair,
                    NotAfter = now.AddSeconds(10),
                    NotBefore = now.AddSeconds(-2),
                    SubjectName = new X509Name("CN=NuGet Test Expired Certificate")
                };
                BcX509Certificate bcCertificate = ca.IssueCertificate(issueOptions);

                using (testServer.RegisterResponder(timestampService))
                using (TestDirectory directory = TestDirectory.Create())
                using (X509Certificate2 certificate = CertificateUtilities.GetCertificateWithPrivateKey(bcCertificate, keyPair))
                {
                    var packageContext = new SimpleTestPackageContext();
                    string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                        certificate,
                        packageContext,
                        directory,
                        timestampService.Url);

                    await SignatureTestUtility.WaitForCertificateExpirationAsync(certificate);

                    var verifier = new PackageSignatureVerifier(_trustProviders);

                    using (var packageReader = new PackageArchiveReader(signedPackagePath))
                    {
                        VerifySignaturesResult results = await verifier.VerifySignaturesAsync(packageReader, _verifyCommandSettings, CancellationToken.None);
                        PackageVerificationResult result = results.Results.Single();

                        Assert.False(results.IsValid);
                        Assert.Equal(SignatureVerificationStatus.Disallowed, result.Trust);
                        Assert.Equal(1, result.Issues.Count(issue => issue.Level == LogLevel.Error));
                        Assert.Equal(0, result.Issues.Count(issue => issue.Level == LogLevel.Warning));

                        Assert.Contains(result.Issues, issue =>
                            issue.Code == NuGetLogCode.NU3037 &&
                            issue.Level == LogLevel.Error &&
                            issue.Message.Contains("validity period has expired."));
                    }
                }
            }

            // Verify a package meeting minimum signature requirements.
            // This signature is neither an author nor repository signature.
            [CIOnlyFact]
            public async Task VerifySignaturesAsync_WithBasicSignedCms_SucceedsAsync()
            {
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: false,
                    allowUntrusted: false,
                    allowIgnoreTimestamp: false,
                    allowMultipleTimestamps: true,
                    allowNoTimestamp: true,
                    allowUnknownRevocation: false,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.All,
                    signaturePlacement: SignaturePlacement.Any,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.IfExistsAndIsNecessary,
                    revocationMode: RevocationMode.Online);

                using (TestDirectory directory = TestDirectory.Create())
                using (var certificate = new X509Certificate2(_trustedTestCert.Source.Cert))
                {
                    var packageContext = new SimpleTestPackageContext();
                    FileInfo unsignedPackageFile = await packageContext.CreateAsFileAsync(directory, "Package.nupkg");
                    FileInfo signedPackageFile = await SignedArchiveTestUtility.SignPackageFileWithBasicSignedCmsAsync(
                        directory,
                        unsignedPackageFile,
                        certificate);
                    var verifier = new PackageSignatureVerifier(_trustProviders);

                    using (var packageReader = new PackageArchiveReader(signedPackageFile.FullName))
                    {
                        VerifySignaturesResult result = await verifier.VerifySignaturesAsync(packageReader, settings, CancellationToken.None);

                        IEnumerable<PackageVerificationResult> resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                        IEnumerable<ILogMessage> totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());

                        Assert.Equal(1, result.Results.Count);

                        var signedPackageVerificationResult = (SignedPackageVerificationResult)result.Results[0];
                        SignerInfo signer = signedPackageVerificationResult.Signature.SignedCms.SignerInfos[0];

                        Assert.Equal(0, signer.SignedAttributes.Count);
                        Assert.Equal(0, signer.UnsignedAttributes.Count);

                        Assert.Equal(0, resultsWithErrors.Count());
                        Assert.Equal(0, totalErrorIssues.Count());
                    }
                }
            }

            [CIOnlyFact]
            public async Task VerifySignaturesAsync_SettingsRequireTimestamp_NoTimestamp_FailsAsync()
            {
                // Arrange
                var nupkg = new SimpleTestPackageContext();
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: false,
                    allowUntrusted: false,
                    allowIgnoreTimestamp: false,
                    allowMultipleTimestamps: true,
                    allowNoTimestamp: false,
                    allowUnknownRevocation: false,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.All,
                    signaturePlacement: SignaturePlacement.Any,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.IfExistsAndIsNecessary,
                    revocationMode: RevocationMode.Online);

                using (TestDirectory dir = TestDirectory.Create())
                using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
                {
                    string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);
                    var verifier = new PackageSignatureVerifier(_trustProviders);

                    using (var packageReader = new PackageArchiveReader(signedPackagePath))
                    {
                        // Act
                        VerifySignaturesResult result = await verifier.VerifySignaturesAsync(packageReader, settings, CancellationToken.None);
                        IEnumerable<PackageVerificationResult> resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                        IEnumerable<ILogMessage> totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());

                        // Assert
                        result.IsValid.Should().BeFalse();
                        resultsWithErrors.Count().Should().Be(1);
                        totalErrorIssues.Count().Should().Be(1);
                        totalErrorIssues.First().Code.Should().Be(NuGetLogCode.NU3027);
                    }
                }
            }

            [CIOnlyFact]
            public async Task VerifySignaturesAsync_SettingsNotRequireCheckCountersignature_WithValidPrimarySignatureAndInvalidCountersignature_SucceedsAsync()
            {
                // Arrange
                var nupkg = new SimpleTestPackageContext();
                TimestampService timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: false,
                    allowUntrusted: false,
                    allowIgnoreTimestamp: true,
                    allowMultipleTimestamps: true,
                    allowNoTimestamp: true,
                    allowUnknownRevocation: true,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.All,
                    signaturePlacement: SignaturePlacement.Any,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.IfExistsAndIsNecessary,
                    revocationMode: RevocationMode.Online);

                using (TestDirectory dir = TestDirectory.Create())
                using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
                using (var repoTestCertificate = new X509Certificate2(_untrustedTestCertificate.Cert))
                {
                    string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                        testCertificate,
                        nupkg,
                        dir,
                        timestampService.Url);
                    string countersignedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                        repoTestCertificate,
                        signedPackagePath,
                        dir,
                        TestServiceIndexUrl,
                        timestampService.Url);
                    var verifier = new PackageSignatureVerifier(_trustProviders);

                    using (var packageReader = new PackageArchiveReader(countersignedPackagePath))
                    {
                        // Act
                        VerifySignaturesResult result = await verifier.VerifySignaturesAsync(packageReader, settings, CancellationToken.None);
                        IEnumerable<PackageVerificationResult> resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());

                        // Assert
                        result.IsValid.Should().BeTrue();
                        resultsWithErrors.Count().Should().Be(0);
                    }
                }
            }

            [CIOnlyFact]
            public async Task VerifySignaturesAsync_SettingsRequireCheckCountersignature_WithValidPrimarySignatureAndInvalidCountersignature_FailsAsync()
            {
                // Arrange
                var nupkg = new SimpleTestPackageContext();
                TimestampService timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: false,
                    allowUntrusted: false,
                    allowIgnoreTimestamp: true,
                    allowMultipleTimestamps: true,
                    allowNoTimestamp: true,
                    allowUnknownRevocation: true,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.All,
                    signaturePlacement: SignaturePlacement.Any,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.IfExists,
                    revocationMode: RevocationMode.Online);

                using (TestDirectory dir = TestDirectory.Create())
                using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
                using (var repoTestCertificate = new X509Certificate2(_untrustedTestCertificate.Cert))
                {
                    string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                        testCertificate,
                        nupkg,
                        dir,
                        timestampService.Url);
                    string countersignedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                        repoTestCertificate,
                        signedPackagePath,
                        dir,
                        TestServiceIndexUrl,
                        timestampService.Url);
                    var verifier = new PackageSignatureVerifier(_trustProviders);

                    using (var packageReader = new PackageArchiveReader(countersignedPackagePath))
                    {
                        // Act
                        VerifySignaturesResult result = await verifier.VerifySignaturesAsync(packageReader, settings, CancellationToken.None);
                        IEnumerable<PackageVerificationResult> resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());

                        // Assert
                        result.IsValid.Should().BeFalse();
                        resultsWithErrors.Count().Should().Be(1);
                    }
                }
            }

            [CIOnlyFact]
            public async Task VerifySignaturesAsync_SettingsRequireCheckCountersignature_WithValidPrimarySignatureAndValidCountersignature_SucceedsAsync()
            {
                // Arrange
                var nupkg = new SimpleTestPackageContext();
                TimestampService timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: false,
                    allowUntrusted: false,
                    allowIgnoreTimestamp: true,
                    allowMultipleTimestamps: true,
                    allowNoTimestamp: true,
                    allowUnknownRevocation: true,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.All,
                    signaturePlacement: SignaturePlacement.Any,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.IfExistsAndIsNecessary,
                    revocationMode: RevocationMode.Online);

                using (TestDirectory dir = TestDirectory.Create())
                using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
                using (var repoTestCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
                {
                    string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                        testCertificate,
                        nupkg,
                        dir,
                        timestampService.Url);
                    string countersignedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                        repoTestCertificate,
                        signedPackagePath,
                        dir,
                        TestServiceIndexUrl,
                        timestampService.Url);
                    var verifier = new PackageSignatureVerifier(_trustProviders);

                    using (var packageReader = new PackageArchiveReader(countersignedPackagePath))
                    {
                        // Act
                        VerifySignaturesResult result = await verifier.VerifySignaturesAsync(packageReader, settings, CancellationToken.None);
                        IEnumerable<PackageVerificationResult> resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());

                        // Assert
                        result.IsValid.Should().BeTrue();
                        resultsWithErrors.Count().Should().Be(0);
                    }
                }
            }

            [CIOnlyFact]
            public async Task VerifySignaturesAsync_WithExpiredPrimarySignature_ValidCountersignature_AndPrimarySignatureValidAtCountersignTime_SucceedsAsync()
            {
                // Arrange
                var nupkg = new SimpleTestPackageContext();
                TimestampService timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: false,
                    allowUntrusted: false,
                    allowIgnoreTimestamp: true,
                    allowMultipleTimestamps: true,
                    allowNoTimestamp: true,
                    allowUnknownRevocation: true,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.All,
                    signaturePlacement: SignaturePlacement.Any,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.IfExistsAndIsNecessary,
                    revocationMode: RevocationMode.Online);

                using (TestDirectory dir = TestDirectory.Create())
                using (TrustedTestCert<TestCertificate> trustedCertificate = _testFixture.CreateTrustedTestCertificateThatWillExpireSoon())
                using (var willExpireCert = new X509Certificate2(trustedCertificate.Source.Cert))
                using (var repoTestCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
                {
                    string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                        willExpireCert,
                        nupkg,
                        dir);
                    string countersignedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                        repoTestCertificate,
                        signedPackagePath,
                        dir,
                        TestServiceIndexUrl,
                        timestampService.Url);

                    await SignatureTestUtility.WaitForCertificateExpirationAsync(willExpireCert);

                    var verifier = new PackageSignatureVerifier(_trustProviders);

                    using (var packageReader = new PackageArchiveReader(countersignedPackagePath))
                    {
                        // Act
                        VerifySignaturesResult result = await verifier.VerifySignaturesAsync(packageReader, settings, CancellationToken.None);
                        IEnumerable<PackageVerificationResult> resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());

                        // Assert
                        result.IsValid.Should().BeTrue();
                        resultsWithErrors.Count().Should().Be(0);
                    }
                }
            }

            [CIOnlyFact]
            public async Task VerifySignaturesAsync_WithExpiredAndUntrustedPrimarySignature_ValidCountersignature_AndPrimarySignatureValidAtCountersignTime_SucceedsAsync()
            {
                var nupkg = new SimpleTestPackageContext();
                TimestampService timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: false,
                    allowUntrusted: false,
                    allowIgnoreTimestamp: true,
                    allowMultipleTimestamps: true,
                    allowNoTimestamp: true,
                    allowUnknownRevocation: true,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.All,
                    signaturePlacement: SignaturePlacement.Any,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.IfExistsAndIsNecessary,
                    revocationMode: RevocationMode.Online);
                var verifier = new PackageSignatureVerifier(_trustProviders);

                using (TestDirectory testDirectory = TestDirectory.Create())
                using (X509Certificate2 untrustedCertificate = _testFixture.CreateUntrustedTestCertificateThatWillExpireSoon().Cert)
                using (var repositoryCertificate = new X509Certificate2(_testFixture.TrustedRepositoryCertificate.Source.Cert))
                {
                    string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                        untrustedCertificate,
                        nupkg,
                        testDirectory);
                    string countersignedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                        repositoryCertificate,
                        signedPackagePath,
                        testDirectory,
                        TestServiceIndexUrl,
                        timestampService.Url);

                    await SignatureTestUtility.WaitForCertificateExpirationAsync(untrustedCertificate);

                    using (var packageReader = new PackageArchiveReader(countersignedPackagePath))
                    {
                        VerifySignaturesResult result = await verifier.VerifySignaturesAsync(packageReader, settings, CancellationToken.None);
                        IEnumerable<PackageVerificationResult> resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());

                        result.IsValid.Should().BeTrue();
                        resultsWithErrors.Count().Should().Be(0);
                    }
                }
            }

            [CIOnlyFact]
            public async Task VerifySignaturesAsync_WithExpiredPrimarySignature_ValidCountersignature_AndPrimarySignatureExpiredAtCountersignTime_FailsAsync()
            {
                // Arrange
                var nupkg = new SimpleTestPackageContext();
                TimestampService timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: false,
                    allowUntrusted: false,
                    allowIgnoreTimestamp: true,
                    allowMultipleTimestamps: true,
                    allowNoTimestamp: true,
                    allowUnknownRevocation: true,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.All,
                    signaturePlacement: SignaturePlacement.Any,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.IfExists,
                    revocationMode: RevocationMode.Online);

                using (TestDirectory dir = TestDirectory.Create())
                using (TrustedTestCert<TestCertificate> trustedCertificate = _testFixture.CreateTrustedTestCertificateThatWillExpireSoon())
                using (var willExpireCert = new X509Certificate2(trustedCertificate.Source.Cert))
                using (var repoTestCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
                {
                    string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                        willExpireCert,
                        nupkg,
                        dir);

                    await SignatureTestUtility.WaitForCertificateExpirationAsync(willExpireCert);

                    string countersignedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                        repoTestCertificate,
                        signedPackagePath,
                        dir,
                        TestServiceIndexUrl,
                        timestampService.Url);

                    var verifier = new PackageSignatureVerifier(_trustProviders);

                    using (var packageReader = new PackageArchiveReader(countersignedPackagePath))
                    {
                        // Act
                        VerifySignaturesResult result = await verifier.VerifySignaturesAsync(packageReader, settings, CancellationToken.None);
                        IEnumerable<PackageVerificationResult> resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());

                        // Assert
                        result.IsValid.Should().BeFalse();
                        resultsWithErrors.Count().Should().Be(1);
                    }
                }
            }


            [CIOnlyFact]
            public async Task VerifySignaturesAsync_WithSignedAndCountersignedPackage_SucceedsAsync()
            {
                // Arrange
                var nupkg = new SimpleTestPackageContext();

                using (TestDirectory dir = TestDirectory.Create())
                using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
                using (TrustedTestCert<TestCertificate> trusted = SigningTestUtility.GenerateTrustedTestCertificate())
                using (var counterCertificate = new X509Certificate2(trusted.Source.Cert))
                {
                    string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                        testCertificate,
                        nupkg,
                        dir);
                    string repositorySignedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                        counterCertificate,
                        signedPackagePath,
                        dir,
                        TestServiceIndexUrl);

                    var verifier = new PackageSignatureVerifier(_trustProviders);

                    using (var packageReader = new PackageArchiveReader(repositorySignedPackagePath))
                    {
                        // Act
                        VerifySignaturesResult result = await verifier.VerifySignaturesAsync(
                            packageReader,
                            _verifyCommandSettings,
                            CancellationToken.None);
                        IEnumerable<PackageVerificationResult> resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());

                        // Assert
                        result.IsValid.Should().BeTrue();
                        resultsWithErrors.Count().Should().Be(0);
                    }
                }
            }

            [CIOnlyFact]
            public async Task VerifySignaturesAsync_WithSignedTimestampedCountersignedAndCountersignatureTimestampedPackage_SucceedsAsync()
            {
                // Arrange
                var nupkg = new SimpleTestPackageContext();
                TimestampService timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();

                using (TestDirectory dir = TestDirectory.Create())
                using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
                using (TrustedTestCert<TestCertificate> trusted = SigningTestUtility.GenerateTrustedTestCertificate())
                using (var counterCertificate = new X509Certificate2(trusted.Source.Cert))
                {
                    string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                        testCertificate,
                        nupkg,
                        dir,
                        timestampService.Url);
                    string repositorySignedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                        counterCertificate,
                        signedPackagePath,
                        dir,
                        TestServiceIndexUrl,
                        timestampService.Url);

                    var verifier = new PackageSignatureVerifier(_trustProviders);

                    using (var packageReader = new PackageArchiveReader(repositorySignedPackagePath))
                    {
                        // Act
                        VerifySignaturesResult result = await verifier.VerifySignaturesAsync(
                            packageReader,
                            _verifyCommandSettings,
                            CancellationToken.None);
                        IEnumerable<PackageVerificationResult> resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());

                        // Assert
                        result.IsValid.Should().BeTrue();
                        resultsWithErrors.Count().Should().Be(0);
                    }
                }
            }

            [CIOnlyFact]
            public async Task VerifySignaturesAsync_WithExpiredTimestamp_NotAllowIgnoreTimestamp_ShouldNotBeAnErrorAsync()
            {
                using (var nupkgStream = new MemoryStream(GetResource("UntrustedTimestampPackage.nupkg")))
                using (var package = new PackageArchiveReader(nupkgStream, leaveStreamOpen: false))
                {
                    var verifier = new PackageSignatureVerifier(_trustProviders);

                    // Act
                    VerifySignaturesResult result = await verifier.VerifySignaturesAsync(
                        package,
                        _verifyCommandSettings,
                        CancellationToken.None);
                    IEnumerable<PackageVerificationResult> resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    IEnumerable<ILogMessage> totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());

                    // Assert
                    totalErrorIssues.Select(i => i.Message).Should().NotContain("A required certificate is not within its validity period when verifying against the current system clock or the timestamp in the signed file.");
                }
            }

            [CIOnlyFact]
            public async Task VerifySignaturesAsync_WithTimestampChainingToUntrustedRoot_NotAllowIgnoreTimestamp_FailAsync()
            {
                using (var nupkgStream = new MemoryStream(GetResource("UntrustedTimestampPackage.nupkg")))
                using (var package = new PackageArchiveReader(nupkgStream, leaveStreamOpen: false))
                {
                    var verifier = new PackageSignatureVerifier(_trustProviders);

                    // Act
                    VerifySignaturesResult result = await verifier.VerifySignaturesAsync(
                        package,
                        _verifyCommandSettings,
                        CancellationToken.None);
                    IEnumerable<PackageVerificationResult> resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    IEnumerable<ILogMessage> allIssues = result.Results.SelectMany(r => r.Issues);
                    IEnumerable<ILogMessage> totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());

                    // Assert
                    result.IsValid.Should().BeFalse();
                    resultsWithErrors.Count().Should().Be(1);
                    totalErrorIssues.Count().Should().Be(3);
                    SigningTestUtility.AssertUntrustedRoot(allIssues, NuGetLogCode.NU3028, LogLevel.Error);
                }
            }
        }

        private static byte[] GetResource(string name)
        {
            return ResourceTestUtility.GetResourceBytes(
                $"NuGet.Packaging.FuncTest.compiler.resources.{name}",
                typeof(PackageSignatureVerifierTests));
        }
    }
}

#endif
