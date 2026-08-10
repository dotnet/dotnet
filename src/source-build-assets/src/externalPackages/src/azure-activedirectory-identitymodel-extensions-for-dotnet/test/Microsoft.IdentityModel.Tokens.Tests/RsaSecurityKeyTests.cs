// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Globalization;
using System.Security.Cryptography;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class RsaSecurityKeyTests
    {
        [Fact]
        public void Constructor()
        {
            // testing constructor that takes rsa parameters
            RsaSecurityKeyConstructor(KeyingMaterial.RsaParameters_1024, ExpectedException.NoExceptionExpected);
            RsaSecurityKeyConstructor(KeyingMaterial.RsaParameters_1024_Public, ExpectedException.NoExceptionExpected);

            // missing modulus or exponent
            RsaSecurityKeyConstructor(KeyingMaterial.RsaParameters_2048_MissingExponent, ExpectedException.ArgumentException("IDX10700"));
            RsaSecurityKeyConstructor(KeyingMaterial.RsaParameters_2048_MissingModulus, ExpectedException.ArgumentException("IDX10700"));

            // testing constructor that takes Rsa instance
            RsaSecurityKeyConstructorWithRsa(null, ExpectedException.ArgumentNullException("rsa"));
        }

        private void RsaSecurityKeyConstructor(RSAParameters parameters, ExpectedException ee)
        {
            try
            {
                var rsaSecurityKey = new RsaSecurityKey(parameters);
                ee.ProcessNoException();
            }
            catch (Exception exception)
            {
                ee.ProcessException(exception);
            }
        }

        private void RsaSecurityKeyConstructorWithRsa(RSA rsa, ExpectedException ee)
        {
            try
            {
                var rsaSecurityKey = new RsaSecurityKey(rsa);
                ee.ProcessNoException();
            }
            catch (Exception exception)
            {
                ee.ProcessException(exception);
            }
        }

        [Theory, MemberData(nameof(HasPrivateKeyTheoryData), DisableDiscoveryEnumeration = true)]
        public void HasPrivateKey(string testId, RsaSecurityKey key, bool expected)
        {
            if (expected)
                Assert.True(key.PrivateKeyStatus == PrivateKeyStatus.Exists, testId);
            else
                Assert.True(key.PrivateKeyStatus != PrivateKeyStatus.Exists, testId);
        }

        public static TheoryData<string, RsaSecurityKey, bool> HasPrivateKeyTheoryData()
        {
            var theoryData = new TheoryData<string, RsaSecurityKey, bool>();
#if NET462
            theoryData.Add(
                "KeyingMaterial.RsaSecurityKeyWithCspProvider_2048",
                KeyingMaterial.RsaSecurityKeyWithCspProvider_2048,
                true
            );

            theoryData.Add(
                "KeyingMaterial.RsaSecurityKeyWithCspProvider_2048_Public",
                KeyingMaterial.RsaSecurityKeyWithCspProvider_2048_Public,
                false
            );
#endif

#if NET462

            theoryData.Add(
                "KeyingMaterial.RsaSecurityKeyWithCngProvider_2048",
                KeyingMaterial.RsaSecurityKeyWithCngProvider_2048,
                true
            );

            theoryData.Add(
                "KeyingMaterial.RsaSecurityKeyWithCngProvider_2048_Public",
                KeyingMaterial.RsaSecurityKeyWithCngProvider_2048_Public,
                false
            );
#endif

            theoryData.Add(
                "KeyingMaterial.RsaSecurityKey_2048",
                KeyingMaterial.RsaSecurityKey_2048,
                true
            );

            theoryData.Add(
                "KeyingMaterial.RsaSecurityKey_2048_Public",
                KeyingMaterial.RsaSecurityKey_2048_Public,
                false
            );

            theoryData.Add(
               "KeyingMaterial.RsaSecurityKey_2048_FromRsa",
               KeyingMaterial.RsaSecurityKey_2048_FromRsa,
               true
           );

            theoryData.Add(
                "KeyingMaterial.RsaSecurityKey_2048_FromRsa_Public",
                KeyingMaterial.RsaSecurityKey_2048_FromRsa_Public,
                false
            );

            return theoryData;
        }

        [Fact]
        public void KeySize()
        {
            Assert.True(KeyingMaterial.RsaSecurityKey_2048.KeySize == 2048, string.Format(CultureInfo.InvariantCulture, "Keysize '{0}' != 2048", KeyingMaterial.RsaSecurityKey_2048.KeySize));
            Assert.True(KeyingMaterial.RsaSecurityKey_4096.KeySize == 4096, string.Format(CultureInfo.InvariantCulture, "Keysize '{0}' != 4096", KeyingMaterial.RsaSecurityKey_4096.KeySize));
        }

        [Fact]
        public void CanComputeJwkThumbprint()
        {
            Assert.True(KeyingMaterial.DefaultRsaSecurityKey1.CanComputeJwkThumbprint(), "Couldn't compute JWK thumbprint on an RSASecurityKey.");
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
