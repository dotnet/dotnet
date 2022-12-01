// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal abstract class AbstractRazorDelegatingEndpoint<TRequest, TResponse> : IRazorRequestHandler<TRequest, TResponse?>
   where TRequest : ITextDocumentPositionParams
{
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;
    private readonly RazorDocumentMappingService _documentMappingService;
    private readonly ClientNotifierServiceBase _languageServer;
    protected readonly ILogger Logger;

    protected AbstractRazorDelegatingEndpoint(
        LanguageServerFeatureOptions languageServerFeatureOptions,
        RazorDocumentMappingService documentMappingService,
        ClientNotifierServiceBase languageServer,
        ILogger logger)
    {
        _languageServerFeatureOptions = languageServerFeatureOptions ?? throw new ArgumentNullException(nameof(languageServerFeatureOptions));
        _documentMappingService = documentMappingService ?? throw new ArgumentNullException(nameof(documentMappingService));
        _languageServer = languageServer ?? throw new ArgumentNullException(nameof(languageServer));

        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected virtual bool OnlySingleServer { get; } = true;

    /// <summary>
    /// The name of the endpoint to delegate to, from <see cref="RazorLanguageServerCustomMessageTargets"/>. This is the
    /// custom endpoint that is sent via <see cref="ClientNotifierServiceBase"/> which returns
    /// a response by delegating to C#/HTML.
    /// </summary>
    /// <remarks>
    /// An example is <see cref="RazorLanguageServerCustomMessageTargets.RazorHoverEndpointName"/>
    /// </remarks>
    protected abstract string CustomMessageTarget { get; }

    public bool MutatesSolutionState { get; } = false;

    /// <summary>
    /// The delegated object to send to the <see cref="CustomMessageTarget"/>
    /// </summary>
    protected abstract Task<IDelegatedParams?> CreateDelegatedParamsAsync(TRequest request, RazorRequestContext requestContext, Projection projection, CancellationToken cancellationToken);

    /// <summary>
    /// If the response needs to be handled, such as for remapping positions back, override and handle here
    /// </summary>
    protected virtual Task<TResponse> HandleDelegatedResponseAsync(TResponse delegatedResponse, TRequest originalRequest, RazorRequestContext requestContext, Projection projection, CancellationToken cancellationToken)
        => Task.FromResult(delegatedResponse);

    /// <summary>
    /// If the request can be handled without delegation, override this to provide a response. If a null
    /// value is returned the request will be delegated to C#/HTML servers, otherwise the response
    /// will be used in <see cref="HandleRequestAsync(TRequest, RazorRequestContext, CancellationToken)"/>
    /// </summary>
    protected virtual Task<TResponse?> TryHandleAsync(TRequest request, RazorRequestContext requestContext, Projection projection, CancellationToken cancellationToken)
        => Task.FromResult<TResponse?>(default);

    /// <summary>
    /// Returns true if the configuration supports this operation being handled, otherwise returns false. Use to
    /// handle cases where <see cref="LanguageServerFeatureOptions"/> other than <see cref="LanguageServerFeatureOptions.SingleServerSupport"/>
    /// need to be checked to validate that the operation can be done.
    /// </summary>
    protected virtual bool IsSupported() => true;

    /// <summary>
    /// Implementation for <see cref="HandleRequestAsync(TRequest, RazorRequestContext, CancellationToken)"/>
    /// </summary>
    public async Task<TResponse?> HandleRequestAsync(TRequest request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (!IsSupported())
        {
            return default;
        }

        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            return default;
        }

        var projection = await _documentMappingService.TryGetProjectionAsync(documentContext, request.Position, requestContext.Logger, cancellationToken).ConfigureAwait(false);
        if (projection is null)
        {
            return default;
        }

        var response = await TryHandleAsync(request, requestContext, projection, cancellationToken).ConfigureAwait(false);
        if (response is not null && response is not ISumType { Value: null })
        {
            return response;
        }

        if (OnlySingleServer && !_languageServerFeatureOptions.SingleServerSupport)
        {
            return default;
        }

        // We can only delegate to C# and HTML, so if we're in a Razor context and our inheritor didn't want to provide
        // any response then that's all we can do.
        if (projection.LanguageKind == RazorLanguageKind.Razor)
        {
            return default;
        }

        var delegatedParams = await CreateDelegatedParamsAsync(request, requestContext, projection, cancellationToken);

        if (delegatedParams is null)
        {
            // I guess they don't want to delegate... fine then!
            return default;
        }

        var delegatedRequest = await _languageServer.SendRequestAsync<IDelegatedParams, TResponse>(CustomMessageTarget, delegatedParams, cancellationToken).ConfigureAwait(false);
        if (delegatedRequest is null)
        {
            return default;
        }

        var remappedResponse = await HandleDelegatedResponseAsync(delegatedRequest, request, requestContext, projection, cancellationToken).ConfigureAwait(false);

        return remappedResponse;
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(TRequest request)
    {
        return request.TextDocument;
    }
}
