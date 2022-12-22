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
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens.Jwt.Tests;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Json;
using Microsoft.IdentityModel.Json.Linq;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.JsonWebTokens.Tests
{
    public class JsonWebTokenTests
    {
        private static DateTime dateTime = new DateTime(2000, 01, 01, 0, 0, 0);
        private string jObject = $@"{{""intarray"":[1,2,3], ""array"":[1,""2"",3], ""jobject"": {{ ""string1"":""string1value"", ""string2"":""string2value"" }},""string"":""bob"", ""float"":42.0, ""integer"":42, ""nill"": null, ""bool"" : true, ""dateTime"": ""{dateTime}"", ""dateTimeIso8061"": ""{dateTime.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture)}"" }}";
        private List<Claim> payloadClaims = new List<Claim>()
        {
            new Claim("intarray", @"[1,2,3]", JsonClaimValueTypes.JsonArray, "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
            new Claim("array", @"[1,""2"",3]", JsonClaimValueTypes.JsonArray, "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
            new Claim("jobject", @"{""string1"":""string1value"",""string2"":""string2value""}", JsonClaimValueTypes.Json, "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
            new Claim("string", "bob", ClaimValueTypes.String, "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
            new Claim("float", "42.0", ClaimValueTypes.Double, "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
            new Claim("integer", "42", ClaimValueTypes.Integer, "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
            new Claim("nill", "", JsonClaimValueTypes.JsonNull, "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
            new Claim("bool", "true", ClaimValueTypes.Boolean, "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
            new Claim("dateTime", dateTime.ToString(), ClaimValueTypes.String, "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
            new Claim("dateTimeIso8061", dateTime.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture), ClaimValueTypes.DateTime, "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
        };

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
            var jsonWebToken = new JsonWebToken("{}", jObject.ToString());

            foreach (var claim in payloadClaims)
            {
                var claimToCompare = jsonWebToken.GetClaim(claim.Type);
                IdentityComparer.AreEqual(claim, claimToCompare, context);
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
            var jsonWebToken = new JsonWebToken("{}", jObject.ToString());

            // Tries to retrieve a value that does not exist in the payload.
            var success = jsonWebToken.TryGetClaim("doesnotexist", out Claim doesNotExist);
            IdentityComparer.AreEqual(null, doesNotExist, context);
            IdentityComparer.AreEqual(false, success, context);

            foreach (var claim in payloadClaims)
            {
                success = jsonWebToken.TryGetClaim(claim.Type, out var claimToCompare);
                IdentityComparer.AreEqual(claim, claimToCompare, context);
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
        public void GetMultipleAudiences()
        {
            var context = new CompareContext();
            var tokenString = "eyJhbGciOiJSUzI1NiJ9.eyJhdWQiOlsiaHR0cDovL0RlZmF1bHQuQXVkaWVuY2UuY29tIiwiaHR0cDovL0RlZmF1bHQuQXVkaWVuY2UxLmNvbSIsImh0dHA6Ly9EZWZhdWx0LkF1ZGllbmNlMi5jb20iLCJodHRwOi8vRGVmYXVsdC5BdWRpZW5jZTMuY29tIiwiaHR0cDovL0RlZmF1bHQuQXVkaWVuY2U0LmNvbSJdLCJleHAiOjE1Mjg4NTAyNzgsImlhdCI6MTUyODg1MDI3OCwiaXNzIjoiaHR0cDovL0RlZmF1bHQuSXNzdWVyLmNvbSIsIm5vbmNlIjoiRGVmYXVsdC5Ob25jZSIsInN1YiI6InVybjpvYXNpczpuYW1zOnRjOlNBTUw6MS4xOm5hbWVpZC1mb3JtYXQ6WDUwOVN1YmplY3ROYW1lIn0.";
            var jsonWebToken = new JsonWebToken(tokenString);
            var jwtSecurityToken = new JwtSecurityToken(tokenString);
            IdentityComparer.AreEqual(jsonWebToken.Claims, jwtSecurityToken.Claims);
            IdentityComparer.AreEqual(jsonWebToken.Audiences, jwtSecurityToken.Audiences, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        // Test checks to make sure that claim values of various types can be successfully retrieved from the header.
        [Fact]
        public void GetHeaderValues()
        {
            var context = new CompareContext();
            IdentityModelEventSource.ShowPII = true;
            TestUtilities.WriteHeader($"{this}.GetHeaderValues");

            var token = new JsonWebToken(jObject, "{}");

            var intarray = token.GetHeaderValue<int[]>("intarray");
            IdentityComparer.AreEqual(new int[] { 1, 2, 3 }, intarray, context);

            var array = token.GetHeaderValue<object[]>("array");
            IdentityComparer.AreEqual(new object[] { 1L, "2", 3L }, array, context);

            // only possible internally within the library since we're using Microsoft.IdentityModel.Json.Linq.JObject
            var jobject = token.GetHeaderValue<JObject>("jobject");
            IdentityComparer.AreEqual(JObject.Parse(@"{ ""string1"":""string1value"", ""string2"":""string2value"" }"), jobject, context);

            var name = token.GetHeaderValue<string>("string");
            IdentityComparer.AreEqual("bob", name, context);

            var floatingPoint = token.GetHeaderValue<float>("float");
            IdentityComparer.AreEqual(42.0, floatingPoint, context);

            var integer = token.GetHeaderValue<int>("integer");
            IdentityComparer.AreEqual(42, integer, context);

            var nill = token.GetHeaderValue<object>("nill");
            IdentityComparer.AreEqual(nill, null, context);

            var boolean = token.GetHeaderValue<bool>("bool");
            IdentityComparer.AreEqual(boolean, true, context);

            try // Try to retrieve a value that doesn't exist in the header.
            {
                token.GetHeaderValue<int>("doesnotexist");
            }
            catch (Exception ex)
            {
                ExpectedException.ArgumentException("IDX14303:").ProcessException(ex, context);
            }

            try // Try to retrieve an integer when the value is actually a string.
            {
                token.GetHeaderValue<int>("string");
            }
            catch (Exception ex)
            {
                ExpectedException.ArgumentException("IDX14305:", typeof(System.FormatException)).ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        // Test checks to make sure that claim values of various types can be successfully retrieved from the header.
        [Fact]
        public void TryGetHeaderValues()
        {
            var context = new CompareContext();
            TestUtilities.WriteHeader($"{this}.TryGetHeaderValues");

            var token = new JsonWebToken(jObject, "{}");

            var success = token.TryGetHeaderValue("intarray", out int[] intarray);
            IdentityComparer.AreEqual(new int[] { 1, 2, 3 }, intarray, context);
            IdentityComparer.AreEqual(true, success, context);

            success = token.TryGetHeaderValue("array", out object[] array);
            IdentityComparer.AreEqual(new object[] { 1L, "2", 3L }, array, context);
            IdentityComparer.AreEqual(true, success, context);

            // only possible internally within the library since we're using Microsoft.IdentityModel.Json.Linq.JObject
            success = token.TryGetHeaderValue("jobject", out JObject jobject);
            IdentityComparer.AreEqual(JObject.Parse(@"{ ""string1"":""string1value"", ""string2"":""string2value"" }"), jobject, context);
            IdentityComparer.AreEqual(true, success, context);

            success = token.TryGetHeaderValue("string", out string name);
            IdentityComparer.AreEqual("bob", name, context);
            IdentityComparer.AreEqual(true, success, context);

            success = token.TryGetHeaderValue("float", out float floatingPoint);
            IdentityComparer.AreEqual(42.0, floatingPoint, context);
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

            var token = new JsonWebToken("{}", jObject);

            var intarray = token.GetPayloadValue<int[]>("intarray");
            IdentityComparer.AreEqual(new int[] { 1, 2, 3 }, intarray, context);

            var array = token.GetPayloadValue<object[]>("array");
            IdentityComparer.AreEqual(new object[] { 1L, "2", 3L }, array, context);

            // only possible internally within the library since we're using Microsoft.IdentityModel.Json.Linq.JObject
            var jobject = token.GetPayloadValue<JObject>("jobject");
            IdentityComparer.AreEqual(JObject.Parse(@"{ ""string1"":""string1value"", ""string2"":""string2value"" }"), jobject, context);

            var name = token.GetPayloadValue<string>("string");
            IdentityComparer.AreEqual("bob", name, context);

            var floatingPoint = token.GetPayloadValue<float>("float");
            IdentityComparer.AreEqual(42.0, floatingPoint, context);

            var integer = token.GetPayloadValue<int>("integer");
            IdentityComparer.AreEqual(42, integer, context);

            var nill = token.GetPayloadValue<object>("nill");
            IdentityComparer.AreEqual(nill, null, context);

            var boolean = token.GetPayloadValue<bool>("bool");
            IdentityComparer.AreEqual(boolean, true, context);

            var dateTimeValue = token.GetPayloadValue<string>("dateTime");
            IdentityComparer.AreEqual(dateTimeValue, dateTime.ToString(), context);

            var dateTimeIso8061Value = token.GetPayloadValue<DateTime>("dateTimeIso8061");
            IdentityComparer.AreEqual(dateTimeIso8061Value, dateTime, context);

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
                ExpectedException.ArgumentException("IDX14305:", typeof(System.FormatException)).ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        // Test checks to make sure that claim values of various types can be successfully retrieved from the payload.
        [Fact]
        public void TryGetPayloadValues()
        {
            var context = new CompareContext();
            TestUtilities.WriteHeader($"{this}.TryGetPayloadValues");

            var token = new JsonWebToken("{}", jObject);

            var success = token.TryGetPayloadValue("intarray", out int[] intarray);
            IdentityComparer.AreEqual(new int[] { 1, 2, 3 }, intarray, context);
            IdentityComparer.AreEqual(true, success, context);

            success = token.TryGetPayloadValue("array", out object[] array);
            IdentityComparer.AreEqual(new object[] { 1L, "2", 3L }, array, context);
            IdentityComparer.AreEqual(true, success, context);

            // only possible internally within the library since we're using Microsoft.IdentityModel.Json.Linq.JObject
            success = token.TryGetPayloadValue("jobject", out JObject jobject);
            IdentityComparer.AreEqual(JObject.Parse(@"{ ""string1"":""string1value"", ""string2"":""string2value"" }"), jobject, context);
            IdentityComparer.AreEqual(true, success, context);

            success = token.TryGetPayloadValue("string", out string name);
            IdentityComparer.AreEqual("bob", name, context);
            IdentityComparer.AreEqual(true, success, context);

            success = token.TryGetPayloadValue("float", out float floatingPoint);
            IdentityComparer.AreEqual(42.0, floatingPoint, context);
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
            IdentityComparer.AreEqual(dateTimeIso8061Value, dateTime, context);
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
                    // Dates as strings
                    new ParseTimeValuesTheoryData
                    {
                        First = true,
                        Payload = Default.PayloadString,
                        Header = new JObject
                        {
                            { JwtHeaderParameterNames.Alg, SecurityAlgorithms.Sha512  },
                            { JwtHeaderParameterNames.Kid, Default.AsymmetricSigningKey.KeyId },
                            { JwtHeaderParameterNames.Typ, JwtConstants.HeaderType }
                        }.ToString(Formatting.None)
                    },
                    // Dates as longs
                    new ParseTimeValuesTheoryData
                    {
                        First = true,
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
                    // Dates as integers
                    new ParseTimeValuesTheoryData
                    {
                        First = true,
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

        // Test ensures that we only try to populate a JsonWebToken from a string if it is a properly formatted JWT. More specifically, we only want to try and decode
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

                JwtTestData.InvalidNumberOfSegmentsData("IDX14100:", theoryData);
                JwtTestData.ValidEncodedSegmentsData(theoryData);

                theoryData.Add(new JwtTheoryData
                {
                    TestId = nameof(EncodedJwts.InvalidHeader),
                    Token = EncodedJwts.InvalidHeader,
                    ExpectedException = ExpectedException.ArgumentException(substringExpected: "IDX14102:", inner: typeof(JsonReaderException))
                });

                theoryData.Add(new JwtTheoryData
                {
                    TestId = nameof(EncodedJwts.InvalidPayload),
                    Token = EncodedJwts.InvalidPayload,
                    ExpectedException = ExpectedException.ArgumentException(substringExpected: "IDX14101:", inner: typeof(JsonReaderException))
                });

                theoryData.Add(new JwtTheoryData
                {
                    TestId = nameof(EncodedJwts.JWSEmptyHeader),
                    Token = EncodedJwts.JWSEmptyHeader,
                    ExpectedException = ExpectedException.ArgumentException(substringExpected: "IDX14102:", inner: typeof(JsonReaderException))
                });

                theoryData.Add(new JwtTheoryData
                {
                    TestId = nameof(EncodedJwts.JWSEmptyPayload),
                    Token = EncodedJwts.JWSEmptyPayload,
                    ExpectedException = ExpectedException.ArgumentException(substringExpected: "IDX14101:", inner: typeof(JsonReaderException))
                });

                theoryData.Add(new JwtTheoryData
                {
                    TestId = nameof(EncodedJwts.JWEEmptyHeader),
                    Token = EncodedJwts.JWEEmptyHeader,
                    ExpectedException = ExpectedException.ArgumentException(substringExpected: "IDX14102:", inner: typeof(JsonReaderException))
                });

                theoryData.Add(new JwtTheoryData
                {
                    TestId = nameof(EncodedJwts.JWEEmptyEncryptedKey),
                    Token = EncodedJwts.JWEEmptyEncryptedKey,
                });

                theoryData.Add(new JwtTheoryData
                {
                    TestId = nameof(EncodedJwts.JWEEmptyIV),
                    Token = EncodedJwts.JWEEmptyIV,
                });

                theoryData.Add(new JwtTheoryData
                {
                    TestId = nameof(EncodedJwts.JWEEmptyCiphertext),
                    Token = EncodedJwts.JWEEmptyCiphertext,
                    ExpectedException = ExpectedException.ArgumentException(substringExpected: "IDX14306:")
                });

                theoryData.Add(new JwtTheoryData
                {
                    TestId = nameof(EncodedJwts.JWEEmptyAuthenticationTag),
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
    }

    public class ParseTimeValuesTheoryData : TheoryDataBase
    {
        public string Payload { get; set; }

        public string Header { get; set; }
    }
}
