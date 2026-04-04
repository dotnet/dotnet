// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using OpenTelemetry.Exporter.Instana.Implementation;
using OpenTelemetry.Exporter.Instana.Implementation.Processors;
using Xunit;

namespace OpenTelemetry.Exporter.Instana.Tests.Processors;

public class DefaultActivityProcessorTests
{
    private readonly DefaultActivityProcessor defaultActivityProcessor = new();

    [Fact]
    public async Task ProcessAsync()
    {
        var activity = new Activity("testOperationName");
        activity.Start();
        await Task.Delay(200);
        activity.Stop();
        var instanaSpan = new InstanaSpan();
        await this.defaultActivityProcessor.ProcessAsync(activity, instanaSpan);

        Assert.False(string.IsNullOrEmpty(instanaSpan.S));
        Assert.False(string.IsNullOrEmpty(instanaSpan.Lt));
        Assert.True(instanaSpan.D > 0);
        Assert.True(instanaSpan.Ts > 0);
        Assert.NotNull(instanaSpan.Data);
        Assert.NotNull(instanaSpan.Data.data);
        Assert.Contains(instanaSpan.Data.data, filter: x =>
        {
            return x.Key == "kind"
                   && x.Value.Equals("internal");
        });
    }
}
