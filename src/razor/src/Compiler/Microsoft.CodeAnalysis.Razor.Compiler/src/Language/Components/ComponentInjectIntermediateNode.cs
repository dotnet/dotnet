// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
using Microsoft.AspNetCore.Razor.Language.Intermediate;

namespace Microsoft.AspNetCore.Razor.Language.Components;

internal class ComponentInjectIntermediateNode : ExtensionIntermediateNode
{
    private static readonly IList<string> _injectedPropertyModifiers = new[]
    {
            $"[global::{ComponentsApi.InjectAttribute.FullTypeName}]",
            "private" // Encapsulation is the default
        };

    public ComponentInjectIntermediateNode(string typeName, string memberName, SourceSpan? typeSpan, SourceSpan? memberSpan)
    {
        TypeName = typeName;
        MemberName = memberName;
        TypeSpan = typeSpan;
        MemberSpan = memberSpan;
    }

    public string TypeName { get; }

    public string MemberName { get; }

    public SourceSpan? TypeSpan { get; }

    public SourceSpan? MemberSpan { get; }

    public override IntermediateNodeCollection Children => IntermediateNodeCollection.ReadOnly;


    public override void Accept(IntermediateNodeVisitor visitor)
    {
        if (visitor == null)
        {
            throw new ArgumentNullException(nameof(visitor));
        }

        AcceptExtensionNode(this, visitor);
    }

    public override void WriteNode(CodeTarget target, CodeRenderingContext context)
    {
        if (target == null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        context.CodeWriter.WriteAutoPropertyDeclaration(
            _injectedPropertyModifiers,
            TypeName,
            MemberName,
            TypeSpan,
            MemberSpan,
            context,
            defaultValue: true);
    }
}
