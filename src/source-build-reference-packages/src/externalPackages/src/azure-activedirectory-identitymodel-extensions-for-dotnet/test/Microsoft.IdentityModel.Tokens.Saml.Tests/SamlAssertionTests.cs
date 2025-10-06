// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.TestUtils;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Saml2.Tests
{
    public class SamlAssertionTests
    {
        [Fact]
        public void CanonicalString()
        {
            var context = new CompareContext($"{this}.CanonicalString");

            var assertion = ReferenceSaml.SamlAssertion;
            var canonicalString = assertion.CanonicalString;
            var canonicalString2 = assertion.CanonicalString;

            IdentityComparer.AreStringsEqual(canonicalString, canonicalString2, context);

            TestUtilities.AssertFailIfErrors(context);
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
