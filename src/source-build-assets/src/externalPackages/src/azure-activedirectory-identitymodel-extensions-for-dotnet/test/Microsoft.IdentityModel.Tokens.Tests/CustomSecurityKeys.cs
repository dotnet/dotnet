// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

// Derived types to simplify testing.
// Helpful when throwing
namespace Microsoft.IdentityModel.TestUtils
{
    public class CustomRsaSecurityKey : RsaSecurityKey
    {
        private int _keySize;
        private PrivateKeyStatus _privateKeyStatus;

        public CustomRsaSecurityKey(int keySize, PrivateKeyStatus privateKeyStatus, RSAParameters parameters)
            : base(parameters)
        {
            _keySize = keySize;
            _privateKeyStatus = privateKeyStatus;
        }

        public CustomRsaSecurityKey(int keySize, PrivateKeyStatus privateKeyStatus, RSAParameters parameters, string InternalId)
            : base(parameters)
        {
            _keySize = keySize;
            _privateKeyStatus = privateKeyStatus;
        }

#pragma warning disable CS0672
        public override bool HasPrivateKey => true;
#pragma warning restore CS0672

        public override PrivateKeyStatus PrivateKeyStatus => _privateKeyStatus;

        public override int KeySize => _keySize;

        internal override string InternalId => "";
    }

}
