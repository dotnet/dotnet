// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Xml.Tests
{
    /// <summary>
    /// DSigCryptoProviderFactory and DSignatureProvider are used to simulate failures and get deeper in the stack
    /// </summary>
    public class DSigCryptoProviderFactory : CryptoProviderFactory
    {
        public DSigCryptoProviderFactory() { }

        public SignatureProvider SignatureProvider { get; set; }

        public override SignatureProvider CreateForSigning(SecurityKey key, string algorithm)
        {
            return SignatureProvider;
        }

        public override SignatureProvider CreateForVerifying(SecurityKey key, string algorithm)
        {
            return SignatureProvider;
        }
    }

    public class DSigSignatureProvider : SignatureProvider
    {
        public DSigSignatureProvider(SecurityKey key, string algorithm)
            : base(key, algorithm)
        { }

        protected override void Dispose(bool disposing)
        {
        }

        public override byte[] Sign(byte[] input)
        {
            return Encoding.UTF8.GetBytes("SignedBytes");
        }

        public override bool Verify(byte[] input, byte[] signature)
        {
            return VerifyResult;
        }

        public override bool Verify(byte[] input, int inputOffset, int inputLength, byte[] signature, int signatureOffset, int signatureLength) => throw new System.NotImplementedException();

        public bool VerifyResult { get; set; } = true;
    }
}
