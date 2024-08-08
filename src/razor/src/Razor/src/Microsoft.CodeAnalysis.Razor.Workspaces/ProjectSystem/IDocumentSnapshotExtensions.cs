// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal static class IDocumentSnapshotExtensions
{
    public static async Task<TagHelperDescriptor?> TryGetTagHelperDescriptorAsync(this IDocumentSnapshot documentSnapshot, CancellationToken cancellationToken)
    {
        // No point doing anything if its not a component
        if (documentSnapshot.FileKind != FileKinds.Component)
        {
            return null;
        }

        var razorCodeDocument = await documentSnapshot.GetGeneratedOutputAsync().ConfigureAwait(false);
        if (razorCodeDocument is null)
        {
            return null;
        }

        var project = documentSnapshot.Project;

        // If the document is an import document, then it can't be a component
        if (project.IsImportDocument(documentSnapshot))
        {
            return null;
        }

        // If we got this far, we can check for tag helpers
        var tagHelpers = await project.GetTagHelpersAsync(cancellationToken).ConfigureAwait(false);
        foreach (var tagHelper in tagHelpers)
        {
            // Check the typename and namespace match
            if (documentSnapshot.IsPathCandidateForComponent(tagHelper.GetTypeNameIdentifier().AsMemory()) &&
                razorCodeDocument.ComponentNamespaceMatches(tagHelper.GetTypeNamespace()))
            {
                return tagHelper;
            }
        }

        return null;
    }

    public static bool IsPathCandidateForComponent(this IDocumentSnapshot documentSnapshot, ReadOnlyMemory<char> path)
    {
        if (documentSnapshot.FileKind != FileKinds.Component)
        {
            return false;
        }

        var fileName = Path.GetFileNameWithoutExtension(documentSnapshot.FilePath);
        return fileName.AsSpan().Equals(path.Span, FilePathComparison.Instance);
    }

    public static Task<RazorCodeDocument> GetFormatterCodeDocumentAsync(this IDocumentSnapshot documentSnapshot)
    {
        var forceRuntimeCodeGeneration = documentSnapshot.Project.Configuration.LanguageServerFlags?.ForceRuntimeCodeGeneration ?? false;
        if (!forceRuntimeCodeGeneration)
        {
            return documentSnapshot.GetGeneratedOutputAsync();
        }

        // if forceRuntimeCodeGeneration is on, GetGeneratedOutputAsync will get runtime code. As of now
        // the formatting service doesn't expect the form of code generated to be what the compiler does with
        // runtime. For now force usage of design time and avoid the cache. There may be a slight perf hit
        // but either the user is typing (which will invalidate the cache) or the user is manually attempting to
        // format. We expect formatting to invalidate the cache if it changes things and consider this an
        // acceptable overhead for now.
        return GetDesignTimeDocumentAsync(documentSnapshot);
    }

    private static async Task<RazorCodeDocument> GetDesignTimeDocumentAsync(IDocumentSnapshot documentSnapshot)
    {
        var project = documentSnapshot.Project;
        var tagHelpers = await project.GetTagHelpersAsync(CancellationToken.None).ConfigureAwait(false);
        var projectEngine = project.GetProjectEngine();
        var imports = await DocumentState.GetImportsAsync(documentSnapshot, projectEngine).ConfigureAwait(false);
        return await DocumentState.GenerateCodeDocumentAsync(documentSnapshot, project.GetProjectEngine(), imports, tagHelpers, false).ConfigureAwait(false);
    }
}
