// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.Common.Filtering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Filter.Source.UnitTests;

[TestClass]
public class FilterHelperTests
{
    [TestMethod]
    public void EscapeShouldReturnOriginalStringWhenNoSpecialCharacters()
    {
        var input = "TestMethod";
        Assert.AreEqual(input, FilterHelper.Escape(input));
    }

    [TestMethod]
    public void EscapeShouldEscapeOpenParenthesis()
    {
        Assert.AreEqual(@"Test\(Method", FilterHelper.Escape("Test(Method"));
    }

    [TestMethod]
    public void EscapeShouldEscapeCloseParenthesis()
    {
        Assert.AreEqual(@"Test\)Method", FilterHelper.Escape("Test)Method"));
    }

    [TestMethod]
    public void EscapeShouldEscapeAmpersand()
    {
        Assert.AreEqual(@"Test\&Method", FilterHelper.Escape("Test&Method"));
    }

    [TestMethod]
    public void EscapeShouldEscapePipe()
    {
        Assert.AreEqual(@"Test\|Method", FilterHelper.Escape("Test|Method"));
    }

    [TestMethod]
    public void EscapeShouldEscapeEqualSign()
    {
        Assert.AreEqual(@"Test\=Method", FilterHelper.Escape("Test=Method"));
    }

    [TestMethod]
    public void EscapeShouldEscapeExclamationMark()
    {
        Assert.AreEqual(@"Test\!Method", FilterHelper.Escape("Test!Method"));
    }

    [TestMethod]
    public void EscapeShouldEscapeTilde()
    {
        Assert.AreEqual(@"Test\~Method", FilterHelper.Escape("Test~Method"));
    }

    [TestMethod]
    public void EscapeShouldEscapeBackslash()
    {
        Assert.AreEqual(@"Test\\Method", FilterHelper.Escape(@"Test\Method"));
    }

    [TestMethod]
    public void EscapeShouldEscapeMultipleSpecialCharacters()
    {
        Assert.AreEqual(@"Test\(A\|B\)", FilterHelper.Escape("Test(A|B)"));
    }

    [TestMethod]
    public void UnescapeShouldReturnOriginalStringWhenNoEscapeCharacters()
    {
        var input = "TestMethod";
        Assert.AreEqual(input, FilterHelper.Unescape(input));
    }

    [TestMethod]
    public void UnescapeShouldUnescapeOpenParenthesis()
    {
        Assert.AreEqual("Test(Method", FilterHelper.Unescape(@"Test\(Method"));
    }

    [TestMethod]
    public void UnescapeShouldUnescapeCloseParenthesis()
    {
        Assert.AreEqual("Test)Method", FilterHelper.Unescape(@"Test\)Method"));
    }

    [TestMethod]
    public void UnescapeShouldUnescapeBackslash()
    {
        Assert.AreEqual(@"Test\Method", FilterHelper.Unescape(@"Test\\Method"));
    }

    [TestMethod]
    public void UnescapeShouldThrowOnInvalidEscapeSequence()
    {
        Assert.ThrowsExactly<ArgumentException>(() => FilterHelper.Unescape(@"Test\AMethod"));
    }

    [TestMethod]
    public void EscapeAndUnescapeRoundtrip()
    {
        var input = "TestClass(\"param\").Method(1.5)";
        Assert.AreEqual(input, FilterHelper.Unescape(FilterHelper.Escape(input)));
    }
}
