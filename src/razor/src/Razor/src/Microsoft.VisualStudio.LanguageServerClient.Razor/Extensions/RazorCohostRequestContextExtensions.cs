// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Cohost;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Extensions;

internal static class RazorCohostRequestContextExtensions
{
    public static async Task<TResponse?> DelegateRequestAsync<TDelegatedParams, TResponse>(this RazorCohostRequestContext requestContext, string target, TDelegatedParams @params, ILogger logger, CancellationToken cancellationToken)
    {
        var clientConnection = requestContext.GetClientConnection();

        try
        {
            return await clientConnection.SendRequestAsync<TDelegatedParams, TResponse>(target, @params, cancellationToken).ConfigureAwait(false);
        }
        catch (RemoteInvocationException e)
        {
            logger.LogError(e, "Error calling delegate server for {method}", target);
            throw;
        }
    }

    public static RazorCohostClientConnection GetClientConnection(this RazorCohostRequestContext requestContext)
    {
        // TODO: We can't MEF import IRazorCohostClientLanguageServerManager in the constructor. We can make this work
        //       by having it implement a base class, RazorClientConnectionBase or something, that in turn implements
        //       AbstractRazorLspService (defined in Roslyn) and then move everything from importing IClientConnection
        //       to importing the new base class, so we can continue to share services.
        //
        //       Until then we have to get the service from the request context.
        var clientLanguageServerManager = requestContext.GetRequiredService<IRazorCohostClientLanguageServerManager>();
        return new RazorCohostClientConnection(clientLanguageServerManager);
    }
}
