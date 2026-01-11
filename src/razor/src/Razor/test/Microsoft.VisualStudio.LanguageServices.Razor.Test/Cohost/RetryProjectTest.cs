// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Remote.Razor.Resources;
using Microsoft.CodeAnalysis.Text;
using Microsoft.NET.Sdk.Razor.SourceGenerators;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

public class RetryProjectTest(ITestOutputHelper testOutputHelper) : CohostEndpointTestBase(testOutputHelper)
{
    [Fact]
    public async Task HoverRequest_ReturnsResults()
    {
        RazorCohostingOptions.UseRazorCohostServer = false;

        TestCode input = """
            <div></div>
            @System.DateTi$$me.Now
            """;
        var document = CreateProjectAndRazorDocument(input.Text, RazorFileKind.Component);

        // Make sure the source generator has been run while cohosting is off, to simular Roslyn winning the initialization race
        Assert.Empty(await document.Project.GetSourceGeneratedDocumentsAsync(DisposalToken));

        // Now turn the source generator on, to simulate Razor starting up and initializing OOP
        RazorCohostingOptions.UseRazorCohostServer = true;

        var inputText = await document.GetTextAsync(DisposalToken);
        var linePosition = inputText.GetLinePosition(input.Position);

        var requestInvoker = new TestHtmlRequestInvoker();
        var endpoint = new CohostHoverEndpoint(IncompatibleProjectService, RemoteServiceInvoker, requestInvoker);

        var textDocumentPositionParams = new TextDocumentPositionParams
        {
            Position = LspFactory.CreatePosition(linePosition),
            TextDocument = new TextDocumentIdentifier { DocumentUri = document.CreateDocumentUri() },
        };

        var hoverResult = await endpoint.GetTestAccessor().HandleRequestAsync(textDocumentPositionParams, document, DisposalToken);
        Assert.NotNull(hoverResult);
    }

    [Fact]
    public async Task HoverRequest_MultipleProjects_ReturnsResults()
    {
        RazorCohostingOptions.UseRazorCohostServer = false;

        TestCode input = """
            <div></div>
            @System.DateTi$$me.Now
            """;
        // Specify remoteOnly because our test infrastructure isn't set up to mutate solutions otherwise
        var document = CreateProjectAndRazorDocument(input.Text, remoteOnly: true);

        // Now we create another document, in another project
        TestCode otherInput = """
            @System.DateTi$$me.Now
            <div></div>
            """;
        var projectId = ProjectId.CreateNewId(debugName: TestProjectData.SomeProject.DisplayName);
        var documentFilePath = TestProjectData.AnotherProjectComponentFile1.FilePath;
        var documentId = DocumentId.CreateNewId(projectId, debugName: documentFilePath);
        var otherDocument = AddProjectAndRazorDocument(document.Project.Solution, TestProjectData.AnotherProject.FilePath, projectId, documentId, documentFilePath, otherInput.Text);

        // Make sure we have the document from our new fork
        document = otherDocument.Project.Solution.GetAdditionalDocument(document.Id).AssumeNotNull();

        // Make sure the source generator has been run while cohosting is off, to simular Roslyn winning the initialization race
        Assert.Empty(await document.Project.GetSourceGeneratedDocumentsAsync(DisposalToken));
        Assert.Empty(await otherDocument.Project.GetSourceGeneratedDocumentsAsync(DisposalToken));

        // Now turn the source generator on, to simulate Razor starting up and initializing OOP
        RazorCohostingOptions.UseRazorCohostServer = true;

        var requestInvoker = new TestHtmlRequestInvoker();
        var endpoint = new CohostHoverEndpoint(IncompatibleProjectService, RemoteServiceInvoker, requestInvoker);

        // Making this request will cause our solution to be redirected, and project 1 to be a retry project
        await MakeHoverRequestAsync(input, document);

        // Making this request will use our redirect solution, but project 2 is not a retry project, so would fail normally
        await MakeHoverRequestAsync(otherInput, otherDocument);

        async Task MakeHoverRequestAsync(TestCode input, TextDocument document)
        {
            var inputText = await document.GetTextAsync(DisposalToken);
            var linePosition = inputText.GetLinePosition(input.Position);
            var textDocumentPositionParams = new TextDocumentPositionParams
            {
                Position = LspFactory.CreatePosition(linePosition),
                TextDocument = new TextDocumentIdentifier { DocumentUri = document.CreateDocumentUri() },
            };

            var hoverResult = await endpoint.GetTestAccessor().HandleRequestAsync(textDocumentPositionParams, document, DisposalToken);
            Assert.NotNull(hoverResult);
        }
    }

    [Fact]
    public async Task GetRequiredCodeDocument_SucceedsAndReturnsRetryProject()
    {
        RazorCohostingOptions.UseRazorCohostServer = false;

        TestCode input = """
            <div></div>
            """;
        var document = CreateProjectAndRazorDocument(input.Text, RazorFileKind.Component);

        // Make sure the source generator has been run while cohosting is off, to simulate Roslyn winning the initialization race
        Assert.Empty(await document.Project.GetSourceGeneratedDocumentsAsync(DisposalToken));

        // Now turn the source generator on, to simulate Razor starting up and initializing OOP
        RazorCohostingOptions.UseRazorCohostServer = true;

        var snapshotManager = OOPExportProvider.GetExportedValue<RemoteSnapshotManager>();
        var projectSnapshot = snapshotManager.GetSnapshot(document.Project);
        var documentSnapshot = projectSnapshot.GetDocument(document);

        await projectSnapshot.GetRequiredCodeDocumentAsync(documentSnapshot, DisposalToken);

        projectSnapshot = snapshotManager.GetSnapshot(document.Project);
        Assert.True(projectSnapshot.Project.IsRetryProject());
    }

    [Fact]
    public async Task GetSnapshot_ReturnsDifferentSolutionSnapshots()
    {
        RazorCohostingOptions.UseRazorCohostServer = false;

        TestCode input = """
            <div></div>
            """;
        var document = CreateProjectAndRazorDocument(input.Text, RazorFileKind.Component);

        // Make sure the source generator has been run while cohosting is off, to simular Roslyn winning the initialization race
        Assert.Empty(await document.Project.GetSourceGeneratedDocumentsAsync(DisposalToken));

        var snapshotManager = OOPExportProvider.GetExportedValue<RemoteSnapshotManager>();

        var solutionSnapshot = snapshotManager.GetSnapshot(document.Project.Solution);

        // Now turn the source generator on, to simulate Razor starting up and initializing OOP
        RazorCohostingOptions.UseRazorCohostServer = true;

        var projectSnapshot = snapshotManager.GetSnapshot(document.Project);
        var documentSnapshot = projectSnapshot.GetDocument(document);
        await projectSnapshot.GetRequiredCodeDocumentAsync(documentSnapshot, DisposalToken);

        var newSolutionSnapshot = snapshotManager.GetSnapshot(document.Project.Solution);

        Assert.NotSame(solutionSnapshot, newSolutionSnapshot);
    }

    [Fact]
    public async Task CohostingOff_Throws()
    {
        RazorCohostingOptions.UseRazorCohostServer = false;

        TestCode input = """
            <div></div>
            """;
        var document = CreateProjectAndRazorDocument(input.Text, RazorFileKind.Component);

        // Make sure the source generator has been run while cohosting is off, to simular Roslyn winning the initialization race
        Assert.Empty(await document.Project.GetSourceGeneratedDocumentsAsync(DisposalToken));

        var snapshotManager = OOPExportProvider.GetExportedValue<RemoteSnapshotManager>();

        var projectSnapshot = snapshotManager.GetSnapshot(document.Project);
        var documentSnapshot = projectSnapshot.GetDocument(document);

        // Make sure we don't infinite loop retrying if cohosting is off
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => projectSnapshot.GetRequiredCodeDocumentAsync(documentSnapshot, DisposalToken));
        Assert.StartsWith(SR.FormatRazor_source_generator_did_not_produce_a_host_output(projectSnapshot.Project.Name, ""), exception.Message);
    }

    [Fact]
    public async Task ProjectChanges_NotRetryProject()
    {
        RazorCohostingOptions.UseRazorCohostServer = false;

        TestCode input = """
            <div></div>
            """;
        var document = CreateProjectAndRazorDocument(input.Text, RazorFileKind.Component);

        // Make sure the source generator has been run while cohosting is off, to simular Roslyn winning the initialization race
        Assert.Empty(await document.Project.GetSourceGeneratedDocumentsAsync(DisposalToken));

        var snapshotManager = OOPExportProvider.GetExportedValue<RemoteSnapshotManager>();

        // Now turn the source generator on, to simulate Razor starting up and initializing OOP
        RazorCohostingOptions.UseRazorCohostServer = true;

        var projectSnapshot = snapshotManager.GetSnapshot(document.Project);
        var documentSnapshot = projectSnapshot.GetDocument(document);
        await projectSnapshot.GetRequiredCodeDocumentAsync(documentSnapshot, DisposalToken);

        projectSnapshot = snapshotManager.GetSnapshot(document.Project);
        Assert.True(projectSnapshot.Project.IsRetryProject());

        var changedDocument = document.Project.Solution.WithAdditionalDocumentText(document.Id, SourceText.From("Different")).GetAdditionalDocument(document.Id);
        Assert.NotNull(changedDocument);

        projectSnapshot = snapshotManager.GetSnapshot(changedDocument.Project);
        documentSnapshot = projectSnapshot.GetDocument(changedDocument);
        await projectSnapshot.GetRequiredCodeDocumentAsync(documentSnapshot, DisposalToken);

        Assert.False(projectSnapshot.Project.IsRetryProject());
    }
}
