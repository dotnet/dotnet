// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.TestUtils;
using System.Collections.Generic;

namespace Microsoft.IdentityModel.Benchmarks
{
    public class BenchmarkUtils
    {
        public static Dictionary<string, object> SimpleClaims
        {
            get => new Dictionary<string, object>()
            {
                { "role", new List<string>() { "role1", "Developer", "Sales"} },
                { "email", "Bob@contoso.com" },
                { "exp", EpochTime.GetIntDate(Default.Expires).ToString() },
                { "nbf", EpochTime.GetIntDate(Default.NotBefore).ToString() },
                { "iat", EpochTime.GetIntDate(Default.IssueInstant).ToString() }
            };
        }
    }
}
