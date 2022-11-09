// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.Common.Filtering;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Discovery;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPlatform.CrossPlatEngine.UnitTests.Discovery;

[TestClass]
public class DiscoveryContextTests
{
    private readonly DiscoveryContext _discoveryContext;

    public DiscoveryContextTests()
    {
        _discoveryContext = new DiscoveryContext();
    }

    /// <summary>
    /// GetTestCaseFilter should return null in case filter expression is null
    /// </summary>
    [TestMethod]
    public void GetTestCaseFilterShouldReturnNullIfFilterExpressionIsNull()
    {
        _discoveryContext.FilterExpressionWrapper = null;

        Assert.IsNull(_discoveryContext.GetTestCaseFilter(null, s => null));
    }

    /// <summary>
    /// If only property value passed, consider property key and operation defaults.
    /// </summary>
    [TestMethod]
    public void GetTestCaseFilterShouldNotThrowIfPropertyValueOnlyPassed()
    {
        _discoveryContext.FilterExpressionWrapper = new FilterExpressionWrapper("Infinity");

        var filter = _discoveryContext.GetTestCaseFilter(new List<string> { "FullyQualifiedName" }, s => null);

        Assert.IsNotNull(filter);
    }

    /// <summary>
    /// Exception should not be thrown in case invalid properties passed in filter expression.
    /// </summary>
    [TestMethod]
    public void GetTestCaseFilterShouldNotThrowOnInvalidProperties()
    {
        _discoveryContext.FilterExpressionWrapper = new FilterExpressionWrapper("highlyunlikelyproperty=unused");

        var filter = _discoveryContext.GetTestCaseFilter(new List<string> { "TestCategory" }, s => null);

        Assert.IsNotNull(filter);
    }

    /// <summary>
    /// GetTestCaseFilter should return correct filter as present in filter expression.
    /// </summary>
    [TestMethod]
    public void GetTestCaseFilterShouldReturnTestCaseFilter()
    {
        _discoveryContext.FilterExpressionWrapper = new FilterExpressionWrapper("TestCategory=Important");

        var filter = _discoveryContext.GetTestCaseFilter(new List<string> { "TestCategory" }, s => null);

        Assert.IsNotNull(filter);
        Assert.AreEqual("TestCategory=Important", filter.TestCaseFilterValue);
    }
}
