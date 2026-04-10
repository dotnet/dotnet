// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

// Note: StyleCop doesn't understand the C#11 "required" modifier yet. Remove
// this in the future once StyleCop is updated. See:
// https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/3527

#if NETSTANDARD2_1_OR_GREATER || NET
using System.Diagnostics.CodeAnalysis;
#endif

namespace OpenTelemetry.Exporter.OneCollector;

internal ref struct BatchSerializationState<T>
    where T : class
{
    private Batch<T>.Enumerator enumerator;

    public BatchSerializationState(in Batch<T> batch)
    {
        this.enumerator = batch.GetEnumerator();
    }

    public bool TryGetNextItem(
#if NETSTANDARD2_1_OR_GREATER || NET
        [NotNullWhen(true)]
#endif
        out T? item)
    {
        if (this.enumerator.MoveNext())
        {
            item = this.enumerator.Current;
            return true;
        }

        item = null;
        return false;
    }
}
