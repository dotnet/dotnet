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

        Assert.HasCount(2, _metricsCollection.Metrics);
        Assert.IsTrue(_metricsCollection.Metrics.ContainsKey("DummyMessage"));
        Assert.IsTrue(_metricsCollection.Metrics.ContainsKey("DummyMessage2"));
    }

    [TestMethod]
    public void MetricsShouldReturnEmptyDictionaryIfMetricsIsEmpty()
    {
        Assert.IsEmpty(_metricsCollection.Metrics);
    }

    [TestMethod]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "MSTEST0049", Justification = "CancellationToken not meaningful for concurrency stress test")]
    public void AddShouldNotThrowWhenCalledConcurrently()
    {
        // Regression test for #15579 — concurrent Add calls on Dictionary
        // caused InvalidOperationException: "Operations that change
        // non-concurrent collections must have exclusive access."
        var tasks = new System.Threading.Tasks.Task[10];
        for (int t = 0; t < tasks.Length; t++)
        {
            var threadId = t;
            tasks[t] = System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    _metricsCollection.Add($"Thread{threadId}_Metric{i}", i);
                }
            });
        }

        foreach (var task in tasks)
        {
            task.GetAwaiter().GetResult();
        }

        Assert.IsGreaterThan(0, _metricsCollection.Metrics.Count);
    }
}
