// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
using Microsoft.CodeAnalysis.Razor.Workspaces;
using RazorSyntaxNode = Microsoft.AspNetCore.Razor.Language.Syntax.SyntaxNode;

namespace Microsoft.CodeAnalysis.Razor.Rename;

internal class RenameService(
    IRazorComponentSearchEngine componentSearchEngine,
    IFileSystem fileSystem,
    LanguageServerFeatureOptions languageServerFeatureOptions) : IRenameService
{
    private readonly IRazorComponentSearchEngine _componentSearchEngine = componentSearchEngine;
    private readonly IFileSystem _fileSystem = fileSystem;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions;

    public async Task<RenameResult> TryGetRazorRenameEditsAsync(
        DocumentContext documentContext,
        DocumentPositionInfo positionInfo,
        string newName,
        ISolutionQueryOperations solutionQueryOperations,
        CancellationToken cancellationToken)
    {
        // We only support renaming of .razor components, not .cshtml tag helpers
        if (!documentContext.FileKind.IsComponent())
        {
            return new(Edit: null);
        }

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        if (!TryGetOriginTagHelpers(codeDocument, positionInfo.HostDocumentIndex, out var originTagHelpers))
        {
            return new(Edit: null);
        }

        var originComponentDocumentSnapshot = await _componentSearchEngine
            .TryLocateComponentAsync(originTagHelpers.Primary, solutionQueryOperations, cancellationToken)
            .ConfigureAwait(false);
        if (originComponentDocumentSnapshot is null)
        {
            return new(Edit: null);
        }

        var originComponentDocumentFilePath = originComponentDocumentSnapshot.FilePath;
        var newPath = MakeNewPath(originComponentDocumentFilePath, newName);
        if (_fileSystem.FileExists(newPath))
        {
            // We found a tag, but the new name would cause a conflict, so we can't proceed with the rename,
            // even if C# might have worked.
            return new(Edit: null, FallbackToCSharp: false);
        }

        using var documentChanges = new PooledArrayBuilder<SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>>();

        var fileRename = GetRenameFileEdit(originComponentDocumentFilePath, newPath);
        documentChanges.Add(fileRename);

        if (!_languageServerFeatureOptions.UseRazorCohostServer)
        {
            AddEditsForCodeDocument(ref documentChanges.AsRef(), originTagHelpers, newName, new(documentContext.Uri), codeDocument);
        }

        AddAdditionalFileRenames(ref documentChanges.AsRef(), originComponentDocumentFilePath, newPath);

        if (!_languageServerFeatureOptions.UseRazorCohostServer)
        {
            var documentSnapshots = GetAllDocumentSnapshots(documentContext.FilePath, solutionQueryOperations);

            foreach (var documentSnapshot in documentSnapshots)
            {
                if (!documentSnapshot.FileKind.IsComponent())
                {
                    continue;
                }

                // VS Code in Windows expects path to start with '/'
                var uri = new DocumentUri(LspFactory.CreateFilePathUri(documentSnapshot.FilePath, _languageServerFeatureOptions));
                var generatedOutput = await documentSnapshot.GetGeneratedOutputAsync(cancellationToken).ConfigureAwait(false);

                AddEditsForCodeDocument(ref documentChanges.AsRef(), originTagHelpers, newName, uri, generatedOutput);
            }
        }

        foreach (var documentChange in documentChanges)
        {
            if (documentChange.TryGetFirst(out var textDocumentEdit) &&
                textDocumentEdit.TextDocument.DocumentUri == fileRename.OldDocumentUri)
            {
                textDocumentEdit.TextDocument.DocumentUri = fileRename.NewDocumentUri;
            }
        }

        return new(Edit: new()
        {
            DocumentChanges = documentChanges.ToArrayAndClear()
        });
    }

    public bool TryGetRazorFileRenameEdit(
        DocumentContext documentContext,
        string newName,
        [NotNullWhen(true)] out WorkspaceEdit? workspaceEdit)
    {
        var oldPath = documentContext.FilePath;
        var newPath = MakeNewPath(oldPath, newName);

        using var documentChanges = new PooledArrayBuilder<SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>>();

        AddAdditionalFileRenames(ref documentChanges.AsRef(), oldPath, newPath);

        if (documentChanges.Count == 0)
        {
            workspaceEdit = null;
            return false;
        }

        workspaceEdit = new()
        {
            DocumentChanges = documentChanges.ToArrayAndClear()
        };
        return true;
    }

    private static ImmutableArray<IDocumentSnapshot> GetAllDocumentSnapshots(string filePath, ISolutionQueryOperations solutionQueryOperations)
    {
        using var documentSnapshots = new PooledArrayBuilder<IDocumentSnapshot>();
        using var _ = SpecializedPools.GetPooledStringHashSet(out var documentPaths);

        foreach (var project in solutionQueryOperations.GetProjects())
        {
            foreach (var documentPath in project.DocumentFilePaths)
            {
                // We've already added refactoring edits for our document snapshot
                if (PathUtilities.OSSpecificPathComparer.Equals(documentPath, filePath))
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

        return documentSnapshots.ToImmutableAndClear();
    }

    private void AddAdditionalFileRenames(
        ref PooledArrayBuilder<SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>> documentChanges,
        string oldFilePath, string newFilePath)
    {
        TryAdd(".cs", ref documentChanges);
        TryAdd(".css", ref documentChanges);

        void TryAdd(
            string extension,
            ref PooledArrayBuilder<SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>> documentChanges)
        {
            var changedPath = oldFilePath + extension;

            if (_fileSystem.FileExists(changedPath))
            {
                documentChanges.Add(GetRenameFileEdit(changedPath, newFilePath + extension));
            }
        }
    }

    private RenameFile GetRenameFileEdit(string oldFilePath, string newFilePath)
        => new()
        {
            OldDocumentUri = new(LspFactory.CreateFilePathUri(oldFilePath, _languageServerFeatureOptions)),
            NewDocumentUri = new(LspFactory.CreateFilePathUri(newFilePath, _languageServerFeatureOptions)),
        };

    private static string MakeNewPath(string originalPath, string newName)
    {
        var newFileName = $"{newName}{Path.GetExtension(originalPath)}";
        var directoryName = Path.GetDirectoryName(originalPath).AssumeNotNull();
        return Path.Combine(directoryName, newFileName);
    }

    private void AddEditsForCodeDocument(
        ref PooledArrayBuilder<SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>> documentChanges,
        OriginTagHelpers originTagHelpers,
        string newName,
        DocumentUri uri,
        RazorCodeDocument codeDocument)
    {
        var documentIdentifier = new OptionalVersionedTextDocumentIdentifier { DocumentUri = uri };

        using var elements = new PooledArrayBuilder<MarkupTagHelperElementSyntax>();

        foreach (var node in codeDocument.GetRequiredSyntaxRoot().DescendantNodes())
        {
            if (node is MarkupTagHelperElementSyntax element)
            {
                elements.Add(element);
            }
        }

        // Collect all edits first, then de-duplicate them by range.
        using var allEdits = new PooledArrayBuilder<SumType<TextEdit, AnnotatedTextEdit>>();

        if (TryCollectEdits(originTagHelpers.Primary, newName, codeDocument.Source, in elements, ref allEdits.AsRef()) &&
            (originTagHelpers.Associated is null || TryCollectEdits(originTagHelpers.Associated, newName, codeDocument.Source, in elements, ref allEdits.AsRef())))
        {
            var uniqueEdits = GetUniqueEdits(ref allEdits.AsRef());

            if (uniqueEdits.Length > 0)
            {
                documentChanges.Add(new TextDocumentEdit
                {
                    TextDocument = documentIdentifier,
                    Edits = uniqueEdits,
                });
            }
        }

        return;

        static bool TryCollectEdits(
            TagHelperDescriptor tagHelper,
            string newName,
            RazorSourceDocument sourceDocument,
            ref readonly PooledArrayBuilder<MarkupTagHelperElementSyntax> elements,
            ref PooledArrayBuilder<SumType<TextEdit, AnnotatedTextEdit>> edits)
        {
            var editedName = newName;

            if (tagHelper.IsFullyQualifiedNameMatch)
            {
                // Fully qualified binding, our "new name" needs to be fully qualified.
                var @namespace = tagHelper.TypeNamespace;
                if (@namespace == null)
                {
                    return false;
                }

                // The origin TagHelper was fully qualified so any fully qualified rename locations
                // we find will need a fully qualified renamed edit.
                editedName = $"{@namespace}.{newName}";
            }

            foreach (var element in elements)
            {
                if (element.TagHelperInfo.BindingResult.TagHelpers.Contains(tagHelper))
                {
                    var startTagEdit = LspFactory.CreateTextEdit(element.StartTag.Name.GetRange(sourceDocument), editedName);

                    edits.Add(startTagEdit);

                    if (element.EndTag is MarkupTagHelperEndTagSyntax endTag)
                    {
                        var endTagEdit = LspFactory.CreateTextEdit(endTag.Name.GetRange(sourceDocument), editedName);

                        edits.Add(endTagEdit);
                    }
                }
            }

            return true;
        }

        static SumType<TextEdit, AnnotatedTextEdit>[] GetUniqueEdits(
            ref PooledArrayBuilder<SumType<TextEdit, AnnotatedTextEdit>> edits)
        {
            if (edits.Count == 0)
            {
                return [];
            }

            // De-duplicate edits by range.
            using var uniqueEdits = new PooledArrayBuilder<SumType<TextEdit, AnnotatedTextEdit>>(edits.Count);
            using var _ = HashSetPool<LspRange>.GetPooledObject(out var seenRanges);

#if NET
            seenRanges.EnsureCapacity(edits.Count);
#endif

            foreach (var edit in edits)
            {
                if (edit.TryGetFirst(out var textEdit))
                {
                    if (seenRanges.Add(textEdit.Range))
                    {
                        uniqueEdits.Add(edit);
                    }
                }
                else if (edit.TryGetSecond(out var annotatedEdit))
                {
                    if (seenRanges.Add(annotatedEdit.Range))
                    {
                        uniqueEdits.Add(edit);
                    }
                }
            }

            return uniqueEdits.ToArrayAndClear();
        }
    }

    private readonly record struct OriginTagHelpers(TagHelperDescriptor Primary, TagHelperDescriptor? Associated);

    private bool TryGetOriginTagHelpers(RazorCodeDocument codeDocument, int absoluteIndex, out OriginTagHelpers originTagHelpers)
    {
        var owner = codeDocument.GetRequiredSyntaxRoot().FindInnermostNode(absoluteIndex);
        if (owner is null)
        {
            Debug.Fail("Owner should never be null.");
            originTagHelpers = default;
            return false;
        }

        if (!TryGetTagHelperBinding(owner, absoluteIndex, out var binding))
        {
            originTagHelpers = default;
            return false;
        }

        // Can only have 1 component TagHelper belonging to an element at a time
        var primaryTagHelper = binding.TagHelpers.FirstOrDefault(static d => d.Kind == TagHelperKind.Component);
        if (primaryTagHelper is null)
        {
            originTagHelpers = default;
            return false;
        }

        var tagHelpers = codeDocument.GetRequiredTagHelpers();
        var associatedTagHelper = TryFindAssociatedTagHelper(primaryTagHelper, tagHelpers);

        originTagHelpers = new(primaryTagHelper, associatedTagHelper);
        return true;
    }

    private bool TryGetTagHelperBinding(RazorSyntaxNode owner, int absoluteIndex, [NotNullWhen(true)] out TagHelperBinding? binding)
    {
        // End tags are easy, because there is only one possible binding result
        if (owner is MarkupTagHelperEndTagSyntax { Parent: MarkupTagHelperElementSyntax { TagHelperInfo.BindingResult: var endTagBindingResult } })
        {
            binding = endTagBindingResult;
            return true;
        }

        // A rename of a start tag could have an "owner" of one of its attributes, so we do a bit more checking
        // to support this case
        if (owner.FirstAncestorOrSelf<MarkupTagHelperStartTagSyntax>() is not { } tagHelperStartTag)
        {
            binding = null;
            return false;
        }

        // Ensure the rename action was invoked on the component name instead of a component parameter. This serves as an issue
        // mitigation till `textDocument/prepareRename` is supported and we can ensure renames aren't triggered in unsupported
        // contexts. (https://github.com/dotnet/razor/issues/4285)
        if (!tagHelperStartTag.Name.Span.IntersectsWith(absoluteIndex))
        {
            binding = null;
            return false;
        }

        if (tagHelperStartTag is { Parent: MarkupTagHelperElementSyntax { TagHelperInfo.BindingResult: var startTagBindingResult } })
        {
            binding = startTagBindingResult;

            // If the component is fully qualified, we need to make sure that the caret is in the actual component name part
            // not a namespace part. This only applies in cohosting, where we also get Roslyn edits for renames, and hence
            // renaming a namespace part will actually rename the namespace.
            if (_languageServerFeatureOptions.UseRazorCohostServer &&
                binding.TagHelpers is [{ IsFullyQualifiedNameMatch: true }, ..])
            {
                var lastDotIndex = tagHelperStartTag.Name.Content.LastIndexOf('.');
                Debug.Assert(lastDotIndex != -1, "Fully qualified component names should contain a dot.");
                if (absoluteIndex < tagHelperStartTag.Name.SpanStart + lastDotIndex + 1)
                {
                    binding = null;
                    return false;
                }
            }

            return true;
        }

        binding = null;
        return false;
    }

    private static TagHelperDescriptor? TryFindAssociatedTagHelper(
        TagHelperDescriptor primary,
        TagHelperCollection tagHelpers)
    {
        var typeName = primary.TypeName;
        var assemblyName = primary.AssemblyName;

        foreach (var tagHelper in tagHelpers)
        {
            if (typeName == tagHelper.TypeName &&
                assemblyName == tagHelper.AssemblyName &&
                !tagHelper.Equals(primary))
            {
                return tagHelper;
            }
        }

        return null;
    }
}
