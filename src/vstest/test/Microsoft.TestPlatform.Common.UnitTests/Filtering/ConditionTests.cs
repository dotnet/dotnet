// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.Common.Filtering;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Common.UnitTests.Filtering;

[TestClass]
public class ConditionTests
{
    [TestMethod]
    public void ParseShouldThrownFormatExceptionOnNullConditionString()
    {
        string? conditionString = null;
        Assert.ThrowsException<FormatException>(() => Condition.Parse(conditionString));
    }

    [TestMethod]
    public void ParseShouldThrownFormatExceptionOnEmptyConditionString()
    {
        var conditionString = "";
        Assert.ThrowsException<FormatException>(() => Condition.Parse(conditionString));
    }

    [TestMethod]
    public void ParseShouldThrownFormatExceptionOnIncompleteConditionString()
    {
        var conditionString = "PropertyName=";
        Assert.ThrowsException<FormatException>(() => Condition.Parse(conditionString));
    }

    [TestMethod]
    public void ParseShouldCreateDefaultConditionWhenOnlyPropertyValuePassed()
    {
        var conditionString = "ABC";
        Condition condition = Condition.Parse(conditionString);
        Assert.AreEqual(Condition.DefaultPropertyName, condition.Name);
        Assert.AreEqual(Operation.Contains, condition.Operation);
        Assert.AreEqual(conditionString, condition.Value);
    }

    [TestMethod]
    public void ParseShouldCreateProperConditionOnValidConditionString()
    {
        var conditionString = "PropertyName=PropertyValue";
        Condition condition = Condition.Parse(conditionString);
        Assert.AreEqual("PropertyName", condition.Name);
        Assert.AreEqual(Operation.Equal, condition.Operation);
        Assert.AreEqual("PropertyValue", condition.Value);
    }

    [TestMethod]
    public void ParseShouldHandleEscapedString()
    {
        var conditionString = @"FullyQualifiedName=TestClass1\(""hello""\).TestMethod\(1.5\)";

        Condition condition = Condition.Parse(conditionString);
        Assert.AreEqual("FullyQualifiedName", condition.Name);
        Assert.AreEqual(Operation.Equal, condition.Operation);
        Assert.AreEqual(@"TestClass1(""hello"").TestMethod(1.5)", condition.Value);
    }

    [TestMethod]
    public void ParseShouldHandleEscapedBang()
    {
        var conditionString = @"FullyQualifiedName!=TestClass1\(""\!""\).TestMethod\(1.5\)";

        Condition condition = Condition.Parse(conditionString);
        Assert.AreEqual("FullyQualifiedName", condition.Name);
        Assert.AreEqual(Operation.NotEqual, condition.Operation);
        Assert.AreEqual(@"TestClass1(""!"").TestMethod(1.5)", condition.Value);
    }

    [TestMethod]
    public void ParseShouldHandleEscapedNotEqual()
    {
        var conditionString = @"FullyQualifiedName!=TestClass1\(""\!\=""\).TestMethod\(1.5\)";

        Condition condition = Condition.Parse(conditionString);
        Assert.AreEqual("FullyQualifiedName", condition.Name);
        Assert.AreEqual(Operation.NotEqual, condition.Operation);
        Assert.AreEqual(@"TestClass1(""!="").TestMethod(1.5)", condition.Value);
    }

    [TestMethod]
    public void ParseShouldHandleEscapedTilde()
    {
        var conditionString = @"FullyQualifiedName~TestClass1\(""\~""\).TestMethod\(1.5\)";

        Condition condition = Condition.Parse(conditionString);
        Assert.AreEqual("FullyQualifiedName", condition.Name);
        Assert.AreEqual(Operation.Contains, condition.Operation);
        Assert.AreEqual(@"TestClass1(""~"").TestMethod(1.5)", condition.Value);
    }

    [TestMethod]
    public void ParseShouldHandleEscapedNotTilde()
    {
        var conditionString = @"FullyQualifiedName!~TestClass1\(""\!\~""\).TestMethod\(1.5\)";

        Condition condition = Condition.Parse(conditionString);
        Assert.AreEqual("FullyQualifiedName", condition.Name);
        Assert.AreEqual(Operation.NotContains, condition.Operation);
        Assert.AreEqual(@"TestClass1(""!~"").TestMethod(1.5)", condition.Value);
    }

    [TestMethod]
    public void ParseStringWithSingleUnescapedBangThrowsFormatException1()
    {
        var conditionString = @"FullyQualifiedName=Test1(""!"")";

        Assert.ThrowsException<FormatException>(() => Condition.Parse(conditionString));
    }

    [TestMethod]
    public void ParseStringWithSingleUnescapedBangThrowsFormatException2()
    {
        var conditionString = @"FullyQualifiedName!Test1()";

        Assert.ThrowsException<FormatException>(() => Condition.Parse(conditionString));
    }

    [TestMethod]
    public void TokenizeNullThrowsArgumentNullException()
    {
        Assert.ThrowsException<ArgumentNullException>(() => Condition.TokenizeFilterConditionString(null!), "str");
    }

    [TestMethod]
    public void TokenizeConditionShouldHandleEscapedBang()
    {
        var conditionString = @"FullyQualifiedName=TestMethod\(""\!""\)";

        var tokens = Condition.TokenizeFilterConditionString(conditionString).ToArray();

        Assert.AreEqual(3, tokens.Length);
        Assert.AreEqual("FullyQualifiedName", tokens[0]);
        Assert.AreEqual("=", tokens[1]);
        Assert.AreEqual(@"TestMethod\(""\!""\)", tokens[2]);
    }

    [TestMethod]
    public void TokenizeConditionShouldHandleEscapedNotEqual1()
    {
        var conditionString = @"FullyQualifiedName=TestMethod\(""\!\=""\)";

        var tokens = Condition.TokenizeFilterConditionString(conditionString).ToArray();

        Assert.AreEqual(3, tokens.Length);
        Assert.AreEqual("FullyQualifiedName", tokens[0]);
        Assert.AreEqual("=", tokens[1]);
        Assert.AreEqual(@"TestMethod\(""\!\=""\)", tokens[2]);
    }

    [TestMethod]
    public void TokenizeConditionShouldHandleEscapedNotEqual2()
    {
        var conditionString = @"FullyQualifiedName!=TestMethod\(""\!\=""\)";

        var tokens = Condition.TokenizeFilterConditionString(conditionString).ToArray();

        Assert.AreEqual(3, tokens.Length);
        Assert.AreEqual("FullyQualifiedName", tokens[0]);
        Assert.AreEqual("!=", tokens[1]);
        Assert.AreEqual(@"TestMethod\(""\!\=""\)", tokens[2]);
    }

    [TestMethod]
    public void TokenizeConditionShouldHandleEscapedBackslash()
    {
        var conditionString = @"FullyQualifiedName=TestMethod\(""\\""\)";

        var tokens = Condition.TokenizeFilterConditionString(conditionString).ToArray();

        Assert.AreEqual(3, tokens.Length);
        Assert.AreEqual("FullyQualifiedName", tokens[0]);
        Assert.AreEqual("=", tokens[1]);
        Assert.AreEqual(@"TestMethod\(""\\""\)", tokens[2]);
    }

    [TestMethod]
    public void TokenizeConditionShouldHandleEscapedTilde()
    {
        var conditionString = @"FullyQualifiedName~TestMethod\(""\~""\)";

        var tokens = Condition.TokenizeFilterConditionString(conditionString).ToArray();

        Assert.AreEqual(3, tokens.Length);
        Assert.AreEqual("FullyQualifiedName", tokens[0]);
        Assert.AreEqual("~", tokens[1]);
        Assert.AreEqual(@"TestMethod\(""\~""\)", tokens[2]);
    }

    [TestMethod]
    public void TokenizeConditionShouldHandleEscapedNotTilde()
    {
        var conditionString = @"FullyQualifiedName!~TestMethod\(""\!\~""\)";

        var tokens = Condition.TokenizeFilterConditionString(conditionString).ToArray();

        Assert.AreEqual(3, tokens.Length);
        Assert.AreEqual("FullyQualifiedName", tokens[0]);
        Assert.AreEqual("!~", tokens[1]);
        Assert.AreEqual(@"TestMethod\(""\!\~""\)", tokens[2]);
    }

    [TestMethod]
    public void TokenizeConditionShouldHandleSingleUnescapedBang()
    {
        var conditionString = @"FullyQualifiedName!=TestMethod\(""!""\)";

        var tokens = Condition.TokenizeFilterConditionString(conditionString).ToArray();

        Assert.AreEqual(5, tokens.Length);
        Assert.AreEqual("FullyQualifiedName", tokens[0]);
        Assert.AreEqual("!=", tokens[1]);
        Assert.AreEqual(@"TestMethod\(""", tokens[2]);
        Assert.AreEqual("!", tokens[3]);
        Assert.AreEqual(@"""\)", tokens[4]);
    }

    [TestMethod]
    public void TokenizeConditionShouldHandleSingleBangAtEnd()
    {
        var conditionString = "FullyQualifiedName!";

        var tokens = Condition.TokenizeFilterConditionString(conditionString).ToArray();

        Assert.AreEqual(2, tokens.Length);
        Assert.AreEqual("FullyQualifiedName", tokens[0]);
        Assert.AreEqual("!", tokens[1]);
    }
}
