// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Remote.Razor.Formatting;

[Export(typeof(IRazorFormattingService)), Shared]
[method: ImportingConstructor]
internal sealed class RemoteRazorFormattingService(IFormattingCodeDocumentProvider codeDocumentProvider, IDocumentMappingService documentMappingService, IHostServicesProvider hostServicesProvider, ILoggerFactory loggerFactory)
    : RazorFormattingService(codeDocumentProvider, documentMappingService, hostServicesProvider, loggerFactory)
{
}
