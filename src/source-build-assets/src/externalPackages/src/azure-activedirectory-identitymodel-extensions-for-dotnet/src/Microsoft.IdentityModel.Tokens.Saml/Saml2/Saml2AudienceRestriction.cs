// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
    /// <summary>
    /// Represents the AudienceRestriction element specified in [Saml2Core, 2.5.1.4].
    /// see: http://docs.oasis-open.org/security/saml/v2.0/saml-core-2.0-os.pdf
    /// </summary>
    public class Saml2AudienceRestriction
    {
        /// <summary>
        /// Creates an instance of Saml2AudienceRestriction.
        /// </summary>
        /// <param name="audience">The audience element contained in this restriction.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="audience"/> is null or empty.</exception>
        public Saml2AudienceRestriction(string audience)
        {
            if (string.IsNullOrEmpty(audience))
                throw LogArgumentNullException(nameof(audience));

            Audiences = new List<string> { audience };
        }

        /// <summary>
        /// Creates an instance of Saml2AudienceRestriction.
        /// </summary>
        /// <param name="audiences">The collection of audience elements contained in this restriction.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="audiences"/> is null.</exception>
        public Saml2AudienceRestriction(IEnumerable<string> audiences)
        {
            if (audiences == null)
                throw LogArgumentNullException(nameof(audiences));

            Audiences = new List<string>(audiences);
        }

        /// <summary>
        /// Gets the audiences for which the assertion is addressed.
        /// </summary>
        public ICollection<string> Audiences
        {
            get;
        }
    }
}
