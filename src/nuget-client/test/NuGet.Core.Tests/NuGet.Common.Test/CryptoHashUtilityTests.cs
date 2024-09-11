// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography;
using NuGet.Packaging.Signing;
using Xunit;

#if !IS_CORECLR
using System.Reflection;
#endif

namespace NuGet.Common.Test
{
    public class CryptoHashUtilityTests
    {
        [Fact]
        public void ConvertToSystemSecurityHashAlgorithmName_WithSha256_Succeeds()
        {
            var hashAlgorithmName = HashAlgorithmName.SHA256.ConvertToSystemSecurityHashAlgorithmName();

            Assert.Equal(System.Security.Cryptography.HashAlgorithmName.SHA256.Name, hashAlgorithmName.Name);
        }

        [Fact]
        public void ConvertToSystemSecurityHashAlgorithmName_WithSha384_Succeeds()
        {
            var hashAlgorithmName = HashAlgorithmName.SHA384.ConvertToSystemSecurityHashAlgorithmName();

            Assert.Equal(System.Security.Cryptography.HashAlgorithmName.SHA384.Name, hashAlgorithmName.Name);
        }

        [Fact]
        public void ConvertToSystemSecurityHashAlgorithmName_WithSha512_Succeeds()
        {
            var hashAlgorithmName = HashAlgorithmName.SHA512.ConvertToSystemSecurityHashAlgorithmName();

            Assert.Equal(System.Security.Cryptography.HashAlgorithmName.SHA512.Name, hashAlgorithmName.Name);
        }

        [Fact]
        public void ConvertToSystemSecurityHashAlgorithmName_WithUnknown_Throws()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => HashAlgorithmName.Unknown.ConvertToSystemSecurityHashAlgorithmName());

            Assert.Equal("hashAlgorithmName", exception.ParamName);
            Assert.StartsWith("Hash algorithm 'Unknown' is unsupported.", exception.Message);
        }

        [Theory]
        [InlineData(HashAlgorithmName.SHA256, Oids.Sha256)]
        [InlineData(HashAlgorithmName.SHA384, Oids.Sha384)]
        [InlineData(HashAlgorithmName.SHA512, Oids.Sha512)]
        public void ConvertToOidString_HashAlgorithmName_WithValidInput_Succeeds(HashAlgorithmName hashAlgorithmName, string expectedOid)
        {
            var actualOid = hashAlgorithmName.ConvertToOidString();

            Assert.Equal(expectedOid, actualOid);
        }

        [Fact]
        public void ConvertToOidString_HashAlgorithmName_WithUnknown_Throws()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => HashAlgorithmName.Unknown.ConvertToOidString());

            Assert.Equal("hashAlgorithmName", exception.ParamName);
            Assert.StartsWith("Hash algorithm 'Unknown' is unsupported.", exception.Message);
        }

        [Theory]
        [InlineData(SignatureAlgorithmName.SHA256RSA, Oids.Sha256WithRSAEncryption)]
        [InlineData(SignatureAlgorithmName.SHA384RSA, Oids.Sha384WithRSAEncryption)]
        [InlineData(SignatureAlgorithmName.SHA512RSA, Oids.Sha512WithRSAEncryption)]
        public void ConvertToOidString_SignatureAlgorithmName_WithValidInput_Succeeds(SignatureAlgorithmName signatureAlgorithmName, string expectedOid)
        {
            var actualOid = signatureAlgorithmName.ConvertToOidString();

            Assert.Equal(expectedOid, actualOid);
        }

        [Fact]
        public void ConvertToOidString_SignatureAlgorithmName_WithUnknown_Throws()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => SignatureAlgorithmName.Unknown.ConvertToOidString());

            Assert.Equal("signatureAlgorithmName", exception.ParamName);
            Assert.StartsWith("Signature algorithm 'Unknown' is unsupported.", exception.Message);
        }

        [Theory]
        [InlineData(Oids.Sha256, HashAlgorithmName.SHA256)]
        [InlineData(Oids.Sha384, HashAlgorithmName.SHA384)]
        [InlineData(Oids.Sha512, HashAlgorithmName.SHA512)]
        public void OidToHashAlgorithmName_WithValidInput_Succeeds(string oid, HashAlgorithmName expectedHashAlgorithmName)
        {
            var actualHashAlgorithmName = CryptoHashUtility.OidToHashAlgorithmName(oid);

            Assert.Equal(expectedHashAlgorithmName, actualHashAlgorithmName);
        }

        [Fact]
        public void OidToHashAlgorithmName_WithUnknown_Throws()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => CryptoHashUtility.OidToHashAlgorithmName(Oids.Sha1));

            Assert.Equal("oid", exception.ParamName);
            Assert.StartsWith($"Hash algorithm '{Oids.Sha1}' is unsupported.", exception.Message);
        }

        [Fact]
        public void GetHashProvider_WhenHashAlgorithmNameIsSupported_ReturnsCorrectImplementation()
        {
            using (var hashAlgorithm = CryptoHashUtility.GetHashProvider(HashAlgorithmName.SHA256))
            {
                Assert.True(hashAlgorithm is SHA256);

#if !IS_CORECLR
                if (AllowOnlyFipsAlgorithms())
                {
                    Assert.IsType<SHA256CryptoServiceProvider>(hashAlgorithm);
                }
                else
                {
                    Assert.IsType<SHA256Managed>(hashAlgorithm);
                }
#endif
            }
        }

        private static bool AllowOnlyFipsAlgorithms()
        {
#if !IS_CORECLR
            // Mono does not currently support this method. Have this in a separate method to avoid JITing exceptions.
            var cryptoConfig = typeof(CryptoConfig);

            if (cryptoConfig != null)
            {
                var allowOnlyFipsAlgorithmsProperty = cryptoConfig.GetProperty("AllowOnlyFipsAlgorithms", BindingFlags.Public | BindingFlags.Static);

                if (allowOnlyFipsAlgorithmsProperty != null)
                {
                    return (bool)allowOnlyFipsAlgorithmsProperty.GetValue(null, null);
                }
            }

            return false;
#else
            return false;
#endif
        }
    }
}
