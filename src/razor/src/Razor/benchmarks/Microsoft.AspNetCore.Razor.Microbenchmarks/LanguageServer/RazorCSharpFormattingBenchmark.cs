// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.LanguageServer;

public enum InputType
{
    Preformatted,
    Unformatted
}

[CsvExporter]
[RPlotExporter]
public class RazorCSharpFormattingBenchmark : RazorLanguageServerBenchmarkBase
{
    private string _filePath;

    private IRazorFormattingService RazorFormattingService { get; set; }

    private Uri DocumentUri { get; set; }

    private IDocumentSnapshot DocumentSnapshot { get; set; }

    private SourceText DocumentText { get; set; }

    /// <summary>
    /// How many blocks of 25 lines of code should be formatted
    /// </summary>
    [Params(1, 2, 3, 4, 5, 10, 20, 30, 40, 100)]
    public int Blocks { get; set; }

    [ParamsAllValues]
    public InputType InputType { get; set; }

    [GlobalSetup(Target = nameof(RazorCSharpFormattingAsync))]
    public async Task InitializeRazorCSharpFormattingAsync()
    {
        EnsureServicesInitialized();

        var projectRoot = Path.Combine(RepoRoot, "src", "Razor", "test", "testapps", "ComponentApp");
        var projectFilePath = Path.Combine(projectRoot, "ComponentApp.csproj");
        _filePath = Path.Combine(projectRoot, "Components", "Pages", $"Generated.razor");

        WriteSampleFormattingFile(_filePath, InputType == InputType.Preformatted, Blocks);

        var targetPath = "/Components/Pages/Generated.razor";

        DocumentUri = new Uri(_filePath);
        DocumentSnapshot = GetDocumentSnapshot(projectFilePath, _filePath, targetPath);
        DocumentText = await DocumentSnapshot.GetTextAsync();
    }

    private static void WriteSampleFormattingFile(string filePath, bool preformatted, int blocks)
    {
        var data = @"
@{
    y = 456;
}

<div>
    <span>@DateTime.Now</span>
    @if (true)
    {
        var x = 123;
        <span>@x</span>
    }
</div>

@code {
    public string Prop$INDEX$ { get; set; }

    public string[] SomeList$INDEX$ { get; set; }

    public class Foo$INDEX$
    {
        @* This is a Razor Comment *@
        void Method() { }
    }
}

";
        using var fileStream = File.CreateText(filePath);

        if (!preformatted)
        {
            data = data.Replace("    ", "")
                .Replace("@code {", "@code{")
                .Replace("@if (true", "@if(true");
        }

        for (var i = 0; i < blocks; i++)
        {
            fileStream.WriteLine(data.Replace("$INDEX$", i.ToString()));
        }
    }

    [Benchmark(Description = "Formatting")]
    public async Task RazorCSharpFormattingAsync()
    {
        var options = new FormattingOptions()
        {
            TabSize = 4,
            InsertSpaces = true
        };

        var documentContext = new VersionedDocumentContext(DocumentUri, DocumentSnapshot, projectContext: null, version: 1);

        var edits = await RazorFormattingService.FormatAsync(documentContext, range: null, options, CancellationToken.None);

#if DEBUG
        // For debugging purposes only.
        var changedText = DocumentText.WithChanges(edits.Select(e => e.ToTextChange(DocumentText)));
        _ = changedText.ToString();
#endif
    }

    [GlobalCleanup]
    public async Task CleanupServerAsync()
    {
        File.Delete(_filePath);

        var innerServer = RazorLanguageServer.GetInnerLanguageServerForTesting();

        await innerServer.ShutdownAsync();
        await innerServer.ExitAsync();
    }

    private void EnsureServicesInitialized()
    {
        var languageServer = RazorLanguageServer.GetInnerLanguageServerForTesting();
        RazorFormattingService = languageServer.GetRequiredService<IRazorFormattingService>();
    }
}
