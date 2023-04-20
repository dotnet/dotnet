// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.Language;

internal partial class DefaultTagHelperDescriptorBuilder : TagHelperDescriptorBuilder, IBuilder<TagHelperDescriptor>
{
    private static readonly ObjectPool<DefaultTagHelperDescriptorBuilder> s_pool = DefaultPool.Create(Policy.Instance);

    public static DefaultTagHelperDescriptorBuilder GetInstance(string name, string assemblyName)
        => GetInstance(TagHelperConventions.DefaultKind, name, assemblyName);

    public static DefaultTagHelperDescriptorBuilder GetInstance(string kind, string name, string assemblyName)
    {
        var builder = s_pool.Get();

        builder._kind = kind;
        builder._name = name;
        builder._assemblyName = assemblyName;

        builder.InitializeMetadata();

        return builder;
    }

    public static void ReturnInstance(DefaultTagHelperDescriptorBuilder builder)
        => s_pool.Return(builder);

    private static readonly ObjectPool<HashSet<AllowedChildTagDescriptor>> s_allowedChildTagSetPool
        = HashSetPool<AllowedChildTagDescriptor>.Create(AllowedChildTagDescriptorComparer.Default);

    private static readonly ObjectPool<HashSet<BoundAttributeDescriptor>> s_boundAttributeSetPool
        = HashSetPool<BoundAttributeDescriptor>.Create(BoundAttributeDescriptorComparer.Default);

    private static readonly ObjectPool<HashSet<TagMatchingRuleDescriptor>> s_tagMatchingRuleSetPool
        = HashSetPool<TagMatchingRuleDescriptor>.Create(TagMatchingRuleDescriptorComparer.Default);

    private string? _kind;
    private string? _name;
    private string? _assemblyName;

    private List<DefaultAllowedChildTagDescriptorBuilder>? _allowedChildTags;
    private List<DefaultBoundAttributeDescriptorBuilder>? _attributeBuilders;
    private List<DefaultTagMatchingRuleDescriptorBuilder>? _tagMatchingRuleBuilders;
    private RazorDiagnosticCollection? _diagnostics;
    private readonly Dictionary<string, string> _metadata;

    private DefaultTagHelperDescriptorBuilder()
    {
        _metadata = new Dictionary<string, string>(StringComparer.Ordinal);
    }

    public DefaultTagHelperDescriptorBuilder(string kind, string name, string assemblyName)
        : this()
    {
        _kind = kind;
        _name = name;
        _assemblyName = assemblyName;

        InitializeMetadata();
    }

    public override string Kind => _kind.AssumeNotNull();
    public override string Name => _name.AssumeNotNull();
    public override string AssemblyName => _assemblyName.AssumeNotNull();
    public override string? DisplayName { get; set; }
    public override string? TagOutputHint { get; set; }
    public override bool CaseSensitive { get; set; }
    public override string? Documentation { get; set; }

    public override IDictionary<string, string> Metadata => _metadata;
    public override RazorDiagnosticCollection Diagnostics => _diagnostics ??= new RazorDiagnosticCollection();

    public override IReadOnlyList<AllowedChildTagDescriptorBuilder> AllowedChildTags
    {
        get
        {
            EnsureAllowedChildTags();

            return _allowedChildTags;
        }
    }

    public override IReadOnlyList<BoundAttributeDescriptorBuilder> BoundAttributes
    {
        get
        {
            EnsureAttributeBuilders();

            return _attributeBuilders;
        }
    }

    public override IReadOnlyList<TagMatchingRuleDescriptorBuilder> TagMatchingRules
    {
        get
        {
            EnsureTagMatchingRuleBuilders();

            return _tagMatchingRuleBuilders;
        }
    }

    public override void AllowChildTag(Action<AllowedChildTagDescriptorBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        EnsureAllowedChildTags();

        var builder = DefaultAllowedChildTagDescriptorBuilder.GetInstance(this);
        configure(builder);
        _allowedChildTags.Add(builder);
    }

    public override void BindAttribute(Action<BoundAttributeDescriptorBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        EnsureAttributeBuilders();

        var builder = DefaultBoundAttributeDescriptorBuilder.GetInstance(this, Kind);
        configure(builder);
        _attributeBuilders.Add(builder);
    }

    public override void TagMatchingRule(Action<TagMatchingRuleDescriptorBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        EnsureTagMatchingRuleBuilders();

        var builder = DefaultTagMatchingRuleDescriptorBuilder.GetInstance(this);
        configure(builder);
        _tagMatchingRuleBuilders.Add(builder);
    }

    public override TagHelperDescriptor Build()
    {
        using var diagnostics = new PooledHashSet<RazorDiagnostic>();

        diagnostics.UnionWith(_diagnostics);

        var allowedChildTags = _allowedChildTags.BuildAllOrEmpty(s_allowedChildTagSetPool);
        var tagMatchingRules = _tagMatchingRuleBuilders.BuildAllOrEmpty(s_tagMatchingRuleSetPool);
        var attributes = _attributeBuilders.BuildAllOrEmpty(s_boundAttributeSetPool);

        var descriptor = new DefaultTagHelperDescriptor(
            Kind,
            Name,
            AssemblyName,
            GetDisplayName(),
            Documentation,
            TagOutputHint,
            CaseSensitive,
            tagMatchingRules,
            attributes,
            allowedChildTags,
            MetadataCollection.Create(_metadata),
            diagnostics.ToArray());

        return descriptor;
    }

    public override void Reset()
    {
        Documentation = null;
        TagOutputHint = null;
        _allowedChildTags?.Clear();
        _attributeBuilders?.Clear();
        _tagMatchingRuleBuilders?.Clear();
        _metadata.Clear();
        _diagnostics?.Clear();
    }

    public string GetDisplayName() => DisplayName ?? this.GetTypeName() ?? Name;

    [MemberNotNull(nameof(_allowedChildTags))]
    private void EnsureAllowedChildTags()
    {
        _allowedChildTags ??= new List<DefaultAllowedChildTagDescriptorBuilder>();
    }

    [MemberNotNull(nameof(_attributeBuilders))]
    private void EnsureAttributeBuilders()
    {
        _attributeBuilders ??= new List<DefaultBoundAttributeDescriptorBuilder>();
    }

    [MemberNotNull(nameof(_tagMatchingRuleBuilders))]
    private void EnsureTagMatchingRuleBuilders()
    {
        _tagMatchingRuleBuilders ??= new List<DefaultTagMatchingRuleDescriptorBuilder>();
    }

    private void InitializeMetadata()
    {
        // Tells code generation that these tag helpers are compatible with ITagHelper.
        // For now that's all we support.
        _metadata.Add(TagHelperMetadata.Runtime.Name, TagHelperConventions.DefaultKind);
    }
}
