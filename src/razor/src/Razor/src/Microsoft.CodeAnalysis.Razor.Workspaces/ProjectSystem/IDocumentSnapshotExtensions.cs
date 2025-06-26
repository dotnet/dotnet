// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal static class IDocumentSnapshotExtensions
{
    public static async Task<RazorSourceDocument> GetSourceAsync(
        this IDocumentSnapshot document,
        CancellationToken cancellationToken)
    {
        var text = await document.GetTextAsync(cancellationToken).ConfigureAwait(false);
        var properties = RazorSourceDocumentProperties.Create(document.FilePath, document.TargetPath);
        return RazorSourceDocument.Create(text, properties);
    }

    public static async Task<TagHelperDescriptor?> TryGetTagHelperDescriptorAsync(
        this IDocumentSnapshot documentSnapshot,
        CancellationToken cancellationToken)
    {
        // No point doing anything if its not a component
        if (documentSnapshot.FileKind != RazorFileKind.Component)
        {
            return null;
        }

        var razorCodeDocument = await documentSnapshot.GetGeneratedOutputAsync(cancellationToken).ConfigureAwait(false);
        if (razorCodeDocument is null)
        {
            return null;
        }

        var project = documentSnapshot.Project;

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
        if (documentSnapshot.FileKind != RazorFileKind.Component)
        {
            return false;
        }

        var fileName = Path.GetFileNameWithoutExtension(documentSnapshot.FilePath);
        return fileName.AsSpan().Equals(path.Span, PathUtilities.OSSpecificPathComparison);
    }
}
