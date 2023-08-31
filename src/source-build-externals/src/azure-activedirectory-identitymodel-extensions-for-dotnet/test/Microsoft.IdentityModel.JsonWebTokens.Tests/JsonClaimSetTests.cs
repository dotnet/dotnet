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

        [Theory, MemberData(nameof(ClaimSetTestCases))]
        public void ClaimSetGetValueTests(JsonClaimSetTheoryData theoryData)
        {
            CompareContext context = TestUtilities.WriteHeader($"{this}.ClaimSetTests", theoryData);
            context.IgnoreType = false;
            
            try
            {
                JsonWebToken jwt = new JsonWebToken("{}", theoryData.Json);
                JsonClaimSet jsonClaimSet = jwt.Payload;
                var methods = typeof(JsonClaimSet).GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                var method = typeof(JsonClaimSet).GetMethod("GetValue", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, CallingConventions.Standard, new Type[] { typeof(string) }, null);
                var retval = method.MakeGenericMethod(theoryData.PropertyType).Invoke(jsonClaimSet, new object[] { theoryData.PropertyName });
                theoryData.ExpectedException.ProcessNoException(context);
                IdentityComparer.AreEqual(retval, theoryData.PropertyValue, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JsonClaimSetTheoryData> ClaimSetTestCases()
        {
            var theoryData = new TheoryData<JsonClaimSetTheoryData>();

            theoryData.Add(new JsonClaimSetTheoryData("uint")
            {
                Json = $@"{{""uint"":1}}",
                PropertyName = "uint",
                PropertyType = typeof(uint),
                PropertyValue = (uint)1,
                ShouldFind = true
            });

            theoryData.Add(new JsonClaimSetTheoryData("decimal")
            {
                Json = $@"{{""decimal"":1}}",
                PropertyName = "decimal",
                PropertyType = typeof(decimal),
                PropertyValue = (decimal)1,
                ShouldFind = true
            });

            #region bool
            theoryData.Add(new JsonClaimSetTheoryData("true")
            {
                Json = $@"{{""true"":true}}",
                PropertyName = "true",
                PropertyType = typeof(bool),
                PropertyValue = true,
                ShouldFind = true
            });

            theoryData.Add(new JsonClaimSetTheoryData("false")
            {
                Json = $@"{{""false"":false}}",
                PropertyName = "false",
                PropertyType = typeof(bool),
                PropertyValue = false,
                ShouldFind = true
            });
            #endregion

            #region datetime
            DateTime dateTime = new DateTime(2000, 01, 01, 0, 0, 0);
            theoryData.Add(new JsonClaimSetTheoryData("datetimeAsString")
            {
                Json = $@"{{""datetime"":""{dateTime.ToString()}""}}",
                PropertyName = "datetime",
                PropertyType = typeof(string),
                PropertyValue = dateTime.ToString(),
                ShouldFind = true
            });

            theoryData.Add(new JsonClaimSetTheoryData("dateTimeIso8061")
            {
                Json = $@"{{""dateTimeIso8061"":""{dateTime.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture)}""}}",
                PropertyName = "dateTimeIso8061",
                PropertyType = typeof(DateTime),
                PropertyValue = dateTime.ToUniversalTime(),
                ShouldFind = true
            });
            #endregion

            #region integer arrays
            theoryData.Add(new JsonClaimSetTheoryData("IntegerAsInt")
            {
                Json = $@"{{""IntegerAsInt"":1}}",
                PropertyName = "IntegerAsInt",
                PropertyType = typeof(int),
                PropertyValue = (int)1,
                ShouldFind = true
            });

            theoryData.Add(new JsonClaimSetTheoryData("IntegerAsIntArray")
            {
                Json = $@"{{""IntegerAsIntArray"":1}}",
                PropertyName = "IntegerAsIntArray",
                PropertyType = typeof(int[]),
                PropertyValue = new int[] { 1 },
                ShouldFind = true
            });

            theoryData.Add(new JsonClaimSetTheoryData("IntegersAsIntArray")
            {
                Json = $@"{{""IntegersAsIntArray"":[1,2,3]}}",
                PropertyName = "IntegersAsIntArray",
                PropertyType = typeof(int[]),
                PropertyValue = new int[] { 1, 2, 3 },
                ShouldFind = true
            });

            theoryData.Add(new JsonClaimSetTheoryData("IntegersAsObjArray")
            {
                Json = $@"{{""IntegersAsObjArray"":[1,2,3]}}",
                PropertyName = "IntegersAsObjArray",
                PropertyType = typeof(object[]),
                PropertyValue = new object[] { (int)1, (int)2, (int)3 },
                ShouldFind = true
            });

            theoryData.Add(new JsonClaimSetTheoryData("IntegersAsObj")
            {
                Json = $@"{{""IntegersAsObj"":[1,2,3]}}",
                PropertyName = "IntegersAsObj",
                PropertyType = typeof(object[]),
                PropertyValue = new object[] { (int)1, (int)2, (int)3 },
                ShouldFind = true
            });

            #endregion

            #region long
            theoryData.Add(new JsonClaimSetTheoryData("long")
            {
                Json = $@"{{""long"":1}}",
                PropertyName = "long",
                PropertyType = typeof(long),
                PropertyValue = (long)1,
                ShouldFind = true
            });

            theoryData.Add(new JsonClaimSetTheoryData("long[]")
            {
                Json = $@"{{""long[]"":1}}",
                PropertyName = "long[]",
                PropertyType = typeof(long[]),
                PropertyValue = new long[] { 1 },
                ShouldFind = true
            });
            #endregion

            #region mixed arrays
            theoryData.Add(new JsonClaimSetTheoryData("MixedArrayAsObjArray")
            {
                Json = $@"{{""MixedArrayAsObjArray"":[1,""2"",3]}}",
                PropertyName = "MixedArrayAsObjArray",
                PropertyType = typeof(object[]),
                PropertyValue = new object[] { (int)1, "2", (int)3},
                ShouldFind = true
            });
            #endregion

            #region strings
            theoryData.Add(new JsonClaimSetTheoryData("string")
            {
                Json = $@"{{""string"":""bob""}}",
                PropertyName = "string",
                PropertyType = typeof(string),
                PropertyValue = "bob",
                ShouldFind = true
            });
            #endregion

            #region null
            theoryData.Add(new JsonClaimSetTheoryData("nill")
            {
                Json = $@"{{""nill"":null}}",
                PropertyName = "nill",
                PropertyType = typeof(object),
                PropertyValue = null,
                ShouldFind = true
            });
            #endregion

            #region numbers
            theoryData.Add(new JsonClaimSetTheoryData("int")
            {
                Json = $@"{{""int"":42}}",
                PropertyName = "int",
                PropertyType = typeof(int),
                PropertyValue = 42,
                ShouldFind = true
            });

            theoryData.Add(new JsonClaimSetTheoryData("float")
            {
                Json = $@"{{""float"":42.0}}",
                PropertyName = "float",
                PropertyType = typeof(float),
                PropertyValue = (float)42.0,
                ShouldFind = true
            });

            theoryData.Add(new JsonClaimSetTheoryData("double")
            {
                Json = $@"{{""double"":42.0}}",
                PropertyName = "double",
                PropertyType = typeof(double),
                PropertyValue = (double)42.0,
                ShouldFind = true
            });
            #endregion

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

        // Test checks to make sure that claim values of various types can be successfully retrieved from the payload.
        [Fact]
        public void TryGetPayloadValues()
        {
            var context = new CompareContext();
            TestUtilities.WriteHeader($"{this}.TryGetPayloadValues");

            var token = new JsonWebToken("{}", _jsonPayload);

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
            IdentityComparer.AreEqual((float)42.0, floatingPoint, context);
            IdentityComparer.AreEqual(true, success, context);

            success = token.TryGetPayloadValue("integer", out int integer);
            IdentityComparer.AreEqual(42, integer, context);
            IdentityComparer.AreEqual(true, success, context);

            success = token.TryGetPayloadValue("nill", out bool? bb);
            if (bb != null)
                context.Diffs.Add("bb != null");

            if (!success)
                context.Diffs.Add("bb (success) != true");

            success = token.TryGetPayloadValue("nill", out ClaimsIdentity ci);
            if (ci != null)
                context.AddDiff("ClaimsIdentity != null");

            if (!success)
                context.AddDiff("ClaimsIdentity not successful");

            success = token.TryGetPayloadValue("nill", out object nill);
            IdentityComparer.AreEqual(nill, null, context);
            IdentityComparer.AreEqual(true, success, context);

            success = token.TryGetPayloadValue("bool", out bool boolean);
            IdentityComparer.AreEqual(boolean, true, context);
            IdentityComparer.AreEqual(true, success, context);

            var dateTimeValue = token.GetPayloadValue<string>("dateTime");
            IdentityComparer.AreEqual(dateTimeValue, _dateTime.ToString(), context);
            IdentityComparer.AreEqual(true, success, context);

            var dateTimeIso8061Value = token.GetPayloadValue<DateTime>("dateTimeIso8061");
            IdentityComparer.AreEqual(dateTimeIso8061Value, _dateTime.ToUniversalTime(), context);
            IdentityComparer.AreEqual(true, success, context);

            success = token.TryGetPayloadValue("doesnotexist", out int doesNotExist);
            IdentityComparer.AreEqual(0, doesNotExist, context);
            IdentityComparer.AreEqual(false, success, context);

            success = token.TryGetPayloadValue("string", out int cannotConvert);
            IdentityComparer.AreEqual(0, cannotConvert, context);
            IdentityComparer.AreEqual(false, success, context);

            TestUtilities.AssertFailIfErrors(context);
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
