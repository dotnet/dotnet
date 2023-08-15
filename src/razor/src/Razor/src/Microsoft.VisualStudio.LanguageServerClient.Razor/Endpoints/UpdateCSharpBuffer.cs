// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Extensions;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

internal partial class RazorCustomMessageTarget
{
    // Called by the Razor Language Server to update the contents of the virtual CSharp buffer.
    [JsonRpcMethod(CustomMessageNames.RazorUpdateCSharpBufferEndpoint, UseSingleObjectParameterDeserialization = true)]
    public async Task UpdateCSharpBufferAsync(UpdateBufferRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        await _joinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        UpdateCSharpBuffer(request);
    }

    // Internal for testing
    internal void UpdateCSharpBuffer(UpdateBufferRequest request)
    {
        if (request is null || request.HostDocumentFilePath is null || request.HostDocumentVersion is null)
        {
            return;
        }

        var hostDocumentUri = new Uri(request.HostDocumentFilePath);

        if (_documentManager.TryGetDocument(hostDocumentUri, out var documentSnapshot) &&
            documentSnapshot.TryGetAllVirtualDocuments<CSharpVirtualDocumentSnapshot>(out var virtualDocuments))
        {
            foreach (var virtualDocument in virtualDocuments)
            {
                // TODO: Remove the null check from this line when multiple CSharpVirtualDocuments are actually being created
                if (virtualDocument.ProjectKey.Id == request.ProjectKeyId || virtualDocument.ProjectKey.Id is null)
                {
                    _documentManager.UpdateVirtualDocument<CSharpVirtualDocument>(
                        hostDocumentUri,
                        virtualDocument.Uri,
                        request.Changes.Select(change => change.ToVisualStudioTextChange()).ToArray(),
                        request.HostDocumentVersion.Value,
                        state: null);
                    return;
                }
            }

            // If the particular document supports multiple virtual documents, we don't want to try to update a single one
            Debug.Fail("Multiple virtual documents seem to be supported, but none were updated.");
            return;
        }

        _documentManager.UpdateVirtualDocument<CSharpVirtualDocument>(
            hostDocumentUri,
            request.Changes.Select(change => change.ToVisualStudioTextChange()).ToArray(),
            request.HostDocumentVersion.Value,
            state: null);
    }
}
