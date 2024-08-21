// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Remote;

internal interface IRemoteDocumentSymbolService : IRemoteJsonService
{
    ValueTask<SumType<DocumentSymbol[], SymbolInformation[]>?> GetDocumentSymbolsAsync(JsonSerializableRazorPinnedSolutionInfoWrapper solutionInfo, JsonSerializableDocumentId documentId, bool useHierarchicalSymbols, CancellationToken cancellationToken);
}
