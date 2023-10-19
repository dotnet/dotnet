// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.Language;

public partial class BoundAttributeParameterDescriptorBuilder
{
    internal static readonly ObjectPool<BoundAttributeParameterDescriptorBuilder> Pool = DefaultPool.Create(Policy.Instance);

    internal static BoundAttributeParameterDescriptorBuilder GetInstance(BoundAttributeDescriptorBuilder parent, string kind)
    {
        var builder = Pool.Get();

        builder._parent = parent;
        builder._kind = kind;

        return builder;
    }

    private protected override void Reset()
    {
        _parent = null;
        _kind = null;
        _documentationObject = default;

        Name = null;
        TypeName = null;
        IsEnum = false;
        DisplayName = null;

        _metadata.Clear();
    }

    private sealed class Policy : PooledBuilderPolicy<BoundAttributeParameterDescriptorBuilder>
    {
        public static readonly Policy Instance = new();

        private Policy()
        {
        }

        public override BoundAttributeParameterDescriptorBuilder Create() => new();
    }
}
