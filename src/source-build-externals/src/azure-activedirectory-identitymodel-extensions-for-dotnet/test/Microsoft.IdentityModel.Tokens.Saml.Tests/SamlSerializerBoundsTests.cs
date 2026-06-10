// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Saml2;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Saml.Tests
{
    public class SamlSerializerBoundsTests
    {
        // Bound enforced by SamlSerializer / Saml2Serializer (private MaxDepth = 8).
        // Tests reference the value indirectly via theory data: [8, 9, 10] (at-or-above)
        // and [1, 7] (lower edge and bound - 1).

        #region SAML2 nested-assertion bounds

        [Theory]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        public void Saml2_ReadAssertion_AtOrAboveBound_ReportsBoundExceeded(int depth)
        {
            var serializer = new Saml2Serializer();
            string xml = BuildNestedSaml2Xml(depth);
            var reader = XmlReader.Create(new StringReader(xml));

            var ex = Assert.Throws<Saml2SecurityTokenReadException>(() => serializer.ReadAssertion(reader));
            Assert.Contains("IDX13111", ex.Message);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(7)]
        public void Saml2_ReadAssertion_WithinBound_DoesNotReportBoundExceeded(int depth)
        {
            var serializer = new Saml2Serializer();
            string xml = BuildNestedSaml2Xml(depth);
            var reader = XmlReader.Create(new StringReader(xml));

            try
            {
                serializer.ReadAssertion(reader);
            }
            catch (Exception ex)
            {
                // Other parse exceptions are acceptable ΓÇö only verify the nesting
                // bound check does not fire when the input is within the bound.
                Assert.DoesNotContain("IDX13111", ex.Message);
            }
        }

        [Fact]
        public void Saml2_ReadAssertion_KnownGoodAssertion_ParsesSuccessfully()
        {
            var serializer = new Saml2Serializer();
            var reader = XmlReader.Create(new StringReader(ReferenceXml.Saml2Valid));

            var assertion = serializer.ReadAssertion(reader);

            Assert.NotNull(assertion);
            Assert.NotNull(assertion.Issuer);
            Assert.False(string.IsNullOrEmpty(assertion.Issuer.Value));
        }

        [Fact]
        public void Saml2_ReadAssertion_NestedAtBoundMinusOne_ParsesSuccessfully()
        {
            // depth = 7 (one below the bound). All 7 nested assertions must parse
            // and the bound check must not fire.
            var serializer = new Saml2Serializer();
            string xml = BuildNestedSaml2Xml(7);
            var reader = XmlReader.Create(new StringReader(xml));

            var assertion = serializer.ReadAssertion(reader);

            Assert.NotNull(assertion);
            Assert.NotNull(assertion.Advice);
        }

        [Fact]
        public void Saml2_ReadAssertion_StateResetsAfterFailure_KnownGoodStillParses()
        {
            var serializer = new Saml2Serializer();

            // Trigger the bound on the first call.
            string deepXml = BuildNestedSaml2Xml(10);
            var reader1 = XmlReader.Create(new StringReader(deepXml));
            var ex = Assert.Throws<Saml2SecurityTokenReadException>(() => serializer.ReadAssertion(reader1));
            Assert.Contains("IDX13111", ex.Message);

            // The same serializer instance must still parse a known-good assertion.
            // This both proves state reset and catches any failure to decrement on the throw path.
            var reader2 = XmlReader.Create(new StringReader(ReferenceXml.Saml2Valid));
            var assertion = serializer.ReadAssertion(reader2);

            Assert.NotNull(assertion);
            Assert.NotNull(assertion.Issuer);
        }

        [Fact]
        public void Saml2_ReadAssertion_StateResetsAfterSuccess_DeepInputStillRejected()
        {
            // After a successful parse the bound counter must be back to zero so
            // a subsequent deep input is still rejected at the expected depth.
            var serializer = new Saml2Serializer();

            var reader1 = XmlReader.Create(new StringReader(ReferenceXml.Saml2Valid));
            Assert.NotNull(serializer.ReadAssertion(reader1));

            var reader2 = XmlReader.Create(new StringReader(BuildNestedSaml2Xml(10)));
            var ex = Assert.Throws<Saml2SecurityTokenReadException>(() => serializer.ReadAssertion(reader2));
            Assert.Contains("IDX13111", ex.Message);
        }

        #endregion

        #region SAML1 nested-assertion bounds

        [Theory]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        public void Saml1_ReadAssertion_AtOrAboveBound_ReportsBoundExceeded(int depth)
        {
            var serializer = new SamlSerializer();
            string xml = BuildNestedSaml1Xml(depth);
            var reader = XmlReader.Create(new StringReader(xml));

            var ex = Assert.Throws<SamlSecurityTokenReadException>(() => serializer.ReadAssertion(reader));
            Assert.Contains("IDX11138", ex.Message);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(7)]
        public void Saml1_ReadAssertion_WithinBound_DoesNotReportBoundExceeded(int depth)
        {
            var serializer = new SamlSerializer();
            string xml = BuildNestedSaml1Xml(depth);
            var reader = XmlReader.Create(new StringReader(xml));

            try
            {
                serializer.ReadAssertion(reader);
            }
            catch (Exception ex)
            {
                Assert.DoesNotContain("IDX11138", ex.Message);
            }
        }

        [Fact]
        public void Saml1_ReadAssertion_KnownGoodAssertion_ParsesSuccessfully()
        {
            var serializer = new SamlSerializer();
            var reader = XmlReader.Create(new StringReader(ReferenceTokens.SamlToken_Valid));

            var assertion = serializer.ReadAssertion(reader);

            Assert.NotNull(assertion);
            Assert.False(string.IsNullOrEmpty(assertion.Issuer));
        }

        [Fact]
        public void Saml1_ReadAssertion_NestedAtBoundMinusOne_ParsesSuccessfully()
        {
            var serializer = new SamlSerializer();
            string xml = BuildNestedSaml1Xml(7);
            var reader = XmlReader.Create(new StringReader(xml));

            var assertion = serializer.ReadAssertion(reader);

            Assert.NotNull(assertion);
            Assert.NotEmpty(assertion.Advice.Assertions);
        }

        [Fact]
        public void Saml1_ReadAssertion_StateResetsAfterFailure_KnownGoodStillParses()
        {
            var serializer = new SamlSerializer();

            string deepXml = BuildNestedSaml1Xml(10);
            var reader1 = XmlReader.Create(new StringReader(deepXml));
            var ex = Assert.Throws<SamlSecurityTokenReadException>(() => serializer.ReadAssertion(reader1));
            Assert.Contains("IDX11138", ex.Message);

            var reader2 = XmlReader.Create(new StringReader(ReferenceTokens.SamlToken_Valid));
            var assertion = serializer.ReadAssertion(reader2);

            Assert.NotNull(assertion);
        }

        [Fact]
        public void Saml1_ReadAssertion_StateResetsAfterSuccess_DeepInputStillRejected()
        {
            var serializer = new SamlSerializer();

            var reader1 = XmlReader.Create(new StringReader(ReferenceTokens.SamlToken_Valid));
            Assert.NotNull(serializer.ReadAssertion(reader1));

            var reader2 = XmlReader.Create(new StringReader(BuildNestedSaml1Xml(10)));
            var ex = Assert.Throws<SamlSecurityTokenReadException>(() => serializer.ReadAssertion(reader2));
            Assert.Contains("IDX11138", ex.Message);
        }

        #endregion

        #region Reader quotas

        [Fact]
        public void BoundedXmlDictionaryReaderQuotas_HasExpectedMaxDepth()
        {
            // Regression: the bounded MaxDepth is the contract these handlers rely on.
            Assert.Equal(32, BoundedXmlDictionaryReaderQuotas.Quotas.MaxDepth);
        }

        [Fact]
        public void BoundedXmlDictionaryReaderQuotas_RejectsXmlBeyondMaxDepth()
        {
            // Using the bounded quotas, an XML element chain deeper than MaxDepth
            // must be rejected by the reader with an XmlException mentioning MaxDepth.
            string deepXml = BuildSimpleNestedXml(depth: BoundedXmlDictionaryReaderQuotas.Quotas.MaxDepth + 5);
            var bytes = Encoding.UTF8.GetBytes(deepXml);

            XmlException ex;
            using (var reader = XmlDictionaryReader.CreateTextReader(bytes, BoundedXmlDictionaryReaderQuotas.Quotas))
            {
                ex = Assert.Throws<XmlException>(() =>
                {
                    while (reader.Read())
                    {
                    }
                });
            }
            Assert.Contains("MaxDepth", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void BoundedXmlDictionaryReaderQuotas_AcceptsXmlAtMaxDepth()
        {
            // Sanity: nesting up to MaxDepth must read cleanly. Catches any off-by-one
            // regression in the bound itself.
            string xml = BuildSimpleNestedXml(depth: BoundedXmlDictionaryReaderQuotas.Quotas.MaxDepth);
            var bytes = Encoding.UTF8.GetBytes(xml);

            using (var reader = XmlDictionaryReader.CreateTextReader(bytes, BoundedXmlDictionaryReaderQuotas.Quotas))
            {
                while (reader.Read())
                {
                }
            }
        }

        private static string BuildSimpleNestedXml(int depth)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < depth; i++)
                sb.Append("<n>");
            for (int i = 0; i < depth; i++)
                sb.Append("</n>");
            return sb.ToString();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Builds a SAML 2.0 assertion XML fragment with the requested nesting depth.
        /// Depth 1 = single assertion; depth 2 = assertion containing one nested assertion via Advice; and so on.
        /// </summary>
        private static string BuildNestedSaml2Xml(int depth)
        {
            const string ns = "urn:oasis:names:tc:SAML:2.0:assertion";
            var sb = new StringBuilder();

            for (int i = 0; i < depth; i++)
            {
                sb.AppendFormat(
                    "<Assertion Version=\"2.0\" ID=\"_id{0}\" IssueInstant=\"2017-03-17T18:33:37.095Z\" xmlns=\"{1}\">",
                    i, ns);
                sb.Append("<Issuer>http://issuer.com</Issuer>");
                sb.Append("<Subject><NameID>user@example.com</NameID></Subject>");
                if (i < depth - 1)
                    sb.Append("<Advice>");
            }

            for (int i = depth - 1; i >= 0; i--)
            {
                sb.Append("</Assertion>");
                if (i > 0)
                    sb.Append("</Advice>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Builds a SAML 1.1 assertion XML fragment with the requested nesting depth.
        /// </summary>
        private static string BuildNestedSaml1Xml(int depth)
        {
            const string ns = "urn:oasis:names:tc:SAML:1.0:assertion";
            var sb = new StringBuilder();

            for (int i = 0; i < depth; i++)
            {
                sb.AppendFormat(
                    "<Assertion MajorVersion=\"1\" MinorVersion=\"1\" AssertionID=\"_id{0}\" Issuer=\"http://issuer.com\" IssueInstant=\"2017-03-17T18:33:37.095Z\" xmlns=\"{1}\">",
                    i, ns);
                if (i < depth - 1)
                    sb.Append("<Advice>");
            }

            AppendSaml1Statement(sb);
            sb.Append("</Assertion>");

            for (int i = depth - 2; i >= 0; i--)
            {
                sb.Append("</Advice>");
                AppendSaml1Statement(sb);
                sb.Append("</Assertion>");
            }

            return sb.ToString();
        }

        private static void AppendSaml1Statement(StringBuilder sb)
        {
            sb.Append("<AttributeStatement>");
            sb.Append("<Subject><NameIdentifier>user@example.com</NameIdentifier></Subject>");
            sb.Append("<Attribute AttributeName=\"role\" AttributeNamespace=\"http://example.com\">");
            sb.Append("<AttributeValue>admin</AttributeValue>");
            sb.Append("</Attribute>");
            sb.Append("</AttributeStatement>");
        }

        #endregion
    }
}