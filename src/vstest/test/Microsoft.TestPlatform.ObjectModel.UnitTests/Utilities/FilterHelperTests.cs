// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Resources;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.ObjectModel.UnitTests.Utilities;

[TestClass]
public class FilterHelpersTests
{
    [TestMethod]
    public void EscapeUnescapeNullThrowsArgumentNullException()
    {
        Assert.ThrowsException<ArgumentNullException>(() => FilterHelper.Escape(null!));
        Assert.ThrowsException<ArgumentNullException>(() => FilterHelper.Unescape(null!));
    }

    [TestMethod]
    public void EscapeUnescapeEmptyString()
    {
        Assert.AreEqual(string.Empty, FilterHelper.Escape(string.Empty));
        Assert.AreEqual(string.Empty, FilterHelper.Unescape(string.Empty));
    }

    [TestMethod]
    public void EscapeUnescapeStringWithoutSpecialCharacters()
    {
        var str = "TestNamespace.TestClass.TestMethod";
        Assert.AreEqual(str, FilterHelper.Escape(str));
        Assert.AreEqual(str, FilterHelper.Unescape(str));
    }

    [TestMethod]
    public void EscapeUnescapeStringWithParenthesis()
    {
        var value = "TestClass(1).TestMethod(2)";
        var escapedValue = FilterHelper.Escape(value);

        Assert.AreEqual(@"TestClass\(1\).TestMethod\(2\)", escapedValue);
        Assert.AreEqual(value, FilterHelper.Unescape(escapedValue));
    }

    [TestMethod]
    public void EscapeUnescapeStringWithSpecialCharacters()
    {
        var value = @"TestClass(""a | b"").TestMethod(""x != y"")";
        var escapedValue = @"TestClass\(""a \| b""\).TestMethod\(""x \!\= y""\)";

        Assert.AreEqual(escapedValue, FilterHelper.Escape(value));
        Assert.AreEqual(value, FilterHelper.Unescape(escapedValue));
    }

    [TestMethod]
    public void EscapeUnescapeStringWithPrefix()
    {
        var value = @"printf(""\r\n"")";
        var escapedValue = @"printf\(""\\r\\n""\)";

        Assert.AreEqual(escapedValue, FilterHelper.Escape(value));
        Assert.AreEqual(value, FilterHelper.Unescape(escapedValue));
    }

    [TestMethod]
    public void UnescapeForInvalidStringThrowsArgumentException1()
    {
        var invalidString = @"TestClass\$""a %4 b""%2.TestMethod";
        Assert.ThrowsException<ArgumentException>(() => FilterHelper.Unescape(invalidString), string.Format(CultureInfo.CurrentCulture, Resources.TestCaseFilterEscapeException, invalidString));
    }

    [TestMethod]
    public void UnescapeForInvalidStringThrowsArgumentException2()
    {
        var invalidString = @"TestClass\";
        Assert.ThrowsException<ArgumentException>(() => FilterHelper.Unescape(invalidString), string.Format(CultureInfo.CurrentCulture, Resources.TestCaseFilterEscapeException, invalidString));
    }
}
