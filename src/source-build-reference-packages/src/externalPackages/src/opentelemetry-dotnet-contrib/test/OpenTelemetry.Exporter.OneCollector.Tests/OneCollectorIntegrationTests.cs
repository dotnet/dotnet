// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Internal;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Tests;
using Xunit;
using Xunit.Abstractions;

namespace OpenTelemetry.Exporter.OneCollector.Tests;

[Trait("CategoryName", "OneCollectorIntegrationTests")]
public class OneCollectorIntegrationTests
{
    private const string OneCollectorInstrumentationKeyEnvName = "OTEL_ONECOLLECTOR_INSTRUMENTATION_KEY";
    private readonly ITestOutputHelper testOutputHelper;

    public OneCollectorIntegrationTests(ITestOutputHelper output)
    {
        this.testOutputHelper = output;
    }

    [SkipUnlessEnvVarFoundFact(OneCollectorInstrumentationKeyEnvName)]
    public void LogWithEventIdAndNameIntegrationTest()
    {
        this.RunIntegrationTest(
            logger =>
            {
                logger.LogInformation(
                    new EventId(18, "MyEvent"),
                    "Hello world");
            },
            out var succeeded,
            out var actualJson);

        Assert.True(succeeded);
        Assert.NotNull(actualJson);

        AssertActualJson(
            actualJson,
            root =>
            {
                Assert.Equal($"{nameof(OneCollectorIntegrationTests)}.MyEvent", root.GetProperty("name").GetString());
            },
            data =>
            {
                Assert.Equal("Information", data.GetProperty("severityText").GetString());
                Assert.Equal(9, data.GetProperty("severityNumber").GetInt32());
                Assert.Equal("Hello world", data.GetProperty("body").GetString());
                Assert.Equal(18, data.GetProperty("eventId").GetInt32());
            });
    }

    [SkipUnlessEnvVarFoundFact(OneCollectorInstrumentationKeyEnvName)]
    public void LogWithEventNameOnlyIntegrationTest()
    {
        this.RunIntegrationTest(
            logger =>
            {
                logger.LogInformation(
                    new EventId(0, "MyEvent"),
                    "Hello world");
            },
            out var succeeded,
            out var actualJson);

        Assert.True(succeeded);
        Assert.NotNull(actualJson);

        AssertActualJson(
            actualJson,
            root =>
            {
                Assert.Equal($"{nameof(OneCollectorIntegrationTests)}.MyEvent", root.GetProperty("name").GetString());
            },
            data =>
            {
                Assert.Equal("Information", data.GetProperty("severityText").GetString());
                Assert.Equal(9, data.GetProperty("severityNumber").GetInt32());
                Assert.Equal("Hello world", data.GetProperty("body").GetString());
                AssertPropertyDoesNotExist(data, "eventId");
            });
    }

    [SkipUnlessEnvVarFoundFact(OneCollectorInstrumentationKeyEnvName)]
    public void LogWithDataIntegrationTest()
    {
        this.RunIntegrationTest(
            logger =>
            {
                logger.LogInformation("Hello world {StructuredData}", "Goodbye world");
            },
            out var succeeded,
            out var actualJson);

        Assert.True(succeeded);
        Assert.NotNull(actualJson);

        AssertActualJson(
            actualJson,
            root =>
            {
                Assert.Equal($"{nameof(OneCollectorIntegrationTests)}.Log", root.GetProperty("name").GetString());
            },
            data =>
            {
                Assert.Equal("Information", data.GetProperty("severityText").GetString());
                Assert.Equal(9, data.GetProperty("severityNumber").GetInt32());
                Assert.Equal("Hello world {StructuredData}", data.GetProperty("body").GetString());
                Assert.Equal("Goodbye world", data.GetProperty("StructuredData").GetString());
            });
    }

    [SkipUnlessEnvVarFoundTheory(OneCollectorInstrumentationKeyEnvName)]
    [InlineData(false)]
    [InlineData(true)]
    public void LogWithExceptionIntegrationTest(bool includeStackTrace)
    {
        var ex = new Exception("Test exception");

        this.RunIntegrationTest(
            logger =>
            {
                logger.LogInformation(ex, "Hello world");
            },
            out var succeeded,
            out var actualJson,
            configureBuilderAction: builder => builder
                .ConfigureSerializationOptions(options => options.ExceptionStackTraceHandling = includeStackTrace
                    ? OneCollectorExporterSerializationExceptionStackTraceHandlingType.IncludeAsString
                    : OneCollectorExporterSerializationExceptionStackTraceHandlingType.Ignore));

        // TODO: Switch this to true. OneCollector doesn't currently support
        // ext.ex (Exception Extension) but it should soon.
        Assert.False(succeeded);
        Assert.NotNull(actualJson);

        AssertActualJson(
            actualJson,
            root =>
            {
                Assert.Equal($"{nameof(OneCollectorIntegrationTests)}.Log", root.GetProperty("name").GetString());
            },
            data =>
            {
                Assert.Equal("Information", data.GetProperty("severityText").GetString());
                Assert.Equal(9, data.GetProperty("severityNumber").GetInt32());
                Assert.Equal("Hello world", data.GetProperty("body").GetString());
            },
            extensions =>
            {
                var exceptionExtension = extensions.GetProperty("ex");

                var type = exceptionExtension.GetProperty("type").GetString();
                var msg = exceptionExtension.GetProperty("msg").GetString();

                Assert.Equal(ex.GetType().FullName, type);
                Assert.Equal(ex.Message, msg);

                if (!includeStackTrace)
                {
                    AssertPropertyDoesNotExist(exceptionExtension, "stack");
                }
                else
                {
                    var stack = exceptionExtension.GetProperty("stack").GetString();

                    Assert.Equal(ex.ToInvariantString(), stack);
                }
            });
    }

    [SkipUnlessEnvVarFoundFact(OneCollectorInstrumentationKeyEnvName)]
    public void LogWithActivityIntegrationTest()
    {
        using var activity = new Activity("TestOperation");
        activity.Start();

        this.RunIntegrationTest(
            logger =>
            {
                logger.LogInformation("Hello world");
            },
            out var succeeded,
            out var actualJson);

        Assert.True(succeeded);
        Assert.NotNull(actualJson);

        AssertActualJson(
            actualJson,
            root =>
            {
                Assert.Equal($"{nameof(OneCollectorIntegrationTests)}.Log", root.GetProperty("name").GetString());
            },
            data =>
            {
                Assert.Equal("Information", data.GetProperty("severityText").GetString());
                Assert.Equal(9, data.GetProperty("severityNumber").GetInt32());
                Assert.Equal("Hello world", data.GetProperty("body").GetString());
            },
            extensions =>
            {
                var distributedTraceExtension = extensions.GetProperty("dt");

                var traceId = distributedTraceExtension.GetProperty("traceId").GetString();
                var spanId = distributedTraceExtension.GetProperty("spanId").GetString();

                Assert.Equal(activity.TraceId.ToHexString(), traceId);
                Assert.Equal(activity.SpanId.ToHexString(), spanId);
            });
    }

    private static void AssertActualJson(
        string actualJson,
        Action<JsonElement> assertRootElement,
        Action<JsonElement> assertDataElement,
        Action<JsonElement>? assertExtensionElement = null)
    {
        var document = JsonDocument.Parse(actualJson);

        var rootElement = document.RootElement;

        Assert.Equal("4.0", rootElement.GetProperty("ver").GetString());
        Assert.True(!string.IsNullOrWhiteSpace(rootElement.GetProperty("time").GetString()));
        Assert.True(!string.IsNullOrWhiteSpace(rootElement.GetProperty("iKey").GetString()));

        assertRootElement(rootElement);

        var data = rootElement.GetProperty("data");

        assertDataElement(data);

        if (assertExtensionElement != null)
        {
            var extensions = rootElement.GetProperty("ext");

            assertExtensionElement(extensions);
        }
        else
        {
            AssertPropertyDoesNotExist(rootElement, "ext");
        }
    }

    private static void AssertPropertyDoesNotExist(JsonElement element, string propertyName)
    {
        if (element.TryGetProperty(propertyName, out _))
        {
            Assert.Fail($"Property '{propertyName}' was found in JSON");
        }
    }

    private void RunIntegrationTest(
        Action<ILogger> testAction,
        out bool succeeded,
        out string? actualJson,
        Action<OpenTelemetryLoggerOptions>? configureOptionsAction = null,
        Action<OneCollectorLogExportProcessorBuilder>? configureBuilderAction = null)
    {
        var innerSucceeded = false;
        string? innerActualJson = null;

        using (var loggerFactory = LoggerFactory.Create(logging => logging
            .AddOpenTelemetry(options =>
            {
                options
                    .SetResourceBuilder(ResourceBuilder.CreateEmpty())
                    .AddOneCollectorExporter(builder =>
                    {
                        builder.SetConnectionString($"InstrumentationKey={Environment.GetEnvironmentVariable(OneCollectorInstrumentationKeyEnvName)}");

                        builder.ConfigureExporter(e => e.RegisterPayloadTransmittedCallback(OneCollectorExporterPayloadTransmittedCallbackAction, includeFailures: true));

                        configureBuilderAction?.Invoke(builder);
                    });

                configureOptionsAction?.Invoke(options);
            })))
        {
            testAction(loggerFactory.CreateLogger(nameof(OneCollectorIntegrationTests)));
        }

        succeeded = innerSucceeded;
        actualJson = innerActualJson;

        this.testOutputHelper.WriteLine($"ActualJson: {actualJson}");

        void OneCollectorExporterPayloadTransmittedCallbackAction(
            in OneCollectorExporterPayloadTransmittedCallbackArguments args)
        {
            innerSucceeded = args.Succeeded;
            using var memoryStream = new MemoryStream();
            args.CopyPayloadToStream(memoryStream);
            innerActualJson = Encoding.UTF8.GetString(memoryStream.ToArray());
        }
    }
}
