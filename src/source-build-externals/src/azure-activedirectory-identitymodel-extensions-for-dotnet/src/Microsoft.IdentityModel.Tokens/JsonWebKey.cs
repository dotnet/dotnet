// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using System.Threading;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens.Json;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Represents a JSON Web Key as defined in https://datatracker.ietf.org/doc/html/rfc7517.
    /// </summary>
    public class JsonWebKey : SecurityKey
    {
        internal const string ClassName = "Microsoft.IdentityModel.Tokens.JsonWebKey";
        private Dictionary<string, object> _additionalData;
        private List<string> _keyOps;
        private List<string> _oth;
        private List<string> _x5c;
        private string _kid;

        /// <summary>
        /// Initializes an new instance of <see cref="JsonWebKey"/>.
        /// </summary>
        public JsonWebKey()
        {
        }

        /// <summary>
        /// Returns a new instance of <see cref="JsonWebKey"/>.
        /// </summary>
        /// <param name="json">A string that contains JSON Web Key parameters in JSON format.</param>
        /// <returns><see cref="JsonWebKey"/></returns>
        /// <exception cref="ArgumentNullException">If 'json' is null or empty.</exception>
        /// <exception cref="ArgumentException">If 'json' fails to deserialize.</exception>
        public static JsonWebKey Create(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw LogHelper.LogArgumentNullException(nameof(json));

            return new JsonWebKey(json);
        }

        /// <summary>
        /// Initializes an new instance of <see cref="JsonWebKey"/> from a json string.
        /// </summary>
        /// <param name="json">A string that contains JSON Web Key parameters in JSON format.</param>
        /// <exception cref="ArgumentNullException">If 'json' is null or empty.</exception>
        /// <exception cref="ArgumentException">If 'json' fails to deserialize.</exception>
        public JsonWebKey(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw LogHelper.LogArgumentNullException(nameof(json));

            try
            {
                if (LogHelper.IsEnabled(EventLogLevel.Verbose))
                    LogHelper.LogVerbose(LogMessages.IDX10806, json, LogHelper.MarkAsNonPII(ClassName));

                JsonWebKeySerializer.Read(json, this);
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10805, json, LogHelper.MarkAsNonPII(ClassName)), ex));
            }
        }

        /// <summary>
        /// If this was converted to or from a SecurityKey, this field will be set.
        /// </summary>
        [JsonIgnore]
        internal SecurityKey ConvertedSecurityKey { get; set; }

        /// <summary>
        /// If this was failed converted to a SecurityKey, this field will be set.
        /// </summary>
        [JsonIgnore]
        internal string ConvertKeyInfo { get; set; }

        /// <summary>
        /// When deserializing from JSON any properties that are not defined will be placed here.
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, object> AdditionalData => _additionalData ??
            Interlocked.CompareExchange(ref _additionalData, new Dictionary<string, object>(StringComparer.Ordinal), null) ??
            _additionalData;

        /// <summary>
        /// Gets or sets the 'alg' (KeyType).
        /// </summary>
        [JsonPropertyName(JsonWebKeyParameterNames.Alg)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string Alg { get; set; }

        /// <summary>
        /// Gets or sets the 'crv' (ECC - Curve).
        /// </summary>
        [JsonPropertyName(JsonWebKeyParameterNames.Crv)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string Crv { get; set; }

        /// <summary>
        /// Gets or sets the 'd' (ECC - Private Key OR RSA - Private Exponent).
        /// </summary>
        /// <remarks>Value is formated as: Base64urlUInt</remarks>
        [JsonPropertyName(JsonWebKeyParameterNames.D)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string D { get; set; }

        /// <summary>
        /// Gets or sets the 'dp' (RSA - First Factor CRT Exponent).
        /// </summary>
        /// <remarks>Value is formated as: Base64urlUInt</remarks>
        [JsonPropertyName(JsonWebKeyParameterNames.DP)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string DP { get; set; }

        /// <summary>
        /// Gets or sets the 'dq' (RSA - Second Factor CRT Exponent).
        /// </summary>
        /// <remarks>Value is formated as: Base64urlUInt</remarks>
        [JsonPropertyName(JsonWebKeyParameterNames.DQ)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string DQ { get; set; }

        /// <summary>
        /// Gets or sets the 'e' (RSA - Exponent).
        /// </summary>
        [JsonPropertyName(JsonWebKeyParameterNames.E)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string E { get; set; }

        /// <summary>
        /// Gets or sets the 'k' (Symmetric - Key Value).
        /// </summary>
        /// Base64urlEncoding
        [JsonPropertyName(JsonWebKeyParameterNames.K)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string K { get; set; }

        /// <summary>
        /// Gets the key id of this <see cref="JsonWebKey"/>.
        /// </summary>
        [JsonIgnore]
        public override string KeyId
        {
            get { return _kid; }
            set { _kid = value; }
        }

        /// <summary>
        /// Gets the 'key_ops' (Key Operations).
        /// </summary>
        [JsonPropertyName(JsonWebKeyParameterNames.KeyOps)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public IList<string> KeyOps => _keyOps ??
            Interlocked.CompareExchange(ref _keyOps, new List<string>(), null) ??
            _keyOps;

        /// <summary>
        /// Gets or sets the 'kid' (Key ID)..
        /// </summary>
        [JsonPropertyName(JsonWebKeyParameterNames.Kid)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string Kid
        {
            get { return _kid; }
            set { _kid = value; }
        }

        /// <summary>
        /// Gets or sets the 'kty' (Key Type).
        /// </summary>
        [JsonPropertyName(JsonWebKeyParameterNames.Kty)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string Kty { get; set; }

        /// <summary>
        /// Gets or sets the 'n' (RSA - Modulus).
        /// </summary>
        /// <remarks>Value is formatted as: Base64urlEncoding</remarks>
        [JsonPropertyName(JsonWebKeyParameterNames.N)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string N { get; set; }

        /// <summary>
        /// Gets or sets the 'oth' (RSA - Other Primes Info).
        /// </summary>
        [JsonPropertyName(JsonWebKeyParameterNames.Oth)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public IList<string> Oth => _oth ??
            Interlocked.CompareExchange(ref _oth, new List<string>(), null) ??
            _oth;

        /// <summary>
        /// Gets or sets the 'p' (RSA - First Prime Factor)..
        /// </summary>
        /// <remarks>Value is formatted as: Base64urlUInt</remarks>
        [JsonPropertyName(JsonWebKeyParameterNames.P)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string P { get; set; }

        /// <summary>
        /// Gets or sets the 'q' (RSA - Second  Prime Factor)..
        /// </summary>
        /// <remarks>Value is formatted as: Base64urlUInt</remarks>
        [JsonPropertyName(JsonWebKeyParameterNames.Q)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string Q { get; set; }

        /// <summary>
        /// Gets or sets the 'qi' (RSA - First CRT Coefficient)..
        /// </summary>
        /// <remarks>Value is formatted as: Base64urlUInt</remarks>
        [JsonPropertyName(JsonWebKeyParameterNames.QI)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string QI { get; set; }

        /// <summary>
        /// Gets or sets the 'use' (Public Key Use)..
        /// </summary>
        [JsonPropertyName(JsonWebKeyParameterNames.Use)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string Use { get; set; }

        /// <summary>
        /// Gets or sets the 'x' (ECC - X Coordinate)..
        /// </summary>
        /// <remarks>Value is formatted as: Base64urlEncoding</remarks>
        [JsonPropertyName(JsonWebKeyParameterNames.X)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string X { get; set; }

        /// <summary>
        /// Gets the 'x5c' collection (X.509 Certificate Chain)..
        /// </summary>
        [JsonPropertyName(JsonWebKeyParameterNames.X5c)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public IList<string> X5c => _x5c ??
            Interlocked.CompareExchange(ref _x5c, new List<string>(), null) ??
            _x5c;

        /// <summary>
        /// Gets or sets the 'x5t' (X.509 Certificate SHA-1 thumbprint)..
        /// </summary>
        [JsonPropertyName(JsonWebKeyParameterNames.X5t)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string X5t { get; set; }

        /// <summary>
        /// Gets or sets the 'x5t#S256' (X.509 Certificate SHA-256 thumbprint)..
        /// </summary>
        [JsonPropertyName(JsonWebKeyParameterNames.X5tS256)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string X5tS256 { get; set; }

        /// <summary>
        /// Gets or sets the 'x5u' (X.509 URL)..
        /// </summary>
        [JsonPropertyName(JsonWebKeyParameterNames.X5u)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string X5u { get; set; }

        /// <summary>
        /// Gets or sets the 'y' (ECC - Y Coordinate)..
        /// </summary>
        /// <remarks>Value is formatted as: Base64urlEncoding</remarks>
        [JsonPropertyName(JsonWebKeyParameterNames.Y)]
#if NET6_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string Y { get; set; }

        /// <summary>
        /// Gets the key size of <see cref="JsonWebKey"/>.
        /// </summary>
        [JsonIgnore]
        public override int KeySize
        {
            get
            {
                if (Kty == JsonWebAlgorithmsKeyTypes.RSA && !string.IsNullOrEmpty(N))
                    return Base64UrlEncoder.DecodeBytes(N).Length * 8;
                else if (Kty == JsonWebAlgorithmsKeyTypes.EllipticCurve && !string.IsNullOrEmpty(X))
                    return Base64UrlEncoder.DecodeBytes(X).Length * 8;
                else if (Kty == JsonWebAlgorithmsKeyTypes.Octet && !string.IsNullOrEmpty(K))
                    return Base64UrlEncoder.DecodeBytes(K).Length * 8;
                else
                    return 0;
            }
        }

        /// <summary>
        /// Gets a bool indicating if a private key exists.
        /// </summary>
        /// <return>true if it has a private key; otherwise, false.</return>
        [JsonIgnore]
        public bool HasPrivateKey
        {
            get
            {
                if (Kty == JsonWebAlgorithmsKeyTypes.RSA)
                    return D != null && DP != null && DQ != null && P != null && Q != null && QI != null;
                else if (Kty == JsonWebAlgorithmsKeyTypes.EllipticCurve)
                    return D != null;
                else
                    return false;
            }
        }

        internal RSAParameters CreateRsaParameters()
        {
            if (string.IsNullOrEmpty(N))
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10700, LogHelper.MarkAsNonPII(ClassName), LogHelper.MarkAsNonPII("Modulus"))));

            if (string.IsNullOrEmpty(E))
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10700, LogHelper.MarkAsNonPII(ClassName), LogHelper.MarkAsNonPII("Exponent"))));

            return new RSAParameters
            {
                Modulus = Base64UrlEncoder.DecodeBytes(N),
                Exponent = Base64UrlEncoder.DecodeBytes(E),
                D = string.IsNullOrEmpty(D) ? null : Base64UrlEncoder.DecodeBytes(D),
                P = string.IsNullOrEmpty(P) ? null : Base64UrlEncoder.DecodeBytes(P),
                Q = string.IsNullOrEmpty(Q) ? null : Base64UrlEncoder.DecodeBytes(Q),
                DP = string.IsNullOrEmpty(DP) ? null : Base64UrlEncoder.DecodeBytes(DP),
                DQ = string.IsNullOrEmpty(DQ) ? null : Base64UrlEncoder.DecodeBytes(DQ),
                InverseQ = string.IsNullOrEmpty(QI) ? null : Base64UrlEncoder.DecodeBytes(QI)
            };
        }

        /// <summary>
        /// Determines whether the <see cref="JsonWebKey"/> can compute a JWK thumbprint.
        /// </summary>
        /// <returns><c>true</c> if JWK thumbprint can be computed; otherwise, <c>false</c>.</returns>
        /// <remarks>https://datatracker.ietf.org/doc/html/rfc7638</remarks>
        public override bool CanComputeJwkThumbprint()
        {
            if (string.IsNullOrEmpty(Kty))
                return false;

            if (string.Equals(Kty, JsonWebAlgorithmsKeyTypes.EllipticCurve))
                return CanComputeECThumbprint();
            else if (string.Equals(Kty, JsonWebAlgorithmsKeyTypes.RSA))
                return CanComputeRsaThumbprint();
            else if (string.Equals(Kty, JsonWebAlgorithmsKeyTypes.Octet))
                return CanComputeOctThumbprint();
            else
                return false;
        }

        /// <summary>
        /// Computes the JWK thumprint per spec: https://datatracker.ietf.org/doc/html/rfc7638 />.
        /// </summary>
        /// <returns>A the JWK thumbprint.</returns>
        public override byte[] ComputeJwkThumbprint()
        {
            if (string.IsNullOrEmpty(Kty))
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10705, LogHelper.MarkAsNonPII(nameof(Kty)))));

            if (string.Equals(Kty, JsonWebAlgorithmsKeyTypes.EllipticCurve))
                return ComputeECThumbprint();
            else if (string.Equals(Kty, JsonWebAlgorithmsKeyTypes.RSA))
                return ComputeRsaThumbprint();
            else if (string.Equals(Kty, JsonWebAlgorithmsKeyTypes.Octet))
                return ComputeOctThumbprint();
            else
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10706, LogHelper.MarkAsNonPII(nameof(Kty)), LogHelper.MarkAsNonPII(string.Join(", ", JsonWebAlgorithmsKeyTypes.EllipticCurve, JsonWebAlgorithmsKeyTypes.RSA, JsonWebAlgorithmsKeyTypes.Octet)), LogHelper.MarkAsNonPII(nameof(Kty)))));
        }

        private bool CanComputeOctThumbprint()
        {
            return !string.IsNullOrEmpty(K);
        }

        private byte[] ComputeOctThumbprint()
        {
            if (string.IsNullOrEmpty(K))
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10705, LogHelper.MarkAsNonPII(nameof(K)))));

            var canonicalJwk = $@"{{""{JsonWebKeyParameterNames.K}"":""{K}"",""{JsonWebKeyParameterNames.Kty}"":""{Kty}""}}";
            return Utility.GenerateSha256Hash(canonicalJwk);
        }

        private bool CanComputeRsaThumbprint()
        {
            return !(string.IsNullOrEmpty(E) || string.IsNullOrEmpty(N));
        }

        private byte[] ComputeRsaThumbprint()
        {
            if (string.IsNullOrEmpty(E))
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10705, LogHelper.MarkAsNonPII(nameof(E)))));

            if (string.IsNullOrEmpty(N))
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10705, LogHelper.MarkAsNonPII(nameof(N)))));

            var canonicalJwk = $@"{{""{JsonWebKeyParameterNames.E}"":""{E}"",""{JsonWebKeyParameterNames.Kty}"":""{Kty}"",""{JsonWebKeyParameterNames.N}"":""{N}""}}";
            return Utility.GenerateSha256Hash(canonicalJwk);
        }

        private bool CanComputeECThumbprint()
        {
            return !(string.IsNullOrEmpty(Crv) || string.IsNullOrEmpty(X) || string.IsNullOrEmpty(Y));
        }

        private byte[] ComputeECThumbprint()
        {
            if (string.IsNullOrEmpty(Crv))
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10705, LogHelper.MarkAsNonPII(nameof(Crv)))));

            if (string.IsNullOrEmpty(X))
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10705, LogHelper.MarkAsNonPII(nameof(X)))));

            if (string.IsNullOrEmpty(Y))
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10705, LogHelper.MarkAsNonPII(nameof(Y)))));

            var canonicalJwk = $@"{{""{JsonWebKeyParameterNames.Crv}"":""{Crv}"",""{JsonWebKeyParameterNames.Kty}"":""{Kty}"",""{JsonWebKeyParameterNames.X}"":""{X}"",""{JsonWebKeyParameterNames.Y}"":""{Y}""}}";
            return Utility.GenerateSha256Hash(canonicalJwk);
        }

        /// <summary>
        /// Creates a JsonWebKey representation of an asymmetric public key.
        /// </summary>
        /// <returns>JsonWebKey representation of an asymmetric public key.</returns>
        /// <remarks>https://datatracker.ietf.org/doc/html/rfc7800#section-3.2</remarks>
        internal string RepresentAsAsymmetricPublicJwk()
        {
            string kid = string.IsNullOrEmpty(Kid) ? "{" : $@"{{""{JsonWebKeyParameterNames.Kid}"":""{Kid}"",";

            if (string.Equals(Kty, JsonWebAlgorithmsKeyTypes.EllipticCurve))
            {
                if (string.IsNullOrEmpty(Crv))
                    throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10708, LogHelper.MarkAsNonPII(nameof(Crv)))));

                if (string.IsNullOrEmpty(X))
                    throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10708, LogHelper.MarkAsNonPII(nameof(X)))));

                if (string.IsNullOrEmpty(Y))
                    throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10708, LogHelper.MarkAsNonPII(nameof(Y)))));

                return  $@"{kid}" +
                        $@"""{JsonWebKeyParameterNames.Crv}"":""{Crv}""," +
                        $@"""{JsonWebKeyParameterNames.Kty}"":""{Kty}""," +
                        $@"""{JsonWebKeyParameterNames.X}"":""{X}""," +
                        $@"""{JsonWebKeyParameterNames.Y}"":""{Y}""}}";
            }
            else if (string.Equals(Kty, JsonWebAlgorithmsKeyTypes.RSA))
            {
                if (string.IsNullOrEmpty(E))
                    throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10709, LogHelper.MarkAsNonPII(nameof(E)))));

                if (string.IsNullOrEmpty(N))
                    throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10709, LogHelper.MarkAsNonPII(nameof(N)))));

                return  $@"{kid}" +
                        $@"""{JsonWebKeyParameterNames.E}"":""{E}""," +
                        $@"""{JsonWebKeyParameterNames.Kty}"":""{Kty}""," +
                        $@"""{JsonWebKeyParameterNames.N}"":""{N}""}}";
            }
            else
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10707, LogHelper.MarkAsNonPII(nameof(Kty)), LogHelper.MarkAsNonPII(string.Join(", ", JsonWebAlgorithmsKeyTypes.EllipticCurve, JsonWebAlgorithmsKeyTypes.RSA)), LogHelper.MarkAsNonPII(nameof(Kty)))));

        }

        /// <summary>
        /// Returns the formatted string: GetType(), Use: 'value', Kid: 'value', Kty: 'value', InternalId: 'value'.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return $"{GetType()}, Use: '{Use}',  Kid: '{Kid}', Kty: '{Kty}', InternalId: '{InternalId}'.";
        }
    }
}

