// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test;

internal class TestLanguageServer : ClientNotifierServiceBase
{
    private readonly IReadOnlyDictionary<string, Func<object?, Task<object>>> _requestResponseFactory;

    public TestLanguageServer(Dictionary<string, Func<object?, Task<object>>> requestResponseFactory)
    {
        _requestResponseFactory = requestResponseFactory;
    }

    public override Task OnInitializedAsync(VSInternalClientCapabilities clientCapabilities, CancellationToken cancellationToken) => Task.CompletedTask;

    public override Task SendNotificationAsync<TParams>(string method, TParams @params, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public override Task SendNotificationAsync(string method, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public override async Task<TResponse> SendRequestAsync<TParams, TResponse>(string method, TParams @params, CancellationToken cancellationToken)
    {
        if (!_requestResponseFactory.TryGetValue(method, out var factory))
        {
            throw new InvalidOperationException($"No request factory setup for {method}");
        }

        var result = await factory(@params);
        return (TResponse)result;
    }
}
