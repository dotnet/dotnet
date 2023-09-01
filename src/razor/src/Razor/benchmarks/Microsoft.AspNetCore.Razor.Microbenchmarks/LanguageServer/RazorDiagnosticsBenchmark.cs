// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.Diagnostics;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.LanguageServer;

[ShortRunJob]
public class RazorDiagnosticsBenchmark : RazorLanguageServerBenchmarkBase
{
    private DocumentPullDiagnosticsEndpoint? DocumentPullDiagnosticsEndpoint { get; set; }
    private RazorRequestContext RazorRequestContext { get; set; }
    private RazorCodeDocument? RazorCodeDocument { get; set; }
    private SourceText? SourceText { get; set; }
    private SourceMapping[]? SourceMappings { get; set; }
    private string? GeneratedCode { get; set; }
    private object? Diagnostics { get; set; }
    private SourceText? CSharpSourceText { get; set; }
    private VersionedDocumentContext? VersionedDocumentContext { get; set; }
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
        var stringSourceDocument = new StringSourceDocument(GetFileContents(), UTF8Encoding.UTF8, new RazorSourceDocumentProperties());
        var mockRazorCodeDocument = new Mock<RazorCodeDocument>(MockBehavior.Strict);

        var mockRazorCSharpDocument = RazorCSharpDocument.Create(
            mockRazorCodeDocument.Object,
            GeneratedCode,
            RazorCodeGenerationOptions.CreateDesignTimeDefault(),
            Array.Empty<RazorDiagnostic>(),
            SourceMappings,
            new List<LinePragma>()
        );

        var itemCollection = new ItemCollection();
        itemCollection[typeof(RazorCSharpDocument)] = mockRazorCSharpDocument;
        mockRazorCodeDocument.Setup(r => r.Source).Returns(stringSourceDocument);
        mockRazorCodeDocument.Setup(r => r.Items).Returns(itemCollection);
        RazorCodeDocument = mockRazorCodeDocument.Object;

        SourceText = RazorCodeDocument.GetSourceText();
        CSharpSourceText = RazorCodeDocument.GetCSharpSourceText();
        var documentContext = new Mock<VersionedDocumentContext>(
            MockBehavior.Strict,
            new object[] { It.IsAny<Uri>(), It.IsAny<IDocumentSnapshot>(), It.IsAny<VSProjectContext>(), It.IsAny<int>() });
        documentContext
            .Setup(r => r.GetCodeDocumentAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(RazorCodeDocument);
        documentContext.Setup(r => r.Uri).Returns(It.IsAny<Uri>());
        documentContext.Setup(r => r.Version).Returns(It.IsAny<int>());
        documentContext.Setup(r => r.GetSourceTextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(It.IsAny<SourceText>());
        RazorRequestContext = new RazorRequestContext(documentContext.Object, Logger, null!);
        VersionedDocumentContext = documentContext.Object;

        var loggerFactory = BuildLoggerFactory();
        var languageServerFeatureOptions = BuildFeatureOptions();
        var languageServer = new ClientNotifierService(Diagnostics!);
        var documentMappingService = BuildRazorDocumentMappingService();

        var translateDiagnosticsService = new RazorTranslateDiagnosticsService(documentMappingService, loggerFactory);
        DocumentPullDiagnosticsEndpoint = new DocumentPullDiagnosticsEndpoint(languageServerFeatureOptions, translateDiagnosticsService, languageServer, telemetryReporter: null);
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
            options.SingleServerCompletionSupport == true &&
            options.CSharpVirtualDocumentSuffix == ".ide.g.cs" &&
            options.HtmlVirtualDocumentSuffix == "__virtual.html",
            MockBehavior.Strict);
    }

    private IRazorDocumentMappingService BuildRazorDocumentMappingService()
    {
        var razorDocumentMappingService = new Mock<IRazorDocumentMappingService>(MockBehavior.Strict);

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

    private ILoggerFactory BuildLoggerFactory() => Mock.Of<ILoggerFactory>(
        r => r.CreateLogger(
            It.IsAny<string>()) == new NoopLogger(),
        MockBehavior.Strict);

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

    private class ClientNotifierService : ClientNotifierServiceBase
    {
        private readonly object _diagnostics;

        public ClientNotifierService(object diagnostics)
        {
            _diagnostics = diagnostics;
        }

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
            return Task.FromResult((TResponse)_diagnostics);
        }
    }

    private Range InRange { get; set; } = new Range { Start = new Position(85, 8), End = new Position(85, 16) };
    private Range OutRange { get; set; } = new Range { Start = new Position(6, 8), End = new Position(6, 16) };

    private Diagnostic[] GetDiagnostics(int N) => Enumerable.Range(1, N).Select(_ => new Diagnostic()
    {
        Range = InRange,
        Code = "CS0103",
        Severity = DiagnosticSeverity.Error,
        Message = "The name 'CallOnMe' does not exist in the current context"
    }).ToArray();

    private string GetGeneratedCode()
        => """
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

""";

    private SourceMapping[] GetSourceMappings()
        => new SourceMapping[] {
            new SourceMapping(
                originalSpan: new SourceSpan(filePath: "test.cshtml", absoluteIndex: 28, lineIndex: 3, characterIndex: 1, length: 58, lineCount: 5, endCharacterIndex: 0),
                generatedSpan: new SourceSpan(filePath: null, absoluteIndex: 2026, lineIndex: 82, characterIndex: 1, length: 58, lineCount: 1, endCharacterIndex: 0)
    )};
}
