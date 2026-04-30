// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.TestPlatform.AdapterUtilities.ManagedNameUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AdapterUtilities.UnitTests.ManagedNameUtilities;

/// <summary>
/// Regression tests for F# method name unescaping in ManagedNameParser.
/// </summary>
[TestClass]
public class ManagedNameParserFSharpTests
{
    // Regression test for #4972 — Unescaping F# method names
    // F# methods wrapped in `` backticks are emitted into CIL with single quotes.
    // If the method name itself contains a single quote, F# emits it as \' in CIL.
    [TestMethod]
    public void ParseMethodName_FSharpEscapedSingleQuote_ShouldParseCorrectly()
    {
        // In CIL, F# method ``don't pass`` becomes 'don\'t pass'
        string managedMethodName = "'don\\'t pass'";
        ManagedNameParser.ParseManagedMethodName(managedMethodName, out string methodName, out int arity, out string[]? parameterTypes);

        Assert.AreEqual("don't pass", methodName);
        Assert.AreEqual(0, arity);
        Assert.IsNull(parameterTypes);
    }

    // Regression test for #4972
    [TestMethod]
    public void ParseMethodName_FSharpQuotedSimpleName_ShouldParseCorrectly()
    {
        // Simple F# method name wrapped in single quotes
        string managedMethodName = "'my method'";
        ManagedNameParser.ParseManagedMethodName(managedMethodName, out string methodName, out int arity, out string[]? parameterTypes);

        Assert.AreEqual("my method", methodName);
        Assert.AreEqual(0, arity);
        Assert.IsNull(parameterTypes);
    }

    // Regression test for #4972
    [TestMethod]
    public void ParseMethodName_FSharpQuotedWithParameters_ShouldParseCorrectly()
    {
        // F# method name with parameters
        string managedMethodName = "'my method'(System.Int32)";
        ManagedNameParser.ParseManagedMethodName(managedMethodName, out string methodName, out int arity, out string[]? parameterTypes);

        Assert.AreEqual("my method", methodName);
        Assert.AreEqual(0, arity);
        Assert.IsNotNull(parameterTypes);
        Assert.HasCount(1, parameterTypes!);
        Assert.AreEqual("System.Int32", parameterTypes[0]);
    }

    // Regression test for #4972
    [TestMethod]
    public void ParseMethodName_RegularMethodName_ShouldStillWork()
    {
        string managedMethodName = "TestMethod(System.String)";
        ManagedNameParser.ParseManagedMethodName(managedMethodName, out string methodName, out int arity, out string[]? parameterTypes);

        Assert.AreEqual("TestMethod", methodName);
        Assert.AreEqual(0, arity);
        Assert.IsNotNull(parameterTypes);
        Assert.HasCount(1, parameterTypes!);
    }

    // Regression test for #4972
    [TestMethod]
    public void ParseMethodName_GenericFSharpMethod_ShouldParseCorrectly()
    {
        // Generic method with arity
        string managedMethodName = "GenericMethod`2(System.Int32,System.String)";
        ManagedNameParser.ParseManagedMethodName(managedMethodName, out string methodName, out int arity, out string[]? parameterTypes);

        Assert.AreEqual("GenericMethod", methodName);
        Assert.AreEqual(2, arity);
        Assert.IsNotNull(parameterTypes);
        Assert.HasCount(2, parameterTypes!);
    }
}
