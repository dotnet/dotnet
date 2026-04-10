// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Json.Tests
{
    public class JsonWebKeySetTests
    {
        [Theory, MemberData(nameof(ConstructorDataSet))]
        public void Constructors(JsonWebKeySetTheoryData theoryData)
        {
            var context = new CompareContext(theoryData);
            context.PropertiesToIgnoreWhenComparing.Add(typeof(JsonWebKeySet), new List<string>() { "SkipUnresolvedJsonWebKeys" });

            try
            {
                var jsonWebKeys = new JsonWebKeySet(theoryData.Json);
                var keys = jsonWebKeys.GetSigningKeys();
                theoryData.ExpectedException.ProcessNoException(context);
                if (theoryData.JsonWebKeySet != null)
                    IdentityComparer.AreEqual(jsonWebKeys, theoryData.JsonWebKeySet, context);

                if (theoryData.ExpectedSigningKeys != null)
                    IdentityComparer.AreEqual(keys, theoryData.ExpectedSigningKeys, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JsonWebKeySetTheoryData> ConstructorDataSet
        {
            get
            {
                var theoryData = new TheoryData<JsonWebKeySetTheoryData>();

                theoryData.Add(
                    new JsonWebKeySetTheoryData("JsonWebKeySet1")
                    {
                        Json = DataSets.JsonWebKeySetString1,
                        JsonWebKeySet = DataSets.JsonWebKeySet1
                    });

                theoryData.Add(
                    new JsonWebKeySetTheoryData("Null")
                    {
                        ExpectedException = ExpectedException.ArgumentNullException()
                    });

                theoryData.Add(
                    new JsonWebKeySetTheoryData("JsonWebKeySetBadFormatingString")
                    {
                        ExpectedException = ExpectedException.ArgumentException(substringExpected: "IDX10805:", inner: typeof(System.Text.Json.JsonException)),
                        Json = DataSets.JsonWebKeySetBadFormatingString
                    });

                theoryData.Add(
                    new JsonWebKeySetTheoryData("JsonWebKeySetBadRsaExponentString")
                    {
                        Json = DataSets.JsonWebKeySetBadRsaExponentString,
                        ExpectedSigningKeys = new List<SecurityKey>()
                    });

                theoryData.Add(
                    new JsonWebKeySetTheoryData("JsonWebKeySetBadRsaModulusString")
                    {
                        Json = DataSets.JsonWebKeySetBadRsaModulusString,
                        ExpectedSigningKeys = new List<SecurityKey>()
                    });

                theoryData.Add(
                    new JsonWebKeySetTheoryData("JsonWebKeySetKtyNotRsaString")
                    {
                        Json = DataSets.JsonWebKeySetKtyNotRsaString,
                        ExpectedSigningKeys = new List<SecurityKey>()
                    });

                theoryData.Add(
                    new JsonWebKeySetTheoryData("JsonWebKeySetUseNotSigString")
                    {
                        Json = DataSets.JsonWebKeySetUseNotSigString,
                        ExpectedSigningKeys = new List<SecurityKey>()
                    });

                List<SecurityKey> keys = new List<SecurityKey>();
                if (JsonWebKeyConverter.TryCreateToRsaSecurityKey(DataSets.JsonWebKeyBadX509Data, out SecurityKey securityKey))
                    keys.Add(securityKey);

                theoryData.Add(
                    new JsonWebKeySetTheoryData("JsonWebKeySetBadX509String")
                    {
                        Json = DataSets.JsonWebKeySetBadX509String,
                        ExpectedSigningKeys = keys
                    });

                theoryData.Add(
                    new JsonWebKeySetTheoryData("JsonWebKeySetECCString")
                    {
                        Json = DataSets.JsonWebKeySetECCString,
                        JsonWebKeySet = DataSets.JsonWebKeySetEC
                    });

                theoryData.Add(
                    new JsonWebKeySetTheoryData("JsonWebKeySetBadECCurveString")
                    {
                        Json = DataSets.JsonWebKeySetBadECCurveString,
                        ExpectedSigningKeys = new List<SecurityKey>()
                    });

                theoryData.Add(
                    new JsonWebKeySetTheoryData("JsonWebKeySetOnlyX5tString")
                    {
                        Json = DataSets.JsonWebKeySetOnlyX5tString,
                        JsonWebKeySet = DataSets.JsonWebKeySetOnlyX5t
                    });

                theoryData.Add(
                    new JsonWebKeySetTheoryData("JsonWebKeySetX509DataString")
                    {
                        Json = DataSets.JsonWebKeySetX509DataString,
                        JsonWebKeySet = DataSets.JsonWebKeySetX509Data
                    });

                return theoryData;
            }
        }

        [Fact]
        public void Defaults()
        {
            var context = new CompareContext();
            JsonWebKeySet jsonWebKeys = new JsonWebKeySet();

            if (jsonWebKeys.Keys == null)
                context.Diffs.Add("jsonWebKeys.Keys == null");
            else if (jsonWebKeys.Keys.Count != 0)
                context.Diffs.Add("jsonWebKeys.Keys.Count != 0");

            if (jsonWebKeys.AdditionalData == null)
                context.Diffs.Add("jsonWebKeys.AdditionalData == null");
            else if (jsonWebKeys.AdditionalData.Count != 0)
                context.Diffs.Add("jsonWebKeys.AdditionalData.Count != 0");

            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void SigningKeysExtensibility()
        {
            var context = new CompareContext($"{this}.SigningKeysExtensibility");
            TestUtilities.WriteHeader($"{this}.SigningKeysExtensibility");

            try
            {
                // Received json web key only has an x5t property and it can't be used for signature validation as it can't be resolved into a SecurityKey, without user's custom code.
                // This test proves that the scenario described above is possible using extensibility.
                JsonWebKeySet.DefaultSkipUnresolvedJsonWebKeys = false;
                var signingKeys = new JsonWebKeySet(DataSets.JsonWebKeySetOnlyX5tString).GetSigningKeys();

                var tokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKeys = signingKeys,
                    IssuerSigningKeyResolver = (token, securityToken, keyIdentifier, tvp) => { return new List<SecurityKey> { ResolveX509Certificate(token, securityToken, keyIdentifier, tvp) }; },
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                };

                var tokenValidationResult = new JsonWebTokens.JsonWebTokenHandler().ValidateTokenAsync(Default.AsymmetricJwt, tokenValidationParameters).Result;

                if (tokenValidationResult.IsValid != true)
                    context.Diffs.Add("tokenValidationResult.IsValid != true");
            }
            catch (Exception ex)
            {
                context.Diffs.Add($"TokenValidationFailed: {ex}");
            }

            finally
            {
                // revert back to default
                JsonWebKeySet.DefaultSkipUnresolvedJsonWebKeys = true;
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        private SecurityKey ResolveX509Certificate(string token, SecurityToken securityToken, string keyIdentifier, TokenValidationParameters tvp)
        {
            // example: get a certificate from a cert store

            if (tvp.IssuerSigningKeys.First() is JsonWebKey jsonWebKey)
            {
                if (!string.IsNullOrEmpty(jsonWebKey.X5t))
                {
                    // X509Store store = new X509Store(StoreLocation.CurrentUser);
                    // store.Open(OpenFlags.ReadOnly);
                    // X509Certificate2Collection certs = store.Certificates.Find(X509FindType.FindByThumbprint, Base64UrlEncoder.Decode(jsonWebKey.X5t), true);
                    // return new X509SecurityKey(certs[0]);

                    return new X509SecurityKey(KeyingMaterial.DefaultCert_2048);
                }
            }

            return null;
        }

        [Theory, MemberData(nameof(GetSigningKeysTheoryData))]
        public void GetSigningKeys(JsonWebKeySetTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.GetSigningKeys", theoryData);
            try
            {
                var signingKeys = theoryData.JsonWebKeySet.GetSigningKeys();

                IdentityComparer.AreEqual(signingKeys, theoryData.ExpectedSigningKeys, context);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JsonWebKeySetTheoryData> GetSigningKeysTheoryData
        {
            get
            {
                var ecdsaAdapter = ECDsaAdapter.Instance;
                var theoryData = new TheoryData<JsonWebKeySetTheoryData>();

                var jsonWebKeySet = new JsonWebKeySet(DataSets.JsonWebKeySetUseNotSigString) { SkipUnresolvedJsonWebKeys = true };
                theoryData.Add(new JsonWebKeySetTheoryData
                {
                    First = true,
                    JsonWebKeySet = jsonWebKeySet,
                    ExpectedSigningKeys = new List<SecurityKey>(),
                    TestId = "ZeroKeysWithSigAsUseSkipUnresolved",
                });

                jsonWebKeySet = new JsonWebKeySet(DataSets.JsonWebKeySetUseNotSigString) { SkipUnresolvedJsonWebKeys = false };
                theoryData.Add(new JsonWebKeySetTheoryData
                {
                    JsonWebKeySet = jsonWebKeySet,
                    ExpectedSigningKeys = new List<SecurityKey>() { (jsonWebKeySet.Keys as List<JsonWebKey>)[0] },
                    TestId = "ZeroKeysWithSigAsUse",
                });

                jsonWebKeySet = new JsonWebKeySet(DataSets.JsonWebKeySetUseNoKtyString) { SkipUnresolvedJsonWebKeys = true };
                theoryData.Add(new JsonWebKeySetTheoryData
                {
                    JsonWebKeySet = jsonWebKeySet,
                    ExpectedSigningKeys = new List<SecurityKey>(),
                    TestId = "KeysWithoutKtySkipUnresolved",
                });

                jsonWebKeySet = new JsonWebKeySet(DataSets.JsonWebKeySetUseNoKtyString) { SkipUnresolvedJsonWebKeys = false };
                theoryData.Add(new JsonWebKeySetTheoryData
                {
                    JsonWebKeySet = jsonWebKeySet,
                    ExpectedSigningKeys = new List<SecurityKey>() { (jsonWebKeySet.Keys as List<JsonWebKey>)[0] },
                    TestId = "KeysWithoutKty",
                });

                jsonWebKeySet = new JsonWebKeySet(DataSets.JsonWebKeySetEvoString) { SkipUnresolvedJsonWebKeys = false };
                theoryData.Add(new JsonWebKeySetTheoryData
                {
                    JsonWebKeySet = jsonWebKeySet,
                    ExpectedSigningKeys = new List<SecurityKey>() { CreateRsaSecurityKey(jsonWebKeySet, 0), CreateX509SecurityKey(jsonWebKeySet, 0) },
                    TestId = "EvoSigningKey",
                });

                jsonWebKeySet = new JsonWebKeySet(DataSets.JsonWebKeySetKtyNotRsaString) { SkipUnresolvedJsonWebKeys = true };
                theoryData.Add(new JsonWebKeySetTheoryData
                {
                    JsonWebKeySet = jsonWebKeySet,
                    ExpectedSigningKeys = new List<SecurityKey>(),
                    TestId = "NonRsaNonEcKeySkipUnresolved",
                });

                jsonWebKeySet = new JsonWebKeySet(DataSets.JsonWebKeySetKtyNotRsaString) { SkipUnresolvedJsonWebKeys = false };
                theoryData.Add(new JsonWebKeySetTheoryData
                {
                    JsonWebKeySet = jsonWebKeySet,
                    ExpectedSigningKeys = new List<SecurityKey>() { (jsonWebKeySet.Keys as List<JsonWebKey>)[0] },
                    TestId = "NonRsaNonEcKey",
                });

                jsonWebKeySet = new JsonWebKeySet(DataSets.JsonWebKeySetOnlyX5tString) { SkipUnresolvedJsonWebKeys = true };
                theoryData.Add(new JsonWebKeySetTheoryData
                {
                    JsonWebKeySet = jsonWebKeySet,
                    ExpectedSigningKeys = new List<SecurityKey>(),
                    TestId = "JsonWebKeyNotInvalidNotResolvedSkipUnresolved"
                });

                jsonWebKeySet = new JsonWebKeySet(DataSets.JsonWebKeySetOnlyX5tString) { SkipUnresolvedJsonWebKeys = false };
                theoryData.Add(new JsonWebKeySetTheoryData
                {
                    JsonWebKeySet = jsonWebKeySet,
                    ExpectedSigningKeys = new List<SecurityKey>() { (jsonWebKeySet.Keys as List<JsonWebKey>)[0] },
                    TestId = "JsonWebKeyNotInvalidNotResolved"
                });

                jsonWebKeySet = new JsonWebKeySet(DataSets.JsonWebKeySetOneValidRsaOneInvalidRsaString) { SkipUnresolvedJsonWebKeys = true };
                theoryData.Add(new JsonWebKeySetTheoryData
                {
                    JsonWebKeySet = jsonWebKeySet,
                    ExpectedSigningKeys = new List<SecurityKey>() { CreateRsaSecurityKey(jsonWebKeySet, 0) },
                    TestId = "OneValidAndOneInvalidRsaSkipUnresolved",
                });

                jsonWebKeySet = new JsonWebKeySet(DataSets.JsonWebKeySetOneValidRsaOneInvalidRsaString) { SkipUnresolvedJsonWebKeys = false };
                theoryData.Add(new JsonWebKeySetTheoryData
                {
                    JsonWebKeySet = jsonWebKeySet,
                    ExpectedSigningKeys = new List<SecurityKey>() { (jsonWebKeySet.Keys as List<JsonWebKey>)[0], (jsonWebKeySet.Keys as List<JsonWebKey>)[1] },
                    TestId = "OneValidAndOneInvalidRsa",
                });

                jsonWebKeySet = new JsonWebKeySet(DataSets.JsonWebKeySetOneInvalidEcOneValidEcString) { SkipUnresolvedJsonWebKeys = true };
                theoryData.Add(new JsonWebKeySetTheoryData
                {
                    JsonWebKeySet = jsonWebKeySet,
                    ExpectedSigningKeys = new List<SecurityKey>() { CreateEcdsaSecurityKey(jsonWebKeySet, 1, ecdsaAdapter) },
                    TestId = "OneValidAndOneInvalidEcSkipUnresolved",
                });

                jsonWebKeySet = new JsonWebKeySet(DataSets.JsonWebKeySetOneInvalidEcOneValidEcString) { SkipUnresolvedJsonWebKeys = false };
                theoryData.Add(new JsonWebKeySetTheoryData
                {
                    JsonWebKeySet = jsonWebKeySet,
                    ExpectedSigningKeys = new List<SecurityKey>() { (jsonWebKeySet.Keys as List<JsonWebKey>)[0], CreateEcdsaSecurityKey(jsonWebKeySet, 1, ecdsaAdapter) },
                    TestId = "OneValidAndOneInvalidEC",
                });

                jsonWebKeySet = new JsonWebKeySet(DataSets.JsonWebKeySetOneValidRsaOneInvalidEcString) { SkipUnresolvedJsonWebKeys = true };
                theoryData.Add(new JsonWebKeySetTheoryData
                {
                    JsonWebKeySet = jsonWebKeySet,
                    ExpectedSigningKeys = new List<SecurityKey>() { CreateRsaSecurityKey(jsonWebKeySet, 0) },
                    TestId = "ValidRsaInvalidEcSkipUnresolved",
                });

                jsonWebKeySet = new JsonWebKeySet(DataSets.JsonWebKeySetOneValidRsaOneInvalidEcString) { SkipUnresolvedJsonWebKeys = false };
                theoryData.Add(new JsonWebKeySetTheoryData
                {
                    JsonWebKeySet = jsonWebKeySet,
                    ExpectedSigningKeys = new List<SecurityKey>() { CreateRsaSecurityKey(jsonWebKeySet, 0), (jsonWebKeySet.Keys as List<JsonWebKey>)[1] },
                    TestId = "ValidRsaInvalidEc",
                });

                jsonWebKeySet = new JsonWebKeySet(DataSets.JsonWebKeySetX509DataString) { SkipUnresolvedJsonWebKeys = false };
                theoryData.Add(new JsonWebKeySetTheoryData
                {
                    JsonWebKeySet = jsonWebKeySet,
                    ExpectedSigningKeys = new List<SecurityKey>() { CreateX509SecurityKey(jsonWebKeySet, 0) },
                    TestId = "ValidX5c",
                });

                jsonWebKeySet = new JsonWebKeySet(DataSets.JsonWebKeySetBadX509String) { SkipUnresolvedJsonWebKeys = true };
                theoryData.Add(new JsonWebKeySetTheoryData
                {
                    JsonWebKeySet = jsonWebKeySet,
                    ExpectedSigningKeys = new List<SecurityKey>() { CreateRsaSecurityKey(jsonWebKeySet, 0) },
                    TestId = "InvalidX5cSkipUnresolvedAddRsa",
                });

                jsonWebKeySet = new JsonWebKeySet(DataSets.JsonWebKeySetBadX509String) { SkipUnresolvedJsonWebKeys = false };
                theoryData.Add(new JsonWebKeySetTheoryData
                {
                    JsonWebKeySet = jsonWebKeySet,
                    ExpectedSigningKeys = new List<SecurityKey>() { CreateRsaSecurityKey(jsonWebKeySet, 0), (jsonWebKeySet.Keys as List<JsonWebKey>)[0] },
                    TestId = "InvalidX5c",
                });

                jsonWebKeySet = new JsonWebKeySet(DataSets.JsonWebKeySetOneInvalidEcOneValidEcString) { SkipUnresolvedJsonWebKeys = true };
                theoryData.Add(new JsonWebKeySetTheoryData
                {
                    JsonWebKeySet = jsonWebKeySet,
                    ExpectedSigningKeys = new List<SecurityKey>() { (jsonWebKeySet.Keys as List<JsonWebKey>)[1] },
                    TestId = "ECDsaAdapterIsNotSupportedSkipUnresolved",
                });

                jsonWebKeySet = new JsonWebKeySet(DataSets.JsonWebKeySetOneInvalidEcOneValidEcString) { SkipUnresolvedJsonWebKeys = false };
                theoryData.Add(new JsonWebKeySetTheoryData
                {
                    JsonWebKeySet = jsonWebKeySet,
                    ExpectedSigningKeys = new List<SecurityKey>() { (jsonWebKeySet.Keys as List<JsonWebKey>)[0], (jsonWebKeySet.Keys as List<JsonWebKey>)[1] },
                    TestId = "ECDsaAdapterIsNotSupported",
                });

                return theoryData;
            }
        }

#if NET8_0_OR_GREATER
        [Fact]
        public void JsonDeserialize()
        {
            var json = DataSets.JsonWebKeySetEvoString;
            var jsonWebKeys = System.Text.Json.JsonSerializer.Deserialize<JsonWebKeySet>(json);
            Assert.NotNull(jsonWebKeys);
            Assert.NotEmpty(jsonWebKeys.Keys);
        }
#endif

        private static X509SecurityKey CreateX509SecurityKey(JsonWebKeySet webKeySet, int keyIndex)
        {
            var webKey = (webKeySet.Keys as List<JsonWebKey>)[keyIndex];

            return new X509SecurityKey(new X509Certificate2(Convert.FromBase64String(webKey.X5c[0])))
            {
                KeyId = webKey.KeyId
            };
        }

        private static RsaSecurityKey CreateRsaSecurityKey(JsonWebKeySet webKeySet, int keyIndex)
        {
            var webKey = (webKeySet.Keys as List<JsonWebKey>)[keyIndex];

            var rsaParams = new RSAParameters()
            {
                Exponent = Base64UrlEncoder.DecodeBytes(webKey.E),
                Modulus = Base64UrlEncoder.DecodeBytes(webKey.N),
            };

            return new RsaSecurityKey(rsaParams)
            {
                KeyId = webKey.KeyId,
            };
        }

        private static ECDsaSecurityKey CreateEcdsaSecurityKey(JsonWebKeySet webKeySet, int keyIndex, ECDsaAdapter ecdsaAdapter)
        {
            var webKey = (webKeySet.Keys as List<JsonWebKey>)[keyIndex];

            return new ECDsaSecurityKey(ecdsaAdapter.CreateECDsa(webKey, false))
            {
                KeyId = webKey.KeyId
            };
        }
    }
}
