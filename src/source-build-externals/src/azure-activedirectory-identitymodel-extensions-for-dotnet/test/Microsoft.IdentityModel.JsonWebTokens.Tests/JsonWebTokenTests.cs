// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens.Jwt.Tests;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

using JsonReaderException = System.Text.Json.JsonException;

namespace Microsoft.IdentityModel.JsonWebTokens.Tests
{
    public class JsonWebTokenTests
    {
        private static DateTime dateTime = new DateTime(2000, 01, 01, 0, 0, 0);
        private static DateTime dateTimeUtc = new DateTime(2000, 01, 01, 0, 0, 0).ToUniversalTime();
        private string jsonString = $@"{{""intarray"":[1,2,3], ""array"":[1,""2"",3], ""jobject"": {{""string1"":""string1value"",""string2"":""string2value""}},""string"":""bob"", ""float"":42, ""integer"":42, ""nill"": null, ""bool"" : true, ""dateTime"": ""{dateTime}"", ""dateTimeIso8061"": ""{dateTime.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture)}"" }}";
        // Note: We need to do some work with doubles and floats.
        // If we serialize 42.0 as a double, then when deserialized, reading as Utf8JsonReader.GetDouble() will return 42.
        // While we figure this out, the ClaimValueType for float was set to Integer32.
        private List<Claim> payloadClaims = new List<Claim>()
        {
            new Claim("intarray", @"1", "http://www.w3.org/2001/XMLSchema#integer32", "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
            new Claim("array", @"1", "http://www.w3.org/2001/XMLSchema#integer32", "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
            new Claim("jobject", @"{""string1"":""string1value"",""string2"":""string2value""}", JsonClaimValueTypes.Json, "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
            new Claim("string", "bob", ClaimValueTypes.String, "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
            new Claim("float", "42", ClaimValueTypes.Integer32, "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
            new Claim("integer", "42", ClaimValueTypes.Integer32, "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
            new Claim("nill", "", JsonClaimValueTypes.JsonNull, "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
            new Claim("bool", "True", ClaimValueTypes.Boolean, "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
            new Claim("dateTime", dateTime.ToString(), ClaimValueTypes.String, "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
            new Claim("dateTimeIso8061", dateTime.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture), ClaimValueTypes.DateTime, "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
        };

        [Fact]
        public void DateTime2038Issue()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(new string('a', 128)));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "Bob") };
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = creds,
                Subject = new ClaimsIdentity(claims),
                Expires = (new DateTime(2038, 1, 20)).ToUniversalTime(),
            };

            JsonWebTokenHandler handler = new();
            string jwt = handler.CreateToken(tokenDescriptor);
            JsonWebToken jsonWebToken = new JsonWebToken(jwt);

            Assert.Equal(jsonWebToken.ValidTo, (new DateTime(2038, 1, 20)).ToUniversalTime());
        }

        // This test is designed to test that all properties of a JWE can be accessed.
        // Some properties rely on an inner token and the Payload can be null.
        [Fact]
        public void JWETouchAllProperties()
        {
            var context = new CompareContext();
            var jsonWebTokenHandler = new JsonWebTokenHandler();
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes256_Sha512_512,
                Subject = new ClaimsIdentity(Default.PayloadClaims),
                TokenType = "TokenType"
            };

            string jwe = jsonWebTokenHandler.CreateToken(tokenDescriptor);
            JsonWebToken jsonWebToken = new JsonWebToken(jwe);
            JsonWebToken jsonWebToken2 = new JsonWebToken(jwe);

            IdentityComparer.AreEqual(jsonWebToken, jsonWebToken2, context);

            TestUtilities.AssertFailIfErrors(context);
        }

        // Test checks to make sure that the JsonWebToken.GetClaim() method is able to retrieve every Claim returned by the Claims property (with the exception 
        // of Claims that are JObjects or arrays, as those are converted to strings by the GetClaim() method).
        [Fact]
        public void CompareGetClaimAndClaims()
        {
            var context = new CompareContext();
            var jsonWebTokenHandler = new JsonWebTokenHandler();
            var jsonWebTokenString = jsonWebTokenHandler.CreateToken(Default.PayloadString, KeyingMaterial.JsonWebKeyRsa256SigningCredentials);
            var jsonWebToken = new JsonWebToken(jsonWebTokenString);
            var claims = jsonWebToken.Claims;

            foreach (var claim in claims)
            {
                var claimFromGetClaim = jsonWebToken.GetClaim(claim.Type);
                IdentityComparer.AreEqual(claim, claimFromGetClaim, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        // Test checks to make sure that GetPayloadValue<Claim>() returns the same vaue as GetClaim() for every { key, 'value' } pair in the payload.
        [Fact]
        public void CompareGetClaimAndGetPayloadValue()
        {
            var context = new CompareContext();
            var jsonWebTokenHandler = new JsonWebTokenHandler();
            var jsonWebTokenString = jsonWebTokenHandler.CreateToken(Default.PayloadString, KeyingMaterial.JsonWebKeyRsa256SigningCredentials);
            var jsonWebToken = new JsonWebToken(jsonWebTokenString);
            var claims = jsonWebToken.Claims;

            foreach (var claim in claims)
            {
                var claimFromGetClaim = jsonWebToken.GetClaim(claim.Type);
                var claimFromGetPayloadValue = jsonWebToken.GetPayloadValue<Claim>(claim.Type);
                IdentityComparer.AreEqual(claimFromGetClaim, claimFromGetPayloadValue, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        // Test checks to make sure that TryGetPayloadValue<Claim>() returns the same vaue as TryGetClaim() for every { key, 'value' } pair in the payload.
        [Fact]
        public void CompareTryGetClaimAndTryGetPayloadValue()
        {
            var context = new CompareContext();
            var jsonWebTokenHandler = new JsonWebTokenHandler();
            var jsonWebTokenString = jsonWebTokenHandler.CreateToken(Default.PayloadString, KeyingMaterial.JsonWebKeyRsa256SigningCredentials);
            var jsonWebToken = new JsonWebToken(jsonWebTokenString);
            var claims = jsonWebToken.Claims;

            var tryGetClaimSucceeded = jsonWebToken.TryGetPayloadValue<Claim>("doesnotexist", out var claimFromTryGetPayloadValue);
            var tryGetPayloadSucceeded = jsonWebToken.TryGetClaim("doesnotexist", out var claimFromTryGetClaim);
            IdentityComparer.AreEqual(tryGetClaimSucceeded, tryGetPayloadSucceeded, context);
            IdentityComparer.AreEqual(claimFromTryGetClaim, claimFromTryGetPayloadValue, context);

            foreach (var claim in claims)
            {
                tryGetClaimSucceeded = jsonWebToken.TryGetPayloadValue(claim.Type, out claimFromTryGetPayloadValue);
                tryGetPayloadSucceeded = jsonWebToken.TryGetClaim(claim.Type, out claimFromTryGetClaim);
                IdentityComparer.AreEqual(tryGetClaimSucceeded, tryGetPayloadSucceeded, context);
                IdentityComparer.AreEqual(claimFromTryGetClaim, claimFromTryGetPayloadValue, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        // Test checks to make sure that the Claim values returned by GetClaim() are what we expect.
        // This includes JArrays and JObjects, which are converted to strings.
        [Fact]
        public void GetClaim()
        {
            var context = new CompareContext();
            var jsonWebToken = new JsonWebToken("{}", jsonString);

            foreach (var claim in payloadClaims)
            {
                var claimToCompare = jsonWebToken.GetClaim(claim.Type);
                if (!IdentityComparer.AreEqual(claim, claimToCompare, context))
                {
                    context.AddDiff($"claim.Type: '{claim.Type}'");
                }
            }

            try // Try to retrieve a value that doesn't exist in the payload.
            {
                jsonWebToken.GetClaim("doesnotexist");
            }
            catch (Exception ex)
            {
                ExpectedException.ArgumentException("IDX14304:").ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        // Test checks to make sure that the Claim values returned by TryGetClaim() are what we expect.
        // This includes JArrays and JObjects, which are converted to strings.
        [Fact]
        public void TryGetClaim()
        {
            var context = new CompareContext();
            var jsonWebToken = new JsonWebToken("{}", jsonString);

            // Tries to retrieve a value that does not exist in the payload.
            var success = jsonWebToken.TryGetClaim("doesnotexist", out Claim doesNotExist);
            IdentityComparer.AreEqual(null, doesNotExist, context);
            IdentityComparer.AreEqual(false, success, context);

            foreach (var claim in payloadClaims)
            {
                success = jsonWebToken.TryGetClaim(claim.Type, out var claimToCompare);
                if (!IdentityComparer.AreEqual(claim, claimToCompare, context))
                {
                    context.AddDiff($"claim.Type: '{claim.Type}'");
                }

                IdentityComparer.AreEqual(true, success, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        // Test checks to make sure that the JsonWebToken payload is correctly converted to IEnumerable<Claim>.
        [Fact]
        public void GetClaimsFromJObject()
        {
            var context = new CompareContext();
            var jsonWebTokenHandler = new JsonWebTokenHandler();
            var jsonWebTokenString = jsonWebTokenHandler.CreateToken(Default.PayloadString, KeyingMaterial.JsonWebKeyRsa256SigningCredentials);
            var jsonWebToken = new JsonWebToken(jsonWebTokenString);
            var claims = jsonWebToken.Claims;
            IdentityComparer.AreEqual(Default.PayloadClaims, claims, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        // Test checks to make sure that the 'Audiences' claim can be successfully retrieved when multiple audiences are present.
        // It also checks that the rest of the claims match up as well
        [Fact]
        public void CompareJwtSecurityTokenWithJsonSecurityTokenMultipleAudiences()
        {
            var context = new CompareContext();
            string payload = @"{""aud"":[""http://Default.Audience.com"", ""http://Default.Audience1.com"", ""http://Default.Audience2.com"", ""http://Default.Audience3.com"", ""http://Default.Audience4.com""]}";
            string header = "{}";
            var jsonWebToken = new JsonWebToken(header, payload);
            var jwtSecurityToken = new JwtSecurityToken($"{Base64UrlEncoder.Encode(header)}.{Base64UrlEncoder.Encode(payload)}.");
            IdentityComparer.AreEqual(jsonWebToken.Claims, jwtSecurityToken.Claims);
            IdentityComparer.AreEqual(jsonWebToken.Audiences, jwtSecurityToken.Audiences, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        // Test checks to make sure that claim values of various types can be successfully retrieved from the header.
        [Fact]
        public void TryGetHeaderValues()
        {
            var context = new CompareContext();
            TestUtilities.WriteHeader($"{this}.TryGetHeaderValues");

            var token = new JsonWebToken(jsonString, "{}");

            var success = token.TryGetHeaderValue("intarray", out int[] intarray);
            IdentityComparer.AreEqual(new int[] { 1, 2, 3 }, intarray, context);
            IdentityComparer.AreEqual(true, success, context);

            success = token.TryGetHeaderValue("array", out object[] array);
            IdentityComparer.AreEqual(new object[] { 1, "2", 3 }, array, context);
            IdentityComparer.AreEqual(true, success, context);

            success = token.TryGetHeaderValue("string", out string name);
            IdentityComparer.AreEqual("bob", name, context);
            IdentityComparer.AreEqual(true, success, context);

            success = token.TryGetHeaderValue("float", out float floatingPoint);
            IdentityComparer.AreEqual((float)42, floatingPoint, context);
            IdentityComparer.AreEqual(true, success, context);

            success = token.TryGetHeaderValue("integer", out int integer);
            IdentityComparer.AreEqual(42, integer, context);
            IdentityComparer.AreEqual(true, success, context);

            success = token.TryGetHeaderValue("nill", out object nill);
            IdentityComparer.AreEqual(nill, null, context);
            IdentityComparer.AreEqual(true, success, context);

            success = token.TryGetHeaderValue("bool", out bool boolean);
            IdentityComparer.AreEqual(boolean, true, context);
            IdentityComparer.AreEqual(true, success, context);

            success = token.TryGetHeaderValue("doesnotexist", out int doesNotExist);
            IdentityComparer.AreEqual(0, doesNotExist, context);
            IdentityComparer.AreEqual(false, success, context);

            success = token.TryGetHeaderValue("string", out int cannotConvert);
            IdentityComparer.AreEqual(0, cannotConvert, context);
            IdentityComparer.AreEqual(false, success, context);

            TestUtilities.AssertFailIfErrors(context);
        }

        // Test checks to make sure that claim values of various types can be successfully retrieved from the payload.
        [Fact]
        public void GetPayloadValues()
        {
            var context = new CompareContext();
            TestUtilities.WriteHeader($"{this}.GetPayloadValues");

            var token = new JsonWebToken("{}", jsonString);

            try // Try to retrieve a value that doesn't exist in the header.
            {
                token.GetPayloadValue<int>("doesnotexist");
            }
            catch (Exception ex)
            {
                ExpectedException.ArgumentException("IDX14304:").ProcessException(ex, context);
            }

            try // Try to retrieve an integer when the value is actually a string.
            {
                token.GetPayloadValue<int>("string");
            }
            catch (Exception ex)
            {
                ExpectedException.ArgumentException("IDX14305:").ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        // Test checks to make sure that claim values of various types can be successfully retrieved from the payload.
        [Fact]
        public void TryGetPayloadValues()
        {
            var context = new CompareContext();
            TestUtilities.WriteHeader($"{this}.TryGetPayloadValues");

            var token = new JsonWebToken("{}", jsonString);

            var success = token.TryGetPayloadValue("intarray", out int[] intarray);
            IdentityComparer.AreEqual(new int[] { 1, 2, 3 }, intarray, context);
            IdentityComparer.AreEqual(true, success, context);

            success = token.TryGetPayloadValue("array", out object[] array);
            IdentityComparer.AreEqual(new object[] { 1, "2", 3 }, array, context);
            IdentityComparer.AreEqual(true, success, context);

            success = token.TryGetPayloadValue("string", out string name);
            IdentityComparer.AreEqual("bob", name, context);
            IdentityComparer.AreEqual(true, success, context);

            success = token.TryGetPayloadValue("float", out float floatingPoint);
            IdentityComparer.AreEqual((float)42, floatingPoint, context);
            IdentityComparer.AreEqual(true, success, context);

            success = token.TryGetPayloadValue("integer", out int integer);
            IdentityComparer.AreEqual(42, integer, context);
            IdentityComparer.AreEqual(true, success, context);

            success = token.TryGetPayloadValue("nill", out object nill);
            IdentityComparer.AreEqual(nill, null, context);
            IdentityComparer.AreEqual(true, success, context);

            success = token.TryGetPayloadValue("bool", out bool boolean);
            IdentityComparer.AreEqual(boolean, true, context);
            IdentityComparer.AreEqual(true, success, context);

            var dateTimeValue = token.GetPayloadValue<string>("dateTime");
            IdentityComparer.AreEqual(dateTimeValue, dateTime.ToString(), context);
            IdentityComparer.AreEqual(true, success, context);

            var dateTimeIso8061Value = token.GetPayloadValue<DateTime>("dateTimeIso8061");
            IdentityComparer.AreEqual(dateTimeIso8061Value, dateTimeUtc, context);
            IdentityComparer.AreEqual(true, success, context);

            success = token.TryGetPayloadValue("doesnotexist", out int doesNotExist);
            IdentityComparer.AreEqual(0, doesNotExist, context);
            IdentityComparer.AreEqual(false, success, context);

            success = token.TryGetPayloadValue("string", out int cannotConvert);
            IdentityComparer.AreEqual(0, cannotConvert, context);
            IdentityComparer.AreEqual(false, success, context);

            TestUtilities.AssertFailIfErrors(context);
        }

        // Time values can be floats, ints, or strings.
        // This test checks to make sure that parsing does not fault in any of the above cases.
        [Theory, MemberData(nameof(GetPayloadValueTheoryData))]
        public void GetPayloadValue(GetPayloadValueTheoryData theoryData)
        {
            CompareContext context = TestUtilities.WriteHeader($"{this}.GetPayloadValue", theoryData);
            try
            {
                JsonWebTokenHandler jsonWebTokenHandler = new JsonWebTokenHandler { SetDefaultTimesOnTokenCreation = false };
                string jwt = jsonWebTokenHandler.CreateToken(theoryData.SecurityTokenDescriptor);
                JsonWebToken jsonWebToken = new JsonWebToken(jwt);
                string payload = Base64UrlEncoder.Decode(jsonWebToken.EncodedPayload);

                var methods = typeof(JsonWebToken).GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                var method = typeof(JsonWebToken).GetMethod("GetPayloadValue", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, CallingConventions.Standard, new Type[] { typeof(string) }, null);
                var retval = method.MakeGenericMethod(theoryData.PropertyType).Invoke(jsonWebToken, new object[] { theoryData.PropertyName });
                theoryData.ExpectedException.ProcessNoException(context);
                IdentityComparer.AreEqual(retval, theoryData.PropertyValue, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex.InnerException, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<GetPayloadValueTheoryData> GetPayloadValueTheoryData
        {
            get
            {
                var theoryData = new TheoryData<GetPayloadValueTheoryData>();

                List<string> listStrings = new List<string> { "list1", "list1" };
                List<object> listObjects = new List<object> { "list1", "list1" };
                Collection<string> collectionStrings = new Collection<string> { "collections1", "collections2" };
                Collection<object> collectionObjects = new Collection<object> { "collections1", "collections2" };
                string[] arrayStrings = new string[] { "array1", "arrray2" };
                object[] arrayObjects = new object[] { "array1", "arrray2" };
                int[] arrayInts = new int[] { 1,2,3 };
                object[] arrayMixed = new object[] { 1, "2", 3 };

                theoryData.Add(new GetPayloadValueTheoryData("DictionaryWithArrayOfStrings")
                {
                    PropertyName = "a",
                    PropertyType = typeof(Dictionary<string, string[]>),
                    PropertyValue = new Dictionary<string, string[]>
                    {
                        ["prop1"] = arrayStrings
                    },
                    SecurityTokenDescriptor = new SecurityTokenDescriptor
                    {
                        Claims = new Dictionary<string, object>
                        {
                            ["a"] = new Dictionary<string, string[]>
                            {
                                ["prop1"] = arrayStrings
                            }
                        }
                    }
                });

                theoryData.Add(new GetPayloadValueTheoryData("DictionaryWithListOfStrings")
                {
                    PropertyName = "a",
                    PropertyType = typeof(Dictionary<string, List<string>>),
                    PropertyValue = new Dictionary<string, List<string>>
                    {
                        ["prop1"] = listStrings
                    },
                    SecurityTokenDescriptor = new SecurityTokenDescriptor
                    {
                        Claims = new Dictionary<string, object>
                        {
                            ["a"] = new Dictionary<string, List<string>>
                            {
                                ["prop1"] = listStrings
                            }
                        }
                    }
                });

                theoryData.Add(new GetPayloadValueTheoryData("DictionaryWithCollectionOfStrings")
                {
                    PropertyName = "a",
                    PropertyType = typeof(Dictionary<string, Collection<string>>),
                    PropertyValue = new Dictionary<string, Collection<string>>
                    {
                        ["prop1"] = collectionStrings
                    },
                    SecurityTokenDescriptor = new SecurityTokenDescriptor
                    {
                        Claims = new Dictionary<string, object>
                        {
                            ["a"] = new Dictionary<string, Collection<string>>
                            {
                                ["prop1"] = collectionStrings
                            }
                        }
                    }
                });

                theoryData.Add(new GetPayloadValueTheoryData("ArrayOfStrings")
                {
                    PropertyName = "c",
                    PropertyType = typeof(string[]),
                    PropertyValue = arrayStrings,
                    SecurityTokenDescriptor = new SecurityTokenDescriptor
                    {
                        Claims = new Dictionary<string, object>
                        {
                            ["c"] = arrayStrings
                        }
                    }
                });

                theoryData.Add(new GetPayloadValueTheoryData("ArrayOfObjects")
                {
                    PropertyName = "c",
                    PropertyType = typeof(object[]),
                    PropertyValue = arrayObjects,
                    SecurityTokenDescriptor = new SecurityTokenDescriptor
                    {
                        Claims = new Dictionary<string, object>
                        {
                            ["c"] = arrayObjects
                        }
                    }
                });

                theoryData.Add(new GetPayloadValueTheoryData("ListOfStrings")
                {
                    PropertyName = "c",
                    PropertyType = typeof(List<string>),
                    PropertyValue = listStrings,
                    SecurityTokenDescriptor = new SecurityTokenDescriptor
                    {
                        Claims = new Dictionary<string, object>
                        {
                            ["c"] = listStrings
                        }
                    }
                });

                theoryData.Add(new GetPayloadValueTheoryData("ListOfObjects")
                {
                    PropertyName = "c",
                    PropertyType = typeof(List<object>),
                    PropertyValue = listObjects,
                    SecurityTokenDescriptor = new SecurityTokenDescriptor
                    {
                        Claims = new Dictionary<string, object>
                        {
                            ["c"] = listObjects
                        }
                    }
                });

                theoryData.Add(new GetPayloadValueTheoryData("CollectionOfStrings")
                {
                    PropertyName = "c",
                    PropertyType = typeof(Collection<string>),
                    PropertyValue = collectionStrings,
                    SecurityTokenDescriptor = new SecurityTokenDescriptor
                    {
                        Claims = new Dictionary<string, object>
                        {
                            ["c"] = collectionStrings
                        }
                    }
                });

                theoryData.Add(new GetPayloadValueTheoryData("CollectionOfObjects")
                {
                    PropertyName = "c",
                    PropertyType = typeof(Collection<object>),
                    PropertyValue = collectionObjects,
                    SecurityTokenDescriptor = new SecurityTokenDescriptor
                    {
                        Claims = new Dictionary<string, object>
                        {
                            ["c"] = collectionStrings
                        }
                    }
                });

                theoryData.Add(new GetPayloadValueTheoryData("ArrayOfMixedTypesAsObject")
                {
                    PropertyName = "c",
                    PropertyType = typeof(object[]),
                    PropertyValue = new object[] { 1, "2", 3 },
                    SecurityTokenDescriptor = new SecurityTokenDescriptor
                    {
                        Claims = new Dictionary<string, object>
                        {
                            ["c"] = arrayMixed
                        }
                    }
                });

                theoryData.Add(new GetPayloadValueTheoryData("ArrayOfIntAsObject")
                {
                    PropertyName = "c",
                    PropertyType = typeof(object[]),
                    PropertyValue = new object[] { 1, 2, 3 },
                    SecurityTokenDescriptor = new SecurityTokenDescriptor
                    {
                        Claims = new Dictionary<string, object>
                        {
                            ["c"] = arrayInts
                        }
                    }
                });

                theoryData.Add(new GetPayloadValueTheoryData("ArrayOfIntAsStrings")
                {
                    PropertyName = "c",
                    PropertyType = typeof(string[]),
                    PropertyValue = new string[] { "1", "2", "3" },
                    SecurityTokenDescriptor = new SecurityTokenDescriptor
                    {
                        Claims = new Dictionary<string, object>
                        {
                            ["c"] = arrayInts
                        }
                    }
                });

                theoryData.Add(new GetPayloadValueTheoryData("NotSupportedArray")
                {
                    ExpectedException = ExpectedException.ArgumentException("IDX14305:"),
                    PropertyName = "c",
                    PropertyType = typeof(Array),
                    PropertyValue = new string[] { "1", "2", "3" },
                    SecurityTokenDescriptor = new SecurityTokenDescriptor
                    {
                        Claims = new Dictionary<string, object>
                        {
                            ["c"] = arrayInts
                        }
                    }
                });

                theoryData.Add(new GetPayloadValueTheoryData("NotSupportedIList")
                {
                    ExpectedException = ExpectedException.ArgumentException("IDX14305:"),
                    PropertyName = "c",
                    PropertyType = typeof(IList),
                    PropertyValue = new string[] { "1", "2", "3" },
                    SecurityTokenDescriptor = new SecurityTokenDescriptor
                    {
                        Claims = new Dictionary<string, object>
                        {
                            ["c"] = arrayInts
                        }
                    }
                });

                theoryData.Add(new GetPayloadValueTheoryData("NotSupportedICollection")
                {
                    ExpectedException = ExpectedException.ArgumentException("IDX14305:"),
                    PropertyName = "c",
                    PropertyType = typeof(ICollection),
                    PropertyValue = new string[] { "1", "2", "3" },
                    SecurityTokenDescriptor = new SecurityTokenDescriptor
                    {
                        Claims = new Dictionary<string, object>
                        {
                            ["c"] = arrayInts
                        }
                    }
                });

                theoryData.Add(new GetPayloadValueTheoryData("NotAbleToConvert")
                {
                    ExpectedException = ExpectedException.ArgumentException("IDX14305:"),
                    PropertyName = "dic",
                    PropertyType = typeof(Dictionary<string, string[]>),
                    PropertyValue = new string[] { "1", "2", "3" },
                    SecurityTokenDescriptor = new SecurityTokenDescriptor
                    {
                        Claims = new Dictionary<string, object>
                        {
                            ["dic"] = arrayInts
                        }
                    }
                });

                theoryData.Add(new GetPayloadValueTheoryData("NotAbleToConvertToInt")
                {
                    ExpectedException = ExpectedException.ArgumentException("IDX14305:"),
                    PropertyName = "int",
                    PropertyType = typeof(int),
                    PropertyValue = "string",
                    SecurityTokenDescriptor = new SecurityTokenDescriptor
                    {
                        Claims = new Dictionary<string, object>
                        {
                            ["int"] = "string"
                        }
                    }
                });
                return theoryData;
            }
        }

        // Time values can be floats, ints, or strings.
        // This test checks to make sure that parsing does not fault in any of the above cases.
        [Theory, MemberData(nameof(ParseTimeValuesTheoryData))]
        public void ParseTimeValues(ParseTimeValuesTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ParseTimeValues", theoryData);
            var jsonWebTokenHandler = new JsonWebTokenHandler();
            try
            {
                var token = new JsonWebToken(theoryData.Header, theoryData.Payload);
                var validFrom = token.ValidFrom;
                var validTo = token.ValidTo;
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ParseTimeValuesTheoryData> ParseTimeValuesTheoryData
        {
            get
            {
                return new TheoryData<ParseTimeValuesTheoryData>
                {
                    new ParseTimeValuesTheoryData("DatesAsStrings")
                    {
                        Payload = Default.PayloadString,
                        Header = new JObject
                        {
                            { JwtHeaderParameterNames.Alg, SecurityAlgorithms.Sha512  },
                            { JwtHeaderParameterNames.Kid, Default.AsymmetricSigningKey.KeyId },
                            { JwtHeaderParameterNames.Typ, JwtConstants.HeaderType }
                        }.ToString(Formatting.None)
                    },
                    new ParseTimeValuesTheoryData("DatesAsLongs")
                    {
                        Payload = new JObject()
                        {
                            { JwtRegisteredClaimNames.Email, "Bob@contoso.com" },
                            { JwtRegisteredClaimNames.GivenName, "Bob" },
                            { JwtRegisteredClaimNames.Iss, Default.Issuer },
                            { JwtRegisteredClaimNames.Aud, Default.Audience },
                            { JwtRegisteredClaimNames.Nbf, EpochTime.GetIntDate(Default.NotBefore)},
                            { JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(Default.Expires) }
                        }.ToString(Formatting.None),
                        Header = new JObject
                        {
                            { JwtHeaderParameterNames.Alg, SecurityAlgorithms.Sha512  },
                            { JwtHeaderParameterNames.Kid, Default.AsymmetricSigningKey.KeyId },
                            { JwtHeaderParameterNames.Typ, JwtConstants.HeaderType }
                        }.ToString(Formatting.None)
                    },
                    new ParseTimeValuesTheoryData("DatesAsFloats")
                    {
                        Payload = new JObject()
                        {
                            { JwtRegisteredClaimNames.Email, "Bob@contoso.com" },
                            { JwtRegisteredClaimNames.GivenName, "Bob" },
                            { JwtRegisteredClaimNames.Iss, Default.Issuer },
                            { JwtRegisteredClaimNames.Aud, Default.Audience },
                            { JwtRegisteredClaimNames.Nbf, (float) EpochTime.GetIntDate(Default.NotBefore)},
                            { JwtRegisteredClaimNames.Exp, (float) EpochTime.GetIntDate(Default.Expires) }
                        }.ToString(Formatting.None),
                        Header = new JObject
                        {
                            { JwtHeaderParameterNames.Alg, SecurityAlgorithms.Sha512  },
                            { JwtHeaderParameterNames.Kid, Default.AsymmetricSigningKey.KeyId },
                            { JwtHeaderParameterNames.Typ, JwtConstants.HeaderType }
                        }.ToString(Formatting.None)
                    },
                };
            }
        }

        // Test ensures that we only try to populate a JsonWebToken from a string if it is a properly formatted JWT.
        // More specifically, we only want to try and decode
        // a JWT token if it has the correct number of (JWE or JWS) token parts.
        [Theory, MemberData(nameof(ParseTokenTheoryData))]
        public void ParseToken(JwtTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ParseToken", theoryData);
            try
            {
                var tokenFromEncodedString = new JsonWebToken(theoryData.Token);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JwtTheoryData> ParseTokenTheoryData
        {
            get
            {
                var theoryData = new TheoryData<JwtTheoryData>();

                JwtTestData.InvalidNumberOfSegmentsData(
                    new List<string>
                    {
                        "IDX14100:",
                        "IDX14120",
                        "IDX14121",
                        "IDX14121",
                        "IDX14310",
                        "IDX14122"
                    },
                    theoryData
                );
                JwtTestData.ValidEncodedSegmentsData(theoryData);

                theoryData.Add(new JwtTheoryData(nameof(EncodedJwts.InvalidHeader))
                {
                    Token = EncodedJwts.InvalidHeader,
                    ExpectedException = new ExpectedException(typeof(ArgumentException), "IDX14102:", typeof(JsonReaderException), true),
                });

                theoryData.Add(new JwtTheoryData(nameof(EncodedJwts.InvalidPayload))
                {
                    Token = EncodedJwts.InvalidPayload,
                    ExpectedException = new ExpectedException(typeof(ArgumentException), "IDX14101:", typeof(JsonReaderException), true),
                });

                theoryData.Add(new JwtTheoryData(nameof(EncodedJwts.JWSEmptyHeader))
                {
                    Token = EncodedJwts.JWSEmptyHeader,
                    ExpectedException = new ExpectedException(typeof(ArgumentException), "IDX14102:", typeof(JsonReaderException), true),
                });

                theoryData.Add(new JwtTheoryData(nameof(EncodedJwts.JWSEmptyPayload))
                {
                    Token = EncodedJwts.JWSEmptyPayload,
                    ExpectedException = new ExpectedException(typeof(ArgumentException), "IDX14101:", typeof(JsonReaderException), true),
                });

                theoryData.Add(new JwtTheoryData(nameof(EncodedJwts.JWEEmptyHeader))
                {
                    Token = EncodedJwts.JWEEmptyHeader,
                    ExpectedException = new ExpectedException(typeof(ArgumentException), "IDX14307:"),
                });

                theoryData.Add(new JwtTheoryData(nameof(EncodedJwts.JWEEmptyEncryptedKey))
                {
                    Token = EncodedJwts.JWEEmptyEncryptedKey,
                });

                theoryData.Add(new JwtTheoryData(nameof(EncodedJwts.JWEEmptyIV))
                {
                    ExpectedException = new ExpectedException(typeof(ArgumentException), "IDX14308:"),
                    Token = EncodedJwts.JWEEmptyIV,
                });

                theoryData.Add(new JwtTheoryData(nameof(EncodedJwts.JWEEmptyCiphertext))
                {
                    Token = EncodedJwts.JWEEmptyCiphertext,
                    ExpectedException = ExpectedException.ArgumentException(substringExpected: "IDX14306:")
                });

                theoryData.Add(new JwtTheoryData(nameof(EncodedJwts.JWEEmptyAuthenticationTag))
                {
                    ExpectedException = ExpectedException.SecurityTokenMalformedTokenException(substringExpected: "IDX14310:"),
                    Token = EncodedJwts.JWEEmptyAuthenticationTag,
                });

                return theoryData;
            }
        }

        [Fact]
        public void DateTimeISO8061Claim()
        {
            var context = new CompareContext();
            TestUtilities.WriteHeader($"{this}.DateTimeISO8061Claim");

            var encodedTokenWithDateTimeISO8061Claim = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJjbGFpbV9hc19kYXRldGltZSI6IjIwMTktMTEtMTVUMTQ6MzE6MjEuNjEwMTMyNloifQ.yYcHSl-rNT2nHe8Nb0aWe6Qu3E0ZOn2_OUidpxuw0wk";
            var claimA = new JwtSecurityTokenHandler().ReadJwtToken(encodedTokenWithDateTimeISO8061Claim).Claims.First();
            var claimB = new JsonWebTokenHandler().ReadJsonWebToken(encodedTokenWithDateTimeISO8061Claim).Claims.First();

            // both claims should be equal
            IdentityComparer.AreClaimsEqual(claimA, claimB, context);
            TestUtilities.AssertFailIfErrors(context);

            // both claim value types should be DateTime
            Assert.True(string.Equals(claimA.ValueType, ClaimValueTypes.DateTime), "ClaimValueType is not DateTime.");
            // claim value shouldn't contain any quotes
            Assert.DoesNotContain("\"", claimA.Value);
        }

        [Fact]
        public void EscapedClaims()
        {
            string json = @"{""family_name"":""\u0027\u0027"",""given_name"":""\u0027\u0027"",""name"":""謝京螢""}";
            string jsonEncoded = Base64UrlEncoder.Encode("{}") + "." + Base64UrlEncoder.Encode(json) + ".";
            JsonWebToken encodedToken = new JsonWebToken(jsonEncoded);
            _ = encodedToken.Claims;
        }
    }

    public class ParseTimeValuesTheoryData : TheoryDataBase
    {
        public ParseTimeValuesTheoryData(string testId) : base(testId) { }

        public string Payload { get; set; }

        public string Header { get; set; }
    }
}
