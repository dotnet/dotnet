// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
