// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// An <see cref="AsymmetricSecurityKey"/> that is backed by a <see cref="X509Certificate2"/>
    /// </summary>
    public class X509SecurityKey : AsymmetricSecurityKey
    {
        AsymmetricAlgorithm _privateKey;
        bool _privateKeyAvailabilityDetermined;
        AsymmetricAlgorithm _publicKey;
        object _thisLock = new Object();

        internal X509SecurityKey(JsonWebKey webKey)
            : base(webKey)
        {
            Certificate = new X509Certificate2(Convert.FromBase64String(webKey.X5c[0]));
            X5t = Base64UrlEncoder.Encode(Certificate.GetCertHash());
            webKey.ConvertedSecurityKey = this;
        }

        /// <summary>
        /// Instantiates a <see cref="X509SecurityKey"/> using a <see cref="X509Certificate2"/>
        /// </summary>
        /// <param name="certificate">The <see cref="X509Certificate2"/> to use.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="certificate"/> is null.</exception>
        public X509SecurityKey(X509Certificate2 certificate)
        {
            Certificate = certificate ?? throw LogHelper.LogArgumentNullException(nameof(certificate));
            KeyId = certificate.Thumbprint;
            X5t = Base64UrlEncoder.Encode(certificate.GetCertHash());
        }

        /// <summary>
        /// Instantiates a <see cref="X509SecurityKey"/> using a <see cref="X509Certificate2"/>.
        /// </summary>
        /// <param name="certificate">The <see cref="X509Certificate2"/> to use.</param>
        /// <param name="keyId">The value to set for the KeyId</param>
        /// <exception cref="ArgumentNullException">if <paramref name="certificate"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="keyId"/> is null or empty.</exception>
        public X509SecurityKey(X509Certificate2 certificate, string keyId)
        {
            Certificate = certificate ?? throw LogHelper.LogArgumentNullException(nameof(certificate));
            KeyId = string.IsNullOrEmpty(keyId) ? throw LogHelper.LogArgumentNullException(nameof(keyId)) : keyId;
            X5t = Base64UrlEncoder.Encode(certificate.GetCertHash());
        }

        /// <summary>
        /// Gets the key size.
        /// </summary>
        public override int KeySize
        {
            get => PublicKey.KeySize;
        }

        /// <summary>
        /// Gets the X5t of this <see cref="X509SecurityKey"/>.
        /// </summary>
        public string X5t { get; }

        /// <summary>
        /// Returns the private key from the <see cref="X509SecurityKey"/>.
        /// </summary>
        public AsymmetricAlgorithm PrivateKey
        {
            get
            {
                if (!_privateKeyAvailabilityDetermined)
                {
                    lock (ThisLock)
                    {
                        if (!_privateKeyAvailabilityDetermined)
                        {
                            _privateKey = RSACertificateExtensions.GetRSAPrivateKey(Certificate);

                            _privateKeyAvailabilityDetermined = true;
                        }
                    }
                }

                return _privateKey;
            }
        }

        /// <summary>
        /// Gets the public key from the <see cref="X509SecurityKey"/>.
        /// </summary>
        public AsymmetricAlgorithm PublicKey
        {
            get
            {
                if (_publicKey == null)
                {
                    lock (ThisLock)
                    {
                        if (_publicKey == null)
                        {
                            _publicKey = RSACertificateExtensions.GetRSAPublicKey(Certificate);
                        }
                    }
                }

                return _publicKey;
            }
        }

        object ThisLock
        {
            get { return _thisLock; }
        }

        /// <summary>
        /// Gets a bool indicating if a private key exists.
        /// </summary>
        /// <return>true if it has a private key; otherwise, false.</return>
        [System.Obsolete("HasPrivateKey method is deprecated, please use PrivateKeyStatus.")]
        public override bool HasPrivateKey
        {
            get { return (PrivateKey != null); }
        }

        /// <summary>
        /// Gets an enum indicating if a private key exists.
        /// </summary>
        /// <return>'Exists' if private key exists for sure; 'DoesNotExist' if private key doesn't exist for sure; 'Unknown' if we cannot determine.</return>
        public override PrivateKeyStatus PrivateKeyStatus
        {
            get
            {
                return PrivateKey == null ? PrivateKeyStatus.DoesNotExist : PrivateKeyStatus.Exists;
            }
        }

        /// <summary>
        /// Gets the <see cref="X509Certificate2"/>.
        /// </summary>
        public X509Certificate2 Certificate
        {
            get; private set;
        }

        internal override string InternalId => X5t;


        /// <summary>
        /// Determines whether the <see cref="X509SecurityKey"/> can compute a JWK thumbprint.
        /// </summary>
        /// <returns><c>true</c> if JWK thumbprint can be computed; otherwise, <c>false</c>.</returns>
        /// <remarks>https://datatracker.ietf.org/doc/html/rfc7638</remarks>
        public override bool CanComputeJwkThumbprint()
        {
            return (PublicKey as RSA) != null ? true : false;
        }

        /// <summary>
        /// Computes a sha256 hash over the <see cref="X509SecurityKey"/>.
        /// </summary>
        /// <returns>A JWK thumbprint.</returns>
        /// <remarks>https://datatracker.ietf.org/doc/html/rfc7638</remarks>
        public override byte[] ComputeJwkThumbprint()
        {
            return new RsaSecurityKey(PublicKey as RSA).ComputeJwkThumbprint();
        }

        /// <summary>
        /// Returns a bool indicating if this key is equivalent to another key.
        /// </summary>
        /// <return>true if the keys are equal; otherwise, false.</return>
        public override bool Equals(object obj)
        {
            if (!(obj is X509SecurityKey other))
                return false;

            return other.Certificate.Thumbprint.ToString() == Certificate.Thumbprint.ToString();
        }

        /// <summary>
        /// Returns an int hash code.
        /// </summary>
        /// <return>An int hash code</return>
        public override int GetHashCode()
        {
            return Certificate.GetHashCode();
        }
    }
}
