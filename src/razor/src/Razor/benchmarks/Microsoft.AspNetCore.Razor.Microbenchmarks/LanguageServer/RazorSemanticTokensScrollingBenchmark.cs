// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using static Microsoft.AspNetCore.Razor.Microbenchmarks.LanguageServer.RazorSemanticTokensBenchmark;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.LanguageServer;

public class RazorSemanticTokensScrollingBenchmark : RazorLanguageServerBenchmarkBase
{
    private IRazorSemanticTokensInfoService RazorSemanticTokenService { get; set; }

    private DocumentVersionCache VersionCache { get; set; }

    private VersionedDocumentContext DocumentContext { get; set; }

    private Uri DocumentUri => DocumentContext.Uri;

    private IDocumentSnapshot DocumentSnapshot => DocumentContext.Snapshot;

    private Range Range { get; set; }

    private ProjectSnapshotManagerDispatcher ProjectSnapshotManagerDispatcher { get; set; }

    private RazorSemanticTokensLegend SemanticTokensLegend { get; set; }

    private string PagesDirectory { get; set; }

    private string ProjectFilePath { get; set; }

    private string TargetPath { get; set; }

    [GlobalSetup(Target = nameof(RazorSemanticTokensRangeScrollingAsync))]
    public async Task InitializeRazorSemanticAsync()
    {
        EnsureServicesInitialized();

        var projectRoot = Path.Combine(RepoRoot, "src", "Razor", "test", "testapps", "ComponentApp");
        ProjectFilePath = Path.Combine(projectRoot, "ComponentApp.csproj");
        PagesDirectory = Path.Combine(projectRoot, "Components", "Pages");
        var filePath = Path.Combine(PagesDirectory, $"FormattingTest.razor");
        TargetPath = "/Components/Pages/FormattingTest.razor";

        var documentUri = new Uri(filePath);
        var documentSnapshot = GetDocumentSnapshot(ProjectFilePath, filePath, TargetPath);
        DocumentContext = new VersionedDocumentContext(documentUri, documentSnapshot, projectContext: null, version: 1);

        SemanticTokensLegend = new RazorSemanticTokensLegend(new VSInternalClientCapabilities() { SupportsVisualStudioExtensions = true });

        var text = await DocumentSnapshot.GetTextAsync().ConfigureAwait(false);
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
                Character = 0
            }
        };
    }

    private const int WindowSize = 10;

    [Benchmark(Description = "Razor Semantic Tokens Range Scrolling")]
    public async Task RazorSemanticTokensRangeScrollingAsync()
    {
        var textDocumentIdentifier = new TextDocumentIdentifier()
        {
            Uri = DocumentUri
        };
        var cancellationToken = CancellationToken.None;
        var correlationId = Guid.Empty;
        var documentVersion = 1;

        await UpdateDocumentAsync(documentVersion, DocumentSnapshot).ConfigureAwait(false);

        var documentLineCount = Range.End.Line;

        var lineCount = 0;
        while (lineCount != documentLineCount)
        {
            var newLineCount = Math.Min(lineCount + WindowSize, documentLineCount);
            var range = new Range
            {
                Start = new Position(lineCount, 0),
                End = new Position(newLineCount, 0)
            };
            await RazorSemanticTokenService!.GetSemanticTokensAsync(
                textDocumentIdentifier,
                range,
                DocumentContext,
                SemanticTokensLegend,
                correlationId,
                cancellationToken);

            lineCount = newLineCount;
        }
    }

    private async Task UpdateDocumentAsync(int newVersion, IDocumentSnapshot documentSnapshot)
    {
        await ProjectSnapshotManagerDispatcher!.RunOnDispatcherThreadAsync(
            () => VersionCache!.TrackDocumentVersion(documentSnapshot, newVersion), CancellationToken.None).ConfigureAwait(false);
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
}
