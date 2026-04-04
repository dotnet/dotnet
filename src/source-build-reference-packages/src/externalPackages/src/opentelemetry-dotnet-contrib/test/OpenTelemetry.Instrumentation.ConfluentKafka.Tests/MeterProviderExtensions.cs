// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Metrics;

namespace OpenTelemetry.Instrumentation.ConfluentKafka.Tests;

internal static class MeterProviderExtensions
{
    public static void EnsureMetricsAreFlushed(this MeterProvider meterProvider)
    {
        bool done;
        do
        {
            done = meterProvider.ForceFlush();
        }
        while (!done);
    }
}
