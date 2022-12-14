// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;

namespace System.Xml.XmlDocumentTests
{
    public static class GetNameTests
    {
        [Fact]
        public static void EmptyElementCountTest()
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml("<elem1 child1='' child2='duu' child3='e1;e2;' child4='a1' child5='goody'> text node two text node three </elem1>");

            var namedNodeMap = (XmlNamedNodeMap)xmlDocument.FirstChild.Attributes;

            Assert.Equal(namedNodeMap.Item(0), namedNodeMap.GetNamedItem("child1"));
            Assert.Equal(string.Empty, namedNodeMap.Item(0).Value);

            Assert.Equal(namedNodeMap.Item(1), namedNodeMap.GetNamedItem("child2"));
            Assert.Equal("duu", namedNodeMap.Item(1).Value);

            Assert.Equal(namedNodeMap.Item(4), namedNodeMap.GetNamedItem("child5"));
            Assert.Equal("goody", namedNodeMap.Item(4).Value);
        }
    }
}
