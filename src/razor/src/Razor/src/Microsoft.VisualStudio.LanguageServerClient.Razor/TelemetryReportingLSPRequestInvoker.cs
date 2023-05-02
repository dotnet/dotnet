// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

internal class TelemetryReportingLSPRequestInvoker : LSPRequestInvoker
{
    private readonly LSPRequestInvoker _requestInvoker;
    private readonly ITelemetryReporter _telemetryReporter;

    public TelemetryReportingLSPRequestInvoker(LSPRequestInvoker requestInvoker, ITelemetryReporter telemetryReporter)
    {
        _requestInvoker = requestInvoker;
        _telemetryReporter = telemetryReporter;
    }

    public override Task<IEnumerable<ReinvokeResponse<TOut>>> ReinvokeRequestOnMultipleServersAsync<TIn, TOut>(string method, string contentType, TIn parameters, CancellationToken cancellationToken)
    {
        return _requestInvoker.ReinvokeRequestOnMultipleServersAsync<TIn, TOut>(method, contentType, parameters, cancellationToken);
    }

    public override Task<IEnumerable<ReinvokeResponse<TOut>>> ReinvokeRequestOnMultipleServersAsync<TIn, TOut>(string method, string contentType, Func<JToken, bool> capabilitiesFilter, TIn parameters, CancellationToken cancellationToken)
    {
        return _requestInvoker.ReinvokeRequestOnMultipleServersAsync<TIn, TOut>(method, contentType, capabilitiesFilter, parameters, cancellationToken);
    }

    public override IAsyncEnumerable<ReinvocationResponse<TOut>> ReinvokeRequestOnMultipleServersAsync<TIn, TOut>(ITextBuffer textBuffer, string method, TIn parameters, CancellationToken cancellationToken)
    {
        return _requestInvoker.ReinvokeRequestOnMultipleServersAsync<TIn, TOut>(textBuffer, method, parameters, cancellationToken);
    }

    public override IAsyncEnumerable<ReinvocationResponse<TOut>> ReinvokeRequestOnMultipleServersAsync<TIn, TOut>(ITextBuffer textBuffer, string method, Func<JToken, bool> capabilitiesFilter, TIn parameters, CancellationToken cancellationToken)
    {
        return _requestInvoker.ReinvokeRequestOnMultipleServersAsync<TIn, TOut>(textBuffer, method, capabilitiesFilter, parameters, cancellationToken);
    }

    public override Task<ReinvokeResponse<TOut>> ReinvokeRequestOnServerAsync<TIn, TOut>(string method, string languageServerName, TIn parameters, CancellationToken cancellationToken)
    {
        using (Track(nameof(ReinvokeRequestOnServerAsync), method, languageServerName))
        {
            return _requestInvoker.ReinvokeRequestOnServerAsync<TIn, TOut>(method, languageServerName, parameters, cancellationToken);
        }
    }

    public override Task<ReinvokeResponse<TOut>> ReinvokeRequestOnServerAsync<TIn, TOut>(string method, string languageServerName, Func<JToken, bool> capabilitiesFilter, TIn parameters, CancellationToken cancellationToken)
    {
        using (Track(nameof(ReinvokeRequestOnServerAsync), method, languageServerName))
        {
            return _requestInvoker.ReinvokeRequestOnServerAsync<TIn, TOut>(method, languageServerName, capabilitiesFilter, parameters, cancellationToken);
        }
    }

    public override Task<ReinvocationResponse<TOut>?> ReinvokeRequestOnServerAsync<TIn, TOut>(ITextBuffer textBuffer, string method, string languageServerName, TIn parameters, CancellationToken cancellationToken)
    {
        using (Track(nameof(ReinvokeRequestOnServerAsync), method, languageServerName))
        {
            return _requestInvoker.ReinvokeRequestOnServerAsync<TIn, TOut>(textBuffer, method, languageServerName, parameters, cancellationToken);
        }
    }

    public override Task<ReinvocationResponse<TOut>?> ReinvokeRequestOnServerAsync<TIn, TOut>(ITextBuffer textBuffer, string method, string languageServerName, Func<JToken, bool> capabilitiesFilter, TIn parameters, CancellationToken cancellationToken)
    {
        using (Track(nameof(ReinvokeRequestOnServerAsync), method, languageServerName))
        {
            return _requestInvoker.ReinvokeRequestOnServerAsync<TIn, TOut>(textBuffer, method, languageServerName, capabilitiesFilter, parameters, cancellationToken);
        }
    }

    private IDisposable? Track(string name, string method, string languageServerName)
    {
        return _telemetryReporter.BeginBlock(name, Severity.Normal, ImmutableDictionary.CreateRange(new KeyValuePair<string, object?>[]
        {
            new("eventscope.method", method),
            new("eventscope.languageservername", languageServerName),
        }));
    }
}
