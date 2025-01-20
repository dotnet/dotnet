// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Immutable;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.SemanticTokens;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Range = Microsoft.VisualStudio.LanguageServer.Protocol.Range;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.LanguageServer;

public class RazorSemanticTokensBenchmark : RazorLanguageServerBenchmarkBase
{
    private IRazorSemanticTokensInfoService RazorSemanticTokenService { get; set; }

    private Uri DocumentUri => DocumentContext.Uri;

    private IDocumentSnapshot DocumentSnapshot => DocumentContext.Snapshot;

    private DocumentContext DocumentContext { get; set; }

    private Range Range { get; set; }

    private string PagesDirectory { get; set; }

    private string ProjectFilePath { get; set; }

    private string TargetPath { get; set; }

    [ParamsAllValues]
    public bool WithMultiLineComment { get; set; }

    [GlobalSetup(Target = nameof(RazorSemanticTokensRangeAsync))]
    public async Task InitializeRazorSemanticAsync()
    {
        EnsureServicesInitialized();

        var projectRoot = Path.Combine(Helpers.GetTestAppsPath(), "ComponentApp");
        ProjectFilePath = Path.Combine(projectRoot, "ComponentApp.csproj");
        PagesDirectory = Path.Combine(projectRoot, "Components", "Pages");

        var fileName = WithMultiLineComment ? "SemanticTokens_LargeMultiLineComment" : "SemanticTokens";
        var filePath = Path.Combine(PagesDirectory, $"{fileName}.razor");
        TargetPath = $"/Components/Pages/{fileName}.razor";

        var documentUri = new Uri(filePath);
        var documentSnapshot = await GetDocumentSnapshotAsync(ProjectFilePath, filePath, TargetPath);
        DocumentContext = new DocumentContext(documentUri, documentSnapshot, projectContext: null);

        var text = await DocumentContext.GetSourceTextAsync(CancellationToken.None).ConfigureAwait(false);
        Range = VsLspFactory.CreateRange(
            start: (0, 0),
            end: (text.Lines.Count - 1, text.Lines[^1].Span.Length - 1));
    }

    [Benchmark(Description = "Razor Semantic Tokens Range Handling")]
    public async Task RazorSemanticTokensRangeAsync()
    {
        var cancellationToken = CancellationToken.None;

        await RazorSemanticTokenService.GetSemanticTokensAsync(DocumentContext, Range.ToLinePositionSpan(), colorBackground: false, Guid.Empty, cancellationToken: cancellationToken).ConfigureAwait(false);
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
        collection.AddSingleton<IRazorSemanticTokensInfoService, TestRazorSemanticTokensInfoService>();
    }

    private void EnsureServicesInitialized()
    {
        RazorSemanticTokenService = RazorLanguageServerHost.GetRequiredService<IRazorSemanticTokensInfoService>();
    }

    internal class TestRazorSemanticTokensInfoService : RazorSemanticTokensInfoService
    {
        public TestRazorSemanticTokensInfoService(
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
            LinePositionSpan razorRange,
            bool colorBackground,
            Guid correlationId,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<ImmutableArray<SemanticRange>?>(ImmutableArray<SemanticRange>.Empty);
        }
    }
}
