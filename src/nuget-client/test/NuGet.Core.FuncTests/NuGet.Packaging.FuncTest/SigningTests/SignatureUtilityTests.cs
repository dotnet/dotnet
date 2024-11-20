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
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Test.Utility.Signing;
using Xunit;

namespace NuGet.Packaging.FuncTest.SigningTests
{
    [Collection(SigningTestCollection.Name)]
    public class SignatureUtilityTests
    {
        private readonly SigningTestFixture _fixture;
        private const int SHA1HashLength = 20;

        public SignatureUtilityTests(SigningTestFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

#if IS_DESKTOP
        [Fact]
        public async Task GetTimestampCertificateChain_WithNoSigningCertificateUsage_Throws()
        {
            ISigningTestServer testServer = await _fixture.GetSigningTestServerAsync();
            CertificateAuthority rootCa = await _fixture.GetDefaultTrustedCertificateAuthorityAsync();
            var options = new TimestampServiceOptions()
            {
                SigningCertificateUsage = SigningCertificateUsage.None
            };
            TimestampService timestampService = TimestampService.Create(rootCa, options);

            using (testServer.RegisterResponder(timestampService))
            {
                var nupkg = new SimpleTestPackageContext();

                using (var certificate = new X509Certificate2(_fixture.TrustedTestCertificate.Source.Cert))
                using (var directory = TestDirectory.Create())
                {
                    var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                        certificate,
                        nupkg,
                        directory,
                        timestampService.Url);

                    using (FileStream stream = File.OpenRead(signedPackagePath))
                    using (var reader = new PackageArchiveReader(stream))
                    {
                        PrimarySignature signature = await reader.GetPrimarySignatureAsync(CancellationToken.None);

                        var exception = Assert.Throws<SignatureException>(
                            () => SignatureUtility.GetTimestampCertificateChain(signature));

                        Assert.Equal(
                            "Either the signing-certificate or signing-certificate-v2 attribute must be present.",
                            exception.Message);
                    }
                }
            }
        }

        [Theory]
        [InlineData(SigningCertificateUsage.V1)]
        public async Task GetTimestampCertificateChain_WithShortEssCertIdCertificateHash_Throws(
            SigningCertificateUsage signingCertificateUsage)
        {
            ISigningTestServer testServer = await _fixture.GetSigningTestServerAsync();
            CertificateAuthority rootCa = await _fixture.GetDefaultTrustedCertificateAuthorityAsync();
            var options = new TimestampServiceOptions()
            {
                SigningCertificateUsage = signingCertificateUsage,
                SigningCertificateV1Hash = new byte[SHA1HashLength - 1]
            };
            TimestampService timestampService = TimestampService.Create(rootCa, options);

            using (testServer.RegisterResponder(timestampService))
            {
                var nupkg = new SimpleTestPackageContext();

                using (var certificate = new X509Certificate2(_fixture.TrustedTestCertificate.Source.Cert))
                using (var directory = TestDirectory.Create())
                {
                    var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                        certificate,
                        nupkg,
                        directory,
                        timestampService.Url);

                    using (FileStream stream = File.OpenRead(signedPackagePath))
                    using (var reader = new PackageArchiveReader(stream))
                    {
                        PrimarySignature signature = await reader.GetPrimarySignatureAsync(CancellationToken.None);

                        var exception = Assert.Throws<SignatureException>(
                            () => SignatureUtility.GetTimestampCertificateChain(signature));

                        Assert.Equal(
                            "A certificate referenced by the signing-certificate attribute could not be found.",
                            exception.Message);
                    }
                }
            }
        }

        [Theory]
        [InlineData(SigningCertificateUsage.V1)]
        public async Task GetTimestampCertificateChain_WithMismatchedEssCertIdCertificateHash_ReturnsChain(
            SigningCertificateUsage signingCertificateUsage)
        {
            ISigningTestServer testServer = await _fixture.GetSigningTestServerAsync();
            CertificateAuthority rootCa = await _fixture.GetDefaultTrustedCertificateAuthorityAsync();
            var options = new TimestampServiceOptions()
            {
                SigningCertificateUsage = signingCertificateUsage,
                SigningCertificateV1Hash = new byte[SHA1HashLength]
            };
            TimestampService timestampService = TimestampService.Create(rootCa, options);

            using (testServer.RegisterResponder(timestampService))
            {
                var nupkg = new SimpleTestPackageContext();

                using (var certificate = new X509Certificate2(_fixture.TrustedTestCertificate.Source.Cert))
                using (var directory = TestDirectory.Create())
                {
                    var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                        certificate,
                        nupkg,
                        directory,
                        timestampService.Url);

                    using (FileStream stream = File.OpenRead(signedPackagePath))
                    using (var reader = new PackageArchiveReader(stream))
                    {
                        PrimarySignature signature = await reader.GetPrimarySignatureAsync(CancellationToken.None);

                        using (IX509CertificateChain actualChain = SignatureUtility.GetTimestampCertificateChain(signature))
                        {
                            Assert.NotEmpty(actualChain);

                            IReadOnlyList<X509Certificate2> expectedChain = GetExpectedCertificateChain(timestampService);

                            Assert.Equal(expectedChain.Count, actualChain.Count);

                            for (var i = 0; i < expectedChain.Count; ++i)
                            {
                                X509Certificate2 expectedCertificate = expectedChain[i];
                                X509Certificate2 actualCertificate = actualChain[i];

                                Assert.True(
                                    expectedCertificate.RawData.SequenceEqual(actualCertificate.RawData),
                                    $"The certificate at index {i} in the chain is unexpected.");
                            }
                        }
                    }
                }
            }
        }
#endif

        [Theory]
        [InlineData(SigningCertificateUsage.V1)]
        [InlineData(SigningCertificateUsage.V2)]
        [InlineData(SigningCertificateUsage.V1 | SigningCertificateUsage.V2)]
        public async Task GetTimestampCertificateChain_WithValidSigningCertificateUsage_ReturnsChain(
            SigningCertificateUsage signingCertificateUsage)
        {
            ISigningTestServer testServer = await _fixture.GetSigningTestServerAsync();
            CertificateAuthority rootCa = await _fixture.GetDefaultTrustedCertificateAuthorityAsync();
            var options = new TimestampServiceOptions()
            {
                SigningCertificateUsage = signingCertificateUsage
            };
            TimestampService timestampService = TimestampService.Create(rootCa, options);

            using (testServer.RegisterResponder(timestampService))
            {
                var nupkg = new SimpleTestPackageContext();

                using (var certificate = new X509Certificate2(_fixture.TrustedTestCertificate.Source.Cert))
                using (var directory = TestDirectory.Create())
                {
                    var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                        certificate,
                        nupkg,
                        directory,
                        timestampService.Url);

                    using (FileStream stream = File.OpenRead(signedPackagePath))
                    using (var reader = new PackageArchiveReader(stream))
                    {
                        PrimarySignature signature = await reader.GetPrimarySignatureAsync(CancellationToken.None);

                        using (IX509CertificateChain actualChain = SignatureUtility.GetTimestampCertificateChain(signature))
                        {
                            Assert.NotEmpty(actualChain);

                            IReadOnlyList<X509Certificate2> expectedChain = GetExpectedCertificateChain(timestampService);

                            Assert.Equal(expectedChain.Count, actualChain.Count);

                            for (var i = 0; i < expectedChain.Count; ++i)
                            {
                                X509Certificate2 expectedCertificate = expectedChain[i];
                                X509Certificate2 actualCertificate = actualChain[i];

                                Assert.True(
                                    expectedCertificate.RawData.SequenceEqual(actualCertificate.RawData),
                                    $"The certificate at index {i} in the chain is unexpected.");
                            }
                        }
                    }
                }
            }
        }

        private static IReadOnlyList<X509Certificate2> GetExpectedCertificateChain(TimestampService timestampService)
        {
            var expectedChain = new List<X509Certificate2>();

            expectedChain.Add(timestampService.Certificate);

            CertificateAuthority ca = timestampService.CertificateAuthority;

            while (ca != null)
            {
                expectedChain.Add(ca.Certificate);

                ca = ca.Parent;
            }

            return expectedChain;
        }
    }
}
#endif
