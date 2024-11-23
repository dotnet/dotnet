// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED

using System;
using System.Collections.Generic;
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

namespace NuGet.Packaging.FuncTest
{
    [Collection(SigningTestCollection.Name)]
    public class AllowListVerificationProviderTests
    {
        private const string _noMatchInAllowList = "The package signature certificate fingerprint does not match any certificate fingerprint in the allow list.";
        private const string _noAllowList = "A list of trusted signers is required but none was found.";

        private readonly SigningTestFixture _testFixture;
        private readonly TrustedTestCert<TestCertificate> _trustedRepoTestCert;

        public AllowListVerificationProviderTests(SigningTestFixture fixture)
        {
            _testFixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            // Do not dispose this.  The fixture will dispose it.
            _trustedRepoTestCert = fixture.TrustedRepositoryCertificate;
        }

        [CIOnlyFact]
        public async Task GetTrustResultAsync_AuthorSignedPackage_WithCertificateInAllowList_SuccessAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_testFixture.TrustedTestCertificate.Source.Cert))
            {
                var certificateFingerprintString = SignatureTestUtility.GetFingerprint(testCertificate, HashAlgorithmName.SHA256);

                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);

                var allowListHashes = new[] { certificateFingerprintString, "abc" };
                var allowList = allowListHashes.Select(hash => new CertificateHashAllowListEntry(
                    VerificationTarget.Author | VerificationTarget.Repository,
                    SignaturePlacement.PrimarySignature,
                    hash,
                    HashAlgorithmName.SHA256)).ToList();

                var trustProviders = new[]
                {
                    new AllowListVerificationProvider(allowList)
                };
                var verifier = new PackageSignatureVerifier(trustProviders);
                var verifierSettings = GetSettings(allowUntrusted: false);

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, verifierSettings, CancellationToken.None);
                    var resultsWithWarnings = result.Results.Where(r => r.GetWarningIssues().Any());
                    var totalWarningIssues = resultsWithWarnings.SelectMany(r => r.GetWarningIssues());

                    // Assert
                    result.IsValid.Should().BeTrue();
                    totalWarningIssues.Count().Should().Be(0);
                }
            }
        }

        [CIOnlyTheory]
        [MemberData(nameof(EmptyNullAndRequiredListCombinations))]
        public async Task GetTrustResultAsync_AuthorSignedPackage_RequirementsAsync(
            SignedPackageVerifierSettings verifierSettings,
            IReadOnlyCollection<VerificationAllowListEntry> allowList,
            bool allowNoAllowList,
            bool valid,
            int resultsWithErrorsCount,
            int totalErrorsCount,
            object[][] issues)
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_testFixture.TrustedTestCertificate.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);

                var trustProviders = new[]
                {
                    new AllowListVerificationProvider(allowList, !allowNoAllowList)
                };
                var verifier = new PackageSignatureVerifier(trustProviders);

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, verifierSettings, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any()).ToList();

                    // Given that the runs are in parallel we need to specify an order to verify them to avoid test flakiness
                    var totalErrors = resultsWithErrors.SelectMany(r => r.GetErrorIssues()).OrderByDescending(i => i.Message.Length).ToList();

                    // Assert
                    result.IsValid.Should().Be(valid);
                    resultsWithErrors.Count().Should().Be(resultsWithErrorsCount);
                    totalErrors.Count().Should().Be(totalErrorsCount);

                    for (var i = 0; i < totalErrorsCount; i++)
                    {
                        totalErrors[i].Code.Should().Be((NuGetLogCode)issues[i][0]);
                        totalErrors[i].Message.Should().Be((string)issues[i][1]);
                    }
                }
            }
        }

        [CIOnlyFact]
        public async Task GetTrustResultAsync_AuthorSignedPackage_VerifyWithoutCertificateInAllowList_AllowUntrusted_WarnAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_testFixture.TrustedTestCertificate.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);

                var allowListHashes = new[] { "abc" };
                var allowList = allowListHashes.Select(hash => new CertificateHashAllowListEntry(
                    VerificationTarget.Author | VerificationTarget.Repository,
                    SignaturePlacement.PrimarySignature,
                    hash,
                    HashAlgorithmName.SHA256)).ToList();

                var trustProviders = new[]
                {
                    new AllowListVerificationProvider(allowList)
                };
                var verifier = new PackageSignatureVerifier(trustProviders);
                var verifierSettings = GetSettings(allowUntrusted: true);

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, verifierSettings, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var resultsWithWarnings = result.Results.Where(r => r.GetWarningIssues().Any());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());
                    var totalWarningIssues = resultsWithWarnings.SelectMany(r => r.GetWarningIssues());

                    // Assert
                    result.IsValid.Should().BeTrue();
                    resultsWithErrors.Count().Should().Be(0);
                    resultsWithWarnings.Count().Should().Be(1);
                    totalErrorIssues.Count().Should().Be(0);
                    totalWarningIssues.Count().Should().Be(1);
                    totalWarningIssues.First().Code.Should().Be(NuGetLogCode.NU3034);
                    totalWarningIssues.First().Message.Should().Be(_noMatchInAllowList);
                }
            }
        }

        [CIOnlyFact]
        public async Task GetTrustResultAsync_AuthorSignedPackage_VerifyWithoutCertificateInAllowList_NotAllowUntrusted_ErrorAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_testFixture.TrustedTestCertificate.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, dir);

                var allowListHashes = new[] { "abc" };
                var allowList = allowListHashes.Select(hash => new CertificateHashAllowListEntry(
                    VerificationTarget.Author | VerificationTarget.Repository,
                    SignaturePlacement.PrimarySignature,
                    hash,
                    HashAlgorithmName.SHA256)).ToList();

                var trustProviders = new[]
                {
                    new AllowListVerificationProvider(allowList)
                };
                var verifier = new PackageSignatureVerifier(trustProviders);
                var verifierSettings = GetSettings(allowUntrusted: false);

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, verifierSettings, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var resultsWithWarnings = result.Results.Where(r => r.GetWarningIssues().Any());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());
                    var totalWarningIssues = resultsWithWarnings.SelectMany(r => r.GetWarningIssues());

                    // Assert
                    result.IsValid.Should().BeFalse();
                    resultsWithErrors.Count().Should().Be(1);
                    resultsWithWarnings.Count().Should().Be(0);
                    totalErrorIssues.Count().Should().Be(1);
                    totalWarningIssues.Count().Should().Be(0);
                    totalErrorIssues.First().Code.Should().Be(NuGetLogCode.NU3034);
                    totalErrorIssues.First().Message.Should().Be(_noMatchInAllowList);
                }
            }
        }

        [CIOnlyFact]
        public async Task GetTrustResultAsync_RepositorySignedPackage_AllowListWithAuthorTarget_AndPrimaryPlacement_ErrorAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedRepoTestCert.Source.Cert))
            {
                var certificateFingerprintString = SignatureTestUtility.GetFingerprint(testCertificate, HashAlgorithmName.SHA256);

                var signedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(testCertificate, nupkg, dir, new Uri("https://v3serviceIndex.test/api/index.json"));

                var allowListHashes = new[] { "abc", certificateFingerprintString };
                var allowList = allowListHashes.Select(hash => new CertificateHashAllowListEntry(
                    VerificationTarget.Author,
                    SignaturePlacement.PrimarySignature,
                    hash,
                    HashAlgorithmName.SHA256)).ToList();

                var trustProviders = new[]
                {
                    new AllowListVerificationProvider(allowList)
                };
                var verifier = new PackageSignatureVerifier(trustProviders);
                var verifierSettings = GetSettings(allowUntrusted: false);

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, verifierSettings, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var resultsWithWarnings = result.Results.Where(r => r.GetWarningIssues().Any());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());
                    var totalWarningIssues = resultsWithWarnings.SelectMany(r => r.GetWarningIssues());

                    // Assert
                    result.IsValid.Should().BeFalse();
                    resultsWithErrors.Count().Should().Be(1);
                    resultsWithWarnings.Count().Should().Be(0);
                    totalErrorIssues.Count().Should().Be(1);
                    totalWarningIssues.Count().Should().Be(0);
                    totalErrorIssues.First().Code.Should().Be(NuGetLogCode.NU3034);
                    totalErrorIssues.First().Message.Should().Be(_noMatchInAllowList);
                }
            }
        }

        [CIOnlyFact]
        public async Task GetTrustResultAsync_RepositorySignedPackage_AllowListWithRepositoryTarget_AndCounterPlacementOnly_ErrorAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedRepoTestCert.Source.Cert))
            {
                var certificateFingerprintString = SignatureTestUtility.GetFingerprint(testCertificate, HashAlgorithmName.SHA256);

                var signedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(testCertificate, nupkg, dir, new Uri("https://v3serviceIndex.test/api/index.json"));

                var allowListHashes = new[] { "abc", certificateFingerprintString };
                var allowList = allowListHashes.Select(hash => new CertificateHashAllowListEntry(
                    VerificationTarget.Repository,
                    SignaturePlacement.Countersignature,
                    hash,
                    HashAlgorithmName.SHA256)).ToList();

                var trustProviders = new[]
                {
                    new AllowListVerificationProvider(allowList)
                };
                var verifier = new PackageSignatureVerifier(trustProviders);
                var verifierSettings = GetSettings(allowUntrusted: false);

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, verifierSettings, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var resultsWithWarnings = result.Results.Where(r => r.GetWarningIssues().Any());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());
                    var totalWarningIssues = resultsWithWarnings.SelectMany(r => r.GetWarningIssues());

                    // Assert
                    result.IsValid.Should().BeFalse();
                    resultsWithErrors.Count().Should().Be(1);
                    resultsWithWarnings.Count().Should().Be(0);
                    totalErrorIssues.Count().Should().Be(1);
                    totalWarningIssues.Count().Should().Be(0);
                    totalErrorIssues.First().Code.Should().Be(NuGetLogCode.NU3034);
                    totalErrorIssues.First().Message.Should().Be(_noMatchInAllowList);
                }
            }
        }

        [CIOnlyFact]
        public async Task GetTrustResultAsync_RepositorySignedPackage_AllowListWithRepositoryTarget_AndPrimaryPlacement_SuccessAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var testCertificate = new X509Certificate2(_trustedRepoTestCert.Source.Cert))
            {
                var certificateFingerprintString = SignatureTestUtility.GetFingerprint(testCertificate, HashAlgorithmName.SHA256);

                var signedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(testCertificate, nupkg, dir, new Uri("https://v3serviceIndex.test/api/index.json"));

                var allowListHashes = new[] { "abc", certificateFingerprintString };
                var allowList = allowListHashes.Select(hash => new CertificateHashAllowListEntry(
                    VerificationTarget.Repository,
                    SignaturePlacement.PrimarySignature | SignaturePlacement.Countersignature,
                    hash,
                    HashAlgorithmName.SHA256)).ToList();

                var trustProviders = new[]
                {
                    new AllowListVerificationProvider(allowList)
                };
                var verifier = new PackageSignatureVerifier(trustProviders);
                var verifierSettings = GetSettings(allowUntrusted: false);

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, verifierSettings, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var resultsWithWarnings = result.Results.Where(r => r.GetWarningIssues().Any());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());
                    var totalWarningIssues = resultsWithWarnings.SelectMany(r => r.GetWarningIssues());

                    // Assert
                    result.IsValid.Should().BeTrue();
                    resultsWithErrors.Count().Should().Be(0);
                    resultsWithWarnings.Count().Should().Be(0);
                    totalErrorIssues.Count().Should().Be(0);
                    totalWarningIssues.Count().Should().Be(0);
                }
            }
        }

        [CIOnlyFact]
        public async Task GetTrustResultAsync_RepositoryCountersignedPackage_AllowListWithAuthorTarget_AndPrimaryPlacement_ErrorAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var primaryCertificate = new X509Certificate2(_testFixture.TrustedTestCertificate.Source.Cert))
            using (var counterCertificate = new X509Certificate2(_trustedRepoTestCert.Source.Cert))
            {
                var certificateFingerprintString = SignatureTestUtility.GetFingerprint(counterCertificate, HashAlgorithmName.SHA256);

                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(primaryCertificate, nupkg, dir);
                var countersignedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(counterCertificate, signedPackagePath, dir, new Uri("https://v3serviceIndex.test/api/index.json"));

                var allowListHashes = new[] { "abc", certificateFingerprintString };
                var allowList = allowListHashes.Select(hash => new CertificateHashAllowListEntry(
                    VerificationTarget.Author,
                    SignaturePlacement.PrimarySignature,
                    hash,
                    HashAlgorithmName.SHA256)).ToList();

                var trustProviders = new[]
                {
                    new AllowListVerificationProvider(allowList)
                };
                var verifier = new PackageSignatureVerifier(trustProviders);
                var verifierSettings = GetSettings(allowUntrusted: false);

                using (var packageReader = new PackageArchiveReader(countersignedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, verifierSettings, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var resultsWithWarnings = result.Results.Where(r => r.GetWarningIssues().Any());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());
                    var totalWarningIssues = resultsWithWarnings.SelectMany(r => r.GetWarningIssues());

                    // Assert
                    result.IsValid.Should().BeFalse();
                    resultsWithErrors.Count().Should().Be(1);
                    resultsWithWarnings.Count().Should().Be(0);
                    totalErrorIssues.Count().Should().Be(1);
                    totalWarningIssues.Count().Should().Be(0);
                    totalErrorIssues.First().Code.Should().Be(NuGetLogCode.NU3034);
                    totalErrorIssues.First().Message.Should().Be(_noMatchInAllowList);
                }
            }
        }

        [CIOnlyFact]
        public async Task GetTrustResultAsync_RepositoryCountersignedPackage_AllowListWithRepositoryTarget_AndPrimaryPlacementOnly_ErrorAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var primaryCertificate = new X509Certificate2(_testFixture.TrustedTestCertificate.Source.Cert))
            using (var counterCertificate = new X509Certificate2(_trustedRepoTestCert.Source.Cert))
            {
                var certificateFingerprintString = SignatureTestUtility.GetFingerprint(counterCertificate, HashAlgorithmName.SHA256);

                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(primaryCertificate, nupkg, dir);
                var countersignedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(counterCertificate, signedPackagePath, dir, new Uri("https://v3serviceIndex.test/api/index.json"));

                var allowListHashes = new[] { "abc", certificateFingerprintString };
                var allowList = allowListHashes.Select(hash => new CertificateHashAllowListEntry(
                    VerificationTarget.Author | VerificationTarget.Repository,
                    SignaturePlacement.PrimarySignature,
                    hash,
                    HashAlgorithmName.SHA256)).ToList();

                var trustProviders = new[]
                {
                    new AllowListVerificationProvider(allowList)
                };
                var verifier = new PackageSignatureVerifier(trustProviders);
                var verifierSettings = GetSettings(allowUntrusted: false);

                using (var packageReader = new PackageArchiveReader(countersignedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, verifierSettings, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var resultsWithWarnings = result.Results.Where(r => r.GetWarningIssues().Any());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());
                    var totalWarningIssues = resultsWithWarnings.SelectMany(r => r.GetWarningIssues());

                    // Assert
                    result.IsValid.Should().BeFalse();
                    resultsWithErrors.Count().Should().Be(1);
                    resultsWithWarnings.Count().Should().Be(0);
                    totalErrorIssues.Count().Should().Be(1);
                    totalWarningIssues.Count().Should().Be(0);
                    totalErrorIssues.First().Code.Should().Be(NuGetLogCode.NU3034);
                    totalErrorIssues.First().Message.Should().Be(_noMatchInAllowList);
                }
            }
        }

        [CIOnlyFact]
        public async Task GetTrustResultAsync_RepositoryCountersignedPackage_AllowListWithRepositoryTarget_AndCounterPlacement_SuccessAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext();

            using (var dir = TestDirectory.Create())
            using (var primaryCertificate = new X509Certificate2(_testFixture.TrustedTestCertificate.Source.Cert))
            using (var counterCertificate = new X509Certificate2(_trustedRepoTestCert.Source.Cert))
            {
                var certificateFingerprintString = SignatureTestUtility.GetFingerprint(counterCertificate, HashAlgorithmName.SHA256);

                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(primaryCertificate, nupkg, dir);
                var countersignedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(counterCertificate, signedPackagePath, dir, new Uri("https://v3serviceIndex.test/api/index.json"));

                var allowListHashes = new[] { "abc", certificateFingerprintString };
                var allowList = allowListHashes.Select(hash => new CertificateHashAllowListEntry(
                    VerificationTarget.Author | VerificationTarget.Repository,
                    SignaturePlacement.PrimarySignature | SignaturePlacement.Countersignature,
                    hash,
                    HashAlgorithmName.SHA256)).ToList();

                var trustProviders = new[]
                {
                    new AllowListVerificationProvider(allowList)
                };
                var verifier = new PackageSignatureVerifier(trustProviders);
                var verifierSettings = GetSettings(allowUntrusted: false);

                using (var packageReader = new PackageArchiveReader(countersignedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, verifierSettings, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var resultsWithWarnings = result.Results.Where(r => r.GetWarningIssues().Any());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());
                    var totalWarningIssues = resultsWithWarnings.SelectMany(r => r.GetWarningIssues());

                    // Assert
                    result.IsValid.Should().BeTrue();
                    resultsWithErrors.Count().Should().Be(0);
                    resultsWithWarnings.Count().Should().Be(0);
                    totalErrorIssues.Count().Should().Be(0);
                    totalWarningIssues.Count().Should().Be(0);
                }
            }
        }

        [CIOnlyFact]
        public async Task GetTrustResultAsync_RepositoryPrimarySignedPackage_PackageSignedWithCertFromAllowList_RequireMode_SuccessAsync()
        {
            var nupkg = new SimpleTestPackageContext();

            // Arrange
            using (var dir = TestDirectory.Create())
            using (var repoCertificate = new X509Certificate2(_trustedRepoTestCert.Source.Cert))
            {
                var hashAlgorithmName = HashAlgorithmName.SHA256;
                var fingerprint = SignatureTestUtility.GetFingerprint(repoCertificate, hashAlgorithmName);
                var allowList = new List<CertificateHashAllowListEntry>()
                {
                    new CertificateHashAllowListEntry(VerificationTarget.Repository, SignaturePlacement.PrimarySignature, fingerprint, hashAlgorithmName)
                };

                var signedPackageVerifierSettings = SignedPackageVerifierSettings.GetRequireModeDefaultPolicy(TestEnvironmentVariableReader.EmptyInstance);
                var trustProviders = new[]
                {
                    new AllowListVerificationProvider(allowList, requireNonEmptyAllowList: true)
                };
                var verifier = new PackageSignatureVerifier(trustProviders);

                var repoSignedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(repoCertificate, nupkg, dir, new Uri(@"https://v3serviceIndex.test/api/index.json"));

                using (var packageReader = new PackageArchiveReader(repoSignedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, signedPackageVerifierSettings, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var resultsWithWarnings = result.Results.Where(r => r.GetWarningIssues().Any());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());
                    var totalWarningIssues = resultsWithWarnings.SelectMany(r => r.GetWarningIssues());

                    // Assert
                    result.IsValid.Should().BeTrue();
                    resultsWithErrors.Count().Should().Be(0);
                    resultsWithWarnings.Count().Should().Be(0);
                    totalErrorIssues.Count().Should().Be(0);
                    totalWarningIssues.Count().Should().Be(0);
                }
            }
        }

        [CIOnlyFact]
        public async Task GetTrustResultAsync_RepositoryPrimarySignedPackage_PackageSignedWithCertNotFromAllowList_RequireMode_ErrorAsync()
        {
            var nupkg = new SimpleTestPackageContext();

            // Arrange
            using (var dir = TestDirectory.Create())
            using (var repoCertificate = new X509Certificate2(_trustedRepoTestCert.Source.Cert))
            using (var trustedCertificate = SigningTestUtility.GenerateTrustedTestCertificate())
            using (var packageSignatureCertificate = trustedCertificate.Source.Cert)
            {
                var hashAlgorithmName = HashAlgorithmName.SHA256;
                var fingerprint = "abcdefg";
                var allowList = new List<CertificateHashAllowListEntry>()
                {
                    new CertificateHashAllowListEntry(VerificationTarget.Repository, SignaturePlacement.PrimarySignature, fingerprint, hashAlgorithmName)
                };

                var signedPackageVerifierSettings = SignedPackageVerifierSettings.GetRequireModeDefaultPolicy(TestEnvironmentVariableReader.EmptyInstance);

                var trustProviders = new[]
                 {
                    new AllowListVerificationProvider(allowList, requireNonEmptyAllowList: true)
                };
                var verifier = new PackageSignatureVerifier(trustProviders);

                var repoSignedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(repoCertificate, nupkg, dir, new Uri(@"https://v3serviceIndex.test/api/index.json"));

                using (var packageReader = new PackageArchiveReader(repoSignedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, signedPackageVerifierSettings, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var resultsWithWarnings = result.Results.Where(r => r.GetWarningIssues().Any());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());
                    var totalWarningIssues = resultsWithWarnings.SelectMany(r => r.GetWarningIssues());

                    // Assert
                    result.IsValid.Should().BeFalse();
                    resultsWithErrors.Count().Should().Be(1);
                    resultsWithWarnings.Count().Should().Be(0);
                    totalWarningIssues.Count().Should().Be(0);


                    totalErrorIssues.Count().Should().Be(1);
                    totalErrorIssues.First().Code.Should().Be(NuGetLogCode.NU3034);
                    totalErrorIssues.First().Message.Should().Be(_noMatchInAllowList);
                }
            }
        }

        [CIOnlyFact]
        public async Task GetTrustResultAsync_RepositoryPrimarySignedPackage_PackageSignedWithCertFromAllowList_WithOwnerInOwnersList_RequireMode_SuccessAsync()
        {
            var nupkg = new SimpleTestPackageContext();

            // Arrange
            using (var dir = TestDirectory.Create())
            using (var repoCertificate = new X509Certificate2(_trustedRepoTestCert.Source.Cert))
            {
                var hashAlgorithmName = HashAlgorithmName.SHA256;
                var fingerprint = SignatureTestUtility.GetFingerprint(repoCertificate, hashAlgorithmName);

                var signedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                    repoCertificate,
                    nupkg,
                    dir,
                    v3ServiceIndex: new Uri("https://v3serviceIndex.test/api/index.json"),
                    timestampService: null,
                    packageOwners: new List<string>() { "owner1", "owner4" });

                var allowList = new List<CertificateHashAllowListEntry>()
                {
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, fingerprint, hashAlgorithmName, owners: new List<string>() { "owner1", "owner2", "owner3" })
                };

                var signedPackageVerifierSettings = SignedPackageVerifierSettings.GetRequireModeDefaultPolicy(TestEnvironmentVariableReader.EmptyInstance);
                var signedPackageVerifier = new PackageSignatureVerifier(new ISignatureVerificationProvider[]
                {
                    new AllowListVerificationProvider(allowList, requireNonEmptyAllowList: true)
                });

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await signedPackageVerifier.VerifySignaturesAsync(packageReader, signedPackageVerifierSettings, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var resultsWithWarnings = result.Results.Where(r => r.GetWarningIssues().Any());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());
                    var totalWarningIssues = resultsWithWarnings.SelectMany(r => r.GetWarningIssues());

                    // Assert
                    result.IsValid.Should().BeTrue();
                    resultsWithErrors.Count().Should().Be(0);
                    resultsWithWarnings.Count().Should().Be(0);
                    totalErrorIssues.Count().Should().Be(0);
                    totalWarningIssues.Count().Should().Be(0);
                }
            }
        }

        [CIOnlyFact]
        public async Task GetTrustResultAsync_RepositoryPrimarySignedPackage_PackageSignedWithCertFromAllowList_WithOwnerNotInOwnersList_RequireMode_ErrorAsync()
        {
            var nupkg = new SimpleTestPackageContext();

            // Arrange
            using (var dir = TestDirectory.Create())
            using (var repoCertificate = new X509Certificate2(_trustedRepoTestCert.Source.Cert))
            {
                var hashAlgorithmName = HashAlgorithmName.SHA256;
                var fingerprint = SignatureTestUtility.GetFingerprint(repoCertificate, hashAlgorithmName);

                var signedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                    repoCertificate,
                    nupkg,
                    dir,
                    v3ServiceIndex: new Uri("https://v3serviceIndex.test/api/index.json"),
                    timestampService: null,
                    packageOwners: new List<string>() { "owner4", "owner5" });

                var allowList = new List<CertificateHashAllowListEntry>()
                {
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, fingerprint, hashAlgorithmName, owners: new List<string>() { "owner1", "owner2", "owner3" })
                };

                var signedPackageVerifierSettings = SignedPackageVerifierSettings.GetRequireModeDefaultPolicy(TestEnvironmentVariableReader.EmptyInstance);
                var signedPackageVerifier = new PackageSignatureVerifier(new ISignatureVerificationProvider[]
                {
                    new AllowListVerificationProvider(allowList, requireNonEmptyAllowList: true)
                });

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await signedPackageVerifier.VerifySignaturesAsync(packageReader, signedPackageVerifierSettings, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var resultsWithWarnings = result.Results.Where(r => r.GetWarningIssues().Any());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());
                    var totalWarningIssues = resultsWithWarnings.SelectMany(r => r.GetWarningIssues());

                    // Assert
                    result.IsValid.Should().BeFalse();
                    resultsWithErrors.Count().Should().Be(1);
                    resultsWithWarnings.Count().Should().Be(0);
                    totalErrorIssues.Count().Should().Be(1);
                    totalErrorIssues.First().Message.Should().Be(_noMatchInAllowList);
                    totalWarningIssues.Count().Should().Be(0);
                }
            }
        }

        [CIOnlyFact]
        public async Task GetTrustResultAsync_RepositoryPrimarySignedPackage_PackageSignedWithCertFromAllowList_WithNoOwnersInPackage_RequireMode_ErrorAsync()
        {
            var nupkg = new SimpleTestPackageContext();

            // Arrange
            using (var dir = TestDirectory.Create())
            using (var repoCertificate = new X509Certificate2(_trustedRepoTestCert.Source.Cert))
            {
                var hashAlgorithmName = HashAlgorithmName.SHA256;
                var fingerprint = SignatureTestUtility.GetFingerprint(repoCertificate, hashAlgorithmName);

                var signedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                    repoCertificate,
                    nupkg,
                    dir,
                    v3ServiceIndex: new Uri("https://v3serviceIndex.test/api/index.json"));

                var allowList = new List<CertificateHashAllowListEntry>()
                {
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, fingerprint, hashAlgorithmName, owners: new List<string>() { "owner1", "owner2", "owner3" })
                };

                var signedPackageVerifierSettings = SignedPackageVerifierSettings.GetRequireModeDefaultPolicy(TestEnvironmentVariableReader.EmptyInstance);
                var signedPackageVerifier = new PackageSignatureVerifier(new ISignatureVerificationProvider[]
                {
                    new AllowListVerificationProvider(allowList, requireNonEmptyAllowList: true)
                });

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await signedPackageVerifier.VerifySignaturesAsync(packageReader, signedPackageVerifierSettings, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var resultsWithWarnings = result.Results.Where(r => r.GetWarningIssues().Any());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());
                    var totalWarningIssues = resultsWithWarnings.SelectMany(r => r.GetWarningIssues());

                    // Assert
                    result.IsValid.Should().BeFalse();
                    resultsWithErrors.Count().Should().Be(1);
                    resultsWithWarnings.Count().Should().Be(0);
                    totalErrorIssues.Count().Should().Be(1);
                    totalErrorIssues.First().Message.Should().Be(_noMatchInAllowList);
                    totalWarningIssues.Count().Should().Be(0);
                }
            }
        }

        [CIOnlyFact]
        public async Task GetTrustResultAsync_RepositoryCountersignedPackage_PackageSignedWithCertFromAllowList_WithOwnerInOwnersList_RequireMode_SuccessAsync()
        {
            var nupkg = new SimpleTestPackageContext();

            // Arrange
            using (var dir = TestDirectory.Create())
            using (var primaryCertificate = new X509Certificate2(_testFixture.TrustedTestCertificate.Source.Cert))
            using (var counterCertificate = new X509Certificate2(_trustedRepoTestCert.Source.Cert))
            {
                var hashAlgorithmName = HashAlgorithmName.SHA256;
                var fingerprint = SignatureTestUtility.GetFingerprint(counterCertificate, hashAlgorithmName);

                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(primaryCertificate, nupkg, dir);
                var countersignedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                    counterCertificate,
                    signedPackagePath,
                    dir,
                    v3ServiceIndex: new Uri("https://v3serviceIndex.test/api/index.json"),
                    timestampService: null,
                    packageOwners: new List<string>() { "owner1", "owner4" });

                var allowList = new List<CertificateHashAllowListEntry>()
                {
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, fingerprint, hashAlgorithmName, owners: new List<string>() { "owner1", "owner2", "owner3" })
                };

                var signedPackageVerifierSettings = SignedPackageVerifierSettings.GetRequireModeDefaultPolicy(TestEnvironmentVariableReader.EmptyInstance);
                var signedPackageVerifier = new PackageSignatureVerifier(new ISignatureVerificationProvider[]
                {
                    new AllowListVerificationProvider(allowList, requireNonEmptyAllowList: true)
                });

                using (var packageReader = new PackageArchiveReader(countersignedPackagePath))
                {
                    // Act
                    var result = await signedPackageVerifier.VerifySignaturesAsync(packageReader, signedPackageVerifierSettings, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var resultsWithWarnings = result.Results.Where(r => r.GetWarningIssues().Any());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());
                    var totalWarningIssues = resultsWithWarnings.SelectMany(r => r.GetWarningIssues());

                    // Assert
                    result.IsValid.Should().BeTrue();
                    resultsWithErrors.Count().Should().Be(0);
                    resultsWithWarnings.Count().Should().Be(0);
                    totalErrorIssues.Count().Should().Be(0);
                    totalWarningIssues.Count().Should().Be(0);
                }
            }
        }

        [CIOnlyFact]
        public async Task GetTrustResultAsync_RepositoryCountersignedPackage_PackageSignedWithCertFromAllowList_WithOwnerNotInOwnersList_RequireMode_ErrorAsync()
        {
            var nupkg = new SimpleTestPackageContext();

            // Arrange
            using (var dir = TestDirectory.Create())
            using (var primaryCertificate = new X509Certificate2(_testFixture.TrustedTestCertificate.Source.Cert))
            using (var counterCertificate = new X509Certificate2(_trustedRepoTestCert.Source.Cert))
            {
                var hashAlgorithmName = HashAlgorithmName.SHA256;
                var fingerprint = SignatureTestUtility.GetFingerprint(counterCertificate, hashAlgorithmName);

                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(primaryCertificate, nupkg, dir);
                var countersignedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                    counterCertificate,
                    signedPackagePath,
                    dir,
                    v3ServiceIndex: new Uri("https://v3serviceIndex.test/api/index.json"),
                    timestampService: null,
                    packageOwners: new List<string>() { "owner4", "owner5" });

                var allowList = new List<CertificateHashAllowListEntry>()
                {
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, fingerprint, hashAlgorithmName, owners: new List<string>() { "owner1", "owner2", "owner3" })
                };

                var signedPackageVerifierSettings = SignedPackageVerifierSettings.GetRequireModeDefaultPolicy(TestEnvironmentVariableReader.EmptyInstance);
                var signedPackageVerifier = new PackageSignatureVerifier(new ISignatureVerificationProvider[]
                {
                    new AllowListVerificationProvider(allowList, requireNonEmptyAllowList: true)
                });

                using (var packageReader = new PackageArchiveReader(countersignedPackagePath))
                {
                    // Act
                    var result = await signedPackageVerifier.VerifySignaturesAsync(packageReader, signedPackageVerifierSettings, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var resultsWithWarnings = result.Results.Where(r => r.GetWarningIssues().Any());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());
                    var totalWarningIssues = resultsWithWarnings.SelectMany(r => r.GetWarningIssues());

                    // Assert
                    result.IsValid.Should().BeFalse();
                    resultsWithErrors.Count().Should().Be(1);
                    resultsWithWarnings.Count().Should().Be(0);
                    totalErrorIssues.Count().Should().Be(1);
                    totalErrorIssues.First().Message.Should().Be(_noMatchInAllowList);
                    totalWarningIssues.Count().Should().Be(0);
                }
            }
        }

        [CIOnlyFact]
        public async Task GetTrustResultAsync_RepositoryCountersignedPackage_PackageSignedWithCertFromAllowList_WithOwnerNotInOwnersList_AuthorInList_RequireMode_SuccessAsync()
        {
            var nupkg = new SimpleTestPackageContext();

            // Arrange
            using (var dir = TestDirectory.Create())
            using (var primaryCertificate = new X509Certificate2(_testFixture.TrustedTestCertificate.Source.Cert))
            using (var counterCertificate = new X509Certificate2(_trustedRepoTestCert.Source.Cert))
            {
                var hashAlgorithmName = HashAlgorithmName.SHA256;
                var authorFingerprint = SignatureTestUtility.GetFingerprint(primaryCertificate, hashAlgorithmName);
                var repositoryFingerprint = SignatureTestUtility.GetFingerprint(counterCertificate, hashAlgorithmName);

                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(primaryCertificate, nupkg, dir);
                var countersignedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                    counterCertificate,
                    signedPackagePath,
                    dir,
                    v3ServiceIndex: new Uri("https://v3serviceIndex.test/api/index.json"),
                    timestampService: null,
                    packageOwners: new List<string>() { "owner4", "owner5" });

                var allowList = new List<CertificateHashAllowListEntry>()
                {
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, repositoryFingerprint, hashAlgorithmName, owners: new List<string>() { "owner1", "owner2", "owner3" }),
                    new TrustedSignerAllowListEntry(VerificationTarget.Author, SignaturePlacement.PrimarySignature, authorFingerprint, hashAlgorithmName)
                };

                var signedPackageVerifierSettings = SignedPackageVerifierSettings.GetRequireModeDefaultPolicy(TestEnvironmentVariableReader.EmptyInstance);
                var signedPackageVerifier = new PackageSignatureVerifier(new ISignatureVerificationProvider[]
                {
                    new AllowListVerificationProvider(allowList, requireNonEmptyAllowList: true)
                });

                using (var packageReader = new PackageArchiveReader(countersignedPackagePath))
                {
                    // Act
                    var result = await signedPackageVerifier.VerifySignaturesAsync(packageReader, signedPackageVerifierSettings, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var resultsWithWarnings = result.Results.Where(r => r.GetWarningIssues().Any());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());
                    var totalWarningIssues = resultsWithWarnings.SelectMany(r => r.GetWarningIssues());

                    // Assert
                    result.IsValid.Should().BeTrue();
                    resultsWithErrors.Count().Should().Be(0);
                    resultsWithWarnings.Count().Should().Be(0);
                    totalErrorIssues.Count().Should().Be(0);
                    totalWarningIssues.Count().Should().Be(0);
                }
            }
        }

        [CIOnlyFact]
        public async Task GetTrustResultAsync_RepositoryCountersignedPackage_PackageSignedWithCertFromAllowList_WithNoOwnersInPackage_RequireMode_ErrorAsync()
        {
            var nupkg = new SimpleTestPackageContext();

            // Arrange
            using (var dir = TestDirectory.Create())
            using (var primaryCertificate = new X509Certificate2(_testFixture.TrustedTestCertificate.Source.Cert))
            using (var counterCertificate = new X509Certificate2(_trustedRepoTestCert.Source.Cert))
            {
                var hashAlgorithmName = HashAlgorithmName.SHA256;
                var fingerprint = SignatureTestUtility.GetFingerprint(counterCertificate, hashAlgorithmName);

                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(primaryCertificate, nupkg, dir);
                var countersignedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                    counterCertificate,
                    signedPackagePath,
                    dir,
                    v3ServiceIndex: new Uri("https://v3serviceIndex.test/api/index.json"));

                var allowList = new List<CertificateHashAllowListEntry>()
                {
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, fingerprint, hashAlgorithmName, owners: new List<string>() { "owner1", "owner2", "owner3" })
                };

                var signedPackageVerifierSettings = SignedPackageVerifierSettings.GetRequireModeDefaultPolicy(TestEnvironmentVariableReader.EmptyInstance);
                var signedPackageVerifier = new PackageSignatureVerifier(new ISignatureVerificationProvider[]
                {
                    new AllowListVerificationProvider(allowList, requireNonEmptyAllowList: true)
                });

                using (var packageReader = new PackageArchiveReader(countersignedPackagePath))
                {
                    // Act
                    var result = await signedPackageVerifier.VerifySignaturesAsync(packageReader, signedPackageVerifierSettings, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var resultsWithWarnings = result.Results.Where(r => r.GetWarningIssues().Any());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());
                    var totalWarningIssues = resultsWithWarnings.SelectMany(r => r.GetWarningIssues());

                    // Assert
                    result.IsValid.Should().BeFalse();
                    resultsWithErrors.Count().Should().Be(1);
                    resultsWithWarnings.Count().Should().Be(0);
                    totalErrorIssues.Count().Should().Be(1);
                    totalErrorIssues.First().Message.Should().Be(_noMatchInAllowList);
                    totalWarningIssues.Count().Should().Be(0);
                }
            }
        }


        [CIOnlyFact]
        public async Task GetTrustResultAsync_RepositoryPrimarySignedPackage_PackageSignedWithCertFromAllowList_WithEmptyOwnersList_RequireMode_SuccessAsync()
        {
            var nupkg = new SimpleTestPackageContext();

            // Arrange
            using (var dir = TestDirectory.Create())
            using (var repoCertificate = new X509Certificate2(_trustedRepoTestCert.Source.Cert))
            {
                var hashAlgorithmName = HashAlgorithmName.SHA256;
                var fingerprint = SignatureTestUtility.GetFingerprint(repoCertificate, hashAlgorithmName);

                var signedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                    repoCertificate,
                    nupkg,
                    dir,
                    v3ServiceIndex: new Uri("https://v3serviceIndex.test/api/index.json"),
                    timestampService: null,
                    packageOwners: new List<string>() { "owner1", "owner4" });

                var allowList = new List<CertificateHashAllowListEntry>()
                {
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, fingerprint, hashAlgorithmName, owners: new List<string>())
                };

                var signedPackageVerifierSettings = SignedPackageVerifierSettings.GetRequireModeDefaultPolicy(TestEnvironmentVariableReader.EmptyInstance);
                var signedPackageVerifier = new PackageSignatureVerifier(new ISignatureVerificationProvider[]
                {
                    new AllowListVerificationProvider(allowList, requireNonEmptyAllowList: true)
                });

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await signedPackageVerifier.VerifySignaturesAsync(packageReader, signedPackageVerifierSettings, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var resultsWithWarnings = result.Results.Where(r => r.GetWarningIssues().Any());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());
                    var totalWarningIssues = resultsWithWarnings.SelectMany(r => r.GetWarningIssues());

                    // Assert
                    result.IsValid.Should().BeTrue();
                    resultsWithErrors.Count().Should().Be(0);
                    resultsWithWarnings.Count().Should().Be(0);
                    totalErrorIssues.Count().Should().Be(0);
                    totalWarningIssues.Count().Should().Be(0);
                }
            }
        }

        [CIOnlyFact]
        public async Task GetTrustResultAsync_RepositoryPrimarySignedPackage_PackageSignedWithCertFromAllowList__WithNullOwnersList_RequireMode_SuccessAsync()
        {
            var nupkg = new SimpleTestPackageContext();

            // Arrange
            using (var dir = TestDirectory.Create())
            using (var repoCertificate = new X509Certificate2(_trustedRepoTestCert.Source.Cert))
            {
                var hashAlgorithmName = HashAlgorithmName.SHA256;
                var fingerprint = SignatureTestUtility.GetFingerprint(repoCertificate, hashAlgorithmName);

                var signedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(
                    repoCertificate,
                    nupkg,
                    dir,
                    v3ServiceIndex: new Uri("https://v3serviceIndex.test/api/index.json"),
                    timestampService: null,
                    packageOwners: new List<string>() { "owner1", "owner4" });

                var allowList = new List<CertificateHashAllowListEntry>()
                {
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, fingerprint, hashAlgorithmName, owners: null)
                };

                var signedPackageVerifierSettings = SignedPackageVerifierSettings.GetRequireModeDefaultPolicy(TestEnvironmentVariableReader.EmptyInstance);
                var signedPackageVerifier = new PackageSignatureVerifier(new ISignatureVerificationProvider[]
                {
                    new AllowListVerificationProvider(allowList, requireNonEmptyAllowList: true)
                });

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await signedPackageVerifier.VerifySignaturesAsync(packageReader, signedPackageVerifierSettings, CancellationToken.None);
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var resultsWithWarnings = result.Results.Where(r => r.GetWarningIssues().Any());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());
                    var totalWarningIssues = resultsWithWarnings.SelectMany(r => r.GetWarningIssues());

                    // Assert
                    result.IsValid.Should().BeTrue();
                    resultsWithErrors.Count().Should().Be(0);
                    resultsWithWarnings.Count().Should().Be(0);
                    totalErrorIssues.Count().Should().Be(0);
                    totalWarningIssues.Count().Should().Be(0);
                }
            }
        }

        private static SignedPackageVerifierSettings GetSettings(bool allowUntrusted)
        {
            return new SignedPackageVerifierSettings(
                allowUnsigned: false,
                allowIllegal: true,
                allowUntrusted: allowUntrusted,
                allowIgnoreTimestamp: true,
                allowMultipleTimestamps: true,
                allowNoTimestamp: true,
                allowUnknownRevocation: true,
                reportUnknownRevocation: false,
                verificationTarget: VerificationTarget.All,
                signaturePlacement: SignaturePlacement.Any,
                repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.IfExistsAndIsNecessary,
                revocationMode: RevocationMode.Online);
        }

        public static IEnumerable<object[]> EmptyNullAndRequiredListCombinations()
        {
            // AllowUntrusted | Allow List | AllowNoAllowList | Valid | ResultsWithErrorCount | TotalErrorIssues | ErrorLogCodes and Messages
            // NoAllowUntrusted_AllowListRequired_NoAllowList_Error
            yield return new object[]
            { GetSettings(false), null, false, false, 1, 1, new object[]{  new object[]{ NuGetLogCode.NU3034, _noAllowList } } };
            // NoAllowUntrusted_AllowListRequired_EmptyAllowList_Error
            yield return new object[]
            { GetSettings(false), new List<CertificateHashAllowListEntry>(), false, false, 1, 1, new object[]{  new object[]{ NuGetLogCode.NU3034, _noAllowList } } };
            // NoAllowUntrusted_AllowListNotRequired_NoAllowList_Succeess
            yield return new object[]
            { GetSettings(false), null, true, true, 0, 0, null };
            // NoAllowUntrusted_AllowListNotRequired_EmptyAllowList_Succeess
            yield return new object[]
            { GetSettings(false), new List<CertificateHashAllowListEntry>(), true, true, 0, 0, null };
        }
    }
}
#endif
