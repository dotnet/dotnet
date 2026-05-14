// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Microsoft.IdentityModel.Tokens.Saml
{
    /// <summary>
    /// Represents the Conditions element specified in [Saml, 2.3.2.1].
    /// </summary>
    public class SamlConditions
    {
        internal SamlConditions()
        {
            Conditions = new List<SamlCondition>();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SamlConditions"/>.
        /// </summary>
        /// <param name="notBefore">The earliest time instant at which the assertion is valid</param>
        /// <param name="notOnOrAfter">The time instant at which the assertion has expired.</param>
        public SamlConditions(DateTime notBefore, DateTime notOnOrAfter)
            : this(notBefore, notOnOrAfter, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SamlConditions"/>.
        /// </summary>
        /// <param name="notBefore">The earliest time instant at which the assertion is valid</param>
        /// <param name="notOnOrAfter">The time instant at which the assertion has expired.</param>
        /// <param name="conditions"><see cref="IEnumerable{SamlCondition}"/>.</param>
        public SamlConditions(DateTime notBefore, DateTime notOnOrAfter,
            IEnumerable<SamlCondition> conditions
            )
        {
            NotBefore = notBefore.ToUniversalTime();
            NotOnOrAfter = notOnOrAfter.ToUniversalTime();

            Conditions = (conditions == null) ? new List<SamlCondition>() : new List<SamlCondition>(conditions);
        }

        /// <summary>
        /// Gets a collection of <see cref="ICollection{SamlCondition}"/> that the assertion is addressed to.
        /// </summary>
        public ICollection<SamlCondition> Conditions { get; }

        /// <summary>
        /// Gets or sets the earliest time instant at which the assertion is valid. This value should be in UTC.
        /// </summary>
        public DateTime NotBefore { get; set; } = DateTimeUtil.GetMinValue(DateTimeKind.Utc);

        /// <summary>
        /// Gets or sets the time instant at which the assertion has expired. This value should be in UTC.
        /// </summary>
        public DateTime NotOnOrAfter { get; set; } = DateTimeUtil.GetMaxValue(DateTimeKind.Utc);
    }
}
