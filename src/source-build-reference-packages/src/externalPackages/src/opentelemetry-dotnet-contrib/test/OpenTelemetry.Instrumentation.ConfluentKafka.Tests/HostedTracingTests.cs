// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Tests;
using OpenTelemetry.Trace;
using Xunit;
using Xunit.Abstractions;

namespace OpenTelemetry.Instrumentation.ConfluentKafka.Tests;

[Collection("Kafka")]
public class HostedTracingTests(ITestOutputHelper outputHelper)
{
    [Trait("CategoryName", "KafkaIntegrationTests")]
    [SkipUnlessEnvVarFoundFact(KafkaHelpers.KafkaEndPointEnvVarName)]
    public async Task ResolveInstrumentedBuildersFromHostServiceProviderTest()
    {
        List<Activity> activities = [];
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

            services.AddOpenTelemetry().WithTracing(tracingBuilder =>
            {
                tracingBuilder
                    .AddInMemoryExporter(activities)
                    .SetSampler(new TestSampler())
                    .AddKafkaProducerInstrumentation<string, string>()
                    .AddKafkaConsumerInstrumentation<string, string>();
            });
        });

        using (var host = builder.Build())
        {
            await host.StartAsync();

            var topic = $"otel-topic-{Guid.NewGuid()}";
            using (var producer = host.Services.GetRequiredService<InstrumentedProducerBuilder<string, string>>().Build())
            {
                for (var i = 0; i < 100; i++)
                {
                    producer.Produce(topic, new Message<string, string>() { Key = $"any_key_{i}", Value = $"any_value_{i}", });
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

                Assert.Equal(100, j);
            }

            await host.StopAsync();
        }

        Assert.Equal(200, activities.Count);
    }
}
