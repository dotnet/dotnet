// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Exporter.OneCollector;

internal readonly struct BatchSerializationResult
{
#if NET
    public required int NumberOfItemsSerialized { get; init; }

    public required int NumberOfItemsDropped { get; init; }

    public required long PayloadSizeInBytes { get; init; }
#else
    public int NumberOfItemsSerialized { get; init; }

    public int NumberOfItemsDropped { get; init; }

    public long PayloadSizeInBytes { get; init; }
#endif

    public long? PayloadOverflowItemSizeInBytes { get; init; }
}
