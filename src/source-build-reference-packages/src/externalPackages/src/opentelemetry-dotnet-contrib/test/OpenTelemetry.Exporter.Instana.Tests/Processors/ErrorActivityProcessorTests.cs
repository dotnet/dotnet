// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using OpenTelemetry.Exporter.Instana.Implementation;
using OpenTelemetry.Exporter.Instana.Implementation.Processors;
using Xunit;

namespace OpenTelemetry.Exporter.Instana.Tests.Processors;

public class ErrorActivityProcessorTests
{
    private readonly ErrorActivityProcessor errorActivityProcessor = new();

    [Fact]
    public async Task Process_ErrorStatusCodeIsSet()
    {
        var activity = new Activity("testOperationName");
        activity.SetStatus(ActivityStatusCode.Error, "TestErrorDesc");
        var instanaSpan = new InstanaSpan();
        await this.errorActivityProcessor.ProcessAsync(activity, instanaSpan);

        Assert.Equal(1, instanaSpan.Ec);
        Assert.NotNull(instanaSpan.Data);
        Assert.NotNull(instanaSpan.Data.data);
        Assert.Equal("Error", instanaSpan.Data.data[InstanaExporterConstants.ERROR_FIELD]);
        Assert.Equal("TestErrorDesc", instanaSpan.Data.data[InstanaExporterConstants.ERROR_DETAIL_FIELD]);
    }

    [Fact]
    public async Task Process_ExistsExceptionEvent()
    {
        var activity = new Activity("testOperationName");
        var instanaSpan = new InstanaSpan() { TransformInfo = new Implementation.InstanaSpanTransformInfo() { HasExceptionEvent = true } };
        await this.errorActivityProcessor.ProcessAsync(activity, instanaSpan);

        Assert.Equal(1, instanaSpan.Ec);
    }

    [Fact]
    public async Task Process_NoError()
    {
        var activity = new Activity("testOperationName");
        var instanaSpan = new InstanaSpan() { TransformInfo = new Implementation.InstanaSpanTransformInfo() };
        await this.errorActivityProcessor.ProcessAsync(activity, instanaSpan);

        Assert.Equal(0, instanaSpan.Ec);
    }
}
