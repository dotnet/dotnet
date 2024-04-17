// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.Mef;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

[UseExportProvider]
public abstract partial class SingleServerDelegatingEndpointTestBase : LanguageServerTestBase
{
    private protected IDocumentContextFactory? DocumentContextFactory { get; private set; }
    private protected LanguageServerFeatureOptions? LanguageServerFeatureOptions { get; private set; }
    private protected IRazorDocumentMappingService? DocumentMappingService { get; private set; }

    protected SingleServerDelegatingEndpointTestBase(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [MemberNotNull(nameof(DocumentContextFactory), nameof(LanguageServerFeatureOptions), nameof(DocumentMappingService))]
    private protected async Task<TestLanguageServer> CreateLanguageServerAsync(
        RazorCodeDocument codeDocument,
        string razorFilePath,
        IEnumerable<(string, string)>? additionalRazorDocuments = null)
    {
        var projectKey = TestProjectKey.Create("");
        var csharpSourceText = codeDocument.GetCSharpSourceText();
        var csharpDocumentUri = new Uri(FilePathService.GetRazorCSharpFilePath(projectKey, razorFilePath));

        var csharpFiles = new List<(Uri, SourceText)>
        {
            (csharpDocumentUri, csharpSourceText)
        };

        if (additionalRazorDocuments is not null)
        {
            foreach ((var filePath, var contents) in additionalRazorDocuments)
            {
                var additionalDocument = CreateCodeDocument(contents, filePath: filePath);
                var additionalDocumentSourceText = additionalDocument.GetCSharpSourceText();
                var additionalDocumentUri = new Uri(FilePathService.GetRazorCSharpFilePath(projectKey, "C:/path/to/" + filePath));

                csharpFiles.Add((additionalDocumentUri, additionalDocumentSourceText));
            }
        }

        DocumentContextFactory = new TestDocumentContextFactory(razorFilePath, codeDocument, version: 1337);

        LanguageServerFeatureOptions = Mock.Of<LanguageServerFeatureOptions>(options =>
            options.SupportsFileManipulation == true &&
            options.SingleServerSupport == true &&
            options.CSharpVirtualDocumentSuffix == DefaultLanguageServerFeatureOptions.DefaultCSharpVirtualDocumentSuffix &&
            options.HtmlVirtualDocumentSuffix == DefaultLanguageServerFeatureOptions.DefaultHtmlVirtualDocumentSuffix,
            MockBehavior.Strict);

        DocumentMappingService = new RazorDocumentMappingService(FilePathService, DocumentContextFactory, LoggerFactory);

        var csharpServer = await CSharpTestLspServerHelpers.CreateCSharpLspServerAsync(
            csharpFiles,
            new VSInternalServerCapabilities
            {
                SupportsDiagnosticRequests = true,
            },
            razorSpanMappingService: null,
            DisposalToken);

        AddDisposable(csharpServer);

        await csharpServer.OpenDocumentAsync(csharpDocumentUri, csharpSourceText.ToString()).ConfigureAwait(false);

        return new TestLanguageServer(csharpServer, csharpDocumentUri, DisposalToken);
    }
}
