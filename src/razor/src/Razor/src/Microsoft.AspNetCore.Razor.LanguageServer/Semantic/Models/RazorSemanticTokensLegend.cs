// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

[Export(typeof(RazorSemanticTokensLegendService))]
internal sealed class RazorSemanticTokensLegendService
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

    public int MarkupAttribute => RazorTokenTypeMap[MarkupAttributeType];
    public int MarkupAttributeQuote => RazorTokenTypeMap[MarkupAttributeQuoteType];
    public int MarkupAttributeValue => RazorTokenTypeMap[MarkupAttributeValueType];
    public int MarkupComment => RazorTokenTypeMap[MarkupCommentType];
    public int MarkupCommentPunctuation => RazorTokenTypeMap[MarkupCommentPunctuationType];
    public int MarkupElement => RazorTokenTypeMap[MarkupElementType];
    public int MarkupOperator => RazorTokenTypeMap[MarkupOperatorType];
    public int MarkupTagDelimiter => RazorTokenTypeMap[MarkupTagDelimiterType];
    public int MarkupTextLiteral => RazorTokenTypeMap[MarkupTextLiteralType];

    public int RazorComment => RazorTokenTypeMap[RazorCommentType];
    public int RazorCommentStar => RazorTokenTypeMap[RazorCommentStarType];
    public int RazorCommentTransition => RazorTokenTypeMap[RazorCommentTransitionType];
    public int RazorComponentAttribute => RazorTokenTypeMap[RazorComponentAttributeType];
    public int RazorComponentElement => RazorTokenTypeMap[RazorComponentElementType];
    public int RazorDirective => RazorTokenTypeMap[RazorDirectiveType];
    public int RazorDirectiveAttribute => RazorTokenTypeMap[RazorDirectiveAttributeType];
    public int RazorDirectiveColon => RazorTokenTypeMap[RazorDirectiveColonType];
    public int RazorTagHelperAttribute => RazorTokenTypeMap[RazorTagHelperAttributeType];
    public int RazorTagHelperElement => RazorTokenTypeMap[RazorTagHelperElementType];
    public int RazorTransition => RazorTokenTypeMap[RazorTransitionType];

    public SemanticTokensLegend Legend => _lazyInitializer.Value.Legend;

    private Dictionary<string, int> RazorTokenTypeMap => _lazyInitializer.Value.TokenTypeMap;

    private readonly Lazy<(SemanticTokensLegend Legend, Dictionary<string, int> TokenTypeMap)> _lazyInitializer;

    [ImportingConstructor]
    public RazorSemanticTokensLegendService(IClientCapabilitiesService clientCapabilitiesService)
    {
        // DI calls this constructor to build the service container, but we can't access clientCapabilitiesService
        // until the language server has received the Initialize message, so we have to do this lazily.
        _lazyInitializer = new Lazy<(SemanticTokensLegend, Dictionary<string, int>)>(() =>
        {
            using var _ = ArrayBuilderPool<string>.GetPooledObject(out var builder);

            var supportsVsExtensions = clientCapabilitiesService.ClientCapabilities.SupportsVisualStudioExtensions;

            builder.AddRange(RazorSemanticTokensAccessor.GetTokenTypes(supportsVsExtensions));

            var razorTokenTypeMap = new Dictionary<string, int>();
            foreach (var razorTokenType in GetRazorSemanticTokenTypes())
            {
                razorTokenTypeMap.Add(razorTokenType, builder.Count);
                builder.Add(razorTokenType);
            }

            var legend = new SemanticTokensLegend()
            {
                TokenModifiers = s_tokenModifiers,
                TokenTypes = builder.ToArray()
            };

            return (legend, razorTokenTypeMap);
        });
    }

    private static ImmutableArray<string> GetRazorSemanticTokenTypes()
    {
        using var _ = ArrayBuilderPool<string>.GetPooledObject(out var builder);

        foreach (var field in typeof(RazorSemanticTokensLegendService).GetFields(BindingFlags.NonPublic | BindingFlags.Static))
        {
            if (field.GetValue(null) is string value)
            {
                builder.Add(value);
            }
        }

        return builder.ToImmutable();
    }

    private static readonly string[] s_tokenModifiers = RazorSemanticTokensAccessor.GetTokenModifiers().Concat(Enum.GetNames(typeof(RazorTokenModifiers))).ToArray();

    [Flags]
    public enum RazorTokenModifiers
    {
        // None = 0
        // Static, from Roslyn = 1
        // ReassignedVariable, from Roslyn = 1 << 1

        // By convention, all LSP token modifiers start with a lowercase letter

        // Must start after the last Roslyn modifier
        razorCode = 1 << 2
    }
}
