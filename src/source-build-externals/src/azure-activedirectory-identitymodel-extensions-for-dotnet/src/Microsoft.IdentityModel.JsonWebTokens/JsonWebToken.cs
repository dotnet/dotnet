// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.JsonWebTokens
{
    /// <summary>
    /// A <see cref="SecurityToken"/> designed for representing a JSON Web Token (JWT). 
    /// </summary>
    public partial class JsonWebToken : SecurityToken
    {
        internal const string ClassName = "Microsoft.IdentityModel.JsonWebTokens.JsonWebToken";

        private ClaimsIdentity _claimsIdentity;
        private bool _wasClaimsIdentitySet;

        private string _act;
        private string _authenticationTag;
        private string _ciphertext;
        private string _encodedHeader;
        private string _encodedPayload;
        private string _encodedSignature;
        private string _encodedToken;
        private string _encryptedKey;
        private string _initializationVector;
        private List<string> _audiences;
        private readonly ReadOnlyMemory<char> _encodedTokenMemory;

        #region properties relating to the header
        // when constructing a JWT, these properties, when found, will be set
        internal string _alg;
        internal string _cty;
        internal string _enc;
        internal string _kid;
        internal string _typ;
        internal string _x5t;
        internal string _zip;
        #endregion

        #region properties relating to the payload
        // when constructing a JWT, these properties, when found, will be set
        internal string _azp;
        internal long? _exp;
        internal DateTime? _expDateTime;
        internal long? _iat;
        internal DateTime? _iatDateTime;
        internal string _id;
        internal string _iss;
        internal string _jti;
        internal string _sub;
        internal long? _nbf;
        internal DateTime? _nbfDateTime;
        internal DateTime? _validFrom;
        internal DateTime? _validTo;
        #endregion

        /// <summary>
        /// Initializes a new instance of <see cref="JsonWebToken"/> from a string in JWS or JWE Compact serialized format.
        /// </summary>
        /// <param name="jwtEncodedString">A JSON Web Token that has been serialized in JWS or JWE Compact serialized format.</param>
        /// <exception cref="ArgumentNullException">'jwtEncodedString' is null or empty.</exception>
        /// <exception cref="ArgumentException">'jwtEncodedString' is not in JWS or JWE Compact serialization format.</exception>
        /// <remarks>
        /// see: https://datatracker.ietf.org/doc/html/rfc7519 (JWT)
        /// see: https://datatracker.ietf.org/doc/html/rfc7515 (JWS)
        /// see: https://datatracker.ietf.org/doc/html/rfc7516 (JWE)
        /// <para>
        /// The contents of the returned <see cref="JsonWebToken"/> have not been validated, the JSON Web Token is simply decoded. Validation can be accomplished using the validation methods in <see cref="JsonWebTokenHandler"/>
        /// </para>
        /// </remarks>
        public JsonWebToken(string jwtEncodedString)
        {
            if (string.IsNullOrEmpty(jwtEncodedString))
                throw LogHelper.LogExceptionMessage(new ArgumentNullException(nameof(jwtEncodedString)));

            ReadToken(jwtEncodedString.AsMemory());

            _encodedToken = jwtEncodedString;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="JsonWebToken"/> from a ReadOnlyMemory{char} in JWS or JWE Compact serialized format.
        /// </summary>
        /// <param name="encodedTokenMemory">A ReadOnlyMemory{char} containing the JSON Web Token serialized in JWS or JWE Compact format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="encodedTokenMemory"/> is empty.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="encodedTokenMemory"/> does not represent a valid JWS or JWE Compact serialization format.</exception>
        /// <remarks>
        /// See: https://datatracker.ietf.org/doc/html/rfc7519 (JWT)
        /// See: https://datatracker.ietf.org/doc/html/rfc7515 (JWS)
        /// See: https://datatracker.ietf.org/doc/html/rfc7516 (JWE)
        /// <para>
        /// The contents of the returned <see cref="JsonWebToken"/> have not been validated; the JSON Web Token is simply decoded. Validation can be performed using the methods in <see cref="JsonWebTokenHandler"/>.
        /// </para>
        /// </remarks>
        public JsonWebToken(ReadOnlyMemory<char> encodedTokenMemory)
        {
            if (encodedTokenMemory.IsEmpty)
                throw LogHelper.LogExceptionMessage(new ArgumentNullException(nameof(encodedTokenMemory)));

            ReadToken(encodedTokenMemory);

            _encodedTokenMemory = encodedTokenMemory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonWebToken"/> class where the header contains the crypto algorithms applied to the encoded header and payload.
        /// </summary>
        /// <param name="header">A string containing JSON which represents the cryptographic operations applied to the JWT and optionally any additional properties of the JWT.</param>
        /// <param name="payload">A string containing JSON which represents the claims contained in the JWT. Each claim is a JSON object of the form { Name, Value }.</param>
        /// <remarks>
        /// see: https://datatracker.ietf.org/doc/html/rfc7519 (JWT)
        /// see: https://datatracker.ietf.org/doc/html/rfc7515 (JWS)
        /// see: https://datatracker.ietf.org/doc/html/rfc7516 (JWE)
        /// <para>
        /// The contents of the returned <see cref="JsonWebToken"/> have not been validated, the JSON Web Token is simply decoded. Validation can be accomplished using the validation methods in <see cref="JsonWebTokenHandler"/>
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">'header' is null.</exception>
        /// <exception cref="ArgumentNullException">'payload' is null.</exception>
        public JsonWebToken(string header, string payload)
        {
            if (string.IsNullOrEmpty(header))
                throw LogHelper.LogArgumentNullException(nameof(header));

            if (string.IsNullOrEmpty(payload))
                throw LogHelper.LogArgumentNullException(nameof(payload));

            var encodedHeader = Base64UrlEncoder.Encode(header);
            var encodedPayload = Base64UrlEncoder.Encode(payload);
            var encodedToken = encodedHeader + "." + encodedPayload + ".";

            ReadToken(encodedToken.AsMemory());

            _encodedToken = encodedToken;
        }

        internal string ActualIssuer { get; set; }

        internal ClaimsIdentity ActorClaimsIdentity { get; set; }

        /// <summary>
        /// Gets the AuthenticationTag from the original raw data of this instance when it was created.
        /// </summary>
        /// <remarks>
        /// Contains the results of a Authentication Encryption with Associated Data (AEAD).
        /// see: https://datatracker.ietf.org/doc/html/rfc7516#section-2
        /// <para>
        /// If this JWT is not encrypted with an algorithms that uses an Authentication Tag, an empty string will be returned.
        /// </para>
        /// </remarks>
        public string AuthenticationTag
        {
            get
            {
                _authenticationTag ??= AuthenticationTagBytes == null ? string.Empty : UTF8Encoding.UTF8.GetString(AuthenticationTagBytes);
                return _authenticationTag;
            }
        }

        /// <summary>
        ///
        /// </summary>
        internal byte[] AuthenticationTagBytes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the Ciphertext representing the encrypted JWT in the original raw data.
        /// </summary>
        /// <remarks>
        /// When decrypted using values in the JWE header will contain the plaintext payload.
        /// see: https://datatracker.ietf.org/doc/html/rfc7516#section-2
        /// <para>
        /// If this JWT is not encrypted, an empty string will be returned.
        /// </para>
        /// </remarks>
        public string Ciphertext
        {
            get
            {
                _ciphertext ??= CipherTextBytes == null ? string.Empty : UTF8Encoding.UTF8.GetString(CipherTextBytes);
                return _ciphertext;
            }
        }

        /// <summary>
        ///
        /// </summary>
        internal byte[] CipherTextBytes
        {
            get;
            set;
        }

        internal int Dot1 { get; set; }

        internal int Dot2 { get; set; }

        internal int Dot3 { get; set; }

        internal int Dot4 { get; set; }

        /// <summary>
        /// Gets the EncodedHeader from the original raw data of this instance when it was created.
        /// </summary>
        /// <remarks>
        /// The original Base64UrlEncoded string of the JWT header.
        /// </remarks>
        public string EncodedHeader
        {
            get
            {
                // TODO - need to account for JWE
                if (_encodedHeader == null)
                {
                    if (!_encodedTokenMemory.IsEmpty)
                        _encodedHeader = _encodedTokenMemory.Span.Slice(0, Dot1).ToString();
                    else
                        _encodedHeader = (_encodedToken is not null) ? _encodedToken.Substring(0, Dot1) :  string.Empty;
                }

                return _encodedHeader;
            }
        }

        /// <summary>
        /// Gets the Encrypted Content Encryption Key.
        /// </summary>
        /// <remarks>
        /// For some algorithms this value may be null even though the JWT was encrypted.
        /// see: https://datatracker.ietf.org/doc/html/rfc7516#section-2
        /// <para>
        /// If not found, an empty string is returned.
        /// </para>
        /// </remarks>
        public string EncryptedKey
        {
            get
            {
                if (_encryptedKey == null)
                    _encryptedKey = EncryptedKeyBytes == null ? string.Empty : UTF8Encoding.UTF8.GetString(EncryptedKeyBytes);

                return _encryptedKey;
            }
        }

        internal byte[] EncryptedKeyBytes { get; set; }

        /// <summary>
        /// Gets the EncodedPayload from the original raw data of this instance when it was created.
        /// </summary>
        /// <remarks>
        /// The original Base64UrlEncoded of the JWT payload, for JWE this will an empty string.
        /// </remarks>
        public string EncodedPayload
        {
            get
            {
                if (_encodedPayload == null)
                {
                    if (!_encodedTokenMemory.IsEmpty)
                    {
                        _encodedPayload = IsEncrypted ? string.Empty : _encodedTokenMemory.Span.Slice(Dot1 + 1, Dot2 - Dot1 - 1).ToString();
                    }
                    else
                    {
                        if (_encodedToken is not null)
                            _encodedPayload = IsEncrypted ? string.Empty : _encodedToken.Substring(Dot1 + 1, Dot2 - Dot1 - 1);
                        else
                            _encodedPayload = string.Empty;
                    }
                }

                return _encodedPayload;
            }
        }

        /// <summary>
        /// Gets the EncodedSignature from the original raw data of this instance when it was created.
        /// </summary>
        /// <remarks>
        /// The original Base64UrlEncoded of the JWT signature.
        /// If the JWT was not signed or a JWE, an empty string is returned.
        /// </remarks>
        public string EncodedSignature
        {
            get
            {
                if (_encodedSignature == null)
                {
                    if (!_encodedTokenMemory.IsEmpty)
                    {
                        _encodedSignature = IsEncrypted ? string.Empty : _encodedTokenMemory.Span.Slice(Dot2 + 1, _encodedTokenMemory.Length - Dot2 - 1).ToString();
                    }
                    else
                    {
                        if (_encodedToken is not null)
                            _encodedSignature = IsEncrypted ? string.Empty : _encodedToken.Substring(Dot2 + 1, _encodedToken.Length - Dot2 - 1);
                        else
                            _encodedSignature = string.Empty;
                    }
                }

                return _encodedSignature;
            }
        }

        /// <summary>
        /// Gets the original raw data of this instance when it was created.
        /// </summary>
        /// <remarks>
        /// The original Base64UrlEncoded of the JWT.
        /// </remarks>
        public string EncodedToken
        {
            get
            {
               if (_encodedToken is null && !_encodedTokenMemory.IsEmpty)
                    _encodedToken = _encodedTokenMemory.ToString();

               return _encodedToken;
            }
        }

        internal JsonClaimSet Header { get; set; }

        internal byte[] HeaderAsciiBytes { get; set; }

        internal byte[] InitializationVectorBytes { get; set; }

        /// <summary>
        /// Gets the Initialization Vector used when encrypting the plaintext.
        /// </summary>
        /// <remarks>
        /// see: https://datatracker.ietf.org/doc/html/rfc7516#appendix-A-1-4
        /// <para>
        /// Some algorithms may not use an Initialization Vector.
        /// If not found an empty string is returned.
        /// </para>
        /// </remarks>
        public string InitializationVector
        {
            get
            {
                if (InitializationVectorBytes == null)
                    _initializationVector = InitializationVectorBytes == null ? string.Empty : UTF8Encoding.UTF8.GetString(InitializationVectorBytes);

                return _initializationVector;
            }
        }

        /// <summary>
        /// Gets the <see cref="JsonWebToken"/> associated with this instance.
        /// </summary>
        /// <remarks>
        /// see: https://datatracker.ietf.org/doc/html/rfc7516#section-2
        /// For encrypted tokens {JWE}, this represents the JWT that was encrypted.
        /// <para>
        /// If the JWT is not encrypted, this value will be null.
        /// </para>
        /// </remarks>
        public JsonWebToken InnerToken { get; internal set; }

        /// <summary>
        /// Returns true if this JsonWebToken was encrypted a JWE.
        /// </summary>
        public bool IsEncrypted { get => CipherTextBytes != null; }

        /// <summary>
        /// Returns true if this JsonWebToken was signed a JWS.
        /// </summary>
        public bool IsSigned { get; internal set; }

        internal JsonClaimSet Payload { get; set; }

        /// <summary>
        /// Not implemented.
        /// </summary>
        public override SecurityKey SecurityKey { get; }

        /// <summary>
        /// Gets or sets the <see cref="SecurityKey"/> that was used to sign this token.
        /// </summary>
        /// <remarks>
        /// If the JWT was not signed or validated, this value will be null.
        /// </remarks>
        public override SecurityKey SigningKey { get; set; }

        internal byte[] MessageBytes{ get; set; }

        internal int NumberOfDots { get; set; }

        /// <summary>
        /// Converts a span into an instance of <see cref="JsonWebToken"/>.
        /// </summary>
        /// <param name="encodedTokenMemory">A span representing a 'JSON Web Token' (JWT) in JWS or JWE Compact Serialization Format.</param>
        /// <exception cref="SecurityTokenMalformedException">if <paramref name="encodedTokenMemory"/> is malformed, a valid JWT should have either 2 dots (JWS) or 4 dots (JWE).</exception>
        /// <exception cref="SecurityTokenMalformedException">if <paramref name="encodedTokenMemory"/> does not have a non-empty authentication tag after the 4th dot for a JWE.</exception>
        /// <exception cref="SecurityTokenMalformedException">if <paramref name="encodedTokenMemory"/> has more than 4 dots.</exception>
        internal void ReadToken(ReadOnlyMemory<char> encodedTokenMemory)
        {
            // JWT must have 2 dots for JWS or 4 dots for JWE (a.b.c.d.e)
            ReadOnlySpan<char> encodedTokenSpan = encodedTokenMemory.Span;

            Dot1 = encodedTokenSpan.IndexOf('.'); 
            if (Dot1 == -1 || Dot1 == encodedTokenSpan.Length - 1)
                throw LogHelper.LogExceptionMessage(new SecurityTokenMalformedException(LogMessages.IDX14100));

            // Dot2 index in the second segment
            Dot2 = encodedTokenSpan.Slice(Dot1 + 1).IndexOf('.');
            if (Dot2 == -1)
                throw LogHelper.LogExceptionMessage(new SecurityTokenMalformedException(LogMessages.IDX14120));

            // Dot2 index in the whole token
            Dot2 = Dot1 + Dot2 + 1;
            Dot3 = (Dot2 == encodedTokenSpan.Length - 1) ? -1 : encodedTokenSpan.Slice(Dot2 + 1).IndexOf('.');

            if (Dot3 == -1)
            {
                // JWS has two dots
                // JWS: https://www.rfc-editor.org/rfc/rfc7515
                // Format: https://www.rfc-editor.org/rfc/rfc7515#page-7

                IsSigned = !(Dot2 + 1 == encodedTokenSpan.Length);
                try
                {
                    Header = CreateClaimSet(encodedTokenSpan, 0, Dot1, createHeaderClaimSet: true);
                }
                catch (Exception ex)
                {
                    throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(
                        LogMessages.IDX14102,
                        LogHelper.MarkAsUnsafeSecurityArtifact(encodedTokenSpan.Slice(0, Dot1).ToString(), t => t.ToString())), // TODO: Add an overload to LogHelper.MarkAsUnsafeSecurityArtifact that accepts span?
                        ex));
                }

                try
                {
                    Payload = CreateClaimSet(encodedTokenSpan, Dot1 + 1, Dot2 - Dot1 - 1, createHeaderClaimSet: false);
                }
                catch (Exception ex)
                {
                    throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(
                        LogMessages.IDX14101,
                        LogHelper.MarkAsUnsafeSecurityArtifact(encodedTokenSpan.Slice(Dot1 + 1, Dot2 - Dot1 - 1).ToString(), t => t.ToString())),
                        ex));
                }
            }
            else
            {
                // JWE: https://www.rfc-editor.org/rfc/rfc7516
                // Format: https://www.rfc-editor.org/rfc/rfc7516#page-8
                // empty payload for JWE's {encrypted tokens}.
                Payload = new JsonClaimSet();

                Dot3 = Dot2 + Dot3 + 1;
                if (Dot3 == encodedTokenSpan.Length - 1)
                    throw LogHelper.LogExceptionMessage(new SecurityTokenMalformedException(LogMessages.IDX14121));
                
                Dot4 = encodedTokenSpan.Slice(Dot3 + 1).IndexOf('.');
                if (Dot4 == -1)
                    throw LogHelper.LogExceptionMessage(new SecurityTokenMalformedException(LogMessages.IDX14121));
                
                Dot4 = Dot3 + Dot4 + 1;

                // must have something after 4th dot
                if (Dot4 == encodedTokenSpan.Length - 1)
                    throw LogHelper.LogExceptionMessage(new SecurityTokenMalformedException(LogMessages.IDX14310));

                if (encodedTokenSpan.Slice(Dot4 + 1).IndexOf('.') != -1)
                    throw LogHelper.LogExceptionMessage(new SecurityTokenMalformedException(LogMessages.IDX14122));

                ReadOnlySpan<char> headerSpan = encodedTokenSpan.Slice(0, Dot1);
                if (headerSpan.IsEmpty)
                    throw LogHelper.LogExceptionMessage(new ArgumentException(LogMessages.IDX14307));

                // right number of dots for JWE (4)
                byte[] headerAsciiBytes = new byte[headerSpan.Length];
#if NET6_0_OR_GREATER
                Encoding.ASCII.GetBytes(headerSpan, headerAsciiBytes);
#else
                unsafe
                {
                    fixed (char* hCharsPtr = headerSpan)
                    fixed (byte* headerAsciiBytesPtr = headerAsciiBytes)
                    {
                        Encoding.ASCII.GetBytes(hCharsPtr, headerSpan.Length, headerAsciiBytesPtr, headerAsciiBytes.Length);
                    }
                }
#endif

                HeaderAsciiBytes = headerAsciiBytes;

                try
                {
                    Header = CreateHeaderClaimSet(Base64UrlEncoder.UnsafeDecode(headerSpan).AsSpan());
                }
                catch (Exception ex)
                {
                    throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(
                        LogMessages.IDX14102,
                        LogHelper.MarkAsUnsafeSecurityArtifact(headerSpan.ToString(), t => t.ToString())),
                        ex));
                }

                // delegating retrieving encrypted Key to the getter on EncryptedKey
                ReadOnlySpan<char> encryptedKeyBytes = encodedTokenSpan.Slice(Dot1 + 1, Dot2 - Dot1 - 1);
                if (!encryptedKeyBytes.IsEmpty)
                {
                    EncryptedKeyBytes = Base64UrlEncoder.UnsafeDecode(encryptedKeyBytes);
                    _encryptedKey = encryptedKeyBytes.ToString();
                }
                else
                {
                    _encryptedKey = string.Empty;
                }

                ReadOnlySpan<char> initializationVectorSpan = encodedTokenSpan.Slice(Dot2 + 1, Dot3 - Dot2 - 1);
                if (initializationVectorSpan.IsEmpty)
                    throw LogHelper.LogExceptionMessage(new ArgumentException(LogMessages.IDX14308));

                try
                {
                    InitializationVectorBytes = Base64UrlEncoder.UnsafeDecode(initializationVectorSpan);
                }
                catch (Exception ex)
                {
                    throw LogHelper.LogExceptionMessage(new ArgumentException(LogMessages.IDX14309, ex));
                }

                ReadOnlySpan<char> authTagSpan = encodedTokenSpan.Slice(Dot4 + 1);
                if (authTagSpan.IsEmpty)
                    throw LogHelper.LogExceptionMessage(new ArgumentException(LogMessages.IDX14310));

                try
                {
                    AuthenticationTagBytes = Base64UrlEncoder.UnsafeDecode(authTagSpan);
                }
                catch (Exception ex)
                {
                    throw LogHelper.LogExceptionMessage(new ArgumentException(LogMessages.IDX14311, ex));
                }

                ReadOnlySpan<char> cipherTextSpan = encodedTokenSpan.Slice(Dot3 + 1, Dot4 - Dot3 - 1);
                if (cipherTextSpan.IsEmpty)
                    throw LogHelper.LogExceptionMessage(new ArgumentException(LogMessages.IDX14306));

                try
                {
                    CipherTextBytes = Base64UrlEncoder.UnsafeDecode(cipherTextSpan);
                }
                catch (Exception ex)
                {
                    throw LogHelper.LogExceptionMessage(new ArgumentException(LogMessages.IDX14312, ex));
                }
            }
        }

        internal JsonClaimSet CreateClaimSet(ReadOnlySpan<char> strSpan, int startIndex, int length, bool createHeaderClaimSet)
        {
            int outputSize = Base64UrlEncoding.ValidateAndGetOutputSize(strSpan, startIndex, length);
            byte[] output = ArrayPool<byte>.Shared.Rent(outputSize);
            try
            {
                Base64UrlEncoding.Decode(strSpan, startIndex, length, output);
                return createHeaderClaimSet ? CreateHeaderClaimSet(output.AsSpan()) : CreatePayloadClaimSet(output.AsSpan());
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(output);
            }
        }

        /// <summary>
        /// Returns the encoded token without signature or authentication tag.
        /// </summary>
        /// <returns>Encoded token string without signature or authentication tag.</returns>
        public override string ToString()
        {
            return EncodedToken.Substring(0, EncodedToken.LastIndexOf("."));
        }

        /// <inheritdoc/>
        public override string UnsafeToString() => EncodedToken;

        #region System.Security.Claims.Claim methods
        /// <summary>
        /// Gets a <see cref="IEnumerable{Claim}"/> where each claim in the JWT { name, value } is returned as a <see cref="Claim"/>.
        /// </summary>
        /// <remarks>
        /// A <see cref="Claim"/> requires each value to be represented as a string. If the value was not a string, then <see cref="Claim.Type"/> contains the json type.
        /// <see cref="JsonClaimValueTypes"/> and <see cref="ClaimValueTypes"/> to determine the json type.
        /// </remarks>
        public virtual IEnumerable<Claim> Claims
        {
            get
            {
                return Payload.Claims(Issuer ?? ClaimsIdentity.DefaultIssuer);
            }
        }

        /// <summary>
        /// Gets a <see cref="Claim"/> representing the { key, 'value' } pair corresponding to the provided <paramref name="key"/>.
        /// </summary>
        /// <remarks>
        /// A <see cref="Claim"/> requires each value to be represented as a string. If the value was not a string, then <see cref="Claim.Type"/> contains the json type.
        /// <see cref="JsonClaimValueTypes"/> and <see cref="ClaimValueTypes"/> to determine the json type.
        /// <para>
        /// If the key has no corresponding value, this method will throw.
        /// </para>
        /// </remarks>
        public Claim GetClaim(string key)
        {
            return Payload.GetClaim(key, Issuer ?? ClaimsIdentity.DefaultIssuer);
        }

        internal ClaimsIdentity ClaimsIdentity
        {
            get
            {
                if (!_wasClaimsIdentitySet)
                {
                    _wasClaimsIdentitySet = true;
                    string actualIssuer = ActualIssuer ?? Issuer;

                    foreach (Claim claim in Claims)
                    {
                        string claimType = claim.Type;
                        if (claimType == ClaimTypes.Actor)
                        {
                            if (_claimsIdentity.Actor != null)
                                throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(LogMessages.IDX14112, LogHelper.MarkAsNonPII(JwtRegisteredClaimNames.Actort), claim.Value)));

#pragma warning disable CA1031 // Do not catch general exception types
                            try
                            {
                                JsonWebToken actorToken = new JsonWebToken(claim.Value);
                                _claimsIdentity.Actor = ActorClaimsIdentity;
                            }
                            catch
                            {

                            }
#pragma warning restore CA1031 // Do not catch general exception types
                        }

                        if (claim.Properties.Count == 0)
                        {
                            _claimsIdentity.AddClaim(new Claim(claimType, claim.Value, claim.ValueType, actualIssuer, actualIssuer, _claimsIdentity));
                        }
                        else
                        {
                            Claim newClaim = new Claim(claimType, claim.Value, claim.ValueType, actualIssuer, actualIssuer, _claimsIdentity);

                            foreach (var kv in claim.Properties)
                                newClaim.Properties[kv.Key] = kv.Value;

                            _claimsIdentity.AddClaim(newClaim);
                        }
                    }
                }

                return _claimsIdentity;
            }

            set
            {
                _claimsIdentity = value;
            }
        }

        /// <summary>
        /// Try to get a <see cref="Claim"/> representing the { key, 'value' } pair corresponding to the provided <paramref name="key"/>.
        /// The value is obtained from the Payload.
        /// </summary>
        /// <remarks>
        /// A <see cref="Claim"/> requires each value to be represented as a string. If the value was not a string, then <see cref="Claim.Type"/> contains the json type.
        /// <see cref="JsonClaimValueTypes"/> and <see cref="ClaimValueTypes"/> to determine the json type.
        /// </remarks>
        /// <returns>true if successful, false otherwise.</returns>
        public bool TryGetClaim(string key, out Claim value)
        {
            return Payload.TryGetClaim(key, Issuer ?? ClaimsIdentity.DefaultIssuer, out value);
        }
        #endregion

        #region Get Claims from the JWT Header and Payload
        internal bool HasPayloadClaim(string claimName)
        {
            return Payload.HasClaim(claimName);
        }

        /// <summary>
        /// Gets the 'value' corresponding to key from the JWT header transformed as type 'T'.
        /// </summary>
        /// <remarks>
        /// The expectation is that the 'value' corresponds to a type are expected in a JWT token.
        /// The 5 basic types: number, string, true / false, nil, array (of basic types).
        /// This is not a general purpose translation layer for complex types.
        /// </remarks>
        /// <returns>The value as <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentException">if claim is not found or a transformation to <typeparamref name="T"/> cannot be made.</exception>
        public T GetHeaderValue<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw LogHelper.LogArgumentNullException(nameof(key));

            return Header.GetValue<T>(key);
        }

        /// <summary>
        /// Gets the 'value' corresponding to key from the JWT payload transformed as type 'T'.
        /// </summary>
        /// <remarks>
        /// The expectation is that the 'value' corresponds to a type are expected in a JWT token.
        /// The 5 basic types: number, string, true / false, nil, array (of basic types).
        /// This is not a general purpose translation layer for complex types.
        /// </remarks>
        /// <returns>The value as <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentException">if claim is not found or a transformation to <typeparamref name="T"/> cannot be made.</exception>
        public T GetPayloadValue<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw LogHelper.LogArgumentNullException(nameof(key));

            if (typeof(T).Equals(typeof(Claim)))
                return (T)(object)GetClaim(key);

            return Payload.GetValue<T>(key);
        }

        /// <summary>
        /// Tries to get the claim from the JWT payload.
        /// </summary>
        /// <remarks>
        /// The 'value' a type T if possible.
        /// </remarks>
        /// <returns>true if successful, false otherwise.</returns>
        public bool TryGetValue<T>(string key, out T value)
        {
            if (string.IsNullOrEmpty(key))
            {
                value = default;
                return false;
            }

            return Payload.TryGetValue(key, out value);
        }

        /// <summary>
        /// Tries to get the value corresponding to the provided key from the JWT header { key, 'value' }.
        /// </summary>
        /// <remarks>
        /// The expectation is that the 'value' corresponds to a type expected in a JWT token.
        /// The 5 basic types: number, string, true / false, nil, array (of basic types).
        /// This is not a general purpose translation layer for complex types.
        /// </remarks>
        /// <returns>true if successful, false otherwise.</returns>
        public bool TryGetHeaderValue<T>(string key, out T value)
        {
            if (string.IsNullOrEmpty(key))
            {
                value = default;
                return false;
            }

            return Header.TryGetValue(key, out value);
        }

        /// <summary>
        /// Try to get the 'value' corresponding to key from the JWT payload transformed as type 'T'.
        /// </summary>
        /// <remarks>
        /// The expectation is that the 'value' corresponds to a type are expected in a JWT token.
        /// The 5 basic types: number, string, true / false, nil, array (of basic types).
        /// This is not a general purpose translation layer for complex types.
        /// </remarks>
        /// <returns>true if successful, false otherwise.</returns>
        public bool TryGetPayloadValue<T>(string key, out T value)
        {
            if (string.IsNullOrEmpty(key))
            {
                value = default;
                return false;
            }

            if (typeof(T).Equals(typeof(Claim)))
            {
                bool foundClaim = TryGetClaim(key, out var claim);
                value = (T)(object)claim;
                return foundClaim;
            }

            return Payload.TryGetValue(key, out value);
        }
        #endregion

        #region Header Properties
        /// <summary>
        /// Gets the 'value' of the 'alg' claim from the header.
        /// </summary>
        /// <remarks>
        /// Identifies the cryptographic algorithm used to encrypt or determine the value of the Content Encryption Key.
        /// Applicable to an encrypted JWT {JWE}.
        /// see: https://datatracker.ietf.org/doc/html/rfc7516#section-4-1-1
        /// <para>
        /// If the 'alg' claim is not found, an empty string is returned.
        /// </para>
        /// </remarks>
        public string Alg
        {
            get
            {
                _alg ??= Header.GetStringValue(JwtHeaderParameterNames.Alg);
                return _alg;
            }
        }

        /// <summary>
        /// Gets the 'value' of the 'cty' claim from the header.
        /// </summary>
        /// <remarks>
        /// Used by JWS applications to declare the media type[IANA.MediaTypes] of the secured content (the payload).
        /// see: https://datatracker.ietf.org/doc/html/rfc7516#section-4.1.12 (JWE)
        /// see: https://datatracker.ietf.org/doc/html/rfc7515#section-4.1.10 (JWS)
        /// <para>
        /// If the 'cty' claim is not found, an empty string is returned.
        /// </para>
        /// </remarks>
        public string Cty
        {
            get
            {
                _cty ??= Header.GetStringValue(JwtHeaderParameterNames.Cty);
                return _cty;
            }
        }

        /// <summary>
        /// Gets the 'value' of the 'enc' claim from the header.
        /// </summary>
        /// <remarks>
        /// Identifies the content encryption algorithm used to perform authenticated encryption
        /// on the plaintext to produce the ciphertext and the Authentication Tag.
        /// see: https://datatracker.ietf.org/doc/html/rfc7516#section-4.1.2
        /// </remarks>
        public string Enc
        {
            get
            {
                _enc ??= Header.GetStringValue(JwtHeaderParameterNames.Enc);
                return _enc;
            }
        }

        /// <summary>
        /// Gets the 'value' of the 'kid' claim from the header.
        /// </summary>
        /// <remarks>
        /// 'kid'is a hint indicating which key was used to secure the JWS.
        /// see: https://datatracker.ietf.org/doc/html/rfc7515#section-4.1.4 (JWS)
        /// see: https://datatracker.ietf.org/doc/html/rfc7516#section-4.1.6 (JWE)
        /// <para>
        /// If the 'kid' claim is not found, an empty string is returned.
        /// </para>
        /// </remarks>
        public string Kid
        {
            get
            {
                _kid ??= Header.GetStringValue(JwtHeaderParameterNames.Kid);
                return _kid;
            }
        }

        /// <summary>
        /// Gets the 'value' of the 'typ' claim from the header.
        /// </summary>
        /// <remarks>
        /// Is used by JWT applications to declare the media type.
        /// see: https://datatracker.ietf.org/doc/html/rfc7519#section-5.1
        /// <para>
        /// If the 'typ' claim is not found, an empty string is returned.
        /// </para>
        /// </remarks>
        public string Typ
        {
            get
            {
                _typ ??= Header.GetStringValue(JwtHeaderParameterNames.Typ);
                return _typ;
            }

            internal set => _typ = value;
        }

        /// <summary>
        /// Gets the 'value' of the 'x5t' claim from the header.
        /// </summary>
        /// <remarks>
        /// Is the base64url-encoded SHA-1 thumbprint(a.k.a.digest) of the DER encoding of the X.509 certificate used to sign this token.
        /// see: https://datatracker.ietf.org/doc/html/rfc7515#section-4.1.7
        /// <para>
        /// If the 'x5t' claim is not found, an empty string is returned.
        /// </para>
        /// </remarks>
        public string X5t
        {
            get
            {
                _x5t ??= Header.GetStringValue(JwtHeaderParameterNames.X5t);
                return _x5t;
            }
        }

        /// <summary>
        /// Gets the 'value' of the 'zip' claim from the header.
        /// </summary>
        /// <remarks>
        /// The "zip" (compression algorithm) applied to the plaintext before encryption, if any.
        /// see: https://datatracker.ietf.org/doc/html/rfc7516#section-4.1.3
        /// <para>
        /// If the 'zip' claim is not found, an empty string is returned.
        /// </para>
        /// </remarks>
        public string Zip
        {
            get
            {
                _zip ??= Header.GetStringValue(JwtHeaderParameterNames.Zip);
                return _zip;
            }
        }
        #endregion

        #region Payload Properties
        /// <summary>
        /// Gets the 'value' of the 'actort' claim the payload.
        /// </summary>
        /// <remarks>
        /// If the 'actort' claim is not found, an empty string is returned.
        /// </remarks>
        public string Actor
            {
                get
                {
                    _act ??= Payload.GetStringValue(JwtRegisteredClaimNames.Actort);
                    return _act;
                }
            }

        /// <summary>
        /// Gets the list of 'aud' claims from the payload.
        /// </summary>
        /// <remarks>
        /// Identifies the recipients that the JWT is intended for.
        /// see: https://datatracker.ietf.org/doc/html/rfc7519#section-4-1-3
        /// <para>
        /// If the 'aud' claim is not found, enumeration will be empty.
        /// </para>
        /// </remarks>
        public IEnumerable<string> Audiences
        {
            get
            {
                if (_audiences == null)
                {
                    List<string> tmp = TryGetValue(JwtRegisteredClaimNames.Aud, out List<string> audiences) ? audiences : new List<string>();
                    Interlocked.CompareExchange(ref _audiences, tmp, null);
                }

                return _audiences;
            }
        }

        /// <summary>
        /// Gets the 'azp' claim from the payload.
        /// </summary>
        /// <remarks>
        /// Identifies the authorized party for the id_token.
        /// see: https://openid.net/specs/openid-connect-core-1_0.html
        /// <para>
        /// If the 'azp' claim is not found, an empty string is returned.
        /// </para>
        /// </remarks>
        public string Azp
        {
            get
            {
                _azp ??= Payload.GetStringValue(JwtRegisteredClaimNames.Azp);
                return _azp;
            }
        }

        /// <summary>
        /// Gets the 'value' of the 'iat' claim converted to a <see cref="DateTime"/> from the payload.
        /// </summary>
        /// <remarks>
        /// Identifies the time at which the JWT was issued.
        /// see: https://datatracker.ietf.org/doc/html/rfc7519#section-4.1.6
        /// <para>
        /// If the 'iat' claim is not found, then <see cref="DateTime.MinValue"/> is returned.
        /// </para>
        /// </remarks>
        public DateTime IssuedAt
        {
            get
            {
                _iatDateTime ??= Payload.GetDateTime(JwtRegisteredClaimNames.Iat);
                return _iatDateTime.Value;
            }
        }

        /// <summary>
        /// Gets the 'value' of the 'iss' claim from the payload.
        /// </summary>
        /// <remarks>
        /// Identifies the principal that issued the JWT.
        /// see: https://datatracker.ietf.org/doc/html/rfc7519#section-4.1.1
        /// <para>
        /// If the 'iss' claim is not found, an empty string is returned.
        /// </para>
        /// </remarks>
        public override string Issuer
        {
            get
            {
                _iss ??= Payload.GetStringValue(JwtRegisteredClaimNames.Iss);
                return _iss;
            }
        }

        /// <summary>
        /// Gets the 'value' of the 'jti' claim from the payload.
        /// </summary>
        /// <remarks>
        /// Provides a unique identifier for the JWT.
        /// see: https://datatracker.ietf.org/doc/html/rfc7519#section-4.1.7
        /// <para>
        /// If the 'jti' claim is not found, an empty string is returned.
        /// </para>
        /// </remarks>
        public override string Id
        {
            get
            {
                _jti ??= Payload.GetStringValue(JwtRegisteredClaimNames.Jti);
                return _jti;
            }
        }

        /// <summary>
        /// Gets the 'value' of the 'sub' claim from the payload.
        /// </summary>
        /// <remarks>
        /// see: https://datatracker.ietf.org/doc/html/rfc7519#section-4.1.2
        /// Identifies the principal that is the subject of the JWT.
        /// <para>
        /// If the 'sub' claim is not found, an empty string is returned.
        /// </para>
        /// </remarks>
        public string Subject
        {
            get
            {
                _sub ??= Payload.GetStringValue(JwtRegisteredClaimNames.Sub);
                return _sub;
            }
        }

        /// <summary>
        /// Gets the 'value' of the 'nbf' claim converted to a <see cref="DateTime"/> from the payload.
        /// </summary>
        /// <remarks>
        /// Identifies the time before which the JWT MUST NOT be accepted for processing.
        /// see: https://datatracker.ietf.org/doc/html/rfc7519#section-4.1.5
        /// <para>
        /// If the 'nbf' claim is not found, then <see cref="DateTime.MinValue"/> is returned.
        /// </para>
        /// </remarks>
        public override DateTime ValidFrom
        {
            get
            {
                _validFrom ??= Payload.GetDateTime(JwtRegisteredClaimNames.Nbf);
                return _validFrom.Value;
            }
        }

        internal DateTime? ValidFromNullable
        {
            get
            {
                _validFrom ??= Payload.GetDateTime(JwtRegisteredClaimNames.Nbf);
                return _validFrom;
            }
        }

        /// <summary>
        /// Gets the 'value' of the 'exp' claim converted to a <see cref="DateTime"/> from the payload.
        /// </summary>
        /// <remarks>
        /// Identifies the expiration time on or after which the JWT MUST NOT be accepted for processing.
        /// see: https://datatracker.ietf.org/doc/html/rfc7519#section-4.1.4
        /// <para>
        /// If the 'exp' claim is not found, then <see cref="DateTime.MinValue"/> is returned.
        /// </para>
        /// </remarks>
        public override DateTime ValidTo
        {
            get
            {
                _validTo ??= Payload.GetDateTime(JwtRegisteredClaimNames.Exp);
                return _validTo.Value;
            }
        }

        internal DateTime? ValidToNullable
        {
            get
            {
                _validTo ??= Payload.GetDateTime(JwtRegisteredClaimNames.Exp);
                return _validTo;
            }
        }
        #endregion
    }
}
