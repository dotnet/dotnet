// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;


/// <summary>
/// Derived types to simplify testing.
/// Helpful when throwing
/// </summary>
namespace Microsoft.IdentityModel.TestUtils
{
    public class CustomCryptoProvider : ICryptoProvider
    {
        public CustomCryptoProvider()
        {
        }

        public CustomCryptoProvider(string[] supportedAlgorithms)
        {
            SupportedAlgorithms.AddRange(supportedAlgorithms);
        }

        public SignatureProvider SignatureProvider { get; set; }

        public List<string> SupportedAlgorithms { get; set; } = new List<string>();

        public IList<string> AdditionalHashAlgorithms { get; private set; } = new List<string>();

        public HashAlgorithm HashAlgorithm { get; set; }
        
        public KeyWrapProvider KeyWrapProvider { get; set; }

        public RsaKeyWrapProvider RsaKeyWrapProvider { get; set; }

        public bool IsSupportedResult { get; set; } = false;

        public bool CreateCalled { get; set; } = false;

        public bool IsSupportedAlgorithmCalled { get; set; } = false;

        public bool ReleaseCalled { get; set; } = false;

        public object Create(string algorithm, params object[] args)
        {
            CreateCalled = true;
            
            if (IsHashAlgorithm(algorithm))
                return HashAlgorithm;
            else
                return SignatureProvider;
        }

        public bool IsHashAlgorithm(string algorithm)
        {
            switch (algorithm)
            {
                case SecurityAlgorithms.Sha256:
                case SecurityAlgorithms.Sha256Digest:
                case SecurityAlgorithms.Sha384:
                case SecurityAlgorithms.Sha384Digest:
                case SecurityAlgorithms.Sha512:
                case SecurityAlgorithms.Sha512Digest:
                    return true;
            }


            foreach (var alg in AdditionalHashAlgorithms)
                if (alg.Equals(algorithm))
                    return true;

            return false;

        }

        public bool IsSupportedAlgorithm(string algorithm, params object[] args)
        {
            IsSupportedAlgorithmCalled = true;
            foreach (var alg in SupportedAlgorithms)
                if (alg.Equals(algorithm, StringComparison.OrdinalIgnoreCase))
                    return true;

            return IsSupportedResult;
        }

        public void Release(object cryptoObject)
        {
            ReleaseCalled = true;
            if (cryptoObject as ICustomObject != null)
                return;

            if (cryptoObject is IDisposable disposableObject)
                disposableObject.Dispose();
        }
    }

    public class CustomCryptoProviderFactory : CryptoProviderFactory
    {
        public CustomCryptoProviderFactory() : base(new InMemoryCryptoProviderCache(new CryptoProviderCacheOptions(), TaskCreationOptions.None, 50))
        {
        }

        public CustomCryptoProviderFactory(ICryptoProvider cryptoProvider)
        {
            CustomCryptoProvider = cryptoProvider;
        }

        public CustomCryptoProviderFactory(string[] supportedAlgorithms)
        {
            SupportedAlgorithms.AddRange(supportedAlgorithms);
        }

        public override SignatureProvider CreateForSigning(SecurityKey key, string algorithm)
        {
            CreateForSigningCalled = true;
            if (CustomCryptoProvider != null && CustomCryptoProvider.IsSupportedAlgorithm(algorithm, key))
                return CustomCryptoProvider.Create(algorithm, key) as SignatureProvider;

            if (SigningSignatureProvider != null && CacheSignatureProviders)
                CryptoProviderCache.TryAdd(SigningSignatureProvider);

            return SigningSignatureProvider;
        }

        public bool CreateForSigningCalled { get; set; } = false;

        public override SignatureProvider CreateForVerifying(SecurityKey key, string algorithm)
        {
            CreateForVerifyingCalled = true;
            if (CustomCryptoProvider != null && CustomCryptoProvider.IsSupportedAlgorithm(algorithm, key))
                return CustomCryptoProvider.Create(algorithm, key) as SignatureProvider;

            if (VerifyingSignatureProvider != null && CacheSignatureProviders)
                CryptoProviderCache.TryAdd(VerifyingSignatureProvider);

            return VerifyingSignatureProvider;
        }

        public bool CreateForVerifyingCalled { get; set; } = false;

        public override HashAlgorithm CreateHashAlgorithm(string algorithm)
        {
            if (CustomCryptoProvider != null && CustomCryptoProvider.IsSupportedAlgorithm(algorithm))
                return CustomCryptoProvider.Create(algorithm) as HashAlgorithm;

            return HashAlgorithm;
        }

        public override KeyedHashAlgorithm CreateKeyedHashAlgorithm(byte[] keyBytes, string algorithm)
        {
            CreateKeyedHashAlgorithmCalled = true;

            if (KeyedHashAlgorithm != null)
                return KeyedHashAlgorithm;

            return base.CreateKeyedHashAlgorithm(keyBytes, algorithm);
        }

        public bool CreateKeyedHashAlgorithmCalled { get; set; } = false;

        public HashAlgorithm HashAlgorithm { get; set; }

        public KeyedHashAlgorithm KeyedHashAlgorithm { get; set; }

        public override bool IsSupportedAlgorithm(string algorithm)
        {

            IsSupportedAlgorithmCalled = true;
            foreach (var alg in SupportedAlgorithms)
                if (alg.Equals(algorithm, StringComparison.OrdinalIgnoreCase))
                    return true;

            return false;
        }

        public override bool IsSupportedAlgorithm(string algorithm, SecurityKey key)
        {
            IsSupportedAlgorithmCalled = true;
            foreach (var alg in SupportedAlgorithms)
                if (alg.Equals(algorithm, StringComparison.OrdinalIgnoreCase))
                    return true;

            return false;
        }

        public bool IsSupportedAlgorithmCalled { get; set; } = false;

        public override void ReleaseHashAlgorithm(HashAlgorithm hashAlgorithm)
        {
            ReleaseHashAlgorithmCalled = true;
            if (CustomCryptoProvider != null)
                CustomCryptoProvider.Release(hashAlgorithm);
            else
                hashAlgorithm.Dispose();
        }

        public bool ReleaseHashAlgorithmCalled { get; set; } = false;

        public override void ReleaseSignatureProvider(SignatureProvider signatureProvider)
        {
            ReleaseSignatureProviderCalled = true;
            if (CustomCryptoProvider != null)
                CustomCryptoProvider.Release(signatureProvider);
            else
                signatureProvider.Dispose();
        }

        public bool ReleaseSignatureProviderCalled { get; set; } = false;

        public SignatureProvider SigningSignatureProvider { get; set; }

        public List<string> SupportedAlgorithms { get; set; } = new List<string>();

        public SignatureProvider VerifyingSignatureProvider { get; set; }
    }

    public class CustomHashAlgorithm : SHA256, ICustomObject
    {
        public bool DisposeCalled { get; set; } = false;

        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        protected override void Dispose(bool disposing)
        {
            DisposeCalled = true;
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            throw new NotImplementedException();
        }

        protected override byte[] HashFinal()
        {
            throw new NotImplementedException();
        }
    }

    public class CustomKeyedHashAlgorithm : KeyedHashAlgorithm
    {
        byte[] _key;

        public CustomKeyedHashAlgorithm(byte[] key)
        {
            _key = key;
        }

        protected CustomKeyedHashAlgorithm()
        {
        }

        public override byte[] Key
        {
            get
            {
                if (ThrowOnKey != null)
                    throw ThrowOnKey;

                return _key;
            }
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            if (ThrowOnHashCore != null)
                throw ThrowOnHashCore;

            return;
        }

        protected override byte[] HashFinal()
        {
            if (ThrowOnHashFinal != null)
                throw ThrowOnHashFinal;

            return new byte[256];
        }

        public override void Initialize()
        {
        }

        public Exception ThrowOnKey { get; set; }

        public Exception ThrowOnHashCore { get; set; }

        public Exception ThrowOnHashFinal { get; set; }
    }

    public class CustomAsymmetricSignatureProvider : AsymmetricSignatureProvider
    {
        public CustomAsymmetricSignatureProvider(SecurityKey key, string algorithm, bool willCreateSignatures)
            : base(key, algorithm, willCreateSignatures)
        { }

        protected override void Dispose(bool disposing)
        {
            if (ThrowOnDispose != null)
                throw ThrowOnDispose;

            base.Dispose(disposing);
        }

        public override byte[] Sign(byte[] input)
        {
            if (ThrowOnSign != null)
                throw ThrowOnSign;

            return base.Sign(input);
        }

        public Exception ThrowOnDispose { get; set; }

        public Exception ThrowOnVerify { get; set; }

        public Exception ThrowOnSign { get; set; }

        public override bool Verify(byte[] input, byte[] signature)
        {
            if (ThrowOnVerify != null)
                throw ThrowOnVerify;

            return base.Verify(input, signature);
        }
    }

    public class CustomSymmetricSignatureProvider : SymmetricSignatureProvider
    {
        public CustomSymmetricSignatureProvider(SecurityKey key, string algorithm, bool willCreateSignatures )
            :base(key, algorithm, willCreateSignatures)
        { }

        protected override void Dispose(bool disposing)
        {
            if (ThrowOnDispose != null)
                throw ThrowOnDispose;

            base.Dispose(disposing);
        }
        protected override KeyedHashAlgorithm GetKeyedHashAlgorithm(byte[] keyBytes, string algorithm)
        {
            if (KeyedHashAlgorithmPublic != null)
                return KeyedHashAlgorithmPublic;

            return base.GetKeyedHashAlgorithm(keyBytes, algorithm);
        }

        public KeyedHashAlgorithm KeyedHashAlgorithmPublic { get; set; }

        public override byte[] Sign(byte[] input)
        {
            if (ThrowOnSign != null)
                throw ThrowOnSign;

            return base.Sign(input);
        }

        public Exception ThrowOnDispose { get; set; }

        public Exception ThrowOnSign { get; set; }

        public Exception ThrowOnVerify { get; set; }

        public override bool Verify(byte[] input, byte[] signature)
        {
            if (ThrowOnVerify != null)
                throw ThrowOnVerify;

            return base.Verify(input, signature);
        }
    }

    public class CustomSignatureProvider : SignatureProvider
    {
        public CustomSignatureProvider(SecurityKey key, string algorithm)
            : this(key, algorithm, true)
        { }

        public CustomSignatureProvider(SecurityKey key, string algorithm, bool willCreateSignatures)
            : this(key, algorithm, willCreateSignatures, null)
        {
        }

        public CustomSignatureProvider(SecurityKey key, string algorithm, bool willCreateSignatures, CryptoProviderCache cryptoProviderCache)
            : base(key, algorithm)
        {
            CryptoProviderCache = cryptoProviderCache;
            WillCreateSignatures = willCreateSignatures;
        }

        public bool DisposeCalled { get; set; } = false;

        public bool SignCalled { get; set; } = false;

        public Exception ThrowOnVerify { get; set; }

        public Exception ThrowOnSign { get; set; }

        public bool VerifyCalled { get; set; } = false;

        public override byte[] Sign(byte[] input)
        {
            SignCalled = true;
            if (ThrowOnSign != null)
                throw ThrowOnSign;

            return Encoding.UTF8.GetBytes("SignedBytes");
        }

        public bool VerifyResult { get; set; } = true;

        public override bool Verify(byte[] input, byte[] signature)
        {
            VerifyCalled = true;
            if (ThrowOnVerify != null)
                throw ThrowOnVerify;

            return VerifyResult;
        }

        protected override void Dispose(bool disposing)
        {
            DisposeCalled = true;
        }

        public override bool Verify(byte[] input, int inputOffset, int inputLength, byte[] signature, int signatureOffset, int signatureLength) => throw new NotImplementedException();
    }

    public interface ICustomObject { }
}
