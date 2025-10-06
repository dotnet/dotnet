// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;

/// <summary>
///  Helper implementation of <see cref="IClientConnection"/>.
/// </summary>
internal sealed partial class TestClientConnection : IClientConnection
{
    private readonly Dictionary<string, object?> _methodToResponseMap;

    private TestClientConnection(Dictionary<string, object?> methodToResponseMap)
    {
        _methodToResponseMap = methodToResponseMap;
    }

    public static IClientConnection Create(Action<Builder> configure)
    {
        var builder = new Builder();
        configure(builder);

        return builder.ToClientConnection();
    }

    public Task SendNotificationAsync<TParams>(string method, TParams @params, CancellationToken cancellationToken)
        => throw new NotImplementedException();

    public Task SendNotificationAsync(string method, CancellationToken cancellationToken)
        => throw new NotImplementedException();

    public Task<TResponse> SendRequestAsync<TParams, TResponse>(string method, TParams @params, CancellationToken cancellationToken)
    {
        Assert.True(_methodToResponseMap.TryGetValue(method, out var response), $"'{method}' is not registered.");
        Assert.NotNull(@params);

        if (response is ResponseFactory<TParams, TResponse> typedResponseFactory)
        {
            return typedResponseFactory(method, @params, cancellationToken);
        }

        var typedResponse = (TResponse?)response;

        return Task.FromResult(typedResponse!);
    }

    public delegate Task<TResponse> ResponseFactory<TParams, TResponse>(string method, TParams @params, CancellationToken cancellationToken);

    public sealed class Builder
    {
        private readonly Dictionary<string, object?> _methodToResponseMap = new(StringComparer.Ordinal);

        public void Add<TResponse>(string method, TResponse response)
        {
            Assert.False(_methodToResponseMap.ContainsKey(method), $"'{method}' has already been registered.");

            _methodToResponseMap.Add(method, response);
        }

        public void AddFactory<TParams, TResponse>(string method, ResponseFactory<TParams, TResponse> responseFactory)
        {
            Assert.False(_methodToResponseMap.ContainsKey(method), $"'{method}' has already been registered.");

            _methodToResponseMap.Add(method, responseFactory);
        }

        public IClientConnection ToClientConnection()
        {
            return new TestClientConnection(_methodToResponseMap);
        }
    }
}
