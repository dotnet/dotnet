// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectContexts;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test.ProjectContexts;

public class ProjectContextsEndpointTest : SingleServerDelegatingEndpointTestBase
{
    public ProjectContextsEndpointTest(ITestOutputHelper testOutput) : base(testOutput)
    {
    }

    [Fact]
    public async Task GetProjectContexts_ReturnsExpected()
    {
        var input = """
            <p> Hello World </p>
            """;

        var codeDocument = CreateCodeDocument(input);
        var razorFilePath = "C:/path/to/file.razor";

        await CreateLanguageServerAsync(codeDocument, razorFilePath);

        var endpoint = new ProjectContextsEndpoint(LanguageServer);

        var request = new VSGetProjectContextsParams()
        {
            TextDocument = new TextDocumentItem()
            {
                Uri = new Uri(razorFilePath),
                LanguageId = "razor",
                Text = input,
                Version = 1337
            }
        };

        var documentContext = DocumentContextFactory.TryCreateForOpenDocument(request.TextDocument.Uri);
        var requestContext = CreateRazorRequestContext(documentContext);

        var results = await endpoint.HandleRequestAsync(request, requestContext, default);

        Assert.NotNull(results);
        Assert.Collection(results.ProjectContexts,
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
