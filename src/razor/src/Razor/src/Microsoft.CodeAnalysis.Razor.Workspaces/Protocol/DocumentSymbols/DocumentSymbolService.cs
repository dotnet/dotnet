// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;

namespace Microsoft.CodeAnalysis.Razor.Protocol.DocumentSymbols;

internal class DocumentSymbolService(IDocumentMappingService documentMappingService) : IDocumentSymbolService
{
    private readonly IDocumentMappingService _documentMappingService = documentMappingService;

    public SumType<DocumentSymbol[], SymbolInformation[]>? GetDocumentSymbols(Uri razorDocumentUri, RazorCSharpDocument csharpDocument, SumType<DocumentSymbol[], SymbolInformation[]> csharpSymbols)
    {
        if (csharpSymbols.TryGetFirst(out var documentSymbols))
        {
            return RemapDocumentSymbols(csharpDocument, documentSymbols);
        }
        else if (csharpSymbols.TryGetSecond(out var symbolInformations))
        {
            using var _ = ListPool<SymbolInformation>.GetPooledObject(out var mappedSymbols);

            foreach (var symbolInformation in symbolInformations)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                // SymbolInformation is obsolete, but things still return it so we have to handle it
                if (_documentMappingService.TryMapToRazorDocumentRange(csharpDocument, symbolInformation.Location.Range, out var newRange))
                {
                    symbolInformation.Location.Range = newRange;
                    symbolInformation.Location.Uri = razorDocumentUri;
                    mappedSymbols.Add(symbolInformation);
                }
#pragma warning restore CS0618 // Type or member is obsolete
            }

            return mappedSymbols.ToArray();
        }
        else
        {
            Debug.Fail("Unsupported response type");
            throw new InvalidOperationException();
        }
    }

    private DocumentSymbol[]? RemapDocumentSymbols(RazorCSharpDocument csharpDocument, DocumentSymbol[]? documentSymbols)
    {
        if (documentSymbols is null)
        {
            return null;
        }

        using var _ = ListPool<DocumentSymbol>.GetPooledObject(out var mappedSymbols);

        foreach (var documentSymbol in documentSymbols)
        {
            if (TryRemapRanges(csharpDocument, documentSymbol))
            {
                documentSymbol.Children = RemapDocumentSymbols(csharpDocument, documentSymbol.Children);

                mappedSymbols.Add(documentSymbol);
            }
            else if (documentSymbol.Children is [_, ..] &&
                RemapDocumentSymbols(csharpDocument, documentSymbol.Children) is [_, ..] mappedChildren)
            {
                // This range didn't map, but some/all of its children did, so we promote them to this level so we don't
                // lose any information.
                mappedSymbols.AddRange(mappedChildren);
            }
        }

        return mappedSymbols.ToArray();

        bool TryRemapRanges(RazorCSharpDocument csharpDocument, DocumentSymbol documentSymbol)
        {
            if (_documentMappingService.TryMapToRazorDocumentRange(csharpDocument, documentSymbol.Range, out var newRange) &&
                _documentMappingService.TryMapToRazorDocumentRange(csharpDocument, documentSymbol.SelectionRange, out var newSelectionRange))
            {
                documentSymbol.Range = newRange;
                documentSymbol.SelectionRange = newSelectionRange;

                return true;
            }

            return false;
        }
    }
}
