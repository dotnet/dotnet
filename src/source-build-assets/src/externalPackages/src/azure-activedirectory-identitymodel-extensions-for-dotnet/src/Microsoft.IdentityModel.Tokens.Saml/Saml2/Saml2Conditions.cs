// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
    /// <summary>
    /// Represents the Conditions element specified in [Saml2Core, 2.5.1].
    /// see: http://docs.oasis-open.org/security/saml/v2.0/saml-core-2.0-os.pdf
    /// </summary>
    public class Saml2Conditions
    {
        private DateTime? _notBefore;
        private DateTime? _notOnOrAfter;

        /// <summary>
        /// Initializes a new instance of <see cref="Saml2Conditions"/>. class.
        /// </summary>
        public Saml2Conditions()
        {
            AudienceRestrictions = new List<Saml2AudienceRestriction>();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Saml2Conditions"/>. class.
        /// </summary>
        /// <exception cref="ArgumentNullException">if <paramref name="audienceRestrictions"/> is null.</exception>
        public Saml2Conditions(IEnumerable<Saml2AudienceRestriction> audienceRestrictions)
        {
            if (audienceRestrictions == null)
                throw LogArgumentNullException(nameof(audienceRestrictions));

            AudienceRestrictions = new List<Saml2AudienceRestriction>(audienceRestrictions);
        }

        /// <summary>
        /// Gets a collection of <see cref="Saml2AudienceRestriction"/> that the assertion is addressed to.
        /// [Saml2Core, 2.5.1]
        /// </summary>
        public ICollection<Saml2AudienceRestriction> AudienceRestrictions
        {
            get;
        }

        /// <summary>
        /// Gets or sets the earliest time instant at which the assertion is valid. If the provided DateTime is not in UTC, it will
        /// be converted to UTC.
        /// [Saml2Core, 2.5.1]
        /// </summary>
        /// <exception cref="ArgumentException">if 'value' is greater or equal to <see cref="NotOnOrAfter"/>.</exception>
        public DateTime? NotBefore
        {
            get { return _notBefore; }
            set
            {
                value = DateTimeUtil.ToUniversalTime(value);

                // NotBefore must be earlier than NotOnOrAfter
                if (value != null && NotOnOrAfter.HasValue)
                {
                    if (value.Value >= NotOnOrAfter.Value)
                        throw LogExceptionMessage(new ArgumentException(FormatInvariant(LogMessages.IDX13513, MarkAsNonPII(value), MarkAsNonPII(NotOnOrAfter))));
                }

                _notBefore = value;
            }
        }

        /// <summary>
        /// Gets or sets the time instant at which the assertion has expired. If the provided DateTime is not in UTC, it will
        /// be converted to UTC.
        /// [Saml2Core, 2.5.1]
        /// </summary>
        /// <exception cref="ArgumentException">if 'value' is less than or equal to <see cref="NotBefore"/>.</exception>
        public DateTime? NotOnOrAfter
        {
            get { return _notOnOrAfter; }
            set
            {
                value = DateTimeUtil.ToUniversalTime(value);

                // NotBefore must be earlier than NotOnOrAfter
                if (value != null && NotBefore.HasValue)
                {
                    if (value.Value <= NotBefore.Value)
                        throw LogExceptionMessage(new ArgumentException(FormatInvariant(LogMessages.IDX13514, MarkAsNonPII(value), MarkAsNonPII(NotBefore))));
                }

                _notOnOrAfter = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the assertion SHOULD be used immediately and MUST NOT
        /// be retained for future use. [Saml2Core, 2.5.1]
        /// </summary>
        public bool OneTimeUse
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="Saml2ProxyRestriction"/> that specified limitations that the asserting party imposes on relying parties
        /// that wish to subsequently act as asserting parties themselves and issue assertions of their own on the basis of the information contained in
        /// the original assertion. [Saml2Core, 2.5.1]
        /// </summary>
        public Saml2ProxyRestriction ProxyRestriction
        {
            get;
            set;
        }
    }
}
