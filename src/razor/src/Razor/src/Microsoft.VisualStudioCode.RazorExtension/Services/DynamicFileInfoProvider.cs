// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Features;

namespace Microsoft.VisualStudioCode.RazorExtension.Services;

internal sealed partial class LspDynamicFileProvider(IRazorClientLanguageServerManager clientLanguageServerManager) : RazorLspDynamicFileInfoProvider
{
    private const string ProvideRazorDynamicFileInfoMethodName = "razor/provideDynamicFileInfo";
    private const string RemoveRazorDynamicFileInfoMethodName = "razor/removeDynamicFileInfo";

    private readonly IRazorClientLanguageServerManager _clientLanguageServerManager = clientLanguageServerManager;

    public override async Task<RazorDynamicFileInfo?> GetDynamicFileInfoAsync(Workspace workspace, ProjectId projectId, string? projectFilePath, string filePath, CancellationToken cancellationToken)
    {
        var razorUri = new Uri(filePath);

        var requestParams = new RazorProvideDynamicFileParams
        {
            RazorDocument = new()
            {
                Uri = razorUri
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

        var textDocument = await WorkspaceExtensions.GetTextDocumentAsync(workspace, response.CSharpDocument.Uri, cancellationToken).ConfigureAwait(false);
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
            response.CSharpDocument.Uri.ToString(),
            SourceCodeKind.Regular,
            textLoader,
            documentServiceProvider: EmptyServiceProvider.Instance);
    }

    public override Task RemoveDynamicFileInfoAsync(Workspace workspace, ProjectId projectId, string? projectFilePath, string filePath, CancellationToken cancellationToken)
    {
        var notificationParams = new RazorRemoveDynamicFileParams
        {
            CSharpDocument = new()
            {
                Uri = new Uri(filePath)
            }
        };
        return _clientLanguageServerManager.SendNotificationAsync(
            RemoveRazorDynamicFileInfoMethodName, notificationParams, cancellationToken).AsTask();
    }
}

