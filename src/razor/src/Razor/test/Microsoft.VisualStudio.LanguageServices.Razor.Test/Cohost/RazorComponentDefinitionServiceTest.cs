// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.GoToDefinition;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

public class RazorComponentDefinitionServiceTest(ITestOutputHelper testOutputHelper) : CohostEndpointTestBase(testOutputHelper)
{
    // PREAMBLE: Right now these tests are about ensuring we don't accidentally introduce a future bug
    //           in the generated document handling code in the RazorComponentDefinitionService in OOP.
    //           Right now in cohosting none of the code under test is actually used. This is because
    //           the logic for manually looking up properties from attributes is only necessary when
    //           "Single Server Mode" is off, which is currently only VS Code. When cohosting comes to
    //           VS Code, that will no longer be true, and VS Code will use the same code paths as VS,
    //           even then these tests will be exercising uncalled code.
    //           The tests, and the "ignoreAttributes" parameter in the call to GetDefinitionAsync, should
    //           be deleted entirely at that point. "ignoreAttributes" will essentially always be true,
    //           as directly calling Roslyn provides better results.

    [Fact]
    public async Task Do()
    {
        TestCode input = """
            <SurveyPrompt Ti$$tle="InputValue" />

            @code
            {
                private string? InputValue { get; set; }

                private void BindAfter()
                {
                }
            }
            """;

        TestCode surveyPrompt = """
            @namespace SomeProject

            <div></div>

            @code
            {
                [Parameter]
                public string [|Title|] { get; set; }
            }
            """;

        await VerifyDefinitionAsync(input, surveyPrompt,
            (FileName("SurveyPrompt.razor"), surveyPrompt.Text));
    }

    private async Task VerifyDefinitionAsync(TestCode input, TestCode expectedDocument, params (string fileName, string contents)[]? additionalFiles)
    {
        var document = CreateProjectAndRazorDocument(input.Text, FileKinds.Component, additionalFiles);

        var service = OOPExportProvider.GetExportedValue<IRazorComponentDefinitionService>();
        var snapshotManager = OOPExportProvider.GetExportedValue<RemoteSnapshotManager>();
        var documentMappingService = OOPExportProvider.GetExportedValue<IDocumentMappingService>();

        var documentSnapshot = snapshotManager.GetSnapshot(document);
        var codeDocument = await documentSnapshot.GetGeneratedOutputAsync(DisposalToken);
        var positionInfo = documentMappingService.GetPositionInfo(codeDocument, input.Position);

        var location = await service.GetDefinitionAsync(
            documentSnapshot,
            positionInfo,
            solutionQueryOperations: documentSnapshot.ProjectSnapshot.SolutionSnapshot,
            ignoreAttributes: false,
            DisposalToken);

        Assert.NotNull(location);

        var text = SourceText.From(expectedDocument.Text);
        var range = text.GetRange(expectedDocument.Span);
        Assert.Equal(range, location.Range);
    }

    private static string FileName(string projectRelativeFileName)
        => Path.Combine(TestProjectData.SomeProjectPath, projectRelativeFileName);
}
