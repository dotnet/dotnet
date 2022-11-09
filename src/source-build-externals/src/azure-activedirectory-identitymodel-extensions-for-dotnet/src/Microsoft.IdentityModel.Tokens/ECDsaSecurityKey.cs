//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

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
        /// Returns a new instance of <see cref="ECDsaSecurityKey"/>.
        /// </summary>
        /// <param name="ecdsa"><see cref="System.Security.Cryptography.ECDsa"/></param>
        public ECDsaSecurityKey(ECDsa ecdsa)
        {
            ECDsa = ecdsa ?? throw LogHelper.LogArgumentNullException(nameof(ecdsa));
        }

        /// <summary>
        /// <see cref="System.Security.Cryptography.ECDsa"/> instance used to initialize the key.
        /// </summary>
        public ECDsa ECDsa { get; private set; }

        /// <summary>
        /// Gets a bool indicating if a private key exists.
        /// </summary>
        /// <return>true if it has a private key; otherwise, false.</return>
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
        /// Gets an enum indicating if a private key exists.
        /// </summary>
        /// <return>'Exists' if private key exists for sure; 'DoesNotExist' if private key doesn't exist for sure; 'Unknown' if we cannot determine.</return>
        public override PrivateKeyStatus PrivateKeyStatus
        {
            get
            {
                return PrivateKeyStatus.Unknown;
            }
        }

        /// <summary>
        /// Gets <see cref="System.Security.Cryptography.ECDsa"/> key size.
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
        /// <returns><c>true</c> if JWK thumbprint can be computed; otherwise, <c>false</c>.</returns>
        /// <remarks>https://datatracker.ietf.org/doc/html/rfc7638</remarks>
        public override bool CanComputeJwkThumbprint()
        {
#if NET472 || NETSTANDARD2_0 || NET6_0
            if (ECDsaAdapter.SupportsECParameters())
                return true;
#endif
            return false;
        }

        /// <summary>
        /// Computes a sha256 hash over the <see cref="ECDsaSecurityKey"/>.
        /// </summary>
        /// <returns>A JWK thumbprint.</returns>
        /// <remarks>https://datatracker.ietf.org/doc/html/rfc7638</remarks>
        public override byte[] ComputeJwkThumbprint()
        {
#if NET472 || NETSTANDARD2_0 || NET6_0
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
