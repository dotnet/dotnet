// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using TokenLogMessages = Microsoft.IdentityModel.Tokens.LogMessages;

namespace System.IdentityModel.Tokens.Jwt
{
    /// <summary>
    /// A <see cref="SecurityTokenHandler"/> designed for creating and validating Json Web Tokens. See: https://datatracker.ietf.org/doc/html/rfc7519 and http://www.rfc-editor.org/info/rfc7515
    /// </summary>
    public class JwtSecurityTokenHandler : SecurityTokenHandler
    {

        private delegate bool CertMatcher(X509Certificate2 cert);
        private ISet<string> _inboundClaimFilter;
        private IDictionary<string, string> _inboundClaimTypeMap;
        private static string _jsonClaimType = _namespace + "/json_type";
        private const string _namespace = "http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties";
        private const string _className = "System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler";
        private IDictionary<string, string> _outboundClaimTypeMap;
        private Dictionary<string, string> _outboundAlgorithmMap = null;
        private static string _shortClaimType = _namespace + "/ShortTypeName";
        private bool _mapInboundClaims = DefaultMapInboundClaims;

        /// <summary>
        /// Default claim type mapping for inbound claims.
        /// </summary>
        public static IDictionary<string, string> DefaultInboundClaimTypeMap = new Dictionary<string, string>(ClaimTypeMapping.InboundClaimTypeMap);

        /// <summary>
        /// Default value for the flag that determines whether or not the InboundClaimTypeMap is used.
        /// </summary>
        public static bool DefaultMapInboundClaims = true;

        /// <summary>
        /// Default claim type mapping for outbound claims.
        /// </summary>
        public static IDictionary<string, string> DefaultOutboundClaimTypeMap = new Dictionary<string, string>(ClaimTypeMapping.OutboundClaimTypeMap);

        /// <summary>
        /// Default claim type filter list.
        /// </summary>
        public static ISet<string> DefaultInboundClaimFilter = ClaimTypeMapping.InboundClaimFilter;

        /// <summary>
        /// Default JwtHeader algorithm mapping
        /// </summary>
        public static IDictionary<string, string> DefaultOutboundAlgorithmMap = new Dictionary<string, string>
        {
            { SecurityAlgorithms.EcdsaSha256Signature, SecurityAlgorithms.EcdsaSha256 },
            { SecurityAlgorithms.EcdsaSha384Signature, SecurityAlgorithms.EcdsaSha384 },
            { SecurityAlgorithms.EcdsaSha512Signature, SecurityAlgorithms.EcdsaSha512 },
            { SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.HmacSha256 },
            { SecurityAlgorithms.HmacSha384Signature, SecurityAlgorithms.HmacSha384 },
            { SecurityAlgorithms.HmacSha512Signature, SecurityAlgorithms.HmacSha512 },
            { SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.RsaSha256 },
            { SecurityAlgorithms.RsaSha384Signature, SecurityAlgorithms.RsaSha384 },
            { SecurityAlgorithms.RsaSha512Signature, SecurityAlgorithms.RsaSha512 },
        };

        /// <summary>
        /// Static initializer for a new object. Static initializers run before the first instance of the type is created.
        /// </summary>
        static JwtSecurityTokenHandler()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtSecurityTokenHandler"/> class.
        /// </summary>
        public JwtSecurityTokenHandler()
        {
            if (_mapInboundClaims)
                _inboundClaimTypeMap = new Dictionary<string, string>(DefaultInboundClaimTypeMap);
            else
                _inboundClaimTypeMap = new Dictionary<string, string>();

            _outboundClaimTypeMap = new Dictionary<string, string>(DefaultOutboundClaimTypeMap);
            _inboundClaimFilter = new HashSet<string>(DefaultInboundClaimFilter);
            _outboundAlgorithmMap = new Dictionary<string, string>(DefaultOutboundAlgorithmMap);
        }

        /// <summary>
        /// Gets or sets the <see cref="MapInboundClaims"/> property which is used when determining whether or not to map claim types that are extracted when validating a <see cref="JwtSecurityToken"/>. 
        /// <para>If this is set to true, the <see cref="Claim.Type"/> is set to the JSON claim 'name' after translating using this mapping. Otherwise, no mapping occurs.</para>
        /// <para>The default value is true.</para>
        /// </summary>
        public bool MapInboundClaims
        {
            get
            {
                return _mapInboundClaims;
            }

            set
            {
                // If the inbound claim type mapping was turned off and is being turned on for the first time, make sure that the _inboundClaimTypeMap is populated with the default mappings.
                if (!_mapInboundClaims && value && _inboundClaimTypeMap.Count == 0)
                    _inboundClaimTypeMap = new Dictionary<string, string>(DefaultInboundClaimTypeMap);

                _mapInboundClaims = value;            
            }
        } 

        /// <summary>
        /// Gets or sets the <see cref="InboundClaimTypeMap"/> which is used when setting the <see cref="Claim.Type"/> for claims in the <see cref="ClaimsPrincipal"/> extracted when validating a <see cref="JwtSecurityToken"/>. 
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

        /// <summary>
        /// <para>Gets or sets the <see cref="OutboundClaimTypeMap"/> which is used when creating a <see cref="JwtSecurityToken"/> from <see cref="Claim"/>(s).</para>
        /// <para>The JSON claim 'name' value is set to <see cref="Claim.Type"/> after translating using this mapping.</para>
        /// <para>The default value is ClaimTypeMapping.OutboundClaimTypeMap</para>
        /// </summary>
        /// <remarks>This mapping is applied only when using <see cref="JwtPayload.AddClaim"/> or <see cref="JwtPayload.AddClaims"/>. Adding values directly will not result in translation.</remarks>
        /// <exception cref="ArgumentNullException">'value' is null.</exception>
        public IDictionary<string, string> OutboundClaimTypeMap
        {
            get
            {
                return _outboundClaimTypeMap;
            }

            set
            {
                if (value == null)
                    throw LogHelper.LogArgumentNullException(nameof(value));

                _outboundClaimTypeMap = value;
            }
        }

        /// <summary>
        /// Gets the outbound algorithm map that is passed to the <see cref="JwtHeader"/> constructor.
        /// </summary>
        public IDictionary<string, string> OutboundAlgorithmMap
        {
            get
            {
                return _outboundAlgorithmMap;
            }
        }


        /// <summary>Gets or sets the <see cref="ISet{String}"/> used to filter claims when populating a <see cref="ClaimsIdentity"/> claims form a <see cref="JwtSecurityToken"/>.
        /// When a <see cref="JwtSecurityToken"/> is validated, claims with types found in this <see cref="ISet{String}"/> will not be added to the <see cref="ClaimsIdentity"/>.
        /// <para>The default value is ClaimTypeMapping.InboundClaimFilter.</para>
        /// </summary>
        /// <exception cref="ArgumentNullException">'value' is null.</exception>
        public ISet<string> InboundClaimFilter
        {
            get
            {
                return _inboundClaimFilter;
            }

            set
            {
                if (value == null)
                    throw LogHelper.LogArgumentNullException(nameof(value));

                _inboundClaimFilter = value;
            }
        }

        /// <summary>
        /// Gets or sets the property name of <see cref="Claim.Properties"/> the will contain the original JSON claim 'name' if a mapping occurred when the <see cref="Claim"/>(s) were created.
        /// <para>See <seealso cref="InboundClaimTypeMap"/> for more information.</para>
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
        /// Gets or sets the property name of <see cref="Claim.Properties"/> the will contain .Net type that was recognized when <see cref="JwtPayload.Claims"/> serialized the value to JSON.
        /// <para>See <seealso cref="InboundClaimTypeMap"/> for more information.</para>
        /// </summary>
        /// <exception cref="ArgumentException">If <see cref="string"/>.IsNullOrWhiteSpace('value') is true.</exception>
        public static string JsonClaimTypeProperty
        {
            get
            {
                return _jsonClaimType;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw LogHelper.LogArgumentNullException(nameof(value));

                _jsonClaimType = value;
            }
        }

        /// <summary>
        /// Returns a value that indicates if this handler can validate a <see cref="SecurityToken"/>.
        /// </summary>
        /// <returns>'true', indicating this instance can validate a <see cref="JwtSecurityToken"/>.</returns>
        public override bool CanValidateToken
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the value that indicates if this instance can write a <see cref="SecurityToken"/>.
        /// </summary>
        /// <returns>'true', indicating this instance can write a <see cref="JwtSecurityToken"/>.</returns>
        public override bool CanWriteToken
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the type of the <see cref="JwtSecurityToken"/>.
        /// </summary>
        /// <return>The type of <see cref="JwtSecurityToken"/></return>
        public override Type TokenType
        {
            get { return typeof(JwtSecurityToken); }
        }

        /// <summary>
        /// Determines if the string is a well formed Json Web Token (JWT).
        /// <para>See: https://datatracker.ietf.org/doc/html/rfc7519 </para>
        /// </summary>
        /// <param name="token">String that should represent a valid JWT.</param>
        /// <remarks>Uses <see cref="Regex.IsMatch(string, string)"/> matching one of:
        /// <para>JWS: @"^[A-Za-z0-9-_]+\.[A-Za-z0-9-_]+\.[A-Za-z0-9-_]*$"</para>
        /// <para>JWE: (dir): @"^[A-Za-z0-9-_]+\.\.[A-Za-z0-9-_]+\.[A-Za-z0-9-_]+\.[A-Za-z0-9-_]*$"</para>
        /// <para>JWE: (wrappedkey): @"^[A-Za-z0-9-_]+\.[A-Za-z0-9-_]+\.[A-Za-z0-9-_]+\.[A-Za-z0-9-_]+\.[A-Za-z0-9-_]$"</para>
        /// </remarks>
        /// <returns>
        /// <para>'false' if the token is null or whitespace.</para>
        /// <para>'false' if token.Length is greater than <see cref="TokenHandler.MaximumTokenSizeInBytes"/>.</para>
        /// <para>'true' if the token is in JSON compact serialization format.</para>
        /// </returns>
        public override bool CanReadToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            if (token.Length > MaximumTokenSizeInBytes)
            {
                if (LogHelper.IsEnabled(EventLogLevel.Informational))
                    LogHelper.LogInformation(TokenLogMessages.IDX10209, LogHelper.MarkAsNonPII(token.Length), LogHelper.MarkAsNonPII(MaximumTokenSizeInBytes));

                return false;
            }

            // Set the maximum number of segments to MaxJwtSegmentCount + 1. This controls the number of splits and allows detecting the number of segments is too large.
            // For example: "a.b.c.d.e.f.g.h" => [a], [b], [c], [d], [e], [f.g.h]. 6 segments.
            // If just MaxJwtSegmentCount was used, then [a], [b], [c], [d], [e.f.g.h] would be returned. 5 segments.
            int tokenPartCount = JwtTokenUtilities.CountJwtTokenPart(token, JwtConstants.MaxJwtSegmentCount + 1);
            if (tokenPartCount == JwtConstants.JwsSegmentCount)
            {
                return JwtTokenUtilities.RegexJws.IsMatch(token);
            }
            else if (tokenPartCount == JwtConstants.JweSegmentCount)
            {
                return JwtTokenUtilities.RegexJwe.IsMatch(token);
            }

            LogHelper.LogInformation(LogMessages.IDX12720);
            return false;
        }

        /// <summary>
        /// Returns a Json Web Token (JWT).
        /// </summary>
        /// <param name="tokenDescriptor">A <see cref="SecurityTokenDescriptor"/> that contains details of contents of the token.</param>
        /// <remarks>A JWS and JWE can be returned.
        /// <para>If <see cref="SecurityTokenDescriptor.EncryptingCredentials"/>is provided, then a JWE will be created.</para>
        /// <para>If <see cref="SecurityTokenDescriptor.SigningCredentials"/> is provided then a JWS will be created.</para>
        /// <para>If both are provided then a JWE with an embedded JWS will be created.</para>
        /// </remarks>
        public virtual string CreateEncodedJwt(SecurityTokenDescriptor tokenDescriptor)
        {
            if (tokenDescriptor == null)
                throw LogHelper.LogArgumentNullException(nameof(tokenDescriptor));

            return CreateJwtSecurityToken(tokenDescriptor).RawData;
        }

        /// <summary>
        /// Creates a JWT in 'Compact Serialization Format'.
        /// </summary>
        /// <param name="issuer">The issuer of the token.</param>
        /// <param name="audience">The audience for this token.</param>
        /// <param name="subject">The source of the <see cref="Claim"/>(s) for this token.</param>
        /// <param name="notBefore">The notbefore time for this token.</param>
        /// <param name="expires">The expiration time for this token.</param>
        /// <param name="issuedAt">The issue time for this token.</param>
        /// <param name="signingCredentials">Contains cryptographic material for generating a signature.</param>
        /// <remarks>If <see cref="ClaimsIdentity.Actor"/> is not null, then a claim { actort, 'value' } will be added to the payload. See <see cref="CreateActorValue"/> for details on how the value is created.
        /// <para>See <seealso cref="JwtHeader"/> for details on how the HeaderParameters are added to the header.</para>
        /// <para>See <seealso cref="JwtPayload"/> for details on how the values are added to the payload.</para>
        /// <para>Each <see cref="Claim"/> in the <paramref name="subject"/> will map <see cref="Claim.Type"/> by applying <see cref="OutboundClaimTypeMap"/>. Modifying <see cref="OutboundClaimTypeMap"/> could change the outbound JWT.</para>
        /// <para>If <see cref="SigningCredentials"/> is provided, then a JWS will be created.</para>
        /// </remarks>
        /// <returns>A Base64UrlEncoded string in 'Compact Serialization Format'.</returns>
        public virtual string CreateEncodedJwt(
            string issuer,
            string audience,
            ClaimsIdentity subject,
            DateTime? notBefore,
            DateTime? expires,
            DateTime? issuedAt,
            SigningCredentials signingCredentials)
        {
            return CreateJwtSecurityTokenPrivate(
                issuer,
                audience,
                subject,
                notBefore,
                expires,
                issuedAt,
                signingCredentials,
                null, null, null, null, null).RawData;
        }

        /// <summary>
        /// Creates a JWT in 'Compact Serialization Format'.
        /// </summary>
        /// <param name="issuer">The issuer of the token.</param>
        /// <param name="audience">The audience for this token.</param>
        /// <param name="subject">The source of the <see cref="Claim"/>(s) for this token.</param>
        /// <param name="notBefore">Translated into 'epoch time' and assigned to 'nbf'.</param>
        /// <param name="expires">Translated into 'epoch time' and assigned to 'exp'.</param>
        /// <param name="issuedAt">Translated into 'epoch time' and assigned to 'iat'.</param>
        /// <param name="signingCredentials">Contains cryptographic material for signing.</param>
        /// <param name="encryptingCredentials">Contains cryptographic material for encrypting.</param>
        /// <remarks>If <see cref="ClaimsIdentity.Actor"/> is not null, then a claim { actort, 'value' } will be added to the payload. <see cref="CreateActorValue"/> for details on how the value is created.
        /// <para>See <seealso cref="JwtHeader"/> for details on how the HeaderParameters are added to the header.</para>
        /// <para>See <seealso cref="JwtPayload"/> for details on how the values are added to the payload.</para>
        /// <para>Each <see cref="Claim"/> in the <paramref name="subject"/> will map <see cref="Claim.Type"/> by applying <see cref="OutboundClaimTypeMap"/>. Modifying <see cref="OutboundClaimTypeMap"/> could change the outbound JWT.</para>
        /// </remarks>
        /// <returns>A Base64UrlEncoded string in 'Compact Serialization Format'.</returns>
        /// <exception cref="ArgumentException">If 'expires' &lt;= 'notBefore'.</exception>
        public virtual string CreateEncodedJwt(
            string issuer,
            string audience,
            ClaimsIdentity subject,
            DateTime? notBefore,
            DateTime? expires,
            DateTime? issuedAt,
            SigningCredentials signingCredentials,
            EncryptingCredentials encryptingCredentials)
        {
            return CreateJwtSecurityTokenPrivate(
                issuer,
                audience,
                subject,
                notBefore,
                expires,
                issuedAt,
                signingCredentials,
                encryptingCredentials, null, null, null, null).RawData;
        }

        /// <summary>
        /// Creates a JWT in 'Compact Serialization Format'.
        /// </summary>
        /// <param name="issuer">The issuer of the token.</param>
        /// <param name="audience">The audience for this token.</param>
        /// <param name="subject">The source of the <see cref="Claim"/>(s) for this token.</param>
        /// <param name="notBefore">Translated into 'epoch time' and assigned to 'nbf'.</param>
        /// <param name="expires">Translated into 'epoch time' and assigned to 'exp'.</param>
        /// <param name="issuedAt">Translated into 'epoch time' and assigned to 'iat'.</param>
        /// <param name="signingCredentials">Contains cryptographic material for signing.</param>
        /// <param name="encryptingCredentials">Contains cryptographic material for encrypting.</param>
        /// <param name="claimCollection">A collection of (key,value) pairs representing <see cref="Claim"/>(s) for this token.</param>
        /// <remarks>If <see cref="ClaimsIdentity.Actor"/> is not null, then a claim { actort, 'value' } will be added to the payload. <see cref="CreateActorValue"/> for details on how the value is created.
        /// <para>See <seealso cref="JwtHeader"/> for details on how the HeaderParameters are added to the header.</para>
        /// <para>See <seealso cref="JwtPayload"/> for details on how the values are added to the payload.</para>
        /// <para>Each <see cref="Claim"/> in the <paramref name="subject"/> will map <see cref="Claim.Type"/> by applying <see cref="OutboundClaimTypeMap"/>. Modifying <see cref="OutboundClaimTypeMap"/> could change the outbound JWT.</para>
        /// </remarks>
        /// <returns>A Base64UrlEncoded string in 'Compact Serialization Format'.</returns>
        /// <exception cref="ArgumentException">If 'expires' &lt;= 'notBefore'.</exception>
        public virtual string CreateEncodedJwt(
            string issuer,
            string audience,
            ClaimsIdentity subject,
            DateTime? notBefore,
            DateTime? expires,
            DateTime? issuedAt,
            SigningCredentials signingCredentials,
            EncryptingCredentials encryptingCredentials,
            IDictionary<string, object> claimCollection)
        {
            return CreateJwtSecurityTokenPrivate(
                issuer,
                audience,
                subject,
                notBefore,
                expires,
                issuedAt,
                signingCredentials,
                encryptingCredentials,
                claimCollection, null, null, null).RawData;
        }

        /// <summary>
        /// Creates a Json Web Token (JWT).
        /// </summary>
        /// <param name="tokenDescriptor"> A <see cref="SecurityTokenDescriptor"/> that contains details of contents of the token.</param>
        /// <remarks><see cref="SecurityTokenDescriptor.SigningCredentials"/> is used to sign <see cref="JwtSecurityToken.RawData"/>.</remarks>
        public virtual JwtSecurityToken CreateJwtSecurityToken(SecurityTokenDescriptor tokenDescriptor)
        {
            if (tokenDescriptor == null)
                throw LogHelper.LogArgumentNullException(nameof(tokenDescriptor));

            return CreateJwtSecurityTokenPrivate(
                tokenDescriptor.Issuer,
                tokenDescriptor.Audience,
                tokenDescriptor.Subject,
                tokenDescriptor.NotBefore,
                tokenDescriptor.Expires,
                tokenDescriptor.IssuedAt,
                tokenDescriptor.SigningCredentials,
                tokenDescriptor.EncryptingCredentials,
                tokenDescriptor.Claims,
                tokenDescriptor.TokenType,
                tokenDescriptor.AdditionalHeaderClaims,
                tokenDescriptor.AdditionalInnerHeaderClaims);
        }

        /// <summary>
        /// Creates a <see cref="JwtSecurityToken"/>
        /// </summary>
        /// <param name="issuer">The issuer of the token.</param>
        /// <param name="audience">The audience for this token.</param>
        /// <param name="subject">The source of the <see cref="Claim"/>(s) for this token.</param>
        /// <param name="notBefore">The notbefore time for this token.</param>
        /// <param name="expires">The expiration time for this token.</param>
        /// <param name="issuedAt">The issue time for this token.</param>
        /// <param name="signingCredentials">Contains cryptographic material for generating a signature.</param>
        /// <param name="encryptingCredentials">Contains cryptographic material for encrypting the token.</param>
        /// <remarks>If <see cref="ClaimsIdentity.Actor"/> is not null, then a claim { actort, 'value' } will be added to the payload. <see cref="CreateActorValue"/> for details on how the value is created.
        /// <para>See <seealso cref="JwtHeader"/> for details on how the HeaderParameters are added to the header.</para>
        /// <para>See <seealso cref="JwtPayload"/> for details on how the values are added to the payload.</para>
        /// <para>Each <see cref="Claim"/> on the <paramref name="subject"/> added will have <see cref="Claim.Type"/> translated according to the mapping found in
        /// <see cref="OutboundClaimTypeMap"/>. Adding and removing to <see cref="OutboundClaimTypeMap"/> will affect the name component of the Json claim.</para>
        /// <para><see cref="SigningCredentials.SigningCredentials(SecurityKey, string)"/> is used to sign <see cref="JwtSecurityToken.RawData"/>.</para>
        /// <para><see cref="EncryptingCredentials.EncryptingCredentials(SecurityKey, string, string)"/> is used to encrypt <see cref="JwtSecurityToken.RawData"/> or <see cref="JwtSecurityToken.RawPayload"/> .</para>
        /// </remarks>
        /// <returns>A <see cref="JwtSecurityToken"/>.</returns>
        /// <exception cref="ArgumentException">If <paramref name="expires"/> &lt;= <paramref name="notBefore"/>.</exception>
        public virtual JwtSecurityToken CreateJwtSecurityToken(
            string issuer,
            string audience,
            ClaimsIdentity subject,
            DateTime? notBefore,
            DateTime? expires,
            DateTime? issuedAt,
            SigningCredentials signingCredentials,
            EncryptingCredentials encryptingCredentials)
        {
            return CreateJwtSecurityTokenPrivate(
                issuer,
                audience,
                subject,
                notBefore,
                expires,
                issuedAt,
                signingCredentials,
                encryptingCredentials, null, null, null, null);
        }

        /// <summary>
        /// Creates a <see cref="JwtSecurityToken"/>
        /// </summary>
        /// <param name="issuer">The issuer of the token.</param>
        /// <param name="audience">The audience for this token.</param>
        /// <param name="subject">The source of the <see cref="Claim"/>(s) for this token.</param>
        /// <param name="notBefore">The notbefore time for this token.</param>
        /// <param name="expires">The expiration time for this token.</param>
        /// <param name="issuedAt">The issue time for this token.</param>
        /// <param name="signingCredentials">Contains cryptographic material for generating a signature.</param>
        /// <param name="encryptingCredentials">Contains cryptographic material for encrypting the token.</param>
        /// <param name="claimCollection">A collection of (key,value) pairs representing <see cref="Claim"/>(s) for this token.</param>
        /// <remarks>If <see cref="ClaimsIdentity.Actor"/> is not null, then a claim { actort, 'value' } will be added to the payload. <see cref="CreateActorValue"/> for details on how the value is created.
        /// <para>See <seealso cref="JwtHeader"/> for details on how the HeaderParameters are added to the header.</para>
        /// <para>See <seealso cref="JwtPayload"/> for details on how the values are added to the payload.</para>
        /// <para>Each <see cref="Claim"/> on the <paramref name="subject"/> added will have <see cref="Claim.Type"/> translated according to the mapping found in
        /// <see cref="OutboundClaimTypeMap"/>. Adding and removing to <see cref="OutboundClaimTypeMap"/> will affect the name component of the Json claim.</para>
        /// <para><see cref="SigningCredentials.SigningCredentials(SecurityKey, string)"/> is used to sign <see cref="JwtSecurityToken.RawData"/>.</para>
        /// <para><see cref="EncryptingCredentials.EncryptingCredentials(SecurityKey, string, string)"/> is used to encrypt <see cref="JwtSecurityToken.RawData"/> or <see cref="JwtSecurityToken.RawPayload"/> .</para>
        /// </remarks>
        /// <returns>A <see cref="JwtSecurityToken"/>.</returns>
        /// <exception cref="ArgumentException">If <paramref name="expires"/> &lt;= <paramref name="notBefore"/>.</exception>
        public virtual JwtSecurityToken CreateJwtSecurityToken(
            string issuer,
            string audience,
            ClaimsIdentity subject,
            DateTime? notBefore,
            DateTime? expires,
            DateTime? issuedAt,
            SigningCredentials signingCredentials,
            EncryptingCredentials encryptingCredentials,
            IDictionary<string, object> claimCollection)
        {
            return CreateJwtSecurityTokenPrivate(
                issuer,
                audience,
                subject,
                notBefore,
                expires,
                issuedAt,
                signingCredentials,
                encryptingCredentials,
                claimCollection, null, null, null);
        }

        /// <summary>
        /// Creates a <see cref="JwtSecurityToken"/>
        /// </summary>
        /// <param name="issuer">The issuer of the token.</param>
        /// <param name="audience">The audience for this token.</param>
        /// <param name="subject">The source of the <see cref="Claim"/>(s) for this token.</param>
        /// <param name="notBefore">The notbefore time for this token.</param>
        /// <param name="expires">The expiration time for this token.</param>
        /// <param name="issuedAt">The issue time for this token.</param>
        /// <param name="signingCredentials">Contains cryptographic material for generating a signature.</param>
        /// <remarks>If <see cref="ClaimsIdentity.Actor"/> is not null, then a claim { actort, 'value' } will be added to the payload. <see cref="CreateActorValue"/> for details on how the value is created.
        /// <para>See <seealso cref="JwtHeader"/> for details on how the HeaderParameters are added to the header.</para>
        /// <para>See <seealso cref="JwtPayload"/> for details on how the values are added to the payload.</para>
        /// <para>Each <see cref="Claim"/> on the <paramref name="subject"/> added will have <see cref="Claim.Type"/> translated according to the mapping found in
        /// <see cref="OutboundClaimTypeMap"/>. Adding and removing to <see cref="OutboundClaimTypeMap"/> will affect the name component of the Json claim.</para>
        /// <para><see cref="SigningCredentials.SigningCredentials(SecurityKey, string)"/> is used to sign <see cref="JwtSecurityToken.RawData"/>.</para>
        /// </remarks>
        /// <returns>A <see cref="JwtSecurityToken"/>.</returns>
        /// <exception cref="ArgumentException">If <paramref name="expires"/> &lt;= <paramref name="notBefore"/>.</exception>
        public virtual JwtSecurityToken CreateJwtSecurityToken(
            string issuer = null,
            string audience = null,
            ClaimsIdentity subject = null,
            DateTime? notBefore = null,
            DateTime? expires = null,
            DateTime? issuedAt = null,
            SigningCredentials signingCredentials = null)
        {
            return CreateJwtSecurityTokenPrivate(
                issuer,
                audience,
                subject,
                notBefore,
                expires,
                issuedAt,
                signingCredentials, null, null, null, null, null);
        }

        /// <summary>
        /// Creates a Json Web Token (JWT).
        /// </summary>
        /// <param name="tokenDescriptor"> A <see cref="SecurityTokenDescriptor"/> that contains details of contents of the token.</param>
        /// <remarks><see cref="SecurityTokenDescriptor.SigningCredentials"/> is used to sign <see cref="JwtSecurityToken.RawData"/>.</remarks>
        public override SecurityToken CreateToken(SecurityTokenDescriptor tokenDescriptor)
        {
            if (tokenDescriptor == null)
                throw LogHelper.LogArgumentNullException(nameof(tokenDescriptor));

            return CreateJwtSecurityTokenPrivate(
                tokenDescriptor.Issuer,
                tokenDescriptor.Audience,
                tokenDescriptor.Subject,
                tokenDescriptor.NotBefore,
                tokenDescriptor.Expires,
                tokenDescriptor.IssuedAt,
                tokenDescriptor.SigningCredentials,
                tokenDescriptor.EncryptingCredentials,
                tokenDescriptor.Claims,
                tokenDescriptor.TokenType,
                tokenDescriptor.AdditionalHeaderClaims,
                tokenDescriptor.AdditionalInnerHeaderClaims);
        }

        private JwtSecurityToken CreateJwtSecurityTokenPrivate(
            string issuer,
            string audience,
            ClaimsIdentity subject,
            DateTime? notBefore,
            DateTime? expires,
            DateTime? issuedAt,
            SigningCredentials signingCredentials,
            EncryptingCredentials encryptingCredentials,
            IDictionary<string, object> claimCollection,
            string tokenType,
            IDictionary<string, object> additionalHeaderClaims,
            IDictionary<string, object> additionalInnerHeaderClaims)
        {
            if (SetDefaultTimesOnTokenCreation && (!expires.HasValue || !issuedAt.HasValue || !notBefore.HasValue))
            {
                DateTime now = DateTime.UtcNow;
                if (!expires.HasValue)
                    expires = now + TimeSpan.FromMinutes(TokenLifetimeInMinutes);

                if (!issuedAt.HasValue)
                    issuedAt = now;

                if (!notBefore.HasValue)
                    notBefore = now;
            }

            if (LogHelper.IsEnabled(EventLogLevel.Verbose))
                LogHelper.LogVerbose(LogMessages.IDX12721, LogHelper.MarkAsNonPII(issuer ?? "null"), LogHelper.MarkAsNonPII(audience ?? "null"));

            JwtPayload payload = new JwtPayload(issuer, audience, (subject == null ? null : OutboundClaimTypeTransform(subject.Claims)), (claimCollection == null ? null : OutboundClaimTypeTransform(claimCollection)), notBefore, expires, issuedAt);
            JwtHeader header = new JwtHeader(signingCredentials, OutboundAlgorithmMap, tokenType, additionalInnerHeaderClaims);

            if (subject?.Actor != null)
                payload.AddClaim(new Claim(JwtRegisteredClaimNames.Actort, CreateActorValue(subject.Actor)));

            string rawHeader = header.Base64UrlEncode();
            string rawPayload = payload.Base64UrlEncode();
            string rawSignature = string.Empty;
            if (signingCredentials != null)
            {
                string message = string.Concat(rawHeader, ".", rawPayload);
                rawSignature = JwtTokenUtilities.CreateEncodedSignature(message, signingCredentials);
            }

            if (LogHelper.IsEnabled(EventLogLevel.Informational))
                LogHelper.LogInformation(LogMessages.IDX12722, rawHeader, rawPayload);

            if (encryptingCredentials != null)
            {
                return EncryptToken(
                        new JwtSecurityToken(header, payload, rawHeader, rawPayload, rawSignature),
                        encryptingCredentials,
                        tokenType,
                        additionalHeaderClaims);
            }

            return new JwtSecurityToken(header, payload, rawHeader, rawPayload, rawSignature);
        }

        private JwtSecurityToken EncryptToken(
            JwtSecurityToken innerJwt,
            EncryptingCredentials encryptingCredentials,
            string tokenType,
            IDictionary<string, object> additionalHeaderClaims)
        {
            if (encryptingCredentials == null)
                throw LogHelper.LogArgumentNullException(nameof(encryptingCredentials));

            var cryptoProviderFactory = encryptingCredentials.CryptoProviderFactory ?? encryptingCredentials.Key.CryptoProviderFactory;

            if (cryptoProviderFactory == null)
                throw LogHelper.LogExceptionMessage(new ArgumentException(TokenLogMessages.IDX10620));

            SecurityKey securityKey = JwtTokenUtilities.GetSecurityKey(encryptingCredentials, cryptoProviderFactory, additionalHeaderClaims, out byte[] wrappedKey);
            using (AuthenticatedEncryptionProvider encryptionProvider = cryptoProviderFactory.CreateAuthenticatedEncryptionProvider(securityKey, encryptingCredentials.Enc))
            {
                if (encryptionProvider == null)
                    throw LogHelper.LogExceptionMessage(new SecurityTokenEncryptionFailedException(LogMessages.IDX12730));

                try
                {
                    var header = new JwtHeader(encryptingCredentials, OutboundAlgorithmMap, tokenType, additionalHeaderClaims);
                    var encodedHeader = header.Base64UrlEncode();
                    AuthenticatedEncryptionResult encryptionResult = encryptionProvider.Encrypt(Encoding.UTF8.GetBytes(innerJwt.RawData), Encoding.ASCII.GetBytes(encodedHeader));
                    return JwtConstants.DirectKeyUseAlg.Equals(encryptingCredentials.Alg) ?
                        new JwtSecurityToken(
                            header,
                            innerJwt,
                            encodedHeader,
                            string.Empty,
                            Base64UrlEncoder.Encode(encryptionResult.IV),
                            Base64UrlEncoder.Encode(encryptionResult.Ciphertext),
                            Base64UrlEncoder.Encode(encryptionResult.AuthenticationTag)) :
                        new JwtSecurityToken(
                            header,
                            innerJwt,
                            encodedHeader,
                            Base64UrlEncoder.Encode(wrappedKey),
                            Base64UrlEncoder.Encode(encryptionResult.IV),
                            Base64UrlEncoder.Encode(encryptionResult.Ciphertext),
                            Base64UrlEncoder.Encode(encryptionResult.AuthenticationTag));
                }
                catch (Exception ex)
                {
                    throw LogHelper.LogExceptionMessage(new SecurityTokenEncryptionFailedException(LogHelper.FormatInvariant(TokenLogMessages.IDX10616, LogHelper.MarkAsNonPII(encryptingCredentials.Enc), encryptingCredentials.Key), ex));
                }
            }
        }

        private IEnumerable<Claim> OutboundClaimTypeTransform(IEnumerable<Claim> claims)
        {
            foreach (Claim claim in claims)
            {
                string type = null;
                if (_outboundClaimTypeMap.TryGetValue(claim.Type, out type))
                {
                    yield return new Claim(type, claim.Value, claim.ValueType, claim.Issuer, claim.OriginalIssuer, claim.Subject);
                }
                else
                {
                    yield return claim;
                }
            }
        }

        private Dictionary<string, object> OutboundClaimTypeTransform(IDictionary<string, object> claimCollection)
        {
            var claims = new Dictionary<string, object>();

            foreach (string claimType in claimCollection.Keys)
            {
                if (_outboundClaimTypeMap.TryGetValue(claimType, out string type))
                    claims[type] = claimCollection[claimType];

                else
                    claims[claimType] = claimCollection[claimType];
            }

            return claims;
        }

        /// <summary>
        /// Converts a string into an instance of <see cref="JwtSecurityToken"/>.
        /// </summary>
        /// <param name="token">A 'JSON Web Token' (JWT) in JWS or JWE Compact Serialization Format.</param>
        /// <returns>A <see cref="JwtSecurityToken"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="token"/> is null or empty.</exception>
        /// <exception cref="ArgumentException">'token.Length' is greater than <see cref="TokenHandler.MaximumTokenSizeInBytes"/>.</exception>
        /// <exception cref="SecurityTokenMalformedException"><see cref="CanReadToken(string)"/></exception>
        /// <remarks><para>If the <paramref name="token"/> is in JWE Compact Serialization format, only the protected header will be deserialized.
        /// This method is unable to decrypt the payload. Use <see cref="ValidateToken(string, TokenValidationParameters, out SecurityToken)"/>to obtain the payload.</para>
        /// <para>The token is NOT validated and no security decisions should be made about the contents.
        /// Use <see cref="ValidateTokenAsync(string, TokenValidationParameters)"/> to ensure the token is acceptable.</para></remarks>
        public JwtSecurityToken ReadJwtToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw LogHelper.LogArgumentNullException(nameof(token));

            if (token.Length > MaximumTokenSizeInBytes)
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(TokenLogMessages.IDX10209, LogHelper.MarkAsNonPII(token.Length), LogHelper.MarkAsNonPII(MaximumTokenSizeInBytes))));

            if (!CanReadToken(token))
                throw LogHelper.LogExceptionMessage(new SecurityTokenMalformedException(LogMessages.IDX12709));

            var jwtToken = new JwtSecurityToken();
            jwtToken.Decode(token.Split('.'), token);
            return jwtToken;
        }

        /// <summary>
        /// Converts a string into an instance of <see cref="JwtSecurityToken"/>.
        /// </summary>
        /// <param name="token">A 'JSON Web Token' (JWT) in JWS or JWE Compact Serialization Format.</param>
        /// <returns>A <see cref="JwtSecurityToken"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="token"/> is null or empty.</exception>
        /// <exception cref="ArgumentException">'token.Length' is greater than <see cref="TokenHandler.MaximumTokenSizeInBytes"/>.</exception>
        /// <exception cref="ArgumentException"><see cref="CanReadToken(string)"/></exception>
        /// <remarks><para>If the <paramref name="token"/> is in JWE Compact Serialization format, only the protected header will be deserialized.</para>
        /// This method is unable to decrypt the payload. Use <see cref="ValidateToken(string, TokenValidationParameters, out SecurityToken)"/>to obtain the payload.</remarks>
        /// <remarks>The token is NOT validated and no security decisions should be made about the contents.
        /// <para>Use <see cref="ValidateTokenAsync(string, TokenValidationParameters)"/> to ensure the token is acceptable.</para></remarks>
        public override SecurityToken ReadToken(string token)
        {
            return ReadJwtToken(token);
        }
        
        /// <summary>
        /// Deserializes token with the provided <see cref="TokenValidationParameters"/>.
        /// </summary>
        /// <param name="reader"><see cref="XmlReader"/>.</param>
        /// <param name="validationParameters">The current <see cref="TokenValidationParameters"/>.</param>
        /// <returns>The <see cref="SecurityToken"/></returns>
        /// <remarks>This method is not current supported.</remarks>
        public override SecurityToken ReadToken(XmlReader reader, TokenValidationParameters validationParameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads and validates a 'JSON Web Token' (JWT) encoded as a JWS or JWE in Compact Serialized Format.
        /// </summary>
        /// <param name="token">the JWT encoded as JWE or JWS</param>
        /// <param name="validationParameters">Contains validation parameters for the <see cref="JwtSecurityToken"/>.</param>
        /// <param name="validatedToken">The <see cref="JwtSecurityToken"/> that was validated.</param>
        /// <exception cref="ArgumentNullException"><paramref name="token"/> is null or whitespace.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="validationParameters"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="token"/>.Length is greater than <see cref="TokenHandler.MaximumTokenSizeInBytes"/>.</exception>
        /// <exception cref="SecurityTokenMalformedException"><paramref name="token"/> does not have 3 or 5 parts.</exception>
        /// <exception cref="SecurityTokenMalformedException"><see cref="CanReadToken(string)"/> returns false.</exception>
        /// <exception cref="SecurityTokenDecryptionFailedException"><paramref name="token"/> was a JWE was not able to be decrypted.</exception>
        /// <exception cref="SecurityTokenEncryptionKeyNotFoundException"><paramref name="token"/> 'kid' header claim is not null AND decryption fails.</exception>
        /// <exception cref="SecurityTokenException"><paramref name="token"/> 'enc' header claim is null or empty.</exception>
        /// <exception cref="SecurityTokenExpiredException"><paramref name="token"/> 'exp' claim is &lt; DateTime.UtcNow.</exception>
        /// <exception cref="SecurityTokenInvalidAudienceException"><see cref="TokenValidationParameters.ValidAudience"/> is null or whitespace and <see cref="TokenValidationParameters.ValidAudiences"/> is null. Audience is not validated if <see cref="TokenValidationParameters.ValidateAudience"/> is set to false.</exception>
        /// <exception cref="SecurityTokenInvalidAudienceException"><paramref name="token"/> 'aud' claim did not match either <see cref="TokenValidationParameters.ValidAudience"/> or one of <see cref="TokenValidationParameters.ValidAudiences"/>.</exception>
        /// <exception cref="SecurityTokenInvalidLifetimeException"><paramref name="token"/> 'nbf' claim is &gt; 'exp' claim.</exception>
        /// <exception cref="SecurityTokenInvalidSignatureException"><paramref name="token"/>.signature is not properly formatted.</exception>
        /// <exception cref="SecurityTokenNoExpirationException"><paramref name="token"/> 'exp' claim is missing and <see cref="TokenValidationParameters.RequireExpirationTime"/> is true.</exception>
        /// <exception cref="SecurityTokenNoExpirationException"><see cref="TokenValidationParameters.TokenReplayCache"/> is not null and expirationTime.HasValue is false. When a TokenReplayCache is set, tokens require an expiration time.</exception>
        /// <exception cref="SecurityTokenNotYetValidException"><paramref name="token"/> 'nbf' claim is &gt; DateTime.UtcNow.</exception>
        /// <exception cref="SecurityTokenReplayAddFailedException"><paramref name="token"/> could not be added to the <see cref="TokenValidationParameters.TokenReplayCache"/>.</exception>
        /// <exception cref="SecurityTokenReplayDetectedException"><paramref name="token"/> is found in the cache.</exception>
        /// <returns> A <see cref="ClaimsPrincipal"/> from the JWT. Does not include claims found in the JWT header.</returns>
        /// <remarks> 
        /// Many of the exceptions listed above are not thrown directly from this method. See <see cref="Validators"/> to examine the call graph.
        /// </remarks>
        public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw LogHelper.LogArgumentNullException(nameof(token));

            if (validationParameters == null)
                throw LogHelper.LogArgumentNullException(nameof(validationParameters));

            if (token.Length > MaximumTokenSizeInBytes)
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(TokenLogMessages.IDX10209, LogHelper.MarkAsNonPII(token.Length), LogHelper.MarkAsNonPII(MaximumTokenSizeInBytes))));

            int tokenPartCount = JwtTokenUtilities.CountJwtTokenPart(token, JwtConstants.MaxJwtSegmentCount + 1);

            if (tokenPartCount != JwtConstants.JwsSegmentCount && tokenPartCount != JwtConstants.JweSegmentCount)
                throw LogHelper.LogExceptionMessage(new SecurityTokenMalformedException(LogMessages.IDX12741));

            if (tokenPartCount == JwtConstants.JweSegmentCount)
            {
                var jwtToken = ReadJwtToken(token);
                var decryptedJwt = DecryptToken(jwtToken, validationParameters);
                return ValidateToken(decryptedJwt, jwtToken, validationParameters, out validatedToken);
            }
            else
            {
                return ValidateToken(token, null, validationParameters, out validatedToken);
            }
        }

        /// <summary>
        ///  Private method for token validation, responsible for:
        ///  (1) Obtaining a configuration from the <see cref="TokenValidationParameters.ConfigurationManager"/>.
        ///  (2) Revalidating using the Last Known Good Configuration (if present), and obtaining a refreshed configuration (if necessary) and revalidating using it.
        /// </summary>
        /// <param name="token">The JWS string, or the decrypted token if the token is a JWE.</param>
        /// <param name="outerToken">If the token being validated is a JWE, this is the <see cref="JwtSecurityToken"/> that represents the outer token.
        ///  If the token is a JWS, the value of this parameter is <see langword="null" />.
        /// </param>
        /// <param name="validationParameters">The <see cref="TokenValidationParameters"/> to be used for validation.</param>
        /// <param name="signatureValidatedToken">The <see cref="JwtSecurityToken"/> that was validated.</param>
        /// <returns> A <see cref="ClaimsPrincipal"/> from the JWT. Does not include claims found in the JWT header.</returns>
        private ClaimsPrincipal ValidateToken(string token, JwtSecurityToken outerToken, TokenValidationParameters validationParameters, out SecurityToken signatureValidatedToken)
        {
            BaseConfiguration currentConfiguration = null;
            if (validationParameters.ConfigurationManager != null)
            {
                try
                {
                    currentConfiguration = validationParameters.ConfigurationManager.GetBaseConfigurationAsync(CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    // The exception is not re-thrown as the TokenValidationParameters may have the issuer and signing key set
                    // directly on them, allowing the library to continue with token validation.
                    if (LogHelper.IsEnabled(EventLogLevel.Warning))
                        LogHelper.LogWarning(LogHelper.FormatInvariant(TokenLogMessages.IDX10261, LogHelper.MarkAsNonPII(validationParameters.ConfigurationManager.MetadataAddress), ex.ToString()));
                }
            }

            ExceptionDispatchInfo exceptionThrown;
            ClaimsPrincipal claimsPrincipal = outerToken != null ? ValidateJWE(token, outerToken, validationParameters, currentConfiguration, out signatureValidatedToken, out exceptionThrown) :
                ValidateJWS(token, validationParameters, currentConfiguration, out signatureValidatedToken, out exceptionThrown);
            if (validationParameters.ConfigurationManager != null)
            {
                if (claimsPrincipal != null)
                {
                    // Set current configuration as LKG if it exists.
                    if (currentConfiguration != null)
                        validationParameters.ConfigurationManager.LastKnownGoodConfiguration = currentConfiguration;

                    return claimsPrincipal;
                }
                else if (TokenUtilities.IsRecoverableException(exceptionThrown.SourceException))
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
                        currentConfiguration = validationParameters.ConfigurationManager.GetBaseConfigurationAsync(CancellationToken.None).GetAwaiter().GetResult();

                        // Only try to re-validate using the newly obtained config if it doesn't reference equal the previously used configuration.
                        if (lastConfig != currentConfiguration)
                        {
                            claimsPrincipal = outerToken != null ? ValidateJWE(token, outerToken, validationParameters, currentConfiguration, out signatureValidatedToken, out exceptionThrown) :
                                ValidateJWS(token, validationParameters, currentConfiguration, out signatureValidatedToken, out exceptionThrown);

                            if (claimsPrincipal != null)
                            {
                                validationParameters.ConfigurationManager.LastKnownGoodConfiguration = currentConfiguration;
                                return claimsPrincipal;
                            }
                        }
                    }

                    if (validationParameters.ConfigurationManager.UseLastKnownGoodConfiguration)
                    {
                        validationParameters.RefreshBeforeValidation = false;
                        validationParameters.ValidateWithLKG = true;
                        var recoverableException = exceptionThrown.SourceException;
                        string kid = outerToken != null ? outerToken.Header.Kid :
                            (ValidateSignatureUsingDelegates(token, validationParameters, null) ?? GetJwtSecurityTokenFromToken(token, validationParameters)).Header.Kid;

                        foreach (BaseConfiguration lkgConfiguration in validationParameters.ConfigurationManager.GetValidLkgConfigurations())
                        {
                            if (!lkgConfiguration.Equals(currentConfiguration) && TokenUtilities.IsRecoverableConfiguration(kid, currentConfiguration, lkgConfiguration, recoverableException))
                            {
                                claimsPrincipal = outerToken != null ? ValidateJWE(token, outerToken, validationParameters, lkgConfiguration, out signatureValidatedToken, out exceptionThrown) :
                                    ValidateJWS(token, validationParameters, lkgConfiguration, out signatureValidatedToken, out exceptionThrown);

                                if (claimsPrincipal != null)
                                    return claimsPrincipal;
                            }
                        }
                    }
                }
            }

            if (claimsPrincipal != null)
                return claimsPrincipal;

            exceptionThrown.Throw();

            // This should be unreachable code, adding to make the complier happy.
            return null;
        }

        private ClaimsPrincipal ValidateJWE(
            string decryptedJwt,
            JwtSecurityToken outerToken,
            TokenValidationParameters validationParameters,
            BaseConfiguration currentConfiguration,
            out SecurityToken signatureValidatedToken,
            out ExceptionDispatchInfo exceptionThrown)
        {
            exceptionThrown = null;
            try
            {
                SecurityToken innerToken;
                ClaimsPrincipal claimsPrincipal = ValidateJWS(decryptedJwt, validationParameters, currentConfiguration, out innerToken, out exceptionThrown);
                outerToken.InnerToken = innerToken as JwtSecurityToken;
                signatureValidatedToken = exceptionThrown == null ? outerToken : null;
                return claimsPrincipal;
            }
            catch (Exception ex)
            {
                exceptionThrown = ExceptionDispatchInfo.Capture(ex);
                signatureValidatedToken = null;
                return null;
            }
        }

        private ClaimsPrincipal ValidateJWS(
            string token,
            TokenValidationParameters validationParameters,
            BaseConfiguration currentConfiguration,
            out SecurityToken signatureValidatedToken,
            out ExceptionDispatchInfo exceptionThrown)
        {
            exceptionThrown = null;
            try
            {
                ClaimsPrincipal claimsPrincipal;
                if (validationParameters.SignatureValidator != null || validationParameters.SignatureValidatorUsingConfiguration != null)
                {
                    signatureValidatedToken = ValidateSignatureUsingDelegates(token, validationParameters, currentConfiguration);
                    claimsPrincipal = ValidateTokenPayload(signatureValidatedToken as JwtSecurityToken, validationParameters, currentConfiguration);

                    // use protected virtual method that does not take in configuration for back compatibility purposes
                    if (currentConfiguration == null)
                        ValidateIssuerSecurityKey(signatureValidatedToken.SigningKey, signatureValidatedToken as JwtSecurityToken, validationParameters);
                    else
                        Validators.ValidateIssuerSecurityKey(signatureValidatedToken.SigningKey, signatureValidatedToken, validationParameters, currentConfiguration);
                }
                else
                {
                    JwtSecurityToken jwtToken = GetJwtSecurityTokenFromToken(token, validationParameters);

                    if (validationParameters.ValidateSignatureLast)
                    {
                        claimsPrincipal = ValidateTokenPayload(jwtToken, validationParameters, currentConfiguration);
                        jwtToken = ValidateSignatureAndIssuerSecurityKey(token, jwtToken, validationParameters, currentConfiguration);
                        signatureValidatedToken = jwtToken;
                    }
                    else
                    {
                        signatureValidatedToken = ValidateSignatureAndIssuerSecurityKey(token, jwtToken, validationParameters, currentConfiguration);
                        claimsPrincipal = ValidateTokenPayload(
                             signatureValidatedToken as JwtSecurityToken,
                             validationParameters,
                             currentConfiguration);
                    }
                }

                return claimsPrincipal;
            }
            catch (Exception ex)
            {
                exceptionThrown = ExceptionDispatchInfo.Capture(ex);
                signatureValidatedToken = null;
                return null;
            }
        }

        private static JwtSecurityToken ValidateSignatureUsingDelegates(string token, TokenValidationParameters validationParameters, BaseConfiguration configuration)
        {
            _ = validationParameters ?? throw LogHelper.LogArgumentNullException(nameof(validationParameters));

            if (validationParameters.SignatureValidatorUsingConfiguration != null)
            {
                var validatedJwtToken = validationParameters.SignatureValidatorUsingConfiguration(token, validationParameters, configuration);
                if (validatedJwtToken == null)
                    throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant(TokenLogMessages.IDX10505, LogHelper.MarkAsSecurityArtifact(token, JwtTokenUtilities.SafeLogJwtToken))));

                if (!(validatedJwtToken is JwtSecurityToken validatedJwt))
                    throw LogHelper.LogExceptionMessage(
                        new SecurityTokenInvalidSignatureException(
                            LogHelper.FormatInvariant(
                                TokenLogMessages.IDX10506,
                                LogHelper.MarkAsNonPII(typeof(JwtSecurityToken)),
                                LogHelper.MarkAsNonPII(validatedJwtToken.GetType()),
                                LogHelper.MarkAsSecurityArtifact(token, JwtTokenUtilities.SafeLogJwtToken))));

                return validatedJwt;
            }
            else if (validationParameters.SignatureValidator != null)
            {
                var validatedJwtToken = validationParameters.SignatureValidator(token, validationParameters);
                if (validatedJwtToken == null)
                    throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant(TokenLogMessages.IDX10505, LogHelper.MarkAsSecurityArtifact(token, JwtTokenUtilities.SafeLogJwtToken))));

                if (!(validatedJwtToken is JwtSecurityToken validatedJwt))
                    throw LogHelper.LogExceptionMessage(
                        new SecurityTokenInvalidSignatureException(
                            LogHelper.FormatInvariant(
                                TokenLogMessages.IDX10506,
                                LogHelper.MarkAsNonPII(typeof(JwtSecurityToken)),
                                LogHelper.MarkAsNonPII(validatedJwtToken.GetType()),
                                LogHelper.MarkAsSecurityArtifact(token, JwtTokenUtilities.SafeLogJwtToken))));

                return validatedJwt;
            }

            return null;
        }

        private JwtSecurityToken ValidateSignatureAndIssuerSecurityKey(string token, JwtSecurityToken jwtToken, TokenValidationParameters validationParameters, BaseConfiguration configuration)
        {
            JwtSecurityToken validatedToken = ValidateSignature(token, jwtToken, validationParameters, configuration);

            // use protected virtual method that does not take in configuration for back compatibility purposes
            if (configuration == null)
                ValidateIssuerSecurityKey(jwtToken.SigningKey, jwtToken, validationParameters);
            else
                Validators.ValidateIssuerSecurityKey(jwtToken.SigningKey, jwtToken, validationParameters, configuration);

            return validatedToken;
        }

        private JwtSecurityToken GetJwtSecurityTokenFromToken(string token, TokenValidationParameters validationParameters)
        {
            if (validationParameters.TokenReader != null)
            {
                var securityToken = validationParameters.TokenReader(token, validationParameters);
                if (securityToken == null)
                    throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant(TokenLogMessages.IDX10510, LogHelper.MarkAsSecurityArtifact(token, JwtTokenUtilities.SafeLogJwtToken))));

                if (!(securityToken is JwtSecurityToken jwtToken))
                    throw LogHelper.LogExceptionMessage(
                        new SecurityTokenInvalidSignatureException(
                            LogHelper.FormatInvariant(
                                TokenLogMessages.IDX10509,
                                LogHelper.MarkAsNonPII(typeof(JsonWebToken)),
                                LogHelper.MarkAsNonPII(securityToken.GetType()),
                                LogHelper.MarkAsSecurityArtifact(token, JwtTokenUtilities.SafeLogJwtToken))));

                return jwtToken;
            }
            else
            {
                return ReadJwtToken(token);
            }
        }

        /// <summary>
        /// Validates the JSON payload of a <see cref="JwtSecurityToken"/>.
        /// </summary>
        /// <param name="jwtToken">The token to validate.</param>
        /// <param name="validationParameters">Contains validation parameters for the <see cref="JwtSecurityToken"/>.</param>
        /// <returns>A <see cref="ClaimsPrincipal"/> from the jwt. Does not include the header claims.</returns>
        protected ClaimsPrincipal ValidateTokenPayload(JwtSecurityToken jwtToken, TokenValidationParameters validationParameters)
        {
            return ValidateTokenPayload(jwtToken, validationParameters, null);
        }

        private ClaimsPrincipal ValidateTokenPayload(JwtSecurityToken jwtToken, TokenValidationParameters validationParameters, BaseConfiguration configuration)
        {
            if (jwtToken is null)
                throw LogHelper.LogArgumentNullException(nameof(jwtToken));

            if (validationParameters is null)
                throw LogHelper.LogArgumentNullException(nameof(validationParameters));

            DateTime? expires = (jwtToken.Payload.Expiration == null) ? null : new DateTime?(jwtToken.ValidTo);
            DateTime? notBefore = (jwtToken.Payload.NotBefore == null) ? null : new DateTime?(jwtToken.ValidFrom);

            ValidateLifetime(notBefore, expires, jwtToken, validationParameters);
            ValidateAudience(jwtToken.Audiences, jwtToken, validationParameters);

            // use protected virtual method that does not take in configuration for back compatibility purposes
            string issuer = configuration == null ? ValidateIssuer(jwtToken.Issuer, jwtToken, validationParameters) :
                Validators.ValidateIssuer(jwtToken.Issuer, jwtToken, validationParameters, configuration);

            ValidateTokenReplay(expires, jwtToken.RawData, validationParameters);
            if (validationParameters.ValidateActor && !string.IsNullOrWhiteSpace(jwtToken.Actor))
            {
                ValidateToken(jwtToken.Actor, validationParameters.ActorValidationParameters ?? validationParameters, out _);
            }

            Validators.ValidateTokenType(jwtToken.Header.Typ, jwtToken, validationParameters);

            var identity = CreateClaimsIdentity(jwtToken, issuer, validationParameters);
            if (validationParameters.SaveSigninToken)
                identity.BootstrapContext = jwtToken.RawData;

            if (LogHelper.IsEnabled(EventLogLevel.Informational))
                LogHelper.LogInformation(TokenLogMessages.IDX10241, jwtToken);

            return new ClaimsPrincipal(identity);
        }

        private ClaimsPrincipal CreateClaimsPrincipalFromToken(JwtSecurityToken jwtToken, string issuer, TokenValidationParameters validationParameters)
        {
            var identity = CreateClaimsIdentity(jwtToken, issuer, validationParameters);
            if (validationParameters.SaveSigninToken)
                identity.BootstrapContext = jwtToken.RawData;

            if (LogHelper.IsEnabled(EventLogLevel.Informational))
                LogHelper.LogInformation(TokenLogMessages.IDX10241, jwtToken);

            return new ClaimsPrincipal(identity);
        }

        /// <summary>
        /// Serializes a <see cref="JwtSecurityToken"/> into a JWT in Compact Serialization Format.
        /// </summary>
        /// <param name="token"><see cref="JwtSecurityToken"/> to serialize.</param>
        /// <remarks>
        /// <para>The JWT will be serialized as a JWE or JWS.</para>
        /// <para><see cref="JwtSecurityToken.Payload"/> will be used to create the JWT. If there is an inner token, the inner token's payload will be used.</para>
        /// <para>If either <see cref="JwtSecurityToken.SigningCredentials"/> or <see cref="JwtSecurityToken.InnerToken"/>.SigningCredentials are set, the JWT will be signed.</para>
        /// <para>If <see cref="JwtSecurityToken.EncryptingCredentials"/> is set, a JWE will be created using the JWT above as the plaintext.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="token"/> is null.</exception>
        /// <exception cref="ArgumentException">'token' is not a not <see cref="JwtSecurityToken"/>.</exception>
        /// <exception cref="SecurityTokenEncryptionFailedException">both <see cref="JwtSecurityToken.SigningCredentials"/> and <see cref="JwtSecurityToken.InnerToken"/> are set.</exception>
        /// <exception cref="SecurityTokenEncryptionFailedException">both <see cref="JwtSecurityToken.InnerToken"/> and <see cref="JwtSecurityToken.InnerToken"/>.EncryptingCredentials are set.</exception>
        /// <exception cref="SecurityTokenEncryptionFailedException">if <see cref="JwtSecurityToken.InnerToken"/> is set and <see cref="JwtSecurityToken.EncryptingCredentials"/> is not set.</exception>
        /// <returns>A JWE or JWS in 'Compact Serialization Format'.</returns>
        public override string WriteToken(SecurityToken token)
        {
            if (token == null)
                throw LogHelper.LogArgumentNullException(nameof(token));

            JwtSecurityToken jwtToken = token as JwtSecurityToken;
            if (jwtToken == null)
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX12706, LogHelper.MarkAsNonPII(_className), LogHelper.MarkAsNonPII(typeof(JwtSecurityToken)), LogHelper.MarkAsNonPII(token.GetType())), nameof(token)));

            var encodedPayload = jwtToken.EncodedPayload;
            var encodedSignature = string.Empty;
            var encodedHeader = string.Empty;
            if (jwtToken.InnerToken != null)
            {
                if (jwtToken.SigningCredentials != null)
                    throw LogHelper.LogExceptionMessage(new SecurityTokenEncryptionFailedException(LogMessages.IDX12736));

                if (jwtToken.InnerToken.Header.EncryptingCredentials != null)
                    throw LogHelper.LogExceptionMessage(new SecurityTokenEncryptionFailedException(LogMessages.IDX12737));

                if (jwtToken.Header.EncryptingCredentials == null)
                    throw LogHelper.LogExceptionMessage(new SecurityTokenEncryptionFailedException(LogMessages.IDX12735));

                if (jwtToken.InnerToken.SigningCredentials != null)
                    encodedSignature = JwtTokenUtilities.CreateEncodedSignature(string.Concat(jwtToken.InnerToken.EncodedHeader, ".", jwtToken.EncodedPayload), jwtToken.InnerToken.SigningCredentials);

                return EncryptToken(
                    new JwtSecurityToken(
                        jwtToken.InnerToken.Header,
                        jwtToken.InnerToken.Payload,
                        jwtToken.InnerToken.EncodedHeader,
                        encodedPayload, encodedSignature),
                    jwtToken.EncryptingCredentials,
                    jwtToken.InnerToken.Header.Typ,
                    null).RawData;
            }

            // if EncryptingCredentials isn't set, then we need to create JWE
            // first create a new header with the SigningCredentials, Create a JWS then wrap it in a JWE
            var header = jwtToken.EncryptingCredentials == null ? jwtToken.Header : new JwtHeader(jwtToken.SigningCredentials);
            encodedHeader = header.Base64UrlEncode();
            if (jwtToken.SigningCredentials != null)
                encodedSignature =  JwtTokenUtilities.CreateEncodedSignature(string.Concat(encodedHeader, ".", encodedPayload), jwtToken.SigningCredentials);

            if (jwtToken.EncryptingCredentials != null)
                return EncryptToken(
                    new JwtSecurityToken(
                        header,
                        jwtToken.Payload,
                        encodedHeader,
                        encodedPayload,
                        encodedSignature),
                    jwtToken.EncryptingCredentials,
                    jwtToken.Header.Typ,
                    null).RawData;
            else
                return string.Concat(encodedHeader, ".", encodedPayload, ".", encodedSignature);
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
        private static bool ValidateSignature(byte[] encodedBytes, byte[] signature, SecurityKey key, string algorithm, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            Validators.ValidateAlgorithm(algorithm, key, securityToken, validationParameters);

            var cryptoProviderFactory = validationParameters.CryptoProviderFactory ?? key.CryptoProviderFactory;
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

        /// <summary>
        /// Validates that the signature, if found or required, is valid.
        /// </summary>
        /// <param name="token">A JWS token.</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> that contains signing keys.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="token"/> is null or whitespace.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="validationParameters"/> is null.</exception>
        /// <exception cref="SecurityTokenValidationException">If a signature is not found and <see cref="TokenValidationParameters.RequireSignedTokens"/> is true.</exception>
        /// <exception cref="SecurityTokenSignatureKeyNotFoundException">
        /// If the <paramref name="token"/> has a key identifier and none of the <see cref="SecurityKey"/>(s) provided result in a validated signature.
        /// This can indicate that a key refresh is required.
        /// </exception>
        /// <exception cref="SecurityTokenInvalidSignatureException">If after trying all the <see cref="SecurityKey"/>(s), none result in a validated signature AND the <paramref name="token"/> does not have a key identifier.</exception>
        /// <returns>A <see cref="JwtSecurityToken"/> that has the signature validated if token was signed.</returns>
        /// <remarks><para>If the <paramref name="token"/> is signed, the signature is validated even if <see cref="TokenValidationParameters.RequireSignedTokens"/> is false.</para>
        /// <para>If the <paramref name="token"/> signature is validated, then the <see cref="JwtSecurityToken.SigningKey"/> will be set to the key that signed the 'token'.It is the responsibility of <see cref="TokenValidationParameters.SignatureValidator"/> to set the <see cref="JwtSecurityToken.SigningKey"/></para></remarks>
        protected virtual JwtSecurityToken ValidateSignature(string token, TokenValidationParameters validationParameters)
        {
            JwtSecurityToken validatedJwt = ValidateSignatureUsingDelegates(token, validationParameters, null);
            JwtSecurityToken parsedJwtToken = GetJwtSecurityTokenFromToken(token, validationParameters);
            return ValidateSignature(token, validatedJwt ?? parsedJwtToken, validationParameters, null);
        }

        private JwtSecurityToken ValidateSignature(string token, JwtSecurityToken jwtToken, TokenValidationParameters validationParameters, BaseConfiguration configuration)
        {
            byte[] encodedBytes = Encoding.UTF8.GetBytes(jwtToken.RawHeader + "." + jwtToken.RawPayload);
            bool kidMatched = false;
            IEnumerable<SecurityKey> keys = null;

            if (string.IsNullOrEmpty(jwtToken.RawSignature))
            {
                if (validationParameters.RequireSignedTokens)
                    throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant(TokenLogMessages.IDX10504, jwtToken)));
                else
                    return jwtToken;
            }

            if (validationParameters.IssuerSigningKeyResolverUsingConfiguration != null)
            {
                keys = validationParameters.IssuerSigningKeyResolverUsingConfiguration(token, jwtToken, jwtToken.Header.Kid, validationParameters, configuration);
            }
            if (validationParameters.IssuerSigningKeyResolver != null)
            {
                keys = validationParameters.IssuerSigningKeyResolver(token, jwtToken, jwtToken.Header.Kid, validationParameters);
            }
            else
            {
                var key = configuration == null ? ResolveIssuerSigningKey(token, jwtToken, validationParameters)
                    : JwtTokenUtilities.ResolveTokenSigningKey(jwtToken.Header.Kid, jwtToken.Header.X5t, validationParameters, configuration);
                if (key != null)
                {
                    kidMatched = true;
                    keys = new List<SecurityKey> { key };
                }
            }

            if (keys == null && validationParameters.TryAllIssuerSigningKeys)
            {
                // control gets here if:
                // 1. User specified delegate: IssuerSigningKeyResolver returned null
                // 2. ResolveIssuerSigningKey returned null
                // Try all the keys. This is the degenerate case, not concerned about perf.
                keys = TokenUtilities.GetAllSigningKeys(configuration, validationParameters);
            }

            // keep track of exceptions thrown, keys that were tried
            var exceptionStrings = new StringBuilder();
            var keysAttempted = new StringBuilder();
            bool kidExists = !string.IsNullOrEmpty(jwtToken.Header.Kid);
            byte[] signatureBytes;

            try
            {
                signatureBytes = Base64UrlEncoder.DecodeBytes(jwtToken.RawSignature);
            }
            catch (FormatException e)
            {
                throw new SecurityTokenInvalidSignatureException(TokenLogMessages.IDX10508, e);
            }

            if (keys != null)
            {
                foreach (var key in keys)
                {
                    try
                    {
                        if (ValidateSignature(encodedBytes, signatureBytes, key, jwtToken.Header.Alg, jwtToken, validationParameters))
                        {
                            if (LogHelper.IsEnabled(EventLogLevel.Informational))
                                LogHelper.LogInformation(TokenLogMessages.IDX10242, jwtToken);

                            jwtToken.SigningKey = key;
                            return jwtToken;
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptionStrings.AppendLine(ex.ToString());
                    }

                    if (key != null)
                    {
                        keysAttempted.Append(key.ToString()).Append(" , KeyId: ").AppendLine(key.KeyId);
                        if (kidExists && !kidMatched && key.KeyId != null)
                            kidMatched = jwtToken.Header.Kid.Equals(key.KeyId, key is X509SecurityKey ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
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
                    var isKidInTVP = keysInTokenValidationParameters.Any(x => x.KeyId.Equals(jwtToken.Header.Kid));
                    var keyLocation = isKidInTVP ? "TokenValidationParameters" : "Configuration";
                    throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidSignatureException(LogHelper.FormatInvariant(TokenLogMessages.IDX10511,
                        keysAttempted,
                        LogHelper.MarkAsNonPII(numKeysInTokenValidationParameters),
                        LogHelper.MarkAsNonPII(numKeysInConfiguration),
                        LogHelper.MarkAsNonPII(keyLocation),
                        LogHelper.MarkAsNonPII(jwtToken.Header.Kid),
                        exceptionStrings,
                        jwtToken)));
                }

                DateTime? expires = (jwtToken.Payload.Expiration == null) ? null : new DateTime?(jwtToken.ValidTo);
                DateTime? notBefore = (jwtToken.Payload.NotBefore == null) ? null : new DateTime?(jwtToken.ValidFrom);

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

            if (keysAttempted.Length > 0)
                throw LogHelper.LogExceptionMessage(new SecurityTokenSignatureKeyNotFoundException(LogHelper.FormatInvariant(TokenLogMessages.IDX10503,
                    keysAttempted,
                    LogHelper.MarkAsNonPII(numKeysInTokenValidationParameters),
                    LogHelper.MarkAsNonPII(numKeysInConfiguration),
                    exceptionStrings,
                    jwtToken)));

            throw LogHelper.LogExceptionMessage(new SecurityTokenSignatureKeyNotFoundException(TokenLogMessages.IDX10500));
        }

        private static IEnumerable<SecurityKey> GetAllDecryptionKeys(TokenValidationParameters validationParameters)
        {
            if (validationParameters.TokenDecryptionKey != null)
                yield return validationParameters.TokenDecryptionKey;

            if (validationParameters.TokenDecryptionKeys != null)
                foreach (SecurityKey key in validationParameters.TokenDecryptionKeys)
                    yield return key;
        }

        /// <summary>
        /// Creates a <see cref="ClaimsIdentity"/> from a <see cref="JwtSecurityToken"/>.
        /// </summary>
        /// <param name="jwtToken">The <see cref="JwtSecurityToken"/> to use as a <see cref="Claim"/> source.</param>
        /// <param name="issuer">The value to set <see cref="Claim.Issuer"/></param>
        /// <param name="validationParameters"> Contains parameters for validating the token.</param>
        /// <returns>A <see cref="ClaimsIdentity"/> containing the <see cref="JwtSecurityToken.Claims"/>.</returns>
        protected virtual ClaimsIdentity CreateClaimsIdentity(JwtSecurityToken jwtToken, string issuer, TokenValidationParameters validationParameters)
        {
            if (jwtToken == null)
                throw LogHelper.LogArgumentNullException(nameof(jwtToken));

            if (validationParameters == null)
                throw LogHelper.LogArgumentNullException(nameof(validationParameters));

            var actualIssuer = issuer;
            if (string.IsNullOrWhiteSpace(issuer))
            {
                if (LogHelper.IsEnabled(EventLogLevel.Verbose))
                    LogHelper.LogVerbose(TokenLogMessages.IDX10244, LogHelper.MarkAsNonPII(ClaimsIdentity.DefaultIssuer));

                actualIssuer = ClaimsIdentity.DefaultIssuer;
            }
            
            return MapInboundClaims ? CreateClaimsIdentityWithMapping(jwtToken, actualIssuer, validationParameters) : CreateClaimsIdentityWithoutMapping(jwtToken, actualIssuer, validationParameters);
        }

        private ClaimsIdentity CreateClaimsIdentityWithMapping(JwtSecurityToken jwtToken, string actualIssuer, TokenValidationParameters validationParameters)
        {
            ClaimsIdentity identity = validationParameters.CreateClaimsIdentity(jwtToken, actualIssuer);
            foreach (Claim jwtClaim in jwtToken.Claims)
            {
                if (_inboundClaimFilter.Contains(jwtClaim.Type))
                    continue;

                string claimType;
                bool wasMapped = true;
                if (!_inboundClaimTypeMap.TryGetValue(jwtClaim.Type, out claimType))
                {
                    claimType = jwtClaim.Type;
                    wasMapped = false;
                }

                if (claimType == ClaimTypes.Actor)
                {
                    if (identity.Actor != null)
                        throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(LogMessages.IDX12710, LogHelper.MarkAsNonPII(JwtRegisteredClaimNames.Actort), LogHelper.MarkAsSecurityArtifact(jwtClaim.Value, JwtTokenUtilities.SafeLogJwtToken))));

                    if (CanReadToken(jwtClaim.Value))
                    {
                        JwtSecurityToken actor = ReadToken(jwtClaim.Value) as JwtSecurityToken;
                        identity.Actor = CreateClaimsIdentity(actor, actualIssuer, validationParameters);
                    }
                }

                Claim claim = new Claim(claimType, jwtClaim.Value, jwtClaim.ValueType, actualIssuer, actualIssuer, identity);

                if (jwtClaim.Properties.Count > 0)
                {
                    foreach (var kv in jwtClaim.Properties)
                    {
                        claim.Properties[kv.Key] = kv.Value;
                    }
                }
                if (wasMapped)
                    claim.Properties[ShortClaimTypeProperty] = jwtClaim.Type;

                identity.AddClaim(claim);
            }

            return identity;
        }

        private ClaimsIdentity CreateClaimsIdentityWithoutMapping(JwtSecurityToken jwtToken, string actualIssuer, TokenValidationParameters validationParameters)
        {
            ClaimsIdentity identity = validationParameters.CreateClaimsIdentity(jwtToken, actualIssuer);
            foreach (Claim jwtClaim in jwtToken.Claims)
            {
                if (_inboundClaimFilter.Contains(jwtClaim.Type))
                    continue;

                string claimType = jwtClaim.Type;
                if (claimType == ClaimTypes.Actor)
                {
                    if (identity.Actor != null)
                        throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(LogMessages.IDX12710, LogHelper.MarkAsNonPII(JwtRegisteredClaimNames.Actort), LogHelper.MarkAsSecurityArtifact(jwtClaim.Value, JwtTokenUtilities.SafeLogJwtToken))));

                    if (CanReadToken(jwtClaim.Value))
                    {
                        JwtSecurityToken actor = ReadToken(jwtClaim.Value) as JwtSecurityToken;
                        identity.Actor = CreateClaimsIdentity(actor, actualIssuer, validationParameters);
                    }
                }

                Claim claim = new Claim(claimType, jwtClaim.Value, jwtClaim.ValueType, actualIssuer, actualIssuer, identity);
                if (jwtClaim.Properties.Count > 0)
                {
                    foreach (var kv in jwtClaim.Properties)
                        claim.Properties[kv.Key] = kv.Value;
                }

                identity.AddClaim(claim);
            }

            return identity;
        }

        /// <summary>
        /// Creates the 'value' for the actor claim: { actort, 'value' }
        /// </summary>
        /// <param name="actor"><see cref="ClaimsIdentity"/> as actor.</param>
        /// <returns><see cref="string"/> representing the actor.</returns>
        /// <remarks>If <see cref="ClaimsIdentity.BootstrapContext"/> is not null:
        /// <para>&#160;&#160;If 'type' is 'string', return as string.</para>
        /// <para>&#160;&#160;if 'type' is 'BootstrapContext' and 'BootstrapContext.SecurityToken' is 'JwtSecurityToken'</para>
        /// <para>&#160;&#160;&#160;&#160;if 'JwtSecurityToken.RawData' != null, return RawData.</para>        
        /// <para>&#160;&#160;&#160;&#160;else return <see cref="JwtSecurityTokenHandler.WriteToken( SecurityToken )"/>.</para>        
        /// <para>&#160;&#160;if 'BootstrapContext.Token' != null, return 'Token'.</para>
        /// <para>default: <see cref="JwtSecurityTokenHandler.WriteToken(SecurityToken)"/> new ( <see cref="JwtSecurityToken"/>( actor.Claims ).</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">'actor' is null.</exception>
        protected virtual string CreateActorValue(ClaimsIdentity actor)
        {
            if (actor == null)
                throw LogHelper.LogArgumentNullException(nameof(actor));

            if (actor.BootstrapContext != null)
            {
                string encodedJwt = actor.BootstrapContext as string;
                if (encodedJwt != null)
                {
                    LogHelper.LogVerbose(LogMessages.IDX12713);
                    return encodedJwt;
                }

                JwtSecurityToken jwtToken = actor.BootstrapContext as JwtSecurityToken;
                if (jwtToken != null)
                {
                    if (jwtToken.RawData != null)
                    {
                        LogHelper.LogVerbose(LogMessages.IDX12714);
                        return jwtToken.RawData;
                    }
                    else
                    {
                        LogHelper.LogVerbose(LogMessages.IDX12715);
                        return this.WriteToken(jwtToken);
                    }
                }

                LogHelper.LogVerbose(LogMessages.IDX12711);
            }

            LogHelper.LogVerbose(LogMessages.IDX12712);
            return WriteToken(new JwtSecurityToken(claims: actor.Claims));
        }

        /// <summary>
        /// Determines if the audiences found in a <see cref="JwtSecurityToken"/> are valid.
        /// </summary>
        /// <param name="audiences">The audiences found in the <see cref="JwtSecurityToken"/>.</param>
        /// <param name="jwtToken">The <see cref="JwtSecurityToken"/> being validated.</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
        /// <remarks>See <see cref="Validators.ValidateAudience"/> for additional details.</remarks>
        protected virtual void ValidateAudience(IEnumerable<string> audiences, JwtSecurityToken jwtToken, TokenValidationParameters validationParameters)
        {
            Validators.ValidateAudience(audiences, jwtToken, validationParameters);
        }

        /// <summary>
        /// Validates the lifetime of a <see cref="JwtSecurityToken"/>.
        /// </summary>
        /// <param name="notBefore">The <see cref="DateTime"/> value of the 'nbf' claim if it exists in the 'jwtToken'.</param>
        /// <param name="expires">The <see cref="DateTime"/> value of the 'exp' claim if it exists in the 'jwtToken'.</param>
        /// <param name="jwtToken">The <see cref="JwtSecurityToken"/> being validated.</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
        /// <remarks><see cref="Validators.ValidateLifetime"/> for additional details.</remarks>
        protected virtual void ValidateLifetime(DateTime? notBefore, DateTime? expires, JwtSecurityToken jwtToken, TokenValidationParameters validationParameters)
        {
            Validators.ValidateLifetime(notBefore, expires, jwtToken, validationParameters);
        }

        /// <summary>
        /// Determines if the issuer found in a <see cref="JwtSecurityToken"/> is valid.
        /// </summary>
        /// <param name="issuer">The issuer to validate</param>
        /// <param name="jwtToken">The <see cref="JwtSecurityToken"/> that is being validated.</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
        /// <returns>The issuer to use when creating the <see cref="Claim"/>(s) in the <see cref="ClaimsIdentity"/>.</returns>
        /// <remarks><see cref="Validators.ValidateIssuer(string, SecurityToken, TokenValidationParameters)"/> for additional details.</remarks>
        protected virtual string ValidateIssuer(string issuer, JwtSecurityToken jwtToken, TokenValidationParameters validationParameters)
        {
            return Validators.ValidateIssuer(issuer, jwtToken, validationParameters);
        }

        /// <summary>
        /// Determines if a <see cref="JwtSecurityToken"/> is already validated.
        /// </summary>
        /// <param name="expires">The <see cref="DateTime"/> value of the 'exp' claim if it exists in the <see cref="JwtSecurityToken"/>'.</param>
        /// <param name="securityToken">The <see cref="JwtSecurityToken"/> that is being validated.</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
        protected virtual void ValidateTokenReplay(DateTime? expires, string securityToken, TokenValidationParameters validationParameters)
        {
            Validators.ValidateTokenReplay(expires, securityToken, validationParameters);
        }

        /// <summary>
        /// Returns a <see cref="SecurityKey"/> to use when validating the signature of a token.
        /// </summary>
        /// <param name="token">The <see cref="string"/> representation of the token that is being validated.</param>
        /// <param name="jwtToken">The <see cref="JwtSecurityToken"/> that is being validated.</param>
        /// <param name="validationParameters">A <see cref="TokenValidationParameters"/>  required for validation.</param>
        /// <returns>Returns a <see cref="SecurityKey"/> to use for signature validation.</returns>
        /// <remarks>If key fails to resolve, then null is returned</remarks>
        protected virtual SecurityKey ResolveIssuerSigningKey(string token, JwtSecurityToken jwtToken, TokenValidationParameters validationParameters)
        {
            if (validationParameters == null)
                throw LogHelper.LogArgumentNullException(nameof(validationParameters));

            if (jwtToken == null)
                throw LogHelper.LogArgumentNullException(nameof(jwtToken));

            return JwtTokenUtilities.ResolveTokenSigningKey(jwtToken.Header.Kid, jwtToken.Header.X5t, validationParameters, null);
        }

        /// <summary>
        /// Returns a <see cref="SecurityKey"/> to use when decryption a JWE.
        /// </summary>
        /// <param name="token">The <see cref="string"/> the token that is being decrypted.</param>
        /// <param name="jwtToken">The <see cref="JwtSecurityToken"/> that is being decrypted.</param>
        /// <param name="validationParameters">A <see cref="TokenValidationParameters"/>  required for validation.</param>
        /// <returns>Returns a <see cref="SecurityKey"/> to use for signature validation.</returns>
        /// <remarks>If key fails to resolve, then null is returned</remarks>
        protected virtual SecurityKey ResolveTokenDecryptionKey(string token, JwtSecurityToken jwtToken, TokenValidationParameters validationParameters)
        {
            if (jwtToken == null)
                throw LogHelper.LogArgumentNullException(nameof(jwtToken));

            if (validationParameters == null)
                throw LogHelper.LogArgumentNullException(nameof(validationParameters));

            if (!string.IsNullOrEmpty(jwtToken.Header.Kid))
            {
                if (validationParameters.TokenDecryptionKey != null 
                    && string.Equals(validationParameters.TokenDecryptionKey.KeyId, jwtToken.Header.Kid, validationParameters.TokenDecryptionKey is X509SecurityKey ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
                    return validationParameters.TokenDecryptionKey;

                if (validationParameters.TokenDecryptionKeys != null)
                {
                    foreach (var key in validationParameters.TokenDecryptionKeys)
                    {
                        if (key != null && string.Equals(key.KeyId, jwtToken.Header.Kid, key is X509SecurityKey ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
                            return key;
                    }
                }
            }

            if (!string.IsNullOrEmpty(jwtToken.Header.X5t))
            {
                if (validationParameters.TokenDecryptionKey != null)
                {
                    if (string.Equals(validationParameters.TokenDecryptionKey.KeyId, jwtToken.Header.X5t, validationParameters.TokenDecryptionKey is X509SecurityKey ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
                        return validationParameters.TokenDecryptionKey;

                    X509SecurityKey x509Key = validationParameters.TokenDecryptionKey as X509SecurityKey;
                    if (x509Key != null && string.Equals(x509Key.X5t, jwtToken.Header.X5t, StringComparison.OrdinalIgnoreCase))
                        return validationParameters.TokenDecryptionKey;
                }

                if (validationParameters.TokenDecryptionKeys != null)
                {
                    foreach (var key in validationParameters.TokenDecryptionKeys)
                    {
                        if (key != null && string.Equals(key.KeyId, jwtToken.Header.X5t, key is X509SecurityKey ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
                            return key;

                        X509SecurityKey x509Key = key as X509SecurityKey;
                        if (x509Key != null && string.Equals(x509Key.X5t, jwtToken.Header.X5t, StringComparison.OrdinalIgnoreCase))
                            return key;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Decrypts a JWE and returns the clear text 
        /// </summary>
        /// <param name="jwtToken">the JWE that contains the cypher text.</param>
        /// <param name="validationParameters">contains crypto material.</param>
        /// <returns>the decoded / cleartext contents of the JWE.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="jwtToken"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="validationParameters"/> is null.</exception>
        /// <exception cref="SecurityTokenException">if 'jwtToken.Header.enc' is null or empty.</exception>
        /// <exception cref="SecurityTokenEncryptionKeyNotFoundException">if 'jwtToken.Header.kid' is not null AND decryption fails.</exception>
        /// <exception cref="SecurityTokenDecryptionFailedException">if the JWE was not able to be decrypted.</exception>
        protected string DecryptToken(JwtSecurityToken jwtToken, TokenValidationParameters validationParameters)
        {
            if (jwtToken == null)
                throw LogHelper.LogArgumentNullException(nameof(jwtToken));

            if (validationParameters == null)
                throw LogHelper.LogArgumentNullException(nameof(validationParameters));

            if (string.IsNullOrEmpty(jwtToken.Header.Enc))
                throw LogHelper.LogExceptionMessage(new SecurityTokenException(LogHelper.FormatInvariant(TokenLogMessages.IDX10612)));

            var keys = GetContentEncryptionKeys(jwtToken, validationParameters);

            return JwtTokenUtilities.DecryptJwtToken(jwtToken, validationParameters, new JwtTokenDecryptionParameters
            {
                Alg = jwtToken.Header.Alg,
                AuthenticationTagBytes = Base64UrlEncoder.DecodeBytes(jwtToken.RawAuthenticationTag),
                CipherTextBytes = Base64UrlEncoder.DecodeBytes(jwtToken.RawCiphertext),
                DecompressionFunction = JwtTokenUtilities.DecompressToken,
                Enc = jwtToken.Header.Enc,
                EncodedToken = jwtToken.RawData,
                HeaderAsciiBytes = Encoding.ASCII.GetBytes(jwtToken.EncodedHeader),
                InitializationVectorBytes = Base64UrlEncoder.DecodeBytes(jwtToken.RawInitializationVector),
                MaximumDeflateSize = MaximumTokenSizeInBytes,
                Keys = keys,
                Zip = jwtToken.Header.Zip,
            });
        }

        internal IEnumerable<SecurityKey> GetContentEncryptionKeys(JwtSecurityToken jwtToken, TokenValidationParameters validationParameters)
        {
            IEnumerable<SecurityKey> keys = null;

            if (validationParameters.TokenDecryptionKeyResolver != null)
                keys = validationParameters.TokenDecryptionKeyResolver(jwtToken.RawData, jwtToken, jwtToken.Header.Kid, validationParameters);
            else
            {
                var key = ResolveTokenDecryptionKey(jwtToken.RawData, jwtToken, validationParameters);
                if (key != null)
                    keys = new List<SecurityKey> { key };
            }

            // control gets here if:
            // 1. User specified delegate: TokenDecryptionKeyResolver returned null
            // 2. ResolveTokenDecryptionKey returned null
            // Try all the keys. This is the degenerate case, not concerned about perf.
            if (keys == null)
                keys = GetAllDecryptionKeys(validationParameters);

            if (jwtToken.Header.Alg.Equals(JwtConstants.DirectKeyUseAlg))
                return keys;

            var unwrappedKeys = new List<SecurityKey>();
            // keep track of exceptions thrown, keys that were tried
            var exceptionStrings = new StringBuilder();
            var keysAttempted = new StringBuilder();
            foreach (var key in keys)
            {
                try
                {
#if NET472 || NET6_0_OR_GREATER
                    if (SupportedAlgorithms.EcdsaWrapAlgorithms.Contains(jwtToken.Header.Alg))
                    {
                        //// on decryption we get the public key from the EPK value see: https://datatracker.ietf.org/doc/html/rfc7518#appendix-C
                        var ecdhKeyExchangeProvider = new EcdhKeyExchangeProvider(
                            key as ECDsaSecurityKey,
                            validationParameters.TokenDecryptionKey as ECDsaSecurityKey,
                            jwtToken.Header.Alg,
                            jwtToken.Header.Enc);
                        string apu = jwtToken.Header.GetStandardClaim(JwtHeaderParameterNames.Apu);
                        string apv = jwtToken.Header.GetStandardClaim(JwtHeaderParameterNames.Apv);
                        SecurityKey kdf = ecdhKeyExchangeProvider.GenerateKdf(apu, apv);
                        var kwp = key.CryptoProviderFactory.CreateKeyWrapProviderForUnwrap(kdf, ecdhKeyExchangeProvider.GetEncryptionAlgorithm());
                        var unwrappedKey = kwp.UnwrapKey(Base64UrlEncoder.DecodeBytes(jwtToken.RawEncryptedKey));
                        unwrappedKeys.Add(new SymmetricSecurityKey(unwrappedKey));
                    }
                    else
#endif
                    if (key.CryptoProviderFactory.IsSupportedAlgorithm(jwtToken.Header.Alg, key))
                    {
                        var kwp = key.CryptoProviderFactory.CreateKeyWrapProviderForUnwrap(key, jwtToken.Header.Alg);
                        var unwrappedKey = kwp.UnwrapKey(Base64UrlEncoder.DecodeBytes(jwtToken.RawEncryptedKey));
                        unwrappedKeys.Add(new SymmetricSecurityKey(unwrappedKey));
                    }
                }
                catch (Exception ex)
                {
                    exceptionStrings.AppendLine(ex.ToString());
                }
                keysAttempted.AppendLine(key.ToString());
            }

            if (unwrappedKeys.Count > 0 || exceptionStrings.Length == 0)
                return unwrappedKeys;
            else
                throw LogHelper.LogExceptionMessage(new SecurityTokenKeyWrapException(LogHelper.FormatInvariant(TokenLogMessages.IDX10618, keysAttempted, exceptionStrings, jwtToken)));
        }

        private static byte[] GetSymmetricSecurityKey(SecurityKey key)
        {
            if (key == null)
                throw LogHelper.LogArgumentNullException(nameof(key));

            // try to use the provided key directly.
            SymmetricSecurityKey symmetricSecurityKey = key as SymmetricSecurityKey;
            if (symmetricSecurityKey != null)
                return symmetricSecurityKey.Key;
            else
            {
                JsonWebKey jsonWebKey = key as JsonWebKey;
                if (jsonWebKey != null && jsonWebKey.K != null)
                    return Base64UrlEncoder.DecodeBytes(jsonWebKey.K);
            }

            return null;
        }

        /// <summary>
        /// Validates the <see cref="JwtSecurityToken.SigningKey"/> is an expected value.
        /// </summary>
        /// <param name="key">The <see cref="SecurityKey"/> that signed the <see cref="SecurityToken"/>.</param>
        /// <param name="securityToken">The <see cref="JwtSecurityToken"/> to validate.</param>
        /// <param name="validationParameters">The current <see cref="TokenValidationParameters"/>.</param>
        /// <remarks>If the <see cref="JwtSecurityToken.SigningKey"/> is a <see cref="X509SecurityKey"/> then the X509Certificate2 will be validated using the CertificateValidator.</remarks>
        protected virtual void ValidateIssuerSecurityKey(SecurityKey key, JwtSecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            Validators.ValidateIssuerSecurityKey(key, securityToken, validationParameters);
        }

        /// <summary>
        /// Serializes to XML a token of the type handled by this instance.
        /// </summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="token">A token of type <see cref="TokenType"/>.</param>
        public override void WriteToken(XmlWriter writer, SecurityToken token)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override Task<TokenValidationResult> ValidateTokenAsync(string token, TokenValidationParameters validationParameters)
        {
            try
            {
                var claimsPrincipal = ValidateToken(token, validationParameters, out var validatedToken);
                return Task.FromResult(new TokenValidationResult
                {
                    SecurityToken = validatedToken,
                    ClaimsIdentity = claimsPrincipal?.Identity as ClaimsIdentity,
                    IsValid = true,
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new TokenValidationResult
                {
                    IsValid = false,
                    Exception = ex
                });
            }
        }
    }
}
