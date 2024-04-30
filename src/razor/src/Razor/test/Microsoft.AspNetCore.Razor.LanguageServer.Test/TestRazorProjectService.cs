// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal class TestRazorProjectService(
    RemoteTextLoaderFactory remoteTextLoaderFactory,
    ISnapshotResolver snapshotResolver,
    IDocumentVersionCache documentVersionCache,
    IProjectSnapshotManager projectManager,
    ILoggerFactory loggerFactory)
    : RazorProjectService(remoteTextLoaderFactory, snapshotResolver, documentVersionCache, projectManager, loggerFactory)
{
    private readonly ISnapshotResolver _snapshotResolver = snapshotResolver;

    public async Task AddDocumentToPotentialProjectsAsync(string textDocumentPath, CancellationToken cancellationToken)
    {
        foreach (var projectSnapshot in _snapshotResolver.FindPotentialProjects(textDocumentPath))
        {
            var normalizedProjectPath = FilePathNormalizer.NormalizeDirectory(projectSnapshot.FilePath);
            var documents = ImmutableArray
                .CreateRange(projectSnapshot.DocumentFilePaths)
                .Add(textDocumentPath)
                .Select(d => new DocumentSnapshotHandle(d, d, FileKinds.GetFileKindFromFilePath(d)))
                .ToImmutableArray();

            await this.UpdateProjectAsync(projectSnapshot.Key, projectSnapshot.Configuration, projectSnapshot.RootNamespace, projectSnapshot.DisplayName, projectSnapshot.ProjectWorkspaceState,
                documents, cancellationToken).ConfigureAwait(false);
        }
    }

    private static string GetTargetPath(string documentFilePath, string normalizedProjectPath)
    {
        var targetFilePath = FilePathNormalizer.Normalize(documentFilePath);
        if (targetFilePath.StartsWith(normalizedProjectPath, FilePathComparison.Instance))
        {
            // Make relative
            targetFilePath = documentFilePath[normalizedProjectPath.Length..];
        }

        // Representing all of our host documents with a re-normalized target path to workaround GetRelatedDocument limitations.
        var normalizedTargetFilePath = targetFilePath.Replace('/', '\\').TrimStart('\\');

        return normalizedTargetFilePath;
    }
}
