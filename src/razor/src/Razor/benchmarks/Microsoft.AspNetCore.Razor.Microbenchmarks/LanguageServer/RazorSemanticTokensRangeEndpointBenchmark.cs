// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.SemanticTokens;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.LanguageServer;

public class RazorSemanticTokensRangeEndpointBenchmark : RazorLanguageServerBenchmarkBase
{
    private IRazorSemanticTokensInfoService RazorSemanticTokenService { get; set; }

    private SemanticTokensRangeEndpoint SemanticTokensRangeEndpoint { get; set; }

    private IDocumentVersionCache VersionCache { get; set; }

    private Uri DocumentUri => DocumentContext.Uri;

    private IDocumentSnapshot DocumentSnapshot => DocumentContext.Snapshot;

    private VersionedDocumentContext DocumentContext { get; set; }

    private Range Range { get; set; }

    private ProjectSnapshotManagerDispatcher ProjectSnapshotManagerDispatcher { get; set; }

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

        var loggerFactory = RazorLanguageServer.GetRequiredService<IRazorLoggerFactory>();

        var projectRoot = Path.Combine(RepoRoot, "src", "Razor", "test", "testapps", "ComponentApp");
        ProjectFilePath = Path.Combine(projectRoot, "ComponentApp.csproj");
        PagesDirectory = Path.Combine(projectRoot, "Components", "Pages");
        var filePath = Path.Combine(PagesDirectory, $"SemanticTokens.razor");
        TargetPath = "/Components/Pages/SemanticTokens.razor";

        var documentUri = new Uri(filePath);
        var documentSnapshot = await GetDocumentSnapshotAsync(ProjectFilePath, filePath, TargetPath);
        var version = 1;
        DocumentContext = new VersionedDocumentContext(documentUri, documentSnapshot, projectContext: null, version);

        var razorOptionsMonitor = RazorLanguageServer.GetRequiredService<RazorLSPOptionsMonitor>();
        var clientCapabilitiesService = new BenchmarkClientCapabilitiesService(new VSInternalClientCapabilities() { SupportsVisualStudioExtensions = true });
        var razorSemanticTokensLegendService = new RazorSemanticTokensLegendService(clientCapabilitiesService);
        SemanticTokensRangeEndpoint = new SemanticTokensRangeEndpoint(RazorSemanticTokenService, razorSemanticTokensLegendService, razorOptionsMonitor, telemetryReporter: null);

        var text = await DocumentContext.GetSourceTextAsync(CancellationToken.None).ConfigureAwait(false);
        Range = new Range
        {
            Start = new Position { Line = 0, Character = 0 },
            End = new Position { Line = text.Lines.Count - 1, Character = text.Lines.Last().Span.Length - 1 }
        };

        var documentVersion = 1;
        CancellationToken = CancellationToken.None;
        await UpdateDocumentAsync(documentVersion, DocumentSnapshot, CancellationToken).ConfigureAwait(false);

        var languageServer = RazorLanguageServer.GetInnerLanguageServerForTesting();
        RequestContext = new RazorRequestContext(DocumentContext, languageServer.GetLspServices(), "lsp/method", uri: null);

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

        PregeneratedRandomSemanticRanges = builder.DrainToImmutable();
    }

    [Benchmark(Description = "Razor Semantic Tokens Range Endpoint")]
    public async Task RazorSemanticTokensRangeEndpointRangesAsync()
    {
        var textDocumentIdentifier = new TextDocumentIdentifier { Uri = DocumentUri };
        var request = new SemanticTokensRangeParams { Range = Range, TextDocument = textDocumentIdentifier };

        await SemanticTokensRangeEndpoint.HandleRequestAsync(request, RequestContext, CancellationToken);
    }

    private async Task UpdateDocumentAsync(int newVersion, IDocumentSnapshot documentSnapshot,
        CancellationToken cancellationToken)
    {
        await ProjectSnapshotManagerDispatcher.RunAsync(
                () => VersionCache.TrackDocumentVersion(documentSnapshot, newVersion), cancellationToken)
            .ConfigureAwait(false);
    }

    [GlobalCleanup]
    public async Task CleanupServerAsync()
    {
        var innerServer = RazorLanguageServer.GetInnerLanguageServerForTesting();
        await innerServer.ShutdownAsync();
        await innerServer.ExitAsync();
    }

    protected internal override void Builder(IServiceCollection collection)
    {
        collection.AddSingleton<IRazorSemanticTokensInfoService, TestCustomizableRazorSemanticTokensInfoService>();
    }

    private void EnsureServicesInitialized()
    {
        var languageServer = RazorLanguageServer.GetInnerLanguageServerForTesting();
        RazorSemanticTokenService = languageServer.GetRequiredService<IRazorSemanticTokensInfoService>();
        VersionCache = languageServer.GetRequiredService<IDocumentVersionCache>();
        ProjectSnapshotManagerDispatcher = languageServer.GetRequiredService<ProjectSnapshotManagerDispatcher>();
    }

    internal class TestCustomizableRazorSemanticTokensInfoService : RazorSemanticTokensInfoService
    {
        public TestCustomizableRazorSemanticTokensInfoService(
            LanguageServerFeatureOptions languageServerFeatureOptions,
            IRazorDocumentMappingService documentMappingService,
            RazorSemanticTokensLegendService razorSemanticTokensLegendService,
            IRazorLoggerFactory loggerFactory)
            : base(documentMappingService, razorSemanticTokensLegendService, csharpSemanticTokensProvider: null!, languageServerFeatureOptions, loggerFactory)
        {
        }

        // We can't get C# responses without significant amounts of extra work, so let's just shim it for now, any non-Null result is fine.
        protected override Task<ImmutableArray<SemanticRange>?> GetCSharpSemanticRangesAsync(
            VersionedDocumentContext documentContext,
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
