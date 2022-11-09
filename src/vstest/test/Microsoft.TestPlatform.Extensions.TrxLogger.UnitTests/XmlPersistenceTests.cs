// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.TestPlatform.Extensions.TrxLogger.XML;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.UnitTests;

[TestClass]
public class XmlPersistenceTests
{
    [TestMethod]
    public void SaveObjectShouldReplaceInvalidCharacter()
    {
        XmlPersistence xmlPersistence = new();
        var node = xmlPersistence.CreateRootElement("TestRun");

        // we are handling only #x9 | #xA | #xD | [#x20-#xD7FF] | [#xE000-#xFFFD]
        char[] invalidXmlCharacterArray = new char[7];
        invalidXmlCharacterArray[0] = (char)0x5;
        invalidXmlCharacterArray[1] = (char)0xb;
        invalidXmlCharacterArray[2] = (char)0xf;
        invalidXmlCharacterArray[3] = (char)0xd800;
        invalidXmlCharacterArray[4] = (char)0xdc00;
        invalidXmlCharacterArray[5] = (char)0xfffe;
        invalidXmlCharacterArray[6] = (char)0x0;

        string strWithInvalidCharForXml = new(invalidXmlCharacterArray);
        XmlPersistence.SaveObject(strWithInvalidCharForXml, node, null, "dummy");

        string expectedResult = "\\u0005\\u000b\\u000f\\ud800\\udc00\\ufffe\\u0000";
        Assert.AreEqual(0, string.Compare(expectedResult, node.InnerXml));
    }

    [TestMethod]
    public void SaveObjectShouldNotReplaceValidCharacter()
    {
        XmlPersistence xmlPersistence = new();
        var node = xmlPersistence.CreateRootElement("TestRun");

        // we are handling only #x9 | #xA | #xD | [#x20-#xD7FF] | [#xE000-#xFFFD]
        char[] validXmlCharacterArray = new char[8];
        validXmlCharacterArray[0] = (char)0x9;
        validXmlCharacterArray[1] = (char)0xa;
        validXmlCharacterArray[2] = (char)0xd;
        validXmlCharacterArray[3] = (char)0x20;
        validXmlCharacterArray[4] = (char)0xc123;
        validXmlCharacterArray[5] = (char)0xe000;
        validXmlCharacterArray[6] = (char)0xea12;
        validXmlCharacterArray[7] = (char)0xfffd;

        string strWithValidCharForXml = new(validXmlCharacterArray);

        XmlPersistence.SaveObject(strWithValidCharForXml, node, null, "dummy");

        string expectedResult = "\t\n\r 섣�";
        Assert.AreEqual(0, string.Compare(expectedResult, node.InnerXml));
    }

    [TestMethod]
    public void SaveObjectShouldReplaceOnlyInvalidCharacter()
    {
        XmlPersistence xmlPersistence = new();
        var node = xmlPersistence.CreateRootElement("TestRun");
        string strWithInvalidCharForXml = "This string has these \0 \v invalid characters";
        XmlPersistence.SaveObject(strWithInvalidCharForXml, node, null, "dummy");
        string expectedResult = "This string has these \\u0000 \\u000b invalid characters";
        Assert.AreEqual(0, string.Compare(expectedResult, node.InnerXml));
    }
}
