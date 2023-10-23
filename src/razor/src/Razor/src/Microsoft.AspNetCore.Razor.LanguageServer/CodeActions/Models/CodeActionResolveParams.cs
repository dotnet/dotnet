// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;

internal sealed class CodeActionResolveParams
{
    public object? Data { get; set; }

    // Need to use the VS type so that project context info, if present, is maintained
    public required VSTextDocumentIdentifier RazorFileIdentifier { get; set; }
}
