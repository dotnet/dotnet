// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace OpenTelemetry.Exporter.InfluxDB.Tests.Utils;

internal static class MeterProviderBuilderTestExtensions
{
    public static MeterProviderBuilder ConfigureDefaultTestResource(this MeterProviderBuilder meterProviderBuilder)
    {
        return meterProviderBuilder.ConfigureResource(builder => builder.AddService(
            serviceName: "my-service",
            serviceNamespace: "my-service-namespace",
            serviceVersion: "1.0",
            serviceInstanceId: "my-service-id"));
    }
}
