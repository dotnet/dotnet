// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.SolutionPersistence.Utilities;

namespace Microsoft.VisualStudio.SolutionPersistence.Model;

internal sealed partial class SolutionConfigurationMap
{
    /// <summary>
    /// This is used to create a simplified set of rules for a project to get project
    /// configurations. This uses the cached set of mappings to determine what rules to create.
    /// </summary>
    internal List<ConfigurationRule>? CreateProjectRules(SolutionProjectModel projectModel)
    {
        // If this project doesn't have any mappings, then we can't create any rules for it.
        if (!this.perProjectCurrent.TryGetValue(projectModel, out SolutionToProjectMappings currentMatrix))
        {
            return null;
        }

        // What a project would look like with the default configuration rules.
        SolutionToProjectMappings expectedMatrix = new SolutionToProjectMappings(this, projectModel, out bool _);

        return this.CreateRules(in expectedMatrix, in currentMatrix);
    }

    /// <summary>
    /// Applies the rules to the mappings. This will update the mappings in place.
    /// Only runs the rules on the scope specified by the build type and platform.
    /// </summary>
    /// <param name="projectMappings">The mappings to update.</param>
    /// <param name="scopedRules">The rules to run, scoped to their effect.</param>
    private void ApplyRules(in SolutionToProjectMappings projectMappings, scoped in ScopedRules scopedRules)
    {
        int iBuildTypeBegin = scopedRules.BuildTypeIndex == ScopedRules.All ? 0 : scopedRules.BuildTypeIndex;
        int iBuildTypeEnd = scopedRules.BuildTypeIndex == ScopedRules.All ? this.BuildTypesCount : scopedRules.BuildTypeIndex + 1;

        int iPlatformBegin = scopedRules.PlatformIndex == ScopedRules.All ? 0 : scopedRules.PlatformIndex;
        int iPlatformEnd = scopedRules.PlatformIndex == ScopedRules.All ? this.PlatformsCount : scopedRules.PlatformIndex + 1;

        for (int iBuildType = iBuildTypeBegin; iBuildType < iBuildTypeEnd; iBuildType++)
        {
            for (int iPlatform = iPlatformBegin; iPlatform < iPlatformEnd; iPlatform++)
            {
                if (!scopedRules.Rules.HasRules)
                {
                    continue;
                }

                SolutionConfigIndex idx = this.ToIndex(iBuildType, iPlatform);
                ProjectConfigMapping mapping = projectMappings[idx];
                string solutionBuildType = idx.BuildType(this);
                string solutionPlatform = idx.Platform(this);

                string projectBuildType = scopedRules.Rules.GetProjectBuildType(solutionBuildType, solutionPlatform) ?? mapping.BuildType;
                string projectPlatform = scopedRules.Rules.GetProjectPlatform(solutionBuildType, solutionPlatform) ?? mapping.Platform;
                bool build = scopedRules.Rules.GetIsBuildable(solutionBuildType, solutionPlatform) ?? mapping.Build;
                bool deploy = scopedRules.Rules.GetIsDeployable(solutionBuildType, solutionPlatform) ?? mapping.Deploy;

                projectMappings[idx] = new ProjectConfigMapping(projectBuildType, projectPlatform, build, deploy);
            }
        }
    }

    /// <summary>
    /// Creates a set of rules that convert the project mappings in currentMatrix to the expectedMatrix.
    /// </summary>
    /// <param name="expectedMatrix">What the default mappings are. These get updated after each rule is added.</param>
    /// <param name="currentMatrix">What the final mappings should be.</param>
    private List<ConfigurationRule>? CreateRules(
        in SolutionToProjectMappings expectedMatrix,
        in SolutionToProjectMappings currentMatrix)
    {
        // trying to minimize the rules
        // common case is to simply has a different platform selected, aka CSProj uses x86 instad of AnyCPU for x86
        // build type difference

        // Create rules when mappings are the same for all dimensions in the project. (e.g. Build => false)
        bool hasRemainingDiffs = this.CreateProjectGlobalRules(
            in expectedMatrix,
            in currentMatrix,
            out ProjectDiffTracker[] perPlatform,
            out ProjectDiffTracker[] perBuildType,
            out List<ConfigurationRule>? allRules);

        if (!hasRemainingDiffs)
        {
            return allRules;
        }

        // Create rules when mappings are the same for all build types. (e.g. AnyCPU => arm64)
        bool addedRules = false;
        for (int iPlatform = 0; iPlatform < this.PlatformsCount; iPlatform++)
        {
            // emit all *|plat = xxx|yyy|....
            ref ProjectDiffTracker projectDiffTracker = ref perPlatform[iPlatform];
            if (projectDiffTracker.HasSame)
            {
                addedRules = true;

                ConfigurationRule[] platformRules = this.CreateDimensionRules(in expectedMatrix, ref projectDiffTracker, ScopedRules.All, iPlatform);
                allRules ??= [];
                allRules.AddRange(platformRules);
            }
        }

        if (addedRules)
        {
            ProjectDiffTracker.ClearDiffs(perBuildType);

            for (int iBuildType = 0; iBuildType < this.BuildTypesCount; iBuildType++)
            {
                for (int iPlatform = 0; iPlatform < this.PlatformsCount; iPlatform++)
                {
                    SolutionConfigIndex index = this.ToIndex(iBuildType, iPlatform);
                    ProjectConfigMapping expectedMapping = expectedMatrix[index];
                    ProjectConfigMapping currentMapping = currentMatrix[index];
                    perBuildType[iBuildType].ObserveValue(in expectedMapping, in currentMapping);
                }
            }
        }

        // Create rules when mappings are the same for all platforms.
        bool hasSingleChanges = false;
        for (int iBuildType = 0; iBuildType < this.BuildTypesCount; iBuildType++)
        {
            // emit all cfg|* = xxx|yyy|....
            ref ProjectDiffTracker projectDiffTracker = ref perBuildType[iBuildType];
            if (projectDiffTracker.HasSame)
            {
                ConfigurationRule[] buildTypeRules = this.CreateDimensionRules(in expectedMatrix, ref projectDiffTracker, iBuildType, ScopedRules.All);
                allRules ??= [];
                allRules.AddRange(buildTypeRules);
            }

            hasSingleChanges |= projectDiffTracker.HasDifferences;
        }

        if (hasSingleChanges)
        {
            // all remaining "config|plat => .....;
            // CONSIDER: we can add "majority" simplification here, aka if
            // for example one entry with different platform , but all other entry are still different that expected, then we can
            // collapse to "*|foo = *|majority" + "specifig|foo = specific|different" instead of expand all.
            // OPINION: This may add complexity and remove the predictability of the rules, we would then
            // need to keep track of rules instead of being able to always regenerate them.
            for (int iBuildType = 0; iBuildType < this.BuildTypesCount; iBuildType++)
            {
                for (int iPlatform = 0; iPlatform < this.PlatformsCount; iPlatform++)
                {
                    SolutionConfigIndex index = this.ToIndex(iBuildType, iPlatform);
                    ProjectConfigMapping expectedMapping = expectedMatrix[index];
                    ProjectConfigMapping currentMapping = currentMatrix[index];
                    if (expectedMapping.IsSame(in currentMapping))
                    {
                        continue;
                    }

                    ListBuilderStruct<ConfigurationRule> newRules = new ListBuilderStruct<ConfigurationRule>();

                    string solutionBuildType = index.BuildType(this);
                    string solutionPlatform = index.Platform(this);

                    if (!StringComparer.Ordinal.Equals(currentMapping.BuildType, expectedMapping.BuildType))
                    {
                        newRules.Add(new ConfigurationRule(BuildDimension.BuildType, solutionBuildType, solutionPlatform, currentMapping.BuildType));
                    }

                    if (!StringComparer.Ordinal.Equals(PlatformNames.Canonical(currentMapping.Platform), PlatformNames.Canonical(expectedMapping.Platform)))
                    {
                        newRules.Add(new ConfigurationRule(BuildDimension.Platform, solutionBuildType, solutionPlatform, currentMapping.Platform));
                    }

                    if (currentMapping.Build != expectedMapping.Build)
                    {
                        newRules.Add(new ConfigurationRule(BuildDimension.Build, solutionBuildType, solutionPlatform, currentMapping.Build.ToString()));
                    }

                    if (currentMapping.Deploy != expectedMapping.Deploy)
                    {
                        newRules.Add(new ConfigurationRule(BuildDimension.Deploy, solutionBuildType, solutionPlatform, currentMapping.Deploy.ToString()));
                    }

                    if (newRules.Count != 0)
                    {
                        allRules ??= [];
                        foreach (ConfigurationRule rule in newRules)
                        {
                            allRules.Add(rule);
                        }

                        // no need to update expected, but it is easier for debuging purposes.
                        this.ApplyRules(in expectedMatrix, new ScopedRules(iBuildType, iPlatform, newRules.ToArray()));
                    }
                }
            }
        }

        return allRules;
    }

    // Create all the rules that apply to all project configurations.
    private bool CreateProjectGlobalRules(
        in SolutionToProjectMappings expectedMatrix,
        in SolutionToProjectMappings currentMatrix,
        out ProjectDiffTracker[] perPlatform,
        out ProjectDiffTracker[] perBuildType,
        out List<ConfigurationRule>? rules)
    {
        rules = null;
        perPlatform = new ProjectDiffTracker[this.PlatformsCount];
        perBuildType = new ProjectDiffTracker[this.BuildTypesCount];

        // Looks for any mappings that always differer the same way in this project.
        ProjectDiffTracker global = new ProjectDiffTracker();

        // Looks for any mappings that are always the same in this project.
        ProjectDiffTracker unique = new ProjectDiffTracker();

        // Try to create a rule that applies to all build types and platforms.
        for (int iBuildType = 0; iBuildType < this.BuildTypesCount; iBuildType++)
        {
            for (int iPlatform = 0; iPlatform < this.PlatformsCount; iPlatform++)
            {
                SolutionConfigIndex index = this.ToIndex(iBuildType, iPlatform);
                ProjectConfigMapping expectedMapping = expectedMatrix[index];
                ProjectConfigMapping currentMapping = currentMatrix[index];
                perPlatform[iPlatform].ObserveValue(in expectedMapping, in currentMapping);
                perBuildType[iBuildType].ObserveValue(in expectedMapping, in currentMapping);
                global.ObserveValue(in expectedMapping, in currentMapping);
                unique.ObserveDifferentValue(in currentMapping);
            }
        }

        // Create Build rule.
        if (global.BuildTracker.TryGetSame(unique.BuildTracker, out bool buildable))
        {
            rules ??= [];
            rules.Add(new ConfigurationRule(BuildDimension.Build, string.Empty, string.Empty, buildable.ToString()));
            global.ClearDiffs(BuildDimension.Build);
            ProjectDiffTracker.ClearDiffs(BuildDimension.Build, perBuildType);
            ProjectDiffTracker.ClearDiffs(BuildDimension.Build, perPlatform);
        }

        // Create Deploy rule.
        if (global.DeployTracker.TryGetSame(unique.DeployTracker, out bool deployable))
        {
            rules ??= [];
            rules.Add(new ConfigurationRule(BuildDimension.Deploy, string.Empty, string.Empty, deployable.ToString()));
            global.ClearDiffs(BuildDimension.Deploy);
            ProjectDiffTracker.ClearDiffs(BuildDimension.Deploy, perBuildType);
            ProjectDiffTracker.ClearDiffs(BuildDimension.Deploy, perPlatform);
        }

        // Check if rule applies to all build types or just specific one.
        if (global.BuildTypeTracker.TryGetSame(unique.BuildTypeTracker, out string? projectBuildType))
        {
            rules ??= [];
            rules.Add(new ConfigurationRule(BuildDimension.BuildType, string.Empty, string.Empty, projectBuildType));
            global.ClearDiffs(BuildDimension.BuildType);
            ProjectDiffTracker.ClearDiffs(BuildDimension.BuildType, perBuildType);
            ProjectDiffTracker.ClearDiffs(BuildDimension.BuildType, perPlatform);
        }

        // Check if rule applies to all platforms or just specific one.
        if (global.PlatformTracker.TryGetSame(unique.PlatformTracker, out string? projectPlatform))
        {
            rules ??= [];
            rules.Add(new ConfigurationRule(BuildDimension.Platform, string.Empty, string.Empty, projectPlatform));
            global.ClearDiffs(BuildDimension.Platform);
            ProjectDiffTracker.ClearDiffs(BuildDimension.Platform, perBuildType);
            ProjectDiffTracker.ClearDiffs(BuildDimension.Platform, perPlatform);
        }

        if (!rules.IsNullOrEmpty())
        {
            this.ApplyRules(in expectedMatrix, new ScopedRules(ScopedRules.All, ScopedRules.All, rules));
        }

        // easy case = all is the same as expected;
        return global.HasDifferences;
    }

    // Creates rules that apply to all mappings for a specific dimension.
    private ConfigurationRule[] CreateDimensionRules(
        in SolutionToProjectMappings expectedMatrix,
        ref ProjectDiffTracker projectDiffTracker,
        int iBuildType,
        int iPlatform)
    {
        string solutionBuildType = iBuildType == ScopedRules.All ? string.Empty : this.solutionModel.BuildTypes[iBuildType];
        string solutionPlatform = iPlatform == ScopedRules.All ? string.Empty : this.solutionModel.Platforms[iPlatform];

        ListBuilderStruct<ConfigurationRule> rulesBuilder = new ListBuilderStruct<ConfigurationRule>();

        // Create Build rule.
        if (projectDiffTracker.BuildTracker.TryGetSame(out bool buildable))
        {
            rulesBuilder.Add(new ConfigurationRule(BuildDimension.Build, solutionBuildType, solutionPlatform, buildable.ToString()));
            projectDiffTracker.BuildTracker.ClearDifferences();
        }

        // Create Deploy rule.
        if (projectDiffTracker.DeployTracker.TryGetSame(out bool deployable))
        {
            rulesBuilder.Add(new ConfigurationRule(BuildDimension.Deploy, solutionBuildType, solutionPlatform, deployable.ToString()));
            projectDiffTracker.DeployTracker.ClearDifferences();
        }

        // Check if rule applies to all build types or just specific one.
        if (projectDiffTracker.BuildTypeTracker.TryGetSame(out string? buildType))
        {
            rulesBuilder.Add(new ConfigurationRule(BuildDimension.BuildType, solutionBuildType, solutionPlatform, buildType));
            projectDiffTracker.BuildTypeTracker.ClearDifferences();
        }

        // Check if rule applies to all platforms or just specific one.
        if (projectDiffTracker.PlatformTracker.TryGetSame(out string? platform))
        {
            rulesBuilder.Add(new ConfigurationRule(BuildDimension.Platform, solutionBuildType, solutionPlatform, platform));
            projectDiffTracker.PlatformTracker.ClearDifferences();
        }

        ConfigurationRule[] rules = rulesBuilder.ToArray();
        if (rules.Length != 0)
        {
            this.ApplyRules(in expectedMatrix, new ScopedRules(iBuildType, iPlatform, rules));
        }

        return rules;
    }

    /// <summary>
    /// A list of rules and a specific or range of build types and platforms that the rules apply to.
    /// </summary>
    private readonly ref struct ScopedRules(int buildTypeIndex, int platformIndex, IReadOnlyList<ConfigurationRule> rules)
    {
        internal const int All = -1;
        internal readonly int BuildTypeIndex = buildTypeIndex;
        internal readonly int PlatformIndex = platformIndex;

        internal readonly ConfigurationRuleFollower Rules = new ConfigurationRuleFollower(rules);
    }
}
