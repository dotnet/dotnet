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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Json.Linq;
using Microsoft.IdentityModel.Logging;
using TokenLogMessages = Microsoft.IdentityModel.Tokens.LogMessages;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// A class which contains useful methods for processing tokens.
    /// </summary>
    internal class TokenUtilities
    {
        /// <summary>
        /// A URI that represents the JSON XML data type.
        /// </summary>
        /// <remarks>When mapping json to .Net Claim(s), if the value was not a string (or an enumeration of strings), the ClaimValue will serialized using the current JSON serializer, a property will be added with the .Net type and the ClaimTypeValue will be set to 'JsonClaimValueType'.</remarks>
        internal const string Json = "JSON";

        /// <summary>
        /// A URI that represents the JSON array XML data type.
        /// </summary>
        /// <remarks>When mapping json to .Net Claim(s), if the value was not a string (or an enumeration of strings), the ClaimValue will serialized using the current JSON serializer, a property will be added with the .Net type and the ClaimTypeValue will be set to 'JsonClaimValueType'.</remarks>
        internal const string JsonArray = "JSON_ARRAY";

        /// <summary>
        /// A URI that represents the JSON null data type
        /// </summary>
        /// <remarks>When mapping json to .Net Claim(s), we use empty string to represent the claim value and set the ClaimValueType to JsonNull</remarks>
        internal const string JsonNull = "JSON_NULL";

        /// <summary>
        /// Creates a dictionary from a list of Claim's.
        /// </summary>
        /// <param name="claims"> A list of claims.</param>
        /// <returns> A Dictionary representing claims.</returns>
        internal static IDictionary<string, object> CreateDictionaryFromClaims(IEnumerable<Claim> claims)
        {
            var payload = new Dictionary<string, object>();

            if (claims == null)
                return payload;

            foreach (Claim claim in claims)
            {
                if (claim == null)
                    continue;

                string jsonClaimType = claim.Type;
                object jsonClaimValue = claim.ValueType.Equals(ClaimValueTypes.String) ? claim.Value : GetClaimValueUsingValueType(claim);
                object existingValue;

                // If there is an existing value, append to it.
                // What to do if the 'ClaimValueType' is not the same.
                if (payload.TryGetValue(jsonClaimType, out existingValue))
                {
                    IList<object> claimValues = existingValue as IList<object>;
                    if (claimValues == null)
                    {
                        claimValues = new List<object>();
                        claimValues.Add(existingValue);
                        payload[jsonClaimType] = claimValues;
                    }

                    claimValues.Add(jsonClaimValue);
                }
                else
                {
                    payload[jsonClaimType] = jsonClaimValue;
                }
            }

            return payload;
        }

        internal static object GetClaimValueUsingValueType(Claim claim)
        {
            if (claim.ValueType == ClaimValueTypes.String)
                return claim.Value;

            if (claim.ValueType == ClaimValueTypes.Boolean && bool.TryParse(claim.Value, out bool boolValue))
                return boolValue;

            if (claim.ValueType == ClaimValueTypes.Double && double.TryParse(claim.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double doubleValue))
                return doubleValue;

            if ((claim.ValueType == ClaimValueTypes.Integer || claim.ValueType == ClaimValueTypes.Integer32) && int.TryParse(claim.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out int intValue))
                return intValue;

            if (claim.ValueType == ClaimValueTypes.Integer64 && long.TryParse(claim.Value, out long longValue))
                return longValue;

            if (claim.ValueType == ClaimValueTypes.DateTime && DateTime.TryParse(claim.Value, out DateTime dateTimeValue))
                return dateTimeValue;

            if (claim.ValueType == Json)
                return JObject.Parse(claim.Value);

            if (claim.ValueType == JsonArray)
                return JArray.Parse(claim.Value);

            if (claim.ValueType == JsonNull)
                return string.Empty;

            return claim.Value;
        }

        /// <summary>
        /// Returns all <see cref="SecurityKey"/> provided in validationParameters.
        /// </summary>
        /// <param name="validationParameters">A <see cref="TokenValidationParameters"/> required for validation.</param>
        /// <returns>Returns all <see cref="SecurityKey"/> provided in validationParameters.</returns>
        internal static IEnumerable<SecurityKey> GetAllSigningKeys(TokenValidationParameters validationParameters)
        {
            LogHelper.LogInformation(TokenLogMessages.IDX10243);
            if (validationParameters.IssuerSigningKey != null)
                yield return validationParameters.IssuerSigningKey;

            if (validationParameters.IssuerSigningKeys != null)
                foreach (SecurityKey key in validationParameters.IssuerSigningKeys)
                    yield return key;
        }


        /// <summary>
        /// Returns all <see cref="SecurityKey"/> provided in <paramref name="configuration"/>.
        /// </summary>
        /// <param name="configuration">The <see cref="BaseConfiguration"/> that contains signing keys used for validation.</param>
        /// <returns>Returns all <see cref="SecurityKey"/> provided in provided in <paramref name="configuration"/>.</returns>
        internal static IEnumerable<SecurityKey> GetAllSigningKeys(BaseConfiguration configuration)
        {
            LogHelper.LogInformation(TokenLogMessages.IDX10265);

            if (configuration?.SigningKeys != null)
                foreach (SecurityKey key in configuration.SigningKeys)
                    yield return key;
        }

        /// <summary>
        /// Returns all <see cref="SecurityKey"/> provided in <paramref name="configuration"/> and <paramref name="validationParameters"/>.
        /// </summary>
        /// <param name="configuration">The <see cref="BaseConfiguration"/> that contains signing keys used for validation.</param>
        /// <param name="validationParameters">A <see cref="TokenValidationParameters"/> required for validation.</param>
        /// <returns>Returns all <see cref="SecurityKey"/> provided in provided in <paramref name="configuration"/> and <paramref name="validationParameters"/>.</returns>
        internal static IEnumerable<SecurityKey> GetAllSigningKeys(TokenValidationParameters validationParameters, BaseConfiguration configuration)
        {
            LogHelper.LogInformation(TokenLogMessages.IDX10264);

            return GetAllSigningKeys(configuration).Concat(GetAllSigningKeys(validationParameters));
        }

        /// <summary>
        /// Merges claims. If a claim with same type exists in both <paramref name="claims"/> and <paramref name="subjectClaims"/>, the one in claims will be kept.
        /// </summary>
        /// <param name="claims"> Collection of <see cref="Claim"/>'s.</param>
        /// <param name="subjectClaims"> Collection of <see cref="Claim"/>'s.</param>
        /// <returns> A Merged list of <see cref="Claim"/>'s.</returns>
        internal static IEnumerable<Claim> MergeClaims(IEnumerable<Claim> claims, IEnumerable<Claim> subjectClaims)
        {
            if (claims == null)
                return subjectClaims;

            if (subjectClaims == null)
                return claims;

            List<Claim> result = claims.ToList();

            foreach (Claim claim in subjectClaims)
            {
                if (!claims.Any(i => i.Type == claim.Type))
                    result.Add(claim);
            }

            return result;
        }

        /// <summary>
        /// Check whether the given exception type is recoverable by LKG.
        /// </summary>
        /// <param name="exception">The exception to check.</param>
        /// <returns><c>true</c> if the exception is certain types of exceptions otherwise, <c>false</c>.</returns>
        internal static bool IsRecoverableException(Exception exception)
        {
            // using 'GetType()' instead of 'is' as SecurityTokenUnableToValidException (and others) extend SecurityTokenInvalidSignatureException
            // we want to make sure that the clause for SecurityTokenUnableToValidateException is hit so that the ValidationFailure is checked
            return exception.GetType().Equals(typeof(SecurityTokenInvalidSignatureException))
                   || exception is SecurityTokenInvalidSigningKeyException
                   || exception is SecurityTokenInvalidIssuerException
                   // we should not try to revalidate with the LKG or request a refresh if the token has an invalid lifetime
                   || (exception as SecurityTokenUnableToValidateException)?.ValidationFailure != ValidationFailure.InvalidLifetime
                   || exception is SecurityTokenSignatureKeyNotFoundException;
        }

        /// <summary>
        /// Check whether the given configuration is recoverable by LKG.
        /// </summary>
        /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validation.</param>
        /// <param name="configuration">The <see cref="BaseConfiguration"/> to check.</param>
        /// <param name="currentConfiguration">The updated <see cref="BaseConfiguration"/>.</param>
        /// <returns><c>true</c> if the configuration is recoverable otherwise, <c>false</c>.</returns>
        internal static bool IsRecoverableConfiguration(TokenValidationParameters validationParameters, BaseConfiguration configuration, out BaseConfiguration currentConfiguration)
        {
            bool isRecoverableConfiguration = (validationParameters.ConfigurationManager.UseLastKnownGoodConfiguration
                && validationParameters.ConfigurationManager.LastKnownGoodConfiguration != null
                && !ReferenceEquals(configuration, validationParameters.ConfigurationManager.LastKnownGoodConfiguration));

            currentConfiguration = configuration;
            if (isRecoverableConfiguration)
            {
                // Inform the user that the LKG is expired.
                if (!validationParameters.ConfigurationManager.IsLastKnownGoodValid)
                {
                    LogHelper.LogInformation(TokenLogMessages.IDX10263);
                    return false;
                }
                else
                    currentConfiguration = validationParameters.ConfigurationManager.LastKnownGoodConfiguration;
            }

            return isRecoverableConfiguration;
        }
    }
}
