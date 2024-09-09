// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Remote.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Roslyn.LanguageServer.Protocol;
using static Microsoft.CodeAnalysis.Razor.Remote.RemoteResponse<Roslyn.LanguageServer.Protocol.Location[]?>;
using ExternalHandlers = Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost.Handlers;
using RoslynLocation = Roslyn.LanguageServer.Protocol.Location;
using RoslynPosition = Roslyn.LanguageServer.Protocol.Position;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class RemoteGoToImplementationService(in ServiceArgs args) : RazorDocumentServiceBase(in args), IRemoteGoToImplementationService
{
    internal sealed class Factory : FactoryBase<IRemoteGoToImplementationService>
    {
        protected override IRemoteGoToImplementationService CreateService(in ServiceArgs args)
            => new RemoteGoToImplementationService(in args);
    }

    protected override IDocumentPositionInfoStrategy DocumentPositionInfoStrategy => PreferAttributeNameDocumentPositionInfoStrategy.Instance;

    public ValueTask<RemoteResponse<RoslynLocation[]?>> GetImplementationAsync(
        JsonSerializableRazorPinnedSolutionInfoWrapper solutionInfo,
        JsonSerializableDocumentId documentId,
        RoslynPosition position,
        CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
            documentId,
            context => GetImplementationAsync(context, position, cancellationToken),
            cancellationToken);

    private async ValueTask<RemoteResponse<RoslynLocation[]?>> GetImplementationAsync(
        RemoteDocumentContext context,
        RoslynPosition position,
        CancellationToken cancellationToken)
    {
        var codeDocument = await context.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        if (!codeDocument.Source.Text.TryGetAbsoluteIndex(position, out var hostDocumentIndex))
        {
            return NoFurtherHandling;
        }

        var positionInfo = GetPositionInfo(codeDocument, hostDocumentIndex, preferCSharpOverHtml: true);

        if (positionInfo.LanguageKind is RazorLanguageKind.Razor)
        {
            return NoFurtherHandling;
        }

        if (positionInfo.LanguageKind is RazorLanguageKind.Html)
        {
            return CallHtml;
        }

        if (!DocumentMappingService.TryMapToGeneratedDocumentPosition(codeDocument.GetCSharpDocument(), positionInfo.HostDocumentIndex, out var mappedPosition, out _))
        {
            // If we can't map to the generated C# file, we're done.
            return NoFurtherHandling;
        }

        // Finally, call into C#.
        var generatedDocument = await context.Snapshot.GetGeneratedDocumentAsync().ConfigureAwait(false);

        var locations = await ExternalHandlers.GoToImplementation
            .FindImplementationsAsync(
                generatedDocument,
                mappedPosition,
                supportsVisualStudioExtensions: true,
                cancellationToken)
            .ConfigureAwait(false);

        if (locations is null and not [])
        {
            // C# didn't return anything, so we're done.
            return NoFurtherHandling;
        }

        // Map the C# locations back to the Razor file.
        using var mappedLocations = new PooledArrayBuilder<RoslynLocation>(locations.Length);

        foreach (var location in locations)
        {
            var (uri, range) = location;

            var (mappedDocumentUri, mappedRange) = await DocumentMappingService
                .MapToHostDocumentUriAndRangeAsync(context.Snapshot, uri, range.ToLinePositionSpan(), cancellationToken)
                .ConfigureAwait(false);

            var mappedLocation = RoslynLspFactory.CreateLocation(mappedDocumentUri, mappedRange);

            mappedLocations.Add(mappedLocation);
        }

        return Results(mappedLocations.ToArray());
    }
}
