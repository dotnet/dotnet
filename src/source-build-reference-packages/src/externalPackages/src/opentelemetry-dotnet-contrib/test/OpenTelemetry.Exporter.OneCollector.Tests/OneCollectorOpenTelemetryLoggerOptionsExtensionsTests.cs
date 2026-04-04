// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using Xunit;

namespace OpenTelemetry.Exporter.OneCollector.Tests;

public class OneCollectorOpenTelemetryLoggerOptionsExtensionsTests
{
    [Fact]
    public void ConfigureExporterTest()
    {
        OneCollectorExporter<LogRecord>? exporterInstance = null;

        using var loggerFactory = LoggerFactory.Create(builder => builder
            .AddOpenTelemetry(builder =>
            {
                builder.AddOneCollectorExporter(
                    "InstrumentationKey=token-extrainformation",
                    configure => configure.ConfigureExporter(exporter => exporterInstance = exporter));
            }));

        Assert.NotNull(exporterInstance);

        using var payloadTransmittedRegistration = exporterInstance.RegisterPayloadTransmittedCallback(OnPayloadTransmitted);

        Assert.NotNull(payloadTransmittedRegistration);

        static void OnPayloadTransmitted(in OneCollectorExporterPayloadTransmittedCallbackArguments args)
        {
        }
    }

    [Fact]
    public void InstrumentationKeyAndTenantTokenValidationTest()
    {
        {
            using var loggerFactory = LoggerFactory.Create(builder => builder
                .AddOpenTelemetry(builder =>
                {
                    builder.AddOneCollectorExporter("InstrumentationKey=token-extrainformation");
                }));
        }

        {
            using var loggerFactory = LoggerFactory.Create(builder => builder
                .AddOpenTelemetry(builder =>
                {
                    builder.AddOneCollectorExporter(configure => configure.SetConnectionString("InstrumentationKey=token-extrainformation"));
                }));
        }

        Assert.Throws<OneCollectorExporterValidationException>(() =>
        {
            using var loggerFactory = LoggerFactory.Create(builder => builder
                .AddOpenTelemetry(builder =>
                {
                    builder.AddOneCollectorExporter(configure => { });
                }));
        });

        Assert.Throws<OneCollectorExporterValidationException>(() =>
        {
            using var loggerFactory = LoggerFactory.Create(builder => builder
                .AddOpenTelemetry(builder =>
                {
                    builder.AddOneCollectorExporter("InstrumentationKey=invalidinstrumentationkey");
                }));
        });

        Assert.Throws<OneCollectorExporterValidationException>(() =>
        {
            using var loggerFactory = LoggerFactory.Create(builder => builder
                .AddOpenTelemetry(builder =>
                {
                    builder.AddOneCollectorExporter("UnknownKey=invalidinstrumentationkey");
                }));
        });
    }
}
