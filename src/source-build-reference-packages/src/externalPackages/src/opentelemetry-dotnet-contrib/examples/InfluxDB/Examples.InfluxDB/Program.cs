// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry;
using OpenTelemetry.Exporter.InfluxDB;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

Action<ResourceBuilder> configureResource = r => r.AddService(
    serviceName: "influx-exporter-test",
    serviceVersion: typeof(Program).Assembly.GetName().Version?.ToString() ?? "unknown",
    serviceInstanceId: Environment.MachineName);

using var meterProvider = Sdk.CreateMeterProviderBuilder()
    .ConfigureResource(configureResource)
    .AddRuntimeInstrumentation()
    .AddInfluxDBMetricsExporter(options =>
    {
        options.Org = "org";
        options.Bucket = "bucket";
        options.Token = "token";
        options.Endpoint = new Uri("http://localhost:8086");
        options.MetricsSchema = MetricsSchema.TelegrafPrometheusV2;
    })
    .Build();

meterProvider.ForceFlush();

await Task.Delay(TimeSpan.FromSeconds(10));
