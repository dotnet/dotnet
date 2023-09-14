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
using Microsoft.IdentityModel.Tokens.Json.Tests;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

using JsonReaderException = System.Text.Json.JsonException;

namespace Microsoft.IdentityModel.JsonWebTokens.Tests
{
    public class JsonWebTokenTests
    {
        private static DateTime dateTime = new DateTime(2000, 01, 01, 0, 0, 0);
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

        // This test ensures audience values are skipping nulls as expected.
        [Theory, MemberData(nameof(CheckAudienceValuesTheoryData), DisableDiscoveryEnumeration = true)]
        public void CheckAudienceValues(GetPayloadValueTheoryData theoryData)
        {
            CompareContext context = TestUtilities.WriteHeader($"{this}.CheckAudienceValues", theoryData);
            try
            {
                JsonWebToken jsonWebToken = new JsonWebToken(theoryData.Json);
                MethodInfo method = typeof(JsonWebToken).GetMethod("GetPayloadValue");
                MethodInfo generic = method.MakeGenericMethod(theoryData.PropertyType);
                object[] parameters = new object[] { theoryData.PropertyName };
                var audiences = generic.Invoke(jsonWebToken, parameters);

                if (!IdentityComparer.AreEqual(jsonWebToken.Audiences, theoryData.PropertyValue, context))
                    context.AddDiff($"jsonWebToken.Audiences != theoryData.PropertyValue: '{jsonWebToken.Audiences}' != '{theoryData.PropertyValue}'.");

                if (theoryData.ClaimValue != null)
                    if (!IdentityComparer.AreEqual(audiences, theoryData.ClaimValue, context))
                        context.AddDiff($"audiences != theoryData.ClaimValue: '{audiences}' != '{theoryData.ClaimValue}'.");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex.InnerException, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<GetPayloadValueTheoryData> CheckAudienceValuesTheoryData
        {
            get
            {
                var theoryData = new TheoryData<GetPayloadValueTheoryData>();

                theoryData.Add(new GetPayloadValueTheoryData("stringFromSingleInList")
                {
                    ClaimValue = "audience",
                    PropertyName = "aud",
                    PropertyType = typeof(string),
                    PropertyValue = new List<string> { "audience" },
                    Json = JsonUtilities.CreateUnsignedToken("aud", new List<string> { "audience" })
                });

                theoryData.Add(new GetPayloadValueTheoryData("stringFromMultipeInList")
                {
                    ClaimValue = "audience",
                    ExpectedException = ExpectedException.ArgumentException("IDX14305:"),
                    PropertyName = "aud",
                    PropertyValue = new List<string> { "audience", "audience2" },
                    PropertyType = typeof(string),
                    Json = JsonUtilities.CreateUnsignedToken("aud", new List<string> { "audience", "audience2" })
                });

                theoryData.Add(new GetPayloadValueTheoryData("stringTwoNulloneNonNull")
                {
                    ClaimValue = "audience1",
                    PropertyName = "aud",
                    PropertyValue = new List<string> { "audience1" },
                    PropertyType = typeof(string),
                    Json = JsonUtilities.CreateUnsignedToken("aud", new List<string> { null, "audience1", null })
                });

                theoryData.Add(new GetPayloadValueTheoryData("stringFromCollection")
                {
                    ClaimValue = "audience",
                    PropertyName = "aud",
                    PropertyType = typeof(string),
                    PropertyValue = new Collection<string> { "audience" },
                    Json = JsonUtilities.CreateUnsignedToken("aud", new Collection<string> { "audience" })
                });

                theoryData.Add(new GetPayloadValueTheoryData("singleNull")
                {
                    ClaimValue = new List<string>(),
                    PropertyName = "aud",
                    PropertyValue = new List<string>(),
                    PropertyType = typeof(List<string>),
                    Json = JsonUtilities.CreateUnsignedToken("aud", null)
                });

                theoryData.Add(new GetPayloadValueTheoryData("twoNull")
                {
                    ClaimValue = new List<string>(),
                    PropertyName = "aud",
                    PropertyValue = new List<string>(),
                    PropertyType = typeof(List<string>),
                    Json = JsonUtilities.CreateUnsignedToken("aud", new List<string>{ null, null })
                });

                theoryData.Add(new GetPayloadValueTheoryData("singleNonNull")
                {
                    ClaimValue = new List<string> { "audience" },
                    PropertyName = "aud",
                    PropertyValue = new List<string> { "audience"},
                    PropertyType = typeof(List<string>),
                    Json = JsonUtilities.CreateUnsignedToken("aud", "audience")
                });

                theoryData.Add(new GetPayloadValueTheoryData("twoNulloneNonNull")
                {
                    ClaimValue = new List<string> { "audience1" },
                    PropertyName = "aud",
                    PropertyValue = new List<string> { "audience1"},
                    PropertyType = typeof(List<string>),
                    Json = JsonUtilities.CreateUnsignedToken("aud", new List<string> { null, "audience1", null })
                });

                return theoryData;
            }
        }

        // This test ensures that TryGetPayloadValue does not throw
        // No need to check for equal as GetPayloadValue does that
        [Theory, MemberData(nameof(GetPayloadValueTheoryData), DisableDiscoveryEnumeration = true)]
        public void TryGetPayloadValue(GetPayloadValueTheoryData theoryData)
        {
            CompareContext context = TestUtilities.WriteHeader($"{this}.TryGetPayloadValue", theoryData);
            string payload = null;
            try
            {
                JsonWebToken jsonWebToken = new JsonWebToken(theoryData.Json);
                payload = Base64UrlEncoder.Decode(jsonWebToken.EncodedPayload);
                MethodInfo method = typeof(JsonWebToken).GetMethod("TryGetPayloadValue");
                MethodInfo generic = method.MakeGenericMethod(theoryData.PropertyType);
                object[] parameters = new object[] { theoryData.PropertyName, null };
                var retVal = generic.Invoke(jsonWebToken, parameters);
            }
            catch (Exception ex)
            {
                context.AddDiff($"TryGetPayloadValue: payload: '{payload}'.  threw an exception: {ex.GetType()}: {ex.Message}");
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        // This test ensures that our roundtripping works as expected.
        [Theory, MemberData(nameof(GetPayloadValueTheoryData), DisableDiscoveryEnumeration = true)]
        public void GetPayloadValue(GetPayloadValueTheoryData theoryData)
        {
            CompareContext context = TestUtilities.WriteHeader($"{this}.GetPayloadValue", theoryData);
            try
            {
                JsonWebToken jsonWebToken = new JsonWebToken(theoryData.Json);
                string payload = Base64UrlEncoder.Decode(jsonWebToken.EncodedPayload);
                MethodInfo method = typeof(JsonWebToken).GetMethod("GetPayloadValue");
                MethodInfo generic = method.MakeGenericMethod(theoryData.PropertyType);
                object[] parameters = new object[] { theoryData.PropertyName };
                var retVal = generic.Invoke(jsonWebToken, parameters);

                theoryData.ExpectedException.ProcessNoException(context);
                IdentityComparer.AreEqual(retVal, theoryData.PropertyValue, context);
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
                DateTime dateTime = DateTime.UtcNow;

                #region simple types from string

                theoryData.Add(new GetPayloadValueTheoryData("stringFromDateTime")
                {
                    PropertyName = "stringFromDateTime",
                    PropertyType = typeof(string),
                    PropertyValue = dateTime.ToString("o", CultureInfo.InvariantCulture),
                    Json = JsonUtilities.CreateUnsignedToken("stringFromDateTime", dateTime)
                });

                theoryData.Add(new GetPayloadValueTheoryData("stringFromDateTimeString")
                {
                    PropertyName = "stringFromDateTime",
                    PropertyType = typeof(string),
                    PropertyValue = dateTime.ToString("o", CultureInfo.InvariantCulture),
                    Json = JsonUtilities.CreateUnsignedToken("stringFromDateTime", dateTime.ToString("o", CultureInfo.InvariantCulture))
                });

                theoryData.Add(new GetPayloadValueTheoryData("dateTimeFromString")
                {
                    PropertyName = "dateTime",
                    PropertyType = typeof(DateTime),
                    PropertyValue = dateTime,
                    Json = JsonUtilities.CreateUnsignedToken("dateTime", dateTime.ToString("o", CultureInfo.InvariantCulture))
                });

                theoryData.Add(new GetPayloadValueTheoryData("tRueFromString")
                {
                    PropertyName = "tRue",
                    PropertyType = typeof(bool),
                    PropertyValue = true,
                    Json = JsonUtilities.CreateUnsignedToken("tRue", "tRue")
                });

                theoryData.Add(new GetPayloadValueTheoryData("trueFromString")
                {
                    PropertyName = "true",
                    PropertyType = typeof(bool),
                    PropertyValue = true,
                    Json = JsonUtilities.CreateUnsignedToken("true", "true")
                });

                theoryData.Add(new GetPayloadValueTheoryData("FalseFromString")
                {
                    PropertyName = "False",
                    PropertyType = typeof(bool),
                    PropertyValue = false,
                    Json = JsonUtilities.CreateUnsignedToken("False", "False")
                });

                theoryData.Add(new GetPayloadValueTheoryData("doubleFromString")
                {
                    PropertyName = "double",
                    PropertyType = typeof(double),
                    PropertyValue = 622.101,
                    Json = JsonUtilities.CreateUnsignedToken("double", "622.101")
                });

                theoryData.Add(new GetPayloadValueTheoryData("decimalFromString")
                {
                    PropertyName = "decimal",
                    PropertyType = typeof(decimal),
                    PropertyValue = 422.101,
                    Json = JsonUtilities.CreateUnsignedToken("decimal", "422.101")
                });

                theoryData.Add(new GetPayloadValueTheoryData("floatFromString")
                {
                    PropertyName = "float",
                    PropertyType = typeof(float),
                    PropertyValue = 42.1,
                    Json = JsonUtilities.CreateUnsignedToken("float", "42.1")
                });

                theoryData.Add(new GetPayloadValueTheoryData("integerFromString")
                {
                    PropertyName = "integer",
                    PropertyType = typeof(int),
                    PropertyValue = 42,
                    Json = JsonUtilities.CreateUnsignedToken("integer", "42")
                });

                theoryData.Add(new GetPayloadValueTheoryData("uintFromString")
                {
                    PropertyName = "uint",
                    PropertyType = typeof(uint),
                    PropertyValue = 540,
                    Json = JsonUtilities.CreateUnsignedToken("uint", "540")
                });

                theoryData.Add(new GetPayloadValueTheoryData("ulongFromString")
                {
                    PropertyName = "ulong",
                    PropertyType = typeof(ulong),
                    PropertyValue = 642,
                    Json = JsonUtilities.CreateUnsignedToken("ulong", "642")
                });
                #endregion

                #region simple types
                theoryData.Add(new GetPayloadValueTheoryData("dateTime")
                {
                    PropertyName = "dateTime",
                    PropertyType = typeof(DateTime),
                    PropertyValue = dateTime,
                    Json = JsonUtilities.CreateUnsignedToken("dateTime", dateTime)
                });

                theoryData.Add(new GetPayloadValueTheoryData("null")
                {
                    PropertyName = "null",
                    PropertyType = typeof(object),
                    PropertyValue = null,
                    Json = JsonUtilities.CreateUnsignedToken("null", null)
                });

                theoryData.Add(new GetPayloadValueTheoryData("true")
                {
                    PropertyName = "true",
                    PropertyType = typeof(bool),
                    PropertyValue = true,
                    Json = JsonUtilities.CreateUnsignedToken("true", true)
                });

                theoryData.Add(new GetPayloadValueTheoryData("false")
                {
                    PropertyName = "false",
                    PropertyType = typeof(bool),
                    PropertyValue = false,
                    Json = JsonUtilities.CreateUnsignedToken("false", false)
                });

                theoryData.Add(new GetPayloadValueTheoryData("double")
                {
                    PropertyName = "double",
                    PropertyType = typeof(double),
                    PropertyValue = 422.101,
                    Json = JsonUtilities.CreateUnsignedToken("double", 422.101)
                });

                theoryData.Add(new GetPayloadValueTheoryData("decimal")
                {
                    PropertyName = "decimal",
                    PropertyType = typeof(decimal),
                    PropertyValue = 422.101,
                    Json = JsonUtilities.CreateUnsignedToken("decimal", 422.101)
                });

                theoryData.Add(new GetPayloadValueTheoryData("float")
                {
                    PropertyName = "float",
                    PropertyType = typeof(float),
                    PropertyValue = 42.1,
                    Json = JsonUtilities.CreateUnsignedToken("float", 42.1)
                });

                theoryData.Add(new GetPayloadValueTheoryData("integer")
                {
                    PropertyName = "integer",
                    PropertyType = typeof(int),
                    PropertyValue = 42,
                    Json = JsonUtilities.CreateUnsignedToken("integer", 42)
                });

                theoryData.Add(new GetPayloadValueTheoryData("uint")
                {
                    PropertyName = "uint",
                    PropertyType = typeof(uint),
                    PropertyValue = 42,
                    Json = JsonUtilities.CreateUnsignedToken("uint", 42)
                });

                theoryData.Add(new GetPayloadValueTheoryData("ulong")
                {
                    PropertyName = "ulong",
                    PropertyType = typeof(ulong),
                    PropertyValue = 42,
                    Json = JsonUtilities.CreateUnsignedToken("ulong", 42)
                });

                theoryData.Add(new GetPayloadValueTheoryData("string")
                {
                    PropertyName = "string",
                    PropertyType = typeof(string),
                    PropertyValue = "property",
                    Json = JsonUtilities.CreateUnsignedToken("string", "property")
                });
                #endregion

                #region collection of strings from simple types

                #region string[]
                theoryData.Add(new GetPayloadValueTheoryData("string[]dateTime")
                {
                    PropertyName = "dateTime",
                    PropertyType = typeof(string[]),
                    PropertyValue = new string[] {dateTime.ToString("o", CultureInfo.InvariantCulture)},
                    Json = JsonUtilities.CreateUnsignedToken("dateTime", dateTime)
                });

                theoryData.Add(new GetPayloadValueTheoryData("string[]true")
                {
                    PropertyName = "true",
                    PropertyType = typeof(string[]),
                    PropertyValue = new string[] { "True" },
                    Json = JsonUtilities.CreateUnsignedToken("true", true)
                });

                theoryData.Add(new GetPayloadValueTheoryData("string[]double")
                {
                    PropertyName = "double",
                    PropertyType = typeof(string[]),
                    PropertyValue = new string[] { "422.101" },
                    Json = JsonUtilities.CreateUnsignedToken("double", 422.101)
                });

                theoryData.Add(new GetPayloadValueTheoryData("string[]integer")
                {
                    PropertyName = "integer",
                    PropertyType = typeof(string[]),
                    PropertyValue = new string[] { "42" },
                    Json = JsonUtilities.CreateUnsignedToken("integer", 42)
                });

                theoryData.Add(new GetPayloadValueTheoryData("string[]ulong")
                {
                    PropertyName = "ulong",
                    PropertyType = typeof(string[]),
                    PropertyValue = new string[] { "42" },
                    Json = JsonUtilities.CreateUnsignedToken("ulong", 42)
                });

                theoryData.Add(new GetPayloadValueTheoryData("string[]string")
                {
                    PropertyName = "string",
                    PropertyType = typeof(string[]),
                    PropertyValue = new string[] { "property" },
                    Json = JsonUtilities.CreateUnsignedToken("string", "property")
                });
                #endregion

                #region List:string
                theoryData.Add(new GetPayloadValueTheoryData("List<string>dateTime")
                {
                    PropertyName = "dateTime",
                    PropertyType = typeof(List<string>),
                    PropertyValue = new List<string> { dateTime.ToString("o", CultureInfo.InvariantCulture) },
                    Json = JsonUtilities.CreateUnsignedToken("dateTime", dateTime)
                });

                theoryData.Add(new GetPayloadValueTheoryData("List<string>true")
                {
                    PropertyName = "true",
                    PropertyType = typeof(List<string>),
                    PropertyValue = new List<string> { "True" },
                    Json = JsonUtilities.CreateUnsignedToken("true", true)
                });

                theoryData.Add(new GetPayloadValueTheoryData("List<string>double")
                {
                    PropertyName = "double",
                    PropertyType = typeof(List<string>),
                    PropertyValue = new List<string> { "422.101" },
                    Json = JsonUtilities.CreateUnsignedToken("double", 422.101)
                });

                theoryData.Add(new GetPayloadValueTheoryData("List<string>integer")
                {
                    PropertyName = "integer",
                    PropertyType = typeof(List<string>),
                    PropertyValue = new List<string> { "42" },
                    Json = JsonUtilities.CreateUnsignedToken("integer", 42)
                });

                theoryData.Add(new GetPayloadValueTheoryData("List<string>ulong")
                {
                    PropertyName = "ulong",
                    PropertyType = typeof(List<string>),
                    PropertyValue = new List<string> { "42" },
                    Json = JsonUtilities.CreateUnsignedToken("ulong", 42)
                });

                theoryData.Add(new GetPayloadValueTheoryData("List<string>string")
                {
                    PropertyName = "string",
                    PropertyType = typeof(List<string>),
                    PropertyValue = new List<string> { "property" },
                    Json = JsonUtilities.CreateUnsignedToken("string", "property")
                });
                #endregion

                #region Collection:string
                theoryData.Add(new GetPayloadValueTheoryData("Collection<string>dateTime")
                {
                    PropertyName = "dateTime",
                    PropertyType = typeof(Collection<string>),
                    PropertyValue = new Collection<string> { dateTime.ToString("o", CultureInfo.InvariantCulture) },
                    Json = JsonUtilities.CreateUnsignedToken("dateTime", dateTime)
                });

                theoryData.Add(new GetPayloadValueTheoryData("Collection<string>true")
                {
                    PropertyName = "true",
                    PropertyType = typeof(Collection<string>),
                    PropertyValue = new Collection<string> { "True" },
                    Json = JsonUtilities.CreateUnsignedToken("true", true)
                });

                theoryData.Add(new GetPayloadValueTheoryData("Collection<string>double")
                {
                    PropertyName = "double",
                    PropertyType = typeof(Collection<string>),
                    PropertyValue = new Collection<string> { "422.101" },
                    Json = JsonUtilities.CreateUnsignedToken("double", 422.101)
                });

                theoryData.Add(new GetPayloadValueTheoryData("Collection<string>integer")
                {
                    PropertyName = "integer",
                    PropertyType = typeof(Collection<string>),
                    PropertyValue = new Collection<string> { "42" },
                    Json = JsonUtilities.CreateUnsignedToken("integer", 42)
                });

                theoryData.Add(new GetPayloadValueTheoryData("Collection<string>ulong")
                {
                    PropertyName = "ulong",
                    PropertyType = typeof(Collection<string>),
                    PropertyValue = new Collection<string> { "42" },
                    Json = JsonUtilities.CreateUnsignedToken("ulong", 42)
                });

                theoryData.Add(new GetPayloadValueTheoryData("Collection<string>string")
                {
                    PropertyName = "string",
                    PropertyType = typeof(Collection<string>),
                    PropertyValue = new Collection<string> { "property" },
                    Json = JsonUtilities.CreateUnsignedToken("string", "property")
                });
                #endregion

                #region IList:string
                theoryData.Add(new GetPayloadValueTheoryData("IList<string>dateTime")
                {
                    PropertyName = "dateTime",
                    PropertyType = typeof(IList<string>),
                    PropertyValue = new List<string> { dateTime.ToString("o", CultureInfo.InvariantCulture) },
                    Json = JsonUtilities.CreateUnsignedToken("dateTime", dateTime)
                });

                theoryData.Add(new GetPayloadValueTheoryData("IList<string>true")
                {
                    PropertyName = "true",
                    PropertyType = typeof(IList<string>),
                    PropertyValue = new List<string> { "True" },
                    Json = JsonUtilities.CreateUnsignedToken("true", true)
                });

                theoryData.Add(new GetPayloadValueTheoryData("IList<string>double")
                {
                    PropertyName = "double",
                    PropertyType = typeof(IList<string>),
                    PropertyValue = new List<string> { "422.101" },
                    Json = JsonUtilities.CreateUnsignedToken("double", 422.101)
                });

                theoryData.Add(new GetPayloadValueTheoryData("IList<string>integer")
                {
                    PropertyName = "integer",
                    PropertyType = typeof(IList<string>),
                    PropertyValue = new List<string> { "42" },
                    Json = JsonUtilities.CreateUnsignedToken("integer", 42)
                });

                theoryData.Add(new GetPayloadValueTheoryData("IList<string>ulong")
                {
                    PropertyName = "ulong",
                    PropertyType = typeof(IList<string>),
                    PropertyValue = new List<string> { "42" },
                    Json = JsonUtilities.CreateUnsignedToken("ulong", 42)
                });

                theoryData.Add(new GetPayloadValueTheoryData("IList<string>string")
                {
                    PropertyName = "string",
                    PropertyType = typeof(IList<string>),
                    PropertyValue = new List<string> { "property" },
                    Json = JsonUtilities.CreateUnsignedToken("string", "property")
                });
                #endregion

                #region ICollection:string
                theoryData.Add(new GetPayloadValueTheoryData("ICollection<string>dateTime")
                {
                    PropertyName = "dateTime",
                    PropertyType = typeof(ICollection<string>),
                    PropertyValue = new Collection<string> { dateTime.ToString("o", CultureInfo.InvariantCulture) },
                    Json = JsonUtilities.CreateUnsignedToken("dateTime", dateTime)
                });

                theoryData.Add(new GetPayloadValueTheoryData("ICollection<string>true")
                {
                    PropertyName = "true",
                    PropertyType = typeof(ICollection<string>),
                    PropertyValue = new Collection<string> { "True" },
                    Json = JsonUtilities.CreateUnsignedToken("true", true)
                });

                theoryData.Add(new GetPayloadValueTheoryData("ICollection<string>double")
                {
                    PropertyName = "double",
                    PropertyType = typeof(ICollection<string>),
                    PropertyValue = new Collection<string> { "422.101" },
                    Json = JsonUtilities.CreateUnsignedToken("double", 422.101)
                });

                theoryData.Add(new GetPayloadValueTheoryData("ICollection<string>integer")
                {
                    PropertyName = "integer",
                    PropertyType = typeof(ICollection<string>),
                    PropertyValue = new Collection<string> { "42" },
                    Json = JsonUtilities.CreateUnsignedToken("integer", 42)
                });

                theoryData.Add(new GetPayloadValueTheoryData("ICollection<string>ulong")
                {
                    PropertyName = "ulong",
                    PropertyType = typeof(ICollection<string>),
                    PropertyValue = new Collection<string> { "42" },
                    Json = JsonUtilities.CreateUnsignedToken("ulong", 42)
                });

                theoryData.Add(new GetPayloadValueTheoryData("ICollection<string>string")
                {
                    PropertyName = "string",
                    PropertyType = typeof(ICollection<string>),
                    PropertyValue = new Collection<string> { "property" },
                    Json = JsonUtilities.CreateUnsignedToken("string", "property")
                });
                #endregion

                #endregion

                #region complex types, dictionary, list, array, collection
                List<string> listStrings = new List<string> { "listValue1", "listValue2" };
                List<object> listObjects = new List<object> { "listValue1", "listValue2" };
                Collection<string> collectionStrings = new Collection<string> { "collectionValue1", "collectionValue2" };
                Collection<object> collectionObjects = new Collection<object> { "collectionValue1", "collectionValue2" };
                string[] arrayStrings = new string[] { "arrayValue1", "arrayValue2" };
                string[] arrayIntAsStrings = new string[] { "1", "2", "3" };
                object[] arrayObjects = new object[] { "arrayValue1", "arrayValue2" };
                object[] arrayMixed = new object[] { 1, "2", 3 };
                object[] arrayIntAsObjects = new object[] { 1, 2, 3 };
                int[] arrayInts = new int[] { 1, 2, 3 };

                object propertyValue = new Dictionary<string, string[]>
                {
                    ["property1"] = arrayStrings
                };

                theoryData.Add(new GetPayloadValueTheoryData("DictionaryWithArrayOfStrings")
                {
                    PropertyName = "a",
                    PropertyType = typeof(Dictionary<string, string[]>),
                    PropertyValue = propertyValue,
                    Json = JsonUtilities.CreateUnsignedToken("a", propertyValue)
                });

                propertyValue = new Dictionary<string, List<string>>
                {
                    ["property1"] = listStrings
                };

                theoryData.Add(new GetPayloadValueTheoryData("DictionaryWithListOfStrings")
                {
                    PropertyName = "a",
                    PropertyType = typeof(Dictionary<string, List<string>>),
                    PropertyValue = propertyValue,
                    Json = JsonUtilities.CreateUnsignedToken("a", propertyValue)
                });

                propertyValue = new Dictionary<string, Collection<string>>
                {
                    ["property1"] = collectionStrings
                };

                theoryData.Add(new GetPayloadValueTheoryData("DictionaryWithCollectionOfStrings")
                {
                    PropertyName = "a",
                    PropertyType = typeof(Dictionary<string, Collection<string>>),
                    PropertyValue = propertyValue,
                    Json = JsonUtilities.CreateUnsignedToken("a", propertyValue)
                });

                theoryData.Add(new GetPayloadValueTheoryData("ArrayOfStrings")
                {
                    PropertyName = "c",
                    PropertyType = typeof(string[]),
                    PropertyValue = arrayStrings,
                    Json = JsonUtilities.CreateUnsignedToken("c", arrayStrings)
                });

                theoryData.Add(new GetPayloadValueTheoryData("ArrayOfObjects")
                {
                    PropertyName = "c",
                    PropertyType = typeof(object[]),
                    PropertyValue = arrayObjects,
                    Json = JsonUtilities.CreateUnsignedToken("c", arrayObjects)
                });

                theoryData.Add(new GetPayloadValueTheoryData("ListOfStrings")
                {
                    PropertyName = "c",
                    PropertyType = typeof(List<string>),
                    PropertyValue = listStrings,
                    Json = JsonUtilities.CreateUnsignedToken("c", listStrings)
                });

                theoryData.Add(new GetPayloadValueTheoryData("IListOfStrings")
                {
                    PropertyName = "c",
                    PropertyType = typeof(IList<string>),
                    PropertyValue = listStrings,
                    Json = JsonUtilities.CreateUnsignedToken("c", listStrings)
                });

                theoryData.Add(new GetPayloadValueTheoryData("ListOfObjects")
                {
                    PropertyName = "c",
                    PropertyType = typeof(List<object>),
                    PropertyValue = listObjects,
                    Json = JsonUtilities.CreateUnsignedToken("c", listObjects)
                });

                theoryData.Add(new GetPayloadValueTheoryData("CollectionOfStrings")
                {
                    PropertyName = "c",
                    PropertyType = typeof(Collection<string>),
                    PropertyValue = collectionStrings,
                    Json = JsonUtilities.CreateUnsignedToken("c", collectionStrings)
                });

                theoryData.Add(new GetPayloadValueTheoryData("ICollectionOfStrings")
                {
                    PropertyName = "c",
                    PropertyType = typeof(ICollection<string>),
                    PropertyValue = collectionStrings,
                    Json = JsonUtilities.CreateUnsignedToken("c", collectionStrings)
                });

                theoryData.Add(new GetPayloadValueTheoryData("CollectionOfObjects")
                {
                    PropertyName = "c",
                    PropertyType = typeof(Collection<object>),
                    PropertyValue = collectionObjects,
                    Json = JsonUtilities.CreateUnsignedToken("c", collectionObjects)
                });

                theoryData.Add(new GetPayloadValueTheoryData("ArrayOfMixedTypesAsObject")
                {
                    PropertyName = "c",
                    PropertyType = typeof(object[]),
                    PropertyValue = arrayMixed,
                    Json = JsonUtilities.CreateUnsignedToken("c", arrayMixed)
                });

                theoryData.Add(new GetPayloadValueTheoryData("ArrayOfIntAsObject")
                {
                    PropertyName = "c",
                    PropertyType = typeof(object[]),
                    PropertyValue = arrayIntAsObjects,
                    Json = JsonUtilities.CreateUnsignedToken("c", arrayInts)
                });

                theoryData.Add(new GetPayloadValueTheoryData("ArrayOfIntAsStrings")
                {
                    PropertyName = "c",
                    PropertyType = typeof(string[]),
                    PropertyValue = arrayIntAsStrings,
                    Json = JsonUtilities.CreateUnsignedToken("c", arrayInts)
                });
                #endregion

                #region unsupported types / failures
                theoryData.Add(new GetPayloadValueTheoryData("NotSupportedArray")
                {
                    ExpectedException = ExpectedException.ArgumentException("IDX14305:"),
                    PropertyName = "c",
                    PropertyType = typeof(Array),
                    PropertyValue = arrayIntAsStrings,
                    Json = JsonUtilities.CreateUnsignedToken("c", arrayInts)
                });

                theoryData.Add(new GetPayloadValueTheoryData("NotSupportedIList")
                {
                    ExpectedException = ExpectedException.ArgumentException("IDX14305:"),
                    PropertyName = "c",
                    PropertyType = typeof(IList),
                    PropertyValue = arrayIntAsStrings,
                    Json = JsonUtilities.CreateUnsignedToken("c", arrayInts)
                });

                theoryData.Add(new GetPayloadValueTheoryData("NotSupportedICollection")
                {
                    ExpectedException = ExpectedException.ArgumentException("IDX14305:"),
                    PropertyName = "c",
                    PropertyType = typeof(ICollection),
                    PropertyValue = arrayIntAsStrings,
                    Json = JsonUtilities.CreateUnsignedToken("c", arrayInts)
                });

                theoryData.Add(new GetPayloadValueTheoryData("NotAbleToConvert")
                {
                    ExpectedException = ExpectedException.ArgumentException("IDX14305:"),
                    PropertyName = "dic",
                    PropertyType = typeof(Dictionary<string, string[]>),
                    PropertyValue = arrayIntAsStrings,
                    Json = JsonUtilities.CreateUnsignedToken("dic", arrayInts)
                });

                theoryData.Add(new GetPayloadValueTheoryData("NotAbleToConvertToInt")
                {
                    ExpectedException = ExpectedException.ArgumentException("IDX14305:"),
                    PropertyName = "int",
                    PropertyType = typeof(int),
                    PropertyValue = "string",
                    Json = JsonUtilities.CreateUnsignedToken("int", "string")
                });

                theoryData.Add(new GetPayloadValueTheoryData("doubleToInt")
                {
                    ExpectedException = ExpectedException.ArgumentException("IDX14305:"),
                    PropertyName = "doubleToInt",
                    PropertyType = typeof(int),
                    PropertyValue = 422.101,
                    Json = JsonUtilities.CreateUnsignedToken("doubleToInt", 422.101),
                });

                theoryData.Add(new GetPayloadValueTheoryData("propertyNotFound")
                {
                    ExpectedException = ExpectedException.ArgumentException("IDX14304:"),
                    PropertyName = "doubleToInt",
                    PropertyType = typeof(int),
                    PropertyValue = 422.101,
                    Json = JsonUtilities.CreateUnsignedToken("propertyNotFound", 422.101),
                });
                #endregion

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

            DateTime dateTime = DateTime.UtcNow;
            string token = JsonUtilities.CreateUnsignedToken("dateTime", dateTime);
            var claimA = new JwtSecurityTokenHandler().ReadJwtToken(token).Claims.First();
            var claimB = new JsonWebTokenHandler().ReadJsonWebToken(token).Claims.First();

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
