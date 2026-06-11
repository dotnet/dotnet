// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#nullable enable
using System;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Telemetry;

/// <summary>
/// Provides utility methods for telemetry and analytics purposes.
/// </summary>
public static class CryptoTelemetry
{
    internal static bool RecordSignatureValidationTelemetry { get; set; }
    private static volatile string[] _trackedIssuersArray = Array.Empty<string>();
    private const string OtherIssuersLabel = "other";

    /// <summary>
    /// Enables or disables telemetry for signature validation and configures related settings.
    /// </summary>
    /// <param name="enable">Indicates whether to enable signature validation telemetry.</param>
    /// <param name="trackedIssuers">An optional list of issuer hosts to track in telemetry. Issuers not in this list will be reported as "other".</param>
    public static void EnableSignatureValidationTelemetry(
        bool enable,
        string[]? trackedIssuers)
    {
        RecordSignatureValidationTelemetry = enable;
        TrackedIssuers = trackedIssuers ?? Array.Empty<string>();
    }

    /// <summary>
    /// Gets or sets the allowlist of issuer hosts to track in telemetry.
    /// </summary>
    /// <remarks>
    /// <para>Issuer hosts not in this list will be reported as "other" to prevent cardinality explosion in telemetry systems.</para>
    /// <para>When set to null or empty, all issuers will be reported as "other".</para>
    /// <para>Example: new[] { "login.microsoftonline.com", "accounts.google.com" }</para>
    /// </remarks>
    private static string[] TrackedIssuers
    {
        get => _trackedIssuersArray;
        set
        {
            if (value == null || value.Length == 0)
            {
                _trackedIssuersArray = Array.Empty<string>();
            }
            else
            {
                _trackedIssuersArray = value
                    .Where(h => !string.IsNullOrWhiteSpace(h))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray();
            }
        }
    }

    private static class KeyAlgorithmIds
    {
        public const string Rsa2048 = "RSA-2048";
        public const string Rsa3072 = "RSA-3072";
        public const string Rsa4096 = "RSA-4096";
        public const string RsaUnknown = "RSA-UNKNOWN";

        public const string EcdsaP256 = "ECDSA-P256";
        public const string EcdsaP384 = "ECDSA-P384";
        public const string EcdsaP521 = "ECDSA-P521";
        public const string EcdsaUnknown = "ECDSA-UNKNOWN";

        public const string Symmetric128 = "SYM-128";
        public const string Symmetric192 = "SYM-192";
        public const string Symmetric256 = "SYM-256";
        public const string Symmetric384 = "SYM-384";
        public const string Symmetric512 = "SYM-512";
        public const string SymmetricUnknown = "SYM-UNKNOWN";

        public const string MlDsa44 = "MLDSA-44";
        public const string MlDsa65 = "MLDSA-65";
        public const string MlDsa87 = "MLDSA-87";
        public const string MlDsaUnknown = "MLDSA-UNKNOWN";

        public const string Unknown = "UNKNOWN"; // Key type is not recognized.
        public const string NoKey = "NO-KEY"; // Used when no key is found or provided to differentiate from unknown key types
    }

    internal static string GetKeyAlgorithmId(SecurityKey key)
    {
        return key switch
        {
            null => KeyAlgorithmIds.NoKey,

            RsaSecurityKey rsa => rsa.KeySize switch
            {
                2048 => KeyAlgorithmIds.Rsa2048,
                3072 => KeyAlgorithmIds.Rsa3072,
                4096 => KeyAlgorithmIds.Rsa4096,
                _ => KeyAlgorithmIds.RsaUnknown
            },

            ECDsaSecurityKey ecdsa => ecdsa.KeySize switch
            {
                256 => KeyAlgorithmIds.EcdsaP256,
                384 => KeyAlgorithmIds.EcdsaP384,
                521 => KeyAlgorithmIds.EcdsaP521,
                _ => KeyAlgorithmIds.EcdsaUnknown
            },

            SymmetricSecurityKey symmetric => symmetric.KeySize switch
            {
                128 => KeyAlgorithmIds.Symmetric128,
                192 => KeyAlgorithmIds.Symmetric192,
                256 => KeyAlgorithmIds.Symmetric256,
                384 => KeyAlgorithmIds.Symmetric384,
                512 => KeyAlgorithmIds.Symmetric512,
                _ => KeyAlgorithmIds.SymmetricUnknown
            },

            X509SecurityKey x509 => GetX509KeyAlgorithmId(x509),

            JsonWebKey jwk => GetJsonWebKeyAlgorithmId(jwk),

            MlDsaSecurityKey mlDsa => mlDsa.MLDsa.Algorithm.Name switch
            {
                "ML-DSA-44" => KeyAlgorithmIds.MlDsa44,
                "ML-DSA-65" => KeyAlgorithmIds.MlDsa65,
                "ML-DSA-87" => KeyAlgorithmIds.MlDsa87,
                _ => KeyAlgorithmIds.MlDsaUnknown
            },

            // EdDSA and other key types can be added here when needed.
            _ => KeyAlgorithmIds.Unknown
        };
    }

    private static string GetX509KeyAlgorithmId(X509SecurityKey x509)
    {
        if (x509.MlDsaPublicKey != null)
        {
            return x509.MlDsaPublicKey.Algorithm.Name switch
            {
                "ML-DSA-44" => KeyAlgorithmIds.MlDsa44,
                "ML-DSA-65" => KeyAlgorithmIds.MlDsa65,
                "ML-DSA-87" => KeyAlgorithmIds.MlDsa87,
                _ => KeyAlgorithmIds.MlDsaUnknown
            };
        }

        return x509.KeySize switch
        {
            2048 => KeyAlgorithmIds.Rsa2048,
            3072 => KeyAlgorithmIds.Rsa3072,
            4096 => KeyAlgorithmIds.Rsa4096,
            256 => KeyAlgorithmIds.EcdsaP256,
            384 => KeyAlgorithmIds.EcdsaP384,
            521 => KeyAlgorithmIds.EcdsaP521,
            _ => x509.PublicKey is RSA
                ? KeyAlgorithmIds.RsaUnknown
                : x509.PublicKey is ECDsa
                    ? KeyAlgorithmIds.EcdsaUnknown
                    : KeyAlgorithmIds.Unknown
        };
    }

    private static string GetJsonWebKeyAlgorithmId(JsonWebKey jwk)
    {
        if (jwk.ConvertedSecurityKey != null)
            return GetKeyAlgorithmId(jwk.ConvertedSecurityKey);

        return jwk.Kty switch
        {
            JsonWebAlgorithmsKeyTypes.RSA => jwk.KeySize switch
            {
                2048 => KeyAlgorithmIds.Rsa2048,
                3072 => KeyAlgorithmIds.Rsa3072,
                4096 => KeyAlgorithmIds.Rsa4096,
                _ => KeyAlgorithmIds.RsaUnknown
            },
            JsonWebAlgorithmsKeyTypes.EllipticCurve => jwk.KeySize switch
            {
                256 => KeyAlgorithmIds.EcdsaP256,
                384 => KeyAlgorithmIds.EcdsaP384,
                521 => KeyAlgorithmIds.EcdsaP521,
                _ => KeyAlgorithmIds.EcdsaUnknown
            },
            JsonWebAlgorithmsKeyTypes.Octet => jwk.KeySize switch
            {
                128 => KeyAlgorithmIds.Symmetric128,
                192 => KeyAlgorithmIds.Symmetric192,
                256 => KeyAlgorithmIds.Symmetric256,
                384 => KeyAlgorithmIds.Symmetric384,
                512 => KeyAlgorithmIds.Symmetric512,
                _ => KeyAlgorithmIds.SymmetricUnknown
            },
            JsonWebAlgorithmsKeyTypes.Akp => jwk.Alg switch
            {
                SecurityAlgorithms.MlDsa44 => KeyAlgorithmIds.MlDsa44,
                SecurityAlgorithms.MlDsa65 => KeyAlgorithmIds.MlDsa65,
                SecurityAlgorithms.MlDsa87 => KeyAlgorithmIds.MlDsa87,
                _ => KeyAlgorithmIds.MlDsaUnknown
            },
            _ => KeyAlgorithmIds.Unknown
        };
    }

    /// <summary>
    /// Maps a raw algorithm string to a known algorithm family or "other" to prevent cardinality explosion.
    /// </summary>
    /// <param name="algorithm">The raw algorithm identifier (e.g., RS256, ES512, hmac-sha256).</param>
    /// <returns>A bounded algorithm family string: RSA, RSA-PSS, ECDSA, HMAC, none, or other.</returns>
    internal static string GetKnownAlgorithmFamilyOrOther(string algorithm)
    {
        if (string.IsNullOrEmpty(algorithm))
            return TelemetryConstants.AlgorithmFamilies.None;

        // JWT short-form and XML long-form algorithm identifiers
        if (algorithm.StartsWith("RS", StringComparison.Ordinal) ||
            algorithm.IndexOf("rsa-sha", StringComparison.OrdinalIgnoreCase) >= 0)
            return TelemetryConstants.AlgorithmFamilies.RSA;
        if (algorithm.StartsWith("PS", StringComparison.Ordinal) ||
            algorithm.IndexOf("rsa-MGF1", StringComparison.OrdinalIgnoreCase) >= 0)
            return TelemetryConstants.AlgorithmFamilies.RSAPSS;
        if (algorithm.StartsWith("ES", StringComparison.Ordinal) ||
            algorithm.IndexOf("ecdsa", StringComparison.OrdinalIgnoreCase) >= 0)
            return TelemetryConstants.AlgorithmFamilies.ECDSA;
        if (algorithm.StartsWith("HS", StringComparison.Ordinal) ||
            algorithm.IndexOf("hmac", StringComparison.OrdinalIgnoreCase) >= 0)
            return TelemetryConstants.AlgorithmFamilies.HMAC;
        if (algorithm.Equals("none", StringComparison.OrdinalIgnoreCase))
            return TelemetryConstants.AlgorithmFamilies.None;

        return TelemetryConstants.AlgorithmFamilies.Other;
    }

    /// <summary>
    /// Gets the issuer host for telemetry, returning "other" if not in the tracked issuers allowlist.
    /// </summary>
    /// <param name="issuer">The full issuer URI.</param>
    /// <returns>The issuer host if in the allowlist, otherwise "other".</returns>
    internal static string GetTrackedIssuerOrOther(string issuer)
    {
        if (TrackedIssuers.Length == 0 || string.IsNullOrWhiteSpace(issuer))
            return OtherIssuersLabel;

        foreach (string trackedHost in TrackedIssuers)
        {
            if (issuer.IndexOf(trackedHost, StringComparison.Ordinal) >= 0)
                return trackedHost;
        }

        return OtherIssuersLabel;
    }
}
