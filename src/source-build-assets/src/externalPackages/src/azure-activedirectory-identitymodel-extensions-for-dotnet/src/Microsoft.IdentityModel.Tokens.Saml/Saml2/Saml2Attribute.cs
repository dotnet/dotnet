// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.Xml;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
    /// <summary>
    /// Represents the Attribute element specified in [Saml2Core, 2.7.3.1].
    /// see: http://docs.oasis-open.org/security/saml/v2.0/saml-core-2.0-os.pdf
    /// </summary>
    public class Saml2Attribute
    {
        private string _attributeValueXsiType = System.Security.Claims.ClaimValueTypes.String;
        private string _friendlyName;
        private string _name;
        private Uri _nameFormat;

        /// <summary>
        /// Initializes a new instance of the Saml2Attribute class.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="name"/> is Null or Empty.</exception>
        public Saml2Attribute(string name)
        {
            Name = name;
            Values = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the Saml2Attribute class.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="value">The value of the attribute.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="name"/> is Null or Empty.</exception>
        public Saml2Attribute(string name, string value)
            : this(name, new string[] { value })
        {
        }

        /// <summary>
        /// Initializes a new instance of the Saml2Attribute class.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="values">The collection of values that define the attribute.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="name"/> is Null or Empty.</exception>
        public Saml2Attribute(string name, IEnumerable<string> values)
        {
            Name = name;
            if (values == null)
                throw LogArgumentNullException(nameof(values));

            Values = new List<string>(values);
        }

        /// <summary>
        /// Gets or sets a string that provides a more human-readable form of the attribute's 
        /// name. [Saml2Core, 2.7.3.1]
        /// </summary>
        public string FriendlyName
        {
            get => _friendlyName;
            set => _friendlyName = XmlUtil.NormalizeEmptyString(value);
        }

        /// <summary>
        /// Gets or sets the name of the attribute. [Saml2Core, 2.7.3.1]
        /// </summary>
        public string Name
        {
            get => _name;
            set => _name = string.IsNullOrEmpty(value)
                ? throw LogExceptionMessage(new ArgumentNullException(nameof(value))) : value;
        }

        /// <summary>
        /// Gets or sets a URI reference representing the classification of the attribute 
        /// name for the purposes of interpreting the name. [Saml2Core, 2.7.3.1]
        /// </summary>
        public Uri NameFormat
        {
            get => _nameFormat;
            set => _nameFormat = (value != null && !value.IsAbsoluteUri)
                ? throw LogExceptionMessage(new ArgumentException(FormatInvariant(LogMessages.IDX13300, MarkAsNonPII(nameof(NameFormat)), value), nameof(value)))
                : value;
        }

        /// <summary>
        /// Gets or sets the string that represents the OriginalIssuer of the this SAML Attribute.
        /// </summary>
        public string OriginalIssuer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the xsi:type of the values contained in the SAML Attribute.
        /// </summary>
        public string AttributeValueXsiType
        {
            get => _attributeValueXsiType;
            set => _attributeValueXsiType = string.IsNullOrEmpty(value)
                ? throw LogArgumentNullException(nameof(value))
                : value;
        }

        /// <summary>
        /// Gets the values of the attribute.
        /// </summary>
        public ICollection<string> Values
        {
            get;
        }
    }
}
