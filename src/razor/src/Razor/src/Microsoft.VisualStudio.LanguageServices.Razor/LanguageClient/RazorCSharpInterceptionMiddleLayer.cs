// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces.Protocol.SemanticTokens;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.Razor.LanguageClient;

[Export(typeof(IRazorCSharpInterceptionMiddleLayer))]
[method: ImportingConstructor]
internal class RazorCSharpInterceptionMiddleLayer(LSPRequestInvoker requestInvoker) : IRazorCSharpInterceptionMiddleLayer
{
    private readonly LSPRequestInvoker _requestInvoker = requestInvoker;

    public bool CanHandle(string methodName)
        => methodName.Equals(Methods.WorkspaceSemanticTokensRefreshName);

    public Task HandleNotificationAsync(string methodName, JToken methodParam, Func<JToken, Task> sendNotification)
    {
        // IMPORTANT: This API shape is old and the methodParam and sendNotification parameters are likely to be null
        // as Roslyn moves away from Newtonsoft.Json. Do not use these parameters without designing a new API contract.
        Debug.Assert(CanHandle(methodName), "Got a call to intercept a message we were not expecting");

        // Normally we use the LSPRequestInvoker when we're on the client side of things, and want
        // to send to a server, so at first glance this might seem redundant. However, we're actually
        // intercepting a message from the Roslyn server, so need to call into our server, so we can turn
        // around and call the client from our server, because its our server we want it to refresh
        // semantic tokens from.

        var refreshParams = new SemanticTokensRefreshParams();
        return _requestInvoker.ReinvokeRequestOnServerAsync<SemanticTokensRefreshParams, Unit>(
            CustomMessageNames.RazorSemanticTokensRefreshEndpoint,
            RazorLSPConstants.RazorLanguageServerName,
            refreshParams,
            CancellationToken.None);
    }

    public Task<JToken?> HandleRequestAsync(string methodName, JToken methodParam, Func<JToken, Task<JToken?>> sendRequest)
        => throw new NotImplementedException();

    // A basic POCO which will handle the lack of data in the response.
    private class Unit
    {

    }
}
