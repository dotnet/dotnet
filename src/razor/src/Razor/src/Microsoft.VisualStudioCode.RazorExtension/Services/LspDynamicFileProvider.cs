// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Features;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.VisualStudioCode.RazorExtension.Services;

internal sealed partial class LspDynamicFileProvider(
    IRazorClientLanguageServerManager clientLanguageServerManager,
    LanguageServerFeatureOptions languageServerFeatureOptions,
    VSCodeWorkspaceProvider workspaceProvider) : RazorLspDynamicFileInfoProvider
{
    private const string ProvideRazorDynamicFileInfoMethodName = "razor/provideDynamicFileInfo";
    private const string RemoveRazorDynamicFileInfoMethodName = "razor/removeDynamicFileInfo";

    private readonly IRazorClientLanguageServerManager _clientLanguageServerManager = clientLanguageServerManager;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions;
    private readonly VSCodeWorkspaceProvider _workspaceProvider = workspaceProvider;

    public override async Task<RazorDynamicFileInfo?> GetDynamicFileInfoAsync(Workspace workspace, ProjectId projectId, string? projectFilePath, string filePath, CancellationToken cancellationToken)
    {
        // TODO: Temporarily using this as a hook to get the workspace into cohosting. In future when we delete the IDynamicFileInfo
        // system as a whole, we'll need some other hook to get to the LspWorkspace
        _workspaceProvider.SetWorkspace(workspace);

        if (_languageServerFeatureOptions.UseRazorCohostServer)
        {
            return null;
        }

        var razorUri = new Uri(filePath);

        var requestParams = new RazorProvideDynamicFileParams
        {
            RazorDocument = new()
            {
                DocumentUri = new(razorUri)
            }
        };

        var response = await _clientLanguageServerManager.SendRequestAsync<RazorProvideDynamicFileParams, RazorProvideDynamicFileResponse?>(
            ProvideRazorDynamicFileInfoMethodName,
            requestParams,
            cancellationToken).ConfigureAwait(false);

        if (response is null)
        {
            return null;
        }

        var textDocument = await WorkspaceExtensions.GetTextDocumentAsync(workspace, response.CSharpDocument.DocumentUri, cancellationToken).ConfigureAwait(false);
        var checksum = Convert.FromBase64String(response.Checksum);
        var textLoader = new LspTextChangesTextLoader(
            textDocument,
            response.Edits,
            checksum,
            response.ChecksumAlgorithm,
            response.SourceEncodingCodePage,
            razorUri,
            _clientLanguageServerManager);

        return new RazorDynamicFileInfo(
            RazorUri.GetDocumentFilePathFromUri(response.CSharpDocument.DocumentUri.GetRequiredParsedUri()),
            SourceCodeKind.Regular,
            textLoader,
            documentServiceProvider: new LspDocumentServiceProvider(_clientLanguageServerManager));
    }

    public override Task RemoveDynamicFileInfoAsync(Workspace workspace, ProjectId projectId, string? projectFilePath, string filePath, CancellationToken cancellationToken)
    {
        var notificationParams = new RazorRemoveDynamicFileParams
        {
            CSharpDocument = new()
            {
                DocumentUri = new(new Uri(filePath))
            }
        };
        return _clientLanguageServerManager.SendNotificationAsync(
            RemoveRazorDynamicFileInfoMethodName, notificationParams, cancellationToken).AsTask();
    }
}

