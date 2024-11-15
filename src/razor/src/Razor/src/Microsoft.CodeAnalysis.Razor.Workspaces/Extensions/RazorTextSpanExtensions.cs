// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.Protocol;

internal static class RazorTextSpanExtensions
{
    public static TextSpan ToTextSpan(this RazorTextSpan razorTextSpan)
        => new TextSpan(razorTextSpan.Start, razorTextSpan.Length);
}
