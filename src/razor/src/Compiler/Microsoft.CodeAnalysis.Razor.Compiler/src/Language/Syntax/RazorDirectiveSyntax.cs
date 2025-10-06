// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

internal sealed partial class RazorDirectiveSyntax
{
    private static readonly string DirectiveDescriptorKey = typeof(DirectiveDescriptor).Name;

    public DirectiveDescriptor DirectiveDescriptor
    {
        get
        {
            var descriptor = this.GetAnnotationValue(DirectiveDescriptorKey) as DirectiveDescriptor;
            return descriptor;
        }
    }

    public RazorDirectiveSyntax WithDirectiveDescriptor(DirectiveDescriptor descriptor)
    {
        var newGreen = Green.WithAnnotationsGreen([.. GetAnnotations(), new(DirectiveDescriptorKey, descriptor)]);

        return (RazorDirectiveSyntax)newGreen.CreateRed(Parent, Position);
    }
}
