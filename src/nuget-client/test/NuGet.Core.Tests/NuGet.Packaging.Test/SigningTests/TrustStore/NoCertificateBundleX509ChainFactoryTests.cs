// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if NET5_0_OR_GREATER

using System.Security.Cryptography.X509Certificates;
using NuGet.Packaging.Signing;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class NoCertificateBundleX509ChainFactoryTests
    {
        private readonly NoCertificateBundleX509ChainFactory _factory;

        public NoCertificateBundleX509ChainFactoryTests()
        {
            _factory = new NoCertificateBundleX509ChainFactory();
        }

        [Fact]
        public void Certificates_Always_IsEmpty()
        {
            Assert.Empty(_factory.Certificates);
        }

        [Fact]
        public void FilePath_Always_IsNull()
        {
            Assert.Null(_factory.FilePath);
        }

        [Fact]
        public void Create_Always_ReturnsInstance()
        {
            using (IX509Chain chain = _factory.Create())
            {
                Assert.Equal(X509ChainTrustMode.CustomRootTrust, chain.ChainPolicy.TrustMode);
                Assert.Empty(chain.ChainPolicy.CustomTrustStore);
            }
        }
    }
}
#endif
