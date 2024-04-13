// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.VisualStudio.Razor.LanguageClient.Extensions;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Endpoints;

internal partial class RazorCustomMessageTarget
{
    // Called by the Razor Language Server to update the contents of the virtual Html buffer.
    [JsonRpcMethod(CustomMessageNames.RazorUpdateHtmlBufferEndpoint, UseSingleObjectParameterDeserialization = true)]
    public async Task UpdateHtmlBufferAsync(UpdateBufferRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        await _joinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        UpdateHtmlBuffer(request);
    }

    // Internal for testing
    internal void UpdateHtmlBuffer(UpdateBufferRequest request)
    {
        if (request is null || request.HostDocumentFilePath is null || request.HostDocumentVersion is null)
        {
            return;
        }

        var hostDocumentUri = new Uri(request.HostDocumentFilePath);
        _documentManager.UpdateVirtualDocument<HtmlVirtualDocument>(
            hostDocumentUri,
            request.Changes.Select(change => change.ToVisualStudioTextChange()).ToArray(),
            request.HostDocumentVersion.Value,
            state: request.PreviousWasEmpty);
    }
}
