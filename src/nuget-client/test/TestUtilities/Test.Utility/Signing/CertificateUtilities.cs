// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Test.Utility.Signing
{
    public static class CertificateUtilities
    {
        internal static RSA CreateKeyPair(int strength = 2048)
        {
            return RSA.Create(strength);
        }

        internal static string GenerateFingerprint(X509Certificate2 certificate)
        {
#if NETFRAMEWORK
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(certificate.RawData);

                return BitConverter.ToString(hash).Replace("-", "");
            }
#else
            return certificate.GetCertHashString(HashAlgorithmName.SHA256);
#endif
        }

        internal static string GenerateRandomId()
        {
            return Guid.NewGuid().ToString();
        }

        public static X509Certificate2 GetCertificateWithPrivateKey(X509Certificate2 certificate, RSA keyPair)
        {
            X509Certificate2 certificateWithPrivateKey = certificate.CopyWithPrivateKey(keyPair);
#if NET
            return certificateWithPrivateKey;
#else
            using (certificateWithPrivateKey)
            {
                return new X509Certificate2(certificateWithPrivateKey.Export(X509ContentType.Pfx));
            }
#endif
        }
    }
}
