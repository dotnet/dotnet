// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Text.Adornments;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.FindAllReferences;

public class FindAllReferencesEndpointTest(ITestOutputHelper testOutput) : SingleServerDelegatingEndpointTestBase(testOutput)
{
    [Fact]
    public Task FindCSharpReferences()
        => VerifyCSharpFindAllReferencesAsyncAsync("""

            @{
                const string [|$$S|] = "";

                string M()
                {
                    return [|S|];
                }

                string N()
                {
                    return [|S|];
                }
            }

            <p>@[|S|]</p>
            """);

    private async Task VerifyCSharpFindAllReferencesAsyncAsync(string input)
    {
        // Arrange
        TestFileMarkupParser.GetPositionAndSpans(input, out var output, out int cursorPosition, out ImmutableArray<TextSpan> expectedSpans);

        var codeDocument = CreateCodeDocument(output);
        var razorFilePath = "C:/path/to/file.razor";

        var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath, multiTargetProject: false);
        var projectManager = CreateProjectSnapshotManager();
        var hostProject = TestHostProject.Create("C:/path/to/project.csproj");
        var hostDocument = TestHostDocument.Create(TestProjectData.SomeProject, razorFilePath);

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(hostProject);
            updater.DocumentAdded(hostProject.Key, hostDocument, TextLoader.From(TextAndVersion.Create(codeDocument.Source.Text, VersionStamp.Default)));
        });

        var endpoint = new FindAllReferencesEndpoint(
            LanguageServerFeatureOptions, DocumentMappingService, languageServer, LoggerFactory, FilePathService, projectManager);

        var sourceText = codeDocument.Source.Text;

        var request = new ReferenceParams
        {
            Context = new ReferenceContext()
            {
                IncludeDeclaration = true
            },
            TextDocument = new TextDocumentIdentifier
            {
                Uri = new Uri(razorFilePath)
            },
            Position = sourceText.GetPosition(cursorPosition)
        };
        Assert.True(DocumentContextFactory.TryCreate(request.TextDocument, out var documentContext));
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);

        Assert.Equal(expectedSpans.Length, result.Length);

        var i = 0;
        foreach (var referenceItem in result.OrderBy(l => l.Location.Range.Start.Line))
        {
            Assert.Equal(new Uri(razorFilePath), referenceItem.Location.Uri);

            var expectedRange = codeDocument.Source.Text.GetRange(expectedSpans[i]);
            Assert.Equal(expectedRange, referenceItem.Location.Range);

            var expected = codeDocument.Source.Text.Lines[referenceItem.Location.Range.Start.Line].ToString();
            Assert.Equal(expected.Trim(), GetText(referenceItem));

            i++;
        }
    }

    private static string GetText(VSInternalReferenceItem referenceItem)
    {
        if (referenceItem.Text is ClassifiedTextElement classifiedText)
        {
            return string.Join("", classifiedText.Runs.Select(s => s.Text));
        }

        return referenceItem.Text.AssumeNotNull().ToString().AssumeNotNull();
    }
}
