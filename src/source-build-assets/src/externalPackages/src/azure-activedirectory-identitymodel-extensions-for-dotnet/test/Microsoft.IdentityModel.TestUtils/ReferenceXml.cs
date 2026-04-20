// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.Protocols.WsFederation;

namespace Microsoft.IdentityModel.TestUtils
{
    public static class ReferenceXml
    {
        #region EnvelopedSignatureReader / Writer
        #endregion

        #region EnvelopedSignatureTransform
        #endregion

        #region ExclusiveCanonicalizationTransform
        #endregion

        #region Wresult

        public static string WresultSaml2Valid
        {
            get => @"<t:RequestSecurityTokenResponse xmlns:t=""http://schemas.xmlsoap.org/ws/2005/02/trust""><t:Lifetime><wsu:Created xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">2017-04-23T16:11:17.348Z</wsu:Created><wsu:Expires xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">2017-04-23T17:11:17.348Z</wsu:Expires></t:Lifetime><wsp:AppliesTo xmlns:wsp=""http://schemas.xmlsoap.org/ws/2004/09/policy""><wsa:EndpointReference xmlns:wsa=""http://www.w3.org/2005/08/addressing""><wsa:Address>spn:fe78e0b4-6fe7-47e6-812c-fb75cee266a4</wsa:Address></wsa:EndpointReference></wsp:AppliesTo><t:RequestedSecurityToken><Assertion ID=""_edc15efd-1117-4bf9-89da-28b1663fb890"" IssueInstant=""2017-04-23T16:16:17.348Z"" Version=""2.0"" xmlns=""urn:oasis:names:tc:SAML:2.0:assertion""><Issuer>https://sts.windows.net/add29489-7269-41f4-8841-b63c95564420/</Issuer><Signature xmlns=""http://www.w3.org/2000/09/xmldsig#""><SignedInfo><CanonicalizationMethod Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#""/><SignatureMethod Algorithm=""http://www.w3.org/2001/04/xmldsig-more#rsa-sha256""/><Reference URI=""#_edc15efd-1117-4bf9-89da-28b1663fb890""><Transforms><Transform Algorithm=""http://www.w3.org/2000/09/xmldsig#enveloped-signature""/><Transform Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#""/></Transforms><DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256""/><DigestValue>DO8QQoO629ApWPV3LiY2epQSv+I82iChybeRrXbhgtw=</DigestValue></Reference></SignedInfo><SignatureValue>O8JNyVKm9I7kMqlsaBgLCNwHA0qdXv34YHBVfg217lgeKkMC5taLU/EH7UeeMtapU6zMafcYoCH+Bp9zoqDpflgs78Hkjgn/dEUtjPFn7211VXClcTNqk+yhqXWtu6SKrabeIhKCKtoMA9lUAB4D6ABesb6MpwbM/ULq7T16tycZ3X//iXHeOiMwNiUAePYF22fmgrqRSDRHyLPtiLskP4UMksWJBrXUV96e9EU9aEciCvYpzMDv/VFUOCLiEkBqCdAtPVwVun+5eRk9zEh6qscWi0kAgFl3W3JhugcTTuGQYHXYVIHxbd5O33MwFIMUOmGrI1EXuk+cHIq2KUtSLg==</SignatureValue><KeyInfo><X509Data><X509Certificate>MIIDBTCCAe2gAwIBAgIQY4RNIR0dX6dBZggnkhCRoDANBgkqhkiG9w0BAQsFADAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MB4XDTE3MDIxMzAwMDAwMFoXDTE5MDIxNDAwMDAwMFowLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldDCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAMBEizU1OJms31S/ry7iav/IICYVtQ2MRPhHhYknHImtU03sgVk1Xxub4GD7R15i9UWIGbzYSGKaUtGU9lP55wrfLpDjQjEgaXi4fE6mcZBwa9qc22is23B6R67KMcVyxyDWei+IP3sKmCcMX7Ibsg+ubZUpvKGxXZ27YgqFTPqCT2znD7K81YKfy+SVg3uW6epW114yZzClTQlarptYuE2mujxjZtx7ZUlwc9AhVi8CeiLwGO1wzTmpd/uctpner6oc335rvdJikNmc1cFKCK+2irew1bgUJHuN+LJA0y5iVXKvojiKZ2Ii7QKXn19Ssg1FoJ3x2NWA06wc0CnruLsCAwEAAaMhMB8wHQYDVR0OBBYEFDAr/HCMaGqmcDJa5oualVdWAEBEMA0GCSqGSIb3DQEBCwUAA4IBAQAiUke5mA86R/X4visjceUlv5jVzCn/SIq6Gm9/wCqtSxYvifRXxwNpQTOyvHhrY/IJLRUp2g9/fDELYd65t9Dp+N8SznhfB6/Cl7P7FRo99rIlj/q7JXa8UB/vLJPDlr+NREvAkMwUs1sDhL3kSuNBoxrbLC5Jo4es+juQLXd9HcRraE4U3UZVhUS2xqjFOfaGsCbJEqqkjihssruofaxdKT1CPzPMANfREFJznNzkpJt4H0aMDgVzq69NxZ7t1JiIuc43xRjeiixQMRGMi1mAB75fTyfFJ/rWQ5J/9kh0HMZVtHsqICBF1tHMTMIK5rwoweY0cuCIpN7A/zMOQtoD</X509Certificate></X509Data></KeyInfo></Signature><Subject><NameID Format=""urn:oasis:names:tc:SAML:2.0:nameid-format:persistent"">RrX3SPSxDw6z4KHaKB2V_mnv0G-LbRZdYvo1RQa1L7s</NameID><SubjectConfirmation Method=""urn:oasis:names:tc:SAML:2.0:cm:bearer""/></Subject><Conditions NotBefore=""2017-04-23T16:11:17.348Z"" NotOnOrAfter=""2017-04-23T17:11:17.348Z""><AudienceRestriction><Audience>spn:fe78e0b4-6fe7-47e6-812c-fb75cee266a4</Audience></AudienceRestriction></Conditions><AttributeStatement><Attribute Name=""http://schemas.microsoft.com/identity/claims/tenantid""><AttributeValue>add29489-7269-41f4-8841-b63c95564420</AttributeValue></Attribute><Attribute Name=""http://schemas.microsoft.com/identity/claims/objectidentifier""><AttributeValue>d1ad9ce7-b322-4221-ab74-1e1011e1bbcb</AttributeValue></Attribute><Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name""><AttributeValue>User1@Cyrano.onmicrosoft.com</AttributeValue></Attribute><Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname""><AttributeValue>1</AttributeValue></Attribute><Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname""><AttributeValue>User</AttributeValue></Attribute><Attribute Name=""http://schemas.microsoft.com/identity/claims/displayname""><AttributeValue>User1</AttributeValue></Attribute><Attribute Name=""http://schemas.microsoft.com/identity/claims/identityprovider""><AttributeValue>https://sts.windows.net/add29489-7269-41f4-8841-b63c95564420/</AttributeValue></Attribute><Attribute Name=""http://schemas.microsoft.com/claims/authnmethodsreferences""><AttributeValue>http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/password</AttributeValue></Attribute></AttributeStatement><AuthnStatement AuthnInstant=""2017-04-23T16:16:17.270Z""><AuthnContext><AuthnContextClassRef>urn:oasis:names:tc:SAML:2.0:ac:classes:Password</AuthnContextClassRef></AuthnContext></AuthnStatement></Assertion></t:RequestedSecurityToken><t:RequestedAttachedReference><SecurityTokenReference d3p1:TokenType=""http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV2.0"" xmlns:d3p1=""http://docs.oasis-open.org/wss/oasis-wss-wssecurity-secext-1.1.xsd"" xmlns=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd""><KeyIdentifier ValueType=""http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLID"">_edc15efd-1117-4bf9-89da-28b1663fb890</KeyIdentifier></SecurityTokenReference></t:RequestedAttachedReference><t:RequestedUnattachedReference><SecurityTokenReference d3p1:TokenType=""http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV2.0"" xmlns:d3p1=""http://docs.oasis-open.org/wss/oasis-wss-wssecurity-secext-1.1.xsd"" xmlns=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd""><KeyIdentifier ValueType=""http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLID"">_edc15efd-1117-4bf9-89da-28b1663fb890</KeyIdentifier></SecurityTokenReference></t:RequestedUnattachedReference><t:TokenType>http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV2.0</t:TokenType><t:RequestType>http://schemas.xmlsoap.org/ws/2005/02/trust/Issue</t:RequestType><t:KeyType>http://schemas.xmlsoap.org/ws/2005/05/identity/NoProofKey</t:KeyType></t:RequestSecurityTokenResponse>";
        }

        public static string WresultSaml2ValidFormated
        {
            get
            {
                return @"<t:RequestSecurityTokenResponse xmlns:t=""http://schemas.xmlsoap.org/ws/2005/02/trust"">
                            <t:Lifetime>
                                <wsu:Created xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">2017-04-23T16:11:17.348Z</wsu:Created>
                                <wsu:Expires xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">2017-04-23T17:11:17.348Z</wsu:Expires>
                            </t:Lifetime>
                            <wsp:AppliesTo xmlns:wsp=""http://schemas.xmlsoap.org/ws/2004/09/policy"">
                                <wsa:EndpointReference xmlns:wsa=""http://www.w3.org/2005/08/addressing"">
                                <wsa:Address>spn:fe78e0b4-6fe7-47e6-812c-fb75cee266a4</wsa:Address></wsa:EndpointReference>
                            </wsp:AppliesTo>
                            <t:RequestedSecurityToken>
                                <Assertion ID=""_edc15efd-1117-4bf9-89da-28b1663fb890"" IssueInstant=""2017-04-23T16:16:17.348Z"" Version=""2.0"" xmlns=""urn:oasis:names:tc:SAML:2.0:assertion"">
                                    <Issuer>https://sts.windows.net/add29489-7269-41f4-8841-b63c95564420/</Issuer>
                                    <Signature xmlns=""http://www.w3.org/2000/09/xmldsig#"">
                                        <SignedInfo>
                                            <CanonicalizationMethod Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#"" />
                                            <SignatureMethod Algorithm=""http://www.w3.org/2001/04/xmldsig-more#rsa-sha256"" />
                                            <Reference URI=""#_edc15efd-1117-4bf9-89da-28b1663fb890"">
                                                <Transforms>
                                                    <Transform Algorithm=""http://www.w3.org/2000/09/xmldsig#enveloped-signature"" />
                                                    <Transform Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#"" />
                                                </Transforms><DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256"" />
                                                <DigestValue>DO8QQoO629ApWPV3LiY2epQSv+I82iChybeRrXbhgtw=</DigestValue>
                                            </Reference>
                                        </SignedInfo>
                                        <SignatureValue>O8JNyVKm9I7kMqlsaBgLCNwHA0qdXv34YHBVfg217lgeKkMC5taLU/EH7UeeMtapU6zMafcYoCH+Bp9zoqDpflgs78Hkjgn/dEUtjPFn7211VXClcTNqk+yhqXWtu6SKrabeIhKCKtoMA9lUAB4D6ABesb6MpwbM/ULq7T16tycZ3X//iXHeOiMwNiUAePYF22fmgrqRSDRHyLPtiLskP4UMksWJBrXUV96e9EU9aEciCvYpzMDv/VFUOCLiEkBqCdAtPVwVun+5eRk9zEh6qscWi0kAgFl3W3JhugcTTuGQYHXYVIHxbd5O33MwFIMUOmGrI1EXuk+cHIq2KUtSLg==</SignatureValue>
                                        <KeyInfo>
                                            <X509Data>
                                                <X509Certificate>MIIDBTCCAe2gAwIBAgIQY4RNIR0dX6dBZggnkhCRoDANBgkqhkiG9w0BAQsFADAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MB4XDTE3MDIxMzAwMDAwMFoXDTE5MDIxNDAwMDAwMFowLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldDCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAMBEizU1OJms31S/ry7iav/IICYVtQ2MRPhHhYknHImtU03sgVk1Xxub4GD7R15i9UWIGbzYSGKaUtGU9lP55wrfLpDjQjEgaXi4fE6mcZBwa9qc22is23B6R67KMcVyxyDWei+IP3sKmCcMX7Ibsg+ubZUpvKGxXZ27YgqFTPqCT2znD7K81YKfy+SVg3uW6epW114yZzClTQlarptYuE2mujxjZtx7ZUlwc9AhVi8CeiLwGO1wzTmpd/uctpner6oc335rvdJikNmc1cFKCK+2irew1bgUJHuN+LJA0y5iVXKvojiKZ2Ii7QKXn19Ssg1FoJ3x2NWA06wc0CnruLsCAwEAAaMhMB8wHQYDVR0OBBYEFDAr/HCMaGqmcDJa5oualVdWAEBEMA0GCSqGSIb3DQEBCwUAA4IBAQAiUke5mA86R/X4visjceUlv5jVzCn/SIq6Gm9/wCqtSxYvifRXxwNpQTOyvHhrY/IJLRUp2g9/fDELYd65t9Dp+N8SznhfB6/Cl7P7FRo99rIlj/q7JXa8UB/vLJPDlr+NREvAkMwUs1sDhL3kSuNBoxrbLC5Jo4es+juQLXd9HcRraE4U3UZVhUS2xqjFOfaGsCbJEqqkjihssruofaxdKT1CPzPMANfREFJznNzkpJt4H0aMDgVzq69NxZ7t1JiIuc43xRjeiixQMRGMi1mAB75fTyfFJ/rWQ5J/9kh0HMZVtHsqICBF1tHMTMIK5rwoweY0cuCIpN7A/zMOQtoD</X509Certificate>
                                            </X509Data>
                                        </KeyInfo>
                                    </Signature>
                                    <Subject>
                                        <NameID Format=""urn:oasis:names:tc:SAML:2.0:nameid-format:persistent"">RrX3SPSxDw6z4KHaKB2V_mnv0G-LbRZdYvo1RQa1L7s</NameID>
                                        <SubjectConfirmation Method=""urn:oasis:names:tc:SAML:2.0:cm:bearer"" />
                                    </Subject>
                                    <Conditions NotBefore=""2017-04-23T16:11:17.348Z"" NotOnOrAfter=""2017-04-23T17:11:17.348Z"">
                                        <AudienceRestriction>
                                            <Audience>spn:fe78e0b4-6fe7-47e6-812c-fb75cee266a4</Audience>
                                        </AudienceRestriction>
                                    </Conditions>
                                    <AttributeStatement>
                                        <Attribute Name=""http://schemas.microsoft.com/identity/claims/tenantid""><AttributeValue>add29489-7269-41f4-8841-b63c95564420</AttributeValue></Attribute>
                                        <Attribute Name=""http://schemas.microsoft.com/identity/claims/objectidentifier""><AttributeValue>d1ad9ce7-b322-4221-ab74-1e1011e1bbcb</AttributeValue></Attribute>
                                        <Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name""><AttributeValue>User1@Cyrano.onmicrosoft.com</AttributeValue></Attribute>
                                        <Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname""><AttributeValue>1</AttributeValue></Attribute>
                                        <Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname""><AttributeValue>User</AttributeValue></Attribute>
                                        <Attribute Name=""http://schemas.microsoft.com/identity/claims/displayname""><AttributeValue>User1</AttributeValue></Attribute>
                                        <Attribute Name=""http://schemas.microsoft.com/identity/claims/identityprovider""><AttributeValue>https://sts.windows.net/add29489-7269-41f4-8841-b63c95564420/</AttributeValue></Attribute>
                                        <Attribute Name=""http://schemas.microsoft.com/claims/authnmethodsreferences""><AttributeValue>http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/password</AttributeValue></Attribute>
                                    </AttributeStatement>
                                    <AuthnStatement AuthnInstant=""2017-04-23T16:16:17.270Z"">
                                        <AuthnContext>
                                            <AuthnContextClassRef>urn:oasis:names:tc:SAML:2.0:ac:classes:Password</AuthnContextClassRef>
                                        </AuthnContext>
                                    </AuthnStatement>
                                </Assertion>
                            </t:RequestedSecurityToken>
                            <t:RequestedAttachedReference>
                                <SecurityTokenReference d3p1:TokenType=""http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV2.0"" xmlns:d3p1=""http://docs.oasis-open.org/wss/oasis-wss-wssecurity-secext-1.1.xsd"" xmlns=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"">
                                    <KeyIdentifier ValueType=""http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLID"">_edc15efd-1117-4bf9-89da-28b1663fb890</KeyIdentifier>
                                </SecurityTokenReference>
                            </t:RequestedAttachedReference>
                            <t:RequestedUnattachedReference>
                                <SecurityTokenReference d3p1:TokenType=""http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV2.0"" xmlns:d3p1=""http://docs.oasis-open.org/wss/oasis-wss-wssecurity-secext-1.1.xsd"" xmlns=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"">
                                    <KeyIdentifier ValueType=""http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLID"">_edc15efd-1117-4bf9-89da-28b1663fb890</KeyIdentifier>
                                </SecurityTokenReference>
                            </t:RequestedUnattachedReference>
                            <t:TokenType>http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV2.0</t:TokenType>
                            <t:RequestType>http://schemas.xmlsoap.org/ws/2005/02/trust/Issue</t:RequestType>
                            <t:KeyType>http://schemas.xmlsoap.org/ws/2005/05/identity/NoProofKey</t:KeyType>
                        </t:RequestSecurityTokenResponse>";
            }
        }

        public static string WresultSaml2ValidWithWhitespace
        {
            get
            {
                return @"<t:RequestSecurityTokenResponse xmlns:t=""http://schemas.xmlsoap.org/ws/2005/02/trust"">
                             
                             <t:Lifetime><wsu:Created xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">2017-04-23T16:11:17.348Z</wsu:Created><wsu:Expires xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">2017-04-23T17:11:17.348Z</wsu:Expires></t:Lifetime><wsp:AppliesTo xmlns:wsp=""http://schemas.xmlsoap.org/ws/2004/09/policy""><wsa:EndpointReference xmlns:wsa=""http://www.w3.org/2005/08/addressing""><wsa:Address>spn:fe78e0b4-6fe7-47e6-812c-fb75cee266a4</wsa:Address></wsa:EndpointReference></wsp:AppliesTo>
                             
                             <t:RequestedSecurityToken><Assertion ID=""_edc15efd-1117-4bf9-89da-28b1663fb890"" IssueInstant=""2017-04-23T16:16:17.348Z"" Version=""2.0"" xmlns=""urn:oasis:names:tc:SAML:2.0:assertion""><Issuer>https://sts.windows.net/add29489-7269-41f4-8841-b63c95564420/</Issuer><Signature xmlns=""http://www.w3.org/2000/09/xmldsig#""><SignedInfo><CanonicalizationMethod Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#""/><SignatureMethod Algorithm=""http://www.w3.org/2001/04/xmldsig-more#rsa-sha256""/><Reference URI=""#_edc15efd-1117-4bf9-89da-28b1663fb890""><Transforms><Transform Algorithm=""http://www.w3.org/2000/09/xmldsig#enveloped-signature""/><Transform Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#""/></Transforms><DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256""/><DigestValue>DO8QQoO629ApWPV3LiY2epQSv+I82iChybeRrXbhgtw=</DigestValue></Reference></SignedInfo><SignatureValue>O8JNyVKm9I7kMqlsaBgLCNwHA0qdXv34YHBVfg217lgeKkMC5taLU/EH7UeeMtapU6zMafcYoCH+Bp9zoqDpflgs78Hkjgn/dEUtjPFn7211VXClcTNqk+yhqXWtu6SKrabeIhKCKtoMA9lUAB4D6ABesb6MpwbM/ULq7T16tycZ3X//iXHeOiMwNiUAePYF22fmgrqRSDRHyLPtiLskP4UMksWJBrXUV96e9EU9aEciCvYpzMDv/VFUOCLiEkBqCdAtPVwVun+5eRk9zEh6qscWi0kAgFl3W3JhugcTTuGQYHXYVIHxbd5O33MwFIMUOmGrI1EXuk+cHIq2KUtSLg==</SignatureValue><KeyInfo><X509Data><X509Certificate>MIIDBTCCAe2gAwIBAgIQY4RNIR0dX6dBZggnkhCRoDANBgkqhkiG9w0BAQsFADAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MB4XDTE3MDIxMzAwMDAwMFoXDTE5MDIxNDAwMDAwMFowLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldDCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAMBEizU1OJms31S/ry7iav/IICYVtQ2MRPhHhYknHImtU03sgVk1Xxub4GD7R15i9UWIGbzYSGKaUtGU9lP55wrfLpDjQjEgaXi4fE6mcZBwa9qc22is23B6R67KMcVyxyDWei+IP3sKmCcMX7Ibsg+ubZUpvKGxXZ27YgqFTPqCT2znD7K81YKfy+SVg3uW6epW114yZzClTQlarptYuE2mujxjZtx7ZUlwc9AhVi8CeiLwGO1wzTmpd/uctpner6oc335rvdJikNmc1cFKCK+2irew1bgUJHuN+LJA0y5iVXKvojiKZ2Ii7QKXn19Ssg1FoJ3x2NWA06wc0CnruLsCAwEAAaMhMB8wHQYDVR0OBBYEFDAr/HCMaGqmcDJa5oualVdWAEBEMA0GCSqGSIb3DQEBCwUAA4IBAQAiUke5mA86R/X4visjceUlv5jVzCn/SIq6Gm9/wCqtSxYvifRXxwNpQTOyvHhrY/IJLRUp2g9/fDELYd65t9Dp+N8SznhfB6/Cl7P7FRo99rIlj/q7JXa8UB/vLJPDlr+NREvAkMwUs1sDhL3kSuNBoxrbLC5Jo4es+juQLXd9HcRraE4U3UZVhUS2xqjFOfaGsCbJEqqkjihssruofaxdKT1CPzPMANfREFJznNzkpJt4H0aMDgVzq69NxZ7t1JiIuc43xRjeiixQMRGMi1mAB75fTyfFJ/rWQ5J/9kh0HMZVtHsqICBF1tHMTMIK5rwoweY0cuCIpN7A/zMOQtoD</X509Certificate></X509Data></KeyInfo></Signature><Subject><NameID Format=""urn:oasis:names:tc:SAML:2.0:nameid-format:persistent"">RrX3SPSxDw6z4KHaKB2V_mnv0G-LbRZdYvo1RQa1L7s</NameID><SubjectConfirmation Method=""urn:oasis:names:tc:SAML:2.0:cm:bearer""/></Subject><Conditions NotBefore=""2017-04-23T16:11:17.348Z"" NotOnOrAfter=""2017-04-23T17:11:17.348Z""><AudienceRestriction><Audience>spn:fe78e0b4-6fe7-47e6-812c-fb75cee266a4</Audience></AudienceRestriction></Conditions><AttributeStatement><Attribute Name=""http://schemas.microsoft.com/identity/claims/tenantid""><AttributeValue>add29489-7269-41f4-8841-b63c95564420</AttributeValue></Attribute><Attribute Name=""http://schemas.microsoft.com/identity/claims/objectidentifier""><AttributeValue>d1ad9ce7-b322-4221-ab74-1e1011e1bbcb</AttributeValue></Attribute><Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name""><AttributeValue>User1@Cyrano.onmicrosoft.com</AttributeValue></Attribute><Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname""><AttributeValue>1</AttributeValue></Attribute><Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname""><AttributeValue>User</AttributeValue></Attribute><Attribute Name=""http://schemas.microsoft.com/identity/claims/displayname""><AttributeValue>User1</AttributeValue></Attribute><Attribute Name=""http://schemas.microsoft.com/identity/claims/identityprovider""><AttributeValue>https://sts.windows.net/add29489-7269-41f4-8841-b63c95564420/</AttributeValue></Attribute><Attribute Name=""http://schemas.microsoft.com/claims/authnmethodsreferences""><AttributeValue>http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/password</AttributeValue></Attribute></AttributeStatement><AuthnStatement AuthnInstant=""2017-04-23T16:16:17.270Z""><AuthnContext><AuthnContextClassRef>urn:oasis:names:tc:SAML:2.0:ac:classes:Password</AuthnContextClassRef></AuthnContext></AuthnStatement></Assertion></t:RequestedSecurityToken><t:RequestedAttachedReference><SecurityTokenReference d3p1:TokenType=""http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV2.0"" xmlns:d3p1=""http://docs.oasis-open.org/wss/oasis-wss-wssecurity-secext-1.1.xsd"" xmlns=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd""><KeyIdentifier ValueType=""http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLID"">_edc15efd-1117-4bf9-89da-28b1663fb890</KeyIdentifier></SecurityTokenReference></t:RequestedAttachedReference><t:RequestedUnattachedReference><SecurityTokenReference d3p1:TokenType=""http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV2.0"" xmlns:d3p1=""http://docs.oasis-open.org/wss/oasis-wss-wssecurity-secext-1.1.xsd"" xmlns=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd""><KeyIdentifier ValueType=""http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLID"">_edc15efd-1117-4bf9-89da-28b1663fb890</KeyIdentifier></SecurityTokenReference></t:RequestedUnattachedReference><t:TokenType>http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV2.0</t:TokenType><t:RequestType>http://schemas.xmlsoap.org/ws/2005/02/trust/Issue</t:RequestType><t:KeyType>http://schemas.xmlsoap.org/ws/2005/05/identity/NoProofKey</t:KeyType>

                         </t:RequestSecurityTokenResponse>";
            }
        }

        public static string WresultSaml2MissingRequestedSecurityTokenResponse
        {
            get
            {
                return @"<t:_RequestSecurityTokenResponse xmlns:t=""http://schemas.xmlsoap.org/ws/2005/02/trust""></t:_RequestSecurityTokenResponse>";
            }
        }

        public static string WresultSaml2MissingRequestedSecurityToken
        {
            get
            {
                return @"<t:RequestSecurityTokenResponse xmlns:t=""http://schemas.xmlsoap.org/ws/2005/02/trust"">
                            <t:_RequestedSecurityToken></t:_RequestedSecurityToken>
                         </t:RequestSecurityTokenResponse>";
            }
        }

        public static string WresultWsTrust13
        {
            get => @"<trust:RequestSecurityTokenResponseCollection xmlns:trust = ""http://docs.oasis-open.org/ws-sx/ws-trust/200512""><trust:RequestSecurityTokenResponse><trust:RequestedSecurityToken><token>dummy</token></trust:RequestedSecurityToken></trust:RequestSecurityTokenResponse></trust:RequestSecurityTokenResponseCollection>";
        }

        public static string WresultWsTrust14
        {
            get => @"<trust:RequestSecurityTokenResponseCollection xmlns:trust = ""http://docs.oasis-open.org/ws-sx/ws-trust/200802""><trust:RequestSecurityTokenResponse><trust:RequestedSecurityToken><token>dummy</token></trust:RequestedSecurityToken></trust:RequestSecurityTokenResponse></trust:RequestSecurityTokenResponseCollection>";
        }

        public static string WresultInvalidNamespace
        {
            get => @"<trust:RequestSecurityTokenResponseCollection xmlns:trust=""unsupported""><trust:RequestSecurityTokenResponse><trust:RequestedSecurityToken><token>dummy</token></trust:RequestedSecurityToken></trust:RequestSecurityTokenResponse></trust:RequestSecurityTokenResponseCollection>";
        }

        public static string WresultWsTrust13MultipleTokens
        {
            get
            {
                return @"<trust:RequestSecurityTokenResponseCollection xmlns:trust = ""http://docs.oasis-open.org/ws-sx/ws-trust/200512"">
                            <trust:RequestSecurityTokenResponse><trust:RequestedSecurityToken><token>dummy</token></trust:RequestedSecurityToken></trust:RequestSecurityTokenResponse>
                            <trust:RequestSecurityTokenResponse><trust:RequestedSecurityToken><token>dummy</token></trust:RequestedSecurityToken></trust:RequestSecurityTokenResponse>
                         </trust:RequestSecurityTokenResponseCollection>";
            }
        }

        public static string WresultWsTrust14MultipleTokens
        {
            get
            {
                return @"<trust:RequestSecurityTokenResponseCollection xmlns:trust = ""http://docs.oasis-open.org/ws-sx/ws-trust/200802"">
                            <trust:RequestSecurityTokenResponse><trust:RequestedSecurityToken><token>dummy</token></trust:RequestedSecurityToken></trust:RequestSecurityTokenResponse>
                            <trust:RequestSecurityTokenResponse><trust:RequestedSecurityToken><token>dummy</token></trust:RequestedSecurityToken></trust:RequestSecurityTokenResponse>
                         </trust:RequestSecurityTokenResponseCollection>";
            }
        }

        public static string WresultMissingRequestedSecurityTokenStartElement
        {
            get
            {
                return @"<t:RequestSecurityTokenResponse xmlns:t=""http://schemas.xmlsoap.org/ws/2005/02/trust"">
                            <t:equestedSecurityToken><token>Dummy</token></t:RequestedSecurityToken>
                         </t:RequestSecurityTokenResponse>";
            }
        }

        public static string WresultMissingRequestedSecurityTokenEndElement
        {
            get
            {
                return @"<t:RequestSecurityTokenResponse xmlns:t=""http://schemas.xmlsoap.org/ws/2005/02/trust"">
                            <t:RequestedSecurityToken><token>Dummy</token></t:equestedSecurityToken>
                         </t:RequestSecurityTokenResponse>";
            }
        }

        public static string WresultWsTrust14WithoutNamespace => "<RequestSecurityTokenResponseCollection><RequestSecurityTokenResponse><RequestedSecurityToken><token>dummy</token></RequestedSecurityToken></RequestSecurityTokenResponse></RequestSecurityTokenResponseCollection>";

        public static string WresultWsTrust14WithoutNamespaceUnusualSpacing => "<RequestSecurityTokenResponseCollection><RequestSecurityTokenResponse><  RequestedSecurityToken  >  <token>dummy</token>  </   RequestedSecurityToken></RequestSecurityTokenResponse></RequestSecurityTokenResponseCollection>";

        public static string WresultWsTrust14UnusualSpacing => @"<trust:RequestSecurityTokenResponseCollection xmlns:trust=""http://docs.oasis-open.org/ws-sx/ws-trust/200802""><trust:RequestSecurityTokenResponse>< trust:  RequestedSecurityToken  >  <token>dummy</token>  </ trust:  RequestedSecurityToken></RequestSecurityTokenResponse></RequestSecurityTokenResponseCollection>";

        public static string WresultAspWsFedHandlerValidToken = "<t:RequestSecurityTokenResponse Context=\"WsFedOwinState=AQAAANCMnd8BFdERjHoAwE_Cl-sBAAAAzaTmu3688ESVbKJen1i8YwAAAAACAAAAAAADZgAAwAAAABAAAADoUPrFjHqMTp30emvI0XZ_AAAAAASAAACgAAAAEAAAAGTBC8oT24BI8BSJf4SbwjowAAAAA4ip7JyKg6vyK-PtWTapIASA3XLOXiIj8KFO3cuSd4t4H4o-W_wnQl2FAKMOKNNrFAAAAEoWRHnCSYvPKPo0kU09EciG6TJS\" xmlns:t=\"http://schemas.xmlsoap.org/ws/2005/02/trust\">\r\n  <t:Lifetime>\r\n    <wsu:Created xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\">2014-04-18T20:21:17.341Z</wsu:Created>\r\n    <wsu:Expires xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\">2014-04-19T08:21:17.341Z</wsu:Expires>\r\n  </t:Lifetime>\r\n  <wsp:AppliesTo xmlns:wsp=\"http://schemas.xmlsoap.org/ws/2004/09/policy\">\r\n    <EndpointReference xmlns=\"http://www.w3.org/2005/08/addressing\">\r\n      <Address>http://automation1/</Address>\r\n    </EndpointReference>\r\n  </wsp:AppliesTo>\r\n  <t:RequestedSecurityToken>\r\n    <Assertion ID=\"_660ec874-f70a-4997-a9c4-bd591f1c7469\" IssueInstant=\"2014-04-18T20:21:17.450Z\" Version=\"2.0\" xmlns=\"urn:oasis:names:tc:SAML:2.0:assertion\">\r\n      <Issuer>https://sts.windows.net/4afbc689-805b-48cf-a24c-d4aa3248a248/</Issuer>\r\n      <ds:Signature xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\">\r\n        <ds:SignedInfo>\r\n          <ds:CanonicalizationMethod Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\" />\r\n          <ds:SignatureMethod Algorithm=\"http://www.w3.org/2001/04/xmldsig-more#rsa-sha256\" />\r\n          <ds:Reference URI=\"#_660ec874-f70a-4997-a9c4-bd591f1c7469\">\r\n            <ds:Transforms>\r\n              <ds:Transform Algorithm=\"http://www.w3.org/2000/09/xmldsig#enveloped-signature\" />\r\n              <ds:Transform Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\" />\r\n            </ds:Transforms>\r\n            <ds:DigestMethod Algorithm=\"http://www.w3.org/2001/04/xmlenc#sha256\" />\r\n            <ds:DigestValue>Lkq0wTyTFxLUU2cyx0XybJqhka5RzRGj6kC4aIpFg+g=</ds:DigestValue>\r\n          </ds:Reference>\r\n        </ds:SignedInfo>\r\n        <ds:SignatureValue>bPwNswOB/B9xcdAljIkin9A2vjq+u94JdyvK03mf8vZFGUYNu9uN/Q6ims1DvW1FnP7SgFBwhIvW5OjZyW8fdYGhC2bq36izkxH6ulkWbciOcyELkyHDACLudvh8kP/Q+IwpicefKzAeI2Qu/5MFq16vFg5YgI+dovg8u1fYPPEPmmptW893RNTHWeh9mLRpLYnHyg7aLG6emNRkEu7w9rzeoICeMFybb9BvJl/q/8MFCW/Z5WemQhCi6YXFSEwCO6zJzCFi/3T6ChU/xYgXbFykDLqulsNOCQxdgutyqxJzugt+3PH5IKHHuoqe7UZNUIyELJ4BgwE1sXCGYIi24rg==</ds:SignatureValue>\r\n        <KeyInfo xmlns=\"http://www.w3.org/2000/09/xmldsig#\">\r\n          <X509Data>\r\n            <X509Certificate>ThisIsAValidToken</X509Certificate>\r\n          </X509Data>\r\n        </KeyInfo>\r\n      </ds:Signature>\r\n      <Subject>\r\n        <NameID>t0ch1TsP0pi5VoW8q5CGWsCXVZoNtpsg0mbMZPOYb4I</NameID>\r\n        <SubjectConfirmation Method=\"urn:oasis:names:tc:SAML:2.0:cm:bearer\" />\r\n      </Subject>\r\n      <Conditions NotBefore=\"2014-04-18T20:21:17.341Z\" NotOnOrAfter=\"2014-04-19T08:21:17.341Z\">\r\n        <AudienceRestriction>\r\n          <Audience>http://Automation1</Audience>\r\n        </AudienceRestriction>\r\n      </Conditions>\r\n      <AttributeStatement>\r\n        <Attribute Name=\"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname\">\r\n          <AttributeValue>Test</AttributeValue>\r\n        </Attribute>\r\n        <Attribute Name=\"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname\">\r\n          <AttributeValue>Test</AttributeValue>\r\n        </Attribute>\r\n        <Attribute Name=\"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name\">\r\n          <AttributeValue>user1@praburajgmail.onmicrosoft.com</AttributeValue>\r\n        </Attribute>\r\n        <Attribute Name=\"http://schemas.microsoft.com/identity/claims/tenantid\">\r\n          <AttributeValue>4afbc689-805b-48cf-a24c-d4aa3248a248</AttributeValue>\r\n        </Attribute>\r\n        <Attribute Name=\"http://schemas.microsoft.com/identity/claims/objectidentifier\">\r\n          <AttributeValue>c2f0cd49-5e53-4520-8ed9-4e178dc488c5</AttributeValue>\r\n        </Attribute>\r\n        <Attribute Name=\"http://schemas.microsoft.com/identity/claims/identityprovider\">\r\n          <AttributeValue>https://sts.windows.net/4afbc689-805b-48cf-a24c-d4aa3248a248/</AttributeValue>\r\n        </Attribute>\r\n      </AttributeStatement>\r\n      <AuthnStatement AuthnInstant=\"2014-04-18T20:21:14.000Z\">\r\n        <AuthnContext>\r\n          <AuthnContextClassRef>urn:oasis:names:tc:SAML:2.0:ac:classes:Password</AuthnContextClassRef>\r\n        </AuthnContext>\r\n      </AuthnStatement>\r\n    </Assertion>\r\n  </t:RequestedSecurityToken>\r\n  <t:RequestedAttachedReference>\r\n    <SecurityTokenReference d3p1:TokenType=\"http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV2.0\" xmlns:d3p1=\"http://docs.oasis-open.org/wss/oasis-wss-wssecurity-secext-1.1.xsd\" xmlns=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">\r\n      <KeyIdentifier ValueType=\"http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLID\">_660ec874-f70a-4997-a9c4-bd591f1c7469</KeyIdentifier>\r\n    </SecurityTokenReference>\r\n  </t:RequestedAttachedReference>\r\n  <t:RequestedUnattachedReference>\r\n    <SecurityTokenReference d3p1:TokenType=\"http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV2.0\" xmlns:d3p1=\"http://docs.oasis-open.org/wss/oasis-wss-wssecurity-secext-1.1.xsd\" xmlns=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">\r\n      <KeyIdentifier ValueType=\"http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLID\">_660ec874-f70a-4997-a9c4-bd591f1c7469</KeyIdentifier>\r\n    </SecurityTokenReference>\r\n  </t:RequestedUnattachedReference>\r\n  <t:TokenType>http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV2.0</t:TokenType>\r\n  <t:RequestType>http://schemas.xmlsoap.org/ws/2005/02/trust/Issue</t:RequestType>\r\n  <t:KeyType>http://schemas.xmlsoap.org/ws/2005/05/identity/NoProofKey</t:KeyType>\r\n</t:RequestSecurityTokenResponse>";

        #endregion

        #region WaSignin

        public static string WaSignInValid
        {
            get => @"wa=wsignin1.0&wresult=%3Ct%3ARequestSecurityTokenResponse+xmlns%3At%3D%22http%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2005%2F02%2Ftrust%22%3E%3Ct%3ALifetime%3E%3Cwsu%3ACreated+xmlns%3Awsu%3D%22http%3A%2F%2Fdocs.oasis-open.org%2Fwss%2F2004%2F01%2Foasis-200401-wss-wssecurity-utility-1.0.xsd%22%3E2017-04-23T17%3A40%3A36.882Z%3C%2Fwsu%3ACreated%3E%3Cwsu%3AExpires+xmlns%3Awsu%3D%22http%3A%2F%2Fdocs.oasis-open.org%2Fwss%2F2004%2F01%2Foasis-200401-wss-wssecurity-utility-1.0.xsd%22%3E2017-04-23T18%3A40%3A36.882Z%3C%2Fwsu%3AExpires%3E%3C%2Ft%3ALifetime%3E%3Cwsp%3AAppliesTo+xmlns%3Awsp%3D%22http%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2004%2F09%2Fpolicy%22%3E%3Cwsa%3AEndpointReference+xmlns%3Awsa%3D%22http%3A%2F%2Fwww.w3.org%2F2005%2F08%2Faddressing%22%3E%3Cwsa%3AAddress%3Espn%3Afe78e0b4-6fe7-47e6-812c-fb75cee266a4%3C%2Fwsa%3AAddress%3E%3C%2Fwsa%3AEndpointReference%3E%3C%2Fwsp%3AAppliesTo%3E%3Ct%3ARequestedSecurityToken%3E%3CAssertion+ID%3D%22_710d4516-27ac-4547-816d-3947aeea6edf%22+IssueInstant%3D%222017-04-23T17%3A45%3A36.882Z%22+Version%3D%222.0%22+xmlns%3D%22urn%3Aoasis%3Anames%3Atc%3ASAML%3A2.0%3Aassertion%22%3E%3CIssuer%3Ehttps%3A%2F%2Fsts.windows.net%2Fadd29489-7269-41f4-8841-b63c95564420%2F%3C%2FIssuer%3E%3CSignature+xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2F09%2Fxmldsig%23%22%3E%3CSignedInfo%3E%3CCanonicalizationMethod+Algorithm%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2F10%2Fxml-exc-c14n%23%22+%2F%3E%3CSignatureMethod+Algorithm%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2F04%2Fxmldsig-more%23rsa-sha256%22+%2F%3E%3CReference+URI%3D%22%23_710d4516-27ac-4547-816d-3947aeea6edf%22%3E%3CTransforms%3E%3CTransform+Algorithm%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2F09%2Fxmldsig%23enveloped-signature%22+%2F%3E%3CTransform+Algorithm%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2F10%2Fxml-exc-c14n%23%22+%2F%3E%3C%2FTransforms%3E%3CDigestMethod+Algorithm%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2F04%2Fxmlenc%23sha256%22+%2F%3E%3CDigestValue%3Ekv1eRd%2BMFFaevhwOeeJa5BLkLRV3tFB5QKVD8JBOgMg%3D%3C%2FDigestValue%3E%3C%2FReference%3E%3C%2FSignedInfo%3E%3CSignatureValue%3EABAzMsMPGlKCA0HcmAAiCFwMZr0gtKMUQpRUTCX8YaLAIxOgIW3ZBMTwPHKa2K2lp1Tk97oQBE3S%2Bfg7TKriP1bZB8bdPpfu4GxeeYQteWT7dD%2FJPy1SYCMiRMSsNO4T3O6Keaci1pzwIxzrH8S5s7gzBFPgljf5mHBtZrCTQIK7Ng%2Fnk%2BSpea3RXew05yAQX%2B14Eq6IGPIBsFJidjLqyoslK4OZMNtHLF443AfLs4Ltwfaf89QuOXDpIkKaaHb98uh9XlhB8IMcqzZ1hi2kSadoO7drtOppmeFgYcyOidEuwqQ5%2BksxrOwRb%2F88AfSYEjqUM6U2ldrE8RoUdSf3gQ%3D%3D%3C%2FSignatureValue%3E%3CKeyInfo%3E%3CX509Data%3E%3CX509Certificate%3EMIIDBTCCAe2gAwIBAgIQY4RNIR0dX6dBZggnkhCRoDANBgkqhkiG9w0BAQsFADAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MB4XDTE3MDIxMzAwMDAwMFoXDTE5MDIxNDAwMDAwMFowLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldDCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAMBEizU1OJms31S%2Fry7iav%2FIICYVtQ2MRPhHhYknHImtU03sgVk1Xxub4GD7R15i9UWIGbzYSGKaUtGU9lP55wrfLpDjQjEgaXi4fE6mcZBwa9qc22is23B6R67KMcVyxyDWei%2BIP3sKmCcMX7Ibsg%2BubZUpvKGxXZ27YgqFTPqCT2znD7K81YKfy%2BSVg3uW6epW114yZzClTQlarptYuE2mujxjZtx7ZUlwc9AhVi8CeiLwGO1wzTmpd%2Fuctpner6oc335rvdJikNmc1cFKCK%2B2irew1bgUJHuN%2BLJA0y5iVXKvojiKZ2Ii7QKXn19Ssg1FoJ3x2NWA06wc0CnruLsCAwEAAaMhMB8wHQYDVR0OBBYEFDAr%2FHCMaGqmcDJa5oualVdWAEBEMA0GCSqGSIb3DQEBCwUAA4IBAQAiUke5mA86R%2FX4visjceUlv5jVzCn%2FSIq6Gm9%2FwCqtSxYvifRXxwNpQTOyvHhrY%2FIJLRUp2g9%2FfDELYd65t9Dp%2BN8SznhfB6%2FCl7P7FRo99rIlj%2Fq7JXa8UB%2FvLJPDlr%2BNREvAkMwUs1sDhL3kSuNBoxrbLC5Jo4es%2BjuQLXd9HcRraE4U3UZVhUS2xqjFOfaGsCbJEqqkjihssruofaxdKT1CPzPMANfREFJznNzkpJt4H0aMDgVzq69NxZ7t1JiIuc43xRjeiixQMRGMi1mAB75fTyfFJ%2FrWQ5J%2F9kh0HMZVtHsqICBF1tHMTMIK5rwoweY0cuCIpN7A%2FzMOQtoD%3C%2FX509Certificate%3E%3C%2FX509Data%3E%3C%2FKeyInfo%3E%3C%2FSignature%3E%3CSubject%3E%3CNameID+Format%3D%22urn%3Aoasis%3Anames%3Atc%3ASAML%3A2.0%3Anameid-format%3Apersistent%22%3ERrX3SPSxDw6z4KHaKB2V_mnv0G-LbRZdYvo1RQa1L7s%3C%2FNameID%3E%3CSubjectConfirmation+Method%3D%22urn%3Aoasis%3Anames%3Atc%3ASAML%3A2.0%3Acm%3Abearer%22+%2F%3E%3C%2FSubject%3E%3CConditions+NotBefore%3D%222017-04-23T17%3A40%3A36.882Z%22+NotOnOrAfter%3D%222017-04-23T18%3A40%3A36.882Z%22%3E%3CAudienceRestriction%3E%3CAudience%3Espn%3Afe78e0b4-6fe7-47e6-812c-fb75cee266a4%3C%2FAudience%3E%3C%2FAudienceRestriction%3E%3C%2FConditions%3E%3CAttributeStatement%3E%3CAttribute+Name%3D%22http%3A%2F%2Fschemas.microsoft.com%2Fidentity%2Fclaims%2Ftenantid%22%3E%3CAttributeValue%3Eadd29489-7269-41f4-8841-b63c95564420%3C%2FAttributeValue%3E%3C%2FAttribute%3E%3CAttribute+Name%3D%22http%3A%2F%2Fschemas.microsoft.com%2Fidentity%2Fclaims%2Fobjectidentifier%22%3E%3CAttributeValue%3Ed1ad9ce7-b322-4221-ab74-1e1011e1bbcb%3C%2FAttributeValue%3E%3C%2FAttribute%3E%3CAttribute+Name%3D%22http%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2005%2F05%2Fidentity%2Fclaims%2Fname%22%3E%3CAttributeValue%3EUser1%40Cyrano.onmicrosoft.com%3C%2FAttributeValue%3E%3C%2FAttribute%3E%3CAttribute+Name%3D%22http%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2005%2F05%2Fidentity%2Fclaims%2Fsurname%22%3E%3CAttributeValue%3E1%3C%2FAttributeValue%3E%3C%2FAttribute%3E%3CAttribute+Name%3D%22http%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2005%2F05%2Fidentity%2Fclaims%2Fgivenname%22%3E%3CAttributeValue%3EUser%3C%2FAttributeValue%3E%3C%2FAttribute%3E%3CAttribute+Name%3D%22http%3A%2F%2Fschemas.microsoft.com%2Fidentity%2Fclaims%2Fdisplayname%22%3E%3CAttributeValue%3EUser1%3C%2FAttributeValue%3E%3C%2FAttribute%3E%3CAttribute+Name%3D%22http%3A%2F%2Fschemas.microsoft.com%2Fidentity%2Fclaims%2Fidentityprovider%22%3E%3CAttributeValue%3Ehttps%3A%2F%2Fsts.windows.net%2Fadd29489-7269-41f4-8841-b63c95564420%2F%3C%2FAttributeValue%3E%3C%2FAttribute%3E%3CAttribute+Name%3D%22http%3A%2F%2Fschemas.microsoft.com%2Fclaims%2Fauthnmethodsreferences%22%3E%3CAttributeValue%3Ehttp%3A%2F%2Fschemas.microsoft.com%2Fws%2F2008%2F06%2Fidentity%2Fauthenticationmethod%2Fpassword%3C%2FAttributeValue%3E%3C%2FAttribute%3E%3C%2FAttributeStatement%3E%3CAuthnStatement+AuthnInstant%3D%222017-04-23T16%3A16%3A17.270Z%22%3E%3CAuthnContext%3E%3CAuthnContextClassRef%3Eurn%3Aoasis%3Anames%3Atc%3ASAML%3A2.0%3Aac%3Aclasses%3APassword%3C%2FAuthnContextClassRef%3E%3C%2FAuthnContext%3E%3C%2FAuthnStatement%3E%3C%2FAssertion%3E%3C%2Ft%3ARequestedSecurityToken%3E%3Ct%3ARequestedAttachedReference%3E%3CSecurityTokenReference+d3p1%3ATokenType%3D%22http%3A%2F%2Fdocs.oasis-open.org%2Fwss%2Foasis-wss-saml-token-profile-1.1%23SAMLV2.0%22+xmlns%3Ad3p1%3D%22http%3A%2F%2Fdocs.oasis-open.org%2Fwss%2Foasis-wss-wssecurity-secext-1.1.xsd%22+xmlns%3D%22http%3A%2F%2Fdocs.oasis-open.org%2Fwss%2F2004%2F01%2Foasis-200401-wss-wssecurity-secext-1.0.xsd%22%3E%3CKeyIdentifier+ValueType%3D%22http%3A%2F%2Fdocs.oasis-open.org%2Fwss%2Foasis-wss-saml-token-profile-1.1%23SAMLID%22%3E_710d4516-27ac-4547-816d-3947aeea6edf%3C%2FKeyIdentifier%3E%3C%2FSecurityTokenReference%3E%3C%2Ft%3ARequestedAttachedReference%3E%3Ct%3ARequestedUnattachedReference%3E%3CSecurityTokenReference+d3p1%3ATokenType%3D%22http%3A%2F%2Fdocs.oasis-open.org%2Fwss%2Foasis-wss-saml-token-profile-1.1%23SAMLV2.0%22+xmlns%3Ad3p1%3D%22http%3A%2F%2Fdocs.oasis-open.org%2Fwss%2Foasis-wss-wssecurity-secext-1.1.xsd%22+xmlns%3D%22http%3A%2F%2Fdocs.oasis-open.org%2Fwss%2F2004%2F01%2Foasis-200401-wss-wssecurity-secext-1.0.xsd%22%3E%3CKeyIdentifier+ValueType%3D%22http%3A%2F%2Fdocs.oasis-open.org%2Fwss%2Foasis-wss-saml-token-profile-1.1%23SAMLID%22%3E_710d4516-27ac-4547-816d-3947aeea6edf%3C%2FKeyIdentifier%3E%3C%2FSecurityTokenReference%3E%3C%2Ft%3ARequestedUnattachedReference%3E%3Ct%3ATokenType%3Ehttp%3A%2F%2Fdocs.oasis-open.org%2Fwss%2Foasis-wss-saml-token-profile-1.1%23SAMLV2.0%3C%2Ft%3ATokenType%3E%3Ct%3ARequestType%3Ehttp%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2005%2F02%2Ftrust%2FIssue%3C%2Ft%3ARequestType%3E%3Ct%3AKeyType%3Ehttp%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2005%2F05%2Fidentity%2FNoProofKey%3C%2Ft%3AKeyType%3E%3C%2Ft%3ARequestSecurityTokenResponse%3E&wctx=WsFedOwinState%3DZfCHQBMGl9Nia9P6tbsUq5AFCEu9fGolLxTkikMW-zGMhRMsZb6ofrdCD9uni2PoEuW_1zfJPtZawNSjiy4vIg5o2TJeGiwmKjqM0y3bi4w";
        }

        public static WsFederationMessageTestSet WsSignInTestSet
        {
            get
            {
                return new WsFederationMessageTestSet
                {
                    WsFederationMessage = new WsFederationMessage
                    {
                        Wa = WsFederationConstants.WsFederationActions.SignIn,
                        Wresult = Uri.UnescapeDataString(@"%3Ct%3ARequestSecurityTokenResponse+xmlns%3At%3D%22http%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2005%2F02%2Ftrust%22%3E%3Ct%3ALifetime%3E%3Cwsu%3ACreated+xmlns%3Awsu%3D%22http%3A%2F%2Fdocs.oasis-open.org%2Fwss%2F2004%2F01%2Foasis-200401-wss-wssecurity-utility-1.0.xsd%22%3E2017-04-23T17%3A40%3A36.882Z%3C%2Fwsu%3ACreated%3E%3Cwsu%3AExpires+xmlns%3Awsu%3D%22http%3A%2F%2Fdocs.oasis-open.org%2Fwss%2F2004%2F01%2Foasis-200401-wss-wssecurity-utility-1.0.xsd%22%3E2017-04-23T18%3A40%3A36.882Z%3C%2Fwsu%3AExpires%3E%3C%2Ft%3ALifetime%3E%3Cwsp%3AAppliesTo+xmlns%3Awsp%3D%22http%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2004%2F09%2Fpolicy%22%3E%3Cwsa%3AEndpointReference+xmlns%3Awsa%3D%22http%3A%2F%2Fwww.w3.org%2F2005%2F08%2Faddressing%22%3E%3Cwsa%3AAddress%3Espn%3Afe78e0b4-6fe7-47e6-812c-fb75cee266a4%3C%2Fwsa%3AAddress%3E%3C%2Fwsa%3AEndpointReference%3E%3C%2Fwsp%3AAppliesTo%3E%3Ct%3ARequestedSecurityToken%3E%3CAssertion+ID%3D%22_710d4516-27ac-4547-816d-3947aeea6edf%22+IssueInstant%3D%222017-04-23T17%3A45%3A36.882Z%22+Version%3D%222.0%22+xmlns%3D%22urn%3Aoasis%3Anames%3Atc%3ASAML%3A2.0%3Aassertion%22%3E%3CIssuer%3Ehttps%3A%2F%2Fsts.windows.net%2Fadd29489-7269-41f4-8841-b63c95564420%2F%3C%2FIssuer%3E%3CSignature+xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2F09%2Fxmldsig%23%22%3E%3CSignedInfo%3E%3CCanonicalizationMethod+Algorithm%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2F10%2Fxml-exc-c14n%23%22+%2F%3E%3CSignatureMethod+Algorithm%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2F04%2Fxmldsig-more%23rsa-sha256%22+%2F%3E%3CReference+URI%3D%22%23_710d4516-27ac-4547-816d-3947aeea6edf%22%3E%3CTransforms%3E%3CTransform+Algorithm%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2F09%2Fxmldsig%23enveloped-signature%22+%2F%3E%3CTransform+Algorithm%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2F10%2Fxml-exc-c14n%23%22+%2F%3E%3C%2FTransforms%3E%3CDigestMethod+Algorithm%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2F04%2Fxmlenc%23sha256%22+%2F%3E%3CDigestValue%3Ekv1eRd%2BMFFaevhwOeeJa5BLkLRV3tFB5QKVD8JBOgMg%3D%3C%2FDigestValue%3E%3C%2FReference%3E%3C%2FSignedInfo%3E%3CSignatureValue%3EABAzMsMPGlKCA0HcmAAiCFwMZr0gtKMUQpRUTCX8YaLAIxOgIW3ZBMTwPHKa2K2lp1Tk97oQBE3S%2Bfg7TKriP1bZB8bdPpfu4GxeeYQteWT7dD%2FJPy1SYCMiRMSsNO4T3O6Keaci1pzwIxzrH8S5s7gzBFPgljf5mHBtZrCTQIK7Ng%2Fnk%2BSpea3RXew05yAQX%2B14Eq6IGPIBsFJidjLqyoslK4OZMNtHLF443AfLs4Ltwfaf89QuOXDpIkKaaHb98uh9XlhB8IMcqzZ1hi2kSadoO7drtOppmeFgYcyOidEuwqQ5%2BksxrOwRb%2F88AfSYEjqUM6U2ldrE8RoUdSf3gQ%3D%3D%3C%2FSignatureValue%3E%3CKeyInfo%3E%3CX509Data%3E%3CX509Certificate%3EMIIDBTCCAe2gAwIBAgIQY4RNIR0dX6dBZggnkhCRoDANBgkqhkiG9w0BAQsFADAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MB4XDTE3MDIxMzAwMDAwMFoXDTE5MDIxNDAwMDAwMFowLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldDCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAMBEizU1OJms31S%2Fry7iav%2FIICYVtQ2MRPhHhYknHImtU03sgVk1Xxub4GD7R15i9UWIGbzYSGKaUtGU9lP55wrfLpDjQjEgaXi4fE6mcZBwa9qc22is23B6R67KMcVyxyDWei%2BIP3sKmCcMX7Ibsg%2BubZUpvKGxXZ27YgqFTPqCT2znD7K81YKfy%2BSVg3uW6epW114yZzClTQlarptYuE2mujxjZtx7ZUlwc9AhVi8CeiLwGO1wzTmpd%2Fuctpner6oc335rvdJikNmc1cFKCK%2B2irew1bgUJHuN%2BLJA0y5iVXKvojiKZ2Ii7QKXn19Ssg1FoJ3x2NWA06wc0CnruLsCAwEAAaMhMB8wHQYDVR0OBBYEFDAr%2FHCMaGqmcDJa5oualVdWAEBEMA0GCSqGSIb3DQEBCwUAA4IBAQAiUke5mA86R%2FX4visjceUlv5jVzCn%2FSIq6Gm9%2FwCqtSxYvifRXxwNpQTOyvHhrY%2FIJLRUp2g9%2FfDELYd65t9Dp%2BN8SznhfB6%2FCl7P7FRo99rIlj%2Fq7JXa8UB%2FvLJPDlr%2BNREvAkMwUs1sDhL3kSuNBoxrbLC5Jo4es%2BjuQLXd9HcRraE4U3UZVhUS2xqjFOfaGsCbJEqqkjihssruofaxdKT1CPzPMANfREFJznNzkpJt4H0aMDgVzq69NxZ7t1JiIuc43xRjeiixQMRGMi1mAB75fTyfFJ%2FrWQ5J%2F9kh0HMZVtHsqICBF1tHMTMIK5rwoweY0cuCIpN7A%2FzMOQtoD%3C%2FX509Certificate%3E%3C%2FX509Data%3E%3C%2FKeyInfo%3E%3C%2FSignature%3E%3CSubject%3E%3CNameID+Format%3D%22urn%3Aoasis%3Anames%3Atc%3ASAML%3A2.0%3Anameid-format%3Apersistent%22%3ERrX3SPSxDw6z4KHaKB2V_mnv0G-LbRZdYvo1RQa1L7s%3C%2FNameID%3E%3CSubjectConfirmation+Method%3D%22urn%3Aoasis%3Anames%3Atc%3ASAML%3A2.0%3Acm%3Abearer%22+%2F%3E%3C%2FSubject%3E%3CConditions+NotBefore%3D%222017-04-23T17%3A40%3A36.882Z%22+NotOnOrAfter%3D%222017-04-23T18%3A40%3A36.882Z%22%3E%3CAudienceRestriction%3E%3CAudience%3Espn%3Afe78e0b4-6fe7-47e6-812c-fb75cee266a4%3C%2FAudience%3E%3C%2FAudienceRestriction%3E%3C%2FConditions%3E%3CAttributeStatement%3E%3CAttribute+Name%3D%22http%3A%2F%2Fschemas.microsoft.com%2Fidentity%2Fclaims%2Ftenantid%22%3E%3CAttributeValue%3Eadd29489-7269-41f4-8841-b63c95564420%3C%2FAttributeValue%3E%3C%2FAttribute%3E%3CAttribute+Name%3D%22http%3A%2F%2Fschemas.microsoft.com%2Fidentity%2Fclaims%2Fobjectidentifier%22%3E%3CAttributeValue%3Ed1ad9ce7-b322-4221-ab74-1e1011e1bbcb%3C%2FAttributeValue%3E%3C%2FAttribute%3E%3CAttribute+Name%3D%22http%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2005%2F05%2Fidentity%2Fclaims%2Fname%22%3E%3CAttributeValue%3EUser1%40Cyrano.onmicrosoft.com%3C%2FAttributeValue%3E%3C%2FAttribute%3E%3CAttribute+Name%3D%22http%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2005%2F05%2Fidentity%2Fclaims%2Fsurname%22%3E%3CAttributeValue%3E1%3C%2FAttributeValue%3E%3C%2FAttribute%3E%3CAttribute+Name%3D%22http%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2005%2F05%2Fidentity%2Fclaims%2Fgivenname%22%3E%3CAttributeValue%3EUser%3C%2FAttributeValue%3E%3C%2FAttribute%3E%3CAttribute+Name%3D%22http%3A%2F%2Fschemas.microsoft.com%2Fidentity%2Fclaims%2Fdisplayname%22%3E%3CAttributeValue%3EUser1%3C%2FAttributeValue%3E%3C%2FAttribute%3E%3CAttribute+Name%3D%22http%3A%2F%2Fschemas.microsoft.com%2Fidentity%2Fclaims%2Fidentityprovider%22%3E%3CAttributeValue%3Ehttps%3A%2F%2Fsts.windows.net%2Fadd29489-7269-41f4-8841-b63c95564420%2F%3C%2FAttributeValue%3E%3C%2FAttribute%3E%3CAttribute+Name%3D%22http%3A%2F%2Fschemas.microsoft.com%2Fclaims%2Fauthnmethodsreferences%22%3E%3CAttributeValue%3Ehttp%3A%2F%2Fschemas.microsoft.com%2Fws%2F2008%2F06%2Fidentity%2Fauthenticationmethod%2Fpassword%3C%2FAttributeValue%3E%3C%2FAttribute%3E%3C%2FAttributeStatement%3E%3CAuthnStatement+AuthnInstant%3D%222017-04-23T16%3A16%3A17.270Z%22%3E%3CAuthnContext%3E%3CAuthnContextClassRef%3Eurn%3Aoasis%3Anames%3Atc%3ASAML%3A2.0%3Aac%3Aclasses%3APassword%3C%2FAuthnContextClassRef%3E%3C%2FAuthnContext%3E%3C%2FAuthnStatement%3E%3C%2FAssertion%3E%3C%2Ft%3ARequestedSecurityToken%3E%3Ct%3ARequestedAttachedReference%3E%3CSecurityTokenReference+d3p1%3ATokenType%3D%22http%3A%2F%2Fdocs.oasis-open.org%2Fwss%2Foasis-wss-saml-token-profile-1.1%23SAMLV2.0%22+xmlns%3Ad3p1%3D%22http%3A%2F%2Fdocs.oasis-open.org%2Fwss%2Foasis-wss-wssecurity-secext-1.1.xsd%22+xmlns%3D%22http%3A%2F%2Fdocs.oasis-open.org%2Fwss%2F2004%2F01%2Foasis-200401-wss-wssecurity-secext-1.0.xsd%22%3E%3CKeyIdentifier+ValueType%3D%22http%3A%2F%2Fdocs.oasis-open.org%2Fwss%2Foasis-wss-saml-token-profile-1.1%23SAMLID%22%3E_710d4516-27ac-4547-816d-3947aeea6edf%3C%2FKeyIdentifier%3E%3C%2FSecurityTokenReference%3E%3C%2Ft%3ARequestedAttachedReference%3E%3Ct%3ARequestedUnattachedReference%3E%3CSecurityTokenReference+d3p1%3ATokenType%3D%22http%3A%2F%2Fdocs.oasis-open.org%2Fwss%2Foasis-wss-saml-token-profile-1.1%23SAMLV2.0%22+xmlns%3Ad3p1%3D%22http%3A%2F%2Fdocs.oasis-open.org%2Fwss%2Foasis-wss-wssecurity-secext-1.1.xsd%22+xmlns%3D%22http%3A%2F%2Fdocs.oasis-open.org%2Fwss%2F2004%2F01%2Foasis-200401-wss-wssecurity-secext-1.0.xsd%22%3E%3CKeyIdentifier+ValueType%3D%22http%3A%2F%2Fdocs.oasis-open.org%2Fwss%2Foasis-wss-saml-token-profile-1.1%23SAMLID%22%3E_710d4516-27ac-4547-816d-3947aeea6edf%3C%2FKeyIdentifier%3E%3C%2FSecurityTokenReference%3E%3C%2Ft%3ARequestedUnattachedReference%3E%3Ct%3ATokenType%3Ehttp%3A%2F%2Fdocs.oasis-open.org%2Fwss%2Foasis-wss-saml-token-profile-1.1%23SAMLV2.0%3C%2Ft%3ATokenType%3E%3Ct%3ARequestType%3Ehttp%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2005%2F02%2Ftrust%2FIssue%3C%2Ft%3ARequestType%3E%3Ct%3AKeyType%3Ehttp%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2005%2F05%2Fidentity%2FNoProofKey%3C%2Ft%3AKeyType%3E%3C%2Ft%3ARequestSecurityTokenResponse%3E".Replace('+', ' ')),
                        Wctx = Uri.UnescapeDataString(@"WsFedOwinState%3DZfCHQBMGl9Nia9P6tbsUq5AFCEu9fGolLxTkikMW-zGMhRMsZb6ofrdCD9uni2PoEuW_1zfJPtZawNSjiy4vIg5o2TJeGiwmKjqM0y3bi4w".Replace('+', ' '))
                    },
                    Xml = WaSignInValid
                };
            }
        }

        public static string WaSignInWithCRLF => @"wa=wsignin1.0&wresult=%3Ct%3ARequestSecurityTokenResponse+xmlns%3At%3D%22http%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2005%2F02%2Ftrust%22%3E%3Ct%3ALifetime%3E%3Cwsu%3ACreated+xmlns%3Awsu%3D%22http%3A%2F%2Fdocs.oasis-open.org%2Fwss%2F2004%2F01%2Foasis-200401-wss-wssecurity-utility-1.0.xsd%22%3E2017-07-28T15%3A13%3A11.331Z%3C%2Fwsu%3ACreated%3E%3Cwsu%3AExpires+xmlns%3Awsu%3D%22http%3A%2F%2Fdocs.oasis-open.org%2Fwss%2F2004%2F01%2Foasis-200401-wss-wssecurity-utility-1.0.xsd%22%3E2017-07-28T16%3A13%3A11.331Z%3C%2Fwsu%3AExpires%3E%3C%2Ft%3ALifetime%3E%3Cwsp%3AAppliesTo+xmlns%3Awsp%3D%22http%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2004%2F09%2Fpolicy%22%3E%3Cwsa%3AEndpointReference+xmlns%3Awsa%3D%22http%3A%2F%2Fwww.w3.org%2F2005%2F08%2Faddressing%22%3E%3Cwsa%3AAddress%3Ehttps%3A%2F%2Fapp1.sub2.fracas365.msftonlinerepro.com%2Fsampapp%2F%3C%2Fwsa%3AAddress%3E%3C%2Fwsa%3AEndpointReference%3E%3C%2Fwsp%3AAppliesTo%3E%3Ct%3ARequestedSecurityToken%3E%3Csaml%3AAssertion+MajorVersion%3D%221%22+MinorVersion%3D%221%22+AssertionID%3D%22_6f8e1e8b-d3df-43b7-8b39-2681776af63d%22+Issuer%3D%22http%3A%2F%2Fsts.sub2.fracas365.msftonlinerepro.com%2Fadfs%2Fservices%2Ftrust%22+IssueInstant%3D%222017-07-28T15%3A13%3A11.331Z%22+xmlns%3Asaml%3D%22urn%3Aoasis%3Anames%3Atc%3ASAML%3A1.0%3Aassertion%22%3E%3Csaml%3AConditions+NotBefore%3D%222017-07-28T15%3A13%3A11.331Z%22+NotOnOrAfter%3D%222017-07-28T16%3A13%3A11.331Z%22%3E%3Csaml%3AAudienceRestrictionCondition%3E%3Csaml%3AAudience%3Ehttps%3A%2F%2Fapp1.sub2.fracas365.msftonlinerepro.com%2Fsampapp%2F%3C%2Fsaml%3AAudience%3E%3C%2Fsaml%3AAudienceRestrictionCondition%3E%3C%2Fsaml%3AConditions%3E%3Csaml%3AAttributeStatement%3E%3Csaml%3ASubject%3E%3Csaml%3ANameIdentifier+Format%3D%22urn%3Aoasis%3Anames%3Atc%3ASAML%3A1.1%3Anameid-format%3Aunspecified%22%3Ekiller%3C%2Fsaml%3ANameIdentifier%3E%3Csaml%3ASubjectConfirmation%3E%3Csaml%3AConfirmationMethod%3Eurn%3Aoasis%3Anames%3Atc%3ASAML%3A1.0%3Acm%3Abearer%3C%2Fsaml%3AConfirmationMethod%3E%3C%2Fsaml%3ASubjectConfirmation%3E%3C%2Fsaml%3ASubject%3E%3Csaml%3AAttribute+AttributeName%3D%22upn%22+AttributeNamespace%3D%22http%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2005%2F05%2Fidentity%2Fclaims%22%3E%3Csaml%3AAttributeValue%3Ekiller@sub2.fracas365.msftonlinerepro.com%3C%2Fsaml%3AAttributeValue%3E%3C%2Fsaml%3AAttribute%3E%3Csaml%3AAttribute+AttributeName%3D%22x-ms-endpoint-absolute-path%22+AttributeNamespace%3D%22http%3A%2F%2Fschemas.microsoft.com%2F2012%2F01%2Frequestcontext%2Fclaims%22+a%3AOriginalIssuer%3D%22CLIENT+CONTEXT%22+xmlns%3Aa%3D%22http%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2009%2F09%2Fidentity%2Fclaims%22%3E%3Csaml%3AAttributeValue%3E%2Fadfs%2Fls%2Fwia%3C%2Fsaml%3AAttributeValue%3E%3C%2Fsaml%3AAttribute%3E%3Csaml%3AAttribute+AttributeName%3D%22x-ms-client-ip%22+AttributeNamespace%3D%22http%3A%2F%2Fschemas.microsoft.com%2F2012%2F01%2Frequestcontext%2Fclaims%22+a%3AOriginalIssuer%3D%22CLIENT+CONTEXT%22+xmlns%3Aa%3D%22http%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2009%2F09%2Fidentity%2Fclaims%22%3E%3Csaml%3AAttributeValue%3E172.15.0.67%3C%2Fsaml%3AAttributeValue%3E%3C%2Fsaml%3AAttribute%3E%3Csaml%3AAttribute+AttributeName%3D%22primarygroupsid%22+AttributeNamespace%3D%22http%3A%2F%2Fschemas.microsoft.com%2Fws%2F2008%2F06%2Fidentity%2Fclaims%22%3E%3Csaml%3AAttributeValue%3ES-1-5-21-487734988-61580006-1080473273-513%3C%2Fsaml%3AAttributeValue%3E%3C%2Fsaml%3AAttribute%3E%3Csaml%3AAttribute+AttributeName%3D%22authnmethodsreferences%22+AttributeNamespace%3D%22http%3A%2F%2Fschemas.microsoft.com%2Fclaims%22%3E%3Csaml%3AAttributeValue%3Ehttp%3A%2F%2Fschemas.microsoft.com%2Fws%2F2008%2F06%2Fidentity%2Fauthenticationmethod%2Fwindows%3C%2Fsaml%3AAttributeValue%3E%3C%2Fsaml%3AAttribute%3E%3Csaml%3AAttribute+AttributeName%3D%22windowsaccountname%22+AttributeNamespace%3D%22http%3A%2F%2Fschemas.microsoft.com%2Fws%2F2008%2F06%2Fidentity%2Fclaims%22%3E%3Csaml%3AAttributeValue%3EFRACAS-O365%5Ckiller%3C%2Fsaml%3AAttributeValue%3E%3C%2Fsaml%3AAttribute%3E%3Csaml%3AAttribute+AttributeName%3D%22streetAddress%22+AttributeNamespace%3D%22http%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2005%2F05%2Fidentity%2Fclaims%22%3E%3Csaml%3AAttributeValue%3Estreet%0D%0AVia+Roggia+Arzona+1%3C%2Fsaml%3AAttributeValue%3E%3C%2Fsaml%3AAttribute%3E%3Csaml%3AAttribute+AttributeName%3D%22givenname%22+AttributeNamespace%3D%22http%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2005%2F05%2Fidentity%2Fclaims%22%3E%3Csaml%3AAttributeValue%3Ekiller%3C%2Fsaml%3AAttributeValue%3E%3C%2Fsaml%3AAttribute%3E%3C%2Fsaml%3AAttributeStatement%3E%3Csaml%3AAuthenticationStatement+AuthenticationMethod%3D%22urn%3Afederation%3Aauthentication%3Awindows%22+AuthenticationInstant%3D%222017-07-28T15%3A13%3A11.331Z%22%3E%3Csaml%3ASubject%3E%3Csaml%3ANameIdentifier+Format%3D%22urn%3Aoasis%3Anames%3Atc%3ASAML%3A1.1%3Anameid-format%3Aunspecified%22%3Ekiller%3C%2Fsaml%3ANameIdentifier%3E%3Csaml%3ASubjectConfirmation%3E%3Csaml%3AConfirmationMethod%3Eurn%3Aoasis%3Anames%3Atc%3ASAML%3A1.0%3Acm%3Abearer%3C%2Fsaml%3AConfirmationMethod%3E%3C%2Fsaml%3ASubjectConfirmation%3E%3C%2Fsaml%3ASubject%3E%3C%2Fsaml%3AAuthenticationStatement%3E%3Cds%3ASignature+xmlns%3Ads%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2F09%2Fxmldsig%23%22%3E%3Cds%3ASignedInfo%3E%3Cds%3ACanonicalizationMethod+Algorithm%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2F10%2Fxml-exc-c14n%23%22+%2F%3E%3Cds%3ASignatureMethod+Algorithm%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2F04%2Fxmldsig-more%23rsa-sha256%22+%2F%3E%3Cds%3AReference+URI%3D%22%23_6f8e1e8b-d3df-43b7-8b39-2681776af63d%22%3E%3Cds%3ATransforms%3E%3Cds%3ATransform+Algorithm%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2F09%2Fxmldsig%23enveloped-signature%22+%2F%3E%3Cds%3ATransform+Algorithm%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2F10%2Fxml-exc-c14n%23%22+%2F%3E%3C%2Fds%3ATransforms%3E%3Cds%3ADigestMethod+Algorithm%3D%22http%3A%2F%2Fwww.w3.org%2F2001%2F04%2Fxmlenc%23sha256%22+%2F%3E%3Cds%3ADigestValue%3EOLtXjvVHJ6qTpQEV6mXscC5jtEcWYvWLfllgaHcV33w%3D%3C%2Fds%3ADigestValue%3E%3C%2Fds%3AReference%3E%3C%2Fds%3ASignedInfo%3E%3Cds%3ASignatureValue%3EvGbDK%2FSLUBp0yrDXrGB3hwnwOq6uTgpXbOaAlzSs%2FxFvZf7ZRL062%2BwsAxqLRZWhAtI7563h6W9MA1T1ayToIAAyy5fU1PJ92i3uE0Dh%2B25OtJQh20g3T2%2FExPrygtgrSE%2FQOc4RJOhQtmws4YPIQjUP9QBHI3ET%2FwSDRIrEmhD3BwEmDkwdIDUCXPgE%2FRvlRwuZRhia1HUy9iitWPw4GTGew0LJ6gOzQRmUbLWgDMfh5z4m2jzHrgM32ylahnbundoP8%2B74UByAO%2FUszFLCM3LrtaIPAdJ7xE2w1qJjwiC6ZkmXLb6Uo%2F87bCEZBqy3yGgOURPdCTEpFdDmqeS0Aw%3D%3D%3C%2Fds%3ASignatureValue%3E%3CKeyInfo+xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2F09%2Fxmldsig%23%22%3E%3CX509Data%3E%3CX509Certificate%3EMIIDCDCCAfCgAwIBAgIQNz4YVbYAIJVFCc47HFD3RzANBgkqhkiG9w0BAQsFADBAMT4wPAYDVQQDEzVBREZTIFNpZ25pbmcgLSBzdHMuc3ViMi5mcmFjYXMzNjUubXNmdG9ubGluZXJlcHJvLmNvbTAeFw0xNTAzMzExMDQyMTNaFw0zNDA1MzAxMDQyMTNaMEAxPjA8BgNVBAMTNUFERlMgU2lnbmluZyAtIHN0cy5zdWIyLmZyYWNhczM2NS5tc2Z0b25saW5lcmVwcm8uY29tMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAxdjzB%2BwGV6hYekOvWwKoL%2FDFNBiLQsLx6w02FzcFnpGwR38gVTn%2Fglg9CNSsOT0riRM3%2FMwU8o2fwseQyVtv9Kee%2Fyvia8cB6GD0CARlYizb6GkJJzMvWkPSas1zpn10Bs3SBBgn0pvAKZCWWir5WJ7DRY32X2yo2do8mQftsoLGYsEU8%2Bjj9AMYQWaR3A86AEWjXoQY3AodfMMzdVFX%2BO%2FkjsvKcBfPqGRT6jUSGBOOaqzMOJBT39SueD8zePDW7SejBl7fRi4TLx5H6xuMldOAAH6oD70yIrobqosGG9X%2FLdijHajMSoaYzZIlG7fl4PCVvAjh1Dytw%2Fy8K70flQIDAQABMA0GCSqGSIb3DQEBCwUAA4IBAQBy08dAsSTKd2YlNE8bM4O5C2bFKR1YakR8L%2FzLEy8g%2BRNsKN5V%2FcIll0b%2Ftf9iQ5464nc%2BnM%2F%2F%2FU%2BUVxqT8ipeR7ThIPwuWX98cFZFQNNGkha4PaYap%2FosquEpRAJOcTqZf2K95ipeQ%2B5Hhw00mK0hcV1QT%2F7maTUqCHDfBCaD%2BuYAFvaNBXOYpdoIGM9cMk7Qjc%2FyowLDm%2BDpmJek54MWmN%2BiZ0YtDEhMSh%2F%2FQPFMLPT5Ucat%2BqRTen1HZNGdxfZ7NIIDL3dNKVDN%2BvDUbW7rjvPyxA8Rtj4JplI9ENqpzRq4m1sDWUTk2hJYw9Ec1kGo7AFKRmOS6DRbwUn5Ptdc%3C%2FX509Certificate%3E%3C%2FX509Data%3E%3C%2FKeyInfo%3E%3C%2Fds%3ASignature%3E%3C%2Fsaml%3AAssertion%3E%3C%2Ft%3ARequestedSecurityToken%3E%3Ct%3ATokenType%3Eurn%3Aoasis%3Anames%3Atc%3ASAML%3A1.0%3Aassertion%3C%2Ft%3ATokenType%3E%3Ct%3ARequestType%3Ehttp%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2005%2F02%2Ftrust%2FIssue%3C%2Ft%3ARequestType%3E%3Ct%3AKeyType%3Ehttp%3A%2F%2Fschemas.xmlsoap.org%2Fws%2F2005%2F05%2Fidentity%2FNoProofKey%3C%2Ft%3AKeyType%3E%3C%2Ft%3ARequestSecurityTokenResponse%3E&wctx=rm%3D0%26id%3Dpassive%26ru%3D%252fSampApp%252f";

        #endregion

        #region Token

        public static string Saml2TokenTwoSignatures
        {
            get
            {
                return @"<Assertion ID = ""_d60bd9ed-8aab-40c8-ba5f-f548c3401ae2"" IssueInstant=""2017-03-20T15:52:31.957Z"" Version=""2.0"" xmlns=""urn:oasis:names:tc:SAML:2.0:assertion"">
                        <Issuer>https://sts.windows.net/add29489-7269-41f4-8841-b63c95564420/</Issuer>
                        <Signature xmlns=""http://www.w3.org/2000/09/xmldsig#"">
                            <SignedInfo>
                                <CanonicalizationMethod Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#"" />
                                <SignatureMethod Algorithm=""http://www.w3.org/2001/04/xmldsig-more#rsa-sha256"" />
                                    <Reference URI=""#_d60bd9ed-8aab-40c8-ba5f-f548c3401ae2"">
                                        <Transforms>
                                            <Transform Algorithm=""http://www.w3.org/2000/09/xmldsig#enveloped-signature"" />
                                            <Transform Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#"" />
                                        </Transforms>
                                        <DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256"" />
                                        <DigestValue>Ytfkc60mLe1Zgu7TBQpMv8nJ1SVxT0ZjsFHaFqSB2VI=</DigestValue>
                                    </Reference>
                            </SignedInfo>
                            <SignatureValue>NRV7REVbDRflg616G6gYg0fAGTEw8BhtyPzqaU+kPQI35S1vpgt12VlQ57PkY7Rs0Jucx9npno+bQVMKN2DNhhnzs9qoNY2V3TcdJCcwaMexinHoFXHA0+J6+vR3RWTXhX+iAnfudtKThqbh/mECRLrjyTdy6L+qNkP7sALCWrSVwJVRmzkTOUF8zG4AKY9dQziec94Zv4S7G3cFgj/i7ok2DfBi7AEMCu1lh3dsQAMDeCvt7binhIH2D2ad3iCfYyifDGJ2ncn9hIyxrEiBdS8hZzWijcLs6+HQhVaz9yhZL9u/ZxSRaisXClMdqrLFjUghJ82sVfgQdp7SF165+Q==</SignatureValue>
                                <KeyInfo>
                                    <X509Data>
                                        <X509Certificate>MIIDBTCCAe2gAwIBAgIQY4RNIR0dX6dBZggnkhCRoDANBgkqhkiG9w0BAQsFADAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MB4XDTE3MDIxMzAwMDAwMFoXDTE5MDIxNDAwMDAwMFowLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldDCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAMBEizU1OJms31S/ry7iav/IICYVtQ2MRPhHhYknHImtU03sgVk1Xxub4GD7R15i9UWIGbzYSGKaUtGU9lP55wrfLpDjQjEgaXi4fE6mcZBwa9qc22is23B6R67KMcVyxyDWei+IP3sKmCcMX7Ibsg+ubZUpvKGxXZ27YgqFTPqCT2znD7K81YKfy+SVg3uW6epW114yZzClTQlarptYuE2mujxjZtx7ZUlwc9AhVi8CeiLwGO1wzTmpd/uctpner6oc335rvdJikNmc1cFKCK+2irew1bgUJHuN+LJA0y5iVXKvojiKZ2Ii7QKXn19Ssg1FoJ3x2NWA06wc0CnruLsCAwEAAaMhMB8wHQYDVR0OBBYEFDAr/HCMaGqmcDJa5oualVdWAEBEMA0GCSqGSIb3DQEBCwUAA4IBAQAiUke5mA86R/X4visjceUlv5jVzCn/SIq6Gm9/wCqtSxYvifRXxwNpQTOyvHhrY/IJLRUp2g9/fDELYd65t9Dp+N8SznhfB6/Cl7P7FRo99rIlj/q7JXa8UB/vLJPDlr+NREvAkMwUs1sDhL3kSuNBoxrbLC5Jo4es+juQLXd9HcRraE4U3UZVhUS2xqjFOfaGsCbJEqqkjihssruofaxdKT1CPzPMANfREFJznNzkpJt4H0aMDgVzq69NxZ7t1JiIuc43xRjeiixQMRGMi1mAB75fTyfFJ/rWQ5J/9kh0HMZVtHsqICBF1tHMTMIK5rwoweY0cuCIpN7A/zMOQtoD</X509Certificate>
                                    </X509Data>
                                </KeyInfo>
                        </Signature>
                        <Signature xmlns=""http://www.w3.org/2000/09/xmldsig#"">
                            <SignedInfo>
                                <CanonicalizationMethod Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#"" />
                                <SignatureMethod Algorithm=""http://www.w3.org/2001/04/xmldsig-more#rsa-sha256"" />
                                    <Reference URI=""#_d60bd9ed-8aab-40c8-ba5f-f548c3401ae2"">
                                        <Transforms>
                                            <Transform Algorithm=""http://www.w3.org/2000/09/xmldsig#enveloped-signature"" />
                                            <Transform Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#"" />
                                        </Transforms>
                                        <DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256"" />
                                        <DigestValue>Ytfkc60mLe1Zgu7TBQpMv8nJ1SVxT0ZjsFHaFqSB2VI=</DigestValue>
                                    </Reference>
                            </SignedInfo>
                            <SignatureValue>NRV7REVbDRflg616G6gYg0fAGTEw8BhtyPzqaU+kPQI35S1vpgt12VlQ57PkY7Rs0Jucx9npno+bQVMKN2DNhhnzs9qoNY2V3TcdJCcwaMexinHoFXHA0+J6+vR3RWTXhX+iAnfudtKThqbh/mECRLrjyTdy6L+qNkP7sALCWrSVwJVRmzkTOUF8zG4AKY9dQziec94Zv4S7G3cFgj/i7ok2DfBi7AEMCu1lh3dsQAMDeCvt7binhIH2D2ad3iCfYyifDGJ2ncn9hIyxrEiBdS8hZzWijcLs6+HQhVaz9yhZL9u/ZxSRaisXClMdqrLFjUghJ82sVfgQdp7SF165+Q==</SignatureValue>
                                <KeyInfo>
                                    <X509Data>
                                        <X509Certificate>MIIDBTCCAe2gAwIBAgIQY4RNIR0dX6dBZggnkhCRoDANBgkqhkiG9w0BAQsFADAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MB4XDTE3MDIxMzAwMDAwMFoXDTE5MDIxNDAwMDAwMFowLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldDCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAMBEizU1OJms31S/ry7iav/IICYVtQ2MRPhHhYknHImtU03sgVk1Xxub4GD7R15i9UWIGbzYSGKaUtGU9lP55wrfLpDjQjEgaXi4fE6mcZBwa9qc22is23B6R67KMcVyxyDWei+IP3sKmCcMX7Ibsg+ubZUpvKGxXZ27YgqFTPqCT2znD7K81YKfy+SVg3uW6epW114yZzClTQlarptYuE2mujxjZtx7ZUlwc9AhVi8CeiLwGO1wzTmpd/uctpner6oc335rvdJikNmc1cFKCK+2irew1bgUJHuN+LJA0y5iVXKvojiKZ2Ii7QKXn19Ssg1FoJ3x2NWA06wc0CnruLsCAwEAAaMhMB8wHQYDVR0OBBYEFDAr/HCMaGqmcDJa5oualVdWAEBEMA0GCSqGSIb3DQEBCwUAA4IBAQAiUke5mA86R/X4visjceUlv5jVzCn/SIq6Gm9/wCqtSxYvifRXxwNpQTOyvHhrY/IJLRUp2g9/fDELYd65t9Dp+N8SznhfB6/Cl7P7FRo99rIlj/q7JXa8UB/vLJPDlr+NREvAkMwUs1sDhL3kSuNBoxrbLC5Jo4es+juQLXd9HcRraE4U3UZVhUS2xqjFOfaGsCbJEqqkjihssruofaxdKT1CPzPMANfREFJznNzkpJt4H0aMDgVzq69NxZ7t1JiIuc43xRjeiixQMRGMi1mAB75fTyfFJ/rWQ5J/9kh0HMZVtHsqICBF1tHMTMIK5rwoweY0cuCIpN7A/zMOQtoD</X509Certificate>
                                    </X509Data>
                                </KeyInfo>
                        </Signature>
                        <Subject>
                            <NameID Format=""urn:oasis:names:tc:SAML:2.0:nameid-format:persistent"">RrX3SPSxDw6z4KHaKB2V_mnv0G-LbRZdYvo1RQa1L7s</NameID>
                            <SubjectConfirmation Method=""urn:oasis:names:tc:SAML:2.0:cm:bearer"" />
                        </Subject>
                        <Conditions NotBefore=""2017-03-20T15:47:31.957Z"" NotOnOrAfter=""2017-03-20T16:47:31.957Z"">
                            <AudienceRestriction>
                                <Audience>spn:fe78e0b4-6fe7-47e6-812c-fb75cee266a4</Audience>
                            </AudienceRestriction>
                        </Conditions>
                        <AttributeStatement>
                            <Attribute Name=""http://schemas.microsoft.com/identity/claims/tenantid"">
                                <AttributeValue>add29489-7269-41f4-8841-b63c95564420</AttributeValue>
                            </Attribute>
                            <Attribute Name=""http://schemas.microsoft.com/identity/claims/objectidentifier"">
                                <AttributeValue>d1ad9ce7-b322-4221-ab74-1e1011e1bbcb</AttributeValue>
                            </Attribute>
                            <Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"">
                                <AttributeValue>User1@Cyrano.onmicrosoft.com</AttributeValue>
                            </Attribute>
                            <Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname"">
                                <AttributeValue>1</AttributeValue>
                            </Attribute>
                            <Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"">
                                <AttributeValue>User</AttributeValue>
                            </Attribute>
                            <Attribute Name=""http://schemas.microsoft.com/identity/claims/displayname"">
                                <AttributeValue>User1</AttributeValue>
                            </Attribute>
                            <Attribute Name=""http://schemas.microsoft.com/identity/claims/identityprovider"">
                                <AttributeValue>https://sts.windows.net/add29489-7269-41f4-8841-b63c95564420/</AttributeValue>
                            </Attribute>
                        </AttributeStatement>
                        <AuthnStatement AuthnInstant=""2017-03-20T15:52:31.551Z"">
                            <AuthnContext>
                                <AuthnContextClassRef>urn:oasis:names:tc:SAML:2.0:ac:classes:Password</AuthnContextClassRef>
                            </AuthnContext>
                        </AuthnStatement>
                    </Assertion>";
            }
        }

        public static string Saml2TokenValidFormated
        {
            get
            {
                return @"<Assertion ID = ""_d60bd9ed-8aab-40c8-ba5f-f548c3401ae2"" IssueInstant=""2017-03-20T15:52:31.957Z"" Version=""2.0"" xmlns=""urn:oasis:names:tc:SAML:2.0:assertion"">
                        <Issuer>https://sts.windows.net/add29489-7269-41f4-8841-b63c95564420/</Issuer>
                        <Signature xmlns=""http://www.w3.org/2000/09/xmldsig#"">
                            <SignedInfo>
                                <CanonicalizationMethod Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#"" />
                                <SignatureMethod Algorithm=""http://www.w3.org/2001/04/xmldsig-more#rsa-sha256"" />
                                    <Reference URI=""#_d60bd9ed-8aab-40c8-ba5f-f548c3401ae2"">
                                        <Transforms>
                                            <Transform Algorithm=""http://www.w3.org/2000/09/xmldsig#enveloped-signature"" />
                                            <Transform Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#"" />
                                        </Transforms>
                                        <DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256"" />
                                        <DigestValue>Ytfkc60mLe1Zgu7TBQpMv8nJ1SVxT0ZjsFHaFqSB2VI=</DigestValue>
                                    </Reference>
                            </SignedInfo>
                            <SignatureValue>NRV7REVbDRflg616G6gYg0fAGTEw8BhtyPzqaU+kPQI35S1vpgt12VlQ57PkY7Rs0Jucx9npno+bQVMKN2DNhhnzs9qoNY2V3TcdJCcwaMexinHoFXHA0+J6+vR3RWTXhX+iAnfudtKThqbh/mECRLrjyTdy6L+qNkP7sALCWrSVwJVRmzkTOUF8zG4AKY9dQziec94Zv4S7G3cFgj/i7ok2DfBi7AEMCu1lh3dsQAMDeCvt7binhIH2D2ad3iCfYyifDGJ2ncn9hIyxrEiBdS8hZzWijcLs6+HQhVaz9yhZL9u/ZxSRaisXClMdqrLFjUghJ82sVfgQdp7SF165+Q==</SignatureValue>
                                <KeyInfo>
                                    <X509Data>
                                        <X509Certificate>MIIDBTCCAe2gAwIBAgIQY4RNIR0dX6dBZggnkhCRoDANBgkqhkiG9w0BAQsFADAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MB4XDTE3MDIxMzAwMDAwMFoXDTE5MDIxNDAwMDAwMFowLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldDCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAMBEizU1OJms31S/ry7iav/IICYVtQ2MRPhHhYknHImtU03sgVk1Xxub4GD7R15i9UWIGbzYSGKaUtGU9lP55wrfLpDjQjEgaXi4fE6mcZBwa9qc22is23B6R67KMcVyxyDWei+IP3sKmCcMX7Ibsg+ubZUpvKGxXZ27YgqFTPqCT2znD7K81YKfy+SVg3uW6epW114yZzClTQlarptYuE2mujxjZtx7ZUlwc9AhVi8CeiLwGO1wzTmpd/uctpner6oc335rvdJikNmc1cFKCK+2irew1bgUJHuN+LJA0y5iVXKvojiKZ2Ii7QKXn19Ssg1FoJ3x2NWA06wc0CnruLsCAwEAAaMhMB8wHQYDVR0OBBYEFDAr/HCMaGqmcDJa5oualVdWAEBEMA0GCSqGSIb3DQEBCwUAA4IBAQAiUke5mA86R/X4visjceUlv5jVzCn/SIq6Gm9/wCqtSxYvifRXxwNpQTOyvHhrY/IJLRUp2g9/fDELYd65t9Dp+N8SznhfB6/Cl7P7FRo99rIlj/q7JXa8UB/vLJPDlr+NREvAkMwUs1sDhL3kSuNBoxrbLC5Jo4es+juQLXd9HcRraE4U3UZVhUS2xqjFOfaGsCbJEqqkjihssruofaxdKT1CPzPMANfREFJznNzkpJt4H0aMDgVzq69NxZ7t1JiIuc43xRjeiixQMRGMi1mAB75fTyfFJ/rWQ5J/9kh0HMZVtHsqICBF1tHMTMIK5rwoweY0cuCIpN7A/zMOQtoD</X509Certificate>
                                    </X509Data>
                                </KeyInfo>
                        </Signature>
                        <Subject>
                            <NameID Format=""urn:oasis:names:tc:SAML:2.0:nameid-format:persistent"">RrX3SPSxDw6z4KHaKB2V_mnv0G-LbRZdYvo1RQa1L7s</NameID>
                            <SubjectConfirmation Method=""urn:oasis:names:tc:SAML:2.0:cm:bearer"" />
                        </Subject>
                        <Conditions NotBefore=""2017-03-20T15:47:31.957Z"" NotOnOrAfter=""2017-03-20T16:47:31.957Z"">
                            <AudienceRestriction>
                                <Audience>spn:fe78e0b4-6fe7-47e6-812c-fb75cee266a4</Audience>
                            </AudienceRestriction>
                        </Conditions>
                        <AttributeStatement>
                            <Attribute Name=""http://schemas.microsoft.com/identity/claims/tenantid"">
                                <AttributeValue>add29489-7269-41f4-8841-b63c95564420</AttributeValue>
                            </Attribute>
                            <Attribute Name=""http://schemas.microsoft.com/identity/claims/objectidentifier"">
                                <AttributeValue>d1ad9ce7-b322-4221-ab74-1e1011e1bbcb</AttributeValue>
                            </Attribute>
                            <Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"">
                                <AttributeValue>User1@Cyrano.onmicrosoft.com</AttributeValue>
                            </Attribute>
                            <Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname"">
                                <AttributeValue>1</AttributeValue>
                            </Attribute>
                            <Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"">
                                <AttributeValue>User</AttributeValue>
                            </Attribute>
                            <Attribute Name=""http://schemas.microsoft.com/identity/claims/displayname"">
                                <AttributeValue>User1</AttributeValue>
                            </Attribute>
                            <Attribute Name=""http://schemas.microsoft.com/identity/claims/identityprovider"">
                                <AttributeValue>https://sts.windows.net/add29489-7269-41f4-8841-b63c95564420/</AttributeValue>
                            </Attribute>
                        </AttributeStatement>
                        <AuthnStatement AuthnInstant=""2017-03-20T15:52:31.551Z"">
                            <AuthnContext>
                                <AuthnContextClassRef>urn:oasis:names:tc:SAML:2.0:ac:classes:Password</AuthnContextClassRef>
                            </AuthnContext>
                        </AuthnStatement>
                    </Assertion>";
            }
        }

        public static string Saml2TokenValidSignatureNOTFormated
        {
            get
            {
                return @"<Assertion ID = ""_d60bd9ed-8aab-40c8-ba5f-f548c3401ae2"" IssueInstant=""2017-03-20T15:52:31.957Z"" Version=""2.0"" xmlns=""urn:oasis:names:tc:SAML:2.0:assertion"">
                            <Issuer>https://sts.windows.net/add29489-7269-41f4-8841-b63c95564420/</Issuer>
                            <Signature xmlns=""http://www.w3.org/2000/09/xmldsig#""><SignedInfo><CanonicalizationMethod Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#"" /><SignatureMethod Algorithm=""http://www.w3.org/2001/04/xmldsig-more#rsa-sha256"" /><Reference URI=""#_d60bd9ed-8aab-40c8-ba5f-f548c3401ae2""><Transforms><Transform Algorithm=""http://www.w3.org/2000/09/xmldsig#enveloped-signature"" /><Transform Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#"" /></Transforms><DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256"" /><DigestValue>Ytfkc60mLe1Zgu7TBQpMv8nJ1SVxT0ZjsFHaFqSB2VI=</DigestValue></Reference></SignedInfo><SignatureValue>NRV7REVbDRflg616G6gYg0fAGTEw8BhtyPzqaU+kPQI35S1vpgt12VlQ57PkY7Rs0Jucx9npno+bQVMKN2DNhhnzs9qoNY2V3TcdJCcwaMexinHoFXHA0+J6+vR3RWTXhX+iAnfudtKThqbh/mECRLrjyTdy6L+qNkP7sALCWrSVwJVRmzkTOUF8zG4AKY9dQziec94Zv4S7G3cFgj/i7ok2DfBi7AEMCu1lh3dsQAMDeCvt7binhIH2D2ad3iCfYyifDGJ2ncn9hIyxrEiBdS8hZzWijcLs6+HQhVaz9yhZL9u/ZxSRaisXClMdqrLFjUghJ82sVfgQdp7SF165+Q==</SignatureValue><KeyInfo><X509Data><X509Certificate>MIIDBTCCAe2gAwIBAgIQY4RNIR0dX6dBZggnkhCRoDANBgkqhkiG9w0BAQsFADAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MB4XDTE3MDIxMzAwMDAwMFoXDTE5MDIxNDAwMDAwMFowLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldDCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAMBEizU1OJms31S/ry7iav/IICYVtQ2MRPhHhYknHImtU03sgVk1Xxub4GD7R15i9UWIGbzYSGKaUtGU9lP55wrfLpDjQjEgaXi4fE6mcZBwa9qc22is23B6R67KMcVyxyDWei+IP3sKmCcMX7Ibsg+ubZUpvKGxXZ27YgqFTPqCT2znD7K81YKfy+SVg3uW6epW114yZzClTQlarptYuE2mujxjZtx7ZUlwc9AhVi8CeiLwGO1wzTmpd/uctpner6oc335rvdJikNmc1cFKCK+2irew1bgUJHuN+LJA0y5iVXKvojiKZ2Ii7QKXn19Ssg1FoJ3x2NWA06wc0CnruLsCAwEAAaMhMB8wHQYDVR0OBBYEFDAr/HCMaGqmcDJa5oualVdWAEBEMA0GCSqGSIb3DQEBCwUAA4IBAQAiUke5mA86R/X4visjceUlv5jVzCn/SIq6Gm9/wCqtSxYvifRXxwNpQTOyvHhrY/IJLRUp2g9/fDELYd65t9Dp+N8SznhfB6/Cl7P7FRo99rIlj/q7JXa8UB/vLJPDlr+NREvAkMwUs1sDhL3kSuNBoxrbLC5Jo4es+juQLXd9HcRraE4U3UZVhUS2xqjFOfaGsCbJEqqkjihssruofaxdKT1CPzPMANfREFJznNzkpJt4H0aMDgVzq69NxZ7t1JiIuc43xRjeiixQMRGMi1mAB75fTyfFJ/rWQ5J/9kh0HMZVtHsqICBF1tHMTMIK5rwoweY0cuCIpN7A/zMOQtoD</X509Certificate></X509Data></KeyInfo></Signature>
                            <Subject>
                                <NameID Format=""urn:oasis:names:tc:SAML:2.0:nameid-format:persistent"">RrX3SPSxDw6z4KHaKB2V_mnv0G-LbRZdYvo1RQa1L7s</NameID>
                                <SubjectConfirmation Method=""urn:oasis:names:tc:SAML:2.0:cm:bearer"" />
                            </Subject>
                            <Conditions NotBefore=""2017-03-20T15:47:31.957Z"" NotOnOrAfter=""2017-03-20T16:47:31.957Z"">
                                <AudienceRestriction>
                                    <Audience>spn:fe78e0b4-6fe7-47e6-812c-fb75cee266a4</Audience>
                                </AudienceRestriction>
                            </Conditions>
                            <AttributeStatement>
                                <Attribute Name=""http://schemas.microsoft.com/identity/claims/tenantid"">
                                    <AttributeValue>add29489-7269-41f4-8841-b63c95564420</AttributeValue>
                                </Attribute>
                                <Attribute Name=""http://schemas.microsoft.com/identity/claims/objectidentifier"">
                                    <AttributeValue>d1ad9ce7-b322-4221-ab74-1e1011e1bbcb</AttributeValue>
                                </Attribute>
                                <Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"">
                                    <AttributeValue>User1@Cyrano.onmicrosoft.com</AttributeValue>
                                </Attribute>
                                <Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname"">
                                    <AttributeValue>1</AttributeValue>
                                </Attribute>
                                <Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"">
                                    <AttributeValue>User</AttributeValue>
                                </Attribute>
                                <Attribute Name=""http://schemas.microsoft.com/identity/claims/displayname"">
                                    <AttributeValue>User1</AttributeValue>
                                </Attribute>
                                <Attribute Name=""http://schemas.microsoft.com/identity/claims/identityprovider"">
                                    <AttributeValue>https://sts.windows.net/add29489-7269-41f4-8841-b63c95564420/</AttributeValue>
                                </Attribute>
                            </AttributeStatement>
                            <AuthnStatement AuthnInstant=""2017-03-20T15:52:31.551Z"">
                                <AuthnContext>
                                    <AuthnContextClassRef>urn:oasis:names:tc:SAML:2.0:ac:classes:Password</AuthnContextClassRef>
                                </AuthnContext>
                            </AuthnStatement>
                        </Assertion>";
            }
        }

        public static string Saml2TokenValidSigned
        {
            get { return @"<Assertion ID = ""_d60bd9ed-8aab-40c8-ba5f-f548c3401ae2"" IssueInstant=""2017-03-20T15:52:31.957Z"" Version=""2.0"" xmlns=""urn:oasis:names:tc:SAML:2.0:assertion""><Issuer>https://sts.windows.net/add29489-7269-41f4-8841-b63c95564420/</Issuer><Signature xmlns=""http://www.w3.org/2000/09/xmldsig#""><SignedInfo><CanonicalizationMethod Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#"" /><SignatureMethod Algorithm=""http://www.w3.org/2001/04/xmldsig-more#rsa-sha256"" /><Reference URI=""#_d60bd9ed-8aab-40c8-ba5f-f548c3401ae2""><Transforms><Transform Algorithm=""http://www.w3.org/2000/09/xmldsig#enveloped-signature"" /><Transform Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#"" /></Transforms><DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256"" /><DigestValue>Ytfkc60mLe1Zgu7TBQpMv8nJ1SVxT0ZjsFHaFqSB2VI=</DigestValue></Reference></SignedInfo><SignatureValue>NRV7REVbDRflg616G6gYg0fAGTEw8BhtyPzqaU+kPQI35S1vpgt12VlQ57PkY7Rs0Jucx9npno+bQVMKN2DNhhnzs9qoNY2V3TcdJCcwaMexinHoFXHA0+J6+vR3RWTXhX+iAnfudtKThqbh/mECRLrjyTdy6L+qNkP7sALCWrSVwJVRmzkTOUF8zG4AKY9dQziec94Zv4S7G3cFgj/i7ok2DfBi7AEMCu1lh3dsQAMDeCvt7binhIH2D2ad3iCfYyifDGJ2ncn9hIyxrEiBdS8hZzWijcLs6+HQhVaz9yhZL9u/ZxSRaisXClMdqrLFjUghJ82sVfgQdp7SF165+Q==</SignatureValue><KeyInfo><X509Data><X509Certificate>MIIDBTCCAe2gAwIBAgIQY4RNIR0dX6dBZggnkhCRoDANBgkqhkiG9w0BAQsFADAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MB4XDTE3MDIxMzAwMDAwMFoXDTE5MDIxNDAwMDAwMFowLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldDCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAMBEizU1OJms31S/ry7iav/IICYVtQ2MRPhHhYknHImtU03sgVk1Xxub4GD7R15i9UWIGbzYSGKaUtGU9lP55wrfLpDjQjEgaXi4fE6mcZBwa9qc22is23B6R67KMcVyxyDWei+IP3sKmCcMX7Ibsg+ubZUpvKGxXZ27YgqFTPqCT2znD7K81YKfy+SVg3uW6epW114yZzClTQlarptYuE2mujxjZtx7ZUlwc9AhVi8CeiLwGO1wzTmpd/uctpner6oc335rvdJikNmc1cFKCK+2irew1bgUJHuN+LJA0y5iVXKvojiKZ2Ii7QKXn19Ssg1FoJ3x2NWA06wc0CnruLsCAwEAAaMhMB8wHQYDVR0OBBYEFDAr/HCMaGqmcDJa5oualVdWAEBEMA0GCSqGSIb3DQEBCwUAA4IBAQAiUke5mA86R/X4visjceUlv5jVzCn/SIq6Gm9/wCqtSxYvifRXxwNpQTOyvHhrY/IJLRUp2g9/fDELYd65t9Dp+N8SznhfB6/Cl7P7FRo99rIlj/q7JXa8UB/vLJPDlr+NREvAkMwUs1sDhL3kSuNBoxrbLC5Jo4es+juQLXd9HcRraE4U3UZVhUS2xqjFOfaGsCbJEqqkjihssruofaxdKT1CPzPMANfREFJznNzkpJt4H0aMDgVzq69NxZ7t1JiIuc43xRjeiixQMRGMi1mAB75fTyfFJ/rWQ5J/9kh0HMZVtHsqICBF1tHMTMIK5rwoweY0cuCIpN7A/zMOQtoD</X509Certificate></X509Data></KeyInfo></Signature><Subject><NameID Format=""urn:oasis:names:tc:SAML:2.0:nameid-format:persistent"">RrX3SPSxDw6z4KHaKB2V_mnv0G-LbRZdYvo1RQa1L7s</NameID><SubjectConfirmation Method=""urn:oasis:names:tc:SAML:2.0:cm:bearer"" /></Subject><Conditions NotBefore=""2017-03-20T15:47:31.957Z"" NotOnOrAfter=""2017-03-20T16:47:31.957Z""><AudienceRestriction><Audience>spn:fe78e0b4-6fe7-47e6-812c-fb75cee266a4</Audience></AudienceRestriction></Conditions><AttributeStatement><Attribute Name=""http://schemas.microsoft.com/identity/claims/tenantid""><AttributeValue>add29489-7269-41f4-8841-b63c95564420</AttributeValue></Attribute><Attribute Name=""http://schemas.microsoft.com/identity/claims/objectidentifier""><AttributeValue>d1ad9ce7-b322-4221-ab74-1e1011e1bbcb</AttributeValue></Attribute><Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name""><AttributeValue>User1@Cyrano.onmicrosoft.com</AttributeValue></Attribute><Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname""><AttributeValue>1</AttributeValue></Attribute><Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname""><AttributeValue>User</AttributeValue></Attribute><Attribute Name=""http://schemas.microsoft.com/identity/claims/displayname""><AttributeValue>User1</AttributeValue></Attribute><Attribute Name=""http://schemas.microsoft.com/identity/claims/identityprovider""><AttributeValue>https://sts.windows.net/add29489-7269-41f4-8841-b63c95564420/</AttributeValue></Attribute></AttributeStatement><AuthnStatement AuthnInstant=""2017-03-20T15:52:31.551Z""><AuthnContext><AuthnContextClassRef>urn:oasis:names:tc:SAML:2.0:ac:classes:Password</AuthnContextClassRef></AuthnContext></AuthnStatement></Assertion>"; }
        }

        public static string Saml2Valid
        {
            get
            {
                return @"<Assertion ID=""_edc15efd-1117-4bf9-89da-28b1663fb890"" IssueInstant=""2017-04-23T16:16:17.348Z"" Version=""2.0"" xmlns=""urn:oasis:names:tc:SAML:2.0:assertion""><Issuer>https://sts.windows.net/add29489-7269-41f4-8841-b63c95564420/</Issuer><Signature xmlns=""http://www.w3.org/2000/09/xmldsig#""><SignedInfo><CanonicalizationMethod Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#""/><SignatureMethod Algorithm=""http://www.w3.org/2001/04/xmldsig-more#rsa-sha256""/><Reference URI=""#_edc15efd-1117-4bf9-89da-28b1663fb890""><Transforms><Transform Algorithm=""http://www.w3.org/2000/09/xmldsig#enveloped-signature""/><Transform Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#""/></Transforms><DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256""/><DigestValue>DO8QQoO629ApWPV3LiY2epQSv+I82iChybeRrXbhgtw=</DigestValue></Reference></SignedInfo><SignatureValue>O8JNyVKm9I7kMqlsaBgLCNwHA0qdXv34YHBVfg217lgeKkMC5taLU/EH7UeeMtapU6zMafcYoCH+Bp9zoqDpflgs78Hkjgn/dEUtjPFn7211VXClcTNqk+yhqXWtu6SKrabeIhKCKtoMA9lUAB4D6ABesb6MpwbM/ULq7T16tycZ3X//iXHeOiMwNiUAePYF22fmgrqRSDRHyLPtiLskP4UMksWJBrXUV96e9EU9aEciCvYpzMDv/VFUOCLiEkBqCdAtPVwVun+5eRk9zEh6qscWi0kAgFl3W3JhugcTTuGQYHXYVIHxbd5O33MwFIMUOmGrI1EXuk+cHIq2KUtSLg==</SignatureValue><KeyInfo><X509Data><X509Certificate>MIIDBTCCAe2gAwIBAgIQY4RNIR0dX6dBZggnkhCRoDANBgkqhkiG9w0BAQsFADAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MB4XDTE3MDIxMzAwMDAwMFoXDTE5MDIxNDAwMDAwMFowLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldDCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAMBEizU1OJms31S/ry7iav/IICYVtQ2MRPhHhYknHImtU03sgVk1Xxub4GD7R15i9UWIGbzYSGKaUtGU9lP55wrfLpDjQjEgaXi4fE6mcZBwa9qc22is23B6R67KMcVyxyDWei+IP3sKmCcMX7Ibsg+ubZUpvKGxXZ27YgqFTPqCT2znD7K81YKfy+SVg3uW6epW114yZzClTQlarptYuE2mujxjZtx7ZUlwc9AhVi8CeiLwGO1wzTmpd/uctpner6oc335rvdJikNmc1cFKCK+2irew1bgUJHuN+LJA0y5iVXKvojiKZ2Ii7QKXn19Ssg1FoJ3x2NWA06wc0CnruLsCAwEAAaMhMB8wHQYDVR0OBBYEFDAr/HCMaGqmcDJa5oualVdWAEBEMA0GCSqGSIb3DQEBCwUAA4IBAQAiUke5mA86R/X4visjceUlv5jVzCn/SIq6Gm9/wCqtSxYvifRXxwNpQTOyvHhrY/IJLRUp2g9/fDELYd65t9Dp+N8SznhfB6/Cl7P7FRo99rIlj/q7JXa8UB/vLJPDlr+NREvAkMwUs1sDhL3kSuNBoxrbLC5Jo4es+juQLXd9HcRraE4U3UZVhUS2xqjFOfaGsCbJEqqkjihssruofaxdKT1CPzPMANfREFJznNzkpJt4H0aMDgVzq69NxZ7t1JiIuc43xRjeiixQMRGMi1mAB75fTyfFJ/rWQ5J/9kh0HMZVtHsqICBF1tHMTMIK5rwoweY0cuCIpN7A/zMOQtoD</X509Certificate></X509Data></KeyInfo></Signature><Subject><NameID Format=""urn:oasis:names:tc:SAML:2.0:nameid-format:persistent"">RrX3SPSxDw6z4KHaKB2V_mnv0G-LbRZdYvo1RQa1L7s</NameID><SubjectConfirmation Method=""urn:oasis:names:tc:SAML:2.0:cm:bearer""/></Subject><Conditions NotBefore=""2017-04-23T16:11:17.348Z"" NotOnOrAfter=""2017-04-23T17:11:17.348Z""><AudienceRestriction><Audience>spn:fe78e0b4-6fe7-47e6-812c-fb75cee266a4</Audience></AudienceRestriction></Conditions><AttributeStatement><Attribute Name=""http://schemas.microsoft.com/identity/claims/tenantid""><AttributeValue>add29489-7269-41f4-8841-b63c95564420</AttributeValue></Attribute><Attribute Name=""http://schemas.microsoft.com/identity/claims/objectidentifier""><AttributeValue>d1ad9ce7-b322-4221-ab74-1e1011e1bbcb</AttributeValue></Attribute><Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name""><AttributeValue>User1@Cyrano.onmicrosoft.com</AttributeValue></Attribute><Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname""><AttributeValue>1</AttributeValue></Attribute><Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname""><AttributeValue>User</AttributeValue></Attribute><Attribute Name=""http://schemas.microsoft.com/identity/claims/displayname""><AttributeValue>User1</AttributeValue></Attribute><Attribute Name=""http://schemas.microsoft.com/identity/claims/identityprovider""><AttributeValue>https://sts.windows.net/add29489-7269-41f4-8841-b63c95564420/</AttributeValue></Attribute><Attribute Name=""http://schemas.microsoft.com/claims/authnmethodsreferences""><AttributeValue>http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/password</AttributeValue></Attribute></AttributeStatement><AuthnStatement AuthnInstant=""2017-04-23T16:16:17.270Z""><AuthnContext><AuthnContextClassRef>urn:oasis:names:tc:SAML:2.0:ac:classes:Password</AuthnContextClassRef></AuthnContext></AuthnStatement></Assertion>";
            }
        }

        public static string TokenDummy
        {
            get
            {
                return "<token>dummy</token>";
            }
        }

        #endregion
    }
}
