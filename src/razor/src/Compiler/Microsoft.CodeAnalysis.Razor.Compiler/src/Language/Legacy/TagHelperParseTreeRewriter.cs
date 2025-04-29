// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Language.Legacy;

internal static class TagHelperParseTreeRewriter
{
    public static RazorSyntaxTree Rewrite(RazorSyntaxTree syntaxTree, TagHelperBinder binder, out ISet<TagHelperDescriptor> usedDescriptors)
    {
        using var errorSink = new ErrorSink();

        var rewriter = new Rewriter(
            syntaxTree.Source,
            binder,
            syntaxTree.Options,
            errorSink);

        var rewritten = rewriter.Visit(syntaxTree.Root);

        var treeDiagnostics = syntaxTree.Diagnostics;
        var sinkDiagnostics = errorSink.GetErrorsAndClear();

        using var builder = new PooledArrayBuilder<RazorDiagnostic>(capacity: treeDiagnostics.Length + sinkDiagnostics.Length);

        builder.AddRange(treeDiagnostics);
        builder.AddRange(sinkDiagnostics);

        foreach (var tagHelper in binder.TagHelpers)
        {
            foreach (var diagnostic in tagHelper.GetAllDiagnostics())
            {
                builder.Add(diagnostic);
            }
        }

        var diagnostics = builder.ToImmutableOrderedBy(static d => d.Span.AbsoluteIndex);

        usedDescriptors = rewriter.UsedDescriptors;
        return new RazorSyntaxTree(rewritten, syntaxTree.Source, diagnostics, syntaxTree.Options);
    }

    // Internal for testing.
    internal sealed class Rewriter : SyntaxRewriter
    {
        // Internal for testing.
        // Null characters are invalid markup for HTML attribute values.
        internal const char InvalidAttributeValueMarker = '\0';

        private readonly RazorSourceDocument _source;
        private readonly TagHelperBinder _binder;
        private readonly Stack<TagTracker> _trackerStack;
        private readonly ErrorSink _errorSink;
        private readonly RazorParserOptions _options;
        private readonly HashSet<TagHelperDescriptor> _usedDescriptors;

        public Rewriter(
            RazorSourceDocument source,
            TagHelperBinder binder,
            RazorParserOptions options,
            ErrorSink errorSink)
        {
            _source = source;
            _binder = binder;
            _trackerStack = new Stack<TagTracker>();
            _options = options;
            _usedDescriptors = new HashSet<TagHelperDescriptor>();
            _errorSink = errorSink;
        }

        public HashSet<TagHelperDescriptor> UsedDescriptors => _usedDescriptors;

        private TagTracker? CurrentTracker => _trackerStack.Count > 0 ? _trackerStack.Peek() : null;

        private string? CurrentParentTagName => CurrentTracker?.TagName;

        private bool CurrentParentIsTagHelper => CurrentTracker?.IsTagHelper ?? false;

        private TagHelperTracker? CurrentTagHelperTracker => _trackerStack.FirstOrDefault(t => t.IsTagHelper) as TagHelperTracker;

        public override SyntaxNode VisitMarkupElement(MarkupElementSyntax node)
        {
            if (IsPartOfStartTag(node))
            {
                // If this element is inside a start tag, it is some sort of malformed case like
                // <p @do { someattribute=\"btn\"></p>, where the end "p" tag is inside the start "p" tag.
                // We don't want to do tag helper parsing for this tag.
                return base.VisitMarkupElement(node);
            }

            MarkupTagHelperStartTagSyntax? tagHelperStart = null;
            MarkupTagHelperEndTagSyntax? tagHelperEnd = null;
            TagHelperInfo? tagHelperInfo = null;

            // Visit the start tag.
            var startTag = (MarkupStartTagSyntax?)Visit(node.StartTag);
            if (startTag != null)
            {
                var tagName = startTag.GetTagNameWithOptionalBang();
                if (TryRewriteTagHelperStart(startTag, node.EndTag, out tagHelperStart, out tagHelperInfo))
                {
                    // This is a tag helper.
                    if (tagHelperInfo.TagMode == TagMode.SelfClosing || tagHelperInfo.TagMode == TagMode.StartTagOnly)
                    {
                        var tagHelperElement = SyntaxFactory.MarkupTagHelperElement(tagHelperStart, body: default, endTag: null);
                        var rewrittenTagHelper = tagHelperElement.WithTagHelperInfo(tagHelperInfo);
                        if (node.Body.Count == 0 && node.EndTag == null)
                        {
                            return rewrittenTagHelper;
                        }

                        // This tag contains a body and/or an end tag which needs to be moved to the parent.
                        using var _ = SyntaxListBuilderPool.GetPooledBuilder<RazorSyntaxNode>(out var rewrittenNodes);
                        rewrittenNodes.Add(rewrittenTagHelper);
                        var rewrittenBody = VisitList(node.Body);
                        rewrittenNodes.AddRange(rewrittenBody);

                        return SyntaxFactory.MarkupElement(startTag: null, body: rewrittenNodes.ToList(), endTag: node.EndTag);
                    }
                    else if (node.EndTag == null)
                    {
                        // Start tag helper with no corresponding end tag.
                        var source = new SourceSpan(SourceLocationTracker.Advance(startTag.GetSourceLocation(_source), "<"), tagName.Length);
                        _errorSink.OnError(
                            node.StartTag.IsVoidElement()
                                ? RazorDiagnosticFactory.CreateParsing_VoidElement(source, tagName)
                                : RazorDiagnosticFactory.CreateParsing_TagHelperFoundMalformedTagHelper(source, tagName));
                    }
                    else
                    {
                        // Tag helper start tag. Keep track.
                        var tracker = new TagHelperTracker(_binder.TagHelperPrefix, tagHelperInfo);
                        _trackerStack.Push(tracker);
                    }
                }
                else
                {
                    // Non-TagHelper tag.
                    ValidateParentAllowsPlainStartTag(startTag);

                    if (node.EndTag != null || (!startTag.IsSelfClosing() && !startTag.IsVoidElement()))
                    {
                        // Ideally we don't want to keep track of self-closing or void tags.
                        // But if a matching end tag exists, keep track of the start tag no matter what.
                        // We will just assume the parser had a good reason to do this.
                        var tracker = new TagTracker(tagName, IsTagHelper: false);
                        _trackerStack.Push(tracker);
                    }
                }
            }

            // Visit body between start and end tags.
            var body = VisitList(node.Body);

            // Visit end tag.
            var endTag = (MarkupEndTagSyntax)Visit(node.EndTag);
            if (endTag != null)
            {
                var tagName = endTag.GetTagNameWithOptionalBang();
                if (TryRewriteTagHelperEnd(startTag, endTag, out tagHelperEnd))
                {
                    // This is a tag helper
                    if (startTag == null)
                    {
                        // The end tag helper has no corresponding start tag, create an error.
                        _errorSink.OnError(
                            RazorDiagnosticFactory.CreateParsing_TagHelperFoundMalformedTagHelper(
                                new SourceSpan(SourceLocationTracker.Advance(endTag.GetSourceLocation(_source), "</"), tagName.Length), tagName));
                    }
                }
                else
                {
                    // Non tag helper end tag.
                    if (startTag == null)
                    {
                        // Standalone end tag. We may need to error if it is not supposed to be here.
                        // If there was a corresponding start tag, we would have already added this error.
                        ValidateParentAllowsPlainEndTag(endTag);
                    }
                    else
                    {
                        // Since a start tag exists, we must already be tracking it.
                        // Pop the stack as we're done with the end tag.
                        _trackerStack.Pop();
                    }
                }
            }

            if (tagHelperInfo != null)
            {
                // If we get here it means this element was rewritten as a tag helper.
                var tagHelperElement = SyntaxFactory.MarkupTagHelperElement(tagHelperStart, body, tagHelperEnd);
                return tagHelperElement.WithTagHelperInfo(tagHelperInfo);
            }

            // There was no matching tag helper for this element. Return.
            return node.Update(startTag, body, endTag);
        }

        public override SyntaxNode VisitMarkupTextLiteral(MarkupTextLiteralSyntax node)
        {
            var tagParent = node.FirstAncestorOrSelf<SyntaxNode>(n => n is MarkupStartTagSyntax || n is MarkupEndTagSyntax);
            var isPartOfTagBlock = tagParent != null;
            if (!isPartOfTagBlock)
            {
                ValidateParentAllowsContent(node);
            }

            return base.VisitMarkupTextLiteral(node);
        }

        private bool TryRewriteTagHelperStart(
            MarkupStartTagSyntax startTag,
            MarkupEndTagSyntax endTag,
            [NotNullWhen(true)] out MarkupTagHelperStartTagSyntax? rewritten,
            [NotNullWhen(true)] out TagHelperInfo? tagHelperInfo)
        {
            rewritten = null;
            tagHelperInfo = null;

            // Get tag name of the current block
            var tagName = startTag.GetTagNameWithOptionalBang();

            // Could not determine tag name, it can't be a TagHelper, continue on and track the element.
            if (string.IsNullOrEmpty(tagName) || tagName.StartsWith("!", StringComparison.Ordinal))
            {
                return false;
            }

            TagHelperBinding? tagHelperBinding;

            if (!IsPotentialTagHelperStart(tagName, startTag))
            {
                return false;
            }

            var tracker = CurrentTagHelperTracker;
            var tagNameScope = tracker?.TagName ?? string.Empty;

            // We're now in a start tag block, we first need to see if the tag block is a tag helper.
            var elementAttributes = GetAttributeNameValuePairs(startTag);

            tagHelperBinding = _binder.GetBinding(
                tagName,
                elementAttributes,
                CurrentParentTagName,
                CurrentParentIsTagHelper);

            // If there aren't any TagHelperDescriptors registered then we aren't a TagHelper
            if (tagHelperBinding == null)
            {
                // If the current tag matches the current TagHelper scope it means the parent TagHelper matched
                // all the required attributes but the current one did not; therefore, we need to increment the
                // OpenMatchingTags counter for current the TagHelperBlock so we don't end it too early.
                // ex: <myth req="..."><myth></myth></myth> We don't want the first myth to close on the inside
                // tag.
                if (string.Equals(tagNameScope, tagName, StringComparison.OrdinalIgnoreCase) && !startTag.IsSelfClosing())
                {
                    tracker.AssumeNotNull().OpenMatchingTags++;
                }

                return false;
            }

            foreach (var descriptor in tagHelperBinding.Descriptors)
            {
                _usedDescriptors.Add(descriptor);
            }

            ValidateParentAllowsTagHelper(tagName, startTag);
            ValidateBinding(tagHelperBinding, tagName, startTag);

            // We're in a start TagHelper block.
            ValidateStartTagSyntax(tagName, startTag);

            var rewrittenStartTag = TagHelperBlockRewriter.Rewrite(
                tagName,
                _options,
                startTag,
                tagHelperBinding,
                _errorSink,
                _source);

            var tagMode = TagHelperBlockRewriter.GetTagMode(startTag, endTag, tagHelperBinding);
            tagHelperInfo = new TagHelperInfo(tagName, tagMode, tagHelperBinding);
            rewritten = rewrittenStartTag;

            return true;
        }

        private bool TryRewriteTagHelperEnd(
            MarkupStartTagSyntax? startTag,
            MarkupEndTagSyntax endTag,
            [NotNullWhen(true)] out MarkupTagHelperEndTagSyntax? rewritten)
        {
            rewritten = null;
            var tagName = endTag.GetTagNameWithOptionalBang();
            // Could not determine tag name, it can't be a TagHelper, continue on and track the element.
            if (string.IsNullOrEmpty(tagName) || tagName.StartsWith("!", StringComparison.Ordinal))
            {
                return false;
            }

            var tracker = CurrentTagHelperTracker;
            var tagNameScope = tracker?.TagName ?? string.Empty;
            if (!IsPotentialTagHelperEnd(tagName, endTag))
            {
                return false;
            }

            // Validate that our end tag matches the currently scoped tag, if not we may need to error.
            if (startTag != null && tagNameScope.Equals(tagName, StringComparison.OrdinalIgnoreCase))
            {
                // If there are additional end tags required before we can build our block it means we're in a
                // situation like this: <myth req="..."><myth></myth></myth> where we're at the inside </myth>.
                if (tracker.AssumeNotNull().OpenMatchingTags > 0)
                {
                    tracker.OpenMatchingTags--;

                    return false;
                }

                ValidateEndTagSyntax(tagName, endTag);

                _trackerStack.Pop();
            }
            else
            {
                var tagHelperBinding = _binder.GetBinding(
                    tagName,
                    attributes: [],
                    parentTagName: CurrentParentTagName,
                    parentIsTagHelper: CurrentParentIsTagHelper);

                // If there are not TagHelperDescriptors associated with the end tag block that also have no
                // required attributes then it means we can't be a TagHelper, bail out.
                if (tagHelperBinding == null)
                {
                    return false;
                }

                foreach (var descriptor in tagHelperBinding.Descriptors)
                {
                    var boundRules = tagHelperBinding.Mappings[descriptor];
                    var invalidRule = boundRules.FirstOrDefault(static rule => rule.TagStructure == TagStructure.WithoutEndTag);

                    if (invalidRule != null)
                    {
                        // End tag TagHelper that states it shouldn't have an end tag.
                        _errorSink.OnError(
                            RazorDiagnosticFactory.CreateParsing_TagHelperMustNotHaveAnEndTag(
                                new SourceSpan(SourceLocationTracker.Advance(endTag.GetSourceLocation(_source), "</"), tagName.Length),
                                tagName,
                                descriptor.DisplayName,
                                invalidRule.TagStructure));

                        return false;
                    }
                }
            }

            rewritten = SyntaxFactory.MarkupTagHelperEndTag(
                endTag.OpenAngle, endTag.ForwardSlash, endTag.Bang, endTag.Name, endTag.MiscAttributeContent, endTag.CloseAngle, chunkGenerator: null);

            return true;
        }

        // Internal for testing
        internal static ImmutableArray<KeyValuePair<string, string>> GetAttributeNameValuePairs(MarkupStartTagSyntax tagBlock)
        {
            if (tagBlock.Attributes.Count == 0)
            {
                return [];
            }

            using var _ = StringBuilderPool.GetPooledObject(out var attributeValueBuilder);
            using var attributes = new PooledArrayBuilder<KeyValuePair<string, string>>();

            foreach (var attribute in tagBlock.Attributes)
            {
                if (attribute is CSharpCodeBlockSyntax)
                {
                    // Code blocks in the attribute area of tags mangles following attributes.
                    // It's also not supported by TagHelpers, bail early to avoid creating bad attribute value pairs.
                    break;
                }

                if (attribute is MarkupMinimizedAttributeBlockSyntax minimizedAttributeBlock)
                {
                    if (minimizedAttributeBlock.Name == null)
                    {
                        attributeValueBuilder.Append(InvalidAttributeValueMarker);
                        continue;
                    }

                    var minimizedAttribute = new KeyValuePair<string, string>(minimizedAttributeBlock.Name.GetContent(), string.Empty);
                    attributes.Add(minimizedAttribute);
                    continue;
                }

                if (attribute is not MarkupAttributeBlockSyntax attributeBlock)
                {
                    // If the parser thought these aren't attributes, we don't care about them. Move on.
                    continue;
                }

                if (attributeBlock.Name == null)
                {
                    attributeValueBuilder.Append(InvalidAttributeValueMarker);
                    continue;
                }

                if (attributeBlock.Value != null)
                {
                    foreach (var child in attributeBlock.Value.Children)
                    {
                        if (child is MarkupLiteralAttributeValueSyntax literalValue)
                        {
                            attributeValueBuilder.Append(literalValue.GetContent());
                        }
                        else
                        {
                            attributeValueBuilder.Append(InvalidAttributeValueMarker);
                        }
                    }
                }

                var attributeName = attributeBlock.Name.GetContent();
                var attributeValue = attributeValueBuilder.ToString();
                attributes.Add(new(attributeName, attributeValue));

                attributeValueBuilder.Clear();
            }

            return attributes.DrainToImmutable();
        }

        private void ValidateParentAllowsTagHelper(string tagName, MarkupStartTagSyntax tagBlock)
        {
            if (HasAllowedChildren() &&
                !CurrentTagHelperTracker.AllowsChild(tagName, nameIncludesPrefix: true))
            {
                OnAllowedChildrenStartTagError(CurrentTagHelperTracker, tagName, tagBlock, _errorSink, _source);
            }
        }

        private void ValidateBinding(
            TagHelperBinding bindingResult,
            string tagName,
            MarkupStartTagSyntax tagBlock)
        {
            // Ensure that all descriptors associated with this tag have appropriate TagStructures. Cannot have
            // multiple descriptors that expect different TagStructures (other than TagStructure.Unspecified).
            TagHelperDescriptor? baseDescriptor = null;
            TagStructure? baseStructure = null;

            foreach (var descriptor in bindingResult.Descriptors)
            {
                var boundRules = bindingResult.Mappings[descriptor];
                foreach (var rule in boundRules)
                {
                    if (rule.TagStructure != TagStructure.Unspecified)
                    {
                        // Can't have a set of TagHelpers that expect different structures.
                        if (baseStructure.HasValue && baseStructure != rule.TagStructure)
                        {
                            _errorSink.OnError(
                                RazorDiagnosticFactory.CreateTagHelper_InconsistentTagStructure(
                                    new SourceSpan(tagBlock.GetSourceLocation(_source), tagBlock.FullWidth),
                                    baseDescriptor!.DisplayName,
                                    descriptor.DisplayName,
                                    tagName));
                        }

                        baseDescriptor = descriptor;
                        baseStructure = rule.TagStructure;
                    }
                }
            }
        }

        private bool ValidateStartTagSyntax(string tagName, MarkupStartTagSyntax tag)
        {
            // We assume an invalid syntax until we verify that the tag meets all of our "valid syntax" criteria.
            if (IsPartialStartTag(tag))
            {
                var errorStart = GetStartTagDeclarationErrorStart(tag, _source);

                _errorSink.OnError(
                    RazorDiagnosticFactory.CreateParsing_TagHelperMissingCloseAngle(
                        new SourceSpan(errorStart, tagName.Length), tagName));

                return false;
            }

            return true;
        }

        private bool ValidateEndTagSyntax(string tagName, MarkupEndTagSyntax tag)
        {
            // We assume an invalid syntax until we verify that the tag meets all of our "valid syntax" criteria.
            if (IsPartialEndTag(tag))
            {
                var errorStart = GetEndTagDeclarationErrorStart(tag, _source);

                _errorSink.OnError(
                    RazorDiagnosticFactory.CreateParsing_TagHelperMissingCloseAngle(
                        new SourceSpan(errorStart, tagName.Length), tagName));

                return false;
            }

            return true;
        }

        private static bool IsPotentialTagHelperStart(string tagName, MarkupStartTagSyntax startTag)
        {
            return !string.Equals(tagName, SyntaxConstants.TextTagName, StringComparison.OrdinalIgnoreCase) ||
                   !startTag.IsMarkupTransition;
        }

        private static bool IsPotentialTagHelperEnd(string tagName, MarkupEndTagSyntax endTag)
        {
            return !string.Equals(tagName, SyntaxConstants.TextTagName, StringComparison.OrdinalIgnoreCase) ||
                   !endTag.IsMarkupTransition;
        }

        private static bool IsPartialStartTag(MarkupStartTagSyntax startTag)
        {
            return startTag.CloseAngle.IsMissing;
        }

        private static bool IsPartialEndTag(MarkupEndTagSyntax endTag)
        {
            return endTag.CloseAngle.IsMissing;
        }

        private void ValidateParentAllowsContent(SyntaxNode child)
        {
            if (HasAllowedChildren())
            {
                var isDisallowedContent = true;
                if (_options.AllowHtmlCommentsInTagHelpers)
                {
                    isDisallowedContent = !IsComment(child) &&
                        !child.IsTransitionSpanKind() &&
                        !child.IsCodeSpanKind();
                }

                if (isDisallowedContent)
                {
                    var content = child.GetContent();
                    if (!string.IsNullOrWhiteSpace(content))
                    {
                        var trimmedStart = content.AsSpan().TrimStart();
                        var whitespace = content[..^trimmedStart.Length];
                        var errorStart = SourceLocationTracker.Advance(child.GetSourceLocation(_source), whitespace);
                        var length = trimmedStart.TrimEnd().Length;
                        var allowedChildren = CurrentTagHelperTracker.AllowedChildren;
                        var allowedChildrenString = string.Join(", ", allowedChildren);
                        _errorSink.OnError(
                            RazorDiagnosticFactory.CreateTagHelper_CannotHaveNonTagContent(
                                new SourceSpan(errorStart, length),
                                CurrentTagHelperTracker.TagName,
                                allowedChildrenString));
                    }
                }
            }
        }

        private void ValidateParentAllowsPlainStartTag(MarkupStartTagSyntax tagBlock)
        {
            var tagName = tagBlock.GetTagNameWithOptionalBang();

            // Treat partial tags such as '</' which have no tag names as content.
            if (string.IsNullOrEmpty(tagName))
            {
                var firstChild = tagBlock.LegacyChildren.First();
                Debug.Assert(firstChild is MarkupTextLiteralSyntax);

                ValidateParentAllowsContent(firstChild);
                return;
            }

            if (!HasAllowedChildren())
            {
                return;
            }

            var binding = _binder.GetBinding(
                tagName,
                attributes: [],
                parentTagName: CurrentParentTagName,
                parentIsTagHelper: CurrentParentIsTagHelper);

            // If we found a binding for the current tag, then it is a tag helper. Use the prefixed allowed children to compare.
            if (!CurrentTagHelperTracker.AllowsChild(tagName, nameIncludesPrefix: binding is not null))
            {
                OnAllowedChildrenStartTagError(CurrentTagHelperTracker, tagName, tagBlock, _errorSink, _source);
            }
        }

        private void ValidateParentAllowsPlainEndTag(MarkupEndTagSyntax tagBlock)
        {
            var tagName = tagBlock.GetTagNameWithOptionalBang();

            // Treat partial tags such as '</' which have no tag names as content.
            if (string.IsNullOrEmpty(tagName))
            {
                var firstChild = tagBlock.LegacyChildren.First();
                Debug.Assert(firstChild is MarkupTextLiteralSyntax);

                ValidateParentAllowsContent(firstChild);
                return;
            }

            if (!HasAllowedChildren())
            {
                return;
            }

            var binding = _binder.GetBinding(
                tagName,
                attributes: [],
                parentTagName: CurrentParentTagName,
                parentIsTagHelper: CurrentParentIsTagHelper);

            // If we found a binding for the current tag, then it is a tag helper. Use the prefixed allowed children to compare.
            if (!CurrentTagHelperTracker.AllowsChild(tagName, nameIncludesPrefix: binding is not null))
            {
                OnAllowedChildrenEndTagError(CurrentTagHelperTracker, tagName, tagBlock, _errorSink, _source);
            }
        }

        [MemberNotNullWhen(true, nameof(CurrentTagHelperTracker))]
        private bool HasAllowedChildren()
        {
            // TODO: Questionable logic. Need to revisit
            var currentTracker = _trackerStack.Count > 0 ? _trackerStack.Peek() : null;

            // If the current tracker is not a TagHelper then there's no AllowedChildren to enforce.
            if (currentTracker == null || !currentTracker.IsTagHelper)
            {
                return false;
            }

            return CurrentTagHelperTracker.AssumeNotNull().AllowedChildren.Length > 0;
        }

        private static bool IsPartOfStartTag(SyntaxNode node)
        {
            // Check if an ancestor is a start tag of a MarkupElement.
            var parent = node.FirstAncestorOrSelf<SyntaxNode>(static n =>
            {
                return n.Parent is MarkupElementSyntax element && element.StartTag == n;
            });

            return parent != null;
        }

        private static bool IsComment(SyntaxNode node)
        {
            var commentParent = node.FirstAncestorOrSelf<SyntaxNode>(
                n => n is RazorCommentBlockSyntax || n is MarkupCommentBlockSyntax);

            return commentParent != null;
        }

        private static void OnAllowedChildrenStartTagError(
            TagHelperTracker tracker,
            string tagName,
            MarkupStartTagSyntax tagBlock,
            ErrorSink errorSink,
            RazorSourceDocument source)
        {
            var allowedChildrenString = string.Join(", ", tracker.AllowedChildren);
            var errorStart = GetStartTagDeclarationErrorStart(tagBlock, source);

            errorSink.OnError(
                RazorDiagnosticFactory.CreateTagHelper_InvalidNestedTag(
                    new SourceSpan(errorStart, tagName.Length),
                    tagName,
                    tracker.TagName,
                    allowedChildrenString));
        }

        private static void OnAllowedChildrenEndTagError(
            TagHelperTracker tracker,
            string tagName,
            MarkupEndTagSyntax tagBlock,
            ErrorSink errorSink,
            RazorSourceDocument source)
        {
            var allowedChildrenString = string.Join(", ", tracker.AllowedChildren);
            var errorStart = GetEndTagDeclarationErrorStart(tagBlock, source);

            errorSink.OnError(
                RazorDiagnosticFactory.CreateTagHelper_InvalidNestedTag(
                    new SourceSpan(errorStart, tagName.Length),
                    tagName,
                    tracker.TagName,
                    allowedChildrenString));
        }

        private static SourceLocation GetStartTagDeclarationErrorStart(MarkupStartTagSyntax tagBlock, RazorSourceDocument source)
        {
            return SourceLocationTracker.Advance(tagBlock.GetSourceLocation(source), "<");
        }

        private static SourceLocation GetEndTagDeclarationErrorStart(MarkupEndTagSyntax tagBlock, RazorSourceDocument source)
        {
            return SourceLocationTracker.Advance(tagBlock.GetSourceLocation(source), "</");
        }

        private record TagTracker(string TagName, bool IsTagHelper);

        private record TagHelperTracker : TagTracker
        {
            public uint OpenMatchingTags;

            private readonly string? _tagHelperPrefix;
            private readonly TagHelperBinding _binding;

            private readonly Lazy<(ImmutableArray<string> Names, HashSet<string> NameSet)> _lazyAllowedChildren;
            private readonly Lazy<HashSet<string>> _lazyPrefixedAllowedChildrenNameSet;

            public ImmutableArray<string> AllowedChildren => _lazyAllowedChildren.Value.Names;

            public TagHelperTracker(string? tagHelperPrefix, TagHelperInfo info)
                : base(info.TagName, IsTagHelper: true)
            {
                _tagHelperPrefix = tagHelperPrefix;
                _binding = info.BindingResult;

                _lazyAllowedChildren = new(CreateAllowedChildren);
                _lazyPrefixedAllowedChildrenNameSet = new(CreatePrefixedAllowedChildren);
            }

            public bool AllowsChild(string tagName, bool nameIncludesPrefix)
                => nameIncludesPrefix
                    ? _lazyPrefixedAllowedChildrenNameSet.Value.Contains(tagName)
                    : _lazyAllowedChildren.Value.NameSet.Contains(tagName);

            private (ImmutableArray<string>, HashSet<string>) CreateAllowedChildren()
            {
                var distinctSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                using var result = new PooledArrayBuilder<string>();

                foreach (var tagHelper in _binding.Descriptors)
                {
                    foreach (var allowedChildTag in tagHelper.AllowedChildTags)
                    {
                        var name = allowedChildTag.Name;

                        if (distinctSet.Add(name))
                        {
                            result.Add(name);
                        }
                    }
                }

                return (result.DrainToImmutable(), distinctSet);
            }

            private HashSet<string> CreatePrefixedAllowedChildren()
            {
                if (_tagHelperPrefix is not string tagHelperPrefix)
                {
                    return _lazyAllowedChildren.Value.NameSet;
                }

                var distinctSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var childName in AllowedChildren)
                {
                    distinctSet.Add(tagHelperPrefix + childName);
                }

                return distinctSet;
            }
        }
    }
}
