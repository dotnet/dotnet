// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Moq;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal class TestRazorProjectService(
    RemoteTextLoaderFactory remoteTextLoaderFactory,
    IProjectSnapshotManager projectManager,
    ILoggerFactory loggerFactory)
    : RazorProjectService(
        projectManager,
        CreateProjectInfoDriver(),
        remoteTextLoaderFactory,
        loggerFactory)
{
    private readonly IProjectSnapshotManager _projectManager = projectManager;

    private static IRazorProjectInfoDriver CreateProjectInfoDriver()
    {
        var mock = new StrictMock<IRazorProjectInfoDriver>();

        mock.Setup(x => x.GetLatestProjectInfo())
            .Returns([]);

        mock.Setup(x => x.AddListener(It.IsAny<IRazorProjectInfoListener>()));

        return mock.Object;
    }

    public async Task AddDocumentToPotentialProjectsAsync(string textDocumentPath, CancellationToken cancellationToken)
    {
        foreach (var projectSnapshot in _projectManager.FindPotentialProjects(textDocumentPath))
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
}
