// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.DocumentMapping;

internal sealed class DefaultDocumentPositionInfoStrategy : IDocumentPositionInfoStrategy
{
    public static IDocumentPositionInfoStrategy Instance { get; } = new DefaultDocumentPositionInfoStrategy();

    private DefaultDocumentPositionInfoStrategy()
    {
    }

    public DocumentPositionInfo GetPositionInfo(IDocumentMappingService mappingService, RazorCodeDocument codeDocument, int hostDocumentIndex)
        => mappingService.GetPositionInfo(codeDocument, hostDocumentIndex);
}
