// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Remote.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Roslyn.LanguageServer.Protocol;
using static Microsoft.CodeAnalysis.Razor.Remote.RemoteResponse<Roslyn.LanguageServer.Protocol.SumType<Roslyn.LanguageServer.Protocol.VSInternalReferenceItem, Roslyn.LanguageServer.Protocol.Location>[]?>;
using ExternalHandlers = Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost.Handlers;
using LspLocation = Roslyn.LanguageServer.Protocol.Location;
using VsLspExtensions = Microsoft.VisualStudio.LanguageServer.Protocol.VsLspExtensions;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class RemoteFindAllReferencesService(in ServiceArgs args) : RazorDocumentServiceBase(in args), IRemoteFindAllReferencesService
{
    internal sealed class Factory : FactoryBase<IRemoteFindAllReferencesService>
    {
        protected override IRemoteFindAllReferencesService CreateService(in ServiceArgs args)
            => new RemoteFindAllReferencesService(in args);
    }

    private readonly IClientCapabilitiesService _clientCapabilitiesService = args.ExportProvider.GetExportedValue<IClientCapabilitiesService>();

    protected override IDocumentPositionInfoStrategy DocumentPositionInfoStrategy => PreferAttributeNameDocumentPositionInfoStrategy.Instance;

    public ValueTask<RemoteResponse<SumType<VSInternalReferenceItem, LspLocation>[]?>> FindAllReferencesAsync(
        JsonSerializableRazorPinnedSolutionInfoWrapper solutionInfo,
        JsonSerializableDocumentId documentId,
        Position position,
        CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
            documentId,
            context => GetImplementationAsync(context, position, cancellationToken),
            cancellationToken);

    private async ValueTask<RemoteResponse<SumType<VSInternalReferenceItem, LspLocation>[]?>> GetImplementationAsync(
        RemoteDocumentContext context,
        Position position,
        CancellationToken cancellationToken)
    {
        var codeDocument = await context.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        if (!codeDocument.Source.Text.TryGetAbsoluteIndex(position, out var hostDocumentIndex))
        {
            return NoFurtherHandling;
        }

        var positionInfo = GetPositionInfo(codeDocument, hostDocumentIndex, preferCSharpOverHtml: true);

        if (positionInfo.LanguageKind is not RazorLanguageKind.CSharp)
        {
            return NoFurtherHandling;
        }

        // Finally, call into C#.
        var generatedDocument = await context.Snapshot
            .GetGeneratedDocumentAsync(cancellationToken)
            .ConfigureAwait(false);

        var results = await ExternalHandlers.FindAllReferences
            .FindReferencesAsync(
                RemoteWorkspaceAccessor.GetWorkspace(),
                generatedDocument,
                VsLspExtensions.ToLinePosition(positionInfo.Position),
                _clientCapabilitiesService.ClientCapabilities.SupportsVisualStudioExtensions,
                cancellationToken)
            .ConfigureAwait(false);

        if (results is null and not [])
        {
            // C# didn't return anything, so we're done.
            return NoFurtherHandling;
        }

        // Map the C# locations back to the Razor file.
        foreach (var result in results)
        {
            var location = result.TryGetFirst(out var referenceItem)
                ? referenceItem.Location
                : result.Second;

            if (location is null)
            {
                continue;
            }

            var (mappedUri, mappedRange) = await DocumentMappingService.MapToHostDocumentUriAndRangeAsync(context.Snapshot, location.Uri, location.Range.ToLinePositionSpan(), cancellationToken).ConfigureAwait(false);

            if (referenceItem is not null)
            {
                // Indicates the reference item is directly available in the code
                referenceItem.Origin = VSInternalItemOrigin.Exact;

                // If we're going to change the Uri, then also override the file paths
                if (mappedUri != location.Uri)
                {
                    referenceItem.DisplayPath = mappedUri.AbsolutePath;
                    referenceItem.DocumentName = mappedUri.AbsolutePath;
                }
            }

            location.Uri = mappedUri;
            location.Range = mappedRange.ToRange();
        }

        return Results(results);
    }
}
