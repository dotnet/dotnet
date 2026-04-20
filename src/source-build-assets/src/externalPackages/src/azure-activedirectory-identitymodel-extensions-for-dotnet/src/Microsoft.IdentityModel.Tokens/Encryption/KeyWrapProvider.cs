// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Provides Wrap and Unwrap key services.
    /// </summary>
    public abstract class KeyWrapProvider : IDisposable
    {
        /// <summary>
        /// Gets the KeyWrap algorithm that is being used.
        /// </summary>
        public abstract string Algorithm { get; }

        /// <summary>
        /// Gets or sets a user context for a <see cref="KeyWrapProvider"/>.
        /// </summary>
        /// <remarks>This is null by default. This can be used by runtimes or for extensibility scenarios.</remarks>
        public abstract string Context { get; set; }

        /// <summary>
        /// Gets the <see cref="SecurityKey"/> that is being used.
        /// </summary>
        public abstract SecurityKey Key { get; }

        /// <summary>
        /// Calls <see cref="Dispose(bool)"/> and <see cref="GC.SuppressFinalize"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Can be overridden in descendants to dispose of internal components.
        /// </summary>
        /// <param name="disposing">true if called from Dispose(), false if invoked inside a finalizer.</param>
        protected abstract void Dispose(bool disposing);

        /// <summary>
        /// Unwraps a key.
        /// </summary>
        /// <param name="keyBytes">The key to unwrap.</param>
        /// <returns>The unwrapped key.</returns>
        public abstract byte[] UnwrapKey(byte[] keyBytes);

        /// <summary>
        /// Wraps a key.
        /// </summary>
        /// <param name="keyBytes">The key to be wrapped.</param>
        /// <returns>The wrapped key.</returns>
        public abstract byte[] WrapKey(byte[] keyBytes);
    }
}
