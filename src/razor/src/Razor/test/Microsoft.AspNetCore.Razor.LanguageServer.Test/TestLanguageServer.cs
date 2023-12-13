// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test;

internal class TestLanguageServer : IClientConnection
{
    private readonly IReadOnlyDictionary<string, Func<object?, Task<object>>> _requestResponseFactory;

    public TestLanguageServer(Dictionary<string, Func<object?, Task<object>>> requestResponseFactory)
    {
        _requestResponseFactory = requestResponseFactory;
    }

    public Task SendNotificationAsync<TParams>(string method, TParams @params, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SendNotificationAsync(string method, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<TResponse> SendRequestAsync<TParams, TResponse>(string method, TParams @params, CancellationToken cancellationToken)
    {
        if (!_requestResponseFactory.TryGetValue(method, out var factory))
        {
            throw new InvalidOperationException($"No request factory setup for {method}");
        }

        var result = await factory(@params);
        return (TResponse)result;
    }
}
