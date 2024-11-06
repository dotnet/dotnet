// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hosting;

internal static class FormattingFlagExtensions
{
    public static bool IsEnabled(this FormattingFlags flags)
        => flags.IsFlagSet(FormattingFlags.Enabled);

    public static bool IsOnTypeEnabled(this FormattingFlags flags)
        => flags.IsEnabled() && flags.IsFlagSet(FormattingFlags.OnType);

    public static bool IsOnPasteEnabled(this FormattingFlags flags)
        => flags.IsEnabled() && flags.IsFlagSet(FormattingFlags.OnPaste);
}
