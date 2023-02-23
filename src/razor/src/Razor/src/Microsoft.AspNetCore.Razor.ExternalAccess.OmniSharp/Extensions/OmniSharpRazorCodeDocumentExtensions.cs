// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.OmniSharp.Extensions;

public static class OmniSharpRazorCodeDocumentExtensions
{
    public static SourceText GetInternalCSharpSourceText(this RazorCodeDocument codeDocument)
    {
        return codeDocument.GetCSharpSourceText();
    }
}
