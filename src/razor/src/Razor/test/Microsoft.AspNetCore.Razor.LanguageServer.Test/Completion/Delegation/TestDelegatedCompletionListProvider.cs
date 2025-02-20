// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Test;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;

internal class TestDelegatedCompletionListProvider : DelegatedCompletionListProvider
{
    private readonly CompletionRequestResponseFactory _completionFactory;

    private TestDelegatedCompletionListProvider(
        CompletionRequestResponseFactory completionFactory,
        ILoggerFactory loggerFactory)
        : base(
            new LspDocumentMappingService(new LSPFilePathService(TestLanguageServerFeatureOptions.Instance), new TestDocumentContextFactory(), loggerFactory),
            new TestLanguageServer(new Dictionary<string, Func<object, Task<object>>>()
            {
                [LanguageServerConstants.RazorCompletionEndpointName] = completionFactory.OnDelegationAsync,
            }),
            new CompletionListCache(),
            new CompletionTriggerAndCommitCharacters(TestLanguageServerFeatureOptions.Instance))
    {
        _completionFactory = completionFactory;
    }

    public static TestDelegatedCompletionListProvider Create(
        ILoggerFactory loggerFactory)
        => Create(delegatedCompletionList: null, loggerFactory);

    public static TestDelegatedCompletionListProvider Create(
        CSharpTestLspServer csharpServer,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var requestResponseFactory = new DelegatedCSharpCompletionRequestResponseFactory(csharpServer, cancellationToken);
        var provider = new TestDelegatedCompletionListProvider(requestResponseFactory, loggerFactory);
        return provider;
    }

    public static TestDelegatedCompletionListProvider Create(
        VSInternalCompletionList delegatedCompletionList,
        ILoggerFactory loggerFactory)
    {
        delegatedCompletionList ??= new VSInternalCompletionList()
        {
            Items = Array.Empty<CompletionItem>(),
        };
        var requestResponseFactory = new StaticCompletionRequestResponseFactory(delegatedCompletionList);
        var provider = new TestDelegatedCompletionListProvider(requestResponseFactory, loggerFactory);
        return provider;
    }

    public static TestDelegatedCompletionListProvider CreateWithNullResponse(
        ILoggerFactory loggerFactory)
    {
        var requestResponseFactory = new StaticCompletionRequestResponseFactory(null);
        var provider = new TestDelegatedCompletionListProvider(requestResponseFactory, loggerFactory);
        return provider;
    }

    public DelegatedCompletionParams DelegatedParams => _completionFactory.DelegatedParams;

    private class StaticCompletionRequestResponseFactory : CompletionRequestResponseFactory
    {
        private readonly VSInternalCompletionList _completionResponse;
        private DelegatedCompletionParams _delegatedParams;

        public StaticCompletionRequestResponseFactory(VSInternalCompletionList completionResponse)
        {
            _completionResponse = completionResponse;
        }

        public override DelegatedCompletionParams DelegatedParams => _delegatedParams;

        public override Task<object> OnDelegationAsync(object parameters)
        {
            _delegatedParams = (DelegatedCompletionParams)parameters;

            return Task.FromResult<object>(_completionResponse);
        }
    }

    private class DelegatedCSharpCompletionRequestResponseFactory : CompletionRequestResponseFactory
    {
        private readonly CSharpTestLspServer _csharpServer;
        private readonly CancellationToken _cancellationToken;
        private DelegatedCompletionParams _delegatedParams;

        public DelegatedCSharpCompletionRequestResponseFactory(
            CSharpTestLspServer csharpServer,
            CancellationToken cancellationToken)
        {
            _csharpServer = csharpServer;
            _cancellationToken = cancellationToken;
        }

        public override DelegatedCompletionParams DelegatedParams => _delegatedParams;

        public override async Task<object> OnDelegationAsync(object parameters)
        {
            var completionParams = (DelegatedCompletionParams)parameters;
            _delegatedParams = completionParams;

            var csharpDocumentPath = completionParams.Identifier.TextDocumentIdentifier.Uri.OriginalString + "__virtual.g.cs";
            var csharpDocumentUri = new Uri(csharpDocumentPath);
            var csharpCompletionParams = new CompletionParams()
            {
                Context = completionParams.Context,
                Position = completionParams.ProjectedPosition,
                TextDocument = new TextDocumentIdentifier()
                {
                    Uri = csharpDocumentUri,
                }
            };

            var delegatedCompletionList = await _csharpServer.ExecuteRequestAsync<CompletionParams, VSInternalCompletionList>(
                Methods.TextDocumentCompletionName,
                csharpCompletionParams,
                _cancellationToken);

            return delegatedCompletionList;
        }
    }

    private abstract class CompletionRequestResponseFactory
    {
        public abstract DelegatedCompletionParams DelegatedParams { get; }

        public abstract Task<object> OnDelegationAsync(object parameters);
    }
}
