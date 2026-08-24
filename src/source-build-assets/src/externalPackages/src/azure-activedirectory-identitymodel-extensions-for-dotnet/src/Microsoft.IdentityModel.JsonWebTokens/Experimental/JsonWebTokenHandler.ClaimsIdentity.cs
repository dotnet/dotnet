// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens.Experimental;
using System.Collections.Generic;

#nullable enable
namespace Microsoft.IdentityModel.JsonWebTokens
{
    public partial class JsonWebTokenHandler
    {
        /// <summary>
        /// Creates a <see cref="ClaimsIdentity"/> from a <see cref="JsonWebToken"/>.
        /// </summary>
        /// <param name="jwtToken">The <see cref="JsonWebToken"/> to use as a <see cref="Claim"/> source.</param>
        /// <param name="validationParameters">The <see cref="ValidationParameters"/> to be used for validating the token.</param>
        /// <returns>A <see cref="ClaimsIdentity"/> containing the <see cref="JsonWebToken.Claims"/>.</returns>
        internal virtual ClaimsIdentity CreateClaimsIdentity(JsonWebToken? jwtToken, ValidationParameters validationParameters)
        {
            // TODO: Make protected once ValidationParameters is public.
            _ = jwtToken ?? throw LogHelper.LogArgumentNullException(nameof(jwtToken));

            return CreateClaimsIdentityPrivate(jwtToken, validationParameters, GetActualIssuer(jwtToken));
        }

        /// <summary>
        /// Creates a <see cref="ClaimsIdentity"/> from a <see cref="JsonWebToken"/> with the specified issuer.
        /// </summary>
        /// <param name="jwtToken">The <see cref="JsonWebToken"/> to use as a <see cref="Claim"/> source.</param>
        /// <param name="validationParameters">The <see cref="ValidationParameters"/> to be used for validating the token.</param>
        /// <param name="issuer">Specifies the issuer for the <see cref="ClaimsIdentity"/>.</param>
        /// <returns>A <see cref="ClaimsIdentity"/> containing the <see cref="JsonWebToken.Claims"/>.</returns>
        internal virtual ClaimsIdentity CreateClaimsIdentity(
            JsonWebToken? jwtToken,
            ValidationParameters validationParameters,
            string issuer)
        {
            // TODO: Make protected once ValidationParameters is public.
            _ = jwtToken ?? throw LogHelper.LogArgumentNullException(nameof(jwtToken));

            if (string.IsNullOrWhiteSpace(issuer))
                issuer = GetActualIssuer(jwtToken);

            if (MapInboundClaims)
                return CreateClaimsIdentityWithMapping(jwtToken, validationParameters, issuer);

            return CreateClaimsIdentityPrivate(jwtToken, validationParameters, issuer);
        }

        internal override ClaimsIdentity CreateClaimsIdentityInternal(
            SecurityToken securityToken,
            ValidationParameters validationParameters,
            string issuer)
        {
            return CreateClaimsIdentity(securityToken as JsonWebToken, validationParameters, issuer);
        }

        private ClaimsIdentity CreateClaimsIdentityWithMapping(JsonWebToken jwtToken, ValidationParameters validationParameters, string issuer)
        {
            _ = validationParameters ?? throw LogHelper.LogArgumentNullException(nameof(validationParameters));

            ClaimsIdentity identity = validationParameters.CreateClaimsIdentity(jwtToken, issuer);
            foreach (Claim jwtClaim in jwtToken.Claims)
            {
                bool wasMapped = _inboundClaimTypeMap.TryGetValue(jwtClaim.Type, out string? type);

                string claimType = type ?? jwtClaim.Type;

                if (claimType == ClaimTypes.Actor)
                {
                    if (identity.Actor != null)
                        throw LogHelper.LogExceptionMessage(
                            new InvalidOperationException(
                                LogHelper.FormatInvariant(
                                    LogMessages.IDX14112,
                                    LogHelper.MarkAsNonPII(JwtRegisteredClaimNames.Actort),
                                    jwtClaim.Value)));

                    if (CanReadToken(jwtClaim.Value))
                    {
                        JsonWebToken? actor = ReadToken(jwtClaim.Value) as JsonWebToken;
                        identity.Actor = CreateClaimsIdentity(actor, validationParameters);
                    }
                }

                if (wasMapped)
                {
                    Claim claim = new Claim(claimType, jwtClaim.Value, jwtClaim.ValueType, issuer, issuer, identity);
                    if (jwtClaim.Properties.Count > 0)
                    {
                        foreach (var kv in jwtClaim.Properties)
                        {
                            claim.Properties[kv.Key] = kv.Value;
                        }
                    }

                    claim.Properties[ShortClaimTypeProperty] = jwtClaim.Type;
                    identity.AddClaim(claim);
                }
                else
                {
                    identity.AddClaim(jwtClaim);
                }
            }

            return identity;
        }

        private ClaimsIdentity CreateClaimsIdentityPrivate(JsonWebToken jwtToken, ValidationParameters validationParameters, string issuer)
        {
            _ = validationParameters ?? throw LogHelper.LogArgumentNullException(nameof(validationParameters));

            ClaimsIdentity identity = validationParameters.CreateClaimsIdentity(jwtToken, issuer);
            foreach (Claim jwtClaim in jwtToken.Claims)
            {
                string claimType = jwtClaim.Type;
                if (claimType == ClaimTypes.Actor)
                {
                    if (identity.Actor != null)
                        throw LogHelper.LogExceptionMessage(
                            new InvalidOperationException(
                                LogHelper.FormatInvariant(
                                    LogMessages.IDX14112,
                                    LogHelper.MarkAsNonPII(JwtRegisteredClaimNames.Actort),
                                    jwtClaim.Value)));

                    if (CanReadToken(jwtClaim.Value))
                    {
                        JsonWebToken? actor = ReadToken(jwtClaim.Value) as JsonWebToken;
                        identity.Actor = CreateClaimsIdentity(actor, validationParameters, issuer);
                    }
                }

                if (jwtClaim.Properties.Count == 0)
                {
                    identity.AddClaim(new Claim(claimType, jwtClaim.Value, jwtClaim.ValueType, issuer, issuer, identity));
                }
                else
                {
                    Claim claim = new(claimType, jwtClaim.Value, jwtClaim.ValueType, issuer, issuer, identity);

                    foreach (KeyValuePair<string, string> kv in jwtClaim.Properties)
                        claim.Properties[kv.Key] = kv.Value;

                    identity.AddClaim(claim);
                }
            }

            return identity;
        }
    }
}
#nullable restore
