// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.TestPlatform.AdapterUtilities.ManagedNameUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AdapterUtilities.UnitTests.ManagedNameUtilities;

/// <summary>
/// Regression tests for ManagedNameParser type name parsing variations.
/// </summary>
[TestClass]
public class ManagedNameParserTypeNameRegressionTests
{
    // Regression test for #15259 — Cache AssemblyName in ManagedNameHelper
    // Regression test for #15255 — Do half the work in GetManagedName
    // Verify type name parsing correctness for various patterns.

    [TestMethod]
    public void ParseManagedTypeName_SimpleType_ShouldParse()
    {
        ManagedNameParser.ParseManagedTypeName("MyNamespace.MyClass",
            out string namespaceName, out string typeName);

        Assert.AreEqual("MyNamespace", namespaceName);
        Assert.AreEqual("MyClass", typeName);
    }

    [TestMethod]
    public void ParseManagedTypeName_GenericType_ShouldParseEntireTypeName()
    {
        ManagedNameParser.ParseManagedTypeName("MyNamespace.MyClass`2",
            out string namespaceName, out string typeName);

        Assert.AreEqual("MyNamespace", namespaceName);
        Assert.AreEqual("MyClass`2", typeName);
    }

    [TestMethod]
    public void ParseManagedTypeName_DeeplyNestedType_ShouldParseCorrectly()
    {
        ManagedNameParser.ParseManagedTypeName("A.B.C+D+E",
            out string namespaceName, out string typeName);

        Assert.AreEqual("A.B", namespaceName);
        Assert.AreEqual("C+D+E", typeName);
    }

    [TestMethod]
    public void ParseManagedTypeName_EmptyString_ShouldReturnAsTypeName()
    {
        ManagedNameParser.ParseManagedTypeName("",
            out string namespaceName, out string typeName);

        Assert.AreEqual(string.Empty, namespaceName);
        Assert.AreEqual("", typeName);
    }
}
