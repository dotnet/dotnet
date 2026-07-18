// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Helix.AzureDevOpsTestPublisher;

/// <summary>
/// Builds the human-visible test title (AzDO <c>testCaseTitle</c>) shown when a job opts in to
/// fully qualified test names. The goal is to always surface the fully qualified name while keeping
/// any information the display name adds on top of it.
/// </summary>
/// <remarks>
/// Rules (given a stable <c>FQN</c> = <c>Namespace.Type.Method</c> and a framework display name):
/// <list type="bullet">
/// <item>Display name is the method (the FQN's last segment) — e.g. MSTest/xUnit defaults — emit just <c>FQN</c>.</item>
/// <item>Parameterized row whose base is the method — e.g. <c>Method ("net10.0")</c> — emit <c>FQN ("net10.0")</c>
/// so the class prefix isn't duplicated but the argument list is preserved.</item>
/// <item>Display name carries something else — e.g. a custom xUnit <c>DisplayName</c> — emit <c>FQN (display name)</c>.</item>
/// </list>
/// </remarks>
internal static class TestNameFormatter
{
    public static string FormatDisplayName(string? fullyQualifiedName, string? displayName)
    {
        if (string.IsNullOrEmpty(fullyQualifiedName))
        {
            return displayName ?? string.Empty;
        }

        if (string.IsNullOrEmpty(displayName))
        {
            return fullyQualifiedName;
        }

        // Split the display name into its base and any argument/parameter suffix so parameterized
        // rows keep their arguments without repeating the method name, e.g.:
        //   "Method ("net10.0")" -> base "Method",           suffix "("net10.0")"
        //   "Method(1, 2)"        -> base "Method",           suffix "(1, 2)"
        //   "My scenario"         -> base "My scenario",      suffix ""
        int parenIndex = displayName.IndexOf('(');
        string baseName = parenIndex >= 0 ? displayName[..parenIndex].TrimEnd() : displayName;
        string suffix = parenIndex >= 0 ? displayName[parenIndex..] : string.Empty;

        // When the display's base already matches the FQN (equal, or its trailing method segment),
        // the FQN conveys it. Append only the argument suffix, if any.
        bool baseMatchesFullyQualifiedName =
            string.Equals(fullyQualifiedName, baseName, StringComparison.Ordinal)
            || fullyQualifiedName.EndsWith("." + baseName, StringComparison.Ordinal);

        if (baseMatchesFullyQualifiedName)
        {
            return suffix.Length > 0 ? $"{fullyQualifiedName} {suffix}" : fullyQualifiedName;
        }

        // The display name adds information beyond the FQN (e.g. a custom display name). Keep both.
        return $"{fullyQualifiedName} ({displayName})";
    }
}
