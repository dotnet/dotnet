// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.Common.Filtering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.Common.UnitTests.Filtering;

/// <summary>
/// Regression tests for Condition.Evaluate refactored operations.
/// </summary>
[TestClass]
public class ConditionEvaluationRegressionTests
{
    // Regression test for #15357 — Refactor Condition evaluation
    // EvaluateEqualOperation and EvaluateContainsOperation were extracted as helpers.
    // Verify all four operations work correctly.

    [TestMethod]
    public void Evaluate_EqualOperation_SingleValueMatch_ShouldReturnTrue()
    {
        var condition = new Condition("Category", Operation.Equal, "UnitTest");
        bool result = condition.Evaluate(name => name == "Category" ? "UnitTest" : null);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Evaluate_EqualOperation_SingleValueNoMatch_ShouldReturnFalse()
    {
        var condition = new Condition("Category", Operation.Equal, "UnitTest");
        bool result = condition.Evaluate(name => name == "Category" ? "Integration" : null);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Evaluate_EqualOperation_MultiValue_AnyMatch_ShouldReturnTrue()
    {
        var condition = new Condition("Category", Operation.Equal, "UnitTest");
        bool result = condition.Evaluate(name => name == "Category"
            ? new string[] { "Integration", "UnitTest", "Smoke" }
            : null);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Evaluate_EqualOperation_MultiValue_NoMatch_ShouldReturnFalse()
    {
        var condition = new Condition("Category", Operation.Equal, "UnitTest");
        bool result = condition.Evaluate(name => name == "Category"
            ? new string[] { "Integration", "Smoke" }
            : null);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Evaluate_NotEqualOperation_NoMatch_ShouldReturnTrue()
    {
        var condition = new Condition("Category", Operation.NotEqual, "UnitTest");
        bool result = condition.Evaluate(name => name == "Category" ? "Integration" : null);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Evaluate_NotEqualOperation_Match_ShouldReturnFalse()
    {
        var condition = new Condition("Category", Operation.NotEqual, "UnitTest");
        bool result = condition.Evaluate(name => name == "Category" ? "UnitTest" : null);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Evaluate_NotEqualOperation_MultiValue_AllDifferent_ShouldReturnTrue()
    {
        var condition = new Condition("Category", Operation.NotEqual, "UnitTest");
        bool result = condition.Evaluate(name => name == "Category"
            ? new string[] { "Integration", "Smoke" }
            : null);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Evaluate_NotEqualOperation_MultiValue_AnyMatch_ShouldReturnFalse()
    {
        var condition = new Condition("Category", Operation.NotEqual, "UnitTest");
        bool result = condition.Evaluate(name => name == "Category"
            ? new string[] { "Integration", "UnitTest" }
            : null);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Evaluate_ContainsOperation_SubstringMatch_ShouldReturnTrue()
    {
        var condition = new Condition("FullyQualifiedName", Operation.Contains, "MyTests");
        bool result = condition.Evaluate(name => name == "FullyQualifiedName"
            ? "Namespace.MyTests.TestMethod1"
            : null);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Evaluate_ContainsOperation_NoSubstringMatch_ShouldReturnFalse()
    {
        var condition = new Condition("FullyQualifiedName", Operation.Contains, "MyTests");
        bool result = condition.Evaluate(name => name == "FullyQualifiedName"
            ? "Namespace.OtherTests.TestMethod1"
            : null);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Evaluate_NotContainsOperation_NoSubstring_ShouldReturnTrue()
    {
        var condition = new Condition("FullyQualifiedName", Operation.NotContains, "MyTests");
        bool result = condition.Evaluate(name => name == "FullyQualifiedName"
            ? "Namespace.OtherTests.TestMethod1"
            : null);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Evaluate_NotContainsOperation_SubstringPresent_ShouldReturnFalse()
    {
        var condition = new Condition("FullyQualifiedName", Operation.NotContains, "MyTests");
        bool result = condition.Evaluate(name => name == "FullyQualifiedName"
            ? "Namespace.MyTests.TestMethod1"
            : null);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Evaluate_EqualOperation_CaseInsensitive_ShouldReturnTrue()
    {
        var condition = new Condition("Category", Operation.Equal, "unittest");
        bool result = condition.Evaluate(name => name == "Category" ? "UnitTest" : null);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Evaluate_ContainsOperation_CaseInsensitive_ShouldReturnTrue()
    {
        var condition = new Condition("FullyQualifiedName", Operation.Contains, "mytests");
        bool result = condition.Evaluate(name => name == "FullyQualifiedName"
            ? "Namespace.MyTests.TestMethod1"
            : null);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Evaluate_NullPropertyValue_EqualShouldReturnFalse()
    {
        var condition = new Condition("Category", Operation.Equal, "UnitTest");
        bool result = condition.Evaluate(name => null);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Evaluate_NullPropertyValue_NotEqualShouldReturnTrue()
    {
        var condition = new Condition("Category", Operation.NotEqual, "UnitTest");
        bool result = condition.Evaluate(name => null);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Evaluate_ContainsOperation_MultiValue_AnyContains_ShouldReturnTrue()
    {
        var condition = new Condition("Tag", Operation.Contains, "unit");
        bool result = condition.Evaluate(name => name == "Tag"
            ? new string[] { "integration", "unit-test", "smoke" }
            : null);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Evaluate_NotContainsOperation_MultiValue_AllNotContaining_ShouldReturnTrue()
    {
        var condition = new Condition("Tag", Operation.NotContains, "unit");
        bool result = condition.Evaluate(name => name == "Tag"
            ? new string[] { "integration", "smoke", "e2e" }
            : null);
        Assert.IsTrue(result);
    }
}
