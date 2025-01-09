// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.SolutionPersistence.Model;

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml.XmlDecorators;

/// <summary>
/// Represents the root Solution XML element in the slnx file.
/// These methods are used to update the Xml DOM with changes from the model.
/// </summary>
internal sealed partial class XmlSolution
{
    // Update the Xml DOM with changes from the model.
    internal bool ApplyModelToXml(SolutionModel modelSolution)
    {
        bool modified = false;

        // Attributes
        string description = modelSolution.Description ?? string.Empty;
        if (!StringComparer.Ordinal.Equals(this.Description, description))
        {
            this.Description = description;
            modified = true;
        }

        // Configurations
        // Use the item ref logic to allow only a single "Configurations" element, and use string.Empty as the item ref.
        modified |= this.ApplyModelItemsToXml<SolutionModel, XmlConfigurations>(
            modelItems: modelSolution.IsConfigurationImplicit() ? null : [(ItemRef: string.Empty, Item: modelSolution)],
            ref this.configurationsSingle,
            Keyword.Configurations,
            applyModelToXml: static (newConfigs, newValue) => newConfigs.ApplyModelToXml(newValue));

        // Folders
        modified |= this.ApplyModelItemsToXml(
            modelItems: modelSolution.SolutionFolders.ToList(folder => (folder.ItemRef, Item: folder)),
            ref this.folders,
            Keyword.Folder,
            applyModelToXml: static (newFolder, newValue) => newFolder.ApplyModelToXml(newValue));

        // Projects
        List<(string ItemRef, SolutionProjectModel Item)> solutionProjects = modelSolution.SolutionProjects.WhereToList(
            (project, _) => project.Parent is null,
            (project, _) => (ItemRef: this.Root.ConvertToUserPath(project.ItemRef), Item: project),
            (object?)null);
        modified |= this.ApplyModelItemsToXml(
            modelItems: solutionProjects,
            ref this.rootProjects,
            Keyword.Project,
            applyModelToXml: static (newProject, newValue) => newProject.ApplyModelToXml(newValue));

        // Properties
        modified |= this.ApplyModelToXml(modelSolution.Properties);

        return modified;
    }
}
