// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Rename;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Refactoring;

[RazorLanguageServerEndpoint(Methods.TextDocumentRenameName)]
internal sealed class RenameEndpoint(
    IRenameService renameService,
    LanguageServerFeatureOptions languageServerFeatureOptions,
    IDocumentMappingService documentMappingService,
    IEditMappingService editMappingService,
    ProjectSnapshotManager projectManager,
    IClientConnection clientConnection,
    ILoggerFactory loggerFactory)
    : AbstractRazorDelegatingEndpoint<RenameParams, WorkspaceEdit?>(
        languageServerFeatureOptions,
        documentMappingService,
        clientConnection,
        loggerFactory.GetOrCreateLogger<RenameEndpoint>()), ICapabilitiesProvider
{
    private readonly IRenameService _renameService = renameService;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions;
    private readonly IEditMappingService _editMappingService = editMappingService;
    private readonly ProjectSnapshotManager _projectManager = projectManager;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.RenameProvider = new RenameOptions
        {
            PrepareProvider = false,
        };
    }

    protected override bool PreferCSharpOverHtmlIfPossible => true;

    protected override string CustomMessageTarget => CustomMessageNames.RazorRenameEndpointName;

    protected override Task<WorkspaceEdit?> TryHandleAsync(RenameParams request, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            return SpecializedTasks.Null<WorkspaceEdit>();
        }

        return _renameService.TryGetRazorRenameEditsAsync(documentContext, positionInfo, request.NewName, _projectManager.GetQueryOperations(), cancellationToken);
    }

    protected override bool IsSupported()
        => _languageServerFeatureOptions.SupportsFileManipulation;

    protected override Task<IDelegatedParams?> CreateDelegatedParamsAsync(RenameParams request, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            return SpecializedTasks.Null<IDelegatedParams>();
        }

        return Task.FromResult<IDelegatedParams?>(new DelegatedRenameParams(
            documentContext.GetTextDocumentIdentifierAndVersion(),
            positionInfo.Position,
            positionInfo.LanguageKind,
            request.NewName));
    }

    protected override async Task<WorkspaceEdit?> HandleDelegatedResponseAsync(WorkspaceEdit? response, RenameParams request, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        if (response is null)
        {
            return null;
        }

        var documentContext = requestContext.DocumentContext.AssumeNotNull();
        return await _editMappingService.RemapWorkspaceEditAsync(documentContext.Snapshot, response, cancellationToken).ConfigureAwait(false);
    }
}
