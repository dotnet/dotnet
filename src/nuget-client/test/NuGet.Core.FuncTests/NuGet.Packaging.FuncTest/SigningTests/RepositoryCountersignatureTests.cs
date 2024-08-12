// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Test.Utility.Signing;
using Xunit;

namespace NuGet.Packaging.FuncTest
{
    [Collection(SigningTestCollection.Name)]
    public class RepositoryCountersignatureTests
    {
        private readonly SigningTestFixture _fixture;

        public RepositoryCountersignatureTests(SigningTestFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            _fixture = fixture;
        }

        [Fact]
        public void GetRepositoryCountersignature_WhenPrimarySignatureNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => RepositoryCountersignature.GetRepositoryCountersignature(primarySignature: null));

            Assert.Equal("primarySignature", exception.ParamName);
        }

        [Fact]
        public async Task GetRepositoryCountersignature_WithNoCountersignatures_ReturnsNull()
        {
            using (var test = await Test.CreateWithoutRepositoryCountersignatureAsync(_fixture.TrustedTestCertificate.Source.Cert))
            {
                var repositoryCountersignature = RepositoryCountersignature.GetRepositoryCountersignature(test.PrimarySignature);

                Assert.Null(repositoryCountersignature);
            }
        }

        [Fact]
        public async Task GetRepositoryCountersignature_WithRepositoryCountersignature_ReturnsInstance()
        {
            using (var test = await Test.CreateAsync(_fixture.TrustedTestCertificate.Source.Cert))
            {
                var repositoryCountersignature = RepositoryCountersignature.GetRepositoryCountersignature(test.PrimarySignature);

                Assert.NotNull(repositoryCountersignature);

                Assert.Equal(test.Request.Certificate, repositoryCountersignature.SignerInfo.Certificate);
                Assert.Equal(
                    test.Request.SignatureHashAlgorithm.ConvertToOidString(),
                    repositoryCountersignature.SignerInfo.DigestAlgorithm.Value);
                Assert.Equal(test.Request.V3ServiceIndexUrl, repositoryCountersignature.V3ServiceIndexUrl);
                Assert.Equal(test.Request.PackageOwners, repositoryCountersignature.PackageOwners);
            }
        }

        [Fact]
        public async Task GetSignatureValue_WithSha256_ReturnsValue()
        {
            using (var test = await Test.CreateAsync(_fixture.TrustedTestCertificate.Source.Cert))
            {
                var repositoryCountersignature = RepositoryCountersignature.GetRepositoryCountersignature(test.PrimarySignature);
                var actualValue = repositoryCountersignature.GetSignatureValue();
                var expectedValue = GetRepositoryCountersignatureSignatureValue(test.PrimarySignature.SignedCms);

                Assert.Equal(expectedValue, actualValue);
            }
        }

        private static byte[] GetRepositoryCountersignatureSignatureValue(SignedCms signedCms)
        {
            SignerInfo primarySignerInfo = signedCms.SignerInfos[0];
            SignerInfo counterSignerInfo = primarySignerInfo.CounterSignerInfos[0];

            return counterSignerInfo.GetSignature();
        }

        private sealed class Test : IDisposable
        {
            private bool _isDisposed;

            internal RepositorySignPackageRequest Request { get; }
            internal PrimarySignature PrimarySignature { get; }

            private Test(RepositorySignPackageRequest request, PrimarySignature primarySignature)
            {
                Request = request;
                PrimarySignature = primarySignature;
            }

            public void Dispose()
            {
                if (!_isDisposed)
                {
                    Request?.Dispose();

                    GC.SuppressFinalize(this);

                    _isDisposed = true;
                }
            }

            internal static async Task<Test> CreateAsync(
                X509Certificate2 certificate,
                Uri v3ServiceIndexUrl = null,
                IReadOnlyList<string> packageOwners = null)
            {
                v3ServiceIndexUrl = v3ServiceIndexUrl ?? new Uri("https://test.test");

                var test = await CreateWithoutRepositoryCountersignatureAsync(certificate);
                var request = new RepositorySignPackageRequest(
                    new X509Certificate2(certificate),
                    HashAlgorithmName.SHA256,
                    HashAlgorithmName.SHA256,
                    v3ServiceIndexUrl,
                    packageOwners);
                var cmsSigner = SigningUtility.CreateCmsSigner(request, NullLogger.Instance);
                var signedCms = test.PrimarySignature.SignedCms;

                signedCms.SignerInfos[0].ComputeCounterSignature(cmsSigner);

                var primarySignature = PrimarySignature.Load(signedCms.Encode());

                return new Test(request, primarySignature);
            }

            internal static async Task<Test> CreateWithoutRepositoryCountersignatureAsync(
                X509Certificate2 certificate)
            {
                var packageContext = new SimpleTestPackageContext();

                using (var directory = TestDirectory.Create())
                {
                    var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(
                        certificate,
                        packageContext,
                        directory);

                    using (var stream = File.OpenRead(signedPackagePath))
                    using (var reader = new PackageArchiveReader(stream))
                    {
                        var primarySignature = await reader.GetPrimarySignatureAsync(CancellationToken.None);

                        return new Test(request: null, primarySignature: primarySignature);
                    }
                }
            }
        }
    }
}
#endif
