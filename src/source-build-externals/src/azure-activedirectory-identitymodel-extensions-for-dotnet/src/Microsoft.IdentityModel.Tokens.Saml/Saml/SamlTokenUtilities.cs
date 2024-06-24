
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Security.Claims;
using System.Collections;
using System.Collections.Generic;
using Microsoft.IdentityModel.Xml;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Logging;
using TokenLogMessages = Microsoft.IdentityModel.Tokens.LogMessages;

namespace Microsoft.IdentityModel.Tokens.Saml
{
    /// <summary>
    /// A class which contains useful methods for processing saml tokens.
    /// </summary>
    internal class SamlTokenUtilities
    {
        /// <summary>
        /// Returns a <see cref="SecurityKey"/> to use when validating the signature of a token.
        /// </summary>
        /// <param name="tokenKeyInfo">The <see cref="KeyInfo"/> field of the token being validated</param>
        /// <param name="validationParameters">A <see cref="TokenValidationParameters"/> required for validation.</param>
        /// <returns>Returns a <see cref="SecurityKey"/> to use for signature validation.</returns>
        /// <remarks>If key fails to resolve, then null is returned</remarks>
        internal static SecurityKey ResolveTokenSigningKey(KeyInfo tokenKeyInfo, TokenValidationParameters validationParameters)
        {
            if (tokenKeyInfo == null)
                return null;

            if (validationParameters.IssuerSigningKey != null && tokenKeyInfo.MatchesKey(validationParameters.IssuerSigningKey))
                return validationParameters.IssuerSigningKey;

            if (validationParameters.IssuerSigningKeys != null)
            {
                foreach (var key in validationParameters.IssuerSigningKeys)
                {
                    if (tokenKeyInfo.MatchesKey(key))
                        return key;
                }
            }

            return null;
        }

        /// <summary>
        /// Creates <see cref="Claim"/>'s from <paramref name="claimsCollection"/>.
        /// </summary>
        /// <param name="claimsCollection"> A dictionary that represents a set of claims.</param>
        /// <returns> A collection of <see cref="Claim"/>'s created from the <paramref name="claimsCollection"/>.</returns>
        internal static IEnumerable<Claim> CreateClaimsFromDictionary(IDictionary<string, object> claimsCollection)
        {
            if (claimsCollection == null)
                return null;

            var claims = new List<Claim>();
            foreach (var claim in claimsCollection)
            {
                string claimType = claim.Key;
                object claimValue = claim.Value;
                if (claimValue != null)
                {
                    var valueType = GetXsiTypeForValue(claimValue);
                    if (valueType == null && claimValue is IEnumerable claimList)
                    {
                        foreach (var item in claimList)
                        {
                            valueType = GetXsiTypeForValue(item);
                            if (valueType == null && item is IEnumerable)
                                throw new NotSupportedException(LogHelper.FormatInvariant(TokenLogMessages.IDX10105, LogHelper.MarkAsNonPII(claimType)));

                            claims.Add(new Claim(claimType, item.ToString(), valueType));
                        }
                    }
                    else
                    {
                        claims.Add(new Claim(claimType, claimValue.ToString(), valueType));
                    }
                }
            }

            return claims;
        }

        /// <summary>
        /// Merges <paramref name="claims"/> and <paramref name="subjectClaims"/>
        /// </summary>
        /// <param name="claims"> A dictionary of claims.</param>
        /// <param name="subjectClaims"> A collection of <see cref="Claim"/>'s</param>
        /// <returns> A merged list of <see cref="Claim"/>'s.</returns>
        internal static IEnumerable<Claim> GetAllClaims(IDictionary<string, object> claims, IEnumerable<Claim> subjectClaims)
        {
            if (claims == null)
                return subjectClaims;
            else
                return TokenUtilities.MergeClaims(CreateClaimsFromDictionary(claims), subjectClaims);
        }

        /// <summary>
        /// Gets the value type of the <see cref="Claim"/> from its value <paramref name="value"/>
        /// </summary>
        /// <param name="value"> The <see cref="Claim"/> value.</param>
        /// <returns> The value type of the <see cref="Claim"/>.</returns>
        internal static string GetXsiTypeForValue(object value)
        {
            if (value != null)
            {
                if (value is string)
                    return ClaimValueTypes.String;

                if (value is bool)
                    return ClaimValueTypes.Boolean;

                if (value is int)
                    return ClaimValueTypes.Integer32;

                if (value is long)
                    return ClaimValueTypes.Integer64;

                if (value is double)
                    return ClaimValueTypes.Double;

                if (value is DateTime)
                    return ClaimValueTypes.DateTime;
            }

            return null;
        }

        /// <summary>
        /// Fetches current configuration from the ConfigurationManager of <paramref name="validationParameters"/>
        /// and populates ValidIssuers and IssuerSigningKeys.
        /// </summary>
        /// <param name="validationParameters"> the token validation parameters to update.</param>
        /// <returns> New TokenValidationParameters with ValidIssuers and IssuerSigningKeys updated.</returns>
        internal static async Task<TokenValidationParameters> PopulateValidationParametersWithCurrentConfigurationAsync(
            TokenValidationParameters validationParameters)
        {
            if (validationParameters.ConfigurationManager == null)
            {
                return validationParameters;
            }

            var currentConfiguration = await validationParameters.ConfigurationManager.GetBaseConfigurationAsync(CancellationToken.None).ConfigureAwait(false);
            var validationParametersCloned = validationParameters.Clone();
            var issuers = new[] { currentConfiguration.Issuer };

            validationParametersCloned.ValidIssuers = (validationParametersCloned.ValidIssuers == null ? issuers : validationParametersCloned.ValidIssuers.Concat(issuers));
            validationParametersCloned.IssuerSigningKeys = (validationParametersCloned.IssuerSigningKeys == null ? currentConfiguration.SigningKeys : validationParametersCloned.IssuerSigningKeys.Concat(currentConfiguration.SigningKeys));
            return validationParametersCloned;

        }
    }
}
