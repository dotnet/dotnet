// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens.Saml
{
    /// <summary>
    /// Represents the AttributeStatement element.
    /// </summary>
    public class SamlAttributeStatement : SamlSubjectStatement
    {
        internal SamlAttributeStatement()
        {
            Attributes = new List<SamlAttribute>();
        }

        /// <summary>
        /// Creates an instance of <see cref="SamlAttributeStatement"/>.
        /// </summary>
        /// <param name="samlSubject">The subject of the attribute statement.</param>
        /// <param name="attribute">The <see cref="SamlAttribute"/> contained in this statement.</param>
        public SamlAttributeStatement(SamlSubject samlSubject, SamlAttribute attribute)
            : this(samlSubject, new SamlAttribute[] { attribute })
        { }

        /// <summary>
        /// Creates an instance of <see cref="SamlAttributeStatement"/>.
        /// </summary>
        /// <param name="samlSubject">The subject of the attribute statement.</param>
        /// <param name="attributes"><see cref="IEnumerable{SamlAttribute}"/>.</param>
        public SamlAttributeStatement(SamlSubject samlSubject, IEnumerable<SamlAttribute> attributes)
        {
            Subject = samlSubject;
            Attributes = (attributes == null) ? throw LogArgumentNullException(nameof(attributes)) : new List<SamlAttribute>(attributes);
        }

        /// <summary>
        ///  Gets a collection of <see cref="ICollection{SamlAttribute}"/>.
        /// </summary>
        public ICollection<SamlAttribute> Attributes { get; }
    }
}
