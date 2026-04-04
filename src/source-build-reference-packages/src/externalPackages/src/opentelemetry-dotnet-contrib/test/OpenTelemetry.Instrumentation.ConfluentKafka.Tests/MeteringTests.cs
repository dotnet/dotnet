// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Confluent.Kafka;
using OpenTelemetry.Metrics;
using OpenTelemetry.Tests;
using Xunit;

namespace OpenTelemetry.Instrumentation.ConfluentKafka.Tests;

[Collection("Kafka")]
public class MeteringTests
{
    /*
        To run the integration tests, set the OTEL_KAFKAENDPOINT machine-level environment variable to a valid Kafka endpoint.

        To use Docker...
         1) Run: docker run -d --name kafka -p 9092:9092 confluentinc/confluent-local
         2) Set OTEL_KAFKAENDPOINT as: localhost:9092
     */

    [Trait("CategoryName", "KafkaIntegrationTests")]
    [SkipUnlessEnvVarFoundFact(KafkaHelpers.KafkaEndPointEnvVarName)]
    public async Task BasicProduceToTopicTest()
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = KafkaHelpers.KafkaEndPoint,
        };
        InstrumentedProducerBuilder<string, string> producerBuilder = new(producerConfig);
        var metrics = new List<Metric>();
        var topic = $"otel-topic-{Guid.NewGuid()}";
        using (var meterProvider = Sdk.CreateMeterProviderBuilder()
                   .AddInMemoryExporter(metrics)
                   .AddKafkaProducerInstrumentation(producerBuilder)
                   .Build())
        {
            var producer = producerBuilder.Build();
            producer.Produce(topic, new Message<string, string>
            {
                Value = "any_value",
            });

            await producer.FlushAsync();

            meterProvider.EnsureMetricsAreFlushed();
        }

        var groups = metrics.GroupBy(m => m.Name).ToArray();

        Assert.Equal(2, groups.Length);
    }

    [Trait("CategoryName", "KafkaIntegrationTests")]
    [SkipUnlessEnvVarFoundFact(KafkaHelpers.KafkaEndPointEnvVarName)]
    public async Task BasicProduceAsyncToTopicTest()
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = KafkaHelpers.KafkaEndPoint,
        };
        InstrumentedProducerBuilder<string, string> producerBuilder = new(producerConfig);
        var metrics = new List<Metric>();
        var topic = $"otel-topic-{Guid.NewGuid()}";
        using (var meterProvider = Sdk.CreateMeterProviderBuilder()
                   .AddInMemoryExporter(metrics)
                   .AddKafkaProducerInstrumentation(producerBuilder)
                   .Build())
        {
            var producer = producerBuilder.Build();
            await producer.ProduceAsync(topic, new Message<string, string>
            {
                Value = "any_value",
            });

            await producer.FlushAsync();

            meterProvider.EnsureMetricsAreFlushed();
        }

        var groups = metrics.GroupBy(m => m.Name).ToArray();

        Assert.Equal(2, groups.Length);
    }

    [Trait("CategoryName", "KafkaIntegrationTests")]
    [SkipUnlessEnvVarFoundFact(KafkaHelpers.KafkaEndPointEnvVarName)]
    public async Task BasicConsumeWithTimeoutTimespanTest()
    {
        var topic = await KafkaHelpers.ProduceTestMessageAsync();

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = KafkaHelpers.KafkaEndPoint,
            GroupId = "test-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnablePartitionEof = true,
        };
        InstrumentedConsumerBuilder<string, string> consumerBuilder = new(consumerConfig);

        var metrics = new List<Metric>();
        using (var meterProvider = Sdk.CreateMeterProviderBuilder()
                .AddInMemoryExporter(metrics)
                .AddKafkaConsumerInstrumentation(consumerBuilder)
                .Build())
        {
            using (var consumer = consumerBuilder.Build())
            {
                consumer.Subscribe(topic);
                while (true)
                {
                    var consumeResult = consumer.Consume();

                    if (consumeResult == null)
                    {
                        continue;
                    }

                    if (consumeResult.IsPartitionEOF)
                    {
                        break;
                    }
                }

                consumer.Close();
            }

            meterProvider.EnsureMetricsAreFlushed();
        }

        var groups = metrics.GroupBy(m => m.Name).ToArray();

        Assert.Equal(2, groups.Length);
    }
}
