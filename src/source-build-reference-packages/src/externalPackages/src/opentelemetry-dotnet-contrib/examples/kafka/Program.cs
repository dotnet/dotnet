// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Confluent.Kafka;
using Examples.ConfluentKafka;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

var builder = Host.CreateApplicationBuilder(args);

const string bootstrapServers = "localhost:9092";

builder.Services.AddSingleton(_ =>
{
    ProducerConfig producerConfig = new() { BootstrapServers = bootstrapServers };
    return new InstrumentedProducerBuilder<string, string>(producerConfig);
});
builder.Services.AddSingleton(_ =>
{
    ConsumerConfig consumerConfigA = new()
    {
        BootstrapServers = bootstrapServers,
        GroupId = "group-a",
        AutoOffsetReset = AutoOffsetReset.Earliest,
        EnablePartitionEof = true,
    };
    return new InstrumentedConsumerBuilder<string, string>(consumerConfigA);
});

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing.AddConsoleExporter()
            .AddOtlpExporter()
            .AddKafkaProducerInstrumentation<string, string>()
            .AddKafkaConsumerInstrumentation<string, string>();
    })
    .WithMetrics(metering =>
    {
        metering.AddConsoleExporter()
            .AddOtlpExporter()
            .AddKafkaProducerInstrumentation<string, string>()
            .AddKafkaConsumerInstrumentation<string, string>();
    });

builder.Services.AddHostedService<ProduceConsumeHostedService>();

var app = builder.Build();
await app.RunAsync();
