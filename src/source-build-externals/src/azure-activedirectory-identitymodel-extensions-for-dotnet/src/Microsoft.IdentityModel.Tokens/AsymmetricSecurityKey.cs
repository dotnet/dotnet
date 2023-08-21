// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Base class for a Security Key that contains Asymmetric key material.
    /// </summary>
    public abstract class AsymmetricSecurityKey : SecurityKey
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public AsymmetricSecurityKey()
        {
        }

        internal AsymmetricSecurityKey(SecurityKey key)
            : base(key)
        {
        }

        /// <summary>
        /// This must be overridden to get a bool indicating if a private key exists.
        /// </summary>
        /// <return>true if it has a private key; otherwise, false.</return>
        [System.Obsolete("HasPrivateKey method is deprecated, please use PrivateKeyStatus instead.")]
        public abstract bool HasPrivateKey { get; }

        /// <summary>
        /// Gets the status of the private key.
        /// </summary>
        /// <return>'Exists' if private key exists for sure; 'DoesNotExist' if private key doesn't exist for sure; 'Unknown' if we cannot determine.</return>
        public abstract PrivateKeyStatus PrivateKeyStatus { get; }
    }

    /// <summary>
    /// Enum for the existence of private key
    /// </summary>
    public enum PrivateKeyStatus
    {
        /// <summary>
        /// private key exists for sure
        /// </summary>
        Exists,

        /// <summary>
        /// private key doesn't exist for sure
        /// </summary>
        DoesNotExist,

        /// <summary>
        /// unable to determine the existence of private key
        /// </summary>
        Unknown
    };
}
