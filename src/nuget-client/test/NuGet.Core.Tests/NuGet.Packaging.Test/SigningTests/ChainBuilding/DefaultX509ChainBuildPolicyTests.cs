// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography.X509Certificates;
using Moq;
using NuGet.Packaging.Signing;
using Xunit;

namespace NuGet.Packaging.Test
{
    [Collection(SigningTestsCollection.Name)]
    public class DefaultX509ChainBuildPolicyTests
    {
        private readonly CertificatesFixture _fixture;

        public DefaultX509ChainBuildPolicyTests(CertificatesFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public void Instance_Always_IsIdempotent()
        {
            IX509ChainBuildPolicy instance0 = DefaultX509ChainBuildPolicy.Instance;
            IX509ChainBuildPolicy instance1 = DefaultX509ChainBuildPolicy.Instance;

            Assert.Same(instance0, instance1);
        }

        [Fact]
        public void Build_WhenChainIsNull_Throws()
        {
            using (X509Certificate2 certificate = _fixture.GetDefaultCertificate())
            {
                ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                    () => DefaultX509ChainBuildPolicy.Instance.Build(chain: null, certificate));

                Assert.Equal("chain", exception.ParamName);
            }
        }

        [Fact]
        public void Build_WhenCertificateIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => DefaultX509ChainBuildPolicy.Instance.Build(Mock.Of<IX509Chain>(), certificate: null));

            Assert.Equal("certificate", exception.ParamName);
        }

#if NET5_0_OR_GREATER || IS_DESKTOP
        [Fact]
        public void Build_WhenArgumentsAreValid_ReturnsExpectedResult()
        {
            using (X509ChainWrapper chain = new(new X509Chain()))
            using (X509Certificate2 expectedCertificate = _fixture.GetDefaultCertificate())
            {
                bool actualResult = DefaultX509ChainBuildPolicy.Instance.Build(chain, expectedCertificate);

                Assert.False(actualResult);
                Assert.Equal(X509ChainStatusFlags.UntrustedRoot, chain.ChainStatus[0].Status);
            }
        }
#endif
    }
}
