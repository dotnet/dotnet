// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Common.UnitTests.Telemetry;

[TestClass]
public class MetricsCollectionTests
{
    private readonly IMetricsCollection _metricsCollection;

    public MetricsCollectionTests()
    {
        _metricsCollection = new MetricsCollection();
    }

    [TestMethod]
    public void AddShouldAddMetric()
    {
        _metricsCollection.Add("DummyMessage", "DummyValue");

        Assert.IsTrue(_metricsCollection.Metrics.TryGetValue("DummyMessage", out var value));
        Assert.AreEqual("DummyValue", value);
    }

    [TestMethod]
    public void AddShouldUpdateMetricIfSameKeyIsPresentAlready()
    {
        _metricsCollection.Add("DummyMessage", "DummyValue");

        Assert.IsTrue(_metricsCollection.Metrics.TryGetValue("DummyMessage", out var value));
        Assert.AreEqual("DummyValue", value);

        _metricsCollection.Add("DummyMessage", "newValue");

        Assert.IsTrue(_metricsCollection.Metrics.TryGetValue("DummyMessage", out var newValue));
        Assert.AreEqual("newValue", newValue);
    }

    [TestMethod]
    public void MetricsShouldReturnValidMetricsIfValidItemsAreThere()
    {
        _metricsCollection.Add("DummyMessage", "DummyValue");
        _metricsCollection.Add("DummyMessage2", "DummyValue");

        Assert.AreEqual(2, _metricsCollection.Metrics.Count);
        Assert.IsTrue(_metricsCollection.Metrics.ContainsKey("DummyMessage"));
        Assert.IsTrue(_metricsCollection.Metrics.ContainsKey("DummyMessage2"));
    }

    [TestMethod]
    public void MetricsShouldReturnEmptyDictionaryIfMetricsIsEmpty()
    {
        Assert.AreEqual(0, _metricsCollection.Metrics.Count);
    }
}
