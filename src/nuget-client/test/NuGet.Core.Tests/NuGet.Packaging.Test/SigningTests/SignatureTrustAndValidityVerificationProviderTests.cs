// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using Microsoft.Internal.NuGet.Testing.SignedPackages.Asn1;
using Moq;
using NuGet.Common;
using NuGet.Packaging.Signing;
using Test.Utility;
using Test.Utility.Signing;
using Xunit;

namespace NuGet.Packaging.Test
{
    [Collection(SigningTestsCollection.Name)]
    public class SignatureTrustAndValidityVerificationProviderTests
    {
        private static readonly Lazy<PrimarySignature> _signature = new Lazy<PrimarySignature>(
            () => PrimarySignature.Load(SigningTestUtility.GetResourceBytes(".signature.p7s")));
        private readonly SignatureTrustAndValidityVerificationProvider _provider;
        private readonly CertificatesFixture _fixture;

        public SignatureTrustAndValidityVerificationProviderTests(CertificatesFixture fixture)
        {
            _fixture = fixture;
            _provider = new SignatureTrustAndValidityVerificationProvider();
        }

        [Fact]
        public async Task GetTrustResultAsync_WhenPackageIsNull_Throws()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => _provider.GetTrustResultAsync(
                    package: null!,
                    signature: _signature.Value,
                    settings: SignedPackageVerifierSettings.GetDefault(TestEnvironmentVariableReader.EmptyInstance),
                    token: CancellationToken.None));

            Assert.Equal("package", exception.ParamName);
        }

        [Fact]
        public async Task GetTrustResultAsync_WhenSignatureIsNull_Throws()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => _provider.GetTrustResultAsync(
                    package: Mock.Of<ISignedPackageReader>(),
                    signature: null!,
                    settings: SignedPackageVerifierSettings.GetDefault(TestEnvironmentVariableReader.EmptyInstance),
                    token: CancellationToken.None));

            Assert.Equal("signature", exception.ParamName);
        }

        [Fact]
        public async Task GetTrustResultAsync_WhenSettingsIsNull_Throws()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => _provider.GetTrustResultAsync(
                    package: Mock.Of<ISignedPackageReader>(),
                    signature: _signature.Value,
                    settings: null!,
                    token: CancellationToken.None));

            Assert.Equal("settings", exception.ParamName);
        }

        [Fact]
        public async Task GetTrustResultAsync_WhenTokenIsCanceled_Throws()
        {
            await Assert.ThrowsAsync<OperationCanceledException>(
                () => _provider.GetTrustResultAsync(
                    package: Mock.Of<ISignedPackageReader>(),
                    signature: _signature.Value,
                    settings: SignedPackageVerifierSettings.GetDefault(TestEnvironmentVariableReader.EmptyInstance),
                    token: new CancellationToken(canceled: true)));
        }

        [Theory]
        [InlineData(X509ChainStatusFlags.NoError, X509ChainStatusFlags.NoError, RevocationMode.Online)]
        [InlineData(X509ChainStatusFlags.NoError, X509ChainStatusFlags.NoError, RevocationMode.Offline)]
        [InlineData(X509ChainStatusFlags.RevocationStatusUnknown, X509ChainStatusFlags.NoError, RevocationMode.Online)]
        [InlineData(X509ChainStatusFlags.OfflineRevocation, X509ChainStatusFlags.NoError, RevocationMode.Online)]
        [InlineData(X509ChainStatusFlags.NoError, X509ChainStatusFlags.RevocationStatusUnknown, RevocationMode.Online)]
        [InlineData(X509ChainStatusFlags.NoError, X509ChainStatusFlags.OfflineRevocation, RevocationMode.Online)]
        [InlineData(X509ChainStatusFlags.OfflineRevocation | X509ChainStatusFlags.RevocationStatusUnknown,
            X509ChainStatusFlags.OfflineRevocation | X509ChainStatusFlags.RevocationStatusUnknown, RevocationMode.Online)]
        [InlineData(X509ChainStatusFlags.OfflineRevocation | X509ChainStatusFlags.RevocationStatusUnknown,
            X509ChainStatusFlags.OfflineRevocation | X509ChainStatusFlags.RevocationStatusUnknown, RevocationMode.Offline)]
        public async Task VerifySignaturesAsync_WithAllowedUnknownRevocation_RecordsObservationsWithoutChangingTrust(
            X509ChainStatusFlags signerFlags,
            X509ChainStatusFlags timestampFlags,
            RevocationMode revocationMode)
        {
            Mock<IEnvironmentVariableReader> environment = new();
            environment.Setup(reader => reader.GetEnvironmentVariable("NUGET_CERT_REVOCATION_MODE")).Returns(revocationMode.ToString());
            SignedPackageVerifierSettings settings = SignedPackageVerifierSettings.GetDefault(environment.Object);
            (VerifySignaturesResult result, TelemetryEvent telemetryEvent) = await VerifyWithChainsAsync(signerFlags, timestampFlags, settings);

            Assert.True(result.IsValid);
            SignedPackageVerificationResult trustResult = Assert.IsType<SignedPackageVerificationResult>(Assert.Single(result.Results));
            SignatureVerificationSummary summary = Assert.Single(trustResult.SignatureSummaries);
            Assert.Equal(signerFlags, summary.SignerChainStatusFlags);
            Assert.Equal(timestampFlags, summary.TimestampChainStatusFlags);
            Assert.Equal(SignatureChainEvaluationStatus.Evaluated, summary.SignerChainEvaluationStatus);
            Assert.Equal(SignatureChainEvaluationStatus.Evaluated, summary.TimestampChainEvaluationStatus);
            Assert.DoesNotContain(trustResult.Issues, issue => issue.Level == LogLevel.Warning || issue.Level == LogLevel.Error);
            Assert.Equal(NuGetOperationStatus.Succeeded, telemetryEvent["Status"]);
            Assert.Equal(revocationMode.ToString(), telemetryEvent["RevocationMode"]);
            Assert.Equal((int)signerFlags, telemetryEvent["SignerRevocationStatusFlags"]);
            Assert.Equal((int)timestampFlags, telemetryEvent["TimestampRevocationStatusFlags"]);
            Assert.Equal((signerFlags | timestampFlags) != X509ChainStatusFlags.NoError, telemetryEvent["RevocationInformationUnavailable"]);
            Assert.Equal("Evaluated", telemetryEvent["RevocationEvaluationStatus"]);
            Assert.Equal(0, telemetryEvent["VerificationRejectionReasons"]);
            Assert.Equal(0, telemetryEvent["VerificationRevocationRejectionFlags"]);
        }

        [Theory]
        [InlineData(X509ChainStatusFlags.RevocationStatusUnknown, X509ChainStatusFlags.NoError, false, false, (int)SignatureVerificationRejectionReasons.RevocationUnavailable)]
        [InlineData(X509ChainStatusFlags.NoError, X509ChainStatusFlags.OfflineRevocation, false, false, (int)SignatureVerificationRejectionReasons.RevocationUnavailable)]
        [InlineData(X509ChainStatusFlags.RevocationStatusUnknown | X509ChainStatusFlags.UntrustedRoot, X509ChainStatusFlags.NoError, false, false,
            (int)(SignatureVerificationRejectionReasons.RevocationUnavailable | SignatureVerificationRejectionReasons.Other))]
        [InlineData(X509ChainStatusFlags.NoError, X509ChainStatusFlags.RevocationStatusUnknown | X509ChainStatusFlags.UntrustedRoot, true, false, (int)SignatureVerificationRejectionReasons.Other)]
        [InlineData(X509ChainStatusFlags.NoError, X509ChainStatusFlags.RevocationStatusUnknown | X509ChainStatusFlags.UntrustedRoot, false, true, (int)SignatureVerificationRejectionReasons.None)]
        [InlineData(X509ChainStatusFlags.PartialChain, X509ChainStatusFlags.NoError, true, false, (int)SignatureVerificationRejectionReasons.Other)]
        [InlineData(X509ChainStatusFlags.UntrustedRoot, X509ChainStatusFlags.NoError, true, false, (int)SignatureVerificationRejectionReasons.Other)]
        [InlineData(X509ChainStatusFlags.Revoked, X509ChainStatusFlags.NoError, true, false, (int)SignatureVerificationRejectionReasons.Other)]
        [InlineData(X509ChainStatusFlags.NoError, X509ChainStatusFlags.Revoked, true, false, (int)SignatureVerificationRejectionReasons.Other)]
        [InlineData(X509ChainStatusFlags.NoError,
            X509ChainStatusFlags.Revoked | X509ChainStatusFlags.RevocationStatusUnknown | X509ChainStatusFlags.OfflineRevocation,
            false, false, (int)SignatureVerificationRejectionReasons.Other)]
        [InlineData(X509ChainStatusFlags.NoError,
            X509ChainStatusFlags.Revoked | X509ChainStatusFlags.RevocationStatusUnknown | X509ChainStatusFlags.OfflineRevocation,
            false, true, (int)SignatureVerificationRejectionReasons.None)]
        public async Task VerifySignaturesAsync_RevocationTelemetry_PreservesFullFlagsAndEffectiveRejection(
            X509ChainStatusFlags signerFlags, X509ChainStatusFlags timestampFlags, bool allowUnknownRevocation,
            bool allowIgnoreTimestamp, int expectedReasons)
        {
            SignedPackageVerifierSettings settings = new(
                allowUnsigned: true, allowIllegal: false, allowUntrusted: allowIgnoreTimestamp,
                allowIgnoreTimestamp, allowMultipleTimestamps: true, allowNoTimestamp: true,
                allowUnknownRevocation, reportUnknownRevocation: false,
                verificationTarget: VerificationTarget.All, signaturePlacement: SignaturePlacement.PrimarySignature,
                repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.Never, revocationMode: RevocationMode.Online);
            (VerifySignaturesResult result, TelemetryEvent telemetryEvent) = await VerifyWithChainsAsync(signerFlags, timestampFlags, settings);

            Assert.Equal(expectedReasons == 0, result.IsValid);
            SignedPackageVerificationResult trustResult = Assert.IsType<SignedPackageVerificationResult>(Assert.Single(result.Results));
            Assert.Equal((SignatureVerificationRejectionReasons)expectedReasons, trustResult.RejectionReasons);
            Assert.Equal(expectedReasons, telemetryEvent["VerificationRejectionReasons"]);
            Assert.Equal((int)signerFlags, telemetryEvent["SignerChainStatusFlags"]);
            Assert.Equal((int)timestampFlags, telemetryEvent["TimestampChainStatusFlags"]);
            bool indeterminate = signerFlags == X509ChainStatusFlags.PartialChain || signerFlags == X509ChainStatusFlags.UntrustedRoot;
            Assert.Equal(indeterminate ? "Indeterminate" : "Evaluated", telemetryEvent["RevocationEvaluationStatus"]);
            Assert.Equal(((signerFlags | timestampFlags) & CertificateChainUtility.RevocationUnavailableFlags) != 0,
                telemetryEvent["RevocationInformationUnavailable"]);
            List<TelemetryEvent> chains = Assert.IsType<List<TelemetryEvent>>(telemetryEvent.ComplexData["SignatureChains"]);
            Assert.Equal(2, chains.Count);
            Assert.Equal(SignaturePlacement.PrimarySignature.ToString(), chains[0]["SignaturePlacement"]);
            Assert.Equal((int)signerFlags, chains[0]["ChainStatusFlags"]);
            Assert.Equal(CertificateType.Timestamp.ToString(), chains[1]["CertificateType"]);
            Assert.Equal((int)timestampFlags, chains[1]["ChainStatusFlags"]);
            SignatureVerificationSummary summary = Assert.Single(trustResult.SignatureSummaries);
            Assert.Equal((int)summary.SignerRejectionReasons, chains[0]["RejectionReasons"]);
            Assert.Equal((int)summary.TimestampRejectionReasons, chains[1]["RejectionReasons"]);
            X509ChainStatusFlags expectedRevocationRejectionFlags = X509ChainStatusFlags.NoError;
            if (summary.SignerRejectionReasons.HasFlag(SignatureVerificationRejectionReasons.RevocationUnavailable))
            {
                expectedRevocationRejectionFlags |= signerFlags & CertificateChainUtility.RevocationUnavailableFlags;
            }
            if (summary.TimestampRejectionReasons.HasFlag(SignatureVerificationRejectionReasons.RevocationUnavailable))
            {
                expectedRevocationRejectionFlags |= timestampFlags & CertificateChainUtility.RevocationUnavailableFlags;
            }
            Assert.Equal((int)expectedRevocationRejectionFlags, telemetryEvent["VerificationRevocationRejectionFlags"]);
        }

        [Theory]
        [InlineData(X509ChainStatusFlags.PartialChain | X509ChainStatusFlags.NotValidForUsage | X509ChainStatusFlags.RevocationStatusUnknown,
            X509ChainStatusFlags.NoError, false, true, false,
            X509ChainStatusFlags.PartialChain | X509ChainStatusFlags.NotValidForUsage, X509ChainStatusFlags.NoError)]
        [InlineData(X509ChainStatusFlags.Revoked | X509ChainStatusFlags.NotValidForUsage | X509ChainStatusFlags.RevocationStatusUnknown,
            X509ChainStatusFlags.NoError, false, false, false,
            X509ChainStatusFlags.Revoked | X509ChainStatusFlags.NotValidForUsage, X509ChainStatusFlags.NoError)]
        [InlineData(X509ChainStatusFlags.UntrustedRoot | X509ChainStatusFlags.RevocationStatusUnknown,
            X509ChainStatusFlags.NoError, true, true, false, X509ChainStatusFlags.NoError, X509ChainStatusFlags.NoError)]
        [InlineData(X509ChainStatusFlags.NoError,
            X509ChainStatusFlags.UntrustedRoot | X509ChainStatusFlags.RevocationStatusUnknown, false, false, false,
            X509ChainStatusFlags.UntrustedRoot | X509ChainStatusFlags.RevocationStatusUnknown,
            X509ChainStatusFlags.UntrustedRoot | X509ChainStatusFlags.RevocationStatusUnknown)]
        [InlineData(X509ChainStatusFlags.NoError,
            X509ChainStatusFlags.UntrustedRoot | X509ChainStatusFlags.RevocationStatusUnknown, false, true, false,
            X509ChainStatusFlags.UntrustedRoot,
            X509ChainStatusFlags.UntrustedRoot | X509ChainStatusFlags.RevocationStatusUnknown)]
        [InlineData(X509ChainStatusFlags.NoError,
            X509ChainStatusFlags.Revoked | X509ChainStatusFlags.RevocationStatusUnknown, false, false, false,
            X509ChainStatusFlags.Revoked, X509ChainStatusFlags.Revoked | X509ChainStatusFlags.RevocationStatusUnknown)]
        [InlineData(X509ChainStatusFlags.RevocationStatusUnknown,
            X509ChainStatusFlags.Revoked | X509ChainStatusFlags.RevocationStatusUnknown, true, false, true,
            X509ChainStatusFlags.RevocationStatusUnknown, X509ChainStatusFlags.NoError)]
        [InlineData(X509ChainStatusFlags.NoError, X509ChainStatusFlags.RevocationStatusUnknown,
            true, false, true, X509ChainStatusFlags.NoError, X509ChainStatusFlags.RevocationStatusUnknown)]
        [InlineData(X509ChainStatusFlags.NotTimeValid, X509ChainStatusFlags.NoError,
            false, false, false, X509ChainStatusFlags.NoError, X509ChainStatusFlags.NoError)]
        public async Task VerifySignaturesAsync_ChainTelemetry_DistinguishesRelevantObservationsAndEnforcedFlags(
            X509ChainStatusFlags signerFlags, X509ChainStatusFlags timestampFlags,
            bool allowUntrusted, bool allowUnknownRevocation, bool allowIgnoreTimestamp,
            X509ChainStatusFlags expectedRejectionFlags, X509ChainStatusFlags expectedTimestampDecisionFlags)
        {
            SignedPackageVerifierSettings settings = new(
                allowUnsigned: true, allowIllegal: true, allowUntrusted, allowIgnoreTimestamp,
                allowMultipleTimestamps: true, allowNoTimestamp: true, allowUnknownRevocation,
                reportUnknownRevocation: true, verificationTarget: VerificationTarget.All,
                signaturePlacement: SignaturePlacement.PrimarySignature,
                repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.Never,
                revocationMode: RevocationMode.Online);
            (VerifySignaturesResult result, TelemetryEvent telemetryEvent) = await VerifyWithChainsAsync(
                signerFlags, timestampFlags, settings);

            Assert.Equal(expectedRejectionFlags == X509ChainStatusFlags.NoError, result.IsValid);
            Assert.Equal((int)(signerFlags | expectedTimestampDecisionFlags),
                telemetryEvent["VerificationDecisionRelevantChainStatusFlags"]);
            Assert.Equal((int)expectedRejectionFlags, telemetryEvent["VerificationChainRejectionFlags"]);
            Assert.Equal((int)expectedRejectionFlags, telemetryEvent["AcceptanceChainRejectionFlags"]);
            Assert.Equal(0, telemetryEvent["WarningPromotionChainStatusFlags"]);
            bool expectedNonX509Rejection = !allowUntrusted && timestampFlags != X509ChainStatusFlags.NoError && !result.IsValid;
            Assert.Equal(expectedNonX509Rejection, telemetryEvent["VerificationHasNonX509Rejection"]);
            List<TelemetryEvent> chains = Assert.IsType<List<TelemetryEvent>>(telemetryEvent.ComplexData["SignatureChains"]);
            Assert.Equal((int)signerFlags, chains[0]["DecisionRelevantChainStatusFlags"]);
            Assert.Equal((int)expectedTimestampDecisionFlags, chains[1]["DecisionRelevantChainStatusFlags"]);
            Assert.Equal((int)expectedRejectionFlags,
                Assert.IsType<int>(chains[0]["RejectionChainStatusFlags"]) |
                Assert.IsType<int>(chains[1]["RejectionChainStatusFlags"]));
            if (signerFlags.HasFlag(X509ChainStatusFlags.NotValidForUsage))
            {
                Assert.Contains(result.Results.SelectMany(value => value.Issues), issue =>
                    issue.Level == LogLevel.Warning && issue.ChainStatusFlags == X509ChainStatusFlags.NotValidForUsage);
            }
        }

        [Theory]
        [InlineData(X509ChainStatusFlags.RevocationStatusUnknown)]
        [InlineData(X509ChainStatusFlags.OfflineRevocation)]
        [InlineData(X509ChainStatusFlags.RevocationStatusUnknown | X509ChainStatusFlags.OfflineRevocation)]
        public async Task VerifySignaturesAsync_WithRevokedSigner_PreservesUnavailableFlagsWithoutAttributingTheirRejection(
            X509ChainStatusFlags unavailableFlags)
        {
            SignedPackageVerifierSettings settings = new(
                allowUnsigned: true, allowIllegal: false, allowUntrusted: false, allowIgnoreTimestamp: false,
                allowMultipleTimestamps: true, allowNoTimestamp: true, allowUnknownRevocation: false,
                reportUnknownRevocation: true, verificationTarget: VerificationTarget.All,
                signaturePlacement: SignaturePlacement.PrimarySignature,
                repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.Never,
                revocationMode: RevocationMode.Online);
            X509ChainStatusFlags signerFlags = X509ChainStatusFlags.Revoked | unavailableFlags;
            (VerifySignaturesResult result, TelemetryEvent telemetryEvent) = await VerifyWithChainsAsync(
                signerFlags, X509ChainStatusFlags.NoError, settings);

            Assert.False(result.IsValid);
            SignedPackageVerificationResult trustResult =
                Assert.IsType<SignedPackageVerificationResult>(Assert.Single(result.Results));
            SignatureVerificationSummary summary = Assert.Single(trustResult.SignatureSummaries);
            Assert.Equal(SignatureVerificationStatus.Suspect, summary.Status);
            Assert.Equal(SignatureVerificationStatusFlags.CertificateRevoked, summary.Flags);
            Assert.Equal(SignatureVerificationRejectionReasons.Other, summary.SignerRejectionReasons);
            Assert.Equal(signerFlags, summary.SignerChainStatusFlags);
            Assert.Equal((int)signerFlags, telemetryEvent["SignerChainStatusFlags"]);
            Assert.Equal((int)unavailableFlags, telemetryEvent["SignerRevocationStatusFlags"]);
            Assert.Equal(true, telemetryEvent["RevocationInformationUnavailable"]);
            Assert.Equal((int)SignatureVerificationRejectionReasons.Other, telemetryEvent["VerificationRejectionReasons"]);
            Assert.Equal(0, telemetryEvent["VerificationRevocationRejectionFlags"]);
            Assert.Equal((int)X509ChainStatusFlags.Revoked, telemetryEvent["VerificationChainRejectionFlags"]);
            List<TelemetryEvent> chains =
                Assert.IsType<List<TelemetryEvent>>(telemetryEvent.ComplexData["SignatureChains"]);
            Assert.Equal((int)signerFlags, chains[0]["ChainStatusFlags"]);
        }

        [Fact]
        public async Task VerifySignaturesAsync_WithIgnoredRevokedTimestamp_AttributesUnavailableRevocationOnlyToSigner()
        {
            SignedPackageVerifierSettings settings = new(
                allowUnsigned: true, allowIllegal: false, allowUntrusted: true, allowIgnoreTimestamp: true,
                allowMultipleTimestamps: true, allowNoTimestamp: true, allowUnknownRevocation: false,
                reportUnknownRevocation: true, verificationTarget: VerificationTarget.All,
                signaturePlacement: SignaturePlacement.PrimarySignature,
                repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.Never,
                revocationMode: RevocationMode.Online);
            X509ChainStatusFlags timestampFlags = X509ChainStatusFlags.Revoked | X509ChainStatusFlags.OfflineRevocation;
            (VerifySignaturesResult result, TelemetryEvent telemetryEvent) = await VerifyWithChainsAsync(
                X509ChainStatusFlags.RevocationStatusUnknown, timestampFlags, settings);

            Assert.False(result.IsValid);
            SignedPackageVerificationResult trustResult =
                Assert.IsType<SignedPackageVerificationResult>(Assert.Single(result.Results));
            SignatureVerificationSummary summary = Assert.Single(trustResult.SignatureSummaries);
            Assert.Equal(SignatureVerificationStatusFlags.UnknownRevocation, summary.Flags);
            Assert.Equal(SignatureVerificationRejectionReasons.RevocationUnavailable, summary.SignerRejectionReasons);
            Assert.Equal(SignatureVerificationRejectionReasons.None, summary.TimestampRejectionReasons);
            Assert.Equal((int)timestampFlags, telemetryEvent["TimestampChainStatusFlags"]);
            Assert.Equal((int)SignatureVerificationRejectionReasons.RevocationUnavailable,
                telemetryEvent["VerificationRejectionReasons"]);
            Assert.Equal((int)X509ChainStatusFlags.RevocationStatusUnknown,
                telemetryEvent["VerificationRevocationRejectionFlags"]);
            List<TelemetryEvent> chains =
                Assert.IsType<List<TelemetryEvent>>(telemetryEvent.ComplexData["SignatureChains"]);
            Assert.Equal((int)timestampFlags, chains[1]["ChainStatusFlags"]);
            Assert.Equal(0, chains[1]["RejectionReasons"]);
            Assert.Equal(0, chains[1]["DecisionRelevantChainStatusFlags"]);
            Assert.Equal(0, chains[1]["RejectionChainStatusFlags"]);
            Assert.Equal((int)X509ChainStatusFlags.RevocationStatusUnknown, telemetryEvent["VerificationChainRejectionFlags"]);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task VerifySignaturesAsync_RevocationTelemetry_DoesNotTreatAnUnknownBuildAsAvailable(bool signerBuildFails)
        {
            SignedPackageVerifierSettings defaults = SignedPackageVerifierSettings.GetDefault(TestEnvironmentVariableReader.EmptyInstance);
            SignedPackageVerifierSettings settings = new(
                defaults.AllowUnsigned, defaults.AllowIllegal, defaults.AllowUntrusted,
                allowIgnoreTimestamp: false, defaults.AllowMultipleTimestamps, defaults.AllowNoTimestamp,
                defaults.AllowUnknownRevocation, defaults.ReportUnknownRevocation, defaults.VerificationTarget,
                defaults.SignaturePlacement, defaults.RepositoryCountersignatureVerificationBehavior, defaults.RevocationMode);
            (VerifySignaturesResult result, TelemetryEvent telemetryEvent) = await VerifyWithChainsAsync(
                X509ChainStatusFlags.NoError, X509ChainStatusFlags.NoError, settings,
                signerBuildSucceeded: !signerBuildFails, timestampBuildSucceeded: signerBuildFails);

            Assert.False(result.IsValid);
            Assert.Equal(false, telemetryEvent["RevocationInformationUnavailable"]);
            Assert.Equal("Indeterminate", telemetryEvent["RevocationEvaluationStatus"]);
            Assert.Equal((int)SignatureVerificationRejectionReasons.Unknown, telemetryEvent["VerificationRejectionReasons"]);
            Assert.Equal(false, telemetryEvent["VerificationHasNonX509Rejection"]);
        }

        [Fact]
        public async Task VerifySignaturesAsync_RevocationTelemetry_DistinguishesVerificationThatDidNotEvaluateChains()
        {
            SignedPackageVerifierSettings defaults = SignedPackageVerifierSettings.GetDefault(TestEnvironmentVariableReader.EmptyInstance);
            SignedPackageVerifierSettings settings = new(
                defaults.AllowUnsigned, defaults.AllowIllegal, defaults.AllowUntrusted, defaults.AllowIgnoreTimestamp,
                defaults.AllowMultipleTimestamps, defaults.AllowNoTimestamp, defaults.AllowUnknownRevocation,
                defaults.ReportUnknownRevocation, VerificationTarget.Repository, SignaturePlacement.PrimarySignature,
                SignatureVerificationBehavior.Never, defaults.RevocationMode);
            (VerifySignaturesResult result, TelemetryEvent telemetryEvent) = await VerifyWithChainsAsync(
                X509ChainStatusFlags.NoError, X509ChainStatusFlags.NoError, settings);

            Assert.False(result.IsValid);
            Assert.Equal("NotEvaluated", telemetryEvent["RevocationEvaluationStatus"]);
            Assert.Empty(Assert.IsType<List<TelemetryEvent>>(telemetryEvent.ComplexData["SignatureChains"]));
        }

        [Fact]
        public async Task VerifySignaturesAsync_RevocationTelemetry_DistinguishesAnAbsentTimestamp()
        {
            TestSignedCms cms = TestSignedCms.Decode(_signature.Value.SignedCms);
            cms.SignerInfos[0].RemoveUnsignedAttribute(TestOids.SignatureTimestampToken);
            cms.SignerInfos[0].RemoveUnsignedAttribute(TestOids.Countersignature);
            PrimarySignature signature = PrimarySignature.Load(cms.Encode());
            SignedPackageVerifierSettings settings = SignedPackageVerifierSettings.GetDefault(TestEnvironmentVariableReader.EmptyInstance);
            (VerifySignaturesResult result, TelemetryEvent telemetryEvent) = await VerifyWithChainsAsync(
                X509ChainStatusFlags.NoError, X509ChainStatusFlags.RevocationStatusUnknown, settings, signature: signature);

            Assert.True(result.IsValid);
            Assert.Equal("Evaluated", telemetryEvent["RevocationEvaluationStatus"]);
            Assert.Equal(0, telemetryEvent["TimestampChainStatusFlags"]);
            List<TelemetryEvent> chains = Assert.IsType<List<TelemetryEvent>>(telemetryEvent.ComplexData["SignatureChains"]);
            Assert.Equal("NotApplicable", chains[1]["EvaluationStatus"]);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task VerifySignaturesAsync_RevocationTelemetry_DoesNotAttributeRejectionAfterCountersignatureFallback(bool allowUnknownRevocation)
        {
            using X509Certificate2 repositoryCertificate = _fixture.GetDefaultCertificate();
            using RepositorySignPackageRequest request = new(
                repositoryCertificate, HashAlgorithmName.SHA256, HashAlgorithmName.SHA256,
                new Uri("https://v3serviceIndex.test/api/index.json"), packageOwners: null);
            TestSignedCms cms = TestSignedCms.Decode(_signature.Value.SignedCms);
            cms.SignerInfos[0].RemoveUnsignedAttribute(TestOids.Countersignature);
            PrimarySignature signature = await SignedArchiveTestUtility.RepositoryCountersignPrimarySignatureAsync(
                PrimarySignature.Load(cms.Encode()), request);
            SignedPackageVerifierSettings defaults = SignedPackageVerifierSettings.GetDefault(TestEnvironmentVariableReader.EmptyInstance);
            SignedPackageVerifierSettings settings = new(
                defaults.AllowUnsigned, defaults.AllowIllegal, allowUntrusted: false, defaults.AllowIgnoreTimestamp,
                defaults.AllowMultipleTimestamps, defaults.AllowNoTimestamp, allowUnknownRevocation,
                defaults.ReportUnknownRevocation, defaults.VerificationTarget, defaults.SignaturePlacement,
                defaults.RepositoryCountersignatureVerificationBehavior, defaults.RevocationMode);
            X509ChainStatusFlags primaryFlags = X509ChainStatusFlags.UntrustedRoot | X509ChainStatusFlags.RevocationStatusUnknown;
            (VerifySignaturesResult result, TelemetryEvent telemetryEvent) = await VerifyWithChainsAsync(
                primaryFlags, X509ChainStatusFlags.NoError, settings, signature: signature,
                countersignatureFlags: X509ChainStatusFlags.NoError);

            Assert.True(result.IsValid);
            SignedPackageVerificationResult trustResult = Assert.IsType<SignedPackageVerificationResult>(Assert.Single(result.Results));
            Assert.Equal(2, trustResult.SignatureSummaries.Count);
            Assert.Equal(0, telemetryEvent["VerificationRejectionReasons"]);
            Assert.Equal(0, telemetryEvent["VerificationRevocationRejectionFlags"]);
            Assert.Equal(true, telemetryEvent["RevocationInformationUnavailable"]);
            List<TelemetryEvent> chains = Assert.IsType<List<TelemetryEvent>>(telemetryEvent.ComplexData["SignatureChains"]);
            Assert.Equal(4, chains.Count);
            Assert.Equal(SignaturePlacement.PrimarySignature.ToString(), chains[0]["SignaturePlacement"]);
            Assert.Equal((int)primaryFlags, chains[0]["ChainStatusFlags"]);
            Assert.Equal(0, chains[0]["RejectionReasons"]);
            Assert.Equal(0, chains[0]["DecisionRelevantChainStatusFlags"]);
            Assert.Equal(0, chains[1]["DecisionRelevantChainStatusFlags"]);
            Assert.Equal(0, telemetryEvent["VerificationDecisionRelevantChainStatusFlags"]);
            Assert.Equal(0, telemetryEvent["VerificationChainRejectionFlags"]);
            Assert.Equal(SignaturePlacement.Countersignature.ToString(), chains[2]["SignaturePlacement"]);
            Assert.Equal(false, telemetryEvent["VerificationHasNonX509Rejection"]);
            Assert.All(trustResult.SignatureSummaries, summary =>
            {
                Assert.False(summary.SignerHasNonX509Rejection);
                Assert.False(summary.TimestampHasNonX509Rejection);
            });
            Assert.Equal(SignatureType.Repository.ToString(), chains[2]["SignatureType"]);
            Assert.Equal(0, chains[2]["ChainStatusFlags"]);
            Assert.Equal(0, chains[3]["ChainStatusFlags"]);
            Assert.Equal("NotApplicable", chains[3]["EvaluationStatus"]);
        }

        [Theory]
        [InlineData(false, false, X509ChainStatusFlags.RevocationStatusUnknown)]
        [InlineData(false, false, X509ChainStatusFlags.OfflineRevocation)]
        [InlineData(false, false, X509ChainStatusFlags.RevocationStatusUnknown | X509ChainStatusFlags.OfflineRevocation)]
        [InlineData(false, true, X509ChainStatusFlags.RevocationStatusUnknown)]
        [InlineData(true, false, X509ChainStatusFlags.RevocationStatusUnknown)]
        [InlineData(true, true, X509ChainStatusFlags.RevocationStatusUnknown)]
        public async Task VerifySignaturesAsync_WithUnusableCountersignatureTimestamp_PreservesPrimaryDecision(
            bool allowUntrusted, bool allowUnknownRevocation, X509ChainStatusFlags unavailableFlags)
        {
            using X509Certificate2 repositoryCertificate = _fixture.GetDefaultCertificate();
            using RepositorySignPackageRequest request = new(
                repositoryCertificate, HashAlgorithmName.SHA256, HashAlgorithmName.SHA256,
                new Uri("https://v3serviceIndex.test/api/index.json"), packageOwners: null);
            TestSignedCms cms = TestSignedCms.Decode(_signature.Value.SignedCms);
            cms.SignerInfos[0].RemoveUnsignedAttribute(TestOids.SignatureTimestampToken);
            cms.SignerInfos[0].RemoveUnsignedAttribute(TestOids.Countersignature);
            PrimarySignature signature = await SignedArchiveTestUtility.RepositoryCountersignPrimarySignatureAsync(
                PrimarySignature.Load(cms.Encode()), request);
            X509Certificate2 authorCertificate = Assert.IsType<X509Certificate2>(signature.SignerInfo.Certificate);
            Assert.True(authorCertificate.NotAfter.ToUniversalTime() < DateTime.UtcNow);

            SignedPackageVerifierSettings settings = new(
                allowUnsigned: true, allowIllegal: false, allowUntrusted, allowIgnoreTimestamp: true,
                allowMultipleTimestamps: true, allowNoTimestamp: true, allowUnknownRevocation,
                reportUnknownRevocation: true, verificationTarget: VerificationTarget.All,
                signaturePlacement: SignaturePlacement.Any,
                repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.IfExistsAndIsNecessary,
                revocationMode: RevocationMode.Online);
            X509ChainStatusFlags primaryFlags = X509ChainStatusFlags.UntrustedRoot | unavailableFlags;
            (VerifySignaturesResult result, TelemetryEvent telemetryEvent) = await VerifyWithChainsAsync(
                primaryFlags, X509ChainStatusFlags.NoError, settings, signature: signature,
                countersignatureFlags: X509ChainStatusFlags.NoError);

            SignedPackageVerificationResult trustResult =
                Assert.IsType<SignedPackageVerificationResult>(Assert.Single(result.Results));
            Assert.Equal(2, trustResult.SignatureSummaries.Count);
            SignatureVerificationSummary primarySummary = trustResult.SignatureSummaries[0];
            SignatureVerificationSummary countersignatureSummary = trustResult.SignatureSummaries[1];
            Assert.NotNull(primarySummary.ExpirationTime);
            Assert.Equal(SignatureVerificationStatus.Valid, countersignatureSummary.Status);
            Timestamp countersignatureTimestamp = Assert.IsType<Timestamp>(countersignatureSummary.Timestamp);
            Assert.False(Rfc3161TimestampVerificationUtility.ValidateSignerCertificateAgainstTimestamp(
                authorCertificate, countersignatureTimestamp));

            SignatureVerificationStatusFlags expectedFlags = SignatureVerificationStatusFlags.NoErrors;
            SignatureVerificationRejectionReasons expectedReasons = SignatureVerificationRejectionReasons.None;
            if (!allowUntrusted)
            {
                expectedFlags |= SignatureVerificationStatusFlags.UntrustedRoot |
                    SignatureVerificationStatusFlags.CertificateExpired;
                expectedReasons |= SignatureVerificationRejectionReasons.Other;
                Assert.DoesNotContain(trustResult.Issues, issue => issue.Code == NuGetLogCode.NU3018);
                Assert.Contains(trustResult.Issues,
                    issue => issue.Code == NuGetLogCode.NU3037 && issue.Level == LogLevel.Error);
            }
            if (!allowUnknownRevocation)
            {
                expectedFlags |= SignatureVerificationStatusFlags.UnknownRevocation;
                expectedReasons |= SignatureVerificationRejectionReasons.RevocationUnavailable;
            }

            Assert.Equal(expectedFlags, primarySummary.Flags);
            Assert.Equal(expectedReasons, primarySummary.SignerRejectionReasons);
            Assert.Equal(primarySummary.Status, trustResult.Trust);
            Assert.Equal(allowUntrusted && allowUnknownRevocation, result.IsValid);
            Assert.Equal(expectedReasons, trustResult.RejectionReasons);
            Assert.Equal((int)expectedReasons, telemetryEvent["VerificationRejectionReasons"]);
            Assert.Equal((int)(allowUnknownRevocation ? X509ChainStatusFlags.NoError : unavailableFlags),
                telemetryEvent["VerificationRevocationRejectionFlags"]);
            X509ChainStatusFlags expectedChainRejectionFlags =
                (allowUntrusted ? X509ChainStatusFlags.NoError : X509ChainStatusFlags.UntrustedRoot) |
                (allowUnknownRevocation ? X509ChainStatusFlags.NoError : unavailableFlags);
            Assert.Equal((int)primaryFlags, telemetryEvent["VerificationDecisionRelevantChainStatusFlags"]);
            Assert.Equal((int)expectedChainRejectionFlags, telemetryEvent["VerificationChainRejectionFlags"]);
            Assert.Equal(!allowUntrusted, telemetryEvent["VerificationHasNonX509Rejection"]);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public async Task VerifySignaturesAsync_WithLaterParsingFailure_PreservesCompletedObservations(
            bool allowIllegal, bool cryptographicException)
        {
            PrimarySignature signature = PrimarySignature.Load(_signature.Value.GetBytes());
            Mock<ISignatureVerificationProvider>? failingProvider = null;
            if (cryptographicException)
            {
                failingProvider = new();
                failingProvider.Setup(provider => provider.GetTrustResultAsync(It.IsAny<ISignedPackageReader>(),
                    It.IsAny<PrimarySignature>(), It.IsAny<SignedPackageVerifierSettings>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromException<PackageVerificationResult>(
                        new System.Security.Cryptography.CryptographicException("A later provider failed.")));
            }
            else
            {
                using X509Certificate2 certificate = _fixture.GetDefaultCertificate();
                CmsSigner countersigner = new(certificate);
                countersigner.SignedAttributes.Add(AttributeUtility.CreateCommitmentTypeIndication(SignatureType.Repository));
                countersigner.SignedAttributes.Add(
                    AttributeUtility.CreateNuGetV3ServiceIndexUrl(new Uri("https://v3serviceIndex.test/api/index.json")));
                countersigner.SignedAttributes.Add(new Pkcs9SigningTime());
                countersigner.SignedAttributes.Add(AttributeUtility.CreateSigningCertificateV2(certificate, HashAlgorithmName.SHA256));
                signature.SignedCms.SignerInfos[0].ComputeCounterSignature(countersigner);
                signature = PrimarySignature.Load(signature.SignedCms.Encode());
                Assert.Equal(2, signature.SignerInfo.CounterSignerInfos.Count);
            }
            SignedPackageVerifierSettings settings = new(
                allowUnsigned: true, allowIllegal, allowUntrusted: false, allowIgnoreTimestamp: true,
                allowMultipleTimestamps: true, allowNoTimestamp: true, allowUnknownRevocation: false,
                reportUnknownRevocation: true, verificationTarget: VerificationTarget.All,
                signaturePlacement: cryptographicException ? SignaturePlacement.PrimarySignature : SignaturePlacement.Any,
                repositoryCountersignatureVerificationBehavior: cryptographicException
                    ? SignatureVerificationBehavior.Never : SignatureVerificationBehavior.IfExists,
                revocationMode: RevocationMode.Online);
            X509ChainStatusFlags signerFlags = X509ChainStatusFlags.UntrustedRoot | X509ChainStatusFlags.RevocationStatusUnknown;
            X509ChainStatusFlags timestampFlags =
                X509ChainStatusFlags.OfflineRevocation | X509ChainStatusFlags.RevocationStatusUnknown;
            int signerBuilds = 0;
            (VerifySignaturesResult result, TelemetryEvent telemetryEvent) = await VerifyWithChainsAsync(
                signerFlags, timestampFlags, settings, signature: signature, signerChainBuilt: () => signerBuilds++,
                additionalProvider: failingProvider?.Object);

            Assert.True(signerBuilds > 0);
            Assert.Equal(allowIllegal, result.IsValid);
            InvalidSignaturePackageVerificationResult parsingResult =
                Assert.IsType<InvalidSignaturePackageVerificationResult>(Assert.Single(result.Results));
            Assert.Equal(allowIllegal ? SignatureVerificationStatus.Valid : SignatureVerificationStatus.Disallowed,
                parsingResult.Trust);
            NuGetLogCode expectedCode = cryptographicException ? NuGetLogCode.NU3003 : NuGetLogCode.NU3032;
            SignatureLog parsingIssue = Assert.Single(parsingResult.Issues, issue => issue.Code == expectedCode);
            Assert.Equal(allowIllegal ? LogLevel.Warning : LogLevel.Error, parsingIssue.Level);
            Assert.Equal(cryptographicException ? Strings.ErrorPackageSignatureInvalid : Strings.Error_NotOneRepositoryCounterSignature,
                parsingIssue.Message);
            SignatureVerificationSummary summary = Assert.Single(parsingResult.SignatureSummaries);
            Assert.Equal(signerFlags, summary.SignerChainStatusFlags);
            Assert.Equal(timestampFlags, summary.TimestampChainStatusFlags);
            Assert.True(summary.SignerRejectionReasons.HasFlag(SignatureVerificationRejectionReasons.RevocationUnavailable));
            Assert.Equal((int)signerFlags, telemetryEvent["SignerChainStatusFlags"]);
            Assert.Equal((int)timestampFlags, telemetryEvent["TimestampChainStatusFlags"]);
            Assert.Equal("Evaluated", telemetryEvent["RevocationEvaluationStatus"]);
            Assert.Equal(true, telemetryEvent["RevocationInformationUnavailable"]);
            Assert.Equal(allowIllegal ? 0 : (int)SignatureVerificationRejectionReasons.Other,
                telemetryEvent["VerificationRejectionReasons"]);
            Assert.Equal(!allowIllegal, telemetryEvent["VerificationHasNonX509Rejection"]);
            Assert.Equal(0, telemetryEvent["VerificationRevocationRejectionFlags"]);
            List<TelemetryEvent> chains =
                Assert.IsType<List<TelemetryEvent>>(telemetryEvent.ComplexData["SignatureChains"]);
            Assert.Equal(0, telemetryEvent["VerificationDecisionRelevantChainStatusFlags"]);
            Assert.Equal(0, telemetryEvent["VerificationChainRejectionFlags"]);
            Assert.Equal(2, chains.Count);
            Assert.Equal(SignaturePlacement.PrimarySignature.ToString(), chains[0]["SignaturePlacement"]);
            Assert.Equal(SignaturePlacement.PrimarySignature.ToString(), chains[1]["SignaturePlacement"]);
            Assert.Equal(CertificateType.Signature.ToString(), chains[0]["CertificateType"]);
            Assert.Equal(CertificateType.Timestamp.ToString(), chains[1]["CertificateType"]);
            Assert.Equal((int)signerFlags, chains[0]["ChainStatusFlags"]);
            Assert.Equal((int)timestampFlags, chains[1]["ChainStatusFlags"]);
            Assert.Equal(0, chains[0]["RejectionReasons"]);
            Assert.Equal(0, chains[1]["RejectionReasons"]);
            Assert.Equal(0, chains[0]["DecisionRelevantChainStatusFlags"]);
            Assert.Equal(0, chains[0]["RejectionChainStatusFlags"]);
            Assert.Equal(0, chains[1]["DecisionRelevantChainStatusFlags"]);
            Assert.Equal(0, chains[1]["RejectionChainStatusFlags"]);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public async Task VerifySignaturesAsync_UnsignedTelemetry_AttributesOnlyPolicyRejection(
            bool allowUnsigned, bool allowIllegal)
        {
            SignedPackageVerifierSettings settings = new(
                allowUnsigned, allowIllegal, allowUntrusted: true, allowIgnoreTimestamp: true,
                allowMultipleTimestamps: true, allowNoTimestamp: true, allowUnknownRevocation: true,
                reportUnknownRevocation: true, verificationTarget: VerificationTarget.All,
                signaturePlacement: SignaturePlacement.Any,
                repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.IfExists,
                revocationMode: RevocationMode.Online);
            (VerifySignaturesResult result, TelemetryEvent telemetryEvent) = await VerifyWithChainsAsync(
                X509ChainStatusFlags.NoError, X509ChainStatusFlags.NoError, settings, isSigned: false);

            Assert.Equal(allowUnsigned, result.IsValid);
            Assert.False(result.IsSigned);
            if (allowUnsigned)
            {
                Assert.Empty(result.Results);
            }
            else
            {
                UnsignedPackageVerificationResult unsignedResult =
                    Assert.IsType<UnsignedPackageVerificationResult>(Assert.Single(result.Results));
                Assert.Equal(allowIllegal ? SignatureVerificationStatus.Valid : SignatureVerificationStatus.Disallowed,
                    unsignedResult.Trust);
                SignatureLog issue = Assert.Single(unsignedResult.Issues);
                Assert.Equal(NuGetLogCode.NU3004, issue.Code);
                Assert.Equal(LogLevel.Error, issue.Level);
                Assert.Equal(Strings.ErrorPackageNotSigned, issue.Message);
            }
            Assert.Equal(allowUnsigned ? NuGetOperationStatus.Succeeded : NuGetOperationStatus.Failed, telemetryEvent["Status"]);
            Assert.Equal(allowUnsigned ? 0 : (int)SignatureVerificationRejectionReasons.Other,
                telemetryEvent["VerificationRejectionReasons"]);
            Assert.Equal(!allowUnsigned, telemetryEvent["VerificationHasNonX509Rejection"]);
            Assert.Equal(0, telemetryEvent["VerificationRevocationRejectionFlags"]);
            Assert.Equal(0, telemetryEvent["SignerChainStatusFlags"]);
            Assert.Equal(0, telemetryEvent["TimestampChainStatusFlags"]);
            Assert.Equal(false, telemetryEvent["RevocationInformationUnavailable"]);
            Assert.Equal("NotEvaluated", telemetryEvent["RevocationEvaluationStatus"]);
            Assert.Empty(Assert.IsType<List<TelemetryEvent>>(telemetryEvent.ComplexData["SignatureChains"]));
        }

        [Theory]
        [InlineData(X509ChainStatusFlags.NoError, false)]
        [InlineData(X509ChainStatusFlags.NoError, true)]
        [InlineData(X509ChainStatusFlags.Revoked, false)]
        [InlineData(X509ChainStatusFlags.Revoked, true)]
        public async Task VerifySignaturesAsync_ChainTelemetry_DistinguishesMissingCountersignatureBlocker(
            X509ChainStatusFlags signerFlags, bool requireCountersignature)
        {
            TestSignedCms cms = TestSignedCms.Decode(_signature.Value.SignedCms);
            cms.SignerInfos[0].RemoveUnsignedAttribute(TestOids.Countersignature);
            PrimarySignature signature = PrimarySignature.Load(cms.Encode());
            SignedPackageVerifierSettings settings = new(
                allowUnsigned: true, allowIllegal: false, allowUntrusted: false, allowIgnoreTimestamp: false,
                allowMultipleTimestamps: true, allowNoTimestamp: true, allowUnknownRevocation: false,
                reportUnknownRevocation: true, verificationTarget: VerificationTarget.All,
                signaturePlacement: SignaturePlacement.Any,
                repositoryCountersignatureVerificationBehavior: requireCountersignature
                    ? SignatureVerificationBehavior.Always : SignatureVerificationBehavior.IfExists,
                revocationMode: RevocationMode.Online);
            (VerifySignaturesResult result, TelemetryEvent telemetryEvent) = await VerifyWithChainsAsync(
                signerFlags, X509ChainStatusFlags.NoError, settings, signature: signature);

            Assert.Equal(signerFlags == X509ChainStatusFlags.NoError && !requireCountersignature, result.IsValid);
            Assert.Equal(requireCountersignature, telemetryEvent["VerificationHasNonX509Rejection"]);
            Assert.Equal(requireCountersignature, telemetryEvent["AcceptanceHasNonX509Rejection"]);
            Assert.Equal(false, telemetryEvent["WarningPromotionHasNonX509Rejection"]);
            Assert.Equal((int)signerFlags, telemetryEvent["VerificationChainRejectionFlags"]);
            Assert.Equal(result.IsValid ? 0 : (int)SignatureVerificationRejectionReasons.Other,
                telemetryEvent["VerificationRejectionReasons"]);
            Assert.Equal(requireCountersignature, result.Results.SelectMany(value => value.Issues)
                .Any(issue => issue.Code == NuGetLogCode.NU3038 && issue.Level == LogLevel.Error));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task VerifySignaturesAsync_ChainTelemetry_DistinguishesMissingTimestampBlocker(bool requireTimestamp)
        {
            TestSignedCms cms = TestSignedCms.Decode(_signature.Value.SignedCms);
            cms.SignerInfos[0].RemoveUnsignedAttribute(TestOids.SignatureTimestampToken);
            cms.SignerInfos[0].RemoveUnsignedAttribute(TestOids.Countersignature);
            PrimarySignature signature = PrimarySignature.Load(cms.Encode());
            SignedPackageVerifierSettings settings = new(
                allowUnsigned: true, allowIllegal: false, allowUntrusted: false, allowIgnoreTimestamp: false,
                allowMultipleTimestamps: true, allowNoTimestamp: !requireTimestamp, allowUnknownRevocation: false,
                reportUnknownRevocation: true, verificationTarget: VerificationTarget.All,
                signaturePlacement: SignaturePlacement.PrimarySignature,
                repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.Never,
                revocationMode: RevocationMode.Online);
            (VerifySignaturesResult result, TelemetryEvent telemetryEvent) = await VerifyWithChainsAsync(
                X509ChainStatusFlags.Revoked, X509ChainStatusFlags.NoError, settings, signature: signature);

            Assert.False(result.IsValid);
            Assert.Equal(requireTimestamp, telemetryEvent["VerificationHasNonX509Rejection"]);
            Assert.Equal((int)X509ChainStatusFlags.Revoked, telemetryEvent["VerificationChainRejectionFlags"]);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task VerifySignaturesAsync_ChainTelemetry_DistinguishesTimestampRejectionFromSignerTimeBlocker(bool allowUntrusted)
        {
            SignedPackageVerifierSettings settings = new(
                allowUnsigned: true, allowIllegal: false, allowUntrusted, allowIgnoreTimestamp: false,
                allowMultipleTimestamps: true, allowNoTimestamp: true, allowUnknownRevocation: false,
                reportUnknownRevocation: true, verificationTarget: VerificationTarget.All,
                signaturePlacement: SignaturePlacement.PrimarySignature,
                repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.Never,
                revocationMode: RevocationMode.Online);
            X509ChainStatusFlags timestampFlags = X509ChainStatusFlags.Revoked |
                X509ChainStatusFlags.RevocationStatusUnknown | X509ChainStatusFlags.OfflineRevocation;
            (VerifySignaturesResult result, TelemetryEvent telemetryEvent) = await VerifyWithChainsAsync(
                X509ChainStatusFlags.NoError, timestampFlags, settings);

            Assert.False(result.IsValid);
            SignedPackageVerificationResult trustResult =
                Assert.IsType<SignedPackageVerificationResult>(Assert.Single(result.Results));
            SignatureVerificationSummary summary = Assert.Single(trustResult.SignatureSummaries);
            Assert.Equal(SignatureVerificationStatusFlags.NoValidTimestamp, summary.Flags);
            Assert.False(summary.TimestampHasNonX509Rejection);
            Assert.Equal(!allowUntrusted, summary.SignerHasNonX509Rejection);
            Assert.Equal(!allowUntrusted, telemetryEvent["VerificationHasNonX509Rejection"]);
            Assert.Equal((int)X509ChainStatusFlags.Revoked, telemetryEvent["VerificationChainRejectionFlags"]);
            Assert.Equal(!allowUntrusted, trustResult.Issues.Any(issue =>
                issue.Code == NuGetLogCode.NU3037 && issue.Level == LogLevel.Error));
        }

        [Theory]
        [InlineData(false, X509ChainStatusFlags.NoError)]
        [InlineData(true, X509ChainStatusFlags.NoError)]
        [InlineData(false, X509ChainStatusFlags.Revoked)]
        [InlineData(true, X509ChainStatusFlags.Revoked)]
        [InlineData(false, X509ChainStatusFlags.RevocationStatusUnknown)]
        [InlineData(true, X509ChainStatusFlags.RevocationStatusUnknown)]
        public async Task VerifySignaturesAsync_ChainTelemetry_DistinguishesAdditionalProviderBlocker(
            bool integrityFailure, X509ChainStatusFlags signerFlags)
        {
            ISignatureVerificationProvider additionalProvider = integrityFailure
                ? new IntegrityVerificationProvider()
                : new AllowListVerificationProvider(Array.Empty<VerificationAllowListEntry>(), requireNonEmptyAllowList: true);
            SignedPackageVerifierSettings settings = new(
                allowUnsigned: true, allowIllegal: false, allowUntrusted: false, allowIgnoreTimestamp: false,
                allowMultipleTimestamps: true, allowNoTimestamp: true, allowUnknownRevocation: false,
                reportUnknownRevocation: true, verificationTarget: VerificationTarget.All,
                signaturePlacement: SignaturePlacement.PrimarySignature,
                repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.Never,
                revocationMode: RevocationMode.Online);
            (VerifySignaturesResult result, TelemetryEvent telemetryEvent) = await VerifyWithChainsAsync(
                signerFlags, X509ChainStatusFlags.NoError, settings,
                additionalProvider: additionalProvider, integrityFails: integrityFailure);

            Assert.False(result.IsValid);
            Assert.Equal(true, telemetryEvent["VerificationHasNonX509Rejection"]);
            Assert.Equal((int)signerFlags, telemetryEvent["VerificationChainRejectionFlags"]);
            SignatureVerificationRejectionReasons expectedReasons = SignatureVerificationRejectionReasons.Other;
            if (signerFlags == X509ChainStatusFlags.RevocationStatusUnknown)
            {
                expectedReasons |= SignatureVerificationRejectionReasons.RevocationUnavailable;
            }
            Assert.Equal((int)expectedReasons, telemetryEvent["VerificationRejectionReasons"]);
            Assert.Equal(SignatureVerificationRejectionReasons.Other, result.Results.Last().RejectionReasons);
            Assert.Contains(result.Results.SelectMany(value => value.Issues),
                issue => issue.Code == (integrityFailure ? NuGetLogCode.NU3008 : NuGetLogCode.NU3034));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task VerifySignaturesAsync_ChainTelemetry_DoesNotAttributeValidProviderResults(bool integrityProvider)
        {
            ISignatureVerificationProvider additionalProvider = integrityProvider
                ? new IntegrityVerificationProvider()
                : new AllowListVerificationProvider(Array.Empty<VerificationAllowListEntry>());
            SignedPackageVerifierSettings settings = SignedPackageVerifierSettings.GetDefault(TestEnvironmentVariableReader.EmptyInstance);
            (VerifySignaturesResult result, TelemetryEvent telemetryEvent) = await VerifyWithChainsAsync(
                X509ChainStatusFlags.NoError, X509ChainStatusFlags.NoError, settings,
                additionalProvider: additionalProvider);

            Assert.True(result.IsValid);
            Assert.Equal(SignatureVerificationRejectionReasons.None, result.Results.Last().RejectionReasons);
            Assert.Equal(0, telemetryEvent["VerificationRejectionReasons"]);
            Assert.Equal(false, telemetryEvent["VerificationHasNonX509Rejection"]);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task VerifySignaturesAsync_ChainTelemetry_AttributesOnlyEnforcedAllowListMismatch(bool allowUntrusted)
        {
            AllowListVerificationProvider additionalProvider = new(new VerificationAllowListEntry[]
            {
                new CertificateHashAllowListEntry(
                    VerificationTarget.All, SignaturePlacement.PrimarySignature, "not-a-matching-fingerprint", HashAlgorithmName.SHA256)
            });
            SignedPackageVerifierSettings settings = new(
                allowUnsigned: true, allowIllegal: false, allowUntrusted, allowIgnoreTimestamp: false,
                allowMultipleTimestamps: true, allowNoTimestamp: true, allowUnknownRevocation: false,
                reportUnknownRevocation: true, verificationTarget: VerificationTarget.All,
                signaturePlacement: SignaturePlacement.PrimarySignature,
                repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.Never,
                revocationMode: RevocationMode.Online);
            (VerifySignaturesResult result, TelemetryEvent telemetryEvent) = await VerifyWithChainsAsync(
                X509ChainStatusFlags.NoError, X509ChainStatusFlags.NoError, settings,
                additionalProvider: additionalProvider);

            SignatureVerificationRejectionReasons expectedReasons = allowUntrusted
                ? SignatureVerificationRejectionReasons.None : SignatureVerificationRejectionReasons.Other;
            Assert.Equal(allowUntrusted, result.IsValid);
            Assert.Equal(expectedReasons, result.Results.Last().RejectionReasons);
            Assert.Equal((int)expectedReasons, telemetryEvent["VerificationRejectionReasons"]);
            Assert.Equal(!allowUntrusted, telemetryEvent["VerificationHasNonX509Rejection"]);
            Assert.Contains(result.Results.Last().Issues, issue => issue.Code == NuGetLogCode.NU3034 &&
                issue.Level == (allowUntrusted ? LogLevel.Warning : LogLevel.Error));
        }

        [Theory]
        [InlineData(PackageExtractionTelemetryEvent.EventName)]
        [InlineData(PackageSigningTelemetryEvent.EventName)]
        public async Task VerifySignaturesAsync_ChainTelemetry_IgnoresUnrelatedTelemetry(string eventName)
        {
            SignedPackageVerifierSettings settings = SignedPackageVerifierSettings.GetDefault(TestEnvironmentVariableReader.EmptyInstance);
            TelemetryEvent unrelatedEvent = new(eventName);
            (VerifySignaturesResult result, TelemetryEvent telemetryEvent) = await VerifyWithChainsAsync(
                X509ChainStatusFlags.NoError, X509ChainStatusFlags.NoError, settings,
                signerChainBuilt: () =>
                {
                    using TelemetryActivity activity = TelemetryActivity.Create(Guid.NewGuid(), unrelatedEvent);
                });

            Assert.True(result.IsValid);
            Assert.NotSame(unrelatedEvent, telemetryEvent);
            Assert.Equal(PackageSigningTelemetryEvent.EventName, telemetryEvent.Name);
            Assert.Equal(NuGetOperationStatus.Succeeded, telemetryEvent["Status"]);
        }

        private async Task<(VerifySignaturesResult Result, TelemetryEvent Telemetry)> VerifyWithChainsAsync(
            X509ChainStatusFlags signerFlags, X509ChainStatusFlags timestampFlags, SignedPackageVerifierSettings settings,
            bool? signerBuildSucceeded = null, bool? timestampBuildSucceeded = null, PrimarySignature? signature = null,
            X509ChainStatusFlags? countersignatureFlags = null, Action? signerChainBuilt = null,
            ISignatureVerificationProvider? additionalProvider = null, bool isSigned = true, bool integrityFails = false)
        {
            IX509ChainFactory originalSignerFactory = X509TrustStore.GetX509ChainFactory(Signing.X509StorePurpose.CodeSigning, NullLogger.Instance);
            IX509ChainFactory originalTimestampFactory = X509TrustStore.GetX509ChainFactory(Signing.X509StorePurpose.Timestamping, NullLogger.Instance);
            INuGetTelemetryService? originalTelemetryService = TelemetryActivity.NuGetTelemetryService;
            Guid parentId = Guid.NewGuid();
            ConcurrentQueue<TelemetryEvent> events = new();
            Mock<INuGetTelemetryService> telemetryService = new();
            telemetryService.Setup(service => service.EmitTelemetryEvent(It.Is<TelemetryEvent>(telemetryEvent =>
                telemetryEvent.Name == PackageSigningTelemetryEvent.EventName &&
                parentId.ToString().Equals(telemetryEvent[nameof(TelemetryActivity.ParentId)]))))
                .Callback<TelemetryEvent>(telemetryEvent => events.Enqueue(telemetryEvent));
            signature ??= _signature.Value;
            Mock<ISignedPackageReader> package = new();
            package.Setup(reader => reader.IsSignedAsync(It.IsAny<CancellationToken>())).ReturnsAsync(isSigned);
            package.Setup(reader => reader.GetPrimarySignatureAsync(It.IsAny<CancellationToken>())).ReturnsAsync(signature);
            if (integrityFails)
            {
                package.Setup(reader => reader.ValidateIntegrityAsync(It.IsAny<SignatureContent>(), It.IsAny<CancellationToken>()))
                    .ThrowsAsync(new InvalidOperationException("Package integrity failed."));
            }

            try
            {
                X509Certificate2? countersignatureCertificate = countersignatureFlags.HasValue
                    ? RepositoryCountersignature.GetRepositoryCountersignature(signature)?.SignerInfo.Certificate : null;
                X509TrustStore.SetCodeSigningX509ChainFactory(CreateChainFactory(
                    signerFlags, settings.RevocationMode, signerBuildSucceeded, countersignatureCertificate,
                    countersignatureFlags.GetValueOrDefault(), signerChainBuilt));
                X509TrustStore.SetTimestampingX509ChainFactory(CreateChainFactory(timestampFlags, settings.RevocationMode, timestampBuildSucceeded));
                TelemetryActivity.NuGetTelemetryService = telemetryService.Object;

                ISignatureVerificationProvider[] providers = additionalProvider == null
                    ? new ISignatureVerificationProvider[] { _provider }
                    : new ISignatureVerificationProvider[] { _provider, additionalProvider };
                PackageSignatureVerifier verifier = new(providers);
                VerifySignaturesResult result = await verifier.VerifySignaturesAsync(package.Object, settings, CancellationToken.None, parentId);

                return (result, Assert.Single(events));
            }
            finally
            {
                X509TrustStore.SetCodeSigningX509ChainFactory(originalSignerFactory);
                X509TrustStore.SetTimestampingX509ChainFactory(originalTimestampFactory);
                TelemetryActivity.NuGetTelemetryService = originalTelemetryService;
            }
        }

        private static IX509ChainFactory CreateChainFactory(
            X509ChainStatusFlags flags, RevocationMode revocationMode, bool? buildSucceeded = null,
            X509Certificate2? countersignatureCertificate = null,
            X509ChainStatusFlags countersignatureFlags = X509ChainStatusFlags.NoError, Action? chainBuilt = null)
        {
            Mock<IX509ChainFactory> factory = new();
            factory.Setup(value => value.Create()).Returns(() =>
            {
                X509Chain nativeChain = new();
                X509ChainStatusFlags currentFlags = flags;
                Mock<IX509Chain> chain = new();
                chain.SetupGet(value => value.PrivateReference).Returns(nativeChain);
                chain.SetupGet(value => value.ChainPolicy).Returns(nativeChain.ChainPolicy);
                chain.SetupGet(value => value.ChainElements).Returns(nativeChain.ChainElements);
                chain.Setup(value => value.Dispose()).Callback(nativeChain.Dispose);
                chain.SetupGet(value => value.ChainStatus).Returns(() => Enum.GetValues(typeof(X509ChainStatusFlags))
                    .Cast<X509ChainStatusFlags>()
                    .Where(flag => flag != X509ChainStatusFlags.NoError && currentFlags.HasFlag(flag))
                    .Select(flag => new X509ChainStatus { Status = flag, StatusInformation = flag.ToString() })
                    .ToArray());
                chain.Setup(value => value.Build(It.IsAny<X509Certificate2>())).Returns<X509Certificate2>(certificate =>
                {
                    chainBuilt?.Invoke();
                    currentFlags = certificate.Equals(countersignatureCertificate) ? countersignatureFlags : flags;
                    Assert.Equal(revocationMode == RevocationMode.Offline ? X509RevocationMode.Offline : X509RevocationMode.Online, nativeChain.ChainPolicy.RevocationMode);
                    return buildSucceeded ?? currentFlags == X509ChainStatusFlags.NoError;
                });
                return chain.Object;
            });
            return factory.Object;
        }
    }
}
