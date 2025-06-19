// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Xunit;
using Microsoft.IdentityModel.Tokens.Saml2;

namespace Microsoft.IdentityModel.Tokens.Saml.Tests
{
    public class Saml2ActionTests
    {
        [Fact]
        public void Saml2Action_NullValue_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Saml2Action(null, new Uri("http://localhost", UriKind.Absolute)));
        }

        [Fact]
        public void Saml2Action_NullNamespace_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Saml2Action("resource", null));
        }

        [Fact]
        public void Saml2Action_RelativeNamespace_ArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Saml2Action(null, new Uri("api", UriKind.Relative)));
        }

        [Fact]
        public void Saml2Action_CanCreate()
        {
            new Saml2Action("resource", new Uri("http://localhost", UriKind.Absolute));
        }
    }
}
