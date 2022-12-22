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

using KEY = Microsoft.IdentityModel.TestUtils.KeyingMaterial;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class ECDsaAdapterTests
    {
        [Theory, MemberData(nameof(CreateECDsaFromJsonWebKeyTheoryData))]
        public void CreateECDsa(JsonWebKeyTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateECDsa", theoryData);
            try
            {
                var jsonWebKey = new JsonWebKey
                {
                    Crv = theoryData.Crv,
                    X = theoryData.X,
                    Y = theoryData.Y,
                    D = theoryData.D,
                };
                var ecdsaAdapter = ECDsaAdapter.Instance;
                ecdsaAdapter.CreateECDsa(jsonWebKey, theoryData.UsePrivateKey);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JsonWebKeyTheoryData> CreateECDsaFromJsonWebKeyTheoryData
        {
            get => new TheoryData<JsonWebKeyTheoryData>
            {
                new JsonWebKeyTheoryData {
                    First = true,
                    UsePrivateKey = false,
                    Crv = null,
                    X = KEY.JsonWebKeyP256.X,
                    Y = KEY.JsonWebKeyP256.Y,
                    TestId = "nullCrv",
                    ExpectedException = ExpectedException.ArgumentNullException(),
                },
                new JsonWebKeyTheoryData {
                    UsePrivateKey = false,
                    Crv = "P-255",
                    X = KEY.JsonWebKeyP256.X,
                    Y = KEY.JsonWebKeyP256.Y,
                    TestId = "unknownCrv",
                    // ignoreInnerException - throws different inner exceptions on different ECDsaAdapter flows
                    ExpectedException = ExpectedException.CryptographicException("IDX10689:", ignoreInnerException: true),
                },
                new JsonWebKeyTheoryData {
                    UsePrivateKey = false,
                    Crv = "P-256",
                    X = null,
                    Y = KEY.JsonWebKeyP256.Y,
                    TestId = "nullXparam",
                    ExpectedException = ExpectedException.ArgumentNullException(),
                },
                new JsonWebKeyTheoryData {
                    UsePrivateKey = false,
                    Crv = "P-256",
                    X = KEY.JsonWebKeyP256.X,
                    Y = null,
                    TestId = "nullYparam",
                    ExpectedException = ExpectedException.ArgumentNullException(),
                },
                new JsonWebKeyTheoryData {
                    UsePrivateKey = true,
                    Crv = "P-256",
                    X = KEY.JsonWebKeyP256.X,
                    Y = KEY.JsonWebKeyP256.Y,
                    D = null,
                    TestId = "nullDparam",
                    // ignoreInnerException - throws different inner exceptions on different ECDsaAdapter flows
                    ExpectedException = ExpectedException.CryptographicException("IDX10689:", typeof(ArgumentNullException)),
                },
                new JsonWebKeyTheoryData {
                    UsePrivateKey = false,
                    Crv = "P-256",
                    X = KEY.JsonWebKeyP256.X + "_dummy_data",
                    Y = KEY.JsonWebKeyP256.Y,
                    TestId = "xLongerThanY",
                    // ignoreInnerException - throws different inner exceptions on different ECDsaAdapter flows
                    ExpectedException = ExpectedException.CryptographicException("IDX10689:", ignoreInnerException: true),
                },
                new JsonWebKeyTheoryData {
                    UsePrivateKey = false,
                    Crv = "P-256",
                    X = KEY.JsonWebKeyP256.X.Remove(KEY.JsonWebKeyP256.X.Length - 1),
                    Y = KEY.JsonWebKeyP256.Y,
                    TestId = "xShorterThanY",
                    // ignoreInnerException - throws different inner exceptions on different ECDsaAdapter flows
                    ExpectedException = ExpectedException.CryptographicException("IDX10689:", ignoreInnerException: true),
                },
                new JsonWebKeyTheoryData {
                    UsePrivateKey = false,
                    Crv = "P-256",
                    X = KEY.JsonWebKeyP256.X,
                    Y = KEY.JsonWebKeyP256.Y + "_dummy_data",
                    TestId = "YLongerThanX",
                    // ignoreInnerException - throws different inner exceptions on different ECDsaAdapter flows
                    ExpectedException = ExpectedException.CryptographicException("IDX10689:", ignoreInnerException: true),
                },
                new JsonWebKeyTheoryData {
                    UsePrivateKey = true,
                    Crv = "P-256",
                    X = KEY.JsonWebKeyP256.X,
                    Y = KEY.JsonWebKeyP256.Y,
                    D = KEY.JsonWebKeyP256.D + "_dummy_data",
                    TestId = "dLongerThanXandY",
                    // ignoreInnerException - throws different inner exceptions on different ECDsaAdapter flows
                    ExpectedException = ExpectedException.CryptographicException("IDX10689:", ignoreInnerException: true),
                },
                new JsonWebKeyTheoryData {
                    UsePrivateKey = true,
                    Crv = "P-256",
                    X = KEY.JsonWebKeyP384.X,
                    Y = KEY.JsonWebKeyP384.Y,
                    D = KEY.JsonWebKeyP384.D,
                    TestId = "paramsMoreBytesThanCurve",
                    // ignoreInnerException - throws different inner exceptions on different platforms and ECDsaAdapter flows
                    ExpectedException = ExpectedException.CryptographicException("IDX10689:", ignoreInnerException: true),
                },
                new JsonWebKeyTheoryData {
                    UsePrivateKey = true,
                    Crv = "P-256",
                    X = "",
                    Y = "",
                    D = "",
                    TestId = "emptyParams",
                    // ignoreInnerException - throws different inner exceptions on different platforms and ECDsaAdapter flows
                    ExpectedException = ExpectedException.CryptographicException("IDX10689:", ignoreInnerException: true),
                },
                new JsonWebKeyTheoryData {
                    UsePrivateKey = false,
                    Crv = "P-256",
                    X = KEY.JsonWebKeyP256.X,
                    Y = KEY.JsonWebKeyP256.Y,
                    D = null,
                    TestId = "successNullDnoPrivateKey",
                },
                new JsonWebKeyTheoryData {
                    UsePrivateKey = true,
                    Crv = "P-256",
                    X = KEY.JsonWebKeyP256.X,
                    Y = KEY.JsonWebKeyP256.Y,
                    D = KEY.JsonWebKeyP256.D,
                    TestId = "successfulCall",
                },
            };
        }

        public class JsonWebKeyTheoryData : TheoryDataBase
        {
            public string Crv { get; set; }

            public string D { get; set; }

            public bool UsePrivateKey { get; set; }

            public string X { get; set; }

            public string Y { get; set; }
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
