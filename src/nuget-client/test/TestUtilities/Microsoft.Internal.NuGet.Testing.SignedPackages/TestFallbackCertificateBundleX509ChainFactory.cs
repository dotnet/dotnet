// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if NET5_0_OR_GREATER
using System.Security.Cryptography.X509Certificates;
using System.Text;
using NuGet.Packaging.Signing;
#endif

namespace Microsoft.Internal.NuGet.Testing.SignedPackages
{
    internal sealed class TestFallbackCertificateBundleX509ChainFactory
#if NET5_0_OR_GREATER
        : CertificateBundleX509ChainFactory
#endif
    {
        internal TestFallbackCertificateBundleX509ChainFactory(string resourceName)
#if NET5_0_OR_GREATER
            : base(LoadCertificates(resourceName))
#endif
        {
        }

#if NET5_0_OR_GREATER
        private static X509Certificate2Collection LoadCertificates(string resourceName)
        {
            // Load an extract from the August 2022 Windows CTL update.
            // Similar to Windows' trusted root authority certificates trust store, this file contains
            // both expired and active root certificates.  Tests should not be affected by expiration.
            byte[] bytes = SigningTestUtility.GetResourceBytes(resourceName);
            string pem = Encoding.UTF8.GetString(bytes);
            X509Certificate2Collection certificates = new();

            certificates.ImportFromPem(pem);

            return certificates;
        }
#endif
    }
}
