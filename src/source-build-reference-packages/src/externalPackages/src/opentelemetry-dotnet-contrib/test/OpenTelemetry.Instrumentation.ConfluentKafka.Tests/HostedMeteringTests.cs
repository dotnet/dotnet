// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Metrics;
using OpenTelemetry.Tests;
using Xunit;
using Xunit.Abstractions;

namespace OpenTelemetry.Instrumentation.ConfluentKafka.Tests;

[Collection("Kafka")]
public class HostedMeteringTests(ITestOutputHelper outputHelper)
{
    [Trait("CategoryName", "KafkaIntegrationTests")]
    [SkipUnlessEnvVarFoundFact(KafkaHelpers.KafkaEndPointEnvVarName)]
    public async Task ResolveInstrumentedBuildersFromHostServiceProviderTest()
    {
        List<Metric> metrics = [];
        var builder = Host.CreateDefaultBuilder();
        builder.ConfigureServices(services =>
        {
            services.AddSingleton(_ =>
                new InstrumentedProducerBuilder<string, string>(new ProducerConfig()
                {
                    BootstrapServers = KafkaHelpers.KafkaEndPoint,
                }));
            services.AddSingleton(_ =>
                new InstrumentedConsumerBuilder<string, string>(new ConsumerConfig()
                {
                    BootstrapServers = KafkaHelpers.KafkaEndPoint,
                    GroupId = Guid.NewGuid().ToString(),
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    EnablePartitionEof = true,
                }));

            services.AddOpenTelemetry().WithMetrics(metricsBuilder =>
            {
                metricsBuilder
                    .AddInMemoryExporter(metrics)
                    .AddKafkaProducerInstrumentation<string, string>()
                    .AddKafkaConsumerInstrumentation<string, string>();
            });
        });

        IGrouping<string, Metric>[] groups;
        using (var host = builder.Build())
        {
            await host.StartAsync();

            var topic = $"otel-topic-{Guid.NewGuid()}";
            using (var producer = host.Services.GetRequiredService<InstrumentedProducerBuilder<string, string>>().Build())
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

            using (var consumer = host.Services.GetRequiredService<InstrumentedConsumerBuilder<string, string>>().Build())
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
            }

            host.Services.GetRequiredService<MeterProvider>().EnsureMetricsAreFlushed();

            await host.StopAsync();
        }

        groups = metrics.GroupBy(x => x.Name).ToArray();

        Assert.Equal(4, groups.Length);
    }
}
