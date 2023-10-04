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
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.LanguageServer;

public class RazorSemanticTokensBenchmark : RazorLanguageServerBenchmarkBase
{
    private IRazorSemanticTokensInfoService RazorSemanticTokenService { get; set; }

    private DocumentVersionCache VersionCache { get; set; }

    private Uri DocumentUri => DocumentContext.Uri;

    private IDocumentSnapshot DocumentSnapshot => DocumentContext.Snapshot;

    private VersionedDocumentContext DocumentContext { get; set; }

    private Range Range { get; set; }

    private ProjectSnapshotManagerDispatcher ProjectSnapshotManagerDispatcher { get; set; }

    private RazorSemanticTokensLegend SemanticTokensLegend { get; set; }

    private string PagesDirectory { get; set; }

    private string ProjectFilePath { get; set; }

    private string TargetPath { get; set; }

    [ParamsAllValues]
    public bool WithMultiLineComment { get; set; }

    [GlobalSetup(Target = nameof(RazorSemanticTokensRangeAsync))]
    public async Task InitializeRazorSemanticAsync()
    {
        EnsureServicesInitialized();

        var projectRoot = Path.Combine(RepoRoot, "src", "Razor", "test", "testapps", "ComponentApp");
        ProjectFilePath = Path.Combine(projectRoot, "ComponentApp.csproj");
        PagesDirectory = Path.Combine(projectRoot, "Components", "Pages");

        var fileName = WithMultiLineComment ? "SemanticTokens_LargeMultiLineComment" : "SemanticTokens";
        var filePath = Path.Combine(PagesDirectory, $"{fileName}.razor");
        TargetPath = $"/Components/Pages/{fileName}.razor";

        var documentUri = new Uri(filePath);
        var documentSnapshot = GetDocumentSnapshot(ProjectFilePath, filePath, TargetPath);
        var version = 1;
        DocumentContext = new VersionedDocumentContext(documentUri, documentSnapshot, projectContext: null, version);

        var text = await DocumentContext.GetSourceTextAsync(CancellationToken.None).ConfigureAwait(false);
        Range = new Range
        {
            Start = new Position
            {
                Line = 0,
                Character = 0
            },
            End = new Position
            {
                Line = text.Lines.Count - 1,
                Character = text.Lines.Last().Span.Length - 1
            }
        };

        SemanticTokensLegend = new RazorSemanticTokensLegend(new VSInternalClientCapabilities() { SupportsVisualStudioExtensions = true });
    }

    [Benchmark(Description = "Razor Semantic Tokens Range Handling")]
    public async Task RazorSemanticTokensRangeAsync()
    {
        var textDocumentIdentifier = new TextDocumentIdentifier
        {
            Uri = DocumentUri
        };
        var cancellationToken = CancellationToken.None;
        var documentVersion = 1;
        var correlationId = Guid.Empty;

        await UpdateDocumentAsync(documentVersion, DocumentSnapshot, cancellationToken).ConfigureAwait(false);
        await RazorSemanticTokenService.GetSemanticTokensAsync(
            textDocumentIdentifier, Range, DocumentContext, SemanticTokensLegend, correlationId, cancellationToken).ConfigureAwait(false);
    }

    private async Task UpdateDocumentAsync(int newVersion, IDocumentSnapshot documentSnapshot, CancellationToken cancellationToken)
    {
        await ProjectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync(
            () => VersionCache.TrackDocumentVersion(documentSnapshot, newVersion), cancellationToken).ConfigureAwait(false);
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
        collection.AddSingleton<IRazorSemanticTokensInfoService, TestRazorSemanticTokensInfoService>();
    }

    private void EnsureServicesInitialized()
    {
        var languageServer = RazorLanguageServer.GetInnerLanguageServerForTesting();
        RazorSemanticTokenService = languageServer.GetRequiredService<IRazorSemanticTokensInfoService>();
        VersionCache = languageServer.GetRequiredService<DocumentVersionCache>();
        ProjectSnapshotManagerDispatcher = languageServer.GetRequiredService<ProjectSnapshotManagerDispatcher>();
    }

    internal class TestRazorSemanticTokensInfoService : RazorSemanticTokensInfoService
    {
        public TestRazorSemanticTokensInfoService(
            ClientNotifierServiceBase languageServer,
            LanguageServerFeatureOptions languageServerFeatureOptions,
            IRazorDocumentMappingService documentMappingService,
            RazorLSPOptionsMonitor razorLSPOptionsMonitor,
            ILoggerFactory loggerFactory)
            : base(languageServer, documentMappingService, razorLSPOptionsMonitor, languageServerFeatureOptions, loggerFactory)
        {
        }

        // We can't get C# responses without significant amounts of extra work, so let's just shim it for now, any non-Null result is fine.
        internal override Task<ImmutableArray<SemanticRange>?> GetCSharpSemanticRangesAsync(
            RazorCodeDocument codeDocument,
            TextDocumentIdentifier textDocumentIdentifier,
            Range razorRange,
            RazorSemanticTokensLegend razorSemanticTokensLegend,
            long documentVersion,
            Guid correlationId,
            CancellationToken cancellationToken,
            string previousResultId = null)
        {
            return Task.FromResult<ImmutableArray<SemanticRange>?>(ImmutableArray<SemanticRange>.Empty);
        }
    }
}
