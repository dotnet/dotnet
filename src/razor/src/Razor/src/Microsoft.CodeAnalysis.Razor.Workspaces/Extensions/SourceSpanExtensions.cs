// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

internal static class SourceSpanExtensions
{
    public static TextSpan ToTextSpan(this SourceSpan span)
        => new(span.AbsoluteIndex, span.Length);
}
