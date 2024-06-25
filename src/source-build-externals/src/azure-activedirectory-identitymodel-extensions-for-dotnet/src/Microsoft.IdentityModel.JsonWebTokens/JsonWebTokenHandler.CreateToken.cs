// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using JsonPrimitives = Microsoft.IdentityModel.Tokens.Json.JsonSerializerPrimitives;
using TokenLogMessages = Microsoft.IdentityModel.Tokens.LogMessages;

namespace Microsoft.IdentityModel.JsonWebTokens
{
    /// <summary>
    /// A <see cref="SecurityTokenHandler"/> designed for creating and validating Json Web Tokens. 
    /// See: https://datatracker.ietf.org/doc/html/rfc7519 and http://www.rfc-editor.org/info/rfc7515.
    /// </summary>
    /// <remarks>This partial class is focused on TokenCreation.</remarks>
    public partial class JsonWebTokenHandler : TokenHandler
    {
        /// <summary>
        /// Creates an unsigned JWS (Json Web Signature).
        /// </summary>
        /// <param name="payload">A string containing JSON which represents the JWT token payload.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="payload"/> is null.</exception>
        /// <returns>A JWS in Compact Serialization Format.</returns>
        public virtual string CreateToken(
            string payload)
        {
            if (string.IsNullOrEmpty(payload))
                throw LogHelper.LogArgumentNullException(nameof(payload));

                return CreateToken(
                    payload,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null);
        }

        /// <summary>
        /// Creates an unsigned JWS (Json Web Signature).
        /// </summary>
        /// <param name="payload">A string containing JSON which represents the JWT token payload.</param>
        /// <param name="additionalHeaderClaims">Defines the dictionary containing any custom header claims that need to be added to the JWT token header.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="payload"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="additionalHeaderClaims"/> is null.</exception>
        /// <returns>A JWS in Compact Serialization Format.</returns>
        public virtual string CreateToken(
            string payload,
            IDictionary<string, object> additionalHeaderClaims)
        {
            if (string.IsNullOrEmpty(payload))
                throw LogHelper.LogArgumentNullException(nameof(payload));

            _ = additionalHeaderClaims ?? throw LogHelper.LogArgumentNullException(nameof(additionalHeaderClaims));

            return CreateToken(payload,
                null,
                null,
                null,
                additionalHeaderClaims,
                null,
                null);
        }

        /// <summary>
        /// Creates a JWS (Json Web Signature).
        /// </summary>
        /// <param name="payload">A string containing JSON which represents the JWT token payload.</param>
        /// <param name="signingCredentials">Defines the security key and algorithm that will be used to sign the JWS.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="payload"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="signingCredentials"/> is null.</exception>
        /// <returns>A JWS in Compact Serialization Format.</returns>
        public virtual string CreateToken(
            string payload,
            SigningCredentials signingCredentials)
        {
            if (string.IsNullOrEmpty(payload))
                throw LogHelper.LogArgumentNullException(nameof(payload));

            _ = signingCredentials ?? throw LogHelper.LogArgumentNullException(nameof(signingCredentials));

            return CreateToken(
                payload,
                signingCredentials,
                null,
                null,
                null,
                null,
                null);
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
        public virtual string CreateToken(
            string payload,
            SigningCredentials signingCredentials,
            IDictionary<string, object> additionalHeaderClaims)
        {
            if (string.IsNullOrEmpty(payload))
                throw LogHelper.LogArgumentNullException(nameof(payload));

            _ = signingCredentials ?? throw LogHelper.LogArgumentNullException(nameof(signingCredentials));
            _ = additionalHeaderClaims ?? throw LogHelper.LogArgumentNullException(nameof(additionalHeaderClaims));

            return CreateToken(
                payload,
                signingCredentials,
                null,
                null,
                additionalHeaderClaims,
                null,
                null);
        }

        /// <summary>
        /// Creates a JWt that can be a JWS or JWE.
        /// </summary>
        /// <param name="tokenDescriptor">A <see cref="SecurityTokenDescriptor"/> that contains details of contents of the token.</param>
        /// <returns>A JWT in Compact Serialization Format.</returns>
        public virtual string CreateToken(SecurityTokenDescriptor tokenDescriptor)
        {
            _ = tokenDescriptor ?? throw LogHelper.LogArgumentNullException(nameof(tokenDescriptor));

            if (LogHelper.IsEnabled(EventLogLevel.Warning))
            {
                if ((tokenDescriptor.Subject == null || !tokenDescriptor.Subject.Claims.Any())
                    && (tokenDescriptor.Claims == null || !tokenDescriptor.Claims.Any()))
                    LogHelper.LogWarning(
                        LogMessages.IDX14114, LogHelper.MarkAsNonPII(nameof(SecurityTokenDescriptor)), LogHelper.MarkAsNonPII(nameof(SecurityTokenDescriptor.Subject)), LogHelper.MarkAsNonPII(nameof(SecurityTokenDescriptor.Claims)));
            }

            if (tokenDescriptor.AdditionalHeaderClaims?.Count > 0 && tokenDescriptor.AdditionalHeaderClaims.Keys.Intersect(JwtTokenUtilities.DefaultHeaderParameters, StringComparer.OrdinalIgnoreCase).Any())
                throw LogHelper.LogExceptionMessage(
                    new SecurityTokenException(
                        LogHelper.FormatInvariant(
                            LogMessages.IDX14116,
                            LogHelper.MarkAsNonPII(nameof(tokenDescriptor.AdditionalHeaderClaims)),
                            LogHelper.MarkAsNonPII(string.Join(", ", JwtTokenUtilities.DefaultHeaderParameters)))));

            if (tokenDescriptor.AdditionalInnerHeaderClaims?.Count > 0 && tokenDescriptor.AdditionalInnerHeaderClaims.Keys.Intersect(JwtTokenUtilities.DefaultHeaderParameters, StringComparer.OrdinalIgnoreCase).Any())
                throw LogHelper.LogExceptionMessage(
                    new SecurityTokenException(
                        LogHelper.FormatInvariant(
                            LogMessages.IDX14116,
                            LogHelper.MarkAsNonPII(nameof(tokenDescriptor.AdditionalInnerHeaderClaims)),
                            LogHelper.MarkAsNonPII(string.Join(", ", JwtTokenUtilities.DefaultHeaderParameters)))));

            return CreateToken(
                tokenDescriptor,
                SetDefaultTimesOnTokenCreation,
                TokenLifetimeInMinutes);
        }

        internal static string CreateToken(
            SecurityTokenDescriptor tokenDescriptor,
            bool setdefaultTimesOnTokenCreation,
            int tokenLifetimeInMinutes)
        {
            // The form of a JWS is: Base64UrlEncoding(UTF8(Header)) | . | Base64UrlEncoding(Payload) | . | Base64UrlEncoding(Signature)
            // Where the Header is specifically the UTF8 bytes of the JSON, whereas the Payload encoding is not specified, but UTF8 is used by everyone.
            // The signature is over ASCII(Utf8Bytes(Base64UrlEncoding(Header) | . | Base64UrlEncoding(Payload)))
            // Since it is not known how large the JWS will be, a MemoryStream is used.
            // An ArrayBufferWriter was benchmarked, while slightly faster, more memory is used and different code would be needed for 461+ and net6.0+
            //
            // net6.0 has added api's that allow passing an allocated buffer when calculating the signature, so ArrayPool.Rent can be used.

            using (MemoryStream utf8ByteMemoryStream = new())
            {
                Utf8JsonWriter writer = null;
                char[] encodedChars = null;
                byte[] asciiBytes = null;
                byte[] signatureBytes = null;

                try
                {
                    writer = new(utf8ByteMemoryStream, new JsonWriterOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });

                    WriteJwsHeader(
                        ref writer,
                        tokenDescriptor.SigningCredentials,
                        tokenDescriptor.EncryptingCredentials,
                        tokenDescriptor.AdditionalHeaderClaims,
                        tokenDescriptor.AdditionalInnerHeaderClaims,
                        tokenDescriptor.TokenType);

                    // mark length of jwt header
                    int headerLength = (int)utf8ByteMemoryStream.Length;

                    // reset the writer and write the payload
                    writer.Reset();
                    WriteJwsPayload(
                        ref writer,
                        tokenDescriptor,
                        setdefaultTimesOnTokenCreation,
                        tokenLifetimeInMinutes);

                    // mark end of payload
                    int payloadEnd = (int)utf8ByteMemoryStream.Length;
                    int signatureSize = 0;
                    if (tokenDescriptor.SigningCredentials != null)
                        signatureSize = SupportedAlgorithms.GetMaxByteCount(tokenDescriptor.SigningCredentials.Algorithm);

                    int encodedBufferSize = (payloadEnd + 4 + signatureSize) / 3 * 4;
                    encodedChars = ArrayPool<char>.Shared.Rent(encodedBufferSize + 4);

                    // Base64UrlEncode the Header
                    int sizeOfEncodedHeader = Base64UrlEncoder.Encode(utf8ByteMemoryStream.GetBuffer().AsSpan(0, headerLength), encodedChars);
                    encodedChars[sizeOfEncodedHeader] = '.';
                    int sizeOfEncodedPayload = Base64UrlEncoder.Encode(utf8ByteMemoryStream.GetBuffer().AsSpan(headerLength, payloadEnd - headerLength), encodedChars.AsSpan(sizeOfEncodedHeader + 1));
                    // encodeChars => 'EncodedHeader.EncodedPayload'

                    // Get ASCII Bytes of 'EncodedHeader.EncodedPayload' which is used to calculate the signature
                    asciiBytes = ArrayPool<byte>.Shared.Rent(Encoding.ASCII.GetMaxByteCount(encodedBufferSize));
                    int sizeOfEncodedHeaderAndPayloadAsciiBytes
                        = Encoding.ASCII.GetBytes(encodedChars, 0, sizeOfEncodedHeader + sizeOfEncodedPayload + 1, asciiBytes, 0);

                    encodedChars[sizeOfEncodedHeader + sizeOfEncodedPayload + 1] = '.';
                    // encodedChars => 'EncodedHeader.EncodedPayload.'

                    int sizeOfEncodedSignature = 0;
                    if (tokenDescriptor.SigningCredentials != null)
                    {
#if NET6_0_OR_GREATER
                        signatureBytes = ArrayPool<byte>.Shared.Rent(signatureSize);
                        bool signatureSucceeded = JwtTokenUtilities.CreateSignature(
                            asciiBytes.AsSpan(0, sizeOfEncodedHeaderAndPayloadAsciiBytes),
                            signatureBytes,
                            tokenDescriptor.SigningCredentials,
                            out int signatureLength);
#else
                        signatureBytes = JwtTokenUtilities.CreateEncodedSignature(asciiBytes, 0, sizeOfEncodedHeaderAndPayloadAsciiBytes, tokenDescriptor.SigningCredentials);
                        int signatureLength = signatureBytes.Length;
#endif
                        sizeOfEncodedSignature = Base64UrlEncoder.Encode(signatureBytes.AsSpan(0, signatureLength), encodedChars.AsSpan(sizeOfEncodedHeader + sizeOfEncodedPayload + 2));
                    }

                    if (tokenDescriptor.EncryptingCredentials != null)
                    {
                        return EncryptToken(
                            Encoding.UTF8.GetBytes(encodedChars, 0, sizeOfEncodedHeader + sizeOfEncodedPayload + sizeOfEncodedSignature + 2),
                            tokenDescriptor.EncryptingCredentials,
                            tokenDescriptor.CompressionAlgorithm,
                            tokenDescriptor.AdditionalHeaderClaims,
                            tokenDescriptor.TokenType);
                    }
                    else
                    {
                        return encodedChars.AsSpan(0, sizeOfEncodedHeader + sizeOfEncodedPayload + sizeOfEncodedSignature + 2).ToString();
                    }
                }
                finally
                {
                    if (encodedChars is not null)
                        ArrayPool<char>.Shared.Return(encodedChars);
#if NET6_0_OR_GREATER
                    if (signatureBytes is not null)
                        ArrayPool<byte>.Shared.Return(signatureBytes);
#endif
                    if (asciiBytes is not null)
                        ArrayPool<byte>.Shared.Return(asciiBytes);

                    writer?.Dispose();
                }
            }
        }

        /// <summary>
        /// Creates a JWE (Json Web Encryption).
        /// </summary>
        /// <param name="payload">A string containing JSON which represents the JWT token payload.</param>
        /// <param name="encryptingCredentials">Defines the security key and algorithm that will be used to encrypt the JWT.</param>
        /// <returns>A JWE in compact serialization format.</returns>
        public virtual string CreateToken(
            string payload,
            EncryptingCredentials encryptingCredentials)
        {
            if (string.IsNullOrEmpty(payload))
                throw LogHelper.LogArgumentNullException(nameof(payload));

            _ = encryptingCredentials ?? throw LogHelper.LogArgumentNullException(nameof(encryptingCredentials));

            return CreateToken(
                payload,
                null,
                encryptingCredentials,
                null,
                null,
                null,
                null);
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
        public virtual string CreateToken(
            string payload,
            EncryptingCredentials encryptingCredentials,
            IDictionary<string, object> additionalHeaderClaims)
        {
            if (string.IsNullOrEmpty(payload))
                throw LogHelper.LogArgumentNullException(nameof(payload));

            _ = encryptingCredentials ?? throw LogHelper.LogArgumentNullException(nameof(encryptingCredentials));
            _ = additionalHeaderClaims ?? throw LogHelper.LogArgumentNullException(nameof(additionalHeaderClaims));

            return CreateToken(
                payload,
                null,
                encryptingCredentials,
                null,
                additionalHeaderClaims,
                null,
                null);
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
        public virtual string CreateToken(
            string payload,
            SigningCredentials signingCredentials,
            EncryptingCredentials encryptingCredentials)
        {
            if (string.IsNullOrEmpty(payload))
                throw LogHelper.LogArgumentNullException(nameof(payload));

            _ = signingCredentials ?? throw LogHelper.LogArgumentNullException(nameof(signingCredentials));
            _ = encryptingCredentials ?? throw LogHelper.LogArgumentNullException(nameof(encryptingCredentials));

            return CreateToken(
                payload,
                signingCredentials,
                encryptingCredentials,
                null,
                null,
                null,
                null);
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

            _ = signingCredentials ?? throw LogHelper.LogArgumentNullException(nameof(signingCredentials));
            _ = encryptingCredentials ?? throw LogHelper.LogArgumentNullException(nameof(encryptingCredentials));
            _ = additionalHeaderClaims ?? throw LogHelper.LogArgumentNullException(nameof(additionalHeaderClaims));

            return CreateToken(
                payload,
                signingCredentials,
                encryptingCredentials,
                null,
                additionalHeaderClaims,
                null,
                null);
        }

        /// <summary>
        /// Creates a JWE (Json Web Encryption).
        /// </summary>
        /// <param name="payload">A string containing JSON which represents the JWT token payload.</param>
        /// <param name="encryptingCredentials">Defines the security key and algorithm that will be used to encrypt the JWT.</param>
        /// <param name="compressionAlgorithm">Defines the compression algorithm that will be used to compress the JWT token payload.</param>
        /// <returns>A JWE in compact serialization format.</returns>
        public virtual string CreateToken(
            string payload,
            EncryptingCredentials encryptingCredentials,
            string compressionAlgorithm)
        {
            if (string.IsNullOrEmpty(payload))
                throw LogHelper.LogArgumentNullException(nameof(payload));

            if (string.IsNullOrEmpty(compressionAlgorithm))
                throw LogHelper.LogArgumentNullException(nameof(compressionAlgorithm));

            _ = encryptingCredentials ?? throw LogHelper.LogArgumentNullException(nameof(encryptingCredentials));

            return CreateToken(
                payload,
                null,
                encryptingCredentials,
                compressionAlgorithm,
                null,
                null,
                null);
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
        public virtual string CreateToken(
            string payload,
            SigningCredentials signingCredentials,
            EncryptingCredentials encryptingCredentials,
            string compressionAlgorithm)
        {
            if (string.IsNullOrEmpty(payload))
                throw LogHelper.LogArgumentNullException(nameof(payload));

            if (string.IsNullOrEmpty(compressionAlgorithm))
                throw LogHelper.LogArgumentNullException(nameof(compressionAlgorithm));

            _ = signingCredentials ?? throw LogHelper.LogArgumentNullException(nameof(signingCredentials));
            _ = encryptingCredentials ?? throw LogHelper.LogArgumentNullException(nameof(encryptingCredentials));

            return CreateToken(
                payload,
                signingCredentials,
                encryptingCredentials,
                compressionAlgorithm,
                null,
                null,
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

            if (string.IsNullOrEmpty(compressionAlgorithm))
                throw LogHelper.LogArgumentNullException(nameof(compressionAlgorithm));

            _ = signingCredentials ?? throw LogHelper.LogArgumentNullException(nameof(signingCredentials));
            _ = encryptingCredentials ?? throw LogHelper.LogArgumentNullException(nameof(encryptingCredentials));
            _ = additionalHeaderClaims ?? throw LogHelper.LogArgumentNullException(nameof(additionalHeaderClaims));
            _ = additionalInnerHeaderClaims ?? throw LogHelper.LogArgumentNullException(nameof(additionalInnerHeaderClaims));

            return CreateToken(
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

            if (string.IsNullOrEmpty(compressionAlgorithm))
                throw LogHelper.LogArgumentNullException(nameof(compressionAlgorithm));

            _ = signingCredentials ?? throw LogHelper.LogArgumentNullException(nameof(signingCredentials));
            _ = encryptingCredentials ?? throw LogHelper.LogArgumentNullException(nameof(encryptingCredentials));
            _ = additionalHeaderClaims ?? throw LogHelper.LogArgumentNullException(nameof(additionalHeaderClaims));

            return CreateToken(
                payload,
                signingCredentials,
                encryptingCredentials,
                compressionAlgorithm,
                additionalHeaderClaims,
                null,
                null);
        }

        internal static string CreateToken
        (
            string payload,
            SigningCredentials signingCredentials,
            EncryptingCredentials encryptingCredentials,
            string compressionAlgorithm,
            IDictionary<string, object> additionalHeaderClaims,
            IDictionary<string, object> additionalInnerHeaderClaims,
            string tokenType)
        {
            using (MemoryStream utf8ByteMemoryStream = new ())
            {
                Utf8JsonWriter writer = null;
                char[] encodedChars = null;
                byte[] asciiBytes = null;
                byte[] signatureBytes = null;
                byte[] payloadBytes = null;

                try
                {
                    writer = new Utf8JsonWriter(utf8ByteMemoryStream, new JsonWriterOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });

                    WriteJwsHeader(
                        ref writer,
                        signingCredentials,
                        encryptingCredentials,
                        additionalHeaderClaims,
                        additionalInnerHeaderClaims,
                        null);

                    // mark length of jwt header
                    int headerLength = (int)utf8ByteMemoryStream.Length;
                    int signatureSize = 0;
                    if (signingCredentials != null)
                        signatureSize = SupportedAlgorithms.GetMaxByteCount(signingCredentials.Algorithm);

                    payloadBytes = ArrayPool<byte>.Shared.Rent(Encoding.UTF8.GetMaxByteCount(payload.Length));
                    int payloadSize = Encoding.UTF8.GetBytes(payload, 0, payload.Length, payloadBytes, 0);

                    int encodedBufferSize = (headerLength + payloadSize + 4 + signatureSize) / 3 * 4;
                    encodedChars = ArrayPool<char>.Shared.Rent(encodedBufferSize + 4);

                    int sizeOfEncodedHeader = Base64UrlEncoder.Encode(utf8ByteMemoryStream.GetBuffer().AsSpan(0, headerLength), encodedChars);
                    encodedChars[sizeOfEncodedHeader] = '.';

                    int sizeOfEncodedPayload = Base64UrlEncoder.Encode(payloadBytes.AsSpan(0, payloadSize), encodedChars.AsSpan(sizeOfEncodedHeader + 1));
                    // encodeChars => 'EncodedHeader.EncodedPayload'

                    // Get ASCII Bytes of 'EncodedHeader.EncodedPayload' which is used to calculate the signature
                    asciiBytes = ArrayPool<byte>.Shared.Rent(Encoding.ASCII.GetMaxByteCount(encodedBufferSize));
                    int sizeOfEncodedHeaderAndPayloadAsciiBytes
                        = Encoding.ASCII.GetBytes(encodedChars, 0, sizeOfEncodedHeader + sizeOfEncodedPayload + 1, asciiBytes, 0);

                    encodedChars[sizeOfEncodedHeader + sizeOfEncodedPayload + 1] = '.';
                    // encodedChars => 'EncodedHeader.EncodedPayload.'

                    int sizeOfEncodedSignature = 0;
                    if (signingCredentials != null)
                    {
#if NET6_0_OR_GREATER
                        signatureBytes = ArrayPool<byte>.Shared.Rent(signatureSize);
                        bool signatureSucceeded = JwtTokenUtilities.CreateSignature(
                            asciiBytes.AsSpan(0, sizeOfEncodedHeaderAndPayloadAsciiBytes),
                            signatureBytes,
                            signingCredentials,
                            out int signatureLength);
#else
                        signatureBytes = JwtTokenUtilities.CreateEncodedSignature(asciiBytes, 0, sizeOfEncodedHeaderAndPayloadAsciiBytes, signingCredentials);
                        int signatureLength = signatureBytes.Length;
#endif
                        sizeOfEncodedSignature = Base64UrlEncoder.Encode(signatureBytes.AsSpan(0, signatureLength), encodedChars.AsSpan(sizeOfEncodedHeader + sizeOfEncodedPayload + 2));
                    }

                    if (encryptingCredentials != null)
                    {
                        return EncryptToken(
                            Encoding.UTF8.GetBytes(encodedChars, 0, sizeOfEncodedHeader + sizeOfEncodedPayload + sizeOfEncodedSignature + 2),
                            encryptingCredentials,
                            compressionAlgorithm,
                            additionalHeaderClaims,
                            tokenType);
                    }
                    else
                    {
                        return encodedChars.AsSpan(0, sizeOfEncodedHeader + sizeOfEncodedPayload + sizeOfEncodedSignature + 2).ToString();
                    }
                }
                finally
                {
                    if (encodedChars is not null)
                        ArrayPool<char>.Shared.Return(encodedChars);
#if NET6_0_OR_GREATER
                    if (signatureBytes is not null)
                        ArrayPool<byte>.Shared.Return(signatureBytes);
#endif
                    if (asciiBytes is not null)
                        ArrayPool<byte>.Shared.Return(asciiBytes);

                    if (payloadBytes is not null)
                        ArrayPool<byte>.Shared.Return(payloadBytes);

                    writer?.Dispose();
                }
            }
        }

        /// <summary>
        /// A <see cref="SecurityTokenDescriptor"/> can contain claims from multiple locations.
        /// This method consolidates the claims and adds default times {exp, iat, nbf} if needed.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to use.</param>
        /// <param name="tokenDescriptor">The <see cref="SecurityTokenDescriptor"/> used to create the token.</param>
        /// <param name="setDefaultTimesOnTokenCreation">A boolean that controls if expiration, notbefore, issuedat should be added if missing.</param>
        /// <param name="tokenLifetimeInMinutes">The default value for the token lifetime in minutes.</param>
        /// <returns>A dictionary of claims.</returns>
        internal static void WriteJwsPayload(
            ref Utf8JsonWriter writer,
            SecurityTokenDescriptor tokenDescriptor,
            bool setDefaultTimesOnTokenCreation,
            int tokenLifetimeInMinutes)
        {
            bool audienceChecked = false;
            bool audienceSet = false;
            bool issuerChecked = false;
            bool issuerSet = false;
            bool expChecked = false;
            bool expSet = false;
            bool iatChecked = false;
            bool iatSet = false;
            bool nbfChecked = false;
            bool nbfSet = false;

            writer.WriteStartObject();

            if (!string.IsNullOrEmpty(tokenDescriptor.Audience))
            {
                audienceSet = true;
                writer.WritePropertyName(JwtPayloadUtf8Bytes.Aud);
                writer.WriteStringValue(tokenDescriptor.Audience);
            }

            if (!string.IsNullOrEmpty(tokenDescriptor.Issuer))
            {
                issuerSet = true;
                writer.WritePropertyName(JwtPayloadUtf8Bytes.Iss);
                writer.WriteStringValue(tokenDescriptor.Issuer);
            }

            if (tokenDescriptor.Expires.HasValue)
            {
                expSet = true;
                writer.WritePropertyName(JwtPayloadUtf8Bytes.Exp);
                writer.WriteNumberValue(EpochTime.GetIntDate(tokenDescriptor.Expires.Value));
            }

            if (tokenDescriptor.IssuedAt.HasValue)
            {
                iatSet = true;
                writer.WritePropertyName(JwtPayloadUtf8Bytes.Iat);
                writer.WriteNumberValue(EpochTime.GetIntDate(tokenDescriptor.IssuedAt.Value));
            }

            if (tokenDescriptor.NotBefore.HasValue)
            {
                nbfSet = true;
                writer.WritePropertyName(JwtPayloadUtf8Bytes.Nbf);
                writer.WriteNumberValue(EpochTime.GetIntDate(tokenDescriptor.NotBefore.Value));
            }

            // Duplicates are resolved according to the following priority:
            // SecurityTokenDescriptor.{Audience, Issuer, Expires, IssuedAt, NotBefore}, SecurityTokenDescriptor.Claims, SecurityTokenDescriptor.Subject.Claims
            // SecurityTokenDescriptor.Claims are KeyValuePairs<string,object>, whereas SecurityTokenDescriptor.Subject.Claims are System.Security.Claims.Claim and are processed differently.

            if (tokenDescriptor.Claims != null && tokenDescriptor.Claims.Count > 0)
            {
                foreach (KeyValuePair<string, object> kvp in tokenDescriptor.Claims)
                {
                    if (!audienceChecked && kvp.Key.Equals(JwtRegisteredClaimNames.Aud, StringComparison.Ordinal))
                    {
                        audienceChecked = true;
                        if (audienceSet)
                        {
                            if (LogHelper.IsEnabled(EventLogLevel.Informational))
                                LogHelper.LogInformation(LogHelper.FormatInvariant(LogMessages.IDX14113, LogHelper.MarkAsNonPII(nameof(tokenDescriptor.Audience))));

                            continue;
                        }

                        audienceSet = true;
                    }

                    if (!issuerChecked && kvp.Key.Equals(JwtRegisteredClaimNames.Iss, StringComparison.Ordinal))
                    {
                        issuerChecked = true;
                        if (issuerSet)
                        {
                            if (LogHelper.IsEnabled(EventLogLevel.Informational))
                                LogHelper.LogInformation(LogHelper.FormatInvariant(LogMessages.IDX14113, LogHelper.MarkAsNonPII(nameof(tokenDescriptor.Issuer))));

                            continue;
                        }

                        issuerSet = true;
                    }

                    if (!expChecked && kvp.Key.Equals(JwtRegisteredClaimNames.Exp, StringComparison.Ordinal))
                    {
                        expChecked = true;
                        if (expSet)
                        {
                            if (LogHelper.IsEnabled(EventLogLevel.Informational))
                                LogHelper.LogInformation(LogHelper.FormatInvariant(LogMessages.IDX14113, LogHelper.MarkAsNonPII(nameof(tokenDescriptor.Expires))));

                            continue;
                        }

                        expSet = true;
                    }

                    if (!iatChecked && kvp.Key.Equals(JwtRegisteredClaimNames.Iat, StringComparison.Ordinal))
                    {
                        iatChecked = true;
                        if (iatSet)
                        {
                            if (LogHelper.IsEnabled(EventLogLevel.Informational))
                                LogHelper.LogInformation(LogHelper.FormatInvariant(LogMessages.IDX14113, LogHelper.MarkAsNonPII(nameof(tokenDescriptor.Expires))));

                            continue;
                        }

                        iatSet = true;
                    }

                    if (!nbfChecked && kvp.Key.Equals(JwtRegisteredClaimNames.Nbf, StringComparison.Ordinal))
                    {
                        nbfChecked = true;
                        if (nbfSet)
                        {
                            if (LogHelper.IsEnabled(EventLogLevel.Informational))
                                LogHelper.LogInformation(LogHelper.FormatInvariant(LogMessages.IDX14113, LogHelper.MarkAsNonPII(nameof(tokenDescriptor.Expires))));

                            continue;
                        }

                        nbfSet = true;
                    }

                    JsonPrimitives.WriteObject(ref writer, kvp.Key, kvp.Value);
                }
            }

            AddSubjectClaims(ref writer, tokenDescriptor, audienceSet, issuerSet, ref expSet, ref iatSet, ref nbfSet);

            // By default we set these three properties only if they haven't been detected before.
            if (setDefaultTimesOnTokenCreation && !(expSet && iatSet && nbfSet))
            {
                DateTime now = DateTime.UtcNow;

                if (!expSet)
                {
                    writer.WritePropertyName(JwtPayloadUtf8Bytes.Exp);
                    writer.WriteNumberValue(EpochTime.GetIntDate(now + TimeSpan.FromMinutes(tokenLifetimeInMinutes)));
                }

                if (!iatSet)
                {
                    writer.WritePropertyName(JwtPayloadUtf8Bytes.Iat);
                    writer.WriteNumberValue(EpochTime.GetIntDate(now));
                }

                if (!nbfSet)
                {
                    writer.WritePropertyName(JwtPayloadUtf8Bytes.Nbf);
                    writer.WriteNumberValue(EpochTime.GetIntDate(now));
                }
            }

            writer.WriteEndObject();
            writer.Flush();
        }

        internal static void AddSubjectClaims(
            ref Utf8JsonWriter writer,
            SecurityTokenDescriptor tokenDescriptor,
            bool audienceSet,
            bool issuerSet,
            ref bool expSet,
            ref bool iatSet,
            ref bool nbfSet)
        {
            if (tokenDescriptor.Subject == null)
                return;

            bool expReset = false;
            bool iatReset = false;
            bool nbfReset = false;

            var payload = new Dictionary<string, object>();

            bool checkClaims = tokenDescriptor.Claims != null && tokenDescriptor.Claims.Count > 0;

            foreach (Claim claim in tokenDescriptor.Subject.Claims)
            {
                if (claim == null)
                    continue;

                // skipping these as they have been added by values in the SecurityTokenDescriptor
                if (checkClaims && tokenDescriptor.Claims.ContainsKey(claim.Type))
                    continue;

                if (audienceSet && claim.Type.Equals(JwtRegisteredClaimNames.Aud, StringComparison.Ordinal))
                    continue;

                if (issuerSet && claim.Type.Equals(JwtRegisteredClaimNames.Iss, StringComparison.Ordinal))
                    continue;

                if (claim.Type.Equals(JwtRegisteredClaimNames.Exp, StringComparison.Ordinal))
                {
                    if (expSet)
                        continue;

                    expReset = true;
                }

                if (claim.Type.Equals(JwtRegisteredClaimNames.Iat, StringComparison.Ordinal))
                {
                    if (iatSet)
                        continue;

                    iatReset = true;
                }

                if (claim.Type.Equals(JwtRegisteredClaimNames.Nbf, StringComparison.Ordinal))
                {
                    if (nbfSet)
                        continue;

                    nbfReset = true;
                }

                object jsonClaimValue = claim.ValueType.Equals(ClaimValueTypes.String) ? claim.Value : TokenUtilities.GetClaimValueUsingValueType(claim);

                // The enumeration is from ClaimsIdentity.Claims, there can be duplicates.
                // When a duplicate is detected, we create a List and add both to a list.
                // When the creating the JWT and a list is found, a JsonArray will be created.
                if (payload.TryGetValue(claim.Type, out object existingValue))
                {
                    if (existingValue is List<object> existingList)
                    {
                        existingList.Add(jsonClaimValue);
                    }
                    else
                    {
                        payload[claim.Type] = new List<object>
                        {
                            existingValue,
                            jsonClaimValue
                        };
                    }
                }
                else
                {
                    payload[claim.Type] = jsonClaimValue;
                }
            }

            foreach (KeyValuePair<string, object> kvp in payload)
                JsonPrimitives.WriteObject(ref writer, kvp.Key, kvp.Value);

            expSet |= expReset;
            iatSet |= iatReset;
            nbfSet |= nbfReset;
        }

        internal static void WriteJwsHeader(
            ref Utf8JsonWriter writer,
            SigningCredentials signingCredentials,
            EncryptingCredentials encryptingCredentials,
            IDictionary<string, object> jweHeaderClaims,
            IDictionary<string, object> jwsHeaderClaims,
            string tokenType)
        {
            if (jweHeaderClaims?.Count > 0 && jweHeaderClaims.Keys.Intersect(JwtTokenUtilities.DefaultHeaderParameters, StringComparer.OrdinalIgnoreCase).Any())
                throw LogHelper.LogExceptionMessage(
                    new SecurityTokenException(
                        LogHelper.FormatInvariant(
                            LogMessages.IDX14116,
                            LogHelper.MarkAsNonPII(nameof(jweHeaderClaims)),
                            LogHelper.MarkAsNonPII(string.Join(", ", JwtTokenUtilities.DefaultHeaderParameters)))));

            if (jwsHeaderClaims?.Count > 0 && jwsHeaderClaims.Keys.Intersect(JwtTokenUtilities.DefaultHeaderParameters, StringComparer.OrdinalIgnoreCase).Any())
                throw LogHelper.LogExceptionMessage(
                    new SecurityTokenException(
                        LogHelper.FormatInvariant(
                            LogMessages.IDX14116,
                            LogHelper.MarkAsNonPII(nameof(jwsHeaderClaims)),
                            LogHelper.MarkAsNonPII(string.Join(", ", JwtTokenUtilities.DefaultHeaderParameters)))));


            // If token is a JWE, jweHeaderClaims go in outer header.
            bool addJweHeaderClaims = encryptingCredentials is null && jweHeaderClaims?.Count > 0;
            bool addJwsHeaderClaims = jwsHeaderClaims?.Count > 0;
            bool typeWritten = false;
            writer.WriteStartObject();

            if (signingCredentials == null)
            {
                writer.WriteString(JwtHeaderUtf8Bytes.Alg, SecurityAlgorithms.None);
            }
            else
            {
                writer.WriteString(JwtHeaderUtf8Bytes.Alg, signingCredentials.Algorithm);
                if (signingCredentials.Key.KeyId != null)
                    writer.WriteString(JwtHeaderUtf8Bytes.Kid, signingCredentials.Key.KeyId);

                if (signingCredentials.Key is X509SecurityKey x509SecurityKey)
                    writer.WriteString(JwtHeaderUtf8Bytes.X5t, x509SecurityKey.X5t);
            }

            // Priority is additionalInnerHeaderClaims, additionalHeaderClaims, defaults
            if (addJweHeaderClaims)
            {
                foreach (KeyValuePair<string, object> kvp in jweHeaderClaims)
                {
                    if (addJwsHeaderClaims && jwsHeaderClaims.ContainsKey(kvp.Key))
                        continue;

                    JsonPrimitives.WriteObject(ref writer, kvp.Key, kvp.Value);
                    if (!typeWritten && kvp.Key.Equals(JwtHeaderParameterNames.Typ, StringComparison.Ordinal))
                        typeWritten = true;
                }
            }

            if (addJwsHeaderClaims)
            {
                foreach (KeyValuePair<string, object> kvp in jwsHeaderClaims)
                {
                    JsonPrimitives.WriteObject(ref writer, kvp.Key, kvp.Value);
                    if (!typeWritten && kvp.Key.Equals(JwtHeaderParameterNames.Typ, StringComparison.Ordinal))
                        typeWritten = true;
                }
            }

            if (!typeWritten)
                writer.WriteString(JwtHeaderUtf8Bytes.Typ, string.IsNullOrEmpty(tokenType) ? JwtConstants.HeaderType : tokenType);

            writer.WriteEndObject();
            writer.Flush();
        }

        internal static byte[] WriteJweHeader(
            EncryptingCredentials encryptingCredentials,
            string compressionAlgorithm,
            string tokenType,
            IDictionary<string, object> jweHeaderClaims)
        {
            using (MemoryStream memoryStream = new())
            {
                Utf8JsonWriter writer = null;
                try
                {
                    writer = new Utf8JsonWriter(memoryStream, new JsonWriterOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                    writer.WriteStartObject();

                    writer.WriteString(JwtHeaderUtf8Bytes.Alg, encryptingCredentials.Alg);
                    writer.WriteString(JwtHeaderUtf8Bytes.Enc, encryptingCredentials.Enc);

                    if (encryptingCredentials.Key.KeyId != null)
                        writer.WriteString(JwtHeaderUtf8Bytes.Kid, encryptingCredentials.Key.KeyId);

                    if (!string.IsNullOrEmpty(compressionAlgorithm))
                        writer.WriteString(JwtHeaderUtf8Bytes.Zip, compressionAlgorithm);

                    bool typeWritten = false;
                    bool ctyWritten = !encryptingCredentials.SetDefaultCtyClaim;

                    // Current 6x Priority is jweHeaderClaims, type, cty
                    if (jweHeaderClaims != null && jweHeaderClaims.Count > 0)
                    {
                        foreach (KeyValuePair<string, object> kvp in jweHeaderClaims)
                        {
                            JsonPrimitives.WriteObject(ref writer, kvp.Key, kvp.Value);
                            if (!typeWritten && kvp.Key.Equals(JwtHeaderParameterNames.Typ, StringComparison.Ordinal))
                                typeWritten = true;
                            else if (!ctyWritten && kvp.Key.Equals(JwtHeaderParameterNames.Cty, StringComparison.Ordinal))
                                ctyWritten = true;
                        }
                    }

                    if (!typeWritten)
                        writer.WriteString(JwtHeaderUtf8Bytes.Typ, string.IsNullOrEmpty(tokenType) ? JwtConstants.HeaderType : tokenType);

                    if (!ctyWritten)
                        writer.WriteString(JwtHeaderUtf8Bytes.Cty, JwtConstants.HeaderType);

                    writer.WriteEndObject();
                    writer.Flush();

                    return memoryStream.ToArray();
                }
                finally
                {
                    writer?.Dispose();
                }
            }
        }

        internal static byte[] CompressToken(byte[] utf8Bytes, string compressionAlgorithm)
        {
            if (string.IsNullOrEmpty(compressionAlgorithm))
                throw LogHelper.LogArgumentNullException(nameof(compressionAlgorithm));

            if (!CompressionProviderFactory.Default.IsSupportedAlgorithm(compressionAlgorithm))
                throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(TokenLogMessages.IDX10682, LogHelper.MarkAsNonPII(compressionAlgorithm))));

            var compressionProvider = CompressionProviderFactory.Default.CreateCompressionProvider(compressionAlgorithm);

            return compressionProvider.Compress(utf8Bytes) ?? throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(TokenLogMessages.IDX10680, LogHelper.MarkAsNonPII(compressionAlgorithm))));
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
        public string EncryptToken(
            string innerJwt,
            EncryptingCredentials encryptingCredentials,
            IDictionary<string, object> additionalHeaderClaims)
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
        public string EncryptToken(
            string innerJwt,
            EncryptingCredentials encryptingCredentials,
            string algorithm)
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
        public string EncryptToken(
            string innerJwt,
            EncryptingCredentials encryptingCredentials,
            string algorithm,
            IDictionary<string, object> additionalHeaderClaims)
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

        private static string EncryptTokenPrivate(
            string innerJwt,
            EncryptingCredentials encryptingCredentials,
            string compressionAlgorithm,
            IDictionary<string, object> additionalHeaderClaims,
            string tokenType)
        {
            return (EncryptToken(
                        Encoding.UTF8.GetBytes(innerJwt),
                        encryptingCredentials,
                        compressionAlgorithm,
                        additionalHeaderClaims,
                        tokenType));
        }

        internal static string EncryptToken(
            byte[] innerTokenUtf8Bytes,
            EncryptingCredentials encryptingCredentials,
            string compressionAlgorithm,
            IDictionary<string, object> additionalHeaderClaims,
            string tokenType)
        {
            CryptoProviderFactory cryptoProviderFactory = encryptingCredentials.CryptoProviderFactory ?? encryptingCredentials.Key.CryptoProviderFactory;

            if (cryptoProviderFactory == null)
                throw LogHelper.LogExceptionMessage(new ArgumentException(TokenLogMessages.IDX10620));

            SecurityKey securityKey = JwtTokenUtilities.GetSecurityKey(encryptingCredentials, cryptoProviderFactory, additionalHeaderClaims, out byte[] wrappedKey);

            using (AuthenticatedEncryptionProvider encryptionProvider = cryptoProviderFactory.CreateAuthenticatedEncryptionProvider(securityKey, encryptingCredentials.Enc))
            {
                if (encryptionProvider == null)
                    throw LogHelper.LogExceptionMessage(new SecurityTokenEncryptionFailedException(LogMessages.IDX14103));

                byte[] jweHeader = WriteJweHeader(encryptingCredentials, compressionAlgorithm, tokenType, additionalHeaderClaims);
                byte[] plainText;
                if (!string.IsNullOrEmpty(compressionAlgorithm))
                {
                    try
                    {
                        plainText = CompressToken(innerTokenUtf8Bytes, compressionAlgorithm);
                    }
                    catch (Exception ex)
                    {
                        throw LogHelper.LogExceptionMessage(new SecurityTokenCompressionFailedException(LogHelper.FormatInvariant(TokenLogMessages.IDX10680, LogHelper.MarkAsNonPII(compressionAlgorithm)), ex));
                    }
                }
                else
                {
                    plainText = innerTokenUtf8Bytes;
                }

                try
                {
                    string rawHeader = Base64UrlEncoder.Encode(jweHeader);

                    var encryptionResult = encryptionProvider.Encrypt(plainText, Encoding.ASCII.GetBytes(rawHeader));
                    return JwtConstants.DirectKeyUseAlg.Equals(encryptingCredentials.Alg) ?
                        string.Join(".", rawHeader, string.Empty, Base64UrlEncoder.Encode(encryptionResult.IV), Base64UrlEncoder.Encode(encryptionResult.Ciphertext), Base64UrlEncoder.Encode(encryptionResult.AuthenticationTag)) :
                        string.Join(".", rawHeader, Base64UrlEncoder.Encode(wrappedKey), Base64UrlEncoder.Encode(encryptionResult.IV), Base64UrlEncoder.Encode(encryptionResult.Ciphertext), Base64UrlEncoder.Encode(encryptionResult.AuthenticationTag));
                }
                catch (Exception ex)
                {
                    throw LogHelper.LogExceptionMessage(new SecurityTokenEncryptionFailedException(LogHelper.FormatInvariant(TokenLogMessages.IDX10616, LogHelper.MarkAsNonPII(encryptingCredentials.Enc), encryptingCredentials.Key), ex));
                }
            }
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
                    keys = [key];
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
    }
}
