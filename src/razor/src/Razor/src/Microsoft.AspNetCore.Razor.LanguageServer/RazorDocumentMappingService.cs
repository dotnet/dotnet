// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal sealed class RazorDocumentMappingService(
        IFilePathService filePathService,
        IDocumentContextFactory documentContextFactory,
        ILoggerFactory loggerFactory)
         : AbstractRazorDocumentMappingService(filePathService, documentContextFactory, loggerFactory.GetOrCreateLogger<RazorDocumentMappingService>())
{
}
