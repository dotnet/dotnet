// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
    /// <summary>
    /// Represents the ProxyRestriction element specified in [Saml2Core, 2.5.1.6].
    /// see: http://docs.oasis-open.org/security/saml/v2.0/saml-core-2.0-os.pdf
    /// </summary>
    public class Saml2ProxyRestriction
    {
        private Collection<Uri> _audiences = new AbsoluteUriCollection();
        private int? _count;

        /// <summary>
        /// Initializes an instance of <see cref="Saml2ProxyRestriction"/>.
        /// </summary>
        public Saml2ProxyRestriction()
        {
        }

        /// <summary>
        /// Gets the set of audiences to whom the asserting party permits
        /// new assertions to be issued on the basis of this assertion.
        /// </summary>
        public ICollection<Uri> Audiences
        {
            get { return _audiences; }
        }

        /// <summary>
        /// Gets or sets the maximum number of indirections that the asserting party
        /// permits to exist between this assertion and an assertion which has 
        /// ultimately been issued on the basis of it.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">if 'value' is less than 0.</exception>
        public int? Count
        {
            get { return _count; }
            set
            {
                if (null != value)
                {
                    if (value.Value < 0)
                        throw LogExceptionMessage(new ArgumentOutOfRangeException(nameof(value), "ID0002"));
                }

                _count = value;
            }
        }
    }
}
