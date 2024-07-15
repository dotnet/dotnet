// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Remote.Razor;
using Microsoft.CodeAnalysis.Text;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Test.Cohost;

public abstract class CohostTestBase(ITestOutputHelper testOutputHelper) : WorkspaceTestBase(testOutputHelper)
{
    private IRemoteServiceProvider? _remoteServiceProvider;

    private protected IRemoteServiceProvider RemoteServiceProvider => _remoteServiceProvider.AssumeNotNull();

    protected override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        var exportProvider = AddDisposable(await RemoteMefComposition.CreateExportProviderAsync());
        _remoteServiceProvider = AddDisposable(new TestRemoteServiceProvider(exportProvider));
    }

    protected TextDocument CreateRazorDocument(string contents)
    {
        var projectFilePath = TestProjectData.SomeProject.FilePath;
        var documentFilePath = TestProjectData.SomeProjectComponentFile1.FilePath;
        var projectName = Path.GetFileNameWithoutExtension(projectFilePath);
        var projectId = ProjectId.CreateNewId(debugName: projectName);
        var documentId = DocumentId.CreateNewId(projectId, debugName: documentFilePath);

        var solution = Workspace.CurrentSolution.AddProject(ProjectInfo.Create(
            projectId,
            VersionStamp.Create(),
            name: projectName,
            assemblyName: projectName,
            LanguageNames.CSharp,
            documentFilePath));

        solution = solution.AddAdditionalDocument(
            documentId,
            documentFilePath,
            SourceText.From(contents),
            filePath: documentFilePath);

        return solution.GetAdditionalDocument(documentId).AssumeNotNull();
    }
}
