// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Json.Tests
{
    public class JsonWebKeySetSerializationTests
    {
        /// <summary>
        /// This test is to ensure that JsonWebKeySet serialization with fully populated with each property checked.
        /// </summary>
        /// <param name="theoryData"></param>
        [Theory, MemberData(nameof(SerializeDataSet), DisableDiscoveryEnumeration = true)]
        public void Serialize(JsonWebKeySetTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.Serialize", theoryData);
            context.PropertiesToIgnoreWhenComparing = new Dictionary<Type, List<string>>
            {
                // If the objects being compared are created from the same string and they are equal, the string itself can be ignored.
                // The strings may not be equal because of whitespace, but the json they represent is semantically identical.
                { typeof(JsonWebKeySet), [ "JsonData" ] },
            };

            try
            {
                string jsonSerialize = System.Text.Json.JsonSerializer.Serialize<JsonWebKeySet>(theoryData.JsonWebKeySet);
                string jsonUtf8Writer = JsonWebKeySetSerializer.Write(theoryData.JsonWebKeySet);

                JsonWebKeySet jsonWebKeySetDeserialize = System.Text.Json.JsonSerializer.Deserialize<JsonWebKeySet>(jsonUtf8Writer);
                JsonWebKeySet jsonWebKeySetUtf8Reader = new JsonWebKeySet(jsonUtf8Writer);

                // compare our utf8Reader with expected value
                if (!IdentityComparer.AreEqual(jsonWebKeySetUtf8Reader, theoryData.JsonWebKeySet, context))
                {
                    context.Diffs.Add("jsonWebKeySetUtf8Reader != theoryData.JsonWebKeySet");
                    context.Diffs.Add("=========================================");
                }
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
                });

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(JsonWebKeySetTheoryData), DisableDiscoveryEnumeration = true)]
        public void Deserialize(JsonWebKeySetTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.Deserialize", theoryData);
            context.PropertiesToIgnoreWhenComparing = new Dictionary<Type, List<string>>
            {
                // If the objects being compared are created from the same string and they are equal, the string itself can be ignored.
                // The strings may not be equal because of whitespace, but the json they represent is semantically identical.
                { typeof(JsonWebKeySet), [ "JsonData" ] },
            };

            try
            {
                JsonWebKeySet jsonWebKeySet = new JsonWebKeySet(theoryData.Json);
                JsonWebKeySet jsonWebKeySetUpperCase = new JsonWebKeySet(JsonUtilities.SetPropertiesToUpperCase(theoryData.Json));
                theoryData.ExpectedException.ProcessNoException(context);

                IdentityComparer.AreEqual(jsonWebKeySet, theoryData.JsonWebKeySet, context);
                IdentityComparer.AreEqual(jsonWebKeySetUpperCase, theoryData.JsonWebKeySet, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JsonWebKeySetTheoryData> JsonWebKeySetTheoryData
        {
            get
            {
                var theoryData = new TheoryData<JsonWebKeySetTheoryData>();

                theoryData.Add(new JsonWebKeySetTheoryData("AADCommonV1")
                {
                    Json = DataSets.AADCommonV1KeySetJson,
                    JsonWebKeySet = DataSets.AADCommonV1KeySet
                });

                // the reason to replace "issuer" with "ISSUER" is because the test deserializes uppercase and lowercase.
                // since "issuer" is not a property of JsonWebKeySet the value ends up in the AdditionalData dictionary, which is case sensitive.
                // we wanted to leave the data sets as they were obtained from metadata so they can be used in other tests.
                theoryData.Add(new JsonWebKeySetTheoryData("AADCommonV2")
                {
                    Json = JsonUtilities.SetAdditionalDataKeysToUpperCase(DataSets.AADCommonV2KeySetJson, DataSets.AADCommonV2KeySet),
                    JsonWebKeySet = JsonUtilities.SetAdditionalDataKeysToUpperCase(DataSets.AADCommonV2KeySet)
                });

                theoryData.Add(new JsonWebKeySetTheoryData("AccountsGoogleCom")
                {
                    Json = DataSets.AccountsGoogleJson,
                    JsonWebKeySet = DataSets.AccountsGoogleKeySet
                });

                return theoryData;
            }
        }
    }
}
