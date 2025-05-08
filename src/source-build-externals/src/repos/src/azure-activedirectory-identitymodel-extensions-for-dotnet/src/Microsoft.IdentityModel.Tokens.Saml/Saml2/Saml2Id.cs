// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Xml;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
    /// <summary>
    /// Represents the identifier used for SAML assertions.
    /// see: http://docs.oasis-open.org/security/saml/v2.0/saml-core-2.0-os.pdf
    /// </summary>
    /// <details>
    /// This identifier should be unique per [Saml2Core, 1.3.4] 
    /// and must fit the NCName xml schema definition, which is to say that
    /// it must begin with a letter or underscore. 
    /// </details>
    public class Saml2Id
    {
        /// <summary>
        /// Creates a new ID value based on a GUID.
        /// </summary>
        public Saml2Id()
            : this(UniqueId.CreateRandomId())
        {
        }

        /// <summary>
        /// Creates a new ID whose value is the given string.
        /// </summary>
        /// <param name="value">The Saml2 Id.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="value"/> is null or empty.</exception>
        /// <exception cref="ArgumentException">if <paramref name="value"/> is not a valid NCName.</exception>
        public Saml2Id(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw LogArgumentNullException(nameof(value));

            try
            {
                Value = XmlConvert.VerifyNCName(value);
            }
            catch (XmlException ex)
            {
                throw LogExceptionMessage(new ArgumentException(FormatInvariant(LogMessages.IDX13515, value), ex));
            }
        }

        /// <summary>
        /// Gets the identifier string.
        /// </summary>
        public string Value
        {
            get;
        }
    }
}
