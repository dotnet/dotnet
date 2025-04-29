// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.FindAllReferences;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Text.Adornments;

namespace Microsoft.AspNetCore.Razor.LanguageServer.FindAllReferences;

[RazorLanguageServerEndpoint(Methods.TextDocumentReferencesName)]
internal sealed class FindAllReferencesEndpoint : AbstractRazorDelegatingEndpoint<ReferenceParams, VSInternalReferenceItem[]?>, ICapabilitiesProvider
{
    private readonly IFilePathService _filePathService;
    private readonly ProjectSnapshotManager _projectSnapshotManager;
    private readonly IDocumentMappingService _documentMappingService;

    public FindAllReferencesEndpoint(
        LanguageServerFeatureOptions languageServerFeatureOptions,
        IDocumentMappingService documentMappingService,
        IClientConnection clientConnection,
        ILoggerFactory loggerFactory,
        IFilePathService filePathService,
        ProjectSnapshotManager projectSnapshotManager)
        : base(languageServerFeatureOptions, documentMappingService, clientConnection, loggerFactory.GetOrCreateLogger<FindAllReferencesEndpoint>())
    {
        _filePathService = filePathService ?? throw new ArgumentNullException(nameof(filePathService));
        _projectSnapshotManager = projectSnapshotManager;
        _documentMappingService = documentMappingService ?? throw new ArgumentNullException(nameof(documentMappingService));
    }

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.ReferencesProvider = new ReferenceOptions()
        {
            // https://github.com/dotnet/razor/issues/8033
            WorkDoneProgress = false,
        };
    }

    protected override string CustomMessageTarget => CustomMessageNames.RazorReferencesEndpointName;

    protected override bool PreferCSharpOverHtmlIfPossible => true;

    protected override IDocumentPositionInfoStrategy DocumentPositionInfoStrategy => PreferAttributeNameDocumentPositionInfoStrategy.Instance;

    protected override Task<IDelegatedParams?> CreateDelegatedParamsAsync(ReferenceParams request, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        // HTML doesn't need to do FAR
        if (positionInfo.LanguageKind != RazorLanguageKind.CSharp)
        {
            return SpecializedTasks.Null<IDelegatedParams>();
        }

        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            return SpecializedTasks.Null<IDelegatedParams>();
        }

        return Task.FromResult<IDelegatedParams?>(new DelegatedPositionParams(
            documentContext.GetTextDocumentIdentifierAndVersion(),
            positionInfo.Position,
            positionInfo.LanguageKind));
    }

    protected override async Task<VSInternalReferenceItem[]?> HandleDelegatedResponseAsync(
        VSInternalReferenceItem[]? delegatedResponse,
        ReferenceParams originalRequest,
        RazorRequestContext requestContext,
        DocumentPositionInfo positionInfo,
        CancellationToken cancellationToken)
    {
        if (delegatedResponse is null)
        {
            return null;
        }

        using var remappedLocations = new PooledArrayBuilder<VSInternalReferenceItem>();

        foreach (var referenceItem in delegatedResponse)
        {
            if (referenceItem?.Location is null || referenceItem.Text is null)
            {
                continue;
            }

            // Indicates the reference item is directly available in the code
            referenceItem.Origin = VSInternalItemOrigin.Exact;

            if (!_filePathService.IsVirtualCSharpFile(referenceItem.Location.Uri) &&
                !_filePathService.IsVirtualHtmlFile(referenceItem.Location.Uri))
            {
                // This location doesn't point to a virtual file. No need to remap, but we might still want to fix the text,
                // because Roslyn may have done the remapping for us
                var resultText = await FindAllReferencesHelper.GetResultTextAsync(_documentMappingService, _projectSnapshotManager.GetQueryOperations(), referenceItem.Location.Range.Start.Line, referenceItem.Location.Uri.GetAbsoluteOrUNCPath(), cancellationToken).ConfigureAwait(false);
                referenceItem.Text = resultText ?? referenceItem.Text;

                remappedLocations.Add(referenceItem);
                continue;
            }

            var (itemUri, mappedRange) = await _documentMappingService.MapToHostDocumentUriAndRangeAsync(referenceItem.Location.Uri, referenceItem.Location.Range, cancellationToken).ConfigureAwait(false);

            referenceItem.Location.Uri = itemUri;
            referenceItem.DisplayPath = itemUri.AbsolutePath;
            referenceItem.Location.Range = mappedRange;

            var fixedResultText = await FindAllReferencesHelper.GetResultTextAsync(_documentMappingService, _projectSnapshotManager.GetQueryOperations(), mappedRange.Start.Line, itemUri.GetAbsoluteOrUNCPath(), cancellationToken).ConfigureAwait(false);
            referenceItem.Text = fixedResultText ?? referenceItem.Text;

            remappedLocations.Add(referenceItem);
        }

        return remappedLocations.ToArray();
    }
}
