// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// The Microsoft.TestPlatform.Filter.Source package embeds its source files into this project
// as contentFiles. FilterExpressionWrapper and TestCaseFilterExpression are compiled as
// internal sealed types in this assembly (non-IS_VSTEST_REPO form). These tests verify that
// the package works correctly when consumed as a NuGet package.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.Common.Filtering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilterSourcePackageConsumerTests;

[TestClass]
public class FilterExpressionWrapperPackageTests
{
    [TestMethod]
    public void EvaluateShouldReturnTrueForMatchingFilter()
    {
        var wrapper = new FilterExpressionWrapper("Name=Test1");
        bool result = wrapper.Evaluate(p => p == "Name" ? "Test1" : null);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void EvaluateShouldReturnFalseForNonMatchingFilter()
    {
        var wrapper = new FilterExpressionWrapper("Name=Test1");
        bool result = wrapper.Evaluate(p => p == "Name" ? "Test2" : null);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void EvaluateShouldReturnFalseWhenFilterHasParseError()
    {
        var wrapper = new FilterExpressionWrapper("(Name=Test1");
        bool result = wrapper.Evaluate(p => "Test1");
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void EvaluateShouldSupportAndOperator()
    {
        var wrapper = new FilterExpressionWrapper("Name=Test1&Category=Unit");
        bool result = wrapper.Evaluate(p => p switch
        {
            "Name" => "Test1",
            "Category" => "Unit",
            _ => null,
        });
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void EvaluateShouldSupportOrOperator()
    {
        var wrapper = new FilterExpressionWrapper("Name=Test1|Name=Test2");
        Assert.IsTrue(wrapper.Evaluate(p => p == "Name" ? "Test1" : null));
        Assert.IsTrue(wrapper.Evaluate(p => p == "Name" ? "Test2" : null));
        Assert.IsFalse(wrapper.Evaluate(p => p == "Name" ? "Test3" : null));
    }

    [TestMethod]
    public void ValidForPropertiesShouldReturnNullForSupportedProperties()
    {
        var wrapper = new FilterExpressionWrapper("Name=Test1");
        string[] invalid = wrapper.ValidForProperties(new List<string> { "Name" });
        Assert.IsNull(invalid);
    }

    [TestMethod]
    public void ValidForPropertiesShouldReturnUnsupportedPropertyNames()
    {
        var wrapper = new FilterExpressionWrapper("UnknownProp=Value");
        string[] invalid = wrapper.ValidForProperties(new List<string> { "Name" });
        Assert.IsNotNull(invalid);
        Assert.AreEqual(1, invalid.Length);
        Assert.AreEqual("UnknownProp", invalid[0]);
    }
}

[TestClass]
public class TestCaseFilterExpressionPackageTests
{
    [TestMethod]
    public void MatchTestCaseShouldReturnTrueForMatchingFilter()
    {
        var wrapper = new FilterExpressionWrapper("Name=Test1");
        var expression = new TestCaseFilterExpression(wrapper);
        bool result = expression.MatchTestCase(p => p == "Name" ? "Test1" : null);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void MatchTestCaseShouldReturnFalseForNonMatchingFilter()
    {
        var wrapper = new FilterExpressionWrapper("Name=Test1");
        var expression = new TestCaseFilterExpression(wrapper);
        bool result = expression.MatchTestCase(p => p == "Name" ? "Test2" : null);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void MatchTestCaseShouldReturnFalseWhenFilterHasParseError()
    {
        var wrapper = new FilterExpressionWrapper("(Name=Test1");
        var expression = new TestCaseFilterExpression(wrapper);
        bool result = expression.MatchTestCase(p => "Test1");
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void MatchTestCaseShouldSupportOrOperator()
    {
        var wrapper = new FilterExpressionWrapper("Name=Test1|Name=Test2");
        var expression = new TestCaseFilterExpression(wrapper);
        Assert.IsTrue(expression.MatchTestCase(p => p == "Name" ? "Test1" : null));
        Assert.IsTrue(expression.MatchTestCase(p => p == "Name" ? "Test2" : null));
        Assert.IsFalse(expression.MatchTestCase(p => p == "Name" ? "Test3" : null));
    }

    [TestMethod]
    public void TestCaseFilterValueShouldReturnOriginalFilterString()
    {
        var filterString = "Name=Test1";
        var wrapper = new FilterExpressionWrapper(filterString);
        var expression = new TestCaseFilterExpression(wrapper);
        Assert.AreEqual(filterString, expression.TestCaseFilterValue);
    }

    [TestMethod]
    public void ValidForPropertiesShouldReturnNullForSupportedProperties()
    {
        var wrapper = new FilterExpressionWrapper("Name=Test1");
        var expression = new TestCaseFilterExpression(wrapper);
        string[] invalid = expression.ValidForProperties(new List<string> { "Name" });
        Assert.IsNull(invalid);
    }

    [TestMethod]
    public void ValidForPropertiesShouldReturnUnsupportedPropertyNames()
    {
        var wrapper = new FilterExpressionWrapper("UnknownProp=Value");
        var expression = new TestCaseFilterExpression(wrapper);
        string[] invalid = expression.ValidForProperties(new List<string> { "Name" });
        Assert.IsNotNull(invalid);
        Assert.AreEqual(1, invalid.Length);
        Assert.AreEqual("UnknownProp", invalid[0]);
    }
}
