// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Rename;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using ExternalHandlers = Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost.Handlers;
using static Microsoft.CodeAnalysis.Razor.Remote.RemoteResponse<Microsoft.VisualStudio.LanguageServer.Protocol.WorkspaceEdit?>;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class RemoteRenameService(in ServiceArgs args) : RazorDocumentServiceBase(in args), IRemoteRenameService
{
    internal sealed class Factory : FactoryBase<IRemoteRenameService>
    {
        protected override IRemoteRenameService CreateService(in ServiceArgs args)
            => new RemoteRenameService(in args);
    }

    private readonly IRenameService _renameService = args.ExportProvider.GetExportedValue<IRenameService>();
    private readonly IFilePathService _filePathService = args.ExportProvider.GetExportedValue<IFilePathService>();
    private readonly IDocumentMappingService _documentMappingService = args.ExportProvider.GetExportedValue<IDocumentMappingService>();
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
        var generatedDocument = await context.GetGeneratedDocumentAsync(_filePathService, cancellationToken).ConfigureAwait(false);

        var hostDocumentIndex = codeDocument.Source.Text.GetRequiredAbsoluteIndex(position);
        var positionInfo = _documentMappingService.GetPositionInfo(codeDocument, hostDocumentIndex);

        var razorEdit = await _renameService.TryGetRazorRenameEditsAsync(context, positionInfo, newName, cancellationToken).ConfigureAwait(false);
        if (razorEdit is not null)
        {
            return Results(razorEdit);
        }

        if (positionInfo.LanguageKind != CodeAnalysis.Razor.Protocol.RazorLanguageKind.CSharp)
        {
            return CallHtml;
        }

        var csharpEdit = await ExternalHandlers.Rename.GetRenameEditAsync(generatedDocument, positionInfo.Position.ToLinePosition(), newName, cancellationToken).ConfigureAwait(false);
        if (csharpEdit is null)
        {
            return NoFurtherHandling;
        }

        // This is, to say the least, not ideal. In future we're going to normalize on to Roslyn LSP types, and this can go.
        var vsEdit = JsonSerializer.Deserialize<WorkspaceEdit>(JsonSerializer.SerializeToDocument(csharpEdit));
        if (vsEdit is null)
        {
            return NoFurtherHandling;
        }

        var mappedEdit = await _editMappingService.RemapWorkspaceEditAsync(context.Snapshot, vsEdit, cancellationToken).ConfigureAwait(false);
        return Results(mappedEdit);
    }
}
