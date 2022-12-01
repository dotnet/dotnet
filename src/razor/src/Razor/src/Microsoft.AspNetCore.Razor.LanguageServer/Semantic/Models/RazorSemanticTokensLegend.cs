// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic.Models;

internal class RazorSemanticTokensLegend
{
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

    public static int MarkupAttribute => TokenTypesLegend[MarkupAttributeType];
    public static int MarkupAttributeQuote => TokenTypesLegend[MarkupAttributeQuoteType];
    public static int MarkupAttributeValue => TokenTypesLegend[MarkupAttributeValueType];
    public static int MarkupComment => TokenTypesLegend[MarkupCommentType];
    public static int MarkupCommentPunctuation => TokenTypesLegend[MarkupCommentPunctuationType];
    public static int MarkupElement => TokenTypesLegend[MarkupElementType];
    public static int MarkupOperator => TokenTypesLegend[MarkupOperatorType];
    public static int MarkupTagDelimiter => TokenTypesLegend[MarkupTagDelimiterType];
    public static int MarkupTextLiteral => TokenTypesLegend[MarkupTextLiteralType];

    public static int RazorComment => TokenTypesLegend[RazorCommentType];
    public static int RazorCommentStar => TokenTypesLegend[RazorCommentStarType];
    public static int RazorCommentTransition => TokenTypesLegend[RazorCommentTransitionType];
    public static int RazorComponentAttribute => TokenTypesLegend[RazorComponentAttributeType];
    public static int RazorComponentElement => TokenTypesLegend[RazorComponentElementType];
    public static int RazorDirective => TokenTypesLegend[RazorDirectiveType];
    public static int RazorDirectiveAttribute => TokenTypesLegend[RazorDirectiveAttributeType];
    public static int RazorDirectiveColon => TokenTypesLegend[RazorDirectiveColonType];
    public static int RazorTagHelperAttribute => TokenTypesLegend[RazorTagHelperAttributeType];
    public static int RazorTagHelperElement => TokenTypesLegend[RazorTagHelperElementType];
    public static int RazorTransition => TokenTypesLegend[RazorTransitionType];

    public static int CSharpKeyword => TokenTypesLegend["keyword"];
    public static int CSharpOperator => TokenTypesLegend["operator"];
    public static int CSharpPunctuation => TokenTypesLegend["punctuation"];
    public static int CSharpString => TokenTypesLegend["string"];
    public static int CSharpVariable => TokenTypesLegend["variable"];

    // C# types + Razor types
    public static readonly ImmutableArray<string> TokenTypes = GetTokenTypes();

    private static ImmutableArray<string> GetTokenTypes()
    {
        var builder = ImmutableArray.CreateBuilder<string>();

        builder.AddRange(RazorSemanticTokensAccessor.RoslynTokenTypes);

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
        // Razor
        "None",
        // C# Modifiers
        "static",
    };

    public static readonly IReadOnlyDictionary<string, int> TokenTypesLegend = GetMap(TokenTypes);

    public static readonly SemanticTokensLegend Instance = new()
    {
        TokenModifiers = s_tokenModifiers,
        TokenTypes = TokenTypes.ToArray(),
    };

    private static IReadOnlyDictionary<string, int> GetMap(IReadOnlyList<string> tokens)
    {
        var result = new Dictionary<string, int>();
        for (var i = 0; i < tokens.Count; i++)
        {
            result[tokens[i]] = i;
        }

        return result;
    }
}
