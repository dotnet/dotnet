// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Xml
{
    /// <summary>
    /// Maintains a collection of XML nodes obtained when reading signed XML.
    /// </summary>
    public class XmlTokenStream
    {
        private List<XmlToken> _xmlTokens = new List<XmlToken>();
        private string _excludedElement;
        private string _excludedElementNamespace;

        /// <summary>
        /// Initializes a <see cref="XmlTokenStream"/>
        /// </summary>
        public XmlTokenStream()
        {
        }

        internal int SignatureElement { get; set; } = -1;

        /// <summary>
        /// Adds a XML node to the collection.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException">if <paramref name="value"/> is null.</exception>
        public void Add(XmlNodeType type, string value)
        {
            _xmlTokens.Add(new XmlToken(type, value));
        }

        /// <summary>
        /// Adds a XML attribute node to the collection
        /// </summary>
        /// <param name="prefix">the XML prefix.</param>
        /// <param name="localName">the local name of the attribute.</param>
        /// <param name="namespace">the namespace of the attribute.</param>
        /// <param name="value">the value of the attribute.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="localName"/> is null or empty.</exception>
        public void AddAttribute(string prefix, string localName, string @namespace, string value)
        {
            if (string.IsNullOrEmpty(localName))
                throw LogArgumentNullException(nameof(localName));

            _xmlTokens.Add(new XmlToken(XmlNodeType.Attribute, prefix, localName, @namespace, value));
        }

        /// <summary>
        /// Adds a XML element node to the collection
        /// </summary>
        /// <param name="prefix">the XML prefix.</param>
        /// <param name="localName">the local name of the element.</param>
        /// <param name="namespace">the namespace of the attribute.</param>
        /// <param name="isEmptyElement">value indicating if the element is empty.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="localName"/> is null or empty.</exception>
        public void AddElement(string prefix, string localName, string @namespace, bool isEmptyElement)
        {
            if (string.IsNullOrEmpty(localName))
                throw LogArgumentNullException(nameof(localName));

            _xmlTokens.Add(new XmlToken(XmlNodeType.Element, prefix, localName, @namespace, isEmptyElement));
        }

        /// <summary>
        /// Sets the name and namespace of which element to exclude. Normally this is the &lt;Signature> element.
        /// </summary>
        /// <param name="element">the name of the Element to exclude.</param>
        /// <param name="namespace">the namespace of the Element to exclude.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="element"/> is null or empty.</exception>
        public void SetElementExclusion(string element, string @namespace)
        {
            if (string.IsNullOrEmpty(element))
                throw LogArgumentNullException(nameof(element));

            _excludedElement = element;
            _excludedElementNamespace = @namespace;
        }

        /// <summary>
        /// Writes the XML nodes into the <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">the <see cref="XmlWriter"/> to use.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="writer"/> is null.</exception>
        public void WriteTo(XmlWriter writer)
        {
            if (writer == null)
                throw LogArgumentNullException(nameof(writer));

            var streamWriter = new XmlTokenStreamWriter(this);
            streamWriter.WriteTo(writer, _excludedElement, _excludedElementNamespace);
        }

        internal ReadOnlyCollection<XmlToken> XmlTokens => _xmlTokens.AsReadOnly();
    }
}
