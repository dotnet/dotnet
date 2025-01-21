// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.SemanticTokens;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using static Microsoft.AspNetCore.Razor.Microbenchmarks.LanguageServer.RazorSemanticTokensBenchmark;
using Range = Microsoft.VisualStudio.LanguageServer.Protocol.Range;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.LanguageServer;

public class RazorSemanticTokensScrollingBenchmark : RazorLanguageServerBenchmarkBase
{
    private IRazorSemanticTokensInfoService RazorSemanticTokenService { get; set; }

    private DocumentContext DocumentContext { get; set; }

    private Uri DocumentUri => DocumentContext.Uri;

    private IDocumentSnapshot DocumentSnapshot => DocumentContext.Snapshot;

    private Range Range { get; set; }

    private string PagesDirectory { get; set; }

    private string ProjectFilePath { get; set; }

    private string TargetPath { get; set; }

    [GlobalSetup(Target = nameof(RazorSemanticTokensRangeScrollingAsync))]
    public async Task InitializeRazorSemanticAsync()
    {
        EnsureServicesInitialized();

        var projectRoot = Path.Combine(Helpers.GetTestAppsPath(), "ComponentApp");
        ProjectFilePath = Path.Combine(projectRoot, "ComponentApp.csproj");
        PagesDirectory = Path.Combine(projectRoot, "Components", "Pages");
        var filePath = Path.Combine(PagesDirectory, "FormattingTest.razor");
        TargetPath = "/Components/Pages/FormattingTest.razor";

        var documentUri = new Uri(filePath);
        var documentSnapshot = await GetDocumentSnapshotAsync(ProjectFilePath, filePath, TargetPath);
        DocumentContext = new DocumentContext(documentUri, documentSnapshot, projectContext: null);

        var text = await DocumentSnapshot.GetTextAsync(CancellationToken.None).ConfigureAwait(false);
        Range = VsLspFactory.CreateRange(
            start: (0, 0),
            end: (text.Lines.Count - 1, 0));
    }

    private const int WindowSize = 10;

    [Benchmark(Description = "Razor Semantic Tokens Range Scrolling")]
    public async Task RazorSemanticTokensRangeScrollingAsync()
    {
        var cancellationToken = CancellationToken.None;

        var documentLineCount = Range.End.Line;

        var lineCount = 0;
        while (lineCount != documentLineCount)
        {
            var newLineCount = Math.Min(lineCount + WindowSize, documentLineCount);
            var span = new LinePositionSpan(new LinePosition(lineCount, 0), new LinePosition(newLineCount, 0));
            await RazorSemanticTokenService!.GetSemanticTokensAsync(
                DocumentContext,
                span,
                colorBackground: false,
                Guid.Empty,
                cancellationToken);

            lineCount = newLineCount;
        }
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
}
