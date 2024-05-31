// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Composition;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.VisualStudio.LanguageServer.Client;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.LanguageServer.ContainedLanguage;

[Shared]
[Export(typeof(LSPRequestInvoker))]
internal class DefaultLSPRequestInvoker : LSPRequestInvoker
{
    private readonly ILanguageServiceBroker2 _languageServiceBroker;
    private readonly FallbackCapabilitiesFilterResolver _fallbackCapabilitiesFilterResolver;
    private readonly JsonSerializer _serializer;

    [ImportingConstructor]
    public DefaultLSPRequestInvoker(
        ILanguageServiceBroker2 languageServiceBroker,
        FallbackCapabilitiesFilterResolver fallbackCapabilitiesFilterResolver)
    {
        if (languageServiceBroker is null)
        {
            throw new ArgumentNullException(nameof(languageServiceBroker));
        }

        if (fallbackCapabilitiesFilterResolver is null)
        {
            throw new ArgumentNullException(nameof(fallbackCapabilitiesFilterResolver));
        }

        _languageServiceBroker = languageServiceBroker;
        _fallbackCapabilitiesFilterResolver = fallbackCapabilitiesFilterResolver;

        // We need these converters so we don't lose information as part of the deserialization.
        _serializer = new JsonSerializer();
        _serializer.AddVSInternalExtensionConverters();
    }

    [Obsolete]
    public override Task<IEnumerable<ReinvokeResponse<TOut>>> ReinvokeRequestOnMultipleServersAsync<TIn, TOut>(string method, string contentType, TIn parameters, CancellationToken cancellationToken)
    {
        return RequestMultipleServerCoreAsync<TIn, TOut>(method, parameters, cancellationToken);
    }

    [Obsolete]
    public override Task<IEnumerable<ReinvokeResponse<TOut>>> ReinvokeRequestOnMultipleServersAsync<TIn, TOut>(string method, string contentType, Func<JToken, bool> capabilitiesFilter, TIn parameters, CancellationToken cancellationToken)
    {
        return RequestMultipleServerCoreAsync<TIn, TOut>(method, parameters, cancellationToken);
    }

    public override Task<ReinvokeResponse<TOut>> ReinvokeRequestOnServerAsync<TIn, TOut>(
        string method,
        string languageServerName,
        TIn parameters,
        CancellationToken cancellationToken)
    {
        var capabilitiesFilter = _fallbackCapabilitiesFilterResolver.Resolve(method);
        return ReinvokeRequestOnServerAsync<TIn, TOut>(method, languageServerName, capabilitiesFilter, parameters, cancellationToken);
    }

    public override async Task<ReinvokeResponse<TOut>> ReinvokeRequestOnServerAsync<TIn, TOut>(
        string method,
        string languageServerName,
        Func<JToken, bool> capabilitiesFilter,
        TIn parameters,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(method))
        {
            throw new ArgumentException("message", nameof(method));
        }

        var response = await _languageServiceBroker.RequestAsync(
            new GeneralRequest<TIn, TOut> { LanguageServerName = languageServerName, Method = method, Request = parameters },
            cancellationToken);

        // No callers actually use the languageClient when handling the response.
        var result = response is not null ? new ReinvokeResponse<TOut>(languageClient: null!, response) : default;
        return result;
    }

    public override Task<ReinvocationResponse<TOut>?> ReinvokeRequestOnServerAsync<TIn, TOut>(ITextBuffer textBuffer, string method, string languageServerName, TIn parameters, CancellationToken cancellationToken)
    {
        var capabilitiesFilter = _fallbackCapabilitiesFilterResolver.Resolve(method);
        return ReinvokeRequestOnServerAsync<TIn, TOut>(textBuffer, method, languageServerName, capabilitiesFilter, parameters, cancellationToken);
    }

    public override async Task<ReinvocationResponse<TOut>?> ReinvokeRequestOnServerAsync<TIn, TOut>(
        ITextBuffer textBuffer,
        string method,
        string languageServerName,
        Func<JToken, bool> capabilitiesFilter,
        TIn parameters,
        CancellationToken cancellationToken)
    {
        var response = await _languageServiceBroker.RequestAsync(
            new DocumentRequest<TIn, TOut>()
            {
                TextBuffer = textBuffer,
                LanguageServerName = languageServerName,
                ParameterFactory = _ => parameters,
                Method = method,
            },
            cancellationToken);

        if (response is null)
        {
            return null;
        }

        var reinvocationResponse = new ReinvocationResponse<TOut>(languageServerName, response);
        return reinvocationResponse;
    }

    private async Task<IEnumerable<ReinvokeResponse<TOut>>> RequestMultipleServerCoreAsync<TIn, TOut>(string method, TIn parameters, CancellationToken cancellationToken)
        where TIn : notnull
    {
        if (string.IsNullOrEmpty(method))
        {
            throw new ArgumentException("message", nameof(method));
        }

        var reinvokeResponses = _languageServiceBroker.RequestAllAsync(
            new GeneralRequest<TIn, TOut>() { LanguageServerName = null, Method = method, Request = parameters },
            cancellationToken).ConfigureAwait(false);

        using var responses = new PooledArrayBuilder<ReinvokeResponse<TOut>>();
        await foreach (var reinvokeResponse in reinvokeResponses)
        {
            // No callers actually use the languageClient when handling the response.
            responses.Add(new ReinvokeResponse<TOut>(languageClient: null!, reinvokeResponse.response!));
        }

        return responses.ToArray();
    }

    public override IAsyncEnumerable<ReinvocationResponse<TOut>> ReinvokeRequestOnMultipleServersAsync<TIn, TOut>(
        ITextBuffer textBuffer,
        string method,
        TIn parameters,
        CancellationToken cancellationToken)
    {
        var capabilitiesFilter = _fallbackCapabilitiesFilterResolver.Resolve(method);
        return ReinvokeRequestOnMultipleServersAsync<TIn, TOut>(textBuffer, method, capabilitiesFilter, parameters, cancellationToken);
    }

    public override async IAsyncEnumerable<ReinvocationResponse<TOut>> ReinvokeRequestOnMultipleServersAsync<TIn, TOut>(
        ITextBuffer textBuffer,
        string method,
        Func<JToken, bool> capabilitiesFilter,
        TIn parameters,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var requests = _languageServiceBroker.RequestAllAsync(
            new DocumentRequest<TIn, TOut> { ParameterFactory = _ => parameters, Method = method, TextBuffer = textBuffer },
            cancellationToken);

        await foreach (var response in requests)
        {
            yield return new ReinvocationResponse<TOut>(response.client, response.response);
        }
    }
}
