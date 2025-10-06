// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml;

/// <summary>
/// Ensure XmlElements are created with <see cref="IXmlLineInfo"/> so that errors on elements
/// can be reported with line and position information.
/// </summary>
internal sealed class LineInfoXmlDocument : XmlDocument
{
    private IXmlLineInfo? xmlLineInfo;

    public override XmlElement CreateElement(string? prefix, string localName, string? namespaceURI)
    {
        return this.xmlLineInfo is not null && this.xmlLineInfo.HasLineInfo() ?
            new LineInfoXmlElement(prefix, localName, namespaceURI, this, this.xmlLineInfo.LineNumber, this.xmlLineInfo.LinePosition) :
            new LineInfoXmlElement(prefix, localName, namespaceURI, this);
    }

    public override void Load(XmlReader reader)
    {
        this.xmlLineInfo = reader as IXmlLineInfo;
        try
        {
            base.Load(reader);
        }
        finally
        {
            this.xmlLineInfo = null;
        }
    }

    // Extend XmlElement to include line and position information.
    internal sealed class LineInfoXmlElement : XmlElement, IXmlLineInfo
    {
        private readonly bool hasLineInfo;

        internal LineInfoXmlElement(string? prefix, string localName, string? namespaceURI, XmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        internal LineInfoXmlElement(string? prefix, string localName, string? namespaceURI, XmlDocument doc, int line, int column)
            : base(prefix, localName, namespaceURI, doc)
        {
            this.hasLineInfo = true;
            this.LineNumber = line;
            this.LinePosition = column;
        }

        /// <inheritdoc/>
        public int LineNumber { get; }

        /// <inheritdoc/>
        public int LinePosition { get; }

        /// <inheritdoc/>
        public bool HasLineInfo() => this.hasLineInfo;
    }
}
