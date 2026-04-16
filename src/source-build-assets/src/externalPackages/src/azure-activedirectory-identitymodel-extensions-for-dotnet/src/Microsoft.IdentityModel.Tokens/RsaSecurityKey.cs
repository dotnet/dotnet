// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Represents a Rsa security key.
    /// </summary>
    public class RsaSecurityKey : AsymmetricSecurityKey
    {
        private bool? _hasPrivateKey;

        private bool _foundPrivateKeyDetermined = false;

        private PrivateKeyStatus _foundPrivateKey;

        private const string _className = "Microsoft.IdentityModel.Tokens.RsaSecurityKey";

        internal RsaSecurityKey(JsonWebKey webKey)
            : base(webKey)
        {
            IntializeWithRsaParameters(webKey.CreateRsaParameters());
            webKey.ConvertedSecurityKey = this;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RsaSecurityKey"/> class.
        /// </summary>
        /// <param name="rsaParameters"><see cref="RSAParameters"/></param>
        public RsaSecurityKey(RSAParameters rsaParameters)
        {
            IntializeWithRsaParameters(rsaParameters);
        }

        internal void IntializeWithRsaParameters(RSAParameters rsaParameters)
        {
            // must have modulus and exponent otherwise the crypto operations fail later
            if (rsaParameters.Modulus == null)
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10700, LogHelper.MarkAsNonPII(_className), LogHelper.MarkAsNonPII("Modulus"))));

            if (rsaParameters.Exponent == null)
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10700, LogHelper.MarkAsNonPII(_className), LogHelper.MarkAsNonPII("Exponent"))));

            _hasPrivateKey = rsaParameters.D != null && rsaParameters.DP != null && rsaParameters.DQ != null && rsaParameters.P != null && rsaParameters.Q != null && rsaParameters.InverseQ != null;
            _foundPrivateKey = _hasPrivateKey.Value ? PrivateKeyStatus.Exists : PrivateKeyStatus.DoesNotExist;
            _foundPrivateKeyDetermined = true;
            Parameters = rsaParameters;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RsaSecurityKey"/> class.
        /// </summary>
        /// <param name="rsa"><see cref="RSA"/></param>
        public RsaSecurityKey(RSA rsa)
        {
            Rsa = rsa ?? throw LogHelper.LogArgumentNullException(nameof(rsa));
        }

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
                        byte[] hash = new byte[20];
                        Rsa.SignData(hash, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
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
                if (_foundPrivateKeyDetermined)
                    return _foundPrivateKey;

                _foundPrivateKeyDetermined = true;
                if (Rsa != null)
                {
                    try
                    {
                        var parameters = Rsa.ExportParameters(true);
                        if (parameters.D != null && parameters.DP != null && parameters.DQ != null &&
                            parameters.P != null && parameters.Q != null && parameters.InverseQ != null)
                            _foundPrivateKey = PrivateKeyStatus.Exists;
                        else
                            _foundPrivateKey = PrivateKeyStatus.DoesNotExist;

                    }
                    catch (Exception)
                    {
                        _foundPrivateKey = PrivateKeyStatus.Unknown;
                        return _foundPrivateKey;
                    }
                }
                else
                {
                    if (Parameters.D != null && Parameters.DP != null && Parameters.DQ != null &&
                        Parameters.P != null && Parameters.Q != null && Parameters.InverseQ != null)
                        _foundPrivateKey = PrivateKeyStatus.Exists;
                    else
                        _foundPrivateKey = PrivateKeyStatus.DoesNotExist;
                }

                return _foundPrivateKey;
            }           
        }

        /// <summary>
        /// Gets RSA key size.
        /// </summary>
        public override int KeySize
        {
            get
            {
                if (Rsa != null)
                    return Rsa.KeySize;
                else if (Parameters.Modulus != null)
                    return Parameters.Modulus.Length * 8;
                else
                    return 0;
            }
        }

        /// <summary>
        /// <see cref="RSAParameters"/> used to initialize the key.
        /// </summary>
        public RSAParameters Parameters { get; private set; }

        /// <summary>
        /// <see cref="RSA"/> instance used to initialize the key.
        /// </summary>
        public RSA Rsa { get; private set; }

        /// <summary>
        /// Determines whether the <see cref="RsaSecurityKey"/> can compute a JWK thumbprint.
        /// </summary>
        /// <returns><c>true</c> if JWK thumbprint can be computed; otherwise, <c>false</c>.</returns>
        /// <remarks>https://datatracker.ietf.org/doc/html/rfc7638</remarks>
        public override bool CanComputeJwkThumbprint()
        {
            return (Rsa != null || (Parameters.Exponent != null && Parameters.Modulus != null));
        }

        /// <summary>
        /// Computes a sha256 hash over the <see cref="RsaSecurityKey"/>.
        /// </summary>
        /// <returns>A JWK thumbprint.</returns>
        /// <remarks>https://datatracker.ietf.org/doc/html/rfc7638</remarks>
        public override byte[] ComputeJwkThumbprint()
        {
            var rsaParameters = Parameters;

            if (rsaParameters.Exponent == null || rsaParameters.Modulus == null)
                rsaParameters = Rsa.ExportParameters(false);

            var canonicalJwk = $@"{{""{JsonWebKeyParameterNames.E}"":""{Base64UrlEncoder.Encode(rsaParameters.Exponent)}"",""{JsonWebKeyParameterNames.Kty}"":""{JsonWebAlgorithmsKeyTypes.RSA}"",""{JsonWebKeyParameterNames.N}"":""{Base64UrlEncoder.Encode(rsaParameters.Modulus)}""}}";
            return Utility.GenerateSha256Hash(canonicalJwk);
        }
    }
}
