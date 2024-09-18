// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using RazorSyntaxNode = Microsoft.AspNetCore.Razor.Language.Syntax.SyntaxNode;

namespace Microsoft.CodeAnalysis.Razor.Formatting;

internal abstract partial class CSharpFormattingPassBase(IDocumentMappingService documentMappingService, IHostServicesProvider hostServicesProvider, bool isFormatOnType) : IFormattingPass
{
    private readonly bool _isFormatOnType = isFormatOnType;

    protected IDocumentMappingService DocumentMappingService { get; } = documentMappingService;

    public async Task<ImmutableArray<TextChange>> ExecuteAsync(FormattingContext context, ImmutableArray<TextChange> changes, CancellationToken cancellationToken)
    {
        using var roslynWorkspaceHelper = new RoslynWorkspaceHelper(hostServicesProvider);

        return await ExecuteCoreAsync(context, roslynWorkspaceHelper, changes, cancellationToken).ConfigureAwait(false);
    }

    protected abstract Task<ImmutableArray<TextChange>> ExecuteCoreAsync(FormattingContext context, RoslynWorkspaceHelper roslynWorkspaceHelper, ImmutableArray<TextChange> changes, CancellationToken cancellationToken);

    protected async Task<ImmutableArray<TextChange>> AdjustIndentationAsync(FormattingContext context, int startLine, int endLineInclusive, HostWorkspaceServices hostWorkspaceServices, CancellationToken cancellationToken)
    {
        // In this method, the goal is to make final adjustments to the indentation of each line.
        // We will take into account the following,
        // 1. The indentation due to nested C# structures
        // 2. The indentation due to Razor and HTML constructs

        var text = context.SourceText;

        // To help with figuring out the correct indentation, first we will need the indentation
        // that the C# formatter wants to apply in the following locations,
        // 1. The start and end of each of our source mappings
        // 2. The start of every line that starts in C# context

        // Due to perf concerns, we only want to invoke the real C# formatter once.
        // So, let's collect all the significant locations that we want to obtain the CSharpDesiredIndentations for.

        using var _1 = HashSetPool<int>.GetPooledObject(out var significantLocations);

        // First, collect all the locations at the beginning and end of each source mapping.
        var sourceMappingMap = new Dictionary<int, int>();
        foreach (var mapping in context.CodeDocument.GetCSharpDocument().SourceMappings)
        {
            var mappingSpan = new TextSpan(mapping.OriginalSpan.AbsoluteIndex, mapping.OriginalSpan.Length);
#if DEBUG
            var spanText = context.SourceText.GetSubTextString(mappingSpan);
#endif

            var options = new ShouldFormatOptions(
                // Implicit expressions and single line explicit expressions don't affect the indentation of anything
                // under them, so we don't want their positions to be "significant".
                AllowImplicitExpressions: false,
                AllowSingleLineExplicitExpressions: false,

                // Implicit statements are @if, @foreach etc. so they do affect indentation
                AllowImplicitStatements: true,

                IsLineRequest: false);

            if (!ShouldFormat(context, mappingSpan, options, out var owner))
            {
                // We don't care about this range as this can potentially lead to incorrect scopes.
                continue;
            }

            var originalStartLocation = mapping.OriginalSpan.AbsoluteIndex;
            var projectedStartLocation = mapping.GeneratedSpan.AbsoluteIndex;
            sourceMappingMap[originalStartLocation] = projectedStartLocation;
            significantLocations.Add(projectedStartLocation);

            var originalEndLocation = mapping.OriginalSpan.AbsoluteIndex + mapping.OriginalSpan.Length + 1;
            var projectedEndLocation = mapping.GeneratedSpan.AbsoluteIndex + mapping.GeneratedSpan.Length + 1;
            sourceMappingMap[originalEndLocation] = projectedEndLocation;
            significantLocations.Add(projectedEndLocation);
        }

        // Next, collect all the line starts that start in C# context
        var indentations = context.GetIndentations();
        var lineStartMap = new Dictionary<int, int>();
        for (var i = startLine; i <= endLineInclusive; i++)
        {
            if (indentations[i].EmptyOrWhitespaceLine)
            {
                // We should remove whitespace on empty lines.
                continue;
            }

            var line = context.SourceText.Lines[i];
            var lineStart = line.GetFirstNonWhitespacePosition() ?? line.Start;

            var lineStartSpan = new TextSpan(lineStart, 0);
            if (!ShouldFormat(context, lineStartSpan, allowImplicitStatements: true))
            {
                // We don't care about this range as this can potentially lead to incorrect scopes.
                continue;
            }

            if (DocumentMappingService.TryMapToGeneratedDocumentPosition(context.CodeDocument.GetCSharpDocument(), lineStart, out _, out var projectedLineStart))
            {
                lineStartMap[lineStart] = projectedLineStart;
                significantLocations.Add(projectedLineStart);
            }
        }

        // Now, invoke the C# formatter to obtain the CSharpDesiredIndentation for all significant locations.
        var significantLocationIndentation = await CSharpFormatter.GetCSharpIndentationAsync(context, significantLocations, hostWorkspaceServices, cancellationToken).ConfigureAwait(false);

        // Build source mapping indentation scopes.
        var sourceMappingIndentations = new SortedDictionary<int, IndentationData>();
        var syntaxTreeRoot = context.CodeDocument.GetSyntaxTree().Root;
        foreach (var originalLocation in sourceMappingMap.Keys)
        {
            var significantLocation = sourceMappingMap[originalLocation];
            if (!significantLocationIndentation.TryGetValue(significantLocation, out var indentation))
            {
                // C# formatter didn't return an indentation for this. Skip.
                continue;
            }

            if (originalLocation > syntaxTreeRoot.EndPosition)
            {
                continue;
            }

            var scopeOwner = syntaxTreeRoot.FindInnermostNode(originalLocation);
            if (!sourceMappingIndentations.ContainsKey(originalLocation))
            {
                sourceMappingIndentations[originalLocation] = new IndentationData(indentation);
            }

            // For @section blocks we have special handling to add a fake source mapping/significant location at the end of the
            // section, to return the indentation back to before the start of the section block.
            if (scopeOwner?.Parent?.Parent?.Parent is RazorDirectiveSyntax containingDirective &&
                containingDirective.DirectiveDescriptor.Directive == SectionDirective.Directive.Directive &&
                !sourceMappingIndentations.ContainsKey(containingDirective.EndPosition - 1))
            {
                // We want the indentation for the end point to be whatever the indentation was before the start point. For
                // performance reasons, and because source mappings could be un-ordered, we defer that calculation until
                // later, when we have all of the information in place. We use a negative number to indicate that there is
                // more processing to do.
                // This is saving repeatedly realising the source mapping indentations keys, then converting them to an array,
                // and then doing binary search here, before we've processed all of the mappings
                sourceMappingIndentations[containingDirective.EndPosition - 1] = new IndentationData(lazyLoad: true, offset: originalLocation - 1);
            }
        }

        var sourceMappingIndentationScopes = sourceMappingIndentations.Keys.ToArray();

        // Build lineStart indentation map.
        var lineStartIndentations = new Dictionary<int, int>();
        foreach (var originalLocation in lineStartMap.Keys)
        {
            var significantLocation = lineStartMap[originalLocation];
            if (!significantLocationIndentation.TryGetValue(significantLocation, out var indentation))
            {
                // C# formatter didn't return an indentation for this. Skip.
                continue;
            }

            lineStartIndentations[originalLocation] = indentation;
        }

        // Now, let's combine the C# desired indentation with the Razor and HTML indentation for each line.
        var newIndentations = new Dictionary<int, int>();
        for (var i = startLine; i <= endLineInclusive; i++)
        {
            if (indentations[i].EmptyOrWhitespaceLine)
            {
                // We should remove whitespace on empty lines.
                newIndentations[i] = 0;
                continue;
            }

            var minCSharpIndentation = context.GetIndentationOffsetForLevel(indentations[i].MinCSharpIndentLevel);
            var line = context.SourceText.Lines[i];
            var lineStart = line.GetFirstNonWhitespacePosition() ?? line.Start;
            var lineStartSpan = new TextSpan(lineStart, 0);
            if (!ShouldFormatLine(context, lineStartSpan, allowImplicitStatements: true))
            {
                // We don't care about this line as it lies in an area we don't want to format.
                continue;
            }

            if (!lineStartIndentations.TryGetValue(lineStart, out var csharpDesiredIndentation))
            {
                // Couldn't remap. This is probably a non-C# location.
                // Use SourceMapping indentations to locate the C# scope of this line.
                // E.g,
                //
                // @if (true) {
                //   <div>
                //  |</div>
                // }
                //
                // We can't find a direct mapping at |, but we can infer its base indentation from the
                // indentation of the latest source mapping prior to this line.
                // We use binary search to find that spot.

                var index = Array.BinarySearch(sourceMappingIndentationScopes, lineStart);

                if (index < 0)
                {
                    // Couldn't find the exact value. Find the index of the element to the left of the searched value.
                    index = (~index) - 1;
                }

                if (index < 0)
                {
                    // If we _still_ couldn't find the right indentation, then it probably means that the text is
                    // before the first source mapping location, so we can just place it in the minimum spot (realistically
                    // at index 0 in the razor file, but we use minCSharpIndentation because we're adjusting based on the
                    // generated file here)
                    csharpDesiredIndentation = minCSharpIndentation;
                }
                else
                {
                    // index will now be set to the same value as the end of the closest source mapping.
                    var absoluteIndex = sourceMappingIndentationScopes[index];
                    csharpDesiredIndentation = sourceMappingIndentations[absoluteIndex].GetIndentation(sourceMappingIndentations, sourceMappingIndentationScopes, minCSharpIndentation);

                    // This means we didn't find an exact match and so we used the indentation of the end of a previous mapping.
                    // So let's use the MinCSharpIndentation of that same location if possible.
                    if (context.TryGetFormattingSpan(absoluteIndex, out var span))
                    {
                        minCSharpIndentation = context.GetIndentationOffsetForLevel(span.MinCSharpIndentLevel);
                    }
                }
            }

            // Now let's use that information to figure out the effective C# indentation.
            // This should be based on context.
            // For instance, lines inside @code/@functions block should be reduced one level
            // and lines inside @{} should be reduced by two levels.

            if (csharpDesiredIndentation < minCSharpIndentation)
            {
                // CSharp formatter doesn't want to indent this. Let's not touch it.
                continue;
            }

            var effectiveCSharpDesiredIndentation = csharpDesiredIndentation - minCSharpIndentation;
            var razorDesiredIndentation = context.GetIndentationOffsetForLevel(indentations[i].IndentationLevel);
            if (indentations[i].StartsInHtmlContext)
            {
                // This is a non-C# line.
                if (_isFormatOnType)
                {
                    // HTML formatter doesn't run in the case of format on type.
                    // Let's stick with our syntax understanding of HTML to figure out the desired indentation.
                }
                else
                {
                    // Given that the HTML formatter ran before this, we can assume
                    // HTML is already correctly formatted. So we can use the existing indentation as is.
                    // We need to make sure to use the indentation size, as this will get passed to
                    // GetIndentationString eventually.
                    razorDesiredIndentation = indentations[i].ExistingIndentationSize;
                }
            }

            var effectiveDesiredIndentation = razorDesiredIndentation + effectiveCSharpDesiredIndentation;

            // This will now contain the indentation we ultimately want to apply to this line.
            newIndentations[i] = effectiveDesiredIndentation;
        }

        // Now that we have collected all the indentations for each line, let's convert them to text edits.
        using var changes = new PooledArrayBuilder<TextChange>(capacity: newIndentations.Count);
        foreach (var item in newIndentations)
        {
            var line = item.Key;
            var indentation = item.Value;
            Debug.Assert(indentation >= 0, "Negative indentation. This is unexpected.");

            var existingIndentationLength = indentations[line].ExistingIndentation;
            var spanToReplace = new TextSpan(context.SourceText.Lines[line].Start, existingIndentationLength);
            var effectiveDesiredIndentation = FormattingUtilities.GetIndentationString(indentation, context.Options.InsertSpaces, context.Options.TabSize);
            changes.Add(new TextChange(spanToReplace, effectiveDesiredIndentation));
        }

        return changes.DrainToImmutable();
    }

    protected static bool ShouldFormat(FormattingContext context, TextSpan mappingSpan, bool allowImplicitStatements)
        => ShouldFormat(context, mappingSpan, allowImplicitStatements, out _);

    protected static bool ShouldFormat(FormattingContext context, TextSpan mappingSpan, bool allowImplicitStatements, out RazorSyntaxNode? foundOwner)
        => ShouldFormat(context, mappingSpan, new ShouldFormatOptions(allowImplicitStatements, isLineRequest: false), out foundOwner);

    private static bool ShouldFormatLine(FormattingContext context, TextSpan mappingSpan, bool allowImplicitStatements)
        => ShouldFormat(context, mappingSpan, new ShouldFormatOptions(allowImplicitStatements, isLineRequest: true), out _);

    private static bool ShouldFormat(FormattingContext context, TextSpan mappingSpan, ShouldFormatOptions options, out RazorSyntaxNode? foundOwner)
    {
        // We should be called with the range of various C# SourceMappings.

        if (mappingSpan.Start == 0)
        {
            // The mapping starts at 0. It can't be anything special but pure C#. Let's format it.
            foundOwner = null;
            return true;
        }

        var syntaxTree = context.CodeDocument.GetSyntaxTree();
        var owner = syntaxTree.Root.FindInnermostNode(mappingSpan.Start, includeWhitespace: true);
        if (owner is null)
        {
            // Can't determine owner of this position. Optimistically allow formatting.
            foundOwner = null;
            return true;
        }

        foundOwner = owner;

        // Special case: If we're formatting implicit statements, we want to treat the `@attribute` directive and
        // the `@typeparam` directive as one so that the C# content within them is formatted as C#
        if (options.AllowImplicitStatements &&
            (
                IsAttributeDirective() ||
                IsTypeParamDirective()
            ))
        {
            return true;
        }

        if (IsInsideRazorComment())
        {
            return false;
        }

        if (IsInBoundComponentAttributeName())
        {
            return false;
        }

        if (IsInHtmlAttributeValue())
        {
            return false;
        }

        if (IsInDirectiveWithNoKind())
        {
            return false;
        }

        if (IsInSingleLineDirective())
        {
            return false;
        }

        if (!options.AllowImplicitExpressions && IsImplicitExpression())
        {
            return false;
        }

        if (!options.AllowSingleLineExplicitExpressions && IsSingleLineExplicitExpression())
        {
            return false;
        }

        if (IsInSectionDirectiveBrace())
        {
            return false;
        }

        if (!options.AllowImplicitStatements && IsImplicitStatementStart())
        {
            return false;
        }

        if (IsInTemplateBlock())
        {
            return false;
        }

        return true;

        bool IsInsideRazorComment()
        {
            // We don't want to format _in_ comments, but we do want to move the start `@*` to the right position
            if (owner is RazorCommentBlockSyntax &&
                mappingSpan.Start != owner.SpanStart)
            {
                return true;
            }

            return false;
        }

        bool IsImplicitStatementStart()
        {
            // We will return true if the position points to the start of the C# portion of an implicit statement.
            // `@|for(...)` - true
            // `@|if(...)` - true
            // `@{|...` - false
            // `@code {|...` - false
            //

            if (owner.SpanStart == mappingSpan.Start &&
                owner is CSharpStatementLiteralSyntax &&
                owner.Parent is CSharpCodeBlockSyntax &&
                owner.TryGetPreviousSibling(out var transition) && transition is CSharpTransitionSyntax)
            {
                return true;
            }

            // Not an implicit statement.
            return false;
        }

        bool IsInBoundComponentAttributeName()
        {
            // E.g, (| is position)
            //
            // `<p |csharpattr="Variable">` - true
            //
            // Because we map attributes, so rename and FAR works, there could be C# mapping for them,
            // but only if they're actually bound attributes. We don't want the mapping to throw make the
            // formatting engine think it needs to apply C# indentation rules.
            //
            // The exception here is if we're being asked whether to format the line of code at all,
            // then we want to pretend it's not a component attribute, because we do still want the line
            // formatted. ie, given this:
            //
            // `<p
            //     |csharpattr="Variable">`
            //
            // We want to return false when being asked to format the line, so the line gets indented, but
            // return true if we're just being asked "should we format this according to C# rules".

            return owner is MarkupTextLiteralSyntax
            {
                Parent: MarkupTagHelperAttributeSyntax { TagHelperAttributeInfo.Bound: true } or
                        MarkupTagHelperDirectiveAttributeSyntax { TagHelperAttributeInfo.Bound: true } or
                        MarkupMinimizedTagHelperAttributeSyntax { TagHelperAttributeInfo.Bound: true } or
                        MarkupMinimizedTagHelperDirectiveAttributeSyntax { TagHelperAttributeInfo.Bound: true }
            } && !options.IsLineRequest;
        }

        bool IsInHtmlAttributeValue()
        {
            // E.g, (| is position)
            //
            // `<p csharpattr="|Variable">` - true
            //
            return owner.AncestorsAndSelf().Any(
                n => n is MarkupDynamicAttributeValueSyntax or
                          MarkupLiteralAttributeValueSyntax or
                          MarkupTagHelperAttributeValueSyntax);
        }

        bool IsInDirectiveWithNoKind()
        {
            // E.g, (| is position)
            //
            // `@using |System;
            //
            return owner.AncestorsAndSelf().Any(
                n => n is RazorDirectiveSyntax { DirectiveDescriptor: null });
        }

        bool IsAttributeDirective()
        {
            // E.g, (| is position)
            //
            // `@attribute |[System.Obsolete]
            //
            return owner.AncestorsAndSelf().Any(
                n => n is RazorDirectiveSyntax directive &&
                    directive.DirectiveDescriptor != null &&
                    directive.DirectiveDescriptor.Kind == DirectiveKind.SingleLine &&
                    directive.DirectiveDescriptor.Directive.Equals(AttributeDirective.Directive.Directive, StringComparison.Ordinal));
        }

        bool IsTypeParamDirective()
        {
            // E.g, (| is position)
            //
            // `@typeparam |T where T : IDisposable
            //
            return owner.AncestorsAndSelf().Any(
                n => n is RazorDirectiveSyntax directive &&
                    directive.DirectiveDescriptor != null &&
                    directive.DirectiveDescriptor.Kind == DirectiveKind.SingleLine &&
                    directive.DirectiveDescriptor.Directive.Equals(ComponentTypeParamDirective.Directive.Directive, StringComparison.Ordinal));
        }

        bool IsInSingleLineDirective()
        {
            // E.g, (| is position)
            //
            // `@inject |SomeType SomeName` - true
            //
            return owner.AncestorsAndSelf().Any(
                n => n is RazorDirectiveSyntax directive && directive.DirectiveDescriptor.Kind == DirectiveKind.SingleLine);
        }

        bool IsImplicitExpression()
        {
            // E.g, (| is position)
            //
            // `@|foo` - true
            //
            return owner.AncestorsAndSelf().Any(n => n is CSharpImplicitExpressionSyntax);
        }

        bool IsSingleLineExplicitExpression()
        {
            // E.g, (| is position)
            //
            // `|@{ foo }` - true
            //
            if (owner is { Parent.Parent.Parent: CSharpExplicitExpressionSyntax explicitExpression } &&
                context.SourceText.GetRange(explicitExpression.Span) is { } exprRange &&
                exprRange.IsSingleLine())
            {
                return true;
            }

            return owner.AncestorsAndSelf().Any(n => n is CSharpImplicitExpressionSyntax);
        }

        bool IsInTemplateBlock()
        {
            // E.g, (| is position)
            //
            // `RenderFragment(|@<Component>);` - true
            //
            return owner.AncestorsAndSelf().Any(n => n is CSharpTemplateBlockSyntax);
        }

        bool IsInSectionDirectiveBrace()
        {
            // @section Scripts {
            //     <script></script>
            // }
            //
            // Due to how sections are generated (inside a multi-line lambda), we want to exclude the braces
            // from being formatted, or it will be indented by one level due to the lambda. The rest we don't
            // need to worry about, because the one level indent is actually desirable.

            // Due to the Razor tree being so odd, the checks for open and close are surprisingly different

            // Open brace is a child of the C# code block that is the directive itself
            if (owner is RazorMetaCodeSyntax &&
                owner.Parent is CSharpCodeBlockSyntax codeBlock &&
                codeBlock.Children.Count > 3 &&
                owner == codeBlock.Children[3] &&
                // CSharpCodeBlock -> RazorDirectiveBody -> RazorDirective
                codeBlock.Parent?.Parent is RazorDirectiveSyntax directive2 &&
                directive2.DirectiveDescriptor.Directive == SectionDirective.Directive.Directive)
            {
                return true;
            }

            // Close brace is a child of the section content, which is a MarkupBlock
            if (owner is MarkupTextLiteralSyntax &&
                owner.Parent is MarkupBlockSyntax block &&
                owner == block.Children[^1] &&
                // MarkupBlock -> CSharpCodeBlock -> RazorDirectiveBody -> RazorDirective
                block.Parent?.Parent?.Parent is RazorDirectiveSyntax directive &&
                directive.DirectiveDescriptor.Directive == SectionDirective.Directive.Directive)
            {
                return true;
            }

            return false;
        }
    }

    private record struct ShouldFormatOptions(bool AllowImplicitStatements, bool AllowImplicitExpressions, bool AllowSingleLineExplicitExpressions, bool IsLineRequest)
    {
        public ShouldFormatOptions(bool allowImplicitStatements, bool isLineRequest)
            : this(allowImplicitStatements, true, true, isLineRequest)
        {
        }
    }

    private class IndentationData
    {
        private readonly int _offset;
        private int _indentation;
        private bool _lazyLoad;

        public IndentationData(int indentation)
        {
            _indentation = indentation;
        }

        public IndentationData(bool lazyLoad, int offset)
        {
            _lazyLoad = lazyLoad;
            _offset = offset;
        }

        public int GetIndentation(SortedDictionary<int, IndentationData> sourceMappingIndentations, int[] indentationScopes, int minCSharpIndentation)
        {
            // If we're lazy loading, then we need to find the indentation from the source mappings, at the offset,
            // which for whatever reason may not have been available when creating this class.
            if (_lazyLoad)
            {
                _lazyLoad = false;

                var index = Array.BinarySearch(indentationScopes, _offset);
                if (index < 0)
                {
                    index = (~index) - 1;
                }

                // If there is a source mapping to the left of the original start point, then we use its indentation
                // otherwise use the minimum
                _indentation = index < 0
                    ? minCSharpIndentation
                    : sourceMappingIndentations[indentationScopes[index]]._indentation;
            }

            return _indentation;
        }
    }
}
