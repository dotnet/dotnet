// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal static partial class RazorWrapperFactory
{
    private class DiagnosticWrapper(RazorDiagnostic obj) : Wrapper<RazorDiagnostic>(obj), IRazorDiagnostic
    {
        public RazorSourceSpan Span => ConvertSourceSpan(Object.Span);

        public string GetMessage(IFormatProvider? formatProvider)
            => Object.GetMessage(formatProvider);
    }
}
