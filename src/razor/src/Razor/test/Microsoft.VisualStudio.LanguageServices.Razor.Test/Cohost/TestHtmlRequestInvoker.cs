// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

internal class TestHtmlRequestInvoker : IHtmlRequestInvoker
{
    private readonly Dictionary<string, object?> _htmlResponses;

    public TestHtmlRequestInvoker(params (string method, object? response)[] htmlResponses)
    {
        _htmlResponses = htmlResponses.ToDictionary(kvp => kvp.method, kvp => kvp.response);
    }

    public Task<TResponse?> MakeHtmlLspRequestAsync<TRequest, TResponse>(TextDocument razorDocument, string method, TRequest request, TimeSpan threshold, Guid correlationId, CancellationToken cancellationToken) where TRequest : notnull
    {
        if (_htmlResponses is not null &&
            _htmlResponses.TryGetValue(method, out var response))
        {
            return Task.FromResult((TResponse?)response);
        }

        return Task.FromResult<TResponse?>(default);
    }
}
