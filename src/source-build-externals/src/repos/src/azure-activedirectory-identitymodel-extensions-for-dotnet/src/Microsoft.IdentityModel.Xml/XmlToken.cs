// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Xml;

namespace Microsoft.IdentityModel.Xml
{

    /// <summary>
    /// Contains info about an single xml token read by an XmlReader.
    /// </summary>
    internal class XmlToken
    {
        public bool IsEmptyElement
        {
            get { return Value == null; }
            set { Value = value ? null : ""; }
        }

        public string Prefix { get; private set; }

        public string LocalName { get; private set; }

        public string Namespace { get; private set; }

        public XmlNodeType NodeType { get; private set; }

        public string Value { get; private set; }

        public XmlToken(XmlNodeType nodeType, string value)
        {
            NodeType = nodeType;
            Value = value;
        }

        public XmlToken(XmlNodeType nodeType, string prefix, string localName, string @namespace, string value)
        {
            NodeType = nodeType;
            Prefix = prefix;
            LocalName = localName;
            Namespace = @namespace;
            Value = value;
        }

        public XmlToken(XmlNodeType nodeType, string prefix, string localName, string @namespace, bool isEmptyElement)
        {
            NodeType = nodeType;
            Prefix = prefix;
            LocalName = localName;
            Namespace = @namespace;
            IsEmptyElement = isEmptyElement;
        }
    }
}
