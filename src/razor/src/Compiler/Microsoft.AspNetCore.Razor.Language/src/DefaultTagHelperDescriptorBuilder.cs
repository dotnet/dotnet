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

    private DocumentationObject _documentationObject;

    private List<DefaultAllowedChildTagDescriptorBuilder>? _allowedChildTags;
    private List<DefaultBoundAttributeDescriptorBuilder>? _attributeBuilders;
    private List<DefaultTagMatchingRuleDescriptorBuilder>? _tagMatchingRuleBuilders;
    private RazorDiagnosticCollection? _diagnostics;
    private MetadataHolder _metadata;

    private DefaultTagHelperDescriptorBuilder()
    {
    }

    public DefaultTagHelperDescriptorBuilder(string kind, string name, string assemblyName)
        : this()
    {
        _kind = kind;
        _name = name;
        _assemblyName = assemblyName;
    }

    public override string Kind => _kind.AssumeNotNull();
    public override string Name => _name.AssumeNotNull();
    public override string AssemblyName => _assemblyName.AssumeNotNull();
    public override string? DisplayName { get; set; }
    public override string? TagOutputHint { get; set; }
    public override bool CaseSensitive { get; set; }

    public override string? Documentation
    {
        get => _documentationObject.GetText();
        set => _documentationObject = new(value);
    }

    public override IDictionary<string, string?> Metadata => _metadata.MetadataDictionary;

    public override void SetMetadata(MetadataCollection metadata) => _metadata.SetMetadataCollection(metadata);

    public override bool TryGetMetadataValue(string key, [NotNullWhen(true)] out string? value)
        => _metadata.TryGetMetadataValue(key, out value);

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

    internal override void SetDocumentation(string? text)
    {
        _documentationObject = new(text);
    }

    internal override void SetDocumentation(DocumentationDescriptor? documentation)
    {
        _documentationObject = new(documentation);
    }

    public override TagHelperDescriptor Build()
    {
        using var diagnostics = new PooledHashSet<RazorDiagnostic>();

        diagnostics.UnionWith(_diagnostics);

        var allowedChildTags = _allowedChildTags.BuildAllOrEmpty(s_allowedChildTagSetPool);
        var tagMatchingRules = _tagMatchingRuleBuilders.BuildAllOrEmpty(s_tagMatchingRuleSetPool);
        var attributes = _attributeBuilders.BuildAllOrEmpty(s_boundAttributeSetPool);

        _metadata.AddIfMissing(TagHelperMetadata.Runtime.Name, TagHelperConventions.DefaultKind);
        var metadata = _metadata.GetMetadataCollection();

        var descriptor = new DefaultTagHelperDescriptor(
            Kind,
            Name,
            AssemblyName,
            GetDisplayName(),
            _documentationObject,
            TagOutputHint,
            CaseSensitive,
            tagMatchingRules,
            attributes,
            allowedChildTags,
            metadata,
            diagnostics.ToArray());

        return descriptor;
    }

    public override void Reset()
    {
        _documentationObject = default;
        TagOutputHint = null;
        _allowedChildTags?.Clear();
        _attributeBuilders?.Clear();
        _tagMatchingRuleBuilders?.Clear();
        _metadata.Clear();
        _diagnostics?.Clear();
    }

    public string GetDisplayName()
    {
        return DisplayName ?? GetTypeName() ?? Name;

        string? GetTypeName()
        {
            return TryGetMetadataValue(TagHelperMetadata.Common.TypeName, out var value)
                ? value
                : null;
        }
    }

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

    internal override MetadataBuilder GetMetadataBuilder(string? runtimeName = null)
    {
        var metadataBuilder = new MetadataBuilder();

        metadataBuilder.Add(TagHelperMetadata.Runtime.Name, runtimeName ?? TagHelperConventions.DefaultKind);

        return metadataBuilder;
    }
}
