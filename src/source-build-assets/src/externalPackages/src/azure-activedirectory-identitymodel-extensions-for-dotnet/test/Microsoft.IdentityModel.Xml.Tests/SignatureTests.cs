// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Xml;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Xml.Tests
{
    public class SignatureTests
    {
        [Fact]
        public void GetSets()
        {
            var type = typeof(Signature);
            var properties = type.GetProperties();
            Assert.True(properties.Length == 5, $"Number of properties has changed from 5 to: {properties.Length}, adjust tests");
            var context = new GetSetContext
            {
                PropertyNamesAndSetGetValue = new List<KeyValuePair<string, List<object>>>
                {
                    new KeyValuePair<string, List<object>>("Id", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                    new KeyValuePair<string, List<object>>("KeyInfo", new List<object>{(KeyInfo)null, new KeyInfo(), new KeyInfo()}),
                    new KeyValuePair<string, List<object>>("Prefix", new List<object>{"", Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                    new KeyValuePair<string, List<object>>("SignatureValue", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                    new KeyValuePair<string, List<object>>("SignedInfo", new List<object>{(SignedInfo)null, new SignedInfo(), new SignedInfo()}),
                },
                Object = new Signature(),
            };

            TestUtilities.GetSet(context);
            TestUtilities.AssertFailIfErrors($"{this}.GetSets", context.Errors);
        }

        [Fact]
        public void Publics()
        {
            var signature = new Signature();
            var properties = new List<string>()
            {
                "SignatureValue",
                "SignedInfo",
            };

            var context = new GetSetContext();
            foreach (string property in properties)
            {
                TestUtilities.SetGet(signature, property, null, ExpectedException.ArgumentNullException(substringExpected: "value"), context);
            }

            TestUtilities.AssertFailIfErrors($"{this}.Publics", context.Errors);
        }

        [Theory, MemberData(nameof(ConstructorTheoryData), DisableDiscoveryEnumeration = true)]
        public void Constructor(SignatureTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.Constructor", theoryData);
            var context = new CompareContext($"{this}.Constructor, {theoryData.TestId}");
            try
            {
                var signature = new Signature(theoryData.SignedInfo);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SignatureTheoryData> ConstructorTheoryData
        {
            get
            {
                return new TheoryData<SignatureTheoryData>()
                {
                    new SignatureTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000: The parameter 'value' cannot be a 'null' or an empty object."),
                        First = true,
                        SignedInfo = null,
                        TestId = "SignedInfo NULL"
                    },
                    new SignatureTheoryData
                    {
                        SignedInfo = new SignedInfo(),
                        TestId = "SignedInfo"
                    }
                };
            }
        }

        [Theory, MemberData(nameof(VerifyTheoryData), DisableDiscoveryEnumeration = true)]
        public void Verify(SignatureTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.Verify", theoryData);
            try
            {
                theoryData.Signature.Verify(theoryData.SecurityKey, theoryData.CryptoProviderFactory);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SignatureTheoryData> VerifyTheoryData
        {
            get
            {
                var signatureUnknownReferenceDigestAlg = Default.Signature;
                signatureUnknownReferenceDigestAlg.SignedInfo.References[0].DigestMethod = $"_{SecurityAlgorithms.Sha256Digest}";

                return new TheoryData<SignatureTheoryData>
                {
                    new SignatureTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(XmlValidationException), "IDX30212:"),
                        First = true,
                        SecurityKey = Default.AsymmetricSigningKey,
                        Signature = new Signature(),
                        TestId = "SignedInfo:Null"
                    },
                    new SignatureTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("key"),
                        SecurityKey = null,
                        Signature = new Signature(),
                        TestId = "SecurityKey:Null"
                    },
                    new SignatureTheoryData
                    {
                        CryptoProviderFactory = null,
                        ExpectedException = ExpectedException.ArgumentNullException("cryptoProviderFactory"),
                        SecurityKey = Default.AsymmetricSigningKey,
                        Signature = new Signature(),
                        TestId = "CryptoProviderFactory:Null"
                    },
                    new SignatureTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(XmlValidationException), "IDX30207:"),
                        SecurityKey = Default.AsymmetricSigningKey,
                        Signature = new Signature(new SignedInfo{SignatureMethod = SecurityAlgorithms.Aes128CbcHmacSha256 }),
                        TestId = "Signature:MethodNotSupported"
                    },
                    new SignatureTheoryData
                    {
                        CryptoProviderFactory =  new CustomCryptoProviderFactory(new string[] { SecurityAlgorithms.RsaSha256Signature }),
                        ExpectedException = new ExpectedException(typeof(XmlValidationException), "IDX30203:"),
                        SecurityKey = Default.AsymmetricSigningKey,
                        Signature = new Signature(new SignedInfo{SignatureMethod = SecurityAlgorithms.RsaSha256Signature }),
                        TestId = "SignatureProvider.CreateForVerifying:ReturnsNull"
                    },
                    new SignatureTheoryData
                    {
                        CryptoProviderFactory = new CustomCryptoProviderFactory(new string[] { SecurityAlgorithms.RsaSha256Signature })
                        {
                            SigningSignatureProvider = new CustomSignatureProvider(Default.AsymmetricSigningKey, SecurityAlgorithms.RsaSha256Signature),
                            VerifyingSignatureProvider = new CustomSignatureProvider(Default.AsymmetricSigningKey, SecurityAlgorithms.RsaSha256Signature)
                        },
                        ExpectedException = new ExpectedException(typeof(XmlValidationException), "IDX30208:"),
                        SecurityKey = Default.AsymmetricSigningKey,
                        Signature = signatureUnknownReferenceDigestAlg,
                        TestId = "Reference:UnknownDigestAlg",
                    }
                };
            }
        }

        private static SignatureTheoryData SignatureTest(SignatureTestSet testSet, SecurityKey key, XmlTokenStream tokenStream, ExpectedException expectedException = null, bool first = false)
        {
            return new SignatureTheoryData
            {
                ExpectedException = expectedException ?? ExpectedException.NoExceptionExpected,
                SecurityKey = key,
                Signature = testSet.Signature,
                TestId = testSet.TestId ?? nameof(testSet),
                TokenStream = tokenStream,
                Xml = testSet.Xml
            };
        }
    }

    public class SignatureTheoryData : TheoryDataBase
    {
        public CryptoProviderFactory CryptoProviderFactory
        {
            get;
            set;
        } = CryptoProviderFactory.Default;

        public SecurityKey SecurityKey
        {
            get;
            set;
        } = Default.AsymmetricSigningKey;

        public DSigSerializer Serializer
        {
            get;
            set;
        } = new DSigSerializer();

        public Signature Signature
        {
            get;
            set;
        }

        public SignedInfo SignedInfo
        {
            get;
            set;
        }

        public XmlTokenStream TokenStream
        {
            get;
            set;
        } = Default.TokenStream;

        public string Xml
        {
            get;
            set;
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
