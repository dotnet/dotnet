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

using Microsoft.IdentityModel.Tokens.Saml2;
using Microsoft.IdentityModel.Tokens.Saml;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;

using SecurityTokenDescriptor4x = System.IdentityModel.Tokens.SecurityTokenDescriptor;
using TokenValidationParameters4x = System.IdentityModel.Tokens.TokenValidationParameters;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.CrossVersionTokenValidation.Tests
{
    public class CrossTokenVersionTheoryData : TheoryDataBase
    {
        public Tokens.Saml2.AuthenticationInformation AuthenticationInformationSaml2 { get; set; }

        public Tokens.Saml.AuthenticationInformation AuthenticationInformationSaml { get; set; }

        public string TokenString4x { get; set; }

        public string TokenString5x { get; set; }

        public SecurityTokenDescriptor4x TokenDescriptor4x { get; set; }

        public SecurityTokenDescriptor TokenDescriptor5x { get; set; }

        public TokenValidationParameters4x ValidationParameters4x { get; set; }

        public TokenValidationParameters ValidationParameters5x { get; set; }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
