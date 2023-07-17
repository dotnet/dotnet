// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
