// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.TestUtils;

namespace Microsoft.IdentityModel.Tokens.Json.Tests
{
    public class JsonWebKeyTheoryData : TheoryDataBase
    {
        public JsonWebKeyTheoryData() { }

        public JsonWebKeyTheoryData(string testId) : base(testId) { }

        public string Crv { get; set; }

        public string D { get; set; }

        public string Json { get; set; }

        public JsonWebKey JsonWebKey { get; set; }

        public ExpectedException JsonReaderExpectedException { get; set; } = ExpectedException.NoExceptionExpected;

        public ExpectedException JsonSerializerExpectedException { get; set; } = ExpectedException.NoExceptionExpected;

        public string X { get; set; }

        public string Y { get; set; }

        public bool UsePrivateKey { get; set; }
    }
}
