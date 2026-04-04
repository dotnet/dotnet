// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using OpenTelemetry.Exporter.Instana.Implementation;
using OpenTelemetry.Exporter.Instana.Implementation.Processors;
using Xunit;

namespace OpenTelemetry.Exporter.Instana.Tests.Processors;

public class TagsActivityProcessorTests
{
    private readonly TagsActivityProcessor tagsActivityProcessor = new();

    [Fact]
    public async Task ProcessAsync_StatusTagsExist()
    {
        var activity = new Activity("testOperationName");
        activity.AddTag("otel.status_code", "testStatusCode");
        activity.AddTag("otel.status_description", "testStatusDescription");
        activity.AddTag("otel.testTag", "testTag");

        var instanaSpan = new InstanaSpan();
        await this.tagsActivityProcessor.ProcessAsync(activity, instanaSpan);

        Assert.NotNull(instanaSpan.Data);
        Assert.NotNull(instanaSpan.Data.Tags);
        Assert.Contains(instanaSpan.Data.Tags, x => x.Key == "otel.testTag" && x.Value == "testTag");
        Assert.Equal("testStatusCode", instanaSpan.TransformInfo.StatusCode);
        Assert.Equal("testStatusDescription", instanaSpan.TransformInfo.StatusDesc);
    }

    [Fact]
    public async Task ProcessAsync_StatusTagsDoNotExist()
    {
        var activity = new Activity("testOperationName");
        activity.AddTag("otel.testTag", "testTag");

        var instanaSpan = new InstanaSpan();
        await this.tagsActivityProcessor.ProcessAsync(activity, instanaSpan);

        Assert.NotNull(instanaSpan.Data);
        Assert.NotNull(instanaSpan.Data.Tags);
        Assert.Contains(instanaSpan.Data.Tags, x => x.Key == "otel.testTag" && x.Value == "testTag");
        Assert.Equal(string.Empty, instanaSpan.TransformInfo.StatusCode);
        Assert.Equal(string.Empty, instanaSpan.TransformInfo.StatusDesc);
    }
}
