// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Composition;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.FoldingRanges;
using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.CodeAnalysis.Remote.Razor.FoldingRanges;

[Export(typeof(IFoldingRangeService)), Shared]
[method: ImportingConstructor]
internal sealed class OOPFoldingRangeService(
    IRazorDocumentMappingService documentMappingService,
    [ImportMany] IEnumerable<IRazorFoldingRangeProvider> foldingRangeProviders,
    ILoggerFactory loggerFactory)
    : FoldingRangeService(documentMappingService, foldingRangeProviders, loggerFactory)
{
}
