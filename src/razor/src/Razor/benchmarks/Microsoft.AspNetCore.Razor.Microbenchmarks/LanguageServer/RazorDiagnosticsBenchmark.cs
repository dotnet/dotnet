// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.Diagnostics;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Diagnostics;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol.Diagnostics;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Range = Microsoft.VisualStudio.LanguageServer.Protocol.Range;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.LanguageServer;

[ShortRunJob]
public class RazorDiagnosticsBenchmark : RazorLanguageServerBenchmarkBase
{
    private VSDocumentDiagnosticsEndpoint? DocumentPullDiagnosticsEndpoint { get; set; }
    private RazorRequestContext RazorRequestContext { get; set; }
    private RazorCodeDocument? RazorCodeDocument { get; set; }
    private SourceText? SourceText { get; set; }
    private ImmutableArray<SourceMapping> SourceMappings { get; set; }
    private SourceText? GeneratedCode { get; set; }
    private object? Diagnostics { get; set; }
    private DocumentContext? DocumentContext { get; set; }
    private VSInternalDocumentDiagnosticsParams? Request { get; set; }
    private IEnumerable<VSInternalDiagnosticReport?>? Response { get; set; }

    [Params(0, 1, 1000)]
    public int N { get; set; }

    [IterationSetup]
    public void Setup()
    {
        SourceMappings = GetSourceMappings();
        GeneratedCode = GetGeneratedCode();
        Diagnostics = BuildDiagnostics();
        var razorFilePath = "file://C:/path/test.razor";
        var uri = new Uri(razorFilePath);
        Request = new VSInternalDocumentDiagnosticsParams
        {
            TextDocument = new TextDocumentIdentifier { Uri = uri }
        };
        var stringSourceDocument = RazorSourceDocument.Create(GetFileContents(), UTF8Encoding.UTF8, RazorSourceDocumentProperties.Default);
        var mockRazorCodeDocument = new Mock<RazorCodeDocument>(MockBehavior.Strict);

        var mockRazorCSharpDocument = new RazorCSharpDocument(
            mockRazorCodeDocument.Object,
            GeneratedCode,
            RazorCodeGenerationOptions.DesignTimeDefault,
            diagnostics: [],
            SourceMappings,
            linePragmas: []);

        var itemCollection = new ItemCollection();
        itemCollection[typeof(RazorCSharpDocument)] = mockRazorCSharpDocument;
        mockRazorCodeDocument.Setup(r => r.Source).Returns(stringSourceDocument);
        mockRazorCodeDocument.Setup(r => r.Items).Returns(itemCollection);
        RazorCodeDocument = mockRazorCodeDocument.Object;

        SourceText = RazorCodeDocument.Source.Text;
        var documentContext = new Mock<DocumentContext>(
            MockBehavior.Strict,
            new object[] { It.IsAny<Uri>(), It.IsAny<IDocumentSnapshot>(), It.IsAny<VSProjectContext>(), It.IsAny<int>() });
        documentContext
            .Setup(r => r.GetCodeDocumentAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(RazorCodeDocument);
        documentContext.Setup(r => r.Uri).Returns(It.IsAny<Uri>());
        documentContext.Setup(r => r.Snapshot.Version).Returns(It.IsAny<int>());
        documentContext.Setup(r => r.GetSourceTextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(It.IsAny<SourceText>());
        RazorRequestContext = new RazorRequestContext(documentContext.Object, null!, "lsp/method", uri: null);
        DocumentContext = documentContext.Object;

        var loggerFactory = EmptyLoggerFactory.Instance;
        var languageServerFeatureOptions = BuildFeatureOptions();
        var languageServer = new ClientNotifierService(Diagnostics!);
        var documentMappingService = BuildRazorDocumentMappingService();

        var optionsMonitor = Mock.Of<RazorLSPOptionsMonitor>(MockBehavior.Strict);
        var translateDiagnosticsService = new RazorTranslateDiagnosticsService(documentMappingService, loggerFactory);
        DocumentPullDiagnosticsEndpoint = new VSDocumentDiagnosticsEndpoint(languageServerFeatureOptions, translateDiagnosticsService, optionsMonitor, languageServer, telemetryReporter: null);
    }

    private object BuildDiagnostics()
        => new RazorPullDiagnosticResponse(
            new[]
            {
                new VSInternalDiagnosticReport()
                {
                    Diagnostics = GetDiagnostics(N)
                }
            }, Array.Empty<VSInternalDiagnosticReport>());

    private protected override LanguageServerFeatureOptions BuildFeatureOptions()
    {
        return Mock.Of<LanguageServerFeatureOptions>(options =>
            options.SupportsFileManipulation == true &&
            options.SingleServerSupport == true &&
            options.CSharpVirtualDocumentSuffix == ".ide.g.cs" &&
            options.HtmlVirtualDocumentSuffix == "__virtual.html",
            MockBehavior.Strict);
    }

    private IDocumentMappingService BuildRazorDocumentMappingService()
    {
        var razorDocumentMappingService = new Mock<IDocumentMappingService>(MockBehavior.Strict);

        Range? hostDocumentRange;
        razorDocumentMappingService.Setup(
            r => r.TryMapToHostDocumentRange(
                It.IsAny<IRazorGeneratedDocument>(),
                InRange,
                It.IsAny<MappingBehavior>(),
                out hostDocumentRange))
            .Returns((IRazorGeneratedDocument generatedDocument, Range range, MappingBehavior mappingBehavior, out Range? actualOutRange) =>
            {
                actualOutRange = OutRange;
                return true;
            });

        Range? hostDocumentRange2;
        razorDocumentMappingService.Setup(
            r => r.TryMapToHostDocumentRange(
                It.IsAny<IRazorGeneratedDocument>(),
                It.IsNotIn(InRange),
                It.IsAny<MappingBehavior>(),
                out hostDocumentRange2))
            .Returns((IRazorGeneratedDocument generatedDocument, Range range, MappingBehavior mappingBehavior, out Range? actualOutRange) =>
            {
                actualOutRange = null;
                return false;
            });

        return razorDocumentMappingService.Object;
    }

    private string GetFileContents()
        => """
<div></div>

@functions
{
    public void M()
    {
        CallOnMe();
    }
}

""";

    [Benchmark(Description = "Diagnostics")]
    public async Task RazorDiagnosticsAsync()
    {
        Response = await DocumentPullDiagnosticsEndpoint!.HandleRequestAsync(Request!, RazorRequestContext, CancellationToken.None);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        if (N > 0)
        {
            var diagnostics = Response!.SelectMany(r => r!.Diagnostics!);
            if (!diagnostics.Any(d => d.Message.Contains("CallOnMe")) ||
                !diagnostics.Any(y => y.Range == OutRange))
            {
                throw new NotImplementedException("benchmark setup is wrong");
            }
        }
    }

    private class ClientNotifierService : IClientConnection
    {
        private readonly object _diagnostics;

        public ClientNotifierService(object diagnostics)
        {
            _diagnostics = diagnostics;
        }

        public Task SendNotificationAsync<TParams>(string method, TParams @params, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SendNotificationAsync(string method, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> SendRequestAsync<TParams, TResponse>(string method, TParams @params, CancellationToken cancellationToken)
        {
            return Task.FromResult((TResponse)_diagnostics);
        }
    }

    private Range InRange { get; set; } = VsLspFactory.CreateSingleLineRange(line: 85, character: 8, length: 8);
    private Range OutRange { get; set; } = VsLspFactory.CreateSingleLineRange(line: 6, character: 8, length: 8);

    private Diagnostic[] GetDiagnostics(int n) => Enumerable.Range(1, n).Select(_ => new Diagnostic()
    {
        Range = InRange,
        Code = "CS0103",
        Severity = DiagnosticSeverity.Error,
        Message = "The name 'CallOnMe' does not exist in the current context"
    }).ToArray();

    private static SourceText GetGeneratedCode()
        => SourceText.From("""
// <auto-generated/>
#pragma warning disable 1591
namespace AspNetCore
{
    #line hidden
    using TModel = global::System.Object;
#nullable restore
#line 1 "_ViewImports.cshtml"
using BlazorApp1;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "_ViewImports.cshtml"
using BlazorApp1.Pages;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "_ViewImports.cshtml"
using BlazorApp1.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "_ViewImports.cshtml"
using System;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "_ViewImports.cshtml"
using Microsoft.AspNetCore.Components;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "_ViewImports.cshtml"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "_ViewImports.cshtml"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "_ViewImports.cshtml"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemMetadataAttribute("Identifier", "/test.cshtml")]
    [global::System.Runtime.CompilerServices.CreateNewOnMetadataUpdateAttribute]
    #nullable restore
    public class test : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        #pragma warning disable 219
        private void __RazorDirectiveTokenHelpers__() {
        }
        #pragma warning restore 219
        #pragma warning disable 0414
        private static object __o = null;
        #pragma warning restore 0414
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 4 "test.cshtml"

    public void M()
    {
        CallOnMe();
    }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591

""", Encoding.UTF8);

    private ImmutableArray<SourceMapping> GetSourceMappings()
        => ImmutableArray<SourceMapping>.Empty.Add(
            new SourceMapping(
                originalSpan: new SourceSpan(filePath: "test.cshtml", absoluteIndex: 28, lineIndex: 3, characterIndex: 1, length: 58, lineCount: 5, endCharacterIndex: 0),
                generatedSpan: new SourceSpan(filePath: null, absoluteIndex: 2026, lineIndex: 82, characterIndex: 1, length: 58, lineCount: 1, endCharacterIndex: 0)
        ));
}
