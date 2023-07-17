// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal sealed record RazorSourceChange(
    RazorSourceSpan Span,
    string NewText)
{
    public bool IsDelete => Span.Length > 0 && NewText.Length == 0;
    public bool IsInsert => Span.Length == 0 && NewText.Length > 0;
    public bool IsReplace => Span.Length > 0 && NewText.Length > 0;
}
