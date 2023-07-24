// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Xml;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens.Saml
{
    /// <summary>
    /// Represents the SamlAuthorityBinding specified in [Saml, 2.4.3.2].
    /// </summary>
    public class SamlAuthorityBinding
    {
        private XmlQualifiedName _authorityKind;
        private string _binding;
        private string _location;

        internal SamlAuthorityBinding()
        {
        }

        /// <summary>
        /// Create an instance of <see cref="SamlAuthorityBinding"/>.
        /// </summary>
        /// <param name="authorityKind">The type of SAML protocol queries to which the authority described by this element will respond.</param>
        /// <param name="binding">The URI identifying the SAML protocol binding to use in communicating with the authority.</param>
        /// <param name="location">The URI describing how to locate and communicate with the authority.</param>
        public SamlAuthorityBinding(XmlQualifiedName authorityKind, string binding, string location)
        {
            AuthorityKind = authorityKind;
            Binding = binding;
            Location = location;
        }

        /// <summary>
        /// Gets or sets the AuthorityKind of the binding.
        /// </summary>
        public XmlQualifiedName AuthorityKind
        {
            get { return _authorityKind; }
            set
            {
                if (value == null)
                    throw LogArgumentNullException(nameof(value));

                if (string.IsNullOrEmpty(value.Name))
                    throw LogExceptionMessage(new SamlSecurityTokenException(LogMessages.IDX11507));

                _authorityKind = value;
            }
        }

        /// <summary>
        /// Gets or sets the binding.
        /// </summary>
        public string Binding
        {
            get { return _binding; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw LogArgumentNullException(nameof(value));

                _binding = value;
            }
        }

        /// <summary>
        /// Gets or sets the location of the binding.
        /// </summary>
        public string Location
        {
            get { return _location; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw LogArgumentNullException(nameof(value));

                _location = value;
            }
        }
    }
}
