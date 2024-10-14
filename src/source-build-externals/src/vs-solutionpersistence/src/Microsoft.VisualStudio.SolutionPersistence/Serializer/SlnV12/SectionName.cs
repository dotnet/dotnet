// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.SlnV12;

internal static class SectionName
{
    // A property for items directory on the solution or project.
    internal const string VisualStudio = "Visual Studio";
    internal const string SolutionProperties = nameof(SolutionProperties);
    internal const string ExtensibilityGlobals = nameof(ExtensibilityGlobals);
    internal const string NestedProjects = nameof(NestedProjects);
    internal const string SolutionConfigurationPlatforms = nameof(SolutionConfigurationPlatforms);
    internal const string ProjectConfigurationPlatforms = nameof(ProjectConfigurationPlatforms);

    // Shared project system properties.
    internal const string SharedMSBuildProjectFiles = nameof(SharedMSBuildProjectFiles);

    // Project's build dependencies.
    internal const string ProjectDependencies = nameof(ProjectDependencies);

    // Solution Folder's files.
    internal const string SolutionItems = nameof(SolutionItems);

    // Convert section names to the already interned constants.
    internal static string InternKnownSectionName(string sectionName)
    {
        return
            StringComparer.OrdinalIgnoreCase.Equals(sectionName, SolutionProperties) ? SolutionProperties :
            StringComparer.OrdinalIgnoreCase.Equals(sectionName, ExtensibilityGlobals) ? ExtensibilityGlobals :
            StringComparer.OrdinalIgnoreCase.Equals(sectionName, NestedProjects) ? NestedProjects :
            StringComparer.OrdinalIgnoreCase.Equals(sectionName, SolutionConfigurationPlatforms) ? SolutionConfigurationPlatforms :
            StringComparer.OrdinalIgnoreCase.Equals(sectionName, ProjectConfigurationPlatforms) ? ProjectConfigurationPlatforms :
            StringComparer.OrdinalIgnoreCase.Equals(sectionName, SharedMSBuildProjectFiles) ? SharedMSBuildProjectFiles :
            StringComparer.OrdinalIgnoreCase.Equals(sectionName, ProjectDependencies) ? ProjectDependencies :
            StringComparer.OrdinalIgnoreCase.Equals(sectionName, SolutionItems) ? SolutionItems :
            sectionName;
    }
}
