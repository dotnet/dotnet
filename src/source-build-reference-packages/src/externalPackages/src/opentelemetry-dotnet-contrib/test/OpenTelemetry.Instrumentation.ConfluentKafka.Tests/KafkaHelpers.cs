// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Confluent.Kafka;
using OpenTelemetry.Tests;

namespace OpenTelemetry.Instrumentation.ConfluentKafka.Tests;

internal static class KafkaHelpers
{
    public const string KafkaEndPointEnvVarName = "OTEL_KAFKAENDPOINT";

    public static readonly string? KafkaEndPoint = SkipUnlessEnvVarFoundTheoryAttribute.GetEnvironmentVariable(KafkaEndPointEnvVarName);

    public static async Task<string> ProduceTestMessageAsync()
    {
        var topic = $"otel-topic-{Guid.NewGuid()}";
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = KafkaEndPoint,
        };
        ProducerBuilder<string, string> producerBuilder = new(producerConfig);
        var producer = producerBuilder.Build();
        await producer.ProduceAsync(topic, new Message<string, string>
        {
            Value = "any_value",
        });
        return topic;
    }
}
