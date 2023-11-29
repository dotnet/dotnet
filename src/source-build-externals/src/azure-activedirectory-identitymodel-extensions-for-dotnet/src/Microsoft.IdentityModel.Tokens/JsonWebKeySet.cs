// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Threading;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens.Json;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Contains a collection of <see cref="JsonWebKey"/> that can be populated from a json string.
    /// </summary>
    /// <remarks>provides support for https://datatracker.ietf.org/doc/html/rfc7517.</remarks>
    public class JsonWebKeySet
    {
        internal const string ClassName = "Microsoft.IdentityModel.Tokens.JsonWebKeySet";
        private Dictionary<string, object> _additionalData;

        /// <summary>
        /// Returns a new instance of <see cref="JsonWebKeySet"/>.
        /// </summary>
        /// <param name="json">a string that contains JSON Web Key parameters in JSON format.</param>
        /// <returns><see cref="JsonWebKeySet"/></returns>
        /// <exception cref="ArgumentNullException">If 'json' is null or empty.</exception>
        /// <exception cref="ArgumentException">If 'json' fails to deserialize.</exception>
        public static JsonWebKeySet Create(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw LogHelper.LogArgumentNullException(nameof(json));

            return new JsonWebKeySet(json);
        }

        /// <summary>
        /// Initializes an new instance of <see cref="JsonWebKeySet"/>.
        /// </summary>
        public JsonWebKeySet()
        {
        }

        /// <summary>
        /// Initializes an new instance of <see cref="JsonWebKeySet"/> from a json string.
        /// </summary>
        /// <param name="json">a json string containing values.</param>
        /// <exception cref="ArgumentNullException">If 'json' is null or empty.</exception>
        /// <exception cref="ArgumentException">If 'json' fails to deserialize.</exception>
        public JsonWebKeySet(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw LogHelper.LogArgumentNullException(nameof(json));

            try
            {
                if (LogHelper.IsEnabled(EventLogLevel.Verbose))
                    LogHelper.LogVerbose(LogMessages.IDX10806, json, LogHelper.MarkAsNonPII(ClassName));

                JsonWebKeySetSerializer.Read(json, this);
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10805, json, LogHelper.MarkAsNonPII(ClassName)), ex));
            }
        }

        /// <summary>
        /// When deserializing from JSON any properties that are not defined will be placed here.
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, object> AdditionalData => _additionalData ??
            Interlocked.CompareExchange(ref _additionalData, new Dictionary<string, object>(StringComparer.Ordinal), null) ??
            _additionalData;

        /// <summary>
        /// Gets the <see cref="IList{JsonWebKey}"/>.
        /// </summary>
        [JsonPropertyName(JsonWebKeySetParameterNames.Keys)]
#if NET8_0_OR_GREATER
        [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
#endif
        public IList<JsonWebKey> Keys { get; } = new List<JsonWebKey>();

        /// <summary>
        /// Default value for the flag that controls whether unresolved JsonWebKeys will be included in the resulting collection of <see cref="GetSigningKeys"/> method.
        /// </summary>
        [DefaultValue(true)]
        public static bool DefaultSkipUnresolvedJsonWebKeys = true;

        /// <summary>
        /// Flag that controls whether unresolved JsonWebKeys will be included in the resulting collection of <see cref="GetSigningKeys"/> method.
        /// </summary>
        [DefaultValue(true)]
        [JsonIgnore]
        public bool SkipUnresolvedJsonWebKeys { get; set; } = DefaultSkipUnresolvedJsonWebKeys;

        /// <summary>
        /// Returns the JsonWebKeys as a <see cref="IList{SecurityKey}"/>.
        /// </summary>
        /// <remarks>
        /// To include unresolved JsonWebKeys in the resulting <see cref="SecurityKey"/> collection, set <see cref="SkipUnresolvedJsonWebKeys"/> to <c>false</c>.
        /// </remarks>
        public IList<SecurityKey> GetSigningKeys()
        {
            var signingKeys = new List<SecurityKey>();
            foreach (var webKey in Keys)
            {
                // skip if "use" (Public Key Use) parameter is not empty or "sig".
                // https://datatracker.ietf.org/doc/html/rfc7517#section-4-2
                if (!string.IsNullOrEmpty(webKey.Use) && !webKey.Use.Equals(JsonWebKeyUseNames.Sig))
                {
                    string convertKeyInfo = LogHelper.FormatInvariant(LogMessages.IDX10808, webKey, webKey.Use);
                    webKey.ConvertKeyInfo = convertKeyInfo;
                    LogHelper.LogInformation(convertKeyInfo);
                    if (!SkipUnresolvedJsonWebKeys)
                        signingKeys.Add(webKey);

                    continue;
                }

                if (JsonWebAlgorithmsKeyTypes.RSA.Equals(webKey.Kty))
                {
                    var rsaKeyResolved = true;

                    // in this case, even though RSA was specified, we can't resolve.
                    if ((webKey.X5c == null || webKey.X5c.Count == 0) && (string.IsNullOrEmpty(webKey.E) && string.IsNullOrEmpty(webKey.N)))
                    {
                        var missingComponent = new List<string> { JsonWebKeyParameterNames.X5c, JsonWebKeyParameterNames.E, JsonWebKeyParameterNames.N };
                        string convertKeyInfo = LogHelper.FormatInvariant(LogMessages.IDX10814, LogHelper.MarkAsNonPII(typeof(RsaSecurityKey)), webKey, LogHelper.MarkAsNonPII(string.Join(", ", missingComponent)));
                        webKey.ConvertKeyInfo = convertKeyInfo;
                        LogHelper.LogInformation(convertKeyInfo);
                        rsaKeyResolved = false;
                    }
                    else
                    {
                        // in this case X509SecurityKey should be resolved.
                        if (IsValidX509SecurityKey(webKey))
                            if (JsonWebKeyConverter.TryConvertToX509SecurityKey(webKey, out SecurityKey securityKey))
                                signingKeys.Add(securityKey);
                            else
                                rsaKeyResolved = false;

                        // in this case RsaSecurityKey should be resolved.
                        if (IsValidRsaSecurityKey(webKey))
                            if (JsonWebKeyConverter.TryCreateToRsaSecurityKey(webKey, out SecurityKey securityKey))
                                signingKeys.Add(securityKey);
                            else
                                rsaKeyResolved = false;
                    }

                    if (!rsaKeyResolved && !SkipUnresolvedJsonWebKeys)
                        signingKeys.Add(webKey);
                }
                else if (JsonWebAlgorithmsKeyTypes.EllipticCurve.Equals(webKey.Kty))
                {
                    if (JsonWebKeyConverter.TryConvertToECDsaSecurityKey(webKey, out SecurityKey securityKey))
                        signingKeys.Add(securityKey);
                    else if (!SkipUnresolvedJsonWebKeys)
                        signingKeys.Add(webKey);
                }
                else
                {
                    string convertKeyInfo = LogHelper.FormatInvariant(LogMessages.IDX10810, webKey);
                    webKey.ConvertKeyInfo = convertKeyInfo;
                    LogHelper.LogInformation(convertKeyInfo);

                    if (!SkipUnresolvedJsonWebKeys)
                        signingKeys.Add(webKey);
                }
            }

            return signingKeys;
        }

        private static bool IsValidX509SecurityKey(JsonWebKey webKey)
        {
            if (webKey.X5c == null || webKey.X5c.Count == 0)
            {
                webKey.ConvertKeyInfo = LogHelper.FormatInvariant(LogMessages.IDX10814, LogHelper.MarkAsNonPII(typeof(X509SecurityKey)), webKey, LogHelper.MarkAsNonPII(JsonWebKeyParameterNames.X5c));
                return false;
            }

            return true;
        }

        private static bool IsValidRsaSecurityKey(JsonWebKey webKey)
        {
            var missingComponent = new List<string>();
            if (string.IsNullOrWhiteSpace(webKey.E))
                missingComponent.Add(JsonWebKeyParameterNames.E);

            if (string.IsNullOrWhiteSpace(webKey.N))
                missingComponent.Add(JsonWebKeyParameterNames.N);

            if (missingComponent.Count > 0)
            {
                string convertKeyInfo = LogHelper.FormatInvariant(LogMessages.IDX10814, LogHelper.MarkAsNonPII(typeof(RsaSecurityKey)), webKey, LogHelper.MarkAsNonPII(string.Join(", ", missingComponent)));
                if (string.IsNullOrEmpty(webKey.ConvertKeyInfo))
                    webKey.ConvertKeyInfo = convertKeyInfo;
                else
                    webKey.ConvertKeyInfo += convertKeyInfo;
            }

            return missingComponent.Count == 0;
        }
    }
}
