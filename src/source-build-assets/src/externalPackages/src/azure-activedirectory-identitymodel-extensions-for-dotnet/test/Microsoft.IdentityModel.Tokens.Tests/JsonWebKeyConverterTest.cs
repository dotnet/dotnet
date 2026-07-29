// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class JsonWebKeyConverterTest
    {
        [Theory, MemberData(nameof(ConversionKeyTheoryData), DisableDiscoveryEnumeration = true)]
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
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(ConversionKeyTheoryData), DisableDiscoveryEnumeration = true)]
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

        [Theory, MemberData(nameof(ConvertX509SecurityKeyToJsonWebKeyTheoryData), DisableDiscoveryEnumeration = true)]
        public void ConvertX509SecurityKeyAsRsaSecurityKeyToJsonWebKey(JsonWebKeyConverterTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ConvertX509SecurityKeyToJsonWebKeyTheoryData", theoryData);
            try
            {
                var convertedKey = JsonWebKeyConverter.ConvertFromX509SecurityKey(theoryData.SecurityKey as X509SecurityKey, theoryData.RepresentAsRsaKey);

                theoryData.ExpectedException.ProcessNoException(context);
                IdentityComparer.AreEqual(convertedKey, theoryData.JsonWebKey, context);

                if (!theoryData.RepresentAsRsaKey && convertedKey.ConvertedSecurityKey.GetType() != typeof(X509SecurityKey))
                    context.AddDiff($"convertedKey.ConvertedSecurityKey.GetType() should be {typeof(X509SecurityKey)}.");
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
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
#if NET472 || NET
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
#if NET472 || NET
                theoryData.Add(new JsonWebKeyConverterTheoryData
                {
                    SecurityKey = KeyingMaterial.DefaultX509Key_256ECDSA,
                    JsonWebKey = KeyingMaterial.JsonWebKeyX509_256ECDSA,
                    TestId = nameof(KeyingMaterial.DefaultX509Key_256ECDSA)
                });

                theoryData.Add(new JsonWebKeyConverterTheoryData
                {
                    SecurityKey = KeyingMaterial.DefaultX509Key_256ECDSA_With_KeyId,
                    JsonWebKey = KeyingMaterial.JsonWebKeyX509_256ECDSA_With_KeyId,
                    TestId = nameof(KeyingMaterial.DefaultX509Key_256ECDSA_With_KeyId)
                });

                theoryData.Add(new JsonWebKeyConverterTheoryData
                {
                    SecurityKey = KeyingMaterial.DefaultX509Key_256ECDSA,
                    RepresentAsRsaKey = true,
                    JsonWebKey = KeyingMaterial.JsonWebKeyX509_256ECDSA_As_ECDSA,
                    TestId = nameof(KeyingMaterial.DefaultX509Key_256ECDSA_With_KeyId) + nameof(JsonWebKeyConverterTheoryData.RepresentAsRsaKey)
                });

                theoryData.Add(new JsonWebKeyConverterTheoryData
                {
                    SecurityKey = KeyingMaterial.DefaultX509Key_256ECDSA_With_KeyId,
                    RepresentAsRsaKey = true,
                    JsonWebKey = KeyingMaterial.JsonWebKeyX509_256ECDSA_As_ECDSA_With_KeyId,
                    TestId = nameof(KeyingMaterial.DefaultX509Key_256ECDSA) + nameof(JsonWebKeyConverterTheoryData.RepresentAsRsaKey)
                });
#endif
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
        public bool RepresentAsRsaKey { get; set; }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
