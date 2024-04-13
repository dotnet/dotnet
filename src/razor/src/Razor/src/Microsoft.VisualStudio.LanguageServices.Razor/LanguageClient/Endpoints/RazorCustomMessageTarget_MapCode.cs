// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Razor.LanguageClient.Extensions;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Endpoints;

internal partial class RazorCustomMessageTarget
{
    // Called by the Razor Language Server to provide code actions from the platform.
    [JsonRpcMethod(CustomMessageNames.RazorMapCodeEndpoint, UseSingleObjectParameterDeserialization = true)]
    public async Task<WorkspaceEdit?> MapCodeAsync(DelegatedMapCodeParams request, CancellationToken cancellationToken)
    {
        var delegationDetails = await GetProjectedRequestDetailsAsync(request, cancellationToken).ConfigureAwait(false);
        if (delegationDetails is null)
        {
            return null;
        }

        ConvertCSharpFocusLocationUris(request.FocusLocations);

        var mappings = new VSInternalMapCodeMapping()
        {
            TextDocument = request.Identifier.TextDocumentIdentifier.WithUri(delegationDetails.Value.ProjectedUri),
            Contents = request.Contents,
            FocusLocations = request.FocusLocations,
        };

        var mapCodeParams = new VSInternalMapCodeParams()
        {
            Mappings = [mappings],
            MapCodeCorrelationId = request.MapCodeCorrelationId,
        };

        var textBuffer = delegationDetails.Value.TextBuffer;
        var lspMethodName = VSInternalMethods.WorkspaceMapCodeName;
        var languageServerName = delegationDetails.Value.LanguageServerName;
        using var _ = _telemetryReporter.TrackLspRequest(lspMethodName, languageServerName, request.MapCodeCorrelationId);

        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<VSInternalMapCodeParams, WorkspaceEdit?>(
            textBuffer,
            lspMethodName,
            languageServerName,
            SupportsMapCode,
            mapCodeParams,
            cancellationToken).ConfigureAwait(false);

        return response?.Response;
    }

    private void ConvertCSharpFocusLocationUris(Location[][] focusLocations)
    {
        // Focus locations should be in a C# context. Map from Razor URI -> C# URI.
        foreach (var locationsByPriority in focusLocations)
        {
            foreach (var location in locationsByPriority)
            {
                if (location is null)
                {
                    continue;
                }

                if (!_documentManager.TryGetDocument(location.Uri, out var documentSnapshot))
                {
                    continue;
                }

                if (!documentSnapshot.TryGetVirtualDocument<CSharpVirtualDocumentSnapshot>(out var virtualDocument))
                {
                    continue;
                }

                location.Uri = virtualDocument.Uri;
            }
        }
    }

    private static bool SupportsMapCode(JToken token)
    {
        return token is JObject obj
            && obj.TryGetValue(VSInternalMethods.WorkspaceMapCodeName, out var supportsMapCode)
            && supportsMapCode.ToObject<bool>();
    }
}
