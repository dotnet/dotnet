// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Language;

internal sealed class DefaultRazorTagHelperContextDiscoveryPhase : RazorEnginePhaseBase
{
    protected override void ExecuteCore(RazorCodeDocument codeDocument)
    {
        var syntaxTree = codeDocument.GetPreTagHelperSyntaxTree() ?? codeDocument.GetSyntaxTree();
        ThrowForMissingDocumentDependency(syntaxTree);

        var descriptors = codeDocument.GetTagHelpers();
        if (descriptors == null)
        {
            var feature = Engine.GetFeature<ITagHelperFeature>();
            if (feature == null)
            {
                // No feature, nothing to do.
                return;
            }

            descriptors = feature.GetDescriptors();
        }

        var parserOptions = codeDocument.GetParserOptions();

        // We need to find directives in all of the *imports* as well as in the main razor file
        //
        // The imports come logically before the main razor file and are in the order they
        // should be processed.
        DirectiveVisitor visitor;
        if (FileKinds.IsComponent(codeDocument.GetFileKind()) &&
            (parserOptions == null || parserOptions.FeatureFlags.AllowComponentFileKind))
        {
            codeDocument.TryComputeNamespace(fallbackToRootNamespace: true, out var currentNamespace);
            visitor = new ComponentDirectiveVisitor(codeDocument.Source.FilePath, descriptors, currentNamespace);
        }
        else
        {
            visitor = new TagHelperDirectiveVisitor(descriptors);
        }
        var imports = codeDocument.GetImportSyntaxTrees();
        if (imports != null)
        {
            for (var i = 0; i < imports.Count; i++)
            {
                var import = imports[i];
                visitor.Visit(import);
            }
        }

        visitor.Visit(syntaxTree);

        // This will always be null for a component document.
        var tagHelperPrefix = visitor.TagHelperPrefix;

        descriptors = visitor.Matches.ToArray();

        var context = TagHelperDocumentContext.Create(tagHelperPrefix, descriptors);
        codeDocument.SetTagHelperContext(context);
        codeDocument.SetPreTagHelperSyntaxTree(syntaxTree);
    }

    private static bool MatchesDirective(TagHelperDescriptor descriptor, string typePattern, string assemblyName)
    {
        if (!string.Equals(descriptor.AssemblyName, assemblyName, StringComparison.Ordinal))
        {
            return false;
        }

        var typePatternSpan = RemoveGlobalPrefix(typePattern.AsSpan());

        if (typePatternSpan[^1] == '*')
        {
            if (typePatternSpan.Length == 1)
            {
                // TypePattern is "*".
                return true;
            }

            return descriptor.Name.AsSpan().StartsWith(typePatternSpan[..^1], StringComparison.Ordinal);
        }

        return descriptor.Name.AsSpan().Equals(typePatternSpan, StringComparison.Ordinal);
    }

    private static ReadOnlySpan<char> RemoveGlobalPrefix(in ReadOnlySpan<char> span)
    {
        const string globalPrefix = "global::";

        if (span.StartsWith(globalPrefix.AsSpan(), StringComparison.Ordinal))
        {
            return span[globalPrefix.Length..];
        }

        return span;
    }

    internal abstract class DirectiveVisitor : SyntaxWalker
    {
        public abstract HashSet<TagHelperDescriptor> Matches { get; }

        public abstract string TagHelperPrefix { get; }

        public abstract void Visit(RazorSyntaxTree tree);
    }

    internal sealed class TagHelperDirectiveVisitor : DirectiveVisitor
    {
        private readonly List<TagHelperDescriptor> _tagHelpers;
        private string _tagHelperPrefix;

        public TagHelperDirectiveVisitor(IReadOnlyList<TagHelperDescriptor> tagHelpers)
        {
            // We don't want to consider components in a view document.
            _tagHelpers = new();
            for (var i = 0; i < tagHelpers.Count; i++)
            {
                var tagHelper = tagHelpers[i];
                if (!tagHelper.IsAnyComponentDocumentTagHelper())
                {
                    _tagHelpers.Add(tagHelper);
                }
            }
        }

        public override string TagHelperPrefix => _tagHelperPrefix;

        public override HashSet<TagHelperDescriptor> Matches { get; } = new HashSet<TagHelperDescriptor>();

        public override void Visit(RazorSyntaxTree tree)
        {
            Visit(tree.Root);
        }

        public override void VisitRazorDirective(RazorDirectiveSyntax node)
        {
            var descendantLiterals = node.DescendantNodes();
            foreach (var child in descendantLiterals)
            {
                if (child is not CSharpStatementLiteralSyntax literal)
                {
                    continue;
                }

                switch (literal.ChunkGenerator)
                {
                    case null:
                        continue;
                    case AddTagHelperChunkGenerator addTagHelper:
                        if (addTagHelper.AssemblyName == null)
                        {
                            // Skip this one, it's an error
                            continue;
                        }

                        if (!AssemblyContainsTagHelpers(addTagHelper.AssemblyName, _tagHelpers))
                        {
                            // No tag helpers in the assembly.
                            continue;
                        }

                        for (var i = 0; i < _tagHelpers.Count; i++)
                        {
                            var tagHelper = _tagHelpers[i];
                            if (MatchesDirective(tagHelper, addTagHelper.TypePattern, addTagHelper.AssemblyName))
                            {
                                Matches.Add(tagHelper);
                            }
                        }
                        break;
                    case RemoveTagHelperChunkGenerator removeTagHelper:
                        if (removeTagHelper.AssemblyName == null)
                        {
                            // Skip this one, it's an error
                            continue;
                        }


                        if (!AssemblyContainsTagHelpers(removeTagHelper.AssemblyName, _tagHelpers))
                        {
                            // No tag helpers in the assembly.
                            continue;
                        }

                        for (var i = 0; i < _tagHelpers.Count; i++)
                        {
                            var tagHelper = _tagHelpers[i];
                            if (MatchesDirective(tagHelper, removeTagHelper.TypePattern, removeTagHelper.AssemblyName))
                            {
                                Matches.Remove(tagHelper);
                            }
                        }
                        break;
                    case TagHelperPrefixDirectiveChunkGenerator tagHelperPrefix:
                        if (!string.IsNullOrEmpty(tagHelperPrefix.DirectiveText))
                        {
                            // We only expect to see a single one of these per file, but that's enforced at another level.
                            _tagHelperPrefix = tagHelperPrefix.DirectiveText;
                        }
                        break;
                }
            }
        }

        private bool AssemblyContainsTagHelpers(string assemblyName, IReadOnlyList<TagHelperDescriptor> tagHelpers)
        {
            for (var i = 0; i < tagHelpers.Count; i++)
            {
                if (string.Equals(tagHelpers[i].AssemblyName, assemblyName, StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }
    }

    internal sealed class ComponentDirectiveVisitor : DirectiveVisitor
    {
        private readonly ImmutableArray<TagHelperDescriptor> _notFullyQualifiedComponents;
        private readonly string _filePath;
        private RazorSourceDocument _source;

        public ComponentDirectiveVisitor(string filePath, IReadOnlyList<TagHelperDescriptor> tagHelpers, string currentNamespace)
        {
            _filePath = filePath;

            using var builder = new PooledArrayBuilder<TagHelperDescriptor>(capacity: tagHelpers.Count);

            for (var i = 0; i < tagHelpers.Count; i++)
            {
                var tagHelper = tagHelpers[i];
                // We don't want to consider non-component tag helpers in a component document.
                if (!tagHelper.IsAnyComponentDocumentTagHelper() || IsTagHelperFromMangledClass(tagHelper))
                {
                    continue;
                }

                if (tagHelper.IsComponentFullyQualifiedNameMatch())
                {
                    // If the component descriptor matches for a fully qualified name, using directives shouldn't matter.
                    Matches.Add(tagHelper);
                    continue;
                }

                builder.Add(tagHelper);

                if (currentNamespace is null)
                {
                    continue;
                }

                if (tagHelper.IsChildContentTagHelper())
                {
                    // If this is a child content tag helper, we want to add it if it's original type is in scope.
                    // E.g, if the type name is `Test.MyComponent.ChildContent`, we want to add it if `Test.MyComponent` is in scope.
                    if (IsTypeInScope(tagHelper, currentNamespace))
                    {
                        Matches.Add(tagHelper);
                    }
                }
                else if (IsTypeInScope(tagHelper, currentNamespace))
                {
                    // Also, if the type is already in scope of the document's namespace, using isn't necessary.
                    Matches.Add(tagHelper);
                }
            }

            _notFullyQualifiedComponents = builder.DrainToImmutable();
        }

        public override HashSet<TagHelperDescriptor> Matches { get; } = new HashSet<TagHelperDescriptor>();

        // There is no support for tag helper prefix in component documents.
        public override string TagHelperPrefix => null;

        public override void Visit(RazorSyntaxTree tree)
        {
            _source = tree.Source;
            Visit(tree.Root);
        }

        public override void VisitRazorDirective(RazorDirectiveSyntax node)
        {
            var descendantLiterals = node.DescendantNodes();
            foreach (var child in descendantLiterals)
            {
                if (child is not CSharpStatementLiteralSyntax literal)
                {
                    continue;
                }

                switch (literal.ChunkGenerator)
                {
                    case null:
                        // We can't find a chunk generator.
                        continue;
                    case AddTagHelperChunkGenerator addTagHelper:
                        // Make sure this node exists in the file we're parsing and not in its imports.
                        if (_filePath.Equals(_source.FilePath, StringComparison.Ordinal))
                        {
                            addTagHelper.Diagnostics.Add(
                                ComponentDiagnosticFactory.Create_UnsupportedTagHelperDirective(node.GetSourceSpan(_source)));
                        }
                        break;
                    case RemoveTagHelperChunkGenerator removeTagHelper:
                        // Make sure this node exists in the file we're parsing and not in its imports.
                        if (_filePath.Equals(_source.FilePath, StringComparison.Ordinal))
                        {
                            removeTagHelper.Diagnostics.Add(
                                ComponentDiagnosticFactory.Create_UnsupportedTagHelperDirective(node.GetSourceSpan(_source)));
                        }
                        break;
                    case TagHelperPrefixDirectiveChunkGenerator tagHelperPrefix:
                        // Make sure this node exists in the file we're parsing and not in its imports.
                        if (_filePath.Equals(_source.FilePath, StringComparison.Ordinal))
                        {
                            tagHelperPrefix.Diagnostics.Add(
                                ComponentDiagnosticFactory.Create_UnsupportedTagHelperDirective(node.GetSourceSpan(_source)));
                        }
                        break;
                    case AddImportChunkGenerator { IsStatic: false } usingStatement:
                        // Get the namespace from the using statement.
                        var @namespace = usingStatement.ParsedNamespace;
                        if (@namespace.IndexOf('=') != -1)
                        {
                            // We don't support usings with alias.
                            continue;
                        }

                        foreach (var tagHelper in _notFullyQualifiedComponents)
                        {
                            Debug.Assert(!tagHelper.IsComponentFullyQualifiedNameMatch(), "We've already processed these.");

                            if (tagHelper.IsChildContentTagHelper())
                            {
                                // If this is a child content tag helper, we want to add it if it's original type is in scope of the given namespace.
                                // E.g, if the type name is `Test.MyComponent.ChildContent`, we want to add it if `Test.MyComponent` is in this namespace.
                                if (IsTypeInNamespace(tagHelper, @namespace))
                                {
                                    Matches.Add(tagHelper);
                                }
                            }
                            else if (IsTypeInNamespace(tagHelper, @namespace))
                            {
                                // If the type is at the top-level or if the type's namespace matches the using's namespace, add it.
                                Matches.Add(tagHelper);
                            }
                        }
                        break;
                }
            }
        }

        internal static bool IsTypeInNamespace(TagHelperDescriptor tagHelper, string @namespace)
        {
            return IsTypeInNamespace(tagHelper.GetTypeNamespace(), @namespace);

            static bool IsTypeInNamespace(string typeNamespace, string @namespace)
            {
                if (string.IsNullOrEmpty(typeNamespace))
                {
                    // Either the typeName is not the full type name or this type is at the top level.
                    return true;
                }

                // Remove global:: prefix from namespace.
                var normalizedNamespace = RemoveGlobalPrefix(@namespace.AsSpan());

                return normalizedNamespace.Equals(typeNamespace.AsSpan(), StringComparison.Ordinal);
            }
        }

        // Check if the given type is already in scope given the namespace of the current document.
        // E.g,
        // If the namespace of the document is `MyComponents.Components.Shared`,
        // then the types `MyComponents.FooComponent`, `MyComponents.Components.BarComponent`, `MyComponents.Components.Shared.BazComponent` are all in scope.
        // Whereas `MyComponents.SomethingElse.OtherComponent` is not in scope.
        internal static bool IsTypeInScope(TagHelperDescriptor descriptor, string currentNamespace)
        {
            return IsTypeInScopeCore(currentNamespace, descriptor.GetTypeNamespace());
        }

        private static bool IsTypeInScopeCore(string currentNamespace, string typeNamespace)
        {
            if (string.IsNullOrEmpty(typeNamespace))
            {
                // Either the typeName is not the full type name or this type is at the top level.
                return true;
            }

            if (!currentNamespace.StartsWith(typeNamespace, StringComparison.Ordinal))
            {
                // typeName: MyComponents.Shared.SomeCoolNamespace
                // currentNamespace: MyComponents.Shared
                return false;
            }

            if (typeNamespace.Length > currentNamespace.Length && typeNamespace[currentNamespace.Length] != '.')
            {
                // typeName: MyComponents.SharedFoo
                // currentNamespace: MyComponent.Shared
                return false;
            }

            return true;
        }

        // We need to filter out the duplicate tag helper descriptors that come from the
        // open file in the editor. We mangle the class name for its generated code, so using that here to filter these out.
        internal static bool IsTagHelperFromMangledClass(TagHelperDescriptor tagHelper)
        {
            return ComponentMetadata.IsMangledClass(tagHelper.GetTypeNameIdentifier());
        }
    }
}
