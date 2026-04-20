// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens.Saml
{
    /// <summary>
    /// Represents the Action element specified in [Saml, 2.4.5.1].
    /// see: https://www.oasis-open.org/committees/download.php/3406/oasis-sstc-saml-core-1.1.pdf
    /// </summary>
    public class SamlAction
    {
        private Uri _namespace;
        private string _value;

        /// <summary>
        /// Constructs an instance of <see cref="SamlAction"/> class.
        /// </summary>
        /// <param name="value">Action value represented by this class.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="value"/> is null or empty.</exception>
        public SamlAction(string value)
            : this(value, null)
        {
        }

        /// <summary>
        /// Constructs an instance of <see cref="SamlAction"/> class.
        /// </summary>
        /// <param name="value">Value represented by this class.</param>
        /// <param name="namespace">Namespace in which the action is interpreted.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="value"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="namespace"/> is not an absolute Uri.</exception>
        public SamlAction(string value, Uri @namespace)
        {
            Value = value;
            Namespace = @namespace;
        }

        /// <summary>
        /// Gets or sets a URI reference representing the namespace in which the name of the
        /// specified action is to be interpreted. [Saml, 2.4.5.1]
        /// </summary>
        /// <exception cref="ArgumentNullException">if <paramref name="value"/> is not an absolute Uri.</exception>
        public Uri Namespace
        {
            get => _namespace;
            set
            {
                // optional, use default if not set.
                if (value == null)
                {
                    _namespace = new Uri(SamlConstants.DefaultActionNamespace);
                }
                else
                {

                    if (!value.IsAbsoluteUri)
                        throw LogExceptionMessage(new SamlSecurityTokenException(FormatInvariant(LogMessages.IDX11502, value)));

                    _namespace = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the label for an action sought to be performed on the
        /// specified resource. [Saml, 2.4.5.1]
        /// </summary>
        /// <exception cref="ArgumentNullException">if <paramref name="value"/> is null or empty.</exception>
        public string Value
        {
            get => _value;
            set => _value = (string.IsNullOrEmpty(value)) ? throw LogArgumentNullException(nameof(value)) : value;
        }
    }
}
