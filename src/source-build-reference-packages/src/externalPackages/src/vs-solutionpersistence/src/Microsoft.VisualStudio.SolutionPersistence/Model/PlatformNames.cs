// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.SolutionPersistence.Model;

internal static class PlatformNames
{
    internal const string All = "*";
    internal const string Missing = "?";

    internal const string AnyCPU = nameof(AnyCPU);
    internal const string AnySpaceCPU = "Any CPU";
    internal const string Win32 = nameof(Win32);
#pragma warning disable SA1303 // Const field names should begin with upper-case letter
    internal const string x64 = nameof(x64);
    internal const string x86 = nameof(x86);
    internal const string arm = nameof(arm);
    internal const string arm64 = nameof(arm64);
#pragma warning restore SA1303 // Const field names should begin with upper-case letter

    // All caps to intern this common version.
    internal const string ARM = nameof(ARM);
    internal const string ARM64 = nameof(ARM64);

    internal static string Canonical(string platform) => string.Equals(platform, AnySpaceCPU, StringComparison.OrdinalIgnoreCase) ? AnyCPU : platform;

    internal static string ToStringKnown(string platform)
    {
        return TryGetKnown(platform.AsSpan(), out string? value) ? value : platform;
    }

    internal static bool TryGetKnown(StringSpan platform, [NotNullWhen(true)] out string? value)
    {
        value = platform switch
        {
            All => All,
            Missing => Missing,
            AnySpaceCPU => AnySpaceCPU,
            Win32 => Win32,
            x64 => x64,
            x86 => x86,
            arm => arm,
            arm64 => arm64,
            ARM => ARM,
            ARM64 => ARM64,
            _ => null,
        };
        return value is not null;
    }
}
