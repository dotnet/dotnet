// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Xunit;
using Microsoft.IdentityModel.Tokens.Saml2;

namespace Microsoft.IdentityModel.Tokens.Saml.Tests
{
    public class Saml2NameIdentifierTests
    {
        [Fact]
        public void Saml2NameIdentifier_RelativeFormat_ArgumentException()
        {
            var format = new Uri("format", UriKind.Relative);
            Assert.Throws<ArgumentException>(() => new Saml2NameIdentifier("name", format));
        }

        [Fact]
        public void Saml2NameIdentifier_NullFormat_Noxception()
        {
            new Saml2NameIdentifier("name", null);
        }

        [Fact]
        public void Saml2NameIdentifier_AbsoluteClassReference_NoException()
        {
            var format = new Uri("http://resource", UriKind.Absolute);
            new Saml2NameIdentifier("name", format);
        }
    }
}
