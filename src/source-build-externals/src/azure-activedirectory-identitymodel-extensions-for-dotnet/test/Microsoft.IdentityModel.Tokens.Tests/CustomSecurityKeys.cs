//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

/// <summary>
/// Derived types to simplify testing.
/// Helpful when throwing
/// </summary>
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

        internal override string InternalId  => "";
    }

}
