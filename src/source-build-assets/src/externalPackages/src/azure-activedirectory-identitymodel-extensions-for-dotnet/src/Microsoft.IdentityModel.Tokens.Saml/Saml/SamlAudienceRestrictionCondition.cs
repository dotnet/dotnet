// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens.Saml
{
    /// <summary>
    /// Represents the AudienceRestrictionCondition.
    /// </summary>
    public class SamlAudienceRestrictionCondition : SamlCondition
    {
        internal SamlAudienceRestrictionCondition()
        {
            Audiences = new List<Uri>();
        }

        /// <summary>
        /// Creates an instance of <see cref="SamlAudienceRestrictionCondition"/>.
        /// </summary>
        /// <param name="audience">The audience element contained in this restriction.</param>
        public SamlAudienceRestrictionCondition(Uri audience)
            : this(new Uri[] { audience })
        { }

        /// <summary>
        /// Creates an instance of <see cref="SamlAudienceRestrictionCondition"/>.
        /// </summary>
        /// <param name="audiences">An <see cref="IEnumerable{String}"/> containing the audiences for a <see cref="SamlAssertion"/>.</param>
        public SamlAudienceRestrictionCondition(IEnumerable<Uri> audiences)
        {
            Audiences = (audiences == null) ? throw LogArgumentNullException(nameof(audiences)) : new List<Uri>(audiences);
        }

        /// <summary>
        /// Gets the <see cref="ICollection{stringT}"/> of audiences for a <see cref="SamlAssertion"/>.
        /// </summary>
        public ICollection<Uri> Audiences
        {
            get;
        }
    }
}
