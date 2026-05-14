// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

/// <summary>
/// Contains derived types that are useful across multiple handlers / protocols.
/// </summary>
namespace Microsoft.IdentityModel.TestUtils
{
    public class DerivedAuthenticatedEncryptionProvider : AuthenticatedEncryptionProvider
    {
        public DerivedAuthenticatedEncryptionProvider()
            : base(Default.SymmetricEncryptionKey256, SecurityAlgorithms.Aes128CbcHmacSha256)
        {
        }
        public DerivedAuthenticatedEncryptionProvider(SymmetricSecurityKey key, string algorithm)
            : base(key, algorithm)
        {
        }

        public bool DecryptCalled { get; set; } = false;

        public bool EncryptCalled { get; set; } = false;

        public bool GetKeyBytesCalled { get; set; } = false;

        public bool IsSupportedAlgorithmCalled { get; set; } = false;

        public bool ValidateKeySizeCalled { get; set; } = false;

        public override byte[] Decrypt(byte[] ciphertext, byte[] authenticatedData, byte[] iv, byte[] authenticationTag)
        {
            DecryptCalled = true;
            return base.Decrypt(ciphertext, authenticatedData, iv, authenticationTag);
        }

        public override AuthenticatedEncryptionResult Encrypt(byte[] plaintext, byte[] authenticatedData)
        {
            EncryptCalled = true;
            return base.Encrypt(plaintext, authenticatedData);
        }

        protected override byte[] GetKeyBytes(SecurityKey key)
        {
            GetKeyBytesCalled = true;
            return base.GetKeyBytes(key);
        }

        public byte[] GetKeyBytesPublic(SecurityKey key)
        {
            GetKeyBytesCalled = true;
            return base.GetKeyBytes(key);
        }

        protected override bool IsSupportedAlgorithm(SecurityKey key, string algorithm)
        {
            IsSupportedAlgorithmCalled = true;
            return base.IsSupportedAlgorithm(key, algorithm);
        }

        public bool IsSupportedAlgorithmPublic(SecurityKey key, string algorithm)
        {
            IsSupportedAlgorithmCalled = true;
            return base.IsSupportedAlgorithm(key, algorithm);
        }

        protected override void ValidateKeySize(SecurityKey key, string algorithm)
        {
            ValidateKeySizeCalled = true;
            base.ValidateKeySize(key, algorithm);
        }

        public void ValidateKeySizePublic(SecurityKey key, string algorithm)
        {
            ValidateKeySizeCalled = true;

            base.ValidateKeySize(key, algorithm);
        }
    }

    /// <summary>
    /// Used by AuthenticationEncryptionProviderTests.
    /// </summary>
    public class AuthenticatedEncryptionCryptoProviderFactory : CryptoProviderFactory
    {
        public bool DisposeSignatureProvider { get; set; }

        public SymmetricSignatureProvider SymmetricSignatureProviderForSigning { get; set; }

        public SymmetricSignatureProvider SymmetricSignatureProviderForSigningCaching { get; set; }

        public SymmetricSignatureProvider SymmetricSignatureProviderForVerifying { get; set; }

        public override SignatureProvider CreateForSigning(SecurityKey key, string algorithm)
        {
            return SymmetricSignatureProviderForSigning;
        }

        public override SignatureProvider CreateForSigning(SecurityKey key, string algorithm, bool cacheSignatureProvider)
        {
            return base.CreateForSigning(key, algorithm, cacheSignatureProvider);
        }

        public override SignatureProvider CreateForVerifying(SecurityKey key, string algorithm)
        {
            return SymmetricSignatureProviderForVerifying;
        }

        public override SignatureProvider CreateForVerifying(SecurityKey key, string algorithm, bool cacheSignatureProvider)
        {
            return SymmetricSignatureProviderForVerifying;
        }

        public override void ReleaseSignatureProvider(SignatureProvider signatureProvider)
        {
            if (DisposeSignatureProvider)
                base.ReleaseSignatureProvider(signatureProvider);
        }
    }

    /// <summary>
    /// Allows distinguishing where exceptions are thrown
    /// </summary>
    public class DecryptAuthenticatedEncryptionCryptoProviderFactory : AuthenticatedEncryptionCryptoProviderFactory
    {
    }

    /// <summary>
    /// Allows distinguishing where exceptions are thrown
    /// </summary>
    public class EncryptAuthenticatedEncryptionCryptoProviderFactory : AuthenticatedEncryptionCryptoProviderFactory
    {
    }

    public class EncryptSymmetricSignatureProvider : SymmetricSignatureProvider
    {
        public EncryptSymmetricSignatureProvider(SecurityKey key, string algorithm) : base(key, algorithm)
        {
        }
        public EncryptSymmetricSignatureProvider(SecurityKey key, string algorithm, bool willCreateSignatures) : base(key, algorithm, willCreateSignatures)
        {
        }
    }
    public class DecryptSymmetricSignatureProvider : SymmetricSignatureProvider
    {
        public DecryptSymmetricSignatureProvider(SecurityKey key, string algorithm) : base(key, algorithm)
        {
        }
        public DecryptSymmetricSignatureProvider(SecurityKey key, string algorithm, bool willCreateSignatures) : base(key, algorithm, willCreateSignatures)
        {
        }
    }

    public class DecryptAuthenticatedEncryptionProvider : AuthenticatedEncryptionProvider
    {
        public DecryptAuthenticatedEncryptionProvider(SecurityKey key, string algorithm) : base(key, algorithm)
        {
        }
    }
    public class EncryptAuthenticatedEncryptionProvider : AuthenticatedEncryptionProvider
    {
        public EncryptAuthenticatedEncryptionProvider(SecurityKey key, string algorithm) : base(key, algorithm)
        {
        }
    }

    public class DerivedKeyWrapProvider : SymmetricKeyWrapProvider
    {
        public DerivedKeyWrapProvider(SecurityKey key, string algorithm)
            : base(key, algorithm)
        {
        }

        public bool GetSymmetricAlgorithmCalled { get; set; } = false;

        public bool IsSupportedAlgorithmCalled { get; set; } = false;

        public bool UnwrapKeyCalled { get; set; } = false;

        public bool WrapKeyCalled { get; set; } = false;

        protected override SymmetricAlgorithm GetSymmetricAlgorithm(SecurityKey key, string algorithm)
        {
            GetSymmetricAlgorithmCalled = true;
            return base.GetSymmetricAlgorithm(key, algorithm);
        }

        protected override bool IsSupportedAlgorithm(SecurityKey key, string algorithm)
        {
            IsSupportedAlgorithmCalled = true;
            return base.IsSupportedAlgorithm(key, algorithm);
        }

        public override byte[] UnwrapKey(byte[] keyBytes)
        {
            UnwrapKeyCalled = true;
            return base.UnwrapKey(keyBytes);
        }

        public override byte[] WrapKey(byte[] keyBytes)
        {
            WrapKeyCalled = true;
            return base.WrapKey(keyBytes);
        }
    }

    public class DerivedRsa : RSA
    {
        int _keySize;

        public DerivedRsa(int keySize)
        {
            _keySize = keySize;
        }

        public override int KeySize
        {
            get => _keySize;
            set => _keySize = value;
        }

        public override string SignatureAlgorithm => throw new NotImplementedException();

        public override string KeyExchangeAlgorithm => throw new NotImplementedException();

#if NET8_0_OR_GREATER
        [Obsolete("Formatter-based serialization is obsolete", DiagnosticId = "SYSLIB0051")]
#endif
        public override byte[] DecryptValue(byte[] rgb)
        {
            throw new NotImplementedException();
        }

#if NET8_0_OR_GREATER
        [Obsolete("Formatter-based serialization is obsolete", DiagnosticId = "SYSLIB0051")]
#endif
        public override byte[] EncryptValue(byte[] rgb)
        {
            throw new NotImplementedException();
        }

        public override RSAParameters ExportParameters(bool includePrivateParameters)
        {
            throw new NotImplementedException();
        }

        public override void ImportParameters(RSAParameters parameters)
        {
            throw new NotImplementedException();
        }
    }

    public class DerivedRsaKeyWrapProvider : RsaKeyWrapProvider
    {
        public DerivedRsaKeyWrapProvider(SecurityKey key, string algorithm, bool willUnwrap)
            : base(key, algorithm, willUnwrap)
        {
        }

        public bool IsSupportedAlgorithmCalled { get; set; } = false;

        public bool ResolveRsaAlgorithmCalled { get; set; } = false;

        public bool UnwrapKeyCalled { get; set; } = false;

        public bool WrapKeyCalled { get; set; } = false;

        protected override bool IsSupportedAlgorithm(SecurityKey key, string algorithm)
        {
            IsSupportedAlgorithmCalled = true;
            return base.IsSupportedAlgorithm(key, algorithm);
        }

        public override byte[] UnwrapKey(byte[] keyBytes)
        {
            UnwrapKeyCalled = true;
            return base.UnwrapKey(keyBytes);
        }

        public override byte[] WrapKey(byte[] keyBytes)
        {
            WrapKeyCalled = true;
            return base.WrapKey(keyBytes);
        }
    }

    /// <summary>
    /// Useful when one needs a security key to fault at different times.
    /// Each Get / Set has an exception associated with it that if set will throw
    /// instead of returning the value passed in the constructor.
    /// </summary>
    public class DerivedSecurityKey : SecurityKey
    {
        private string _keyId;
        private int _keySize;

        public DerivedSecurityKey(string keyId, int keySize)
        {
            _keyId = keyId;
            _keySize = keySize;
        }

        internal override string InternalId { get => _keyId; }

        public Exception ThrowOnGetKeyId { get; set; }

        public Exception ThrowOnSetKeyId { get; set; }

        public Exception ThrowOnGetKeySize { get; set; }

        public override string KeyId
        {
            get
            {
                if (ThrowOnGetKeyId != null)
                    throw ThrowOnGetKeyId;

                return _keyId;
            }

            set
            {
                if (ThrowOnSetKeyId != null)
                    throw ThrowOnSetKeyId;

                _keyId = value;
            }
        }

        public override int KeySize
        {
            get
            {
                if (ThrowOnGetKeySize != null)
                    throw ThrowOnGetKeySize;

                return _keySize;
            }
        }
    }

    public class DerivedSecurityToken : SecurityToken
    {
        public override string Id { get { return "DeriverSecurityToken"; } }

        public override string Issuer { get { return "DeriverSecurityToken.Issuer"; } }

        public override SecurityKey SecurityKey { get { return null; } }

        public override SecurityKey SigningKey { get; set; }

        public override DateTime ValidFrom { get { return DateTime.UtcNow; } }

        public override DateTime ValidTo { get { return DateTime.UtcNow + TimeSpan.FromDays(1); } }
    }

    /// <summary>
    /// This type is helpful to test difficult call graphs and simulate throwing.
    /// </summary>
    public class FaultingSymmetricSecurityKey : SymmetricSecurityKey
    {
        Exception _throwOnKeyProperty;
        KeyedHashAlgorithm _keyedHash;
        SymmetricSecurityKey _key;
        SymmetricAlgorithm _agorithm;
        byte[] _keyBytes;

        public FaultingSymmetricSecurityKey(SymmetricSecurityKey key, Exception throwOnKeyProperty, SymmetricAlgorithm algorithm = null, KeyedHashAlgorithm keyedHash = null, byte[] keyBytes = null)
            : base(keyBytes)
        {
            _throwOnKeyProperty = throwOnKeyProperty;
            _key = key;
            _keyedHash = keyedHash;
            _agorithm = algorithm;
            _keyBytes = keyBytes;
        }
        public override byte[] Key => _keyBytes;

        public override int KeySize
        {
            get
            {
                if (_throwOnKeyProperty != null)
                    throw _throwOnKeyProperty;

                return _key.KeySize;
            }
        }
    }

    public class NotAsymmetricOrSymmetricSecurityKey : SecurityKey
    {
        public override int KeySize
        {
            get { throw new NotImplementedException(); }
        }

        public static SecurityKey New { get { return new NotAsymmetricOrSymmetricSecurityKey(); } }
    }
}
