// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
    /// <summary>
    /// The authentication information that an authority asserted when creating a token for a subject.
    /// </summary>
    public class AuthenticationInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationInformation"/> class.
        /// </summary>
        public AuthenticationInformation(Uri authenticationMethod, DateTime authenticationInstant)
        {
            AuthenticationMethod = authenticationMethod ?? throw LogHelper.LogArgumentNullException(nameof(authenticationMethod));
            AuthenticationInstant = authenticationInstant;
        }

        /// <summary>
        /// Gets or sets the address of the authority that created the token.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the AuthenticationMethod
        /// </summary>
        public Uri AuthenticationMethod
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the AuthenticationInstant. This value should be in UTC.
        /// </summary>
        public DateTime AuthenticationInstant { get; set; }

        /// <summary>
        /// Gets or sets the DNS name of the authority that created the token.
        /// </summary>
        public string DnsName { get; set; }

        /// <summary>
        /// Gets or sets the time that the session referred to in the session index MUST be considered ended. This value should be in UTC.
        /// </summary>
        public DateTime? NotOnOrAfter { get; set; }

        /// <summary>
        /// Gets or sets the session index that describes the session between the authority and the client.
        /// </summary>
        public string Session { get; set; }
    }
}
