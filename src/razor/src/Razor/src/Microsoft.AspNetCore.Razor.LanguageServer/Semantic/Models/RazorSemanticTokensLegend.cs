// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

internal class RazorSemanticTokensLegend
{
#pragma warning disable IDE1006 // Naming Styles - These names are queried with reflection below
    private static readonly string MarkupAttributeQuoteType = "markupAttributeQuote";
    private static readonly string MarkupAttributeType = "markupAttribute";
    private static readonly string MarkupAttributeValueType = "markupAttributeValue";
    private static readonly string MarkupCommentPunctuationType = "markupCommentPunctuation";
    private static readonly string MarkupCommentType = "markupComment";
    private static readonly string MarkupElementType = "markupElement";
    private static readonly string MarkupOperatorType = "markupOperator";
    private static readonly string MarkupTagDelimiterType = "markupTagDelimiter";
    private static readonly string MarkupTextLiteralType = "markupTextLiteral";

    private static readonly string RazorCommentStarType = "razorCommentStar";
    private static readonly string RazorCommentTransitionType = "razorCommentTransition";
    private static readonly string RazorCommentType = "razorComment";
    private static readonly string RazorComponentAttributeType = "RazorComponentAttribute";
    private static readonly string RazorComponentElementType = "razorComponentElement";
    private static readonly string RazorDirectiveAttributeType = "razorDirectiveAttribute";
    private static readonly string RazorDirectiveColonType = "razorDirectiveColon";
    private static readonly string RazorDirectiveType = "razorDirective";
    private static readonly string RazorTagHelperAttributeType = "razorTagHelperAttribute";
    private static readonly string RazorTagHelperElementType = "razorTagHelperElement";
    private static readonly string RazorTransitionType = "razorTransition";
#pragma warning restore IDE1006 // Naming Styles

    public int MarkupAttribute => _razorTokenTypeMap[MarkupAttributeType];
    public int MarkupAttributeQuote => _razorTokenTypeMap[MarkupAttributeQuoteType];
    public int MarkupAttributeValue => _razorTokenTypeMap[MarkupAttributeValueType];
    public int MarkupComment => _razorTokenTypeMap[MarkupCommentType];
    public int MarkupCommentPunctuation => _razorTokenTypeMap[MarkupCommentPunctuationType];
    public int MarkupElement => _razorTokenTypeMap[MarkupElementType];
    public int MarkupOperator => _razorTokenTypeMap[MarkupOperatorType];
    public int MarkupTagDelimiter => _razorTokenTypeMap[MarkupTagDelimiterType];
    public int MarkupTextLiteral => _razorTokenTypeMap[MarkupTextLiteralType];

    public int RazorComment => _razorTokenTypeMap[RazorCommentType];
    public int RazorCommentStar => _razorTokenTypeMap[RazorCommentStarType];
    public int RazorCommentTransition => _razorTokenTypeMap[RazorCommentTransitionType];
    public int RazorComponentAttribute => _razorTokenTypeMap[RazorComponentAttributeType];
    public int RazorComponentElement => _razorTokenTypeMap[RazorComponentElementType];
    public int RazorDirective => _razorTokenTypeMap[RazorDirectiveType];
    public int RazorDirectiveAttribute => _razorTokenTypeMap[RazorDirectiveAttributeType];
    public int RazorDirectiveColon => _razorTokenTypeMap[RazorDirectiveColonType];
    public int RazorTagHelperAttribute => _razorTokenTypeMap[RazorTagHelperAttributeType];
    public int RazorTagHelperElement => _razorTokenTypeMap[RazorTagHelperElementType];
    public int RazorTransition => _razorTokenTypeMap[RazorTransitionType];

    public SemanticTokensLegend Legend => _legend;

    private readonly SemanticTokensLegend _legend;
    private readonly Dictionary<string, int> _razorTokenTypeMap;

#pragma warning disable IDE0060 // Remove unused parameter
    public RazorSemanticTokensLegend(ClientCapabilities clientCapabilities)
#pragma warning restore IDE0060 // Remove unused parameter
    {
        using var _ = ArrayBuilderPool<string>.GetPooledObject(out var builder);

#pragma warning disable CS0618 // Type or member is obsolete
        builder.AddRange(RazorSemanticTokensAccessor.RoslynTokenTypes);
#pragma warning restore CS0618 // Type or member is obsolete

        _razorTokenTypeMap = new Dictionary<string, int>();
        foreach (var razorTokenType in GetRazorSemanticTokenTypes())
        {
            _razorTokenTypeMap.Add(razorTokenType, builder.Count);
            builder.Add(razorTokenType);
        }

        _legend = new()
        {
            TokenModifiers = s_tokenModifiers,
            TokenTypes = builder.ToArray()
        };
    }

    private static ImmutableArray<string> GetRazorSemanticTokenTypes()
    {
        using var _ = ArrayBuilderPool<string>.GetPooledObject(out var builder);

        foreach (var field in typeof(RazorSemanticTokensLegend).GetFields(BindingFlags.NonPublic | BindingFlags.Static))
        {
            if (field.GetValue(null) is string value)
            {
                builder.Add(value);
            }
        }

        return builder.ToImmutable();
    }

    private static readonly string[] s_tokenModifiers = new string[] {
        // TODO: These two should come from Roslyn once an available version is inserted
        // Razor
        "static",
        // C# Modifiers
        "ReassignedVariable",
        // Razor background
        "razorCode" // Not using nameof() because the convention is for modifiers to start with a lowercase letter. Yes I am aware of what the line above this says.
    };

    [Flags]
    public enum RazorTokenModifiers
    {
        // None = 0
        // Static, from Roslyn = 1
        // ReassignedVariable, from Roslyn = 1 << 1

        // Must start after the last Roslyn modifier
        RazorCode = 1 << 2
    }
}
