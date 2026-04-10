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
    public class X509DataTests
    {
        [Fact]
        public void X509Data_ListCollectionTests()
        {
            var x509Data = new X509Data()
            {
                SKI = "anotherSKI",
                SubjectName = "anotherSubjectName",
                CRL = "anotherCRL",
                IssuerSerial = new IssuerSerial(string.Empty, string.Empty),
            };
            x509Data.Certificates.Add(ReferenceMetadata.X509CertificateData1);

            var secondx509Data = new X509Data();

            var list = new List<X509Data> { x509Data, secondx509Data };
            var secondList = new List<X509Data> { x509Data, secondx509Data };

            Assert.True(Enumerable.SequenceEqual(list, secondList));
        }

        [Fact]
        public void X509Data_HashSetCollectionTests()
        {
            var set = new HashSet<X509Data>();

            var x509Data = new X509Data();

            set.Add(x509Data);

            // modify each property to check that hashcode is stable
            x509Data.SKI = "anotherSKI";
            x509Data.SubjectName = "anotherSubjectName";
            x509Data.CRL = "anotherCRL";
            x509Data.IssuerSerial = new IssuerSerial(string.Empty, string.Empty);
            x509Data.Certificates.Add(ReferenceMetadata.X509CertificateData1);

            bool inCollection = set.Contains(x509Data);
            Assert.True(inCollection);
        }

        [Theory, MemberData(nameof(X509DataComparisonData))]
        public void X509Data_HashCodeTests(X509DataComparisonTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.{nameof(X509Data_HashCodeTests)}", theoryData);
            try
            {
                var firstHashCode = theoryData.FirstX509Data.GetHashCode();
                var secondHashCode = theoryData.SecondX509Data.GetHashCode();

                Assert.Equal(theoryData.HashShouldMatch, firstHashCode.Equals(secondHashCode));
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }


        [Theory, MemberData(nameof(X509DataComparisonData))]
        public void X509Data_EqualsTests(X509DataComparisonTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.{nameof(X509Data_EqualsTests)}", theoryData);
            try
            {
                Assert.Equal(theoryData.ShouldBeConsideredEqual, theoryData.FirstX509Data.Equals(theoryData.SecondX509Data));
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<X509DataComparisonTheoryData> X509DataComparisonData
        {
            get
            {
                return new TheoryData<X509DataComparisonTheoryData>
                {
                    new X509DataComparisonTheoryData
                    {
                        TestId = "Matching_empty",
                        FirstX509Data = new X509Data(),
                        SecondX509Data = new X509Data(),
                        ShouldBeConsideredEqual = true,
                        // Hash will always differ
                        HashShouldMatch = false,
                    },
                    new X509DataComparisonTheoryData
                    {
                        TestId = "Matching_Certificates",
                        FirstX509Data = new X509Data(ReferenceMetadata.X509Certificate1),
                        SecondX509Data = new X509Data(ReferenceMetadata.X509Certificate1),
                        ShouldBeConsideredEqual = true,
                    },
                    new X509DataComparisonTheoryData
                    {
                        TestId = "Nonmatching_Certificates",
                        FirstX509Data = new X509Data(ReferenceMetadata.X509Certificate1),
                        SecondX509Data = new X509Data(ReferenceMetadata.X509Certificate2),
                    },
                    new X509DataComparisonTheoryData
                    {
                        TestId = "Matching_MultipleCertificates",
                        FirstX509Data = new X509Data(new [] { ReferenceMetadata.X509Certificate1, ReferenceMetadata.X509Certificate2 }),
                        SecondX509Data = new X509Data(new [] { ReferenceMetadata.X509Certificate1, ReferenceMetadata.X509Certificate2 }),
                        ShouldBeConsideredEqual = true,
                    },
                    new X509DataComparisonTheoryData
                    {
                        TestId = "Nonmatching_MultipleCertificates",
                        FirstX509Data = new X509Data(new [] { ReferenceMetadata.X509Certificate1, ReferenceMetadata.X509Certificate2 }),
                        SecondX509Data = new X509Data(new [] { ReferenceMetadata.X509Certificate1, ReferenceMetadata.X509Certificate3 }),
                    },
                    new X509DataComparisonTheoryData
                    {
                        TestId = "Matching_SKI",
                        FirstX509Data = new X509Data()
                        {
                            SKI = "SKISampleString"
                        },
                        SecondX509Data = new X509Data()
                        {
                            SKI = "SKISampleString"
                        },
                        ShouldBeConsideredEqual = true,
                    },
                    new X509DataComparisonTheoryData
                    {
                        TestId = "Nonmatching_SKI",
                        FirstX509Data = new X509Data()
                        {
                            SKI = "SKISampleString"
                        },
                        SecondX509Data = new X509Data()
                        {
                            SKI = "AnotherSKISampleString"
                        },
                    },
                    new X509DataComparisonTheoryData
                    {
                        TestId = "Matching_CRL",
                        FirstX509Data = new X509Data()
                        {
                            CRL = "CRLSampleString"
                        },
                        SecondX509Data = new X509Data()
                        {
                            CRL = "CRLSampleString"
                        },
                        ShouldBeConsideredEqual = true,
                    },
                    new X509DataComparisonTheoryData
                    {
                        TestId = "Nonmatching_CRL",
                        FirstX509Data = new X509Data()
                        {
                            CRL = "CRLSampleString"
                        },
                        SecondX509Data = new X509Data()
                        {
                            CRL = "AnotherCRLSampleString"
                        },
                    },
                    new X509DataComparisonTheoryData
                    {
                        TestId = "Matching_IssuerSerial",
                        FirstX509Data = new X509Data()
                        {
                            IssuerSerial = new IssuerSerial("IssuerName", "SerialNumber"),
                        },
                        SecondX509Data = new X509Data()
                        {
                            IssuerSerial = new IssuerSerial("IssuerName", "SerialNumber"),
                        },
                        ShouldBeConsideredEqual = true,
                    },
                    new X509DataComparisonTheoryData
                    {
                        TestId = "Nonmatching_IssuerSerial",
                        FirstX509Data = new X509Data()
                        {
                            IssuerSerial = new IssuerSerial("IssuerName", "SerialNumber"),
                        },
                        SecondX509Data = new X509Data()
                        {
                            IssuerSerial = new IssuerSerial("AnotherIssuerName", "AnotherSerialNumber"),
                        },
                    }
                };
            }
        }

        public class X509DataComparisonTheoryData : TheoryDataBase
        {
            public X509Data FirstX509Data { get; set; }

            public X509Data SecondX509Data { get; set; }

            public bool ShouldBeConsideredEqual { get; set; }

            public bool HashShouldMatch { get; set; }
        }
    }
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
}
