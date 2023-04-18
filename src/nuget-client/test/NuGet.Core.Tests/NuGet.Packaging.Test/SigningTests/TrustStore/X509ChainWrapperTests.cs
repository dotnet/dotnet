// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if NET5_0_OR_GREATER

using System;
using System.Security.Cryptography.X509Certificates;
using Moq;
using NuGet.Common;
using NuGet.Packaging.Signing;
using Xunit;

namespace NuGet.Packaging.Test
{
    [Collection(SigningTestsCollection.Name)]
    public class X509ChainWrapperTests
    {
        private readonly CertificatesFixture _fixture;

        public X509ChainWrapperTests(CertificatesFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public void ConstructorWithOneParameter_WhenChainIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => new X509ChainWrapper(chain: null));

            Assert.Equal("chain", exception.ParamName);
        }

        [Fact]
        public void ConstructorWithTwoParameters_WhenChainIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => new X509ChainWrapper(chain: null, getAdditionalContext: null!));

            Assert.Equal("chain", exception.ParamName);
        }

        [Fact]
        public void Dispose_Always_CallsBaseDispose()
        {
            using (X509ChainSpy spy = new())
            {
                Assert.False(spy.IsDisposed);

                using (X509ChainWrapper wrapper = new(spy))
                {
                }

                Assert.True(spy.IsDisposed);
            }
        }

        [Fact]
        public void Build_WhenCertificateIsNull_Throws()
        {
            using (X509ChainWrapper wrapper = new(Mock.Of<X509Chain>()))
            {
                ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                   () => wrapper.Build(certificate: null!));

                Assert.Equal("certificate", exception.ParamName);
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void AdditionalContext_WhenGetAdditionalContextIsNull_ReturnsNull(bool buildResponse)
        {
            using (X509Certificate2 certificate = _fixture.GetDefaultCertificate())
            using (X509ChainWrapper wrapper = new(CreateX509Chain(certificate, buildResponse)))
            {
                Assert.Equal(buildResponse, wrapper.Build(certificate));
                Assert.Null(wrapper.AdditionalContext);
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void AdditionalContext_WhenGetAdditionalContextReturnsNull_ReturnsNull(bool buildResponse)
        {
            using (X509Certificate2 certificate = _fixture.GetDefaultCertificate())
            using (X509ChainWrapper wrapper = new(CreateX509Chain(certificate, buildResponse), chain => null))
            {
                Assert.Equal(buildResponse, wrapper.Build(certificate));
                Assert.Null(wrapper.AdditionalContext);
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void AdditionalContext_WhenGetAdditionalContextReturnsObject_ReturnsObjectOnlyIfBuildReturnsFalse(bool buildResponse)
        {
            ILogMessage logMessage = Mock.Of<ILogMessage>();

            using (X509Certificate2 certificate = _fixture.GetDefaultCertificate())
            using (X509ChainWrapper wrapper = new(CreateX509Chain(certificate, buildResponse), chain => logMessage))
            {
                Assert.Equal(buildResponse, wrapper.Build(certificate));

                if (buildResponse)
                {
                    Assert.Null(wrapper.AdditionalContext);
                }
                else
                {
                    Assert.Same(logMessage, wrapper.AdditionalContext);
                }
            }
        }

        [Fact]
        public void ChainElements_Always_ReturnsInnerChainElements()
        {
            using (X509Certificate2 certificate = _fixture.GetDefaultCertificate())
            using (X509Chain chain = CreateX509Chain(certificate, buildResponse: true))
            using (X509ChainWrapper wrapper = new(chain))
            {
                Assert.True(wrapper.Build(certificate));
                Assert.Same(chain.ChainElements, wrapper.ChainElements);
            }
        }

        [Fact]
        public void ChainPolicy_Always_ReturnsInnerChainPolicy()
        {
            using (X509Certificate2 certificate = _fixture.GetDefaultCertificate())
            using (X509Chain chain = CreateX509Chain(certificate, buildResponse: true))
            using (X509ChainWrapper wrapper = new(chain))
            {
                Assert.True(wrapper.Build(certificate));
                Assert.Same(chain.ChainPolicy, wrapper.ChainPolicy);
            }
        }

        [Fact]
        public void ChainStatus_Always_ReturnsInnerChainStatus()
        {
            using (X509Certificate2 certificate = _fixture.GetDefaultCertificate())
            using (X509Chain chain = CreateX509Chain(certificate, buildResponse: true))
            using (X509ChainWrapper wrapper = new(chain))
            {
                Assert.True(wrapper.Build(certificate));
                Assert.Same(chain.ChainStatus, wrapper.ChainStatus);
            }
        }

        [Fact]
        public void PrivateReference_Always_ReturnsInnerInstance()
        {
            using (X509Certificate2 certificate = _fixture.GetDefaultCertificate())
            using (X509Chain chain = CreateX509Chain(certificate, buildResponse: true))
            using (X509ChainWrapper wrapper = new(chain))
            {
                Assert.Same(chain, wrapper.PrivateReference);
            }
        }

        private static X509Chain CreateX509Chain(X509Certificate2 certificate, bool buildResponse)
        {
            X509Chain chain = new();

            if (buildResponse)
            {
                chain.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;
                chain.ChainPolicy.CustomTrustStore.Add(certificate);
                chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
            }

            return chain;
        }

        private sealed class X509ChainSpy : X509Chain
        {
            internal bool IsDisposed { get; private set; }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                IsDisposed = true;
            }
        }
    }
}

#endif
