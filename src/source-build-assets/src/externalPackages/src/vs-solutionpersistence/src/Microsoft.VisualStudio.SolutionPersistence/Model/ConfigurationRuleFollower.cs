// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.SolutionPersistence.Model;

/// <summary>
/// Helper to process configuration rules.
/// </summary>
internal readonly ref struct ConfigurationRuleFollower(IReadOnlyList<ConfigurationRule>? configurationRules)
{
    private readonly IReadOnlyList<ConfigurationRule>? configurationRules = configurationRules;

    internal bool HasRules => !this.configurationRules.IsNullOrEmpty();

    internal readonly bool? GetIsBuildable(string? solutionBuildType = null, string? solutionPlatform = null)
    {
        string value = this.GetDimensionValue(BuildDimension.Build, solutionBuildType, solutionPlatform);
        return string.IsNullOrEmpty(value) ? null : bool.Parse(value);
    }

    internal readonly bool? GetIsDeployable(string solutionBuildType, string solutionPlatform)
    {
        string value = this.GetDimensionValue(BuildDimension.Deploy, solutionBuildType, solutionPlatform);
        return string.IsNullOrEmpty(value) ? null : bool.Parse(value);
    }

    internal readonly string? GetProjectBuildType(string solutionBuildType, string solutionPlatform)
    {
        string value = this.GetDimensionValue(BuildDimension.BuildType, solutionBuildType, solutionPlatform);
        return value == "*" ? solutionBuildType : value.NullIfEmpty();
    }

    internal readonly string? GetProjectPlatform(string solutionBuildType, string solutionPlatform)
    {
        string value = this.GetDimensionValue(BuildDimension.Platform, solutionBuildType, solutionPlatform);
        return value == "*" ? solutionPlatform : value.NullIfEmpty();
    }

    /// <summary>
    /// Checks if the rule applies to the specified configuration.
    /// Null/Empty dimensions represent all values of the dimension.
    /// If null dimensions are passed, only rules that apply to all values of the dimensions apply.
    /// </summary>
    private static bool RuleAppliesTo(ConfigurationRule rule, BuildDimension dimension, string? solutionBuildType, string? solutionPlatform)
    {
        if (rule.Dimension != dimension)
        {
            return false;
        }

        // The rule applies to all values.
        if (AppliesToAllBuildTypes(rule) && AppliesToAllPlatforms(rule))
        {
            // Special case for handling the "*" value. This mapping uses the solution value for the project value,
            // so this needs to validate that the solution value is specified.
            // Otherwise this always returns true.
            return rule.ProjectValue != "*" ||
                ((dimension != BuildDimension.BuildType || IsSpecified(solutionBuildType)) &&
                 (dimension != BuildDimension.Platform || IsSpecified(solutionPlatform)));
        }

        // If the rule applies to all Platforms, the BuildType must be specified and match.
        // If the rule applies to all BuildTypes, the Platform must be specified and match.
        // If the rule applies to specific BuildType and Platform, both must be specified and match.
        return
            AppliesToAllPlatforms(rule) && IsSpecified(solutionBuildType) ? IsSameBuildType(rule, solutionBuildType) :
            AppliesToAllBuildTypes(rule) && IsSpecified(solutionPlatform) ? IsSamePlatform(rule, solutionPlatform) :
            IsSpecified(solutionBuildType) && IsSpecified(solutionPlatform) && IsSame(rule, solutionBuildType, solutionPlatform);

        // These local functions are used to (hopefullly) make the code more readable.
        static bool AppliesToAllBuildTypes(ConfigurationRule rule) => rule.SolutionBuildType.IsNullOrEmpty();
        static bool AppliesToAllPlatforms(ConfigurationRule rule) => rule.SolutionPlatform.IsNullOrEmpty();

        static bool IsSpecified([NotNullWhen(true)] string? dimensionValue) => !dimensionValue.IsNullOrEmpty();
        static bool IsSame(ConfigurationRule rule, string solutionBuildType, string solutionPlatform) =>
            IsSameBuildType(rule, solutionBuildType) && IsSamePlatform(rule, solutionPlatform);
        static bool IsSameBuildType(ConfigurationRule rule, string solutionBuildType) =>
            StringComparer.OrdinalIgnoreCase.Equals(rule.SolutionBuildType, solutionBuildType);
        static bool IsSamePlatform(ConfigurationRule rule, string solutionPlatform) =>
            StringComparer.OrdinalIgnoreCase.Equals(PlatformNames.Canonical(rule.SolutionPlatform), PlatformNames.Canonical(solutionPlatform));
    }

    private readonly string GetDimensionValue(BuildDimension dimension, string? solutionBuildType, string? solutionPlatform)
    {
        foreach (ConfigurationRule rule in this.configurationRules.GetStructReverseEnumerable())
        {
            if (RuleAppliesTo(rule, dimension, solutionBuildType, solutionPlatform))
            {
                return rule.ProjectValue;
            }
        }

        return string.Empty;
    }
}
