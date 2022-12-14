// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;

namespace System.Xml.XmlDocumentTests
{
    public class NodeRemovedTests
    {
        [Fact]
        public static void RemoveNode()
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(@"<root> <elem1>text1</elem1> <elem2>text2</elem2> </root>");

            int count = 0;
            xmlDocument.NodeRemoved += (s, e) => count++;
            xmlDocument.NodeRemoved += (s, e) => Assert.Equal(XmlNodeChangedAction.Remove, e.Action);

            Assert.Equal(0, count);
            xmlDocument.DocumentElement.RemoveChild(xmlDocument.DocumentElement.FirstChild);
            Assert.Equal(1, count);
        }

        [Fact]
        public static void RemoveEventHandler()
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(@"<root> <elem1>text1</elem1> <elem2>text2</elem2> </root>");

            XmlNodeChangedEventHandler handler = (s, e) => { throw new ShouldNotBeInvokedException(); };
            xmlDocument.NodeRemoved += handler;
            xmlDocument.NodeRemoved -= handler;

            xmlDocument.DocumentElement.RemoveChild(xmlDocument.DocumentElement.FirstChild);
        }
    }
}
