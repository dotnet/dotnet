// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal sealed record class RazorCodeActionContext(
    VSCodeActionParams Request,
    IDocumentSnapshot DocumentSnapshot,
    RazorCodeDocument CodeDocument,
    SourceLocation Location,
    SourceText SourceText,
    bool SupportsFileCreation,
    bool SupportsCodeActionResolve);
