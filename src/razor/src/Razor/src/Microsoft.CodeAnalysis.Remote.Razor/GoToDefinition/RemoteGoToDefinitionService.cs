// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.GoToDefinition;
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

internal sealed class RemoteGoToDefinitionService(in ServiceArgs args) : RazorDocumentServiceBase(in args), IRemoteGoToDefinitionService
{
    internal sealed class Factory : FactoryBase<IRemoteGoToDefinitionService>
    {
        protected override IRemoteGoToDefinitionService CreateService(in ServiceArgs args)
            => new RemoteGoToDefinitionService(in args);
    }

    private readonly IRazorComponentDefinitionService _componentDefinitionService = args.ExportProvider.GetExportedValue<IRazorComponentDefinitionService>();

    protected override IDocumentPositionInfoStrategy DocumentPositionInfoStrategy => PreferAttributeNameDocumentPositionInfoStrategy.Instance;

    public ValueTask<RemoteResponse<RoslynLocation[]?>> GetDefinitionAsync(
        JsonSerializableRazorPinnedSolutionInfoWrapper solutionInfo,
        JsonSerializableDocumentId documentId,
        RoslynPosition position,
        CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
            documentId,
            context => GetDefinitionAsync(context, position, cancellationToken),
            cancellationToken);

    private async ValueTask<RemoteResponse<RoslynLocation[]?>> GetDefinitionAsync(
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

        if (positionInfo.LanguageKind is RazorLanguageKind.Html or RazorLanguageKind.Razor)
        {
            // First, see if this is a Razor component. We ignore attributes here, because they're better served by the C# handler.
            var componentLocation = await _componentDefinitionService
                .GetDefinitionAsync(context.Snapshot, positionInfo, context.GetSolutionQueryOperations(), ignoreAttributes: true, cancellationToken)
                .ConfigureAwait(false);

            if (componentLocation is not null)
            {
                // Convert from VS LSP Location to Roslyn. This can be removed when Razor moves fully onto Roslyn's LSP types.
                return Results([RoslynLspFactory.CreateLocation(componentLocation.Uri, componentLocation.Range.ToLinePositionSpan())]);
            }

            // If it isn't a Razor component, and it isn't C#, let the server know to delegate to HTML.
            return CallHtml;
        }

        // Finally, call into C#.
        var generatedDocument = await context.Snapshot
            .GetGeneratedDocumentAsync(cancellationToken)
            .ConfigureAwait(false);

        var locations = await ExternalHandlers.GoToDefinition
            .GetDefinitionsAsync(
                RemoteWorkspaceAccessor.GetWorkspace(),
                generatedDocument,
                typeOnly: false,
                positionInfo.Position.ToLinePosition(),
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
