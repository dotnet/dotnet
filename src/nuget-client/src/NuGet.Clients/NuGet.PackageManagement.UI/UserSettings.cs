// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel;
using NuGet.PackageManagement.VisualStudio;
using NuGet.ProjectManagement;
using NuGet.Resolver;

namespace NuGet.PackageManagement.UI
{
    [Serializable]
    public class UserSettings
    {
        /// <summary>
        /// Represents the user settings persisted in suo files.
        /// </summary>
        public UserSettings()
        {
            IncludePrerelease = RegistrySettingUtility.GetBooleanSetting(Constants.IncludePrereleaseRegistryName);
            ShowPreviewWindow = true;
            ShowDeprecatedFrameworkWindow = true;
            SelectedFilter = ItemFilter.Installed;
            DependencyBehavior = DependencyBehavior.Lowest;
            FileConflictAction = FileConflictAction.PromptUser;
            SelectedPackageMetadataTab = PackageMetadataTab.Readme;
            OptionsExpanded = false;
        }

        public string SourceRepository { get; set; }

        public bool ShowPreviewWindow { get; set; }

        public bool ShowDeprecatedFrameworkWindow { get; set; }

        public bool RemoveDependencies { get; set; }

        public bool ForceRemove { get; set; }

        public bool IncludePrerelease { get; set; }

        public ItemFilter SelectedFilter { get; set; }

        public PackageMetadataTab SelectedPackageMetadataTab { get; set; }

        public DependencyBehavior DependencyBehavior { get; set; }

        public FileConflictAction FileConflictAction { get; set; }

        public bool OptionsExpanded { get; set; }

        // The sort property of the project list in the solution package manager
        public string SortPropertyName { get; set; }

        // The sort direction of the project list in the solution package manager
        public ListSortDirection SortDirection { get; set; }
    }
}
