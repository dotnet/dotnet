// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Moq;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal class TestRazorProjectService(
    RemoteTextLoaderFactory remoteTextLoaderFactory,
    ProjectSnapshotManager projectManager,
    ILoggerFactory loggerFactory)
    : RazorProjectService(
        projectManager,
        CreateProjectInfoDriver(),
        remoteTextLoaderFactory,
        loggerFactory)
{
    private readonly ProjectSnapshotManager _projectManager = projectManager;

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
        var document = new DocumentSnapshotHandle(
            textDocumentPath, textDocumentPath, FileKinds.GetFileKindFromFilePath(textDocumentPath));

        foreach (var projectSnapshot in _projectManager.FindPotentialProjects(textDocumentPath))
        {
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
