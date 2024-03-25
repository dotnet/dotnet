// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Remote.Razor.DocumentMapping;

[Export(typeof(IRazorDocumentMappingService)), Shared]
[method: ImportingConstructor]
internal sealed class RemoteDocumentMappingService(
    IFilePathService filePathService,
    IDocumentContextFactory documentContextFactory,
    IRazorLoggerFactory loggerFactory)
    : AbstractRazorDocumentMappingService(
        filePathService,
        documentContextFactory,
        loggerFactory.CreateLogger<RemoteDocumentMappingService>())
{
}
