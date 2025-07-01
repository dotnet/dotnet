// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Immutable;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.SemanticTokens;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.LanguageServer;

public class RazorSemanticTokensRangeEndpointBenchmark : RazorLanguageServerBenchmarkBase
{
    private IRazorSemanticTokensInfoService RazorSemanticTokenService { get; set; }

    private SemanticTokensRangeEndpoint SemanticTokensRangeEndpoint { get; set; }

    private Uri DocumentUri => DocumentContext.Uri;

    private DocumentContext DocumentContext { get; set; }

    private LspRange Range { get; set; }

    private string PagesDirectory { get; set; }

    private string ProjectFilePath { get; set; }

    private string TargetPath { get; set; }

    private CancellationToken CancellationToken { get; set; }

    private RazorRequestContext RequestContext { get; set; }

    [Params(0, 100, 1000)]
    public int NumberOfCsSemanticRangesToReturn { get; set; }

    private static ImmutableArray<SemanticRange>? PregeneratedRandomSemanticRanges { get; set; }

    [GlobalSetup]
    public async Task InitializeRazorSemanticAsync()
    {
        EnsureServicesInitialized();

        var projectRoot = Path.Combine(Helpers.GetTestAppsPath(), "ComponentApp");
        ProjectFilePath = Path.Combine(projectRoot, "ComponentApp.csproj");
        PagesDirectory = Path.Combine(projectRoot, "Components", "Pages");
        var filePath = Path.Combine(PagesDirectory, "SemanticTokens.razor");
        TargetPath = "/Components/Pages/SemanticTokens.razor";

        var documentUri = new Uri(filePath);
        var documentSnapshot = await GetDocumentSnapshotAsync(ProjectFilePath, filePath, TargetPath);
        DocumentContext = new DocumentContext(documentUri, documentSnapshot, projectContext: null);

        var razorOptionsMonitor = RazorLanguageServerHost.GetRequiredService<RazorLSPOptionsMonitor>();
        var clientCapabilitiesService = new BenchmarkClientCapabilitiesService(new VSInternalClientCapabilities() { SupportsVisualStudioExtensions = true });
        var razorSemanticTokensLegendService = new RazorSemanticTokensLegendService(clientCapabilitiesService);
        SemanticTokensRangeEndpoint = new SemanticTokensRangeEndpoint(RazorSemanticTokenService, razorSemanticTokensLegendService, razorOptionsMonitor, telemetryReporter: null);

        var text = await DocumentContext.GetSourceTextAsync(CancellationToken.None).ConfigureAwait(false);
        Range = LspFactory.CreateRange(
            start: (0, 0),
            end: (text.Lines.Count - 1, text.Lines[^1].Span.Length - 1));

        RequestContext = new RazorRequestContext(
            DocumentContext,
            RazorLanguageServerHost.GetRequiredService<LspServices>(),
            "lsp/method",
            uri: null);

        var random = new Random();
        var codeDocument = await DocumentContext.GetCodeDocumentAsync(CancellationToken);
        var builder = ImmutableArray.CreateBuilder<SemanticRange>(NumberOfCsSemanticRangesToReturn);
        for (var i = 0; i < NumberOfCsSemanticRangesToReturn; i++)
        {
            var startLine = random.Next(Range.Start.Line, Range.End.Line);
            var startChar = random.Next(0, codeDocument.Source.Text.Lines[startLine].Span.Length);
            var endLine = random.Next(startLine, Range.End.Line);
            var endChar = startLine == endLine
                ? random.Next(startChar, codeDocument.Source.Text.Lines[startLine].Span.Length)
                : random.Next(0, codeDocument.Source.Text.Lines[endLine].Span.Length);

            builder.Add(
                new SemanticRange(random.Next(), startLine, startChar, endLine, endChar, 0, fromRazor: false));
        }

        PregeneratedRandomSemanticRanges = builder.ToImmutableAndClear();
    }

    [Benchmark(Description = "Razor Semantic Tokens Range Endpoint")]
    public async Task RazorSemanticTokensRangeEndpointRangesAsync()
    {
        var textDocumentIdentifier = new TextDocumentIdentifier { DocumentUri = new(DocumentUri) };
        var request = new SemanticTokensRangeParams { Range = Range, TextDocument = textDocumentIdentifier };

        await SemanticTokensRangeEndpoint.HandleRequestAsync(request, RequestContext, CancellationToken);
    }

    [GlobalCleanup]
    public async Task CleanupServerAsync()
    {
        var server = RazorLanguageServerHost.GetTestAccessor().Server;

        await server.ShutdownAsync();
        await server.ExitAsync();
    }

    protected internal override void Builder(IServiceCollection collection)
    {
        collection.AddSingleton<IRazorSemanticTokensInfoService, TestCustomizableRazorSemanticTokensInfoService>();
    }

    private void EnsureServicesInitialized()
    {
        RazorSemanticTokenService = RazorLanguageServerHost.GetRequiredService<IRazorSemanticTokensInfoService>();
    }

    internal class TestCustomizableRazorSemanticTokensInfoService : RazorSemanticTokensInfoService
    {
        public TestCustomizableRazorSemanticTokensInfoService(
            LanguageServerFeatureOptions languageServerFeatureOptions,
            IDocumentMappingService documentMappingService,
            RazorSemanticTokensLegendService razorSemanticTokensLegendService,
            ILoggerFactory loggerFactory)
            : base(documentMappingService, razorSemanticTokensLegendService, csharpSemanticTokensProvider: null!, languageServerFeatureOptions, loggerFactory)
        {
        }

        // We can't get C# responses without significant amounts of extra work, so let's just shim it for now, any non-Null result is fine.
        protected override Task<ImmutableArray<SemanticRange>?> GetCSharpSemanticRangesAsync(
            DocumentContext documentContext,
            RazorCodeDocument codeDocument,
            LinePositionSpan razorSpan,
            bool colorBackground,
            Guid correlationId,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<ImmutableArray<SemanticRange>?>(PregeneratedRandomSemanticRanges);
        }
    }
}
