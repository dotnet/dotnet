// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.Text;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.Razor.LanguageClient;

internal class TestLSPRequestInvoker : LSPRequestInvoker
{
    private readonly CSharpTestLspServer _csharpServer;
    private readonly Dictionary<string, object> _htmlResponses;

    public TestLSPRequestInvoker() { }

    public TestLSPRequestInvoker(List<(string method, object response)> htmlResponses)
    {
        _htmlResponses = htmlResponses.ToDictionary(kvp => kvp.method, kvp => kvp.response);
    }

    public TestLSPRequestInvoker(CSharpTestLspServer csharpServer)
    {
        if (csharpServer is null)
        {
            throw new ArgumentNullException(nameof(csharpServer));
        }

        _csharpServer = csharpServer;
    }

    [Obsolete]
    public override Task<IEnumerable<ReinvokeResponse<TOut>>> ReinvokeRequestOnMultipleServersAsync<TIn, TOut>(
        string method,
        string contentType,
        TIn parameters,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [Obsolete]
    public override Task<IEnumerable<ReinvokeResponse<TOut>>> ReinvokeRequestOnMultipleServersAsync<TIn, TOut>(
        string method,
        string contentType,
        Func<JToken, bool> capabilitiesFilter,
        TIn parameters,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public override IAsyncEnumerable<ReinvocationResponse<TOut>> ReinvokeRequestOnMultipleServersAsync<TIn, TOut>(
        ITextBuffer textBuffer,
        string method,
        TIn parameters,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [Obsolete]
    public override IAsyncEnumerable<ReinvocationResponse<TOut>> ReinvokeRequestOnMultipleServersAsync<TIn, TOut>(
        ITextBuffer textBuffer,
        string method,
        Func<JToken, bool> capabilitiesFilter,
        TIn parameters,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public override Task<ReinvokeResponse<TOut>> ReinvokeRequestOnServerAsync<TIn, TOut>(
        string method,
        string languageServerName,
        TIn parameters,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [Obsolete]
    public override Task<ReinvokeResponse<TOut>> ReinvokeRequestOnServerAsync<TIn, TOut>(
        string method,
        string languageServerName,
        Func<JToken, bool> capabilitiesFilter,
        TIn parameters,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async override Task<ReinvocationResponse<TOut>> ReinvokeRequestOnServerAsync<TIn, TOut>(
        ITextBuffer textBuffer,
        string method,
        string languageServerName,
        TIn parameters,
        CancellationToken cancellationToken)
    {
        if (languageServerName is RazorLSPConstants.RazorCSharpLanguageServerName)
        {
            var result = await _csharpServer.ExecuteRequestAsync<TIn, TOut>(method, parameters, cancellationToken).ConfigureAwait(false);
            return new ReinvocationResponse<TOut>(languageClientName: RazorLSPConstants.RazorCSharpLanguageServerName, result);
        }

        if (_htmlResponses is not null &&
            _htmlResponses.TryGetValue(method, out var response))
        {
            return new ReinvocationResponse<TOut>(languageClientName: "html", (TOut)response);
        }

        return default;
    }

    [Obsolete]
    public override Task<ReinvocationResponse<TOut>> ReinvokeRequestOnServerAsync<TIn, TOut>(
        ITextBuffer textBuffer,
        string method,
        string languageServerName,
        Func<JToken, bool> capabilitiesFilter,
        TIn parameters,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
