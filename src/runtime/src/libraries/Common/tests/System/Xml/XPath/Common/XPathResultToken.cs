// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.XPath;

namespace XPathTests.Common
{
    public class XPathResultToken
    {
        public XPathNodeType NodeType { get; set; }
        public string BaseURI { get; set; }
        public bool HasChildren { get; set; }
        public bool HasAttributes { get; set; }
        public bool IsEmptyElement { get; set; }
        public string LocalName { get; set; }
        public string Name { get; set; }
        public string NamespaceURI { get; set; }
        public bool HasNameTable { get; set; }
        public string Prefix { get; set; }
        public string Value { get; set; }
        public string XmlLang { get; set; }

        public XPathResultToken()
        {
            BaseURI = string.Empty;
            LocalName = string.Empty;
            Name = string.Empty;
            NamespaceURI = string.Empty;
            Prefix = string.Empty;
            Value = string.Empty;
            XmlLang = string.Empty;
        }
    }
}
