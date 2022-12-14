// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;

namespace System.Xml.XmlDocumentTests
{
    public static class CountTests
    {
        [Fact]
        public static void EmptyElementCountTest()
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml("<foo />");

            var namedNodeMap = (XmlNamedNodeMap)xmlDocument.FirstChild.Attributes;

            Assert.Equal(0, namedNodeMap.Count);
        }

        [Fact]
        public static void AttributeCountTest()
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml("<foo a='1' />");

            var namedNodeMap = (XmlNamedNodeMap)xmlDocument.FirstChild.Attributes;

            Assert.Equal(1, namedNodeMap.Count);
        }
    }
}
