// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Features;

namespace Microsoft.VisualStudioCode.RazorExtension.Services;

[ExportRazorLspServiceFactory(typeof(RazorLspDynamicFileInfoProvider)), Shared]
internal sealed class DynamicFileProviderFactory : AbstractRazorLspServiceFactory
{
    protected override AbstractRazorLspService CreateService(IRazorLspServices lspServices)
    {
        var clientLanguageServerManager = lspServices.GetRequiredService<IRazorClientLanguageServerManager>();
        return new LspDynamicFileProvider(clientLanguageServerManager);
    }
}

