// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if NET5_0_OR_GREATER

using System.Security.Cryptography.X509Certificates;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Xunit;

using X509StorePurpose = Microsoft.Internal.NuGet.Testing.SignedPackages.X509StorePurpose;

namespace NuGet.Packaging.FuncTest.SigningTests
{
    [Collection(SigningTestCollection.Name)]
    public class InMemoryTrustStoreTests
    {
        private readonly SigningTestFixture _fixture;

        public InMemoryTrustStoreTests(SigningTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void ChainBuilding_WithInMemoryTrustStore_SucceedsWithoutElevation()
        {
            TestFallbackCertificateBundleX509ChainFactories.SetTryUseAsDefault(tryUseAsDefault: true);

            try
            {
                TestCertificate testCert = TestCertificate.Generate(
                    X509StorePurpose.CodeSigning,
                    SigningTestUtility.CertificateModificationGeneratorForCodeSigningEkuCert);

                using (TrustedTestCert<TestCertificate> trustedCert = testCert.WithTrust())
                {
                    IX509ChainFactory factory = X509TrustStore.GetX509ChainFactory(
                        Signing.X509StorePurpose.CodeSigning, NullLogger.Instance);

                    using (IX509Chain chain = factory.Create())
                    {
                        Assert.Equal(X509ChainTrustMode.CustomRootTrust, chain.ChainPolicy.TrustMode);

                        bool built = chain.Build(trustedCert.TrustedCert);

                        Assert.True(built, "Chain building should succeed with in-memory trust store on all platforms.");
                    }
                }
            }
            finally
            {
                TestFallbackCertificateBundleX509ChainFactories.SetTryUseAsDefault(tryUseAsDefault: true);
            }
        }

        [Fact]
        public void DefaultFactory_WhenTestFactoryNotActive_UsesDotNetDefaultWithSystemTrust()
        {
            TestFallbackCertificateBundleX509ChainFactories.SetTryUseAsDefault(tryUseAsDefault: false);

            try
            {
                X509TrustStore.SetCodeSigningX509ChainFactory(chainFactory: null!);

                TestLogger logger = new();
                IX509ChainFactory factory = X509TrustStore.CreateX509ChainFactory(
                    Signing.X509StorePurpose.CodeSigning, logger);

                Assert.IsType<DotNetDefaultTrustStoreX509ChainFactory>(factory);

                using (IX509Chain chain = factory.Create())
                {
                    Assert.Equal(X509ChainTrustMode.System, chain.ChainPolicy.TrustMode);
                }
            }
            finally
            {
                TestFallbackCertificateBundleX509ChainFactories.SetTryUseAsDefault(tryUseAsDefault: true);
            }
        }
    }
}

#endif
