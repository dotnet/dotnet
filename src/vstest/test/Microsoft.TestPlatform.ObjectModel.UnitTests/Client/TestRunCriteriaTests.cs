// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.ObjectModel.UnitTests;

[TestClass]
public class TestRunCriteriaTests
{
    #region Constructor tests.

    [TestMethod]
    public void ConstructorForSourcesShouldInitializeAdapterSourceMap()
    {
        var sources = new List<string> { "s1.dll", "s2.dll" };
        var testRunCriteria = new TestRunCriteria(sources, frequencyOfRunStatsChangeEvent: 10);

        Assert.IsNotNull(testRunCriteria.AdapterSourceMap);
        CollectionAssert.AreEqual(new List<string> { "_none_" }, testRunCriteria.AdapterSourceMap.Keys);
        CollectionAssert.AreEqual(sources, testRunCriteria.AdapterSourceMap.Values.First().ToList());
    }

    [TestMethod]
    public void ConstructorForSourcesWithBaseTestRunCriteriaShouldInitializeAdapterSourceMap()
    {
        var sources = new List<string> { "s1.dll", "s2.dll" };
        var testRunCriteria = new TestRunCriteria(sources, new TestRunCriteria(new List<string> { "temp.dll" }, 10));

        Assert.IsNotNull(testRunCriteria.AdapterSourceMap);
        CollectionAssert.AreEqual(new List<string> { "_none_" }, testRunCriteria.AdapterSourceMap.Keys);
        CollectionAssert.AreEqual(sources, testRunCriteria.AdapterSourceMap.Values.First().ToList());
    }

    [TestMethod]
    public void ConstructorForSourcesWithAdapterSourceMapShouldInitializeSourceMap()
    {
        var adapterSourceMap = new Dictionary<string, IEnumerable<string>>();
        var sourceSet1 = new List<string> { "s1.dll", "s2.dll" };
        var sourceSet2 = new List<string> { "s1.json", "s2.json" };
        adapterSourceMap.Add("dummyadapter1", sourceSet1);
        adapterSourceMap.Add("dummyadapter2", sourceSet2);

        var testRunCriteria = new TestRunCriteria(adapterSourceMap, 10, false, null, TimeSpan.MaxValue, null);

        Assert.IsNotNull(testRunCriteria.AdapterSourceMap);
        CollectionAssert.AreEqual(new List<string> { "dummyadapter1", "dummyadapter2" }, testRunCriteria.AdapterSourceMap.Keys);
        CollectionAssert.AreEqual(sourceSet1, testRunCriteria.AdapterSourceMap.Values.First().ToList());
        CollectionAssert.AreEqual(sourceSet2, testRunCriteria.AdapterSourceMap.Values.ToArray()[1].ToList());
    }

    #endregion

    #region Sources tests.

    [TestMethod]
    public void SourcesShouldEnumerateThroughAllSourcesInTheAdapterSourceMap()
    {
        var adapterSourceMap = new Dictionary<string, IEnumerable<string>>();
        var sourceSet1 = new List<string> { "s1.dll", "s2.dll" };
        var sourceSet2 = new List<string> { "s1.json", "s2.json" };
        adapterSourceMap.Add("dummyadapter1", sourceSet1);
        adapterSourceMap.Add("dummyadapter2", sourceSet2);

        var testRunCriteria = new TestRunCriteria(adapterSourceMap, 10, false, null, TimeSpan.MaxValue, null);

        var expectedSourceSet = new List<string>(sourceSet1);
        expectedSourceSet.AddRange(sourceSet2);
        CollectionAssert.AreEqual(expectedSourceSet, testRunCriteria.Sources!.ToList());
    }

    [TestMethod]
    public void SourcesShouldReturnNullIfAdapterSourceMapIsNull()
    {
        var testRunCriteria =
            new TestRunCriteria(
                new List<TestCase> { new TestCase("A.C.M", new Uri("excutor://dummy"), "s.dll") },
                frequencyOfRunStatsChangeEvent: 10);

        Assert.IsNull(testRunCriteria.Sources);
    }

    #endregion

    #region HasSpecificSources tests

    [TestMethod]
    public void HasSpecificSourcesReturnsFalseIfSourcesAreNotSpecified()
    {
        var testRunCriteria =
            new TestRunCriteria(
                new List<TestCase> { new TestCase("A.C.M", new Uri("excutor://dummy"), "s.dll") },
                frequencyOfRunStatsChangeEvent: 10);

        Assert.IsFalse(testRunCriteria.HasSpecificSources);
    }

    [TestMethod]
    public void HasSpecificSourcesReturnsTrueIfSourcesAreSpecified()
    {
        var sources = new List<string> { "s1.dll", "s2.dll" };
        var testRunCriteria = new TestRunCriteria(sources, frequencyOfRunStatsChangeEvent: 10);

        Assert.IsTrue(testRunCriteria.HasSpecificSources);
    }

    #endregion

    #region HasSpecificTests tests

    [TestMethod]
    public void HasSpecificTestsReturnsTrueIfTestsAreSpecified()
    {
        var testRunCriteria =
            new TestRunCriteria(
                new List<TestCase> { new TestCase("A.C.M", new Uri("excutor://dummy"), "s.dll") },
                frequencyOfRunStatsChangeEvent: 10);

        Assert.IsTrue(testRunCriteria.HasSpecificTests);
    }

    [TestMethod]
    public void HasSpecificTestsReturnsFalseIfSourcesAreSpecified()
    {
        var sources = new List<string> { "s1.dll", "s2.dll" };
        var testRunCriteria = new TestRunCriteria(sources, frequencyOfRunStatsChangeEvent: 10);

        Assert.IsFalse(testRunCriteria.HasSpecificTests);
    }

    #endregion

    #region TestCaseFilter tests

    [TestMethod]
    public void TestCaseFilterSetterShouldSetFilterCriteriaForSources()
    {
        var sources = new List<string> { "s1.dll", "s2.dll" };
        var testRunCriteria = new TestRunCriteria(sources, 10, false, string.Empty, TimeSpan.MaxValue, null, "foo", null);

        Assert.AreEqual("foo", testRunCriteria.TestCaseFilter);
    }

    #endregion
}
