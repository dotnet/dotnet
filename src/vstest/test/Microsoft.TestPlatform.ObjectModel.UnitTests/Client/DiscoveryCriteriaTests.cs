// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;

namespace Microsoft.TestPlatform.ObjectModel.UnitTests.Client;

[TestClass]
public class DiscoveryCriteriaTests
{
    private readonly DiscoveryCriteria _discoveryCriteria;
    private static readonly JsonSerializerSettings Settings = new()
    {
        TypeNameHandling = TypeNameHandling.None
    };

    public DiscoveryCriteriaTests()
    {
        _discoveryCriteria = new DiscoveryCriteria(
            new[] { "sampleTest.dll" },
            100,
            "<RunConfiguration></RunConfiguration>");
        _discoveryCriteria.TestCaseFilter = "TestFilter";
    }

    [TestMethod]
    public void DiscoveryCriteriaSerializesToExpectedJson()
    {
        var expectedJson = "{\"Package\":null,\"AdapterSourceMap\":{\"_none_\":[\"sampleTest.dll\"]},\"FrequencyOfDiscoveredTestsEvent\":100,\"DiscoveredTestEventTimeout\":\"10675199.02:48:05.4775807\",\"RunSettings\":\"<RunConfiguration></RunConfiguration>\",\"TestCaseFilter\":\"TestFilter\",\"TestSessionInfo\":null}";

        var json = JsonConvert.SerializeObject(_discoveryCriteria, Settings);

        Assert.AreEqual(expectedJson, json);
    }

    [TestMethod]
    public void DiscoveryCriteriaShouldBeDeserializable()
    {
        var json = "{\"Sources\":[\"sampleTest.dll\"],\"AdapterSourceMap\":{\"_none_\":[\"sampleTest.dll\"]},\"FrequencyOfDiscoveredTestsEvent\":100,\"DiscoveredTestEventTimeout\":\"10675199.02:48:05.4775807\",\"RunSettings\":\"<RunConfiguration></RunConfiguration>\",\"TestCaseFilter\":\"TestFilter\"}";

        var criteria = JsonConvert.DeserializeObject<DiscoveryCriteria>(json, Settings)!;

        Assert.AreEqual(TimeSpan.MaxValue, criteria.DiscoveredTestEventTimeout);
        Assert.AreEqual(100, criteria.FrequencyOfDiscoveredTestsEvent);
        Assert.AreEqual("<RunConfiguration></RunConfiguration>", criteria.RunSettings);
        Assert.AreEqual("TestFilter", criteria.TestCaseFilter);
        Assert.AreEqual("sampleTest.dll", criteria.AdapterSourceMap["_none_"].Single());
        CollectionAssert.AreEqual(new[] { "sampleTest.dll" }, criteria.Sources.ToArray());
    }
}
