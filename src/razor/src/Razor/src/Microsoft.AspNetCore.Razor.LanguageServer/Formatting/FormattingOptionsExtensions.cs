// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

internal static class FormattingOptionsExtensions
{
    public static RazorIndentationOptions GetIndentationOptions(this FormattingOptions options)
        => new(
            UseTabs: !options.InsertSpaces,
            TabSize: options.TabSize,
            IndentationSize: options.TabSize);
}
