// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Text;

namespace Roslyn.LanguageServer.Protocol;

internal static partial class LspExtensions
{
    public static RazorTextChange ToRazorTextChange(this TextChange textChange)
    {
        return new RazorTextChange()
        {
            Span = new RazorTextSpan()
            {
                Start = textChange.Span.Start,
                Length = textChange.Span.Length,
            },
            NewText = textChange.NewText
        };
    }
}
