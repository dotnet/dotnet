// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class TestClient : IClientConnection
{
    public TestClient()
    {
    }

    private readonly List<UpdateBufferRequest> _updateRequests = new();

    private readonly List<RequestPair> _requests = new();

    public IReadOnlyList<UpdateBufferRequest> UpdateRequests => _updateRequests;

    public IReadOnlyList<RequestPair> Requests => _requests;

    public InitializeResult ServerSettings => throw new NotImplementedException();

    public object GetService(Type serviceType)
    {
        throw new NotImplementedException();
    }

    public Task SendNotificationAsync(string methodName, CancellationToken cancellationToken)
    {
        _requests.Add(new RequestPair(methodName, Params: null));

        return Task.CompletedTask;
    }

    public Task SendNotificationAsync<TParams>(string methodName, TParams @params, CancellationToken cancellationToken)
    {
        if (@params is UpdateBufferRequest updateRequest)
        {
            _updateRequests.Add(updateRequest);
        }
        else
        {
            _requests.Add(new RequestPair(methodName, @params));
        }

        return Task.CompletedTask;
    }

    public Task<TResponse> SendRequestAsync<TParams, TResponse>(string methodName, TParams @params, CancellationToken cancellationToken)
    {
        _requests.Add(new RequestPair(methodName, @params));
        return Task.FromResult<TResponse>(default!);
    }

    internal record RequestPair(string Method, object? Params);
}
