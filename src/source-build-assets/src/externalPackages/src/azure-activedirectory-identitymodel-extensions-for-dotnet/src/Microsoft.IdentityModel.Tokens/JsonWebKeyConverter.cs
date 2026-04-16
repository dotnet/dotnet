// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Converts a <see cref="SecurityKey"/> into a <see cref="JsonWebKey"/>
    /// Supports: converting to a <see cref="JsonWebKey"/> from one of: <see cref="RsaSecurityKey"/>, <see cref="X509SecurityKey"/>, and <see cref=" SymmetricSecurityKey"/>.
    /// </summary>
    public class JsonWebKeyConverter
    {
        /// <summary>
        /// Converts a <see cref="SecurityKey"/> into a <see cref="JsonWebKey"/>
        /// </summary>
        /// <param name="key">a <see cref="SecurityKey"/> to convert.</param>
        /// <returns>a <see cref="JsonWebKey"/></returns>
        /// <exception cref="ArgumentNullException">if <paramref name="key"/>is null.</exception>
        /// <exception cref="NotSupportedException">if <paramref name="key"/>is not a supported type.</exception>
        /// <remarks>Supports: <see cref="RsaSecurityKey"/>, <see cref="X509SecurityKey"/> and <see cref=" SymmetricSecurityKey"/>.</remarks>
        public static JsonWebKey ConvertFromSecurityKey(SecurityKey key)
        {
            if (key == null)
                throw LogHelper.LogArgumentNullException(nameof(key));

            if (key is RsaSecurityKey rsaKey)
                return ConvertFromRSASecurityKey(rsaKey);
            else if (key is SymmetricSecurityKey symmetricKey)
                return ConvertFromSymmetricSecurityKey(symmetricKey);
            else if (key is X509SecurityKey x509Key)
                return ConvertFromX509SecurityKey(x509Key);
#if NET472 || NETSTANDARD2_0 || NET6_0_OR_GREATER
            else if (key is ECDsaSecurityKey ecdsaSecurityKey)
                return ConvertFromECDsaSecurityKey(ecdsaSecurityKey);
#endif
            else
                throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX10674, LogHelper.MarkAsNonPII(key.GetType().FullName))));
        }

        /// <summary>
        /// Converts a <see cref="RsaSecurityKey"/> into a <see cref="JsonWebKey"/>
        /// </summary>
        /// <param name="key">a <see cref="RsaSecurityKey"/> to convert.</param>
        /// <returns>a <see cref="JsonWebKey"/></returns>
        /// <exception cref="ArgumentNullException">if <paramref name="key"/>is null.</exception>
        public static JsonWebKey ConvertFromRSASecurityKey(RsaSecurityKey key)
        {
            if (key == null)
                throw LogHelper.LogArgumentNullException(nameof(key));

            RSAParameters parameters;
            if (key.Rsa != null)
            {
                try
                {
                    parameters = key.Rsa.ExportParameters(true);
                }
                catch
                {
                    parameters = key.Rsa.ExportParameters(false);
                }
            }
            else
            {
                parameters = key.Parameters;
            }

            return new JsonWebKey
            {
                N = parameters.Modulus != null ? Base64UrlEncoder.Encode(parameters.Modulus) : null,
                E = parameters.Exponent != null ? Base64UrlEncoder.Encode(parameters.Exponent) : null,
                D = parameters.D != null ? Base64UrlEncoder.Encode(parameters.D) : null,
                P = parameters.P != null ? Base64UrlEncoder.Encode(parameters.P) : null,
                Q = parameters.Q != null ? Base64UrlEncoder.Encode(parameters.Q) : null,
                DP = parameters.DP != null ? Base64UrlEncoder.Encode(parameters.DP) : null,
                DQ = parameters.DQ != null ? Base64UrlEncoder.Encode(parameters.DQ) : null,
                QI = parameters.InverseQ != null ? Base64UrlEncoder.Encode(parameters.InverseQ) : null,
                Kty = JsonWebAlgorithmsKeyTypes.RSA,
                Kid = key.KeyId,
                ConvertedSecurityKey = key
            };
        }

        /// <summary>
        /// Converts a <see cref="X509SecurityKey"/> into a <see cref="JsonWebKey"/>
        /// </summary>
        /// <param name="key">a <see cref="X509SecurityKey"/> to convert.</param>
        /// <returns>a <see cref="JsonWebKey"/></returns>
        /// <exception cref="ArgumentNullException">if <paramref name="key"/>is null.</exception>
        public static JsonWebKey ConvertFromX509SecurityKey(X509SecurityKey key)
        {
            if (key == null)
                throw LogHelper.LogArgumentNullException(nameof(key));

            var jsonWebKey = new JsonWebKey
            {
                Kty = JsonWebAlgorithmsKeyTypes.RSA,
                Kid = key.KeyId,
                X5t = key.X5t,
                ConvertedSecurityKey = key
            };

            if (key.Certificate.RawData != null)
                jsonWebKey.X5c.Add(Convert.ToBase64String(key.Certificate.RawData));

            return jsonWebKey;
        }

        /// <summary>
        /// Converts a <see cref="X509SecurityKey"/> into a <see cref="JsonWebKey"/>.
        /// </summary>
        /// <param name="key">a <see cref="X509SecurityKey"/> to convert.</param>
        /// <param name="representAsRsaKey">
        /// <c>true</c> to represent the <paramref name="key"/> as an <see cref="RsaSecurityKey"/>, 
        /// <c>false</c> to represent the <paramref name="key"/> as an <see cref="X509SecurityKey"/>, using the "x5c" parameter.
        /// </param>
        /// <returns>a <see cref="JsonWebKey"/>.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="key"/>is null.</exception>
        public static JsonWebKey ConvertFromX509SecurityKey(X509SecurityKey key, bool representAsRsaKey)
        {
            if (!representAsRsaKey)
                return ConvertFromX509SecurityKey(key);

            if (key == null)
                throw LogHelper.LogArgumentNullException(nameof(key));

            RSA rsaKey;
            if (key.PrivateKeyStatus == PrivateKeyStatus.Exists)
                rsaKey = key.PrivateKey as RSA;
            else
                rsaKey = key.PublicKey as RSA;

            return ConvertFromRSASecurityKey(new RsaSecurityKey(rsaKey) { KeyId = key.KeyId });
        }

        /// <summary>
        /// Converts a <see cref="SymmetricSecurityKey"/> into a <see cref="JsonWebKey"/>
        /// </summary>
        /// <param name="key">a <see cref="SymmetricSecurityKey"/> to convert.</param>
        /// <returns>a <see cref="JsonWebKey"/></returns>
        /// <exception cref="ArgumentNullException">if <paramref name="key"/>is null.</exception>
        public static JsonWebKey ConvertFromSymmetricSecurityKey(SymmetricSecurityKey key)
        {
            if (key == null)
                throw LogHelper.LogArgumentNullException(nameof(key));

            return new JsonWebKey
            {
                K = Base64UrlEncoder.Encode(key.Key),
                Kid = key.KeyId,
                Kty = JsonWebAlgorithmsKeyTypes.Octet,
                ConvertedSecurityKey = key
            };
        }

#if NET472 || NETSTANDARD2_0 || NET6_0_OR_GREATER
        /// <summary>
        /// Converts a <see cref="ECDsaSecurityKey"/> into a <see cref="JsonWebKey"/>
        /// </summary>
        /// <param name="key">an <see cref="ECDsaSecurityKey"/> to convert.</param>
        /// <returns>a <see cref="JsonWebKey"/></returns>
        /// <exception cref="ArgumentNullException">if <paramref name="key"/>is null.</exception>
        public static JsonWebKey ConvertFromECDsaSecurityKey(ECDsaSecurityKey key)
        {
            if (!ECDsaAdapter.SupportsECParameters())
                throw LogHelper.LogExceptionMessage(new PlatformNotSupportedException(LogMessages.IDX10695));

            if (key == null)
                throw LogHelper.LogArgumentNullException(nameof(key));

            if (key.ECDsa == null)
                throw LogHelper.LogArgumentNullException(nameof(key.ECDsa));

            ECParameters parameters;
            try
            {
                parameters = key.ECDsa.ExportParameters(true);
            }
            catch
            {
                parameters = key.ECDsa.ExportParameters(false);
            }

            return new JsonWebKey
            {
                Crv = ECDsaAdapter.GetCrvParameterValue(parameters.Curve),
                X = parameters.Q.X != null ? Base64UrlEncoder.Encode(parameters.Q.X) : null,
                Y = parameters.Q.Y != null ? Base64UrlEncoder.Encode(parameters.Q.Y) : null,
                D = parameters.D != null ? Base64UrlEncoder.Encode(parameters.D) : null,
                Kty = JsonWebAlgorithmsKeyTypes.EllipticCurve,
                Kid = key.KeyId,
                ConvertedSecurityKey = key
            };
        }
#endif

        internal static bool TryConvertToSecurityKey(JsonWebKey webKey, out SecurityKey key)
        {
            if (webKey.ConvertedSecurityKey != null)
            {
                key = webKey.ConvertedSecurityKey;
                return true;
            }

            key = null;
            try
            {
                if (JsonWebAlgorithmsKeyTypes.RSA.Equals(webKey.Kty))
                {
                    if (TryConvertToX509SecurityKey(webKey, out key))
                        return true;

                    if (TryCreateToRsaSecurityKey(webKey, out key))
                        return true;
                }
                else if (JsonWebAlgorithmsKeyTypes.EllipticCurve.Equals(webKey.Kty))
                {
                    return TryConvertToECDsaSecurityKey(webKey, out key);
                }
                else if (JsonWebAlgorithmsKeyTypes.Octet.Equals(webKey.Kty))
                {
                    return TryConvertToSymmetricSecurityKey(webKey, out key);
                }
            }
            catch (Exception ex)
            {
                if (LogHelper.IsEnabled(EventLogLevel.Warning))
                    LogHelper.LogWarning(LogHelper.FormatInvariant(LogMessages.IDX10813, LogHelper.MarkAsNonPII(typeof(SecurityKey)), webKey, ex));
            }

            if (LogHelper.IsEnabled(EventLogLevel.Warning))
                LogHelper.LogWarning(LogHelper.FormatInvariant(LogMessages.IDX10812, LogHelper.MarkAsNonPII(typeof(SecurityKey)), webKey));

            return false;
        }

        internal static bool TryConvertToSymmetricSecurityKey(JsonWebKey webKey, out SecurityKey key)
        {
            if (webKey.ConvertedSecurityKey is SymmetricSecurityKey)
            {
                key = webKey.ConvertedSecurityKey;
                return true;
            }

            key = null;
            if (string.IsNullOrEmpty(webKey.K))
                return false;

            try
            {
                key = new SymmetricSecurityKey(webKey);
                return true;
            }
            catch(Exception ex)
            {
                if (LogHelper.IsEnabled(EventLogLevel.Error))
                    LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(LogMessages.IDX10813, LogHelper.MarkAsNonPII(typeof(SymmetricSecurityKey)), webKey, ex), ex));
            }

            return false;
        }

        internal static bool TryConvertToX509SecurityKey(JsonWebKey webKey, out SecurityKey key)
        {
            if (webKey.ConvertedSecurityKey is X509SecurityKey)
            {
                key = webKey.ConvertedSecurityKey;
                return true;
            }

            key = null;
            if (webKey.X5c == null || webKey.X5c.Count == 0)
                return false;

            try
            {
                // only the first certificate should be used to perform signing operations
                // https://datatracker.ietf.org/doc/html/rfc7517#section-4.7
                key = new X509SecurityKey(webKey);
                return true;
            }
            catch (Exception ex)
            {
                string convertKeyInfo = LogHelper.FormatInvariant(LogMessages.IDX10813, LogHelper.MarkAsNonPII(typeof(X509SecurityKey)), webKey, ex);
                webKey.ConvertKeyInfo = convertKeyInfo;
                if (LogHelper.IsEnabled(EventLogLevel.Error))
                    LogHelper.LogExceptionMessage(new InvalidOperationException(convertKeyInfo, ex));
            }

            return false;
        }

        internal static bool TryCreateToRsaSecurityKey(JsonWebKey webKey, out SecurityKey key)
        {
            if (webKey.ConvertedSecurityKey is RsaSecurityKey)
            {
                key = webKey.ConvertedSecurityKey;
                return true;
            }

            key = null;
            if (string.IsNullOrWhiteSpace(webKey.E) || string.IsNullOrWhiteSpace(webKey.N))
                return false;

            try
            {
                key = new RsaSecurityKey(webKey);
                return true;
            }
            catch (Exception ex)
            {
                string convertKeyInfo = LogHelper.FormatInvariant(LogMessages.IDX10813, LogHelper.MarkAsNonPII(typeof(RsaSecurityKey)), webKey, ex);
                webKey.ConvertKeyInfo = convertKeyInfo;
                if (LogHelper.IsEnabled(EventLogLevel.Error))
                    LogHelper.LogExceptionMessage(new InvalidOperationException(convertKeyInfo, ex));
            }

            return false;
        }

        internal static bool TryConvertToECDsaSecurityKey(JsonWebKey webKey, out SecurityKey key)
        {
            if (webKey.ConvertedSecurityKey is ECDsaSecurityKey)
            {
                key = webKey.ConvertedSecurityKey;
                return true;
            }

            key = null;
            if (string.IsNullOrEmpty(webKey.Crv) || string.IsNullOrEmpty(webKey.X) || string.IsNullOrEmpty(webKey.Y))
            {
                var missingComponent = new List<string>();
                if (string.IsNullOrEmpty(webKey.Crv))
                    missingComponent.Add(JsonWebKeyParameterNames.Crv);

                if (string.IsNullOrEmpty(webKey.X))
                    missingComponent.Add(JsonWebKeyParameterNames.X);

                if (string.IsNullOrEmpty(webKey.Y))
                    missingComponent.Add(JsonWebKeyParameterNames.Y);

                webKey.ConvertKeyInfo = LogHelper.FormatInvariant(LogMessages.IDX10814, LogHelper.MarkAsNonPII(typeof(ECDsaSecurityKey)), webKey, string.Join(", ", missingComponent));
                return false;
            }

            try
            {
                key = new ECDsaSecurityKey(webKey, !string.IsNullOrEmpty(webKey.D));
                return true;
            }
            catch (Exception ex)
            {
                string convertKeyInfo = LogHelper.FormatInvariant(LogMessages.IDX10813, LogHelper.MarkAsNonPII(typeof(ECDsaSecurityKey)), webKey, ex);
                webKey.ConvertKeyInfo = convertKeyInfo;
                if (LogHelper.IsEnabled(EventLogLevel.Error))
                    LogHelper.LogExceptionMessage(new InvalidOperationException(convertKeyInfo, ex));
            }

            return false;
        }
    }
}
