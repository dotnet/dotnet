// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Metrics;
using OpenTelemetry.Tests;
using OpenTelemetry.Trace;
using Xunit;
using Xunit.Abstractions;

namespace OpenTelemetry.Instrumentation.ConfluentKafka.Tests;

[Collection("Kafka")]
public class HostedTracingAndMeteringTests(ITestOutputHelper outputHelper)
{
    [Trait("CategoryName", "KafkaIntegrationTests")]
    [SkipUnlessEnvVarFoundTheory(KafkaHelpers.KafkaEndPointEnvVarName)]
    [InlineData(true, true, true, true, true, true)]
    [InlineData(true, true, true, true, true, false)]
    [InlineData(true, true, true, true, false, true)]
    [InlineData(true, true, true, true, false, false)]
    [InlineData(true, true, true, false, true, true)]
    [InlineData(true, true, true, false, true, false)]
    [InlineData(true, true, true, false, false, true)]
    [InlineData(true, true, true, false, false, false)]
    [InlineData(true, true, false, true, true, true)]
    [InlineData(true, true, false, true, true, false)]
    [InlineData(true, true, false, true, false, true)]
    [InlineData(true, true, false, true, false, false)]
    [InlineData(true, true, false, false, true, true)]
    [InlineData(true, true, false, false, true, false)]
    [InlineData(true, true, false, false, false, true)]
    [InlineData(true, true, false, false, false, false)]
    [InlineData(true, false, true, true, true, true)]
    [InlineData(true, false, true, true, true, false)]
    [InlineData(true, false, true, true, false, true)]
    [InlineData(true, false, true, true, false, false)]
    [InlineData(true, false, true, false, true, true)]
    [InlineData(true, false, true, false, true, false)]
    [InlineData(true, false, true, false, false, true)]
    [InlineData(true, false, true, false, false, false)]
    [InlineData(true, false, false, true, true, true)]
    [InlineData(true, false, false, true, true, false)]
    [InlineData(true, false, false, true, false, true)]
    [InlineData(true, false, false, true, false, false)]
    [InlineData(true, false, false, false, true, true)]
    [InlineData(true, false, false, false, true, false)]
    [InlineData(true, false, false, false, false, true)]
    [InlineData(true, false, false, false, false, false)]
    [InlineData(false, true, true, true, true, true)]
    [InlineData(false, true, true, true, true, false)]
    [InlineData(false, true, true, true, false, true)]
    [InlineData(false, true, true, true, false, false)]
    [InlineData(false, true, true, false, true, true)]
    [InlineData(false, true, true, false, true, false)]
    [InlineData(false, true, true, false, false, true)]
    [InlineData(false, true, true, false, false, false)]
    [InlineData(false, true, false, true, true, true)]
    [InlineData(false, true, false, true, true, false)]
    [InlineData(false, true, false, true, false, true)]
    [InlineData(false, true, false, true, false, false)]
    [InlineData(false, true, false, false, true, true)]
    [InlineData(false, true, false, false, true, false)]
    [InlineData(false, true, false, false, false, true)]
    [InlineData(false, true, false, false, false, false)]
    [InlineData(false, false, true, true, true, true)]
    [InlineData(false, false, true, true, true, false)]
    [InlineData(false, false, true, true, false, true)]
    [InlineData(false, false, true, true, false, false)]
    [InlineData(false, false, true, false, true, true)]
    [InlineData(false, false, true, false, true, false)]
    [InlineData(false, false, true, false, false, true)]
    [InlineData(false, false, true, false, false, false)]
    [InlineData(false, false, false, true, true, true)]
    [InlineData(false, false, false, true, true, false)]
    [InlineData(false, false, false, true, false, true)]
    [InlineData(false, false, false, true, false, false)]
    [InlineData(false, false, false, false, true, true)]
    [InlineData(false, false, false, false, true, false)]
    [InlineData(false, false, false, false, false, true)]
    [InlineData(false, false, false, false, false, false)]
    public async Task ResolveInstrumentedBuildersFromHostServiceProviderTest(bool enableProducerMetrics, bool enableProducerTraces, bool useNamedProducerInstrumentation, bool enableConsumerMetrics, bool enableConsumerTraces, bool useNamedConsumerInstrumentation)
    {
        var producerInstrumentationName = useNamedProducerInstrumentation ? "MyProducer" : null;
        var consumerInstrumentationName = useNamedConsumerInstrumentation ? "MyConsumer" : null;
        List<Metric> metrics = [];
        List<Activity> activities = [];
        var builder = Host.CreateDefaultBuilder();
        builder.ConfigureServices(services =>
        {
            if (useNamedProducerInstrumentation)
            {
                services.AddKeyedSingleton(producerInstrumentationName, (_, _) =>
                    new InstrumentedProducerBuilder<string, string>(new ProducerConfig()
                    {
                        BootstrapServers = KafkaHelpers.KafkaEndPoint,
                    }));
            }
            else
            {
                services.AddSingleton(_ =>
                    new InstrumentedProducerBuilder<string, string>(new ProducerConfig()
                    {
                        BootstrapServers = KafkaHelpers.KafkaEndPoint,
                    }));
            }

            if (useNamedConsumerInstrumentation)
            {
                services.AddKeyedSingleton(consumerInstrumentationName, (_, _) =>
                    new InstrumentedConsumerBuilder<string, string>(new ConsumerConfig()
                    {
                        BootstrapServers = KafkaHelpers.KafkaEndPoint,
                        GroupId = Guid.NewGuid().ToString(),
                        AutoOffsetReset = AutoOffsetReset.Earliest,
                        EnablePartitionEof = true,
                    }));
            }
            else
            {
                services.AddSingleton(_ =>
                    new InstrumentedConsumerBuilder<string, string>(new ConsumerConfig()
                    {
                        BootstrapServers = KafkaHelpers.KafkaEndPoint,
                        GroupId = Guid.NewGuid().ToString(),
                        AutoOffsetReset = AutoOffsetReset.Earliest,
                        EnablePartitionEof = true,
                    }));
            }

            services.AddOpenTelemetry().WithTracing(tracingBuilder =>
            {
                tracingBuilder
                    .AddInMemoryExporter(activities)
                    .SetSampler(new TestSampler());
                if (enableProducerTraces)
                {
                    tracingBuilder.AddKafkaProducerInstrumentation<string, string>(name: producerInstrumentationName, producerBuilder: null);
                }

                if (enableConsumerTraces)
                {
                    tracingBuilder.AddKafkaConsumerInstrumentation<string, string>(name: consumerInstrumentationName, consumerBuilder: null);
                }
            }).WithMetrics(metricsBuilder =>
            {
                metricsBuilder
                    .AddInMemoryExporter(metrics);
                if (enableProducerMetrics)
                {
                    metricsBuilder.AddKafkaProducerInstrumentation<string, string>(name: producerInstrumentationName, producerBuilder: null);
                }

                if (enableConsumerMetrics)
                {
                    metricsBuilder.AddKafkaConsumerInstrumentation<string, string>(name: consumerInstrumentationName, consumerBuilder: null);
                }
            });
        });

        using (var host = builder.Build())
        {
            await host.StartAsync();

            var producerBuilder = useNamedProducerInstrumentation
                ? host.Services.GetRequiredKeyedService<InstrumentedProducerBuilder<string, string>>(producerInstrumentationName)
                : host.Services.GetRequiredService<InstrumentedProducerBuilder<string, string>>();
            Assert.Equal(enableProducerMetrics, producerBuilder.EnableMetrics);
            Assert.Equal(enableProducerTraces, producerBuilder.EnableTraces);
            var consumerBuilder = useNamedConsumerInstrumentation
                ? host.Services.GetRequiredKeyedService<InstrumentedConsumerBuilder<string, string>>(consumerInstrumentationName)
                : host.Services.GetRequiredService<InstrumentedConsumerBuilder<string, string>>();
            Assert.Equal(enableConsumerMetrics, consumerBuilder.EnableMetrics);
            Assert.Equal(enableConsumerTraces, consumerBuilder.EnableTraces);

            var topic = $"otel-topic-{Guid.NewGuid()}";
            using (var producer = (useNamedProducerInstrumentation
                       ? host.Services.GetRequiredKeyedService<InstrumentedProducerBuilder<string, string>>(producerInstrumentationName)
                       : host.Services.GetRequiredService<InstrumentedProducerBuilder<string, string>>()).Build())
            {
                for (var i = 0; i < 100; i++)
                {
                    producer.Produce(topic, new Message<string, string>()
                    {
                        Key = $"any_key_{i}",
                        Value = $"any_value_{i}",
                    });
                    outputHelper.WriteLine("produced message {0}", i);
                }

                await producer.FlushAsync();
            }

            if (enableProducerTraces)
            {
                Assert.Equal(100, activities.Count);
            }

            activities.Clear();

            using (var consumer = (useNamedConsumerInstrumentation
                       ? host.Services.GetRequiredKeyedService<InstrumentedConsumerBuilder<string, string>>(consumerInstrumentationName)
                       : host.Services.GetRequiredService<InstrumentedConsumerBuilder<string, string>>()).Build())
            {
                consumer.Subscribe(topic);

                var j = 0;
                while (true)
                {
                    var consumerResult = consumer.Consume();
                    if (consumerResult == null)
                    {
                        continue;
                    }

                    if (consumerResult.IsPartitionEOF)
                    {
                        break;
                    }

                    outputHelper.WriteLine("consumed message {0}", j);
                    j++;
                }

                Assert.Equal(100, j);
            }

            await host.StopAsync();

            if (enableConsumerTraces)
            {
                Assert.Equal(100, activities.Count);
            }

            host.Services.GetRequiredService<MeterProvider>().EnsureMetricsAreFlushed();
        }

        var groups = metrics.GroupBy(x => x.Name).ToArray();

        if (enableProducerMetrics)
        {
            Assert.Contains("messaging.publish.messages", groups.Select(x => x.Key));
            Assert.Contains("messaging.publish.duration", groups.Select(x => x.Key));
        }
        else
        {
            Assert.DoesNotContain("messaging.publish.messages", groups.Select(x => x.Key));
            Assert.DoesNotContain("messaging.publish.duration", groups.Select(x => x.Key));
        }

        if (enableConsumerMetrics)
        {
            Assert.Contains("messaging.receive.messages", groups.Select(x => x.Key));
            Assert.Contains("messaging.receive.duration", groups.Select(x => x.Key));
        }
        else
        {
            Assert.DoesNotContain("messaging.receive.messages", groups.Select(x => x.Key));
            Assert.DoesNotContain("messaging.receive.duration", groups.Select(x => x.Key));
        }
    }
}
