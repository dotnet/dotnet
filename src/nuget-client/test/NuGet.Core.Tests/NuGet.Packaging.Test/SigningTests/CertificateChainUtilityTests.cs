// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Moq;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Test.Utility.Signing;
using Xunit;

namespace NuGet.Packaging.Test
{
    [Collection(SigningTestsCollection.Name)]
    public class CertificateChainUtilityTests
    {
        private readonly CertificatesFixture _fixture;

        public CertificateChainUtilityTests(CertificatesFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public void GetCertificateChain_WhenCertificateNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => CertificateChainUtility.GetCertificateChain(
                    certificate: null,
                    extraStore: new X509Certificate2Collection(),
                    logger: NullLogger.Instance,
                    certificateType: CertificateType.Signature));

            Assert.Equal("certificate", exception.ParamName);
        }

        [Fact]
        public void GetCertificateChain_WhenExtraStoreNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => CertificateChainUtility.GetCertificateChain(
                    new X509Certificate2(),
                    extraStore: null,
                    logger: NullLogger.Instance,
                    certificateType: CertificateType.Signature));

            Assert.Equal("extraStore", exception.ParamName);
        }

        [Fact]
        public void GetCertificateChain_WhenLoggerNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => CertificateChainUtility.GetCertificateChain(
                    new X509Certificate2(),
                    new X509Certificate2Collection(),
                    logger: null,
                    certificateType: CertificateType.Signature));

            Assert.Equal("logger", exception.ParamName);
        }

        [Fact]
        public void GetCertificateChain_WhenCertificateTypeUndefined_Throws()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => CertificateChainUtility.GetCertificateChain(
                    new X509Certificate2(),
                    new X509Certificate2Collection(),
                    NullLogger.Instance,
                    (CertificateType)int.MaxValue));

            Assert.Equal("certificateType", exception.ParamName);
        }

        [Fact]
        public void GetCertificateChain_WithUntrustedRoot_Throws()
        {
            using (X509ChainHolder chainHolder = X509ChainHolder.CreateForCodeSigning())
            using (var rootCertificate = SigningTestUtility.GetCertificate("root.crt"))
            using (var intermediateCertificate = SigningTestUtility.GetCertificate("intermediate.crt"))
            using (var leafCertificate = SigningTestUtility.GetCertificate("leaf.crt"))
            {
                var chain = chainHolder.Chain2;
                var extraStore = new X509Certificate2Collection() { rootCertificate, intermediateCertificate };
                var logger = new TestLogger();

                var exception = Assert.Throws<SignatureException>(
                    () => CertificateChainUtility.GetCertificateChain(
                        leafCertificate,
                        extraStore,
                        logger,
                        CertificateType.Signature));

                Assert.Equal(NuGetLogCode.NU3018, exception.Code);
                Assert.Equal("Certificate chain validation failed.", exception.Message);

                Assert.Equal(1, logger.Errors);
                SigningTestUtility.AssertUntrustedRoot(logger.LogMessages, LogLevel.Error);

                SigningTestUtility.AssertRevocationStatusUnknown(logger.LogMessages, LogLevel.Warning);
                if (RuntimeEnvironmentHelper.IsWindows)
                {
                    SigningTestUtility.AssertOfflineRevocation(logger.LogMessages, LogLevel.Warning);
                }
#if NETCORE5_0
                else if (RuntimeEnvironmentHelper.IsLinux)
                {
                    SigningTestUtility.AssertOfflineRevocation(logger.LogMessages, LogLevel.Warning);
                }
#endif
            }
        }

        [Fact]
        public void GetCertificateChain_WithUntrustedSelfIssuedCertificate_ReturnsChain()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var logger = new TestLogger();

                using (var chain = CertificateChainUtility.GetCertificateChain(
                    certificate,
                    new X509Certificate2Collection(),
                    logger,
                    CertificateType.Signature))
                {
                    Assert.Equal(1, chain.Count);
                    Assert.NotSame(certificate, chain[0]);
                    Assert.True(certificate.RawData.SequenceEqual(chain[0].RawData));
                }

                Assert.Equal(0, logger.Errors);
                SigningTestUtility.AssertUntrustedRoot(logger.LogMessages, LogLevel.Warning);

#if !NETCORE5_0
                if (RuntimeEnvironmentHelper.IsLinux)
                {
                    SigningTestUtility.AssertRevocationStatusUnknown(logger.LogMessages, LogLevel.Warning);
                }
#endif
            }
        }

        [Fact]
        public void GetCertificateChain_WhenCertChainNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => CertificateChainUtility.GetCertificateChain(x509Chain: null));

            Assert.Equal("x509Chain", exception.ParamName);
        }

        [Fact]
        public void GetCertificateChain_ReturnsCertificatesInOrder()
        {
            using (X509ChainHolder chainHolder = X509ChainHolder.CreateForCodeSigning())
            using (var rootCertificate = SigningTestUtility.GetCertificate("root.crt"))
            using (var intermediateCertificate = SigningTestUtility.GetCertificate("intermediate.crt"))
            using (var leafCertificate = SigningTestUtility.GetCertificate("leaf.crt"))
            {
                IX509Chain chain = chainHolder.Chain2;

                chain.ChainPolicy.ExtraStore.Add(rootCertificate);
                chain.ChainPolicy.ExtraStore.Add(intermediateCertificate);

                chain.Build(leafCertificate);

                using (IX509CertificateChain certificateChain = CertificateChainUtility.GetCertificateChain(chain.PrivateReference))
                {
                    Assert.Equal(3, certificateChain.Count);
                    Assert.Equal(leafCertificate.Thumbprint, certificateChain[0].Thumbprint);
                    Assert.Equal(intermediateCertificate.Thumbprint, certificateChain[1].Thumbprint);
                    Assert.Equal(rootCertificate.Thumbprint, certificateChain[2].Thumbprint);
                }
            }
        }

        [Fact]
        public void BuildWithPolicy_WhenChainIsNull_Throws()
        {
            using (X509Certificate2 certificate = _fixture.GetDefaultCertificate())
            {
                ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                    () => CertificateChainUtility.BuildWithPolicy(chain: null, certificate));

                Assert.Equal("chain", exception.ParamName);
            }
        }

        [Fact]
        public void BuildWithPolicy_WhenCertificateIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => CertificateChainUtility.BuildWithPolicy(Mock.Of<IX509Chain>(), certificate: null));

            Assert.Equal("certificate", exception.ParamName);
        }
    }
}
