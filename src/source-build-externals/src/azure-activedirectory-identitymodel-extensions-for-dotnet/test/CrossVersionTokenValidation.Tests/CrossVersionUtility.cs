// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IdentityModel.Tokens;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Xml;
using Microsoft.IdentityModel.TestUtils;

using Saml2SecurityTokenHandler4x = Microsoft.IdentityModel.Tokens.Saml2SecurityTokenHandler;
using SamlSecurityTokenHandler4x = Microsoft.IdentityModel.Tokens.SamlSecurityTokenHandler;
using SecurityToken4x = System.IdentityModel.Tokens.SecurityToken;
using TokenValidationParameters4x = System.IdentityModel.Tokens.TokenValidationParameters;

namespace Microsoft.IdentityModel.Protocols.Extensions.OldVersion
{
    /// <summary>
    /// Tests for references in specs
    /// https://datatracker.ietf.org/doc/html/rfc7518#appendix-A.3
    /// </summary>
    public class CrossVersionUtility
    {
        public static SecurityToken4x CreateSamlToken4x(SecurityTokenDescriptor descriptor)
        {
            return new SamlSecurityTokenHandler4x().CreateToken(descriptor);
        }

        public static SecurityToken4x CreateSaml2Token4x(SecurityTokenDescriptor descriptor)
        {
            return new Saml2SecurityTokenHandler4x().CreateToken(descriptor);
        }

        public static ClaimsPrincipal ValidateSamlToken(string securityToken, TokenValidationParameters4x validationParameters, out SecurityToken4x validatedToken)
        {
            return new SamlSecurityTokenHandler4x().ValidateToken(securityToken, validationParameters, out validatedToken);
        }

        public static ClaimsPrincipal ValidateSaml2Token(string securityToken, TokenValidationParameters4x validationParameters, out SecurityToken4x validatedToken)
        {
            return new Saml2SecurityTokenHandler4x().ValidateToken(securityToken, validationParameters, out validatedToken);
        }

        public static string WriteSamlToken(SecurityToken4x token)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var writer = XmlDictionaryWriter.CreateTextWriter(memoryStream, Encoding.UTF8, false))
                {
                    new SamlSecurityTokenHandler4x().WriteToken(writer, token);
                    writer.Flush();
                    writer.Close();
                    return Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }
        }

        public static string WriteSaml2Token(SecurityToken4x token)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var writer = XmlDictionaryWriter.CreateTextWriter(memoryStream, Encoding.UTF8, false))
                {
                    new Saml2SecurityTokenHandler4x().WriteToken(writer, token);
                    writer.Flush();
                    writer.Close();
                    return Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }
        }

        public static bool AreDateTimesEqual(DateTime? date1, DateTime? date2, CompareContext context)
        {
            var localContext = new CompareContext(context);
            if (!IdentityComparer.ContinueCheckingEquality(date1, date2, localContext))
                return context.Merge(localContext);

            if (!date1.Equals(date2))
            {
                context.Diffs.Add($"dates are not equal: '{date1}' : '{date2}'");
                return false;
            }

            return true;
        }

        public static bool AreDateTimesEqual(DateTime date1, DateTime date2, CompareContext context)
        {
            var localContext = new CompareContext(context);
            if (!date1.Equals(date2))
            {
                context.Diffs.Add($"dates are not equal: '{date1}' : '{date2}'");
                return false;
            }

            return true;
        }

        public static bool AreStringsEqual(string str1, string str2, CompareContext context)
        {
            var localContext = new CompareContext(context);

            if (!IdentityComparer.ContinueCheckingEquality(str1, str2, localContext))
                return context.Merge(localContext);

            if (!str1.Equals(str2))
            {
                context.Diffs.Add($"strings are not equal: '{str1}' : '{str2}'");
                return false;
            }

            return true;
        }

        public static bool AreUrisEqual(Uri uri1, Uri uri2, CompareContext context)
        {
            var localContext = new CompareContext(context);

            if (!IdentityComparer.ContinueCheckingEquality(uri1, uri2, localContext))
                return context.Merge(localContext);

            var str1 = uri1.OriginalString;
            var str2 = uri2.OriginalString;

            if (!str1.Equals(str2))
            {
                context.Diffs.Add($"uris are not equal: '{str1}' : '{str2}'");
                return false;
            }

            return true;
        }
    }
}
