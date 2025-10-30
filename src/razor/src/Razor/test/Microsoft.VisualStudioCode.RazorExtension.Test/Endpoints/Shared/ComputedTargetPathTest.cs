// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Razor.LanguageClient.Cohost;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudioCode.RazorExtension.Test.Endpoints.Shared;

public class ComputedTargetPathTest(ITestOutputHelper testOutputHelper) : CohostEndpointTestBase(testOutputHelper)
{
    // What the source generator would produce for TestProjectData.SomeProjectPath
    private static readonly string s_hintNamePrefix = PlatformInformation.IsWindows
        ? "c__users_example_src_SomeProject"
        : "_home_example_SomeProject";

    [Theory]
    [InlineData(true, false)]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public async Task SingleDocument(bool projectPath, bool generateConfigFile)
    {
        var builder = new RazorProjectBuilder
        {
            ProjectFilePath = projectPath ? TestProjectData.SomeProject.FilePath : null,
            GenerateGlobalConfigFile = generateConfigFile,
            GenerateAdditionalDocumentMetadata = false,
            GenerateMSBuildProjectDirectory = false
        };

        var id = builder.AddAdditionalDocument(FilePath("File1.razor"), SourceText.From(""));

        var solution = LocalWorkspace.CurrentSolution;
        solution = builder.Build(solution);

        var document = solution.GetAdditionalDocument(id).AssumeNotNull();

        _ = await document.Project.GetCompilationAsync(DisposalToken);

        var generatedDocument = await document.Project.TryGetSourceGeneratedDocumentForRazorDocumentAsync(document, DisposalToken);
        Assert.NotNull(generatedDocument);
        Assert.Equal($"{s_hintNamePrefix}_File1_razor.g.cs", generatedDocument.HintName);
    }

    [Theory]
    [CombinatorialData]
    public async Task TwoDocumentsWithTheSameBaseFileName(bool generateTargetPath)
    {
        // This test just proves the "correct" behaviour, with the Razor SDL
        var builder = new RazorProjectBuilder
        {
            ProjectFilePath = TestProjectData.SomeProject.FilePath,
            GenerateAdditionalDocumentMetadata = generateTargetPath
        };

        var doc1Id = builder.AddAdditionalDocument(FilePath(@"Pages\Index.razor"), SourceText.From(""));
        var doc2Id = builder.AddAdditionalDocument(FilePath(@"Components\Index.razor"), SourceText.From(""));

        var solution = LocalWorkspace.CurrentSolution;
        solution = builder.Build(solution);

        var doc1 = solution.GetAdditionalDocument(doc1Id).AssumeNotNull();
        var doc2 = solution.GetAdditionalDocument(doc2Id).AssumeNotNull();

        var generatedDocument = await doc1.Project.TryGetSourceGeneratedDocumentForRazorDocumentAsync(doc1, DisposalToken);
        Assert.NotNull(generatedDocument);
        Assert.Equal($"Pages_Index_razor.g.cs", generatedDocument.HintName);

        generatedDocument = await doc2.Project.TryGetSourceGeneratedDocumentForRazorDocumentAsync(doc2, DisposalToken);
        Assert.NotNull(generatedDocument);
        Assert.Equal($"Components_Index_razor.g.cs", generatedDocument.HintName);
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public async Task TwoDocumentsWithTheSameBaseFileName_FullPathHintName(bool projectPath, bool generateConfigFile)
    {
        var builder = new RazorProjectBuilder
        {
            ProjectFilePath = projectPath ? TestProjectData.SomeProject.FilePath : null,
            GenerateGlobalConfigFile = generateConfigFile,
            GenerateAdditionalDocumentMetadata = false,
            GenerateMSBuildProjectDirectory = false
        };

        var doc1Id = builder.AddAdditionalDocument(FilePath(@"Pages\Index.razor"), SourceText.From(""));
        var doc2Id = builder.AddAdditionalDocument(FilePath(@"Components\Index.razor"), SourceText.From(""));

        var solution = LocalWorkspace.CurrentSolution;
        solution = builder.Build(solution);

        var doc1 = solution.GetAdditionalDocument(doc1Id).AssumeNotNull();
        var doc2 = solution.GetAdditionalDocument(doc2Id).AssumeNotNull();

        var generatedDocument = await doc1.Project.TryGetSourceGeneratedDocumentForRazorDocumentAsync(doc1, DisposalToken);
        Assert.NotNull(generatedDocument);
        Assert.Equal($"{s_hintNamePrefix}_Pages_Index_razor.g.cs", generatedDocument.HintName);

        generatedDocument = await doc2.Project.TryGetSourceGeneratedDocumentForRazorDocumentAsync(doc2, DisposalToken);
        Assert.NotNull(generatedDocument);
        Assert.Equal($"{s_hintNamePrefix}_Components_Index_razor.g.cs", generatedDocument.HintName);
    }
}
