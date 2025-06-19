// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens.Saml
{
    /// <summary>
    /// Represents the AttributeStatement element.
    /// </summary>
    public class SamlAuthenticationStatement : SamlSubjectStatement
    {
        private string _authenticationMethod = SamlConstants.UnspecifiedAuthenticationMethod;

        internal SamlAuthenticationStatement()
        {
            AuthorityBindings = new List<SamlAuthorityBinding>();
        }

        /// <summary>
        /// Creates an instance of <see cref="SamlAuthenticationStatement"/>.
        /// </summary>
        /// <param name="samlSubject">The Subject of the Statement.</param>
        /// <param name="authenticationMethod">The URI reference that specifies the type of authentication that took place.</param>
        /// <param name="authenticationInstant">The time at which the authentication took place.</param>
        /// <param name="dnsAddress">The DNS domain name for the system entity from which the subject was apparently authenticated.</param>
        /// <param name="ipAddress">The IP address for the system entity from which the subject was apparently authenticated.</param>
        /// <param name="authorityBindings"><see cref="IEnumerable{SamlAuthorityBinding}"/>.</param>
        public SamlAuthenticationStatement(
            SamlSubject samlSubject,
            string authenticationMethod,
            DateTime authenticationInstant,
            string dnsAddress,
            string ipAddress,
            IEnumerable<SamlAuthorityBinding> authorityBindings)
        {
            if (string.IsNullOrEmpty(authenticationMethod))
                throw LogArgumentNullException(nameof(authenticationMethod));

            AuthenticationMethod = authenticationMethod;
            AuthenticationInstant = authenticationInstant.ToUniversalTime();
            DnsAddress = dnsAddress;
            IPAddress = ipAddress;
            Subject = samlSubject;

            AuthorityBindings = (authorityBindings == null) ? new List<SamlAuthorityBinding>() : new List<SamlAuthorityBinding>(authorityBindings);
        }

        /// <summary>
        /// Gets or sets the instant of authentication. This value should be in UTC.
        /// </summary>
        public DateTime AuthenticationInstant
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the method of authentication.
        /// </summary>
        /// <exception cref="ArgumentNullException">if 'value' is null or empty.</exception>
        public string AuthenticationMethod
        {
            get => _authenticationMethod;
            set => _authenticationMethod = (string.IsNullOrEmpty(value)) ? throw LogArgumentNullException(nameof(value)) : value;
        }

        /// <summary>
        /// Gets the collection of <see cref="ICollection{SamlAuthorityBinding}"/>.
        /// </summary>
        public ICollection<SamlAuthorityBinding> AuthorityBindings
        {
            get;
        }

        /// <summary>
        /// Gets or sets Domain Name Service address.
        /// </summary>
        public string DnsAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Internet Protocol address.
        /// </summary>
        public string IPAddress
        {
            get;
            set;
        }
    }
}
