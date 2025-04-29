// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.Language.Syntax;

namespace Microsoft.AspNetCore.Razor.Language.Legacy;

internal static class TagHelperBlockRewriter
{
    public static TagMode GetTagMode(
        MarkupStartTagSyntax startTag,
        MarkupEndTagSyntax endTag,
        TagHelperBinding bindingResult)
    {
        var childSpan = startTag.GetLastToken()?.Parent;

        // Self-closing tags are always valid despite descriptors[X].TagStructure.
        if (childSpan?.GetContent().EndsWith("/>", StringComparison.Ordinal) ?? false)
        {
            return TagMode.SelfClosing;
        }

        var hasDirectiveAttribute = false;
        foreach (var descriptor in bindingResult.Descriptors)
        {
            var boundRules = bindingResult.Mappings[descriptor];
            var nonDefaultRule = boundRules.FirstOrDefault(static rule => rule.TagStructure != TagStructure.Unspecified);

            if (nonDefaultRule?.TagStructure == TagStructure.WithoutEndTag)
            {
                return TagMode.StartTagOnly;
            }

            // Directive attribute will tolerate forms that don't work for tag helpers. For instance:
            //
            // <input @onclick="..."> vs <input onclick="..." />
            //
            // We don't want this to become an error just because you added a directive attribute.
            if (descriptor.IsAnyComponentDocumentTagHelper() && !descriptor.IsComponentOrChildContentTagHelper)
            {
                hasDirectiveAttribute = true;
            }
        }

        if (hasDirectiveAttribute && startTag.IsVoidElement() && endTag == null)
        {
            return TagMode.StartTagOnly;
        }

        return TagMode.StartTagAndEndTag;
    }

    public static MarkupTagHelperStartTagSyntax Rewrite(
        string tagName,
        RazorParserOptions options,
        MarkupStartTagSyntax startTag,
        TagHelperBinding bindingResult,
        ErrorSink errorSink,
        RazorSourceDocument source)
    {
        var processedBoundAttributeNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var attributes = startTag.Attributes;
        using var _ = SyntaxListBuilderPool.GetPooledBuilder<RazorSyntaxNode>(out var attributeBuilder);

        for (var i = 0; i < startTag.Attributes.Count; i++)
        {
            var isMinimized = false;
            var attributeNameLocation = SourceLocation.Undefined;
            var child = startTag.Attributes[i];
            TryParseResult result;
            if (child is MarkupAttributeBlockSyntax attributeBlock)
            {
                attributeNameLocation = attributeBlock.Name.GetSourceLocation(source);
                result = TryParseAttribute(
                    tagName,
                    attributeBlock,
                    bindingResult.Descriptors,
                    errorSink,
                    processedBoundAttributeNames,
                    options);
                attributeBuilder.Add(result.RewrittenAttribute);
            }
            else if (child is MarkupMinimizedAttributeBlockSyntax minimizedAttributeBlock)
            {
                isMinimized = true;
                attributeNameLocation = minimizedAttributeBlock.Name.GetSourceLocation(source);
                result = TryParseMinimizedAttribute(
                    tagName,
                    minimizedAttributeBlock,
                    bindingResult.Descriptors,
                    errorSink,
                    processedBoundAttributeNames);
                attributeBuilder.Add(result.RewrittenAttribute);
            }
            else if (child is MarkupMiscAttributeContentSyntax miscContent)
            {
                foreach (var contentChild in miscContent.Children)
                {
                    if (contentChild is CSharpCodeBlockSyntax codeBlock)
                    {
                        // TODO: Accept more than just Markup attributes: https://github.com/aspnet/Razor/issues/96.
                        // Something like:
                        // <input @checked />
                        var location = new SourceSpan(codeBlock.GetSourceLocation(source), codeBlock.FullWidth);
                        var diagnostic = RazorDiagnosticFactory.CreateParsing_TagHelpersCannotHaveCSharpInTagDeclaration(location, tagName);
                        errorSink.OnError(diagnostic);
                        break;
                    }
                    else
                    {
                        // If the original span content was whitespace it ultimately means the tag
                        // that owns this "attribute" is malformed and is expecting a user to type a new attribute.
                        // ex: <myTH class="btn"| |
                        var literalContent = contentChild.GetContent();
                        if (!string.IsNullOrWhiteSpace(literalContent))
                        {
                            var location = contentChild.GetSourceSpan(source);
                            var diagnostic = RazorDiagnosticFactory.CreateParsing_TagHelperAttributeListMustBeWellFormed(location);
                            errorSink.OnError(diagnostic);
                            break;
                        }
                    }
                }

                result = null;
            }
            else
            {
                result = null;
            }

            // Only want to track the attribute if we succeeded in parsing its corresponding Block/Span.
            if (result == null)
            {
                // Error occurred while parsing the attribute. Don't try parsing the rest to avoid misleading errors.
                for (var j = i; j < startTag.Attributes.Count; j++)
                {
                    attributeBuilder.Add(startTag.Attributes[j]);
                }

                break;
            }

            // Check if it's a non-boolean bound attribute that is minimized or if it's a bound
            // non-string attribute that has null or whitespace content.
            var isValidMinimizedAttribute = options.AllowMinimizedBooleanTagHelperAttributes && result.IsBoundBooleanAttribute;
            if ((isMinimized &&
                result.IsBoundAttribute &&
                !isValidMinimizedAttribute) ||
                (!isMinimized &&
                result.IsBoundNonStringAttribute &&
                 string.IsNullOrWhiteSpace(GetAttributeValueContent(result.RewrittenAttribute))))
            {
                var errorLocation = new SourceSpan(attributeNameLocation, result.AttributeName.Length);
                var propertyTypeName = GetPropertyType(result.AttributeName, bindingResult.Descriptors);
                var diagnostic = RazorDiagnosticFactory.CreateTagHelper_EmptyBoundAttribute(errorLocation, result.AttributeName, tagName, propertyTypeName);
                errorSink.OnError(diagnostic);
            }

            // Check if the attribute was a prefix match for a tag helper dictionary property but the
            // dictionary key would be the empty string.
            if (result.IsMissingDictionaryKey)
            {
                var errorLocation = new SourceSpan(attributeNameLocation, result.AttributeName.Length);
                var diagnostic = RazorDiagnosticFactory.CreateParsing_TagHelperIndexerAttributeNameMustIncludeKey(errorLocation, result.AttributeName, tagName);
                errorSink.OnError(diagnostic);
            }
        }

        if (attributeBuilder.Count > 0)
        {
            // This means we rewrote something. Use the new set of attributes.
            attributes = attributeBuilder.ToList();
        }

        var tagHelperStartTag = SyntaxFactory.MarkupTagHelperStartTag(
            startTag.OpenAngle, startTag.Bang, startTag.Name, attributes, startTag.ForwardSlash, startTag.CloseAngle, startTag.ChunkGenerator);

        return tagHelperStartTag.WithEditHandler(startTag.GetEditHandler());
    }

    private static TryParseResult TryParseMinimizedAttribute(
        string tagName,
        MarkupMinimizedAttributeBlockSyntax attributeBlock,
        IEnumerable<TagHelperDescriptor> descriptors,
        ErrorSink errorSink,
        HashSet<string> processedBoundAttributeNames)
    {
        // Have a name now. Able to determine correct isBoundNonStringAttribute value.
        var result = CreateTryParseResult(attributeBlock.Name.GetContent(), descriptors, processedBoundAttributeNames);

        result.AttributeStructure = AttributeStructure.Minimized;

        if (result.IsDirectiveAttribute)
        {
            // Directive attributes have a different syntax.
            result.RewrittenAttribute = RewriteToMinimizedDirectiveAttribute(attributeBlock, result);

            return result;
        }
        else
        {
            var rewritten = SyntaxFactory.MarkupMinimizedTagHelperAttribute(
                attributeBlock.NamePrefix,
                attributeBlock.Name);

            rewritten = rewritten.WithTagHelperAttributeInfo(
                    new TagHelperAttributeInfo(result.AttributeName, parameterName: null, result.AttributeStructure, result.IsBoundAttribute, isDirectiveAttribute: false));

            result.RewrittenAttribute = rewritten;

            return result;
        }
    }

    private static TryParseResult TryParseAttribute(
        string tagName,
        MarkupAttributeBlockSyntax attributeBlock,
        IEnumerable<TagHelperDescriptor> descriptors,
        ErrorSink errorSink,
        HashSet<string> processedBoundAttributeNames,
        RazorParserOptions options)
    {
        // Have a name now. Able to determine correct isBoundNonStringAttribute value.
        var result = CreateTryParseResult(attributeBlock.Name.GetContent(), descriptors, processedBoundAttributeNames);

        if (attributeBlock.ValuePrefix == null)
        {
            // We are purposefully not persisting NoQuotes even for unbound attributes because it is still possible to
            // rewrite the values that introduces a space like in UrlResolutionTagHelper.
            // The other case is it could be an expression, treat NoQuotes and DoubleQuotes equivalently. We purposefully do not persist NoQuotes
            // ValueStyles at code generation time to protect users from rendering dynamic content with spaces
            // that can break attributes.
            // Ex: <tag my-attribute=@value /> where @value results in the test "hello world".
            // This way, the above code would render <tag my-attribute="hello world" />.
            result.AttributeStructure = AttributeStructure.DoubleQuotes;
        }
        else
        {
            var lastToken = attributeBlock.ValuePrefix.GetLastToken();
            switch (lastToken.Kind)
            {
                case SyntaxKind.DoubleQuote:
                    result.AttributeStructure = AttributeStructure.DoubleQuotes;
                    break;
                case SyntaxKind.SingleQuote:
                    result.AttributeStructure = AttributeStructure.SingleQuotes;
                    break;
                default:
                    result.AttributeStructure = AttributeStructure.Minimized;
                    break;
            }
        }

        var attributeValue = attributeBlock.Value;
        if (attributeValue == null)
        {
            using var _ = SyntaxListBuilderPool.GetPooledBuilder<RazorSyntaxNode>(out var builder);

            // Add a marker for attribute value when there are no quotes like, <p class= >
            builder.Add(SyntaxFactory.MarkupTextLiteral(new SyntaxList<SyntaxToken>(), chunkGenerator: null));

            attributeValue = SyntaxFactory.GenericBlock(builder.ToList());
        }
        var rewrittenValue = RewriteAttributeValue(result, attributeValue, options);

        if (result.IsDirectiveAttribute)
        {
            // Directive attributes have a different syntax.
            result.RewrittenAttribute = RewriteToDirectiveAttribute(attributeBlock, result, rewrittenValue);

            return result;
        }
        else
        {
            var rewritten = SyntaxFactory.MarkupTagHelperAttribute(
                attributeBlock.NamePrefix,
                attributeBlock.Name,
                attributeBlock.NameSuffix,
                attributeBlock.EqualsToken,
                attributeBlock.ValuePrefix,
                rewrittenValue,
                attributeBlock.ValueSuffix);

            rewritten = rewritten.WithTagHelperAttributeInfo(
                new TagHelperAttributeInfo(result.AttributeName, parameterName: null, result.AttributeStructure, result.IsBoundAttribute, isDirectiveAttribute: false));

            result.RewrittenAttribute = rewritten;

            return result;
        }
    }

    private static MarkupTagHelperDirectiveAttributeSyntax RewriteToDirectiveAttribute(
        MarkupAttributeBlockSyntax attributeBlock,
        TryParseResult result,
        MarkupTagHelperAttributeValueSyntax rewrittenValue)
    {
        //
        // Consider, <Foo @bind:param="..." />
        // We're now going to rewrite @bind:param from a regular MarkupAttributeBlock to a MarkupTagHelperDirectiveAttribute.
        // We need to split the name "@bind:param" into four parts,
        // @ - Transition (MetaCode)
        // bind - Name (Text)
        // : - Colon (MetaCode)
        // param - ParameterName (Text)
        //
        var attributeName = result.AttributeName;
        var attributeNameSyntax = attributeBlock.Name;
        var transition = SyntaxFactory.RazorMetaCode(
            new SyntaxList<SyntaxToken>(SyntaxFactory.MissingToken(SyntaxKind.Transition)), chunkGenerator: null);
        RazorMetaCodeSyntax colon = null;
        MarkupTextLiteralSyntax parameterName = null;
        if (attributeName.StartsWith("@", StringComparison.Ordinal))
        {
            attributeName = attributeName.Substring(1);
            var attributeNameToken = SyntaxFactory.Token(SyntaxKind.Text, attributeName);
            attributeNameSyntax = SyntaxFactory.MarkupTextLiteral(chunkGenerator: null).AddLiteralTokens(attributeNameToken);

            var transitionToken = SyntaxFactory.Token(SyntaxKind.Transition, "@");
            transition = SyntaxFactory.RazorMetaCode(new SyntaxList<SyntaxToken>(transitionToken), chunkGenerator: null);
        }

        if (attributeName.IndexOf(':') != -1)
        {
            var segments = attributeName.Split(new[] { ':' }, 2);

            var attributeNameToken = SyntaxFactory.Token(SyntaxKind.Text, segments[0]);
            attributeNameSyntax = SyntaxFactory.MarkupTextLiteral(chunkGenerator: null).AddLiteralTokens(attributeNameToken);

            var colonToken = SyntaxFactory.Token(SyntaxKind.Colon, ":");
            colon = SyntaxFactory.RazorMetaCode(new SyntaxList<SyntaxToken>(colonToken), chunkGenerator: null);

            var parameterNameToken = SyntaxFactory.Token(SyntaxKind.Text, segments[1]);
            parameterName = SyntaxFactory.MarkupTextLiteral(chunkGenerator: null).AddLiteralTokens(parameterNameToken);
        }

        var rewritten = SyntaxFactory.MarkupTagHelperDirectiveAttribute(
            attributeBlock.NamePrefix,
            transition,
            attributeNameSyntax,
            colon,
            parameterName,
            attributeBlock.NameSuffix,
            attributeBlock.EqualsToken,
            attributeBlock.ValuePrefix,
            rewrittenValue,
            attributeBlock.ValueSuffix);

        rewritten = rewritten.WithTagHelperAttributeInfo(
            new TagHelperAttributeInfo(result.AttributeName, parameterName?.GetContent(), result.AttributeStructure, result.IsBoundAttribute, isDirectiveAttribute: true));

        return rewritten;
    }

    private static MarkupMinimizedTagHelperDirectiveAttributeSyntax RewriteToMinimizedDirectiveAttribute(
        MarkupMinimizedAttributeBlockSyntax attributeBlock,
        TryParseResult result)
    {
        //
        // Consider, <Foo @bind:param />
        // We're now going to rewrite @bind:param from a regular MarkupAttributeBlock to a MarkupTagHelperDirectiveAttribute.
        // We need to split the name "@bind:param" into four parts,
        // @ - Transition (MetaCode)
        // bind - Name (Text)
        // : - Colon (MetaCode)
        // param - ParameterName (Text)
        //
        var attributeName = result.AttributeName;
        var attributeNameSyntax = attributeBlock.Name;
        var transition = SyntaxFactory.RazorMetaCode(
            new SyntaxList<SyntaxToken>(SyntaxFactory.MissingToken(SyntaxKind.Transition)), chunkGenerator: null);
        RazorMetaCodeSyntax colon = null;
        MarkupTextLiteralSyntax parameterName = null;
        if (attributeName.StartsWith("@", StringComparison.Ordinal))
        {
            attributeName = attributeName.Substring(1);
            var attributeNameToken = SyntaxFactory.Token(SyntaxKind.Text, attributeName);
            attributeNameSyntax = SyntaxFactory.MarkupTextLiteral(chunkGenerator: null).AddLiteralTokens(attributeNameToken);

            var transitionToken = SyntaxFactory.Token(SyntaxKind.Transition, "@");
            transition = SyntaxFactory.RazorMetaCode(new SyntaxList<SyntaxToken>(transitionToken), chunkGenerator: null);
        }

        if (attributeName.IndexOf(':') != -1)
        {
            var segments = attributeName.Split(new[] { ':' }, 2);

            var attributeNameToken = SyntaxFactory.Token(SyntaxKind.Text, segments[0]);
            attributeNameSyntax = SyntaxFactory.MarkupTextLiteral(chunkGenerator: null).AddLiteralTokens(attributeNameToken);

            var colonToken = SyntaxFactory.Token(SyntaxKind.Colon, ":");
            colon = SyntaxFactory.RazorMetaCode(new SyntaxList<SyntaxToken>(colonToken), chunkGenerator: null);

            var parameterNameToken = SyntaxFactory.Token(SyntaxKind.Text, segments[1]);
            parameterName = SyntaxFactory.MarkupTextLiteral(chunkGenerator: null).AddLiteralTokens(parameterNameToken);
        }

        var rewritten = SyntaxFactory.MarkupMinimizedTagHelperDirectiveAttribute(
            attributeBlock.NamePrefix,
            transition,
            attributeNameSyntax,
            colon,
            parameterName);

        rewritten = rewritten.WithTagHelperAttributeInfo(
            new TagHelperAttributeInfo(result.AttributeName, parameterName?.GetContent(), result.AttributeStructure, result.IsBoundAttribute, isDirectiveAttribute: true));

        return rewritten;
    }

    private static MarkupTagHelperAttributeValueSyntax RewriteAttributeValue(TryParseResult result, RazorBlockSyntax attributeValue, RazorParserOptions options)
    {
        var rewriter = new AttributeValueRewriter(result, options);
        var rewrittenValue = attributeValue;
        if (result.IsBoundAttribute)
        {
            // If the attribute was requested by a tag helper but the corresponding property was not a
            // string, then treat its value as code. A non-string value can be any C# value so we need
            // to ensure the tree reflects that.
            rewrittenValue = (RazorBlockSyntax)rewriter.Visit(attributeValue);
        }

        return SyntaxFactory.MarkupTagHelperAttributeValue(rewrittenValue.Children);
    }

    // Determines the full name of the Type of the property corresponding to an attribute with the given name.
    private static string GetPropertyType(string name, IEnumerable<TagHelperDescriptor> descriptors)
    {
        foreach (var descriptor in descriptors)
        {
            if (TagHelperMatchingConventions.TryGetFirstBoundAttributeMatch(descriptor, name, out var firstBoundAttribute, out var indexerMatch, out var _, out var _))
            {
                if (indexerMatch)
                {
                    return firstBoundAttribute.IndexerTypeName;
                }
                else
                {
                    return firstBoundAttribute.TypeName;
                }
            }
        }

        return null;
    }

    // Create a TryParseResult for given name, filling in binding details.
    private static TryParseResult CreateTryParseResult(
        string name,
        IEnumerable<TagHelperDescriptor> descriptors,
        HashSet<string> processedBoundAttributeNames)
    {
        var isBoundAttribute = false;
        var isBoundNonStringAttribute = false;
        var isBoundBooleanAttribute = false;
        var isMissingDictionaryKey = false;
        var isDirectiveAttribute = false;

        foreach (var descriptor in descriptors)
        {
            if (TagHelperMatchingConventions.TryGetFirstBoundAttributeMatch(
                descriptor,
                name,
                out var firstBoundAttribute,
                out var indexerMatch,
                out var parameterMatch,
                out var boundAttributeParameter))
            {
                isBoundAttribute = true;
                if (parameterMatch)
                {
                    isBoundNonStringAttribute = !boundAttributeParameter.IsStringProperty;
                    isBoundBooleanAttribute = boundAttributeParameter.IsBooleanProperty;
                    isMissingDictionaryKey = false;
                }
                else
                {
                    isBoundNonStringAttribute = !firstBoundAttribute.ExpectsStringValue(name);
                    isBoundBooleanAttribute = firstBoundAttribute.ExpectsBooleanValue(name);
                    isMissingDictionaryKey = firstBoundAttribute.IndexerNamePrefix != null &&
                        name.Length == firstBoundAttribute.IndexerNamePrefix.Length;
                }

                isDirectiveAttribute = firstBoundAttribute.IsDirectiveAttribute;

                break;
            }
        }

        var isDuplicateAttribute = false;
        if (isBoundAttribute && !processedBoundAttributeNames.Add(name))
        {
            // A bound attribute with the same name has already been processed.
            isDuplicateAttribute = true;
        }

        return new TryParseResult
        {
            AttributeName = name,
            IsBoundAttribute = isBoundAttribute,
            IsBoundNonStringAttribute = isBoundNonStringAttribute,
            IsBoundBooleanAttribute = isBoundBooleanAttribute,
            IsMissingDictionaryKey = isMissingDictionaryKey,
            IsDuplicateAttribute = isDuplicateAttribute,
            IsDirectiveAttribute = isDirectiveAttribute
        };
    }

    private static string GetAttributeValueContent(RazorSyntaxNode attributeBlock)
    {
        if (attributeBlock is MarkupTagHelperAttributeSyntax tagHelperAttribute)
        {
            return tagHelperAttribute.Value?.GetContent();
        }
        else if (attributeBlock is MarkupTagHelperDirectiveAttributeSyntax directiveAttribute)
        {
            return directiveAttribute.Value?.GetContent();
        }
        else if (attributeBlock is MarkupAttributeBlockSyntax attribute)
        {
            return attribute.Value?.GetContent();
        }

        return null;
    }

    private class AttributeValueRewriter : SyntaxRewriter
    {
        private readonly TryParseResult _tryParseResult;
        private bool _rewriteAsMarkup;
        private readonly RazorParserOptions _options;

        public AttributeValueRewriter(TryParseResult result, RazorParserOptions options)
        {
            _tryParseResult = result;
            _options = options;
        }

        public override SyntaxNode VisitGenericBlock(GenericBlockSyntax node)
        {
            if (_tryParseResult.IsBoundNonStringAttribute && CanBeCollapsed(node))
            {
                var tokens = node.GetTokens();

                if (tokens is [{ Kind: SyntaxKind.Transition } transition, ..])
                {
                    // Lift the transition token so it is not part of the generated output.
                    tokens = tokens.RemoveAt(0);
                    var children = createExpressionLiteral(tokens);
                    return SyntaxFactory.MarkupBlock(
                        new SyntaxList<RazorSyntaxNode>(SyntaxFactory.CSharpCodeBlock(
                            new SyntaxList<RazorSyntaxNode>(SyntaxFactory.CSharpImplicitExpression(
                                SyntaxFactory.CSharpTransition(transition, chunkGenerator: null),
                                SyntaxFactory.CSharpImplicitExpressionBody(
                                    SyntaxFactory.CSharpCodeBlock(children)))))));
                }

                return node.Update(createExpressionLiteral(tokens));
            }

            return base.VisitGenericBlock(node);

            SyntaxList<RazorSyntaxNode> createExpressionLiteral(SyntaxList<SyntaxToken> tokens)
            {
                var expression = SyntaxFactory.CSharpExpressionLiteral(tokens, chunkGenerator: null);
                var rewrittenExpression = (CSharpExpressionLiteralSyntax)VisitCSharpExpressionLiteral(expression);
                return new SyntaxList<RazorSyntaxNode>(rewrittenExpression);
            }
        }

        public override SyntaxNode VisitCSharpTransition(CSharpTransitionSyntax node)
        {
            if (!_tryParseResult.IsBoundNonStringAttribute)
            {
                return base.VisitCSharpTransition(node);
            }

            // For bound non-string attributes, we'll only allow a transition span to appear at the very
            // beginning of the attribute expression. All later transitions would appear as code so that
            // they are part of the generated output. E.g.
            // key="@value" -> MyTagHelper.key = value
            // key=" @value" -> MyTagHelper.key =  @value
            // key="1 + @case" -> MyTagHelper.key = 1 + @case
            // key="@int + @case" -> MyTagHelper.key = int + @case
            // key="@(a + b) -> MyTagHelper.key = a + b
            // key="4 + @(a + b)" -> MyTagHelper.key = 4 + @(a + b)
            if (_rewriteAsMarkup)
            {
                // Change to a MarkupChunkGenerator so that the '@' \ parenthesis is generated as part of the output.
                var expression = SyntaxFactory.CSharpExpressionLiteral(new SyntaxList<SyntaxToken>(node.Transition), MarkupChunkGenerator.Instance).WithEditHandler(node.GetEditHandler());

                return base.VisitCSharpExpressionLiteral(expression);
            }

            _rewriteAsMarkup = true;
            return base.VisitCSharpTransition(node);
        }

        public override SyntaxNode VisitCSharpImplicitExpression(CSharpImplicitExpressionSyntax node)
        {
            if (_rewriteAsMarkup)
            {
                using var _ = SyntaxListBuilderPool.GetPooledBuilder<RazorSyntaxNode>(out var builder);

                var rewrittenBody = (CSharpCodeBlockSyntax)VisitCSharpCodeBlock(((CSharpImplicitExpressionBodySyntax)node.Body).CSharpCode);

                // The transition needs to be considered part of the first token in the rewritten body, so we update the first child to include it.
                // This ensures that, when we create tracking spans and write out the final C# code, the `@` is not treated as a separate, separable
                // token from the content that follows it.
                var firstChild = rewrittenBody.Children[0];
                var firstToken = firstChild.GetFirstToken();
                var newFirstToken = SyntaxFactory.Token(firstToken.Kind, node.Transition.Transition.Content + firstToken.Content).WithAnnotations(firstToken.GetAnnotations());

                var newFirstChild = firstChild.ReplaceNode(firstToken, newFirstToken);
                builder.AddRange(rewrittenBody.Children.Replace(firstChild, newFirstChild));

                // Since the original transition is part of the body, we need something to take it's place.
                var transition = SyntaxFactory.CSharpTransition(SyntaxFactory.MissingToken(SyntaxKind.Transition), chunkGenerator: null);

                var rewrittenCodeBlock = SyntaxFactory.CSharpCodeBlock(builder.ToList());
                return SyntaxFactory.CSharpImplicitExpression(transition, SyntaxFactory.CSharpImplicitExpressionBody(rewrittenCodeBlock));
            }

            return base.VisitCSharpImplicitExpression(node);
        }

        public override SyntaxNode VisitCSharpExplicitExpression(CSharpExplicitExpressionSyntax node)
        {
            CSharpTransitionSyntax transition = null;
            using var _ = SyntaxListBuilderPool.GetPooledBuilder<RazorSyntaxNode>(out var builder);

            if (_rewriteAsMarkup)
            {
                // Convert transition.
                // Change to a MarkupChunkGenerator so that the '@' \ parenthesis is generated as part of the output.
                // This is bad code, since @( is never valid C#, so we don't worry about trying to stitch the @ and the ( together.
                var editHandler = _options.EnableSpanEditHandlers
                    ? node.GetEditHandler() ?? SpanEditHandler.CreateDefault((content) => Enumerable.Empty<Syntax.InternalSyntax.SyntaxToken>(), AcceptedCharactersInternal.Any)
                    : null;

                var expression = SyntaxFactory.CSharpExpressionLiteral(new SyntaxList<SyntaxToken>(node.Transition.Transition), MarkupChunkGenerator.Instance).WithEditHandler(editHandler);
                expression = (CSharpExpressionLiteralSyntax)VisitCSharpExpressionLiteral(expression);
                builder.Add(expression);

                // Since the original transition is part of the body, we need something to take it's place.
                transition = SyntaxFactory.CSharpTransition(SyntaxFactory.MissingToken(SyntaxKind.Transition), chunkGenerator: null);

                var body = (CSharpExplicitExpressionBodySyntax)node.Body;
                var rewrittenOpenParen = (RazorSyntaxNode)VisitRazorMetaCode(body.OpenParen);
                var rewrittenBody = (CSharpCodeBlockSyntax)VisitCSharpCodeBlock(body.CSharpCode);
                var rewrittenCloseParen = (RazorSyntaxNode)VisitRazorMetaCode(body.CloseParen);
                builder.Add(rewrittenOpenParen);
                builder.AddRange(rewrittenBody.Children);
                builder.Add(rewrittenCloseParen);
            }
            else
            {
                // This is the first expression of a non-string attribute like attr=@(a + b)
                // Below code converts this to an implicit expression to make the parens
                // part of the expression so that it is rendered.
                transition = (CSharpTransitionSyntax)Visit(node.Transition);
                var body = (CSharpExplicitExpressionBodySyntax)node.Body;
                var rewrittenOpenParen = (RazorSyntaxNode)VisitRazorMetaCode(body.OpenParen);
                var rewrittenBody = (CSharpCodeBlockSyntax)VisitCSharpCodeBlock(body.CSharpCode);
                var rewrittenCloseParen = (RazorSyntaxNode)VisitRazorMetaCode(body.CloseParen);
                builder.Add(rewrittenOpenParen);
                builder.AddRange(rewrittenBody.Children);
                builder.Add(rewrittenCloseParen);
            }

            var rewrittenCodeBlock = SyntaxFactory.CSharpCodeBlock(builder.ToList());
            return SyntaxFactory.CSharpImplicitExpression(transition, SyntaxFactory.CSharpImplicitExpressionBody(rewrittenCodeBlock));
        }

        public override SyntaxNode VisitRazorMetaCode(RazorMetaCodeSyntax node)
        {
            if (!_tryParseResult.IsBoundNonStringAttribute)
            {
                return base.VisitRazorMetaCode(node);
            }

            if (_rewriteAsMarkup)
            {
                // Change to a MarkupChunkGenerator so that the '@' \ parenthesis is generated as part of the output.
                var expression = SyntaxFactory.CSharpExpressionLiteral(new SyntaxList<SyntaxToken>(node.MetaCode), MarkupChunkGenerator.Instance).WithEditHandler(node.GetEditHandler());

                return VisitCSharpExpressionLiteral(expression);
            }

            _rewriteAsMarkup = true;
            return base.VisitRazorMetaCode(node);
        }

        public override SyntaxNode VisitCSharpStatement(CSharpStatementSyntax node)
        {
            // We don't support code blocks inside tag helper attributes. Don't rewrite anything inside a code block.
            // E.g, <p age="@{1 + 2}"> is not supported.
            return node;
        }

        public override SyntaxNode VisitRazorDirective(RazorDirectiveSyntax node)
        {
            // We don't support directives inside tag helper attributes. Don't rewrite anything inside a directive.
            // E.g, <p age="@functions { }"> is not supported.
            return node;
        }

        public override SyntaxNode VisitMarkupElement(MarkupElementSyntax node)
        {
            // We're visiting an attribute value. If we encounter a MarkupElement this means the attribute value is invalid.
            // We don't want to rewrite anything here.
            // E.g, <my age="@if (true) { <my4 age=... }"></my4>
            return node;
        }

        public override SyntaxNode VisitCSharpExpressionLiteral(CSharpExpressionLiteralSyntax node)
        {
            if (!_tryParseResult.IsBoundNonStringAttribute)
            {
                return base.VisitCSharpExpressionLiteral(node);
            }

            node = (CSharpExpressionLiteralSyntax)ConfigureNonStringAttribute(node);

            _rewriteAsMarkup = true;
            return base.VisitCSharpExpressionLiteral(node);
        }

        public override SyntaxNode VisitMarkupLiteralAttributeValue(MarkupLiteralAttributeValueSyntax node)
        {
            using var _ = SyntaxListBuilderPool.GetPooledBuilder<SyntaxToken>(out var builder);

            if (node.Prefix != null)
            {
                builder.AddRange(node.Prefix.LiteralTokens);
            }

            if (node.Value != null)
            {
                builder.AddRange(node.Value.LiteralTokens);
            }

            if (_tryParseResult.IsBoundNonStringAttribute)
            {
                _rewriteAsMarkup = true;
                // Since this is a bound non-string attribute, we want to convert LiteralAttributeValue to just be a CSharp Expression literal.
                var expression = SyntaxFactory.CSharpExpressionLiteral(builder.ToList(), chunkGenerator: null);
                return VisitCSharpExpressionLiteral(expression);
            }
            else
            {
                var literal = SyntaxFactory.MarkupTextLiteral(builder.ToList(), node.Value?.ChunkGenerator);
                var editHandler = node.Value?.GetEditHandler();
                literal = literal.WithEditHandler(editHandler);

                return Visit(literal);
            }
        }

        public override SyntaxNode VisitMarkupDynamicAttributeValue(MarkupDynamicAttributeValueSyntax node)
        {
            // Move the prefix to be part of the actual value.
            using var _ = SyntaxListBuilderPool.GetPooledBuilder<RazorSyntaxNode>(out var builder);

            if (node.Prefix != null)
            {
                builder.Add(node.Prefix);
            }

            if (node.Value?.Children != null)
            {
                builder.AddRange(node.Value.Children);
            }

            var rewrittenValue = SyntaxFactory.MarkupBlock(builder.ToList());

            return base.VisitMarkupBlock(rewrittenValue);
        }

        public override SyntaxNode VisitCSharpStatementLiteral(CSharpStatementLiteralSyntax node)
        {
            if (!_tryParseResult.IsBoundNonStringAttribute)
            {
                return base.VisitCSharpStatementLiteral(node);
            }

            _rewriteAsMarkup = true;
            return base.VisitCSharpStatementLiteral(node);
        }

        public override SyntaxNode VisitMarkupTextLiteral(MarkupTextLiteralSyntax node)
        {
            if (!_tryParseResult.IsBoundNonStringAttribute)
            {
                return base.VisitMarkupTextLiteral(node);
            }

            _rewriteAsMarkup = true;
            node = (MarkupTextLiteralSyntax)ConfigureNonStringAttribute(node);
            var tokens = new SyntaxList<SyntaxToken>(node.LiteralTokens);
            var value = SyntaxFactory.CSharpExpressionLiteral(tokens, node.ChunkGenerator);
            return value.WithEditHandler(node.GetEditHandler());
        }

        public override SyntaxNode VisitMarkupEphemeralTextLiteral(MarkupEphemeralTextLiteralSyntax node)
        {
            if (!_tryParseResult.IsBoundNonStringAttribute)
            {
                return base.VisitMarkupEphemeralTextLiteral(node);
            }

            // Since this is a non-string attribute we need to rewrite this as code.
            // Rewriting it to CSharpEphemeralTextLiteral so that it is not rendered to output.
            _rewriteAsMarkup = true;
            node = (MarkupEphemeralTextLiteralSyntax)ConfigureNonStringAttribute(node);
            var tokens = new SyntaxList<SyntaxToken>(node.LiteralTokens);
            var value = SyntaxFactory.CSharpEphemeralTextLiteral(tokens, node.ChunkGenerator);
            return value.WithEditHandler(node.GetEditHandler());
        }

        // Being collapsed represents that a block contains several identical looking markup literal attribute values. This can be the case
        // when a user has written something like: @onclick="() => SomeMethod()"
        // In that case there would be 3 children:
        //   - ()
        //   -  =>
        //   -  SomeMethod()
        // There are 3 children because the Razor parser separates attribute values based on whitespace.
        private bool CanBeCollapsed(GenericBlockSyntax node)
        {
            if (node.Children.Count <= 1)
            {
                // The node is either already collapsed or has no children.
                return false;
            }

            for (var i = 0; i < node.Children.Count; i++)
            {
                var kind = node.Children[i].Kind;
                if (kind != SyntaxKind.MarkupLiteralAttributeValue &&
                    // We only want to collapse dynamic values if we're in a legacy file.
                    // Mixed C#/HTML content is not allowed in components.
                    (kind != SyntaxKind.MarkupDynamicAttributeValue || !FileKinds.IsLegacy(_options.FileKind)))
                {
                    return false;
                }
            }

            return true;
        }

        private SyntaxNode ConfigureNonStringAttribute(SyntaxNode node)
        {
            var context = node.GetEditHandler();
            var builder = _options.EnableSpanEditHandlers
                ? new SpanEditHandlerBuilder(defaultLanguageTokenizer: null)
                {
                    Tokenizer = context?.Tokenizer,
                    Factory = (acceptedCharacters, tokenizer) => new ImplicitExpressionEditHandler
                    {
                        Tokenizer = tokenizer,
                        AcceptedCharacters = acceptedCharacters,
                        AcceptTrailingDot = true,
                        Keywords = CSharpCodeParser.DefaultKeywords
                    }
                }
                : null;

            var originalGenerator = node.GetChunkGenerator();
            var newGenerator = originalGenerator ?? SpanChunkGenerator.Null;
            if (!_tryParseResult.IsDuplicateAttribute && originalGenerator != null && originalGenerator != SpanChunkGenerator.Null)
            {
                // We want to mark the value of non-string bound attributes to be CSharp.
                // Except in two cases,
                // 1. Cases when we don't want to render the span. Eg: Transition span '@'.
                // 2. Cases when it is a duplicate of a bound attribute. This should just be rendered as html.

                newGenerator = new ExpressionChunkGenerator();
            }

            if (originalGenerator != newGenerator)
            {
                node = node switch
                {
                    MarkupStartTagSyntax start => start.Update(start.OpenAngle, start.Bang, start.Name, start.Attributes, start.ForwardSlash, start.CloseAngle, newGenerator),
                    MarkupEndTagSyntax end => end.Update(end.OpenAngle, end.ForwardSlash, end.Bang, end.Name, end.MiscAttributeContent, end.CloseAngle, newGenerator),
                    MarkupEphemeralTextLiteralSyntax ephemeral => ephemeral.Update(ephemeral.LiteralTokens, newGenerator),
                    MarkupTagHelperStartTagSyntax start => start.Update(start.OpenAngle, start.Bang, start.Name, start.Attributes, start.ForwardSlash, start.CloseAngle, newGenerator),
                    MarkupTagHelperEndTagSyntax end => end.Update(end.OpenAngle, end.ForwardSlash, end.Bang, end.Name, end.MiscAttributeContent, end.CloseAngle, newGenerator),
                    MarkupTextLiteralSyntax text => text.Update(text.LiteralTokens, newGenerator),
                    MarkupTransitionSyntax transition => transition.Update(transition.TransitionTokens, newGenerator),
                    CSharpStatementLiteralSyntax csharp => csharp.Update(csharp.LiteralTokens, newGenerator),
                    CSharpExpressionLiteralSyntax csharp => csharp.Update(csharp.LiteralTokens, newGenerator),
                    CSharpEphemeralTextLiteralSyntax ephemeral => ephemeral.Update(ephemeral.LiteralTokens, newGenerator),
                    CSharpTransitionSyntax transition => transition.Update(transition.Transition, newGenerator),
                    RazorMetaCodeSyntax meta => meta.Update(meta.MetaCode, newGenerator),
                    UnclassifiedTextLiteralSyntax unclassified => unclassified.Update(unclassified.LiteralTokens, newGenerator),
                    _ => throw new InvalidOperationException($"Unexpected node type {node.Kind}"),
                };
            }

            context = builder?.Build(AcceptedCharactersInternal.AnyExceptNewline);

            return node.WithEditHandler(context);
        }
    }

    private class TryParseResult
    {
        public string AttributeName { get; set; }

        public RazorSyntaxNode RewrittenAttribute { get; set; }

        public AttributeStructure AttributeStructure { get; set; }

        public bool IsBoundAttribute { get; set; }

        public bool IsBoundNonStringAttribute { get; set; }

        public bool IsBoundBooleanAttribute { get; set; }

        public bool IsMissingDictionaryKey { get; set; }

        public bool IsDuplicateAttribute { get; set; }

        public bool IsDirectiveAttribute { get; set; }
    }
}
