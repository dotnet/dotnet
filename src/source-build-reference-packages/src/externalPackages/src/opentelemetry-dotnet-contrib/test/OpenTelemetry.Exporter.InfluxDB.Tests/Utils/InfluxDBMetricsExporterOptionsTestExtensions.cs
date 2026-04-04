// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Exporter.InfluxDB.Tests.Utils;

internal static class InfluxDBMetricsExporterOptionsTestExtensions
{
    public static void WithDefaultTestConfiguration(this InfluxDBMetricsExporterOptions options)
    {
        options.Bucket = "MyBucket";
        options.Org = "MyOrg";
        options.Token = "MyToken";

        // For tests, we want to flush the metrics ASAP
        options.FlushInterval = 1;
    }
}
