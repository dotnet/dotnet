// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.TestUtils;

namespace Microsoft.IdentityModel.JsonWebTokens.Tests
{
    public class GetPayloadValueTheoryData : TheoryDataBase
    {
        public GetPayloadValueTheoryData(string testId) : base(testId)
        { }

        public object ClaimValue { get; set; }

        public string PropertyName { get; set; }

        public Type PropertyType { get; set; }

        public object PropertyValue { get; set; }

        public string Json { get; set; }
    }
}
