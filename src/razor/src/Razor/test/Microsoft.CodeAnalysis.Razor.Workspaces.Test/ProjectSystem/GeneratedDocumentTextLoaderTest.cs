// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

public class GeneratedDocumentTextLoaderTest(ITestOutputHelper testOutput) : WorkspaceTestBase(testOutput)
{
    private readonly HostProject _hostProject = TestProjectData.SomeProject;
    private readonly HostDocument _hostDocument = TestProjectData.SomeProjectFile1;

    [Fact, WorkItem("https://github.com/dotnet/aspnetcore/issues/7997")]
    public async Task LoadAsync_SpecifiesEncoding()
    {
        // Arrange
        var state = ProjectState
            .Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, _hostProject, ProjectWorkspaceState.Default)
            .AddDocument(_hostDocument, TestMocks.CreateEmptyTextLoader());

        var project = new ProjectSnapshot(state);

        var document = project.GetRequiredDocument(_hostDocument.FilePath);

        var loader = new GeneratedDocumentTextLoader(document, "file.cshtml");

        // Act
        var textAndVersion = await loader.LoadTextAndVersionAsync(default, DisposalToken);

        // Assert
        Assert.True(textAndVersion.Text.CanBeEmbedded);
        Assert.Same(Encoding.UTF8, textAndVersion.Text.Encoding);
    }
}
