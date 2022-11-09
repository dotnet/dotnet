// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Common;
using NuGet.Packaging.Signing;

namespace Test.Utility.Signing
{
    internal sealed class TestFallbackCertificateBundleX509ChainFactories
    {
        internal static TestFallbackCertificateBundleX509ChainFactories Instance { get; } = new();

        internal TestFallbackCertificateBundleX509ChainFactory CodeSigningX509ChainFactory { get; } = new TestFallbackCertificateBundleX509ChainFactory("codesignctl.pem");
        internal TestFallbackCertificateBundleX509ChainFactory TimestampingX509ChainFactory { get; } = new TestFallbackCertificateBundleX509ChainFactory("timestampctl.pem");

        internal static void SetTryUseAsDefault(bool tryUseAsDefault)
        {
            IX509ChainFactory codeSigningFactory = null;
            IX509ChainFactory timestampingFactory = null;

#if NET5_0_OR_GREATER
            if (tryUseAsDefault && !RuntimeEnvironmentHelper.IsWindows)
            {
                codeSigningFactory = Instance.CodeSigningX509ChainFactory;
                timestampingFactory = Instance.TimestampingX509ChainFactory;
            }
#endif

            X509TrustStore.SetCodeSigningX509ChainFactory(codeSigningFactory);
            X509TrustStore.SetTimestampingX509ChainFactory(timestampingFactory);
        }
    }
}
