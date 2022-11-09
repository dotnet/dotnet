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
    public class JsonWebKeyConverterTest
    {
        [Theory, MemberData(nameof(ConvertSecurityKeyToJsonWebKeyTheoryData))]
        public void ConvertSecurityKeyToJsonWebKey(JsonWebKeyConverterTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ConvertSecurityKeyToJsonWebKey", theoryData);
            try
            {
                var convertedKey = JsonWebKeyConverter.ConvertFromSecurityKey(theoryData.SecurityKey);

                theoryData.ExpectedException.ProcessNoException(context);
                IdentityComparer.AreEqual(convertedKey, theoryData.JsonWebKey, context);
                if (convertedKey.ConvertedSecurityKey.GetType() != theoryData.SecurityKey.GetType())
                    context.AddDiff($"theoryData.JsonWebKey.RelatedSecurityKey.GetType(): '{theoryData.JsonWebKey.ConvertedSecurityKey.GetType()}' != theoryData.SecurityKey.GetType(): '{theoryData.SecurityKey.GetType()}'.");
            }
            catch(Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(ConvertToJsonWebKeyToSecurityKeyTheoryData))]
        public void ConvertJsonWebKeyToSecurityKey(JsonWebKeyConverterTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ConvertJsonWebKeyToSecurityKey", theoryData);
            try
            {
                var wasConverted = JsonWebKeyConverter.TryConvertToSecurityKey(theoryData.JsonWebKey, out SecurityKey securityKey);
                theoryData.ExpectedException.ProcessNoException(context);
                IdentityComparer.AreEqual(securityKey, theoryData.SecurityKey, context);
                if (theoryData.JsonWebKey.ConvertedSecurityKey.GetType() != theoryData.SecurityKey.GetType())
                    context.AddDiff($"theoryData.JsonWebKey.RelatedSecurityKey.GetType(): '{theoryData.JsonWebKey.ConvertedSecurityKey.GetType()}' != theoryData.SecurityKey.GetType(): '{theoryData.SecurityKey.GetType()}'.");
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(ConvertX509SecurityKeyToJsonWebKeyTheoryData))]
        public void ConvertX509SecurityKeyAsRsaSecurityKeyToJsonWebKey(JsonWebKeyConverterTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ConvertX509SecurityKeyToJsonWebKeyTheoryData", theoryData);
            try
            {
                var convertedKey = JsonWebKeyConverter.ConvertFromX509SecurityKey(theoryData.SecurityKey as X509SecurityKey, theoryData.RepresentAsRsaKey);

                theoryData.ExpectedException.ProcessNoException(context);
                IdentityComparer.AreEqual(convertedKey, theoryData.JsonWebKey, context);

                var expectedConvertedKeyType = theoryData.RepresentAsRsaKey == true ? typeof(RsaSecurityKey) : typeof(X509SecurityKey);
                if (convertedKey.ConvertedSecurityKey.GetType() != expectedConvertedKeyType)
                    context.AddDiff($"convertedKey.ConvertedSecurityKey.GetType(): '{convertedKey.ConvertedSecurityKey.GetType()}' != expectedConvertedKeyType: '{expectedConvertedKeyType}'.");
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JsonWebKeyConverterTheoryData> ConvertSecurityKeyToJsonWebKeyTheoryData
        {
            get
            {
                var theoryData = ConversionKeyTheoryData;
#if !NET472 && !NET_CORE
                theoryData.Add(new JsonWebKeyConverterTheoryData
                {
                    SecurityKey = KeyingMaterial.Ecdsa256Key,
                    JsonWebKey = KeyingMaterial.JsonWebKeyP256_Public,
                    ExpectedException = ExpectedException.NotSupportedException("IDX10674"),
                    TestId = "SecurityKeyNotSupported"
                });
#endif
                return theoryData;
            }
        }

        public static TheoryData<JsonWebKeyConverterTheoryData> ConvertToJsonWebKeyToSecurityKeyTheoryData
        {
            get 
            {
                return ConversionKeyTheoryData;
            }
        }

        public static TheoryData<JsonWebKeyConverterTheoryData> ConversionKeyTheoryData
        {
            get
            {
                var theoryData = new TheoryData<JsonWebKeyConverterTheoryData>();

                // need to adjust the kid to match as the keys have different id's.
                var securityKey = KeyingMaterial.RsaSecurityKey_2048;
                securityKey.KeyId = KeyingMaterial.JsonWebKeyRsa_2048.KeyId;
                theoryData.Add(new JsonWebKeyConverterTheoryData
                {
                    First = true,
                    SecurityKey = securityKey,
                    JsonWebKey = KeyingMaterial.JsonWebKeyRsa_2048,
                    TestId = nameof(KeyingMaterial.RsaSecurityKey_2048)
                });

                securityKey = KeyingMaterial.RsaSecurityKey_2048_Public;
                securityKey.KeyId = KeyingMaterial.JsonWebKeyRsa_2048_Public.KeyId;
                theoryData.Add(new JsonWebKeyConverterTheoryData
                {
                    SecurityKey = securityKey,
                    JsonWebKey = KeyingMaterial.JsonWebKeyRsa_2048_Public,
                    TestId = nameof(KeyingMaterial.RsaSecurityKey_2048_Public)
                });

                theoryData.Add(new JsonWebKeyConverterTheoryData
                {
                    SecurityKey = KeyingMaterial.DefaultSymmetricSecurityKey_64,
                    JsonWebKey = KeyingMaterial.JsonWebKeySymmetric64,
                    TestId = nameof(KeyingMaterial.DefaultSymmetricSecurityKey_64)
                });

                theoryData.Add(new JsonWebKeyConverterTheoryData
                {
                    SecurityKey = KeyingMaterial.DefaultX509Key_2048_With_KeyId,
                    JsonWebKey = KeyingMaterial.JsonWebKeyX509_2048_With_KeyId,
                    TestId = nameof(KeyingMaterial.DefaultX509Key_2048_With_KeyId)
                });

                theoryData.Add(new JsonWebKeyConverterTheoryData
                {
                    SecurityKey = KeyingMaterial.DefaultX509Key_2048,
                    JsonWebKey = KeyingMaterial.JsonWebKeyX509_2048,
                    TestId = nameof(KeyingMaterial.DefaultX509Key_2048)
                });

                theoryData.Add(new JsonWebKeyConverterTheoryData
                {
                    SecurityKey = KeyingMaterial.DefaultX509Key_2048_Public,
                    JsonWebKey = KeyingMaterial.JsonWebKeyX509_2048_Public,
                    TestId = nameof(KeyingMaterial.DefaultX509Key_2048_Public)
                });
#if NET472 || NET_CORE
                theoryData.Add(new JsonWebKeyConverterTheoryData
                {
                    SecurityKey = KeyingMaterial.Ecdsa256Key_Public,
                    JsonWebKey = KeyingMaterial.CreateJsonWebKeyEC(
                        JsonWebKeyECTypes.P256, 
                        KeyingMaterial.Ecdsa256Key_Public.KeyId,
                        null,
                        Base64UrlEncoder.Encode(KeyingMaterial.Ecdsa256Parameters_Public.Q.X), 
                        Base64UrlEncoder.Encode(KeyingMaterial.Ecdsa256Parameters_Public.Q.Y)
                    ),
                    TestId = nameof(KeyingMaterial.Ecdsa256Key_Public)
                });

                theoryData.Add(new JsonWebKeyConverterTheoryData
                {
                    SecurityKey = KeyingMaterial.Ecdsa256Key,
                    JsonWebKey = KeyingMaterial.CreateJsonWebKeyEC(
                        JsonWebKeyECTypes.P256,
                        KeyingMaterial.Ecdsa256Key.KeyId,
                        Base64UrlEncoder.Encode(KeyingMaterial.Ecdsa256Parameters.D),
                        Base64UrlEncoder.Encode(KeyingMaterial.Ecdsa256Parameters.Q.X),
                        Base64UrlEncoder.Encode(KeyingMaterial.Ecdsa256Parameters.Q.Y)
                    ),
                    TestId = nameof(KeyingMaterial.Ecdsa256Key)
                });
#endif

                return theoryData;
            }
        }

        public static TheoryData<JsonWebKeyConverterTheoryData> ConvertX509SecurityKeyToJsonWebKeyTheoryData
        {
            get
            {
                var theoryData = new TheoryData<JsonWebKeyConverterTheoryData>();
                theoryData.Add(new JsonWebKeyConverterTheoryData
                {
                    SecurityKey = KeyingMaterial.DefaultX509Key_2048_With_KeyId,
                    JsonWebKey = KeyingMaterial.JsonWebKeyX509_2048_With_KeyId,
                    TestId = nameof(KeyingMaterial.DefaultX509Key_2048_With_KeyId)
                });

                theoryData.Add(new JsonWebKeyConverterTheoryData
                {
                    SecurityKey = KeyingMaterial.DefaultX509Key_2048_With_KeyId,
                    RepresentAsRsaKey = true,
                    JsonWebKey = KeyingMaterial.JsonWebKeyX509_2048_As_RSA_With_KeyId,
                    TestId = nameof(KeyingMaterial.DefaultX509Key_2048_With_KeyId) + nameof(JsonWebKeyConverterTheoryData.RepresentAsRsaKey)
                });

                theoryData.Add(new JsonWebKeyConverterTheoryData
                {
                    SecurityKey = KeyingMaterial.DefaultX509Key_2048,
                    JsonWebKey = KeyingMaterial.JsonWebKeyX509_2048,
                    TestId = nameof(KeyingMaterial.DefaultX509Key_2048)
                });

                theoryData.Add(new JsonWebKeyConverterTheoryData
                {
                    SecurityKey = KeyingMaterial.DefaultX509Key_2048,
                    RepresentAsRsaKey = true,
                    JsonWebKey = KeyingMaterial.JsonWebKeyX509_2048_As_RSA,
                    TestId = nameof(KeyingMaterial.DefaultX509Key_2048) + nameof(JsonWebKeyConverterTheoryData.RepresentAsRsaKey)
                });

                theoryData.Add(new JsonWebKeyConverterTheoryData
                {
                    SecurityKey = KeyingMaterial.DefaultX509Key_2048_Public,
                    JsonWebKey = KeyingMaterial.JsonWebKeyX509_2048_Public,
                    TestId = nameof(KeyingMaterial.DefaultX509Key_2048_Public)
                });

                theoryData.Add(new JsonWebKeyConverterTheoryData
                {
                    SecurityKey = KeyingMaterial.DefaultX509Key_2048_Public,
                    RepresentAsRsaKey = true,
                    JsonWebKey = KeyingMaterial.JsonWebKeyX509_2048_Public_As_RSA,
                    TestId = nameof(KeyingMaterial.DefaultX509Key_2048_Public) + nameof(JsonWebKeyConverterTheoryData.RepresentAsRsaKey)
                });

                return theoryData;
            }
        }
    }

    public class JsonWebKeyConverterTheoryData : TheoryDataBase
    {
        public SecurityKey SecurityKey
        {
            get;
            set;
        }
        public JsonWebKey JsonWebKey
        {
            get;
            set;
        }

        // related to ConvertX509SecurityKeyToJsonWebKeyTheoryData
        public bool RepresentAsRsaKey { get; set; } = false;
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
