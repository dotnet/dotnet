// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Test.Utility.Signing;
using Xunit;

namespace NuGet.Packaging.FuncTest
{
    [Collection(SigningTestCollection.Name)]
    public class TimestampProviderTests
    {
        private const string OperationCancelledExceptionMessage = "The operation was canceled.";

        private SigningTestFixture _testFixture;
        private TrustedTestCert<TestCertificate> _trustedTestCert;

        public TimestampProviderTests(SigningTestFixture fixture)
        {
            _testFixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _trustedTestCert = _testFixture.TrustedTestCertificate;
        }

        [CIOnlyFact]
        public async Task GetTimestampAsync_WithValidInput_ReturnsTimestampAsync()
        {
            var logger = new TestLogger();
            var timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();
            var timestampProvider = new Rfc3161TimestampProvider(timestampService.Url);
            var content = new SignatureContent(SigningSpecifications.V1, Common.HashAlgorithmName.SHA256, "Test data to be signed and timestamped");
            using (var authorCert = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedCms = SigningTestUtility.GenerateSignedCms(authorCert, content.GetBytes());
                var primarySignature = PrimarySignature.Load(signedCms.Encode());
                var timestampHashAlgorithm = Common.HashAlgorithmName.SHA256;
                var signatureValue = primarySignature.GetSignatureValue();
                var messageHash = timestampHashAlgorithm.ComputeHash(signatureValue);

                var request = new TimestampRequest(
                    signingSpecifications: SigningSpecifications.V1,
                    hashedMessage: messageHash,
                    hashAlgorithm: timestampHashAlgorithm,
                    target: SignaturePlacement.PrimarySignature
                );

                // Act
                var timestampedCms = await timestampProvider.GetTimestampAsync(request, logger, CancellationToken.None);

                // Assert
                timestampedCms.Should().NotBeNull();
                timestampedCms.Detached.Should().BeFalse();
                timestampedCms.ContentInfo.Should().NotBeNull();
                timestampedCms.SignerInfos.Count.Should().Be(1);
            }
        }

        [CIOnlyFact]
        public async Task GetTimestampAsync_AssertCompleteChain_SuccessAsync()
        {
            var timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();
            var timestampProvider = new Rfc3161TimestampProvider(timestampService.Url);
            var nupkg = new SimpleTestPackageContext();

            using (var authorCert = new X509Certificate2(_trustedTestCert.Source.Cert))
            using (var packageStream = await nupkg.CreateAsStreamAsync())
            {
                // Act
                AuthorPrimarySignature signature = await SignedArchiveTestUtility.CreateAuthorSignatureForPackageAsync(authorCert, packageStream, timestampProvider);
                var authorSignedCms = signature.SignedCms;
                var timestamp = signature.Timestamps.First();
                var timestampCms = timestamp.SignedCms;
                IX509CertificateChain certificateChain;
                var chainBuildSuccess = true;

                // rebuild the chain to get the list of certificates
                using (X509ChainHolder chainHolder = X509ChainHolder.CreateForTimestamping())
                {
                    IX509Chain chain = chainHolder.Chain2;
                    var policy = chain.ChainPolicy;

                    policy.ApplicationPolicy.Add(new Oid(Oids.TimeStampingEku));
                    policy.VerificationFlags = X509VerificationFlags.IgnoreNotTimeValid;
                    policy.RevocationFlag = X509RevocationFlag.ExcludeRoot;
                    policy.RevocationMode = X509RevocationMode.Online;

                    var timestampSignerCertificate = timestampCms.SignerInfos[0].Certificate;
                    chainBuildSuccess = chain.Build(timestampSignerCertificate);
                    certificateChain = CertificateChainUtility.GetCertificateChain(chain.PrivateReference);
                }

                using (certificateChain)
                {
                    // Assert
                    authorSignedCms.Should().NotBeNull();
                    authorSignedCms.Detached.Should().BeFalse();
                    authorSignedCms.ContentInfo.Should().NotBeNull();
                    authorSignedCms.SignerInfos.Count.Should().Be(1);
                    authorSignedCms.SignerInfos[0].UnsignedAttributes.Count.Should().Be(1);
                    authorSignedCms.SignerInfos[0].UnsignedAttributes[0].Oid.Value.Should().Be(Oids.SignatureTimeStampTokenAttribute);

                    timestampCms.Should().NotBeNull();
                    timestampCms.Detached.Should().BeFalse();
                    timestampCms.ContentInfo.Should().NotBeNull();

                    chainBuildSuccess.Should().BeTrue();
                    certificateChain.Count.Should().Be(timestampCms.Certificates.Count);

                    foreach (var cert in certificateChain)
                    {
                        timestampCms.Certificates.Contains(cert).Should().BeTrue();
                    }
                }
            }
        }

        [CIOnlyFact]
        public async Task GetTimestampAsync_WhenRequestNull_ThrowsAsync()
        {
            var logger = new TestLogger();
            var timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();
            var timestampProvider = new Rfc3161TimestampProvider(timestampService.Url);
            var authorCertName = "author@nuget.func.test";
            var content = new SignatureContent(SigningSpecifications.V1, Common.HashAlgorithmName.SHA256, "Test data to be signed and timestamped");

            Action<TestCertificateGenerator> modifyGenerator = delegate (TestCertificateGenerator gen)
            {
                gen.NotBefore = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)); // cert has expired
            };

            using (var authorCert = SigningTestUtility.GenerateCertificate(authorCertName, modifyGenerator: modifyGenerator))
            {
                var timestampHashAlgorithm = Common.HashAlgorithmName.SHA256;
                var signedCms = SigningTestUtility.GenerateSignedCms(authorCert, content.GetBytes());
                var signature = PrimarySignature.Load(signedCms.Encode());
                var signatureValue = signature.GetSignatureValue();
                var messageHash = timestampHashAlgorithm.ComputeHash(signatureValue);

                var request = new TimestampRequest(
                    signingSpecifications: SigningSpecifications.V1,
                    hashedMessage: messageHash,
                    hashAlgorithm: timestampHashAlgorithm,
                    target: SignaturePlacement.PrimarySignature
                );

                // Assert
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => timestampProvider.GetTimestampAsync(request: null, logger, CancellationToken.None));

                Assert.Equal("request", exception.ParamName);
                Assert.StartsWith("Value cannot be null.", exception.Message);
            }
        }

        [CIOnlyFact]
        public async Task GetTimestampAsync_WhenLoggerNull_ThrowsAsync()
        {
            var timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();
            var timestampProvider = new Rfc3161TimestampProvider(timestampService.Url);
            var content = new SignatureContent(SigningSpecifications.V1, Common.HashAlgorithmName.SHA256, "Test data to be signed and timestamped");

            using (var authorCert = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var timestampHashAlgorithm = Common.HashAlgorithmName.SHA256;
                var signedCms = SigningTestUtility.GenerateSignedCms(authorCert, content.GetBytes());
                var signature = PrimarySignature.Load(signedCms.Encode());
                var signatureValue = signature.GetSignatureValue();
                var messageHash = timestampHashAlgorithm.ComputeHash(signatureValue);

                var request = new TimestampRequest(
                    signingSpecifications: SigningSpecifications.V1,
                    hashedMessage: messageHash,
                    hashAlgorithm: timestampHashAlgorithm,
                    target: SignaturePlacement.PrimarySignature
                );

                // Assert
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => timestampProvider.GetTimestampAsync(request, logger: null, CancellationToken.None));

                Assert.Equal("logger", exception.ParamName);
                Assert.StartsWith("Value cannot be null.", exception.Message);
            }
        }

        [CIOnlyFact]
        public async Task GetTimestampAsync_WhenCancelled_ThrowsAsync()
        {
            var logger = new TestLogger();
            var timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();
            var timestampProvider = new Rfc3161TimestampProvider(timestampService.Url);
            var content = new SignatureContent(SigningSpecifications.V1, Common.HashAlgorithmName.SHA256, "Test data to be signed and timestamped");

            using (var authorCert = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var timestampHashAlgorithm = Common.HashAlgorithmName.SHA256;
                var signedCms = SigningTestUtility.GenerateSignedCms(authorCert, content.GetBytes());
                var signature = PrimarySignature.Load(signedCms.Encode());
                var signatureValue = signature.GetSignatureValue();
                var messageHash = timestampHashAlgorithm.ComputeHash(signatureValue);

                var request = new TimestampRequest(
                   signingSpecifications: SigningSpecifications.V1,
                   hashedMessage: messageHash,
                   hashAlgorithm: timestampHashAlgorithm,
                   target: SignaturePlacement.PrimarySignature
               );

                // Assert
                var exception = await Assert.ThrowsAsync<OperationCanceledException>(
                    () => timestampProvider.GetTimestampAsync(request, logger, new CancellationToken(canceled: true)));

                Assert.Equal(OperationCancelledExceptionMessage, exception.Message);
            }
        }

        [CIOnlyFact]
        public async Task GetTimestampAsync_WhenRevocationInformationUnavailable_SuccessAsync()
        {
            var testServer = await _testFixture.GetSigningTestServerAsync();
            var ca = await _testFixture.GetDefaultTrustedCertificateAuthorityAsync();
            var ca2 = ca.CreateIntermediateCertificateAuthority();
            var timestampService = TimestampService.Create(ca2);

            // Only register the AIA responder.  Do not register the OCSP responder.
            using (testServer.RegisterResponder(ca2))
            {
                VerifyTimestampData(
                    testServer,
                    timestampService,
                    async (timestampProvider, request) =>
                    {
                        var logger = new TestLogger();
                        var timestamp = await timestampProvider.GetTimestampAsync(request, logger, CancellationToken.None);

                        Assert.NotNull(timestamp);

                        Assert.Equal(0, logger.Errors);
                        Assert.Equal(2, logger.Warnings);

                        AssertOfflineRevocation(logger.LogMessages, LogLevel.Warning);
                        AssertRevocationStatusUnknown(logger.LogMessages, LogLevel.Warning);
                    });
            }
        }

        [CIOnlyFact]
        public async Task GetTimestampAsync_WhenTimestampSigningCertificateRevoked_ThrowsAsync()
        {
            var testServer = await _testFixture.GetSigningTestServerAsync();
            var certificateAuthority = await _testFixture.GetDefaultTrustedCertificateAuthorityAsync();
            var timestampService = TimestampService.Create(certificateAuthority);

            certificateAuthority.Revoke(
                timestampService.Certificate,
                RevocationReason.KeyCompromise,
                DateTimeOffset.UtcNow);

            VerifyTimestampData(
                testServer,
                timestampService,
                async (timestampProvider, request) =>
                {
                    var exception = await Assert.ThrowsAsync<TimestampException>(
                        () => timestampProvider.GetTimestampAsync(request, NullLogger.Instance, CancellationToken.None));

                    Assert.Equal("Certificate chain validation failed.", exception.Message);
                });
        }

        [CIOnlyFact]
        public async Task GetTimestampAsync_WithFailureReponse_ThrowsAsync()
        {
            var testServer = await _testFixture.GetSigningTestServerAsync();
            var certificateAuthority = await _testFixture.GetDefaultTrustedCertificateAuthorityAsync();
            var options = new TimestampServiceOptions() { ReturnFailure = true };
            var timestampService = TimestampService.Create(certificateAuthority, options);

            VerifyTimestampData(
                testServer,
                timestampService,
                async (timestampProvider, request) =>
                {
                    var exception = await Assert.ThrowsAsync<CryptographicException>(
                        () => timestampProvider.GetTimestampAsync(request, NullLogger.Instance, CancellationToken.None));

                    Assert.StartsWith(
                        "The timestamp signature and/or certificate could not be verified or is malformed.",
                        exception.Message);
                });
        }

        [CIOnlyFact]
        public async Task GetTimestampAsync_WhenSigningCertificateNotReturned_ThrowsAsync()
        {
            var testServer = await _testFixture.GetSigningTestServerAsync();
            var certificateAuthority = await _testFixture.GetDefaultTrustedCertificateAuthorityAsync();
            var options = new TimestampServiceOptions() { ReturnSigningCertificate = false };
            var timestampService = TimestampService.Create(certificateAuthority, options);

            VerifyTimestampData(
                testServer,
                timestampService,
                async (timestampProvider, request) =>
                {
                    var exception = await Assert.ThrowsAsync<CryptographicException>(
                        () => timestampProvider.GetTimestampAsync(request, NullLogger.Instance, CancellationToken.None));

                    Assert.StartsWith("Cannot find object or property.", exception.Message);
                });
        }

        [CIOnlyFact]
        public async Task GetTimestampAsync_WhenSignatureHashAlgorithmIsSha1_ThrowsAsync()
        {
            var testServer = await _testFixture.GetSigningTestServerAsync();
            var certificateAuthority = await _testFixture.GetDefaultTrustedCertificateAuthorityAsync();
            var timestampServiceOptions = new TimestampServiceOptions() { SignatureHashAlgorithm = new Oid(Oids.Sha1) };
            var timestampService = TimestampService.Create(certificateAuthority, timestampServiceOptions);

            VerifyTimestampData(
                testServer,
                timestampService,
                async (timestampProvider, request) =>
                {
                    var exception = await Assert.ThrowsAsync<TimestampException>(
                        () => timestampProvider.GetTimestampAsync(request, NullLogger.Instance, CancellationToken.None));

                    Assert.Equal(
                        "The timestamp signature has an unsupported digest algorithm (SHA1). The following algorithms are supported: SHA256, SHA384, SHA512.",
                        exception.Message);
                });
        }

        [CIOnlyFact]
        public async Task GetTimestampAsync_WhenCertificateSignatureAlgorithmIsSha1_ThrowsAsync()
        {
            var testServer = await _testFixture.GetSigningTestServerAsync();
            var certificateAuthority = await _testFixture.GetDefaultTrustedCertificateAuthorityAsync();
            var timestampServiceOptions = new TimestampServiceOptions() { SignatureHashAlgorithm = new Oid(Oids.Sha1) };
            var issueCertificateOptions = IssueCertificateOptions.CreateDefaultForTimestampService();
            issueCertificateOptions.SignatureAlgorithmName = "SHA1WITHRSA";

            var timestampService = TimestampService.Create(certificateAuthority, timestampServiceOptions, issueCertificateOptions);

            VerifyTimestampData(
                testServer,
                timestampService,
                async (timestampProvider, request) =>
                {
                    var exception = await Assert.ThrowsAsync<TimestampException>(
                        () => timestampProvider.GetTimestampAsync(request, NullLogger.Instance, CancellationToken.None));

                    Assert.Equal(
                        "The timestamp certificate has an unsupported signature algorithm (SHA1RSA). The following algorithms are supported: SHA256RSA, SHA384RSA, SHA512RSA.",
                        exception.Message);
                });
        }

        [CIOnlyFact]
        public async Task GetTimestampAsync_TimestampGeneralizedTimeOutsideCertificateValidityPeriod_FailAsync()
        {
            // Arrange
            var testServer = await _testFixture.GetSigningTestServerAsync();
            var certificateAuthority = await _testFixture.GetDefaultTrustedCertificateAuthorityAsync();
            var options = new TimestampServiceOptions()
            {
                IssuedCertificateNotBefore = DateTimeOffset.UtcNow.AddHours(-1),
                IssuedCertificateNotAfter = DateTimeOffset.UtcNow.AddHours(1),
                GeneralizedTime = DateTimeOffset.UtcNow.AddHours(3)
            };

            var timestampService = TimestampService.Create(certificateAuthority, options);

            VerifyTimestampData(
                testServer,
                timestampService,
                async (timestampProvider, request) =>
                {
                    var exception = await Assert.ThrowsAsync<TimestampException>(
                          () => timestampProvider.GetTimestampAsync(request, NullLogger.Instance, CancellationToken.None));

                    Assert.Equal(NuGetLogCode.NU3036, exception.Code);
                    Assert.Contains(
                        "The timestamp's generalized time is outside the timestamping certificate's validity period.",
                        exception.Message);
                });
        }

        [CIOnlyFact]
        public async Task TimestampSignatureAsync_TimestampingPrimarySignature_SuccedsAsync()
        {
            var logger = new TestLogger();
            var timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();
            var timestampProvider = new Rfc3161TimestampProvider(timestampService.Url);
            var signatureContent = new SignatureContent(SigningSpecifications.V1, Common.HashAlgorithmName.SHA256, "Test data to be signed and timestamped");

            using (var authorCert = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var timestampHashAlgorithm = Common.HashAlgorithmName.SHA256;
                var signedCms = SigningTestUtility.GenerateSignedCms(authorCert, signatureContent.GetBytes());
                var signature = PrimarySignature.Load(signedCms.Encode());
                var signatureValue = signature.GetSignatureValue();
                var messageHash = timestampHashAlgorithm.ComputeHash(signatureValue);

                var request = new TimestampRequest(
                   SigningSpecifications.V1,
                   messageHash,
                   timestampHashAlgorithm,
                   SignaturePlacement.PrimarySignature);

                // Act
                var primarySignature = await timestampProvider.TimestampSignatureAsync(signature, request, logger, CancellationToken.None);

                // Assert
                primarySignature.Should().NotBeNull();
                primarySignature.SignedCms.Should().NotBeNull();
                primarySignature.SignerInfo.Should().NotBeNull();
                primarySignature.SignerInfo.UnsignedAttributes.Count.Should().BeGreaterOrEqualTo(1);

                var hasTimestampUnsignedAttribute = false;
                var timestampCms = new SignedCms();

                foreach (var attr in primarySignature.SignerInfo.UnsignedAttributes)
                {
                    if (string.Compare(attr.Oid.Value, Oids.SignatureTimeStampTokenAttribute, CultureInfo.CurrentCulture, CompareOptions.Ordinal) == 0)
                    {
                        hasTimestampUnsignedAttribute = true;
                        timestampCms.Decode(attr.Values[0].RawData);
                    }
                }
                hasTimestampUnsignedAttribute.Should().BeTrue();

                timestampCms.CheckSignature(verifySignatureOnly: true);
            }
        }

        [CIOnlyFact]
        public async Task TimestampSignatureAsync_TimestampingCountersignature_SucceedsAsync()
        {
            var logger = new TestLogger();
            var timestampService = await _testFixture.GetDefaultTrustedTimestampServiceAsync();
            var timestampProvider = new Rfc3161TimestampProvider(timestampService.Url);
            var signatureContent = new SignatureContent(SigningSpecifications.V1, Common.HashAlgorithmName.SHA256, "Test data to be signed and timestamped");

            using (var authorCert = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var timestampHashAlgorithm = Common.HashAlgorithmName.SHA256;
                var signedCms = SigningTestUtility.GenerateRepositoryCountersignedSignedCms(authorCert, signatureContent.GetBytes());
                var signature = PrimarySignature.Load(signedCms.Encode());
                var signatureValue = signature.GetSignatureValue();
                var messageHash = timestampHashAlgorithm.ComputeHash(signatureValue);

                var request = new TimestampRequest(
                   SigningSpecifications.V1,
                   messageHash,
                   timestampHashAlgorithm,
                   SignaturePlacement.Countersignature);

                // Act
                var primarySignature = await timestampProvider.TimestampSignatureAsync(signature, request, logger, CancellationToken.None);

                var repositoryCountersignature = RepositoryCountersignature.GetRepositoryCountersignature(primarySignature);

                // Assert
                repositoryCountersignature.Should().NotBeNull();
                repositoryCountersignature.SignerInfo.Should().NotBeNull();
                repositoryCountersignature.SignerInfo.UnsignedAttributes.Count.Should().BeGreaterOrEqualTo(1);

                var hasTimestampUnsignedAttribute = false;
                var timestampCms = new SignedCms();

                foreach (var attr in repositoryCountersignature.SignerInfo.UnsignedAttributes)
                {
                    if (string.Compare(attr.Oid.Value, Oids.SignatureTimeStampTokenAttribute, CultureInfo.CurrentCulture, CompareOptions.Ordinal) == 0)
                    {
                        hasTimestampUnsignedAttribute = true;
                        timestampCms.Decode(attr.Values[0].RawData);
                    }
                }
                hasTimestampUnsignedAttribute.Should().BeTrue();

                timestampCms.CheckSignature(verifySignatureOnly: true);
            }
        }

        private void VerifyTimestampData(
            ISigningTestServer testServer,
            TimestampService timestampService,
            Action<Rfc3161TimestampProvider, TimestampRequest> verifyTimestampData)
        {
            using (testServer.RegisterResponder(timestampService))
            {
                var timestampProvider = new Rfc3161TimestampProvider(timestampService.Url);

                using (var certificate = new X509Certificate2(_trustedTestCert.Source.Cert))
                {
                    var timestampHashAlgorithm = Common.HashAlgorithmName.SHA256;
                    var content = new SignatureContent(SigningSpecifications.V1, Common.HashAlgorithmName.SHA256, "peach");
                    var signedCms = SigningTestUtility.GenerateSignedCms(certificate, content.GetBytes());
                    var signature = PrimarySignature.Load(signedCms.Encode());
                    var signatureValue = signature.GetSignatureValue();
                    var messageHash = timestampHashAlgorithm.ComputeHash(signatureValue);

                    var request = new TimestampRequest(
                       SigningSpecifications.V1,
                       messageHash,
                       timestampHashAlgorithm,
                       SignaturePlacement.PrimarySignature);

                    verifyTimestampData(timestampProvider, request);
                }
            }
        }

        private static void AssertOfflineRevocation(IEnumerable<ILogMessage> issues, LogLevel logLevel)
        {
            Assert.Contains(issues, issue =>
                issue.Code == NuGetLogCode.NU3028 &&
                issue.Level == logLevel &&
                issue.Message == "The revocation function was unable to check revocation because the revocation server was offline.");
        }

        private static void AssertRevocationStatusUnknown(IEnumerable<ILogMessage> issues, LogLevel logLevel)
        {
            Assert.Contains(issues, issue =>
                issue.Code == NuGetLogCode.NU3028 &&
                issue.Level == logLevel &&
                issue.Message == "The revocation function was unable to check revocation for the certificate.");
        }
    }
}
#endif
