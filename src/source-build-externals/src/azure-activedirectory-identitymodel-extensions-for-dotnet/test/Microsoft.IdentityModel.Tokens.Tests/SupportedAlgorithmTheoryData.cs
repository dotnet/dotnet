// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class SupportedAlgorithmTheoryData : TheoryDataBase
    {
        public string Algorithm { get; set; }

        public string Digest { get; set; }

        public bool IsSupportedAlgorithm { get; set; } = true;

        public SecurityKey SecurityKey { get; set; }

        public static void AddTestCase(string algorithm, SecurityKey securityKey, string testId, TheoryData<SupportedAlgorithmTheoryData> theoryData, ExpectedException expectedException = null)
        {
            AddTestCase(algorithm, securityKey, true, testId, theoryData, expectedException);
        }

        public static void AddTestCase(string algorithm, SecurityKey securityKey, bool isSupportedAlgorithm, string testId, TheoryData<SupportedAlgorithmTheoryData> theoryData, ExpectedException expectedException = null)
        {
            theoryData.Add(new SupportedAlgorithmTheoryData
            {
                Algorithm = algorithm,
                ExpectedException = expectedException ?? ExpectedException.NoExceptionExpected,
                IsSupportedAlgorithm = isSupportedAlgorithm,
                SecurityKey = securityKey,
                TestId = testId
            });
        }
    }
}
