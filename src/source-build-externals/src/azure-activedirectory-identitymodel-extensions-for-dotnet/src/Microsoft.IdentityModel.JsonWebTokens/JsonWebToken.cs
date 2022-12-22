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
using Microsoft.IdentityModel.Json;
using Microsoft.IdentityModel.Json.Linq;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.JsonWebTokens
{
    /// <summary>
    /// A <see cref="SecurityToken"/> designed for representing a JSON Web Token (JWT). 
    /// </summary>
    public class JsonWebToken : SecurityToken
    {
        /// <summary>
        /// Initializes a new instance of <see cref="JsonWebToken"/> from a string in JWS or JWE Compact serialized format.
        /// </summary>
        /// <param name="jwtEncodedString">A JSON Web Token that has been serialized in JWS or JWE Compact serialized format.</param>
        /// <exception cref="ArgumentNullException">'jwtEncodedString' is null or empty.</exception>
        /// <exception cref="ArgumentException">'jwtEncodedString' is not in JWS or JWE Compact serialization format.</exception>
        /// <remarks>
        /// The contents of the returned <see cref="JsonWebToken"/> have not been validated, the JSON Web Token is simply decoded. Validation can be accomplished using the validation methods in <see cref="JsonWebTokenHandler"/>
        /// </remarks>
        public JsonWebToken(string jwtEncodedString)
        {
            if (string.IsNullOrEmpty(jwtEncodedString))
                throw new ArgumentNullException(nameof(jwtEncodedString));

            // Max number of segments is set to JwtConstants.MaxJwtSegmentCount + 1 so that we know if there were more than 5 segments present.
            // In the case where JwtEncodedString has greater than 5 segments, the length of tokenParts will always be 6.
            var tokenParts = jwtEncodedString.Split(new char[] { '.' }, JwtConstants.MaxJwtSegmentCount + 1);
            if (tokenParts.Length == JwtConstants.JwsSegmentCount || tokenParts.Length == JwtConstants.JweSegmentCount)
                Decode(tokenParts, jwtEncodedString);
            else
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX14100, jwtEncodedString)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonWebToken"/> class where the header contains the crypto algorithms applied to the encoded header and payload.
        /// </summary>
        /// <param name="header">A string containing JSON which represents the cryptographic operations applied to the JWT and optionally any additional properties of the JWT.</param>
        /// <param name="payload">A string containing JSON which represents the claims contained in the JWT. Each claim is a JSON object of the form { Name, Value }.</param>
        /// <exception cref="ArgumentNullException">'header' is null.</exception>
        /// <exception cref="ArgumentNullException">'payload' is null.</exception>
        public JsonWebToken(string header, string payload)
        {
            if (string.IsNullOrEmpty(header))
                throw LogHelper.LogArgumentNullException(nameof(header));

            if (string.IsNullOrEmpty(payload))
                throw LogHelper.LogArgumentNullException(nameof(payload));

            try
            {
                Header = JObject.Parse(header);
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX14301, header), ex));
            }

            try
            {
                Payload = JObject.Parse(payload);
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX14302, payload), ex));
            }
        }

        /// <summary>
        /// Gets the 'value' of the 'actort' claim { actort, 'value' }.
        /// </summary>
        /// <remarks>If the 'actort' claim is not found, an empty string is returned.</remarks> 
        public string Actor => Payload.Value<string>(JwtRegisteredClaimNames.Actort) ?? string.Empty;

        /// <summary>
        /// Gets the 'value' of the 'alg' claim { alg, 'value' }.
        /// </summary>
        /// <remarks>If the 'alg' claim is not found, an empty string is returned.</remarks>   
        public string Alg => Header.Value<string>(JwtHeaderParameterNames.Alg) ?? string.Empty;

        /// <summary>
        /// Gets the list of 'aud' claim { aud, 'value' }.
        /// </summary>
        /// <remarks>If the 'aud' claim is not found, enumeration will be empty.</remarks>
        public IEnumerable<string> Audiences
        {
            get
            {
                if (Payload.GetValue(JwtRegisteredClaimNames.Aud, StringComparison.Ordinal) is JToken value)
                {
                    if (value.Type is JTokenType.String)
                        return new List<string> { value.ToObject<string>() };
                    else if (value.Type is JTokenType.Array)
                        return value.ToObject<List<string>>();
                }

                return Enumerable.Empty<string>();
            }
        }

        /// <summary>
        /// Gets the AuthenticationTag from the original raw data of this instance when it was created.
        /// </summary>
        /// <remarks>The original JSON Compact serialized format passed into the constructor. <see cref="JsonWebToken(string)"/></remarks>
        public string AuthenticationTag { get; private set; }

        /// <summary>
        /// Gets the Ciphertext from the original raw data of this instance when it was created.
        /// </summary>
        /// <remarks>The original JSON Compact serialized format passed into the constructor. <see cref="JsonWebToken(string)"/></remarks>
        public string Ciphertext { get; private set; }

        /// <summary>
        /// Gets a <see cref="IEnumerable{Claim}"/><see cref="Claim"/> for each JSON { name, value }.
        /// </summary>
        public virtual IEnumerable<Claim> Claims
        {
            get
            {
                if (InnerToken != null)
                    return InnerToken.Claims;

                if (!Payload.HasValues)
                    return Enumerable.Empty<Claim>();

                var claims = new List<Claim>();
                string issuer = this.Issuer ?? ClaimsIdentity.DefaultIssuer;

                // there is some code redundancy here that was not factored as this is a high use method. Each identity received from the host will pass through here.
                foreach (var entry in Payload)
                {
                    if (entry.Value == null)
                    {
                        claims.Add(new Claim(entry.Key, string.Empty, JsonClaimValueTypes.JsonNull, issuer, issuer));
                        continue;
                    }

                    if (entry.Value.Type is JTokenType.String)
                    {
                        var claimValue = entry.Value.ToObject<string>();
                        claims.Add(new Claim(entry.Key, claimValue, ClaimValueTypes.String, issuer, issuer));
                        continue;
                    }

                    var jtoken = entry.Value;
                    if (jtoken != null)
                    {
                        AddClaimsFromJToken(claims, entry.Key, jtoken, issuer);
                        continue;
                    }

                }

                return claims;
            }
        }

        /// <summary>
        /// Gets the 'value' of the 'cty' claim { cty, 'value' }.
        /// </summary>
        /// <remarks>If the 'cty' claim is not found, an empty string is returned.</remarks>   
        public string Cty => Header.Value<string>(JwtHeaderParameterNames.Cty) ?? string.Empty;

        /// <summary>
        /// Gets the 'value' of the 'enc' claim { enc, 'value' }.
        /// </summary>
        /// <remarks>If the 'enc' value is not found, an empty string is returned.</remarks>   
        public string Enc => Header.Value<string>(JwtHeaderParameterNames.Enc) ?? string.Empty;

        /// <summary>
        /// Gets the EncryptedKey from the original raw data of this instance when it was created.
        /// </summary>
        /// <remarks>The original JSON Compact serialized format passed into the constructor. <see cref="JsonWebToken(string)"/></remarks>
        public string EncryptedKey { get; private set; }

        /// <summary>
        /// Represents the cryptographic operations applied to the JWT and optionally any additional properties of the JWT. 
        /// </summary>
        internal JObject Header { get; private set; } = new JObject();

        /// <summary>
        /// Gets the 'value' of the 'jti' claim { jti, ''value' }.
        /// </summary>
        /// <remarks>If the 'jti' claim is not found, an empty string is returned.</remarks>
        public override string Id => Payload.Value<string>(JwtRegisteredClaimNames.Jti) ?? string.Empty;

        /// <summary>
        /// Gets the InitializationVector from the original raw data of this instance when it was created.
        /// </summary>
        /// <remarks>The original JSON Compact serialized format passed into the constructor. <see cref="JsonWebToken(string)"/></remarks>
        public string InitializationVector { get; private set; }

        /// <summary>
        /// Gets the <see cref="JsonWebToken"/> associated with this instance.
        /// </summary>
        public JsonWebToken InnerToken { get; internal set; }

        /// <summary>
        /// Gets the 'value' of the 'iat' claim { iat, 'value' } converted to a <see cref="DateTime"/> assuming 'value' is seconds since UnixEpoch (UTC 1970-01-01T0:0:0Z).
        /// </summary>
        /// <remarks>If the 'iat' claim is not found, then <see cref="DateTime.MinValue"/> is returned.</remarks>
        public DateTime IssuedAt => JwtTokenUtilities.GetDateTime(JwtRegisteredClaimNames.Iat, Payload);

        /// <summary>
        /// Gets the 'value' of the 'iss' claim { iss, 'value' }.
        /// </summary>
        /// <remarks>If the 'iss' claim is not found, an empty string is returned.</remarks>   
        public override string Issuer => Payload.Value<string>(JwtRegisteredClaimNames.Iss) ?? string.Empty;

        /// <summary>
        /// Gets the 'value' of the 'kid' claim { kid, 'value' }.
        /// </summary>
        /// <remarks>If the 'kid' claim is not found, an empty string is returned.</remarks>   
        public string Kid => Header.Value<string>(JwtHeaderParameterNames.Kid) ?? string.Empty;

        /// <summary>
        /// Represents the JSON payload.
        /// </summary>
        internal JObject Payload { get; private set; } = new JObject();

        /// <summary>
        /// Gets the EncodedHeader from the original raw data of this instance when it was created.
        /// </summary>
        /// <remarks>The original JSON Compact serialized format passed into the constructor. <see cref="JsonWebToken(string)"/></remarks>
        public string EncodedHeader { get; internal set; }

        /// <summary>
        /// Gets the EncodedPayload from the original raw data of this instance when it was created.
        /// </summary>
        /// <remarks>The original JSON Compact serialized format passed into the constructor. <see cref="JsonWebToken(string)"/></remarks>
        public string EncodedPayload { get; internal set; }

        /// <summary>
        /// Gets the EncodedSignature from the original raw data of this instance when it was created.
        /// </summary>
        /// <remarks>The original JSON Compact serialized format passed into the constructor. <see cref="JsonWebToken(string)"/></remarks>
        public string EncodedSignature { get; internal set; }

        /// <summary>
        /// Gets the original raw data of this instance when it was created.
        /// </summary>
        public string EncodedToken { get; private set; }

        /// <summary>
        /// Not implemented.
        /// </summary>
        public override SecurityKey SecurityKey { get; }

        /// <summary>
        /// Not implemented.
        /// </summary>
        public override SecurityKey SigningKey
        {
            set;
            get;
        }

        /// <summary>
        /// Gets the 'value' of the 'sub' claim { sub, 'value' }.
        /// </summary>
        /// <remarks>If the 'sub' claim is not found, an empty string is returned.</remarks>   
        public string Subject => Payload.Value<string>(JwtRegisteredClaimNames.Sub) ?? string.Empty;

        /// <summary>
        /// Gets the 'value' of the 'typ' claim { typ, 'value' }.
        /// </summary>
        /// <remarks>If the 'typ' claim is not found, an empty string is returned.</remarks>   
        public string Typ => Header.Value<string>(JwtHeaderParameterNames.Typ) ?? string.Empty;

        /// <summary>
        /// Gets the 'value' of the 'nbf' claim { nbf, 'value' } converted to a <see cref="DateTime"/> assuming 'value' is seconds since UnixEpoch (UTC 1970-01-01T0:0:0Z).
        /// </summary>
        /// <remarks>If the 'nbf' claim is not found, then <see cref="DateTime.MinValue"/> is returned.</remarks>
        public override DateTime ValidFrom => JwtTokenUtilities.GetDateTime(JwtRegisteredClaimNames.Nbf, Payload);

        /// <summary>
        /// Gets the 'value' of the 'exp' claim { exp, 'value' } converted to a <see cref="DateTime"/> assuming 'value' is seconds since UnixEpoch (UTC 1970-01-01T0:0:0Z).
        /// </summary>
        /// <remarks>If the 'exp' claim is not found, then <see cref="DateTime.MinValue"/> is returned.</remarks>
        public override DateTime ValidTo => JwtTokenUtilities.GetDateTime(JwtRegisteredClaimNames.Exp, Payload);

        /// <summary>
        /// Gets the 'value' of the 'x5t' claim { x5t, 'value' }.
        /// </summary>
        /// <remarks>If the 'x5t' claim is not found, an empty string is returned.</remarks>   
        public string X5t => Header.Value<string>(JwtHeaderParameterNames.X5t) ?? string.Empty;

        /// <summary>
        /// Gets the 'value' of the 'zip' claim { zip, 'value' }.
        /// </summary>
        /// <remarks>If the 'zip' claim is not found, an empty string is returned.</remarks>   
        public string Zip => Header.Value<string>(JwtHeaderParameterNames.Zip) ?? String.Empty;

        /// <summary>
        /// Decodes the string into the header, payload and signature.
        /// </summary>
        /// <param name="tokenParts">the tokenized string.</param>
        /// <param name="rawData">the original token.</param>
        private void Decode(string[] tokenParts, string rawData)
        {
            try
            {
                Header = JObject.Parse(Base64UrlEncoder.Decode(tokenParts[0]));
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX14102, tokenParts[0], rawData), ex));
            }

            if (tokenParts.Length == JwtConstants.JweSegmentCount)
                DecodeJwe(tokenParts);
            else
                DecodeJws(tokenParts);

            EncodedToken = rawData;
        }

        private static void AddClaimsFromJToken(List<Claim> claims, string claimType, JToken jtoken, string issuer)
        {
            if (jtoken.Type is JTokenType.Object)
            {
                claims.Add(new Claim(claimType, jtoken.ToString(Formatting.None), JsonClaimValueTypes.Json, issuer, issuer));
            }
            else if (jtoken.Type is JTokenType.Array)
            {
                var jarray = jtoken as JArray;
                foreach (var item in jarray)
                {
                    switch (item.Type)
                    {
                        case JTokenType.Object:
                            claims.Add(new Claim(claimType, item.ToString(Formatting.None), JsonClaimValueTypes.Json, issuer, issuer));
                            break;

                        // only go one level deep on arrays.
                        case JTokenType.Array:
                            claims.Add(new Claim(claimType, item.ToString(Formatting.None), JsonClaimValueTypes.JsonArray, issuer, issuer));
                            break;

                        default:
                            AddDefaultClaimFromJToken(claims, claimType, item, issuer);
                            break;
                    }
                }
            }
            else
            {
                AddDefaultClaimFromJToken(claims, claimType, jtoken, issuer);
            }
        }

        private static void AddDefaultClaimFromJToken(List<Claim> claims, string claimType, JToken jtoken, string issuer)
        {
            if (jtoken is JValue jvalue)
            {
                // String is special because item.ToString(Formatting.None) will result in "/"string/"". The quotes will be added.
                // Boolean needs item.ToString otherwise 'true' => 'True'
                if (jvalue.Type is JTokenType.String)
                    claims.Add(new Claim(claimType, jvalue.Value.ToString(), ClaimValueTypes.String, issuer, issuer));
                // DateTime claims require special processing. jtoken.ToString(Formatting.None) will result in "\"dateTimeValue\"". The quotes will be added.
                else if (jvalue.Value is DateTime dateTimeValue)
                    claims.Add(new Claim(claimType, dateTimeValue.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture), ClaimValueTypes.DateTime, issuer, issuer));
                else
                    claims.Add(new Claim(claimType, jtoken.ToString(Formatting.None), GetClaimValueType(jvalue.Value), issuer, issuer));
            }
            else
                claims.Add(new Claim(claimType, jtoken.ToString(Formatting.None), GetClaimValueType(jtoken), issuer, issuer));
        }

        /// <summary>
        /// Decodes the payload and signature from the JWE parts.
        /// </summary>
        /// <param name="tokenParts">Parts of the JWE including the header.</param>
        /// <remarks>
        /// Assumes Header has already been set.
        /// According to the JWE documentation (https://datatracker.ietf.org/doc/html/rfc7516#section-2), it is possible for the EncryptedKey, InitializationVector, and AuthenticationTag to be empty strings.
        /// </remarks>
        private void DecodeJwe(string[] tokenParts)
        {
            EncodedHeader = tokenParts[0];
            EncryptedKey = tokenParts[1];
            InitializationVector = tokenParts[2];
            Ciphertext = !string.IsNullOrWhiteSpace(tokenParts[3]) ? tokenParts[3] : throw LogHelper.LogExceptionMessage(new ArgumentException(LogMessages.IDX14306));
            AuthenticationTag = tokenParts[4];
        }

        /// <summary>
        /// Decodes the payload and signature from the JWS parts.
        /// </summary>
        /// <param name="tokenParts">Parts of the JWS including the header.</param>
        /// <remarks>Assumes Header has already been set.</remarks>
        private void DecodeJws(string[] tokenParts)
        {
            // Log if CTY is set, assume compact JWS
            if (!string.IsNullOrEmpty(Cty))
                LogHelper.LogVerbose(LogHelper.FormatInvariant(LogMessages.IDX14105, Payload.Value<string>(JwtHeaderParameterNames.Cty)));

            try
            {
                Payload = JObject.Parse(Base64UrlEncoder.Decode(tokenParts[1]));
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX14101, tokenParts[1], EncodedToken), ex));
            }

            EncodedHeader = tokenParts[0];
            EncodedPayload = tokenParts[1];
            EncodedSignature = tokenParts[2];
        }

        private static string GetClaimValueType(object obj)
        {
            if (obj == null)
                return JsonClaimValueTypes.JsonNull;

            var objType = obj.GetType();

            if (objType == typeof(string))
                return ClaimValueTypes.String;

            if (objType == typeof(int))
                return ClaimValueTypes.Integer;

            if (objType == typeof(bool))
                return ClaimValueTypes.Boolean;

            if (objType == typeof(double))
                return ClaimValueTypes.Double;

            if (objType == typeof(long))
            {
                long l = (long)obj;
                if (l >= int.MinValue && l <= int.MaxValue)
                    return ClaimValueTypes.Integer;

                return ClaimValueTypes.Integer64;
            }

            if (objType == typeof(DateTime))
                return ClaimValueTypes.DateTime;

            if (objType == typeof(JObject))
                return JsonClaimValueTypes.Json;

            if (objType == typeof(JArray))
                return JsonClaimValueTypes.JsonArray;

            return objType.ToString();
        }

        /// <summary>
        /// Gets a <see cref="Claim"/> representing the { key, 'value' } pair corresponding to the provided <paramref name="key"/>.
        /// </summary>
        /// <remarks>If the key has no corresponding value, this method will throw.</remarks>   
        public Claim GetClaim(string key)
        {
            string issuer = Issuer ?? ClaimsIdentity.DefaultIssuer;

            if (!Payload.TryGetValue(key, out var jTokenValue))
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX14304, key)));

            if (jTokenValue.Type == JTokenType.Null)
                return new Claim(key, string.Empty, JsonClaimValueTypes.JsonNull, issuer, issuer);
            else if (jTokenValue.Type is JTokenType.Object)
                return new Claim(key, jTokenValue.ToString(Formatting.None), JsonClaimValueTypes.Json, issuer, issuer);
            else if (jTokenValue.Type is JTokenType.Array)
                return new Claim(key, jTokenValue.ToString(Formatting.None), JsonClaimValueTypes.JsonArray, issuer, issuer);
            else if (jTokenValue is JValue jvalue)
            {
                // String is special because item.ToString(Formatting.None) will result in "/"string/"". The quotes will be added.
                // Boolean needs item.ToString otherwise 'true' => 'True'
                if (jvalue.Type is JTokenType.String)
                    return new Claim(key, jvalue.Value.ToString(), ClaimValueTypes.String, issuer, issuer);
                // DateTime claims require special processing. jTokenValue.ToString(Formatting.None) will result in "\"dateTimeValue\"". The quotes will be added.
                else if (jvalue.Value is DateTime dateTimeValue)
                    return new Claim(key, dateTimeValue.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture), ClaimValueTypes.DateTime, issuer, issuer);
                else
                    return new Claim(key, jTokenValue.ToString(Formatting.None), GetClaimValueType(jvalue.Value), issuer, issuer);
            }
            else
                return new Claim(key, jTokenValue.ToString(Formatting.None), GetClaimValueType(jTokenValue), issuer, issuer);
        }

        /// <summary>
        /// Gets the 'value' corresponding to the provided key from the JWT payload { key, 'value' }.
        /// </summary>
        /// <remarks>If the key has no corresponding value, this method will throw. </remarks>   
        public T GetPayloadValue<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw LogHelper.LogArgumentNullException(nameof(key));

            if (typeof(T).Equals(typeof(Claim)))
                return (T)(object)GetClaim(key);

            if (!Payload.TryGetValue(key, out var jTokenValue))
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX14304, key)));

            T value;
            try
            {
                value = jTokenValue.ToObject<T>();
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX14305, key, LogHelper.MarkAsNonPII(typeof(T)), LogHelper.MarkAsNonPII(jTokenValue.Type)), ex));
            }

            return value;
        }

        /// <summary>
        /// Tries to get the <see cref="Claim"/> representing the { key, 'value' } pair corresponding to the provided <paramref name="key"/>.
        /// </summary>
        /// <remarks>If the key has no corresponding value, returns false. Otherwise returns true. </remarks>   
        public bool TryGetClaim(string key, out Claim value)
        {
            string issuer = Issuer ?? ClaimsIdentity.DefaultIssuer;

            if (!Payload.TryGetValue(key, out var jTokenValue))
            {
                value = null;
                return false;
            }

            if (jTokenValue.Type == JTokenType.Null)
                value = new Claim(key, string.Empty, JsonClaimValueTypes.JsonNull, issuer, issuer);
            else if (jTokenValue.Type is JTokenType.Object)
                value = new Claim(key, jTokenValue.ToString(Formatting.None), JsonClaimValueTypes.Json, issuer, issuer);
            else if (jTokenValue.Type is JTokenType.Array)
                value = new Claim(key, jTokenValue.ToString(Formatting.None), JsonClaimValueTypes.JsonArray, issuer, issuer);
            else if (jTokenValue is JValue jvalue)
            {
                // String is special because item.ToString(Formatting.None) will result in "/"string/"". The quotes will be added.
                // Boolean needs item.ToString otherwise 'true' => 'True'
                if (jvalue.Type is JTokenType.String)
                    value = new Claim(key, jvalue.Value.ToString(), ClaimValueTypes.String, issuer, issuer);
                // DateTime claims require special processing. jTokenValue.ToString(Formatting.None) will result in "\"dateTimeValue\"". The quotes will be added.
                else if (jvalue.Value is DateTime dateTimeValue)
                    value = new Claim(key, dateTimeValue.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture), ClaimValueTypes.DateTime, issuer);
                else
                    value = new Claim(key, jTokenValue.ToString(Formatting.None), GetClaimValueType(jvalue.Value), issuer, issuer);
            }
            else
                value = new Claim(key, jTokenValue.ToString(Formatting.None), GetClaimValueType(jTokenValue), issuer, issuer);

            return true;
        }

        /// <summary>
        /// Tries to get the 'value' corresponding to the provided key from the JWT payload { key, 'value' }.
        /// </summary>
        /// <remarks>If the key has no corresponding value, returns false. Otherwise returns true. </remarks>   
        public bool TryGetPayloadValue<T>(string key, out T value)
        {
            if (string.IsNullOrEmpty(key))
            {
                value = default(T);
                return false;
            }

            if (typeof(T).Equals(typeof(Claim)))
            {
                var foundClaim = TryGetClaim(key, out var claim);
                value = (T)(object)claim;
                return foundClaim;
            }

            if (!Payload.TryGetValue(key, out var jTokenValue))
            {
                value = default(T);
                return false;
            }

            try
            {
                value = jTokenValue.ToObject<T>();
            }
            catch (Exception)
            {
                value = default(T);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the 'value' corresponding to the provided key from the JWT header { key, 'value' }.
        /// </summary>
        /// <remarks>If the key has no corresponding value, this method will throw. </remarks>   
        public T GetHeaderValue<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw LogHelper.LogArgumentNullException(nameof(key));

            if (!Header.TryGetValue(key, out var jTokenValue))
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX14303, key)));

            T value;
            try
            {
                value = jTokenValue.ToObject<T>();
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX14305, key, LogHelper.MarkAsNonPII(typeof(T)), LogHelper.MarkAsNonPII(jTokenValue.Type)), ex));
            }

            return value;
        }

        /// <summary>
        /// Tries to get the value corresponding to the provided key from the JWT header { key, 'value' }.
        /// </summary>
        /// <remarks>If the key has no corresponding value, returns false. Otherwise returns true. </remarks>   
        public bool TryGetHeaderValue<T>(string key, out T value)
        {
            if (string.IsNullOrEmpty(key))
            {
                value = default(T);
                return false;
            }

            if (!Header.TryGetValue(key, out var jTokenValue))
            {
                value = default(T);
                return false;
            }

            try
            {
                value = jTokenValue.ToObject<T>();
            }
            catch (Exception)
            {
                value = default(T);
                return false;
            }

            return true;
        }
    }
}
