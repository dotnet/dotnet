// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Language;

internal class DefaultRequiredAttributeDescriptorBuilder : RequiredAttributeDescriptorBuilder, IBuilder<RequiredAttributeDescriptor>
{
    private readonly DefaultTagMatchingRuleDescriptorBuilder _parent;
    private RazorDiagnosticCollection? _diagnostics;
    private readonly ImmutableDictionary<string, string>.Builder _metadata;

    public DefaultRequiredAttributeDescriptorBuilder(DefaultTagMatchingRuleDescriptorBuilder parent)
    {
        _parent = parent;
        _metadata = ImmutableDictionary.CreateBuilder<string, string>();
    }

    public override string? Name { get; set; }

    public override RequiredAttributeDescriptor.NameComparisonMode NameComparisonMode { get; set; }

    public override string? Value { get; set; }

    public override RequiredAttributeDescriptor.ValueComparisonMode ValueComparisonMode { get; set; }

    public override RazorDiagnosticCollection Diagnostics => _diagnostics ??= new RazorDiagnosticCollection();

    public override IDictionary<string, string> Metadata => _metadata;

    internal bool CaseSensitive => _parent.CaseSensitive;

    public RequiredAttributeDescriptor Build()
    {
        var diagnostics = new PooledHashSet<RazorDiagnostic>();
        try
        {
            Validate(ref diagnostics);

            diagnostics.UnionWith(_diagnostics);

            var displayName = GetDisplayName();

            var descriptor = new DefaultRequiredAttributeDescriptor(
                Name,
                NameComparisonMode,
                CaseSensitive,
                Value,
                ValueComparisonMode,
                displayName,
                diagnostics.ToArray(),
                _metadata.ToImmutable());

            return descriptor;
        }
        finally
        {
            diagnostics.ClearAndFree();
        }
    }

    private string GetDisplayName()
    {
        return (NameComparisonMode == RequiredAttributeDescriptor.NameComparisonMode.PrefixMatch ? string.Concat(Name, "...") : Name) ?? string.Empty;
    }

    private void Validate(ref PooledHashSet<RazorDiagnostic> diagnostics)
    {
        if (Name.IsNullOrWhiteSpace())
        {
            var diagnostic = RazorDiagnosticFactory.CreateTagHelper_InvalidTargetedAttributeNameNullOrWhitespace();

            diagnostics.Add(diagnostic);
        }
        else
        {
            var name = new StringSegment(Name);
            var isDirectiveAttribute = this.IsDirectiveAttribute();
            if (isDirectiveAttribute && name.StartsWith("@", StringComparison.Ordinal))
            {
                name = name.Subsegment(1);
            }
            else if (isDirectiveAttribute)
            {
                var diagnostic = RazorDiagnosticFactory.CreateTagHelper_InvalidRequiredDirectiveAttributeName(GetDisplayName(), Name);

                diagnostics.Add(diagnostic);
            }

            for (var i = 0; i < name.Length; i++)
            {
                var character = name[i];
                if (char.IsWhiteSpace(character) || HtmlConventions.IsInvalidNonWhitespaceHtmlCharacters(character))
                {
                    var diagnostic = RazorDiagnosticFactory.CreateTagHelper_InvalidTargetedAttributeName(Name, character);

                    diagnostics.Add(diagnostic);
                }
            }
        }
    }
}
