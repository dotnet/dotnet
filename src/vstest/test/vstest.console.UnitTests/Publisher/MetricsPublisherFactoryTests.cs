// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CommandLine.Publisher;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace vstest.console.UnitTests.Publisher;

[TestClass]
public class MetricsPublisherFactoryTests
{
    [TestMethod]
    public void GetMetricsPublisherShouldReturnNoOpMetricsPublisherIfTelemetryOptedOutAndNotInDesignMode()
    {
        var result = MetricsPublisherFactory.GetMetricsPublisher(false, false);

        Assert.IsTrue(result.Result is NoOpMetricsPublisher);
    }

    [TestMethod]
    public void GetMetricsPublisherShouldReturnNoOpMetricsPublisherIfTelemetryOptedInAndInDesignMode()
    {
        var result = MetricsPublisherFactory.GetMetricsPublisher(true, true);

        Assert.IsTrue(result.Result is NoOpMetricsPublisher);
    }

    [TestMethod]
    public void GetMetricsPublisherShouldReturnNoOpMetricsPublisherIfTelemetryOptedOutAndInDesignMode()
    {
        var result = MetricsPublisherFactory.GetMetricsPublisher(false, true);

        Assert.IsTrue(result.Result is NoOpMetricsPublisher);
    }
}
