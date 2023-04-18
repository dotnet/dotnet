// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Language;

internal class DefaultAllowedChildTagDescriptorBuilder : AllowedChildTagDescriptorBuilder, IBuilder<AllowedChildTagDescriptor>
{
    private readonly DefaultTagHelperDescriptorBuilder _parent;
    private RazorDiagnosticCollection? _diagnostics;

    public DefaultAllowedChildTagDescriptorBuilder(DefaultTagHelperDescriptorBuilder parent)
    {
        _parent = parent;
    }

    public override string? Name { get; set; }

    public override string? DisplayName { get; set; }

    public override RazorDiagnosticCollection Diagnostics => _diagnostics ??= new RazorDiagnosticCollection();

    public AllowedChildTagDescriptor Build()
    {
        var diagnostics = new PooledHashSet<RazorDiagnostic>();
        try
        {
            Validate(ref diagnostics);

            diagnostics.UnionWith(_diagnostics);

            var displayName = DisplayName ?? Name;

            var descriptor = new DefaultAllowedChildTagDescriptor(
                Name,
                displayName,
                diagnostics.ToArray());

            return descriptor;
        }
        finally
        {
            diagnostics.ClearAndFree();
        }
    }

    private void Validate(ref PooledHashSet<RazorDiagnostic> diagnostics)
    {
        if (Name.IsNullOrWhiteSpace())
        {
            var diagnostic = RazorDiagnosticFactory.CreateTagHelper_InvalidRestrictedChildNullOrWhitespace(_parent.GetDisplayName());

            diagnostics.Add(diagnostic);
        }
        else if (Name != TagHelperMatchingConventions.ElementCatchAllName)
        {
            foreach (var character in Name)
            {
                if (char.IsWhiteSpace(character) || HtmlConventions.IsInvalidNonWhitespaceHtmlCharacters(character))
                {
                    var diagnostic = RazorDiagnosticFactory.CreateTagHelper_InvalidRestrictedChild(_parent.GetDisplayName(), Name, character);

                    diagnostics.Add(diagnostic);
                }
            }
        }
    }
}
