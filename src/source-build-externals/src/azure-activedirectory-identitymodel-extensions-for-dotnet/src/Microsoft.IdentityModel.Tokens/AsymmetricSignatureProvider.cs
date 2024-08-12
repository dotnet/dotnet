// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Provides signature and verification operations for Asymmetric Algorithms using a <see cref="SecurityKey"/>.
    /// </summary>
    public class AsymmetricSignatureProvider : SignatureProvider
    {
        private DisposableObjectPool<AsymmetricAdapter> _asymmetricAdapterObjectPool;
        private CryptoProviderFactory _cryptoProviderFactory;
        private bool _disposed;
        private Dictionary<string, int> _minimumAsymmetricKeySizeInBitsForSigningMap;
        private Dictionary<string, int> _minimumAsymmetricKeySizeInBitsForVerifyingMap;

        /// <summary>
        /// Mapping from algorithm to minimum <see cref="AsymmetricSecurityKey"/>.KeySize when creating signatures.
        /// </summary>
        public static readonly Dictionary<string, int> DefaultMinimumAsymmetricKeySizeInBitsForSigningMap = new Dictionary<string, int>()
        {
            { SecurityAlgorithms.EcdsaSha256, 256 },
            { SecurityAlgorithms.EcdsaSha384, 256 },
            { SecurityAlgorithms.EcdsaSha512, 256 },
            { SecurityAlgorithms.EcdsaSha256Signature, 256 },
            { SecurityAlgorithms.EcdsaSha384Signature, 256 },
            { SecurityAlgorithms.EcdsaSha512Signature, 256 },
            { SecurityAlgorithms.RsaSha256, 2048 },
            { SecurityAlgorithms.RsaSha384, 2048 },
            { SecurityAlgorithms.RsaSha512, 2048 },
            { SecurityAlgorithms.RsaSha256Signature, 2048 },
            { SecurityAlgorithms.RsaSha384Signature, 2048 },
            { SecurityAlgorithms.RsaSha512Signature, 2048 },
            { SecurityAlgorithms.RsaSsaPssSha256, 528 },
            { SecurityAlgorithms.RsaSsaPssSha384, 784 },
            { SecurityAlgorithms.RsaSsaPssSha512, 1040 },
            { SecurityAlgorithms.RsaSsaPssSha256Signature, 528 },
            { SecurityAlgorithms.RsaSsaPssSha384Signature, 784 },
            { SecurityAlgorithms.RsaSsaPssSha512Signature, 1040 }
        };

        /// <summary>
        /// Mapping from algorithm to minimum <see cref="AsymmetricSecurityKey"/>.KeySize when verifying signatures.
        /// </summary>
        public static readonly Dictionary<string, int> DefaultMinimumAsymmetricKeySizeInBitsForVerifyingMap = new Dictionary<string, int>()
        {
            { SecurityAlgorithms.EcdsaSha256, 256 },
            { SecurityAlgorithms.EcdsaSha384, 256 },
            { SecurityAlgorithms.EcdsaSha512, 256 },
            { SecurityAlgorithms.EcdsaSha256Signature, 256 },
            { SecurityAlgorithms.EcdsaSha384Signature, 256 },
            { SecurityAlgorithms.EcdsaSha512Signature, 256 },
            { SecurityAlgorithms.RsaSha256, 1024 },
            { SecurityAlgorithms.RsaSha384, 1024 },
            { SecurityAlgorithms.RsaSha512, 1024 },
            { SecurityAlgorithms.RsaSha256Signature, 1024 },
            { SecurityAlgorithms.RsaSha384Signature, 1024 },
            { SecurityAlgorithms.RsaSha512Signature, 1024 },
            { SecurityAlgorithms.RsaSsaPssSha256, 528 },
            { SecurityAlgorithms.RsaSsaPssSha384, 784 },
            { SecurityAlgorithms.RsaSsaPssSha512, 1040 },
            { SecurityAlgorithms.RsaSsaPssSha256Signature, 528 },
            { SecurityAlgorithms.RsaSsaPssSha384Signature, 784 },
            { SecurityAlgorithms.RsaSsaPssSha512Signature, 1040 }
        };

        internal AsymmetricSignatureProvider(SecurityKey key, string algorithm, CryptoProviderFactory cryptoProviderFactory)
            : this(key, algorithm)
        {
            _cryptoProviderFactory = cryptoProviderFactory;
        }

        internal AsymmetricSignatureProvider(SecurityKey key, string algorithm, bool willCreateSignatures, CryptoProviderFactory cryptoProviderFactory)
            : this(key, algorithm, willCreateSignatures)
        {
            _cryptoProviderFactory = cryptoProviderFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsymmetricSignatureProvider"/> class used to create and verify signatures.
        /// </summary>
        /// <param name="key">The <see cref="SecurityKey"/> that will be used for signature operations.</param>
        /// <param name="algorithm">The signature algorithm to be used.</param>
        public AsymmetricSignatureProvider(SecurityKey key, string algorithm)
            : this(key, algorithm, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsymmetricSignatureProvider"/> class used for creating and verifying signatures.
        /// </summary>
        /// <param name="key">The <see cref="SecurityKey"/> that will be used for signature operations.</param>
        /// <param name="algorithm">The signature algorithm to be used.</param>
        /// <param name="willCreateSignatures">If true, the provider will be used for creating signatures; otherwise, it will be used for verifying signatures.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="algorithm"/> is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="willCreateSignatures"/> is true and there is no private key available.</exception>
        /// <exception cref="NotSupportedException">Thrown if the <see cref="SecurityKey"/> and algorithm pair are not supported.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="willCreateSignatures"/> is true and <see cref="SecurityKey.KeySize"/> is less than the required size for signing.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <see cref="SecurityKey.KeySize"/> is less than the required size for verifying signatures.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the runtime is unable to create a suitable cryptographic provider.</exception>
        public AsymmetricSignatureProvider(SecurityKey key, string algorithm, bool willCreateSignatures)
            : base(key, algorithm)
        {
            _cryptoProviderFactory = key.CryptoProviderFactory;
            _minimumAsymmetricKeySizeInBitsForSigningMap = new Dictionary<string, int>(DefaultMinimumAsymmetricKeySizeInBitsForSigningMap);
            _minimumAsymmetricKeySizeInBitsForVerifyingMap = new Dictionary<string, int>(DefaultMinimumAsymmetricKeySizeInBitsForVerifyingMap);

            var jsonWebKey = key as JsonWebKey;
            if (jsonWebKey != null)
                JsonWebKeyConverter.TryConvertToSecurityKey(jsonWebKey, out SecurityKey _);

            if (willCreateSignatures && FoundPrivateKey(key) == PrivateKeyStatus.DoesNotExist)
                throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(LogMessages.IDX10638, key)));

            if (!_cryptoProviderFactory.IsSupportedAlgorithm(algorithm, key))
                throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX10634, LogHelper.MarkAsNonPII((algorithm)), key)));

            WillCreateSignatures = willCreateSignatures;
            _asymmetricAdapterObjectPool = new DisposableObjectPool<AsymmetricAdapter>(CreateAsymmetricAdapter, _cryptoProviderFactory.SignatureProviderObjectPoolCacheSize);
        }

        /// <summary>
        /// Gets the mapping from algorithm to the minimum <see cref="AsymmetricSecurityKey"/>.KeySize for creating signatures.
        /// </summary>
        public IReadOnlyDictionary<string, int> MinimumAsymmetricKeySizeInBitsForSigningMap
        {
            get => _minimumAsymmetricKeySizeInBitsForSigningMap;
        }

        /// <summary>
        /// Gets the mapping from algorithm to the minimum <see cref="AsymmetricSecurityKey"/>.KeySize for verifying signatures.
        /// </summary>
        public IReadOnlyDictionary<string, int> MinimumAsymmetricKeySizeInBitsForVerifyingMap
        {
            get => _minimumAsymmetricKeySizeInBitsForVerifyingMap;
        }

        private static PrivateKeyStatus FoundPrivateKey(SecurityKey key)
        {
            if (key is AsymmetricSecurityKey asymmetricSecurityKey)
                return asymmetricSecurityKey.PrivateKeyStatus;

            if (key is JsonWebKey jsonWebKey)
                return jsonWebKey.HasPrivateKey ? PrivateKeyStatus.Exists : PrivateKeyStatus.DoesNotExist;

            return PrivateKeyStatus.Unknown;
        }

        /// <summary>
        /// Creating a Signature requires the use of a <see cref="HashAlgorithm"/>.
        /// This method returns the <see cref="HashAlgorithmName"/>
        /// that describes the <see cref="HashAlgorithm"/>to use when generating a Signature.
        /// </summary>
        /// <param name="algorithm">The SignatureAlgorithm in use.</param>
        /// <returns>The <see cref="HashAlgorithmName"/> to use.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="algorithm"/> is null or whitespace.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="algorithm"/> is not supported.</exception>
        protected virtual HashAlgorithmName GetHashAlgorithmName(string algorithm)
        {
            if (string.IsNullOrWhiteSpace(algorithm))
                throw LogHelper.LogArgumentNullException(nameof(algorithm));

            return SupportedAlgorithms.GetHashAlgorithmName(algorithm);
        }

        private AsymmetricAdapter CreateAsymmetricAdapter()
        {
            var hashAlgoritmName = GetHashAlgorithmName(Algorithm);
            return new AsymmetricAdapter(Key, Algorithm, _cryptoProviderFactory.CreateHashAlgorithm(hashAlgoritmName), hashAlgoritmName, WillCreateSignatures);
        }

        internal bool ValidKeySize()
        {
            ValidateAsymmetricSecurityKeySize(Key, Algorithm, WillCreateSignatures);
            return true;
        }

        /// <summary>
        /// For testing purposes
        /// </summary>
        internal override int ObjectPoolSize => _asymmetricAdapterObjectPool.Size;

#if NET6_0_OR_GREATER
        /// <inheritdoc/>
        public override bool Sign(ReadOnlySpan<byte> input, Span<byte> signature, out int bytesWritten)
        {
            if (input.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(input));

            if (_disposed)
            {
                CryptoProviderCache?.TryRemove(this);
                throw LogHelper.LogExceptionMessage(new ObjectDisposedException(GetType().ToString()));
            }

            AsymmetricAdapter asym = null;
            try
            {
                asym = _asymmetricAdapterObjectPool.Allocate();
                return asym.SignUsingSpan(input, signature, out bytesWritten);
            }
            catch
            {
                CryptoProviderCache?.TryRemove(this);
                throw;
            }
            finally
            {
                if (asym != null)
                    _asymmetricAdapterObjectPool.Free(asym);
            }
        }
#endif

        /// <summary>
        /// Produces a signature over the 'input' using the <see cref="AsymmetricSecurityKey"/> and algorithm passed to <see cref="AsymmetricSignatureProvider( SecurityKey, string, bool )"/>.
        /// </summary>
        /// <param name="input">The bytes to be signed.</param>
        /// <returns>A signature over the input.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="input"/> is null or has length of 0.</exception>
        /// <exception cref="ObjectDisposedException">Thrown If <see cref="AsymmetricSignatureProvider.Dispose(bool)"/> has been called.</exception>
        /// <remarks>Sign is thread safe.</remarks>
        public override byte[] Sign(byte[] input)
        {
            if (input == null || input.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(input));

            if (_disposed)
            {
                CryptoProviderCache?.TryRemove(this);
                throw LogHelper.LogExceptionMessage(new ObjectDisposedException(GetType().ToString()));
            }

            AsymmetricAdapter asym = null;
            try
            {
                asym = _asymmetricAdapterObjectPool.Allocate();
                return asym.Sign(input);
            }
            catch
            {
                CryptoProviderCache?.TryRemove(this);
                throw;
            }
            finally
            {
                if (asym != null)
                    _asymmetricAdapterObjectPool.Free(asym);
            }
        }

        /// <inheritdoc/>
        public override byte[] Sign(byte[] input, int offset, int count)
        {
            if (input == null || input.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(input));

            if (_disposed)
            {
                CryptoProviderCache?.TryRemove(this);
                throw LogHelper.LogExceptionMessage(new ObjectDisposedException(GetType().ToString()));
            }

            AsymmetricAdapter asym = null;
            try
            {
                asym = _asymmetricAdapterObjectPool.Allocate();
                return asym.SignUsingOffset(input, offset, count);
            }
            catch
            {
                CryptoProviderCache?.TryRemove(this);
                throw;
            }
            finally
            {
                if (asym != null)
                    _asymmetricAdapterObjectPool.Free(asym);
            }
        }

        /// <summary>
        /// Validates that an asymmetric key size is of sufficient size for a SignatureAlgorithm.
        /// </summary>
        /// <param name="key">The asymmetric key to validate.</param>
        /// <param name="algorithm">The algorithm for which this key will be used.</param>
        /// <param name="willCreateSignatures">If true, the provider will be used for creating signatures.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="key"/>is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="algorithm"/> is null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="key.KeySize"/> is less than the minimum acceptable size.</exception>
        /// <remarks>
        /// <seealso cref="MinimumAsymmetricKeySizeInBitsForSigningMap"/> for minimum signing sizes.
        /// <seealso cref="MinimumAsymmetricKeySizeInBitsForVerifyingMap"/> for minimum verifying sizes.
        /// </remarks>
        public virtual void ValidateAsymmetricSecurityKeySize(SecurityKey key, string algorithm, bool willCreateSignatures)
        {
            if (key == null)
                throw LogHelper.LogArgumentNullException(nameof(key));

            if (string.IsNullOrEmpty(algorithm))
                throw LogHelper.LogArgumentNullException(nameof(algorithm));

            int keySize = key.KeySize;
            if (key is AsymmetricSecurityKey securityKey)
            {
                keySize = securityKey.KeySize;
            }
            else if (key is JsonWebKey jsonWebKey)
            {
                JsonWebKeyConverter.TryConvertToSecurityKey(jsonWebKey, out SecurityKey convertedSecurityKey);
                if (convertedSecurityKey is AsymmetricSecurityKey convertedAsymmetricKey)
                    keySize = convertedAsymmetricKey.KeySize;
                else if (convertedSecurityKey is SymmetricSecurityKey)
                    throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX10704, key)));
            }
            else
            {
                throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX10704, key)));
            }

            if (willCreateSignatures)
            {
                if (MinimumAsymmetricKeySizeInBitsForSigningMap.ContainsKey(algorithm)
                && keySize < MinimumAsymmetricKeySizeInBitsForSigningMap[algorithm])
                    throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(key), LogHelper.FormatInvariant(LogMessages.IDX10630, key, LogHelper.MarkAsNonPII(MinimumAsymmetricKeySizeInBitsForSigningMap[algorithm]), LogHelper.MarkAsNonPII(keySize))));
            }
            else if (MinimumAsymmetricKeySizeInBitsForVerifyingMap.ContainsKey(algorithm)
                 && keySize < MinimumAsymmetricKeySizeInBitsForVerifyingMap[algorithm])
            {
                throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(key), LogHelper.FormatInvariant(LogMessages.IDX10631, key, LogHelper.MarkAsNonPII(MinimumAsymmetricKeySizeInBitsForVerifyingMap[algorithm]), LogHelper.MarkAsNonPII(keySize))));
            }
        }

        /// <summary>
        /// Verifies that the <paramref name="signature"/> over <paramref name="input"/> using the
        /// <see cref="SecurityKey"/> and <see cref="SignatureProvider.Algorithm"/> specified by this
        /// <see cref="SignatureProvider"/> are consistent.
        /// </summary>
        /// <param name="input">The bytes to generate the signature over.</param>
        /// <param name="signature">The value to verify against.</param>
        /// <returns>true if signature matches, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="input"/> is null or has length of 0.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="signature"/> is null or has length of 0.</exception>
        /// <exception cref="ObjectDisposedException">Thrown if <see cref="AsymmetricSignatureProvider.Dispose(bool)"/> has been called.</exception>
        /// <remarks>Verify is thread safe.</remarks>
        public override bool Verify(byte[] input, byte[] signature)
        {
            // The reason this method doesn't call through to: Verify(input, 0, input.Length, signature, 0, signature.Length)
            // Is because this method's contract is to check the entire signature, if the signature was truncated and signature.Length
            // was passed, the signature may verify.

            if (input == null || input.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(input));

            if (signature == null || signature.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(signature));

            if (_disposed)
            {
                CryptoProviderCache?.TryRemove(this);
                throw LogHelper.LogExceptionMessage(new ObjectDisposedException(GetType().ToString()));
            }

            AsymmetricAdapter asym = null;
            try
            {
                asym = _asymmetricAdapterObjectPool.Allocate();
                return asym.Verify(input, signature);
            }
            catch
            {
                CryptoProviderCache?.TryRemove(this);
                throw;
            }
            finally
            {
                if (asym != null)
                    _asymmetricAdapterObjectPool.Free(asym);
            }
        }

        /// <inheritdoc/>
        public override bool Verify(byte[] input, int inputOffset, int inputLength, byte[] signature, int signatureOffset, int signatureLength)
        {
            if (input == null || input.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(input));

            if (signature == null || signature.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(signature));

            if (inputOffset < 0)
                throw LogHelper.LogExceptionMessage(new ArgumentException(
                    LogHelper.FormatInvariant(
                        LogMessages.IDX10716,
                        LogHelper.MarkAsNonPII(nameof(inputOffset)),
                        LogHelper.MarkAsNonPII(inputOffset))));

            if (inputLength < 1)
                throw LogHelper.LogExceptionMessage(new ArgumentException(
                    LogHelper.FormatInvariant(
                        LogMessages.IDX10655,
                        LogHelper.MarkAsNonPII(nameof(inputLength)),
                        LogHelper.MarkAsNonPII(inputLength))));

            if (inputOffset + inputLength > input.Length)
                throw LogHelper.LogExceptionMessage(new ArgumentException(
                    LogHelper.FormatInvariant(
                        LogMessages.IDX10717,
                        LogHelper.MarkAsNonPII(nameof(inputOffset)),
                        LogHelper.MarkAsNonPII(nameof(inputLength)),
                        LogHelper.MarkAsNonPII(nameof(input)),
                        LogHelper.MarkAsNonPII(inputOffset),
                        LogHelper.MarkAsNonPII(inputLength),
                        LogHelper.MarkAsNonPII(input.Length))));

            if (signatureOffset < 0)
                throw LogHelper.LogExceptionMessage(new ArgumentException(
                    LogHelper.FormatInvariant(
                        LogMessages.IDX10716,
                        LogHelper.MarkAsNonPII(nameof(signatureOffset)),
                        LogHelper.MarkAsNonPII(signatureOffset))));

            if (signatureLength < 1)
                throw LogHelper.LogExceptionMessage(new ArgumentException(
                    LogHelper.FormatInvariant(
                        LogMessages.IDX10655,
                        LogHelper.MarkAsNonPII(nameof(signatureLength)),
                        LogHelper.MarkAsNonPII(signatureLength))));

            if (signatureOffset + signatureLength > signature.Length)
                throw LogHelper.LogExceptionMessage(new ArgumentException(
                    LogHelper.FormatInvariant(
                        LogMessages.IDX10717,
                        LogHelper.MarkAsNonPII(nameof(signatureOffset)),
                        LogHelper.MarkAsNonPII(nameof(signatureLength)),
                        LogHelper.MarkAsNonPII(nameof(signature)),
                        LogHelper.MarkAsNonPII(signatureOffset),
                        LogHelper.MarkAsNonPII(signatureLength),
                        LogHelper.MarkAsNonPII(signature.Length))));

            if (_disposed)
            {
                CryptoProviderCache?.TryRemove(this);
                throw LogHelper.LogExceptionMessage(new ObjectDisposedException(GetType().ToString()));
            }

            AsymmetricAdapter asym = null;
            try
            {
                asym = _asymmetricAdapterObjectPool.Allocate();
                if (signature.Length == signatureLength)
                {
                    return asym.VerifyUsingOffset(input, inputOffset, inputLength, signature);
                }
                else
                {
                    // AsymetricAdapter.Verify could do this.
                    // Having the logic here, handles EC and RSA. We can revisit when we start using spans in 3.1+.
                    byte[] signatureBytes = new byte[signatureLength];
                    Array.Copy(signature, 0, signatureBytes, 0, signatureLength);
                    return asym.VerifyUsingOffset(input, inputOffset, inputLength, signatureBytes);
                }
            }
            catch
            {
                CryptoProviderCache?.TryRemove(this);
                throw;
            }
            finally
            {
                if (asym != null)
                    _asymmetricAdapterObjectPool.Free(asym);
            }
        }

        /// <summary>
        /// Releases the resources used by the current instance.
        /// </summary>
        /// <param name="disposing">If true, release both managed and unmanaged resources; otherwise, release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                if (disposing)
                {
                    foreach (var item in _asymmetricAdapterObjectPool.Items)
                        item.Value?.Dispose();

                    CryptoProviderCache?.TryRemove(this);
                }
            }
        }
    }
}
