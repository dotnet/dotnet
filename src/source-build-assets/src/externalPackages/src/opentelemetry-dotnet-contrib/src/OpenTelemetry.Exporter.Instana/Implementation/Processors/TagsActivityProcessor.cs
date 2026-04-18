// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;

namespace OpenTelemetry.Exporter.Instana.Implementation.Processors;

internal class TagsActivityProcessor : ActivityProcessorBase, IActivityProcessor
{
    public override async Task ProcessAsync(Activity activity, InstanaSpan instanaSpan)
    {
        if (instanaSpan == null)
        {
            return;
        }

        if (activity == null)
        {
            return;
        }

        this.PreProcess(activity, instanaSpan);

        var statusCode = string.Empty;
        var statusDesc = string.Empty;
        var tags = new Dictionary<string, string>();
        foreach (var tag in activity.Tags)
        {
            if (tag.Key == "otel.status_code")
            {
                statusCode = tag.Value ?? string.Empty;
                continue;
            }

            if (tag.Key == "otel.status_description")
            {
                statusDesc = tag.Value ?? string.Empty;
                continue;
            }

            if (tag.Value != null)
            {
                tags[tag.Key] = tag.Value.ToString();
            }
        }

        if (instanaSpan.Data != null)
        {
            instanaSpan.Data.Tags = tags;
        }

        instanaSpan.TransformInfo.StatusCode = statusCode;
        instanaSpan.TransformInfo.StatusDesc = statusDesc;

        await base.ProcessAsync(activity, instanaSpan).ConfigureAwait(false);
    }
}
