//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.Azure.KeyVault.WebKey;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Xunit;
using static Microsoft.IdentityModel.KeyVaultExtensions.KeyVaultSecurityKey;

namespace Microsoft.IdentityModel.KeyVaultExtensions.Tests
{
    public class KeyVaultCryptoProviderTests
    {
        [Fact]
        public void ShouldCacheSignatureProvider()
        {
            TestUtilities.WriteHeader($"{this}.ShouldCacheSignatureProvider");
            var context = new CompareContext($"{this}.ShouldCacheSignatureProvider");
            var keyVaultKeyWithEmptyKid = new CustomKeyVaultSecurityKey("test", new AuthenticationCallback((string authority, string resource, string scope) => Task.FromResult(string.Empty)));
            var keyVaultCryptoProvider = new KeyVaultCryptoProvider();
            var signatureProvider = keyVaultCryptoProvider.Create(JsonWebKeySignatureAlgorithm.RS256, keyVaultKeyWithEmptyKid, true);
            if (keyVaultCryptoProvider.CryptoProviderCache.TryGetSignatureProvider(keyVaultKeyWithEmptyKid, SecurityAlgorithms.RsaSha256Signature, typeof(KeyVaultSignatureProvider).ToString(), true, out var _))
                context.Diffs.Add("A SignatureProvider was added to keyVaultCryptoProvider.CryptoProviderCache.CryptoProviderCache, but ShouldCacheSignatureProvider() should return false as the key has an empty key id.");

            CryptoProviderFactory.Default.ReleaseSignatureProvider(signatureProvider as KeyVaultSignatureProvider);

            TestUtilities.AssertFailIfErrors(context);
        }

        public class CustomKeyVaultSecurityKey : KeyVaultSecurityKey
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="KeyVaultSecurityKey"/> class.
            /// </summary>
            public CustomKeyVaultSecurityKey(string keyIdentifier, AuthenticationCallback callback) : base(keyIdentifier, callback)
            {
            }

            internal override string InternalId => "";
        }
    }
}
