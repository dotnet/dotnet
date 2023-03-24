// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED

using System;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Test.Utility.Signing;
using Xunit;

namespace NuGet.Packaging.FuncTest
{
    [Collection(SigningTestCollection.Name)]
    public class X509TrustStoreTests : IDisposable
    {
        private readonly TestLogger _logger;

        public X509TrustStoreTests()
        {
            _logger = new TestLogger();

            // For these tests, use whatever factory X509TrustStore creates by default.
            TestFallbackCertificateBundleX509ChainFactories.SetTryUseAsDefault(tryUseAsDefault: false);
        }

        public void Dispose()
        {
            TestFallbackCertificateBundleX509ChainFactories.SetTryUseAsDefault(tryUseAsDefault: true);

            GC.SuppressFinalize(this);
        }

        [Fact]
        public void InitializeForDotNetSdk_WhenArgumentIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => X509TrustStore.InitializeForDotNetSdk(logger: null));

            Assert.Equal("logger", exception.ParamName);
        }

        [Fact]
        public void CreateX509ChainFactory_Always_ReturnsInstance()
        {
            IX509ChainFactory factory = X509TrustStore.CreateX509ChainFactory(NuGet.Packaging.Signing.X509StorePurpose.CodeSigning, _logger);

            Assert.IsType<DotNetDefaultTrustStoreX509ChainFactory>(factory);
            Assert.Equal(1, _logger.Messages.Count);
            Assert.Equal(1, _logger.InformationMessages.Count);
            Assert.True(_logger.InformationMessages.TryDequeue(out string actualMessage));
            Assert.Equal(Strings.ChainBuilding_UsingDefaultTrustStoreForCodeSigning, actualMessage);
        }
    }
}
#endif
