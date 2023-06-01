// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal interface IDocumentPositionInfoStrategy
{
    Task<DocumentPositionInfo?> TryGetPositionInfoAsync(IRazorDocumentMappingService documentMappingService, DocumentContext documentContext, Position position, ILogger logger, CancellationToken cancellationToken);
}
