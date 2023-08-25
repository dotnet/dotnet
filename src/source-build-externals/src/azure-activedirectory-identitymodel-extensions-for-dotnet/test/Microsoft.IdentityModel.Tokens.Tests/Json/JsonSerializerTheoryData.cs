// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Json.Tests;

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class JsonSerializerTheoryData : TheoryDataBase
    {
        public JsonSerializerTheoryData(string testId) : base(testId) { }

        // newtonsoft will deserialize types using a best fit algorithm, System.Text.Json will throw.
        // we set this flag to false when we want to skip the test of equality between the object deserialized
        // from newtonsoft as the object deserialized from System.Text.Json will be null.
        public bool CompareMicrosoftJson { get; set; } = true;

        public ExpectedException IdentityModelSerializerExpectedException { get; set; } = ExpectedException.NoExceptionExpected;

        public string Json { get; set; }

        public ExpectedException JsonReaderExpectedException { get; set; } = ExpectedException.NoExceptionExpected;

        public ExpectedException JsonSerializerExpectedException { get; set; } = ExpectedException.NoExceptionExpected;

        public JsonTestClass JsonTestClass { get; set; }

        public string PropertyName { get;  set; }

        public object Object { get; set; }

        public IDictionary<Type, IJsonSerializer> Serializers { get; set; } = new Dictionary<Type, IJsonSerializer>();
    }
}
