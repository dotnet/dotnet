// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

// Note: StyleCop doesn't understand the C#11 "required" modifier yet. Remove
// this in the future once StyleCop is updated. See:
// https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/3527

#pragma warning disable SA1206 // Declaration keywords should follow order

namespace OpenTelemetry.Exporter.OneCollector;

internal readonly struct TransportSendRequest
{
    public TransportSendRequest()
    {
#if !NET
        // Note: This is needed because < NET7 doesn't understand required.
        this.ItemType = string.Empty;
        this.ItemStream = default!;
#endif
    }

#if NET
    public required string ItemType { get; init; }

    public required OneCollectorExporterSerializationFormatType ItemSerializationFormat { get; init; }

    public required Stream ItemStream { get; init; }

    public required int NumberOfItems { get; init; }
#else
    public string ItemType { get; init; }

    public OneCollectorExporterSerializationFormatType ItemSerializationFormat { get; init; }

    public Stream ItemStream { get; init; }

    public int NumberOfItems { get; init; }
#endif
}
