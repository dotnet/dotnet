// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.TestPlatform.AdapterUtilities.ManagedNameUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AdapterUtilities.UnitTests.ManagedNameUtilities;

/// <summary>
/// Regression tests for ManagedNameParser type and method name parsing edge cases.
/// </summary>
[TestClass]
public class ManagedNameParserEdgeCaseTests
{
    // Regression test for #15259 — Cache AssemblyName in ManagedNameHelper
    // Regression test for #15255 — Do half the work in GetManagedName
    // These tests verify that managed name parsing produces correct results for various patterns.

    [TestMethod]
    public void ParseManagedTypeName_NestedType_ShouldParseCorrectly()
    {
        ManagedNameParser.ParseManagedTypeName("Namespace.OuterClass+InnerClass", out string namespaceName, out string typeName);

        Assert.AreEqual("Namespace", namespaceName);
        Assert.AreEqual("OuterClass+InnerClass", typeName);
    }

    [TestMethod]
    public void ParseManagedTypeName_NoNamespace_ShouldReturnEmptyNamespace()
    {
        ManagedNameParser.ParseManagedTypeName("GlobalClass", out string namespaceName, out string typeName);

        Assert.AreEqual(string.Empty, namespaceName);
        Assert.AreEqual("GlobalClass", typeName);
    }

    [TestMethod]
    public void ParseManagedTypeName_DeepNamespace_ShouldParseCorrectly()
    {
        ManagedNameParser.ParseManagedTypeName("A.B.C.D.E.ClassName", out string namespaceName, out string typeName);

        Assert.AreEqual("A.B.C.D.E", namespaceName);
        Assert.AreEqual("ClassName", typeName);
    }

    [TestMethod]
    public void ParseManagedMethodName_NoParameters_ShouldReturnNullParamTypes()
    {
        ManagedNameParser.ParseManagedMethodName("SimpleMethod", out string methodName, out int arity, out string[]? parameterTypes);

        Assert.AreEqual("SimpleMethod", methodName);
        Assert.AreEqual(0, arity);
        Assert.IsNull(parameterTypes);
    }

    [TestMethod]
    public void ParseManagedMethodName_EmptyParameters_ShouldReturnNullParamTypes()
    {
        ManagedNameParser.ParseManagedMethodName("Method()", out string methodName, out int arity, out string[]? parameterTypes);

        Assert.AreEqual("Method", methodName);
        Assert.AreEqual(0, arity);
        Assert.IsNull(parameterTypes);
    }

    [TestMethod]
    public void ParseManagedMethodName_MultipleParams_ShouldParseAll()
    {
        ManagedNameParser.ParseManagedMethodName("Method(System.Int32,System.String,System.Boolean)",
            out string methodName, out int arity, out string[]? parameterTypes);

        Assert.AreEqual("Method", methodName);
        Assert.AreEqual(0, arity);
        Assert.IsNotNull(parameterTypes);
        Assert.HasCount(3, parameterTypes!);
        Assert.AreEqual("System.Int32", parameterTypes[0]);
        Assert.AreEqual("System.String", parameterTypes[1]);
        Assert.AreEqual("System.Boolean", parameterTypes[2]);
    }

    [TestMethod]
    public void ParseManagedMethodName_WithArity_ShouldParseArityCorrectly()
    {
        ManagedNameParser.ParseManagedMethodName("Method`3(System.Int32)",
            out string methodName, out int arity, out string[]? parameterTypes);

        Assert.AreEqual("Method", methodName);
        Assert.AreEqual(3, arity);
        Assert.IsNotNull(parameterTypes);
    }

    [TestMethod]
    public void ParseManagedMethodName_WhitespaceInName_ShouldThrow()
    {
        Assert.ThrowsExactly<InvalidManagedNameException>(
            () => ManagedNameParser.ParseManagedMethodName("Method Name", out _, out _, out _));
    }

    [TestMethod]
    public void ParseManagedMethodName_NonNumericArity_ShouldThrow()
    {
        Assert.ThrowsExactly<InvalidManagedNameException>(
            () => ManagedNameParser.ParseManagedMethodName("Method`abc", out _, out _, out _));
    }
}
