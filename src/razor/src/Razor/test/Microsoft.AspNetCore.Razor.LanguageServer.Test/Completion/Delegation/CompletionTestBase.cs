// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;

public abstract class CompletionTestBase(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    private static readonly FormattingOptions s_defaultFormattingOptions = new()
    {
        InsertSpaces = true,
        TabSize = 4
    };

    private IDocumentContextFactory? _documentContextFactory;
    private IDocumentMappingService? _documentMappingService;

    private protected IDocumentContextFactory DocumentContextFactory
    {
        get
        {
            return _documentContextFactory ?? InterlockedOperations.Initialize(ref _documentContextFactory, CreateService());

            static IDocumentContextFactory CreateService()
            {
                return new TestDocumentContextFactory();
            }
        }
    }

    private protected IDocumentMappingService DocumentMappingService
    {
        get
        {
            return _documentMappingService ?? InterlockedOperations.Initialize(ref _documentMappingService, CreateService());

            IDocumentMappingService CreateService()
            {
                return new LspDocumentMappingService(FilePathService, DocumentContextFactory, LoggerFactory);
            }
        }
    }

    private protected DelegatedCompletionListProvider CreateDelegatedCompletionListProvider(
        IClientConnection clientConnection,
        IDocumentMappingService? documentMappingService = null,
        CompletionListCache? completionListCache = null,
        CompletionTriggerAndCommitCharacters? completionCharacters = null)
    {
        return new(
            documentMappingService ?? DocumentMappingService,
            clientConnection,
            completionListCache ?? new(),
            completionCharacters ?? new(TestLanguageServerFeatureOptions.Instance));
    }

    private protected static IClientConnection CreateClientConnectionForCompletion(
        CSharpTestLspServer csharpServer,
        Action<DelegatedCompletionParams>? processParams = null)
    {
        return TestClientConnection.Create(builder =>
        {
            builder.AddFactory<DelegatedCompletionParams, RazorVSInternalCompletionList?>(
                method: LanguageServerConstants.RazorCompletionEndpointName,
                (_, @params, cancellationToken) =>
                {
                    processParams?.Invoke(@params);

                    var csharpDocumentPath = @params.Identifier.TextDocumentIdentifier.DocumentUri.GetRequiredParsedUri().OriginalString + "__virtual.g.cs";
                    var csharpDocumentUri = new Uri(csharpDocumentPath);
                    var csharpCompletionParams = new CompletionParams()
                    {
                        Context = @params.Context,
                        Position = @params.ProjectedPosition,
                        TextDocument = new TextDocumentIdentifier()
                        {
                            DocumentUri = new(csharpDocumentUri)
                        }
                    };

                    return csharpServer.ExecuteRequestAsync<CompletionParams, RazorVSInternalCompletionList?>(
                        Methods.TextDocumentCompletionName,
                        csharpCompletionParams,
                        cancellationToken);
                });
        });
    }

    private protected static IClientConnection CreateClientConnectionForCompletionWithNullResponse(
        Action<DelegatedCompletionParams>? processParams = null)
    {
        return TestClientConnection.Create(builder =>
        {
            builder.AddFactory<DelegatedCompletionParams, RazorVSInternalCompletionList?>(
                method: LanguageServerConstants.RazorCompletionEndpointName,
                (_, @params, _) =>
                {
                    processParams?.Invoke(@params);

                    return Task.FromResult<RazorVSInternalCompletionList?>(null);
                });

            builder.Add(LanguageServerConstants.RazorGetFormattingOptionsEndpointName, s_defaultFormattingOptions);
        });
    }

    private protected static IClientConnection CreateClientConnectionForCompletion(
        RazorVSInternalCompletionList? response = null,
        Action<DelegatedCompletionParams>? processParams = null)
    {
        return TestClientConnection.Create(builder =>
        {
            builder.AddFactory<DelegatedCompletionParams, RazorVSInternalCompletionList>(
                method: LanguageServerConstants.RazorCompletionEndpointName,
                (_, @params, _) =>
                {
                    processParams?.Invoke(@params);

                    return Task.FromResult(response ?? new() { Items = [] });
                });

            builder.Add(LanguageServerConstants.RazorGetFormattingOptionsEndpointName, s_defaultFormattingOptions);
        });
    }

    private protected static IClientConnection CreateClientConnectionForResolve(
        CSharpTestLspServer csharpServer,
        Action<DelegatedCompletionItemResolveParams>? processParams = null)
    {
        return TestClientConnection.Create(builder =>
        {
            builder.AddFactory<DelegatedCompletionItemResolveParams, VSInternalCompletionItem>(
                method: LanguageServerConstants.RazorCompletionResolveEndpointName,
                (_, @params, cancellationToken) =>
                {
                    processParams?.Invoke(@params);

                    return csharpServer.ExecuteRequestAsync<VSInternalCompletionItem, VSInternalCompletionItem>(
                        Methods.TextDocumentCompletionResolveName,
                        @params.CompletionItem,
                        cancellationToken);
                });

            builder.Add(LanguageServerConstants.RazorGetFormattingOptionsEndpointName, s_defaultFormattingOptions);
        });
    }

    private protected static IClientConnection CreateClientConnectionForResolve(
        VSInternalCompletionItem? response,
        Action<DelegatedCompletionItemResolveParams>? processParams = null)
    {
        return TestClientConnection.Create(builder =>
        {
            builder.AddFactory<DelegatedCompletionItemResolveParams, VSInternalCompletionItem>(
                method: LanguageServerConstants.RazorCompletionResolveEndpointName,
                (_, @params, _) =>
                {
                    processParams?.Invoke(@params);

                    return Task.FromResult(response ?? new());
                });

            builder.Add(LanguageServerConstants.RazorGetFormattingOptionsEndpointName, s_defaultFormattingOptions);
        });
    }
}
