// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if NET5_0_OR_GREATER

using System;
using System.Security.Cryptography.X509Certificates;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Packaging.FuncTest.SigningTests
{
    [Collection(SigningTestCollection.Name)]
    public class DotNetDefaultTrustStoreX509ChainFactoryTests
    {
        private readonly SigningTestFixture _fixture;

        public DotNetDefaultTrustStoreX509ChainFactoryTests(SigningTestFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [CIOnlyFact]
        public void AdditionalContext_WhenRootCertificateIsUntrusted_ReturnsNull()
        {
            DotNetDefaultTrustStoreX509ChainFactory factory = new();

            using (IX509Chain chain = factory.Create())
            {
                X509Certificate2 certificate = _fixture.UntrustedTestCertificate.Cert;

                Assert.False(chain.Build(certificate));
                Assert.Null(chain.AdditionalContext);
            }
        }
    }
}

#endif
