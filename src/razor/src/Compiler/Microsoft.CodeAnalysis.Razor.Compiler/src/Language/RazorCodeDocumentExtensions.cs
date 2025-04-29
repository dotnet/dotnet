// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Language;

public static class RazorCodeDocumentExtensions
{
    private static readonly char[] PathSeparators = ['/', '\\'];
    private static readonly char[] NamespaceSeparators = ['.'];
    private static readonly object CssScopeKey = new();
    private static readonly object NamespaceKey = new();

    internal static TagHelperDocumentContext GetTagHelperContext(this RazorCodeDocument document)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        return (TagHelperDocumentContext)document.Items[typeof(TagHelperDocumentContext)];
    }

    internal static void SetTagHelperContext(this RazorCodeDocument document, TagHelperDocumentContext context)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        document.Items[typeof(TagHelperDocumentContext)] = context;
    }

    internal static IReadOnlyList<TagHelperDescriptor> GetTagHelpers(this RazorCodeDocument document)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        return (document.Items[typeof(TagHelpersHolder)] as TagHelpersHolder)?.TagHelpers;
    }

    internal static void SetTagHelpers(this RazorCodeDocument document, IReadOnlyList<TagHelperDescriptor> tagHelpers)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        document.Items[typeof(TagHelpersHolder)] = new TagHelpersHolder(tagHelpers);
    }

    internal static ISet<TagHelperDescriptor> GetReferencedTagHelpers(this RazorCodeDocument document)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        return document.Items[nameof(GetReferencedTagHelpers)] as ISet<TagHelperDescriptor>;
    }

    internal static void SetReferencedTagHelpers(this RazorCodeDocument document, ISet<TagHelperDescriptor> tagHelpers)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        document.Items[nameof(GetReferencedTagHelpers)] = tagHelpers;
    }

    public static RazorSyntaxTree GetPreTagHelperSyntaxTree(this RazorCodeDocument document)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        return document.Items[nameof(GetPreTagHelperSyntaxTree)] as RazorSyntaxTree;
    }

    public static void SetPreTagHelperSyntaxTree(this RazorCodeDocument document, RazorSyntaxTree syntaxTree)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        document.Items[nameof(GetPreTagHelperSyntaxTree)] = syntaxTree;
    }

    public static RazorSyntaxTree GetSyntaxTree(this RazorCodeDocument document)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        return document.Items[typeof(RazorSyntaxTree)] as RazorSyntaxTree;
    }

    public static void SetSyntaxTree(this RazorCodeDocument document, RazorSyntaxTree syntaxTree)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        document.Items[typeof(RazorSyntaxTree)] = syntaxTree;
    }

    public static ImmutableArray<RazorSyntaxTree> GetImportSyntaxTrees(this RazorCodeDocument document)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        return (document.Items[typeof(ImportSyntaxTreesHolder)] as ImportSyntaxTreesHolder)?.SyntaxTrees ?? default;
    }

    public static void SetImportSyntaxTrees(this RazorCodeDocument document, ImmutableArray<RazorSyntaxTree> syntaxTrees)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        if (syntaxTrees.IsDefault)
        {
            throw new ArgumentException("", nameof(syntaxTrees));
        }

        document.Items[typeof(ImportSyntaxTreesHolder)] = new ImportSyntaxTreesHolder(syntaxTrees);
    }

    public static DocumentIntermediateNode GetDocumentIntermediateNode(this RazorCodeDocument document)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        return document.Items[typeof(DocumentIntermediateNode)] as DocumentIntermediateNode;
    }

    public static void SetDocumentIntermediateNode(this RazorCodeDocument document, DocumentIntermediateNode documentNode)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        document.Items[typeof(DocumentIntermediateNode)] = documentNode;
    }

    internal static RazorHtmlDocument GetHtmlDocument(this RazorCodeDocument codeDocument)
    {
        ArgHelper.ThrowIfNull(codeDocument);

        var razorHtmlObj = codeDocument.Items[typeof(RazorHtmlDocument)];
        if (razorHtmlObj == null)
        {
            var razorHtmlDocument = RazorHtmlWriter.GetHtmlDocument(codeDocument);
            codeDocument.Items[typeof(RazorHtmlDocument)] = razorHtmlDocument;
            return razorHtmlDocument;
        }

        return (RazorHtmlDocument)razorHtmlObj;
    }

    public static RazorCSharpDocument GetCSharpDocument(this RazorCodeDocument document)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        return (RazorCSharpDocument)document.Items[typeof(RazorCSharpDocument)];
    }

    public static void SetCSharpDocument(this RazorCodeDocument document, RazorCSharpDocument csharp)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        document.Items[typeof(RazorCSharpDocument)] = csharp;
    }

    public static string GetCssScope(this RazorCodeDocument document)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        return (string)document.Items[CssScopeKey];
    }

    public static void SetCssScope(this RazorCodeDocument document, string cssScope)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        document.Items[CssScopeKey] = cssScope;
    }

#nullable enable
    public static bool TryComputeClassName(this RazorCodeDocument codeDocument, [NotNullWhen(true)] out string? className)
    {
        var filePath = codeDocument.Source.RelativePath ?? codeDocument.Source.FilePath;
        if (filePath.IsNullOrEmpty())
        {
            className = null;
            return false;
        }

        className = CSharpIdentifier.GetClassNameFromPath(filePath);
        return className is not null;
    }
#nullable disable

    public static bool TryComputeNamespace(this RazorCodeDocument codeDocument, bool fallbackToRootNamespace, out string @namespace)
        => TryComputeNamespace(codeDocument, fallbackToRootNamespace, out @namespace, out _);

    // In general documents will have a relative path (relative to the project root).
    // We can only really compute a nice namespace when we know a relative path.
    //
    // However all kinds of thing are possible in tools. We shouldn't barf here if the document isn't
    // set up correctly.
    public static bool TryComputeNamespace(this RazorCodeDocument codeDocument, bool fallbackToRootNamespace, out string @namespace, out SourceSpan? namespaceSpan)
    {
        ArgHelper.ThrowIfNull(codeDocument);

        var cachedNsInfo = codeDocument.Items[NamespaceKey];
        if (cachedNsInfo is not null)
        {
            (@namespace, namespaceSpan) = ((string, SourceSpan?))cachedNsInfo;
        }
        else
        {
            var result = TryComputeNamespaceCore(codeDocument, fallbackToRootNamespace, out @namespace, out namespaceSpan);
            if (result)
            {
                codeDocument.Items[NamespaceKey] = (@namespace, namespaceSpan);
            }
            return result;
        }

#if DEBUG
        // In debug mode, even if we're cached, lets take the hit to run this again and make sure the cached value is correct.
        // This is to help us find issues with caching logic during development.
        var validateResult = TryComputeNamespaceCore(codeDocument, fallbackToRootNamespace, out var validateNamespace, out _);
        Debug.Assert(validateResult, "We couldn't compute the namespace, but have a cached value, so something has gone wrong");
        Debug.Assert(validateNamespace == @namespace, $"We cached a namespace of {@namespace} but calculated that it should be {validateNamespace}");
#endif

        return true;

        bool TryComputeNamespaceCore(RazorCodeDocument codeDocument, bool fallbackToRootNamespace, out string @namespace, out SourceSpan? namespaceSpan)
        {
            var filePath = codeDocument.Source.FilePath;
            if (filePath == null || codeDocument.Source.RelativePath == null || filePath.Length < codeDocument.Source.RelativePath.Length)
            {
                @namespace = null;
                namespaceSpan = null;
                return false;
            }

            // If the document or it's imports contains a @namespace directive, we want to use that over the root namespace.
            var baseNamespace = string.Empty;
            var appendSuffix = true;
            var lastNamespaceContent = string.Empty;
            namespaceSpan = null;

            if (codeDocument.GetImportSyntaxTrees() is { IsDefault: false } importSyntaxTrees)
            {
                // ImportSyntaxTrees is usually set. Just being defensive.
                foreach (var importSyntaxTree in importSyntaxTrees)
                {
                    if (importSyntaxTree != null && NamespaceVisitor.TryGetLastNamespaceDirective(importSyntaxTree, out var importNamespaceContent, out var importNamespaceLocation))
                    {
                        lastNamespaceContent = importNamespaceContent;
                        namespaceSpan = importNamespaceLocation;
                    }
                }
            }

            var syntaxTree = codeDocument.GetSyntaxTree();
            if (syntaxTree != null && NamespaceVisitor.TryGetLastNamespaceDirective(syntaxTree, out var namespaceContent, out var namespaceLocation))
            {
                lastNamespaceContent = namespaceContent;
                namespaceSpan = namespaceLocation;
            }

            var relativePath = codeDocument.Source.RelativePath.AsSpan();

            // If there are multiple @namespace directives in the hierarchy,
            // we want to pick the closest one to the current document.
            if (!string.IsNullOrEmpty(lastNamespaceContent))
            {
                baseNamespace = lastNamespaceContent;
                var directiveLocationDirectory = NormalizeDirectory(namespaceSpan.Value.FilePath);

                var sourceFilePath = codeDocument.Source.FilePath.AsSpan();
                // We're specifically using OrdinalIgnoreCase here because Razor treats all paths as case-insensitive.
                if (!sourceFilePath.StartsWith(directiveLocationDirectory, StringComparison.OrdinalIgnoreCase) ||
                    sourceFilePath.Length <= directiveLocationDirectory.Length)
                {
                    // The most relevant directive is not from the directory hierarchy, can't compute a suffix.
                    appendSuffix = false;
                }
                else
                {
                    // We know that the document containing the namespace directive is in the current document's hierarchy.
                    // Let's compute the actual relative path that we'll use to compute the namespace suffix.
                    relativePath = sourceFilePath.Slice(directiveLocationDirectory.Length);
                }
            }
            else if (fallbackToRootNamespace)
            {
                baseNamespace = codeDocument.CodeGenerationOptions.RootNamespace;
                appendSuffix = true;

                // Empty RootNamespace is allowed only in components.
                if (!FileKinds.IsComponent(codeDocument.FileKind) && string.IsNullOrEmpty(baseNamespace))
                {
                    @namespace = null;
                    return false;
                }
            }
            else
            {
                // There was no valid @namespace directive.
                @namespace = null;
                return false;
            }

            using var _ = StringBuilderPool.GetPooledObject(out var builder);

            // Sanitize the base namespace, but leave the dots.
            var segments = new StringTokenizer(baseNamespace, NamespaceSeparators);
            var first = true;
            foreach (var token in segments)
            {
                if (token.IsEmpty)
                {
                    continue;
                }

                if (first)
                {
                    first = false;
                }
                else
                {
                    builder.Append('.');
                }

                CSharpIdentifier.AppendSanitized(builder, token);
            }

            if (appendSuffix)
            {
                // If we get here, we already have a base namespace and the relative path that should be used as the namespace suffix.
                segments = new StringTokenizer(relativePath, PathSeparators);
                var previousLength = builder.Length;
                foreach (var token in segments)
                {
                    if (token.IsEmpty)
                    {
                        continue;
                    }

                    previousLength = builder.Length;

                    if (previousLength != 0)
                    {
                        builder.Append('.');
                    }

                    CSharpIdentifier.AppendSanitized(builder, token);
                }

                // Trim the last segment because it's the FileName.
                builder.Length = previousLength;
            }

            @namespace = builder.ToString();

            return true;
        }

        // We want to normalize the path of the file containing the '@namespace' directive to just the containing
        // directory with a trailing separator.
        //
        // Not using Path.GetDirectoryName here because it doesn't meet these requirements, and we want to handle
        // both 'view engine' style paths and absolute paths.
        //
        // We also don't normalize the separators here. We expect that all documents are using a consistent style of path.
        //
        // If we can't normalize the path, we just return null so it will be ignored.
        ReadOnlySpan<char> NormalizeDirectory(string path)
        {
            var span = path.AsSpanOrDefault();

            if (span.IsEmpty)
            {
                return default;
            }

            var lastSeparator = span.LastIndexOfAny(PathSeparators);
            if (lastSeparator < 0)
            {
                return default;
            }

            // Includes the separator
            return span[..(lastSeparator + 1)];
        }
    }

    private record class ImportSyntaxTreesHolder(ImmutableArray<RazorSyntaxTree> SyntaxTrees);

    private class IncludeSyntaxTreesHolder
    {
        public IncludeSyntaxTreesHolder(IReadOnlyList<RazorSyntaxTree> syntaxTrees)
        {
            SyntaxTrees = syntaxTrees;
        }

        public IReadOnlyList<RazorSyntaxTree> SyntaxTrees { get; }
    }

    private class TagHelpersHolder
    {
        public TagHelpersHolder(IReadOnlyList<TagHelperDescriptor> tagHelpers)
        {
            TagHelpers = tagHelpers;
        }

        public IReadOnlyList<TagHelperDescriptor> TagHelpers { get; }
    }

    private class NamespaceVisitor : SyntaxWalker
    {
        private readonly RazorSourceDocument _source;

        private NamespaceVisitor(RazorSourceDocument source)
        {
            _source = source;
        }

        public string LastNamespaceContent { get; set; }

        public SourceSpan LastNamespaceLocation { get; set; }

        public static bool TryGetLastNamespaceDirective(
            RazorSyntaxTree syntaxTree,
            out string namespaceDirectiveContent,
            out SourceSpan namespaceDirectiveSpan)
        {
            var visitor = new NamespaceVisitor(syntaxTree.Source);
            visitor.Visit(syntaxTree.Root);
            if (string.IsNullOrEmpty(visitor.LastNamespaceContent))
            {
                namespaceDirectiveContent = null;
                namespaceDirectiveSpan = SourceSpan.Undefined;
                return false;
            }

            namespaceDirectiveContent = visitor.LastNamespaceContent;
            namespaceDirectiveSpan = visitor.LastNamespaceLocation;
            return true;
        }

        public override void VisitRazorDirective(RazorDirectiveSyntax node)
        {
            if (node != null && node.DirectiveDescriptor == NamespaceDirective.Directive)
            {
                if (node.Body?.ChildNodes() is [_, CSharpCodeBlockSyntax { Children: [_, CSharpSyntaxNode @namespace, ..] }])
                {
                    LastNamespaceContent = @namespace.GetContent();
                    LastNamespaceLocation = @namespace.GetSourceSpan(_source);
                }
            }

            base.VisitRazorDirective(node);
        }
    }
}
