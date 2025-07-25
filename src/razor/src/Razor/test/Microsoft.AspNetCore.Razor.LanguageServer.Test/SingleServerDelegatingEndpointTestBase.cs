﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Moq;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public abstract partial class SingleServerDelegatingEndpointTestBase(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    private protected IDocumentContextFactory? DocumentContextFactory { get; private set; }
    private protected LanguageServerFeatureOptions? LanguageServerFeatureOptions { get; private set; }
    private protected IDocumentMappingService? DocumentMappingService { get; private set; }
    private protected IEditMappingService? EditMappingService { get; private set; }

    [MemberNotNull(nameof(DocumentContextFactory), nameof(LanguageServerFeatureOptions), nameof(DocumentMappingService), nameof(EditMappingService))]
    private protected async Task<TestLanguageServer> CreateLanguageServerAsync(
        RazorCodeDocument codeDocument,
        string razorFilePath,
        IEnumerable<(string, string)>? additionalRazorDocuments = null,
        bool multiTargetProject = true,
        Action<VSInternalClientCapabilities>? capabilitiesUpdater = null)
    {
        var projectKey = new ProjectKey("C:/path/to/obj");
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

        DocumentContextFactory = new TestDocumentContextFactory(razorFilePath, codeDocument);

        LanguageServerFeatureOptions = Mock.Of<LanguageServerFeatureOptions>(options =>
            options.SupportsFileManipulation == true &&
            options.SingleServerSupport == true &&
            options.CSharpVirtualDocumentSuffix == DefaultLanguageServerFeatureOptions.DefaultCSharpVirtualDocumentSuffix &&
            options.HtmlVirtualDocumentSuffix == DefaultLanguageServerFeatureOptions.DefaultHtmlVirtualDocumentSuffix,
            MockBehavior.Strict);

        DocumentMappingService = new LspDocumentMappingService(FilePathService, DocumentContextFactory, LoggerFactory);
        EditMappingService = new LspEditMappingService(DocumentMappingService, FilePathService, DocumentContextFactory);

        // Don't declare this with an 'await using'. TestLanguageServer will own the lifetime of this C# LSP server.
        var csharpServer = await CSharpTestLspServerHelpers.CreateCSharpLspServerAsync(
            csharpFiles,
            new VSInternalServerCapabilities
            {
                SupportsDiagnosticRequests = true,
            },
            razorMappingService: null,
            multiTargetProject,
            capabilitiesUpdater,
            DisposalToken);

        await csharpServer.OpenDocumentAsync(csharpDocumentUri, csharpSourceText.ToString(), DisposalToken);

        return new TestLanguageServer(csharpServer, csharpDocumentUri);
    }
}
