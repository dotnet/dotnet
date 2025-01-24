// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.Language.Components;

namespace Microsoft.AspNetCore.Razor.Language.Intermediate;

public sealed class ComponentAttributeIntermediateNode : IntermediateNode
{
    public ComponentAttributeIntermediateNode()
    {
    }

    public ComponentAttributeIntermediateNode(TagHelperHtmlAttributeIntermediateNode attributeNode)
    {
        if (attributeNode == null)
        {
            throw new ArgumentNullException(nameof(attributeNode));
        }

        AttributeName = attributeNode.AttributeName;
        AttributeStructure = attributeNode.AttributeStructure;
        Source = attributeNode.Source;

        foreach (var annotation in attributeNode.Annotations)
        {
            Annotations[annotation.Key] = annotation.Value;
        }

        for (var i = 0; i < attributeNode.Children.Count; i++)
        {
            Children.Add(attributeNode.Children[i]);
        }

        for (var i = 0; i < attributeNode.Diagnostics.Count; i++)
        {
            Diagnostics.Add(attributeNode.Diagnostics[i]);
        }
    }

    public ComponentAttributeIntermediateNode(TagHelperPropertyIntermediateNode propertyNode)
    {
        if (propertyNode == null)
        {
            throw new ArgumentNullException(nameof(propertyNode));
        }

        var attributeName = propertyNode.AttributeName;

        AttributeName = attributeName;
        AttributeStructure = propertyNode.AttributeStructure;
        BoundAttribute = propertyNode.BoundAttribute;
        PropertyName = propertyNode.BoundAttribute.GetPropertyName();
        Source = propertyNode.Source;
        TagHelper = propertyNode.TagHelper;
        TypeName = propertyNode.BoundAttribute.IsWeaklyTyped() ? null : propertyNode.BoundAttribute.TypeName;

        foreach (var annotation in propertyNode.Annotations)
        {
            Annotations[annotation.Key] = annotation.Value;
        }

        for (var i = 0; i < propertyNode.Children.Count; i++)
        {
            Children.Add(propertyNode.Children[i]);
        }

        for (var i = 0; i < propertyNode.Diagnostics.Count; i++)
        {
            Diagnostics.Add(propertyNode.Diagnostics[i]);
        }
    }

    public ComponentAttributeIntermediateNode(TagHelperDirectiveAttributeIntermediateNode directiveAttributeNode)
    {
        if (directiveAttributeNode == null)
        {
            throw new ArgumentNullException(nameof(directiveAttributeNode));
        }

        AttributeName = directiveAttributeNode.AttributeName;
        AttributeStructure = directiveAttributeNode.AttributeStructure;
        BoundAttribute = directiveAttributeNode.BoundAttribute;
        PropertyName = directiveAttributeNode.BoundAttribute.GetPropertyName();
        Source = directiveAttributeNode.Source;
        TagHelper = directiveAttributeNode.TagHelper;
        TypeName = directiveAttributeNode.BoundAttribute.IsWeaklyTyped() ? null : directiveAttributeNode.BoundAttribute.TypeName;

        foreach (var annotation in directiveAttributeNode.Annotations)
        {
            Annotations[annotation.Key] = annotation.Value;
        }

        for (var i = 0; i < directiveAttributeNode.Children.Count; i++)
        {
            Children.Add(directiveAttributeNode.Children[i]);
        }

        for (var i = 0; i < directiveAttributeNode.Diagnostics.Count; i++)
        {
            Diagnostics.Add(directiveAttributeNode.Diagnostics[i]);
        }
    }

    public ComponentAttributeIntermediateNode(TagHelperDirectiveAttributeParameterIntermediateNode directiveAttributeParameterNode)
    {
        if (directiveAttributeParameterNode == null)
        {
            throw new ArgumentNullException(nameof(directiveAttributeParameterNode));
        }

        AttributeName = directiveAttributeParameterNode.AttributeNameWithoutParameter;
        AttributeStructure = directiveAttributeParameterNode.AttributeStructure;
        BoundAttribute = directiveAttributeParameterNode.BoundAttribute;
        PropertyName = directiveAttributeParameterNode.BoundAttributeParameter.GetPropertyName();
        Source = directiveAttributeParameterNode.Source;
        TagHelper = directiveAttributeParameterNode.TagHelper;
        TypeName = directiveAttributeParameterNode.BoundAttributeParameter.TypeName;

        foreach (var annotation in directiveAttributeParameterNode.Annotations)
        {
            Annotations[annotation.Key] = annotation.Value;
        }

        for (var i = 0; i < directiveAttributeParameterNode.Children.Count; i++)
        {
            Children.Add(directiveAttributeParameterNode.Children[i]);
        }

        for (var i = 0; i < directiveAttributeParameterNode.Diagnostics.Count; i++)
        {
            Diagnostics.Add(directiveAttributeParameterNode.Diagnostics[i]);
        }
    }

    public override IntermediateNodeCollection Children { get; } = new IntermediateNodeCollection();

    public string AttributeName { get; set; }

    public AttributeStructure AttributeStructure { get; set; }

    public BoundAttributeDescriptor BoundAttribute { get; set; }

    public string PropertyName { get; set; }

    public TagHelperDescriptor TagHelper { get; set; }

    public string TypeName { get; set; }

    public string GloballyQualifiedTypeName { get; set; }

    public override void Accept(IntermediateNodeVisitor visitor)
    {
        if (visitor == null)
        {
            throw new ArgumentNullException(nameof(visitor));
        }

        visitor.VisitComponentAttribute(this);
    }

    public override void FormatNode(IntermediateNodeFormatter formatter)
    {
        if (formatter == null)
        {
            throw new ArgumentNullException(nameof(formatter));
        }

        formatter.WriteContent(AttributeName);

        formatter.WriteProperty(nameof(AttributeName), AttributeName);
        formatter.WriteProperty(nameof(AttributeStructure), AttributeStructure.ToString());
        formatter.WriteProperty(nameof(BoundAttribute), BoundAttribute?.DisplayName);
        formatter.WriteProperty(nameof(PropertyName), PropertyName);
        formatter.WriteProperty(nameof(TagHelper), TagHelper?.DisplayName);
        formatter.WriteProperty(nameof(TypeName), TypeName);
        formatter.WriteProperty(nameof(GloballyQualifiedTypeName), GloballyQualifiedTypeName);
    }

    public bool TryParseEventCallbackTypeArgument(out string argument)
    {
        if (TryParseEventCallbackTypeArgument(out ReadOnlyMemory<char> memory))
        {
            argument = memory.ToString();
            return true;
        }

        argument = null;
        return false;
    }

    internal bool TryParseEventCallbackTypeArgument(out ReadOnlyMemory<char> argument)
    {
        // This is ugly and ad-hoc, but for various layering reasons we can't just use Roslyn APIs
        // to parse this. We need to parse this just before we write it out to the code generator,
        // so we can't compute it up front either.

        if (BoundAttribute == null || !BoundAttribute.IsEventCallbackProperty())
        {
            throw new InvalidOperationException("This attribute is not an EventCallback attribute.");
        }

        return TryGetEventCallbackArgument(TypeName.AsMemory(), out argument);
    }

    internal static bool TryGetEventCallbackArgument(ReadOnlyMemory<char> candidate, out ReadOnlyMemory<char> argument)
    {
        // Strip 'global::' from the candidate.
        if (candidate.Span.StartsWith("global::".AsSpan()))
        {
            candidate = candidate["global::".Length..];
        }

        var eventCallbackName = ComponentsApi.EventCallback.FullTypeName.AsSpan();

        // Check to see if this is the non-generic form. If so, there's no argument to retrieve.
        if (candidate.Span.Equals(eventCallbackName, StringComparison.Ordinal))
        {
            argument = default;
            return false;
        }

        if (candidate.Length <= eventCallbackName.Length + "<>".Length ||
            !candidate.Span.StartsWith(eventCallbackName, StringComparison.Ordinal))
        {
            argument = default;
            return false;
        }

        var afterCallbackName = candidate[eventCallbackName.Length..];
        if (afterCallbackName.Span is ['<', .., '>'])
        {
            argument = afterCallbackName[1..^1];
            return true;
        }

        // If we get here this is a failure. This should only happen if someone manages to mangle the name with extensibility.
        // We don't really want to crash though.
        argument = default;
        return false;
    }

    internal static bool TryGetActionArgument(ReadOnlyMemory<char> candidate, out ReadOnlyMemory<char> argument)
    {
        // Strip 'global::' from the candidate.
        if (candidate.Span.StartsWith("global::".AsSpan()))
        {
            candidate = candidate["global::".Length..];
        }

        if (!candidate.Span.StartsWith("System.Action".AsSpan()))
        {
            argument = default;
            return false;
        }


        candidate = candidate["System.Action".Length..];
        if (candidate.Span is ['<', .., '>'])
        {
            argument = candidate[1..^1];
            return true;
        }

        argument = default;
        return false;
    }

    /// <summary>
    /// Returns a string where the generic type argument of a System.Action&lt;MyType&lt;TItem&gt;&gt; this method
    /// constructs a new string replacing 'TItem' with <paramref name="genericType"/>
    /// </summary>
    /// <param name="candidate">The full candidate string to parse, e.g System.Action&lt;MyType&lt;TItem&gt;&gt;</param>
    /// <param name="genericType">The type name to replace the generic argument with</param>
    /// <param name="argument"/>The resulting string with the generic argument replaced with <paramref name="genericType"/></param>
    /// <returns>True if the argument could be constructed</returns>
    internal static bool TryGetGenericActionArgument(ReadOnlyMemory<char> candidate, string genericType, [NotNullWhen(true)] out string argument)
    {
        if (!TryGetActionArgument(candidate, out var actionArgument))
        {
            argument = default;
            return false;
        }

        var genericTypeStart = actionArgument.Span.IndexOf('<');
        var genericTypeEnd = actionArgument.Span.IndexOf('>');

        // Check (start + 1) to ensure there is at least one character
        // between the start and end
        if (genericTypeStart <= 0 || genericTypeEnd < (genericTypeStart + 1))
        {
            argument = default;
            return false;
        }

        argument = actionArgument[0..(genericTypeStart + 1)] + genericType + actionArgument[genericTypeEnd..];
        return true;
    }
}
