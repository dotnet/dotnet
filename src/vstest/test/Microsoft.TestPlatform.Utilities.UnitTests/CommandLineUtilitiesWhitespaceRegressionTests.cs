// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Utilities.Tests;

/// <summary>
/// Regression tests for CommandLineUtilities with multi-line and whitespace scenarios.
/// </summary>
[TestClass]
public class CommandLineUtilitiesWhitespaceRegressionTests
{
    // Regression test for #15304 — Answer file parsing
    [TestMethod]
    public void SplitCommandLineIntoArguments_MultiLineWithComments_ShouldParseCorrectly()
    {
        string input = "/param1\n# This is a comment\n/param2\n# Another comment\n/param3";
        bool hadError = CommandLineUtilities.SplitCommandLineIntoArguments(input, out string[] args);

        Assert.IsFalse(hadError);
        Assert.HasCount(3, args);
        Assert.AreEqual("/param1", args[0]);
        Assert.AreEqual("/param2", args[1]);
        Assert.AreEqual("/param3", args[2]);
    }

    // Regression test for #15304
    [TestMethod]
    public void SplitCommandLineIntoArguments_LeadingWhitespace_ShouldBeTrimmed()
    {
        string input = "   /param1   /param2   ";
        bool hadError = CommandLineUtilities.SplitCommandLineIntoArguments(input, out string[] args);

        Assert.IsFalse(hadError);
        Assert.HasCount(2, args);
        Assert.AreEqual("/param1", args[0]);
        Assert.AreEqual("/param2", args[1]);
    }

    // Regression test for #15304
    [TestMethod]
    public void SplitCommandLineIntoArguments_TabsAsWhitespace_ShouldSplit()
    {
        string input = "/param1\t/param2\t/param3";
        bool hadError = CommandLineUtilities.SplitCommandLineIntoArguments(input, out string[] args);

        Assert.IsFalse(hadError);
        Assert.HasCount(3, args);
    }

    // Regression test for #15304
    [TestMethod]
    public void SplitCommandLineIntoArguments_QuotedWithSpaces_ShouldKeepTogether()
    {
        string input = @"/filter:""Category=Unit Test""";
        bool hadError = CommandLineUtilities.SplitCommandLineIntoArguments(input, out string[] args);

        Assert.IsFalse(hadError);
        Assert.HasCount(1, args);
        Assert.AreEqual("/filter:Category=Unit Test", args[0]);
    }

    // Regression test for #15304
    [TestMethod]
    public void SplitCommandLineIntoArguments_CommentWithHashInMiddle_ShouldTreatAsComment()
    {
        string input = "arg1 # comment with arg2";
        bool hadError = CommandLineUtilities.SplitCommandLineIntoArguments(input, out string[] args);

        Assert.IsFalse(hadError);
        Assert.HasCount(1, args);
        Assert.AreEqual("arg1", args[0]);
    }

    // Regression test for #15304
    [TestMethod]
    public void SplitCommandLineIntoArguments_MultipleBackslashesBeforeQuote_ShouldHandleCorrectly()
    {
        // Three backslashes before a quote: \\\" -> backslash + escaped quote
        string input = """arg\\\\"value" """;
        bool hadError = CommandLineUtilities.SplitCommandLineIntoArguments(input, out string[] args);

        Assert.IsFalse(hadError);
        Assert.IsNotEmpty(args);
    }

    // Regression test for #15304
    [TestMethod]
    public void SplitCommandLineIntoArguments_AllComments_ShouldReturnEmpty()
    {
        string input = "# Just a comment\n# Another comment";
        bool hadError = CommandLineUtilities.SplitCommandLineIntoArguments(input, out string[] args);

        Assert.IsFalse(hadError);
        Assert.IsEmpty(args);
    }
}
