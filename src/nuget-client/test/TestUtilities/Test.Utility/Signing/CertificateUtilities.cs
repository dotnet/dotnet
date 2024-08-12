// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using NuGet.Common;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Xunit;
using X509Certificate = Org.BouncyCastle.X509.X509Certificate;

namespace Test.Utility.Signing
{
    public static class CertificateUtilities
    {
        internal static AsymmetricCipherKeyPair CreateKeyPair(int strength = 2048)
        {
            var generator = new RsaKeyPairGenerator();

            generator.Init(new KeyGenerationParameters(new SecureRandom(), strength));

            return generator.GenerateKeyPair();
        }

        internal static string GenerateFingerprint(X509Certificate certificate)
        {
            using (var hashAlgorithm = NuGet.Common.HashAlgorithmName.SHA256.GetHashProvider())
            {
                var hash = hashAlgorithm.ComputeHash(certificate.GetEncoded());

                return BitConverter.ToString(hash).Replace("-", "");
            }
        }

        internal static string GenerateRandomId()
        {
            return Guid.NewGuid().ToString();
        }

        public static X509Certificate2 GetCertificateWithPrivateKey(X509Certificate bcCertificate, AsymmetricCipherKeyPair keyPair)
        {
            Assert.IsType<RsaPrivateCrtKeyParameters>(keyPair.Private);

            var privateKeyParameters = (RsaPrivateCrtKeyParameters)keyPair.Private;
#if IS_DESKTOP
            RSA privateKey = DotNetUtilities.ToRSA(privateKeyParameters);

            var certificate = new X509Certificate2(bcCertificate.GetEncoded());

            certificate.PrivateKey = privateKey;
#else
            RSAParameters rsaParameters = DotNetUtilities.ToRSAParameters(privateKeyParameters);

            var privateKey = new RSACryptoServiceProvider();

            privateKey.ImportParameters(rsaParameters);

            X509Certificate2 certificate;

            using (var certificateTmp = new X509Certificate2(bcCertificate.GetEncoded()))
            {
                certificate = RSACertificateExtensions.CopyWithPrivateKey(certificateTmp, privateKey);
            }
#endif
            return certificate;
        }
    }
}
