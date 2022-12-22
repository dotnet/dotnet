//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

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
