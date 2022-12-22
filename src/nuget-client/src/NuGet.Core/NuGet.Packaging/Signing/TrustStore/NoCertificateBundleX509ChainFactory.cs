// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if NET5_0_OR_GREATER

using System.Security.Cryptography.X509Certificates;

namespace NuGet.Packaging.Signing
{
    internal sealed class NoCertificateBundleX509ChainFactory : CertificateBundleX509ChainFactory
    {
        internal NoCertificateBundleX509ChainFactory()
            : base(new X509Certificate2Collection())
        {
        }
    }
}

#endif
