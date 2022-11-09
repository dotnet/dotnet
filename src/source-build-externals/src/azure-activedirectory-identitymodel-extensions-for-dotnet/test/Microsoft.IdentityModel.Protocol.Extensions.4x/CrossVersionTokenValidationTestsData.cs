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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Tokens;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

using SamlSecurityTokenHandler4x = Microsoft.IdentityModel.Tokens.SamlSecurityTokenHandler;
using SecurityToken4x = System.IdentityModel.Tokens.SecurityToken;
using TokenValidationParameters4x = System.IdentityModel.Tokens.TokenValidationParameters;
using X509SecurityKey4x = System.IdentityModel.Tokens.X509SecurityKey;

namespace Microsoft.IdentityModel.Protocols.Extensions.OldVersion
{
    /// <summary>
    /// Tests for references in specs
    /// https://tools.ietf.org/html/rfc7518#appendix-A.3
    /// </summary>
    public class CrossVersionTokenValidationTestsData
    {
        public static SecurityToken4x CreateToken4x(SecurityTokenDescriptor descriptor)
        {
            return new SamlSecurityTokenHandler4x().CreateToken(descriptor);
        }

        public static ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters4x validationParameters, out SecurityToken4x validatedToken)
        {
            return new SamlSecurityTokenHandler4x().ValidateToken(securityToken, validationParameters, out validatedToken);
        }

        public static string WriteToken(SecurityToken4x token)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(sb);
            new SamlSecurityTokenHandler4x().WriteToken(writer, token);
            writer.Flush();
            writer.Close();
            return sb.ToString();
        }
    }
}
