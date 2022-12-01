// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Test.Common.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Composition;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test.Common;

internal class CSharpTestLspServerHelpers
{
    private const string EditRangeSetting = "editRange";

    public static Task<CSharpTestLspServer> CreateCSharpLspServerAsync(
        SourceText csharpSourceText,
        Uri csharpDocumentUri,
        ServerCapabilities serverCapabilities,
        CancellationToken cancellationToken) =>
        CreateCSharpLspServerAsync(csharpSourceText, csharpDocumentUri, serverCapabilities, new EmptyMappingService(), cancellationToken);

    public static async Task<CSharpTestLspServer> CreateCSharpLspServerAsync(
        SourceText csharpSourceText,
        Uri csharpDocumentUri,
        ServerCapabilities serverCapabilities,
        IRazorSpanMappingService razorSpanMappingService,
        CancellationToken cancellationToken)
    {
        var files = new[]
        {
            new CSharpFile(csharpDocumentUri, csharpSourceText)
        };

        var exportProvider = RoslynTestCompositions.Roslyn.ExportProviderFactory.CreateExportProvider();
        var metadataReferences = await ReferenceAssemblies.Default.ResolveAsync(language: LanguageNames.CSharp, cancellationToken);
        var workspace = CreateCSharpTestWorkspace(files, exportProvider, metadataReferences, razorSpanMappingService);
        var clientCapabilities = new VSInternalClientCapabilities
        {
            SupportsVisualStudioExtensions = true,
            TextDocument = new TextDocumentClientCapabilities
            {
                Completion = new VSInternalCompletionSetting
                {
                    CompletionListSetting = new()
                    {
                        ItemDefaults = new string[] { EditRangeSetting }
                    },
                    CompletionItem = new()
                    {
                        SnippetSupport = true
                    }
                },
            },
            SupportsDiagnosticRequests = true,
        };

        var testLspServer = await CSharpTestLspServer.CreateAsync(
            workspace, exportProvider, clientCapabilities, serverCapabilities, cancellationToken);

        return testLspServer;
    }

    private static AdhocWorkspace CreateCSharpTestWorkspace(
        IEnumerable<CSharpFile> files,
        ExportProvider exportProvider,
        ImmutableArray<MetadataReference> metadataReferences,
        IRazorSpanMappingService razorSpanMappingService)
    {
        var hostServices = MefHostServices.Create(exportProvider.AsCompositionContext());
        var workspace = TestWorkspace.Create(hostServices);

        // Add project and solution to workspace
        var projectInfo = ProjectInfo.Create(
            id: ProjectId.CreateNewId("TestProject"),
            version: VersionStamp.Default,
            name: "TestProject",
            assemblyName: "TestProject.dll",
            language: LanguageNames.CSharp,
            filePath: @"C:\TestSolution\TestProject.csproj",
            metadataReferences: metadataReferences);

        var solutionInfo = SolutionInfo.Create(
            id: SolutionId.CreateNewId("TestSolution"),
            version: VersionStamp.Default,
            projects: new ProjectInfo[] { projectInfo });

        workspace.AddSolution(solutionInfo);

        // Add document to workspace. We use an IVT method to create the DocumentInfo variable because there's
        // a special constructor in Roslyn that will help identify the document as belonging to Razor.
        var languageServerFactory = exportProvider.GetExportedValue<IRazorLanguageServerFactoryWrapper>();

        var documentCount = 0;
        foreach (var (documentUri, csharpSourceText) in files)
        {
            var documentFilePath = documentUri.AbsolutePath;
            var textAndVersion = TextAndVersion.Create(csharpSourceText, VersionStamp.Default, documentFilePath);
            var documentInfo = languageServerFactory.CreateDocumentInfo(
                id: DocumentId.CreateNewId(projectInfo.Id),
                name: "TestDocument" + documentCount,
                filePath: documentFilePath,
                loader: TextLoader.From(textAndVersion),
                razorDocumentServiceProvider: new TestRazorDocumentServiceProvider(razorSpanMappingService));

            workspace.AddDocument(documentInfo);
            documentCount++;
        }

        return workspace;
    }

    private record CSharpFile(Uri DocumentUri, SourceText CSharpSourceText);

    private class EmptyMappingService : IRazorSpanMappingService
    {
        public Task<ImmutableArray<RazorMappedSpanResult>> MapSpansAsync(Document document, IEnumerable<TextSpan> spans, CancellationToken cancellationToken)
        {
            var result = Enumerable.Empty<RazorMappedSpanResult>().ToImmutableArray();
            return Task.FromResult(result);
        }
    }
}
