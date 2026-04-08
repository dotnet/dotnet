// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.Common.Filtering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.Common.UnitTests.Filtering;

/// <summary>
/// Regression tests for Condition.Parse and tokenization.
/// </summary>
[TestClass]
public class ConditionParsingRegressionTests
{
    // Regression test for #15357 — Refactor Condition evaluation
    // Verify that parsing and evaluation are integrated correctly.

    [TestMethod]
    public void Parse_EqualCondition_ShouldEvaluateCorrectly()
    {
        var condition = Condition.Parse("Category=UnitTest");

        Assert.IsTrue(condition.Evaluate(name => name == "Category" ? "UnitTest" : null));
        Assert.IsFalse(condition.Evaluate(name => name == "Category" ? "Integration" : null));
    }

    [TestMethod]
    public void Parse_NotEqualCondition_ShouldEvaluateCorrectly()
    {
        var condition = Condition.Parse("Category!=UnitTest");

        Assert.IsTrue(condition.Evaluate(name => name == "Category" ? "Integration" : null));
        Assert.IsFalse(condition.Evaluate(name => name == "Category" ? "UnitTest" : null));
    }

    [TestMethod]
    public void Parse_ContainsCondition_ShouldEvaluateCorrectly()
    {
        var condition = Condition.Parse("FullyQualifiedName~MyClass");

        Assert.IsTrue(condition.Evaluate(name => name == "FullyQualifiedName"
            ? "Namespace.MyClass.Method" : null));
        Assert.IsFalse(condition.Evaluate(name => name == "FullyQualifiedName"
            ? "Namespace.OtherClass.Method" : null));
    }

    [TestMethod]
    public void Parse_NotContainsCondition_ShouldEvaluateCorrectly()
    {
        var condition = Condition.Parse("FullyQualifiedName!~MyClass");

        Assert.IsTrue(condition.Evaluate(name => name == "FullyQualifiedName"
            ? "Namespace.OtherClass.Method" : null));
        Assert.IsFalse(condition.Evaluate(name => name == "FullyQualifiedName"
            ? "Namespace.MyClass.Method" : null));
    }

    [TestMethod]
    public void Parse_DefaultCondition_ShouldUseContainsOnFQN()
    {
        // When only a value is provided (no operator), it defaults to FullyQualifiedName Contains
        var condition = Condition.Parse("TestMethod");

        Assert.IsTrue(condition.Evaluate(name => name == "FullyQualifiedName"
            ? "Namespace.Class.TestMethod" : null));
    }

    [TestMethod]
    public void Parse_EscapedCondition_ShouldHandleSpecialChars()
    {
        // Escaped = should be treated as literal
        var condition = Condition.Parse(@"FullyQualifiedName~test\=method");

        Assert.IsTrue(condition.Evaluate(name => name == "FullyQualifiedName"
            ? "test=method" : null));
    }

    [TestMethod]
    public void Parse_EmptyString_ShouldThrow()
    {
        Assert.ThrowsExactly<FormatException>(() => Condition.Parse(""));
    }

    [TestMethod]
    public void Parse_NullString_ShouldThrow()
    {
        Assert.ThrowsExactly<FormatException>(() => Condition.Parse(null));
    }
}
