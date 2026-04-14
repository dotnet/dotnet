// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Utilities.Tests;

/// <summary>
/// Regression tests for CommandLineUtilities.SplitCommandLineIntoArguments.
/// </summary>
[TestClass]
public class CommandLineUtilitiesRegressionTests
{
    // Regression test for #15304 — Answer file parsing interprets `\"` as end of quoted string
    // Before the fix, backslash-quote inside a quoted argument was misinterpreted.
    [TestMethod]
    public void SplitCommandLineIntoArguments_BackslashQuoteInArgument_ShouldEscapeQuote()
    {
        // \"value\" should produce a literal "value"
        string input = """/Tests:"Test(\"iCT 256\")" """;
        bool hadError = CommandLineUtilities.SplitCommandLineIntoArguments(input, out string[] args);

        Assert.IsFalse(hadError);
        Assert.HasCount(1, args);
        Assert.AreEqual(@"/Tests:Test(""iCT 256"")", args[0]);
    }

    // Regression test for #15304
    [TestMethod]
    public void SplitCommandLineIntoArguments_DoubleBackslash_ShouldProduceSingleBackslash()
    {
        string input = @"arg\\value";
        bool hadError = CommandLineUtilities.SplitCommandLineIntoArguments(input, out string[] args);

        Assert.IsFalse(hadError);
        Assert.HasCount(1, args);
        Assert.AreEqual(@"arg\value", args[0]);
    }

    // Regression test for #15304
    [TestMethod]
    public void SplitCommandLineIntoArguments_BackslashBeforeNonSpecialChar_ShouldPreserveBackslash()
    {
        string input = @"arg\nvalue";
        bool hadError = CommandLineUtilities.SplitCommandLineIntoArguments(input, out string[] args);

        Assert.IsFalse(hadError);
        Assert.HasCount(1, args);
        Assert.AreEqual(@"arg\nvalue", args[0]);
    }

    // Regression test for #15304
    [TestMethod]
    public void SplitCommandLineIntoArguments_TrailingBackslash_ShouldPreserveBackslash()
    {
        string input = @"arg\";
        bool hadError = CommandLineUtilities.SplitCommandLineIntoArguments(input, out string[] args);

        Assert.IsFalse(hadError);
        Assert.HasCount(1, args);
        Assert.AreEqual(@"arg\", args[0]);
    }

    // Regression test for #15304
    [TestMethod]
    public void SplitCommandLineIntoArguments_UnbalancedQuotes_ShouldReportError()
    {
        string input = @"""unbalanced";
        bool hadError = CommandLineUtilities.SplitCommandLineIntoArguments(input, out string[] _);

        Assert.IsTrue(hadError);
    }

    // Regression test for #15304
    [TestMethod]
    public void SplitCommandLineIntoArguments_CommentLine_ShouldBeIgnored()
    {
        string input = "arg1\n# This is a comment\narg2";
        bool hadError = CommandLineUtilities.SplitCommandLineIntoArguments(input, out string[] args);

        Assert.IsFalse(hadError);
        Assert.HasCount(2, args);
        Assert.AreEqual("arg1", args[0]);
        Assert.AreEqual("arg2", args[1]);
    }

    // Regression test for #15304
    [TestMethod]
    public void SplitCommandLineIntoArguments_QuotedPathWithBackslashes_ShouldHandleCorrectly()
    {
        string input = """/testadapterpath:"c:\Path\To\Adapters" """;
        bool hadError = CommandLineUtilities.SplitCommandLineIntoArguments(input, out string[] args);

        Assert.IsFalse(hadError);
        Assert.HasCount(1, args);
        Assert.AreEqual(@"/testadapterpath:c:\Path\To\Adapters", args[0]);
    }

    // Regression test for #15304
    [TestMethod]
    public void SplitCommandLineIntoArguments_EmptyQuotedString_ShouldProduceEmptyArgument()
    {
        string input = @"arg1 """" arg3";
        bool hadError = CommandLineUtilities.SplitCommandLineIntoArguments(input, out string[] args);

        Assert.IsFalse(hadError);
        Assert.HasCount(3, args);
        Assert.AreEqual("arg1", args[0]);
        Assert.AreEqual("", args[1]);
        Assert.AreEqual("arg3", args[2]);
    }
}
