// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Rename;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using static Microsoft.CodeAnalysis.Razor.Remote.RemoteResponse<Roslyn.LanguageServer.Protocol.WorkspaceEdit?>;
using ExternalHandlers = Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost.Handlers;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class RemoteRenameService(in ServiceArgs args) : RazorDocumentServiceBase(in args), IRemoteRenameService
{
    internal sealed class Factory : FactoryBase<IRemoteRenameService>
    {
        protected override IRemoteRenameService CreateService(in ServiceArgs args)
            => new RemoteRenameService(in args);
    }

    private readonly IRenameService _renameService = args.ExportProvider.GetExportedValue<IRenameService>();
    private readonly IEditMappingService _editMappingService = args.ExportProvider.GetExportedValue<IEditMappingService>();

    public ValueTask<RemoteResponse<WorkspaceEdit?>> GetRenameEditAsync(
        JsonSerializableRazorPinnedSolutionInfoWrapper solutionInfo,
        JsonSerializableDocumentId documentId,
        Position position,
        string newName,
        CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
            documentId,
            context => GetRenameEditAsync(context, position, newName, cancellationToken),
            cancellationToken);

    private async ValueTask<RemoteResponse<WorkspaceEdit?>> GetRenameEditAsync(
        RemoteDocumentContext context,
        Position position,
        string newName,
        CancellationToken cancellationToken)
    {
        var codeDocument = await context.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        if (!TryGetDocumentPositionInfo(codeDocument, position, out var positionInfo))
        {
            return NoFurtherHandling;
        }

        var generatedDocument = await context.Snapshot
            .GetGeneratedDocumentAsync(cancellationToken)
            .ConfigureAwait(false);

        var razorEdit = await _renameService
            .TryGetRazorRenameEditsAsync(context, positionInfo, newName, context.GetSolutionQueryOperations(), cancellationToken)
            .ConfigureAwait(false);

        if (razorEdit is not null)
        {
            return Results(razorEdit);
        }

        if (positionInfo.LanguageKind != CodeAnalysis.Razor.Protocol.RazorLanguageKind.CSharp)
        {
            return CallHtml;
        }

        var csharpEdit = await ExternalHandlers.Rename
            .GetRenameEditAsync(generatedDocument, positionInfo.Position.ToLinePosition(), newName, cancellationToken)
            .ConfigureAwait(false);

        if (csharpEdit is null)
        {
            return NoFurtherHandling;
        }

        var mappedEdit = await _editMappingService.RemapWorkspaceEditAsync(context.Snapshot, csharpEdit, cancellationToken).ConfigureAwait(false);
        return Results(mappedEdit);
    }
}
