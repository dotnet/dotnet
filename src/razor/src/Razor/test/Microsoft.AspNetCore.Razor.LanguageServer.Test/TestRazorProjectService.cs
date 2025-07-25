// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Serialization;
using Microsoft.CodeAnalysis.Razor.Utilities;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal class TestRazorProjectService(
    RemoteTextLoaderFactory remoteTextLoaderFactory,
    ProjectSnapshotManager projectManager,
    ILoggerFactory loggerFactory)
    : RazorProjectService(
        projectManager,
        TestRazorProjectInfoDriver.Instance,
        remoteTextLoaderFactory,
        loggerFactory)
{
    private readonly ProjectSnapshotManager _projectManager = projectManager;

    public async Task AddDocumentToPotentialProjectsAsync(string filePath, CancellationToken cancellationToken)
    {
        foreach (var projectSnapshot in _projectManager.FindPotentialProjects(filePath))
        {
            var projectDirectory = FilePathNormalizer.GetNormalizedDirectoryName(projectSnapshot.FilePath);
            var normalizedFilePath = FilePathNormalizer.Normalize(filePath);

            var targetPath = normalizedFilePath.StartsWith(projectDirectory, PathUtilities.OSSpecificPathComparison)
                ? normalizedFilePath[projectDirectory.Length..]
                : normalizedFilePath;

            var document = new DocumentSnapshotHandle(filePath, targetPath, FileKinds.GetFileKindFromPath(filePath));

            var projectInfo = projectSnapshot.ToRazorProjectInfo();

            projectInfo = projectInfo with
            {
                Documents = projectInfo.Documents.Add(document)
            };

            await ((IRazorProjectInfoListener)this)
                .UpdatedAsync(projectInfo, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
