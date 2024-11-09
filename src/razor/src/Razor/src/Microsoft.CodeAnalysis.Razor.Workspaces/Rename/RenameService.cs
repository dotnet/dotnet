// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using RazorSyntaxKind = Microsoft.AspNetCore.Razor.Language.SyntaxKind;
using RazorSyntaxNode = Microsoft.AspNetCore.Razor.Language.Syntax.SyntaxNode;

namespace Microsoft.CodeAnalysis.Razor.Rename;

internal class RenameService(
    IRazorComponentSearchEngine componentSearchEngine,
    LanguageServerFeatureOptions languageServerFeatureOptions) : IRenameService
{
    private readonly IRazorComponentSearchEngine _componentSearchEngine = componentSearchEngine;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions;

    public async Task<WorkspaceEdit?> TryGetRazorRenameEditsAsync(
        DocumentContext documentContext,
        DocumentPositionInfo positionInfo,
        string newName,
        ISolutionQueryOperations solutionQueryOperations,
        CancellationToken cancellationToken)
    {
        // We only support renaming of .razor components, not .cshtml tag helpers
        if (!FileKinds.IsComponent(documentContext.FileKind))
        {
            return null;
        }

        // If we're in C# then there is no point checking for a component tag, because there won't be one
        if (positionInfo.LanguageKind == RazorLanguageKind.CSharp)
        {
            return null;
        }

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        var originTagHelpers = await GetOriginTagHelpersAsync(documentContext, positionInfo.HostDocumentIndex, cancellationToken).ConfigureAwait(false);
        if (originTagHelpers.IsDefaultOrEmpty)
        {
            return null;
        }

        var originComponentDocumentSnapshot = await _componentSearchEngine
            .TryLocateComponentAsync(originTagHelpers.First(), solutionQueryOperations, cancellationToken)
            .ConfigureAwait(false);
        if (originComponentDocumentSnapshot is null)
        {
            return null;
        }

        var originComponentDocumentFilePath = originComponentDocumentSnapshot.FilePath;
        var newPath = MakeNewPath(originComponentDocumentFilePath, newName);
        if (File.Exists(newPath))
        {
            return null;
        }

        using var _ = ListPool<SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>>.GetPooledObject(out var documentChanges);
        var fileRename = GetFileRenameForComponent(originComponentDocumentSnapshot, newPath);
        documentChanges.Add(fileRename);
        AddEditsForCodeDocument(documentChanges, originTagHelpers, newName, documentContext.Uri, codeDocument);

        var documentSnapshots = GetAllDocumentSnapshots(documentContext.FilePath, solutionQueryOperations);

        foreach (var documentSnapshot in documentSnapshots)
        {
            await AddEditsForCodeDocumentAsync(documentChanges, originTagHelpers, newName, documentSnapshot, cancellationToken).ConfigureAwait(false);
        }

        foreach (var documentChange in documentChanges)
        {
            if (documentChange.TryGetFirst(out var textDocumentEdit) &&
                textDocumentEdit.TextDocument.Uri == fileRename.OldUri)
            {
                textDocumentEdit.TextDocument.Uri = fileRename.NewUri;
            }
        }

        return new WorkspaceEdit
        {
            DocumentChanges = documentChanges.ToArray(),
        };
    }

    private static ImmutableArray<IDocumentSnapshot> GetAllDocumentSnapshots(string filePath, ISolutionQueryOperations solutionQueryOperations)
    {
        using var documentSnapshots = new PooledArrayBuilder<IDocumentSnapshot>();
        using var _ = StringHashSetPool.GetPooledObject(out var documentPaths);

        foreach (var project in solutionQueryOperations.GetProjects())
        {
            foreach (var documentPath in project.DocumentFilePaths)
            {
                // We've already added refactoring edits for our document snapshot
                if (FilePathComparer.Instance.Equals(documentPath, filePath))
                {
                    continue;
                }

                // Don't add duplicates between projects
                if (!documentPaths.Add(documentPath))
                {
                    continue;
                }

                // Add to the list and add the path to the set
                if (!project.TryGetDocument(documentPath, out var snapshot))
                {
                    throw new InvalidOperationException($"{documentPath} in project {project.FilePath} but not retrievable");
                }

                documentSnapshots.Add(snapshot);
            }
        }

        return documentSnapshots.DrainToImmutable();
    }

    private RenameFile GetFileRenameForComponent(IDocumentSnapshot documentSnapshot, string newPath)
        => new RenameFile
        {
            OldUri = BuildUri(documentSnapshot.FilePath),
            NewUri = BuildUri(newPath),
        };

    private Uri BuildUri(string filePath)
    {
        // VS Code in Windows expects path to start with '/'
        var updatedPath = _languageServerFeatureOptions.ReturnCodeActionAndRenamePathsWithPrefixedSlash && !filePath.StartsWith("/")
                    ? '/' + filePath
                    : filePath;
        var oldUri = new UriBuilder
        {
            Path = updatedPath,
            Host = string.Empty,
            Scheme = Uri.UriSchemeFile,
        }.Uri;
        return oldUri;
    }

    private static string MakeNewPath(string originalPath, string newName)
    {
        var newFileName = $"{newName}{Path.GetExtension(originalPath)}";
        var directoryName = Path.GetDirectoryName(originalPath).AssumeNotNull();
        return Path.Combine(directoryName, newFileName);
    }

    private async Task AddEditsForCodeDocumentAsync(
        List<SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>> documentChanges,
        ImmutableArray<TagHelperDescriptor> originTagHelpers,
        string newName,
        IDocumentSnapshot documentSnapshot,
        CancellationToken cancellationToken)
    {
        if (!FileKinds.IsComponent(documentSnapshot.FileKind))
        {
            return;
        }

        var codeDocument = await documentSnapshot.GetGeneratedOutputAsync(cancellationToken).ConfigureAwait(false);
        if (codeDocument.IsUnsupported())
        {
            return;
        }

        // VS Code in Windows expects path to start with '/'
        var uri = BuildUri(documentSnapshot.FilePath);

        AddEditsForCodeDocument(documentChanges, originTagHelpers, newName, uri, codeDocument);
    }

    private static void AddEditsForCodeDocument(
        List<SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>> documentChanges,
        ImmutableArray<TagHelperDescriptor> originTagHelpers,
        string newName,
        Uri uri,
        RazorCodeDocument codeDocument)
    {
        var documentIdentifier = new OptionalVersionedTextDocumentIdentifier { Uri = uri };
        var tagHelperElements = codeDocument.GetSyntaxTree().Root
            .DescendantNodes()
            .Where(n => n.Kind == RazorSyntaxKind.MarkupTagHelperElement)
            .OfType<MarkupTagHelperElementSyntax>();

        foreach (var originTagHelper in originTagHelpers)
        {
            var editedName = newName;
            if (originTagHelper.IsComponentFullyQualifiedNameMatch)
            {
                // Fully qualified binding, our "new name" needs to be fully qualified.
                var @namespace = originTagHelper.GetTypeNamespace();
                if (@namespace == null)
                {
                    return;
                }

                // The origin TagHelper was fully qualified so any fully qualified rename locations we find will need a fully qualified renamed edit.
                editedName = $"{@namespace}.{newName}";
            }

            foreach (var node in tagHelperElements)
            {
                if (node is MarkupTagHelperElementSyntax { TagHelperInfo.BindingResult: var binding } tagHelperElement &&
                    BindingContainsTagHelper(originTagHelper, binding))
                {
                    documentChanges.Add(new TextDocumentEdit
                    {
                        TextDocument = documentIdentifier,
                        Edits = CreateEditsForMarkupTagHelperElement(tagHelperElement, codeDocument, editedName),
                    });
                }
            }
        }
    }

    private static TextEdit[] CreateEditsForMarkupTagHelperElement(MarkupTagHelperElementSyntax element, RazorCodeDocument codeDocument, string newName)
    {
        var startTagEdit = VsLspFactory.CreateTextEdit(element.StartTag.Name.GetRange(codeDocument.Source), newName);

        if (element.EndTag is MarkupTagHelperEndTagSyntax endTag)
        {
            var endTagEdit = VsLspFactory.CreateTextEdit(endTag.Name.GetRange(codeDocument.Source), newName);

            return [startTagEdit, endTagEdit];
        }

        return [startTagEdit];
    }

    private static bool BindingContainsTagHelper(TagHelperDescriptor tagHelper, TagHelperBinding potentialBinding)
        => potentialBinding.Descriptors.Any(descriptor => descriptor.Equals(tagHelper));

    private static async Task<ImmutableArray<TagHelperDescriptor>> GetOriginTagHelpersAsync(DocumentContext documentContext, int absoluteIndex, CancellationToken cancellationToken)
    {
        var owner = await documentContext.GetSyntaxNodeAsync(absoluteIndex, cancellationToken).ConfigureAwait(false);
        if (owner is null)
        {
            Debug.Fail("Owner should never be null.");
            return default;
        }

        if (!TryGetTagHelperBinding(owner, absoluteIndex, out var binding))
        {
            return default;
        }

        // Can only have 1 component TagHelper belonging to an element at a time
        var primaryTagHelper = binding.Descriptors.FirstOrDefault(static d => d.IsComponentTagHelper);
        if (primaryTagHelper is null)
        {
            return default;
        }

        var tagHelpers = await documentContext.Snapshot.Project.GetTagHelpersAsync(cancellationToken).ConfigureAwait(false);
        var associatedTagHelper = FindAssociatedTagHelper(primaryTagHelper, tagHelpers);
        if (associatedTagHelper is null)
        {
            return default;
        }

        return [primaryTagHelper, associatedTagHelper];
    }

    private static bool TryGetTagHelperBinding(RazorSyntaxNode owner, int absoluteIndex, [NotNullWhen(true)] out TagHelperBinding? binding)
    {
        // End tags are easy, because there is only one possible binding result
        if (owner is MarkupTagHelperEndTagSyntax { Parent: MarkupTagHelperElementSyntax { TagHelperInfo.BindingResult: var endTagBindingResult } })
        {
            binding = endTagBindingResult;
            return true;
        }

        // A rename of a start tag could have an "owner" of one of its attributes, so we do a bit more checking
        // to support this case
        var node = owner.FirstAncestorOrSelf<RazorSyntaxNode>(n => n.Kind == RazorSyntaxKind.MarkupTagHelperStartTag);
        if (node is not MarkupTagHelperStartTagSyntax tagHelperStartTag)
        {
            binding = null;
            return false;
        }

        // Ensure the rename action was invoked on the component name instead of a component parameter. This serves as an issue
        // mitigation till `textDocument/prepareRename` is supported and we can ensure renames aren't triggered in unsupported
        // contexts. (https://github.com/dotnet/razor/issues/4285)
        if (!tagHelperStartTag.Name.FullSpan.IntersectsWith(absoluteIndex))
        {
            binding = null;
            return false;
        }

        if (tagHelperStartTag is { Parent: MarkupTagHelperElementSyntax { TagHelperInfo.BindingResult: var startTagBindingResult } })
        {
            binding = startTagBindingResult;
            return true;
        }

        binding = null;
        return false;
    }

    private static TagHelperDescriptor? FindAssociatedTagHelper(TagHelperDescriptor tagHelper, ImmutableArray<TagHelperDescriptor> tagHelpers)
    {
        var typeName = tagHelper.GetTypeName();
        var assemblyName = tagHelper.AssemblyName;
        foreach (var currentTagHelper in tagHelpers)
        {
            if (tagHelper == currentTagHelper)
            {
                // Same as the primary, we're looking for our other pair.
                continue;
            }

            if (typeName != currentTagHelper.GetTypeName())
            {
                continue;
            }

            if (assemblyName != currentTagHelper.AssemblyName)
            {
                continue;
            }

            // Found our associated TagHelper, there should only ever be 1 other associated TagHelper (fully qualified and non-fully qualified).
            return currentTagHelper;
        }

        Debug.Fail("Components should always have an associated TagHelper.");
        return null;
    }
}
