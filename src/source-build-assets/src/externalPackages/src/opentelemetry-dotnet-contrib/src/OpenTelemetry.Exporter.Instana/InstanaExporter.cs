// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Globalization;
using OpenTelemetry.Exporter.Instana.Implementation;
using OpenTelemetry.Exporter.Instana.Implementation.Processors;
using OpenTelemetry.Resources;

namespace OpenTelemetry.Exporter.Instana;

internal sealed class InstanaExporter : BaseExporter<Activity>
{
    private readonly IActivityProcessor activityProcessor;
    private bool shutdownCalled;

    public InstanaExporter(IActivityProcessor? activityProcessor = null)
    {
        this.activityProcessor = activityProcessor ?? new DefaultActivityProcessor
        {
            NextProcessor = new TagsActivityProcessor
            {
                NextProcessor = new EventsActivityProcessor { NextProcessor = new ErrorActivityProcessor() },
            },
        };
    }

    internal ISpanSender SpanSender { get; set; } = new SpanSender();

    internal IInstanaExporterHelper InstanaExporterHelper { get; set; } = new InstanaExporterHelper();

    public override ExportResult Export(in Batch<Activity> batch)
    {
        if (this.shutdownCalled)
        {
            return ExportResult.Failure;
        }

        var from = new From();
        if (this.InstanaExporterHelper.IsWindows())
        {
            from = new From { E = Process.GetCurrentProcess().Id.ToString(CultureInfo.InvariantCulture) };
        }

        var serviceName = this.ExtractServiceName(ref from);

        foreach (var activity in batch)
        {
            if (activity == null)
            {
                continue;
            }

            var span = this.ParseActivityAsync(activity, serviceName, from).Result;
            this.SpanSender.Enqueue(span);
        }

        return ExportResult.Success;
    }

    protected override bool OnShutdown(int timeoutMilliseconds)
    {
        if (!this.shutdownCalled)
        {
            this.shutdownCalled = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    protected override bool OnForceFlush(int timeoutMilliseconds)
    {
        return base.OnForceFlush(timeoutMilliseconds);
    }

    private string ExtractServiceName(ref From from)
    {
        var serviceName = string.Empty;
        var serviceId = string.Empty;
        var processId = string.Empty;
        var hostId = string.Empty;
        var resource = this.InstanaExporterHelper.GetParentProviderResource(this);
        if (resource != Resource.Empty && resource.Attributes.Any())
        {
            foreach (var resourceAttribute in resource.Attributes)
            {
                if (resourceAttribute.Key.Equals("service.name", StringComparison.OrdinalIgnoreCase)
                    && resourceAttribute.Value is string servName
                    && !string.IsNullOrEmpty(servName))
                {
                    serviceName = servName;
                }

                if (from.IsEmpty())
                {
                    if (resourceAttribute.Key.Equals("service.instance.id", StringComparison.OrdinalIgnoreCase))
                    {
                        serviceId = resourceAttribute.Value.ToString();
                    }
                    else if (resourceAttribute.Key.Equals("process.pid", StringComparison.OrdinalIgnoreCase))
                    {
                        processId = resourceAttribute.Value.ToString();
                    }
                    else if (resourceAttribute.Key.Equals("host.id", StringComparison.OrdinalIgnoreCase))
                    {
                        hostId = resourceAttribute.Value.ToString();
                    }
                }
            }
        }

        if (from.IsEmpty())
        {
            if (!string.IsNullOrEmpty(processId))
            {
                from.E = processId;
            }
            else if (!string.IsNullOrEmpty(serviceId))
            {
                from.E = serviceId;
            }

            if (!string.IsNullOrEmpty(hostId))
            {
                from.H = hostId;
            }
        }

        return serviceName;
    }

    private async Task<InstanaSpan> ParseActivityAsync(Activity activity, string? serviceName = null, From? from = null)
    {
        var instanaSpan = InstanaSpanFactory.CreateSpan();

        await this.activityProcessor.ProcessAsync(activity, instanaSpan).ConfigureAwait(false);

        if (serviceName != null && !string.IsNullOrEmpty(serviceName) && instanaSpan.Data.data != null)
        {
            instanaSpan.Data.data[InstanaExporterConstants.SERVICE_FIELD] = serviceName;
        }

        if (instanaSpan.Data.data != null)
        {
            instanaSpan.Data.data[InstanaExporterConstants.OPERATION_FIELD] = activity.DisplayName;
        }

        if (activity.TraceStateString != null && !string.IsNullOrEmpty(activity.TraceStateString) && instanaSpan.Data.data != null)
        {
            instanaSpan.Data.data[InstanaExporterConstants.TRACE_STATE_FIELD] = activity.TraceStateString;
        }

        if (from != null)
        {
            instanaSpan.F = from;
        }

        return instanaSpan;
    }
}
