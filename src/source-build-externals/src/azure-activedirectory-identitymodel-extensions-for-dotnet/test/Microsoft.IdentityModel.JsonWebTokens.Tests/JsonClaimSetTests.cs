// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Microsoft.IdentityModel.JsonWebTokens.Tests
{
    public class JsonClaimSetTests
    {
        private static DateTime _dateTime = new DateTime(2000, 01, 01, 0, 0, 0);
        private static string _dateTimePropertyName = "dateTime";
        private static string _jsonPayload = $@"{{""intarray"":[1,2,3], ""array"":[1,""2"",3], ""jobject"": {{""string1"":""string1value"",""string2"":""string2value""}},""string"":""bob"", ""float"":42.0, ""integer"":42, ""nill"": null, ""bool"" : true, ""{_dateTimePropertyName}"": ""{_dateTime}"", ""dateTimeIso8061"": ""{_dateTime.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture)}"" }}";
        private List<Claim> _payloadClaims = new List<Claim>()
        {
            new Claim("intarray", @"[1,2,3]", JsonClaimValueTypes.JsonArray, "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
            new Claim("array", @"[1,""2"",3]", JsonClaimValueTypes.JsonArray, "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
            new Claim("jobject", @"{""string1"":""string1value"",""string2"":""string2value""}", JsonClaimValueTypes.Json, "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
            new Claim("string", "bob", ClaimValueTypes.String, "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
            new Claim("float", "42.0", ClaimValueTypes.Double, "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
            new Claim("integer", "42", ClaimValueTypes.Integer, "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
            new Claim("nill", "", JsonClaimValueTypes.JsonNull, "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
            new Claim("bool", "true", ClaimValueTypes.Boolean, "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
            new Claim("dateTime", _dateTime.ToString(), ClaimValueTypes.String, "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
            new Claim("dateTimeIso8061", _dateTime.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture), ClaimValueTypes.DateTime, "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
        };

        [Theory, MemberData(nameof(DirectClaimSetTestCases))]
        public void DirectClaimSetTests(JsonClaimSetTheoryData theoryData)
        {
            CompareContext context = TestUtilities.WriteHeader($"{this}.ClaimSetTests", theoryData);
            context.IgnoreType = false;

            try
            {
                JsonWebToken jwt = new JsonWebToken("{}", $@"{{""true"":true}}");
                JsonClaimSet claimSet = jwt.CreatePayloadClaimSet(theoryData.Utf8Bytes, theoryData.Utf8Bytes.Length);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (JsonException ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JsonClaimSetTheoryData> DirectClaimSetTestCases()
        {
            var theoryData = new TheoryData<JsonClaimSetTheoryData>();
            theoryData.Add(new JsonClaimSetTheoryData("NotOnStartObject")
            {
                ExpectedException = new ExpectedException(typeof(JsonException)),
                Utf8Bytes = Encoding.UTF8.GetBytes($@"[""a""]")
            });

            // ignore exception as a System.Text.Json.JsonReaderException is thrown
            // which is internal to System.Text.Json so we can't define it.
            theoryData.Add(new JsonClaimSetTheoryData("badJson")
            {
                ExpectedException = new ExpectedException(typeof(JsonException)) { IgnoreExceptionType = true },
                Utf8Bytes = Encoding.UTF8.GetBytes("badJson")
            });

            return theoryData;
        }

        [Theory, MemberData(nameof(GetClaimAsTypeTheoryData))]
        public void GetClaimAsType(JsonClaimSetTheoryData theoryData)
        {
            CompareContext context = TestUtilities.WriteHeader($"{this}.GetClaimAsType", theoryData);
            try
            {
                JsonWebToken token = new JsonWebToken(theoryData.Json);

                var methods = typeof(JsonWebToken).GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                var method = typeof(JsonWebToken).GetMethod("GetPayloadValue", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, CallingConventions.Standard, new Type[] { typeof(string) }, null);
                var retval = method.MakeGenericMethod(theoryData.PropertyType).Invoke(token, new object[] { theoryData.PropertyName });
                theoryData.ExpectedException.ProcessNoException(context);
                IdentityComparer.AreEqual(retval, theoryData.PropertyValue, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JsonClaimSetTheoryData> GetClaimAsTypeTheoryData()
        {
            var theoryData = new TheoryData<JsonClaimSetTheoryData>();

            string header = Base64UrlEncoder.Encode("{}");
            string payload = Base64UrlEncoder.Encode(@"{""a"":{""prop1"":""value1""},""b"":{""prop1"":[""value1"",""value2""]}, ""exp"": 1692706803,""iat"": 1692703203,""nbf"": 1692703203}");

            theoryData.Add(
                new JsonClaimSetTheoryData("DictionaryWithListOfStrings")
                {
                    Json = header + "." + payload + ".",
                    PropertyName = "b",
                    PropertyType = typeof(Dictionary<string, List<string>>),
                    PropertyValue = new Dictionary<string, List<string>> { { "prop1", new List<string> { "value1", "value2" } } }
                });

            theoryData.Add(
                new JsonClaimSetTheoryData("DictionaryWithArrayOfStrings")
                {
                    Json = header + "." + payload + ".",
                    PropertyName = "b",
                    PropertyType = typeof(Dictionary<string, string[]>),
                    PropertyValue = new Dictionary<string, string[]> {{"prop1", new string[]{"value1","value2"}}}
                });

            theoryData.Add(
                new JsonClaimSetTheoryData("DictionaryOfStrings")
                {
                    Json = header + "." + payload + ".",
                    PropertyName = "a",
                    PropertyType = typeof(Dictionary<string, string>),
                    PropertyValue = new Dictionary<string, string> {{"prop1","value1"}}
                });


            return theoryData;
        }

        public class JsonClaimSetTheoryData : TheoryDataBase
        {
            public JsonClaimSetTheoryData(string id) : base(id) { }

            public string Json { get; set; }

            public JsonWebToken JsonWebToken { get; set; }

            public string PropertyName { get; set; }

            public Type PropertyOut { get; set; }

             public Type PropertyType { get; set; }

            public object PropertyValue { get; set; }

            public bool ShouldFind { get; set; }

            public byte[] Utf8Bytes { get; set; }
        }
    }
}
