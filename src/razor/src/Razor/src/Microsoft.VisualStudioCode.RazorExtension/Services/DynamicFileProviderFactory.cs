// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Composition;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Features;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.VisualStudioCode.RazorExtension.Services;

[ExportRazorLspServiceFactory(typeof(RazorLspDynamicFileInfoProvider)), Shared]
[method: ImportingConstructor]
internal sealed class DynamicFileProviderFactory(
    LanguageServerFeatureOptions featureOptions) : AbstractRazorLspServiceFactory
{
    private readonly LanguageServerFeatureOptions _featureOptions = featureOptions;

    protected override AbstractRazorLspService CreateService(IRazorLspServices lspServices)
    {
        var clientLanguageServerManager = lspServices.GetRequiredService<IRazorClientLanguageServerManager>();
        return new LspDynamicFileProvider(clientLanguageServerManager, _featureOptions);
    }
}

