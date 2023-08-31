// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Tests;
using Newtonsoft.Json;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Json.Tests
{
    public class JsonSerializerPrimitivesTests
    {
        /// <summary>
        /// This test is designed to ensure that JsonSerializationPrimitives maximize depth of arrays of arrays to two.
        /// </summary>
        /// <param name="theoryData"></param>
        [Theory, MemberData(nameof(CheckMaximumDepthTheoryData))]
        public void CheckMaximumDepth(JsonSerializerTheoryData theoryData)
        {
            CompareContext context = new CompareContext(theoryData);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Utf8JsonWriter writer = null;
                try
                {
                    writer = new Utf8JsonWriter(memoryStream, new JsonWriterOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                    writer.WriteStartObject();

                    JsonSerializerPrimitives.WriteObject(ref writer, theoryData.PropertyName, theoryData.Object);

                    writer.WriteEndObject();
                    writer.Flush();

                    string json = Encoding.UTF8.GetString(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                    IdentityComparer.AreEqual(json, theoryData.Json, context);
                }
                finally
                {
                    writer?.Dispose();
                }
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JsonSerializerTheoryData> CheckMaximumDepthTheoryData
        {
            get
            {
                var theoryData = new TheoryData<JsonSerializerTheoryData>();

                theoryData.Add(
                    new JsonSerializerTheoryData("ObjectWithDictionary<string,string>")
                    {
                        Json = $@"{{""_claim_sources"":{{"+
                                    $@"""src1"":{{"+
                                    $@"""endpoint"":""https://graph.windows.net/5803816d-c4ab-4601-a128-e2576e5d6910/users/0c9545d0-a670-4628-8c1f-e90618a3b940/getMemberObjects"","+
                                    $@"""access_token"":""ksj3n283dke"""+
                                    $@"}},"+
                                    $@"""src2"":{{"+
                                    $@"""endpoint2"":""https://graph.windows.net/5803816d-c4ab-4601-a128-e2576e5d6910/users/0c9545d0-a670-4628-8c1f-e90618a3b940/getMemberObjects"","+
                                    $@"""access_token2"":""ksj3n283dke"""+
                               $@"}}}}}}",
                        PropertyName = "_claim_sources",
                        Object = new Dictionary<string, object>
                        {
                            {
                                "src1",
                                new Dictionary<string,string>
                                {
                                    { "endpoint", "https://graph.windows.net/5803816d-c4ab-4601-a128-e2576e5d6910/users/0c9545d0-a670-4628-8c1f-e90618a3b940/getMemberObjects"},
                                    { "access_token", "ksj3n283dke"}
                                }
                            },
                            {
                                "src2",
                                new Dictionary<string,string>
                                {
                                    { "endpoint2", "https://graph.windows.net/5803816d-c4ab-4601-a128-e2576e5d6910/users/0c9545d0-a670-4628-8c1f-e90618a3b940/getMemberObjects"},
                                    { "access_token2", "ksj3n283dke"}
                                }
                            }
                        }
                    });

                theoryData.Add(
                    new JsonSerializerTheoryData("Dictionary<string,object>Level3")
                    {
                        Json = $@"{{""key"":{{""l1_1"":1,""l1_2"":""level1"",""l2_dict"":{{""l2_1"":1,""l2_2"":""level2"",""l3_dict"":{{""l3_1"":1,""l3_2"":""level3""}}}}}}}}",
                        PropertyName = "key",
                        Object = new Dictionary<string, object> { { "l1_1", 1 }, { "l1_2", "level1" },
                                        { "l2_dict", new Dictionary<string, object> { { "l2_1", 1 }, { "l2_2", "level2" },
                                            { "l3_dict", new Dictionary<string, object> { { "l3_1", 1 }, { "l3_2", "level3" } } } } } }
                    });

                theoryData.Add(
                    new JsonSerializerTheoryData("Dictionary<string,object>Level1")
                    {
                        Json = $@"{{""key"":{{""l1_1"":1,""l1_2"":""level1""}}}}",
                        PropertyName = "key",
                        Object = new Dictionary<string, object> { { "l1_1", 1 }, { "l1_2", "level1" } },
                    });

                theoryData.Add(
                    new JsonSerializerTheoryData("List<object>Level1")
                    {
                        Json = @$"{{""key"":[1,""stringLevel1"",1.11]}}",
                        PropertyName = "key",
                        Object = new List<object> { 1, "stringLevel1", 1.11 },
                    });

                theoryData.Add(
                    new JsonSerializerTheoryData("List<object>Level2")
                    {
                        Json = @$"{{""key"":[1,""string"",1.11,[2,""stringLevel2"",2.22]]}}",
                        PropertyName = "key",
                        Object = new List<object> { 1, "string", 1.11, new List<object> { 2, "stringLevel2", 2.22 } },
                    });

                theoryData.Add(
                    new JsonSerializerTheoryData("List<object>Level3")
                    {
                        Json = $@"{{""key"":[1,""string"",1.11,[2,""stringLevel2"",2.22,[3,""stringLevel3"",3.33]]]}}",
                        PropertyName = "key",
                        Object = new List<object> { 1, "string", 1.11,
                                                    new List<object> { 2, "stringLevel2", 2.22,
                                                        new List<object> { 3, "stringLevel3", 3.33 } } }
                    });

                return theoryData;
            }
        }

        /// <summary>
        /// This test is designed to ensure that JsonDeserialize and Utf8Reader are consistent and
        /// that we understand the differences with newtonsoft.
        /// </summary>
        /// <param name="theoryData"></param>
        [Theory, MemberData(nameof(DeserializeTheoryData))]
        public void Deserialize(JsonSerializerTheoryData theoryData)
        {
            var context = new CompareContext(theoryData);
            JsonTestClass jsonDeserialize = null;
            JsonTestClass jsonRead = null;
            JsonTestClass jsonIdentityModel = null;

            CompareContext serializationContext = new CompareContext(theoryData);
            try
            {
                jsonIdentityModel = JsonConvert.DeserializeObject<JsonTestClass>(theoryData.Json);
                theoryData.IdentityModelSerializerExpectedException.ProcessNoException(serializationContext);
            }
            catch (Exception ex )
            {
                theoryData.IdentityModelSerializerExpectedException.ProcessException(ex, serializationContext);
            }

            if (serializationContext.Diffs.Count > 0)
            {
                context.Diffs.Add("ExpectedException difference in IdentityModel.Json.JsonConvert.DeserializeObject");
                context.Merge(serializationContext);
            }

            serializationContext.Diffs.Clear();
            try
            {
                jsonDeserialize = System.Text.Json.JsonSerializer.Deserialize<JsonTestClass>(theoryData.Json);
                theoryData.JsonSerializerExpectedException.ProcessNoException(serializationContext);
            }
            catch (Exception ex)
            {
                theoryData.JsonSerializerExpectedException.ProcessException(ex, serializationContext);
            }

            if (serializationContext.Diffs.Count > 0)
            {
                context.Diffs.Add("ExpectedException difference in JsonSerializer.Deserialize");
                context.Merge(serializationContext);
            }


            serializationContext.Diffs.Clear();
            try
            {
                jsonRead = JsonTestClassSerializer.Deserialize(theoryData.Json);
                theoryData.JsonReaderExpectedException.ProcessNoException(serializationContext);
            }
            catch (Exception ex)
            {
                theoryData.JsonReaderExpectedException.ProcessException(ex, serializationContext);
            }

            if (serializationContext.Diffs.Count > 0)
            {
                context.Diffs.Add("ExpectedException difference in JsonTestClassSerializer.Deserialize");
                context.Merge(serializationContext);
            }

            // newtonsoft maps Number to bool, System.Text.Json does not, it throws.
            if (theoryData.CompareMicrosoftJson)
            {
                serializationContext.Diffs.Clear();
                IdentityComparer.AreEqual(jsonDeserialize, jsonIdentityModel, serializationContext);
                if (serializationContext.Diffs.Count > 0)
                {
                    context.Diffs.Add("Difference between JsonSerializer.Deserialize and IdentityModel.Json.JsonConvert.DeserializeObject");
                    context.Merge(serializationContext);
                }
            }

            serializationContext.Diffs.Clear();
            IdentityComparer.AreEqual(jsonDeserialize, jsonRead, context);
            if (serializationContext.Diffs.Count > 0)
            {
                context.Diffs.Add("Difference between JsonSerializer.Deserialize and JsonTestClassSerializer");
                context.Merge(serializationContext);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JsonSerializerTheoryData> DeserializeTheoryData
        {
            get
            {
                var theoryData = new TheoryData<JsonSerializerTheoryData>();

                JsonSerializationTestUtilities.AddSerializationTestCases(
                    theoryData,
                    "Boolean",
                    new ExpectedException(typeof(JsonReaderException), ""),
                    new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11020: "),
                    new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted") { IgnoreInnerException = true }
                );

                JsonSerializationTestUtilities.AddSerializationTestCases(
                    theoryData,
                    "Double",
                    new ExpectedException(typeof(JsonReaderException), ""),
                    new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11020: "),
                    new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted") { IgnoreInnerException = true }
                );

                JsonSerializationTestUtilities.AddSerializationTestCases(
                    theoryData,
                    "Int",
                    new ExpectedException(typeof(JsonReaderException), ""),
                    new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11020: ") { IgnoreInnerException = true },
                    new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted") { IgnoreInnerException = true }
                );

                JsonSerializationTestUtilities.AddSerializationTestCases(
                    theoryData,
                    "ListObject",
                    new ExpectedException(typeof(JsonSerializationException), "") { IgnoreInnerException = true },
                    new ExpectedException(typeof(System.Text.Json.JsonException), ""),
                    new ExpectedException(typeof(System.Text.Json.JsonException), "")
                );

                JsonSerializationTestUtilities.AddSerializationTestCases(
                    theoryData,
                    "ListString",
                    new ExpectedException(typeof(JsonSerializationException), "") { IgnoreInnerException = true },
                    new ExpectedException(typeof(System.Text.Json.JsonException), ""),
                    new ExpectedException(typeof(System.Text.Json.JsonException), "") { IgnoreInnerException = true }
                    );

                JsonSerializationTestUtilities.AddSerializationTestCases(
                    theoryData,
                    "String",
                    new ExpectedException(typeof(JsonReaderException), "") { IgnoreInnerException = true },
                    new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11020: "),
                    new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted") { IgnoreInnerException = true }
                );

                return theoryData;
            }
        }

        /// <summary>
        /// This test is designed to ensure that JsonDeserialize and Utf8Reader are consistent w.r.t. exceptions.
        /// </summary>
        /// <param name="theoryData"></param>
        [Theory, MemberData(nameof(SerializeTheoryData))]
        public void Serialize(JsonSerializerTheoryData theoryData)
        {
            var context = new CompareContext(theoryData);
            string jsonIdentityModel = JsonConvert.SerializeObject(theoryData.JsonTestClass);
            string jsonNewtonsoft = JsonConvert.SerializeObject(theoryData.JsonTestClass);

            // without using the JavaScriptEncoder.UnsafeRelaxedJsonEscaping, System.Text.Json will escape all characters
            // we will need to have some way for the user to specify the encoder to use.
            string jsonSerialize = System.Text.Json.JsonSerializer.Serialize(
                theoryData.JsonTestClass,
                new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
#if NET6_0_OR_GREATER
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
#endif
                });

            string serialize = JsonTestClassSerializer.Serialize(
                theoryData.JsonTestClass,
                new JsonWriterOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                },
                theoryData.Serializers);

            CompareContext serializeContext = new CompareContext(theoryData);
            IdentityComparer.AreEqual(jsonNewtonsoft, jsonIdentityModel, serializeContext);
            if (serializeContext.Diffs.Count > 0)
            {
                context.Diffs.Add("Difference in Newtonsoft, IdentityModel");
                context.Merge(serializeContext);
            }

#if NET6_0_OR_GREATER
            serializeContext.Diffs.Clear();
            IdentityComparer.AreEqual(jsonNewtonsoft, jsonSerialize, serializeContext);
            if (serializeContext.Diffs.Count > 0)
            {
                context.Diffs.Add("Difference in Newtonsoft, JsonSerializer.Serialize");
                context.Merge(serializeContext);
            }
#endif
            serializeContext.Diffs.Clear();
            IdentityComparer.AreEqual(jsonNewtonsoft, serialize, serializeContext);
            if (serializeContext.Diffs.Count > 0)
            {
                context.Diffs.Add("Difference in Newtonsoft, JsonTestClassSerializer.Serialize");
                context.Merge(serializeContext);
            }

#if NET6_0_OR_GREATER
            serializeContext.Diffs.Clear();
            IdentityComparer.AreEqual(jsonSerialize, serialize, serializeContext);
            if (serializeContext.Diffs.Count > 0)
            {
                context.Diffs.Add("Difference in JsonSerializer.Serialize and JsonTestClassSerializer.Write");
                context.Merge(serializeContext);
            }
#endif
            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JsonSerializerTheoryData> SerializeTheoryData
        {
            get
            {
                TheoryData<JsonSerializerTheoryData> theoryData = new TheoryData<JsonSerializerTheoryData>();

                IDictionary<Type, IJsonSerializer> serializers = new Dictionary<Type, IJsonSerializer>
                {
                    { typeof(JsonTestClass), new SystemTextJsonSerializer() }
                };

                theoryData.Add(new JsonSerializerTheoryData("FullyPopulated")
                {
                    JsonTestClass = CreateJsonTestClass("*"),
                    Serializers = serializers
                });

                theoryData.Add(new JsonSerializerTheoryData("AdditionalData")
                {
                    JsonTestClass = CreateJsonTestClass("AdditionalData"),
                    Serializers = serializers
                });

                theoryData.Add(new JsonSerializerTheoryData("Boolean")
                {
                    JsonTestClass = CreateJsonTestClass("Boolean")
                });

                theoryData.Add(new JsonSerializerTheoryData("Double")
                {
                    JsonTestClass = CreateJsonTestClass("Double")
                });

                theoryData.Add(new JsonSerializerTheoryData("Int")
                {
                    JsonTestClass = CreateJsonTestClass("Int")
                });

                theoryData.Add(new JsonSerializerTheoryData("ListObject")
                {
                    JsonTestClass = CreateJsonTestClass("ListObject")
                });

                theoryData.Add(new JsonSerializerTheoryData("ListString")
                {
                    JsonTestClass = CreateJsonTestClass("ListString")
                });

                theoryData.Add(new JsonSerializerTheoryData("String")
                {
                    JsonTestClass = CreateJsonTestClass("String")
                });

                return theoryData;
            }
        }

        private static JsonTestClass CreateJsonTestClass(string propertiesToSet)
        {
            JsonTestClass jsonTestClass = new JsonTestClass();

            if (propertiesToSet == "*" || propertiesToSet.Contains("AdditionalData"))
            {
                jsonTestClass.AdditionalData["Key1"] = "Data1";
                jsonTestClass.AdditionalData["Object"] = new JsonTestClass { Boolean = true, Double = 1.4, AdditionalData = new Dictionary<string, object> { { "key", "value" } } };
            }

            if (propertiesToSet == "*" || propertiesToSet.Contains("Boolean"))
                jsonTestClass.Boolean = true;

            if (propertiesToSet == "*" || propertiesToSet.Contains("Double"))
                jsonTestClass.Double = 1.1;

            if (propertiesToSet == "*" || propertiesToSet.Contains("Int"))
                jsonTestClass.Int = 1;

            if (propertiesToSet == "*" || propertiesToSet.Contains("ListObject"))
                jsonTestClass.ListObject = new List<object> { 1, "string", true, "{\"innerArray\", [1, \"innerValue\"] }" };

            if (propertiesToSet == "*" || propertiesToSet.Contains("ListString"))
                jsonTestClass.ListString = new List<string> { "string1", "string2" };

            if (propertiesToSet == "*" || propertiesToSet.Contains("String"))
                jsonTestClass.String = "string";

            return jsonTestClass;
        }
    }
}

