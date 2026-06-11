// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
    /// <summary>
    /// Contains constants related to SAML2.
    /// </summary>
    public static class Saml2Constants
    {
#pragma warning disable 1591
        public const string Namespace = "urn:oasis:names:tc:SAML:2.0:assertion";
        public const string OasisWssSaml2TokenProfile11 = "http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV2.0";
        public const string Prefix = "saml";
        public const string Saml2TokenProfile11 = "urn:oasis:names:tc:SAML:2.0:assertion";
        public const string Version = "2.0";
        public static string[] AcceptedDateTimeFormats = new string[] {
                "yyyy-MM-ddTHH:mm:ss.fffffffZ",
                "yyyy-MM-ddTHH:mm:ss.ffffffZ",
                "yyyy-MM-ddTHH:mm:ss.fffffZ",
                "yyyy-MM-ddTHH:mm:ss.ffffZ",
                "yyyy-MM-ddTHH:mm:ss.fffZ",
                "yyyy-MM-ddTHH:mm:ss.ffZ",
                "yyyy-MM-ddTHH:mm:ss.fZ",
                "yyyy-MM-ddTHH:mm:ssZ",
                "yyyy-MM-ddTHH:mm:ss.fffffffzzz",
                "yyyy-MM-ddTHH:mm:ss.ffffffzzz",
                "yyyy-MM-ddTHH:mm:ss.fffffzzz",
                "yyyy-MM-ddTHH:mm:ss.ffffzzz",
                "yyyy-MM-ddTHH:mm:ss.fffzzz",
                "yyyy-MM-ddTHH:mm:ss.ffzzz",
                "yyyy-MM-ddTHH:mm:ss.fzzz",
                "yyyy-MM-ddTHH:mm:sszzz" };

        internal const string ClaimType2009Namespace = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims";
        internal const string ClaimValueTypeSerializationPrefix = "tn";
        internal const string ClaimValueTypeSerializationPrefixWithColon = "tn:";
        internal const string GeneratedDateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";
        internal const string MsIdentityNamespaceUri = "http://schemas.microsoft.com/ws/2008/06/identity";

        public static class AccessDecision
        {
            public static string Deny { get { return "Deny"; } }

            public static string Indeterminate { get { return "Indeterminate"; } }

            public static string Permit { get { return "Permit"; } }
        }

        public static class Attributes
        {
            public const string Address = "Address";
            public const string AuthnInstant = "AuthnInstant";
            public const string Count = "Count";
            public const string Decision = "Decision";
            public const string DNSName = "DNSName";
            public const string Format = "Format";
            public const string FriendlyName = "FriendlyName";
            public const string ID = "ID";
            public const string InResponseTo = "InResponseTo";
            public const string IssueInstant = "IssueInstant";
            public const string Method = "Method";
            public const string Name = "Name";
            public const string NameFormat = "NameFormat";
            public const string NameQualifier = "NameQualifier";
            public const string Namespace = "Namespace";
            public const string NotBefore = "NotBefore";
            public const string NotOnOrAfter = "NotOnOrAfter";
            public const string OriginalIssuer = "OriginalIssuer";
            public const string Recipient = "Recipient";
            public const string Resource = "Resource";
            public const string SessionIndex = "SessionIndex";
            public const string SessionNotOnOrAfter = "SessionNotOnOrAfter";
            public const string SPNameQualifier = "SPNameQualifier";
            public const string SPProvidedID = "SPProvidedID";
            public const string Type = "type";
            public const string Version = "Version";
        }

        public static class ConfirmationMethods
        {
            public const string BearerString = "urn:oasis:names:tc:SAML:2.0:cm:bearer";
            public const string HolderOfKeyString = "urn:oasis:names:tc:SAML:2.0:cm:holder-of-key";
            public const string SenderVouchesString = "urn:oasis:names:tc:SAML:2.0:cm:sender-vouches";

            /// <summary>
            /// The subject of the assertion is the bearer of the assertion. [Saml2Prof, 3.3]
            /// </summary>
            public static readonly Uri Bearer = new Uri(BearerString);

            /// <summary>
            /// The holder of a specified key is considered to be the subject of the assertion
            /// by the asserting party. [Saml2Prof, 3.1]
            /// </summary>
            public static readonly Uri HolderOfKey = new Uri(HolderOfKeyString);

            /// <summary>
            /// Indicates that no other information is available about the context of use of the 
            /// assertion. [Saml2Prof, 3.2]
            /// </summary>
            public static readonly Uri SenderVouches = new Uri(SenderVouchesString);
        }

        public static class Elements
        {
            public const string Action = "Action";
            public const string Advice = "Advice";
            public const string Assertion = "Assertion";
            public const string AssertionIDRef = "AssertionIDRef";
            public const string AssertionURIRef = "AssertionURIRef";
            public const string Attribute = "Attribute";
            public const string AttributeStatement = "AttributeStatement";
            public const string AttributeValue = "AttributeValue";
            public const string Audience = "Audience";
            public const string AudienceRestriction = "AudienceRestriction";
            public const string AuthenticatingAuthority = "AuthenticatingAuthority";
            public const string AuthnContext = "AuthnContext";
            public const string AuthnContextClassRef = "AuthnContextClassRef";
            public const string AuthnContextDecl = "AuthnContextDecl";
            public const string AuthnContextDeclRef = "AuthnContextDeclRef";
            public const string AuthnStatement = "AuthnStatement";
            public const string AuthzDecisionStatement = "AuthzDecisionStatement";
            public const string BaseID = "BaseID";
            public const string Condition = "Condition";
            public const string Conditions = "Conditions";
            public const string EncryptedAssertion = "EncryptedAssertion";
            public const string EncryptedAttribute = "EncryptedAttribute";
            public const string EncryptedID = "EncryptedID";
            public const string Evidence = "Evidence";
            public const string Issuer = "Issuer";
            public const string NameID = "NameID";
            public const string OneTimeUse = "OneTimeUse";
            public const string ProxyRestricton = "ProxyRestriction";
            public const string Statement = "Statement";
            public const string Subject = "Subject";
            public const string SubjectConfirmation = "SubjectConfirmation";
            public const string SubjectConfirmationData = "SubjectConfirmationData";
            public const string SubjectLocality = "SubjectLocality";
        }

        /// <summary>
        /// These identifiers MAY be used in the Format attribute of the NameID,
        /// NameIDPolicy, or Issuer elements to refer to common formats for the
        /// content of the elements and the associated processing rules, if any.
        /// [Saml2Core, 8.3]
        /// </summary>
        public static class NameIdentifierFormats
        {
            /// <summary>
            /// The interpretation of the content of the element is left to 
            /// individual implementations. [Saml2Core, 8.3.1]
            /// </summary>
            public static readonly Uri Unspecified = new Uri(UnspecifiedString);

            /// <summary>
            /// Indicates that the content of the element is in the form of an 
            /// email address, specifically "addr-spec" as defined in 
            /// [RFC 2822, 3.4.1]. [Saml2Core, 8.3.2]
            /// </summary>
            public static readonly Uri EmailAddress = new Uri(EmailAddressString);

            /// <summary>
            /// Indicates that the content of the element is in the form specified
            /// for the contents of the X509SubjectName element from [XMLSig].
            /// [Saml2Core, 8.3.3]
            /// </summary>
            public static readonly Uri X509SubjectName = new Uri(X509SubjectNameString);

            /// <summary>
            /// Indicates that the content of the element is a Windows domain 
            /// qualified name. [Saml2Core, 8.3.4]
            /// </summary>
            public static readonly Uri WindowsDomainQualifiedName = new Uri(WindowsDomainQualifiedNameString);

            /// <summary>
            /// Indicates that the content of the element is in the form of a 
            /// Kerberos principal name using the format name[/instance]@REALM.
            /// [Saml2Core, 8.3.5]
            /// </summary>
            public static readonly Uri Kerberos = new Uri(KerberosString);

            /// <summary>
            /// Indicates that the content of the element is a URI identifying an
            /// entity that provides SAML-based services (such as a SAML authority,
            /// requester, or responder) or is a participant in SAML profiles (such
            /// as a service provider supporting the browser SSO profile). 
            /// [Saml2Core, 8.3.6]
            /// </summary>
            public static readonly Uri Entity = new Uri(EntityString);

            /// <summary>
            /// Indicates that the content of the element is a persistent opaque 
            /// identifier for a principal that is specific to an identity provider
            /// and a service provider or affiliation of service providers.
            /// [Saml2Core, 8.3.7] (See also for many restrictions on the data.)
            /// </summary>
            public static readonly Uri Persistent = new Uri(PersistentString);

            /// <summary>
            /// Indicates that the content of the element is an identifier with 
            /// transient semantics and SHOULD be treated as an opaque and 
            /// temporary value by the relying party. [Saml2Core, 8.3.8]
            /// </summary>
            public static readonly Uri Transient = new Uri(TransientString);

            /// <summary>
            /// When included in the Format attribute of the NameIDPolicy attribute,
            /// requests that the resulting identifier be encrypted. [Saml2Core, 3.4.1.1]
            /// </summary>
            public static readonly Uri Encrypted = new Uri(EncryptedString);

            public const string UnspecifiedString = "urn:oasis:names:tc:SAML:1.1:nameid-format:unspecified";
            public const string EmailAddressString = "urn:oasis:names:tc:SAML:1.1:nameid-format:emailAddress";
            public const string X509SubjectNameString = "urn:oasis:names:tc:SAML:1.1:nameid-format:X509SubjectName";
            public const string WindowsDomainQualifiedNameString = "urn:oasis:names:tc:SAML:1.1:nameid-format:WindowsDomainQualifiedName";
            public const string KerberosString = "urn:oasis:names:tc:SAML:2.0:nameid-format:kerberos";
            public const string EntityString = "urn:oasis:names:tc:SAML:2.0:nameid-format:entity";
            public const string PersistentString = "urn:oasis:names:tc:SAML:2.0:nameid-format:persistent";
            public const string TransientString = "urn:oasis:names:tc:SAML:2.0:nameid-format:transient";
            public const string EncryptedString = "urn:oasis:names:tc:SAML:2.0:nameid-format:encrypted";
        }

        public static class Types
        {
            public const string ActionType = "ActionType";
            public const string AdviceType = "AdviceType";
            public const string AssertionType = "AssertionType";
            public const string AttributeStatementType = "AttributeStatementType";
            public const string AttributeType = "AttributeType";
            public const string AudienceRestrictionType = "AudienceRestrictionType";
            public const string AuthnContextType = "AuthnContextType";
            public const string AuthnStatementType = "AuthnStatementType";
            public const string AuthzDecisionStatementType = "AuthzDecisionStatementType";
            public const string BaseIDAbstractType = "BaseIDAbstractType";
            public const string ConditionAbstractType = "ConditionAbstractType";
            public const string ConditionsType = "ConditionsType";
            public const string EncryptedElementType = "EncryptedElementType";
            public const string EvidenceType = "EvidenceType";
            public const string KeyInfoConfirmationDataType = "KeyInfoConfirmationDataType";
            public const string NameIDType = "NameIDType";
            public const string OneTimeUseType = "OneTimeUseType";
            public const string ProxyRestrictionType = "ProxyRestrictionType";
            public const string SubjectType = "SubjectType";
            public const string SubjectConfirmationDataType = "SubjectConfirmationDataType";
            public const string SubjectConfirmationType = "SubjectConfirmationType";
            public const string SubjectLocalityType = "SubjectLocalityType";
            public const string StatementAbstractType = "StatementAbstractType";
        }
#pragma warning restore 1591
    }
}
