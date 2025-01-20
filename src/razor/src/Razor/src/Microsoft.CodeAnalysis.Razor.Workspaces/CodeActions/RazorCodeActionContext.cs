// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.CodeActions;

internal sealed record class RazorCodeActionContext(
    VSCodeActionParams Request,
    IDocumentSnapshot DocumentSnapshot,
    RazorCodeDocument CodeDocument,
    Uri? DelegatedDocumentUri,
    int StartAbsoluteIndex,
    int EndAbsoluteIndex,
    Protocol.RazorLanguageKind LanguageKind,
    SourceText SourceText,
    bool SupportsFileCreation,
    bool SupportsCodeActionResolve)
{
    public bool HasSelection => StartAbsoluteIndex != EndAbsoluteIndex;
}
