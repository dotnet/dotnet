// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
