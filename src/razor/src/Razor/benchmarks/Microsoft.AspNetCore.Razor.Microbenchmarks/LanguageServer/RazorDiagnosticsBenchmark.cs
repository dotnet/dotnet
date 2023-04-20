// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.Diagnostics;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.LanguageServer;

public class RazorDiagnosticsBenchmark : RazorLanguageServerBenchmarkBase
{
    private string? _filePath;
    private Uri? DocumentUri { get; set; }
    private DocumentPullDiagnosticsEndpoint? DocumentPullDiagnosticsEndpoint { get; set; }
    private IDocumentSnapshot? DocumentSnapshot { get; set; }
    private SourceText? DocumentText { get; set; }
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
        var languageServer = RazorLanguageServer.GetInnerLanguageServerForTesting();

        DocumentPullDiagnosticsEndpoint = new DocumentPullDiagnosticsEndpoint(
            languageServerFeatureOptions: languageServer.GetRequiredService<LanguageServerFeatureOptions>(),
            translateDiagnosticsService: languageServer.GetRequiredService<RazorTranslateDiagnosticsService>(),
            languageServer: new ClientNotifierService());
        var projectRoot = Path.Combine(RepoRoot, "src", "Razor", "test", "testapps", "ComponentApp");
        var projectFilePath = Path.Combine(projectRoot, "ComponentApp.csproj");
        _filePath = Path.Combine(projectRoot, "Components", "Pages", $"Generated.razor");

        var content = GetFileContents(this.FileType);
        File.WriteAllText(_filePath, content);

        var targetPath = "/Components/Pages/Generated.razor";

        DocumentUri = new Uri(_filePath);
        DocumentSnapshot = GetDocumentSnapshot(projectFilePath, _filePath, targetPath);
        DocumentText = await DocumentSnapshot.GetTextAsync();
        var documentContext = new VersionedDocumentContext(DocumentUri, DocumentSnapshot, 1);

        RazorRequestContext = new RazorRequestContext(documentContext, Logger, languageServer.GetLspServices());

        // Run once in setup to verify setup is correct
        var request = new VSInternalDocumentDiagnosticsParams
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = DocumentUri!
            },
        };

        var diagnostics = await DocumentPullDiagnosticsEndpoint!.HandleRequestAsync(request, RazorRequestContext, CancellationToken.None);

        if (!diagnostics!.ElementAtOrDefault(0)!.Diagnostics!.ElementAtOrDefault(0)!.Message.Contains("CallOnMe"))
        {
            throw new NotImplementedException("benchmark setup is wrong");
        }
    }


    private protected override LanguageServerFeatureOptions BuildFeatureOptions()
    {
        return new DiagnosticsLanguageServerFeatureOptions();
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

             <div></div>

             @functions
             {
                public void M()
                {
                    CallOnMe();
                }
             }

             """);

        return sb.ToString();
    }

    [Benchmark(Description = "Diagnostics")]
    public async Task RazorDiagnosticsAsync()
    {
        var request = new VSInternalDocumentDiagnosticsParams
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = DocumentUri!
            },
        };

        await DocumentPullDiagnosticsEndpoint!.HandleRequestAsync(request, RazorRequestContext, CancellationToken.None);
    }

    [GlobalCleanup]
    public async Task CleanupAsync()
    {
        File.Delete(_filePath!);

        var innerServer = RazorLanguageServer.GetInnerLanguageServerForTesting();

        await innerServer.ShutdownAsync();
        await innerServer.ExitAsync();
    }

    private class DiagnosticsLanguageServerFeatureOptions : LanguageServerFeatureOptions
    {
        public override bool SupportsFileManipulation => true;

        public override string ProjectConfigurationFileName => "project.razor.json";

        public override string CSharpVirtualDocumentSuffix => ".ide.g.cs";

        public override string HtmlVirtualDocumentSuffix => "__virtual.html";

        public override bool SingleServerCompletionSupport => false;

        public override bool SingleServerSupport => true;

        public override bool SupportsDelegatedCodeActions => true;

        // Code action and rename paths in Windows VS Code need to be prefixed with '/':
        // https://github.com/dotnet/razor/issues/8131
        public override bool ReturnCodeActionAndRenamePathsWithPrefixedSlash
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public override bool ShowAllCSharpCodeActions => false;
    }

    private class ClientNotifierService : ClientNotifierServiceBase
    {
        public override Task OnInitializedAsync(VSInternalClientCapabilities clientCapabilities, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public override Task SendNotificationAsync<TParams>(string method, TParams @params, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task SendNotificationAsync(string method, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<TResponse> SendRequestAsync<TParams, TResponse>(string method, TParams @params, CancellationToken cancellationToken)
        {
            object result = new RazorPullDiagnosticResponse(
                new[]
                {
                    new VSInternalDiagnosticReport()
                    {
                        ResultId = "5",
                        Diagnostics = new Diagnostic[]
                        {
                            new()
                            {
                                Range = new Range()
                                {
                                    Start = new Position(10, 19),
                                    End = new Position(10, 23)
                                },
                                Code = "CS0103",
                                Severity = DiagnosticSeverity.Error,
                                Source = "DocumentPullDiagnosticHandler",
                                Message = "The name 'CallOnMe' does not exist in the current context"
                            }
                        }
                    }
                }, Array.Empty<VSInternalDiagnosticReport>());
            return Task.FromResult((TResponse)result);
        }
    }
}
