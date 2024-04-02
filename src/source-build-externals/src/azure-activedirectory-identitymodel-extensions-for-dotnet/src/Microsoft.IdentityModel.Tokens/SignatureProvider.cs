// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.Logging;
using System;
using System.Threading;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Provides signature services, signing and verifying.
    /// </summary>
    public abstract class SignatureProvider : IDisposable
    {
        /// <summary>
        /// Maintains the number of external references
        /// see: <see cref="AddRef"/>, <see cref="RefCount"/>, <see cref="Release"/>
        /// </summary>
        private int _referenceCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignatureProvider"/> class used to create and verify signatures.
        /// </summary>
        /// <param name="key">The <see cref="SecurityKey"/> that will be used for signature operations.</param>
        /// <param name="algorithm">The signature algorithm to apply.</param>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="algorithm"/> is null or empty.</exception>
        protected SignatureProvider(SecurityKey key, string algorithm)
        {
            Key = key ?? throw LogHelper.LogArgumentNullException(nameof(key));
            Algorithm = (string.IsNullOrEmpty(algorithm)) ? throw LogHelper.LogArgumentNullException(nameof(algorithm)) : algorithm;
            _referenceCount = 1;
        }

        internal int AddRef()
        {
            return Interlocked.Increment(ref _referenceCount);
        }

        /// <summary>
        /// Gets the signature algorithm.
        /// </summary>
        public string Algorithm { get; private set; }

        /// <summary>
        /// Gets or sets a user context for a <see cref="SignatureProvider"/>.
        /// </summary>
        /// <remarks>This is null by default. This is for use by the application and not used by this SDK.</remarks>
        public string Context { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="CryptoProviderCache"/> that is associated with this <see cref="SignatureProvider"/>
        /// </summary>
        public CryptoProviderCache CryptoProviderCache { get; set; }

        /// <summary>
        /// Calls <see cref="Dispose(bool)"/> and <see cref="GC.SuppressFinalize"/>
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Can be over written in descendants to dispose of internal components.
        /// </summary>
        /// <param name="disposing">true, if called from Dispose(), false, if invoked inside a finalizer</param>
        protected abstract void Dispose(bool disposing);

        /// <summary>
        /// Gets the <see cref="SecurityKey"/>.
        /// </summary>
        public SecurityKey Key { get; private set; }

        /// <summary>
        /// For testing purposes
        /// </summary>
        internal virtual int ObjectPoolSize => 0;

        internal int RefCount => _referenceCount;

        internal int Release()
        {
            if (_referenceCount > 0)
                Interlocked.Decrement(ref _referenceCount);

            return _referenceCount;
        }

        /// <summary>
        /// This must be overridden to produce a signature over the 'input'.
        /// </summary>
        /// <param name="input">bytes to sign.</param>
        /// <returns>signed bytes</returns>
        public abstract byte[] Sign(byte[] input);

        /// <summary>
        /// Produces a signature over the specified region of the <paramref name="input"/>.
        /// </summary>
        /// <param name="input">The bytes to produce a signature over.</param>
        /// <param name="offset">The offset to specify the beginning of the region.</param>
        /// <param name="count">The count to specify the end of the region.</param>
        /// <returns>The signature bytes.</returns>
        public virtual byte[] Sign(byte[] input, int offset, int count)
        {
            throw LogHelper.LogExceptionMessage(new NotImplementedException());
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Produces a signature over the <paramref name="data"/> and writes it to <paramref name="destination"/>.
        /// </summary>
        /// <param name="data">The bytes to produce a signature over.</param>
        /// <param name="destination">The pre-allocated span where signature bytes will be placed.</param>
        /// <param name="bytesWritten">The number of bytes written into the signature span.</param>         
        /// <returns>returns <see langword="true"/> if creation of signature succeeded, <see langword="false"/> otherwise.</returns>
        public virtual bool Sign(ReadOnlySpan<byte> data, Span<byte> destination, out int bytesWritten)
        {
            throw LogHelper.LogExceptionMessage(new NotImplementedException());
        }
#endif
        /// <summary>
        /// Verifies that the <paramref name="signature"/> over <paramref name="input"/> using the
        /// <see cref="SecurityKey"/> and <see cref="SignatureProvider.Algorithm"/> specified by this
        /// <see cref="SignatureProvider"/> are consistent.
        /// </summary>
        /// <param name="input">the bytes that were signed.</param>
        /// <param name="signature">signature to compare against.</param>
        /// <returns>true if the computed signature matches the signature parameter, false otherwise.</returns>
        public abstract bool Verify(byte[] input, byte[] signature);

        /// <summary>
        /// Verifies that a signature created over the 'input' matches the signature. Using <see cref="SecurityKey"/> and 'algorithm' passed to <see cref="SignatureProvider( SecurityKey, string )"/>.
        /// </summary>
        /// <param name="input">The bytes to verify.</param>
        /// <param name="inputOffset">offset in to input bytes to caculate hash.</param>
        /// <param name="inputLength">number of bytes of signature to use.</param>
        /// <param name="signature">signature to compare against.</param>
        /// <param name="signatureOffset">offset into signature array.</param>
        /// <param name="signatureLength">how many bytes to verfiy.</param>
        /// <returns>true if computed signature matches the signature parameter, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">'input' is null.</exception>
        /// <exception cref="ArgumentNullException">'signature' is null.</exception>
        /// <exception cref="ArgumentException">'input.Length' == 0.</exception>
        /// <exception cref="ArgumentException">'signature.Length' == 0. </exception>
        /// <exception cref="ArgumentException">'length &lt; 1'</exception>
        /// <exception cref="ArgumentException">'offset + length &gt; input.Length'</exception>
        /// <exception cref="ObjectDisposedException"><see cref="Dispose(bool)"/> has been called.</exception>
        public virtual bool Verify(byte[] input, int inputOffset, int inputLength, byte[] signature, int signatureOffset, int signatureLength)
        {
            throw LogHelper.LogExceptionMessage(new NotImplementedException());
        }

        /// <summary>
        /// Gets or sets a bool indicating if this <see cref="SignatureProvider"/> is expected to create signatures.
        /// </summary>
        public bool WillCreateSignatures { get; protected set; }
    }
}
