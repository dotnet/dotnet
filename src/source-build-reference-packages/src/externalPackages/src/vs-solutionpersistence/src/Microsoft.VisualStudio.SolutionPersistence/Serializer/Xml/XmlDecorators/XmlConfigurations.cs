// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;
using Microsoft.VisualStudio.SolutionPersistence.Model;

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml.XmlDecorators;

/// <summary>
/// Child to a Solution that represents a collection of configurations.
/// </summary>
internal sealed class XmlConfigurations(SlnxFile root, XmlElement element) :
    XmlContainer(root, element, Keyword.Configurations),
    IItemRefDecorator
{
    private ItemRefList<XmlBuildType> buildType = new ItemRefList<XmlBuildType>(ignoreCase: true);
    private ItemRefList<XmlPlatform> platforms = new ItemRefList<XmlPlatform>(ignoreCase: true);
    private ItemRefList<XmlProjectType> projectTypes = new ItemRefList<XmlProjectType>();

    public Keyword ItemRefAttribute => Keyword.Configurations;

    private protected override bool AllowEmptyItemRef => true;

    private protected override string RawItemRef
    {
        get => string.Empty;
        set { }
    }

    /// <inheritdoc/>
    internal override XmlDecorator? ChildDecoratorFactory(XmlElement element, Keyword elementName)
    {
        return elementName switch
        {
            Keyword.Platform => new XmlPlatform(this.Root, element),
            Keyword.BuildType => new XmlBuildType(this.Root, element),
            Keyword.ProjectType => new XmlProjectType(this.Root, element),
            _ => base.ChildDecoratorFactory(element, elementName),
        };
    }

    /// <inheritdoc/>
    internal override void OnNewChildDecoratorAdded(XmlDecorator childDecorator)
    {
        switch (childDecorator)
        {
            case XmlPlatform platform:
                this.platforms.Add(platform);
                break;
            case XmlBuildType buildType:
                this.buildType.Add(buildType);
                break;
            case XmlProjectType projectType:
                this.projectTypes.Add(projectType);
                break;
        }

        base.OnNewChildDecoratorAdded(childDecorator);
    }

    /// <inheritdoc/>
    internal override XmlDecorator? FindNextDecorator<TDecorator>()
    {
        return typeof(TDecorator).Name switch
        {
            nameof(XmlBuildType) => this.platforms.FirstOrDefault() ?? this.FindNextDecorator<XmlPlatform>(),
            nameof(XmlPlatform) => this.projectTypes.FirstOrDefault(),
            nameof(XmlProjectType) => null,
            _ => null,
        };
    }

    #region Deserialize model

    internal void AddToModel(SolutionModel solution)
    {
        foreach (XmlPlatform platform in this.platforms.GetItems())
        {
            try
            {
                solution.AddPlatform(PlatformNames.ToStringKnown(platform.Name));
            }
            catch (Exception ex) when (SolutionException.ShouldWrap(ex))
            {
                throw SolutionException.Create(ex, platform);
            }
        }

        foreach (XmlBuildType buildType in this.buildType.GetItems())
        {
            try
            {
                solution.AddBuildType(BuildTypeNames.ToStringKnown(buildType.Name));
            }
            catch (Exception ex) when (SolutionException.ShouldWrap(ex))
            {
                throw SolutionException.Create(ex, buildType);
            }
        }
    }

    /// <summary>
    /// Create a project type table from the declared project types in this solution.
    /// </summary>
    internal ProjectTypeTable? GetProjectTypeTable()
    {
        List<ProjectType> declaredTypes = new List<ProjectType>(this.projectTypes.ItemsCount);
        foreach (XmlProjectType projectType in this.projectTypes.GetItems())
        {
            declaredTypes.Add(projectType.ToModel());
        }

        try
        {
            return declaredTypes.Count > 0 ?
                new ProjectTypeTable(declaredTypes) :
                null;
        }
        catch (SolutionException ex)
        {
            throw SolutionException.Create(ex, this);
        }
    }

    #endregion

    // Update the Xml DOM with changes from the model.
    internal bool ApplyModelToXml(SolutionModel modelSolution)
    {
        bool modified = false;

        // BuildTypes
        modified |= this.ApplyModelItemsToXml(
            itemRefs: modelSolution.IsBuildTypeImplicit() ? null : modelSolution.BuildTypes,
            decoratorItems: ref this.buildType,
            decoratorElementName: Keyword.BuildType);

        // Platforms
        modified |= this.ApplyModelItemsToXml(
            itemRefs: modelSolution.IsPlatformImplicit() ? null : modelSolution.Platforms,
            decoratorItems: ref this.platforms,
            decoratorElementName: Keyword.Platform);

        // Project Types
        modified |= this.ApplyModelItemsToXml(
            modelItems: modelSolution.ProjectTypes.ToList(type => (ItemRef: XmlProjectType.GetItemRef(type.Name, type.Extension, type.ProjectTypeId), Item: type)),
            ref this.projectTypes,
            Keyword.ProjectType,
            applyModelToXml: static (newProjectTypes, newValue) => newProjectTypes.ApplyModelToXml(newValue));

        return modified;
    }
}
