// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using OpenTelemetry.Exporter.Instana.Implementation;
using Xunit;

namespace OpenTelemetry.Exporter.Instana.Tests;

public class InstanaExporterTests
{
    private readonly TestInstanaExporterHelper instanaExporterHelper = new();
    private readonly TestActivityProcessor activityProcessor = new();
    private readonly TestSpanSender spanSender = new();
    private InstanaSpan? instanaSpan;
    private InstanaExporter? instanaExporter;

    [Fact]
    public void Export()
    {
        this.instanaExporterHelper.Attributes.Clear();
        this.instanaExporterHelper.Attributes.Add("service.name", "serviceName");
        this.instanaExporterHelper.Attributes.Add("service.instance.id", "serviceInstanceId");
        this.instanaExporterHelper.Attributes.Add("process.pid", "processPid");
        this.instanaExporterHelper.Attributes.Add("host.id", "hostId");

        this.spanSender.OnEnqueue = span => this.CloneSpan(span);

        this.instanaExporter = new InstanaExporter(this.activityProcessor)
        {
            InstanaExporterHelper = this.instanaExporterHelper,
            SpanSender = this.spanSender,
        };

        var activity = new Activity("testOperationName");
        activity.SetStatus(ActivityStatusCode.Error, "TestErrorDesc");
        activity.TraceStateString = "TraceStateString";

        Activity[] activities = [activity];
        var batch = new Batch<Activity>(activities, activities.Length);
        var result = this.instanaExporter.Export(in batch);

        Assert.Equal(ExportResult.Success, result);
        Assert.NotNull(this.instanaSpan);
        Assert.Equal("processPid", this.instanaSpan.F.E);
        Assert.Equal("hostId", this.instanaSpan.F.H);
        Assert.Equal("serviceName", this.instanaSpan.Data.data["service"]);
        Assert.Equal("testOperationName", this.instanaSpan.Data.data["operation"]);
        Assert.Equal("TraceStateString", this.instanaSpan.Data.data["trace_state"]);
    }

    [Fact]
    public void Export_ProcessPidDoesNotExistButServiceIdExists()
    {
        this.instanaExporterHelper.Attributes.Clear();
        this.instanaExporterHelper.Attributes.Add("service.name", "serviceName");
        this.instanaExporterHelper.Attributes.Add("service.instance.id", "serviceInstanceId");
        this.instanaExporterHelper.Attributes.Add("host.id", "hostId");

        this.spanSender.OnEnqueue = span => this.CloneSpan(span);

        this.instanaExporter = new InstanaExporter(this.activityProcessor)
        {
            InstanaExporterHelper = this.instanaExporterHelper,
            SpanSender = this.spanSender,
        };

        var activity = new Activity("testOperationName");
        activity.SetStatus(ActivityStatusCode.Error, "TestErrorDesc");

        Activity[] activities = [activity];
        var batch = new Batch<Activity>(activities, activities.Length);
        var result = this.instanaExporter.Export(in batch);

        Assert.Equal(ExportResult.Success, result);
        Assert.NotNull(this.instanaSpan);
        Assert.Equal("serviceInstanceId", this.instanaSpan.F.E);
        Assert.Equal("hostId", this.instanaSpan.F.H);
        Assert.Equal("serviceName", this.instanaSpan.Data.data["service"]);
        Assert.Equal("testOperationName", this.instanaSpan.Data.data["operation"]);
    }

    [Fact]
    public void Export_ExporterIsShotDown()
    {
        this.instanaExporter = new InstanaExporter(this.activityProcessor)
        {
            InstanaExporterHelper = this.instanaExporterHelper,
            SpanSender = this.spanSender,
        };

        var activity = new Activity("testOperationName");
        activity.SetStatus(ActivityStatusCode.Error, "TestErrorDesc");

        this.instanaExporter.Shutdown();

        Activity[] activities = [activity];
        var batch = new Batch<Activity>(activities, activities.Length);
        var result = this.instanaExporter.Export(in batch);

        Assert.Equal(ExportResult.Failure, result);
        Assert.Null(this.instanaSpan);
    }

    private bool CloneSpan(InstanaSpan span)
    {
        this.instanaSpan = span;
        return true;
    }
}
