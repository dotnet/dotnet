// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal interface IRazorDiagnostic
{
    RazorSourceSpan Span { get; }

    string GetMessage(IFormatProvider? formatProvider);
}
