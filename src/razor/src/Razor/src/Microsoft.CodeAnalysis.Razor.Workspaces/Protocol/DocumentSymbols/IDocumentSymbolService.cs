// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.Protocol.DocumentSymbols;

internal interface IDocumentSymbolService
{
    SumType<DocumentSymbol[], SymbolInformation[]>? GetDocumentSymbols(Uri razorDocumentUri, RazorCSharpDocument csharpDocument, SumType<DocumentSymbol[], SymbolInformation[]> csharpSymbols);
}
