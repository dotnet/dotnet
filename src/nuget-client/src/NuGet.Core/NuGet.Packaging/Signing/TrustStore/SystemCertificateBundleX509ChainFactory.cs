// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if NET5_0_OR_GREATER

using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace NuGet.Packaging.Signing
{
    internal sealed class SystemCertificateBundleX509ChainFactory : CertificateBundleX509ChainFactory
    {
        internal static readonly IReadOnlyList<string> ProbePaths = new[]
        {
            "/etc/pki/ca-trust/extracted/pem/objsign-ca-bundle.pem"
        };

        private SystemCertificateBundleX509ChainFactory(X509Certificate2Collection certificates, string filePath)
            : base(certificates, filePath)
        {
        }

        internal static bool TryCreate(out SystemCertificateBundleX509ChainFactory factory)
        {
            return TryCreate(ProbePaths, out factory);
        }

        // For testing purposes only.
        internal static bool TryCreate(IReadOnlyList<string> probePaths, out SystemCertificateBundleX509ChainFactory factory)
        {
            factory = null;

            foreach (string probePath in probePaths)
            {
                if (TryImportFromPemFile(probePath, out X509Certificate2Collection certificates)
                    && certificates.Count > 0)
                {
                    factory = new SystemCertificateBundleX509ChainFactory(certificates, probePath);

                    return true;
                }
            }

            return false;
        }
    }
}

#endif
