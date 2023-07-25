// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.Completion;
using Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.LanguageServer;

public class RazorCompletionBenchmark : RazorLanguageServerBenchmarkBase
{
    private string? _filePath;
    private Uri? DocumentUri { get; set; }
    private RazorCompletionEndpoint? CompletionEndpoint { get; set; }
    private IDocumentSnapshot? DocumentSnapshot { get; set; }
    private SourceText? DocumentText { get; set; }
    private Position? RazorPosition { get; set; }
    private RazorRequestContext RazorRequestContext { get; set; }

    [GlobalSetup]
    public async Task SetupAsync()
    {
        var languageServer = RazorLanguageServer.GetInnerLanguageServerForTesting();

        var razorCompletionListProvider = languageServer.GetRequiredService<RazorCompletionListProvider>();
        var lspServices = languageServer.GetLspServices();
        var responseRewriters = lspServices.GetRequiredServices<DelegatedCompletionResponseRewriter>();
        var documentMappingService = lspServices.GetRequiredService<IRazorDocumentMappingService>();
        var clientNotifierServiceBase = lspServices.GetRequiredService<ClientNotifierServiceBase>();
        var completionListCache = lspServices.GetRequiredService<CompletionListCache>();

        var delegatedCompletionListProvider = new TestDelegatedCompletionListProvider(responseRewriters, documentMappingService, clientNotifierServiceBase, completionListCache);
        var completionListProvider = new CompletionListProvider(razorCompletionListProvider, delegatedCompletionListProvider);
        CompletionEndpoint = new RazorCompletionEndpoint(completionListProvider, telemetryReporter: null);

        var clientCapabilities = new VSInternalClientCapabilities
        {
            TextDocument = new TextDocumentClientCapabilities
            {
                Completion = new VSInternalCompletionSetting
                {
                },
            },
        };
        CompletionEndpoint.ApplyCapabilities(new(), clientCapabilities);
        var projectRoot = Path.Combine(RepoRoot, "src", "Razor", "test", "testapps", "ComponentApp");
        var projectFilePath = Path.Combine(projectRoot, "ComponentApp.csproj");
        _filePath = Path.Combine(projectRoot, "Components", "Pages", $"Generated.razor");

        var content = GetFileContents();

        var razorCodeActionIndex = content.IndexOf("|R|");
        content = content.Replace("|R|", "");

        File.WriteAllText(_filePath, content);

        var targetPath = "/Components/Pages/Generated.razor";

        DocumentUri = new Uri(_filePath);
        DocumentSnapshot = GetDocumentSnapshot(projectFilePath, _filePath, targetPath);
        DocumentText = await DocumentSnapshot.GetTextAsync();

        RazorPosition = ToPosition(razorCodeActionIndex);

        var documentContext = new VersionedDocumentContext(DocumentUri, DocumentSnapshot, projectContext: null, 1);
        RazorRequestContext = new RazorRequestContext(documentContext, Logger, languageServer.GetLspServices());

        Position ToPosition(int index)
        {
            DocumentText.GetLineAndOffset(index, out var line, out var offset);
            return new Position(line, offset);
        }
    }

    private static string GetFileContents()
    {
        var sb = new StringBuilder();

        sb.Append("""
            @using System;
            @using Endpoints.Pages;
            """);

        for (var i = 0; i < 100; i++)
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
            <div>
                <Ind|R|
                <span>@DateTime.Now</span>
                @if (true)
                {
                    <span>@y</span>
                }
            </div>
            @code {
                private void Goo()
                {
                }
            }"
            """);

        return sb.ToString();
    }

    [Benchmark(Description = "Razor Completion")]
    public async Task RazorCompletionAsync()
    {
        var completionParams = new CompletionParams
        {
            Position = RazorPosition!,
            Context = new VSInternalCompletionContext { },
            TextDocument = new TextDocumentIdentifier
            {
                Uri = DocumentUri!,
            },
        };

        var _ = await CompletionEndpoint!.HandleRequestAsync(completionParams, RazorRequestContext, CancellationToken.None);
    }

    private class TestDelegatedCompletionListProvider : DelegatedCompletionListProvider
    {
        public TestDelegatedCompletionListProvider(IEnumerable<DelegatedCompletionResponseRewriter> responseRewriters, IRazorDocumentMappingService documentMappingService, ClientNotifierServiceBase languageServer, CompletionListCache completionListCache)
            : base(responseRewriters, documentMappingService, languageServer, completionListCache)
        {
        }

        public override Task<VSInternalCompletionList?> GetCompletionListAsync(int absoluteIndex, VSInternalCompletionContext completionContext, VersionedDocumentContext documentContext, VSInternalClientCapabilities clientCapabilities, Guid correlationId, CancellationToken cancellationToken)
        {
            return Task.FromResult<VSInternalCompletionList?>(
                new VSInternalCompletionList
                {
                });
        }
    }
}
