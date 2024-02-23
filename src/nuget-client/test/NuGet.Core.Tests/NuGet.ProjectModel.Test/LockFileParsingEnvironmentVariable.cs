// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Test.Utility;

namespace NuGet.ProjectModel
{
    public static class LockFileParsingEnvironmentVariable
    {
        public static IEnumerable<object[]> TestEnvironmentVariableReader()
        {
            return GetTestEnvironmentVariableReader();
        }

        public static IEnumerable<object[]> TestEnvironmentVariableReader(object value1)
        {
            return GetTestEnvironmentVariableReader(value1);
        }

        public static IEnumerable<object[]> TestEnvironmentVariableReader(object value1, object value2)
        {
            return GetTestEnvironmentVariableReader(value1, value2);
        }

        public static IEnumerable<object[]> TestEnvironmentVariableReader(object value1, object value2, object value3)
        {
            return GetTestEnvironmentVariableReader(value1, value2, value3);
        }

        private static IEnumerable<object[]> GetTestEnvironmentVariableReader(params object[] objects)
        {
            var UseNjForFileTrue = new List<object> {
                new TestEnvironmentVariableReader(
                    new Dictionary<string, string>()
                    {
                        [JsonUtility.NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING] = bool.TrueString
                    }, "NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING: true")
                };
            var UseNjForFileFalse = new List<object> {
                new TestEnvironmentVariableReader(
                    new Dictionary<string, string>()
                    {
                        [JsonUtility.NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING] = bool.FalseString
                    }, "NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING: false")
                };

            if (objects != null)
            {
                UseNjForFileFalse.AddRange(objects);
                UseNjForFileTrue.AddRange(objects);
            }

            return new List<object[]>
            {
                UseNjForFileTrue.ToArray(),
                UseNjForFileFalse.ToArray()
            };
        }
    }
}
