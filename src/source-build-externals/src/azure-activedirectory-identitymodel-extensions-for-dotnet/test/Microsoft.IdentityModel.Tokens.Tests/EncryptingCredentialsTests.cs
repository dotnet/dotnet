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
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class EncryptingCredentialsTests
    {
        //public EncryptingCredentials(SecurityKey key, string alg, string enc)
        [Theory, MemberData(nameof(ConstructorATheoryData))]
        public void ConstructorA(EncryptingCredentialsTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ConstructorA", theoryData);
            try
            {
                var encryptingCredentials = new EncryptingCredentials(theoryData.Key, theoryData.Alg, theoryData.Enc);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception exception)
            {
                theoryData.ExpectedException.ProcessException(exception, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        //Used in scenarios when a key represents a 'shared' symmetric key
        //public EncryptingCredentials(SecurityKey key, string enc)
        [Theory, MemberData(nameof(ConstructorBTheoryData))]
        public void ConstructorB(EncryptingCredentialsTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ConstructorB", theoryData);
            try
            {
                var encryptingCredentials = new EncryptingCredentials((SymmetricSecurityKey)theoryData.Key, theoryData.Enc);
                //Algorithm value should be 'None'
                IdentityComparer.AreEqual(encryptingCredentials.Alg, SecurityAlgorithms.None, context);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception exception)
            {
                theoryData.ExpectedException.ProcessException(exception, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<EncryptingCredentialsTheoryData> ConstructorATheoryData()
        {
            return new TheoryData<EncryptingCredentialsTheoryData>
            {
                new EncryptingCredentialsTheoryData
                {
                    Key = null,
                    Alg = SecurityAlgorithms.RsaOaepKeyWrap,
                    Enc = SecurityAlgorithms.Aes128CbcHmacSha256,
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000: The parameter 'key'"),
                    TestId = "NullKey"
                },
                new EncryptingCredentialsTheoryData
                {
                    Key = Default.AsymmetricEncryptionKeyPublic,
                    Alg = String.Empty,
                    Enc = SecurityAlgorithms.Aes128CbcHmacSha256,
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000: The parameter 'alg'"),
                    TestId = "EmptyAlgString"
                },
                new EncryptingCredentialsTheoryData
                {
                    Key = Default.AsymmetricEncryptionKeyPublic,
                    Alg = SecurityAlgorithms.RsaOaepKeyWrap,
                    Enc = String.Empty,
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000: The parameter 'enc'"),
                    TestId = "EmptyEncString"
                },
                new EncryptingCredentialsTheoryData
                {
                    Key = Default.AsymmetricEncryptionKeyPublic,
                    Alg = null,
                    Enc = SecurityAlgorithms.Aes128CbcHmacSha256,
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000: The parameter 'alg'"),
                    TestId = "NullAlgString"
                },
                new EncryptingCredentialsTheoryData
                {
                    Key = Default.AsymmetricEncryptionKeyPublic,
                    Alg = SecurityAlgorithms.RsaOaepKeyWrap,
                    Enc = null,
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000: The parameter 'enc'"),
                    TestId = "NullEncString"
                },
                new EncryptingCredentialsTheoryData
                {
                    Key = Default.AsymmetricEncryptionKeyPublic,
                    Alg = SecurityAlgorithms.RsaOaepKeyWrap,
                    Enc = SecurityAlgorithms.Aes128CbcHmacSha256,
                    TestId = "ValidTest"
                }
            };
        }

        public static TheoryData<EncryptingCredentialsTheoryData> ConstructorBTheoryData()
        {
            return new TheoryData<EncryptingCredentialsTheoryData>
            {
                new EncryptingCredentialsTheoryData
                {
                    Key = null,
                    Enc = SecurityAlgorithms.Aes128CbcHmacSha256,
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000: The parameter 'key'"),
                    TestId = "NullKey"
                },
                new EncryptingCredentialsTheoryData
                {
                    Key = Default.SymmetricEncryptionKey128,
                    Enc = String.Empty,
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000: The parameter 'enc'"),
                    TestId = "EmptyEncString"
                },
                new EncryptingCredentialsTheoryData
                {
                    Key = Default.SymmetricEncryptionKey128,
                    Enc = null,
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000: The parameter 'enc'"),
                    TestId = "NullEncString"
                },
                new EncryptingCredentialsTheoryData
                {
                    Key = Default.SymmetricEncryptionKey128,
                    Enc = SecurityAlgorithms.Aes128CbcHmacSha256,
                    TestId = "ValidTest"
                }
            };
        }
    }

    public class EncryptingCredentialsTheoryData : TheoryDataBase
    {
        public SecurityKey Key { get; set; }
        public string Alg { get; set; }
        public string Enc { get; set; }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
