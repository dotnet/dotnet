// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Build.Utils.UnitTests;

[TestClass]
public class ArgumentEscaperTests
{
    [TestMethod]
    public void EscapeArgForProcessStartShouldAddDoubleQuoteIfThereIsSpace()
    {
        string stringWithSpace = "Some string";

        string expected = "\"Some string\"";
        string result = ArgumentEscaper.HandleEscapeSequenceInArgForProcessStart(stringWithSpace);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void EscapeArgForProcessStartShouldAddDoubleQuoteIfThereIsSpaceAtEnd()
    {
        string stringWithSpaceAtEnd = "Some string  ";

        string expected = "\"Some string  \"";
        string result = ArgumentEscaper.HandleEscapeSequenceInArgForProcessStart(stringWithSpaceAtEnd);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void EscapeArgForProcessStartShouldHandleForwardSlash()
    {
        string stringWithForwardSlash = "Some/string";

        string expected = "Some/string";
        string result = ArgumentEscaper.HandleEscapeSequenceInArgForProcessStart(stringWithForwardSlash);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void EscapeArgForProcessStartShouldPreserveDoubleQuote()
    {
        string stringWithDoubleQuote = "Some\"string";

        string expected = "Some\\\"string";
        string result = ArgumentEscaper.HandleEscapeSequenceInArgForProcessStart(stringWithDoubleQuote);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void EscapeArgForProcessStartShouldPreserveSingleQuote()
    {
        string stringWithSingleQuote = "Some'string";

        string expected = "Some'string";
        string result = ArgumentEscaper.HandleEscapeSequenceInArgForProcessStart(stringWithSingleQuote);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void EscapeArgForProcessStartShouldPreserveBackSlash()
    {
        string stringWithBackSlash = @"Some\\string";

        string expected = "Some\\\\string";
        string result = ArgumentEscaper.HandleEscapeSequenceInArgForProcessStart(stringWithBackSlash);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void EscapeArgForProcessStartShouldPreserveBackSlashIfStringHasWhiteSpace()
    {
        string stringWithBackSlash = @"Some string With Space\\";

        string expected = @"""Some string With Space\\\\""";
        string result = ArgumentEscaper.HandleEscapeSequenceInArgForProcessStart(stringWithBackSlash);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void ShouldSurroundWithQuotesShouldReturnFalseIfAlreadySurroundWithQuotes()
    {
        string stringSurroundWithQuotes = "\"some string\"";

        Assert.IsFalse(ArgumentEscaper.ShouldSurroundWithQuotes(stringSurroundWithQuotes));
    }

    [TestMethod]
    public void ShouldSurroundWithQuotesShouldReturnFalseIfItIsNotSurroundWithQuotesAndHasNoWhiteSpace()
    {
        string stringWithoutSpace = "someStringWithNoWhiteSpace";

        Assert.IsFalse(ArgumentEscaper.ShouldSurroundWithQuotes(stringWithoutSpace));
    }

    [TestMethod]
    public void ShouldSurroundWithQuotesShouldReturnTrueIfItIsNotSurroundWithQuotesAndHasWhiteSpace()
    {
        string stringWithSpace = "some String With WhiteSpace";

        Assert.IsTrue(ArgumentEscaper.ShouldSurroundWithQuotes(stringWithSpace));
    }

    [TestMethod]
    public void IsSurroundedWithQuotesShouldReturnTrueIfStringIsSurroundedByQuotes()
    {
        string stringSurroundWithQuotes = "\"some string\"";

        Assert.IsTrue(ArgumentEscaper.IsSurroundedWithQuotes(stringSurroundWithQuotes));
    }

    [TestMethod]
    public void IsSurroundedWithQuotesShouldReturnFalseIfStringIsNotSurroundedByQuotes()
    {
        string stringNotSurroundWithQuotes = "some string";

        Assert.IsFalse(ArgumentEscaper.IsSurroundedWithQuotes(stringNotSurroundWithQuotes));
    }
}
