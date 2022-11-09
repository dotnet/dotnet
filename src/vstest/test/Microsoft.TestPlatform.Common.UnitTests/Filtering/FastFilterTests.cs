// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestPlatform.Common.Filtering;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Common.UnitTests.Filtering;

[TestClass]
public class FastFilterTests
{
    [TestMethod]
    public void MultipleOperatorKindsShouldNotCreateFastFilter()
    {
        var filterExpressionWrapper = new FilterExpressionWrapper("Name=Test1&(Name=Test2|NameTest3)");
        var fastFilter = filterExpressionWrapper.FastFilter;

        Assert.IsTrue(fastFilter == null);
    }

    [TestMethod]
    public void MultipleOperationKindsShouldNotCreateFastFilter()
    {
        var filterExpressionWrapper = new FilterExpressionWrapper("Name!=TestClass1&Category=Nightly");
        var fastFilter = filterExpressionWrapper.FastFilter;

        Assert.IsTrue(fastFilter == null);
    }

    [TestMethod]
    public void ContainsOperationShouldNotCreateFastFilter()
    {
        var filterExpressionWrapper = new FilterExpressionWrapper("Name~TestClass1");
        var fastFilter = filterExpressionWrapper.FastFilter;

        Assert.IsTrue(fastFilter == null);
    }

    [TestMethod]
    public void NotContainsOperationShouldNotCreateFastFilter()
    {
        var filterExpressionWrapper = new FilterExpressionWrapper("Name!~TestClass1");
        var fastFilter = filterExpressionWrapper.FastFilter;

        Assert.IsTrue(fastFilter == null);
    }

    [TestMethod]
    public void AndOperatorAndEqualsOperationShouldNotCreateFastFilter()
    {
        var filterExpressionWrapper = new FilterExpressionWrapper("Name=Test1&Name=Test2");
        var fastFilter = filterExpressionWrapper.FastFilter;

        Assert.IsTrue(fastFilter == null);
        Assert.IsTrue(string.IsNullOrEmpty(filterExpressionWrapper.ParseError));
    }

    [TestMethod]
    public void OrOperatorAndNotEqualsOperationShouldNotCreateFastFilter()
    {
        var filterExpressionWrapper = new FilterExpressionWrapper("Name!=Test1|Name!=Test2");
        var fastFilter = filterExpressionWrapper.FastFilter;

        Assert.IsTrue(fastFilter == null);
        Assert.IsTrue(string.IsNullOrEmpty(filterExpressionWrapper.ParseError));
    }

    [TestMethod]
    public void FastFilterWithSingleEqualsClause()
    {
        var filterExpressionWrapper = new FilterExpressionWrapper("FullyQualifiedName=Test1");
        var fastFilter = filterExpressionWrapper.FastFilter;

        var expectedFilterValues = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "test1" };

        Assert.IsTrue(fastFilter != null);
        Assert.AreEqual("FullyQualifiedName", fastFilter.FilterProperties.Keys.Single());
        Assert.IsFalse(fastFilter.IsFilteredOutWhenMatched);
        Assert.IsTrue(expectedFilterValues.SetEquals(fastFilter.FilterProperties.Values.Single()));

        Assert.IsNull(filterExpressionWrapper.ValidForProperties(new List<string>() { "FullyQualifiedName" }, null));

        Assert.IsTrue(fastFilter.Evaluate(s => "Test1"));
        Assert.IsFalse(fastFilter.Evaluate(s => "Test2"));
    }

    [TestMethod]
    public void ValidForPropertiesHandlesBigFilteringExpressions()
    {
        StringBuilder testCaseFilter = new("Category=Test1");

        for (int i = 0; i < 1e5; i++)  // creating a 100k filter cases string
        {
            testCaseFilter.Append("|Test2");
        }

        var filterExpressionWrapper = new FilterExpressionWrapper(testCaseFilter.ToString());
        string[]? invalidProperties = filterExpressionWrapper.ValidForProperties(new List<string>() { "FullyQualifiedName" }, null);

        Assert.IsNotNull(invalidProperties);
        Assert.AreEqual(invalidProperties.Length, 1);
        Assert.AreEqual(invalidProperties[0], "Category");
    }

    [TestMethod]
    public void EvaluateHandlesBigFilteringExpressions()
    {
        StringBuilder testCaseFilter = new("Test1");
        // Create filter with 100k conditions.
        for (int i = 0; i < 1e5; i++)
        {
            testCaseFilter.Append("|Test2");
        }

        var filterExpressionWrapper = new FilterExpressionWrapper(testCaseFilter.ToString());
        Assert.IsTrue(filterExpressionWrapper.Evaluate(s => "Test1"));
    }

    [TestMethod]
    public void FastFilterWithMultipleEqualsClause()
    {
        var filterExpressionWrapper = new FilterExpressionWrapper("FullyQualifiedName=Test1|FullyQualifiedName=Test2|FullyQualifiedName=Test3");
        var fastFilter = filterExpressionWrapper.FastFilter;

        var expectedFilterValues = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "test1", "test2", "test3" };

        Assert.IsTrue(fastFilter != null);
        Assert.AreEqual("FullyQualifiedName", fastFilter.FilterProperties.Keys.Single());
        Assert.IsFalse(fastFilter.IsFilteredOutWhenMatched);
        Assert.IsTrue(expectedFilterValues.SetEquals(fastFilter.FilterProperties.Values.Single()));

        Assert.IsNull(filterExpressionWrapper.ValidForProperties(new List<string>() { "FullyQualifiedName" }, null));

        Assert.IsTrue(fastFilter.Evaluate(s => "Test1"));
        Assert.IsTrue(fastFilter.Evaluate(s => "test2"));
        Assert.IsTrue(fastFilter.Evaluate(s => "test3"));
        Assert.IsFalse(fastFilter.Evaluate(s => "Test4"));
    }

    [TestMethod]
    public void FastFilterWithMultipleEqualsClauseAndParentheses()
    {
        var filterExpressionWrapper = new FilterExpressionWrapper("FullyQualifiedName=Test1|(FullyQualifiedName=Test2|FullyQualifiedName=Test3)");
        var fastFilter = filterExpressionWrapper.FastFilter;

        var expectedFilterValues = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "test1", "test2", "test3" };

        Assert.IsTrue(fastFilter != null);
        Assert.AreEqual("FullyQualifiedName", fastFilter.FilterProperties.Keys.Single());
        Assert.IsFalse(fastFilter.IsFilteredOutWhenMatched);
        Assert.IsTrue(expectedFilterValues.SetEquals(fastFilter.FilterProperties.Values.Single()));

        Assert.IsNull(filterExpressionWrapper.ValidForProperties(new List<string>() { "FullyQualifiedName" }, null));

        Assert.IsTrue(fastFilter.Evaluate(s => "Test1"));
        Assert.IsTrue(fastFilter.Evaluate(s => "Test2"));
        Assert.IsTrue(fastFilter.Evaluate(s => "Test3"));
        Assert.IsFalse(fastFilter.Evaluate(s => "Test4"));
    }

    [TestMethod]
    public void FastFilterWithMultipleEqualsClauseAndRegex()
    {
        var filterExpressionWrapper = new FilterExpressionWrapper("FullyQualifiedName=Test1|FullyQualifiedName=Test2|FullyQualifiedName=Test3", new FilterOptions() { FilterRegEx = @"^[^\s\(]+" });
        var fastFilter = filterExpressionWrapper.FastFilter;

        var expectedFilterValues = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "test1", "test2", "test3" };

        Assert.IsTrue(fastFilter != null);
        Assert.AreEqual("FullyQualifiedName", fastFilter.FilterProperties.Keys.Single());
        Assert.IsFalse(fastFilter.IsFilteredOutWhenMatched);
        Assert.IsTrue(expectedFilterValues.SetEquals(fastFilter.FilterProperties.Values.Single()));

        Assert.IsNull(filterExpressionWrapper.ValidForProperties(new List<string>() { "FullyQualifiedName" }, null));

        Assert.IsTrue(fastFilter.Evaluate(s => "Test1"));
        Assert.IsTrue(fastFilter.Evaluate(s => "Test2"));
        Assert.IsTrue(fastFilter.Evaluate(s => "Test3"));
        Assert.IsTrue(fastFilter.Evaluate(s => "Test1 (123)"));
        Assert.IsTrue(fastFilter.Evaluate(s => "Test2(123)"));
        Assert.IsTrue(fastFilter.Evaluate(s => "Test3  (123)"));
        Assert.IsFalse(fastFilter.Evaluate(s => "Test4"));
        Assert.IsFalse(fastFilter.Evaluate(s => "Test4 ()"));
    }

    [TestMethod]
    public void FastFilterWithMultipleEqualsClauseForMultiplePropertyValues()
    {
        var filterExpressionWrapper = new FilterExpressionWrapper("Category=UnitTest|Category=PerfTest", null);
        var fastFilter = filterExpressionWrapper.FastFilter;

        var expectedFilterValues = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "unittest", "perftest" };

        Assert.IsTrue(fastFilter != null);
        Assert.AreEqual("Category", fastFilter.FilterProperties.Keys.Single());
        Assert.IsFalse(fastFilter.IsFilteredOutWhenMatched);
        Assert.IsTrue(expectedFilterValues.SetEquals(fastFilter.FilterProperties.Values.Single()));

        Assert.IsNull(filterExpressionWrapper.ValidForProperties(new List<string>() { "Category" }, null));

        Assert.IsTrue(fastFilter.Evaluate(s => new[] { "UnitTest" }));
        Assert.IsTrue(fastFilter.Evaluate(s => new[] { "PerfTest" }));
        Assert.IsTrue(fastFilter.Evaluate(s => new[] { "UnitTest", "PerfTest" }));
        Assert.IsTrue(fastFilter.Evaluate(s => new[] { "UnitTest", "IntegrationTest" }));
        Assert.IsFalse(fastFilter.Evaluate(s => new[] { "IntegrationTest" }));
        Assert.IsFalse(fastFilter.Evaluate(s => null));
    }

    [TestMethod]
    public void FastFilterWithMultipleEqualsClauseAndRegexReplacement()
    {
        var filterExpressionWrapper = new FilterExpressionWrapper("FullyQualifiedName=TestClass.Test1|FullyQualifiedName=TestClass.Test2|FullyQualifiedName=TestClass.Test3", new FilterOptions() { FilterRegEx = @"\s*\([^\)]*\)", FilterRegExReplacement = "" });
        var fastFilter = filterExpressionWrapper.FastFilter;

        var expectedFilterValues = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "testclass.test1", "testclass.test2", "testclass.test3" };

        Assert.IsTrue(fastFilter != null);
        Assert.AreEqual("FullyQualifiedName", fastFilter.FilterProperties.Keys.Single());
        Assert.IsFalse(fastFilter.IsFilteredOutWhenMatched);
        Assert.IsTrue(expectedFilterValues.SetEquals(fastFilter.FilterProperties.Values.Single()));

        Assert.IsNull(filterExpressionWrapper.ValidForProperties(new List<string>() { "FullyQualifiedName" }, null));

        Assert.IsTrue(fastFilter.Evaluate(s => "TestClass(1).Test1"));
        Assert.IsTrue(fastFilter.Evaluate(s => "TestClass().Test1()"));
        Assert.IsTrue(fastFilter.Evaluate(s => "TestClass(1, 2).Test2"));
        Assert.IsTrue(fastFilter.Evaluate(s => "TestClass.Test3 (abcd1234)"));
        Assert.IsTrue(fastFilter.Evaluate(s => "TestClass(1).Test1(123)"));
        Assert.IsTrue(fastFilter.Evaluate(s => "TestClass(1, 2).Test2(x:1, y:2, z:3)"));
        Assert.IsTrue(fastFilter.Evaluate(s => "TestClass(1, 2,3).Test3(1)  (123)"));
        Assert.IsFalse(fastFilter.Evaluate(s => "TestClass1.Test1"));
        Assert.IsFalse(fastFilter.Evaluate(s => "TestClass1(1).Test1"));
        Assert.IsFalse(fastFilter.Evaluate(s => "TestClass((1, 2, 3)).Test1"));
    }

    [TestMethod]
    public void FastFilterWithSingleNotEqualsClause()
    {
        var filterString = "FullyQualifiedName!=Test1";
        CheckFastFailureWithNotEqualClause(filterString);
    }

    [TestMethod]
    public void FastFilterWithNotEqualsClauseAndDifferentCase()
    {
        var filterString = "FullyQualifiedName!=Test1&FullyQualifiedName!=test1";
        CheckFastFailureWithNotEqualClause(filterString);
    }

    private static void CheckFastFailureWithNotEqualClause(string filterString)
    {
        var filterExpressionWrapper = new FilterExpressionWrapper(filterString);
        var fastFilter = filterExpressionWrapper.FastFilter;

        var expectedFilterValues = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "test1" };

        Assert.IsTrue(fastFilter != null);
        Assert.AreEqual("FullyQualifiedName", fastFilter.FilterProperties.Keys.Single());
        Assert.IsTrue(fastFilter.IsFilteredOutWhenMatched);
        Assert.IsTrue(expectedFilterValues.SetEquals(fastFilter.FilterProperties.Values.Single()));

        Assert.IsNull(filterExpressionWrapper.ValidForProperties(new List<string>() { "FullyQualifiedName" }, null));

        Assert.IsFalse(fastFilter.Evaluate(s => "Test1"));
        Assert.IsTrue(fastFilter.Evaluate(s => "Test2"));
    }

    [TestMethod]
    public void FastFilterWithMultipleNotEqualsClause()
    {
        var filterExpressionWrapper = new FilterExpressionWrapper("FullyQualifiedName!=Test1&FullyQualifiedName!=Test2&FullyQualifiedName!=Test3");
        var fastFilter = filterExpressionWrapper.FastFilter;

        var expectedFilterValues = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "test1", "test2", "test3" };

        Assert.IsTrue(fastFilter != null);
        Assert.AreEqual("FullyQualifiedName", fastFilter.FilterProperties.Keys.Single());
        Assert.IsTrue(fastFilter.IsFilteredOutWhenMatched);
        Assert.IsTrue(expectedFilterValues.SetEquals(fastFilter.FilterProperties.Values.Single()));

        Assert.IsNull(filterExpressionWrapper.ValidForProperties(new List<string>() { "FullyQualifiedName" }, null));

        Assert.IsFalse(fastFilter.Evaluate(s => "Test1"));
        Assert.IsFalse(fastFilter.Evaluate(s => "Test2"));
        Assert.IsFalse(fastFilter.Evaluate(s => "Test3"));
        Assert.IsTrue(fastFilter.Evaluate(s => "Test4"));
    }

    [TestMethod]
    public void FastFilterWithMultipleNotEqualsClauseAndRegex()
    {
        var filterExpressionWrapper = new FilterExpressionWrapper("FullyQualifiedName!=Test1&FullyQualifiedName!=Test2&FullyQualifiedName!=Test3", new FilterOptions() { FilterRegEx = @"^[^\s\(]+" });
        var fastFilter = filterExpressionWrapper.FastFilter;

        var expectedFilterValues = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "test1", "test2", "test3" };

        Assert.IsTrue(fastFilter != null);
        Assert.AreEqual("FullyQualifiedName", fastFilter.FilterProperties.Keys.Single());
        Assert.IsTrue(fastFilter.IsFilteredOutWhenMatched);
        Assert.IsTrue(expectedFilterValues.SetEquals(fastFilter.FilterProperties.Values.Single()));

        Assert.IsNull(filterExpressionWrapper.ValidForProperties(new List<string>() { "FullyQualifiedName" }, null));

        Assert.IsFalse(fastFilter.Evaluate(s => "Test1"));
        Assert.IsFalse(fastFilter.Evaluate(s => "Test2"));
        Assert.IsFalse(fastFilter.Evaluate(s => "Test3"));
        Assert.IsFalse(fastFilter.Evaluate(s => "Test1 (123)"));
        Assert.IsFalse(fastFilter.Evaluate(s => "Test2(123)"));
        Assert.IsFalse(fastFilter.Evaluate(s => "Test3  (123)"));
        Assert.IsTrue(fastFilter.Evaluate(s => "Test4"));
        Assert.IsTrue(fastFilter.Evaluate(s => "Test4 (123)"));
    }

    [TestMethod]
    public void FastFilterWithMultipleNotEqualsClauseForMultiplePropertyValues()
    {
        var filterExpressionWrapper = new FilterExpressionWrapper("Category!=UnitTest&Category!=PerfTest", null);
        var fastFilter = filterExpressionWrapper.FastFilter;

        var expectedFilterValues = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "unittest", "perftest" };

        Assert.IsTrue(fastFilter != null);
        Assert.AreEqual("Category", fastFilter.FilterProperties.Keys.Single());
        Assert.IsTrue(fastFilter.IsFilteredOutWhenMatched);
        Assert.IsTrue(expectedFilterValues.SetEquals(fastFilter.FilterProperties.Values.Single()));

        Assert.IsNull(filterExpressionWrapper.ValidForProperties(new List<string>() { "Category" }, null));

        Assert.IsFalse(fastFilter.Evaluate(s => new[] { "UnitTest" }));
        Assert.IsFalse(fastFilter.Evaluate(s => new[] { "PerfTest" }));
        Assert.IsFalse(fastFilter.Evaluate(s => new[] { "UnitTest", "PerfTest" }));
        Assert.IsFalse(fastFilter.Evaluate(s => new[] { "UnitTest", "IntegrationTest" }));
        Assert.IsTrue(fastFilter.Evaluate(s => new[] { "IntegrationTest" }));
        Assert.IsTrue(fastFilter.Evaluate(s => null));
    }

    [TestMethod]
    public void FastFilterWithWithRegexParseErrorShouldNotCreateFastFilter()
    {
        var filterExpressionWrapper = new FilterExpressionWrapper("FullyQualifiedName=Test", new FilterOptions() { FilterRegEx = @"^[^\s\(]+\1" });

        Assert.IsNull(filterExpressionWrapper.FastFilter);
        Assert.IsFalse(string.IsNullOrEmpty(filterExpressionWrapper.ParseError));
    }

    [TestMethod]
    public void FastFilterShouldThrowExceptionForUnsupportedOperatorOperationCombination()
    {
        ImmutableHashSet<string>.Builder filterHashSetBuilder = ImmutableHashSet.CreateBuilder<string>();
        try
        {
            var filter = new FastFilter(ImmutableDictionary.CreateRange(new[] { new KeyValuePair<string, ISet<string>>("dummyName", filterHashSetBuilder.ToImmutableHashSet()) }), Operation.Equal, Operator.And);
        }
        catch (Exception ex)
        {
            Assert.IsTrue(ex is ArgumentException);
            Assert.AreEqual("An error occurred while creating Fast filter.", ex.Message);
        }
    }

    [TestMethod]
    public void MultiplePropertyNamesEqualOr()
    {
        var filterExpressionWrapper = new FilterExpressionWrapper("FullyQualifiedName=Test1|Category=IntegrationTest");
        var fastFilter = filterExpressionWrapper.FastFilter;

        var expectedFilterKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Category", "FullyQualifiedName" };

        Assert.IsNotNull(fastFilter);
        Assert.IsTrue(expectedFilterKeys.SetEquals(fastFilter.FilterProperties.Keys));
        Assert.IsFalse(fastFilter.IsFilteredOutWhenMatched);
        Assert.IsTrue(fastFilter.FilterProperties["FullyQualifiedName"].SequenceEqual(new[] { "Test1" }));
        Assert.IsTrue(fastFilter.FilterProperties["Category"].SequenceEqual(new[] { "IntegrationTest" }));

        Assert.IsNull(filterExpressionWrapper.ValidForProperties(new List<string>() { "FullyQualifiedName", "Category" }, null));
        Assert.AreEqual("FullyQualifiedName",
            filterExpressionWrapper.ValidForProperties(new List<string>() { "Category" }, null)!.Single());

        Assert.IsFalse(fastFilter.Evaluate(s => s == "Category" ? new[] { "UnitTest" } : null));
        Assert.IsFalse(fastFilter.Evaluate(s => s == "Category" ? new[] { "PerfTest" } : null));
        Assert.IsFalse(fastFilter.Evaluate(s => s == "Category" ? new[] { "UnitTest", "PerfTest" } : null));
        Assert.IsTrue(fastFilter.Evaluate(s => s == "Category" ? new[] { "UnitTest", "IntegrationTest" } : null));
        Assert.IsTrue(fastFilter.Evaluate(s => s == "Category" ? new[] { "IntegrationTest" } : null));
        Assert.IsTrue(fastFilter.Evaluate(s => s switch
            {
                "Category" => new[] { "UnitTest" },
                "FullyQualifiedName" => new[] { "Test1" },
                _ => null,
            }));
        Assert.IsFalse(fastFilter.Evaluate(s => s switch
            {
                "Category" => "UnitTest",
                "FullyQualifiedName" => "Test2",
                _ => null,
            }));
        Assert.IsTrue(fastFilter.Evaluate(s => s switch
            {
                "Category" => new[] { "IntegrationTest" },
                "FullyQualifiedName" => new[] { "Test2" },
                _ => null,
            }));
        Assert.IsFalse(fastFilter.Evaluate(s => null));
    }

    [TestMethod]
    public void MultiplePropertyNamesNotEqualAnd()
    {
        var filterExpressionWrapper = new FilterExpressionWrapper("FullyQualifiedName!=Test1&Category!=IntegrationTest");
        var fastFilter = filterExpressionWrapper.FastFilter;

        var expectedFilterKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Category", "FullyQualifiedName" };

        Assert.IsNotNull(fastFilter);
        Assert.IsTrue(expectedFilterKeys.SetEquals(fastFilter.FilterProperties.Keys));
        Assert.IsTrue(fastFilter.IsFilteredOutWhenMatched);
        Assert.IsTrue(fastFilter.FilterProperties["FullyQualifiedName"].SequenceEqual(new[] { "Test1" }));
        Assert.IsTrue(fastFilter.FilterProperties["Category"].SequenceEqual(new[] { "IntegrationTest" }));

        Assert.IsNull(filterExpressionWrapper.ValidForProperties(new List<string>() { "FullyQualifiedName", "Category" }, null));
        Assert.AreEqual("FullyQualifiedName",
            filterExpressionWrapper.ValidForProperties(new List<string>() { "Category" }, null)!.Single());

        Assert.IsTrue(fastFilter.Evaluate(s => s == "Category" ? new[] { "UnitTest" } : null));
        Assert.IsFalse(fastFilter.Evaluate(s => s == "Category" ? new[] { "UnitTest", "IntegrationTest" } : null));
        Assert.IsFalse(fastFilter.Evaluate(s => s == "Category" ? new[] { "IntegrationTest" } : null));
        Assert.IsFalse(fastFilter.Evaluate(s => s switch
            {
                "Category" => new[] { "UnitTest" },
                "FullyQualifiedName" => new[] { "Test1" },
                _ => null,
            }));
        Assert.IsFalse(fastFilter.Evaluate(s => s switch
            {
                "Category" => new[] { "IntegrationTest" },
                "FullyQualifiedName" => new[] { "Test2" },
                _ => null,
            }));
        Assert.IsTrue(fastFilter.Evaluate(s => s switch
            {
                "Category" => new[] { "UnitTest" },
                "FullyQualifiedName" => new[] { "Test2" },
                _ => null,
            }));
    }
}
