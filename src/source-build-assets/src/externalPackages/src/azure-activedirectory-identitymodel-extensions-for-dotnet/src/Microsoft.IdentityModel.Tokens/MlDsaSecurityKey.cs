// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

/// <summary>
/// Represents an ML-DSA security key.
/// </summary>
public class MlDsaSecurityKey : AsymmetricSecurityKey, IDisposable
{
    private bool? _hasPrivateKey;
    private bool _disposed;
    private readonly bool _ownsMlDsa;

    internal MlDsaSecurityKey(JsonWebKey webKey, bool usePrivateKey)
        : base(webKey)
    {
        MLDsa = MlDsaAdapter.CreateMlDsa(webKey, usePrivateKey);
        _ownsMlDsa = true;
        webKey.ConvertedSecurityKey = this;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MlDsaSecurityKey"/> class.
    /// </summary>
    /// <param name="mlDsa">The <see cref="MLDsa"/> instance. The caller is responsible for the
    /// lifetime of this instance; disposing the key does not dispose the MLDsa.</param>
    public MlDsaSecurityKey(MLDsa mlDsa)
    {
        MLDsa = mlDsa ?? throw LogHelper.LogArgumentNullException(nameof(mlDsa));
    }

    /// <summary>
    /// The <see cref="MLDsa"/> instance used to initialize the key.
    /// </summary>
    public MLDsa MLDsa { get; private set; }

    /// <summary>
    /// Releases resources held by this instance.
    /// Only disposes the <see cref="MLDsa"/> if this key owns the instance
    /// (i.e., the instance was created internally from a <see cref="JsonWebKey"/>).
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases unmanaged and, optionally, managed resources.
    /// </summary>
    /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources;
    /// <see langword="false"/> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            _disposed = true;
            if (disposing && _ownsMlDsa)
            {
                MLDsa?.Dispose();
            }
        }
    }

    /// <summary>
    /// Gets a bool indicating if a private key exists.
    /// </summary>
    /// <return><see langword="true"/> if it has a private key; otherwise, <see langword="false"/>.</return>
    [Obsolete("HasPrivateKey method is deprecated, please use PrivateKeyStatus instead.")]
    public override bool HasPrivateKey => PrivateKeyStatus == PrivateKeyStatus.Exists;

    /// <summary>
    /// Gets a value indicating the existence of the private key.
    /// </summary>
    /// <returns>
    /// <see cref="PrivateKeyStatus.Exists"/> if the private key exists.
    /// <see cref="PrivateKeyStatus.DoesNotExist"/> if the private key does not exist.
    /// <see cref="PrivateKeyStatus.Unknown"/> if the existence of the private key cannot be determined.
    /// </returns>
    public override PrivateKeyStatus PrivateKeyStatus
    {
        get
        {
            if (_hasPrivateKey == null)
            {
                try
                {
                    // Try signing to detect private key — this is the most reliable
                    // check because keys imported from expanded private key material
                    // (not seed) can sign but cannot export the seed.
                    byte[] dummy = new byte[1];
                    byte[] sig = MLDsa.SignData(dummy, context: null);
                    CryptographicOperations.ZeroMemory(sig);
                    _hasPrivateKey = true;
                }
                catch (CryptographicException)
                {
                    _hasPrivateKey = false;
                }
                catch (Exception)
                {
                    // Cannot determine private key status (e.g., platform limitation).
                    return PrivateKeyStatus.Unknown;
                }
            }

            return _hasPrivateKey.Value ? PrivateKeyStatus.Exists : PrivateKeyStatus.DoesNotExist;
        }
    }

    /// <summary>
    /// Gets the ML-DSA key size in bits.
    /// </summary>
    public override int KeySize => MLDsa.Algorithm.PublicKeySizeInBytes * 8;

    /// <summary>
    /// Determines whether the <see cref="MlDsaSecurityKey"/> can compute a JWK thumbprint.
    /// </summary>
    /// <returns><see langword="true"/> if JWK thumbprint can be computed; otherwise, <see langword="false"/>.</returns>
    /// <remarks>See: <see href="https://datatracker.ietf.org/doc/html/rfc7638"/>.</remarks>
    public override bool CanComputeJwkThumbprint() => true;

    /// <summary>
    /// Computes a SHA256 hash over the <see cref="MlDsaSecurityKey"/>.
    /// </summary>
    /// <returns>A JWK thumbprint.</returns>
    /// <remarks>See: <see href="https://datatracker.ietf.org/doc/html/rfc7638"/>.</remarks>
    public override byte[] ComputeJwkThumbprint()
    {
        string algorithmName = GetAlgorithmName(MLDsa.Algorithm);
        byte[] publicKey = MLDsa.ExportMLDsaPublicKey();
        var canonicalJwk = $@"{{""{JsonWebKeyParameterNames.Alg}"":""{algorithmName}"",""{JsonWebKeyParameterNames.Kty}"":""{JsonWebAlgorithmsKeyTypes.Akp}"",""{JsonWebKeyParameterNames.Pub}"":""{Base64UrlEncoder.Encode(publicKey)}""}}";
        return Utility.GenerateSha256Hash(canonicalJwk);
    }

    /// <summary>
    /// Gets the JWK algorithm name for the specified <see cref="MLDsaAlgorithm"/>.
    /// </summary>
    /// <param name="algorithm">The ML-DSA algorithm.</param>
    /// <returns>The algorithm name string (e.g., "ML-DSA-44").</returns>
    /// <exception cref="ArgumentException">Thrown when the algorithm is not supported.</exception>
    internal static string GetAlgorithmName(MLDsaAlgorithm algorithm)
    {
        if (algorithm == MLDsaAlgorithm.MLDsa44)
            return SecurityAlgorithms.MlDsa44;
        if (algorithm == MLDsaAlgorithm.MLDsa65)
            return SecurityAlgorithms.MlDsa65;
        if (algorithm == MLDsaAlgorithm.MLDsa87)
            return SecurityAlgorithms.MlDsa87;

        throw LogHelper.LogArgumentException<ArgumentException>(nameof(algorithm), LogMessages.IDX10652, algorithm);
    }
}
