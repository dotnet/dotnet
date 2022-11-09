// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CommandLine.Publisher;

namespace vstest.ProgrammerTests.Fakes;

internal class FakeMetricsPublisher : IMetricsPublisher
{
    public FakeMetricsPublisher(FakeErrorAggregator fakeErrorAggregator)
    {
        FakeErrorAggregator = fakeErrorAggregator;
    }

    public FakeErrorAggregator FakeErrorAggregator { get; }

    public void Dispose()
    {
        // do nothing
    }

    public void PublishMetrics(string eventName, IDictionary<string, object?> metrics)
    {
        // TODO: does nothing but probably should
    }
}
