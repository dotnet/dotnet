// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPlatform.CrossPlatEngine.UnitTests.Adapter;

[TestClass]
public class TestSourcesUtilityTests
{
    private static readonly string Temp = Path.GetTempPath();

    [TestMethod]
    public void GetSourcesShouldAggregateSourcesIfMultiplePresentInAdapterSourceMap()
    {
        var adapterSourceMap = new Dictionary<string, IEnumerable<string>?>
        {
            { "adapter1", new List<string>() { "source1.dll", "source2.dll" } },
            { "adapter2", new List<string>() { "source1.dll", "source3.dll" } },
            { "adapter3", new List<string>() { "source1.dll" } }
        };

        var sources = TestSourcesUtility.GetSources(adapterSourceMap)!;
        Assert.AreEqual(5, sources.Count());
        Assert.IsTrue(sources.Contains("source1.dll"));
        Assert.IsTrue(sources.Contains("source2.dll"));
        Assert.IsTrue(sources.Contains("source3.dll"));
    }

    [TestMethod]
    public void GetSourcesShouldGetDistinctSourcesFromTestCases()
    {
        var path = Path.Combine(Temp, "d");
        var tests = new List<TestCase>() { new TestCase("test1", new Uri(path), "source1.dll"),
            new TestCase("test2", new Uri(path), "source2.dll"),
            new TestCase("test3", new Uri(path), "source1.dll")};

        var sources = TestSourcesUtility.GetSources(tests);
        Assert.AreEqual(2, sources.Count());
        Assert.IsTrue(sources.Contains("source1.dll"));
        Assert.IsTrue(sources.Contains("source2.dll"));
    }

    [TestMethod]
    public void GetDefaultCodeBasePathShouldReturnNullIfAdapterSourceMapIsEmpty()
    {
        var adapterSourceMap = new Dictionary<string, IEnumerable<string>?>();

        var defaultCodeBase = TestSourcesUtility.GetDefaultCodebasePath(adapterSourceMap);
        Assert.IsNull(defaultCodeBase);
    }

    [TestMethod]
    public void GetDefaultCodeBasePathShouldReturnNullIfTestCaseListIsEmpty()
    {
        var tests = new List<TestCase>();

        var defaultCodeBase = TestSourcesUtility.GetDefaultCodebasePath(tests);
        Assert.IsNull(defaultCodeBase);
    }

    [TestMethod]
    public void GetDefaultCodeBasePathShouldReturnDefaultDirectoryPathForAdapterSourceMap()
    {
        var adapterSourceMap = new Dictionary<string, IEnumerable<string>?>
        {
            { "adapter1", new List<string>() { Path.Combine(Temp, "folder1", "source1.dll"), Path.Combine(Temp, "folder2", "source2.dll") } }
        };

        var defaultCodeBase = TestSourcesUtility.GetDefaultCodebasePath(adapterSourceMap);
        Assert.AreEqual(Path.Combine(Temp, "folder1"), defaultCodeBase);
    }

    [TestMethod]
    public void GetDefaultCodeBasePathShouldReturnDefaultDirectoryPathForTestCaseList()
    {
        var tests = new List<TestCase>() { new TestCase("test1", new Uri(Path.Combine(Temp, "d")), Path.Combine(Temp, "folder1", "source1.dll")) };

        var defaultCodeBase = TestSourcesUtility.GetDefaultCodebasePath(tests);
        Assert.AreEqual(Path.Combine(Temp, "folder1"), defaultCodeBase);
    }
}
