// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.PackageManagement.UI.ViewModels;
using NuGet.PackageManagement.VisualStudio;
using NuGet.ProjectManagement;
using NuGet.Resolver;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// The NuGet package management UI
    /// </summary>
    /// <remarks>This is not expected to be thread safe.</remarks>
    public interface INuGetUI : IDisposable
    {
        bool PromptForPackageManagementFormat(PackageManagementFormat selectedFormat);

        bool ShowNuGetUpgradeWindow(NuGetProjectUpgradeWindowModel nuGetProjectUpgradeWindowModel);

        Task UpgradeProjectsToPackageReferenceAsync(IEnumerable<IProjectContextInfo> msBuildProjects);

        Task<bool> WarnAboutDotnetDeprecationAsync(IEnumerable<IProjectContextInfo> projects, CancellationToken cancellationToken);

        bool PromptForLicenseAcceptance(IEnumerable<PackageLicenseInfo> packages);

        void LaunchExternalLink(Uri url);

        void LaunchNuGetOptionsDialog(OptionsPage optionsPageToOpen);
        void LaunchNuGetOptionsDialog(PackageSourceMappingActionViewModel packageSourceMappingActionViewModel);

        /// <summary>
        /// Displays the preview window with options to accept or cancel
        /// </summary>
        bool PromptForPreviewAcceptance(IEnumerable<PreviewResult> actions);

        /// <summary>
        /// Marks the beginning of NuGet operation
        /// </summary>
        void BeginOperation();

        /// <summary>
        /// Marks the ending of NuGet operation
        /// </summary>
        void EndOperation();

        /// <summary>
        /// Common operations
        /// </summary>
        ICommonOperations CommonOperations { get; }

        /// <summary>
        /// Shared UI context
        /// </summary>
        INuGetUIContext UIContext { get; }

        INuGetUILogger UILogger { get; }

        /// <summary>
        /// A project context used for NuGet operations
        /// </summary>
        INuGetProjectContext ProjectContext { get; }

        /// <summary>
        /// Target projects
        /// </summary>
        IEnumerable<IProjectContextInfo> Projects { get; }

        /// <summary>
        /// True if the option to preview actions first is checked
        /// </summary>
        bool DisplayPreviewWindow { get; }

        /// <summary>
        /// True if the option to ignore the deprecated framework window is unchecked
        /// </summary>
        bool DisplayDeprecatedFrameworkWindow { get; }

        /// <summary>
        /// Package currently selected in the UI
        /// </summary>
        string SelectedPackageId { get; set; }

        /// <summary>
        /// Reports that an error has occurred.
        /// </summary>
        void ShowError(Exception ex);

        /// <summary>
        /// File conflict option
        /// </summary>
        FileConflictAction FileConflictAction { get; }

        PackageSourceMoniker ActivePackageSourceMoniker { get; }

        bool RemoveDependencies { get; }

        bool ForceRemove { get; }

        DependencyBehavior DependencyBehavior { get; }

        Configuration.ISettings Settings { get; }
    }
}
