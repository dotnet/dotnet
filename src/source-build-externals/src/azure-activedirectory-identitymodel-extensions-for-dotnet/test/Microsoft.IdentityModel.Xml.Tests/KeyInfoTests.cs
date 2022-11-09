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
    public class KeyInfoTests
    {
        [Fact]
        public void GetSets()
        {
            var type = typeof(KeyInfo);
            var properties = type.GetProperties();
            Assert.True(properties.Length == 6, $"Number of properties has changed from 6 to: {properties.Length}, adjust tests");

            var keyInfo = new KeyInfo();
            var context = new GetSetContext
            {
                PropertyNamesAndSetGetValue = new List<KeyValuePair<string, List<object>>>
                {
                    new KeyValuePair<string, List<object>>("Id", new List<object>{(string)null, Guid.NewGuid().ToString()}),
                    new KeyValuePair<string, List<object>>("Prefix", new List<object>{"", Guid.NewGuid().ToString()}),
                    new KeyValuePair<string, List<object>>("RetrievalMethodUri", new List<object>{(string)null, Guid.NewGuid().ToString()}),
                    new KeyValuePair<string, List<object>>("RSAKeyValue", new List<object>{(RSAKeyValue)null, new RSAKeyValue(Guid.NewGuid().ToString(), Guid.NewGuid().ToString())}),
                    new KeyValuePair<string, List<object>>("X509Data", new List<object>{keyInfo.X509Data, new List<X509Data>()}),
                    new KeyValuePair<string, List<object>>("KeyName", new List<object>{(string)null, Guid.NewGuid().ToString()}),

                },
                Object = keyInfo
            };

            TestUtilities.GetSet(context);
            TestUtilities.AssertFailIfErrors($"{this}.GetSets", context.Errors);
        }

        [Fact]
        public void KeyInfo_ListCollectionTests()
        {
            var keyInfo = new KeyInfo();
            var secondKeyInfo = new KeyInfo()
            {
                KeyName = "anotherKeyName",
                RetrievalMethodUri = "anotherRetrievalMethodUri",
                RSAKeyValue = new RSAKeyValue(string.Empty, string.Empty),
            };

            secondKeyInfo.X509Data.Add(new X509Data(ReferenceMetadata.X509Certificate1));

            var list = new List<KeyInfo> { keyInfo, secondKeyInfo };
            var secondList = new List<KeyInfo> { keyInfo, secondKeyInfo };

            Assert.True(Enumerable.SequenceEqual(list, secondList));
        }

        [Fact]
        public void KeyInfo_HashCodeCollectionTests()
        {
            var set = new HashSet<KeyInfo>();

            var keyInfo = new KeyInfo();

            set.Add(keyInfo);

            // modify each property to check that hashcode is stable
            keyInfo.KeyName = "anotherKeyName";
            keyInfo.RetrievalMethodUri = "anotherRetrievalMethodUri";
            keyInfo.RSAKeyValue = new RSAKeyValue(string.Empty, string.Empty);
            keyInfo.X509Data.Add(new X509Data(ReferenceMetadata.X509Certificate1));

            bool inCollection = set.Contains(keyInfo);
            Assert.True(inCollection);
        }

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

        [Theory, MemberData(nameof(KeyInfoDataComparisonData))]
        public void KeyInfo_HashCodeTests(KeyInfoComparisonTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.${nameof(KeyInfo_HashCodeTests)}", theoryData);
            try
            {
                var firstHashCode = theoryData.FirstKeyInfo.GetHashCode();
                var secondHashCode = theoryData.SecondKeyInfo.GetHashCode();

                Assert.Equal(theoryData.HashShouldMatch, firstHashCode.Equals(secondHashCode));
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }


        [Theory, MemberData(nameof(KeyInfoDataComparisonData))]
        public void KeyInfo_EqualsTests(KeyInfoComparisonTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.{nameof(KeyInfo_EqualsTests)}", theoryData);
            try
            {
                Assert.Equal(theoryData.ShouldBeConsideredEqual, theoryData.FirstKeyInfo.Equals(theoryData.SecondKeyInfo));
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<KeyInfoComparisonTheoryData> KeyInfoDataComparisonData
        {
            get
            {
                return new TheoryData<KeyInfoComparisonTheoryData>
                {
                    new KeyInfoComparisonTheoryData
                    {
                        TestId = "Matching_empty",
                        FirstKeyInfo = new KeyInfo(),
                        SecondKeyInfo = new KeyInfo(),
                        ShouldBeConsideredEqual = true,
                        // Hashcode will never match as the only immutable field is a reference that will always differ
                        HashShouldMatch = false,
                    },
                    new KeyInfoComparisonTheoryData
                    {
                        TestId = "Matching_KeyName",
                        FirstKeyInfo = new KeyInfo()
                        {
                            KeyName = "KeyNameSampleString"
                        },
                        SecondKeyInfo = new KeyInfo()
                        {
                            KeyName = "KeyNameSampleString"
                        },
                        ShouldBeConsideredEqual = true,
                    },
                    new KeyInfoComparisonTheoryData
                    {
                        TestId = "Nonmatching_KeyName",
                        FirstKeyInfo = new KeyInfo()
                        {
                            KeyName = "KeyNameSampleString"
                        },
                        SecondKeyInfo = new KeyInfo()
                        {
                            KeyName = "AnotherKeyNameSampleString"
                        },
                    },
                    new KeyInfoComparisonTheoryData
                    {
                        TestId = "Matching_RetrievalMethodUri",
                        FirstKeyInfo = new KeyInfo()
                        {
                            RetrievalMethodUri = "RetrievalMethodUriSampleString"
                        },
                        SecondKeyInfo = new KeyInfo()
                        {
                            RetrievalMethodUri = "RetrievalMethodUriSampleString"
                        },
                        ShouldBeConsideredEqual = true,
                    },
                    new KeyInfoComparisonTheoryData
                    {
                        TestId = "Nonmatching_RetrievalMethodUri",
                        FirstKeyInfo = new KeyInfo()
                        {
                            RetrievalMethodUri = "RetrievalMethodUriSampleString"
                        },
                        SecondKeyInfo = new KeyInfo()
                        {
                            RetrievalMethodUri = "AnotherRetrievalMethodUriSampleString"
                        },
                    },
                    new KeyInfoComparisonTheoryData
                    {
                        TestId = "Matching_RSAKeyValue",
                        FirstKeyInfo = new KeyInfo()
                        {
                            RSAKeyValue = new RSAKeyValue(
                            "rCz8Sn3GGXmikH2MdTeGY1D711EORX/lVXpr+ecGgqfUWF8MPB07XkYuJ54DAuYT318+2XrzMjOtqkT94VkXmxv6dFGhG8YZ8vNMPd4tdj9c0lpvWQdqXtL1TlFRpD/P6UMEigfN0c9oWDg9U7Ilymgei0UXtf1gtcQbc5sSQU0S4vr9YJp2gLFIGK11Iqg4XSGdcI0QWLLkkC6cBukhVnd6BCYbLjTYy3fNs4DzNdemJlxGl8sLexFytBF6YApvSdus3nFXaMCtBGx16HzkK9ne3lobAwL2o79bP4imEGqg+ibvyNmbrwFGnQrBc1jTF9LyQX9q+louxVfHs6ZiVw==",
                            "AQAB"),
                        },
                        SecondKeyInfo = new KeyInfo()
                        {
                            RSAKeyValue = new RSAKeyValue(
                            "rCz8Sn3GGXmikH2MdTeGY1D711EORX/lVXpr+ecGgqfUWF8MPB07XkYuJ54DAuYT318+2XrzMjOtqkT94VkXmxv6dFGhG8YZ8vNMPd4tdj9c0lpvWQdqXtL1TlFRpD/P6UMEigfN0c9oWDg9U7Ilymgei0UXtf1gtcQbc5sSQU0S4vr9YJp2gLFIGK11Iqg4XSGdcI0QWLLkkC6cBukhVnd6BCYbLjTYy3fNs4DzNdemJlxGl8sLexFytBF6YApvSdus3nFXaMCtBGx16HzkK9ne3lobAwL2o79bP4imEGqg+ibvyNmbrwFGnQrBc1jTF9LyQX9q+louxVfHs6ZiVw==",
                            "AQAB"),
                        },
                        ShouldBeConsideredEqual = true,
                    },
                    new KeyInfoComparisonTheoryData
                    {
                        TestId = "Nonmatching_RSAKeyValue",
                        FirstKeyInfo = new KeyInfo()
                        {
                            RSAKeyValue = new RSAKeyValue(
                            "rCz8Sn3GGXmikH2MdTeGY1D711EORX/lVXpr+ecGgqfUWF8MPB07XkYuJ54DAuYT318+2XrzMjOtqkT94VkXmxv6dFGhG8YZ8vNMPd4tdj9c0lpvWQdqXtL1TlFRpD/P6UMEigfN0c9oWDg9U7Ilymgei0UXtf1gtcQbc5sSQU0S4vr9YJp2gLFIGK11Iqg4XSGdcI0QWLLkkC6cBukhVnd6BCYbLjTYy3fNs4DzNdemJlxGl8sLexFytBF6YApvSdus3nFXaMCtBGx16HzkK9ne3lobAwL2o79bP4imEGqg+ibvyNmbrwFGnQrBc1jTF9LyQX9q+louxVfHs6ZiVw==",
                            "AQAB"),
                        },
                        SecondKeyInfo = new KeyInfo()
                        {
                            RSAKeyValue = new RSAKeyValue(
                            "rCz8Sn3GGXmikH2MdTeGY1D711EORX/lVXpr+ecGgqfUWF8MPB07XkYuJ54DAuYT318+2XrzMjOtqkT94VkXmxv6dFGhG8YZ8vNMPd4tdj9c0lpvWQdqXtL1TlFRpD/P6UMEigfN0c9oWDg9U7Ilymgei0UXtf1gtcQbc5sSQU0S4vr9YJp2gLFIGK11Iqg4XSGdcI0QWLLkkC6cBukhVnd6BCYbLjTYy3fNs4DzNdemJlxGl8sLexFytBF6YApvSdus3nFXaMCtBGx16HzkK9ne3lobAwL2o79bP4imEGqg+ibvyNmbrwFGnQrBc1jTF9LyQX9q+louxVfHs6ZiVw==",
                            string.Empty),
                        },
                    },
                    new KeyInfoComparisonTheoryData
                    {
                        TestId = "Matching_X509Data",
                        FirstKeyInfo = new KeyInfo(ReferenceMetadata.X509Certificate1),
                        SecondKeyInfo = new KeyInfo(ReferenceMetadata.X509Certificate1),
                        ShouldBeConsideredEqual = true,
                    },
                    new KeyInfoComparisonTheoryData
                    {
                        TestId = "Nonmatching_X509Data",
                        FirstKeyInfo = new KeyInfo(ReferenceMetadata.X509Certificate1),
                        SecondKeyInfo = new KeyInfo(ReferenceMetadata.X509Certificate2),
                    },
                };
            }
        }


#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
    }

    public class KeyInfoComparisonTheoryData : TheoryDataBase
    {
        public KeyInfo FirstKeyInfo { get; set; }

        public KeyInfo SecondKeyInfo { get; set; }

        public bool HashShouldMatch { get; set; }

        public bool ShouldBeConsideredEqual { get; set; }
    }

    public class KeyInfoTheoryData : TheoryDataBase
    {
        public DSigSerializer Serializer
        {
            get;
            set;
        } = new DSigSerializer();

        public KeyInfo KeyInfo
        {
            get;
            set;
        }

        public string Xml
        {
            get;
            set;
        }
    }
}
