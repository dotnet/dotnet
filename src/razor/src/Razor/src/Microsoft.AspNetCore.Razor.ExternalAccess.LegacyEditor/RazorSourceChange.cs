// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal sealed record RazorSourceChange(
    RazorSourceSpan Span,
    string NewText)
{
    public bool IsDelete => Span.Length > 0 && NewText.Length == 0;
    public bool IsInsert => Span.Length == 0 && NewText.Length > 0;
    public bool IsReplace => Span.Length > 0 && NewText.Length > 0;
}
