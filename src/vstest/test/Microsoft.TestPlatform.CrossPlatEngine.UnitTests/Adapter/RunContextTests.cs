// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.Common.Filtering;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Adapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPlatform.CrossPlatEngine.UnitTests.Adapter;

[TestClass]
public class RunContextTests
{
    private readonly RunContext _runContext;

    public RunContextTests()
    {
        _runContext = new RunContext();
    }

    [TestMethod]
    public void GetTestCaseFilterShouldReturnNullIfFilterExpressionIsNull()
    {
        _runContext.FilterExpressionWrapper = null;

        Assert.IsNull(_runContext.GetTestCaseFilter(null, s => null));
    }

    /// <summary>
    /// If only property value passed, consider property key and operation defaults.
    /// </summary>
    [TestMethod]
    public void GetTestCaseFilterShouldNotThrowIfPropertyValueOnlyPassed()
    {
        _runContext.FilterExpressionWrapper = new FilterExpressionWrapper("Infinity");

        var filter = _runContext.GetTestCaseFilter(new List<string> { "FullyQualifiedName" }, s => null);

        Assert.IsNotNull(filter);
    }

    [TestMethod]
    public void GetTestCaseFilterShouldNotThrowOnInvalidProperties()
    {
        _runContext.FilterExpressionWrapper = new FilterExpressionWrapper("highlyunlikelyproperty=unused");

        var filter = _runContext.GetTestCaseFilter(new List<string> { "TestCategory" }, s => null);

        Assert.IsNotNull(filter);
    }

    [TestMethod]
    public void GetTestCaseFilterShouldReturnTestCaseFilter()
    {
        _runContext.FilterExpressionWrapper = new FilterExpressionWrapper("TestCategory=Important");
        var filter = _runContext.GetTestCaseFilter(new List<string> { "TestCategory" }, s => null);

        Assert.IsNotNull(filter);
        Assert.AreEqual("TestCategory=Important", filter.TestCaseFilterValue);
    }
}
