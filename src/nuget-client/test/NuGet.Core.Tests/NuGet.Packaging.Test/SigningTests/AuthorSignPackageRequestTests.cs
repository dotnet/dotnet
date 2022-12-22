// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Packaging.Test
{
    [Collection(SigningTestsCollection.Name)]
    public class AuthorSignPackageRequestTests
    {
        private readonly CertificatesFixture _fixture;

        public AuthorSignPackageRequestTests(CertificatesFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            _fixture = fixture;
        }

        [Fact]
        public void Constructor_CertificateHashAlgorithm_WhenCertificateNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new AuthorSignPackageRequest(certificate: null, hashAlgorithm: HashAlgorithmName.SHA256));

            Assert.Equal("certificate", exception.ParamName);
        }

        [Fact]
        public void Constructor_CertificateHashAlgorithm_WhenHashAlgorithmInvalid_Throws()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var exception = Assert.Throws<ArgumentException>(
                    () => new AuthorSignPackageRequest(certificate, HashAlgorithmName.Unknown));

                Assert.Equal("signatureHashAlgorithm", exception.ParamName);
            }
        }

        [Fact]
        public void Constructor_CertificateHashAlgorithm_WithValidInput_InitializesProperties()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var request = new AuthorSignPackageRequest(certificate, HashAlgorithmName.SHA512);

                Assert.Equal(SignatureType.Author, request.SignatureType);
                Assert.Same(certificate, request.Certificate);
                Assert.Equal(HashAlgorithmName.SHA512, request.SignatureHashAlgorithm);
                Assert.Equal(HashAlgorithmName.SHA512, request.TimestampHashAlgorithm);
            }
        }

        [Fact]
        public void Constructor_CertificateSignatureHashAlgorithmTimestampHashAlgorithm_WhenCertificateNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new AuthorSignPackageRequest(
                    certificate: null,
                    signatureHashAlgorithm: HashAlgorithmName.SHA256,
                    timestampHashAlgorithm: HashAlgorithmName.SHA256));

            Assert.Equal("certificate", exception.ParamName);
        }

        [Fact]
        public void Constructor_CertificateSignatureHashAlgorithmTimestampHashAlgorithm_WhenSignatureHashAlgorithmInvalid_Throws()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var exception = Assert.Throws<ArgumentException>(
                    () => new AuthorSignPackageRequest(
                        certificate,
                        HashAlgorithmName.Unknown,
                        HashAlgorithmName.SHA256));

                Assert.Equal("signatureHashAlgorithm", exception.ParamName);
                Assert.StartsWith("The argument is invalid.", exception.Message);
            }
        }

        [Fact]
        public void Constructor_CertificateSignatureHashAlgorithmTimestampHashAlgorithm_WhenTimestampHashAlgorithmInvalid_Throws()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var exception = Assert.Throws<ArgumentException>(
                    () => new AuthorSignPackageRequest(
                        certificate,
                        HashAlgorithmName.SHA256,
                        HashAlgorithmName.Unknown));

                Assert.Equal("timestampHashAlgorithm", exception.ParamName);
                Assert.StartsWith("The argument is invalid.", exception.Message);
            }
        }

        [Fact]
        public void Constructor_CertificateSignatureHashAlgorithmTimestampHashAlgorithm_WithValidInput_InitializesProperties()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var request = new AuthorSignPackageRequest(certificate, HashAlgorithmName.SHA512, HashAlgorithmName.SHA256);

                Assert.Equal(SignatureType.Author, request.SignatureType);
                Assert.Same(certificate, request.Certificate);
                Assert.Equal(HashAlgorithmName.SHA512, request.SignatureHashAlgorithm);
                Assert.Equal(HashAlgorithmName.SHA256, request.TimestampHashAlgorithm);
            }
        }
    }
}
