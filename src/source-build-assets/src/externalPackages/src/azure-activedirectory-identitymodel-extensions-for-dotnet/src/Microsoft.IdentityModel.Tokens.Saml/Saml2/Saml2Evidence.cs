// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
    /// <summary>
    /// Represents the Evidence element specified in [Saml2Core, 2.7.4.3].
    /// see: http://docs.oasis-open.org/security/saml/v2.0/saml-core-2.0-os.pdf
    /// </summary>
    /// <remarks>
    /// Contains one or more assertions or assertion references that the SAML
    /// authority relied on in issuing the authorization decision. 
    /// [Saml2Core, 2.7.4.3]
    /// </remarks>
    public class Saml2Evidence
    {
        private readonly List<Saml2Id> _assertionIdReferences = new List<Saml2Id>();
        private readonly List<Saml2Assertion> _assertions = new List<Saml2Assertion>();
        private readonly AbsoluteUriCollection _assertionUriReferences = new AbsoluteUriCollection();

        /// <summary>
        /// Initializes a new instance of <see cref="Saml2Evidence"/> class.
        /// </summary>
        public Saml2Evidence()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Saml2Evidence"/> class from a <see cref="Saml2Assertion"/>.
        /// </summary>
        /// <param name="assertion"><see cref="Saml2Assertion"/> containing the evidence.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="assertion"/> is null.</exception>
        public Saml2Evidence(Saml2Assertion assertion)
        {
            if (assertion == null)
                throw LogArgumentNullException(nameof(assertion));

            _assertions.Add(assertion);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Saml2Evidence"/> class from a <see cref="Saml2Id"/>.
        /// </summary>
        /// <param name="idReference"><see cref="Saml2Id"/> containing the evidence.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="idReference"/> is null.</exception>
        public Saml2Evidence(Saml2Id idReference)
        {
            if (idReference == null)
                throw LogArgumentNullException(nameof(idReference));

            _assertionIdReferences.Add(idReference);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Saml2Evidence"/> class from a <see cref="Uri"/>.
        /// </summary>
        /// <param name="uriReference"><see cref="Uri"/> containing the evidence.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="uriReference"/> is null.</exception>
        public Saml2Evidence(Uri uriReference)
        {
            if (uriReference == null)
                throw LogArgumentNullException(nameof(uriReference));

            _assertionUriReferences.Add(uriReference);
        }

        /// <summary>
        /// Gets a collection of <see cref="Saml2Id"/> for use by the <see cref="Saml2Evidence"/>.
        /// </summary>
        public ICollection<Saml2Id> AssertionIdReferences
        {
            get { return _assertionIdReferences; }
        }

        /// <summary>
        /// Gets a collection of <see cref="Saml2Assertion"/>  for use by the <see cref="Saml2Evidence"/>.
        /// </summary>
        public ICollection<Saml2Assertion> Assertions
        {
            get { return _assertions; }
        }

        /// <summary>
        /// Gets a collection of <see cref="Uri"/>  for use by the <see cref="Saml2Evidence"/>.
        /// </summary>
        public ICollection<Uri> AssertionUriReferences
        {
            get { return _assertionUriReferences; }
        }
    }
}
