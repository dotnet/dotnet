// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Tests;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Json.Tests
{
    public class JsonSerializerPrimitivesTests
    {
        [Theory, MemberData(nameof(RoundTripObjectsTheoryData), DisableDiscoveryEnumeration = true)]
        public void RoundTripObjects(JsonSerializerTheoryData theoryData)
        {
            CompareContext context = TestUtilities.WriteHeader($"{this}.RoundTripObjects", theoryData);
            MemoryStream memoryStream = new MemoryStream();
            Utf8JsonWriter writer = new Utf8JsonWriter(memoryStream, new JsonWriterOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
            try
            {
                writer.WriteStartObject();
                JsonSerializerPrimitives.WriteObject(ref writer, theoryData.PropertyName, theoryData.Object);
                writer.WriteEndObject();
                writer.Flush();

                // getting the json string helps with debugging
                string json = Encoding.UTF8.GetString(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                Utf8JsonReader reader = new Utf8JsonReader(memoryStream.GetBuffer());

                // first read positions at the start of the object, second read positions at the property name
                reader.Read();
                reader.Read();

                string propertyName = JsonSerializerPrimitives.ReadPropertyName(ref reader, this.GetType().Name, false);
                object obj = JsonSerializerPrimitives.ReadPropertyValueAsObject(ref reader, theoryData.PropertyName, this.GetType().Name, true);
                IdentityComparer.AreEqual(obj, theoryData.ReadObject, context);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }
            finally
            {
                writer?.Dispose();
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JsonSerializerTheoryData> RoundTripObjectsTheoryData
        {
            // there are a lot of tests here as this tests the lowest level of the serializer.
            // Please do not use '.' in test names as double clicking on TheoryData.TestId will not copy the name to the clipboard.
            // With so many tests conditional break points are very useful.
            get
            {
                var theoryData = new TheoryData<JsonSerializerTheoryData>();
                #region First LevelTests
                // TODO - guid is serialized as a string make sure test exists for JsonWebToken to that TryGetValue<Guid> is tested
                // When creating the JsonElement don't add any spaces in the string or the test will fail on JsonElement.RawText.
                // We could have chosen to remove the spaces, but that might interfer with other tests.

                DateTime dateTime = DateTime.UtcNow;
                AddFloatDoubleVariations(theoryData);
                AddListVariations(new List<string> { "string1", "string2" }, theoryData);
                AddMinMaxVariations(DateTime.MaxValue, DateTime.MinValue, dateTime, theoryData);
                AddMinMaxVariations(float.MaxValue, float.MinValue, (float)0, theoryData);
                AddMinMaxVariations(double.MaxValue, double.MinValue, (double)0, theoryData);
                AddMinMaxVariations(decimal.MaxValue, decimal.MinValue, (decimal)0, theoryData);
                AddMinMaxVariations(long.MaxValue, long.MinValue, (long)0, theoryData);
                AddMinMaxVariations(int.MaxValue, int.MinValue, 0, theoryData);

                Guid guid = Guid.NewGuid();
                theoryData.Add(new JsonSerializerTheoryData("Guid")
                {
                    Object = guid,
                    ReadObject = guid.ToString()
                });

                theoryData.Add(new JsonSerializerTheoryData("true")
                {
                    Object = true,
                    ReadObject = true
                });

                JsonElement? jsonElement = JsonUtilities.CreateJsonElement("""{"string1":"value1"}""");

                theoryData.Add(new JsonSerializerTheoryData("Dictionary_object_object>")
                {
                    Object = new Dictionary<object, object> { { "string1", "value1" } },
                    ReadObject = jsonElement
                });

                theoryData.Add(new JsonSerializerTheoryData("Dictionary_string_string")
                {
                    Object = new Dictionary<string, string> { { "string1", "value1" } },
                    ReadObject = jsonElement
                });

                theoryData.Add(new JsonSerializerTheoryData("IDictionary_string_string")
                {
                    Object = new Dictionary<string, string> { { "string1", "value1" } } as IDictionary<string, string>,
                    ReadObject = jsonElement
                });

                theoryData.Add(new JsonSerializerTheoryData("Dictionary_object_string>")
                {
                    Object = new Dictionary<object, string> { { "string1", "value1" } },
                    ReadObject = jsonElement
                });
                #endregion

                #region Second LevelTests
                // objects embeded in a dictionary or list
                theoryData.Add(new JsonSerializerTheoryData("Dictionary_Guid")
                {
                    Object = new Dictionary<string, object> { { "key1", new Dictionary<string, object> { { "guid", guid } } } },
                    ReadObject = JsonUtilities.CreateJsonElement($$$"""{"key1":{"guid":"{{{guid.ToString()}}}"}}""")
                });

                theoryData.Add(new JsonSerializerTheoryData("Dictionary_Dictionary_List_string")
                {
                    Object = new Dictionary<object, object> { { "key1", new Dictionary<string, object> { { "key2", new List<string> { "string1", "string2" } } } } },
                    ReadObject = JsonUtilities.CreateJsonElement("""{"key1":{"key2":["string1","string2"]}}""")
                });

                theoryData.Add(new JsonSerializerTheoryData("List_Dictionary_String_List_String}")
                {
                    Object = new List<object> { "list", new Dictionary<string, string> { { "string1", "string2" } }, new List<string> { "string3", "string4" } },
                    ReadObject = JsonUtilities.CreateJsonElement("""["list",{"string1":"string2"},["string3","string4"]]""")
                });

                // For some versions of net, JsonDocument.Parse returns 1.7976931348623157E+308 instead of 1.79769313486232E+308 the value returned by double.MaxValue.ToString()
#if NET6_0_OR_GREATER
                string jsonElementString =
                $$"""
                ["string1","{{guid.ToString()}}",{{int.MaxValue}},{{long.MaxValue}},true,{{double.MaxValue}},null,{{decimal.MaxValue}}]
                """;
                theoryData.Add(new JsonSerializerTheoryData("ListWithPrimitiveTypes")
                {
                    Object = new List<object> { "string1", guid, int.MaxValue, long.MaxValue, true, double.MaxValue, null, decimal.MaxValue },
                    ReadObject = JsonUtilities.CreateJsonElement(jsonElementString)
                });
#else
                string jsonElementString =
                $$"""
                ["string1","{{guid.ToString()}}",{{int.MaxValue}},{{long.MaxValue}},true,1.7976931348623157E+308,null,{{decimal.MaxValue}}]
                """;
                theoryData.Add(new JsonSerializerTheoryData("ListWithPrimitiveTypes")
                {
                    Object = new List<object> { "string1", guid, int.MaxValue, long.MaxValue, true, 1.7976931348623157E+308, null, decimal.MaxValue },
                    ReadObject = JsonUtilities.CreateJsonElement(jsonElementString)
                });
#endif
                #endregion

                return theoryData;
            }
        }

        private static void AddListVariations(List<string> strings, TheoryData<JsonSerializerTheoryData> theoryData)
        {
            string jsonElementString = "[";
            for (int i = 0; i < strings.Count - 1; i++)
                jsonElementString += $@"""{strings[i]}"",";

            jsonElementString += $@"""{strings[strings.Count - 1]}""]";

            theoryData.Add(new JsonSerializerTheoryData("StringArray")
            {
                Object = strings.ToArray(),
                ReadObject = JsonUtilities.CreateJsonElement(jsonElementString)
            });

            theoryData.Add(new JsonSerializerTheoryData("ListOfString")
            {
                Object = new List<string>(strings),
                ReadObject = JsonUtilities.CreateJsonElement(jsonElementString)
            });

            theoryData.Add(new JsonSerializerTheoryData("IListOfString")
            {
                Object = (new List<string>(strings) as IList<string>),
                ReadObject = JsonUtilities.CreateJsonElement(jsonElementString)
            });

            theoryData.Add(new JsonSerializerTheoryData("IEnumerableOfString")
            {
                Object = (new List<string>(strings) as IEnumerable<string>),
                ReadObject = JsonUtilities.CreateJsonElement(jsonElementString)
            });

            theoryData.Add(new JsonSerializerTheoryData("CollectionOfString")
            {
                Object = new Collection<string>(strings),
                ReadObject = JsonUtilities.CreateJsonElement(jsonElementString)
            });

            theoryData.Add(new JsonSerializerTheoryData("JsonElement_ListOfString")
            {
                Object = JsonUtilities.CreateJsonElement(jsonElementString),
                ReadObject = JsonUtilities.CreateJsonElement(jsonElementString)
            });
        }

        private static void AddMinMaxVariations(object minValue, object maxValue, object zero, TheoryData<JsonSerializerTheoryData> theoryData)
        {
            theoryData.Add(new JsonSerializerTheoryData(minValue.GetType().Name + "_MinValue")
            {
                Object = minValue,
                ReadObject = minValue
            });

            theoryData.Add(new JsonSerializerTheoryData(minValue.GetType().Name + "_MaxValue")
            {
                Object = minValue,
                ReadObject = minValue
            });

            theoryData.Add(new JsonSerializerTheoryData(minValue.GetType().Name + "_Zero")
            {
                Object = zero,
                ReadObject = zero
            });
        }

        private static void AddFloatDoubleVariations(TheoryData<JsonSerializerTheoryData> theoryData)
        {
            theoryData.Add(new JsonSerializerTheoryData("Single_11.1")
            {
                Object = (float)11.1,
                ReadObject = (float)11.1
            });

            theoryData.Add(new JsonSerializerTheoryData("Single_Minus_11.1")
            {
                Object = (float)-11.1,
                ReadObject = (float)-11.1
            });

            theoryData.Add(new JsonSerializerTheoryData("Double_11.1")
            {
                Object = (double)11.1,
                ReadObject = (double)11.1
            });

            theoryData.Add(new JsonSerializerTheoryData("Double_Minus_11.1")
            {
                Object = (double)-11.1,
                ReadObject = (double)-11.1
            });

            theoryData.Add(new JsonSerializerTheoryData("List_Single_11.1")
            {
                Object = new List<object> { (float)11.1 },
                ReadObject = JsonUtilities.CreateJsonElement("[11.1]")
            });

            theoryData.Add(new JsonSerializerTheoryData("List_Single_Minus_11.1")
            {
                Object = new List<object> { (float)-11.1 },
                ReadObject = JsonUtilities.CreateJsonElement("[-11.1]")
            });

            theoryData.Add(new JsonSerializerTheoryData("List_Double_11.1")
            {
                Object = new List<object> { (double)11.1 },
                ReadObject = JsonUtilities.CreateJsonElement("[11.1]")
            });

            theoryData.Add(new JsonSerializerTheoryData("List_Double_Minus_11.1")
            {
                Object = new List<object> { (double)-11.1 },
                ReadObject = JsonUtilities.CreateJsonElement("[-11.1]")
            });
        }

        [Fact]
        public void CheckMaxDepthReading()
        {
            var document = JsonDocument.Parse(@"{""key"":" + new string('[', 62) + @"""value""" + new string(']', 62) + "}");

            Dictionary<string, object> value;
            JsonSerializerPrimitives.TryCreateTypeFromJsonElement(document.RootElement, out value);
            Assert.NotNull(value);

            document = JsonDocument.Parse(@"{""key"":" + new string('[', 63) + @"""value""" + new string(']', 63) + "}");
            Assert.Throws<InvalidOperationException>(() => JsonSerializerPrimitives.TryCreateTypeFromJsonElement(document.RootElement, out value));

            JsonDocument GenerateJson(int depth)
            {
                var json = new StringBuilder();

                json.Append(@"{""root"":");

                foreach (var idx in Enumerable.Range(0, depth))
                {
                    if (idx != depth - 1)
                        json.Append($@"{{""key-{idx}"":");
                    else
                        json.Append(@"""value""");
                }

                json.Append(new string('}', depth - 1));
                json.Append('}');

                var jsonStr = json.ToString();

                return JsonDocument.Parse(jsonStr);
            }

            document = GenerateJson(63);

            JsonSerializerPrimitives.TryCreateTypeFromJsonElement(document.RootElement, out value);
            Assert.NotNull(value);

            document = GenerateJson(64);
            Assert.Throws<InvalidOperationException>(() => JsonSerializerPrimitives.TryCreateTypeFromJsonElement(document.RootElement, out value));

            document = GenerateJson(50);
            var document2 = GenerateJson(50);

            var mergedJson = @$"{{ ""root1"": {document.RootElement} , ""root2"": {document2.RootElement}}}";
            var doc = JsonDocument.Parse(mergedJson);

            JsonSerializerPrimitives.TryCreateTypeFromJsonElement(doc.RootElement, out value);
            Assert.NotNull(value);
        }

        [Fact]
        public void CheckNumberOfProperties()
        {
            var json = new StringBuilder();

            json.Append('{');

            foreach (var i in Enumerable.Range(0, 100))
            {
                json.Append($@"""key-{i}"":""value-{i}""");
                if (i != 99)
                    json.Append(',');
            }

            json.Append('}');

            var document = JsonDocument.Parse(json.ToString());

            Dictionary<string, object> value;
            JsonSerializerPrimitives.TryCreateTypeFromJsonElement(document.RootElement, out value);
            Assert.NotNull(value);

            json = new StringBuilder();

            json.Append('{');

            foreach (var i in Enumerable.Range(0, 100))
            {
                json.Append($@"""key-{i}"":{{""inner-key-{i}"":""value-{i}""}}");
                if (i != 99)
                    json.Append(',');
            }

            json.Append('}');

            document = JsonDocument.Parse(json.ToString());

            JsonSerializerPrimitives.TryCreateTypeFromJsonElement(document.RootElement, out value);
            Assert.NotNull(value);
        }

        /// <summary>
        /// This test is designed to ensure that JsonSerializationPrimitives maximize depth of arrays of arrays.
        /// </summary>
        /// <param name="theoryData"></param>
        [Theory, MemberData(nameof(CheckMaximumDepthWritingTheoryData), DisableDiscoveryEnumeration = true)]
        public void CheckMaximumDepthWriting(JsonSerializerTheoryData theoryData)
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
                    theoryData.ExpectedException.ProcessNoException(context);
                }
                catch (Exception ex)
                {
                    theoryData.ExpectedException.ProcessException(ex, context);
                }
                finally
                {
                    writer?.Dispose();
                }

                TestUtilities.AssertFailIfErrors(context);
            }
        }

        public static TheoryData<JsonSerializerTheoryData> CheckMaximumDepthWritingTheoryData
        {
            get
            {
                var theoryData = new TheoryData<JsonSerializerTheoryData>();

                theoryData.Add(
                    new JsonSerializerTheoryData("ObjectWithDictionary<string,string>")
                    {
                        Json = $@"{{""_claim_sources"":{{" +
                                    $@"""src1"":{{" +
                                    $@"""endpoint"":""https://graph.windows.net/5803816d-c4ab-4601-a128-e2576e5d6910/users/0c9545d0-a670-4628-8c1f-e90618a3b940/getMemberObjects""," +
                                    $@"""access_token"":""ksj3n283dke""" +
                                    $@"}}," +
                                    $@"""src2"":{{" +
                                    $@"""endpoint2"":""https://graph.windows.net/5803816d-c4ab-4601-a128-e2576e5d6910/users/0c9545d0-a670-4628-8c1f-e90618a3b940/getMemberObjects""," +
                                    $@"""access_token2"":""ksj3n283dke""" +
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

                (var json, var result) = CreateJsonSerializerTheoryData(61);

                theoryData.Add(new JsonSerializerTheoryData($"ListObject64Depth")
                {
                    Json = json,
                    PropertyName = "key",
                    Object = result,
                });

                (json, result) = CreateJsonSerializerTheoryData(63);

                theoryData.Add(new JsonSerializerTheoryData($"ListObject65Depth")
                {
                    Json = json,
                    PropertyName = "key",
                    Object = result,
                    ExpectedException = new ExpectedException(typeExpected: typeof(InvalidOperationException))
                });

                (json, result) = CreateJsonSerializerTheoryData(50);
                (var json2, var result2) = CreateJsonSerializerTheoryData(50);

                // merge
                var mergedJson = @$"{{""key"":{{""key1"":{json},""key2"":{json2}}}}}";
                var mergedObjects = new Dictionary<string, object>
                {
                    ["key1"] = new Dictionary<string, object> { ["key"] = result },
                    ["key2"] = new Dictionary<string, object> { ["key"] = result2 },
                };

                theoryData.Add(new JsonSerializerTheoryData($"MultipleObjects")
                {
                    Json = mergedJson,
                    PropertyName = "key",
                    Object = mergedObjects,
                });

                var jsonBuilder = new StringBuilder();

                var innerObject = new Dictionary<string, object>();

                jsonBuilder.Append(@"{""key"":{");

                foreach (var i in Enumerable.Range(0, 100))
                {
                    innerObject.Add($"key-{i}", new Dictionary<string, string> { [$"inner-key-{i}"] = $"value-{i}" });
                    jsonBuilder.Append($@"""key-{i}"":{{""inner-key-{i}"":""value-{i}""}}");
                    if (i != 99)
                        jsonBuilder.Append(',');
                }

                jsonBuilder.Append("}}");

                theoryData.Add(new JsonSerializerTheoryData("MultipleProperties")
                {
                    PropertyName = "key",
                    Json = jsonBuilder.ToString(),
                    Object = innerObject,
                });

                return theoryData;
            }
        }

        private static (string, object) CreateJsonSerializerTheoryData(int depth)
        {
            var runningJson = new StringBuilder();
            var runningExpectedObject = new List<object>();
            var resultObject = runningExpectedObject;

            runningJson.Append($@"{{""key"":[");

            for (int i = 0; i < depth; i++)
            {
                var toAdd = new List<object> { $"key-{i}" };
                runningExpectedObject.Add(toAdd);
                runningExpectedObject = toAdd;
                runningJson.Append($@"[""key-{i}"",");
            }

            runningJson.Remove(runningJson.Length - 1, 1);
            runningJson.Append(new string(']', depth));
            runningJson.Append("]}");

            return (runningJson.ToString(), resultObject);
        }
    }
}

