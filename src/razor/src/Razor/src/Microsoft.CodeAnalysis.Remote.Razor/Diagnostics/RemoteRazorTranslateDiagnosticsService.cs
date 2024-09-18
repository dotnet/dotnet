// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.Razor.Diagnostics;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.CodeAnalysis.Remote.Razor.Diagnostics;

[Export(typeof(RazorTranslateDiagnosticsService)), Shared]
[method: ImportingConstructor]
internal sealed class RemoteRazorTranslateDiagnosticsService(
    IDocumentMappingService documentMappingService,
    ILoggerFactory loggerFactory) : RazorTranslateDiagnosticsService(documentMappingService, loggerFactory)
{
}
