// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.LanguageServer.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Mef;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;
using DefinitionResult = Microsoft.VisualStudio.LanguageServer.Protocol.SumType<
    Microsoft.VisualStudio.LanguageServer.Protocol.VSInternalLocation,
    Microsoft.VisualStudio.LanguageServer.Protocol.VSInternalLocation[],
    Microsoft.VisualStudio.LanguageServer.Protocol.DocumentLink[]>;
using ImplementationResult = Microsoft.VisualStudio.LanguageServer.Protocol.SumType<
    Microsoft.VisualStudio.LanguageServer.Protocol.Location[],
    Microsoft.VisualStudio.LanguageServer.Protocol.VSInternalReferenceItem[]>;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

[UseExportProvider]
public abstract class SingleServerDelegatingEndpointTestBase : LanguageServerTestBase
{
    internal DocumentContextFactory DocumentContextFactory { get; private set; }
    internal LanguageServerFeatureOptions LanguageServerFeatureOptions { get; private set; }
    internal TestLanguageServer LanguageServer { get; private set; }
    internal RazorDocumentMappingService DocumentMappingService { get; private set; }

    protected SingleServerDelegatingEndpointTestBase(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    protected async Task CreateLanguageServerAsync(RazorCodeDocument codeDocument, string razorFilePath)
    {
        var realLanguageServerFeatureOptions = new DefaultLanguageServerFeatureOptions();

        var csharpSourceText = codeDocument.GetCSharpSourceText();
        var csharpDocumentUri = new Uri(realLanguageServerFeatureOptions.GetRazorCSharpFilePath(razorFilePath));
        var csharpServer = await CSharpTestLspServerHelpers.CreateCSharpLspServerAsync(
            csharpSourceText,
            csharpDocumentUri,
            new VSInternalServerCapabilities
            {
                SupportsDiagnosticRequests = true,
            },
            razorSpanMappingService: null,
            DisposalToken);

        AddDisposable(csharpServer);

        await csharpServer.OpenDocumentAsync(csharpDocumentUri, csharpSourceText.ToString()).ConfigureAwait(false);

        DocumentContextFactory = new TestDocumentContextFactory(razorFilePath, codeDocument, version: 1337);
        LanguageServerFeatureOptions = Mock.Of<LanguageServerFeatureOptions>(options =>
            options.SupportsFileManipulation == true &&
            options.SingleServerSupport == true &&
            options.CSharpVirtualDocumentSuffix == realLanguageServerFeatureOptions.CSharpVirtualDocumentSuffix &&
            options.HtmlVirtualDocumentSuffix == realLanguageServerFeatureOptions.HtmlVirtualDocumentSuffix,
            MockBehavior.Strict);
        LanguageServer = new TestLanguageServer(csharpServer, csharpDocumentUri, DisposalToken);
        DocumentMappingService = new DefaultRazorDocumentMappingService(LanguageServerFeatureOptions, DocumentContextFactory, LoggerFactory);
    }

    internal class TestLanguageServer : ClientNotifierServiceBase
    {
        private readonly CSharpTestLspServer _csharpServer;
        private readonly Uri _csharpDocumentUri;
        private readonly CancellationToken _cancellationToken;

        public int RequestCount;

        public TestLanguageServer(
            CSharpTestLspServer csharpServer,
            Uri csharpDocumentUri,
            CancellationToken cancellationToken)
        {
            _csharpServer = csharpServer;
            _csharpDocumentUri = csharpDocumentUri;
            _cancellationToken = cancellationToken;
        }

        public override Task OnInitializedAsync(VSInternalClientCapabilities clientCapabilities, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async override Task<TResponse> SendRequestAsync<TParams, TResponse>(string method, TParams @params, CancellationToken cancellationToken)
        {
            RequestCount++;
            object result = method switch
            {
                RazorLanguageServerCustomMessageTargets.RazorDefinitionEndpointName => await HandleDefinitionAsync(@params),
                RazorLanguageServerCustomMessageTargets.RazorImplementationEndpointName => await HandleImplementationAsync(@params),
                RazorLanguageServerCustomMessageTargets.RazorSignatureHelpEndpointName => await HandleSignatureHelpAsync(@params),
                RazorLanguageServerCustomMessageTargets.RazorRenameEndpointName => await HandleRenameAsync(@params),
                RazorLanguageServerCustomMessageTargets.RazorOnAutoInsertEndpointName => await HandleOnAutoInsertAsync(@params),
                RazorLanguageServerCustomMessageTargets.RazorValidateBreakpointRangeName => await HandleValidateBreakpointRangeAsync(@params),
                RazorLanguageServerCustomMessageTargets.RazorReferencesEndpointName => await HandleReferencesAsync(@params),
                _ => throw new NotImplementedException($"I don't know how to handle the '{method}' method.")
            };

            return (TResponse)result;
        }

        private async Task<VSInternalReferenceItem[]> HandleReferencesAsync<TParams>(TParams @params)
        {
            var delegatedParams = Assert.IsType<DelegatedPositionParams>(@params);
            var delegatedRequest = new TextDocumentPositionParams()
            {
                TextDocument = new TextDocumentIdentifier()
                {
                    Uri = _csharpDocumentUri
                },
                Position = delegatedParams.ProjectedPosition
            };

            var result = await _csharpServer.ExecuteRequestAsync<TextDocumentPositionParams, VSInternalReferenceItem[]>(
                Methods.TextDocumentReferencesName,
                delegatedRequest,
                _cancellationToken);

            return result;
        }

        private async Task<DefinitionResult?> HandleDefinitionAsync<T>(T @params)
        {
            var delegatedParams = Assert.IsType<DelegatedPositionParams>(@params);
            var delegatedRequest = new TextDocumentPositionParams()
            {
                TextDocument = new TextDocumentIdentifier()
                {
                    Uri = _csharpDocumentUri
                },
                Position = delegatedParams.ProjectedPosition
            };

            var result = await _csharpServer.ExecuteRequestAsync<TextDocumentPositionParams, DefinitionResult?>(
                Methods.TextDocumentDefinitionName,
                delegatedRequest,
                _cancellationToken);

            return result;
        }

        private async Task<ImplementationResult> HandleImplementationAsync<T>(T @params)
        {
            var delegatedParams = Assert.IsType<DelegatedPositionParams>(@params);
            var delegatedRequest = new TextDocumentPositionParams()
            {
                TextDocument = new TextDocumentIdentifier()
                {
                    Uri = _csharpDocumentUri
                },
                Position = delegatedParams.ProjectedPosition
            };

            var result = await _csharpServer.ExecuteRequestAsync<TextDocumentPositionParams, ImplementationResult>(
                Methods.TextDocumentImplementationName,
                delegatedRequest,
                _cancellationToken);

            return result;
        }

        private async Task<VisualStudio.LanguageServer.Protocol.SignatureHelp> HandleSignatureHelpAsync<T>(T @params)
        {
            var delegatedParams = Assert.IsType<DelegatedPositionParams>(@params);
            var delegatedRequest = new SignatureHelpParams()
            {
                TextDocument = new TextDocumentIdentifier()
                {
                    Uri = _csharpDocumentUri
                },
                Position = delegatedParams.ProjectedPosition,
            };

            var result = await _csharpServer.ExecuteRequestAsync<SignatureHelpParams, VisualStudio.LanguageServer.Protocol.SignatureHelp>(
                Methods.TextDocumentSignatureHelpName,
                delegatedRequest,
                _cancellationToken);

            return result;
        }

        private async Task<WorkspaceEdit> HandleRenameAsync<T>(T @params)
        {
            var delegatedParams = Assert.IsType<DelegatedRenameParams>(@params);
            var delegatedRequest = new RenameParams()
            {
                TextDocument = new TextDocumentIdentifier()
                {
                    Uri = _csharpDocumentUri
                },
                Position = delegatedParams.ProjectedPosition,
                NewName = delegatedParams.NewName,
            };

            var result = await _csharpServer.ExecuteRequestAsync<RenameParams, WorkspaceEdit>(
                Methods.TextDocumentRenameName,
                delegatedRequest,
                _cancellationToken);

            return result;
        }

        private async Task<VSInternalDocumentOnAutoInsertResponseItem> HandleOnAutoInsertAsync<T>(T @params)
        {
            var delegatedParams = Assert.IsType<DelegatedOnAutoInsertParams>(@params);
            var delegatedRequest = new VSInternalDocumentOnAutoInsertParams()
            {
                TextDocument = new TextDocumentIdentifier()
                {
                    Uri = _csharpDocumentUri
                },
                Position = delegatedParams.ProjectedPosition,
                Character = delegatedParams.Character,
                Options = delegatedParams.Options
            };

            var result = await _csharpServer.ExecuteRequestAsync<VSInternalDocumentOnAutoInsertParams, VSInternalDocumentOnAutoInsertResponseItem>(
                VSInternalMethods.OnAutoInsertName,
                delegatedRequest,
                _cancellationToken);

            return result;
        }

        public override Task SendNotificationAsync<TParams>(string method, TParams @params, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task SendNotificationAsync(string method, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task<Range> HandleValidateBreakpointRangeAsync<T>(T @params)
        {
            var delegatedParams = Assert.IsType<DelegatedValidateBreakpointRangeParams>(@params);
            var delegatedRequest = new VSInternalValidateBreakableRangeParams()
            {
                TextDocument = new TextDocumentIdentifier()
                {
                    Uri = _csharpDocumentUri
                },
                Range = delegatedParams.ProjectedRange,
            };

            var result = await _csharpServer.ExecuteRequestAsync<VSInternalValidateBreakableRangeParams, Range>(
                VSInternalMethods.TextDocumentValidateBreakableRangeName, delegatedRequest, _cancellationToken);

            return result;
        }
    }
}
