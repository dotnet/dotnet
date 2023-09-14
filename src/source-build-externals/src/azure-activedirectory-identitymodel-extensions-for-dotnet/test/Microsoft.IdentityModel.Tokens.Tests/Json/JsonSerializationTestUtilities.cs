// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Tests;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Json.Tests
{
    public class JsonSerializationTestUtilities
    {
        /// <summary>
        /// Adds tests cases for a type specifying property name desired for the json. Json will be created using that property name with different values.
        /// Different combinations are created to help understand what error messages will be generated and to ensure different forms
        /// For example: for property name: Int, 7 tests will be added with different variations of json, { "Int": false }, { "Int": 1 }, etc.
        /// The propertyType helps us in determining if exceptions are expected.
        /// For example: propertyType == "Int" would create a test with ExpectedException.NoExceptionExpected with the json { "Int", 1 }
        /// This allows us to know the differences between JsonSerializer.Deserialize, Utf8Reader.
        /// </summary>
        /// <param name="theoryData">place to add the test case.</param>
        /// <param name="propertyName">property name that matches <see cref="JsonTestClass/></param>
        /// <param name="newtonsoftExpectedException">expected exception for Newtonsoft.</param>
        /// <param name="jsonReaderExpectedException">expected exception for Utf8JsonReader.</param>
        /// <param name="jsonSerializerExpectedException">expected exception for JsonSerializer.</param>
        public static void AddSerializationTestCases(
            TheoryData<JsonSerializerTheoryData> theoryData,
            string propertyName,
            ExpectedException newtonsoftExpectedException,
            ExpectedException jsonReaderExpectedException,
            ExpectedException jsonSerializerExpectedException)
        {
            theoryData.Add(new JsonSerializerTheoryData($"{propertyName}_ListObject")
            {
                // Newtonsoft will not fault when deseralizing an array of objects List<string>
                // System.Text.Json will fault
                // Therefore we cannot compare the results of Newtonsoft and System.Text.Json
                // Newtonsoft will transform objects into POCO types
                // System.Text.Json will leave objects as a JsonElement
                // Therefore we cannot compare the results of Newtonsoft and System.Text.Json
                CompareMicrosoftJson = (propertyName == "ListObject" || propertyName == "ListString") ? false : true,

                Json = $@"{{""{propertyName}"":[""string"", 1, 1.456, true, null]}}",

                JsonReaderExpectedException =
                    (propertyName == "ListObject") ?
                        ExpectedException.NoExceptionExpected :
                        jsonReaderExpectedException,

                JsonSerializerExpectedException =
                    (propertyName == "ListObject") ?
                        ExpectedException.NoExceptionExpected :
                        jsonSerializerExpectedException,

                IdentityModelSerializerExpectedException =
                    (propertyName == "ListObject" || propertyName == "ListString") ?
                        ExpectedException.NoExceptionExpected :
                        newtonsoftExpectedException
            });

            theoryData.Add(new JsonSerializerTheoryData($"{propertyName}_ListString")
            {
                // Newtonsoft WILL fault when deseralizing an array of string into a property of List<object>
                // System.Text.Json will fault
                // Therefore we cannot compare the results of Newtonsoft and System.Text.Json
                CompareMicrosoftJson = (propertyName == "ListObject") ? false : true,

                Json = $@"{{""{propertyName}"":[""string1"", ""string2""]}}",

                JsonReaderExpectedException =
                    (propertyName == "ListObject" || propertyName == "ListString") ?
                        ExpectedException.NoExceptionExpected :
                        jsonReaderExpectedException,

                JsonSerializerExpectedException =
                    (propertyName == "ListObject" || propertyName == "ListString") ?
                        ExpectedException.NoExceptionExpected :
                        jsonSerializerExpectedException,

                IdentityModelSerializerExpectedException =
                    (propertyName == "ListObject" || propertyName == "ListString") ?
                        ExpectedException.NoExceptionExpected :
                        newtonsoftExpectedException
            });

            theoryData.Add(new JsonSerializerTheoryData($"{propertyName}_Double")
            {
                // Newtonsoft will not fault when deseralizing a double into a property of type bool or string
                // System.Text.Json will fault
                // Therefore we cannot compare the results of Newtonsoft and System.Text.Json
                CompareMicrosoftJson = (propertyName == "Boolean" || propertyName == "String") ? false : true,

                Json = $@"{{""{propertyName}"":1.45}}",

                JsonReaderExpectedException =
                    (propertyName == "Double") ?
                        ExpectedException.NoExceptionExpected :
                        jsonReaderExpectedException,

                JsonSerializerExpectedException =
                    (propertyName == "Double") ?
                        ExpectedException.NoExceptionExpected :
                        jsonSerializerExpectedException,

                IdentityModelSerializerExpectedException =
                    (propertyName == "Boolean" || propertyName == "Double" || propertyName == "String") ?
                        ExpectedException.NoExceptionExpected :
                        newtonsoftExpectedException
            });

            theoryData.Add(new JsonSerializerTheoryData($"{propertyName}_Int")
            {
                // Newtonsoft will not fault when deseralizing an int into a property of type bool or string
                // System.Text.Json will fault
                // Therefore we cannot compare the results of Newtonsoft and System.Text.Json
                CompareMicrosoftJson = (propertyName == "Boolean" || propertyName == "String") ? false : true,

                Json = $@"{{""{propertyName}"":0}}",

                JsonReaderExpectedException =
                    (propertyName == "Double" || propertyName == "Int") ?
                        ExpectedException.NoExceptionExpected :
                        jsonReaderExpectedException,

                JsonSerializerExpectedException =
                    (propertyName == "Double" || propertyName == "Int") ?
                        ExpectedException.NoExceptionExpected :
                        jsonSerializerExpectedException,

                IdentityModelSerializerExpectedException =
                    (propertyName == "Boolean" || propertyName == "Double" || propertyName == "Int" || propertyName == "String") ?
                        ExpectedException.NoExceptionExpected :
                        newtonsoftExpectedException
            });

            theoryData.Add(new JsonSerializerTheoryData($"{propertyName}_Object")
            {
                Json = $@"{{""{propertyName}"":{{""property"": ""false""}}}}",

                JsonReaderExpectedException = (propertyName == "Object") ?
                    ExpectedException.NoExceptionExpected :
                    jsonReaderExpectedException,

                JsonSerializerExpectedException =
                    (propertyName == "Object") ?
                        ExpectedException.NoExceptionExpected :
                        jsonSerializerExpectedException,

                IdentityModelSerializerExpectedException =
                    (propertyName == "Object") ?
                        ExpectedException.NoExceptionExpected :
                        newtonsoftExpectedException
            });

            theoryData.Add(new JsonSerializerTheoryData($"{propertyName}_String")
            {
                Json = $@"{{""{propertyName}"":""string""}}",

                JsonReaderExpectedException =
                    (propertyName == "String") ?
                        ExpectedException.NoExceptionExpected :
                        jsonReaderExpectedException,

                JsonSerializerExpectedException =
                    (propertyName == "String") ?
                        ExpectedException.NoExceptionExpected :
                        jsonSerializerExpectedException,

                IdentityModelSerializerExpectedException =
                    (propertyName == "String") ?
                        ExpectedException.NoExceptionExpected :
                        newtonsoftExpectedException
            });

            theoryData.Add(new JsonSerializerTheoryData($"{propertyName}_true")
            {
                // Newtonsoft will not fault when deseralizing a string into a property of type string
                // System.Text.Json will fault
                // Therefore we cannot compare the results of Newtonsoft and System.Text.Json
                CompareMicrosoftJson = (propertyName == "String") ? false : true,

                Json = $@"{{""{propertyName}"": true}}",

                JsonReaderExpectedException =
                    (propertyName == "Boolean") ?
                        ExpectedException.NoExceptionExpected :
                        jsonReaderExpectedException,

                JsonSerializerExpectedException =
                    (propertyName == "Boolean") ?
                        ExpectedException.NoExceptionExpected :
                        jsonSerializerExpectedException,

                IdentityModelSerializerExpectedException =
                    (propertyName == "Boolean" || propertyName == "String") ?
                        ExpectedException.NoExceptionExpected :
                        newtonsoftExpectedException
            });
        }
    }
}

