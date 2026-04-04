// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Internal.NuGet.Testing.SignedPackages
{
    internal sealed class CodeSigningRootX509Store : IRootX509Store
    {
        internal static CodeSigningRootX509Store Instance { get; } = new();

        public void Add(StoreLocation storeLocation, X509Certificate2 certificate)
        {
#if NET5_0_OR_GREATER
            X509Certificate2Collection certificates = TestFallbackCertificateBundleX509ChainFactories.Instance.CodeSigningX509ChainFactory.Certificates;

            if (!certificates.Contains(certificate))
            {
                certificates.Add(certificate);
            }
#else
            throw new NotSupportedException();
#endif
        }

        public void Remove(StoreLocation storeLocation, X509Certificate2 certificate)
        {
#if NET5_0_OR_GREATER
            // X509Certificate2Collection.Remove() throws if the certificate is not found.
            // Another thread or a prior call for a different purpose may have already removed it.
            try
            {
                TestFallbackCertificateBundleX509ChainFactories.Instance.CodeSigningX509ChainFactory.Certificates.Remove(certificate);
            }
            catch (ArgumentException)
            {
            }
#else
            throw new NotSupportedException();
#endif
        }
    }
}
