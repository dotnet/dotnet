// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

public class GeneratedDocumentTextLoaderTest : WorkspaceTestBase
{
    private readonly HostProject _hostProject;
    private readonly HostDocument _hostDocument;

    public GeneratedDocumentTextLoaderTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _hostProject = TestProjectData.SomeProject;
        _hostDocument = TestProjectData.SomeProjectFile1;
    }

    [Fact, WorkItem("https://github.com/dotnet/aspnetcore/issues/7997")]
    public async Task LoadAsync_SpecifiesEncoding()
    {
        // Arrange
        var project = new ProjectSnapshot(
            ProjectState.Create(Workspace.Services, _hostProject)
            .WithAddedHostDocument(_hostDocument, () => Task.FromResult(TextAndVersion.Create(SourceText.From(""), VersionStamp.Create()))));

        var document = project.GetDocument(_hostDocument.FilePath);

        var loader = new GeneratedDocumentTextLoader(document, "file.cshtml");

        // Act
        var textAndVersion = await loader.LoadTextAndVersionAsync(default, default);

        // Assert
        Assert.True(textAndVersion.Text.CanBeEmbedded);
        Assert.Same(Encoding.UTF8, textAndVersion.Text.Encoding);
    }
}
