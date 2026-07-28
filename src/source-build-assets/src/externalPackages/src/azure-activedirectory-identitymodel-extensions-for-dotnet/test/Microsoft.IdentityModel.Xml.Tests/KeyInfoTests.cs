// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
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

        [Theory, MemberData(nameof(KeyInfoDataComparisonData), DisableDiscoveryEnumeration = true)]
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

        [Theory, MemberData(nameof(KeyInfoDataComparisonData), DisableDiscoveryEnumeration = true)]
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

        [Theory, MemberData(nameof(KeyInfoMatchesTheoryData), DisableDiscoveryEnumeration = true)]
        public void KeyInfoMatchesKey(KeyInfoTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.{nameof(KeyInfoMatchesKey)}", theoryData);
            try
            {
                bool matches = theoryData.KeyInfo.MatchesKey(theoryData.SecurityKey);
                if (matches != theoryData.MatchesKey)
                    context.AddDiff($"KeyInfo.MatchesKey failed, Expected: '{theoryData.MatchesKey}', Actual: '{matches}', SecurityKey: '{theoryData.SecurityKey}', KeyInfo: '{theoryData.KeyInfo}'");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<KeyInfoTheoryData> KeyInfoMatchesTheoryData
        {
            get
            {
                X509Certificate2 x509CertificateWithSki = CertificateHelper.LoadX509Certificate(KeyingMaterial.SelfSignedWithSKIExtension_Public);
                X509ExtensionCollection extensions = x509CertificateWithSki.Extensions ?? throw new NotSupportedException("Extensions are null");
                X509SubjectKeyIdentifierExtension skiExtension = extensions["2.5.29.14"] as X509SubjectKeyIdentifierExtension;
                if (skiExtension == null)
                    throw new NotSupportedException("X509SubjectKeyIdentifierExtension is null");

                string ski = Convert.ToBase64String(skiExtension.RawData, 2, skiExtension.RawData.Length - 2);

                TheoryData<KeyInfoTheoryData> theoryData = new TheoryData<KeyInfoTheoryData>();

                theoryData.Add(new KeyInfoTheoryData("X509CertificateWithSKIExtension_UsingExtensibility")
                {
                    ExpectedException = ExpectedException.NoExceptionExpected,
                    KeyInfo = new CustomKeyInfo(ski),
                    SecurityKey = new X509SecurityKey(x509CertificateWithSki),
                    MatchesKey = true,
                });

                theoryData.Add(new KeyInfoTheoryData("X509CertificateWithoutSKIExtension_UsingExtensibility_NoMatch")
                {
                    ExpectedException = ExpectedException.NoExceptionExpected,
                    KeyInfo = new CustomKeyInfo(ski),
                    SecurityKey = new X509SecurityKey(KeyingMaterial.X509Certificate2),
                    MatchesKey = false,
                });

                theoryData.Add(new KeyInfoTheoryData("X509CertificateWithoutSKIExtension_UsingExtensibility")
                {
                    ExpectedException = ExpectedException.NoExceptionExpected,
                    KeyInfo = new CustomKeyInfo(KeyingMaterial.X509Certificate2),
                    SecurityKey = new X509SecurityKey(KeyingMaterial.X509Certificate2),
                    MatchesKey = true,
                });

                theoryData.Add(new KeyInfoTheoryData("X509CertificateWithSKIExtension")
                {
                    ExpectedException = ExpectedException.NoExceptionExpected,
                    KeyInfo = new KeyInfo(x509CertificateWithSki),
                    SecurityKey = new X509SecurityKey(x509CertificateWithSki),
                    MatchesKey = true,
                });

                theoryData.Add(new KeyInfoTheoryData("X509CertificateWithSKIExtension_NoMatch")
                {
                    ExpectedException = ExpectedException.NoExceptionExpected,
                    KeyInfo = new KeyInfo(KeyingMaterial.RsaSecurityKey1),
                    SecurityKey = new X509SecurityKey(x509CertificateWithSki),
                    MatchesKey = false,
                });

                theoryData.Add(new KeyInfoTheoryData("X509CertificateWithoutSKIExtension")
                {
                    ExpectedException = ExpectedException.NoExceptionExpected,
                    KeyInfo = new KeyInfo(KeyingMaterial.X509Certificate2),
                    SecurityKey = new X509SecurityKey(KeyingMaterial.X509Certificate2),
                    MatchesKey = true,
                });

                theoryData.Add(new KeyInfoTheoryData("RSASecurityKey")
                {
                    ExpectedException = ExpectedException.NoExceptionExpected,
                    KeyInfo = new KeyInfo(KeyingMaterial.RsaSecurityKey1),
                    SecurityKey = KeyingMaterial.RsaSecurityKey1,
                    MatchesKey = true,
                });

                theoryData.Add(new KeyInfoTheoryData("JsonWebKey_X509Cert")
                {
                    ExpectedException = ExpectedException.NoExceptionExpected,
                    KeyInfo = new KeyInfo(KeyingMaterial.DefaultCert_2048_Public),
                    SecurityKey = KeyingMaterial.JsonWebKeyX509_2048_Public,
                    MatchesKey = true,
                });

                theoryData.Add(new KeyInfoTheoryData("JsonWebKey_RsaSecurityKey")
                {
                    ExpectedException = ExpectedException.NoExceptionExpected,
                    KeyInfo = new KeyInfo(KeyingMaterial.RsaSecurityKey_2048_Public),
                    SecurityKey = KeyingMaterial.JsonWebKeyRsa_2048_Public,
                    MatchesKey = true,
                });

                theoryData.Add(new KeyInfoTheoryData("NullRSAKeyValue")
                {
                    ExpectedException = ExpectedException.NoExceptionExpected,
                    KeyInfo = new KeyInfo(), // This will have null RSAKeyValue
                    SecurityKey = KeyingMaterial.RsaSecurityKey_2048_Public,
                    MatchesKey = false,
                });

                return theoryData;
            }
        }
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
        public KeyInfoTheoryData(string testId) : base(testId)
        {
        }

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

        public SecurityKey SecurityKey
        {
            get;
            set;
        }

        public bool MatchesKey
        {
            get;
            set;
        }
    }

    public class CustomKeyInfo : KeyInfo
    {
        private string _securityKeyIdentifier;

        public CustomKeyInfo(string securityKeyIdentifier)
        {
            _securityKeyIdentifier = securityKeyIdentifier;
        }

        public CustomKeyInfo(X509Certificate2 certificate) : base(certificate)
        {
        }

        protected internal override bool MatchesKey(SecurityKey key)
        {
            X509SecurityKey x509SecurityKey = key as X509SecurityKey;
            if (key != null && _securityKeyIdentifier != null)
            {
                X509SubjectKeyIdentifierExtension skiExtension = x509SecurityKey.Certificate.Extensions["2.5.29.14"] as X509SubjectKeyIdentifierExtension;
                if (skiExtension == null)
                    return base.MatchesKey(key);

                string subjectKeyIdentifier = Convert.ToBase64String(skiExtension.RawData, 2, skiExtension.RawData.Length - 2);
                return _securityKeyIdentifier == subjectKeyIdentifier;
            }

            return base.MatchesKey(key);
        }
    }
}
