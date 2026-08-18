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
#if NET472_OR_GREATER || NETSTANDARD2_0 || NET
            else if (key is ECDsaSecurityKey ecdsaSecurityKey)
                return ConvertFromECDsaSecurityKey(ecdsaSecurityKey);
#endif
            else if (key is MlDsaSecurityKey mlDsaSecurityKey)
                return ConvertFromMlDsaSecurityKey(mlDsaSecurityKey);
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

            // ML-DSA certificates: PublicKey (AsymmetricAlgorithm) is null because MLDsa
            // does not inherit from AsymmetricAlgorithm. Route via MlDsaPublicKey instead.
            if (key.MlDsaPublicKey != null)
            {
                string alg = MlDsaSecurityKey.GetAlgorithmName(key.MlDsaPublicKey.Algorithm);
                var jsonWebKey = new JsonWebKey
                {
                    Kty = JsonWebAlgorithmsKeyTypes.Akp,
                    Alg = alg, // REQUIRED for AKP keys per draft-ietf-cose-dilithium (RFC 9964 pending)
                    Kid = key.KeyId,
                    X5t = key.X5t,
                    ConvertedSecurityKey = key
                };

                if (key.Certificate.RawData != null)
                    jsonWebKey.X5c.Add(Convert.ToBase64String(key.Certificate.RawData));

                return jsonWebKey;
            }

            var kty = key.PublicKey switch
            {
                RSA => JsonWebAlgorithmsKeyTypes.RSA,
                ECDsa => JsonWebAlgorithmsKeyTypes.EllipticCurve,
                _ => throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX10674, LogHelper.MarkAsNonPII(key.GetType().FullName))))
            };

            var jwk = new JsonWebKey
            {
                Kty = kty,
                Kid = key.KeyId,
                X5t = key.X5t,
                ConvertedSecurityKey = key
            };

            if (key.Certificate.RawData != null)
                jwk.X5c.Add(Convert.ToBase64String(key.Certificate.RawData));

            return jwk;
        }

        /// <summary>
        /// Converts a <see cref="X509SecurityKey"/> into a <see cref="JsonWebKey"/>.
        /// </summary>
        /// <param name="key">a <see cref="X509SecurityKey"/> to convert.</param>
        /// <param name="representAsRsaKey">
        /// <c>true</c> to extract the key material (RSA, ECDsa, or ML-DSA) from the <paramref name="key"/>,
        /// <c>false</c> to represent the <paramref name="key"/> as an <see cref="X509SecurityKey"/>, using the "x5c" parameter.
        /// </param>
        /// <returns>a <see cref="JsonWebKey"/>.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="key"/>is null.</exception>
        public static JsonWebKey ConvertFromX509SecurityKey(X509SecurityKey key, bool representAsRsaKey)
        {
            if (key == null)
                throw LogHelper.LogArgumentNullException(nameof(key));

            if (!representAsRsaKey)
                return ConvertFromX509SecurityKey(key);

            // ML-DSA: extract key material directly from the X509SecurityKey's cached MLDsa
            // instance rather than wrapping in a temporary MlDsaSecurityKey. This avoids an
            // unnecessary allocation and keeps the data flow explicit — we only need the raw
            // byte exports (public key, seed), not a full SecurityKey wrapper.
            if (key.MlDsaPublicKey != null)
            {
                MLDsa mlDsa = key.PrivateKeyStatus == PrivateKeyStatus.Exists
                    ? key.MlDsaPrivateKey
                    : key.MlDsaPublicKey;

                string alg = MlDsaSecurityKey.GetAlgorithmName(mlDsa.Algorithm);
                byte[] publicKey = mlDsa.ExportMLDsaPublicKey();

                var jsonWebKey = new JsonWebKey
                {
                    Kty = JsonWebAlgorithmsKeyTypes.Akp,
                    Alg = alg,
                    Pub = Base64UrlEncoder.Encode(publicKey),
                    Kid = key.KeyId
                };

                if (key.PrivateKeyStatus == PrivateKeyStatus.Exists)
                {
                    byte[] seed = mlDsa.ExportMLDsaPrivateSeed();
                    try
                    {
                        jsonWebKey.Priv = Base64UrlEncoder.Encode(seed);
                    }
                    finally
                    {
                        CryptographicOperations.ZeroMemory(seed);
                    }
                }

                return jsonWebKey;
            }

            if (key.PrivateKeyStatus == PrivateKeyStatus.Exists)
            {
                if (key.PrivateKey is RSA rsaPrivateKey)
                {
                    return ConvertFromRSASecurityKey(new RsaSecurityKey(rsaPrivateKey) { KeyId = key.KeyId });
                }
#if NET472_OR_GREATER || NETSTANDARD2_0 || NET
                else if (key.PrivateKey is ECDsa ecdsaPrivateKey)
                {
                    return ConvertFromECDsaSecurityKey(new ECDsaSecurityKey(ecdsaPrivateKey) { KeyId = key.KeyId });
                }
#endif
            }
            else if (key.PublicKey is RSA rsaPublicKey)
            {
                return ConvertFromRSASecurityKey(new RsaSecurityKey(rsaPublicKey) { KeyId = key.KeyId });
            }
#if NET472_OR_GREATER || NETSTANDARD2_0 || NET
            else if (key.PublicKey is ECDsa ecdsaPublicKey)
            {
                return ConvertFromECDsaSecurityKey(new ECDsaSecurityKey(ecdsaPublicKey) { KeyId = key.KeyId });
            }
#endif

            throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX10674, LogHelper.MarkAsNonPII(key.GetType().FullName))));
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

#if NET472_OR_GREATER || NETSTANDARD2_0 || NET
        /// <summary>
        /// Converts a <see cref="ECDsaSecurityKey"/> into a <see cref="JsonWebKey"/>
        /// </summary>
        /// <param name="key">an <see cref="ECDsaSecurityKey"/> to convert.</param>
        /// <returns>a <see cref="JsonWebKey"/></returns>
        /// <exception cref="ArgumentNullException">if <paramref name="key"/>is null.</exception>
        public static JsonWebKey ConvertFromECDsaSecurityKey(ECDsaSecurityKey key)
        {
            if (key == null)
                throw LogHelper.LogArgumentNullException(nameof(key));

            if (key.ECDsa == null)
                throw LogHelper.LogArgumentNullException(nameof(key.ECDsa));

            if (!ECDsaAdapter.SupportsECParameters())
                throw LogHelper.LogExceptionMessage(new PlatformNotSupportedException(LogMessages.IDX10695));

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

        /// <summary>
        /// This will attempt to convert the <see cref="JsonWebKey"/> to a <see cref="SecurityKey"/>.
        /// </summary>
        /// <param name="webKey"><see cref="JsonWebKey"/></param>
        /// <param name="key"><see cref="SecurityKey"/></param>
        public static bool TryConvertToSecurityKey(JsonWebKey webKey, out SecurityKey key)
        {
            if (webKey == null)
                throw LogHelper.LogArgumentNullException(nameof(webKey));

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
                else if (JsonWebAlgorithmsKeyTypes.Akp.Equals(webKey.Kty))
                {
                    // alg is REQUIRED for all AKP keys per draft-ietf-cose-dilithium (RFC 9964 pending).
                    if (string.IsNullOrEmpty(webKey.Alg))
                        return false;

                    // Only proceed if the alg is a supported AKP algorithm.
                    if (!SupportedAlgorithms.IsSupportedMlDsaAlgorithm(webKey.Alg))
                        return false;

                    // AKP JWKs with x5c contain a certificate — convert to X509SecurityKey.
                    // The certificate must contain an ML-DSA key matching the claimed alg.
                    if (webKey.X5c != null && webKey.X5c.Count > 0)
                    {
                        if (!TryConvertToX509SecurityKey(webKey, out key))
                            return false;

                        if (key is not X509SecurityKey x509Key
                            || x509Key.MlDsaPublicKey == null
                            || MlDsaSecurityKey.GetAlgorithmName(x509Key.MlDsaPublicKey.Algorithm) != webKey.Alg)
                        {
                            // Dispose the materialized MLDsa handle to avoid leaking
                            // the native SymCrypt handle to the finalizer queue.
                            if (key is X509SecurityKey rejectedKey)
                                rejectedKey.MlDsaPublicKey?.Dispose();

                            key = null;
                            webKey.ConvertedSecurityKey = null;
                            return false;
                        }

                        return true;
                    }

                    return TryConvertToMlDsaSecurityKey(webKey, out key);
                }
            }
            catch (Exception ex)
            {
                if (LogHelper.IsEnabled(EventLogLevel.Warning))
                    LogHelper.LogWarning(LogHelper.FormatInvariant(LogMessages.IDX10813, LogHelper.MarkAsNonPII(typeof(SecurityKey)), LogHelper.MarkAsNonPII(webKey.KeyId), ex));
            }

            if (LogHelper.IsEnabled(EventLogLevel.Warning))
                LogHelper.LogWarning(LogHelper.FormatInvariant(LogMessages.IDX10812, LogHelper.MarkAsNonPII(typeof(SecurityKey)), LogHelper.MarkAsNonPII(webKey.KeyId)));

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
            catch (Exception ex)
            {
                if (LogHelper.IsEnabled(EventLogLevel.Error))
                    LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(LogMessages.IDX10813, LogHelper.MarkAsNonPII(typeof(SymmetricSecurityKey)), LogHelper.MarkAsNonPII(webKey.KeyId), ex), ex));
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
                string convertKeyInfo = LogHelper.FormatInvariant(LogMessages.IDX10813, LogHelper.MarkAsNonPII(typeof(X509SecurityKey)), LogHelper.MarkAsNonPII(webKey.KeyId), ex);
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
                string convertKeyInfo = LogHelper.FormatInvariant(LogMessages.IDX10813, LogHelper.MarkAsNonPII(typeof(RsaSecurityKey)), LogHelper.MarkAsNonPII(webKey.KeyId), ex);
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

                webKey.ConvertKeyInfo = LogHelper.FormatInvariant(LogMessages.IDX10814, LogHelper.MarkAsNonPII(typeof(ECDsaSecurityKey)), LogHelper.MarkAsNonPII(webKey.KeyId), string.Join(", ", missingComponent));
                return false;
            }

            try
            {
                key = new ECDsaSecurityKey(webKey, !string.IsNullOrEmpty(webKey.D));
                return true;
            }
            catch (Exception ex)
            {
                string convertKeyInfo = LogHelper.FormatInvariant(LogMessages.IDX10813, LogHelper.MarkAsNonPII(typeof(ECDsaSecurityKey)), LogHelper.MarkAsNonPII(webKey.KeyId), ex);
                webKey.ConvertKeyInfo = convertKeyInfo;
                if (LogHelper.IsEnabled(EventLogLevel.Error))
                    LogHelper.LogExceptionMessage(new InvalidOperationException(convertKeyInfo, ex));
            }

            return false;
        }

        /// <summary>
        /// Converts an <see cref="MlDsaSecurityKey"/> into a <see cref="JsonWebKey"/>.
        /// </summary>
        public static JsonWebKey ConvertFromMlDsaSecurityKey(MlDsaSecurityKey key)
        {
            if (key == null)
                throw LogHelper.LogArgumentNullException(nameof(key));

            string algorithmName = MlDsaSecurityKey.GetAlgorithmName(key.MLDsa.Algorithm);
            byte[] publicKey = key.MLDsa.ExportMLDsaPublicKey();

            var jsonWebKey = new JsonWebKey
            {
                Kty = JsonWebAlgorithmsKeyTypes.Akp,
                Alg = algorithmName,
                Pub = Base64UrlEncoder.Encode(publicKey),
                Kid = key.KeyId
            };

            if (key.PrivateKeyStatus == PrivateKeyStatus.Exists)
            {
                byte[] seed = key.MLDsa.ExportMLDsaPrivateSeed();
                try
                {
                    jsonWebKey.Priv = Base64UrlEncoder.Encode(seed);
                }
                finally
                {
                    CryptographicOperations.ZeroMemory(seed);
                }
            }

            return jsonWebKey;
        }

        internal static bool TryConvertToMlDsaSecurityKey(JsonWebKey webKey, out SecurityKey key)
        {
            key = null;

            if (!SupportedAlgorithms.IsSupportedMlDsaAlgorithm(webKey.Alg))
                return false;

            try
            {
                key = new MlDsaSecurityKey(webKey, !string.IsNullOrEmpty(webKey.Priv));
                return true;
            }
            catch (Exception ex)
            {
                string convertKeyInfo = LogHelper.FormatInvariant(LogMessages.IDX10813, LogHelper.MarkAsNonPII(typeof(MlDsaSecurityKey)), LogHelper.MarkAsNonPII(webKey.KeyId), ex);
                webKey.ConvertKeyInfo = convertKeyInfo;
                if (LogHelper.IsEnabled(EventLogLevel.Error))
                    LogHelper.LogExceptionMessage(new InvalidOperationException(convertKeyInfo, ex));
            }

            return false;
        }
    }
}
