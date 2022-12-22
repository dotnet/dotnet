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
using System.Collections.Generic;
using System.Linq;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

namespace Microsoft.IdentityModel.Xml.Tests
{
#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
    public class IssuerSerialTests
    {
        [Fact]
        public void IssuerSerial_HashSetCollectionTests()
        {
            var set = new HashSet<IssuerSerial>();

            var issuerSerial = new IssuerSerial(string.Empty, string.Empty);

            set.Add(issuerSerial);

            bool inCollection = set.Contains(issuerSerial);
            Assert.True(inCollection);

            var secondIssuerSerial = new IssuerSerial(string.Empty, string.Empty);

            // hashcode is determined by immutable values, not reference
            inCollection = set.Contains(secondIssuerSerial);
            Assert.True(inCollection);
        }

        [Fact]
        public void IssuerSerial_ListCollectionTests()
        {
            var issuerSerial = new IssuerSerial(string.Empty, string.Empty);
            var secondIssuerSerial = new IssuerSerial("issuerName", "serialNumber");

            var list = new List<IssuerSerial> { issuerSerial, secondIssuerSerial };
            var secondList = new List<IssuerSerial> { issuerSerial, secondIssuerSerial };

            Assert.True(Enumerable.SequenceEqual(list, secondList));
        }

        [Theory, MemberData(nameof(IssuerSerialComparisonData))]
        public void IssuerSerial_HashCodeTests(IssuerSerialComparisonTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.{nameof(IssuerSerial_HashCodeTests)}", theoryData);
            try
            {
                var firstHashCode = theoryData.FirstIssuerSerial.GetHashCode();
                var secondHashCode = theoryData.SecondIssuerSerial.GetHashCode();

                Assert.Equal(theoryData.ShouldMatch, firstHashCode.Equals(secondHashCode));
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(IssuerSerialComparisonData))]
        public void IssuerSerial_EqualsTests(IssuerSerialComparisonTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.{nameof(IssuerSerial_EqualsTests)}", theoryData);
            try
            {
                Assert.Equal(theoryData.ShouldMatch, theoryData.FirstIssuerSerial.Equals(theoryData.SecondIssuerSerial));
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<IssuerSerialComparisonTheoryData> IssuerSerialComparisonData
        {
            get
            {
                return new TheoryData<IssuerSerialComparisonTheoryData>
                {
                    new IssuerSerialComparisonTheoryData
                    {
                        TestId = "Matching_empty",
                        FirstIssuerSerial = new IssuerSerial(string.Empty, string.Empty),
                        SecondIssuerSerial = new IssuerSerial(string.Empty, string.Empty),
                        ShouldMatch = true,
                    },
                    new IssuerSerialComparisonTheoryData
                    {
                        TestId = "Matching_NotEmpty",
                        FirstIssuerSerial = new IssuerSerial("IssuerName", "SerialNumber"),
                        SecondIssuerSerial = new IssuerSerial("IssuerName", "SerialNumber"),
                        ShouldMatch = true,
                    },
                    new IssuerSerialComparisonTheoryData
                    {
                        TestId = "NotMatching_EmptySerialNumber",
                        FirstIssuerSerial = new IssuerSerial("IssuerName", string.Empty),
                        SecondIssuerSerial = new IssuerSerial("IssuerName", "SerialNumber"),
                    },
                    new IssuerSerialComparisonTheoryData
                    {
                        TestId = "NotMatching_EmptyIssuerName",
                        FirstIssuerSerial = new IssuerSerial(string.Empty, "SerialNumber"),
                        SecondIssuerSerial = new IssuerSerial("IssuerName", "SerialNumber"),
                    },
                    new IssuerSerialComparisonTheoryData
                    {
                        TestId = "NotMatching_DifferentIssuerName",
                        FirstIssuerSerial = new IssuerSerial("DifferentIssuerName", "SerialNumber"),
                        SecondIssuerSerial = new IssuerSerial("IssuerName", "SerialNumber"),
                    },
                    new IssuerSerialComparisonTheoryData
                    {
                        TestId = "NotMatching_DifferentSerialNumber",
                        FirstIssuerSerial = new IssuerSerial("IssuerName", "DifferentSerialNumber"),
                        SecondIssuerSerial = new IssuerSerial("IssuerName", "SerialNumber"),
                    },
                    new IssuerSerialComparisonTheoryData
                    {
                        TestId = "NotMatching_DifferentIssuerNameAndSerialNumber",
                        FirstIssuerSerial = new IssuerSerial("DifferentIssuerName", "DifferentSerialNumber"),
                        SecondIssuerSerial = new IssuerSerial("IssuerName", "SerialNumber"),
                    },
                };
            }
        }

        public class IssuerSerialComparisonTheoryData : TheoryDataBase
        {
            public IssuerSerial FirstIssuerSerial { get; set; }

            public IssuerSerial SecondIssuerSerial { get; set; }

            public bool ShouldMatch { get; set; }
        }
    }
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
}
