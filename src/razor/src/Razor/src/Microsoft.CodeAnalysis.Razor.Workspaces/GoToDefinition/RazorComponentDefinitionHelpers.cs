// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using LspRange = Microsoft.VisualStudio.LanguageServer.Protocol.Range;
using RazorSyntaxKind = Microsoft.AspNetCore.Razor.Language.SyntaxKind;
using RazorSyntaxNode = Microsoft.AspNetCore.Razor.Language.Syntax.SyntaxNode;
using RazorSyntaxToken = Microsoft.AspNetCore.Razor.Language.Syntax.SyntaxToken;

namespace Microsoft.CodeAnalysis.Razor.GoToDefinition;

internal static class RazorComponentDefinitionHelpers
{
    public static bool TryGetBoundTagHelpers(
        RazorCodeDocument codeDocument, int absoluteIndex, bool ignoreAttributes, ILogger logger,
        [NotNullWhen(true)] out TagHelperDescriptor? boundTagHelper,
        [MaybeNullWhen(true)] out BoundAttributeDescriptor? boundAttribute)
    {
        boundTagHelper = null;
        boundAttribute = null;

        var syntaxTree = codeDocument.GetSyntaxTree();

        var innermostNode = syntaxTree.Root.FindInnermostNode(absoluteIndex);
        if (innermostNode is null)
        {
            logger.LogInformation($"Could not locate innermost node at index, {absoluteIndex}.");
            return false;
        }

        var tagHelperNode = innermostNode.FirstAncestorOrSelf<RazorSyntaxNode>(IsTagHelperNode);
        if (tagHelperNode is null)
        {
            logger.LogInformation($"Could not locate ancestor of type MarkupTagHelperStartTag or MarkupTagHelperEndTag.");
            return false;
        }

        if (!TryGetTagName(tagHelperNode, out var tagName))
        {
            logger.LogInformation($"Could not retrieve name of start or end tag.");
            return false;
        }

        var nameSpan = tagName.Span;
        string? propertyName = null;

        if (!ignoreAttributes && tagHelperNode is MarkupTagHelperStartTagSyntax startTag)
        {
            // Include attributes where the end index also matches, since GetSyntaxNodeAsync will consider that the start tag but we behave
            // as if the user wants to go to the attribute definition.
            // ie: <Component attribute$$></Component>
            var selectedAttribute = startTag.Attributes.FirstOrDefault(a => a.Span.Contains(absoluteIndex) || a.Span.End == absoluteIndex);

            // If we're on an attribute then just validate against the attribute name
            switch (selectedAttribute)
            {
                case MarkupTagHelperAttributeSyntax attribute:
                    // Normal attribute, ie <Component attribute=value />
                    nameSpan = attribute.Name.Span;
                    propertyName = attribute.TagHelperAttributeInfo.Name;
                    break;

                case MarkupMinimizedTagHelperAttributeSyntax minimizedAttribute:
                    // Minimized attribute, ie <Component attribute />
                    nameSpan = minimizedAttribute.Name.Span;
                    propertyName = minimizedAttribute.TagHelperAttributeInfo.Name;
                    break;
            }
        }

        if (!nameSpan.IntersectsWith(absoluteIndex))
        {
            logger.LogInformation($"Tag name or attributes' span does not intersect with index, {absoluteIndex}.");
            return false;
        }

        if (tagHelperNode.Parent is not MarkupTagHelperElementSyntax tagHelperElement)
        {
            logger.LogInformation($"Parent of start or end tag is not a MarkupTagHelperElement.");
            return false;
        }

        if (tagHelperElement.TagHelperInfo?.BindingResult is not TagHelperBinding binding)
        {
            logger.LogInformation($"MarkupTagHelperElement does not contain TagHelperInfo.");
            return false;
        }

        boundTagHelper = binding.Descriptors.FirstOrDefault(static d => !d.IsAttributeDescriptor());
        if (boundTagHelper is null)
        {
            logger.LogInformation($"Could not locate bound TagHelperDescriptor.");
            return false;
        }

        boundAttribute = propertyName is not null
            ? boundTagHelper.BoundAttributes.FirstOrDefault(a => a.Name?.Equals(propertyName, StringComparison.Ordinal) == true)
            : null;

        return true;

        static bool IsTagHelperNode(RazorSyntaxNode node)
        {
            return node.Kind is RazorSyntaxKind.MarkupTagHelperStartTag or RazorSyntaxKind.MarkupTagHelperEndTag;
        }

        static bool TryGetTagName(RazorSyntaxNode node, [NotNullWhen(true)] out RazorSyntaxToken? tagName)
        {
            tagName = node switch
            {
                MarkupTagHelperStartTagSyntax tagHelperStartTag => tagHelperStartTag.Name,
                MarkupTagHelperEndTagSyntax tagHelperEndTag => tagHelperEndTag.Name,
                _ => null
            };

            return tagName is not null;
        }
    }

    public static async Task<LspRange?> TryGetPropertyRangeAsync(
        IDocumentSnapshot documentSnapshot,
        string propertyName,
        IDocumentMappingService documentMappingService,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        // Process the C# tree and find the property that matches the name.
        // We don't worry about parameter attributes here for two main reasons:
        //   1. We don't have symbolic information, so the best we could do would be checking for any
        //      attribute named Parameter, regardless of which namespace. It also means we would have
        //      to do more checks for all of the various ways that the attribute could be specified
        //      (eg fully qualified, aliased, etc.)
        //   2. Since C# doesn't allow multiple properties with the same name, and we're doing a case
        //      sensitive search, we know the property we find is the one the user is trying to encode in a
        //      tag helper attribute. If they don't have the [Parameter] attribute then the Razor compiler
        //      will error, but allowing them to Go To Def on that property regardless, actually helps
        //      them fix the error.

        var csharpSyntaxTree = await documentSnapshot.GetCSharpSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
        var root = await csharpSyntaxTree.GetRootAsync(cancellationToken).ConfigureAwait(false);
        var codeDocument = await documentSnapshot.GetGeneratedOutputAsync(cancellationToken).ConfigureAwait(false);

        // Since we know how the compiler generates the C# source we can be a little specific here, and avoid
        // long tree walks. If the compiler ever changes how they generate their code, the tests for this will break
        // so we'll know about it.
        if (TryGetClassDeclaration(root, out var classDeclaration))
        {
            var property = classDeclaration
                .Members
                .OfType<PropertyDeclarationSyntax>()
                .Where(p => p.Identifier.ValueText.Equals(propertyName, StringComparison.Ordinal))
                .FirstOrDefault();

            if (property is null)
            {
                // The property probably exists in a partial class
                logger.LogInformation($"Could not find property in the generated source. Comes from partial?");
                return null;
            }

            var csharpText = codeDocument.GetCSharpSourceText();
            var range = csharpText.GetRange(property.Identifier.Span);
            if (documentMappingService.TryMapToHostDocumentRange(codeDocument.GetCSharpDocument(), range, out var originalRange))
            {
                return originalRange;
            }

            logger.LogInformation($"Property found but couldn't map its location.");
        }

        logger.LogInformation($"Generated C# was not in expected shape (CompilationUnit [-> Namespace] -> Class)");

        return null;

        static bool TryGetClassDeclaration(SyntaxNode root, [NotNullWhen(true)] out ClassDeclarationSyntax? classDeclaration)
        {
            classDeclaration = root switch
            {
                CompilationUnitSyntax unit => unit switch
                {
                    { Members: [NamespaceDeclarationSyntax { Members: [ClassDeclarationSyntax c, ..] }, ..] } => c,
                    { Members: [ClassDeclarationSyntax c, ..] } => c,
                    _ => null,
                },
                _ => null,
            };

            return classDeclaration is not null;
        }
    }
}
