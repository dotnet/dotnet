// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Packaging.Signing;
using Test.Utility;
using Test.Utility.Signing;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class SignatureTrustAndValidityVerificationProviderTests
    {
#if IS_SIGNING_SUPPORTED
        private static readonly Lazy<PrimarySignature> _signature = new Lazy<PrimarySignature>(
            () => PrimarySignature.Load(SigningTestUtility.GetResourceBytes(".signature.p7s")));
#endif
        private readonly SignatureTrustAndValidityVerificationProvider _provider;

        public SignatureTrustAndValidityVerificationProviderTests()
        {
            _provider = new SignatureTrustAndValidityVerificationProvider();
        }

#if IS_SIGNING_SUPPORTED
        [Fact]
        public async Task GetTrustResultAsync_WhenPackageIsNull_Throws()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => _provider.GetTrustResultAsync(
                    package: null,
                    signature: _signature.Value,
                    settings: SignedPackageVerifierSettings.GetDefault(TestEnvironmentVariableReader.EmptyInstance),
                    token: CancellationToken.None));

            Assert.Equal("package", exception.ParamName);
        }
#endif

        [Fact]
        public async Task GetTrustResultAsync_WhenSignatureIsNull_Throws()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => _provider.GetTrustResultAsync(
                    package: Mock.Of<ISignedPackageReader>(),
                    signature: null,
                    settings: SignedPackageVerifierSettings.GetDefault(TestEnvironmentVariableReader.EmptyInstance),
                    token: CancellationToken.None));

            Assert.Equal("signature", exception.ParamName);
        }

#if IS_SIGNING_SUPPORTED
        [Fact]
        public async Task GetTrustResultAsync_WhenSettingsIsNull_Throws()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => _provider.GetTrustResultAsync(
                    package: Mock.Of<ISignedPackageReader>(),
                    signature: _signature.Value,
                    settings: null,
                    token: CancellationToken.None));

            Assert.Equal("settings", exception.ParamName);
        }

        [Fact]
        public async Task GetTrustResultAsync_WhenTokenIsCanceled_Throws()
        {
            await Assert.ThrowsAsync<OperationCanceledException>(
                () => _provider.GetTrustResultAsync(
                    package: Mock.Of<ISignedPackageReader>(),
                    signature: _signature.Value,
                    settings: SignedPackageVerifierSettings.GetDefault(TestEnvironmentVariableReader.EmptyInstance),
                    token: new CancellationToken(canceled: true)));
        }
#endif
    }
}
