//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Claims;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens.Saml
{
    /// <summary>
    /// Represents the Attribute element.
    /// </summary>
    public class SamlAttribute
    {
        private string _attributeValueXsiType = ClaimValueTypes.String;
        private string _name;
        private string _nameSpace;
        private string _originalIssuer;

        /// <summary>
        /// Initializes a new instance of <see cref="SamlAttribute"/>.
        /// </summary>
        internal SamlAttribute()
        {
            Values = new Collection<string>();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SamlAttribute"/>s.
        /// </summary>
        /// <param name="ns">The namespace of the attribute.</param>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="value">The value of the attribute.</param>
        public SamlAttribute(string ns, string name, string value)
            : this(ns, name, new string[] { value })
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SamlAttribute"/>.
        /// </summary>
        /// <param name="ns">The namespace of the attribute.</param>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="values"><see cref="IEnumerable{String}"/>.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="values"/> is null.</exception>
        public SamlAttribute(string ns, string name, IEnumerable<string> values)
        {
            Values = (values == null) ? throw LogArgumentNullException(nameof(values)) : new List<string>(values);

            Name = name;
            Namespace = ns;
            ClaimType = string.IsNullOrEmpty(_nameSpace) ? _name : _nameSpace + "/" + _name;
        }

        /// <summary>
        /// Gets or sets the xsi:type of the values contained in the SAML Attribute.
        /// </summary>
        public string AttributeValueXsiType
        {
            get { return _attributeValueXsiType; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw LogArgumentNullException(nameof(value));

                int indexOfHash = value.IndexOf('#');
                if (indexOfHash == -1)
                    throw LogExceptionMessage(new SecurityTokenInvalidAudienceException(FormatInvariant(LogMessages.IDX11314, value)));

                string prefix = value.Substring(0, indexOfHash);
                if (prefix.Length == 0)
                    throw LogExceptionMessage(new ArgumentException(FormatInvariant(LogMessages.IDX11314, value)));

                string suffix = value.Substring(indexOfHash + 1);
                if (suffix.Length == 0)
                {
                    throw LogExceptionMessage(new ArgumentException(FormatInvariant(LogMessages.IDX11314, value)));
                }

                _attributeValueXsiType = value;
            }
        }

        /// <summary>
        /// Gets or sets the ClaimType of the attribute.
        /// </summary>
        public string ClaimType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the attribute.
        /// </summary>
        /// <exception cref="ArgumentNullException"> if 'value' is null or empty.</exception>
        public string Name
        {
            get => _name;
            set => _name = string.IsNullOrEmpty(value) ? throw LogArgumentNullException(nameof(value)) : value;
        }

        /// <summary>
        /// Gets or sets the namespace of the attribute.
        /// </summary>
        public string Namespace
        {
            get { return _nameSpace; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw LogArgumentNullException(nameof(value));

                _nameSpace = value;
            }
        }

        /// <summary>
        /// Gets or sets the string that represents the OriginalIssuer of the SAML Attribute.
        /// </summary>
        public string OriginalIssuer
        {
            get { return _originalIssuer; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw LogArgumentNullException(nameof(value));

                _originalIssuer = value;
            }
        }

        /// <summary>
        /// Gets a collection of <see cref="ICollection{String}"/> representing attributes.
        /// </summary>
        public ICollection<string> Values { get; }
    }
}
