// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Protocol.DocumentSymbols;

namespace Microsoft.CodeAnalysis.Remote.Razor.DocumentSymbols;

[Export(typeof(IDocumentSymbolService)), Shared]
[method: ImportingConstructor]
internal class OOPDocumentSymbolService(IDocumentMappingService documentMappingService) : DocumentSymbolService(documentMappingService)
{
}
