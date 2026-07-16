// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Xml.Tests
{
    public class ReferenceTests
    {
        [Fact]
        public void GetSets()
        {
            var type = typeof(Reference);
            var properties = type.GetProperties();
            Assert.True(properties.Length == 9, $"Number of properties has changed from 9 to: {properties.Length}, adjust tests");
            var context = new GetSetContext
            {
                PropertyNamesAndSetGetValue = new List<KeyValuePair<string, List<object>>>
                {
                    new KeyValuePair<string, List<object>>("DigestMethod", new List<object>{null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                    new KeyValuePair<string, List<object>>("DigestValue", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                    new KeyValuePair<string, List<object>>("Id", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                    new KeyValuePair<string, List<object>>("Prefix", new List<object>{"", Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                    new KeyValuePair<string, List<object>>("TokenStream", new List<object>{(XmlTokenStream)null, new XmlTokenStream(), new XmlTokenStream()}),
                    new KeyValuePair<string, List<object>>("Type", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                    new KeyValuePair<string, List<object>>("Uri", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                },
                Object = new Reference(),
            };

            TestUtilities.GetSet(context);
            TestUtilities.AssertFailIfErrors($"{this}.GetSets", context.Errors);
        }

        [Fact]
        public void VerifyReference()
        {
            var saml1 = @"<saml:Assertion MajorVersion=""1"" MinorVersion=""1"" AssertionID=""_e35fc6da-147e-428e-8c71-fb32867598ab"" Issuer=""http://sts.sub2.fracas365.msftonlinerepro.com/adfs/services/trust"" IssueInstant=""2017-05-08T14:57:58.348Z"" xmlns:saml=""urn:oasis:names:tc:SAML:1.0:assertion""><saml:Conditions NotBefore=""2017-05-08T14:57:58.348Z"" NotOnOrAfter=""2017-05-08T15:57:58.348Z""><saml:AudienceRestrictionCondition><saml:Audience>https://app1.sub2.fracas365.msftonlinerepro.com/sampapp/</saml:Audience></saml:AudienceRestrictionCondition></saml:Conditions><saml:AttributeStatement><saml:Subject><saml:SubjectConfirmation><saml:ConfirmationMethod>urn:oasis:names:tc:SAML:1.0:cm:bearer</saml:ConfirmationMethod></saml:SubjectConfirmation></saml:Subject><saml:Attribute AttributeName=""upn"" AttributeNamespace=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims""><saml:AttributeValue>killer@sub2.fracas365.msftonlinerepro.com</saml:AttributeValue></saml:Attribute><saml:Attribute AttributeName=""x-ms-endpoint-absolute-path"" AttributeNamespace=""http://schemas.microsoft.com/2012/01/requestcontext/claims"" a:OriginalIssuer=""CLIENT CONTEXT"" xmlns:a=""http://schemas.xmlsoap.org/ws/2009/09/identity/claims""><saml:AttributeValue>/adfs/ls/wia</saml:AttributeValue></saml:Attribute><saml:Attribute AttributeName=""x-ms-client-ip"" AttributeNamespace=""http://schemas.microsoft.com/2012/01/requestcontext/claims"" a:OriginalIssuer=""CLIENT CONTEXT"" xmlns:a=""http://schemas.xmlsoap.org/ws/2009/09/identity/claims""><saml:AttributeValue>172.15.0.67</saml:AttributeValue></saml:Attribute><saml:Attribute AttributeName=""primarygroupsid"" AttributeNamespace=""http://schemas.microsoft.com/ws/2008/06/identity/claims""><saml:AttributeValue>S-1-5-21-487734988-61580006-1080473273-513</saml:AttributeValue></saml:Attribute><saml:Attribute AttributeName=""authnmethodsreferences"" AttributeNamespace=""http://schemas.microsoft.com/claims""><saml:AttributeValue>http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/windows</saml:AttributeValue></saml:Attribute><saml:Attribute AttributeName=""windowsaccountname"" AttributeNamespace=""http://schemas.microsoft.com/ws/2008/06/identity/claims""><saml:AttributeValue>FRACAS-O365\killer</saml:AttributeValue></saml:Attribute><saml:Attribute AttributeName=""streetAddress"" AttributeNamespace=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims""><saml:AttributeValue>street
with
return</saml:AttributeValue></saml:Attribute></saml:AttributeStatement><saml:AuthenticationStatement AuthenticationMethod=""urn:federation:authentication:windows"" AuthenticationInstant=""2017-05-08T14:57:58.333Z""><saml:Subject><saml:SubjectConfirmation><saml:ConfirmationMethod>urn:oasis:names:tc:SAML:1.0:cm:bearer</saml:ConfirmationMethod></saml:SubjectConfirmation></saml:Subject></saml:AuthenticationStatement><ds:Signature xmlns:ds=""http://www.w3.org/2000/09/xmldsig#""><ds:SignedInfo><ds:CanonicalizationMethod Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#"" /><ds:SignatureMethod Algorithm=""http://www.w3.org/2001/04/xmldsig-more#rsa-sha256"" /><ds:Reference URI=""#_e35fc6da-147e-428e-8c71-fb32867598ab""><ds:Transforms><ds:Transform Algorithm=""http://www.w3.org/2000/09/xmldsig#enveloped-signature"" /><ds:Transform Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#"" /></ds:Transforms><ds:DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256"" /><ds:DigestValue>F/TMfVx/lEtqy4aGBzMyKPj/b5iyaEH9WyhUJ1EA724=</ds:DigestValue></ds:Reference></ds:SignedInfo><ds:SignatureValue>RY27lz0BoNkWK+67gBdIFaLb2EzIcT3uKq+UTQhg1bRv6TewJScnEkhYCA4qqKEzjWgGBnkBsDfeZ44qLwvi5h1Q4S/cmY2i9eOeZnb63BosXSEzLLkhV4wT2sWy9og5EMB3IGAH5W/qjoPJybO8CMrHpwRC5YR81KsXO0O+8n2U/tih9vHd4ddxnJ1upTopcAhs5jYLVQc1pqtCZxA0EGsrpQaQFUGDggc/bhihCe4p87ppBN8CRE/zfKQYjRR1UL4dvQLDRNE+b+aeE5TIDomekgY7U2ai6NQYkJm+8lhz9824rJp8HqeMZ/77VsbpnH5i7OcOaXhXWIBBmE86eQ==</ds:SignatureValue><KeyInfo xmlns=""http://www.w3.org/2000/09/xmldsig#""><X509Data><X509Certificate>MIIDCDCCAfCgAwIBAgIQNz4YVbYAIJVFCc47HFD3RzANBgkqhkiG9w0BAQsFADBAMT4wPAYDVQQDEzVBREZTIFNpZ25pbmcgLSBzdHMuc3ViMi5mcmFjYXMzNjUubXNmdG9ubGluZXJlcHJvLmNvbTAeFw0xNTAzMzExMDQyMTNaFw0zNDA1MzAxMDQyMTNaMEAxPjA8BgNVBAMTNUFERlMgU2lnbmluZyAtIHN0cy5zdWIyLmZyYWNhczM2NS5tc2Z0b25saW5lcmVwcm8uY29tMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAxdjzB+wGV6hYekOvWwKoL/DFNBiLQsLx6w02FzcFnpGwR38gVTn/glg9CNSsOT0riRM3/MwU8o2fwseQyVtv9Kee/yvia8cB6GD0CARlYizb6GkJJzMvWkPSas1zpn10Bs3SBBgn0pvAKZCWWir5WJ7DRY32X2yo2do8mQftsoLGYsEU8+jj9AMYQWaR3A86AEWjXoQY3AodfMMzdVFX+O/kjsvKcBfPqGRT6jUSGBOOaqzMOJBT39SueD8zePDW7SejBl7fRi4TLx5H6xuMldOAAH6oD70yIrobqosGG9X/LdijHajMSoaYzZIlG7fl4PCVvAjh1Dytw/y8K70flQIDAQABMA0GCSqGSIb3DQEBCwUAA4IBAQBy08dAsSTKd2YlNE8bM4O5C2bFKR1YakR8L/zLEy8g+RNsKN5V/cIll0b/tf9iQ5464nc+nM///U+UVxqT8ipeR7ThIPwuWX98cFZFQNNGkha4PaYap/osquEpRAJOcTqZf2K95ipeQ+5Hhw00mK0hcV1QT/7maTUqCHDfBCaD+uYAFvaNBXOYpdoIGM9cMk7Qjc/yowLDm+DpmJek54MWmN+iZ0YtDEhMSh//QPFMLPT5Ucat+qRTen1HZNGdxfZ7NIIDL3dNKVDN+vDUbW7rjvPyxA8Rtj4JplI9ENqpzRq4m1sDWUTk2hJYw9Ec1kGo7AFKRmOS6DRbwUn5Ptdc</X509Certificate></X509Data></KeyInfo></ds:Signature></saml:Assertion>";

            var envelopedReader = XmlUtilities.CreateEnvelopedSignatureReader(saml1);
            while (envelopedReader.Read()) ;
            // DigestValue>F/TMfVx/lEtqy4aGBzMyKPj/b5iyaEH9WyhUJ1EA724=</ds:DigestValue>
            //<X509Certificate>MIIDCDCCAfCgAwIBAgIQNz4YVbYAIJVFCc47HFD3RzANBgkqhkiG9w0BAQsFADBAMT4wPAYDVQQDEzVBREZTIFNpZ25pbmcgLSBzdHMuc3ViMi5mcmFjYXMzNjUubXNmdG9ubGluZXJlcHJvLmNvbTAeFw0xNTAzMzExMDQyMTNaFw0zNDA1MzAxMDQyMTNaMEAxPjA8BgNVBAMTNUFERlMgU2lnbmluZyAtIHN0cy5zdWIyLmZyYWNhczM2NS5tc2Z0b25saW5lcmVwcm8uY29tMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAxdjzB+wGV6hYekOvWwKoL/DFNBiLQsLx6w02FzcFnpGwR38gVTn/glg9CNSsOT0riRM3/MwU8o2fwseQyVtv9Kee/yvia8cB6GD0CARlYizb6GkJJzMvWkPSas1zpn10Bs3SBBgn0pvAKZCWWir5WJ7DRY32X2yo2do8mQftsoLGYsEU8+jj9AMYQWaR3A86AEWjXoQY3AodfMMzdVFX+O/kjsvKcBfPqGRT6jUSGBOOaqzMOJBT39SueD8zePDW7SejBl7fRi4TLx5H6xuMldOAAH6oD70yIrobqosGG9X/LdijHajMSoaYzZIlG7fl4PCVvAjh1Dytw/y8K70flQIDAQABMA0GCSqGSIb3DQEBCwUAA4IBAQBy08dAsSTKd2YlNE8bM4O5C2bFKR1YakR8L/zLEy8g+RNsKN5V/cIll0b/tf9iQ5464nc+nM///U+UVxqT8ipeR7ThIPwuWX98cFZFQNNGkha4PaYap/osquEpRAJOcTqZf2K95ipeQ+5Hhw00mK0hcV1QT/7maTUqCHDfBCaD+uYAFvaNBXOYpdoIGM9cMk7Qjc/yowLDm+DpmJek54MWmN+iZ0YtDEhMSh//QPFMLPT5Ucat+qRTen1HZNGdxfZ7NIIDL3dNKVDN+vDUbW7rjvPyxA8Rtj4JplI9ENqpzRq4m1sDWUTk2hJYw9Ec1kGo7AFKRmOS6DRbwUn5Ptdc</X509Certificate>

            var cert = CertificateHelper.LoadX509Certificate("MIIDCDCCAfCgAwIBAgIQNz4YVbYAIJVFCc47HFD3RzANBgkqhkiG9w0BAQsFADBAMT4wPAYDVQQDEzVBREZTIFNpZ25pbmcgLSBzdHMuc3ViMi5mcmFjYXMzNjUubXNmdG9ubGluZXJlcHJvLmNvbTAeFw0xNTAzMzExMDQyMTNaFw0zNDA1MzAxMDQyMTNaMEAxPjA8BgNVBAMTNUFERlMgU2lnbmluZyAtIHN0cy5zdWIyLmZyYWNhczM2NS5tc2Z0b25saW5lcmVwcm8uY29tMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAxdjzB+wGV6hYekOvWwKoL/DFNBiLQsLx6w02FzcFnpGwR38gVTn/glg9CNSsOT0riRM3/MwU8o2fwseQyVtv9Kee/yvia8cB6GD0CARlYizb6GkJJzMvWkPSas1zpn10Bs3SBBgn0pvAKZCWWir5WJ7DRY32X2yo2do8mQftsoLGYsEU8+jj9AMYQWaR3A86AEWjXoQY3AodfMMzdVFX+O/kjsvKcBfPqGRT6jUSGBOOaqzMOJBT39SueD8zePDW7SejBl7fRi4TLx5H6xuMldOAAH6oD70yIrobqosGG9X/LdijHajMSoaYzZIlG7fl4PCVvAjh1Dytw/y8K70flQIDAQABMA0GCSqGSIb3DQEBCwUAA4IBAQBy08dAsSTKd2YlNE8bM4O5C2bFKR1YakR8L/zLEy8g+RNsKN5V/cIll0b/tf9iQ5464nc+nM///U+UVxqT8ipeR7ThIPwuWX98cFZFQNNGkha4PaYap/osquEpRAJOcTqZf2K95ipeQ+5Hhw00mK0hcV1QT/7maTUqCHDfBCaD+uYAFvaNBXOYpdoIGM9cMk7Qjc/yowLDm+DpmJek54MWmN+iZ0YtDEhMSh//QPFMLPT5Ucat+qRTen1HZNGdxfZ7NIIDL3dNKVDN+vDUbW7rjvPyxA8Rtj4JplI9ENqpzRq4m1sDWUTk2hJYw9Ec1kGo7AFKRmOS6DRbwUn5Ptdc");
            try
            {
                envelopedReader.Signature.Verify(new X509SecurityKey(cert), CryptoProviderFactory.Default);
            }
            catch
            {

            }
            var saml2 = @"<saml:Assertion MajorVersion=""1"" MinorVersion=""1"" AssertionID=""_e35fc6da-147e-428e-8c71-fb32867598ab"" Issuer=""http://sts.sub2.fracas365.msftonlinerepro.com/adfs/services/trust"" IssueInstant=""2017-05-08T14:57:58.348Z"" xmlns:saml=""urn:oasis:names:tc:SAML:1.0:assertion""><saml:Conditions NotBefore=""2017-05-08T14:57:58.348Z"" NotOnOrAfter=""2017-05-08T15:57:58.348Z""><saml:AudienceRestrictionCondition><saml:Audience>https://app1.sub2.fracas365.msftonlinerepro.com/sampapp/</saml:Audience></saml:AudienceRestrictionCondition></saml:Conditions><saml:AttributeStatement><saml:Subject><saml:SubjectConfirmation><saml:ConfirmationMethod>urn:oasis:names:tc:SAML:1.0:cm:bearer</saml:ConfirmationMethod></saml:SubjectConfirmation></saml:Subject><saml:Attribute AttributeName=""upn"" AttributeNamespace=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims""><saml:AttributeValue>killer@sub2.fracas365.msftonlinerepro.com</saml:AttributeValue></saml:Attribute><saml:Attribute AttributeName=""x-ms-endpoint-absolute-path"" AttributeNamespace=""http://schemas.microsoft.com/2012/01/requestcontext/claims"" a:OriginalIssuer=""CLIENT CONTEXT"" xmlns:a=""http://schemas.xmlsoap.org/ws/2009/09/identity/claims""><saml:AttributeValue>/adfs/ls/wia</saml:AttributeValue></saml:Attribute><saml:Attribute AttributeName=""x-ms-client-ip"" AttributeNamespace=""http://schemas.microsoft.com/2012/01/requestcontext/claims"" a:OriginalIssuer=""CLIENT CONTEXT"" xmlns:a=""http://schemas.xmlsoap.org/ws/2009/09/identity/claims""><saml:AttributeValue>172.15.0.67</saml:AttributeValue></saml:Attribute><saml:Attribute AttributeName=""primarygroupsid"" AttributeNamespace=""http://schemas.microsoft.com/ws/2008/06/identity/claims""><saml:AttributeValue>S-1-5-21-487734988-61580006-1080473273-513</saml:AttributeValue></saml:Attribute><saml:Attribute AttributeName=""authnmethodsreferences"" AttributeNamespace=""http://schemas.microsoft.com/claims""><saml:AttributeValue>http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/windows</saml:AttributeValue></saml:Attribute><saml:Attribute AttributeName=""windowsaccountname"" AttributeNamespace=""http://schemas.microsoft.com/ws/2008/06/identity/claims""><saml:AttributeValue>FRACAS-O365\killer</saml:AttributeValue></saml:Attribute><saml:Attribute AttributeName=""streetAddress"" AttributeNamespace=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims""><saml:AttributeValue>street&#xD;&#xA;with&#xD;&#xA;return</saml:AttributeValue></saml:Attribute></saml:AttributeStatement><saml:AuthenticationStatement AuthenticationMethod=""urn:federation:authentication:windows"" AuthenticationInstant=""2017-05-08T14:57:58.333Z""><saml:Subject><saml:SubjectConfirmation><saml:ConfirmationMethod>urn:oasis:names:tc:SAML:1.0:cm:bearer</saml:ConfirmationMethod></saml:SubjectConfirmation></saml:Subject></saml:AuthenticationStatement><ds:Signature xmlns:ds=""http://www.w3.org/2000/09/xmldsig#""><ds:SignedInfo><ds:CanonicalizationMethod Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#"" /><ds:SignatureMethod Algorithm=""http://www.w3.org/2001/04/xmldsig-more#rsa-sha256"" /><ds:Reference URI=""#_e35fc6da-147e-428e-8c71-fb32867598ab""><ds:Transforms><ds:Transform Algorithm=""http://www.w3.org/2000/09/xmldsig#enveloped-signature"" /><ds:Transform Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#"" /></ds:Transforms><ds:DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256"" /><ds:DigestValue>F/TMfVx/lEtqy4aGBzMyKPj/b5iyaEH9WyhUJ1EA724=</ds:DigestValue></ds:Reference></ds:SignedInfo><ds:SignatureValue>RY27lz0BoNkWK+67gBdIFaLb2EzIcT3uKq+UTQhg1bRv6TewJScnEkhYCA4qqKEzjWgGBnkBsDfeZ44qLwvi5h1Q4S/cmY2i9eOeZnb63BosXSEzLLkhV4wT2sWy9og5EMB3IGAH5W/qjoPJybO8CMrHpwRC5YR81KsXO0O+8n2U/tih9vHd4ddxnJ1upTopcAhs5jYLVQc1pqtCZxA0EGsrpQaQFUGDggc/bhihCe4p87ppBN8CRE/zfKQYjRR1UL4dvQLDRNE+b+aeE5TIDomekgY7U2ai6NQYkJm+8lhz9824rJp8HqeMZ/77VsbpnH5i7OcOaXhXWIBBmE86eQ==</ds:SignatureValue><KeyInfo xmlns=""http://www.w3.org/2000/09/xmldsig#""><X509Data><X509Certificate>MIIDCDCCAfCgAwIBAgIQNz4YVbYAIJVFCc47HFD3RzANBgkqhkiG9w0BAQsFADBAMT4wPAYDVQQDEzVBREZTIFNpZ25pbmcgLSBzdHMuc3ViMi5mcmFjYXMzNjUubXNmdG9ubGluZXJlcHJvLmNvbTAeFw0xNTAzMzExMDQyMTNaFw0zNDA1MzAxMDQyMTNaMEAxPjA8BgNVBAMTNUFERlMgU2lnbmluZyAtIHN0cy5zdWIyLmZyYWNhczM2NS5tc2Z0b25saW5lcmVwcm8uY29tMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAxdjzB+wGV6hYekOvWwKoL/DFNBiLQsLx6w02FzcFnpGwR38gVTn/glg9CNSsOT0riRM3/MwU8o2fwseQyVtv9Kee/yvia8cB6GD0CARlYizb6GkJJzMvWkPSas1zpn10Bs3SBBgn0pvAKZCWWir5WJ7DRY32X2yo2do8mQftsoLGYsEU8+jj9AMYQWaR3A86AEWjXoQY3AodfMMzdVFX+O/kjsvKcBfPqGRT6jUSGBOOaqzMOJBT39SueD8zePDW7SejBl7fRi4TLx5H6xuMldOAAH6oD70yIrobqosGG9X/LdijHajMSoaYzZIlG7fl4PCVvAjh1Dytw/y8K70flQIDAQABMA0GCSqGSIb3DQEBCwUAA4IBAQBy08dAsSTKd2YlNE8bM4O5C2bFKR1YakR8L/zLEy8g+RNsKN5V/cIll0b/tf9iQ5464nc+nM///U+UVxqT8ipeR7ThIPwuWX98cFZFQNNGkha4PaYap/osquEpRAJOcTqZf2K95ipeQ+5Hhw00mK0hcV1QT/7maTUqCHDfBCaD+uYAFvaNBXOYpdoIGM9cMk7Qjc/yowLDm+DpmJek54MWmN+iZ0YtDEhMSh//QPFMLPT5Ucat+qRTen1HZNGdxfZ7NIIDL3dNKVDN+vDUbW7rjvPyxA8Rtj4JplI9ENqpzRq4m1sDWUTk2hJYw9Ec1kGo7AFKRmOS6DRbwUn5Ptdc</X509Certificate></X509Data></KeyInfo></ds:Signature></saml:Assertion>";
            //var saml2 = @"<saml:Assertion MajorVersion=""1"" MinorVersion=""1"" AssertionID=""_e35fc6da-147e-428e-8c71-fb32867598ab"" Issuer=""http://sts.sub2.fracas365.msftonlinerepro.com/adfs/services/trust"" IssueInstant=""2017-05-08T14:57:58.348Z"" xmlns:saml=""urn:oasis:names:tc:SAML:1.0:assertion""><saml:Conditions NotBefore=""2017-05-08T14:57:58.348Z"" NotOnOrAfter=""2017-05-08T15:57:58.348Z""><saml:AudienceRestrictionCondition><saml:Audience>https://app1.sub2.fracas365.msftonlinerepro.com/sampapp/</saml:Audience></saml:AudienceRestrictionCondition></saml:Conditions><saml:AttributeStatement><saml:Subject><saml:SubjectConfirmation><saml:ConfirmationMethod>urn:oasis:names:tc:SAML:1.0:cm:bearer</saml:ConfirmationMethod></saml:SubjectConfirmation></saml:Subject><saml:Attribute AttributeName=""upn"" AttributeNamespace=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims""><saml:AttributeValue>killer@sub2.fracas365.msftonlinerepro.com</saml:AttributeValue></saml:Attribute><saml:Attribute AttributeName=""x-ms-endpoint-absolute-path"" AttributeNamespace=""http://schemas.microsoft.com/2012/01/requestcontext/claims"" a:OriginalIssuer=""CLIENT CONTEXT"" xmlns:a=""http://schemas.xmlsoap.org/ws/2009/09/identity/claims""><saml:AttributeValue>/adfs/ls/wia</saml:AttributeValue></saml:Attribute><saml:Attribute AttributeName=""x-ms-client-ip"" AttributeNamespace=""http://schemas.microsoft.com/2012/01/requestcontext/claims"" a:OriginalIssuer=""CLIENT CONTEXT"" xmlns:a=""http://schemas.xmlsoap.org/ws/2009/09/identity/claims""><saml:AttributeValue>172.15.0.67</saml:AttributeValue></saml:Attribute><saml:Attribute AttributeName=""primarygroupsid"" AttributeNamespace=""http://schemas.microsoft.com/ws/2008/06/identity/claims""><saml:AttributeValue>S-1-5-21-487734988-61580006-1080473273-513</saml:AttributeValue></saml:Attribute><saml:Attribute AttributeName=""authnmethodsreferences"" AttributeNamespace=""http://schemas.microsoft.com/claims""><saml:AttributeValue>http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/windows</saml:AttributeValue></saml:Attribute><saml:Attribute AttributeName=""windowsaccountname"" AttributeNamespace=""http://schemas.microsoft.com/ws/2008/06/identity/claims""><saml:AttributeValue>FRACAS-O365\killer</saml:AttributeValue></saml:Attribute><saml:Attribute AttributeName=""streetAddress"" AttributeNamespace=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims""><saml:AttributeValue>street&#xD;\nwith&#xD;\nreturn</saml:AttributeValue></saml:Attribute></saml:AttributeStatement><saml:AuthenticationStatement AuthenticationMethod=""urn:federation:authentication:windows"" AuthenticationInstant=""2017-05-08T14:57:58.333Z""><saml:Subject><saml:SubjectConfirmation><saml:ConfirmationMethod>urn:oasis:names:tc:SAML:1.0:cm:bearer</saml:ConfirmationMethod></saml:SubjectConfirmation></saml:Subject></saml:AuthenticationStatement><ds:Signature xmlns:ds=""http://www.w3.org/2000/09/xmldsig#""><ds:SignedInfo><ds:CanonicalizationMethod Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#"" /><ds:SignatureMethod Algorithm=""http://www.w3.org/2001/04/xmldsig-more#rsa-sha256"" /><ds:Reference URI=""#_e35fc6da-147e-428e-8c71-fb32867598ab""><ds:Transforms><ds:Transform Algorithm=""http://www.w3.org/2000/09/xmldsig#enveloped-signature"" /><ds:Transform Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#"" /></ds:Transforms><ds:DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256"" /><ds:DigestValue>F/TMfVx/lEtqy4aGBzMyKPj/b5iyaEH9WyhUJ1EA724=</ds:DigestValue></ds:Reference></ds:SignedInfo><ds:SignatureValue>RY27lz0BoNkWK+67gBdIFaLb2EzIcT3uKq+UTQhg1bRv6TewJScnEkhYCA4qqKEzjWgGBnkBsDfeZ44qLwvi5h1Q4S/cmY2i9eOeZnb63BosXSEzLLkhV4wT2sWy9og5EMB3IGAH5W/qjoPJybO8CMrHpwRC5YR81KsXO0O+8n2U/tih9vHd4ddxnJ1upTopcAhs5jYLVQc1pqtCZxA0EGsrpQaQFUGDggc/bhihCe4p87ppBN8CRE/zfKQYjRR1UL4dvQLDRNE+b+aeE5TIDomekgY7U2ai6NQYkJm+8lhz9824rJp8HqeMZ/77VsbpnH5i7OcOaXhXWIBBmE86eQ==</ds:SignatureValue><KeyInfo xmlns=""http://www.w3.org/2000/09/xmldsig#""><X509Data><X509Certificate>MIIDCDCCAfCgAwIBAgIQNz4YVbYAIJVFCc47HFD3RzANBgkqhkiG9w0BAQsFADBAMT4wPAYDVQQDEzVBREZTIFNpZ25pbmcgLSBzdHMuc3ViMi5mcmFjYXMzNjUubXNmdG9ubGluZXJlcHJvLmNvbTAeFw0xNTAzMzExMDQyMTNaFw0zNDA1MzAxMDQyMTNaMEAxPjA8BgNVBAMTNUFERlMgU2lnbmluZyAtIHN0cy5zdWIyLmZyYWNhczM2NS5tc2Z0b25saW5lcmVwcm8uY29tMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAxdjzB+wGV6hYekOvWwKoL/DFNBiLQsLx6w02FzcFnpGwR38gVTn/glg9CNSsOT0riRM3/MwU8o2fwseQyVtv9Kee/yvia8cB6GD0CARlYizb6GkJJzMvWkPSas1zpn10Bs3SBBgn0pvAKZCWWir5WJ7DRY32X2yo2do8mQftsoLGYsEU8+jj9AMYQWaR3A86AEWjXoQY3AodfMMzdVFX+O/kjsvKcBfPqGRT6jUSGBOOaqzMOJBT39SueD8zePDW7SejBl7fRi4TLx5H6xuMldOAAH6oD70yIrobqosGG9X/LdijHajMSoaYzZIlG7fl4PCVvAjh1Dytw/y8K70flQIDAQABMA0GCSqGSIb3DQEBCwUAA4IBAQBy08dAsSTKd2YlNE8bM4O5C2bFKR1YakR8L/zLEy8g+RNsKN5V/cIll0b/tf9iQ5464nc+nM///U+UVxqT8ipeR7ThIPwuWX98cFZFQNNGkha4PaYap/osquEpRAJOcTqZf2K95ipeQ+5Hhw00mK0hcV1QT/7maTUqCHDfBCaD+uYAFvaNBXOYpdoIGM9cMk7Qjc/yowLDm+DpmJek54MWmN+iZ0YtDEhMSh//QPFMLPT5Ucat+qRTen1HZNGdxfZ7NIIDL3dNKVDN+vDUbW7rjvPyxA8Rtj4JplI9ENqpzRq4m1sDWUTk2hJYw9Ec1kGo7AFKRmOS6DRbwUn5Ptdc</X509Certificate></X509Data></KeyInfo></ds:Signature></saml:Assertion>";
            envelopedReader = XmlUtilities.CreateEnvelopedSignatureReader(saml2);
            var doc2 = new XmlDocument();
            doc2.Load(XmlReader.Create(new System.IO.StringReader(saml2), new XmlReaderSettings() { XmlResolver = null }));
            var doc1 = new XmlDocument();
            doc1.Load(XmlReader.Create(new System.IO.StringReader(saml1), new XmlReaderSettings() { XmlResolver = null }));
            while (envelopedReader.Read()) ;
            try
            {
                envelopedReader.Signature.Verify(new X509SecurityKey(cert), CryptoProviderFactory.Default);
            }
            catch
            {

            }

            //MIIDCDCCAfCgAwIBAgIQNz4YVbYAIJVFCc47HFD3RzANBgkqhkiG9w0BAQsFADBAMT4wPAYDVQQDEzVBREZTIFNpZ25pbmcgLSBzdHMuc3ViMi5mcmFjYXMzNjUubXNmdG9ubGluZXJlcHJvLmNvbTAeFw0xNTAzMzExMDQyMTNaFw0zNDA1MzAxMDQyMTNaMEAxPjA8BgNVBAMTNUFERlMgU2lnbmluZyAtIHN0cy5zdWIyLmZyYWNhczM2NS5tc2Z0b25saW5lcmVwcm8uY29tMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAxdjzB+wGV6hYekOvWwKoL/DFNBiLQsLx6w02FzcFnpGwR38gVTn/glg9CNSsOT0riRM3/MwU8o2fwseQyVtv9Kee/yvia8cB6GD0CARlYizb6GkJJzMvWkPSas1zpn10Bs3SBBgn0pvAKZCWWir5WJ7DRY32X2yo2do8mQftsoLGYsEU8+jj9AMYQWaR3A86AEWjXoQY3AodfMMzdVFX+O/kjsvKcBfPqGRT6jUSGBOOaqzMOJBT39SueD8zePDW7SejBl7fRi4TLx5H6xuMldOAAH6oD70yIrobqosGG9X/LdijHajMSoaYzZIlG7fl4PCVvAjh1Dytw/y8K70flQIDAQABMA0GCSqGSIb3DQEBCwUAA4IBAQBy08dAsSTKd2YlNE8bM4O5C2bFKR1YakR8L/zLEy8g+RNsKN5V/cIll0b/tf9iQ5464nc+nM///U+UVxqT8ipeR7ThIPwuWX98cFZFQNNGkha4PaYap/osquEpRAJOcTqZf2K95ipeQ+5Hhw00mK0hcV1QT/7maTUqCHDfBCaD+uYAFvaNBXOYpdoIGM9cMk7Qjc/yowLDm+DpmJek54MWmN+iZ0YtDEhMSh//QPFMLPT5Ucat+qRTen1HZNGdxfZ7NIIDL3dNKVDN+vDUbW7rjvPyxA8Rtj4JplI9ENqpzRq4m1sDWUTk2hJYw9Ec1kGo7AFKRmOS6DRbwUn5Ptdc
        }

        [Theory, MemberData(nameof(VerifyTheoryData), DisableDiscoveryEnumeration = true)]
        public void Verify(ReferenceTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.Verify", theoryData);
            var context = new CompareContext($"{this}.Verify, {theoryData.TestId}");
            try
            {
                theoryData.Reference.Verify(theoryData.ProviderFactory);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ReferenceTheoryData> VerifyTheoryData
        {
            get
            {
                // uncomment to view exception displayed to user
                ExpectedException.DefaultVerbose = true;

                return new TheoryData<ReferenceTheoryData>()
                {
                    new ReferenceTheoryData
                    {
                        ExpectedException = ExpectedException.NoExceptionExpected,
                        ProviderFactory = CryptoProviderFactory.Default,
                        Reference = Default.Reference,
                        TestId = "Successful verification"
                    },
                    new ReferenceTheoryData
                    {
                        ExpectedException = ExpectedException.NoExceptionExpected,
                        ProviderFactory = CryptoProviderFactory.Default,
                        Reference = Default.ReferenceWithOnlyCanonicalizingTransform,
                        TestId = "Successful verification with only canonicalizing transform"
                    },
                    new ReferenceTheoryData
                    {
                        ExpectedException = ExpectedException.NoExceptionExpected,
                        ProviderFactory = CryptoProviderFactory.Default,
                        Reference = Default.ReferenceWithoutTransform,
                        TestId = "Successful verification without transforms"
                    },
                    new ReferenceTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "CryptoProviderFactory == null"
                    },
                    new ReferenceTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(XmlValidationException), "IDX30202"),
                        ProviderFactory = CryptoProviderFactory.Default,
                        Reference = Default.ReferenceWithNullTokenStreamNS,
                        TestId = "XmlTokenStream == null"
                    },
                    new ReferenceTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(XmlValidationException), "IDX30208"),
                        ProviderFactory = new CustomCryptoProviderFactory(),
                        Reference = Default.ReferenceNS,
                        TestId = "DigestMethod Not Supported"
                    },
                    new ReferenceTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(XmlValidationException), "IDX30209"),
                        ProviderFactory = new CustomCryptoProviderFactory
                        {
                            SupportedAlgorithms = new List<string>{Default.ReferenceDigestMethod}
                        },
                        Reference = Default.ReferenceNS,
                        TestId = "CryptoProviderFactory returns null HashAlgorithm"
                    }
                };
            }
        }
    }

    public class ReferenceTheoryData : TheoryDataBase
    {
        public CryptoProviderFactory ProviderFactory
        {
            get;
            set;
        }

        public string DigestMethod
        {
            get;
            set;
        }

        public string DigestValue
        {
            get;
            set;
        }

        public Reference Reference
        {
            get;
            set;
        } = new Reference();

        public IEnumerable<string> Transforms
        {
            get;
            set;
        }

        public string Xml
        {
            get;
            set;
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
