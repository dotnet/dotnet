// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Packaging.Signing.DerEncoding;
using Xunit;

namespace NuGet.Packaging.Test
{
    [Collection(SigningTestsCollection.Name)]
    public class RepositoryCountersignatureTests
    {
        private readonly CertificatesFixture _fixture;

        public RepositoryCountersignatureTests(CertificatesFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            _fixture = fixture;
        }

        [Fact]
        public void GetRepositoryCountersignature_WithoutCountersignature_ReturnsNull()
        {
            using (var test = new Test(_fixture))
            {
                test.CreateValidAuthorPrimarySignature();
                var primarySignature = PrimarySignature.Load(test.PrimarySignedCms.Encode());

                // Validate countersignature
                var countersignature = RepositoryCountersignature.GetRepositoryCountersignature(primarySignature);

                Assert.Null(countersignature);
            }
        }

        [Fact]
        public void GetRepositoryCountersignature_WithNonRepositoryCountersignature_ReturnsNull()
        {
            using (var test = new Test(_fixture))
            {
                test.CreateValidAuthorPrimarySignature();

                // Generate valid countersignature
                test.CounterCmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateNuGetV3ServiceIndexUrl(new Uri("https://api.nuget.org/v3/index.json")));
                test.CounterCmsSigner.SignedAttributes.Add(
                    new Pkcs9SigningTime());
                test.CounterCmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateSigningCertificateV2(test.Certificate, HashAlgorithmName.SHA256));

                // Create counter signature
                test.PrimarySignedCms.SignerInfos[0].ComputeCounterSignature(test.CounterCmsSigner);

                // Load primary signature
                var primarySignature = PrimarySignature.Load(test.PrimarySignedCms.Encode());

                // Validate countersignature
                var countersignature = RepositoryCountersignature.GetRepositoryCountersignature(primarySignature);

                Assert.Null(countersignature);
            }
        }

        [Fact]
        public void GetRepositoryCountersignature_WithRepositoryPrimarySignature_Throws()
        {
            using (var test = new Test(_fixture))
            {
                test.CreateValidRepositoryPrimarySignature();

                // Generate valid countersignature
                test.CounterCmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateCommitmentTypeIndication(SignatureType.Repository));
                test.CounterCmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateNuGetV3ServiceIndexUrl(new Uri("https://api.nuget.org/v3/index.json")));
                test.CounterCmsSigner.SignedAttributes.Add(
                    new Pkcs9SigningTime());
                test.CounterCmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateSigningCertificateV2(test.Certificate, HashAlgorithmName.SHA256));

                // Create counter signature
                test.PrimarySignedCms.SignerInfos[0].ComputeCounterSignature(test.CounterCmsSigner);

                // Load primary signature
                var primarySignature = PrimarySignature.Load(test.PrimarySignedCms.Encode());

                // Validate countersignature
                var exception = Assert.Throws<SignatureException>(
                     () => RepositoryCountersignature.GetRepositoryCountersignature(primarySignature));

                Assert.Equal(NuGetLogCode.NU3033, exception.Code);
                Assert.Equal("A repository primary signature must not have a repository countersignature.", exception.Message);
            }
        }

        [Fact]
        public void GetRepositoryCountersignature_WithMultipleRepositoryCountersignatures_Throws()
        {
            using (var test = new Test(_fixture))
            {
                test.CreateValidAuthorPrimarySignature();

                // Generate valid countersignature
                test.CounterCmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateCommitmentTypeIndication(SignatureType.Repository));
                test.CounterCmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateNuGetV3ServiceIndexUrl(new Uri("https://api.nuget.org/v3/index.json")));
                test.CounterCmsSigner.SignedAttributes.Add(
                    new Pkcs9SigningTime());
                test.CounterCmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateSigningCertificateV2(test.Certificate, HashAlgorithmName.SHA256));

                // Create counter signature
                test.PrimarySignedCms.SignerInfos[0].ComputeCounterSignature(test.CounterCmsSigner);

                // Second countersignature signer
                var SecondCounterCmsSigner = new CmsSigner(test.Certificate);

                // Generate valid countersignature
                SecondCounterCmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateCommitmentTypeIndication(SignatureType.Repository));
                SecondCounterCmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateNuGetV3ServiceIndexUrl(new Uri("https://api.nuget.org/v3/index.json")));
                SecondCounterCmsSigner.SignedAttributes.Add(
                    new Pkcs9SigningTime());
                SecondCounterCmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateSigningCertificateV2(test.Certificate, HashAlgorithmName.SHA256));

                // Create counter signature
                test.PrimarySignedCms.SignerInfos[0].ComputeCounterSignature(SecondCounterCmsSigner);

                // Load primary signature
                var primarySignature = PrimarySignature.Load(test.PrimarySignedCms.Encode());

                // Validate countersignature
                var exception = Assert.Throws<SignatureException>(
                     () => RepositoryCountersignature.GetRepositoryCountersignature(primarySignature));

                Assert.Equal(NuGetLogCode.NU3032, exception.Code);
                Assert.Equal("The package signature contains multiple repository countersignatures.", exception.Message);
            }
        }

        [Fact]
        public void GetRepositoryCountersignature_WithRepositoryCountersignatureWithoutV3ServiceIndexUrl_Throws()
        {
            using (var test = new Test(_fixture))
            {
                test.CreateValidAuthorPrimarySignature();

                // Generate countersignature
                test.CounterCmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateCommitmentTypeIndication(SignatureType.Repository));
                test.CounterCmsSigner.SignedAttributes.Add(
                    new Pkcs9SigningTime());
                test.CounterCmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateSigningCertificateV2(test.Certificate, HashAlgorithmName.SHA256));

                // Create counter signature
                test.PrimarySignedCms.SignerInfos[0].ComputeCounterSignature(test.CounterCmsSigner);

                // Load primary signature
                var primarySignature = PrimarySignature.Load(test.PrimarySignedCms.Encode());

                // Validate countersignature
                var exception = Assert.Throws<SignatureException>(
                     () => RepositoryCountersignature.GetRepositoryCountersignature(primarySignature));

                Assert.Equal(NuGetLogCode.NU3000, exception.Code);
                Assert.Equal("Exactly one nuget-v3-service-index-url attribute is required.", exception.Message);
            }
        }

        [Fact]
        public void GetRepositoryCountersignature_WithRepositoryCountersignatureWithInvalidV3ServiceIndexUrl_Throws()
        {
            using (var test = new Test(_fixture))
            {
                test.CreateValidAuthorPrimarySignature();

                // Generate countersignature
                test.CounterCmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateCommitmentTypeIndication(SignatureType.Repository));

                var invalidV3ServiceIndexBytes = DerEncoder.SegmentedEncodeIA5String("notAValidUri".ToCharArray())
                    .SelectMany(x => x)
                    .ToArray();

                var invalidV3ServiceIndex = new System.Security.Cryptography.CryptographicAttributeObject(
                    new System.Security.Cryptography.Oid(Oids.NuGetV3ServiceIndexUrl),
                    new System.Security.Cryptography.AsnEncodedDataCollection(new System.Security.Cryptography.AsnEncodedData(Oids.NuGetV3ServiceIndexUrl, invalidV3ServiceIndexBytes)));

                test.CounterCmsSigner.SignedAttributes.Add(
                    invalidV3ServiceIndex);
                test.CounterCmsSigner.SignedAttributes.Add(
                    new Pkcs9SigningTime());
                test.CounterCmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateSigningCertificateV2(test.Certificate, HashAlgorithmName.SHA256));

                // Create counter signature
                test.PrimarySignedCms.SignerInfos[0].ComputeCounterSignature(test.CounterCmsSigner);

                // Load primary signature
                var primarySignature = PrimarySignature.Load(test.PrimarySignedCms.Encode());

                // Validate countersignature
                var exception = Assert.Throws<SignatureException>(
                     () => RepositoryCountersignature.GetRepositoryCountersignature(primarySignature));

                Assert.Equal(NuGetLogCode.NU3000, exception.Code);
                Assert.Equal("The nuget-v3-service-index-url attribute is invalid.", exception.Message);
            }
        }

        [Fact]
        public void GetRepositoryCountersignature_WithRepositoryCountersignatureWithInvalidPackageOwners_Throws()
        {
            using (var test = new Test(_fixture))
            {
                test.CreateValidAuthorPrimarySignature();

                // Generate countersignature
                test.CounterCmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateCommitmentTypeIndication(SignatureType.Repository));

                var invalidPackageOwnersBytes = DerEncoder.ConstructSequence(
                        (new List<string> { "", "   " }).Select(packageOwner => DerEncoder.SegmentedEncodeUtf8String(packageOwner.ToCharArray())));

                var invalidPackageOwners = new System.Security.Cryptography.CryptographicAttributeObject(
                    new System.Security.Cryptography.Oid(Oids.NuGetPackageOwners),
                    new System.Security.Cryptography.AsnEncodedDataCollection(new System.Security.Cryptography.AsnEncodedData(Oids.NuGetPackageOwners, invalidPackageOwnersBytes)));

                test.CounterCmsSigner.SignedAttributes.Add(
                    invalidPackageOwners);
                test.CounterCmsSigner.SignedAttributes.Add(
                     AttributeUtility.CreateNuGetV3ServiceIndexUrl(new Uri("https://api.nuget.org/v3/index.json")));
                test.CounterCmsSigner.SignedAttributes.Add(
                    new Pkcs9SigningTime());
                test.CounterCmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateSigningCertificateV2(test.Certificate, HashAlgorithmName.SHA256));

                // Create counter signature
                test.PrimarySignedCms.SignerInfos[0].ComputeCounterSignature(test.CounterCmsSigner);

                // Load primary signature
                var primarySignature = PrimarySignature.Load(test.PrimarySignedCms.Encode());

                // Validate countersignature
                var exception = Assert.Throws<SignatureException>(
                     () => RepositoryCountersignature.GetRepositoryCountersignature(primarySignature));

                Assert.Equal(NuGetLogCode.NU3000, exception.Code);
                Assert.Equal("The nuget-package-owners attribute is invalid.", exception.Message);
            }
        }

        [Fact]
        public void GetRepositoryCountersignature_WithRepositoryCountersignatureWithoutPackageOwners_ReturnsRepositoryCountersignature()
        {
            using (var test = new Test(_fixture))
            {
                test.CreateValidAuthorPrimarySignature();

                // Generate valid countersignature
                test.CounterCmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateCommitmentTypeIndication(SignatureType.Repository));
                test.CounterCmsSigner.SignedAttributes.Add(
                     AttributeUtility.CreateNuGetV3ServiceIndexUrl(new Uri("https://api.nuget.org/v3/index.json")));
                test.CounterCmsSigner.SignedAttributes.Add(
                    new Pkcs9SigningTime());
                test.CounterCmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateSigningCertificateV2(test.Certificate, HashAlgorithmName.SHA256));

                // Create counter signature
                test.PrimarySignedCms.SignerInfos[0].ComputeCounterSignature(test.CounterCmsSigner);

                // Load primary signature
                var primarySignature = PrimarySignature.Load(test.PrimarySignedCms.Encode());

                // Validate countersignature
                var countersignature = RepositoryCountersignature.GetRepositoryCountersignature(primarySignature);

                Assert.Equal(SignatureType.Repository, countersignature.Type);
            }
        }

        [Fact]
        public void GetRepositoryCountersignature_WithValidRepositoryCountersignature_ReturnsRepositoryCountersignature()
        {
            using (var test = new Test(_fixture))
            {
                test.CreateValidAuthorPrimarySignature();

                // Generate valid countersignature
                test.CounterCmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateCommitmentTypeIndication(SignatureType.Repository));
                test.CounterCmsSigner.SignedAttributes.Add(
                     AttributeUtility.CreateNuGetV3ServiceIndexUrl(new Uri("https://api.nuget.org/v3/index.json")));
                test.CounterCmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateNuGetPackageOwners(new List<string> { "microsoft", "nuget" }));
                test.CounterCmsSigner.SignedAttributes.Add(
                    new Pkcs9SigningTime());
                test.CounterCmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateSigningCertificateV2(test.Certificate, HashAlgorithmName.SHA256));

                // Create counter signature
                test.PrimarySignedCms.SignerInfos[0].ComputeCounterSignature(test.CounterCmsSigner);

                // Load primary signature
                var primarySignature = PrimarySignature.Load(test.PrimarySignedCms.Encode());

                // Validate countersignature
                var countersignature = RepositoryCountersignature.GetRepositoryCountersignature(primarySignature);

                Assert.Equal(SignatureType.Repository, countersignature.Type);
            }
        }

        private sealed class Test : IDisposable
        {
            private bool _isDisposed;

            internal X509Certificate2 Certificate { get; }
            internal CmsSigner CounterCmsSigner { get; }

            internal SignedCms PrimarySignedCms { get; }

            internal Test(CertificatesFixture fixture)
            {
                Certificate = fixture.GetDefaultCertificate();

                var content = new SignatureContent(
                    SigningSpecifications.V1,
                    HashAlgorithmName.SHA256,
                    hashValue: "hash");
                var contentInfo = new ContentInfo(content.GetBytes());

                PrimarySignedCms = new SignedCms(contentInfo);
                CounterCmsSigner = new CmsSigner(Certificate);
            }

            internal void CreateValidAuthorPrimarySignature()
            {
                var PrimaryCmsSigner = new CmsSigner(Certificate);

                // Generate valid primary signature
                PrimaryCmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateCommitmentTypeIndication(SignatureType.Author));
                PrimaryCmsSigner.SignedAttributes.Add(
                    new Pkcs9SigningTime());
                PrimaryCmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateSigningCertificateV2(Certificate, HashAlgorithmName.SHA256));

                // Create primary signature
                PrimarySignedCms.ComputeSignature(PrimaryCmsSigner);
            }

            internal void CreateValidRepositoryPrimarySignature()
            {
                var PrimaryCmsSigner = new CmsSigner(Certificate);

                // Generate valid primary signature
                PrimaryCmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateCommitmentTypeIndication(SignatureType.Repository));
                PrimaryCmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateNuGetV3ServiceIndexUrl(new Uri("https://api.nuget.org/v3/index.json")));
                PrimaryCmsSigner.SignedAttributes.Add(
                    new Pkcs9SigningTime());
                PrimaryCmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateSigningCertificateV2(Certificate, HashAlgorithmName.SHA256));

                // Create primary signature
                PrimarySignedCms.ComputeSignature(PrimaryCmsSigner);
            }

            public void Dispose()
            {
                if (!_isDisposed)
                {
                    Certificate.Dispose();

                    GC.SuppressFinalize(this);

                    _isDisposed = true;
                }
            }
        }
    }
}
#endif
