// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.TestUtils;
using Newtonsoft.Json;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Json.Tests
{
    public class JsonWebKeySetSerializationTests
    {
        /// <summary>
        /// This test is to ensure that a JsonWebKeySet1 from 6x == 7x.
        /// The keysets are fully populated and each property checked.
        /// </summary>
        /// <param name="theoryData"></param>
        [Theory, MemberData(nameof(SerializeDataSet))]
        public void Serialize(JsonWebKeySetTheoryData theoryData)
        {
            var context = new CompareContext(theoryData);
            try
            {
                string json6x = JsonConvert.SerializeObject(theoryData.JsonWebKeySet6x);
                string jsonSerialize = System.Text.Json.JsonSerializer.Serialize<JsonWebKeySet>(theoryData.JsonWebKeySet);
                string jsonUtf8Writer = JsonWebKeySetSerializer.Write(theoryData.JsonWebKeySet);

                JsonWebKeySet6x jsonWebKeySet6x = JsonConvert.DeserializeObject<JsonWebKeySet6x>(json6x);
                JsonWebKeySet jsonWebKeySetDeserialize = System.Text.Json.JsonSerializer.Deserialize<JsonWebKeySet>(jsonUtf8Writer);
                JsonWebKeySet jsonWebKeySetUtf8Reader = new JsonWebKeySet(jsonUtf8Writer);

                // ensure that our utf8writer and newtonsoft generate the same json string
                if (!IdentityComparer.AreEqual(jsonUtf8Writer, json6x, context))
                {
                    context.Diffs.Add("jsonUtf8Writer != json6x");
                    context.Diffs.Add("=========================================");
                }

                // compare our utf8Reader with expected value
                if (!IdentityComparer.AreEqual(jsonWebKeySetUtf8Reader, theoryData.JsonWebKeySet, context))
                {
                    context.Diffs.Add("jsonWebKeySetUtf8Reader != theoryData.JsonWebKeySet1");
                    context.Diffs.Add("=========================================");
                }

                // ensure what our utf8reader and newtonsoft keys are the same
                CompareContext localContext = new CompareContext(theoryData);
                localContext.AddDictionaryKeysToIgnoreWhenComparing("Object", "Array", "int");
                if (jsonWebKeySetUtf8Reader.Keys.Count == jsonWebKeySet6x.Keys.Count)
                {
                    // Now compare Keys assuming same order
                    for (int i = 0; i < jsonWebKeySetUtf8Reader.Keys.Count; i++)
                        if (!IdentityComparer.AreEqual(jsonWebKeySetUtf8Reader.Keys[i], jsonWebKeySet6x.Keys[i], localContext))
                        {
                            localContext.Diffs.Add($"jsonWebKeySetUtf8Reader.Keys'{i}' != jsonWebKey6x.Keys'{i}'");
                            localContext.Diffs.Add("=========================================");
                        }
                }
                else
                {
                    localContext.Diffs.Add("jsonWebKeySetUtf8Reader.Keys.Count != jsonWebKey6x.Keys.Count");
                    localContext.Diffs.Add("=========================================");
                }

                context.Merge(localContext);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JsonWebKeySetTheoryData> SerializeDataSet
        {
            get
            {
                var theoryData = new TheoryData<JsonWebKeySetTheoryData>();

                theoryData.Add(new JsonWebKeySetTheoryData("FullyPopulated")
                {
                    JsonWebKeySet = JsonUtilities.FullyPopulatedJsonWebKeySet(),
                    JsonWebKeySet6x = JsonUtilities.FullyPopulatedJsonWebKeySet6x()
                });

                return theoryData;
            }
        }

    }
}
