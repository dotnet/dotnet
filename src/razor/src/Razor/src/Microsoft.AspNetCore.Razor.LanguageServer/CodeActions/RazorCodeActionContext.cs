// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal sealed class RazorCodeActionContext
{
    public RazorCodeActionContext(
        VSCodeActionParams request,
        IDocumentSnapshot documentSnapshot,
        RazorCodeDocument codeDocument,
        SourceLocation location,
        SourceText sourceText,
        bool supportsFileCreation,
        bool supportsCodeActionResolve)
    {
        Request = request ?? throw new ArgumentNullException(nameof(request));
        DocumentSnapshot = documentSnapshot ?? throw new ArgumentNullException(nameof(documentSnapshot));
        CodeDocument = codeDocument ?? throw new ArgumentNullException(nameof(codeDocument));
        Location = location;
        SourceText = sourceText ?? throw new ArgumentNullException(nameof(sourceText));
        SupportsFileCreation = supportsFileCreation;
        SupportsCodeActionResolve = supportsCodeActionResolve;
    }

    public VSCodeActionParams Request { get; }
    public IDocumentSnapshot DocumentSnapshot { get; }
    public RazorCodeDocument CodeDocument { get; }
    public SourceLocation Location { get; }
    public SourceText SourceText { get; }
    public bool SupportsFileCreation { get; }
    public bool SupportsCodeActionResolve { get; }
}
