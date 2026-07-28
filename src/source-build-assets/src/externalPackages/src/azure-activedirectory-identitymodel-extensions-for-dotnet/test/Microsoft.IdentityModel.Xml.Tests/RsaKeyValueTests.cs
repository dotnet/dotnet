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
    public class RsaKeyValueTests
    {
        [Fact]
        public void RsaKeyValue_ListCollectionTests()
        {
            var rsaKeyValue = new RSAKeyValue(string.Empty, string.Empty);
            var secondrsaKeyValue = new RSAKeyValue("modulus", "exponent");

            var list = new List<RSAKeyValue> { rsaKeyValue, secondrsaKeyValue };
            var secondList = new List<RSAKeyValue> { rsaKeyValue, secondrsaKeyValue };

            Assert.True(Enumerable.SequenceEqual(list, secondList));
        }

        [Fact]
        public void RsaKeyValue_HashSetCollectionTests()
        {
            var set = new HashSet<RSAKeyValue>();
            var rsaKeyValue = new RSAKeyValue(string.Empty, string.Empty);

            set.Add(rsaKeyValue);

            bool inCollection = set.Contains(rsaKeyValue);
            Assert.True(inCollection);

            var secondRSAKeyValue = new RSAKeyValue(string.Empty, string.Empty);

            // hashcode is determined by immutable property values, not reference
            inCollection = set.Contains(secondRSAKeyValue);
            Assert.True(inCollection);
        }

        [Theory, MemberData(nameof(IssuerSerialComparisonData), DisableDiscoveryEnumeration = true)]
        public void RsaKeyValue_HashCodeTests(RsaKeyValueComparisonTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.{nameof(RsaKeyValue_HashCodeTests)}", theoryData);
            try
            {
                var firstHashCode = theoryData.FirstRsaKeyValue.GetHashCode();
                var secondHashCode = theoryData.SecondRsaKeyValue.GetHashCode();

                Assert.Equal(theoryData.ShouldMatchHashCode, firstHashCode.Equals(secondHashCode));
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(IssuerSerialComparisonData), DisableDiscoveryEnumeration = true)]
        public void RsaKeyValue_EqualsTests(RsaKeyValueComparisonTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.{nameof(RsaKeyValue_EqualsTests)}", theoryData);
            try
            {
                Assert.Equal(theoryData.ShouldBeConsideredEqual, theoryData.FirstRsaKeyValue.Equals(theoryData.SecondRsaKeyValue));
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<RsaKeyValueComparisonTheoryData> IssuerSerialComparisonData
        {
            get
            {
                return new TheoryData<RsaKeyValueComparisonTheoryData>
                {
                    new RsaKeyValueComparisonTheoryData
                    {
                        TestId = "Matching_empty",
                        FirstRsaKeyValue = new RSAKeyValue(string.Empty, string.Empty),
                        SecondRsaKeyValue = new RSAKeyValue(string.Empty, string.Empty),
                        ShouldMatchHashCode = true,
                        ShouldBeConsideredEqual = true,
                    },
                    new RsaKeyValueComparisonTheoryData
                    {
                        TestId = "Matching_ModulusAndExponent",
                        FirstRsaKeyValue = new RSAKeyValue(
                            "rCz8Sn3GGXmikH2MdTeGY1D711EORX/lVXpr+ecGgqfUWF8MPB07XkYuJ54DAuYT318+2XrzMjOtqkT94VkXmxv6dFGhG8YZ8vNMPd4tdj9c0lpvWQdqXtL1TlFRpD/P6UMEigfN0c9oWDg9U7Ilymgei0UXtf1gtcQbc5sSQU0S4vr9YJp2gLFIGK11Iqg4XSGdcI0QWLLkkC6cBukhVnd6BCYbLjTYy3fNs4DzNdemJlxGl8sLexFytBF6YApvSdus3nFXaMCtBGx16HzkK9ne3lobAwL2o79bP4imEGqg+ibvyNmbrwFGnQrBc1jTF9LyQX9q+louxVfHs6ZiVw==",
                            "AQAB"),
                        SecondRsaKeyValue = new RSAKeyValue(
                            "rCz8Sn3GGXmikH2MdTeGY1D711EORX/lVXpr+ecGgqfUWF8MPB07XkYuJ54DAuYT318+2XrzMjOtqkT94VkXmxv6dFGhG8YZ8vNMPd4tdj9c0lpvWQdqXtL1TlFRpD/P6UMEigfN0c9oWDg9U7Ilymgei0UXtf1gtcQbc5sSQU0S4vr9YJp2gLFIGK11Iqg4XSGdcI0QWLLkkC6cBukhVnd6BCYbLjTYy3fNs4DzNdemJlxGl8sLexFytBF6YApvSdus3nFXaMCtBGx16HzkK9ne3lobAwL2o79bP4imEGqg+ibvyNmbrwFGnQrBc1jTF9LyQX9q+louxVfHs6ZiVw==",
                            "AQAB"),
                        ShouldMatchHashCode = true,
                        ShouldBeConsideredEqual = true,
                    },
                    new RsaKeyValueComparisonTheoryData
                    {
                        TestId = "NotMatching_EmptyModulus",
                        FirstRsaKeyValue = new RSAKeyValue(string.Empty, "AQAB"),
                        SecondRsaKeyValue = new RSAKeyValue(
                            "rCz8Sn3GGXmikH2MdTeGY1D711EORX/lVXpr+ecGgqfUWF8MPB07XkYuJ54DAuYT318+2XrzMjOtqkT94VkXmxv6dFGhG8YZ8vNMPd4tdj9c0lpvWQdqXtL1TlFRpD/P6UMEigfN0c9oWDg9U7Ilymgei0UXtf1gtcQbc5sSQU0S4vr9YJp2gLFIGK11Iqg4XSGdcI0QWLLkkC6cBukhVnd6BCYbLjTYy3fNs4DzNdemJlxGl8sLexFytBF6YApvSdus3nFXaMCtBGx16HzkK9ne3lobAwL2o79bP4imEGqg+ibvyNmbrwFGnQrBc1jTF9LyQX9q+louxVfHs6ZiVw==",
                            "AQAB"),
                    },
                    new RsaKeyValueComparisonTheoryData
                    {
                        TestId = "NotMatching_EmptyExponent",
                        FirstRsaKeyValue = new RSAKeyValue(
                            "rCz8Sn3GGXmikH2MdTeGY1D711EORX/lVXpr+ecGgqfUWF8MPB07XkYuJ54DAuYT318+2XrzMjOtqkT94VkXmxv6dFGhG8YZ8vNMPd4tdj9c0lpvWQdqXtL1TlFRpD/P6UMEigfN0c9oWDg9U7Ilymgei0UXtf1gtcQbc5sSQU0S4vr9YJp2gLFIGK11Iqg4XSGdcI0QWLLkkC6cBukhVnd6BCYbLjTYy3fNs4DzNdemJlxGl8sLexFytBF6YApvSdus3nFXaMCtBGx16HzkK9ne3lobAwL2o79bP4imEGqg+ibvyNmbrwFGnQrBc1jTF9LyQX9q+louxVfHs6ZiVw==",
                            string.Empty),
                        SecondRsaKeyValue = new RSAKeyValue(
                            "rCz8Sn3GGXmikH2MdTeGY1D711EORX/lVXpr+ecGgqfUWF8MPB07XkYuJ54DAuYT318+2XrzMjOtqkT94VkXmxv6dFGhG8YZ8vNMPd4tdj9c0lpvWQdqXtL1TlFRpD/P6UMEigfN0c9oWDg9U7Ilymgei0UXtf1gtcQbc5sSQU0S4vr9YJp2gLFIGK11Iqg4XSGdcI0QWLLkkC6cBukhVnd6BCYbLjTYy3fNs4DzNdemJlxGl8sLexFytBF6YApvSdus3nFXaMCtBGx16HzkK9ne3lobAwL2o79bP4imEGqg+ibvyNmbrwFGnQrBc1jTF9LyQX9q+louxVfHs6ZiVw==",
                            "AQAB"),
                    },
                    new RsaKeyValueComparisonTheoryData
                    {
                        TestId = "NotMatching_DifferentExponent",
                        FirstRsaKeyValue = new RSAKeyValue(
                            "rCz8Sn3GGXmikH2MdTeGY1D711EORX/lVXpr+ecGgqfUWF8MPB07XkYuJ54DAuYT318+2XrzMjOtqkT94VkXmxv6dFGhG8YZ8vNMPd4tdj9c0lpvWQdqXtL1TlFRpD/P6UMEigfN0c9oWDg9U7Ilymgei0UXtf1gtcQbc5sSQU0S4vr9YJp2gLFIGK11Iqg4XSGdcI0QWLLkkC6cBukhVnd6BCYbLjTYy3fNs4DzNdemJlxGl8sLexFytBF6YApvSdus3nFXaMCtBGx16HzkK9ne3lobAwL2o79bP4imEGqg+ibvyNmbrwFGnQrBc1jTF9LyQX9q+louxVfHs6ZiVw==",
                            "differentExponent"),
                        SecondRsaKeyValue = new RSAKeyValue(
                            "rCz8Sn3GGXmikH2MdTeGY1D711EORX/lVXpr+ecGgqfUWF8MPB07XkYuJ54DAuYT318+2XrzMjOtqkT94VkXmxv6dFGhG8YZ8vNMPd4tdj9c0lpvWQdqXtL1TlFRpD/P6UMEigfN0c9oWDg9U7Ilymgei0UXtf1gtcQbc5sSQU0S4vr9YJp2gLFIGK11Iqg4XSGdcI0QWLLkkC6cBukhVnd6BCYbLjTYy3fNs4DzNdemJlxGl8sLexFytBF6YApvSdus3nFXaMCtBGx16HzkK9ne3lobAwL2o79bP4imEGqg+ibvyNmbrwFGnQrBc1jTF9LyQX9q+louxVfHs6ZiVw==",
                            "AQAB"),
                    },
                    new RsaKeyValueComparisonTheoryData
                    {
                        TestId = "NotMatching_DifferentModulus",
                        FirstRsaKeyValue = new RSAKeyValue(
                            "differentModulus",
                            "AQAB"),
                        SecondRsaKeyValue = new RSAKeyValue(
                            "rCz8Sn3GGXmikH2MdTeGY1D711EORX/lVXpr+ecGgqfUWF8MPB07XkYuJ54DAuYT318+2XrzMjOtqkT94VkXmxv6dFGhG8YZ8vNMPd4tdj9c0lpvWQdqXtL1TlFRpD/P6UMEigfN0c9oWDg9U7Ilymgei0UXtf1gtcQbc5sSQU0S4vr9YJp2gLFIGK11Iqg4XSGdcI0QWLLkkC6cBukhVnd6BCYbLjTYy3fNs4DzNdemJlxGl8sLexFytBF6YApvSdus3nFXaMCtBGx16HzkK9ne3lobAwL2o79bP4imEGqg+ibvyNmbrwFGnQrBc1jTF9LyQX9q+louxVfHs6ZiVw==",
                            "AQAB"),
                    },
                    new RsaKeyValueComparisonTheoryData
                    {
                        TestId = "NotMatching_DifferentModulusAndExponent",
                        FirstRsaKeyValue = new RSAKeyValue(
                            "differentModulus",
                            "differentExponent"),
                        SecondRsaKeyValue = new RSAKeyValue(
                            "rCz8Sn3GGXmikH2MdTeGY1D711EORX/lVXpr+ecGgqfUWF8MPB07XkYuJ54DAuYT318+2XrzMjOtqkT94VkXmxv6dFGhG8YZ8vNMPd4tdj9c0lpvWQdqXtL1TlFRpD/P6UMEigfN0c9oWDg9U7Ilymgei0UXtf1gtcQbc5sSQU0S4vr9YJp2gLFIGK11Iqg4XSGdcI0QWLLkkC6cBukhVnd6BCYbLjTYy3fNs4DzNdemJlxGl8sLexFytBF6YApvSdus3nFXaMCtBGx16HzkK9ne3lobAwL2o79bP4imEGqg+ibvyNmbrwFGnQrBc1jTF9LyQX9q+louxVfHs6ZiVw==",
                            "AQAB"),
                    },
                    new RsaKeyValueComparisonTheoryData
                    {
                        TestId = "ModulusAndExponentAreCaseInsensitive",
                        FirstRsaKeyValue = new RSAKeyValue(
                            "modulus",
                            "exponent"),
                        SecondRsaKeyValue = new RSAKeyValue(
                            "MODULUS",
                            "EXPONENT"),
                        ShouldBeConsideredEqual = true,
                    },
                };
            }
        }

        public class RsaKeyValueComparisonTheoryData : TheoryDataBase
        {
            public RSAKeyValue FirstRsaKeyValue { get; set; }

            public RSAKeyValue SecondRsaKeyValue { get; set; }

            public bool ShouldMatchHashCode { get; set; }

            public bool ShouldBeConsideredEqual { get; set; }
        }
    }
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
}
