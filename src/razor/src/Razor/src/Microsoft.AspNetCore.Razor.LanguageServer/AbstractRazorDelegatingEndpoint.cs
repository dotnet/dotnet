// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using StreamJsonRpc;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal abstract class AbstractRazorDelegatingEndpoint<TRequest, TResponse> : IRazorRequestHandler<TRequest, TResponse?>
   where TRequest : ITextDocumentPositionParams
{
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;
    private readonly IRazorDocumentMappingService _documentMappingService;
    private readonly IClientConnection _clientConnection;
    protected readonly ILogger Logger;

    protected AbstractRazorDelegatingEndpoint(
        LanguageServerFeatureOptions languageServerFeatureOptions,
        IRazorDocumentMappingService documentMappingService,
        IClientConnection clientConnection,
        ILogger logger)
    {
        _languageServerFeatureOptions = languageServerFeatureOptions ?? throw new ArgumentNullException(nameof(languageServerFeatureOptions));
        _documentMappingService = documentMappingService ?? throw new ArgumentNullException(nameof(documentMappingService));
        _clientConnection = clientConnection ?? throw new ArgumentNullException(nameof(clientConnection));

        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// The strategy to use to project the incoming caret position onto the generated C#/Html document
    /// </summary>
    protected virtual IDocumentPositionInfoStrategy DocumentPositionInfoStrategy { get; } = DefaultDocumentPositionInfoStrategy.Instance;

    protected bool SingleServerSupport => _languageServerFeatureOptions.SingleServerSupport;

    protected virtual bool OnlySingleServer { get; } = true;

    /// <summary>
    /// When <see langword="true" />, we'll try to map the cursor position to C# even when it is in a Html context, for example
    /// for component attributes that are fully within a Html context, but map to a C# property write in the generated document.
    /// </summary>
    protected virtual bool PreferCSharpOverHtmlIfPossible { get; } = false;

    /// <summary>
    /// The name of the endpoint to delegate to, from <see cref="CustomMessageNames"/>. This is the
    /// custom endpoint that is sent via <see cref="IClientConnection"/> which returns
    /// a response by delegating to C#/HTML.
    /// </summary>
    /// <remarks>
    /// An example is <see cref="CustomMessageNames.RazorHoverEndpointName"/>
    /// </remarks>
    protected abstract string CustomMessageTarget { get; }

    public virtual bool MutatesSolutionState { get; } = false;

    /// <summary>
    /// The delegated object to send to the <see cref="CustomMessageTarget"/>
    /// </summary>
    protected abstract Task<IDelegatedParams?> CreateDelegatedParamsAsync(TRequest request, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken);

    /// <summary>
    /// If the response needs to be handled, such as for remapping positions back, override and handle here
    /// </summary>
    protected virtual Task<TResponse> HandleDelegatedResponseAsync(TResponse delegatedResponse, TRequest originalRequest, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
        => Task.FromResult(delegatedResponse);

    /// <summary>
    /// If the request can be handled without delegation, override this to provide a response. If a null
    /// value is returned the request will be delegated to C#/HTML servers, otherwise the response
    /// will be used in <see cref="HandleRequestAsync(TRequest, RazorRequestContext, CancellationToken)"/>
    /// </summary>
    protected virtual Task<TResponse?> TryHandleAsync(TRequest request, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
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

        var positionInfo = await DocumentPositionInfoStrategy.TryGetPositionInfoAsync(_documentMappingService, documentContext, request.Position, Logger, cancellationToken).ConfigureAwait(false);
        if (positionInfo is null)
        {
            return default;
        }

        var response = await TryHandleAsync(request, requestContext, positionInfo, cancellationToken).ConfigureAwait(false);
        if (response is not null && response is not ISumType { Value: null })
        {
            return response;
        }

        if (OnlySingleServer && !_languageServerFeatureOptions.SingleServerSupport)
        {
            return default;
        }

        if (positionInfo.LanguageKind == RazorLanguageKind.Razor)
        {
            // We can only delegate to C# and HTML, so if we're in a Razor context and our inheritor didn't want to provide
            // any response then that's all we can do.
            return default;
        }
        else if (positionInfo.LanguageKind == RazorLanguageKind.Html && PreferCSharpOverHtmlIfPossible)
        {
            // Sometimes Html can actually be mapped to C#, like for example component attributes, which map to
            // C# properties, even though they appear entirely in a Html context. Since remapping is pretty cheap
            // it's easier to just try mapping, and see what happens, rather than checking for specific syntax nodes.
            var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
            if (_documentMappingService.TryMapToGeneratedDocumentPosition(codeDocument.GetCSharpDocument(), positionInfo.HostDocumentIndex, out Position? csharpPosition, out _))
            {
                // We're just gonna pretend this mapped perfectly normally onto C#. Moving this logic to the actual position info
                // calculating code is possible, but could have untold effects, so opt-in is better (for now?)
                positionInfo = new DocumentPositionInfo(RazorLanguageKind.CSharp, csharpPosition, positionInfo.HostDocumentIndex);
            }
        }

        var delegatedParams = await CreateDelegatedParamsAsync(request, requestContext, positionInfo, cancellationToken).ConfigureAwait(false);

        if (delegatedParams is null)
        {
            // I guess they don't want to delegate... fine then!
            return default;
        }

        TResponse? delegatedRequest;
        try
        {
            delegatedRequest = await _clientConnection.SendRequestAsync<IDelegatedParams, TResponse>(CustomMessageTarget, delegatedParams, cancellationToken).ConfigureAwait(false);
            if (delegatedRequest is null)
            {
                return default;
            }
        }
        catch (RemoteInvocationException e)
        {
            Logger.LogError(e, "Error calling delegate server for {method}", CustomMessageTarget);
            requestContext.GetRequiredService<ITelemetryReporter>().ReportFault(e, "Error calling delegate server for {method}", CustomMessageTarget);
            throw;
        }

        var remappedResponse = await HandleDelegatedResponseAsync(delegatedRequest, request, requestContext, positionInfo, cancellationToken).ConfigureAwait(false);

        return remappedResponse;
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(TRequest request)
    {
        return request.TextDocument;
    }
}
