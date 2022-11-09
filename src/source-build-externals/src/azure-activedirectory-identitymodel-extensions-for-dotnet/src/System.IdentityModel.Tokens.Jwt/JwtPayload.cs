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

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Json;
using Microsoft.IdentityModel.Json.Linq;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace System.IdentityModel.Tokens.Jwt
{
    /// <summary>
    /// Initializes a new instance of <see cref="JwtPayload"/> which contains JSON objects representing the claims contained in the JWT. Each claim is a JSON object of the form { Name, Value }.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable"), System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Serialize not really supported.")]
    public class JwtPayload : Dictionary<string, object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JwtPayload"/> class with no claims. Default string comparer <see cref="StringComparer.Ordinal"/>. 
        /// Creates a empty <see cref="JwtPayload"/>
        /// </summary>
        public JwtPayload()
            : this(issuer: null, audience: null, claims: null, notBefore: null, expires: null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtPayload"/> class with <see cref="IEnumerable{Claim}"/>. Default string comparer <see cref="StringComparer.Ordinal"/>.
        /// <param name="claims">The claims to add.</param>
        /// </summary>
        public JwtPayload(IEnumerable<Claim> claims)
            : this(issuer: null, audience: null, claims: claims, notBefore: null, expires: null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtPayload"/> class with claims added for each parameter specified. Default string comparer <see cref="StringComparer.Ordinal"/>. 
        /// </summary>
        /// <param name="issuer">If this value is not null, a { iss, 'issuer' } claim will be added, overwriting any 'iss' claim in 'claims' if present.</param>
        /// <param name="audience">If this value is not null, a { aud, 'audience' } claim will be added, appending to any 'aud' claims in 'claims' if present.</param>
        /// <param name="claims">If this value is not null then for each <see cref="Claim"/> a { 'Claim.Type', 'Claim.Value' } is added. If duplicate claims are found then a { 'Claim.Type', List&lt;object&gt; } will be created to contain the duplicate values.</param>
        /// <param name="notBefore">If notbefore.HasValue a { nbf, 'value' } claim is added, overwriting any 'nbf' claim in 'claims' if present.</param>
        /// <param name="expires">If expires.HasValue a { exp, 'value' } claim is added, overwriting any 'exp' claim in 'claims' if present.</param>
        public JwtPayload(string issuer, string audience, IEnumerable<Claim> claims, DateTime? notBefore, DateTime? expires)
           : this(issuer, audience, claims, notBefore, expires, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtPayload"/> class with claims added for each parameter specified. Default string comparer <see cref="StringComparer.Ordinal"/>. 
        /// </summary>
        /// <param name="issuer">If this value is not null, a { iss, 'issuer' } claim will be added, overwriting any 'iss' claim in 'claims' if present.</param>
        /// <param name="audience">If this value is not null, a { aud, 'audience' } claim will be added, appending to any 'aud' claims in 'claims' if present.</param>
        /// <param name="claims">If this value is not null then for each <see cref="Claim"/> a { 'Claim.Type', 'Claim.Value' } is added. If duplicate claims are found then a { 'Claim.Type', List&lt;object&gt; } will be created to contain the duplicate values.</param>
        /// <param name="notBefore">If notbefore.HasValue a { nbf, 'value' } claim is added, overwriting any 'nbf' claim in 'claims' if present.</param>
        /// <param name="expires">If expires.HasValue a { exp, 'value' } claim is added, overwriting any 'exp' claim in 'claims' if present.</param>
        /// <param name="issuedAt">If issuedAt.HasValue is 'true' a { iat, 'value' } claim is added, overwriting any 'iat' claim in 'claims' if present.</param>
        /// <remarks>Comparison is set to <see cref="StringComparer.Ordinal"/>
        /// <para>The 4 parameters: 'issuer', 'audience', 'notBefore', 'expires' take precedence over <see cref="Claim"/>(s) in 'claims'. The values will be overridden.</para></remarks>
        /// <exception cref="ArgumentException">If 'expires' &lt;= 'notbefore'.</exception>
        public JwtPayload(string issuer, string audience, IEnumerable<Claim> claims, DateTime? notBefore, DateTime? expires, DateTime? issuedAt)
            : base(StringComparer.Ordinal)
        {
            if (claims != null)
                AddClaims(claims);

            AddFirstPriorityClaims(issuer, audience, notBefore, expires, issuedAt);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtPayload"/> class with claims added for each parameter specified. Default string comparer <see cref="StringComparer.Ordinal"/>. 
        /// </summary>
        /// <param name="issuer">If this value is not null, a { iss, 'issuer' } claim will be added, overwriting any 'iss' claim in 'claims' and 'claimCollection' if present.</param>
        /// <param name="audience">If this value is not null, a { aud, 'audience' } claim will be added, appending to any 'aud' claims in 'claims' or 'claimCollection' if present.</param>
        /// <param name="claims">If this value is not null then for each <see cref="Claim"/> a { 'Claim.Type', 'Claim.Value' } is added. If duplicate claims are found then a { 'Claim.Type', List&lt;object&gt; } will be created to contain the duplicate values.</param>
        /// <param name="claimsCollection">If both <paramref name="claims"/> and <paramref name="claimsCollection"/> are not null then the values in claims will be combined with the values in claimsCollection. The values found in claimCollection take precedence over those found in claims, so any duplicate
        /// values will be overridden.</param>
        /// <param name="notBefore">If notbefore.HasValue a { nbf, 'value' } claim is added, overwriting any 'nbf' claim in 'claims' and 'claimcollection' if present.</param>
        /// <param name="expires">If expires.HasValue a { exp, 'value' } claim is added, overwriting any 'exp' claim in 'claims' and 'claimcollection' if present.</param>
        /// <param name="issuedAt">If issuedAt.HasValue is 'true' a { iat, 'value' } claim is added, overwriting any 'iat' claim in 'claims' and 'claimcollection' if present.</param>
        /// <remarks>Comparison is set to <see cref="StringComparer.Ordinal"/>
        /// <para>The 4 parameters: 'issuer', 'audience', 'notBefore', 'expires' take precedence over <see cref="Claim"/>(s) in 'claims' and 'claimcollection'. The values will be overridden.</para></remarks>
        /// <exception cref="ArgumentException">If 'expires' &lt;= 'notbefore'.</exception>
        public JwtPayload(string issuer, string audience, IEnumerable<Claim> claims, IDictionary<string, object> claimsCollection, DateTime? notBefore, DateTime? expires, DateTime? issuedAt)
            : base(StringComparer.Ordinal)
        {
            if (claims != null)
                AddClaims(claims);

            if (claimsCollection != null && claimsCollection.Any())
                AddDictionaryClaims(claimsCollection);

            AddFirstPriorityClaims(issuer, audience, notBefore, expires, issuedAt);
        }

        /// <summary>
        /// Adds Nbf, Exp, Iat, Iss and Aud claims to payload
        /// </summary>
        /// <param name="issuer">If this value is not null, a { iss, 'issuer' } claim will be added, overwriting any 'iss' claim in <see cref="JwtPayload"/> instance.</param>
        /// <param name="audience">If this value is not null, a { aud, 'audience' } claim will be added, appending to any 'aud' claims in <see cref="JwtPayload"/> instance.</param>
        /// <param name="notBefore">If notbefore.HasValue a { nbf, 'value' } claim is added, overwriting any 'nbf' claim in <see cref="JwtPayload"/> instance.</param>
        /// <param name="expires">If expires.HasValue a { exp, 'value' } claim is added, overwriting any 'exp' claim in <see cref="JwtPayload"/> instance.</param>
        /// <param name="issuedAt">If issuedAt.HasValue is 'true' a { iat, 'value' } claim is added, overwriting any 'iat' claim in <see cref="JwtPayload"/> instance.</param>
        internal void AddFirstPriorityClaims(string issuer, string audience, DateTime? notBefore, DateTime? expires, DateTime? issuedAt)
        {
            if (expires.HasValue)
            {
                if (notBefore.HasValue)
                {
                    if (notBefore.Value >= expires.Value)
                    {
                        throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX12401, LogHelper.MarkAsNonPII(expires.Value), LogHelper.MarkAsNonPII(notBefore.Value))));
                    }

                    this[JwtRegisteredClaimNames.Nbf] = EpochTime.GetIntDate(notBefore.Value.ToUniversalTime());
                }

                this[JwtRegisteredClaimNames.Exp] = EpochTime.GetIntDate(expires.Value.ToUniversalTime());
            }

            if (issuedAt.HasValue)
                this[JwtRegisteredClaimNames.Iat] = EpochTime.GetIntDate(issuedAt.Value.ToUniversalTime());

            if (!string.IsNullOrEmpty(issuer))
                this[JwtRegisteredClaimNames.Iss] = issuer;

            // if could be the case that some of the claims above had an 'aud' claim;
            if (!string.IsNullOrEmpty(audience))
                AddClaim(new Claim(JwtRegisteredClaimNames.Aud, audience, ClaimValueTypes.String));
        }

        /// <summary>
        /// Gets the 'value' of the 'actor' claim { actort, 'value' }.
        /// </summary>
        /// <remarks>If the 'actor' claim is not found, null is returned.</remarks>
        public string Actort
        {
            get
            {
                return this.GetStandardClaim(JwtRegisteredClaimNames.Actort);
            }
        }

        /// <summary>
        /// Gets the 'value' of the 'acr' claim { acr, 'value' }.
        /// </summary>
        /// <remarks>If the 'acr' claim is not found, null is returned.</remarks>
        public string Acr
        {
            get
            {
                return this.GetStandardClaim(JwtRegisteredClaimNames.Acr);
            }
        }

        /// <summary>
        /// Gets the 'value' of the 'amr' claim { amr, 'value' } as list of strings.
        /// </summary>
        /// <remarks>If the 'amr' claim is not found, an empty enumerable is returned.</remarks>
        public IList<string> Amr
        {
            get
            {
                return this.GetIListClaims(JwtRegisteredClaimNames.Amr);
            }
        }

        /// <summary>
        /// Gets the 'value' of the 'auth_time' claim { auth_time, 'value' }.
        /// </summary>
        /// <remarks>If the 'auth_time' claim is not found OR could not be converted to <see cref="Int32"/>, null is returned.</remarks>
        public int? AuthTime
        {
            get
            {
                return this.GetIntClaim(JwtRegisteredClaimNames.AuthTime);
            }
        }

        /// <summary>
        /// Gets the 'value' of the 'audience' claim { aud, 'value' } as a list of strings.
        /// </summary>
        /// <remarks>If the 'audience' claim is not found, an empty enumerable is returned.</remarks>
        public IList<string> Aud
        {
            get
            {
                return this.GetIListClaims(JwtRegisteredClaimNames.Aud);
            }
        }

        /// <summary>
        /// Gets the 'value' of the 'azp' claim { azp, 'value' }.
        /// </summary>
        /// <remarks>If the 'azp' claim is not found, null is returned.</remarks>
        public string Azp
        {
            get
            {
                return this.GetStandardClaim(JwtRegisteredClaimNames.Azp);
            }
        }

        /// <summary>
        /// Gets 'value' of the 'c_hash' claim { c_hash, 'value' }.
        /// </summary>
        /// <remarks>If the 'c_hash' claim is not found, null is returned.</remarks>
        public string CHash
        {
            get
            {
                return this.GetStandardClaim(JwtRegisteredClaimNames.CHash);
            }
        }
        
        /// <summary>
        /// Gets the 'value' of the 'expiration' claim { exp, 'value' }.
        /// </summary>
        /// <remarks>If the 'expiration' claim is not found OR could not be converted to <see cref="Int32"/>, null is returned.</remarks>
        public int? Exp
        {
            get { return this.GetIntClaim(JwtRegisteredClaimNames.Exp); }
        }

        /// <summary>
        /// Gets the 'value' of the 'JWT ID' claim { jti, 'value' }.
        /// </summary>
        /// <remarks>If the 'JWT ID' claim is not found, null is returned.</remarks>
        public string Jti
        {
            get
            {
                return this.GetStandardClaim(JwtRegisteredClaimNames.Jti);
            }
        }

        /// <summary>
        /// Gets the 'value' of the 'Issued At' claim { iat, 'value' }.
        /// </summary>
        /// <remarks>If the 'Issued At' claim is not found OR cannot be converted to <see cref="Int32"/> null is returned.</remarks>
        public int? Iat
        {
            get { return this.GetIntClaim(JwtRegisteredClaimNames.Iat); }
        }

        /// <summary>
        /// Gets the 'value' of the 'issuer' claim { iss, 'value' }.
        /// </summary>
        /// <remarks>If the 'issuer' claim is not found, null is returned.</remarks>
        public string Iss
        {
            get
            {
                return this.GetStandardClaim(JwtRegisteredClaimNames.Iss);
            }
        }

        /// <summary>
        /// Gets the 'value' of the 'expiration' claim { nbf, 'value' }.
        /// </summary>
        /// <remarks>If the 'notbefore' claim is not found OR could not be converted to <see cref="Int32"/>, null is returned.</remarks>
        public int? Nbf
        {
            get { return this.GetIntClaim(JwtRegisteredClaimNames.Nbf); }
        }

        /// <summary>
        /// Gets the 'value' of the 'nonce' claim { nonce, 'value' }.
        /// </summary>
        /// <remarks>If the 'nonce' claim is not found, null is returned.</remarks>
        public string Nonce
        {
            get
            {
                return this.GetStandardClaim(JwtRegisteredClaimNames.Nonce);
            }
        }
        
        /// <summary>
        /// Gets the 'value' of the 'subject' claim { sub, 'value' }.
        /// </summary>
        /// <remarks>If the 'subject' claim is not found, null is returned.</remarks>
        public string Sub
        {
            get
            {
                return this.GetStandardClaim(JwtRegisteredClaimNames.Sub);
            }
        }

        /// <summary>
        /// Gets the 'value' of the 'notbefore' claim { nbf, 'value' } converted to a <see cref="DateTime"/> assuming 'value' is seconds since UnixEpoch (UTC 1970-01-01T0:0:0Z).
        /// </summary>
        /// <remarks>If the 'notbefore' claim is not found, then <see cref="DateTime.MinValue"/> is returned. Time is returned as UTC.</remarks>
        public DateTime ValidFrom
        {
            get
            {
                return this.GetDateTime(JwtRegisteredClaimNames.Nbf);
            }
        }

        /// <summary>
        /// Gets the 'value' of the 'expiration' claim { exp, 'value' } converted to a <see cref="DateTime"/> assuming 'value' is seconds since UnixEpoch (UTC 1970-01-01T0:0:0Z).
        /// </summary>
        /// <remarks>If the 'expiration' claim is not found, then <see cref="DateTime.MinValue"/> is returned.</remarks>
        public DateTime ValidTo
        {
            get
            {
                return this.GetDateTime(JwtRegisteredClaimNames.Exp);
            }
        }

        /// <summary>
        /// Gets the 'value' of the 'issued at' claim { iat, 'value' } converted to a <see cref="DateTime"/> assuming 'value' is seconds since UnixEpoch (UTC 1970-01-01T0:0:0Z).
        /// </summary>
        /// <remarks>If the 'issued at' claim is not found, then <see cref="DateTime.MinValue"/> is returned.</remarks>
        public DateTime IssuedAt
        {
            get
            {
                return this.GetDateTime(JwtRegisteredClaimNames.Iat);
            }
        }
		
        /// <summary>
        /// Gets a <see cref="IEnumerable{Claim}"/><see cref="Claim"/> for each JSON { name, value }.
        /// </summary>
        /// <remarks>Each <see cref="Claim"/>(s) returned will have the <see cref="Claim.Type"/> translated according to the mapping found in <see cref="JwtSecurityTokenHandler.InboundClaimTypeMap"/>. Adding and removing to <see cref="JwtSecurityTokenHandler.InboundClaimTypeMap"/> will affect the value of the <see cref="Claim.Type"/>.
        /// <para><see cref="Claim.Issuer"/> and <see cref="Claim.OriginalIssuer"/> will be set to the value of <see cref="Iss"/> ( <see cref="string.Empty"/> if null).</para></remarks>
        public virtual IEnumerable<Claim> Claims
        {
            get
            {
                List<Claim> claims = new List<Claim>();
                string issuer = this.Iss ?? ClaimsIdentity.DefaultIssuer;

                // there is some code redundancy here that was not factored as this is a high use method. Each identity received from the host will pass through here.
                foreach (KeyValuePair<string, object> keyValuePair in this)
                {
                    if (keyValuePair.Value == null)
                    {
                        claims.Add(new Claim(keyValuePair.Key, string.Empty, JsonClaimValueTypes.JsonNull, issuer, issuer));
                        continue;
                    }

                    var claimValue = keyValuePair.Value as string;
                    if (claimValue != null)
                    {
                        claims.Add(new Claim(keyValuePair.Key, claimValue, ClaimValueTypes.String, issuer, issuer));
                        continue;
                    }

                    var jtoken = keyValuePair.Value as JToken;
                    if (jtoken != null)
                    {
                        AddClaimsFromJToken(claims, keyValuePair.Key, jtoken, issuer);
                        continue;
                    }

                    // in this case, the payload was most likely never serialized.
                    var objects = keyValuePair.Value as IEnumerable<object>;
                    if (objects != null)
                    {
                        foreach (var obj in objects)
                        {
                            claimValue = obj as string;
                            if (claimValue != null)
                            {
                                claims.Add(new Claim(keyValuePair.Key, claimValue, ClaimValueTypes.String, issuer, issuer));
                                continue;
                            }

                            jtoken = obj as JToken;
                            if (jtoken != null)
                            {
                                AddDefaultClaimFromJToken(claims, keyValuePair.Key, jtoken, issuer);
                                continue;
                            }

                            // DateTime claims require special processing. JsonConvert.SerializeObject(obj) will result in "\"dateTimeValue\"". The quotes will be added.
                            if (obj is DateTime dateTimeValue)
                                claims.Add(new Claim(keyValuePair.Key, dateTimeValue.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture), ClaimValueTypes.DateTime, issuer, issuer));
                            else
                                claims.Add(new Claim(keyValuePair.Key, JsonConvert.SerializeObject(obj), GetClaimValueType(obj), issuer, issuer));
                        }

                        continue;
                    }

                    IDictionary<string, object> dictionary = keyValuePair.Value as IDictionary<string, object>;
                    if (dictionary != null)
                    {
                        foreach (var item in dictionary)
                            claims.Add(new Claim(keyValuePair.Key, "{" + item.Key + ":" + JsonConvert.SerializeObject(item.Value) + "}", GetClaimValueType(item.Value), issuer, issuer));

                        continue;
                    }

                    // DateTime claims require special processing. JsonConvert.SerializeObject(keyValuePair.Value) will result in "\"dateTimeValue\"". The quotes will be added.
                    if (keyValuePair.Value is DateTime dateTime)
                        claims.Add(new Claim(keyValuePair.Key, dateTime.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture), ClaimValueTypes.DateTime, issuer, issuer));
                    else
                        claims.Add(new Claim(keyValuePair.Key, JsonConvert.SerializeObject(keyValuePair.Value), GetClaimValueType(keyValuePair.Value), issuer, issuer));
                }

                return claims;
            }
        }

        private static void AddClaimsFromJToken(List<Claim> claims, string claimType, JToken jtoken, string issuer)
        {
            if (jtoken.Type == JTokenType.Object)
            {
                claims.Add(new Claim(claimType, jtoken.ToString(Formatting.None), JsonClaimValueTypes.Json, issuer, issuer));
            }
            else if (jtoken.Type == JTokenType.Array)
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
            JValue jvalue = jtoken as JValue;
            if (jvalue != null)
            {
                // String is special because item.ToString(Formatting.None) will result in "/"string/"". The quotes will be added.
                // Boolean needs item.ToString otherwise 'true' => 'True'
                if (jvalue.Type == JTokenType.String)
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
        /// Adds a JSON object representing the <see cref="Claim"/> to the <see cref="JwtPayload"/>
        /// </summary>
        /// <param name="claim">{ 'Claim.Type', 'Claim.Value' } is added. If a JSON object is found with the name == <see cref="Claim.Type"/> then a { 'Claim.Type', List&lt;object&gt; } will be created to contain the duplicate values.</param>
        /// <remarks>See <see cref="AddClaims"/> For details on how <see cref="JwtSecurityTokenHandler.OutboundClaimTypeMap"/> is applied.</remarks>
        /// <exception cref="ArgumentNullException">'claim' is null.</exception>
        public void AddClaim(Claim claim)
        {
            if (claim == null)
                throw LogHelper.LogExceptionMessage(new ArgumentNullException(nameof(claim)));

            AddClaims(new Claim[] { claim });
        }

        /// <summary>
        /// Adds a number of <see cref="Claim"/> to the <see cref="JwtPayload"/> as JSON { name, value } pairs.
        /// </summary>
        /// <param name="claims">For each <see cref="Claim"/> a JSON pair { 'Claim.Type', 'Claim.Value' } is added. If duplicate claims are found then a { 'Claim.Type', List&lt;object&gt; } will be created to contain the duplicate values.</param>
        /// <remarks>
        /// <para>Any <see cref="Claim"/> in the <see cref="IEnumerable{Claim}"/> that is null, will be ignored.</para></remarks>
        /// <exception cref="ArgumentNullException"><paramref name="claims"/> is null.</exception>
        public void AddClaims(IEnumerable<Claim> claims)
        {
            if (claims == null)
                throw LogHelper.LogExceptionMessage(new ArgumentNullException(nameof(claims)));

            foreach (Claim claim in claims)
            {
                if (claim == null)
                {
                    continue;
                }

                string jsonClaimType = claim.Type;
                object jsonClaimValue = claim.ValueType.Equals(ClaimValueTypes.String) ? claim.Value : TokenUtilities.GetClaimValueUsingValueType(claim);
                object existingValue;

                // If there is an existing value, append to it.
                // What to do if the 'ClaimValueType' is not the same.
                if (TryGetValue(jsonClaimType, out existingValue))
                {
                    IList<object> claimValues = existingValue as IList<object>;
                    if (claimValues == null)
                    {
                        claimValues = new List<object>();
                        claimValues.Add(existingValue);
                        this[jsonClaimType] = claimValues;
                    }

                    claimValues.Add(jsonClaimValue);
                }
                else
                {
                    this[jsonClaimType] = jsonClaimValue;
                }
            }
        }

        /// <summary>
        /// Adds claims from dictionary.
        /// </summary>
        /// <param name="claimsCollection"> A dictionary of claims.</param>
        /// <remark> If a key is already present in target dictionary, its value is overridden by the value of the key in claimsCollection.</remark>
        internal void AddDictionaryClaims(IDictionary<string, object> claimsCollection)
        {
            if (claimsCollection == null)
                throw LogHelper.LogExceptionMessage(new ArgumentNullException(nameof(claimsCollection)));

            foreach (string type in claimsCollection.Keys)
                this[type] = claimsCollection[type];
        }

        internal static string GetClaimValueType(object obj)
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

        internal string GetStandardClaim(string claimType)
        {
            if (TryGetValue(claimType, out object value))
            {
                if (value == null)
                    return null;

                if (value is string str)
                    return str;

                return JsonExtensions.SerializeToJson(value);
            }

            return null;
        }

        internal int? GetIntClaim(string claimType)
        {
            int? retval = null;

            object value;
            if (TryGetValue(claimType, out value))
            {
                IList<object> claimValues = value as IList<object>;
                if (claimValues != null)
                {
                    foreach (object obj in claimValues)
                    {
                        retval = null;
                        if (obj == null)
                        {
                            continue;
                        }

                        try
                        {
                            retval = Convert.ToInt32(Math.Truncate(Convert.ToDouble(obj, CultureInfo.InvariantCulture)));
                        }
                        catch (System.FormatException)
                        {
                            retval = null;
                        }
                        catch (System.InvalidCastException)
                        {
                            retval = null;
                        }
                        catch (OverflowException)
                        {
                            retval = null;
                        }

                        if (retval != null)
                        {
                            return retval;
                        }
                    }
                }
                else
                {
                    try
                    {
                        retval = Convert.ToInt32(Math.Truncate(Convert.ToDouble(value, CultureInfo.InvariantCulture)));
                    }
                    catch (System.FormatException)
                    {
                        retval = null;
                    }
                    catch (OverflowException)
                    {
                        retval = null;
                    }
                }

                return retval;
            }

            return retval;
        }

        internal IList<string> GetIListClaims(string claimType)
        {
            List<string> claimValues = new List<string>();

            object value = null;
            if (!TryGetValue(claimType, out value))
            {
                return claimValues;
            }

            string str = value as string;
            if (str != null)
            {
                claimValues.Add(str);
                return claimValues;
            }

            // values must be an enumeration of strings;
            IEnumerable<object> values = value as IEnumerable<object>;
            if (values != null)
            {
                foreach (var item in values)
                {
                    claimValues.Add(item.ToString());
                }
            }
            else
            {
                claimValues.Add(JsonExtensions.SerializeToJson(value));
            }

            return claimValues;
        }

        /// <summary>
        /// Gets the DateTime using the number of seconds from 1970-01-01T0:0:0Z (UTC)
        /// </summary>
        /// <param name="key">Claim in the payload that should map to an integer.</param>
        /// <remarks>If the claim is not found, the function returns: DateTime.MinValue
        /// </remarks>
        /// <exception cref="SecurityTokenException">If an overflow exception is thrown by the runtime.</exception>
        /// <returns>The DateTime representation of a claim.</returns>
        private DateTime GetDateTime(string key)
        {
            object dateValue;
            if (!TryGetValue(key, out dateValue))
            {
                return DateTime.MinValue;
            }

            // if there are multiple dates, take the first one.
            try
            {
                long secondsAfterBaseTime;
                IList<object> dateValues = dateValue as IList<object>;
                if (dateValues != null)
                {
                    if (dateValues.Count == 0)
                    {
                        return DateTime.MinValue;
                    }
                    else
                    {
                        dateValue = dateValues[0];
                    }
                }

                // null converts to 0.
                secondsAfterBaseTime = Convert.ToInt64(Math.Truncate(Convert.ToDouble(dateValue, CultureInfo.InvariantCulture)));
                return EpochTime.DateTime(secondsAfterBaseTime);
            }
            catch (Exception ex)
            {
                if (ex is FormatException || ex is ArgumentException || ex is InvalidCastException)
                {
                    throw LogHelper.LogExceptionMessage(new SecurityTokenException(LogHelper.FormatInvariant(LogMessages.IDX12700, key, LogHelper.MarkAsNonPII((dateValue ?? "Null"))), ex));
                }

                if (ex is OverflowException)
                {
                    throw LogHelper.LogExceptionMessage(new SecurityTokenException(LogHelper.FormatInvariant(LogMessages.IDX12701, key, LogHelper.MarkAsNonPII((dateValue ?? "Null"))), ex));
                }

                throw;
            }
        }

        /// <summary>
        /// Serializes this instance to JSON.
        /// </summary>
        /// <returns>This instance as JSON.</returns>
        /// <remarks>Use <see cref="JsonExtensions.Serializer"/> to customize JSON serialization.</remarks>
        public virtual string SerializeToJson()
        {
            return JsonExtensions.SerializeToJson(this as IDictionary<string, object>);
        }

        /// <summary>
        /// Encodes this instance as Base64UrlEncoded JSON.
        /// </summary>
        /// <returns>Base64UrlEncoded JSON.</returns>
        /// <remarks>Use <see cref="JsonExtensions.Serializer"/> to customize JSON serialization.</remarks>
        public virtual string Base64UrlEncode()
        {
            return Base64UrlEncoder.Encode(SerializeToJson());
        }

        /// <summary>
        /// Deserializes Base64UrlEncoded JSON into a <see cref="JwtPayload"/> instance.
        /// </summary>
        /// <param name="base64UrlEncodedJsonString">base64url encoded JSON to deserialize.</param>
        /// <returns>An instance of <see cref="JwtPayload"/>.</returns>
        /// <remarks>Use <see cref="JsonExtensions.Deserializer"/> to customize JSON serialization.</remarks>
        public static JwtPayload Base64UrlDeserialize(string base64UrlEncodedJsonString)
        {
            return JsonExtensions.DeserializeJwtPayload(Base64UrlEncoder.Decode(base64UrlEncodedJsonString));
        }

        /// <summary>
        /// Deserialzes JSON into a <see cref="JwtPayload"/> instance.
        /// </summary>
        /// <param name="jsonString">The JSON to deserialize.</param>
        /// <returns>An instance of <see cref="JwtPayload"/>.</returns>
        /// <remarks>Use <see cref="JsonExtensions.Deserializer"/> to customize JSON serialization.</remarks>
        public static JwtPayload Deserialize(string jsonString)
        {
            return JsonExtensions.DeserializeJwtPayload(jsonString);
        }
    }
}
