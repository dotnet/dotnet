// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;

namespace System.Xml.XmlDocumentTests
{
    public class NodeChangedTests
    {
        [Fact]
        public static void ChangingNodeFiresChangedEvent()
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(@"<root> <elem1>text1</elem1> <elem2>text2</elem2> </root>");

            int count = 0;
            xmlDocument.NodeChanged += (s, e) => count++;
            xmlDocument.NodeChanged += (s, e) => Assert.Equal(XmlNodeChangedAction.Change, e.Action);

            Assert.Equal(0, count);
            xmlDocument.DocumentElement.FirstChild.InnerText = "newvalue";
            Assert.Equal(1, count);
        }

        [Fact]
        public static void RemoveEventHandler()
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(@"<root> <elem1>text1</elem1> <elem2>text2</elem2> </root>");

            XmlNodeChangedEventHandler handler = (s, e) => { throw new ShouldNotBeInvokedException(); };
            xmlDocument.NodeChanged += handler;
            xmlDocument.NodeChanged -= handler;

            xmlDocument.DocumentElement.FirstChild.InnerText = "newvalue";
        }
    }
}
