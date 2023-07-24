// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
