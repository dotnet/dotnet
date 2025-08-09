// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.CodeAnalysis;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Test.Cohost;

public class TextDocumentExtensionsTest(ITestOutputHelper testOutput) : VisualStudioWorkspaceTestBase(testOutput)
{
    [Theory]
    [InlineData(@"Pages\Index.razor")]
    [InlineData(@"Pages/Index.razor")]
    [InlineData(@"Pages.Index.razor")]
    public void TryComputeHintNameFromRazorDocument(string razorFilePath)
    {
        var projectId = ProjectId.CreateNewId();
        var projectInfo = ProjectInfo
            .Create(
                projectId,
                VersionStamp.Create(),
                name: "Project",
                assemblyName: "Project",
                LanguageNames.CSharp,
                TestProjectData.SomeProject.FilePath);

        var documentId = DocumentId.CreateNewId(projectId);
        var solution = Workspace.CurrentSolution
            .AddProject(projectInfo)
            .AddAdditionalDocument(documentId, "File.razor", "", filePath: Path.Combine(TestProjectData.SomeProjectPath, razorFilePath));

        var document = solution.GetAdditionalDocument(documentId);

        Assert.NotNull(document);
        Assert.True(document.TryComputeHintNameFromRazorDocument(out var hintName));
        // This tests TryComputeHintNameFromRazorDocument and also neatly demonstrates a bug: https://github.com/dotnet/razor/issues/11578
        Assert.Equal("Pages_Index_razor.g.cs", hintName);
    }
}
