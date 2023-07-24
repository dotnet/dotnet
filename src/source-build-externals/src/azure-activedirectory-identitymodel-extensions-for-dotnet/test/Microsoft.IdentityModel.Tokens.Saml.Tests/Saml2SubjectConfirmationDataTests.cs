// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Xunit;
using Microsoft.IdentityModel.Tokens.Saml2;

namespace Microsoft.IdentityModel.Tokens.Saml.Tests
{
    public class Saml2SubjectConfirmationDataTests
    {
        [Fact]
        public void Saml2SubjectConfirmationData_RelativeFormat_ArgumentException()
        {
            var recipient = new Uri("recipient", UriKind.Relative);
            var subjectConfirmationData = new Saml2SubjectConfirmationData();
            Assert.Throws<ArgumentException>(() => subjectConfirmationData.Recipient = recipient);
        }

        [Fact]
        public void Saml2SubjectConfirmationData_NullFormat_Noxception()
        {
            var subjectConfirmationData = new Saml2SubjectConfirmationData();
            Assert.Throws<ArgumentNullException>(() => subjectConfirmationData.Recipient = null);
        }

        [Fact]
        public void Saml2SubjectConfirmationData_AbsoluteClassReference_NoException()
        {
            var recipient = new Uri("http://resource", UriKind.Absolute);
            var subjectConfirmationData = new Saml2SubjectConfirmationData();
            subjectConfirmationData.Recipient = recipient;
        }
    }
}
