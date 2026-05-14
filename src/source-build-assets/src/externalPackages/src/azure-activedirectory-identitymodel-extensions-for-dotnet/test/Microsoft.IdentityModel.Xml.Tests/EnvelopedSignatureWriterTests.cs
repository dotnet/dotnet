// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.IdentityModel.Protocols.WsFederation;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Saml2;
using Microsoft.IdentityModel.Xml;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Xml.Tests
{
    public class EnvelopedSignatureWriterTests
    {
        [Theory, MemberData(nameof(ConstructorTestCases))]
        public void Constructor(EnvelopedSignatureTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.Constructor", theoryData);
            try
            {
                var envelopedWriter = new EnvelopedSignatureWriter(theoryData.XmlWriter, theoryData.SigningCredentials, theoryData.ReferenceId, theoryData.InclusiveNamespacesPrefixList);
                theoryData.ExpectedException.ProcessNoException();
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }
        }

        public static TheoryData<EnvelopedSignatureTheoryData> ConstructorTestCases
        {
            get
            {
                return new TheoryData<EnvelopedSignatureTheoryData>
                {
                    new EnvelopedSignatureTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        First = true,
                        ReferenceId = null,
                        SigningCredentials = null,
                        TestId = "Null XmlWriter",
                        XmlWriter = null
                    },
                    new EnvelopedSignatureTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        ReferenceId = Guid.NewGuid().ToString(),
                        SigningCredentials = null,
                        TestId = "Null SigningCredentials",
                        XmlWriter = null
                    }
                };
            }
        }

        [Theory, MemberData(nameof(CreateSignatureWithoutSpecifyingDigestTestCases))]
        public void CreateSignatureWithoutSpecifyingDigest(EnvelopedSignatureTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateSignatureWithoutSpecifyingDigest", theoryData);

            try
            {
                using (var buffer = new MemoryStream())
                {
                    var writer = new EnvelopedSignatureWriter(XmlWriter.Create(buffer), theoryData.SigningCredentials, theoryData.ReferenceId);
                    writer.WriteStartElement("EntityDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
                    writer.WriteAttributeString("entityID", "issuer");
                    writer.WriteEndElement();

                    // read and verify signatures
                    EnvelopedSignatureReader envelopedReader = new EnvelopedSignatureReader(XmlUtilities.CreateDictionaryReader(Encoding.UTF8.GetString(buffer.ToArray())));
                    while (envelopedReader.Read());

                    envelopedReader.Signature.Verify(theoryData.SigningCredentials.Key, theoryData.SigningCredentials.Key.CryptoProviderFactory);
                    theoryData.ExpectedException.ProcessNoException(context);
                }
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<EnvelopedSignatureTheoryData> CreateSignatureWithoutSpecifyingDigestTestCases
        {
            get
            {
                var theoryData = new TheoryData<EnvelopedSignatureTheoryData>();

                // ECD
                theoryData.Add(CreateSignatureTestCase(KeyingMaterial.Ecdsa256Key, SecurityAlgorithms.EcdsaSha256, null, SecurityAlgorithms.EcdsaSha256 + "_DigestNULL"));
                theoryData.Add(CreateSignatureTestCase(KeyingMaterial.Ecdsa256Key, SecurityAlgorithms.EcdsaSha256Signature, null, SecurityAlgorithms.EcdsaSha256Signature + "_DigestNULL"));
                theoryData.Add(CreateSignatureTestCase(KeyingMaterial.Ecdsa256Key, SecurityAlgorithms.EcdsaSha384, null, SecurityAlgorithms.EcdsaSha384 + "_DigestNULL"));
                theoryData.Add(CreateSignatureTestCase(KeyingMaterial.Ecdsa256Key, SecurityAlgorithms.EcdsaSha384Signature, null, SecurityAlgorithms.EcdsaSha384Signature + "_DigestNULL"));
                theoryData.Add(CreateSignatureTestCase(KeyingMaterial.Ecdsa256Key, SecurityAlgorithms.EcdsaSha512, null, SecurityAlgorithms.EcdsaSha512 + "_DigestNULL"));
                theoryData.Add(CreateSignatureTestCase(KeyingMaterial.Ecdsa256Key, SecurityAlgorithms.EcdsaSha512Signature, null, SecurityAlgorithms.EcdsaSha512Signature + "_DigestNULL"));

                // RSA
                theoryData.Add(CreateSignatureTestCase(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaSha256, null, SecurityAlgorithms.RsaSha256 + "_DigestNULL"));
                theoryData.Add(CreateSignatureTestCase(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaSha256Signature, null, SecurityAlgorithms.RsaSha256Signature + "_DigestNULL"));
                theoryData.Add(CreateSignatureTestCase(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaSha384, null, SecurityAlgorithms.RsaSha384 + "_DigestNULL"));
                theoryData.Add(CreateSignatureTestCase(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaSha384Signature, null, SecurityAlgorithms.RsaSha384Signature + "_DigestNULL"));
                theoryData.Add(CreateSignatureTestCase(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaSha512, null, SecurityAlgorithms.RsaSha512 + "_DigestNULL"));
                theoryData.Add(CreateSignatureTestCase(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaSha512Signature, null, SecurityAlgorithms.RsaSha512Signature + "_DigestNULL"));

#if NET_CORE
               theoryData.Add(CreateSignatureTestCase(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaSsaPssSha256, null, SecurityAlgorithms.RsaSsaPssSha256 + "_DigestNULL"));
               theoryData.Add(CreateSignatureTestCase(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaSsaPssSha256Signature, null, SecurityAlgorithms.RsaSsaPssSha256Signature + "_DigestNULL"));
               theoryData.Add(CreateSignatureTestCase(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaSsaPssSha384, null, SecurityAlgorithms.RsaSsaPssSha384 + "_DigestNULL"));
               theoryData.Add(CreateSignatureTestCase(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaSsaPssSha384Signature, null, SecurityAlgorithms.RsaSsaPssSha384Signature + "_DigestNULL"));
               theoryData.Add(CreateSignatureTestCase(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaSsaPssSha512, null, SecurityAlgorithms.RsaSsaPssSha512 + "_DigestNULL"));
               theoryData.Add(CreateSignatureTestCase(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaSsaPssSha512Signature, null, SecurityAlgorithms.RsaSsaPssSha512Signature + "_DigestNULL"));
#endif

                // Symmetric
                theoryData.Add(CreateSignatureTestCase(KeyingMaterial.SymmetricSecurityKey2_256, SecurityAlgorithms.HmacSha256, null, SecurityAlgorithms.HmacSha256 + "_DigestNULL"));
                theoryData.Add(CreateSignatureTestCase(KeyingMaterial.SymmetricSecurityKey2_256, SecurityAlgorithms.HmacSha256Signature, null, SecurityAlgorithms.HmacSha256Signature + "_DigestNULL"));
                theoryData.Add(CreateSignatureTestCase(KeyingMaterial.SymmetricSecurityKey2_384, SecurityAlgorithms.HmacSha384, null, SecurityAlgorithms.HmacSha384 + "_DigestNULL"));
                theoryData.Add(CreateSignatureTestCase(KeyingMaterial.SymmetricSecurityKey2_384, SecurityAlgorithms.HmacSha384Signature, null, SecurityAlgorithms.HmacSha384Signature + "_DigestNULL"));
                theoryData.Add(CreateSignatureTestCase(KeyingMaterial.SymmetricSecurityKey2_512, SecurityAlgorithms.HmacSha512, null, SecurityAlgorithms.HmacSha512 + "_DigestNULL"));
                theoryData.Add(CreateSignatureTestCase(KeyingMaterial.SymmetricSecurityKey2_512, SecurityAlgorithms.HmacSha512Signature, null, SecurityAlgorithms.HmacSha512Signature + "_DigestNULL"));

                // couple of failure cases
                theoryData.Add(CreateSignatureTestCase(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.EcdsaSha256, null, "RSASecurityKey_EcdsaSha256", ExpectedException.NotSupportedException()));
                theoryData.Add(CreateSignatureTestCase(KeyingMaterial.Ecdsa256Key, SecurityAlgorithms.RsaSha256, null, "Ecdsa256SecurityKey_RsaSha256", ExpectedException.NotSupportedException()));
                theoryData.Add(CreateSignatureTestCase(KeyingMaterial.SymmetricSecurityKey2_256, SecurityAlgorithms.RsaSha256, null, "SymmetricSecurityKey256_RsaSha256", ExpectedException.NotSupportedException()));

                return theoryData;
            }
        }

        private static EnvelopedSignatureTheoryData CreateSignatureTestCase(SecurityKey securityKey, string algorithm, string digest, string testId, ExpectedException expectedException = null)
        {
            return new EnvelopedSignatureTheoryData
            {
                ExpectedException = expectedException ?? ExpectedException.NoExceptionExpected,
                ReferenceId = Default.ReferenceUriWithOutPrefix,
                SigningCredentials = string.IsNullOrEmpty(digest) ? new SigningCredentials(securityKey, algorithm) : new SigningCredentials(securityKey, algorithm, digest),
                TestId = testId
            };
        }

        [Theory, MemberData(nameof(RoundTripSaml2TestCases))]
        public void RoundTripSaml2(EnvelopedSignatureTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.RoundTripSaml2", theoryData);
            context.PropertiesToIgnoreWhenComparing.Add(typeof(Saml2Assertion), new List<string> { "CanonicalString" });

            try
            {
                var serializer = new Saml2Serializer();
                var samlAssertion = serializer.ReadAssertion(XmlUtilities.CreateDictionaryReader(theoryData.Xml));
                var stream = new MemoryStream();
                var writer = XmlDictionaryWriter.CreateTextWriter(stream);
                samlAssertion.SigningCredentials = theoryData.SigningCredentials;
                serializer.WriteAssertion(writer, samlAssertion);
                writer.Flush();
                var xml = Encoding.UTF8.GetString(stream.ToArray());
                samlAssertion.SigningCredentials = null;
                var samlAssertion2 = serializer.ReadAssertion(XmlUtilities.CreateDictionaryReader(xml));
                samlAssertion2.Signature.Verify(theoryData.SigningCredentials.Key, theoryData.CryptoProviderFactory);
                IdentityComparer.AreEqual(samlAssertion, samlAssertion2, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<EnvelopedSignatureTheoryData> RoundTripSaml2TestCases
        {
            get
            {
                return new TheoryData<EnvelopedSignatureTheoryData>()
                {
                    new EnvelopedSignatureTheoryData
                    {
                        ReferenceId = Default.ReferenceUriWithPrefix,
                        SigningCredentials = Default.AsymmetricSigningCredentials,
                        TestId = nameof(ReferenceTokens.Saml2Token_Valid2) + "1",
                        Xml =  ReferenceTokens.Saml2Token_Valid2
                    },
                    new EnvelopedSignatureTheoryData
                    {
                        ReferenceId = Default.ReferenceUriWithOutPrefix,
                        SigningCredentials = Default.AsymmetricSigningCredentials,
                        TestId = nameof(ReferenceTokens.Saml2Token_Valid2) + "2",
                        Xml =  ReferenceTokens.Saml2Token_Valid2
                    }
                };
            }
        }

        [Theory, MemberData(nameof(RoundTripWsMetadataTestCases))]
        public void RoundTripWsMetadata(EnvelopedSignatureTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.RoundTripWsMetadata", theoryData);

            try
            {
                var settings = new XmlWriterSettings
                {
                    Encoding = new UTF8Encoding(false)
                };

                var buffer = new MemoryStream();
                var esw = new EnvelopedSignatureWriter(XmlWriter.Create(buffer, settings), theoryData.SigningCredentials, theoryData.ReferenceId);

                theoryData.Action.DynamicInvoke(esw);

                var metadata = Encoding.UTF8.GetString(buffer.ToArray());
                var configuration = new WsFederationConfiguration();
                var reader = XmlReader.Create(new StringReader(metadata));
                configuration = new WsFederationMetadataSerializer().ReadMetadata(reader);
                configuration.Signature.Verify(theoryData.SigningCredentials.Key, theoryData.SigningCredentials.Key.CryptoProviderFactory);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<EnvelopedSignatureTheoryData> RoundTripWsMetadataTestCases
        {
            get
            {
                return new TheoryData<EnvelopedSignatureTheoryData>()
                {
                    new EnvelopedSignatureTheoryData
                    {
                        First = true,
                        Action = (EnvelopedSignatureWriter esw) =>
                        {
                            esw.WriteStartElement("EntityDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
                            esw.WriteAttributeString("entityID", "issuer");
                            esw.WriteEndElement();
                        },
                        ReferenceId = Default.ReferenceUriWithOutPrefix,
                        SigningCredentials = Default.AsymmetricSigningCredentials,
                        TestId = "WriteSignatureAsLastElement",
                    },
                    new EnvelopedSignatureTheoryData
                    {
                        Action = (EnvelopedSignatureWriter esw) =>
                        {
                            esw.WriteStartElement("EntityDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
                            esw.WriteAttributeString("entityID", "issuer");
                            esw.WriteStartElement("RoleDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
                            esw.WriteEndElement();
                            esw.WriteEndElement();
                        },
                        ReferenceId = Default.ReferenceUriWithOutPrefix,
                        SigningCredentials = Default.AsymmetricSigningCredentials,
                        TestId = "WriteSignatureAsLastElement2",
                    },
                    new EnvelopedSignatureTheoryData
                    {
                        Action = (EnvelopedSignatureWriter esw) =>
                        {
                            esw.WriteStartElement("EntityDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
                            esw.WriteAttributeString("entityID", "issuer");
                            esw.WriteSignature();
                            esw.WriteStartElement("RoleDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
                            esw.WriteEndElement();
                            esw.WriteEndElement();
                        },
                        ReferenceId = Default.ReferenceUriWithOutPrefix,
                        SigningCredentials = Default.AsymmetricSigningCredentials,
                        TestId = "WriteSignatureInTheMiddle",
                    },
                    new EnvelopedSignatureTheoryData
                    {
                        Action = (EnvelopedSignatureWriter esw) =>
                        {
                            esw.WriteStartElement("EntityDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
                            esw.WriteAttributeString("entityID", "issuer");
                            esw.WriteStartElement("RoleDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
                            esw.WriteSignature();
                            esw.WriteEndElement();
                            esw.WriteEndElement();
                        },
                        ReferenceId = Default.ReferenceUriWithOutPrefix,
                        SigningCredentials = Default.AsymmetricSigningCredentials,
                        TestId = "WriteSignatureInTheMiddle2",
                    },
                    new EnvelopedSignatureTheoryData
                    {
                        Action = (EnvelopedSignatureWriter esw) =>
                        {
                            esw.WriteStartElement("EntityDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
                            esw.WriteAttributeString("entityID", "issuer");
                            esw.WriteStartElement("RoleDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
                            esw.WriteStartElement("KeyDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
                            esw.WriteSignature();
                            esw.WriteEndElement();
                            esw.WriteEndElement();
                            esw.WriteEndElement();
                        },
                        ReferenceId = Default.ReferenceUriWithOutPrefix,
                        SigningCredentials = Default.AsymmetricSigningCredentials,
                        TestId = "WriteSignatureInTheMiddle3",
                    },
                    new EnvelopedSignatureTheoryData
                    {
                        Action = (EnvelopedSignatureWriter esw) =>
                        {
                            esw.WriteStartElement("EntityDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
                            esw.WriteAttributeString("entityID", "issuer");
                            esw.WriteStartElement("RoleDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
                            esw.WriteEndElement();
                            esw.WriteSignature();
                            esw.WriteEndElement();
                        },
                        ReferenceId = Default.ReferenceUriWithOutPrefix,
                        SigningCredentials = Default.AsymmetricSigningCredentials,
                        TestId = "WriteSignatureAsLastElementExplicitly",
                    },
                    new EnvelopedSignatureTheoryData
                    {
                        Action = (EnvelopedSignatureWriter esw) =>
                        {
                            esw.WriteStartElement("EntityDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
                            esw.WriteAttributeString("entityID", "issuer");
                            esw.WriteStartElement("RoleDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
                            esw.WriteEndElement();
                            esw.WriteEndElement();
                            esw.WriteSignature();
                        },
                        ReferenceId = Default.ReferenceUriWithOutPrefix,
                        SigningCredentials = Default.AsymmetricSigningCredentials,
                        TestId = "WriteSignatureAfterLastElementIsIgnored",
                    },
                    new EnvelopedSignatureTheoryData
                    {
                        Action = (EnvelopedSignatureWriter esw) =>
                        {
                            esw.WriteStartElement("EntityDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
                            esw.WriteSignature();
                            esw.WriteAttributeString("entityID", "issuer");
                            esw.WriteStartElement("RoleDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
                            esw.WriteEndElement();
                            esw.WriteEndElement();
                        },
                        ReferenceId = Default.ReferenceUriWithOutPrefix,
                        SigningCredentials = Default.AsymmetricSigningCredentials,
                        ExpectedException = new ExpectedException(typeof(System.Reflection.TargetInvocationException), null, typeof(System.InvalidOperationException)),
                        TestId = "WriteSignatureInTheMiddleBeforeAttributeInvalid",
                    }
                };
            }
        }

        [Fact]
        public void RoundTripSamlP()
        {
            var context = new CompareContext($"{this}.RoundTripSamlP");
            ExpectedException expectedException = ExpectedException.NoExceptionExpected;
            var samlpTokenKey = KeyingMaterial.RsaSigningCreds_4096_Public.Key;
            var samlpTokenSigningCredentials = KeyingMaterial.RsaSigningCreds_4096;
            var samlpKey = KeyingMaterial.RsaSigningCreds_2048_Public.Key;
            var samlpSigningCredentials = KeyingMaterial.RsaSigningCreds_2048;

            try
            {
                // write samlp
                var settings = new XmlWriterSettings
                {
                    Encoding = new UTF8Encoding(false)
                };
                var buffer = new MemoryStream();
                var esw = new EnvelopedSignatureWriter(XmlWriter.Create(buffer, settings), samlpSigningCredentials, "id-uAOhNLe7abGB6WGPk");
                esw.WriteStartElement("ns0", "Response", "urn:oasis:names:tc:SAML:2.0:protocol");

                esw.WriteAttributeString("ns1", "urn:oasis:names:tc:SAML:2.0:assertion");
                esw.WriteAttributeString("ns2", "http://www.w3.org/2000/09/xmldsig#");
                esw.WriteAttributeString("Destination", "https://tnia.eidentita.cz/fpsts/processRequest.aspx");
                esw.WriteAttributeString("ID", "id-uAOhNLe7abGB6WGPk");
                esw.WriteAttributeString("InResponseTo", "ida5714d006fcc430c92aacf34ab30b166");
                esw.WriteAttributeString("IssueInstant", "2019-04-08T10:30:49Z");
                esw.WriteAttributeString("Version", "2.0");
                esw.WriteSignature();
                esw.WriteStartElement("ns1", "Issuer");
                esw.WriteAttributeString("Format", "urn:oasis:names:tc:SAML:2.0:nameid-format:entity");
                esw.WriteString("https://mojeid.regtest.nic.cz/saml/idp.xml");
                esw.WriteEndElement();
                esw.WriteStartElement("ns0", "Status", null);
                esw.WriteStartElement("ns0", "StatusCode", null);
                esw.WriteAttributeString("Value", "urn:oasis:names:tc:SAML:2.0:status:Success");
                esw.WriteEndElement();
                esw.WriteEndElement();
                Saml2Serializer samlSerializer = new Saml2Serializer();
                Saml2Assertion assertion = CreateAssertion(samlpTokenSigningCredentials);
                samlSerializer.WriteAssertion(esw, assertion);
                esw.WriteEndElement();
                var xml = Encoding.UTF8.GetString(buffer.ToArray());

                // read samlp and verify signatures
                XmlReader reader = XmlUtilities.CreateDictionaryReader(xml);
                IXmlElementReader tokenReaders = new TokenReaders(new List<SecurityTokenHandler> { new Saml2SecurityTokenHandler() });
                EnvelopedSignatureReader envelopedReader = new EnvelopedSignatureReader(reader, tokenReaders);

                while (envelopedReader.Read()) ;

                foreach (var item in tokenReaders.Items)
                {
                    if (item is Saml2SecurityToken samlToken)
                        samlToken.Assertion.Signature.Verify(samlpTokenKey);
                }

                envelopedReader.Signature.Verify(samlpKey, samlpKey.CryptoProviderFactory);
                expectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                expectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void RoundTripSamlPSignatureAfterAssertion()
        {
            var context = new CompareContext($"{this}.RoundTripSamlPSignatureAfterAssertion");
            ExpectedException expectedException = ExpectedException.NoExceptionExpected;
            var samlpTokenKey = KeyingMaterial.RsaSigningCreds_4096_Public.Key;
            var samlpTokenSigningCredentials = KeyingMaterial.RsaSigningCreds_4096;
            var samlpKey = KeyingMaterial.RsaSigningCreds_2048_Public.Key;
            var samlpSigningCredentials = KeyingMaterial.RsaSigningCreds_2048;

            try
            {
                // write samlp
                var settings = new XmlWriterSettings
                {
                    Encoding = new UTF8Encoding(false)
                };
                var buffer = new MemoryStream();
                var esw = new EnvelopedSignatureWriter(XmlWriter.Create(buffer, settings), samlpSigningCredentials, "id-uAOhNLe7abGB6WGPk");
                esw.WriteStartElement("ns0", "Response", "urn:oasis:names:tc:SAML:2.0:protocol");

                esw.WriteAttributeString("ns1", "urn:oasis:names:tc:SAML:2.0:assertion");
                esw.WriteAttributeString("ns2", "http://www.w3.org/2000/09/xmldsig#");
                esw.WriteAttributeString("Destination", "https://tnia.eidentita.cz/fpsts/processRequest.aspx");
                esw.WriteAttributeString("ID", "id-uAOhNLe7abGB6WGPk");
                esw.WriteAttributeString("InResponseTo", "ida5714d006fcc430c92aacf34ab30b166");
                esw.WriteAttributeString("IssueInstant", "2019-04-08T10:30:49Z");
                esw.WriteAttributeString("Version", "2.0");
                esw.WriteStartElement("ns1", "Issuer");
                esw.WriteAttributeString("Format", "urn:oasis:names:tc:SAML:2.0:nameid-format:entity");
                esw.WriteString("https://mojeid.regtest.nic.cz/saml/idp.xml");
                esw.WriteEndElement();
                esw.WriteStartElement("ns0", "Status", null);
                esw.WriteStartElement("ns0", "StatusCode", null);
                esw.WriteAttributeString("Value", "urn:oasis:names:tc:SAML:2.0:status:Success");
                esw.WriteEndElement();
                esw.WriteEndElement();
                Saml2Serializer samlSerializer = new Saml2Serializer();
                Saml2Assertion assertion = CreateAssertion(samlpTokenSigningCredentials);
                samlSerializer.WriteAssertion(esw, assertion);
                esw.WriteSignature();
                esw.WriteEndElement();
                var xml = Encoding.UTF8.GetString(buffer.ToArray());

                // read samlp and verify signatures
                XmlReader reader = XmlUtilities.CreateDictionaryReader(xml);
                IXmlElementReader tokenReaders = new TokenReaders(new List<SecurityTokenHandler> { new Saml2SecurityTokenHandler() });
                EnvelopedSignatureReader envelopedReader = new EnvelopedSignatureReader(reader, tokenReaders);

                while (envelopedReader.Read()) ;

                foreach (var item in tokenReaders.Items)
                {
                    if (item is Saml2SecurityToken samlToken)
                        samlToken.Assertion.Signature.Verify(samlpTokenKey);
                }

                envelopedReader.Signature.Verify(samlpKey, samlpKey.CryptoProviderFactory);
                expectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                expectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        private static Saml2Assertion CreateAssertion(SigningCredentials samlpTokenSigningCredentials)
        {
            var assertion = new Saml2Assertion(new Saml2NameIdentifier("https://mojeid.regtest.nic.cz/saml/idp.xml", new Uri("urn:oasis:names:tc:SAML:2.0:nameid-format:entity")))
            {
                InclusiveNamespacesPrefixList = "ns1 ns2",
                IssueInstant = DateTime.Parse("2019-04-08T10:30:49Z"),
                SigningCredentials = samlpTokenSigningCredentials,
                Id = new Saml2Id("id-2bMsOPOKIqeVIDLqJ")
            };

            var saml2SubjectConfirmationData = new Saml2SubjectConfirmationData
            {
                InResponseTo = new Saml2Id("ida5714d006fcc430c92aacf34ab30b166"),
                NotOnOrAfter = DateTime.Parse("2019-04-08T10:45:49Z"),
                Recipient = new Uri("https://tnia.eidentita.cz/fpsts/processRequest.aspx")
            };
            var saml2SubjectConfirmation = new Saml2SubjectConfirmation(new Uri("urn:oasis:names:tc:SAML:2.0:cm:bearer"), saml2SubjectConfirmationData);
            var saml2Subject = new Saml2Subject(new Saml2NameIdentifier("6dfe0399103d11411b1fa00772b6a13e0858605b80c20ea845769c57b41479ed", new Uri("urn:oasis:names:tc:SAML:2.0:nameid-format:persistent")));
            saml2Subject.SubjectConfirmations.Add(saml2SubjectConfirmation);
            assertion.Subject = saml2Subject;

            var saml2AudienceRestrictions = new Saml2AudienceRestriction("urn:microsoft: cgg2010: fpsts");
            var saml2Conditions = new Saml2Conditions();
            saml2Conditions.AudienceRestrictions.Add(saml2AudienceRestrictions);
            saml2Conditions.NotBefore = DateTime.Parse("2019-04-08T10:30:49Z");
            saml2Conditions.NotOnOrAfter = DateTime.Parse("2019-04-08T10:45:49Z");
            assertion.Conditions = saml2Conditions;

            var saml2AuthenticationContext = new Saml2AuthenticationContext(new Uri("urn:oasis:names:tc:SAML:2.0:ac:classes:PasswordProtectedTransport"));
            var saml2Statement = new Saml2AuthenticationStatement(saml2AuthenticationContext)
            {
                AuthenticationInstant = DateTime.Parse("2019-04-08T10:30:49Z"),
                SessionIndex = "id-oTnhrqWtcTTEntvMy"
            };
            assertion.Statements.Add(saml2Statement);

            return assertion;
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
