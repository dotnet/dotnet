// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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

        [Theory, MemberData(nameof(IssuerSerialComparisonData), DisableDiscoveryEnumeration = true)]
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

        [Theory, MemberData(nameof(IssuerSerialComparisonData), DisableDiscoveryEnumeration = true)]
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
