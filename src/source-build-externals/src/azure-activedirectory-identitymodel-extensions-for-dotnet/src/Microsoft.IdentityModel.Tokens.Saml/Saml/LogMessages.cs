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

// Microsoft.IdentityModel.Tokens.Saml
// Range: 11000 - 11999

namespace Microsoft.IdentityModel.Tokens.Saml
{
    /// <summary>
    /// Log messages and codes
    /// </summary>
    internal static class LogMessages
    {
#pragma warning disable 1591
        // SecurityTokenHandler messages
        internal const string IDX11400 = "IDX11400: The '{0}', can only process SecurityTokens of type: '{1}'. The SecurityToken received is of type: '{2}'.";
        internal const string IDX11401 = "IDX11401: Unable to validate token. TokenValidationParameters.RequireAudience is true but no AudienceRestrictions were found in the inbound token.";

        // signature creation / validation
        internal const string IDX11312 = "IDX11312: Unable to validate token. A SamlSamlAttributeStatement can only have one SamlAttribute of type 'Actor'. This special SamlAttribute is used in delegation scenarios.";
        internal const string IDX11313 = "IDX11313: Unable to process Saml attribute. A SamlSubject must contain either or both of Name and ConfirmationMethod.";
        internal const string IDX11314 = "IDX11314: The AttributeValueXsiType of a SAML Attribute must be a string of the form 'prefix#suffix', where prefix and suffix are non-empty strings. Found: '{0}'";

        // SamlSerializer reading
        internal const string IDX11100 = "IDX11100: Saml Only one element of type '{0}' is supported.";
        internal const string IDX11102 = "IDX11102: Saml An AuthorizationDecisionStatement must have at least one Action.";
        internal const string IDX11104 = "IDX11104: Saml Name cannot be null or empty.";
        internal const string IDX11107 = "IDX11107: Saml A Subject requires a NameIdentifier or ConfirmationMethod.";
        internal const string IDX11108 = "IDX11108: Saml AuthorityBinding.AuthorityKind is not well formed. Is should be of the form str:str, it is '{0}'.";
        internal const string IDX11110 = "IDX11110: The SamlSecurityToken must have a value for its Assertion property.";

        internal const string IDX11111 = "IDX11111: When reading '{0}', '{1}' was not a Absolute Uri, was: '{2}'.";
        internal const string IDX11112 = "IDX11112: Exception thrown while reading '{0}' for SamlSecurityToken. Inner exception: '{1}'.";
        internal const string IDX11114 = "IDX11114: Unable to read SamlSecurityToken. Expecting XmlReader sto be at element: '{0}', found 'Empty Element'.";
        internal const string IDX11115 = "IDX11115: Unable to read SamlSecurityToken. Element: '{0}' was missing Attribute: '{1}'.";
        internal const string IDX11116 = "IDX11116: Unable to read SamlSecurityToken. MajorVersion must be '1' was: '{0}'.";
        internal const string IDX11117 = "IDX11117: Unable to read SamlSecurityToken. MinorVersion must be '1' was: '{0}'.";
        internal const string IDX11118 = "IDX11118: Unable to read condition : '{0}'. SamlSecurityToken only support AudienceRestrictionCondition and DoNotCacheCondition.";
        internal const string IDX11120 = "IDX11120: Unable to read SamlSecurityToken. Expecting XmlReader to be at element: '{0}', found: '{1}'.";
        internal const string IDX11121 = "IDX11121: Invalid SAML AssertionID: '{0}'.";
        internal const string IDX11122 = "IDX11122: Exception thrown while reading '{0}' for SamlSecurityToken. Inner exception: '{1}'.";
        internal const string IDX11123 = "IDX11123: Unable to read SamlSecurityToken. Expecting XmlReader to be at element: '{0}', found 'Empty Element'.";
        internal const string IDX11124 = "IDX11124: Unable to read SamlSecurityToken. Unexpecting element: '{0}' in element: '{1}' has been found.";
        internal const string IDX11125 = "IDX11125: Unable to read SamlSecurityToken. Missing data in element: '{0}'.";
        internal const string IDX11126 = "IDX11126: A <saml:'{0}'> contained unrecognized content: '{1}'.The schema allows arbitrary XML elements on this element without explicit schema extension.To handle the content, extend SamlSerializer.";
        internal const string IDX11127 = "IDX11127: Saml unable to read Conditions.";
        internal const string IDX11128 = "IDX11128: Saml unable to read Advice.";
        internal const string IDX11129 = "IDX11129: Saml unable to read Statement.";
        internal const string IDX11130 = "IDX11130: When reading '{0}', no Statements were found.";
        internal const string IDX11131 = "IDX11131: Unable to read for SamlSecurityToken, the AttributeStatement does not contain any Attributes.";
        internal const string IDX11132 = "IDX11132: The Attribute in SamlSecurityToken does not contain any AttributeValue.";
        internal const string IDX11133 = "IDX11133: The <saml:Evidence> element must contain at least one assertion or assertion reference.";
        internal const string IDX11134 = "IDX11134: Unable to read SamlSecurityToken. Expecting XmlReader to be at element: '{0}', found: '{1}'.";
        internal const string IDX11135 = "IDX11135: Unable to read SamlSecurityToken. Saml element '{0}' must have value.";
        internal const string IDX11136 = "IDX11136: 'AuthorizationDecisionStatement' cannot be empty.";
        internal const string IDX11137 = "IDX11137: 'SamlAction' must have a value.";

        // Saml writting
        internal const string IDX11501 = "IDX11501: SamlAssertion Id cannot be null or empty.";
        internal const string IDX11502 = "IDX11502: Value is not an AbsoluteUri: '{0}'.";
        internal const string IDX11503 = "IDX11503: SamlAssertion Id is not well formed: '{0}'.";
        internal const string IDX11504 = "IDX11504: Issuer cannot be null or empty.";
        internal const string IDX11505 = "IDX11505: A SamlAssertion must have at least one statement.";
        internal const string IDX11506 = "IDX11506: A SamlAttribute Value cannot be null or empty.";
        internal const string IDX11507 = "IDX11507: SAML AuthorityKind missing name.";
        internal const string IDX11508 = "IDX11508: SAML AuthorizationDecision should have one Action.";
        internal const string IDX11509 = "IDX11509: SAML Evidence should have one Assertion.";
        internal const string IDX11510 = "IDX11510: SAML Subject requires ConfirmationMethod when ConfirmationData or KeyInfo is specified.";
        internal const string IDX11511 = "IDX11511: SAML unable to read Attribute.";
        internal const string IDX11512 = "IDX11512: SAML AuthorityBinding missing Binding on read.";
        internal const string IDX11513 = "IDX11513: SAML AuthorityBinding missing Location on read.";
        internal const string IDX11514 = "IDX11514: SamlSubjectEqualityComparer cannot be null.";
        internal const string IDX11515 = "IDX11515: Each statement in Assertion.Statements has to be a SamlSubjectStatement";
        internal const string IDX11516 = "IDX11516: We could not write the SamlStatement of type:'{0}'. You will need to override this method to write this statement.";
        internal const string IDX11517 = "IDX11517: Exception thrown while writing '{0}' for SamlSecurityToken. Inner exception: '{1}'.";
        internal const string IDX11518 = "IDX11518: Unable to writen SamlAssertion: SamlSubject.Name and SamlSubject.ConfirmationMethods.Count == 0.";
        internal const string IDX11521 = "IDX11521: Multiple name identifier claim is not allowed in tokenDescriptor.Subject.Claims.";
        internal const string IDX11522 = "IDX11522: More than one delegates acting as an identity are found in Saml attribute.";
        internal const string IDX11523 = "IDX11523: The claim type must have namespace and name which separated by slash. Input claim: '{0}'.";

        // IDX11800 - AuthenticationStatement
        internal const string IDX11800 = "IDX11800: Unable to write SamlAssertion: {0} is required, {1}.{2} is null or empty.";

        // IDX11900 - AuthorizationDecisionStatement
        internal const string IDX11900 = "IDX11900: Unable to write SamlAssertion: {0} is required, {1}.{2} is null or empty.";
        internal const string IDX11901 = "IDX11901: Unable to write SamlAssertion: {0}.{1} is empty. This statement must contain at least one Action.";
        internal const string IDX11902 = "IDX11902: Unable to write SamlAssertion: SamlEvidence must have at least one assertion or assertion reference.";

        internal const string IDX11950 = "IDX11950: API is not supported";
#pragma warning restore 1591
    }
}
