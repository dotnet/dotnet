// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

using JsonPrimitives = Microsoft.IdentityModel.Tokens.Json.JsonSerializerPrimitives;

namespace System.IdentityModel.Tokens.Jwt
{
    /// <summary>
    /// Initializes a new instance of <see cref="JwtHeader"/> which contains JSON objects representing the cryptographic operations applied to the JWT and optionally any additional properties of the JWT.
    /// The member names within the JWT Header are referred to as Header Parameter Names.
    /// <para>These names MUST be unique and the values must be <see cref="string"/>(s). The corresponding values are referred to as Header Parameter Values.</para>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable"), System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Serialize not really supported.")]
    public class JwtHeader : Dictionary<string, object>
    {
        internal string ClassName = "System.IdentityModel.Tokens.Jwt.JwtHeader";

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtHeader"/> class. Default string comparer <see cref="StringComparer.Ordinal"/>.
        /// </summary>
        public JwtHeader()
            : base(StringComparer.Ordinal)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtHeader"/> class. Default string comparer <see cref="StringComparer.Ordinal"/>.
        /// </summary>
        internal JwtHeader(string json)
        {
            _ = json ?? throw LogHelper.LogArgumentNullException(nameof(json));

            Utf8JsonReader reader = new(Encoding.UTF8.GetBytes(json));

            if (!JsonPrimitives.IsReaderAtTokenType(ref reader, JsonTokenType.StartObject, true))
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

            while (true)
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = JsonPrimitives.ReadPropertyName(ref reader, ClassName, true);
                    object obj;
                    if (reader.TokenType == JsonTokenType.StartArray)
                        obj = JsonPrimitives.ReadArrayOfObjects(ref reader, propertyName, ClassName);
                    else
                        obj = JsonPrimitives.ReadPropertyValueAsObject(ref reader, propertyName, ClassName);

                     this[propertyName] = obj;
                }
                // We read a JsonTokenType.StartObject above, exiting and positioning reader at next token.
                else if (JsonPrimitives.IsReaderAtTokenType(ref reader, JsonTokenType.EndObject, true))
                    break;
                else if (!reader.Read())
                    break;
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="JwtHeader"/>.
        /// With the Header Parameters:
        /// <para>{ { typ, JWT }, { alg, SigningCredentials.Algorithm } }</para>
        /// </summary>
        /// <param name="signingCredentials"><see cref="SigningCredentials"/> used creating a JWS Compact JSON.</param>
        public JwtHeader(SigningCredentials signingCredentials)
            : this(signingCredentials, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="JwtHeader"/>.
        /// With the Header Parameters:
        /// <para>{ { typ, JWT }, { alg, EncryptingCredentials.Alg }, { enc, EncryptingCredentials.Enc } }</para>
        /// </summary>
        /// <param name="encryptingCredentials"><see cref="EncryptingCredentials"/> used creating a JWE Compact JSON.</param>
        /// <exception cref="ArgumentNullException">If 'encryptingCredentials' is null.</exception>
        public JwtHeader(EncryptingCredentials encryptingCredentials)
            : this(encryptingCredentials, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="JwtHeader"/>.
        /// With the Header Parameters:
        /// <para>{ { typ, JWT }, { alg, SigningCredentials.Algorithm } }</para>
        /// </summary>
        /// <param name="signingCredentials"><see cref="SigningCredentials"/> used when creating a JWS Compact JSON.</param>
        /// <param name="outboundAlgorithmMap">provides a mapping for the 'alg' value so that values are within the JWT namespace.</param>
        public JwtHeader(SigningCredentials signingCredentials, IDictionary<string,string> outboundAlgorithmMap)
            : this(signingCredentials, outboundAlgorithmMap, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="JwtHeader"/>.
        /// With the Header Parameters:
        /// <para>{ { typ, JWT }, { alg, SigningCredentials.Algorithm } }</para>
        /// </summary>
        /// <param name="signingCredentials"><see cref="SigningCredentials"/> used when creating a JWS Compact JSON.</param>
        /// <param name="outboundAlgorithmMap">provides a mapping for the 'alg' value so that values are within the JWT namespace.</param>
        /// <param name="tokenType"> will be added as the value for the 'typ' claim in the header. If it is null or empty <see cref="JwtConstants.HeaderType"/> will be used as token type</param>
        public JwtHeader(SigningCredentials signingCredentials, IDictionary<string, string> outboundAlgorithmMap, string tokenType)
            : this(signingCredentials, outboundAlgorithmMap, tokenType, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="JwtHeader"/>.
        /// With the Header Parameters:
        /// <para>{ { typ, JWT }, { alg, SigningCredentials.Algorithm } }</para>
        /// </summary>
        /// <param name="signingCredentials"><see cref="SigningCredentials"/> used when creating a JWS Compact JSON.</param>
        /// <param name="outboundAlgorithmMap">provides a mapping for the 'alg' value so that values are within the JWT namespace.</param>
        /// <param name="tokenType"> will be added as the value for the 'typ' claim in the header. If it is null or empty <see cref="JwtConstants.HeaderType"/> will be used as token type</param>
        /// <param name="additionalInnerHeaderClaims">Defines the dictionary containing any custom header claims that need to be added to the inner JWT token header.</param>
        public JwtHeader(SigningCredentials signingCredentials, IDictionary<string, string> outboundAlgorithmMap, string tokenType, IDictionary<string, object> additionalInnerHeaderClaims)
            : base(StringComparer.Ordinal)
        {
            if (signingCredentials == null)
                this[JwtHeaderParameterNames.Alg] = SecurityAlgorithms.None;

            else
            {
                if (outboundAlgorithmMap != null && outboundAlgorithmMap.TryGetValue(signingCredentials.Algorithm, out string outboundAlg))
                    Alg = outboundAlg;
                else
                    Alg = signingCredentials.Algorithm;

                if (!string.IsNullOrEmpty(signingCredentials.Key.KeyId))
                    Kid = signingCredentials.Key.KeyId;

                if (signingCredentials is X509SigningCredentials x509SigningCredentials)
                    this[JwtHeaderParameterNames.X5t] = Base64UrlEncoder.Encode(x509SigningCredentials.Certificate.GetCertHash());
            }

            if (string.IsNullOrEmpty(tokenType))
                Typ = JwtConstants.HeaderType;
            else
                Typ = tokenType;

            AddAdditionalClaims(additionalInnerHeaderClaims, false);
            SigningCredentials = signingCredentials;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="JwtHeader"/>.
        /// With the Header Parameters:
        /// <para>{ { typ, JWT }, { alg, EncryptingCredentials.Algorithm } }</para>
        /// </summary>
        /// <param name="encryptingCredentials"><see cref="EncryptingCredentials"/> used when creating a JWS Compact JSON.</param>
        /// <param name="outboundAlgorithmMap">provides a mapping for the 'alg' value so that values are within the JWT namespace.</param>
        /// <exception cref="ArgumentNullException">If 'encryptingCredentials' is null.</exception>
        public JwtHeader(EncryptingCredentials encryptingCredentials, IDictionary<string, string> outboundAlgorithmMap)
            : this(encryptingCredentials, outboundAlgorithmMap, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="JwtHeader"/>.
        /// With the Header Parameters:
        /// <para>{ { typ, JWT }, { alg, EncryptingCredentials.Algorithm } }</para>
        /// </summary>
        /// <param name="encryptingCredentials"><see cref="EncryptingCredentials"/> used when creating a JWS Compact JSON.</param>
        /// <param name="outboundAlgorithmMap">provides a mapping for the 'alg' value so that values are within the JWT namespace.</param>
        /// <param name="tokenType"> provides the token type</param>
        /// <exception cref="ArgumentNullException">If 'encryptingCredentials' is null.</exception>
        public JwtHeader(EncryptingCredentials encryptingCredentials, IDictionary<string, string> outboundAlgorithmMap, string tokenType)
            : this(encryptingCredentials, outboundAlgorithmMap, tokenType, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="JwtHeader"/>.
        /// With the Header Parameters:
        /// <para>{ { typ, JWT }, { alg, EncryptingCredentials.Algorithm } }</para>
        /// </summary>
        /// <param name="encryptingCredentials"><see cref="EncryptingCredentials"/> used when creating a JWS Compact JSON.</param>
        /// <param name="outboundAlgorithmMap">provides a mapping for the 'alg' value so that values are within the JWT namespace.</param>
        /// <param name="tokenType"> provides the token type</param>
        /// <param name="additionalHeaderClaims">Defines the dictionary containing any custom header claims that need to be added to the outer JWT token header.</param>
        /// <exception cref="ArgumentNullException">If 'encryptingCredentials' is null.</exception>
        public JwtHeader(EncryptingCredentials encryptingCredentials, IDictionary<string, string> outboundAlgorithmMap, string tokenType, IDictionary<string, object> additionalHeaderClaims)
            : base(StringComparer.Ordinal)
        {
            if (encryptingCredentials == null)
                throw LogHelper.LogArgumentNullException(nameof(encryptingCredentials));

            string outboundAlg;
            if (outboundAlgorithmMap != null && outboundAlgorithmMap.TryGetValue(encryptingCredentials.Alg, out outboundAlg))
                Alg = outboundAlg;
            else
                Alg = encryptingCredentials.Alg;

            if (outboundAlgorithmMap != null && outboundAlgorithmMap.TryGetValue(encryptingCredentials.Enc, out outboundAlg))
                Enc = outboundAlg;
            else
                Enc = encryptingCredentials.Enc;

            // Since developers may have already worked around this issue, implicitly taking a dependency on the
            // old behavior, we guard the new behavior behind an AppContext switch. The new/RFC-conforming behavior
            // is treated as opt-in. When the library is at the point where it is able to make breaking changes
            // (such as the next major version update) we should consider whether or not this app-compat switch
            // needs to be maintained.
            if (AppContext.TryGetSwitch(AppCompatSwitches.UseRfcDefinitionOfEpkAndKid, out bool isEnabled) && isEnabled)
            {
                if (!string.IsNullOrEmpty(encryptingCredentials.KeyExchangePublicKey.KeyId))
                    Kid = encryptingCredentials.KeyExchangePublicKey.KeyId;

                // Parameter MUST be present [...] when [key agreement] algorithms are used: https://www.rfc-editor.org/rfc/rfc7518#section-4.6.1.1
                if (SupportedAlgorithms.EcdsaWrapAlgorithms.Contains(encryptingCredentials.Alg))
                    Add(JwtHeaderParameterNames.Epk, JsonWebKeyConverter.ConvertFromSecurityKey(encryptingCredentials.Key).RepresentAsAsymmetricPublicJwk());
            }
            else
            {
                if (!string.IsNullOrEmpty(encryptingCredentials.Key.KeyId))
                    Kid = encryptingCredentials.Key.KeyId;
            }

            if (string.IsNullOrEmpty(tokenType))
                Typ = JwtConstants.HeaderType;
            else
                Typ = tokenType;

            AddAdditionalClaims(additionalHeaderClaims, encryptingCredentials.SetDefaultCtyClaim);
            EncryptingCredentials = encryptingCredentials;
        }

        /// <summary>
        /// Gets the signature algorithm that was used to create the signature.
        /// </summary>
        /// <remarks>If the signature algorithm is not found, null is returned.</remarks>
        public string Alg
        {
            get
            {
                return GetStandardClaim(JwtHeaderParameterNames.Alg);
            }

            private set
            {
                this[JwtHeaderParameterNames.Alg] = value;
            }
        }

        /// <summary>
        /// Gets the content mime type (Cty) of the token.
        /// </summary>
        /// <remarks>If the content mime type is not found, null is returned.</remarks>
        public string Cty
        {
            get
            {
                return GetStandardClaim(JwtHeaderParameterNames.Cty);
            }

            private set
            {
                this[JwtHeaderParameterNames.Cty] = value;
            }
        }

        /// <summary>
        /// Gets the encryption algorithm (Enc) of the token.
        /// </summary>
        /// <remarks>If the content mime type is not found, null is returned.</remarks>
        public string Enc
        {
            get
            {
                return GetStandardClaim(JwtHeaderParameterNames.Enc);
            }

            private set
            {
                this[JwtHeaderParameterNames.Enc] = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="EncryptingCredentials"/> passed in the constructor.
        /// </summary>
        /// <remarks>This value may be null.</remarks>
        public EncryptingCredentials EncryptingCredentials { get; private set; }

        /// <summary>
        /// Gets the iv of symmetric key wrap.
        /// </summary>
        public string IV
        {
            get
            {
                return GetStandardClaim(JwtHeaderParameterNames.IV);
            }
        }

        /// <summary>
        /// Gets the key identifier for the security key used to sign the token
        /// </summary>
        public string Kid
        {
            get
            {
                return GetStandardClaim(JwtHeaderParameterNames.Kid);
            }

            private set
            {
                this[JwtHeaderParameterNames.Kid] = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="SigningCredentials"/> passed in the constructor.
        /// </summary>
        /// <remarks>This value may be null.</remarks>
        public SigningCredentials SigningCredentials
        {
            get; private set;
        }

        /// <summary>
        /// Gets the mime type (Typ) of the token.
        /// </summary>
        /// <remarks>If the mime type is not found, null is returned.</remarks>
        public string Typ
        {
            get
            {
                return GetStandardClaim(JwtHeaderParameterNames.Typ);
            }

            private set
            {
                this[JwtHeaderParameterNames.Typ] = value;
            }
        }

        /// <summary>
        /// Gets the thumbprint of the certificate used to sign the token
        /// </summary>
        public string X5t
        {
            get
            {
                return GetStandardClaim(JwtHeaderParameterNames.X5t);
            }
        }

        /// <summary>
        /// Gets the certificate used to sign the token
        /// </summary>
        /// <remarks>If the 'x5c' claim is not found, null is returned.</remarks>
        public string X5c => GetStandardClaim(JwtHeaderParameterNames.X5c);

        /// <summary>
        /// Gets the 'value' of the 'zip' claim { zip, 'value' }.
        /// </summary>
        /// <remarks>If the 'zip' claim is not found, null is returned.</remarks>
        public string Zip => GetStandardClaim(JwtHeaderParameterNames.Zip);

        /// <summary>
        /// Deserializes Base64UrlEncoded JSON into a <see cref="JwtHeader"/> instance.
        /// </summary>
        /// <param name="base64UrlEncodedJsonString">Base64url encoded JSON to deserialize.</param>
        /// <returns>An instance of <see cref="JwtHeader"/>.</returns>
        public static JwtHeader Base64UrlDeserialize(string base64UrlEncodedJsonString)
        {
            _ = base64UrlEncodedJsonString ?? throw LogHelper.LogArgumentNullException(nameof(base64UrlEncodedJsonString));

            return new JwtHeader(Base64UrlEncoder.Decode(base64UrlEncodedJsonString));
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
        /// Gets a standard claim from the header.
        /// A standard claim is either a string or a value of another type serialized in JSON format.
        /// </summary>
        /// <param name="claimType">The key of the claim.</param>
        /// <returns>The standard claim string; or null if not found.</returns>
        internal string GetStandardClaim(string claimType)
        {
            if (TryGetValue(claimType, out object value))
            {
                if (value == null)
                    return null;

                if (value is string str)
                    return str;

                if (value is JsonElement jsonElement)
                    return jsonElement.ToString();
                else if (value is IList<string> list)
                {
                    JsonElement json = JsonPrimitives.CreateJsonElement(list);
                    return json.ToString();
                }
                else if (value is IList<object> objectList)
                {
                    var stringList = new List<string>(objectList.Count);
                    foreach (object item in objectList)
                    {
                        if (item is string strItem)
                            stringList.Add(strItem);
                        else
                        {
                            // It isn't safe to ToString() an arbitrary object, so we throw here.
                            // We could end up with a string that doesn't represent the object's value, for example a collection type.
                            throw LogHelper.LogExceptionMessage(
                                new JsonException(
                                    LogHelper.FormatInvariant(
                                    Microsoft.IdentityModel.Tokens.LogMessages.IDX11026,
                                    LogHelper.MarkAsNonPII(claimType),
                                    LogHelper.MarkAsNonPII(item.GetType()))));
                        }
                    }
                    JsonElement json = JsonPrimitives.CreateJsonElement(stringList);
                    return json.ToString();
                }

                // TODO - review dev
                return string.Empty;
            }

            return null;
        }

        internal void AddAdditionalClaims(IDictionary<string, object> additionalHeaderClaims, bool setDefaultCtyClaim)
        {
            if (additionalHeaderClaims?.Count > 0 && additionalHeaderClaims.Keys.Intersect(DefaultHeaderParameters, StringComparer.OrdinalIgnoreCase).Any())
                throw LogHelper.LogExceptionMessage(new SecurityTokenException(LogHelper.FormatInvariant(LogMessages.IDX12742, nameof(additionalHeaderClaims), string.Join(", ", DefaultHeaderParameters))));

            if (additionalHeaderClaims != null)
            {
                if (!additionalHeaderClaims.TryGetValue(JwtHeaderParameterNames.Cty, out _) && setDefaultCtyClaim)
                    Cty = JwtConstants.HeaderType;

                foreach (string claim in additionalHeaderClaims.Keys)
                    this[claim] = additionalHeaderClaims[claim];
            }
            else if (setDefaultCtyClaim)
                Cty = JwtConstants.HeaderType;
        }

        internal static IList<string> DefaultHeaderParameters = new List<string>()
        {
            JwtHeaderParameterNames.Alg,
            JwtHeaderParameterNames.Kid,
            JwtHeaderParameterNames.X5t,
            JwtHeaderParameterNames.Enc,
            JwtHeaderParameterNames.Zip
        };

        /// <summary>
        /// Serializes this instance to JSON.
        /// </summary>
        /// <returns>This instance as JSON.</returns>
        public virtual string SerializeToJson()
        {
            // TODO - common method for JwtPayload and JwtHeader
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Utf8JsonWriter writer = null;

                try
                {
                    writer = new Utf8JsonWriter(memoryStream, new JsonWriterOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                    writer.WriteStartObject();

                    JsonPrimitives.WriteObjects(ref writer, this);

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
    }
}
