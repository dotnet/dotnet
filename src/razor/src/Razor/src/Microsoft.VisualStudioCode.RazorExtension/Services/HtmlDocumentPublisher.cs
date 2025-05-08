// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

namespace Microsoft.VisualStudioCode.RazorExtension.Services;

[Export(typeof(IHtmlDocumentPublisher))]
[method: ImportingConstructor]
internal sealed class HtmlDocumentPublisher(
    RazorClientServerManagerProvider razorClientServerManagerProvider) : IHtmlDocumentPublisher
{
    private readonly RazorClientServerManagerProvider _razorClientServerManagerProvider = razorClientServerManagerProvider;

    public async Task PublishAsync(TextDocument document, SynchronizationResult synchronizationResult, string htmlText, CancellationToken cancellationToken)
    {
        Assumed.True(synchronizationResult.Synchronized);

        var request = new HtmlUpdateParameters(new TextDocumentIdentifier { Uri = document.CreateUri() }, synchronizationResult.Checksum.ToString(), htmlText);

        var clientConnection = _razorClientServerManagerProvider.ClientLanguageServerManager.AssumeNotNull();
        await clientConnection.SendRequestAsync("razor/updateHtml", request, cancellationToken).ConfigureAwait(false);
    }

    private record HtmlUpdateParameters(
        [property: JsonPropertyName("textDocument")]
        TextDocumentIdentifier TextDocument,
        [property: JsonPropertyName("checksum")]
        string Checksum,
        [property: JsonPropertyName("text")]
        string Text);
}
