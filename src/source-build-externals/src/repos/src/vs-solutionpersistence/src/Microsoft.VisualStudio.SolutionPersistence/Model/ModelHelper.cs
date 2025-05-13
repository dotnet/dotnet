// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.SolutionPersistence.Model;

/// <summary>
/// Helpers and extension methods to be used on items in the model.
/// </summary>
internal static class ModelHelper
{
    // Generically finds the item in the list by itemRef and returns it, otherwise null.
    internal static T? FindByItemRef<T>(IReadOnlyList<T>? items, string itemRef, Func<T, string> getItemRef, bool ignoreCase)
        where T : notnull
    {
        if (items is null)
        {
            return default;
        }

        StringComparer comparer = ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
        foreach (T item in items.GetStructEnumerable())
        {
            if (comparer.Equals(getItemRef(item), itemRef))
            {
                return item;
            }
        }

        return default;
    }

    // Returns the itemRef string in the list if it exists, otherwise null.
    internal static string? FindByItemRef(IReadOnlyList<string>? items, string itemRef, bool ignoreCase)
    {
        return FindByItemRef(items, itemRef, x => x, ignoreCase);
    }

    // Finds the item in the SolutionItemModel list by itemRef and returns it, otherwise null.
    internal static T? FindByItemRef<T>(this IReadOnlyList<T>? items, string itemRef)
        where T : SolutionItemModel
    {
        return FindByItemRef(items, itemRef, x => x.ItemRef, ignoreCase: true);
    }

    // Finds the item in the list of property sets by itemRef and returns it, otherwise null.
    internal static SolutionPropertyBag? FindByItemRef(this IReadOnlyList<SolutionPropertyBag>? items, string itemRef)
    {
        return FindByItemRef(items, itemRef, x => x.Id, ignoreCase: false);
    }

    internal static string GetDisplayName(this ProjectType projectType)
    {
        return projectType.Name ?? projectType.ProjectTypeId.ToString();
    }

    internal static string GetSolutionConfiguration(this ConfigurationRule rule)
    {
        return rule.SolutionBuildType.IsNullOrEmpty() && rule.SolutionPlatform.IsNullOrEmpty() ?
            string.Empty :
            $"{rule.SolutionBuildType.NullIfEmpty() ?? BuildTypeNames.All}|{rule.SolutionPlatform.NullIfEmpty() ?? PlatformNames.All}";
    }

    // Splits the configuration into build type and platform.
    internal static bool TrySplitFullConfiguration(
        string fullConfiguration,
        out StringSpan buildType,
        out StringSpan platform)
    {
        if (string.IsNullOrEmpty(fullConfiguration))
        {
            buildType = StringSpan.Empty;
            platform = StringSpan.Empty;
            return false;
        }

        int sep = fullConfiguration.IndexOf('|');
        if (sep <= 0)
        {
            buildType = StringSpan.Empty;
            platform = StringSpan.Empty;
            return false;
        }

        buildType = fullConfiguration.AsSpan(0, sep).Trim();
        platform = fullConfiguration.AsSpan(sep + 1).Trim();
        return !buildType.IsEmpty && !platform.IsEmpty;
    }

    // Splits the configuration and uses the string table to reuse existing string.
    internal static bool TrySplitFullConfiguration(
        StringTable stringTable,
        string fullConfiguration,
        [NotNullWhen(true)] out string? buildType,
        [NotNullWhen(true)] out string? platform)
    {
        if (TrySplitFullConfiguration(fullConfiguration, out StringSpan buildTypeSpan, out StringSpan platformSpan))
        {
            if (!BuildTypeNames.TryGetKnown(buildTypeSpan, out buildType))
            {
                buildType = stringTable.GetString(buildTypeSpan);
            }

            if (!PlatformNames.TryGetKnown(platformSpan, out platform))
            {
                platform = stringTable.GetString(platformSpan);
            }

            return true;
        }

        buildType = null;
        platform = null;
        return false;
    }

    internal static ConfigurationRule CreatePlatformRule(string solutionPlatform, string projectPlatform)
    {
        return new ConfigurationRule(BuildDimension.Platform, solutionBuildType: string.Empty, solutionPlatform, projectPlatform);
    }

    internal static ConfigurationRule CreateNoBuildRule()
    {
        return new ConfigurationRule(BuildDimension.Build, solutionBuildType: string.Empty, solutionPlatform: string.Empty, projectValue: bool.FalseString);
    }
}
