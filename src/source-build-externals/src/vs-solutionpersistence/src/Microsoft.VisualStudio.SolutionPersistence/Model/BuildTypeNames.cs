// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.SolutionPersistence.Model;

internal static class BuildTypeNames
{
    internal const string All = PlatformNames.All;
    internal const string Missing = PlatformNames.Missing;

    internal const string Debug = nameof(Debug);
    internal const string Release = nameof(Release);

    internal static string ToStringKnown(string buildType)
    {
        return TryGetKnown(buildType.AsSpan(), out string? value) ? value : buildType;
    }

    internal static bool TryGetKnown(StringSpan buildType, [NotNullWhen(true)] out string? value)
    {
        value = buildType switch
        {
            All => All,
            Missing => Missing,
            Debug => Debug,
            Release => Release,
            _ => null,
        };
        return value is not null;
    }
}
