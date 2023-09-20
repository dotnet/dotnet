// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.JsonWebTokens.Tests
{
    public class GetPayloadValueTheoryData : TheoryDataBase
    {
        public GetPayloadValueTheoryData(string testId) : base(testId)
        { }

        public SecurityTokenDescriptor SecurityTokenDescriptor { get; set; }

        public string PropertyName { get; set; }

        public Type PropertyOut { get; set; }

        public Type PropertyType { get; set; }

        public object PropertyValue { get; set; }

        public string Json { get; set; }
    }
}
