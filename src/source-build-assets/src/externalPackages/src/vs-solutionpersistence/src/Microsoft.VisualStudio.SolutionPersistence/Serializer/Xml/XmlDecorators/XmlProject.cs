// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;
using Microsoft.VisualStudio.SolutionPersistence.Model;
using Microsoft.VisualStudio.SolutionPersistence.Utilities;

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml.XmlDecorators;

/// <summary>
/// Child of a Solution or Folder that represents a project in the solution.
/// </summary>
internal sealed partial class XmlProject(SlnxFile root, XmlFolder? xmlParentFolder, XmlElement element) :
    XmlContainerWithProperties(root, element, Keyword.Project),
    IItemRefDecorator
{
    private ItemRefList<XmlBuildDependency> buildDependencies = new ItemRefList<XmlBuildDependency>(ignoreCase: true);
    private ItemConfigurationRulesList configurationRules = new ItemConfigurationRulesList();

    public Keyword ItemRefAttribute => Keyword.Path;

    internal string Path => this.ItemRef;

    internal StringSpan DefaultDisplayName => PathExtensions.GetStandardDisplayName(PathExtensions.ConvertToModel(this.Path));

    internal Guid Id
    {
        get => this.GetXmlAttributeGuid(Keyword.Id);
        set => this.UpdateXmlAttributeGuid(Keyword.Id, value);
    }

    internal string? DisplayName
    {
        get => this.GetXmlAttribute(Keyword.DisplayName);
        set => this.UpdateXmlAttribute(Keyword.DisplayName, value);
    }

    internal string? Type
    {
        get => this.GetXmlAttribute(Keyword.Type);
        set => this.UpdateXmlAttribute(Keyword.Type, value);
    }

    internal XmlFolder? ParentFolder { get; } = xmlParentFolder;

    /// <inheritdoc/>
    internal override XmlDecorator? ChildDecoratorFactory(XmlElement element, Keyword elementName)
    {
        return elementName switch
        {
            Keyword.BuildDependency => new XmlBuildDependency(this.Root, element),
            Keyword.BuildType => new XmlConfigurationBuildType(this.Root, element),
            Keyword.Platform => new XmlConfigurationPlatform(this.Root, element),
            Keyword.Build => new XmlConfigurationBuild(this.Root, element),
            Keyword.Deploy => new XmlConfigurationDeploy(this.Root, element),
            _ => base.ChildDecoratorFactory(element, elementName),
        };
    }

    /// <inheritdoc/>
    internal override void OnNewChildDecoratorAdded(XmlDecorator childDecorator)
    {
        switch (childDecorator)
        {
            case XmlBuildDependency buildDependency:
                this.buildDependencies.Add(buildDependency);
                break;
            case XmlConfiguration configuration:
                this.configurationRules.Add(configuration);
                break;
        }

        base.OnNewChildDecoratorAdded(childDecorator);
    }

    /// <inheritdoc/>
    internal override XmlDecorator? FindNextDecorator<TDecorator>()
    {
        return typeof(TDecorator).Name switch
        {
            nameof(XmlBuildDependency) => this.configurationRules.FirstOrDefault() ?? this.FindNextDecorator<XmlConfiguration>(),
            nameof(XmlConfiguration) or nameof(XmlConfigurationBuildType) or nameof(XmlConfigurationPlatform) or nameof(XmlConfigurationBuild) or nameof(XmlConfigurationDeploy) =>
                this.configurationRules.FindNextDecorator<TDecorator>() ?? this.propertyBags.FirstOrDefault(),
            _ => null,
        };
    }

    #region Deserialize model

    internal SolutionProjectModel AddToModel(SolutionModel solution)
    {
        try
        {
            SolutionFolderModel? parentFolder = null;
            if (this.ParentFolder is not null)
            {
                SolutionFolderModel? foundParentFolder = solution.FindFolder(this.ParentFolder.ItemRef);
                if (foundParentFolder is not null)
                {
                    parentFolder = foundParentFolder;
                }
                else
                {
                    throw SolutionException.Create(string.Format(Errors.InvalidFolderReference_Args1, this.ParentFolder.Name), this, SolutionErrorType.InvalidFolderReference);
                }
            }

            SolutionProjectModel projectModel = solution.AddProject(
                filePath: PathExtensions.ConvertToModel(this.Path),
                projectTypeName: this.Type ?? string.Empty,
                folder: parentFolder);

            projectModel.Id = this.Id;
            projectModel.DisplayName = this.DisplayName;

            foreach (ConfigurationRule configurationRule in this.configurationRules.ToModel())
            {
                projectModel.AddProjectConfigurationRule(configurationRule);
            }

            foreach (XmlProperties properties in this.propertyBags.GetItems())
            {
                properties.AddToModel(projectModel);
            }

            this.Root.UserPaths[projectModel.FilePath] = this.Path;

            return projectModel;
        }
        catch (Exception ex) when (SolutionException.ShouldWrap(ex))
        {
            throw SolutionException.Create(ex, this);
        }
    }

    internal void AddDependenciesToModel(SolutionModel solution, SolutionProjectModel projectModel)
    {
        foreach (XmlBuildDependency buildDependency in this.buildDependencies.GetItems())
        {
            string dependencyItemRef = PathExtensions.ConvertToModel(buildDependency.Project);
            SolutionProjectModel? dependencyProject = solution.FindProject(dependencyItemRef);
            if (dependencyProject is not null)
            {
                try
                {
                    projectModel.AddDependency(dependencyProject);
                }
                catch (Exception ex) when (SolutionException.ShouldWrap(ex))
                {
                    throw SolutionException.Create(ex, buildDependency);
                }
            }
            else
            {
                throw SolutionException.Create(string.Format(Errors.InvalidProjectReference_Args1, dependencyItemRef), buildDependency, SolutionErrorType.InvalidProjectReference);
            }
        }
    }

    #endregion
}
