// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Features;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.VisualStudioCode.RazorExtension.Services;

[ExportRazorLspServiceFactory(typeof(RazorLspDynamicFileInfoProvider)), Shared]
[method: ImportingConstructor]
internal sealed class DynamicFileProviderFactory(
    LanguageServerFeatureOptions featureOptions,
    VSCodeWorkspaceProvider workspaceProvider) : AbstractRazorLspServiceFactory
{
    private readonly LanguageServerFeatureOptions _featureOptions = featureOptions;
    private readonly VSCodeWorkspaceProvider _workspaceProvider = workspaceProvider;

    protected override AbstractRazorLspService CreateService(IRazorLspServices lspServices)
    {
        var clientLanguageServerManager = lspServices.GetRequiredService<IRazorClientLanguageServerManager>();
        return new LspDynamicFileProvider(clientLanguageServerManager, _featureOptions, _workspaceProvider);
    }
}

