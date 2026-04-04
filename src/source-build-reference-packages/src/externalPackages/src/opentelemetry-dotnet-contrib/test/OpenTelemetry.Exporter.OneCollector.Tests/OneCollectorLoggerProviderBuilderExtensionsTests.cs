// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using Xunit;

namespace OpenTelemetry.Exporter.OneCollector.Tests;

public class OneCollectorLoggerProviderBuilderExtensionsTests
{
    [Fact]
    public void ConfigureBatchOptionsTest()
    {
        var configurationInvocations = 0;

        using var loggerFactory = CreateLoggerFactoryWithOneCollectorExporter(builder =>
        {
            builder.AddOneCollectorExporter(
                "InstrumentationKey=token-extrainformation",
                configure => configure.ConfigureBatchOptions(o => configurationInvocations++));
        });

        Assert.Equal(1, configurationInvocations);
    }

    [Fact]
    public void ConfigureExporterTest()
    {
        OneCollectorExporter<LogRecord>? exporterInstance = null;

        using var loggerFactory = CreateLoggerFactoryWithOneCollectorExporter(builder =>
        {
            builder.AddOneCollectorExporter(
                "InstrumentationKey=token-extrainformation",
                configure => configure.ConfigureExporter(exporter => exporterInstance = exporter));
        });

        Assert.NotNull(exporterInstance);

        using var payloadTransmittedRegistration = exporterInstance.RegisterPayloadTransmittedCallback(OnPayloadTransmitted);

        Assert.NotNull(payloadTransmittedRegistration);

        static void OnPayloadTransmitted(in OneCollectorExporterPayloadTransmittedCallbackArguments args)
        {
        }
    }

    [Fact]
    public void ConfigureSerializationOptionsTest()
    {
        var configurationInvocations = 0;

        using var loggerFactory = CreateLoggerFactoryWithOneCollectorExporter(builder =>
        {
            builder.AddOneCollectorExporter(
                "InstrumentationKey=token-extrainformation",
                configure => configure.ConfigureSerializationOptions(o => configurationInvocations++));
        });

        Assert.Equal(1, configurationInvocations);
    }

    [Fact]
    public void ConfigureTransportOptionsTest()
    {
        var configurationInvocations = 0;

        using var loggerFactory = CreateLoggerFactoryWithOneCollectorExporter(builder =>
        {
            builder.AddOneCollectorExporter(
                "InstrumentationKey=token-extrainformation",
                configure => configure.ConfigureTransportOptions(o => configurationInvocations++));
        });

        Assert.Equal(1, configurationInvocations);
    }

    [Fact]
    public void SetConnectionStringTest()
    {
        OneCollectorLogExporterOptions? options = null;

        using var loggerFactory = CreateLoggerFactoryWithOneCollectorExporter(
            builder =>
            {
                builder.AddOneCollectorExporter(
                    configure => configure.SetConnectionString("InstrumentationKey=token-extrainformation"));
            },
            services => services.Configure<OneCollectorLogExporterOptions>(o => options = o));

        Assert.NotNull(options);
        Assert.Equal("InstrumentationKey=token-extrainformation", options.ConnectionString);
    }

    [Fact]
    public void SetEventFullNameMappingsTest()
    {
        OneCollectorLogExporterOptions? options = null;

        var mappings = new Dictionary<string, string>()
        {
            { "Key1", "Value1" },
        };

        using var loggerFactory = CreateLoggerFactoryWithOneCollectorExporter(
            builder =>
            {
                builder.AddOneCollectorExporter(
                    "InstrumentationKey=token-extrainformation",
                    configure => configure.SetEventFullNameMappings(mappings));
            },
            services => services.Configure<OneCollectorLogExporterOptions>(o => options = o));

        Assert.NotNull(options);
        Assert.Equal(mappings, options.EventFullNameMappings);
    }

    [Fact]
    public void SetDefaultEventNamespaceTest()
    {
        OneCollectorLogExporterOptions? options = null;

        using var loggerFactory = CreateLoggerFactoryWithOneCollectorExporter(
            builder =>
            {
                builder.AddOneCollectorExporter(
                    "InstrumentationKey=token-extrainformation",
                    configure => configure.SetDefaultEventNamespace("MyDefaultEventNamespace"));
            },
            services => services.Configure<OneCollectorLogExporterOptions>(o => options = o));

        Assert.NotNull(options);
        Assert.Equal("MyDefaultEventNamespace", options.DefaultEventNamespace);
    }

    [Fact]
    public void SetDefaultEventNameTest()
    {
        OneCollectorLogExporterOptions? options = null;

        using var loggerFactory = CreateLoggerFactoryWithOneCollectorExporter(
            builder =>
            {
                builder.AddOneCollectorExporter(
                    "InstrumentationKey=token-extrainformation",
                    configure => configure.SetDefaultEventName("MyDefaultEventName"));
            },
            services => services.Configure<OneCollectorLogExporterOptions>(o => options = o));

        Assert.NotNull(options);
        Assert.Equal("MyDefaultEventName", options.DefaultEventName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("CustomName")]
    public void ConfigurationBindingTest(string? name)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>()
            {
                ["ConnectionString"] = "InstrumentationKey=token-extrainformation",
                ["SerializationOptions:ExceptionStackTraceHandling"] = "IncludeAsString",
                ["TransportOptions:Endpoint"] = "http://myendpoint.com/",
                ["BatchOptions:ScheduledDelayMilliseconds"] = "18",
            })
            .Build();

        OneCollectorLogExporterOptions? exporterOptions = null;
        BatchExportLogRecordProcessorOptions? batchOptions = null;

        using var loggerFactory = CreateLoggerFactoryWithOneCollectorExporter(
            builder => builder.AddOneCollectorExporter(name, connectionString: null, configuration: configuration, configure: null),
            services =>
            {
                services.Configure<OneCollectorLogExporterOptions>(name, o => exporterOptions = o);
                services.Configure<BatchExportLogRecordProcessorOptions>(name, o => batchOptions = o);
            });

        Assert.NotNull(exporterOptions);
        Assert.NotNull(batchOptions);

        Assert.Equal("InstrumentationKey=token-extrainformation", exporterOptions.ConnectionString);
        Assert.Equal(OneCollectorExporterSerializationExceptionStackTraceHandlingType.IncludeAsString, exporterOptions.SerializationOptions.ExceptionStackTraceHandling);
        Assert.Equal("http://myendpoint.com/", exporterOptions.TransportOptions.Endpoint.ToString());
        Assert.Equal(18, batchOptions.ScheduledDelayMilliseconds);
    }

    [Fact]
    public void InstrumentationKeyAndTenantTokenValidationTest()
    {
        {
            using var loggerFactory = CreateLoggerFactoryWithOneCollectorExporter(builder =>
            {
                builder.AddOneCollectorExporter("InstrumentationKey=token-extrainformation");
            });
        }

        {
            using var loggerFactory = CreateLoggerFactoryWithOneCollectorExporter(builder =>
            {
                builder.AddOneCollectorExporter(configure => configure.SetConnectionString("InstrumentationKey=token-extrainformation"));
            });
        }

        Assert.Throws<OneCollectorExporterValidationException>(() =>
        {
            using var loggerFactory = CreateLoggerFactoryWithOneCollectorExporter(builder =>
            {
                builder.AddOneCollectorExporter(configure => { });
            });
        });

        Assert.Throws<OneCollectorExporterValidationException>(() =>
        {
            using var loggerFactory = CreateLoggerFactoryWithOneCollectorExporter(builder =>
            {
                builder.AddOneCollectorExporter("InstrumentationKey=invalidinstrumentationkey");
            });
        });

        Assert.Throws<OneCollectorExporterValidationException>(() =>
        {
            using var loggerFactory = CreateLoggerFactoryWithOneCollectorExporter(builder =>
            {
                builder.AddOneCollectorExporter("UnknownKey=invalidinstrumentationkey");
            });
        });
    }

    [Fact]
    public void OptionsTest()
    {
        var configurationInvocations = 0;

        using var loggerFactory = CreateLoggerFactoryWithOneCollectorExporter(
            builder =>
            {
                builder.AddOneCollectorExporter();
            },
            services =>
            {
                services.Configure<OneCollectorLogExporterOptions>(
                    o =>
                    {
                        o.ConnectionString = "InstrumentationKey=token-extrainformation";
                        configurationInvocations++;
                    });

                services.Configure<BatchExportLogRecordProcessorOptions>(
                    o => configurationInvocations++);
            });

        Assert.Equal(2, configurationInvocations);
    }

    [Fact]
    public void NamedOptionsTest()
    {
        var configurationInvocations = 0;

        using var loggerFactory = CreateLoggerFactoryWithOneCollectorExporter(
            builder =>
            {
                builder.AddOneCollectorExporter(
                    name: "MyOneCollectorExporter",
                    connectionString: null,
                    configuration: null,
                    configure: null);
            },
            services =>
            {
                services.Configure<OneCollectorLogExporterOptions>(
                    "MyOneCollectorExporter",
                    o =>
                    {
                        o.ConnectionString = "InstrumentationKey=token-extrainformation";
                        configurationInvocations++;
                    });

                services.Configure<BatchExportLogRecordProcessorOptions>(
                    "MyOneCollectorExporter",
                    o => configurationInvocations++);
            });

        Assert.Equal(2, configurationInvocations);
    }

    private static ILoggerFactory CreateLoggerFactoryWithOneCollectorExporter(
        Action<LoggerProviderBuilder> configureLogging,
        Action<IServiceCollection>? configureServices = null)
    {
        return LoggerFactory.Create(builder =>
        {
            builder.AddOpenTelemetry();

            builder.Services.ConfigureOpenTelemetryLoggerProvider(configureLogging);

            configureServices?.Invoke(builder.Services);
        });
    }
}
