// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.SolutionPersistence.Utilities;

namespace Microsoft.VisualStudio.SolutionPersistence.Model;

/// <summary>
/// Represents a project in the solution model.
/// </summary>
public sealed class SolutionProjectModel : SolutionItemModel
{
    private Guid typeId;
    private string type;
    private string filePath;
    private List<SolutionProjectModel>? dependencies;
    private List<ConfigurationRule>? projectConfigurationRules;

    [SetsRequiredMembers]
    internal SolutionProjectModel(SolutionModel solutionModel, string filePath, Guid typeId, string type, SolutionFolderModel? parent)
        : base(solutionModel, parent)
    {
        this.typeId = typeId;
        this.type = type;
        this.FilePath = filePath;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SolutionProjectModel"/> class.
    /// Copy constructor.
    /// </summary>
    /// <param name="solutionModel">The new solution model parent.</param>
    /// <param name="projectModel">The project model to copy.</param>
    internal SolutionProjectModel(SolutionModel solutionModel, SolutionProjectModel projectModel)
        : base(solutionModel, projectModel)
    {
        this.typeId = projectModel.TypeId;
        this.type = projectModel.Type;
        this.FilePath = projectModel.FilePath;
        this.DisplayName = projectModel.DisplayName;
        if (projectModel.dependencies is not null)
        {
            this.dependencies = [.. projectModel.dependencies];
        }

        if (projectModel.projectConfigurationRules is not null)
        {
            this.projectConfigurationRules = [.. projectModel.projectConfigurationRules];
        }
    }

    /// <inheritdoc/>
    public override Guid TypeId => this.typeId;

    /// <summary>
    /// Gets or sets the project type.
    /// This can be empty if the project file extension is known.
    /// This can be a type name of a defined project type.
    /// This can be a project type id (Guid).
    /// </summary>
    public string Type
    {
        get => this.type;

        set
        {
            // Attempt to resolve the type name,
            if (Guid.TryParse(value, out Guid typeId))
            {
                if (typeId == Guid.Empty)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                // Type looks like a project type id and try to lookup the type name.
                this.typeId = typeId;
                this.type = this.Solution.ProjectTypeTable.GetConciseType(this.typeId, string.Empty, this.Extension);
            }
            else
            {
                // Type looks like a name, lookup the project type id and simplify name if possible.
                this.typeId = this.Solution.ProjectTypeTable.GetProjectTypeId(value, this.Extension.AsSpan()) ?? Guid.Empty;
                this.type = this.Solution.ProjectTypeTable.GetConciseType(this.typeId, value, this.Extension);
            }
        }
    }

    /// <summary>
    /// Gets or sets the path to the project file.
    /// </summary>
    public string FilePath
    {
        get => this.filePath;

        [MemberNotNull(nameof(filePath), nameof(Extension))]
        set
        {
            if (!StringComparer.OrdinalIgnoreCase.Equals(this.filePath, value) || this.Extension is null)
            {
                if (this.Solution.FindProject(value) is not null)
                {
                    throw new SolutionArgumentException(string.Format(Errors.DuplicateItemRef_Args2, value, "Project"), nameof(value), SolutionErrorType.DuplicateItemRef);
                }

                string oldPath = this.filePath!;
                string oldExtension = this.Extension!;
                try
                {
                    this.filePath = value;
                    this.Extension = this.Solution.StringTable.GetString(PathExtensions.GetExtension(value));
                    this.OnItemRefChanged();

                    this.Solution.ValidateProjectName(this);
                }
                catch (Exception)
                {
                    this.filePath = oldPath;
                    this.Extension = oldExtension;
                    throw;
                }
            }
        }
    }

    /// <summary>
    /// Gets the file extension of the project file.
    /// </summary>
    /// <remarks>
    /// Some project types, like web site projects, do not have a file extension.
    /// </remarks>
    public string Extension { get; private set; }

    /// <summary>
    /// Gets or sets the display name of the project.
    /// </summary>
    /// <remarks>
    /// This will be ignored if the project path is a file name.
    /// </remarks>
    public string? DisplayName { get; set; }

    /// <inheritdoc/>
    public override string ActualDisplayName
    {
        get
        {
            // If the project has a file name, use that as the display name.
            // This historically takes precedence over the DisplayName property.
            StringSpan fileName = PathExtensions.GetStandardDisplayName(this.FilePath.AsSpan());
            if (fileName.IsEmpty)
            {
                return this.DisplayName ?? string.Empty;
            }

            return this.Solution.StringTable.GetString(fileName);
        }
    }

    /// <summary>
    /// Gets the list of the dependencies of this project.
    /// </summary>
    /// <remarks>
    /// Project to project dependencies are normally stored in the project file itself,
    /// this is used for solution level dependencies.
    /// </remarks>
    public IReadOnlyList<SolutionProjectModel>? Dependencies => this.dependencies;

    /// <summary>
    /// Gets or sets a list of configuration rules for this project.
    /// These rules can be simplified to essential rules by calling <see cref="SolutionModel.DistillProjectConfigurations"/>.
    /// </summary>
    public IReadOnlyList<ConfigurationRule>? ProjectConfigurationRules
    {
        get => this.projectConfigurationRules;
        set => this.projectConfigurationRules = value is null ? null : [.. value];
    }

    /// <inheritdoc/>
    internal override string ItemRef => this.FilePath;

    /// <summary>
    /// Gets the project configuration for the given solution configuration.
    /// </summary>
    /// <param name="solutionBuildType">The solution build type. (e.g. Debug).</param>
    /// <param name="solutionPlatform">The solution platform. (e.g. x64).</param>
    /// <returns>
    /// The project configuration for the given solution configuration.
    /// BuildType and Platform will be null if the configuration information is missing.
    /// </returns>
    public (string? BuildType, string? Platform, bool Build, bool Deploy) GetProjectConfiguration(string solutionBuildType, string solutionPlatform)
    {
        ConfigurationRuleFollower projectTypeRules = this.Solution.ProjectTypeTable.GetProjectConfigurationRules(this);

        string? buildType = MissingToNull(projectTypeRules.GetProjectBuildType(solutionBuildType, solutionPlatform) ?? solutionBuildType);
        string? platform = MissingToNull(projectTypeRules.GetProjectPlatform(solutionBuildType, solutionPlatform) ?? solutionPlatform);
        bool build = projectTypeRules.GetIsBuildable(solutionBuildType, solutionPlatform) ?? true;
        bool deploy = projectTypeRules.GetIsDeployable(solutionBuildType, solutionPlatform) ?? false;

        return (buildType, platform, build, deploy);

        static string? MissingToNull(string value) => value == BuildTypeNames.Missing ? null : value;
    }

    /// <summary>
    /// Adds a dependency to this project.
    /// </summary>
    /// <param name="dependency">The dependency to add.</param>
    public void AddDependency(SolutionProjectModel dependency)
    {
        Argument.ThrowIfNull(dependency, nameof(dependency));
        this.Solution.ValidateInModel(dependency);

        if (ReferenceEquals(dependency, this))
        {
            throw new SolutionArgumentException(string.Format(Errors.InvalidLoop_Args1, dependency.ItemRef), nameof(dependency), SolutionErrorType.InvalidLoop);
        }

        this.dependencies ??= [];

        if (!this.dependencies.Contains(dependency))
        {
            this.dependencies.Add(dependency);
        }
    }

    /// <summary>
    /// Removes a dependency from this project.
    /// </summary>
    /// <param name="dependency">The dependency to remove.</param>
    /// <returns><see langword="true"/> if the dependency was found and removed.</returns>
    public bool RemoveDependency(SolutionProjectModel dependency)
    {
        Argument.ThrowIfNull(dependency, nameof(dependency));
        this.Solution.ValidateInModel(dependency);

        return
            this.dependencies is not null &&
            this.dependencies.Remove(dependency);
    }

    /// <summary>
    /// Adds a configuration rule to this project.
    /// </summary>
    /// <param name="rule">The rule to add.</param>
    public void AddProjectConfigurationRule(ConfigurationRule rule)
    {
        Argument.ThrowIfNull(rule, nameof(rule));
        this.projectConfigurationRules ??= [];
        this.projectConfigurationRules.Add(rule);
    }

    private protected override Guid GetDefaultId()
    {
        return DefaultIdGenerator.CreateIdFrom(this.FilePath);
    }
}
