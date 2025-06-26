// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectContexts;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.CodeAnalysis.Razor;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test.ProjectContexts;

public class ProjectContextsEndpointTest(ITestOutputHelper testOutput) : SingleServerDelegatingEndpointTestBase(testOutput)
{
    [Fact]
    public async Task GetProjectContexts_ReturnsExpected()
    {
        var input = """
            <p> Hello World </p>
            """;

        var codeDocument = CreateCodeDocument(input);
        var razorFilePath = "C:/path/to/file.razor";

        await using var languageServer = await CreateLanguageServerAsync(codeDocument, razorFilePath);

        var endpoint = new ProjectContextsEndpoint(languageServer);

        var request = new VSGetProjectContextsParams()
        {
            TextDocument = new TextDocumentItem()
            {
                DocumentUri = new(new Uri(razorFilePath)),
                LanguageId = "razor",
                Text = input,
                Version = 1337
            }
        };

        Assert.True(DocumentContextFactory.TryCreate(request.TextDocument.DocumentUri.GetRequiredParsedUri(), out var documentContext));
        var requestContext = CreateRazorRequestContext(documentContext);

        var results = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        Assert.NotNull(results);
        Assert.Collection(results.ProjectContexts.OrderBy(c => c.Label),
            context =>
            {
                Assert.Equal(VSProjectKind.CSharp, context.Kind);
                Assert.Equal("TestProject (net6.0)", context.Label);
                Assert.Contains("|TestProject (net6.0)", context.Id);
            },
            context =>
            {
                Assert.Equal(VSProjectKind.CSharp, context.Kind);
                Assert.Equal("TestProject (net8.0)", context.Label);
                Assert.Contains("|TestProject (net8.0)", context.Id);
            });
    }
}
