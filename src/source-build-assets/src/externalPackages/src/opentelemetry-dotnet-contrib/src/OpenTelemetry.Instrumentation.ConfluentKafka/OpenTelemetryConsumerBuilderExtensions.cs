// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Instrumentation.ConfluentKafka;

namespace Confluent.Kafka;

/// <summary>
/// Extensions for <see cref="ConsumerBuilder{TKey, TValue}"/>.
/// </summary>
public static class OpenTelemetryConsumerBuilderExtensions
{
    /// <summary>
    /// Converts a <see cref="ConsumerBuilder{TKey, TValue}"/> to an <see cref="InstrumentedConsumerBuilder{TKey,TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    /// <typeparam name="TValue">Type of the value.</typeparam>
    /// <param name="consumerBuilder">The <see cref="ConsumerBuilder{TKey, TValue}"/> instance.</param>
    /// <returns>An <see cref="InstrumentedConsumerBuilder{TKey, TValue}"/> instance.</returns>
#if NET
    [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Use 'InstrumentedConsumerBuilder<TKey, TValue>' constructor to avoid reflection.")]
#endif
    public static InstrumentedConsumerBuilder<TKey, TValue> AsInstrumentedConsumerBuilder<TKey, TValue>(this ConsumerBuilder<TKey, TValue> consumerBuilder)
    {
        var result = new InstrumentedConsumerBuilder<TKey, TValue>(consumerBuilder.GetInternalConfig() ?? []);
        result.SetInternalErrorHandler(consumerBuilder.GetInternalErrorHandler());
        result.SetInternalLogHandler(consumerBuilder.GetInternalLogHandler());
        result.SetInternalStatisticsHandler(consumerBuilder.GetInternalStatisticsHandler());
        result.SetInternalOAuthBearerTokenRefreshHandler(consumerBuilder.GetInternalOAuthBearerTokenRefreshHandler());
        result.SetInternalOffsetsCommittedHandler(consumerBuilder.GetInternalOffsetsCommittedHandler());
        result.SetInternalPartitionsAssignedHandler(consumerBuilder.GetInternalPartitionsAssignedHandler());
        result.SetInternalPartitionsRevokedHandler(consumerBuilder.GetInternalPartitionsRevokedHandler());
        result.SetInternalPartitionsLostHandler(consumerBuilder.GetInternalPartitionsLostHandler());
        result.SetInternalRevokedOrLostHandlerIsFunc(consumerBuilder.GetInternalRevokedOrLostHandlerIsFunc() ?? false);
        result.SetInternalKeyDeserializer(consumerBuilder.GetInternalKeyDeserializer());
        result.SetInternalValueDeserializer(consumerBuilder.GetInternalValueDeserializer());
        return result;
    }

    internal static IEnumerable<KeyValuePair<string, string>>? GetInternalConfig<TKey, TValue>(this ConsumerBuilder<TKey, TValue> consumerBuilder)
        => ReflectionHelpers.GetProperty(consumerBuilder, "Config") as IEnumerable<KeyValuePair<string, string>>;

    internal static Action<IConsumer<TKey, TValue>, Error>? GetInternalErrorHandler<TKey, TValue>(this ConsumerBuilder<TKey, TValue> consumerBuilder)
        => ReflectionHelpers.GetProperty(consumerBuilder, "ErrorHandler") as Action<IConsumer<TKey, TValue>, Error>;

    internal static Action<IConsumer<TKey, TValue>, LogMessage>? GetInternalLogHandler<TKey, TValue>(this ConsumerBuilder<TKey, TValue> consumerBuilder)
        => ReflectionHelpers.GetProperty(consumerBuilder, "LogHandler") as Action<IConsumer<TKey, TValue>, LogMessage>;

    internal static Action<IConsumer<TKey, TValue>, string>? GetInternalStatisticsHandler<TKey, TValue>(this ConsumerBuilder<TKey, TValue> consumerBuilder)
        => ReflectionHelpers.GetProperty(consumerBuilder, "StatisticsHandler") as Action<IConsumer<TKey, TValue>, string>;

    internal static Action<IConsumer<TKey, TValue>, string>? GetInternalOAuthBearerTokenRefreshHandler<TKey, TValue>(this ConsumerBuilder<TKey, TValue> consumerBuilder)
        => ReflectionHelpers.GetProperty(consumerBuilder, "OAuthBearerTokenRefreshHandler") as Action<IConsumer<TKey, TValue>, string>;

    internal static Action<IConsumer<TKey, TValue>, CommittedOffsets>? GetInternalOffsetsCommittedHandler<TKey, TValue>(this ConsumerBuilder<TKey, TValue> consumerBuilder)
        => ReflectionHelpers.GetProperty(consumerBuilder, "OffsetsCommittedHandler") as Action<IConsumer<TKey, TValue>, CommittedOffsets>;

    internal static Func<IConsumer<TKey, TValue>, List<TopicPartition>, IEnumerable<TopicPartitionOffset>>? GetInternalPartitionsAssignedHandler<TKey, TValue>(this ConsumerBuilder<TKey, TValue> consumerBuilder)
        => ReflectionHelpers.GetProperty(consumerBuilder, "PartitionsAssignedHandler") as Func<IConsumer<TKey, TValue>, List<TopicPartition>, IEnumerable<TopicPartitionOffset>>;

    internal static Func<IConsumer<TKey, TValue>, List<TopicPartitionOffset>, IEnumerable<TopicPartitionOffset>>? GetInternalPartitionsRevokedHandler<TKey, TValue>(this ConsumerBuilder<TKey, TValue> consumerBuilder)
        => ReflectionHelpers.GetProperty(consumerBuilder, "PartitionsRevokedHandler") as Func<IConsumer<TKey, TValue>, List<TopicPartitionOffset>, IEnumerable<TopicPartitionOffset>>;

    internal static Func<IConsumer<TKey, TValue>, List<TopicPartitionOffset>, IEnumerable<TopicPartitionOffset>>? GetInternalPartitionsLostHandler<TKey, TValue>(this ConsumerBuilder<TKey, TValue> consumerBuilder)
        => ReflectionHelpers.GetProperty(consumerBuilder, "PartitionsLostHandler") as Func<IConsumer<TKey, TValue>, List<TopicPartitionOffset>, IEnumerable<TopicPartitionOffset>>;

    internal static IDeserializer<TKey>? GetInternalKeyDeserializer<TKey, TValue>(this ConsumerBuilder<TKey, TValue> consumerBuilder)
        => ReflectionHelpers.GetProperty(consumerBuilder, "KeyDeserializer") as IDeserializer<TKey>;

    internal static IDeserializer<TValue>? GetInternalValueDeserializer<TKey, TValue>(this ConsumerBuilder<TKey, TValue> consumerBuilder)
        => ReflectionHelpers.GetProperty(consumerBuilder, "ValueDeserializer") as IDeserializer<TValue>;

    internal static bool? GetInternalRevokedOrLostHandlerIsFunc<TKey, TValue>(this ConsumerBuilder<TKey, TValue> consumerBuilder)
        => ReflectionHelpers.GetField(consumerBuilder, "RevokedOrLostHandlerIsFunc") as bool?;

    internal static void SetInternalErrorHandler<TKey, TValue>(this ConsumerBuilder<TKey, TValue> consumerBuilder, Action<IConsumer<TKey, TValue>, Error>? value)
        => ReflectionHelpers.SetProperty(consumerBuilder, "ErrorHandler", value);

    internal static void SetInternalLogHandler<TKey, TValue>(this ConsumerBuilder<TKey, TValue> consumerBuilder, Action<IConsumer<TKey, TValue>, LogMessage>? value)
        => ReflectionHelpers.SetProperty(consumerBuilder, "LogHandler", value);

    internal static void SetInternalStatisticsHandler<TKey, TValue>(this ConsumerBuilder<TKey, TValue> consumerBuilder, Action<IConsumer<TKey, TValue>, string>? value)
        => ReflectionHelpers.SetProperty(consumerBuilder, "StatisticsHandler", value);

    internal static void SetInternalOAuthBearerTokenRefreshHandler<TKey, TValue>(this ConsumerBuilder<TKey, TValue> consumerBuilder, Action<IConsumer<TKey, TValue>, string>? value)
        => ReflectionHelpers.SetProperty(consumerBuilder, "OAuthBearerTokenRefreshHandler", value);

    internal static void SetInternalOffsetsCommittedHandler<TKey, TValue>(this ConsumerBuilder<TKey, TValue> consumerBuilder, Action<IConsumer<TKey, TValue>, CommittedOffsets>? value)
        => ReflectionHelpers.SetProperty(consumerBuilder, "OffsetsCommittedHandler", value);

    internal static void SetInternalPartitionsAssignedHandler<TKey, TValue>(this ConsumerBuilder<TKey, TValue> consumerBuilder, Func<IConsumer<TKey, TValue>, List<TopicPartition>, IEnumerable<TopicPartitionOffset>>? value)
        => ReflectionHelpers.SetProperty(consumerBuilder, "PartitionsAssignedHandler", value);

    internal static void SetInternalPartitionsRevokedHandler<TKey, TValue>(this ConsumerBuilder<TKey, TValue> consumerBuilder, Func<IConsumer<TKey, TValue>, List<TopicPartitionOffset>, IEnumerable<TopicPartitionOffset>>? value)
        => ReflectionHelpers.SetProperty(consumerBuilder, "PartitionsRevokedHandler", value);

    internal static void SetInternalPartitionsLostHandler<TKey, TValue>(this ConsumerBuilder<TKey, TValue> consumerBuilder, Func<IConsumer<TKey, TValue>, List<TopicPartitionOffset>, IEnumerable<TopicPartitionOffset>>? value)
        => ReflectionHelpers.SetProperty(consumerBuilder, "PartitionsLostHandler", value);

    internal static void SetInternalKeyDeserializer<TKey, TValue>(this ConsumerBuilder<TKey, TValue> consumerBuilder, IDeserializer<TKey>? value)
        => ReflectionHelpers.SetProperty(consumerBuilder, "KeyDeserializer", value);

    internal static void SetInternalValueDeserializer<TKey, TValue>(this ConsumerBuilder<TKey, TValue> consumerBuilder, IDeserializer<TValue>? value)
        => ReflectionHelpers.SetProperty(consumerBuilder, "ValueDeserializer", value);

    internal static void SetInternalRevokedOrLostHandlerIsFunc<TKey, TValue>(this ConsumerBuilder<TKey, TValue> consumerBuilder, bool? value)
        => ReflectionHelpers.SetField(consumerBuilder, "RevokedOrLostHandlerIsFunc", value);
}
