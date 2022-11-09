// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.Common.Filtering;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Common.UnitTests.Filtering;

[TestClass]
public class FilterExpressionTests
{
    [TestMethod]
    public void TokenizeNullThrowsArgumentNullException()
    {
        Assert.ThrowsException<ArgumentNullException>(() => FilterExpression.TokenizeFilterExpressionString(null!), "str");
    }

    [TestMethod]
    public void TokenizeFilterShouldHandleEscapedParenthesis()
    {
        var conditionString = @"(T1\(\) | T2\(\))";

        var tokens = FilterExpression.TokenizeFilterExpressionString(conditionString).ToArray();

        Assert.AreEqual(5, tokens.Length);
        Assert.AreEqual("(", tokens[0]);
        Assert.AreEqual(@"T1\(\) ", tokens[1]);
        Assert.AreEqual(@"|", tokens[2]);
        Assert.AreEqual(@" T2\(\)", tokens[3]);
        Assert.AreEqual(")", tokens[4]);
    }

    [TestMethod]
    public void TokenizeFilterShouldHandleEmptyParenthesis()
    {
        var conditionString = @"  (  )  ";

        var tokens = FilterExpression.TokenizeFilterExpressionString(conditionString).ToArray();

        Assert.AreEqual(5, tokens.Length);
        Assert.AreEqual("  ", tokens[0]);
        Assert.AreEqual("(", tokens[1]);
        Assert.AreEqual("  ", tokens[2]);
        Assert.AreEqual(")", tokens[3]);
        Assert.AreEqual("  ", tokens[4]);
    }

    [TestMethod]
    public void TokenizeFilterShouldHandleEscapedBackslash()
    {
        var conditionString = @"(FQN!=T1\(""\\""\) | FQN!=T2\(\))";

        var tokens = FilterExpression.TokenizeFilterExpressionString(conditionString).ToArray();

        Assert.AreEqual(5, tokens.Length);
        Assert.AreEqual("(", tokens[0]);
        Assert.AreEqual(@"FQN!=T1\(""\\""\) ", tokens[1]);
        Assert.AreEqual(@"|", tokens[2]);
        Assert.AreEqual(@" FQN!=T2\(\)", tokens[3]);
        Assert.AreEqual(")", tokens[4]);
    }

    [TestMethod]
    public void TokenizeFilterShouldHandleNestedParenthesis()
    {
        var conditionString = @"((FQN!=T1|FQN!=T2)&(Category=Foo\(\)))";

        var tokens = FilterExpression.TokenizeFilterExpressionString(conditionString).ToArray();

        Assert.AreEqual(11, tokens.Length);
        Assert.AreEqual("(", tokens[0]);
        Assert.AreEqual("(", tokens[1]);
        Assert.AreEqual(@"FQN!=T1", tokens[2]);
        Assert.AreEqual(@"|", tokens[3]);
        Assert.AreEqual(@"FQN!=T2", tokens[4]);
        Assert.AreEqual(")", tokens[5]);
        Assert.AreEqual("&", tokens[6]);
        Assert.AreEqual("(", tokens[7]);
        Assert.AreEqual(@"Category=Foo\(\)", tokens[8]);
        Assert.AreEqual(")", tokens[9]);
        Assert.AreEqual(")", tokens[10]);
    }

    [TestMethod]
    public void TokenizeFilterShouldHandleInvalidEscapeSequence()
    {
        var conditionString = @"(T1\#\#)|T2\)";

        var tokens = FilterExpression.TokenizeFilterExpressionString(conditionString).ToArray();

        Assert.AreEqual(5, tokens.Length);
        Assert.AreEqual("(", tokens[0]);
        Assert.AreEqual(@"T1\#\#", tokens[1]);
        Assert.AreEqual(@")", tokens[2]);
        Assert.AreEqual(@"|", tokens[3]);
        Assert.AreEqual(@"T2\)", tokens[4]);
    }
}
