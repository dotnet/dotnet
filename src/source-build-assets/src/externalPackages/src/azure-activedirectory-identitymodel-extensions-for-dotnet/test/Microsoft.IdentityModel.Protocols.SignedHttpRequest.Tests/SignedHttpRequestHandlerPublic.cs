// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace Microsoft.IdentityModel.Protocols.SignedHttpRequest.Tests
{
    /// <summary>
    /// Mock SignedHttpRequestHandler.
    /// </summary>
    public class SignedHttpRequestHandlerPublic : SignedHttpRequestHandler
    {
        public async Task<SecurityToken> ValidateSignedHttpRequestPayloadPublicAsync(SecurityToken signedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken)
        {
            return await ValidateSignedHttpRequestPayloadAsync(signedHttpRequest, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);
        }

        public async Task<SecurityKey> ValidateSignaturePublicAsync(JsonWebToken signedHttpRequest, SecurityKey popKey, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken)
        {
            return await ValidateSignatureAsync(signedHttpRequest, popKey, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);
        }

        public void ValidateTsClaimPublic(JsonWebToken jwtSignedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext)
        {
            ValidateTsClaim(jwtSignedHttpRequest, signedHttpRequestValidationContext);
        }

        public void ValidateMClaimPublic(JsonWebToken jwtSignedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext)
        {
            ValidateMClaim(jwtSignedHttpRequest, signedHttpRequestValidationContext);
        }

        public void ValidateUClaimPublic(JsonWebToken jwtSignedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext)
        {
            ValidateUClaim(jwtSignedHttpRequest, signedHttpRequestValidationContext);
        }

        public void ValidatePClaimPublic(JsonWebToken jwtSignedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext)
        {
            ValidatePClaim(jwtSignedHttpRequest, signedHttpRequestValidationContext);
        }

        public void ValidateQClaimPublic(JsonWebToken jwtSignedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext)
        {
            ValidateQClaim(jwtSignedHttpRequest, signedHttpRequestValidationContext);
        }

        public void ValidateHClaimPublic(JsonWebToken jwtSignedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext)
        {
            ValidateHClaim(jwtSignedHttpRequest, signedHttpRequestValidationContext);
        }

        public void ValidateBClaimPublic(JsonWebToken jwtSignedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext)
        {
            ValidateBClaim(jwtSignedHttpRequest, signedHttpRequestValidationContext);
        }

        public async Task<SecurityKey> ResolvePopKeyPublicAsync(JsonWebToken signedHttpRequest, JsonWebToken validatedAccessToken, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken)
        {
            return await ResolvePopKeyAsync(signedHttpRequest, validatedAccessToken, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);
        }

        internal Cnf GetCnfClaimValuePublic(JsonWebToken signedHttpRequest, JsonWebToken validatedAccessToken, SignedHttpRequestValidationContext signedHttpRequestValidationContext)
        {
            return GetCnfClaimValue(signedHttpRequest, validatedAccessToken, signedHttpRequestValidationContext);
        }

        internal async Task<SecurityKey> ResolvePopKeyFromCnfClaimPublicAsync(Cnf confirmationClaim, JsonWebToken signedHttpRequest, JsonWebToken validatedAccessToken, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken)
        {
            return await ResolvePopKeyFromCnfClaimAsync(confirmationClaim, signedHttpRequest, validatedAccessToken, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);
        }

        public SecurityKey ResolvePopKeyFromJwkPublic(JsonWebKey jwk, JsonWebToken signedHttpRequest, JsonWebToken validatedAccessToken, SignedHttpRequestValidationContext signedHttpRequestValidationContext)
        {
            return ResolvePopKeyFromJwk(jwk, signedHttpRequestValidationContext);
        }

        public async Task<SecurityKey> ResolvePopKeyFromJwePublicAsync(string jwe, JsonWebToken signedHttpRequest, JsonWebToken validatedAccessToken, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken)
        {
            return await ResolvePopKeyFromJweAsync(jwe, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);
        }

        internal async Task<SecurityKey> ResolvePopKeyFromJkuPublicAsync(string jkuSetUrl, Cnf cnf, JsonWebToken signedHttpRequest, JsonWebToken validatedAccessToken, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken)
        {
            return await ResolvePopKeyFromJkuAsync(jkuSetUrl, cnf, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IList<SecurityKey>> GetPopKeysFromJkuPublicAsync(string jkuSetUrl, JsonWebToken signedHttpRequest, JsonWebToken validatedAccessToken, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken)
        {
            return await GetPopKeysFromJkuAsync(jkuSetUrl, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);
        }

        public async Task<SecurityKey> ResolvePopKeyFromKeyIdentifierPublicAsync(string kid, JsonWebToken signedHttpRequest, JsonWebToken validatedAccessToken, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken)
        {
            return await ResolvePopKeyFromKeyIdentifierAsync(kid, signedHttpRequest, validatedAccessToken, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);
        }

        #region Mock methods
        internal override void ValidateTsClaim(JsonWebToken signedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext)
        {
            if (signedHttpRequestValidationContext.CallContext.PropertyBag != null && signedHttpRequestValidationContext.CallContext.PropertyBag.ContainsKey("onlyTrack_ValidateTsClaimCall"))
                signedHttpRequestValidationContext.CallContext.PropertyBag["onlyTrack_ValidateTsClaimCall"] = true;
            else
                base.ValidateTsClaim(signedHttpRequest, signedHttpRequestValidationContext);
        }

        internal override void ValidateMClaim(JsonWebToken signedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext)
        {
            if (signedHttpRequestValidationContext.CallContext.PropertyBag != null && signedHttpRequestValidationContext.CallContext.PropertyBag.ContainsKey("onlyTrack_ValidateMClaimCall"))
                signedHttpRequestValidationContext.CallContext.PropertyBag["onlyTrack_ValidateMClaimCall"] = true;
            else
                base.ValidateMClaim(signedHttpRequest, signedHttpRequestValidationContext);
        }

        internal override void ValidatePClaim(JsonWebToken signedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext)
        {
            if (signedHttpRequestValidationContext.CallContext.PropertyBag != null && signedHttpRequestValidationContext.CallContext.PropertyBag.ContainsKey("onlyTrack_ValidatePClaimCall"))
                signedHttpRequestValidationContext.CallContext.PropertyBag["onlyTrack_ValidatePClaimCall"] = true;
            else
                base.ValidatePClaim(signedHttpRequest, signedHttpRequestValidationContext);
        }

        internal override void ValidateUClaim(JsonWebToken signedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext)
        {
            if (signedHttpRequestValidationContext.CallContext.PropertyBag != null && signedHttpRequestValidationContext.CallContext.PropertyBag.ContainsKey("onlyTrack_ValidateUClaimCall"))
                signedHttpRequestValidationContext.CallContext.PropertyBag["onlyTrack_ValidateUClaimCall"] = true;
            else
                base.ValidateUClaim(signedHttpRequest, signedHttpRequestValidationContext);
        }

        internal override void ValidateQClaim(JsonWebToken signedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext)
        {
            if (signedHttpRequestValidationContext.CallContext.PropertyBag != null && signedHttpRequestValidationContext.CallContext.PropertyBag.ContainsKey("onlyTrack_ValidateQClaimCall"))
                signedHttpRequestValidationContext.CallContext.PropertyBag["onlyTrack_ValidateQClaimCall"] = true;
            else
                base.ValidateQClaim(signedHttpRequest, signedHttpRequestValidationContext);
        }

        internal override void ValidateHClaim(JsonWebToken signedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext)
        {
            if (signedHttpRequestValidationContext.CallContext.PropertyBag != null && signedHttpRequestValidationContext.CallContext.PropertyBag.ContainsKey("onlyTrack_ValidateHClaimCall"))
                signedHttpRequestValidationContext.CallContext.PropertyBag["onlyTrack_ValidateHClaimCall"] = true;
            else
                base.ValidateHClaim(signedHttpRequest, signedHttpRequestValidationContext);
        }

        internal override void ValidateBClaim(JsonWebToken signedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext)
        {
            if (signedHttpRequestValidationContext.CallContext.PropertyBag != null && signedHttpRequestValidationContext.CallContext.PropertyBag.ContainsKey("onlyTrack_ValidateBClaimCall"))
                signedHttpRequestValidationContext.CallContext.PropertyBag["onlyTrack_ValidateBClaimCall"] = true;
            else
                base.ValidateBClaim(signedHttpRequest, signedHttpRequestValidationContext);
        }

        internal override async Task<SecurityKey> ValidateSignatureAsync(JsonWebToken signedHttpRequest, SecurityKey popKey, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken)
        {
            if (signedHttpRequestValidationContext.CallContext.PropertyBag != null && signedHttpRequestValidationContext.CallContext.PropertyBag.ContainsKey("mockValidateSignedHttpRequestSignatureAsync"))
                return SignedHttpRequestTestUtils.DefaultSigningCredentials.Key;
            else
                return await base.ValidateSignatureAsync(signedHttpRequest, popKey, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);
        }

        internal override async Task<SecurityKey> ResolvePopKeyAsync(JsonWebToken signedHttpRequest, JsonWebToken validatedAccessToken, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken)
        {
            if (signedHttpRequestValidationContext.CallContext.PropertyBag != null && signedHttpRequestValidationContext.CallContext.PropertyBag.ContainsKey("mockResolvePopKeyAsync_returnValidKey"))
            {
                return SignedHttpRequestTestUtils.DefaultSigningCredentials.Key;
            }
            else if (signedHttpRequestValidationContext.CallContext.PropertyBag != null && signedHttpRequestValidationContext.CallContext.PropertyBag.ContainsKey("mockResolvePopKeyAsync_returnInvalidKey"))
            {
                return KeyingMaterial.RsaSecurityKey1;
            }
            else if (signedHttpRequestValidationContext.CallContext.PropertyBag != null && signedHttpRequestValidationContext.CallContext.PropertyBag.ContainsKey("mockResolvePopKeyAsync_returnNullKey"))
            {
                return null;
            }
            else
            {
                return await base.ResolvePopKeyAsync(signedHttpRequest, validatedAccessToken, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);
            }
        }

        internal override async Task<TokenValidationResult> ValidateAccessTokenAsync(string accessToken, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken)
        {
            if (signedHttpRequestValidationContext.CallContext.PropertyBag != null && signedHttpRequestValidationContext.CallContext.PropertyBag.ContainsKey("mockValidateAccessTokenAsync_returnInvalidResult"))
            {
                return new TokenValidationResult()
                {
                    IsValid = false,
                    Exception = new SecurityTokenValidationException()
                };
            }
            else
            {
                return await base.ValidateAccessTokenAsync(accessToken, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);
            }
        }

        internal override Cnf GetCnfClaimValue(JsonWebToken signedHttpRequest, JsonWebToken validatedAccessToken, SignedHttpRequestValidationContext signedHttpRequestValidationContext)
        {
            if (signedHttpRequestValidationContext?.CallContext.PropertyBag != null && signedHttpRequestValidationContext.CallContext.PropertyBag.ContainsKey("mockGetCnfClaimValue_returnJwk"))
            {
                return new Cnf(SignedHttpRequestTestUtils.DefaultCnfJwk.ToString(Newtonsoft.Json.Formatting.None));
            }
            else if (signedHttpRequestValidationContext?.CallContext.PropertyBag != null && signedHttpRequestValidationContext.CallContext.PropertyBag.ContainsKey("mockGetCnfClaimValue_returnJwe"))
            {
                return new Cnf(SignedHttpRequestTestUtils.DefaultCnfJwe.ToString(Newtonsoft.Json.Formatting.None));
            }
            else if (signedHttpRequestValidationContext?.CallContext.PropertyBag != null && signedHttpRequestValidationContext.CallContext.PropertyBag.ContainsKey("mockGetCnfClaimValue_returnJku"))
            {
                return new Cnf(SignedHttpRequestTestUtils.DefaultJku.ToString(Newtonsoft.Json.Formatting.None));
            }
            else if (signedHttpRequestValidationContext?.CallContext.PropertyBag != null && signedHttpRequestValidationContext.CallContext.PropertyBag.ContainsKey("mockGetCnfClaimValue_returnJkuKid"))
            {
                return new Cnf(SignedHttpRequestTestUtils.DefaultJkuKid.ToString(Newtonsoft.Json.Formatting.None));
            }
            else if (signedHttpRequestValidationContext?.CallContext.PropertyBag != null && signedHttpRequestValidationContext.CallContext.PropertyBag.ContainsKey("mockGetCnfClaimValue_returnKid"))
            {
                return new Cnf(SignedHttpRequestTestUtils.DefaultKid.ToString(Newtonsoft.Json.Formatting.None));
            }
            else if (signedHttpRequestValidationContext?.CallContext.PropertyBag != null && signedHttpRequestValidationContext.CallContext.PropertyBag.ContainsKey("mockGetCnfClaimValue_returnCustom"))
            {
                return new Cnf("{\"custom\": 1}");
            }
            else
            {
                return base.GetCnfClaimValue(signedHttpRequest, validatedAccessToken, signedHttpRequestValidationContext);
            }
        }

        internal override async Task<SecurityKey> ResolvePopKeyFromCnfClaimAsync(Cnf confirmationClaim, JsonWebToken signedHttpRequest, JsonWebToken validatedAccessToken, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken)
        {
            if (signedHttpRequestValidationContext?.CallContext.PropertyBag != null && signedHttpRequestValidationContext.CallContext.PropertyBag.ContainsKey("mockResolvePopKeyFromCnfClaimAsync_returnRsa"))
            {
                return signedHttpRequestValidationContext.CallContext.PropertyBag["mockResolvePopKeyFromCnfClaimAsync_returnRsa"] as RsaSecurityKey;
            }
            else
            {
                return await base.ResolvePopKeyFromCnfClaimAsync(confirmationClaim, signedHttpRequest, validatedAccessToken, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);
            }
        }

        internal override SecurityKey ResolvePopKeyFromJwk(JsonWebKey jwk, SignedHttpRequestValidationContext signedHttpRequestValidationContext)
        {
            if (signedHttpRequestValidationContext.CallContext.PropertyBag != null && signedHttpRequestValidationContext.CallContext.PropertyBag.ContainsKey("trackResolvePopKeyFromJwk"))
            {
                signedHttpRequestValidationContext.CallContext.PropertyBag["trackResolvePopKeyFromJwk"] = true;
                return null;
            }

            return base.ResolvePopKeyFromJwk(jwk, signedHttpRequestValidationContext);
        }

        internal override async Task<SecurityKey> ResolvePopKeyFromJweAsync(string jwe, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken)
        {
            if (signedHttpRequestValidationContext.CallContext.PropertyBag != null && signedHttpRequestValidationContext.CallContext.PropertyBag.ContainsKey("trackResolvePopKeyFromJwe"))
            {
                signedHttpRequestValidationContext.CallContext.PropertyBag["trackResolvePopKeyFromJwe"] = true;
                return null;
            }

            return await base.ResolvePopKeyFromJweAsync(jwe, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);
        }

        internal override async Task<SecurityKey> ResolvePopKeyFromJkuAsync(string jkuSetUrl, Cnf cnf, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken)
        {
            if (signedHttpRequestValidationContext.CallContext.PropertyBag != null && signedHttpRequestValidationContext.CallContext.PropertyBag.ContainsKey("trackResolvePopKeyFromJku"))
            {
                signedHttpRequestValidationContext.CallContext.PropertyBag["trackResolvePopKeyFromJku"] = true;
                return null;
            }

            return await base.ResolvePopKeyFromJkuAsync(jkuSetUrl, cnf, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);
        }

        internal override async Task<SecurityKey> ResolvePopKeyFromKeyIdentifierAsync(string kid, JsonWebToken signedHttpRequest, JsonWebToken validatedAccessToken, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken)
        {
            if (signedHttpRequestValidationContext.CallContext.PropertyBag != null && signedHttpRequestValidationContext.CallContext.PropertyBag.ContainsKey("trackResolvePopKeyFromKid"))
            {
                signedHttpRequestValidationContext.CallContext.PropertyBag["trackResolvePopKeyFromKid"] = true;
                return null;
            }

            return await base.ResolvePopKeyFromKeyIdentifierAsync(kid, signedHttpRequest, validatedAccessToken, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);
        }

        internal override async Task<IList<SecurityKey>> GetPopKeysFromJkuAsync(string jkuSetUrl, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken)
        {
            if (signedHttpRequestValidationContext.CallContext.PropertyBag != null && signedHttpRequestValidationContext.CallContext.PropertyBag.ContainsKey("mockGetPopKeysFromJkuAsync_return0Keys"))
            {
                return new List<SecurityKey>();
            }
            else if (signedHttpRequestValidationContext.CallContext.PropertyBag != null && signedHttpRequestValidationContext.CallContext.PropertyBag.ContainsKey("mockGetPopKeysFromJkuAsync_returnNull"))
            {
                return null;
            }
            else if (signedHttpRequestValidationContext.CallContext.PropertyBag != null && signedHttpRequestValidationContext.CallContext.PropertyBag.ContainsKey("mockGetPopKeysFromJkuAsync_return2Keys"))
            {
                return new List<SecurityKey>()
                {
                    SignedHttpRequestTestUtils.DefaultEncryptingCredentials.Key,
                    SignedHttpRequestTestUtils.DefaultSigningCredentials.Key,
                };
            }
            else if (signedHttpRequestValidationContext.CallContext.PropertyBag != null && signedHttpRequestValidationContext.CallContext.PropertyBag.ContainsKey("mockGetPopKeysFromJkuAsync_return1Key"))
            {
                return new List<SecurityKey>()
                {
                    SignedHttpRequestTestUtils.DefaultSigningCredentials.Key
                };
            }
            else if (signedHttpRequestValidationContext.CallContext.PropertyBag != null && signedHttpRequestValidationContext.CallContext.PropertyBag.ContainsKey("mockGetPopKeysFromJkuAsync_returnWrongKey"))
            {
                return new List<SecurityKey>()
                {
                    SignedHttpRequestTestUtils.DefaultEncryptingCredentials.Key
                };
            }

            return await base.GetPopKeysFromJkuAsync(jkuSetUrl, signedHttpRequestValidationContext, cancellationToken).ConfigureAwait(false);
        }

        #endregion
    }
}
