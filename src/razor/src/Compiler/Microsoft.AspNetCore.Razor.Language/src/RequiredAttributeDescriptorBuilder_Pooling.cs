// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.Language;

public partial class RequiredAttributeDescriptorBuilder
{
    internal static readonly ObjectPool<RequiredAttributeDescriptorBuilder> Pool = DefaultPool.Create(Policy.Instance);

    internal static RequiredAttributeDescriptorBuilder GetInstance(TagMatchingRuleDescriptorBuilder parent)
    {
        var builder = Pool.Get();

        builder._parent = parent;

        return builder;
    }

    private protected override void Reset()
    {
        _parent = null;

        Name = null;
        NameComparisonMode = default;
        Value = null;
        ValueComparisonMode = default;

        _metadata.Clear();
    }

    private sealed class Policy : PooledBuilderPolicy<RequiredAttributeDescriptorBuilder>
    {
        public static readonly Policy Instance = new();

        private Policy()
        {
        }

        public override RequiredAttributeDescriptorBuilder Create() => new();
    }
}
