// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED
using System;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using NuGet.Common;
using NuGet.Packaging.Signing;
using Xunit;

namespace NuGet.Packaging.Test
{
    [Collection(SigningTestsCollection.Name)]
    public class PrimarySignatureTests
    {
        private const string NotExactlyOnePrimarySignature = "The package signature file does not contain exactly one primary signature.";

        private readonly CertificatesFixture _fixture;

        public PrimarySignatureTests(CertificatesFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            _fixture = fixture;
        }

        [Fact]
        public void Load_WhenDataNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => PrimarySignature.Load(data: null));

            Assert.Equal("data", exception.ParamName);
        }

        [Fact]
        public void Load_WitNoPrimarySignature_Throws()
        {
            using (var test = new LoadTest(_fixture))
            {
                test.SignedCms.ComputeSignature(test.CmsSigner);
                test.SignedCms.RemoveSignature(index: 0);

                var exception = Assert.Throws<SignatureException>(
                    () => PrimarySignature.Load(test.SignedCms.Encode()));

                Assert.Equal(NuGetLogCode.NU3009, exception.Code);
                Assert.Equal(NotExactlyOnePrimarySignature, exception.Message);
            }
        }

        [Fact]
        public void Load_WithMultiplePrimarySignatures_Throws()
        {
            using (var test = new LoadTest(_fixture))
            {
                test.SignedCms.ComputeSignature(test.CmsSigner);

                var cmsSigner = new CmsSigner(test.Certificate);

                test.SignedCms.ComputeSignature(cmsSigner);

                var exception = Assert.Throws<SignatureException>(
                    () => PrimarySignature.Load(test.SignedCms.Encode()));

                Assert.Equal(NuGetLogCode.NU3009, exception.Code);
                Assert.Equal(NotExactlyOnePrimarySignature, exception.Message);
            }
        }

        [Fact]
        public void Load_WithAuthorSignatureWithoutSigningCertificateV2Attribute_Throws()
        {
            using (var test = new LoadTest(_fixture))
            {
                test.CmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateCommitmentTypeIndication(SignatureType.Author));
                test.CmsSigner.SignedAttributes.Add(new Pkcs9SigningTime());

                test.SignedCms.ComputeSignature(test.CmsSigner);

                var exception = Assert.Throws<SignatureException>(
                    () => PrimarySignature.Load(test.SignedCms.Encode()));

                Assert.Equal(NuGetLogCode.NU3011, exception.Code);
                Assert.Equal("Exactly one signing-certificate-v2 attribute is required.", exception.Message);
            }
        }

        [Fact]
        public void Load_WithAuthorSignatureWithMultipleSigningCertificateV2AttributeValues_Throws()
        {
            using (var test = new LoadTest(_fixture))
            {
                test.CmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateCommitmentTypeIndication(SignatureType.Author));
                test.CmsSigner.SignedAttributes.Add(new Pkcs9SigningTime());

                var attribute = AttributeUtility.CreateSigningCertificateV2(test.Certificate, HashAlgorithmName.SHA256);

                attribute.Values.Add(attribute.Values[0]);

                test.CmsSigner.SignedAttributes.Add(attribute);

                test.SignedCms.ComputeSignature(test.CmsSigner);

                var exception = Assert.Throws<SignatureException>(
                    () => PrimarySignature.Load(test.SignedCms.Encode()));

                Assert.Equal(NuGetLogCode.NU3011, exception.Code);
                Assert.Equal("The signing-certificate-v2 attribute must have exactly one attribute value.", exception.Message);
            }
        }

        [Fact]
        public void Load_WithAuthorSignatureWithoutPkcs9SigningTimeAttribute_Throws()
        {
            using (var test = new LoadTest(_fixture))
            {
                test.CmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateCommitmentTypeIndication(SignatureType.Author));
                test.CmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateSigningCertificateV2(test.Certificate, HashAlgorithmName.SHA256));

                test.SignedCms.ComputeSignature(test.CmsSigner);

                var exception = Assert.Throws<SignatureException>(
                    () => PrimarySignature.Load(test.SignedCms.Encode()));

                Assert.Equal(NuGetLogCode.NU3011, exception.Code);
                Assert.Equal("The primary signature is invalid.", exception.Message);
            }
        }

        [Fact]
        public void Load_WithAuthorSignature_ReturnsSignature()
        {
            using (var test = new LoadTest(_fixture))
            {
                test.CmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateCommitmentTypeIndication(SignatureType.Author));
                test.CmsSigner.SignedAttributes.Add(new Pkcs9SigningTime());
                test.CmsSigner.SignedAttributes.Add(
                    AttributeUtility.CreateSigningCertificateV2(test.Certificate, HashAlgorithmName.SHA256));

                test.SignedCms.ComputeSignature(test.CmsSigner);

                var signature = PrimarySignature.Load(test.SignedCms.Encode());

                Assert.Equal(SignatureType.Author, signature.Type);
            }
        }

        [Fact]
        public void Load_WithGenericSignature_ReturnsSignatureWithUnknownType()
        {
            using (var test = new LoadTest(_fixture))
            {
                test.SignedCms.ComputeSignature(test.CmsSigner);

                var signature = PrimarySignature.Load(test.SignedCms.Encode());

                Assert.Equal(SignatureType.Unknown, signature.Type);
            }
        }

        private sealed class LoadTest : IDisposable
        {
            private bool _isDisposed;

            internal X509Certificate2 Certificate { get; }
            internal CmsSigner CmsSigner { get; }
            internal SignedCms SignedCms { get; }

            internal LoadTest(CertificatesFixture fixture)
            {
                Certificate = fixture.GetDefaultCertificate();

                var content = new SignatureContent(
                    SigningSpecifications.V1,
                    HashAlgorithmName.SHA256,
                    hashValue: "hash");
                var contentInfo = new ContentInfo(content.GetBytes());

                SignedCms = new SignedCms(contentInfo);
                CmsSigner = new CmsSigner(Certificate);
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
