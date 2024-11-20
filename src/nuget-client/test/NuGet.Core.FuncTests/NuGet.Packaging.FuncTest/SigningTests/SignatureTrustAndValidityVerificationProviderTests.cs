// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Test.Utility;
using Test.Utility.Signing;
using Xunit;
using Xunit.Abstractions;
using HashAlgorithmName = NuGet.Common.HashAlgorithmName;

namespace NuGet.Packaging.FuncTest
{
    using X509StorePurpose = Microsoft.Internal.NuGet.Testing.SignedPackages.X509StorePurpose;

    [Collection(SigningTestCollection.Name)]
    public class SignatureTrustAndValidityVerificationProviderTests
    {
        private const string UntrustedChainCertError = "The author primary signature's signing certificate is not trusted by the trust provider.";
        private readonly SignedPackageVerifierSettings _verifyCommandSettings = SignedPackageVerifierSettings.GetVerifyCommandDefaultPolicy(TestEnvironmentVariableReader.EmptyInstance);
        private readonly SignedPackageVerifierSettings _defaultSettings = SignedPackageVerifierSettings.GetDefault(TestEnvironmentVariableReader.EmptyInstance);
        private readonly SigningTestFixture _testFixture;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly TrustedTestCert<TestCertificate> _trustedTestCert;

        public SignatureTrustAndValidityVerificationProviderTests(SigningTestFixture fixture, ITestOutputHelper testOutputHelper)
        {
            _testFixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _testOutputHelper = testOutputHelper;
            _trustedTestCert = _testFixture.TrustedTestCertificate;
        }

        [CIOnlyFact]
        public async Task GetTrustResultAsync_WithInvalidSignature_ThrowsAsync()
        {
            var package = new SimpleTestPackageContext();
            TimestampService timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();

            using (TestDirectory directory = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                string packageFilePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                    testCertificate,
                    package,
                    directory,
                    timestampService.Url);

                using (var packageReader = new PackageArchiveReader(packageFilePath))
                {
                    PrimarySignature signature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PrimarySignature invalidSignature = SignatureTestUtility.GenerateInvalidPrimarySignature(signature);
                    var provider = new SignatureTrustAndValidityVerificationProvider();

                    PackageVerificationResult result = await provider.GetTrustResultAsync(
                        packageReader,
                        invalidSignature,
                        _defaultSettings,
                        CancellationToken.None);

                    SignatureLog issue = result.Issues.FirstOrDefault(log => log.Code == NuGetLogCode.NU3012);

                    Assert.NotNull(issue);
                    Assert.Contains("validation failed.", issue.Message);
                }
            }
        }

        [CIOnlyFact]
        public async Task GetTrustResultAsync_SettingsRequireExactlyOneTimestamp_MultipleTimestamps_FailsAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();
            var testLogger = new TestLogger();
            TimestampService timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();
            var settings = new SignedPackageVerifierSettings(
                allowUnsigned: false,
                allowIllegal: false,
                allowUntrusted: false,
                allowIgnoreTimestamp: false,
                allowMultipleTimestamps: false,
                allowNoTimestamp: false,
                allowUnknownRevocation: false,
                reportUnknownRevocation: true,
                verificationTarget: VerificationTarget.All,
                signaturePlacement: SignaturePlacement.Any,
                repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.IfExistsAndIsNecessary,
                revocationMode: RevocationMode.Online);
            var timestampProvider = new Rfc3161TimestampProvider(timestampService.Url);
            var verificationProvider = new SignatureTrustAndValidityVerificationProvider();

            using (var package = new PackageArchiveReader(await nupkg.CreateAsStreamAsync(), leaveStreamOpen: false))
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            using (var signatureRequest = new AuthorSignPackageRequest(testCertificate, HashAlgorithmName.SHA256))
            {
                PrimarySignature signature = await SignedArchiveTestUtility.CreatePrimarySignatureForPackageAsync(package, signatureRequest);
                PrimarySignature timestampedSignature = await SignedArchiveTestUtility.TimestampSignature(
                    timestampProvider,
                    signature,
                    signatureRequest.TimestampHashAlgorithm,
                    SignaturePlacement.PrimarySignature,
                    testLogger);
                PrimarySignature reTimestampedSignature = await SignedArchiveTestUtility.TimestampSignature(
                    timestampProvider,
                    timestampedSignature,
                    signatureRequest.TimestampHashAlgorithm,
                    SignaturePlacement.PrimarySignature,
                    testLogger);

                timestampedSignature.Timestamps.Count.Should().Be(1);
                reTimestampedSignature.Timestamps.Count.Should().Be(2);

                // Act
                PackageVerificationResult result = await verificationProvider.GetTrustResultAsync(package, reTimestampedSignature, settings, CancellationToken.None);
                IEnumerable<ILogMessage> totalErrorIssues = result.GetErrorIssues();

                // Assert
                result.Trust.Should().Be(SignatureVerificationStatus.Disallowed);
                totalErrorIssues.Count().Should().Be(1);
                totalErrorIssues.First().Code.Should().Be(NuGetLogCode.NU3000);
            }
        }

        [CIOnlyFact]
        public async Task GetTrustResultAsync_PrimarySignatureWithUntrustedRoot_EmptyAllowedUntrustedRootList_AllowUntrustedFalse_ErrorAsync()
        {
            var settings = new SignedPackageVerifierSettings(
                allowUnsigned: false,
                allowIllegal: false,
                allowUntrusted: false,
                allowIgnoreTimestamp: false,
                allowMultipleTimestamps: false,
                allowNoTimestamp: false,
                allowUnknownRevocation: true,
                reportUnknownRevocation: true,
                verificationTarget: VerificationTarget.Repository,
                signaturePlacement: SignaturePlacement.PrimarySignature,
                repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.Never,
                revocationMode: RevocationMode.Online);

            TimestampService timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();

            using (Test test = await Test.CreateRepositoryPrimarySignedPackageAsync(
                _testFixture.UntrustedTestCertificate.Cert,
                timestampService.Url))
            using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
            {
                PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);

                var provider = new SignatureTrustAndValidityVerificationProvider(allowUntrustedRootList: null);

                PackageVerificationResult status = await provider.GetTrustResultAsync(packageReader, primarySignature, settings, CancellationToken.None);

                Assert.Equal(SignatureVerificationStatus.Disallowed, status.Trust);
                SigningTestUtility.AssertUntrustedRoot(status.Issues, LogLevel.Error);
            }
        }

        [CIOnlyFact]
        public async Task GetTrustResultAsync_RepositoryCountersignatureWithUntrustedRoot_EmptyAllowedUntrustedRootList_AllowUntrustedFalse_ErrorAsync()
        {
            var settings = new SignedPackageVerifierSettings(
                allowUnsigned: false,
                allowIllegal: false,
                allowUntrusted: false,
                allowIgnoreTimestamp: false,
                allowMultipleTimestamps: false,
                allowNoTimestamp: false,
                allowUnknownRevocation: true,
                reportUnknownRevocation: true,
                verificationTarget: VerificationTarget.Repository,
                signaturePlacement: SignaturePlacement.Countersignature,
                repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.Always,
                revocationMode: RevocationMode.Online);

            TimestampService timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();

            using (Test test = await Test.CreateAuthorSignedRepositoryCountersignedPackageAsync(
                _testFixture.TrustedTestCertificate.Source.Cert,
                _testFixture.UntrustedTestCertificate.Cert,
                timestampService.Url,
                timestampService.Url))
            using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
            {
                PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);

                var provider = new SignatureTrustAndValidityVerificationProvider(allowUntrustedRootList: null);

                PackageVerificationResult status = await provider.GetTrustResultAsync(packageReader, primarySignature, settings, CancellationToken.None);

                Assert.Equal(SignatureVerificationStatus.Disallowed, status.Trust);
                SigningTestUtility.AssertUntrustedRoot(status.Issues, LogLevel.Error);
            }
        }

        [CIOnlyFact]
        public async Task GetTrustResultAsync_PrimarySignatureWithUntrustedRoot_NotInAllowedUntrustedRootList_AllowUntrustedFalse_ErrorAsync()
        {
            var settings = new SignedPackageVerifierSettings(
                allowUnsigned: false,
                allowIllegal: false,
                allowUntrusted: false,
                allowIgnoreTimestamp: false,
                allowMultipleTimestamps: false,
                allowNoTimestamp: false,
                allowUnknownRevocation: true,
                reportUnknownRevocation: true,
                verificationTarget: VerificationTarget.Repository,
                signaturePlacement: SignaturePlacement.PrimarySignature,
                repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.Never,
                revocationMode: RevocationMode.Online);

            TimestampService timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();

            using (Test test = await Test.CreateRepositoryPrimarySignedPackageAsync(
                _testFixture.UntrustedTestCertificate.Cert,
                timestampService.Url))
            using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
            {
                PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);

                var provider = new SignatureTrustAndValidityVerificationProvider(allowUntrustedRootList:
                    new List<KeyValuePair<string, HashAlgorithmName>>() { new KeyValuePair<string, HashAlgorithmName>("abc", HashAlgorithmName.SHA256) });

                PackageVerificationResult status = await provider.GetTrustResultAsync(packageReader, primarySignature, settings, CancellationToken.None);

                Assert.Equal(SignatureVerificationStatus.Disallowed, status.Trust);
                SigningTestUtility.AssertUntrustedRoot(status.Issues, LogLevel.Error);
            }
        }

        [CIOnlyFact]
        public async Task GetTrustResultAsync_RepositoryCountersignatureWithUntrustedRoot_NotInAllowedUntrustedRootList_AllowUntrustedFalse_ErrorAsync()
        {
            var settings = new SignedPackageVerifierSettings(
                allowUnsigned: false,
                allowIllegal: false,
                allowUntrusted: false,
                allowIgnoreTimestamp: false,
                allowMultipleTimestamps: false,
                allowNoTimestamp: false,
                allowUnknownRevocation: true,
                reportUnknownRevocation: true,
                verificationTarget: VerificationTarget.Repository,
                signaturePlacement: SignaturePlacement.Countersignature,
                repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.Always,
                revocationMode: RevocationMode.Online);

            TimestampService timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();

            using (Test test = await Test.CreateAuthorSignedRepositoryCountersignedPackageAsync(
                _testFixture.TrustedTestCertificate.Source.Cert,
                _testFixture.UntrustedTestCertificate.Cert,
                timestampService.Url,
                timestampService.Url))
            using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
            {
                PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);

                var provider = new SignatureTrustAndValidityVerificationProvider(allowUntrustedRootList:
                    new List<KeyValuePair<string, HashAlgorithmName>>() { new KeyValuePair<string, HashAlgorithmName>("abc", HashAlgorithmName.SHA256) });

                PackageVerificationResult status = await provider.GetTrustResultAsync(packageReader, primarySignature, settings, CancellationToken.None);

                Assert.Equal(SignatureVerificationStatus.Disallowed, status.Trust);
                SigningTestUtility.AssertUntrustedRoot(status.Issues, LogLevel.Error);
            }
        }

        [CIOnlyFact]
        public async Task GetTrustResultAsync_PrimarySignatureWithUntrustedRoot_InAllowedUntrustedRootList_AllowUntrustedFalse_SucceedsAsync()
        {
            var settings = new SignedPackageVerifierSettings(
                allowUnsigned: false,
                allowIllegal: false,
                allowUntrusted: false,
                allowIgnoreTimestamp: false,
                allowMultipleTimestamps: false,
                allowNoTimestamp: false,
                allowUnknownRevocation: true,
                reportUnknownRevocation: true,
                verificationTarget: VerificationTarget.Repository,
                signaturePlacement: SignaturePlacement.PrimarySignature,
                repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.Never,
                revocationMode: RevocationMode.Online);

            TimestampService timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();
            string untrustedCertFingerprint = SignatureTestUtility.GetFingerprint(_testFixture.UntrustedTestCertificate.Cert, HashAlgorithmName.SHA256);

            using (Test test = await Test.CreateRepositoryPrimarySignedPackageAsync(
                _testFixture.UntrustedTestCertificate.Cert,
                timestampService.Url))
            using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
            {
                PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);

                var provider = new SignatureTrustAndValidityVerificationProvider(allowUntrustedRootList:
                    new List<KeyValuePair<string, HashAlgorithmName>>() { new KeyValuePair<string, HashAlgorithmName>(untrustedCertFingerprint, HashAlgorithmName.SHA256) });

                PackageVerificationResult status = await provider.GetTrustResultAsync(packageReader, primarySignature, settings, CancellationToken.None);

                Assert.Equal(SignatureVerificationStatus.Valid, status.Trust);
                Assert.DoesNotContain(status.Issues.Where(i => i.Level >= LogLevel.Warning)
, i => i.Message.Contains(UntrustedChainCertError));
            }
        }

        [CIOnlyFact]
        public async Task GetTrustResultAsync_RepositoryCountersignatureWithUntrustedRoot_InAllowedUntrustedRootList_AllowUntrustedFalse_SucceedsAsync()
        {
            var settings = new SignedPackageVerifierSettings(
                allowUnsigned: false,
                allowIllegal: false,
                allowUntrusted: false,
                allowIgnoreTimestamp: false,
                allowMultipleTimestamps: false,
                allowNoTimestamp: false,
                allowUnknownRevocation: true,
                reportUnknownRevocation: true,
                verificationTarget: VerificationTarget.Repository,
                signaturePlacement: SignaturePlacement.Countersignature,
                repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.Always,
                revocationMode: RevocationMode.Online);

            TimestampService timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();
            string untrustedCertFingerprint = SignatureTestUtility.GetFingerprint(_testFixture.UntrustedTestCertificate.Cert, HashAlgorithmName.SHA256);

            using (Test test = await Test.CreateAuthorSignedRepositoryCountersignedPackageAsync(
                _testFixture.TrustedTestCertificate.Source.Cert,
                _testFixture.UntrustedTestCertificate.Cert,
                timestampService.Url,
                timestampService.Url))
            using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
            {
                PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);

                var provider = new SignatureTrustAndValidityVerificationProvider(allowUntrustedRootList:
                    new List<KeyValuePair<string, HashAlgorithmName>>() { new KeyValuePair<string, HashAlgorithmName>(untrustedCertFingerprint, HashAlgorithmName.SHA256) });

                PackageVerificationResult status = await provider.GetTrustResultAsync(packageReader, primarySignature, settings, CancellationToken.None);

                Assert.Equal(SignatureVerificationStatus.Valid, status.Trust);
                Assert.DoesNotContain(status.Issues.Where(i => i.Level >= LogLevel.Warning)
, i => i.Message.Contains(UntrustedChainCertError));
            }
        }

        [PlatformFact(Platform.Windows, Platform.Linux)]
        public async Task GetTrustResultAsync_WithUnavailableRevocationInformationInAcceptMode_DoesNotWarnAsync()
        {
            // Arrange
            SignedPackageVerifierSettings settings = SignedPackageVerifierSettings.GetAcceptModeDefaultPolicy(TestEnvironmentVariableReader.EmptyInstance);

            // Act & Assert
            List<SignatureLog> matchingIssues = await VerifyUnavailableRevocationInfoAsync(
                SignatureVerificationStatus.Valid,
                LogLevel.Warning,
                settings,
                _testOutputHelper);

            Assert.Empty(matchingIssues);
        }

        [PlatformFact(Platform.Windows, Platform.Linux)]
        public async Task GetTrustResultAsync_WithUnavailableRevocationInformationInRequireMode_WarnsAsync()
        {
            // Arrange
            SignedPackageVerifierSettings settings = SignedPackageVerifierSettings.GetRequireModeDefaultPolicy(TestEnvironmentVariableReader.EmptyInstance);

            // Act & Assert
            List<SignatureLog> matchingIssues = await VerifyUnavailableRevocationInfoAsync(
                SignatureVerificationStatus.Valid,
                LogLevel.Warning,
                settings,
                _testOutputHelper);

            if (RuntimeEnvironmentHelper.IsMacOSX)
            {
                Assert.Equal(1, matchingIssues.Count);
            }
            else
            {
                Assert.Equal(2, matchingIssues.Count);
                SigningTestUtility.AssertOfflineRevocationOnlineMode(matchingIssues, LogLevel.Warning);
            }

            SigningTestUtility.AssertRevocationStatusUnknown(matchingIssues, LogLevel.Warning, NuGetLogCode.NU3018);
        }

        [PlatformFact(Platform.Windows, Platform.Linux)]
        public async Task GetTrustResultAsync_WithUnavailableRevocationInformationInVerify_WarnsAsync()
        {
            // Act & Assert
            List<SignatureLog> matchingIssues = await VerifyUnavailableRevocationInfoAsync(
                SignatureVerificationStatus.Valid,
                LogLevel.Warning,
                _verifyCommandSettings,
                _testOutputHelper);

            if (RuntimeEnvironmentHelper.IsMacOSX)
            {
                Assert.Equal(1, matchingIssues.Count);
            }
            else
            {
                Assert.Equal(2, matchingIssues.Count);
                SigningTestUtility.AssertOfflineRevocationOnlineMode(matchingIssues, LogLevel.Warning);
            }

            SigningTestUtility.AssertRevocationStatusUnknown(matchingIssues, LogLevel.Warning, NuGetLogCode.NU3018);
        }

        [PlatformFact(Platform.Windows, Platform.Linux)]
        public async Task GetTrustResultAsync_WithUnavailableRevocationInformationAndAllowIllegal_WarnsAsync()
        {
            // Arrange
            var settings = new SignedPackageVerifierSettings(
                allowUnsigned: false,
                allowIllegal: true,
                allowUntrusted: false,
                allowIgnoreTimestamp: false,
                allowMultipleTimestamps: false,
                allowNoTimestamp: false,
                allowUnknownRevocation: true,
                reportUnknownRevocation: false,
                verificationTarget: VerificationTarget.All,
                signaturePlacement: SignaturePlacement.Any,
                repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.IfExistsAndIsNecessary,
                revocationMode: RevocationMode.Online);

            // Act & Assert
            List<SignatureLog> matchingIssues = await VerifyUnavailableRevocationInfoAsync(
                SignatureVerificationStatus.Valid,
                LogLevel.Warning,
                settings,
                _testOutputHelper);

            Assert.Empty(matchingIssues);
        }

        [PlatformFact(Platform.Windows, Platform.Linux)]
        public async Task GetTrustResultAsync_WithUnavailableRevocationInformationAndAllowUnknownRevocation_WithOnlineRevocationMode_WarnsAsync()
        {
            // Arrange
            var settings = new SignedPackageVerifierSettings(
                allowUnsigned: false,
                allowIllegal: false,
                allowUntrusted: false,
                allowIgnoreTimestamp: false,
                allowMultipleTimestamps: false,
                allowNoTimestamp: false,
                allowUnknownRevocation: true,
                reportUnknownRevocation: true,
                verificationTarget: VerificationTarget.All,
                signaturePlacement: SignaturePlacement.Any,
                repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.IfExistsAndIsNecessary,
                revocationMode: RevocationMode.Online);

            // Act & Assert
            List<SignatureLog> matchingIssues = await VerifyUnavailableRevocationInfoAsync(
                SignatureVerificationStatus.Valid,
                LogLevel.Warning,
                settings,
                _testOutputHelper);

            if (RuntimeEnvironmentHelper.IsMacOSX)
            {
                Assert.Equal(1, matchingIssues.Count);
            }
            else
            {
                Assert.Equal(2, matchingIssues.Count);
                SigningTestUtility.AssertOfflineRevocationOnlineMode(matchingIssues, LogLevel.Warning);
            }

            SigningTestUtility.AssertRevocationStatusUnknown(matchingIssues, LogLevel.Warning, NuGetLogCode.NU3018);
        }

        [PlatformFact(Platform.Windows, Platform.Linux)]
        public async Task GetTrustResultAsync_WithUnavailableRevocationInformationAndAllowUnknownRevocation_WithOfflineRevocationMode_WarnsAsync()
        {
            // Arrange
            var settings = new SignedPackageVerifierSettings(
                allowUnsigned: false,
                allowIllegal: false,
                allowUntrusted: false,
                allowIgnoreTimestamp: false,
                allowMultipleTimestamps: false,
                allowNoTimestamp: false,
                allowUnknownRevocation: true,
                reportUnknownRevocation: true,
                verificationTarget: VerificationTarget.All,
                signaturePlacement: SignaturePlacement.Any,
                repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.IfExistsAndIsNecessary,
                revocationMode: RevocationMode.Offline);

            // Act & Assert
            List<SignatureLog> matchingIssues = await VerifyUnavailableRevocationInfoAsync(
                SignatureVerificationStatus.Valid,
                LogLevel.Information,
                settings,
                _testOutputHelper);

            if (!RuntimeEnvironmentHelper.IsMacOSX)
            {
                SigningTestUtility.AssertOfflineRevocationOfflineMode(matchingIssues);
            }

            SigningTestUtility.AssertRevocationStatusUnknown(matchingIssues, LogLevel.Information, NuGetLogCode.Undefined);
        }

        [CIOnlyFact]
        public async Task GetTrustResultAsync_WithTrustedButExpiredPrimaryAndTimestampCertificates_WithUnavailableRevocationInformationAndAllowUnknownRevocation_WarnsAsync()
        {
            List<SignatureLog> matchingIssues = await VerifyUnavailableRevocationInfoAsync(
                SignatureVerificationStatus.Valid,
                LogLevel.Warning,
                _verifyCommandSettings,
                _testOutputHelper,
                "ExpiredPrimaryAndTimestampCertificatesWithUnavailableRevocationInfo.nupkg");

            if (RuntimeEnvironmentHelper.IsMacOSX)
            {
                Assert.Equal(2, matchingIssues.Count);
            }
            else
            {
                Assert.Equal(4, matchingIssues.Count);
                SigningTestUtility.AssertOfflineRevocationOnlineMode(matchingIssues, LogLevel.Warning);
            }

            SigningTestUtility.AssertRevocationStatusUnknown(matchingIssues, LogLevel.Warning, NuGetLogCode.NU3018);
        }

        [CIOnlyFact]
        public async Task GetTrustResultAsync_WithNoIgnoringTimestamp_TimestampWithGeneralizedTimeOutsideCertificateValidity_FailAsync()
        {
            var verificationProvider = new SignatureTrustAndValidityVerificationProvider();

            using (var nupkgStream = new MemoryStream(GetResource("TimestampInvalidGenTimePackage.nupkg")))
            using (var package = new PackageArchiveReader(nupkgStream, leaveStreamOpen: false))
            {
                PrimarySignature signature = await package.GetPrimarySignatureAsync(CancellationToken.None);

                // Act
                PackageVerificationResult result = await verificationProvider.GetTrustResultAsync(
                    package,
                    signature,
                    _verifyCommandSettings,
                    CancellationToken.None);
                IEnumerable<SignatureLog> errorIssues = result.Issues.Where(r => r.Level >= LogLevel.Error);

                // Assert
                result.Trust.Should().Be(SignatureVerificationStatus.Disallowed);
                errorIssues.Count().Should().Be(3);
                Assert.Contains(errorIssues, error => error.Code.Equals(NuGetLogCode.NU3036) &&
                                        error.Message.Contains("signature's timestamp's generalized time is outside the timestamping certificate's validity period."));


            }
        }

        [Collection(SigningTestCollection.Name)]
        public class AuthorPrimarySignatures
        {
            private readonly SigningTestFixture _fixture;
            private readonly SignatureTrustAndValidityVerificationProvider _provider;

            public AuthorPrimarySignatures(SigningTestFixture fixture)
            {
                if (fixture == null)
                {
                    throw new ArgumentNullException(nameof(fixture));
                }

                _fixture = fixture;
                _provider = new SignatureTrustAndValidityVerificationProvider();
            }

            [CIOnlyFact]
            public async Task GetTrustResultAsync_WithRepositorySignedPackage_ReturnsUnknownAsync()
            {
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: false,
                    allowUntrusted: false,
                    allowIgnoreTimestamp: false,
                    allowMultipleTimestamps: false,
                    allowNoTimestamp: false,
                    allowUnknownRevocation: true,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.Author,
                    signaturePlacement: SignaturePlacement.PrimarySignature,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.Never,
                    revocationMode: RevocationMode.Online);

                using (Test test = await Test.CreateRepositoryPrimarySignedPackageAsync(_fixture.TrustedTestCertificate.Source.Cert))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    var provider = new SignatureTrustAndValidityVerificationProvider();
                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult result = await provider.GetTrustResultAsync(packageReader, primarySignature, settings, CancellationToken.None);

                    Assert.Equal(SignatureVerificationStatus.Unknown, result.Trust);
                }
            }

            [CIOnlyFact]
            public async Task GetTrustResultAsync_WithValidSignature_ReturnsValidAsync()
            {
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: false,
                    allowUntrusted: false,
                    allowIgnoreTimestamp: false,
                    allowMultipleTimestamps: false,
                    allowNoTimestamp: false,
                    allowUnknownRevocation: true,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.Author,
                    signaturePlacement: SignaturePlacement.PrimarySignature,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.Never,
                    revocationMode: RevocationMode.Online);
                TimestampService timestampService = await _fixture.GetDefaultTrustedTimestampServiceAsync();

                using (Test test = await Test.CreateAuthorSignedPackageAsync(
                    _fixture.TrustedTestCertificate.Source.Cert,
                    timestampService.Url))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, settings, CancellationToken.None);

                    Assert.Equal(SignatureVerificationStatus.Valid, status.Trust);
                }
            }

            [CIOnlyTheory]
            [InlineData(true, SignatureVerificationStatus.Valid)]
            [InlineData(false, SignatureVerificationStatus.Disallowed)]
            public async Task GetTrustResultAsync_WithValidSignatureButNoTimestamp_ReturnsStatusAsync(
                bool allowNoTimestamp,
                SignatureVerificationStatus expectedStatus)
            {
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: false,
                    allowUntrusted: false,
                    allowIgnoreTimestamp: allowNoTimestamp,
                    allowMultipleTimestamps: false,
                    allowNoTimestamp: allowNoTimestamp,
                    allowUnknownRevocation: true,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.Author,
                    signaturePlacement: SignaturePlacement.PrimarySignature,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.Never,
                    revocationMode: RevocationMode.Online);

                using (Test test = await Test.CreateAuthorSignedPackageAsync(_fixture.TrustedTestCertificate.Source.Cert))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, settings, CancellationToken.None);

                    Assert.Equal(expectedStatus, status.Trust);
                }
            }

            [CIOnlyTheory]
            [InlineData(true, SignatureVerificationStatus.Valid)]
            [InlineData(false, SignatureVerificationStatus.Disallowed)]
            public async Task GetTrustResultAsync_WithUntrustedSignature_ReturnsStatusAsync(
                bool allowUntrusted,
                SignatureVerificationStatus expectedStatus)
            {
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: false,
                    allowUntrusted: allowUntrusted,
                    allowIgnoreTimestamp: false,
                    allowMultipleTimestamps: false,
                    allowNoTimestamp: false,
                    allowUnknownRevocation: true,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.Author,
                    signaturePlacement: SignaturePlacement.PrimarySignature,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.Never,
                    revocationMode: RevocationMode.Online);
                TimestampService timestampService = await _fixture.GetDefaultTrustedTimestampServiceAsync();

                using (Test test = await Test.CreateAuthorSignedPackageAsync(
                    _fixture.UntrustedTestCertificate.Cert,
                    timestampService.Url))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, settings, CancellationToken.None);

                    Assert.Equal(expectedStatus, status.Trust);
                }
            }

            [PlatformTheory(Platform.Windows, Platform.Linux)] // https://github.com/NuGet/Home/issues/9501
            [InlineData(true)]
            [InlineData(false)]
            public async Task GetTrustResultAsync_WithRevokedPrimaryCertificate_ReturnsSuspectAsync(bool allowEverything)
            {
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: allowEverything,
                    allowIllegal: allowEverything,
                    allowUntrusted: allowEverything,
                    allowIgnoreTimestamp: allowEverything,
                    allowMultipleTimestamps: allowEverything,
                    allowNoTimestamp: allowEverything,
                    allowUnknownRevocation: true,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.Author,
                    signaturePlacement: SignaturePlacement.PrimarySignature,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.Never,
                    revocationMode: RevocationMode.Online);
                CertificateAuthority certificateAuthority = await _fixture.GetDefaultTrustedCertificateAuthorityAsync();
                IssueCertificateOptions issueCertificateOptions = IssueCertificateOptions.CreateDefaultForEndCertificate();
                TimestampService timestampService = await _fixture.GetDefaultTrustedTimestampServiceAsync();

                using (X509Certificate2 certificate = certificateAuthority.IssueCertificate(issueCertificateOptions))
                using (Test test = await Test.CreateAuthorSignedPackageAsync(
                    certificate,
                    timestampService.Url))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    await certificateAuthority.OcspResponder.WaitForResponseExpirationAsync(certificate);

                    certificateAuthority.Revoke(
                        certificate,
                        X509RevocationReason.KeyCompromise,
                        DateTimeOffset.UtcNow.AddHours(-1));

                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, settings, CancellationToken.None);

                    Assert.Equal(SignatureVerificationStatus.Suspect, status.Trust);
                }
            }

            [PlatformTheory(Platform.Windows, Platform.Linux)] // https://github.com/NuGet/Home/issues/9501
            [InlineData(true, SignatureVerificationStatus.Valid)]
            [InlineData(false, SignatureVerificationStatus.Disallowed)]
            public async Task GetTrustResultAsync_WithRevokedTimestampCertificate_ReturnsStatusAsync(
                bool allowIgnoreTimestamp,
                SignatureVerificationStatus expectedStatus)
            {
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: false,
                    allowUntrusted: false,
                    allowIgnoreTimestamp: allowIgnoreTimestamp,
                    allowMultipleTimestamps: false,
                    allowNoTimestamp: false,
                    allowUnknownRevocation: true,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.Author,
                    signaturePlacement: SignaturePlacement.PrimarySignature,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.Never,
                    revocationMode: RevocationMode.Online);
                ISigningTestServer testServer = await _fixture.GetSigningTestServerAsync();
                CertificateAuthority certificateAuthority = await _fixture.GetDefaultTrustedCertificateAuthorityAsync();
                TimestampService timestampService = TimestampService.Create(certificateAuthority);

                using (testServer.RegisterResponder(timestampService))
                using (Test test = await Test.CreateAuthorSignedPackageAsync(
                    _fixture.TrustedRepositoryCertificate.Source.Cert,
                    timestampService.Url))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    await certificateAuthority.OcspResponder.WaitForResponseExpirationAsync(timestampService.Certificate);

                    certificateAuthority.Revoke(
                        timestampService.Certificate,
                        X509RevocationReason.KeyCompromise,
                        DateTimeOffset.UtcNow.AddHours(-1));

                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, settings, CancellationToken.None);

                    Assert.Equal(expectedStatus, status.Trust);
                }
            }

            [CIOnlyFact]
            public async Task GetTrustResultAsync_WithTamperedRepositoryPrimarySignedPackage_ReturnsValidAsync()
            {
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: false,
                    allowUntrusted: false,
                    allowIgnoreTimestamp: false,
                    allowMultipleTimestamps: false,
                    allowNoTimestamp: false,
                    allowUnknownRevocation: true,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.Author,
                    signaturePlacement: SignaturePlacement.PrimarySignature,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.Never,
                    revocationMode: RevocationMode.Online);
                TimestampService timestampService = await _fixture.GetDefaultTrustedTimestampServiceAsync();

                using (Test test = await Test.CreateAuthorSignedPackageAsync(
                    _fixture.TrustedTestCertificate.Source.Cert,
                    timestampService.Url))
                {
                    using (FileStream stream = test.PackageFile.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    {
                        stream.Position = 0;

                        stream.WriteByte(0x00);
                    }

                    using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                    {
                        PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                        PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, settings, CancellationToken.None);

                        Assert.Equal(SignatureVerificationStatus.Valid, status.Trust);
                    }
                }
            }
        }

        [Collection(SigningTestCollection.Name)]
        public class RepositoryPrimarySignatures
        {
            private readonly SignedPackageVerifierSettings _defaultSettings = SignedPackageVerifierSettings.GetDefault(TestEnvironmentVariableReader.EmptyInstance);
            private readonly SigningTestFixture _fixture;
            private readonly SignatureTrustAndValidityVerificationProvider _provider;

            public RepositoryPrimarySignatures(SigningTestFixture fixture)
            {
                if (fixture == null)
                {
                    throw new ArgumentNullException(nameof(fixture));
                }

                _fixture = fixture;
                _provider = new SignatureTrustAndValidityVerificationProvider();
            }

            [CIOnlyFact]
            public async Task GetTrustResultAsync_WithAuthorSignedPackage_ReturnsUnknownAsync()
            {
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: false,
                    allowUntrusted: false,
                    allowIgnoreTimestamp: false,
                    allowMultipleTimestamps: false,
                    allowNoTimestamp: false,
                    allowUnknownRevocation: true,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.Repository,
                    signaturePlacement: SignaturePlacement.PrimarySignature,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.Never,
                    revocationMode: RevocationMode.Online);

                using (Test test = await Test.CreateAuthorSignedPackageAsync(_fixture.TrustedTestCertificate.Source.Cert))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    var provider = new SignatureTrustAndValidityVerificationProvider();
                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult result = await provider.GetTrustResultAsync(packageReader, primarySignature, settings, CancellationToken.None);

                    Assert.Equal(SignatureVerificationStatus.Unknown, result.Trust);
                }
            }

            [CIOnlyFact]
            public async Task GetTrustResultAsync_WithValidSignature_ReturnsValidAsync()
            {
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: false,
                    allowUntrusted: false,
                    allowIgnoreTimestamp: false,
                    allowMultipleTimestamps: false,
                    allowNoTimestamp: false,
                    allowUnknownRevocation: true,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.Repository,
                    signaturePlacement: SignaturePlacement.PrimarySignature,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.Never,
                    revocationMode: RevocationMode.Online);
                TimestampService timestampService = await _fixture.GetDefaultTrustedTimestampServiceAsync();

                using (Test test = await Test.CreateRepositoryPrimarySignedPackageAsync(
                    _fixture.TrustedRepositoryCertificate.Source.Cert,
                    timestampService.Url))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, settings, CancellationToken.None);

                    Assert.Equal(SignatureVerificationStatus.Valid, status.Trust);
                }
            }

            [CIOnlyTheory]
            [InlineData(true, SignatureVerificationStatus.Valid)]
            [InlineData(false, SignatureVerificationStatus.Disallowed)]
            public async Task GetTrustResultAsync_WithValidSignatureButNoTimestamp_ReturnsStatusAsync(
                bool allowNoTimestamp,
                SignatureVerificationStatus expectedStatus)
            {
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: false,
                    allowUntrusted: false,
                    allowIgnoreTimestamp: allowNoTimestamp,
                    allowMultipleTimestamps: false,
                    allowNoTimestamp: allowNoTimestamp,
                    allowUnknownRevocation: true,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.Repository,
                    signaturePlacement: SignaturePlacement.PrimarySignature,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.Never,
                    revocationMode: RevocationMode.Online);

                using (Test test = await Test.CreateRepositoryPrimarySignedPackageAsync(
                    _fixture.TrustedRepositoryCertificate.Source.Cert))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, settings, CancellationToken.None);

                    Assert.Equal(expectedStatus, status.Trust);
                }
            }

            [CIOnlyTheory]
            [InlineData(true, SignatureVerificationStatus.Valid)]
            [InlineData(false, SignatureVerificationStatus.Disallowed)]
            public async Task GetTrustResultAsync_WithUntrustedSignature_ReturnsStatusAsync(
                bool allowUntrusted,
                SignatureVerificationStatus expectedStatus)
            {
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: false,
                    allowUntrusted: allowUntrusted,
                    allowIgnoreTimestamp: false,
                    allowMultipleTimestamps: false,
                    allowNoTimestamp: false,
                    allowUnknownRevocation: true,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.Repository,
                    signaturePlacement: SignaturePlacement.PrimarySignature,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.Never,
                    revocationMode: RevocationMode.Online);
                TimestampService timestampService = await _fixture.GetDefaultTrustedTimestampServiceAsync();

                using (Test test = await Test.CreateRepositoryPrimarySignedPackageAsync(
                    _fixture.UntrustedTestCertificate.Cert,
                    timestampService.Url))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, settings, CancellationToken.None);

                    Assert.Equal(expectedStatus, status.Trust);
                }
            }

            [PlatformTheory(Platform.Windows, Platform.Linux)] // https://github.com/NuGet/Home/issues/9501
            [InlineData(true)]
            [InlineData(false)]
            public async Task GetTrustResultAsync_WithRevokedPrimaryCertificate_ReturnsSuspectAsync(bool allowEverything)
            {
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: allowEverything,
                    allowIllegal: allowEverything,
                    allowUntrusted: allowEverything,
                    allowIgnoreTimestamp: allowEverything,
                    allowMultipleTimestamps: allowEverything,
                    allowNoTimestamp: allowEverything,
                    allowUnknownRevocation: true,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.Repository,
                    signaturePlacement: SignaturePlacement.PrimarySignature,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.Never,
                    revocationMode: RevocationMode.Online);
                CertificateAuthority certificateAuthority = await _fixture.GetDefaultTrustedCertificateAuthorityAsync();
                IssueCertificateOptions issueCertificateOptions = IssueCertificateOptions.CreateDefaultForEndCertificate();
                TimestampService timestampService = await _fixture.GetDefaultTrustedTimestampServiceAsync();

                using (X509Certificate2 certificate = certificateAuthority.IssueCertificate(issueCertificateOptions))
                using (Test test = await Test.CreateRepositoryPrimarySignedPackageAsync(
                    certificate,
                    timestampService.Url))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    await certificateAuthority.OcspResponder.WaitForResponseExpirationAsync(certificate);

                    certificateAuthority.Revoke(
                        certificate,
                        X509RevocationReason.KeyCompromise,
                        DateTimeOffset.UtcNow.AddHours(-1));

                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, settings, CancellationToken.None);

                    Assert.Equal(SignatureVerificationStatus.Suspect, status.Trust);
                }
            }

            [PlatformTheory(Platform.Windows, Platform.Linux)] // https://github.com/NuGet/Home/issues/9501
            [InlineData(true, SignatureVerificationStatus.Valid)]
            [InlineData(false, SignatureVerificationStatus.Disallowed)]
            public async Task GetTrustResultAsync_WithRevokedTimestampCertificate_ReturnsStatusAsync(
                bool allowIgnoreTimestamp,
                SignatureVerificationStatus expectedStatus)
            {
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: false,
                    allowUntrusted: false,
                    allowIgnoreTimestamp: allowIgnoreTimestamp,
                    allowMultipleTimestamps: false,
                    allowNoTimestamp: false,
                    allowUnknownRevocation: true,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.Repository,
                    signaturePlacement: SignaturePlacement.PrimarySignature,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.Never,
                    revocationMode: RevocationMode.Online);
                ISigningTestServer testServer = await _fixture.GetSigningTestServerAsync();
                CertificateAuthority certificateAuthority = await _fixture.GetDefaultTrustedCertificateAuthorityAsync();
                TimestampService timestampService = TimestampService.Create(certificateAuthority);

                using (testServer.RegisterResponder(timestampService))
                using (Test test = await Test.CreateRepositoryPrimarySignedPackageAsync(
                    _fixture.TrustedRepositoryCertificate.Source.Cert,
                    timestampService.Url))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    await certificateAuthority.OcspResponder.WaitForResponseExpirationAsync(timestampService.Certificate);

                    certificateAuthority.Revoke(
                        timestampService.Certificate,
                        X509RevocationReason.KeyCompromise,
                        DateTimeOffset.UtcNow.AddHours(-1));

                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, settings, CancellationToken.None);

                    Assert.Equal(expectedStatus, status.Trust);
                }
            }

            [CIOnlyFact]
            public async Task GetTrustResultAsync_WithTamperedRepositoryPrimarySignedPackage_ReturnsValidAsync()
            {
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: false,
                    allowUntrusted: false,
                    allowIgnoreTimestamp: false,
                    allowMultipleTimestamps: false,
                    allowNoTimestamp: false,
                    allowUnknownRevocation: true,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.Repository,
                    signaturePlacement: SignaturePlacement.PrimarySignature,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.Never,
                    revocationMode: RevocationMode.Online);
                TimestampService timestampService = await _fixture.GetDefaultTrustedTimestampServiceAsync();

                using (Test test = await Test.CreateRepositoryPrimarySignedPackageAsync(
                    _fixture.TrustedRepositoryCertificate.Source.Cert,
                    timestampService.Url))
                {
                    using (FileStream stream = test.PackageFile.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    {
                        stream.Position = 0;

                        stream.WriteByte(0x00);
                    }

                    using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                    {
                        PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                        PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, settings, CancellationToken.None);

                        Assert.Equal(SignatureVerificationStatus.Valid, status.Trust);
                    }
                }
            }

            [CIOnlyFact]
            public async Task GetTrustResultAsync_WithAlwaysVerifyCountersignatureBehavior_ReturnsDisallowedAsync()
            {
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: false,
                    allowUntrusted: false,
                    allowIgnoreTimestamp: false,
                    allowMultipleTimestamps: false,
                    allowNoTimestamp: false,
                    allowUnknownRevocation: true,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.Repository,
                    signaturePlacement: SignaturePlacement.Any,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.Always,
                    revocationMode: RevocationMode.Online);
                TimestampService timestampService = await _fixture.GetDefaultTrustedTimestampServiceAsync();

                using (Test test = await Test.CreateRepositoryPrimarySignedPackageAsync(
                    _fixture.TrustedRepositoryCertificate.Source.Cert,
                    timestampService.Url))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, settings, CancellationToken.None);

                    Assert.Equal(SignatureVerificationStatus.Disallowed, status.Trust);

                    IEnumerable<ILogMessage> errors = status.GetErrorIssues();

                    Assert.Equal(1, errors.Count());

                    ILogMessage error = errors.Single();

                    Assert.Equal(NuGetLogCode.NU3038, error.Code);
                    Assert.Equal("Verification settings require a repository countersignature, but the package does not have a repository countersignature.", error.Message);
                }
            }

            [CIOnlyFact]
            public async Task GetTrustResultAsync_WithExpiredSignature_ReturnsValidAsync()
            {
                using (X509Certificate2 certificate = await GetExpiringCertificateAsync(_fixture))
                using (Test test = await Test.CreateRepositoryPrimarySignedPackageAsync(certificate))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    await SignatureTestUtility.WaitForCertificateExpirationAsync(certificate);

                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, _defaultSettings, CancellationToken.None);

                    Assert.Equal(SignatureVerificationStatus.Valid, status.Trust);
                    Assert.Collection(
                        status.GetWarningIssues(),
                        logMessage => Assert.Equal(NuGetLogCode.NU3037, logMessage.Code),
                        logMessage => Assert.Equal(NuGetLogCode.NU3027, logMessage.Code));
                    Assert.Empty(status.GetErrorIssues());
                }
            }
        }

        [Collection(SigningTestCollection.Name)]
        public class RepositoryCountersignatures
        {
            private readonly SignedPackageVerifierSettings _defaultSettings = SignedPackageVerifierSettings.GetDefault(TestEnvironmentVariableReader.EmptyInstance);
            private readonly SigningTestFixture _fixture;
            private readonly SignatureTrustAndValidityVerificationProvider _provider;

            public RepositoryCountersignatures(SigningTestFixture fixture)
            {
                if (fixture == null)
                {
                    throw new ArgumentNullException(nameof(fixture));
                }

                _fixture = fixture;
                _provider = new SignatureTrustAndValidityVerificationProvider();
            }

            [CIOnlyFact]
            public async Task GetTrustResultAsync_WithAuthorSignedPackage_ReturnsUnknownAsync()
            {
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: false,
                    allowUntrusted: false,
                    allowIgnoreTimestamp: false,
                    allowMultipleTimestamps: false,
                    allowNoTimestamp: false,
                    allowUnknownRevocation: true,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.Repository,
                    signaturePlacement: SignaturePlacement.Countersignature,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.IfExists,
                    revocationMode: RevocationMode.Online);

                using (Test test = await Test.CreateAuthorSignedPackageAsync(_fixture.TrustedTestCertificate.Source.Cert))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    var provider = new SignatureTrustAndValidityVerificationProvider();
                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult result = await provider.GetTrustResultAsync(packageReader, primarySignature, settings, CancellationToken.None);

                    Assert.Equal(SignatureVerificationStatus.Unknown, result.Trust);
                }
            }

            [CIOnlyFact]
            public async Task GetTrustResultAsync_WithValidCountersignature_ReturnsValidAsync()
            {
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: false,
                    allowUntrusted: false,
                    allowIgnoreTimestamp: false,
                    allowMultipleTimestamps: false,
                    allowNoTimestamp: false,
                    allowUnknownRevocation: true,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.Repository,
                    signaturePlacement: SignaturePlacement.Countersignature,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.IfExists,
                    revocationMode: RevocationMode.Online);
                TimestampService timestampService = await _fixture.GetDefaultTrustedTimestampServiceAsync();

                using (Test test = await Test.CreateAuthorSignedRepositoryCountersignedPackageAsync(
                    _fixture.TrustedTestCertificate.Source.Cert,
                    _fixture.TrustedRepositoryCertificate.Source.Cert,
                    timestampService.Url,
                    timestampService.Url))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, settings, CancellationToken.None);

                    Assert.Equal(SignatureVerificationStatus.Valid, status.Trust);
                }
            }

            [CIOnlyFact]
            public async Task GetTrustResultAsync_WithValidCountersignatureAndUntrustedPrimarySignature_ReturnsValidAsync()
            {
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: false,
                    allowUntrusted: false,
                    allowIgnoreTimestamp: false,
                    allowMultipleTimestamps: false,
                    allowNoTimestamp: false,
                    allowUnknownRevocation: true,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.Repository,
                    signaturePlacement: SignaturePlacement.Countersignature,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.IfExists,
                    revocationMode: RevocationMode.Online);
                TimestampService timestampService = await _fixture.GetDefaultTrustedTimestampServiceAsync();

                using (Test test = await Test.CreateAuthorSignedRepositoryCountersignedPackageAsync(
                    _fixture.UntrustedTestCertificate.Cert,
                    _fixture.TrustedRepositoryCertificate.Source.Cert,
                    timestampService.Url,
                    timestampService.Url))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, settings, CancellationToken.None);

                    Assert.Equal(SignatureVerificationStatus.Valid, status.Trust);
                }
            }

            [CIOnlyTheory]
            [InlineData(true, SignatureVerificationStatus.Valid)]
            [InlineData(false, SignatureVerificationStatus.Disallowed)]
            public async Task GetTrustResultAsync_WithValidCountersignatureButNoTimestamp_ReturnsStatusAsync(
                bool allowNoTimestamp,
                SignatureVerificationStatus expectedStatus)
            {
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: false,
                    allowUntrusted: false,
                    allowIgnoreTimestamp: false,
                    allowMultipleTimestamps: false,
                    allowNoTimestamp: allowNoTimestamp,
                    allowUnknownRevocation: true,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.Repository,
                    signaturePlacement: SignaturePlacement.Countersignature,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.IfExists,
                    revocationMode: RevocationMode.Online);
                TimestampService timestampService = await _fixture.GetDefaultTrustedTimestampServiceAsync();

                using (Test test = await Test.CreateAuthorSignedRepositoryCountersignedPackageAsync(
                    _fixture.TrustedTestCertificate.Source.Cert,
                    _fixture.TrustedRepositoryCertificate.Source.Cert,
                    timestampService.Url))
                using (PackageArchiveReader packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, settings, CancellationToken.None);

                    Assert.Equal(expectedStatus, status.Trust);
                }
            }

            [CIOnlyTheory]
            [InlineData(true, SignatureVerificationStatus.Valid)]
            [InlineData(false, SignatureVerificationStatus.Disallowed)]
            public async Task GetTrustResultAsync_WithUntrustedCountersignature_ReturnsStatusAsync(
                bool allowUntrusted,
                SignatureVerificationStatus expectedStatus)
            {
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: false,
                    allowUntrusted: allowUntrusted,
                    allowIgnoreTimestamp: false,
                    allowMultipleTimestamps: false,
                    allowNoTimestamp: false,
                    allowUnknownRevocation: true,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.Repository,
                    signaturePlacement: SignaturePlacement.Countersignature,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.IfExists,
                    revocationMode: RevocationMode.Online);
                TimestampService timestampService = await _fixture.GetDefaultTrustedTimestampServiceAsync();

                using (Test test = await Test.CreateAuthorSignedRepositoryCountersignedPackageAsync(
                    _fixture.TrustedTestCertificate.Source.Cert,
                    _fixture.UntrustedTestCertificate.Cert,
                    timestampService.Url,
                    timestampService.Url))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, settings, CancellationToken.None);

                    Assert.Equal(expectedStatus, status.Trust);
                }
            }

            [PlatformTheory(Platform.Windows, Platform.Linux)] // https://github.com/NuGet/Home/issues/9501
            [InlineData(true)]
            [InlineData(false)]
            public async Task GetTrustResultAsync_WithRevokedCountersignatureCertificate_ReturnsSuspectAsync(bool allowEverything)
            {
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: allowEverything,
                    allowIllegal: allowEverything,
                    allowUntrusted: allowEverything,
                    allowIgnoreTimestamp: allowEverything,
                    allowMultipleTimestamps: allowEverything,
                    allowNoTimestamp: allowEverything,
                    allowUnknownRevocation: true,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.Repository,
                    signaturePlacement: SignaturePlacement.Countersignature,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.IfExists,
                    revocationMode: RevocationMode.Online);
                CertificateAuthority certificateAuthority = await _fixture.GetDefaultTrustedCertificateAuthorityAsync();
                IssueCertificateOptions issueCertificateOptions = IssueCertificateOptions.CreateDefaultForEndCertificate();
                TimestampService timestampService = await _fixture.GetDefaultTrustedTimestampServiceAsync();

                using (X509Certificate2 certificate = certificateAuthority.IssueCertificate(issueCertificateOptions))
                using (Test test = await Test.CreateAuthorSignedRepositoryCountersignedPackageAsync(
                    _fixture.TrustedTestCertificate.Source.Cert,
                    certificate,
                    timestampService.Url,
                    timestampService.Url))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    await certificateAuthority.OcspResponder.WaitForResponseExpirationAsync(certificate);

                    certificateAuthority.Revoke(
                        certificate,
                        X509RevocationReason.KeyCompromise,
                        DateTimeOffset.UtcNow.AddHours(-1));

                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, settings, CancellationToken.None);

                    Assert.Equal(SignatureVerificationStatus.Suspect, status.Trust);
                }
            }

            [PlatformTheory(Platform.Windows, Platform.Linux)] // https://github.com/NuGet/Home/issues/9501
            [InlineData(true, SignatureVerificationStatus.Valid)]
            [InlineData(false, SignatureVerificationStatus.Disallowed)]
            public async Task GetTrustResultAsync_WithRevokedTimestampCertificate_ReturnsStatusAsync(
                bool allowIgnoreTimestamp,
                SignatureVerificationStatus expectedStatus)
            {
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: false,
                    allowUntrusted: false,
                    allowIgnoreTimestamp: allowIgnoreTimestamp,
                    allowMultipleTimestamps: false,
                    allowNoTimestamp: false,
                    allowUnknownRevocation: true,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.Repository,
                    signaturePlacement: SignaturePlacement.Countersignature,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.IfExists,
                    revocationMode: RevocationMode.Online);
                ISigningTestServer testServer = await _fixture.GetSigningTestServerAsync();
                CertificateAuthority certificateAuthority = await _fixture.GetDefaultTrustedCertificateAuthorityAsync();
                TimestampService timestampService = await _fixture.GetDefaultTrustedTimestampServiceAsync();
                TimestampService revokedTimestampService = TimestampService.Create(certificateAuthority);

                using (testServer.RegisterResponder(revokedTimestampService))
                using (Test test = await Test.CreateAuthorSignedRepositoryCountersignedPackageAsync(
                    _fixture.TrustedTestCertificate.Source.Cert,
                    _fixture.TrustedRepositoryCertificate.Source.Cert,
                    timestampService.Url,
                    revokedTimestampService.Url))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    await certificateAuthority.OcspResponder.WaitForResponseExpirationAsync(revokedTimestampService.Certificate);

                    certificateAuthority.Revoke(
                        revokedTimestampService.Certificate,
                        X509RevocationReason.KeyCompromise,
                        DateTimeOffset.UtcNow.AddHours(-1));

                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, settings, CancellationToken.None);

                    Assert.Equal(expectedStatus, status.Trust);
                }
            }

            [CIOnlyFact]
            public async Task GetTrustResultAsync_WithTamperedRepositoryCountersignedPackage_ReturnsValidAsync()
            {
                var settings = new SignedPackageVerifierSettings(
                    allowUnsigned: false,
                    allowIllegal: false,
                    allowUntrusted: false,
                    allowIgnoreTimestamp: false,
                    allowMultipleTimestamps: false,
                    allowNoTimestamp: false,
                    allowUnknownRevocation: true,
                    reportUnknownRevocation: true,
                    verificationTarget: VerificationTarget.Repository,
                    signaturePlacement: SignaturePlacement.Countersignature,
                    repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.IfExists,
                    revocationMode: RevocationMode.Online);
                TimestampService timestampService = await _fixture.GetDefaultTrustedTimestampServiceAsync();

                using (Test test = await Test.CreateAuthorSignedRepositoryCountersignedPackageAsync(
                    _fixture.TrustedTestCertificate.Source.Cert,
                    _fixture.TrustedRepositoryCertificate.Source.Cert,
                    timestampService.Url,
                    timestampService.Url))
                {
                    using (FileStream stream = test.PackageFile.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    {
                        stream.Position = 0;

                        stream.WriteByte(0x00);
                    }

                    using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                    {
                        PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                        PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, settings, CancellationToken.None);

                        Assert.Equal(SignatureVerificationStatus.Valid, status.Trust);
                    }
                }
            }

            [CIOnlyFact]
            public async Task GetTrustResultAsync_WithExpiredPrimaryCertificateAndExpiredRepositoryCertificateAndValidTimestamps_ReturnsValidAsync()
            {
                TimestampService timestampService = await _fixture.GetDefaultTrustedTimestampServiceAsync();

                using (X509Certificate2 authorCertificate = await GetExpiringCertificateAsync(_fixture))
                using (X509Certificate2 repoCertificate = await GetExpiringCertificateAsync(_fixture))
                using (Test test = await Test.CreateAuthorSignedRepositoryCountersignedPackageAsync(
                    authorCertificate,
                    repoCertificate,
                    timestampService.Url,
                    timestampService.Url))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    await SignatureTestUtility.WaitForCertificateExpirationAsync(authorCertificate);
                    await SignatureTestUtility.WaitForCertificateExpirationAsync(repoCertificate);

                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, _defaultSettings, CancellationToken.None);

                    Assert.Equal(SignatureVerificationStatus.Valid, status.Trust);
                    Assert.Empty(status.GetWarningIssues());
                    Assert.Empty(status.GetErrorIssues());
                }
            }

            [CIOnlyFact]
            public async Task GetTrustResultAsync_WithExpiredRepositoryCertificateAndNoTimestamp_ReturnsValidAsync()
            {
                TimestampService timestampService = await _fixture.GetDefaultTrustedTimestampServiceAsync();

                using (X509Certificate2 repoCertificate = await GetExpiringCertificateAsync(_fixture))
                using (Test test = await Test.CreateAuthorSignedRepositoryCountersignedPackageAsync(
                    _fixture.TrustedTestCertificate.Source.Cert,
                    repoCertificate,
                    timestampService.Url))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    await SignatureTestUtility.WaitForCertificateExpirationAsync(repoCertificate);

                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, _defaultSettings, CancellationToken.None);

                    Assert.Equal(SignatureVerificationStatus.Valid, status.Trust);
                    Assert.Empty(status.GetWarningIssues());
                    Assert.Empty(status.GetErrorIssues());
                }
            }
        }

        [Collection(SigningTestCollection.Name)]
        public class FallbackFromPrimarySignaturesToCountersignatures
        {
            //The settings when validating packages from nuget.org in accept mode (AcceptModeDefaultPolicy + allowUnsigned:false + allowUntrusted:false)
            private readonly SignedPackageVerifierSettings _defaultNuGetOrgSettings = new SignedPackageVerifierSettings(
                allowUnsigned: false,
                allowIllegal: true,
                allowUntrusted: false,
                allowIgnoreTimestamp: true,
                allowMultipleTimestamps: true,
                allowNoTimestamp: true,
                allowUnknownRevocation: true,
                reportUnknownRevocation: false,
                verificationTarget: VerificationTarget.All,
                signaturePlacement: SignaturePlacement.Any,
                repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.IfExistsAndIsNecessary,
                revocationMode: RevocationMode.Online);
            private readonly SigningTestFixture _fixture;
            private readonly SignatureTrustAndValidityVerificationProvider _provider;

            public FallbackFromPrimarySignaturesToCountersignatures(SigningTestFixture fixture)
            {
                if (fixture == null)
                {
                    throw new ArgumentNullException(nameof(fixture));
                }

                _fixture = fixture;
                _provider = new SignatureTrustAndValidityVerificationProvider();
            }

            // Case1: primary signature (trusted + non-expired) doesn't fall back to countersignature (trusted + non-expired).
            // The verification result is the primary signature status(valid).
            [CIOnlyFact]
            public async Task GetTrustResultAsync_WithGoodPrimarySignatureAndGoodCountersignature_NoFallbackAndReturnsValidAsync()
            {
                using (Test test = await Test.CreateAuthorSignedRepositoryCountersignedPackageAsync(
                    authorCertificate: _fixture.TrustedTestCertificate.Source.Cert,
                    repositoryCertificate: _fixture.TrustedRepositoryCertificate.Source.Cert))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, _defaultNuGetOrgSettings, CancellationToken.None);

                    Assert.Equal(SignatureVerificationStatus.Valid, status.Trust);
                    IEnumerable<SignatureLog> NU3018Issues = status.Issues.Where(log => log.Code == NuGetLogCode.NU3018);
                    IEnumerable<SignatureLog> NU3037Issues = status.Issues.Where(log => log.Code == NuGetLogCode.NU3037);
                    Assert.Empty(NU3018Issues);
                    Assert.Empty(NU3037Issues);
                }
            }

            // Case2: primary signature (trusted + non-expired) doesn't fall back to countersignature untrusted + non-expired).
            // The verification result is the primary signature status(valid).
            [CIOnlyFact]
            public async Task GetTrustResultAsync_WithGoodPrimarySignatureAndUntrustedCountersignature_NoFallbackAndReturnsValidAsync()
            {
                using (Test test = await Test.CreateAuthorSignedRepositoryCountersignedPackageAsync(
                    authorCertificate: _fixture.TrustedTestCertificate.Source.Cert,
                    repositoryCertificate: _fixture.UntrustedTestCertificate.Cert))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, _defaultNuGetOrgSettings, CancellationToken.None);

                    Assert.Equal(SignatureVerificationStatus.Valid, status.Trust);
                    IEnumerable<SignatureLog> NU3018Issues = status.Issues.Where(log => log.Code == NuGetLogCode.NU3018);
                    IEnumerable<SignatureLog> NU3037Issues = status.Issues.Where(log => log.Code == NuGetLogCode.NU3037);
                    Assert.Empty(NU3018Issues);
                    Assert.Empty(NU3037Issues);
                }
            }

            // Case3: primary signature (untrusted + non-expired) falls back to countersignature (trusted + non-expired).
            // The verification result is the severe one of fallback status(valid) and the countersignature status(valid), so it's valid.
            [CIOnlyFact]
            public async Task GetTrustResultAsync_WithUntrustedPrimarySignatureAndGoodCountersignature_FallbackAndReturnsValidAsync()
            {
                using (Test test = await Test.CreateAuthorSignedRepositoryCountersignedPackageAsync(
                    authorCertificate: _fixture.UntrustedTestCertificate.Cert,
                    repositoryCertificate: _fixture.TrustedRepositoryCertificate.Source.Cert))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, _defaultNuGetOrgSettings, CancellationToken.None);

                    Assert.Equal(SignatureVerificationStatus.Valid, status.Trust);
                    IEnumerable<SignatureLog> NU3018Issues = status.Issues.Where(log => log.Code == NuGetLogCode.NU3018);
                    IEnumerable<SignatureLog> NU3037Issues = status.Issues.Where(log => log.Code == NuGetLogCode.NU3037);
                    Assert.Empty(NU3018Issues);
                    Assert.Empty(NU3037Issues);
                }
            }

            // Case4: primary signature (untrusted + non-expired) falls back to countersignature (untrusted + non-expired).
            // The verification result is the severe one of fallback status(disallowed) and the countersignature status(disallowed), so it's disallowed.
            [CIOnlyFact]
            public async Task GetTrustResultAsync_WithUntrustedPrimarySignatureAndUntrustedCountersignature_FallbackAndReturnsDisallowedAsync()
            {
                using (Test test = await Test.CreateAuthorSignedRepositoryCountersignedPackageAsync(
                    authorCertificate: _fixture.UntrustedTestCertificate.Cert,
                    repositoryCertificate: _fixture.UntrustedTestCertificate.Cert))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, _defaultNuGetOrgSettings, CancellationToken.None);

                    Assert.Equal(SignatureVerificationStatus.Disallowed, status.Trust);
                    IEnumerable<SignatureLog> NU3018Issues = status.Issues.Where(log => log.Code == NuGetLogCode.NU3018);
                    IEnumerable<SignatureLog> NU3037Issues = status.Issues.Where(log => log.Code == NuGetLogCode.NU3037);
                    Assert.Equal(NU3018Issues.Count(), 2);
                    Assert.Empty(NU3037Issues);
                }
            }

            // Case5: primary signature (trusted + expired) falls back to countersignature (trusted + non-expired).
            // And the timestamp on countersignature could fullfill the role of a trust anchor for primary signature.
            // The verification result is the severe one of fallback status(valid) and the countersignature status(valid), so it's valid.
            [CIOnlyFact]
            public async Task GetTrustResultAsync_WithExpiredPrimarySignatureAndGoodCountersignatureWithTimestamp_FallbackAndReturnsValidAsync()
            {
                TimestampService timestampService = await _fixture.GetDefaultTrustedTimestampServiceAsync();

                using (X509Certificate2 authorSigningCertificate = await GetExpiringCertificateAsync(_fixture))
                using (Test test = await Test.CreateAuthorSignedRepositoryCountersignedPackageAsync(
                    authorCertificate: authorSigningCertificate,
                    repositoryCertificate: _fixture.TrustedTestCertificate.Source.Cert,
                    authorTimestampServiceUrl: null,
                    repoTimestampServiceUrl: timestampService.Url))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    await SignatureTestUtility.WaitForCertificateExpirationAsync(authorSigningCertificate);
                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, _defaultNuGetOrgSettings, CancellationToken.None);

                    Assert.Equal(SignatureVerificationStatus.Valid, status.Trust);
                    IEnumerable<SignatureLog> NU3018Issues = status.Issues.Where(log => log.Code == NuGetLogCode.NU3018);
                    IEnumerable<SignatureLog> NU3037Issues = status.Issues.Where(log => log.Code == NuGetLogCode.NU3037);
                    Assert.Empty(NU3018Issues);
                    Assert.Empty(NU3037Issues);
                }
            }

            // Case6: primary signature (trusted + expired) falls back to countersignature (untrusted + non-expired).
            // The verification result is the severe one of fallback status(disallowed) and the countersignature status(disallowed), so it's disallowed.
            [CIOnlyFact]
            public async Task GetTrustResultAsync_WithExpiredPrimarySignatureAndUntrustedCountersignatureWithTimestamp_FallbackAndReturnsDisallowedAsync()
            {
                TimestampService timestampService = await _fixture.GetDefaultTrustedTimestampServiceAsync();

                using (X509Certificate2 authorSigningCertificate = await GetExpiringCertificateAsync(_fixture))
                using (Test test = await Test.CreateAuthorSignedRepositoryCountersignedPackageAsync(
                    authorCertificate: authorSigningCertificate,
                    repositoryCertificate: _fixture.UntrustedTestCertificate.Cert,
                    authorTimestampServiceUrl: null,
                    repoTimestampServiceUrl: timestampService.Url))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    await SignatureTestUtility.WaitForCertificateExpirationAsync(authorSigningCertificate);
                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, _defaultNuGetOrgSettings, CancellationToken.None);

                    Assert.Equal(SignatureVerificationStatus.Disallowed, status.Trust);
                    IEnumerable<SignatureLog> NU3018Issues = status.Issues.Where(log => log.Code == NuGetLogCode.NU3018);
                    IEnumerable<SignatureLog> NU3037Issues = status.Issues.Where(log => log.Code == NuGetLogCode.NU3037);
                    Assert.Equal(NU3018Issues.Count(), 1);
                    Assert.Equal(NU3037Issues.Count(), 1);
                }
            }

            // Case7: primary signature (trusted + expired) falls back to countersignature (trusted + non-expired).
            // But the timestamp on countersignature could NOT fullfill the role of a trust anchor for primary signature.
            // The verification result is the severe one of fallback status(disallowed) and the countersignature status(valid), so it's disallowed.
            [CIOnlyFact]
            public async Task GetTrustResultAsync_WithExpiredPrimarySignatureAndGoodCountersignatureWithNoTimestamp_FallbackAndReturnsDisallowedAsync()
            {
                using (X509Certificate2 authorSigningCertificate = await GetExpiringCertificateAsync(_fixture))
                using (Test test = await Test.CreateAuthorSignedRepositoryCountersignedPackageAsync(
                    authorCertificate: authorSigningCertificate,
                    repositoryCertificate: _fixture.TrustedTestCertificate.Source.Cert))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    await SignatureTestUtility.WaitForCertificateExpirationAsync(authorSigningCertificate);
                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, _defaultNuGetOrgSettings, CancellationToken.None);

                    Assert.Equal(SignatureVerificationStatus.Disallowed, status.Trust);
                    IEnumerable<SignatureLog> NU3018Issues = status.Issues.Where(log => log.Code == NuGetLogCode.NU3018);
                    IEnumerable<SignatureLog> NU3037Issues = status.Issues.Where(log => log.Code == NuGetLogCode.NU3037);
                    Assert.Empty(NU3018Issues);
                    Assert.Equal(NU3037Issues.Count(), 1);
                }
            }

            // Case8: primary signature (trusted + expired) falls back to countersignature (trusted + expired but protected by a timestamp).
            // And the timestamp on countersignature could fullfill the role of a trust anchor for primary signature.
            // The verification result is the severe one of fallback status(valid) and the countersignature status(valid), so it's valid.
            [CIOnlyFact]
            public async Task GetTrustResultAsync_WithExpiredPrimarySignatureAndExpiredCountersignatureWithTimestamp_FallbackAndReturnsValidAsync()
            {
                TimestampService timestampService = await _fixture.GetDefaultTrustedTimestampServiceAsync();

                using (X509Certificate2 authorSigningCertificate = await GetExpiringCertificateAsync(_fixture))
                using (X509Certificate2 repositorySigningCertificate = await GetExpiringCertificateAsync(_fixture))
                using (Test test = await Test.CreateAuthorSignedRepositoryCountersignedPackageAsync(
                    authorCertificate: authorSigningCertificate,
                    repositoryCertificate: repositorySigningCertificate,
                    authorTimestampServiceUrl: null,
                    repoTimestampServiceUrl: timestampService.Url))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    await SignatureTestUtility.WaitForCertificateExpirationAsync(authorSigningCertificate);
                    await SignatureTestUtility.WaitForCertificateExpirationAsync(repositorySigningCertificate);
                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, _defaultNuGetOrgSettings, CancellationToken.None);

                    Assert.Equal(SignatureVerificationStatus.Valid, status.Trust);
                    IEnumerable<SignatureLog> NU3018Issues = status.Issues.Where(log => log.Code == NuGetLogCode.NU3018);
                    IEnumerable<SignatureLog> NU3037Issues = status.Issues.Where(log => log.Code == NuGetLogCode.NU3037);
                    Assert.Empty(NU3018Issues);
                    Assert.Empty(NU3018Issues);
                }
            }

            // Case9: primary signature (untrusted + expired) falls back to countersignature (trusted + non-expired).
            // But the timestamp on countersignature could NOT fullfill the role of a trust anchor for primary signature.
            // The verification result is the severe one of fallback status(disallowed) and the countersignature status(valid), so it's disallowed.
            [CIOnlyFact]
            public async Task GetTrustResultAsync_WithUntrustedExpiredPrimarySignatureAndGoodCountersignatureWithNoTimestamp_FallbackAndReturnsDisallowedAsync()
            {
                using (X509Certificate2 authorSigningCertificate = _fixture.CreateUntrustedTestCertificateThatWillExpireSoon().Cert)
                using (Test test = await Test.CreateAuthorSignedRepositoryCountersignedPackageAsync(
                    authorCertificate: authorSigningCertificate,
                    repositoryCertificate: _fixture.TrustedRepositoryCertificate.Source.Cert))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    await SignatureTestUtility.WaitForCertificateExpirationAsync(authorSigningCertificate);
                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, _defaultNuGetOrgSettings, CancellationToken.None);

                    Assert.Equal(SignatureVerificationStatus.Disallowed, status.Trust);
                    IEnumerable<SignatureLog> NU3018Issues = status.Issues.Where(log => log.Code == NuGetLogCode.NU3018);
                    IEnumerable<SignatureLog> NU3037Issues = status.Issues.Where(log => log.Code == NuGetLogCode.NU3037);
                    Assert.Empty(NU3018Issues);
                    Assert.Equal(NU3037Issues.Count(), 1);
                }
            }

            // Case10: primary signature (untrusted + expired) falls back to countersignature (trusted + non-expired).
            // And the timestamp on countersignature could fullfill the role of a trust anchor for primary signature.
            // The verification result is the severe one of fallback status(valid) and the countersignature status(valid), so it's valid.
            [CIOnlyFact]
            public async Task GetTrustResultAsync_WithUntrustedExpiredPrimarySignatureAndGoodCountersignatureWithTimestamp_FallbackAndReturnsValidAsync()
            {
                TimestampService timestampService = await _fixture.GetDefaultTrustedTimestampServiceAsync();

                using (X509Certificate2 authorSigningCertificate = _fixture.CreateUntrustedTestCertificateThatWillExpireSoon().Cert)
                using (Test test = await Test.CreateAuthorSignedRepositoryCountersignedPackageAsync(
                    authorCertificate: authorSigningCertificate,
                    repositoryCertificate: _fixture.TrustedRepositoryCertificate.Source.Cert,
                    authorTimestampServiceUrl: null,
                    repoTimestampServiceUrl: timestampService.Url))
                using (var packageReader = new PackageArchiveReader(test.PackageFile.FullName))
                {
                    await SignatureTestUtility.WaitForCertificateExpirationAsync(authorSigningCertificate);
                    PrimarySignature primarySignature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);
                    PackageVerificationResult status = await _provider.GetTrustResultAsync(packageReader, primarySignature, _defaultNuGetOrgSettings, CancellationToken.None);

                    Assert.Equal(SignatureVerificationStatus.Valid, status.Trust);
                    IEnumerable<SignatureLog> NU3018Issues = status.Issues.Where(log => log.Code == NuGetLogCode.NU3018);
                    IEnumerable<SignatureLog> NU3037Issues = status.Issues.Where(log => log.Code == NuGetLogCode.NU3037);
                    Assert.Empty(NU3018Issues);
                    Assert.Empty(NU3037Issues);
                }
            }
        }

        private sealed class Test : IDisposable
        {
            private readonly TestDirectory _directory;
            private bool _isDisposed;

            internal FileInfo PackageFile { get; }

            private Test(TestDirectory directory, FileInfo package)
            {
                _directory = directory;
                PackageFile = package;
            }

            public void Dispose()
            {
                if (!_isDisposed)
                {
                    _directory.Dispose();

                    GC.SuppressFinalize(this);

                    _isDisposed = true;
                }
            }

            internal static async Task<Test> CreateAuthorSignedPackageAsync(
                X509Certificate2 certificate,
                Uri timestampServiceUrl = null)
            {
                var packageContext = new SimpleTestPackageContext();
                TestDirectory directory = TestDirectory.Create();
                string signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                    certificate,
                    packageContext,
                    directory,
                    timestampServiceUrl);

                return new Test(directory, new FileInfo(signedPackagePath));
            }

            internal static async Task<Test> CreateRepositoryPrimarySignedPackageAsync(
                X509Certificate2 certificate,
                Uri timestampServiceUrl = null)
            {
                var packageContext = new SimpleTestPackageContext();
                TestDirectory directory = TestDirectory.Create();
                string signedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                    certificate,
                    packageContext,
                    directory,
                    new Uri("https://nuget.test"),
                    timestampServiceUrl);

                return new Test(directory, new FileInfo(signedPackagePath));
            }

            internal static async Task<Test> CreateAuthorSignedRepositoryCountersignedPackageAsync(
                X509Certificate2 authorCertificate,
                X509Certificate2 repositoryCertificate,
                Uri authorTimestampServiceUrl = null,
                Uri repoTimestampServiceUrl = null)
            {
                TestDirectory directory = TestDirectory.Create();

                using (Test test = await CreateAuthorSignedPackageAsync(authorCertificate, authorTimestampServiceUrl))
                {
                    string signedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                        repositoryCertificate,
                        test.PackageFile.FullName,
                        directory,
                        new Uri("https://nuget.test"),
                        repoTimestampServiceUrl);

                    return new Test(directory, new FileInfo(signedPackagePath));
                }
            }
        }

        private static async Task<List<SignatureLog>> VerifyUnavailableRevocationInfoAsync(
            SignatureVerificationStatus expectedStatus,
            LogLevel expectedLogLevel,
            SignedPackageVerifierSettings settings,
            ITestOutputHelper testOutputHelper,
            string resourceName = "UnavailableCrlPackage.nupkg")
        {
            var verificationProvider = new SignatureTrustAndValidityVerificationProvider();

            using (var nupkgStream = new MemoryStream(GetResource(resourceName)))
            using (var package = new PackageArchiveReader(nupkgStream, leaveStreamOpen: false))
            {
                // Read a signature that is valid in every way except that the CRL information is unavailable.
                PrimarySignature signature = await package.GetPrimarySignatureAsync(CancellationToken.None);

                using (TrustPrimaryRootCertificate(signature))
                using (TrustPrimaryTimestampRootCertificate(signature))
                {
                    // Act
                    PackageVerificationResult result = await verificationProvider.GetTrustResultAsync(package, signature, settings, CancellationToken.None);

                    foreach (SignatureLog item in result.Issues)
                    {
                        testOutputHelper?.WriteLine(item.FormatWithCode());
                    }

                    // Assert
                    Assert.Equal(expectedStatus, result.Trust);

                    return result
                        .Issues
                        .Where(x => x.Level >= expectedLogLevel)
                        .OrderBy(x => x.Message)
                        .ToList();
                }
            }
        }

        private static async Task<X509Certificate2> GetExpiringCertificateAsync(SigningTestFixture fixture)
        {
            CertificateAuthority ca = await fixture.GetDefaultTrustedCertificateAuthorityAsync();

            using (System.Security.Cryptography.RSA keyPair = SigningTestUtility.GenerateKeyPair(publicKeyLength: 2048))
            {
                DateTimeOffset now = DateTimeOffset.UtcNow;
                var issueOptions = new IssueCertificateOptions()
                {
                    KeyPair = keyPair,
                    NotAfter = now.AddSeconds(10),
                    NotBefore = now.AddSeconds(-2),
                    SubjectName = new X500DistinguishedName("CN=NuGet Test Expired Certificate")
                };

                return ca.IssueCertificate(issueOptions);
            }
        }

        private static byte[] GetResource(string name)
        {
            return ResourceTestUtility.GetResourceBytes(
                $"NuGet.Packaging.FuncTest.compiler.resources.{name}",
                typeof(SignatureTrustAndValidityVerificationProviderTests));
        }

        private static IDisposable TrustPrimaryRootCertificate(PrimarySignature signature)
        {
            using (IX509CertificateChain certificateChain = SignatureUtility.GetCertificateChain(signature))
            {
                return TrustRootCertificate(certificateChain, X509StorePurpose.CodeSigning);
            }
        }

        private static IDisposable TrustPrimaryTimestampRootCertificate(PrimarySignature signature)
        {
            Timestamp timestamp = signature.Timestamps.FirstOrDefault();

            if (timestamp == null)
            {
                return null;
            }

            using (IX509CertificateChain certificateChain = SignatureUtility.GetTimestampCertificateChain(signature))
            {
                return TrustRootCertificate(certificateChain, X509StorePurpose.Timestamping);
            }
        }

        private static IDisposable TrustRootCertificate(IX509CertificateChain certificateChain, X509StorePurpose storePurpose)
        {
            X509Certificate2 rootCertificate = certificateChain.Last();
            StoreLocation storeLocation = CertificateStoreUtilities.GetTrustedCertificateStoreLocation();

            return TrustedTestCert.Create(
                new X509Certificate2(rootCertificate),
                storePurpose,
                StoreName.Root,
                storeLocation,
                maximumValidityPeriod: TimeSpan.MaxValue);
        }
    }
}
#endif
