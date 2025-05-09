// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Xml;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Saml2.Tests
{
    public class Saml2SerializerTests
    {
        #region Saml2Action
        [Theory, MemberData(nameof(ReadActionTheoryData))]
        public void ReadAction(Saml2TheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ReadAction", theoryData);
            try
            {
                var reader = XmlUtilities.CreateDictionaryReader(theoryData.Xml);
                var action = (theoryData.Saml2Serializer as Saml2SerializerPublic).ReadActionPublic(reader);
                theoryData.ExpectedException.ProcessNoException(context);

                IdentityComparer.AreEqual(action, theoryData.Action, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<Saml2TheoryData> ReadActionTheoryData
        {
            get
            {
                return new TheoryData<Saml2TheoryData>
                {
                    new Saml2TheoryData
                    {
                        Action = new Saml2Action("Action", new Uri("urn:oasis:names:tc:SAML:2.0:assertion")),
                        Xml = "<Action Namespace=\"urn:oasis:names:tc:SAML:2.0:assertion\" xmlns=\"urn:oasis:names:tc:SAML:2.0:assertion\"/>",
                        ExpectedException = new ExpectedException(typeof(Saml2SecurityTokenReadException), "IDX13310:"),
                        First = true,
                        Saml2Serializer = new Saml2SerializerPublic(),
                        TestId = "Saml2ActionEmpty"
                    }
                };
            }
        }
        #endregion

        #region Saml2Advice
        [Theory, MemberData(nameof(ReadAdviceTheoryData))]
        public void ReadAdvice(Saml2TheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ReadAdvice", theoryData);
            var context = new CompareContext($"{this}.ReadAdvice, {theoryData.TestId}");
            try
            {
                var reader = XmlUtilities.CreateDictionaryReader(theoryData.Xml);
                var advice = (theoryData.Saml2Serializer as Saml2SerializerPublic).ReadAdvicePublic(reader);
                theoryData.ExpectedException.ProcessNoException();

                IdentityComparer.AreEqual(advice, theoryData.Advice, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<Saml2TheoryData> ReadAdviceTheoryData
        {
            get
            {
                return new TheoryData<Saml2TheoryData>
                {
                    new Saml2TheoryData
                    {
                        Advice = new Saml2Advice(),
                        Xml = "<Advice xmlns=\"urn:oasis:names:tc:SAML:2.0:assertion\"/>",
                        First = true,
                        Saml2Serializer = new Saml2SerializerPublic(),
                        TestId = "Saml2AdviceEmpty"
                    },
                };
            }
        }
        #endregion

        #region Saml2Assertion
        [Theory, MemberData(nameof(ReadAssertionTheoryData))]
        public void ReadAssertion(Saml2TheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ReadAssertion", theoryData);
            try
            {
                var reader = XmlUtilities.CreateXmlReader(theoryData.Xml);
                var assertion = (theoryData.Saml2Serializer as Saml2SerializerPublic).ReadAssertionPublic(reader);
                theoryData.ExpectedException.ProcessNoException(context);

                IdentityComparer.AreEqual(assertion, theoryData.Assertion, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(ReadAssertionTheoryData))]
        public void ReadAssertionUsingDictionaryReader(Saml2TheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ReadAssertionUsingDictionaryReader", theoryData);
            try
            {
                var reader = XmlUtilities.CreateDictionaryReader(theoryData.Xml);
                var assertion = (theoryData.Saml2Serializer as Saml2SerializerPublic).ReadAssertionPublic(reader);
                theoryData.ExpectedException.ProcessNoException(context);

                IdentityComparer.AreEqual(assertion, theoryData.Assertion, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(ReadAssertionTheoryData))]
        public void ReadAssertionUsingXDocumentReader(Saml2TheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ReadAssertionUsingXDocumentReader", theoryData);
            try
            {
                var reader = XmlUtilities.CreateXDocumentReader(theoryData.Xml);
                var assertion = (theoryData.Saml2Serializer as Saml2SerializerPublic).ReadAssertionPublic(reader);
                theoryData.ExpectedException.ProcessNoException(context);

                IdentityComparer.AreEqual(assertion, theoryData.Assertion, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<Saml2TheoryData> ReadAssertionTheoryData
        {
            get
            {
                return new TheoryData<Saml2TheoryData>
                {
                    new Saml2TheoryData
                    {
                        First = true,
                        Assertion = new Saml2Assertion(new Saml2NameIdentifier(Default.Issuer)),
                        Xml = "<Assertion Version=\"2.0\" ID=\"_b95759d0-73ae-4072-a140-567ade10a7ad\" Issuer=\"http://Default.Issuer.com\" IssueInstant=\"2017-03-17T18:33:37.095Z\" xmlns=\"urn:oasis:names:tc:SAML:2.0:assertion\"/>",
                        ExpectedException = new ExpectedException(typeof(Saml2SecurityTokenReadException), "IDX13102", typeof(XmlReadException)),
                        Saml2Serializer = new Saml2SerializerPublic(),
                        TestId = "Saml2AssertionEmpty"
                    },
                };
            }
        }
        #endregion

        #region Saml2Attribute
        [Theory, MemberData(nameof(ReadAttributeTheoryData))]
        public void ReadAttribute(Saml2TheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ReadAttribute", theoryData);
            try
            {
                var reader = XmlUtilities.CreateDictionaryReader(theoryData.Xml);
                var attribute = (theoryData.Saml2Serializer as Saml2SerializerPublic).ReadAttributePublic(reader);
                theoryData.ExpectedException.ProcessNoException();

                IdentityComparer.AreEqual(attribute, theoryData.Attribute, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<Saml2TheoryData> ReadAttributeTheoryData
        {
            get
            {
                return new TheoryData<Saml2TheoryData>
                {
                    new Saml2TheoryData()
                    {
                        Attribute = new Saml2Attribute("Country"),
                        Xml = "<Attribute Name =\"Country\" AttributeNamespace=\"http://schemas.xmlsoap.org/ws/2005/05/identity/claims\" xmlns=\"urn:oasis:names:tc:SAML:2.0:assertion\"/>",
                        First = true,
                        Saml2Serializer = new Saml2SerializerPublic(),
                        TestId = "Saml2AttributeEmpty"
                    },
                };
            }
        }
        #endregion

        #region Saml2AttributeStatement
        [Theory, MemberData(nameof(ReadAttributeStatementTheoryData))]
        public void ReadAttributeStatement(Saml2TheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ReadAttributeStatement", theoryData);
            var context = new CompareContext($"{this}.ReadAttributeStatement, {theoryData.TestId}");
            try
            {
                var reader = XmlUtilities.CreateDictionaryReader(theoryData.Xml);
                var attributeStatement = (theoryData.Saml2Serializer as Saml2SerializerPublic).ReadAttributeStatementPublic(reader);
                theoryData.ExpectedException.ProcessNoException();

                IdentityComparer.AreEqual(attributeStatement, theoryData.AttributeStatement, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<Saml2TheoryData> ReadAttributeStatementTheoryData
        {
            get
            {
                return new TheoryData<Saml2TheoryData>
                {
                    new Saml2TheoryData()
                    {
                        AttributeStatement = new Saml2AttributeStatement(),
                        Xml = "<AttributeStatement xmlns=\"urn:oasis:names:tc:SAML:2.0:assertion\"/>",
                        ExpectedException = new ExpectedException(typeof(Saml2SecurityTokenReadException), "IDX13138"),
                        First = true,
                        Saml2Serializer = new Saml2SerializerPublic(),
                        TestId = "Saml2AttributeStatementEmpty"
                    }
                };
            }
        }
        #endregion

        #region Saml2AudienceRestrictionCondition
        [Theory, MemberData(nameof(ReadAudienceRestrictionConditionTheoryData))]
        public void ReadAudienceRestriction(Saml2TheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ReadAudienceRestriction", theoryData);
            var context = new CompareContext($"{this}.ReadAudienceRestriction, {theoryData.TestId}");
            try
            {
                var reader = XmlUtilities.CreateDictionaryReader(theoryData.Xml);
                var audienceRestrictionCondition = (theoryData.Saml2Serializer as Saml2SerializerPublic).ReadAudienceRestrictionConditionPublic(reader);
                theoryData.ExpectedException.ProcessNoException();

                IdentityComparer.AreEqual(audienceRestrictionCondition, theoryData.AudienceRestriction, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<Saml2TheoryData> ReadAudienceRestrictionConditionTheoryData
        {
            get
            {
                return new TheoryData<Saml2TheoryData>
                {
                    new Saml2TheoryData
                    {
                        AudienceRestriction = new Saml2AudienceRestriction("Audience"),
                        Xml = "<AudienceRestriction xmlns=\"urn:oasis:names:tc:SAML:2.0:assertion\"/>",
                        ExpectedException = new ExpectedException(typeof(Saml2SecurityTokenReadException), "IDX13104:"),
                        First = true,
                        Saml2Serializer = new Saml2SerializerPublic(),
                        TestId = "Saml2AudienceRestrictionEmpty"
                    }
                };
            }
        }
        #endregion

        #region Saml2AuthenticationStatement
        [Theory, MemberData(nameof(ReadAuthenticationStatementTheoryData))]
        public void ReadAuthenticationStatement(Saml2TheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ReadAuthenticationStatement", theoryData);
            var context = new CompareContext($"{this}.ReadAuthenticationStatement, {theoryData.TestId}");
            try
            {
                var reader = XmlUtilities.CreateDictionaryReader(theoryData.Xml);
                var authenticationStatement = (theoryData.Saml2Serializer as Saml2SerializerPublic).ReadAuthenticationStatementPublic(reader);
                theoryData.ExpectedException.ProcessNoException();

                IdentityComparer.AreEqual(authenticationStatement, theoryData.AuthenticationStatement, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<Saml2TheoryData> ReadAuthenticationStatementTheoryData
        {
            get
            {
                return new TheoryData<Saml2TheoryData>
                {
                    new Saml2TheoryData
                    {
                        Xml = @"<AuthnStatement AuthenticationMethod=""urn:oasis:names:tc:SAML:2.0:am:password"" AuthnInstant =""2017-03-18T18:33:37.080Z"" xmlns=""urn:oasis:names:tc:SAML:2.0:assertion""/>",
                        First = true,
                        ExpectedException = new ExpectedException(typeof(Saml2SecurityTokenReadException), "IDX13313:"),
                        Saml2Serializer = new Saml2SerializerPublic(),
                        TestId = "Saml2AuthenticationStatementEmpty"
                    }
                };
            }
        }
        #endregion

        #region Saml2AuthorizationDecisionStatement
        [Theory, MemberData(nameof(ReadAuthorizationDecisionStatementTheoryData))]
        public void ReadAuthorizationDecisionStatement(Saml2TheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ReadAuthorizationDecisionStatement", theoryData);
            var context = new CompareContext($"{this}.ReadAuthorizationDecisionStatement, {theoryData.TestId}");
            try
            {
                var reader = XmlUtilities.CreateDictionaryReader(theoryData.Xml);
                var statement = (theoryData.Saml2Serializer as Saml2SerializerPublic).ReadAuthorizationDecisionStatementPublic(reader);
                theoryData.ExpectedException.ProcessNoException();

                IdentityComparer.AreEqual(statement, theoryData.AuthorizationDecision, context);
                
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<Saml2TheoryData> ReadAuthorizationDecisionStatementTheoryData
        {
            get
            {
                return new TheoryData<Saml2TheoryData>
                {
                    new Saml2TheoryData
                    {
                        Xml = @"<AuthzDecisionStatement Resource=""http://www.w3.org/"" Decision=""Permit"" xmlns=""urn:oasis:names:tc:SAML:2.0:assertion""/>",
                        First = true,
                        ExpectedException = new ExpectedException(typeof(Saml2SecurityTokenReadException), "IDX13314:"),
                        Saml2Serializer = new Saml2SerializerPublic(),
                        TestId = "Saml2AuthorizationDecisionEmpty"
                    }
                };
            }
        }
        #endregion

        #region Saml2Conditions
        [Theory, MemberData(nameof(ReadConditionsTheoryData))]
        public void ReadConditions(Saml2TheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ReadConditions", theoryData);
            var context = new CompareContext($"{this}.ReadConditions, {theoryData.TestId}");
            try
            {
                var reader = XmlUtilities.CreateDictionaryReader(theoryData.Xml);
                var conditions = (theoryData.Saml2Serializer as Saml2SerializerPublic).ReadConditionsPublic(reader);
                theoryData.ExpectedException.ProcessNoException();

                IdentityComparer.AreEqual(conditions, theoryData.Conditions, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<Saml2TheoryData> ReadConditionsTheoryData
        {
            get
            {
                return new TheoryData<Saml2TheoryData>
                {
                    new Saml2TheoryData
                    {
                        Conditions = new Saml2Conditions
                        {
                            NotBefore = DateTime.ParseExact("2017-03-17T18:33:37.080Z", Saml2Constants.AcceptedDateTimeFormats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None),
                            NotOnOrAfter = DateTime.ParseExact("2017-03-18T18:33:37.080Z", Saml2Constants.AcceptedDateTimeFormats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None)
                        },
                        Xml = @"<Conditions NotBefore=""2017-03-17T18:33:37.080Z"" NotOnOrAfter=""2017-03-18T18:33:37.080Z"" xmlns=""urn:oasis:names:tc:SAML:2.0:assertion""/>",
                        First = true, 
                        Saml2Serializer = new Saml2SerializerPublic(),
                        TestId = "Saml2ConditionsEmpty"
                    }
                };
            }
        }
        #endregion

        #region SamlEvidence
        [Theory, MemberData(nameof(ReadEvidenceTheoryData))]
        public void ReadEvidence(Saml2TheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ReadEvidence", theoryData);
            var context = new CompareContext($"{this}.ReadEvidence, {theoryData.TestId}");
            try
            {
                var reader = XmlUtilities.CreateDictionaryReader(theoryData.Xml);
                var evidence = (theoryData.Saml2Serializer as Saml2SerializerPublic).ReadEvidencePublic(reader);
                theoryData.ExpectedException.ProcessNoException();

                IdentityComparer.AreEqual(evidence, theoryData.Evidence, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<Saml2TheoryData> ReadEvidenceTheoryData
        {
            get
            {
                return new TheoryData<Saml2TheoryData>
                {
                    new Saml2TheoryData
                    {
                        Xml = "<Evidence xmlns=\"urn:oasis:names:tc:SAML:2.0:assertion\"/>",
                        ExpectedException = new ExpectedException(typeof(Saml2SecurityTokenReadException), "IDX13122"),
                        Saml2Serializer = new Saml2SerializerPublic(),
                        TestId = "Saml2EvidenceEmpty"
                    }
                };
            }
        }
        #endregion

        #region Saml2ProxyRestriction

        [Theory, MemberData(nameof(WriteSaml2ProxyRestrictionTheoryData))]
        public void WriteSaml2ProxyRestriction(Saml2TheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.WriteSaml2ProxyRestriction", theoryData);
            var context = new CompareContext($"{this}.WriteSaml2ProxyRestriction, {theoryData.TestId}");
            try
            {
                var ms = new MemoryStream();
                var writer = XmlDictionaryWriter.CreateTextWriter(ms, Encoding.UTF8, false);
                var envelopedWriter = new EnvelopedSignatureWriter(writer, Default.AsymmetricSigningCredentials, "ref#1");
                (theoryData.Saml2Serializer as Saml2SerializerPublic).WriteProxyRestrictionPublic(writer, theoryData.ProxyRestriction);

                writer.Flush();
                var xml = Encoding.UTF8.GetString(ms.ToArray());
                IdentityComparer.AreEqual(xml, theoryData.Xml, context);

                theoryData.ExpectedException.ProcessNoException();
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<Saml2TheoryData> WriteSaml2ProxyRestrictionTheoryData
        {
            get
            {
                var proxyRestriction = new Saml2ProxyRestriction();
                proxyRestriction.Audiences.Add(new Uri(Default.Uri));

                return new TheoryData<Saml2TheoryData>
                {
                    new Saml2TheoryData
                    {
                        Xml = "<saml:ProxyRestriction xmlns:saml=\"urn:oasis:names:tc:SAML:2.0:assertion\"><saml:Audience>http://referenceUri</saml:Audience></saml:ProxyRestriction>",
                        Saml2Serializer = new Saml2SerializerPublic(),
                        ProxyRestriction = proxyRestriction,
                        TestId = "WriteSaml2ProxyRestriction",
                    }
                };
            }
        }
        #endregion

        #region Saml2Subject
        [Theory, MemberData(nameof(ReadSubjectTheoryData))]
        public void ReadSubject(Saml2TheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ReadSubject", theoryData);
            var context = new CompareContext($"{this}.ReadSubject, {theoryData.TestId}");
            try
            {
                var reader = XmlUtilities.CreateDictionaryReader(theoryData.Xml);
                var subject = (theoryData.Saml2Serializer as Saml2SerializerPublic).ReadSubjectPublic(reader);
                theoryData.ExpectedException.ProcessNoException();

                IdentityComparer.AreEqual(subject, theoryData.Subject, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<Saml2TheoryData> ReadSubjectTheoryData
        {
            get
            {
                return new TheoryData<Saml2TheoryData>
                {
                    new Saml2TheoryData
                    {
                        First = true,
                        Subject = new Saml2Subject(new Saml2NameIdentifier("samlSubjectId")),
                        Xml = "<Subject NameId=\"samlSubjectId\" xmlns =\"urn:oasis:names:tc:SAML:2.0:assertion\"/>",
                        ExpectedException = new ExpectedException(typeof(Saml2SecurityTokenReadException), "IDX13125:"),
                        Saml2Serializer = new Saml2SerializerPublic(),
                        TestId = "Saml2SubjectEmpty"
                    },
                    new Saml2TheoryData
                    {
                        Subject = new Saml2Subject(new Saml2NameIdentifier("samlSubjectId", new Uri("urn:oasis:names:tc:SAML:2.0:nameid-format:entity"))),
                        Xml = "<Subject NameId=\"test\" xmlns =\"urn:oasis:names:tc:SAML:2.0:assertion\"><NameID Format=\"urn:oasis:names:tc:SAML:2.0:nameid-format:entity\">samlSubjectId</NameID></Subject>",
                        Saml2Serializer = new Saml2SerializerPublic(),
                        TestId = "Saml2SubjectNameIDIsNotAbsoluteURI"
                    }
                };
            }
        }
        #endregion

        private class Saml2SerializerPublic : Saml2Serializer
        {
            public Saml2Action ReadActionPublic(XmlDictionaryReader reader)
            {
                return base.ReadAction(reader);
            }

            public Saml2Advice ReadAdvicePublic(XmlDictionaryReader reader)
            {
                return base.ReadAdvice(reader);
            }

            public Saml2Assertion ReadAssertionPublic(XmlReader reader)
            {
                return base.ReadAssertion(reader);
            }

            public Saml2Attribute ReadAttributePublic(XmlDictionaryReader reader)
            {
                return base.ReadAttribute(reader);
            }

            public Saml2AttributeStatement ReadAttributeStatementPublic(XmlDictionaryReader reader)
            {
                return base.ReadAttributeStatement(reader);
            }

            public Saml2AudienceRestriction ReadAudienceRestrictionConditionPublic(XmlDictionaryReader reader)
            {
                return base.ReadAudienceRestriction(reader);
            }

            public Saml2AuthenticationStatement ReadAuthenticationStatementPublic(XmlDictionaryReader reader)
            {
                return base.ReadAuthenticationStatement(reader);
            }

            public Saml2AuthorizationDecisionStatement ReadAuthorizationDecisionStatementPublic(XmlDictionaryReader reader)
            {
                return base.ReadAuthorizationDecisionStatement(reader);
            }

            public Saml2Conditions ReadConditionsPublic(XmlDictionaryReader reader)
            {
                return base.ReadConditions(reader);
            }

            public Saml2Evidence ReadEvidencePublic(XmlDictionaryReader reader)
            {
                return base.ReadEvidence(reader);
            }

            public Saml2Subject ReadSubjectPublic(XmlDictionaryReader reader)
            {
                return base.ReadSubject(reader);
            }

            public void WriteProxyRestrictionPublic(XmlWriter writer, Saml2ProxyRestriction proxyRestriction)
            {
                base.WriteProxyRestriction(writer, proxyRestriction);
            }
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
