// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Json;

namespace System.IdentityModel.Tokens.Jwt
{
    /// <summary>
    /// Initializes a new instance of <see cref="JwtPayload"/> which contains JSON objects representing the claims contained in the JWT. Each claim is a JSON object of the form { Name, Value }.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable"), System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Serialize not really supported.")]
    public class JwtPayload : Dictionary<string, object>
    {
        internal const string ClassName = "System.IdentityModel.Tokens.Jwt.JwtPayload";

        internal List<string> _audiences;
        internal string _azp;
        internal long? _exp;
        internal DateTime? _expDateTime;
        internal long? _iat;
        internal DateTime? _iatDateTime;
        internal string _id;
        internal string _iss;
        internal string _jti;
        internal long? _nbf;
        internal DateTime? _nbfDateTime;
        internal string _sub;
        internal string _tid;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtPayload"/> class with no claims. Default string comparer <see cref="StringComparer.Ordinal"/>. 
        /// Creates a empty <see cref="JwtPayload"/>
        /// </summary>
        public JwtPayload()
            : this(issuer: null, audience: null, claims: null, notBefore: null, expires: null)
        {
        }

        internal static JwtPayload CreatePayload(byte[] bytes, int length)
        {
            JwtPayload payload = new();
            Utf8JsonReader reader = new(bytes.AsSpan().Slice(0, length));

            if (!JsonSerializerPrimitives.IsReaderAtTokenType(ref reader, JsonTokenType.StartObject, false))
                throw LogHelper.LogExceptionMessage(
                    new JsonException(
                        LogHelper.FormatInvariant(
                        Microsoft.IdentityModel.Tokens.LogMessages.IDX11023,
                        LogHelper.MarkAsNonPII("JsonTokenType.StartObject"),
                        LogHelper.MarkAsNonPII(reader.TokenType),
                        LogHelper.MarkAsNonPII(ClassName),
                        LogHelper.MarkAsNonPII(reader.TokenStartIndex),
                        LogHelper.MarkAsNonPII(reader.CurrentDepth),
                        LogHelper.MarkAsNonPII(reader.BytesConsumed))));

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    if (reader.ValueTextEquals(JwtPayloadUtf8Bytes.Aud))
                    {
                        reader.Read();
                        payload._audiences = new List<string>();
                        if (reader.TokenType == JsonTokenType.StartArray)
                        {
                            JsonSerializerPrimitives.ReadStrings(ref reader, payload._audiences, JwtRegisteredClaimNames.Aud, ClassName, false);
                            payload[JwtRegisteredClaimNames.Aud] = payload._audiences;
                        }
                        else
                        {
                            payload._audiences.Add(JsonSerializerPrimitives.ReadString(ref reader, JwtRegisteredClaimNames.Aud, ClassName, false));
                            payload[JwtRegisteredClaimNames.Aud] = payload._audiences[0];
                        }
                    }
                    else if (reader.ValueTextEquals(JwtPayloadUtf8Bytes.Exp))
                    {
                        payload._exp = JsonSerializerPrimitives.ReadLong(ref reader, JwtRegisteredClaimNames.Exp, ClassName, true);
                        payload[JwtRegisteredClaimNames.Exp] = payload._exp;
                        payload._expDateTime = EpochTime.DateTime(payload._exp.Value);
                    }
                    else if (reader.ValueTextEquals(JwtPayloadUtf8Bytes.Iat))
                    {
                        payload._iat = JsonSerializerPrimitives.ReadLong(ref reader, JwtRegisteredClaimNames.Iat, ClassName, true);
                        payload[JwtRegisteredClaimNames.Iat] = payload._iat;
                        payload._iatDateTime = EpochTime.DateTime(payload._iat.Value);
                    }
                    else if (reader.ValueTextEquals(JwtPayloadUtf8Bytes.Iss))
                    {
                        payload._iss = JsonSerializerPrimitives.ReadString(ref reader, JwtRegisteredClaimNames.Iss, ClassName, true);
                        payload[JwtRegisteredClaimNames.Iss] = payload._iss;
                    }
                    else if (reader.ValueTextEquals(JwtPayloadUtf8Bytes.Jti))
                    {
                        payload._jti = JsonSerializerPrimitives.ReadString(ref reader, JwtRegisteredClaimNames.Jti, ClassName, true);
                        payload[JwtRegisteredClaimNames.Jti] = payload._jti;
                    }
                    else if (reader.ValueTextEquals(JwtPayloadUtf8Bytes.Nbf))
                    {
                        payload._nbf = JsonSerializerPrimitives.ReadLong(ref reader, JwtRegisteredClaimNames.Nbf, ClassName, true);
                        payload._nbfDateTime = EpochTime.DateTime(payload._nbf.Value);
                        payload[JwtRegisteredClaimNames.Nbf] = payload._nbf;
                    }
                    else if (reader.ValueTextEquals(JwtPayloadUtf8Bytes.Sub))
                    {
                        payload._sub = JsonSerializerPrimitives.ReadStringOrNumberAsString(ref reader, JwtRegisteredClaimNames.Sub, ClassName, true);
                        payload[JwtRegisteredClaimNames.Sub] = payload._sub;
                    }
                    else
                    {
                        string propertyName = JsonSerializerPrimitives.ReadPropertyName(ref reader, ClassName, true);
                        payload[propertyName] = JsonSerializerPrimitives.ReadPropertyValueAsObject(ref reader, propertyName, ClassName);
                    }
                }
            }

            return payload;
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
                return this.GetListOfClaims(JwtRegisteredClaimNames.Amr);
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
                if (_audiences == null)
                {
                    List<string> tmp = GetListOfClaims(JwtRegisteredClaimNames.Aud);
                    Interlocked.CompareExchange(ref _audiences, tmp, null);
                }

                return _audiences;
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
        [Obsolete("`int? JwtPayload.Exp` is deprecated and will be removed in a future release. Use `long? JwtPayload.Expiration` instead. For more information, see https://aka.ms/IdentityModel/7-breaking-changes")]
        public int? Exp
        {
            get => GetIntClaim(JwtRegisteredClaimNames.Exp);
        }

        /// <summary>
        /// Gets the 'value' of the 'expiration' claim { exp, 'value' }.
        /// </summary>
        /// <remarks>If the 'expiration' claim is not found OR could not be converted to <see cref="long"/>, null is returned.</remarks>
        public long? Expiration
        {
            get => GetLongClaim(JwtRegisteredClaimNames.Exp);
        }

        /// <summary>
        /// Gets the 'value' of the 'JWT ID' claim { jti, 'value' }.
        /// </summary>
        /// <remarks>If the 'JWT ID' claim is not found, null is returned.</remarks>
        public string Jti
        {
            get
            {
                _jti ??= GetStandardClaim(JwtRegisteredClaimNames.Jti);
                return _jti;
            }
        }

        /// <summary>
        /// Gets the 'value' of the 'Issued At' claim { iat, 'value' }.
        /// </summary>
        /// <remarks>If the 'Issued At' claim is not found OR cannot be converted to <see cref="Int32"/> null is returned.</remarks>
        [Obsolete("`int? JwtPayload.Iat` is deprecated and will be removed in a future release. Use `DateTime JwtPayload.IssuedAt` instead. For more information, see https://aka.ms/IdentityModel/7-breaking-changes")]
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
                _iss ??= GetStandardClaim(JwtRegisteredClaimNames.Iss);
                return _iss;
            }
        }

        /// <summary>
        /// Gets the 'value' of the 'expiration' claim { nbf, 'value' }.
        /// </summary>
        /// <remarks>If the 'notbefore' claim is not found OR could not be converted to <see cref="Int32"/>, null is returned.</remarks>
        [Obsolete("`int? JwtPayload.Nbf` is deprecated and will be removed in a future release. Use `long? JwtPayload.NotBefore` instead. For more information, see https://aka.ms/IdentityModel/7-breaking-changes")]
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
        /// Gets the 'value' of the 'notebefore' claim { nbf, 'value' }.
        /// </summary>
        /// <remarks>If the 'notbefore' claim is not found OR could not be converted to <see cref="long"/>, null is returned.</remarks>
        public long? NotBefore
        {
            get => GetLongClaim(JwtRegisteredClaimNames.Nbf); 
        }

        /// <summary>
        /// Gets the 'value' of the 'subject' claim { sub, 'value' }.
        /// </summary>
        /// <remarks>If the 'subject' claim is not found, null is returned.</remarks>
        public string Sub
        {
            get
            {
                _sub ??= GetStandardClaim(JwtRegisteredClaimNames.Sub);
                return _sub;
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
                if (_nbfDateTime.HasValue)
                    return _nbfDateTime.Value;

                long? l = GetLongClaim(JwtRegisteredClaimNames.Nbf);
                if (l.HasValue)
                    return EpochTime.DateTime(l.Value);

                _nbfDateTime = DateTime.MinValue;

                return _nbfDateTime.Value;
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
                if (_expDateTime.HasValue)
                    return _expDateTime.Value;

                long? l = GetLongClaim(JwtRegisteredClaimNames.Exp);
                if (l.HasValue)
                    return EpochTime.DateTime(l.Value);

                _expDateTime = DateTime.MinValue;

                return _expDateTime.Value;
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
                if (_iatDateTime.HasValue)
                    return _iatDateTime.Value;

                long? l = GetLongClaim(JwtRegisteredClaimNames.Iat);
                if (l.HasValue)
                    return EpochTime.DateTime(l.Value);

                _iatDateTime = DateTime.MinValue;

                return _iatDateTime.Value;
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
                string issuer = Iss ?? ClaimsIdentity.DefaultIssuer;

                foreach (KeyValuePair<string, object> keyValuePair in this)
                {
                    if (keyValuePair.Value == null)
                        claims.Add(new Claim(keyValuePair.Key, string.Empty, JsonClaimValueTypes.JsonNull, issuer, issuer));

                    else if (keyValuePair.Value is string str)
                        claims.Add(new Claim(keyValuePair.Key, str, GetClaimValueType(str), issuer, issuer));

                    else if (keyValuePair.Value is JsonElement j)
                        AddClaimsFromJsonElement(keyValuePair.Key, issuer, j, claims);

                    // in this case, the payload was most likely never serialized.
                    else if (keyValuePair.Value is IEnumerable<object> objects)
                        AddListofObjects(keyValuePair.Key, objects, claims, issuer);

                    else if (keyValuePair.Value is IDictionary<string, object> dictionary)
                    {
                        foreach (var item in dictionary)
                            if (item.Value != null)
                                claims.Add(new Claim(keyValuePair.Key, "{" + item.Key + ":" + item.Value.ToString() + "}", GetClaimValueType(item.Value), issuer, issuer));
                    }
                    else if (keyValuePair.Value is DateTime dateTime)
                        claims.Add(new Claim(keyValuePair.Key, dateTime.ToString("o", CultureInfo.InvariantCulture), ClaimValueTypes.DateTime, issuer, issuer));
                    else if (keyValuePair.Value is bool boolValue)
                    {
                        // Can't just use ToString or bools will get encoded as True/False instead of true/false.
                        if (boolValue)
                            claims.Add(new Claim(keyValuePair.Key, "true", ClaimValueTypes.Boolean, issuer, issuer));
                        else
                            claims.Add(new Claim(keyValuePair.Key, "false", ClaimValueTypes.Boolean, issuer, issuer));
                    }
                    else if (keyValuePair.Value != null)
                        claims.Add(new Claim(keyValuePair.Key, keyValuePair.Value.ToString(), GetClaimValueType(keyValuePair.Value), issuer, issuer));
                }

                return claims;
            }
        }

        private void AddListofObjects(string key, IEnumerable<object> objects, List<Claim> claims, string issuer)
        {
            foreach (var obj in objects)
            {
                if (obj is string claimValue)
                    claims.Add(new Claim(key, claimValue, ClaimValueTypes.String, issuer, issuer));
                else if (obj is bool boolValue)
                {
                    if (boolValue)
                        claims.Add(new Claim(key, "true", ClaimValueTypes.Boolean, issuer, issuer));
                    else
                        claims.Add(new Claim(key, "false", ClaimValueTypes.Boolean, issuer, issuer));
                }
                else if (obj is DateTime dateTimeValue)
                    claims.Add(new Claim(key, dateTimeValue.ToString("o", CultureInfo.InvariantCulture), ClaimValueTypes.DateTime, issuer, issuer));
                else if (obj is JsonElement jsonElement)
                    claims.Add(JsonClaimSet.CreateClaimFromJsonElement(key, issuer, jsonElement));
                else if (obj is IEnumerable<object> innerObjects)
                    AddListofObjects(key, innerObjects, claims, issuer);
                else
                    claims.Add(new Claim(key, obj.ToString(), GetClaimValueType(obj), issuer, issuer));
            }
        }

        internal static void AddClaimsFromJsonElement(string claimType, string issuer, JsonElement jsonElement, List<Claim> claims)
        {
            // handle arrays to a single level
            if (jsonElement.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement element in jsonElement.EnumerateArray())
                    claims.Add(JsonClaimSet.CreateClaimFromJsonElement(claimType, issuer, element));
            }
            else
            {
                claims.Add(JsonClaimSet.CreateClaimFromJsonElement(claimType, issuer, jsonElement));
            }
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
        /// <remark> If a key is already present in target dictionary, its claimValue is overridden by the claimValue of the key in claimsCollection.</remark>
        internal void AddDictionaryClaims(IDictionary<string, object> claimsCollection)
        {
            if (claimsCollection == null)
                throw LogHelper.LogExceptionMessage(new ArgumentNullException(nameof(claimsCollection)));

            foreach (KeyValuePair<string, object> kvp in claimsCollection)
                this[kvp.Key] = kvp.Value;
        }

        internal static string GetClaimValueType(object value)
        {
            if (value == null)
                return JsonClaimValueTypes.JsonNull;

            Type objType = value.GetType();

            if (value is string str)
                return JwtTokenUtilities.GetStringClaimValueType(str);
            else if (objType == typeof(int))
                return ClaimValueTypes.Integer32;
            else if (objType == typeof(long))
                return ClaimValueTypes.Integer64;
            else if (objType == typeof(bool))
                return ClaimValueTypes.Boolean;
            else if (objType == typeof(double))
                return ClaimValueTypes.Double;
            else if (objType == typeof(DateTime))
                return ClaimValueTypes.DateTime;
            else if (objType == typeof(float))
                return ClaimValueTypes.Double;
            else if (objType == typeof(decimal))
                return ClaimValueTypes.Double;
            else if (value is null)
                return JsonClaimValueTypes.JsonNull;
            else if (objType == typeof(JsonElement))
                return JsonClaimValueTypes.Json;

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

                return string.Empty;
            }

            return null;
        }

        internal int? GetIntClaim(string claimType)
        {
            if (TryGetValue(claimType, out object claimValue))
            {
                int i = default;
                if (TryConvertToInt(claimValue, ref i))
                    return i;
            }

            return null;
        }

        internal long? GetLongClaim(string claimType)
        {
            if (TryGetValue(claimType, out object claimValue))
            {
                long l = default;
                if (TryConvertToLong(claimValue, ref l))
                    return l;
            }

            return null;
        }

        private static bool TryConvertToInt(object value, ref int outVal)
        {
            outVal = default;
            try
            {
                if (value is int i)
                {
                    outVal = i;
                    return true;
                }

                if (value is string str)
                    if (int.TryParse(str, out int result))
                    {
                        outVal = result;
                        return true;
                    }

                outVal = Convert.ToInt32(Math.Truncate(Convert.ToDouble(value, CultureInfo.InvariantCulture)));
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (OverflowException)
            {
                return false;
            }
            catch (InvalidCastException)
            {
                return false;
            }

#pragma warning disable CS0162 // Unreachable code detected
            return false;
#pragma warning restore CS0162 // Unreachable code detected
        }

        private static bool TryConvertToLong(object value, ref long outVal)
        {
            outVal = default;
            try
            {
                if (value is long l)
                {
                    outVal = l;
                    return true;
                }

                if (value is string str)
                    if (long.TryParse(str, out long result))
                    {
                        outVal = result;
                        return true;
                    }

                outVal = Convert.ToInt64(Math.Truncate(Convert.ToDouble(value, CultureInfo.InvariantCulture)));

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (OverflowException)
            {
                return false;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        internal List<string> GetListOfClaims(string claimType)
        {
            List<string> claimValues = new();
            if (!TryGetValue(claimType, out object value))
                return claimValues;

            // JsonArray and JsonObject are stored in the dictionary as JsonElement
            if (value is JsonElement jsonElement)
            {
                if (JsonSerializerPrimitives.TryCreateTypeFromJsonElement(jsonElement, out List<string> list))
                    return list;
            }
            else if (value is string str)
                claimValues.Add(str);

            // value may not be a string, use ToString();
            else if (value is IEnumerable<object> values)
            {
                foreach (object item in values)
                    claimValues.Add(item.ToString());
            }
            else
                claimValues.Add(value.ToString());

            return claimValues;
        }

        /// <summary>
        /// Serializes this instance to JSON.
        /// </summary>
        /// <returns>This instance as JSON.</returns>
        public virtual string SerializeToJson()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Utf8JsonWriter writer = null;
                try
                {
                    writer = new Utf8JsonWriter(memoryStream, new JsonWriterOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                    writer.WriteStartObject();

                    JsonSerializerPrimitives.WriteObjects(ref writer, this);

                    writer.WriteEndObject();
                    writer.Flush();
                    return Encoding.UTF8.GetString(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                }
                finally
                {
                    writer?.Dispose();
                }
            }
        }

        /// <summary>
        /// Deserializes Base64UrlEncoded JSON into a <see cref="JwtPayload"/>.
        /// </summary>
        /// <param name="base64UrlEncodedJsonString">Base64url encoded JSON to deserialize.</param>
        /// <returns>An instance of <see cref="JwtPayload"/>.</returns>
        public static JwtPayload Base64UrlDeserialize(string base64UrlEncodedJsonString)
        {
            _ = base64UrlEncodedJsonString ?? throw LogHelper.LogArgumentNullException(nameof(base64UrlEncodedJsonString));
            return Base64UrlEncoding.Decode(base64UrlEncodedJsonString, 0, base64UrlEncodedJsonString.Length, CreatePayload);
        }

        /// <summary>
        /// Encodes this instance as Base64UrlEncoded JSON.
        /// </summary>
        /// <returns>Base64UrlEncoded JSON.</returns>
        public virtual string Base64UrlEncode()
        {
            return Base64UrlEncoder.Encode(SerializeToJson());
        }

        /// <summary>
        /// Deserialzes JSON into a <see cref="JwtPayload"/> instance.
        /// </summary>
        /// <param name="jsonString">The JSON to deserialize.</param>
        /// <returns>An instance of <see cref="JwtPayload"/>.</returns>
        public static JwtPayload Deserialize(string jsonString)
        {
            _ = jsonString ?? throw LogHelper.LogArgumentNullException(nameof(jsonString));
            byte[] bytes = Encoding.UTF8.GetBytes(jsonString);
            return CreatePayload(bytes, bytes.Length);
        }
    }
}
