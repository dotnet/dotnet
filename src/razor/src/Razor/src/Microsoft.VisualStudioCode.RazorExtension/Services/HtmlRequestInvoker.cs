// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

namespace Microsoft.VisualStudioCode.RazorExtension.Services;

[Export(typeof(IHtmlRequestInvoker))]
[method: ImportingConstructor]
internal sealed class HtmlRequestInvoker(
    RazorClientServerManagerProvider razorClientServerManagerProvider,
    IHtmlDocumentSynchronizer htmlDocumentSynchronizer,
    ILoggerFactory loggerFactory) : IHtmlRequestInvoker
{
    private readonly RazorClientServerManagerProvider _razorClientServerManagerProvider = razorClientServerManagerProvider;
    private readonly IHtmlDocumentSynchronizer _htmlDocumentSynchronizer = htmlDocumentSynchronizer;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<HtmlRequestInvoker>();

    public async Task<TResponse?> MakeHtmlLspRequestAsync<TRequest, TResponse>(TextDocument razorDocument, string method, TRequest request, TimeSpan threshold, Guid correlationId, CancellationToken cancellationToken) where TRequest : notnull
    {
        var syncResult = await _htmlDocumentSynchronizer.TrySynchronizeAsync(razorDocument, cancellationToken).ConfigureAwait(false);
        if (!syncResult)
        {
            return default;
        }

        _logger.LogDebug($"Making Html request for {method} on {razorDocument.FilePath}");

        var clientConnection = _razorClientServerManagerProvider.ClientLanguageServerManager.AssumeNotNull();
        return await clientConnection.SendRequestAsync<TRequest, TResponse>(method, request, cancellationToken).ConfigureAwait(false);
    }
}
