// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal interface IDocumentProcessedListener
{
    void DocumentProcessed(RazorCodeDocument codeDocument, DocumentSnapshot documentSnapshot);
}
