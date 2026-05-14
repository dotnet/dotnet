// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Shared;

namespace NuGet.ContentModel
{
    /// <summary>
    /// Represents a comparer of <see cref="ReadOnlyMemory{T}" /> that uses string ordinal comparison.
    /// </summary>
    internal sealed class ReadOnlyMemoryCharComparerOrdinal : IEqualityComparer<ReadOnlyMemory<char>>
    {
        public static ReadOnlyMemoryCharComparerOrdinal Instance { get; } = new ReadOnlyMemoryCharComparerOrdinal();

        private ReadOnlyMemoryCharComparerOrdinal()
        {
        }

        public bool Equals(ReadOnlyMemory<char> x, ReadOnlyMemory<char> y)
        {
            return x.Span.Equals(y.Span, StringComparison.Ordinal);
        }

        public int GetHashCode(ReadOnlyMemory<char> obj)
        {
            if (obj.Length == 0)
            {
                return 0;
            }

            var combiner = new HashCodeCombiner();
            foreach (var character in obj.Span)
            {
                combiner.AddObject(character);
            }
            return combiner.CombinedHash;
        }
    }
}
