// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CodeAnalysis.Razor.Workspaces.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

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

        var delegatedParams = new DelegatedProjectContextsParams(request.TextDocument.Uri);

        var response = await _clientConnection.SendRequestAsync<DelegatedProjectContextsParams, VSProjectContextList>(
            CustomMessageNames.RazorProjectContextsEndpoint,
            delegatedParams,
            cancellationToken).ConfigureAwait(false);

        return response ?? new();
    }
}
