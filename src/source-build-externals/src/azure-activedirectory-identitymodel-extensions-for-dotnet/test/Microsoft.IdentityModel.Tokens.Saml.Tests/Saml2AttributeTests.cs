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
using Xunit;
using Microsoft.IdentityModel.Tokens.Saml2;

namespace Microsoft.IdentityModel.Tokens.Saml.Tests
{
    public class Saml2AttributeTests
    {
        [Fact]
        public void Saml2Attribute_RelativeNameFormat_ArgumentException()
        {
            var attr = new Saml2Attribute("Country");

            Assert.Throws<ArgumentException>(() => attr.NameFormat = new Uri("resource", UriKind.Relative));
        }

        [Fact]
        public void Saml2Attribute_NullNameFormat_NoException()
        {
            new Saml2Attribute("Country")
            {
                NameFormat = null
            };
        }

        [Fact]
        public void Saml2Attribute_AbsoluteNameFormat_NoException()
        {
            new Saml2Attribute("Country")
            {
                NameFormat = new Uri("http://resource", UriKind.Absolute)
            };
        }
    }
}
