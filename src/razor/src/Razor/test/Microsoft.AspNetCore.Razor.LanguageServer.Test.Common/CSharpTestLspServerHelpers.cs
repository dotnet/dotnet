// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Test.Common.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
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
        VSInternalServerCapabilities serverCapabilities,
        CancellationToken cancellationToken) =>
        CreateCSharpLspServerAsync(csharpSourceText, csharpDocumentUri, serverCapabilities, new EmptyMappingService(), cancellationToken);

    public static Task<CSharpTestLspServer> CreateCSharpLspServerAsync(
        SourceText csharpSourceText,
        Uri csharpDocumentUri,
        VSInternalServerCapabilities serverCapabilities,
        IRazorSpanMappingService razorSpanMappingService,
        CancellationToken cancellationToken)
    {
        var files = new[]
        {
            (csharpDocumentUri, csharpSourceText)
        };

        return CreateCSharpLspServerAsync(files, serverCapabilities, razorSpanMappingService, cancellationToken);
    }

    public static async Task<CSharpTestLspServer> CreateCSharpLspServerAsync(IEnumerable<(Uri Uri, SourceText SourceText)> files, VSInternalServerCapabilities serverCapabilities, IRazorSpanMappingService razorSpanMappingService, CancellationToken cancellationToken)
    {
        var cSharpFiles = files.Select(f => new CSharpFile(f.Uri, f.SourceText));

        var exportProvider = RoslynTestCompositions.Roslyn.ExportProviderFactory.CreateExportProvider();
        var metadataReferences = await ReferenceAssemblies.Default.ResolveAsync(language: LanguageNames.CSharp, cancellationToken);
        var workspace = CreateCSharpTestWorkspace(cSharpFiles, exportProvider, metadataReferences, razorSpanMappingService);
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
        var projectInfoNet60 = ProjectInfo.Create(
            id: ProjectId.CreateNewId("TestProject (net6.0)"),
            version: VersionStamp.Default,
            name: "TestProject (net6.0)",
            assemblyName: "TestProject.dll",
            language: LanguageNames.CSharp,
            filePath: @"C:\TestSolution\TestProject.csproj",
            metadataReferences: metadataReferences).WithCompilationOutputInfo(new CompilationOutputInfo().WithAssemblyPath(@"C:\TestSolution\obj\TestProject.dll"));

        var projectInfoNet80 = ProjectInfo.Create(
            id: ProjectId.CreateNewId("TestProject (net8.0)"),
            version: VersionStamp.Default,
            name: "TestProject (net8.0)",
            assemblyName: "TestProject.dll",
            language: LanguageNames.CSharp,
            filePath: @"C:\TestSolution\TestProject.csproj",
            metadataReferences: metadataReferences);

        var projectInfos = new ProjectInfo[] { projectInfoNet60, projectInfoNet80 };

        var solutionInfo = SolutionInfo.Create(
            id: SolutionId.CreateNewId("TestSolution"),
            version: VersionStamp.Default,
            projects: projectInfos);

        workspace.AddSolution(solutionInfo);

        AddAnalyzersToWorkspace(workspace, exportProvider);

        // Add document to workspace. We use an IVT method to create the DocumentInfo variable because there's
        // a special constructor in Roslyn that will help identify the document as belonging to Razor.
        var languageServerFactory = exportProvider.GetExportedValue<IRazorLanguageServerFactoryWrapper>();

        var documentCount = 0;
        foreach (var (documentUri, csharpSourceText) in files)
        {
            // File path logic here has to match https://github.com/dotnet/roslyn/blob/3db75baf44332efd490bc0d166983103552370a3/src/Features/LanguageServer/Protocol/Extensions/ProtocolConversions.cs#L163
            // or the Roslyn side won't see the file in the project/solution
            var documentFilePath = documentUri.IsFile ? documentUri.LocalPath : documentUri.AbsolutePath;
            var textAndVersion = TextAndVersion.Create(csharpSourceText, VersionStamp.Default, documentFilePath);

            foreach (var projectInfo in projectInfos)
            {
                var documentInfo = languageServerFactory.CreateDocumentInfo(
                    id: DocumentId.CreateNewId(projectInfo.Id),
                    name: "TestDocument" + documentCount,
                    filePath: documentFilePath,
                    loader: TextLoader.From(textAndVersion),
                    razorDocumentServiceProvider: new TestRazorDocumentServiceProvider(razorSpanMappingService));

                workspace.AddDocument(documentInfo);
            }

            documentCount++;
        }

        return workspace;
    }

    private static void AddAnalyzersToWorkspace(Workspace workspace, ExportProvider exportProvider)
    {
        var analyzerLoader = RazorTestAnalyzerLoader.CreateAnalyzerAssemblyLoader();

        var analyzerPaths = new DirectoryInfo(AppContext.BaseDirectory).GetFiles("*.dll")
            .Where(f => f.Name.StartsWith("Microsoft.CodeAnalysis.", StringComparison.Ordinal) && !f.Name.Contains("LanguageServer") && !f.Name.Contains("Test.Utilities"))
            .Select(f => f.FullName)
            .ToImmutableArray();
        var references = new List<AnalyzerFileReference>();
        foreach (var analyzerPath in analyzerPaths)
        {
            if (File.Exists(analyzerPath))
            {
                references.Add(new AnalyzerFileReference(analyzerPath, analyzerLoader));
            }
        }

        workspace.TryApplyChanges(workspace.CurrentSolution.WithAnalyzerReferences(references));

        // Make sure Roslyn is producing diagnostics for our workspace
        var razorTestAnalyzerLoader = exportProvider.GetExportedValue<RazorTestAnalyzerLoader>();
        razorTestAnalyzerLoader.InitializeDiagnosticsServices(workspace);
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
