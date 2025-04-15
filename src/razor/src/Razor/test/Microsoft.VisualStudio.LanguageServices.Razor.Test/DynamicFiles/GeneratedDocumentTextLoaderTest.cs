// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.DynamicFiles;

public class GeneratedDocumentTextLoaderTest(ITestOutputHelper testOutput) : WorkspaceTestBase(testOutput)
{
    private static readonly HostProject s_hostProject = TestProjectData.SomeProject;
    private static readonly HostDocument s_hostDocument = TestProjectData.SomeProjectFile1;

    [Fact, WorkItem("https://github.com/dotnet/aspnetcore/issues/7997")]
    public async Task LoadAsync_SpecifiesEncoding()
    {
        // Arrange
        var state = ProjectState
            .Create(s_hostProject, CompilerOptions, ProjectEngineFactoryProvider)
            .AddEmptyDocument(s_hostDocument);

        var project = new ProjectSnapshot(state);

        var document = project.GetRequiredDocument(s_hostDocument.FilePath);

        var loader = new GeneratedDocumentTextLoader(document, "file.cshtml");

        // Act
        var textAndVersion = await loader.LoadTextAndVersionAsync(default, DisposalToken);

        // Assert
        Assert.True(textAndVersion.Text.CanBeEmbedded);
        Assert.Same(Encoding.UTF8, textAndVersion.Text.Encoding);
    }
}
