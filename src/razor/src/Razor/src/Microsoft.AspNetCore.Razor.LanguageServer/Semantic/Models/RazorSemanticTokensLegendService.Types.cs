// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

internal sealed partial class RazorSemanticTokensLegendService
{
    public class Types
    {
        private static readonly string s_markupAttributeQuoteType = "markupAttributeQuote";
        private static readonly string s_markupAttributeType = "markupAttribute";
        private static readonly string s_markupAttributeValueType = "markupAttributeValue";
        private static readonly string s_markupCommentPunctuationType = "markupCommentPunctuation";
        private static readonly string s_markupCommentType = "markupComment";
        private static readonly string s_markupElementType = "markupElement";
        private static readonly string s_markupOperatorType = "markupOperator";
        private static readonly string s_markupTagDelimiterType = "markupTagDelimiter";
        private static readonly string s_markupTextLiteralType = "markupTextLiteral";

        private static readonly string s_razorCommentStarType = "razorCommentStar";
        private static readonly string s_razorCommentTransitionType = "razorCommentTransition";
        private static readonly string s_razorCommentType = "razorComment";
        private static readonly string s_razorComponentAttributeType = "RazorComponentAttribute";
        private static readonly string s_razorComponentElementType = "razorComponentElement";
        private static readonly string s_razorDirectiveAttributeType = "razorDirectiveAttribute";
        private static readonly string s_razorDirectiveColonType = "razorDirectiveColon";
        private static readonly string s_razorDirectiveType = "razorDirective";
        private static readonly string s_razorTagHelperAttributeType = "razorTagHelperAttribute";
        private static readonly string s_razorTagHelperElementType = "razorTagHelperElement";
        private static readonly string s_razorTransitionType = "razorTransition";

        public int MarkupAttribute => _tokenTypeMap[s_markupAttributeType];
        public int MarkupAttributeQuote => _tokenTypeMap[s_markupAttributeQuoteType];
        public int MarkupAttributeValue => _tokenTypeMap[s_markupAttributeValueType];
        public int MarkupComment => _tokenTypeMap[s_markupCommentType];
        public int MarkupCommentPunctuation => _tokenTypeMap[s_markupCommentPunctuationType];
        public int MarkupElement => _tokenTypeMap[s_markupElementType];
        public int MarkupOperator => _tokenTypeMap[s_markupOperatorType];
        public int MarkupTagDelimiter => _tokenTypeMap[s_markupTagDelimiterType];
        public int MarkupTextLiteral => _tokenTypeMap[s_markupTextLiteralType];

        public int RazorComment => _tokenTypeMap[s_razorCommentType];
        public int RazorCommentStar => _tokenTypeMap[s_razorCommentStarType];
        public int RazorCommentTransition => _tokenTypeMap[s_razorCommentTransitionType];
        public int RazorComponentAttribute => _tokenTypeMap[s_razorComponentAttributeType];
        public int RazorComponentElement => _tokenTypeMap[s_razorComponentElementType];
        public int RazorDirective => _tokenTypeMap[s_razorDirectiveType];
        public int RazorDirectiveAttribute => _tokenTypeMap[s_razorDirectiveAttributeType];
        public int RazorDirectiveColon => _tokenTypeMap[s_razorDirectiveColonType];
        public int RazorTagHelperAttribute => _tokenTypeMap[s_razorTagHelperAttributeType];
        public int RazorTagHelperElement => _tokenTypeMap[s_razorTagHelperElementType];
        public int RazorTransition => _tokenTypeMap[s_razorTransitionType];

        public string[] TokenTypes { get; }

        private Dictionary<string, int> _tokenTypeMap;

        public Types(IClientCapabilitiesService clientCapabilitiesService)
        {
            using var _ = ArrayBuilderPool<string>.GetPooledObject(out var builder);

            var supportsVsExtensions = clientCapabilitiesService.ClientCapabilities.SupportsVisualStudioExtensions;

            builder.AddRange(RazorSemanticTokensAccessor.GetTokenTypes(supportsVsExtensions));

            var tokenTypeMap = new Dictionary<string, int>();
            foreach (var razorTokenType in GetStaticFieldValues(typeof(Types)))
            {
                tokenTypeMap.Add(razorTokenType, builder.Count);
                builder.Add(razorTokenType);
            }

            _tokenTypeMap = tokenTypeMap;

            TokenTypes = builder.ToArray();
        }
    }
}
