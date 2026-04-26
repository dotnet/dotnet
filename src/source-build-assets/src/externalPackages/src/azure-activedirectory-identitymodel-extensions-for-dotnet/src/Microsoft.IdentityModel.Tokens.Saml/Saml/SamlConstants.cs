// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Tokens.Saml
{
    /// <summary>
    /// Contains constants related to SAML.
    /// </summary>
    public static class SamlConstants
    {
#pragma warning disable 1591
        public const string BearerConfirmationMethod = "urn:oasis:names:tc:SAML:1.0:cm:bearer";
        public const string DefaultActionNamespace = "urn:oasis:names:tc:SAML:1.0:action:rwedc-negation";
        public const string MajorVersionValue = "1";
        public const string MinorVersionValue = "1";
        public const string Namespace = "urn:oasis:names:tc:SAML:1.0:assertion";
        public const string NamespaceAttributePrefix = "NamespaceAttributePrefix";
        public const string OasisWssSamlTokenProfile11 = "http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV1.1";
        public const string Prefix = "saml";
        public const string Saml11Namespace = "urn:oasis:names:tc:SAML:1.0:assertion";
        public const string Statement = "Statement";
        public const string SubjectStatement = "SubjectStatement";
        public const string UserName = "UserName";
        public const string UserNameNamespace = "urn:oasis:names:tc:SAML:1.1:nameid-format:WindowsDomainQualifiedName";
        public const string UnspecifiedAuthenticationMethod = "urn:oasis:names:tc:SAML:1.0:am:unspecified";

        internal const string ClaimType2009Namespace = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims";
        internal const string MsIdentityNamespaceUri = "http://schemas.microsoft.com/ws/2008/06/identity";

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

        public const string AssertionIdPrefix = "SamlSecurityToken-";
        public const string GeneratedDateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";

        public static class AccessDecision
        {
            public static string Deny { get { return "Deny"; } }

            public static string Indeterminate { get { return "Indeterminate"; } }

            public static string Permit { get { return "Permit"; } }
        }

        internal static class Attributes
        {
            public const string ActionNamespaceAttribute = "Namespace";
            public const string AssertionID = "AssertionID";
            public const string AttributeName = "AttributeName";
            public const string AttributeNamespace = "AttributeNamespace";
            public const string AuthenticationInstant = "AuthenticationInstant";
            public const string AuthenticationMethod = "AuthenticationMethod";
            public const string AuthorityBinding = "AuthorityBinding";
            public const string AuthorityKind = "AuthorityKind";
            public const string Binding = "Binding";
            public const string Decision = "Decision";
            public const string Issuer = "Issuer";
            public const string IssueInstant = "IssueInstant";
            public const string Location = "Location";
            public const string MajorVersion = "MajorVersion";
            public const string MinorVersion = "MinorVersion";
            public const string OriginalIssuer = "OriginalIssuer";
            public const string NamespaceAttributePrefix = "xmlns";
            public const string Format = "Format";
            public const string NameQualifier = "NameQualifier";
            public const string Namespace = "Namespace";
            public const string NotBefore = "NotBefore";
            public const string NotOnOrAfter = "NotOnOrAfter";
            public const string Resource = "Resource";
            public const string DNSAddress = "DNSAddress";
            public const string IPAddress = "IPAddress";
        }

        /// <summary>
        /// Known values for <see cref="Microsoft.IdentityModel.Tokens.Saml.SamlAuthenticationStatement"/>
        /// </summary>
        public static class AuthenticationMethods
        {
            public const string HardwareTokenString = "URI:urn:oasis:names:tc:SAML:1.0:am:HardwareToken";
            public const string KerberosString = "urn:ietf:rfc:1510";
            public const string PasswordString = "urn:oasis:names:tc:SAML:1.0:am:password";
            public const string PgpString = "urn:oasis:names:tc:SAML:1.0:am:PGP";
            public const string SecureRemotePasswordString = "urn:ietf:rfc:2945";
            public const string SignatureString = "urn:ietf:rfc:3075";
            public const string SpkiString = "urn:oasis:names:tc:SAML:1.0:am:SPKI";
            public const string TlsClientString = "urn:ietf:rfc:2246";
            public const string UnspecifiedString = "urn:oasis:names:tc:SAML:1.0:am:unspecified";
            public const string WindowsString = "urn:federation:authentication:windows";
            public const string X509String = "urn:oasis:names:tc:SAML:1.0:am:X509-PKI";
            public const string XkmsString = "urn:oasis:names:tc:SAML:1.0:am:XKMS";
        }

        internal static class Elements
        {
            public const string Action = "Action";
            public const string Advice = "Advice";
            public const string Assertion = "Assertion";
            public const string AssertionIDReference = "AssertionIDReference";
            public const string Attribute = "Attribute";
            public const string AttributeStatement = "AttributeStatement";
            public const string AttributeValue = "AttributeValue";
            public const string Audience = "Audience";
            public const string AudienceRestrictionCondition = "AudienceRestrictionCondition";
            public const string AuthenticationStatement = "AuthenticationStatement";
            public const string AuthorityBinding = "AuthorityBinding";
            public const string AuthorizationDecisionStatement = "AuthorizationDecisionStatement";
            public const string Conditions = "Conditions";
            public const string DoNotCacheCondition = "DoNotCacheCondition";
            public const string Evidence = "Evidence";
            public const string NameIdentifier = "NameIdentifier";
            public const string SubjectConfirmation = "SubjectConfirmation";
            public const string Subject = "Subject";
            public const string SubjectConfirmationData = "SubjectConfirmationData";
            public const string ConfirmationMethod = "ConfirmationMethod";
            public const string SubjectLocality = "SubjectLocality";
            public const string DNSAddress = "DNSAddress";
            public const string IPAddress = "IPAddress";

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
            public const string AuthorityBindingType = "AuthorityBindingType";
            public const string AuthzDecisionStatementType = "AuthzDecisionStatementType";
            public const string BaseIDAbstractType = "BaseIDAbstractType";
            public const string ConditionAbstractType = "ConditionAbstractType";
            public const string ConditionsType = "ConditionsType";
            public const string DoNotCacheConditionType = "DoNotCacheConditionType";
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
