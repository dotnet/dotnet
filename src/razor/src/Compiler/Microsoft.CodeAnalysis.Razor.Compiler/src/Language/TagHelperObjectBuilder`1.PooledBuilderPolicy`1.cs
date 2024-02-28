// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.Language;

public abstract partial class TagHelperObjectBuilder<T>
    where T : TagHelperObject<T>
{
    private protected abstract class PooledBuilderPolicy<TBuilder> : IPooledObjectPolicy<TBuilder>
        where TBuilder : TagHelperObjectBuilder<T>
    {
        private const int MaxSize = 32;

        public abstract TBuilder Create();

        public bool Return(TBuilder builder)
        {
            builder._isBuilt = false;

            if (builder._diagnostics is { } diagnostics)
            {
                diagnostics.Clear();

                if (diagnostics.Capacity > MaxSize)
                {
                    diagnostics.Capacity = MaxSize;
                }
            }

            builder.Reset();

            return true;
        }
    }
}
