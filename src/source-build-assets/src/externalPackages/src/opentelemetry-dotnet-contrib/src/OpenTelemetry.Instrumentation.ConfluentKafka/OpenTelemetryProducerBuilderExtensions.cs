// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Instrumentation.ConfluentKafka;

namespace Confluent.Kafka;

/// <summary>
/// Extensions for <see cref="ProducerBuilder{TKey,TValue}"/>.
/// </summary>
public static class OpenTelemetryProducerBuilderExtensions
{
    /// <summary>
    /// Converts <see cref="ProducerBuilder{TKey,TValue}"/> to <see cref="InstrumentedProducerBuilder{TKey,TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    /// <typeparam name="TValue">Type of the value.</typeparam>
    /// <param name="producerBuilder">The <see cref="ProducerBuilder{TKey, TValue}"/> instance.</param>
    /// <returns>An <see cref="InstrumentedProducerBuilder{TKey, TValue}"/> instance.</returns>
#if NET
    [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Use 'InstrumentedProducerBuilder<TKey, TValue>' constructor to avoid reflection.")]
#endif
    public static InstrumentedProducerBuilder<TKey, TValue> AsInstrumentedProducerBuilder<TKey, TValue>(this ProducerBuilder<TKey, TValue> producerBuilder)
    {
        var instrumentedProducerBuilder = new InstrumentedProducerBuilder<TKey, TValue>(producerBuilder.GetInternalConfig() ?? []);
        instrumentedProducerBuilder.SetInternalLogHandler(producerBuilder.GetInternalLogHandler());
        instrumentedProducerBuilder.SetInternalErrorHandler(producerBuilder.GetInternalErrorHandler());
        instrumentedProducerBuilder.SetInternalStatisticsHandler(producerBuilder.GetInternalStatisticsHandler());
        instrumentedProducerBuilder.SetInternalOAuthBearerTokenRefreshHandler(producerBuilder.GetInternalOAuthBearerTokenRefreshHandler());
        instrumentedProducerBuilder.SetInternalPartitioners(producerBuilder.GetInternalPartitioners());
        instrumentedProducerBuilder.SetInternalDefaultPartitioner(producerBuilder.GetInternalDefaultPartitioner());
        instrumentedProducerBuilder.SetInternalKeySerializer(producerBuilder.GetInternalKeySerializer());
        instrumentedProducerBuilder.SetInternalValueSerializer(producerBuilder.GetInternalValueSerializer());
        instrumentedProducerBuilder.SetInternalAsyncKeySerializer(producerBuilder.GetInternalAsyncKeySerializer());
        instrumentedProducerBuilder.SetInternalAsyncValueSerializer(producerBuilder.GetInternalAsyncValueSerializer());
        return instrumentedProducerBuilder;
    }

    internal static IEnumerable<KeyValuePair<string, string>>? GetInternalConfig<TKey, TValue>(this ProducerBuilder<TKey, TValue> producerBuilder)
        => ReflectionHelpers.GetProperty(producerBuilder, "Config") as IEnumerable<KeyValuePair<string, string>>;

    internal static Action<IProducer<TKey, TValue>, Error>? GetInternalErrorHandler<TKey, TValue>(this ProducerBuilder<TKey, TValue> producerBuilder)
        => ReflectionHelpers.GetProperty(producerBuilder, "ErrorHandler") as Action<IProducer<TKey, TValue>, Error>;

    internal static Action<IProducer<TKey, TValue>, LogMessage>? GetInternalLogHandler<TKey, TValue>(this ProducerBuilder<TKey, TValue> producerBuilder)
        => ReflectionHelpers.GetProperty(producerBuilder, "LogHandler") as Action<IProducer<TKey, TValue>, LogMessage>;

    internal static Action<IProducer<TKey, TValue>, string>? GetInternalStatisticsHandler<TKey, TValue>(this ProducerBuilder<TKey, TValue> producerBuilder)
        => ReflectionHelpers.GetProperty(producerBuilder, "StatisticsHandler") as Action<IProducer<TKey, TValue>, string>;

    internal static Action<IProducer<TKey, TValue>, string>? GetInternalOAuthBearerTokenRefreshHandler<TKey, TValue>(this ProducerBuilder<TKey, TValue> producerBuilder)
        => ReflectionHelpers.GetProperty(producerBuilder, "OAuthBearerTokenRefreshHandler") as Action<IProducer<TKey, TValue>, string>;

    internal static Dictionary<string, PartitionerDelegate>? GetInternalPartitioners<TKey, TValue>(this ProducerBuilder<TKey, TValue> producerBuilder)
        => ReflectionHelpers.GetProperty(producerBuilder, "Partitioners") as Dictionary<string, PartitionerDelegate>;

    internal static PartitionerDelegate? GetInternalDefaultPartitioner<TKey, TValue>(this ProducerBuilder<TKey, TValue> producerBuilder)
        => ReflectionHelpers.GetProperty(producerBuilder, "DefaultPartitioner") as PartitionerDelegate;

    internal static ISerializer<TKey>? GetInternalKeySerializer<TKey, TValue>(this ProducerBuilder<TKey, TValue> producerBuilder)
        => ReflectionHelpers.GetProperty(producerBuilder, "KeySerializer") as ISerializer<TKey>;

    internal static ISerializer<TValue>? GetInternalValueSerializer<TKey, TValue>(this ProducerBuilder<TKey, TValue> producerBuilder)
        => ReflectionHelpers.GetProperty(producerBuilder, "ValueSerializer") as ISerializer<TValue>;

    internal static IAsyncSerializer<TKey>? GetInternalAsyncKeySerializer<TKey, TValue>(this ProducerBuilder<TKey, TValue> producerBuilder)
        => ReflectionHelpers.GetProperty(producerBuilder, "AsyncKeySerializer") as IAsyncSerializer<TKey>;

    internal static IAsyncSerializer<TValue>? GetInternalAsyncValueSerializer<TKey, TValue>(this ProducerBuilder<TKey, TValue> producerBuilder)
        => ReflectionHelpers.GetProperty(producerBuilder, "AsyncValueSerializer") as IAsyncSerializer<TValue>;

    internal static void SetInternalConfig<TKey, TValue>(this ProducerBuilder<TKey, TValue> producerBuilder, IEnumerable<KeyValuePair<string, string>>? value)
        => ReflectionHelpers.SetProperty(producerBuilder, "Config", value);

    internal static void SetInternalErrorHandler<TKey, TValue>(this ProducerBuilder<TKey, TValue> producerBuilder, Action<IProducer<TKey, TValue>, Error>? value)
        => ReflectionHelpers.SetProperty(producerBuilder, "ErrorHandler", value);

    internal static void SetInternalLogHandler<TKey, TValue>(this ProducerBuilder<TKey, TValue> producerBuilder, Action<IProducer<TKey, TValue>, LogMessage>? value)
        => ReflectionHelpers.SetProperty(producerBuilder, "LogHandler", value);

    internal static void SetInternalStatisticsHandler<TKey, TValue>(this ProducerBuilder<TKey, TValue> producerBuilder, Action<IProducer<TKey, TValue>, string>? value)
        => ReflectionHelpers.SetProperty(producerBuilder, "StatisticsHandler", value);

    internal static void SetInternalOAuthBearerTokenRefreshHandler<TKey, TValue>(this ProducerBuilder<TKey, TValue> producerBuilder, Action<IProducer<TKey, TValue>, string>? value)
        => ReflectionHelpers.SetProperty(producerBuilder, "OAuthBearerTokenRefreshHandler", value);

    internal static void SetInternalPartitioners<TKey, TValue>(this ProducerBuilder<TKey, TValue> producerBuilder, Dictionary<string, PartitionerDelegate>? value)
        => ReflectionHelpers.SetProperty(producerBuilder, "Partitioners", value);

    internal static void SetInternalDefaultPartitioner<TKey, TValue>(this ProducerBuilder<TKey, TValue> producerBuilder, PartitionerDelegate? value)
        => ReflectionHelpers.SetProperty(producerBuilder, "DefaultPartitioner", value);

    internal static void SetInternalKeySerializer<TKey, TValue>(this ProducerBuilder<TKey, TValue> producerBuilder, ISerializer<TKey>? value)
        => ReflectionHelpers.SetProperty(producerBuilder, "KeySerializer", value);

    internal static void SetInternalValueSerializer<TKey, TValue>(this ProducerBuilder<TKey, TValue> producerBuilder, ISerializer<TValue>? value)
        => ReflectionHelpers.SetProperty(producerBuilder, "ValueSerializer", value);

    internal static void SetInternalAsyncKeySerializer<TKey, TValue>(this ProducerBuilder<TKey, TValue> producerBuilder, IAsyncSerializer<TKey>? value)
        => ReflectionHelpers.SetProperty(producerBuilder, "AsyncKeySerializer", value);

    internal static void SetInternalAsyncValueSerializer<TKey, TValue>(this ProducerBuilder<TKey, TValue> producerBuilder, IAsyncSerializer<TValue>? value)
        => ReflectionHelpers.SetProperty(producerBuilder, "AsyncValueSerializer", value);
}
