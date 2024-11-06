// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.Language;

internal static class IRazorGeneratedDocumentExtensions
{
    public static SourceText GetGeneratedSourceText(this IRazorGeneratedDocument generatedDocument)
    {
        if (generatedDocument.CodeDocument is not { } codeDocument)
        {
            throw new InvalidOperationException("Cannot use document mapping service on a generated document that has a null CodeDocument.");
        }

        return codeDocument.GetGeneratedSourceText(generatedDocument);
    }
}
