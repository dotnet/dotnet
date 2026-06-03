// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.Common.Filtering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Filter.Source.UnitTests;

[TestClass]
public class FilterExpressionWrapperTests
{
    [TestMethod]
    public void ConstructorShouldSetFilterString()
    {
        var filterString = "FullyQualifiedName=Test1";
        var wrapper = new FilterExpressionWrapper(filterString);
        Assert.AreEqual(filterString, wrapper.FilterString);
    }

    [TestMethod]
    public void ParseErrorShouldBeNullForValidFilter()
    {
        var wrapper = new FilterExpressionWrapper("Name=Test1");
        Assert.IsNull(wrapper.ParseError);
    }

    [TestMethod]
    public void ParseErrorShouldBeSetForInvalidFilter()
    {
        var wrapper = new FilterExpressionWrapper("(Name=Test1");
        Assert.IsNotNull(wrapper.ParseError);
    }

    [TestMethod]
    public void ParseErrorShouldBeSetForEmptyParenthesis()
    {
        var wrapper = new FilterExpressionWrapper("Name=Test1 & ()");
        Assert.IsNotNull(wrapper.ParseError);
        Assert.Contains("Empty parenthesis", wrapper.ParseError!);
    }

    [TestMethod]
    public void ParseErrorShouldBeSetForEmptyParenthesisAfterEscapedBackslash()
    {
        // An escaped backslash "\\" leaves the following "(" unescaped, so "\\()" is still
        // a genuine empty parenthesis group and must be reported.
        var wrapper = new FilterExpressionWrapper(@"Name~foo\\&()");
        Assert.IsNotNull(wrapper.ParseError);
        Assert.Contains("Empty parenthesis", wrapper.ParseError!);
    }

    [TestMethod]
    public void ParseErrorShouldNotBeSetWhenOpenParenthesisIsEscaped()
    {
        // Regression for https://github.com/microsoft/testfx/issues/7515 — an escaped
        // open parenthesis must not be flagged as the start of an empty parenthesis group.
        var wrapper = new FilterExpressionWrapper(@"Name~aaa \(");
        Assert.IsNull(wrapper.ParseError);
    }

    [TestMethod]
    public void ParseErrorShouldNotBeSetWhenWrappedEscapedOpenParenthesisIsAtEnd()
    {
        // Regression for https://github.com/microsoft/testfx/issues/7515 — when a caller
        // wraps the filter in an outer parenthesis pair (as the MTP VSTestBridge does),
        // the trailing `\()` must not be misinterpreted as an empty group.
        var wrapper = new FilterExpressionWrapper(@"(Name~aaa \()");
        Assert.IsNull(wrapper.ParseError);
    }

    [TestMethod]
    public void EvaluateShouldMatchParametrizedTestNamePrefix()
    {
        // Regression for https://github.com/microsoft/testfx/issues/7515 — filtering
        // parametrized tests by display-name prefix using an escaped `(` should match
        // tests whose display name starts with the prefix followed by `(parameters)`.
        var wrapper = new FilterExpressionWrapper(@"Name~aaa \(");

        Assert.IsNull(wrapper.ParseError);
        Assert.IsTrue(wrapper.Evaluate(prop => prop == "Name" ? "aaa (1, 2)" : null));
        Assert.IsFalse(wrapper.Evaluate(prop => prop == "Name" ? "aaa2 (1, 2)" : null));
    }

    [TestMethod]
    public void EvaluateShouldReturnTrueWhenPropertyMatches()
    {
        var wrapper = new FilterExpressionWrapper("Name=Test1");

        bool result = wrapper.Evaluate(prop => prop == "Name" ? "Test1" : null);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void EvaluateShouldReturnFalseWhenPropertyDoesNotMatch()
    {
        var wrapper = new FilterExpressionWrapper("Name=Test1");

        bool result = wrapper.Evaluate(prop => prop == "Name" ? "Test2" : null);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void EvaluateShouldReturnFalseWhenFilterHasParseError()
    {
        var wrapper = new FilterExpressionWrapper("(Name=Test1");

        bool result = wrapper.Evaluate(prop => "Test1");

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void EvaluateShouldSupportAndOperator()
    {
        var wrapper = new FilterExpressionWrapper("Name=Test1&Category=Unit");

        bool result = wrapper.Evaluate(prop => prop switch
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

        bool matchFirst = wrapper.Evaluate(prop => prop == "Name" ? "Test1" : null);
        bool matchSecond = wrapper.Evaluate(prop => prop == "Name" ? "Test2" : null);
        bool matchNeither = wrapper.Evaluate(prop => prop == "Name" ? "Test3" : null);

        Assert.IsTrue(matchFirst);
        Assert.IsTrue(matchSecond);
        Assert.IsFalse(matchNeither);
    }

    [TestMethod]
    public void EvaluateShouldSupportNotEqualOperator()
    {
        var wrapper = new FilterExpressionWrapper("Name!=Test1");

        bool result = wrapper.Evaluate(prop => prop == "Name" ? "Test2" : null);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void EvaluateShouldSupportContainsOperator()
    {
        var wrapper = new FilterExpressionWrapper("Name~Test");

        bool result = wrapper.Evaluate(prop => prop == "Name" ? "MyTestMethod" : null);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void ValidForPropertiesShouldReturnNullWhenAllPropertiesAreSupported()
    {
        var wrapper = new FilterExpressionWrapper("Name=Test1");
        var supportedProperties = new List<string> { "Name" };

        string[]? invalidProperties = wrapper.ValidForProperties(supportedProperties);

        Assert.IsNull(invalidProperties);
    }

    [TestMethod]
    public void ValidForPropertiesShouldReturnUnsupportedPropertyNames()
    {
        var wrapper = new FilterExpressionWrapper("UnknownProperty=Value");
        var supportedProperties = new List<string> { "Name", "Category" };

        string[]? invalidProperties = wrapper.ValidForProperties(supportedProperties);

        Assert.IsNotNull(invalidProperties);
        Assert.HasCount(1, invalidProperties);
        Assert.AreEqual("UnknownProperty", invalidProperties[0]);
    }

    [TestMethod]
    public void FastFilterShouldBeCreatedForSimpleEqualityFilter()
    {
        var wrapper = new FilterExpressionWrapper("Name=Test1");

        Assert.IsNotNull(wrapper.FastFilter);
    }

    [TestMethod]
    public void FastFilterShouldBeNullForComplexFilter()
    {
        var wrapper = new FilterExpressionWrapper("Name=Test1&Name=Test2");

        Assert.IsNull(wrapper.FastFilter);
    }
}
