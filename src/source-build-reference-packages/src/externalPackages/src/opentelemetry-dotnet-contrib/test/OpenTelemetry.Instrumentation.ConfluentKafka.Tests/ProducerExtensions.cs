// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Confluent.Kafka;

namespace OpenTelemetry.Instrumentation.ConfluentKafka.Tests;

internal static class ProducerExtensions
{
    public static async Task FlushAsync<TKey, TValue>(this IProducer<TKey, TValue> producer)
    {
        while (producer.Flush(TimeSpan.FromMilliseconds(100)) != 0)
        {
            await Task.Delay(100);
        }
    }
}
