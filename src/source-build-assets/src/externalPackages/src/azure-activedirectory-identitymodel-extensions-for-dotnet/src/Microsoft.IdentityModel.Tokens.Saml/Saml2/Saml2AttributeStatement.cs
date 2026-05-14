// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
    /// <summary>
    /// Represents the AttributeStatement element specified in [Saml2Core, 2.7.3].
    /// see: http://docs.oasis-open.org/security/saml/v2.0/saml-core-2.0-os.pdf
    /// </summary>
    public class Saml2AttributeStatement : Saml2Statement
    {
        /// <summary>
        /// Creates an instance of Saml2AttributeStatement.
        /// </summary>
        public Saml2AttributeStatement()
        {
            Attributes = new List<Saml2Attribute>();
        }

        /// <summary>
        /// Creates an instance of Saml2AttributeStatement.
        /// </summary>
        /// <param name="attribute">The <see cref="Saml2Attribute"/> contained in this statement.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="attribute"/> is null.</exception>
        public Saml2AttributeStatement(Saml2Attribute attribute)
        {
            if (attribute == null)
                throw LogArgumentNullException(nameof(attribute));

            Attributes = new List<Saml2Attribute> { attribute };

        }

        /// <summary>
        /// Creates an instance of Saml2AttributeStatement.
        /// </summary>
        /// <param name="attributes">The collection of <see cref="Saml2Attribute"/> elements contained in this statement.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="attributes"/> is null.</exception>
        public Saml2AttributeStatement(IEnumerable<Saml2Attribute> attributes)
        {
            if (attributes == null)
                throw LogArgumentNullException(nameof(attributes));

            Attributes = new List<Saml2Attribute>(attributes);
        }

        /// <summary>
        /// Gets the collection of <see cref="Saml2Attribute"/> of this statement. [Saml2Core, 2.7.3]
        /// </summary>
        public ICollection<Saml2Attribute> Attributes
        {
            get;
        }
    }
}
