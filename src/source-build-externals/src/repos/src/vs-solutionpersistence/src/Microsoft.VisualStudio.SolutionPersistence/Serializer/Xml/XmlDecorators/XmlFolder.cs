// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;
using Microsoft.VisualStudio.SolutionPersistence.Model;
using Microsoft.VisualStudio.SolutionPersistence.Utilities;

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml.XmlDecorators;

/// <summary>
/// Child of a Solution that represents a solution folder.
/// </summary>
internal sealed class XmlFolder(SlnxFile root, XmlSolution xmlSolution, XmlElement element) :
    XmlContainerWithProperties(root, element, Keyword.Folder),
    IItemRefDecorator
{
    private readonly XmlSolution xmlSolution = xmlSolution;
    private ItemRefList<XmlFile> files = new ItemRefList<XmlFile>(ignoreCase: true);
    private ItemRefList<XmlProject> folderProjects = new ItemRefList<XmlProject>(ignoreCase: true);

    public Keyword ItemRefAttribute => Keyword.Name;

    internal string Name => this.ItemRef;

    internal Guid Id
    {
        get => this.GetXmlAttributeGuid(Keyword.Id);
        set => this.UpdateXmlAttributeGuid(Keyword.Id, value);
    }

#if DEBUG

    internal override string DebugDisplay => $"{base.DebugDisplay} FolderProjects={this.folderProjects} Files={this.files}";

#endif

    /// <inheritdoc/>
    internal override XmlDecorator? ChildDecoratorFactory(XmlElement element, Keyword elementName)
    {
        return elementName switch
        {
            // Forward project handling to the solution decorator.
            Keyword.Project => this.xmlSolution.CreateProjectDecorator(element, xmlParentFolder: this),
            Keyword.File => new XmlFile(this.Root, element),
            _ => base.ChildDecoratorFactory(element, elementName),
        };
    }

    /// <inheritdoc/>
    internal override void OnNewChildDecoratorAdded(XmlDecorator childDecorator)
    {
        switch (childDecorator)
        {
            case XmlFile file:
                this.files.Add(file);
                break;
            case XmlProject project:
                this.folderProjects.Add(project);
                break;
        }

        base.OnNewChildDecoratorAdded(childDecorator);
    }

    /// <inheritdoc/>
    internal override XmlDecorator? FindNextDecorator<TDecorator>()
    {
        return typeof(TDecorator).Name switch
        {
            nameof(XmlFile) => this.folderProjects.FirstOrDefault() ?? this.FindNextDecorator<XmlProject>(),
            nameof(XmlProject) => this.propertyBags.FirstOrDefault(),
            _ => null,
        };
    }

    #region Deserialize model

    internal void AddToModel(SolutionModel solutionModel, List<(XmlProject XmlProject, SolutionProjectModel ModelProject)> newProjects)
    {
        try
        {
            SolutionFolderModel folderModel = solutionModel.AddFolder(this.Name);
            folderModel.Id = this.Id;

            foreach (XmlFile file in this.files.GetItems())
            {
                string modelPath = PathExtensions.ConvertToModel(file.Path);
                folderModel.AddFile(modelPath);
                this.Root.UserPaths[modelPath] = file.Path;
            }

            foreach (XmlProperties properties in this.propertyBags.GetItems())
            {
                properties.AddToModel(folderModel);
            }

            foreach (XmlProject project in this.folderProjects.GetItems())
            {
                newProjects.Add((project, project.AddToModel(solutionModel)));
            }
        }
        catch (Exception ex) when (SolutionException.ShouldWrap(ex))
        {
            throw SolutionException.Create(ex, this);
        }
    }

    #endregion

    // Update the Xml DOM with changes from the model.
    internal bool ApplyModelToXml(SolutionFolderModel modelFolder)
    {
        SolutionModel modelSolution = modelFolder.Solution;
        bool modified = false;

        // Attributes
        Guid id = modelFolder.IsDefaultId ? Guid.Empty : modelFolder.Id;
        if (this.Id != id)
        {
            this.Id = id;
            modified = true;
        }

        // Files
        modified |= this.ApplyModelItemsToXml(
            itemRefs: modelFolder.Files?.ToList(this.Root.ConvertToUserPath),
            decoratorItems: ref this.files,
            decoratorElementName: Keyword.File);

        // Projects
        List<(string ItemRef, SolutionProjectModel Item)> projectsInFolder = modelSolution.SolutionProjects.WhereToList(
            (project, modelFolder) => ReferenceEquals(project.Parent, modelFolder),
            (project, modelFolder) => (ItemRef: this.Root.ConvertToUserPath(project.ItemRef), Item: project),
            modelFolder);
        modified |= this.ApplyModelItemsToXml(
            modelItems: projectsInFolder,
            ref this.folderProjects,
            Keyword.Project,
            applyModelToXml: static (newProject, modelProject) => newProject.ApplyModelToXml(modelProject));

        // Properties
        modified |= this.ApplyModelToXml(modelFolder.Properties);

        return modified;
    }
}
