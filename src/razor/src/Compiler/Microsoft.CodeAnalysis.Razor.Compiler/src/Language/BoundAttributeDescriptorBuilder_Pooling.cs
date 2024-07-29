// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.Language;

public partial class BoundAttributeDescriptorBuilder
{
    internal static readonly ObjectPool<BoundAttributeDescriptorBuilder> Pool = DefaultPool.Create(Policy.Instance);

    internal static BoundAttributeDescriptorBuilder GetInstance(TagHelperDescriptorBuilder parent, string kind)
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
        _caseSensitive = null;

        Name = null;
        TypeName = null;
        IsEnum = false;
        IsDictionary = false;
        IsEditorRequired = false;
        IndexerAttributeNamePrefix = null;
        IndexerValueTypeName = null;
        DisplayName = null;
        ContainingType = null;
        Parameters.Clear();
        _metadata.Clear();
    }

    private sealed class Policy : PooledBuilderPolicy<BoundAttributeDescriptorBuilder>
    {
        public static readonly Policy Instance = new();

        private Policy()
        {
        }

        public override BoundAttributeDescriptorBuilder Create() => new();
    }
}
