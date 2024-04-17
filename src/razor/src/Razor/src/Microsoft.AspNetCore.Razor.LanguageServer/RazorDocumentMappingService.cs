// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal sealed class RazorDocumentMappingService(
        IFilePathService filePathService,
        IDocumentContextFactory documentContextFactory,
        IRazorLoggerFactory loggerFactory)
         : AbstractRazorDocumentMappingService(filePathService, documentContextFactory, loggerFactory.CreateLogger<RazorDocumentMappingService>())
{
}
