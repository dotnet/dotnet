// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;

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

        // we are handling only #x9 | #xA | #xD | [#x20-#xD7FF] | [#xE000-#xFFFD] plus
        // well-formed surrogate pairs, which encode [#x10000-#x10FFFF].
        char[] invalidXmlCharacterArray = new char[5];
        invalidXmlCharacterArray[0] = (char)0x5;
        invalidXmlCharacterArray[1] = (char)0xb;
        invalidXmlCharacterArray[2] = (char)0xf;
        invalidXmlCharacterArray[3] = (char)0xfffe;
        invalidXmlCharacterArray[4] = (char)0x0;

        string strWithInvalidCharForXml = new(invalidXmlCharacterArray);
        XmlPersistence.SaveObject(strWithInvalidCharForXml, node, null, "dummy");

        string expectedResult = "\\u0005\\u000b\\u000f\\ufffe\\u0000";
        Assert.AreEqual(0, string.Compare(expectedResult, node.InnerXml));
    }

    [TestMethod]
    // A well-formed surrogate pair encodes a character in [#x10000-#x10FFFF], which the XML
    // spec lists as valid, so it must survive untouched - as must every ordinary BMP
    // character. Escaping either of those is what this whole area of code got wrong.
    [DataRow("", DisplayName = "empty string")]
    [DataRow("Grüße 日本語 Čau", DisplayName = "non-ASCII BMP characters")]
    [DataRow("party \U0001F389 done", DisplayName = "surrogate pair surrounded by ASCII")]
    [DataRow("\ud800\udc00", DisplayName = "lowest surrogate pair, U+10000")]
    [DataRow("\udbff\udfff", DisplayName = "highest surrogate pair, U+10FFFF")]
    [DataRow("\U0001F389\U0001F389", DisplayName = "two consecutive surrogate pairs")]
    [DataRow("\U0001F389", DisplayName = "surrogate pair is the entire string")]
    public void SaveObjectShouldNotReplaceValidText(string text)
    {
        XmlPersistence xmlPersistence = new();
        var node = xmlPersistence.CreateRootElement("TestRun");

        // Unlike the sibling test below, expected here is the input itself, so a row whose
        // text was mangled in transit would assert sanitize(mangled) == mangled and pass
        // while proving nothing - U+FFFD is a valid XML character and survives sanitizing.
        Assert.DoesNotContain("\ufffd", text, "A surrogate pair did not survive the data row transport - this row would assert nothing.");

        XmlPersistence.SaveObject(text, node, null, "dummy");

        Assert.AreEqual(text, node.InnerXml);
    }

    [TestMethod]
    // Unlike a pair, a lone surrogate is not a valid Unicode scalar value and is not valid
    // XML - XmlWriter would throw on it - so it must still be escaped. Allowing all of
    // \uD800-\uDFFF through would be the naive fix and would trade a mangled-string bug for
    // an unparseable-trx bug, which is worse because it breaks every consumer of the trx.
    //
    // {H} and {L} stand for a lone high and a lone low surrogate. They cannot be written
    // into the row directly - see WithLoneSurrogates below.
    [DataRow("a{H}b", @"a\ud800b", DisplayName = "lone high surrogate between valid characters")]
    [DataRow("a{L}b", @"a\udc00b", DisplayName = "lone low surrogate between valid characters")]
    [DataRow("{L}b", @"\udc00b", DisplayName = "low surrogate as the first character")]
    [DataRow("ab{H}", @"ab\ud800", DisplayName = "high surrogate as the last character")]
    [DataRow("{H}", @"\ud800", DisplayName = "high surrogate is the entire string")]
    [DataRow("{H}{H}", @"\ud800\ud800", DisplayName = "two consecutive high surrogates")]
    [DataRow("{L}{L}", @"\udc00\udc00", DisplayName = "two consecutive low surrogates")]
    [DataRow("{L}{H}", @"\udc00\ud800", DisplayName = "low surrogate followed by high surrogate")]
    [DataRow("\U0001F389{H}\v{L}", "\U0001F389" + @"\ud800\u000b\udc00", DisplayName = "valid pair, lone surrogates and a control character mixed")]
    public void SaveObjectShouldReplaceLoneSurrogate(string text, string expected)
    {
        XmlPersistence xmlPersistence = new();
        var node = xmlPersistence.CreateRootElement("TestRun");

        XmlPersistence.SaveObject(WithLoneSurrogates(text), node, null, "dummy");

        Assert.AreEqual(expected, node.InnerXml);
    }

    [TestMethod]
    public void SaveObjectShouldNotReplaceWellFormedSurrogatePairInAttributeValue()
    {
        XmlPersistence xmlPersistence = new();
        var node = xmlPersistence.CreateRootElement("TestRun");

        xmlPersistence.SaveObject("party \U0001F389 done", node, "@testName", null);

        Assert.AreEqual("party \U0001F389 done", node.GetAttribute("testName"));
    }

    [TestMethod]
    public void SaveObjectShouldProduceWellFormedXmlWithAstralCharacters()
    {
        XmlPersistence xmlPersistence = new();
        var node = xmlPersistence.CreateRootElement("TestRun");

        xmlPersistence.SaveObject("party \U0001F389 done", node, "@testName", null);
        XmlPersistence.SaveObject("output \U0001F389 here", node, null, "dummy");

        XmlDocument document = new();
        document.LoadXml(node.OuterXml);

        Assert.AreEqual("party \U0001F389 done", document.DocumentElement!.GetAttribute("testName"));
        Assert.AreEqual("output \U0001F389 here", document.DocumentElement.InnerText);
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

    /// <summary>
    /// Substitutes the {H} and {L} placeholders used by <see cref="SaveObjectShouldReplaceLoneSurrogate"/>
    /// with a lone high and a lone low surrogate respectively.
    /// </summary>
    /// <remarks>
    /// A lone surrogate cannot be written into a <see cref="DataRowAttribute"/> directly: the
    /// test platform's argument transport is not lone-surrogate safe and substitutes U+FFFD
    /// for it before the row reaches the test method. U+FFFD is itself a valid XML character,
    /// so every such row would silently degrade into an assertion that proves nothing. Only
    /// well-formed pairs survive the transport, which is why
    /// <see cref="SaveObjectShouldNotReplaceValidText"/> can inline them - it asserts the same
    /// way for the same reason, because its expected value is the input and would otherwise go
    /// vacuous if the transport ever stopped round-tripping them.
    /// </remarks>
    private static string WithLoneSurrogates(string text)
    {
        Assert.DoesNotContain("\ufffd", text, "Lone surrogates must be written as {H} or {L}, a literal one does not survive the data row transport.");

        return text.Replace("{H}", "\ud800").Replace("{L}", "\udc00");
    }
}
