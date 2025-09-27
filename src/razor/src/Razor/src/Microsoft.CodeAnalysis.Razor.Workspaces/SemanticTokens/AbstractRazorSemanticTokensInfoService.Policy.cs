// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.CodeAnalysis.Razor.SemanticTokens;

internal abstract partial class AbstractRazorSemanticTokensInfoService
{
    private sealed class Policy : IPooledObjectPolicy<List<SemanticRange>>
    {
        public static readonly Policy Instance = new();

        // Significantly larger than DefaultPool.MaximumObjectSize as these arrays are commonly large.
        // The 2048 limit should be large enough for nearly all semantic token requests, while still
        // keeping the backing arrays off the LOH.
        public const int MaximumObjectSize = 2048;

        private Policy()
        {
        }

        public List<SemanticRange> Create() => [];

        public bool Return(List<SemanticRange> list)
        {
            var count = list.Count;

            list.Clear();

            if (count > MaximumObjectSize)
            {
                list.TrimExcess();
            }

            return true;
        }
    }
}
