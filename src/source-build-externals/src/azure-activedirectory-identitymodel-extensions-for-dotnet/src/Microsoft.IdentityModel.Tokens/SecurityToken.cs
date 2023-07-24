// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Base class for security token.
    /// </summary>
    public abstract class SecurityToken : ISafeLogSecurityArtifact
    {
        internal virtual IEnumerable<Claim> CreateClaims(string issuer)
        {
            return new List<Claim>();
        }

        /// <inheritdoc/>
        public virtual string UnsafeToString() => ToString();

        /// <summary>
        /// This must be overridden to get the Id of this <see cref="SecurityToken"/>.
        /// </summary>
        public abstract string Id { get; }

        /// <summary>
        /// This must be overridden to get the issuer of this <see cref="SecurityToken"/>.
        /// </summary>
        public abstract string Issuer { get; }

        /// <summary>
        /// This must be overridden to get the <see cref="SecurityKey"/>.
        /// </summary>
        public abstract SecurityKey SecurityKey { get; }

        /// <summary>
        /// This must be overridden to get or set the <see cref="SecurityKey"/> that signed this instance.
        /// </summary>
        /// <remarks><see cref="ISecurityTokenValidator"/>.ValidateToken(...) can this value when a <see cref="SecurityKey"/> is used to successfully validate a signature.</remarks>
        public abstract SecurityKey SigningKey { get; set; }

        /// <summary>
        /// This must be overridden to get the time when this <see cref="SecurityToken"/> was Valid.
        /// </summary>
        public abstract DateTime ValidFrom { get; }

        /// <summary>
        /// This must be overridden to get the time when this <see cref="SecurityToken"/> is no longer Valid.
        /// </summary>
        public abstract DateTime ValidTo { get; }
    }
}
