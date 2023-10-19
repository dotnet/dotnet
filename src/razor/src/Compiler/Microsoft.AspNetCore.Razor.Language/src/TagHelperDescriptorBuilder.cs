// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.AspNetCore.Razor.Language;

public sealed partial class TagHelperDescriptorBuilder : TagHelperObjectBuilder<TagHelperDescriptor>
{
    private string? _kind;
    private string? _name;
    private string? _assemblyName;

    private DocumentationObject _documentationObject;
    private MetadataHolder _metadata;

    private TagHelperDescriptorBuilder()
    {
    }

    internal TagHelperDescriptorBuilder(string kind, string name, string assemblyName)
        : this()
    {
        _kind = kind ?? throw new ArgumentNullException(nameof(kind));
        _name = name ?? throw new ArgumentNullException(nameof(name));
        _assemblyName = assemblyName ?? throw new ArgumentNullException(nameof(assemblyName));
    }

    public static TagHelperDescriptorBuilder Create(string name, string assemblyName)
        => new(TagHelperConventions.DefaultKind, name, assemblyName);

    public static TagHelperDescriptorBuilder Create(string kind, string name, string assemblyName)
        => new(kind, name, assemblyName);

    public string Kind => _kind.AssumeNotNull();
    public string Name => _name.AssumeNotNull();
    public string AssemblyName => _assemblyName.AssumeNotNull();
    public string? DisplayName { get; set; }
    public string? TagOutputHint { get; set; }
    public bool CaseSensitive { get; set; }

    public string? Documentation
    {
        get => _documentationObject.GetText();
        set => _documentationObject = new(value);
    }

    public IDictionary<string, string?> Metadata => _metadata.MetadataDictionary;

    public void SetMetadata(MetadataCollection metadata) => _metadata.SetMetadataCollection(metadata);

    public bool TryGetMetadataValue(string key, [NotNullWhen(true)] out string? value)
        => _metadata.TryGetMetadataValue(key, out value);

    public TagHelperObjectBuilderCollection<AllowedChildTagDescriptor, AllowedChildTagDescriptorBuilder> AllowedChildTags { get; }
        = new(AllowedChildTagDescriptorBuilder.Pool);

    public TagHelperObjectBuilderCollection<BoundAttributeDescriptor, BoundAttributeDescriptorBuilder> BoundAttributes { get; }
        = new(BoundAttributeDescriptorBuilder.Pool);

    public TagHelperObjectBuilderCollection<TagMatchingRuleDescriptor, TagMatchingRuleDescriptorBuilder> TagMatchingRules { get; }
        = new(TagMatchingRuleDescriptorBuilder.Pool);

    public void AllowChildTag(Action<AllowedChildTagDescriptorBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var builder = AllowedChildTagDescriptorBuilder.GetInstance(this);
        configure(builder);
        AllowedChildTags.Add(builder);
    }

    public void BindAttribute(Action<BoundAttributeDescriptorBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var builder = BoundAttributeDescriptorBuilder.GetInstance(this, Kind);
        configure(builder);
        BoundAttributes.Add(builder);
    }

    public void TagMatchingRule(Action<TagMatchingRuleDescriptorBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var builder = TagMatchingRuleDescriptorBuilder.GetInstance(this);
        configure(builder);
        TagMatchingRules.Add(builder);
    }

    internal void SetDocumentation(string? text)
    {
        _documentationObject = new(text);
    }

    internal void SetDocumentation(DocumentationDescriptor? documentation)
    {
        _documentationObject = new(documentation);
    }

    private protected override TagHelperDescriptor BuildCore(ImmutableArray<RazorDiagnostic> diagnostics)
    {
        _metadata.AddIfMissing(TagHelperMetadata.Runtime.Name, TagHelperConventions.DefaultKind);
        var metadata = _metadata.GetMetadataCollection();

        return new TagHelperDescriptor(
            Kind,
            Name,
            AssemblyName,
            GetDisplayName(),
            _documentationObject,
            TagOutputHint,
            CaseSensitive,
            TagMatchingRules.ToImmutable(),
            BoundAttributes.ToImmutable(),
            AllowedChildTags.ToImmutable(),
            metadata,
            diagnostics);
    }

    internal string GetDisplayName()
    {
        return DisplayName ?? GetTypeName() ?? Name;

        string? GetTypeName()
        {
            return TryGetMetadataValue(TagHelperMetadata.Common.TypeName, out var value)
                ? value
                : null;
        }
    }

    internal MetadataBuilder GetMetadataBuilder(string? runtimeName = null)
    {
        var metadataBuilder = new MetadataBuilder();

        metadataBuilder.Add(TagHelperMetadata.Runtime.Name, runtimeName ?? TagHelperConventions.DefaultKind);

        return metadataBuilder;
    }
}
