// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.Extensions.ObjectPool;
using static Microsoft.AspNetCore.Razor.Language.RequiredAttributeDescriptor;

namespace Microsoft.AspNetCore.Razor.Language;

internal partial class DefaultRequiredAttributeDescriptorBuilder : RequiredAttributeDescriptorBuilder, IBuilder<RequiredAttributeDescriptor>
{
    private static readonly ObjectPool<DefaultRequiredAttributeDescriptorBuilder> s_pool = DefaultPool.Create(Policy.Instance);

    public static DefaultRequiredAttributeDescriptorBuilder GetInstance(DefaultTagMatchingRuleDescriptorBuilder parent)
    {
        var builder = s_pool.Get();

        builder._parent = parent;

        return builder;
    }

    public static void ReturnInstance(DefaultRequiredAttributeDescriptorBuilder builder)
        => s_pool.Return(builder);

    [AllowNull]
    private DefaultTagMatchingRuleDescriptorBuilder _parent;
    private RazorDiagnosticCollection? _diagnostics;
    private Dictionary<string, string>? _metadata;

    private DefaultRequiredAttributeDescriptorBuilder()
    {
    }

    public DefaultRequiredAttributeDescriptorBuilder(DefaultTagMatchingRuleDescriptorBuilder parent)
    {
        _parent = parent;
    }

    public override string? Name { get; set; }
    public override NameComparisonMode NameComparisonMode { get; set; }
    public override string? Value { get; set; }
    public override ValueComparisonMode ValueComparisonMode { get; set; }

    public override RazorDiagnosticCollection Diagnostics => _diagnostics ??= new RazorDiagnosticCollection();

    public override IDictionary<string, string> Metadata => _metadata ??= new Dictionary<string, string>();

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
                MetadataCollection.CreateOrEmpty(_metadata));

            return descriptor;
        }
        finally
        {
            diagnostics.ClearAndFree();
        }
    }

    private string GetDisplayName()
    {
        return (NameComparisonMode == NameComparisonMode.PrefixMatch ? string.Concat(Name, "...") : Name) ?? string.Empty;
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
