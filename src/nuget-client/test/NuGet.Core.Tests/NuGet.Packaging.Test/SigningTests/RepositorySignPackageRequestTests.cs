// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Packaging.Test
{
    [Collection(SigningTestsCollection.Name)]
    public class RepositorySignPackageRequestTests
    {
        private readonly CertificatesFixture _fixture;
        private static readonly Uri _validV3ServiceIndexUrl = new Uri("https://test.test", UriKind.Absolute);
        private static readonly IReadOnlyList<string> _validPackageOwners = new[] { "a", "b", "c" };

        public RepositorySignPackageRequestTests(CertificatesFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            _fixture = fixture;
        }

        [Fact]
        public void Constructor_WhenCertificateNull_Throws()
        {
            X509Certificate2 certificate = null;

            var exception = Assert.Throws<ArgumentNullException>(
                () => new RepositorySignPackageRequest(
                    certificate,
                    HashAlgorithmName.SHA256,
                    HashAlgorithmName.SHA256,
                    new Uri("https://test.test"),
                    packageOwners: null));

            Assert.Equal("certificate", exception.ParamName);
        }

        [Theory]
        [InlineData(HashAlgorithmName.Unknown)]
        [InlineData((HashAlgorithmName)int.MinValue)]
        public void Constructor_WithInvalidSignatureHashAlgorithm_Throws(HashAlgorithmName signatureHashAlgorithm)
        {
            using (var certificate = new X509Certificate2())
            {
                var exception = Assert.Throws<ArgumentException>(
                    () => new RepositorySignPackageRequest(
                        certificate,
                        signatureHashAlgorithm,
                        HashAlgorithmName.SHA256,
                        _validV3ServiceIndexUrl,
                        _validPackageOwners));

                Assert.Equal("signatureHashAlgorithm", exception.ParamName);
            }
        }

        [Theory]
        [InlineData(HashAlgorithmName.Unknown)]
        [InlineData((HashAlgorithmName)int.MinValue)]
        public void Constructor_WithInvalidTimestampHashAlgorithm_Throws(HashAlgorithmName timestampHashAlgorithm)
        {
            using (var certificate = new X509Certificate2())
            {
                var exception = Assert.Throws<ArgumentException>(
                    () => new RepositorySignPackageRequest(
                        certificate,
                        HashAlgorithmName.SHA256,
                        timestampHashAlgorithm,
                        _validV3ServiceIndexUrl,
                        _validPackageOwners));

                Assert.Equal("timestampHashAlgorithm", exception.ParamName);
            }
        }


        [Fact]
        public void Constructor_WhenV3ServiceIndexUrlNull_Throws()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => new RepositorySignPackageRequest(
                        certificate,
                        HashAlgorithmName.SHA256,
                        HashAlgorithmName.SHA256,
                        v3ServiceIndexUrl: null,
                        packageOwners: _validPackageOwners));

                Assert.Equal("v3ServiceIndexUrl", exception.ParamName);
            }
        }

        [Fact]
        public void Constructor_WhenV3ServiceIndexUrlNotAbsolute_Throws()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var exception = Assert.Throws<ArgumentException>(
                    () => new RepositorySignPackageRequest(
                        certificate,
                        HashAlgorithmName.SHA256,
                        HashAlgorithmName.SHA256,
                        new Uri("/", UriKind.Relative),
                        _validPackageOwners));

                Assert.Equal("v3ServiceIndexUrl", exception.ParamName);
                Assert.StartsWith("The URL value is invalid.", exception.Message);
            }
        }

        [Fact]
        public void Constructor_WhenV3ServiceIndexUrlNotHttps_Throws()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var exception = Assert.Throws<ArgumentException>(
                    () => new RepositorySignPackageRequest(
                        certificate,
                        HashAlgorithmName.SHA256,
                        HashAlgorithmName.SHA256,
                        new Uri("http://test.test", UriKind.Absolute),
                        _validPackageOwners));

                Assert.Equal("v3ServiceIndexUrl", exception.ParamName);
                Assert.StartsWith("The URL value is invalid.", exception.Message);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_WhenPackageOwnersContainsInvalidValue_Throws(string packageOwner)
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var exception = Assert.Throws<ArgumentException>(
                    () => new RepositorySignPackageRequest(
                        certificate,
                        HashAlgorithmName.SHA256,
                        HashAlgorithmName.SHA256,
                        _validV3ServiceIndexUrl,
                        new string[] { packageOwner }));

                Assert.Equal("packageOwners", exception.ParamName);
                Assert.StartsWith("One or more package owner values are invalid.", exception.Message);
            }
        }

        [Fact]
        public void Constructor_WithValidInput_InitializesProperties()
        {
            using (var certificate = new X509Certificate2())
            using (var request = new RepositorySignPackageRequest(
                certificate,
                HashAlgorithmName.SHA256,
                HashAlgorithmName.SHA384,
                _validV3ServiceIndexUrl,
                _validPackageOwners))
            {
                Assert.Equal(SignatureType.Repository, request.SignatureType);
                Assert.Same(certificate, request.Certificate);
                Assert.Equal(HashAlgorithmName.SHA256, request.SignatureHashAlgorithm);
                Assert.Equal(HashAlgorithmName.SHA384, request.TimestampHashAlgorithm);
                Assert.Equal(_validV3ServiceIndexUrl.OriginalString, request.V3ServiceIndexUrl.OriginalString);
                Assert.Equal(_validPackageOwners, request.PackageOwners);
            }
        }
    }
}
