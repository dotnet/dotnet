// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.Text;

namespace Microsoft.CodeAnalysis.Razor.Protocol;

internal static class TextChangeExtensions
{
    public static ITextChange ToVisualStudioTextChange(this RazorTextChange razorTextChange)
        => new VisualStudioTextChange(razorTextChange.Span.Start, razorTextChange.Span.Length, razorTextChange.NewText!);
}
