// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.PooledObjects;
using static Microsoft.AspNetCore.Razor.Language.RequiredAttributeDescriptor;

namespace Microsoft.AspNetCore.Razor.Language;

public sealed partial class RequiredAttributeDescriptorBuilder : TagHelperObjectBuilder<RequiredAttributeDescriptor>
{
    [AllowNull]
    private TagMatchingRuleDescriptorBuilder _parent;
    private MetadataHolder _metadata;

    private RequiredAttributeDescriptorBuilder()
    {
    }

    internal RequiredAttributeDescriptorBuilder(TagMatchingRuleDescriptorBuilder parent)
    {
        _parent = parent;
    }

    public string? Name { get; set; }
    public NameComparisonMode NameComparisonMode { get; set; }
    public string? Value { get; set; }
    public ValueComparisonMode ValueComparisonMode { get; set; }

    internal bool CaseSensitive => _parent.CaseSensitive;

    public IDictionary<string, string?> Metadata => _metadata.MetadataDictionary;

    public void SetMetadata(MetadataCollection metadata) => _metadata.SetMetadataCollection(metadata);

    public bool TryGetMetadataValue(string key, [NotNullWhen(true)] out string? value)
        => _metadata.TryGetMetadataValue(key, out value);

    private protected override RequiredAttributeDescriptor BuildCore(ImmutableArray<RazorDiagnostic> diagnostics)
    {
        var displayName = GetDisplayName();
        var metadata = _metadata.GetMetadataCollection();

        return new RequiredAttributeDescriptor(
            Name ?? string.Empty,
            NameComparisonMode,
            CaseSensitive,
            Value,
            ValueComparisonMode,
            displayName,
            diagnostics,
            metadata);
    }

    private string GetDisplayName()
    {
        return (NameComparisonMode == NameComparisonMode.PrefixMatch ? string.Concat(Name, "...") : Name) ?? string.Empty;
    }

    private bool IsDirectiveAttribute()
        => TryGetMetadataValue(ComponentMetadata.Common.DirectiveAttribute, out var value) &&
           value == bool.TrueString;

    private protected override void CollectDiagnostics(ref PooledHashSet<RazorDiagnostic> diagnostics)
    {
        if (Name.IsNullOrWhiteSpace())
        {
            var diagnostic = RazorDiagnosticFactory.CreateTagHelper_InvalidTargetedAttributeNameNullOrWhitespace();

            diagnostics.Add(diagnostic);
        }
        else
        {
            var name = Name.AsSpan();
            var isDirectiveAttribute = IsDirectiveAttribute();
            if (isDirectiveAttribute && name[0] == '@')
            {
                name = name[1..];
            }
            else if (isDirectiveAttribute)
            {
                var diagnostic = RazorDiagnosticFactory.CreateTagHelper_InvalidRequiredDirectiveAttributeName(GetDisplayName(), Name);

                diagnostics.Add(diagnostic);
            }

            foreach (var ch in name)
            {
                if (char.IsWhiteSpace(ch) || HtmlConventions.IsInvalidNonWhitespaceHtmlCharacters(ch))
                {
                    var diagnostic = RazorDiagnosticFactory.CreateTagHelper_InvalidTargetedAttributeName(Name, ch);

                    diagnostics.Add(diagnostic);
                }
            }
        }
    }
}
