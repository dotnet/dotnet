// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Exporter.Instana.Implementation;

internal class InstanaSpanFactory
{
    internal static InstanaSpan CreateSpan()
    {
        var instanaSpan = new InstanaSpan
        {
            Data = new Data()
            {
                data = [],
                Tags = [],
                Events = new List<SpanEvent>(8),
            },

            TransformInfo = new InstanaSpanTransformInfo(),
        };

        return instanaSpan;
    }
}
