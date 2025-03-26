// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml;

namespace Microsoft.VisualStudio.SolutionPersistence.Model;

/// <summary>
/// Represents a solution.
/// This contains a list of projects and folders and the information
/// required to build the solution in different configurations.
/// </summary>
public sealed class SolutionModel : PropertyContainerModel
{
#if NETFRAMEWORK
    private const string InvalidNameChars = @"?:\/*""<>|";
#else
    private static readonly SearchValues<char> InvalidNameChars = SearchValues.Create(@"?:\/*""<>|");
#endif

    private readonly VisualStudioProperties visualStudioProperties;
    private readonly Dictionary<Guid, SolutionItemModel> solutionItemsById;
    private readonly List<SolutionItemModel> solutionItems;
    private readonly List<SolutionProjectModel> solutionProjects;
    private readonly List<SolutionFolderModel> solutionFolders;
    private readonly List<string> solutionBuildTypes;
    private readonly List<string> solutionPlatforms;
    private readonly List<ProjectType> projectTypes;
    private ProjectTypeTable? projectTypeTable;
    private bool suspendProjectValidation;

    /// <summary>
    /// Initializes a new instance of the <see cref="SolutionModel"/> class.
    /// Creates a new empty solution.
    /// </summary>
    public SolutionModel()
    {
        this.visualStudioProperties = new VisualStudioProperties(this);
        this.StringTable = new StringTable().WithSolutionConstants();
        this.solutionItemsById = [];
        this.solutionItems = [];
        this.solutionProjects = [];
        this.solutionFolders = [];
        this.solutionBuildTypes = [];
        this.solutionPlatforms = [];
        this.projectTypes = [];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SolutionModel"/> class.
    /// Creates a deep copy of the solution.
    /// </summary>
    /// <param name="solutionModel">Instance of the <see cref="SolutionModel"/> to copy.</param>
    public SolutionModel(SolutionModel solutionModel)
        : base(solutionModel ?? throw new ArgumentNullException(nameof(solutionModel)))
    {
        this.visualStudioProperties = new VisualStudioProperties(this);
        this.StringTable = solutionModel.StringTable;
        int itemCount = solutionModel.solutionItems.Count;
        int folderCount = solutionModel.solutionItems.Count(x => x is SolutionFolderModel);
        this.solutionItems = new List<SolutionItemModel>(itemCount);
        this.solutionItemsById = new Dictionary<Guid, SolutionItemModel>(itemCount);
        this.solutionFolders = new List<SolutionFolderModel>(folderCount);
        this.solutionProjects = new List<SolutionProjectModel>(itemCount - folderCount);
        foreach (SolutionItemModel item in solutionModel.solutionItems)
        {
            SolutionItemModel newItem = item switch
            {
                SolutionFolderModel folder => new SolutionFolderModel(this, folder),
                SolutionProjectModel project => new SolutionProjectModel(this, project),
                _ => throw new InvalidOperationException(),
            };

            this.solutionItems.Add(newItem);
            this.solutionFolders.AddIfNotNull(newItem as SolutionFolderModel);
            this.solutionProjects.AddIfNotNull(newItem as SolutionProjectModel);
            this.solutionItemsById[newItem.Id] = newItem;
        }

        // Replace the shallow-parent models with the new folders.
        foreach (SolutionItemModel item in this.solutionItems)
        {
            if (item.Parent is not null)
            {
                item.MoveToFolder(this.FindFolder(item.Parent.ItemRef) ?? throw new InvalidOperationException());
            }
        }

        this.Description = solutionModel.Description;
        this.solutionBuildTypes = [.. solutionModel.solutionBuildTypes];
        this.solutionPlatforms = [.. solutionModel.solutionPlatforms];
        this.projectTypes = [.. solutionModel.projectTypes];
    }

    /// <summary>
    /// Gets or sets the string table used by the solution model.
    /// This is used to reduce string duplication.
    /// </summary>
    public StringTable StringTable { get; set; }

    /// <summary>
    /// Gets or sets the serializer extension model that can be used to
    /// get or specify settings specific to a serializer.
    /// This can be created by a serializer.
    /// </summary>
    public ISerializerModelExtension? SerializerExtension { get; set; }

    /// <summary>
    /// Gets or sets a user visible comment describing the solution.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets the list of solution items in the solution.
    /// This is all of the solution folders and projects in the solution.
    /// </summary>
    public IReadOnlyList<SolutionItemModel> SolutionItems => this.solutionItems;

    /// <summary>
    /// Gets the list of projects in the solution.
    /// </summary>
    public IReadOnlyList<SolutionProjectModel> SolutionProjects => this.solutionProjects;

    /// <summary>
    /// Gets the list of solution folders in the solution.
    /// </summary>
    public IReadOnlyList<SolutionFolderModel> SolutionFolders => this.solutionFolders;

    /// <summary>
    /// Gets the list of build types in the solution. (e.g Debug/Release).
    /// </summary>
    public IReadOnlyList<string> BuildTypes => this.solutionBuildTypes;

    /// <summary>
    /// Gets the list of platforms in the solution. (e.g. x64/Any CPU).
    /// </summary>
    public IReadOnlyList<string> Platforms => this.solutionPlatforms;

    /// <summary>
    /// Gets or sets the list of project types in the solution.
    /// </summary>
    /// <remarks>
    /// These can be defined to provide information about a project type used in the solution.
    /// It can be associated with a file extension or a friendly name.
    /// It contains the project type id and and default configuration mapping rules.
    /// </remarks>
    public IReadOnlyList<ProjectType> ProjectTypes
    {
        get => this.projectTypes;
        set
        {
            this.projectTypes.Clear();
            this.projectTypes.AddRange(value);
            this.projectTypeTable = null;
        }
    }

    /// <summary>
    /// Gets a helper to get and set Visual Studio specific properties.
    /// </summary>
    /// <returns>A helper to get and set Visual Studio properties.</returns>
    public ref readonly VisualStudioProperties VisualStudioProperties => ref this.visualStudioProperties;

    internal ProjectTypeTable ProjectTypeTable => this.projectTypeTable ??= new ProjectTypeTable(this.projectTypes);

    /// <summary>
    /// Gets or adds a solution folder to the solution.
    /// </summary>
    /// <param name="path">
    /// The full path of the solution folder. The path must start and end with a forward slash, with subfolders separated by forward slashes.
    /// Folders will be created as needed.
    /// </param>
    /// <returns>The model for the new folder.</returns>
    public SolutionFolderModel AddFolder(string path)
    {
        Argument.ThrowIfNullOrEmpty(path, nameof(path));
        if (!path.StartsWith('/') || !path.EndsWith('/'))
        {
            throw new SolutionArgumentException(string.Format(Errors.InvalidFolderPath_Args1, path), nameof(path), SolutionErrorType.InvalidFolderPath);
        }

        SolutionFolderModel? existingFolder = this.FindFolder(path);
        if (existingFolder is not null)
        {
            return existingFolder;
        }

        // Process the folder name
        StringSpan folderPath = path.AsSpan(0, path.Length - 1);

        int lastSlash = folderPath.LastIndexOf('/');
        string? parentItemRef = lastSlash > 0 ? folderPath.Slice(0, lastSlash + 1).ToString() : null;
        StringSpan newName = lastSlash > 0 ? folderPath.Slice(lastSlash + 1) : folderPath.Slice(1);

        SolutionFolderModel folder = this.AddFolder(newName, parentItemRef);

        // Ensure the project type is in the project type table, if it is not already.
        this.solutionItemsById[folder.Id] = folder;

        return folder;
    }

    /// <summary>
    /// Adds a project to the solution.
    /// </summary>
    /// <param name="filePath">The relative path to the project.</param>
    /// <param name="projectTypeName">The project type name of the project.
    /// This can be null if the project type can be determined from the project's file extension.
    /// </param>
    /// <param name="folder">The parent solution folder to add the project to.</param>
    /// <returns>The model for the new project.</returns>
    public SolutionProjectModel AddProject(string filePath, string? projectTypeName = null, SolutionFolderModel? folder = null)
    {
        Argument.ThrowIfNullOrEmpty(filePath, nameof(filePath));
        this.ValidateInModel(folder);

        Guid projectTypeId =
            Guid.TryParse(projectTypeName, out Guid projectTypeGuid) ? projectTypeGuid :
            this.ProjectTypeTable.GetProjectTypeId(projectTypeName, Path.GetExtension(filePath.AsSpan())) ??
            throw new SolutionArgumentException(string.Format(Errors.InvalidProjectTypeReference_Args1, projectTypeName), nameof(projectTypeName), SolutionErrorType.InvalidProjectTypeReference);

        return this.AddProject(filePath, projectTypeName ?? string.Empty, projectTypeId, folder);
    }

    /// <summary>
    /// Remove a solution folder from the solution model. This includes any child folders and projects.
    /// </summary>
    /// <param name="folder">The folder to remove.</param>
    /// <returns><see langword="true"/> if the folder was found and removed.</returns>
    public bool RemoveFolder(SolutionFolderModel folder)
    {
        Argument.ThrowIfNull(folder, nameof(folder));
        this.ValidateInModel(folder);

        return this.RemoveFolder(folder, this.SolutionItems.ToArray());
    }

    /// <summary>
    /// Remove a project from the solution model.
    /// </summary>
    /// <param name="project">The item to remove.</param>
    /// <returns><see langword="true"/> if the project was found and removed.</returns>
    public bool RemoveProject(SolutionProjectModel project)
    {
        Argument.ThrowIfNull(project, nameof(project));
        this.ValidateInModel(project);
        _ = this.solutionProjects.Remove(project);

        // Remove any dependencies to this project.
        foreach (SolutionProjectModel existingProject in this.SolutionProjects)
        {
            _ = existingProject.RemoveDependency(project);
        }

        return this.RemoveItem(project);
    }

    /// <summary>
    /// Adds a build type to the solution.
    /// </summary>
    /// <param name="buildType">The build type to add.</param>
    public void AddBuildType(string buildType)
    {
        Argument.ThrowIfNullOrEmpty(buildType, nameof(buildType));

        ValidateName(buildType.AsSpan());

        if (!this.solutionBuildTypes.Contains(buildType, StringComparer.OrdinalIgnoreCase))
        {
            buildType = this.StringTable.GetString(buildType);
            this.solutionBuildTypes.Add(buildType);
        }
    }

    /// <summary>
    /// Removes a build type from the solution.
    /// </summary>
    /// <param name="buildType">The build type to remove.</param>
    /// <returns><see langword="true"/> if the build type was found and removed.</returns>
    public bool RemoveBuildType(string buildType)
    {
        Argument.ThrowIfNullOrEmpty(buildType, nameof(buildType));
        return this.solutionBuildTypes.Remove(buildType);
    }

    /// <summary>
    /// Adds a platform to the solution.
    /// </summary>
    /// <param name="platform">The platform to add.</param>
    public void AddPlatform(string platform)
    {
        Argument.ThrowIfNullOrEmpty(platform, nameof(platform));

        ValidateName(platform.AsSpan());

        if (!this.solutionPlatforms.Contains(platform, StringComparer.OrdinalIgnoreCase))
        {
            platform = this.StringTable.GetString(platform);
            this.solutionPlatforms.Add(platform);
        }
    }

    /// <summary>
    /// Removes a platform from the solution.
    /// </summary>
    /// <param name="platform">The platform to remove.</param>
    /// <returns><see langword="true"/> if the platform was found and removed.</returns>
    public bool RemovePlatform(string platform)
    {
        Argument.ThrowIfNullOrEmpty(platform, nameof(platform));
        return this.solutionPlatforms.Remove(platform);
    }

    /// <summary>
    /// Find a solution folder or project by id.
    /// </summary>
    /// <param name="id">The id of the item to look for.</param>
    /// <returns>The item if found.</returns>
    public SolutionItemModel? FindItemById(Guid id)
    {
        return this.solutionItemsById.TryGetValue(id, out SolutionItemModel? item) ? item : null;
    }

    /// <summary>
    /// Find a solution folder by unique path.
    /// </summary>
    /// <param name="path">The folder path to look for.</param>
    /// <returns>The folder if found.</returns>
    public SolutionFolderModel? FindFolder(string path)
    {
        Argument.ThrowIfNullOrEmpty(path, nameof(path));
        if (!path.StartsWith('/') || !path.EndsWith('/'))
        {
            throw new SolutionArgumentException(string.Format(Errors.InvalidFolderPath_Args1, path), nameof(path), SolutionErrorType.InvalidFolderPath);
        }

        return ModelHelper.FindByItemRef(this.solutionFolders, path);
    }

    /// <summary>
    /// Find a solution project by path.
    /// </summary>
    /// <param name="path">The project path to look for.</param>
    /// <returns>The project if found.</returns>
    public SolutionProjectModel? FindProject(string path)
    {
        Argument.ThrowIfNullOrEmpty(path, nameof(path));

        return ModelHelper.FindByItemRef(this.solutionProjects, path);
    }

    /// <summary>
    /// Regenerates all of the project configuration rules. If rules are added
    /// to project types, or possible redundant rules are added to projects this
    /// can be called to recalculate the rules.
    /// </summary>
    public void DistillProjectConfigurations()
    {
        SolutionConfigurationMap cfgMap = new SolutionConfigurationMap(this);

        // Load all of the current rules for the project and recalculate a new
        // set of configuration rules.
        cfgMap.DistillProjectConfigurations();
    }

    // Throws if the solution folder or project name is not valid.
    internal static void ValidateName(StringSpan name)
    {
        if (name.IsEmpty || name.IsWhiteSpace())
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (name.Length > 260)
        {
            throw new ArgumentOutOfRangeException(nameof(name));
        }

        foreach (char c in name)
        {
            if (char.IsControl(c) || InvalidNameChars.Contains(c))
            {
                throw new SolutionArgumentException(Errors.InvalidName, nameof(name), SolutionErrorType.InvalidName);
            }
        }

        if (IsDosWord(name))
        {
            throw new SolutionArgumentException(Errors.InvalidName, nameof(name), SolutionErrorType.InvalidName);
        }

        static bool IsDosWord(scoped StringSpan name)
        {
            if (name is "." or "..")
            {
                return true;
            }

            // Only care about part before extension
            name = Path.GetFileNameWithoutExtension(name);
            switch (name.Length)
            {
                case 3:
                    return
                        name.EqualsOrdinalIgnoreCase("nul") ||
                        name.EqualsOrdinalIgnoreCase("con") ||
                        name.EqualsOrdinalIgnoreCase("aux") ||
                        name.EqualsOrdinalIgnoreCase("prn");
                case 4:
                    // disallow com? and lpt? where ? can be any number from 1 to 9
                    name = name.TrimEnd("123456789".AsSpan());
                    return name.EqualsOrdinalIgnoreCase("com") || name.EqualsOrdinalIgnoreCase("lpt");
                case 6:
                    return name.EqualsOrdinalIgnoreCase("clock$");
                default:
                    return false;
            }
        }
    }

    /// <summary>
    /// Remove any unneccessary VS properties from the model.
    /// This removes project and solution guid ids plus any properties removed by <see cref="RemoveObsoleteProperties"/>.
    /// </summary>
    internal void TrimVisualStudioProperties()
    {
        // Set project id to default.
        foreach (SolutionItemModel item in this.SolutionItems)
        {
            item.Id = Guid.Empty;
        }

        this.VisualStudioProperties.SolutionId = null;
        this.visualStudioProperties.OpenWith = null;

        this.RemoveObsoleteProperties();
    }

    /// <summary>
    /// Remove any obsolete VS properties from the model.
    /// This removes minimum version older than Dev17, shared project properties, and
    /// removes any CPS project types ids that were accidentally used in .sln files.
    /// </summary>
    internal void RemoveObsoleteProperties()
    {
        // Remove CPS project type ids.
        // This explicitly checks for the built-in CPS type names, so a slnx file can still
        // use the CPS project ids by creating a custom ProjectType.
        foreach (SolutionProjectModel project in this.SolutionProjects)
        {
            // Remove CPS project type that were used by .sln for many years due to a bug.
            if (StringComparer.OrdinalIgnoreCase.Equals(project.Type, "Common C#"))
            {
                project.Type = "C#";
            }
            else if (StringComparer.OrdinalIgnoreCase.Equals(project.Type, "Common VB"))
            {
                project.Type = "VB";
            }
            else if (StringComparer.OrdinalIgnoreCase.Equals(project.Type, "Common F#"))
            {
                project.Type = "F#";
            }
        }

        _ = this.RemoveProperties(Serializer.SlnV12.SectionName.SharedMSBuildProjectFiles);

        VisualStudioProperties vsProperties = this.VisualStudioProperties;
        vsProperties.Version = null;
#pragma warning disable CS0618 // Type or member is obsolete
        vsProperties.HideSolutionNode = null;
#pragma warning restore CS0618 // Type or member is obsolete

        if (vsProperties.MinimumVersion is not null &&
            vsProperties.MinimumVersion < new Version(18, 0))
        {
            vsProperties.MinimumVersion = null;
        }
    }

    internal SolutionProjectModel AddProject(string filePath, string projectTypeName, Guid projectTypeId, SolutionFolderModel? folder)
    {
        SolutionProjectModel project = new SolutionProjectModel(this, filePath, projectTypeId, projectTypeName, folder);

        // Project is already in the solution.
        if (this.FindProject(project.FilePath) is not null)
        {
            throw new SolutionArgumentException(string.Format(Errors.DuplicateProjectPath_Arg1, project.ItemRef), nameof(filePath), SolutionErrorType.DuplicateProjectPath);
        }

        this.ValidateProjectName(project);

        this.solutionProjects.Add(project);
        this.solutionItems.Add(project);

        // Ensure the project type is in the project type table, if it is not already.
        this.solutionItemsById[project.Id] = project;

        return project;
    }

    /// <summary>
    /// Always adds a solution folder to the solution.
    /// </summary>
    /// <param name="name">The name of the new solution folder.</param>
    /// <returns>The model for the new folder.</returns>
    internal SolutionFolderModel CreateFolder(string name)
    {
        Argument.ThrowIfNullOrEmpty(name, nameof(name));

        // Validate the name.
        ValidateName(name.AsSpan());

        return this.AddFolder(name.AsSpan(), parentItemRef: null);
    }

    /// <summary>
    /// Suspends project validation while adding multiple projects without
    /// solution folder information.
    /// This must be called in a using block to properly resume validation.
    /// </summary>
    /// <returns>Use to scope suspension, call <see cref="IDisposable.Dispose"/> to reenable validation.</returns>
    internal IDisposable SuspendProjectValidation()
    {
        this.suspendProjectValidation = true;
        return new ValidationScope(this);
    }

    internal void ResumeProjectValidation()
    {
        this.suspendProjectValidation = false;
        foreach (SolutionProjectModel project in this.solutionProjects)
        {
            this.ValidateProjectName(project);
        }
    }

    internal void ThrowIfProjectValidationSuspended()
    {
        if (this.suspendProjectValidation)
        {
            throw new InvalidOperationException();
        }
    }

    internal bool IsConfigurationImplicit()
    {
        return
            this.IsBuildTypeImplicit() &&
            this.IsPlatformImplicit() &&
            this.ProjectTypeTable.ProjectTypes.Count == 0;
    }

    internal bool IsBuildTypeImplicit()
    {
        // Has 0 build types, or just Debug/Release.
        return
            this.BuildTypes.Count == 0 ||
            (this.BuildTypes.Count == 2 &&
            this.BuildTypes.Contains(BuildTypeNames.Debug) &&
            this.BuildTypes.Contains(BuildTypeNames.Release));
    }

    internal bool IsPlatformImplicit()
    {
        return
            this.Platforms.Count == 0 ||
            (this.Platforms.Count == 1 &&
            this.Platforms[0] == PlatformNames.AnySpaceCPU);
    }

    internal void OnUpdateId(SolutionItemModel solutionItemModel, Guid? oldId)
    {
        if (oldId is not null)
        {
            _ = this.solutionItemsById.Remove(oldId.Value);
        }

        this.solutionItemsById[solutionItemModel.Id] = solutionItemModel;
    }

    internal void ValidateProjectName(SolutionProjectModel project)
    {
        if (this.suspendProjectValidation)
        {
            return;
        }

        string displayName = project.ActualDisplayName;
        foreach (SolutionProjectModel existingProject in this.SolutionProjects)
        {
            if (!ReferenceEquals(existingProject.Parent, project.Parent) || ReferenceEquals(existingProject, project))
            {
                continue;
            }

            if (existingProject.ActualDisplayName.Equals(displayName, StringComparison.OrdinalIgnoreCase))
            {
                throw new SolutionArgumentException(string.Format(Errors.DuplicateProjectName_Arg1, displayName), SolutionErrorType.DuplicateProjectName);
            }
        }
    }

    internal void ValidateInModel(SolutionItemModel? item)
    {
        if (item is not null && item.Solution != this)
        {
            throw new SolutionArgumentException(Errors.InvalidModelItem, nameof(item), SolutionErrorType.InvalidModelItem);
        }
    }

    // Creates a new solution folder. Assumes name has been validated and deduplicated.
    private SolutionFolderModel AddFolder(StringSpan name, string? parentItemRef)
    {
        // Validate the name before creating any parent nodes.
        ValidateName(name);

        SolutionFolderModel? parentFolder =
            parentItemRef is null ? null : this.FindFolder(parentItemRef) ?? this.AddFolder(parentItemRef);

        SolutionFolderModel folder = new SolutionFolderModel(this, this.StringTable.GetString(name), parentFolder);

        this.solutionFolders.Add(folder);
        this.solutionItems.Add(folder);

        return folder;
    }

    // Remove a solution folder from the solution model. This includes any child folders and projects.
    // Recursive call reuses the solutionItems array to avoid creating a new array for each recursive call.
    private bool RemoveFolder(SolutionFolderModel folder, SolutionItemModel[] solutionItems)
    {
        _ = this.solutionFolders.Remove(folder);

        // Remove any children of this folder.
        foreach (SolutionItemModel existingItem in solutionItems)
        {
            if (ReferenceEquals(existingItem.Parent, folder))
            {
                _ = existingItem switch
                {
                    SolutionFolderModel childFolder => this.RemoveFolder(childFolder, solutionItems),
                    SolutionProjectModel childProject => this.RemoveProject(childProject),
                    _ => throw new InvalidOperationException(),
                };
            }
        }

        return this.RemoveItem(folder);
    }

    private bool RemoveItem(SolutionItemModel item)
    {
        _ = this.solutionItemsById.Remove(item.Id);
        return this.solutionItems.Remove(item);
    }

    private sealed class ValidationScope(SolutionModel model) : IDisposable
    {
        public void Dispose()
        {
            model.ResumeProjectValidation();
        }
    }
}
