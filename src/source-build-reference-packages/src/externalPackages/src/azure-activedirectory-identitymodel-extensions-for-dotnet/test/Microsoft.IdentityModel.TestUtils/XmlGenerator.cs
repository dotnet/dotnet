// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.IdentityModel.Xml;

namespace Microsoft.IdentityModel.TestUtils
{
    public class XmlEement
    {
        public XmlEement(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }

        public object Value { get; }

        public static string Generate(XmlEement dataElement)
        {
            int initialCapacity = (dataElement.Name.Length + 3) * 2;
            var stringBuilder = new StringBuilder(initialCapacity);
            stringBuilder.Append('<').Append(dataElement.Name).Append('>');
            if (dataElement.Value is string str)
                stringBuilder.Append(str);
            else if (dataElement.Value is XmlEement element)
                stringBuilder.Append(Generate(element));
            else if (dataElement.Value is List<XmlEement> elements)
                foreach (var ele in elements)
                    stringBuilder.Append(Generate(ele));
            else
                throw new TestException($"dataElement.Value must be of type: '{typeof(string)}' or '{typeof(XmlEement)} or '{typeof(List<XmlEement>)}' was: {dataElement.Value.GetType()}.");

            stringBuilder.Append("</").Append(dataElement.Name).Append('>');
            return stringBuilder.ToString();
        }
    }

    public static class XmlGenerator
    {
        public static string ClaimTypesOffered
        {
            get => "<fed:ClaimTypesOffered><auth:ClaimType Uri=\"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>Name</auth:DisplayName><auth:Description>The mutable display name of the user.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>Subject</auth:DisplayName><auth:Description>An immutable, globally unique, non-reusable identifier of the user that is unique to the application for which a token is issued.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>Given Name</auth:DisplayName><auth:Description>First name of the user.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>Surname</auth:DisplayName><auth:Description>Last name of the user.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.microsoft.com/identity/claims/displayname\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>Display Name</auth:DisplayName><auth:Description>Display name of the user.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.microsoft.com/identity/claims/nickname\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>Nick Name</auth:DisplayName><auth:Description>Nick name of the user.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>Authentication Instant</auth:DisplayName><auth:Description>The time (UTC) when the user is authenticated to Windows Azure Active Directory.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>Authentication Method</auth:DisplayName><auth:Description>The method that Windows Azure Active Directory uses to authenticate users.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.microsoft.com/identity/claims/objectidentifier\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>ObjectIdentifier</auth:DisplayName><auth:Description>Primary identifier for the user in the directory. Immutable, globally unique, non-reusable.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.microsoft.com/identity/claims/tenantid\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>TenantId</auth:DisplayName><auth:Description>Identifier for the user's tenant.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.microsoft.com/identity/claims/identityprovider\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>IdentityProvider</auth:DisplayName><auth:Description>Identity provider for the user.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>Email</auth:DisplayName><auth:Description>Email address of the user.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.microsoft.com/ws/2008/06/identity/claims/groups\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>Groups</auth:DisplayName><auth:Description>Groups of the user.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.microsoft.com/identity/claims/accesstoken\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>External Access Token</auth:DisplayName><auth:Description>Access token issued by external identity provider.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.microsoft.com/ws/2008/06/identity/claims/expiration\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>External Access Token Expiration</auth:DisplayName><auth:Description>UTC expiration time of access token issued by external identity provider.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.microsoft.com/identity/claims/openid2_id\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>External OpenID 2.0 Identifier</auth:DisplayName><auth:Description>OpenID 2.0 identifier issued by external identity provider.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.microsoft.com/claims/groups.link\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>GroupsOverageClaim</auth:DisplayName><auth:Description>Issued when number of user's group claims exceeds return limit.</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.microsoft.com/ws/2008/06/identity/claims/role\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>Role Claim</auth:DisplayName><auth:Description>Roles that the user or Service Principal is attached to</auth:Description></auth:ClaimType><auth:ClaimType Uri=\"http://schemas.microsoft.com/ws/2008/06/identity/claims/wids\" xmlns:auth=\"http://docs.oasis-open.org/wsfed/authorization/200706\"><auth:DisplayName>RoleTemplate Id Claim</auth:DisplayName><auth:Description>Role template id of the Built-in Directory Roles that the user is a member of</auth:Description></auth:ClaimType></fed:ClaimTypesOffered>";
        }

        public static string IDPSSODescriptorTemplate
        {
            get => "<IDPSSODescriptor protocolSupportEnumeration=\"urn:oasis:names:tc:SAML:2.0:protocol\">{0}</IDPSSODescriptor>";
        }

        public static string EntityDescriptorTemplate
        {
            get => "<?xml version=\"1.0\" encoding=\"utf-8\"?><EntityDescriptor ID=\"{0}\" entityID=\"{1}\" xmlns=\"{2}\">{3}</EntityDescriptor>";
        }

        public static string KeyDescriptorTemplate
        {
            get => "<KeyDescriptor use=\"{0}\">{1}</KeyDescriptor>";
        }

        public static string KeyDescriptorXml(KeyInfo keyInfo)
        {
            return string.Format(KeyDescriptorTemplate, XmlSignatureConstants.Namespace, Generate(keyInfo));
        }

        public static string KeyDescriptorXml(string @namespace, string keyInfo)
        {
            return string.Format(KeyDescriptorTemplate, @namespace, keyInfo);
        }

        public static string KeyInfoTemplate
        {
            get => "<KeyInfo xmlns=\"{0}\">{1}</KeyInfo>";
        }

        public static string KeyInfoXml(string @namespace, XmlEement x509Data)
        {
            return string.Format(KeyInfoTemplate, @namespace, XmlEement.Generate(x509Data));
        }

        public static string NotBeforeXml(string notBefore)
        {
            if (string.IsNullOrEmpty(notBefore))
                return string.Empty;

            return $"NotBefore = \"{notBefore}\"";
        }

        public static string NotOnOrAfterXml(string notOnOrAfter)
        {
            if (string.IsNullOrEmpty(notOnOrAfter))
                return string.Empty;

            return $"NotOnOrAfter = \"{notOnOrAfter}\"";
        }

        public static string SamlActionTemplate
        {
            get => "<Action Namespace=\"{0}\" xmlns=\"{1}\">{2}</Action>";
        }

        public static string SamlActionXml(string @namespace, string actionNamespace, string action)
        {
            return string.Format(SamlActionTemplate, actionNamespace, @namespace, action);
        }

        public static string SamlAttributeValueTemplate
        {
            get => "<AttributeValue xmlns=\"{0}\">{1}</AttributeValue>";
        }

        public static string SamlAttributeValueXml(string @namespace, string value)
        {
            return string.Format(SamlAttributeValueTemplate, @namespace, value);
        }

        public static string SamlAttributeTemplate
        {
            get => "<Attribute AttributeName=\"{0}\" AttributeNamespace=\"{1}\" xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\">{2}</Attribute>";
        }

        public static string SamlAttributeXml(string name, string attributeNs, IEnumerable<string> attributes)
        {
            return string.Format(SamlAttributeTemplate, name, attributeNs, (attributes == null) ? string.Empty : string.Concat(attributes));
        }

        public static string SamlAttributeStatementTemplate
        {
            get => "<AttributeStatement xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\">{0} {1}</AttributeStatement>";
        }

        public static string SamlAttributeStatementXml(string subject, string attributes)
        {
            return string.Format(SamlAttributeStatementTemplate, subject, attributes);
        }

        public static string SamlAudienceTemplate
        {
            get => "<Audience xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\">{0}</Audience>";
        }

        public static string SamlAudienceXml(string audience)
        {
            return string.Format(SamlAudienceTemplate, audience);
        }

        public static string SamlAudienceRestrictionConditionTemplate
        {
            get => "<AudienceRestrictionCondition xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\">{0}</AudienceRestrictionCondition>";
        }

        public static string SamlAudienceRestrictionConditionXml(IEnumerable<string> audiences)
        {
            return string.Format(SamlAudienceRestrictionConditionTemplate, string.Concat(audiences));
        }

        public static string SamlSubjectLocalityTemplate
        {
            get => "<SubjectLocality IPAddress=\"{0}\" DNSAddress=\"{1}\" xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\"/>";
        }

        public static string SamlSubjectLocalityXml(string ipAddress, string dnsAddress)
        {
            return string.Format(SamlSubjectLocalityTemplate, ipAddress, dnsAddress);
        }

        public static string SamlAuthorityBindingTemplate
        {
            get => "<AuthorityBinding AuthorityKind=\"{0}\" Location=\"{1}\" Binding=\"{2}\" xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\"/>";
        }

        public static string SamlAuthorityBindingXml(string authorityKind, string location, string binding)
        {
            return string.Format(SamlAuthorityBindingTemplate, authorityKind, location, binding);
        }

        public static string SamlAuthenticationStatementTemplate
        {
            get => "<AuthenticationStatement AuthenticationMethod=\"{0}\" AuthenticationInstant=\"{1}\" xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\">{2}{3}{4}</AuthenticationStatement>";
        }

        public static string SamlAuthenticationStatementXml(string method, string instant, string subject, string subjectLocality, string binding)
        {
            return string.Format(SamlAuthenticationStatementTemplate, method, instant, subject, subjectLocality, binding);
        }

        public static string SamlConditionsTemplate
        {
            get => "<Conditions {0} {1} xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\">{2}</Conditions>";
        }

        public static string SamlConditionsXml(string notBefore, string notOnOrAfter, IEnumerable<string> conditions)
        {
            return string.Format(SamlConditionsTemplate, NotBeforeXml(notBefore), NotOnOrAfterXml(notOnOrAfter), (conditions == null) ? string.Empty : string.Concat(conditions));
        }

        public static string SamlNameIdentifierTemplate
        {
            get => "<NameIdentifier NameQualifier=\"{0}\" Format=\"{1}\" xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\">{2}</NameIdentifier>";
        }

        public static string SamlNameIdentifierXml(string nameQualifier, string format, string name)
        {
            return string.Format(SamlNameIdentifierTemplate, nameQualifier, format, name);
        }

        public static string SamlConfirmationMethodTemplate
        {
            get => "<ConfirmationMethod xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\">{0}</ConfirmationMethod>";
        }

        public static string SamlConfirmationMethodXml(string confirmationMethod)
        {
            return string.Format(SamlConfirmationMethodTemplate, confirmationMethod);
        }

        public static string SamlAdviceTemplate
        {
            get => "<Advice xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\">{0}{1}</Advice>";
        }

        public static string SamlAdviceXml(string assertionIDRef, string assertion)
        {
            return string.Format(SamlAdviceTemplate, assertionIDRef, assertion);
        }

        public static string SamlAssertionTemplate
        {
            get => "<Assertion MajorVersion=\"{0}\" MinorVersion=\"{1}\" AssertionID=\"{2}\" Issuer=\"{3}\" IssueInstant=\"{4}\" xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\">{5}{6}{7}{8}</Assertion>";
        }

        public static string SamlAssertionXml(string majorVersion, string minorVersion, string assertionId, string issuer, string issueInstant, string conditions, string advices, string statements, string signature)
        {
            return string.Format(SamlAssertionTemplate, majorVersion, minorVersion, assertionId, issuer, issueInstant, conditions, advices, statements, signature);
        }

        public static string SamlAssertionIDRefTemplate
        {
            get => "<AssertionIDReference xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\">{0}</AssertionIDReference>";
        }

        public static string SamlAssertionIDRefXml(string assertionID)
        {
            return string.Format(SamlAssertionIDRefTemplate, assertionID);
        }
        public static string SamlAuthorizationDecisionStatementTemplate
        {
            get => "<AuthorizationDecisionStatement Resource=\"{0}\" Decision=\"{1}\" xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\">{2}{3}{4}</AuthorizationDecisionStatement>";
        }

        public static string SamlAuthorizationDecisionStatementXml(string resource, string decision, string subject, string action, string evidence)
        {
            return string.Format(SamlAuthorizationDecisionStatementTemplate, resource, decision, subject, action, evidence);
        }

        public static string SamlEvidenceTemplate
        {
            get => "<Evidence xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\">{0}{1}</Evidence>";
        }

        public static string SamlEvidenceXml(string assertionIdRef, string assertion)
        {
            return string.Format(SamlEvidenceTemplate, assertionIdRef, assertion);
        }

        public static string SamlSubjectConfirmationTemplate
        {
            get => "<SubjectConfirmation xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\">{0}<SubjectConfirmationData>{1}</SubjectConfirmationData></SubjectConfirmation>";
        }

        public static string SamlSubjectConfirmationXml(List<string> confirmations, string confirmationData)
        {

            return string.Format(SamlSubjectConfirmationTemplate, (confirmations == null) ? string.Empty : string.Concat(confirmations), confirmationData);
        }

        public static string SamlSubjectTemplate
        {
            get => "<Subject xmlns=\"urn:oasis:names:tc:SAML:1.0:assertion\">{0} {1}</Subject>";
        }

        public static string SamlSubjectXml(string nameIdentifier, string confirmation)
        {
            return string.Format(SamlSubjectTemplate, nameIdentifier, confirmation);
        }

        public static string Generate(KeyInfo keyInfo)
        {
            var str = "";
            foreach (var data in keyInfo.X509Data)
            {
                // Make a new list of elements for each X509Data object
                var elements = new List<XmlEement>();
                foreach (var certificate in data.Certificates)
                {
                    elements.Add(new XmlEement(XmlSignatureConstants.Elements.X509Certificate, certificate));
                }
                if (data.IssuerSerial != null)
                {
                    if (!string.IsNullOrEmpty(data.IssuerSerial.IssuerName))
                        elements.Add(new XmlEement(XmlSignatureConstants.Elements.X509IssuerName, data.IssuerSerial.IssuerName));

                    if (!string.IsNullOrEmpty(data.IssuerSerial.SerialNumber))
                        elements.Add(new XmlEement(XmlSignatureConstants.Elements.X509SerialNumber, data.IssuerSerial.SerialNumber));
                }
                if (!string.IsNullOrEmpty(data.SKI))
                    elements.Add(new XmlEement(XmlSignatureConstants.Elements.X509SKI, data.SKI));

                if (!string.IsNullOrEmpty(data.SubjectName))
                    elements.Add(new XmlEement(XmlSignatureConstants.Elements.X509SubjectName, data.SubjectName));

                str += XmlEement.Generate(new XmlEement(XmlSignatureConstants.Elements.X509Data, elements));
            }

            return string.Format(KeyInfoTemplate, XmlSignatureConstants.Namespace, str);
        }

        public static string Generate(Signature signature)
        {
            return Generate(signature, new DSigSerializer());
        }

        public static string Generate(Signature signature, DSigSerializer dSigSerializer)
        {
            var signatureBytes = XmlUtilities.GenerateSignatureBytes(signature.SignedInfo, Default.AsymmetricSigningKey);
            signature.SignatureValue = Convert.ToBase64String(signatureBytes);
            var memoryStream = new MemoryStream();
            var writer = XmlDictionaryWriter.CreateTextWriter(memoryStream, Encoding.UTF8, false);
            dSigSerializer.WriteSignature(writer, signature);
            writer.Flush();

            // for debugging purposes use a local variable.
            var retval = Encoding.UTF8.GetString(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
            return retval;
        }

        public static string Generate(SignedInfo signedInfo)
        {
            var memoryStream = new MemoryStream();
            var writer = XmlDictionaryWriter.CreateTextWriter(memoryStream, Encoding.UTF8, false);
            var serializer = new DSigSerializer();
            serializer.WriteSignedInfo(writer, signedInfo);
            writer.Flush();

            var retval = Encoding.UTF8.GetString(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
            return retval;
        }

        public static string ReferenceTemplate
        {
            get => "<{0}Reference Id=\"{1}\" Type=\"{2}\" URI=\"{3}\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\"><Transforms><Transform Algorithm=\"{4}\" /><Transform Algorithm=\"{5}\" /></Transforms><DigestMethod Algorithm=\"{6}\" /><DigestValue>{7}</DigestValue></{0}Reference>";
        }

        public static string ReferenceXml(string prefix, string id, string type, string referenceUri, string envelopingAlgorithm, string c14nAlgorithm, string digestAlgorithm, string digestValue)
        {
            return string.Format(ReferenceTemplate, prefix, id, type, referenceUri, envelopingAlgorithm, c14nAlgorithm, digestAlgorithm, digestValue);
        }

        // Always assumes two transforms
        public static string ReferenceXml(Reference reference)
        {
            var memoryStream = new MemoryStream();
            var writer = XmlDictionaryWriter.CreateTextWriter(memoryStream, Encoding.UTF8, false);
            var serializer = new DSigSerializer();
            serializer.WriteReference(writer, reference);
            writer.Flush();

            var retval = Encoding.UTF8.GetString(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
            return retval;
        }

        public static string RoleDescriptorTemplate
        {
            get => "<RoleDescriptor xsi:type=\"{0}\" protocolSupportEnumeration=\"{1}\" xmlns:xsi=\"{2}\" xmlns:fed=\"{3}\">{4}{5}{6}{7}{8}{9}{10}</RoleDescriptor>";
        }

        public static string SignatureTemplate
        {
            get => "<Signature xmlns=\"{0}\">{1}<SignatureValue>{2}</SignatureValue>{3}</Signature>";
        }

        public static string SignatureXml(string @namespace, string signatureValue, string signedInfo, string keyInfo)
        {
            return string.Format(SignedInfoTemplate, @namespace, signatureValue, signedInfo, keyInfo);
        }

        public static string SignedInfoTemplate
        {
            get => "<SignedInfo xmlns=\"{0}\"><CanonicalizationMethod Algorithm=\"{1}\" /><SignatureMethod Algorithm=\"{2}\" />{3}</SignedInfo>";
        }

        public static string SignedInfoXml(string @namespace, string c14nAlgorithm, string signatureAlgorithm, string reference)
        {
            return string.Format(SignedInfoTemplate, @namespace, c14nAlgorithm, signatureAlgorithm, reference);
        }

        public static string SingleLogoutServiceTemplate
        {
            get => "<SingleLogoutService Binding=\"urn:oasis:names:tc:SAML:2.0:bindings:HTTP-Redirect\" Location=\"https://login.microsoftonline.com/common/saml2\" />";
        }

        public static string SingleSignOnServiceTemplate
        {
            get => "<SingleSignOnService Binding=\"urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST\" Location=\"https://login.microsoftonline.com/common/saml2\" />";
        }

        public static string TransformTemplate
        {
            get => "<{0}{1} {2} = \"{3}\" {4} />";
        }

        public static string TransformXml(string prefix, string attributeName, string algorithm, string @namespace )
        {
            return string.Format(TransformTemplate, prefix, XmlSignatureConstants.Elements.Transform, attributeName, algorithm, @namespace);
        }

        public static string TransformXml(string prefix, string element, string attributeName, string algorithm, string @namespace)
        {
            return string.Format(TransformTemplate, prefix, element, attributeName, algorithm, @namespace);
        }

        public static string TransformsXml(string prefix, List<string> transforms, string @namespace)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append('<').Append(prefix).Append("Transforms ").Append(@namespace).Append('>');
            foreach (var transform in transforms)
                stringBuilder.Append(transform);
            stringBuilder.Append("</").Append(prefix).Append("Transforms >");

            return stringBuilder.ToString();
        }

        public static string TransformTemplateWithInclusivePrefix
        {
            get => "<{0}{1} {2} = \"{3}\"> {4} </{0}{1}>";
        }

        public static string TransformWithInclusivePrefixXml(string prefix, string attributeName, string algorithm, string @namespace, string inclusivePrefix)
        {
            return string.Format(TransformTemplateWithInclusivePrefix, prefix, XmlSignatureConstants.Elements.Transform, attributeName, algorithm, inclusivePrefix);
        }
    }
}
