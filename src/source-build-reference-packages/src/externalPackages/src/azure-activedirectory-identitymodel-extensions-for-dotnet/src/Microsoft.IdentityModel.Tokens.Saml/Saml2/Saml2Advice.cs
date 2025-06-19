// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
    /// <summary>
    /// Represents the Advice element specified in [Saml2Core, 2.6.1].
    /// see: http://docs.oasis-open.org/security/saml/v2.0/saml-core-2.0-os.pdf
    /// </summary>
    /// <remarks>
    /// This information MAY be ignored by applications without affecting either
    /// the semantics or the validity of the assertion. [Saml2Core, 2.6.1]
    /// </remarks>
    public class Saml2Advice
    {
        /// <summary>
        /// Creates an instance of Saml2Advice.
        /// </summary>
        public Saml2Advice()
        {
            AssertionIdReferences = new Collection<Saml2Id>();
            Assertions = new Collection<Saml2Assertion>();
            AssertionUriReferences = new AbsoluteUriCollection();
        }

        /// <summary>
        /// Gets a collection of <see cref="Saml2Id"/> representing the assertions in the <see cref="Saml2Advice"/>.
        /// </summary>
        public ICollection<Saml2Id> AssertionIdReferences
        {
            get;
        }

        /// <summary>
        /// Gets a collection of <see cref="Saml2Assertion"/> representing the assertions in the <see cref="Saml2Advice"/>.
        /// </summary>
        public ICollection<Saml2Assertion> Assertions
        {
            get;
        }

        /// <summary>
        /// Gets a collection of <see cref="Uri"/> representing the assertions in the <see cref="Saml2Advice"/>.
        /// </summary>
        public ICollection<Uri> AssertionUriReferences
        {
            get;
        }
    }
}