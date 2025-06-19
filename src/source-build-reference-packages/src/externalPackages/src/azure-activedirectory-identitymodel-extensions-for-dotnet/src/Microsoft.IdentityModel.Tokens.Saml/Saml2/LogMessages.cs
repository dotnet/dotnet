// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// Microsoft.IdentityModel.Tokens.Saml2
// Range: 13000 - 13999

namespace Microsoft.IdentityModel.Tokens.Saml2
{
    /// <summary>
    /// Log messages and codes for Saml2Processing
    /// </summary>
    internal static class LogMessages
    {
#pragma warning disable 1591
        // token validation
        internal const string IDX13400 = "IDX13400: The '{0}', can only process SecurityTokens of type: '{1}'. The SecurityToken received is of type: '{2}'.";
        internal const string IDX13001 = "IDX13001: A SAML2 assertion that specifies an AuthenticationContext DeclarationReference is not supported.To handle DeclarationReference, extend the Saml2SecurityTokenHandler and override ProcessAuthenticationStatement.";
        internal const string IDX13002 = "IDX13002: Unable to validate token. TokenValidationParameters.RequireAudience is true but no AudienceRestrictions were found in the inbound token.";

        // signature creation / validation
        internal const string IDX13509 = "IDX13509: Unable to validate token, Subject is null.";
        internal const string IDX13510 = "IDX13510: The Saml2SecurityToken cannot be validated because the Assertion specifies a OneTimeUse condition.Enforcement of the OneTimeUse condition is not supported by default.To customize the enforcement of OneTimeUse condition, extend Saml2SecurityTokenHandler and override ValidateOneTimeUseCondition.";
        internal const string IDX13511 = "IDX13511: The Saml2SecurityToken cannot be validated because the Assertion specifies a ProxyRestriction condition.Enforcement of the ProxyRestriction condition is not supported by default. To customize the enforcement of Saml2Conditions, extend Saml2SecurityTokenHandler and override ValidateConditions.";
        internal const string IDX13512 = "IDX13512: Unable to validate token. A Saml2SamlAttributeStatement can only have one Saml2Attribute of type 'Actor'. This special Saml2Attribute is used in delegation scenarios.";
        internal const string IDX13513 = "IDX13513: NotBefore '{0}', is after NotOnOrAfter '{1}'.";
        internal const string IDX13514 = "IDX13514: NotOnOrAfter '{0}', is before NotBefore '{1}'.";
        internal const string IDX13515 = "IDX13515: SamlId value threw on XmlConvert.VerifyNCName. value: '{0}'";
        internal const string IDX13516 = "IDX13516: A Saml2Statement of type: '{0}' was found when ProcessingStatements and creating the ClaimsIdentity. These claims have been skipped. If you need to process this Statement, you will need to derive a custom Saml2SecurityTokenHandler and override ProcessStatements.";

        // SamlSerializing reading
        internal const string IDX13102 = "IDX13102: Exception thrown while reading '{0}' for Saml2SecurityToken. Inner exception: '{1}'.";
        internal const string IDX13103 = "IDX13103: Unable to read '{0}' for Saml2SecurityToken. The element type '{1}' appears to have a custom format that cannot be parsed. If this is expected, you will need to override '{2}.{3}'.";
        internal const string IDX13104 = "IDX13104: Unable to read Saml2SecurityToken. Expecting XmlReader to be at element: '{0}', found 'Empty Element'";
        internal const string IDX13105 = "IDX13105: Unable to read Saml2SecurityToken. Expecting XmlReader to be at element: '{0}', found: '{1}'.";
        internal const string IDX13106 = "IDX13106: Unable to read for Saml2SecurityToken. Element: '{0}' as missing Attribute: '{1}'.";
        internal const string IDX13107 = "IDX13107: When reading '{0}', '{1}' was not a Absolute Uri, was: '{2}'.";
        internal const string IDX13108 = "IDX13108: When reading '{0}', Assertion.Subject is null and no Statements were found. [Saml2Core, line 585].";
        internal const string IDX13109 = "IDX13109: When reading '{0}', Assertion.Subject is null and an Authentication, Attribute or AuthorizationDecision Statement was found. and no Statements were found. [Saml2Core, lines 1050, 1168, 1280].";
        internal const string IDX13110 = "IDX13110: The Saml2SecurityToken must have a value for its Assertion property.";
        internal const string IDX13117 = "IDX13117: A <saml:EncryptedAttribute> was encountered while processing the attribute statement.To handle encrypted attributes, extend the Saml2SecurityTokenHandler and override ReadAttributeStatement.";
        internal const string IDX13118 = "IDX13118: A <saml:AuthnContextDecl> element was encountered.To handle by-value authentication context declarations, extend Saml2SecurityTokenHandler and override ReadAuthenticationContext.In addition, it may be necessary to extend Saml2AuthenticationContext so that its data model can accommodate the declaration value.";
        internal const string IDX13119 = "IDX13119: An abstract element was encountered which does not specify its concrete type. Element name: '{0}' Namespace: '{1}'";
        internal const string IDX13120 = "IDX13120: A <saml:Conditions> element contained more than one '{0}' condition.";
        internal const string IDX13121 = "IDX13121: A <saml:Condition> was encountered which specifies an unrecognized concrete type.To handle a custom Condition, extend Saml2SecurityTokenHandler and override ReadConditions.";
        internal const string IDX13122 = "IDX13122: The <saml:Evidence> element must contain at least one assertion or assertion reference.";
        internal const string IDX13124 = "IDX13124: The SAML NameIdentifier '{0}' is of format '{1}' and NameQualifier/SPNameQualifier/SPProvidedID is not omitted.";
        internal const string IDX13125 = "IDX13125: A Saml2Subject that does not specify an NameId cannot have an empty SubjectConfirmations collection.";
        internal const string IDX13126 = "IDX13126: A <saml:SubjectConfirmationData> element of an unexpected type was encountered.The SubjectConfirmationDataType and KeyInfoConfirmationDataType are handled by default. To handle other types, extend Saml2SecurityTokenHandler and override ReadSubjectConfirmationData. Name: '{0}' Namespace: '{1}'";
        internal const string IDX13127 = "IDX13127: A <saml:SubjectConfirmationData> element cannot be empty when of type KeyInfoConfirmationDataType.";
        internal const string IDX13128 = "IDX13128: A <saml:'{0}'> contained unrecognized content.The schema allows arbitrary XML elements on this element without explicit schema extension.To handle the content, extend Saml2Serializer.";
        internal const string IDX13129 = "IDX13129: The SAML2:AttributeStatement must contain at least one SAML2:Attribute.";
        internal const string IDX13130 = "IDX13130: A Saml2AudienceRestriction must specify at least one Audience.";
        internal const string IDX13133 = "IDX13133: The Saml2Assertion Statements collection contains an unrecognized Saml2Statement.To handle custom Saml2Statement objects, extend Saml2SecurityTokenHandler and override WriteStatement. Type: '{0}'";
        internal const string IDX13136 = "IDX13136: Unable to read for Saml2SecurityToken. Required Element: '{0}' is missing or empty.";
        internal const string IDX13137 = "IDX13137: Unable to read for Saml2SecurityToken. Version must be '2.0' was: '{0}'.";
        internal const string IDX13138 = "IDX13138: Unable to read for Saml2SecurityToken. the AttributeStatement does not contain any Attributes.";
        internal const string IDX13139 = "IDX13139: Uri must be an AbsoluteUri is: '{0}'";
        internal const string IDX13140 = "IDX13140: EncryptedId is not supported. You will need to override ReadEncryptedId and provide support.";
        internal const string IDX13141 = "IDX13141: EncryptedAssertion is not supported. You will need to override ReadAssertion and provide support.";
        internal const string IDX13313 = "IDX13313: 'AuthnStatement' cannot be empty.";
        internal const string IDX13312 = "IDX13312: 'AuthnContext' cannot be empty.";
        internal const string IDX13314 = "IDX13314: 'AuthzDecisionStatement' cannot be empty (must have at least one 'Subject').";

        // Saml2SecurityTokenHandler writing
        internal const string IDX13142 = "IDX13142: A Saml2SamlAttributeStatement can only have one Saml2Attribute of type 'Actor'. This special Saml2Attribute is used in delegation scenarios.";
        internal const string IDX13149 = "IDX13149: Both AuthenticationContext ClassReference DeclarationReference can not be null.";
        internal const string IDX13150 = "IDX13150: The Saml2SecurityTokenHandler can only write a token was of type: '{0}'.";
        internal const string IDX13151 = "IDX13151: Cannot write '{0}' because '{1}' is null or empty.";
        internal const string IDX13300 = "IDX13300: '{0}' must be an absolute Uri, was: '{1}'";
        internal const string IDX13302 = "IDX13302: An assertion with no statements must contain a 'Subject' element.";
        internal const string IDX13303 = "IDX13303: 'Subject' is required in Saml2Assertion for built-in statement type.";
        internal const string IDX13304 = "IDX13304: Encryption is not supported in writing saml2 nameIdentifier.";
        internal const string IDX13305 = "IDX13305: Both id and subjectconfirmation are null in saml2 subject: '{0}'.";
        internal const string IDX13306 = "IDX13306: Multiple name identifier claim is not allowed in tokenDescriptor.Subject.Claims.";
        internal const string IDX13310 = "IDX13310: SAML2 AuthorizationDecision DecisionType must be 'Permit', 'Deny' or 'Indeterminate'.";

        // IDX11900 - AuthorizationDecisionStatement
        internal const string IDX13900 = "IDX13900: Unable to write Saml2Assertion: {0} is required, {1} is null or empty.";
        internal const string IDX13901 = "IDX13901: Unable to write Saml2Assertion: {0}.{1} is empty. This statement must contain at least one Action.";
        internal const string IDX13902 = "IDX13902: Unable to write Saml2Assertion: Saml2Evidence must have at least one assertion or assertion reference.";

        internal const string IDX13950 = "IDX13950: API is not supported";
        internal const string IDX13951 = "IDX13951: Validation of confirmation data is currently not supported by default. To customize SubjectConfirmationData processing, extend Saml2SecurityTokenHandler and override ValidateConfirmationData.";
#pragma warning restore 1591
    }
}
