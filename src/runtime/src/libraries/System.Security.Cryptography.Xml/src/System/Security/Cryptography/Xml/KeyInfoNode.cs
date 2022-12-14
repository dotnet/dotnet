// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml;

namespace System.Security.Cryptography.Xml
{
    // This is for generic, unknown nodes
    public class KeyInfoNode : KeyInfoClause
    {
        private XmlElement? _node;

        //
        // public constructors
        //

        public KeyInfoNode() { }

        public KeyInfoNode(XmlElement node)
        {
            _node = node;
        }

        //
        // public properties
        //

        public XmlElement? Value
        {
            get { return _node; }
            set { _node = value; }
        }

        //
        // public methods
        //

        public override XmlElement GetXml()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.PreserveWhitespace = true;
            return GetXml(xmlDocument);
        }

        internal override XmlElement GetXml(XmlDocument xmlDocument)
        {
            return (xmlDocument.ImportNode(_node!, true) as XmlElement)!;
        }

        public override void LoadXml(XmlElement value)
        {
            _node = value;
        }
    }
}
