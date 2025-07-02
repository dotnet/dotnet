// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.IdentityModel.Tokens.Json.Tests
{
    /// <summary>
    /// Represents a JSON Web Key as defined in https://datatracker.ietf.org/doc/html/rfc7517.
    /// This is the original JsonWebKey in the 6x branch.
    /// Used for ensuring backcompat.
    /// </summary>
    [JsonObject]
    public class JsonWebKey6x : SecurityKey6x
    {
        private string _kid;
        private const string _className = "Microsoft.IdentityModel.Tokens.JsonWebKey6x";

        /// <summary>
        /// Returns a new instance of <see cref="JsonWebKey6x"/>.
        /// </summary>
        /// <param name="json">A string that contains JSON Web Key parameters in JSON format.</param>
        /// <returns><see cref="JsonWebKey6x"/></returns>
        /// <exception cref="ArgumentNullException">If 'json' is null or empty.</exception>
        /// <exception cref="ArgumentException">If 'json' fails to deserialize.</exception>
        static public JsonWebKey6x Create(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw LogHelper.LogArgumentNullException(nameof(json));

            return new JsonWebKey6x(json);
        }

        /// <summary>
        /// Initializes an new instance of <see cref="JsonWebKey6x"/>.
        /// </summary>
        public JsonWebKey6x()
        {
        }

        /// <summary>
        /// Initializes an new instance of <see cref="JsonWebKey6x"/> from a json string.
        /// </summary>
        /// <param name="json">A string that contains JSON Web Key parameters in JSON format.</param>
        /// <exception cref="ArgumentNullException">If 'json' is null or empty.</exception>
        /// <exception cref="ArgumentException">If 'json' fails to deserialize.</exception>
        public JsonWebKey6x(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw LogHelper.LogArgumentNullException(nameof(json));

            try
            {
                LogHelper.LogVerbose(LogMessages.IDX10806, json, LogHelper.MarkAsNonPII(_className));
                JsonConvert.PopulateObject(json, this);
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10805, json, LogHelper.MarkAsNonPII(_className)), ex));
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
        public virtual IDictionary<string, object> AdditionalData { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the 'alg' (KeyType)..
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.Alg, Required = Required.Default)]
        public string Alg { get; set; }

        /// <summary>
        /// Gets or sets the 'crv' (ECC - Curve)..
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.Crv, Required = Required.Default)]
        public string Crv { get; set; }

        /// <summary>
        /// Gets or sets the 'd' (ECC - Private Key OR RSA - Private Exponent)..
        /// </summary>
        /// <remarks>Value is formated as: Base64urlUInt</remarks>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.D, Required = Required.Default)]
        public string D { get; set; }

        /// <summary>
        /// Gets or sets the 'dp' (RSA - First Factor CRT Exponent)..
        /// </summary>
        /// <remarks>Value is formated as: Base64urlUInt</remarks>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.DP, Required = Required.Default)]
        public string DP { get; set; }

        /// <summary>
        /// Gets or sets the 'dq' (RSA - Second Factor CRT Exponent)..
        /// </summary>
        /// <remarks>Value is formated as: Base64urlUInt</remarks>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.DQ, Required = Required.Default)]
        public string DQ { get; set; }

        /// <summary>
        /// Gets or sets the 'e' (RSA - Exponent)..
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.E, Required = Required.Default)]
        public string E { get; set; }

        /// <summary>
        /// Gets or sets the 'k' (Symmetric - Key Value)..
        /// </summary>
        /// Base64urlEncoding
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.K, Required = Required.Default)]
        public string K { get; set; }

        /// <summary>
        /// Gets the key id of this <see cref="JsonWebKey6x"/>.
        /// </summary>
        [JsonIgnore]
        public override string KeyId
        {
            get { return _kid; }
            set { _kid = value; }
        }

        /// <summary>
        /// Gets the 'key_ops' (Key Operations)..
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.KeyOps, Required = Required.Default)]
        public IList<string> KeyOps { get; private set; } = new List<string>();

        /// <summary>
        /// Gets or sets the 'kid' (Key ID)..
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.Kid, Required = Required.Default)]
        public string Kid
        {
            get { return _kid; }
            set { _kid = value; }
        }

        /// <summary>
        /// Gets or sets the 'kty' (Key Type)..
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.Kty, Required = Required.Default)]
        public string Kty { get; set; }

        /// <summary>
        /// Gets or sets the 'n' (RSA - Modulus)..
        /// </summary>
        /// <remarks>Value is formated as: Base64urlEncoding</remarks>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.N, Required = Required.Default)]
        public string N { get; set; }

        /// <summary>
        /// Gets or sets the 'oth' (RSA - Other Primes Info)..
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.Oth, Required = Required.Default)]
        public IList<string> Oth { get; set; }

        /// <summary>
        /// Gets or sets the 'p' (RSA - First Prime Factor)..
        /// </summary>
        /// <remarks>Value is formated as: Base64urlUInt</remarks>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.P, Required = Required.Default)]
        public string P { get; set; }

        /// <summary>
        /// Gets or sets the 'q' (RSA - Second  Prime Factor)..
        /// </summary>
        /// <remarks>Value is formated as: Base64urlUInt</remarks>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.Q, Required = Required.Default)]
        public string Q { get; set; }

        /// <summary>
        /// Gets or sets the 'qi' (RSA - First CRT Coefficient)..
        /// </summary>
        /// <remarks>Value is formated as: Base64urlUInt</remarks>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.QI, Required = Required.Default)]
        public string QI { get; set; }

        /// <summary>
        /// Gets or sets the 'use' (Public Key Use)..
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.Use, Required = Required.Default)]
        public string Use { get; set; }

        /// <summary>
        /// Gets or sets the 'x' (ECC - X Coordinate)..
        /// </summary>
        /// <remarks>Value is formated as: Base64urlEncoding</remarks>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.X, Required = Required.Default)]
        public string X { get; set; }

        /// <summary>
        /// Gets the 'x5c' collection (X.509 Certificate Chain)..
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.X5c, Required = Required.Default)]
        public IList<string> X5c { get; } = new List<string>();

        /// <summary>
        /// Gets or sets the 'x5t' (X.509 Certificate SHA-1 thumbprint)..
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.X5t, Required = Required.Default)]
        public string X5t { get; set; }

        /// <summary>
        /// Gets or sets the 'x5t#S256' (X.509 Certificate SHA-1 thumbprint)..
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.X5tS256, Required = Required.Default)]
        public string X5tS256 { get; set; }

        /// <summary>
        /// Gets or sets the 'x5u' (X.509 URL)..
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.X5u, Required = Required.Default)]
        public string X5u { get; set; }

        /// <summary>
        /// Gets or sets the 'y' (ECC - Y Coordinate)..
        /// </summary>
        /// <remarks>Value is formated as: Base64urlEncoding</remarks>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, PropertyName = JsonWebKeyParameterNames.Y, Required = Required.Default)]
        public string Y { get; set; }

        /// <summary>
        /// Gets the key size of <see cref="JsonWebKey6x"/>.
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

        /// <summary>
        /// Gets a bool that determines if the 'AdditionalData' property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'key_ops' (Key Operations) is not empty; otherwise, false.</return>
        public bool ShouldSerializeAdditionalData()
        {
            return AdditionalData.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'key_ops' (Key Operations) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        /// </summary>
        /// <return>true if 'key_ops' (Key Operations) is not empty; otherwise, false.</return>
        public bool ShouldSerializeKeyOps()
        {
            return KeyOps.Count > 0;
        }

        /// <summary>
        /// Gets a bool that determines if the 'x5c' collection (X.509 Certificate Chain) property should be serialized.
        /// This is used by Json.NET in order to conditionally serialize properties.
        ///</summary>
        /// <return>true if 'x5c' collection (X.509 Certificate Chain) is not empty; otherwise, false.</return>
        public bool ShouldSerializeX5c()
        {
            return X5c.Count > 0;
        }

        internal RSAParameters CreateRsaParameters()
        {
            if (string.IsNullOrEmpty(N))
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10700, LogHelper.MarkAsNonPII(_className), LogHelper.MarkAsNonPII("Modulus"))));

            if (string.IsNullOrEmpty(E))
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10700, LogHelper.MarkAsNonPII(_className), LogHelper.MarkAsNonPII("Exponent"))));

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
        /// Determines whether the <see cref="JsonWebKey6x"/> can compute a JWK thumbprint.
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
        /// Computes a sha256 hash over the <see cref="JsonWebKey6x"/>.
        /// </summary>
        /// <returns>A JWK thumbprint.</returns>
        /// <remarks>https://datatracker.ietf.org/doc/html/rfc7638</remarks>
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
        /// Creates a JsonWebKey6x representation of an asymmetric public key.
        /// </summary>
        /// <returns>JsonWebKey6x representation of an asymmetric public key.</returns>
        /// <remarks>https://datatracker.ietf.org/doc/html/rfc7800#section-3.2</remarks>
        internal string RepresentAsAsymmetricPublicJwk()
        {
            JObject jwk = new JObject();

            if (!string.IsNullOrEmpty(Kid))
                jwk.Add(JsonWebKeyParameterNames.Kid, Kid);

            if (string.Equals(Kty, JsonWebAlgorithmsKeyTypes.EllipticCurve))
                PopulateWithPublicEcParams(jwk);
            else if (string.Equals(Kty, JsonWebAlgorithmsKeyTypes.RSA))
                PopulateWithPublicRsaParams(jwk);
            else
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10707, LogHelper.MarkAsNonPII(nameof(Kty)), LogHelper.MarkAsNonPII(string.Join(", ", JsonWebAlgorithmsKeyTypes.EllipticCurve, JsonWebAlgorithmsKeyTypes.RSA)), LogHelper.MarkAsNonPII(nameof(Kty)))));

            return jwk.ToString(Formatting.None);
        }

        private void PopulateWithPublicEcParams(JObject jwk)
        {
            if (string.IsNullOrEmpty(Crv))
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10708, LogHelper.MarkAsNonPII(nameof(Crv)))));

            if (string.IsNullOrEmpty(X))
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10708, LogHelper.MarkAsNonPII(nameof(X)))));

            if (string.IsNullOrEmpty(Y))
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10708, LogHelper.MarkAsNonPII(nameof(Y)))));

            jwk.Add(JsonWebKeyParameterNames.Crv, Crv);
            jwk.Add(JsonWebKeyParameterNames.Kty, Kty);
            jwk.Add(JsonWebKeyParameterNames.X, X);
            jwk.Add(JsonWebKeyParameterNames.Y, Y);
        }

        private void PopulateWithPublicRsaParams(JObject jwk)
        {
            if (string.IsNullOrEmpty(E))
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10709, LogHelper.MarkAsNonPII(nameof(E)))));

            if (string.IsNullOrEmpty(N))
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10709, LogHelper.MarkAsNonPII(nameof(N)))));

            jwk.Add(JsonWebKeyParameterNames.E, E);
            jwk.Add(JsonWebKeyParameterNames.Kty, Kty);
            jwk.Add(JsonWebKeyParameterNames.N, N);
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
