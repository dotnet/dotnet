// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Xml;

namespace Microsoft.IdentityModel.Xml
{
    /// <summary>
    /// Defines an interface for reading xml that has a known start element
    /// </summary>
    public interface IXmlElementReader
    {
        /// <summary>
        /// Returns true if the <see cref="XmlReader"/> is pointing to an element that can be read.
        /// </summary>
        bool CanRead(XmlReader reader);

        /// <summary>
        /// Reads an object from the current location and stores the result in items.
        /// </summary>
        /// <param name="reader">an <see cref="XmlReader"/>.</param>
        void Read(XmlReader reader);

        /// <summary>
        /// Returns the list of items that were read.
        /// </summary>
        IList<object> Items { get; }
    }
}
