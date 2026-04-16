// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.Common.Filtering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Filter.Source.UnitTests;

[TestClass]
public class TestCaseFilterExpressionTests
{
    [TestMethod]
    public void ConstructorShouldThrowForNullFilterWrapper()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => new TestCaseFilterExpression(null!));
    }

    [TestMethod]
    public void TestCaseFilterValueShouldMatchFilterString()
    {
        var filterString = "Name=Test1";
        var wrapper = new FilterExpressionWrapper(filterString);
        var expression = new TestCaseFilterExpression(wrapper);

        Assert.AreEqual(filterString, expression.TestCaseFilterValue);
    }

    [TestMethod]
    public void MatchTestCaseShouldReturnTrueWhenPropertyMatches()
    {
        var wrapper = new FilterExpressionWrapper("Name=Test1");
        var expression = new TestCaseFilterExpression(wrapper);

        bool result = expression.MatchTestCase(prop => prop == "Name" ? "Test1" : null);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void MatchTestCaseShouldReturnFalseWhenPropertyDoesNotMatch()
    {
        var wrapper = new FilterExpressionWrapper("Name=Test1");
        var expression = new TestCaseFilterExpression(wrapper);

        bool result = expression.MatchTestCase(prop => prop == "Name" ? "Test2" : null);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void MatchTestCaseShouldReturnFalseWhenFilterHasParseError()
    {
        var wrapper = new FilterExpressionWrapper("(Name=Test1");
        var expression = new TestCaseFilterExpression(wrapper);

        bool result = expression.MatchTestCase(prop => "Test1");

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void ValidForPropertiesShouldReturnNullWhenAllPropertiesAreSupported()
    {
        var wrapper = new FilterExpressionWrapper("Name=Test1");
        var expression = new TestCaseFilterExpression(wrapper);
        var supportedProperties = new List<string> { "Name" };

        string[]? invalidProperties = expression.ValidForProperties(supportedProperties);

        Assert.IsNull(invalidProperties);
    }

    [TestMethod]
    public void ValidForPropertiesShouldReturnUnsupportedPropertyNames()
    {
        var wrapper = new FilterExpressionWrapper("UnknownProperty=Value");
        var expression = new TestCaseFilterExpression(wrapper);
        var supportedProperties = new List<string> { "Name", "Category" };

        string[]? invalidProperties = expression.ValidForProperties(supportedProperties);

        Assert.IsNotNull(invalidProperties);
        Assert.HasCount(1, invalidProperties);
        Assert.AreEqual("UnknownProperty", invalidProperties[0]);
    }

    [TestMethod]
    public void ValidForPropertiesShouldReturnNullWhenFilterHasParseError()
    {
        var wrapper = new FilterExpressionWrapper("(Name=Test1");
        var expression = new TestCaseFilterExpression(wrapper);
        var supportedProperties = new List<string> { "Name" };

        // When filter has parse error, ValidForProperties returns null
        string[]? invalidProperties = expression.ValidForProperties(supportedProperties);

        Assert.IsNull(invalidProperties);
    }

    [TestMethod]
    public void MatchTestCaseShouldSupportOrOperator()
    {
        var wrapper = new FilterExpressionWrapper("Name=Test1|Name=Test2");
        var expression = new TestCaseFilterExpression(wrapper);

        bool matchFirst = expression.MatchTestCase(prop => prop == "Name" ? "Test1" : null);
        bool matchSecond = expression.MatchTestCase(prop => prop == "Name" ? "Test2" : null);
        bool matchNeither = expression.MatchTestCase(prop => prop == "Name" ? "Test3" : null);

        Assert.IsTrue(matchFirst);
        Assert.IsTrue(matchSecond);
        Assert.IsFalse(matchNeither);
    }

    [TestMethod]
    public void MatchTestCaseShouldSupportAndOperator()
    {
        var wrapper = new FilterExpressionWrapper("Name=Test1&Category=Unit");
        var expression = new TestCaseFilterExpression(wrapper);

        bool result = expression.MatchTestCase(prop => prop switch
        {
            "Name" => "Test1",
            "Category" => "Unit",
            _ => null,
        });

        Assert.IsTrue(result);
    }
}
