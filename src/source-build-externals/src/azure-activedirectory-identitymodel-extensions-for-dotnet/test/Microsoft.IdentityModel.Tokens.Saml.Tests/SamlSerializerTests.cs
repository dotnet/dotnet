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
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Xml;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Saml.Tests
{
    public class SamlSerializerTests
    {
        #region SamlAction
        [Theory, MemberData(nameof(ReadActionTheoryData))]
        public void ReadAction(SamlTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ReadAction", theoryData);
            try
            {
                var reader = XmlUtilities.CreateDictionaryReader(theoryData.ActionTestSet.Xml);
                var action = (theoryData.SamlSerializer as SamlSerializerPublic).ReadActionPublic(reader);
                theoryData.ExpectedException.ProcessNoException(context);

                IdentityComparer.AreEqual(action, theoryData.ActionTestSet.Action, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SamlTheoryData> ReadActionTheoryData
        {
            get
            {
                return new TheoryData<SamlTheoryData>
                {
                    new SamlTheoryData
                    {
                        ActionTestSet = ReferenceSaml.SamlActionValueNull,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11112:", typeof(ArgumentNullException)),
                        First = true,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlActionValueNull)
                    },
                    new SamlTheoryData
                    {
                        ActionTestSet = ReferenceSaml.SamlActionValueEmptyString,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11112:", typeof(ArgumentNullException)),
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlActionValueEmptyString)
                    },
                    new SamlTheoryData
                    {
                        ActionTestSet = ReferenceSaml.SamlActionNamespaceNull,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlActionNamespaceNull)
                    },
                    new SamlTheoryData
                    {
                        ActionTestSet = ReferenceSaml.SamlActionNamespaceEmptyString,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlActionNamespaceEmptyString)
                    },
                    new SamlTheoryData
                    {
                        ActionTestSet = ReferenceSaml.SamlActionNamespaceNotAbsoluteUri,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11111:"),
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlActionNamespaceNotAbsoluteUri)
                    },
                    new SamlTheoryData
                    {
                        ActionTestSet = ReferenceSaml.SamlActionValid,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlActionValid)
                    },
                    new SamlTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11137:"),
                        ActionTestSet = ReferenceSaml.SamlActionEmpty,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlActionEmpty)
                    }
                };
            }
        }
        #endregion

        #region SamlAdvice
        [Theory, MemberData(nameof(ReadAdviceTheoryData))]
        public void ReadAdvice(SamlTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ReadAdvice", theoryData);
            var context = new CompareContext($"{this}.ReadAdvice, {theoryData.TestId}");
            try
            {
                var reader = XmlUtilities.CreateDictionaryReader(theoryData.AdviceTestSet.Xml);
                var advice = (theoryData.SamlSerializer as SamlSerializerPublic).ReadAdvicePublic(reader);
                theoryData.ExpectedException.ProcessNoException();

                IdentityComparer.AreEqual(advice, theoryData.AdviceTestSet.Advice, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SamlTheoryData> ReadAdviceTheoryData
        {
            get
            {
                return new TheoryData<SamlTheoryData>
                {
                    new SamlTheoryData
                    {
                        AdviceTestSet = ReferenceSaml.AdviceNoAssertionIDRefAndAssertion,
                        First = true,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.AdviceNoAssertionIDRefAndAssertion)
                    },
                    new SamlTheoryData
                    {
                        AdviceTestSet = ReferenceSaml.AdviceWithAssertionIDRef,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.AdviceWithAssertionIDRef)
                    },
                    new SamlTheoryData
                    {
                        AdviceTestSet = ReferenceSaml.SamlAdviceWithAssertions,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAdviceWithAssertions)
                    },
                    new SamlTheoryData
                    {
                        AdviceTestSet = ReferenceSaml.SamlAdviceWithWrongElement,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11126"),
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAdviceWithWrongElement)
                    },
                    new SamlTheoryData
                    {
                        AdviceTestSet = ReferenceSaml.SamlAdviceWithAssertionIDRefAndAssertions,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAdviceWithAssertionIDRefAndAssertions)
                    }
                };
            }
        }
        #endregion

        #region SamlAssertion
        [Theory, MemberData(nameof(ReadAssertionTheoryData))]
        public void ReadAssertion(SamlTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ReadAssertion", theoryData);
            try
            {
                var reader = XmlUtilities.CreateXmlReader(theoryData.AssertionTestSet.Xml);
                var assertion = (theoryData.SamlSerializer as SamlSerializerPublic).ReadAssertionPublic(reader);
                theoryData.ExpectedException.ProcessNoException(context);

                context.PropertiesToIgnoreWhenComparing.Add(typeof(SamlAssertion), new List<string> { "CanonicalString" });
                IdentityComparer.AreEqual(assertion, theoryData.AssertionTestSet.Assertion, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(ReadAssertionTheoryData))]
        public void ReadAssertionUsingDictionaryReader(SamlTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ReadAssertionUsingDictionaryReader", theoryData);
            try
            {
                var reader = XmlUtilities.CreateDictionaryReader(theoryData.AssertionTestSet.Xml);
                var assertion = (theoryData.SamlSerializer as SamlSerializerPublic).ReadAssertionPublic(reader);
                theoryData.ExpectedException.ProcessNoException(context);

                context.PropertiesToIgnoreWhenComparing.Add(typeof(SamlAssertion), new List<string> { "CanonicalString" });
                IdentityComparer.AreEqual(assertion, theoryData.AssertionTestSet.Assertion, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(ReadAssertionTheoryData))]
        public void ReadAssertionUsingXDocumentReader(SamlTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ReadAssertionUsingXDocumentReader", theoryData);
            try
            {
                var reader = XmlUtilities.CreateXDocumentReader(theoryData.AssertionTestSet.Xml);
                var assertion = (theoryData.SamlSerializer as SamlSerializerPublic).ReadAssertionPublic(reader);
                theoryData.ExpectedException.ProcessNoException(context);

                context.PropertiesToIgnoreWhenComparing.Add(typeof(SamlAssertion), new List<string> { "CanonicalString" });
                IdentityComparer.AreEqual(assertion, theoryData.AssertionTestSet.Assertion, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SamlTheoryData> ReadAssertionTheoryData
        {
            get
            {
                return new TheoryData<SamlTheoryData>
                {
                    new SamlTheoryData
                    {
                        AssertionTestSet = ReferenceSaml.SamlAssertionMissMajorVersion,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11115"),
                        First = true,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAssertionMissMajorVersion)
                    },
                    new SamlTheoryData
                    {
                        AssertionTestSet = ReferenceSaml.SamlAssertionWrongMajorVersion,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11116"),
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAssertionWrongMajorVersion)
                    },
                    new SamlTheoryData
                    {
                        AssertionTestSet = ReferenceSaml.SamlAssertionMissMinorVersion,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11115"),
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAssertionMissMinorVersion)
                    },
                    new SamlTheoryData
                    {
                        AssertionTestSet = ReferenceSaml.SamlAssertionWrongMinorVersion,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11117"),
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAssertionWrongMinorVersion)
                    },
                    new SamlTheoryData
                    {
                        AssertionTestSet = ReferenceSaml.SamlAssertionMissAssertionID,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11115"),
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAssertionMissAssertionID)
                    },
                    new SamlTheoryData
                    {
                        AssertionTestSet = ReferenceSaml.SamlAssertionWrongAssertionID,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11121"),
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAssertionWrongAssertionID)
                    },
                    new SamlTheoryData
                    {
                        AssertionTestSet = ReferenceSaml.SamlAssertionMissIssuer,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11115"),
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAssertionMissIssuer)
                    },
                    new SamlTheoryData
                    {
                        AssertionTestSet = ReferenceSaml.SamlAssertionMissIssuerInstant,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11115"),
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAssertionMissIssuerInstant)
                    },
                    new SamlTheoryData
                    {
                        AssertionTestSet = ReferenceSaml.SamlAssertionNoCondition,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAssertionNoCondition)
                    },
                    new SamlTheoryData
                    {
                        AssertionTestSet = ReferenceSaml.SamlAssertionNoAdvice,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAssertionNoAdvice)
                    },
                    new SamlTheoryData
                    {
                        AssertionTestSet = ReferenceSaml.SamlAssertionMissStatement,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11130"),
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAssertionMissStatement)
                    },
                    new SamlTheoryData
                    {
                        AssertionTestSet = ReferenceSaml.SamlAssertionWrongElementInStatementPlace,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11126"),
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAssertionWrongElementInStatementPlace)
                    },
                    new SamlTheoryData
                    {
                        AssertionTestSet = ReferenceSaml.SamlAssertionNoSignature,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAssertionNoSignature)
                    },
                    new SamlTheoryData
                    {
                        AssertionTestSet = ReferenceSaml.SamlAssertionMultiStatements_SameSubject,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAssertionMultiStatements_SameSubject)
                    },
                    new SamlTheoryData
                    {
                        AssertionTestSet = ReferenceSaml.SamlAssertionMultiStatements_DifferentSubject,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAssertionMultiStatements_DifferentSubject)
                    },
                    new SamlTheoryData
                    {
                        AssertionTestSet = ReferenceSaml.SamlAssertionMultiStatements_DifferentStatementType,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAssertionMultiStatements_DifferentStatementType)
                    },
                     new SamlTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11130"),
                        AssertionTestSet = ReferenceSaml.SamlAssertionEmpty,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAssertionEmpty)
                    }
                };
            }
        }

        [Theory, MemberData(nameof(WriteAssertionTheoryData))]
        public void WriteAssertion(SamlTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.WriteToken", theoryData);
            try
            {
                var memoryStream = new MemoryStream();
                var writer = XmlDictionaryWriter.CreateTextWriter(memoryStream, Encoding.UTF8, false);
                theoryData.SamlSerializer.DSigSerializer = theoryData.DSigSerializer;
                theoryData.SamlSerializer.WriteAssertion(writer, theoryData.AssertionTestSet.Assertion);
                theoryData.ExpectedException.ProcessNoException(context);
                writer.Flush();
                var xml = Encoding.UTF8.GetString(memoryStream.ToArray());
                var assertion = theoryData.SamlSerializer.ReadAssertion(XmlUtilities.CreateDictionaryReader(xml));
                if (theoryData.SigningCredentials != null)
                {
                    assertion.SigningCredentials = theoryData.SigningCredentials;
                    assertion.Signature.Verify(theoryData.SigningCredentials.Key, theoryData.SigningCredentials.Key.CryptoProviderFactory);
                }

                theoryData.ExpectedException.ProcessNoException(context);
                context.PropertiesToIgnoreWhenComparing.Add(typeof(SamlAssertion), new List<string> { "CanonicalString" });
                IdentityComparer.AreEqual(assertion, theoryData.AssertionTestSet.Assertion, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SamlTheoryData> WriteAssertionTheoryData
        {
            get
            {
                // uncomment to view exception displayed to user
                // ExpectedException.DefaultVerbose = true;

                return new TheoryData<SamlTheoryData>
                {
                    new SamlTheoryData
                    {
                        AssertionTestSet = ReferenceSaml.SamlAssertionWithSignature,
                        SigningCredentials = Default.AsymmetricSigningCredentials,
                        TestId = nameof(ReferenceSaml.SamlAssertionWithSignature)
                    },
                    new SamlTheoryData
                    {
                        AssertionTestSet = ReferenceSaml.SamlAssertionWithSignatureNS,
                        DSigSerializer = new DSigSerializer{Prefix = "ds"},
                        SigningCredentials = Default.AsymmetricSigningCredentials,
                        TestId = nameof(ReferenceSaml.SamlAssertionWithSignatureNS)
                    },
                    new SamlTheoryData
                    {
                        AssertionTestSet = ReferenceSaml.SamlAssertionNoSignature,
                        TestId = nameof(ReferenceSaml.SamlAssertionNoSignature)
                    }
                };
            }
        }
        #endregion

        #region SamlAttribute
        [Theory, MemberData(nameof(ReadAttributeTheoryData))]
        public void ReadAttribute(SamlTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ReadAttribute", theoryData);
            try
            {
                var reader = XmlUtilities.CreateDictionaryReader(theoryData.AttributeTestSet.Xml);
                var attribute = (theoryData.SamlSerializer as SamlSerializerPublic).ReadAttributePublic(reader);
                theoryData.ExpectedException.ProcessNoException();

                IdentityComparer.AreEqual(attribute, theoryData.AttributeTestSet.Attribute, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SamlTheoryData> ReadAttributeTheoryData
        {
            get
            {
                return new TheoryData<SamlTheoryData>
                {
                    new SamlTheoryData()
                    {
                        AttributeTestSet = ReferenceSaml.SamlAttributeNameNull,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11112:", typeof(ArgumentNullException)),
                        First = true,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAttributeNameNull)
                    },
                    new SamlTheoryData()
                    {
                        AttributeTestSet = ReferenceSaml.SamlAttributeNameEmptyString,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11112:", typeof(ArgumentNullException)),
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAttributeNameEmptyString)
                    },
                    new SamlTheoryData()
                    {
                        AttributeTestSet = ReferenceSaml.SamlAttributeNamespaceNull,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11112:", typeof(ArgumentNullException)),
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAttributeNamespaceNull)
                    },
                    new SamlTheoryData()
                    {
                        AttributeTestSet = ReferenceSaml.SamlAttributeNamespaceEmptyString,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11112:", typeof(ArgumentNullException)),
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAttributeNamespaceEmptyString)
                    },
                    new SamlTheoryData()
                    {
                        AttributeTestSet = ReferenceSaml.SamlAttributeValueNull,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAttributeValueNull)
                    },
                    new SamlTheoryData()
                    {
                        AttributeTestSet = ReferenceSaml.SamlAttributeValueEmptyString,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAttributeValueEmptyString)
                    },
                    new SamlTheoryData()
                    {
                        AttributeTestSet = ReferenceSaml.SamlAttributeSingleValue,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAttributeSingleValue)
                    },
                    new SamlTheoryData()
                    {
                        AttributeTestSet = ReferenceSaml.SamlAttributeMultiValue,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAttributeMultiValue)
                    }
                };
            }
        }
        #endregion

        #region SamlAttributeStatement
        [Theory, MemberData(nameof(ReadAttributeStatementTheoryData))]
        public void ReadAttributeStatement(SamlTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ReadAttributeStatement", theoryData);
            var context = new CompareContext($"{this}.ReadAttributeStatement, {theoryData.TestId}");
            try
            {
                var reader = XmlUtilities.CreateDictionaryReader(theoryData.AttributeStatementTestSet.Xml);
                var attributeStatement = (theoryData.SamlSerializer as SamlSerializerPublic).ReadAttributeStatementPublic(reader);
                theoryData.ExpectedException.ProcessNoException();

                IdentityComparer.AreEqual(attributeStatement, theoryData.AttributeStatementTestSet.AttributeStatement, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SamlTheoryData> ReadAttributeStatementTheoryData
        {
            get
            {
                return new TheoryData<SamlTheoryData>
                {
                    new SamlTheoryData()
                    {
                        AttributeStatementTestSet = ReferenceSaml.SamlAttributeStatementMissSubject,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11112:", typeof(XmlReadException)),
                        First = true,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAttributeStatementMissSubject)
                    },
                    new SamlTheoryData()
                    {
                        AttributeStatementTestSet = ReferenceSaml.SamlAttributeStatementMissAttribute,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11131:"),
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAttributeStatementMissAttribute)
                    },
                    new SamlTheoryData()
                    {
                        AttributeStatementTestSet = ReferenceSaml.SamlAttributeStatementSingleAttribute,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAttributeStatementSingleAttribute)
                    },
                    new SamlTheoryData()
                    {
                        AttributeStatementTestSet = ReferenceSaml.SamlAttributeStatementMultiAttributes,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAttributeStatementMultiAttributes)
                    }
                };
            }
        }
        #endregion

        #region SamlAudienceRestrictionCondition
        [Theory, MemberData(nameof(ReadAudienceRestrictionConditionTheoryData))]
        public void ReadAudienceRestrictionCondition(SamlTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ReadAudienceRestrictionCondition", theoryData);
            var context = new CompareContext($"{this}.ReadAudienceRestrictionCondition, {theoryData.TestId}");
            try
            {
                var reader = XmlUtilities.CreateDictionaryReader(theoryData.AudienceRestrictionConditionTestSet.Xml);
                var audienceRestrictionCondition = (theoryData.SamlSerializer as SamlSerializerPublic).ReadAudienceRestrictionConditionPublic(reader);
                theoryData.ExpectedException.ProcessNoException();

                IdentityComparer.AreEqual(audienceRestrictionCondition, theoryData.AudienceRestrictionConditionTestSet.AudienceRestrictionCondition, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SamlTheoryData> ReadAudienceRestrictionConditionTheoryData
        {
            get
            {
                return new TheoryData<SamlTheoryData>
                {
                    new SamlTheoryData
                    {
                        AudienceRestrictionConditionTestSet = ReferenceSaml.SamlAudienceRestrictionConditionNoAudience,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11120:"),
                        First = true,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAudienceRestrictionConditionNoAudience)
                    },
                    new SamlTheoryData
                    {
                        AudienceRestrictionConditionTestSet = ReferenceSaml.SamlAudienceRestrictionConditionEmptyAudience,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11125:"),
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAudienceRestrictionConditionEmptyAudience)
                    },
                    new SamlTheoryData
                    {
                        AudienceRestrictionConditionTestSet = ReferenceSaml.SamlAudienceRestrictionConditionInvaidElement,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11134:"),
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAudienceRestrictionConditionInvaidElement)
                    },
                    new SamlTheoryData
                    {
                        AudienceRestrictionConditionTestSet = ReferenceSaml.SamlAudienceRestrictionConditionSingleAudience,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAudienceRestrictionConditionSingleAudience)
                    },
                    new SamlTheoryData
                    {
                        AudienceRestrictionConditionTestSet = ReferenceSaml.SamlAudienceRestrictionConditionMultiAudience,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAudienceRestrictionConditionMultiAudience)
                    }
                };
            }
        }
        #endregion

        #region SamlAuthenticationStatement
        [Theory, MemberData(nameof(ReadAuthenticationStatementTheoryData))]
        public void ReadAuthenticationStatement(SamlTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ReadAuthenticationStatement", theoryData);
            var context = new CompareContext($"{this}.ReadAuthenticationStatement, {theoryData.TestId}");
            try
            {
                var reader = XmlUtilities.CreateDictionaryReader(theoryData.AuthenticationStatementTestSet.Xml);
                var authenticationStatement = (theoryData.SamlSerializer as SamlSerializerPublic).ReadAuthenticationStatementPublic(reader);
                theoryData.ExpectedException.ProcessNoException();

                IdentityComparer.AreEqual(authenticationStatement, theoryData.AuthenticationStatementTestSet.AuthenticationStatement, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SamlTheoryData> ReadAuthenticationStatementTheoryData
        {
            get
            {
                return new TheoryData<SamlTheoryData>
                {
                    new SamlTheoryData
                    {
                        AuthenticationStatementTestSet = ReferenceSaml.SamlAuthenticationStatementMissSubject,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11112:", typeof(XmlReadException)),
                        First = true,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAuthenticationStatementMissSubject)
                    },
                    new SamlTheoryData
                    {
                        AuthenticationStatementTestSet = ReferenceSaml.SamlAuthenticationStatementMissMethod,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11115:"),
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAuthenticationStatementMissMethod)
                    },
                    new SamlTheoryData
                    {
                        AuthenticationStatementTestSet = ReferenceSaml.SamlAuthenticationStatementMissInstant,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11115:"),
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAuthenticationStatementMissInstant)
                    },
                    new SamlTheoryData
                    {
                        AuthenticationStatementTestSet = ReferenceSaml.SamlAuthenticationStatementNoSubjectLocality,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAuthenticationStatementNoSubjectLocality)
                    },
                    new SamlTheoryData
                    {
                        AuthenticationStatementTestSet = ReferenceSaml.SamlAuthenticationStatementNoIPAddress,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAuthenticationStatementNoIPAddress)
                    },
                    new SamlTheoryData
                    {
                        AuthenticationStatementTestSet = ReferenceSaml.SamlAuthenticationStatementNoDNSAddress,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAuthenticationStatementNoDNSAddress)
                    },
                    new SamlTheoryData
                    {
                        AuthenticationStatementTestSet = ReferenceSaml.SamlAuthenticationStatementNoAuthorityBinding,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAuthenticationStatementNoAuthorityBinding)
                    },
                    new SamlTheoryData
                    {
                        AuthenticationStatementTestSet = ReferenceSaml.SamlAuthenticationStatementMissAuthorityKind,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11115:"),
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAuthenticationStatementMissAuthorityKind)
                    },
                    new SamlTheoryData
                    {
                        AuthenticationStatementTestSet = ReferenceSaml.SamlAuthenticationStatementMissLocation,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11513:", typeof(SamlSecurityTokenException)),
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAuthenticationStatementMissLocation)
                    },
                    new SamlTheoryData
                    {
                        AuthenticationStatementTestSet = ReferenceSaml.SamlAuthenticationStatementMissBinding,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11512:", typeof(SamlSecurityTokenException)),
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAuthenticationStatementMissBinding)
                    },
                    new SamlTheoryData
                    {
                        AuthenticationStatementTestSet = ReferenceSaml.SamlAuthenticationStatementValid,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAuthenticationStatementValid)
                    },
                    new SamlTheoryData
                    {
                        AuthenticationStatementTestSet = ReferenceSaml.SamlAuthenticationStatementMultiBinding,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAuthenticationStatementMultiBinding)
                    },
                    new SamlTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11112:", typeof(XmlReadException)),
                        AuthenticationStatementTestSet = ReferenceSaml.SamlAuthenticationStatementEmpty,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAuthenticationStatementEmpty)
                    }
                };
            }
        }
        #endregion

        #region SamlAuthorizationDecisionStatement
        [Theory, MemberData(nameof(ReadAuthorizationDecisionStatementTheoryData))]
        public void ReadAuthorizationDecisionStatement(SamlTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ReadAuthorizationDecisionStatement", theoryData);
            var context = new CompareContext($"{this}.ReadAuthorizationDecisionStatement, {theoryData.TestId}");
            try
            {
                if (theoryData.TestId.Equals("Invalid DecisionType", StringComparison.Ordinal))
                {
                    var authorizationDecision = new SamlAuthorizationDecisionStatement(null, null, "InvalidDecisionType", new List<SamlAction>());
                }
                else
                {
                    var reader = XmlUtilities.CreateDictionaryReader(theoryData.AuthorizationDecisionTestSet.Xml);
                    var statement = (theoryData.SamlSerializer as SamlSerializerPublic).ReadAuthorizationDecisionStatementPublic(reader);
                    theoryData.ExpectedException.ProcessNoException();

                    IdentityComparer.AreEqual(statement, theoryData.AuthorizationDecisionTestSet.AuthorizationDecision, context);
                }
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SamlTheoryData> ReadAuthorizationDecisionStatementTheoryData
        {
            get
            {
                return new TheoryData<SamlTheoryData>
                {
                    new SamlTheoryData
                    {
                        AuthorizationDecisionTestSet = ReferenceSaml.SamlAuthorizationDecisionMissResource,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11115"),
                        First = true,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAuthorizationDecisionMissResource)
                    },
                    new SamlTheoryData
                    {
                        AuthorizationDecisionTestSet = ReferenceSaml.SamlAuthorizationDecisionMissAccessDecision,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11115"),
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAuthorizationDecisionMissAccessDecision)
                    },
                    new SamlTheoryData
                    {
                        AuthorizationDecisionTestSet = ReferenceSaml.SamlAuthorizationDecisionMissSubject,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11112:", typeof(XmlReadException)),
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAuthorizationDecisionMissSubject)
                    },
                    new SamlTheoryData
                    {
                        AuthorizationDecisionTestSet = ReferenceSaml.SamlAuthorizationDecisionMissAction,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11102:"),
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAuthorizationDecisionMissAction)
                    },
                    new SamlTheoryData
                    {
                        AuthorizationDecisionTestSet = ReferenceSaml.SamlAuthorizationDecisionNoEvidence,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAuthorizationDecisionNoEvidence)
                    },
                    new SamlTheoryData
                    {
                        AuthorizationDecisionTestSet = ReferenceSaml.SamlAuthorizationDecisionSingleAction,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAuthorizationDecisionSingleAction)
                    },
                    new SamlTheoryData
                    {
                        AuthorizationDecisionTestSet = ReferenceSaml.SamlAuthorizationDecisionMultiActions,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAuthorizationDecisionMultiActions)
                    },
                    new SamlTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenException), "IDX11508:"),
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = "Invalid DecisionType",
                    },
                    new SamlTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11136:"),
                        AuthorizationDecisionTestSet = ReferenceSaml.SamlAuthorizationDecisionEmpty,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlAuthorizationDecisionEmpty)
                    }
                };
            }
        }
        #endregion

        #region SamlConditions
        [Theory, MemberData(nameof(ReadConditionsTheoryData))]
        public void ReadConditions(SamlTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ReadConditions", theoryData);
            var context = new CompareContext($"{this}.ReadConditions, {theoryData.TestId}");
            try
            {
                var reader = XmlUtilities.CreateDictionaryReader(theoryData.ConditionsTestSet.Xml);
                var conditions = (theoryData.SamlSerializer as SamlSerializerPublic).ReadConditionsPublic(reader);
                theoryData.ExpectedException.ProcessNoException();

                IdentityComparer.AreEqual(conditions, theoryData.ConditionsTestSet.Conditions, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SamlTheoryData> ReadConditionsTheoryData
        {
            get
            {
                return new TheoryData<SamlTheoryData>
                {
                    new SamlTheoryData
                    {
                        ConditionsTestSet = ReferenceSaml.SamlConditionsNoNbf,
                        First = true,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlConditionsNoNbf)
                    },
                    new SamlTheoryData
                    {
                        ConditionsTestSet = ReferenceSaml.SamlConditionsNoNotOnOrAfter,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlConditionsNoNotOnOrAfter)
                    },
                    new SamlTheoryData
                    {
                        ConditionsTestSet = ReferenceSaml.SamlConditionsNoCondition,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlConditionsNoCondition)
                    },
                    new SamlTheoryData
                    {
                        ConditionsTestSet = ReferenceSaml.SamlConditionsSingleCondition,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlConditionsSingleCondition)
                    },
                    new SamlTheoryData
                    {
                        ConditionsTestSet = ReferenceSaml.SamlConditionsMultiCondition,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlConditionsMultiCondition)
                    },
                    new SamlTheoryData
                    {
                        ConditionsTestSet = ReferenceSaml.SamlConditionsEmpty,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlConditionsEmpty)
                    }
                };
            }
        }
        #endregion

        #region SamlEvidence
        [Theory, MemberData(nameof(ReadEvidenceTheoryData))]
        public void ReadEvidence(SamlTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ReadEvidence", theoryData);
            var context = new CompareContext($"{this}.ReadEvidence, {theoryData.TestId}");
            try
            {
                var reader = XmlUtilities.CreateDictionaryReader(theoryData.EvidenceTestSet.Xml);
                var evidence = (theoryData.SamlSerializer as SamlSerializerPublic).ReadEvidencePublic(reader);
                theoryData.ExpectedException.ProcessNoException();

                IdentityComparer.AreEqual(evidence, theoryData.EvidenceTestSet.Evidence, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SamlTheoryData> ReadEvidenceTheoryData
        {
            get
            {
                return new TheoryData<SamlTheoryData>
                {
                    new SamlTheoryData
                    {
                        EvidenceTestSet = ReferenceSaml.SamlEvidenceMissAssertionIDRefAndAssertion,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11133"),
                        First = true,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlEvidenceMissAssertionIDRefAndAssertion)
                    },
                    new SamlTheoryData
                    {
                        EvidenceTestSet = ReferenceSaml.SamlEvidenceWithAssertionIDRef,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlEvidenceWithAssertionIDRef)
                    },
                    new SamlTheoryData
                    {
                        EvidenceTestSet = ReferenceSaml.SamlEvidenceWithAssertions,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlEvidenceWithAssertions)
                    },
                    new SamlTheoryData
                    {
                        EvidenceTestSet = ReferenceSaml.SamlEvidenceWithWrongElement,
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11120"),
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlEvidenceWithWrongElement)
                    },
                    new SamlTheoryData
                    {
                        EvidenceTestSet = ReferenceSaml.SamlEvidenceWithAssertionIDRefAndAssertions,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlEvidenceWithAssertionIDRefAndAssertions)
                    },
                    new SamlTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11133"),
                        EvidenceTestSet = ReferenceSaml.SamlEvidenceEmpty,
                        SamlSerializer = new SamlSerializerPublic(),
                        TestId = nameof(ReferenceSaml.SamlEvidenceEmpty)
                    }
                };
            }
        }
        #endregion

        #region SamlSubject
        [Theory, MemberData(nameof(ReadSubjectTheoryData))]
        public void ReadSubject(SamlTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ReadSubject", theoryData);
            var context = new CompareContext($"{this}.ReadSubject, {theoryData.TestId}");
            try
            {
                var reader = XmlUtilities.CreateDictionaryReader(theoryData.SubjectTestSet.Xml);
                var subject = (theoryData.SamlSerializer as SamlSerializerPublic).ReadSubjectPublic(reader);
                theoryData.ExpectedException.ProcessNoException();

                IdentityComparer.AreEqual(subject, theoryData.SubjectTestSet.Subject, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SamlTheoryData> ReadSubjectTheoryData
        {
            get
            {
                return new TheoryData<SamlTheoryData>
                {
                    new SamlTheoryData
                    {
                        First = true,
                        SamlSerializer = new SamlSerializerPublic(),
                        SubjectTestSet = ReferenceSaml.SamlSubjectNameIdentifierNull,
                        TestId = nameof(ReferenceSaml.SamlSubjectNameIdentifierNull)
                    },
                    new SamlTheoryData
                    {
                        SamlSerializer = new SamlSerializerPublic(),
                        SubjectTestSet = ReferenceSaml.SamlSubjectNoNameQualifier,
                        TestId = nameof(ReferenceSaml.SamlSubjectNoNameQualifier)
                    },
                    new SamlTheoryData
                    {
                        SamlSerializer = new SamlSerializerPublic(),
                        SubjectTestSet = ReferenceSaml.SamlSubjectNoFormat,
                        TestId = nameof(ReferenceSaml.SamlSubjectNoFormat)
                    },
                    new SamlTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11104"),
                        SamlSerializer = new SamlSerializerPublic(),
                        SubjectTestSet = ReferenceSaml.SamlSubjectNameNull,
                        TestId = nameof(ReferenceSaml.SamlSubjectNameNull)
                    },
                    new SamlTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11104"),
                        SamlSerializer = new SamlSerializerPublic(),
                        SubjectTestSet = ReferenceSaml.SamlSubjectNameEmptyString,
                        TestId = nameof(ReferenceSaml.SamlSubjectNameEmptyString)
                    },
                    new SamlTheoryData
                    {
                        SamlSerializer = new SamlSerializerPublic(),
                        SubjectTestSet = ReferenceSaml.SamlSubjectNoConfirmationData,
                        TestId = nameof(ReferenceSaml.SamlSubjectNoConfirmationData)
                    },
                    new SamlTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11114"),
                        SamlSerializer = new SamlSerializerPublic(),
                        SubjectTestSet = ReferenceSaml.SamlSubjectConfirmationMethodNull,
                        TestId = nameof(ReferenceSaml.SamlSubjectConfirmationMethodNull)
                    },
                    new SamlTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(SamlSecurityTokenReadException), "IDX11135"),
                        SamlSerializer = new SamlSerializerPublic(),
                        SubjectTestSet = ReferenceSaml.SamlSubjectConfirmationMethodEmptyString,
                        TestId = nameof(ReferenceSaml.SamlSubjectConfirmationMethodEmptyString)
                    },
                    new SamlTheoryData
                    {
                        SamlSerializer = new SamlSerializerPublic(),
                        SubjectTestSet = ReferenceSaml.SamlSubjectWithNameIdentifierAndConfirmation,
                        TestId = nameof(ReferenceSaml.SamlSubjectWithNameIdentifierAndConfirmation)
                    },
                    new SamlTheoryData
                    {
                        SamlSerializer = new SamlSerializerPublic(),
                        SubjectTestSet = ReferenceSaml.SamlSubjectWithMultiConfirmationMethods,
                        TestId = nameof(ReferenceSaml.SamlSubjectWithMultiConfirmationMethods)
                    },
                    new SamlTheoryData
                    {
                        SamlSerializer = new SamlSerializerPublic(),
                        SubjectTestSet = ReferenceSaml.SamlSubjectEmpty,
                        TestId = nameof(ReferenceSaml.SamlSubjectEmpty)
                    },
                    new SamlTheoryData
                    {
                        SamlSerializer = new SamlSerializerPublic(),
                        SubjectTestSet = ReferenceSaml.SamlSubjectNameIDNotAbsoluteURI,
                        TestId = nameof(ReferenceSaml.SamlSubjectNameIDNotAbsoluteURI)
                    }
                };
            }
        }
        #endregion

        private class SamlSerializerPublic : SamlSerializer
        {
            public SamlAction ReadActionPublic(XmlDictionaryReader reader)
            {
                return base.ReadAction(reader);
            }

            public SamlAdvice ReadAdvicePublic(XmlDictionaryReader reader)
            {
                return base.ReadAdvice(reader);
            }

            public SamlAssertion ReadAssertionPublic(XmlReader reader)
            {
                return base.ReadAssertion(reader);
            }

            public SamlAttribute ReadAttributePublic(XmlDictionaryReader reader)
            {
                return base.ReadAttribute(reader);
            }

            public SamlAttributeStatement ReadAttributeStatementPublic(XmlDictionaryReader reader)
            {
                return base.ReadAttributeStatement(reader);
            }

            public SamlAudienceRestrictionCondition ReadAudienceRestrictionConditionPublic(XmlDictionaryReader reader)
            {
                return base.ReadAudienceRestrictionCondition(reader);
            }

            public SamlAuthenticationStatement ReadAuthenticationStatementPublic(XmlDictionaryReader reader)
            {
                return base.ReadAuthenticationStatement(reader);
            }

            public SamlAuthorizationDecisionStatement ReadAuthorizationDecisionStatementPublic(XmlDictionaryReader reader)
            {
                return base.ReadAuthorizationDecisionStatement(reader);
            }

            public SamlConditions ReadConditionsPublic(XmlDictionaryReader reader)
            {
                return base.ReadConditions(reader);
            }

            public SamlEvidence ReadEvidencePublic(XmlDictionaryReader reader)
            {
                return base.ReadEvidence(reader);
            }

            public SamlSubject ReadSubjectPublic(XmlDictionaryReader reader)
            {
                return base.ReadSubject(reader);
            }
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
