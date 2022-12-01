// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.LanguageServer.Client;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.LanguageServer.ContainedLanguage.MessageInterception;

/// <summary>
/// Receives notification messages from the server and invokes any applicable message interception layers.
/// </summary>
internal class InterceptionMiddleLayer : ILanguageClientMiddleLayer
{
    private readonly InterceptorManager _interceptorManager;
    private readonly string _contentType;

    /// <summary>
    /// Create the middle layer
    /// </summary>
    /// <param name="interceptorManager">Interception manager</param>
    /// <param name="contentType">The content type name of the language for the ILanguageClient using this middle layer</param>
    public InterceptionMiddleLayer(InterceptorManager interceptorManager, string contentType)
    {
        _interceptorManager = interceptorManager ?? throw new ArgumentNullException(nameof(interceptorManager));
        _contentType = !string.IsNullOrEmpty(contentType) ? contentType : throw new ArgumentException("Cannot be empty", nameof(contentType));
    }

    public bool CanHandle(string methodName)
    {
        return _interceptorManager.HasInterceptor(methodName, _contentType);
    }

    public async Task HandleNotificationAsync(string methodName, JToken methodParam, Func<JToken, Task> sendNotification)
    {
        var payload = methodParam;
        if (CanHandle(methodName))
        {
            payload = await _interceptorManager.ProcessInterceptorsAsync(methodName, methodParam, _contentType, CancellationToken.None);
        }

        if (payload is not null)
        {
            // this completes the handshake to give the payload back to the client.
            await sendNotification(payload);
        }
    }

    public async Task<JToken?> HandleRequestAsync(string methodName, JToken methodParam, Func<JToken, Task<JToken?>> sendRequest)
    {
        // First send the request through.
        // We don't yet have a scenario where the request needs to be intercepted, but if one does come up, we may need to redesign the interception handshake
        // to handle both request and response interception.
        var response = await sendRequest(methodParam);

        if (response is not null && CanHandle(methodName))
        {
            response = await _interceptorManager.ProcessInterceptorsAsync(methodName, response, _contentType, CancellationToken.None);
        }

        return response;
    }
}
