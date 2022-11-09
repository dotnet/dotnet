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
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class X509EncryptingCredentialsTests
    {
        [Theory, MemberData(nameof(ConstructorsTheoryData))]
        public void Constructors(X509EncryptingCredentialsTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.Constructors", theoryData);
            try
            {
                var encryptingCredentials = new X509EncryptingCredentials(theoryData.Certificate, theoryData.Alg, theoryData.Enc);
                var encryptingCredentialsFromCert = new X509EncryptingCredentials(theoryData.Certificate);
                IdentityComparer.AreEqual(encryptingCredentials.Certificate, encryptingCredentialsFromCert.Certificate, context);
                IdentityComparer.AreEqual(encryptingCredentials.Key, encryptingCredentialsFromCert.Key, context);
                IdentityComparer.AreEqual(encryptingCredentials.Certificate, encryptingCredentialsFromCert.Certificate, context);
                IdentityComparer.AreEqual(encryptingCredentials.Alg, encryptingCredentialsFromCert.Alg, context);
                IdentityComparer.AreEqual(encryptingCredentials.Enc, encryptingCredentialsFromCert.Enc, context);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception exception)
            {
                theoryData.ExpectedException.ProcessException(exception, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<X509EncryptingCredentialsTheoryData> ConstructorsTheoryData()
        {
            return new TheoryData<X509EncryptingCredentialsTheoryData>
            {
                new X509EncryptingCredentialsTheoryData
                {
                    Certificate = null,
                    Alg = SecurityAlgorithms.RsaOaepKeyWrap,
                    Enc = SecurityAlgorithms.Aes128CbcHmacSha256,
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000: The parameter 'certificate'"),
                    TestId = "NullCertificate"
                },
                new X509EncryptingCredentialsTheoryData
                {
                    Certificate = Default.Certificate,
                    Alg = String.Empty,
                    Enc = SecurityAlgorithms.Aes128CbcHmacSha256,
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000: The parameter 'alg'"),
                    TestId = "EmptyAlgString"
                },
                new X509EncryptingCredentialsTheoryData
                {
                    Certificate = Default.Certificate,
                    Alg = SecurityAlgorithms.RsaOaepKeyWrap,
                    Enc = String.Empty,
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000: The parameter 'enc'"),
                    TestId = "EmptyEncString"
                },
                new X509EncryptingCredentialsTheoryData
                {
                    Certificate = Default.Certificate,
                    Alg = null,
                    Enc = SecurityAlgorithms.Aes128CbcHmacSha256,
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000: The parameter 'alg'"),
                    TestId = "NullAlgString"
                },
                new X509EncryptingCredentialsTheoryData
                {
                    Certificate = Default.Certificate,
                    Alg = SecurityAlgorithms.RsaOaepKeyWrap,
                    Enc = null,
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000: The parameter 'enc'"),
                    TestId = "NullEncString"
                },
                new X509EncryptingCredentialsTheoryData
                {
                    Certificate = Default.Certificate,
                    Alg = SecurityAlgorithms.RsaOaepKeyWrap,
                    Enc = SecurityAlgorithms.Aes128CbcHmacSha256,
                    TestId = "ValidTest"
                }
            };
        }
    }

    public class X509EncryptingCredentialsTheoryData : TheoryDataBase
    {
        public X509Certificate2 Certificate { get; set; }
        public string Alg { get; set; }
        public string Enc { get; set; }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
