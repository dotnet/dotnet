// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Represents a ECDsa security key.
    /// </summary>
    public class ECDsaSecurityKey : AsymmetricSecurityKey
    {
        private bool? _hasPrivateKey;

        internal ECDsaSecurityKey(JsonWebKey webKey, bool usePrivateKey)
            : base(webKey)
        {
            ECDsa = ECDsaAdapter.Instance.CreateECDsa(webKey, usePrivateKey);
            webKey.ConvertedSecurityKey = this;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ECDsaSecurityKey"/> class.
        /// </summary>
        /// <param name="ecdsa">The <see cref="ECDsa"/>.</param>
        public ECDsaSecurityKey(ECDsa ecdsa)
        {
            ECDsa = ecdsa ?? throw LogHelper.LogArgumentNullException(nameof(ecdsa));
        }

        /// <summary>
        /// The <see cref="ECDsa"/> instance used to initialize the key.
        /// </summary>
        public ECDsa ECDsa { get; private set; }

        /// <summary>
        /// Gets a bool indicating if a private key exists.
        /// </summary>
        /// <return><see langword="true"/> if it has a private key; otherwise, <see langword="false"/>.</return>
        [System.Obsolete("HasPrivateKey method is deprecated, please use FoundPrivateKey instead.")]
        public override bool HasPrivateKey
        {
            get
            {
                if (_hasPrivateKey == null)
                {
                    try
                    {
                        // imitate signing
                        ECDsa.SignHash(new byte[20]);
                        _hasPrivateKey = true;
                    }
                    catch (CryptographicException)
                    {
                        _hasPrivateKey = false;
                    }
                }

                return _hasPrivateKey.Value;
            }
        }

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
                return PrivateKeyStatus.Unknown;
            }
        }

        /// <summary>
        /// Gets the <see cref="ECDsa"/> key size.
        /// </summary>
        public override int KeySize
        {
            get
            {
                return ECDsa.KeySize;
            }
        }

        /// <summary>
        /// Determines whether the <see cref="ECDsaSecurityKey"/> can compute a JWK thumbprint.
        /// </summary>
        /// <returns><see langword="true"/> if JWK thumbprint can be computed; otherwise, <see langword="false"/>.</returns>
        /// <remarks>See: <see href="https://datatracker.ietf.org/doc/html/rfc7638"/>.</remarks>
        public override bool CanComputeJwkThumbprint()
        {
#if NET472 || NETSTANDARD2_0 || NET6_0_OR_GREATER
            if (ECDsaAdapter.SupportsECParameters())
                return true;
#endif
            return false;
        }

        /// <summary>
        /// Computes a SHA256 hash over the <see cref="ECDsaSecurityKey"/>.
        /// </summary>
        /// <returns>A JWK thumbprint.</returns>
        /// <remarks>See: <see href="https://datatracker.ietf.org/doc/html/rfc7638"/>.</remarks>
        public override byte[] ComputeJwkThumbprint()
        {
#if NET472 || NETSTANDARD2_0 || NET6_0_OR_GREATER
            if (ECDsaAdapter.SupportsECParameters())
            {
                ECParameters parameters = ECDsa.ExportParameters(false);
                var canonicalJwk = $@"{{""{JsonWebKeyParameterNames.Crv}"":""{ECDsaAdapter.GetCrvParameterValue(parameters.Curve)}"",""{JsonWebKeyParameterNames.Kty}"":""{JsonWebAlgorithmsKeyTypes.EllipticCurve}"",""{JsonWebKeyParameterNames.X}"":""{Base64UrlEncoder.Encode(parameters.Q.X)}"",""{JsonWebKeyParameterNames.Y}"":""{Base64UrlEncoder.Encode(parameters.Q.Y)}""}}";
                return Utility.GenerateSha256Hash(canonicalJwk);
            }
#endif
            throw LogHelper.LogExceptionMessage(new PlatformNotSupportedException(LogMessages.IDX10695));
        }
    }
}
