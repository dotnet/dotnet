// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

internal static partial class SyntaxListBuilderPool
{
    private sealed class Policy : IPooledObjectPolicy<SyntaxListBuilder>
    {
        private const int InitializeBuilderSize = 8;

        public static readonly Policy Instance = new();

        private Policy()
        {
        }

        public SyntaxListBuilder Create() => new(InitializeBuilderSize);

        public bool Return(SyntaxListBuilder builder)
        {
            builder.ClearInternal();
            return true;
        }
    }
}
