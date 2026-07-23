// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.TestUtils;

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class JsonSerializerTheoryData : TheoryDataBase
    {
        public JsonSerializerTheoryData(string testId) : base(testId)
        {
            PropertyName = testId;
        }

        public string Json { get; set; }

        public object ReadObject { get; set; }

        public ExpectedException JsonReaderExpectedException { get; set; } = ExpectedException.NoExceptionExpected;

        public ExpectedException JsonSerializerExpectedException { get; set; } = ExpectedException.NoExceptionExpected;

        public string PropertyName { get; set; }

        public object Object { get; set; }
    }
}
