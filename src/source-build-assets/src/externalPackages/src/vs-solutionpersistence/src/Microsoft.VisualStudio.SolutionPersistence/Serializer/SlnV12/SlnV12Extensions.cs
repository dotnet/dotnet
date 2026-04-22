// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Microsoft.VisualStudio.SolutionPersistence.Model;
using Microsoft.VisualStudio.SolutionPersistence.Utilities;

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.SlnV12;

/// <summary>
/// Extension methods for model to make it easier to get SlnV12 properties from model.
/// </summary>
public static class SlnV12Extensions
{
    private const string ActiveCfgSuffix = ".ActiveCfg";
    private const string BuildSuffix = ".Build.0";
    private const string DeploySuffix = ".Deploy.0";

    private enum ConfigLineType
    {
        Unknown = 0,
        ActiveCfg = 1,
        Build = 2,
        Deploy = 3,
    }

    /// <summary>
    /// Automatically converts special SlnV12 property bags into their equivalent model concepts.
    /// This handles property tables that used to represent configurations, solution folder, files and dependencies.
    /// If the properties are not special types, they will be added as regular property bags.
    /// </summary>
    /// <param name="solutionItem">A solution item.</param>
    /// <param name="properties">The properties to add to the model.</param>
    /// <returns>True if the properties were successfully added to the model.</returns>
    public static bool AddSlnProperties(this SolutionItemModel solutionItem, SolutionPropertyBag? properties)
    {
        Argument.ThrowIfNull(solutionItem, nameof(solutionItem));
        if (properties is null)
        {
            return true;
        }

        switch (SectionName.InternKnownSectionName(properties.Id))
        {
            case SectionName.SolutionItems when solutionItem is SolutionFolderModel folder:
                foreach (string fileName in properties.PropertyNames)
                {
                    folder.AddFile(PathExtensions.ConvertBackslashToModel(fileName));
                }

                return true;
            case SectionName.ProjectDependencies when solutionItem is SolutionProjectModel project:
                foreach (string dependencyProjectId in properties.PropertyNames)
                {
                    if (Guid.TryParse(dependencyProjectId, out Guid dependencyProjectGuid) &&
                        (project.Solution.FindItemById(dependencyProjectGuid) is SolutionProjectModel dependency))
                    {
                        project.AddDependency(dependency);
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            default:
                solutionItem.AddProperties(properties.Id, properties.Scope).AddRange(properties);
                return true;
        }
    }

    /// <summary>
    /// Creates solution property bags (or "sections") that used to exist in the .sln file.
    /// These properties were used to store solution files and project dependencies.
    /// These are now represented symantically in the model.
    /// This can be useful for code that used to handle parsing .sln files manually.
    /// </summary>
    /// <param name="solutionItem">The solution item.</param>
    /// <returns>All the solution property bags that are used in solution files.</returns>
    public static IEnumerable<SolutionPropertyBag> GetSlnProperties(this SolutionItemModel solutionItem)
    {
        Argument.ThrowIfNull(solutionItem, nameof(solutionItem));

        ListBuilderStruct<SolutionPropertyBag> slnProperties = new ListBuilderStruct<SolutionPropertyBag>((solutionItem.Properties?.Count ?? 0) + 1);

        IReadOnlyList<SolutionProjectModel>? dependencies = (solutionItem as SolutionProjectModel)?.Dependencies;
        if (!dependencies.IsNullOrEmpty())
        {
            SolutionPropertyBag propertyBag = new SolutionPropertyBag(SectionName.ProjectDependencies, PropertiesScope.PostLoad, dependencies.Count);
            foreach (SolutionProjectModel dependency in dependencies)
            {
                string dependencyProjectId = dependency.Id.ToSlnString();
                propertyBag.Add(dependencyProjectId, dependencyProjectId);
            }

            slnProperties.Add(propertyBag);
        }

        IReadOnlyList<string>? files = (solutionItem as SolutionFolderModel)?.Files;
        if (!files.IsNullOrEmpty())
        {
            SolutionPropertyBag propertyBag = new SolutionPropertyBag(SectionName.SolutionItems, PropertiesScope.PreLoad, files.Count);
            foreach (string file in files)
            {
                string persistenceFile = PathExtensions.ConvertModelToBackslashPath(file);
                propertyBag.Add(persistenceFile, persistenceFile);
            }

            slnProperties.Add(propertyBag);
        }

        foreach (SolutionPropertyBag propertyBag in solutionItem.Properties.GetStructEnumerable())
        {
            if (SectionName.InternKnownSectionName(propertyBag.Id) is
                not SectionName.ProjectDependencies and
                not SectionName.SolutionItems)
            {
                slnProperties.Add(propertyBag);
            }
        }

        return slnProperties.ToArray();
    }

    /// <summary>
    /// Automatically converts special SlnV12 property bags into their equivalent model concepts.
    /// This handles property tables that used to represent configurations, solution folder, files and dependencies.
    /// If the properties are not special types, they will be added as regular property bags.
    /// </summary>
    /// <param name="solution">A solution.</param>
    /// <param name="properties">The properties to add to the model.</param>
    /// <returns>True if the properties were successfully added to the model.</returns>
    public static bool AddSlnProperties(this SolutionModel solution, SolutionPropertyBag? properties)
    {
        Argument.ThrowIfNull(solution, nameof(solution));
        if (properties is null)
        {
            return true;
        }

        switch (SectionName.InternKnownSectionName(properties.Id))
        {
            case SectionName.SolutionConfigurationPlatforms:
                foreach (string slnConfiguration in properties.PropertyNames)
                {
                    // For some reason the description was stored in this property table.
                    if (StringComparer.OrdinalIgnoreCase.Equals(slnConfiguration, SlnConstants.Description))
                    {
                        solution.Description = properties[slnConfiguration];
                        continue;
                    }

                    if (ModelHelper.TrySplitFullConfiguration(solution.StringTable, slnConfiguration, out string? buildType, out string? platform))
                    {
                        solution.AddBuildType(buildType);
                        solution.AddPlatform(platform);
                    }
                }

                return true;

            case SectionName.ProjectConfigurationPlatforms:
                SetProjectConfigurationPlatforms(solution, properties);
                return true;

            case SectionName.NestedProjects:
                foreach ((string childProjectIdStr, string parentProjectIdStr) in properties)
                {
                    if (Guid.TryParse(childProjectIdStr, out Guid childProjectId) &&
                        Guid.TryParse(parentProjectIdStr, out Guid parentProjectId))
                    {
                        SolutionItemModel? childModel = solution.FindItemById(childProjectId);
                        SolutionFolderModel? parentFolder = solution.FindItemById(parentProjectId) as SolutionFolderModel;
                        if (childModel is not null && parentFolder is not null)
                        {
                            childModel.MoveToFolder(parentFolder);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            case SectionName.SolutionProperties:
                if (properties.TryGetValue(SlnConstants.HideSolutionNode, out string? hideSolutionNodeStr) &&
                    bool.TryParse(hideSolutionNodeStr, out bool hideSolutionNode))
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    solution.VisualStudioProperties.HideSolutionNode = hideSolutionNode;
#pragma warning restore CS0618 // Type or member is obsolete

                    if (properties.Count != 1)
                    {
                        properties.Remove(SlnConstants.HideSolutionNode);
                        SolutionPropertyBag solutionProperties = solution.AddProperties(SectionName.SolutionProperties, properties.Scope);
                        solutionProperties.AddRange(properties);
                    }
                }

                return true;
            case SectionName.ExtensibilityGlobals:
                if (properties.TryGetValue(SlnConstants.SolutionGuid, out string? solutionGuidStr) &&
                    Guid.TryParse(solutionGuidStr, out Guid solutionId))
                {
                    solution.VisualStudioProperties.SolutionId = solutionId;

                    if (properties.Count == 1)
                    {
                        return true;
                    }
                    else
                    {
                        properties.Remove(SlnConstants.SolutionGuid);
                        solution.AddProperties(SectionName.ExtensibilityGlobals, properties.Scope).AddRange(properties);
                    }
                }

                return true;
            default:
                solution.AddProperties(properties.Id, properties.Scope).AddRange(properties);
                return true;
        }

        // Handles reading the .sln file configuration mappings and
        // applying them to the model's project configurations.
        static void SetProjectConfigurationPlatforms(SolutionModel solution, SolutionPropertyBag properties)
        {
            StringTable stringTable = solution.StringTable;

            if (properties.Count > 0)
            {
                // Set the default configurations for a .sln file.
                foreach (SolutionProjectModel project in solution.SolutionProjects)
                {
                    ConfigurationRuleFollower projectTypeRules = solution.ProjectTypeTable.GetProjectConfigurationRules(project, excludeProjectSpecificRules: true);
                    if (!(projectTypeRules.GetIsBuildable() ?? true))
                    {
                        continue;
                    }

                    foreach (string buildType in solution.BuildTypes)
                    {
                        foreach (string platform in solution.Platforms)
                        {
                            // Add missing entries for each configuration, so we can detect if any were missing from the .sln file.
                            project.AddProjectConfigurationRule(new ConfigurationRule(BuildDimension.BuildType, buildType, platform, BuildTypeNames.Missing));
                            project.AddProjectConfigurationRule(new ConfigurationRule(BuildDimension.Platform, buildType, platform, PlatformNames.Missing));

                            // In the old .sln file the default configuration is not to build unless there is a build line.
                            // This rule will get overwritten by the build line if it exists.
                            project.AddProjectConfigurationRule(new ConfigurationRule(BuildDimension.Build, buildType, platform, bool.FalseString));
                        }
                    }
                }

                // Converts the .sln style project configuration platforms into a mappings for each configuration.
                foreach ((string projectKey, string projectValue) in properties)
                {
                    ParseProjectConfigLine(solution, projectKey, projectValue);
                }
            }

            // Applies a .SLN configuration line to the current project configuration.
            // This converts each line into un-optimal config rules for each project, these
            // rules can then be distilled into a more optimal set of rules.
            void ParseProjectConfigLine(SolutionModel solutionModel, string name, string value)
            {
                /*
                 * The configurations lines have this format:
                 * {ProjectId}.SolutionBuildType|SolutionPlatform.ConfigLineType = ProjectBuildType|ProjectPlatform
                 * {190CE348-596E-435A-9E5B-12A689F9FC29}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
                 * {190CE348-596E-435A-9E5B-12A689F9FC29}.Debug|Any CPU.Build.0 = Debug|Any CPU
                */

                int firstDot = name.IndexOf('.');
                if (firstDot < 0)
                {
                    return;
                }

#if NETFRAMEWORK
                Guid projectId = Guid.TryParse(name.Substring(0, firstDot), out Guid id) ? id : Guid.Empty;
#else
                Guid projectId = Guid.TryParse(name.AsSpan(0, firstDot), out Guid id) ? id : Guid.Empty;
#endif

                if (projectId == Guid.Empty || solutionModel.FindItemById(projectId) is not SolutionProjectModel projectModel)
                {
                    return;
                }

                ConfigLineType lineType =
                    name.EndsWith(ActiveCfgSuffix) ? ConfigLineType.ActiveCfg :
                    name.EndsWith(BuildSuffix) ? ConfigLineType.Build :
                    name.EndsWith(DeploySuffix) ? ConfigLineType.Deploy :
                    ConfigLineType.Unknown;

                if (lineType == ConfigLineType.Unknown)
                {
                    return;
                }

                int slnCfgEnd = name.Length - lineType switch
                {
                    ConfigLineType.ActiveCfg => ActiveCfgSuffix.Length,
                    ConfigLineType.Build => BuildSuffix.Length,
                    ConfigLineType.Deploy => DeploySuffix.Length,
                    _ => throw new InvalidOperationException(),
                };

                firstDot++;
                if (firstDot >= slnCfgEnd)
                {
                    return;
                }

                string slnCfg = name.Substring(firstDot, slnCfgEnd - firstDot);

                if (!ModelHelper.TrySplitFullConfiguration(stringTable, slnCfg, out string? solutionBuildType, out string? solutionPlatform))
                {
                    return;
                }

                switch (lineType)
                {
                    case ConfigLineType.ActiveCfg:
                        if (ModelHelper.TrySplitFullConfiguration(stringTable, value, out string? projectBuildType, out string? projectPlatform))
                        {
                            projectModel.AddProjectConfigurationRule(new ConfigurationRule(BuildDimension.BuildType, solutionBuildType, solutionPlatform, projectBuildType));
                            projectModel.AddProjectConfigurationRule(new ConfigurationRule(BuildDimension.Platform, solutionBuildType, solutionPlatform, projectPlatform));
                        }

                        break;
                    case ConfigLineType.Build:
                        projectModel.AddProjectConfigurationRule(new ConfigurationRule(BuildDimension.Build, solutionBuildType, solutionPlatform, bool.TrueString));
                        break;
                    case ConfigLineType.Deploy:
                        projectModel.AddProjectConfigurationRule(new ConfigurationRule(BuildDimension.Deploy, solutionBuildType, solutionPlatform, bool.TrueString));
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Creates solution property bags (or "sections") that used to exist in the .sln file.
    /// These properties were used to store configurations, solution folders, and global properties.
    /// These are now represented symantically in the model.
    /// This can be useful for code that used to handle parsing .sln files manually.
    /// </summary>
    /// <param name="solution">The solution.</param>
    /// <returns>All the solution property bags that are used in solution files.</returns>
    public static IEnumerable<SolutionPropertyBag> GetSlnProperties(this SolutionModel solution)
    {
        Argument.ThrowIfNull(solution, nameof(solution));
        List<SolutionPropertyBag> slnProperties = new List<SolutionPropertyBag>((solution.Properties?.Count ?? 0) + 1);

        slnProperties.AddIfNotNull(GetSolutionConfigurationPlatforms(solution));
        slnProperties.AddIfNotNull(GetProjectConfigurationPlatforms(solution));
        slnProperties.AddIfNotNull(GetSolutionProperties(solution));
        slnProperties.AddIfNotNull(GetNestedProjects(solution));
        slnProperties.AddIfNotNull(GetExtensibilityGlobals(solution));

        foreach (SolutionPropertyBag propertyBag in solution.Properties.GetStructEnumerable())
        {
            if (SectionName.InternKnownSectionName(propertyBag.Id) is
                not SectionName.SolutionConfigurationPlatforms and
                not SectionName.ProjectConfigurationPlatforms and
                not SectionName.SolutionProperties and
                not SectionName.NestedProjects and
                not SectionName.ExtensibilityGlobals and
                not SectionName.VisualStudio)
            {
                slnProperties.Add(propertyBag);
            }
        }

        return slnProperties;

        // All solution configurations
        static SolutionPropertyBag? GetSolutionConfigurationPlatforms(SolutionModel model)
        {
            if (model.Platforms.Count == 0 && model.BuildTypes.Count == 0)
            {
                return null;
            }

            int size = model.Platforms.Count * model.BuildTypes.Count;
            if (!model.Description.IsNullOrEmpty())
            {
                size++;
            }

            SolutionPropertyBag propertyBag = new SolutionPropertyBag(
                SectionName.SolutionConfigurationPlatforms,
                PropertiesScope.PreLoad,
                capacity: size);

            foreach (string buildType in model.BuildTypes)
            {
                foreach (string platform in model.Platforms)
                {
                    string slnConfiguration = $"{buildType}|{platform}";
                    propertyBag.Add(slnConfiguration, slnConfiguration);
                }
            }

            if (!model.Description.IsNullOrEmpty())
            {
                propertyBag.Add(SlnConstants.Description, model.Description);
            }

            return propertyBag;
        }

        // All solution to project configuration mappings and build mappings
        static SolutionPropertyBag? GetProjectConfigurationPlatforms(SolutionModel model)
        {
            if (model.Platforms.Count == 0 && model.BuildTypes.Count == 0)
            {
                return null;
            }

            SolutionConfigurationMap cfgMap = new SolutionConfigurationMap(model);
            (string SlnKey, SolutionConfigurationMap.SolutionConfigIndex Index)[] indexer = cfgMap.CreateMatrixAnnotation();

            int size = indexer.Length * model.SolutionProjects.Count * 3;
            SolutionPropertyBag propertyBag = new SolutionPropertyBag(SectionName.ProjectConfigurationPlatforms, PropertiesScope.PostLoad, size);

            foreach (SolutionProjectModel projectModel in model.SolutionProjects)
            {
                // Gets the mapping of solution to project configurations
                cfgMap.GetProjectConfigMap(projectModel, out SolutionConfigurationMap.SolutionToProjectMappings prjSlnCfgInfo, out bool writeConfigurations);
                if (!writeConfigurations)
                {
                    continue;
                }

                string projectId = projectModel.Id.ToSlnString();

                for (int i = 0; i < indexer.Length; i++)
                {
                    ref (string SlnKey, SolutionConfigurationMap.SolutionConfigIndex Index) entry = ref indexer[i];
                    ProjectConfigMapping mapping = prjSlnCfgInfo[entry.Index];
                    if (!mapping.IsValidBuildType || !mapping.IsValidPlatform)
                    {
                        continue;
                    }

                    bool isMissing = mapping.BuildType == BuildTypeNames.Missing || mapping.Platform == PlatformNames.Missing;

                    // Default project mapping in SLN was to use "Any CPU"
                    string platform = mapping.Platform;
                    if (platform == PlatformNames.AnyCPU)
                    {
                        platform = PlatformNames.AnySpaceCPU;
                    }

                    string prjCfgPlatString = $"{mapping.BuildType}|{platform}";

                    if (!isMissing)
                    {
                        WriteProperty(propertyBag, projectId, entry.SlnKey, ActiveCfgSuffix, prjCfgPlatString);
                    }

                    if (mapping.Build)
                    {
                        WriteProperty(propertyBag, projectId, entry.SlnKey, BuildSuffix, prjCfgPlatString);
                    }

                    if (mapping.Deploy)
                    {
                        WriteProperty(propertyBag, projectId, entry.SlnKey, DeploySuffix, prjCfgPlatString);
                    }
                }
            }

            return propertyBag;

            static void WriteProperty(SolutionPropertyBag propertyBag, string projectId, string slnCfg, string name, string value) =>
                propertyBag.Add(projectId + '.' + slnCfg + name, value);
        }

        // HideSolutionNode property
        static SolutionPropertyBag GetSolutionProperties(SolutionModel solution)
        {
            SolutionPropertyBag? additionalProperties = ModelHelper.FindByItemRef(solution.Properties, SectionName.SolutionProperties);
            SolutionPropertyBag propertyBag = new SolutionPropertyBag(SectionName.SolutionProperties, PropertiesScope.PreLoad, 1 + additionalProperties?.Count ?? 0)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                { SlnConstants.HideSolutionNode, solution.VisualStudioProperties.HideSolutionNode.GetValueOrDefault(false) ? "TRUE" : "FALSE" },
#pragma warning restore CS0618 // Type or member is obsolete
            };

            if (additionalProperties is not null)
            {
                foreach ((string propertyName, string value) in additionalProperties)
                {
                    propertyBag.Add(propertyName, value);
                }
            }

            return propertyBag;
        }

        // Project parents to nest projects under solution folders.
        static SolutionPropertyBag? GetNestedProjects(SolutionModel solution)
        {
            if (!AnyNestedProjects(solution))
            {
                return null;
            }

            int count = solution.SolutionItems.Count(static x => x.Parent is not null);

            SolutionPropertyBag propertyBag = new SolutionPropertyBag(SectionName.NestedProjects, PropertiesScope.PreLoad, count);
            foreach (SolutionItemModel item in solution.SolutionItems)
            {
                if (item.Parent is not null)
                {
                    propertyBag.Add(item.Id.ToSlnString(), item.Parent.Id.ToSlnString());
                }
            }

            return propertyBag;

            static bool AnyNestedProjects(SolutionModel model) =>
                model.SolutionItems.Any(static item => item.Parent is not null);
        }

        static SolutionPropertyBag? GetExtensibilityGlobals(SolutionModel model)
        {
            SolutionPropertyBag? additionalProperties = ModelHelper.FindByItemRef(model.Properties, SectionName.ExtensibilityGlobals);

            if (model.VisualStudioProperties.SolutionId is null)
            {
                return additionalProperties;
            }

            SolutionPropertyBag propertyBag = new SolutionPropertyBag(SectionName.ExtensibilityGlobals, PropertiesScope.PostLoad, 1 + additionalProperties?.Count ?? 0)
            {
                { SlnConstants.SolutionGuid, (model.VisualStudioProperties.SolutionId ?? Guid.NewGuid()).ToSlnString() },
            };

            if (additionalProperties is not null)
            {
                propertyBag.AddRange(additionalProperties);
            }

            return propertyBag;
        }
    }

    /// <summary>
    /// Always adds a solution folder to the solution.
    /// </summary>
    /// <remarks>
    /// This method is used for internal purposes. Use <see cref="SolutionModel.AddFolder(string)"/> instead.
    /// </remarks>
    /// <param name="solution">The solution.</param>
    /// <param name="name">The name of the new solution folder.</param>
    /// <returns>The model for the new folder.</returns>
    [Obsolete("This method is used for internal purposes, use AddFolder() instead.")]
    public static SolutionFolderModel CreateSlnFolder(this SolutionModel solution, string name)
    {
        Argument.ThrowIfNull(solution, nameof(solution));
        return solution.CreateFolder(name);
    }

    /// <summary>
    /// Adds a project to the solution.
    /// </summary>
    /// <remarks>
    /// This method is used for internal purposes. Use <see cref="SolutionModel.AddProject(string, string?, SolutionFolderModel?)"/> instead.
    /// </remarks>
    /// <param name="solution">The solution.</param>
    /// <param name="filePath">The relative path to the project.</param>
    /// <param name="projectTypeId">The project type id of the project.</param>
    /// <param name="folder">The parent solution folder to add the project to.</param>
    /// <returns>The model for the new project.</returns>
    [Obsolete("This method is used for internal purposes, use SolutionModel.AddProject() instead.")]
    public static SolutionProjectModel AddSlnProject(this SolutionModel solution, string filePath, Guid projectTypeId, SolutionFolderModel? folder)
    {
        Argument.ThrowIfNull(solution, nameof(solution));
        solution.ValidateInModel(folder);

        string extension = PathExtensions.GetExtension(filePath).ToString();
        string projectTypeName = solution.ProjectTypeTable.GetConciseType(projectTypeId, string.Empty, extension);
        return solution.AddProject(filePath, projectTypeName, projectTypeId, folder);
    }

    /// <summary>
    /// Suspends project validation while adding multiple projects without
    /// solution folder information.
    /// This must be called in a using block to properly resume validation.
    /// </summary>
    /// <remarks>
    /// This method is used for internal purposes.
    /// </remarks>
    /// <param name="solution">The solution.</param>
    /// <returns>Use to scope suspension, call <see cref="IDisposable.Dispose"/> to reenable validation.</returns>
    [Obsolete("This method is used for internal purposes.")]
    public static IDisposable SuspendProjectValidation(this SolutionModel solution)
    {
        Argument.ThrowIfNull(solution, nameof(solution));
        return solution.SuspendProjectValidation();
    }

#if NETFRAMEWORK

    internal static Version? TryParseVSVersion(string? strVersion)
    {
        return strVersion is null ? null : TryParseVSVersion(strVersion.AsSpan());
    }

#endif

    /// <summary>
    /// Parses the version formats allowed in .sln files.
    /// </summary>
    /// <returns>Returns null if the version could not be parsed.</returns>
    internal static Version? TryParseVSVersion(StringSpan strVersion)
    {
        strVersion = strVersion.Trim();
        if (strVersion.IsEmpty)
        {
            return null;
        }

        if (strVersion[0] == 'v' || strVersion[0] == 'V')
        {
            strVersion = strVersion.Slice(1);
        }

        int indexOfSpace = strVersion.IndexOf(' ');
        if (indexOfSpace >= 0)
        {
            strVersion = strVersion.Slice(0, indexOfSpace);
        }

        // Version.TryParse requires a major and minor version. (e.g. 16.0)
        // The old native logic allowed for just the major version.
        if (!strVersion.Contains('.'))
        {
            strVersion = StringExtensions.Concat(strVersion, ".0".AsSpan()).AsSpan();
        }

        do
        {
            if (Version.TryParse(strVersion.ToString(), out Version? version))
            {
                return version;
            }

            // If failed, just trim off extra stuff and try again.
            int lastIndexOfDot = strVersion.LastIndexOf('.');
            if (lastIndexOfDot >= 0)
            {
                strVersion = strVersion.Slice(0, lastIndexOfDot);
            }
        }
        while (strVersion.Contains('.'));

        return null;
    }
}
