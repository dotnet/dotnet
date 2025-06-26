// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectContexts;

[RazorLanguageServerEndpoint(VSMethods.GetProjectContextsName)]
// Using a documentless handler here because:
//   a. We don't need any extra info than just the Uri
//   b. If we say we have a document, then our RequestContextFactory will try to get a DocumentContext for us
//      but as the ProjectContexts endpoint, the request won't have a project context, so it won't be able to
//      get the "right" one anyway. But as I said we don't need it to.
//   c. This gets called a lot, so may as well save some work
internal class ProjectContextsEndpoint : IRazorDocumentlessRequestHandler<VSGetProjectContextsParams, VSProjectContextList>, ICapabilitiesProvider
{
    private readonly IClientConnection _clientConnection;

    public ProjectContextsEndpoint(IClientConnection clientConnection)
    {
        _clientConnection = clientConnection ?? throw new ArgumentNullException(nameof(clientConnection));
    }

    public bool MutatesSolutionState => false;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.ProjectContextProvider = true;
    }

    public async Task<VSProjectContextList> HandleRequestAsync(VSGetProjectContextsParams request, RazorRequestContext context, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var delegatedParams = new DelegatedProjectContextsParams(request.TextDocument.DocumentUri.GetRequiredParsedUri());

        var response = await _clientConnection.SendRequestAsync<DelegatedProjectContextsParams, VSProjectContextList>(
            CustomMessageNames.RazorProjectContextsEndpoint,
            delegatedParams,
            cancellationToken).ConfigureAwait(false);

        return response ?? new();
    }
}
