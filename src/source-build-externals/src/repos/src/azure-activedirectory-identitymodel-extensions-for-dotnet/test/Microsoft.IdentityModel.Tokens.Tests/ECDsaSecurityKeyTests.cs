// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Security.Cryptography;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class ECDsaSecurityKeyTests
    {
        [Fact]
        public void Constructor()
        {

            // testing constructor that takes ECDsa instance
            ECDsaSecurityKeyConstructorWithEcdsa(null, ExpectedException.ArgumentNullException("ecdsa"));
#if !NET_CORE
            ECDsaSecurityKeyConstructorWithEcdsa(new ECDsaCng(), ExpectedException.NoExceptionExpected);
            var ecdsaSecurityKey = new ECDsaSecurityKey(new ECDsaCng());
#elif NET_CORE
            ECDsaSecurityKeyConstructorWithEcdsa(ECDsa.Create(), ExpectedException.NoExceptionExpected);
            var ecdsaSecurityKey = new ECDsaSecurityKey(ECDsa.Create());
#endif
            Assert.True(ecdsaSecurityKey.PrivateKeyStatus == PrivateKeyStatus.Unknown, "ecdsaSecurityKey.FoundPrivateKey is unknown");
        }

        private void ECDsaSecurityKeyConstructorWithEcdsa(ECDsa ecdsa, ExpectedException ee)
        {
            try
            {
                var ecdsaSecurityKey = new ECDsaSecurityKey(ecdsa);
                ee.ProcessNoException();
            }
            catch (Exception exception)
            {
                ee.ProcessException(exception);
            }
        }

        [Fact]
        public void Defaults()
        {
            // there are no defaults.
        }

        [Fact]
        public void CanComputeJwkThumbprint()
        {
#if NET472 || NET_CORE
            Assert.True(KeyingMaterial.Ecdsa256Key.CanComputeJwkThumbprint(), "Couldn't compute JWK thumbprint on an ECDsaSecurityKey on net472 or .net core.");
#else
            Assert.False(KeyingMaterial.Ecdsa256Key.CanComputeJwkThumbprint(), "ECDsaSecurityKey shouldn't be able to compute JWK thumbprint on Desktop (net461 target).");
#endif
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
