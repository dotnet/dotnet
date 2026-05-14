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
        Assert.ThrowsExactly<FormatException>(() => Condition.Parse(conditionString));
    }

    [TestMethod]
    public void ParseShouldThrownFormatExceptionOnEmptyConditionString()
    {
        var conditionString = "";
        Assert.ThrowsExactly<FormatException>(() => Condition.Parse(conditionString));
    }

    [TestMethod]
    public void ParseShouldThrownFormatExceptionOnIncompleteConditionString()
    {
        var conditionString = "PropertyName=";
        Assert.ThrowsExactly<FormatException>(() => Condition.Parse(conditionString));
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

        Assert.ThrowsExactly<FormatException>(() => Condition.Parse(conditionString));
    }

    [TestMethod]
    public void ParseStringWithSingleUnescapedBangThrowsFormatException2()
    {
        var conditionString = @"FullyQualifiedName!Test1()";

        Assert.ThrowsExactly<FormatException>(() => Condition.Parse(conditionString));
    }

    [TestMethod]
    public void TokenizeNullThrowsArgumentNullException()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => Condition.TokenizeFilterConditionString(null!), "str");
    }

    [TestMethod]
    public void TokenizeConditionShouldHandleEscapedBang()
    {
        var conditionString = @"FullyQualifiedName=TestMethod\(""\!""\)";

        var tokens = Condition.TokenizeFilterConditionString(conditionString).ToArray();

        Assert.HasCount(3, tokens);
        Assert.AreEqual("FullyQualifiedName", tokens[0]);
        Assert.AreEqual("=", tokens[1]);
        Assert.AreEqual(@"TestMethod\(""\!""\)", tokens[2]);
    }

    [TestMethod]
    public void TokenizeConditionShouldHandleEscapedNotEqual1()
    {
        var conditionString = @"FullyQualifiedName=TestMethod\(""\!\=""\)";

        var tokens = Condition.TokenizeFilterConditionString(conditionString).ToArray();

        Assert.HasCount(3, tokens);
        Assert.AreEqual("FullyQualifiedName", tokens[0]);
        Assert.AreEqual("=", tokens[1]);
        Assert.AreEqual(@"TestMethod\(""\!\=""\)", tokens[2]);
    }

    [TestMethod]
    public void TokenizeConditionShouldHandleEscapedNotEqual2()
    {
        var conditionString = @"FullyQualifiedName!=TestMethod\(""\!\=""\)";

        var tokens = Condition.TokenizeFilterConditionString(conditionString).ToArray();

        Assert.HasCount(3, tokens);
        Assert.AreEqual("FullyQualifiedName", tokens[0]);
        Assert.AreEqual("!=", tokens[1]);
        Assert.AreEqual(@"TestMethod\(""\!\=""\)", tokens[2]);
    }

    [TestMethod]
    public void TokenizeConditionShouldHandleEscapedBackslash()
    {
        var conditionString = @"FullyQualifiedName=TestMethod\(""\\""\)";

        var tokens = Condition.TokenizeFilterConditionString(conditionString).ToArray();

        Assert.HasCount(3, tokens);
        Assert.AreEqual("FullyQualifiedName", tokens[0]);
        Assert.AreEqual("=", tokens[1]);
        Assert.AreEqual(@"TestMethod\(""\\""\)", tokens[2]);
    }

    [TestMethod]
    public void TokenizeConditionShouldHandleEscapedTilde()
    {
        var conditionString = @"FullyQualifiedName~TestMethod\(""\~""\)";

        var tokens = Condition.TokenizeFilterConditionString(conditionString).ToArray();

        Assert.HasCount(3, tokens);
        Assert.AreEqual("FullyQualifiedName", tokens[0]);
        Assert.AreEqual("~", tokens[1]);
        Assert.AreEqual(@"TestMethod\(""\~""\)", tokens[2]);
    }

    [TestMethod]
    public void TokenizeConditionShouldHandleEscapedNotTilde()
    {
        var conditionString = @"FullyQualifiedName!~TestMethod\(""\!\~""\)";

        var tokens = Condition.TokenizeFilterConditionString(conditionString).ToArray();

        Assert.HasCount(3, tokens);
        Assert.AreEqual("FullyQualifiedName", tokens[0]);
        Assert.AreEqual("!~", tokens[1]);
        Assert.AreEqual(@"TestMethod\(""\!\~""\)", tokens[2]);
    }

    [TestMethod]
    public void TokenizeConditionShouldHandleSingleUnescapedBang()
    {
        var conditionString = @"FullyQualifiedName!=TestMethod\(""!""\)";

        var tokens = Condition.TokenizeFilterConditionString(conditionString).ToArray();

        Assert.HasCount(5, tokens);
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

        Assert.HasCount(2, tokens);
        Assert.AreEqual("FullyQualifiedName", tokens[0]);
        Assert.AreEqual("!", tokens[1]);
    }

    #region None filter value tests (uncategorized tests support)

    [TestMethod]
    public void ParseNoneValueShouldCreateCondition()
    {
        Condition condition = Condition.Parse("TestCategory=None");

        Assert.AreEqual("TestCategory", condition.Name);
        Assert.AreEqual(Operation.Equal, condition.Operation);
        Assert.AreEqual("None", condition.Value);
    }

    [TestMethod]
    public void ParseNoneValueWithNotEqualShouldCreateCondition()
    {
        Condition condition = Condition.Parse("TestCategory!=None");

        Assert.AreEqual("TestCategory", condition.Name);
        Assert.AreEqual(Operation.NotEqual, condition.Operation);
        Assert.AreEqual("None", condition.Value);
    }

    [TestMethod]
    public void EvaluateNoneEqualWithNullPropertyShouldReturnTrue()
    {
        var condition = new Condition("TestCategory", Operation.Equal, "None");
        bool result = condition.Evaluate(propertyName => null);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void EvaluateNoneEqualWithEmptyArrayShouldReturnTrue()
    {
        var condition = new Condition("TestCategory", Operation.Equal, "None");
        bool result = condition.Evaluate(propertyName => Array.Empty<string>());

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void EvaluateNoneEqualWithNonEmptyArrayShouldReturnFalse()
    {
        var condition = new Condition("TestCategory", Operation.Equal, "None");
        bool result = condition.Evaluate(propertyName => new[] { "CategoryA" });

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void EvaluateNoneEqualIsCaseInsensitive()
    {
        var condition = new Condition("TestCategory", Operation.Equal, "none");
        Assert.IsTrue(condition.Evaluate(propertyName => null));

        var condition2 = new Condition("TestCategory", Operation.Equal, "NONE");
        Assert.IsTrue(condition2.Evaluate(propertyName => null));
    }

    [TestMethod]
    public void EvaluateNoneNotEqualWithNullPropertyShouldReturnFalse()
    {
        var condition = new Condition("TestCategory", Operation.NotEqual, "None");
        bool result = condition.Evaluate(propertyName => null);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void EvaluateNoneNotEqualWithEmptyArrayShouldReturnFalse()
    {
        var condition = new Condition("TestCategory", Operation.NotEqual, "None");
        bool result = condition.Evaluate(propertyName => Array.Empty<string>());

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void EvaluateNoneNotEqualWithNonEmptyArrayShouldReturnTrue()
    {
        var condition = new Condition("TestCategory", Operation.NotEqual, "None");
        bool result = condition.Evaluate(propertyName => new[] { "CategoryA" });

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void EvaluateNonNoneValueShouldStillWorkNormally()
    {
        var condition = new Condition("TestCategory", Operation.Equal, "CategoryA");
        Assert.IsTrue(condition.Evaluate(propertyName => new[] { "CategoryA" }));
        Assert.IsFalse(condition.Evaluate(propertyName => new[] { "CategoryB" }));
        Assert.IsFalse(condition.Evaluate(propertyName => null));
    }

    [TestMethod]
    public void EvaluateNoneEqualWithExplicitNoneCategoryShouldReturnTrue()
    {
        // A test with [TestCategory("None")] should also match TestCategory=None.
        // "None" is reserved for uncategorized, but tests that literally use it
        // are included as well (by design, to avoid silent mismatches).
        var condition = new Condition("TestCategory", Operation.Equal, "None");
        bool result = condition.Evaluate(propertyName => new[] { "None" });

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void EvaluateNoneNotEqualWithExplicitNoneCategoryShouldReturnFalse()
    {
        // A test with [TestCategory("None")] should NOT match TestCategory!=None.
        var condition = new Condition("TestCategory", Operation.NotEqual, "None");
        bool result = condition.Evaluate(propertyName => new[] { "None" });

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void EvaluateNoneEqualWithExplicitNoneCategoryAmongOthersShouldReturnTrue()
    {
        // A test with [TestCategory("None")] and [TestCategory("CategoryA")]
        // should match TestCategory=None because one of the values equals "None".
        var condition = new Condition("TestCategory", Operation.Equal, "None");
        bool result = condition.Evaluate(propertyName => new[] { "None", "CategoryA" });

        Assert.IsTrue(result);
    }

    #endregion
}
