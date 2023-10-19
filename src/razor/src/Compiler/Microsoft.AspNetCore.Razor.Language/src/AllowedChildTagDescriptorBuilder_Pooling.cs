// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.Language;

public partial class AllowedChildTagDescriptorBuilder
{
    internal static readonly ObjectPool<AllowedChildTagDescriptorBuilder> Pool = DefaultPool.Create(Policy.Instance);

    internal static AllowedChildTagDescriptorBuilder GetInstance(TagHelperDescriptorBuilder parent)
    {
        var builder = Pool.Get();

        builder._parent = parent;

        return builder;
    }

    private protected override void Reset()
    {
        _parent = null;

        Name = null;
        DisplayName = null;
    }

    private sealed class Policy : PooledBuilderPolicy<AllowedChildTagDescriptorBuilder>
    {
        public static readonly Policy Instance = new();

        private Policy()
        {
        }

        public override AllowedChildTagDescriptorBuilder Create() => new();
    }
}
