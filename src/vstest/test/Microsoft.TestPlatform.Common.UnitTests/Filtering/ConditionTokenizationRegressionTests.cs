// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.Common.Filtering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.Common.UnitTests.Filtering;

/// <summary>
/// Regression tests for Condition tokenization of filter strings.
/// </summary>
[TestClass]
public class ConditionTokenizationRegressionTests
{
    // Regression test for #15357 — Refactor Condition evaluation
    // Verifying tokenization works correctly with special characters.

    [TestMethod]
    public void TokenizeFilterConditionString_EqualOperator_ShouldProduceThreeTokens()
    {
        var tokens = Condition.TokenizeFilterConditionString("Name=Value").ToArray();

        Assert.HasCount(3, tokens);
        Assert.AreEqual("Name", tokens[0]);
        Assert.AreEqual("=", tokens[1]);
        Assert.AreEqual("Value", tokens[2]);
    }

    [TestMethod]
    public void TokenizeFilterConditionString_NotEqualOperator_ShouldProduceThreeTokens()
    {
        var tokens = Condition.TokenizeFilterConditionString("Name!=Value").ToArray();

        Assert.HasCount(3, tokens);
        Assert.AreEqual("Name", tokens[0]);
        Assert.AreEqual("!=", tokens[1]);
        Assert.AreEqual("Value", tokens[2]);
    }

    [TestMethod]
    public void TokenizeFilterConditionString_ContainsOperator_ShouldProduceThreeTokens()
    {
        var tokens = Condition.TokenizeFilterConditionString("Name~Value").ToArray();

        Assert.HasCount(3, tokens);
        Assert.AreEqual("Name", tokens[0]);
        Assert.AreEqual("~", tokens[1]);
        Assert.AreEqual("Value", tokens[2]);
    }

    [TestMethod]
    public void TokenizeFilterConditionString_NotContainsOperator_ShouldProduceThreeTokens()
    {
        var tokens = Condition.TokenizeFilterConditionString("Name!~Value").ToArray();

        Assert.HasCount(3, tokens);
        Assert.AreEqual("Name", tokens[0]);
        Assert.AreEqual("!~", tokens[1]);
        Assert.AreEqual("Value", tokens[2]);
    }

    [TestMethod]
    public void TokenizeFilterConditionString_EscapedEquals_ShouldNotSplitOnEscaped()
    {
        var tokens = Condition.TokenizeFilterConditionString(@"Name=Value\=More").ToArray();

        Assert.HasCount(3, tokens);
        Assert.AreEqual("Name", tokens[0]);
        Assert.AreEqual("=", tokens[1]);
        Assert.AreEqual(@"Value\=More", tokens[2]);
    }

    [TestMethod]
    public void TokenizeFilterConditionString_EscapedBackslash_ShouldPreserve()
    {
        var tokens = Condition.TokenizeFilterConditionString(@"Name=Value\\End").ToArray();

        Assert.HasCount(3, tokens);
        Assert.AreEqual("Name", tokens[0]);
        Assert.AreEqual("=", tokens[1]);
        Assert.AreEqual(@"Value\\End", tokens[2]);
    }

    [TestMethod]
    public void TokenizeFilterConditionString_ValueOnly_ShouldProduceSingleToken()
    {
        var tokens = Condition.TokenizeFilterConditionString("JustAValue").ToArray();

        Assert.HasCount(1, tokens);
        Assert.AreEqual("JustAValue", tokens[0]);
    }

    [TestMethod]
    public void TokenizeFilterConditionString_Null_ShouldThrow()
    {
        Assert.ThrowsExactly<ArgumentNullException>(
            () => Condition.TokenizeFilterConditionString(null!).ToArray());
    }
}
