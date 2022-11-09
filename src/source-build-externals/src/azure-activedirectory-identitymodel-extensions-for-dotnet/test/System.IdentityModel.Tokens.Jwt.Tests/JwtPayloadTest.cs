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

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using Microsoft.IdentityModel.Json;
using Microsoft.IdentityModel.Json.Linq;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace System.IdentityModel.Tokens.Jwt.Tests
{
    public class JwtPayloadTests
    {
        /// <summary>
        /// Test Context Wrapper instance on top of TestContext. Provides better accessor functions
        /// </summary>
        [Fact]
        public void Defaults()
        {
            JwtPayload jwtPayload = new JwtPayload();
            List<Claim> claims = jwtPayload.Claims as List<Claim>;
            Assert.True(claims != null, "claims as List<Claim> == null");

            foreach (Claim c in jwtPayload.Claims)
            {
                Assert.True(false, "jwtPayload.Claims should be empty");
            }

            Assert.True(jwtPayload.Aud != null, "jwtPayload.Aud should not be null");
            foreach (string audience in jwtPayload.Aud)
            {
                Assert.True(false, "jwtPayload.Aud should be empty");
            }

            Assert.True(jwtPayload.Amr != null, "jwtPayload.Amr should not be null");
            foreach (string audience in jwtPayload.Amr)
            {
                Assert.True(false, "jwtPayload.Amr should be empty");
            }

            Assert.True(jwtPayload.ValidFrom == DateTime.MinValue, "jwtPayload.ValidFrom != DateTime.MinValue");
            Assert.True(jwtPayload.ValidTo == DateTime.MinValue, "jwtPayload.ValidTo != DateTime.MinValue");
            Assert.True(jwtPayload.IssuedAt == DateTime.MinValue, "jwtPayload.ValidFrom != DateTime.MinValue");
        }

        [Fact]
        public void GetSets()
        {
            // Aud, Claims, ValidFrom, ValidTo handled in Defaults.

            JwtPayload jwtPayload = new JwtPayload();
            Type type = typeof(JwtPayload);
            PropertyInfo[] properties = type.GetProperties();
            if (properties.Length != 23)
                Assert.True(false, "Number of properties has changed from 23 to: " + properties.Length + ", adjust tests");

            GetSetContext context =
                new GetSetContext
                {
                    PropertyNamesAndSetGetValue = new List<KeyValuePair<string, List<object>>>
                    {
                        new KeyValuePair<string, List<object>>("Actort", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("Acr", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("AuthTime", new List<object>{(string)null, 10, 12 }),
                        new KeyValuePair<string, List<object>>("Azp", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("CHash", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("Exp", new List<object>{(string)null, 1, 0 }),
                        new KeyValuePair<string, List<object>>("Jti", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("Iat", new List<object>{(string)null, 10, 0}),
                        new KeyValuePair<string, List<object>>("Iss", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("Nbf", new List<object>{(string)null, 1, 0 }),
                        new KeyValuePair<string, List<object>>("Nonce", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("Sub", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                    },
                    Object = jwtPayload,
                };
            TestUtilities.GetSet(context);
            TestUtilities.AssertFailIfErrors("JwtPayload_GetSets", context.Errors);
        }

        [Fact]
        public void JwtPayloadUnicodeMapping()
        {
            string issuer = "a\\b";
            List<Claim> claims = new List<Claim>();
            JwtPayload unicodePayload = new JwtPayload("a\u005Cb", "", claims, null, null);
            string json = unicodePayload.SerializeToJson();
            JwtPayload payload = new JwtPayload(issuer, "", claims, null, null);
            string json2 = payload.SerializeToJson();
            Assert.Equal(json, json2);

            JwtPayload retrievePayload = JwtPayload.Deserialize(json);
            Assert.Equal(retrievePayload.Iss, issuer);

            json = unicodePayload.Base64UrlEncode();
            json2 = payload.Base64UrlEncode();
            Assert.Equal(json, json2);

            retrievePayload = JwtPayload.Base64UrlDeserialize(json);
            Assert.Equal(retrievePayload.Iss, issuer);
        }

        [Fact]
        public void JwtPayloadEncoding()
        {
            var context = new CompareContext
            {
                PropertiesToIgnoreWhenComparing = new Dictionary<Type, List<string>>
                {
                    { typeof(JwtPayload), new List<string> { "Item" } },
                }
            };

            RunEncodingVariation(JwtPayloadTestData.ClaimForEachProperty, JwtPayloadTestData.ObjectForEachProperty, context);
            RunEncodingVariation(JwtPayloadTestData.Multiples.Key, JwtPayloadTestData.Multiples.Value, context);

            TestUtilities.AssertFailIfErrors(context.Diffs);
        }

        [Fact]
        public void FirstClassProperties()
        {
            var context = new CompareContext();

            JwtPayload jwtPayload = new JwtPayload();
            int? time = 10000;
            jwtPayload.Add("exp", time);
            DateTime payloadTime = EpochTime.DateTime(time.Value);
            DateTime payloadValidTo = jwtPayload.ValidTo;

            Assert.True(EpochTime.DateTime(time.Value) == jwtPayload.ValidTo, "EpochTime.DateTime( time ) != jwtPayload.ValidTo");

            int? expirationTime = jwtPayload.Exp;
            Assert.True(expirationTime == time, "expirationTime != time");

            TestUtilities.AssertFailIfErrors(GetType().ToString() + ".Claims", context.Diffs);
        }

        [Fact]
        public void TestClaimWithNullValue()
        {
            JwtPayload payload = new JwtPayload();
            string compareTo = "{\"nullClaim\":null}";
            payload.Add("nullClaim", null);
            bool claimFound = false;
            foreach (Claim claim in payload.Claims)
            {
                if (claim.Type == "nullClaim")
                    claimFound = true;
            }

            if (!claimFound)
                Assert.True(false, "Claim with expected type: nullClaim is not found");

            Assert.Equal(payload.SerializeToJson(), compareTo);
        }

        [Fact]
        public void TestDateTimeClaim()
        {
            JwtPayload jwtPayload = new JwtPayload();
            var dateTime = new DateTime(2020, 1, 1, 1, 1, 1, 1);
            jwtPayload.Add("dateTime", dateTime);
            var dateTimeClaim = jwtPayload.Claims.First();

            Assert.True(string.Equals(dateTimeClaim.ValueType, ClaimValueTypes.DateTime), "dateTimeClaim.Type != ClaimValueTypes.DateTime");
            Assert.True(string.Equals(dateTimeClaim.Value, dateTime.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture)), "dateTimeClaim.Value != dateTime.ToUniversalTime('o', CultureInfo.InvariantCulture).ToString()");
        }

        [Fact]
        public void TestClaimWithLargeExpValue()
        {
            JwtPayload jwtPayload = new JwtPayload();
            jwtPayload.Add("exp", 1507680819080);
            DateTime expirationTime = jwtPayload.ValidTo;
            Assert.True(DateTime.MaxValue == expirationTime, "EpochTime.DateTime( time ) != jwtPayload.ValidTo");
        }

        [Theory, MemberData(nameof(PayloadDataSet))]
        public void RoundTrip(List<Claim> claims, JwtPayload payloadSetDirect, JwtPayload payloadSetUsingDeserialize)
        {
            var context = new CompareContext();
            var payload = new JwtPayload(claims);
            var encodedPayload = payload.SerializeToJson();
            var payloadDeserialized = JwtPayload.Deserialize(encodedPayload);
            var instanceContext = new CompareContext
            {
                PropertiesToIgnoreWhenComparing = new Dictionary<Type, List<string>>
                {
                    { typeof(JwtPayload), new List<string> { "Item" } },
                }
            };

            IdentityComparer.AreEqual(payload, payloadDeserialized, instanceContext);
            context.Merge(string.Format(CultureInfo.InvariantCulture, "AreEqual({0}, {1})", nameof(payload), nameof(payloadDeserialized)), instanceContext);

            instanceContext.Diffs.Clear();
            IdentityComparer.AreEqual(payload, payloadSetDirect, instanceContext);
            context.Merge(string.Format(CultureInfo.InvariantCulture, "AreEqual({0}, {1})", nameof(payload), nameof(payloadSetDirect)), instanceContext);

            instanceContext.Diffs.Clear();
            IdentityComparer.AreEqual(payload, payloadSetUsingDeserialize, instanceContext);
            context.Merge(string.Format(CultureInfo.InvariantCulture, "AreEqual({0}, {1})", nameof(payload), nameof(payloadSetUsingDeserialize)), instanceContext);

            instanceContext.Diffs.Clear();
            IdentityComparer.AreEqual(payload.Claims, claims, instanceContext);
            context.Merge(string.Format(CultureInfo.InvariantCulture, "AreEqual({0}, {1})", nameof(payload.Claims), nameof(claims)), instanceContext);

            instanceContext.Diffs.Clear();
            CheckClaimsTypeParsing(payload.Claims, instanceContext);
            context.Merge(string.Format(CultureInfo.InvariantCulture, "CheckClaimsTypeParsing({0})", nameof(payload.Claims)), instanceContext);

            instanceContext.Diffs.Clear();
            CheckClaimsTypeParsing(payloadDeserialized.Claims, instanceContext);
            context.Merge(string.Format(CultureInfo.InvariantCulture, "CheckClaimsTypeParsing({0})", nameof(payloadDeserialized.Claims)), instanceContext);

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<List<Claim>, JwtPayload, JwtPayload> PayloadDataSet
        {
            get
            {
                var intMaxValue = int.MaxValue.ToString();
                var intMinValue = int.MinValue.ToString();
                var longMaxValue = long.MaxValue.ToString();
                var longMinValue = long.MinValue.ToString();
                var longValue = ((long)int.MaxValue + 100).ToString();

                var dataset = new TheoryData<List<Claim>, JwtPayload, JwtPayload>();
                SetDataSet(
                    new List<Claim>
                    {
                        new Claim("ClaimValueTypes.String", "ClaimValueTypes.String.Value", ClaimValueTypes.String),
                        new Claim("ClaimValueTypes.Boolean.true", "true", ClaimValueTypes.Boolean),
                        new Claim("ClaimValueTypes.Boolean.false", "false", ClaimValueTypes.Boolean),
                        new Claim("ClaimValueTypes.Double", "123.4", ClaimValueTypes.Double),
                        new Claim("ClaimValueTypes.int.MaxValue", intMaxValue, ClaimValueTypes.Integer),
                        new Claim("ClaimValueTypes.int.MinValue", intMinValue, ClaimValueTypes.Integer),
                        new Claim("ClaimValueTypes.long.MaxValue", longMaxValue, ClaimValueTypes.Integer64),
                        new Claim("ClaimValueTypes.long.MinValue", longMinValue, ClaimValueTypes.Integer64),
                        new Claim("ClaimValueTypes.DateTime.IS8061", "2019-11-15T14:31:21.6101326Z", ClaimValueTypes.DateTime),
                        new Claim("ClaimValueTypes.DateTime", "2019-11-15", ClaimValueTypes.String),
                        new Claim("ClaimValueTypes.JsonClaimValueTypes.Json1", @"{""jsonProperty1"":""jsonvalue1""}", JsonClaimValueTypes.Json),
                        new Claim("ClaimValueTypes.JsonClaimValueTypes.Json2", @"{""jsonProperty2"":""jsonvalue2""}", JsonClaimValueTypes.Json),
                        new Claim("ClaimValueTypes.JsonClaimValueTypes.JsonArray", "1", ClaimValueTypes.Integer),
                        new Claim("ClaimValueTypes.JsonClaimValueTypes.JsonArray", "2", ClaimValueTypes.Integer),
                    },
                    dataset);

                SetDataSet(
                    new List<Claim>
                    {
                        new Claim("aud", "http://test.local/api/", ClaimValueTypes.String, "http://test.local/api/"),
                        new Claim("exp", "1460647835", ClaimValueTypes.Integer, "http://test.local/api/"),
                        new Claim("emailaddress", "user1@contoso.com", ClaimValueTypes.String, "http://test.local/api/"),
                        new Claim("emailaddress", "user2@contoso.com", ClaimValueTypes.String, "http://test.local/api/"),
                        new Claim("name", "user", ClaimValueTypes.String, "http://test.local/api/"),
                        new Claim("dateTime", "2019-11-15T14:31:21.6101326Z", ClaimValueTypes.DateTime, "http://test.local/api/"),
                        new Claim("iss", "http://test.local/api/", ClaimValueTypes.String, "http://test.local/api/")
                    },
                    dataset);

                SetDataSet(
                    new List<Claim>
                    {
                        new Claim("ClaimValueTypes", "0", ClaimValueTypes.Integer),
                        new Claim("ClaimValueTypes", "100", ClaimValueTypes.Integer),
                        new Claim("ClaimValueTypes", "132", ClaimValueTypes.Integer),
                        new Claim("ClaimValueTypes", "164", ClaimValueTypes.Integer),
                        new Claim("ClaimValueTypes", "-100", ClaimValueTypes.Integer),
                        new Claim("ClaimValueTypes", "-132", ClaimValueTypes.Integer),
                        new Claim("ClaimValueTypes", "-164", ClaimValueTypes.Integer),
                        new Claim("ClaimValueTypes", longValue, ClaimValueTypes.Integer64),
                        new Claim("ClaimValueTypes", "132.64", ClaimValueTypes.Double),
                        new Claim("ClaimValueTypes", "-132.64", ClaimValueTypes.Double),
                        new Claim("ClaimValueTypes", "true", ClaimValueTypes.Boolean),
                        new Claim("ClaimValueTypes", "false", ClaimValueTypes.Boolean),
                        new Claim("ClaimValueTypes", "2019-11-15T14:31:21.6101326Z", ClaimValueTypes.DateTime),
                        new Claim("ClaimValueTypes", "2019-11-15", ClaimValueTypes.String),
                        new Claim("ClaimValueTypes", @"{""name3.1"":""value3.1""}", JsonClaimValueTypes.Json),
                        new Claim("ClaimValueTypes", @"[""status"",""feed""]", JsonClaimValueTypes.JsonArray),
                    },
                    dataset);

                SetDataSet(
                    new List<Claim>
                    {
                        new Claim("json3", @"{""name3.1"":""value3.1""}", JsonClaimValueTypes.Json),
                        new Claim("json3", @"{""name3.2"":""value3.2""}", JsonClaimValueTypes.Json),
                        new Claim("json3", @"{""dateTimeIso8061"":""2019-11-15T14:31:21.6101326Z""}", JsonClaimValueTypes.Json),
                        new Claim("json3", @"{""dateTime"":""2019-11-15""}", JsonClaimValueTypes.Json),
                        new Claim("json3", @"{""name3.3"":[1,2,3]}", JsonClaimValueTypes.Json),
                        new Claim("json3", "name3.4"),
                        new Claim("may_act",  @"{""sub"":""admin@example.net"",""name"":""Admin""}", JsonClaimValueTypes.Json),
                        new Claim("may_act",  @"{""sub"":""admin@example.net"",""name"":""Admin""}", JsonClaimValueTypes.Json),
                        new Claim("may_act2", @"{""sub"":""admin@example.net"",""name"":""Admin""}", JsonClaimValueTypes.Json)
                    },
                    dataset);

                return dataset;
            }
        }

        private static void SetDataSet(List<Claim> claims, TheoryData<List<Claim>, JwtPayload, JwtPayload> dataset)
        {
            var payloadDirect = new JwtPayload();
            var jobj = new JObject();
            foreach (var claim in claims)
            {
                object jsonValue = null;
                object existingValue;
                switch (claim.ValueType)
                {
                    case ClaimValueTypes.String:
                        jsonValue = claim.Value;
                        break;

                    case ClaimValueTypes.Boolean:
                        jsonValue = bool.Parse(claim.Value);
                        break;

                    case ClaimValueTypes.Double:
                        jsonValue = double.Parse(claim.Value, CultureInfo.InvariantCulture);
                        break;

                    case ClaimValueTypes.Integer:
                    case ClaimValueTypes.Integer32:
                        jsonValue = int.Parse(claim.Value, CultureInfo.InvariantCulture);
                        break;

                    case ClaimValueTypes.Integer64:
                        jsonValue = long.Parse(claim.Value, CultureInfo.InvariantCulture);
                        break;

                    case ClaimValueTypes.DateTime:
                        jsonValue = DateTime.Parse(claim.Value);
                        break;

                    case JsonClaimValueTypes.Json:
                        jsonValue = JObject.Parse(claim.Value);
                        break;

                    case JsonClaimValueTypes.JsonArray:
                        jsonValue = JArray.Parse(claim.Value);
                        break;
                }

                JToken jtoken = null;
                if (jobj.TryGetValue(claim.Type, out jtoken))
                {
                    JArray jarray = jtoken as JArray;
                    if (jarray == null)
                    {
                        jarray = new JArray();
                        jarray.Add(jtoken);
                        jobj.Remove(claim.Type);
                        jobj.Add(claim.Type, jarray);
                    }

                    jarray.Add(JToken.FromObject(jsonValue));
                }
                else
                {
                    jobj.Add(claim.Type, JToken.FromObject(jsonValue));
                }

                if (payloadDirect.TryGetValue(claim.Type, out existingValue))
                {
                    IList<object> claimValues = existingValue as IList<object>;
                    if (claimValues == null)
                    {
                        claimValues = new List<object>();
                        claimValues.Add(existingValue);
                        payloadDirect[claim.Type] = claimValues;
                    }

                    claimValues.Add(jsonValue);
                }
                else
                {
                    payloadDirect[claim.Type] = jsonValue;
                }
            }

            var j = jobj.ToString(Formatting.None);
            var payloadDeserialized = JwtPayload.Deserialize(j);
            dataset.Add(claims, payloadDirect, payloadDeserialized);
        }

        private void CheckClaimsTypeParsing(IEnumerable<Claim> claims, CompareContext context)
        {
            if (claims == null)
                return;

            foreach (var claim in claims)
            {
                switch (claim.ValueType)
                {
                    case ClaimValueTypes.Boolean:
                        bool boolRet;
                        if (!bool.TryParse(claim.Value, out boolRet))
                            context.Diffs.Add(string.Format(CultureInfo.InvariantCulture, "bool.TryParse(claim.Value, out boolRet), value: '{0}'", claim.Value));

                        break;

                    case ClaimValueTypes.Double:
                        double doubleRet;
                        if (!double.TryParse(claim.Value, out doubleRet))
                            context.Diffs.Add(string.Format(CultureInfo.InvariantCulture, "double.TryParse(claim.Value, out doubleRet), value: '{0}'", claim.Value));

                        break;

                    case ClaimValueTypes.Integer:
                        int intRet;
                        if (!int.TryParse(claim.Value, out intRet))
                            context.Diffs.Add(string.Format(CultureInfo.InvariantCulture, "int.TryParse(claim.Value, out intRet), value: '{0}'", claim.Value));

                        break;

                    case ClaimValueTypes.Integer32:
                        int intRet32;
                        if (!int.TryParse(claim.Value, out intRet32))
                            context.Diffs.Add(string.Format(CultureInfo.InvariantCulture, "int.TryParse(claim.Value, out intRet32), value: '{0}'", claim.Value));

                        break;

                    case ClaimValueTypes.Integer64:
                        long long64;
                        if (!long.TryParse(claim.Value, out long64))
                            context.Diffs.Add(string.Format(CultureInfo.InvariantCulture, "long.TryParse(claim.Value, out long64), value: '{0}'", claim.Value));

                        break;

                    case ClaimValueTypes.DateTime:
                        DateTime dateTime;
                        if (!DateTime.TryParse(claim.Value, out dateTime))
                            context.Diffs.Add(string.Format(CultureInfo.InvariantCulture, "DateTime.TryParse(claim.Value, out dateTime), value: '{0}'", claim.Value));

                        break;

                    case JsonClaimValueTypes.Json:
                        try
                        {
                            JObject.Parse(claim.Value);
                        }
                        catch (Exception ex)
                        {
                            context.Diffs.Add(string.Format(CultureInfo.InvariantCulture, "JObject.Parse(claim.Value) threw: '{0}', value: '{1}'", ex, claim.Value));
                        }

                        break;

                    case JsonClaimValueTypes.JsonArray:
                        try
                        {
                            JArray.Parse(claim.Value);
                        }
                        catch (Exception ex)
                        {
                            context.Diffs.Add(string.Format(CultureInfo.InvariantCulture, "JArray.Parse(claim.Value) threw: '{0}', value: '{1}'", ex, claim.Value));
                        }
                        break;
                }
            }
        }

        private void RunEncodingVariation(List<Claim> claims, Dictionary<string, object> values, CompareContext context)
        {
            var jwtPayload1 = new JwtPayload(claims);
            var jwtPayload2 = new JwtPayload();
            foreach (var kv in values)
            {
                jwtPayload2[kv.Key] = kv.Value;
            }

            IdentityComparer.AreEqual(jwtPayload1, jwtPayload2, context);

            jwtPayload1 = JwtPayload.Base64UrlDeserialize(jwtPayload1.Base64UrlEncode());
            jwtPayload2 = JwtPayload.Base64UrlDeserialize(jwtPayload2.Base64UrlEncode());
            IdentityComparer.AreEqual(jwtPayload1, jwtPayload2, context);
        }

        // Test checks to make sure that GetStandardClaim() returns null (not "null") if the value associated with the claimType parameter is null.
        [Fact]
        public void GetStandardClaimNull()
        {
            var jwtPayload = new JwtPayload();
            jwtPayload[JwtRegisteredClaimNames.Iss] = null;
            var issuer = jwtPayload.Iss;
            Assert.True(issuer == null);
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
