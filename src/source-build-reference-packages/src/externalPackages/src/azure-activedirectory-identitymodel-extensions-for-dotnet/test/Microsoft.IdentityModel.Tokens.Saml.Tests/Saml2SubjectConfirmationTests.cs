// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Xunit;
using Microsoft.IdentityModel.Tokens.Saml2;

namespace Microsoft.IdentityModel.Tokens.Saml.Tests
{
    public class Saml2SubjectConfirmationTests
    {
        [Fact]
        public void Saml2SubjectConfirmation_RelativeFormat_ArgumentException()
        {
            var method = new Uri("resource", UriKind.Relative);
            Assert.Throws<ArgumentException>(() => new Saml2SubjectConfirmation(method));
        }

        [Fact]
        public void Saml2SubjectConfirmation_NullFormat_Noxception()
        {
            Assert.Throws<ArgumentNullException>(() => new Saml2SubjectConfirmation(null));
        }

        [Fact]
        public void Saml2SubjectConfirmation_AbsoluteClassReference_NoException()
        {
            var method = new Uri("http://resource", UriKind.Absolute);
            new Saml2SubjectConfirmation(method);
        }
    }
}
