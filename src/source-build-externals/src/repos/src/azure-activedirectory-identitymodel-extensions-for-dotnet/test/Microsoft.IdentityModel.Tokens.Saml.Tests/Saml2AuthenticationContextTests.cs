// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Xunit;
using Microsoft.IdentityModel.Tokens.Saml2;

namespace Microsoft.IdentityModel.Tokens.Saml.Tests
{
    public class Saml2AuthenticationContextTests
    {
        [Fact]
        public void Saml2AuthenticationContext_Ctor_NoException()
        {
            new Saml2AuthenticationContext();
        }

        [Fact]
        public void Saml2AuthenticationContext_CtorNullClassRef_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Saml2AuthenticationContext(null));
        }

        [Fact]
        public void Saml2AuthenticationContext_CtorClassRef_NoException()
        {
            var classRef = new Uri("http://resource", UriKind.Absolute);
            new Saml2AuthenticationContext(classRef);
        }

        [Fact]
        public void Saml2AuthenticationContext_CtorNullClassAndDeclaration_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Saml2AuthenticationContext(null, null));
        }

        [Fact]
        public void Saml2AuthenticationContext_CtorNullDeclaration_ArgumentNullException()
        {
            new Saml2AuthenticationContext(new Uri("http://resource", UriKind.Absolute), null);
        }

        [Fact]
        public void Saml2AuthenticationContext_CtorClassAndDeclarationRef_NoException()
        {
            var classRef = new Uri("http://resource", UriKind.Absolute);
            var declarationReference = new Uri("http://resource", UriKind.Absolute);
            new Saml2AuthenticationContext(classRef, declarationReference);
        }

        [Fact]
        public void Saml2AuthenticationContext_RelativeClassReference_ArgumentException()
        {
            var classRef = new Uri("resource", UriKind.Relative);
            var authContext = new Saml2AuthenticationContext();
            Assert.Throws<ArgumentException>(() => new Saml2AuthenticationContext(classRef));
        }

        [Fact]
        public void Saml2AuthenticationContext_NullClassReference_ArgumentNullException()
        {
            var authContext = new Saml2AuthenticationContext();
            Assert.Throws<ArgumentNullException>(() => authContext.ClassReference = null);
        }

        [Fact]
        public void Saml2AuthenticationContext_AbsoluteClassReference_NoException()
        {
            var classRef = new Uri("http://resource", UriKind.Absolute);
            new Saml2AuthenticationContext
            {
                ClassReference = classRef
            };
        }

        [Fact]
        public void Saml2AuthenticationContext_RelativeDeclarationReference_ArgumentException()
        {
            var authContext = new Saml2AuthenticationContext();
            var declarationReference = new Uri("resource", UriKind.Relative);
            Assert.Throws<ArgumentException>(() => authContext.DeclarationReference = declarationReference);
        }

        [Fact]
        public void Saml2AuthenticationContext_AbsoluteDeclarationReference_NoException()
        {
            var declarationReference = new Uri("http://resource", UriKind.Absolute);
            new Saml2AuthenticationContext
            {
                DeclarationReference = declarationReference
            };
        }

        [Fact]
        public void Saml2AuthenticationContext_NullDeclarationReference()
        {
            new Saml2AuthenticationContext
            {
                DeclarationReference = null
            };
        }
    }
}
