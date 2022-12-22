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

using System.Collections.Generic;
using System.Security.Claims;

namespace System.IdentityModel.Tokens.Jwt
{
    /// <summary>
    /// Defines the inbound and outbound mapping for claim claim types from jwt to .net claim 
    /// </summary>
    internal static class ClaimTypeMapping
    {
        // This is the short to long mapping.
        // key      is the long  claim type
        // value    is the short claim type
        private static Dictionary<string, string> shortToLongClaimTypeMapping = new Dictionary<string, string>
        {
            { JwtRegisteredClaimNames.Actort, ClaimTypes.Actor },
            { JwtRegisteredClaimNames.Birthdate, ClaimTypes.DateOfBirth },
            { JwtRegisteredClaimNames.Email, ClaimTypes.Email },
            { JwtRegisteredClaimNames.FamilyName, ClaimTypes.Surname },
            { JwtRegisteredClaimNames.Gender, ClaimTypes.Gender },
            { JwtRegisteredClaimNames.GivenName, ClaimTypes.GivenName },
            { JwtRegisteredClaimNames.NameId, ClaimTypes.NameIdentifier },
            { JwtRegisteredClaimNames.Sub, ClaimTypes.NameIdentifier },
            { JwtRegisteredClaimNames.Website, ClaimTypes.Webpage },
            { JwtRegisteredClaimNames.UniqueName, ClaimTypes.Name },
            { "oid", "http://schemas.microsoft.com/identity/claims/objectidentifier" },
            { "scp", "http://schemas.microsoft.com/identity/claims/scope" },
            { "tid", "http://schemas.microsoft.com/identity/claims/tenantid" },
            { "acr", "http://schemas.microsoft.com/claims/authnclassreference" },
            { "adfs1email", "http://schemas.xmlsoap.org/claims/EmailAddress" },
            { "adfs1upn", "http://schemas.xmlsoap.org/claims/UPN" },
            { "amr", "http://schemas.microsoft.com/claims/authnmethodsreferences" },
            { "authmethod", ClaimTypes.AuthenticationMethod },
            { "certapppolicy", "http://schemas.microsoft.com/2012/12/certificatecontext/extension/applicationpolicy" },
            { "certauthoritykeyidentifier", "http://schemas.microsoft.com/2012/12/certificatecontext/extension/authoritykeyidentifier" },
            { "certbasicconstraints", "http://schemas.microsoft.com/2012/12/certificatecontext/extension/basicconstraints" },
            { "certeku", "http://schemas.microsoft.com/2012/12/certificatecontext/extension/eku" },
            { "certissuer", "http://schemas.microsoft.com/2012/12/certificatecontext/field/issuer" },
            { "certissuername", "http://schemas.microsoft.com/2012/12/certificatecontext/field/issuername" },
            { "certkeyusage", "http://schemas.microsoft.com/2012/12/certificatecontext/extension/keyusage" },
            { "certnotafter", "http://schemas.microsoft.com/2012/12/certificatecontext/field/notafter" },
            { "certnotbefore", "http://schemas.microsoft.com/2012/12/certificatecontext/field/notbefore" },
            { "certpolicy", "http://schemas.microsoft.com/2012/12/certificatecontext/extension/certificatepolicy" },
            { "certpublickey", ClaimTypes.Rsa },
            { "certrawdata", "http://schemas.microsoft.com/2012/12/certificatecontext/field/rawdata" },
            { "certserialnumber", ClaimTypes.SerialNumber },
            { "certsignaturealgorithm", "http://schemas.microsoft.com/2012/12/certificatecontext/field/signaturealgorithm" },
            { "certsubject", "http://schemas.microsoft.com/2012/12/certificatecontext/field/subject" },
            { "certsubjectaltname", "http://schemas.microsoft.com/2012/12/certificatecontext/extension/san" },
            { "certsubjectkeyidentifier", "http://schemas.microsoft.com/2012/12/certificatecontext/extension/subjectkeyidentifier" },
            { "certsubjectname", "http://schemas.microsoft.com/2012/12/certificatecontext/field/subjectname" },
            { "certtemplateinformation", "http://schemas.microsoft.com/2012/12/certificatecontext/extension/certificatetemplateinformation" },
            { "certtemplatename", "http://schemas.microsoft.com/2012/12/certificatecontext/extension/certificatetemplatename" },
            { "certthumbprint", ClaimTypes.Thumbprint },
            { "certx509version", "http://schemas.microsoft.com/2012/12/certificatecontext/field/x509version" },
            { "clientapplication", "http://schemas.microsoft.com/2012/01/requestcontext/claims/x-ms-client-application" },
            { "clientip", "http://schemas.microsoft.com/2012/01/requestcontext/claims/x-ms-client-ip" },
            { "clientuseragent", "http://schemas.microsoft.com/2012/01/requestcontext/claims/x-ms-client-user-agent" },
            { "commonname", "http://schemas.xmlsoap.org/claims/CommonName" },
            { "denyonlyprimarygroupsid", ClaimTypes.DenyOnlyPrimaryGroupSid },
            { "denyonlyprimarysid", ClaimTypes.DenyOnlyPrimarySid },
            { "denyonlysid", ClaimTypes.DenyOnlySid },
            { "devicedispname", "http://schemas.microsoft.com/2012/01/devicecontext/claims/displayname" },
            { "deviceid", "http://schemas.microsoft.com/2012/01/devicecontext/claims/identifier" },
            { "deviceismanaged", "http://schemas.microsoft.com/2012/01/devicecontext/claims/ismanaged" },
            { "deviceostype", "http://schemas.microsoft.com/2012/01/devicecontext/claims/ostype" },
            { "deviceosver", "http://schemas.microsoft.com/2012/01/devicecontext/claims/osversion" },
            { "deviceowner", "http://schemas.microsoft.com/2012/01/devicecontext/claims/userowner" },
            { "deviceregid", "http://schemas.microsoft.com/2012/01/devicecontext/claims/registrationid" },
            { "endpointpath", "http://schemas.microsoft.com/2012/01/requestcontext/claims/x-ms-endpoint-absolute-path" },
            { "forwardedclientip", "http://schemas.microsoft.com/2012/01/requestcontext/claims/x-ms-forwarded-client-ip" },
            { "group", "http://schemas.xmlsoap.org/claims/Group" },
            { "groupsid", ClaimTypes.GroupSid },
            { "idp", "http://schemas.microsoft.com/identity/claims/identityprovider" },
            { "insidecorporatenetwork", "http://schemas.microsoft.com/ws/2012/01/insidecorporatenetwork" },
            { "isregistereduser", "http://schemas.microsoft.com/2012/01/devicecontext/claims/isregistereduser" },
            { "ppid", "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/privatepersonalidentifier" },
            { "primarygroupsid", ClaimTypes.PrimaryGroupSid },
            { "primarysid", ClaimTypes.PrimarySid },
            { "proxy", "http://schemas.microsoft.com/2012/01/requestcontext/claims/x-ms-proxy" },
            { "pwdchgurl", "http://schemas.microsoft.com/ws/2012/01/passwordchangeurl" },
            { "pwdexpdays", "http://schemas.microsoft.com/ws/2012/01/passwordexpirationdays" },
            { "pwdexptime", "http://schemas.microsoft.com/ws/2012/01/passwordexpirationtime" },
            { "relyingpartytrustid", "http://schemas.microsoft.com/2012/01/requestcontext/claims/relyingpartytrustid" },
            { "role", ClaimTypes.Role },
            { "roles", ClaimTypes.Role },
            { "upn", ClaimTypes.Upn },
            { "winaccountname", ClaimTypes.WindowsAccountName },
        };

        private static IDictionary<string, string> longToShortClaimTypeMapping = new Dictionary<string, string>();
        private static HashSet<string> inboundClaimFilter = inboundClaimFilter = new HashSet<string>();

        /// <summary>
        /// Initializes static members of the <see cref="ClaimTypeMapping"/> class. 
        /// </summary>
        static ClaimTypeMapping()
        {
            foreach (KeyValuePair<string, string> kv in shortToLongClaimTypeMapping)
            {
                if (longToShortClaimTypeMapping.ContainsKey(kv.Value))
                {
                    continue;
                }

                longToShortClaimTypeMapping.Add(kv.Value, kv.Key);
            }
        }

        /// <summary>
        /// Gets the InboundClaimTypeMap used by JwtSecurityTokenHandler when producing claims from jwt. 
        /// </summary>
        public static IDictionary<string, string> InboundClaimTypeMap
        {
            get
            {
                return shortToLongClaimTypeMapping;
            }
        }

        /// <summary>
        /// Gets the OutboundClaimTypeMap is used by JwtSecurityTokenHandler to shorten claim types when creating a jwt. 
        /// </summary>
        public static IDictionary<string, string> OutboundClaimTypeMap
        {
            get
            {
                return longToShortClaimTypeMapping;
            }
        }

        public static ISet<string> InboundClaimFilter
        {
            get
            {
                return inboundClaimFilter;
            }
        }
    }
}
