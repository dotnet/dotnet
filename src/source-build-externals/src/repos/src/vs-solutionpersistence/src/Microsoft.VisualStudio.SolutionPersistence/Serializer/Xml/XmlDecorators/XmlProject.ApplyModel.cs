// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.SolutionPersistence.Model;

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml.XmlDecorators;

internal sealed partial class XmlProject
{
    // Update the Xml DOM with changes from the model.
    internal bool ApplyModelToXml(SolutionProjectModel modelProject)
    {
        bool modified = false;

        // Attributes
        string type = this.Root.ProjectTypes.GetConciseType(modelProject);
        if (!StringComparer.Ordinal.Equals(this.Type, type))
        {
            this.Type = type.NullIfEmpty();
            modified = true;
        }

        string? displayName =
            modelProject.DisplayName is null || StringExtensions.EqualsOrdinal(this.DefaultDisplayName, modelProject.ActualDisplayName) ?
            null :
            modelProject.DisplayName;
        if (!StringComparer.Ordinal.Equals(this.DisplayName, displayName))
        {
            this.DisplayName = displayName;
            modified = true;
        }

        Guid id = modelProject.IsDefaultId ? Guid.Empty : modelProject.Id;
        if (this.Id != id)
        {
            this.Id = id;
            modified = true;
        }

        // BuildDependencies
        modified |= this.ApplyModelItemsToXml(
            itemRefs: modelProject.Dependencies?.ToList(dependencyProject => this.Root.ConvertToUserPath(dependencyProject.FilePath)),
            decoratorItems: ref this.buildDependencies,
            decoratorElementName: Keyword.BuildDependency);

        // Configurations
        modified |= this.configurationRules.ApplyModelToXml(this, modelProject.ProjectConfigurationRules);

        // Properties
        modified |= this.ApplyModelToXml(modelProject.Properties);

        return modified;
    }
}
