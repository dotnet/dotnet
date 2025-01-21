// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal interface IDocumentProcessedListener
{
    void DocumentProcessed(RazorCodeDocument codeDocument, DocumentSnapshot documentSnapshot);
}
