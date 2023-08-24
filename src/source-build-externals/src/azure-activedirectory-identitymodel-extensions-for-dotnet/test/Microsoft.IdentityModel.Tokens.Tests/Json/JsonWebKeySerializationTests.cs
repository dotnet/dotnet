// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Tests;
using Newtonsoft.Json;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Json.Tests
{
    /// <summary>
    /// These set of tests ensure that 7x and 6x serialization of JsonWebKey.
    /// 6x uses Newtonsoft, 7x uses System.Text.Json utf8Readers and utf8Writers.
    /// Differences will be discovered here and used for release notes.
    /// </summary>
    public class JsonWebKeySerializationTests
    {
        /// <summary>
        /// This test is designed to ensure that JsonDeserialize and Utf8Reader are consistent w.r.t. exceptions.
        /// </summary>
        /// <param name="theoryData"></param>
        [Theory, MemberData(nameof(DeserializeTheoryData))]
        public void DeserializeExceptions(JsonSerializerTheoryData theoryData)
        {
            var context = new CompareContext(theoryData);
            try
            {
                JsonWebKeySerializer.Read(theoryData.Json);
                theoryData.JsonReaderExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.JsonReaderExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JsonSerializerTheoryData> DeserializeTheoryData
        {
            get
            {
                var theoryData = new TheoryData<JsonSerializerTheoryData>();
                AddSingleStringTestCases(theoryData, "Alg", JsonWebKeyParameterNames.Alg);
                AddArrayOfStringsTestCases(theoryData, "KeyOps", JsonWebKeyParameterNames.KeyOps);
                return theoryData;
            }
        }

        /// <summary>
        /// Adds tests cases for a type with the property name of the class and the json property name.
        /// </summary>
        /// <param name="theoryData">place to add the test case.</param>
        /// <param name="instancePropertyName">the property name on the class.</param>
        /// <param name="jsonPropertyName">the property name in the json mapping to the class</param>
        private static void AddSingleStringTestCases(TheoryData<JsonSerializerTheoryData> theoryData, string instancePropertyName, string jsonPropertyName)
        {
            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_DuplicateProperties")
            {
                Json = $@"{{""{jsonPropertyName}"":""string"", ""{jsonPropertyName}"":""string""}}",
            });

            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_SingleString")
            {
                Json = $@"{{""{jsonPropertyName}"":""string""}}",
            });

            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_ArrayString")
            {
                Json = $@"{{""{jsonPropertyName}"":[""string1"", ""string2""]}}",
                JsonReaderExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11020: "),
                JsonSerializerExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted", typeof(InvalidOperationException))
            });

            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_Array")
            {
                Json = $@"{{""{jsonPropertyName}"":[""value"", 1]}}",
                JsonReaderExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11020: "),
                JsonSerializerExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted", typeof(InvalidOperationException))
            });

            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_true")
            {
                Json = $@"{{""{jsonPropertyName}"": true}}",
                JsonReaderExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11020: "),
                JsonSerializerExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted", typeof(InvalidOperationException))
            });

            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_false")
            {
                Json = $@"{{""{jsonPropertyName}"": false}}",
                JsonReaderExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11020: "),
                JsonSerializerExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted", typeof(InvalidOperationException))
            });

            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_Object")
            {
                Json = $@"{{""{jsonPropertyName}"":{{""property"": ""false""}}}}",
                JsonReaderExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11020: "),
                JsonSerializerExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted", typeof(InvalidOperationException))
            });

            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_Null")
            {
                Json = $@"{{""{jsonPropertyName}"":null}}",
            });

            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_Number")
            {
                Json = $@"{{""d"":""string"",""d"":""string"",""{jsonPropertyName}"":1}}",
                JsonReaderExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11020: "),
                JsonSerializerExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted", typeof(InvalidOperationException))
            });
        }

        /// <summary>
        /// Adds tests cases for a type with the property name of the class and the json property name.
        /// </summary>
        /// <param name="theoryData">place to add the test case.</param>
        /// <param name="instancePropertyName">the property name on the class.</param>
        /// <param name="jsonPropertyName">the property name in the json mapping to the class</param>
        private static void AddArrayOfStringsTestCases(TheoryData<JsonSerializerTheoryData> theoryData, string instancePropertyName, string jsonPropertyName)
        {
            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_SingleString")
            {
                Json = $@"{{""{jsonPropertyName}"":""string""}}",
                JsonReaderExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11022:"),
                JsonSerializerExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted")
            });

            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_ArrayString")
            {
                Json = $@"{{""{jsonPropertyName}"":[""string1"", ""string2""]}}",
            });

            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_Array")
            {
                Json = $@"{{""{jsonPropertyName}"":[""value"", 1]}}",
                JsonReaderExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11020: "),
                JsonSerializerExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted", typeof(InvalidOperationException))
            });

            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_true")
            {
                Json = $@"{{""{jsonPropertyName}"": true}}",
                JsonReaderExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11022: "),
                JsonSerializerExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted")
            });

            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_false")
            {
                Json = $@"{{""{jsonPropertyName}"": false}}",
                JsonReaderExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11022: "),
                JsonSerializerExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted")
            });

            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_Object")
            {
                Json = $@"{{""{jsonPropertyName}"":{{""property"": ""false""}}}}",
                JsonReaderExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11022: "),
                JsonSerializerExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted")
            });

            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_Null")
            {
                Json = $@"{{""{jsonPropertyName}"":null}}",
                JsonReaderExpectedException = new ExpectedException(typeof(ArgumentNullException), JsonWebKeyParameterNames.KeyOps, typeof(System.Text.Json.JsonException)),
                JsonSerializerExpectedException = new ExpectedException(typeof(ArgumentNullException), "IDX10000: ")
            });

            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_Number")
            {
                Json = $@"{{""d"":""string"",""d"":""string"",""{jsonPropertyName}"":1}}",
                JsonReaderExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11022: "),
                JsonSerializerExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted")
            });
        }

        /// <summary>
        /// Compares and finds differences between our internal Newtonsoft.Json and System.Text.Json
        /// comparing the results JsonSerializer.Deserialize and Utf8JsonReader.
        /// </summary>
        /// <param name="theoryData"></param>
        [Theory, MemberData(nameof(DeserializeDataSet))]
        public void Deserialize(JsonWebKeyTheoryData theoryData)
        {
            CompareContext context = new CompareContext(theoryData);

            JsonWebKey6x jsonWebKey6x = null;
            JsonWebKey jsonWebKeyDeserialize = null;
            JsonWebKey jsonWebKeyUtf8Reader = null;

            try
            {
                jsonWebKey6x = new JsonWebKey6x(theoryData.Json);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            try
            {
                jsonWebKeyDeserialize = System.Text.Json.JsonSerializer.Deserialize<JsonWebKey>(theoryData.Json);
                theoryData.JsonSerializerExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.JsonSerializerExpectedException.ProcessException(ex, context);
            }

            try
            {
                jsonWebKeyUtf8Reader = new JsonWebKey(theoryData.Json);
                theoryData.JsonReaderExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.JsonReaderExpectedException.ProcessException(ex, context);
            }

            // when comparing JsonWebKey (JsonSerializer.Deserialize) and (Newtonsoft.Json) ignore the AdditionalData, Oth, X5c, KeyOps properties are they have no getter.
            // We will need to adjust for a 8.0
            CompareContext localContext = new CompareContext(theoryData);
            localContext.PropertiesToIgnoreWhenComparing.Add(typeof(JsonWebKey), new List<string> { "AdditionalData", "Oth", "X5c", "KeyOps" });
            if (!IdentityComparer.AreEqual(jsonWebKeyDeserialize, jsonWebKey6x, localContext))
            {
                localContext.Diffs.Add("jsonWebKeyDeserialize != jsonWebKey6x");
                localContext.Diffs.Add("=========================================");
            }

            context.Merge(localContext);

            // when comparing JsonWebKey (JsonSerializer.Deserialize) and (utf8Reader) ignore the AdditionalData, Oth, X5c, KeyOps properties are they have no getter.
            // We will need to adjust for a 8.0
            localContext.Diffs.Clear();
            localContext.PropertiesToIgnoreWhenComparing.Clear();

            //RELNOTE: JsonSerializer.Deserialize does not handle mixed case
            // DataSets.JsonWebKeyMixedCaseString
            if (theoryData.TestId == "JsonWebKeyMixedCase")
                localContext.PropertiesToIgnoreWhenComparing.Add(typeof(JsonWebKey), new List<string> { "AdditionalData", "Oth", "X5c", "KeyOps", "Alg", "E", "X5tS256" });
            else
                localContext.PropertiesToIgnoreWhenComparing.Add(typeof(JsonWebKey), new List<string> { "AdditionalData", "Oth", "X5c", "KeyOps" });

            if (!IdentityComparer.AreEqual(jsonWebKeyDeserialize, jsonWebKeyUtf8Reader, localContext))
            {
                localContext.Diffs.Add("jsonWebKeyDeserialize != jsonWebKeyUtf8");
                localContext.Diffs.Add("=========================================");
            }

            context.Merge(localContext);

            // when comparing JsonWebKey (NewtonSoft) and (utf8Reader) ignore the AdditionalData
            // (Newtonsoft.Json) sets an Newtonsoft object, utf8Reader sets JsonElement.
            localContext.Diffs.Clear();
            localContext.PropertiesToIgnoreWhenComparing.Clear();
            localContext.AddDictionaryKeysToIgnoreWhenComparing("Object", "Array", "int");
            if (!IdentityComparer.AreEqual(jsonWebKeyUtf8Reader, jsonWebKey6x, localContext))
            {
                localContext.Diffs.Add("jsonWebKeyUtf8Reader != jsonWebKey6x");
                localContext.Diffs.Add("=========================================");
            }

            context.Merge(localContext);

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JsonWebKeyTheoryData> DeserializeDataSet
        {
            get
            {
                var theoryData = new TheoryData<JsonWebKeyTheoryData>();
                theoryData.Add(new JsonWebKeyTheoryData("JsonWebKey")
                {
                    JsonWebKey = DataSets.JsonWebKey1,
                    Json = DataSets.JsonWebKeyString
                });

                theoryData.Add(new JsonWebKeyTheoryData("JsonWebKeyMixedCase")
                {
                    JsonWebKey = DataSets.JsonWebKey1,
                    Json = DataSets.JsonWebKeyMixedCaseString
                });

                theoryData.Add(new JsonWebKeyTheoryData("JsonWebKeyES256")
                {
                    JsonWebKey = DataSets.JsonWebKeyES256,
                    Json = DataSets.JsonWebKeyES256String
                });

                theoryData.Add(new JsonWebKeyTheoryData("JsonWebKeyES384")
                {
                    JsonWebKey = DataSets.JsonWebKeyES384,
                    Json = DataSets.JsonWebKeyES384String
                });

                JsonWebKey jsonWebKey = JsonUtilities.FullyPopulatedJsonWebKey();
                string json = System.Text.Json.JsonSerializer.Serialize(jsonWebKey);
                theoryData.Add(new JsonWebKeyTheoryData("FullyPopulated")
                {
                    JsonWebKey = jsonWebKey,
                    Json = json
                });

                // System.Text.Json throws a JsonException with an inner of JsonReaderException that is internal.
                // We would have to use reflection to compare.
                theoryData.Add(new JsonWebKeyTheoryData("BadJson")
                {
                    ExpectedException = new ExpectedException(typeof(ArgumentException), "IDX10805: Error deserializing json: ", typeof(JsonReaderException)),
                    Json = "{adsd}",
                    JsonReaderExpectedException = new ExpectedException(typeExpected: typeof(ArgumentException), substringExpected: "IDX10805: Error deserializing json:", ignoreInnerException: true),
                    JsonSerializerExpectedException = new ExpectedException(typeExpected: typeof(System.Text.Json.JsonException), substringExpected: "'a' is an invalid start of a", ignoreInnerException: true)
                });

                return theoryData;
            }
        }

        /// <summary>
        /// This test is to ensure that a JsonWebKey from 6x == 7x.
        /// This is different that Deserialize as we are roundtripping objects in AdditionalData.
        /// </summary>
        /// <param name="theoryData"></param>
        [Theory, MemberData(nameof(RoundTripDataSet))]
        public void RoundTrip(JsonWebKeyTheoryData theoryData)
        {
            var context = new CompareContext(theoryData);
            try
            {
                string json6x = JsonConvert.SerializeObject(theoryData.JsonWebKey6x, Formatting.None);
                string jsonSerialize = System.Text.Json.JsonSerializer.Serialize<JsonWebKey>(theoryData.JsonWebKey);
                string jsonUtf8Writer = JsonWebKeySerializer.Write(theoryData.JsonWebKey);

                JsonWebKey6x jsonWebKey6x = JsonConvert.DeserializeObject<JsonWebKey6x>(json6x);
                JsonWebKey jsonWebKeyDeserialize = System.Text.Json.JsonSerializer.Deserialize<JsonWebKey>(jsonUtf8Writer);
                JsonWebKey jsonWebKeyUtf8Reader = new JsonWebKey(jsonUtf8Writer);

                // ensure that our utf8writer and newtonsoft are the same
                if (!IdentityComparer.AreEqual(jsonUtf8Writer, json6x, context))
                {
                    context.Diffs.Add("jsonUtf8Writer != json6x");
                    context.Diffs.Add("=========================================");
                }

                // RELNOTE: ensure that the output from our utf8writer is consummable by JsonSerializer.Deserialize since our collections are not settable, ignore the AdditionalData, KeyOps, Oth, X5c properties are they have no getter.
                // We will need to adjust for a 8.0 target
                // use a new CompareContext so that Properties to ignore are just applied to this compare
                CompareContext localContext = new CompareContext(theoryData);
                localContext.PropertiesToIgnoreWhenComparing.Add(typeof(JsonWebKey), new List<string> { "AdditionalData", "KeyOps", "Oth", "X5c" });
                if (!IdentityComparer.AreEqual(jsonWebKeyDeserialize, theoryData.JsonWebKey, localContext))
                {
                    localContext.Diffs.Add("jsonWebKeyDeserialize != theoryData.JsonWebKey");
                    localContext.Diffs.Add("=========================================");
                }

                context.Merge(localContext);

                // compare our utf8Reader with expected value
                if (!IdentityComparer.AreEqual(jsonWebKeyUtf8Reader, theoryData.JsonWebKey, context))
                {
                    context.Diffs.Add("jsonWebKeyUtf8Reader != theoryData.JsonWebKey");
                    context.Diffs.Add("=========================================");
                }

                // RELNOTE: we can give users a sample showing how to deserialize an object into a known type.
                if (jsonWebKeyUtf8Reader.AdditionalData.TryGetValue("JsonWebKey", out object jsonWebKey))
                {
                    JsonElement? jsonElement = jsonWebKey as JsonElement?;
                    if (jsonElement.HasValue)
                        jsonWebKeyUtf8Reader.AdditionalData["JsonWebKey"] = JsonWebKeySerializer.Read(jsonElement.Value.GetRawText());

                    jsonWebKey6x.AdditionalData["JsonWebKey"] = JsonConvert.DeserializeObject<JsonWebKey6x>(jsonWebKey6x.AdditionalData["JsonWebKey"].ToString());
                }

                // ensure what our utf8reader and newtonsoft are the same
                // RELNOTE:
                // Object for Newtonsoft will be 'Newtonsoft.Json.Linq.JObject', System.Text.Json 'System.Text.Json.JsonElement'
                // Array  for Newtonsoft will be 'Newtonsoft.Json.Linq.JArray', System.Text.Json 'System.Text.Json.JsonElement'
                // int for Newtonsoft will be 'System.Int64', System.Text.Json 'System.Int32'
                context.AddDictionaryKeysToIgnoreWhenComparing("Object", "Array", "int");
                if (!IdentityComparer.AreEqual(jsonWebKeyUtf8Reader, jsonWebKey6x, context))
                {
                    context.Diffs.Add("jsonWebKeyUtf8Reader != jsonWebKey6x");
                    context.Diffs.Add("=========================================");
                }
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }
        public static TheoryData<JsonWebKeyTheoryData> RoundTripDataSet
        {
            get
            {
                var theoryData = new TheoryData<JsonWebKeyTheoryData>();

                theoryData.Add(new JsonWebKeyTheoryData("AdditionalDataWithJsonWebKey")
                {
                    JsonWebKey = AdditionalData("JsonWebKey", JsonUtilities.CreateJsonElement(JsonWebKeySerializer.Write(new JsonWebKey { Alg = "Alg" }))),
                    JsonWebKey6x = AdditionalData6x("JsonWebKey", new JsonWebKey6x { Alg = "Alg" })
                });

                theoryData.Add(new JsonWebKeyTheoryData("AdditionalData")
                {
                    JsonWebKey = AdditionalData(),
                    JsonWebKey6x = AdditionalData6x()
                });

                theoryData.Add(new JsonWebKeyTheoryData("FullyPopulated")
                {
                    JsonWebKey = JsonUtilities.FullyPopulatedJsonWebKey(),
                    JsonWebKey6x = JsonUtilities.FullyPopulatedJsonWebKey6x()
                });

                return theoryData;
            }
        }

        private static JsonWebKey AdditionalData(string key = null, object obj = null)
        {
            JsonWebKey jsonWebKey = new JsonWebKey();
            JsonUtilities.SetAdditionalData(jsonWebKey.AdditionalData, key, obj);
            return jsonWebKey;
        }

        private static JsonWebKey6x AdditionalData6x(string key = null, object obj = null)
        {
            JsonWebKey6x jsonWebKey = new JsonWebKey6x();
            JsonUtilities.SetAdditionalData6x(jsonWebKey.AdditionalData, key, obj);
            return jsonWebKey;
        }
    }
}
