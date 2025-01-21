// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.CodeActions;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Range = Microsoft.VisualStudio.LanguageServer.Protocol.Range;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.LanguageServer;

public class RazorCodeActionsBenchmark : RazorLanguageServerBenchmarkBase
{
    private string? _filePath;
    private Uri? DocumentUri { get; set; }
    private CodeActionEndpoint? CodeActionEndpoint { get; set; }
    private IDocumentSnapshot? DocumentSnapshot { get; set; }
    private SourceText? DocumentText { get; set; }
    private Range? RazorCodeActionRange { get; set; }
    private Range? CSharpCodeActionRange { get; set; }
    private Range? HtmlCodeActionRange { get; set; }
    private RazorRequestContext RazorRequestContext { get; set; }

    public enum FileTypes
    {
        Small,
        Large
    }

    [ParamsAllValues]
    public FileTypes FileType { get; set; }

    [GlobalSetup]
    public async Task SetupAsync()
    {
        CodeActionEndpoint = new CodeActionEndpoint(
            codeActionsService: RazorLanguageServerHost.GetRequiredService<ICodeActionsService>(),
            delegatedCodeActionProvider: RazorLanguageServerHost.GetRequiredService<IDelegatedCodeActionsProvider>(),
            telemetryReporter: NoOpTelemetryReporter.Instance);

        var projectRoot = Path.Combine(Helpers.GetTestAppsPath(), "ComponentApp");
        var projectFilePath = Path.Combine(projectRoot, "ComponentApp.csproj");
        _filePath = Path.Combine(projectRoot, "Components", "Pages", "Generated.razor");

        var content = GetFileContents(this.FileType);

        var htmlCodeActionIndex = content.IndexOf("|H|");
        content = content.Replace("|H|", "");
        var razorCodeActionIndex = content.IndexOf("|R|");
        content = content.Replace("|R|", "");
        var csharpCodeActionIndex = content.IndexOf("|C|");
        content = content.Replace("|C|", "");

        File.WriteAllText(_filePath, content);

        var targetPath = "/Components/Pages/Generated.razor";

        DocumentUri = new Uri(_filePath);
        DocumentSnapshot = await GetDocumentSnapshotAsync(projectFilePath, _filePath, targetPath);
        DocumentText = await DocumentSnapshot.GetTextAsync(CancellationToken.None);

        RazorCodeActionRange = DocumentText.GetZeroWidthRange(razorCodeActionIndex);
        CSharpCodeActionRange = DocumentText.GetZeroWidthRange(csharpCodeActionIndex);
        HtmlCodeActionRange = DocumentText.GetZeroWidthRange(htmlCodeActionIndex);

        var documentContext = new DocumentContext(DocumentUri, DocumentSnapshot, projectContext: null);

        var codeDocument = await documentContext.GetCodeDocumentAsync(CancellationToken.None);
        // Need a root namespace for the Extract to Code Behind light bulb to be happy
        codeDocument.SetCodeGenerationOptions(RazorCodeGenerationOptions.Create(c => c.RootNamespace = "Root.Namespace"));

        RazorRequestContext = new RazorRequestContext(documentContext, RazorLanguageServerHost.GetRequiredService<ILspServices>(), "lsp/method", uri: null);
    }

    private string GetFileContents(FileTypes fileType)
    {
        var sb = new StringBuilder();

        sb.Append("""
            @using System;
            """);

        for (var i = 0; i < (fileType == FileTypes.Small ? 1 : 100); i++)
        {
            sb.Append($$"""
            @{
                var y{{i}} = 456;
            }

            <div>
                <p>Hello there Mr {{i}}</p>
            </div>
            """);
        }

        sb.Append("""
            <|H|div>
                <span>@DateTime.Now</span>
                @if (true)
                {
                    <span>@y</span>
                }
            </div>

            @co|R|de {
                private void |C|Goo()
                {
                }
            }"
            """);

        return sb.ToString();
    }

    [Benchmark(Description = "Lightbulbs")]
    public async Task RazorLightbulbAsync()
    {
        var request = new VSCodeActionParams
        {
            Range = RazorCodeActionRange!,
            Context = new VSInternalCodeActionContext(),
            TextDocument = new VSTextDocumentIdentifier
            {
                Uri = DocumentUri!
            },
        };

        await CodeActionEndpoint!.HandleRequestAsync(request, RazorRequestContext, CancellationToken.None);
    }

    [GlobalCleanup]
    public async Task CleanupAsync()
    {
        File.Delete(_filePath!);

        var server = RazorLanguageServerHost.GetTestAccessor().Server;

        await server.ShutdownAsync();
        await server.ExitAsync();
    }
}
