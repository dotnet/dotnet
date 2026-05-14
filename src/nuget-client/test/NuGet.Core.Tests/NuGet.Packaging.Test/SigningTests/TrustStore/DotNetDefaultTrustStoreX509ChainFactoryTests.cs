// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if NET5_0_OR_GREATER

using System.Security.Cryptography.X509Certificates;
using NuGet.Packaging.Signing;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class DotNetDefaultTrustStoreX509ChainFactoryTests
    {
        [Fact]
        public void Create_Always_ReturnsInstance()
        {
            DotNetDefaultTrustStoreX509ChainFactory factory = new();

            using (IX509Chain chain = factory.Create())
            {
                Assert.Equal(X509ChainTrustMode.System, chain.ChainPolicy.TrustMode);
            }
        }
    }
}
#endif
