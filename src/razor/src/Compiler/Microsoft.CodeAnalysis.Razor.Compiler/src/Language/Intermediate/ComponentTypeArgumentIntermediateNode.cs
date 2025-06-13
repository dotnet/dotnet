// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Diagnostics;

namespace Microsoft.AspNetCore.Razor.Language.Intermediate;

public sealed class ComponentTypeArgumentIntermediateNode : IntermediateNode
{
    public ComponentTypeArgumentIntermediateNode(TagHelperPropertyIntermediateNode propertyNode)
    {
        if (propertyNode == null)
        {
            throw new ArgumentNullException(nameof(propertyNode));
        }

        BoundAttribute = propertyNode.BoundAttribute;
        Source = propertyNode.Source;
        TagHelper = propertyNode.TagHelper;

        Debug.Assert(propertyNode.Children.Count == 1);
        Value = propertyNode.Children[0] switch
        {
            IntermediateToken t => t,
            CSharpExpressionIntermediateNode c => (IntermediateToken)c.Children[0], // TODO: can we break this in error cases?
            _ => Assumed.Unreachable<IntermediateToken>()
        };
        Children = [Value];

        AddDiagnosticsFromNode(propertyNode);
    }

    public override IntermediateNodeCollection Children { get; }

    public BoundAttributeDescriptor BoundAttribute { get; set; }

    public string TypeParameterName => BoundAttribute.Name;

    public TagHelperDescriptor TagHelper { get; set; }

    public IntermediateToken Value { get; set; }

    public override void Accept(IntermediateNodeVisitor visitor)
    {
        if (visitor == null)
        {
            throw new ArgumentNullException(nameof(visitor));
        }

        visitor.VisitComponentTypeArgument(this);
    }

    public override void FormatNode(IntermediateNodeFormatter formatter)
    {
        if (formatter == null)
        {
            throw new ArgumentNullException(nameof(formatter));
        }

        formatter.WriteContent(TypeParameterName);

        formatter.WriteProperty(nameof(BoundAttribute), BoundAttribute?.DisplayName);
        formatter.WriteProperty(nameof(TagHelper), TagHelper?.DisplayName);
    }
}
