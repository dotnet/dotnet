// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Microsoft.IdentityModel.Xml;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens.Saml
{
    /// <summary>
    /// Reads and writes SamlAssertions
    /// </summary>
    public class SamlSerializer
    {
        private DSigSerializer _dsigSerializer = DSigSerializer.Default;
        private string _prefix = SamlConstants.Prefix;

        /// <summary>
        /// Instantiates a new instance of <see cref="SamlSerializer"/>.
        /// </summary>
        public SamlSerializer()
        {
        }

        /// <summary>
        /// Gets or sets the <see cref="DSigSerializer"/> to use for reading / writing the <see cref="Xml.Signature"/>
        /// </summary>
        /// <exception cref="ArgumentNullException">if value is null.</exception>
        /// <remarks>Will be passed to readers that process xmlDsig such as <see cref="EnvelopedSignatureReader"/> and <see cref="EnvelopedSignatureWriter"/>.</remarks>
        public DSigSerializer DSigSerializer
        {
            get => _dsigSerializer;
            set => _dsigSerializer = value ?? throw LogArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets the prefix to use when writing xml.
        /// </summary>
        /// <exception cref="ArgumentNullException">if value is null.</exception>
        public string Prefix
        {
            get => _prefix;
            set => _prefix = value ?? throw LogExceptionMessage(new ArgumentNullException(nameof(value)));
        }

        /// <summary>
        /// Determines whether a URI is valid and can be created using the specified UriKind.
        /// Uri.TryCreate is used here, which is more lax than Uri.IsWellFormedUriString.
        /// The reason we use this function is because IsWellFormedUriString will reject valid URIs if they are IPv6 or require escaping.
        /// </summary>
        /// <param name="uriString">The string to check.</param>
        /// <param name="uriKind">The type of URI (usually UriKind.Absolute)</param>
        /// <returns>True if the URI is valid, false otherwise.</returns>
        internal static bool CanCreateValidUri(string uriString, UriKind uriKind)
        {
            return Uri.TryCreate(uriString, uriKind, out Uri tempUri);
        }

        internal static bool IsAssertionIdValid(string assertionId)
        {
            if (string.IsNullOrEmpty(assertionId))
                return false;

            // The first character of the Assertion ID should be a letter or a '_'
            return (((assertionId[0] >= 'A') && (assertionId[0] <= 'Z')) ||
                ((assertionId[0] >= 'a') && (assertionId[0] <= 'z')) ||
                (assertionId[0] == '_'));
        }

        internal static Exception LogReadException(string format, params object[] args)
        {
            return LogExceptionMessage(new SamlSecurityTokenReadException(FormatInvariant(format, args)));
        }

        internal static Exception LogReadException(string format, Exception inner, params object[] args)
        {
            return LogExceptionMessage(new SamlSecurityTokenReadException(FormatInvariant(format, args), inner));
        }

        internal static Exception LogWriteException(string format, params object[] args)
        {
            return LogExceptionMessage(new SamlSecurityTokenWriteException(FormatInvariant(format, args)));
        }

        internal static Exception LogWriteException(string format, Exception inner, params object[] args)
        {
            return LogExceptionMessage(new SamlSecurityTokenWriteException(FormatInvariant(format, args), inner));
        }

        /// <summary>
        /// Read the &lt;saml:Action> element.
        /// </summary>
        /// <param name="reader">A <see cref="XmlDictionaryReader"/> positioned at a <see cref="SamlAction"/> element.</param>
        /// <returns>A <see cref="SamlAction"/> instance.</returns>
        protected virtual SamlAction ReadAction(XmlDictionaryReader reader)
        {
            XmlUtil.CheckReaderOnEntry(reader, SamlConstants.Elements.Action, SamlConstants.Namespace);

            if (reader.IsEmptyElement)
                throw LogReadException(LogMessages.IDX11137);

            try
            {
                // @xsi:type
                XmlUtil.ValidateXsiType(reader, SamlConstants.Types.ActionType, SamlConstants.Namespace);

                // @Namespace - optional. If this element is absent, the default namespace is in effect.
                // @attributes
                var namespaceValue = reader.GetAttribute(SamlConstants.Attributes.Namespace);
                if (!string.IsNullOrEmpty(namespaceValue))
                {
                    if (!CanCreateValidUri(namespaceValue, UriKind.Absolute))
                        throw LogReadException(LogMessages.IDX11111, SamlConstants.Elements.Action, SamlConstants.Attributes.Namespace, namespaceValue);
                }
                else
                    namespaceValue = SamlConstants.DefaultActionNamespace;

                return new SamlAction(reader.ReadElementContentAsString(), new Uri(namespaceValue));
            }
            catch (Exception ex)
            {
                if (ex is SamlSecurityTokenReadException)
                    throw;

                throw LogReadException(LogMessages.IDX11112, ex, SamlConstants.Elements.Action, ex);
            }
        }

        /// <summary>
        /// Reads the &lt;saml:Advice> element.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The Advice element has an extensibility point to allow XML elements
        /// from non-SAML namespaces to be included. By default, because the 
        /// Advice may be ignored without affecting the semantics of the 
        /// assertion, any such elements are ignored. To handle the processing
        /// of those elements, override this method.
        /// </para>
        /// </remarks>
        /// <param name="reader">A <see cref="XmlReader"/> positioned at a <see cref="SamlAdvice"/> element.</param>
        /// <returns>A <see cref="SamlAdvice"/> instance.</returns>
        protected virtual SamlAdvice ReadAdvice(XmlDictionaryReader reader)
        {
            XmlUtil.CheckReaderOnEntry(reader, SamlConstants.Elements.Advice, SamlConstants.Namespace);

            try
            {
                var advice = new SamlAdvice();

                // @xsi:type
                XmlUtil.ValidateXsiType(reader, SamlConstants.Types.AdviceType, SamlConstants.Namespace);

                // SAML Advice is an optional element and all its child elements are optional
                // too. So we may have an empty saml:Advice element in the saml token.
                if (reader.IsEmptyElement)
                {
                    // Just issue a read for the empty element.
                    reader.Read();
                    return advice;
                }

                reader.MoveToContent();
                reader.Read();
                while (reader.IsStartElement())
                {
                    if (reader.IsStartElement(SamlConstants.Elements.AssertionIDReference, SamlConstants.Namespace))
                        advice.AssertionIdReferences.Add(reader.ReadElementContentAsString());
                    else if (reader.IsStartElement(SamlConstants.Elements.Assertion, SamlConstants.Namespace))
                        advice.Assertions.Add(ReadAssertion(reader));
                    else
                        throw LogReadException(LogMessages.IDX11126, SamlConstants.Elements.Advice, reader.Name);
                }

                reader.MoveToContent();
                reader.ReadEndElement();
                return advice;
            }
            catch (Exception ex)
            {
                if (ex is SamlSecurityTokenReadException)
                    throw;

                throw LogReadException(LogMessages.IDX11112, ex, SamlConstants.Elements.Advice, ex);
            }
        }

        /// <summary>
        /// Reads a &lt;saml:Assertion> element.
        /// </summary>
        /// <param name="reader">A <see cref="XmlReader"/> positioned at a 'saml:assertion' element.</param>
        /// <returns>A <see cref="SamlAssertion"/> instance.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="reader"/> is null.</exception>
        public virtual SamlAssertion ReadAssertion(XmlReader reader)
        {
            XmlUtil.CheckReaderOnEntry(reader, SamlConstants.Elements.Assertion, SamlConstants.Namespace);

            try
            {
                var envelopeReader = new EnvelopedSignatureReader(reader) { Serializer = DSigSerializer };

                // @xsi:type
                XmlUtil.ValidateXsiType(envelopeReader, SamlConstants.Types.AssertionType, SamlConstants.Namespace);

                // @MajorVersion - required - must be "1"
                var majorVersion = envelopeReader.GetAttribute(SamlConstants.Attributes.MajorVersion);
                if (string.IsNullOrEmpty(majorVersion))
                    throw LogReadException(LogMessages.IDX11115, MarkAsNonPII(SamlConstants.Elements.Assertion), MarkAsNonPII(SamlConstants.Attributes.MajorVersion));

                if (!majorVersion.Equals(SamlConstants.MajorVersionValue))
                    throw LogReadException(LogMessages.IDX11116, MarkAsNonPII(majorVersion));

                // @MinorVersion - required - must be "1"
                var minorVersion = envelopeReader.GetAttribute(SamlConstants.Attributes.MinorVersion);
                if (string.IsNullOrEmpty(minorVersion))
                    throw LogReadException(LogMessages.IDX11115, MarkAsNonPII(SamlConstants.Elements.Assertion), MarkAsNonPII(SamlConstants.Attributes.MinorVersion));

                if (!minorVersion.Equals(SamlConstants.MinorVersionValue))
                    throw LogReadException(LogMessages.IDX11117, MarkAsNonPII(minorVersion));

                // @AssertionId - required
                var assertionId = envelopeReader.GetAttribute(SamlConstants.Attributes.AssertionID);
                if (string.IsNullOrEmpty(assertionId))
                    throw LogReadException(LogMessages.IDX11115, MarkAsNonPII(SamlConstants.Elements.Assertion), MarkAsNonPII(SamlConstants.Attributes.AssertionID));

                if (!IsAssertionIdValid(assertionId))
                    throw LogReadException(LogMessages.IDX11121, assertionId);

                // @Issuer - required
                var issuer = envelopeReader.GetAttribute(SamlConstants.Attributes.Issuer);
                if (string.IsNullOrEmpty(issuer))
                    throw LogReadException(LogMessages.IDX11115, MarkAsNonPII(SamlConstants.Elements.Assertion), MarkAsNonPII(SamlConstants.Attributes.Issuer));

                // @IssueInstant - required
                var issueInstantAttribute = envelopeReader.GetAttribute(SamlConstants.Attributes.IssueInstant);
                if (string.IsNullOrEmpty(issueInstantAttribute))
                    throw LogReadException(LogMessages.IDX11115, MarkAsNonPII(SamlConstants.Elements.Assertion), MarkAsNonPII(SamlConstants.Attributes.IssueInstant));

                var issueInstant = DateTime.ParseExact(issueInstantAttribute, SamlConstants.AcceptedDateTimeFormats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None).ToUniversalTime();

                envelopeReader.MoveToContent();
                envelopeReader.Read();

                SamlConditions conditions = null;
                // <Conditions> 0-1
                if (envelopeReader.IsStartElement(SamlConstants.Elements.Conditions, SamlConstants.Namespace))
                    conditions = ReadConditions(envelopeReader) ?? throw LogReadException(LogMessages.IDX11127);

                SamlAdvice advice = null;
                // <Advice> 0-1
                if (envelopeReader.IsStartElement(SamlConstants.Elements.Advice, SamlConstants.Namespace))
                    advice = ReadAdvice(envelopeReader) ?? throw LogReadException(LogMessages.IDX11128);

                List<SamlStatement> statements = new List<SamlStatement>();
                while (envelopeReader.IsStartElement())
                {
                    // <ds:Signature> 0-1 read by EnvelopedSignatureReader
                    // will move to next element
                    if (envelopeReader.IsStartElement(XmlSignatureConstants.Elements.Signature, XmlSignatureConstants.Namespace))
                        envelopeReader.Read();
                    else
                    {
                        // <Statement|AuthnStatement|AuthzDecisionStatement|AttributeStatement>, 0-OO
                        var statement = ReadStatement(envelopeReader);
                        if (statement == null)
                            throw LogReadException(LogMessages.IDX11129);

                        statements.Add(statement);
                    }
                }

                if (statements.Count == 0)
                    throw LogReadException(LogMessages.IDX11130, SamlConstants.Elements.Assertion);

                envelopeReader.MoveToContent();
                envelopeReader.ReadEndElement();

                return new SamlAssertion(assertionId, issuer, issueInstant, conditions, advice, statements)
                {
                    // attach signature for verification
                    Signature = envelopeReader.Signature,
                    XmlTokenStream = envelopeReader.XmlTokenStream
                };
            }
            catch (Exception ex)
            {
                if (ex is SamlSecurityTokenException)
                    throw;

                throw LogReadException(LogMessages.IDX11122, ex, SamlConstants.Elements.Assertion, ex);
            }
        }

        /// <summary>
        /// Reads the &lt;saml:Attribute> element.
        /// </summary>
        /// <remarks>
        /// The default implementation requires that the content of the
        /// Attribute element be a simple string. To handle complex content
        /// or content of declared simple types other than xs:string, override
        /// this method.
        /// </remarks>
        /// <param name="reader">An <see cref="XmlReader"/> positioned at a <see cref="SamlAttribute"/> element.</param>
        /// <returns>A <see cref="SamlAttribute"/> instance.</returns>
        public virtual SamlAttribute ReadAttribute(XmlDictionaryReader reader)
        {
            XmlUtil.CheckReaderOnEntry(reader, SamlConstants.Elements.Attribute, SamlConstants.Namespace);

            try
            {
                var attribute = new SamlAttribute();

                // @xsi:type
                XmlUtil.ValidateXsiType(reader, SamlConstants.Types.AttributeType, SamlConstants.Namespace);

                attribute.Name = reader.GetAttribute(SamlConstants.Attributes.AttributeName);
                if (string.IsNullOrEmpty(attribute.Name))
                    throw LogReadException(LogMessages.IDX11115, SamlConstants.Elements.Attribute, SamlConstants.Attributes.AttributeName);

                attribute.Namespace = reader.GetAttribute(SamlConstants.Attributes.AttributeNamespace);
                if (string.IsNullOrEmpty(attribute.Namespace))
                    throw LogReadException(LogMessages.IDX11115, SamlConstants.Elements.Attribute, SamlConstants.Attributes.AttributeNamespace);

                attribute.ClaimType = attribute.Namespace + "/" + attribute.Name;

                // The following code is for aligning to the old version.
                string originalIssuer = reader.GetAttribute(SamlConstants.Attributes.OriginalIssuer, SamlConstants.ClaimType2009Namespace);
                if (originalIssuer == null)
                    originalIssuer = reader.GetAttribute(SamlConstants.Attributes.OriginalIssuer, SamlConstants.MsIdentityNamespaceUri);

                if (originalIssuer == null)
                    originalIssuer = reader.GetAttribute(SamlConstants.Attributes.OriginalIssuer);

                if (originalIssuer != null)
                    attribute.OriginalIssuer = originalIssuer;

                reader.MoveToContent();
                reader.Read();
                // We will load all Attributes as a string value by default.
                while (reader.IsStartElement(SamlConstants.Elements.AttributeValue, SamlConstants.Namespace))
                {
                    if (!reader.IsEmptyElement)
                        attribute.Values.Add(reader.ReadElementContentAsString());
                    else
                        reader.Read();
                }

                if (attribute.Values.Count == 0)
                    LogWarning(LogMessages.IDX11132);

                reader.MoveToContent();
                reader.ReadEndElement();

                return attribute;
            }
            catch (Exception ex)
            {
                if (ex is SamlSecurityTokenReadException)
                    throw;

                throw LogReadException(LogMessages.IDX11112, ex, SamlConstants.Elements.Attribute, ex);
            }
        }

        /// <summary>
        /// Reads the &lt;saml:AttributeStatement> element, or a
        /// &lt;saml:Statement element that specifies an xsi:type of
        /// saml:AttributeStatementType.
        /// </summary>
        /// <param name="reader">A <see cref="XmlReader"/> positioned at a <see cref="SamlAttributeStatement"/> element.</param>
        /// <returns>A <see cref="SamlAttributeStatement"/> instance.</returns>
        protected virtual SamlAttributeStatement ReadAttributeStatement(XmlDictionaryReader reader)
        {
            XmlUtil.CheckReaderOnEntry(reader, SamlConstants.Elements.AttributeStatement, SamlConstants.Namespace);

            try
            {
                // @xsi:type
                XmlUtil.ValidateXsiType(reader, SamlConstants.Types.AttributeStatementType, SamlConstants.Namespace);

                reader.ReadStartElement();
                var statement = new SamlAttributeStatement() { Subject = ReadSubject(reader) };
                while (reader.IsStartElement())
                {
                    if (reader.IsStartElement(SamlConstants.Elements.Attribute, SamlConstants.Namespace))
                    {
                        SamlAttribute attribute = ReadAttribute(reader);
                        if (attribute == null)
                            throw LogExceptionMessage(new SamlSecurityTokenException(LogMessages.IDX11511));

                        statement.Attributes.Add(attribute);
                    }
                    else
                        break;
                }

                // Each Attribute statement should have at least one attribute.
                if (statement.Attributes.Count == 0)
                    throw LogReadException(LogMessages.IDX11131);

                reader.MoveToContent();
                reader.ReadEndElement();

                return statement;
            }
            catch (Exception ex)
            {
                if (ex is SamlSecurityTokenReadException)
                    throw;

                throw LogReadException(LogMessages.IDX11112, ex, SamlConstants.Elements.AttributeStatement, ex);
            }
        }

        /// <summary>
        /// Reads the &lt;saml:AudienceRestriction> element or a 
        /// &lt;saml:Condition> element that specifies an xsi:type
        /// of saml:AudienceRestrictionType.
        /// </summary>
        /// <param name="reader">A <see cref="XmlReader"/> positioned at a <see cref="SamlAudienceRestrictionCondition"/> element.</param>
        /// <returns></returns>
        protected virtual SamlAudienceRestrictionCondition ReadAudienceRestrictionCondition(XmlDictionaryReader reader)
        {
            XmlUtil.CheckReaderOnEntry(reader, SamlConstants.Elements.AudienceRestrictionCondition, SamlConstants.Namespace);

            try
            {
                // disallow empty
                if (reader.IsEmptyElement)
                    throw LogReadException(LogMessages.IDX11123, SamlConstants.Elements.AudienceRestrictionCondition);

                // @xsi:type -- if we're a <Condition> element, this declaration must be present
                XmlUtil.ValidateXsiType(reader, SamlConstants.Types.AudienceRestrictionType, SamlConstants.Namespace);

                reader.ReadStartElement();
                var audienceRestrictionCondition = new SamlAudienceRestrictionCondition();

                while (reader.IsStartElement())
                {
                    if (reader.IsStartElement(SamlConstants.Elements.Audience, SamlConstants.Namespace))
                    {
                        string audience = reader.ReadElementContentAsString();
                        if (string.IsNullOrEmpty(audience))
                            throw LogReadException(LogMessages.IDX11125, SamlConstants.Elements.Audience);

                        audienceRestrictionCondition.Audiences.Add(new Uri(audience));
                    }
                    else
                        throw LogReadException(LogMessages.IDX11134, SamlConstants.Elements.Audience, reader.Name);
                }

                if (audienceRestrictionCondition.Audiences.Count == 0)
                    throw LogReadException(LogMessages.IDX11120, SamlConstants.Elements.Audience, SamlConstants.Elements.AudienceRestrictionCondition);

                reader.MoveToContent();
                reader.ReadEndElement();

                return audienceRestrictionCondition;
            }
            catch (Exception ex)
            {
                if (ex is SamlSecurityTokenReadException)
                    throw;

                throw LogReadException(LogMessages.IDX11112, ex, SamlConstants.Elements.Audience, ex);
            }
        }

        /// <summary>
        /// Read the saml:AuthenticationStatement.
        /// </summary>
        /// <param name="reader">XmlReader positioned at a saml:AuthenticationStatement.</param>
        /// <returns>SamlAuthenticationStatement</returns>
        /// <exception cref="ArgumentNullException">The input parameter 'reader' is null.
        /// or the statement contains a unknown child element.</exception>
        protected virtual SamlAuthenticationStatement ReadAuthenticationStatement(XmlDictionaryReader reader)
        {
            XmlUtil.CheckReaderOnEntry(reader, SamlConstants.Elements.AuthenticationStatement, SamlConstants.Namespace);

            try
            {
                // @xsi:type
                XmlUtil.ValidateXsiType(reader, SamlConstants.Types.AuthnContextType, SamlConstants.Namespace);

                var authenticationStatement = new SamlAuthenticationStatement();

                var authInstance = reader.GetAttribute(SamlConstants.Attributes.AuthenticationInstant);
                if (string.IsNullOrEmpty(authInstance))
                    throw LogReadException(LogMessages.IDX11115, SamlConstants.Elements.AuthenticationStatement, SamlConstants.Attributes.AuthenticationInstant);

                authenticationStatement.AuthenticationInstant = DateTime.ParseExact(
                    authInstance, SamlConstants.AcceptedDateTimeFormats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None).ToUniversalTime();

                var authenticationMethod = reader.GetAttribute(SamlConstants.Attributes.AuthenticationMethod);
                if (string.IsNullOrEmpty(authenticationMethod))
                    throw LogReadException(LogMessages.IDX11115, SamlConstants.Elements.AuthenticationStatement, SamlConstants.Attributes.AuthenticationMethod);

                authenticationStatement.AuthenticationMethod = authenticationMethod;

                reader.ReadStartElement();
                authenticationStatement.Subject = ReadSubject(reader);
                if (reader.IsStartElement(SamlConstants.Elements.SubjectLocality, SamlConstants.Namespace))
                {
                    authenticationStatement.DnsAddress = reader.GetAttribute(SamlConstants.Elements.DNSAddress);
                    authenticationStatement.IPAddress = reader.GetAttribute(SamlConstants.Elements.IPAddress);

                    bool isEmptyElement = reader.IsEmptyElement;
                    reader.MoveToContent();
                    reader.Read();

                    if (!isEmptyElement)
                        reader.ReadEndElement();
                }

                while (reader.IsStartElement())
                {
                    authenticationStatement.AuthorityBindings.Add(ReadAuthorityBinding(reader));
                }

                reader.MoveToContent();
                reader.ReadEndElement();

                return authenticationStatement;
            }
            catch (Exception ex)
            {
                if (ex is SamlSecurityTokenReadException)
                    throw;

                throw LogReadException(LogMessages.IDX11112, ex, SamlConstants.Elements.AuthenticationStatement, ex);
            }
        }

        /// <summary>
        /// Reads a &lt;saml:Binding> element.
        /// </summary>
        /// <param name="reader">A <see cref="XmlReader"/> positioned at a <see cref="SamlAssertion"/> element.</param>
        /// <returns>A <see cref="SamlAuthorityBinding"/> instance.</returns>
        protected virtual SamlAuthorityBinding ReadAuthorityBinding(XmlDictionaryReader reader)
        {
            XmlUtil.CheckReaderOnEntry(reader, SamlConstants.Elements.AuthorityBinding, SamlConstants.Namespace);

            try
            {
                // @xsi:type
                XmlUtil.ValidateXsiType(reader, SamlConstants.Types.AuthorityBindingType, SamlConstants.Namespace);

                var authKind = reader.GetAttribute(SamlConstants.Attributes.AuthorityKind);
                if (string.IsNullOrEmpty(authKind))
                    throw LogReadException(LogMessages.IDX11115, SamlConstants.Elements.AuthorityBinding, SamlConstants.Attributes.AuthorityKind);

                string[] authKindParts = authKind.Split(':');
                if (authKindParts.Length > 2)
                    throw LogReadException(LogMessages.IDX11108, authKind);

                string localName;
                string prefix;
                string nameSpace;
                if (authKindParts.Length == 2)
                {
                    prefix = authKindParts[0];
                    localName = authKindParts[1];
                }
                else
                {
                    prefix = string.Empty;
                    localName = authKindParts[0];
                }

                nameSpace = reader.LookupNamespace(prefix);
                XmlQualifiedName authorityKind;

                if (string.IsNullOrEmpty(nameSpace))
                    authorityKind = new XmlQualifiedName(authKind, nameSpace);
                else
                    authorityKind = new XmlQualifiedName(localName, nameSpace);

                var binding = reader.GetAttribute(SamlConstants.Attributes.Binding);
                if (string.IsNullOrEmpty(binding))
                    throw LogExceptionMessage(new SamlSecurityTokenException(LogMessages.IDX11512));

                var location = reader.GetAttribute(SamlConstants.Attributes.Location);
                if (string.IsNullOrEmpty(location))
                    throw LogExceptionMessage(new SamlSecurityTokenException(LogMessages.IDX11513));

                bool isEmptyElement = reader.IsEmptyElement;
                reader.MoveToContent();
                reader.Read();

                if (!isEmptyElement)
                    reader.ReadEndElement();

                return new SamlAuthorityBinding(authorityKind, binding, location);
            }
            catch (Exception ex)
            {
                if (ex is SamlSecurityTokenReadException)
                    throw;

                throw LogReadException(LogMessages.IDX11112, ex, SamlConstants.Elements.AuthorityBinding, ex);
            }
        }

        /// <summary>
        /// Reads the &lt;saml:AuthzDecisionStatement> element or a
        /// &lt;saml:Statement element that specifies an xsi:type of
        /// saml:AuthzDecisionStatementType.
        /// </summary>
        /// <param name="reader">A <see cref="XmlReader"/> positioned at a <see cref="SamlAuthorizationDecisionStatement"/> element.</param>
        /// <returns>A <see cref="SamlAuthorizationDecisionStatement"/> instance.</returns>
        protected virtual SamlAuthorizationDecisionStatement ReadAuthorizationDecisionStatement(XmlDictionaryReader reader)
        {
            XmlUtil.CheckReaderOnEntry(reader, SamlConstants.Elements.AuthorizationDecisionStatement, SamlConstants.Namespace);

            try
            {
                // @xsi:type
                XmlUtil.ValidateXsiType(reader, SamlConstants.Types.AuthzDecisionStatementType, SamlConstants.Namespace, false);

                var statement = new SamlAuthorizationDecisionStatement();

                var resource = reader.GetAttribute(SamlConstants.Attributes.Resource);
                if (string.IsNullOrEmpty(resource))
                    throw LogReadException(LogMessages.IDX11115, SamlConstants.Elements.AuthorizationDecisionStatement, SamlConstants.Attributes.Resource);

                statement.Resource = resource;

                var decision = reader.GetAttribute(SamlConstants.Attributes.Decision);
                if (string.IsNullOrEmpty(decision))
                    throw LogReadException(LogMessages.IDX11115, SamlConstants.Elements.AuthorizationDecisionStatement, SamlConstants.Attributes.Decision);

                statement.Decision = decision;

                if (reader.IsEmptyElement)
                    throw LogReadException(LogMessages.IDX11136);

                reader.ReadStartElement();
                statement.Subject = ReadSubject(reader);
                while (reader.IsStartElement())
                {
                    if (reader.IsStartElement(SamlConstants.Elements.Action, SamlConstants.Namespace))
                        statement.Actions.Add(ReadAction(reader));
                    else if (reader.IsStartElement(SamlConstants.Elements.Evidence, SamlConstants.Namespace))
                    {
                        if (statement.Evidence != null)
                            throw LogReadException(LogMessages.IDX11100, SamlConstants.Elements.Evidence);

                        statement.Evidence = ReadEvidence(reader);
                    }
                    else
                        throw LogReadException(LogMessages.IDX11124, reader.Name, SamlConstants.Elements.AuthorizationDecisionStatement);
                }

                if (statement.Actions.Count == 0)
                    throw LogReadException(LogMessages.IDX11102);

                reader.MoveToContent();
                reader.ReadEndElement();

                return statement;
            }
            catch (Exception ex)
            {
                if (ex is SamlSecurityTokenReadException)
                    throw;

                throw LogReadException(LogMessages.IDX11112, ex, SamlConstants.Elements.AuthorizationDecisionStatement, ex);
            }
        }

        /// <summary>
        /// Reads the &lt;saml:Condition> element.
        /// </summary>
        /// <param name="reader">A <see cref="XmlReader"/> positioned at a <see cref="SamlCondition"/> element.</param>
        /// <returns>A <see cref="SamlCondition"/> instance.</returns>
        protected virtual SamlCondition ReadCondition(XmlDictionaryReader reader)
        {
            if (reader == null)
                throw LogArgumentNullException(nameof(reader));

            if (reader.IsStartElement(SamlConstants.Elements.AudienceRestrictionCondition, SamlConstants.Namespace))
                return ReadAudienceRestrictionCondition(reader);
            else if (reader.IsStartElement(SamlConstants.Elements.DoNotCacheCondition, SamlConstants.Namespace))
                return ReadDoNotCacheCondition(reader);
            else
                throw LogReadException(LogMessages.IDX11118, reader.Name);
        }

        /// <summary>
        /// Reads the &lt;saml:Conditions> element.
        /// </summary>
        /// <remarks>
        /// To handle custom &lt;saml:Conditions> elements, override this
        /// method.
        /// </remarks>
        /// <param name="reader">A <see cref="XmlReader"/> positioned at a <see cref="SamlConditions"/> element.</param>
        /// <returns>A <see cref="SamlConditions"/> instance.</returns>
        protected virtual SamlConditions ReadConditions(XmlDictionaryReader reader)
        {
            XmlUtil.CheckReaderOnEntry(reader, SamlConstants.Elements.Conditions, SamlConstants.Namespace);

            try
            {
                // @xsi:type
                XmlUtil.ValidateXsiType(reader, SamlConstants.Types.ConditionsType, SamlConstants.Namespace);

                var nbf = DateTimeUtil.GetMinValue(DateTimeKind.Utc);
                var time = reader.GetAttribute(SamlConstants.Attributes.NotBefore);
                if (!string.IsNullOrEmpty(time))
                    nbf = DateTime.ParseExact(time, SamlConstants.AcceptedDateTimeFormats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None).ToUniversalTime();

                var notOnOrAfter = DateTimeUtil.GetMaxValue(DateTimeKind.Utc);
                time = reader.GetAttribute(SamlConstants.Attributes.NotOnOrAfter);
                if (!string.IsNullOrEmpty(time))
                    notOnOrAfter = DateTime.ParseExact(time, SamlConstants.AcceptedDateTimeFormats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None).ToUniversalTime();

                var conditions = new SamlConditions(nbf, notOnOrAfter);
                // Saml Conditions element is an optional element and all its child element
                // are optional as well. So we can have a empty <saml:Conditions /> element
                // in a valid Saml token.
                if (reader.IsEmptyElement)
                {
                    // Just issue a read to read the Empty element.
                    reader.MoveToContent();
                    reader.Read();
                    return conditions;
                }

                reader.ReadStartElement();
                while (reader.IsStartElement())
                {
                    conditions.Conditions.Add(ReadCondition(reader));
                }

                reader.ReadEndElement();

                return conditions;
            }
            catch (Exception ex)
            {
                if (ex is SamlSecurityTokenReadException)
                    throw;

                throw LogReadException(LogMessages.IDX11112, ex, SamlConstants.Elements.Conditions, ex);
            }
        }

        /// <summary>
        /// Reads the &lt;saml:DoNotCacheCondition> element.
        /// </summary>
        /// <param name="reader">A <see cref="XmlReader"/> positioned at a <see cref="SamlConditions"/> element.</param>
        /// <returns>A <see cref="SamlDoNotCacheCondition"/> instance.</returns>
        protected virtual SamlDoNotCacheCondition ReadDoNotCacheCondition(XmlDictionaryReader reader)
        {
            XmlUtil.CheckReaderOnEntry(reader, SamlConstants.Elements.DoNotCacheCondition, SamlConstants.Namespace);

            try
            {
                // @xsi:type
                XmlUtil.ValidateXsiType(reader, SamlConstants.Types.DoNotCacheConditionType, SamlConstants.Namespace);

                if (reader.IsEmptyElement)
                {
                    reader.MoveToContent();
                    reader.Read();
                    return new SamlDoNotCacheCondition();
                }

                reader.MoveToContent();
                reader.Read();
                reader.ReadEndElement();

                return new SamlDoNotCacheCondition();
            }
            catch (Exception ex)
            {
                if (ex is SamlSecurityTokenReadException)
                    throw;

                throw LogReadException(LogMessages.IDX11112, ex, SamlConstants.Elements.DoNotCacheCondition, ex);
            }
        }

        /// <summary>
        /// Reads the &lt;saml:Evidence> element.
        /// </summary>
        /// <param name="reader">A <see cref="XmlReader"/> positioned at a <see cref="SamlEvidence"/> element.</param>
        /// <returns>A <see cref="SamlEvidence"/> instance.</returns>
        protected virtual SamlEvidence ReadEvidence(XmlDictionaryReader reader)
        {
            XmlUtil.CheckReaderOnEntry(reader, SamlConstants.Elements.Evidence, SamlConstants.Namespace);

            try
            {
                var evidence = new SamlEvidence();

                reader.Read();
                while (reader.IsStartElement())
                {
                    if (reader.IsStartElement(SamlConstants.Elements.AssertionIDReference, SamlConstants.Namespace))
                        evidence.AssertionIDReferences.Add(reader.ReadElementContentAsString());
                    else if (reader.IsStartElement(SamlConstants.Elements.Assertion, SamlConstants.Namespace))
                        evidence.Assertions.Add(ReadAssertion(reader));
                    else
                        throw LogReadException(LogMessages.IDX11120, SamlConstants.Elements.Evidence, reader.Name);
                }

                if (evidence.AssertionIDReferences.Count == 0 && evidence.Assertions.Count == 0)
                    throw LogReadException(LogMessages.IDX11133);

                reader.MoveToContent();
                reader.ReadEndElement();

                return evidence;
            }
            catch (Exception ex)
            {
                if (ex is SamlSecurityTokenReadException)
                    throw;

                throw LogReadException(LogMessages.IDX11112, ex, SamlConstants.Elements.Evidence, ex);
            }
        }

        /// <summary>
        /// Reads the &lt;saml:Statement> element.
        /// </summary>
        /// <param name="reader">A <see cref="XmlReader"/> positioned at a <see cref="SamlStatement"/> element.</param>
        /// <returns>An instance of <see cref="SamlStatement"/> derived type.</returns>
        /// <remarks>
        /// The default implementation only handles Statement elements which
        /// specify an xsi:type of saml:AttributeStatementType,
        /// saml:AuthnStatementType, and saml:AuthzDecisionStatementType. To
        /// handle custom statements, override this method.
        /// </remarks>
        protected virtual SamlStatement ReadStatement(XmlDictionaryReader reader)
        {
            if (reader == null)
                throw LogArgumentNullException(nameof(reader));

            if (reader.IsStartElement(SamlConstants.Elements.AuthenticationStatement, SamlConstants.Namespace))
                return ReadAuthenticationStatement(reader);
            else if (reader.IsStartElement(SamlConstants.Elements.AttributeStatement, SamlConstants.Namespace))
                return ReadAttributeStatement(reader);
            else if (reader.IsStartElement(SamlConstants.Elements.AuthorizationDecisionStatement, SamlConstants.Namespace))
                return ReadAuthorizationDecisionStatement(reader);
            else
                throw LogReadException(LogMessages.IDX11126, SamlConstants.Elements.Assertion, reader.Name);
        }

        /// <summary>
        /// Read the SamlSubject from the XmlReader.
        /// </summary>
        /// <param name="reader">A <see cref="XmlReader"/> positioned at a <see cref="SamlSubject"/> element.</param>
        /// <returns>An instance of <see cref="SamlSubject"/> .</returns>
        protected virtual SamlSubject ReadSubject(XmlDictionaryReader reader)
        {
            XmlUtil.CheckReaderOnEntry(reader, SamlConstants.Elements.Subject, SamlConstants.Namespace);

            var isEmpty = reader.IsEmptyElement;

            try
            {
                var subject = new SamlSubject();

                reader.Read();

                if (reader.IsStartElement(SamlConstants.Elements.NameIdentifier, SamlConstants.Namespace))
                {
                    // @xsi:type
                    XmlUtil.ValidateXsiType(reader, SamlConstants.Types.NameIDType, SamlConstants.Namespace);

                    var nameFormat = reader.GetAttribute(SamlConstants.Attributes.Format);
                    if (!string.IsNullOrEmpty(nameFormat))
                        subject.NameFormat = nameFormat;

                    var nameQualifier = reader.GetAttribute(SamlConstants.Attributes.NameQualifier);
                    if (!string.IsNullOrEmpty(nameQualifier))
                        subject.NameQualifier = nameQualifier;

                    reader.MoveToContent();
                    subject.Name = reader.ReadElementContentAsString();

                    if (string.IsNullOrEmpty(subject.Name))
                        throw LogReadException(LogMessages.IDX11104);
                }

                if (reader.IsStartElement(SamlConstants.Elements.SubjectConfirmation, SamlConstants.Namespace))
                {
                    // @xsi:type
                    XmlUtil.ValidateXsiType(reader, SamlConstants.Types.SubjectConfirmationDataType, SamlConstants.Namespace);

                    reader.MoveToContent();
                    reader.Read();

                    while (reader.IsStartElement(SamlConstants.Elements.ConfirmationMethod, SamlConstants.Namespace))
                    {
                        string method = reader.ReadElementContentAsString();
                        if (string.IsNullOrEmpty(method))
                            throw LogReadException(LogMessages.IDX11135, SamlConstants.Elements.ConfirmationMethod);

                        subject.ConfirmationMethods.Add(method);
                    }

                    // A SubjectConfirmaton clause should specify at least one ConfirmationMethod.
                    if (subject.ConfirmationMethods.Count == 0)
                        throw LogReadException(LogMessages.IDX11114, SamlConstants.Elements.ConfirmationMethod);

                    // An Authentication protocol specified in the confirmation method might need this
                    // data. Just store this content value as string.
                    if (reader.IsStartElement(SamlConstants.Elements.SubjectConfirmationData, SamlConstants.Namespace))
                    {
                        var confirmationData = reader.ReadElementContentAsString();
                        if (!string.IsNullOrEmpty(confirmationData))
                            subject.ConfirmationData = confirmationData;
                    }

                    if (reader.IsStartElement(XmlSignatureConstants.Elements.KeyInfo, XmlSignatureConstants.Namespace))
                    {
                        subject.KeyInfo = _dsigSerializer.ReadKeyInfo(reader);
                    }

                    if ((subject.ConfirmationMethods.Count == 0) && (string.IsNullOrEmpty(subject.Name)))
                        throw LogReadException(LogMessages.IDX11107);

                    reader.MoveToContent();
                    reader.ReadEndElement();
                }

                if (!isEmpty)
                    reader.ReadEndElement();

                reader.MoveToContent();

                return subject;
            }
            catch (Exception ex)
            {
                if (ex is SamlSecurityTokenReadException)
                    throw;

                throw LogReadException(LogMessages.IDX11112, ex, SamlConstants.Elements.Subject, ex);
            }
        }

        /// <summary>
        /// Writes the &lt;saml:Action> element.
        /// </summary>
        /// <param name="writer">A <see cref="XmlWriter"/> to serialize the <see cref="SamlAction"/>.</param>
        /// <param name="action">The <see cref="SamlAction"/> to serialize.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="action"/> is null.</exception>
        protected virtual void WriteAction(XmlWriter writer, SamlAction action)
        {
            if (writer == null)
                throw LogArgumentNullException(nameof(writer));

            if (action == null)
                throw LogArgumentNullException(nameof(action));

            writer.WriteStartElement(Prefix, SamlConstants.Elements.Action, SamlConstants.Namespace);
            if (!string.IsNullOrEmpty(action.Namespace.OriginalString))
            {
                writer.WriteStartAttribute(SamlConstants.Attributes.ActionNamespaceAttribute, null);
                writer.WriteString(action.Namespace.OriginalString);
                writer.WriteEndAttribute();
            }

            writer.WriteString(action.Value);
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the &lt;saml:Advice> element.
        /// </summary>
        /// <param name="writer">A <see cref="XmlWriter"/> to serialize the <see cref="SamlAdvice"/>.</param>
        /// <param name="advice">The <see cref="SamlAdvice"/> to serialize.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="advice"/> is null.</exception>
        protected virtual void WriteAdvice(XmlWriter writer, SamlAdvice advice)
        {
            if (writer == null)
                throw LogArgumentNullException(nameof(writer));

            if (advice == null)
                throw LogArgumentNullException(nameof(advice));

            // <Advice>
            writer.WriteStartElement(Prefix, SamlConstants.Elements.Advice, SamlConstants.Namespace);

            // <AssertionIdReferences> 0-OO
            foreach (var reference in advice.AssertionIdReferences)
                writer.WriteElementString(Prefix, SamlConstants.Elements.AssertionIDReference, SamlConstants.Namespace, reference);

            // <Assertion> 0-OO
            foreach (var assertion in advice.Assertions)
                WriteAssertion(writer, assertion);

            // </Advice>
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the &lt;Assertion> element.
        /// </summary>
        /// <param name="writer">A <see cref="XmlWriter"/> to serialize the <see cref="SamlAssertion"/>.</param>
        /// <param name="assertion">The <see cref="SamlAssertion"/> to serialize.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="assertion"/> is null.</exception>
        /// <exception cref="SamlSecurityTokenWriteException">if <see cref="SamlAssertion.AssertionId"/> is null or empty.</exception>
        /// <exception cref="SamlSecurityTokenWriteException">if <see cref="SamlAssertion.AssertionId"/> is not well formed. See <see cref="SamlSerializer.IsAssertionIdValid(string)"/>.</exception>
        /// <exception cref="SamlSecurityTokenWriteException">if <see cref="SamlAssertion.Issuer"/> is null or empty.</exception>
        /// <exception cref="SamlSecurityTokenWriteException">if <see cref="SamlAssertion.Statements"/>.Count == 0.</exception>
        public virtual void WriteAssertion(XmlWriter writer, SamlAssertion assertion)
        {
            if (writer == null)
                throw LogArgumentNullException(nameof(writer));

            if (assertion == null)
                throw LogArgumentNullException(nameof(assertion));

            if (string.IsNullOrEmpty(assertion.AssertionId))
                throw LogWriteException(LogMessages.IDX11501);

            if (!IsAssertionIdValid(assertion.AssertionId))
                throw LogWriteException(LogMessages.IDX11503, assertion.AssertionId);

            if (string.IsNullOrEmpty(assertion.Issuer))
                throw LogWriteException(LogMessages.IDX11504);

            if (assertion.Statements.Count == 0)
                throw LogWriteException(LogMessages.IDX11505);

            // Wrap the writer if necessary for a signature
            // We do not dispose this writer, since as a delegating writer it would
            // dispose the inner writer, which we don't properly own.
            EnvelopedSignatureWriter signatureWriter = null;
            if (assertion.SigningCredentials != null)
                writer = signatureWriter = new EnvelopedSignatureWriter(writer, assertion.SigningCredentials, assertion.AssertionId, assertion.InclusiveNamespacesPrefixList) { DSigSerializer = DSigSerializer };

            try
            {
                // <Assertion>
                writer.WriteStartElement(Prefix, SamlConstants.Elements.Assertion, SamlConstants.Namespace);

                // @MajorVersion
                writer.WriteAttributeString(SamlConstants.Attributes.MajorVersion, SamlConstants.MajorVersionValue);

                // @MinorVersion
                writer.WriteAttributeString(SamlConstants.Attributes.MinorVersion, SamlConstants.MinorVersionValue);

                // @AssertionID
                writer.WriteAttributeString(SamlConstants.Attributes.AssertionID, assertion.AssertionId);

                // @Issuer
                writer.WriteAttributeString(SamlConstants.Attributes.Issuer, assertion.Issuer);

                // @IssuerInstance
                writer.WriteAttributeString(SamlConstants.Attributes.IssueInstant, assertion.IssueInstant.ToString(SamlConstants.GeneratedDateTimeFormat, CultureInfo.InvariantCulture));

                // Write out conditions
                if (assertion.Conditions != null)
                    WriteConditions(writer, assertion.Conditions);

                // Write out advice if there is one
                if (assertion.Advice != null)
                    WriteAdvice(writer, assertion.Advice);

                foreach (var statement in assertion.Statements)
                    WriteStatement(writer, statement);

                // </Assertion>
                writer.WriteEndElement();
            }
            catch (Exception ex)
            {
                if (ex is SamlSecurityTokenWriteException)
                    throw;

                throw LogWriteException(LogMessages.IDX11517, ex, SamlConstants.Elements.Assertion, ex);
            }
        }

        /// <summary>
        /// Writes the &lt;saml:Attribute> element.
        /// </summary>
        /// <param name="writer">A <see cref="XmlWriter"/> to serialize the <see cref="SamlAttribute"/>.</param>
        /// <param name="attribute">The <see cref="SamlAttribute"/> to serialize.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="attribute"/> is null.</exception>
        /// <exception cref="SamlSecurityTokenWriteException">if any attribute values are null or empty.</exception>
        public virtual void WriteAttribute(XmlWriter writer, SamlAttribute attribute)
        {
            if (writer == null)
                throw LogArgumentNullException(nameof(writer));

            if (attribute == null)
                throw LogArgumentNullException(nameof(attribute));

            // <Attribute>
            writer.WriteStartElement(Prefix, SamlConstants.Elements.Attribute, SamlConstants.Namespace);

            // @AttributeName
            writer.WriteAttributeString(SamlConstants.Attributes.AttributeName, attribute.Name);
            writer.WriteAttributeString(SamlConstants.Attributes.AttributeNamespace, attribute.Namespace);

            // @OriginalIssuer - optional
            if (attribute.OriginalIssuer != null)
                writer.WriteAttributeString(SamlConstants.Attributes.OriginalIssuer, SamlConstants.ClaimType2009Namespace, attribute.OriginalIssuer);

            foreach (var value in attribute.Values)
            {
                // TODO - review SAML2 for handling of null values.
                if (string.IsNullOrEmpty(value))
                    throw LogWriteException(LogMessages.IDX11506);

                writer.WriteElementString(Prefix, SamlConstants.Elements.AttributeValue, SamlConstants.Namespace, value);
            }

            // </Attribute>
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the &lt;saml:AttributeStatement> element.
        /// </summary>
        /// <param name="writer">A <see cref="XmlWriter"/> to serialize the <see cref="SamlAttributeStatement"/>.</param>
        /// <param name="statement">The <see cref="SamlAttributeStatement"/> to serialize.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="statement"/> is null.</exception>
        /// <exception cref="SamlSecurityTokenWriteException">if <see cref="SamlAttributeStatement.Attributes"/>.Count == 0.</exception>
        protected virtual void WriteAttributeStatement(XmlWriter writer, SamlAttributeStatement statement)
        {
            if (writer == null)
                throw LogArgumentNullException(nameof(writer));

            if (statement == null)
                throw LogArgumentNullException(nameof(statement));

            // <AttributeStatement>
            writer.WriteStartElement(Prefix, SamlConstants.Elements.AttributeStatement, SamlConstants.Namespace);

            // <Subject>
            WriteSubject(writer, statement.Subject);

            // <Attribute> 1-OO
            foreach (var attribute in statement.Attributes)
                WriteAttribute(writer, attribute);

            // </AttributeStatement>
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the &lt;saml:AudienceRestriction> element.
        /// </summary>
        /// <param name="writer">A <see cref="XmlWriter"/> to serialize the <see cref="SamlAudienceRestrictionCondition"/>.</param>
        /// <param name="audienceRestriction">The <see cref="SamlAudienceRestrictionCondition"/> to serialize.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="audienceRestriction"/> is null.</exception>
        /// <exception cref="SamlSecurityTokenWriteException">if <see cref="SamlAudienceRestrictionCondition.Audiences"/> is empty.</exception>
        protected virtual void WriteAudienceRestrictionCondition(XmlWriter writer, SamlAudienceRestrictionCondition audienceRestriction)
        {
            if (writer == null)
                throw LogArgumentNullException(nameof(writer));

            if (audienceRestriction == null)
                throw LogArgumentNullException(nameof(audienceRestriction));
            
            // <AudienceRestrictionCondition>
            writer.WriteStartElement(Prefix, SamlConstants.Elements.AudienceRestrictionCondition, SamlConstants.Namespace);

            // <Audience> - 1-OO
            foreach (var audience in audienceRestriction.Audiences)
            {
                if (audience != null)
                    writer.WriteElementString(Prefix, SamlConstants.Elements.Audience, SamlConstants.Namespace, audience.OriginalString);
            }

            // </AudienceRestrictionCondition>
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the &lt;AuthenticationStatement> element.
        /// </summary>
        /// <param name="writer">A <see cref="XmlWriter"/> to serialize the <see cref="SamlAuthenticationStatement"/>.</param>
        /// <param name="statement">The <see cref="SamlAuthenticationStatement"/> to serialize.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="statement"/> is null.</exception>
        protected virtual void WriteAuthenticationStatement(XmlWriter writer, SamlAuthenticationStatement statement)
        {
            if (writer == null)
                throw LogArgumentNullException(nameof(writer));

            if (statement == null)
                throw LogArgumentNullException(nameof(statement));

            if (string.IsNullOrEmpty(statement.AuthenticationMethod))
                throw LogWriteException(LogMessages.IDX11800, SamlConstants.Elements.AuthenticationStatement, statement.GetType(), SamlConstants.Attributes.AuthenticationMethod);

            // <AuthnStatement>
            writer.WriteStartElement(Prefix, SamlConstants.Elements.AuthenticationStatement, SamlConstants.Namespace);

            // @AuthenticationMethod - required
            writer.WriteAttributeString(SamlConstants.Attributes.AuthenticationMethod, statement.AuthenticationMethod);

            // @AuthnInstant - required
            writer.WriteAttributeString(SamlConstants.Attributes.AuthenticationInstant, statement.AuthenticationInstant.ToString(SamlConstants.GeneratedDateTimeFormat, CultureInfo.InvariantCulture));

            // <Subject> - required
            WriteSubject(writer, statement.Subject);

            if ((!string.IsNullOrEmpty(statement.IPAddress)) || (!string.IsNullOrEmpty(statement.DnsAddress)))
            {
                // <SubjectLocality>
                writer.WriteStartElement(Prefix, SamlConstants.Elements.SubjectLocality, SamlConstants.Namespace);

                // @IPAddress - optional
                if (!string.IsNullOrEmpty(statement.IPAddress))
                    writer.WriteAttributeString(SamlConstants.Attributes.IPAddress, statement.IPAddress);

                // @DNSAddress - optional
                if (!string.IsNullOrEmpty(statement.DnsAddress))
                    writer.WriteAttributeString(SamlConstants.Attributes.DNSAddress, statement.DnsAddress);

                // </SubjectLocality>
                writer.WriteEndElement();
            }

            foreach (var binding in statement.AuthorityBindings)
                WriteAuthorityBinding(writer, binding);

            // <AuthnStatement>
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the &lt;AuthorityBinding> element.
        /// </summary>
        /// <param name="writer">A <see cref="XmlWriter"/> to serialize the <see cref="SamlAuthenticationStatement"/>.</param>
        /// <param name="authorityBinding">The <see cref="SamlAuthorityBinding"/> to serialize.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="authorityBinding"/> is null.</exception>
        /// <exception cref="SamlSecurityTokenWriteException">if <see cref="SamlAuthorityBinding.AuthorityKind"/> is null.</exception>
        /// <exception cref="SamlSecurityTokenWriteException">if <see cref="SamlAuthorityBinding.Binding"/> is null or empty.</exception>
        /// <exception cref="SamlSecurityTokenWriteException">if <see cref="SamlAuthorityBinding.Location"/> is null or empty.</exception>
        protected virtual void WriteAuthorityBinding(XmlWriter writer, SamlAuthorityBinding authorityBinding)
        {
            if (writer == null)
                throw LogArgumentNullException(nameof(writer));

            if (authorityBinding == null)
                throw LogArgumentNullException(nameof(authorityBinding));

            if (authorityBinding.AuthorityKind == null)
                throw LogWriteException(LogMessages.IDX11800, SamlConstants.Elements.AuthorityBinding, authorityBinding.GetType(), "AuthorityKind");

            if (string.IsNullOrEmpty(authorityBinding.Binding))
                throw LogWriteException(LogMessages.IDX11800, SamlConstants.Elements.AuthorityBinding, authorityBinding.GetType(), "Binding");

            if (string.IsNullOrEmpty(authorityBinding.Location))
                throw LogWriteException(LogMessages.IDX11800, SamlConstants.Elements.AuthorityBinding, authorityBinding.GetType(), "Location");

            // <AuthorityBinding>
            writer.WriteStartElement(Prefix, SamlConstants.Elements.AuthorityBinding, SamlConstants.Namespace);

            // @AuthorityKind
            string prefix = null;
            if (!string.IsNullOrEmpty(authorityBinding.AuthorityKind.Namespace))
            {
                writer.WriteAttributeString(string.Empty, SamlConstants.NamespaceAttributePrefix, null, authorityBinding.AuthorityKind.Namespace);
                prefix = writer.LookupPrefix(authorityBinding.AuthorityKind.Namespace);
            }

            writer.WriteStartAttribute(Prefix, SamlConstants.Attributes.AuthorityKind, null);
            if (string.IsNullOrEmpty(prefix))
                writer.WriteString(authorityBinding.AuthorityKind.Name);
            else
                writer.WriteString(prefix + ":" + authorityBinding.AuthorityKind.Name);
            writer.WriteEndAttribute();

            // @Location
            writer.WriteAttributeString(SamlConstants.Attributes.Location, authorityBinding.Location);

            // Binding
            writer.WriteAttributeString(SamlConstants.Attributes.Binding, authorityBinding.Binding);

            // </AuthorityBinding>
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the &lt;saml:AuthzDecisionStatement> element.
        /// </summary>
        /// <param name="writer">A <see cref="XmlWriter"/> to serialize the <see cref="SamlAuthorizationDecisionStatement"/>.</param>
        /// <param name="statement">The <see cref="SamlAuthorizationDecisionStatement"/> to serialize.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="statement"/> is null.</exception>
        /// <exception cref="SamlSecurityTokenWriteException">if <see cref="SamlAuthorizationDecisionStatement.Actions"/> is empty.</exception>
        /// <exception cref="SamlSecurityTokenWriteException">if <see cref="SamlAuthorizationDecisionStatement.Decision"/> is null or empty.</exception>
        /// <exception cref="SamlSecurityTokenWriteException">if <see cref="SamlAuthorizationDecisionStatement.Resource"/> is null.</exception>
        protected virtual void WriteAuthorizationDecisionStatement(XmlWriter writer, SamlAuthorizationDecisionStatement statement)
        {
            if (writer == null)
                throw LogArgumentNullException(nameof(writer));

            if (statement == null)
                throw LogArgumentNullException(nameof(statement));

            if (statement.Actions.Count == 0)
                throw LogWriteException(LogMessages.IDX11901, statement.GetType(), "Actions");

            if (string.IsNullOrEmpty(statement.Decision))
                throw LogWriteException(LogMessages.IDX11900, SamlConstants.Attributes.Decision, statement.GetType(), nameof(statement.Decision));

            if (string.IsNullOrEmpty(statement.Resource))
                throw LogWriteException(LogMessages.IDX11900, SamlConstants.Attributes.Resource, statement.GetType(), nameof(statement.Resource));

            // <AuthorizationDecisionStatement>
            writer.WriteStartElement(Prefix, SamlConstants.Elements.AuthorizationDecisionStatement, SamlConstants.Namespace);

            // @Decision - required
            writer.WriteAttributeString(SamlConstants.Attributes.Decision, statement.Decision);

            // @Resource - required
            writer.WriteAttributeString(SamlConstants.Attributes.Resource, statement.Resource);

            // <Subject>
            WriteSubject(writer, statement.Subject);

            foreach (var action in statement.Actions)
                WriteAction(writer, action);

            //<Evidence> - optional
            if (statement.Evidence != null)
                WriteEvidence(writer, statement.Evidence);

            // </AuthorizationDecisionStatement>
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the &lt;saml:Condition> element.
        /// </summary>
        /// <param name="writer">A <see cref="XmlWriter"/> to serialize the <see cref="SamlCondition"/>.</param>
        /// <param name="condition">The <see cref="SamlCondition"/> to serialize.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="condition"/> is null.</exception>
        /// <remarks>Writes a <see cref="SamlAudienceRestrictionCondition"/> or a <see cref="SamlDoNotCacheCondition"/> all others are skipped.</remarks>
        protected virtual void WriteCondition(XmlWriter writer, SamlCondition condition)
        {
            if (condition is SamlAudienceRestrictionCondition audienceRestrictionCondition)
                WriteAudienceRestrictionCondition(writer, audienceRestrictionCondition);

            if (condition is SamlDoNotCacheCondition donotCacheCondition)
                WriteDoNotCacheCondition(writer, donotCacheCondition);
        }

        /// <summary>
        /// Writes the &lt;saml:Conditions> element.
        /// </summary>
        /// <param name="writer">A <see cref="XmlWriter"/> to serialize the <see cref="SamlConditions"/>.</param>
        /// <param name="conditions">The <see cref="SamlConditions"/> to serialize.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="conditions"/> is null.</exception>
        protected virtual void WriteConditions(XmlWriter writer, SamlConditions conditions)
        {
            if (writer == null)
                throw LogArgumentNullException(nameof(writer));

            if (conditions == null)
                throw LogArgumentNullException(nameof(conditions));

            // <Conditions>
            writer.WriteStartElement(Prefix, SamlConstants.Elements.Conditions, SamlConstants.Namespace);

            // @NotBefore
            if (conditions.NotBefore != DateTimeUtil.GetMinValue(DateTimeKind.Utc))
                writer.WriteAttributeString(SamlConstants.Attributes.NotBefore, conditions.NotBefore.ToString(SamlConstants.GeneratedDateTimeFormat, DateTimeFormatInfo.InvariantInfo));

            // @NotOnOrAfter
            if (conditions.NotOnOrAfter != DateTimeUtil.GetMaxValue(DateTimeKind.Utc))
                writer.WriteAttributeString(SamlConstants.Attributes.NotOnOrAfter, conditions.NotOnOrAfter.ToString(SamlConstants.GeneratedDateTimeFormat, DateTimeFormatInfo.InvariantInfo));

            // <Condition>
            foreach (var condition in conditions.Conditions)
                WriteCondition(writer, condition);

            // <Conditions>
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the &lt;saml:DoNotCacheCondition> element.
        /// </summary>
        /// <param name="writer">A <see cref="XmlWriter"/> to serialize the <see cref="SamlDoNotCacheCondition"/>.</param>
        /// <param name="condition">The <see cref="SamlDoNotCacheCondition"/> to serialize.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="condition"/> is null.</exception>
        protected virtual void WriteDoNotCacheCondition(XmlWriter writer, SamlDoNotCacheCondition condition)
        {
            if (writer == null)
                throw LogArgumentNullException(nameof(writer));

            if (condition == null)
                throw LogArgumentNullException(nameof(condition));

            // <DoNotCacheCondition>
            writer.WriteStartElement(Prefix, SamlConstants.Elements.DoNotCacheCondition, SamlConstants.Namespace);

            // </DoNotCacheCondition>
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the &lt;saml:Evidence> element.
        /// </summary>
        /// <param name="writer">A <see cref="XmlWriter"/> to serialize the <see cref="SamlEvidence"/>.</param>
        /// <param name="evidence">The <see cref="SamlEvidence"/> to serialize.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="evidence"/> is null.</exception>
        /// <exception cref="SamlSecurityTokenWriteException">if <see cref="SamlEvidence"/> does not contain any assertions or assertions references.</exception>
        protected virtual void WriteEvidence(XmlWriter writer, SamlEvidence evidence)
        {
            if (writer == null)
                throw LogArgumentNullException(nameof(writer));

            if (evidence == null)
                throw LogArgumentNullException(nameof(evidence));

            if (evidence.AssertionIDReferences.Count == 0 && evidence.Assertions.Count == 0)
                throw LogWriteException(LogMessages.IDX11902);

            // <Evidence>
            writer.WriteStartElement(Prefix, SamlConstants.Elements.Evidence, SamlConstants.Namespace);

            // <AssertionIDReference> 0-OO
            foreach (var assertionId in evidence.AssertionIDReferences)
                writer.WriteElementString(Prefix, SamlConstants.Elements.AssertionIDReference, SamlConstants.Namespace, assertionId);

            // <Assertion> 0-OO
            foreach (var assertion in evidence.Assertions)
                WriteAssertion(writer, assertion);

            // </Evidence>
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes one of the suppported Statements.
        /// </summary>
        /// <param name="writer">A <see cref="XmlWriter"/> to serialize the <see cref="SamlStatement"/>.</param>
        /// <param name="statement">The <see cref="SamlStatement"/> to serialize.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="statement"/> is null.</exception>
        protected virtual void WriteStatement(XmlWriter writer, SamlStatement statement)
        {
            if (writer == null)
                throw LogArgumentNullException(nameof(writer));

            if (statement == null)
                throw LogArgumentNullException(nameof(statement));

            if (statement is SamlAttributeStatement attributeStatement)
                WriteAttributeStatement(writer, attributeStatement);
            else if (statement is SamlAuthenticationStatement authenticationStatement)
                WriteAuthenticationStatement(writer, authenticationStatement);
            else if (statement is SamlAuthorizationDecisionStatement authorizationStatement)
                WriteAuthorizationDecisionStatement(writer, authorizationStatement);
            else
                throw LogWriteException(LogMessages.IDX11516, statement.GetType());
        }

        /// <summary>
        /// Writes the &lt;saml:Subject> element.
        /// </summary>
        /// <param name="writer">A <see cref="XmlWriter"/> to serialize the <see cref="SamlSubject"/>.</param>
        /// <param name="subject">The <see cref="SamlSubject"/> to serialize.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="subject"/> is null.</exception>
        /// <exception cref="SamlSecurityTokenWriteException">if <see cref="SamlEvidence"/> does not contain any assertions or assertions references.</exception>
        protected virtual void WriteSubject(XmlWriter writer, SamlSubject subject)
        {
            if (writer == null)
                throw LogArgumentNullException(nameof(writer));

            if (subject == null)
                throw LogArgumentNullException(nameof(subject));

            if (string.IsNullOrEmpty(subject.Name) && subject.ConfirmationMethods.Count == 0)
                throw LogWriteException(LogMessages.IDX11518);

            // <Subject>
            writer.WriteStartElement(Prefix, SamlConstants.Elements.Subject, SamlConstants.Namespace);

            if (!string.IsNullOrEmpty(subject.Name))
            {
                // <NameIdentifier>
                writer.WriteStartElement(Prefix, SamlConstants.Elements.NameIdentifier, SamlConstants.Namespace);

                // @Format
                if (!string.IsNullOrEmpty(subject.NameFormat))
                    writer.WriteAttributeString(SamlConstants.Attributes.Format, subject.NameFormat);

                // @NameQualifier
                if (!string.IsNullOrEmpty(subject.NameQualifier))
                    writer.WriteAttributeString(SamlConstants.Attributes.NameQualifier, subject.NameQualifier);

                // name
                writer.WriteString(subject.Name);

                // </NameIdentifier>
                writer.WriteEndElement();
            }

            if (subject.ConfirmationMethods.Count > 0)
            {
                // <SubjectConfirmation>
                writer.WriteStartElement(Prefix, SamlConstants.Elements.SubjectConfirmation, SamlConstants.Namespace);

                // <ConfirmationMethod> 1-OO
                foreach (string method in subject.ConfirmationMethods)
                    writer.WriteElementString(Prefix, SamlConstants.Elements.ConfirmationMethod, SamlConstants.Namespace, method);

                if (!string.IsNullOrEmpty(subject.ConfirmationData))
                    writer.WriteElementString(Prefix, SamlConstants.Elements.SubjectConfirmationData, SamlConstants.Namespace, subject.ConfirmationData);

                // </SubjectConfirmation>
                writer.WriteEndElement();
            }

            // <Subject>
            writer.WriteEndElement();
        }
    }
}
