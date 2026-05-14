// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TokenLogMessages = Microsoft.IdentityModel.Tokens.LogMessages;

namespace Microsoft.IdentityModel.JsonWebTokens.Tests
{
    /// <summary>
    /// A <see cref="SecurityTokenHandler"/> designed for creating and validating Json Web Tokens. 
    /// See: https://datatracker.ietf.org/doc/html/rfc7519 and http://www.rfc-editor.org/info/rfc7515.
    /// </summary>
    public class JsonWebTokenHandler6x : TokenHandler
    {
        private IDictionary<string, string> _inboundClaimTypeMap;
        private const string _namespace = "http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties";
        private static string _shortClaimType = _namespace + "/ShortTypeName";
        private bool _mapInboundClaims = DefaultMapInboundClaims;

        /// <summary>
        /// Default claim type mapping for inbound claims.
        /// </summary>
        public static IDictionary<string, string> DefaultInboundClaimTypeMap = new Dictionary<string, string>(ClaimTypeMapping.InboundClaimTypeMap);

        /// <summary>
        /// Default value for the flag that determines whether or not the InboundClaimTypeMap is used.
        /// </summary>
        public static bool DefaultMapInboundClaims = false;

        /// <summary>
        /// Gets the Base64Url encoded string representation of the following JWT header: 
        /// { <see cref="JwtHeaderParameterNames.Alg"/>, <see cref="SecurityAlgorithms.None"/> }.
        /// </summary>
        /// <return>The Base64Url encoded string representation of the unsigned JWT header.</return>
        public const string Base64UrlEncodedUnsignedJWSHeader = "eyJhbGciOiJub25lIn0";

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonWebTokenHandler"/> class.
        /// </summary>
        public JsonWebTokenHandler6x()
        {
            if (_mapInboundClaims)
                _inboundClaimTypeMap = new Dictionary<string, string>(DefaultInboundClaimTypeMap);
            else
                _inboundClaimTypeMap = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets the type of the <see cref="JsonWebToken"/>.
        /// </summary>
        /// <return>The type of <see cref="JsonWebToken"/></return>
        public Type TokenType
        {
            get { return typeof(JsonWebToken); }
        }

        /// <summary>
        /// Gets or sets the property name of <see cref="Claim.Properties"/> the will contain the original JSON claim 'name' if a mapping occurred when the <see cref="Claim"/>(s) were created.
        /// </summary>
        /// <exception cref="ArgumentException">If <see cref="string"/>.IsNullOrWhiteSpace('value') is true.</exception>
        public static string ShortClaimTypeProperty
        {
            get
            {
                return _shortClaimType;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw LogHelper.LogArgumentNullException(nameof(value));

                _shortClaimType = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="MapInboundClaims"/> property which is used when determining whether or not to map claim types that are extracted when validating a <see cref="JsonWebToken"/>. 
        /// <para>If this is set to true, the <see cref="Claim.Type"/> is set to the JSON claim 'name' after translating using this mapping. Otherwise, no mapping occurs.</para>
        /// <para>The default value is false.</para>
        /// </summary>
        public bool MapInboundClaims
        {
            get
            {
                return _mapInboundClaims;
            }
            set
            {
                if(!_mapInboundClaims && value && _inboundClaimTypeMap.Count == 0)
                    _inboundClaimTypeMap = new Dictionary<string, string>(DefaultInboundClaimTypeMap);
                _mapInboundClaims = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="InboundClaimTypeMap"/> which is used when setting the <see cref="Claim.Type"/> for claims in the <see cref="ClaimsPrincipal"/> extracted when validating a <see cref="JsonWebToken"/>. 
        /// <para>The <see cref="Claim.Type"/> is set to the JSON claim 'name' after translating using this mapping.</para>
        /// <para>The default value is ClaimTypeMapping.InboundClaimTypeMap.</para>
        /// </summary>
        /// <exception cref="ArgumentNullException">'value' is null.</exception>
        public IDictionary<string, string> InboundClaimTypeMap
        {
            get
            {
                return _inboundClaimTypeMap;
            }

            set
            {
                _inboundClaimTypeMap = value ?? throw LogHelper.LogArgumentNullException(nameof(value));
            }
        }

        internal static IDictionary<string, object> AddCtyClaimDefaultValue(IDictionary<string, object> additionalClaims, bool setDefaultCtyClaim)
        {
            if (!setDefaultCtyClaim)
                return additionalClaims;

            if (additionalClaims == null)
                additionalClaims = new Dictionary<string, object> { { JwtHeaderParameterNames.Cty, JwtConstants.HeaderType } };
            else if (!additionalClaims.TryGetValue(JwtHeaderParameterNames.Cty, out _))
                additionalClaims.Add(JwtHeaderParameterNames.Cty, JwtConstants.HeaderType);

            return additionalClaims;
        }

        /// <summary>
        /// Determines if the string is a well formed Json Web Token (JWT).
        /// <para>See: https://datatracker.ietf.org/doc/html/rfc7519 </para>
        /// </summary>
        /// <param name="token">String that should represent a valid JWT.</param>
        /// <remarks>Uses <see cref="Regex.IsMatch(string, string)"/> matching:
        /// <para>JWS: @"^[A-Za-z0-9-_]+\.[A-Za-z0-9-_]+\.[A-Za-z0-9-_]*$"</para>
        /// <para>JWE: (dir): @"^[A-Za-z0-9-_]+\.\.[A-Za-z0-9-_]+\.[A-Za-z0-9-_]+\.[A-Za-z0-9-_]*$"</para>
        /// <para>JWE: (wrappedkey): @"^[A-Za-z0-9-_]+\.[A-Za-z0-9-_]+\.[A-Za-z0-9-_]+\.[A-Za-z0-9-_]+\.[A-Za-z0-9-_]$"</para>
        /// </remarks>
        /// <returns>
        /// <para>'false' if the token is null or whitespace.</para>
        /// <para>'false' if token.Length is greater than <see cref="TokenHandler.MaximumTokenSizeInBytes"/>.</para>
        /// <para>'true' if the token is in JSON compact serialization format.</para>
        /// </returns>
        public virtual bool CanReadToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            if (token.Length > MaximumTokenSizeInBytes)
            {
                if (LogHelper.IsEnabled(EventLogLevel.Informational))
                    LogHelper.LogInformation(TokenLogMessages.IDX10209, LogHelper.MarkAsNonPII(token.Length), LogHelper.MarkAsNonPII(MaximumTokenSizeInBytes));

                return false;
            }

            // Count the number of segments, which is the number of periods + 1. We can stop when we've encountered
            // more segments than the maximum we know how to handle.
            int pos = 0;
            int segmentCount = 1;
            while (segmentCount <= JwtConstants.MaxJwtSegmentCount && ((pos = token.IndexOf('.', pos)) >= 0))
            {
                pos++;
                segmentCount++;
            }

            switch (segmentCount)
            {
                case JwtConstants.JwsSegmentCount:
                    return JwtTokenUtilities.RegexJws.IsMatch(token);

                case JwtConstants.JweSegmentCount:
                    return JwtTokenUtilities.RegexJwe.IsMatch(token);

                default:
                    LogHelper.LogInformation(LogMessages.IDX14107);
                    return false;
            }
        }

        /// <summary>
        /// Returns a value that indicates if this handler can validate a <see cref="SecurityToken"/>.
        /// </summary>
        /// <returns>'true', indicating this instance can validate a <see cref="JsonWebToken"/>.</returns>
        public virtual bool CanValidateToken
        {
            get { return true; }
        }

        private static JObject CreateDefaultJWEHeader(EncryptingCredentials encryptingCredentials, string compressionAlgorithm, string tokenType)
        {
            var header = new JObject();
            header.Add(JwtHeaderParameterNames.Alg, encryptingCredentials.Alg);
            header.Add(JwtHeaderParameterNames.Enc, encryptingCredentials.Enc);

            if (!string.IsNullOrEmpty(encryptingCredentials.Key.KeyId))
                header.Add(JwtHeaderParameterNames.Kid, encryptingCredentials.Key.KeyId);

            if (!string.IsNullOrEmpty(compressionAlgorithm))
                header.Add(JwtHeaderParameterNames.Zip, compressionAlgorithm);

            if (string.IsNullOrEmpty(tokenType))
                header.Add(JwtHeaderParameterNames.Typ, JwtConstants.HeaderType);
            else
                header.Add(JwtHeaderParameterNames.Typ, tokenType);

            return header;
        }

        private static JObject CreateDefaultJWSHeader(SigningCredentials signingCredentials, string tokenType)
        {
            JObject header = null;

            if (signingCredentials == null)
            {
                header = new JObject()
                {
                    {JwtHeaderParameterNames.Alg, SecurityAlgorithms.None }
                };
            }
            else
            {
                header = new JObject()
                {
                    { JwtHeaderParameterNames.Alg, signingCredentials.Algorithm }
                };

                if (signingCredentials.Key.KeyId != null)
                    header.Add(JwtHeaderParameterNames.Kid, signingCredentials.Key.KeyId);

                if (signingCredentials.Key is X509SecurityKey x509SecurityKey)
                    header[JwtHeaderParameterNames.X5t] = x509SecurityKey.X5t;
            }

            if (string.IsNullOrEmpty(tokenType))
                header.Add(JwtHeaderParameterNames.Typ, JwtConstants.HeaderType);
            else
                header.Add(JwtHeaderParameterNames.Typ, tokenType);

            return header;
        }

        /// <summary>
        /// Creates an unsigned JWS (Json Web Signature).
        /// </summary>
        /// <param name="payload">A string containing JSON which represents the JWT token payload.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="payload"/> is null.</exception>
        /// <returns>A JWS in Compact Serialization Format.</returns>
        public virtual string CreateToken(string payload)
        {
            if (string.IsNullOrEmpty(payload))
                throw LogHelper.LogArgumentNullException(nameof(payload));

            return CreateTokenPrivate(payload, null, null, null, null, null, null);
        }

        /// <summary>
        /// Creates an unsigned JWS (Json Web Signature).
        /// </summary>
        /// <param name="payload">A string containing JSON which represents the JWT token payload.</param>
        /// <param name="additionalHeaderClaims">Defines the dictionary containing any custom header claims that need to be added to the JWT token header.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="payload"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="additionalHeaderClaims"/> is null.</exception>
        /// <returns>A JWS in Compact Serialization Format.</returns>
        public virtual string CreateToken(string payload, IDictionary<string, object> additionalHeaderClaims)
        {
            if (string.IsNullOrEmpty(payload))
                throw LogHelper.LogArgumentNullException(nameof(payload));

            if (additionalHeaderClaims == null)
                throw LogHelper.LogArgumentNullException(nameof(additionalHeaderClaims));

            return CreateTokenPrivate(payload, null, null, null, additionalHeaderClaims, null, null);
        }

        /// <summary>
        /// Creates a JWS (Json Web Signature).
        /// </summary>
        /// <param name="payload">A string containing JSON which represents the JWT token payload.</param>
        /// <param name="signingCredentials">Defines the security key and algorithm that will be used to sign the JWS.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="payload"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="signingCredentials"/> is null.</exception>
        /// <returns>A JWS in Compact Serialization Format.</returns>
        public virtual string CreateToken(string payload, SigningCredentials signingCredentials)
        {
            if (string.IsNullOrEmpty(payload))
                throw LogHelper.LogArgumentNullException(nameof(payload));

            if (signingCredentials == null)
                throw LogHelper.LogArgumentNullException(nameof(signingCredentials));

            return CreateTokenPrivate(payload, signingCredentials, null, null, null, null, null);
        }

        /// <summary>
        /// Creates a JWS (Json Web Signature).
        /// </summary>
        /// <param name="payload">A string containing JSON which represents the JWT token payload.</param>
        /// <param name="signingCredentials">Defines the security key and algorithm that will be used to sign the JWS.</param>
        /// <param name="additionalHeaderClaims">Defines the dictionary containing any custom header claims that need to be added to the JWT token header.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="payload"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="signingCredentials"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="additionalHeaderClaims"/> is null.</exception>
        /// <exception cref="SecurityTokenException">if <see cref="JwtHeaderParameterNames.Alg"/>, <see cref="JwtHeaderParameterNames.Kid"/>
        /// <see cref="JwtHeaderParameterNames.X5t"/>, <see cref="JwtHeaderParameterNames.Enc"/>, and/or <see cref="JwtHeaderParameterNames.Zip"/>
        /// are present inside of <paramref name="additionalHeaderClaims"/>.</exception>
        /// <returns>A JWS in Compact Serialization Format.</returns>
        public virtual string CreateToken(string payload, SigningCredentials signingCredentials, IDictionary<string, object> additionalHeaderClaims)
        {
            if (string.IsNullOrEmpty(payload))
                throw LogHelper.LogArgumentNullException(nameof(payload));

            if (signingCredentials == null)
                throw LogHelper.LogArgumentNullException(nameof(signingCredentials));

            if (additionalHeaderClaims == null)
                throw LogHelper.LogArgumentNullException(nameof(additionalHeaderClaims));

            return CreateTokenPrivate(payload, signingCredentials, null, null, additionalHeaderClaims, null, null);
        }

        /// <summary>
        /// Creates a JWS(Json Web Signature).
        /// </summary>
        /// <param name="tokenDescriptor">A <see cref="SecurityTokenDescriptor"/> that contains details of contents of the token.</param>
        /// <returns>A JWS in Compact Serialization Format.</returns>
        public virtual string CreateToken(SecurityTokenDescriptor tokenDescriptor)
        {
            if (tokenDescriptor == null)
                throw LogHelper.LogArgumentNullException(nameof(tokenDescriptor));

            if (LogHelper.IsEnabled(EventLogLevel.Warning))
            {
                if ((tokenDescriptor.Subject == null || !tokenDescriptor.Subject.Claims.Any())
                    && (tokenDescriptor.Claims == null || !tokenDescriptor.Claims.Any()))
                    LogHelper.LogWarning(LogMessages.IDX14114, LogHelper.MarkAsNonPII(nameof(SecurityTokenDescriptor)), LogHelper.MarkAsNonPII(nameof(SecurityTokenDescriptor.Subject)), LogHelper.MarkAsNonPII(nameof(SecurityTokenDescriptor.Claims)));
            }

            JObject payload;
            if (tokenDescriptor.Subject != null)
                payload = JObject.FromObject(TokenUtilities.CreateDictionaryFromClaims(tokenDescriptor.Subject.Claims));
            else
                payload = new JObject();

            // If a key is present in both tokenDescriptor.Subject.Claims and tokenDescriptor.Claims, the value present in tokenDescriptor.Claims is the
            // one that takes precedence and will remain after the merge. Key comparison is case sensitive. 
            if (tokenDescriptor.Claims != null && tokenDescriptor.Claims.Count > 0)
                payload.Merge(JObject.FromObject(tokenDescriptor.Claims), new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Replace });

            if (tokenDescriptor.Audiences.Count > 0)
            {
                if (payload.ContainsKey(JwtRegisteredClaimNames.Aud))
                    LogDuplicatedClaim(nameof(tokenDescriptor.Audiences));

                payload[JwtRegisteredClaimNames.Aud] = JArray.FromObject(tokenDescriptor.Audiences);
            }
            else if (!string.IsNullOrEmpty(tokenDescriptor.Audience))
            {
                if (payload.ContainsKey(JwtRegisteredClaimNames.Aud))
                    LogDuplicatedClaim(nameof(tokenDescriptor.Audience));

                payload[JwtRegisteredClaimNames.Aud] = tokenDescriptor.Audience;
            }

            if (tokenDescriptor.Expires.HasValue)
            {
                if (payload.ContainsKey(JwtRegisteredClaimNames.Exp))
                    LogDuplicatedClaim(nameof(tokenDescriptor.Expires));

                payload[JwtRegisteredClaimNames.Exp] = EpochTime.GetIntDate(tokenDescriptor.Expires.Value);
            }

            if (tokenDescriptor.Issuer != null)
            {
                if (payload.ContainsKey(JwtRegisteredClaimNames.Iss))
                    LogDuplicatedClaim(nameof(tokenDescriptor.Issuer));

                payload[JwtRegisteredClaimNames.Iss] = tokenDescriptor.Issuer;
            }

            if (tokenDescriptor.IssuedAt.HasValue)
            {
                if (payload.ContainsKey(JwtRegisteredClaimNames.Iat))
                    LogDuplicatedClaim(nameof(tokenDescriptor.IssuedAt));

                payload[JwtRegisteredClaimNames.Iat] = EpochTime.GetIntDate(tokenDescriptor.IssuedAt.Value);
            }

            if (tokenDescriptor.NotBefore.HasValue)
            {
                if (payload.ContainsKey(JwtRegisteredClaimNames.Nbf))
                    LogDuplicatedClaim(nameof(tokenDescriptor.NotBefore));

                payload[JwtRegisteredClaimNames.Nbf] = EpochTime.GetIntDate(tokenDescriptor.NotBefore.Value);
            }

            return CreateTokenPrivate(
                payload.ToString(Formatting.None),
                tokenDescriptor.SigningCredentials,
                tokenDescriptor.EncryptingCredentials,
                tokenDescriptor.CompressionAlgorithm,
                tokenDescriptor.AdditionalHeaderClaims,
                tokenDescriptor.AdditionalInnerHeaderClaims,
                tokenDescriptor.TokenType);
        }

        private void LogDuplicatedClaim(string claimType)
        {
            if (LogHelper.IsEnabled(EventLogLevel.Informational))
                LogHelper.LogInformation(LogHelper.FormatInvariant(LogMessages.IDX14113, LogHelper.MarkAsNonPII(claimType)));
        }

        /// <summary>
        /// Creates a JWE (Json Web Encryption).
        /// </summary>
        /// <param name="payload">A string containing JSON which represents the JWT token payload.</param>
        /// <param name="encryptingCredentials">Defines the security key and algorithm that will be used to encrypt the JWT.</param>
        /// <returns>A JWE in compact serialization format.</returns>
        public virtual string CreateToken(string payload, EncryptingCredentials encryptingCredentials)
        {
            if (string.IsNullOrEmpty(payload))
                throw LogHelper.LogArgumentNullException(nameof(payload));

            if (encryptingCredentials == null)
                throw LogHelper.LogArgumentNullException(nameof(encryptingCredentials));

            return CreateTokenPrivate(payload, null, encryptingCredentials, null, null, null, null);
        }

        /// <summary>
        /// Creates a JWE (Json Web Encryption).
        /// </summary>
        /// <param name="payload">A string containing JSON which represents the JWT token payload.</param>
        /// <param name="encryptingCredentials">Defines the security key and algorithm that will be used to encrypt the JWT.</param>
        /// <param name="additionalHeaderClaims">Defines the dictionary containing any custom header claims that need to be added to the outer JWT token header.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="payload"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="encryptingCredentials"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="additionalHeaderClaims"/> is null.</exception>
        /// <exception cref="SecurityTokenException">if <see cref="JwtHeaderParameterNames.Alg"/>, <see cref="JwtHeaderParameterNames.Kid"/>
        /// <see cref="JwtHeaderParameterNames.X5t"/>, <see cref="JwtHeaderParameterNames.Enc"/>, and/or <see cref="JwtHeaderParameterNames.Zip"/>
        /// are present inside of <paramref name="additionalHeaderClaims"/>.</exception>
        /// <returns>A JWS in Compact Serialization Format.</returns>
        public virtual string CreateToken(string payload, EncryptingCredentials encryptingCredentials, IDictionary<string, object> additionalHeaderClaims)
        {
            if (string.IsNullOrEmpty(payload))
                throw LogHelper.LogArgumentNullException(nameof(payload));

            if (encryptingCredentials == null)
                throw LogHelper.LogArgumentNullException(nameof(encryptingCredentials));

            if (additionalHeaderClaims == null)
                throw LogHelper.LogArgumentNullException(nameof(additionalHeaderClaims));

            return CreateTokenPrivate(payload, null, encryptingCredentials, null, additionalHeaderClaims, null, null);
        }

        /// <summary>
        /// Creates a JWE (Json Web Encryption).
        /// </summary>
        /// <param name="payload">A string containing JSON which represents the JWT token payload.</param>
        /// <param name="signingCredentials">Defines the security key and algorithm that will be used to sign the JWT.</param>
        /// <param name="encryptingCredentials">Defines the security key and algorithm that will be used to encrypt the JWT.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="payload"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="signingCredentials"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="encryptingCredentials"/> is null.</exception>
        /// <returns>A JWE in compact serialization format.</returns>
        public virtual string CreateToken(string payload, SigningCredentials signingCredentials, EncryptingCredentials encryptingCredentials)
        {
            if (string.IsNullOrEmpty(payload))
                throw LogHelper.LogArgumentNullException(nameof(payload));

            if (signingCredentials == null)
                throw LogHelper.LogArgumentNullException(nameof(signingCredentials));

            if (encryptingCredentials == null)
                throw LogHelper.LogArgumentNullException(nameof(encryptingCredentials));

            return CreateTokenPrivate(payload, signingCredentials, encryptingCredentials, null, null, null, null);
        }

        /// <summary>
        /// Creates a JWE (Json Web Encryption).
        /// </summary>
        /// <param name="payload">A string containing JSON which represents the JWT token payload.</param>
        /// <param name="signingCredentials">Defines the security key and algorithm that will be used to sign the JWT.</param>
        /// <param name="encryptingCredentials">Defines the security key and algorithm that will be used to encrypt the JWT.</param>
        /// <param name="additionalHeaderClaims">Defines the dictionary containing any custom header claims that need to be added to the outer JWT token header.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="payload"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="signingCredentials"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="encryptingCredentials"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="additionalHeaderClaims"/> is null.</exception>
        /// <exception cref="SecurityTokenException">if <see cref="JwtHeaderParameterNames.Alg"/>, <see cref="JwtHeaderParameterNames.Kid"/>
        /// <see cref="JwtHeaderParameterNames.X5t"/>, <see cref="JwtHeaderParameterNames.Enc"/>, and/or <see cref="JwtHeaderParameterNames.Zip"/>
        /// are present inside of <paramref name="additionalHeaderClaims"/>.</exception>
        /// <returns>A JWE in compact serialization format.</returns>
        public virtual string CreateToken(
            string payload,
            SigningCredentials signingCredentials,
            EncryptingCredentials encryptingCredentials,
            IDictionary<string, object> additionalHeaderClaims)
        {
            if (string.IsNullOrEmpty(payload))
                throw LogHelper.LogArgumentNullException(nameof(payload));

            if (signingCredentials == null)
                throw LogHelper.LogArgumentNullException(nameof(signingCredentials));

            if (encryptingCredentials == null)
                throw LogHelper.LogArgumentNullException(nameof(encryptingCredentials));

            if (additionalHeaderClaims == null)
                throw LogHelper.LogArgumentNullException(nameof(additionalHeaderClaims));

            return CreateTokenPrivate(payload, signingCredentials, encryptingCredentials, null, additionalHeaderClaims, null, null);
        }

        /// <summary>
        /// Creates a JWE (Json Web Encryption).
        /// </summary>
        /// <param name="payload">A string containing JSON which represents the JWT token payload.</param>
        /// <param name="encryptingCredentials">Defines the security key and algorithm that will be used to encrypt the JWT.</param>
        /// <param name="compressionAlgorithm">Defines the compression algorithm that will be used to compress the JWT token payload.</param>
        /// <returns>A JWE in compact serialization format.</returns>
        public virtual string CreateToken(string payload, EncryptingCredentials encryptingCredentials, string compressionAlgorithm)
        {
            if (string.IsNullOrEmpty(payload))
                throw LogHelper.LogArgumentNullException(nameof(payload));

            if (encryptingCredentials == null)
                throw LogHelper.LogArgumentNullException(nameof(encryptingCredentials));

            if (string.IsNullOrEmpty(compressionAlgorithm))
                throw LogHelper.LogArgumentNullException(nameof(compressionAlgorithm));

            return CreateTokenPrivate(payload, null, encryptingCredentials, compressionAlgorithm, null, null, null);
        }

        /// <summary>
        /// Creates a JWE (Json Web Encryption).
        /// </summary>
        /// <param name="payload">A string containing JSON which represents the JWT token payload.</param>
        /// <param name="signingCredentials">Defines the security key and algorithm that will be used to sign the JWT.</param>
        /// <param name="encryptingCredentials">Defines the security key and algorithm that will be used to encrypt the JWT.</param>
        /// <param name="compressionAlgorithm">Defines the compression algorithm that will be used to compress the JWT token payload.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="payload"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="signingCredentials"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="encryptingCredentials"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="compressionAlgorithm"/> is null.</exception>
        /// <returns>A JWE in compact serialization format.</returns>
        public virtual string CreateToken(string payload, SigningCredentials signingCredentials, EncryptingCredentials encryptingCredentials, string compressionAlgorithm)
        {
            if (string.IsNullOrEmpty(payload))
                throw LogHelper.LogArgumentNullException(nameof(payload));

            if (signingCredentials == null)
                throw LogHelper.LogArgumentNullException(nameof(signingCredentials));

            if (encryptingCredentials == null)
                throw LogHelper.LogArgumentNullException(nameof(encryptingCredentials));

            if (string.IsNullOrEmpty(compressionAlgorithm))
                throw LogHelper.LogArgumentNullException(nameof(compressionAlgorithm));

            return CreateTokenPrivate(payload, signingCredentials, encryptingCredentials, compressionAlgorithm, null, null, null);
        }

        /// <summary>
        /// Creates a JWE (Json Web Encryption).
        /// </summary>
        /// <param name="payload">A string containing JSON which represents the JWT token payload.</param>
        /// <param name="signingCredentials">Defines the security key and algorithm that will be used to sign the JWT.</param>
        /// <param name="encryptingCredentials">Defines the security key and algorithm that will be used to encrypt the JWT.</param>
        /// <param name="compressionAlgorithm">Defines the compression algorithm that will be used to compress the JWT token payload.</param>       
        /// <param name="additionalHeaderClaims">Defines the dictionary containing any custom header claims that need to be added to the outer JWT token header.</param>
        /// <param name="additionalInnerHeaderClaims">Defines the dictionary containing any custom header claims that need to be added to the inner JWT token header.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="payload"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="signingCredentials"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="encryptingCredentials"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="compressionAlgorithm"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="additionalHeaderClaims"/> is null.</exception>
        /// <exception cref="SecurityTokenException">if <see cref="JwtHeaderParameterNames.Alg"/>, <see cref="JwtHeaderParameterNames.Kid"/>
        /// <see cref="JwtHeaderParameterNames.X5t"/>, <see cref="JwtHeaderParameterNames.Enc"/>, and/or <see cref="JwtHeaderParameterNames.Zip"/>
        /// are present inside of <paramref name="additionalHeaderClaims"/>.</exception>
        /// <returns>A JWE in compact serialization format.</returns>
        public virtual string CreateToken(
            string payload,
            SigningCredentials signingCredentials,
            EncryptingCredentials encryptingCredentials,
            string compressionAlgorithm,
            IDictionary<string, object> additionalHeaderClaims,
            IDictionary<string, object> additionalInnerHeaderClaims)
        {
            if (string.IsNullOrEmpty(payload))
                throw LogHelper.LogArgumentNullException(nameof(payload));

            if (signingCredentials == null)
                throw LogHelper.LogArgumentNullException(nameof(signingCredentials));

            if (encryptingCredentials == null)
                throw LogHelper.LogArgumentNullException(nameof(encryptingCredentials));

            if (string.IsNullOrEmpty(compressionAlgorithm))
                throw LogHelper.LogArgumentNullException(nameof(compressionAlgorithm));

            if (additionalHeaderClaims == null)
                throw LogHelper.LogArgumentNullException(nameof(additionalHeaderClaims));

            if (additionalInnerHeaderClaims == null)
                throw LogHelper.LogArgumentNullException(nameof(additionalInnerHeaderClaims));

            return CreateTokenPrivate(
                payload,
                signingCredentials,
                encryptingCredentials,
                compressionAlgorithm,
                additionalHeaderClaims,
                additionalInnerHeaderClaims,
                null);
        }

        /// <summary>
        /// Creates a JWE (Json Web Encryption).
        /// </summary>
        /// <param name="payload">A string containing JSON which represents the JWT token payload.</param>
        /// <param name="signingCredentials">Defines the security key and algorithm that will be used to sign the JWT.</param>
        /// <param name="encryptingCredentials">Defines the security key and algorithm that will be used to encrypt the JWT.</param>
        /// <param name="compressionAlgorithm">Defines the compression algorithm that will be used to compress the JWT token payload.</param>       
        /// <param name="additionalHeaderClaims">Defines the dictionary containing any custom header claims that need to be added to the outer JWT token header.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="payload"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="signingCredentials"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="encryptingCredentials"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="compressionAlgorithm"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="additionalHeaderClaims"/> is null.</exception>
        /// <exception cref="SecurityTokenException">if <see cref="JwtHeaderParameterNames.Alg"/>, <see cref="JwtHeaderParameterNames.Kid"/>
        /// <see cref="JwtHeaderParameterNames.X5t"/>, <see cref="JwtHeaderParameterNames.Enc"/>, and/or <see cref="JwtHeaderParameterNames.Zip"/>
        /// are present inside of <paramref name="additionalHeaderClaims"/>.</exception>
        /// <returns>A JWE in compact serialization format.</returns>
        public virtual string CreateToken(
            string payload,
            SigningCredentials signingCredentials,
            EncryptingCredentials encryptingCredentials,
            string compressionAlgorithm,
            IDictionary<string, object> additionalHeaderClaims)
        {
            if (string.IsNullOrEmpty(payload))
                throw LogHelper.LogArgumentNullException(nameof(payload));

            if (signingCredentials == null)
                throw LogHelper.LogArgumentNullException(nameof(signingCredentials));

            if (encryptingCredentials == null)
                throw LogHelper.LogArgumentNullException(nameof(encryptingCredentials));

            if (string.IsNullOrEmpty(compressionAlgorithm))
                throw LogHelper.LogArgumentNullException(nameof(compressionAlgorithm));

            if (additionalHeaderClaims == null)
                throw LogHelper.LogArgumentNullException(nameof(additionalHeaderClaims));

            return CreateTokenPrivate(payload, signingCredentials, encryptingCredentials, compressionAlgorithm, additionalHeaderClaims, null, null);
        }

        private string CreateTokenPrivate(
            string payload,
            SigningCredentials signingCredentials,
            EncryptingCredentials encryptingCredentials,
            string compressionAlgorithm,
            IDictionary<string, object> additionalHeaderClaims,
            IDictionary<string, object> additionalInnerHeaderClaims,
            string tokenType)
        {
            if (additionalHeaderClaims?.Count > 0 && additionalHeaderClaims.Keys.Intersect(JwtTokenUtilities.DefaultHeaderParameters, StringComparer.OrdinalIgnoreCase).Any())
                throw LogHelper.LogExceptionMessage(new SecurityTokenException(LogHelper.FormatInvariant(LogMessages.IDX14116, LogHelper.MarkAsNonPII(nameof(additionalHeaderClaims)), LogHelper.MarkAsNonPII(string.Join(", ", JwtTokenUtilities.DefaultHeaderParameters)))));

            if (additionalInnerHeaderClaims?.Count > 0 && additionalInnerHeaderClaims.Keys.Intersect(JwtTokenUtilities.DefaultHeaderParameters, StringComparer.OrdinalIgnoreCase).Any())
                throw LogHelper.LogExceptionMessage(new SecurityTokenException(LogHelper.FormatInvariant(LogMessages.IDX14116, nameof(additionalInnerHeaderClaims), string.Join(", ", JwtTokenUtilities.DefaultHeaderParameters))));

            var header = CreateDefaultJWSHeader(signingCredentials, tokenType);

            if (encryptingCredentials == null && additionalHeaderClaims != null && additionalHeaderClaims.Count > 0)
                header.Merge(JObject.FromObject(additionalHeaderClaims));

            if (additionalInnerHeaderClaims != null && additionalInnerHeaderClaims.Count > 0)
                header.Merge(JObject.FromObject(additionalInnerHeaderClaims));

            var rawHeader = Base64UrlEncoder.Encode(Encoding.UTF8.GetBytes(header.ToString(Formatting.None)));
            JObject jsonPayload = null;
            try
            {
                if (SetDefaultTimesOnTokenCreation)
                {
                    jsonPayload = JObject.Parse(payload);
                    if (jsonPayload != null)
                    {
                        var now = EpochTime.GetIntDate(DateTime.UtcNow);
                        if (!jsonPayload.TryGetValue(JwtRegisteredClaimNames.Exp, out _))
                            jsonPayload.Add(JwtRegisteredClaimNames.Exp, now + TokenLifetimeInMinutes * 60);

                        if (!jsonPayload.TryGetValue(JwtRegisteredClaimNames.Iat, out _))
                            jsonPayload.Add(JwtRegisteredClaimNames.Iat, now);

                        if (!jsonPayload.TryGetValue(JwtRegisteredClaimNames.Nbf, out _))
                            jsonPayload.Add(JwtRegisteredClaimNames.Nbf, now);
                    }
                }
            }
            catch(Exception ex)
            {
                if (LogHelper.IsEnabled(EventLogLevel.Error))
                    LogHelper.LogExceptionMessage(new SecurityTokenException(LogMessages.IDX14307, ex));
            }

            payload = jsonPayload != null ? jsonPayload.ToString(Formatting.None) : payload;
            var rawPayload = Base64UrlEncoder.Encode(Encoding.UTF8.GetBytes(payload));
            var message = rawHeader + "." + rawPayload;
            var rawSignature = signingCredentials == null ? string.Empty : JwtTokenUtilities.CreateEncodedSignature(message, signingCredentials);

            if (encryptingCredentials != null)
            {
                additionalHeaderClaims = AddCtyClaimDefaultValue(additionalHeaderClaims, encryptingCredentials.SetDefaultCtyClaim);

                return EncryptTokenPrivate(message + "." + rawSignature, encryptingCredentials, compressionAlgorithm, additionalHeaderClaims, tokenType);
            }

            return message + "." + rawSignature;
        }

        /// <summary>
        /// Compress a JWT token string.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="compressionAlgorithm"></param>
        /// <exception cref="ArgumentNullException">if <paramref name="token"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="compressionAlgorithm"/> is null.</exception>
        /// <exception cref="NotSupportedException">if the compression algorithm is not supported.</exception>
        /// <returns>Compressed JWT token bytes.</returns>
        private static byte[] CompressToken(string token, string compressionAlgorithm)
        {
            if (token == null)
                throw LogHelper.LogArgumentNullException(nameof(token));

            if (string.IsNullOrEmpty(compressionAlgorithm))
                throw LogHelper.LogArgumentNullException(nameof(compressionAlgorithm));

            if (!CompressionProviderFactory.Default.IsSupportedAlgorithm(compressionAlgorithm))
                throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(TokenLogMessages.IDX10682, LogHelper.MarkAsNonPII(compressionAlgorithm))));

            var compressionProvider = CompressionProviderFactory.Default.CreateCompressionProvider(compressionAlgorithm);

            return compressionProvider.Compress(Encoding.UTF8.GetBytes(token)) ?? throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(TokenLogMessages.IDX10680, LogHelper.MarkAsNonPII(compressionAlgorithm))));
        }

        private static StringComparison GetStringComparisonRuleIf509(SecurityKey securityKey) => (securityKey is X509SecurityKey)
                            ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

        private static StringComparison GetStringComparisonRuleIf509OrECDsa(SecurityKey securityKey) => (securityKey is X509SecurityKey
                            || securityKey is ECDsaSecurityKey)
                            ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

        /// <summary>
        /// Creates a <see cref="ClaimsIdentity"/> from a <see cref="JsonWebToken"/>.
        /// </summary>
        /// <param name="jwtToken">The <see cref="JsonWebToken"/> to use as a <see cref="Claim"/> source.</param>
        /// <param name="validationParameters"> Contains parameters for validating the token.</param>
        /// <returns>A <see cref="ClaimsIdentity"/> containing the <see cref="JsonWebToken.Claims"/>.</returns>
        protected virtual ClaimsIdentity CreateClaimsIdentity(JsonWebToken jwtToken, TokenValidationParameters validationParameters)
        {
            _ = jwtToken ?? throw LogHelper.LogArgumentNullException(nameof(jwtToken));

            return CreateClaimsIdentityPrivate(jwtToken, validationParameters, GetActualIssuer(jwtToken));
        }

        /// <summary>
        /// Creates a <see cref="ClaimsIdentity"/> from a <see cref="JsonWebToken"/> with the specified issuer.
        /// </summary>
        /// <param name="jwtToken">The <see cref="JsonWebToken"/> to use as a <see cref="Claim"/> source.</param>
        /// <param name="validationParameters">Contains parameters for validating the token.</param>
        /// <param name="issuer">Specifies the issuer for the <see cref="ClaimsIdentity"/>.</param>
        /// <returns>A <see cref="ClaimsIdentity"/> containing the <see cref="JsonWebToken.Claims"/>.</returns>
        protected virtual ClaimsIdentity CreateClaimsIdentity(JsonWebToken jwtToken, TokenValidationParameters validationParameters, string issuer)
        {
            _ = jwtToken ?? throw LogHelper.LogArgumentNullException(nameof(jwtToken));

            if (string.IsNullOrWhiteSpace(issuer))
                issuer = GetActualIssuer(jwtToken);

            if (MapInboundClaims)
                return CreateClaimsIdentityWithMapping(jwtToken, validationParameters, issuer);

            return CreateClaimsIdentityPrivate(jwtToken, validationParameters, issuer);
        }

        private ClaimsIdentity CreateClaimsIdentityWithMapping(JsonWebToken jwtToken, TokenValidationParameters validationParameters, string issuer)
        {
            _ = validationParameters ?? throw LogHelper.LogArgumentNullException(nameof(validationParameters));

            ClaimsIdentity identity = validationParameters.CreateClaimsIdentity(jwtToken, issuer);
            foreach (Claim jwtClaim in jwtToken.Claims)
            {
                bool wasMapped = _inboundClaimTypeMap.TryGetValue(jwtClaim.Type, out string claimType);

                if (!wasMapped)
                    claimType = jwtClaim.Type;

                if (claimType == ClaimTypes.Actor)
                {
                    if (identity.Actor != null)
                        throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(
                                    LogMessages.IDX14112,
                                    LogHelper.MarkAsNonPII(JwtRegisteredClaimNames.Actort),
                                    jwtClaim.Value)));

                    if (CanReadToken(jwtClaim.Value))
                    {
                        JsonWebToken actor = ReadToken(jwtClaim.Value) as JsonWebToken;
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

        internal override ClaimsIdentity CreateClaimsIdentityInternal(SecurityToken securityToken, TokenValidationParameters tokenValidationParameters, string issuer)
        {
            return CreateClaimsIdentity(securityToken as JsonWebToken, tokenValidationParameters, issuer);
        }

        private static string GetActualIssuer(JsonWebToken jwtToken)
        {
            string actualIssuer = jwtToken.Issuer;
            if (string.IsNullOrWhiteSpace(actualIssuer))
            {
                if (LogHelper.IsEnabled(EventLogLevel.Verbose))
                    LogHelper.LogVerbose(TokenLogMessages.IDX10244, ClaimsIdentity.DefaultIssuer);

                actualIssuer = ClaimsIdentity.DefaultIssuer;
            }

            return actualIssuer;
        }

        private ClaimsIdentity CreateClaimsIdentityPrivate(JsonWebToken jwtToken, TokenValidationParameters validationParameters, string issuer)
        {
            _ = validationParameters ?? throw LogHelper.LogArgumentNullException(nameof(validationParameters));

            ClaimsIdentity identity = validationParameters.CreateClaimsIdentity(jwtToken, issuer);
            foreach (Claim jwtClaim in jwtToken.Claims)
            {
                string claimType = jwtClaim.Type;
                if (claimType == ClaimTypes.Actor)
                {
                    if (identity.Actor != null)
                        throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(LogMessages.IDX14112, LogHelper.MarkAsNonPII(JwtRegisteredClaimNames.Actort), jwtClaim.Value)));

                    if (CanReadToken(jwtClaim.Value))
                    {
                        JsonWebToken actor = ReadToken(jwtClaim.Value) as JsonWebToken;
                        identity.Actor = CreateClaimsIdentity(actor, validationParameters, issuer);
                    }
                }

                if (jwtClaim.Properties.Count == 0)
                {
                    identity.AddClaim(new Claim(claimType, jwtClaim.Value, jwtClaim.ValueType, issuer, issuer, identity));
                }
                else
                {
                    Claim claim = new Claim(claimType, jwtClaim.Value, jwtClaim.ValueType, issuer, issuer, identity);

                    foreach (var kv in jwtClaim.Properties)
                        claim.Properties[kv.Key] = kv.Value;

                    identity.AddClaim(claim);
                }
            }

            return identity;
        }

        /// <summary>
        /// Decrypts a JWE and returns the clear text 
        /// </summary>
        /// <param name="jwtToken">the JWE that contains the cypher text.</param>
        /// <param name="validationParameters">contains crypto material.</param>
        /// <returns>the decoded / cleartext contents of the JWE.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="jwtToken"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="validationParameters"/>  is null.</exception>
        /// <exception cref="SecurityTokenException">if '<paramref name="jwtToken"/> .Enc' is null or empty.</exception>
        /// <exception cref="SecurityTokenDecompressionFailedException">if decompression failed.</exception>
        /// <exception cref="SecurityTokenEncryptionKeyNotFoundException">if '<paramref name="jwtToken"/> .Kid' is not null AND decryption fails.</exception>
        /// <exception cref="SecurityTokenDecryptionFailedException">if the JWE was not able to be decrypted.</exception>
        public string DecryptToken(JsonWebToken jwtToken, TokenValidationParameters validationParameters)
        {
            return DecryptToken(jwtToken, validationParameters, null);
        }

        private string DecryptToken(JsonWebToken jwtToken, TokenValidationParameters validationParameters, BaseConfiguration configuration)
        {
            if (jwtToken == null)
                throw LogHelper.LogArgumentNullException(nameof(jwtToken));

            if (validationParameters == null)
                throw LogHelper.LogArgumentNullException(nameof(validationParameters));

            if (string.IsNullOrEmpty(jwtToken.Enc))
                throw LogHelper.LogExceptionMessage(new SecurityTokenException(LogHelper.FormatInvariant(TokenLogMessages.IDX10612)));

            var keys = GetContentEncryptionKeys(jwtToken, validationParameters, configuration);
            return JwtTokenUtilities.DecryptJwtToken(
                jwtToken,
                validationParameters,
                new JwtTokenDecryptionParameters
                {
                    DecompressionFunction = JwtTokenUtilities.DecompressToken,
                    Keys = keys
                });
        }

        /// <summary>
        /// Encrypts a JWS.
        /// </summary>
        /// <param name="innerJwt">A 'JSON Web Token' (JWT) in JWS Compact Serialization Format.</param>
        /// <param name="encryptingCredentials">Defines the security key and algorithm that will be used to encrypt the <paramref name="innerJwt"/>.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="innerJwt"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="encryptingCredentials"/> is null.</exception>
        /// <exception cref="ArgumentException">if both <see cref="EncryptingCredentials.CryptoProviderFactory"/> and <see cref="EncryptingCredentials.Key"/>.<see cref="CryptoProviderFactory"/> are null.</exception>
        /// <exception cref="SecurityTokenEncryptionFailedException">if the CryptoProviderFactory being used does not support the <see cref="EncryptingCredentials.Enc"/> (algorithm), <see cref="EncryptingCredentials.Key"/> pair.</exception>
        /// <exception cref="SecurityTokenEncryptionFailedException">if unable to create a token encryption provider for the <see cref="EncryptingCredentials.Enc"/> (algorithm), <see cref="EncryptingCredentials.Key"/> pair.</exception>
        /// <exception cref="SecurityTokenEncryptionFailedException">if encryption fails using the <see cref="EncryptingCredentials.Enc"/> (algorithm), <see cref="EncryptingCredentials.Key"/> pair.</exception>
        /// <exception cref="SecurityTokenEncryptionFailedException">if not using one of the supported content encryption key (CEK) algorithms: 128, 384 or 512 AesCbcHmac (this applies in the case of key wrap only, not direct encryption).</exception>
        public string EncryptToken(string innerJwt, EncryptingCredentials encryptingCredentials)
        {
            if (string.IsNullOrEmpty(innerJwt))
                throw LogHelper.LogArgumentNullException(nameof(innerJwt));

            if (encryptingCredentials == null)
                throw LogHelper.LogArgumentNullException(nameof(encryptingCredentials));

            return EncryptTokenPrivate(innerJwt, encryptingCredentials, null, null, null);
        }

        /// <summary>
        /// Encrypts a JWS.
        /// </summary>
        /// <param name="innerJwt">A 'JSON Web Token' (JWT) in JWS Compact Serialization Format.</param>
        /// <param name="encryptingCredentials">Defines the security key and algorithm that will be used to encrypt the <paramref name="innerJwt"/>.</param>
        /// <param name="additionalHeaderClaims">Defines the dictionary containing any custom header claims that need to be added to the outer JWT token header.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="innerJwt"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="encryptingCredentials"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="additionalHeaderClaims"/> is null.</exception>
        /// <exception cref="ArgumentException">if both <see cref="EncryptingCredentials.CryptoProviderFactory"/> and <see cref="EncryptingCredentials.Key"/>.<see cref="CryptoProviderFactory"/> are null.</exception>
        /// <exception cref="SecurityTokenEncryptionFailedException">if the CryptoProviderFactory being used does not support the <see cref="EncryptingCredentials.Enc"/> (algorithm), <see cref="EncryptingCredentials.Key"/> pair.</exception>
        /// <exception cref="SecurityTokenEncryptionFailedException">if unable to create a token encryption provider for the <see cref="EncryptingCredentials.Enc"/> (algorithm), <see cref="EncryptingCredentials.Key"/> pair.</exception>
        /// <exception cref="SecurityTokenEncryptionFailedException">if encryption fails using the <see cref="EncryptingCredentials.Enc"/> (algorithm), <see cref="EncryptingCredentials.Key"/> pair.</exception>
        /// <exception cref="SecurityTokenEncryptionFailedException">if not using one of the supported content encryption key (CEK) algorithms: 128, 384 or 512 AesCbcHmac (this applies in the case of key wrap only, not direct encryption).</exception>
        public string EncryptToken(string innerJwt, EncryptingCredentials encryptingCredentials, IDictionary<string, object> additionalHeaderClaims)
        {
            if (string.IsNullOrEmpty(innerJwt))
                throw LogHelper.LogArgumentNullException(nameof(innerJwt));

            if (encryptingCredentials == null)
                throw LogHelper.LogArgumentNullException(nameof(encryptingCredentials));

            if (additionalHeaderClaims == null)
                throw LogHelper.LogArgumentNullException(nameof(additionalHeaderClaims));

            return EncryptTokenPrivate(innerJwt, encryptingCredentials, null, additionalHeaderClaims, null);
        }

        /// <summary>
        /// Encrypts a JWS.
        /// </summary>
        /// <param name="innerJwt">A 'JSON Web Token' (JWT) in JWS Compact Serialization Format.</param>
        /// <param name="encryptingCredentials">Defines the security key and algorithm that will be used to encrypt the <paramref name="innerJwt"/>.</param>
        /// <param name="algorithm">Defines the compression algorithm that will be used to compress the 'innerJwt'.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="innerJwt"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="encryptingCredentials"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="algorithm"/> is null or empty.</exception>
        /// <exception cref="ArgumentException">if both <see cref="EncryptingCredentials.CryptoProviderFactory"/> and <see cref="EncryptingCredentials.Key"/>.<see cref="CryptoProviderFactory"/> are null.</exception>
        /// <exception cref="SecurityTokenEncryptionFailedException">if the CryptoProviderFactory being used does not support the <see cref="EncryptingCredentials.Enc"/> (algorithm), <see cref="EncryptingCredentials.Key"/> pair.</exception>
        /// <exception cref="SecurityTokenEncryptionFailedException">if unable to create a token encryption provider for the <see cref="EncryptingCredentials.Enc"/> (algorithm), <see cref="EncryptingCredentials.Key"/> pair.</exception>
        /// <exception cref="SecurityTokenCompressionFailedException">if compression using <paramref name="algorithm"/> fails.</exception>
        /// <exception cref="SecurityTokenEncryptionFailedException">if encryption fails using the <see cref="EncryptingCredentials.Enc"/> (algorithm), <see cref="EncryptingCredentials.Key"/> pair.</exception>
        /// <exception cref="SecurityTokenEncryptionFailedException">if not using one of the supported content encryption key (CEK) algorithms: 128, 384 or 512 AesCbcHmac (this applies in the case of key wrap only, not direct encryption).</exception>
        public string EncryptToken(string innerJwt, EncryptingCredentials encryptingCredentials, string algorithm)
        {
            if (string.IsNullOrEmpty(innerJwt))
                throw LogHelper.LogArgumentNullException(nameof(innerJwt));

            if (encryptingCredentials == null)
                throw LogHelper.LogArgumentNullException(nameof(encryptingCredentials));

            if (string.IsNullOrEmpty(algorithm))
                throw LogHelper.LogArgumentNullException(nameof(algorithm));

            return EncryptTokenPrivate(innerJwt, encryptingCredentials, algorithm, null, null);
        }

        /// <summary>
        /// Encrypts a JWS.
        /// </summary>
        /// <param name="innerJwt">A 'JSON Web Token' (JWT) in JWS Compact Serialization Format.</param>
        /// <param name="encryptingCredentials">Defines the security key and algorithm that will be used to encrypt the <paramref name="innerJwt"/>.</param>
        /// <param name="algorithm">Defines the compression algorithm that will be used to compress the <paramref name="innerJwt"/></param>
        /// <param name="additionalHeaderClaims">Defines the dictionary containing any custom header claims that need to be added to the outer JWT token header.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="innerJwt"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="encryptingCredentials"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="algorithm"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="additionalHeaderClaims"/> is null or empty.</exception>
        /// <exception cref="ArgumentException">if both <see cref="EncryptingCredentials.CryptoProviderFactory"/> and <see cref="EncryptingCredentials.Key"/>.<see cref="CryptoProviderFactory"/> are null.</exception>
        /// <exception cref="SecurityTokenEncryptionFailedException">if the CryptoProviderFactory being used does not support the <see cref="EncryptingCredentials.Enc"/> (algorithm), <see cref="EncryptingCredentials.Key"/> pair.</exception>
        /// <exception cref="SecurityTokenEncryptionFailedException">if unable to create a token encryption provider for the <see cref="EncryptingCredentials.Enc"/> (algorithm), <see cref="EncryptingCredentials.Key"/> pair.</exception>
        /// <exception cref="SecurityTokenCompressionFailedException">if compression using 'algorithm' fails.</exception>
        /// <exception cref="SecurityTokenEncryptionFailedException">if encryption fails using the <see cref="EncryptingCredentials.Enc"/> (algorithm), <see cref="EncryptingCredentials.Key"/> pair.</exception>
        /// <exception cref="SecurityTokenEncryptionFailedException">if not using one of the supported content encryption key (CEK) algorithms: 128, 384 or 512 AesCbcHmac (this applies in the case of key wrap only, not direct encryption).</exception>
        public string EncryptToken(string innerJwt, EncryptingCredentials encryptingCredentials, string algorithm, IDictionary<string, object> additionalHeaderClaims)
        {
            if (string.IsNullOrEmpty(innerJwt))
                throw LogHelper.LogArgumentNullException(nameof(innerJwt));

            if (encryptingCredentials == null)
                throw LogHelper.LogArgumentNullException(nameof(encryptingCredentials));

            if (string.IsNullOrEmpty(algorithm))
                throw LogHelper.LogArgumentNullException(nameof(algorithm));

            if (additionalHeaderClaims == null)
                throw LogHelper.LogArgumentNullException(nameof(additionalHeaderClaims));

            return EncryptTokenPrivate(innerJwt, encryptingCredentials, algorithm, additionalHeaderClaims, null);
        }

        private static string EncryptTokenPrivate(string innerJwt, EncryptingCredentials encryptingCredentials, string compressionAlgorithm, IDictionary<string, object> additionalHeaderClaims, string tokenType)
        {
            var cryptoProviderFactory = encryptingCredentials.CryptoProviderFactory ?? encryptingCredentials.Key.CryptoProviderFactory;

            if (cryptoProviderFactory == null)
                throw LogHelper.LogExceptionMessage(new ArgumentException(TokenLogMessages.IDX10620));

            byte[] wrappedKey = null;
            SecurityKey securityKey = JwtTokenUtilities.GetSecurityKey(encryptingCredentials, cryptoProviderFactory, additionalHeaderClaims, out wrappedKey);

            using (var encryptionProvider = cryptoProviderFactory.CreateAuthenticatedEncryptionProvider(securityKey, encryptingCredentials.Enc))
            {
                if (encryptionProvider == null)
                    throw LogHelper.LogExceptionMessage(new SecurityTokenEncryptionFailedException(LogMessages.IDX14103));

                var header = CreateDefaultJWEHeader(encryptingCredentials, compressionAlgorithm, tokenType);
                if (additionalHeaderClaims != null)
                    header.Merge(JObject.FromObject(additionalHeaderClaims));

                byte[] plainText;
                if (!string.IsNullOrEmpty(compressionAlgorithm))
                {
                    try
                    {
                        plainText = CompressToken(innerJwt, compressionAlgorithm);
                    }
                    catch (Exception ex)
                    {
                        throw LogHelper.LogExceptionMessage(new SecurityTokenCompressionFailedException(LogHelper.FormatInvariant(TokenLogMessages.IDX10680, LogHelper.MarkAsNonPII(compressionAlgorithm)), ex));
                    }
                }
                else
                {
                    plainText = Encoding.UTF8.GetBytes(innerJwt);
                }

                try
                {
                    var rawHeader = Base64UrlEncoder.Encode(Encoding.UTF8.GetBytes(header.ToString(Formatting.None)));
                    var encryptionResult = encryptionProvider.Encrypt(plainText, Encoding.ASCII.GetBytes(rawHeader));
                    return JwtConstants.DirectKeyUseAlg.Equals(encryptingCredentials.Alg) ?
                        string.Join(".", rawHeader, string.Empty, Base64UrlEncoder.Encode(encryptionResult.IV), Base64UrlEncoder.Encode(encryptionResult.Ciphertext), Base64UrlEncoder.Encode(encryptionResult.AuthenticationTag)):
                        string.Join(".", rawHeader, Base64UrlEncoder.Encode(wrappedKey), Base64UrlEncoder.Encode(encryptionResult.IV), Base64UrlEncoder.Encode(encryptionResult.Ciphertext), Base64UrlEncoder.Encode(encryptionResult.AuthenticationTag));
                }
                catch (Exception ex)
                {
                    throw LogHelper.LogExceptionMessage(new SecurityTokenEncryptionFailedException(LogHelper.FormatInvariant(TokenLogMessages.IDX10616, LogHelper.MarkAsNonPII(encryptingCredentials.Enc), encryptingCredentials.Key), ex));
                }
            }
        }

        private static SecurityKey ResolveTokenDecryptionKeyFromConfig(JsonWebToken jwtToken, BaseConfiguration configuration)
        {
            if (jwtToken == null)
                throw LogHelper.LogArgumentNullException(nameof(jwtToken));

            if (!string.IsNullOrEmpty(jwtToken.Kid) && configuration.TokenDecryptionKeys != null)
            {
                foreach (var key in configuration.TokenDecryptionKeys)
                {
                    if (key != null && string.Equals(key.KeyId, jwtToken.Kid, GetStringComparisonRuleIf509OrECDsa(key)))
                        return key;
                }
            }

            if (!string.IsNullOrEmpty(jwtToken.X5t) && configuration.TokenDecryptionKeys != null)
            {
                foreach (var key in configuration.TokenDecryptionKeys)
                {
                    if (key != null && string.Equals(key.KeyId, jwtToken.X5t, GetStringComparisonRuleIf509(key)))
                        return key;

                    var x509Key = key as X509SecurityKey;
                    if (x509Key != null && string.Equals(x509Key.X5t, jwtToken.X5t, StringComparison.OrdinalIgnoreCase))
                        return key;
                }
            }

            return null;
        }

        internal IEnumerable<SecurityKey> GetContentEncryptionKeys(JsonWebToken jwtToken, TokenValidationParameters validationParameters, BaseConfiguration configuration)
        {
            IEnumerable<SecurityKey> keys = null;

            // First we check to see if the caller has set a custom decryption resolver on TVP for the call, if so any keys set on TVP and keys in Configuration are ignored.
            // If no custom decryption resolver set, we'll check to see if they've set some static decryption keys on TVP. If a key found, we ignore configuration.
            // If no key found in TVP, we'll check the configuration.
            if (validationParameters.TokenDecryptionKeyResolver != null)
            {
                keys = validationParameters.TokenDecryptionKeyResolver(jwtToken.EncodedToken, jwtToken, jwtToken.Kid, validationParameters);
            }
            else
            {
                var key = ResolveTokenDecryptionKey(jwtToken.EncodedToken, jwtToken, validationParameters);
                if (key != null)
                {
                    if (LogHelper.IsEnabled(EventLogLevel.Informational))
                        LogHelper.LogInformation(TokenLogMessages.IDX10904, key);
                } 
                else if (configuration != null)
                {
                    key = ResolveTokenDecryptionKeyFromConfig(jwtToken, configuration);
                    if (key != null && LogHelper.IsEnabled(EventLogLevel.Informational))
                        LogHelper.LogInformation(TokenLogMessages.IDX10905, key);
                }
                    
                if (key != null)
                    keys = new List<SecurityKey> { key };                   
            }

            // on decryption for ECDH-ES, we get the public key from the EPK value see: https://datatracker.ietf.org/doc/html/rfc7518#appendix-C
            // we need the ECDSASecurityKey for the receiver, use TokenValidationParameters.TokenDecryptionKey

            // control gets here if:
            // 1. User specified delegate: TokenDecryptionKeyResolver returned null
            // 2. ResolveTokenDecryptionKey returned null
            // 3. ResolveTokenDecryptionKeyFromConfig returned null
            // Try all the keys. This is the degenerate case, not concerned about perf.
            if (keys == null)
            {
                keys = JwtTokenUtilities.GetAllDecryptionKeys(validationParameters);
                if (configuration != null)
                    keys = keys == null ? configuration.TokenDecryptionKeys : keys.Concat(configuration.TokenDecryptionKeys);
            }

            if (jwtToken.Alg.Equals(JwtConstants.DirectKeyUseAlg, StringComparison.Ordinal)
                || jwtToken.Alg.Equals(SecurityAlgorithms.EcdhEs, StringComparison.Ordinal))
                return keys;

            var unwrappedKeys = new List<SecurityKey>();
            // keep track of exceptions thrown, keys that were tried
            StringBuilder exceptionStrings = null;
            StringBuilder keysAttempted = null;
            foreach (var key in keys)
            {
                try
                {
#if NET472 || NET6_0_OR_GREATER
                    if (SupportedAlgorithms.EcdsaWrapAlgorithms.Contains(jwtToken.Alg))
                    {
                        // on decryption we get the public key from the EPK value see: https://datatracker.ietf.org/doc/html/rfc7518#appendix-C
                        var ecdhKeyExchangeProvider = new EcdhKeyExchangeProvider(
                            key as ECDsaSecurityKey,
                            validationParameters.TokenDecryptionKey as ECDsaSecurityKey,
                            jwtToken.Alg,
                            jwtToken.Enc);
                        jwtToken.TryGetHeaderValue(JwtHeaderParameterNames.Apu, out string apu);
                        jwtToken.TryGetHeaderValue(JwtHeaderParameterNames.Apv, out string apv);
                        SecurityKey kdf = ecdhKeyExchangeProvider.GenerateKdf(apu, apv);
                        var kwp = key.CryptoProviderFactory.CreateKeyWrapProviderForUnwrap(kdf, ecdhKeyExchangeProvider.GetEncryptionAlgorithm());
                        var unwrappedKey = kwp.UnwrapKey(Base64UrlEncoder.DecodeBytes(jwtToken.EncryptedKey));
                        unwrappedKeys.Add(new SymmetricSecurityKey(unwrappedKey));
                    }
                    else
#endif
                    if (key.CryptoProviderFactory.IsSupportedAlgorithm(jwtToken.Alg, key))
                    {
                        var kwp = key.CryptoProviderFactory.CreateKeyWrapProviderForUnwrap(key, jwtToken.Alg);
                        var unwrappedKey = kwp.UnwrapKey(jwtToken.EncryptedKeyBytes);
                        unwrappedKeys.Add(new SymmetricSecurityKey(unwrappedKey));
                    }
                }
                catch (Exception ex)
                {
                    (exceptionStrings ??= new StringBuilder()).AppendLine(ex.ToString());
                }

                (keysAttempted ??= new StringBuilder()).AppendLine(key.ToString());
            }

            if (unwrappedKeys.Count > 0 && exceptionStrings is null)
                return unwrappedKeys;
            else
                throw LogHelper.LogExceptionMessage(new SecurityTokenKeyWrapException(LogHelper.FormatInvariant(TokenLogMessages.IDX10618, (object)keysAttempted ?? "", (object)exceptionStrings ?? "", jwtToken)));
        }

        /// <summary>
        /// Returns a <see cref="SecurityKey"/> to use when decrypting a JWE.
        /// </summary>
        /// <param name="token">The <see cref="string"/> the token that is being decrypted.</param>
        /// <param name="jwtToken">The <see cref="JsonWebToken"/> that is being decrypted.</param>
        /// <param name="validationParameters">A <see cref="TokenValidationParameters"/>  required for validation.</param>
        /// <returns>Returns a <see cref="SecurityKey"/> to use for signature validation.</returns>
        /// <remarks>If key fails to resolve, then null is returned</remarks>
        protected virtual SecurityKey ResolveTokenDecryptionKey(string token, JsonWebToken jwtToken, TokenValidationParameters validationParameters)
        {
            if (jwtToken == null)
                throw LogHelper.LogArgumentNullException(nameof(jwtToken));

            if (validationParameters == null)
                throw LogHelper.LogArgumentNullException(nameof(validationParameters));

            StringComparison stringComparison = GetStringComparisonRuleIf509OrECDsa(validationParameters.TokenDecryptionKey);
            if (!string.IsNullOrEmpty(jwtToken.Kid))
            {
                if (validationParameters.TokenDecryptionKey != null
                    && string.Equals(validationParameters.TokenDecryptionKey.KeyId, jwtToken.Kid, stringComparison))
                    return validationParameters.TokenDecryptionKey;

                if (validationParameters.TokenDecryptionKeys != null)
                {
                    foreach (var key in validationParameters.TokenDecryptionKeys)
                    {
                        if (key != null && string.Equals(key.KeyId, jwtToken.Kid, GetStringComparisonRuleIf509OrECDsa(key)))
                            return key;
                    }
                }
            }

            if (!string.IsNullOrEmpty(jwtToken.X5t))
            {
                if (validationParameters.TokenDecryptionKey != null)
                {
                    if (string.Equals(validationParameters.TokenDecryptionKey.KeyId, jwtToken.X5t, stringComparison))
                        return validationParameters.TokenDecryptionKey;

                    var x509Key = validationParameters.TokenDecryptionKey as X509SecurityKey;
                    if (x509Key != null && string.Equals(x509Key.X5t, jwtToken.X5t, StringComparison.OrdinalIgnoreCase))
                        return validationParameters.TokenDecryptionKey;
                }

                if (validationParameters.TokenDecryptionKeys != null)
                {
                    foreach (var key in validationParameters.TokenDecryptionKeys)
                    {
                        if (key != null && string.Equals(key.KeyId, jwtToken.X5t, GetStringComparisonRuleIf509(key)))
                            return key;

                        var x509Key = key as X509SecurityKey;
                        if (x509Key != null && string.Equals(x509Key.X5t, jwtToken.X5t, StringComparison.OrdinalIgnoreCase))
                            return key;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Converts a string into an instance of <see cref="JsonWebToken"/>.
        /// </summary>
        /// <param name="token">A 'JSON Web Token' (JWT) in JWS or JWE Compact Serialization Format.</param>
        /// <returns>A <see cref="JsonWebToken"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="token"/> is null or empty.</exception>
        /// <exception cref="ArgumentException">'token.Length' is greater than <see cref="TokenHandler.MaximumTokenSizeInBytes"/>.</exception>
        /// <remarks><para>If the <paramref name="token"/> is in JWE Compact Serialization format, only the protected header will be deserialized.</para>
        /// This method is unable to decrypt the payload. Use <see cref="ValidateToken(string, TokenValidationParameters)"/>to obtain the payload.
        /// <para>The token is NOT validated and no security decisions should be made about the contents.
        /// Use <see cref="ValidateToken(string, TokenValidationParameters)"/> or <see cref="ValidateTokenAsync(string, TokenValidationParameters)"/> to ensure the token is acceptable.</para></remarks>
        public virtual JsonWebToken ReadJsonWebToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw LogHelper.LogArgumentNullException(nameof(token));

            if (token.Length > MaximumTokenSizeInBytes)
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(TokenLogMessages.IDX10209, LogHelper.MarkAsNonPII(token.Length), LogHelper.MarkAsNonPII(MaximumTokenSizeInBytes))));

            return new JsonWebToken(token);
        }

        /// <summary>
        /// Converts a string into an instance of <see cref="JsonWebToken"/>.
        /// </summary>
        /// <param name="token">A 'JSON Web Token' (JWT) in JWS or JWE Compact Serialization Format.</param>
        /// <returns>A <see cref="JsonWebToken"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="token"/> is null or empty.</exception>
        /// <exception cref="ArgumentException">'token.Length' is greater than <see cref="TokenHandler.MaximumTokenSizeInBytes"/>.</exception>
        /// <remarks>The token is NOT validated and no security decisions should be made about the contents.
        /// <para>Use <see cref="ValidateToken(string, TokenValidationParameters)"/> or <see cref="ValidateTokenAsync(string, TokenValidationParameters)"/> to ensure the token is acceptable.</para></remarks>
        public override SecurityToken ReadToken(string token)
        {
            return ReadJsonWebToken(token);
        }

        /// <summary>
        /// Validates a JWS or a JWE.
        /// </summary>
        /// <param name="token">A 'JSON Web Token' (JWT) in JWS or JWE Compact Serialization Format.</param>
        /// <param name="validationParameters">A <see cref="TokenValidationParameters"/>  required for validation.</param>
        /// <returns>A <see cref="TokenValidationResult"/></returns>
        public virtual TokenValidationResult ValidateToken(string token, TokenValidationParameters validationParameters)
        {
            return ValidateTokenAsync(token, validationParameters).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Validates a token.
        /// On a validation failure, no exception will be thrown; instead, the exception will be set in the returned TokenValidationResult.Exception property.
        /// Callers should always check the TokenValidationResult.IsValid property to verify the validity of the result.
        /// </summary>
        /// <param name="token">The token to be validated.</param>
        /// <param name="validationParameters">A <see cref="TokenValidationParameters"/> required for validation.</param>
        /// <returns>A <see cref="TokenValidationResult"/></returns>
        /// <remarks>
        /// <para>TokenValidationResult.Exception will be set to one of the following exceptions if the  <paramref name="token"/> is invalid.</para>
        /// <para><exception cref="ArgumentNullException">if <paramref name="token"/> is null or empty.</exception></para>
        /// <para><exception cref="ArgumentNullException">if <paramref name="validationParameters"/> is null.</exception></para>
        /// <para><exception cref="ArgumentException">'token.Length' is greater than <see cref="TokenHandler.MaximumTokenSizeInBytes"/>.</exception></para>
        /// <para><exception cref="SecurityTokenMalformedException">if <paramref name="token"/> is not a valid <see cref="JsonWebToken"/>, <see cref="ReadToken(string, TokenValidationParameters)"/></exception></para>
        /// <para><exception cref="SecurityTokenMalformedException">if the validationParameters.TokenReader delegate is not able to parse/read the token as a valid <see cref="JsonWebToken"/>, <see cref="ReadToken(string, TokenValidationParameters)"/></exception></para>
        /// </remarks>
        public override async Task<TokenValidationResult> ValidateTokenAsync(string token, TokenValidationParameters validationParameters)
        {
            if (string.IsNullOrEmpty(token))
                return new TokenValidationResult { Exception = LogHelper.LogArgumentNullException(nameof(token)), IsValid = false };

            if (validationParameters == null)
                return new TokenValidationResult { Exception = LogHelper.LogArgumentNullException(nameof(validationParameters)), IsValid = false };

            if (token.Length > MaximumTokenSizeInBytes)
                return new TokenValidationResult { Exception = LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(TokenLogMessages.IDX10209, LogHelper.MarkAsNonPII(token.Length), LogHelper.MarkAsNonPII(MaximumTokenSizeInBytes)))), IsValid = false };

            try
            {
                TokenValidationResult result = ReadToken(token, validationParameters);
                if (result.IsValid)
                    return await ValidateTokenAsync(result.SecurityToken, validationParameters).ConfigureAwait(false);

                return result;
            }
            catch (Exception ex)
            {
                return new TokenValidationResult
                {
                    Exception = ex,
                    IsValid = false
                };
            }
        }

        /// <inheritdoc/>
        public override async Task<TokenValidationResult> ValidateTokenAsync(SecurityToken token, TokenValidationParameters validationParameters)
        {
            if (token == null)
                throw LogHelper.LogArgumentNullException(nameof(token));

            if (validationParameters == null)
                return new TokenValidationResult { Exception = LogHelper.LogArgumentNullException(nameof(validationParameters)), IsValid = false };

            var jwt = token as JsonWebToken;
            if (jwt == null)
                return new TokenValidationResult { Exception = LogHelper.LogExceptionMessage(new SecurityTokenMalformedException(LogMessages.IDX14100)), IsValid = false };

            try
            {
                return await ValidateTokenAsync(jwt, validationParameters).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new TokenValidationResult
                {
                    Exception = ex,
                    IsValid = false
                };
            }
        }

        /// <summary>
        /// Converts a string into an instance of <see cref="JsonWebToken"/>.
        /// </summary>
        /// <param name="token">A 'JSON Web Token' (JWT) in JWS or JWE Compact Serialization Format.</param>
        /// <param name="validationParameters">A <see cref="TokenValidationParameters"/> whose TokenReader, if set, will be used to read a JWT.</param>
        /// <returns>A <see cref="TokenValidationResult"/></returns>
        /// <exception cref="SecurityTokenMalformedException">if the validationParameters.TokenReader delegate is not able to parse/read the token as a valid <see cref="JsonWebToken"/>.</exception>
        /// <exception cref="SecurityTokenMalformedException">if <paramref name="token"/> is not a valid JWT, <see cref="JsonWebToken"/>.</exception>
        private static TokenValidationResult ReadToken(string token, TokenValidationParameters validationParameters)
        {
            JsonWebToken jsonWebToken = null;
            if (validationParameters.TokenReader != null)
            {
                var securityToken = validationParameters.TokenReader(token, validationParameters);
                if (securityToken == null)
                    throw LogHelper.LogExceptionMessage(new SecurityTokenMalformedException(LogHelper.FormatInvariant(TokenLogMessages.IDX10510, LogHelper.MarkAsSecurityArtifact(token, JwtTokenUtilities.SafeLogJwtToken))));

                jsonWebToken = securityToken as JsonWebToken;
                if (jsonWebToken == null)
                    throw LogHelper.LogExceptionMessage(new SecurityTokenMalformedException(LogHelper.FormatInvariant(TokenLogMessages.IDX10509, typeof(JsonWebToken), securityToken.GetType(), LogHelper.MarkAsSecurityArtifact(token, JwtTokenUtilities.SafeLogJwtToken))));
            }
            else
            {
#pragma warning disable CA1031 // Do not catch general exception types
                try
                {
                    jsonWebToken = new JsonWebToken(token);
                }
                catch (Exception ex)
                {
                    return new TokenValidationResult
                    {
                        Exception = LogHelper.LogExceptionMessage(new SecurityTokenMalformedException(LogMessages.IDX14100, ex)),
                        IsValid = false
                    };
                }
#pragma warning restore CA1031 // Do not catch general exception types
            }

            return new TokenValidationResult
            {
                SecurityToken = jsonWebToken,
                IsValid = true
            };
        }

        /// <summary>
        ///  Private method for token validation, responsible for:
        ///  (1) Obtaining a configuration from the <see cref="TokenValidationParameters.ConfigurationManager"/>.
        ///  (2) Revalidating using the Last Known Good Configuration (if present), and obtaining a refreshed configuration (if necessary) and revalidating using it.
        /// </summary>
        /// <param name="jsonWebToken">The JWT token</param>
        /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validation.</param>
        /// <returns></returns>
        private async ValueTask<TokenValidationResult> ValidateTokenAsync(JsonWebToken jsonWebToken, TokenValidationParameters validationParameters)
        {
            BaseConfiguration currentConfiguration = null;
            if (validationParameters.ConfigurationManager != null)
            {
                try
                {
                    currentConfiguration = await validationParameters.ConfigurationManager.GetBaseConfigurationAsync(CancellationToken.None).ConfigureAwait(false);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    // The exception is not re-thrown as the TokenValidationParameters may have the issuer and signing key set
                    // directly on them, allowing the library to continue with token validation.
                    if (LogHelper.IsEnabled(EventLogLevel.Warning))
                        LogHelper.LogWarning(LogHelper.FormatInvariant(TokenLogMessages.IDX10261, validationParameters.ConfigurationManager.MetadataAddress, ex.ToString()));
                }
            }

            TokenValidationResult tokenValidationResult = await ValidateTokenAsync(jsonWebToken, validationParameters, currentConfiguration).ConfigureAwait(false);
            if (validationParameters.ConfigurationManager != null)
            {
                if (tokenValidationResult.IsValid)
                {
                    // Set current configuration as LKG if it exists.
                    if (currentConfiguration != null)
                        validationParameters.ConfigurationManager.LastKnownGoodConfiguration = currentConfiguration;

                    return tokenValidationResult;
                }
                else if (TokenUtilities.IsRecoverableException(tokenValidationResult.Exception))
                {
                    // If we were still unable to validate, attempt to refresh the configuration and validate using it
                    // but ONLY if the currentConfiguration is not null. We want to avoid refreshing the configuration on
                    // retrieval error as this case should have already been hit before. This refresh handles the case
                    // where a new valid configuration was somehow published during validation time.
                    if (currentConfiguration != null)
                    {
                        validationParameters.ConfigurationManager.RequestRefresh();
                        validationParameters.RefreshBeforeValidation = true;
                        var lastConfig = currentConfiguration;
                        currentConfiguration = await validationParameters.ConfigurationManager.GetBaseConfigurationAsync(CancellationToken.None).ConfigureAwait(false);

                        // Only try to re-validate using the newly obtained config if it doesn't reference equal the previously used configuration.
                        if (lastConfig != currentConfiguration)
                        {
                            tokenValidationResult = await ValidateTokenAsync(jsonWebToken, validationParameters, currentConfiguration).ConfigureAwait(false);

                            if (tokenValidationResult.IsValid)
                            {
                                validationParameters.ConfigurationManager.LastKnownGoodConfiguration = currentConfiguration;
                                return tokenValidationResult;
                            }
                        }
                    }

                    if (validationParameters.ConfigurationManager.UseLastKnownGoodConfiguration)
                    {
                        validationParameters.RefreshBeforeValidation = false;
                        validationParameters.ValidateWithLKG = true;
                        var recoverableException = tokenValidationResult.Exception;

                        foreach (BaseConfiguration lkgConfiguration in validationParameters.ConfigurationManager.GetValidLkgConfigurations())
                        {
                            if (!lkgConfiguration.Equals(currentConfiguration) && TokenUtilities.IsRecoverableConfiguration(jsonWebToken.Kid, currentConfiguration, lkgConfiguration, recoverableException))
                            {
                                tokenValidationResult = await ValidateTokenAsync(jsonWebToken, validationParameters, lkgConfiguration).ConfigureAwait(false);

                                if (tokenValidationResult.IsValid)
                                    return tokenValidationResult;
                            }
                        }
                    }
                }
            }

            return tokenValidationResult;
        }

        private ValueTask<TokenValidationResult> ValidateTokenAsync(JsonWebToken jsonWebToken, TokenValidationParameters validationParameters, BaseConfiguration configuration)
        {
            return jsonWebToken.IsEncrypted ?
                ValidateJWEAsync(jsonWebToken, validationParameters, configuration) :
                ValidateJWSAsync(jsonWebToken, validationParameters, configuration);
        }

        private async ValueTask<TokenValidationResult> ValidateJWSAsync(JsonWebToken jsonWebToken, TokenValidationParameters validationParameters, BaseConfiguration configuration)
        {
            try
            {
                TokenValidationResult tokenValidationResult;
                if (validationParameters.TransformBeforeSignatureValidation != null)
                    jsonWebToken = validationParameters.TransformBeforeSignatureValidation(jsonWebToken, validationParameters) as JsonWebToken;

                if (validationParameters.SignatureValidator != null || validationParameters.SignatureValidatorUsingConfiguration != null)
                {
                    var validatedToken = ValidateSignatureUsingDelegates(jsonWebToken, validationParameters, configuration);
                    tokenValidationResult = await ValidateTokenPayloadAsync(validatedToken, validationParameters, configuration).ConfigureAwait(false);
                    Tokens.Validators.ValidateIssuerSecurityKey(validatedToken.SigningKey, validatedToken, validationParameters, configuration);
                }
                else
                {
                    if (validationParameters.ValidateSignatureLast)
                    {
                        tokenValidationResult = await ValidateTokenPayloadAsync(jsonWebToken, validationParameters, configuration).ConfigureAwait(false);
                        if (tokenValidationResult.IsValid)
                            tokenValidationResult.SecurityToken = ValidateSignatureAndIssuerSecurityKey(jsonWebToken, validationParameters, configuration);
                    }
                    else
                    {
                        var validatedToken = ValidateSignatureAndIssuerSecurityKey(jsonWebToken, validationParameters, configuration);
                        tokenValidationResult = await ValidateTokenPayloadAsync(validatedToken, validationParameters, configuration).ConfigureAwait(false);
                    }
                }

                return tokenValidationResult;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return new TokenValidationResult
                {
                    Exception = ex,
                    IsValid = false,
                    TokenOnFailedValidation = validationParameters.IncludeTokenOnFailedValidation ? jsonWebToken : null
                };
            }
        }

        private async ValueTask<TokenValidationResult> ValidateJWEAsync(JsonWebToken jwtToken, TokenValidationParameters validationParameters, BaseConfiguration configuration)
        {
            try
            {
                TokenValidationResult tokenValidationResult = ReadToken(DecryptToken(jwtToken, validationParameters, configuration), validationParameters);
                if (!tokenValidationResult.IsValid)
                    return tokenValidationResult;

                tokenValidationResult = await ValidateJWSAsync(tokenValidationResult.SecurityToken as JsonWebToken, validationParameters, configuration).ConfigureAwait(false);
                if (!tokenValidationResult.IsValid)
                    return tokenValidationResult;

                jwtToken.InnerToken = tokenValidationResult.SecurityToken as JsonWebToken;
                jwtToken.Payload = (tokenValidationResult.SecurityToken as JsonWebToken).Payload;
                return new TokenValidationResult
                {
                    SecurityToken = jwtToken,
                    ClaimsIdentityNoLocking = tokenValidationResult.ClaimsIdentityNoLocking,
                    IsValid = true,
                    TokenType = tokenValidationResult.TokenType
                };
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return new TokenValidationResult
                {
                    Exception = ex,
                    IsValid = false,
                    TokenOnFailedValidation = validationParameters.IncludeTokenOnFailedValidation ? jwtToken : null
                };
            }
        }

        private static JsonWebToken ValidateSignatureUsingDelegates(JsonWebToken jsonWebToken, TokenValidationParameters validationParameters, BaseConfiguration configuration)
        {
            if (validationParameters.SignatureValidatorUsingConfiguration != null)
            {
                var validatedToken = validationParameters.SignatureValidatorUsingConfiguration(jsonWebToken.EncodedToken, validationParameters, configuration);
                if (validatedToken == null)
                    throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant(TokenLogMessages.IDX10505, jsonWebToken)));

                if (!(validatedToken is JsonWebToken validatedJsonWebToken))
                    throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant(TokenLogMessages.IDX10506, LogHelper.MarkAsNonPII(typeof(JsonWebToken)), LogHelper.MarkAsNonPII(validatedToken.GetType()), jsonWebToken)));

                return validatedJsonWebToken;
            }
            else if (validationParameters.SignatureValidator != null)
            {
                var validatedToken = validationParameters.SignatureValidator(jsonWebToken.EncodedToken, validationParameters);
                if (validatedToken == null)
                    throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant(TokenLogMessages.IDX10505, jsonWebToken)));

                if (!(validatedToken is JsonWebToken validatedJsonWebToken))
                    throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant(TokenLogMessages.IDX10506, LogHelper.MarkAsNonPII(typeof(JsonWebToken)), LogHelper.MarkAsNonPII(validatedToken.GetType()), jsonWebToken)));

                return validatedJsonWebToken;
            }

            throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant(TokenLogMessages.IDX10505, jsonWebToken)));
        }

        private static JsonWebToken ValidateSignatureAndIssuerSecurityKey(JsonWebToken jsonWebToken, TokenValidationParameters validationParameters, BaseConfiguration configuration)
        {
            JsonWebToken validatedToken = ValidateSignature(jsonWebToken, validationParameters, configuration);
            Microsoft.IdentityModel.Tokens.Validators.ValidateIssuerSecurityKey(validatedToken.SigningKey, jsonWebToken, validationParameters, configuration);

            return validatedToken;
        }

        private async ValueTask<TokenValidationResult> ValidateTokenPayloadAsync(JsonWebToken jsonWebToken, TokenValidationParameters validationParameters, BaseConfiguration configuration)
        {
            var expires = jsonWebToken.HasPayloadClaim(JwtRegisteredClaimNames.Exp) ? (DateTime?)jsonWebToken.ValidTo : null;
            var notBefore = jsonWebToken.HasPayloadClaim(JwtRegisteredClaimNames.Nbf) ? (DateTime?)jsonWebToken.ValidFrom : null;

            Tokens.Validators.ValidateLifetime(notBefore, expires, jsonWebToken, validationParameters);
            Tokens.Validators.ValidateAudience(jsonWebToken.Audiences, jsonWebToken, validationParameters);
            string issuer = await Tokens.Validators.ValidateIssuerAsync(jsonWebToken.Issuer, jsonWebToken, validationParameters, configuration).ConfigureAwait(false);

            Tokens.Validators.ValidateTokenReplay(expires, jsonWebToken.EncodedToken, validationParameters);
            if (validationParameters.ValidateActor && !string.IsNullOrWhiteSpace(jsonWebToken.Actor))
            {
                // Infinite recursion should not occur here, as the JsonWebToken passed into this method is (1) constructed from a string
                // AND (2) the signature is successfully validated on it. (1) implies that even if there are nested actor tokens,
                // they must end at some point since they cannot reference one another. (2) means that the token has a valid signature
                // and (since issuer validation occurs first) came from a trusted authority.
                // NOTE: More than one nested actor token should not be considered a valid token, but if we somehow encounter one,
                // this code will still work properly.
                TokenValidationResult tokenValidationResult =
                    await ValidateTokenAsync(jsonWebToken.Actor, validationParameters.ActorValidationParameters ?? validationParameters).ConfigureAwait(false);

                if (!tokenValidationResult.IsValid)
                    return tokenValidationResult;
            }

            string tokenType = Tokens.Validators.ValidateTokenType(jsonWebToken.Typ, jsonWebToken, validationParameters);
            return new TokenValidationResult(jsonWebToken, this, validationParameters.Clone(), issuer)
            {
                IsValid = true,
                TokenType = tokenType
            };
        }

        /// <summary>
        /// Validates the JWT signature.
        /// </summary>
        private static JsonWebToken ValidateSignature(JsonWebToken jwtToken, TokenValidationParameters validationParameters, BaseConfiguration configuration)
        {
            bool kidMatched = false;
            IEnumerable<SecurityKey> keys = null;

            if (!jwtToken.IsSigned)
            {
                if (validationParameters.RequireSignedTokens)
                    throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant(TokenLogMessages.IDX10504, jwtToken)));
                else
                    return jwtToken;
            }

            if (validationParameters.IssuerSigningKeyResolverUsingConfiguration != null)
            {
                keys = validationParameters.IssuerSigningKeyResolverUsingConfiguration(jwtToken.EncodedToken, jwtToken, jwtToken.Kid, validationParameters, configuration);
            }
            else if (validationParameters.IssuerSigningKeyResolver != null)
            {
                keys = validationParameters.IssuerSigningKeyResolver(jwtToken.EncodedToken, jwtToken, jwtToken.Kid, validationParameters);
            }
            else
            {
                var key = JwtTokenUtilities.ResolveTokenSigningKey(jwtToken.Kid, jwtToken.X5t, validationParameters, configuration);
                if (key != null)
                {
                    kidMatched = true;
                    keys = new List<SecurityKey> { key };
                }
            }

            if (validationParameters.TryAllIssuerSigningKeys && keys.IsNullOrEmpty())
            {
                // control gets here if:
                // 1. User specified delegate: IssuerSigningKeyResolver returned null
                // 2. ResolveIssuerSigningKey returned null
                // Try all the keys. This is the degenerate case, not concerned about perf.
                keys = TokenUtilities.GetAllSigningKeys(configuration, validationParameters);
            }

            // keep track of exceptions thrown, keys that were tried
            StringBuilder exceptionStrings = null;
            StringBuilder keysAttempted = null;
            var kidExists = !string.IsNullOrEmpty(jwtToken.Kid);

            if (keys != null)
            {
                foreach (var key in keys)
                {
                    try
                    {
                        if (ValidateSignature(jwtToken, key, validationParameters))
                        {
                            if (LogHelper.IsEnabled(EventLogLevel.Informational))
                                LogHelper.LogInformation(TokenLogMessages.IDX10242, jwtToken);

                            jwtToken.SigningKey = key;
                            return jwtToken;
                        }
                    }
                    catch (Exception ex)
                    {
                        (exceptionStrings ??= new StringBuilder()).AppendLine(ex.ToString());
                    }

                    if (key != null)
                    {
                        (keysAttempted ??= new StringBuilder()).Append(key.ToString()).Append(" , KeyId: ").AppendLine(key.KeyId);
                        if (kidExists && !kidMatched && key.KeyId != null)
                            kidMatched = jwtToken.Kid.Equals(key.KeyId, key is X509SecurityKey ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
                    }
                }
            }

            // Get information on where keys used during token validation came from for debugging purposes.
            var keysInTokenValidationParameters = TokenUtilities.GetAllSigningKeys(validationParameters: validationParameters);
            var keysInConfiguration = TokenUtilities.GetAllSigningKeys(configuration);
            var numKeysInTokenValidationParameters = keysInTokenValidationParameters.Count();
            var numKeysInConfiguration = keysInConfiguration.Count();

            if (kidExists)
            {
                if (kidMatched)
                {
                    JsonWebToken localJwtToken = jwtToken; // avoid closure on non-exceptional path
                    var isKidInTVP = keysInTokenValidationParameters.Any(x => x.KeyId.Equals(localJwtToken.Kid));
                    var keyLocation = isKidInTVP ? "TokenValidationParameters" : "Configuration";
                    throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant(TokenLogMessages.IDX10511,
                        (object)keysAttempted ?? "",
                        LogHelper.MarkAsNonPII(numKeysInTokenValidationParameters),
                        LogHelper.MarkAsNonPII(numKeysInConfiguration),
                        LogHelper.MarkAsNonPII(keyLocation),
                        LogHelper.MarkAsNonPII(jwtToken.Kid),
                        (object)exceptionStrings ?? "",
                        jwtToken)));
                }

                var expires = jwtToken.TryGetClaim(JwtRegisteredClaimNames.Exp, out var _) ? (DateTime?)jwtToken.ValidTo : null;
                var notBefore = jwtToken.TryGetClaim(JwtRegisteredClaimNames.Nbf, out var _) ? (DateTime?)jwtToken.ValidFrom : null;

                if (!validationParameters.ValidateSignatureLast)
                {
                    InternalValidators.ValidateAfterSignatureFailed(
                        jwtToken,
                        notBefore,
                        expires,
                        jwtToken.Audiences,
                        validationParameters,
                        configuration);
                }
            }

            if (keysAttempted is not null)
                throw LogHelper.LogExceptionMessage(new SecurityTokenSignatureKeyNotFoundException(LogHelper.FormatInvariant(TokenLogMessages.IDX10503,
                    keysAttempted,
                    LogHelper.MarkAsNonPII(numKeysInTokenValidationParameters),
                    LogHelper.MarkAsNonPII(numKeysInConfiguration),
                    (object)exceptionStrings ?? "",
                    jwtToken)));

            throw LogHelper.LogExceptionMessage(new SecurityTokenSignatureKeyNotFoundException(TokenLogMessages.IDX10500));
        }

        /// <summary>
        /// Obtains a <see cref="SignatureProvider "/> and validates the signature.
        /// </summary>
        /// <param name="encodedBytes">Bytes to validate.</param>
        /// <param name="signature">Signature to compare against.</param>
        /// <param name="key"><See cref="SecurityKey"/> to use.</param>
        /// <param name="algorithm">Crypto algorithm to use.</param>
        /// <param name="securityToken">The <see cref="SecurityToken"/> being validated.</param>
        /// <param name="validationParameters">Priority will be given to <see cref="TokenValidationParameters.CryptoProviderFactory"/> over <see cref="SecurityKey.CryptoProviderFactory"/>.</param>
        /// <returns>'true' if signature is valid.</returns>
        internal static bool ValidateSignature(byte[] encodedBytes, byte[] signature, SecurityKey key, string algorithm, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            var cryptoProviderFactory = validationParameters.CryptoProviderFactory ?? key.CryptoProviderFactory;
            if (!cryptoProviderFactory.IsSupportedAlgorithm(algorithm, key))
            {
                if (LogHelper.IsEnabled(EventLogLevel.Informational))
                    LogHelper.LogInformation(LogMessages.IDX14000, LogHelper.MarkAsNonPII(algorithm), key);

                return false;
            }

            Tokens.Validators.ValidateAlgorithm(algorithm, key, securityToken, validationParameters);

            var signatureProvider = cryptoProviderFactory.CreateForVerifying(key, algorithm);
            if (signatureProvider == null)
                throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(TokenLogMessages.IDX10636, key == null ? "Null" : key.ToString(), LogHelper.MarkAsNonPII(algorithm))));

            try
            {
                return signatureProvider.Verify(encodedBytes, signature);
            }
            finally
            {
                cryptoProviderFactory.ReleaseSignatureProvider(signatureProvider);
            }
        }

        internal static bool IsSignatureValid(byte[] signatureBytes, int signatureBytesLength, SignatureProvider signatureProvider, byte[] dataToVerify, int dataToVerifyLength)
        {
            if (signatureProvider is SymmetricSignatureProvider)
            {
                return signatureProvider.Verify(dataToVerify, 0, dataToVerifyLength, signatureBytes, 0, signatureBytesLength);
            }
            else
            {
                if (signatureBytes.Length == signatureBytesLength)
                {
                    return signatureProvider.Verify(dataToVerify, 0, dataToVerifyLength, signatureBytes, 0, signatureBytesLength);
                }
                else
                {
                    byte[] sigBytes = new byte[signatureBytesLength];
                    Array.Copy(signatureBytes, 0, sigBytes, 0, signatureBytesLength);
                    return signatureProvider.Verify(dataToVerify, 0, dataToVerifyLength, sigBytes, 0, signatureBytesLength);
                }
            }
        }

        internal static bool ValidateSignature(byte[] bytes, int len, string stringWithSignature, int signatureStartIndex, SignatureProvider signatureProvider)
        {
            return Base64UrlEncoding.Decode<bool, SignatureProvider, byte[], int>(
                    stringWithSignature,
                    signatureStartIndex + 1,
                    stringWithSignature.Length - signatureStartIndex - 1,
                    signatureProvider,
                    bytes,
                    len,
                    IsSignatureValid);
        }

        internal static bool ValidateSignature(JsonWebToken jsonWebToken, SecurityKey key, TokenValidationParameters validationParameters)
        {
            var cryptoProviderFactory = validationParameters.CryptoProviderFactory ?? key.CryptoProviderFactory;
            if (!cryptoProviderFactory.IsSupportedAlgorithm(jsonWebToken.Alg, key))
            {
                if (LogHelper.IsEnabled(EventLogLevel.Informational))
                    LogHelper.LogInformation(LogMessages.IDX14000, LogHelper.MarkAsNonPII(jsonWebToken.Alg), key);

                return false;
            }

            Tokens.Validators.ValidateAlgorithm(jsonWebToken.Alg, key, jsonWebToken, validationParameters);
            var signatureProvider = cryptoProviderFactory.CreateForVerifying(key, jsonWebToken.Alg);
            try
            {
                if (signatureProvider == null)
                    throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(TokenLogMessages.IDX10636, key == null ? "Null" : key.ToString(), LogHelper.MarkAsNonPII(jsonWebToken.Alg))));

                return EncodingUtils.PerformEncodingDependentOperation<bool, string, int, SignatureProvider>(
                    jsonWebToken.EncodedToken,
                    0,
                    jsonWebToken.Dot2,
                    Encoding.UTF8,
                    jsonWebToken.EncodedToken,
                    jsonWebToken.Dot2,
                    signatureProvider,
                    ValidateSignature);
            }
            finally
            {
                cryptoProviderFactory.ReleaseSignatureProvider(signatureProvider);
            }
        }
    }
}
